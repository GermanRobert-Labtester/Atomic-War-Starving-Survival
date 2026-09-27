// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: trauma bond authority — focused contract tests over the
// sealed Core system: bond formation, decay, expiry, save round-trip, and the
// hook surface the host routes into canonical owners.

using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class PlanTraumaBondHostIntegrationTests
    {
        private static TraumaBondSystem BuildSystem(float day = 1f, List<string>? affinity = null)
        {
            var sys = new TraumaBondSystem
            {
                GetDay = () => day,
                AdjustAffinity = (a, b, delta) => affinity?.Add($"{a}->{b}:{delta}")
            };
            return sys;
        }

        [Fact]
        public void SharedHazard_FormsABond_AndRoutesAffinityThroughTheHook()
        {
            var affinity = new List<string>();
            var sys = BuildSystem(3f, affinity);

            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "fallout_storm");

            Assert.True(sys.GetBondStrength("a", "b") >= TraumaBondSystem.MinBondStrengthForBonus);
            Assert.True(sys.HasBond("a", "b"));
            // Exactly one affinity call per pair, routed to the affinity owner.
            Assert.Contains("a->b:15", affinity);
        }

        [Fact]
        public void BondStrength_IsClamped_AndReinforced_ByRepeatedHazards()
        {
            var sys = BuildSystem(1f);

            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "h1");
            float afterOne = sys.GetBondStrength("a", "b");
            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "h2");
            float afterTwo = sys.GetBondStrength("a", "b");

            Assert.Equal(TraumaBondSystem.BondStrengthPerSharedHazard, afterOne);
            Assert.True(afterTwo > afterOne);
            Assert.True(afterTwo <= 1f);
        }

        [Fact]
        public void SingleParticipantHazard_FormsNoBond()
        {
            var sys = BuildSystem(1f);

            sys.OnSharedHazardEndured(new List<string> { "solo" }, "storm");

            Assert.Equal(0, sys.GetBondCount("solo"));
            Assert.False(sys.HasBond("solo", "solo"));
        }

        [Fact]
        public void DailyTick_DecaysBonds_AndEventuallyExpiresThem()
        {
            var sys = BuildSystem(1f);
            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "h");
            float initial = sys.GetBondStrength("a", "b");

            sys.Tick("a", 24f);

            float afterOneDay = sys.GetBondStrength("a", "b");
            Assert.True(afterOneDay < initial);

            for (int d = 0; d < 200; d++) sys.Tick("a", 24f);

            Assert.Equal(0f, sys.GetBondStrength("a", "b"));
            Assert.Equal(0, sys.GetBondCount("a"));
        }

        [Fact]
        public void CoShiftBonus_RequiresABond_AndScalesWithBondStrength()
        {
            var unbonded = BuildSystem(1f);
            Assert.Equal(0f, unbonded.GetCoShiftEfficiencyBonus("a", "b"));

            var bonded = BuildSystem(1f);
            bonded.OnSharedHazardEndured(new List<string> { "a", "b" }, "h");
            float afterOneHazard = bonded.GetCoShiftEfficiencyBonus("a", "b");

            bonded.OnSharedHazardEndured(new List<string> { "a", "b" }, "h2");
            float afterTwoHazards = bonded.GetCoShiftEfficiencyBonus("a", "b");

            Assert.True(afterOneHazard > 0f);
            Assert.True(afterTwoHazards > afterOneHazard);
            // The bonus never exceeds the authority's authored cap.
            Assert.True(afterTwoHazards <= TraumaBondSystem.CoShiftEfficiencyBonus);
        }

        [Fact]
        public void ShiftHook_IsAHostSeam_TheAuthorityDoesNotConsultItItself()
        {
            // The Core authority exposes AreOnSameShift as a host seam; the shared
            // shift gate is enforced by TraumaBondHostSession, which refuses the
            // bonus when the canonical shift owner disagrees. The authority itself
            // only gates on bond strength, so a bare system with no shift hook
            // installed still reports the bond-scaled value.
            var bonded = BuildSystem(1f);
            bonded.OnSharedHazardEndured(new List<string> { "a", "b" }, "h");

            Assert.Null(bonded.AreOnSameShift);
            Assert.True(bonded.GetCoShiftEfficiencyBonus("a", "b") > 0f);
        }

        [Fact]
        public void SameSurvivorPairing_NeverYieldsABonus()
        {
            var sys = BuildSystem(1f);
            sys.AreOnSameShift = (_, _) => true;

            Assert.Equal(0f, sys.GetCoShiftEfficiencyBonus("a", "a"));
        }

        [Fact]
        public void SaveRoundTrip_PreservesBondStrength_AndSharedHazardProvenance()
        {
            var sys = BuildSystem(7f);
            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "raid");
            var captured = sys.CaptureState();

            string json = new SystemTextJsonSerializer().Serialize(captured);
            var restored = new SystemTextJsonSerializer().Deserialize<TraumaBondSaveState>(json);

            Assert.NotNull(restored);
            var reload = new TraumaBondSystem();
            reload.RestoreState(restored);

            Assert.Equal(sys.GetBondStrength("a", "b"), reload.GetBondStrength("a", "b"));
            Assert.Equal(1, reload.GetBondCount("a"));
        }

        [Fact]
        public void NullRestore_ClearsAllBondState()
        {
            var sys = BuildSystem(1f);
            sys.OnSharedHazardEndured(new List<string> { "a", "b" }, "h");

            sys.RestoreState(null);

            Assert.Equal(0, sys.GetBondCount("a"));
            Assert.Equal(0f, sys.GetBondStrength("a", "b"));
        }
    }
}
