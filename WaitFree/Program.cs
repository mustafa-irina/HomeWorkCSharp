using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WaitFree
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const int Task_number = 10;
            const int register_amount = 2;
            
            var registers = new Register[register_amount];

            for (var i = 0; i < register_amount; i++)
            {
                registers[i] = new Register(0, i, register_amount);
            }

            var tasks = new List<Task>();
            var sau = new ScanAndUpdate(registers);

            Task.Run(() =>
            {
                for (int i = 2; i < 12; i += 2)
                {
                    Console.WriteLine("Task {0} begins to Update() register {1} ; val = {2}...",
                        Task.CurrentId, 0, i);
                    sau.Update(0, i);
                    Thread.Sleep(100);
                }


            });

            Task.Run(() =>
            {
                for (int i = 1; i < 12; i += 2)
                {
                    Console.WriteLine("Task {0} begins to Update() register {1} ; val = {2}...",
                        Task.CurrentId, 1, i);
                    sau.Update(1, i);
                    Thread.Sleep(100);
                }


            });


            for (var i = 0; i < Task_number; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Console.WriteLine("Task {0} begins to Scan()...", Task.CurrentId);
                    var array = sau.Scan();
                    Console.WriteLine("Task {0} Scaned :>> {{ {1} , {2} }}\n", Task.CurrentId, array[0], array[1]);
                }));
                Thread.Sleep(100);

            }
            Task.WaitAll(tasks.ToArray());

            Console.ReadKey();
        }
    }
}
