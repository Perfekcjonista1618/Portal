using System.Collections.Generic;
using System.Linq;
using PortalData.Models;

namespace PortalData.Services.AnalysisComponents
{
    public class AverageComponent :IComputable
    {
        private List<double> _result = new List<double>();

         public void Analyze(List<ReceivedMeasurement> measurements)
        {
            _result.Add(measurements.Sum(m => m.Value) / measurements.Count);
        }

        public List<double> GetResult()
        {
            return _result;
        }
    }
}