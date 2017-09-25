using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Media;
using System.IO;

namespace FourierTransform
{
    public partial class Form1 : Form
    {
        WavReader reader;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //read
        private void button1_Click(object sender, EventArgs e)
        {
            reader = new WavReader ();
            reader.read(inputFile.Text);


        }

        //write
        private void button2_Click(object sender, EventArgs e)
        {
            reader.write(outputFile.Text);
        }

        // play
        private void playButton_Click(object sender, EventArgs e)
        {
            String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Console.WriteLine(path + "\\" + outputFile.Text);
                SoundPlayer player = new SoundPlayer(path + "\\" + outputFile.Text);
                player.Play();   
        }

        //edit
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < reader.rDataList.Count; i++)
            {
                reader.rDataList[i] = 0;
            }
            Complex[] lComplex = new Complex[reader.lDataList.Count];
            for (int i = 0; i < reader.lDataList.Count; i++)
            {
                lComplex[i] = (double)reader.lDataList[i] / short.MaxValue;
                if(i % 1000 == 0) Console.Write(lComplex[i].Magnitude + " ");
            }
            Console.WriteLine();
            lComplex = Furier.DPF(lComplex);
            lComplex = Furier.translate(lComplex);
            lComplex = Furier.DPF(lComplex);
            double max = -1;
            for (int i = 0; i < lComplex.Count(); i++)
            {
                if (lComplex[i].Magnitude > max)
                {
                    max = lComplex[i].Magnitude;
                }
            }
            for(int i = 0; i < lComplex.Count(); i++)
            {
                reader.lDataList[i] = (short)(lComplex[i].Magnitude / max * short.MaxValue);
                if (i % 1000 == 0) Console.Write(lComplex[i].Magnitude + " ");
            }
        }
    }
}
