// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    /// <summary>
    /// B5–B8 Phase 0 fixture freeze (Plans 64–67 flagship checkpoint).
    ///
    /// Serializes representative <c>CaptureState</c> DTOs for the six shelter
    /// sections this flagship touches — greenhouse, power_grid, water_treatment,
    /// sump_flooding, airlock_security, perimeter_defense — into JSON fixtures,
    /// then pins that future code changes cannot alter the serialized shape
    /// without a deliberate, reviewed migration.
    ///
    /// Capture mode: run with environment variable <c>B5B8_CAPTURE_FIXTURES=1</c>
    /// to (re)write the fixture files. Capture mode is a deliberate, opt-in
    /// action and prints a loud banner so it never masks a regression.
    /// Default mode: every fixture must deserialize and re-serialize to
    /// byte-identical JSON (shape parity) with sentinel fields intact.
    ///
    /// The DTO instances below are populated directly (public field shapes are
    /// what the serializer persists; each system's CaptureState is a clone of
    /// these same DTOs), so the fixtures pin exactly what a live campaign
    /// writes for the populated fields.
    /// </summary>
    public sealed class B5B8Phase0SaveFixtureTests
    {
        private const string CaptureEnvVar = "B5B8_CAPTURE_FIXTURES";
        private static readonly string FixtureDir = ResolveFixtureDir();

        // ---- sentinel values (deliberately distinctive, non-default) ----------
        private const int SentinelDay = 47;
        private const float SentinelGeneration = 813.5f;
        private const float SentinelFuel = 62.25f;
        private const float SentinelBattery = 1234.5f;

        // ---- greenhouse -------------------------------------------------------

        [Fact]
        public void Greenhouse_Phase0Fixture_RoundTrips()
        {
            var state = new GreenhouseState
            {
                saveId = GreenhouseExpansionCatalog.SaveId,
                preWarWheatUnlocked = true,
                totalHarvests = 12,
                blightRollCount = 3456
            };
            state.plots.Add(new GreenhousePlotState
            {
                plotIndex = 0,
                seedItemId = "item_seeds_wheat",
                stage = (int)GreenhouseStage.Mature,
                growth = 8.5f,
                water = 72.5f,
                soilContamination = 4.25f,
                blight = 0.15f,
                plantedDay = 41
            });
            state.plots.Add(new GreenhousePlotState
            {
                plotIndex = 1,
                seedItemId = "item_seeds_tuber",
                stage = (int)GreenhouseStage.Fallow, // concurrent-stream fix: GreenhouseStage has no 'Empty'; Fallow = 0 is the empty-plot state
                growth = 0f,
                water = 0f,
                soilContamination = 0f,
                blight = 0f,
                plantedDay = 0
            });

            RoundTrip("greenhouse_phase0.json", state, s =>
            {
                Assert.Equal(2, s.plots.Count);
                Assert.Equal(72.5f, s.plots[0].water);
                Assert.Equal(0.15f, s.plots[0].blight);
                Assert.Equal(3456L, s.blightRollCount);
                Assert.True(s.preWarWheatUnlocked);
            });
        }

        // ---- power grid -------------------------------------------------------

        [Fact]
        public void PowerGrid_Phase0Fixture_RoundTrips()
        {
            var state = new PowerGridState
            {
                SimDay = SentinelDay,
                GenerationWatts = SentinelGeneration,
                FuelUnits = SentinelFuel,
                BatteryReserveWh = SentinelBattery,
                BatteryCapacityWh = 4000f,
                LastSurgeDay = 31
            };
            state.SetBreaker("room_workshop", false); // open breaker persists
            state.SetRoomPriority("room_greenhouse", PowerGridRoomPriority.Low);
            state.SetRoomPriority("room_clinic", PowerGridRoomPriority.Critical);
            state.MarkTripped("room_foundry", 44);

            RoundTrip("power_grid_phase0.json", state, s =>
            {
                Assert.Equal(SentinelDay, s.SimDay);
                Assert.Equal(SentinelGeneration, s.GenerationWatts);
                Assert.Equal(SentinelFuel, s.FuelUnits);
                Assert.Equal(SentinelBattery, s.BatteryReserveWh);
                Assert.Contains("room_workshop", s.ClosedBreakers);
                Assert.Contains("room_foundry", s.TrippedRooms);
                Assert.Equal(2, s.Priorities.Count);
                Assert.Equal(31, s.LastSurgeDay);
            });
        }

        // ---- water treatment --------------------------------------------------

        [Fact]
        public void WaterTreatment_Phase0Fixture_RoundTrips()
        {
            var state = new WaterTreatmentState
            {
                cleanWater = 118.75f,
                rawWater = 45.5f,
                brackishWater = 22.25f,
                irradiatedWater = 9.5f,
                filterIntegrity = 61.5f,
                filterMaxIntegrity = 100f,
                charcoalSupply = 7.5f,
                distillationFuel = 14.25f,
                activeMode = TreatmentMode.ReverseOsmosis,
                isProcessing = true,
                processingProgress = 0.45f,
                processingTarget = 30f,
                totalWaterProcessed = 921.5f,
                totalContaminationExposure = 3.75f,
                filterReplacements = 4
            };
            state.completedJobs.Add(new WaterTreatmentJob
            {
                mode = TreatmentMode.CharcoalFiltration,
                inputAmount = 50f,
                cleanOutput = 42.5f,
                wasteAmount = 7.5f,
                filterDegradation = 8f,
                fuelConsumed = 0f,
                contaminationRemoved = 12.5f,
                dayCompleted = 44
            });

            RoundTrip("water_treatment_phase0.json", state, s =>
            {
                Assert.Equal(118.75f, s.cleanWater);
                Assert.Equal(9.5f, s.irradiatedWater);
                Assert.Equal(TreatmentMode.ReverseOsmosis, s.activeMode);
                Assert.Single(s.completedJobs);
                Assert.Equal(42.5f, s.completedJobs[0].cleanOutput);
                Assert.Equal(4, s.filterReplacements);
            });
        }

        // ---- sump flooding ----------------------------------------------------

        [Fact]
        public void SumpFlooding_Phase0Fixture_RoundTrips()
        {
            var state = new SumpFloodingState
            {
                globalGroundwaterLevel = 63.25f,
                lastFloodDay = 43
            };
            state.nodes.Add(new SumpNode
            {
                nodeId = "sump_main",
                displayName = "Main Sump",
                waterLevelCm = 88.5f,
                maxWaterLevelCm = 200f,
                hasSumpPump = true,
                pumpCondition = 74.5f,
                pumpPowered = true,
                hasFloatValve = true,
                hasSandbagMitigation = false,
                isFlooded = false,
                equipmentDisabled = false
            });
            state.incidentLog.Add(new FloodIncident
            {
                day = 43,
                nodeId = "sump_main",
                kind = FloodIncidentKind.FloodStart,
                description = "phase0 fixture incident"
            });

            RoundTrip("sump_flooding_phase0.json", state, s =>
            {
                Assert.Single(s.nodes);
                Assert.Equal(74.5f, s.nodes[0].pumpCondition);
                Assert.True(s.nodes[0].pumpPowered);
                Assert.Equal(63.25f, s.globalGroundwaterLevel);
                Assert.Single(s.incidentLog);
                Assert.Equal(43, s.lastFloodDay);
            });
        }

        // ---- airlock security -------------------------------------------------

        [Fact]
        public void AirlockSecurity_Phase0Fixture_RoundTrips()
        {
            var state = new AirlockSecurityState
            {
                blastDoorIntegrity = 87.5f,
                doorState = AirlockDoorState.Secure,
                sentryId = "survivor_7",
                alertness = 92.25f,
                totalAdmissions = 19,
                totalTurnaways = 6
            };
            state.incidentLog.Add(new AirlockIncidentLog
            {
                day = 40,
                visitorId = "npc_traveler_3",
                decision = VisitorDecision.Quarantine,
                outcome = "phase0 fixture outcome"
            });

            RoundTrip("airlock_security_phase0.json", state, s =>
            {
                Assert.Equal(87.5f, s.blastDoorIntegrity);
                Assert.Equal(AirlockDoorState.Secure, s.doorState);
                Assert.Equal("survivor_7", s.sentryId);
                Assert.Single(s.incidentLog);
                Assert.Equal(VisitorDecision.Quarantine, s.incidentLog[0].decision);
            });
        }

        // ---- perimeter defense (Plan 203, schema_version 2) --------------------

        [Fact]
        public void PerimeterDefense_Phase0Fixture_RoundTrips()
        {
            var state = new PerimeterDefenseSave
            {
                systemId = "perimeter_defense",
                schema_version = 2,
                last_tick_day = SentinelDay,
                assault_count = 3
            };
            state.emplacements.Add(new EmplacementRuntimeState
            {
                emplacement_id = "emp_gate_turret_1",
                defense_id = "def_sentry_turret_9mm",
                current_hp = 165,
                max_hp = 200,
                is_active = true,
                is_destroyed = false,
                loaded_ammo_count = 22,
                required_ammo_type = "ammo_9x19",
                magazine_capacity = 30,
                is_jammed = false
            });
            state.sectors.Add(new PerimeterSectorState
            {
                sector_id = PerimeterSector.Gate,
                alarm_armed = true
            });
            state.sectors[0].emplacement_ids.Add("emp_gate_turret_1");

            RoundTrip("perimeter_defense_phase0.json", state, s =>
            {
                Assert.Equal(2, s.schema_version);
                Assert.Single(s.emplacements);
                Assert.Equal(22, s.emplacements[0].loaded_ammo_count);
                Assert.Equal("ammo_9x19", s.emplacements[0].required_ammo_type);
                Assert.Single(s.sectors);
                Assert.True(s.sectors[0].alarm_armed);
                Assert.Equal(3, s.assault_count);
            });
        }

        // ---- shared capture / verify plumbing ---------------------------------

        private static string ResolveFixtureDir()
        {
            // Ashfall.Core.Tests/bin/.../... → walk up to the repo root, then
            // into the canonical fixture tree.
            var dir = AppContext.BaseDirectory;
            for (int i = 0; i < 8; i++)
            {
                var candidate = Path.Combine(dir, "Ashfall.Core.Tests", "Fixtures", "B5B8_Phase0");
                if (Directory.Exists(Path.Combine(dir, "Ashfall.Core.Tests")))
                    return candidate;
                dir = Path.GetDirectoryName(dir)!;
            }
            throw new InvalidOperationException("Could not locate Ashfall.Core.Tests from " + AppContext.BaseDirectory);
        }

        private static void RoundTrip<T>(string fileName, T state, Action<T> assertions) where T : class
        {
            var serializer = new SystemTextJsonSerializer();
            var path = Path.Combine(FixtureDir, fileName);
            Directory.CreateDirectory(FixtureDir);

            string original = serializer.Serialize(state);

            if (Environment.GetEnvironmentVariable(CaptureEnvVar) == "1")
            {
                File.WriteAllText(path, original);
                Console.WriteLine($"[B5B8-PHASE0] CAPTURED {fileName} ({original.Length} bytes) — deliberate fixture capture via {CaptureEnvVar}=1");
            }

            Assert.True(File.Exists(path), $"Missing Phase 0 fixture '{path}'. Re-run tests with {CaptureEnvVar}=1 to capture.");

            string stored = File.ReadAllText(path);
            var restored = serializer.Deserialize<T>(stored);
            Assert.NotNull(restored);

            // Shape parity: deserialize(fixture) → serialize must be byte-identical.
            string reserialized = serializer.Serialize(restored!);
            Assert.True(stored == reserialized,
                $"{fileName}: serialized shape drifted from the Phase 0 fixture. " +
                "If this change is a deliberate, reviewed save migration, re-capture with " +
                $"{CaptureEnvVar}=1 and document it in the B5-B8 completion report. Otherwise restore shape parity.");

            assertions(restored!);
        }
    }
}
