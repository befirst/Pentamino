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
            string result = "";//для вывода ячеек которые занимает пентамино в одном положении
            for (int y = 0; y < figure.GetLength(0) - pentamino.GetLength(0) + 1; y++)
                for (int x = 0; x < figure.GetLength(1) - pentamino.GetLength(1) + 1; x++)
                {                    
                    int g = 0;//пентамино состоят из 5 квадратиков, если 5 квадратиков совпадают с маской то все хорошо                    
                    string newPlaceIndexes = "";//хранитель индексов занятых ячеек
                    //новое положение 
                    for (int i = 0; i < pentamino.GetLength(0); i++)
                        for (int j = 0; j < pentamino.GetLength(1); j++)
                        {
                            if (pentamino[i, j] == 1 && figure[i + y, j + x] == 1)
                            {
                                newPlaceIndexes += (i + y) + "," + (j + x) + ";";
                                g++;
                            }                            
                        }
                    if (g == 5)
                    {
                        Row newPlace = list.AddRow();//создаем новую строку в списке, которая означает новое положение пентамино
                        string[] temp = newPlaceIndexes.Split(';');                        
                        result += count + ": ";
                        for (int i = 0; i<5; i++)
                        {
                            string[] splited = temp[i].Split(',');
                            list.AddNode(newPlace, Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]));//для каждой ячейки создаем узел на пересечении ее индексов, означает занятую ячейку
                            result += splited[0] + "," + splited[1] + "; ";
                        }
                        result += Environment.NewLine;                        
                        count++;
                    }
                    newPlaceIndexes = "";
                }

            return result;
        }

        static void Main(string[] args)
        {
            char[,] source;
            System.Console.WriteLine("Please insert path to file or 0 for using default path: ");
            string path = System.Console.ReadLine();
            int[,] vPentamino = { {1,1,1 },
                                  {1,0,0 },
                                  {1,0,0 } };
            int[,] v90Pentamino = { {0,0,1 },
                                    {0,0,1 },
                                    {1,1,1 } };
            int[,] iPentamino = { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } };
            int[,] i90Pentamino = { { 1, 1, 1, 1, 1 } };
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
            System.Console.WriteLine(CheckPentamino(vPentamino, binarySource, dl));
            System.Console.WriteLine(dl.PrintTableHeader());
            System.Console.WriteLine(CheckPentamino(v90Pentamino, binarySource, dl));
            System.Console.WriteLine(CheckPentamino(iPentamino, binarySource, dl));
            System.Console.WriteLine(CheckPentamino(i90Pentamino, binarySource, dl));
            Pentaminos p = new Pentaminos();
            System.Console.ReadLine();
        }
    }
}
