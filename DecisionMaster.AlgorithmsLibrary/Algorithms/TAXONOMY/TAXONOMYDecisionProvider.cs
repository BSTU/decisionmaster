using System;
using System.Collections.Generic;
using System.Text;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.TAXONOMY
{
    public class TAXONOMYDecisionProvider : IDecisionProvider
    {
        IDecisionConfiguration _configuration;
        AlternativesBase _alternatives;

        public void Init(IDecisionConfiguration config)
        {
            _configuration = config;
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;
            double[,] normalizeWeights = GetNormalizedMatrix();
            double[] bestAlternative = GetBestAlternative(normalizeWeights);
            double[] distances = GetBlockDistances(
                normalizeWeights,
                bestAlternative
                );

            DecisionResultBase result = new DecisionResultBase();

            for (int i = 0; i < distances.Length; ++i)
            {
                int rank = distances.Length;
                for (int j = 0; j < distances.Length; ++j)
                {
                    if (i!=j && distances[i] <= distances[j])
                    {
                        --rank;
                    }
                }
                result.Ranks.Add(rank);
            }
            return result;
        }

        private double [,] GetNormalizedMatrix()
        {
            double [] meanDeviations = GetMeanDeviations();
            double[] standartDeviations = GetStandartDeviations(meanDeviations);

            double[,] result = new double [_alternatives.Alternatives.Count, _alternatives.Criterias.Count];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Criterias.Count; ++j)
                {
                    result[i,j] = (_alternatives.Alternatives[i].Values[j].Value - meanDeviations[j]) / standartDeviations[j];
                }
            }

            return result;
        }

        private double [] GetMeanDeviations()
        {
            double [] result = new double [_alternatives.Criterias.Count];
            for (int i = 0; i < _alternatives.Criterias.Count; ++i)
            {
                result[i] = 0;
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    result[i] += _alternatives.Alternatives[j].Values[i].Value;
                }
                result[i] /= _alternatives.Alternatives.Count;
            }

            return result;
        }

        private double [] GetStandartDeviations(double [] meanDeviations)
        {
            double[] result = new double[meanDeviations.Length];

            for (int i = 0; i < _alternatives.Criterias.Count; ++i)
            {
                result[i] = 0;
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    result[i] += Math.Pow(_alternatives.Alternatives[j].Values[i].Value - meanDeviations[i], 2);
                }
                result[i] /= _alternatives.Alternatives.Count;
                result[i] = Math.Sqrt(result[i]);
            }
            return result;
        }

        private double[] GetBestAlternative(double [,] matrix)
        {
            double[] result = new double [_alternatives.Criterias.Count];
            for (int i = 0; i < _alternatives.Criterias.Count; ++i)
            {
                result[i] = _alternatives.Alternatives[0].Values[i].Value;
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (_alternatives.Criterias[i] is IQualitativeCriteria ||
                        _alternatives.Criterias[i].CriteriaDirection == CriteriaDirectionType.Maximization)
                    {
                        result[i] = Math.Max(result[i], matrix[j,i]);
                    }
                    else
                    {
                        result[i] = Math.Min(result[i], matrix[j, i]);
                    }
                }
            }
            return result;
        }

        private double[] GetBlockDistances(double[,] matrix, double[] best)
        {
            double []result = new double [_alternatives.Alternatives.Count];

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                result[i] = 0;
                for (int j = 0; j < _configuration.CriteriaRanks.Count; ++j)
                {
                    result[i] += _configuration.CriteriaRanks[j] * Math.Pow(matrix[i,j] - best[j], 2);
                }
            }

            return result;
        }
    }
}
