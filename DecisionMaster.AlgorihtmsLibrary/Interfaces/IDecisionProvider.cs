using DecisionMaster.AlgorihtmsLibrary.Models;

namespace DecisionMaster.AlgorihtmsLibrary.Interfaces
{
    public interface IDecisionProvider
    {
        void Init(DecisionConfigurationBase configuration);
        DecisionResultBase Solve(AlternativesBase alternatives);
    }
}
