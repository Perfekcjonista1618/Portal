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
        private readonly IAnalysisable _analysisService;
        public AnalysisComputationController(IAnalysisable analysisService)
        {
            _analysisService = analysisService;
        }
        //, string operation  List<ReceivedMeasurement> [FromBody]
        [HttpPost]
        public JsonResult Analyze([FromBody] List<ReceivedMeasurement> measurements, string operation = null)
        {
            if (measurements != null)
            {
               // var result = _analysisService.Compute(measurements, operation);

                return Json(new { success = true, result = "gowno" });
            }
            else
                return Json(new { success = false, result = "gowno" });
        }
    }
}