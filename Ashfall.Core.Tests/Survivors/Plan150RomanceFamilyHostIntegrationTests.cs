// SPDX-License-Identifier: MIT
// Plan 150 host integration tests: verifies the Core romance/family authority's
// validated-catalog seam, census contract, schema-gated restore, the
// cohabitation round-trip, and the save-section / day-event wiring the host
// depends on.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan150RomanceFamilyHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static RomanceCourtshipCatalog BoundCatalog()
        {
            var loaded = RomanceCourtshipCatalogLoader.Load(DataDir(), new FileSystemIO());
            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));

            var catalog = new RomanceCourtshipCatalog();
            catalog.BindValidatedEvents(loaded.Events);
            return catalog;
        }

        // ── 1 — the validated bind seam is the only path the host uses ──────

        [Fact]
        public void BindValidatedEvents_ReplacesLenientState_AndKeepsAuthoredRows()
        {
            // Seed the lenient parser's notion of the table first.
            var catalog = RomanceCourtshipCatalog.LoadFromJson(@"{ ""courtship_events"": [ { ""event_id"": ""lenient_row"", ""name"": ""L"", ""score_gain"": 1 } ] }");
            Assert.Single(catalog.CourtshipEvents);

            var loaded = RomanceCourtshipCatalogLoader.Load(DataDir(), new FileSystemIO());
            catalog.BindValidatedEvents(loaded.Events);

            Assert.True(catalog.CourtshipEvents.Count >= 20);
            Assert.DoesNotContain(catalog.CourtshipEvents, e => e.EventId == "lenient_row");
        }

        [Fact]
        public void AuthoredCatalog_IsStrict_AndEveryRowIsReachable()
        {
            var loaded = RomanceCourtshipCatalogLoader.Load(DataDir(), new FileSystemIO());

            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));
            Assert.True(loaded.Events.Count >= 20);

            var system = new RomanceFamilySystem(BoundCatalog());
            Assert.Equal(loaded.Events.Count, system.Catalog.CourtshipEvents.Count);
        }

        // ── 2 — census contract ─────────────────────────────────────────────

        [Fact]
        public void Census_ReportsLiveRelationshipsFamiliesAndCatalogSize()
        {
            var system = new RomanceFamilySystem(BoundCatalog());
            var rng = new SeededRng(1501);

            Assert.True(system.TryInitiateAttraction(
                "s_a", "s_b", 80f, 30, 31, "militarism", "militarism", false, rng, currentDay: 2, force: true));

            // Push to bonded.
            int guard = 0;
            while ((system.GetRelationship("s_a", "s_b")?.RomanceScore ?? 0) < 75 && guard++ < 60)
            {
                system.ConductCourtshipEvent("s_a", "s_b", "deep_conversation", 90f, rng, currentDay: 3 + guard, forceSuccess: true);
            }

            var family = system.FormFamilyUnit("s_a", "s_b", "Household A-B");
            system.AddChildToFamily(family.FamilyId, "s_child", isAdopted: true);

            var census = system.GetCensus();

            Assert.Equal(1, census.TotalRelationships);
            Assert.Equal(1, census.BondedCount);
            Assert.Equal(1, census.TotalFamilies);
            Assert.Equal(1, census.TotalChildren);
            Assert.True(census.LoadedCourtshipEvents >= 20);
        }

        // ── 3 — schema gating ───────────────────────────────────────────────

        [Fact]
        public void RestoreState_RejectsAnUnknownSchemaVersion()
        {
            var system = new RomanceFamilySystem(BoundCatalog());

            var ex = Assert.Throws<InvalidOperationException>(
                () => system.RestoreState(@"{""schema_version"":99,""relationships"":[],""families"":[]}"));

            Assert.Contains("schema_version", ex.Message);
        }

        [Fact]
        public void RestoreState_AcceptsALegacyPayloadWithoutASchemaVersion()
        {
            // Pre-integration saves had no schema_version field; they must still load.
            var system = new RomanceFamilySystem(BoundCatalog());

            system.RestoreState(@"{""relationships"":[{""a"":""s_a"",""b"":""s_b"",""stage"":2,""score"":55}],""families"":[]}");

            var rel = system.GetRelationship("s_a", "s_b");
            Assert.NotNull(rel);
            Assert.Equal(RomanceStage.Partnership, rel!.Stage);
        }

        // ── 4 — the cohabitation round-trip defect ──────────────────────────

        [Fact]
        public void CohabitationQuarters_SurviveACaptureRestoreRoundTrip()
        {
            var system = new RomanceFamilySystem(BoundCatalog());
            var rng = new SeededRng(1502);
            system.TryInitiateAttraction("s_a", "s_b", 80f, 30, 31, "militarism", "militarism", false, rng, 2, force: true);

            var live = system.GetRelationship("s_a", "s_b")!;
            live.CohabitationQuarters = "quarters_east_bunk";

            string captured = system.CaptureState();

            var restored = new RomanceFamilySystem(BoundCatalog());
            restored.RestoreState(captured);

            // CaptureState always wrote cohab; before the fix RestoreState never
            // read it back, so the assignment was silently dropped on every load.
            Assert.Equal("quarters_east_bunk", restored.GetRelationship("s_a", "s_b")!.CohabitationQuarters);
        }

        [Fact]
        public void CaptureRestore_IsAFullRoundTrip()
        {
            var system = new RomanceFamilySystem(BoundCatalog());
            var rng = new SeededRng(1503);
            system.TryInitiateAttraction("s_a", "s_b", 80f, 30, 31, "militarism", "militarism", false, rng, 2, force: true);
            var family = system.FormFamilyUnit("s_a", "s_b", "Household A-B");
            system.AddChildToFamily(family.FamilyId, "s_child", isAdopted: true);

            var restored = new RomanceFamilySystem(BoundCatalog());
            restored.RestoreState(system.CaptureState());

            Assert.Equal(system.Relationships.Count, restored.Relationships.Count);
            Assert.Equal(system.FamilyUnits.Count, restored.FamilyUnits.Count);
            Assert.Equal(system.GetCensus().TotalChildren, restored.GetCensus().TotalChildren);
        }

        [Fact]
        public void CaptureRestore_IsDeterministic()
        {
            var system = new RomanceFamilySystem(BoundCatalog());
            var rng = new SeededRng(1504);
            system.TryInitiateAttraction("s_a", "s_b", 80f, 30, 31, "militarism", "militarism", false, rng, 2, force: true);
            system.FormFamilyUnit("s_a", "s_b", "Household A-B");

            Assert.Equal(system.CaptureState(), system.CaptureState());
        }

        // ── 5 — host wiring the save orchestrator depends on ────────────────

        [Fact]
        public void SaveSectionRegistry_DeclaresRomanceFamily()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("romance_family", out _));
            Assert.Equal("romance_family_save.json", SaveSectionRegistry.FileNameFor("romance_family"));
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesRomanceFamilyTickAsAnInternalHeartbeat()
        {
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("romance_family_ticked"));
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("romance_family_ticked"));
        }
    }
}
