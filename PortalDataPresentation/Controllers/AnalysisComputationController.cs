using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalData.Models;
using PortalData.Services;
using PortalDataPresentation.ViewModels;

namespace PortalDataPresentation.Controllers
{
    [Produces("application/json")]
    public class AnalysisComputationController : Controller
    {
        private readonly IMeasurementDataService _measurementsDataService;
        private readonly IAnalysisComputationService _analysisService;
        public AnalysisComputationController(IAnalysisComputationService analysisService, IMeasurementDataService measurementsDataService)
        {
            _analysisService = analysisService;
            _measurementsDataService = measurementsDataService;
        }
        [HttpPost]
        public JsonResult Analyze([FromBody] AnalysisVM viewmodel)
        {
            if (viewmodel != null)
            {
                List<ReceivedMeasurement> measurements;
                if (viewmodel.dataTypeName.Equals("All"))
                    viewmodel.dataTypeName = null;

                measurements =
                    _measurementsDataService.ExtractData(null, viewmodel.dataTypeName, DateTime.Parse(viewmodel.minDate),
                        DateTime.Parse(viewmodel.maxDate), null, null).ToList();

                if (measurements.Count > 0)
                {
                    var result = _analysisService.Compute(measurements, viewmodel.operation);

                    return Json(new { success = true, result = result });
                }
                else
                    return Json(new { success = false, result = "Selected conditions match no data" }); 
            }
            else
                return Json(new { success = false, result = "No data sent to server" });
        }
    }
}