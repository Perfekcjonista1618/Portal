using System.Collections.Generic;
using PortalData.Models;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services
{
    public interface IAnalysisComputationService
    {
        ComputationResultVM Compute(List<ReceivedMeasurement> measurements, string operation);
    }
}
