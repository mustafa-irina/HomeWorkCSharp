using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserHTML
{
    class HTMLmain
    {
        static void Main(string[] args)
        {
            var address = Console.ReadLine();
            var parser = new Parser();
            var links = new string[1];
            links[0] = address;
            var pages = parser.Web_link(links).Result;
            var p = new Page();
            foreach (var page in pages)
            {
                var l = p.Get_web_code(page).Result;
                Task.WaitAll();
                Console.WriteLine("Link: {0} - {1} symbols", page, l.Length);
            }

            Console.ReadLine();
        }
    }
}
