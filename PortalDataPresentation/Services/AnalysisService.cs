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
        public ComputationResultVM Compute(List<ReceivedMeasurement> measurements, AnalysisVM viewmodel)
        {
            Operation computation = (Operation)Enum.Parse(typeof(Operation), viewmodel.operation);

            switch (computation)
            {
                case Operation.Average :
                    return Count(measurements, new AverageComponent(), viewmodel);
                case Operation.Max:
                    return Count(measurements, new MaxComponent(), viewmodel);
                case Operation.Trend:
                    return Count(measurements, new TrendComponent(), viewmodel);
                case Operation.Predict:
                    return Count(measurements, new PredictionComponent(), viewmodel);
                default: return new ComputationResultVM();
            }

        }

        private ComputationResultVM Count(List<ReceivedMeasurement> measurements, IComputable analyzeComponent, AnalysisVM viewmodel)
        {
            analyzeComponent.Analyze(measurements, viewmodel);
            return analyzeComponent.GetResult();
        }
    }
}