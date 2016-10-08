using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoConsole
{
    class Pentaminos
    {
        List<PentaminoType> pentaminoList = new List<PentaminoType>();
        Pentaminos()
        {
            pentaminoList.Add(new VPentamino());
        }
    }

    class PentaminoType
    {
        public string name;
        public List<int[,]> data;
        private void Rotate90() { }
        private void Rotate180() { }
        private void Rotate270() { }
        private void Mirror() { }
        protected void FillList() { }
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
        }
    }
}
