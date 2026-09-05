// SPDX-License-Identifier: MIT
// Plan 29 consumer side — machine tell → audio condition sync tests.
// Pins: healthy readings sustain only the seven personality beds, diagnostic
// tells start exactly at the owning systems' own thresholds (strict
// comparisons), recovery stops them while personality persists, repeated
// applies are no-ops (threshold-transition firing, not continuous re-fire),
// foreign conditions are never touched, bus mapping + loop resolver behave,
// and the sync is deterministic. Real catalog from the data authority.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class MachineTellAudioSyncTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _files = new FileSystemIO();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();

        public MachineTellAudioSyncTests()
        {
            string baseDir = AppContext.BaseDirectory;
            _dataDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
        }

        private ShelterMachineTellCatalog LoadMachineCatalog() =>
            ShelterMachineTellCatalog.Load(_files, _json, _dataDir);

        /// <summary>All plant healthy: every diagnostic gate sits on the safe side.</summary>
        private static MachineConditionReadings HealthyReadings() => new MachineConditionReadings
        {
            HepaFilterHealth = 100f,
            HepaRadon = 12f,
            HazardWeather = false,
            FoundryRefractoryLining = 100f,
            FoundryHearthTuyeres = 100f,
            FoundrySandBeds = 100f,
            FoundryStructuralSupports = 100f,
            FoundrySafetyExhaust = 100f,
            PowerFuelUnits = 100f,
            PowerBatteryReserve = 100f,
            PowerBrownout = false,
            VentilationFilterSaturation = 0f,
            VentilationDuctIntegrity = 100f,
            VentilationSmokeSoot = 0f,
            WaterFilterIntegrity = 100f,
            ThermalBoilerFuel = 100f,
            AirlockIncidentActive = false
        };

        // ── Healthy baseline: personality beds only ───────────────────────

        [Fact]
        public void HealthyReadings_StartOnlyPersonalityBeds()
        {
            var catalog = LoadMachineCatalog();
            var audio = new AudioConditionSystem();

            var outcome = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);

            Assert.Equal(7, outcome.Started.Count);
            Assert.Empty(outcome.Stopped);
            Assert.Equal(7, outcome.ActiveTotal);
            // Personality tells are stable behaviour — never threshold-bound.
            foreach (var quirkId in outcome.Started)
                Assert.Equal("personality", catalog.GetQuirk(quirkId)?.kind);
        }

        // ── Threshold truthfulness: tells fire exactly at the owners' floors ──

        [Fact]
        public void HepaWhistle_FiresExactlyBelow_OwnerWarningFloor()
        {
            var catalog = LoadMachineCatalog();

            // StartingLevelSystem warns at filter < 50 — 50 itself stays quiet.
            var atFloor = HealthyReadings();
            atFloor.HepaFilterHealth = 50f;
            var audioAtFloor = new AudioConditionSystem();
            var outcomeAtFloor = MachineTellAudioSync.Apply(catalog, atFloor, audioAtFloor);
            Assert.DoesNotContain("machine_quirk_hepa_intake_whistle",
                audioAtFloor.State.activeConditions.Select(c => c.conditionId));

            var below = HealthyReadings();
            below.HepaFilterHealth = 49.9f;
            var audioBelow = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, below, audioBelow);
            Assert.Contains("machine_quirk_hepa_intake_whistle",
                audioBelow.State.activeConditions.Select(c => c.conditionId));
            Assert.Equal(outcomeAtFloor.Started.Count, 7); // at-floor stays personality-only
        }

        [Fact]
        public void DegradedHepa_StartsWhistleStormCoughAndRadonHum()
        {
            var catalog = LoadMachineCatalog();
            var readings = HealthyReadings();
            readings.HepaFilterHealth = 40f;   // < 50 whistle floor, < 70 cough floor
            readings.HazardWeather = true;     // cough's authoritative context gate
            readings.HepaRadon = 60f;          // > 50 radon hum gate

            var audio = new AudioConditionSystem();
            var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);

            Assert.Contains("machine_quirk_hepa_intake_whistle", outcome.Started);
            Assert.Contains("machine_quirk_hepa_storm_cough", outcome.Started);
            Assert.Contains("machine_quirk_hepa_radon_hum", outcome.Started);
            Assert.Equal(10, outcome.ActiveTotal); // 7 personality + 3 hepa diagnostics
        }

        [Fact]
        public void StormCough_RequiresHazardWeatherContext()
        {
            var catalog = LoadMachineCatalog();
            var readings = HealthyReadings();
            readings.HepaFilterHealth = 40f; // below both hepa floors…
            readings.HazardWeather = false; // …but clean air after the storm

            var audio = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, readings, audio);

            Assert.Contains("machine_quirk_hepa_intake_whistle",
                audio.State.activeConditions.Select(c => c.conditionId));
            Assert.DoesNotContain("machine_quirk_hepa_storm_cough",
                audio.State.activeConditions.Select(c => c.conditionId));
        }

        [Fact]
        public void AllFaults_ActivateAllTwentyCuedTells()
        {
            var catalog = LoadMachineCatalog();
            var readings = new MachineConditionReadings
            {
                HepaFilterHealth = 10f,
                HepaRadon = 80f,
                HazardWeather = true,
                FoundryRefractoryLining = 20f,
                FoundryHearthTuyeres = 20f,    // < 35 tuyere knock
                FoundrySandBeds = 20f,
                FoundryStructuralSupports = 20f,
                FoundrySafetyExhaust = 20f,    // < 30 exhaust whine
                PowerFuelUnits = 5f,           // < 20 fuel cough
                PowerBatteryReserve = 4f,      // < 5 brownout flicker (also < 10 relay chatter)
                PowerBrownout = true,
                VentilationFilterSaturation = 90f, // > 80 rattle and > 60 soot
                VentilationDuctIntegrity = 40f,
                VentilationSmokeSoot = 70f,
                WaterFilterIntegrity = 20f,    // < 40 RO choke
                ThermalBoilerFuel = 5f,        // < 15 boiler cutout
                AirlockIncidentActive = true   // 1 > 0.5 seal drag
            };

            var audio = new AudioConditionSystem();
            var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);

            Assert.Equal(catalog.Quirks.Count, outcome.ActiveTotal); // all 20 carry audio cues
            Assert.Equal(13, outcome.Started.Count - 7);             // 13 diagnostics over the beds
        }

        // ── Transition semantics: start on crossing, stop on recovery ─────

        [Fact]
        public void SecondApply_IsANoOp_ThresholdTransitionNotContinuous()
        {
            var catalog = LoadMachineCatalog();
            var readings = HealthyReadings();
            readings.HepaFilterHealth = 40f;
            var audio = new AudioConditionSystem();

            MachineTellAudioSync.Apply(catalog, readings, audio);
            var second = MachineTellAudioSync.Apply(catalog, readings, audio);

            Assert.True(second.Clean);
            Assert.Empty(second.Started);
            Assert.Empty(second.Stopped);
        }

        [Fact]
        public void Recovery_StopsDiagnostics_KeepsPersonalityBeds()
        {
            var catalog = LoadMachineCatalog();
            var degraded = HealthyReadings();
            degraded.HepaFilterHealth = 40f;
            degraded.HazardWeather = true;
            var audio = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, degraded, audio);

            var recovered = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);

            Assert.Contains("machine_quirk_hepa_intake_whistle", recovered.Stopped);
            Assert.Contains("machine_quirk_hepa_storm_cough", recovered.Stopped);
            Assert.Empty(recovered.Started);
            Assert.Equal(7, recovered.ActiveTotal);
        }

        // ── Namespacing, buses, loop knowledge ────────────────────────────

        [Fact]
        public void ForeignConditions_AreNeverStoppedOrCounted()
        {
            var catalog = LoadMachineCatalog();
            var audio = new AudioConditionSystem();
            Assert.True(audio.StartCondition("radon_alarm", "alerts", "rad_alert_acute", 1f, false).IsSuccess);

            var outcome = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);

            Assert.Empty(outcome.Stopped);
            Assert.Equal(7, outcome.ActiveTotal); // foreign condition not counted
            Assert.Contains(audio.State.activeConditions,
                c => c.conditionId == "radon_alarm" && c.isActive);
        }

        [Fact]
        public void BusMapping_RoutesFamiliesToConditionBuses()
        {
            Assert.Equal("ventilation", MachineTellAudioSync.BusForMachine("machine_hepa_stack"));
            Assert.Equal("ventilation", MachineTellAudioSync.BusForMachine("machine_ventilation_plant"));
            Assert.Equal("generator", MachineTellAudioSync.BusForMachine("machine_generator"));
            Assert.Equal("ambient", MachineTellAudioSync.BusForMachine("machine_foundry_cupola"));
            Assert.Equal("ambient", MachineTellAudioSync.BusForMachine("machine_boiler"));

            var catalog = LoadMachineCatalog();
            var audio = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);

            var hepaTick = audio.State.activeConditions
                .First(c => c.conditionId == "machine_quirk_hepa_housing_tick");
            Assert.Equal("ventilation", hepaTick.bus);
            var generatorTick = audio.State.activeConditions
                .First(c => c.conditionId == "machine_quirk_generator_vibration_tick");
            Assert.Equal("generator", generatorTick.bus);
            var boilerTick = audio.State.activeConditions
                .First(c => c.conditionId == "machine_quirk_boiler_jacket_tick");
            Assert.Equal("ambient", boilerTick.bus);
        }

        [Fact]
        public void LoopResolver_CarriesHostCueKnowledge()
        {
            var catalog = LoadMachineCatalog();
            var degraded = HealthyReadings();
            degraded.HepaFilterHealth = 40f;
            degraded.HepaRadon = 60f;

            // Host says the whistle is a one-shot crossing cue; the radon hum sustains.
            var audio = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, degraded, audio,
                cue => !string.Equals(cue, "hepa_intake_whistle", StringComparison.Ordinal));

            var whistle = audio.State.activeConditions
                .First(c => c.conditionId == "machine_quirk_hepa_intake_whistle");
            Assert.False(whistle.isLooping);
            var radonHum = audio.State.activeConditions
                .First(c => c.conditionId == "machine_quirk_hepa_radon_hum");
            Assert.True(radonHum.isLooping);

            // Null resolver defaults to sustained tells.
            var audioDefault = new AudioConditionSystem();
            MachineTellAudioSync.Apply(catalog, degraded, audioDefault);
            Assert.All(audioDefault.State.activeConditions.Where(c => c.isActive),
                c => Assert.True(c.isLooping));
        }

        // ── Determinism & guards ──────────────────────────────────────────

        [Fact]
        public void Apply_IsDeterministic_AcrossFreshSystems()
        {
            var catalog = LoadMachineCatalog();
            var readings = HealthyReadings();
            readings.HepaFilterHealth = 40f;
            readings.HazardWeather = true;
            readings.PowerFuelUnits = 5f;

            var audioA = new AudioConditionSystem();
            var audioB = new AudioConditionSystem();
            var outcomeA = MachineTellAudioSync.Apply(catalog, readings, audioA);
            var outcomeB = MachineTellAudioSync.Apply(catalog, readings, audioB);

            Assert.Equal(outcomeA.Started, outcomeB.Started);
            Assert.Equal(outcomeA.ActiveTotal, outcomeB.ActiveTotal);
        }

        [Fact]
        public void NullArguments_YieldEmptyOutcome()
        {
            var catalog = LoadMachineCatalog();
            var audio = new AudioConditionSystem();

            Assert.True(MachineTellAudioSync.Apply(null!, HealthyReadings(), audio).Clean);
            Assert.True(MachineTellAudioSync.Apply(catalog, null!, audio).Clean);
            Assert.True(MachineTellAudioSync.Apply(catalog, HealthyReadings(), null!).Clean);
        }
    }
}
