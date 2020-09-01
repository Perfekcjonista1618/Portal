using System.Collections.Generic;
using System.Linq;
using PortalData.Models;

namespace PortalData.Services.AnalysisComponents
{
    public class AverageComponent :IComputable
    {
        private List<double> _result { get; set; }

        public void Analyze(List<ReceivedMeasurement> measurements)
        {
            _result = new List<double>( new[]
            {
                measurements.Sum(m => m.Value) / measurements.Count
            });
        }

        public List<double> GetResult()
        {
            return _result;
        }
    }
}