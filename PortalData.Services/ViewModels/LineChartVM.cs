using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalData.Models;

namespace PortalDataPresentation.ViewModels
{
    public class LineChartVM
    {
        public List<ReceivedMeasurement> Data { get; set; }
        public int? ChartWidth { get; set; }
        public int? ChartHeight { get; set; }
        public DateTime minDate { get; set; }
        public DateTime maxDate { get; set; }
        public string dataTypeName { get; set; }

        public List<SelectListItem> dataTypeNames { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = String.Empty, Text = "All"},
            new SelectListItem { Value = "Temperature", Text = "Temperature" },
            new SelectListItem { Value = "Humidity", Text = "Humidity" },
            new SelectListItem { Value = "Photoresistor", Text = "Photoresistor"  },
            new SelectListItem { Value = "Pressure", Text = "Pressure"  },
            new SelectListItem { Value = "Lux", Text = "Lux"  },
            new SelectListItem { Value = "TEMP", Text = "TEMP"  },
        };
    }
}