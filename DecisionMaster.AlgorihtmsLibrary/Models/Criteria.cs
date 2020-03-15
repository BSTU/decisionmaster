namespace DecisionMaster.AlgorihtmsLibrary.Models
{
    public class CriteriaBase
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public CriteriaDirectionType CriteriaDirection { get; set; }
    }
    public enum CriteriaDirectionType
    {
        None = 0,
        Maximization = 1,
        Minimization = 2
    }
}
