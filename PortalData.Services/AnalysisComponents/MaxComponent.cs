using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortalData.Models;

namespace PortalData.Services.AnalysisComponents
{
    public class MaxComponent : IComputable
    {
        private double _result { get; set; }
        public void Analyze(List<ReceivedMeasurement> measurements)
        {
            _result = measurements.Max(m => m.Value);
        }

        public double GetResult()
        {
            return _result;
        }
    }
}
