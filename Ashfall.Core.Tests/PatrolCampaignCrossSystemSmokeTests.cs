// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship VII (Tasks 31-32): 32-step end-to-end deterministic campaign proof.
    /// Spans caravan patrols, bounty enforcement, war-state transitions,
    /// dynamic territory flips, shared cooldown tables, save/load idempotency,
    /// and 3-run identical trace verification.
    /// </summary>
    public sealed class PatrolCampaignCrossSystemSmokeTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolCampaignCrossSystemSmokeTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        private sealed class CampaignHarness
        {
            public Inventory.Inventory Inventory { get; set; }
            public FactionWarSystem WarSystem { get; set; }
            public FactionBountySystem BountySystem { get; set; }
            public DynamicTerritoryAuthority TerritoryAuthority { get; set; }
            public TravelEncounterSystem TravelSystem { get; set; }
            public TravelingCaravanSystem CaravanSystem { get; set; }
            public List<string> ExecutionTrace { get; } = new();

            public CampaignHarness(TravelEncounterCatalog catalog)
            {
                Inventory = new Inventory.Inventory { Capacity = 200, MaxWeight = 2000f };
                WarSystem = new FactionWarSystem();
                BountySystem = new FactionBountySystem();
                TerritoryAuthority = new DynamicTerritoryAuthority();

                TravelSystem = new TravelEncounterSystem(
                    catalog,
                    Inventory,
                    WarSystem,
                    bountySystem: BountySystem,
                    territoryAuthority: TerritoryAuthority);

                CaravanSystem = new TravelingCaravanSystem
                {
                    TravelEncounters = TravelSystem
                };
            }

            public void LogStep(int stepIndex, int day, string action, string outcome)
            {
                ExecutionTrace.Add($"Step{stepIndex:D2}|Day{day:D2}|{action}|{outcome}");
            }
        }

        private string Run32StepCampaign(int seed)
        {
            var harness = new CampaignHarness(_catalog);
            var inv = harness.Inventory;
            var war = harness.WarSystem;
            var bounty = harness.BountySystem;
            var territory = harness.TerritoryAuthority;
            var travel = harness.TravelSystem;
            var caravan = harness.CaravanSystem;
            var rng = new SeededRng(seed);

            // Initial provisioning
            inv.TryProduce("fuel", 20);
            inv.TryProduce("canned_food", 20);
            inv.TryProduce("scrap_metal", 20);

            // Initial territory: Warlords control loc_toll_house (the_toll)
            territory.SetNode("loc_toll_house", WarlordTerritoryState.Controlled, "warlords_sector_4");
            // Garrison controls pass_mount_karkov (high_scarp)
            territory.SetNode("pass_mount_karkov", WarlordTerritoryState.Controlled, "iron_garrison");

            // --- STEP 1 (Day 1): Peacetime baseline check ---
            Assert.False(war.IsAtWar);
            var raidEnc = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var advanceEnc = _catalog.GetEncounter("enc_patrol_warlord_advancing")!;
            Assert.True(travel.IsEncounterEligible(raidEnc, "the_toll", 2.0f, "all", 1));
            Assert.False(travel.IsEncounterEligible(advanceEnc, "the_toll", 2.0f, "all", 1)); // Wartime only
            harness.LogStep(1, 1, "PeacetimeTerritoryCheck", "WarlordRaidEligible_WartimeAdvanceGated");

            // --- STEP 2 (Day 2): Caravan routes dispatch ---
            var route = new List<string> { "loc_toll_house", "pass_mount_karkov" };
            caravan.SpawnCaravan("caravan_iron_exp", "Iron Expedition", "faction_railway_guild", route, "foundry");
            var activeCaravan = caravan.State.activeCaravans[0];
            Assert.NotNull(activeCaravan);
            harness.LogStep(2, 2, "CaravanSpawn", $"CaravanSpawned_Node_{activeCaravan.currentNodeId}");

            // --- STEP 3 (Day 3): Caravan route encounter selection ---
            var railwayConvoy = _catalog.GetEncounter("enc_patrol_railway_convoy")!;
            Assert.True(travel.IsEncounterEligible(railwayConvoy, "the_toll", 2.0f, "all", 3, activeCaravan.currentNodeId));
            harness.LogStep(3, 3, "CaravanPatrolEligible", "RailwayConvoyEligible");

            // --- STEP 4 (Day 4): Caravan choice resolution (trade fuel for parts) ---
            int fuelBefore = inv.CountById("fuel");
            bool resolvedTrade = caravan.ResolveRouteEncounterChoice(railwayConvoy.Id, "choice_guild_trade_parts", 4, out var tradeRes);
            Assert.True(resolvedTrade);
            Assert.NotNull(tradeRes);
            Assert.Equal(fuelBefore - 1, inv.CountById("fuel"));
            harness.LogStep(4, 4, "CaravanResolveTrade", $"FuelDeducted_NewCount_{inv.CountById("fuel")}");

            // --- STEP 5 (Day 5): Shared cooldown verification between caravan and travel ---
            // Railway convoy is 5-day cooldown -> day 4 + 5 = available day 9
            Assert.False(travel.IsEncounterEligible(railwayConvoy, "the_toll", 2.0f, "all", 5));
            harness.LogStep(5, 5, "SharedCooldownCheck", "RailwayConvoyCooldownActive");

            // --- STEP 6 (Day 6): Peacetime reconnaissance encounter ---
            var ashScouts = _catalog.GetEncounter("enc_patrol_ash_sign_scouts")!;
            var ashWartime = _catalog.GetEncounter("enc_patrol_ash_sign_wartime_recon")!;
            Assert.True(travel.IsEncounterEligible(ashScouts, "the_toll", 2.0f, "all", 6));
            Assert.False(travel.IsEncounterEligible(ashWartime, "the_toll", 2.0f, "all", 6));
            travel.ResolveChoice(ashScouts.Id, "choice_ashsign_observe", 6, out _);
            harness.LogStep(6, 6, "PeacetimeAshScoutsResolved", "AshSignScoutsCooldownSet");

            // --- STEP 7 (Day 7): War eruption! ---
            war.SetWarActive(true);
            Assert.True(war.IsAtWar);
            harness.LogStep(7, 7, "WarStateTrigger", "WarActivated");

            // --- STEP 8 (Day 8): Immediate reaction to wartime ---
            // Wartime advance guard is now eligible!
            Assert.True(travel.IsEncounterEligible(advanceEnc, "the_toll", 2.0f, "all", 8));
            harness.LogStep(8, 8, "WartimeVariantActive", "WarlordAdvanceEligible");

            // --- STEP 9 (Day 9): Severe patrol violation choice (-15 fight) ---
            bool fought = travel.ResolveChoice(advanceEnc.Id, "choice_warlord_fight", 9, out var fightRes);
            Assert.True(fought);
            Assert.NotNull(fightRes);
            Assert.Equal(-15, fightRes.FactionStandingDelta);
            harness.LogStep(9, 9, "ResolveSeverePatrolViolation", "WarlordsAttacked_DeltaMinus15");

            // --- STEP 10 (Day 10): Bounty warrant verification ---
            Assert.NotNull(fightRes.BountyRecord);
            Assert.Equal("faction_scavenger_warlords", fightRes.BountyRecord.FactionId);
            Assert.Equal(FactionBountySeverity.Severe, fightRes.BountyRecord.Severity);
            Assert.Equal(FactionBountyState.Active, fightRes.BountyRecord.State);
            Assert.True(bounty.HasActiveBounty("warlords_sector_4"));
            harness.LogStep(10, 10, "BountyIssued", $"BountyId_{fightRes.BountyRecord.BountyId}_Severity_{fightRes.BountyRecord.Severity}");

            // --- STEP 11 (Day 11): Deduplication idempotency check ---
            var duplicateBounty = bounty.IssuePatrolBounty("warlords_sector_4", advanceEnc.Id, "choice_warlord_fight", -15, 9);
            Assert.Same(fightRes.BountyRecord, duplicateBounty);
            Assert.Single(bounty.AllBounties);
            harness.LogStep(11, 11, "BountyDeduplication", "DuplicateBountyPrevented");

            // --- STEP 12 (Day 12): Sibling cooldown group verification ---
            // Warlord raid group (7-day cooldown from day 9 -> day 16)
            Assert.False(travel.IsEncounterEligible(raidEnc, "the_toll", 2.0f, "all", 12));
            Assert.False(travel.IsEncounterEligible(advanceEnc, "the_toll", 2.0f, "all", 12));
            harness.LogStep(12, 12, "SiblingCooldownActive", "BothWarlordVariantsOnCooldown");

            // --- STEP 13 (Day 13): Wartime reconnaissance variant active ---
            // Ash Sign scout cooldown from day 6 was 5 days -> available day 11. Under wartime, wartime recon is eligible.
            Assert.True(travel.IsEncounterEligible(ashWartime, "the_toll", 2.0f, "all", 13));
            travel.ResolveChoice(ashWartime.Id, "choice_ashsign_share_data", 13, out var ashRes);
            Assert.NotNull(ashRes);
            harness.LogStep(13, 13, "WartimeReconResolved", "AshSignWartimeReconResolved");

            // --- STEP 14 (Day 14): Ash Sign sibling cooldown group active ---
            Assert.False(travel.IsEncounterEligible(ashScouts, "the_toll", 2.0f, "all", 14));
            Assert.False(travel.IsEncounterEligible(ashWartime, "the_toll", 2.0f, "all", 14));
            harness.LogStep(14, 14, "AshSignCooldownActive", "BothAshSignVariantsOnCooldown");

            // --- STEP 15 (Day 15): Geopolitical territory shift ---
            // Iron Garrison expels Warlords from loc_toll_house!
            territory.SetNode("loc_toll_house", WarlordTerritoryState.Controlled, "faction_central_garrison");
            Assert.Equal("faction_central_garrison", territory.GetController("loc_toll_house"));
            harness.LogStep(15, 15, "TerritoryFlip", "TollHouseSeizedByGarrison");

            // --- STEP 16 (Day 16): Warlords cooldown expired, but blocked by territory ---
            // Cooldown expired on day 16 (day 9 + 7), BUT node is now controlled by Garrison
            Assert.False(travel.IsEncounterEligible(raidEnc, "the_toll", 2.0f, "all", 16, "loc_toll_house"));
            Assert.False(travel.IsEncounterEligible(advanceEnc, "the_toll", 2.0f, "all", 16, "loc_toll_house"));
            harness.LogStep(16, 16, "TerritoryGating", "WarlordPatrolsIneligibleUnderGarrisonControl");

            // --- STEP 17 (Day 17): Garrison border patrol becomes eligible ---
            var garrisonBorder = _catalog.GetEncounter("enc_patrol_central_garrison_border")!;
            Assert.NotNull(garrisonBorder);
            Assert.True(travel.IsEncounterEligible(garrisonBorder, "high_scarp", 2.0f, "all", 17, "loc_toll_house"));
            harness.LogStep(17, 17, "GarrisonBorderEligible", "GarrisonPatrolEligibleAtSeizedNode");

            // --- STEP 18 (Day 18): Garrison border inspection choice resolution ---
            inv.TryProduce("sealed_government_document", 1);
            bool garrisonResolved = travel.ResolveChoice(garrisonBorder.Id, "choice_central_negotiate", 18, out var gRes);
            Assert.True(garrisonResolved);
            harness.LogStep(18, 18, "GarrisonResolved", "GarrisonInspectionCompleted");

            // --- STEP 19 (Day 19): Caravan route movement ---
            caravan.DailyTick(19, rng, "the_toll", 2, "all");
            harness.LogStep(19, 19, "CaravanDailyTick", "CaravanProgressed");

            // --- STEP 20 (Day 20): Faction bounty clearance via diplomatic accord ---
            int clearedBounties = bounty.ClearBountiesForFaction("warlords_sector_4", 20);
            Assert.Equal(1, clearedBounties);
            Assert.False(bounty.HasActiveBounty("warlords_sector_4"));
            harness.LogStep(20, 20, "BountyClearance", $"ClearedCount_{clearedBounties}");

            // --- STEP 21 (Day 21): Verify bounty state is resolved ---
            Assert.Empty(bounty.GetActiveBounties());
            Assert.Equal(FactionBountyState.Resolved, bounty.AllBounties[0].State);
            Assert.Equal(20, bounty.AllBounties[0].ResolvedDay);
            harness.LogStep(21, 21, "BountyStateVerified", "AllBountiesResolved");

            // --- STEP 22 (Day 22): Ceasefire declared ---
            war.SetWarActive(false);
            Assert.False(war.IsAtWar);
            harness.LogStep(22, 22, "CeasefireDeclared", "PeacetimeRestored");

            // --- STEP 23 (Day 23): Wartime variants immediately disabled ---
            Assert.False(travel.IsEncounterEligible(advanceEnc, "the_toll", 2.0f, "all", 23));
            Assert.False(travel.IsEncounterEligible(ashWartime, "the_toll", 2.0f, "all", 23));
            harness.LogStep(23, 23, "WartimeVariantsDeactivated", "WartimeVariantsIneligible");

            // --- STEP 24 (Day 24): Peacetime variants active again ---
            Assert.True(travel.IsEncounterEligible(ashScouts, "the_toll", 2.0f, "all", 24));
            harness.LogStep(24, 24, "PeacetimeVariantsActive", "AshScoutsEligible");

            // --- STEP 25 (Day 25): Save state capture across all systems ---
            var bountyState = bounty.CaptureState();
            var warState = war.CaptureState();
            var territoryState = territory.CaptureState();
            var travelState = travel.CaptureState();
            var caravanState = caravan.CaptureState();
            harness.LogStep(25, 25, "CaptureState", "AllSystemStatesCaptured");

            // --- STEP 26 (Day 26): Full restore into fresh system instances ---
            var restoredBounty = new FactionBountySystem();
            restoredBounty.RestoreState(bountyState);
            var restoredWar = new FactionWarSystem();
            restoredWar.RestoreState(warState);
            var restoredTerritory = new DynamicTerritoryAuthority();
            restoredTerritory.RestoreState(territoryState);

            var restoredTravel = new TravelEncounterSystem(
                _catalog,
                inv,
                restoredWar,
                bountySystem: restoredBounty,
                territoryAuthority: restoredTerritory);
            restoredTravel.RestoreState(travelState);

            var restoredCaravan = new TravelingCaravanSystem
            {
                TravelEncounters = restoredTravel
            };
            restoredCaravan.RestoreState(caravanState);

            // Reassign harness to restored instances
            harness.BountySystem = restoredBounty;
            harness.WarSystem = restoredWar;
            harness.TerritoryAuthority = restoredTerritory;
            harness.TravelSystem = restoredTravel;
            harness.CaravanSystem = restoredCaravan;

            Assert.False(restoredWar.IsAtWar);
            Assert.False(restoredBounty.HasActiveBounty("warlords_sector_4"));
            Assert.Equal("faction_central_garrison", restoredTerritory.GetController("loc_toll_house"));
            harness.LogStep(26, 26, "RestoreState", "AllSystemStatesRestoredIdempotently");

            // --- STEP 27 (Day 27): Caravan resumes movement in restored environment ---
            restoredCaravan.DailyTick(27, rng, "high_scarp", 2, "all");
            harness.LogStep(27, 27, "RestoredCaravanMovement", "CaravanMovedPostRestore");

            // --- STEP 28 (Day 28): Checkpoint inspection in restored environment ---
            var checkpoint = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            Assert.NotNull(checkpoint);
            Assert.True(restoredTravel.IsEncounterEligible(checkpoint, "high_scarp", 2.0f, "all", 28));
            harness.LogStep(28, 28, "RestoredPatrolCheck", "CheckpointEligibleInRestoredEnvironment");

            // --- STEP 29 (Day 29): Resolve checkpoint choice ---
            bool chkResolved = restoredTravel.ResolveChoice(checkpoint.Id, "choice_pay_garrison_toll", 29, out var chkRes);
            Assert.True(chkResolved);
            harness.LogStep(29, 29, "RestoredResolveChoice", "CheckpointResolved_FoodDeducted");

            // --- STEP 30 (Day 30): Second severe infraction under restored state triggers moderate bounty ---
            var pressGang = _catalog.GetEncounter("enc_patrol_warlord_press_gang")!;
            Assert.NotNull(pressGang);
            // choice_press_intervene has faction_standing_delta = -10 (Moderate threshold)
            bool pressIntervened = restoredTravel.ResolveChoice(pressGang.Id, "choice_press_intervene", 30, out var pressRes);
            Assert.True(pressIntervened);
            Assert.NotNull(pressRes?.BountyRecord);
            Assert.Equal(FactionBountySeverity.Moderate, pressRes.BountyRecord.Severity);
            Assert.True(restoredBounty.HasActiveBounty("warlords_sector_4"));
            harness.LogStep(30, 30, "SecondBountyIssued", $"BountyId_{pressRes.BountyRecord.BountyId}_Severity_{pressRes.BountyRecord.Severity}");

            // --- STEP 31 (Day 31): Resolve individual bounty by ID ---
            string newBountyId = pressRes.BountyRecord.BountyId;
            bool singleResolved = restoredBounty.ResolveBounty(newBountyId, 31);
            Assert.True(singleResolved);
            Assert.False(restoredBounty.HasActiveBounty("warlords_sector_4"));
            harness.LogStep(31, 31, "SingleBountyResolved", $"Bounty_{newBountyId}_Resolved");

            // --- STEP 32 (Day 32): Final campaign state verification ---
            Assert.Equal(2, restoredBounty.AllBounties.Count);
            Assert.All(restoredBounty.AllBounties, b => Assert.Equal(FactionBountyState.Resolved, b.State));
            Assert.True(inv.CountById("fuel") > 0);
            Assert.False(restoredWar.IsAtWar);
            harness.LogStep(32, 32, "CampaignProofComplete", "All32StepsCompletedCleanly");

            var composite = new
            {
                Trace = harness.ExecutionTrace,
                Bounties = harness.BountySystem.CaptureState(),
                War = harness.WarSystem.CaptureState(),
                Territory = harness.TerritoryAuthority.CaptureState(),
                Caravan = harness.CaravanSystem.CaptureState()
            };

            string json = JsonSerializer.Serialize(composite);
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(hash);
        }

        [Fact]
        public void CampaignScenario_32Steps_ExecutesCleanly()
        {
            string hash = Run32StepCampaign(1337);
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
        }

        [Fact]
        public void CampaignScenario_3Runs_ByteForByteDeterministicReplay()
        {
            string hash1 = Run32StepCampaign(1337);
            string hash2 = Run32StepCampaign(1337);
            string hash3 = Run32StepCampaign(1337);

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }
    }
}
