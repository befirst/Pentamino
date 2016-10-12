using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoConsole
{
    //унаследовать бы в programm по хорошему   
    class Tree
    {
        public TreeNode root;
        public TreeNode current;
        public Tree()
        {
            root = current = new TreeNode();
        }
        public TreeNode AddNode(TreeNode parent, Row data, string name, int id)
        {
            current = new TreeNode(parent, data, name, id);
            return current;
        }
        public TreeNode Back(TreeNode child)
        {
            current = child.parent;
            return current;
        }
        public string Print(TreeNode node)
        {
            string result = "";
            if (node.name == "root")
                result += "-1" + Environment.NewLine + "root";
            else
            {
                result += node.parent.name + "," + node.parent.id + Environment.NewLine;
                foreach (var i in node.parent.children)
                    result += i.name + "," + i.id + "|";
            }
            return result;
        }
        public void Reload(TreeNode start)
        {
            if (start.parent.children.Count != 0)
                foreach (var i in start.parent.children)
                    i.data.used = false;
        }
    }

    class TreeNode
    {
        public string name;
        public int id;
        int depth;
        public Row data;
        public TreeNode parent;
        public List<TreeNode> children = new List<TreeNode>();
        public TreeNode()
        {
            parent = null;
            name = "root";
            id = -1;
            depth = 0;
        }
        public TreeNode(TreeNode _parent, Row _data, string _name, int _id)
        {
            parent = _parent;
            name = _name;
            id = _id;
            data = _data;
            depth = _parent.depth + 1;
            _parent.children.Add(this);
        }
    }
}
