using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Quotes
{
    public class GraphGenerator
    {
        #region Data

        private Dictionary<DateTime, float> m_values;
        private int m_horizontalSize;
        private int m_verticalSize;
        private string m_backgroundColor;
        private string m_foregroundColor;
        private int m_margin;
        private int m_innerMarginHorizontal;
        private int m_innerMarginVertical;
        private float m_minValue;
        private float m_maxValue;
        private DateTime m_minTime;
        private DateTime m_maxTime;
        private int m_pointSize;

        public Dictionary<DateTime, float> Values { get { return m_values; } set { m_values = value; } }
        public int HorizontalSize { get { return m_horizontalSize; } set { m_horizontalSize = value; } }
        public int VerticalSize { get { return m_verticalSize; } set { m_verticalSize = value; } }

        public string BackgroundColor { get { return m_backgroundColor; } set { m_backgroundColor = value; } }
        public string ForegroundColor { get { return m_foregroundColor; } set { m_foregroundColor = value; } }
        public int Margin { get { return m_margin; } set { m_margin = value; } }
        public int InnerMarginHorizontal { get { return m_innerMarginHorizontal; } set { m_innerMarginHorizontal = value; } }
        public int InnerMarginVertical { get { return m_innerMarginVertical; } set { m_innerMarginVertical = value; } }
        public float MinValue { get { return m_minValue; } set { m_minValue = value; } }
        public float MaxValue { get { return m_maxValue; } set { m_maxValue = value; } }
        public DateTime MinTime { get { return m_minTime; } set { m_minTime = value; } }
        public DateTime MaxTime { get { return m_maxTime; } set { m_maxTime = value; } }
        public int PointSize { get { return m_pointSize; } set { m_pointSize = value; } }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of the GraphGenerator object, parsing values from a dictionary contaning DateTime and float
        /// </summary>
        /// <param name="p_horizontalSize">The horizontal size of the graph</param>
        /// <param name="p_verticalSize">The vertical size of the graph</param>
        /// <param name="p_values">The dictionary containing the values associated with the proper times</param>
        /// <param name="p_backgroundColor">Background color of the graph</param>
        /// <param name="p_foregroundColor">Color of the points and lines in the graph</param>
        /// <param name="p_margin">Outer margin of the graph, specified in pixels</param>
        /// <param name="p_innerMarginHorizontal">Inner horizontal margin of the graph, specified in pixels</param>
        /// <param name="p_innerMarginVertical">Inner vertical margin of the graph, specified in pixels</param>
        /// <param name="p_pointSize">Size of the points in the graph, specified in pixels</param>
        public GraphGenerator(int p_horizontalSize, int p_verticalSize, Dictionary<DateTime, float> p_values, string p_backgroundColor = "White", 
            string p_foregroundColor = "Black", int p_margin = 20, int p_innerMarginHorizontal = 20, int p_innerMarginVertical = 20, int p_pointSize = 1)
        {
            this.HorizontalSize = p_horizontalSize;
            this.VerticalSize = p_verticalSize;
            this.Values = p_values;
            this.BackgroundColor = p_backgroundColor;
            this.ForegroundColor = p_foregroundColor;
            this.Margin = p_margin;
            this.InnerMarginHorizontal = p_innerMarginHorizontal;
            this.InnerMarginVertical = p_innerMarginVertical;
            this.MinValue = FindMinValue();
            this.MaxValue = FindMaxValue();
            this.MinTime = FindMinTime();
            this.MaxTime = FindMaxTime();
            this.PointSize = p_pointSize;
        }

        #endregion

        public void Generate(string p_path)
        {
            
            Bitmap bmp = new Bitmap(this.HorizontalSize, this.VerticalSize);

            Graphics canvas = CreateCanvas(ref bmp);

            canvas = CreateAxis(ref canvas);

            canvas = CreatePoints(ref canvas);

            Debug.WriteLine("Vertical Resolution : " + bmp.VerticalResolution.ToString());
            Debug.WriteLine("Inner margins : " + this.InnerMarginHorizontal.ToString() + " x " + this.InnerMarginVertical.ToString());

            using (var m = new MemoryStream())
            {
                bmp.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);

                var img = Image.FromStream(m);

                img.Save("C:/Users/Charles-Antoine/Desktop/test.jpg");
            }

           // bmp.Save("C:/Users/Charles-Antoine/Desktop/test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        private Graphics CreateCanvas(ref Bitmap p_bitmap)
        {
            Color backgroundColor = Color.FromName(this.BackgroundColor);

            SolidBrush backgroundBrush = new SolidBrush(backgroundColor);

            Graphics graph = Graphics.FromImage(p_bitmap);

            graph.FillRectangle(backgroundBrush, 0, 0, this.HorizontalSize, this.VerticalSize);

            return graph;
        }

        private Graphics CreateAxis(ref Graphics p_canvas)
        {

            Color foregroundColor = Color.FromName(this.ForegroundColor);
            Pen linesPen = new Pen(foregroundColor, 2);

            /*
            // Vertical axis
            Debug.WriteLine("Vertical axis");
            Debug.WriteLine(Value2Graph(this.MinTime, this.MinValue).ToString());
            Debug.WriteLine(Value2Graph(this.MinTime, this.MaxValue).ToString());
            p_canvas.DrawLine(linesPen, Value2Graph(this.MinTime, this.MinValue), Value2Graph(this.MinTime, this.MaxValue));

            // Horizontal axis
            Debug.WriteLine("Horizontal axis");
            Debug.WriteLine(Value2Graph(this.MinTime, this.MinValue).ToString());
            Debug.WriteLine(Value2Graph(this.MaxTime, this.MinValue).ToString());
            p_canvas.DrawLine(linesPen, Value2Graph(this.MinTime, this.MinValue), Value2Graph(this.MaxTime, this.MinValue));
            */

            // Test
            Point bottomLeft = new Point(this.Margin, this.VerticalSize - this.Margin);
            Point topLeft = new Point(this.Margin, this.Margin);
            Point bottomRight = new Point(this.HorizontalSize - this.Margin, this.VerticalSize - this.Margin);
            p_canvas.DrawLine(linesPen, bottomLeft, topLeft);
            p_canvas.DrawLine(linesPen, bottomLeft, bottomRight);

            return p_canvas;
        }

        private Graphics CreatePoints(ref Graphics p_canvas)
        {
            foreach (KeyValuePair<DateTime, float> valuePair in this.Values)
            {
                Color foregroundColor = Color.FromName(this.ForegroundColor);
                Pen pointsPen = new Pen(foregroundColor, 2);
                Size pointSize = new Size(this.PointSize, this.PointSize);
                Point topCornerOfRect = new Point(Value2Graph(valuePair.Key, valuePair.Value).X - this.PointSize / 2, Value2Graph(valuePair.Key, valuePair.Value).Y - this.PointSize / 2);
                Rectangle pointRect = new Rectangle(topCornerOfRect, pointSize);
                p_canvas.DrawEllipse(pointsPen, pointRect);
            }

            return p_canvas;
        }


        #region Helpers

        /// <summary>
        /// Converts a pair of values in a Point in the graph, taking the margin into account
        /// </summary>
        /// <param name="p_x">The DateTime at which the p_y stock value was recorded</param>
        /// <param name="p_y">Recorded stock value</param>
        /// <returns>The point in the graph's coordinates</returns>
        private Point Value2Graph(DateTime p_x, float p_y)
        {
            /* Debug
            Debug.WriteLine("Seconds diff in Value2Graph call : " + ((p_x - this.MinTime).TotalSeconds).ToString());
            Debug.WriteLine("Dividend in Value2Graph call : " + ((this.MaxTime - this.MinTime).TotalSeconds).ToString());
            */
            
            // Value Calculation
            int xVal = (int)(((p_x - this.MinTime).TotalSeconds/(this.MaxTime - this.MinTime).TotalSeconds) * (this.HorizontalSize - 2 * this.Margin) + this.Margin);
            int yVal = this.VerticalSize - (int)((p_y - this.MinValue) / (this.MaxValue - this.MinValue) * (this.VerticalSize - 2 * this.Margin - 2 * this.InnerMarginVertical) + this.Margin + this.InnerMarginVertical);

            return new Point(xVal, yVal);
        }

        #endregion

        #region Value Parsing

        //TODO situations où il y a juste une valeur dans le dictionnaire

        /// <summary>
        /// Finds the maximal stock value contained in the dictionary and modifies its value considering the InnerMarginTop parameter
        /// </summary>
        /// <returns>The maximal stock value contained in the dictionary</returns>
        private float FindMaxValue()
        {
            if(this.Values.Count == 0)
            {
                return 1;
            }

            float maxValue = this.Values[this.Values.Keys.First()];

            foreach (KeyValuePair<DateTime, float> valuePair in this.Values)
            {
                if (valuePair.Value > maxValue) maxValue = valuePair.Value;
            }

            return maxValue;

        }

        /// <summary>
        /// Finds the minimal stock value contained in the dictionary and modifies its value considering the InnerMarginBottom parameter
        /// </summary>
        /// <returns>The minimal stock value contained in the dictionary</returns>
        private float FindMinValue()
        {
            if (this.Values.Count == 0)
            {
                return 0;
            }

            float minValue = this.Values[this.Values.Keys.First()];

            foreach (KeyValuePair<DateTime, float> valuePair in this.Values)
            {
                if (valuePair.Value < minValue) minValue = valuePair.Value;
            }

            return minValue;

        }

        /// <summary>
        /// Finds the maximal time registered in the dictionary
        /// </summary>
        /// <returns>The maximal time at which the stock value was saved</returns>
        private DateTime FindMaxTime()
        {
            if (this.Values.Count == 0)
            {
                DateTime output = DateTime.Now;
                output = output.AddSeconds(60);
                return output;
            }


            DateTime maxValue = this.Values.Keys.First();

            foreach (KeyValuePair<DateTime, float> valuePair in this.Values)
            {
                if (valuePair.Key > maxValue) maxValue = valuePair.Key;
            }

            return maxValue;
        }

        /// <summary>
        /// Finds the minimal time registered in the dictionary
        /// </summary>
        /// <returns>The minimal time at which the stock value was saved</returns>
        private DateTime FindMinTime()
        {
            if (this.Values.Count == 0)
            {
                return DateTime.Now;
            }

            DateTime minValue = this.Values.Keys.First();

            foreach (KeyValuePair<DateTime, float> valuePair in this.Values)
            {
                if (valuePair.Key < minValue) minValue = valuePair.Key;
            }

            return minValue;
        }

        #endregion
    }
}
