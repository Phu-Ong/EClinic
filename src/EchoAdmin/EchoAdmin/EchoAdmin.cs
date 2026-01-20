using CrystRptManager;
using DBInteraction;
using SKCDExtCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Utility;


namespace EchoAdmin
{
    // Token: 0x02000003 RID: 3
    public partial class EchoAdmin : Form
    {
        // Crop percent cho video display và capture (10% mỗi phía)
        private float videoCropLeftPercent = EClinicConfig.CropLeft;
        private float videoCropRightPercent = EClinicConfig.CropRight;
        private float videoCropTopPercent = EClinicConfig.CropTop;
        private float videoCropBottomPercent = EClinicConfig.CropBottom;
        // Token: 0x0600000F RID: 15 RVA: 0x00003E44 File Offset: 0x00002E44
        public EchoAdmin()
        {
            this.InitializeComponent();
            this.comboBoxPhongSieuAm.SelectedValueChanged += this.comboBoxPhongSieuAm_SelectedValueChanged;
            this.tabControl1.SelectedIndexChanged += this.tabControl1_SelectedIndexChanged;
            this.dataGridViewExt1.SelectionChanged += this.dataGridViewExt1_SelectionChanged;

            // Apply crop trực tiếp trên video display (giữ nguyên kích thước design)
            ApplyVideoCrop();
        }

