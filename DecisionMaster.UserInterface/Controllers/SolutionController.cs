using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;
using DecisionMaster.AlgorithmsLibrary.Algorithms.REGIME;
using DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE;
using System.Windows.Forms;

namespace DecisionMaster.UserInterface.Controllers
{
    class SolutionController
    {
        public CriteriasController _criterias = new CriteriasController();
        public AlternativesController _alternatives = new AlternativesController();
        public Dictionary<MethodsEnum, bool> _methods = new Dictionary<MethodsEnum, bool>
        {
            {MethodsEnum.SMART, false },
            {MethodsEnum.REGIME, false },
            {MethodsEnum.PROMETHEE, false }
        };

        public List <String> GetAlternativesTitles()
        {
            List<String> result = new List<string>();
            result.Add("Method's title");
            foreach(AlternativeController alternative in _alternatives.Alternatives)
            {
                result.Add(alternative.Title);
            }
            return result;
        }

        public List<DataGridViewRow> GetSolutionsAsDataGrid()
        {
            List<DataGridViewRow> result = new List<DataGridViewRow>();
            if (_methods[MethodsEnum.SMART] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "SMART";
                NewRow.Cells.Add(TitleCell);

                foreach(int value in GetRanksSMART())
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }
            if (_methods[MethodsEnum.REGIME] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "REGIME";
                NewRow.Cells.Add(TitleCell);

                foreach (int value in GetRanksREGIME())
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }
            if (_methods[MethodsEnum.PROMETHEE] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "PROMETHEE";
                NewRow.Cells.Add(TitleCell);

                foreach (int value in GetRanksPROMETHEE())
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }

            return result;
        }

        public List<int> GetRanksSMART()
        {
            SMARTDecisionProvider provider = new SMARTDecisionProvider();
            SMARTDecisionConfiguration config = new SMARTDecisionConfiguration
            {
                CriteriaRanks = _criterias.GetWeights(),
                Epsilon = 1
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.SMART);

            List<int> result = provider.Solve(alternatives).Ranks;
            return result;
        }

        public List<int> GetRanksREGIME()
        {
            REGIMEDecisionProvider provider = new REGIMEDecisionProvider();
            DecisionConfigurationBase config = new DecisionConfigurationBase
            {
                CriteriaRanks = _criterias.GetNormalizedWeight()
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.REGIME);

            List<int> result = provider.Solve(alternatives).Ranks;
            return result;
        }

        public List<int> GetRanksPROMETHEE()
        {
            PROMETHEEDecisionProvider provider = new PROMETHEEDecisionProvider();
            PROMETHEEDecisionConfiguration config = new PROMETHEEDecisionConfiguration
            {
                CriteriaRanks = _criterias.GetNormalizedWeight(),
                PreferenceFunctions = _criterias.GetPreferenceFunctions()        
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.PROMETHEE);

            List<int> result = provider.Solve(alternatives).Ranks;
            return result;
        }

        public AlternativesBase GetAlternativesBases(MethodsEnum method)
        {
            List<ICriteria> criterias = _criterias.GetCriteriasAsSMART();
            List<AlternativeBase> alternatives = _alternatives.GetAlternativeBases(criterias, GetQualitativeConverter(method));

            return new AlternativesBase
            {
                Criterias = criterias,
                Alternatives = alternatives
            };
        }

        private IQualitativeCriteria GetQualitativeConverter(MethodsEnum method)
        {
            if (method == MethodsEnum.SMART)
            {
                return new SMARTQualitativeCriteria();
            }
            else
            {
                return new QualitativeCriteriaBase();
            }
        }

        public enum MethodsEnum
        {
            SMART = 1,
            REGIME = 2,
            PROMETHEE = 3
        };
    }
}
