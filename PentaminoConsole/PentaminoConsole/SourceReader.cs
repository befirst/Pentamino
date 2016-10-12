using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace PentaminoConsole
{
    class SourceReader
    {
        private static SourceReader sr = new SourceReader();
        private static string[] stringResultArray;
        private static char[,] resultArray;
        private SourceReader() { }
        public static char[,] GetSource(string path)
        {
            string temp = "";
            int rowCount = 0;
            int columnCount = 0;
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    if (rowCount == 0)
                        temp += reader.ReadLine();
                    else
                        temp += "\n" + reader.ReadLine();
                    rowCount++;
                }
            }
            stringResultArray = temp.Split('\n');
            foreach (var i in stringResultArray)
                if (columnCount < i.Length)
                    columnCount = i.Length;
            resultArray = new char[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                {
                    if (j < stringResultArray[i].Length)
                        resultArray[i, j] = stringResultArray[i][j];
                    else
                        resultArray[i, j] = ' ';
                }
            return resultArray;
        }
        public static void CreateSolutionFile(string solutions)
        {
            string userName = Environment.UserName;
            string textFileName = @"C:\Users\" + userName + @"\Desktop\solutions.out";
            FileStream f = new FileStream(textFileName, FileMode.Create, FileAccess.Write);
            using (StreamWriter textFile = new StreamWriter(f))
            {
                textFile.WriteLine(solutions);
            }

            Process.Start(@"C:\Windows\System32\notepad.exe", textFileName);
        }
    }
}
