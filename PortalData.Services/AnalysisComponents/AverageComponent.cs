using System.Collections.Generic;
using System.Linq;
using PortalData.Models;

namespace PortalData.Services.AnalysisComponents
{
    public class AverageComponent :IComputable
    {
        private double _result { get; set; }

         public void Analyze(List<ReceivedMeasurement> measurements)
        {
            _result = measurements.Sum(m => m.Value) / measurements.Count;
        }

        public double GetResult()
        {
            return _result;
        }
    }
}