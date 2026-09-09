using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Codex;
using Ashfall.Core.IO;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests.Codex
{
    /// <summary>
    /// codex_entries.json loader and its integration as the fourth source of
    /// <see cref="CodexProjectionBuilder"/>.
    ///
    /// Covers: the authoritative 63-record catalog and its closed vocabularies,
    /// loader hardening (missing / malformed / future schema / duplicates /
    /// bodyless entries), unlock derivation from journal knowledge and the host
    /// faction-contact resolver, spoiler-tier withholding, provenance to
    /// confidence mapping, category mapping, backward compatibility of the
    /// builder's original signature, and projection determinism.
    /// </summary>
    public sealed class CodexEntryCatalogTests
    {
        private static string? FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return null;
        }

        private static List<AuthoredCodexEntry> LoadAuthoritative()
        {
            string? dir = FindDataDir();
            Assert.False(dir == null, "StreamingAssets/Data directory not found");
            return CodexEntryCatalogLoader.LoadEntries(dir!, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static AuthoredCodexEntry Entry(
            string id,
            string category = CodexEntryCatalogLoader.CategoryLocations,
            CodexUnlockCondition condition = CodexUnlockCondition.VisitLocation,
            string unlockRef = "loc_test_site",
            int spoilerTier = 0,
            string provenance = CodexEntryCatalogLoader.ProvenanceCanonical,
            string body = "Authored body text for the test record.")
        {
            return new AuthoredCodexEntry
            {
                id = id,
                category = category,
                displayName = "Test Record",
                spoilerTier = spoilerTier,
                unlockCondition = condition,
                unlockRef = unlockRef,
                body = body,
                provenance = provenance,
                tags = new List<string> { "test_tag" }
            };
        }

        private static IReadOnlyList<CodexEntryProjection> Project(
            IReadOnlyList<AuthoredCodexEntry> authored,
            JournalSystem? journal = null,
            Func<string, bool>? factionContact = null,
            int maxSpoilerTier = int.MaxValue,
            int day = 5)
        {
            return CodexProjectionBuilder.Build(
                null, null, null, journal, day, authored, factionContact, maxSpoilerTier);
        }

        // ── authoritative catalog ─────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsAll63AuthoredRecords()
        {
            Assert.Equal(63, LoadAuthoritative().Count);
        }

        [Fact]
        public void Catalog_IdsAreUniqueWithNonEmptyProseAndDisplayName()
        {
            var entries = LoadAuthoritative();
            Assert.Equal(entries.Count, entries.Select(e => e.id).Distinct(StringComparer.Ordinal).Count());
            Assert.All(entries, e =>
            {
                Assert.False(string.IsNullOrWhiteSpace(e.id));
                Assert.False(string.IsNullOrWhiteSpace(e.displayName));
                Assert.True(e.body.Length >= 80, $"{e.id}: body too thin ({e.body.Length})");
                Assert.False(string.IsNullOrWhiteSpace(e.unlockRef), $"{e.id}: no unlock reference");
            });
        }

        [Fact]
        public void Catalog_VocabulariesAreClosedAndFullyParsed()
        {
            var entries = LoadAuthoritative();
            var categories = new HashSet<string>(entries.Select(e => e.category), StringComparer.Ordinal);
            var expectedCategories = new HashSet<string>(StringComparer.Ordinal)
            {
                CodexEntryCatalogLoader.CategoryRegions,
                CodexEntryCatalogLoader.CategoryLocations,
                CodexEntryCatalogLoader.CategoryFactions,
                CodexEntryCatalogLoader.CategoryDeepLore,
                CodexEntryCatalogLoader.CategoryWildlife
            };
            Assert.True(categories.SetEquals(expectedCategories),
                "unexpected categories: " + string.Join(",", categories.Except(expectedCategories)));

            var provenances = new HashSet<string>(entries.Select(e => e.provenance), StringComparer.Ordinal);
            var expectedProvenance = new HashSet<string>(StringComparer.Ordinal)
            {
                CodexEntryCatalogLoader.ProvenanceCanonical,
                CodexEntryCatalogLoader.ProvenanceRestricted,
                CodexEntryCatalogLoader.ProvenanceEyewitness,
                CodexEntryCatalogLoader.ProvenanceMaterial,
                CodexEntryCatalogLoader.ProvenanceRumor
            };
            Assert.True(provenances.IsSubsetOf(expectedProvenance),
                "unexpected provenance: " + string.Join(",", provenances.Except(expectedProvenance)));

            // No record may fall through to Unknown: an unrecognized condition
            // string would silently make that lore unrecoverable.
            Assert.All(entries, e => Assert.NotEqual(CodexUnlockCondition.Unknown, e.unlockCondition));
        }

        [Fact]
        public void Catalog_SpoilerTiersStayInTheAuthoredBand()
        {
            var tiers = LoadAuthoritative().Select(e => e.spoilerTier).ToList();
            Assert.All(tiers, t => Assert.InRange(t, 0, 3));
            Assert.Contains(3, tiers);
        }

        [Fact]
        public void Catalog_VisitLocationRefsResolveInTheMergedLocationAuthority()
        {
            string? dir = FindDataDir();
            Assert.False(dir == null, "StreamingAssets/Data directory not found");

            // Locations are authored across several catalogs; the codex may point at
            // any of them, so resolve against the union rather than locations.json alone.
            var known = new HashSet<string>(StringComparer.Ordinal);
            foreach (string path in Directory.GetFiles(dir!, "*location*.json"))
            {
                string raw = File.ReadAllText(path);
                foreach (string id in System.Text.RegularExpressions.Regex.Matches(raw, "\"(loc(?:ation)?_[a-z0-9_]+)\"")
                         .Cast<System.Text.RegularExpressions.Match>().Select(m => m.Groups[1].Value))
                {
                    known.Add(id);
                }
            }
            Assert.NotEmpty(known);

            var visitRefs = LoadAuthoritative()
                .Where(e => e.unlockCondition == CodexUnlockCondition.VisitLocation)
                .Select(e => e.unlockRef)
                .Distinct(StringComparer.Ordinal)
                .ToList();
            Assert.NotEmpty(visitRefs);

            var unresolved = visitRefs.Where(r => !known.Contains(r)).OrderBy(r => r, StringComparer.Ordinal).ToList();
            Assert.True(unresolved.Count == 0,
                "visit_location refs that resolve in no location catalog:\n  " + string.Join("\n  ", unresolved));
        }

        [Fact]
        public void Catalog_NonLocationUnlockRefsAreNotLocations()
        {
            foreach (var e in LoadAuthoritative())
            {
                if (e.unlockCondition == CodexUnlockCondition.VisitLocation) continue;
                Assert.False(e.unlockRef.StartsWith("loc_", StringComparison.Ordinal)
                             || e.unlockRef.StartsWith("location_", StringComparison.Ordinal),
                    $"{e.id}: {e.unlockCondition} should not reference a location ({e.unlockRef})");
            }
        }

        // ── loader hardening ──────────────────────────────────────────────────

        [Fact]
        public void Loader_MissingDirectoryYieldsEmptyWithoutThrowing()
        {
            var entries = CodexEntryCatalogLoader.LoadEntries(
                Path.Combine(Path.GetTempPath(), "ashfall_codex_absent_" + Guid.NewGuid().ToString("N")),
                new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(entries);
        }

        [Fact]
        public void Loader_MalformedJsonYieldsEmptyWithoutThrowing()
        {
            var entries = CodexEntryCatalogLoader.Parse(
                "{\"schema_version\":1,\"entries\":[{\"id\":", new SystemTextJsonSerializer());
            Assert.Empty(entries);
        }

        [Fact]
        public void Loader_FutureSchemaIsRefusedWholeNotPartially()
        {
            string raw = "{\"schema_version\":99,\"entries\":[{" +
                         "\"id\":\"codex_x\",\"category\":\"regions\",\"display_name\":\"X\"," +
                         "\"spoiler_tier\":0,\"unlock_condition\":\"visit_location\"," +
                         "\"unlock_ref\":\"loc_x\",\"body\":\"long enough body text to pass\"," +
                         "\"provenance\":\"canonical\",\"tags\":[\"t\"]}]}";
            Assert.Empty(CodexEntryCatalogLoader.Parse(raw, new SystemTextJsonSerializer()));
        }

        [Fact]
        public void Loader_SkipsBodylessAndDuplicateRecords()
        {
            string raw = "{\"schema_version\":1,\"entries\":[" +
                         // no body -> skipped
                         "{\"id\":\"codex_a\",\"category\":\"regions\",\"display_name\":\"A\"," +
                         "\"unlock_condition\":\"visit_location\",\"unlock_ref\":\"loc_a\",\"body\":\"\"}," +
                         // valid
                         "{\"id\":\"codex_b\",\"category\":\"regions\",\"display_name\":\"B\"," +
                         "\"unlock_condition\":\"visit_location\",\"unlock_ref\":\"loc_b\"," +
                         "\"body\":\"A sufficiently long body of authored prose.\",\"tags\":[\"t\",\"t\"]}," +
                         // duplicate id -> skipped
                         "{\"id\":\"codex_b\",\"category\":\"factions\",\"display_name\":\"B2\"," +
                         "\"unlock_condition\":\"meet_faction\",\"unlock_ref\":\"iron_garrison\"," +
                         "\"body\":\"Another sufficiently long body of prose.\"}]}";

            var entries = CodexEntryCatalogLoader.Parse(raw, new SystemTextJsonSerializer());
            Assert.Single(entries);
            Assert.Equal("codex_b", entries[0].id);
            Assert.Equal("regions", entries[0].category);
            // duplicate tags collapse
            Assert.Single(entries[0].tags);
        }

        [Fact]
        public void Loader_UnknownUnlockConditionBecomesUnknownAndNeverUnlocks()
        {
            Assert.Equal(CodexUnlockCondition.Unknown, CodexEntryCatalogLoader.ParseUnlockCondition("dance_under_moon"));
            Assert.Equal(CodexUnlockCondition.Unknown, CodexEntryCatalogLoader.ParseUnlockCondition(null));
            Assert.Equal(CodexUnlockCondition.Unknown, CodexEntryCatalogLoader.ParseUnlockCondition(""));
            Assert.Equal(CodexUnlockCondition.VisitLocation, CodexEntryCatalogLoader.ParseUnlockCondition("visit_location"));
            Assert.Equal(CodexUnlockCondition.MeetFaction, CodexEntryCatalogLoader.ParseUnlockCondition("meet_faction"));
            Assert.Equal(CodexUnlockCondition.FirstCatch, CodexEntryCatalogLoader.ParseUnlockCondition("first_catch"));

            var unknown = Entry("codex_unknown", condition: CodexUnlockCondition.Unknown);
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_test_site");
            var projected = Project(new[] { unknown }, journal);
            Assert.Equal(CodexEntryState.Locked, projected.Single(e => e.EntryId == "codex_unknown").State);
        }

        // ── unlock derivation ─────────────────────────────────────────────────

        [Fact]
        public void Projection_VisitLocationUnlocksFromJournalKnowledge()
        {
            var authored = Entry("codex_loc_site", body: "The site record, recovered in full.");
            var journal = new JournalSystem();

            var locked = Project(new[] { authored }, journal).Single(e => e.EntryId == "codex_loc_site");
            Assert.Equal(CodexEntryState.Locked, locked.State);
            Assert.DoesNotContain("recovered in full", locked.Body);
            Assert.Equal(0, locked.DayLearned);

            journal.UnlockLocationVisited("loc_test_site");
            var known = Project(new[] { authored }, journal, day: 9).Single(e => e.EntryId == "codex_loc_site");
            Assert.Equal(CodexEntryState.Known, known.State);
            Assert.Equal("The site record, recovered in full.", known.Body);
            Assert.Equal(9, known.DayLearned);
            Assert.Contains("loc_test_site", known.RelatedLocationIds);
        }

        [Fact]
        public void Projection_FirstCatchUnlocksFromJournalWildlife()
        {
            var authored = Entry("codex_wl_hare",
                category: CodexEntryCatalogLoader.CategoryWildlife,
                condition: CodexUnlockCondition.FirstCatch,
                unlockRef: "cotton_hare");
            var journal = new JournalSystem();

            Assert.Equal(CodexEntryState.Locked,
                Project(new[] { authored }, journal).Single(e => e.EntryId == "codex_wl_hare").State);

            journal.UnlockWildlifeCaught("cotton_hare");
            var known = Project(new[] { authored }, journal).Single(e => e.EntryId == "codex_wl_hare");
            Assert.Equal(CodexEntryState.Known, known.State);
            Assert.Equal(CodexCategory.Ecology, known.Category);
            // a species unlock is not a location, so no related location is claimed
            Assert.Empty(known.RelatedLocationIds);
        }

        [Fact]
        public void Projection_MeetFactionNeedsAHostResolverAndNeverUnlocksByDefault()
        {
            var authored = Entry("codex_fac_garrison",
                category: CodexEntryCatalogLoader.CategoryFactions,
                condition: CodexUnlockCondition.MeetFaction,
                unlockRef: "iron_garrison");

            // No resolver: must stay locked rather than defaulting to unlocked.
            Assert.Equal(CodexEntryState.Locked,
                Project(new[] { authored }).Single(e => e.EntryId == "codex_fac_garrison").State);

            // A resolver that has not met this faction keeps it locked.
            Assert.Equal(CodexEntryState.Locked,
                Project(new[] { authored }, factionContact: _ => false)
                    .Single(e => e.EntryId == "codex_fac_garrison").State);

            var met = Project(new[] { authored }, factionContact: id => id == "iron_garrison")
                .Single(e => e.EntryId == "codex_fac_garrison");
            Assert.Equal(CodexEntryState.Known, met.State);
            Assert.Equal(CodexCategory.Factions, met.Category);
        }

        [Fact]
        public void Projection_UnrecoverableRecordStillAnnouncesItselfAtTierZero()
        {
            var authored = Entry("codex_loc_site", spoilerTier: 0);
            var locked = Project(new[] { authored }).Single(e => e.EntryId == "codex_loc_site");
            Assert.Equal(CodexEntryState.Locked, locked.State);
            Assert.Equal("Test Record", locked.Title);
            Assert.Contains("has not been recovered", locked.Body);
        }

        // ── spoiler tiers ─────────────────────────────────────────────────────

        [Fact]
        public void Projection_WithholdsUnrecoveredHigherTierRecordsEntirely()
        {
            var tier1 = Entry("codex_lore_t1", spoilerTier: 1, unlockRef: "loc_t1");
            var tier3 = Entry("codex_lore_t3", spoilerTier: 3, unlockRef: "loc_t3");
            var projected = Project(new[] { tier1, tier3 });

            Assert.DoesNotContain(projected, e => e.EntryId == "codex_lore_t1");
            Assert.DoesNotContain(projected, e => e.EntryId == "codex_lore_t3");
            // and nothing leaks their prose or titles
            Assert.DoesNotContain(projected, e => e.Body.Contains("Authored body text"));
        }

        [Fact]
        public void Projection_RecoveredHigherTierRecordsAreShown()
        {
            var tier3 = Entry("codex_lore_t3", spoilerTier: 3, unlockRef: "loc_t3", body: "The late revelation.");
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_t3");

            var known = Project(new[] { tier3 }, journal).Single(e => e.EntryId == "codex_lore_t3");
            Assert.Equal(CodexEntryState.Known, known.State);
            Assert.Equal("The late revelation.", known.Body);
        }

        [Fact]
        public void Projection_MaxSpoilerTierFiltersEvenRecoveredRecords()
        {
            var tier0 = Entry("codex_t0", spoilerTier: 0, unlockRef: "loc_a");
            var tier2 = Entry("codex_t2", spoilerTier: 2, unlockRef: "loc_b");
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_a");
            journal.UnlockLocationVisited("loc_b");

            var filtered = Project(new[] { tier0, tier2 }, journal, maxSpoilerTier: 0);
            Assert.Contains(filtered, e => e.EntryId == "codex_t0");
            Assert.DoesNotContain(filtered, e => e.EntryId == "codex_t2");

            var unfiltered = Project(new[] { tier0, tier2 }, journal);
            Assert.Equal(2, unfiltered.Count(e => e.EntryId == "codex_t0" || e.EntryId == "codex_t2"));
        }

        // ── mapping ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData(CodexEntryCatalogLoader.CategoryRegions, CodexCategory.WastelandLore)]
        [InlineData(CodexEntryCatalogLoader.CategoryLocations, CodexCategory.WastelandLore)]
        [InlineData(CodexEntryCatalogLoader.CategoryDeepLore, CodexCategory.WastelandLore)]
        [InlineData(CodexEntryCatalogLoader.CategoryFactions, CodexCategory.Factions)]
        [InlineData(CodexEntryCatalogLoader.CategoryWildlife, CodexCategory.Ecology)]
        public void Projection_MapsAuthoredCategoryToCodexCategory(string authored, CodexCategory expected)
        {
            var entry = Entry("codex_map_" + authored, category: authored);
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_test_site");
            var projected = Project(new[] { entry }, journal).Single(e => e.EntryId == entry.id);
            Assert.Equal(expected, projected.Category);
        }

        [Theory]
        [InlineData(CodexEntryCatalogLoader.ProvenanceRumor, InformationConfidence.Low, KnowledgeSourceKind.TraderRumor)]
        [InlineData(CodexEntryCatalogLoader.ProvenanceEyewitness, InformationConfidence.Medium, KnowledgeSourceKind.JournalEvidence)]
        [InlineData(CodexEntryCatalogLoader.ProvenanceMaterial, InformationConfidence.High, KnowledgeSourceKind.ExpeditionSurvey)]
        [InlineData(CodexEntryCatalogLoader.ProvenanceRestricted, InformationConfidence.High, KnowledgeSourceKind.Manual)]
        [InlineData(CodexEntryCatalogLoader.ProvenanceCanonical, InformationConfidence.Confirmed, KnowledgeSourceKind.NarrativeArticle)]
        public void Projection_MapsProvenanceToConfidenceAndSourceKind(
            string provenance, InformationConfidence expectedConfidence, KnowledgeSourceKind expectedKind)
        {
            var entry = Entry("codex_prov_" + provenance, provenance: provenance);
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_test_site");

            var projected = Project(new[] { entry }, journal).Single(e => e.EntryId == entry.id);
            Assert.Equal(expectedConfidence, projected.Confidence);
            var prov = Assert.Single(projected.Provenance);
            Assert.Equal(expectedKind, prov.SourceKind);
            Assert.Equal("codex_entries", prov.ProducerSystemId);
            Assert.Equal(entry.id, prov.SourceId);
            Assert.Equal("loc_test_site", prov.RelatedEntityId);
        }

        [Fact]
        public void Projection_TagsCarryAuthoredTagsPlusCategoryAndProvenance()
        {
            var entry = Entry("codex_tags", provenance: CodexEntryCatalogLoader.ProvenanceRumor);
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_test_site");

            var tags = Project(new[] { entry }, journal).Single(e => e.EntryId == "codex_tags").Tags;
            Assert.Contains("test_tag", tags);
            Assert.Contains(CodexEntryCatalogLoader.CategoryLocations, tags);
            Assert.Contains(CodexEntryCatalogLoader.ProvenanceRumor, tags);
            Assert.Equal(tags.Count, tags.Distinct(StringComparer.Ordinal).Count());
        }

        // ── coexistence, compatibility, determinism ───────────────────────────

        [Fact]
        public void Projection_AuthoredIdsDoNotCollideWithOtherSourcePrefixes()
        {
            foreach (var e in LoadAuthoritative())
            {
                Assert.False(e.id.StartsWith("codex_fg_", StringComparison.Ordinal), e.id);
                Assert.False(e.id.StartsWith("codex_tech_", StringComparison.Ordinal), e.id);
                Assert.False(e.id.StartsWith("codex_journal_", StringComparison.Ordinal), e.id);
            }
        }

        [Fact]
        public void Projection_AuthoritativeCatalogProjectsAlongsideJournalSource()
        {
            var authored = LoadAuthoritative();
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_ration_queue_plaza");
            journal.Knowledge.Discover(KnowledgeKeys.ColdCountBeforeTheLab);

            var projected = Project(authored, journal, day: 12);

            // the journal-sourced history record still projects
            Assert.Contains(projected, e => e.EntryId.StartsWith("codex_journal_history_", StringComparison.Ordinal));
            // the authored record for the visited location is recovered in full
            var grid = projected.FirstOrDefault(e => e.EntryId == "codex_regions_the_grid");
            Assert.NotNull(grid);
            Assert.Equal(CodexEntryState.Known, grid!.State);
            Assert.Equal("The Grid", grid.Title);
            Assert.Contains("conscripted", grid.Body);
            Assert.Equal(CodexCategory.WastelandLore, grid.Category);
        }

        [Fact]
        public void Projection_OriginalSignatureIsUnchangedByTheAuthoredSource()
        {
            var journal = new JournalSystem();
            journal.Knowledge.Discover(KnowledgeKeys.HighCo2);

            var entries = CodexProjectionBuilder.Build(null, null, null, journal, currentDay: 3);
            Assert.NotEmpty(entries);
            Assert.DoesNotContain(entries, e => e.EntryId.StartsWith("codex_regions_", StringComparison.Ordinal));
            Assert.Contains(entries, e => e.EntryId.StartsWith("codex_journal_", StringComparison.Ordinal));
        }

        [Fact]
        public void Projection_IsDeterministicAcrossRepeatedBuilds()
        {
            var authored = LoadAuthoritative();
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_ration_queue_plaza");
            journal.UnlockWildlifeCaught("rabbit");

            string Fingerprint() => string.Join("|", Project(authored, journal, day: 4)
                .Select(e => $"{e.EntryId}:{(int)e.State}:{(int)e.Confidence}:{e.Title}"));

            string a = Fingerprint();
            string b = Fingerprint();
            Assert.False(string.IsNullOrEmpty(a));
            Assert.Equal(a, b);
        }

        [Fact]
        public void Projection_EmptyAndNullAuthoredSourcesAreSafe()
        {
            Assert.Empty(Project(Array.Empty<AuthoredCodexEntry>()));

            var journal = new JournalSystem();
            // A tier-0 record that has not been recovered still announces itself as
            // Locked, so an unrecovered catalog is not an empty projection.
            var lockedOnly = Project(new[] { Entry("codex_x") }, journal);
            var locked = Assert.Single(lockedOnly);
            Assert.Equal("codex_x", locked.EntryId);
            Assert.Equal(CodexEntryState.Locked, locked.State);

            // entries with no unlock ref can never be recovered, and must not throw
            var orphan = Entry("codex_orphan", unlockRef: string.Empty);
            Assert.Equal(CodexEntryState.Locked,
                Project(new[] { orphan }).Single(e => e.EntryId == "codex_orphan").State);
        }
    }
}
