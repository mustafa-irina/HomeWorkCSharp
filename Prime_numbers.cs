using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace prime
{
    class Program
    {
        public static int count_threads = 0; 
        public static int interval = 0;
        public static int completed = 0, toComplete = -1;
        private static Object locker1 = new Object();        
        private static Object locker2 = new Object();
        public static List<int> prime_list = new List<int>();
        public static ManualResetEvent allDone = new ManualResetEvent(initialState: false);
        
        static void Main()
        {
            int left = 0, right = 0;
            bool is_not_number = true;
            while (is_not_number)
            {
                Console.Write("Enter left margin : ");
                try
                {
                    left = Convert.ToInt32(Console.ReadLine());
                    is_not_number = false;
                }
                catch (Exception)
                {
                    Console.Write("You should write an integer\n");
                }
            }
            is_not_number = true;
            while (is_not_number)
            {
                Console.Write("Enter right margin : ");
                try
                {
                    right = Convert.ToInt32(Console.ReadLine());
                    is_not_number = false;
                    if (left > right)
                    {
                        int buf = right;
                        right = left;
                        left = buf;
                    }
                }
                catch (Exception)
                {
                    Console.Write("You should write an integer\n");
                }
            }
            
            long time = 100000000;
            int opt_interval = 0;
            Stopwatch stopwatch = new Stopwatch();
            interval = right - left + 1;
            do
            {
                stopwatch.Restart();
                Create_Threads(left, right);
                stopwatch.Stop();
                if (time > stopwatch.ElapsedMilliseconds)
                {
                    opt_interval = interval;
                    time = stopwatch.ElapsedMilliseconds;
                }
                if (count_threads == 0) count_threads = 1;
                Console.Write("{0} threads. {1} prime numbers, {2} time spent, interval = {3}\n", count_threads, prime_list.Count, stopwatch.Elapsed, interval);

                prime_list.Clear();
                count_threads = 0;
                interval = interval / 2;
            } while (interval > (right / 64));


            completed = 0;
            toComplete = -1;
            if (interval >= 10) interval = opt_interval / 10;
            else interval = opt_interval;
            int buf1 = (right - left);
            while (buf1 > interval)
            {
                buf1 /= 2;
                toComplete *= 2;
            }
            stopwatch.Restart();
            Create_Tasks(left, right);
            stopwatch.Stop();
            Console.Write("Tasks. {0} prime numbers, {1} time spent\n", prime_list.Count, stopwatch.Elapsed);
            prime_list.Clear();


            completed = 0;
            toComplete = 1;
            stopwatch.Restart();
            Create_Threadpool(left, right);
            allDone.WaitOne();
            stopwatch.Stop();
            Console.Write("ThreadPool. {0} prime numbers, {1} time spent\n", prime_list.Count, stopwatch.Elapsed);
            

            

            Console.ReadKey();
        }


        //*******************END MAIN*******************//


        public static void Create_Threads(int left, int right)
        {
            if ((right - left + 1) <= interval) Check_prime(left, right);
            else
            {
                Thread[] thread = new Thread[2];
                lock (locker1)
                {
                    count_threads += 2;
                }
                thread[0] = new Thread(() => Create_Threads(left, (left + right) / 2));
                thread[1] = new Thread(() => Create_Threads(((left + right) / 2) + 1, right));
                foreach (Thread th in thread) th.Start();
                foreach (Thread th in thread) th.Join();

            }
        }
        

        public static void Create_Threadpool(int left, int right)
        {
            if ((right - left + 1) <= interval)
            {
                ThreadPool.QueueUserWorkItem((_ =>
                {
                    Check_prime(left, right);
                }));
            }
            else
            {
                Create_Threadpool(left, (left + right) / 2);
                Create_Threadpool(((left + right) / 2) + 1, right);
            }
        }

        
        public static void Create_Tasks(int left, int right)
        {
            if ((right - left + 1) <= interval) Check_prime(left, right);
            else
            {
                Task leftTask = Task.Run(() => Create_Tasks(left, (left + right) / 2));
                Task rightTask = Task.Run(() => Create_Tasks(((left + right) / 2) + 1, right));

                Task.WaitAll(leftTask, rightTask);

            }
        }

        
        public static void Check_prime(int left, int right)
        {
            bool check = true;
            for (int i = left; i <= right; i++)
            {
                check = true;
                for (int j = 2; j <= Math.Ceiling(Math.Sqrt(i)); j++)
                {
                    if (((i % j) == 0) && (i != j))
                    {
                        check = false;
                        break;
                    }
                }
                lock (locker2)
                {
                    if (check && (i != 1)) prime_list.Add(i);
                }
            }
            if (Interlocked.Increment(ref completed) == toComplete) allDone.Set();
        }

        
    }
}