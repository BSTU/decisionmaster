using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using System;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.SMART
{
    public class SMARTQualitativeCriteria : IQualitativeCriteria
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public CriteriaDirectionType CriteriaDirection { get; set; }
        public double GetValue(QualitativeCriteriaEnum qualitativeCriteria)
        {
            double result;
            switch (qualitativeCriteria)
            {
                case QualitativeCriteriaEnum.Poor:
                    result = 4;
                    break;
                case QualitativeCriteriaEnum.FairlyWeak:
                    result = 5;
                    break;
                case QualitativeCriteriaEnum.Medium:
                    result = 6;
                    break;
                case QualitativeCriteriaEnum.FairlyGood:
                    result = 7;
                    break;
                case QualitativeCriteriaEnum.Good:
                    result = 8;
                    break;
                case QualitativeCriteriaEnum.VeryGood:
                    result = 9;
                    break;
                case QualitativeCriteriaEnum.Excellent:
                    result = 10;
                    break;
                default:
                    throw new Exception("Invalid argument");
            };
            return result;
        }
    }
}
