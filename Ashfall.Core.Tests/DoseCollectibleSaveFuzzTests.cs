// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Save-fuzz battery for the Plan 81/86 persistence chains (skill:
    /// ashfall-save-fuzz, Phase 2 battery scoped to the dose + collectible
    /// state). Five cases per store/codec where applicable:
    /// clean round-trip, checksum-mutation reject, null-checksum reject,
    /// legacy fallback, version/future guard — plus serialize-twice
    /// byte-determinism for every envelope.
    ///
    /// Coverage map (what already existed vs what this file adds):
    /// - DoseLedgerSaveCodec: tamper + checksumless rejects existed in
    ///   DoseLedgerSystemTests; this file adds full 5-register round-trip,
    ///   v1→v2 migration, future-version guard, byte-determinism.
    /// - CollectibleDiscoveryState: ordinal/round-trip/pre-Plan-47 coverage
    ///   existed in CollectibleDiscoveryPersistenceTests; this file adds
    ///   v1-legacy restore, null-restore, byte-determinism.
    /// - UniqueItemClaimRegistry, CollectibleTutorialTracker: no save tests
    ///   existed — full battery here.
    /// - ScavengingTableCatalog: pinned as NOT a save implementer (data-only).
    /// </summary>
    public class DoseCollectibleSaveFuzzTests
    {
        private static readonly IJsonSerializer Json = new SystemTextJsonSerializer();

        // ── Dose chain (DoseLedgerSaveCodec, checksummed v2 envelope) ─────

        private static (DoseLedgerSystem ledger, SickListSystem sick, CohortSystem cohort,
            VoluntaryRegisterSystem voluntary, QuestlineSystem quests) BuildDoseSystems()
        {
            var ledger = new DoseLedgerSystem();
            ledger.AssignDosimeter("survivor_fuzz_a", "tag_f1", 12f);
            ledger.AssignDosimeter("survivor_fuzz_b", "tag_f2", 30f);
            var rng = new SeededRng(860);
            ledger.BookReading("survivor_fuzz_a", 14, 0.00085f, "loc_shelter_exterior_approach",
                false, false, false, rng);
            ledger.BookReading("survivor_fuzz_b", 15, 0.18f, "loc_military_depot_perimeter",
                false, false, false, rng);

            var sick = new SickListSystem();
            sick.Diagnose("survivor_fuzz_b", DoseLedgerSystem.BandRed, 16);

            var cohort = new CohortSystem();
            cohort.BookChild("sv_fuzz_child", new[] { "survivor_fuzz_a" }, "low", 20, "told a number");
            cohort.CorrectBaseline("sv_fuzz_child", "medium");

            var voluntary = new VoluntaryRegisterSystem();
            voluntary.Volunteer("survivor_fuzz_a", "brine line inspection", 22, "Someone has to walk it.");
            voluntary.CompleteVolunteer("survivor_fuzz_a", "brine line inspection", 12f, 23);

            var quests = new QuestlineSystem();
            quests.RegisterQuestline(new QuestlineDefinition
            {
                questlineId = "quest_fuzz_line",
                title = "Fuzz Line",
                synopsis = "deterministic",
                stages =
                {
                    new QuestStage
                    {
                        stageId = "s1", title = "one", isTerminal = true,
                        choices = { new QuestChoice { choiceId = "c1", text = "sign" } }
                    }
                }
            });
            quests.StartQuestline("quest_fuzz_line", 24);

            return (ledger, sick, cohort, voluntary, quests);
        }

        [Fact]
        public void DoseFuzz_FullFiveRegisterEnvelope_CleanRoundTrip()
        {
            var (ledger, sick, cohort, voluntary, quests) = BuildDoseSystems();

            var save = DoseLedgerSaveCodec.Capture(30, ledger, sick, cohort, voluntary, quests);
            string json = DoseLedgerSaveCodec.Encode(save, Json);

            var fresh = BuildDoseSystems();
            var decoded = DoseLedgerSaveCodec.Decode(json, Json);
            Assert.Equal(2, decoded.saveVersion);
            Assert.Equal(30, decoded.simDay);
            DoseLedgerSaveCodec.Restore(decoded, fresh.ledger, fresh.sick, fresh.cohort, fresh.voluntary, fresh.quests);

            // Ledger provenance survives with location attribution (Plan 81).
            var entry = fresh.ledger.Entries.Single(e => e.survivorId == "survivor_fuzz_b");
            Assert.Equal(2, fresh.ledger.Entries.Sum(e => e.readingsHistory.Count));
            Assert.Equal("loc_military_depot_perimeter", entry.readingsHistory[0].source);
            Assert.Equal(0.18f, entry.readingsHistory[0].nominalMsv);

            Assert.Single(fresh.sick.Bands);
            Assert.Single(fresh.cohort.Children);
            Assert.Single(fresh.voluntary.Entries);
            Assert.True(fresh.voluntary.Entries[0].completed);
            Assert.NotNull(fresh.quests.FindDefinition("quest_fuzz_line"));
            Assert.Contains(fresh.quests.State.active, a => a.questlineId == "quest_fuzz_line");
        }

        [Fact]
        public void DoseFuzz_ChecksumMutation_IsRejectedWithExactError()
        {
            var (ledger, sick, cohort, voluntary, quests) = BuildDoseSystems();
            var save = DoseLedgerSaveCodec.Capture(30, ledger, sick, cohort, voluntary, quests);
            string forged = DoseLedgerSaveCodec.Encode(save, Json);
            // Flip one field in the serialized payload AFTER checksumming — the
            // checksum on disk no longer matches the mutated content.
            forged = json_replace_simday(forged, 30, 99);
            var ex = Assert.Throws<InvalidOperationException>(() => DoseLedgerSaveCodec.Decode(forged, Json));
            Assert.Contains("checksum mismatch", ex.Message);
        }

        [Fact]
        public void DoseFuzz_NullChecksum_IsRejected_NotSilentlyAccepted()
        {
            var (ledger, sick, cohort, voluntary, quests) = BuildDoseSystems();
            var save = DoseLedgerSaveCodec.Capture(30, ledger, sick, cohort, voluntary, quests);
            save.Checksum = string.Empty;
            string json = Json.Serialize(save);
            var ex = Assert.Throws<InvalidOperationException>(() => DoseLedgerSaveCodec.Decode(json, Json));
            Assert.Contains("no checksum", ex.Message);
        }

        [Fact]
        public void DoseFuzz_V1Legacy_MigratesWithEmptyQuestSection()
        {
            // Build a valid v1 payload over its FROZEN shape.
            var (ledger, sick, cohort, voluntary, _) = BuildDoseSystems();
            var v1 = new DoseLedgerSaveV1
            {
                saveVersion = 1,
                simDay = 44,
                doseLedger = ledger.CaptureState(),
                sickList = sick.CaptureState(),
                cohort = cohort.CaptureState(),
                voluntaryRegister = voluntary.CaptureState(),
            };
            v1.Checksum = SaveChecksum.Compute(v1);
            string json = Json.Serialize(v1);

            var decoded = DoseLedgerSaveCodec.Decode(json, Json);
            Assert.Equal(DoseLedgerSave.CurrentSaveVersion, decoded.saveVersion);
            Assert.Equal(44, decoded.simDay);
            Assert.Empty(decoded.quests.active);
            // Migrated payload re-stamps a valid v2 checksum.
            DoseLedgerSaveCodec.Decode(Json.Serialize(decoded), Json); // must not throw
        }

        [Fact]
        public void DoseFuzz_FutureVersion_IsRejected()
        {
            var (ledger, sick, cohort, voluntary, quests) = BuildDoseSystems();
            var save = DoseLedgerSaveCodec.Capture(30, ledger, sick, cohort, voluntary, quests);
            save.saveVersion = DoseLedgerSave.CurrentSaveVersion + 1;
            save.Checksum = SaveChecksum.Compute(save);
            string json = Json.Serialize(save);
            var ex = Assert.Throws<InvalidOperationException>(() => DoseLedgerSaveCodec.Decode(json, Json));
            Assert.Contains("newer than supported", ex.Message);
        }

        [Fact]
        public void DoseFuzz_SerializeTwice_IsByteIdentical()
        {
            var (ledger, sick, cohort, voluntary, quests) = BuildDoseSystems();
            var save = DoseLedgerSaveCodec.Capture(30, ledger, sick, cohort, voluntary, quests);
            string a = DoseLedgerSaveCodec.Encode(save, Json);
            string b = DoseLedgerSaveCodec.Encode(save, Json);
            Assert.Equal(a, b);
        }

        // ── Collectible chain (bare-state CaptureState/RestoreState) ─────

        [Fact]
        public void DiscoveryFuzz_CleanRoundTrip_PreservesPartitionsAndLocations()
        {
            var state = new CollectibleDiscoveryState();
            Assert.True(state.MarkDiscovered("item_collectible_road_map", "table_loot_police_station"));
            Assert.True(state.MarkDiscovered("item_collectible_family_portrait", "table_loot_apartment_block"));
            Assert.True(state.AcknowledgeDiscovery("item_collectible_road_map"));
            // ever-acquired only: acknowledge road_map then discover it again is not
            // possible; use a third id to occupy unacknowledged partition.
            Assert.True(state.MarkDiscovered("item_collectible_exchange_day_newspaper", "table_loot_school"));

            var save = state.CaptureState();
            string json = Json.Serialize(save);

            var fresh = new CollectibleDiscoveryState();
            fresh.RestoreState(Json.Deserialize<CollectibleDiscoverySave>(json));

            Assert.True(fresh.IsAcknowledged("item_collectible_road_map"));
            Assert.True(fresh.IsUnacknowledged("item_collectible_exchange_day_newspaper"));
            Assert.True(fresh.WasEverAcquired("item_collectible_family_portrait"));
            Assert.Equal("table_loot_police_station", fresh.GetDiscoveryLocation("item_collectible_road_map"));
            Assert.Equal(3, fresh.Count);
        }

        [Fact]
        public void DiscoveryFuzz_SerializeTwice_IsByteIdentical()
        {
            var state = new CollectibleDiscoveryState();
            // Insert out of ordinal order — capture must normalize.
            state.MarkDiscovered("item_collective_zulu_marker".Replace("collective", "collectible"), null);
            state.MarkDiscovered("item_collectible_alpha_marker", null);
            state.MarkDiscovered("item_collectible_mid_marker", null);
            var a = Json.Serialize(state.CaptureState());
            var b = Json.Serialize(state.CaptureState());
            Assert.Equal(a, b);
            Assert.Contains("item_collectible_alpha_marker", a);
        }

        [Fact]
        public void DiscoveryFuzz_V1LegacyRestore_MarksAllDiscoveredAsAcknowledged()
        {
            var legacy = new CollectibleDiscoverySave
            {
                schema_version = 1,
                discovered_ids = new[] { "item_collectible_road_map", "item_collectible_topo_map" },
            };
            var state = new CollectibleDiscoveryState();
            state.RestoreState(legacy);
            // v1 invariant: every legacy id is acknowledged (no unacknowledged backlog).
            Assert.True(state.IsAcknowledged("item_collectible_road_map"));
            Assert.True(state.IsAcknowledged("item_collectible_topo_map"));
            Assert.Equal(0, state.UnacknowledgedCount);
            Assert.Equal(2, state.Count);
        }

        [Fact]
        public void DiscoveryFuzz_NullRestore_IsHonestEmptyState()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_road_map", null);
            state.RestoreState(null);
            Assert.Equal(0, state.Count);
            Assert.False(state.IsDiscovered("item_collectible_road_map"));
        }

        [Fact]
        public void ClaimsFuzz_CleanRoundTrip_PreservesClaimsAndAvailabilityGate()
        {
            var uniques = new[]
            {
                "item_collectible_casualty_list", "item_collectible_exchange_day_newspaper",
                "item_collectible_survivor_map", "item_collectible_not_really_unique"
            };
            var registry = new UniqueItemClaimRegistry(uniques);
            Assert.True(registry.TryClaim("item_collectible_casualty_list"));
            Assert.True(registry.TryClaim("item_collectible_survivor_map"));
            // TryClaim is claim-or-confirm: re-claiming returns true and the
            // claim set stays a single entry (idempotent).
            Assert.True(registry.TryClaim("item_collectible_casualty_list"));
            Assert.Equal(2, registry.CaptureState().claimed_unique_ids.Length);

            var save = registry.CaptureState();
            string json = Json.Serialize(save);

            var fresh = new UniqueItemClaimRegistry(uniques);
            fresh.RestoreState(Json.Deserialize<UniqueClaimSave>(json));
            Assert.Equal(2, fresh.ClaimedCount);
            Assert.False(fresh.IsAvailable("item_collectible_casualty_list"));
            Assert.True(fresh.IsAvailable("item_collectible_exchange_day_newspaper"));
            Assert.True(fresh.IsAvailable("item_collectible_not_really_unique")); // unknown id passthrough
        }

        [Fact]
        public void ClaimsFuzz_SerializeTwice_IsByteIdentical_AndOrdinalSorted()
        {
            var uniques = new[]
            {
                "item_collectible_casualty_list", "item_collectible_exchange_day_newspaper",
                "item_collectible_survivor_map"
            };
            var registry = new UniqueItemClaimRegistry(uniques);
            registry.TryClaim("item_collectible_survivor_map");
            registry.TryClaim("item_collectible_casualty_list"); // out of ordinal order
            var save = registry.CaptureState();
            string a = Json.Serialize(save);
            string b = Json.Serialize(registry.CaptureState());
            Assert.Equal(a, b);
            // Ordinal-sorted persistence (checksum-stable by contract).
            Assert.Equal(
                new[] { "item_collectible_casualty_list", "item_collectible_survivor_map" },
                save.claimed_unique_ids);
        }

        [Fact]
        public void ClaimsFuzz_StaleSaveIds_NoLongerUnique_AreDroppedOnRestore()
        {
            var oldUniques = new[] { "item_collectible_casualty_list", "item_collectible_retired_unique" };
            var registry = new UniqueItemClaimRegistry(oldUniques);
            registry.TryClaim("item_collectible_casualty_list");
            registry.TryClaim("item_collectible_retired_unique");
            var save = registry.CaptureState();

            // The item is no longer unique under the current catalog.
            var currentUniques = new[] { "item_collectible_casualty_list" };
            var fresh = new UniqueItemClaimRegistry(currentUniques);
            fresh.RestoreState(save);
            Assert.Equal(1, fresh.ClaimedCount);
            // A stale claim must not suppress an ordinary item forever.
            Assert.True(fresh.IsAvailable("item_collectible_retired_unique"));
        }

        [Fact]
        public void TutorialFuzz_CleanRoundTrip_PreservesSeenAndQueue()
        {
            var tracker = new CollectibleTutorialTracker();
            tracker.OnCollectibleDiscovered(new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "knowledge", EffectApplied = true,
            });
            Assert.Equal(2, tracker.SeenCount); // cultural_artifacts + reading_and_discovering

            var save = tracker.CaptureState();
            string json = Json.Serialize(save);

            var fresh = new CollectibleTutorialTracker();
            fresh.RestoreState(Json.Deserialize<CollectibleTutorialSave>(json));
            Assert.True(fresh.HasSeen(CollectibleTutorialTracker.CulturalArtifactsId));
            Assert.True(fresh.HasSeen(CollectibleTutorialTracker.ReadingAndDiscoveringId));
            // Re-firing the same trigger on the restored tracker must not re-queue.
            fresh.OnCollectibleDiscovered(new CollectibleDispatchResult
            {
                IsCollectible = true,
                DiscoveryRegistered = true,
                EffectType = "knowledge", EffectApplied = true,
            });
            Assert.Equal(2, fresh.SeenCount);
        }

        [Fact]
        public void TutorialFuzz_NullRestore_IsHonestEmptyState()
        {
            var tracker = new CollectibleTutorialTracker();
            tracker.MarkSeen(CollectibleTutorialTracker.CulturalArtifactsId);
            tracker.RestoreState(null);
            Assert.Equal(0, tracker.SeenCount);
        }

        // ── ScavengingTableCatalog: pinned as NOT a save implementer ─────

        [Fact]
        public void ScavengingCatalog_IsPinnedAsDataOnly_NoSaveSurface()
        {
            // The placement catalog is loaded from the JSON authority and holds
            // no campaign state. If a save surface ever appears on it, this pin
            // forces a review of which authority owns persistence.
            var methods = typeof(ScavengingTableCatalog).GetMethods(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            Assert.DoesNotContain(methods, m => m.Name == "CaptureState" || m.Name == "RestoreState");
        }

        // ── helpers ──

        private static string json_replace_simday(string json, int from, int to)
        {
            var needle = "\"simDay\":" + from;
            var replacement = "\"simDay\":" + to;
            var compact = json.Replace(", ", ",").Replace(": ", ":");
            Assert.Contains(needle, compact);
            return compact.Replace(needle, replacement);
        }
    }
}
