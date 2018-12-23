using System;
using System.Collections.Generic;
using System.Linq;
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
            IQueryable<MeasurementData> measurements;
            IQueryable<MeasurementDataViewModel> measurementsVM;
            LineChartViewModel viewModel;
            try
            {

                // var measurements = await _repository.GetAllAsync(d => d.DataType, l => l.Location, i => i.Instalation);

                if (portalID != null)
                    measurements = _measurementsRepo.SearchBy(x => x.InstalationID == portalID);
                else
                    measurements = _measurementsRepo.SearchBy(x => x.InstalationID == 1);

                if (!string.IsNullOrWhiteSpace(dataTypeName))
                    measurements = measurements
                        .Include(r => r.DataType)
                        .Where(x => x.DataType.Name == dataTypeName);
                else
                    measurements = measurements
                        .Include(r => r.DataType)
                        .Where(x => x.DataType.Name == "powerLevel");

                measurementsVM = measurements.Select(x => _mapper.Map<MeasurementData, MeasurementDataViewModel>(x));
                viewModel = new LineChartViewModel()
                {
                    ChartHeight = resultHeight,
                    ChartWidth = resultWidth,
                    Data = measurementsVM.ToList()
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return View(viewModel);
        }

        public async Task<PartialViewResult> RefreshTable(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate)
        {

            var measurements = await _measurementsRepo.GetAllAsync(d => d.DataType, l => l.Location, i => i.Instalation);
            var measurementsVM = measurements.Select(x => _mapper.Map<MeasurementData, MeasurementDataViewModel>(x));

            return PartialView(measurementsVM);
        }
        public async Task<IActionResult> LineChart(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            LineChartViewModel viewModel;
            IQueryable<MeasurementData> filteredMesurments;
            IQueryable<MeasurementData> measurements;
            try
            {
                measurements = await _measurementsRepo.GetAllAsync(d => d.DataType, l => l.Location, i => i.Instalation);

                filteredMesurments = measurements;
                if (!string.IsNullOrEmpty(dataTypeName))
                    filteredMesurments = filteredMesurments.Where(r => r.DataType.Name == dataTypeName);
                if (minDate.HasValue)
                    filteredMesurments = filteredMesurments.Where(r => r.MeasurmentDate >= minDate);
                if (maxDate.HasValue)
                    filteredMesurments = filteredMesurments.Where(r => r.MeasurmentDate <= maxDate);

                var measurementsVM = measurements.Select(x => _mapper.Map<MeasurementData, MeasurementDataViewModel>(x));
                viewModel = new LineChartViewModel()
                {
                    ChartHeight = resultHeight,
                    ChartWidth = resultWidth,
                    Data = measurementsVM.ToList()
                };
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return PartialView(viewModel);
        }

        public async Task<PartialViewResult> MapLocation()
        {

            return PartialView();
        }

        public async Task<IActionResult> UpdateView(LineChartViewModel measurementsVM)
        {
            List<MeasurementData> MeasurementsList = new List<MeasurementData>();
            var lastMeasurement = measurementsVM.Data.Max();
            var receivedMeasurementsToAdd = _receivedMeasurementsRepo
                .Get()
                .Where(date => date.RecordCreateTime.Date > lastMeasurement.MeasurmentDate);

            foreach (var receivedMeasurement in receivedMeasurementsToAdd)
            {
                var MeasurementDataTypeID =
                    _dataTypesRepo.Single(type => type.KeyName == receivedMeasurement.DataTypeKeyName).ID;
                MeasurementsList.Add(new MeasurementData { DataTypeID = MeasurementDataTypeID, InstalationID = _instalationsRepo.First().ID, LocationID = _locationsRepo.First().ID, MeasurmentDate = receivedMeasurement.RecordCreateTime.Date, Value = receivedMeasurement.Value });
            }

            foreach (var measurement in MeasurementsList)
            {
            _measurementsRepo.AddAsync(measurement);
            }
            return RedirectToAction(nameof(Index));
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
            try
            {
                measurement = new MeasurementData()
                {
                    DataType = _dataTypesRepo.SearchBy(dt => dt.Name == measurementVM.DataTypeName).FirstOrDefault(),
                    Instalation = _instalationsRepo.SearchBy(i => i.Name == measurementVM.InstalationName).FirstOrDefault(),
                    Location = _locationsRepo.SearchBy(l => l.Name == measurementVM.LocationName).FirstOrDefault(),
                    MeasurmentDate = DateTime.UtcNow,
                    Value = double.Parse(measurementVM.Value.Replace('.', ','))
                };

                await _measurementsRepo.AddAsync(measurement);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
