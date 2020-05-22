using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using Quotes;
using StocksGUI.Directory;

namespace StocksGUI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<QuotesGUI> items = new ObservableCollection<QuotesGUI>();
        string quotesListPath = "../../Directory/QuotesList.txt";

        #region Initialization

        public MainWindow()
        {
            InitializeComponent();

            // File reading
            using (StreamReader sr = new StreamReader(quotesListPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try { 
                    items.Add(new QuotesGUI(line));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
                
            this.QuotesList.ItemsSource = items;

        }

        #endregion

        #region Events
        private void refresh_button_Click(object sender, RoutedEventArgs e)
        {
            foreach (QuotesGUI quote in items)
            {
                quote.Refresh();
                this.refresh_textBox.Text = DateTime.Now.ToString();
            }
            return;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Add_Popup.IsOpen = true;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

            if (this.QuotesList.SelectedItem != null)
            {
                var item = (QuotesGUI)this.QuotesList.SelectedItem;
                RemoveItem(item.URL);
                return;
            }
            this.Remove_Popup.IsOpen = true;
        }

        private void Add_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                QuotesGUI elem = new QuotesGUI(this.URL.Text);
                items.Add(elem);
                Debug.Write("Ajout");
            }
            catch (QuoteException error)
            {
                this.Add_Popup.IsOpen = false;
                MessageBox.Show(error.Message);
                return;
            }

            catch (Exception error)
            {
                this.Add_Popup.IsOpen = false;
                MessageBox.Show("An error occured");
                return;
            }
            this.URL.Text = "";
            this.Add_Popup.IsOpen = false;
        }

        private void Add_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.URL.Text = "";
            this.Add_Popup.IsOpen = false;
        }

        private void Remove_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.URL_remove.Text = "";
            this.Remove_Popup.IsOpen = false;
        }

        private void Remove_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                RemoveItem(this.URL_remove.Text);
            }
            catch
            {
                MessageBox.Show("An error occured");
            }

            this.URL_remove.Text = "";
            this.Remove_Popup.IsOpen = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile(quotesListPath, items);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Graph.Source = null;

            // DateTime creation for testing
            DateTime time1 = DateTime.Parse("5/1/2008 8:30:00 AM");
            DateTime time2 = DateTime.Parse("5/1/2008 8:31:00 AM");
            DateTime time3 = DateTime.Parse("5/1/2008 8:32:00 AM");

            Dictionary<DateTime, float> temp = new Dictionary<DateTime, float>()
            {
                {time1, 10.0f }, {time2, 15.0f}, {time3, 20.0f}
            };
            GraphGenerator graph = new GraphGenerator((int)this.Graph.ActualWidth * 2, (int)this.Graph.ActualHeight * 2, temp) {PointSize = 10};
            Debug.WriteLine("Graph minimal value " + graph.MinValue.ToString());
            Debug.WriteLine("Graph maximal value " + graph.MaxValue.ToString());
            graph.Generate("tes");
            this.Graph.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/Charles-Antoine/Desktop/test.jpg"));
        }

        #endregion

        #region Helpers
        private void SaveFile(string p_path, ObservableCollection<QuotesGUI> p_quotesList)
        {
            File.WriteAllText(p_path, String.Empty);

            using (StreamWriter sr = new StreamWriter(p_path))
            {
                foreach (QuotesGUI item in p_quotesList)
                {
                    sr.WriteLine(item.URL);
                }
            }
        }

        private void RemoveItem(string p_url)
        {
            items.Remove(new QuotesGUI(p_url));
        }

        private void CreateGraph()
        {

            /*
            Bitmap output = new Bitmap(100, 100);

            Graphics test = Graphics.FromImage(output);

            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red, 5);

            test.DrawLine(myPen, new System.Drawing.Point(10, 10), new System.Drawing.Point(20, 20));

            output.Save("C:/Users/Charles-Antoine/Desktop/test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            */

        }

        #endregion


    }

}

/*
Code pour faire apparaitre une nouvelle fenêtre
AddQuoteWindow add = new AddQuoteWindow();
App.Current.MainWindow = add;
this.Close();
add.Show();
*/
