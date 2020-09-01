﻿using System;
using System.Linq;
using System.Text;
using PortalData.Models;
using PortalDataPresentation.DAL;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services
{
    public class MeasurementsDataService : IMeasurementDataService
    {
        private readonly IRepository<MeasurementData> _measurementsRepo;
        private readonly IRepository<DataType> _dataTypesRepo;
        private readonly IRepository<Location> _locationsRepo;
        private readonly IRepository<ArtisticInstalation> _instalationsRepo;
        private readonly IRepository<ReceivedMeasurement> _receivedMeasurementsRepo;
        public MeasurementsDataService(PortalContext context)
        {
            _measurementsRepo = new EfRepository<MeasurementData>(context);
            _dataTypesRepo = new EfRepository<DataType>(context);
            _locationsRepo = new EfRepository<Location>(context);
            _instalationsRepo = new EfRepository<ArtisticInstalation>(context);
            _receivedMeasurementsRepo = new EfRepository<ReceivedMeasurement>(context);
        }
        public IQueryable<ReceivedMeasurement> ExtractData(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight)
        {
            IQueryable<ReceivedMeasurement> measurements = null;

            if (!string.IsNullOrWhiteSpace(dataTypeName))
                measurements = _receivedMeasurementsRepo
                    .SearchBy(x => x.DataTypeKeyName == dataTypeName);
            else
                measurements = _receivedMeasurementsRepo.GetAllAsync();

            if (minDate != null && minDate != DateTime.MinValue)
                measurements = measurements.Where(m => m.RecordCreateTime > minDate);

            if (maxDate != null && maxDate != DateTime.MaxValue)
                measurements = measurements.Where(m => m.RecordCreateTime < maxDate);
            return measurements;
        }

        public LineChartVM CreateViewModel(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, IQueryable<ReceivedMeasurement> measurements,
            int? resultWidth, int? resultHeight)
        {
            LineChartVM viewModel = new LineChartVM()
            {
                ChartHeight = resultHeight,
                ChartWidth = resultWidth,
                dataTypeName = dataTypeName,
                minDate = minDate ?? DateTime.MinValue,
                maxDate = maxDate ?? DateTime.MaxValue,
                Data = measurements.ToList()
            };
            return viewModel;
        }
        public StringBuilder CreateCsv(IQueryable<ReceivedMeasurement> measurements)
        {
            var builder = new StringBuilder();
            builder.AppendLine("ID,\"Measurement type\",\"Value\",\"Measurement date\"");

            foreach (var measurement in measurements)
                builder.AppendLine($"{measurement.ID},\"{measurement.DataTypeKeyName}\",\"{measurement.Value}\",\"{measurement.RecordCreateTime}\"");
            return builder;
        }
        public async void AddMeasurement(PostMeasurementsVM measurementVM)
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
        }
    }
}