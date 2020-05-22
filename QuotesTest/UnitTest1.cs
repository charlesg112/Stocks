using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quotes;

namespace QuotesTest
{
    [TestClass]
    public class GraphGeneratorUnitTest
    {
        [TestMethod]
        public void MinValue()
        {
            Dictionary<DateTime, float> dict = new Dictionary<DateTime, float>()
            {
                {DateTime.Parse("5/1/2008 8:30:00 AM"), 10}, 
                {DateTime.Parse("5/1/2008 8:31:00 AM"), 12}, 
                {DateTime.Parse("5/1/2008 8:32:00 AM"), 20}, 
                {DateTime.Parse("5/1/2008 8:33:00 AM"), 2}
            };
            GraphGenerator graph = new GraphGenerator(100, 100, dict);

            Assert.AreEqual(2, graph.MinValue);
        }

        [TestMethod]
        public void MaxValue()
        {
            Dictionary<DateTime, float> dict = new Dictionary<DateTime, float>()
            {
                {DateTime.Parse("5/1/2008 8:30:00 AM"), 10},
                {DateTime.Parse("5/1/2008 8:31:00 AM"), 12},
                {DateTime.Parse("5/1/2008 8:32:00 AM"), 20},
                {DateTime.Parse("5/1/2008 8:33:00 AM"), 2}
            };
            GraphGenerator graph = new GraphGenerator(100, 100, dict);

            Assert.AreEqual(20, graph.MaxValue);
        }

        [TestMethod]
        public void MaxTime()
        {
            Dictionary<DateTime, float> dict = new Dictionary<DateTime, float>()
            {
                {DateTime.Parse("5/1/2008 8:30:00 AM"), 10},
                {DateTime.Parse("5/1/2008 8:31:00 AM"), 12},
                {DateTime.Parse("5/1/2008 8:32:00 AM"), 20},
                {DateTime.Parse("5/1/2008 8:33:00 AM"), 2}
            };
            GraphGenerator graph = new GraphGenerator(100, 100, dict);

            Assert.AreEqual(DateTime.Parse("5/1/2008 8:33:00 AM"), graph.MaxTime);
        }

        [TestMethod]
        public void MinTime()
        {
            Dictionary<DateTime, float> dict = new Dictionary<DateTime, float>()
            {
                {DateTime.Parse("5/1/2008 8:30:00 AM"), 10},
                {DateTime.Parse("5/1/2008 8:31:00 AM"), 12},
                {DateTime.Parse("5/1/2008 8:32:00 AM"), 20},
                {DateTime.Parse("5/1/2008 8:33:00 AM"), 2}
            };
            GraphGenerator graph = new GraphGenerator(100, 100, dict);

            Assert.AreEqual(DateTime.Parse("5/1/2008 8:30:00 AM"), graph.MinTime);
        }

    }
}
