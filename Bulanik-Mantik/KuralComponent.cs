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
            this.SuspendLayout();
            label1.Text = kural.ToString(Enums.InputType.Hassas);
            label2.Text = kural.ToString(Enums.InputType.Miktr);
            label3.Text = kural.ToString(Enums.InputType.Kirli);


            label5.Text = kural.x1.ToString();
            label7.Text = kural.x2.ToString();
            label9.Text = kural.x3.ToString();



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
    }
}
