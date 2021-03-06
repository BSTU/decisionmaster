﻿using DecisionMaster.AlgorithmsLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DecisionMaster.AlgorithmsLibrary.Models
{
    public class CriteriaBase : ICriteria
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public CriteriaDirectionType CriteriaDirection { get ; set; }
    }


    public class QualitativeCriteriaBase : IQualitativeCriteria
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public CriteriaDirectionType CriteriaDirection { get; set; }
        public double GetValue(QualitativeCriteriaEnum qualitativeCriteria)
        {
            return (int)qualitativeCriteria;
        }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QualitativeCriteriaEnum
    {
        Poor = 1,
        FairlyWeak = 2,
        Medium = 3,
        FairlyGood = 4,
        Good = 5,
        VeryGood = 6,
        Excellent = 7
    }
}