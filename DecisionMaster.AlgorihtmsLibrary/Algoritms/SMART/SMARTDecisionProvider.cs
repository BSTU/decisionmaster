using DecisionMaster.AlgorihtmsLibrary.Interfaces;
using DecisionMaster.AlgorihtmsLibrary.Models;

namespace DecisionMaster.AlgorihtmsLibrary.Algoritms.SMART
{
    public class SMARTDecisionProvider : IDecisionProvider
    {
        private DecisionConfigurationBase _configuration;
        private AlternativesBase _alternatives;

        public void Init(DecisionConfigurationBase configuration)
        {
            _configuration = configuration;
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;
            return new DecisionResultBase();
        }
    }
}
