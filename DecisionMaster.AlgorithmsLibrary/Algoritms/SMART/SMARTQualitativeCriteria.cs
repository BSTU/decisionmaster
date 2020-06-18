using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using System;

namespace DecisionMaster.AlgorithmsLibrary.Algoritms.SMART
{
    public class SMARTQualitativeCriteria: IQualitativeCriteria
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public CriteriaDirectionType CriteriaDirection { get; set; }
        public double GetValue(QualitativeCriteriaEnum qualitativeCriteria) => qualitativeCriteria switch
        {
            QualitativeCriteriaEnum.Poor => 4,
            QualitativeCriteriaEnum.FairlyWeak => 5,
            QualitativeCriteriaEnum.Medium => 6,
            QualitativeCriteriaEnum.FairlyGood => 7,
            QualitativeCriteriaEnum.Good => 8,
            QualitativeCriteriaEnum.VeryGood => 9,
            QualitativeCriteriaEnum.Excellent => 10,
            _ => throw new Exception("Invalid argument")
        };
    }
}
