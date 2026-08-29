using System;
using System.Collections.Generic;
using Ashfall.Core.Lifecycle;
using Xunit;

namespace Ashfall.Core.Tests.Lifecycle
{
    public class SessionLifecycleRegistryTests
    {
        [Fact]
        public void Registration_ComputesTopologicalOrderCorrectly()
        {
            var reg = new SessionLifecycleRegistry();

            reg.Register(new DelegateSessionParticipant("combat", dependsOn: new[] { "expeditions" }));
            reg.Register(new DelegateSessionParticipant("expeditions", dependsOn: new[] { "survivors", "inventory" }));
            reg.Register(new DelegateSessionParticipant("survivors"));
            reg.Register(new DelegateSessionParticipant("inventory"));

            var topo = reg.GetTopologicalOrder();

            // Survivors & Inventory must come before Expeditions; Expeditions before Combat
            int idxSurvivors = ((List<string>)topo).IndexOf("survivors");
            int idxInventory = ((List<string>)topo).IndexOf("inventory");
            int idxExpeditions = ((List<string>)topo).IndexOf("expeditions");
            int idxCombat = ((List<string>)topo).IndexOf("combat");

            Assert.True(idxSurvivors < idxExpeditions);
            Assert.True(idxInventory < idxExpeditions);
            Assert.True(idxExpeditions < idxCombat);
        }

        [Fact]
        public void ResetAll_ExecutesInReverseTopologicalDependencyOrder()
        {
            var reg = new SessionLifecycleRegistry();
            var log = new List<string>();

            reg.Register(new DelegateSessionParticipant(
                "combat",
                dependsOn: new[] { "expeditions" },
                onReset: () => log.Add("reset:combat")));

            reg.Register(new DelegateSessionParticipant(
                "expeditions",
                dependsOn: new[] { "survivors" },
                onReset: () => log.Add("reset:expeditions")));

            reg.Register(new DelegateSessionParticipant(
                "survivors",
                onReset: () => log.Add("reset:survivors")));

            reg.ResetAll();

            Assert.Equal(new[] { "reset:combat", "reset:expeditions", "reset:survivors" }, log);
        }

        [Fact]
        public void ValidatePolicies_FlagsMissingDependencies()
        {
            var reg = new SessionLifecycleRegistry();
            reg.Register(new DelegateSessionParticipant("orphan", dependsOn: new[] { "non_existent_dep" }));

            var errors = reg.ValidatePolicies();
            Assert.NotEmpty(errors);
            Assert.Contains("Participant 'orphan' depends on unregistered participant 'non_existent_dep'.", errors[0]);
        }

        [Fact]
        public void RepeatedReset_SupportsMultipleNewGameCyclesInOneProcess()
        {
            var reg = new SessionLifecycleRegistry();
            int resetCountA = 0;
            int resetCountB = 0;

            reg.Register(new DelegateSessionParticipant("session_a", onReset: () => resetCountA++));
            reg.Register(new DelegateSessionParticipant("session_b", dependsOn: new[] { "session_a" }, onReset: () => resetCountB++));

            for (int cycle = 0; cycle < 10; cycle++)
            {
                reg.ResetAll();
            }

            Assert.Equal(10, resetCountA);
            Assert.Equal(10, resetCountB);
        }

        [Fact]
        public void PartiallyInitializedSessions_DoNotThrowOnReset()
        {
            var reg = new SessionLifecycleRegistry();

            // Null action callbacks
            reg.Register(new DelegateSessionParticipant("empty_participant"));
            reg.Register(new DelegateSessionParticipant("null_delegates", null, null, null, null));

            var ex = Record.Exception(() =>
            {
                reg.ResetAll();
                reg.DisposeAll();
            });

            Assert.Null(ex);
        }
    }
}
