using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Bulanik_Mantik
{
    public partial class Form1 : Form
    {
        FuzzyLogicCore core = new FuzzyLogicCore();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Parent = chart1;
            label5.Parent = chart1;
            label6.Parent = chart1;

            label7.Parent = chart2;
            label2.Parent = chart2;
            label3.Parent = chart2;

            label10.Parent = chart3;
            label11.Parent = chart3;
            label9.Parent = chart3;

            double maxDataPoint = chart1.ChartAreas[0].AxisY.Maximum;
            double minDataPoint = chart1.ChartAreas[0].AxisY.Minimum;
            trackBar2_Scroll(trackBar1, null);
            trackBar2_Scroll(trackBar2, null);
            trackBar2_Scroll(trackBar3, null);

            for (int i = 0; i < 27; i++)
            {
                flowLayoutPanel1.Controls.Add(new KuralComponent() { Visible = false });
            }
            //LineAnnotation annotation2 = new LineAnnotation();
            //annotation2.IsSizeAlwaysRelative = false;
            //annotation2.AxisX = chart1.ChartAreas[0].AxisX;
            //annotation2.AxisY = chart1.ChartAreas[0].AxisY;
            //annotation2.AnchorY = minDataPoint;
            //annotation2.Height = maxDataPoint - minDataPoint;;
            //annotation2.Width = 0;
            //annotation2.LineWidth = 2;
            //annotation2.StartCap = LineAnchorCapStyle.None;
            //annotation2.EndCap = LineAnchorCapStyle.None;
            //annotation2.AnchorX = 5;  // <- your point
            //annotation2.LineColor = Color.Pink; // <- your color
            //chart1.Annotations.Add(annotation2);

            List<Point> p = new List<Point>();
            p.Add(new Point(0, 0));
            p.Add(new Point(5, 1));
            p.Add(new Point(6, 1));
            p.Add(new Point(7, 0));


            List<Point> p2 = new List<Point>();
            p.Add(new Point(1, 0));
            p.Add(new Point(6, 1));
            p.Add(new Point(7, 1));
            p.Add(new Point(8, 0));

            var result = p.Intersect(p2);

            var fl = new FuzzyLogicCore();
            //fl.HassaslikKesisim(2.75);
            var x = getnode(new Point(1, 1), new Point(10, 10), new Point(2, 2), new Point(2, 15));

        }
        public PointF getnode(Point A, Point B, Point C, Point D)
        {
            double dy1 = B.Y - A.Y;
            double dx1 = B.X - A.X;
            double dy2 = D.Y - C.Y;
            double dx2 = D.X - C.X;
            PointF p = new PointF();
            // check whether the two line parallel
            if (dy1 * dx2 == dy2 * dx1)
                return new PointF(0, 0);
            else
            {
                double x = ((C.Y - A.Y) * dx1 * dx2 + dy1 * dx2 * A.X - dy2 * dx1 * C.X) / (dy1 * dx2 - dy2 * dx1);
                double y = A.Y + (dy1 / dx1) * (x - A.X);
                p = new PointF((float)x, (float)y);
                return p;
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (flowLayoutPanel1.Controls.Count !=27) return;

            int sayici = 0;

            double x1, x2, x3;
            x1 = (double)numericUpDown1.Value;
            x2 = (double)numericUpDown2.Value;
            x3 = (double)numericUpDown3.Value;

            // flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.SuspendLayout();
            List<Kural> kurallar = new List<Kural>();

            foreach (var list1 in core.KesisimList[0])
            {
                foreach (var list2 in core.KesisimList[1])
                {
                    foreach (var list3 in core.KesisimList[2])
                    {

                        Kural kural = new Kural(list1, list2, list3);
                        kural.XValues(x1, x2, x3);
                        kurallar.Add(kural);

                        KuralComponent kuralComp = flowLayoutPanel1.Controls[sayici++] as KuralComponent;
                        kuralComp.SetKural(kural);
                        //flowLayoutPanel1.Controls.Add(kuralComp);
                        //KuralComponent kuralComp = new KuralComponent(kural);
                        //flowLayoutPanel1.Controls.Add(kuralComp);
                        kuralComp.Visible = true;
                        //flowLayoutPanel1.Controls.SetChildIndex(kuralComp,sayici);
                        //flowLayoutPanel1.Controls.OfType<KuralComponent>().OrderBy(a => a.Visible).Reverse();

                    }
                }
            }

            var count = flowLayoutPanel1.Controls.OfType<KuralComponent>().Where(a => a.Visible == true).Count();
            for (int i = sayici; i < 27; i++)
            {
                    flowLayoutPanel1.Controls[i].Visible = false;
                    
            }





            flowLayoutPanel1.ResumeLayout();


            // <<Kurala ait Minimum kesişimi> , <Minimum değere sahip ara değer grafiğinin ağırlığı>>
            List<Tuple<double, double>> agirlikliDonusOrtTuple = new List<Tuple<double, double>>(); 
            for (int i = 0; i < 5; i++)
            {
                Enums.Donus donus = (Enums.Donus)i;
                var item = kurallar.Where(a => a.DonusHizi == donus); //Çıktı Grafiğinin Ara değer grafiklerine karşılık gelen Enumlarını getir
                Tuple<double, double> maxTupple=null;
                if (item.Count() > 0)
                {
                    var maxValue = item.Max(b => b.GetMinKesisimX); //Min. Gelen değerin Maximum değer seçimi
                    var maxItem = item.First(a => a.GetMinKesisimX == maxValue); // Maximum değere sahip nesnenin bulunması
                    maxTupple= new Tuple<double, double>(maxItem.GetMinKesisimX,maxItem.AğırlıkGetir(Enums.AgirlikMerkez.Donus));
                    agirlikliDonusOrtTuple.Add(maxTupple);
                }

                if (maxTupple!=null)
                {
                    Series series = chart4.Series.Any(a=>a.Name=="area"+i)?chart4.Series["area"+i]: new Series("area"+i);
                    series.ChartType = SeriesChartType.Area;
                    series.Color = Color.FromArgb(175, 255-(i*51), i*51,255/ (i + 1) );
                    series.Points.Clear();
                   
                    var list= core.AreaHesapla(Enums.AgirlikMerkez.Donus, maxTupple.Item1,i);
                    foreach (var l in list)
                        series.Points.AddXY(l.X,l.Y);
                    
                    if (!chart4.Series.Contains(series))
                        chart4.Series.Add(series);
                }
            }

            List<Tuple<double, double>> agirlikliDeterjanOrtTuple = new List<Tuple<double, double>>();
            for (int i = 0; i < 5; i++)
            {
                Enums.Deterjan deterjan = (Enums.Deterjan)i;
                var item = kurallar.Where(a => a.DeterjanMiktari == deterjan);
                Tuple<double, double> maxTupple=null;
                if (item.Count() > 0)
                {
                    var maxValue = item.Max(b => b.GetMinKesisimX);    //Maximum Seçimi
                    var maxItem = item.First(a => a.GetMinKesisimX == maxValue);
                    maxTupple= new Tuple<double, double>(maxItem.GetMinKesisimX,maxItem.AğırlıkGetir(Enums.AgirlikMerkez.Deterjan));
                    agirlikliDeterjanOrtTuple.Add(maxTupple);
                }

                if (maxTupple!=null)
                {
                    Series series = chart5.Series.Any(a=>a.Name=="area"+i)?chart5.Series["area"+i]: new Series("area"+i);
                    series.ChartType = SeriesChartType.Area;
                    series.Color = Color.FromArgb(175, 255-(i*51), i*51,255/ (i + 1) );
                    series.Points.Clear();
                   
                    var list= core.AreaHesapla(Enums.AgirlikMerkez.Deterjan, maxTupple.Item1,i);
                    foreach (var l in list)
                        series.Points.AddXY(l.X,l.Y);
                    
                    if (!chart5.Series.Contains(series))
                        chart5.Series.Add(series);
                }
            }


            List<Tuple<double, double>> agirlikliSureOrtTuple = new List<Tuple<double, double>>();
            for (int i = 0; i < 5; i++)
            {
                Enums.Sure sure = (Enums.Sure)i;
                var item = kurallar.Where(a => a.Suresi == sure);
                Tuple<double, double> maxTupple=null;
                if (item.Count() > 0)
                {
                    var maxValue = item.Max(b => b.GetMinKesisimX);
                    var maxItem = item.First(a => a.GetMinKesisimX == maxValue);
                    maxTupple= new Tuple<double, double>(maxItem.GetMinKesisimX,maxItem.AğırlıkGetir(Enums.AgirlikMerkez.Sure));
                    agirlikliSureOrtTuple.Add(maxTupple);
                }

                if (maxTupple!=null)
                {
                    Series series = chart6.Series.Any(a=>a.Name=="area"+i)?chart6.Series["area"+i]: new Series("area"+i);
                    series.ChartType = SeriesChartType.Area;
                    series.Color = Color.FromArgb(175, 255-(i*51), i*51,255/ (i + 1) );
                    series.Points.Clear();
                   
                    var list= core.AreaHesapla(Enums.AgirlikMerkez.Sure, maxTupple.Item1,i);
                    foreach (var l in list)
                        series.Points.AddXY(l.X,l.Y);
                    
                    if (!chart6.Series.Contains(series))
                        chart6.Series.Add(series);
                }
            }


            label15.Text = agirlikliDonusOrtTuple.AgirlikliOrtalamaExt(a => a.Item1, b => b.Item2).ToString();
            label18.Text = agirlikliDeterjanOrtTuple.AgirlikliOrtalamaExt(a => a.Item1, b => b.Item2).ToString();
            label17.Text = agirlikliSureOrtTuple.AgirlikliOrtalamaExt(a => a.Item1, b => b.Item2).ToString();
          

        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // button2.Location=new Point(e.X,button2.Location.Y);

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            //(e != null) Event tetiklenmesi dışında girilmemesi için

            TrackBar activeTrackBar = sender as TrackBar;
            NumericUpDown activeNumericUpDown = activeTrackBar.Parent.Controls.OfType<NumericUpDown>().First();
            int indis = int.Parse(Regex.Replace(activeTrackBar.Name, "\\D*", ""));
            if (e == null)
                activeTrackBar.Value = (int)(activeNumericUpDown.Value * 1000);
            var temp = (double)activeTrackBar.Value / 1000;
            var chart = activeTrackBar.Parent.Controls.OfType<Panel>().First().Controls.OfType<Chart>().First();
            double X = temp > 0 ? temp : 0.03;
            if (e != null)
                activeNumericUpDown.Value = (decimal)X;
            chart.Series[3].Points[0].XValue = X;
            chart.Series[3].Points[0].Label = (core.KesisimHesapla(temp, (FuzzyLogicCore.KESISIM)(indis - 1)).Min().ToString() + ".000").Substring(0, 5);
            if (e != null)
                button1_Click(null, null);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender as Control).Focused) return;
            TrackBar tb = (sender as Control).Parent.Controls.OfType<TrackBar>().First();
            trackBar2_Scroll(tb, null);
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
           //Series bar = new Series();
           //bar.ChartType = SeriesChartType.Area;
           //bar.Color = Color.FromArgb(150, 155, 250, 155);
        
           Series series = chart1.Series.Any(a=>a.Name=="area1")?chart1.Series["area1"]: new Series("area1");
           series.ChartType = SeriesChartType.Area;
           series.Color = Color.FromArgb(150, 155, 250, 155);
           series.Points.Clear();
           series.Points.AddXY(1,0);
           series.Points.AddXY(5,1);
           series.Points.AddXY(6,1);
           series.Points.AddXY(8,0);
           if (!chart1.Series.Contains(series))
           {
               chart1.Series.Add(series);
           }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
                control.Visible = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
                control.Visible = true;
        }
    }
}
