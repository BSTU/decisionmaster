using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface IQualitativeCriteria: ICriteria
    {
        double GetValue(QualitativeCriteriaEnum qualitativeCriteria);
    }
}