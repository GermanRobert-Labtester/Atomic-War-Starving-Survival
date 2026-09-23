// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 49 / C2[21] — content orphan certification host-integration gate.
//
// Pins the production wiring contract:
//   * every manifest family names a real authored catalog file,
//   * every named canonical consumer is a real Core class (or a real catalog
//     authority) — no phantom systems,
//   * the host reads live evidence (loaded catalogs + constructed owners) and
//     never hardcodes a loader name,
//   * the engine's clean / dormant / orphan semantics are the verdict,
//   * the probe and journal surface are registered.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class Plan49ContentCertificationHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static string[] ManifestConsumerNames()
        {
            string host = ReadRepoFile("src", "Host", "ContentCertificationHostSession.cs");
            return host.Split('\n')
                .Select(line => line.Trim())
                .Where(line => line.StartsWith("new ContentCertificationFamily(", StringComparison.Ordinal))
                .Select(line =>
                {
                    // The four constructor arguments are all quoted strings;
                    // the last one is the canonical consumer.
                    string[] tokens = line.Split('"');
                    return tokens.Where((t, i) => i % 2 == 1).Last();
                })
                .ToArray();
        }

        [Fact]
        public void EveryManifestFamily_PointsAtARealAuthoredCatalogFile()
        {
            string host = ReadRepoFile("src", "Host", "ContentCertificationHostSession.cs");
            string dataDir = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

            foreach (string catalog in host.Split('\n')
                .Select(l => l.Trim())
                .Where(l => l.Contains(".json\","))
                .Select(l =>
                {
                    var token = l.Split('"').FirstOrDefault(t => t.EndsWith(".json", StringComparison.Ordinal));
                    return token;
                })
                .Where(t => !string.IsNullOrEmpty(t) && !t.Contains("Path.Combine", StringComparison.Ordinal)))
            {
                Assert.True(File.Exists(Path.Combine(dataDir, catalog!)), $"missing catalog {catalog}");
            }
        }

        [Fact]
        public void EveryNamedCanonicalConsumer_IsARealCoreAuthority()
        {
            string[] consumers = ManifestConsumerNames();
            Assert.Equal(9, consumers.Length);

            string coreRoot = Path.Combine(RepoRoot(), "Assets", "Ashfall.Core");
            foreach (string consumer in consumers)
            {
                bool isCoreClass = Directory.EnumerateFiles(coreRoot, "*.cs", SearchOption.AllDirectories)
                    .Any(f => File.ReadAllText(f).Contains($"class {consumer}", StringComparison.Ordinal));
                Assert.True(isCoreClass, $"consumer '{consumer}' is not a real Core authority");
            }
        }

        [Fact]
        public void HostEvidence_ReadsTheLiveCompositionNotLoaderNames()
        {
            string main = ReadRepoFile("src", "Main.ContentCertification.cs");
            // Each evidence flag reads the live object graph...
            Assert.Contains("_world?.AtmosphereTexts?.Count > 0", main);
            Assert.Contains("_journal?.HasAuthoredCorpus == true", main);
            Assert.Contains("_confessionSecrets != null", main);
            Assert.Contains("_survivorVoice != null", main);
            // ...and orphans are reported, never faked as loaded.
            Assert.Contains("session.MarkConsumerActive(\"CassettePlaybackSystem\", false);", main);
            Assert.Contains("session.MarkCatalogLoaded(\"cassette_sets.json\", false);", main);
        }

        [Fact]
        public void EngineVerdicts_DormantIsNotOrphan()
        {
            // The engine's contract: dormant (unloaded) content is excluded and
            // keeps the report clean; only an uncomposed consumer is an orphan.
            var rows = new[]
            {
                new Ashfall.Core.Content.ContentCandidateRow("loaded_family", "catalog.json", "SomeSystem", true),
                new Ashfall.Core.Content.ContentCandidateRow("dormant_family", "catalog.json", "SomeSystem", false)
            };
            var active = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal) { "SomeSystem" };

            var report = Ashfall.Core.Content.ContentOrphanCertificationEngine.Certify(rows, active);

            Assert.True(report.IsCertificationClean);
            Assert.Equal(1, report.ExcludedDormantCount);
            Assert.Equal(1, report.CertifiedActiveCount);
            Assert.Equal(0, report.OrphanWarningCount);
            Assert.True(report.CanPromotePlan49);
        }

        [Fact]
        public void SetupAndJournalSurface_AreRegistered()
        {
            string expanded = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupContentCertification();", expanded);

            string main = ReadRepoFile("src", "Main.ContentCertification.cs");
            Assert.Contains("content_certification_review", main);

            string cli = ReadRepoFile("src", "Host", "HostCli.cs");
            Assert.Contains("ContentCertificationSelfTest", cli);
            Assert.Contains("--content-certification-selftest", cli);
        }
    }
}
