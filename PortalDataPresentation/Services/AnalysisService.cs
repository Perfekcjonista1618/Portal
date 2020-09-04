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
        public ComputationResultVM Result { get; set; }

        public ComputationResultVM Compute(List<ReceivedMeasurement> measurements, AnalysisVM viewmodel)
        {
            Operation computation = (Operation)Enum.Parse(typeof(Operation), viewmodel.operation);

            switch (computation)
            {
                case Operation.Average :
                    Count(measurements, new AverageComponent(), viewmodel);
                    break;
                case Operation.Max:
                    Count(measurements, new MaxComponent(), viewmodel);
                    break;
                case Operation.Trend:
                    Count(measurements, new TrendComponent(), viewmodel);
                    break;
            }

            return Result;
        }

        private void Count(List<ReceivedMeasurement> measurements, IComputable analyzeComponent, AnalysisVM viewmodel)
        {
            analyzeComponent.Analyze(measurements, viewmodel);
            Result = analyzeComponent.GetResult();
        }
    }
}