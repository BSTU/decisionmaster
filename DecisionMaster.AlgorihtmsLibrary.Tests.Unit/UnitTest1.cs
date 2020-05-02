using DecisionMaster.AlgorihtmsLibrary.Algoritms.SMART;
using DecisionMaster.AlgorihtmsLibrary.Interfaces;
using DecisionMaster.AlgorihtmsLibrary.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace DecisionMaster.AlgorihtmsLibrary.Tests.Unit
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
                Criterias = new List<CriteriaBase>
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
    }
}