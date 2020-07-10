using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToyFileManager;
using Microsoft.VisualBasic;

namespace ToyFileManager
{
    public partial class Form1 : Form
    {
        ToyFileManager MyFileManeger = new ToyFileManager();
        string path = "root";
        ListViewGroup file = new ListViewGroup();
        ListViewGroup folder = new ListViewGroup();

        public Form1()
        {
            InitializeComponent();
            MyFileManeger.startToInit();
            file.Header = "file";
            folder.Header = "folder";
            this.listView1.Groups.Add(file);
            this.listView1.Groups.Add(folder);

        }

        private void refreshFolder(bool type)
        {

            if (path != null && path.Length > 4)
                path = path.Substring(0, 4);
            ArrayList pathList = MyFileManeger.filePath;

            for (int i = 1; i < pathList.Count; i++)
            {
                string filename = (string)pathList[i];
                path += ("/" + filename);
            }
            this.filePath.Text = path;
            if (type == true)
            {
                this.textBox2.Visible = false;
                this.listView1.Visible = true;
                this.listView1.Clear();
                string[] contentList = MyFileManeger.getFolderContent();
                this.listView1.BeginUpdate();
                for (int i = 0; i < contentList.Length; i += 2)
                {
                    ListViewItem lvi = new ListViewItem();
                    if (contentList[i] == "0")
                    {
                        lvi.Group = file;
                        lvi.ImageIndex = 0;
                    }
                    else
                    {
                        lvi.Group = folder;
                        lvi.ImageIndex = 1;
                    }
                    lvi.Text = contentList[i + 1];
                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();
            }
            else
            {
                this.listView1.Visible = false;
                this.textBox2.Visible = true;
                this.textBox2.Text = MyFileManeger.getFileContent();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshFolder(true);
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            ListView lv = this.listView1;
            if (lv.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lv.SelectedItems[0];
                string name = lvi.Text;
                if (!MyFileManeger.delete(name))
                {
                    MessageBox.Show("please select a folder or a file!");
                }
                refreshFolder(true);
            }
            else
            {
                MessageBox.Show("please select a folder or a file!");
            }
        }

        private void btCreateFile_Click(object sender, EventArgs e)
        {
            
            string name = Interaction.InputBox("input file name(Only supports English and no more than 12 characters)", "create file");
            if (!MyFileManeger.createFile(name))
            {
                MessageBox.Show("create failed!");
            }
            refreshFolder(MyFileManeger.currentFCB.type);
        }

        private void btCreateFolder_Click(object sender, EventArgs e)
        {
            string name = Interaction.InputBox("input file name(Only supports English and no more than 12 characters)", "create folder");
            if (!MyFileManeger.createFolder(name))
            {
                MessageBox.Show("create failed!");
            }
            refreshFolder(MyFileManeger.currentFCB.type);
        }

        private void btWrite_Click(object sender, EventArgs e)
        {
            string content = Interaction.InputBox("input in English", "write file");
            if (!MyFileManeger.writeFile(content))
            {
                MessageBox.Show("create failed!");
            }
            refreshFolder(MyFileManeger.currentFCB.type);
        }

        private void btRename_Click(object sender, EventArgs e)
        {
            ListView lv = this.listView1;
            if (lv.SelectedItems.Count > 0)
            {
                string newName = Interaction.InputBox("input file name(Only supports English and no more than 12 characters)", "rename");
                ListViewItem lvi = lv.SelectedItems[0];
                string name = lvi.Text;
                if (MyFileManeger.rename(name, newName))
                {
                    bool type = MyFileManeger.currentFCB.type;
                    refreshFolder(type);
                }
                else
                {
                    MessageBox.Show("please select a folder or a file!");
                }

            }
            else
            {
                MessageBox.Show("please select a folder or a file!");
            }
            
        }

        private void btReturn_Click(object sender, EventArgs e)
        {
            MyFileManeger.backUp();
            refreshFolder(MyFileManeger.currentFCB.type);
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            FileStream dataFile = new FileStream(MyFileManeger.dataFileName, FileMode.OpenOrCreate, FileAccess.Write);

            BinaryWriter dataBinaryWriter = new BinaryWriter(dataFile);
            int i;
            for (i = 0; i < ToyFileManager.blockNum * ToyFileManager.blockSize / 8; i++) ;
            {
                byte t = 0;
                if (MyFileManeger.disk[i * 8 + 7]) t ^= 1;
                if (MyFileManeger.disk[i * 8 + 6]) t ^= 1 << 1;
                if (MyFileManeger.disk[i * 8 + 5]) t ^= 1 << 2;
                if (MyFileManeger.disk[i * 8 + 4]) t ^= 1 << 3;
                if (MyFileManeger.disk[i * 8 + 3]) t ^= 1 << 4;
                if (MyFileManeger.disk[i * 8 + 2]) t ^= 1 << 5;
                if (MyFileManeger.disk[i * 8 + 1]) t ^= 1 << 6;
                if (MyFileManeger.disk[i * 8]) t ^= 1 << 7;
                dataBinaryWriter.Write(t);
            }
            dataFile.Close();
            MessageBox.Show("Saved successfully!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
       

        private void btFormat_Click(object sender, EventArgs e)
        {
            string text = Interaction.InputBox("Are you sure to format?(Yes or No)", "format");
            if(text == "Yes") MyFileManeger.format();
            refreshFolder(MyFileManeger.currentFCB.type);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = this.listView1;
            if (lv.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lv.SelectedItems[0];
                string name = lvi.Text;
                this.tips.Text = "focus: " + name;
            }
            else
            {
                this.tips.Text = "ready!";
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView lv = this.listView1;
            ListViewItem lvi = lv.SelectedItems[0];
            string name = lvi.Text;
            if (MyFileManeger.openFile(name))
            {
                bool type = MyFileManeger.currentFCB.type;
                refreshFolder(type);
            }
            else
            {
                MessageBox.Show("please select a folder or a file!");
            }
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            ListView lv = this.listView1;
            if(lv.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lv.SelectedItems[0];
                string name = lvi.Text;
                if (MyFileManeger.openFile(name))
                {
                    bool type = MyFileManeger.currentFCB.type;
                    refreshFolder(type);
                }
                else
                {
                    MessageBox.Show("please select a folder or a file!");
                }

            }
            else
            {
                MessageBox.Show("please select a folder or a file!");
            }
        }
    }
}
