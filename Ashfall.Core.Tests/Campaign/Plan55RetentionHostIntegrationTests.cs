// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 55 — retention & save-budgeting host-integration gate.
//
// Pins the production wiring contract:
//   * the authored retention policy table loads through the strict loader,
//   * the overlay reaches the live catalog and preserves defaults,
//   * retention is applied by the OWNER (no mirrored collection),
//   * protected obligations can never be pruned,
//   * the campaign tick, save section, journal fact, and CLI probe exist.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Records;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan55RetentionHostIntegrationTests
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

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void AuthoredPolicyTable_ReachesTheCatalogAndPreservesDefaults()
        {
            var catalog = new RetentionPolicyCatalog();
            int defaults = catalog.ActivePolicies.Count;

            var loaded = RetentionPolicyCatalogLoader.Load(DataDir(), new Ashfall.Core.FileSystemIO());
            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));

            int applied = catalog.ApplyOverlay(loaded.Policies);
            Assert.True(applied > 0);
            Assert.True(catalog.ActivePolicies.Count >= defaults, "an authored overlay must never drop a default bound");
            Assert.True(catalog.HasPolicy("survivor_wills_and_legacies"));
        }

        [Fact]
        public void EveryCanonicalOwner_AppliesRetentionToItsOwnCollection()
        {
            var catalog = new RetentionPolicyCatalog();

            // Kitchen serving log.
            var kitchen = new Ashfall.Core.KitchenNutritionSystem(
                new Ashfall.Core.SeededRng(7),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.Survivors.NeedsSystem(),
                new Ashfall.Core.NullLog());
            for (int i = 0; i < 260; i++)
                kitchen.State.servingLog.Add(new Ashfall.Core.MealServingLog { day = i });
            Assert.Equal(60, kitchen.ApplyRetention(catalog));
            Assert.Equal(200, kitchen.State.servingLog.Count);

            // Faction-war decree history.
            var factionWar = new FactionWarSystem();
            for (int i = 0; i < 140; i++) factionWar.State.enactedDecrees.Add($"decree_{i:D4}");
            Assert.Equal(40, factionWar.ApplyRetention(catalog));
            Assert.Equal(100, factionWar.State.enactedDecrees.Count);

            // Machine log.
            var machineLog = new MachineLogSystem();
            for (int i = 0; i < 340; i++)
                machineLog.Post("test_bay", i, "operating", "probe", "evidence");
            Assert.Equal(40, machineLog.ApplyRetention(catalog));
            Assert.Equal(300, machineLog.Entries.Count);

            // Dose ledger: cumulative dose is owned and untouched.
            var dose = new Ashfall.Core.DoseLedgerSystem();
            dose.AssignDosimeter("probe_dosimeter", "tag_01");
            var doseRng = new Ashfall.Core.SeededRng(11);
            for (int i = 0; i < 520; i++)
                dose.BookReading("probe_dosimeter", i, 0.1f, "probe_flux",
                    highEnergyEvent: false, antiRadBefore: false, antiRadAfter: false, doseRng);
            var entry = dose.GetEntry("probe_dosimeter")!;
            Assert.Equal(20, dose.ApplyRetention(catalog));
            Assert.Equal(500, entry.readingsHistory.Count);
            Assert.True(entry.cumulativeMsv > 0f, "retention must never touch cumulative dose");
        }

        [Fact]
        public void ProtectedObligations_AreNeverPruned()
        {
            var catalog = new RetentionPolicyCatalog();
            foreach (string key in new[]
            {
                "survivor_wills_and_legacies",
                "memorial_monuments",
                "campaign_deadlines",
                "survivor_grief_markers"
            })
            {
                var obligations = Enumerable.Range(0, 900).Select(i => $"record_{key}_{i:D4}").ToList();
                Assert.True(catalog.ApplyRetention(key, obligations, out int pruned));
                Assert.Equal(0, pruned);
                Assert.Equal(900, obligations.Count);
            }
        }

        [Fact]
        public void RetentionAppliesThroughTheCanonicalDayTickAndJournalsTheFact()
        {
            string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("_campaignDay.Register(\"retention\"", owners);
            Assert.Contains("new RetentionDayOwner(this)", owners);

            string retention = ReadRepoFile("src", "Main.Retention.cs");
            string host = ReadRepoFile("src", "Host", "RetentionHostSession.cs");
            Assert.Contains("SaveStoreHub.Checksummed<RetentionAuditState>", host);
            Assert.Contains("RetentionAuditReport ApplyRetention()", host);
            Assert.Contains("\"retention_applied\"", retention);
            Assert.Contains("RetentionSaveStore.SectionName", retention);
        }

        [Fact]
        public void OutpostSettlement_CapturesAndRestoresThroughTheRegisteredSection()
        {
            string system = ReadRepoFile("Assets", "Ashfall.Core", "Settlements", "OutpostSettlementSystem.cs");
            Assert.Contains("public OutpostSettlementState CaptureState()", system);
            Assert.Contains("public bool RestoreState(OutpostSettlementState? state)", system);

            var settlements = Ashfall.Core.Settlements.OutpostSettlementSystem.FromJson(
                File.ReadAllText(Path.Combine(DataDir(), "outposts.json")));

            Assert.True(settlements.EstablishOutpost("outpost_north_watch"));
            Assert.True(settlements.AssignGarrison("outpost_north_watch", "survivor_01"));
            Assert.True(settlements.SupplyOutpost("outpost_north_watch", 25));

            var captured = settlements.CaptureState();
            Assert.Equal(settlements.GetAllDefinitions().Count, captured.outposts.Count);

            settlements.RestoreState(captured);
            var restored = settlements.GetInstance("outpost_north_watch")!;
            Assert.True(restored.IsEstablished);
            Assert.Single(restored.GarrisonSurvivorIds);
            Assert.Equal(25, restored.RationReserve);
        }
    }
}
