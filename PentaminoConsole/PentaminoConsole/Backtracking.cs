using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PentaminoConsole
{
    class Backtracking
    {
    }

    class Tree
    {
        public TreeNode root;
        public TreeNode current;
        public Tree()
        {
            root = current = new TreeNode();
        }
        public TreeNode AddNode(TreeNode parent, string name, int id)
        {
            current = new TreeNode(parent, name, id);
            return current;
        }
        public TreeNode Back(TreeNode child)
        {
            current = child.parent;
            return current;
        }
    }

    class TreeNode
    {
        public string name;
        public int id;
        public TreeNode parent;
        public List<TreeNode> children = new List<TreeNode>();
        public TreeNode()
        {
            parent = null;
            name = "root";
            id = -1;
        }
        public TreeNode(TreeNode _parent, string _name, int _id)
        {
            parent = _parent;
            name = _name;
            id = _id;
            _parent.children.Add(this);
        }
    }
}
