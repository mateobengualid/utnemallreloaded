using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CommentsWalker
{
    public partial class CommentsWalkerWindow : Form
    {
        Walker _walker = new Walker();
        public CommentsWalkerWindow()
        {
            InitializeComponent();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                this.textDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            string path = this.textDirectory.Text;
            if (path == String.Empty)
            {
                MessageBox.Show("Insert a valid path.");
                return;
            }
            _walker = new Walker();
            _walker.RootPath = path;
            _walker.Run();
            this.textNewComment.Text = this.textOriginalComment.Text = String.Empty;
            ClearLog();
            button1_Click(sender, e);
        }

        private void ClearLog()
        {
            this.textLog.Text = String.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textNewComment.Text.Trim() != String.Empty)
                {
                    _walker.NewComment = this.textNewComment.Text.Trim();
                }
                if (_walker.NextComment())
                {
                    FillTexts();
                }
                else
                {
                    if (_walker.NextFile())
                    {
                        UpdateNextFile();
                    }
                    else
                    {
                        MessageBox.Show("All files explored");
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("ERROR: " + error.Message);
            }
        }

        private void UpdateNextFile()
        {
            this.textOriginalComment.Text = _walker.OriginalFileText;
            FillTexts();
            this.labelFileIndex.Text = _walker.CurrentFileIndex + "/" + _walker.FilesCount;
            this.labelCurrentFile.Text = _walker.CurrentFile;
            AddLog(_walker.CurrentFile);
        }

        private void FillTexts()
        {
            if(this.textOriginalComment.Text==String.Empty)
                this.textOriginalComment.Text = _walker.OriginalFileText;
            this.textOriginalComment.SelectionStart = _walker.CurrentCommentCharacter;
            this.textOriginalComment.ScrollToCaret();
            this.textNewComment.Text = _walker.NewComment;
            this.textNewComment.SelectAll();
            this.textNewComment.Focus();
        }

        private void AddLog(string text)
        {
            this.textLog.Text += text + "\n";
        }

        private void textLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonCopyAll_Click(object sender, EventArgs e)
        {
            while (_walker.CurrentFileIndex < _walker.FilesCount-1)
            {
                this.button1_Click(sender, e);
            }
        }

        private void buttonCopyAllFile_Click(object sender, EventArgs e)
        {
            while (_walker.NextComment())
            {                
                FillTexts();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_walker.AllFiles == null) return;
            string retStr = String.Empty;
            foreach (string file in _walker.AllFiles)
            {
                retStr += file + "\n";
            }
            Clipboard.SetText(retStr);
            MessageBox.Show(retStr);
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            try
            {
                _walker.SkipFile();
                UpdateNextFile();
            }
            catch (Exception error)
            {
                MessageBox.Show("ERROR: " + error.Message);
            }
        }
    }
}
