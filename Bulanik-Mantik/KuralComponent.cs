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
