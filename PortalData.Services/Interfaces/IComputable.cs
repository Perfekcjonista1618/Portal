using System;
using System.Collections.Generic;
using System.Text;
using PortalData.Models;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services
{
    public interface IComputable
    {
        void Analyze(List<ReceivedMeasurement> measurements);
        ComputationResultVM GetResult();
    }
}
