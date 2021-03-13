using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface IDecisionConfiguration
    {
        List<double> CriteriaRanks { get; set; }
    }
}
