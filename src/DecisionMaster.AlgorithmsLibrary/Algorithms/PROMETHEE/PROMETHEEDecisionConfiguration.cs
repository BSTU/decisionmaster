using DecisionMaster.AlgorithmsLibrary.Interfaces;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE
{
    public class PROMETHEEDecisionConfiguration : IDecisionConfiguration
    {
        public List<double> CriteriaRanks { get; set; }
        public List<PreferenceFunction> PreferenceFunctions { get; set; }
    }


    public class PreferenceFunction
    {
        public PreferenceFunction()
        {

        }
        public PreferenceFunction(PreferenceFunctionEnum functionType, List<double> parameters)
        {
            int errorCode;
            switch (functionType)
            {
                case PreferenceFunctionEnum.UsualCriterion:
                    errorCode = parameters != null && parameters.Count > 0 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.QuasiCriterion:
                    errorCode = parameters == null || parameters.Count != 1 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.VShapeCriterion:
                    errorCode = parameters == null || parameters.Count != 1 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.LevelCriterion:
                    errorCode = parameters == null || parameters.Count != 2 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.LinearCriterion:
                    errorCode = parameters == null || parameters.Count != 2 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.GaussianCriterion:
                    errorCode = parameters == null || parameters.Count != 1 ? 0 : 1;
                    break;
                default:
                    throw new Exception("Invalid argument");
            };
            if (errorCode == 0)
            {
                throw new Exception("List of parameters doesn't match Preference Function's type");
            }
            else
            {
                _type = functionType;
                _parameters = parameters;
            }
        }
        public PreferenceFunctionEnum _type { get; set; }
        public List<double> _parameters { get; set; }

        public double GetValue(double value)
        {
            double result;
            switch (_type)
            {
                case PreferenceFunctionEnum.UsualCriterion:
                    result = value <= 0 ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.QuasiCriterion:
                    result = value <= _parameters[0] ? 0 : 1;
                    break;
                case PreferenceFunctionEnum.VShapeCriterion:
                    result = Math.Max(0, value <= _parameters[0] ? value / _parameters[0] : 1);
                    break;
                case PreferenceFunctionEnum.LevelCriterion:
                    result = value <= _parameters[0] ? 0 : (value <= _parameters[0] + _parameters[1] ? 0.5 : 1);
                    break;
                case PreferenceFunctionEnum.LinearCriterion:
                    result = value <= _parameters[0] ? 0 : (value <= _parameters[0] + _parameters[1] ? (value - _parameters[0]) / _parameters[1] : 1);
                    break;
                case PreferenceFunctionEnum.GaussianCriterion:
                    result = value <= 0 ? 0 : 1 - Math.Exp(-((value * value) / (2 * (_parameters[0] * _parameters[0]))));
                    break;
                default:
                    throw new Exception("Invalid argument");
            }
            return result;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
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
