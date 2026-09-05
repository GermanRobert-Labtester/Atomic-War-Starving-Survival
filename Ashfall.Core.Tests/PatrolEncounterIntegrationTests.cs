using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class PatrolEncounterIntegrationTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolEncounterIntegrationTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        private Inventory.Inventory CreateInventory(int capacity = 50, float maxWeight = 500f)
        {
            return new Inventory.Inventory
            {
                Capacity = capacity,
                MaxWeight = maxWeight
            };
        }

        #region Task F1 — Faction Standing Effects

        [Fact]
        public void FactionStandingDelta_AuthoredFixtures_ApplyCorrectDeltas()
        {
            var war = new FactionWarSystem();
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 10);
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            // Fixture 1: enc_patrol_garrison_checkpoint / choice_pay_garrison_toll -> iron_garrison +1
            int initialGarrison = war.GetStanding("iron_garrison");
            bool ok1 = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res1);
            Assert.True(ok1);
            Assert.NotNull(res1);
            Assert.Equal("iron_garrison", res1!.FactionId);
            Assert.Equal(1, res1.FactionStandingDelta);
            Assert.Equal(initialGarrison + 1, war.GetStanding("iron_garrison"));

            // Fixture 2: enc_patrol_warlord_raid / choice_warlord_fight -> warlords_sector_4 -15
            int initialWarlords = war.GetStanding("warlords_sector_4");
            bool ok2 = sys.ResolveChoice("enc_patrol_warlord_raid", "choice_warlord_fight", 1, out var res2);
            Assert.True(ok2);
            Assert.NotNull(res2);
            Assert.Equal("warlords_sector_4", res2!.FactionId);
            Assert.Equal(-15, res2.FactionStandingDelta);
            Assert.Equal(initialWarlords - 15, war.GetStanding("warlords_sector_4"));

            // Fixture 3: enc_patrol_black_ops_ambush / choice_blackops_fight -> faction_black_ops -20
            int initialBlackOps = war.GetStanding("faction_black_ops");
            bool ok3 = sys.ResolveChoice("enc_patrol_black_ops_ambush", "choice_blackops_fight", 1, out var res3);
            Assert.True(ok3);
            Assert.NotNull(res3);
            Assert.Equal("faction_black_ops", res3!.FactionId);
            Assert.Equal(-20, res3.FactionStandingDelta);
            Assert.Equal(initialBlackOps - 20, war.GetStanding("faction_black_ops"));

            // Fixture 4: enc_patrol_warlord_press_gang / choice_press_side_with -> warlords_sector_4 +8
            int beforePressGang = war.GetStanding("warlords_sector_4");
            bool ok4 = sys.ResolveChoice("enc_patrol_warlord_press_gang", "choice_press_side_with", 1, out var res4);
            Assert.True(ok4);
            Assert.NotNull(res4);
            Assert.Equal("warlords_sector_4", res4!.FactionId);
            Assert.Equal(8, res4.FactionStandingDelta);
            Assert.Equal(beforePressGang + 8, war.GetStanding("warlords_sector_4"));
        }

        [Fact]
        public void FactionStanding_ClampedWithinBounds()
        {
            var war = new FactionWarSystem();
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 10);
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            // Set warlords_sector_4 near upper bound
            war.ModifyStanding("warlords_sector_4", 95);
            Assert.Equal(95, war.GetStanding("warlords_sector_4"));

            // Applying +8 should clamp to 100
            bool ok1 = sys.ResolveChoice("enc_patrol_warlord_press_gang", "choice_press_side_with", 1, out _);
            Assert.True(ok1);
            Assert.Equal(100, war.GetStanding("warlords_sector_4"));

            // Set faction_black_ops near lower bound
            war.ModifyStanding("faction_black_ops", -90);
            Assert.Equal(-90, war.GetStanding("faction_black_ops"));

            // Applying -20 should clamp to -100
            bool ok2 = sys.ResolveChoice("enc_patrol_black_ops_ambush", "choice_blackops_fight", 1, out _);
            Assert.True(ok2);
            Assert.Equal(-100, war.GetStanding("faction_black_ops"));
        }

        [Fact]
        public void FactionStanding_ZeroDeltaOrEmpty_DoesNotModifyStanding()
        {
            var war = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, null, war);

            int garrisonBefore = war.GetStanding("iron_garrison");

            // Zero delta: choice_negotiate_garrison has delta 0
            bool ok1 = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_negotiate_garrison", 1, out var res1);
            Assert.True(ok1);
            Assert.NotNull(res1);
            Assert.Equal(0, res1!.FactionStandingDelta);
            Assert.Equal(garrisonBefore, war.GetStanding("iron_garrison"));

            // Empty faction: choice_avoid_garrison has empty faction_id (tested on day 7 after day 6 cooldown)
            bool ok2 = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_avoid_garrison", 7, out var res2);
            Assert.True(ok2);
            Assert.NotNull(res2);
            Assert.True(string.IsNullOrEmpty(res2!.FactionId));
            Assert.Equal(garrisonBefore, war.GetStanding("iron_garrison"));
        }

        [Fact]
        public void FactionStanding_Isolation_OnlyTargetFactionChanges()
        {
            var war = new FactionWarSystem();
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 5);
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            int centralBefore = war.GetStanding("faction_central_garrison");
            int rebuildersBefore = war.GetStanding("faction_rebuilders");
            int blackOpsBefore = war.GetStanding("faction_black_ops");

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out _);
            Assert.True(ok);

            // Only iron_garrison changed
            Assert.Equal(1, war.GetStanding("iron_garrison"));
            Assert.Equal(centralBefore, war.GetStanding("faction_central_garrison"));
            Assert.Equal(rebuildersBefore, war.GetStanding("faction_rebuilders"));
            Assert.Equal(blackOpsBefore, war.GetStanding("faction_black_ops"));
        }

        [Fact]
        public void FactionStanding_SaveReloadDuringCooldown_DoesNotReapplyStanding()
        {
            var war = new FactionWarSystem();
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 10);
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            // Resolve on Day 1
            bool ok1 = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out _);
            Assert.True(ok1);
            Assert.Equal(1, war.GetStanding("iron_garrison"));

            // Save and restore
            var state = sys.CaptureState();
            var sysRestored = new TravelEncounterSystem(_catalog, inv, war);
            sysRestored.RestoreState(state);

            // Attempting to resolve on Day 2 during cooldown (next available Day 6)
            bool ok2 = sysRestored.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 2, out _);
            Assert.False(ok2);

            // Standing remains 1 (not reapplied)
            Assert.Equal(1, war.GetStanding("iron_garrison"));
        }

        #endregion

        #region Task F2 — Cost Items

        [Fact]
        public void CostItems_NormalizedAggregation_CombinesDuplicates()
        {
            Assert.True(_catalog.TryGetEncounter("enc_patrol_garrison_checkpoint", out var enc1));
            var choice1 = enc1.Choices.First(c => c.ChoiceId == "choice_pay_garrison_toll");
            var costs1 = choice1.GetNormalizedCosts();
            Assert.Single(costs1);
            Assert.Equal("canned_food", costs1[0].ItemId);
            Assert.Equal(2, costs1[0].Quantity);

            Assert.True(_catalog.TryGetEncounter("enc_patrol_warlord_raid", out var enc2));
            var choice2 = enc2.Choices.First(c => c.ChoiceId == "choice_warlord_bribe");
            var costs2 = choice2.GetNormalizedCosts();
            Assert.Single(costs2);
            Assert.Equal("canned_food", costs2[0].ItemId);
            Assert.Equal(3, costs2[0].Quantity);

            Assert.True(_catalog.TryGetEncounter("enc_patrol_refugee_eviction", out var enc3));
            var choice3 = enc3.Choices.First(c => c.ChoiceId == "choice_eviction_supplies");
            var costs3 = choice3.GetNormalizedCosts();
            Assert.Equal(2, costs3.Count);
            Assert.Contains(costs3, c => c.ItemId == "canned_food" && c.Quantity == 1);
            Assert.Contains(costs3, c => c.ItemId == "bandage" && c.Quantity == 1);

            Assert.True(_catalog.TryGetEncounter("enc_patrol_cult_recon", out var enc4));
            var choice4 = enc4.Choices.First(c => c.ChoiceId == "choice_cult_trade_relics");
            var costs4 = choice4.GetNormalizedCosts();
            Assert.Single(costs4);
            Assert.Equal("tarnished_medal", costs4[0].ItemId);
            Assert.Equal(1, costs4[0].Quantity);

            Assert.True(_catalog.TryGetEncounter("enc_patrol_penal_battalion", out var enc5));
            var choice5 = enc5.Choices.First(c => c.ChoiceId == "choice_penal_trade_warden");
            var costs5 = choice5.GetNormalizedCosts();
            Assert.Single(costs5);
            Assert.Equal("soldering_kit", costs5[0].ItemId);
            Assert.Equal(1, costs5[0].Quantity);
        }

        [Fact]
        public void CostItems_AtomicRemoval_AllOrNothingOnInsufficient()
        {
            var inv = CreateInventory();
            // Player only has 1 canned_food, but choice_pay_garrison_toll requires 2
            inv.TryProduce("canned_food", 1);
            var war = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.False(ok);
            Assert.Null(res);

            // 0 items removed!
            Assert.Equal(1, inv.CountById("canned_food"));

            // 0 side effects!
            Assert.Equal(0, war.GetStanding("iron_garrison"));

            // Cooldown was NOT set
            Assert.True(sys.IsEncounterEligible(
                _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!,
                "high_scarp", 1.0f, "all", 1));
        }

        [Fact]
        public void CostItems_MultiItem_PartialShortfall_RollsBack()
        {
            var inv = CreateInventory();
            // choice_eviction_supplies requires 1 canned_food + 1 bandage
            // Player has canned_food but NO bandage
            inv.TryProduce("canned_food", 1);
            var sys = new TravelEncounterSystem(_catalog, inv);

            bool ok = sys.ResolveChoice("enc_patrol_refugee_eviction", "choice_eviction_supplies", 1, out var res);
            Assert.False(ok);
            Assert.Null(res);

            // The canned_food was NOT consumed
            Assert.Equal(1, inv.CountById("canned_food"));
            Assert.Equal(0, inv.CountById("bandage"));
        }

        [Fact]
        public void CostItems_SufficientStock_DeductsExactCost()
        {
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 5);
            var war = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.True(ok);
            Assert.NotNull(res);

            // Exactly 2 consumed, 3 remaining
            Assert.Equal(3, inv.CountById("canned_food"));
            Assert.Equal(1, war.GetStanding("iron_garrison"));
        }

        [Fact]
        public void CostItems_AllFiveAuthoredFixtures_DeductExpectedQuantities()
        {
            var inv = CreateInventory();
            inv.TryProduce("canned_food", 10);
            inv.TryProduce("bandage", 5);
            inv.TryProduce("tarnished_medal", 2);
            inv.TryProduce("soldering_kit", 2);

            var sys = new TravelEncounterSystem(_catalog, inv);

            // 1. Toll: 2 canned_food
            Assert.True(sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out _));
            Assert.Equal(8, inv.CountById("canned_food"));

            // 2. Bribe: 3 canned_food
            Assert.True(sys.ResolveChoice("enc_patrol_warlord_raid", "choice_warlord_bribe", 1, out _));
            Assert.Equal(5, inv.CountById("canned_food"));

            // 3. Eviction supplies: 1 canned_food, 1 bandage
            Assert.True(sys.ResolveChoice("enc_patrol_refugee_eviction", "choice_eviction_supplies", 1, out _));
            Assert.Equal(4, inv.CountById("canned_food"));
            Assert.Equal(4, inv.CountById("bandage"));

            // 4. Cult relics: 1 tarnished_medal
            Assert.True(sys.ResolveChoice("enc_patrol_cult_recon", "choice_cult_trade_relics", 1, out _));
            Assert.Equal(1, inv.CountById("tarnished_medal"));

            // 5. Penal trade: 1 soldering_kit
            Assert.True(sys.ResolveChoice("enc_patrol_penal_battalion", "choice_penal_trade_warden", 1, out _));
            Assert.Equal(1, inv.CountById("soldering_kit"));
        }

        #endregion

        #region Task F3 — Required Item Availability

        [Fact]
        public void RequiredItem_GatesChoiceAvailability()
        {
            var inv = CreateInventory();
            var sys = new TravelEncounterSystem(_catalog, inv);

            var enc1 = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var choice1 = enc1.Choices.First(c => c.ChoiceId == "choice_show_garrison_pass");

            // Empty inventory: Unavailable
            var avail1 = sys.EvaluateChoiceAvailability(choice1, inv);
            Assert.False(avail1.IsAvailable);
            Assert.Single(avail1.Failures);
            Assert.Equal(ChoiceRequirementFailureType.MissingRequiredItem, avail1.Failures[0].FailureType);
            Assert.Equal("sealed_government_document", avail1.Failures[0].ItemId);
            Assert.Equal(1, avail1.Failures[0].RequiredQuantity);
            Assert.Equal(0, avail1.Failures[0].AvailableQuantity);

            // Cannot resolve
            bool resolvedNoItem = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_show_garrison_pass", 1, out _);
            Assert.False(resolvedNoItem);

            // Give document
            inv.TryProduce("sealed_government_document", 1);

            // Now available
            var avail2 = sys.EvaluateChoiceAvailability(choice1, inv);
            Assert.True(avail2.IsAvailable);
            Assert.Empty(avail2.Failures);

            // Second fixture: enc_patrol_central_garrison_border / choice_central_comply
            var enc2 = _catalog.GetEncounter("enc_patrol_central_garrison_border")!;
            var choice2 = enc2.Choices.First(c => c.ChoiceId == "choice_central_comply");
            var avail3 = sys.EvaluateChoiceAvailability(choice2, inv);
            Assert.True(avail3.IsAvailable);
        }

        [Fact]
        public void RequiredItem_NonConsuming_RemainsInInventory()
        {
            var inv = CreateInventory();
            inv.TryProduce("sealed_government_document", 1);
            var war = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_show_garrison_pass", 1, out var res);
            Assert.True(ok);
            Assert.NotNull(res);

            // Non-consuming gate: sealed_government_document is still in inventory!
            Assert.Equal(1, inv.CountById("sealed_government_document"));
            Assert.Equal(2, war.GetStanding("iron_garrison"));
        }

        [Fact]
        public void RequiredItem_DynamicAvailability_ReEvaluatedAtSelectionTime()
        {
            var inv = CreateInventory();
            var sys = new TravelEncounterSystem(_catalog, inv);

            var enc = _catalog.GetEncounter("enc_patrol_central_garrison_border")!;
            var choice = enc.Choices.First(c => c.ChoiceId == "choice_central_comply");

            // Check before acquiring document
            Assert.False(sys.EvaluateChoiceAvailability(choice, inv).IsAvailable);

            // Player acquires document during gameplay
            inv.TryProduce("sealed_government_document", 1);

            // Re-evaluate choice dynamically: now available!
            Assert.True(sys.EvaluateChoiceAvailability(choice, inv).IsAvailable);
        }

        [Fact]
        public void ChoiceAvailability_DescriptiveText_ProvidesReadableReasons()
        {
            var inv = CreateInventory();
            var sys = new TravelEncounterSystem(_catalog, inv);

            var enc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var passChoice = enc.Choices.First(c => c.ChoiceId == "choice_show_garrison_pass");
            var tollChoice = enc.Choices.First(c => c.ChoiceId == "choice_pay_garrison_toll");

            var passAvail = sys.EvaluateChoiceAvailability(passChoice, inv);
            Assert.Contains("sealed_government_document", passAvail.Failures[0].Reason);
            Assert.Contains("need 1", passAvail.Failures[0].Reason);

            var tollAvail = sys.EvaluateChoiceAvailability(tollChoice, inv);
            Assert.Contains("canned_food", tollAvail.Failures[0].Reason);
            Assert.Contains("x2", tollAvail.Failures[0].Reason);
        }

        #endregion

        #region Task F4 — Expedition Wiring

        [Fact]
        public void ExpeditionBridge_SurfacesPatrolArchetypes()
        {
            var narrative = new NarrativeEncounterSystem();
            var travelSys = new TravelEncounterSystem(_catalog);
            var rng = new SeededRng(100);
            var bridge = new ExpeditionEncounterBridge(narrative, rng, travelSys);

            string[] expectedPatrolArchetypes = new[]
            {
                "enc_patrol_garrison_checkpoint",
                "enc_patrol_warlord_raid",
                "enc_patrol_refugee_eviction",
                "enc_patrol_foundry_supply",
                "enc_patrol_supply_corps_convoy"
            };

            var surfacedPatrols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            bridge.OnSurfaced += dto =>
            {
                if (!string.IsNullOrEmpty(dto.encounter_id) && dto.encounter_id.StartsWith("enc_patrol_"))
                {
                    surfacedPatrols.Add(dto.encounter_id);
                }
            };

            string[] locations = new[]
            {
                "loc_high_scarp_road",
                "loc_industrial_belt_foundry",
                "loc_the_toll_bridge"
            };

            // Run sorties across various regions, danger levels, and stances
            for (int day = 1; day <= 300; day++)
            {
                bridge.CurrentDay = day;
                string loc = locations[day % locations.Length];
                var state = new ExpeditionState
                {
                    survivorId = "survivor_scout",
                    locationId = loc,
                    displayName = "Scout Route",
                    stance = "Balanced",
                    dangerLevel = (day % 4) + 1,
                    startedDay = day
                };
                bridge.Surface(state);
            }

            // Verify that at least 5 patrol archetypes surfaced
            int matchCount = expectedPatrolArchetypes.Count(id => surfacedPatrols.Contains(id));
            Assert.True(matchCount >= 5, $"Expected all 5 patrol archetypes to surface, but found {matchCount}: {string.Join(", ", surfacedPatrols)}");
        }

        [Fact]
        public void ExpeditionBridge_SharedDeterministicRng_ProducesIdenticalSurfacedSequence()
        {
            var narrative1 = new NarrativeEncounterSystem();
            var narrative2 = new NarrativeEncounterSystem();
            var travel1 = new TravelEncounterSystem(_catalog);
            var travel2 = new TravelEncounterSystem(_catalog);

            var rng1 = new SeededRng(777);
            var rng2 = new SeededRng(777);

            var bridge1 = new ExpeditionEncounterBridge(narrative1, rng1, travel1);
            var bridge2 = new ExpeditionEncounterBridge(narrative2, rng2, travel2);

            var list1 = new List<string>();
            var list2 = new List<string>();

            bridge1.OnSurfaced += dto => list1.Add(dto.encounter_id ?? "bare");
            bridge2.OnSurfaced += dto => list2.Add(dto.encounter_id ?? "bare");

            for (int i = 1; i <= 20; i++)
            {
                var state = new ExpeditionState
                {
                    survivorId = "survivor_scout",
                    locationId = "loc_high_scarp_road",
                    stance = "Balanced",
                    dangerLevel = 2,
                    startedDay = i
                };
                bridge1.Surface(state);
                bridge2.Surface(state);
            }

            Assert.Equal(list1.Count, list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.Equal(list1[i], list2[i]);
            }
        }

        [Fact]
        public void ExpeditionBridge_SharedCooldown_SynchronizedAcrossExpeditionsAndTravel()
        {
            var narrative = new NarrativeEncounterSystem();
            var travelSys = new TravelEncounterSystem(_catalog);
            var rng = new SeededRng(42);
            var bridge = new ExpeditionEncounterBridge(narrative, rng, travelSys);

            string encId = "enc_patrol_garrison_checkpoint";
            string choiceId = "choice_avoid_garrison"; // choice with 0 costs so it always succeeds

            // Day 10: Resolve through expedition bridge
            bool resolved = bridge.ResolveChoice(encId, choiceId, 10, "loc_high_scarp");
            Assert.True(resolved);

            var def = _catalog.GetEncounter(encId)!;

            // Day 12: Inactive in TravelEncounterSystem (5-day cooldown until day 15)
            Assert.False(travelSys.IsEncounterEligible(def, "high_scarp", 1.0f, "all", 12));

            // Day 14: Still inactive
            Assert.False(travelSys.IsEncounterEligible(def, "high_scarp", 1.0f, "all", 14));

            // Day 15: Cooldown expired! Active again in TravelEncounterSystem
            Assert.True(travelSys.IsEncounterEligible(def, "high_scarp", 1.0f, "all", 15));
        }

        [Fact]
        public void ExpeditionBridge_PatrolChoices_CarryAuthoritativeMetadata()
        {
            var narrative = new NarrativeEncounterSystem();
            var travelSys = new TravelEncounterSystem(_catalog);
            var rng = new SeededRng(1);
            var bridge = new ExpeditionEncounterBridge(narrative, rng, travelSys);

            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto =>
            {
                if (dto.encounter_id == "enc_patrol_garrison_checkpoint")
                    surfaced = dto;
            };

            for (int i = 1; i <= 50 && surfaced == null; i++)
            {
                bridge.Surface(new ExpeditionState
                {
                    locationId = "loc_high_scarp",
                    stance = "Cautious",
                    dangerLevel = 1,
                    startedDay = i
                });
            }

            Assert.NotNull(surfaced);
            Assert.Equal("enc_patrol_garrison_checkpoint", surfaced!.encounter_id);
            Assert.NotEmpty(surfaced!.choices);

            var passChoice = surfaced.choices.FirstOrDefault(c => c.choiceId == "choice_show_garrison_pass");
            Assert.NotNull(passChoice);
            Assert.Equal("sealed_government_document", passChoice!.requiredItemId);
            Assert.Equal(1, passChoice.requiredItemQuantity);
            Assert.Equal("iron_garrison", passChoice.factionId);
            Assert.Equal(2, passChoice.factionStandingDelta);

            var tollChoice = surfaced.choices.FirstOrDefault(c => c.choiceId == "choice_pay_garrison_toll");
            Assert.NotNull(tollChoice);
            Assert.Equal(2, tollChoice!.costItems.Count);
            Assert.Equal("canned_food", tollChoice.costItems[0]);
        }

        #endregion
    }
}
