// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 217 — Survivor Genealogy host wiring: kinship facts only, exactly-once
// lineage, no second family-unit ledger, restore parity, production gate.
// ============================================================================
using System;
using System.Linq;
using System.IO;
using Ashfall.Core.Legacy;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan217GenealogyHostWiringTests
    {
        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Main.Genealogy.cs")))
                    return directory.FullName;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        [Fact]
        public void CoreLineage_RecordsExactlyOnceKinship_NoFamilyUnitOnHostFacts()
        {
            var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());

            Assert.True(lineage.SetSpouse("a", "b"));
            Assert.Equal("b", lineage.GetSpouse("a"));

            var first = lineage.EstablishLineage("a", "child", "parent");
            var replay = lineage.EstablishLineage("a", "child", "parent");

            Assert.Equal(ActionResult.StatusKind.Success, first.Status);
            Assert.NotEqual(ActionResult.StatusKind.Success, replay.Status);
            Assert.Single(lineage.LineageRecords.Where(l => l.childId == "child" && !string.IsNullOrWhiteSpace(l.parentId)));

            lineage.RecordFamilyEvent("death", 4, new[] { "a" }, "a died.", "major");
            Assert.Single(lineage.FamilyEventLog.Where(e => e.eventType == "death"));
        }

        [Fact]
        public void CoreLineage_CaptureRestore_RoundTripsKinshipFacts()
        {
            var lineage = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            lineage.SetSpouse("a", "b");
            lineage.EstablishLineage("a", "child", "parent");
            var saved = lineage.CaptureState();

            var restored = new GenerationalLineageExtension(new GenerationalSuccessionEngine());
            restored.RestoreState(saved);

            Assert.Equal("b", restored.GetSpouse("a"));
            Assert.NotEqual(ActionResult.StatusKind.Success, restored.EstablishLineage("a", "child", "parent").Status);
        }

        [Fact]
        public void HostWiring_SubscribesCanonicalFacts_RegistersSaveSectionAndProbe()
        {
            string main = Source("src/Main.Genealogy.cs");
            string host = Source("src/Host/GenealogyHostSession.cs");
            string registry = Source("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs");
            string cli = Source("Assets/Ashfall.Core/HostCliRegistry.cs");
            string panel = Source("src/UI/SurvivorDetailPanel.cs");

            // Canonical producers only — no inference from names.
            Assert.Contains("OnFamilyUnitEstablishedSeam", main);
            Assert.Contains("OnChildWelcomedToFamilySeam", main);
            Assert.Contains("_survivorFate.OnSurvivorFate", main);
            Assert.DoesNotContain("GenerateFamilyName", main);

            // The union route must NOT go through the bridge's unit-forming path.
            Assert.Contains("_lineage.SetSpouse(parentA, parentB);", host);
            Assert.DoesNotContain("_bridge.OnMarriage(", host);

            // Own checksummed save key; save-section registration; probe.
            Assert.Contains("SaveStoreHub.Checksummed<GenealogySaveState>", host);
            Assert.Contains("new(\"genealogy\", \"SaveGenealogy\", \"SetupGenealogy\"", registry);
            Assert.Contains("GenealogySelfTest", cli);

            // Read-only survivor-detail projection.
            Assert.Contains("KinshipProvider", panel);
            Assert.Contains("Lineage: {kinship}", panel);
        }
    }
}
