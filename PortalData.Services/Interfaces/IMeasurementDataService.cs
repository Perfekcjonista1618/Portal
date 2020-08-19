using System;
using System.Linq;
using System.Text;
using PortalData.Models;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services
{
    public interface IMeasurementDataService
    {
        IQueryable<ReceivedMeasurement> ExtractData(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, int? resultWidth, int? resultHeight);

        LineChartVM CreateViewModel(int? portalID, string dataTypeName, DateTime? minDate, DateTime? maxDate, IQueryable<ReceivedMeasurement> measurements, int? resultWidth, int? resultHeight);
        void AddMeasurement(PostMeasurementsVM measurementVM);
        StringBuilder CreateCsv(IQueryable<ReceivedMeasurement> measurements);
    }
}