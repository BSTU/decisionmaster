namespace DecisionMaster.AlgorithmsLibrary.Interfaces
{
    public interface ICriteria
    {
        double MinValue { get; set; }
        double MaxValue { get; set; }
        CriteriaDirectionType CriteriaDirection { get; set; }
    }
    public enum CriteriaDirectionType
    {
        None = 0,
        Maximization = 1,
        Minimization = 2
    }
}
