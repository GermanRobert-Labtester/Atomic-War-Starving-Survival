// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Plans B86-B89 selftests
// Plan B89        : --precision-metrology-selftest — catalog validation,
//                   registered-consumer calibration, zero-benefit for
//                   unregistered systems, workshop projection, disturbance
//                   determinism, and save round-trip.
// Plan B88        : --direction-finding-selftest — DF catalog, baselines,
//                   skywave uncertainty, fingerprint identity (≠ coordinates),
//                   triangulation mid-op save, RadioSave V3 nest.
// Plan B87        : --aquaponics-selftest — catalog validation, growth
//                   determinism, power-loss DO crash, harvest into canonical
//                   food, nutrient export, save round-trip.
// Plan B86        : --combat-breaching-selftest — catalog validation, quiet
//                   cut vs loud breach, vehicle gate, mid-breach save/load,
//                   barrier clone fields.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Godot;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunPrecisionMetrologySelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} precision_metrology/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            var catalog = PrecisionMetrologyCatalogLoader.Load(dataDirectory, io, json);
            try
            {
                PrecisionMetrologyCatalogLoader.Validate(catalog);
                Check("catalog_valid", true);
            }
            catch (Exception ex)
            {
                Check("catalog_valid", false, ex.Message);
            }

            Check("grade_roster", catalog.grades != null && catalog.grades.Count >= 5,
                $"got {catalog.grades?.Count ?? 0}");
            Check("standard_roster", catalog.standards != null && catalog.standards.Count >= 4,
                $"got {catalog.standards?.Count ?? 0}");
            Check("consumer_roster", catalog.consumers != null && catalog.consumers.Count >= 3,
                $"got {catalog.consumers?.Count ?? 0}");

            var inv = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
            inv.TryProduce("item_gauge_block_set", 2);
            inv.TryProduce("machinist_caliper", 1);

            var sys = new PrecisionMetrologySystem(new SeededRng(89), inv);
            sys.LoadCatalog(catalog);

            Check("unregistered_zero_benefit",
                sys.QueryToolingCalibration("not_registered") == 0f
                && !sys.QueryCapability("not_registered").IsRegisteredConsumer);

            var reject = sys.CalibrateInstrument("not_registered", "std_gauge_block_set", 1);
            Check("unregistered_calibrate_rejected",
                !reject.IsSuccess && reject.FailureCode == "unregistered_consumer");

            var cal = sys.CalibrateInstrument(
                "workshop_precision", "std_gauge_block_set", 10, "room_workshop_precision");
            Check("calibrate_registered", cal.IsSuccess, cal.MessageKey);
            float tooling = sys.QueryToolingCalibration("workshop_precision");
            Check("tooling_raised", tooling > 0.5f, $"tooling={tooling:F3}");

            var workshop = new ShelterWorkshopSystem(
                new InventoryContainer { Capacity = 32 },
                new SeededRng(1));
            var unrelated = workshop.GetOrCreateMachineState("room_filtration");
            unrelated.Calibration = 0.42f;
            int projected = sys.ProjectToWorkshop(workshop);
            Check("workshop_projection",
                projected >= 1
                && workshop.GetOrCreateMachineState("room_workshop_precision").Calibration > 0.5f
                && workshop.GetOrCreateMachineState("room_filtration").Calibration == 0.42f,
                $"projected={projected}");

            // Ballistics live tooling is not the old hardcoded 0.75f.
            // Gauge-block path needs an explicit precision room; caliper uses consumer room default.
            AssertCalibrateBallistics(sys, Check);

            var a = new PrecisionMetrologySystem(new SeededRng(101), CloneStock());
            a.LoadCatalog(catalog);
            a.CalibrateInstrument("ballistics_workbench", "std_gauge_block_set", 20,
                roomId: "room_workshop_precision");
            float before = a.QueryToolingCalibration("ballistics_workbench");
            a.ApplyDisturbance(5.0f, 21, "quake_21");
            float afterA = a.QueryToolingCalibration("ballistics_workbench");

            var b = new PrecisionMetrologySystem(new SeededRng(101), CloneStock());
            b.LoadCatalog(catalog);
            b.CalibrateInstrument("ballistics_workbench", "std_gauge_block_set", 20,
                roomId: "room_workshop_precision");
            b.ApplyDisturbance(5.0f, 21, "quake_21");
            float afterB = b.QueryToolingCalibration("ballistics_workbench");
            Check("disturbance_deterministic",
                afterA < before && Math.Abs(afterA - afterB) < 0.0001f,
                $"before={before:F3} afterA={afterA:F3} afterB={afterB:F3}");

            var snapshot = sys.CaptureState();
            var restored = new PrecisionMetrologySystem(new SeededRng(7));
            restored.LoadCatalog(catalog);
            restored.RestoreState(snapshot);
            Check("save_round_trip",
                Math.Abs(restored.QueryToolingCalibration("workshop_precision") - tooling) < 0.0001f
                && restored.QueryGrade("workshop_precision") == sys.QueryGrade("workshop_precision"));

            GD.Print($"precision_metrology selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("precision_metrology_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));

            static InventoryContainer CloneStock()
            {
                var stock = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
                stock.TryProduce("item_gauge_block_set", 1);
                return stock;
            }
        }

        private static void AssertCalibrateBallistics(
            PrecisionMetrologySystem sys,
            Action<string, bool, string> check)
        {
            var ball = sys.CalibrateInstrument("ballistics_workbench", "std_machinist_caliper", 1);
            float fieldish = sys.QueryToolingCalibration("ballistics_workbench");
            check("ballistics_live_not_hardcoded",
                ball.IsSuccess && fieldish > 0.30f && fieldish < 0.60f && Math.Abs(fieldish - 0.75f) > 0.001f,
                ball.IsSuccess ? $"tooling={fieldish:F3}" : ball.FailureCode ?? ball.MessageKey);
        }

        public static int RunDirectionFindingSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} direction_finding/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            DirectionFindingCatalogDto? dto = null;
            try
            {
                dto = DirectionFindingCatalogLoader.Load(dataDirectory, io, json);
                DirectionFindingCatalogLoader.Validate(dto);
                Check("catalog_valid", true);
            }
            catch (Exception ex)
            {
                Check("catalog_valid", false, ex.Message);
            }

            Check("array_roster", dto != null && dto.arrays != null && dto.arrays.Count >= 3,
                $"got {dto?.arrays?.Count ?? 0}");
            Check("fingerprint_roster", dto != null && dto.fingerprints != null && dto.fingerprints.Count >= 3,
                $"got {dto?.fingerprints?.Count ?? 0}");

            if (dto == null)
            {
                GD.Print($"direction_finding selftest: {pass} passed, {fail} failed");
                return EmitSummary("direction_finding_selftest", false, failedCount: fail, passedCount: pass,
                    details: string.Join("; ", details));
            }

            var catalog = DirectionFindingCatalogLoader.Build(dto);
            var sys = new SignalTriangulationSystem();
            sys.LoadCatalog(catalog);
            Check("baselines_registered",
                sys.StationBaselines.Count >= 3
                && sys.TryGetStationBaseline("station_shelter_primary", out _),
                $"baselines={sys.StationBaselines.Count}");

            string signalId = "sig_civil_defense";
            var clearObs = MakeObs(signalId, "station_shelter_primary", 40f, "Clear", 1.2f);
            var ridgeObs = MakeObs(signalId, "station_ridge_outrigger", 95f, "Clear", 1.0f);
            var nightObs = MakeObs(signalId, "station_long_haul_relay", 150f, "Night", 1.0f);

            var clearSys = new SignalTriangulationSystem();
            clearSys.LoadCatalog(catalog);
            clearSys.RecordObservation(clearObs);
            clearSys.RecordObservation(ridgeObs);
            clearSys.RecordObservation(MakeObs(signalId, "station_long_haul_relay", 150f, "Clear", 1.0f));
            var clearFix = clearSys.Triangulate(signalId, new SeededRng(88));
            Check("clear_fix",
                clearFix != null
                && clearFix.confidence >= SignalTriangulationSystem.ConfidenceThreshold
                && clearFix.identityConfidence > 0.5f
                && clearFix.locationId == "loc_broadcast_bunker_echo",
                clearFix == null
                    ? "null candidate"
                    : $"conf={clearFix.confidence:F3} id={clearFix.identityConfidence:F3} loc={clearFix.locationId}");

            var nightSys = new SignalTriangulationSystem();
            nightSys.LoadCatalog(catalog);
            nightSys.RecordObservation(clearObs);
            nightSys.RecordObservation(ridgeObs);
            nightSys.RecordObservation(nightObs);
            var nightFix = nightSys.Triangulate(signalId, new SeededRng(88));
            Check("skywave_degrades",
                nightFix != null
                && clearFix != null
                && (nightFix.uncertaintyRadiusKm > clearFix.uncertaintyRadiusKm
                    || nightFix.confidence < clearFix.confidence),
                nightFix == null
                    ? "null night candidate"
                    : $"clearU={clearFix!.uncertaintyRadiusKm:F2} nightU={nightFix.uncertaintyRadiusKm:F2} clearC={clearFix.confidence:F3} nightC={nightFix.confidence:F3}");

            // Identity-only fingerprint must not invent a mapped location.
            var numbersSys = new SignalTriangulationSystem();
            numbersSys.LoadCatalog(catalog);
            const string numbersId = "sig_numbers";
            numbersSys.RecordObservation(MakeObs(numbersId, "station_shelter_primary", 10f, "Clear", 1.0f));
            numbersSys.RecordObservation(MakeObs(numbersId, "station_ridge_outrigger", 80f, "Clear", 1.0f));
            numbersSys.RecordObservation(MakeObs(numbersId, "station_long_haul_relay", 140f, "Clear", 1.0f));
            var numbersFix = numbersSys.Triangulate(numbersId, new SeededRng(7));
            Check("identity_not_coordinates",
                numbersFix != null
                && numbersFix.identityConfidence >= 0.8f
                && numbersFix.locationId == "triangulated_" + numbersId,
                numbersFix == null
                    ? "null"
                    : $"idConf={numbersFix.identityConfidence:F3} loc={numbersFix.locationId}");

            // Deterministic same-seed replay of night observations with jitter.
            var a = new SignalTriangulationSystem();
            a.LoadCatalog(catalog);
            a.RecordObservation(nightObs, new SeededRng(201));
            var b = new SignalTriangulationSystem();
            b.LoadCatalog(catalog);
            b.RecordObservation(nightObs, new SeededRng(201));
            Check("observation_deterministic",
                a.Observations.Count == 1 && b.Observations.Count == 1
                && Math.Abs(a.Observations[0].bearingDegrees - b.Observations[0].bearingDegrees) < 0.0001f
                && Math.Abs(a.Observations[0].errorDegrees - b.Observations[0].errorDegrees) < 0.0001f);

            var mid = clearSys.CaptureState();
            var restored = new SignalTriangulationSystem();
            restored.LoadCatalog(catalog);
            restored.RestoreState(mid);
            Check("triangulation_save_round_trip",
                restored.GetObservationCount(signalId) == clearSys.GetObservationCount(signalId)
                && restored.IsLocationDiscovered("loc_broadcast_bunker_echo")
                && restored.StationBaselines.Count >= 3);

            // RadioSave V3 nest: encode with triangulation payload, decode, nest survives.
            var radioState = new RadioSaveState
            {
                day = 12,
                currentFrequency = 97.5f,
                triangulation = clearSys.CaptureState()
            };
            string encoded = RadioSaveCodec.Encode(radioState, json);
            bool decoded = RadioSaveCodec.TryDecode(encoded, json, out var loaded);
            Check("radio_save_v3_nest",
                decoded
                && loaded != null
                && loaded.saveVersion == RadioSaveCodec.CurrentSaveVersion
                && loaded.triangulation != null
                && loaded.triangulation.observations.Count >= 3
                && loaded.triangulation.discoveredLocationIds.Contains("loc_broadcast_bunker_echo"),
                decoded ? $"v={loaded?.saveVersion} obs={loaded?.triangulation?.observations?.Count}" : "decode failed");

            // Frozen V2 migrates to V3 with empty triangulation nest.
            var v2 = new RadioSaveStateFrozenV2
            {
                saveVersion = 2,
                day = 9,
                currentFrequency = 88.5f,
                history = new List<RadioInterceptEntry>(),
                playedBroadcastKeys = new List<string>()
            };
            v2.Checksum = SaveChecksum.Compute(v2);
            string v2Json = json.Serialize(v2);
            bool migrated = RadioSaveCodec.TryDecode(v2Json, json, out var fromV2);
            Check("radio_save_v2_to_v3",
                migrated
                && fromV2 != null
                && fromV2.saveVersion == 3
                && fromV2.triangulation != null
                && fromV2.triangulation.observations.Count == 0,
                migrated ? $"v={fromV2?.saveVersion}" : "migrate failed");

            GD.Print($"direction_finding selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("direction_finding_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));

            static RadioObservation MakeObs(
                string signalId, string stationId, float bearing, string weather, float error) =>
                new RadioObservation
                {
                    signalId = signalId,
                    stationId = stationId,
                    day = 1,
                    hour = 12f,
                    bearingDegrees = bearing,
                    errorDegrees = error,
                    signalStrength = 0.95f,
                    noiseLevel = 0.05f,
                    frequencyMhz = 12.5f,
                    weatherCondition = weather,
                    operatorSkill = 0.9f
                };
        }

        public static int RunAquaponicsSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} aquaponics/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            AquaponicsCatalog? catalog = null;
            try
            {
                catalog = AquaponicsCatalogLoader.Load(dataDirectory, io, json);
                AquaponicsCatalogLoader.Validate(catalog);
                Check("catalog_valid", true);
            }
            catch (Exception ex)
            {
                Check("catalog_valid", false, ex.Message);
            }

            Check("tank_roster", catalog != null && catalog.tank_classes != null && catalog.tank_classes.Count >= 2,
                $"got {catalog?.tank_classes?.Count ?? 0}");
            Check("species_roster", catalog != null && catalog.species != null && catalog.species.Count >= 2,
                $"got {catalog?.species?.Count ?? 0}");
            Check("biofilter_roster", catalog != null && catalog.biofilters != null && catalog.biofilters.Count >= 1,
                $"got {catalog?.biofilters?.Count ?? 0}");

            if (catalog == null)
            {
                GD.Print($"aquaponics selftest: {pass} passed, {fail} failed");
                return EmitSummary("aquaponics_selftest", false, failedCount: fail, passedCount: pass,
                    details: string.Join("; ", details));
            }

            InventoryContainer MakeStock(int feed = 40, int media = 4)
            {
                var inv = new InventoryContainer { Capacity = 128, MaxWeight = 1000f };
                inv.TryProduce("item_insect_larvae_meal", feed);
                inv.TryProduce("item_biofilter_media", media);
                return inv;
            }

            var invA = MakeStock();
            var invB = MakeStock();
            float power = 1f;
            var a = new AquaponicsSystem(new SeededRng(87), invA, _ => power);
            var b = new AquaponicsSystem(new SeededRng(87), invB, _ => power);
            a.LoadCatalog(catalog);
            b.LoadCatalog(catalog);

            Check("commission",
                a.CommissionTank("tank_a", "tank_raft_small", "room_greenhouse").IsSuccess
                && b.CommissionTank("tank_a", "tank_raft_small", "room_greenhouse").IsSuccess);
            Check("stock",
                a.StockFish("tank_a", "species_rad_tilapia", 4f, day: 1).IsSuccess
                && b.StockFish("tank_a", "species_rad_tilapia", 4f, day: 1).IsSuccess);
            Check("feed",
                a.Feed("tank_a", "feed_insect_meal", 3f, day: 1).IsSuccess
                && b.Feed("tank_a", "feed_insect_meal", 3f, day: 1).IsSuccess);

            float startBiomass = a.Snapshot("tank_a").BiomassKg;
            for (int day = 2; day <= 12; day++)
            {
                if (day % 2 == 0)
                {
                    a.Feed("tank_a", "feed_insect_meal", 1.5f, day);
                    b.Feed("tank_a", "feed_insect_meal", 1.5f, day);
                }
                a.TickDay(day, temperatureModifier: 1f);
                b.TickDay(day, temperatureModifier: 1f);
            }

            float biomassA = a.Snapshot("tank_a").BiomassKg;
            float biomassB = b.Snapshot("tank_a").BiomassKg;
            Check("growth_deterministic",
                biomassA > startBiomass && Math.Abs(biomassA - biomassB) < 0.0001f,
                $"start={startBiomass:F3} a={biomassA:F3} b={biomassB:F3}");

            float doBefore = a.Snapshot("tank_a").DissolvedOxygen;
            power = 0f;
            for (int day = 13; day <= 16; day++)
                a.TickDay(day, temperatureModifier: 1f);
            var starved = a.Snapshot("tank_a");
            Check("power_loss_do_crash",
                starved.PowerStarved && starved.DissolvedOxygen < doBefore && starved.DissolvedOxygen < 3.5f,
                $"before={doBefore:F2} after={starved.DissolvedOxygen:F2}");

            power = 1f;
            var harvestInv = MakeStock(feed: 10, media: 1);
            var harvester = new AquaponicsSystem(new SeededRng(21), harvestInv, _ => 1f);
            harvester.LoadCatalog(catalog);
            harvester.CommissionTank("tank_a", "tank_raft_small", "room_greenhouse");
            harvester.StockFish("tank_a", "species_rad_tilapia", 4f, day: 1);
            harvester.State.tanks[0].biomassKg = 6f;
            int fishBefore = harvestInv.CountById("item_aquaponic_fish");
            var fishHarvest = harvester.HarvestFish("tank_a", biomassKg: 2f, day: 5);
            Check("harvest_fish_canonical",
                fishHarvest.Success
                && fishHarvest.ItemId == "item_aquaponic_fish"
                && fishHarvest.Amount >= 1
                && harvestInv.CountById("item_aquaponic_fish") == fishBefore + fishHarvest.Amount,
                fishHarvest.Success ? $"amount={fishHarvest.Amount}" : fishHarvest.FailureCode);

            harvester.State.tanks[0].nitratePool = 4f;
            harvester.State.tanks[0].plantHealth = 1f;
            var plantHarvest = harvester.HarvestPlants("tank_a", day: 6);
            Check("harvest_plants_leafy",
                plantHarvest.Success && plantHarvest.ItemId == "crop_leafy_green",
                plantHarvest.Success ? $"amount={plantHarvest.Amount}" : plantHarvest.FailureCode);

            var nutrientSys = new AquaponicsSystem(new SeededRng(61), MakeStock(), _ => 1f);
            nutrientSys.LoadCatalog(catalog);
            nutrientSys.CommissionTank("tank_a", "tank_raft_small", "room_greenhouse");
            nutrientSys.StockFish("tank_a", "species_rad_tilapia", 4f, day: 1);
            nutrientSys.Feed("tank_a", "feed_insect_meal", 3f, day: 1);
            nutrientSys.State.tanks[0].nLoad = 5f;
            nutrientSys.State.tanks[0].biofilterHealth = 100f;
            nutrientSys.TickDay(2, temperatureModifier: 1f);
            var export = nutrientSys.GetNutrientExport();
            Check("nutrient_export",
                export.NitrateAvailable >= 0f && export.SourceSystemId == AquaponicsSystem.SystemId,
                $"nitrate={export.NitrateAvailable:F3}");

            var mid = a.CaptureState();
            var restored = new AquaponicsSystem(new SeededRng(99), MakeStock(1, 0), _ => 1f);
            restored.LoadCatalog(catalog);
            restored.RestoreState(mid);
            Check("save_round_trip",
                Math.Abs(restored.Snapshot("tank_a").BiomassKg - a.Snapshot("tank_a").BiomassKg) < 0.0001f
                && Math.Abs(restored.Snapshot("tank_a").DissolvedOxygen - a.Snapshot("tank_a").DissolvedOxygen) < 0.0001f
                && restored.Snapshot("tank_a").DiseaseBandOrdinal == a.Snapshot("tank_a").DiseaseBandOrdinal);

            GD.Print($"aquaponics selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("aquaponics_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }

        public static int RunCombatBreachingSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} combat-breaching/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();
            BreachingCatalog? catalog = null;
            try
            {
                catalog = BreachingCatalogLoader.Load(dataDirectory, io, json);
                BreachingCatalogLoader.Validate(catalog);
                Check("catalog_valid", true);
            }
            catch (Exception ex)
            {
                Check("catalog_valid", false, ex.Message);
            }

            Check("obstacle_roster", catalog != null && catalog.obstacles != null && catalog.obstacles.Count == 12,
                $"got {catalog?.obstacles?.Count ?? 0}");
            Check("tool_roster", catalog != null && catalog.tools != null && catalog.tools.Count == 5,
                $"got {catalog?.tools?.Count ?? 0}");

            if (catalog == null)
            {
                GD.Print($"combat-breaching selftest: {pass} passed, {fail} failed");
                return EmitSummary("combat_breaching_selftest", false, failedCount: fail, passedCount: pass,
                    details: string.Join("; ", details));
            }

            InventoryContainer MakeInv(params (string id, int qty)[] stock)
            {
                var inv = new InventoryContainer { Capacity = 64, MaxWeight = 500f };
                foreach (var (id, qty) in stock)
                    inv.TryProduce(id, qty);
                return inv;
            }

            float noiseA = 0f, noiseB = 0f;
            var portsA = new CombatHostPorts(emitBreachNoise: n => noiseA += n);
            var portsB = new CombatHostPorts(emitBreachNoise: n => noiseB += n);

            var combatA = new TacticalCombatSystem(null, portsA);
            var combatB = new TacticalCombatSystem(null, portsB);
            combatA.LoadBreachingCatalog(catalog);
            combatB.LoadBreachingCatalog(catalog);

            var players = new List<CombatantState>
            {
                new CombatantState
                {
                    Id = "p1", Name = "Cutter", SurvivorId = "survivor_cutter",
                    IsPlayer = true, Health = 100, MaxHealth = 100
                }
            };
            var weapons = new List<WeaponInstanceState>
            {
                new WeaponInstanceState
                {
                    InstanceId = "wp1", WeaponId = "weapon_pipe_rifle",
                    OwnerSurvivorId = "survivor_cutter", ConditionPct = 0.9f,
                    AmmoId = "ammo_357", AmmoRemaining = 20
                }
            };

            Check("encounter_start",
                combatA.BeginEncounter("enc_breach_a", "exp_a", "loc_test", "Test Cut", 4, 86, players, weapons, 1, 40f)
                && combatB.BeginEncounter("enc_breach_b", "exp_b", "loc_test", "Test Cut", 4, 86, players, weapons, 1, 40f));

            var invQuiet = MakeInv(("item_hydraulic_wire_cutter", 1));
            var invLoud = MakeInv(("item_linear_breach_section", 1));
            combatA.ConfigureBreachingLogistics(invQuiet, vehicleAvailable: false);
            combatB.ConfigureBreachingLogistics(invLoud, vehicleAvailable: false);

            var wireA = combatA.EnsureObstacleBarrier("bar_wire", "obstacle_concertina_wire");
            var doorB = combatB.EnsureObstacleBarrier("bar_door", "obstacle_armored_door");

            var beginQuiet = combatA.BeginBreach(wireA.Id, "breach_tool_hydraulic_cutter", operatorSkill01: 0.8f);
            var beginLoud = combatB.BeginBreach(doorB.Id, "breach_tool_linear_section", operatorSkill01: 0.8f);
            Check("begin_quiet_and_loud", beginQuiet.Success && beginLoud.Success,
                $"quiet={beginQuiet.Message} loud={beginLoud.Message}");

            // Drain setup + clear ticks with high skill to avoid incidental failure noise.
            for (int i = 0; i < 20 && !string.Equals(wireA.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal); i++)
                combatA.AdvanceBreach(wireA.Id, new SeededRng(8600 + i), operatorSkill01: 0.95f);
            for (int i = 0; i < 20 && !string.Equals(doorB.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal); i++)
                combatB.AdvanceBreach(doorB.Id, new SeededRng(8600 + i), operatorSkill01: 0.95f);

            Check("quiet_clears_wire",
                string.Equals(wireA.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal) && wireA.PathBlocking <= 0.01f,
                wireA.BreachPhase);
            Check("loud_clears_door",
                string.Equals(doorB.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal) && doorB.PathBlocking <= 0.01f,
                doorB.BreachPhase);
            Check("quiet_noise_below_loud",
                combatA.BreachEncounterNoise < combatB.BreachEncounterNoise,
                $"quiet={combatA.BreachEncounterNoise:F2} loud={combatB.BreachEncounterNoise:F2}");
            Check("loud_consumes_section",
                invLoud.CountById("item_linear_breach_section") == 0);

            // Unsupported method / vehicle gate
            var invWinch = MakeInv(("item_expedition_winch_kit", 1));
            var combatGate = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            combatGate.LoadBreachingCatalog(catalog);
            combatGate.BeginEncounter("enc_gate", "exp_g", "loc_test", "Gate", 1, 11, players, weapons, 1, 30f);
            combatGate.ConfigureBreachingLogistics(invWinch, vehicleAvailable: false);
            var hedgehog = combatGate.EnsureObstacleBarrier("bar_hog", "obstacle_anti_vehicle_hedgehog");
            var noVehicle = combatGate.EvaluateBreach(hedgehog.Id, "breach_tool_winch_assist");
            Check("vehicle_required_blocks", !noVehicle.CanExecute && noVehicle.Reason.Contains("vehicle_required"),
                noVehicle.Reason);
            combatGate.ConfigureBreachingLogistics(invWinch, vehicleAvailable: true);
            var withVehicle = combatGate.EvaluateBreach(hedgehog.Id, "breach_tool_winch_assist");
            Check("vehicle_available_allows", withVehicle.CanExecute, withVehicle.Reason);

            var unsupported = combatGate.EvaluateBreach(hedgehog.Id, "breach_tool_manual_shears");
            Check("unsupported_method_blocks", !unsupported.CanExecute && unsupported.Reason.Contains("unsupported_method"),
                unsupported.Reason);

            // Mid-breach save/load preserves progress fields via CloneBarriers.
            var midCombat = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            midCombat.LoadBreachingCatalog(catalog);
            midCombat.BeginEncounter("enc_mid", "exp_m", "loc_test", "Mid", 2, 42, players, weapons, 1, 30f);
            midCombat.ConfigureBreachingLogistics(MakeInv(("item_mechanical_breach_ram", 1)), false);
            var sandbag = midCombat.EnsureObstacleBarrier("bar_sand", "obstacle_sandbag_redoubt");
            midCombat.BeginBreach(sandbag.Id, "breach_tool_mechanical_ram", operatorSkill01: 0.7f);
            midCombat.AdvanceBreach(sandbag.Id, new SeededRng(4201), operatorSkill01: 0.7f);
            midCombat.AdvanceBreach(sandbag.Id, new SeededRng(4202), operatorSkill01: 0.7f);
            var captured = midCombat.CaptureState();
            var restored = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
            restored.LoadBreachingCatalog(catalog);
            restored.RestoreState(captured);
            var restoredBarrier = restored.FindBarrier("bar_sand");
            Check("mid_breach_save_fields",
                restoredBarrier != null
                && restoredBarrier.ObstacleProfileId == "obstacle_sandbag_redoubt"
                && !string.IsNullOrEmpty(restoredBarrier.ActiveBreachToolId)
                && restoredBarrier.BreachClearTicksTotal > 0
                && Math.Abs(restoredBarrier.BreachProgress01 - sandbag.BreachProgress01) < 0.0001f
                && string.Equals(restoredBarrier.BreachPhase, sandbag.BreachPhase, StringComparison.Ordinal),
                restoredBarrier == null
                    ? "missing barrier"
                    : $"phase={restoredBarrier.BreachPhase} progress={restoredBarrier.BreachProgress01:F3}");

            GD.Print($"combat-breaching selftest: {pass} passed, {fail} failed");
            if (fail > 0) GD.Print(string.Join("\n", details));
            return EmitSummary("combat_breaching_selftest", fail == 0, failedCount: fail, passedCount: pass,
                details: string.Join("; ", details));
        }
    }
}
