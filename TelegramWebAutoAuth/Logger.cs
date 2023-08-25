using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TelegramWebAutoAuth
{
    public static class Logger
    {
        public static RichTextBox logBox;
        public static object locker = new object();

        public static void LogAdd(string text)
        {
            lock(locker)
            {
                string line = $"{DateTime.Now}: {text} {Environment.NewLine}";
                logBox.AppendText(line);
                logBox.ScrollToCaret();

                if (logBox.Lines.Length > 500)
                {
                    DeleteFirstLine();
                }
            }
        }
        public static void LogAdd(string text, Color color)
        {
            lock(locker)
            {
                //logBox.Focus();
                if (logBox.Lines.Length > 500)
                {
                    DeleteFirstLine();
                }

                string line = $"{DateTime.Now}: {text} {Environment.NewLine}";
                logBox.AppendText(line);
                logBox.Select((logBox.TextLength - line.Length) < 0 ? 0 : (logBox.TextLength - line.Length) + 1, line.Length);
                logBox.SelectionColor = color;
                //logBox.ScrollToCaret();
            }
        }
        private static void DeleteFirstLine()
        {
            int start_index = logBox.GetFirstCharIndexFromLine(0);
            int count = logBox.Lines[0].Length;

            if (0 < logBox.Lines.Length - 1)
            {
                count += logBox.GetFirstCharIndexFromLine(1) -
                    ((start_index + count - 1) + 1);
            }

            logBox.Text = logBox.Text.Remove(start_index, count);
        }
    }
}
