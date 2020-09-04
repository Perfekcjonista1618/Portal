using System;

namespace PortalDataPresentation.ViewModels
{
    public class AnalysisVM
    {
        public string minDate { get; set; }
        public string maxDate { get; set; }
        public string dataTypeName { get; set; }
        public string operation { get; set; }
        public int polymonialDegree { get; set; }
    }
}