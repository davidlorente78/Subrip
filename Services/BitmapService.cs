using Segmentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace SubripServices
{
	public static class BitmapService
	{
		public static Bitmap BitmapFromScreen(int sizex, int sizey, int left, int top, int screen)
		{
			Rectangle region = Screen.AllScreens[screen].Bounds;
			Bitmap bitmap = new Bitmap(sizex, sizey);

			Graphics graphic = Graphics.FromImage(bitmap);
			Size s = new Size(sizex, sizey);

			graphic.CopyFromScreen(region.Left + left, region.Top + top, 0, 0, s); //Top Left Change

			return bitmap;
		}

		public static Bitmap DrawSegmentsinBitmap(List<Segment> Segments, Bitmap bitmap, Brush color)
		{
			Pen pen = new Pen(color);
			Graphics graphic = Graphics.FromImage(bitmap);
			foreach (Segment s in Segments)
			{
				Point p = new Point(Convert.ToInt32(s.Starts), Convert.ToInt32(s.MinValue));
				Size size = new Size(Convert.ToInt32(s.End - s.Starts), Convert.ToInt32(Convert.ToInt32(s.MaxValue - s.MinValue)));
				Rectangle rec = new Rectangle(p, size);
				graphic.DrawRectangle(pen, rec);
			}

			return bitmap;
		}

		public static List<Bitmap> ExtractCropBitmaps(List<Segment> Segments, Bitmap bitmap)
		{


			List<Bitmap> cropped = new List<Bitmap>();
			try
			{
				foreach (Segment s in Segments)
				{
					Point p = new Point(Convert.ToInt32(s.Starts) , Convert.ToInt32(s.MinValue) );
					Size size = new Size(Convert.ToInt32(s.End - s.Starts) , Convert.ToInt32(Convert.ToInt32(s.MaxValue - s.MinValue)) );
					Rectangle rec = new Rectangle(p, size);

					cropped.Add(bitmap.Clone(rec, PixelFormat.Format64bppArgb));
				}
			}
			catch
			{ //TODO}

				
			}
			return cropped;
		}

		public static Bitmap ResizeImage(Bitmap source, int width, int height)
		{
			Bitmap result = null;

			try
			{
				if (source.Width != width || source.Height != height)
				{
					// Resize image
					float sourceRatio = (float)source.Width / source.Height;

					using (var target = new Bitmap(width, height))
					{
						using (var g = System.Drawing.Graphics.FromImage(target))
						{
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							g.SmoothingMode = SmoothingMode.HighQuality;

							// Scaling
							float scaling;
							float scalingY = (float)source.Height / height;
							float scalingX = (float)source.Width / width;
							if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

							int newWidth = (int)(source.Width / scaling);
							int newHeight = (int)(source.Height / scaling);

							// Correct float to int rounding
							if (newWidth < width) newWidth = width;
							if (newHeight < height) newHeight = height;

							// See if image needs to be cropped
							int shiftX = 0;
							int shiftY = 0;

							if (newWidth > width)
							{
								shiftX = (newWidth - width) / 2;
							}

							if (newHeight > height)
							{
								shiftY = (newHeight - height) / 2;
							}

							// Draw image
							g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
						}

						result = (Bitmap)target.Clone();
					}
				}
				else
				{
					// Image size matched the given size
					result = (Bitmap)source.Clone();
				}
			}
			catch (Exception)
			{
				result = null;
			}

			return result;
		}

		public static Bitmap GenerateCenteredBitmapfromCropped(Bitmap bitmap, int size)
			{
				Bitmap bitmapwhite = new Bitmap(size, size, PixelFormat.Format64bppArgb);
				Graphics graphic = Graphics.FromImage(bitmapwhite);
				Point p = new Point((size - bitmap.Height) / 2, (size - bitmap.Height) / 2);
				graphic.DrawImage(bitmap, p);
				return bitmapwhite;
			}

		public static Bitmap CorrectMargin(Bitmap crop)
		{
			Bitmap marginCorrected = new Bitmap(crop.Width + 20, crop.Height + 20);
			Graphics graphic = Graphics.FromImage(marginCorrected);
			Point p = new Point(9, 5);
			graphic.DrawImage(crop, p);
			return marginCorrected;

		}

		public static void GenerateBitmapfromFontChar(string c,  string FontName, FontStyle fontStyle, string sIndex)
		{

			Int32 fontPoints = 575;

			
			//Size size = new Size(2000 * c.Length + 5, 2000);

			Size size = new Size(1920,1280);

			var MaxSize = Math.Max(size.Width, size.Height);
			Bitmap bitmapchar = new Bitmap(size.Width, size.Height);
			Graphics graphic = Graphics.FromImage(bitmapchar);


			//FangSong KaiTi


			Font f = new Font(FontName, fontPoints, fontStyle, GraphicsUnit.World);
			Brush b = Brushes.Black;
			Brush brushwhite = Brushes.White;

			int Correct = 0;

			if (c.Length == 3) Correct = 0;
			else if (c.Length == 2) Correct = 375;
			else if (c.Length == 1) Correct = 600;
			else throw new Exception();

			Point p = new Point(Correct, 265);
			Rectangle rec = new Rectangle(0, 0, Math.Max(size.Width, size.Height), Math.Max(size.Width, size.Height));
			Region reg = new Region(rec);

			graphic.FillRegion(brushwhite, reg);
			graphic.DrawString(c.ToString(), f, b, p);

			ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg"); ;
			EncoderParameter encCompressionrParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionNone); ;

			//100L sin compresion
			EncoderParameter encQualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
			EncoderParameters myEncoderParameters = new EncoderParameters(2);
			myEncoderParameters.Param[0] = encCompressionrParameter;
			myEncoderParameters.Param[1] = encQualityParameter;
			
			bitmapchar.Save(@"D:\HSK1\HD\" + FontName + @"\" + sIndex + "_" + c.ToString() + ".jpg", myImageCodecInfo, myEncoderParameters);




		}

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}
    }
	}