        // Crop trực tiếp trên video display của captureMovie1
        // Giữ nguyên kích thước design bằng cách dùng Panel wrapper
        // Panel giữ nguyên kích thước design, nhưng Region sẽ clip visual
        private void ApplyVideoCrop()
        {
            if (this.captureMovie1 == null) return;

            // Tính toán crop area cho cả 4 phía
            int leftCrop = (int)(this.captureMovie1.Width * this.videoCropLeftPercent);
            int rightCrop = (int)(this.captureMovie1.Width * this.videoCropRightPercent);
            int topCrop = (int)(this.captureMovie1.Height * this.videoCropTopPercent);
            int bottomCrop = (int)(this.captureMovie1.Height * this.videoCropBottomPercent);

            int cropWidth = this.captureMovie1.Width - leftCrop - rightCrop;
            int cropHeight = this.captureMovie1.Height - topCrop - bottomCrop;

            // Scale factor để crop area fill toàn bộ control (cả width và height)
            float scaleFactorX = (float)this.captureMovie1.Width / cropWidth;
            float scaleFactorY = (float)this.captureMovie1.Height / cropHeight;
            float scaleFactor = Math.Max(scaleFactorX, scaleFactorY); // Dùng max để đảm bảo fill toàn bộ

            // Tạo Panel wrapper để crop visual, nhưng Panel giữ nguyên kích thước design
            Control parent = this.captureMovie1.Parent;
            if (parent == null) return;

            // Lưu lại vị trí và kích thước gốc
            Point originalLocation = this.captureMovie1.Location;
            Size originalSize = this.captureMovie1.Size;

            Panel cropPanel = parent.Controls["cropPanel"] as Panel;
            if (cropPanel == null)
            {
                // Tạo Panel wrapper mới
                cropPanel = new Panel();
                cropPanel.Name = "cropPanel";
                cropPanel.BackColor = Color.Black;
                cropPanel.BorderStyle = BorderStyle.FixedSingle;

                // Panel có kích thước đầy đủ (giữ nguyên design)
                cropPanel.Location = originalLocation;
                cropPanel.Size = originalSize;

                // Thêm Panel vào parent (sau captureMovie1)
                int captureIndex = parent.Controls.GetChildIndex(this.captureMovie1);
                parent.Controls.Add(cropPanel);
                parent.Controls.SetChildIndex(cropPanel, captureIndex);

                // Di chuyển captureMovie1 vào Panel
                parent.Controls.Remove(this.captureMovie1);
                cropPanel.Controls.Add(this.captureMovie1);
            }

            // Scale captureMovie1 để crop area fill toàn bộ Panel
            // Scale width và height riêng biệt với scaleFactorX và scaleFactorY để đảm bảo crop đúng tỷ lệ cho cả 4 chiều
            int scaledWidth = (int)(originalSize.Width * scaleFactorX);
            int scaledHeight = (int)(originalSize.Height * scaleFactorY);
            
            // Tính toán Location để đảm bảo crop đúng tỷ lệ cho cả 4 chiều
            // Logic cho Y (top/bottom):
            // - scaleFactorY = originalSize.Height / cropHeight
            //   với cropHeight = originalSize.Height - topCrop - bottomCrop
            // - scaledHeight = originalSize.Height * scaleFactorY
            // - Crop area trong scaled image: từ Y = topCrop * scaleFactorY đến Y = scaledHeight - bottomCrop * scaleFactorY
            // - Crop area trong Panel: từ Y = 0 đến Y = originalSize.Height
            //
            // Để map crop area trong scaled image vào Panel:
            // - locationY + topCrop * scaleFactorY = 0
            // - locationY = -topCrop * scaleFactorY
            //
            // Logic cho X (left/right):
            // - scaleFactorX = originalSize.Width / cropWidth
            //   với cropWidth = originalSize.Width - leftCrop - rightCrop
            // - scaledWidth = originalSize.Width * scaleFactorX
            // - Crop area trong scaled image: từ X = leftCrop * scaleFactorX đến X = scaledWidth - rightCrop * scaleFactorX
            // - Crop area trong Panel: từ X = 0 đến X = originalSize.Width
            //
            // Để map crop area trong scaled image vào Panel:
            // - locationX + leftCrop * scaleFactorX = 0
            // - locationX = -leftCrop * scaleFactorX
            //
            // Công thức này đảm bảo cả 4 chiều (left, right, top, bottom) đều được crop đúng tỷ lệ
            float topCropScaled = topCrop * scaleFactorY;
            int locationY = -(int)topCropScaled;
            
            float leftCropScaled = leftCrop * scaleFactorX;
            int locationX = -(int)leftCropScaled;
            
            // Đặt captureMovie1 với negative margin để crop area fill Panel
            this.captureMovie1.Location = new Point(locationX, locationY);
            this.captureMovie1.Size = new Size(scaledWidth, scaledHeight);

            // Set Region trên Panel để crop visual (chỉ hiển thị phần giữa)
            // Panel vẫn có kích thước đầy đủ (giữ nguyên design), nhưng Region sẽ clip visual
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(0, 0, cropPanel.Width, cropPanel.Height));
            cropPanel.Region = new Region(path);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00003EEB File Offset: 0x00002EEB
        private void dataGridViewExt1_SelectionChanged(object sender, EventArgs e)
        {
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00003EF0 File Offset: 0x00002EF0
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool flag = this.tabControl1.SelectedTab.Name == "tabPageHinhAnh" && this.boolLoadedCapture;
            if (flag)
            {
                this.captureMovie1.EnablePreview = true;
                this.captureMovie1.ShowMovie();
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00003F44 File Offset: 0x00002F44
        private void EchoAdmin_Load(object sender, EventArgs e)
        {
            EClinicDB.LoadDatasetIntoComboBox(ref this.comboBoxBsSieuAm, "BacSiSieuAm", "", true);
            EClinicDB.LoadDatasetIntoComboBox(ref this.comboBoxPhongSieuAm, "PhongSieuAm", "", true);
            bool flag = EClinicConfig.AllowChangeRoom == "0";
            if (flag)
            {
                this.comboBoxPhongSieuAm.Enabled = false;
                int num = (int)EClinicDB.ExecuteScalar("select phongban_id from dm_phongban where maphongban = '" + EClinicConfig.ComputerNo + "'");
                this.isChangePhongSieuAm = true;
                this.comboBoxPhongSieuAm.SelectedValue = num;
            }
            else
            {
                this.comboBoxPhongSieuAm.Enabled = true;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00003FEC File Offset: 0x00002FEC
        private void SetTrangThaiNutLenh(EchoAdmin.ButtonStat butStat)
        {
            switch (butStat)
            {
                case EchoAdmin.ButtonStat.Progress:
                    this.buttonSaveKetQua.Enabled = true;
                    this.buttonKetQua.Enabled = true;
                    this.buttonInHinhAnh.Enabled = true;
                    this.buttonInKetQua.Enabled = false;
                    this.boolDangTienHanhSieuAm = true;
                    break;
                case EchoAdmin.ButtonStat.Edit:
                    this.buttonSaveKetQua.Enabled = true;
                    this.buttonKetQua.Enabled = true;
                    this.buttonInHinhAnh.Enabled = true;
                    break;
                case EchoAdmin.ButtonStat.Save:
                    this.boolDangTienHanhSieuAm = false;
                    this.buttonInKetQua.Enabled = true;
                    break;
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00004090 File Offset: 0x00003090
        private void buttonTienHanh_Click(object sender, EventArgs e)
        {
            bool flag = this.boolLoadedControlKetqua;
            if (flag)
            {
                this.LoadControlKetQuaSA("TQ1");
            }
            try
            {
                this.intCLSKetQuaChiTiet_Id = int.Parse(StringExt.StringZero(this.dataGridViewExt1.SelectedRows[0].Cells["YeuCauChiTiet_Id"].Value));
                bool flag2 = this.TaoMoiKetQuaSieuAm(this.intCLSKetQuaChiTiet_Id);
                if (flag2)
                {
                    this.boolLoadedCapture = true;
                    this.SetTrangThaiNutLenh(EchoAdmin.ButtonStat.Progress);
                    this.labelSoPhieuYeuCau.Text = this.dataGridViewExt1.SelectedRows[0].Cells["SoPhieuYeuCau"].Value.ToString();
                    EClinicConfig.BenhNhanID = (int)this.dataGridViewExt1.SelectedRows[0].Cells["BenhNhan_Id"].Value;
                    bool flag3 = this.LoadDuLieuBenhNhan(EClinicConfig.BenhNhanID);
                    if (flag3)
                    {
                        this.labelBsSieuAm.Text = this.comboBoxBsSieuAm.Text;
                        this.labelPhongBanThucHien.Text = this.comboBoxPhongSieuAm.Text;
                        this.labelDichVuSieuAm.Text = StringExt.StringNull(this.dataGridViewExt1.SelectedRows[0].Cells["TenDichVu"].Value);
                        this.intDichVu_Id = int.Parse(StringExt.StringZero(this.dataGridViewExt1.SelectedRows[0].Cells["DichVu_id"].Value));
                        bool flag4 = this.LoadMauKetQua(this.intDichVu_Id) > 0;
                        if (flag4)
                        {
                            this.buttonMacDinh.Enabled = true;
                        }
                    }
                    this.tabControl1.SelectedTab = this.tabPageHinhAnh;
                }
                else
                {
                    this.boolDangTienHanhSieuAm = true;
                }
                bool flag5 = this.boolDangTienHanhSieuAm;
                if (flag5)
                {
                    this.buttonTienHanh.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat(new object[]
                {
                    ex.Message,
                    "\n",
                    ex.Source,
                    "\n",
                    ex.InnerException
                }), "Lỗi tiến hành siêu âm.");
            }
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000042E8 File Offset: 0x000032E8
        private bool TaoMoiKetQuaSieuAm(int iSoPhieuYeuCau)
        {
            this.intClsKetqua = 0;
            return true;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00004304 File Offset: 0x00003304
        private void buttonCapnhatDataGrid_Click(object sender, EventArgs e)
        {
            this.CapNhatDataGridDsYeuCau();
            this.buttonSuaKetQua.Enabled = false;
            bool flag = this.dataGridViewExt1.RowCount > 0;
            if (flag)
            {
                this.buttonTienHanh.Enabled = true;
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00004348 File Offset: 0x00003348
        private void comboBoxPhongSieuAm_SelectedValueChanged(object sender, EventArgs e)
        {
            bool flag = this.isChangePhongSieuAm;
            if (flag)
            {
                this.buttonCapnhatDataGrid.PerformClick();
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x0000436C File Offset: 0x0000336C
        private void CapNhatDataGridDsYeuCau()
        {
            EClinicDB.LoadDatasetEncryptedIntoDataGridView(ref this.dataGridViewExt1, "dm_benhnhan", "YeuCauSieuAmChuaThucHien", "", StringExt.StringNull(this.comboBoxPhongSieuAm.SelectedValue));
            this.dataGridViewExt1.HeaderColumnState = DataGridViewExt.CustomHeaderState.HideColumn;
            this.dataGridViewExt1.CustomHeaderText = "SoPhieuYeuCau|Số phiếu YC|80; Ho_Ten|Họ tên Bệnh nhân|130; TenDichVu|Tên dịch vụ|120;DonGia|Đơn giá|50; MienGiam|Miễn giảm|50; TenPhongBan|Phòng ban thực hiện|80; NgayYeuCau|Ngày yêu cầu|110; TrangThai|Trạng thái|100";
        }

        // Helper method để crop ảnh
        private Bitmap CropImage(Bitmap originalImage, Rectangle cropArea)
        {
            if (cropArea.Width <= 0 || cropArea.Height <= 0)
                return originalImage;

            // Đảm bảo crop area không vượt quá kích thước ảnh
            cropArea.X = Math.Max(0, cropArea.X);
            cropArea.Y = Math.Max(0, cropArea.Y);
            cropArea.Width = Math.Min(cropArea.Width, originalImage.Width - cropArea.X);
            cropArea.Height = Math.Min(cropArea.Height, originalImage.Height - cropArea.Y);

            Bitmap cropped = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage,
                    new Rectangle(0, 0, cropArea.Width, cropArea.Height),
                    cropArea,
                    GraphicsUnit.Pixel);
            }
            return cropped;
        }

        // Crop cả 4 phía (left/right/top/bottom) để fit vào khung mà không méo
        private Bitmap CropLeftRightAndFit(Bitmap originalImage, Size targetSize,
            float leftCropPercent = 0.10f, float rightCropPercent = 0.10f,
            float topCropPercent = 0.10f, float bottomCropPercent = 0.10f)
        {
            // Bước 1: Crop cả 4 phía
            int leftCrop = (int)(originalImage.Width * leftCropPercent);
            int rightCrop = (int)(originalImage.Width * rightCropPercent);
            int topCrop = (int)(originalImage.Height * topCropPercent);
            int bottomCrop = (int)(originalImage.Height * bottomCropPercent);

            Rectangle cropArea = new Rectangle(
                leftCrop,
                topCrop,
                originalImage.Width - leftCrop - rightCrop,
                originalImage.Height - topCrop - bottomCrop
            );

            Bitmap cropped = CropImage(originalImage, cropArea);

            // Bước 2: Resize để fit vào targetSize mà không méo (giữ aspect ratio)
            Bitmap resized = ResizeImageKeepAspectRatio(cropped, targetSize);

            // Dispose cropped nếu khác với resized
            if (cropped != resized)
            {
                cropped.Dispose();
            }

            return resized;
        }

        // Resize ảnh giữ nguyên tỷ lệ để fit vào khung
        private Bitmap ResizeImageKeepAspectRatio(Bitmap originalImage, Size targetSize)
        {
            // Tính tỷ lệ scale để fit vào khung
            double ratioX = (double)targetSize.Width / originalImage.Width;
            double ratioY = (double)targetSize.Height / originalImage.Height;
            double ratio = Math.Min(ratioX, ratioY); // Dùng min để fit vừa khung

            int newWidth = (int)(originalImage.Width * ratio);
            int newHeight = (int)(originalImage.Height * ratio);

            Bitmap resized = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resized;
        }

        // Crop cả 4 phía tự động dựa trên kích thước pictureBox
        private Bitmap CropAndFitToPictureBox(Bitmap originalImage, PictureBox targetPictureBox)
        {
            Size targetSize = targetPictureBox.Size;
            return CropLeftRightAndFit(originalImage, targetSize
                , this.videoCropLeftPercent
                , this.videoCropRightPercent
                , this.videoCropTopPercent
                , this.videoCropBottomPercent);
        }


        // Token: 0x06000019 RID: 25 RVA: 0x000043C4 File Offset: 0x000033C4
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(this.captureMovie1.ImageVideoShot);

            // Crop cả 4 phía với cùng crop percent như video display và fit vào pictureBox
            Bitmap croppedImage = CropAndFitToPictureBox(originalImage, this.pictureBox1);

            this.pictureBox1.Image = croppedImage;
            this.checkBox1.Checked = true;

            // Dispose original
            originalImage.Dispose();
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00004400 File Offset: 0x00003400
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(this.captureMovie1.ImageVideoShot);
            Bitmap croppedImage = CropAndFitToPictureBox(originalImage, this.pictureBox2);
            this.pictureBox2.Image = croppedImage;
            this.checkBox2.Checked = true;
            originalImage.Dispose();
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000443C File Offset: 0x0000343C
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(this.captureMovie1.ImageVideoShot);
            Bitmap croppedImage = CropAndFitToPictureBox(originalImage, this.pictureBox3);
            this.pictureBox3.Image = croppedImage;
            this.checkBox3.Checked = true;
            originalImage.Dispose();
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00004478 File Offset: 0x00003478
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Bitmap originalImage = new Bitmap(this.captureMovie1.ImageVideoShot);
            Bitmap croppedImage = CropAndFitToPictureBox(originalImage, this.pictureBox4);
            this.pictureBox4.Image = croppedImage;
            this.checkBox4.Checked = true;
            originalImage.Dispose();
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000044B4 File Offset: 0x000034B4
        private void buttonInHinhAnh_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            GeneralUtility.ArrayCaptureImagesPath.Clear();
            bool @checked = this.checkBox1.Checked;
            if (@checked)
            {
                text = Path.GetTempFileName();
                this.pictureBox1.Image.Save(text, ImageFormat.Jpeg);
                GeneralUtility.ArrayCaptureImagesPath.Add(text);
            }
            bool checked2 = this.checkBox2.Checked;
            if (checked2)
            {
                text = Path.GetTempFileName();
                this.pictureBox2.Image.Save(text, ImageFormat.Jpeg);
                GeneralUtility.ArrayCaptureImagesPath.Add(text);
            }
            bool checked3 = this.checkBox3.Checked;
            if (checked3)
            {
                text = Path.GetTempFileName();
                this.pictureBox3.Image.Save(text, ImageFormat.Jpeg);
                GeneralUtility.ArrayCaptureImagesPath.Add(text);
            }
            bool checked4 = this.checkBox4.Checked;
            if (checked4)
            {
                text = Path.GetTempFileName();
                this.pictureBox4.Image.Save(text, ImageFormat.Jpeg);
                GeneralUtility.ArrayCaptureImagesPath.Add(text);
            }
            ReportDirectImage reportDirectImage = new ReportDirectImage();
            reportDirectImage.Show();
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000045CC File Offset: 0x000035CC
        private void buttonKetQua_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPageKetQua;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000045E4 File Offset: 0x000035E4
        private int LoadMauKetQua(int intDichVuId)
        {
            EClinicDB.LoadDatasetIntoComboBox(ref this.comboBoxMauKetQua, "KetQuaSieuAmMau", "", StringExt.StringNull(intDichVuId), string.Empty);
            return this.comboBoxMauKetQua.Items.Count;
        }

        // Token: 0x06000020 RID: 32 RVA: 0x0000462C File Offset: 0x0000362C
        private void LoadControlKetQuaSA(string stringMaSieuAm)
        {
            this.boolLoadedControlKetqua = true;
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00004638 File Offset: 0x00003638
        private void buttonMacDinh_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SELECT  `sa`.Title, `sa`.Title_Alias,  `sa`.Text_001,  `sa`.Text_002,  `sa`.Text_003,  `sa`.KetLuan,  `sa`.GhiChu FROM  `dm_dichvu_giatrichuan` sa WHERE sa.`DichVu_GiaTriChuan_Id` ='{0}'", this.comboBoxMauKetQua.SelectedValue);
            dataSet = EClinicDB.FillDataset(stringBuilder.ToString(), true);
            DataTable dataTable = dataSet.Tables[0];
            DataTableReader dataTableReader = dataTable.CreateDataReader();
            while (dataTableReader.Read())
            {
                this.richTextBoxExtNoiDung.RichTextBox.Rtf = dataTableReader.GetString(2);
                this.richTextBoxExtKetLuan.RichTextBox.Rtf = dataTableReader.GetString(5);
                this.richTextBoxLoiDan.Rtf = dataTableReader.GetValue(6).ToString();
            }
            dataTableReader.Close();
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000046EC File Offset: 0x000036EC
        private void buttonLayLaiKetQua_Click(object sender, EventArgs e)
        {
            this.DanhSachKetQuaSieuAmDaThucHien();
            bool flag = this.dataGridViewExt1.RowCount > 0;
            if (flag)
            {
                this.buttonSuaKetQua.Enabled = true;
            }
            else
            {
                this.buttonSuaKetQua.Enabled = false;
            }
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00004734 File Offset: 0x00003734
        private void DanhSachKetQuaSieuAmDaThucHien()
        {
            string empty = string.Empty;
            EClinicDB.LoadDatasetIntoDataGridView(ref this.dataGridViewExt1, "YeuCauSieuAmDaThucHien", "", StringExt.StringNull(this.comboBoxPhongSieuAm.SelectedValue));
            this.dataGridViewExt1.HeaderColumnState = DataGridViewExt.CustomHeaderState.HideColumn;
            this.dataGridViewExt1.CustomHeaderText = "SoPhieuYeuCau|Số phiếu YC|80; Ho_Ten|Họ tên Bệnh nhân|130; TenDichVu|Tên dịch vụ|120;DonGia|Đơn giá|50; MienGiam|Miễn giảm|50; TenPhongBan|Phòng ban thực hiện|80; NgayYeuCau|Ngày yêu cầu|110; TrangThai|Trạng thái|100";
        }

        // Token: 0x06000024 RID: 36 RVA: 0x0000478C File Offset: 0x0000378C
        private bool LoadDuLieuBenhNhan(int intBenhNhanID)
        {
            ParamCollection paramCollection = new ParamCollection();
            paramCollection.Add("DataGroup", DbDataType.VarChar, 50, "ThongTinBenhNhan");
            paramCollection.Add("FiltData", DbDataType.Int32, 11, intBenhNhanID);
            DataSet dataSet = EClinicDB.FillDatasetDecrypt("sp_Sys_GetListDataGridView", paramCollection, "dm_benhnhan");
            DataRow dataRow = dataSet.Tables[0].Rows[0];
            bool flag = dataRow != null;
            bool result;
            if (flag)
            {
                this.labelMaYTE.Text = StringExt.StringNull(dataRow["MaYTe"]);
                this.labelHoTenBenhNhan.Text = StringExt.StringNull(dataRow["Ho_Ten"]);
                this.labelGioiTinh.Text = StringExt.StringNull(dataRow["Gioi_Tinh"]);
                this.labelNamSinh.Text = StringExt.StringNull(dataRow["Nam_Sinh"]);
                this.labelDiaChi.Text = StringExt.StringNull(dataRow["Dia_Chi"]) + ", " + StringExt.StringNull(dataRow["Quan_Huyen"]);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        // Token: 0x06000025 RID: 37 RVA: 0x000048B8 File Offset: 0x000038B8
        private void buttonSaveKetQua_Click(object sender, EventArgs e)
        {
            bool flag = this.LuuKetQuaSieuAm();
            if (flag)
            {
                this.SetTrangThaiNutLenh(EchoAdmin.ButtonStat.Save);
                this.buttonCapnhatDataGrid.PerformClick();
            }
        }

        // Token: 0x06000026 RID: 38 RVA: 0x000048E8 File Offset: 0x000038E8
        private bool LuuKetQuaSieuAm()
        {
            ParamCollection paramCollection = new ParamCollection();
            bool flag = false;
            string str = "Chỉnh sửa";
            bool flag2 = this.intClsKetqua == 0;
            if (flag2)
            {
                paramCollection.Add("GroupCommand", DbDataType.VarString, 10, "INSERT");
                flag = true;
                str = "Lưu mới";
            }
            else
            {
                paramCollection.Add("GroupCommand", DbDataType.VarString, 10, "UPDATE");
            }
            paramCollection.Add("CLSKetQua_Id", DbDataType.Int32, 11, this.intClsKetqua, ParameterDirection.InputOutput);
            paramCollection.Add("YeuCauChiTiet_Id", DbDataType.Int32, 11, this.intCLSKetQuaChiTiet_Id);
            paramCollection.Add("NgayThucHien", DbDataType.DateTime, 1, this.dateTimePickerThucHien.Value);
            paramCollection.Add("NoiThucHien_Id", DbDataType.Int32, 1, (int)this.comboBoxPhongSieuAm.SelectedValue);
            paramCollection.Add("BacSiThucHien_Id", DbDataType.Int32, 11, (int)this.comboBoxBsSieuAm.SelectedValue);
            paramCollection.Add("ChanDoanLamSang", DbDataType.String, 500, this.textBoxChanDoanLamSang.Text);
            paramCollection.Add("MoTa", DbDataType.Blob, 1, this.richTextBoxExtNoiDung.RichTextBox.Rtf);
            paramCollection.Add("KetLuan", DbDataType.Blob, 1, this.richTextBoxExtKetLuan.RichTextBox.Rtf);
            paramCollection.Add("GhiChu", DbDataType.Blob, 1, this.richTextBoxLoiDan.Rtf);
            try
            {
                EClinicDB.ExecuteNonQuery("spps_clsketqua", CommandType.StoredProcedure, paramCollection);
                this.intClsKetqua = int.Parse(paramCollection[1].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Concat(new object[]
                {
                    ex.Message,
                    "\n",
                    ex.Source,
                    "\n",
                    ex.InnerException
                }), "Lỗi " + str + " kết quả.");
            }
            paramCollection.Clear();
            GeneralUtility.ArrayCaptureImages.Clear();
            bool @checked = this.checkBox1.Checked;
            if (@checked)
            {
                GeneralUtility.ArrayCaptureImages.Add((Bitmap)this.pictureBox1.Image.Clone());
            }
            bool checked2 = this.checkBox2.Checked;
            if (checked2)
            {
                GeneralUtility.ArrayCaptureImages.Add((Bitmap)this.pictureBox2.Image.Clone());
            }
            bool checked3 = this.checkBox3.Checked;
            if (checked3)
            {
                GeneralUtility.ArrayCaptureImages.Add((Bitmap)this.pictureBox3.Image.Clone());
            }
            bool checked4 = this.checkBox4.Checked;
            if (checked4)
            {
                GeneralUtility.ArrayCaptureImages.Add((Bitmap)this.pictureBox4.Image.Clone());
            }
            bool flag3 = !flag;
            if (flag3)
            {
                paramCollection.Add("GroupCommand", DbDataType.VarString, 10, "DELETE2");
                paramCollection.Add("Images_Id", DbDataType.Int32, 11, 0);
                paramCollection.Add("CLSKetQua_Id", DbDataType.Int32, 11, this.intClsKetqua);
                paramCollection.Add("Image_Data", DbDataType.Blob, 0, null);
                paramCollection.Add("TamNgung", DbDataType.Int32, 1, 0);
                try
                {
                    EClinicDB.ExecuteNonQuery("spps_clsketqua_images", CommandType.StoredProcedure, paramCollection);
                    paramCollection.Clear();
                }
                catch (Exception ex2)
                {
                    paramCollection.Clear();
                    MessageBox.Show(string.Concat(new object[]
                    {
                        ex2.Message,
                        "\n",
                        ex2.Source,
                        "\n",
                        ex2.InnerException
                    }), "Lỗi xóa hình ảnh");
                }
            }
            foreach (Bitmap image in GeneralUtility.ArrayCaptureImages)
            {
                paramCollection.Add("GroupCommand", DbDataType.VarString, 10, "INSERT");
                paramCollection.Add("Images_Id", DbDataType.Int32, 11, 0);
                paramCollection.Add("CLSKetQua_Id", DbDataType.Int32, 11, this.intClsKetqua);
                paramCollection.Add("Image_Data", DbDataType.Blob, 0, GeneralUtility.ReadBitmap2ByteArray(image));
                paramCollection.Add("TamNgung", DbDataType.Int32, 1, 0);
                try
                {
                    EClinicDB.ExecuteNonQuery("spps_clsketqua_images", CommandType.StoredProcedure, paramCollection);
                    paramCollection.Clear();
                }
                catch (Exception ex3)
                {
                    paramCollection.Clear();
                    MessageBox.Show(string.Concat(new object[]
                    {
                        ex3.Message,
                        "\n",
                        ex3.Source,
                        "\n",
                        ex3.InnerException
                    }), "Lỗi lưu hình ảnh");
                }
            }
            return true;
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00004DEC File Offset: 0x00003DEC
        private void buttonInKetQua_Click(object sender, EventArgs e)
        {
            ParamCollection paramCollection = new ParamCollection();
            paramCollection.Add("param1", DbDataType.Int32, 11, this.intCLSKetQuaChiTiet_Id);
            DataSet dataSet = EClinicDB.FillDatasetDecrypt("sp_rp_ketqua_sieuam_all_value", paramCollection, "dm_benhnhan");
            dataSet.Tables[0].TableName = "KetQuaSieuAm";
            paramCollection.Clear();
            paramCollection.Add("param1", DbDataType.Int32, 11, this.intCLSKetQuaChiTiet_Id);
            EClinicDB.FillDataset(ref dataSet, "sp_clsketqua_sieuam_image_2", CommandType.StoredProcedure, paramCollection, "Image");
            paramCollection.Clear();
            DataRowCollection rows = dataSet.Tables["Image"].Rows;
            bool flag = rows.Count > 1;
            if (flag)
            {
                int columnIndex = dataSet.Tables["Image"].Columns.IndexOf("image");
                object value = rows[0][columnIndex];
                object value2 = rows[1][columnIndex];
                DataTable dataTable = dataSet.Tables["KetQuaSieuAm"];
                dataTable.Columns.Add("image2", typeof(byte[]));
                int columnIndex2 = dataTable.Columns.IndexOf("image");
                int columnIndex3 = dataTable.Columns.IndexOf("image2");
                dataTable.Rows[0][columnIndex2] = value;
                dataTable.Rows[0][columnIndex3] = value2;
            }
            new ReportCommon(EClinicConfig.ReportsPath + "SieuAmChung.rpt")
            {
                DataSource = dataSet
            }.Show();
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00004F90 File Offset: 0x00003F90
        private void buttonSuaKetQua_Click(object sender, EventArgs e)
        {
            bool flag = this.boolDangTienHanhSieuAm;
            if (flag)
            {
                MessageBox.Show("Đang thực hiện siêu âm!!!\n\nKhông thể tiến hành sửa chữa kết quả cũ\n\n Cần lưu lại kết quả đang thực hiện.");
                this.tabControl1.SelectedTab = this.tabPageKetQua;
            }
            else
            {
                this.LoadKetQuaSieuAmDaThucHien(this.intCLSKetQuaChiTiet_Id);
                this.SetTrangThaiNutLenh(EchoAdmin.ButtonStat.Edit);
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00004FE0 File Offset: 0x00003FE0
        private void LoadKetQuaSieuAmDaThucHien(int intCLSKetQuaChiTiet_Id)
        {
            this.boolLoadedCapture = true;
            this.labelSoPhieuYeuCau.Text = this.dataGridViewExt1.SelectedRows[0].Cells["SoPhieuYeuCau"].Value.ToString();
            EClinicConfig.BenhNhanID = int.Parse(StringExt.StringZero(this.dataGridViewExt1.SelectedRows[0].Cells["BenhNhan_Id"].Value));
            bool flag = this.LoadDuLieuBenhNhan(EClinicConfig.BenhNhanID);
            if (flag)
            {
                this.labelBsSieuAm.Text = this.comboBoxBsSieuAm.Text;
                this.labelPhongBanThucHien.Text = this.comboBoxPhongSieuAm.Text;
                this.labelDichVuSieuAm.Text = StringExt.StringNull(this.dataGridViewExt1.SelectedRows[0].Cells["TenDichVu"].Value);
                this.intDichVu_Id = int.Parse(StringExt.StringZero(this.dataGridViewExt1.SelectedRows[0].Cells["DichVu_id"].Value));
                DataSet dataSet = EClinicDB.FillDataset("SELECT * FROM clsketqua KQ WHERE KQ.CLSKetQua_Id = '" + this.dataGridViewExt1.SelectedRows[0].Cells["CLSKetQua_Id"].Value.ToString() + "'");
                DataRow dataRow = dataSet.Tables[0].Rows[0];
                bool flag2 = dataRow != null;
                if (flag2)
                {
                    this.textBoxChanDoanLamSang.Text = StringExt.StringNull(dataRow["ChanDoanLamSang"]);
                    this.richTextBoxExtNoiDung.RichTextBox.Rtf = StringExt.StringNull(dataRow["MoTa"]);
                    this.richTextBoxExtKetLuan.RichTextBox.Rtf = StringExt.StringNull(dataRow["KetLuan"]);
                    this.richTextBoxLoiDan.Rtf = StringExt.StringNull(dataRow["GhiChu"]);
                }
                dataSet = EClinicDB.FillDataset("SELECT * FROM clsketqua_images HA WHERE HA.CLSKetQua_Id = '" + this.dataGridViewExt1.SelectedRows[0].Cells["CLSKetQua_Id"].Value.ToString() + "'");
                this.aImagesId.Clear();
                this.pictureBox1.Image = null;
                this.checkBox1.Checked = false;
                this.pictureBox2.Image = null;
                this.checkBox2.Checked = false;
                this.pictureBox3.Image = null;
                this.checkBox3.Checked = false;
                this.pictureBox4.Image = null;
                this.checkBox4.Checked = false;
                for (int i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
                {
                    dataRow = dataSet.Tables[0].Rows[i];
                    this.aImagesId.Add((int)dataRow["Images_Id"]);
                    byte[] content = (byte[])dataRow["Image_Data"];
                    Bitmap image = GeneralUtility.ReadByteArray2Image(content);
                    bool flag3 = i == 0;
                    if (flag3)
                    {
                        this.pictureBox1.Image = image;
                        this.checkBox1.Checked = true;
                    }
                    bool flag4 = i == 1;
                    if (flag4)
                    {
                        this.pictureBox2.Image = image;
                        this.checkBox2.Checked = true;
                    }
                    bool flag5 = i == 2;
                    if (flag5)
                    {
                        this.pictureBox3.Image = image;
                        this.checkBox3.Checked = true;
                    }
                    bool flag6 = i == 3;
                    if (flag6)
                    {
                        this.pictureBox4.Image = image;
                        this.checkBox4.Checked = true;
                    }
                }
                bool flag7 = this.LoadMauKetQua(this.intDichVu_Id) > 0;
                if (flag7)
                {
                    this.buttonMacDinh.Enabled = true;
                }
            }
            this.boolDangTienHanhSieuAm = true;
            this.tabControl1.SelectedTab = this.tabPageKetQua;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00005400 File Offset: 0x00004400
        private void buttonChonBenhMoi_Click(object sender, EventArgs e)
        {
            this.intClsKetqua = 0;
            this.intCLSKetQuaChiTiet_Id = 0;
            this.boolDangTienHanhSieuAm = false;
            this.CapNhatDataGridDsYeuCau();
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00003EEB File Offset: 0x00002EEB
        private void buttonPauseHinhAnh_Click(object sender, EventArgs e)
        {
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00005420 File Offset: 0x00004420
        private void buttonTimKetQua_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ParamCollection paramCollection = new ParamCollection();
            paramCollection.Add("DataGroup", DbDataType.VarChar, 50, "yeucausieuamdathuchien");
            paramCollection.Add("FiltData", DbDataType.VarChar, 11, string.Empty);
            ds = EClinicDB.FillDatasetDecrypt("sp_Sys_GetListDataGridView", paramCollection, "dm_benhnhan");
            TimKetQuaSieuAm timKetQuaSieuAm = new TimKetQuaSieuAm(ds);
            timKetQuaSieuAm.ShowDialog();
            this.intClsKetqua = timKetQuaSieuAm.ValueMember;
            bool flag = this.intClsKetqua > 0;
            if (flag)
            {
                this.buttonTienHanh.Enabled = false;
                this.buttonSuaKetQua.Enabled = true;
                this.ThemDanhSachKetQuaSieuAmDaThucHien(this.intClsKetqua);
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000054C8 File Offset: 0x000044C8
        private void ThemDanhSachKetQuaSieuAmDaThucHien(int intClsKetqua)
        {
            ParamCollection paramCollection = new ParamCollection();
            paramCollection.Add("param1", DbDataType.Int32, 11, intClsKetqua);
            DataSet dataSet = EClinicDB.FillDatasetDecrypt("sp_KetQuaDaThucHienSieuAm", paramCollection, "dm_benhnhan");
            this.dataGridViewExt1.DataSource = dataSet;
            this.dataGridViewExt1.DataMember = dataSet.Tables[0].TableName;
            this.dataGridViewExt1.HeaderColumnState = DataGridViewExt.CustomHeaderState.HideColumn;
            this.dataGridViewExt1.CustomHeaderText = "SoPhieuYeuCau|Số phiếu YC|80; Ho_Ten|Họ tên Bệnh nhân|130; TenDichVu|Tên dịch vụ|120;TenPhongBan|Phòng ban thực hiện|80; NgayTao|Ngày thực hiện|110";
            this.intCLSKetQuaChiTiet_Id = int.Parse(StringExt.StringZero(this.dataGridViewExt1.SelectedRows[0].Cells["YeuCauChiTiet_Id"].Value));
        }

        // Token: 0x0400001C RID: 28
        private const string HEADER_COLUMN_TEXT = "SoPhieuYeuCau|Số phiếu YC|80; Ho_Ten|Họ tên Bệnh nhân|130; TenDichVu|Tên dịch vụ|120;DonGia|Đơn giá|50; MienGiam|Miễn giảm|50; TenPhongBan|Phòng ban thực hiện|80; NgayYeuCau|Ngày yêu cầu|110; TrangThai|Trạng thái|100";

        // Token: 0x0400001D RID: 29
        private bool boolLoadedControlKetqua = false;

        // Token: 0x0400001E RID: 30
        private bool boolLoadedCapture = true;

        // Token: 0x0400001F RID: 31
        private bool boolDangTienHanhSieuAm = false;

        // Token: 0x04000020 RID: 32
        private int intDichVu_Id = 0;

        // Token: 0x04000021 RID: 33
        private int intCLSKetQuaChiTiet_Id = 0;

        // Token: 0x04000022 RID: 34
        private List<int> aImagesId = new List<int>();

        // Token: 0x04000023 RID: 35
        private int intClsKetqua = 0;

        // Token: 0x04000024 RID: 36
        private bool isChangePhongSieuAm = false;

        // Token: 0x0200000E RID: 14
        private enum ButtonStat
        {
            // Token: 0x040000A0 RID: 160
            Progress,
            // Token: 0x040000A1 RID: 161
            Edit,
            // Token: 0x040000A2 RID: 162
            Save,
            // Token: 0x040000A3 RID: 163
            Update,
            // Token: 0x040000A4 RID: 164
            Cancel,
            // Token: 0x040000A5 RID: 165
            AddNew
        }
    }
}
