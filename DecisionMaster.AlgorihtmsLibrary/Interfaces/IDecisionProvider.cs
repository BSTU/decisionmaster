using DecisionMaster.AlgorihtmsLibrary.Models;

namespace DecisionMaster.AlgorihtmsLibrary.Interfaces
{
    public interface IDecisionProvider
    {
        void Init(IDecisionConfiguration configuration);
        DecisionResultBase Solve(AlternativesBase alternatives);
    }
}
