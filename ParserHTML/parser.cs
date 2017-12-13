using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ParserHTML
{
    class Parser
    {
        public async Task<string[]> Web_link(string[] pages)
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (var page in pages)
                {
                    var array_of_links = await Parser_web_code(page);
                    pages = pages.Concat(array_of_links).ToArray();
                }
            }
            return pages;
        }

        private async Task<string[]> Parser_web_code(string p)
        {
            var links = new List<string>();
            var r = new Regex(@"<a.*? href=""(?<url>https?[\w\.:?&-_=#/]*)""+?");
            Page page = new Page();
            var web_code = await page.Get_web_code(p);

            var link = r.Match(web_code);

            while (link.Success)
            {
                string s1 = link.Groups["url"].Value;;
                links.Add(s1);
                link = link.NextMatch();
            }
            var pages = new string[links.Count];
            int i = 0;
            foreach (var l in links)
            {
                pages[i] = l;
                i++;
            }
            return pages;
        }
    }
}