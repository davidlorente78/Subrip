using DALReaders;
using DomainServices;
using HSK;
using Recogzi;
using Recogzi.FileWriters;
using Segmentation;
using SubripCapture;
using SubripServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Subrip
{
	public partial class FormRip : Form
	{



		private ArrayList Included_Words = new ArrayList();
		private ArrayList _pinyinToPronunce = new ArrayList();
		private ArrayList _phrasesWords = new ArrayList();
		private ArrayList _dictionaryWords = new ArrayList();
		private ArrayList _common300Words = new ArrayList();
		private ArrayList _hsk1Words = new ArrayList();
		private ArrayList _hsk2Words = new ArrayList();
		private ArrayList HSK3_Words = new ArrayList();
		private ArrayList HSK4_Words = new ArrayList();
		private ArrayList HSK5_Words = new ArrayList();
		private ArrayList HSK6_Words = new ArrayList();


		private bool boolDictionary = false;

		private Point StartPositionPanel = new Point(0, 0); //Panel 3 Included
		private Int32 GeneratorButtonSize_Width = 45;
		private Int32 GeneratorButtonSize_Height = 45;		

		private FormRip m_InstanceRef = null;
		public FormRip InstanceRef
		{

			get
			{

				return m_InstanceRef;

			}
			set
			{

				m_InstanceRef = value;

			}

		}


		private Rectangle m_CaptureArea = new Rectangle(500, 1100, 400, 60);
		public Rectangle CaptureArea
		{

			get
			{

				return m_CaptureArea;

			}
			set
			{

				m_CaptureArea = value;

			}

		}

		Color SelectedColor = Color.Black;
		Int64[] Ranges = new Int64[1000];
		int x = 0;
		Projection projectionBitMapFilter = new Projection();

		Int32 numseg = 0; //Cursor para pintar en el panel 1 los detectados

        private readonly bool bcapture = true;

        public FormRip()
		{
			InitializeComponent();
			this.comboBoxScreen.SelectedIndex = 0;
			this.SelectedColor = Color.White;
			LoadWordsfromAssembly();

		}


		private void timer1_Tick(object sender, EventArgs e)
		{

			if (!bcapture)
			{
				Process();
			}
			else {

				FrameCapture();
				}
		}

		private int numFrame = 0;
        private void FrameCapture()
        {

			int Episode = 1;
			numFrame++;
			Bitmap bitmapFromScreen = BitmapService.BitmapFromScreen(CaptureArea.Width, CaptureArea.Height, CaptureArea.Left, CaptureArea.Top, this.comboBoxScreen.SelectedIndex);
			pictureBox1.Image = bitmapFromScreen;

			bitmapFromScreen.Save(@"D:\Addicted\" + Episode.ToString() + @"\" + numFrame.ToString() + ".jpg",ImageFormat.Jpeg);


		}

        private void button1_Click(object sender, EventArgs e)
		{
			Process();
		}

		private void Process()
		{

			Bitmap bitmapFromScreen = BitmapService.BitmapFromScreen(CaptureArea.Width, CaptureArea.Height, CaptureArea.Left, CaptureArea.Top, this.comboBoxScreen.SelectedIndex);
			pictureBox1.Image = bitmapFromScreen;
			

			projectionBitMapFilter = ProjectionService.ProjectandFilter(SelectedColor, Convert.ToInt32(this.numericUpDownColorMargin.Value), bitmapFromScreen);
			ProjectionsToChartSeries(projectionBitMapFilter);
			ProjectionsToChartSeries(projectionBitMapFilter);
			this.pictureBox2.Image = projectionBitMapFilter.Bitmap;

			projectionBitMapFilter.HorizontalSegments = ProjectionService.ToSegments(projectionBitMapFilter.HorizontalProjection, projectionBitMapFilter.Bitmap.Height);
			projectionBitMapFilter.VerticalSegments = new List<Segment>();

			if (SubtitlesDetected(projectionBitMapFilter.HorizontalSegments))
			{
				long Range = projectionBitMapFilter.HorizontalSegments[0].End - projectionBitMapFilter.HorizontalSegments[0].Starts;
				Int64 AverageRange = MathHelper.Average(Ranges);

				projectionBitMapFilter.VerticalSegments = ProjectionService.ToSegments(projectionBitMapFilter.VerticalProjection, projectionBitMapFilter.Bitmap.Height);

				if (projectionBitMapFilter.VerticalSegments.Count != 0)
				{
					projectionBitMapFilter = ProjectionService.MaxMinEvaluation(bitmapFromScreen.Height, projectionBitMapFilter);

					List<Segment> GroupedSegments = SegmentationService.GroupSegments(projectionBitMapFilter.VerticalSegments);

					Bitmap bitmapInitialSegments = (Bitmap)projectionBitMapFilter.Bitmap.Clone();
					Bitmap bitmapGroupedSegments = (Bitmap)projectionBitMapFilter.Bitmap.Clone();

					this.pictureBox2.Image = BitmapService.DrawSegmentsinBitmap(projectionBitMapFilter.VerticalSegments, bitmapInitialSegments, Brushes.DarkRed);
					this.pictureBoxGrouped.Image = BitmapService.DrawSegmentsinBitmap(GroupedSegments, bitmapGroupedSegments, Brushes.Orange);
					projectionBitMapFilter.CroppedBitmaps = BitmapService.ExtractCropBitmaps(GroupedSegments, projectionBitMapFilter.Bitmap);


					foreach (Bitmap crop in projectionBitMapFilter.CroppedBitmaps)
					{
						var margin = BitmapService.CorrectMargin(crop);

						projectionBitMapFilter.CorrectedMarginBitmaps.Add(margin);
					}

										
				

					List<char> predictions = new List<char>();

					foreach (Bitmap bitmap in projectionBitMapFilter.CorrectedMarginBitmaps)
					{
						Bitmap resized = BitmapService.ResizeImage(bitmap, 32,32);

						projectionBitMapFilter.ResizedBitmaps.Add(resized);

						string zerosandones = DatasetGenerator.ToZerosOnesSequence(' ', resized);

						var c = PredictionService.Predict(zerosandones);
						predictions.Add(c);

						x++;
					}

						BitmapsToScreen(projectionBitMapFilter.ResizedBitmaps);

					AddTextBoxToScreen(projectionBitMapFilter.CroppedBitmaps.Count,predictions);

				}
			}
		}

		private void AddTextBoxToScreen(int count, List<char> predictions)
		{

			this.panel2.Controls.Clear();
			for (int i = 0; i < count; i++)
			{
				TextBox tx = new TextBox
				{
					Width = 40,
					Height = 40,
					Font = new Font("DengXian", 28, FontStyle.Regular, GraphicsUnit.World),
					Name = i.ToString(),
					Top = 3,
					Left = 5 + i * 45,
					Text = predictions[i].ToString()
					

				};

				this.panel2.Controls.Add(tx);

			}
		}

		private void BitmapsToScreen(List<Bitmap> cropped)
		{
			this.panel1.Controls.Clear();
			numseg = 0;

			foreach (Bitmap bitmap in cropped)
			{

				PictureBox pc = new PictureBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					Height = 32,
					Width = 32,
					SizeMode = PictureBoxSizeMode.StretchImage,

					Top = 0,
					Left = numseg * Convert.ToInt32(34), //80 pixels de separacion entre controles
					Image = bitmap ,			
					
				};

				this.panel1.Controls.Add(pc);
				numseg++;

			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.numericUpDownRatioTh.Value = 0.78m; //Determinado mediante pruebas          
			
		}

		private void button2_Click(object sender, EventArgs e)
		{
			colorDialog1.Color = SelectedColor;
			colorDialog1.ShowDialog();
			SelectedColor = colorDialog1.Color;
		}

		//Dado un Bitmap y los segmentos localizados en el, traza los rectangulos que enmarcan los segmentos con el Brush y lo vuelca en pb que se pasa como parametro. 

		private bool SubtitlesDetected(List<Segment> HorizontalSegments)
		{
			return (HorizontalSegments.Count == 1);
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(this.numericUpDownInterval.Value) != 0)
				timer1.Interval = Convert.ToInt32(this.numericUpDownInterval.Value);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			timer1.Start();
		}


		private void button5_Click(object sender, EventArgs e)
		{
			timer1.Stop();
		}

		private void ProjectionsToChartSeries(Projection projectionBitOut)
		{
			chart1.Series["X"].Points.Clear();
			chart2.Series["Y"].Points.Clear();

			//En este punto estan calculadas las projecciones horizontal y vertical
			for (Int32 x = 0; x < projectionBitOut.VerticalProjection.Length; x++)
			{
				chart1.Series["X"].Points.AddXY(Convert.ToDouble(x), Convert.ToDouble(projectionBitOut.VerticalProjection[x]));
			}

			chart1.Series["X"].ChartType = SeriesChartType.FastLine;
			chart1.Series["X"].Color = Color.Red;

			for (Int32 y = 0; y < projectionBitOut.HorizontalProjection.Length; y++)
			{
				chart2.Series["Y"].Points.AddXY(Convert.ToDouble(y), Convert.ToDouble(projectionBitOut.HorizontalProjection[y]));
			}

			chart2.Series["Y"].ChartType = SeriesChartType.FastLine;
			chart2.Series["Y"].Color = Color.Black;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			//Add to dataset

			var textboxs = this.panel2.Controls;

			int samples = 0;
			foreach (TextBox tx in textboxs)
			{
				if (tx.Text != String.Empty)
				{
					//TO CHECK
					var zi = tx.Text;
					var i = Convert.ToInt32(tx.Name);
					var bit = projectionBitMapFilter.ResizedBitmaps[i];
					var sequence = DatasetGenerator.ToZerosOnesSequence(Convert.ToChar(zi), bit);

					projectionBitMapFilter.ResizedBitmaps[i].Save(Variables.datasetimagespath  + tx.Text + @".bmp");

					FileWriter.AddLine(sequence, Variables.datasetpath);
				}


			}

			MessageBox.Show(samples + " new samples have been added");

		}

        private void button6_Click(object sender, EventArgs e)
        {
			this.Hide();
			Form1 form1 = new Form1();
			form1.InstanceRef = this;			
			form1.Show();
		}

		private void btnTextAnalysis_Click(object sender, EventArgs e)
        {
			GenerateWith(0);
		}

        private string getInputText()
        {
            string inputText = "";

            var textboxs = this.panel2.Controls;

            foreach (TextBox tx in textboxs)
            {
                if (tx.Text != String.Empty)
                {
                    inputText = inputText + tx.Text;
                }

            }

            return inputText;
        }

        private void GenerateWith(Int32 option)
		{
			panel3.Controls.Clear();
			_pinyinToPronunce.Clear();

			Int32 FastCompute = 10;
			
			Int32 CaractersPerLine = panel1.Width / GeneratorButtonSize_Width - 1;

			string Input = getInputText();

			int Lenght = Input.Length;

			int textCursor = 0;

			Int32 Posx = 0;
			Int32 Posy = 0;

			while (textCursor < Input.Length)
			{
				ArrayList Matches = new ArrayList();
				Word BestMatch = new Word();

				Int32 MaxMatch = 0;

				for (Int32 x = 1; x < Input.Length - textCursor + 1; x++)
				{
					if (x > FastCompute) break; //Fast Compute


					if ((textCursor + x < Lenght) || (textCursor + x == Lenght))
					{
						foreach (Word w in Included_Words)
						{
							if (w.Character == Input.Substring(textCursor, x))
							{
								MaxMatch = x;
								Matches.Add(w);
								BestMatch = w;
							}


						}
					}
				} //End for Single Match

				int CountMatches = Matches.Count;

				if (CountMatches != 0)
				{
					Button btnWordMatch = new Button();
					btnWordMatch.Size = new Size(GeneratorButtonSize_Width * BestMatch.Character.Length, GeneratorButtonSize_Height);
					btnWordMatch.Text = BestMatch.Character;
					btnWordMatch.Location = new Point(StartPositionPanel.X + Posx * GeneratorButtonSize_Width,
						StartPositionPanel.Y + Posy * GeneratorButtonSize_Height);
					//btnWordMatch.Font = textBoxInputText.Font;

					//Puede que tenga mas de un pinyin para pronunciar

					if (BestMatch.NumberPinyin != null)
					{

						BestMatch.NumberPinyin = BestMatch.NumberPinyin.Replace("1", "1 ");
						BestMatch.NumberPinyin = BestMatch.NumberPinyin.Replace("2", "2 ");
						BestMatch.NumberPinyin = BestMatch.NumberPinyin.Replace("3", "3 ");
						BestMatch.NumberPinyin = BestMatch.NumberPinyin.Replace("4", "4 ");
						//No existe entrada de audio para el tono 5
						//BestMatch.NumberPinyin = BestMatch.NumberPinyin.Replace("5", "1 ");

						string[] numberPinyinDecomposed = BestMatch.NumberPinyin.Split(' ');

						foreach (string PinyinSillabe in numberPinyinDecomposed)
						{
							Word_Phrase toPronunce = new Word_Phrase();
							toPronunce.NumberPinyin = PinyinSillabe.Trim();
							this._pinyinToPronunce.Add((toPronunce));

						}
					}


					switch (BestMatch.Level)
					{

						case 0:
							btnWordMatch.BackColor = Color.GreenYellow;
							break;
						case 1: //HSK1
							btnWordMatch.BackColor = Color.LimeGreen;
							break;
						case 2:
							btnWordMatch.BackColor = Color.Green;
							break;
						case 3:
							btnWordMatch.BackColor = Color.Aqua;
							break;
						case 4:
							btnWordMatch.BackColor = Color.Blue;
							break;
						case 5:
							btnWordMatch.BackColor = Color.Orange;
							break;
						case 6:
							btnWordMatch.BackColor = Color.DarkOrange;
							break;

						case 7:
							btnWordMatch.BackColor = Color.Crimson;
							break;

						case 20:
							btnWordMatch.BackColor = Color.MediumVioletRed;
							break;



						default:
							btnWordMatch.ForeColor = Color.Red;
							break;
					}


					this.panel3.Controls.Add((btnWordMatch));

					if (option == 0) this.toolTip1.SetToolTip(btnWordMatch, "[" + BestMatch.Level.ToString() + "]" + "\n[" + BestMatch.Pinyin + "]\n" + BestMatch.Description);

					if (option == 1) this.toolTip1.SetToolTip(btnWordMatch, BestMatch.Pinyin);


					//string[] descriptions = BestMatch.Description.Split(';');

					//this.textBoxTranslated.Text = this.textBoxTranslated.Text + " " + descriptions[0];

					Posx = Posx + BestMatch.Character.Length;

					if (Posx > CaractersPerLine)
					{
						Posy++;
						Posx = 0;
					}

					textCursor = textCursor + MaxMatch;
				}

				else
				{
					//No hay match


					Button WordMatch = new Button();
					WordMatch.Size = new Size(GeneratorButtonSize_Width, GeneratorButtonSize_Height);
					WordMatch.Text = Input[textCursor].ToString();
					WordMatch.Location = new Point(StartPositionPanel.X + Posx * GeneratorButtonSize_Width,
						StartPositionPanel.Y + Posy * GeneratorButtonSize_Height);
					//WordMatch.Font = textBoxInputText.Font;
					WordMatch.ForeColor = Color.Red;

					this.panel3.Controls.Add((WordMatch));
					this.toolTip1.SetToolTip(WordMatch, "Not Match Found");


					Posx = Posx + 1;

					if (Posx > CaractersPerLine)
					{
						Posy++;
						Posx = 0;
					}

					textCursor = textCursor + 1;
				}
			} //While Cursor
		}


		private void LoadWordsfromAssembly()
		{

			Reader_Phrases phr = new Reader_Phrases();
			_phrasesWords = phr.AllWords();


			Reader_Dictionary dic = new Reader_Dictionary();
			_dictionaryWords = dic.AllWords();

			//ArrayList see = dic.Words_Description_Contains("see");
			//ArrayList shang  = dic.Words_Description_Contains("上");

			_hsk1Words = Load_HSK(1);
			_hsk2Words = Load_HSK(2);
			HSK3_Words = Load_HSK(3);
			HSK4_Words = Load_HSK(4);
			HSK5_Words = Load_HSK(5);
			HSK6_Words = Load_HSK(6);

			Reader_Common300 csv = new Reader_Common300();
			_common300Words = csv.AllWords();
			Load_Included();
			
		}
		private ArrayList Load_HSK(int i)
		{
			Reader_HSK hsk = new Reader_HSK();
			hsk.Level = i;
			return hsk.AllWords();
		}
		private void Load_Included()
		{
			Included_Words.Clear();

			//Exclude for test Purposes
			if (boolDictionary)
			{

				foreach (Word_Dictionary w in _dictionaryWords)
				{
					Included_Words.Add(w);
				}
			}

			foreach (Word_HSK w in _hsk1Words)
			{
				Included_Words.Add(w);
			}

			foreach (Word_Phrase w in _phrasesWords)
			{
				if (w.Level == 11) Included_Words.Add(w);
			}

			foreach (Word_HSK w in _hsk2Words)
			{
				Included_Words.Add(w);
			}

			foreach (Word_Phrase w in _phrasesWords)
			{
				if (w.Level == 12) Included_Words.Add(w);
			}

			foreach (Word_HSK w in HSK3_Words)
			{
				Included_Words.Add(w);
			}

			foreach (Word_Phrase w in _phrasesWords)
			{
				if (w.Level == 13) Included_Words.Add(w);
			}

			foreach (Word_HSK w in HSK4_Words)
			{
				Included_Words.Add(w);
			}

			foreach (Word_HSK w in HSK5_Words)
			{
				Included_Words.Add(w);
			}

			foreach (Word_HSK w in HSK6_Words)
			{
				Included_Words.Add(w);
			}


			foreach (Word_Common300 w in _common300Words)
			{
				Included_Words.Add(w);
			}

			Word Coma = new Word();
			Coma.Description = "";
			Coma.Character = ",";
			Coma.Pinyin = "";

			Word AnotherComa = new Word();
			AnotherComa.Description = "";
			AnotherComa.Character = "，";
			AnotherComa.Pinyin = "";


			Word Point = new Word();
			Point.Description = "";
			Point.Character = ".";
			Point.Pinyin = "";

			Word ChinesePoint = new Word();
			ChinesePoint.Description = "";
			ChinesePoint.Character = "。";
			ChinesePoint.Pinyin = "";

			Included_Words.Add(Coma);
			Included_Words.Add(AnotherComa);
			Included_Words.Add(Point);
			Included_Words.Add(ChinesePoint);

		}
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
			{
				boolDictionary = !boolDictionary;
				Load_Included();
			}


		private static List<Word> ToList(ArrayList arrayList)
		{
			List<Word> list = new List<Word>();
			foreach (Word instance in arrayList)
			{
				list.Add(instance);
			}
			return list;
		}


		private void button3_Click_1(object sender, EventArgs e)
        {
            JPGGenerator();

        }

        private void JPGGenerator()
        {
            ListService service = new ListService(new Reader_HSK());

            var includedWords = service.GetAllWords(1);

            List<Word> arrayIncluded = ToList(includedWords);

            int index = 1;

            foreach (Word_HSK w in arrayIncluded.OrderBy(x => x.NumberPinyin))

            {
                //FANGSONG
                //	KAITI
                //	SIMHEI
                //DengXian

                string sIndex = NumeroCuadrado(index, 3);

                BitmapService.GenerateBitmapfromFontChar(w.Character, "FangSong", FontStyle.Regular, sIndex);
                BitmapService.GenerateBitmapfromFontChar(w.Character, "Kaiti", FontStyle.Regular, sIndex);

                index++;


            }
        }

        private string NumeroCuadrado(int index, int v)
        {
			string s = index.ToString();
			string sIndex = string.Empty;

			for (x = 0; x < v - s.Length; x++) sIndex = sIndex + "0";

			sIndex = sIndex + s;

			return sIndex;
        }
    }
}
