using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace amiralBatti
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            pictureBox1.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"..\..\rec\IMG_20221125_160918_515.jpg");
            pictureBox2.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"..\..\rec\WhatsApp Image 2023-12-26 at 21.47.17_2b91f526.jpg");
        }
        int i=0, j=0, k = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            string mami = "Muhammed Şahbaz";
            string gorkem = "Görkem Canik";
            string hayri = "Hayrullah Kalkan";

            if(i<mami.Length)
            {
                label1.Text += mami[i];
            }
            i++;

            if(j<gorkem.Length)
                label2.Text += gorkem[j];
            j++;

            if(k<hayri.Length)
                label3.Text += hayri[k];
            k++;
            if ((i >= mami.Length) && (k >= gorkem.Length) && (j >= hayri.Length))
                timer1.Enabled = false;

        }
    }
}
