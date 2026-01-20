using AMS.Profile;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Utility
{
    // Token: 0x0200000C RID: 12
    public static class EClinicConfig
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600002B RID: 43 RVA: 0x00002BD8 File Offset: 0x00001BD8
        public static Size FullSizeWindows
        {
            get
            {
                return new Size(1024, 700);
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600002D RID: 45 RVA: 0x00002C04 File Offset: 0x00001C04
        // (set) Token: 0x0600002C RID: 44 RVA: 0x00002BF9 File Offset: 0x00001BF9
        public static int UserIdLogin
        {
            get
            {
                return EClinicConfig.intUserID;
            }
            set
            {
                EClinicConfig.intUserID = value;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600002F RID: 47 RVA: 0x00002C24 File Offset: 0x00001C24
        // (set) Token: 0x0600002E RID: 46 RVA: 0x00002C1B File Offset: 0x00001C1B
        public static string UserGroupTypes
        {
            get
            {
                return EClinicConfig.sUserGroupType;
            }
            set
            {
                EClinicConfig.sUserGroupType = value;
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00002C44 File Offset: 0x00001C44
        // (set) Token: 0x06000030 RID: 48 RVA: 0x00002C3B File Offset: 0x00001C3B
        public static int NhanVienLoginID
        {
            get
            {
                return EClinicConfig.intNhanVienLogin;
            }
            set
            {
                EClinicConfig.intNhanVienLogin = value;
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000032 RID: 50 RVA: 0x00002C5C File Offset: 0x00001C5C
        public static string ComputerNo
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Computer No.");
                }
                return result;
            }
        }

        public static float CropLeft
        {
            get
            {
                float result = 0f;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    // Lấy giá trị dưới dạng string trước
                    var rawValue = EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "CropLeft")?.ToString();

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        // Loại bỏ ký tự 'f' nếu lỡ có trong file XML
                        rawValue = rawValue.Replace("f", "").Trim();

                        // Dùng TryParse với InvariantCulture để chấp nhận dấu chấm thập phân bất kể máy cài vùng nào
                        float.TryParse(rawValue,
                                       System.Globalization.NumberStyles.Any,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       out result);
                    }
                }
                return result;
            }
        }

        public static float CropRight
        {
            get
            {
                float result = 0f;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    // Lấy giá trị dưới dạng string trước
                    var rawValue = EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "CropRight")?.ToString();

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        // Loại bỏ ký tự 'f' nếu lỡ có trong file XML
                        rawValue = rawValue.Replace("f", "").Trim();

                        // Dùng TryParse với InvariantCulture để chấp nhận dấu chấm thập phân bất kể máy cài vùng nào
                        float.TryParse(rawValue,
                                       System.Globalization.NumberStyles.Any,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       out result);
                    }
                }
                return result;
            }

        }

        public static float CropTop
        {
            get
            {
                float result = 0f;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    // Lấy giá trị dưới dạng string trước
                    var rawValue = EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "CropTop")?.ToString();

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        // Loại bỏ ký tự 'f' nếu lỡ có trong file XML
                        rawValue = rawValue.Replace("f", "").Trim();

                        // Dùng TryParse với InvariantCulture để chấp nhận dấu chấm thập phân bất kể máy cài vùng nào
                        float.TryParse(rawValue,
                                       System.Globalization.NumberStyles.Any,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       out result);
                    }
                }
                return result;
            }

        }

        public static float CropBottom
        {
            get
            {
                float result = 0f;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    // Lấy giá trị dưới dạng string trước
                    var rawValue = EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "CropBottom")?.ToString();

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        // Loại bỏ ký tự 'f' nếu lỡ có trong file XML
                        rawValue = rawValue.Replace("f", "").Trim();

                        // Dùng TryParse với InvariantCulture để chấp nhận dấu chấm thập phân bất kể máy cài vùng nào
                        float.TryParse(rawValue,
                                       System.Globalization.NumberStyles.Any,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       out result);
                    }
                }
                return result;
            }

        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00002CB8 File Offset: 0x00001CB8
        public static string AllowChangeRoom
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Cho phep thay doi phong");
                }
                return result;
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000034 RID: 52 RVA: 0x00002D14 File Offset: 0x00001D14
        // (set) Token: 0x06000035 RID: 53 RVA: 0x00002D2B File Offset: 0x00001D2B
        public static int BenhNhanID
        {
            get
            {
                return EClinicConfig.BenhNhan_Id;
            }
            set
            {
                EClinicConfig.BenhNhan_Id = value;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000036 RID: 54 RVA: 0x00002D34 File Offset: 0x00001D34
        public static string PreMaYT
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Program Config", "preMaYTe");
                }
                return result;
            }
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002D8C File Offset: 0x00001D8C
        public static string GetDefineOfUnspecifiedComputer()
        {
            if (EClinicConfig.unspecifiedComputer == "")
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.unspecifiedComputer = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Unspecified Computer");
                }
            }
            return EClinicConfig.unspecifiedComputer;
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00002E0C File Offset: 0x00001E0C
        public static string GetEClinicPCName()
        {
            if (EClinicConfig.pcName == "")
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.pcName = EClinicConfig.OPERATION_MODULE_NAME + __.T(" Máy ") + (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Computer No.");
                }
            }
            return EClinicConfig.pcName;
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00002EA0 File Offset: 0x00001EA0
        public static string CalEClinicPCName(string module, string pcNo)
        {
            return module + __.T(" Máy ") + pcNo;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00002EC4 File Offset: 0x00001EC4
        public static string GetAddinFolder()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Add-in Folder");
            }
            return result;
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002F1C File Offset: 0x00001F1C
        public static int GetDefaultVideoCard()
        {
            int result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = int.Parse((string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Default Video Card No."));
            }
            return result;
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00002F7C File Offset: 0x00001F7C
        public static string GetProvince()
        {
            if (EClinicConfig.sProvince == "")
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.sProvince = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Province");
                }
            }
            return EClinicConfig.sProvince;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00002FFC File Offset: 0x00001FFC
        public static void SetProvince(string sCode)
        {
            if (sCode.ToString() == null)
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "Province", "");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "Province", sCode);
            }
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00003050 File Offset: 0x00002050
        public static string GetProvinceCode()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "ProvinceCode");
            }
            return result;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x000030B0 File Offset: 0x000020B0
        public static void SetProvinceCode(string sCode)
        {
            if (sCode.ToString() == null)
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "ProvinceCode", "801");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "ProvinceCode", sCode);
            }
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00003104 File Offset: 0x00002104
        public static string GetDistrict()
        {
            if (EClinicConfig.sDist == "")
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.sDist = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "District");
                }
            }
            return EClinicConfig.sDist;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00003184 File Offset: 0x00002184
        public static void SetDistrict(string sCode)
        {
            if (sCode.ToString() == null)
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "District", "");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "District", sCode);
            }
        }

        // Token: 0x06000042 RID: 66 RVA: 0x000031D8 File Offset: 0x000021D8
        public static string GetDistrictCode()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "DistrictCode");
            }
            return result;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00003238 File Offset: 0x00002238
        public static void SetDistrictCode(string sCode)
        {
            if (sCode.ToString() == null)
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "DistrictCode", "801");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "DistrictCode", sCode);
            }
        }

        // Token: 0x06000044 RID: 68 RVA: 0x0000328C File Offset: 0x0000228C
        public static string GetSeparatorString()
        {
            return EClinicConfig.sSeparator;
        }

        // Token: 0x06000045 RID: 69 RVA: 0x000032A4 File Offset: 0x000022A4
        public static int GetPatientQueueWaitTime()
        {
            int result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                int num = Convert.ToInt32((string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "PatientQueueWaitTimeSeconds")) * 1000;
                if (num <= 0)
                {
                    num = 5000;
                }
                result = num;
            }
            return result;
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00003318 File Offset: 0x00002318
        public static string GetStoredImageFolder()
        {
            if (EClinicConfig.sImageFolder == "")
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.sImageFolder = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Image Folder");
                    if (!Directory.Exists(EClinicConfig.sImageFolder))
                    {
                        Directory.CreateDirectory(EClinicConfig.sImageFolder);
                    }
                }
            }
            return EClinicConfig.sImageFolder;
        }

        // Token: 0x06000047 RID: 71 RVA: 0x000033B0 File Offset: 0x000023B0
        public static string GetLogoFilePath()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Logo File");
            }
            return result;
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00003408 File Offset: 0x00002408
        public static CultureInfo GetCultureInfo()
        {
            if (EClinicConfig.culInfo == null)
            {
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    EClinicConfig.culInfo = new CultureInfo((string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "CultureInfo"));
                }
            }
            return EClinicConfig.culInfo;
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00003484 File Offset: 0x00002484
        public static bool IsAutoCompleteTextBoxMode()
        {
            bool result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                string text = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "AutoCompleteTextBoxMode");
                result = (text.ToLower() == "on");
            }
            return result;
        }

        // Token: 0x0600004A RID: 74 RVA: 0x000034F4 File Offset: 0x000024F4
        public static bool EnableServiceRequestSelection()
        {
            bool result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                string text = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Enable Service Request Selection");
                result = (text.ToLower() == "yes");
            }
            return result;
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00003564 File Offset: 0x00002564
        public static void SetServiceRequestSelectionMode(string mode)
        {
            if (mode.Trim().ToLower() == "no")
            {
                EClinicConfig.xmlEclinicProfile.SetValue("EchoAdmin Params", "Enable Service Request Selection", "no");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("EchoAdmin Params", "Enable Service Request Selection", "yes");
            }
        }

        // Token: 0x0600004C RID: 76 RVA: 0x000035C8 File Offset: 0x000025C8
        public static string GetServiceRequestSelectionMode()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Enable Service Request Selection");
            }
            return result;
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00003628 File Offset: 0x00002628
        public static bool EnableRemovingFinishedPatient()
        {
            bool result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                string text = (string)EClinicConfig.xmlEclinicProfile.GetValue("EchoAdmin Params", "Remove Finished Patient");
                result = (text.ToLower() == "yes");
            }
            return result;
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00003698 File Offset: 0x00002698
        public static void SetRemovingFinishedPatientMode(string mode)
        {
            if (mode.Trim().ToLower() == "no")
            {
                EClinicConfig.xmlEclinicProfile.SetValue("EchoAdmin Params", "Remove Finished Patient", "no");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("EchoAdmin Params", "Remove Finished Patient", "yes");
            }
        }

        // Token: 0x0600004F RID: 79 RVA: 0x000036FC File Offset: 0x000026FC
        public static bool IsStandAloneModule()
        {
            bool result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                string text = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Stand Alone Module");
                result = (text.ToLower() == "yes");
            }
            return result;
        }

        // Token: 0x06000050 RID: 80 RVA: 0x0000376C File Offset: 0x0000276C
        public static void SetStandAloneMode(string mode)
        {
            if (mode.Trim().ToLower() == "no")
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "Stand Alone Module", "no");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "Stand Alone Module", "yes");
            }
        }

        // Token: 0x06000051 RID: 81 RVA: 0x000037D0 File Offset: 0x000027D0
        public static void SetAutoCompleteMode(string mode)
        {
            if (mode.Trim().ToLower() == "on")
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "AutoCompleteTextBoxMode", "on");
            }
            else
            {
                EClinicConfig.xmlEclinicProfile.SetValue("Common Configuration", "AutoCompleteTextBoxMode", "off");
            }
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00003834 File Offset: 0x00002834
        public static string GetStandAloneMode()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Stand Alone Module");
            }
            return result;
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00003894 File Offset: 0x00002894
        public static string GetAutoCompleteMode()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "AutoCompleteTextBoxMode");
            }
            return result;
        }

        // Token: 0x06000054 RID: 84 RVA: 0x000038F4 File Offset: 0x000028F4
        public static string GetInitPasswd()
        {
            string result;
            using (EClinicConfig.xmlEclinicProfile.Buffer())
            {
                result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "Init Passwd");
            }
            return result;
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000055 RID: 85 RVA: 0x0000394C File Offset: 0x0000294C
        public static string ConnectionString
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Data Connection", "ConnectionString");
                }
                return result;
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000056 RID: 86 RVA: 0x000039A4 File Offset: 0x000029A4
        public static bool ConnectionStringEncrypt
        {
            get
            {
                bool result = false;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = ((string)EClinicConfig.xmlEclinicProfile.GetValue("Data Connection", "Encrypt") == "Yes");
                }
                return result;
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000057 RID: 87 RVA: 0x00003A1C File Offset: 0x00002A1C
        public static int CountryID
        {
            get
            {
                string s;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    s = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "CountryID");
                }
                return int.Parse(s);
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000058 RID: 88 RVA: 0x00003A80 File Offset: 0x00002A80
        public static string PreMaYTe
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "PreMaYTe");
                }
                return result;
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000059 RID: 89 RVA: 0x00003ADC File Offset: 0x00002ADC
        public static string ReportsPath
        {
            get
            {
                string result;
                using (EClinicConfig.xmlEclinicProfile.Buffer())
                {
                    result = (string)EClinicConfig.xmlEclinicProfile.GetValue("Common Configuration", "ReportsPath");
                }
                return result;
            }
        }

        // Token: 0x04000013 RID: 19
        private static Xml xmlEclinicProfile = new Xml("EClinicProfile.xml");

        // Token: 0x04000014 RID: 20
        private static CultureInfo culInfo = null;

        // Token: 0x04000015 RID: 21
        private static string sSeparator = ";";

        // Token: 0x04000016 RID: 22
        private static string sProvince = string.Empty;

        // Token: 0x04000017 RID: 23
        private static string sDist = string.Empty;

        // Token: 0x04000018 RID: 24
        private static string unspecifiedComputer = string.Empty;

        // Token: 0x04000019 RID: 25
        private static string pcName = string.Empty;

        // Token: 0x0400001A RID: 26
        private static int BenhNhan_Id = 0;

        // Token: 0x0400001B RID: 27
        private static int intUserID = 1;

        // Token: 0x0400001C RID: 28
        private static int intNhanVienLogin = 1;

        // Token: 0x0400001D RID: 29
        private static string sUserGroupType = string.Empty;

        // Token: 0x0400001E RID: 30
        public static string userNameLogin = string.Empty;

        // Token: 0x0400001F RID: 31
        public static string OPERATION_MODULE_NAME = Process.GetCurrentProcess().ProcessName.ToUpper().Split(new char[]
        {
            '.'
        })[0];

        // Token: 0x04000020 RID: 32
        public static string BenhNhan_Gioi = "Nam";

        // Token: 0x04000021 RID: 33
        public static int KetQua_SA_Id_cur = 0;

        // Token: 0x04000022 RID: 34
        public static int KetQua_SA_Id_search = 0;

        // Token: 0x04000023 RID: 35
        public static string sImageFolder = "";

        // Token: 0x04000024 RID: 36
        public static int Nhan_Vien_Id = 0;

        // Token: 0x04000025 RID: 37
        public static int Loai_SA_Id = 1;

        // Token: 0x04000026 RID: 38
        public static string Loai_SA_Code = "SATQ";

        // Token: 0x04000027 RID: 39
        public static string LamSang = "";

        // Token: 0x04000028 RID: 40
        public static string TitleReport = string.Empty;

        // Token: 0x04000029 RID: 41
        public static string sSelectImage = string.Empty;
    }
}
