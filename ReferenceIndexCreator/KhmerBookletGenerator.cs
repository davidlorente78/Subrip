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
using DomainHSK.Word.Khemer;
using System.Globalization;

namespace ReferenceIndexCreator
{

    /// <summary>
    /// Utilizado para generar Cuaderno Alfabeto Khmer
    /// A4 en Vertical
    /// Plantilla de un cuaderno por hoja
    /// </summary>
    /// 
    public class KhmerBookletGenerator
    {
        public static System.Drawing.Size MeasureText;

        KhmerService KhmerService = new KhmerService();

        ArrayList KhmerConsonants = new ArrayList();
        ArrayList DependentVowels = new ArrayList();
        ArrayList IndependentVowels = new ArrayList();



        List<Word> includedChars = new List<Word>();
        List<Word> includedWords = new List<Word>();
        int Maxrepeticion = 0;

        int Level = 1;
        string pathLevel; //Se usa como indicador de nivel combinado en los ficheros de salida.  Combinado nivel 1 2 3 sera 123;
        public KhmerBookletGenerator()
        {

            this.KhmerConsonants = KhmerService.Consonants;
            this.DependentVowels = KhmerService.DependentVowels;
            this.IndependentVowels = KhmerService.IndependentVowels;


        }

        public void GenerateConsonants()
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);


            int index = 1;
           
            PdfWriter writer = new PdfWriter(@"D:\\KH\\" + "Consonants.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            //TABLA DE CONTENIDO
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
            table.UseAllAvailableWidth();

           
            foreach (Consonant character in KhmerConsonants)
            {
                string sIndex = NumeroCuadrado(index, 3);
                this.AddConsonantSubscripPage(character, pdf, document, sIndex);

               

                index++;
            }

            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (index % 2 == 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            }

            document.Close();
            pdf.Close();
            writer.Close();
        }


        public void GenerateDependent()
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            int index = 1;

            PdfWriter writerdep = new PdfWriter(@"D:\\KH\\Dependent.pdf");
            PdfDocument pdfdep = new PdfDocument(writerdep);
            Document documentdep= new Document(pdfdep, PageSize.A4);

