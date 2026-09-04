using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Culture;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 5 — CulturalArchiveVaultSystem behaviour gates:
    /// restoration/transcription/microfiche/discs/salons/chronicles, atomic
    /// inventory use, authoritative humidity input, and save continuation.
    /// </summary>
    public class CulturalArchiveVaultTests
    {
        private const string DataRelative = "Assets/StreamingAssets/Data";

        private static string DataDir
        {
            get
            {
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found))
                    return found;
                throw new InvalidOperationException("data dir not found from " + AppContext.BaseDirectory);
            }
        }

        private static List<CulturalArchiveTomeDefinition> LoadTomes() =>
            CulturalArchiveTomeCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        /// <summary>Strict availability double tracking live claims.</summary>
        private sealed class TrackingAvailability : IInstitutionAvailability
        {
            // One live claim per survivor across ALL institutions — mirrors the
            // shared assignment authority the host wires in Phase G.
            public readonly HashSet<string> Claims = new(StringComparer.Ordinal);
            public bool IsAvailable(string survivorId) => !Claims.Contains(survivorId);
            public bool TryClaim(string survivorId, string institutionId, string roleId) =>
                Claims.Add(survivorId);
            public void Release(string survivorId, string institutionId, string roleId) =>
                Claims.Remove(survivorId);
            public bool HasAny => Claims.Count > 0;
        }

        private sealed class Fixture
        {
            public Inventory.Inventory Inventory = new();
            public TrackingAvailability Availability = new();
            public VinylMoraleSystem Vinyl = new();
            public List<VinylRecordDefinition> HostRecordCatalog = new();
            public int HumidityPercent;
            public CulturalArchiveVaultSystem Culture = null!;
            public List<string> LostDocuments = new();

            public static Fixture Create(float humidityPercent = 20f, List<CulturalArchiveTomeDefinition>? tomes = null)
            {
                var f = new Fixture { HumidityPercent = (int)humidityPercent };
                f.Culture = new CulturalArchiveVaultSystem(
                    f.Inventory,
                    humidityPercentProvider: () => f.HumidityPercent,
                    availability: f.Availability,
                    vinyl: f.Vinyl);
                f.Culture.OnArchiveRecordingCreated += (_, def) => f.HostRecordCatalog.Add(def);
                f.Culture.OnDocumentLost += id => f.LostDocuments.Add(id);
                f.Culture.LoadTomeCatalog(tomes ?? LoadTomes());
                // Standard stock for cost gates.
                foreach (var id in new[]
                         {
                             "scrap_chemical", "clean_water", "paper_stock",
                             "microfiche_film", "acetate_blank_disc",
                         })
                    f.Inventory.TryProduce(id, 10);
                return f;
            }

            public void Tick(int fromDay, int days)
            {
                for (int i = 0; i < days; i++)
                    Culture.TickDay(fromDay + i);
            }
        }

        // ------------------------------------------------------------------
        // NORMAL
        // ------------------------------------------------------------------

        [Fact]
        public void CatalogLoad_SeedsTwelveAuthoritativeDocuments()
        {
            var f = Fixture.Create();
            Assert.Equal(12, f.Culture.Documents.Count);
            var first = f.Culture.Documents.First();
            var tome = LoadTomes().First(t => t.tome_id == first.document_id);
            Assert.Equal(tome.initial_degradation_permille, first.physical_degradation_permille);
        }

        [Fact]
        public void Restoration_ConsumesAtomically_AndRelieves()
        {
            var f = Fixture.Create();
            var doc = f.Culture.GetDocument("tome_stoic_meditations")!;
            int before = doc.physical_degradation_permille;

            var result = f.Culture.TryRestoreDocument("tome_stoic_meditations");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(9, f.Inventory.CountById("clean_water"));
            Assert.True(doc.physical_degradation_permille < before, "degradation reduced");
            Assert.True(doc.is_chemically_stabilized, "stabilized by restoration");
        }

        [Fact]
        public void Transcription_ProgressesDeterministically_ToCompletion()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Culture.TryStartTranscription("tome_children_primers", "survivor_scholar_a").Status);

            int transcribed = 0;
            f.Culture.OnTomeTranscribed += _ => transcribed++;
            f.Tick(1, 2); // 2-day protocol, 1000/2 per day
            var doc = f.Culture.GetDocument("tome_children_primers")!;
            Assert.Equal(1000, doc.transcription_permille);
            Assert.Equal("transcribed", doc.status);
            Assert.Equal(1, transcribed);
            Assert.False(f.Availability.HasAny, "scholar released at completion");
        }

        [Fact]
        public void Microfiche_PreservesKnowledge_AndBlocksDuplicateUnlock()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Culture.TryCreateMicroficheCopy("tome_mechanics_handbook_1974", "survivor_archivist").Status);
            var doc = f.Culture.GetDocument("tome_mechanics_handbook_1974")!;
            Assert.True(doc.knowledge_preserved);
            Assert.Equal(1, doc.microfiche_copy_count);

            var again = f.Culture.TryCreateMicroficheCopy("tome_mechanics_handbook_1974", "survivor_archivist");
            Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
            Assert.Equal(1, doc.microfiche_copy_count);
            Assert.Equal(9, f.Inventory.CountById("microfiche_film"));
        }

        [Fact]
        public void DiscCutting_ConsumesBlank_AndResolvesInPlaybackSystem()
        {
            var f = Fixture.Create();
            int fired = 0;
            f.Culture.OnArchiveRecordingCreated += (_, _) => fired++;

            var result = f.Culture.TryCutArchiveDisc(
                "archive_disc_first_winter_songs", "music_performance", "survivor_musician", 12);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(1, fired);
            Assert.Equal(9, f.Inventory.CountById("acetate_blank_disc"));

            // Host merges the authored definition into the playback catalog;
            // the record is owned by vinyl and resolvable by id.
            f.Vinyl.LoadCatalog(f.HostRecordCatalog);
            Assert.NotNull(f.Vinyl.GetRecord("archive_disc_first_winter_songs"));
            Assert.Contains("archive_disc_first_winter_songs", f.Vinyl.State.ownedRecordIds);

            var play = f.Vinyl.Play("archive_disc_first_winter_songs", 13);
            Assert.Equal(ActionResult.StatusKind.Success, play.Status);
        }

        [Fact]
        public void Salon_AppliesOncePerDay_NeverStacks_AndCooldowns()
        {
            var f = Fixture.Create();
            int ticks = 0;
            f.Culture.OnSalonMoraleTick += _ => ticks++;
            int ended = 0;
            f.Culture.OnSalonEnded += _ => ended++;

            Assert.Equal(ActionResult.StatusKind.Success, f.Culture.TryStartSalon(1).Status);
            f.Tick(1, 5); // 5-day salon

            Assert.Equal(5, ticks);  // exactly one morale tick per active day
            Assert.Equal(1, ended);
            Assert.False(f.Culture.Salon.active);

            // cooldown blocks an immediate second salon
            var blocked = f.Culture.TryStartSalon(6);
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.Equal(5, ticks);  // no further ticks while blocked
        }

        [Fact]
        public void Chronicle_RecordsStructuredMilestone_Once()
        {
            var f = Fixture.Create();
            ArchiveChronicleEntry? seen = null;
            f.Culture.OnChronicleEntryAdded += e => seen = e;

            var result = f.Culture.TryRecordChronicleEntry(
                "first_winter_survived", 90, "chronicle.first_winter",
                new[] { "survivor_clerk" }, "survivor_clerk");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.NotNull(seen);
            Assert.Equal("first_winter_survived", seen!.event_type);
            Assert.Equal(90, seen.campaign_day);
            Assert.StartsWith("volume_", seen.volume_id);

            // same milestone again → rejected, no duplicate entry
            var dup = f.Culture.TryRecordChronicleEntry(
                "first_winter_survived", 90, "chronicle.first_winter", new[] { "survivor_clerk" });
            Assert.Equal(ActionResult.StatusKind.Blocked, dup.Status);
            Assert.Equal(1, f.Culture.Chronicle.Count);
        }

        // ------------------------------------------------------------------
        // INVALID / BOUNDARY
        // ------------------------------------------------------------------

        [Fact]
        public void Restoration_UnknownDocument_AndMissingInputs_FailAtomically()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Culture.TryRestoreDocument("tome_this_does_not_exist").Status);

            // drain clean_water → restoration cost unmet
            f.Inventory.TryConsume("clean_water", 10);
            var doc = f.Culture.GetDocument("tome_mechanics_handbook_1974")!;
            int before = doc.physical_degradation_permille;
            var blocked = f.Culture.TryRestoreDocument("tome_mechanics_handbook_1974");
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.Equal(before, doc.physical_degradation_permille);
            Assert.Equal(10, f.Inventory.CountById("scrap_chemical")); // untouched
        }

        [Fact]
        public void Transcription_RejectsUnknownDoc_AndUnavailableScholar()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Culture.TryStartTranscription("tome_nope", "survivor_x").Status);

            Assert.True(f.Availability.TryClaim("survivor_scholar_a", "institution_elsewhere", "other"));
            var blocked = f.Culture.TryStartTranscription("tome_stoic_meditations", "survivor_scholar_a");
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);

            // double-assignment of an already-transcribing document
            Assert.Equal(ActionResult.StatusKind.Success,
                f.Culture.TryStartTranscription("tome_stoic_meditations", "survivor_scholar_b").Status);
            var second = f.Culture.TryStartTranscription("tome_stoic_meditations", "survivor_scholar_c");
            Assert.Equal(ActionResult.StatusKind.Blocked, second.Status);
        }

        [Fact]
        public void DiscCutting_RejectsUnknownCategory_AndDuplicateIds()
        {
            var f = Fixture.Create();
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Culture.TryCutArchiveDisc("disc_a", "hall_of_fame", "survivor_x", 1).Status);

            Assert.Equal(ActionResult.StatusKind.Success,
                f.Culture.TryCutArchiveDisc("disc_b", "oral_history", "survivor_x", 1).Status);
            Assert.Equal(ActionResult.StatusKind.Blocked,
                f.Culture.TryCutArchiveDisc("disc_b", "oral_history", "survivor_x", 2).Status);
            Assert.Equal(1, f.Culture.Recordings.Count);
        }

        // ------------------------------------------------------------------
        // ENVIRONMENT
        // ------------------------------------------------------------------

        [Fact]
        public void Humidity_DrivesDegradation_ThroughAuthoritativeInput()
        {
            var dry = Fixture.Create(humidityPercent: 0f);
            var soaked = Fixture.Create(humidityPercent: 100f);

            dry.Tick(1, 10);
            soaked.Tick(1, 10);

            int dryDeg = dry.Culture.GetDocument("tome_metallurgical_handbook")!.physical_degradation_permille;
            int soakedDeg = soaked.Culture.GetDocument("tome_metallurgical_handbook")!.physical_degradation_permille;
            Assert.True(soakedDeg > dryDeg,
                $"soaked {soakedDeg} should exceed dry {dryDeg}");
        }

        [Fact]
        public void Stabilization_ReducesAuthoredDegradation()
        {
            var stabilized = Fixture.Create(humidityPercent: 100f);
            var untreated = Fixture.Create(humidityPercent: 100f);

            stabilized.Culture.TryRestoreDocument("tome_agricultural_botany"); // stabilizes
            stabilized.Tick(1, 10);
            untreated.Tick(1, 10);

            int a = stabilized.Culture.GetDocument("tome_agricultural_botany")!.physical_degradation_permille;
            int b = untreated.Culture.GetDocument("tome_agricultural_botany")!.physical_degradation_permille;
            Assert.True(a < b, $"stabilized {a} should decay slower than untreated {b}");
        }

        [Fact]
        public void LostPaper_DoesNotEraseMicroficheKnowledge()
        {
            var f = Fixture.Create(humidityPercent: 100f);
            // Shrink the book to be brittle/soaked: force degradation high via a
            // bespoke tome so the test stays fast.
            var tomes = LoadTomes();
            tomes.Add(new CulturalArchiveTomeDefinition
            {
                tome_id = "tome_test_frail",
                display_name = "Frail Test Folio",
                category = "technical",
                transcription_days = 1,
                paper_brittleness_tier = 3,
                initial_degradation_permille = 890,
                microfiche_frame_density = 10,
            });
            var f2 = Fixture.Create(humidityPercent: 100f, tomes: tomes);
            Assert.Equal(ActionResult.StatusKind.Success,
                f2.Culture.TryCreateMicroficheCopy("tome_test_frail", "survivor_archivist").Status);

            f2.Tick(1, 40); // long decay window
            var doc = f2.Culture.GetDocument("tome_test_frail")!;
            Assert.True(doc.physical_degradation_permille >= CulturalArchiveVaultSystem.LostThresholdPermille
                        || doc.physical_degradation_permille > 980,
                "paper should keep decaying");
            Assert.True(doc.knowledge_preserved, "microfiche knowledge survives paper loss");
        }

        // ------------------------------------------------------------------
        // SAVE / RESTORE
        // ------------------------------------------------------------------

        [Fact]
        public void ScholarAssignment_AndProgress_SurviveSaveLoad()
        {
            var f = Fixture.Create();
            f.Culture.TryStartTranscription("tome_symphonic_scores", "survivor_scholar_deep");
            f.Tick(1, 2); // partial progress on a 7-day protocol

            var saved = f.Culture.CaptureState();
            var fresh = new CulturalArchiveVaultSystem(f.Inventory, availability: f.Availability, vinyl: f.Vinyl);
            fresh.LoadTomeCatalog(LoadTomes());
            fresh.RestoreState(saved);

            var doc = fresh.GetDocument("tome_symphonic_scores")!;
            Assert.Equal("survivor_scholar_deep", doc.active_scholar_id);
            Assert.Equal("transcribing", doc.status);
            Assert.Equal(doc.transcription_permille, f.Culture.GetDocument("tome_symphonic_scores")!.transcription_permille);
        }

        [Fact]
        public void OldSave_MissingCultureSection_DefaultsSafely()
        {
            var f = Fixture.Create();
            f.Culture.RestoreState(null);
            Assert.Empty(f.Culture.Recordings);
            Assert.Empty(f.Culture.Chronicle);
            Assert.False(f.Culture.Salon.active);
        }

        [Fact]
        public void UninterruptedVsRestored_ContinuationMatches()
        {
            var tomes = LoadTomes();

            // Run A — uninterrupted 12 days.
            var a = Fixture.Create(humidityPercent: 65f);
            a.Culture.TryStartTranscription("tome_radio_service_manual", "survivor_scholar_a");
            a.Culture.TryStartSalon(1);
            a.Tick(1, 12);

            // Run B — 6 days, save, fresh composition, 6 more days.
            var b = Fixture.Create(humidityPercent: 65f, tomes: tomes);
            b.Culture.TryStartTranscription("tome_radio_service_manual", "survivor_scholar_a");
            b.Culture.TryStartSalon(1);
            b.Tick(1, 6);
            var saved = b.Culture.CaptureState();

            var freshInventory = new Inventory.Inventory();
            foreach (var id in new[] { "scrap_chemical", "clean_water", "paper_stock", "microfiche_film", "acetate_blank_disc" })
                freshInventory.TryProduce(id, 10);
            var fresh = new CulturalArchiveVaultSystem(
                freshInventory, humidityPercentProvider: () => 65, vinyl: b.Vinyl);
            fresh.LoadTomeCatalog(tomes);
            fresh.RestoreState(saved);
            for (int i = 6; i < 12; i++)
                fresh.TickDay(1 + i);

            var docA = a.Culture.GetDocument("tome_radio_service_manual")!;
            var docB = fresh.GetDocument("tome_radio_service_manual")!;
            Assert.Equal(docA.transcription_permille, docB.transcription_permille);
            Assert.Equal(docA.physical_degradation_permille, docB.physical_degradation_permille);
            Assert.Equal(docA.status, docB.status);
            Assert.Equal(a.Culture.Salon.active, fresh.Salon.active);
            Assert.Equal(a.Culture.Salon.cooldown_until_day, fresh.Salon.cooldown_until_day);
            Assert.Equal(a.Culture.Documents.Select(d => d.physical_degradation_permille),
                         fresh.Documents.Select(d => d.physical_degradation_permille));
        }
    }
}
