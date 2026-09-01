// SPDX-License-Identifier: MIT
// Plan 29 Task 29A — Phase 1 pilot contract tests.
// Pins: room identity catalog loads from the data authority, alias resolution,
// vignette contract bounds, journal unlock persistence + old-save defaults,
// and the StartingLevel location-id migration (loc_bunker_holdfast → loc_holdfast).
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core.Tests
{
    public class ShelterRoomIdentityTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _files = new FileSystemIO();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();

        public ShelterRoomIdentityTests()
        {
            string baseDir = AppContext.BaseDirectory;
            _dataDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
        }

        public void Dispose() { }

        private ShelterRoomIdentityCatalog LoadCatalog() =>
            ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);

        private static string? FindRepoRoot()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }
            return Directory.Exists(Path.Combine(root ?? "", "src")) ? root : null;
        }

        // ── Catalog load & contract validation ────────────────────────────

        [Fact]
        public void Catalog_Loads_FromDataAuthority()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.RoomCount >= 2, "pilot expects at least the two pilot rooms");
            Assert.True(catalog.Vignettes.Count >= 1, "pilot expects at least one vignette");
        }

        [Fact]
        public void Catalog_CoversEveryMajorPlayerFacingRoom()
        {
            // Plan 29 §28: 100% of major player-facing rooms carry identity records.
            var catalog = LoadCatalog();
            var expected = new[]
            {
                "room_bunker_corridor", "room_storage_bay", "room_bunks", "room_kitchen",
                "room_clinic", "room_workshop", "room_filtration", "room_airlock",
                "room_radio_tuner", "room_foundry", "room_greenhouse", "room_main",
                "room_water_pump"
            };
            foreach (var roomId in expected)
                Assert.NotNull(catalog.GetRoomIdentity(roomId));
            Assert.Equal(expected.Length, catalog.RoomCount);
        }

        [Fact]
        public void Catalog_HasTwentyVignettes_WithVariedUnlockPaths()
        {
            var catalog = LoadCatalog();
            Assert.Equal(21, catalog.Vignettes.Count);
            var unlocks = catalog.Vignettes
                .Select(v => v.unlock)
                .Distinct(StringComparer.Ordinal)
                .ToList();
            // §29A.9: not every history unlocks the same way — at least one
            // non-inspection path must exist alongside inspection.
            Assert.Contains(ShelterRoomIdentityCatalog.UnlockInspectRoom, unlocks);
            Assert.True(unlocks.Count >= 2, $"expected varied unlock paths, got: {string.Join(",", unlocks)}");
            Assert.All(catalog.Vignettes, v => Assert.False(string.IsNullOrEmpty(v.unlock)));
        }

        // ── Fixtures (Plan 29 §29A.10-29A.11) ───────────────────────────

        [Fact]
        public void Fixtures_AreAuthoredForMajorRooms_AndResolveToTheirRoom()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Fixtures.Count >= 20, "expected authored fixture pools for the major rooms");
            var roomsWithFixtures = 0;
            foreach (var room in catalog.Rooms)
            {
                if (catalog.GetFixturesForRoom(room.id).Count > 0) roomsWithFixtures++;
                Assert.True(room.fixture_ids.Count <= 6, $"{room.id} exceeds the 6-fixture cap");
            }
            Assert.True(roomsWithFixtures >= catalog.RoomCount - 1,
                "nearly every major room should carry a fixture pool");
        }

        [Fact]
        public void Fixtures_LegacyRoomId_LooksUpTheCanonicalPool()
        {
            var catalog = LoadCatalog();
            var viaLegacy = catalog.GetFixturesForRoom("room_filtration_stack");
            var viaCanonical = catalog.GetFixturesForRoom("room_filtration");
            Assert.NotEmpty(viaCanonical);
            Assert.Equal(viaCanonical.Count, viaLegacy.Count);
        }

        [Fact]
        public void Fixtures_HistoricalMeaning_IsAlwaysAuthored()
        {
            var catalog = LoadCatalog();
            foreach (var fixture in catalog.Fixtures)
            {
                Assert.False(string.IsNullOrWhiteSpace(fixture.detail));
                Assert.False(string.IsNullOrWhiteSpace(fixture.historical_meaning));
                // §29A.12: no fake interactivity — an inspectable flag is only set
                // once the UI actually offers a per-fixture action.
                Assert.False(fixture.inspectable);
            }
        }

        // ── Trigger evaluation (Plan 29 §12 determinism) ──────────────────

        [Fact]
        public void Triggers_InspectionDoesNotFireRepairOrDayVignettes()
        {
            var catalog = LoadCatalog();
            var inspected = catalog.GetUnlockableVignettes("room_filtration",
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected);
            Assert.All(inspected, v =>
                Assert.Equal(ShelterRoomIdentityCatalog.UnlockInspectRoom, v.unlock));

            var repaired = catalog.GetUnlockableVignettes("room_filtration",
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed);
            Assert.All(repaired, v =>
                Assert.Equal(ShelterRoomIdentityCatalog.UnlockRepairPerformed, v.unlock));
            Assert.NotEmpty(repaired);
        }

        [Fact]
        public void Triggers_DayMilestone_FiresOnlyAtRequiredDay()
        {
            var catalog = LoadCatalog();
            var milestones = catalog.Vignettes
                .Where(v => v.unlock == ShelterRoomIdentityCatalog.UnlockDayMilestone)
                .ToList();
            Assert.NotEmpty(milestones);

            foreach (var milestone in milestones)
            {
                var early = catalog.GetUnlockableVignettes(milestone.room_id,
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.DayElapsed, milestone.unlock_day - 1);
                Assert.DoesNotContain(milestone.id, early.Select(v => v.id));

                var onDay = catalog.GetUnlockableVignettes(milestone.room_id,
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.DayElapsed, milestone.unlock_day);
                Assert.Contains(milestone.id, onDay.Select(v => v.id));
            }
        }

        [Fact]
        public void Triggers_CatalogWideDayPass_IsStableAcrossRepeats()
        {
            var catalog = LoadCatalog();
            var first = catalog.GetDayMilestoneVignettes(60).Select(v => v.id).ToList();
            var again = catalog.GetDayMilestoneVignettes(60).Select(v => v.id).ToList();
            Assert.Equal(first, again); // ordinal-stable, no dictionary-order drift
            Assert.All(first, id =>
            {
                var v = catalog.GetVignette(id)!;
                Assert.True(v.unlock_day >= 1);
            });
        }

        [Fact]
        public void Triggers_UnknownRoom_YieldsNothing()
        {
            var catalog = LoadCatalog();
            Assert.Empty(catalog.GetUnlockableVignettes("room_nonexistent",
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
        }

        [Fact]
        public void UnlockVocabulary_MapsEveryTriggerAndRejectsUnknown()
        {
            var catalog = LoadCatalog();
            Assert.Equal(ShelterRoomIdentityCatalog.UnlockInspectRoom, ShelterRoomIdentityCatalog.UnlockValueFor(
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
            Assert.Equal(ShelterRoomIdentityCatalog.UnlockRepairPerformed, ShelterRoomIdentityCatalog.UnlockValueFor(
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed));
            Assert.Equal(ShelterRoomIdentityCatalog.UnlockDayMilestone, ShelterRoomIdentityCatalog.UnlockValueFor(
                ShelterRoomIdentityCatalog.RoomHistoryTrigger.DayElapsed));
            Assert.All(catalog.Vignettes, v =>
                Assert.Contains(v.unlock, new[]
                {
                    ShelterRoomIdentityCatalog.UnlockInspectRoom,
                    ShelterRoomIdentityCatalog.UnlockRepairPerformed,
                    ShelterRoomIdentityCatalog.UnlockDayMilestone
                }));
        }

        [Fact]
        public void HostSourceGate_EveryUnlockPathHasAWiredTrigger()
        {
            // Guards §13.2 / §29A.9: authored content may not use an unlock path the
            // Godot host never raises. Scans the shelter wiring partials.
            string? root = FindRepoRoot();
            if (root == null) return; // not running in repo tree

            string shelter = Path.Combine(root, "src", "Main.ShelterInfrastructure.cs");
            string owners = Path.Combine(root, "src", "Main.CampaignOwners.cs");
            Assert.True(File.Exists(shelter) && File.Exists(owners));
            string text = File.ReadAllText(shelter) + "\n" + File.ReadAllText(owners);

            Assert.Contains("RoomHistoryTrigger.RoomInspected", text);
            Assert.Contains("RoomHistoryTrigger.RepairPerformed", text);
            Assert.Contains("GetDayMilestoneVignettes", text);
        }

        [Fact]
        public void Catalog_Validate_ZeroErrors()
        {
            var catalog = LoadCatalog();
            var errors = catalog.Validate();
            Assert.True(errors.Count == 0, string.Join("; ", errors));
        }

        [Fact]
        public void Catalog_PilotRooms_HaveIdentity()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog.GetRoomIdentity("room_filtration"));
            Assert.NotNull(catalog.GetRoomIdentity("room_bunks"));
            foreach (var room in catalog.Rooms)
            {
                Assert.False(string.IsNullOrWhiteSpace(room.display_name));
                Assert.False(string.IsNullOrWhiteSpace(room.former_use));
                Assert.False(string.IsNullOrWhiteSpace(room.current_use));
                Assert.False(string.IsNullOrWhiteSpace(room.one_line_history));
            }
        }

        [Fact]
        public void Catalog_MissingFile_YieldsEmptyValidCatalog()
        {
            var catalog = ShelterRoomIdentityCatalog.Load(_files, _json, Path.Combine(_dataDir, "no_such_dir"));
            Assert.Equal(0, catalog.RoomCount);
            Assert.Empty(catalog.Validate());
            // Unknown rooms resolve unchanged and have no identity.
            Assert.Equal("room_unknown", catalog.ResolveRoomId("room_unknown"));
            Assert.Null(catalog.GetRoomIdentity("room_unknown"));
        }

        // ── Alias map (Plan 29 §5.1 — legacy ids bind to canonical records) ──

        [Fact]
        public void AliasResolution_LegacyIds_MapToCanonicalRooms()
        {
            var catalog = LoadCatalog();

            Assert.Equal("room_filtration", catalog.ResolveRoomId("room_filtration_stack"));
            Assert.Equal("room_filtration", catalog.ResolveRoomId("room_air_filtration"));
            Assert.Equal("room_bunks", catalog.ResolveRoomId("room_bunks_living"));

            // Legacy ids retrieve the canonical identity record.
            var viaLegacy = catalog.GetRoomIdentity("room_filtration_stack");
            Assert.NotNull(viaLegacy);
            Assert.Equal("room_filtration", viaLegacy.id);
        }

        [Fact]
        public void AliasResolution_CanonicalIds_AreStable()
        {
            var catalog = LoadCatalog();
            Assert.Equal("room_filtration", catalog.ResolveRoomId("room_filtration"));
            Assert.Equal("room_bunks", catalog.ResolveRoomId("room_bunks"));
        }

        [Fact]
        public void AliasResolution_GetLegacyAliases_BridgeCanonicalToLegacyRoster()
        {
            var catalog = LoadCatalog();
            var aliases = catalog.GetLegacyAliases("room_filtration");
            Assert.Contains("room_filtration_stack", aliases);
            Assert.Contains("room_air_filtration", aliases);
            Assert.Empty(catalog.GetLegacyAliases("room_nonexistent"));
        }

        // ── Vignette contract (Plan 29 §29A.8) ─────────────────────────────

        [Fact]
        public void Vignette_RoomReference_Resolves_AndRequiredFieldsPresent()
        {
            var catalog = LoadCatalog();
            foreach (var vignette in catalog.Vignettes)
            {
                Assert.NotNull(catalog.GetRoomIdentity(vignette.room_id));
                Assert.False(string.IsNullOrWhiteSpace(vignette.unlock));
                Assert.False(string.IsNullOrWhiteSpace(vignette.title));
                Assert.False(string.IsNullOrWhiteSpace(vignette.time_period));
                Assert.False(string.IsNullOrWhiteSpace(vignette.body));
            }
        }

        [Fact]
        public void Vignettes_AreReachable_AndTheirUnlockKeysRoundTrip()
        {
            // Every authored vignette must have a trigger the host actually raises
            // (no dead content), and every unlock key must survive save/load.
            var catalog = LoadCatalog();
            var journal = new JournalSystem();
            int inspect = 0, repair = 0, milestone = 0;

            foreach (var room in catalog.Rooms)
            {
                inspect += catalog.GetUnlockableVignettes(room.id,
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected).Count;
                repair += catalog.GetUnlockableVignettes(room.id,
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed).Count;
            }
            milestone = catalog.GetDayMilestoneVignettes(9999).Count;

            Assert.Equal(catalog.Vignettes.Count, inspect + repair + milestone);

            foreach (var vignette in catalog.Vignettes)
                Assert.True(journal.UnlockRoomHistorySeen(vignette.id), vignette.id);

            var restored = new JournalSystem();
            restored.RestoreState(journal.CaptureState());
            foreach (var vignette in catalog.Vignettes)
                Assert.True(restored.IsRoomHistorySeen(vignette.id), vignette.id);
        }

        // ── Journal unlock: persistence, idempotence, old-save default ────

        [Fact]
        public void Journal_RoomHistoryUnlock_IsIdempotent()
        {
            var journal = new JournalSystem();
            const string vignetteId = "room_history_the_first_filter_change";

            Assert.False(journal.IsRoomHistorySeen(vignetteId));
            Assert.True(journal.UnlockRoomHistorySeen(vignetteId));
            Assert.True(journal.IsRoomHistorySeen(vignetteId));
            // Second unlock is a no-op (KnowledgeBase.Discover returns false).
            Assert.False(journal.UnlockRoomHistorySeen(vignetteId));
            Assert.True(journal.IsRoomHistorySeen(vignetteId));
        }

        [Fact]
        public void Journal_RoomHistoryUnlock_SurvivesSaveRoundTrip()
        {
            var journal = new JournalSystem();
            const string vignetteId = "room_history_the_first_filter_change";
            journal.UnlockRoomHistorySeen(vignetteId);

            var save = journal.CaptureState();
            var restored = new JournalSystem();
            restored.RestoreState(save);

            Assert.True(restored.IsRoomHistorySeen(vignetteId));
        }

        [Fact]
        public void Journal_OldSave_DefaultsRoomHistoryLocked()
        {
            // A pre-Plan-29 journal save has no room_history_seen_* key: the
            // vignette stays locked and unlocks on the next inspection (29A.19).
            var journal = new JournalSystem();
            journal.UnlockItemSeen("item_test_seed");
            var save = journal.CaptureState();

            var restored = new JournalSystem();
            restored.RestoreState(save);

            Assert.False(restored.IsRoomHistorySeen("room_history_the_first_filter_change"));
        }

        // ── StartingLevel location-id migration (data authority: locations.json) ──

        [Fact]
        public void StartingLevel_DefaultUsesCanonicalLocationId()
        {
            var system = new StartingLevelSystem();
            Assert.Equal("loc_holdfast", system.State.locationId);
            Assert.Equal("loc_holdfast", StartingLevelSystem.HoldfastLocationId);
        }

        [Fact]
        public void StartingLevel_RestoreMigratesLegacyLocationId()
        {
            var system = new StartingLevelSystem();
            var legacySave = system.CaptureState();
            legacySave.locationId = "loc_bunker_holdfast";

            var restored = new StartingLevelSystem();
            restored.RestoreState(legacySave);

            Assert.Equal("loc_holdfast", restored.State.locationId);
        }

        // ── Inspection seam (canonical click bridged to legacy roster) ────

        [Fact]
        public void StartingLevel_InspectRoom_ReturnsFoundForKnownRooms()
        {
            var system = new StartingLevelSystem();
            // Legacy roster id (authoritative Day-1 roster).
            Assert.True(system.InspectRoom("room_filtration_stack"));
            // Unknown id: not found, no crash.
            Assert.False(system.InspectRoom("room_does_not_exist"));
            // Re-inspect: still found, idempotent.
            Assert.True(system.InspectRoom("room_filtration_stack"));
            Assert.True(system.State.rooms.First(r => r.roomId == "room_filtration_stack").isInspected);
        }
    }
}
