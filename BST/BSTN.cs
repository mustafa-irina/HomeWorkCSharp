using System;

namespace BST
{
    static partial class BST
    {
        public class Node<K, V> where K : IComparable<K>
        {
            public Node<K, V> left = null;
            public Node<K, V> right = null;
            public Node<K, V> parent = null;
            public K key;
            public V value;

            public Node(K Key, V Value, Node<K, V> Parent)
            {
                key = Key;
                value = Value;
                parent = Parent;
            }
        }
    }
}