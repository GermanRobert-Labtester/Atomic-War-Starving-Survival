// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 73 — Faction Radio Corpus Expansion (30 authored faction broadcasts).
    /// Pins corpus shape, referential integrity, tuner/frequency bounds, schedule
    /// integration through the canonical RadioScheduleCoordinator, cross-system
    /// references (patrol/territory, distress, telemetry, questlines), engine
    /// dual-path chatter bridging, and deterministic selection.
    /// </summary>
    public class FactionRadioBroadcastExpansionTests
    {
        private static string FindDataDir()
        {
            // Matches FactionRadioCorpusTests resolution: locate the repo-root data tree.
            string relative = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data");
            if (Directory.Exists(relative)) return Path.GetFullPath(relative);
            string direct = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(direct)) return direct;
            string fallback = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data"));
            Assert.True(Directory.Exists(fallback), $"Data directory not found: {fallback}");
            return fallback;
        }

        private static JsonElement LoadJson(string dataDir, string fileName)
        {
            string path = Path.Combine(dataDir, fileName);
            Assert.True(File.Exists(path), $"Required data file missing: {fileName}");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            return doc.RootElement.Clone();
        }

        private static List<JsonElement> GetArray(JsonElement root, string key)
        {
            var list = new List<JsonElement>();
            if (root.TryGetProperty(key, out var prop) && prop.ValueKind == JsonValueKind.Array)
            {
                foreach (var e in prop.EnumerateArray()) list.Add(e.Clone());
            }
            return list;
        }

        private static string GetString(JsonElement e, string key, string fallback = "")
        {
            return e.TryGetProperty(key, out var p) && p.ValueKind == JsonValueKind.String
                ? p.GetString() ?? fallback
                : fallback;
        }

        private static float GetFloat(JsonElement e, string key)
        {
            return e.TryGetProperty(key, out var p) ? (float)p.GetDouble() : 0f;
        }

        private static int GetInt(JsonElement e, string key)
        {
            return e.TryGetProperty(key, out var p) ? p.GetInt32() : 0;
        }

        private static List<string> GetStringList(JsonElement e, string key)
        {
            var list = new List<string>();
            if (e.TryGetProperty(key, out var p) && p.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in p.EnumerateArray())
                {
                    string v = item.GetString() ?? "";
                    if (!string.IsNullOrEmpty(v)) list.Add(v);
                }
            }
            return list;
        }

        private static List<JsonElement> LoadBroadcasts(out JsonElement corpus, out HashSet<string> factionIds)
        {
            string dataDir = FindDataDir();
            corpus = LoadJson(dataDir, "faction_radio_corpus.json");
            factionIds = new HashSet<string>(StringComparer.Ordinal);
            if (corpus.TryGetProperty("factions", out var fac) && fac.ValueKind == JsonValueKind.Object)
            {
                foreach (var f in fac.EnumerateObject()) factionIds.Add(f.Name);
            }
            return GetArray(corpus, "broadcasts");
        }

        // ── Catalog shape ───────────────────────────────────────────────────

        [Fact]
        public void Corpus_HasExactlyThirtyBroadcasts_AndSilenceEventsRetained()
        {
            // Plan 73 roster = 30 canonical broadcasts. The later patrol-radio
            // hooks integration (Plans 60-63 closure) added 5 additional
            // `radio_patrol_*` engine-hook entries to the same corpus.
            var broadcasts = LoadBroadcasts(out var corpus, out _);
            var canonical = broadcasts.Where(b => !GetString(b, "id").StartsWith("radio_patrol_", StringComparison.Ordinal)).ToList();
            var hooks = broadcasts.Where(b => GetString(b, "id").StartsWith("radio_patrol_", StringComparison.Ordinal)).ToList();
            Assert.Equal(30, canonical.Count);
            Assert.Equal(5, hooks.Count);
            Assert.All(hooks, h => Assert.Equal("patrol_report", GetString(h, "type")));
            Assert.True(GetArray(corpus, "silence_events").Count >= 12, "Silence events must be retained.");
        }

        [Fact]
        public void Corpus_TenRequiredTypes_ThreeEach()
        {
            var broadcasts = LoadBroadcasts(out _, out _);
            string[] required =
            {
                "patrol_report", "supply_request", "propaganda", "distress_call",
                "encrypted_traffic", "military_traffic", "civilian_intercept",
                "dead_hand_ping", "weather_report", "supply_inventory"
            };
            foreach (var type in required)
            {
                int canonical = 0;
                int total = 0;
                foreach (var b in broadcasts)
                {
                    if (!string.Equals(GetString(b, "type"), type, StringComparison.Ordinal)) continue;
                    total++;
                    if (!GetString(b, "id").StartsWith("radio_patrol_", StringComparison.Ordinal)) canonical++;
                }
                Assert.True(canonical == 3, $"Expected exactly 3 canonical broadcasts of type '{type}', found {canonical}.");
                if (type == "patrol_report")
                {
                    // 3 canonical + 5 radio_patrol_* engine hooks from the
                    // patrol-radio integration.
                    Assert.True(total == 8, $"Expected 8 patrol_report broadcasts total, found {total}.");
                }
                else
                {
                    Assert.True(total == 3, $"Expected exactly 3 broadcasts of type '{type}', found {total}.");
                }
            }
        }

        [Fact]
        public void Corpus_IdsUnique_AndNoCollisionWithRadioJson()
        {
            string dataDir = FindDataDir();
            var broadcasts = LoadBroadcasts(out _, out _);

            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var b in broadcasts)
            {
                string id = GetString(b, "id");
                Assert.False(string.IsNullOrEmpty(id), "Broadcast id must not be empty.");
                Assert.True(ids.Add(id), $"Duplicate broadcast id in corpus: {id}");
            }

            var radio = LoadJson(dataDir, "radio.json");
            foreach (var rb in GetArray(radio, "radio_broadcasts"))
            {
                string rid = GetString(rb, "id");
                Assert.False(ids.Contains(rid), $"Broadcast id collides with radio.json: {rid}");
            }
        }

        [Fact]
        public void Corpus_FactionReferencesResolve_ToCorpusFactions()
        {
            var broadcasts = LoadBroadcasts(out _, out var factionIds);
            foreach (var b in broadcasts)
            {
                string faction = GetString(b, "faction_id");
                string type = GetString(b, "type");
                string id = GetString(b, "id");
                if (type == "dead_hand_ping")
                {
                    // Automated infrastructure pings carry no living faction.
                    Assert.True(string.IsNullOrEmpty(faction), $"Dead-hand ping {id} must not claim a living faction.");
                }
                else
                {
                    Assert.True(factionIds.Contains(faction),
                        $"Broadcast {id} references unknown faction '{faction}'.");
                }
            }
        }

        [Fact]
        public void Corpus_AllFactionsAppearAtLeastOnce()
        {
            var broadcasts = LoadBroadcasts(out _, out var factionIds);
            var used = new HashSet<string>(StringComparer.Ordinal);
            foreach (var b in broadcasts)
            {
                string f = GetString(b, "faction_id");
                if (!string.IsNullOrEmpty(f)) used.Add(f);
            }
            Assert.Equal(factionIds.Count, used.Count);
            Assert.Subset(used, factionIds);
        }

        // ── Tuner / frequency / signal bounds ───────────────────────────────

        [Fact]
        public void Corpus_FrequenciesWithinHostTuningBand_AndSignalValid()
        {
            var broadcasts = LoadBroadcasts(out _, out _);
            foreach (var b in broadcasts)
            {
                string id = GetString(b, "id");
                float freq = GetFloat(b, "frequency_mhz");
                Assert.True(freq >= 88.0f && freq <= 150.0f,
                    $"Broadcast {id} frequency {freq} outside host tuning band 88.0–150.0 MHz.");
                int sig = GetInt(b, "signal_strength");
                Assert.InRange(sig, 1, 9);
                int dayMin = GetInt(b, "day_min");
                int dayMax = b.TryGetProperty("day_max", out _) ? GetInt(b, "day_max") : 9999;
                Assert.True(dayMin >= 1, $"Broadcast {id} day_min must be >= 1.");
                Assert.True(dayMax >= dayMin, $"Broadcast {id} day window inverted.");
            }
        }

        [Fact]
        public void Corpus_MessagesConcise_AndToneCompliant()
        {
            var broadcasts = LoadBroadcasts(out _, out _);
            string[] forbidden = { " lol ", " gg ", " bruh ", " meta ", " player ", " respawn ", " nerf ", " buff ", " xp " };
            foreach (var b in broadcasts)
            {
                string msg = GetString(b, "message");
                string id = GetString(b, "id");
                Assert.InRange(msg.Length, 20, 240);
                string lower = " " + msg.ToLowerInvariant() + " ";
                foreach (var word in forbidden)
                {
                    Assert.True(!lower.Contains(word), $"Broadcast {id} fails tone lint on '{word.Trim()}'.");
                }
            }
        }

        // ── Referential integrity: intel / distress / telemetry / quests ────

        [Fact]
        public void Corpus_IntelRefs_ResolveAgainstLocationRegistry()
        {
            string dataDir = FindDataDir();
            var broadcasts = LoadBroadcasts(out _, out _);
            var locations = LoadJson(dataDir, "locations.json");
            var locationIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var loc in GetArray(locations, "locations"))
            {
                string lid = GetString(loc, "id");
                if (!string.IsNullOrEmpty(lid)) locationIds.Add(lid);
            }

            foreach (var b in broadcasts)
            {
                foreach (var intelRef in GetStringList(b, "intel_refs"))
                {
                    Assert.True(locationIds.Contains(intelRef),
                        $"Broadcast {GetString(b, "id")} intel ref '{intelRef}' does not resolve to a location.");
                }
            }
        }

        [Fact]
        public void Corpus_DistressRefs_ResolveAgainstDistressAuthority()
        {
            string dataDir = FindDataDir();
            var broadcasts = LoadBroadcasts(out _, out _);
            var validDistressIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (string file in new[] { "radio_distress_signals.json", "radio_distress_signals_expansion.json" })
            {
                var root = LoadJson(dataDir, file);
                foreach (var d in GetArray(root, "radio_broadcasts"))
                {
                    string fid = GetString(d, "frequency_id");
                    if (!string.IsNullOrEmpty(fid)) validDistressIds.Add(fid);
                }
            }
            // Built-in canonical signals registered by RadioDistressSystem.
            foreach (string builtin in new[]
            {
                "freq_distress_217_4", "freq_distress_148_2", "freq_distress_108_9",
                "freq_distress_124_7", "freq_distress_134_5", "freq_distress_162_1",
                "freq_distress_162_8", "freq_distress_77_3"
            })
            {
                validDistressIds.Add(builtin);
            }

            int linked = 0;
            foreach (var b in broadcasts)
            {
                string distressId = GetString(b, "distress_id");
                if (string.IsNullOrEmpty(distressId)) continue;
                linked++;
                Assert.True(validDistressIds.Contains(distressId),
                    $"Broadcast {GetString(b, "id")} references unknown distress signal '{distressId}'.");
            }
            Assert.True(linked >= 3, $"Expected >= 3 distress-linked broadcasts, found {linked}.");
        }

        [Fact]
        public void Corpus_TelemetryRefs_ResolveAgainstOrbitalHarrowAuthority()
        {
            string dataDir = FindDataDir();
            var broadcasts = LoadBroadcasts(out _, out _);
            var events = LoadJson(dataDir, "orbital_harrow_events.json");
            var eventIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in GetArray(events, "events"))
            {
                string eid = GetString(e, "id");
                if (!string.IsNullOrEmpty(eid)) eventIds.Add(eid);
            }

            int linked = 0;
            foreach (var b in broadcasts)
            {
                string telemetryId = GetString(b, "telemetry_event_id");
                if (string.IsNullOrEmpty(telemetryId)) continue;
                linked++;
                Assert.True(eventIds.Contains(telemetryId),
                    $"Broadcast {GetString(b, "id")} references unknown telemetry event '{telemetryId}'.");
            }
            Assert.True(linked >= 3, $"Expected >= 3 telemetry-linked broadcasts, found {linked}.");
        }

        [Fact]
        public void Corpus_QuestHooks_ResolveAgainstRuntimeQuestRegistry()
        {
            // The faction corpus carries quest_hook intel tags consumed by the
            // broadcast catalog as `quest:<id>` metadata. The radio rumor
            // quest-hook IDs are reserved runtime vocabulary registered in
            // CatalogIntegrityValidator.KnownRuntimeIds ("Radio rumor /
            // broadcast quest hooks"). dynamic_questlines.json does not yet
            // expose a radio-trigger grammar, so quest START remains owned by
            // the questline system — the hook is an intel breadcrumb, not a
            // mechanical trigger (Plan 73 §43 Intel semantics).
            var broadcasts = LoadBroadcasts(out _, out _);
            int linked = 0;
            foreach (var b in broadcasts)
            {
                string questHook = GetString(b, "quest_hook");
                if (string.IsNullOrEmpty(questHook)) continue;
                linked++;
                Assert.True(Array.IndexOf(CatalogIntegrityValidator.KnownRuntimeIds, questHook) >= 0,
                    $"Broadcast {GetString(b, "id")} references unregistered quest hook '{questHook}'.");
            }
            Assert.True(linked >= 3, $"Expected >= 3 quest-hook broadcasts, found {linked}.");
        }

        [Fact]
        public void Corpus_PatrolTerritoryLinks_AtLeastFive()
        {
            var broadcasts = LoadBroadcasts(out _, out _);
            string[] territoryIds =
            {
                "radio_faction_patrol_north_culvert",
                "radio_faction_patrol_missing_siding",
                "radio_faction_patrol_customs_road",
                "radio_faction_military_checkpoint_reinforce",
                "radio_faction_military_withdrawal_order"
            };
            var byId = new Dictionary<string, JsonElement>();
            foreach (var b in broadcasts) byId[GetString(b, "id")] = b;

            foreach (var tid in territoryIds)
            {
                Assert.True(byId.ContainsKey(tid), $"Required patrol/territory broadcast missing: {tid}");
                var b = byId[tid];
                Assert.True(GetStringList(b, "intel_tags").Count > 0 || GetStringList(b, "intel_refs").Count > 0,
                    $"Patrol/territory broadcast {tid} carries no intel payload.");
            }
        }

        // ── Catalog loader (R1 mechanical bridge) ───────────────────────────

        private static RadioBroadcastCatalog LoadCatalog(string dataDir)
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.LoadFromDataDirectory(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            return catalog;
        }

        [Fact]
        public void Catalog_FactionCorpusBroadcastsRegister_ThroughUnifiedLoader()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);

            var expected = new[]
            {
                "radio_faction_patrol_north_culvert",
                "radio_faction_propaganda_work_order",
                "radio_faction_military_withdrawal_order",
                "radio_faction_dead_hand_readiness_check",
                "radio_faction_inventory_tools_welding"
            };
            foreach (var id in expected)
            {
                var b = catalog.GetById(id);
                Assert.True(b != null, $"Expected faction broadcast missing from catalog: {id}");
            }

            var distress = catalog.GetById("radio_faction_distress_patrol_ambush");
            Assert.Equal(BroadcastGenre.DistressSignal, distress!.Genre);
            Assert.Equal(BroadcastPriority.Urgent, distress.Priority);
            Assert.True(distress.Tags.Contains("distress:freq_distress_55_6"));

            var deadHand = catalog.GetById("radio_faction_dead_hand_readiness_check");
            Assert.Equal(BroadcastGenre.AutomatedLoop, deadHand!.Genre);
            Assert.Equal(SourceReliability.Automated, deadHand.Reliability);
            Assert.True(deadHand.Tags.Contains("telemetry:event_orbital_dead_hand_repeating_ping"));

            var encrypted = catalog.GetById("radio_faction_encrypted_short_burst");
            Assert.Equal(BroadcastGenre.NumbersStation, encrypted!.Genre);
            Assert.Equal(118.5f, encrypted.FrequencyMhz);
        }

        [Fact]
        public void Catalog_AllThirtyBroadcastsRegistered_WithDistinctIds()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);
            int factionBroadcasts = 0;
            foreach (var b in catalog.AllBroadcasts)
            {
                if (b.BroadcastId.StartsWith("radio_faction_", StringComparison.Ordinal)) factionBroadcasts++;
            }
            Assert.Equal(30, factionBroadcasts);
        }

        [Fact]
        public void Catalog_DayGates_ExcludeBroadcastsBeforeMinDay()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);

            // Withdrawal order has day_min 30 — must not be eligible on day 10.
            var day10 = catalog.GetEligibleBroadcasts(88.4f, 10);
            Assert.DoesNotContain(day10, b => b.BroadcastId == "radio_faction_military_withdrawal_order");

            // Dead-hand readiness has day_min 30 — must not be eligible on day 20.
            var day20 = catalog.GetEligibleBroadcasts(142.85f, 20);
            Assert.DoesNotContain(day20, b => b.BroadcastId == "radio_faction_dead_hand_readiness_check");
        }

        // ── Schedule integration (8 required broadcasts) ────────────────────

        [Fact]
        public void Schedule_EightRequiredBroadcasts_EligibleThroughCanonicalSchedule()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);

            var scheduled = new (string id, float freq, int day)[]
            {
                ("radio_faction_patrol_north_culvert", 88.4f, 10),
                ("radio_faction_supply_request_clinic", 91.3f, 8),
                ("radio_faction_propaganda_work_order", 88.4f, 10),
                ("radio_faction_distress_patrol_ambush", 104.2f, 16),
                ("radio_faction_encrypted_repeating_groups", 104.7f, 20),
                ("radio_faction_military_convoy_corridor", 88.4f, 14),
                ("radio_faction_dead_hand_readiness_check", 142.85f, 32),
                ("radio_faction_weather_ash_front", 104.2f, 12)
            };

            foreach (var (id, freq, day) in scheduled)
            {
                var eligible = catalog.GetEligibleBroadcasts(freq, day);
                Assert.Contains(eligible, b => b.BroadcastId == id);
            }
        }

        [Fact]
        public void Schedule_ResolveSurfacesDistressBroadcast_AtPriority()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);
            var stations = new RadioStationCatalog();
            stations.LoadFromDataDirectory(dataDir);
            var coordinator = new RadioScheduleCoordinator(catalog, stations);

            // Day 18 on 104.7: the distress broadcast is the only Urgent entry in
            // window — the priority sort must surface it deterministically.
            var result = coordinator.Resolve(104.7f, 18, new SeededRng(2026));
            Assert.True(result.HasTransmission);
            Assert.Equal("radio_faction_distress_patrol_ambush", result.BroadcastId);
            Assert.Equal(BroadcastGenre.DistressSignal, result.Genre);
        }

        [Fact]
        public void Schedule_ResolveSurfacesDeadHandPing_OnAutomatedRelayBand()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);
            var stations = new RadioStationCatalog();
            stations.LoadFromDataDirectory(dataDir);
            var coordinator = new RadioScheduleCoordinator(catalog, stations);

            // Day 32 on 142.85: the later radio-authority stream added the
            // automated emergency teletype (radio.json, day 20-300) to this
            // band at Urgent priority — it outranks the Important dead-hand
            // pings while its window is open. The surfaced transmission must
            // still be a faction-corpus Plan 73 broadcast OR the canonical
            // relay teletype.
            var result = coordinator.Resolve(142.85f, 32, new SeededRng(7));
            Assert.True(result.HasTransmission);
            Assert.True(
                result.BroadcastId == "radio_faction_dead_hand_readiness_check" ||
                result.BroadcastId == "radio_faction_distress_clinic_evacuation" ||
                result.BroadcastId == "radio_broadcast_relay_teletype_ash_front",
                $"Unexpected broadcast on the automated relay band: {result.BroadcastId}");

            // Day 320: the teletype window (20-300) has closed. Only the
            // dead-hand pings remain eligible — one of the three must surface.
            // Day 320: the teletype (20-300), battery countdown (80-365) and
            // continuity roll (200-365) from the later radio-authority stream
            // share the band, and Year-of-Ash automated traffic (dayTrigger
            // 180+, never closing) is permanently eligible. Dead-hand pings
            // therefore cannot deterministically WIN resolution on this band
            // — but they must remain ELIGIBLE through the canonical catalog,
            // and resolution must stay deterministic.
            var eligible = catalog.GetEligibleBroadcasts(142.85f, 320);
            Assert.Contains(eligible, b => b.BroadcastId == "radio_faction_dead_hand_readiness_check");
            Assert.Contains(eligible, b => b.BroadcastId == "radio_faction_dead_hand_orbital_track");
            Assert.Contains(eligible, b => b.BroadcastId == "radio_faction_dead_hand_command_link");
            var late = coordinator.Resolve(142.85f, 320, new SeededRng(7));
            Assert.True(late.HasTransmission);
            Assert.Contains(eligible, b => b.BroadcastId == late.BroadcastId);
            var lateAgain = coordinator.Resolve(142.85f, 320, new SeededRng(7));
            Assert.Equal(late.BroadcastId, lateAgain.BroadcastId);

            // The FactionRadioEngine dual path keeps the relay band reachable
            // as faction chatter regardless of unified-schedule priority.
            var engine = LoadEngine(dataDir);
            var chatter = engine.GetBroadcastAtFrequency(142.85f, 320, new SeededRng(11));
            Assert.True(chatter.Kind == RadioEventKind.InterceptChatter || chatter.Kind == RadioEventKind.Silence);
        }

        [Fact]
        public void Schedule_Resolve_IsDeterministicForSameState()
        {
            string dataDir = FindDataDir();
            var catalog = LoadCatalog(dataDir);
            var stations = new RadioStationCatalog();
            stations.LoadFromDataDirectory(dataDir);
            var coordinator = new RadioScheduleCoordinator(catalog, stations);

            for (int day = 40; day <= 44; day++)
            {
                var r1 = coordinator.Resolve(88.4f, day, new SeededRng(999));
                var r2 = coordinator.Resolve(88.4f, day, new SeededRng(999));
                Assert.Equal(r1.BroadcastId, r2.BroadcastId);
                Assert.Equal(r1.Message, r2.Message);
            }
        }

        // ── Faction engine dual-path bridge ─────────────────────────────────

        private static FactionRadioEngine LoadEngine(string dataDir)
        {
            string json = File.ReadAllText(Path.Combine(dataDir, "faction_radio_corpus.json"));
            return FactionRadioEngine.LoadFromJson(json);
        }

        [Fact]
        public void Engine_NearChannelBroadcasts_EnterFactionChatterPools()
        {
            string dataDir = FindDataDir();
            var engine = LoadEngine(dataDir);
            var rng = new SeededRng(42);

            // patrol_north_culvert sits on the military_remnants channel (88.4).
            var patrol = engine.GetBroadcastAtFrequency(88.4f, 1, rng);
            Assert.Equal("military_remnants", patrol.FactionId);

            // Every appended broadcast message must be selectable from its faction pool.
            string patrolMsg = "Patrol Two, check-in from the north service road. Culvert is clear. Bridge approach is blocked again. We are cutting east through the pump station.";
            Assert.True(EnginePoolContains(engine, "military_remnants", patrolMsg),
                "Patrol broadcast message missing from military_remnants chatter pool.");

            string burstMsg = "Seven-nine-two. Birch. Seven-nine-two. Northglass. Nothing further. End of burst.";
            Assert.True(EnginePoolContains(engine, "wire_heads", burstMsg),
                "Encrypted burst message missing from wire_heads chatter pool.");
        }

        [Fact]
        public void Engine_OffChannelBroadcasts_DoNotEnterChatterPools()
        {
            string dataDir = FindDataDir();
            var engine = LoadEngine(dataDir);

            // supply_request_clinic transmits on 91.3 — far from the
            // safe_haven_community channel (112.3): must not join that pool.
            string clinicMsg = "Clinic annex requests antibiotics, dressings, clean saline. We can cover lamp fuel or batteries in trade. Do not send food. We have food.";
            Assert.False(EnginePoolContains(engine, "safe_haven_community", clinicMsg));
        }

        private static bool EnginePoolContains(FactionRadioEngine engine, string factionId, string message)
        {
            // Drain the pool across many draws to prove membership without
            // depending on internal collections.
            var rng = new SeededRng(1234);
            for (int day = 1; day <= 400; day++)
            {
                var intercept = engine.GetFactionEvent(factionId, RadioEventKind.InterceptChatter, day, rng);
                if (string.Equals(intercept.Message, message, StringComparison.Ordinal)) return true;
            }
            return false;
        }

        [Fact]
        public void Engine_DeterministicSelection_UnderSameSeed()
        {
            string dataDir = FindDataDir();
            var e1 = LoadEngine(dataDir);
            var e2 = LoadEngine(dataDir);

            for (int day = 1; day <= 60; day += 7)
            {
                var i1 = e1.GetBroadcastAtFrequency(88.4f, day, new SeededRng(555));
                var i2 = e2.GetBroadcastAtFrequency(88.4f, day, new SeededRng(555));
                Assert.Equal(i1.Message, i2.Message);
                Assert.Equal(i1.SignalStrength, i2.SignalStrength);
                Assert.Equal(i1.FactionId, i2.FactionId);
            }
        }
    }
}
