// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 58 — The Continuation (outposts, waystations, second holdfast)
// host-integration gate.
//
// Pins the production wiring contract:
//   * the authored outposts.json loads through the Core parser,
//   * capture/restore exists as a real state DTO (the ORPHAN-W1 blocker),
//   * the custody decision is honoured: no second population/food/settlement
//     store is created, and the three neighbouring authorities stay distinct,
//   * garrison, rations, hostile pressure and the day owners are wired,
//   * the section and the CLI probe are registered.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Settlements
{
    public sealed class Plan58OutpostHostIntegrationTests
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

        private static Ashfall.Core.Settlements.OutpostSettlementSystem LoadAuthored()
            => Ashfall.Core.Settlements.OutpostSettlementSystem.FromJson(
                File.ReadAllText(Path.Combine(DataDir(), "outposts.json")));

        [Fact]
        public void AuthoredCatalog_LoadsFourOutpostsWithGraphNodesAndCosts()
        {
            var system = LoadAuthored();
            var defs = system.GetAllDefinitions();

            Assert.Equal(4, defs.Count);
            Assert.All(defs, d =>
            {
                Assert.False(string.IsNullOrWhiteSpace(d.Id));
                Assert.False(string.IsNullOrWhiteSpace(d.Name));
                Assert.False(string.IsNullOrWhiteSpace(d.GraphNodeId));
                Assert.True(d.MaxGarrisonBunks > 0);
                Assert.True(d.DefenseRating > 0);
                Assert.NotEmpty(d.BuildCost);
            });
            Assert.Equal(defs.Select(d => d.Id).Distinct().Count(), defs.Count);
        }

        [Fact]
        public void CaptureRestore_RestoresEveryFieldThroughTheStateDto()
        {
            var system = LoadAuthored();
            Assert.True(system.EstablishOutpost("outpost_rail_depot"));
            for (int i = 0; i < 3; i++)
                Assert.True(system.AssignGarrison("outpost_rail_depot", $"survivor_{i:D2}"));
            Assert.True(system.SupplyOutpost("outpost_rail_depot", 40));
            system.SimulateRisk("outpost_rail_depot", 200, new Ashfall.Core.SeededRng(3));

            var captured = system.CaptureState();
            Assert.Equal(4, captured.outposts.Count);
            Assert.All(captured.outposts, o => Assert.False(string.IsNullOrWhiteSpace(o.outpost_id)));

            // A fresh campaign with the same authored catalog resumes exactly.
            var resumed = LoadAuthored();
            Assert.True(resumed.RestoreState(captured));

            foreach (var id in system.GetAllDefinitions().Select(d => d.Id))
            {
                var a = system.GetInstance(id)!;
                var b = resumed.GetInstance(id)!;
                Assert.Equal(a.IsEstablished, b.IsEstablished);
                Assert.Equal(a.IsOverrun, b.IsOverrun);
                Assert.Equal(a.IsStarving, b.IsStarving);
                Assert.Equal(a.ConditionPermille, b.ConditionPermille);
                Assert.Equal(a.DaysSinceSupply, b.DaysSinceSupply);
                Assert.Equal(a.RationReserve, b.RationReserve);
                Assert.Equal(a.GarrisonSurvivorIds, b.GarrisonSurvivorIds);
            }
        }

        [Fact]
        public void Restore_IsDeterministicAndRejectsPhantomOrEditedState()
        {
            var system = LoadAuthored();
            system.EstablishOutpost("outpost_north_watch");
            var captured = system.CaptureState();

            Assert.True(system.RestoreState(captured));
            var again = system.CaptureState();
            Assert.Equal(
                captured.outposts.Select(o => o.outpost_id).OrderBy(x => x),
                again.outposts.Select(o => o.outpost_id).OrderBy(x => x));
            Assert.True(again.outposts.Single(o => o.outpost_id == "outpost_north_watch").is_established);

            // A phantom row cannot invent an outpost.
            captured.outposts.Add(new Ashfall.Core.Settlements.OutpostInstanceState
            {
                outpost_id = "outpost_does_not_exist", is_established = true
            });
            Assert.True(system.RestoreState(captured));
            Assert.Null(system.GetInstance("outpost_does_not_exist"));

            // A wrong schema version is refused, not half-applied.
            captured.outposts.Clear();
            captured.schema_version = 99;
            Assert.False(system.RestoreState(captured));

            // Null state is a refusal, not a wipe of live state.
            Assert.False(system.RestoreState(null));
            Assert.True(system.GetInstance("outpost_north_watch")!.IsEstablished);
        }

        [Fact]
        public void Garrison_IsBunkCappedAndFitnessGated()
        {
            var system = LoadAuthored();
            system.EstablishOutpost("outpost_rail_depot");
            var def = system.GetDefinition("outpost_rail_depot")!;

            for (int i = 0; i < def.MaxGarrisonBunks; i++)
                Assert.True(system.AssignGarrison("outpost_rail_depot", $"survivor_{i:D2}"));

            Assert.False(system.AssignGarrison("outpost_rail_depot", "survivor_overflow"));
            Assert.False(system.AssignGarrison("outpost_rail_depot", "survivor_00"));
            Assert.False(system.AssignGarrison("outpost_rail_depot", "unfit_survivor", id => id != "unfit_survivor"));

            Assert.True(system.RelieveGarrison("outpost_rail_depot", "survivor_00"));
            Assert.False(system.RelieveGarrison("outpost_rail_depot", "survivor_00"));
        }

        [Fact]
        public void DailyTick_DrawsRationsFromTheCanonicalProviderAndStarvesWithout()
        {
            var system = LoadAuthored();
            system.EstablishOutpost("outpost_rail_depot");
            system.AssignGarrison("outpost_rail_depot", "survivor_00");
            system.AssignGarrison("outpost_rail_depot", "survivor_01");

            int stock = 4;
            int Provide(string _, int demand)
            {
                int drawn = Math.Min(demand, stock);
                stock -= drawn;
                return drawn;
            }

            system.TickDay(Provide);
            Assert.Equal(2, stock);
            Assert.False(system.GetInstance("outpost_rail_depot")!.IsStarving);

            // With the provider empty the garrison starves and condition decays.
            system.TickDay(Provide);
            system.TickDay(Provide);
            system.TickDay(Provide);
            Assert.True(system.GetInstance("outpost_rail_depot")!.IsStarving);
            Assert.True(system.GetInstance("outpost_rail_depot")!.ConditionPermille < 1000);

            // Supply recovers it.
            Assert.True(system.SupplyOutpost("outpost_rail_depot", 12));
            Assert.False(system.GetInstance("outpost_rail_depot")!.IsStarving);
        }

        [Fact]
        public void CustodyBoundary_HoldsAndNoSecondStoreIsCreated()
        {
            // The three neighbouring settlement authorities remain distinct.
            string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
            Assert.Contains("new(\"outpost_settlement\", \"SaveOutpostSettlement\", \"SetupOutpostSettlement\"",
                registry.Replace('"', '"'));
            Assert.Contains("\"outpost_settlement\", \"outpost_settlement_save.json\"", registry);
            // The waystation, colony and settlement-politics sections are untouched.
            Assert.Contains("new(\"waystation\"", registry);
            Assert.Contains("new(\"colony\"", registry);

            string host = ReadRepoFile("src", "Host", "OutpostSettlementHostSession.cs");
            Assert.Contains("OutpostSettlementSystem.FromJson", host);
            Assert.Contains("Func<string, int, bool>", host);
            Assert.Contains("OutpostSettlementState CaptureState()", host);
            Assert.Contains("public bool RestoreState(OutpostSettlementState? state)", host);

            // The session never owns a food or population store.
            Assert.DoesNotContain("class OutpostInventory", host);
            Assert.DoesNotContain("class OutpostPopulation", host);
        }

        [Fact]
        public void CampaignLifecycle_WiresTheDayOwnerJournalFactsAndProbe()
        {
            string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("_campaignDay.Register(\"outpost_settlement\"", owners);
            Assert.Contains("new OutpostSettlementDayOwner(this)", owners);

            string main = ReadRepoFile("src", "Main.OutpostSettlement.cs");
            foreach (string fact in new[]
            {
                "outpost_established",
                "outpost_supplied",
                "outpost_starving",
                "outpost_overrun",
                "outpost_garrisoned",
                "outpost_relieved"
            })
            {
                Assert.Contains($"\"{fact}\"", main);
            }

            Assert.Contains("CampaignStreamIds.OutpostRisk", main);
            Assert.Contains("OutpostSettlementSaveStore.SectionName", main);

            string registry = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "Ashfall.Core", "HostCliRegistry.cs"));
            Assert.Contains("HostCliAction.OutpostSettlementSelfTest,", registry);
            Assert.Contains("--outpost-settlement-selftest", registry);

            string probe = ReadRepoFile("src", "Host", "HostCli.OutpostSettlement.cs");
            Assert.Contains("public static int RunOutpostSettlementSelfTest(", probe);
        }
    }
}
