using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MainForm;
using Newtonsoft.Json.Linq;

using System.Threading;

namespace Server
{
    public class ServerRun
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        

        public static void Main(string[] args)
        {
            Stock_Crawler crawl = new Stock_Crawler();
            crawl.CrawlTickers();
            Console.WriteLine(crawl.GetTickers());
            //Multi-threading Capabilities
            Thread[] tArray = new Thread[11];
            tArray[0] = new Thread(new ParameterizedThreadStart(Connection));
            tArray[0].Start(crawl);
            List<String> tA = crawl.GetTickers();
            tArray[1] = new Thread(() => CrawlUpdater(tA, crawl));
            tArray[1].Start();
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
            
        }

        public static void Connection(Object stockC)
        {
            Stock_Crawler crawl = (Stock_Crawler)stockC;
            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening...");
            listener.Start();

            //Main form stuff
            string json;

            //---incoming client connected---
            TcpClient client = listener.AcceptTcpClient();

            //---get the incoming data through a network stream---
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //---read incoming stream---
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //---convert the data received into a string---
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received : " + dataReceived);

            //Stock logic
            using (var web = new WebClient())
            {
                var url = $"";
                //If batch, special URL used
                url = $"https://api.iextrading.com/1.0/stock/{dataReceived}/book/";

                //Download the returned API call as a string
                json = web.DownloadString(url);
                Console.WriteLine(url);
            }
            json = json.Replace("//", "");
            var v = JToken.Parse(json);
            var mainStuff = v.First.First;
            Stock stockShow = crawl.FillStock(mainStuff);

            //---write back the text to the client---
            Console.WriteLine("End Received. Sending back : " + stockShow.ToString());
            string toSend = stockShow.ToString();
            switch (stockShow.Rating)
            {
                case float n when n >= 100:
                    toSend += "\nRating of: Watch Carefully";
                    break;
                case float n when n < 100 && n >= 80:
                    toSend += "\nRating of: Optimistic Look";
                    break;
                case float n when n < 80 && n >= 60:
                    toSend += "\nRating of: Solid Choice";
                    break;
                case float n when n < 60 && n >= 40:
                    toSend += "\nRating of: Decent Choice";
                    break;
                case float n when n < 40 && n >= 20:
                    toSend += "\nRating of: Stable, but Stalled";
                    break;
                default:
                    toSend += "\nRating of: Wary, Sell or Keep Eye On";
                    break;
            }

            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(stockShow.ToString());
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            client.Close();
            listener.Stop();

        }

        public static void CrawlUpdater(Object ticks, Object sC)
        {
            List<String> tickers = (List<string>)ticks;
            Console.WriteLine("Print Tickers");
            foreach(string x in tickers)
            {
                Console.WriteLine(x);
            }
            Stock_Crawler y = (Stock_Crawler)sC;
            foreach(string x in tickers){
                Stock dummy = y.CrawlStock(x);
                Console.WriteLine(dummy.ToString());
                //Sleeping to meet API compliance
                Thread.Sleep(15);
            }
        }
    }
}
