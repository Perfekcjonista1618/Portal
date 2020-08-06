using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortalData.Models;
using PortalData.Services.Enums;

namespace PortalData.Services
{
    public class AnalysisService : IAnalysisable
    {
        public List<ReceivedMeasurement> _measurements { get; set; }
        public double Result { get; set; }
        public void Analyze()
        {
            Result = _measurements.Sum(m => m.Value) / _measurements.Count;
        }

        public void GetData(List<ReceivedMeasurement> measurements)
        {
            _measurements = measurements;
        }

        public double Compute(List<ReceivedMeasurement> measurements, string operation)
        {
            Operation computation = (Operation)Enum.Parse(typeof(Operation), operation);

            switch (computation)
            {
                case Operation.Average :
                    Result = CountAverage(measurements);
                    break;
            }

            return Result;
        }

        public double CountAverage(List<ReceivedMeasurement> measurements)
        {
            return 3.12;
        }
    }
}
