// SPDX-License-Identifier: MIT
// ASHFALL Core: Shelter room identity catalog (Plan 29 Phase 1 — Task 29A pilot).
//
// Data-driven room identity for the Holdfast's canonical rooms: display name,
// former use, current use, one-line history, inspection summary, legacy id
// aliases, and discoverable room-history vignettes. Authority:
//   • room identity data  → Assets/StreamingAssets/Data/shelter_room_identities.json
//   • runtime room ids    → existing rosters (StartingLevelSystem, ShelterAssignment,
//                           HoldfastInteriorView, power_grid.json) — NEVER renamed here
//   • vignette discovery  → JournalSystem knowledge keys (room_history_seen_*)
//                           — no second discovery/save authority in this system
//
// This catalog is a read-only projection layer. It carries NO condition state
// and must never become a competing shelter-condition meter (Plan 29 §1.2).
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>Root DTO for shelter_room_identities.json (snake_case matches the data authority).</summary>
    [Serializable]
    public sealed class ShelterRoomIdentityCatalogData
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<ShelterRoomIdentityRecord> rooms = new List<ShelterRoomIdentityRecord>();
        public List<RoomHistoryVignette> vignettes = new List<RoomHistoryVignette>();
        public List<RoomFixtureDetail> fixtures = new List<RoomFixtureDetail>();
    }

    /// <summary>
    /// Identity record for one canonical shelter room. Binds to an existing
    /// runtime room id; never duplicates functional room stats (Plan 29 §29A.2).
    /// </summary>
    [Serializable]
    public sealed class ShelterRoomIdentityRecord
    {
        /// <summary>Canonical runtime room id (e.g. "room_filtration"). Definition position.</summary>
        public string id = string.Empty;
        public string display_name = string.Empty;
        /// <summary>Pre-war / original use of the space (Plan 29 §29A.4).</summary>
        public string former_use = string.Empty;
        /// <summary>Post-crisis conversion + current use.</summary>
        public string current_use = string.Empty;
        /// <summary>One memorable fact, ~12–30 words, hover/inspection sized (§29A.5).</summary>
        public string one_line_history = string.Empty;
        /// <summary>Short inspection surface text shown before deeper lore.</summary>
        public string inspection_summary = string.Empty;
        /// <summary>Legacy runtime ids that resolve to this canonical room (§5.1 alias policy).</summary>
        public List<string> legacy_aliases = new List<string>();
        /// <summary>Fixture detail ids (Task 29A.10 pools — authored from Phase 2).</summary>
        public List<string> fixture_ids = new List<string>();
    }

    /// <summary>One discoverable room-history vignette (Plan 29 §29A.7–29A.8).</summary>
    [Serializable]
    public sealed class RoomHistoryVignette
    {
        /// <summary>Vignette id, prefix room_history_ (definition position).</summary>
        public string id = string.Empty;
        /// <summary>Canonical room id this vignette belongs to (reference).</summary>
        public string room_id = string.Empty;
        public string title = string.Empty;
        /// <summary>Era tag: original construction / pre-war / crisis conversion / early occupancy / current campaign.</summary>
        public string time_period = string.Empty;
        /// <summary>Unlock path: inspect_room | repair_performed | day_milestone.</summary>
        public string unlock = string.Empty;
        /// <summary>Required day when unlock == day_milestone; must be 0 otherwise.</summary>
        public int unlock_day;
        /// <summary>100–300 word codex body.</summary>
        public string body = string.Empty;
    }

    /// <summary>
    /// Original-fixture detail (Plan 29 §29A.10–29A.11): an authored inspection/art
    /// hook for one room. Most are ambient detail — <see cref="inspectable"/> is true
    /// only where the current UI actually offers an action (§29A.12: no fake
    /// interactivity), and fixtures are never individual scene nodes by default.
    /// </summary>
    [Serializable]
    public sealed class RoomFixtureDetail
    {
        /// <summary>Fixture id, prefix room_fixture_ (definition position).</summary>
        public string id = string.Empty;
        /// <summary>Canonical room id the fixture belongs to (reference).</summary>
        public string room_id = string.Empty;
        /// <summary>Short visible line: what a survivor notices.</summary>
        public string detail = string.Empty;
        /// <summary>Why it is there / what it records (§29A.11 historical meaning).</summary>
        public string historical_meaning = string.Empty;
        /// <summary>True only when the current UI supports inspecting it.</summary>
        public bool inspectable;
        /// <summary>True when Task 08/29A art should render it in the room view.</summary>
        public bool art_visible = true;
        /// <summary>True when renovation (Task 29C) may remove, cover or restore it.</summary>
        public bool renovation_sensitive;
        /// <summary>Optional codex entry reference (empty when the fixture has no entry).</summary>
        public string codex_entry_id = string.Empty;
    }

    /// <summary>
    /// Read-only room identity + vignette catalog with legacy-id alias
    /// resolution. Loaded once at boot; ordinal-stable ordering (authored
    /// order, no dictionary-iteration exposure). A missing data file yields an
    /// empty, valid catalog — identity is an overlay, never a domain dependency.
    /// </summary>
    public sealed class ShelterRoomIdentityCatalog
    {
        public const string FileName = "shelter_room_identities.json";

        /// <summary>Vignette unlock: the room's hotspot was inspected.</summary>
        public const string UnlockInspectRoom = "inspect_room";
        /// <summary>Vignette unlock: a real repair/maintenance action completed in the room.</summary>
        public const string UnlockRepairPerformed = "repair_performed";
        /// <summary>Vignette unlock: campaign day reached <c>unlock_day</c>.</summary>
        public const string UnlockDayMilestone = "day_milestone";

        /// <summary>Host-side trigger sources. Every value must have a real wired host
        /// trigger; authored content may not use an unlock path that nothing raises.</summary>
        public enum RoomHistoryTrigger
        {
            /// <summary>Raised when a shelter room is inspected (hotspot click).</summary>
            RoomInspected = 0,
            /// <summary>Raised when a maintenance/repair action completes in that room.</summary>
            RepairPerformed = 1,
            /// <summary>Raised once per campaign day by the day-advance owner.</summary>
            DayElapsed = 2
        }

        private readonly List<ShelterRoomIdentityRecord> _rooms = new List<ShelterRoomIdentityRecord>();
        private readonly List<RoomHistoryVignette> _vignettes = new List<RoomHistoryVignette>();
        private readonly List<RoomFixtureDetail> _fixtures = new List<RoomFixtureDetail>();
        private readonly Dictionary<string, string> _aliasToCanonical = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, ShelterRoomIdentityRecord> _canonicalToRecord = new Dictionary<string, ShelterRoomIdentityRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<RoomHistoryVignette>> _roomVignettes = new Dictionary<string, List<RoomHistoryVignette>>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<RoomFixtureDetail>> _roomFixtures = new Dictionary<string, List<RoomFixtureDetail>>(StringComparer.Ordinal);
        private readonly Dictionary<string, RoomFixtureDetail> _fixtureById = new Dictionary<string, RoomFixtureDetail>(StringComparer.Ordinal);

        public IReadOnlyList<ShelterRoomIdentityRecord> Rooms => _rooms;
        public IReadOnlyList<RoomHistoryVignette> Vignettes => _vignettes;
        public IReadOnlyList<RoomFixtureDetail> Fixtures => _fixtures;
        public int RoomCount => _rooms.Count;

        /// <summary>
        /// Load the catalog from a data directory. Missing file → empty catalog
        /// (silent by design, like optional catalog loaders); malformed file →
        /// logged warning + empty catalog. Never throws.
        /// </summary>
        public static ShelterRoomIdentityCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
        {
            var catalog = new ShelterRoomIdentityCatalog();
            if (files == null || json == null || string.IsNullOrEmpty(dataDirectory)) return catalog;
            string path = System.IO.Path.Combine(dataDirectory, FileName);
            try
            {
                if (!files.FileExists(path)) return catalog;
                string raw = files.ReadAllText(path);
                var data = json.Deserialize<ShelterRoomIdentityCatalogData>(raw);
                if (data == null) return catalog;
                catalog.Build(data);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "ShelterRoomIdentityCatalog", ex);
            }
            return catalog;
        }

        private void Build(ShelterRoomIdentityCatalogData data)
        {
            _rooms.Clear();
            _vignettes.Clear();
            _aliasToCanonical.Clear();
            _canonicalToRecord.Clear();
            _roomVignettes.Clear();
            _fixtures.Clear();
            _roomFixtures.Clear();
            _fixtureById.Clear();

            if (data.rooms != null)
            {
                foreach (var room in data.rooms)
                {
                    if (room == null || string.IsNullOrEmpty(room.id)) continue;
                    _rooms.Add(room);
                    _canonicalToRecord[room.id] = room;
                    if (room.legacy_aliases == null) continue;
                    foreach (var alias in room.legacy_aliases)
                    {
                        if (!string.IsNullOrEmpty(alias))
                            _aliasToCanonical[alias] = room.id;
                    }
                }
            }

            if (data.vignettes != null)
            {
                foreach (var vignette in data.vignettes)
                {
                    if (vignette == null || string.IsNullOrEmpty(vignette.id) || string.IsNullOrEmpty(vignette.room_id)) continue;
                    _vignettes.Add(vignette);
                    if (!_roomVignettes.TryGetValue(vignette.room_id, out var list))
                    {
                        list = new List<RoomHistoryVignette>();
                        _roomVignettes[vignette.room_id] = list;
                    }
                    list.Add(vignette);
                }
            }

            if (data.fixtures != null)
            {
                foreach (var fixture in data.fixtures)
                {
                    if (fixture == null || string.IsNullOrEmpty(fixture.id) || string.IsNullOrEmpty(fixture.room_id)) continue;
                    _fixtures.Add(fixture);
                    _fixtureById[fixture.id] = fixture;
                    if (!_roomFixtures.TryGetValue(fixture.room_id, out var list))
                    {
                        list = new List<RoomFixtureDetail>();
                        _roomFixtures[fixture.room_id] = list;
                    }
                    list.Add(fixture);
                }
            }
        }

        /// <summary>Resolve any runtime room id (legacy or canonical) to its canonical id. Unknown ids return unchanged.</summary>
        public string ResolveRoomId(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return roomId ?? string.Empty;
            return _aliasToCanonical.TryGetValue(roomId, out var canonical) ? canonical : roomId;
        }

        /// <summary>Identity record for a room (legacy or canonical id), or null when the room has no authored identity.</summary>
        public ShelterRoomIdentityRecord? GetRoomIdentity(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            return _canonicalToRecord.TryGetValue(ResolveRoomId(roomId), out var record) ? record : null;
        }

        /// <summary>Legacy aliases declared for a canonical room id (bridges canonical clicks to legacy rosters).</summary>
        public IReadOnlyList<string> GetLegacyAliases(string canonicalRoomId)
        {
            if (!string.IsNullOrEmpty(canonicalRoomId) &&
                _canonicalToRecord.TryGetValue(canonicalRoomId, out var record))
            {
                return record.legacy_aliases ?? new List<string>();
            }
            return Array.Empty<string>();
        }

        /// <summary>Vignettes bound to one canonical room id, in authored (ordinal-stable) order.</summary>
        public IReadOnlyList<RoomHistoryVignette> GetVignettesForRoom(string canonicalRoomId)
        {
            if (string.IsNullOrEmpty(canonicalRoomId)) return Array.Empty<RoomHistoryVignette>();
            return _roomVignettes.TryGetValue(canonicalRoomId, out var list) ? list : Array.Empty<RoomHistoryVignette>();
        }

        /// <summary>Vignette by id, or null.</summary>
        public RoomHistoryVignette? GetVignette(string vignetteId)
        {
            if (string.IsNullOrEmpty(vignetteId)) return null;
            for (int i = 0; i < _vignettes.Count; i++)
                if (string.Equals(_vignettes[i].id, vignetteId, StringComparison.Ordinal))
                    return _vignettes[i];
            return null;
        }

        /// <summary>Fixtures for one room (legacy or canonical id), authored order.</summary>
        public IReadOnlyList<RoomFixtureDetail> GetFixturesForRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return Array.Empty<RoomFixtureDetail>();
            return _roomFixtures.TryGetValue(ResolveRoomId(roomId), out var list) ? list : Array.Empty<RoomFixtureDetail>();
        }

        /// <summary>Fixture by id, or null.</summary>
        public RoomFixtureDetail? GetFixture(string fixtureId)
        {
            if (string.IsNullOrEmpty(fixtureId)) return null;
            return _fixtureById.TryGetValue(fixtureId, out var fixture) ? fixture : null;
        }

        /// <summary>The authored unlock value that corresponds to a host trigger.</summary>
        public static string UnlockValueFor(RoomHistoryTrigger trigger) => trigger switch
        {
            RoomHistoryTrigger.RoomInspected => UnlockInspectRoom,
            RoomHistoryTrigger.RepairPerformed => UnlockRepairPerformed,
            RoomHistoryTrigger.DayElapsed => UnlockDayMilestone,
            _ => string.Empty
        };

        /// <summary>
        /// Deterministic trigger evaluation (Plan 29 §12): given one room and one
        /// trigger, return the vignettes that become discoverable, in authored
        /// (ordinal-stable) order. <see cref="currentDay"/> is only consulted by
        /// the day-milestone trigger. Never random, never wall-clock, never
        /// dictionary-ordered; unlock itself stays idempotent in the journal.
        /// </summary>
        public IReadOnlyList<RoomHistoryVignette> GetUnlockableVignettes(
            string roomId, RoomHistoryTrigger trigger, int currentDay = 0)
        {
            var matches = GetVignettesForRoom(roomId);
            if (matches.Count == 0) return Array.Empty<RoomHistoryVignette>();
            string wanted = UnlockValueFor(trigger);
            if (string.IsNullOrEmpty(wanted)) return Array.Empty<RoomHistoryVignette>();

            List<RoomHistoryVignette>? result = null;
            for (int i = 0; i < matches.Count; i++)
            {
                var vignette = matches[i];
                if (!string.Equals(vignette.unlock, wanted, StringComparison.Ordinal)) continue;
                if (trigger == RoomHistoryTrigger.DayElapsed && currentDay < vignette.unlock_day) continue;
                (result ??= new List<RoomHistoryVignette>()).Add(vignette);
            }
            return result ?? (IReadOnlyList<RoomHistoryVignette>)Array.Empty<RoomHistoryVignette>();
        }

        /// <summary>
        /// Day-milestone vignettes that have reached their required day, across the
        /// whole catalog (authored order). Used by the daily owner so no per-room
        /// scan of narrative entries is needed at any other cadence (Plan 29 §14).
        /// </summary>
        public IReadOnlyList<RoomHistoryVignette> GetDayMilestoneVignettes(int currentDay)
        {
            List<RoomHistoryVignette>? result = null;
            for (int i = 0; i < _vignettes.Count; i++)
            {
                var vignette = _vignettes[i];
                if (!string.Equals(vignette.unlock, UnlockDayMilestone, StringComparison.Ordinal)) continue;
                if (currentDay < vignette.unlock_day) continue;
                (result ??= new List<RoomHistoryVignette>()).Add(vignette);
            }
            return result ?? (IReadOnlyList<RoomHistoryVignette>)Array.Empty<RoomHistoryVignette>();
        }

        /// <summary>
        /// Contract validation (Plan 29 §29A.20). Returns a list of human-readable
        /// errors; empty list = valid. Checks: unique room ids, unique vignette
        /// ids, alias collisions, vignette room references, unlock vocabulary,
        /// one-line-history length band, vignette body word-count band.
        /// </summary>
        public List<string> Validate()
        {
            var errors = new List<string>();
            var seenRooms = new HashSet<string>(StringComparer.Ordinal);
            var seenVignettes = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < _rooms.Count; i++)
            {
                var room = _rooms[i];
                if (!seenRooms.Add(room.id))
                    errors.Add($"duplicate room id '{room.id}'");
                if (string.IsNullOrWhiteSpace(room.display_name))
                    errors.Add($"room '{room.id}' missing display_name");
                if (string.IsNullOrWhiteSpace(room.former_use))
                    errors.Add($"room '{room.id}' missing former_use");
                if (string.IsNullOrWhiteSpace(room.current_use))
                    errors.Add($"room '{room.id}' missing current_use");
                int historyWords = CountWords(room.one_line_history);
                if (historyWords < 10 || historyWords > 36)
                    errors.Add($"room '{room.id}' one_line_history outside 10–36 word band ({historyWords} words)");
                if (room.fixture_ids != null && room.fixture_ids.Count > 6)
                    errors.Add($"room '{room.id}' lists {room.fixture_ids.Count} fixtures (max 6 — Plan 29 §29A.10)");
                if (room.legacy_aliases != null)
                {
                    foreach (var alias in room.legacy_aliases)
                    {
                        if (string.IsNullOrEmpty(alias)) continue;
                        if (_canonicalToRecord.ContainsKey(alias) && !string.Equals(alias, room.id, StringComparison.Ordinal))
                            errors.Add($"alias '{alias}' of room '{room.id}' collides with another canonical room id");
                    }
                }
            }

            for (int i = 0; i < _vignettes.Count; i++)
            {
                var vignette = _vignettes[i];
                if (!seenVignettes.Add(vignette.id))
                    errors.Add($"duplicate vignette id '{vignette.id}'");
                if (!_canonicalToRecord.ContainsKey(vignette.room_id))
                    errors.Add($"vignette '{vignette.id}' references unknown room '{vignette.room_id}'");
                if (!IsSupportedUnlock(vignette.unlock))
                    errors.Add($"vignette '{vignette.id}' has unsupported unlock '{vignette.unlock}'");
                else if (string.Equals(vignette.unlock, UnlockDayMilestone, StringComparison.Ordinal))
                {
                    if (vignette.unlock_day < 1)
                        errors.Add($"vignette '{vignette.id}' day_milestone unlock requires unlock_day >= 1");
                }
                else if (vignette.unlock_day != 0)
                {
                    errors.Add($"vignette '{vignette.id}' sets unlock_day without day_milestone");
                }
                int bodyWords = CountWords(vignette.body);
                if (bodyWords < 100 || bodyWords > 300)
                    errors.Add($"vignette '{vignette.id}' body outside 100–300 word band ({bodyWords} words)");
                if (string.IsNullOrWhiteSpace(vignette.title))
                    errors.Add($"vignette '{vignette.id}' missing title");
            }

            // ── Fixtures (§29A.11): unique ids, resolvable rooms, bidirectional
            // consistency with each room's fixture_ids pool, no orphans.
            var seenFixtures = new HashSet<string>(StringComparer.Ordinal);
            var referencedFixtures = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < _rooms.Count; i++)
            {
                var room = _rooms[i];
                if (room.fixture_ids == null) continue;
                foreach (var fixtureId in room.fixture_ids)
                {
                    if (string.IsNullOrEmpty(fixtureId)) continue;
                    referencedFixtures.Add(fixtureId);
                    var fixture = GetFixture(fixtureId);
                    if (fixture == null)
                        errors.Add($"room '{room.id}' references unknown fixture '{fixtureId}'");
                    else if (!string.Equals(fixture.room_id, room.id, StringComparison.Ordinal))
                        errors.Add($"fixture '{fixtureId}' is listed by room '{room.id}' but owned by '{fixture.room_id}'");
                }
            }
            for (int i = 0; i < _fixtures.Count; i++)
            {
                var fixture = _fixtures[i];
                if (!seenFixtures.Add(fixture.id))
                    errors.Add($"duplicate fixture id '{fixture.id}'");
                if (!_canonicalToRecord.ContainsKey(fixture.room_id))
                    errors.Add($"fixture '{fixture.id}' references unknown room '{fixture.room_id}'");
                if (string.IsNullOrWhiteSpace(fixture.detail))
                    errors.Add($"fixture '{fixture.id}' missing detail");
                if (string.IsNullOrWhiteSpace(fixture.historical_meaning))
                    errors.Add($"fixture '{fixture.id}' missing historical_meaning");
                if (!referencedFixtures.Contains(fixture.id))
                    errors.Add($"fixture '{fixture.id}' is not listed by any room (orphan)");
            }

            return errors;
        }

        /// <summary>Unlock vocabulary. A value is only allowed here once a real host trigger raises it.</summary>
        private static bool IsSupportedUnlock(string unlock) =>
            string.Equals(unlock, UnlockInspectRoom, StringComparison.Ordinal)
            || string.Equals(unlock, UnlockRepairPerformed, StringComparison.Ordinal)
            || string.Equals(unlock, UnlockDayMilestone, StringComparison.Ordinal);

        private static int CountWords(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            int words = 0;
            bool inWord = false;
            for (int i = 0; i < text.Length; i++)
            {
                bool isSep = char.IsWhiteSpace(text[i]);
                if (!isSep && !inWord) words++;
                inWord = !isSep;
            }
            return words;
        }
    }
}
