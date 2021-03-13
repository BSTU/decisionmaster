using System;
using System.Collections.Generic;
using System.Linq;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;
using DecisionMaster.AlgorithmsLibrary.Algorithms.REGIME;
using DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE;
using DecisionMaster.AlgorithmsLibrary.Algorithms.WASPAS;
using DecisionMaster.AlgorithmsLibrary.Algorithms.TAXONOMY;
using DecisionMaster.AlgorithmsLibrary.Algorithms.ELECTRE;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DecisionMaster.UserInterface.Controllers
{
    class SolutionController
    {
        public CriteriasController _criterias = new CriteriasController();
        public AlternativesController _alternatives = new AlternativesController();
        public SpecialParametersEnum WASPASConfiguration = SpecialParametersEnum.None;
        public double WASPASLambda { get; set; }

        public double ELECTREAlpha { get; set; }
        public double ELECTREBeta { get; set; }

        public Dictionary<MethodsEnum, bool> _methods = new Dictionary<MethodsEnum, bool>
        {
            {MethodsEnum.SMART, false },
            {MethodsEnum.REGIME, false },
            {MethodsEnum.PROMETHEE, false },
            {MethodsEnum.WASPAS, false },
            {MethodsEnum.TAXONOMY, false },
            {MethodsEnum.ELECTRE, false }
        };

        public SolutionController()
        {

        }

        public List<string> GetAlternativesTitles()
        {
            List<string> result = new List<string>
            {
                "Method's title"
            };
            foreach (AlternativeController alternative in _alternatives.Alternatives)
            {
                result.Add(alternative.Title);
            }
            return result;
        }

        private List <ICriteria> GetResultCriterias()
        {
            List<ICriteria> result = new List<ICriteria>();
            foreach(KeyValuePair <MethodsEnum, bool> method in _methods)
            {
                if (method.Value == true)
                {
                    result.Add(new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Minimization });
                }
            }
            return result;
        }

        private List<double> GetResultRanks()
        {
            List<double> result = new List<double>();
            foreach (KeyValuePair<MethodsEnum, bool> method in _methods)
            {
                if (method.Value == true)
                {
                    result.Add(1);
                }
            }
            return result;
        }

        private List <AlternativeBase> PrepareResultAlternatives()
        {
            List<AlternativeBase> result = new List <AlternativeBase>();
           for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
            {
                result.Add(new AlternativeBase { Values = new List<IAlternativeValue>() });
            }

            return result;
        }

        private AlternativesBase PrepareResultAlternativesBase()
        {
            return new AlternativesBase
            {
                Alternatives = PrepareResultAlternatives(),
                Criterias = GetResultCriterias()
            };
        }

        public List<DataGridViewRow> GetSolutionsAsDataGrid()
        {
            REGIMEDecisionProvider resultProvider = new REGIMEDecisionProvider();
            DecisionConfigurationBase resultBase = new DecisionConfigurationBase
            {
                CriteriaRanks = GetResultRanks()
            };
            AlternativesBase resultAlternatives = PrepareResultAlternativesBase();

            List<DataGridViewRow> result = new List<DataGridViewRow>();
            if (_methods[MethodsEnum.SMART] == true)
            {               
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "SMART";
                NewRow.Cells.Add(TitleCell);

                List<int> localRanks = GetRanksSMART();

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach(int value in localRanks)
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

                List<int> localRanks = GetRanksREGIME();

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach (int value in localRanks)
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

                List<int> localRanks = GetRanksPROMETHEE();

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach (int value in localRanks)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }
            if (_methods[MethodsEnum.WASPAS] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "WASPAS";
                NewRow.Cells.Add(TitleCell);

                List<int> localRanks = GetRanksWASPAS();

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach (int value in localRanks)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }
            if (_methods[MethodsEnum.TAXONOMY] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "TAXONOMY";
                NewRow.Cells.Add(TitleCell);

                List<int> localRanks = GetRanksTAXONOMY();

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach (int value in localRanks)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }
            if (_methods[MethodsEnum.ELECTRE] == true)
            {
                DataGridViewRow NewRow = new DataGridViewRow();
                DataGridViewTextBoxCell TitleCell = new DataGridViewTextBoxCell();
                TitleCell.Value = "ELECTRE";
                NewRow.Cells.Add(TitleCell);

                List<int> localRanks;
                try
                {
                    localRanks = GetRanksELECTRE();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    localRanks = new List<int>();
                    for (int i = 0; i < _alternatives.Alternatives.Count; ++i)
                    {
                        localRanks.Add(1);
                    }
                }

                for (int i = 0; i < localRanks.Count; ++i)
                {
                    resultAlternatives.Alternatives[i].Values.Add(new AlternativeValueBase(localRanks[i]));
                }

                foreach (int value in localRanks)
                {
                    DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                    newCell.Value = value.ToString();
                    NewRow.Cells.Add(newCell);
                }
                result.Add(NewRow);
            }

            resultProvider.Init(resultBase);
            List<int> ranks = resultProvider.Solve(resultAlternatives).Ranks;

            DataGridViewRow NewRowResult = new DataGridViewRow();
            DataGridViewTextBoxCell TitleCellResult = new DataGridViewTextBoxCell();
            TitleCellResult.Value = "RESULT";
            NewRowResult.Cells.Add(TitleCellResult);
            foreach (int value in ranks)
            {
                DataGridViewTextBoxCell newCell = new DataGridViewTextBoxCell();
                newCell.Value = value.ToString();
                NewRowResult.Cells.Add(newCell);
            }
            result.Add(NewRowResult);
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

        public List<int> GetRanksELECTRE()
        {
            ELECTREDecisionProvider provider = new ELECTREDecisionProvider();
            ELECTREDecisionConfiguration config = new ELECTREDecisionConfiguration
            {
                CriteriaRanks = _criterias.GetNormalizedWeight(),
                Parameters = _criterias.GetELECTREParameters(),
                alpha = ELECTREAlpha,
                beta = ELECTREBeta
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.REGIME);

            List<int> result = provider.Solve(alternatives).Ranks;
            int MinResult = result.Min();
            for (int i = 0; i < result.Count; ++i)
            {
                result[i] -= (MinResult-1);
            }
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

        public List<int> GetRanksWASPAS()
        {
            WASPASDecisionProvider provider = new WASPASDecisionProvider();
            WASPASDecisionConfiguration config = new WASPASDecisionConfiguration
            {
                CriteriaRanks = _criterias.GetNormalizedWeight(),
                Lambda = WASPASLambda
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.WASPAS);

            List<int> result = provider.Solve(alternatives).Ranks;
            return result;
        }

        public List<int> GetRanksTAXONOMY()
        {
            IDecisionProvider provider = new TAXONOMYDecisionProvider();
            IDecisionConfiguration config = new DecisionConfigurationBase
            {
                CriteriaRanks = _criterias.GetNormalizedWeight()
            };
            provider.Init(config);

            AlternativesBase alternatives = GetAlternativesBases(MethodsEnum.TAXONOMY);

            List<int> result = provider.Solve(alternatives).Ranks;
            return result;
        }

        private AlternativesBase GetAlternativesBases(MethodsEnum method)
        {
            List<ICriteria> criterias = (method == MethodsEnum.SMART ? _criterias.GetCriteriasAsSMART() : _criterias.GetCriterias());
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
        [JsonConverter(typeof(StringEnumConverter))]
        public enum MethodsEnum
        {
            SMART = 1,
            REGIME = 2,
            PROMETHEE = 3,
            WASPAS = 4,
            TAXONOMY = 5,
            ELECTRE = 6
        };
    }
}
