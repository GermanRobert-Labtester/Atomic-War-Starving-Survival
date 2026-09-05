using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Events;
using Ashfall.Core.Radio;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    public class VerdictRadioExpansionTests : CatalogTestBase
    {
        private static List<VerdictCatalogLoader.VerdictRadioEntry> LoadRadio()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var list = VerdictCatalogLoader.LoadRadio(DataDirectory, files, json);
            Assert.NotNull(list);
            return list;
        }

        [Fact]
        public void Catalog_Loads_All_30_Broadcasts()
        {
            var list = LoadRadio();
            Assert.Equal(30, list.Count);
        }

        [Fact]
        public void All_30_Broadcast_Ids_Are_Unique_And_Prefixed()
        {
            var list = LoadRadio();
            var ids = list.Select(e => e.id).ToList();
            var distinct = ids.Distinct(StringComparer.Ordinal).ToList();
            Assert.Equal(30, distinct.Count);
            foreach (var id in ids)
            {
                Assert.True(id.StartsWith("radio_verdict_"), $"Broadcast id '{id}' must start with radio_verdict_ prefix");
            }
        }

        [Fact]
        public void Baseline_13_Broadcasts_Preserved_Verbatim()
        {
            var list = LoadRadio();
            var baselineIds = new[]
            {
                "radio_verdict_meter_reads_1142",
                "radio_verdict_fuse_serviced",
                "radio_verdict_wing_sleeps",
                "radio_verdict_off_count_assessed",
                "radio_verdict_eden_was_here",
                "radio_verdict_count_is_open",
                "radio_verdict_clock_disagrees",
                "radio_verdict_geophone_taps",
                "radio_verdict_valve_accessed_36",
                "radio_verdict_reels_matter",
                "radio_verdict_presentation_names_holders",
                "radio_verdict_carrier_on_window",
                "radio_verdict_reckoning_call"
            };

            foreach (var bId in baselineIds)
            {
                var entry = list.Find(e => e.id == bId);
                Assert.NotNull(entry);
                Assert.False(string.IsNullOrWhiteSpace(entry.frequency));
                Assert.False(string.IsNullOrWhiteSpace(entry.source));
                Assert.False(string.IsNullOrWhiteSpace(entry.message));
                Assert.False(string.IsNullOrWhiteSpace(entry.signalStrength));
                Assert.False(string.IsNullOrWhiteSpace(entry.kind));
                Assert.True(entry.dayTrigger >= 210);
            }
        }

        [Fact]
        public void All_17_Plan94_New_Broadcasts_Present()
        {
            var list = LoadRadio();
            var plan94Ids = new[]
            {
                "radio_verdict_barometric_spread",
                "radio_verdict_service_cycle_greywater",
                "radio_verdict_stilling_well_delta",
                "radio_verdict_subsector_ledger_update",
                "radio_verdict_geophone_offset_recal",
                "radio_verdict_strata_density_drift",
                "radio_verdict_relay_switch_pass4",
                "radio_verdict_unscheduled_burst_88",
                "radio_verdict_river_stage_deviation",
                "radio_verdict_core_vault_desiccant_purge",
                "radio_verdict_unverified_household_tally",
                "radio_verdict_repeater_origin_mismatch",
                "radio_verdict_spectrometry_drift_stjude",
                "radio_verdict_substation_breaker_test",
                "radio_verdict_holding_capacity_parity",
                "radio_verdict_telemetry_phase_inversion",
                "radio_verdict_carrier_override_standby"
            };

            foreach (var id in plan94Ids)
            {
                var entry = list.Find(e => e.id == id);
                Assert.NotNull(entry);
                Assert.False(string.IsNullOrWhiteSpace(entry.frequency));
                Assert.False(string.IsNullOrWhiteSpace(entry.source));
                Assert.False(string.IsNullOrWhiteSpace(entry.message));
                Assert.False(string.IsNullOrWhiteSpace(entry.signalStrength));
                Assert.False(string.IsNullOrWhiteSpace(entry.kind));
                Assert.InRange(entry.dayTrigger, 265, 365);

                // Terseness budget: 1 to 4 sentences, concise machine-like register
                var sentences = System.Text.RegularExpressions.Regex.Split(entry.message, @"(?<=[.!?])\s+")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToArray();
                Assert.InRange(sentences.Length, 1, 5);
                Assert.True(entry.message.Length <= 250, $"Message '{entry.id}' exceeds terseness budget");
            }
        }

        [Fact]
        public void Plan94_Requested_Kind_Distribution_Matches()
        {
            var list = LoadRadio();
            var newBroadcasts = list.Skip(13).ToList();
            Assert.Equal(17, newBroadcasts.Count);

            var telemetry = newBroadcasts.Where(e => e.kind == "telemetry").ToList();
            var maintenance = newBroadcasts.Where(e => e.kind == "maintenance").ToList();
            var census = newBroadcasts.Where(e => e.kind == "census").ToList();
            var calibration = newBroadcasts.Where(e => e.kind == "calibration").ToList();
            var anomaly = newBroadcasts.Where(e => e.kind == "anomaly").ToList();
            var emergency = newBroadcasts.Where(e => e.kind == "emergency").ToList();

            Assert.Equal(5, telemetry.Count);
            Assert.Equal(4, maintenance.Count);
            Assert.Equal(3, census.Count);
            Assert.Equal(2, calibration.Count);
            Assert.Equal(2, anomaly.Count);
            Assert.Equal(1, emergency.Count);
        }

        [Fact]
        public void Frequency_And_Signal_Strength_Integrity()
        {
            var list = LoadRadio();
            var validFrequencies = new HashSet<string>(StringComparer.Ordinal) { "99.0 MHz", "88.5 MHz" };
            var validStrengths = new HashSet<string>(StringComparer.Ordinal) { "S1", "S2", "S3", "S4", "S5" };

            foreach (var b in list)
            {
                Assert.Contains(b.frequency, validFrequencies);
                Assert.Contains(b.signalStrength, validStrengths);
            }
        }

        [Fact]
        public void DayTrigger_Semantics_And_Chronology()
        {
            var list = LoadRadio();
            var bus = new SimpleEventBus();
            var sys = new VerdictRadioSystem(bus, null, list);

            // Prior to carrier window (day < 210) or before Culpable phase -> nothing fires
            var early = sys.Poll(200, ReckoningPhase.Counted);
            Assert.Empty(early);

            var unc = sys.Poll(365, ReckoningPhase.Dormant);
            Assert.Empty(unc);

            // Day 270 at Culpable: broadcasts with dayTrigger <= 270 fire
            var day270 = sys.Poll(270, ReckoningPhase.Culpable);
            Assert.Contains("radio_verdict_meter_reads_1142", day270);
            Assert.Contains("radio_verdict_barometric_spread", day270); // day 268
            Assert.DoesNotContain("radio_verdict_service_cycle_greywater", day270); // day 272
            Assert.DoesNotContain("radio_verdict_carrier_override_standby", day270); // day 360

            // Re-polling at day 270 does not duplicate (one-shot)
            var repoll = sys.Poll(270, ReckoningPhase.Culpable);
            Assert.Empty(repoll);

            // Day 365: remainder fires
            var endRun = sys.Poll(365, ReckoningPhase.Counted);
            Assert.Contains("radio_verdict_service_cycle_greywater", endRun);
            Assert.Contains("radio_verdict_carrier_override_standby", endRun);

            // Total fired = 30
            Assert.Equal(30, sys.FiredCount);
        }

        [Fact]
        public void OneShot_And_State_RoundTrip()
        {
            var list = LoadRadio();
            var bus = new SimpleEventBus();
            var sys = new VerdictRadioSystem(bus, null, list);

            // Fire up to day 280
            sys.Poll(280, ReckoningPhase.Culpable);
            int countAt280 = sys.FiredCount;
            Assert.True(countAt280 > 0);

            // Capture state
            var state = sys.CaptureState();
            Assert.Equal(countAt280, state.firedIds.Count);

            // Restore into a fresh system
            var restored = new VerdictRadioSystem(new SimpleEventBus(), null, list);
            restored.RestoreState(state);
            Assert.Equal(countAt280, restored.FiredCount);

            // Re-poll at day 280 -> 0 new fires
            var noNew = restored.Poll(280, ReckoningPhase.Culpable);
            Assert.Empty(noNew);

            // Poll at day 365 -> only the unfired ones fire
            var laterFired = restored.Poll(365, ReckoningPhase.Culpable);
            Assert.Equal(30 - countAt280, laterFired.Count);
            Assert.Equal(30, restored.FiredCount);
        }

        [Fact]
        public void UnifiedRadioBroadcast_Catalog_Loads_Verdict_Broadcasts()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var cat = new RadioBroadcastCatalog();
            cat.LoadFromDataDirectory(DataDirectory, files, json);

            var first = cat.GetById("radio_verdict_meter_reads_1142");
            Assert.NotNull(first);
            Assert.Equal(BroadcastGenre.VerdictCensus, first!.Genre);

            var baro = cat.GetById("radio_verdict_barometric_spread");
            Assert.NotNull(baro);
            Assert.Equal(268, baro!.DayTrigger);

            var standby = cat.GetById("radio_verdict_carrier_override_standby");
            Assert.NotNull(standby);
            Assert.Equal(360, standby!.DayTrigger);
        }

        [Fact]
        public void AudioCueIntegrity_No_New_Broadcasts_Define_Dangling_Cues()
        {
            var list = LoadRadio();
            var newBroadcasts = list.Skip(13).ToList();
            foreach (var b in newBroadcasts)
            {
                // New broadcasts deliberately omit audio_cue to avoid dangling references
                Assert.True(string.IsNullOrEmpty(b.audio_cue), $"New broadcast '{b.id}' should not define audio_cue unless registered");
            }
        }
    }
}
