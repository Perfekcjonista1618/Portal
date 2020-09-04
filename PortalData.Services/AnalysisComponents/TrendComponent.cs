using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization.Internal;
using PortalData.Models;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services.AnalysisComponents
{
    public class TrendComponent : IComputable
    {
        private ComputationResultVM _result { get; set; }
        public void Analyze(List<ReceivedMeasurement> measurements, AnalysisVM viewmodel)
        {
            
            var topMeasurements = measurements.Take(3);
            _result = new ComputationResultVM()
            {
                X_values = topMeasurements.Select(d => d.RecordCreateTime.ToString("yyyy-MM-dd")).ToList(),
                Y_values = topMeasurements.Select(v => v.Value).ToList()
            };
        }

        public ComputationResultVM GetResult()
        {
            return _result;
        }
    }
}