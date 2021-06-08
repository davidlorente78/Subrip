using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DomainServices;
using DALReaders;
using System.Collections;
using HSK;
using System.Linq;

namespace ReferenceIndexCreator
{
    public  class Tools
    {
        List<Tuple<int, string>> document;

        public Tools() {

            LoadDocument();

        }
        public  byte[] convertDocToByteArray(String sourcePath)
        {

            byte[] byteArray = null;
            try
            {
                byteArray = File.ReadAllBytes(sourcePath);


                MemoryStream c = new MemoryStream(byteArray);


            }
            catch (FileNotFoundException e)
            {

            }
            catch (IOException e)
            {

            }
            return byteArray;
        }


          void  LoadDocument()
        {



            MemoryStream memory = new MemoryStream(convertDocToByteArray(@"D:\Code Project\Subrip\Document\HSK Book.pdf"));
            BinaryReader BRreader = new BinaryReader(memory);
            StringBuilder text = new StringBuilder();


            iText.Kernel.Pdf.PdfReader iTextReader = new iText.Kernel.Pdf.PdfReader(memory);
            iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(iTextReader);



            int numberofpages = pdfDoc.GetNumberOfPages();
            List<Tuple<int, string>> Contents = new List<Tuple<int, string>>();


            for (int page = 1; page <= numberofpages; page++)
            {
                iText.Kernel.Pdf.Canvas.Parser.Listener.ITextExtractionStrategy strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();

                string currentText = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                    Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                text.Append(currentText);


                Tuple<int, string> tuple = new Tuple<int, string>(page, currentText);
                Contents.Add(tuple);


            }


            document = Contents;



        }

        public Report stringIndex(int Level,bool acumulativo) {


            const int INICIOANEXOS = 126;
            string tocopypaste = "";

            string palabrassinfrases = "";
                int con = 0;
                int sin = 0;
            var index = HSKIndex(Level, acumulativo);

            foreach (var item in index) {
                string pages = "";

                //Esto excluye las palabras que estan siempre en los anexos del documento 
                IEnumerable<int> items = item.Item2.Where(x => x < INICIOANEXOS);


                //Quitar las palabras que estan en el indice



                if (items.Count()>0)
                {


                    foreach (int i in items)
                    {
                        
                        pages = pages + i.ToString() + ",";
                    }
                    //Eliminar ultima , añadida
                    tocopypaste = tocopypaste + item.Item1 + "\t" + pages + "\n";
                    con++;
                }

                else {

                    palabrassinfrases = palabrassinfrases + item.Item1 + "\n";
                    sin++;

                }



            }

            Report r = new Report() { Included = con, NonIncluded = sin, NonIncludedWords = palabrassinfrases, ReportFormatted= tocopypaste };



            return r;



        }


        public  List<Tuple<string, List<int>>> HSKIndex(int Level,bool acumulative)
        {
            List<Tuple<string, List<int>>> Index = new List<Tuple<string, List<int>>>();

            ArrayList includedWords = new ArrayList();

            if (acumulative)
            {
                for (int i = 1; i < Level + 1; i++)
                {

                    Reader_HSK reader = new Reader_HSK() { Level = i };
                    ListService service = new ListService(reader);

                    var levelwords = service.GetAllWords(i);
                    includedWords.AddRange(levelwords);

                }

            }
            else {

                Reader_HSK reader = new Reader_HSK() { Level = Level };
                ListService service = new ListService(reader);

                var levelwords = service.GetAllWords(Level);
                includedWords.AddRange(levelwords);
            }


            //Argh
            List<Word> arrayIncluded = ToList(includedWords);

            foreach (HSK.Word_HSK w in arrayIncluded.OrderBy(x => x.NumberPinyin)) {

                List<int> pagesthatcontainsword = new List<int>();
                string HSKWord = w.Character;

                foreach (var page in document) {


                    if (page.Item2.Contains(HSKWord)) {

                        if (!pagesthatcontainsword.Contains (page.Item1))  pagesthatcontainsword.Add(page.Item1);
                    }
                
                
                
                
                }


                Index.Add(new Tuple<string, List<int>>(w.Character, pagesthatcontainsword));
            
            
            
            
            
            }

            return Index;


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
    }
}



