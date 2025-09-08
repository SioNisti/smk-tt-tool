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
            label1 = new Label();
            button1 = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Super Mario Kart", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(451, 47);
            label1.Name = "label1";
            label1.Size = new Size(337, 175);
            label1.TabIndex = 0;
            label1.Text = "L1 0'00\"00\r\nL2 0'00\"00\r\nL3 0'00\"00\r\nL4 0'00\"00\r\nL5 0'00\"00\r\n\r\nTOTAL 0'00\"00";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            button1.Location = new Point(333, 362);
            button1.Name = "button1";
            button1.Size = new Size(119, 23);
            button1.TabIndex = 1;
            button1.Text = "Attach To System";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Super Mario Kart", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(658, 9);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 2;
            label2.Text = "Lap 0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Super Mario Kart", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(346, 24);
            label3.TabIndex = 3;
            label3.Text = "Character none";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Super Mario Kart", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 47);
            label4.Name = "label4";
            label4.Size = new Size(250, 24);
            label4.TabIndex = 4;
            label4.Text = "Track none";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
