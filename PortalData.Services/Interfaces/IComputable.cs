using System;
using System.Collections.Generic;
using System.Text;
using PortalData.Models;

namespace PortalData.Services
{
    public interface IComputable
    {
        void Analyze(List<ReceivedMeasurement> measurements);
        List<double> GetResult();
    }
}
