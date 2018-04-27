using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainForm
{
    class Stock
    {
        private string name;
        private string ticker;
        private float currentValue;
        private float dayHigh;
        private float dayLow;
        private float longTermHigh;
        private float longTermLow;

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

        public Stock(string nm, string tckr, float crvl, float dayH, float dayL, float longH, float longL)
        {
            name = nm;
            ticker = tckr;
            currentValue = crvl;
            dayHigh = dayH;
            dayLow = dayL;
            longTermHigh = longH;
            longTermLow = longL;
        }
        override
        public string ToString()
        {
            string toReturn = "";
            toReturn += "Name: " + name + " Ticker: " + ticker + " Current Value: " + currentValue + "\nDay High: " + dayHigh + " Day Low: " + dayLow + "\n52 Week High: " + longTermHigh + " 52 Week Low: " + longTermLow;
            return toReturn;
        }
    }
}
