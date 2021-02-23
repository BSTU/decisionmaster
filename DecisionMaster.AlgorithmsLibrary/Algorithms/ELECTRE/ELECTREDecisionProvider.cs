using System;
using System.Collections.Generic;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.ELECTRE
{
    public class ELECTREDecisionProvider : IDecisionProvider
    {
        ELECTREDecisionConfiguration _configuration;
        AlternativesBase _alternatives;
        public void Init(IDecisionConfiguration config)
        {
            if (config is ELECTREDecisionConfiguration)
            {
                _configuration = (ELECTREDecisionConfiguration)config;
            }
            else
            {
                throw new Exception("Invalid Configuration");
            }
        }

        public DecisionResultBase Solve(AlternativesBase alternatives)
        {
            _alternatives = alternatives;

            double[,] normalize = GetNormalizedMatrix();
            List<double>[,] discordanceSets = GetDiscordances(normalize);
            double[,] concordances = GetConcordances(normalize);

            double[,] nonDiscordance = GetNonDiscordanceIndices(discordanceSets, concordances);
            double[,] generalPreference = GetGeneralPreferences(concordances, nonDiscordance);
            int[,] rangeMatrix = GetResultMatrix(generalPreference);
            int[,] finalRange = GetFinalRanking(rangeMatrix);

            DecisionResultBase result = new DecisionResultBase();
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                int rank = _alternatives.Alternatives.Count;
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    rank -= finalRange[i, j];
                }
                result.Ranks.Add(rank);
            }
            return result;
        }

        private int[,] GetResultMatrix(double[,] matrix)
        {
            int[,] result = new int[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];

            double max = matrix[0, 0];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    max = Math.Max(max, matrix[i, j]);
                }
            }

            double S = _configuration.alpha + _configuration.beta * max;
            double edge = max - S;

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        result[i, j] = (matrix[i, j] >= edge ? 1 : 0);
                    }
                }
            }

            return result;
        }

        private int[,] GetFinalRanking(int[,] matrix)
        {
            int[,] result = new int[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                Queue<int> Q = new Queue<int>();
                int[] used = new int[_alternatives.Alternatives.Count];
                Q.Enqueue(i);
                used[i] = 1;
                while (Q.Count > 0)
                {
                    int current = Q.Dequeue();
                    for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                    {
                        if (matrix[current, j] == 1)
                        {
                            if (used[j] == 0)
                            {
                                result[i, j] = 1;
                                used[j] = 1;
                                Q.Enqueue(j);
                            }
                            else
                            {
                                if (j == i)
                                {
                                    throw new Exception("Invalid ELECTRE input data for full-ranking");
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private double[,] GetGeneralPreferences(double[,] concordances, double[,] nonDiscordances)
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        result[i, j] = concordances[i, j] * nonDiscordances[i, j];
                    }
                }
            }
            return result;
        }

        private double[,] GetNonDiscordanceIndices(List<double>[,] discordanceSets, double[,] concordances)
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        result[i, j] = GetNonDiscordanceIndex(concordances[i, j], discordanceSets[i, j]);
                    }
                }
            }

            return result;
        }

        private double GetNonDiscordanceIndex(double concordance, List<double> discordances)
        {
            double result = 1;
            for (int i = 0; i < _configuration.CriteriaRanks.Count; ++i)
            {
                if (discordances[i] > concordance)
                {
                    result *= (1 - discordances[i]) / (1 - concordance);
                }
            }
            return result;
        }

        private double[,] GetConcordances(double[,] matrix)
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        result[i, j] = GetConcordanceIndex(matrix, i, j);
                    }
                }
            }
            return result;
        }

        private double GetConcordanceIndex(double[,] matrix, int i, int k)
        {
            double result = 0;

            for (int j = 0; j < _alternatives.Criterias.Count; ++j)
            {
                double diff = matrix[i, j] - matrix[k, j];
                double value = 0;
                if (diff >= _configuration.Parameters[j].p)
                {
                    value = 1;
                }
                else if (diff > _configuration.Parameters[j].q)
                {
                    value = diff / _configuration.Parameters[j].p;
                }
                result += value * _configuration.CriteriaRanks[j];
            }

            return result;
        }

        private List<double>[,] GetDiscordances(double[,] matrix)
        {
            List<double>[,] result = new List<double>[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    if (i != j)
                    {
                        result[i, j] = GetDiscordanceSet(matrix, i, j);
                    }
                }
            }

            return result;
        }

        private List<double> GetDiscordanceSet(double[,] matrix, int i, int k)
        {
            List<double> result = new List<double>();

            for (int j = 0; j < _alternatives.Criterias.Count; ++j)
            {
                double diff = matrix[k, j] - matrix[i, j];
                if (diff <= _configuration.Parameters[j].q)
                {
                    result.Add(0);
                }
                else if (diff >= _configuration.Parameters[j].v)
                {
                    result.Add(1);
                }
                else
                {
                    result.Add(diff / _configuration.Parameters[j].v);
                }
            }

            return result;
        }


        private double[,] GetNormalizedMatrix()
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Criterias.Count];
            double[] denominators = GetNormalizeDenominators();

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Criterias.Count; ++j)
                {
                    if (_alternatives.Criterias[j] is CriteriaBase && _alternatives.Criterias[j].CriteriaDirection == CriteriaDirectionType.Minimization)
                    {
                        result[i, j] = -(_alternatives.Alternatives[i].Values[j].Value / denominators[j]);
                    }
                    else
                    {
                        result[i, j] = _alternatives.Alternatives[i].Values[j].Value / denominators[j];
                    }
                    result[i, j] *= _configuration.CriteriaRanks[j];
                }
            }

            return result;
        }

        private double[] GetNormalizeDenominators()
        {
            double[] result = new double[_alternatives.Criterias.Count];
            for (int i = 0; i < _alternatives.Criterias.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    result[i] += Math.Pow(_alternatives.Alternatives[j].Values[i].Value, 2);
                }
                result[i] = Math.Sqrt(result[i]);
            }
            return result;
        }
    }
}
