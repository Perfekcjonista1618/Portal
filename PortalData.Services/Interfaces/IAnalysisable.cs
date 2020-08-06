using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Bson;
using PortalData.Models;

namespace PortalData.Services
{
    public interface IAnalysisable
    {
        double Compute(List<ReceivedMeasurement> measurements, string operation);
    }
}
