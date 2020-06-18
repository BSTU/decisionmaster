using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
    }
}
