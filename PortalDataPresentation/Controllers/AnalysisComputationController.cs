using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalData.Models;
using PortalData.Services;

namespace PortalDataPresentation.Controllers
{
    [Produces("application/json")]
    public class AnalysisComputationController : Controller
    {
        private readonly IAnalysisComputationService _analysisService;
        public AnalysisComputationController(IAnalysisComputationService analysisService)
        {
            _analysisService = analysisService;
        }
        [HttpPost]
        public JsonResult Analyze([FromBody] List<ReceivedMeasurement> measurements, string operation = null)
        {
            if (measurements != null)
            {
                var result = _analysisService.Compute(measurements, operation);

                return Json(new { success = true, result = result });
            }
            else
                return Json(new { success = false, result = "gowno" });
        }
    }
}