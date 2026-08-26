using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — unit tests for the nine Verdict
    /// systems that previously had only integration-level coverage.
    /// </summary>
    public class VerdictUnitTests
    {
        // ── EvidenceLedger ──────────────────────────────────────────────────────

        [Fact]
        public void Evidence_Enroll_IsIdempotent()
        {
            var ledger = new EvidenceLedger();
            ledger.Register(new EvidenceDefinition { id = "ev_a" });
            Assert.True(ledger.Enroll("ev_a", 160));
            Assert.False(ledger.Enroll("ev_a", 161));
            Assert.Equal(1, ledger.Count);
        }

        [Fact]
        public void Evidence_Enroll_RejectsUnknown_WhenCatalogPopulated()
        {
            var ledger = new EvidenceLedger();
            ledger.Register(new EvidenceDefinition { id = "ev_a" });
            Assert.False(ledger.Enroll("ev_unknown", 160));
            Assert.Equal(0, ledger.Count);
        }

        [Fact]
        public void Evidence_Enroll_AllowsAny_WhenCatalogEmpty()
        {
            var ledger = new EvidenceLedger();
            Assert.True(ledger.Enroll("ev_any", 160));
            Assert.Equal(1, ledger.Count);
        }

        [Fact]
        public void Evidence_FiresEvent()
        {
            var ledger = new EvidenceLedger();
            string fired = null;
            ledger.OnEnrolled += id => fired = id;
            ledger.Enroll("ev_x", 200);
            Assert.Equal("ev_x", fired);
        }

        [Fact]
        public void Evidence_CaptureRestore_Roundtrip()
        {
            var ledger = new EvidenceLedger();
            ledger.Enroll("ev_1", 160);
            ledger.Enroll("ev_2", 180);
            var snap = ledger.CaptureState();

            var restored = new EvidenceLedger();
            restored.RestoreState(snap);
            Assert.Equal(2, restored.Count);
            Assert.True(restored.IsEnrolled("ev_1"));
            Assert.True(restored.IsEnrolled("ev_2"));
        }

        [Fact]
        public void Evidence_RejectNullEmpty()
        {
            var ledger = new EvidenceLedger();
            Assert.False(ledger.Enroll("", 0));
            Assert.False(ledger.Enroll(null, 0));
            Assert.False(ledger.IsEnrolled(""));
            Assert.False(ledger.IsEnrolled(null));
        }

        // ── MachineLogSystem ────────────────────────────────────────────────────

        [Fact]
        public void MachineLog_Post_DuplicateSuppression()
        {
            var log = new MachineLogSystem();
            Assert.True(log.Post("fac_a", 160, "operating", "body", "ev_a"));
            Assert.False(log.Post("fac_a", 160, "operating", "body2", "ev_b"));
            Assert.Single(log.Entries);
        }

        [Fact]
        public void MachineLog_Post_DifferentKind_Allowed()
        {
            var log = new MachineLogSystem();
            Assert.True(log.Post("fac_a", 160, "operating", "body", "ev_a"));
            Assert.True(log.Post("fac_a", 160, "maintenance", "body2", "ev_b"));
            Assert.Equal(2, log.Entries.Count);
        }

        [Fact]
        public void MachineLog_ReadEntry_OneWay()
        {
            var log = new MachineLogSystem();
            log.Post("fac_a", 160, "operating", "body", "ev_a");
            Assert.Equal("ev_a", log.ReadEntry(0));
            Assert.Equal(string.Empty, log.ReadEntry(0));
            Assert.Equal(1, log.ReadCount());
            Assert.Equal(0, log.UnreadCount());
        }

        [Fact]
        public void MachineLog_ReadEntry_OutOfRange()
        {
            var log = new MachineLogSystem();
            Assert.Equal(string.Empty, log.ReadEntry(-1));
            Assert.Equal(string.Empty, log.ReadEntry(0));
            Assert.Equal(string.Empty, log.ReadEntry(999));
        }

        [Fact]
        public void MachineLog_CorruptionMarker_Deterministic()
        {
            var log1 = new MachineLogSystem();
            var log2 = new MachineLogSystem();
            var rng1 = new SeededRng(42);
            var rng2 = new SeededRng(42);
            log1.InsertCorruptionMarker(170, rng1);
            log2.InsertCorruptionMarker(170, rng2);
            Assert.Equal(log1.Entries[0].bodyShort, log2.Entries[0].bodyShort);
        }

        [Fact]
        public void MachineLog_SpinTape_OnePerDay()
        {
            var log = new MachineLogSystem();
            int spins = 0;
            log.OnTapeSpin += () => spins++;
            log.SpinTape(160);
            log.SpinTape(160);
            log.SpinTape(161);
            Assert.Equal(2, spins);
        }

        [Fact]
        public void MachineLog_CaptureRestore_Roundtrip()
        {
            var log = new MachineLogSystem();
            log.Post("fac_a", 160, "operating", "body", "ev_a");
            log.ReadEntry(0);
            log.SpinTape(160);
            var snap = log.CaptureState();

            var restored = new MachineLogSystem();
            restored.RestoreState(snap);
            Assert.Single(restored.Entries);
            Assert.True(restored.Entries[0].read);
            Assert.Equal(160, restored.State.lastTapeSpinDay);
        }

        [Fact]
        public void MachineLog_Post_RejectsEmptyFacility()
        {
            var log = new MachineLogSystem();
            Assert.False(log.Post("", 160, "operating", "body", "ev"));
            Assert.False(log.Post(null, 160, "operating", "body", "ev"));
        }

        // ── ReckoningSystem ─────────────────────────────────────────────────────

        [Fact]
        public void Reckoning_Dormant_BeforeDay160()
        {
            var r = new ReckoningSystem();
            var fired = r.Poll(159, 14, 0, 0);
            Assert.Equal(ReckoningPhase.Dormant, r.Phase);
            Assert.Empty(fired);
        }

        [Fact]
        public void Reckoning_Knowing_AtDay160()
        {
            var r = new ReckoningSystem();
            var fired = r.Poll(160, 14, 1, 0);
            Assert.Equal(ReckoningPhase.Knowing, r.Phase);
            Assert.Contains("phase_knowing", fired);
        }

        [Fact]
        public void Reckoning_Culpable_NeedsEvidence()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            var fired = r.Poll(210, 14, 2, 0);
            Assert.Equal(ReckoningPhase.Knowing, r.Phase);
            Assert.Empty(fired);

            r.EnrollEvidence(1);
            fired = r.Poll(210, 14, 2, 0);
            Assert.Equal(ReckoningPhase.Culpable, r.Phase);
            Assert.Contains("carrier_heard", fired);
        }

        [Fact]
        public void Reckoning_Counted_AtDay240()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            var fired = r.Poll(240, 14, 3, 1);
            Assert.Equal(ReckoningPhase.Counted, r.Phase);
            Assert.Contains("reckoning_call", fired);
            Assert.True(r.State.callResolved);
        }

        [Fact]
        public void Reckoning_CallIsOneShot()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            r.Poll(240, 14, 3, 1);
            var fired = r.Poll(250, 14, 3, 1);
            Assert.DoesNotContain("reckoning_call", fired);
        }

        [Fact]
        public void Reckoning_NeverReverses()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            r.Poll(240, 14, 3, 1);
            Assert.Equal(ReckoningPhase.Counted, r.Phase);
            r.Poll(300, 14, 3, 1);
            Assert.Equal(ReckoningPhase.Counted, r.Phase);
        }

        [Fact]
        public void Reckoning_SelectEnding_MutuallyExclusive()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            r.Poll(240, 14, 3, 1);

            Assert.True(r.SelectEnding("ending_verdict_the_sector_recounts", 240));
            Assert.False(r.SelectEnding("ending_verdict_the_count_is_held", 241));
            Assert.True(r.State.countPresented);
            Assert.False(r.State.countHeld);
        }

        [Fact]
        public void Reckoning_SelectEnding_RejectsBeforeCounted()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            Assert.False(r.SelectEnding("ending_verdict_the_sector_recounts", 170));
        }

        [Fact]
        public void Reckoning_SelectEnding_RejectsUnknown()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            r.Poll(240, 14, 3, 1);
            Assert.False(r.SelectEnding("ending_verdict_unknown", 240));
        }

        [Fact]
        public void Reckoning_CensusWindow_OpenInCulpable()
        {
            var r = new ReckoningSystem();
            Assert.False(r.IsCensusWindowOpen(210));
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(1);
            r.Poll(210, 14, 2, 0);
            Assert.True(r.IsCensusWindowOpen(217));
        }

        [Fact]
        public void Reckoning_CaptureRestore_Roundtrip()
        {
            var r = new ReckoningSystem();
            r.Poll(160, 14, 1, 0);
            r.EnrollEvidence(2);
            r.Poll(210, 14, 2, 2);
            var snap = r.CaptureState();

            var restored = new ReckoningSystem();
            restored.RestoreState(snap);
            Assert.Equal(ReckoningPhase.Culpable, restored.Phase);
            Assert.True(restored.State.carrierHeard);
            Assert.Equal(2, restored.State.enrolledEvidence);
        }

        // ── VerdictEndingEvaluator ──────────────────────────────────────────────

        [Fact]
        public void EndingEvaluator_ResolvedEnding_Priority()
        {
            var s = new ReckoningState { countPresented = true };
            Assert.Equal(VerdictEndingEvaluator.EndingKeyCounted, VerdictEndingEvaluator.ResolvedEnding(s));

            var s2 = new ReckoningState { countHeld = true };
            Assert.Equal(VerdictEndingEvaluator.EndingKeyHeld, VerdictEndingEvaluator.ResolvedEnding(s2));

            var s3 = new ReckoningState { offerIsLease = true };
            Assert.Equal(VerdictEndingEvaluator.EndingKeyLease, VerdictEndingEvaluator.ResolvedEnding(s3));
        }

        [Fact]
        public void EndingEvaluator_NullState_ReturnsNull()
        {
            Assert.Null(VerdictEndingEvaluator.ResolvedEnding(null));
            Assert.Null(VerdictEndingEvaluator.DecideEnding(null, 0, 240));
        }

        [Fact]
        public void EndingEvaluator_DecideEnding_FallsBackByEvidence()
        {
            var s = new ReckoningState { phase = ReckoningPhase.Counted };
            Assert.Equal(VerdictEndingEvaluator.EndingKeyCounted,
                VerdictEndingEvaluator.DecideEnding(s, 5, 240));
            Assert.Equal(VerdictEndingEvaluator.EndingKeyHeld,
                VerdictEndingEvaluator.DecideEnding(s, 2, 240));
        }

        [Fact]
        public void EndingEvaluator_DecideEnding_NullBeforeCounted()
        {
            var s = new ReckoningState { phase = ReckoningPhase.Knowing };
            Assert.Null(VerdictEndingEvaluator.DecideEnding(s, 10, 200));
        }

        [Fact]
        public void EndingEvaluator_TempestDecommissioned_OnlyOnRecount()
        {
            Assert.True(VerdictEndingEvaluator.IsTempestDecommissioned(
                new ReckoningState { countPresented = true }));
            Assert.False(VerdictEndingEvaluator.IsTempestDecommissioned(
                new ReckoningState { countHeld = true }));
            Assert.False(VerdictEndingEvaluator.IsTempestDecommissioned(
                new ReckoningState { offerIsLease = true }));
        }

        // ── VerdictReadout ──────────────────────────────────────────────────────

        [Fact]
        public void Readout_Dormant_WhenStateNull()
        {
            var line = VerdictReadout.LineFor(null, 0, 0);
            Assert.Contains("shelter instruments", line);
        }

        [Fact]
        public void Readout_Knowing_InPhase()
        {
            var s = new ReckoningState { phase = ReckoningPhase.Knowing };
            var line = VerdictReadout.LineFor(s, 1, 1);
            Assert.Contains("shelter instruments", line);
        }

        [Fact]
        public void Readout_Resolved_WhenCountPresented()
        {
            var s = new ReckoningState { countPresented = true };
            var line = VerdictReadout.LineFor(s, 5, 5);
            Assert.Contains("signature received", line.ToLower());
        }

        // ── VerdictNpcSystem ────────────────────────────────────────────────────

        [Fact]
        public void Npc_Register_Find()
        {
            var npcs = new VerdictNpcSystem();
            npcs.Register(new VerdictNpcEntry { id = "npc_a", name = "A" });
            Assert.NotNull(npcs.Find("npc_a"));
            Assert.Null(npcs.Find("npc_z"));
        }

        [Fact]
        public void Npc_Speak_OneShot()
        {
            var npcs = new VerdictNpcSystem();
            npcs.Register(new VerdictNpcEntry { id = "npc_a" });
            Assert.True(npcs.Speak("npc_a"));
            Assert.False(npcs.Speak("npc_a"));
            Assert.Single(npcs.State.spokenNpcIds);
        }

        [Fact]
        public void Npc_GetAvailable_RespectsPhase()
        {
            var npcs = new VerdictNpcSystem();
            npcs.Register(new VerdictNpcEntry { id = "npc_a", phaseMin = 2 });
            var avail1 = npcs.GetAvailable(new List<string>(), 1);
            Assert.Empty(avail1);
            var avail2 = npcs.GetAvailable(new List<string>(), 2);
            Assert.Single(avail2);
        }

        [Fact]
        public void Npc_GetAvailable_RespectsGatingFlag()
        {
            var npcs = new VerdictNpcSystem();
            npcs.Register(new VerdictNpcEntry { id = "npc_a", gatingFlag = "flag_x" });
            Assert.Empty(npcs.GetAvailable(new List<string>(), 3));
            Assert.Single(npcs.GetAvailable(new List<string> { "flag_x" }, 3));
        }

        [Fact]
        public void Npc_CaptureRestore_Roundtrip()
        {
            var npcs = new VerdictNpcSystem();
            npcs.Register(new VerdictNpcEntry { id = "npc_a" });
            npcs.Speak("npc_a");
            var snap = npcs.CaptureState();

            var restored = new VerdictNpcSystem();
            restored.RestoreState(snap);
            Assert.Contains("npc_a", restored.State.spokenNpcIds);
        }

        // ── VerdictSave (codec) ─────────────────────────────────────────────────

        [Fact]
        public void Save_CaptureEncode_DecodeRestore_Roundtrip()
        {
            var log = new MachineLogSystem();
            log.Post("fac", 160, "operating", "body", "ev");
            var reck = new ReckoningSystem();
            reck.Poll(160, 14, 1, 0);
            var evidence = new EvidenceLedger();
            evidence.Enroll("ev", 160);
            var json = new SystemTextJsonSerializer();

            var save = VerdictSaveCodec.Capture(160, log, reck, evidence, -1);
            string encoded = VerdictSaveCodec.Encode(save, json);
            Assert.True(VerdictSaveCodec.TryDecode(encoded, json, out var decoded));
            Assert.Equal(160, decoded.simDay);

            var log2 = new MachineLogSystem();
            var reck2 = new ReckoningSystem();
            var evidence2 = new EvidenceLedger();
            VerdictSaveCodec.Restore(decoded, log2, reck2, evidence2);
            Assert.Single(log2.Entries);
            Assert.Equal(ReckoningPhase.Knowing, reck2.Phase);
            Assert.Equal(1, evidence2.Count);
        }

        [Fact]
        public void Save_TamperRejection()
        {
            var log = new MachineLogSystem();
            var reck = new ReckoningSystem();
            var evidence = new EvidenceLedger();
            var json = new SystemTextJsonSerializer();

            var save = VerdictSaveCodec.Capture(160, log, reck, evidence, -1);
            string encoded = VerdictSaveCodec.Encode(save, json);
            string tampered = encoded.Replace("\"simDay\":160", "\"simDay\":999");
            Assert.False(VerdictSaveCodec.TryDecode(tampered, json, out _));
        }

        [Fact]
        public void Save_RejectsEmptyChecksum()
        {
            var json = new SystemTextJsonSerializer();
            var save = new VerdictSave { Checksum = "" };
            string encoded = json.Serialize(save);
            Assert.False(VerdictSaveCodec.TryDecode(encoded, json, out _));
        }

        [Fact]
        public void Save_RejectsNewerVersion()
        {
            var json = new SystemTextJsonSerializer();
            var save = new VerdictSave { saveVersion = 999, Checksum = "x" };
            string encoded = json.Serialize(save);
            Assert.False(VerdictSaveCodec.TryDecode(encoded, json, out _));
        }

        // ── VerdictCensusBroadcast ──────────────────────────────────────────────

        private class StubClock : ISimClock
        {
            private int _dayIndex;
            private int _hourOfDay;
            public int DayIndex { get => _dayIndex; set { _dayIndex = value; } }
            public int HourOfDay { get => _hourOfDay; set { _hourOfDay = value; } }
            public long CurrentTick => (long)_dayIndex * 1440 + (long)_hourOfDay * 60;
            public void AdvanceTicks(long ticks) { _hourOfDay += (int)(ticks / 60); if (_hourOfDay >= 24) { _dayIndex += _hourOfDay / 24; _hourOfDay %= 24; } }
            public void AdvanceHours(int hours) { AdvanceTicks(hours * 60L); }
            public void AdvanceDays(int days) { _dayIndex += days; }
        }

        private class StubCensus : IWorldCensus
        {
            public long Count { get; set; }
            public long LivingRegisteredSouls() => Count;
        }

        [Fact]
        public void Census_WindowOpen_Every7DaysAt03()
        {
            var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
            var census = new VerdictCensusBroadcast(
                clock, new SimpleEventBus(), new InMemoryFlagLedger(),
                new SeededRng(1), new StubCensus { Count = 14 });
            Assert.True(census.IsWindowOpen());

            clock.DayIndex = 211;
            Assert.False(census.IsWindowOpen());

            clock.DayIndex = 217;
            Assert.True(census.IsWindowOpen());
        }

        [Fact]
        public void Census_BroadcastOnce_PerWindow()
        {
            var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
            var bus = new SimpleEventBus();
            var flags = new InMemoryFlagLedger();
            var census = new VerdictCensusBroadcast(
                clock, bus, flags, new SeededRng(1), new StubCensus { Count = 14 });

            int headers = 0;
            bus.Subscribe("radio.census.header", _ => headers++);
            census.BroadcastIfDue();
            census.BroadcastIfDue();
            Assert.Equal(1, headers);
        }

        [Fact]
        public void Census_SilentAfterSigning()
        {
            var clock = new StubClock { DayIndex = 210, HourOfDay = 3 };
            var bus = new SimpleEventBus();
            var flags = new InMemoryFlagLedger();
            flags.Set("flag_exp08_signed_reckoning");
            var census = new VerdictCensusBroadcast(
                clock, bus, flags, new SeededRng(1), new StubCensus { Count = 14 });

            int headers = 0;
            bus.Subscribe("radio.census.header", _ => headers++);
            census.BroadcastIfDue();
            Assert.Equal(0, headers);
        }

        [Fact]
        public void Census_CanonConstants()
        {
            Assert.Equal(4.0, VerdictCensusBroadcast.CarrierSeconds);
            Assert.Equal(1.7, VerdictCensusBroadcast.HeldBreathPauseSeconds);
            Assert.Equal(211004, VerdictCensusBroadcast.ExpectedProvincialCount);
        }

        // ── VerdictCatalogLoader (locations) ────────────────────────────────────

        [Fact]
        public void CatalogLoader_Locations_ReturnsEmpty_WhenFileMissing()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = VerdictCatalogLoader.LoadLocations("/nonexistent", io, json);
            Assert.Empty(result);
        }

        [Fact]
        public void CatalogLoader_Locations_ReturnsEmpty_WhenNullArgs()
        {
            Assert.Empty(VerdictCatalogLoader.LoadLocations(null, null, null));
            Assert.Empty(VerdictCatalogLoader.LoadLocations("", new FileSystemIO(), new SystemTextJsonSerializer()));
        }

        // ── Verdict items JSON schema validation ────────────────────────────────

        [Fact]
        public void VerdictItemsJson_MatchesRuntimeSchema()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dataDir);
            if (string.IsNullOrEmpty(dataDir)) return;

            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string path = io.Combine(dataDir, "verdict_items.json");
            if (!io.FileExists(path)) return;

            string raw = io.ReadAllText(path);
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
            var items = CatalogLocator.LoadWrappedList<Ashfall.Core.Inventory.ItemDefinition>(array.GetRawText(), SystemTextJsonSerializer.Options);
            Assert.NotNull(items);
            Assert.True(items.Count >= 15, $"expected >=15 verdict items, got {items?.Count ?? 0}");

            foreach (var item in items!)
            {
                Assert.NotNull(item);
                Assert.False(string.IsNullOrEmpty(item!.id), $"item has empty id");
                Assert.False(string.IsNullOrEmpty(item.displayName), $"item {item.id} has empty displayName");
            }
        }
    }
}
