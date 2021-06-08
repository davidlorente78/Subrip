using Segmentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subrip
{
	public class Projection
	{
		Bitmap bitmap;
		private Int64[] verticalProjection;
		private Int64[] horizontalProjection;
		private Int32[] maxColumnValue;
		private Int32[] minColumnValue;

		public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
		public long[] VerticalProjection { get => verticalProjection; set => verticalProjection = value; }
		public long[] HorizontalProjection { get => horizontalProjection; set => horizontalProjection = value; }
		public Int32[] MaxColumnValue { get => maxColumnValue; set => maxColumnValue = value; }
		public Int32[] MinColumnValue { get => minColumnValue; set => minColumnValue = value; }
		public List<Segment> HorizontalSegments { get; set; } = new List<Segment>();
		public List<Segment> VerticalSegments { get; set; } = new List<Segment>();
		public List<Bitmap> CroppedBitmaps { get; set; } = new List<Bitmap>();
		public List<Bitmap> ResizedBitmaps { get; set; } = new List<Bitmap>();
        public List<Bitmap> CorrectedMarginBitmaps { get; set; } = new List<Bitmap>();
	}
}
