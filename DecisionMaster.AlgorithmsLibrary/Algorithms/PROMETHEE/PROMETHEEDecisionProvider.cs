using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using System;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE
{
    public class PROMETHEEDecisionProvider :IDecisionProvider
    {
        private PROMETHEEDecisionConfiguration _configuration;
        private AlternativesBase _alternatives;

        public void Init(IDecisionConfiguration configuration)
        {
            if (configuration is PROMETHEEDecisionConfiguration SMARTDecisionConfiguration)
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
            double epsilon = 1e-3;// epsilon for double comparison
            _alternatives = alternatives;

            var PreferenceIndex = ComputePreferenceMatrix();
            double[,] Flows = ComputeIOFlows(PreferenceIndex, _alternatives.Alternatives.Count);

            double [] NetFlows = ComputeNetFlows(Flows, _alternatives.Alternatives.Count);

            List<int> Ranks = new List<int>();

            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                int Rank = 1;
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {                   
                    if (i != j && NetFlows[i] - NetFlows[j] < epsilon)
                    {
                        ++Rank;
                    }
                }
                Ranks.Add(Rank);
            }

            var Result = new DecisionResultBase();
            Result.Ranks = Ranks;

            return Result;
        }

        double PreferenceIndex(AlternativeBase lhs, AlternativeBase rhs)
        {
            double result = 0;
            for (int i = 0; i < lhs.Values.Count; ++i)
            {
                if (_alternatives.Criterias[i] is CriteriaBase)
                {
                    result +=
                        _configuration.PreferenceFunctions[i].GetValue(
                            _alternatives.Criterias[i].CriteriaDirection == CriteriaDirectionType.Maximization ? lhs.Values[i].Value - rhs.Values[i].Value : rhs.Values[i].Value - lhs.Values[i].Value
                            ) * _configuration.CriteriaRanks[i];
                }
                else
                {
                    result += _configuration.PreferenceFunctions[i].GetValue(lhs.Values[i].Value - rhs.Values[i].Value) * _configuration.CriteriaRanks[i];
                }
            }

            return result;
        }


        double[,] ComputePreferenceMatrix()
        {
            double[,] result = new double[_alternatives.Alternatives.Count, _alternatives.Alternatives.Count];
            for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                for (int j = 0; j < _alternatives.Alternatives.Count; ++j)
                {
                    result[i, j] = PreferenceIndex(
                        _alternatives.Alternatives[i],
                        _alternatives.Alternatives[j]
                        );
                }
            }
            return result;
        }

        double[,] ComputeIOFlows(double[,] PreferenceIndex, int MatrixSize)
        {
            double[,] result = new double[MatrixSize, 2];

            for (int i = 0; i < MatrixSize; ++i)
            {
                result[i, 0] = 0;
                result[i, 0] = 0;
                for (int j = 0; j < MatrixSize; ++j)
                {
                    if (i != j)
                    {
                        result[i, 0] += PreferenceIndex[i, j];
                        result[i, 1] += PreferenceIndex[j, i];
                    }
                }
                result[i, 0] /= (1.0 / (MatrixSize - 1.0));
                result[i, 1] /= (1.0 / (MatrixSize - 1.0));
            }

            return result;
        }

        double[] ComputeNetFlows(double[,] Flows, int Size)
        {
            double[] result = new double[Size];
            for (int i = 0; i < Size; ++i)
            {
                result[i] = Flows[i, 0] - Flows[i, 1];
            }
            return result;
        }

    }
}
