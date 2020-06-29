using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using System;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.SMART
{
    public class SMARTDecisionProvider : IDecisionProvider
    {
        private SMARTDecisionConfiguration _configuration;
        private AlternativesBase _alternatives;

        public void Init(IDecisionConfiguration configuration)
        {
            if (configuration is SMARTDecisionConfiguration SMARTDecisionConfiguration)
            {
                _configuration = SMARTDecisionConfiguration;
            }
            else
            {
                throw new Exception("Invalid configuration");
            }
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;
            //Main constant for algorithm
            var basis = 1.0 + _configuration.Epsilon;
            var intervals = 6;
            var factor = Math.Pow(basis, intervals); // = 64 (default)
            var root2 = Math.Sqrt(2.0);

            double[,] g = new double[alternatives.Criterias.Count, alternatives.Alternatives.Count];
            int i, j;
            for (i = 0; i < alternatives.Criterias.Count; i++) //i - criteria number
            {
                var critera = alternatives.Criterias[i];
                var minCriteriaValue = critera.MinValue;
                var maxCriteriaValue = critera.MaxValue;
                //posible correcting minCriteraValue
                //minCriteriaValue += (maxCriteriaValue - minCriteriaValue) / factor;
                var deltaCriteraValue = maxCriteriaValue - minCriteriaValue;

                for (j = 0; j < alternatives.Alternatives.Count; j++) // j - alternative number
                {
                    var alternative = alternatives.Alternatives[j];
                    if (critera is IQualitativeCriteria)
                    {
                        g[i, j] = alternative.Values[i].Value;
                    }
                    else
                    {
                        double v;
                        v = Math.Log((alternative.Values[i].Value - minCriteriaValue) * factor / deltaCriteraValue, basis);
                        g[i, j] = critera.CriteriaDirection == CriteriaDirectionType.Maximization ? 4 + v : 10 - v;
                    }
                }
            }
            var w = new double[alternatives.Criterias.Count];

            if (IsCriterialWeightsNormalized() == false)
            {
                double sum = 0;
                for (i = 0; i < alternatives.Criterias.Count; i++)
                {
                    w[i] = Math.Pow(root2, _configuration.CriteriaRanks[i]);
                    sum += w[i];
                }
                //Normalizing w
                for (i = 0; i < alternatives.Criterias.Count; i++)
                {
                    w[i] /= sum;
                }
            }
            else
            {
                for (i = 0; i < alternatives.Criterias.Count; i++)
                {
                    w[i] = _configuration.CriteriaRanks[i];
                }
            }

            var f = new double[alternatives.Alternatives.Count];
            //count alternative ranks
            for (j = 0; j < alternatives.Alternatives.Count; j++)
            {
                f[j] = 0;
                for (i = 0; i < alternatives.Criterias.Count; i++)
                {
                    f[j] += w[i] * g[i, j];
                }
            }
            var permutation = new int[alternatives.Alternatives.Count];
            var number = new int[alternatives.Alternatives.Count];
            for (j = 0; j < alternatives.Alternatives.Count; j++)
            {
                number[j] = j;
            }

            for (int j1 = 0; j1 < alternatives.Alternatives.Count - 1; j1++)
            {
                var maxIndex = j1;
                for (int j2 = j1 + 1; j2 < alternatives.Alternatives.Count; j2++)
                {
                    if (f[j2] > f[maxIndex])
                    {
                        maxIndex = j2;
                    }
                }
                var temp = f[j1];
                f[j1] = f[maxIndex];
                f[maxIndex] = temp;

                var tempNumber = j1;
                number[j1] = number[maxIndex];
                number[maxIndex] = tempNumber;
                permutation[number[j1]] = j1;
            }
            permutation[number[alternatives.Alternatives.Count - 1]] = alternatives.Alternatives.Count - 1;

            var result = new DecisionResultBase();
            for (j = 0; j < alternatives.Alternatives.Count; j++)
            {
                result.Ranks.Add(permutation[j] + 1);
            }
            return result;
        }

        private bool IsCriterialWeightsNormalized()
        {
            double sum = 0;
            for (int i = 0; i < _alternatives.Criterias.Count; i++)
            {
                sum += _configuration.CriteriaRanks[i];
            }

            return (Math.Abs(sum - 1) <= 0.001);
        }
    }
}