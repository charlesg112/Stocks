using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Quotes { 
    public class Quote
    {

        string m_url;

        HtmlDocument m_html;

        public string Name { get; set; }
        public string Symbol { get; set; }

        public string Percentage { get { return GetPercentage(); } }

        public char Sign { get { return GetSign(); } }

        public string Color { get { return GetColor(); } }

        public Quote(string p_url)
        {
            this.m_url = p_url;
            try
            {
                this.m_html = FuncGetHtml();
                this.Name = FindName();
                this.Symbol = FindSymbol();
            }
            catch
            {
                throw new QuoteException("Could not reach URL");
            }

            if (this.Name == null || this.Symbol == null)
            {
                throw new QuoteException("Could not find data");
            }
        }

        public string Value { get { return GetValue(); } }


        public string GetUrl()
        {
            return this.m_url;
        }

        public HtmlDocument FuncGetHtml()
        {   
            HttpClient httpClient = new HttpClient();
            string html = httpClient.GetStringAsync(this.m_url).Result;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument;
        }

        public string GetValue()
        {
            HtmlDocument htmlDocument = m_html;
            string stringValue = htmlDocument.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("Trsdu(0.3s) Fw(b) Fz(36px) Mb(-4px) D(ib)")).FirstOrDefault().InnerText;
            //float value = float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
            return stringValue;
        }

            private string FindName()
        {
            string desc = NameAndSymbol();
            string name = "";
            foreach (char letter in desc)
            {
                if (letter != '(') name += letter;
                else break;
            }

            return name;
        }

        private string FindSymbol()
        {
            string desc = NameAndSymbol();
            string symbol = "";
            bool foundP = false;
            for (int i = 0; i < desc.Length - 1; i++)
            {
                if (foundP) symbol += desc[i];

                else if (desc[i] == '(')
                {
                    foundP = true;
                }
            }
            return symbol;
        }
        
        private string NameAndSymbol()
        {
            HtmlDocument htmlDocument = this.m_html;
            string name = htmlDocument.DocumentNode.Descendants("h1")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("D(ib) Fz(18px)")).FirstOrDefault().InnerText;
            return name;
        }

        public new string ToString()
        {
            StringBuilder sb = new StringBuilder(50);
            sb.AppendLine(this.Name);
            sb.AppendLine(this.Symbol);
            sb.AppendLine(this.GetValue().ToString());
            return sb.ToString();
        }

        public string GetPercentage()
        {

            HtmlDocument htmlDocument = this.m_html;
            string stringValue = htmlDocument.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("Trsdu(0.3s) Fw(500) Pstart(10px)")).FirstOrDefault().InnerText;
            int firstIndex = stringValue.IndexOf('(') + 2;
            int lastIndex = stringValue.IndexOf(')');
            int lenght = lastIndex - firstIndex - 1;
            stringValue = stringValue.Substring(firstIndex, lenght);
            //float value = float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
            return stringValue;
        }

        public char GetSign()
        {
            HtmlDocument htmlDocument = this.m_html;
            string stringValue = htmlDocument.DocumentNode.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("Trsdu(0.3s) Fw(500) Pstart(10px)")).FirstOrDefault().InnerText;
            char value = stringValue[0];
            //float value = float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
            return value;
        }

        public string GetColor()
        {
            string color = "#FF08B600"; // Green
            if (this.Sign == '-') color = "#FFF71D13"; // Red 
            return color;
        }

        public void Refresh()
        {
            this.m_html = FuncGetHtml();
        }
    }
}
