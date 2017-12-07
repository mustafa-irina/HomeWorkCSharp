using System;

namespace BST
{
    static partial class BST
    {
        public class BinarySearchTree<K, V> : ITree<K, V> where K : IComparable<K>
        {
            public Node<K, V> root = null;

            public bool Add(K key, V value)
            {
                Node<K, V> parent = null;
                Node<K, V> cur = root;

                while (cur != null)
                {
                    if (parent == null)
                    {
                        lock (cur)
                        {
                            parent = cur;
                            if (key.CompareTo(cur.key) < 0) cur = cur.left;
                            else if (key.CompareTo(cur.key) > 0) cur = cur.right;
                            else if (key.CompareTo(cur.key) == 0)
                            {
                                cur.value = value;
                                return (true);
                            }
                        }
                    }
                    else
                    {
                        lock (parent)
                        {
                            lock (cur)
                            {
                                parent = cur;
                                if (key.CompareTo(cur.key) < 0) cur = cur.left;
                                else if (key.CompareTo(cur.key) > 0) cur = cur.right;
                                else if (key.CompareTo(cur.key) == 0)
                                {
                                    cur.value = value;
                                    return (true);
                                }
                            }
                        }
                    }
                }


                if (parent == null)
                {
                    root = new Node<K, V>(key, value, null);
                    return (true);
                }
                lock (parent)
                {
                    if (key.CompareTo(parent.key) < 0)
                    {
                        cur = new Node<K, V>(key, value, parent);
                        parent.left = cur;
                        return (true);
                    }
                    else
                    {
                        cur = new Node<K, V>(key, value, parent);
                        parent.right = cur;
                        return (true);
                    }
                }
            }

            public V Find(K key)
            {
                Node<K,V> node = (FindNode(key));
                if (node == null) return default(V);
                else return (node.value);
            }

            public Node<K, V> FindNode(K key)
            {
                Node<K, V> cur = root;
                while (cur != null)
                {
                    if (cur.parent == null)
                    {
                        lock (cur)
                        { 
                            if (key.CompareTo(cur.key) == 0)
                            {
                                return cur;
                            }
                            else if (key.CompareTo(cur.key) < 0) cur = cur.left;
                            else cur = cur.right;
                        }
                    }
                    else if (cur.parent != null)
                    {
                        lock (cur.parent)
                        {
                            lock (cur)
                            {
                                if (key.CompareTo(cur.key) == 0)
                                {
                                    return cur;
                                }
                                else if (key.CompareTo(cur.key) < 0) cur = cur.left;
                                else cur = cur.right;
                            }
                        }
                    }
                }
                return null;
            }

            public void Delete(K key)
            {
                Node<K, V> removeNode = FindNode(key);
                if (removeNode == null) return;
                Node<K, V> delParent = removeNode.parent;

                if (delParent == null && removeNode.left == null && removeNode.right == null)
                {
                    lock (removeNode)
                    {
                        root = null;
                        return;
                    }
                }
                else if (delParent != null && removeNode.left == null && removeNode.right == null)
                {
                    lock (delParent)
                    {
                        lock (removeNode)
                        {
                            if (removeNode == delParent.left)
                                delParent.left = null;

                            if (removeNode == delParent.right)
                                delParent.right = null;
                        }
                    }
                }
                else if (delParent == null && removeNode.left == null && removeNode.right != null)
                {
                    lock (removeNode)
                    {
                        removeNode.right.parent = null;
                        root = removeNode.right;
                    }
                }
                else if (delParent == null && removeNode.right == null && removeNode.left != null)
                {
                    lock (removeNode)
                    {
                        removeNode.left.parent = null;
                        root = removeNode.left;
                    }
                }
                else if (delParent != null && (removeNode.right == null || removeNode.left == null))
                {
                    lock (delParent)
                    {
                        lock (removeNode)
                        {
                            if (removeNode.right != null)
                            {
                                if (delParent.left == removeNode)
                                {
                                    removeNode.right.parent = delParent;
                                    delParent.left = removeNode.right;
                                }
                                else if (delParent.right == removeNode)
                                {
                                    removeNode.right.parent = delParent;
                                    delParent.right = removeNode.right;
                                }
                            }
                            else if (removeNode.left != null)
                            {
                                if (delParent.left == removeNode)
                                {
                                    removeNode.left.parent = delParent;
                                    delParent.left = removeNode.left;
                                }
                                else if (delParent.right == removeNode)
                                {
                                    removeNode.left.parent = delParent;
                                    delParent.right = removeNode.left;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (delParent != null)
                    {
                        lock (delParent)
                        {
                            lock (removeNode)
                            {
                                var temp = Min(removeNode.right);
                                removeNode.key = temp.key;

                                if (temp.parent.left == temp)
                                {
                                    temp.parent.left = temp.right;

                                    if (temp.right != null)
                                        temp.right.parent = temp.parent;
                                }
                                else
                                {
                                    temp.parent.right = temp.right;

                                    if (temp.right != null)
                                        temp.right.parent = temp.parent;
                                }
                            }
                        }
                    }
                    else if (delParent == null)
                    {
                        lock (removeNode)
                        {
                            var temp = Min(removeNode.right);
                            removeNode.key = temp.key;

                            if (temp.parent.left == temp)
                            {
                                temp.parent.left = temp.right;

                                if (temp.right != null)
                                    temp.right.parent = temp.parent;
                            }
                            else
                            {
                                temp.parent.right = temp.right;

                                if (temp.right != null)
                                    temp.right.parent = temp.parent;
                            }
                        }
                    }
                }
            }

            public bool IsBST(Node<K, V> node)
            {
                if (node == null) return (true);
                if (node.left != null)
                {
                    if ((Max(node.left).key.CompareTo(node.key) > 0)) return false;
                }
                if (node.right != null)
                {
                    if ((Min(node.right).key.CompareTo(node.key) < 0)) return false;
                }
                if (!IsBST(node.left) || !IsBST(node.right)) return false;
                return true;
            }

            public Node<K, V> Min(Node<K, V> cur)
            {
                if (cur.left == null) return cur;
                else return Min(cur.left);
            }

            public Node<K, V> Max(Node<K, V> cur)
            {
                if (cur.right == null) return cur;
                else return Max(cur.right);
            }
        }
    }
}