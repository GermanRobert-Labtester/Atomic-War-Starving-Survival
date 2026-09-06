// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterPrisonerSystemTests
    {
        private static CaptiveInterrogationCatalog CreateMockCatalog()
        {
            var catalog = new CaptiveInterrogationCatalog
            {
                captive_archetypes = new List<CaptiveArchetypeDef>
                {
                    new CaptiveArchetypeDef
                    {
                        id = "captive_raider_scout",
                        display_name = "Raider Scout",
                        faction_origin = "faction_iron_raiders",
                        base_resistance = 40f,
                        base_hostility = 60f
                    }
                },
                interrogation_topics = new List<InterrogationTopicDef>
                {
                    new InterrogationTopicDef
                    {
                        id = "topic_arms_cache",
                        display_name = "Arms Cache",
                        intel_category = "cache_location",
                        intel_gain = 2,
                        resistance_cost = 20f,
                        reward_item_id = "item_travel_ration"
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        [Fact]
        public void CapturePrisoner_WithinCapacity_Succeeds()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            var res = system.CapturePrisoner("captive_raider_scout", "Prisoner Dan", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(1, system.PrisonerCount);

            var p = system.GetPrisoner("captive_2");
            Assert.NotNull(p);
            Assert.Equal("Prisoner Dan", p.Name);
            Assert.Equal(PrisonerStatus.Detained, p.Status);
        }

        [Fact]
        public void CapturePrisoner_AtMaxCapacity_IsBlocked()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            for (int i = 0; i < 4; i++)
            {
                var r = system.CapturePrisoner("captive_raider_scout", $"P{i}", 1);
                Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            }

            var blocked = system.CapturePrisoner("captive_raider_scout", "P5", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.Equal(4, system.PrisonerCount);
        }

        [Fact]
        public void Interrogate_RapportBuilding_LowersHostilityAndExtractsTopic()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            system.CapturePrisoner("captive_raider_scout", "Dan", 1);
            var p = system.GetPrisoner("captive_2");
            Assert.NotNull(p);

            float initialHostility = p.Hostility;

            // First round
            var res1 = system.Interrogate("captive_2", "topic_arms_cache", InterrogationApproach.RapportBuilding, "surv_interrogator");
            Assert.Equal(ActionResult.StatusKind.Success, res1.Status);
            Assert.True(p.Hostility < initialHostility);
            Assert.True(p.Compliance > 0f);

            // Second round -> should cross threshold and extract
            var res2 = system.Interrogate("captive_2", "topic_arms_cache", InterrogationApproach.RapportBuilding, "surv_interrogator");
            Assert.Equal(ActionResult.StatusKind.Success, res2.Status);
            Assert.Contains("topic_arms_cache", p.ExtractedTopics);
            Assert.Equal(1, inv.CountById("item_travel_ration"));
        }

        [Fact]
        public void AssignPenalLabor_AccumulatesFatigueAndProducesResources()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            system.CapturePrisoner("captive_raider_scout", "Worker Dan", 1);
            system.AssignGuards(new[] { "guard_1" });
            system.AssignPenalLabor("captive_2", PenalShiftKind.SlurryPumping);

            system.TickDay(2);

            var p = system.GetPrisoner("captive_2");
            Assert.NotNull(p);
            Assert.True(p.Fatigue >= 20f);
            Assert.Equal(1, system.State.TotalLaborShiftsCompleted);
            Assert.Equal(1, inv.CountById("item_travel_ration"));
        }

        [Fact]
        public void GrantParole_FailsWhenUnreformed_SucceedsWhenReformed()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            system.CapturePrisoner("captive_raider_scout", "Dan", 1);

            // Cannot parole unreformed prisoner
            var fail = system.GrantParole("captive_2", out string survId1);
            Assert.Equal(ActionResult.StatusKind.Blocked, fail.Status);

            // Manually set reformed attributes
            var p = system.GetPrisoner("captive_2");
            Assert.NotNull(p);
            p.Compliance = 85f;
            p.Hostility = 10f;
            p.Resistance = 5f;

            var ok = system.GrantParole("captive_2", out string survId2);
            Assert.Equal(ActionResult.StatusKind.Success, ok.Status);
            Assert.Equal("survivor_paroled_captive_2", survId2);
            Assert.Equal(1, system.TotalParoled);
            Assert.Equal(0, system.PrisonerCount);
        }

        [Fact]
        public void SaveRestore_PreservesAllPrisonerRecords()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new ShelterPrisonerSystem(rng, inv, catalog);

            system.CapturePrisoner("captive_raider_scout", "Dan", 1);
            system.AssignGuards(new[] { "guard_1" });
            system.AssignPenalLabor("captive_2", PenalShiftKind.FilterScrubbing);
            system.TickDay(2);

            var state = system.CaptureState();

            var system2 = new ShelterPrisonerSystem(rng, inv, catalog);
            system2.RestoreState(state);

            Assert.Equal(1, system2.PrisonerCount);
            var p = system2.GetPrisoner("captive_2");
            Assert.NotNull(p);
            Assert.Equal("Dan", p.Name);
            Assert.Equal(PrisonerStatus.PenalLabor, p.Status);
            Assert.Equal(PenalShiftKind.FilterScrubbing, p.AssignedShift);
        }
    }
}
