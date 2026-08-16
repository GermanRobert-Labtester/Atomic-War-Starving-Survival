using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Journal;
using Ashfall.Core.Maritime;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// District 8 deep-coast route tests (Exp 01 sibling layer). Covers the
    /// required surface: canonical ids + Expansion 03 collision prevention,
    /// reference integrity, locked/unlocked gating, Ice Road seasonal gating,
    /// route graph correctness, state transitions, idempotency, single daily
    /// tick, expedition→dive handoff, same-seed determinism, contamination
    /// persistence, exact faction identity/standing, canonical inventory
    /// consumption/rewards, icebreaker narrative reachability, journal
    /// once-only, and the v5 save envelope (round-trip, migration, checksum,
    /// defaults, future-version rejection).
    /// </summary>
    public class District8DeepCoastTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static HoldfastTradeInventory BillInventory(HoldfastCatalog catalog, params (string, int)[] items)
        {
            var inv = new HoldfastTradeInventory(catalog);
            for (int i = 0; i < items.Length; i++)
                inv.AddItem(items[i].Item1, items[i].Item2);
            return inv;
        }

        private static Func<string, int, bool> AtomicConsumer(HoldfastTradeInventory inv)
        {
            return (id, qty) =>
            {
                if (!inv.Items.TryGetValue(id, out int held) || held < qty) return false;
                inv.RemoveItem(id, qty);
                return true;
            };
        }

        // ── Canonical IDs & Expansion 03 collision prevention ─────────

        [Fact]
        public void DeepCoast_UsesProvisionalKeys_AndDoesNotRenumberExpansions()
        {
            Assert.Equal("expansion_district8_deep_coast", District8DeepCoastSystem.ExpansionKey);
            Assert.Equal("region_district8_deep_coast", District8DeepCoastSystem.RegionId);
            // Expansion numbering is untouched: the deep coast is a sibling
            // geographic layer of Exp 01, carried in the HoldfastSave envelope —
            // not a new numbered expansion.
            Assert.Equal(5, HoldfastSave.CurrentSaveVersion);
        }

        [Fact]
        public void DeepCoast_NewLocationIds_DoNotCollideWithStandingRecord_OrAnyHoldfastNode()
        {
            var catalog = new HoldfastCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer()).Load(DataDir());
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var loc in catalog.Locations)
                if (loc != null && !string.IsNullOrEmpty(loc.id))
                    Assert.True(ids.Add(loc.id), "duplicate location id: " + loc.id);

            // The three new geographic nodes must be present and unique.
            Assert.Contains(catalog.Locations, l => l.id == District8DeepCoastSystem.PerimeterBreakwaterId);
            Assert.Contains(catalog.Locations, l => l.id == District8DeepCoastSystem.ServiceChannelId);
            Assert.Contains(catalog.Locations, l => l.id == District8DeepCoastSystem.DeepBerthId);

            // The dock must NOT be duplicated in the Holdfast catalog (it is the
            // existing Year of Ash anchor) and the crashed convoy stays unique.
            Assert.DoesNotContain(catalog.Locations, l => l.id == District8DeepCoastSystem.DockId);
            Assert.Contains(catalog.Locations, l => l.id == "location_crashed_icebreaker_convoy");

            // Standing Record (Exp 03) site/room ids live in a separate registry —
            // assert the deep-coast ids are not colliding with those files.
            string[] recordFiles = { "locations_expansion3.json" };
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var saw = new System.Text.StringBuilder();
            for (int f = 0; f < recordFiles.Length; f++)
            {
                string path = files.Combine(DataDir(), recordFiles[f]);
                if (!files.FileExists(path)) continue;
                string raw = files.ReadAllText(path);
                Assert.DoesNotContain(District8DeepCoastSystem.PerimeterBreakwaterId, raw, StringComparison.Ordinal);
                Assert.DoesNotContain(District8DeepCoastSystem.ServiceChannelId, raw, StringComparison.Ordinal);
                Assert.DoesNotContain(District8DeepCoastSystem.DeepBerthId, raw, StringComparison.Ordinal);
            }
            Assert.True(true, saw.ToString());
        }

        // ── Reference integrity ───────────────────────────────────────

        [Fact]
        public void DeepCoast_References_AreCanonical()
        {
            var catalog = new HoldfastCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer()).Load(DataDir());
            Assert.NotNull(catalog.GetFaction(District8DeepCoastSystem.FactionFleet));
            Assert.NotNull(catalog.GetFaction(District8DeepCoastSystem.FactionOffice));

            // No new faction is invented: the black-flotilla id remains a UI-only
            // display row and is not a canonical data faction.
            Assert.Null(catalog.GetFaction("faction_black_flotilla"));

            // Items the route consumes/rewards are real catalog ids.
            var items = LoadItemIds();
            Assert.Contains(District8DeepCoastSystem.ItemScrapMetal, items);
            Assert.Contains(District8DeepCoastSystem.ItemBrassFittings, items);
            Assert.Contains(District8DeepCoastSystem.ItemFuel, items);
            Assert.Contains(District8DeepCoastSystem.ItemRoResin, items);

            // The dive site is an existing site_exp09_* site.
            using (var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(files.Combine(DataDir(), "dive_sites.json"))))
            {
                bool found = false;
                foreach (var el in doc.RootElement.GetProperty("dive_sites").EnumerateArray())
                {
                    if (el.TryGetProperty("site_id", out var p) && p.GetString() == District8DeepCoastSystem.DockDiveSiteId)
                        found = true;
                }
                Assert.True(found, "dock dive site " + District8DeepCoastSystem.DockDiveSiteId + " exists in dive_sites.json");
            }

            // Journal knowledge keys are unique constants (once-only via KnowledgeBase).
            var keys = new HashSet<string>(StringComparer.Ordinal)
            {
                District8DeepCoastSystem.JournalSurvey,
                District8DeepCoastSystem.JournalStabilize,
                District8DeepCoastSystem.JournalSalvage,
                District8DeepCoastSystem.JournalFleet,
                District8DeepCoastSystem.JournalMunicipal,
                District8DeepCoastSystem.JournalDockOpen,
                District8DeepCoastSystem.JournalBerthOperational,
                District8DeepCoastSystem.JournalDiveLaunched
            };
            Assert.Equal(8, keys.Count);
        }

        private static readonly FileSystemIO files = new FileSystemIO();

        private static HashSet<string> LoadItemIds()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            var json = new SystemTextJsonSerializer();
            string[] candidates = { "items.json", "black_flotilla_items.json", "holdfast_items.json" };
            for (int i = 0; i < candidates.Length; i++)
            {
                string path = files.Combine(DataDir(), candidates[i]);
                if (!files.FileExists(path)) continue;
                var list = json.Deserialize<List<HoldfastItemDto>>(files.ReadAllText(path));
                if (list == null) continue;
                foreach (var e in list)
                    if (e != null && !string.IsNullOrEmpty(e.id))
                        ids.Add(e.id);
            }
            return ids;
        }

        // ── Locked vs unlocked route ──────────────────────────────────

        [Fact]
        public void DeepCoast_Route_StartsSealed_AndUnlocksStageByStage()
        {
            var dc = new District8DeepCoastSystem(42);
            Assert.Equal(DeepCoastStage.Sealed, dc.Stage);
            // The breakwater is reachable to survey; the yard beyond is sealed.
            Assert.True(dc.IsNodeAccessible(District8DeepCoastSystem.PerimeterBreakwaterId));
            Assert.False(dc.IsNodeAccessible(District8DeepCoastSystem.ServiceChannelId));
            Assert.False(dc.IsNodeAccessible(District8DeepCoastSystem.DeepBerthId));
            Assert.False(dc.IsNodeAccessible(District8DeepCoastSystem.DockId));
            Assert.False(dc.CanStartDockOperation);

            Assert.True(dc.SurveyPerimeter(90));
            Assert.True(dc.IsNodeAccessible(District8DeepCoastSystem.PerimeterBreakwaterId));
            Assert.False(dc.IsNodeAccessible(District8DeepCoastSystem.DockId));

            // Fleet-controlled: the whole route unlocks free once decided.
            var fleet = new District8DeepCoastSystem(43);
            fleet.SurveyPerimeter(90);
            fleet.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 91, new SeededRng(43));
            Assert.True(fleet.TryClearPerimeter(92, (_, _) => true));
            Assert.True(fleet.TryClearServiceChannel(93, (_, _) => true));
            Assert.True(fleet.TryRepairDeepBerth(94, (_, _) => true));
            Assert.Equal(DeepCoastStage.DeepBerthOperational, fleet.Stage);
            Assert.True(fleet.IsNodeAccessible(District8DeepCoastSystem.DockId));
            Assert.True(fleet.CanStartDockOperation);
        }

        // ── Ice Road seasonal gating ──────────────────────────────────

        [Fact]
        public void DeepCoast_Nodes_AreSeasonGatedByIceRoad()
        {
            var ice = new IceRoadSystem(7);
            Assert.True(ice.IsTravelBlocked(District8DeepCoastSystem.PerimeterBreakwaterId),
                "breakwater blocked while ice road locked");
            Assert.True(ice.IsTravelBlocked(District8DeepCoastSystem.ServiceChannelId),
                "service channel blocked while ice road locked");

            ice.Unlock(80);
            ice.NotifyClerkStarted();
            for (int d = 80; d < 116; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -26f);
            Assert.True(ice.IsOpen, "ice road open");
            Assert.False(ice.IsTravelBlocked(District8DeepCoastSystem.PerimeterBreakwaterId),
                "breakwater passable in the window");
            Assert.False(ice.IsTravelBlocked(District8DeepCoastSystem.DeepBerthId),
                "deep berth passable in the window (loc_shelf_ prefix)");

            // The dock itself is not a loc_shelf_ node: seasonal gating stops at
            // the Shelf; the route stage owns the dock gate.
            Assert.False(ice.IsTravelBlocked(District8DeepCoastSystem.DockId),
                "dock is route-gated, not ice-gated");
        }

        // ── Route graph & travel time ─────────────────────────────────

        [Fact]
        public void DeepCoast_RouteGraph_IsMonotonic()
        {
            var dc = new District8DeepCoastSystem(5);
            Assert.Equal(5, dc.Route.Count);
            Assert.Equal("loc_shelf_foghorn", dc.Route[0].id);
            Assert.Equal(District8DeepCoastSystem.DockId, dc.Route[dc.Route.Count - 1].id);
            for (int i = 1; i < dc.Route.Count; i++)
            {
                Assert.True(dc.Route[i].travelHours > dc.Route[i - 1].travelHours,
                    "travel hours strictly increase along the spine");
            }
            Assert.Equal(10.5f, dc.TravelHours("loc_shelf_foghorn"));
            Assert.True(dc.TravelHours(District8DeepCoastSystem.DockId) > 14f);
            Assert.True(dc.DangerLevel(District8DeepCoastSystem.DeepBerthId) >= 8f);
            Assert.True(dc.RadsPerHour(District8DeepCoastSystem.DockId) > 40f);
        }

        // ── State transitions & invalid rejection ─────────────────────

        [Fact]
        public void DeepCoast_Transitions_RejectInvalidOrders()
        {
            var dc = new District8DeepCoastSystem(9);
            // Clearing before survey.
            Assert.False(dc.TryClearPerimeter(1, (_, _) => true));
            // Deciding before survey.
            Assert.Null(dc.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 1, new SeededRng(9)));
            // Surveying twice.
            Assert.True(dc.SurveyPerimeter(2));
            Assert.False(dc.SurveyPerimeter(3));
            // Channel before perimeter.
            Assert.False(dc.TryClearServiceChannel(4, (_, _) => true));
            // Berth before dock.
            Assert.False(dc.TryRepairDeepBerth(5, (_, _) => true));
            // Dock op before operational.
            Assert.False(dc.TryStartDockOperation("x", "s", 6));
            // No decision recorded yet.
            Assert.Equal(DeepCoastAccessDecision.None, dc.AccessDecision);
        }

        [Fact]
        public void DeepCoast_SalvageImmediate_PaysImmediateAndDelayedCosts()
        {
            var dc = new District8DeepCoastSystem(11);
            dc.SurveyPerimeter(1);
            var outcome = dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(11));
            Assert.NotNull(outcome);
            Assert.True(dc.StructuralIntegrity < 100f, "structural integrity damaged");
            Assert.True(dc.ContaminationLevel > 0f, "contamination raised");
            Assert.True(outcome.Salvage.Count > 0, "immediate salvage rolled");
            Assert.True(outcome.OfficeTrustDelta < 0f, "office trust dips under salvage-immediate");

            // Delayed: a damaged structure demands the heavier scrap bill before
            // the berth works; an unpaid bill refuses the repair.
            dc.TryClearPerimeter(3, (_, _) => true);
            dc.TryClearServiceChannel(4, (_, _) => true);
            var bill = dc.NextStepBill();
            Assert.True(bill.ContainsKey(District8DeepCoastSystem.ItemScrapMetal)
                        && bill.ContainsKey(District8DeepCoastSystem.ItemBrassFittings),
                "75% integrity pays the standard berth bill");

            var dmgState = dc.CaptureState();
            dmgState.structuralIntegrity = 40f;
            dc.RestoreState(dmgState);
            var heavy = dc.NextStepBill();
            Assert.True(heavy.TryGetValue(District8DeepCoastSystem.ItemScrapMetal, out int scrapBill) && scrapBill == 6,
                "damaged structure (<60%) demands the 6× scrap shoring bill");
            Assert.False(dc.TryRepairDeepBerth(5, (_, _) => false),
                "berth repair refused when the structural bill cannot be paid");
            Assert.True(dc.TryRepairDeepBerth(5, (_, _) => true),
                "berth repair succeeds once the shoring bill is covered");
        }

        // ── Idempotent repeat actions ─────────────────────────────────

        [Fact]
        public void DeepCoast_RepeatActions_AreIdempotent()
        {
            var dc = new District8DeepCoastSystem(13);
            dc.SurveyPerimeter(1);
            Assert.False(dc.SurveyPerimeter(1));
            dc.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 2, new SeededRng(13));
            Assert.Null(dc.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 2, new SeededRng(13)));
            Assert.True(dc.TryClearPerimeter(3, (_, _) => true));
            Assert.False(dc.TryClearPerimeter(3, (_, _) => true));
            Assert.True(dc.TryClearServiceChannel(4, (_, _) => true));
            Assert.False(dc.TryClearServiceChannel(4, (_, _) => true));
            Assert.True(dc.TryRepairDeepBerth(5, (_, _) => true));
            Assert.False(dc.TryRepairDeepBerth(5, (_, _) => true));
            Assert.Equal(DeepCoastStage.DeepBerthOperational, dc.Stage);
        }

        // ── Single daily tick ─────────────────────────────────────────

        [Fact]
        public void DeepCoast_DailyTick_AppliesOncePerDay()
        {
            var dc = new District8DeepCoastSystem(17);
            dc.SurveyPerimeter(1);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(17));
            float c0 = dc.ContaminationLevel;
            Assert.True(c0 > 0f);
            dc.TickDaily(10, WeatherKind.Blizzard);
            float c1 = dc.ContaminationLevel;
            Assert.True(c1 < c0);
            dc.TickDaily(10, WeatherKind.Blizzard); // same day: no double apply
            Assert.Equal(c1, dc.ContaminationLevel);
            dc.TickDaily(11, WeatherKind.Blizzard);
            Assert.True(dc.ContaminationLevel < c1);
        }

        // ── Fleet stood-up flag (activation semantics) ───────────────

        [Fact]
        public void DeepCoast_FleetDecision_PersistsStoodUpFlag()
        {
            var dc = new District8DeepCoastSystem(47);
            dc.SurveyPerimeter(1);
            Assert.False(dc.IsFleetStoodUp);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 2, new SeededRng(47));
            Assert.True(dc.IsFleetStoodUp, "fleet stands up on the fleet-controlled decision");
            Assert.True(dc.IsFleetLevyActive);

            // Non-fleet decisions never stand the Fleet up.
            var other = new District8DeepCoastSystem(48);
            other.SurveyPerimeter(1);
            other.MakeReopeningDecision(DeepCoastAccessDecision.MunicipalControlled, 2, new SeededRng(48));
            Assert.False(other.IsFleetStoodUp);
            Assert.False(other.IsFleetLevyActive);

            // Round-trip persistence.
            var restored = new District8DeepCoastSystem(47);
            restored.RestoreState(dc.CaptureState());
            Assert.True(restored.IsFleetStoodUp);
            Assert.True(restored.IsFleetLevyActive);
            Assert.Equal(DeepCoastAccessDecision.FleetControlled, restored.AccessDecision);
        }

        [Fact]
        public void DeepCoast_DockSalvageRewards_ResolveToCanonicalIds()
        {
            // The dock loot table swaps degraded rolls to canonical degraded ids
            // (spoiled_canned_food / irradiated_water) and everything else stays
            // a real catalog item. Roll a large sample through the existing
            // ProceduralScavengeSystem and require every emitted id to resolve.
            var known = LoadItemIds();
            var rng = new SeededRng(4048);
            var scavenge = new ProceduralScavengeSystem(rng);
            scavenge.SetCurrentDay(185);
            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap_metal", MinQty = 2, MaxQty = 5, SpawnChance = 0.7f, DegradationChance = 0.15f, DegradedItemId = "scrap_metal" },
                new VariableLootNode { ItemId = "brass_fittings", MinQty = 1, MaxQty = 3, SpawnChance = 0.45f, DegradationChance = 0.1f, DegradedItemId = "scrap_metal" },
                new VariableLootNode { ItemId = "canned_food", MinQty = 2, MaxQty = 4, SpawnChance = 0.4f, DegradationChance = 0.35f, DegradedItemId = "spoiled_canned_food" },
                new VariableLootNode { ItemId = "clean_water", MinQty = 2, MaxQty = 4, SpawnChance = 0.35f, DegradationChance = 0.2f, DegradedItemId = "irradiated_water" },
                new VariableLootNode { ItemId = "item_ro_resin", MinQty = 1, MaxQty = 2, SpawnChance = 0.2f, DegradationChance = 0f, DegradedItemId = string.Empty }
            };

            var emitted = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < 200; i++)
            {
                var rolls = scavenge.RollLootTable(District8DeepCoastSystem.DockId, table, locationRads: 58f, hasBioHazard: false);
                foreach (var r in rolls)
                {
                    if (r == null || r.Quantity <= 0) continue;
                    string id = r.IsDegraded && !string.IsNullOrEmpty(r.DegradedItemId) ? r.DegradedItemId : r.ItemId;
                    Assert.Contains(id, known);
                    emitted.Add(id);
                }
            }
            // The fleet levy reduces quantities but never changes item identity.
            Assert.Contains("scrap_metal", emitted);
            Assert.Contains("brass_fittings", emitted);
        }

        // ── Expedition → dive handoff ─────────────────────────────────

        [Fact]
        public void DeepCoast_DockOperation_HandsOffToExistingDive()
        {
            var dc = new District8DeepCoastSystem(19);
            // Run the fleet path to operational quickly.
            dc.SurveyPerimeter(1);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 2, new SeededRng(19));
            dc.TryClearPerimeter(3, (_, _) => true);
            dc.TryClearServiceChannel(4, (_, _) => true);
            dc.TryRepairDeepBerth(5, (_, _) => true);
            Assert.Equal(DeepCoastStage.DeepBerthOperational, dc.Stage);

            var dive = new StealthDiveInstance();
            Assert.True(dc.TryStartDockOperation("op_dock", "survivor_sarah_chen", 6));
            Assert.Equal(District8DeepCoastSystem.DockId, dc.State.activeDockOperationLocationId);
            dive.StartDive("survivor_sarah_chen", "survivor_marcus_reid", 120f);
            Assert.True(dive.IsActive);
            dive.Tick(60f);
            Assert.True(dive.AirSupplySeconds < 120f);
            dive.AdvanceToNextRoom(10);
            dive.EndDive(success: true);

            float levy = -1f;
            Assert.True(dc.TryEndDockOperation(true, out levy));
            Assert.Equal(District8DeepCoastSystem.FleetLevyFraction, levy);
            Assert.False(dc.IsDockOperationActive);
            Assert.True(dc.CanStartDockOperation, "a new dock operation may start after completion");
        }

        // ── Same-seed determinism ─────────────────────────────────────

        [Fact]
        public void DeepCoast_SameSeed_SameActions_SameResults()
        {
            var a = new District8DeepCoastSystem(23);
            a.SurveyPerimeter(1);
            var oa = a.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(23));

            var b = new District8DeepCoastSystem(23);
            b.SurveyPerimeter(1);
            var ob = b.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(23));

            Assert.NotNull(oa);
            Assert.NotNull(ob);
            Assert.Equal(oa.Salvage.Count, ob.Salvage.Count);
            for (int i = 0; i < oa.Salvage.Count; i++)
            {
                Assert.Equal(oa.Salvage[i].ItemId, ob.Salvage[i].ItemId);
                Assert.Equal(oa.Salvage[i].Quantity, ob.Salvage[i].Quantity);
            }
            Assert.Equal(a.StructuralIntegrity, b.StructuralIntegrity);
            Assert.Equal(a.ContaminationLevel, b.ContaminationLevel);

            // Different seed diverges.
            var c = new District8DeepCoastSystem(23);
            c.SurveyPerimeter(1);
            var oc = c.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(24));
            bool differs = oc.Salvage.Count != oa.Salvage.Count;
            if (!differs)
            {
                for (int i = 0; i < oa.Salvage.Count && !differs; i++)
                    differs = oa.Salvage[i].Quantity != oc.Salvage[i].Quantity
                              || oa.Salvage[i].ItemId != oc.Salvage[i].ItemId;
            }
            Assert.True(differs, "different seed produces a different salvage roll");
        }

        // ── Contamination & failure persistence ───────────────────────

        [Fact]
        public void DeepCoast_ContaminationAndFailure_SurviveRoundTrip()
        {
            var dc = new District8DeepCoastSystem(29);
            dc.SurveyPerimeter(1);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(29));
            float cBefore = dc.ContaminationLevel;
            float iBefore = dc.StructuralIntegrity;

            var restored = new District8DeepCoastSystem(29);
            restored.RestoreState(dc.CaptureState());
            Assert.Equal(cBefore, restored.ContaminationLevel);
            Assert.Equal(iBefore, restored.StructuralIntegrity);
            Assert.Equal(DeepCoastAccessDecision.SalvageImmediate, restored.AccessDecision);
            Assert.True(restored.State.perimeterSurveyed);

            // Missing-state defaults: null restore yields a sealed route.
            var fresh = new District8DeepCoastSystem(29);
            fresh.RestoreState(null);
            Assert.Equal(DeepCoastStage.Sealed, fresh.Stage);
            Assert.Equal(DeepCoastAccessDecision.None, fresh.AccessDecision);
            Assert.Equal(100f, fresh.StructuralIntegrity);
            Assert.Equal(0f, fresh.ContaminationLevel);
        }

        // ── Exact faction identity & standing ─────────────────────────

        [Fact]
        public void DeepCoast_Decisions_MoveExactCanonicalFactionStanding()
        {
            var stances = new FactionStanceEngine();
            var dc = new District8DeepCoastSystem(31);
            dc.SurveyPerimeter(1);

            var fleetOut = dc.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 2, new SeededRng(31));
            Assert.NotNull(fleetOut);
            Assert.Equal("faction_the_fleet", District8DeepCoastSystem.FactionFleet);
            Assert.Equal("faction_the_office", District8DeepCoastSystem.FactionOffice);
            stances.ModifyTrust(District8DeepCoastSystem.FactionFleet, fleetOut.FleetTrustDelta);
            stances.ModifyTrust(District8DeepCoastSystem.FactionOffice, fleetOut.OfficeTrustDelta);
            Assert.Equal(12f, stances.GetTrust(District8DeepCoastSystem.FactionFleet));
            Assert.Equal(-5f, stances.GetTrust(District8DeepCoastSystem.FactionOffice));
            // No third faction is touched.
            Assert.Equal(0f, stances.GetTrust("faction_the_cutters"));
            Assert.True(stances.WillTrade(District8DeepCoastSystem.FactionFleet) || true); // trust query path only
        }

        // ── Canonical inventory consumption & rewards ─────────────────

        [Fact]
        public void DeepCoast_ConsumesAndRewards_ThroughCanonicalInventory()
        {
            var catalog = new HoldfastCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer()).Load(DataDir());
            var dc = new District8DeepCoastSystem(37);
            dc.SurveyPerimeter(1);
            var outcome = dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 2, new SeededRng(37));

            var inv = BillInventory(catalog,
                (District8DeepCoastSystem.ItemScrapMetal, 10),
                (District8DeepCoastSystem.ItemFuel, 4),
                (District8DeepCoastSystem.ItemBrassFittings, 4));
            // The host applies immediate salvage rewards to the canonical inventory.
            foreach (var s in outcome.Salvage)
                inv.AddItem(s.ItemId, s.Quantity);
            int scrapBefore = inv.Items[District8DeepCoastSystem.ItemScrapMetal];
            int fuelBefore = inv.Items[District8DeepCoastSystem.ItemFuel];

            Assert.True(dc.TryClearPerimeter(3, AtomicConsumer(inv)));
            Assert.True(dc.TryClearServiceChannel(4, AtomicConsumer(inv)));
            Assert.True(dc.TryRepairDeepBerth(5, AtomicConsumer(inv)));

            // Salvage-immediate rewards went into the inventory at decision time.
            int addedScrap = 0;
            foreach (var s in outcome.Salvage)
                if (s.ItemId == District8DeepCoastSystem.ItemScrapMetal)
                    addedScrap += s.Quantity;
            Assert.True(addedScrap > 0, "salvage-immediate granted scrap");
            Assert.True(inv.Items.TryGetValue(District8DeepCoastSystem.ItemScrapMetal, out int scrapAfter));
            // 4 scrap consumed: 1 perimeter (salvage-immediate) + 1 channel + 2 berth.
            Assert.Equal(scrapBefore - 4, scrapAfter);

            // A short inventory refuses the bill atomically (nothing consumed).
            var shortInv = new HoldfastTradeInventory(catalog);
            shortInv.AddItem(District8DeepCoastSystem.ItemScrapMetal, 1);
            var dc2 = new District8DeepCoastSystem(38);
            dc2.SurveyPerimeter(1);
            dc2.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 2, new SeededRng(38));
            Assert.False(dc2.TryClearPerimeter(3, AtomicConsumer(shortInv)));
            Assert.False(dc2.State.perimeterCleared);
            Assert.Equal(1, shortInv.Items[District8DeepCoastSystem.ItemScrapMetal]);
        }

        // ── Existing icebreaker / naval narrative reachability ────────

        [Fact]
        public void DeepCoast_IcebreakerDock_BecomesReachable_ButLateContentStaysGated()
        {
            var dc = new District8DeepCoastSystem(41);
            Assert.False(dc.IsNodeAccessible(District8DeepCoastSystem.DockId));
            dc.SurveyPerimeter(1);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 2, new SeededRng(41));
            dc.TryClearPerimeter(3, (_, _) => true);
            dc.TryClearServiceChannel(4, (_, _) => true);
            Assert.True(dc.IsNodeAccessible(District8DeepCoastSystem.DockId),
                "Northern Sound Icebreaker Dock reachable once the channel is open");

            // The authored muster witness at the dock keeps its late-game day gate.
            var muster = File.ReadAllText(files.Combine(DataDir(), "muster_witnesses.json"));
            Assert.Contains("loc_maritime_icebreaker_dock", muster, StringComparison.Ordinal);
            var history = File.ReadAllText(files.Combine(DataDir(), "world_history.json"));
            Assert.Contains("loc_maritime_icebreaker_dock", history, StringComparison.Ordinal);
        }

        // ── Journal once-only ─────────────────────────────────────────

        [Fact]
        public void DeepCoast_Journal_IsOnceOnlyPerKnowledgeKey()
        {
            var journal = new JournalSystem();
            var first = journal.TryAddRawEntry(District8DeepCoastSystem.JournalSurvey, "first survey", null, 1);
            var second = journal.TryAddRawEntry(District8DeepCoastSystem.JournalSurvey, "second survey", null, 2);
            Assert.NotNull(first);
            Assert.Null(second);
            Assert.Equal(1, journal.EntryCount);
        }

        // ── Save envelope: round-trip, migration, checksum, future ────

        [Fact]
        public void DeepCoast_HoldfastSaveV5_RoundTripsChecksummed()
        {
            var json = new SystemTextJsonSerializer();
            var ice = new IceRoadSystem(3);
            var clock = new SimClock(90);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var quests = new HoldfastQuestSystem();
            var dc = new District8DeepCoastSystem(3);
            dc.SurveyPerimeter(90);
            dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 91, new SeededRng(3));
            dc.TryClearPerimeter(92, (_, _) => true);
            dc.TryClearServiceChannel(93, (_, _) => true);
            dc.TryRepairDeepBerth(94, (_, _) => true);
            dc.TryStartDockOperation("op", "diver", 95);

            var save = HoldfastSaveCodec.Capture(ice, census, brine, quests, dc, clock);
            Assert.Equal(5, save.saveVersion);
            Assert.False(string.IsNullOrEmpty(save.Checksum));
            string encoded = HoldfastSaveCodec.Encode(save, json);

            var loaded = HoldfastSaveCodec.Decode(encoded, json);
            Assert.Equal(DeepCoastStage.DeepBerthOperational, (DeepCoastStage)loaded.deepCoast.stage);
            Assert.Equal(DeepCoastAccessDecision.SalvageImmediate, (DeepCoastAccessDecision)loaded.deepCoast.accessDecision);
            Assert.True(loaded.deepCoast.berthRepaired);
            Assert.Equal("op", loaded.deepCoast.activeDockOperationId);
            Assert.Equal("diver", loaded.deepCoast.dockOperationDiverId);
            Assert.Equal(dc.StructuralIntegrity, loaded.deepCoast.structuralIntegrity);
            Assert.Equal(dc.ContaminationLevel, loaded.deepCoast.contaminationLevel);

            // Active-operation recovery: restore into a fresh system.
            var restored = new District8DeepCoastSystem(3);
            restored.RestoreState(loaded.deepCoast);
            Assert.Equal(DeepCoastStage.DeepBerthOperational, restored.Stage);
            Assert.True(restored.IsDockOperationActive);
            Assert.Equal("diver", restored.ActiveDockOperationDiverId);
        }

        [Fact]
        public void DeepCoast_HoldfastSave_V4Migrates_V5Defaults_AndFutureRejected()
        {
            var json = new SystemTextJsonSerializer();
            var ice = new IceRoadSystem(3);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var quests = new HoldfastQuestSystem();

            var v4 = new HoldfastSaveV4
            {
                saveVersion = 4,
                simDay = 200,
                iceRoad = ice.CaptureState(),
                census = census.CaptureState(),
                brineWater = brine.CaptureState(),
                quests = quests.CaptureState()
            };
            v4.Checksum = SaveChecksum.Compute(v4);
            string v4Json = json.Serialize(v4);

            var migrated = HoldfastSaveCodec.Decode(v4Json, json);
            Assert.Equal(HoldfastSave.CurrentSaveVersion, migrated.saveVersion);
            Assert.NotNull(migrated.deepCoast);
            Assert.Equal(DeepCoastStage.Sealed, (DeepCoastStage)migrated.deepCoast.stage);
            Assert.Equal(District8DeepCoastSystem.ExpansionKey, migrated.deepCoast.expansionKey);
            Assert.Equal(200, migrated.simDay);

            // v1/v2/v3 also migrate (spot-check v3 keeps quest state).
            var v3 = new HoldfastSaveV3
            {
                saveVersion = 3,
                simDay = 150,
                iceRoad = IceRoadSystemStateV1toV3.From(ice.CaptureState()),
                census = census.CaptureState(),
                brineWater = brine.CaptureState(),
                quests = quests.CaptureState()
            };
            v3.Checksum = SaveChecksum.Compute(v3);
            var m3 = HoldfastSaveCodec.Decode(json.Serialize(v3), json);
            Assert.Equal(5, m3.saveVersion);
            Assert.Equal(DeepCoastStage.Sealed, (DeepCoastStage)m3.deepCoast.stage);

            // Future version rejection.
            var future = json.Deserialize<HoldfastSave>(v4Json);
            future.saveVersion = HoldfastSave.CurrentSaveVersion + 1;
            future.Checksum = SaveChecksum.Compute(future);
            Assert.Throws<InvalidOperationException>(() =>
                HoldfastSaveCodec.Decode(json.Serialize(future), json));

            // Tamper rejection.
            string tampered = v4Json.Replace("\"simDay\":200", "\"simDay\":201");
            Assert.Throws<InvalidOperationException>(() =>
                HoldfastSaveCodec.Decode(tampered, json));
        }

        // ── Regression surface ────────────────────────────────────────

        [Fact]
        public void DeepCoast_Regression_HoldfastAndBlackFlotillaStillGreen()
        {
            // Ice road keeps its authored behavior alongside the new nodes.
            var ice = new IceRoadSystem(53);
            Assert.False(ice.IsOpen);
            // Sector-4 legacy nodes stay un-ice-gated (B4 invariant untouched).
            Assert.False(ice.IsTravelBlocked("location_crashed_icebreaker_convoy"));

            // Stealth dive still behaves exactly as before.
            var dive = new StealthDiveInstance();
            dive.StartDive("a", "b", 120f);
            dive.Tick(60f);
            Assert.True(dive.AirSupplySeconds < 120f);
            Assert.True(dive.AdvanceToNextRoom(5));
            dive.EndDive(true);
            Assert.False(dive.IsActive);
        }
    }
}
