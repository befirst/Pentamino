using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PentaminoConsole
{
    class Program
    {
        static string CheckPentamino(int[,] pentamino, string pentaminoName, int[,] figure, DancingLinks list)
        {
            int count = 0;//количество положений в котором может распологаться пентамино
            string result = "";//для вывода ячеек которые занимает пентамино в одном положении

            for (int y = 0; y < figure.GetLength(0) - pentamino.GetLength(0) + 1; y++)
                for (int x = 0; x < figure.GetLength(1) - pentamino.GetLength(1) + 1; x++)
                {
                    int g = 0;//пентамино состоят из 5 квадратиков, если эти 5 квадратиков совпадают с маской то все хорошо                    
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
                        Row newPlace = list.AddRow(pentaminoName);//создаем новую строку в списке, которая означает новое положение пентамино
                        string[] temp = newPlaceIndexes.Split(';');
                        result += count + ": ";
                        for (int i = 0; i < 5; i++)
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
            //System.Console.WriteLine(CheckPentamino(vPentamino, binarySource, dl));
            
            System.Console.WriteLine(dl.PrintTableHeader());//проверка добавления всех клеток
            System.Console.ReadLine();
            Pentaminos p = new Pentaminos();
            foreach (var x in p.pentaminoList)//добавление данных о положении пентамино
            {
                System.Console.WriteLine(x.name);
                foreach (var y in x.data)
                    System.Console.WriteLine(CheckPentamino(y, x.name, binarySource, dl));
                System.Console.ReadLine();
            }
            dl.RemoveColWithZero();//удалить все незаполняемые клетки
            System.Console.WriteLine(dl.PrintTableHeader());//проверка удаления нужных клеток
            System.Console.WriteLine(dl.PrintTableLefter());
            System.Console.ReadLine();
            Tree tree = new Tree();//создание дерева
            //System.Console.WriteLine(dl.FindMinCol().x + ":" + dl.FindMinCol().y + "length= " + dl.FindMinCol().length);//проверка функции нахождения столбца с минимальным количеством возможных положений
            List<char[,]> solutions = FindSolutions(tree, dl, p);
            System.Console.ReadLine();
        }

        static List<char[,]> FindSolutions(Tree tree, DancingLinks list, Pentaminos pentaminos)
        {
            List<char[,]> solutions = new List<char[,]>();//все возможные решения
            Stack<Row> deletedRows = new Stack<Row>();//стек удаленных строк, из него восстанавливаем список
            Stack<Col> deletedCols = new Stack<Col>();//стек удаленных столбцов, из него восстанавливаем список
            int depth = 0;//глубина в которой мы находимся
            int[] backRows = new int[12];
            int[] backCols = new int[12];
            list.cCurrent = list.cFirst;
            TreeNode tnCurrent = tree.root;
            //while (list.cCurrent != null)
            //{

            //    list.cCurrent = list.cCurrent._right;
            //}

            System.Console.WriteLine("Begin");

            XAlgorithm(list, tnCurrent, deletedRows, backRows, deletedCols, backCols, depth);

            return solutions;
        }
        static void XAlgorithm(DancingLinks list, TreeNode currentNode, Stack<Row> deletedRows, int[] backRows, Stack<Col> deletedCols, int[] backCols, int depth)
        {
            backRows[depth] = deletedRows.Count;
            backCols[depth] = deletedCols.Count;
            while (list.cFirst != null)
            {
                System.Console.WriteLine(depth + " :не пустой");
                System.Console.ReadLine();
                Col chosenCol = list.FindMinCol();//1
                System.Console.WriteLine("первый шаг: " + chosenCol.x + ":" + chosenCol.y);
                System.Console.ReadLine();
                if (chosenCol.length != 0 && chosenCol!=null)
                {
                    Row chosenRow = list.FindNotUsedRowInCol(chosenCol);//2                    
                    if (chosenRow != null)
                    {
                        chosenRow.used = true;
                        System.Console.WriteLine("второй шаг: " + chosenRow.name + ":" + chosenRow.id);
                        System.Console.ReadLine();
                        TreeNode temp = new TreeNode(currentNode, chosenRow.name, chosenRow.id);
                        //currentNode.children.Add(temp);//3
                        System.Console.WriteLine("ребенок добавлен");
                        System.Console.ReadLine();
                        Row iter = list.rFirst;
                        while (iter != null)//4
                        {
                            if (iter != chosenRow && iter.name == chosenRow.name)
                            {
                                list.RemoveRow(iter);/////////////////
                                deletedRows.Push(iter);
                                //backRows++;
                                System.Console.WriteLine("удалена строка: " + iter.name + ":" + iter.id);
                            }
                            iter = iter._down;
                        }
                        System.Console.WriteLine("выполнен шаг 4, удалены все одноименные строки");
                        System.Console.ReadLine();
                        foreach (var x in list.FindAllColInRow(chosenRow))//5
                        {
                            foreach (var y in list.FindAllRowInCol(x))
                                if (y != chosenRow && y.deleted == false)
                                {
                                    list.RemoveRow(y);/////////////////
                                    deletedRows.Push(y);
                                    //backRows++;
                                    System.Console.WriteLine("удалена строка: " + y.name + ":" + y.id);
                                }
                            if (x.deleted == false)
                            {
                                list.RemoveCol(x);/////////////////
                                deletedCols.Push(x);
                                //backCols++;
                                System.Console.WriteLine("удален столбец: " + x.x + ":" + x.y);
                            }
                        }
                        System.Console.WriteLine("выполнен шаг 5");
                        System.Console.ReadLine();
                        list.RemoveRow(chosenRow);//6/////////////////
                        deletedRows.Push(chosenRow);
                        //backRows++;
                        System.Console.WriteLine("удалена выбранная строка");
                        System.Console.WriteLine("подготовка к рекурсии");
                        System.Console.ReadLine();
                        XAlgorithm(list, temp, deletedRows, backRows, deletedCols, backCols, ++depth);//7 рекурсия
                        depth--;
                    }
                    else
                    {
                        currentNode = currentNode.parent;
                        depth--;
                        while (backRows[depth] != deletedRows.Count)
                        {
                            System.Console.WriteLine("восстанавливается строка: " + deletedRows.Peek().name + ":" + deletedRows.Peek().id);
                            list.RestoreRow(deletedRows.Pop());
                            //backRows--;
                            //System.Console.WriteLine("восстанавлен!");
                        }
                        while (backCols[depth] != deletedCols.Count)
                        {
                            System.Console.WriteLine("восстанавливается столбец: " + deletedCols.Peek().x + ":" + deletedCols.Peek().y);
                            list.RestoreCol(deletedCols.Pop());
                            //backCols--;
                            //System.Console.WriteLine("восстанавлен!");
                        }
                        System.Console.WriteLine("на шаг назад");
                        //System.Console.WriteLine(list.PrintTableHeader());
                        //System.Console.WriteLine(list.PrintTableLefter());
                        System.Console.WriteLine(currentNode.name + ":" + currentNode.id);

                        Row iter = list.rFirst;
                        while (iter != null)//4
                        {
                            if (iter.name == currentNode.name)
                            {
                                list.RemoveRow(iter);/////////////////
                                deletedRows.Push(iter);
                                //backRows++;
                                System.Console.WriteLine("удалена строка: " + iter.name + ":" + iter.id);
                            }
                            iter = iter._down;
                        }
                        System.Console.WriteLine("удалены все одноименные строки");
                        System.Console.ReadLine();                        
                    }
                }
                else
                {
                    currentNode = currentNode.parent;
                    depth--;
                    while (backRows[depth] != deletedRows.Count)
                    {
                        System.Console.WriteLine("восстанавливается строка: " + deletedRows.Peek().name + ":" + deletedRows.Peek().id);
                        list.RestoreRow(deletedRows.Pop());
                        //backRows--;
                        //System.Console.WriteLine("восстанавлен!");
                    }
                    while (backCols[depth] != deletedCols.Count)
                    {
                        System.Console.WriteLine("восстанавливается столбец: " + deletedCols.Peek().x + ":" + deletedCols.Peek().y);
                        list.RestoreCol(deletedCols.Pop());
                        //backCols--;
                        //System.Console.WriteLine("восстанавлен!");
                    }
                    System.Console.WriteLine("на шаг назад");
                    //System.Console.WriteLine(list.PrintTableHeader());
                    //System.Console.WriteLine(list.PrintTableLefter());
                    System.Console.WriteLine(currentNode.name + ":" + currentNode.id);

                    Row iter = list.rFirst;
                    while (iter != null)//4
                    {
                        if (iter.name == currentNode.name)
                        {
                            list.RemoveRow(iter);/////////////////
                            deletedRows.Push(iter);
                            //backRows++;
                            System.Console.WriteLine("удалена строка: " + iter.name + ":" + iter.id);
                        }
                        iter = iter._down;
                    }
                    System.Console.WriteLine("удалены все одноименные строки");
                    System.Console.ReadLine();
                }
            }
                System.Console.WriteLine("нужно записать решение");
                System.Console.ReadLine();
            
            //currentNode = currentNode.parent;//перейти к корню, собрать все имена и id и добавить их последовательность в решение

        }
    }
}
