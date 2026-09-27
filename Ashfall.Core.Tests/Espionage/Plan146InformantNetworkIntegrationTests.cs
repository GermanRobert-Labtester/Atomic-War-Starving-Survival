// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 146 batch-4 / ORPHAN-SEAL A.83 — informant network integration contract.
// Recruitment idempotence, deterministic ops, compromise gate, drift,
// interrogation doctrine, sweeps, save custody, registry.
// ============================================================================
using System.Linq;
using Ashfall.Core.Espionage;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Espionage
{
    public sealed class Plan146InformantNetworkIntegrationTests
    {
        [Fact]
        public void Recruitment_IsIdempotent_KeepsFirstProfile()
        {
            var system = new InformantNetworkSystem();
            var a = system.Recruit("informant_x", "faction_a", InformantArchetype.IdeologicalDefector, TradecraftMethod.DeadDrop);
            var b = system.Recruit("informant_x", "faction_b", InformantArchetype.MercenaryBroker, TradecraftMethod.DirectBriefing);
            Assert.Same(a, b);
            Assert.Equal("faction_a", a.TargetFactionId);
            Assert.Single(system.State.informants);
        }

        [Fact]
        public void Operations_AreDeterministic_AcrossReplay()
        {
            var system = new InformantNetworkSystem();
            system.Recruit("informant_det", "faction_g", InformantArchetype.EmbeddedOfficial, TradecraftMethod.DeadDrop);
            var first = system.RunOperation("informant_det", day: 500, worldSeed: 424242);
            var replay = new InformantNetworkSystem(system.CaptureState());
            var second = replay.RunOperation("informant_det", day: 500, worldSeed: 424242);
            Assert.Equal(first.Success, second.Success);
            Assert.Equal(first.IntelPointsDelivered, second.IntelPointsDelivered);
            Assert.Equal(first.InterceptedByEnemy, second.InterceptedByEnemy);
        }

        [Fact]
        public void CompromisedAssets_RefuseToRun_AndBankNoIntel()
        {
            var system = new InformantNetworkSystem();
            system.Recruit("informant_burn", "faction_h", InformantArchetype.CoercedAsset, TradecraftMethod.DeadDrop);
            system.State.informants[0].IsCompromised = true;
            int banked = system.State.totalIntelPoints;
            var result = system.RunOperation("informant_burn", 10, 999);
            Assert.False(result.Success);
            Assert.True(result.AssetCompromised);
            Assert.Equal(banked, system.State.totalIntelPoints);
        }

        [Fact]
        public void DailyDrift_LowersSuspicion_OrdinalOrder()
        {
            var system = new InformantNetworkSystem();
            system.Recruit("informant_a", "faction_a", InformantArchetype.IdeologicalDefector, TradecraftMethod.DeadDrop, suspicionPermille: 500);
            system.Recruit("informant_b", "faction_b", InformantArchetype.MercenaryBroker, TradecraftMethod.DeadDrop, suspicionPermille: 500);
            system.TickDay(900);
            Assert.All(system.State.informants, i => Assert.True(i.SuspicionPermille < 500));
            Assert.Equal(900, system.State.lastProcessedDay);
        }

        [Fact]
        public void InterrogationDoctrine_GatesReliability()
        {
            var system = new InformantNetworkSystem();
            system.Recruit("informant_captive", "faction_r", InformantArchetype.CoercedAsset, TradecraftMethod.DeadDrop);
            var humane = system.InterrogateCaptive("informant_captive", humaneProtocolsEnforced: true);
            var coercive = system.InterrogateCaptive("informant_captive", humaneProtocolsEnforced: false);
            Assert.True(humane.ReliableIntelligenceObtained);
            Assert.Equal(0, humane.MoraleCostPermille);
            Assert.False(coercive.ReliableIntelligenceObtained);
            Assert.True(coercive.FabricatedIntelWarning);
            Assert.True(coercive.MoraleCostPermille >= 400);
        }

        [Fact]
        public void CaptureRestore_PreservesRosterAndBank()
        {
            var system = new InformantNetworkSystem();
            system.Recruit("informant_1", "faction_a", InformantArchetype.EmbeddedOfficial, TradecraftMethod.DeadDrop, yieldPermille: 800);
            system.RunOperation("informant_1", 50, 123);
            var restored = new InformantNetworkSystem();
            restored.RestoreState(system.CaptureState());
            Assert.Equal(system.State.informants.Count, restored.State.informants.Count);
            Assert.Equal(system.State.totalIntelPoints, restored.State.totalIntelPoints);
            Assert.Equal(800, restored.State.informants.Single(i => i.InformantId == "informant_1").IntelligenceYieldPermille);
        }

        [Fact]
        public void Registry_RegistersInformantNetworkSection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("informant_network", out var meta));
            Assert.Equal("SaveInformantNetwork", meta!.SaveMethod);
            Assert.Equal("informant_network_save.json", SaveSectionRegistry.FileNameFor("informant_network"));
        }
    }
}
