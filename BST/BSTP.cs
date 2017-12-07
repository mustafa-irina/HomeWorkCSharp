using System;

namespace BST
{
    static partial class BST
    {
        public class Print <K,V> where K : IComparable<K>
        {
            public void PrintTree (BinarySearchTree<K, V> tree)
            {
                Node<K, V> node = tree.root;
                PrintNode(node, 0);
            }

            void PrintNode(Node<K, V> node, int height)
            {
                if (node == null) return;
                Console.ForegroundColor = ConsoleColor.Green;
                PrintNode(node.right, height + 1);
                for (int i = 1; i <= height; i++) Console.Write(" |");
                Console.WriteLine("({0} ; {1})",node.key,node.value);
                PrintNode(node.left, height + 1);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}