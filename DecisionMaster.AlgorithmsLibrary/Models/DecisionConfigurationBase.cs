using DecisionMaster.AlgorithmsLibrary.Interfaces;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Models
{
    public class DecisionConfigurationBase: IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
    }
}
