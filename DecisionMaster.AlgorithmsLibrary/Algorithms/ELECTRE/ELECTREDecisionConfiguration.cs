using System;
using System.Collections.Generic;
using System.Text;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.AlgorithmsLibrary.Algorithms;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.ELECTRE
{
    public class ELECTREDecisionConfiguration : IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public List<ELECTREParameters> Parameters { get; set; }

        public double alpha { get; set; }
        public double beta { get; set; }
    }

    public class ELECTREParameters
    {
        public double p { get; set; }
        public double q { get; set; }
        public double v { get; set; }
    }
}
