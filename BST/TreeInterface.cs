using System;


namespace BST
{
    static partial class BST
    {
        public interface ITree<K, V> where K : IComparable<K>
        {
            bool Add(K key, V value);

            void Delete(K key);

            V Find(K key);
        }
    }
}