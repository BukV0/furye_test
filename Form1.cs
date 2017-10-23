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
using System.Windows.Forms.DataVisualization.Charting;
//using System.Web.UI.DataVisualization.Charting;

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

            fileHeader.Text = "File was loaded:\n" +
                reader.ToString();

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
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
                reader.lDataList[i] = 0;
            }
            //Complex[] rComplex = new Complex[reader.getSampleRate()];
            Complex[] rComplex = new Complex[32768];
            for (int i = 0; i < rComplex.Length; i++)
            {
                rComplex[i] = (double)reader.rDataList[i] / short.MaxValue;
                if(i % 1000 == 0) Console.Write(rComplex[i].Magnitude + " ");
            }
            Console.WriteLine();

            String s = "freq, Hz";
            chart3.Series.Add(s);
            chart3.Series[s].ChartType = SeriesChartType.Line;
            for (int i = 0; i < rComplex.Length; i++)
            {
                chart3.Series[s].Points.AddY(rComplex[i].Real);
            }

            rComplex = Furier.DPF(rComplex);

            chart1.Series.Add(s);
            chart1.Series[s].ChartType = SeriesChartType.Line;
            for (int i = 0; i < 200; i++)
            {
                chart1.Series[s].Points.AddY(rComplex[i].Magnitude);
            }
            //rComplex = Furier.translate(rComplex);

            for (int i = 0; i < rComplex.Count(); i++)
            {
                rComplex[i] = new Complex(rComplex[i].Real, -rComplex[i].Imaginary);
            }

            rComplex = Furier.DPF(rComplex);

            for (int i = 0; i < rComplex.Count(); i++)
            {
                rComplex[i] = new Complex(rComplex[i].Real, -rComplex[i].Imaginary);
                rComplex[i] /= rComplex.Count();
            }

            chart2.Series.Add(s);
            chart2.Series[s].ChartType = SeriesChartType.Line;
            for (int i = 0; i < rComplex.Length; i++)
            {
                chart2.Series[s].Points.AddY(rComplex[i].Real);
            }

            double max = -1;
            for (int i = 0; i < rComplex.Count(); i++)
            {
                if (rComplex[i].Magnitude > max)
                {
                    max = rComplex[i].Magnitude;
                }
            }
            for(int i = 0; i < rComplex.Count(); i++)
            {
                reader.rDataList[i] = (short)(rComplex[i].Real * short.MaxValue);
                if (i % 1000 == 0) Console.Write(rComplex[i].Magnitude + " ");
            }
        }
    }
}
