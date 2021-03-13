using System.Collections.Generic;
using DecisionMaster.AlgorithmsLibrary.Interfaces;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.WASPAS
{
    public class WASPASDecisionConfiguration : IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public double Lambda { get; set; } = 0.5;
    }
}
