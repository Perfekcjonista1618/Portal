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
        private readonly IRepository<MeasurementData> _measurementsRepo;
        private readonly IRepository<DataType> _dataTypesRepo;
        private readonly IRepository<Location> _locationsRepo;
        private readonly IRepository<ArtisticInstalation> _instalationsRepo;
        private readonly IMapper _mapper;
        private readonly IRepository<ReceivedMeasurement> _receivedMeasurementsRepo;

        public MeasurementDatasController(PortalContext context, IMapper mapper)
        {
            _measurementsRepo = new EfRepository<MeasurementData>(context);
            _dataTypesRepo = new EfRepository<DataType>(context);
            _locationsRepo = new EfRepository<Location>(context);
            _instalationsRepo = new EfRepository<ArtisticInstalation>(context);
            _receivedMeasurementsRepo = new EfRepository<ReceivedMeasurement>(context);
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            IQueryable<ReceivedMeasurement> measurements;
            LineChartVM viewModel;

            if (!string.IsNullOrWhiteSpace(dataTypeName))
                measurements = _receivedMeasurementsRepo
                    .SearchBy(x => x.DataTypeKeyName == dataTypeName);
            else
            {
                measurements = _receivedMeasurementsRepo.GetAllAsync();
                dataTypeName = "All";
            }

            if (minDate != null)
                measurements = measurements.Where(m => m.RecordCreateTime > minDate);

            if (maxDate != null)
                measurements = measurements.Where(m => m.RecordCreateTime < maxDate);

            viewModel = new LineChartVM()
            {
                ChartHeight = resultHeight,
                ChartWidth = resultWidth,
                dataTypeName = dataTypeName,
                minDate = minDate ?? DateTime.MinValue,
                maxDate = maxDate ?? DateTime.MaxValue,
                Data = measurements.ToList()
            };

            return View(viewModel);
        }

        public async Task<PartialViewResult> RefreshTable(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<ReceivedMeasurement> measurements;
            measurements = _receivedMeasurementsRepo.Get();
            if (!string.IsNullOrEmpty(dataTypeName))
                measurements = measurements.Where(m => m.DataTypeKeyName.Equals(dataTypeName));
            else
                measurements = measurements.Where(m => m.DataTypeKeyName.Equals("Temperature"));

            return PartialView(measurements);
        }
        public async Task<IActionResult> LineChart(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            LineChartVM viewModel;
            IQueryable<ReceivedMeasurement> filteredMesurments;
            IQueryable<ReceivedMeasurement> measurements;

            measurements = _receivedMeasurementsRepo.Get();

            filteredMesurments = measurements;
            if (!string.IsNullOrEmpty(dataTypeName))
                filteredMesurments = filteredMesurments.Where(r => r.DataTypeKeyName == dataTypeName);
            if (minDate.HasValue)
                filteredMesurments = filteredMesurments.Where(r => r.RecordCreateTime >= minDate);
            if (maxDate.HasValue)
                filteredMesurments = filteredMesurments.Where(r => r.RecordCreateTime <= maxDate);

            viewModel = new LineChartVM()
            {
                ChartHeight = resultHeight,
                ChartWidth = resultWidth,
                Data = filteredMesurments.ToList()
            };
            return PartialView(viewModel);
        }
        public IActionResult DownloadCsv(string dataTypeName, DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<ReceivedMeasurement> measurements;

            if (!string.IsNullOrWhiteSpace(dataTypeName))
                measurements = _receivedMeasurementsRepo
                    .SearchBy(x => x.DataTypeKeyName == dataTypeName);
            else
                measurements = _receivedMeasurementsRepo.GetAllAsync();

            if (minDate != null && minDate != DateTime.MinValue)
                measurements = measurements.Where(m => m.RecordCreateTime > minDate);

            if (maxDate != null && maxDate != DateTime.MaxValue)
                measurements = measurements.Where(m => m.RecordCreateTime < maxDate);

            var builder = new StringBuilder();
            builder.AppendLine("ID,\"Measurement type\",\"Value\",\"Measurement date\"");

            foreach (var measurement in measurements)
                builder.AppendLine($"{measurement.ID},\"{measurement.DataTypeKeyName}\",\"{measurement.Value}\",\"{measurement.RecordCreateTime}\"");
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Measurements.csv");
        }
        public async Task<PartialViewResult> MapLocation()
        {
            return PartialView();
        }

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
            MeasurementData measurement;
            measurement = new MeasurementData()
            {
                DataType = _dataTypesRepo.SearchBy(dt => dt.Name == measurementVM.DataTypeName).FirstOrDefault(),
                Instalation = _instalationsRepo.SearchBy(i => i.Name == measurementVM.InstalationName).FirstOrDefault(),
                Location = _locationsRepo.SearchBy(l => l.Name == measurementVM.LocationName).FirstOrDefault(),
                MeasurmentDate = DateTime.UtcNow,
                Value = double.Parse(measurementVM.Value.Replace('.', ','))
            };

            await _measurementsRepo.AddAsync(measurement);
            return RedirectToAction(nameof(Index));
        }
    }
}
