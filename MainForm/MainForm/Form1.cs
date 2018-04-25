using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MainForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //Is this a batch? True if yes
            bool batchOrder = batchOrderCheck.Checked;
            //Comma separated list of tickers to use as standby

            //TODO: Add custom ticker ability
            const string tickers = "AAPL,GOOG,GOOGL,YHOO,TSLA,INTC,AMZN,BIDU,ORCL,MSFT,ORCL,ATVI,NVDA,GME,LNKD,NFLX";

            //base string that represents the JSON
            string json;

            //use web as a webclient to allow API access
            using (var web = new WebClient())
            {
                var url = $"";
                //If batch, special URL used
                if(batchOrder)
                {
                    url = $"https://api.iextrading.com/1.0/stock/market/batch?symbols={tickers}&types=quote";
                }
                else
                {
                    url = $"https://api.iextrading.com/1.0/stock/GOOG/book/";
                }
                //Download the returned API call as a string
                json = web.DownloadString(url);
            }

            json = json.Replace("//", "");
            if (batchOrder)
            {
                //Parse the json string to a JSON object
                var v = JToken.Parse(json);
                //Display ticker and price for each stock requested in batch.
                foreach (var j in v)
                {
                    var i = j.First.First.First;
                    var ticker = i.SelectToken("symbol");
                    var price = i.SelectToken("close");

                    DisplayBox.AppendText($"{ticker} : {price}");
                    DisplayBox.AppendText(Environment.NewLine);
                }
            }
            else
            {
                var v = JToken.Parse(json);
                var mainStuff = v.First.First;
                var ticker = mainStuff.SelectToken("symbol");
                var price = mainStuff.SelectToken("close");

                DisplayBox.Text += ($"{ticker} : {price}");
            }

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}
