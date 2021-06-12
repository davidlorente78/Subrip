using ReferenceIndexCreator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndexGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tools t = new Tools();
            //var index = t.HSKIndex(3,true);


            var copypastethisintoword = t.stringIndex(3, true);

            //Utilizado para generar el Indice del Libro de Gramatica
            var report1 = t.stringIndex(1, false);
            this.txtIncludedHSK1.Text = report1.Included.ToString();
            this.txtNonIncludedHSK1.Text = report1.NonIncluded.ToString();
            this.richTextBox1.Text = report1.NonIncludedWords.ToString();
            this.richTextBox2.Text = report1.ReportFormatted.ToString();
            this.richTextBox9.Text = report1.ExtremelyReapeted.ToString();


            var report2 = t.stringIndex(2, false);
            this.textBox2.Text = report2.Included.ToString();
            this.textBox1.Text = report2.NonIncluded.ToString();
            this.richTextBox4.Text = report2.NonIncludedWords.ToString();
            this.richTextBox3.Text = report2.ReportFormatted.ToString();
            this.richTextBox10.Text = report2.ExtremelyReapeted.ToString();


            var report3 = t.stringIndex(3, false);
            this.textBox4.Text = report3.Included.ToString();
            this.textBox3.Text = report3.NonIncluded.ToString();
            this.richTextBox6.Text = report3.NonIncludedWords.ToString();
            this.richTextBox5.Text = report3.ReportFormatted.ToString();
            this.richTextBox11.Text = report3.ExtremelyReapeted.ToString();



            var report4 = t.stringIndex(4, false);
            this.textBox6.Text = report4.Included.ToString();
            this.textBox5.Text = report4.NonIncluded.ToString();
            this.richTextBox8.Text = report4.NonIncludedWords.ToString();
            this.richTextBox7.Text = report4.ReportFormatted.ToString();
            this.richTextBox12.Text = report4.ExtremelyReapeted.ToString();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
