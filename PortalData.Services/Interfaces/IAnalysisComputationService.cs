using System.Collections.Generic;
using PortalData.Models;

namespace PortalData.Services
{
    public interface IAnalysisComputationService
    {
        List<double> Compute(List<ReceivedMeasurement> measurements, string operation);
    }
}
