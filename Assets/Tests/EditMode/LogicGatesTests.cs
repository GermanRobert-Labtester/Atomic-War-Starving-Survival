using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #799 — logic gates: IF/THEN rules over power modules, capture/restore, save.
    /// </summary>
    [TestFixture]
    public class LogicGatesTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void AddRemoveRule_TracksCountAndEvents()
        {
            var gates = new System_LogicGates();
            Assert.AreEqual(0, gates.GetRuleCount());

            string added = null;
            string removed = null;
            gates.OnRuleAdded += id => added = id;
            gates.OnRuleRemoved += id => removed = id;

            gates.AddRule("shed_grow", new LogicRule
            {
                conditionModule = "generation",
                conditionOperator = "lt",
                conditionValue = 50f,
                actionModule = "grow_light",
                actionCommand = System_LogicGates.CmdDisable
            });

            Assert.AreEqual(1, gates.GetRuleCount());
            Assert.AreEqual("shed_grow", added);
            Assert.IsNotNull(gates.FindRule("shed_grow"));

            gates.RemoveRule("shed_grow");
            Assert.AreEqual(0, gates.GetRuleCount());
            Assert.AreEqual("shed_grow", removed);
            Assert.IsNull(gates.FindRule("shed_grow"));
        }

        [Test]
        public void EvaluateRules_TriggersWhenConditionMet()
        {
            var gates = new System_LogicGates();
            gates.AddRule("low_gen_off_grow", new LogicRule
            {
                conditionModule = "generation",
                conditionOperator = "lt",
                conditionValue = 40f,
                actionModule = "grow_light",
                actionCommand = System_LogicGates.CmdDisable
            });

            int hits = 0;
            gates.OnRuleTriggered += _ => hits++;

            var miss = gates.EvaluateRules(new Dictionary<string, float> { { "generation", 80f } });
            Assert.AreEqual(0, miss.Count);
            Assert.AreEqual(0, hits);

            var hit = gates.EvaluateRules(new Dictionary<string, float> { { "generation", 10f } });
            Assert.AreEqual(1, hit.Count);
            Assert.AreEqual("low_gen_off_grow", hit[0]);
            Assert.AreEqual(1, hits);
        }

        [Test]
        public void HostPattern_PowerNetwork_RuleDisablesConsumer()
        {
            // Mirrors GameBootstrap.TickLogicGates host pattern.
            var power = PowerNetwork.CreateDefault(dieselFuel: 40f);
            power.SetRequested("grow_light", true);
            power.Rebalance();
            Assert.IsTrue(power.GetConsumer("grow_light")?.IsRequested ?? false);

            var gates = new System_LogicGates();
            // Always true: blackout == 0 under default grid with fuel.
            gates.AddRule("auto_shed_grow", new LogicRule
            {
                conditionModule = "generation",
                conditionOperator = "gt",
                conditionValue = 0f,
                actionModule = "grow_light",
                actionCommand = System_LogicGates.CmdDisable
            });

            var states = new Dictionary<string, float>
            {
                ["generation"] = power.TotalGeneration,
                ["grow_light"] = power.IsModulePowered("grow_light") ? 1f : 0f
            };
            // Ensure generation is positive for the rule.
            if (power.TotalGeneration <= 0f)
                states["generation"] = 100f;

            var triggered = gates.EvaluateRules(states);
            Assert.Contains("auto_shed_grow", triggered);

            var rule = gates.FindRule("auto_shed_grow");
            Assert.IsNotNull(rule);
            // Host action apply
            if (rule.actionCommand == System_LogicGates.CmdDisable)
                power.SetRequested(rule.actionModule, false);

            Assert.IsFalse(power.GetConsumer("grow_light").IsRequested);
        }

        [Test]
        public void CaptureRestore_PreservesRules()
        {
            var a = new System_LogicGates();
            a.AddRule("r1", new LogicRule
            {
                conditionModule = "co_ppm",
                conditionOperator = "gte",
                conditionValue = 25f,
                actionModule = "diesel_generator",
                actionCommand = System_LogicGates.CmdSourceOff
            });
            a.AddRule("r2", new LogicRule
            {
                conditionModule = "water_purifier",
                conditionOperator = "eq",
                conditionValue = 0f,
                actionModule = "water_purifier",
                actionCommand = System_LogicGates.CmdEnable
            });

            var save = a.CaptureState();
            Assert.AreEqual("system_logic_gates", save.systemId);
            Assert.AreEqual(2, save.rules.Count);

            // Mutate after capture.
            a.RemoveRule("r1");
            Assert.AreEqual(2, save.rules.Count);

            var b = new System_LogicGates();
            b.RestoreState(save);
            Assert.AreEqual(2, b.GetRuleCount());
            Assert.IsNotNull(b.FindRule("r1"));
            Assert.AreEqual("co_ppm", b.FindRule("r1").conditionModule);
            Assert.AreEqual(25f, b.FindRule("r1").conditionValue, Eps);
            Assert.AreEqual(System_LogicGates.CmdSourceOff, b.FindRule("r1").actionCommand);

            b.RestoreState(null);
            Assert.AreEqual(0, b.GetRuleCount());
        }

        [Test]
        public void SaveSystemAdapter_LogicGatesSlot_RoundTrip()
        {
            string dir = SaveSystemTestFactory.TempDir("logic");
            try
            {
                var gatesA = new System_LogicGates();
                gatesA.AddRule("shed_radio", new LogicRule
                {
                    conditionModule = "blackout",
                    conditionOperator = "eq",
                    conditionValue = 1f,
                    actionModule = "radio",
                    actionCommand = System_LogicGates.CmdDisable
                });

                SaveSystem Make(System_LogicGates gates) =>
                    SaveSystemTestFactory.MakeSave(dir, ss => { ss.SetLogicGatesSystem(gates); });

                Assert.IsTrue(Make(gatesA).Save("logic_slot"));

                var gatesB = new System_LogicGates();
                Assert.IsTrue(Make(gatesB).Load("logic_slot"));

                Assert.AreEqual(1, gatesB.GetRuleCount());
                var rule = gatesB.FindRule("shed_radio");
                Assert.IsNotNull(rule);
                Assert.AreEqual("blackout", rule.conditionModule);
                Assert.AreEqual("radio", rule.actionModule);
                Assert.AreEqual(System_LogicGates.CmdDisable, rule.actionCommand);
                Assert.AreEqual("system_logic_gates", gatesB.SystemId);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
