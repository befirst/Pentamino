using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PentaminoConsole
{
    class Program
    {
        static int CheckPentamino(int[,] pentamino, int[,] pic)
        {

            return 0;
        }

        static void Main(string[] args)
        {
            char[,] source;
            System.Console.WriteLine("Please insert path to file or 0 for using default path: ");
            string path = System.Console.ReadLine();
            int[,] pentamino1 = { {1,1,1 }, {1,0,0 }, {1,0,0 } };
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
            System.Console.ReadLine();
        }
    }
}
