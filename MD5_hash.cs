using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace md5
{
    class Program
    {
        static void Main()
        {
            string dir_path = "";
            Console.WriteLine("Enter path to folder directory: ");
            bool check = true;
            while (check)
            {
                dir_path = Console.ReadLine();
                if (!Directory.Exists(dir_path))
                {
                    Console.WriteLine("No such directory, prease enter again:");
                }
                else
                {
                    check = false;

                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var hash = GetDirHash(dir_path);
            stopwatch.Stop();

            Console.WriteLine("Time spent: {0}; MD5 hash: {1}", stopwatch.Elapsed, BitConverter.ToString(hash).Replace("-", "").ToLower());

            Console.ReadKey();
        }



        //**************END MAIN**************//



        public static byte[] GetFileHash(FileStream file_stream) 
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(file_stream);
            }
                

        }

        public static byte[] GetDirHash(string path)
        {
            var file_paths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            Array.Sort(file_paths);

            List<Byte[]> hash = new List<Byte[]>();

            using (var md5 = MD5.Create())
            {

                List<Task<byte[]>> tasks = new List<Task<byte[]>>(); 

                foreach (var paths in file_paths)
                {
                    var stream = File.OpenRead(paths);
                    Task<byte[]> task = Task.Run(() => GetFileHash(stream));
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());

                foreach (Task<byte[]> task in tasks) 
                {
                    hash.Add((task.Result));
                }

                byte[] array = hash.SelectMany(a => a).ToArray();

                return md5.ComputeHash(array); 
            }
        }


        
    }
}
