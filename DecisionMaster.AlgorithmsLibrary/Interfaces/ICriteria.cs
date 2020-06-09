namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface ICriteria
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
