// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;
using Xunit;

namespace Ashfall.Core.Tests.Research
{
    public sealed class PrewarArchiveDecryptionTests
    {
        private static PrewarArchiveCatalog CreateMockCatalog()
        {
            var catalog = new PrewarArchiveCatalog
            {
                archives = new List<PrewarArchiveDef>
                {
                    new PrewarArchiveDef
                    {
                        id = "archive_orbital_telemetry_array",
                        display_name = "Orbital Telemetry",
                        cleaning_solvent_id = "chemicals",
                        cleaning_solvent_count = 2,
                        base_effort_points = 30.0f,
                        reward_research_ids = new List<string> { "knowledge_encrypted_radio_blueprint" }
                    }
                }
            };
            catalog.Index();
            return catalog;
        }

        [Fact]
        public void DiscoverArchive_AddsProject()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new PrewarArchiveDecryptionSystem(rng, inv, catalog);

            var res = system.DiscoverArchive("archive_orbital_telemetry_array", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            var proj = system.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj);
            Assert.Equal(ArchiveDecryptionStatus.Discovered, proj.Status);
            Assert.Equal(30.0f, proj.TargetProgress);
        }

        [Fact]
        public void StabilizeArchive_ConsumesSolvents()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new PrewarArchiveDecryptionSystem(rng, inv, catalog);

            system.DiscoverArchive("archive_orbital_telemetry_array", 1);

            // Fail without solvent
            var failRes = system.StabilizeArchive("archive_orbital_telemetry_array", "survivor_lead", 1);
            Assert.Equal(ActionResult.StatusKind.Blocked, failRes.Status);

            // Add solvent and retry
            inv.AddById("chemicals", 2);
            var okRes = system.StabilizeArchive("archive_orbital_telemetry_array", "survivor_lead", 1);
            Assert.Equal(ActionResult.StatusKind.Success, okRes.Status);
            Assert.Equal(0, inv.CountById("chemicals"));

            var proj = system.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj);
            Assert.Equal(ArchiveDecryptionStatus.Stabilized, proj.Status);
            Assert.True(proj.HasSolventApplied);
        }

        [Fact]
        public void TickDay_AdvancesProgress_AndCompletes()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new PrewarArchiveDecryptionSystem(rng, inv, catalog);

            inv.AddById("chemicals", 2);
            system.DiscoverArchive("archive_orbital_telemetry_array", 1);
            system.StabilizeArchive("archive_orbital_telemetry_array", "survivor_lead", 1);
            system.StartDecryption("archive_orbital_telemetry_array", "survivor_lead", 1);

            bool completedFired = false;
            system.OnArchiveDecrypted += (p, def) => completedFired = true;

            // Target effort = 30.0f, base progress = 15.0f/day. Should finish in <= 2 days.
            system.TickDay(2);
            var proj = system.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj);
            Assert.True(proj.Progress >= 15.0f);

            system.TickDay(3);
            Assert.Equal(ArchiveDecryptionStatus.Completed, proj.Status);
            Assert.True(completedFired);
            Assert.Equal(1, system.TotalDecrypted);
        }

        [Fact]
        public void TickDay_PowerOutage_HaltsProgress()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            var rng = new SeededRng(12345);
            var system = new PrewarArchiveDecryptionSystem(rng, inv, catalog);

            inv.AddById("chemicals", 2);
            system.DiscoverArchive("archive_orbital_telemetry_array", 1);
            system.StabilizeArchive("archive_orbital_telemetry_array", "survivor_lead", 1);
            system.StartDecryption("archive_orbital_telemetry_array", "survivor_lead", 1);

            system.SetPowerStatus(false);
            system.TickDay(2);

            var proj = system.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj);
            Assert.Equal(0f, proj.Progress);
        }

        [Fact]
        public void DeterministicReplay_ProducesIdenticalBreakthroughs()
        {
            var catalog = CreateMockCatalog();
            var inv1 = new Inventory.Inventory();
            var inv2 = new Inventory.Inventory();
            inv1.AddById("chemicals", 5);
            inv2.AddById("chemicals", 5);

            var sys1 = new PrewarArchiveDecryptionSystem(new SeededRng(99999), inv1, catalog);
            var sys2 = new PrewarArchiveDecryptionSystem(new SeededRng(99999), inv2, catalog);

            sys1.DiscoverArchive("archive_orbital_telemetry_array", 1);
            sys1.StabilizeArchive("archive_orbital_telemetry_array", "surv_1", 1);
            sys1.StartDecryption("archive_orbital_telemetry_array", "surv_1", 1);

            sys2.DiscoverArchive("archive_orbital_telemetry_array", 1);
            sys2.StabilizeArchive("archive_orbital_telemetry_array", "surv_1", 1);
            sys2.StartDecryption("archive_orbital_telemetry_array", "surv_1", 1);

            sys1.TickDay(2);
            sys2.TickDay(2);

            Assert.Equal(sys1.GetProject("archive_orbital_telemetry_array")!.Progress,
                         sys2.GetProject("archive_orbital_telemetry_array")!.Progress);
        }

        [Fact]
        public void SaveRestore_PreservesAllState()
        {
            var catalog = CreateMockCatalog();
            var inv = new Inventory.Inventory();
            inv.AddById("chemicals", 2);
            var rng = new SeededRng(12345);
            var system = new PrewarArchiveDecryptionSystem(rng, inv, catalog);

            system.DiscoverArchive("archive_orbital_telemetry_array", 1);
            system.StabilizeArchive("archive_orbital_telemetry_array", "surv_1", 1);
            system.StartDecryption("archive_orbital_telemetry_array", "surv_1", 1);
            system.TickDay(2);

            var state = system.CaptureState();

            var system2 = new PrewarArchiveDecryptionSystem(rng, inv, catalog);
            system2.RestoreState(state);

            var proj2 = system2.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj2);
            Assert.Equal(ArchiveDecryptionStatus.Decrypting, proj2.Status);
            Assert.True(proj2.Progress > 0f);
        }
    }
}
