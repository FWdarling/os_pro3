namespace ToyFileManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btOpne = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btCreateFile = new System.Windows.Forms.Button();
            this.btCreateFolder = new System.Windows.Forms.Button();
            this.btWrite = new System.Windows.Forms.Button();
            this.btRename = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btFormat = new System.Windows.Forms.Button();
            this.btReturn = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btRename);
            this.groupBox2.Controls.Add(this.btWrite);
            this.groupBox2.Controls.Add(this.btCreateFolder);
            this.groupBox2.Controls.Add(this.btCreateFile);
            this.groupBox2.Controls.Add(this.btDelete);
            this.groupBox2.Controls.Add(this.btOpne);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btOpne
            // 
            resources.ApplyResources(this.btOpne, "btOpne");
            this.btOpne.Name = "btOpne";
            this.btOpne.UseVisualStyleBackColor = true;
            // 
            // btDelete
            // 
            resources.ApplyResources(this.btDelete, "btDelete");
            this.btDelete.Name = "btDelete";
            this.btDelete.UseVisualStyleBackColor = true;
            // 
            // btCreateFile
            // 
            resources.ApplyResources(this.btCreateFile, "btCreateFile");
            this.btCreateFile.Name = "btCreateFile";
            this.btCreateFile.UseVisualStyleBackColor = true;
            // 
            // btCreateFolder
            // 
            resources.ApplyResources(this.btCreateFolder, "btCreateFolder");
            this.btCreateFolder.Name = "btCreateFolder";
            this.btCreateFolder.UseVisualStyleBackColor = true;
            // 
            // btWrite
            // 
            resources.ApplyResources(this.btWrite, "btWrite");
            this.btWrite.Name = "btWrite";
            this.btWrite.UseVisualStyleBackColor = true;
            // 
            // btRename
            // 
            resources.ApplyResources(this.btRename, "btRename");
            this.btRename.Name = "btRename";
            this.btRename.UseVisualStyleBackColor = true;
            // 
            // btExit
            // 
            this.btExit.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.btExit, "btExit");
            this.btExit.Name = "btExit";
            this.btExit.UseVisualStyleBackColor = false;
            // 
            // btFormat
            // 
            this.btFormat.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.btFormat, "btFormat");
            this.btFormat.Name = "btFormat";
            this.btFormat.UseVisualStyleBackColor = false;
            // 
            // btReturn
            // 
            resources.ApplyResources(this.btReturn, "btReturn");
            this.btReturn.Name = "btReturn";
            this.btReturn.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.ForeColor = System.Drawing.SystemColors.Window;
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("listView1.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("listView1.Groups1")))});
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items"))),
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("listView1.Items1")))});
            this.listView1.LargeImageList = this.imageList2;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "file.png");
            this.imageList2.Images.SetKeyName(1, "folder.png");
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btReturn);
            this.Controls.Add(this.btFormat);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Name = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox filePathText;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button renameBt;
        private System.Windows.Forms.Button writeBt;
        private System.Windows.Forms.Button createFloderBt;
        private System.Windows.Forms.Button createFileBt;
        private System.Windows.Forms.Button deleteBt;
        private System.Windows.Forms.Button openBt;
        private System.Windows.Forms.Button exitBt;
        private System.Windows.Forms.Button formatBt;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btRename;
        private System.Windows.Forms.Button btWrite;
        private System.Windows.Forms.Button btCreateFolder;
        private System.Windows.Forms.Button btCreateFile;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btOpne;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btFormat;
        private System.Windows.Forms.Button btReturn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList2;
    }
}

