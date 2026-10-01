// SPDX-License-Identifier: MIT
// P013–P016 status-glance wiring gate.
//
// The host UI is not compiled into this test project, so these are source-level
// contracts plus the pure-Core forecast calculator: each presentation surface
// must consume an owning authority (radiation status, clothing/thermal owners,
// ration+inventory projection) rather than re-typing a value, and each producer
// must have a call site.
using System;
using System.IO;
using Xunit;
using Ashfall.Core.Campaign;

namespace Ashfall.Core.Tests.UI
{
    public sealed class StatusGlanceAndForecastGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("repository root not found");
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot(), relativePath.Replace('/', Path.DirectorySeparatorChar)));

        // ── P016 — pure forecast calculator ────────────────────────────────

        [Fact]
        public void Project_SubtractsConsumptionPerDay_AndKeepsNegatives()
        {
            var rows = SupplyForecast.Project(startDay: 1, foodUnits: 5, waterUnits: 2,
                foodPerDay: 3, waterPerDay: 3, horizonDays: 3);

            Assert.Equal(3, rows.Count);
            Assert.Equal(2, rows[0].Day);
            Assert.Equal(2, rows[0].FoodAfter);
            Assert.Equal(-1, rows[0].WaterAfter);
            Assert.Equal(-1, rows[1].FoodAfter);
            Assert.Equal(-4, rows[2].FoodAfter);
            Assert.True(rows[0].WaterShort);
            Assert.False(rows[0].FoodShort);
        }

        [Fact]
        public void FirstShortfallDay_ReportsTheFirstShortRow_OrZero()
        {
            // food 7 -> 4 -> 1 -> -2, so the shortfall lands on the third row (Day 4).
            var shortOnThirdRow = SupplyForecast.Project(1, 7, 7, 3, 3, 3);
            Assert.Equal(4, SupplyForecast.FirstShortfallDay(shortOnThirdRow));

            var neverShort = SupplyForecast.Project(1, 30, 30, 3, 3, 3);
            Assert.Equal(0, SupplyForecast.FirstShortfallDay(neverShort));
        }

        [Fact]
        public void Project_NonPositiveHorizon_IsEmpty()
        {
            Assert.Empty(SupplyForecast.Project(1, 5, 5, 1, 1, 0));
            Assert.Empty(SupplyForecast.Project(1, 5, 5, 1, 1, -2));
        }

        // ── P013 — the acute-radiation flag on the status glance ───────────

        [Fact]
        public void StatusPanel_FlagsAcuteRadiationFromTheOwner()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("HasAcuteRadiationSickness", status, StringComparison.Ordinal);
            // The English phrase now lives in the catalog; pin the key the panel calls.
            Assert.Contains("ui.status.objective.acute_rad", status, StringComparison.Ordinal);
            Assert.Contains("acute radiation sickness", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);

            string radiation = Read("Assets/Ashfall.Core/Radiation/RadiationSystem.cs");
            Assert.Contains("ClearResolvedAcuteStatus", radiation, StringComparison.Ordinal);
        }

        // ── P014 — per-survivor Game Over ledger ───────────────────────────

        [Fact]
        public void GameOver_CarriesAPerSurvivorLedger()
        {
            string panel = Read("src/UI/GameOverPanel.cs");
            Assert.Contains("_lblLedger", panel, StringComparison.Ordinal);
            Assert.Contains("ShowGameOver(string cause, string stats, string ledger)", panel, StringComparison.Ordinal);

            string fate = Read("src/Main.SurvivorFate.cs");
            Assert.Contains("BuildSurvivorLossLedger", fate, StringComparison.Ordinal);
            Assert.Contains("SurvivorFateSystem.DescribeCause", fate, StringComparison.Ordinal);
        }

        // ── P015 — warmth/temperature readout from the owners ──────────────

        [Fact]
        public void StatusPanel_RendersWarmthAndThermal_FromOwningSessions()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("RenderThermal", status, StringComparison.Ordinal);
            Assert.Contains("ClothingWarmthHostSession", status, StringComparison.Ordinal);
            Assert.Contains("ShelterThermalHostSession", status, StringComparison.Ordinal);
            Assert.Contains("CalculateColdLossReduction", status, StringComparison.Ordinal);
            Assert.Contains("BoilerFuelLevel", status, StringComparison.Ordinal);
        }

        // ── P016 — forecast strip producer + panel renderer ────────────────

        [Fact]
        public void StatusPanel_RendersSupplyForecast_AndHostProducesIt()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("RenderForecast", status, StringComparison.Ordinal);
            Assert.Contains("SupplyForecast.FirstShortfallDay", status, StringComparison.Ordinal);

            string host = Read("src/Main.SupplyForecast.cs");
            Assert.Contains("SupplyForecast.Project", host, StringComparison.Ordinal);
            Assert.Contains("rationPolicy", host, StringComparison.Ordinal);

            string surfaces = Read("src/Main.PlayerSurfaces.cs");
            Assert.Contains("BuildSupplyForecast()", surfaces, StringComparison.Ordinal);
        }
    }
}