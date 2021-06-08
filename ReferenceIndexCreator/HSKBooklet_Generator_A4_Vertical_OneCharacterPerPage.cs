using DALReaders;
using DomainServices;
using HSK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Layer;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Font;
using iText.Kernel.Utils;

namespace ReferenceIndexCreator
{

    /// <summary>
    /// Utilizado para generar Cuadernos HSK
    /// A4 en Vertical
    /// Plantilla de un cuaderno por hoja
    /// </summary>
    public class HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage
    {

        const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
        //PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true, true);

        PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

        CharService charservice = new CharService();
        ListService listservice = new ListService(new Reader_HSK());
        List<Word> includedChars = new List<Word>();
        ArrayList includedWords = new ArrayList();
        int Maxrepeticion = 0;

        int Level = 1;
        public HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage(int Level, bool Acumulative)
        {

            this.Level = Level;

            //Obtener Caracteres          
            includedChars = charservice.GetChars(Level);

            //Obtener palabras 
            includedWords = listservice.GetAllWords(Level);


        }

        public void ConvertToSinglePDFPages(int level)
        {
            PdfReader reader = new PdfReader(@"D:\\HSKBooklet\Final Print Level " + Level.ToString() + ".pdf");

            PdfDocument pdfSource = new PdfDocument(reader);
            Document document = new Document(pdfSource, PageSize.A4);
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

                       
            for (int x = 1; x < pdfSource.GetNumberOfPages()+1; x++)
            {
                                
                PdfWriter writter = new PdfWriter(@"D:\\HSKBooklet\Pages\" +  Level.ToString() + @"\Final_HSK_" + Level.ToString() + "_" + this.NumeroCuadrado(x,3)+".pdf");
                PdfDocument pdfwritter = new PdfDocument(writter);
                PdfMerger merger = new PdfMerger(pdfwritter);


                PdfPage page = pdfSource.GetPage(x);
                Document pdfDestination = new Document(pdfwritter, PageSize.A4);

                merger.Merge(pdfSource, x, x);
                pdfwritter.Close();

            }



        }

        public void AddCaracterPage(string c, PdfDocument pdf, Document document, string sIndex)
        {

            //GENERAR CARACTER
            SaveBitmapfromFontChar(c, "Kaiti", FontStyle.Regular, sIndex);
            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());

            Canvas canvas = new Canvas(pdfCanvas, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));


            //Layer 1
            PdfLayer pdflayer1 = new PdfLayer("Layer 1", pdf);
            pdflayer1.SetOn(true);
            pdfCanvas.BeginLayer(pdflayer1);

            ///Template IMG        
            string template = @"D:\Portada\Cuadernos\PNG A4\Mizige DIN A4 Template HSK - Pie Descripcion 2 lines more.png";

            ImageData data = ImageDataFactory.Create(template);
            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(template))
            .ScaleToFit(PageSize.A4.GetWidth(), PageSize.A4.GetHeight())
            .SetFixedPosition(0, 0);
            canvas.Add(img);
            pdfCanvas.EndLayer();

            //Layer 2 CARACTER GRANDE
            PdfLayer pdflayer2 = new PdfLayer("Layer 2", pdf);
            pdflayer2.SetOn(true);
            pdfCanvas.BeginLayer(pdflayer2);

            //Para poner una imagen con 300 ppp, use una escala de 72 / 300 = 24 % .Por ejemplo:
            //si tiene una imagen de 5 x 5 pulgadas que escanea a 300 ppp, la
            //imagen resultante es de 1500 x 1500 píxeles(5 x 300 = 1500).Cuando coloca esta imagen en
            //el pdf con una escala del 24 % (72 / 300 = 0.24), la imagen en el pdf será de
            //5X5 pulgadas con 1500X1500 píxeles a 300 dpi.La imagen siempre será de
            //1500X1500 píxeles sea cual sea el tamaño.

            int LeftMarginMiZige = 114 * 72 / 300 - 20;
            int UpMarginMiZige = 114 * 72 / 300 - 5;
            int CaracterSize = 180;

            //Este es el path con el que se ha grabado la imagen previamente
            string path = @"D:\HSKBooklet\" + sIndex + "_" + c.ToString() + ".png";

