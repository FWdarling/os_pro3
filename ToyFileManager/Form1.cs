using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToyFileManager;

namespace ToyFileManager
{
    public partial class Form1 : Form
    {
        ToyFileManager MyFileManeger = new ToyFileManager();

        public Form1()
        {
            InitializeComponent();
            MyFileManeger.startToInit();
        }

        private void refreshFolder()
        {
            ListView lv = this.listView;
            lv.Clear();
            ArrayList path = MyFileManeger.filePath;
            for (int i = 0; i < path.Count; i++)
            {

            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MyFileManeger.createFolder("folder1");

        }


    }
}
