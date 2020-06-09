using System.Collections.Generic;
using DecisionMaster.AlgorihtmsLibrary.Interfaces;

namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class AlternativesBase
    {
        public List<AlternativeBase> Alternatives { get; set; }
        public List<ICriteria> Criterias { get; set; }
    }
}
