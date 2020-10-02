using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization.Internal;
using PortalData.Models;
using PortalDataPresentation.ViewModels;

namespace PortalData.Services.AnalysisComponents
{
    public class TrendComponent : IComputable
    {
        private ComputationResultVM _result { get; set; }
        public void Analyze(List<ReceivedMeasurement> measurements, AnalysisVM viewmodel)
        {
            var dates = measurements.Select(x => x.RecordCreateTime.ToOADate()).ToArray();

            DirectRegressionMethod regressionMethod = (DirectRegressionMethod)Enum.Parse(typeof(DirectRegressionMethod), viewmodel.regressionMethod);

            var polynomialCoefficients = Fit.Polynomial(dates,
                measurements.Select(y => y.Value).ToArray(), viewmodel.polynomialDegree, regressionMethod).ToList();

            var functionFormula = GetFunctionFormula(polynomialCoefficients);

            var approximationResults = GetApproximationResults(dates, polynomialCoefficients);

            //measurements.Select(d => d.RecordCreateTime.ToString("yyyy-MM-dd")).ToList(),
            _result = new ComputationResultVM()
            {
                X_values = dates.Select(x => DateTime.FromOADate(x).ToString("yyyy-MM-dd")).ToList(),
                Y_values = approximationResults,
                Formula = functionFormula
            };
        }
        private string GetFunctionFormula(List<double> polynomialCoefficients)
        {
            string functionFormula = "y = ";
            for (int i = polynomialCoefficients.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                    functionFormula += polynomialCoefficients[i].ToString();
                else
                    functionFormula += polynomialCoefficients[i] + "x^" + i + "+";
            }

            return functionFormula;
        }
        private List<double> GetApproximationResults(double[] dates, List<double> polynomialCoefficients)
        {
            List<double> approximationResults = new List<double>();
            double result = 0;

            foreach (var date in dates)
            {
                result = 0;
                for (int i = polynomialCoefficients.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                        result += polynomialCoefficients[i];
                    else
                        result += polynomialCoefficients[i] * Math.Pow(date, i);
                }

                approximationResults.Add(result);
            }

            return approximationResults;
        }

        public ComputationResultVM GetResult()
        {
            return _result;
        }
    }
}