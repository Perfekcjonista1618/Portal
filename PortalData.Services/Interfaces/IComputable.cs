using System;
using System.Collections.Generic;
using System.Text;
using PortalData.Models;

namespace PortalData.Services
{
    public interface IComputable
    {
        void Analyze();
        void GetData(List<ReceivedMeasurement> measurements);
    }
}
