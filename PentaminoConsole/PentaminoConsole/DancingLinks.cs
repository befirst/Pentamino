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
        Row rFirst, rLast, rCurrent;
        Col cFirst, cLast, cCurrent;
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
                }
        }
        public Row AddRow()//WithNodes(int i, int j)
        {
            if (rFirst == null)
                rFirst = rLast = rCurrent = new Row();
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
                nCurrent._row = row;
                nCurrent._col = col;
            }
            else
            {
                if (row._head == null)
                {
                    row._head = nCurrent;
                    nCurrent._up = FindLastInCol(col);
                    nCurrent._up._down = nCurrent;
                    nCurrent._row = row;
                    nCurrent._col = col;
                }
                else
                {
                    if (col._head == null)
                    {
                        col._head = nCurrent;
                        nCurrent._left = FindLastInRow(row);
                        nCurrent._left._right = nCurrent;
                        nCurrent._row = row;
                        nCurrent._col = col;
                    }
                    else
                    {
                        nCurrent._up = FindLastInCol(col);
                        nCurrent._up._down = nCurrent;
                        nCurrent._left = FindLastInRow(row);
                        nCurrent._left._right = nCurrent;
                        nCurrent._row = row;
                        nCurrent._col = col;
                    }
                }
            }
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
        Row _up, _down;
        public Node _head;
        int id;
        string name;//название пентамино
        int length;
        public Row()
        {
            _up = _down = null;
            _head = null;
            id = 0;
            name = "first";
            length = 0;
        }
        public Row(Row prev)
        {
            _up = prev;
            prev._down = this;
            _down = null;
            id = prev.id++;
            name = id.ToString();
            length = 0;
        }
    }

    class Col
    {
        public Col _left, _right;
        public Node _head;
        public int x, y; //индексы элемента изображения (маски)
        int length;
        public Col()
        {
            _left = _right = null;
            _head = null;
            length = 0;
            x = y = 0;
        }
        public Col(int _x, int _y)
        {
            _left = _right = null;
            _head = null;
            length = 0;
            x = _x;
            y = _y;
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
        }
    }
}
