// SPDX-License-Identifier: MIT
// Plan 29 Phase 3 — Task 29B machine personality pilot tests.
// Pins: machine catalog loads + validates, machines bind to canonical rooms,
// diagnostic tells fire exactly at the owning systems' own thresholds (mechanical
// truthfulness, cross-checked on REAL StartingLevelSystem / SilentFoundrySystem
// instances), personality tells are never fault-styled, and evaluation is
// deterministic. No new condition state: tells are pure projections.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core.Tests
{
    public class ShelterMachineTellTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _files = new FileSystemIO();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();

        public ShelterMachineTellTests()
        {
            string baseDir = AppContext.BaseDirectory;
            _dataDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
        }

        public void Dispose() { }

        private ShelterMachineTellCatalog LoadMachineCatalog() =>
            ShelterMachineTellCatalog.Load(_files, _json, _dataDir);

        private ShelterRoomIdentityCatalog LoadRoomCatalog() =>
            ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);

        // ── Load & validate ───────────────────────────────────────────────

        [Fact]
        public void MachineCatalog_Loads_FromDataAuthority_AndValidates()
        {
            var catalog = LoadMachineCatalog();
            Assert.Equal(7, catalog.MachineCount);
            Assert.Equal(20, catalog.Quirks.Count);
            Assert.Equal(11, catalog.GlitchEvents.Count);
            Assert.Empty(catalog.Validate());
        }

        [Fact]
        public void MachineCatalog_MissingFile_YieldsEmptyValidCatalog()
        {
            var catalog = ShelterMachineTellCatalog.Load(_files, _json, Path.Combine(_dataDir, "no_such_dir"));
            Assert.Equal(0, catalog.MachineCount);
            Assert.Empty(catalog.Validate());
        }

        // ── Identity & naming policy (§29B.2–29B.4) ───────────────────────

        [Fact]
        public void Machines_BindToCanonicalRooms_WithRealConditionOwners()
        {
            var machines = LoadMachineCatalog();
            var rooms = ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);

            var hepa = machines.GetMachine("machine_hepa_stack");
            var foundry = machines.GetMachine("machine_foundry_cupola");
            Assert.NotNull(hepa);
            Assert.NotNull(foundry);

            // Machine→room bindings resolve to canonical shelter rooms.
            Assert.Equal("room_filtration", rooms.ResolveRoomId(hepa.room_id));
            Assert.Equal("room_foundry", foundry.room_id);
            Assert.NotNull(rooms.GetRoomIdentity(hepa.room_id));

            // Condition owners are the real systems (provenance chain, §29B.2).
            Assert.Contains("StartingLevelSystem", hepa.condition_owner);
            Assert.Contains("SilentFoundrySystem", foundry.condition_owner);

            // §29B.3/29B.4: sparse, survivor-made naming. The HEPA stack earns a
            // nickname; the foundry keeps its canonical name and stays technical.
            Assert.Equal("The Lung", hepa.nickname);
            Assert.True(string.IsNullOrEmpty(foundry.nickname), "the foundry is canonically 'The Silent Foundry' — no nickname");
            Assert.False(string.IsNullOrWhiteSpace(foundry.display_name));
        }

        // ── Truthfulness: tells sit on the owner's real thresholds (§1.5) ──

        [Fact]
        public void HepaTell_FiresExactlyWhenOwnerRaisesItsAirHazard()
        {
            // Drive a real StartingLevelSystem across its own warning threshold.
            var system = new StartingLevelSystem();
            Assert.False(system.State.airHazardWarning);

            int guard = 0;
            while (!system.State.airHazardWarning && guard++ < 60)
                system.TickDay(isFilterDutyAssigned: false, outdoorWeather: WeatherKind.Clear);

            Assert.True(system.State.airHazardWarning, "real system should reach its air-hazard warning");
            Assert.True(system.State.airFilterHealthPercent < 50f);

            var catalog = LoadMachineCatalog();
            var readings = new MachineConditionReadings
            {
                HepaFilterHealth = system.State.airFilterHealthPercent,
                HepaRadon = system.State.radonLevelBqm3
            };
            var tellIds = catalog.EvaluateQuirks("machine_hepa_stack", readings).Select(q => q.id).ToList();
            Assert.Contains("machine_quirk_hepa_intake_whistle", tellIds);

            // Healthy plant: no diagnostic tells at all.
            var fresh = new MachineConditionReadings(); // 100 everywhere
            Assert.DoesNotContain("machine_quirk_hepa_intake_whistle",
                catalog.EvaluateQuirks("machine_hepa_stack", fresh).Select(q => q.id));
        }

        [Fact]
        public void HepaBand_MatchesOwnerWarningState()
        {
            var catalog = LoadMachineCatalog();

            // At/above 50 the owner is quiet; below 50 it warns. The band agrees.
            // (Healthy >= 70; 55 is wearing but not yet at the owner's service floor.)
            Assert.Equal(MachineConditionBand.Worn, catalog.EvaluateBand("machine_hepa_stack",
                new MachineConditionReadings { HepaFilterHealth = 55f }));
            Assert.Equal(MachineConditionBand.Healthy, catalog.EvaluateBand("machine_hepa_stack",
                new MachineConditionReadings { HepaFilterHealth = 70f }));
            Assert.Equal(MachineConditionBand.ServiceDue, catalog.EvaluateBand("machine_hepa_stack",
                new MachineConditionReadings { HepaFilterHealth = 49f }));

            // Escalation below the service floor: Critical under 25, Failed at 0.
            Assert.Equal(MachineConditionBand.Critical, catalog.EvaluateBand("machine_hepa_stack",
                new MachineConditionReadings { HepaFilterHealth = 24.9f }));
            Assert.Equal(MachineConditionBand.Failed, catalog.EvaluateBand("machine_hepa_stack",
                new MachineConditionReadings { HepaFilterHealth = 0f }));
        }

        [Fact]
        public void FoundryTells_MirrorOwnerSafetyWarningFloors()
        {
            var catalog = LoadMachineCatalog();

            // The owner's own floors (SilentFoundrySystem.GetSafetyWarnings):
            // hearthTuyeres < 35, safetyExhaust < 30. The tells use exactly these.
            var warn = new MachineConditionReadings
            {
                FoundryHearthTuyeres = 34f,
                FoundrySafetyExhaust = 29f
            };
            var fired = catalog.EvaluateQuirks("machine_foundry_cupola", warn).Select(q => q.id).ToList();
            Assert.Contains("machine_quirk_foundry_tuyere_knock", fired);
            Assert.Contains("machine_quirk_foundry_exhaust_whine", fired);

            // Above the floors the tells clear — condition improvement clears tell.
            var healthy = new MachineConditionReadings();
            Assert.DoesNotContain("machine_quirk_foundry_tuyere_knock",
                catalog.EvaluateQuirks("machine_foundry_cupola", healthy).Select(q => q.id));
            Assert.DoesNotContain("machine_quirk_foundry_exhaust_whine",
                catalog.EvaluateQuirks("machine_foundry_cupola", healthy).Select(q => q.id));
        }

        [Fact]
        public void FoundryTell_FiresExactlyWhenLiveOwnerWouldWarn()
        {
            // Cross-check against the REAL owner: when GetSafetyWarnings() complains
            // about the tuyeres, the projector must name that tell — and not before.
            var foundry = new SilentFoundrySystem(rng: new SeededRng(1234));

            var healthyReadings = ReadFrom(foundry);
            Assert.DoesNotContain("machine_quirk_foundry_tuyere_knock",
                LoadMachineCatalog().EvaluateQuirks("machine_foundry_cupola", healthyReadings).Select(q => q.id));

            foundry.State.hearthTuyeres = 34f;
            Assert.Contains("Hearth brick and tuyeres are badly worn", string.Join("\n", foundry.GetSafetyWarnings()));
            var warnReadings = ReadFrom(foundry);
            Assert.Contains("machine_quirk_foundry_tuyere_knock",
                LoadMachineCatalog().EvaluateQuirks("machine_foundry_cupola", warnReadings).Select(q => q.id));
        }

        [Fact]
        public void FoundryBand_UsesTheOwnerOverallCondition()
        {
            var foundry = new SilentFoundrySystem(rng: new SeededRng(7));
            foundry.State.hearthTuyeres = 60f;
            foundry.State.refractoryLining = 60f;
            foundry.State.sandBeds = 60f;
            foundry.State.structuralSupports = 60f;
            foundry.State.safetyExhaust = 60f;

            float average = foundry.AverageFacilityCondition(); // the owner's own overall figure
            var catalog = LoadMachineCatalog();
            Assert.Equal(ShelterMachineTellCatalog.BandFor(average),
                catalog.EvaluateBand("machine_foundry_cupola", ReadFrom(foundry)));
            Assert.Equal(MachineConditionBand.Worn, catalog.EvaluateBand("machine_foundry_cupola", ReadFrom(foundry)));
        }

        // ── Harmless vs diagnostic (§29B.9) ───────────────────────────────

        [Fact]
        public void PersonalityTell_AlwaysPresent_NeverFaultStyled()
        {
            var catalog = LoadMachineCatalog();
            var healthyReadings = new MachineConditionReadings(); // everything healthy

            var tells = catalog.EvaluateQuirks("machine_hepa_stack", healthyReadings);
            var personality = tells.Where(q => q.kind == MachineQuirkKinds.Personality).ToList();
            var diagnostics = tells.Where(q => q.kind == MachineQuirkKinds.Diagnostic).ToList();

            // The harmless tick is stable behaviour on a healthy plant (§29B.9)...
            Assert.Single(personality);
            Assert.Equal("machine_quirk_hepa_housing_tick", personality[0].id);
            // ...and no diagnostic tell exists while the machine is healthy.
            Assert.Empty(diagnostics);
        }

        [Fact]
        public void PersonalityAndDiagnosticTells_AreClassifiedInData()
        {
            var catalog = LoadMachineCatalog();
            foreach (var quirk in catalog.Quirks)
            {
                if (string.Equals(quirk.kind, MachineQuirkKinds.Personality, StringComparison.Ordinal))
                {
                    Assert.Equal("info", quirk.severity);            // §29B.9 styling rule
                    Assert.True(string.IsNullOrWhiteSpace(quirk.condition_key));
                }
                else
                {
                    Assert.False(string.IsNullOrWhiteSpace(quirk.maintenance_action)); // §29B.14
                    Assert.True(quirk.trigger_below > 0f && quirk.trigger_below <= 100f);
                }
            }
        }

        // ── Determinism (§12) ─────────────────────────────────────────────

        [Fact]
        public void Evaluation_IsDeterministic_AndAuthoredOrdered()
        {
            var catalog = LoadMachineCatalog();
            var readings = new MachineConditionReadings
            {
                FoundryHearthTuyeres = 30f,
                FoundrySafetyExhaust = 29f,
                HepaFilterHealth = 40f
            };
            var first = catalog.EvaluateQuirks("machine_foundry_cupola", readings).Select(q => q.id).ToList();
            var again = catalog.EvaluateQuirks("machine_foundry_cupola", readings).Select(q => q.id).ToList();
            Assert.Equal(first, again);
            Assert.Equal(new[] { "machine_quirk_foundry_tuyere_knock", "machine_quirk_foundry_exhaust_whine", "machine_quirk_foundry_heat_shimmer", "machine_quirk_foundry_vibration_tune" }, first);
        }

        [Fact]
        public void UnknownMachineOrMissingCondition_YieldsNothing()
        {
            var catalog = LoadMachineCatalog();
            Assert.Empty(catalog.EvaluateQuirks("machine_nonexistent", new MachineConditionReadings()));
        }

        [Fact]
        public void BandFor_MapsTheDocumentedBands()
        {
            Assert.Equal(MachineConditionBand.Healthy, ShelterMachineTellCatalog.BandFor(100f));
            Assert.Equal(MachineConditionBand.Worn, ShelterMachineTellCatalog.BandFor(69.9f));
            Assert.Equal(MachineConditionBand.ServiceDue, ShelterMachineTellCatalog.BandFor(49.9f));
            Assert.Equal(MachineConditionBand.Critical, ShelterMachineTellCatalog.BandFor(24.9f));
            Assert.Equal(MachineConditionBand.Failed, ShelterMachineTellCatalog.BandFor(0f));
        }

        [Fact]
        public void GlitchEvents_Harmless_AlwaysEligible_AndOneShotGatedByJournal()
        {
            var catalog = LoadMachineCatalog();
            Assert.True(catalog.GlitchEvents.Count >= 3, "expect at least 3 glitch events (2 harmless + 1 real)");

            var readings = new MachineConditionReadings(); // all defaults = nominal
            var notedOnce = new HashSet<string>(StringComparer.Ordinal);
            bool IsNoted(string id) => notedOnce.Contains(id);

            // First pass: harmless glitches fire, one-shots get journaled
            var firstPass = catalog.EvaluateGlitchEvents("machine_airlock_machinery", readings, IsNoted);
            Assert.Contains(firstPass, g => string.Equals(g.id, "glitch_21_phantom_draft", StringComparison.Ordinal));
            Assert.Contains(firstPass, g => string.Equals(g.id, "glitch_23_old_intercom_burst", StringComparison.Ordinal));

            // Journal the one-shots
            foreach (var gl in firstPass)
                if (string.Equals(gl.repeat_policy, "once", StringComparison.Ordinal))
                    notedOnce.Add(gl.id);

            // Second pass: one-shots suppressed
            var secondPass = catalog.EvaluateGlitchEvents("machine_airlock_machinery", readings, IsNoted);
            Assert.DoesNotContain(secondPass, g => string.Equals(g.id, "glitch_21_phantom_draft", StringComparison.Ordinal));
            Assert.DoesNotContain(secondPass, g => string.Equals(g.id, "glitch_23_old_intercom_burst", StringComparison.Ordinal));
        }

        [Fact]
        public void GlitchEvents_RealFault_FiresOnlyWhenThresholdMet()
        {
            var catalog = LoadMachineCatalog();
            var readings = new MachineConditionReadings
            {
                PowerBatteryReserve = 5f, // below 10 → ground loop eligible
                VentilationFilterSaturation = 85f // above 80 → stuck damper eligible
            };

            bool neverNoted(string _) => false;
            var genGlitches = catalog.EvaluateGlitchEvents("machine_generator", readings, neverNoted);
            Assert.Contains(genGlitches, g => string.Equals(g.id, "glitch_25_ground_loop", StringComparison.Ordinal));

            var ventGlitches = catalog.EvaluateGlitchEvents("machine_ventilation_plant", readings, neverNoted);
            Assert.Contains(ventGlitches, g => string.Equals(g.id, "glitch_26_stuck_damper", StringComparison.Ordinal));
        }

        [Fact]
        public void QuirkComparison_Above_TriggersWhenValueExceedsThreshold()
        {
            var catalog = LoadMachineCatalog();
            // ventilation_loaded_rattle: comparison=above, trigger_below=80 → fires at 85
            var readings = new MachineConditionReadings { VentilationFilterSaturation = 85f };
            var quirks = catalog.EvaluateQuirks("machine_ventilation_plant", readings);
            Assert.Contains(quirks, q => string.Equals(q.id, "machine_quirk_ventilation_loaded_rattle", StringComparison.Ordinal));

            // same quirk must NOT fire at 75
            var readingsLow = new MachineConditionReadings { VentilationFilterSaturation = 75f };
            var quirksLow = catalog.EvaluateQuirks("machine_ventilation_plant", readingsLow);
            Assert.DoesNotContain(quirksLow, q => string.Equals(q.id, "machine_quirk_ventilation_loaded_rattle", StringComparison.Ordinal));
        }

        [Fact]
        public void GlitchEvents_UnknownMachine_ReturnsEmpty()
        {
            var catalog = LoadMachineCatalog();
            Assert.Empty(catalog.EvaluateGlitchEvents("machine_nonexistent", new MachineConditionReadings()));
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private static MachineConditionReadings ReadFrom(SilentFoundrySystem foundry) => new MachineConditionReadings
        {
            FoundryRefractoryLining = foundry.GetComponentCondition(FoundryFacilityComponent.RefractoryLining),
            FoundryHearthTuyeres = foundry.GetComponentCondition(FoundryFacilityComponent.HearthTuyeres),
            FoundrySandBeds = foundry.GetComponentCondition(FoundryFacilityComponent.SandBeds),
            FoundryStructuralSupports = foundry.GetComponentCondition(FoundryFacilityComponent.StructuralSupports),
            FoundrySafetyExhaust = foundry.GetComponentCondition(FoundryFacilityComponent.SafetyExhaust)
        };
    }
}
