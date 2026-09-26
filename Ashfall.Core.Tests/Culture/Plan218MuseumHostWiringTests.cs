// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 218 — Shelter Museum & Historical Archive: host wiring and visit-policy
// tests. Core contract: explicit once-per-day visits, exactly-once exhibition
// expiry, save/restore round-trip including the visit ledger, and the
// production wiring gate. Physical-inventory donation stays unexposed.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class Plan218MuseumHostWiringTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.ShelterMuseum.cs")))
                        return directory.FullName;
                    directory = directory.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        private static string DataDirectory =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static ShelterMuseumSystem CreateCatalogLoadedSystem()
        {
            var system = new ShelterMuseumSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "museum_collection_templates.json")));
            return system;
        }

        // ── Core contract: once-per-day visit ledger ──

        [Fact]
        public void TryVisitMuseum_AllowsOneVisitPerDay_AndRefusesSameDayRepeat()
        {
            var museum = CreateCatalogLoadedSystem();

            bool first = museum.TryVisitMuseum("surv_a", currentDay: 4, out float moraleFirst);
            Assert.True(first);
            Assert.InRange(moraleFirst, 1f, 25f);
            Assert.Equal(1, museum.TotalVisitors);

            bool repeat = museum.TryVisitMuseum("surv_a", currentDay: 4, out float moraleRepeat);
            Assert.False(repeat);
            Assert.Equal(0f, moraleRepeat);
            Assert.Equal(1, museum.TotalVisitors);

            bool nextDay = museum.TryVisitMuseum("surv_a", currentDay: 5, out _);
            Assert.True(nextDay);
            Assert.Equal(2, museum.TotalVisitors);
        }

        [Fact]
        public void TryVisitMuseum_RefusesEmptyVisitor_AndUnknownVisitorStillCounts()
        {
            var museum = CreateCatalogLoadedSystem();
            Assert.False(museum.TryVisitMuseum("", currentDay: 1, out _));
            Assert.Equal(0, museum.TotalVisitors);
        }

        // ── Exhibition expiry: exactly once ──

        [Fact]
        public void TickDay_ClosesExpiredExhibition_ExactlyOnce()
        {
            var museum = CreateCatalogLoadedSystem();
            var artifact = museum.DonateFromTemplate("artifact_founding_charter", "surv_curator", currentDay: 1);
            Assert.NotNull(artifact);

            int closedCount = 0;
            museum.OnExhibitionClosed += _ => closedCount++;

            museum.CurateExhibition("Founding", ExhibitionTheme.Founding,
                new[] { artifact!.ArtifactId }, startDay: 1, durationDays: 2, description: "d");

            museum.TickDay(3); // closes (1 + 2 = 3)
            museum.TickDay(4); // no further closure
            museum.TickDay(5); // no further closure

            Assert.Equal(1, closedCount);
            Assert.Equal(0, museum.GetActiveExhibitions().Count);
        }

        // ── Persistence: full state + visit ledger ──

        [Fact]
        public void SaveRestore_PreservesCollectionCuratorVisitsEventsAndLedger()
        {
            var museum = CreateCatalogLoadedSystem();
            museum.AppointCurator("surv_curator", currentDay: 1);
            var artifact = museum.DonateFromTemplate("artifact_pioneer_geiger_counter", "surv_curator", currentDay: 2);
            museum.CurateExhibition("Radiation Pioneers", ExhibitionTheme.Memorial,
                new[] { artifact!.ArtifactId }, startDay: 2, durationDays: 5, description: "d");
            museum.TryVisitMuseum("surv_a", currentDay: 3, out _);
            museum.TryVisitMuseum("surv_b", currentDay: 3, out _);

            var state = museum.CaptureState();

            var restored = new ShelterMuseumSystem();
            restored.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "museum_collection_templates.json")));
            restored.RestoreState(state);

            Assert.Equal("surv_curator", restored.CuratorId);
            Assert.Equal(1, restored.TotalArtifactCount);
            Assert.Equal(2, restored.TotalVisitors);
            Assert.Equal(1, restored.GetActiveExhibitions().Count);
            Assert.True(restored.GetEvents().Any(e => e.EventType == "curator_appointed"));

            // The visit ledger survives: day-3 visits cannot be recorded again.
            Assert.False(restored.TryVisitMuseum("surv_a", currentDay: 3, out _));
            Assert.True(restored.TryVisitMuseum("surv_a", currentDay: 4, out _));
        }

        // ── Production wiring gate (source-text evidence) ──

        [Fact]
        public void HostWiring_BindsDailyTickSaveLifecycleDispatchRegistryAndPanel()
        {
            string mainMuseum = Source("src/Main.ShelterMuseum.cs");
            string orchestrator = Source("src/Main.SaveOrchestrator.cs");
            string lifecycle = Source("src/Main.Lifecycle.cs");
            string application = Source("src/Main.Application.cs");
            string registry = Source("Assets/Ashfall.Core/HostCliRegistry.cs");
            string sections = Source("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs");
            string hostSession = Source("src/Host/ShelterMuseumHostSession.cs");
            string cliProbe = Source("src/Host/HostCli.ShelterMuseum.cs");
            string dailyOrchestration = Source("src/Main.Plans46_49.cs");
            string panel = Source("src/UI/ArchiveDeskPanel.cs");
            string bind = Source("src/Main.ShelterBatch3.cs");

            // Morale through the canonical Needs owner, exactly once per visit.
            Assert.Contains("OnMuseumVisited", mainMuseum);
            Assert.Contains("NeedKind.Morale", mainMuseum);

            // Save custody through the existing orchestrator seam; own save key.
            Assert.Contains("SetupShelterMuseum();", orchestrator);
            Assert.Contains("SaveShelterMuseum();", orchestrator);
            Assert.Contains("ResetShelterMuseum();", lifecycle);
            Assert.Contains("SaveStoreHub.Checksummed<ShelterMuseumState>", hostSession);
            Assert.Contains("SectionName = \"shelter_museum\"", hostSession);

            // Daily expiry rides the existing world/day orchestration.
            Assert.Contains("TickShelterMuseum(day);", dailyOrchestration);

            // CLI probe registration and dispatch.
            Assert.Contains("ShelterMuseumSelfTest", registry);
            Assert.Contains("case HostCliAction.ShelterMuseumSelfTest:", application);
            Assert.Contains("HostCliShelterMuseum.RunSelfTest", application);

            // Save section registration.
            Assert.Contains("new(\"shelter_museum\", \"SaveShelterMuseum\", \"SetupShelterMuseum\"", sections);

            // Read-only projection + explicit visit command through archive desk.
            Assert.Contains("MuseumProvider", panel);
            Assert.Contains("OnMuseumVisitPressed", panel);
            Assert.Contains("_archiveDeskPanel.MuseumProvider = () => GetShelterMuseumSnapshot();", bind);
            Assert.Contains("_archiveDeskPanel.MuseumVisitCommand = id => VisitShelterMuseum(id);", bind);
        }

        [Fact]
        public void PhysicalInventoryDonation_IsNotExposedToPlayers()
        {
            // The player surface must not enable donation from physical
            // inventory until a transaction-safe custody bridge is signed.
            string panel = Source("src/UI/ArchiveDeskPanel.cs");
            string bind = Source("src/Main.ShelterBatch3.cs");
            Assert.DoesNotContain("DonateArtifact", panel);
            Assert.DoesNotContain("DonateArtifact", bind);
            Assert.DoesNotContain("DonateFromTemplate", panel);
            Assert.DoesNotContain("DonateFromTemplate", bind);

            // Host session exposes only the nonphysical template accession.
            string hostSession = Source("src/Host/ShelterMuseumHostSession.cs");
            Assert.DoesNotContain("public MuseumArtifact DonateArtifact", hostSession);
        }
    }
}
