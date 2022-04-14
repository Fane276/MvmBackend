using System.Collections.Generic;

namespace MvManagement.Chart
{
    public class BarChart
    {
        public List<string> Labels { get; }  = new List<string>();
        public List<double> Values { get; }  = new List<double>();
        public double TotalValues { get; set; } = 0;

        public void AddItem(BarChartItem item)
        {
            Labels.Add(item.Label);
            Values.Add(item.Value);
            TotalValues += item.Value;
        }
    }
}