using DecisionMaster.AlgorithmsLibrary.Interfaces;
using System.Collections.Generic;
using System;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE
{
    public class PROMETHEEDecisionConfiguration:IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public List<PreferenceFunction> CriteriaFunctions { get; set; }        
    }


    public class PreferenceFunction
    {
        public PreferenceFunction(PreferenceFunctionEnum FunctionType = PreferenceFunctionEnum.UsualCriterion,
            List<double> Parameters = null)
        {
            int ErrorCode = FunctionType switch
            {
                PreferenceFunctionEnum.UsualCriterion => (Parameters != null && Parameters.Count > 0 ? 0 : 1),
                PreferenceFunctionEnum.QuasiCriterion => (Parameters == null || Parameters.Count != 1 ? 0 : 1),
                PreferenceFunctionEnum.VShapeCriterion => (Parameters == null || Parameters.Count != 1 ? 0 : 1),
                PreferenceFunctionEnum.LevelCriterion => (Parameters == null || Parameters.Count != 2 ? 0 : 1),
                PreferenceFunctionEnum.LinearCriterion => (Parameters == null || Parameters.Count != 2 ? 0 : 1),
                PreferenceFunctionEnum.GaussianCriterion => (Parameters == null || Parameters.Count != 1 ? 0 : 1),
                _ => throw new Exception("Invalid argument")
            };
            if (ErrorCode == 0)
            {
                throw new Exception("List of parameters doesn't match Preference Function's type");
            }
            else
            {
                _type = FunctionType;
                _parameters = Parameters;
            }
        }
        public PreferenceFunctionEnum _type { get; set; }
        public List<double> _parameters { get; set; }

        public double GetValue(double value) => _type switch
        {
            PreferenceFunctionEnum.UsualCriterion => (value <= 0 ? 0 : 1),
            PreferenceFunctionEnum.QuasiCriterion => (value <= _parameters[0] ? 0 : 1),
            PreferenceFunctionEnum.VShapeCriterion => Math.Max(0,(value <= _parameters[0] ? value / _parameters[0] : 1)),
            PreferenceFunctionEnum.LevelCriterion => (value <= _parameters[0] ? 0 : (value <= _parameters[0] + _parameters[1] ? 0.5 : 1)),
            PreferenceFunctionEnum.LinearCriterion => (
            value <= _parameters[0] ? 0 : (value <= _parameters[0] + _parameters[1] ? (value - _parameters[0]) / _parameters[1] : 1)
            ),
            PreferenceFunctionEnum.GaussianCriterion => (value <= 0 ? 0 : 1 - Math.Exp(-((value* value) / (2 * (_parameters[0]* _parameters[0]))))),
            _ => throw new Exception("Invalid argument")
        };

    }


    public enum PreferenceFunctionEnum
    {
        UsualCriterion = 1,
        QuasiCriterion = 2,
        VShapeCriterion = 3,
        LevelCriterion = 4,
        LinearCriterion = 5,
        GaussianCriterion = 6
    }
}
