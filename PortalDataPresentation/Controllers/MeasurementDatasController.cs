using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalData.Models;
using PortalData.Services;
using PortalDataPresentation.DAL;
using System.Threading.Tasks;
using AutoMapper;
using PortalDataPresentation.ViewModels;

namespace PortalDataPresentation.Controllers
{
    [Produces("application/json")]
    public class MeasurementDatasController : Controller
    {
        private readonly IMeasurementDataService _measurementsDataService;

        public MeasurementDatasController(PortalContext context, IMapper mapper, IMeasurementDataService measurementsDataService)
        {
            _measurementsDataService = measurementsDataService;
        }
        public async Task<IActionResult> Index(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            IQueryable<ReceivedMeasurement> measurements;
            LineChartVM viewModel;

            measurements =
                _measurementsDataService.ExtractData(portalID, dataTypeName, minDate, maxDate, resultWidth, resultHeight);

            if (string.IsNullOrWhiteSpace(dataTypeName))
                dataTypeName = "All";

            viewModel = _measurementsDataService.CreateViewModel(null, dataTypeName, minDate, maxDate, measurements, resultWidth, resultHeight);

            return View(viewModel);
        }

        public async Task<PartialViewResult> RefreshTable(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<ReceivedMeasurement> measurements;
            measurements = _measurementsDataService.ExtractData(portalID, dataTypeName, minDate, maxDate, null, null);

            return PartialView(measurements);
        }

        public async Task<IActionResult> LineChart(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            LineChartVM viewModel;
            IQueryable<ReceivedMeasurement> measurements;

            measurements = _measurementsDataService.ExtractData(portalID, dataTypeName, minDate, maxDate, resultWidth, resultHeight);

            viewModel = _measurementsDataService.CreateViewModel(null, null, null, null, measurements, resultWidth, resultHeight);
            return PartialView(viewModel);
        }

        public IActionResult DownloadCsv(string dataTypeName, DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<ReceivedMeasurement> measurements;
            StringBuilder builder;

            measurements = _measurementsDataService.ExtractData(null, dataTypeName, minDate, maxDate, null, null);
            builder = _measurementsDataService.CreateCsv(measurements);

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Measurements.csv");
        }

        public async Task<PartialViewResult> MapLocation()
        {
            return PartialView();
        }

        //Admin panel features
        public async Task<IActionResult> GetMeasurements(string key, string value)
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PostMeasurements()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostMeasurements(PostMeasurementsVM measurementVM)
        {
            _measurementsDataService.AddMeasurement(measurementVM);
           
            return RedirectToAction(nameof(Index));
        }
    }
}
