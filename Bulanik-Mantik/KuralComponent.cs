using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bulanik_Mantik
{
    public partial class KuralComponent : UserControl
    {
        public KuralComponent()
        {
            InitializeComponent();
        }

        public KuralComponent(string kural1, string kural2, string kural3) : this()
        {
            this.SuspendLayout();
            label1.Text = kural1;
            label2.Text = kural2;
            label3.Text = kural3;
            this.ResumeLayout();
        }
      
        public KuralComponent(Kural kural) : this()
        {
            SetKural(kural);
        }

        public void SetKural(Kural kural)
        {
            this.SuspendLayout();
            label1.Text = kural.ToString(Enums.InputType.Hassas);
            label2.Text = kural.ToString(Enums.InputType.Miktr);
            label3.Text = kural.ToString(Enums.InputType.Kirli);

            switch (kural.Hassaslık)
            {
                case Enums.Hassas.sağlam:
                    label1.ForeColor = Color.LimeGreen;
                    break;
                case Enums.Hassas.orta:
                    label1.ForeColor = Color.MediumBlue;
                    break;
                case Enums.Hassas.hassas:
                    label1.ForeColor = Color.MediumVioletRed;
                    break;
            }



            switch (kural.Miktar)
            {
                case Enums.Miktr.küçük:
                    label2.ForeColor = Color.LimeGreen;
                    break;
                case Enums.Miktr.orta:
                    label2.ForeColor = Color.MediumBlue;
                    break;
                case Enums.Miktr.büyük:
                    label2.ForeColor = Color.MediumVioletRed;
                    break;
            }
            switch (kural.Kirlilik)
            {
                case Enums.Kirli.küçük:
                    label3.ForeColor = Color.LimeGreen;
                    break;
                case Enums.Kirli.orta:
                    label3.ForeColor = Color.MediumBlue;
                    break;
                case Enums.Kirli.büyük:
                    label3.ForeColor = Color.MediumVioletRed;
                    break;
            }

            label5.Text = kural.GetKesisimX[0].ToString();
            label7.Text = kural.GetKesisimX[1].ToString();
            label9.Text = kural.GetKesisimX[2].ToString();

            label5.Text = label5.Text.Length > 5 ? label5.Text.Substring(0, 5) : label5.Text;
            label7.Text = label7.Text.Length > 5 ? label7.Text.Substring(0, 5) : label7.Text;
            label9.Text = label9.Text.Length > 5 ? label9.Text.Substring(0, 5) : label9.Text;


            this.ResumeLayout();
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
