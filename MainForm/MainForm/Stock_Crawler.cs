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
    public class Stock_Crawler
    {
        List<string> tickerList;
        List<Stock> scannedStocks;

        public Stock_Crawler()
        {
            tickerList = new List<string>();
            scannedStocks = new List<Stock>();
        }

        public void CrawlTickers()
        {
            string json;
            Stopwatch fullTimer = new Stopwatch();
            Stopwatch timer = new Stopwatch();
            fullTimer.Start();

            //use web as a webclient to allow API access
            using (var web = new WebClient())
            {
                timer.Start();
                Console.WriteLine("Started URL...");
                var url = $"https://api.iextrading.com/1.0/ref-data/symbols";
                //Download the returned API call as a string
                json = web.DownloadString(url).Replace("//", "");
                Console.WriteLine("URL took " + timer.ElapsedMilliseconds + "milliseconds.");
                timer.Reset();
            }
            //Parse the json string to a JSON object
            var v = JToken.Parse(json);
            //Display ticker and price for each stock requested in batch.
            foreach (var j in v)
            {
                //JToken i = j.First;
                var ticker = j.SelectToken("symbol");
                tickerList.Add((string)ticker);
                Console.WriteLine(ticker);
            }
            fullTimer.Stop();
            Console.WriteLine("Program finished in " + fullTimer.Elapsed + " seconds.");
        }

        public Stock CrawlStock(string tick)
        {
            Stock toReturn;
            string json;

            using (var web = new WebClient())
            {
                Console.WriteLine("Started URL...");
                var url = $"https://api.iextrading.com/1.0/stock/{tick}/book/";
                //Download the returned API call as a string
                json = web.DownloadString(url).Replace("//", "");
            }
            var v = JToken.Parse(json).First.First;

            toReturn = FillStock(v);
            return toReturn;
        }

        public Stock FillStock(JToken i)
        {
            var name = i.SelectToken("companyName");
            var price = i.SelectToken("latestPrice");
            float tPrice = 0.0f;
            if (float.TryParse(i.SelectToken("latestPrice").ToString(), out tPrice))
            {
                if (!name.Value<string>().Equals("") && !price.Value<string>().Equals(null))
                {
                    var ticker = i.SelectToken("symbol");
                    var ytdC = i.SelectToken("ytdChange");

                    float dH = 0.0f;
                    float dL = 0.0f;
                    float peR = 0.0f;
                    float ltH = 0.0f;
                    float ltL = 0.0f;

                    if (float.TryParse(i.SelectToken("peRatio").ToString(), out peR))
                    {
                        peR = float.Parse(i.SelectToken("peRatio").ToString());
                    }
                    if (peR.Equals(null))
                    {
                        peR = 0.0f;
                    }

                    if (float.TryParse(i.SelectToken("high").ToString(), out dH))
                    {
                        dH = float.Parse(i.SelectToken("high").ToString());
                    }
                    if (dH.Equals(null))
                    {
                        dH = (float)price;
                    }
                    if (float.TryParse(i.SelectToken("low").ToString(), out dL))
                    {
                        dL = float.Parse(i.SelectToken("low").ToString());
                    }
                    if (peR.Equals(null))
                    {
                        peR = (float)price;
                    }

                    if (float.TryParse(i.SelectToken("week52High").ToString(), out ltH))
                    {
                        ltH = float.Parse(i.SelectToken("week52High").ToString());
                    }
                    if (peR.Equals(null))
                    {
                        ltH = dH;
                    }
                    if (float.TryParse(i.SelectToken("week52Low").ToString(), out ltL))
                    {
                        ltL = float.Parse(i.SelectToken("week52Low").ToString());
                    }
                    if (peR.Equals(null))
                    {
                        ltL = dL;
                    }

                    Stock newOne = new Stock((string)name, (string)ticker, (float)price, (float)dH, (float)dL, (float)ltH, (float)ltL, (float)ytdC, (float)peR);
                    scannedStocks.Add(newOne);
                    return newOne;
                }
                else
                    return new Stock("NULL", "NULL", -1, -1, -1, -1, -1, -1, -1);
            }
            else
                return new Stock("NULL", "NULL", -1, -1, -1, -1, -1, -1, -1);
        }

        public List<string> GetTickers()
        {
            return tickerList;
        }

        public bool TickerContained(string check)
        {
            return tickerList.Contains(check);
        }
    }
}
