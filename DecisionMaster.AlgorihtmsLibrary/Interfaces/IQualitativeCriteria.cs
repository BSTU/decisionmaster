using DecisionMaster.AlgorihtmsLibrary.Models;

namespace DecisionMaster.AlgorihtmsLibrary.Interfaces
{
    public interface IQualitativeCriteria: ICriteria
    {
        public double GetValue(QualitativeCriteriaEnum qualitativeCriteria);
    }
}
