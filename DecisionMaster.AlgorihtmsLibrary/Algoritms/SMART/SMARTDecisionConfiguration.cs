using DecisionMaster.AlgorihtmsLibrary.Interfaces;
using System.Collections.Generic;

namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class SMARTDecisionConfiguration: IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public double Epsilon { get; set; } = 1;
    }
}
