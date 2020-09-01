using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortalData.Models;
using PortalData.Services.AnalysisComponents;
using PortalData.Services.Enums;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services
{
    public class AnalysisService : IAnalysisComputationService
    {
        public List<ReceivedMeasurement> _measurements { get; set; }
        public ComputationResultVM Result { get; set; }

        public ComputationResultVM Compute(List<ReceivedMeasurement> measurements, string operation)
        {
            Operation computation = (Operation)Enum.Parse(typeof(Operation), operation);

            switch (computation)
            {
                case Operation.Average :
                    Result = Count(measurements, new AverageComponent());
                    break;
                case Operation.Max:
                    Result = Count(measurements, new MaxComponent());
                    break;
                case Operation.Trend:
                    Result = Count(measurements, new TrendComponent());
                    break;
            }

            return Result;
        }

        public ComputationResultVM Count(List<ReceivedMeasurement> measurements, IComputable analyzeComponent)
        {
            analyzeComponent.Analyze(measurements);
            var result = analyzeComponent.GetResult();
            return result;
        }
    }
}
