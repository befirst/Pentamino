using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoConsole
{
    class Pentaminos
    {
        public List<PentaminoType> pentaminoList = new List<PentaminoType>();
        public Pentaminos()
        {
            pentaminoList.Add(new VPentamino());
            pentaminoList.Add(new FPentamino());
            pentaminoList.Add(new IPentamino());
            pentaminoList.Add(new LPentamino());
            pentaminoList.Add(new NPentamino());
            pentaminoList.Add(new PPentamino());
            pentaminoList.Add(new TPentamino());
            pentaminoList.Add(new UPentamino());
            pentaminoList.Add(new WPentamino());
            pentaminoList.Add(new XPentamino());
            pentaminoList.Add(new YPentamino());
            pentaminoList.Add(new ZPentamino());
            foreach (var x in pentaminoList)
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

    class PentaminoType
    {
        public string name;
        public List<int[,]> data = new List<int[,]>();
        //public HashSet<int[,]> data = new HashSet<int[,]>();
        private int[,] Rotate90(int[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            int[,] rotated = new int[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0, t = n - 1; j < n; j++, --t)
                    rotated[i, j] = origin[t, i];
            return rotated;
        }
        private int[,] Rotate180(int[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            int[,] rotated = new int[n, m];
            for (int i = 0, t = n - 1; i < n; i++, t--)
                for (int j = 0, k = m - 1; j < m; j++, --k)
                    rotated[i, j] = origin[t, k];
            return rotated;
        }
        private int[,] Rotate270(int[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            int[,] rotated = new int[m, n];
            for (int i = 0, t = m - 1; i < m || t >= 0; i++, t--)
                for (int j = 0; j < n; j++)
                    rotated[i, j] = origin[j, t];
            return rotated;
        }
        private int[,] Mirror(int[,] origin)
        {
            int m = origin.GetLength(1);
            int n = origin.GetLength(0);
            int[,] mirrored = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0, t = m - 1; j < m; j++, t--)
                    mirrored[i, j] = origin[i, t];
            return mirrored;
        }
        protected void FillList(int[,] origin)
        {
            Check(Rotate90(origin));
            Check(Rotate180(origin));
            Check(Rotate270(origin));
            int[,] mirrored = Mirror(origin);
            Check(mirrored);
            Check(Rotate90(mirrored));
            Check(Rotate180(mirrored));
            Check(Rotate270(mirrored));
            //foreach (var i in data)
            //    print(i);
        }
        private void Check(int[,] origin)
        {            
            int count = 0;
            foreach (var x in data)
            {
                if (x.GetLength(0) == origin.GetLength(0) && x.GetLength(1) == origin.GetLength(1))
                {
                    if(CheckArray(x, origin))
                        count++;
                }
            }
            if (count == 0)
                data.Add(origin);
        }
        private bool CheckArray(int[,] x, int[,] origin)
        {
            for (int i = 0; i < x.GetLength(0); i++)            
                for (int j = 0; j < x.GetLength(1); j++)
                    if (x[i, j] != origin[i, j])                    
                        return false;
            return true;
        }
        private void print(int[,] origin)
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

    class VPentamino : PentaminoType
    {
        int[,] v = new int[,]{{ 1,1,1 },
                              { 1,0,0 },
                              { 1,0,0 }};
        public VPentamino()
        {
            name = "v";
            data.Add(v);
            FillList(v);
        }
    }
    class FPentamino : PentaminoType
    {
        int[,] f = new int[,]{{ 0,1,1 },
                              { 1,1,0 },
                              { 0,1,0 }};
        public FPentamino()
        {
            name = "f";
            data.Add(f);
            FillList(f);
        }
    }
    class IPentamino : PentaminoType
    {
        int[,] i = new int[,]{{ 1 },
                              { 1 },
                              { 1 },
                              { 1},
                              { 1} };
        public IPentamino()
        {
            name = "i";
            data.Add(i);
            FillList(i);
        }
    }
    class LPentamino : PentaminoType
    {
        int[,] l = new int[,]{{ 1,0 },
                              { 1,0 },
                              { 1,0 },
                              { 1,1 } };
        public LPentamino()
        {
            name = "l";
            data.Add(l);
            FillList(l);
        }
    }
    class NPentamino : PentaminoType
    {
        int[,] n = new int[,]{{ 1,0 },
                              { 1,0 },
                              { 1,1 },
                              { 0,1 } };
        public NPentamino()
        {
            name = "n";
            data.Add(n);
            FillList(n);
        }
    }
    class PPentamino : PentaminoType
    {
        int[,] p = new int[,]{{ 1,1 },
                              { 1,1 },
                              { 1,0 }};
        public PPentamino()
        {
            name = "p";
            data.Add(p);
            FillList(p);
        }
    }
    class TPentamino : PentaminoType
    {
        int[,] t = new int[,]{{ 1,1,1 },
                              { 0,1,0 },
                              { 0,1,0 }};
        public TPentamino()
        {
            name = "t";
            data.Add(t);
            FillList(t);
        }
    }
    class UPentamino : PentaminoType
    {
        int[,] u = new int[,]{{ 1,1 },
                              { 1,0 },
                              { 1,1 } };
        public UPentamino()
        {
            name = "u";
            data.Add(u);
            FillList(u);
        }
    }
    class WPentamino : PentaminoType
    {
        int[,] w = new int[,]{{ 0,1,1 },
                              { 1,1,0 },
                              { 1,0,0 }};
        public WPentamino()
        {
            name = "w";
            data.Add(w);
            FillList(w);
        }
    }
    class XPentamino : PentaminoType
    {
        int[,] x = new int[,]{{ 0,1,0 },
                              { 1,1,1 },
                              { 0,1,0 }};
        public XPentamino()
        {
            name = "x";
            data.Add(x);
            FillList(x);
        }
    }
    class YPentamino : PentaminoType
    {
        int[,] y = new int[,]{{ 1,0 },
                              { 1,1 },
                              { 1,0 },
                              { 1,0 } };
        public YPentamino()
        {
            name = "y";
            data.Add(y);
            FillList(y);
        }
    }
    class ZPentamino : PentaminoType
    {
        int[,] z = new int[,]{{ 1,1,0 },
                              { 0,1,0 },
                              { 0,1,1 }};
        public ZPentamino()
        {
            name = "z";
            data.Add(z);
            FillList(z);
        }
    }
}
