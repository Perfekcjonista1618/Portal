using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortalData.Models;
using PortalData.Services.AnalysisComponents;
using PortalData.Services.Enums;

namespace PortalData.Services
{
    public class AnalysisService : IAnalysisComputationService
    {
        public List<ReceivedMeasurement> _measurements { get; set; }
        public List<double> Result { get; set; }

        public List<double> Compute(List<ReceivedMeasurement> measurements, string operation)
        {
            Operation computation = (Operation)Enum.Parse(typeof(Operation), operation);

            switch (computation)
            {
                case Operation.Average :
                    Result = CountAverage(measurements, new AverageComponent());
                    break;
                case Operation.Max:
                    Result = CountMax(measurements, new MaxComponent());
                    break;
            }

            return Result;
        }

        private List<double> CountMax(List<ReceivedMeasurement> measurements, MaxComponent analyzeComponent)
        {
            analyzeComponent.Analyze(measurements);
            var result = analyzeComponent.GetResult();
            return result;
        }

        public List<double>  CountAverage(List<ReceivedMeasurement> measurements, IComputable analyzeComponent)
        {
            analyzeComponent.Analyze(measurements);
            var result = analyzeComponent.GetResult();
            return result;
        }
    }
}
