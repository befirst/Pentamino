using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PentaminoConsole
{
    class Program
    {
        static string CheckPentamino(int[,] pentamino, int[,] figure, DancingLinks list)
        {
            int count = 0;//количество положений в котором может распологаться пентамино
            string result = "";
            for (int y = 0; y < figure.GetLength(0) - pentamino.GetLength(0) + 1; y++)
                for (int x = 0; x < figure.GetLength(1) - pentamino.GetLength(1) + 1; x++)
                {                    
                    int g = 0;
                    Row newPlace = list.AddRow();                    
                    Dictionary<int, int> newPlaceIndexes = new Dictionary<int, int>();//использовать строку вместо словаря и потом сплитом разделить
                    //новое положение 
                    for (int i = 0; i < pentamino.GetLength(0); i++)
                        for (int j = 0; j < pentamino.GetLength(1); j++)
                        {
                            if (pentamino[i, j] == 1 && figure[i + y, j + x] == 1)
                            //поменять значения в dancinglinks, запомнить положение
                            {
                                newPlaceIndexes.Add(i + y, j + x);
                                g++;
                            }                            
                        }
                    if (g == 5)
                    {                        
                        foreach (var i in newPlaceIndexes)
                        {
                            list.AddNode(newPlace, i.Key, i.Value);
                            result += count + ": " + i.Key + ", " + i.Value + Environment.NewLine;
                        }
                        newPlaceIndexes.Clear();
                        count++;
                    }
                    else
                        newPlaceIndexes.Clear();
                }

            return result;
        }

        static void Main(string[] args)
        {
            char[,] source;
            System.Console.WriteLine("Please insert path to file or 0 for using default path: ");
            string path = System.Console.ReadLine();
            int[,] pentamino1 = { {1,1,1 },
                                  {1,0,0 },
                                  {1,0,0 } };
            //сделать выполнение для каждого файла в базовой директории
            if (path == "0")
                source = SourceReader.GetSource("1.in");
            else
                source = SourceReader.GetSource(path);
            int[,] binarySource = new int[source.GetLength(0), source.GetLength(1)];
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    if (source[i, j] == 'o')
                        binarySource[i, j] = 1;
                    else
                        binarySource[i, j] = 0;
                    System.Console.Write(binarySource[i, j] + "");
                }
                System.Console.WriteLine();
            }            
            DancingLinks dl = new DancingLinks(binarySource.GetLength(0), binarySource.GetLength(1));
            System.Console.WriteLine(CheckPentamino(pentamino1, binarySource, dl));
            System.Console.WriteLine(dl.PrintTableHeader());
            System.Console.ReadLine();
        }
    }
}
