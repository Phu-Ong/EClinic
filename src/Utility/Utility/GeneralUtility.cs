using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Utility
{
	// Token: 0x02000008 RID: 8
	public static class GeneralUtility
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002578 File Offset: 0x00001578
		public static byte[] ReadBitmap2ByteArray(Bitmap image)
		{
			byte[] result;
			try
			{
				// Tạo bitmap mới với DPI cao (300 DPI) để đảm bảo chất lượng in tốt
				Bitmap highDpiBitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
				highDpiBitmap.SetResolution(300f, 300f); // 300 DPI cho in ấn chất lượng cao
				
				using (Graphics g = Graphics.FromImage(highDpiBitmap))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.CompositingQuality = CompositingQuality.HighQuality;
					g.CompositingMode = CompositingMode.SourceCopy;
					g.DrawImage(image, 0, 0, image.Width, image.Height);
				}

				MemoryStream memoryStream = new MemoryStream();
				// Dùng PNG (lossless) thay vì JPEG để giữ chất lượng ảnh hoàn hảo
				// PNG không mất chất lượng như JPEG, phù hợp cho in ấn y tế
				highDpiBitmap.Save(memoryStream, ImageFormat.Png);
				result = memoryStream.ToArray();
				
				highDpiBitmap.Dispose();
			}
			finally
			{
				if (image != null)
				{
					((IDisposable)image).Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025C8 File Offset: 0x000015C8
		public static byte[] ReadBitmap2ByteArray(string imagePath)
		{
			byte[] result;
			using (Bitmap bitmap = new Bitmap(imagePath))
			{
				// Tạo bitmap mới với DPI cao (300 DPI) để đảm bảo chất lượng in tốt
				Bitmap highDpiBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
				highDpiBitmap.SetResolution(300f, 300f); // 300 DPI cho in ấn chất lượng cao
				
				using (Graphics g = Graphics.FromImage(highDpiBitmap))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
					g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
					g.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
				}

				MemoryStream memoryStream = new MemoryStream();
				// Dùng PNG (lossless) thay vì JPEG để giữ chất lượng ảnh hoàn hảo
				highDpiBitmap.Save(memoryStream, ImageFormat.Png);
				result = memoryStream.ToArray();
				
				highDpiBitmap.Dispose();
			}
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000261C File Offset: 0x0000161C
		public static Bitmap ReadByteArray2Image(byte[] content)
		{
			MemoryStream stream = new MemoryStream(content);
			return new Bitmap(stream);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002640 File Offset: 0x00001640
		public static void DeleteArrayImage()
		{
			foreach (string path in GeneralUtility.ArrayCaptureImagesPath)
			{
				File.Delete(path);
			}
			GeneralUtility.ArrayCaptureImagesPath.Clear();
		}

		// Helper method để lấy ImageCodecInfo cho format cụ thể
		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}

		// Save ảnh với PNG (lossless) và DPI cao để giữ chất lượng tối đa
		public static void SaveImageWithHighQuality(Image image, string filePath)
		{
			// Tạo bitmap mới với DPI cao (300 DPI) để đảm bảo chất lượng in tốt
			Bitmap highDpiBitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
			highDpiBitmap.SetResolution(300f, 300f); // 300 DPI cho in ấn chất lượng cao
			
			using (Graphics g = Graphics.FromImage(highDpiBitmap))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
				g.DrawImage(image, 0, 0, image.Width, image.Height);
			}

			// Dùng PNG (lossless) thay vì JPEG để giữ chất lượng ảnh hoàn hảo
			highDpiBitmap.Save(filePath, ImageFormat.Png);
			highDpiBitmap.Dispose();
		}

		// Token: 0x04000008 RID: 8
		public static List<Bitmap> ArrayCaptureImages = new List<Bitmap>();

		// Token: 0x04000009 RID: 9
		public static List<string> ArrayCaptureImagesPath = new List<string>();
	}
}