            //TABLA DE CONTENIDO
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
            table.UseAllAvailableWidth();
            ArrayList sizes = new ArrayList();
            foreach (DependentVowel character in DependentVowels)
            {
                string sIndex = NumeroCuadrado(index, 3);
                sizes.Add(this.AddDependentVowelPage(character.Character, pdfdep, documentdep, sIndex));
                index++;
            }

            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (index % 2 == 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdfdep.AddNewPage());
            }

            documentdep.Close();
            pdfdep.Close();
            writerdep.Close();
        }

        public void GenerateIndependent()
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);


            int index = 1;

            PdfWriter writer = new PdfWriter(@"D:\\KH\\" + "Independent.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            //TABLA DE CONTENIDO
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 100 }));
            table.UseAllAvailableWidth();

            ArrayList sizes = new ArrayList();
            foreach (IndependentVowel character in IndependentVowels)
            {
                string sIndex = NumeroCuadrado(index, 3);
                //sizes.Add(this.AddCaracterPage(character, pdf, document, sIndex));
                index++;
            }

            //Se añade una pagina en blanco al lomo de caracteres solo si es impar 
            if (index % 2 == 0)
            {
                PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());
            }

            document.Close();
            pdf.Close();
            writer.Close();
        }




            public void ConvertToSinglePDFPages(int level)
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            PdfReader reader = new PdfReader(@"D:\\HSKBooklet\Final Print Level " + Level.ToString() + ".pdf");

            PdfDocument pdfSource = new PdfDocument(reader);
            Document document = new Document(pdfSource, PageSize.A4);
         
                       
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

        public void  AddConsonantSubscripPage(Consonant khc, PdfDocument pdf, Document document, string sIndex)
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            //GENERAR IMAGENES CONSONANTES
            SaveBitmapfromFontChar(khc.Character, "Kaiti", FontStyle.Regular, sIndex,false,Brushes.Black,"Black");
            SaveBitmapfromFontChar(khc.SubscriptCharacter, "Kaiti", FontStyle.Regular, sIndex,true, Brushes.Black, "Black");
            SaveBitmapfromFontChar(khc.Character, "Kaiti", FontStyle.Regular, sIndex, false, Brushes.Black, "Gray");
            SaveBitmapfromFontChar(khc.SubscriptCharacter, "Kaiti", FontStyle.Regular, sIndex, true, Brushes.Black, "Gray");


            PdfCanvas pdfCanvas = new PdfCanvas(pdf.AddNewPage());

            Canvas canvas = new Canvas(pdfCanvas, pdf, document.GetPageEffectiveArea(pdf.GetDefaultPageSize()));


            //Layer 1
            PdfLayer pdflayer1 = new PdfLayer("Layer 1", pdf);
            pdflayer1.SetOn(true);
            pdfCanvas.BeginLayer(pdflayer1);

            ///Template IMG        
            string template = @"D:\KH\Template\ConsonantTemplate.png";

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

            int LeftMarginMiZige = 114 * 72 / 300 - 20;
            int UpMarginMiZige = 114 * 72 / 300 - 5;

            int CaracterSize = 200;

            //Este es el path con el que se ha grabado la imagen previamente
            string path = @"D:\KH\KhmerWorkbook\Chars\"  + khc.Character.ToString() + "_Black.png";

            string SubscriptCharacterpath = @"D:\KH\KhmerWorkbook\Chars\" + khc.SubscriptCharacter.ToString() + "_Black.png";


            //COLOCAR CONSONANTE
            iText.Layout.Element.Image MainCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path))
           .SetFixedPosition(LeftMarginMiZige, PageSize.A4.GetHeight() - CaracterSize - UpMarginMiZige)
           .ScaleAbsolute(CaracterSize, CaracterSize)
           .SetTextAlignment(TextAlignment.CENTER);
            canvas.Add(MainCaracter);

            //COLOCAR FORMA DE SUBSCRIPT AQUI 
            int ScriptSize =  200;



            iText.Layout.Element.Image SubscriptCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(SubscriptCharacterpath))
            .SetFixedPosition(1000 * 72 / 300 - 20, PageSize.A4.GetHeight() - CaracterSize - UpMarginMiZige)
            .ScaleAbsolute(ScriptSize, ScriptSize)
            .SetTextAlignment(TextAlignment.CENTER);
            canvas.Add(SubscriptCaracter);
            pdfCanvas.EndLayer();

            //Layer 4 CARACTER PEQUEÑOS FOR
            PdfLayer pdflayer4 = new PdfLayer("Layer 4", pdf);
            pdflayer4.SetOn(true);
            pdfCanvas.BeginLayer(pdflayer4);

            int MiniCaracterSize = 30;
            int MaxColumns = 16; //Se elimina una columna al ver los resultados del margen del primer Workbook TianziGe enviado por Amazon
            int MaxRows = 17;

            float width = 30.6F;
            float height = 30.7F;           
            int UpMarginMiZigeMini = 22;

            float left = 0;
            float bottom = 0;

            iText.Layout.Element.Image MiniCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path));
            MiniCaracter.ScaleAbsolute(MiniCaracterSize, MiniCaracterSize);
            MiniCaracter.SetTextAlignment(TextAlignment.CENTER);

            for (int col = 0; col < MaxColumns; col++)

            {
                for (int row = 0; row < MaxRows; row++)
                {
                    //No se imprimen donde esta en caracter grande situado
                    if (!(row < 6 ))
                    {
                        left = 35 + col * width;
                        bottom = PageSize.A4.GetHeight() - MiniCaracterSize - 2 - UpMarginMiZigeMini - row * height;
                        MiniCaracter.SetFixedPosition(left, bottom);
                        canvas.Add(MiniCaracter);
                    }

                }
            }
            pdfCanvas.EndLayer();

            //INFORMACION CARACTER          

            document.SetFont(fontKaiti);

            var dimensionsColumns = new float[] { 1, 1, 1, 1, 1, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(dimensionsColumns));
            table.UseAllAvailableWidth();

            PdfLayer pdflayerInfo = new PdfLayer("pdflayerInfo", pdf);
            pdflayerInfo.SetOn(true);
            pdfCanvas.BeginLayer(pdflayerInfo);
            int repeticion = 0;

            List<Paragraph> pGlobals = new List<Paragraph>();
            foreach (Word_HSK word in includedWords)
            {
                //Add row to table
                if (word.Character.Contains(khc.Character))
                {
                    repeticion++;

                    Paragraph pGlobal = new Paragraph(word.Character + "\t (" + word.Pinyin + ") , " + word.Type + " , " + word.Description).SetFont(fontKaiti).SetFontSize(10);

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


        public Tuple<float, float> AddDependentVowelPage(string c, PdfDocument pdf, Document document, string sIndex)
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            //GENERAR CARACTER
            Tuple<float, float> textSize = SaveBitmapfromFontCharVowel(c, "Kaiti", FontStyle.Regular, sIndex);
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

            int LeftMarginMiZige = 114 * 72 / 300 - 20;
            int UpMarginMiZige = 114 * 72 / 300 - 5;
            // int CaracterSize = 180;

            int CaracterSize = 200;

            //Este es el path con el que se ha grabado la imagen previamente
            string path = @"D:\KH\KhmerWorkbook\Chars\" + c.ToString() + ".png";

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

            int MiniCaracterSize = 30;
            int MaxColumns = 17;
            int MaxRows = 17;

            float width = 30.6F;
            float height = 30.7F;
            int UpMarginMiZigeMini = 22;

            float left = 0;
            float bottom = 0;

            iText.Layout.Element.Image MiniCaracter = new iText.Layout.Element.Image(ImageDataFactory.Create(path));
            MiniCaracter.ScaleAbsolute(MiniCaracterSize, MiniCaracterSize);
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
                        MiniCaracter.SetFixedPosition(left, bottom);
                        canvas.Add(MiniCaracter);
                    }

                }
            }
            pdfCanvas.EndLayer();

            //INFORMACION CARACTER          

            document.SetFont(fontKaiti);

            var dimensionsColumns = new float[] { 1, 1, 1, 1, 1, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(dimensionsColumns));
            table.UseAllAvailableWidth();

            PdfLayer pdflayerInfo = new PdfLayer("pdflayerInfo", pdf);
            pdflayerInfo.SetOn(true);
            pdfCanvas.BeginLayer(pdflayerInfo);
            int repeticion = 0;

            List<Paragraph> pGlobals = new List<Paragraph>();
            foreach (Word_HSK word in includedWords)
            {
                //Add row to table
                if (word.Character.Contains(c))
                {
                    repeticion++;

                    Paragraph pGlobal = new Paragraph(word.Character + "\t (" + word.Pinyin + ") , " + word.Type + " , " + word.Description).SetFont(fontKaiti).SetFontSize(10);

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


            return textSize;

        }
        private static void RemoveBorder(Table table)
        {
            foreach (IElement iElement in table.GetChildren())
            {
                ((Cell)iElement).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            }
        }

     

        public void CharacterIndexGenerator()
        {
            //Se incluyen los caracteres no las palabras
            //List<Word> arrayIncluded = ToList(includedWords);

            int index = 0;

            //Hay que obtener caracter a caracter no las palabras TODO

            //INICIO DOCUMENTO PDF
            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\" + this.pathLevel + "CaracterIndex.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4);

            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            document.SetFont(fontKaiti);

            //TABLA DE CONTENIDO

            //Se añade el Tipo de palabra 
            //Una columna mas. 
            var DimensionesColumnas = new float[] { 5, 20, 20, 50, 5, 10, 15 };
            Table table = new Table(UnitValue.CreatePercentArray(DimensionesColumnas));
            float tablewidth = 95;
            //table.UseAllAvailableWidth();
            //Se ajusta el margen del indice a 90% de la pagina
            table.SetWidth(UnitValue.CreatePercentValue(tablewidth));
            table.SetMarginLeft(5);

           

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
                foreach (Word_HSK word in includedWords)
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

                        Cell cellType = new Cell();
                        cellType.Add(new Paragraph(word.Type).SetFontSize(10).SetMarginTop(SetMarginTopP).SetTextAlignment(TextAlignment.LEFT).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                        table.AddCell(cellType);


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
                            //table.UseAllAvailableWidth();
                            //Se ajusta el margen del indice a 90% de la pagina
                            table.SetWidth(UnitValue.CreatePercentValue(tablewidth));
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

        //Graba en disco en caracter indicado c
        private Tuple<float, float> SaveBitmapfromFontChar(string c,  string FontName, FontStyle fontStyle, string sIndex,  bool subscript, Brush brush, string BrushColor)
        {
            Int32 fontPoints = 375;

            int MaxWidth = 700;
            int MaxHeight = 500;

            if (subscript) MaxHeight = 700;
            
            Size size = new Size(MaxWidth, MaxHeight);

            var MaxSize = Math.Max(size.Width, size.Height);
            Bitmap bitmapchar = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(bitmapchar);
          
            Font f = new Font(FontName, fontPoints, fontStyle, GraphicsUnit.World);
            Brush brushblack = Brushes.Black;
            Brush brushwhite = Brushes.White;
            Brush brushLightGray = Brushes.LightGray;

          

            int LeftCorrect = 845 - size.Width;

            if (c == "ណ") LeftCorrect = 75;
            if (c == "ឈ") LeftCorrect = 50;


            if (!((sIndex != "004") || (sIndex != "009") || (sIndex != "014") || (sIndex != "021") || (sIndex != "026") || (sIndex != "027") || (sIndex != "020")) && (subscript = true))

                LeftCorrect = LeftCorrect + 200;


            { }
               


            //ឈ 669 Width 
          

            //Un cuarto de espacio para intentar centrar cadena 
            float textWidth = graphic.MeasureString(c.ToString(), f).Width;
            float textHeight = graphic.MeasureString( c.ToString(), f).Height;

            System.Drawing.Point p = new System.Drawing.Point(LeftCorrect, 0);
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, Math.Max(size.Width, size.Height), Math.Max(size.Width, size.Height));

            Region reg = new Region(rec);

            graphic.FillRegion(brushwhite, reg);
                       

            Tuple<float, float> textSize = new Tuple<float, float>(textWidth, textHeight);

            graphic.DrawString(c.ToString(), f, brush, p);
            //Transparencia para el png . Blanco transparente
            bitmapchar.MakeTransparent(Color.White);
            bitmapchar.Save(@"D:\KH\KhmerWorkbook\Chars\" + c.ToString() + "_" + BrushColor +".png", ImageFormat.Png);

            return textSize;

        }


        private Tuple<float, float> SaveBitmapfromFontCharVowel(string vowel, string FontName, FontStyle fontStyle, string sIndex)
        {
            Int32 fontPoints = 375;

            int MaxWidth = 700;
            int MaxHeight = 700;

            Size size = new Size(MaxWidth, MaxHeight);

            var MaxSize = Math.Max(size.Width, size.Height);
            Bitmap bitmapchar = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(bitmapchar);

            Font f = new Font(FontName, fontPoints, fontStyle, GraphicsUnit.World);
            Brush brushblack = Brushes.Black;
            Brush brushwhite = Brushes.White;
            Brush brushLightGray = Brushes.LightGray;

            int FixedMarginCorrect = 100;
            //Ajustar AQUI
            int LeftCorrect = 700 - size.Width + FixedMarginCorrect;
           

            //Un cuarto de espacio para intentar cenrar cadena 
            float textWidth = graphic.MeasureString(vowel.ToString(), f).Width;
            float textHeight = graphic.MeasureString(vowel.ToString(), f).Height;

           //CORRECCIONES ESPECIFICAS 
            int UpCorrect = 150;
            //Corregir Height para caracteres manualmente aqui . 
            //Parece que todos tienen el mismo Height ?=


            System.Drawing.Point p = new System.Drawing.Point(LeftCorrect, UpCorrect);
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, Math.Max(size.Width, size.Height), Math.Max(size.Width, size.Height));

            Region reg = new Region(rec);

            graphic.FillRegion(brushwhite, reg);


            Tuple<float, float> textSize = new Tuple<float, float>(textWidth, textHeight);

            graphic.DrawString(vowel.ToString(), f, brushLightGray, p);

            //Transparencia para el png . Blanco transparente
            bitmapchar.MakeTransparent(Color.White);
            bitmapchar.Save(@"D:\KH\KhmerWorkbook\Chars\" + vowel.ToString() + ".png", ImageFormat.Png);

            return textSize;

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


        //Encargado de juntar todos los documentos PDF generados en uno solo. 
        public void FinalGroup(int Level)
        {
            const String FONT = @"C:\Windows\Fonts\STKAITI.TTF";
            PdfFont fontKaiti = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED, true);

            //D:\Portada\HSKBooklets

            PdfWriter writer = new PdfWriter(@"D:\\HSKBooklet\Final_HSK_" + this.pathLevel + ".pdf");

            PdfDocument pdf = new PdfDocument(new PdfWriter(writer));
            PdfMerger merger = new PdfMerger(pdf);
          

            pdf.AddFont(fontKaiti);

            //Interior Cover

            //Add pages from the first document
            PdfDocument InteriorCoverPdf = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\Interior Cover Level " + this.pathLevel + ".pdf"));
            //From to
            merger.Merge(InteriorCoverPdf, 1, InteriorCoverPdf.GetNumberOfPages());



            //White White Page A4.pdf
            PdfDocument IWhitePdf = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\White Page A4.pdf"));
            //From to
            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //HSK Booklets D:\HSKBooklet 1_HSK
            PdfDocument bookletHSK = new PdfDocument(new PdfReader(@"D:\HSKBooklet\" + this.pathLevel + "_HSK.pdf"));
            //From to
            merger.Merge(bookletHSK, 1, bookletHSK.GetNumberOfPages());


            //Word Index Cover "Word Index Level 3.pdf"
            PdfDocument wordIndexCover = new PdfDocument(new PdfReader(@"D:\\Portada\\HSKBooklets\Word Index Level " + this.pathLevel + ".pdf"));
            //From to
            merger.Merge(wordIndexCover, 1, wordIndexCover.GetNumberOfPages());

            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //Word Index 

            PdfDocument wordIndex = new PdfDocument(new PdfReader(@"D:\\HSKBooklet\" + this.pathLevel + "WordIndex.pdf"));
            //From to
            merger.Merge(wordIndex, 1, wordIndex.GetNumberOfPages());


            //Char Cover "Character Index Level 1.pdf"
            PdfDocument CharacterIndexCover = new PdfDocument(new PdfReader(@"D:\\Portada\HSKBooklets\Character Index Level " + this.pathLevel + ".pdf"));
            //From to
            merger.Merge(CharacterIndexCover, 1, CharacterIndexCover.GetNumberOfPages());

            merger.Merge(IWhitePdf, 1, IWhitePdf.GetNumberOfPages());

            //Char Index

            PdfDocument caracterIndex = new PdfDocument(new PdfReader(@"D:\\HSKBooklet\" + this.pathLevel + "CaracterIndex.pdf"));
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


    }
}
