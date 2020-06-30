using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;
using DecisionMaster.AlgorithmsLibrary.Algorithms.REGIME;
using DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE;
using DecisionMaster.AlgorithmsLibrary.Algorithms.WASPAS;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace DecisionMaster.AlgorithmsLibrary.Tests.Unit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSMARTDecision()
        {
            SMARTDecisionProvider provider = new SMARTDecisionProvider();
            SMARTDecisionConfiguration configuration = new SMARTDecisionConfiguration
            {
                Epsilon = 1,
                CriteriaRanks = new List<double> { 9, 5, 7, 6 }
            };
            provider.Init(configuration);
            var alternatives = new AlternativesBase
            {
                Criterias = new List<ICriteria>
                {
                    new CriteriaBase{MinValue = 20000, MaxValue = 40000, CriteriaDirection = CriteriaDirectionType.Minimization},//Consumer price
                    new CriteriaBase{MinValue = 140, MaxValue = 220, CriteriaDirection = CriteriaDirectionType.Maximization},//Maximum speed
                    new CriteriaBase{MinValue = 8, MaxValue = 20, CriteriaDirection = CriteriaDirectionType.Minimization},//Acceleration 0–100
                    new CriteriaBase{MinValue = 200, MaxValue = 2000, CriteriaDirection = CriteriaDirectionType.Maximization}//Trunk volume of car
                },
                Alternatives = new List<AlternativeBase>
                {
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(25000),
                            new AlternativeValueBase(153),
                            new AlternativeValueBase(15.3),
                            new AlternativeValueBase(250)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(33000),
                            new AlternativeValueBase(177),
                            new AlternativeValueBase(12.3),
                            new AlternativeValueBase(380)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(40000),
                            new AlternativeValueBase(199),
                            new AlternativeValueBase(11.1),
                            new AlternativeValueBase(480)
                        }
                    }
                }
            };

            var result = provider.Solve(alternatives);
            Assert.IsTrue(result.Ranks.Count == 3);
            Assert.IsTrue(result.Ranks[0] == 3);
            Assert.IsTrue(result.Ranks[1] == 2);
            Assert.IsTrue(result.Ranks[2] == 1);
        }

        [Test]
        public void TestREGIMEDecision()
        {
            REGIMEDecisionProvider provider = new REGIMEDecisionProvider();
            DecisionConfigurationBase configuration = new DecisionConfigurationBase
            {
                CriteriaRanks = new List<double> { 0.1, 0.175, 0.25, 0.35, 0.125 }
            };

            provider.Init(configuration);

            var alternatives = new AlternativesBase
            {
                Criterias = new List<ICriteria>
                {
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization},//cost
                    new QualitativeCriteriaBase{CriteriaDirection = CriteriaDirectionType.Maximization}, //strength
                    new QualitativeCriteriaBase{CriteriaDirection = CriteriaDirectionType.Maximization }, //national reputation
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Maximization}, // capacity
                    new QualitativeCriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization}, // work hardness
                },
                Alternatives = new List<AlternativeBase>
                {
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(3),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.Good, new QualitativeCriteriaBase()),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.FairlyWeak, new QualitativeCriteriaBase()),
                            new AlternativeValueBase(24000),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.FairlyWeak, new QualitativeCriteriaBase())
                        }                       
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(1.2),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.Medium, new QualitativeCriteriaBase()),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.Good, new QualitativeCriteriaBase()),
                            new AlternativeValueBase(25000),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.FairlyWeak, new QualitativeCriteriaBase())
                        }
                    },
                     new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(1.5),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.FairlyWeak, new QualitativeCriteriaBase()),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.Excellent, new QualitativeCriteriaBase()),
                            new AlternativeValueBase(32000),
                            new QualitativeAlternativeValue(QualitativeCriteriaEnum.Excellent, new QualitativeCriteriaBase())
                        }
                     }
                }
            };
            var result = provider.Solve(alternatives);
            Assert.IsTrue(result.Ranks.Count == 3);
            Assert.IsTrue(result.Ranks[0] == 3);
            Assert.IsTrue(result.Ranks[1] == 2);
            Assert.IsTrue(result.Ranks[2] == 1);
        }

        [Test]
        public void TestPROMETHEEDecision()
        {
            PROMETHEEDecisionProvider provider = new PROMETHEEDecisionProvider();
            PROMETHEEDecisionConfiguration config = new PROMETHEEDecisionConfiguration
            {
                CriteriaRanks = new List<double> { 1.0 / 6, 1.0 / 6, 1.0 / 6, 1.0 / 6, 1.0 / 6, 1.0 / 6 },
                PreferenceFunctions = new List<PreferenceFunction>
                {
                    new PreferenceFunction(
                        PreferenceFunctionEnum.QuasiCriterion,
                        new List<double> {10 }
                        ),
                    new PreferenceFunction(
                        PreferenceFunctionEnum.VShapeCriterion,
                        new List<double> {30}
                        ),
                    new PreferenceFunction(
                        PreferenceFunctionEnum.LinearCriterion,
                        new List<double> {50, 450}
                        ),
                    new PreferenceFunction(
                        PreferenceFunctionEnum.LevelCriterion,
                        new List<double> {10,50}
                        ),
                    new PreferenceFunction(
                        PreferenceFunctionEnum.UsualCriterion,
                        new List<double>()
                        ),
                    new PreferenceFunction(
                        PreferenceFunctionEnum.GaussianCriterion,
                        new List<double>{5 }
                        )
                }
            };

            provider.Init(config);

            var Alternatives = new AlternativesBase
            {
                Criterias = new List<ICriteria>
                {
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization},
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Maximization},
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization},
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization},
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Minimization},
                    new CriteriaBase{CriteriaDirection = CriteriaDirectionType.Maximization}
                },
                Alternatives = new List<AlternativeBase>
                {
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(80),
                            new AlternativeValueBase(90),
                            new AlternativeValueBase(600),
                            new AlternativeValueBase(54),
                            new AlternativeValueBase(8),
                            new AlternativeValueBase(5)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(65),
                            new AlternativeValueBase(58),
                            new AlternativeValueBase(200),
                            new AlternativeValueBase(97),
                            new AlternativeValueBase(1),
                            new AlternativeValueBase(1)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(83),
                            new AlternativeValueBase(60),
                            new AlternativeValueBase(400),
                            new AlternativeValueBase(72),
                            new AlternativeValueBase(4),
                            new AlternativeValueBase(7)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(40),
                            new AlternativeValueBase(80),
                            new AlternativeValueBase(1000),
                            new AlternativeValueBase(75),
                            new AlternativeValueBase(7),
                            new AlternativeValueBase(10)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(52),
                            new AlternativeValueBase(72),
                            new AlternativeValueBase(600),
                            new AlternativeValueBase(20),
                            new AlternativeValueBase(3),
                            new AlternativeValueBase(8)
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(94),
                            new AlternativeValueBase(96),
                            new AlternativeValueBase(700),
                            new AlternativeValueBase(36),
                            new AlternativeValueBase(5),
                            new AlternativeValueBase(6)
                        }
                    }
                }
            };
            var result = provider.Solve(Alternatives);
            Assert.IsTrue(result.Ranks.Count == 6);
            Assert.IsTrue(result.Ranks[0] == 6);
            Assert.IsTrue(result.Ranks[1] == 2);
            Assert.IsTrue(result.Ranks[2] == 5);
            Assert.IsTrue(result.Ranks[3] == 3);
            Assert.IsTrue(result.Ranks[4] == 1);
            Assert.IsTrue(result.Ranks[5] == 4);
        }

        [Test]
        public void TestWASPASDecision()
        {
            WASPASDecisionProvider provider = new WASPASDecisionProvider();
            WASPASDecisionConfiguration config = new WASPASDecisionConfiguration
            {
                Lambda = 0.2,
                CriteriaRanks = new List<double>
                {
                    0.331, 0.181, 0.369, 0.072, 0.047
                }
            };

            provider.Init(config);

            var alternatives = new AlternativesBase
            {
                Criterias = new List<ICriteria>
                {
                    new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Minimization },
                    new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Minimization },
                    new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Maximization },
                    new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Minimization },
                    new CriteriaBase { CriteriaDirection = CriteriaDirectionType.Maximization },
                },
                Alternatives = new List<AlternativeBase>
                {
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(0.035),
                            new AlternativeValueBase(847),
                            new AlternativeValueBase(0.335),
                            new AlternativeValueBase(1.760),
                            new AlternativeValueBase(0.590),
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(0.027),
                            new AlternativeValueBase(834),
                            new AlternativeValueBase(0.335),
                            new AlternativeValueBase(1.680),
                            new AlternativeValueBase(0.665),
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(0.037),
                            new AlternativeValueBase(808),
                            new AlternativeValueBase(0.590),
                            new AlternativeValueBase(2.400),
                            new AlternativeValueBase(0.500),
                        }
                    },
                    new AlternativeBase
                    {
                        Values = new List<IAlternativeValue>
                        {
                            new AlternativeValueBase(0.028),
                            new AlternativeValueBase(821),
                            new AlternativeValueBase(0.500),
                            new AlternativeValueBase(1.590),
                            new AlternativeValueBase(0.410),
                        }
                    }
                }
            };

            var result = provider.Solve(alternatives);
            Assert.IsTrue(result.Ranks.Count == 4);
            Assert.IsTrue(result.Ranks[0] == 4);
            Assert.IsTrue(result.Ranks[1] == 3);
            Assert.IsTrue(result.Ranks[2] == 2);
            Assert.IsTrue(result.Ranks[3] == 1);
        }
        
    }
}