using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface IDecisionProvider
    {
        void Init(IDecisionConfiguration configuration);
        DecisionResultBase Solve(AlternativesBase alternatives);
    }
}
