// SPDX-License-Identifier: MIT
// ============================================================================
// Unit Tests: StealthSystemTests (Plan 181)
// ============================================================================
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Combat
{
    public sealed class StealthSystemTests
    {
        private static StealthSystem CreateSystem(int seed = 42)
        {
            var inv = new Inventory.Inventory();
            var sys = new StealthSystem(new SeededRng(seed), inv);
            sys.RegisterCamouflageGear(new CamouflageGearDef
            {
                camo_id = "camo_ash_cloak",
                display_name = "Ash-Washed Cowl",
                camo_rating = 0.5f,
                terrain_tags = new List<string> { "ash_dunes", "ruins" },
                weather_tags = new List<string> { "ash_storm" },
                night_modifier = 0.15f,
                noise_modifier = -0.05f
            });
            return sys;
        }

        [Fact]
        public void DetectionRisk_Reflects_Light_Weather_And_Camouflage()
        {
            var sys = CreateSystem();
            sys.EnsurePartyStealth("exp_01");

            var dayClearRisk = sys.CalculateDetectionRisk("exp_01", "clear", isNight: false, "ruins");
            var nightFogRisk = sys.CalculateDetectionRisk("exp_01", "fog", isNight: true, "ruins");

            Assert.True(nightFogRisk.FinalProbability < dayClearRisk.FinalProbability);

            // Equipping camouflage further reduces risk
            sys.EquipCamoGear("exp_01", "camo_ash_cloak");
            var camoRisk = sys.CalculateDetectionRisk("exp_01", "clear", isNight: false, "ruins");
            Assert.True(camoRisk.FinalProbability < dayClearRisk.FinalProbability);
        }

        [Fact]
        public void Weapon_Noise_Profile_Increases_Detection_Probability()
        {
            var sys = CreateSystem();
            var party = sys.EnsurePartyStealth("exp_noisy");
            party.accumulatedNoise = 0.55f; // Heavy unsuppressed rifles

            var profile = sys.CalculateDetectionRisk("exp_noisy", "clear", false, "wasteland");
            Assert.True(profile.FinalProbability >= 0.70f);
        }

        [Fact]
        public void BypassEncounter_Skips_Combat_On_Success()
        {
            // Seed chosen such that roll > detection risk
            var sys = CreateSystem(999);
            sys.EnsurePartyStealth("exp_bypass");
            sys.EquipCamoGear("exp_bypass", "camo_ash_cloak");

            bool bypassed = sys.BypassEncounter("exp_bypass", "fog", true, "ruins");
            Assert.True(bypassed);

            var party = sys.EnsurePartyStealth("exp_bypass");
            Assert.Equal(1, party.consecutiveBypasses);
            Assert.True(party.hasAmbushAdvantage);
            Assert.False(party.isDetected);
        }

        [Fact]
        public void BypassEncounter_Failure_Emits_OnStealthBroken()
        {
            // High noise party fails bypass under daylight
            var sys = CreateSystem(1);
            var party = sys.EnsurePartyStealth("exp_loud");
            party.accumulatedNoise = 0.90f;

            bool brokenFired = false;
            sys.OnStealthBroken += (id, reason) => brokenFired = true;

            bool bypassed = sys.BypassEncounter("exp_loud", "clear", false, "plain");
            Assert.False(bypassed);
            Assert.True(brokenFired);
            Assert.True(party.isDetected);
            Assert.False(party.hasAmbushAdvantage);
        }

        [Fact]
        public void AmbushAttack_Grants_Bounded_First_Strike_Advantage()
        {
            var sys = CreateSystem();
            var party = sys.EnsurePartyStealth("exp_ambush");
            party.hasAmbushAdvantage = true;

            Assert.True(sys.TriggerAmbush("exp_ambush"));
            Assert.Equal(1, sys.State.totalAmbushes);

            // Advantage consumed after round 1
            Assert.False(party.hasAmbushAdvantage);
            Assert.False(sys.TriggerAmbush("exp_ambush"));
        }

        [Fact]
        public void Apex_Predator_Hearing_And_Scent_Defeats_Visual_Camo()
        {
            var sys = CreateSystem();
            sys.EnsurePartyStealth("exp_predator");
            sys.EquipCamoGear("exp_predator", "camo_ash_cloak");

            var normalObserverRisk = sys.CalculateDetectionRisk("exp_predator", "fog", true, "ruins");
            var predatorRisk = sys.CalculateDetectionRisk("exp_predator", "fog", true, "ruins",
                new List<string> { "sense_hearing", "sense_scent" });

            Assert.True(predatorRisk.FinalProbability > normalObserverRisk.FinalProbability);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Stealth_State()
        {
            var sys = CreateSystem();
            var party = sys.EnsurePartyStealth("save_exp");
            party.consecutiveBypasses = 3;
            party.travelMode = StealthTravelMode.NightOps;
            party.nightOpsActive = true;
            sys.EquipCamoGear("save_exp", "camo_ash_cloak");

            var state = sys.CaptureState();
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var restoredState = System.Text.Json.JsonSerializer.Deserialize<StealthState>(json);

            Assert.NotNull(restoredState);
            var restoredSys = CreateSystem();
            restoredSys.RestoreState(restoredState!);

            var restoredParty = restoredSys.EnsurePartyStealth("save_exp");
            Assert.Equal(3, restoredParty.consecutiveBypasses);
            Assert.Equal(StealthTravelMode.NightOps, restoredParty.travelMode);
            Assert.True(restoredParty.nightOpsActive);
            Assert.Contains("camo_ash_cloak", restoredParty.equippedCamoIds);
        }
    }
}
