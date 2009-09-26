namespace CommentsWalker
{
    partial class CommentsWalkerWindow
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.textOriginalComment = new System.Windows.Forms.TextBox();
            this.textNewComment = new System.Windows.Forms.TextBox();
            this.textLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelFileIndex = new System.Windows.Forms.Label();
            this.buttonCopyAll = new System.Windows.Forms.Button();
            this.buttonCopyAllFile = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textDirectory
            // 
            this.textDirectory.Location = new System.Drawing.Point(12, 15);
            this.textDirectory.Name = "textDirectory";
            this.textDirectory.Size = new System.Drawing.Size(479, 20);
            this.textDirectory.TabIndex = 0;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(497, 13);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(66, 23);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(721, 12);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(98, 23);
            this.buttonRun.TabIndex = 2;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // textOriginalComment
            // 
            this.textOriginalComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textOriginalComment.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOriginalComment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.textOriginalComment.Location = new System.Drawing.Point(12, 94);
            this.textOriginalComment.Multiline = true;
            this.textOriginalComment.Name = "textOriginalComment";
            this.textOriginalComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textOriginalComment.Size = new System.Drawing.Size(807, 188);
            this.textOriginalComment.TabIndex = 3;
            // 
            // textNewComment
            // 
            this.textNewComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textNewComment.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNewComment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.textNewComment.Location = new System.Drawing.Point(12, 301);
            this.textNewComment.Multiline = true;
            this.textNewComment.Name = "textNewComment";
            this.textNewComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textNewComment.Size = new System.Drawing.Size(807, 208);
            this.textNewComment.TabIndex = 4;
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLog.Location = new System.Drawing.Point(12, 555);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(807, 28);
            this.textLog.TabIndex = 6;
            this.textLog.TextChanged += new System.EventHandler(this.textLog_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Current File:";
            // 
            // labelCurrentFile
            // 
            this.labelCurrentFile.AutoSize = true;
            this.labelCurrentFile.Location = new System.Drawing.Point(140, 47);
            this.labelCurrentFile.Name = "labelCurrentFile";
            this.labelCurrentFile.Size = new System.Drawing.Size(22, 13);
            this.labelCurrentFile.TabIndex = 7;
            this.labelCurrentFile.Text = "C:\\";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Original Comment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 285);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "New Comment:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 539);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Log:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(708, 515);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 21);
            this.button1.TabIndex = 5;
            this.button1.Text = "Change!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelFileIndex
            // 
            this.labelFileIndex.AutoSize = true;
            this.labelFileIndex.Location = new System.Drawing.Point(81, 47);
            this.labelFileIndex.Name = "labelFileIndex";
            this.labelFileIndex.Size = new System.Drawing.Size(24, 13);
            this.labelFileIndex.TabIndex = 12;
            this.labelFileIndex.Text = "0/0";
            // 
            // buttonCopyAll
            // 
            this.buttonCopyAll.Location = new System.Drawing.Point(12, 515);
            this.buttonCopyAll.Name = "buttonCopyAll";
            this.buttonCopyAll.Size = new System.Drawing.Size(110, 21);
            this.buttonCopyAll.TabIndex = 13;
            this.buttonCopyAll.Text = "Copy paste all";
            this.buttonCopyAll.UseVisualStyleBackColor = true;
            this.buttonCopyAll.Click += new System.EventHandler(this.buttonCopyAll_Click);
            // 
            // buttonCopyAllFile
            // 
            this.buttonCopyAllFile.Location = new System.Drawing.Point(128, 515);
            this.buttonCopyAllFile.Name = "buttonCopyAllFile";
            this.buttonCopyAllFile.Size = new System.Drawing.Size(136, 21);
            this.buttonCopyAllFile.TabIndex = 14;
            this.buttonCopyAllFile.Text = "Copy all in this file";
            this.buttonCopyAllFile.UseVisualStyleBackColor = true;
            this.buttonCopyAllFile.Click += new System.EventHandler(this.buttonCopyAllFile_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(569, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 21);
            this.button2.TabIndex = 15;
            this.button2.Text = "CFLTC";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSkip
            // 
            this.buttonSkip.Location = new System.Drawing.Point(270, 515);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(125, 20);
            this.buttonSkip.TabIndex = 16;
            this.buttonSkip.Text = "Skip File!";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // CommentsWalkerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 594);
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonCopyAllFile);
            this.Controls.Add(this.buttonCopyAll);
            this.Controls.Add(this.labelFileIndex);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCurrentFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.textNewComment);
            this.Controls.Add(this.textOriginalComment);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textDirectory);
            this.Name = "CommentsWalkerWindow";
            this.Text = "Comments Walker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textDirectory;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.TextBox textOriginalComment;
        private System.Windows.Forms.TextBox textNewComment;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCurrentFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelFileIndex;
        private System.Windows.Forms.Button buttonCopyAll;
        private System.Windows.Forms.Button buttonCopyAllFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSkip;
    }
}

