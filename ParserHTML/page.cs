using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ParserHTML
{
    class Page
    {
        public async Task<string> Get_web_code(string page)
        {
            String web_code = "";
            using (WebClient client = new WebClient())
            {
                try
                {
                    web_code = await client.DownloadStringTaskAsync(page);
                }
                catch (Exception ex)
                {
                    Console.Write("ERROR {0} - {1}", page, ex.Message);
                    Console.WriteLine();
                }
            }
            return web_code;
        }
    }
}