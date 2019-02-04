using System.Collections.Generic;
using PortalData.Models;

namespace PortalDataPresentation.ViewModels
{
    public class LineChartVM
    {
        public List<ReceivedMeasurement> Data { get; set; }
        public int? ChartWidth { get; set; }
        public int? ChartHeight { get; set; }
    }
}