            iText.Layout.Element.Image MainCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path))
           .SetFixedPosition(LeftMarginMiZige, PageSize.A4.GetHeight() - CaracterSize - UpMarginMiZige)
           .ScaleAbsolute(CaracterSize, CaracterSize)
            .SetTextAlignment(TextAlignment.CENTER);
            canvas.Add(MainCaracter);
            pdfCanvas.EndLayer();





            //Layer 4 CARACTER PEQUEÑOS FOR
            PdfLayer pdflayer4 = new PdfLayer("Layer 4", pdf);
            pdflayer4.SetOn(true);
            pdfCanvas.BeginLayer(pdflayer4);



            //int MaxColumns = 17;
            //int MaxRows = 26;
            int MiniCaracterSize = 30;
            int MaxColumns = 17;
            int MaxRows = 17;

            float width = 30.6F;
            float height = 30.7F;
            int InitialLeftMargin = 35;
            int UpMarginMiZigeMini = 22;

            float left = 0;
            float bottom = 0;

            iText.Layout.Element.Image MiniCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path));
            MiniCaracter.ScaleAbsolute(MiniCaracterSize, MiniCaracterSize);
            MiniCaracter.SetTextAlignment(TextAlignment.CENTER);
            MiniCaracter.SetTextAlignment(TextAlignment.CENTER);

            for (int col = 0; col < MaxColumns; col++)

            {
                for (int row = 0; row < MaxRows; row++)
                {
                    //No se imprimen donde esta en caracter grande situado
                    if (!(row < 5 && col < 5))
                    {
                        left = 35 + col * width;
                        bottom = PageSize.A4.GetHeight() - MiniCaracterSize - 2 - UpMarginMiZigeMini - row * height;

                        //iText.Layout.Element.Image MiniCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path))
                        MiniCaracter.SetFixedPosition(left, bottom);
                        canvas.Add(MiniCaracter);
                    }

                }
            }
            pdfCanvas.EndLayer();

            //INFORMACION CARACTER

            //Se incluyen los caracteres no las palabras
            List<Word> arrayIncluded = ToList(includedWords);


            document.SetFont(fontKaiti);

            var dimensionsColumns = new float[] { 1, 1, 1, 1, 1, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(dimensionsColumns));
            table.UseAllAvailableWidth();

            PdfLayer pdflayerInfo = new PdfLayer("pdflayerInfo", pdf);
            pdflayerInfo.SetOn(true);
            pdfCanvas.BeginLayer(pdflayerInfo);
            int repeticion = 0;

            List<Paragraph> pGlobals = new List<Paragraph>();
            foreach (Word_HSK word in arrayIncluded)
            {
                //Add row to table
                if (word.Character.Contains(c))
                {
                    repeticion++;

                    Paragraph pGlobal = new Paragraph(word.Character + "\t (" + word.Pinyin + ") , " + word.Description).SetFont(fontKaiti).SetFontSize(10);


                    pGlobals.Add(pGlobal);

                }
            }


            if (repeticion > Maxrepeticion)
            {
                Maxrepeticion = repeticion;
                repeticion = 0;
                int checkLevel = Level;
            }

            int rowspan = 1;
            int colspan;


            int PalabrasPorFila = 0;
            if (pGlobals.Count <= 4)
            {
                colspan = 6;
                PalabrasPorFila = 1;
            }
            else if ((pGlobals.Count > 4) && (pGlobals.Count <= 8))
            {
                colspan = 3;
                PalabrasPorFila = 2;
            }
            else
            {
                colspan = 2;
                PalabrasPorFila = 3;
            }


            //En blanco antes del numero de pagina        

            double Count = pGlobals.Count;
            double doubleplarba = PalabrasPorFila;
            int NumeroFilas = (int)Math.Ceiling(Count / PalabrasPorFila);


            int nwhite = 4 - NumeroFilas;

            int Height = 14;
            int Columns = 6; //Se utiliza para graduar al colspan anterior 
            //Celdas en blanco hasta el final del documento
            int FilasBlancasAntesdepalabras = 33 + 4; //Al añadir dos filas mas a la plantilla

            for (int x = 0; x < FilasBlancasAntesdepalabras * Columns; x++)
            {
                Cell cellWhite = new Cell();  // Creating a cell                         
                cellWhite.SetHeight(Height).Add(new Paragraph(" "));
                table.AddCell(cellWhite);      // Adding cell to the table  
            }

            foreach (Paragraph p in pGlobals)
            {
                Cell cellGlobal = new Cell(rowspan, colspan);
                cellGlobal.SetHeight(Height).Add(p);
                table.AddCell(cellGlobal);
            }

            RemoveBorder(table);
            canvas.Add(table);


            Table tableNumberPage = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));

            tableNumberPage.UseAllAvailableWidth();

            for (int x = 0; x < nwhite; x++)
            {
                Cell cellWhite = new Cell();  // Creating a cell                         
                cellWhite.SetHeight(Height).Add(new Paragraph(" ").SetHeight(Height));
                tableNumberPage.AddCell(cellWhite);      // Adding cell to the table  
            }

            Cell cellNumberPage = new Cell();   // Creating a cell                         
            cellNumberPage.SetHeight(Height).Add(new Paragraph(sIndex).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetFont(fontKaiti));
            tableNumberPage.AddCell(cellNumberPage);      // Adding cell to the table  

            RemoveBorder(tableNumberPage);
            canvas.Add(tableNumberPage);
            pdfCanvas.EndLayer();
        }

        private static void RemoveBorder(Table table)
        {
            foreach (IElement iElement in table.GetChildren())
            {
                ((Cell)iElement).SetBorder(iText.Layout.Borders.SolidBorder.NO_BORDER);
            }
        }

        public void DocumentGenerator()
        {
            //Se incluyen los caracteres no las palabras
            List<Word> arrayIncluded = ToList(includedWords);

            int index = 1;
            //Hay que obtener caracter a caracter no las palabras TODO   
            //LEVELPATH 12 123 1234 

            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\" + Level.ToString() + "_HSK.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            //TABLA DE CONTENIDO
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
            table.UseAllAvailableWidth();

            foreach (Word character in includedChars.OrderBy(x => x.Pinyin))
            {
                string sIndex = NumeroCuadrado(index, 3);
                this.AddCaracterPage(character.Character, pdf, document, sIndex);
                index++;
            }

            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (index % 2 == 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            }
            //Canvas canvas = new Canvas(pdfCanvas, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));

            document.Close();
        }

        public void CharacterIndexGenerator()
        {
            //Se incluyen los caracteres no las palabras
            List<Word> arrayIncluded = ToList(includedWords);

            int index = 0;

            //Hay que obtener caracter a caracter no las palabras TODO

            //INICIO DOCUMENTO PDF
            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\" + Level.ToString() + "CaracterIndex.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            //PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true, true);
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            document.SetFont(fontKaiti);

            //TABLA DE CONTENIDO

            var DimensionesColumnas = new float[] { 5, 20, 20, 50, 10, 15 };
            Table table = new Table(UnitValue.CreatePercentArray(DimensionesColumnas));
            table.UseAllAvailableWidth();

            //TEST CON SOLO CINCO CARACTERES
            //includedChars.Clear();
            //includedChars.Add(new Word { Character = "电" });
            //includedChars.Add(new Word { Character = "出" });
            //includedChars.Add(new Word { Character = "永" });
            //includedChars.Add(new Word { Character = "一" });
            //includedChars.Add(new Word { Character = "问" });

            //Se usara para el salto de pagina
            int rowCount = 0;
            int SetMarginTopP = 10;
            int SetHeightP = 30;
            //Caracteres ordenados alfabeticamente
            var OrderedChars = includedChars.OrderBy(x => x.Pinyin);
            foreach (Word character in OrderedChars)

            {
                index++;
                bool first = true;
                foreach (Word_HSK word in arrayIncluded)
                {
                    //Add row to table
                    if (word.Character.Contains(character.Character))
                    {

                        //El caracter aparece en la palabra word
                        //En la pagina index

                        // Adding cell 1 to the table 
                        if (first)
                        {
                            Cell cell1 = new Cell();   // Creating a cell                         
                            cell1.Add(new Paragraph(character.Character).SetHeight(SetHeightP).SetFont(fontKaiti).SetBold().SetFontSize(20).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                            table.AddCell(cell1);      // Adding cell to the table  

                            first = false;
                        }
                        else
                        {
                            Cell cell1 = new Cell();
                            cell1.Add(new Paragraph(" ").SetHeight(SetHeightP).SetFont(fontKaiti).SetBold().SetFontSize(30).SetTextAlignment(TextAlignment.LEFT));
                            table.AddCell(cell1);

                        }
                        //Palabra que lo usa 
                        Cell cell2 = new Cell();
                        cell2.Add(new Paragraph(word.Character).SetFont(fontKaiti).SetFontSize(20).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell2);

                        Cell cellPinyin = new Cell();
                        cellPinyin.Add(new Paragraph(word.Pinyin).SetFont(fontKaiti).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cellPinyin);

                        Cell cell3 = new Cell();
                        cell3.Add(new Paragraph(word.Description).SetFontSize(10).SetMarginLeft(20).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell3);

                        Cell cell4 = new Cell();
                        cell4.Add(new Paragraph("HSK " + word.Level.ToString()).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell4);

                        Cell cell5 = new Cell();
                        cell5.Add(new Paragraph("Pag. " + index.ToString()).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell5);

                        rowCount++;

                        //Salto de linea
                        if (rowCount == 21)
                        {

                            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
                            Canvas canvas = new Canvas(pdfCanvas, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));
                            canvas.Add(table);

                            rowCount = 0;

                            table = new Table(UnitValue.CreatePercentArray(DimensionesColumnas));
                            table.UseAllAvailableWidth();
                        }
                    }
                }


                string sIndex = NumeroCuadrado(index, 3);
            }


            PdfCanvas pdfCanvasend = new PdfCanvas(pdf.AddNewPage());
            Canvas canvasend = new Canvas(pdfCanvasend, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));
            canvasend.Add(table);



            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (pdf.GetNumberOfPages() % 2 == 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            }

            document.Close();
        }

        public void WordIndexGenerator()
        {

            //Se incluyen los caracteres no las palabras
            List<Word> arrayIncluded = ToList(includedWords);

            int index = 1;

            //Hay que obtener caracter a caracter no las palabras TODO


            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\" + Level.ToString() + "WordIndex.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            //PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true, true);
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            document.SetFont(fontKaiti);


            var dimensionsColumns = new float[] { 25, 20, 20, 50, 10, 15 };

            Table table = new Table(UnitValue.CreatePercentArray(dimensionsColumns));
            table.UseAllAvailableWidth();

            int rowCount = 0;

            var OrderedChars = includedChars.OrderBy(x => x.Pinyin);

            var ArrayChars = OrderedChars.ToArray();

            //Palabra ordenadas alfabeticamente
            foreach (Word_HSK word in arrayIncluded)
            {
                bool first = true;

                foreach (char ch in word.Character.ToCharArray().Distinct())
                {
                    int Page = 0;
                    for (int x = 0; x < OrderedChars.Count(); x++)
                    {
                        if (ArrayChars.ElementAt(x).Character == ch.ToString())
                        {
                            Page = x;
                            break;

                        }
                    }

                    //El caracter aparece en la palabra word
                    //En la pagina index

                    // Adding cell 1 to the table 

                    int SetMarginTopP = 10;
                    int SetHeightP = 30;

                    //Shhh Esta chapuza es para que la descripcion no se repita tampoco
                    if (first)
                    {
                        Cell cell1 = new Cell();   // Creating a cell                         
                        cell1.Add(new Paragraph(word.Character).SetHeight(SetHeightP).SetFont(fontKaiti).SetBold().SetFontSize(20).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell1); // Adding cell to the table  


                        Cell cellPinyin = new Cell();
                        cellPinyin.Add(new Paragraph(word.Pinyin).SetFont(fontKaiti).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cellPinyin);

                        //Palabra que lo usa 
                        Cell cell2 = new Cell();
                        cell2.Add(new Paragraph(ch.ToString()).SetFont(fontKaiti).SetFontSize(20).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell2);

                        Cell cell3 = new Cell();
                        cell3.Add(new Paragraph(word.Description).SetFontSize(10).SetMarginLeft(20).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell3);

                        first = false;

                    }
                    else
                    {
                        Cell cell1 = new Cell();   // Creating a cell                         
                        cell1.Add(new Paragraph(" ").SetHeight(SetHeightP).SetFont(fontKaiti).SetBold().SetFontSize(20).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell1);

                        Cell cellPinyin = new Cell();
                        cellPinyin.Add(new Paragraph(word.Pinyin).SetFont(fontKaiti).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cellPinyin);

                        //Palabra que lo usa 
                        Cell cell2 = new Cell();
                        cell2.Add(new Paragraph(ch.ToString()).SetFont(fontKaiti).SetFontSize(20).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell2);

                        Cell cell3 = new Cell();
                        cell3.Add(new Paragraph(" ").SetFontSize(10).SetMarginLeft(20).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cell3);


                    }

                    Cell cell4 = new Cell();
                    cell4.Add(new Paragraph("HSK " + word.Level.ToString()).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    table.AddCell(cell4);

                    Cell cell5 = new Cell();
                    cell5.Add(new Paragraph("Pag. " + (Page + 1).ToString()).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    table.AddCell(cell5);

                    rowCount++;

                    if (rowCount == 21)
                    {
                        PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
                        Canvas canvas = new Canvas(pdfCanvas, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));
                        canvas.Add(table);

                        rowCount = 0;
                        table = new Table(UnitValue.CreatePercentArray(dimensionsColumns));
                        table.UseAllAvailableWidth();
                    }
                }


                string sIndex = NumeroCuadrado(index, 3);


            }


            PdfCanvas pdfCanvasend = new PdfCanvas(pdf.AddNewPage());
            Canvas canvasend = new Canvas(pdfCanvasend, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));
            canvasend.Add(table);

            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (pdf.GetNumberOfPages() % 2 != 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            }

            document.Close();
        }

        //Graba en disco en caracter indicado c
        private void SaveBitmapfromFontChar(string c, string FontName, FontStyle fontStyle, string sIndex)
        {
            Int32 fontPoints = 430;

            //Mizige
            Size size = new Size(514, 514);

            var MaxSize = Math.Max(size.Width, size.Height);
            Bitmap bitmapchar = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(bitmapchar);

            //FangSong KaiTi
            Font f = new Font(FontName, fontPoints, fontStyle, GraphicsUnit.World);
            Brush brushblack = Brushes.Black;
            Brush brushwhite = Brushes.White;
            Brush brushLightGray = Brushes.LightGray;

            int Correct = 22;
            System.Drawing.Point p = new System.Drawing.Point(Correct, 0);
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, Math.Max(size.Width, size.Height), Math.Max(size.Width, size.Height));

            Region reg = new Region(rec);

            graphic.FillRegion(brushwhite, reg);
            graphic.DrawString(c.ToString(), f, brushLightGray, p);

            //Transparencia para el png . Blanco transparente
            bitmapchar.MakeTransparent(Color.White);
            bitmapchar.Save(@"D:\HSKBooklet\" + sIndex + "_" + c.ToString() + ".png", ImageFormat.Png);


        }

        private string NumeroCuadrado(int index, int v)
        {
            string s = index.ToString();
            string sIndex = string.Empty;

            for (int x = 0; x < v - s.Length; x++) sIndex = sIndex + "0";

            sIndex = sIndex + s;

            return sIndex;
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



        public void FinalGroup(int Level)
        {

            //D:\Portada\HSKBooklets

            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\Final_HSK_" + Level.ToString() + ".pdf");

            PdfDocument pdf = new PdfDocument(new PdfWriter(writer));
            PdfMerger merger = new PdfMerger(pdf);
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);


            pdf.AddFont(fontKaiti);

            //Interior Cover

            //Add pages from the first document
            PdfDocument InteriorCoverPdf = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\Interior Cover Level " + Level.ToString() + ".pdf"));
            //From to
            merger.Merge(InteriorCoverPdf, 1, InteriorCoverPdf.GetNumberOfPages());



            //White White Page A4.pdf
            PdfDocument IWhitePdf = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\White Page A4.pdf"));
            //From to
            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //HSK Booklets D:\HSKBooklet 1_HSK
            PdfDocument bookletHSK = new PdfDocument(new PdfReader(@"D:\HSKBooklet\" + Level.ToString() + "_HSK.pdf"));
            //From to
            merger.Merge(bookletHSK, 1, bookletHSK.GetNumberOfPages());


            //Word Index Cover "Word Index Level 3.pdf"
            PdfDocument wordIndexCover = new PdfDocument(new PdfReader(@"D:\\Portada\\HSKBooklets\Word Index Level " + Level.ToString() + ".pdf"));
            //From to
            merger.Merge(wordIndexCover, 1, wordIndexCover.GetNumberOfPages());

            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //Word Index 

            PdfDocument wordIndex = new PdfDocument(new PdfReader(@"D:\\HSKBooklet\" + Level.ToString() + "WordIndex.pdf"));
            //From to
            merger.Merge(wordIndex, 1, wordIndex.GetNumberOfPages());


            //Char Cover "Character Index Level 1.pdf"
            PdfDocument CharacterIndexCover = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\Character Index Level " + Level.ToString() + ".pdf"));
            //From to
            merger.Merge(CharacterIndexCover, 1, CharacterIndexCover.GetNumberOfPages());

            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //Char Index

            PdfDocument caracterIndex = new PdfDocument(new PdfReader(@"D:\\HSKBooklet\" + Level.ToString() + "CaracterIndex.pdf"));
            //From to
            merger.Merge(caracterIndex, 1, caracterIndex.GetNumberOfPages());


            PdfDocument FinalCover = new PdfDocument(new PdfReader(@"D:\Portada\HSKBooklets\End Cover.pdf"));
            merger.Merge(FinalCover, 1, FinalCover.GetNumberOfPages());

            InteriorCoverPdf.Close();
            IWhitePdf.Close();

            bookletHSK.Close();
            wordIndexCover.Close();

            InteriorCoverPdf.Close();
            wordIndex.Close();
            CharacterIndexCover.Close();
            caracterIndex.Close();
            FinalCover.Close();
            pdf.Close();


        }

        //Metodo de prueba que se utilizo para crear una pagina del documento

    }
}
