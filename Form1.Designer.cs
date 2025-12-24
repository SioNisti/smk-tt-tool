namespace smk_tt_tool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            appContextMenu = new ContextMenuStrip(components);
            connectToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            appContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ContextMenuStrip = appContextMenu;
            label1.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(182, 203);
            label1.TabIndex = 0;
            label1.Text = "L1 0'00\"00\r\nL2 0'00\"00\r\nL3 0'00\"00\r\nL4 0'00\"00\r\nL5 0'00\"00\r\n\r\nTOTAL 0'00\"00";
            label1.TextAlign = ContentAlignment.MiddleRight;
            label1.MouseDown += Form1_MouseDown;
            // 
            // appContextMenu
            // 
            appContextMenu.Items.AddRange(new ToolStripItem[] { connectToolStripMenuItem, exitToolStripMenuItem1 });
            appContextMenu.Name = "appContextMenu";
            appContextMenu.Size = new Size(120, 48);
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(119, 22);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(119, 22);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ContextMenuStrip = appContextMenu;
            label2.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(116, 9);
            label2.Name = "label2";
            label2.Size = new Size(78, 29);
            label2.TabIndex = 2;
            label2.Text = "Lap 0";
            label2.MouseDown += Form1_MouseDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ContextMenuStrip = appContextMenu;
            label3.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 241);
            label3.Name = "label3";
            label3.Size = new Size(78, 29);
            label3.TabIndex = 3;
            label3.Text = "Mario";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.MouseDown += Form1_MouseDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ContextMenuStrip = appContextMenu;
            label4.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(12, 9);
            label4.Name = "label4";
            label4.Size = new Size(52, 29);
            label4.TabIndex = 4;
            label4.Text = "MC3";
            label4.MouseDown += Form1_MouseDown;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(212, 291);
            ContextMenuStrip = appContextMenu;
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "SMK TT Tool";
            Load += Form1_Load;
            MouseDown += Form1_MouseDown;
            appContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ContextMenuStrip appContextMenu;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
    }
}
