// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SilverFactorial
{
    // This delegate enables asynchronous calls for setting
    // the text property on a TextBox control.
    delegate void AppendTextCallback(string text);

    public class LoggedTextBox 
    {
        StreamWriter streamWriter;

        TextBox textBox;
        AppendTextCallback appendText;

        public LoggedTextBox(TextBox textBox, string name)
        {
            string outputDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BenchmarkApplication\";
            DirectoryInfo df = new DirectoryInfo(outputDir);
            if (! df.Exists) 
            {
                df = Directory.CreateDirectory(df.FullName);
            }

            string fileName = string.Format(df.FullName + "BenchmarkApplication{0}.log", DateTime.Now.ToFileTime());
            FileStream logFile = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
            streamWriter = new StreamWriter(logFile);
            this.textBox = textBox;

            appendText = delegate(string text) 
            {
                textBox.AppendText(text);
                textBox.ScrollToEnd();
                textBox.Focus();
            };
        }

        private void AddToBox(string str)
        {
            textBox.Dispatcher.Invoke(appendText, str);
        }

        public StreamWriter GetStreamWriter() 
        { 
            return streamWriter; 
        }

        public void Write(string str)
        {
            if (LogToFile) streamWriter.Write(str);
            AddToBox(str);
        }

        public void WriteFlush(string str)
        {
            if (LogToFile) streamWriter.Write(str);
            AddToBox(str);
        }

        public void WriteLine(string str)
        {
            if (LogToFile) streamWriter.WriteLine(str);
            AddToBox(str);
        }

        public void WriteLine(string format, string message)
        {
            string str = string.Format(format, message) + Environment.NewLine;
            if (LogToFile) streamWriter.WriteLine(str);
            AddToBox(str);
        }

        public void WriteLine()
        {
            if (LogToFile) streamWriter.Write(streamWriter.NewLine);
            AddToBox(Environment.NewLine);
        }

        public void WriteHot(string str)
        {
            if (LogToFile) streamWriter.WriteLine(str);
            AddToBox(str);
        }

        public void WriteHotLine(string str)
        {
            if (LogToFile) streamWriter.WriteLine(str);
            AddToBox(str);
        }

        public void WriteRed(string str)
        {
            if (LogToFile) streamWriter.Write(str);
            AddToBox(str);
        }

        public void WriteRedLine(string str)
        {
            if (LogToFile) streamWriter.WriteLine(str);
            AddToBox(str);
        }

        public void Flush()
        {
            streamWriter.Flush();
        }

        public void Dispose()
        {
            streamWriter.Flush();
            streamWriter.Close();
        }

        public bool LogToFile
        {
            get;
            set;  
        }
    }
}   // endOfLoggedTextBox
