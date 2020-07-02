using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Algorithms;
using DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE;
using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;
using DecisionMaster.AlgorithmsLibrary.Algorithms.ELECTRE;

namespace DecisionMaster.UserInterface.Controllers
{
    public class CriteriaController
    {
        public String Title { get; set; }
        public double Weight { get; set; }
        public CriteriaTypeEnum CriteriaType { get; set; }
        public CriteriaDirectionType CriteriaDirection { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        public SpecialParametersEnum PROMETHEEConfiguration { get; set; }
        public PreferenceFunction PreferenceFunction { get; set; }

        public SpecialParametersEnum ELECTREConfiguration { get; set; }
        public ELECTREParameters ELECTREspecialParameters { get; set; }
    }

    public class CriteriasController
    {
        public CriteriasController()
        {
            Criterias = new List<CriteriaController>();
        }
        public List<CriteriaController> Criterias { get; set; }

        public List<ELECTREParameters> GetELECTREParameters()
        {
            List<ELECTREParameters> result = new List<ELECTREParameters>();
            foreach (CriteriaController criteria in Criterias)
            {
                result.Add(criteria.ELECTREspecialParameters);
            }

            return result;
        }

        public List<PreferenceFunction> GetPreferenceFunctions()
        {
            List<PreferenceFunction> result = new List<PreferenceFunction>();

            foreach(CriteriaController criteria in Criterias)
            {
                result.Add(criteria.PreferenceFunction);
            }

            return result;
        }

        public List<double> GetWeights()
        {
            List<double> result = new List<double>();

            foreach(CriteriaController criteria in Criterias)
            {
                result.Add(criteria.Weight);
            }

            return result;
        }

        public List<double> GetNormalizedWeight()
        {
            List<double> result = new List<double>();
            double sum = 0;

            foreach (CriteriaController criteria in Criterias)
            {
                result.Add(criteria.Weight);
                sum += criteria.Weight;
            }
            for (int i = 0; i < result.Count; ++i)
            {
                result[i] /= sum;
            }

            return result;
        }

        public List<ICriteria> GetCriteriasAsSMART()
        {
            List<ICriteria> result = new List<ICriteria>();
            foreach(CriteriaController criteria in Criterias)
            {
                if (criteria.CriteriaType == CriteriaTypeEnum.Qualitative)
                {
                    result.Add(
                        new SMARTQualitativeCriteria
                        {
                            MinValue = criteria.MinValue,
                            MaxValue = criteria.MaxValue,
                            CriteriaDirection = criteria.CriteriaDirection
                        }
                        );
                }
                else
                {
                    result.Add(
                        new CriteriaBase
                        {
                            MinValue = criteria.MinValue,
                            MaxValue = criteria.MaxValue,
                            CriteriaDirection = criteria.CriteriaDirection
                        }
                        );
                }
            }

            return result;
        }

        public List<ICriteria> GetCriterias()
        {
            List<ICriteria> result = new List<ICriteria>();
            foreach (CriteriaController criteria in Criterias)
            {
                if (criteria.CriteriaType == CriteriaTypeEnum.Qualitative)
                {
                    result.Add(
                        new QualitativeCriteriaBase
                        {
                            MinValue = criteria.MinValue,
                            MaxValue = criteria.MaxValue,
                            CriteriaDirection = criteria.CriteriaDirection
                        }
                        );
                }
                else
                {
                    result.Add(
                        new CriteriaBase
                        {
                            MinValue = criteria.MinValue,
                            MaxValue = criteria.MaxValue,
                            CriteriaDirection = criteria.CriteriaDirection
                        }
                        );
                }
            }

            return result;
        }
    }

    public enum SpecialParametersEnum
    {
        None = 0,
        Default = 1,
        Manual = 2
    }

    public enum CriteriaTypeEnum
    {
        Quantitative = 0,
        Qualitative = 1
    }
}
