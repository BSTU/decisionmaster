using System.Collections.Generic;
using DecisionMaster.AlgorithmsLibrary.Interfaces;

namespace DecisionMaster.AlgorithmsLibrary.Models
{
    public class AlternativesBase
    {
        public List<AlternativeBase> Alternatives { get; set; }
        public List<ICriteria> Criterias { get; set; }
    }
}
