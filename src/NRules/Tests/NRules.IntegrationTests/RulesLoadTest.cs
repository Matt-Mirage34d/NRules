﻿using System.Linq;
using System.Reflection;
using NRules.Fluent;
using NRules.IntegrationTests.TestRules;
using NRules.RuleModel;
using NUnit.Framework;

namespace NRules.IntegrationTests
{
    [TestFixture]
    public class RulesLoadTest
    {
        [Test]
        public void Load_AssemblyWithoutRules_Empty()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(typeof (string).Assembly));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual(0, ruleSet.Rules.Count());
        }

        [Test]
        public void Load_InvalidTypes_Empty()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(typeof (string)));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual(0, ruleSet.Rules.Count());
        }

        [Test]
        public void Load_AssemblyWithRulesToNamedRuleSet_RuleSetNameMatches()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(ThisAssembly).To("Test"));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual("Test", ruleSet.Name);
        }

        [Test]
        public void Load_AssemblyWithRulesToDefaultRuleSet_DefaultRuleSetName()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(ThisAssembly));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual("default", ruleSet.Name);
        }

        [Test]
        public void Load_FilterRuleByName_MatchingRule()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(ThisAssembly).Where(r => r.Name.Contains("PriorityLowRule")));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual(1, ruleSet.Rules.Count());
            Assert.AreEqual(typeof (PriorityLowRule).FullName, ruleSet.Rules.First().Name);
        }

        [Test]
        public void Load_FilterRuleByTag_MatchingRule()
        {
            //Arrange
            RuleRepository target = CreateTarget();

            //Act
            target.Load(x => x.From(ThisAssembly).Where(r => r.IsTagged("Test")));
            IRuleSet ruleSet = target.GetRuleSets().First();

            //Assert
            Assert.AreEqual(1, ruleSet.Rules.Count());
            Assert.AreEqual("Rule with metadata", ruleSet.Rules.First().Name);
        }

        private static Assembly ThisAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }

        public RuleRepository CreateTarget()
        {
            return new RuleRepository();
        }
    }
}