using System.Collections.Generic;

namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class AlternativesBase
    {
        public List<AlternativeBase> Alternatives { get; set; }
        public List<CriteriaBase> Criterias { get; set; }
    }
}
