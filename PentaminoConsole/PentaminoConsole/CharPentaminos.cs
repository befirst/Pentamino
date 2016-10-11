using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoCharConsole
{
    class CharPentaminoChars
    {
        public List<CharPentaminoCharType> PentaminoCharList = new List<CharPentaminoCharType>();
        public CharPentaminoChars()
        {
            PentaminoCharList.Add(new VPentaminoChar());
            PentaminoCharList.Add(new FPentaminoChar());
            PentaminoCharList.Add(new IPentaminoChar());
            PentaminoCharList.Add(new LPentaminoChar());
            PentaminoCharList.Add(new NPentaminoChar());
            PentaminoCharList.Add(new PPentaminoChar());
            PentaminoCharList.Add(new TPentaminoChar());
            PentaminoCharList.Add(new UPentaminoChar());
            PentaminoCharList.Add(new WPentaminoChar());
            PentaminoCharList.Add(new XPentaminoChar());
            PentaminoCharList.Add(new YPentaminoChar());
            PentaminoCharList.Add(new ZPentaminoChar());
            foreach (var x in PentaminoCharList)
            {
                System.Console.WriteLine(x.name);
                foreach (var y in x.data)
                {
                    for (int i = 0; i < y.GetLength(0); i++)
                    {
                        for (int j = 0; j < y.GetLength(1); j++)
                        {
                            System.Console.Write(y[i, j] + "");
                        }
                        System.Console.WriteLine();
                    }
                    System.Console.WriteLine();
                }
            }
        }
    }

    class CharPentaminoCharType
    {
        public string name;
        public List<char[,]> data = new List<char[,]>();
        //public HashSet<char[,]> data = new HashSet<char[,]>();
        private char[,] Rotate90(char[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            char[,] rotated = new char[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0, t = n - 1; j < n; j++, --t)
                    rotated[i, j] = origin[t, i];
            return rotated;
        }
        private char[,] Rotate180(char[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            char[,] rotated = new char[n, m];
            for (int i = 0, t = n - 1; i < n; i++, t--)
                for (int j = 0, k = m - 1; j < m; j++, --k)
                    rotated[i, j] = origin[t, k];
            return rotated;
        }
        private char[,] Rotate270(char[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            char[,] rotated = new char[m, n];
            for (int i = 0, t = m - 1; i < m || t >= 0; i++, t--)
                for (int j = 0; j < n; j++)
                    rotated[i, j] = origin[j, t];
            return rotated;
        }
        private char[,] Mirror(char[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            char[,] mirrored = new char[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0, t = m - 1; j < m; j++, t--)
                    mirrored[i, j] = origin[i, t];
            return mirrored;
        }
        protected void FillList(char[,] origin)
        {
            Check(Rotate90(origin));
            Check(Rotate180(origin));
            Check(Rotate270(origin));
            char[,] mirrored = Mirror(origin);
            Check(mirrored);
            Check(Rotate90(mirrored));
            Check(Rotate180(mirrored));
            Check(Rotate270(mirrored));
            //foreach (var i in data)
            //    print(i);
        }
        private void Check(char[,] origin)
        {
            int count = 0;
            foreach (var x in data)
            {
                if (x.GetLength(0) == origin.GetLength(0) && x.GetLength(1) == origin.GetLength(1))
                {
                    if (CheckArray(x, origin))
                        count++;
                }
            }
            if (count == 0)
                data.Add(origin);
        }
        private bool CheckArray(char[,] x, char[,] origin)
        {
            for (int i = 0; i < x.GetLength(0); i++)
                for (int j = 0; j < x.GetLength(1); j++)
                    if (x[i, j] != origin[i, j])
                        return false;
            return true;
        }
        private void print(char[,] origin)
        {
            for (int i = 0; i < origin.GetLength(0); i++)
            {
                for (int j = 0; j < origin.GetLength(1); j++)
                {
                    System.Console.Write(origin[i, j] + "");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }
    }

    class VPentaminoChar : CharPentaminoCharType
    {
        char[,] v = new char[,]{{ 'o','o','o' },
                                { 'o',' ',' ' },
                                { 'o',' ',' ' }};
        public VPentaminoChar()
        {
            name = "v";
            data.Add(v);
            FillList(v);
        }
    }
    class FPentaminoChar : CharPentaminoCharType
    {
        char[,] f = new char[,]{{ ' ','o','o' },
                              { 'o','o',' ' },
                              { ' ','o',' ' }};
        public FPentaminoChar()
        {
            name = "f";
            data.Add(f);
            FillList(f);
        }
    }
    class IPentaminoChar : CharPentaminoCharType
    {
        char[,] i = new char[,]{{ 'o' },
                              { 'o' },
                              { 'o' },
                              { 'o'},
                              { 'o'} };
        public IPentaminoChar()
        {
            name = "i";
            data.Add(i);
            FillList(i);
        }
    }
    class LPentaminoChar : CharPentaminoCharType
    {
        char[,] l = new char[,]{{ 'o',' ' },
                              { 'o',' ' },
                              { 'o',' ' },
                              { 'o','o' } };
        public LPentaminoChar()
        {
            name = "l";
            data.Add(l);
            FillList(l);
        }
    }
    class NPentaminoChar : CharPentaminoCharType
    {
        char[,] n = new char[,]{{ 'o',' ' },
                              { 'o',' ' },
                              { 'o','o' },
                              { ' ','o' } };
        public NPentaminoChar()
        {
            name = "n";
            data.Add(n);
            FillList(n);
        }
    }
    class PPentaminoChar : CharPentaminoCharType
    {
        char[,] p = new char[,]{{ 'o','o' },
                              { 'o','o' },
                              { 'o',' ' }};
        public PPentaminoChar()
        {
            name = "p";
            data.Add(p);
            FillList(p);
        }
    }
    class TPentaminoChar : CharPentaminoCharType
    {
        char[,] t = new char[,]{{ 'o','o','o' },
                              { ' ','o',' ' },
                              { ' ','o',' ' }};
        public TPentaminoChar()
        {
            name = "t";
            data.Add(t);
            FillList(t);
        }
    }
    class UPentaminoChar : CharPentaminoCharType
    {
        char[,] u = new char[,]{{ 'o','o' },
                              { 'o',' ' },
                              { 'o','o' } };
        public UPentaminoChar()
        {
            name = "u";
            data.Add(u);
            FillList(u);
        }
    }
    class WPentaminoChar : CharPentaminoCharType
    {
        char[,] w = new char[,]{{ ' ','o','o' },
                              { 'o','o',' ' },
                              { 'o',' ',' ' }};
        public WPentaminoChar()
        {
            name = "w";
            data.Add(w);
            FillList(w);
        }
    }
    class XPentaminoChar : CharPentaminoCharType
    {
        char[,] x = new char[,]{{ ' ','o',' ' },
                              { 'o','o','o' },
                              { ' ','o',' ' }};
        public XPentaminoChar()
        {
            name = "x";
            data.Add(x);
            FillList(x);
        }
    }
    class YPentaminoChar : CharPentaminoCharType
    {
        char[,] y = new char[,]{{ 'o',' ' },
                              { 'o','o' },
                              { 'o',' ' },
                              { 'o',' ' } };
        public YPentaminoChar()
        {
            name = "y";
            data.Add(y);
            FillList(y);
        }
    }
    class ZPentaminoChar : CharPentaminoCharType
    {
        char[,] z = new char[,]{{ 'o','o',' ' },
                              {' ','o',' ' },
                              { ' ','o','o' }};
        public ZPentaminoChar()
        {
            name = "z";
            data.Add(z);
            FillList(z);
        }
    }
}
