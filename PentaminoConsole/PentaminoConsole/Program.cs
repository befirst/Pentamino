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
            //System.Console.ReadLine();
            Pentaminos p = new Pentaminos();
            foreach (var x in p.pentaminoList)//добавление данных о положении пентамино
            {
                System.Console.WriteLine(x.name);
                foreach (var y in x.data)
                    System.Console.WriteLine(CheckPentamino(y, x.name, binarySource, dl));
                //System.Console.ReadLine();
            }
            dl.RemoveColWithZero();//удалить все незаполняемые клетки
            System.Console.WriteLine(dl.PrintTableHeader());//проверка удаления нужных клеток
            System.Console.WriteLine(dl.PrintTableLefter());
            System.Console.ReadLine();
            Tree tree = new Tree();//создание дерева
            //System.Console.WriteLine(dl.FindMinCol().x + ":" + dl.FindMinCol().y + "length= " + dl.FindMinCol().length);//проверка функции нахождения столбца с минимальным количеством возможных положений
            List<TreeNode> solutions = FindSolutions(tree, dl, p, source);
            System.Console.ReadLine();
        }

        static void NewX(DancingLinks list, Tree tree, Stack<Row> deletedRows, int[] backRows, Stack<Col> deletedCols, int[] backCols, int depth)
        {
            if (list.cCount == 0)
            {
                //записать решение
                Console.WriteLine("Решилось!");
                Console.ReadLine();
                return;
            }
            if (list.rCount == 0)
            {
                Console.WriteLine("Нет!");
                Console.ReadLine();
                return;
            }
            Col chosen = list.SelectCol();
            if (chosen.length == 0)
            {
                Console.WriteLine("Нет!");
                Console.ReadLine();
                return;
            }
            Node nodeFromChosen = chosen._head;
            do
            {
                backRows[depth] = deletedRows.Count;
                backCols[depth] = deletedCols.Count;
                Node horizontal = nodeFromChosen;
                do
                {
                    Node vertical = chosen._head._down;
                    while (vertical._down != null)
                    {
                        list.RemoveRow(horizontal._row);/////////////////
                        deletedRows.Push(horizontal._row);
                        vertical = vertical._down;
                    }
                    deletedCols.Push(vertical._col);/////////////////
                    horizontal = horizontal._right;
                } while (horizontal != null);
                list.RemoveRow(nodeFromChosen._row);
                deletedRows.Push(nodeFromChosen._row);/////////////////
                Col[] a = deletedCols.ToArray();
                for (int i = backCols[depth]; i < deletedCols.Count; ++i)
                    list.RemoveCol(a[i]);

                depth++;

                NewX(list, tree, deletedRows, backRows, deletedCols, backCols, depth);

                depth--;
                while (backRows[depth] < deletedRows.Count)
                {
                    Row temp = deletedRows.Pop();
                    temp.used = false;//лишнее
                    list.RestoreRow(temp);
                }
                while (backCols[depth] < deletedCols.Count)
                {
                    list.RestoreCol(deletedCols.Pop());
                }
                if (tree.current != tree.root)
                {

                }
                nodeFromChosen = nodeFromChosen._down;
            } while (nodeFromChosen != null || depth != -1);
            if (depth == -1)
                Console.WriteLine("Найдено решений: ");
        }

        static List<TreeNode> FindSolutions(Tree tree, DancingLinks list, Pentaminos pentaminos, char[,] source)
        {
            List<TreeNode> solutions = new List<TreeNode>();//все возможные решения
            Stack<Row> deletedRows = new Stack<Row>();//стек удаленных строк, из него восстанавливаем список
            Stack<Col> deletedCols = new Stack<Col>();//стек удаленных столбцов, из него восстанавливаем список
            int depth = 1;//глубина в которой мы находимся
            int[] backRows = new int[1000];
            int[] backCols = new int[1000];
            list.cCurrent = list.cFirst;
            TreeNode tnCurrent = tree.root;
            //while (list.cCurrent != null)
            //{

            //    list.cCurrent = list.cCurrent._right;
            //}

            System.Console.WriteLine("Begin");

            //NewX(list, tree, deletedRows, backRows, deletedCols, backCols, depth);
            XAlgorithm2(list, solutions, source, tree, deletedRows, backRows, deletedCols, backCols, depth);

            System.Console.WriteLine("End");


            return solutions;
        }
        static TreeNode XAlgorithm2(DancingLinks list, List<TreeNode> solutions, char[,] source, Tree tree, Stack<Row> deletedRows, int[] backRows, Stack<Col> deletedCols, int[] backCols, int depth)
        {
            if (list.cCount == 0)
            {
                //записать решение
                Console.WriteLine("Решилось!");
                Console.ReadLine();
                solutions.Add(tree.current);
                return tree.current.parent;
            }
            if (list.rCount == 0)
            {
                Console.WriteLine("Не осталось строк! ");
                //Console.ReadLine();
                return tree.current;
            }
            Col chosen = list.FindMinCol();
            
            if (chosen.length == 0 || chosen == null)
            {
                Console.WriteLine("Нет положений пентамино для ячейки: " + chosen.x + ":" + chosen.y);
                //Console.ReadLine();
                return tree.current.parent;//.parent;
            }
            Node nodeFromChosen = chosen._head;
            Row chosenRow = list.FindNotUsedRowInCol(chosen);
            if (chosenRow == null)
            {
                Console.WriteLine("Нет положений пентамино для ячейки: " + chosen.x + ":" + chosen.y);
                //Console.ReadLine();
                return tree.current.parent;//.parent;
            }
            //tree.current.children.Add();
            tree.current = new TreeNode(tree.current, chosenRow, chosenRow.name, chosenRow.id);
            
            System.Console.WriteLine(tree.Print(tree.current)); //("мы находимся в узле: " + tree.current.id + ":" + tree.current.name);
            //System.Console.ReadLine();
            do
            {
                //System.Console.WriteLine("первый шаг: " + chosen.x + ":" + chosen.y);
                //System.Console.ReadLine();

                //System.Console.WriteLine("второй шаг: " + chosenRow.name + ":" + chosenRow.id);
                //System.Console.ReadLine();
                backRows[depth] = deletedRows.Count;
                backCols[depth] = deletedCols.Count;
                Node horizontal = nodeFromChosen;

                foreach (var x in list.FindAllColInRow(chosenRow))//5
                {
                    foreach (var y in list.FindAllRowInCol(x))
                        if (y != chosenRow && y.deleted == false)
                        {
                            list.RemoveRow(y);/////////////////
                            deletedRows.Push(y);
                            //backRows++;
                            //System.Console.WriteLine("удалена строка: " + y.name + ":" + y.id);
                        }
                    if (x.deleted == false)
                    {
                        list.RemoveCol(x);/////////////////
                        deletedCols.Push(x);
                        //backCols++;
                        //System.Console.WriteLine("удален столбец: " + x.x + ":" + x.y);
                    }
                }

                //do
                //{
                //    Node vertical = chosen._head._down;
                //    while (vertical._down != null)
                //    {
                //        list.RemoveRow(horizontal._row);/////////////////
                //        deletedRows.Push(horizontal._row);
                //        vertical = vertical._down;
                //    }
                //    deletedCols.Push(vertical._col);/////////////////
                //    horizontal = horizontal._right;
                //} while (horizontal != null);
                list.RemoveRow(chosenRow);
                deletedRows.Push(chosenRow);/////////////////
                //System.Console.WriteLine("удалена строка: " + chosenRow.name + ":" + chosenRow.id);
                //Col[] a = deletedCols.ToArray();
                //for (int i = backCols[depth]; i < deletedCols.Count; ++i)
                //    list.RemoveCol(a[i]);

                Row iter = list.rFirst;
                while (iter != null)//4
                {
                    if (iter.deleted == false && iter != chosenRow && iter.name == chosenRow.name)
                    {
                        list.RemoveRow(iter);/////////////////
                        deletedRows.Push(iter);
                        //backRows++;
                        //System.Console.WriteLine("удалена строка: " + iter.name + ":" + iter.id);
                    }
                    iter = iter._down;
                }
                //System.Console.WriteLine("выполнен шаг 4, удалены все одноименные строки");
                //System.Console.ReadLine();
                Console.Clear();
                printArray(source, chosenRow, true);

                depth++;

                tree.current = XAlgorithm2(list, solutions, source, tree, deletedRows, backRows, deletedCols, backCols, depth);

                depth--;
                //System.Console.WriteLine("возврат:" + depth);
                
                if (tree.current == null)
                {
                }
                Console.WriteLine(tree.Print(tree.current));
                //tree.Print(tree.current = tree.current.parent);
                //System.Console.ReadLine();

                while (backRows[depth] < deletedRows.Count)
                {
                    Row temp = deletedRows.Pop();
                    if (temp != chosenRow)
                        temp.used = false;//лишнее
                    //System.Console.WriteLine("восстанавливается строка: " + temp.name + ":" + temp.id + ":" + temp.used.ToString());
                    list.RestoreRow(temp);
                }
                while (backCols[depth] < deletedCols.Count)
                {
                    Col temp = deletedCols.Pop();
                    //System.Console.WriteLine("восстановлен столбец: " + temp.x + ":" + temp.y);
                    list.RestoreCol(temp);
                }
                //foreach (var x in list.FindAllColInRow(chosenRow))//5
                //{
                //    foreach (var y in list.FindAllRowInCol(x))
                //        if (y != chosenRow && y.deleted == false)
                //        {
                //            list.RemoveRow(y);/////////////////
                //            //deletedRows.Push(y);
                //            //backRows++;
                //            System.Console.WriteLine("удалена строка: " + y.name + ":" + y.id);
                //        }
                //    //if (x.deleted == false)
                //    //{
                //    //    list.RemoveCol(x);/////////////////
                //    //    deletedCols.Push(x);
                //    //    //backCols++;
                //    //    System.Console.WriteLine("удален столбец: " + x.x + ":" + x.y);
                //    //}
                //}
                
                //System.Console.WriteLine("мы находимся в узле: " + tree.current.name + ":" + tree.current.id);
                //System.Console.ReadLine();
                if (tree.current != tree.root.parent)
                {
                    if (tree.current.children.Count > 0)
                        foreach (var i in tree.current.children)
                        {
                            if (i.id != tree.current.id)
                                i.data.used = true;
                            else
                                i.data.used = false;
                        }
                    
                }
                Console.Clear();
                printArray(source, chosenRow, false);
                chosenRow = list.FindNotUsedRowInCol(chosen);
                if (chosenRow != null)
                {
                    tree.current = new TreeNode(tree.current, chosenRow, chosenRow.name, chosenRow.id);
                    System.Console.WriteLine(tree.Print(tree.current));
                    //System.Console.ReadLine();
                }
                else
                    break;
                //nodeFromChosen = nodeFromChosen._down;

            } while (chosenRow != null || nodeFromChosen != null || depth != -1);
            //tree.current = tree.current.parent;
            if (depth == -1)
                Console.WriteLine("Найдено решений: " + solutions.Count);
            return tree.current.parent;

        }
        static void XAlgorithm(DancingLinks list, TreeNode currentNode, Tree tree, Stack<Row> deletedRows, int[] backRows, Stack<Col> deletedCols, int[] backCols, int depth)
        {
            backRows[depth] = deletedRows.Count;
            backCols[depth] = deletedCols.Count;
            if (list.cCount != 0)
            {
                if (list.rCount != 0)
                {
                    System.Console.WriteLine(depth + " :не пустой");
                    //System.Console.ReadLine();
                    Col chosenCol = list.FindMinCol();//1
                    System.Console.WriteLine("первый шаг: " + chosenCol.x + ":" + chosenCol.y);
                    //System.Console.ReadLine();
                    if (chosenCol.length != 0)
                    {
                        Row chosenRow = list.FindNotUsedRowInCol(chosenCol);//2                    
                        if (chosenRow != null)
                        {
                            chosenRow.used = true;
                            System.Console.WriteLine("второй шаг: " + chosenRow.name + ":" + chosenRow.id);
                            //System.Console.ReadLine();
                            if (currentNode != tree.root)
                                tree.Reload(currentNode);
                            TreeNode temp = new TreeNode(currentNode, chosenRow, chosenRow.name, chosenRow.id);
                            //currentNode.children.Add(temp);//3
                            System.Console.WriteLine("ребенок добавлен");
                            //System.Console.ReadLine();
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
                            //System.Console.ReadLine();
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
                            //System.Console.ReadLine();
                            list.RemoveRow(chosenRow);//6/////////////////
                            deletedRows.Push(chosenRow);
                            //backRows++;
                            System.Console.WriteLine("удалена выбранная строка");
                            System.Console.WriteLine("подготовка к рекурсии");
                            //System.Console.ReadLine();
                            XAlgorithm(list, temp, tree, deletedRows, backRows, deletedCols, backCols, ++depth);//7 рекурсия
                                                                                                                ////depth--;
                            System.Console.WriteLine("Алгоритм завершен на глубине: " + depth);
                            System.Console.ReadLine();
                            currentNode = currentNode.parent;
                            depth--;
                            while (backRows[depth] < deletedRows.Count)
                            {
                                System.Console.WriteLine("восстанавливается строка: " + deletedRows.Peek().name + ":" + deletedRows.Peek().id);
                                list.RestoreRow(deletedRows.Pop());
                                //backRows--;
                                //System.Console.WriteLine("восстанавлен!");
                            }
                            while (backCols[depth] < deletedCols.Count)
                            {
                                System.Console.WriteLine("восстанавливается столбец: " + deletedCols.Peek().x + ":" + deletedCols.Peek().y);
                                list.RestoreCol(deletedCols.Pop());
                                //backCols--;
                                //System.Console.WriteLine("восстанавлен!");
                            }
                            System.Console.WriteLine("на шаг назад");

                            //System.Console.WriteLine(list.PrintTableHeader());
                            //System.Console.WriteLine(list.PrintTableLefter());
                            //System.Console.WriteLine(currentNode.name + ":" + currentNode.id); 

                            Row iter2 = list.rFirst;
                            while (iter != null)//4
                            {
                                if (iter2.name == currentNode.name)
                                {
                                    list.RemoveRow(iter2);/////////////////
                                    deletedRows.Push(iter2);
                                    //backRows++;
                                    System.Console.WriteLine("удалена строка: " + iter2.name + ":" + iter2.id);
                                }
                                iter2 = iter2._down;
                            }
                            System.Console.WriteLine("удалены все одноименные строки");
                        }
                        else
                        {
                            System.Console.WriteLine("ChosenRow=0");
                            //System.Console.ReadLine();

                            depth--;
                            while (backRows[depth] < deletedRows.Count)
                            {
                                System.Console.WriteLine("восстанавливается строка: " + deletedRows.Peek().name + ":" + deletedRows.Peek().id);
                                list.RestoreRow(deletedRows.Pop());
                                //backRows--;
                                //System.Console.WriteLine("восстанавлен!");
                            }
                            while (backCols[depth] < deletedCols.Count)
                            {
                                System.Console.WriteLine("восстанавливается столбец: " + deletedCols.Peek().x + ":" + deletedCols.Peek().y);
                                list.RestoreCol(deletedCols.Pop());
                                //backCols--;
                                //System.Console.WriteLine("восстанавлен!");
                            }
                            System.Console.WriteLine("на шаг назад");
                            //System.Console.WriteLine(list.PrintTableHeader());
                            //System.Console.WriteLine(list.PrintTableLefter());
                            //System.Console.WriteLine(currentNode.name + ":" + currentNode.id); 

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
                            currentNode = currentNode.parent;
                            //System.Console.ReadLine();                        
                        }
                    }
                    else
                    {
                        return;
                        //currentNode = currentNode.parent;
                        //depth--;
                        //while (backRows[depth] < deletedRows.Count)
                        //{
                        //    System.Console.WriteLine("восстанавливается строка: " + deletedRows.Peek().name + ":" + deletedRows.Peek().id);
                        //    list.RestoreRow(deletedRows.Pop());
                        //    //backRows--;
                        //    //System.Console.WriteLine("восстанавлен!");
                        //}
                        //while (backCols[depth] < deletedCols.Count)
                        //{
                        //    System.Console.WriteLine("восстанавливается столбец: " + deletedCols.Peek().x + ":" + deletedCols.Peek().y);
                        //    list.RestoreCol(deletedCols.Pop());
                        //    //backCols--;
                        //    //System.Console.WriteLine("восстанавлен!");
                        //}
                        //System.Console.WriteLine("на шаг назад");
                        ////System.Console.WriteLine(list.PrintTableHeader());
                        ////System.Console.WriteLine(list.PrintTableLefter());
                        //System.Console.WriteLine(currentNode.name + ":" + currentNode.id);

                        //Row iter = list.rFirst;
                        //while (iter != null)//4
                        //{
                        //    if (iter.name == currentNode.name)
                        //    {
                        //        list.RemoveRow(iter);/////////////////
                        //        deletedRows.Push(iter);
                        //        //backRows++;
                        //        System.Console.WriteLine("удалена строка: " + iter.name + ":" + iter.id);
                        //    }
                        //    iter = iter._down;
                        //}
                        //System.Console.WriteLine("удалены все одноименные строки");
                        //System.Console.ReadLine();
                    }
                    return;
                }
            }
            System.Console.WriteLine("нужно записать решение");
            while (currentNode.parent != null)
            {
                Node n = currentNode.data._head;
                Console.Write(currentNode.data.name + ":" + currentNode.data.id + "| ");
                while (n != null)
                {
                    Console.Write(n._col.x + "," + n._col.y);
                    n = n._right;

                }
                Console.WriteLine();
                currentNode = currentNode.parent;
            }
            System.Console.ReadLine();

            //currentNode = currentNode.parent;//перейти к корню, собрать все имена и id и добавить их последовательность в решение

        }
        static void printArray(char[,] source, Row row, bool change)
        {
            if (change)
            {
                for (int i = 0; i < source.GetLength(0); i++)
                {
                    for (int j = 0; j < source.GetLength(1); j++)
                    {
                        Node n = row._head;
                        while (n != null)
                        {
                            if (i == n._col.x && j == n._col.y)
                                source[i, j] = n._row.name[0];
                            n = n._right;
                        }
                        System.Console.Write(source[i, j] + "");
                    }
                    System.Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < source.GetLength(0); i++)
                {
                    for (int j = 0; j < source.GetLength(1); j++)
                    {
                        Node n = row._head;
                        while (n != null)
                        {
                            if (i == n._col.x && j == n._col.y)
                                source[i, j] = 'o';
                            n = n._right;
                        }
                        System.Console.Write(source[i, j] + "");
                    }
                    System.Console.WriteLine();
                }
            }
        }
    }
}
