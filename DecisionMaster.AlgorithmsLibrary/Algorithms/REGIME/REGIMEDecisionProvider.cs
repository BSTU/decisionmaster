using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.REGIME
{
    public class REGIMEDecisionProvider
    {
        private IDecisionConfiguration _configuration;
        private AlternativesBase _alternatives;

        public void Init(IDecisionConfiguration configuration)
        {
            _configuration = configuration;
        }

        private double CalculateSuperiorityIdentifier(
            AlternativeBase lhs, 
            AlternativeBase rhs, 
            List <ICriteria> criterias)
        {
            double result = 0;

            for (int i = 0; i < criterias.Count; ++i)
            {
                var criteria = criterias[i];
                var lhs_value = lhs.Values[i].Value;
                var rhs_value = rhs.Values[i].Value;
                if (criteria is IQualitativeCriteria)
                {
                    if (lhs_value >= rhs_value)
                    {
                        result += _configuration.CriteriaRanks[i];
                    }
                  
                }
                else 
                {
                    if (criteria.CriteriaDirection == CriteriaDirectionType.Maximization &&
                    lhs_value >= rhs_value)
                    {
                        result += _configuration.CriteriaRanks[i];
                    }
                    if (criteria.CriteriaDirection == CriteriaDirectionType.Minimization &&
                        lhs_value <= rhs_value)
                    {
                        result += _configuration.CriteriaRanks[i];
                    }
                }
            }

            return result;
        }

        private double [,] CalculateImpactMatrix(IDecisionConfiguration configuration, AlternativesBase alternatives)
        {
            double[,] impact_matrix = new double[alternatives.Alternatives.Count, alternatives.Alternatives.Count];

            for (int i = 0; i < alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < alternatives.Alternatives.Count; ++j)
                {
                    impact_matrix[i, j] = CalculateSuperiorityIdentifier(
                        alternatives.Alternatives[i],
                        alternatives.Alternatives[j],
                        alternatives.Criterias
                      );
                }
            }

            return impact_matrix;
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;

            //Impacts Matrix
            double[,] impact_matrix = CalculateImpactMatrix(_configuration, _alternatives);

            //calculate range with pairwise comparison
            DecisionResultBase result = new DecisionResultBase();
            for (int i = 0; i < alternatives.Alternatives.Count; ++i)
            {
                int dominated = 0;
                for (int j = 0; j < alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        if (impact_matrix[i,j] - impact_matrix[j,i] >= 0)
                        {
                            dominated++;
                        }
                    }
                }
                result.Ranks.Add(alternatives.Alternatives.Count - dominated);
            }
            return result;
        }
    }
}
