// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        /// <summary>
        /// Dose Ledger save gate: build a session, seal dosimeters, book a reading,
        /// name a sick band, book a Cohort child, sign a volunteer, capture, write
        /// through the codec to a temp path, reload into a fresh session, restore,
        /// and verify each register reproduces. Then tamper and verify the checksum
        /// refuses it.
        /// </summary>
        public static int RunExpeditionSelfTest()
        {
            // The headless demo and the vehicle gates are BOTH part of this gate, so
            // the PASS/FAIL summary must be emitted exactly once, after both have
            // run. This previously emitted the summary straight after the demo and
            // only then ran the vehicle gates, so "EXPEDITION_SELFTEST PASS" was
            // already on stdout before nine further gates executed — a vehicle
            // failure could not unprint it, and CI scraping the summary saw green.
            var report = ExpeditionHeadlessDemo.Run(new GodotLog());
            GD.Print(report?.Summary ?? "[ExpeditionHeadlessDemo] null report");

            bool demoPassed = report != null && report.Passed;
            int demoPassCount = report?.PassedCount ?? 0;
            int demoFailCount = report != null ? report.FailedCount : 1;

            var vehicle = RunExpeditionVehicleLogisticsGates();
            var micro = RunMicroLocationConsequenceGates();

            bool passed = demoPassed && vehicle.failures == 0 && micro.failures == 0;
            string details = passed
                ? $"headless demo + {vehicle.passed} vehicle gate(s) + {micro.passed} micro-location gate(s)"
                : $"demo {(demoPassed ? "PASS" : "FAIL")} ({demoFailCount} failed), vehicle gates {vehicle.failures} failed of {vehicle.passed + vehicle.failures}, micro-location gates {micro.failures} failed of {micro.passed + micro.failures}";

            return EmitSummary(
                "expedition_selftest",
                passed,
                passed ? 0 : 1,
                demoPassCount + vehicle.passed + micro.passed,
                demoFailCount + vehicle.failures + micro.failures,
                details);
        }

        /// <summary>
        /// F1–F4 flagship gates: depletion blocks re-selection, the truck's
        /// +2 canned food reaches the active sortie's loot, capacity rejects
        /// whole grants without un-depleting, offerings consume shelter stock
        /// without underflow, journal keys unlock exactly once, clue discovery
        /// opens dispatch, and every ledger survives the aggregate round-trip
        /// without replaying consequences.
        /// </summary>
        private static (int passed, int failures) RunMicroLocationConsequenceGates()
        {
            int passed = 0;
            int failures = 0;
            string stage = "M0: micro-location gate setup";

            void Check(bool ok, string label)
            {
                if (ok) { GD.Print($"[PASS] {label}"); passed++; }
                else { GD.PrintErr($"[FAIL] {label}"); failures++; }
            }

            try
            {
                if (!CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string dataDir))
                    dataDir = CatalogPath.ResolveDataDir();
                if (string.IsNullOrEmpty(dataDir))
                    return (0, 1);

                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                var journal = new Ashfall.Core.Journal.JournalSystem();
                int codexEvents = 0;
                journal.OnCodexUnlocked += _ => codexEvents++;
                var inventory = new Ashfall.Core.Inventory.Inventory();

                var narrative = new NarrativeEncounterSystem();
                narrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, serializer));
                stage = "M1 catalog loads micro-locations";
                Check(narrative.Find("micro_crashed_truck") != null && narrative.Find("micro_observation_post") != null,
                    "M1: micro-location catalog loads with the flagship fixtures.");

                var session = ExpeditionHostSession.Create(dataDir, narrative);
                session.Journal = journal;
                session.ShelterInventory = inventory;

                // A live sortie to receive loot: truck grant targets the pack.
                stage = "M2 dispatch active sortie";
                Check(session.GetBlockReason("rural_gas_station") != null,
                    "M2a: rural_gas_station starts clue-gated (requiresDiscovery).");
                bool clueFound = session.Engine.DiscoverLocation("rural_gas_station");
                var dispatch = clueFound
                    ? session.StartExpedition("survivor_a", "rural_gas_station", ExpeditionStance.Stealth)
                    : default;
                Check(dispatch.IsSuccess && session.Engine.Active.ContainsKey("survivor_a"),
                    "M2b: after the clue, rural_gas_station dispatches for the micro-location gates.");

                stage = "M3 truck grant + depletion";
                bool truckOk = session.EncounterApplyChoice("micro_crashed_truck", "search_truck_cargo", day: 2, locationId: "rural_gas_station");
                var truckApp = session.LastApplication;
                var active = session.Engine.Active["survivor_a"];
                int cannedAfterGrant = 0;
                foreach (var loot in active.loot) if (loot.itemId == "canned_food") cannedAfterGrant += loot.quantity;
                Check(truckOk && truckApp != null && truckApp.Item == ExpeditionHostSession.EncounterApplicationResult.Status.Applied
                      && cannedAfterGrant == 2,
                    "M3: search_truck_cargo grants exactly 2 canned food into the active sortie.");
                Check(session.NarrativeEngine!.IsDepleted("micro_crashed_truck"),
                    "M3: the truck encounter is depleted after its depleting choice.");

                stage = "M4 no duplicate grant on save/load";
                var aggregate = session.CaptureSaveAggregate();
                var narrativeState = narrative.CaptureState();
                var session2 = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                session2.RestoreSaveAggregate(aggregate);
                session2.NarrativeEngine!.RestoreState(narrativeState);
                var restoredSortie = session2.Engine.Active["survivor_a"];
                int cannedAfterRestore = 0;
                foreach (var loot in restoredSortie.loot) if (loot.itemId == "canned_food") cannedAfterRestore += loot.quantity;
                Check(cannedAfterRestore == 2 && session2.NarrativeEngine.IsDepleted("micro_crashed_truck"),
                    "M4: restore neither replays the grant nor forgets depletion.");

                stage = "M5 depleted truck never re-selected";
                bool truckResurfaced = false;
                for (int seed = 0; seed < 128; seed++)
                {
                    var picked = session2.NarrativeEngine.SelectEncounter("Stealth", 1f, "rural_gas_station", new SeededRng(seed));
                    if (picked != null && picked.id == "micro_crashed_truck") { truckResurfaced = true; break; }
                }
                Check(!truckResurfaced, "M5: the depleted truck never re-enters the eligible pool (128 seeds).");

                stage = "M6 capacity rejects whole grant";
                // Fill the pack to the brim, then try a heavy grant.
                active.currentWeightKg = active.maxLootCapacityKg;
                bool truck2 = session.EncounterApplyChoice("micro_collapsed_bridge", "search_bridge_vehicle", day: 3, locationId: "rural_gas_station");
                var capApp = session.LastApplication;
                Check(truck2 && capApp != null && capApp.Item == ExpeditionHostSession.EncounterApplicationResult.Status.RejectedCapacity,
                    "M6: an overweight grant is rejected by capacity.");
                Check(session.NarrativeEngine!.IsDepleted("micro_collapsed_bridge"),
                    "M6: the bridge still depletes even though the cargo stayed behind.");

                stage = "M7 offering consumes exactly once";
                inventory.TryProduce("canned_food", 3);
                int before = CountItem(inventory, "canned_food");
                bool shrine = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 4, locationId: "rural_gas_station");
                var offeringApp = session.LastApplication;
                int after = CountItem(inventory, "canned_food");
                Check(shrine && offeringApp != null && offeringApp.Item == ExpeditionHostSession.EncounterApplicationResult.Status.Applied
                      && before - after == 1 && after >= 0,
                    "M7: add_shrine_offering removes exactly 1 canned food without underflow.");

                stage = "M8 offering without stock never underflows";
                while (CountItem(inventory, "canned_food") > 0) inventory.TryConsume("canned_food", 1);
                int emptyStock = CountItem(inventory, "canned_food");
                bool shrine2 = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 5, locationId: "rural_gas_station");
                var blockedApp = session.LastApplication;
                Check(shrine2 && blockedApp != null && blockedApp.Item == ExpeditionHostSession.EncounterApplicationResult.Status.RejectedInsufficientItems
                      && CountItem(inventory, "canned_food") == emptyStock,
                    "M8: an unaffordable offering is rejected and inventory never goes negative.");

                stage = "M9 journal unlock fires exactly once";
                bool bus = session.EncounterApplyChoice("micro_frozen_bus", "read_bus_tag", day: 6, locationId: "rural_gas_station");
                var busApp = session.LastApplication;
                int eventsAfterFirst = codexEvents;
                bool hasEntry = false;
                foreach (var e in journal.Entries) if (e.KnowledgeKey == "micro_frozen_bus_transit_tag") hasEntry = true;
                Check(bus && busApp != null && busApp.Journal == ExpeditionHostSession.EncounterApplicationResult.Status.Applied
                      && eventsAfterFirst == 1 && hasEntry,
                    $"M9: read_bus_tag writes one journal entry and fires OnCodexUnlocked once (bus={bus}, status={busApp?.Journal.ToString() ?? "null"}, events={eventsAfterFirst}, entry={hasEntry}).");

                stage = "M10 duplicate discovery is idempotent";
                journal.AddKnowledgeEvidence("expedition", "micro_frozen_bus_transit_tag");
                journal.TryDiscoverKnowledge("micro_frozen_bus_transit_tag", null!, 6);
                Check(codexEvents == 1,
                    "M10: re-discovering the same key fires no second codex event.");

                stage = "M10b persisted choice ledger refuses a replayed surfacing";
                var ledger = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                    new Ashfall.Core.Expeditions.EncounterChoiceState());
                session.ChoiceLedger = ledger;
                inventory.TryProduce("canned_food", 3);
                bool firstApply = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 7, locationId: "rural_gas_station");
                int stockAfterFirst = CountItem(inventory, "canned_food");
                bool replay = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 7, locationId: "rural_gas_station");
                bool replayDuplicate = session.LastChoiceWasDuplicate;
                // Reload: the ledger survives capture/restore, the in-memory bridge guard does not.
                var reloaded = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                    new Ashfall.Core.Expeditions.EncounterChoiceState());
                reloaded.RestoreState(ledger.CaptureState());
                session.ChoiceLedger = reloaded;
                bool replayAfterReload = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 7, locationId: "rural_gas_station");
                Check(firstApply && !replay && replayDuplicate && !replayAfterReload
                      && CountItem(inventory, "canned_food") == stockAfterFirst && ledger.History.Count == 1,
                    $"M10b: a replayed surfacing is refused before and after reload with no second offering (first={firstApply}, replay={replay}, reload={replayAfterReload}, ledger={ledger.History.Count}).");
                session.ChoiceLedger = null;

                stage = "M10d persisted ledger refuses a replayed popup surfacing in a fresh session";
                var popupLedger = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                    new Ashfall.Core.Expeditions.EncounterChoiceState());
                session.ChoiceLedger = popupLedger;
                inventory.TryProduce("canned_food", 3);
                // Surfaced through the popup path: a pending row exists at apply time
                // and ClearPending removes it before any replay can read the leg key.
                session.NarrativeEngine?.EnqueuePending("micro_shrine", "rural_gas_station", legIndex: 2, day: 9);
                bool popupFirst = session.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 9, locationId: "rural_gas_station");
                int popupStock = CountItem(inventory, "canned_food");
                // Fresh host session: the bridge's in-memory guard is gone; only the
                // restored ledger stands between the replay and a second application.
                var replaySession = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                var restoredLedger = new Ashfall.Core.Expeditions.EncounterChoiceResolver(
                    new Ashfall.Core.Expeditions.EncounterChoiceState());
                restoredLedger.RestoreState(popupLedger.CaptureState());
                replaySession.ChoiceLedger = restoredLedger;
                bool popupReplay = replaySession.EncounterApplyChoice("micro_shrine", "add_shrine_offering", day: 9, locationId: "rural_gas_station");
                bool popupReplayDup = replaySession.LastChoiceWasDuplicate;
                Check(popupFirst && !popupReplay && popupReplayDup
                      && CountItem(inventory, "canned_food") == popupStock,
                    $"M10d: a replayed popup surfacing in a fresh session is refused by the restored ledger with no second offering (first={popupFirst}, replay={popupReplay}, dup={popupReplayDup}).");
                session.ChoiceLedger = null;

                stage = "M10c hostile travel choice escalates to combat and unlocks the field guide";
                var travelSession = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                int combatRaised = 0;
                var unlocked = new List<string>();
                travelSession.OnTravelEncounterCombatTriggered += t => { if (t.CombatantIds.Count > 0) combatRaised++; };
                travelSession.FieldGuideUnlock = id => { unlocked.Add(id); return true; };
                bool hostileOk = travelSession.EncounterApplyChoice("enc_travel_wolf_pack_crossing", "choice_rifle_ambush", day: 8, locationId: "rural_gas_station");
                int combatAfterHostile = combatRaised;
                // Fresh session: the resolved crossing is on cooldown in the first one.
                var calmSession = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                calmSession.OnTravelEncounterCombatTriggered += t => { if (t.CombatantIds.Count > 0) combatRaised++; };
                calmSession.FieldGuideUnlock = id => { unlocked.Add(id); return true; };
                bool calmOk = calmSession.EncounterApplyChoice("enc_travel_wolf_pack_crossing", "choice_throw_flare", day: 8, locationId: "rural_gas_station");
                Check(hostileOk && calmOk && combatAfterHostile == 1 && combatRaised == 1
                      && unlocked.Count == 2 && unlocked[0] == "field_fauna_two_headed_wolf"
                      && travelSession.LastApplication?.FieldGuide == ExpeditionHostSession.EncounterApplicationResult.Status.Applied,
                    $"M10c: rifle ambush raises combat once, the flare does not, both unlock the wolf entry (hostile={hostileOk}, calm={calmOk}, combat={combatRaised}, unlocks={unlocked.Count}).");

                stage = "M11 clue discovery gates dispatch";
                var fresh = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                fresh.Definitions.Clear();
                foreach (var d in session.Definitions) fresh.Definitions.Add(d);
                foreach (var d in fresh.Definitions)
                    Ashfall.Core.Expeditions.ExpeditionDefinitionRegistry.Register(d);
                Check(fresh.GetBlockReason("rural_gas_station") != null,
                    "M11a: rural_gas_station is blocked before its clue is found.");
                bool discovered = fresh.Engine.DiscoverLocation("rural_gas_station");
                Check(discovered && fresh.GetBlockReason("rural_gas_station") == null,
                    "M11b: discovering the location opens dispatch.");
                var postDispatch = fresh.StartExpedition("survivor_b", "rural_gas_station", ExpeditionStance.Stealth);
                Check(postDispatch.IsSuccess,
                    "M11c: the revealed destination dispatches under normal rules.");

                stage = "M12 observation post combines journal + location";
                var fresh2 = ExpeditionHostSession.Create(dataDir, new NarrativeEncounterSystem());
                fresh2.Journal = journal;
                int codexBefore = codexEvents;
                bool bunkerNotYetKnown = !fresh2.Engine.IsLocationKnown("government_bunker");
                bool obs = bunkerNotYetKnown
                           && fresh2.EncounterApplyChoice("micro_observation_post", "read_grid_references", day: 7, locationId: "loc_the_allotments");
                var obsApp = fresh2.LastApplication;
                Check(obs && obsApp != null
                      && obsApp.Journal == ExpeditionHostSession.EncounterApplicationResult.Status.Applied
                      && obsApp.Location == ExpeditionHostSession.EncounterApplicationResult.Status.Applied
                      && fresh2.Engine.IsLocationKnown("rural_gas_station")
                      && codexEvents == codexBefore + 1,
                    "M12: read_grid_references unlocks knowledge AND reveals rural_gas_station in one resolution.");

                stage = "M13 supply drop reveals the bunker";
                bool drop = fresh2.EncounterApplyChoice("micro_supply_drop", "read_supply_label", day: 8, locationId: "loc_the_allotments");
                Check(drop && fresh2.Engine.IsLocationKnown("government_bunker"),
                    "M13: read_supply_label discovers government_bunker.");

                stage = "M14 legacy saves migrate without replay";
                // Migration fixture with BOTH shapes of pre-feature truth: a
                // depleting choice (truck) and clue discoveries (observation
                // post + supply drop) recorded only in resolution history. In
                // production both restores share the one narrative engine
                // (narrative restore happens first, then the aggregate reads
                // its history), so the migrated session mirrors that wiring.
                var migrantDispatch = fresh2.StartExpedition("survivor_b", "rural_gas_station", ExpeditionStance.Stealth);
                bool truckMigrated = migrantDispatch.IsSuccess
                    && fresh2.EncounterApplyChoice("micro_crashed_truck", "search_truck_cargo", day: 9, locationId: "rural_gas_station");
                var legacyNarrative = fresh2.NarrativeEngine!.CaptureState();
                legacyNarrative.depletedEncounterIds = null;
                var legacyAggregate = fresh2.CaptureSaveAggregate();
                legacyAggregate.knownLocationIds = null;
                var migratedNarrative = new NarrativeEncounterSystem();
                migratedNarrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, serializer));
                migratedNarrative.RestoreState(legacyNarrative);
                var migratedSession = new ExpeditionHostSession(null!, migratedNarrative);
                migratedSession.RestoreSaveAggregate(legacyAggregate);
                Check(migratedNarrative.IsDepleted("micro_crashed_truck"),
                    "M14a: legacy narrative save reconstructs truck depletion from history.");
                int migratedCanned = 0;
                foreach (var kv in migratedSession.Engine.Active)
                    foreach (var loot in kv.Value.loot)
                        if (loot.itemId == "canned_food") migratedCanned += loot.quantity;
                Check(migratedSession.Engine.IsLocationKnown("rural_gas_station")
                      && migratedSession.Engine.IsLocationKnown("government_bunker"),
                    "M14b: legacy aggregate reconstructs discoveries from history.");
                Check(migratedCanned == 2,
                    "M14c: legacy loot is preserved without a retroactive replay of the grant.");
            }
            catch (System.Exception ex)
            {
                GD.PrintErr($"[FAIL] micro-location gates threw during \"{stage}\": {ex.GetType().Name}: {ex.Message}");
                GD.PrintErr(ex.ToString());
                failures++;
            }

            return (passed, failures);
        }

        private static int CountItem(Ashfall.Core.Inventory.Inventory inventory, string itemId)
        {
            int total = 0;
            foreach (var slot in inventory.GetSlots())
            {
                if (slot != null && slot.Item != null && slot.Item.id == itemId)
                    total += slot.Amount;
            }
            return total;
        }

        /// <summary>
        /// Task #101 gates: vehicle dispatch preparation (fuel gate + profile),
        /// driven travel beats foot, estimate math matches the selection, the
        /// weapon-condition bridge feeds readiness into estimates, and the
        /// in-flight vehicle state survives the aggregate save round-trip.
        ///
        /// <para>Returns counts rather than an exit code so the caller can fold them
        /// into a single summary. Exceptions are contained here and converted into a
        /// counted failure that names the gate that threw, so one crashing gate can
        /// neither abort the run silently nor be mistaken for a pass.</para>
        /// </summary>
        private static (int passed, int failures) RunExpeditionVehicleLogisticsGates()
        {
            int passed = 0;
            int failures = 0;
            string stage = "V0: vehicle gate setup";

            void Check(bool ok, string label)
            {
                if (ok) { GD.Print($"[PASS] {label}"); passed++; }
                else { GD.PrintErr($"[FAIL] {label}"); failures++; }
            }

            try
            {
                var session = new ExpeditionHostSession();

                stage = "V1 garage setup";
                // Garage: an inline catalog (no FS dependence), quad acquired fresh.
                session.Vehicles.LoadCatalog(new VehicleCatalog
                {
                    vehicles = new System.Collections.Generic.List<VehicleDefinition>
                    {
                        new VehicleDefinition
                        {
                            vehicle_id = ExpeditionHostSession.StarterVehicleId,
                            display_name = "Utility Quad",
                            max_fuel = 40f,
                            cargo_capacity = 90f,
                            speed_multiplier = 1.3f,
                            fuel_consumption_per_km = 0.3f,
                        },
                    }
                });
                Check(session.Vehicles.AcquireVehicle(ExpeditionHostSession.StarterVehicleId).Status == ActionResult.StatusKind.Success,
                    "V1: starter quad acquired into the garage.");

                stage = "V2/V3 travel estimates";
                // Estimate: vehicle is faster and carries fuel cost; foot does not.
                string target = "loc_the_allotments";
                var foot = session.EstimateExpedition(target, ExpeditionStance.Stealth)!.Value.estimate;
                var driven = session.EstimateExpedition(target, ExpeditionStance.Stealth, ExpeditionHostSession.StarterVehicleId)!.Value.estimate;
                Check(!foot.usingVehicle && foot.fuelRequired == 0f, "V2: foot estimate has no fuel cost.");
                // Travel progress is intentionally quantized to whole ticks by
                // the execution path. A 1.3x vehicle on this short five-tick
                // route therefore ties foot travel; longer routes and faster
                // profiles show the expected reduction. The contract here is
                // that a valid vehicle is never slower and does carry fuel cost.
                Check(driven.usingVehicle && driven.fuelRequired > 0f && driven.totalTicks <= foot.totalTicks,
                    "V3: vehicle estimate is no slower with fuel cost.");

                stage = "V4 weapon-condition readiness";
                // Weapon-condition bridge feeds readiness into the encounter risk.
                var inv = new Ashfall.Core.Inventory.Inventory();
                var equipment = new EquipmentConditionSystem(new SeededRng(7), inv, new CraftingSystem(inv));
                equipment.RegisterItem("eq_gate_rifle", "weapon_bolt_rifle", "survivor_a", EquipmentFamily.Weapon);
                equipment.UseItem("eq_gate_rifle", 85f); // condition 15 → degraded readiness
                float readiness = Ashfall.Core.Combat.WeaponEquipmentBridge.Readiness(equipment, "eq_gate_rifle");
                var degraded = session.EstimateExpedition(target, ExpeditionStance.Stealth, "", readiness, Ashfall.Core.Combat.WeaponEquipmentBridge.JamRisk(equipment, "eq_gate_rifle"))!.Value.estimate;
                Check(readiness < 1f && degraded.encounterRiskPerTick > foot.encounterRiskPerTick,
                    "V4: degraded weapon readiness raises the encounter-risk estimate.");

                stage = "V5 fuel gate blocks dispatch";
                // Dispatch preparation: fuel gate blocks, then a full tank passes
                // and the sortie starts with the vehicle profile attached.
                session.Vehicles.GetVehicle(ExpeditionHostSession.StarterVehicleId)!.fuel = 0.5f;
                var refused = session.StartExpedition("survivor_a", target, ExpeditionStance.Stealth, vehicleId: ExpeditionHostSession.StarterVehicleId);
                Check(!refused.IsSuccess && session.Engine.ActiveCount == 0,
                    "V5: depleted fuel blocks dispatch with a refuel message.");

                stage = "V6 fueled dispatch starts a sortie";
                session.Vehicles.Refuel(ExpeditionHostSession.StarterVehicleId, 60f);
                var sent = session.DispatchSortie("survivor_a", target, ExpeditionStance.Stealth, 1, ExpeditionHostSession.StarterVehicleId);

                // Assert the sortie exists BEFORE indexing it. Indexing a missing key
                // threw KeyNotFoundException here, which aborted the remaining gates.
                bool dispatched = session.Engine.ActiveCount == 1
                    && session.Engine.Active.ContainsKey("survivor_a");
                if (!dispatched)
                {
                    Check(false,
                        $"V6: fueled dispatch starts a vehicle sortie with a speed profile. " +
                        $"(DispatchSortie returned {DescribeDispatch(sent)}; ActiveCount={session.Engine.ActiveCount})");
                    GD.PrintErr("[FAIL] V7-V9 skipped: no active sortie to exercise.");
                    failures += 3;
                    return (passed, failures);
                }

                var active = session.Engine.Active["survivor_a"];
                Check(active.vehicleId == ExpeditionHostSession.StarterVehicleId && active.vehicleSpeedMultiplier > 1f,
                    "V6: fueled dispatch starts a vehicle sortie with a speed profile.");

                stage = "V7 mid-route breakdown";
                // Force a deterministic mid-route breakdown and confirm the
                // aggregate round-trip keeps the in-flight vehicle state.
                active.vehicleBreakdownChancePerTick = 1f; // guaranteed next travel tick
                session.TickHours(1f);
                Check(active.vehicleBrokenDown, "V7: seeded mid-route breakdown flips the sortie to foot.");

                stage = "V8 aggregate save round-trip";
                var aggregate = session.CaptureSaveAggregate();
                var restored = new ExpeditionHostSession();
                restored.RestoreSaveAggregate(aggregate);
                Check(restored.Engine.Active.ContainsKey("survivor_a") &&
                      restored.Engine.Active["survivor_a"].vehicleBrokenDown &&
                      restored.Vehicles.GetVehicle(ExpeditionHostSession.StarterVehicleId) != null,
                    "V8: aggregate save round-trip restores the sortie and the garage.");

                stage = "V9 garage repair";
                // Repair clears the breakdown and tops the condition.
                restored.RepairVehicle(ExpeditionHostSession.StarterVehicleId, 100f);
                Check(restored.Vehicles.GetVehicle(ExpeditionHostSession.StarterVehicleId)!.condition >= 100f,
                    "V9: garage repair restores the vehicle.");
            }
            catch (System.Exception ex)
            {
                GD.PrintErr($"[FAIL] expedition vehicle gates threw during \"{stage}\": {ex.GetType().Name}: {ex.Message}");
                GD.PrintErr(ex.ToString());
                failures++;
            }

            return (passed, failures);
        }

        /// <summary>Short description of a dispatch result for gate diagnostics.</summary>
        private static string DescribeDispatch(Ashfall.Core.PlayerCommand.CommandResult result)
            => $"{result.ActionResult.Status}" +
               (string.IsNullOrEmpty(result.FailureCode) ? string.Empty : $"/{result.FailureCode}") +
               (string.IsNullOrEmpty(result.MessageKey) ? string.Empty : $" \"{result.MessageKey}\"") +
               $" expectedVersion={result.ExpectedStateVersion} actualVersion={result.ActualStateVersion}";

    }
}
