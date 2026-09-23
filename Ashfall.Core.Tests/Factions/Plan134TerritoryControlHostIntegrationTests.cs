#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 134 — Dynamic Faction Territory & Supply Lines host-integration gate.
//
// Pins the production wiring contract:
//   * the authored faction_territory.json and supply_lines.json load cleanly,
//   * capture/restore exists as a real state DTO with schema version 1,
//   * save section registry and filename map are registered,
//   * event vocabulary and semantic parity matrix are registered,
//   * CLI probe and registry action are registered,
//   * campaign lifecycle and day owner are wired in Main.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    public sealed class Plan134TerritoryControlHostIntegrationTests
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

        private static TerritoryControlSystem LoadAuthored()
        {
            string terrJson = File.ReadAllText(Path.Combine(DataDir(), "faction_territory.json"));
            string supplyJson = File.ReadAllText(Path.Combine(DataDir(), "supply_lines.json"));
            return TerritoryControlSystem.FromJson(terrJson, supplyJson);
        }

        [Fact]
        public void AuthoredCatalogs_LoadTerritoriesAndSupplyLines()
        {
            var system = LoadAuthored();
            var territories = system.GetAllTerritories();
            var supplyLines = system.GetAllSupplyLines();

            Assert.True(territories.Count >= 10, $"Expected at least 10 territories, found {territories.Count}");
            Assert.True(supplyLines.Count >= 3, $"Expected at least 3 supply lines, found {supplyLines.Count}");

            Assert.All(territories, t =>
            {
                Assert.False(string.IsNullOrWhiteSpace(t.Id));
                Assert.False(string.IsNullOrWhiteSpace(t.Faction));
                Assert.True(t.BaseControlStrength > 0);
            });

            Assert.All(supplyLines, s =>
            {
                Assert.False(string.IsNullOrWhiteSpace(s.SupplyLineId));
                Assert.False(string.IsNullOrWhiteSpace(s.OwningFactionId));
                Assert.Equal(SupplyLineStatus.Active, s.Status);
            });
        }

        [Fact]
        public void StateCaptureAndRestore_RoundTripsDeterministically()
        {
            var system = LoadAuthored();

            // Mutate dynamic state
            var firstLoc = system.GetAllLocationStates().First();
            system.FortifyLocation(firstLoc.LocationId, 2);
            system.AssignGarrison(firstLoc.LocationId, 15);

            var firstLine = system.GetAllSupplyLines().First();
            var rng = new SeededRng(134);
            system.RaidSupplyLine(firstLine.SupplyLineId, 85, rng);

            system.TickDay(currentDay: 7, rng: rng);

            var captured = system.CaptureState();
            Assert.Equal(1, captured.schema_version);
            Assert.NotEmpty(captured.locations);
            Assert.NotEmpty(captured.supply_lines);

            string jsonA = JsonSerializer.Serialize(captured);

            // Restore onto fresh system
            var system2 = LoadAuthored();
            bool restored = system2.RestoreState(captured);
            Assert.True(restored);

            var captured2 = system2.CaptureState();
            string jsonB = JsonSerializer.Serialize(captured2);

            Assert.Equal(jsonA, jsonB);

            var restoredLoc = system2.GetLocationState(firstLoc.LocationId);
            Assert.NotNull(restoredLoc);
            Assert.Equal(2, restoredLoc!.FortificationLevel);
            Assert.Equal(firstLoc.GarrisonStrength, restoredLoc.GarrisonStrength);

            var restoredLine = system2.GetSupplyLineState(firstLine.SupplyLineId);
            Assert.NotNull(restoredLine);
            Assert.Equal(SupplyLineStatus.Severed, restoredLine!.Status);
            Assert.Equal(0, restoredLine.LastDeliveredDay);

            var activeLine = system.GetAllSupplyLines().Skip(1).First();
            var restoredActive = system2.GetSupplyLineState(activeLine.SupplyLineId);
            Assert.NotNull(restoredActive);
            Assert.Equal(SupplyLineStatus.Active, restoredActive!.Status);
            Assert.Equal(7, restoredActive.LastDeliveredDay);
        }

        [Fact]
        public void SaveSectionRegistry_RegistersTerritoryControl()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("territory_control", out var metadata));
            Assert.NotNull(metadata);
            Assert.Equal("SaveTerritoryControl", metadata!.SaveMethod);
            Assert.Equal("SetupTerritoryControl", metadata.SetupMethod);
            Assert.Equal("factions", metadata.Owner);

            string? fileName = SaveSectionRegistry.FileNameFor("territory_control");
            Assert.Equal("territory_control_save.json", fileName);
        }

        [Fact]
        public void DayEventVocabulary_RegistersTerritoryControlHeartbeat()
        {
            var kind = DayEventVocabulary.GetSemanticKind("territory_control_ticked");
            Assert.Equal(SemanticKind.Heartbeat, kind);
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("territory_control_ticked"));

            string matrix = ReadRepoFile("docs", "campaign", "EVENT_SEMANTIC_PARITY_MATRIX.md");
            Assert.Contains("territory_control_ticked", matrix);
        }

        [Fact]
        public void HostCliRegistry_RegistersTerritoryControlSelfTest()
        {
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.TerritoryControlSelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--territory-control-selftest", desc!.PrimaryFlag);
            Assert.Contains("--territory-selftest", desc.Aliases);
        }

        [Fact]
        public void ProductionHostSource_WiresTerritoryControlIntoMainAndSessions()
        {
            string owners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("_campaignDay.Register(\"territory_control\"", owners);
            Assert.Contains("new TerritoryControlDayOwner(this)", owners);
            Assert.Contains("territory_control_ticked", owners);

            string expanded = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupTerritoryControl();", expanded);

            string orchestrator = ReadRepoFile("src", "Main.SaveOrchestrator.cs");
            Assert.Contains("SetupTerritoryControl();", orchestrator);
            Assert.Contains("SaveTerritoryControl();", orchestrator);

            string app = ReadRepoFile("src", "Main.Application.cs");
            Assert.Contains("HostCliAction.TerritoryControlSelfTest:", app);
            Assert.Contains("FlushTerritoryControlIfDirty();", app);

            string mainPartial = ReadRepoFile("src", "Main.TerritoryControl.cs");
            Assert.Contains("territory_control_changed", mainPartial);
            Assert.Contains("territory_contested", mainPartial);
            Assert.Contains("supply_line_status_changed", mainPartial);
            Assert.Contains("location_fortified", mainPartial);

            string hostSession = ReadRepoFile("src", "Host", "TerritoryControlHostSession.cs");
            Assert.Contains("class TerritoryControlHostSession", hostSession);
            Assert.Contains("class TerritoryControlSaveStore", hostSession);

            string cliProbe = ReadRepoFile("src", "Host", "HostCli.TerritoryControl.cs");
            Assert.Contains("RunTerritoryControlSelfTest", cliProbe);
        }
    }
}
