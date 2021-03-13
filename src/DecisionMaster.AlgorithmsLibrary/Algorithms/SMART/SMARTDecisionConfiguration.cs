using DecisionMaster.AlgorithmsLibrary.Interfaces;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.SMART
{
    public class SMARTDecisionConfiguration: IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public double Epsilon { get; set; } = 1;
    }
}
