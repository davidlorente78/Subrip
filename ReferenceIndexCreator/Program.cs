using System;
using System.IO;
using System.Text;
using iText;
namespace ReferenceIndexCreator
{
    class Program
    {
        static void Main(string[] args)
        {

            Tools t = new Tools();
            //var index = t.HSKIndex(3,true);


            var copypastethisintoword = t.stringIndex(3,true);

            //Utilizado para generar el Indice del Libro de Gramatica
            var report1 = t.stringIndex(1, false);
            var report2 = t.stringIndex(2, false);
            var report3 = t.stringIndex(3, false);
            var report4 = t.stringIndex(4, false);



            #region Cuadernos HSK por Nivel A4 Vertical 
            //bool Acumulative = false;

            ////TODO Se tiene que quitar uno de los indices porque Amazon solo acepta hasta 828 Paginas
            ////Tambien afecta a ACUMULATIVE
            //for (int Level = 1; Level < 2; Level++)
            //{
            //    HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage hSKBooklet_Generator = new HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage(Level, Acumulative);
            //    hSKBooklet_Generator.DocumentGenerator();
            //    hSKBooklet_Generator.CharacterIndexGenerator();
            //    hSKBooklet_Generator.WordIndexGenerator();
            //    hSKBooklet_Generator.FinalGroup(Level);

            //    //Abrir archivo generado con WPS y Imprimir a PDF con Impresora Windows una sola cara
            //    //Grabar como  Final Print Level X.pdf
            //    //Final Print Level
            //    //Cerrar archivo y continuar ejecucion

            //    hSKBooklet_Generator.ConvertToSinglePDFPages(Level);
            //    //Una vez convertidas las paginas a PDF una por archivo
            //    //Abrir Photoshop y convertir a JPG maxima calidad con SECUENCIA DE COMANDOS PROCESADOR DE IMAGEN
            //    //Crear documento WORD con las imagenes JPG ajustando los margenes al acabado final. 
            //    //Las paginas 1, 175 189 y 202 son a tamaño A4 sin margenes. Hay que ajustarlas y reducir un poco su tamaño. 
            //    //Se ajustaron manualmente a 2480x3508

            //}

            #endregion


            for (int Level = 2; Level <= 3; Level++)
            {
                HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage_Acumulative hSKBooklet_Generator = new HSKBooklet_Generator_A4_Vertical_OneCharacterPerPage_Acumulative(Level);
                hSKBooklet_Generator.DocumentGenerator();
                hSKBooklet_Generator.CharacterIndexGenerator();
                hSKBooklet_Generator.WordIndexGenerator();
            }


        }
    }
}
