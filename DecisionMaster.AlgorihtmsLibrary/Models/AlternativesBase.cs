using System.Collections.Generic;

namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class AlternativesBase
    {
        List<AlternativeBase> Alternatives { get; set; }
        List<CriteriaBase> Criterias { get; set; }
    }
}
