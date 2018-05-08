using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MainForm
{
    class Stock_Crawler
    {
        List<string> tickerList;

        public Stock_Crawler()
        {
            tickerList = new List<string>();
            CrawlTickers();
        }

        public void CrawlTickers()
        {
            string json;
            Stopwatch fullTimer = new Stopwatch();
            Stopwatch timer = new Stopwatch();
            //use web as a webclient to allow API access
            using (var web = new WebClient())
            {
                timer.Start();
                Console.WriteLine("Started URL...");
                var url = $"https://api.iextrading.com/1.0/ref-data/symbols";
                //Download the returned API call as a string
                json = web.DownloadString(url).Replace("//", "");
                Console.WriteLine("URL took " + timer.ElapsedMilliseconds + "milliseconds.");
            }
            //Parse the json string to a JSON object
            var v = JToken.Parse(json);
            //Display ticker and price for each stock requested in batch.
            foreach (var j in v)
            {
                JToken i = j.First;//.First.First;
                var ticker = i.SelectToken("symbol");
                tickerList.Add((string)ticker);
                Console.WriteLine(ticker);
            }
        }

    }
}
