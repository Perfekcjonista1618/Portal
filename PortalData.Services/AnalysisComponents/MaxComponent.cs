using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortalData.Models;

namespace PortalData.Services.AnalysisComponents
{
    public class MaxComponent : IComputable
    {
        private List<double> _result { get; set; }
        public void Analyze(List<ReceivedMeasurement> measurements)
        {
            _result = new List<double>(new[]
            {
                measurements.Max(m => m.Value)
            });
        }

        public List<double> GetResult()
        {
            return _result;
        }
    }
}