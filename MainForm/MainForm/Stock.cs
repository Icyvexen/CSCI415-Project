using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainForm
{
    [Serializable()]
    public class Stock
    {
        private string name;
        private string ticker;
        private float currentValue;
        private float dayHigh;
        private float dayLow;
        private float longTermHigh;
        private float longTermLow;
        private float ytdChange;
        private float peRatio;
        private float rating;

        public string Name
        {
            get { return name; }
            set { }
        }
        public string Ticker
        {
            get { return ticker; }
            set { }
        }
        public float CurrentValue
        {
            get { return currentValue; }
            set { }
        }
        public float DayHigh
        {
            get { return dayHigh; }
            set { }
        }
        public float DayLow
        {
            get { return dayLow; }
            set { }
        }
        public float LongTermHigh
        {
            get { return longTermHigh; }
            set { }
        }
        public float LongTermLow
        {
            get { return longTermLow; }
            set { }
        }
        public float YTDChange
        {
            get { return ytdChange; }
            set { }
        }
        public float PERatio
        {
            get { return peRatio; }
            set { }
        }
        public float Rating
        {
            get { return rating; }
            set { }
        }

        public Stock(string nm, string tckr, float crvl, float dayH, float dayL, float longH, float longL, float ytdC, float peR)
        {
            name = nm;
            ticker = tckr;
            currentValue = crvl;
            dayHigh = dayH;
            dayLow = dayL;
            longTermHigh = longH;
            longTermLow = longL;
            ytdChange = ytdC;
            peRatio = peR;
            rating = CalcRating();
        }

        private float CalcRating()
        {
            float toReturn = 0;
            toReturn += peRatio;
            toReturn += (longTermHigh - longTermLow) * ytdChange;
            return toReturn;
        }

        override
        public string ToString()
        {
            string toReturn = "";
            toReturn += "Name: " + name + " Ticker: " + ticker + " Current Value: " + currentValue;
            toReturn += "\nDay High: " + dayHigh + " Day Low: " + dayLow;
            toReturn += "\n52 Week High: " + longTermHigh + " 52 Week Low: " + longTermLow;
            toReturn += "\nYear-to-Date Change: " + ytdChange + " Price to Earnings Ratio: " + peRatio + " Calculated Rating: " + rating + "\n";

            return toReturn;
        }
    }
}
