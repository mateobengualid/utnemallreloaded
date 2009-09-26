using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace CommentsWalker
{
    class Walker
    {
        class MatchChange
        {
            public string NewValue;
            public List<Match> Matchs = new List<Match>();

            public MatchChange() { }
            public MatchChange(string newValue)
            {
                this.NewValue = newValue;
            }
            public MatchChange(string newValue, params Match[] matchs)
            {
                this.NewValue = newValue;
                foreach (Match match in matchs)
                    this.Matchs.Add(match);
            }

        }
        string _commentsPattern = @"//.*$|/\*[\n.]*\*/";
        string _currentFile;
        string _currentFileText;
        string[] _files;
        int _currentFileIndex;
        MatchCollection _currentMatch;
        MatchChange _matchChange;
        List<MatchChange> _matchChangeList;
        int _currentMatchIndex;


        public int CurrentFileIndex
        {
            get
            {
                return _currentFileIndex;
            }
        }
        public int CurrentCommentCharacter
        {
            get
            {
                return _matchChange.Matchs[_matchChange.Matchs.Count-1].Index;
            }
        }
        public string OriginalFileText
        {
            get
            {
                return _currentFileText;
            }
        }
        public int FilesCount
        {
            get
            {
                return _files.Length;
            }
        }
        public string[] AllFiles
        {
            get
            {
                return _files;
            }
        }
        public string RootPath
        {
            get;
            set;
        }
        public string CurrentFile
        {
            get
            {
                return _currentFile;
            }
        }

        Dictionary<string, string> _history = new Dictionary<string,string>();

        public string NewComment
        {
            get
            {
                return _matchChange.NewValue;
            }
            set
            {
                _matchChange.NewValue = value;
                string[] parts = value.Split('\n');
                if (parts.Length == _matchChange.Matchs.Count)
                {
                    for(int n = 0 ; n<parts.Length ; n++)
                    {
                        string key = this.RemoveCommentChars(_matchChange.Matchs[n].Value).ToLower();
                        if (!_history.ContainsKey(key))
                        {
                            _history.Add(key, parts[n]);
                        }
                    }
                }
            }
        }
        public string CurrentComment
        {
            get;
            set;
        }
        public void Run()
        {
            _files = Directory.GetFiles(RootPath, "*.cs", SearchOption.AllDirectories);
            ArrayList lista = new ArrayList();
            foreach(string file in _files){
                if(!file.ToLower().EndsWith(".designer.cs"))lista.Add(file);
            }
            _files = (string[])lista.ToArray(typeof(string));
            _currentFileIndex = -1;
        }
        public bool SkipFile()
        {
            _currentFile = String.Empty;
            return NextFile();
        }
        public bool NextFile()
        {
            if (_currentFileIndex > -1 && CurrentFile!= String.Empty) SaveFile(_currentFile);
            if (_currentFileIndex >= _files.Length - 1)
            {
                _currentFile = String.Empty;
                return false;
            }
            _currentFileIndex++;
            // save the file
            _currentFile = _files[_currentFileIndex];
            try
            {
                _currentFileText = File.ReadAllText(_files[_currentFileIndex]);
                _currentMatch = Regex.Matches(_currentFileText, _commentsPattern, RegexOptions.Multiline);
                _matchChangeList = new List<MatchChange>();
                _currentMatchIndex = 0;
                if (_currentMatch.Count == 0) return NextFile();
                LoadComment();
            }
            catch (IOException ioError)
            {
                throw new IOException("Error opening file " + _files[_currentFileIndex] + "." + ioError.Message);
            }
            catch (Exception error)
            {
                throw new Exception("Error opening file " + _files[_currentFileIndex] + "." + error.Message);
            }
            return true;
        }

        private void SaveFile(string filename)
        {            
            string newText = String.Empty;
            int lastIndex = 0;
            foreach (MatchChange change in _matchChangeList)
            {
                int firstIndex = change.Matchs[0].Index;
                newText += _currentFileText.Substring(lastIndex, firstIndex - lastIndex);
                lastIndex = change.Matchs[change.Matchs.Count - 1].Index + change.Matchs[change.Matchs.Count - 1].Length;
                newText += ToComment(change.NewValue, change.Matchs[0].Index);
            }
            newText += _currentFileText.Substring(lastIndex);

            if(File.Exists(filename + ".bak")) File.Delete(filename + ".bak");
            File.Copy(filename, filename + ".bak");
            File.Delete(filename);
            TextWriter writer = new StreamWriter(filename, false, Encoding.UTF8);
            writer.Write(newText);
            writer.Close();
        }

        private string ToComment(string comment, int sourceIndex)
        {
            string[] test = comment.Split('\n');
            string result = "";
            bool xmlComment = false;
            int walkerIndex = sourceIndex;
            string preStr = String.Empty;
            if (sourceIndex > 0)
            {
                while (walkerIndex > 0 && _currentFileText[walkerIndex] != '\n') walkerIndex--;
                preStr = _currentFileText.Substring(walkerIndex + 1, sourceIndex - walkerIndex - 1);
            }
            if (test[0][0] == '<') xmlComment = true;
            int n = 0;
            foreach (string part in test)
            {
                if(n==0)
                    result += (xmlComment ? "/// " : "// ") + part ;
                else
                    result += preStr + (xmlComment ? "///" : "//") + part ;
                n++;
                if (n < test.Length) result += "\n";
                else result += "\r";
            }
            return result;
        }

        private void LoadComment()
        {
            int upperComment = FindMaxUpperComment(_currentMatchIndex);

            _matchChange = new MatchChange();
            _matchChangeList.Add(_matchChange);
            CurrentComment = String.Empty;
            string newComment = String.Empty;
            for (int n = _currentMatchIndex; n < upperComment; n++)
            {
                string part = RemoveCommentChars(_currentMatch[n].Value);
                CurrentComment += part + "\n";
                newComment += (_history.ContainsKey(part.ToLower()) ? _history[part.ToLower()].Replace("\r","") + "\r" : part) + "\n";
                _matchChange.Matchs.Add(_currentMatch[n]);
            }
            _matchChange.NewValue = newComment[0]==' ' ? newComment : " " + newComment;
            _currentMatchIndex = upperComment;
        }

        private string RemoveCommentChars(string commentString)
        {
            if (commentString.StartsWith("//"))
                return commentString.StartsWith("///") ? commentString.Substring(3) : commentString.Substring(2);
            if (commentString.StartsWith("/*")) return commentString.Replace("/*", "").Replace("*/", "");
            return commentString;
        }

        private int FindMaxUpperComment(int index)
        {
            if (index == _currentMatch.Count - 1) return index + 1;
            int from = _currentMatch[index].Index + _currentMatch[index].Length;
            int to = _currentMatch[index+1].Index;
            for (; from < to; from++)
            {
                if (!Char.IsWhiteSpace(_currentFileText[from])) return index + 1;
            }
            return FindMaxUpperComment(index + 1);
        }

        public bool NextComment()
        {
            if (_currentMatch==null || _currentMatchIndex > _currentMatch.Count-1) return false;
            LoadComment();
            return true;
        }
    }
}
