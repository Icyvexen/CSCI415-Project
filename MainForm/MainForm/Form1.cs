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
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MainForm
{
    public partial class Form1 : Form
    {
        //Stock_Crawler to collect data
        Stock_Crawler crawl;

        //Port and IP to connect to (for most purposes will be 127.0.0.1 until further testing done).
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        TcpClient client;
        NetworkStream nwStream;

        //array of characters to send in buffer stream.
        char[] sendingText;
        public Form1()
        {
            InitializeComponent();

            //Initialize stock crawler
            crawl = new Stock_Crawler();

            client = new TcpClient(SERVER_IP, PORT_NO);
            nwStream = client.GetStream();

            //Tickers used have a max of 6 characters
            sendingText = new char[6];
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //Is this a batch? True if yes
            //bool batchOrder = batchOrderCheck.Checked;

            //Used to store the added tickers
            string tickers = "";
            
            //base string that represents the JSON and the input
            string input = "";

            //Goes until 'end' is entered
            while (!input.ToUpper().Equals("END"))
            {
                ShowInputDialog(ref input);
                //only adds valid tickers
                if (!input.ToUpper().Equals("END"))
                {
                    tickers = input.ToUpper();
                }
            }
            //Networking section
            try
            {
                //Defines arrays to be used
                char[] textToSend = new char[0];
                byte[] bytesToRead = new byte[0];
                byte[] bytesToSend = new byte[0];
                int bytesRead = 0;

                //Initializes NetworkStream and TCPClient
                textToSend = tickers.ToCharArray();

                bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                MessageBox.Show("Sending : " + tickers);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                //---read back the text---
                bytesToRead = new byte[client.ReceiveBufferSize];
                bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string returned = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                Console.WriteLine("Received : " + returned);
                DisplayBox.AppendText(returned);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            /*{
            //    //use web as a webclient to allow API access
            //    using (var web = new WebClient())
            //    {
            //        if (input.Equals(""))
            //        {
            //            input = "AAPL";
            //        }
            //        var url = $"";
            //        //If batch, special URL used
            //        if (batchOrder)
            //        {
            //            url = $"https://api.iextrading.com/1.0/stock/market/batch?symbols={tickers}&types=quote";
            //        }
            //        else
            //        {
            //            url = $"https://api.iextrading.com/1.0/stock/{tickers}/book/";
            //        }
            //        //Download the returned API call as a string
            //        json = web.DownloadString(url);
            //    }
            //}//Moved JSON
            //json = json.Replace("//", "");
            {
                //Parse the json string to a JSON object
                var v = JToken.Parse(json);
                //Display ticker and other info for each stock requested in batch.
                foreach (var j in v)
                {
                    JToken i = j.First.First.First;
                    Stock stockShow = crawl.FillStock(i);
                    var ticker = i.SelectToken("symbol");
                    var price = i.SelectToken("latestPrice");
                    var peRat = i.SelectToken("peRatio");

                    DisplayBox.AppendText($"{ticker} : {price} : {peRat}");
                    DisplayBox.AppendText(Environment.NewLine);
                    DisplayBox.AppendText(stockShow.ToString());
                }
            }//Old JSON Batch Code
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
            }//Old JSON Solo Code*/
        }

        private static DialogResult ShowInputDialog(ref string input)
        {
            Size size = new Size(200, 100);
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
                Size = new System.Drawing.Size(75, 60),
                Text = "&OK (Type 'END' to end)",
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

        private void ButtonServerTest_Click(object sender, EventArgs e)
        {
            char[] textToSend = new char[0];
            byte[] bytesToRead = new byte[0];
            byte[] bytesToSend = new byte[0];
            int bytesRead;
            TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
            NetworkStream nwStream = client.GetStream();

            string toArray = "MAKEANAMEND";
            while (true)
            {
                textToSend = toArray.ToCharArray();
                
                bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

                //---send the text---
                Console.WriteLine("Sending : " + toArray);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);


                if (toArray.Equals("END"))
                {
                    Console.WriteLine("Sending END...");
                    bytesToRead = new byte[client.ReceiveBufferSize];
                    bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    client.Close();
                    break;
                }
                //---read back the text---
                bytesToRead = new byte[client.ReceiveBufferSize];
                bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                toArray = toArray.Substring(1);
            }

            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }
    }
}
