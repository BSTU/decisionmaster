using DecisionMaster.AlgorithmsLibrary.Interfaces;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Models
{
    public class SMARTDecisionConfiguration: IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public double Epsilon { get; set; } = 1;
    }
}
