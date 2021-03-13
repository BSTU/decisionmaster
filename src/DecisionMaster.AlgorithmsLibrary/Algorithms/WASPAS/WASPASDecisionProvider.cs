using System;
using System.Collections.Generic;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.WASPAS
{
    public class WASPASDecisionProvider : IDecisionProvider
    {
        private WASPASDecisionConfiguration _configuration;
        private AlternativesBase _alternatives;

        public void Init(IDecisionConfiguration configuration)
        {
            if (configuration is WASPASDecisionConfiguration)
            {
                _configuration = (WASPASDecisionConfiguration)configuration;
            }
            else
            {
                throw new Exception("Invalid configuration");
            }
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;

            double[,] normalizedDecisionMarix = GetNormalizedMatrix();

            double[] ranks = GetResultValues(
                GetAdditiveValues(normalizedDecisionMarix),
                GetMultiplicativeValues(normalizedDecisionMarix)
                );

            //calculate range with pairwise comparison
            DecisionResultBase result = new DecisionResultBase();
            for (int i = 0; i < alternatives.Alternatives.Count; ++i)
            {
                int dominated = 0;
                for (int j = 0; j < alternatives.Alternatives.Count; ++j)
                {
                    if (i != j && ranks[i] >= ranks[j])
                    {
                        ++dominated;
                    }
                }
                result.Ranks.Add(alternatives.Alternatives.Count - dominated);
            }

            return result;
        }

        private double [] GetResultValues(double [] additive, double [] multiplicative)
        {
            double[] result = new double [additive.Length];
            for (int i = 0; i < additive.Length; ++i)
            {
                result[i] = _configuration.Lambda * additive[i] + (1 - _configuration.Lambda) * multiplicative[i];
            }
            return result;
        }

        private double [,] GetNormalizedMatrix()
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Criterias.Count];

            List<double> MinMaxValues = GetMinMaxValues();
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Criterias.Count; ++j)
                {
                    if (_alternatives.Criterias[j] is QualitativeCriteriaBase ||
                    _alternatives.Criterias[j].CriteriaDirection == CriteriaDirectionType.Maximization)
                    {
                        result[i, j] = _alternatives.Alternatives[i].Values[j].Value / MinMaxValues[j];
                    }
                    else
                    {
                        result[i, j] = MinMaxValues[j] / _alternatives.Alternatives[i].Values[j].Value;
                    }
                }
            }      

            return result;
        }

        double [] GetAdditiveValues(double [,] matrix)
        {
            double[] result = new double[matrix.Length/_configuration.CriteriaRanks.Count];

            for (int i = 0; i < matrix.Length/_configuration.CriteriaRanks.Count; ++i)
            {
                double newVal = 0;
                for (int j = 0; j < _configuration.CriteriaRanks.Count; ++j)
                {
                    newVal += matrix[i, j] * _configuration.CriteriaRanks[j];
                }
                result[i] = newVal;
            }

            return result;
        }

        double [] GetMultiplicativeValues(double [,] matrix)
        {
            double[] result = new double[matrix.Length / _configuration.CriteriaRanks.Count];

            for (int i = 0; i < matrix.Length / _configuration.CriteriaRanks.Count; ++i)
            {
                double newVal = 1;
                for (int j = 0; j < _configuration.CriteriaRanks.Count; ++j)
                {
                    newVal *= Math.Pow(matrix[i, j],_configuration.CriteriaRanks[j]);
                }
                result[i] = newVal;
            }

            return result;
        }

        private List<double> GetMinMaxValues()
        {
            List<double> result = new List<double>();
            for (int i = 0; i < _alternatives.Criterias.Count; ++i)
            {
                if (_alternatives.Criterias[i] is IQualitativeCriteria || 
                    _alternatives.Criterias[i].CriteriaDirection == CriteriaDirectionType.Maximization)
                {
                    result.Add(GetMaxValue(GetValuesRow(i)));
                }
                else
                {
                    result.Add(GetMinValue(GetValuesRow(i)));
                }
            }
            return result;
        }

        double [] GetValuesRow(int index)
        {
            double[] result = new double[_alternatives.Alternatives.Count];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                result[i] = _alternatives.Alternatives[i].Values[index].Value;
            }

            return result;
        }

        private double GetMaxValue(double [] values)
        {
            double result = values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                result = Math.Max(result, values[i]);
            }
            return result;
        }

        private double GetMinValue(double[] values)
        {
            double result = values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                result = Math.Min(result, values[i]);
            }
            return result;
        }
    }
}
