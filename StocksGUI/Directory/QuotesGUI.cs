using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using Quotes;

namespace StocksGUI.Directory
{
    class QuotesGUI : INotifyPropertyChanged
    {
        #region Attributes
        private Quote m_quote;
        private string m_value;
        private string m_percentage;
        private char m_sign;
        private string m_color;
        private string m_URL;
        public string Value { get { return this.m_value; } set { this.m_value = value; } }
        public string Name { get { return m_quote.Name; } }
        public string Symbol { get { return m_quote.Symbol; } }

        public string Percentage { get { return m_percentage; } set { this.m_percentage = value; } }

        public char Sign { get { return m_sign; } set { this.m_sign = value; } }

        public string Color { get { return m_color; } set { this.m_color = value; } }

        public string URL { get { return m_URL; } set { this.m_URL = value; } }

        #endregion

        #region Constructors
        public QuotesGUI(Quote p_quote)
        {
            m_quote = p_quote;
        }

        public QuotesGUI(string p_url)
        {
            URL = p_url;
            m_quote = new Quote(p_url);
            SetValues();
            
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged = (IChannelSender, e) => { };

        #region Public Methods
        public void Refresh()
        {
            m_quote.Refresh();
            SetValues();
            PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
            PropertyChanged(this, new PropertyChangedEventArgs("Sign"));
            PropertyChanged(this, new PropertyChangedEventArgs("Color"));
        }

        public void SetValues()
        {
            this.Value = m_quote.Value;
            this.Percentage = m_quote.Percentage;
            this.Sign = m_quote.Sign;
            this.Color = m_quote.Color;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is QuotesGUI)) return false;
            return this.URL == ((QuotesGUI)obj).URL;
        }

        public override int GetHashCode()
        {
            return URL.GetHashCode();
        }

        #endregion
    }
}
