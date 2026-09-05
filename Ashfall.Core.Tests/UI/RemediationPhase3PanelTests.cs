using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class RemediationPhase3PanelTests
    {
        public RemediationPhase3PanelTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void Phase3Panels_AreRegisteredInPanelRegistry()
        {
            Assert.True(PanelRegistry.IsRegistered("trauma_bonding_cohort"),
                "trauma_bonding_cohort must be registered in PanelRegistry");
            var traumaDesc = PanelRegistry.Get("trauma_bonding_cohort");
            Assert.NotNull(traumaDesc);
            Assert.Equal(PanelGroup.Expanded, traumaDesc.Group);
            Assert.Equal("Trauma Bonding Cohort", traumaDesc.DisplayName);

            Assert.True(PanelRegistry.IsRegistered("crossing_safe_conduct_vouch"),
                "crossing_safe_conduct_vouch must be registered in PanelRegistry");
            var vouchDesc = PanelRegistry.Get("crossing_safe_conduct_vouch");
            Assert.NotNull(vouchDesc);
            Assert.Equal(PanelGroup.Expanded, vouchDesc.Group);
            Assert.Equal("Crossing Safe-Conduct Vouch", vouchDesc.DisplayName);
        }

        [Fact]
        public void TraumaBondSystem_AllBondsAndGetBonds_ReflectActiveBonds()
        {
            var system = new TraumaBondSystem();
            system.GetDay = () => 5f;

            Assert.Empty(system.AllBonds);
            Assert.Empty(system.GetBonds("survivor_1"));

            system.OnSharedHazardEndured(new List<string> { "survivor_1", "survivor_2" }, "hazard_fallout_storm");

            Assert.True(system.HasBond("survivor_1", "survivor_2"));
            Assert.True(system.HasBond("survivor_2", "survivor_1"));

            var bonds1 = system.GetBonds("survivor_1");
            Assert.Single(bonds1);
            Assert.Equal("survivor_2", bonds1[0].BondedSurvivorId);
            Assert.Equal("hazard_fallout_storm", bonds1[0].SharedHazardId);
            Assert.Equal(5, bonds1[0].DayFormed);

            Assert.Equal(2, system.AllBonds.Count);
            Assert.True(system.AllBonds.ContainsKey("survivor_1"));
            Assert.True(system.AllBonds.ContainsKey("survivor_2"));

            float bonus = system.GetCoShiftEfficiencyBonus("survivor_1", "survivor_2");
            Assert.True(bonus > 0f);
        }

        [Fact]
        public void VouchAccessSystem_Lifecycle_TransitionsThroughStates()
        {
            var vouch = new VouchAccessSystem();

            // Initial state: requires vouch, no access
            Assert.True(vouch.RequiresVouch);
            Assert.False(vouch.HasAccess);
            Assert.Empty(vouch.VouchedBy);
            Assert.False(vouch.VouchBurned);
            Assert.False(vouch.AccessSoftened);
            Assert.False(vouch.NeedsLastResort);

            // Grant vouch: Captain Ostrowski
            bool granted = vouch.GrantVouch("npc_ostrowski");
            Assert.True(granted);
            Assert.False(vouch.RequiresVouch);
            Assert.True(vouch.HasAccess);
            Assert.Equal("npc_ostrowski", vouch.VouchedBy);

            // Burn vouch
            bool burned = vouch.BurnVouch();
            Assert.True(burned);
            Assert.True(vouch.RequiresVouch);
            Assert.False(vouch.HasAccess);
            Assert.True(vouch.VouchBurned);
            Assert.Empty(vouch.VouchedBy);
            Assert.True(vouch.NeedsLastResort);

            // Grant last resort: Mattis
            bool lastResort = vouch.GrantVouch("npc_mattis", isLastResort: true);
            Assert.True(lastResort);
            Assert.True(vouch.HasAccess);
            Assert.True(vouch.LastResortUsed);
            Assert.False(vouch.NeedsLastResort);

            // Soften access into permanent pass
            bool softened = vouch.SoftenAccess();
            Assert.True(softened);
            Assert.True(vouch.AccessSoftened);
            Assert.True(vouch.HasAccess);
            Assert.False(vouch.RequiresVouch);
        }
    }
}
