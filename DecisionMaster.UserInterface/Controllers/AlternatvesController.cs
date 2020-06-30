using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;

namespace DecisionMaster.UserInterface.Controllers
{

    public class AlternativeController
    {
        public AlternativeController()
        {
            Values = new List<double>();
        }
        public String Title { get; set; }
        public List<double> Values { get; set; }

        public List<IAlternativeValue> GetAlternativeValues(List <ICriteria> criterias, IQualitativeCriteria converter)
        {
            List<IAlternativeValue> result = new List<IAlternativeValue>();

            for (int i = 0; i < criterias.Count; ++i)
            {
                if (criterias[i] is QualitativeCriteriaBase)
                {
                    result.Add(new QualitativeAlternativeValue(
                        (QualitativeCriteriaEnum)Values[i],
                        converter));
                }
                else
                {
                    result.Add(new AlternativeValueBase(Values[i]));
                }
            }

            return result;
        }
    }
    public class AlternativesController
    {
        public AlternativesController()
        {
            Alternatives = new List<AlternativeController>();
        }
        public List<AlternativeController> Alternatives { get; set; }

        public List<AlternativeBase> GetAlternativeBases(List <ICriteria> criterias, IQualitativeCriteria converter)
        {
            List<AlternativeBase> result = new List<AlternativeBase>();
            
            for (int i = 0; i < Alternatives.Count; ++i)
            {
                result.Add(new AlternativeBase
                {
                    Values = Alternatives[i].GetAlternativeValues(criterias, converter)
                }) ;
            }

            return result;
        }
    }

    
}
