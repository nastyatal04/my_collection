using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryTree
{
    public class Node<T>
    {
        public List<T> Keys { get; set; }
        public List<BTreeNode<T>> Children { get; set; }
        public bool IsLeaf { get; set; }

        public Node(int degree, bool isLeaf)
        {
            Keys = new List<T>();
            Children = new List<BTreeNode<T>>();
            IsLeaf = isLeaf;
        }
    }

    public class BTree<T> where T : IComparable<T>
    {
        private int degree;
        private Node<T> root;

        public BTree(int degree)
        {
            this.degree = degree;
            root = null;
        }

        public void Insert(T key)
        {
            if (root == null)
            {
                root = new Node<T>(degree, true);
                root.Keys.Add(key);
            }
            else
            {
                if (root.Keys.Count == (2 * degree))
                {
                    Node<T> newRoot = new Node<T>(degree, false);
                    newRoot.Children.Add(root);
                    SplitChild(newRoot, 0, root);
                    InsertNonFull(newRoot, key);
                    root = newRoot;
                }
                else
                    InsertNonFull(root, key);
            }
        }

        private void InsertNonFull(Node<T> node, T key)
        {
            int i = node.Keys.Count - 1;
            if (node.IsLeaf)
            {
                while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
                    i--;
                node.Keys.Insert(i + 1, key);
            }
            else
            {
                while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
                    i--;
                i++;
                if (node.Children[i].Keys.Count == (2 * degree))
                {
                    SplitChild(node, i, node.Children[i]);
                    if (key.CompareTo(node.Keys[i]) > 0)
                        i++;
                }
                InsertNonFull(node.Children[i], key);
            }
        }

        private void SplitChild(Node<T> parentNode, int childIndex, Node<T> childNode)
        {
            Node<T> newChildNode = new Node<T>(degree, childNode.IsLeaf);
            parentNode.Keys.Insert(childIndex, childNode.Keys[degree]);
            parentNode.Children.Insert(childIndex + 1, newChildNode);

            newChildNode.Keys.AddRange(childNode.Keys.GetRange(degree + 1, degree - 1));
            childNode.Keys.RemoveRange(degree, degree);

            if (!childNode.IsLeaf)
            {
                newChildNode.Children.AddRange(childNode.Children.GetRange(degree, degree));
                childNode.Children.RemoveRange(degree, degree);
            }
        }
        public void Print()
        {
            PrintNode(root, "");
        }
        private void PrintNode(Node<T> node, string indent)
        {
            Console.Write(indent);
            foreach (var key in node.Keys)
                Console.Write(key + " ");
            Console.WriteLine();

            if (!node.IsLeaf)
                for (int i = 0; i < node.Children.Count; i++)
                    PrintNode(node.Children[i], indent + "\t");
        }
    }
}