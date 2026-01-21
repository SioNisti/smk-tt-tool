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
            raceSplitsLabel = new Label();
            appContextMenu = new ContextMenuStrip(components);
            connectToolStripMenuItem = new ToolStripMenuItem();
            palToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem1 = new ToolStripMenuItem();
            courseLabel = new Label();
            attemptLabel = new Label();
            prLabel = new Label();
            sessionBestsLabel = new Label();
            sessionAttemptLabel = new Label();
            appContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // raceSplitsLabel
            // 
            raceSplitsLabel.AutoSize = true;
            raceSplitsLabel.ContextMenuStrip = appContextMenu;
            raceSplitsLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            raceSplitsLabel.ForeColor = Color.White;
            raceSplitsLabel.Location = new Point(12, 9);
            raceSplitsLabel.Name = "raceSplitsLabel";
            raceSplitsLabel.Size = new Size(182, 174);
            raceSplitsLabel.TabIndex = 0;
            raceSplitsLabel.Text = "L1 0'00\"00\r\nL2 0'00\"00\r\nL3 0'00\"00\r\nL4 0'00\"00\r\nL5 0'00\"00\r\nTOTAL 0'00\"00";
            raceSplitsLabel.TextAlign = ContentAlignment.MiddleRight;
            raceSplitsLabel.MouseDown += Form1_MouseDown;
            // 
            // appContextMenu
            // 
            appContextMenu.Items.AddRange(new ToolStripItem[] { connectToolStripMenuItem, palToolStripMenuItem, copyToolStripMenuItem, exitToolStripMenuItem1 });
            appContextMenu.Name = "appContextMenu";
            appContextMenu.Size = new Size(137, 92);
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(136, 22);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // palToolStripMenuItem
            // 
            palToolStripMenuItem.CheckOnClick = true;
            palToolStripMenuItem.Name = "palToolStripMenuItem";
            palToolStripMenuItem.Size = new Size(136, 22);
            palToolStripMenuItem.Text = "Use PAL";
            palToolStripMenuItem.CheckStateChanged += palToolStripMenuItem_CheckStateChanged;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(136, 22);
            copyToolStripMenuItem.Text = "Copy Times";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem1
            // 
            exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            exitToolStripMenuItem1.Size = new Size(136, 22);
            exitToolStripMenuItem1.Text = "Exit";
            exitToolStripMenuItem1.Click += exitToolStripMenuItem1_Click;
            // 
            // courseLabel
            // 
            courseLabel.AutoSize = true;
            courseLabel.ContextMenuStrip = appContextMenu;
            courseLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            courseLabel.ForeColor = Color.White;
            courseLabel.Location = new Point(12, 205);
            courseLabel.Name = "courseLabel";
            courseLabel.Size = new Size(52, 29);
            courseLabel.TabIndex = 4;
            courseLabel.Text = "MC3";
            courseLabel.MouseDown += Form1_MouseDown;
            // 
            // attemptLabel
            // 
            attemptLabel.ContextMenuStrip = appContextMenu;
            attemptLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            attemptLabel.ForeColor = Color.White;
            attemptLabel.Location = new Point(64, 204);
            attemptLabel.Name = "attemptLabel";
            attemptLabel.Size = new Size(130, 29);
            attemptLabel.TabIndex = 5;
            attemptLabel.Text = "0/0";
            attemptLabel.TextAlign = ContentAlignment.MiddleRight;
            attemptLabel.MouseDown += Form1_MouseDown;
            // 
            // prLabel
            // 
            prLabel.AutoSize = true;
            prLabel.ContextMenuStrip = appContextMenu;
            prLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel);
            prLabel.ForeColor = Color.White;
            prLabel.Location = new Point(12, 234);
            prLabel.Name = "prLabel";
            prLabel.Size = new Size(182, 58);
            prLabel.TabIndex = 6;
            prLabel.Text = "5lap: 0'00\"00\r\nFlap: 0'00\"00";
            prLabel.MouseDown += Form1_MouseDown;
            // 
            // sessionBestsLabel
            // 
            sessionBestsLabel.AutoSize = true;
            sessionBestsLabel.ContextMenuStrip = appContextMenu;
            sessionBestsLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel);
            sessionBestsLabel.ForeColor = Color.White;
            sessionBestsLabel.Location = new Point(12, 303);
            sessionBestsLabel.Name = "sessionBestsLabel";
            sessionBestsLabel.Size = new Size(182, 87);
            sessionBestsLabel.TabIndex = 7;
            sessionBestsLabel.Text = "Now:\r\n5lap: 0'00\"00\r\nFlap: 0'00\"00\r\n";
            sessionBestsLabel.MouseDown += Form1_MouseDown;
            // 
            // sessionAttemptLabel
            // 
            sessionAttemptLabel.ContextMenuStrip = appContextMenu;
            sessionAttemptLabel.Font = new Font("Cascadia Mono", 22F, FontStyle.Regular, GraphicsUnit.Pixel, 0);
            sessionAttemptLabel.ForeColor = Color.White;
            sessionAttemptLabel.Location = new Point(66, 303);
            sessionAttemptLabel.Name = "sessionAttemptLabel";
            sessionAttemptLabel.Size = new Size(130, 29);
            sessionAttemptLabel.TabIndex = 8;
            sessionAttemptLabel.Text = "0/0";
            sessionAttemptLabel.TextAlign = ContentAlignment.MiddleRight;
            sessionAttemptLabel.MouseDown += Form1_MouseDown;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.Black;
            ClientSize = new Size(208, 405);
            ContextMenuStrip = appContextMenu;
            Controls.Add(sessionAttemptLabel);
            Controls.Add(sessionBestsLabel);
            Controls.Add(prLabel);
            Controls.Add(attemptLabel);
            Controls.Add(courseLabel);
            Controls.Add(raceSplitsLabel);
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

        private Label raceSplitsLabel;
        private Label courseLabel;
        private ContextMenuStrip appContextMenu;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private Label attemptLabel;
        private Label prLabel;
        private ToolStripMenuItem palToolStripMenuItem;
        private Label sessionBestsLabel;
        private Label sessionAttemptLabel;
    }
}
