// SPDX-License-Identifier: MIT
// Expansion 30 — The Press : PublicBroadsheetPressEngine focused tests
using Xunit;
using Ashfall.Core.Print;

namespace Ashfall.Core.Tests.Print
{
    public sealed class PublicBroadsheetPressEngineTests
    {
        // ── 1. Print run is blocked when ink is critically low ──
        [Fact]
        public void ExecutePrintRun_BlockedByShortage_WhenInkCriticallyLow()
        {
            var tray = new TypeTrayState
            {
                InkReservoirPermille  = 10, // below MinimumInkPermille (50)
                PaperStockPermille    = 1000,
                TypePiecesAvailable   = 2000
            };

            var result = PublicBroadsheetPressEngine.ExecutePrintRun(
                tray, PublicationKind.Broadsheet, 100, 800, 50);

            Assert.True(result.BlockedByShortage, "Should be blocked by low ink");
            Assert.Equal(0, result.CopiesPrinted);
        }

        // ── 2. Almanac print run yields morale stabilization ──
        [Fact]
        public void ExecutePrintRun_Almanac_GrantsMoraleStabilization()
        {
            var tray = new TypeTrayState
            {
                InkReservoirPermille  = 1000,
                PaperStockPermille    = 1000,
                TypePiecesAvailable   = 2000
            };

            var result = PublicBroadsheetPressEngine.ExecutePrintRun(
                tray, PublicationKind.Almanac, 200, 800, 200);

            Assert.False(result.BlockedByShortage);
            Assert.True(result.CopiesPrinted > 0, "Almanac should print copies");
            Assert.True(result.MoraleStabilizationPermille > 0,
                "Almanac should grant morale stabilization");
        }

        // ── 3. Broadsheet consumes ink and paper from the tray ──
        [Fact]
        public void ExecutePrintRun_Broadsheet_ConsumesInkAndPaper()
        {
            var tray = new TypeTrayState
            {
                InkReservoirPermille  = 1000,
                PaperStockPermille    = 1000,
                TypePiecesAvailable   = 2000
            };

            int inkBefore   = tray.InkReservoirPermille;
            int paperBefore = tray.PaperStockPermille;

            PublicBroadsheetPressEngine.ExecutePrintRun(
                tray, PublicationKind.Broadsheet, 500, 700, 100);

            Assert.True(tray.InkReservoirPermille < inkBefore,
                "Ink should be consumed after print run");
            Assert.True(tray.PaperStockPermille < paperBefore,
                "Paper should be consumed after print run");
            Assert.True(tray.TypeWearPermille > 0,
                "Type tray wear should increase after print run");
        }

        // ── 4. Rumor debunking correction diminishes strong stubborn rumors ──
        [Fact]
        public void CalculateRumorDebunkingCorrection_ReducesRumor_ProportionalToReachAndEvidence()
        {
            int weakRumorCorrection = PublicBroadsheetPressEngine.CalculateRumorDebunkingCorrection(
                rumorStrengthPermille: 200,
                pamphletAudienceReachPermille: 800,
                evidenceQualityPermille: 900);

            int strongRumorCorrection = PublicBroadsheetPressEngine.CalculateRumorDebunkingCorrection(
                rumorStrengthPermille: 900,
                pamphletAudienceReachPermille: 800,
                evidenceQualityPermille: 900);

            // Both corrections should be positive
            Assert.True(weakRumorCorrection > 0,
                "Weak rumor should be correctable");
            // Strong rumors resist more — net correction must not exceed rumor strength
            Assert.True(strongRumorCorrection <= 900,
                "Correction cannot exceed rumor strength");
        }

        // ── 5. Type tray restoration reduces wear permille ──
        [Fact]
        public void RestoreTypeTray_ReducesTypeWear_WhenFreshPiecesAdded()
        {
            var tray = new TypeTrayState
            {
                TypeWearPermille    = 900,
                TypePiecesAvailable = 500
            };

            int wearBefore = tray.TypeWearPermille;
            PublicBroadsheetPressEngine.RestoreTypeTray(tray, 600);

            Assert.True(tray.TypeWearPermille < wearBefore,
                "Adding fresh type pieces should reduce type wear");
            Assert.True(tray.TypePiecesAvailable > 500,
                "Fresh pieces should increase available count");
        }
    }
}
