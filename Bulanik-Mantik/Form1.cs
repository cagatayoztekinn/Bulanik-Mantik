using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Bulanik_Mantik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Area;
            series.Points.AddXY(5,0);
            series.Points.AddXY(8,0.7);
            series.Points.AddXY(12,0.89);
            series.Points.AddXY(15,0);
            series.Color = Color.FromArgb(50,220,25,0);
            

            chart1.Series.Add(series);

        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
               // button2.Location=new Point(e.X,button2.Location.Y);

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            double ratio = (double)trackBar1.Maximum / ((double) trackBar1.Size.Width -33);

            var temp = (int) ((trackBar1.Value/ratio) +15+ trackBar1.Location.X);
            button2.Location=new Point(temp,button2.Location.Y);
        }
    }
}
