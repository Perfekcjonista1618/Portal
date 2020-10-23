using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalData.Models;
using PortalData.Services;
using PortalDataPresentation.ViewModels;

namespace PortalDataPresentation.Controllers
{
    public class MaxController : Controller
    {
        private readonly IMeasurementDataService _measurementsDataService;
        private readonly IAnalysisComputationService _analysisService;
        public MaxController(IAnalysisComputationService analysisService, IMeasurementDataService measurementsDataService)
        {
            _measurementsDataService = measurementsDataService;
            _analysisService = analysisService;
        }
        public IActionResult Index(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            IQueryable<ReceivedMeasurement> measurements;
            LineChartVM viewModel;

            measurements =
                _measurementsDataService.ExtractData(portalID, dataTypeName, minDate, maxDate);

            if (string.IsNullOrWhiteSpace(dataTypeName))
                dataTypeName = "All";

            viewModel = _measurementsDataService.CreateViewModel(null, "Max", dataTypeName, minDate, maxDate, null, measurements, resultWidth, resultHeight);

            return View(viewModel);
        }
        [HttpPost]
        public JsonResult Max([FromBody] AnalysisVM viewmodel)
        {
            if (viewmodel != null)
            {
                List<ReceivedMeasurement> measurements;
                if (viewmodel.dataTypeName.Equals("All"))
                    viewmodel.dataTypeName = null;

                measurements =
                    _measurementsDataService.ExtractData(null, viewmodel.dataTypeName, DateTime.Parse(viewmodel.minDate),
                        DateTime.Parse(viewmodel.maxDate)).ToList();

                if (measurements.Count > 0)
                {
                    var result = _analysisService.Max(measurements, viewmodel);

                    return Json(new { success = true, labels = result.X_values, result = result.Y_values});
                }
                else
                    return Json(new { success = false, result = "Selected conditions match no data" });
            }
            else
                return Json(new { success = false, result = "No data sent to server" });
        }
    }
}