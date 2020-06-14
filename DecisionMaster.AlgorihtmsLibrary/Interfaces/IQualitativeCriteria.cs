using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface IQualitativeCriteria: ICriteria
    {
        public double GetValue(QualitativeCriteriaEnum qualitativeCriteria);
    }
}
