using DecisionMaster.AlgorihtmsLibrary.Interfaces;
using System.Collections.Generic;

namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class AlternativeBase
    {
        public List<IAlternativeValue> Values { get; set; }
    }

    public class AlternativeValueBase : IAlternativeValue
    {
        public double Value { get; }

        public AlternativeValueBase(double value)
        {
            Value = value;
        }
    }

    public class QualitativeAlternativeValue : IAlternativeValue
    {
        QualitativeCriteriaEnum _qualitativeCriteriaValue;
        IQualitativeCriteria _qualitativeCriteriaConverter;
        public double Value => _qualitativeCriteriaConverter.GetValue(_qualitativeCriteriaValue);
        public QualitativeAlternativeValue(QualitativeCriteriaEnum qualitativeCriteriaValue, IQualitativeCriteria qualitativeCriteriaConverter)
        {
            _qualitativeCriteriaValue = qualitativeCriteriaValue;
            _qualitativeCriteriaConverter = qualitativeCriteriaConverter;
        }
    }
}
