using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Ashfall.Core.Economy;
using Ashfall.Core.Journal;
using Ashfall.Core.Maritime;

using Ashfall.Core.IO;
namespace Ashfall.Core
{
    /// <summary>
    /// Vertical-slice smoke for the District 8 deep-coast route (Exp 01 sibling
    /// layer): sealed → surveyed → perimeter_open → dock_accessible →
    /// deep_berth_operational, with the meaningful reopening decision, seasonal
    /// gating via IceRoadSystem, canonical inventory consumption/rewards,
    /// faction standing via FactionStanceEngine, once-only journal keys, a
    /// dock-operation handoff into the existing StealthDiveInstance, and a
    /// HoldfastSave v5 round-trip with v4 migration + future-version rejection.
    /// Invoked by `dotnet test` and by Godot `-- --deep-coast-selftest`.
    /// </summary>
    public static class DeepCoastHeadlessDemo
    {
        public const int DefaultSeed = 4048;

        public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) report.PassedCount++;
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
                if (condition) log.Info("[PASS] " + name);
            }

            log.Info("[DeepCoastHeadlessDemo] begin");

            // ── Catalog + canonical ID checks ──────────────────────────
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log);
            var catalog = loader.Load(dataDirectory, expansionUnlocked: true);
            Check(catalog.GetLocation(District8DeepCoastSystem.PerimeterBreakwaterId) != null,
                "breakwater node authored in holdfast_locations.json");
            Check(catalog.GetLocation(District8DeepCoastSystem.ServiceChannelId) != null,
                "service channel node authored");
            Check(catalog.GetLocation(District8DeepCoastSystem.DeepBerthId) != null,
                "deep berth node authored");
            Check(catalog.GetLocation(District8DeepCoastSystem.RouteStartId) != null,
                "route start (loc_shelf_foghorn) is an existing Shelf node");
            Check(catalog.GetLocation(District8DeepCoastSystem.DockId) == null,
                "dock is NOT duplicated in holdfast_locations (stays the existing year_of_ash anchor)");
            Check(catalog.GetFaction(District8DeepCoastSystem.FactionFleet) != null,
                "faction_the_fleet is canonical");
            Check(catalog.GetFaction(District8DeepCoastSystem.FactionOffice) != null,
                "faction_the_office is canonical");
            Check(IsKnownGood(District8DeepCoastSystem.ItemScrapMetal),
                "scrap_metal exists in an item catalog");
            Check(IsKnownGood(District8DeepCoastSystem.ItemBrassFittings),
                "brass_fittings exists in an item catalog");
            Check(IsKnownGood(District8DeepCoastSystem.ItemFuel),
                "fuel exists in an item catalog");
            Check(IsKnownGood(District8DeepCoastSystem.ItemRoResin),
                "item_ro_resin exists in an item catalog");

            // ── Locked route behaviour ─────────────────────────────────
            var dc = new District8DeepCoastSystem(DefaultSeed);
            Check(dc.Stage == DeepCoastStage.Sealed, "route starts sealed");
            Check(dc.IsNodeAccessible(District8DeepCoastSystem.PerimeterBreakwaterId),
                "breakwater reachable while sealed (the survey trip is the first expedition)");
            Check(!dc.IsNodeAccessible(District8DeepCoastSystem.ServiceChannelId),
                "service channel locked while sealed");
            Check(!dc.IsNodeAccessible(District8DeepCoastSystem.DockId), "dock locked while sealed");
            Check(!dc.CanStartDockOperation, "no dock operation while sealed");

            // ── Ice road seasonal gating (existing system, new nodes) ──
            var ice = new IceRoadSystem(DefaultSeed);
            Check(ice.IsTravelBlocked(District8DeepCoastSystem.PerimeterBreakwaterId),
                "deep-coast node season-blocked while ice road closed (loc_shelf_ prefix)");
            ice.Unlock(90);
            ice.NotifyClerkStarted();
            for (int d = 90; d < 124; d++)
                ice.TickDaily(d, WeatherKind.Blizzard, -24f);
            Check(ice.IsOpen, "ice road open window (clerk + freeze)");
            Check(!ice.IsTravelBlocked(District8DeepCoastSystem.PerimeterBreakwaterId),
                "deep-coast node passable while ice road open");

            // ── Stage machine + decision ───────────────────────────────
            Check(dc.SurveyPerimeter(124), "survey accepts on sealed route");
            Check(!dc.SurveyPerimeter(124), "survey is once-only");
            Check(dc.Stage == DeepCoastStage.Surveyed, "stage is surveyed");
            Check(dc.IsNodeAccessible(District8DeepCoastSystem.PerimeterBreakwaterId),
                "breakwater accessible once surveyed");

            var rngA = new SeededRng(DefaultSeed);
            var outcome = dc.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 125, rngA);
            if (outcome == null) { log.Warn("DeepCoastHeadlessDemo: MakeReopeningDecision returned null — aborting route test"); return report; }
            Check(outcome != null, "reopening decision accepted");
            Check(dc.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 125, new SeededRng(DefaultSeed)) == null,
                "second decision rejected (one decision per route)");
            Check(outcome!.Salvage.Count > 0, "salvage-immediate rolls immediate salvage");
            Check(dc.StructuralIntegrity < 100f, "salvage-immediate damages the structure");
            Check(dc.ContaminationLevel > 0f, "salvage-immediate raises contamination");
            Check(dc.AccessDecision == DeepCoastAccessDecision.SalvageImmediate, "decision recorded");

            // Determinism: same seed + same actions ⇒ same salvage.
            var dc2 = new District8DeepCoastSystem(DefaultSeed);
            dc2.SurveyPerimeter(124);
            var out2 = dc2.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 125, new SeededRng(DefaultSeed));
            if (out2 == null) { log.Warn("DeepCoastHeadlessDemo: second decision null — aborting"); return report; }
            Check(SameSalvage(outcome.Salvage, out2.Salvage), "same-seed salvage determinism");

            // Invalid transition rejection.
            var dc3 = new District8DeepCoastSystem(DefaultSeed);
            Check(dc3.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 1, new SeededRng(DefaultSeed)) == null,
                "decision rejected before survey");
            Check(!dc3.TryClearPerimeter(1, (_, _) => true), "perimeter clear rejected before survey");

            // ── Material consumption through the canonical inventory ───
            var inv = new HoldfastTradeInventory(catalog);
            inv.AddItem(District8DeepCoastSystem.ItemScrapMetal, 4);
            var bills = new List<Dictionary<string, int>> { dc.NextStepBill() };
            bool consumed = dc.TryClearPerimeter(126, (id, qty) =>
            {
                if (!inv.Items.TryGetValue(id, out int held) || held < qty) return false;
                inv.RemoveItem(id, qty);
                return true;
            });
            Check(consumed, "perimeter cleared with materials");
            Check(dc.Stage == DeepCoastStage.PerimeterOpen, "stage is perimeter_open");
            Check(!dc.TryClearPerimeter(126, (_, _) => true), "perimeter clear idempotent");

            // Channel: requires fuel + scrap; verify a short inventory refuses first.
            var shortInv = new HoldfastTradeInventory(catalog);
            Check(!dc.TryClearServiceChannel(127, (id, qty) =>
            {
                if (!shortInv.Items.TryGetValue(id, out int held) || held < qty) return false;
                shortInv.RemoveItem(id, qty);
                return true;
            }), "channel clear refused on empty inventory");
            inv.AddItem(District8DeepCoastSystem.ItemFuel, 3);
            inv.AddItem(District8DeepCoastSystem.ItemScrapMetal, 2);
            Check(dc.TryClearServiceChannel(127, (id, qty) =>
            {
                if (!inv.Items.TryGetValue(id, out int held) || held < qty) return false;
                inv.RemoveItem(id, qty);
                return true;
            }), "channel cleared with materials");
            Check(dc.Stage == DeepCoastStage.DockAccessible, "stage is dock_accessible");
            Check(dc.IsNodeAccessible(District8DeepCoastSystem.DockId),
                "existing icebreaker dock is reachable once dock_accessible");

            // Berth: at 75% integrity (salvage-immediate) the standard bill applies.
            inv.AddItem(District8DeepCoastSystem.ItemScrapMetal, 6);
            inv.AddItem(District8DeepCoastSystem.ItemBrassFittings, 3);
            Check(dc.TryRepairDeepBerth(128, (id, qty) =>
            {
                if (!inv.Items.TryGetValue(id, out int held) || held < qty) return false;
                inv.RemoveItem(id, qty);
                return true;
            }), "berth repaired after structural work");
            Check(dc.Stage == DeepCoastStage.DeepBerthOperational, "stage is deep_berth_operational");
            Check(dc.CanStartDockOperation, "dock operation available at operational berth");

            // Severely damaged structure (<60%) forces the heavier scrap bill.
            var damaged = new District8DeepCoastSystem(DefaultSeed + 3);
            damaged.SurveyPerimeter(124);
            damaged.MakeReopeningDecision(DeepCoastAccessDecision.StabilizeRepair, 125, new SeededRng(DefaultSeed + 3));
            Check(damaged.TryClearPerimeter(126, (_, _) => true), "damaged-route perimeter clears");
            Check(damaged.TryClearServiceChannel(127, (_, _) => true), "damaged-route channel clears");
            var dmgState = damaged.CaptureState();
            dmgState.structuralIntegrity = 40f;
            damaged.RestoreState(dmgState);
            var dmgBill = damaged.NextStepBill();
            Check(dmgBill.TryGetValue(District8DeepCoastSystem.ItemScrapMetal, out int dmgScrap) && dmgScrap == 6,
                "damaged structure demands the heavier scrap bill (6× scrap_metal)");
            Check(!damaged.TryRepairDeepBerth(128, (_, _) => false),
                "berth repair refused when the structural bill cannot be paid");

            // ── Expedition → dive handoff (existing maritime dive) ─────
            var dive = new StealthDiveInstance();
            Check(dc.TryStartDockOperation("dc8_op_test", "survivor_sarah_chen", 129),
                "dock operation starts at operational berth");
            Check(!dc.TryStartDockOperation("dc8_op_second", "survivor_marcus_reid", 129),
                "second dock operation rejected while one is active");
            dive.StartDive("survivor_sarah_chen", "survivor_marcus_reid", 120f);
            Check(dive.IsActive, "dock dive handed off to existing StealthDiveInstance");
            dive.Tick(60f);
            dive.EndDive(success: true);
            float levy;
            Check(dc.TryEndDockOperation(true, out levy) && levy == 0f,
                "dock operation completes; no levy when not fleet-controlled");
            Check(dc.CanStartDockOperation, "dock operation can restart after completion");

            // ── Fleet-controlled decision: levy + free work ────────────
            var dcFleet = new District8DeepCoastSystem(DefaultSeed + 1);
            dcFleet.SurveyPerimeter(124);
            var stances = new FactionStanceEngine();
            var fleetOut = dcFleet.MakeReopeningDecision(DeepCoastAccessDecision.FleetControlled, 125, new SeededRng(DefaultSeed + 1));
            if (fleetOut == null) { log.Warn("DeepCoastHeadlessDemo: fleet decision null — aborting"); return report; }
            stances.ModifyTrust(District8DeepCoastSystem.FactionFleet, fleetOut.FleetTrustDelta);
            stances.ModifyTrust(District8DeepCoastSystem.FactionOffice, fleetOut.OfficeTrustDelta);
            Check(dcFleet.TryClearPerimeter(126, (_, _) => true), "fleet clears perimeter free");
            Check(dcFleet.TryClearServiceChannel(127, (_, _) => true), "fleet cuts channel free");
            Check(dcFleet.TryRepairDeepBerth(128, (_, _) => true), "fleet stands the berth up free");
            Check(dcFleet.Stage == DeepCoastStage.DeepBerthOperational, "fleet-controlled reaches operational");
            Check(dcFleet.IsFleetLevyActive, "fleet levy active under fleet control");
            Check(stances.GetTrust(District8DeepCoastSystem.FactionFleet) == fleetOut.FleetTrustDelta,
                "exact fleet trust delta applied via FactionStanceEngine");
            dcFleet.TryStartDockOperation("dc8_op_fleet", "survivor_sarah_chen", 129);
            float fleetLevy;
            Check(dcFleet.TryEndDockOperation(true, out fleetLevy) && fleetLevy == District8DeepCoastSystem.FleetLevyFraction,
                "fleet levy applies to successful dock salvage");

            // ── Journal once-only (real JournalSystem dedupe) ──────────
            var journal = new JournalSystem();
            int before = journal.EntryCount;
            journal.TryAddRawEntry(District8DeepCoastSystem.JournalSurvey, "first", null!, 1);
            journal.TryAddRawEntry(District8DeepCoastSystem.JournalSurvey, "second", null!, 2);
            Check(journal.EntryCount == before + 1, "journal entry lands once per knowledge key");

            // ── Daily tick idempotence ─────────────────────────────────
            var dcTick = new District8DeepCoastSystem(DefaultSeed + 2);
            dcTick.SurveyPerimeter(124);
            dcTick.MakeReopeningDecision(DeepCoastAccessDecision.SalvageImmediate, 125, new SeededRng(DefaultSeed + 2));
            float c0 = dcTick.ContaminationLevel;
            Check(c0 > 0f, "salvage-immediate contaminates (daily tick precondition)");
            dcTick.TickDaily(130, WeatherKind.Blizzard);
            float c1 = dcTick.ContaminationLevel;
            dcTick.TickDaily(130, WeatherKind.Blizzard);
            Check(c1 < c0, "daily degradation applied once");
            Check(dcTick.ContaminationLevel == c1, "no duplicate daily tick");
            Check(dcTick.State.lastTickDay == 130, "single daily tick recorded");

            // ── HoldfastSave v5 round-trip + migration + rejection ────
            var json = new SystemTextJsonSerializer();
            var clock = new SimClock(130);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var quests = new HoldfastQuestSystem();
            var save = HoldfastSaveCodec.Capture(ice, census, brine, quests, dc, clock);
            Check(save.saveVersion == HoldfastSave.CurrentSaveVersion,
                "saveVersion is " + HoldfastSave.CurrentSaveVersion);
            Check(save.deepCoast != null && save.deepCoast.stage == (int)DeepCoastStage.DeepBerthOperational,
                "deep coast state captured in HoldfastSave");
            string encoded = HoldfastSaveCodec.Encode(save, json);
            var loaded = HoldfastSaveCodec.Decode(encoded, json);
            Check(loaded.deepCoast.stage == (int)DeepCoastStage.DeepBerthOperational,
                "deep coast state round-trips through the codec");
            Check(loaded.deepCoast.accessDecision == (int)DeepCoastAccessDecision.SalvageImmediate,
                "access decision round-trips");
            Check(loaded.deepCoast.structuralIntegrity == dc.StructuralIntegrity,
                "structural integrity round-trips");

            // Missing-state defaults: a v4 save migrates to a sealed route.
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
            Check(migrated.saveVersion == HoldfastSave.CurrentSaveVersion, "v4 migrates to v5");
            Check(migrated.deepCoast != null && migrated.deepCoast.stage == (int)DeepCoastStage.Sealed,
                "v4 migration yields a sealed deep-coast route (missing-state default)");

            // Future-version rejection.
            var future = json.Deserialize<HoldfastSave>(encoded) ?? new HoldfastSave();
            future.saveVersion = HoldfastSave.CurrentSaveVersion + 1;
            future.Checksum = SaveChecksum.Compute(future);
            bool rejected = false;
            try
            {
                HoldfastSaveCodec.Decode(json.Serialize(future), json);
            }
            catch (InvalidOperationException)
            {
                rejected = true;
            }
            Check(rejected, "future saveVersion rejected");

            // Tamper rejection.
            string tampered = encoded.Replace("\"simDay\":130", "\"simDay\":131");
            bool tamperRejected = false;
            try
            {
                HoldfastSaveCodec.Decode(tampered, json);
            }
            catch (InvalidOperationException)
            {
                tamperRejected = true;
            }
            Check(tamperRejected, "tampered v5 save rejected (checksum)");

            report.Summary = $"[DeepCoastHeadlessDemo] {report.PassedCount}/{report.PassedCount + report.FailedCount} PASSED";
            report.Passed = report.FailedCount == 0;
            log.Info(report.Summary);
            return report;
        }

        private static bool IsKnownGood(string itemId)
        {
            // Existence probe across the StreamingAssets item catalogs.
            string dataDir = null!;
            if (!CatalogLocator.TryFindDataDirectory(Environment.CurrentDirectory, out dataDir) || string.IsNullOrEmpty(dataDir))
                return false;
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string[] candidates =
            {
                "items.json", "black_flotilla_items.json", "holdfast_items.json",
                "crossing_items.json", "chemical_dependency_items.json", "dose_items.json"
            };
            for (int i = 0; i < candidates.Length; i++)
            {
                string path = files.Combine(dataDir, candidates[i]);
                if (!files.FileExists(path)) continue;
                try
                {
                    string raw = files.ReadAllText(path);
                    using var doc = JsonDocument.Parse(raw);
                    JsonElement array = doc.RootElement;
                    if (array.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in array.EnumerateObject())
                        {
                            if (prop.Name.Equals("schema_version", StringComparison.OrdinalIgnoreCase))
                                continue;
                            if (prop.Value.ValueKind == JsonValueKind.Array)
                            {
                                array = prop.Value;
                                break;
                            }
                        }
                    }
                    if (array.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var elem in array.EnumerateArray())
                        {
                            if (elem.TryGetProperty("id", out var idProp))
                            {
                                string id = idProp.GetString();
                                if (!string.IsNullOrEmpty(id) && id == itemId)
                                    return true;
                            }
                        }
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(path, "item catalog probe", ex_CATDIAG);
                    // Not every catalog uses the item shape; skip it.
                }
            }
            return false;
        }

        private static bool SameSalvage(List<SalvageEntry> a, List<SalvageEntry> b)
        {
            if (a == null || b == null || a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
                if (a[i].ItemId != b[i].ItemId || a[i].Quantity != b[i].Quantity)
                    return false;
            return true;
        }
    }
}
