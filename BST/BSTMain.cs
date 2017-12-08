using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BST
{
    static partial class BST
    {
        class Program
        {
            public static int key;
            public static string value;
            public static bool marker = true;


            static void Main()
            {
                BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
                Console.WriteLine("Работа с деревом (ввод в ручную) или Тестирование скорости распараллеливания?\n" +
                    "1 - для ручного ввода\n" +
                    "2 - Тестирования скорости распараллеливания");
                var choose = 1;
                while (marker)
                {
                    try
                    {
                        choose = Convert.ToInt32(Console.ReadLine());
                        marker = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Ошибка ввода. Введите 1 или 2");
                    }
                }
                marker = true;
                if (choose == 1)
                {
                    char cur = 'I';
                    while (cur != 'E')
                    {
                        if (cur == 'I')
                        {
                            Console.WriteLine("Вставка узлов через Enter \"key value\" (без \"\") ");
                            Console.WriteLine("Выход в основное меню 'M' (без '') ");
                            while (cur == 'I')
                            {
                                var temp1 = Console.ReadLine();
                                if (temp1 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    GetInputForAdd(temp1);
                                    if (!marker) Console.WriteLine("Ошибка ввода. \n" +
                                        "Вставка узлов через Enter \"key value\" (без \"\") \n" +
                                        "Выход в основное меню 'M' (без '') ");
                                    else
                                    {
                                        tree.Add(key, value);
                                    }
                                }
                            }
                        }
                        else if (cur == 'M')
                        {
                            bool flagcase = true;
                            do
                            {
                                Console.WriteLine("Меню\n" +
                                "Добавить новый узел: 'I'\n" +
                                "Поиск значения по ключу 'F'\n" +
                                "Удаление узла 'D'\n" +
                                "Выход 'E'\n" +
                                "Вывод дерева 'P'");

                                var temp2 = Console.ReadLine();
                                switch (temp2)
                                {
                                    case "I":
                                        cur = 'I';
                                        flagcase = false;
                                        break;
                                    case "E":
                                        cur = 'E';
                                        flagcase = false;
                                        break;
                                    case "D":
                                        cur = 'D';
                                        flagcase = false;
                                        break;
                                    case "F":
                                        cur = 'F';
                                        flagcase = false;
                                        break;
                                    case "P":
                                        cur = 'P';
                                        flagcase = false;
                                        break;
                                    default:
                                        Console.WriteLine("Ошибка ввода");
                                        break;
                                }
                            } while (flagcase);

                        }
                        else if (cur == 'D')
                        {
                            Console.WriteLine("Для удаления введите ключ узла");
                            Console.WriteLine("Выход в основное меню 'M' (без '') ");
                            while (cur == 'D')
                            {
                                var temp3 = Console.ReadLine();
                                if (temp3 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    try
                                    {
                                        key = Convert.ToInt32(temp3);
                                        tree.Delete(key);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Ошибка ввода. Введите числовой ключ \n" +
                                        "Для удаления введите ключ узла \n" +
                                        "Выход в основное меню 'M' (без '') ");
                                    }
                                }
                            }
                        }
                        else if (cur == 'F')
                        {
                            Console.WriteLine("Вводите ключи для поиска значений");
                            Console.WriteLine("Выход в основное меню 'M' (без '') ");
                            while (cur == 'F')
                            {
                                var temp4 = Console.ReadLine();
                                if (temp4 == "M")
                                {
                                    cur = 'M';
                                }
                                else
                                {
                                    try
                                    {
                                        key = Convert.ToInt32(temp4);
                                        value = tree.Find(key);
                                        if (value == default(string)) Console.WriteLine("Нет значения");
                                        else Console.WriteLine("{0}", value);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Ошибка ввода. Введите числовой ключ \n" +
                                        "Для удаления введите ключ узла \n" +
                                        "Выход в основное меню 'M' (без '') ");
                                    }
                                }
                            }
                        }
                        else if (cur == 'P')
                        {
                            Print<int, string> print = new Print<int, string>();
                            print.PrintTree(tree);
                            cur = 'M';
                        }
                    }
                }
                else if (choose == 2)
                {
                    int[] toAdd = new int[1000000];                   
                    int[] toDelete = new int[500000];
                    int[] toFind = new int[1000000];

                    Random random = new Random();
                    for (int j =0; j< toAdd.Length; j++)
                    {
                        toAdd[j] = (random.Next(1,10000));
                    }
                    for (int j = 0; j < toDelete.Length; j++)
                    {
                        toDelete[j] = toAdd[(random.Next(1, 1000000))];
                    }
                    for (int j = 0; j < toFind.Length; j++)
                    {
                        toFind[j] = toAdd[(random.Next(1, 1000000))];
                    }
                    
                    BinarySearchTree<int, string> tree_parallel = new BinarySearchTree<int, string>();
                    BinarySearchTree<int, string> tree_NotParallel = new BinarySearchTree<int, string>();

                    Stopwatch add_parallel = Stopwatch.StartNew();
                    Parallel.ForEach(toAdd, key =>
                    {
                        tree_parallel.Add(key,"op");
                    });
                    add_parallel.Stop();
                    Console.WriteLine("parallel add: {0}", add_parallel.Elapsed);
                    if (!tree_parallel.IsBST(tree_parallel.root)) throw new Exception("Parallel add broke BST");

                    
                    Stopwatch del_parallel = Stopwatch.StartNew();
                    Parallel.ForEach(toDelete, key =>
                    {
                        tree_parallel.Delete(key);
                    });
                    del_parallel.Stop();
                    Console.WriteLine("parallel delete: {0}", del_parallel.Elapsed);
                    if (!tree_parallel.IsBST(tree_parallel.root)) throw new Exception("Parallel delete broke BST");

                    Stopwatch find_parallel = Stopwatch.StartNew();
                    Parallel.ForEach(toFind, key =>
                    {
                        tree_parallel.Find(key);
                    });
                    find_parallel.Stop();
                    Console.WriteLine("parallel find: {0}", find_parallel.Elapsed);
                    if (!tree_parallel.IsBST(tree_parallel.root)) throw new Exception("Parallel find broke BST");


                    Stopwatch add_NotParallel = Stopwatch.StartNew();
                    foreach(int key in toAdd)
                    {
                        tree_NotParallel.Add(key,"op");
                    }
                    add_NotParallel.Stop();
                    Console.WriteLine("not parallel add: {0}", add_NotParallel.Elapsed);
                    if (!tree_NotParallel.IsBST(tree_NotParallel.root)) throw new Exception("NotParallel add broke BST");

                    Stopwatch del_NotParallel = Stopwatch.StartNew();
                    foreach (int key in toDelete)
                    {
                        tree_NotParallel.Delete(key);
                    }
                    del_NotParallel.Stop();
                    Console.WriteLine("not parallel delete: {0}", del_NotParallel.Elapsed);
                    if (!tree_NotParallel.IsBST(tree_NotParallel.root)) throw new Exception("NotParallel delete broke BST");

                    Stopwatch find_NotParallel = Stopwatch.StartNew();
                    foreach (int key in toFind)
                    {
                        tree_NotParallel.Find(key);
                    }
                    find_NotParallel.Stop();
                    Console.WriteLine("not parallel find: {0}", find_NotParallel.Elapsed);
                    if (!tree_NotParallel.IsBST(tree_NotParallel.root)) throw new Exception("NotParallel find broke BST");

                   
                    Console.ReadKey();
                }
            }

            static void GetInputForAdd(string temp)
            {
                int pos = temp.IndexOf(' ');
                if (pos == -1)
                {
                    marker = false;
                    return;
                }
                try
                {
                    key = Convert.ToInt32(temp.Substring(0, pos));
                }
                catch (Exception)
                {
                    marker = false;
                    return;
                }
                value = temp.Substring(pos + 1);
                marker = true;
            }
        }
    }
}
