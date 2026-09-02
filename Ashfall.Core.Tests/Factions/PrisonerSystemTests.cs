// SPDX-License-Identifier: MIT
// ============================================================================
// Unit Tests: PrisonerSystemTests (Plan 179)
// ============================================================================
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class PrisonerSystemTests
    {
        private static PrisonerSystem CreateSystem(int seed = 42)
        {
            var inv = new Inventory.Inventory();
            var sys = new PrisonerSystem(new SeededRng(seed), inv);
            sys.RegisterTactic(new InterrogationTacticDef
            {
                tactic_id = "interrogation_conversation",
                display_name = "Rapport & Conversation",
                base_compliance_delta = 10f,
                trust_delta = 15f,
                fear_delta = -5f,
                resentment_delta = -10f,
                health_damage = 0f,
                morale_penalty = 0f,
                intel_chance = 0.6f,
                false_intel_chance = 0.05f,
                cooldown_days = 1,
                severity = "Humane"
            });
            sys.RegisterTactic(new InterrogationTacticDef
            {
                tactic_id = "interrogation_torture",
                display_name = "Physical Torture",
                base_compliance_delta = 40f,
                trust_delta = -50f,
                fear_delta = 50f,
                resentment_delta = 50f,
                health_damage = 25f,
                morale_penalty = -15f,
                intel_chance = 0.9f,
                false_intel_chance = 0.40f,
                cooldown_days = 2,
                severity = "Brutal"
            });
            return sys;
        }

        [Fact]
        public void TakePrisoner_Enforces_Cell_Capacity()
        {
            var sys = CreateSystem();
            sys.State.maxCellCapacity = 2;

            Assert.True(sys.TakePrisoner("captive_1", "faction_iron_crows", 1));
            Assert.True(sys.TakePrisoner("captive_2", "faction_iron_crows", 1));
            // Capacity reached: should block 3rd prisoner
            Assert.False(sys.TakePrisoner("captive_3", "faction_iron_crows", 1));
        }

        [Fact]
        public void Cell_Upkeep_Consumes_Food_And_Water()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("ration_hardtack", 10);
            inv.AddById("clean_water", 10);
            var sys = new PrisonerSystem(new SeededRng(42), inv);

            sys.TakePrisoner("captive_hungry", "faction_raiders", 1);
            int rationsBefore = inv.CountById("ration_hardtack");
            int waterBefore = inv.CountById("clean_water");

            sys.TickUpkeepAndEscape(2);
            Assert.Equal(rationsBefore - 1, inv.CountById("ration_hardtack"));
            Assert.Equal(waterBefore - 1, inv.CountById("clean_water"));
        }

        [Fact]
        public void Interrogation_Yields_Deterministic_Compliance_And_Intel()
        {
            var sys = CreateSystem(123);
            sys.TakePrisoner("captive_intel", "faction_vultures", 1);

            var res = sys.Interrogate("captive_intel", "interrogation_conversation", 1);
            Assert.True(res.Success);
            Assert.Equal(10.0f, res.ComplianceDelta);

            var captive = sys.GetCaptive("captive_intel");
            Assert.NotNull(captive);
            Assert.Equal(25.0f, captive!.compliance); // 15 initial + 10 delta
        }

        [Fact]
        public void Severe_Interrogation_Inflicts_Morale_Shock()
        {
            var sys = CreateSystem(456);
            sys.TakePrisoner("captive_brutal", "faction_mutants", 1);

            float shockObserved = 0f;
            sys.OnSettlementMoraleShock += delta => shockObserved = delta;

            var res = sys.Interrogate("captive_brutal", "interrogation_torture", 1);
            Assert.True(res.Success);
            Assert.Equal(-15f, res.MoraleShock);
            Assert.Equal(-15f, shockObserved);

            var captive = sys.GetCaptive("captive_brutal");
            Assert.Equal(1, captive!.abuseWitnessCount);
            Assert.Equal(75.0f, captive.health); // 100 - 25 damage
        }

        [Fact]
        public void Unguarded_Cells_Accumulate_Escape_Pressure()
        {
            var sys = CreateSystem();
            sys.TakePrisoner("captive_runaway", "faction_bandits", 1);
            var captive = sys.GetCaptive("captive_runaway");
            captive!.assignedGuardId = null;

            for (int day = 2; day <= 6; day++)
            {
                sys.TickUpkeepAndEscape(day);
            }

            Assert.True(captive.escapeProgress >= 80.0f || captive.status == CaptiveStatus.Escaped);
        }

        [Fact]
        public void Recruitment_Requires_Trust_And_Clean_Record()
        {
            var sys = CreateSystem();
            sys.TakePrisoner("captive_friendly", "faction_scavengers", 1);
            var captive = sys.GetCaptive("captive_friendly");

            // Not enough trust or time served
            Assert.False(sys.RecruitPrisoner("captive_friendly", 2));

            // Elevate trust, compliance, and advance days
            captive!.trust = 55.0f;
            captive.compliance = 65.0f;
            captive.resentment = 10.0f;

            Assert.True(sys.RecruitPrisoner("captive_friendly", 5));
            Assert.Equal(CaptiveStatus.Recruited, captive.status);
            Assert.Equal(1, sys.State.totalRecruits);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Captives_And_Intel()
        {
            var sys = CreateSystem();
            sys.TakePrisoner("save_captive", "faction_syndicate", 1);
            var captive = sys.GetCaptive("save_captive");
            captive!.compliance = 45.0f;
            captive.escapeProgress = 30.0f;

            var state = sys.CaptureState();
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var restoredState = System.Text.Json.JsonSerializer.Deserialize<PrisonerState>(json);

            Assert.NotNull(restoredState);
            var restoredSys = CreateSystem();
            restoredSys.RestoreState(restoredState!);

            var restoredCaptive = restoredSys.GetCaptive("save_captive");
            Assert.NotNull(restoredCaptive);
            Assert.Equal(45.0f, restoredCaptive!.compliance);
            Assert.Equal(30.0f, restoredCaptive.escapeProgress);
            Assert.Equal("faction_syndicate", restoredCaptive.sourceFactionId);
        }
    }
}
