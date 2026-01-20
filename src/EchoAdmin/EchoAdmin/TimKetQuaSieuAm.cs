using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SKCDExtCtrl;
using SKCDExtCtrl.GridFilterFactories;
using SKCDExtCtrl.GridFilters;

namespace EchoAdmin
{
	// Token: 0x02000007 RID: 7
	public partial class TimKetQuaSieuAm : Form
	{
		// Token: 0x0600004D RID: 77 RVA: 0x0000AC20 File Offset: 0x00009C20
		public TimKetQuaSieuAm()
		{
			this.InitializeComponent();
			this.dataGridView1.DoubleClick += this.dataGridView1_DoubleClick;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000ACAC File Offset: 0x00009CAC
		public TimKetQuaSieuAm(DataSet ds)
		{
			this.InitializeComponent();
			this.dataGridView1.DoubleClick += this.dataGridView1_DoubleClick;
			this._dsSearch = ds;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000AD40 File Offset: 0x00009D40
		private void TimKetQuaSieuAm_Load(object sender, EventArgs e)
		{
			this.dataGridView1.DataSource = this._dsSearch.Tables[0];
			this.dataGridView1.Columns["CLSKetQua_Id"].Visible = false;
			this.dataGridView1.Columns["BenhNhan_Id"].Visible = false;
			this.dataGridView1.Columns["CLSYeuCau_Id"].Visible = false;
			this.dataGridView1.Columns["YeuCauChiTiet_Id"].Visible = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000ADDC File Offset: 0x00009DDC
		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			bool flag = this.dataGridView1.SelectedRows.Count > 0;
			if (flag)
			{
				this.buttonOK.PerformClick();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000AE10 File Offset: 0x00009E10
		private void buttonOK_Click(object sender, EventArgs e)
		{
			bool flag = this.dataGridView1.SelectedCells[0].RowIndex >= 0;
			if (flag)
			{
				this._iRow = this.dataGridView1.SelectedCells[1].RowIndex;
				bool flag2 = this._columnMember != string.Empty;
				if (flag2)
				{
					this._sValue = this.dataGridView1[this._columnMember, this._iRow].Value.ToString();
				}
				else
				{
					this._sValue = this.dataGridView1[1, this._iRow].Value.ToString();
				}
				this.key_id = (int)this.dataGridView1[0, this._iRow].Value;
				base.Close();
			}
			else
			{
				bool flag3 = MessageBox.Show("Bạn chưa chọn dòng dữ liệu", "Chú ý", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK;
				if (flag3)
				{
					base.Close();
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000AF0F File Offset: 0x00009F0F
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this._sValue = string.Empty;
			this._iRow = -1;
			this.key_id = -1;
			base.Close();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000AF34 File Offset: 0x00009F34
		public int SelectRowIndex
		{
			get
			{
				return this._iRow;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000AF4C File Offset: 0x00009F4C
		public string SelectMemberValue
		{
			get
			{
				return this._sValue;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000AF64 File Offset: 0x00009F64
		public int ValueMember
		{
			get
			{
				return this.key_id;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000056 RID: 86 RVA: 0x0000AF7C File Offset: 0x00009F7C
		// (set) Token: 0x06000057 RID: 87 RVA: 0x0000AF94 File Offset: 0x00009F94
		public string ColumnMember
		{
			get
			{
				return this._columnMember;
			}
			set
			{
				this._columnMember = value;
			}
		}

		// Token: 0x04000088 RID: 136
		private DataSet _dsSearch = new DataSet();

		// Token: 0x04000089 RID: 137
		private string _sdataMember = string.Empty;

		// Token: 0x0400008A RID: 138
		private string _sheaderGridColumnText = string.Empty;

		// Token: 0x0400008B RID: 139
		private string _sValue = string.Empty;

		// Token: 0x0400008C RID: 140
		private int _iRow = -1;

		// Token: 0x0400008D RID: 141
		private string _sTitle = string.Empty;

		// Token: 0x0400008E RID: 142
		private int key_id = -1;

		// Token: 0x0400008F RID: 143
		private string _columnMember = string.Empty;
	}
}
