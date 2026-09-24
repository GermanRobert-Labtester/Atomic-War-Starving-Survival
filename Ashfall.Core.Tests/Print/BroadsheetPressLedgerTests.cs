// SPDX-License-Identifier: MIT
// Expansion 30 — broadsheet press ledger (stateful owner over the signed pure engine).

using System;
using Ashfall.Core.Print;
using Xunit;

namespace Ashfall.Core.Tests.Print
{
    public sealed class BroadsheetPressLedgerTests
    {
        [Fact]
        public void ExecuteRun_ArchivesPublicationAndConsumesConsumables()
        {
            var ledger = new BroadsheetPressLedger();
            var result = ledger.ExecuteRun("pub_1", PublicationKind.Broadsheet, 200, 500, 40, "Bulletin", 3);

            Assert.False(result.BlockedByShortage);
            Assert.True(result.CopiesPrinted > 0);
            Assert.Equal(200, result.CopiesPrinted); // stock is ample for 200 broadsheets
            Assert.Equal(1, ledger.PublicationCount);

            var archived = ledger.FindPublication("pub_1");
            Assert.NotNull(archived);
            Assert.Equal("Bulletin", archived!.Headline);
            Assert.Equal(3, archived.Day);
            Assert.Equal(PublicationKind.Broadsheet, archived.KindValue);

            Assert.True(ledger.Tray.InkReservoirPermille < 1000);
            Assert.True(ledger.Tray.PaperStockPermille < 1000);
            Assert.True(ledger.Tray.TypeWearPermille > 0);
        }

        [Fact]
        public void BlockedRun_IsRefusedAndNotArchived()
        {
            var ledger = new BroadsheetPressLedger(new BroadsheetPressState
            {
                TypeTray = new TypeTrayState { InkReservoirPermille = 1 }
            });

            var result = ledger.ExecuteRun("pub_x", PublicationKind.Broadsheet, 500, 500, 10);

            Assert.True(result.BlockedByShortage);
            Assert.Equal(0, result.CopiesPrinted);
            Assert.Equal(0, ledger.PublicationCount);
            Assert.Equal(1, ledger.Tray.InkReservoirPermille); // nothing consumed
        }

        [Fact]
        public void Census_ReportsReachWearAndDebunkTallies()
        {
            var ledger = new BroadsheetPressLedger();
            ledger.ExecuteRun("p1", PublicationKind.Broadsheet, 100, 600, 20);
            ledger.ExecuteRun("p2", PublicationKind.Pamphlet, 50, 600, 20);
            ledger.Tray.TypeWearPermille = PublicBroadsheetPressEngine.TypeWearDegradationThreshold;

            var census = ledger.GetCensus();

            Assert.Equal(2, census.PublicationCount);
            Assert.Equal(1, census.DebunkPamphletsPrinted);
            Assert.Equal(150, census.CumulativeCopiesPrinted);
            Assert.True(census.AverageReachPermille > 0);
            Assert.True(census.IsTypeDegraded);
        }

        [Fact]
        public void CaptureRestore_RoundTripsTrayAndArchive_AndSchemaGates()
        {
            var ledger = new BroadsheetPressLedger();
            ledger.ExecuteRun("p1", PublicationKind.Almanac, 120, 700, 15, "Almanac", 9);
            var state = ledger.CaptureState();
            var beforeCensus = ledger.GetCensus();

            var restored = new BroadsheetPressLedger();
            restored.RestoreState(state);

            var census = restored.GetCensus();
            Assert.Equal(ledger.PublicationCount, census.PublicationCount);
            Assert.Equal(beforeCensus.CumulativeCopiesPrinted, census.CumulativeCopiesPrinted);
            Assert.Equal(ledger.Tray.TypeWearPermille, restored.Tray.TypeWearPermille);
            Assert.Equal(ledger.Tray.InkReservoirPermille, restored.Tray.InkReservoirPermille);
            Assert.Equal("Almanac", restored.FindPublication("p1")!.Headline);

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));

            var legacy = ledger.CaptureState();
            legacy.SchemaVersion = 0;
            restored.RestoreState(legacy);
        }

        [Fact]
        public void DebunkCorrection_IsPureAndArchiveIsBounded()
        {
            var ledger = new BroadsheetPressLedger();
            int correction = ledger.CalculateDebunkCorrection(1000, 1000, 1000);
            Assert.True(correction > 0);
            // The calculation must not mutate press or rumor state.
            Assert.Equal(0, ledger.PublicationCount);
            Assert.Equal(1000, ledger.Tray.InkReservoirPermille);

            for (int i = 0; i < BroadsheetPressLedger.MaxArchivedPublications + 25; i++)
                ledger.ExecuteRun($"p{i}", PublicationKind.Wanted, 1, 100, 500);

            Assert.Equal(BroadsheetPressLedger.MaxArchivedPublications, ledger.PublicationCount);
        }

        [Fact]
        public void RestockConsumables_ClampsAtFull()
        {
            var ledger = new BroadsheetPressLedger(new BroadsheetPressState
            {
                TypeTray = new TypeTrayState { InkReservoirPermille = 900, PaperStockPermille = 100, TypePiecesAvailable = 100 }
            });

            ledger.RestockConsumables(500, 500, 200);

            Assert.Equal(1000, ledger.Tray.InkReservoirPermille);
            Assert.Equal(600, ledger.Tray.PaperStockPermille);
            Assert.Equal(300, ledger.Tray.TypePiecesAvailable);
        }
    }
}
