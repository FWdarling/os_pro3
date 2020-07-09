using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToyFileManager
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

        private void button1_Click(object sender, EventArgs e)
        {
            int clickcnt = int.Parse((string)this.lab.Text);
            clickcnt++;
            this.lab.Text = clickcnt.ToString();
        }

        private void lab_Click(object sender, EventArgs e)
        {

        }
    }
}
