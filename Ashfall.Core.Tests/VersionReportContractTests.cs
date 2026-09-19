// SPDX-License-Identifier: MIT
// ASHFALL gate: the host `--version` flag renders the pinned VersionReport
// contract — build/game version line, live data-authority schema summary,
// and save-codec schema versions sourced from the actual CurrentSaveVersion
// constants. If the output shape or a codec version regresses, these fail.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VersionReportContractTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new FileNotFoundException(
                "Could not locate the repository root (Assets/StreamingAssets/Data) from the test run");
        }

        // ── Output shape contract ────────────────────────────────────────

        [Fact]
        public void Compose_RendersPinnedHeaderAndSectionLines()
        {
            string report = VersionReport.Compose("9.9.9-test", dataDir: null);

            Assert.StartsWith("ASHFALL version report", report, StringComparison.Ordinal);
            Assert.Contains("game         : 9.9.9-test", report);
            Assert.Contains("data schemas : no data directory", report);
            Assert.Contains("save schemas : ", report);
        }

        [Fact]
        public void Compose_ContainsEverySaveStoreVersion()
        {
            string report = VersionReport.Compose("1.0.0", dataDir: null);

            foreach (var entry in VersionReport.SaveSchemaVersions)
            {
                Assert.Contains($"{entry.Store} v{entry.CurrentVersion}", report);
            }
        }

        [Fact]
        public void SaveSchemaVersions_ListsEveryVersionedSaveCodec()
        {
            // The report must cover every versioned save codec constant in
            // Core. When a new codec with CurrentSaveVersion ships, add it
            // here (and to VersionReport.SaveSchemaVersions).
            var stores = VersionReport.SaveSchemaVersions.Select(s => s.Store).OrderBy(s => s).ToArray();

            Assert.Equal(
                new[] { "dose_ledger", "expansion_hub", "expansion_quest", "holdfast", "weight_of_choices", "year_of_ash" },
                stores);
        }

        [Fact]
        public void SaveSchemaVersions_MatchTheActualCodecConstants()
        {
            // Values are compiled from the codec constants themselves; this
            // pins the mapping so a renamed/moved constant breaks loudly
            // instead of silently reporting a stale number.
            Assert.Contains(VersionReport.SaveSchemaVersions,
                s => s.Store == "holdfast" && s.CurrentVersion == HoldfastSave.CurrentSaveVersion);
            Assert.Contains(VersionReport.SaveSchemaVersions,
                s => s.Store == "year_of_ash" && s.CurrentVersion == YearOfAsh.YearOfAshSave.CurrentSaveVersion);
            Assert.Contains(VersionReport.SaveSchemaVersions,
                s => s.Store == "dose_ledger" && s.CurrentVersion == DoseLedgerSave.CurrentSaveVersion);
            Assert.Contains(VersionReport.SaveSchemaVersions,
                s => s.Store == "expansion_hub" && s.CurrentVersion == ExpansionHubSave.CurrentSaveVersion);
            Assert.Contains(VersionReport.SaveSchemaVersions,
                s => s.Store == "expansion_quest" && s.CurrentVersion == ExpansionQuestSaveEnvelope.CurrentVersion);
        }

        // ── Data schema scanner contract ─────────────────────────────────

        [Fact]
        public void ScanDataSchemas_TalliesVersionsAndMissingDeclarations()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall-version-tests",
                Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllText(Path.Combine(dir, "a.json"), "{\"schema_version\": 1, \"items\": []}");
                File.WriteAllText(Path.Combine(dir, "b.json"), "{\"schema_version\": 2, \"items\": []}");
                File.WriteAllText(Path.Combine(dir, "c.json"), "{\"items\": []}");
                // Nested schema_version must lose to a top-level declaration.
                File.WriteAllText(Path.Combine(dir, "d.json"),
                    "{\"schema_version\": 1, \"items\": [{\"schema_version\": 9}]}");
                // Unreadable/unparseable JSON still counts as a catalog
                // (inventory semantics); it simply has no schema_version → v0.
                File.WriteAllText(Path.Combine(dir, "broken.json"), "{not json");
                Directory.CreateDirectory(Path.Combine(dir, "sub"));
                File.WriteAllText(Path.Combine(dir, "sub", "e.json"), "{\"schema_version\": 2}");

                var summary = VersionReport.ScanDataSchemas(dir);

                Assert.Equal(6, summary.Catalogs);          // inventory: every readable *.json
                Assert.Equal(4, summary.WithSchemaVersion); // c.json & broken.json declare none
                Assert.Equal(2, summary.WithoutSchemaVersion);
                Assert.Equal(2, summary.MaxVersion);
                Assert.Equal(new[] { (0, 2), (1, 2), (2, 2) },
                    summary.Distribution.Select(d => (d.Version, d.Files)).ToArray());
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        [Fact]
        public void ScanDataSchemas_MissingDirectory_ReturnsEmptySummary()
        {
            var summary = VersionReport.ScanDataSchemas(Path.Combine(Path.GetTempPath(), "definitely-not-here"));
            Assert.Equal(0, summary.Catalogs);
            Assert.Equal(0, summary.WithSchemaVersion);
            Assert.Equal(0, summary.MaxVersion);
            Assert.Empty(summary.Distribution);
            Assert.Equal("no data directory", VersionReport.FormatDataSchemas(summary));
        }

        [Fact]
        public void FormatDataSchemas_RendersDistributionWithMax()
        {
            var summary = new VersionReport.DataSchemaSummary
            {
                Catalogs = 3,
                MaxVersion = 2,
                Distribution = { (1, 1), (2, 2) }
            };
            Assert.Equal("3 catalogs — v1: 1, v2: 2 (max v2)", VersionReport.FormatDataSchemas(summary));
        }

        // ── Live data authority pin ──────────────────────────────────────

        [Fact]
        public void ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent()
        {
            string dataDir = Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");
            var summary = VersionReport.ScanDataSchemas(dataDir);

            // The data authority is large and overwhelmingly versioned; both
            // facts must hold or the report is scanning the wrong place.
            Assert.True(summary.Catalogs >= 100,
                $"Expected at least 100 JSON catalogs under the data authority, found {summary.Catalogs}");
            Assert.True(summary.WithSchemaVersion >= 100,
                $"Expected at least 100 catalogs with schema_version, found {summary.WithSchemaVersion}");
            Assert.True(summary.MaxVersion >= 1, "Expected at least schema_version 1 in the data authority");
        }

        // ── Persistence format inventory contract ─────────────────────────

        [Fact]
        public void AllPersistenceFormats_CoversAllSaveSectionRegistrySections()
        {
            var registryKeys = global::Ashfall.Core.Save.SaveSectionRegistry.All.Select(s => s.SectionKey).OrderBy(k => k).ToArray();
            var inventoryKeys = VersionReport.AllPersistenceFormats.Select(f => f.SectionKey).OrderBy(k => k).ToArray();

            Assert.Equal(registryKeys, inventoryKeys);
        }

        [Fact]
        public void AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes()
        {
            var versioned = VersionReport.AllPersistenceFormats.Where(f => f.Kind == SavePersistenceKind.VersionedCodec).ToList();
            var envelopes = VersionReport.AllPersistenceFormats.Where(f => f.Kind == SavePersistenceKind.ChecksumEnvelope).ToList();

            // 6 versioned Core codecs
            Assert.Equal(6, versioned.Count);
            Assert.Contains(versioned, f => f.SectionKey == "holdfast" && f.Version == HoldfastSave.CurrentSaveVersion);
            Assert.Contains(versioned, f => f.SectionKey == "year_of_ash" && f.Version == YearOfAsh.YearOfAshSave.CurrentSaveVersion);
            Assert.Contains(versioned, f => f.SectionKey == "dose_ledger" && f.Version == DoseLedgerSave.CurrentSaveVersion);
            Assert.Contains(versioned, f => f.SectionKey == "expansion_hub" && f.Version == ExpansionHubSave.CurrentSaveVersion);
            Assert.Contains(versioned, f => f.SectionKey == "expansion_quest" && f.Version == ExpansionQuestSaveEnvelope.CurrentVersion);
            Assert.Contains(versioned, f => f.SectionKey == "weight_of_choices" && f.Version == Ashfall.Core.Factions.WeightOfChoicesSave.CurrentSaveVersion);

            // 166 unversioned checksum envelopes, including the Plans 130–133
            // state sections (powder_metallurgy, nvis_communications, lyophilization, draisine_recovery),
            // Tasks 5–8 state sections (weather_hardening, geothermal_aquifer, counter_intelligence, recon_telemetry),
            // Plans 146–149 state sections (route_infrastructure, ebpvd_coating, microfluidic_diagnostic, mine_clearing_flail, rail_grinding),
            // Plans 166–169 state sections (espionage, fluid_logistics, procedural_narrative),
            // Plans 62–65 state sections (food_preservation, prewar_archives, shelter_prisoners),
            // Plans 50–53 state sections (vehicle_garage, faction_espionage, survivor_mental_health),
            // Plans B68–B69 state sections (seismic_dynamics, cryo_vault),
            // Plans B86–B89 state sections (precision_metrology, aquaponics; B88 nests under radio),
            // and the muster-warfare / plans-74-77 state sections from the concurrent flagship streams.
            // Plan 155 added oral_lore.
            // Plan 157 added grain_milling_archive.
            // Plan 159 added leatherwork_archive.
            // Plan 143 added narrative_questlines.
            // B5–B8 Phase 6 added deep_well (pins were already stale at 186/180
            // before this package — live registry was 190; bumped to the true count).
            // D1 drift rematch 2026-09-17: the live registry advanced by one
            // further unversioned checksum section since that pin (VersionReport
            // is the authority; pins track its current output).
            // 2026-09-20: Plan 220/205 +2 checksum envelopes (188 → 190).
            Assert.Equal(190, envelopes.Count);
            foreach (var envelope in envelopes)
            {
                Assert.Null(envelope.Version);
                Assert.Contains("envelope", envelope.FormatDescription);
            }
        }

        [Fact]
        public void FormatPersistenceInventory_RendersSummaryAndEntries()
        {
            string inventory = VersionReport.FormatPersistenceInventory();

            Assert.Contains("Save Persistence Inventory (196 sections: 6 versioned codecs, 190 checksum envelopes):", inventory);
            Assert.Contains("holdfast", inventory);
            Assert.Contains("dose_ledger", inventory);
            Assert.Contains("journal", inventory);
            Assert.Contains("survivors", inventory);
            Assert.Contains("weight_of_choices", inventory);
        }
    }
}
