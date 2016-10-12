using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoConsole
{
    /// <summary>
    /// Controller for DancingLinks list
    /// </summary>
    class DancingLinks
    {
        public Row rFirst, rLast, rCurrent;
        public Col cFirst, cLast, cCurrent;
        public int cCount = 0, rCount = 0;
        Node nCurrent;
        DancingLinks()
        {
            rFirst = rLast = rCurrent = null;
            cFirst = cLast = cCurrent = null;
            nCurrent = null;
        }
        public DancingLinks(int xCount, int yCount)
        {
            for (int i = 0; i < xCount; i++)
                for (int j = 0; j < yCount; j++)
                {
                    if (i == 0 && j == 0)
                        cFirst = cLast = new Col(i, j);
                    else
                    {
                        if (i == 0 && j == 1)
                        {
                            cLast = new Col(i, j);
                            cFirst._right = cLast;
                            cLast._left = cFirst;
                        }
                        else
                            cLast = new Col(cLast, i, j);
                    }
                    cCount++;
                }
        }
        public Row AddRow(string name)//WithNodes(int i, int j)//сделать заполнение имени через конструктор
        {
            if (rFirst == null)
            {
                rFirst = rLast = rCurrent = new Row();
                rFirst.name = name;
            }
            else
            {
                if (rFirst == rLast)
                {
                    rLast = rCurrent = new Row(rFirst);
                }
                else
                {
                    rLast = rCurrent = new Row(rLast);
                }
            }
            rLast.name = rCurrent.name = name;
            rCount++;
            Node i = rCurrent._head;
            while (i != null)
            {
                i._col.length++;
                i = i._right;
            }
            return rCurrent;
        }
        /// <summary>
        /// Добавить узел
        /// </summary>
        /// <param name="row"> Строка(положение пентамино) </param>
        /// <param name="i"> X индекс клетки, которую закрывает пентамино </param>
        /// <param name="j"> Y индекс клетки, которую закрывает пентамино </param>
        public void AddNode(Row row, int i, int j)
        {
            Col col = SearchCol(i, j);
            nCurrent = new Node(row, col);
            if (row._head == null && col._head == null)
            {
                row._head = nCurrent;
                col._head = nCurrent;
            }
            else
            {
                if (row._head == null)
                {
                    row._head = nCurrent;
                    nCurrent._up = FindLastInCol(col);
                    nCurrent._up._down = nCurrent;
                }
                else
                {
                    if (col._head == null)
                    {
                        col._head = nCurrent;
                        nCurrent._left = FindLastInRow(row);
                        nCurrent._left._right = nCurrent;
                    }
                    else
                    {
                        nCurrent._up = FindLastInCol(col);
                        nCurrent._up._down = nCurrent;
                        nCurrent._left = FindLastInRow(row);
                        nCurrent._left._right = nCurrent;
                    }
                }
            }
            col.length++;
            row.length++;
            nCurrent._row = row;
            nCurrent._col = col;
        }

        public void RemoveRow(Row row)
        {
            if (row._down == null && row._up == null)
            {
                rFirst = rLast = null;
            }
            else
            {
                if (row._up == null || row == rFirst)
                {
                    rFirst = row._down;
                    rFirst._up = null;
                }
                else
                {
                    if (row._down == null || row == rLast)
                    {
                        rLast = row._up;
                        rLast._down = null;
                    }
                    else
                    {
                        row._down._up = row._up;
                        row._up._down = row._down;
                    }
                }
            }
            rCount--;
            Node i = row._head;
            while (i != null)
            {
                i._col.length--;
                i = i._right;
            }
            row.deleted = true;
        }
        public void RestoreRow(Row row)
        {
            if (row._up == null && row._down == null)
            {
                rFirst = rLast = row;
            }
            else
            {
                if (row._down == null)
                {
                    rLast = row;
                    row._up._down = row;
                }
                else
                {
                    if (row._up == null)
                    {
                        rFirst = row;
                        row._down._up = row;
                    }
                    else
                    {
                        if (row._down == rFirst)
                        {
                            rFirst = row;
                        }
                        else
                        {
                            if (row._up == rLast)
                                rLast = row;
                        }
                        row._down._up = row;
                        row._up._down = row;
                    }
                }
            }
            //row.used = false;
            Node i = row._head;
            while (i != null)
            {
                i._col.length++;
                i = i._right;
            }
            row.deleted = false;
        }
        public void RemoveCol(Col col)
        {
            if (col._left == null && col._right == null)
            {
                cFirst = cLast = null;
            }
            else
            {
                if (col._left == null || col == cFirst)
                {
                    cFirst = col._right;
                    cFirst._left = null;
                }
                else
                {
                    if (col._right == null || col == cLast)
                    {
                        cLast = col._left;
                        cLast._right = null;
                    }
                    else
                    {
                        col._right._left = col._left;
                        col._left._right = col._right;
                    }
                }
            }
            cCount--;
            col.deleted = true;
        }
        public void RestoreCol(Col col)
        {
            if (col._left == null && col._right == null)
            {
                cFirst = cLast = col;
            }
            else
            {
                if (col._right == null)
                {
                    cLast = col;
                    col._left._right = col;
                }
                else
                {
                    if (col._left == null)
                    {
                        cFirst = col;
                        col._right._left = col;
                    }
                    else
                    {
                        if (col._right == cFirst)
                        {
                            cFirst = col;
                        }
                        else
                        {
                            if (col._left == cLast)
                                cLast = col;
                        }
                        col._right._left = col;
                        col._left._right = col;
                    }
                }
            }
            cCount++;
            col.deleted = false;
        }

        /// <summary>
        /// Найти столбец с наименьшим количеством элементов, т.е. такую ячейку, которую может занимать наименьшее количество пентамино
        /// </summary>
        /// <returns></returns>
        public Col FindMinCol()
        {
            cCurrent = cFirst;
            int min = Int32.MaxValue;
            Col temp = new Col();
            while (cCurrent != null)
            {
                if (cCurrent.length < min && cCurrent.used == false && cCurrent.deleted == false) //&& cCurrent.length != 0)
                {
                    min = cCurrent.length;
                    temp = cCurrent;
                }
                cCurrent = cCurrent._right;
            }
            //temp.used = true;
            return temp;
        }
        public Col SelectCol()
        {
            cCurrent = cFirst;
            int min = cCurrent.length;
            Col temp = cCurrent._right, mc = cCurrent;
            while (temp != null)
            {
                if (temp.length < min)
                {
                    min = temp.length;
                    mc = temp;
                }
                temp = temp._right;
            }
            return mc;
        }
        public List<Col> FindAllColInRow(Row row)
        {
            List<Col> colList = new List<Col>();
            rCurrent = row;
            nCurrent = row._head;
            while (nCurrent != null)
            {
                colList.Add(nCurrent._col);
                nCurrent = nCurrent._right;
            }
            return colList;
        }
        public List<Row> FindAllRowInCol(Col col)
        {
            List<Row> rowList = new List<Row>();
            cCurrent = col;
            nCurrent = col._head;
            while (nCurrent != null)
            {
                rowList.Add(nCurrent._row);
                nCurrent = nCurrent._down;
            }
            return rowList;
        }
        public void RemoveColWithZero()
        {
            cCurrent = cFirst;
            while (cCurrent != null)
            {
                if (cCurrent.length == 0)
                    RemoveCol(cCurrent);
                cCurrent = cCurrent._right;
            }
        }
        public Row FindNotUsedRowInCol(Col col)//здесь проблема
        {
            if (col.x == 7 && col.y == 12)
            {
            }
            Row result = new Row();
            result.id = -1;
            if (col.length == 0)
                result = null;
            else
            {
                //if (col._head._row.used == true || col._head._row.deleted == true)
                //{
                nCurrent = col._head;
                while (nCurrent != null)
                {
                    if (nCurrent._row.used == false && nCurrent._row.deleted == false && nCurrent._row._head._col.deleted == false)
                    {
                        result = nCurrent._row;
                        break;
                    }
                    nCurrent = nCurrent._down;
                }
                //}
                //else
                //{
                //    //col._head._row.used = true;
                //    result = col._head._row;
                //}
            }
            if (result.id == -1)
                result = null;
            if (result != null)
                result.used = true;
            return result;
        }
        private Node FindLastInCol(Col col)
        {
            Node needed;
            if (col._head == null)
                needed = null;
            else
            {
                needed = col._head;
                while (needed._down != null)
                    needed = needed._down;
            }
            return needed;
        }
        private Node FindLastInRow(Row row)
        {
            Node needed;
            if (row._head == null)
                needed = null;
            else
            {
                needed = row._head;
                while (needed._right != null)
                    needed = needed._right;
            }
            return needed;
        }
        /// <summary>
        /// Найти столбец по заданным индексам
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private Col SearchCol(int i, int j)
        {
            cCurrent = cFirst;
            while (cCurrent != null)
            {
                if (cCurrent.x == i && cCurrent.y == j)
                    return cCurrent;
                else
                    cCurrent = cCurrent._right;
            }
            return cCurrent;
        }
        public string PrintTableHeader()
        {
            cCurrent = cFirst;
            string result = "";
            while (cCurrent != null)
            {
                result += "|" + cCurrent.x + ":" + cCurrent.y + "|=";
                cCurrent = cCurrent._right;
            }
            return result;
        }
        public string PrintTableLefter()
        {
            rCurrent = rFirst;
            string result = "";
            while (rCurrent != null)
            {
                result += "|" + rCurrent.name + ":" + rCurrent.id + "|=";
                rCurrent = rCurrent._down;
            }
            return result;
        }

    }

    class Node
    {
        public Node _up, _down, _left, _right;
        public Row _row;
        public Col _col;
        //bool state;//состояние 1/0 означающее покрытие ячейки пентаминошкой
        public Node(Row row, Col col)
        {
            _row = row;
            _col = col;
            _up = _down = _left = _right = null;
        }
    }

    class Row
    {
        public Row _up, _down;
        public Node _head;
        public bool used;
        public bool deleted;
        public int id;
        public string name;//название пентамино
        public int length;
        public Row()
        {
            _up = _down = null;
            _head = null;
            id = 0;
            name = "first";
            length = 0;
            used = false;
            deleted = false;
        }
        public Row(Row prev)
        {
            _up = prev;
            prev._down = this;
            _down = null;
            id = prev.id + 1;
            name = id.ToString();
            length = 0;
            used = false;
            deleted = false;
        }
    }

    class Col
    {
        public Col _left, _right;
        public Node _head;
        public bool used;
        public int x, y; //индексы элемента изображения (маски)
        public int length;
        public bool deleted;
        public Col()
        {
            _left = _right = null;
            _head = null;
            length = 0;
            x = y = 0;
            deleted = false;
            used = false;
        }
        public Col(int _x, int _y)
        {
            _left = _right = null;
            _head = null;
            length = 0;
            x = _x;
            y = _y;
            deleted = false;
            used = false;
        }
        public Col(Col prev, int _x, int _y)
        {
            _left = prev;
            prev._right = this;
            _right = null;
            _head = null;
            length = 0;
            x = _x;
            y = _y;
            deleted = false;
            used = false;
        }
    }
}
