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
using Microsoft.VisualBasic;

namespace MainForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Stock_Crawler to collect data
        Stock_Crawler crawl;

        private void SearchButton_Click(object sender, EventArgs e)
        {
            crawl = new Stock_Crawler();
            //Is this a batch? True if yes
            bool batchOrder = batchOrderCheck.Checked;
            //Comma separated list of tickers to use as standby

            //TODO: Add custom ticker ability
            string tickers = "";

            //base string that represents the JSON
            string json;
            string input = "";
            while (!crawl.TickerContained(input.ToUpper()))
            {
                ShowInputDialog(ref input);
                if (batchOrder)
                {
                    if (input.ToUpper().Equals("END"))
                    {
                        input = "GOOG";
                        break;
                    }
                    else
                    {
                        if (tickers.Equals("") && crawl.TickerContained(input.ToUpper()))
                        {
                            tickers += input.ToUpper();
                        }
                        else if (crawl.TickerContained(input.ToUpper()))
                        {
                            tickers += "," + input.ToUpper();
                        }
                    }
                }
                else
                {
                    if (input.ToUpper().Equals("END"))
                    {
                        input = "GOOG";
                        break;
                    }
                    else if (crawl.TickerContained(input.ToUpper()))
                    {
                        input = input.ToUpper();
                    }
                }
            }

            //use web as a webclient to allow API access
            using (var web = new WebClient())
            {
                var url = $"";
                //If batch, special URL used
                if (batchOrder)
                {
                    url = $"https://api.iextrading.com/1.0/stock/market/batch?symbols={tickers}&types=quote";
                }
                else
                {
                    url = $"https://api.iextrading.com/1.0/stock/{input}/book/";
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
                    JToken i = j.First.First.First;
                    Stock stockShow = crawl.FillStock(i);
                    var ticker = i.SelectToken("symbol");
                    var price = i.SelectToken("latestPrice");
                    var peRat = i.SelectToken("peRatio");

                    DisplayBox.AppendText($"{ticker} : {price} : {peRat}");
                    DisplayBox.AppendText(Environment.NewLine);
                    MessageBox.Show(j.ToString());
                }
            }
            else
            {
                var v = JToken.Parse(json);
                var mainStuff = v.First.First;
                Stock stockShow = crawl.FillStock(mainStuff);
                var ticker = mainStuff.SelectToken("symbol");
                var price = mainStuff.SelectToken("close");
                var peRat = mainStuff.SelectToken("peRatio");
                var rating = stockShow.Rating;

                DisplayBox.Text += ($"{ticker} : {price} : {peRat} : {rating}");
                DisplayBox.AppendText(Environment.NewLine);
                DisplayBox.AppendText(stockShow.ToString());
            }
        }

        private void ButtonTestAllTickers_Click(object sender, EventArgs e)
        {
            crawl = new Stock_Crawler();
            crawl.CrawlTickers();
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
                ClientSize = size,
                Text = "Name"
            };

            TextBox textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = System.Windows.Forms.DialogResult.OK,
                Name = "okButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&OK",
                Location = new System.Drawing.Point(size.Width - 80 - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel,
                Name = "cancelButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&Cancel",
                Location = new System.Drawing.Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
    }
}
