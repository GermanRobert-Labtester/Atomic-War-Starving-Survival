// SPDX-License-Identifier: MIT
// Expansion 29 — glassworks ledger (stateful owner over DEC-83).

using System;
using Ashfall.Core.Optics;
using Xunit;

namespace Ashfall.Core.Tests.Optics
{
    public sealed class GlassworksLedgerTests
    {
        [Fact]
        public void AddBatch_RejectsDuplicateAndEmptyIds()
        {
            var ledger = new GlassworksLedger();
            Assert.True(ledger.AddBatch("b1", 900));
            Assert.False(ledger.AddBatch("b1", 900));
            Assert.False(ledger.AddBatch("", 900));
            Assert.Equal(1, ledger.BatchCount);
        }

        [Fact]
        public void AdvancingAnnealing_ProducesExpectedTier()
        {
            var ledger = new GlassworksLedger();
            ledger.AddBatch("b1", 980);
            for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                ledger.AdvanceAnnealing("b1", 900);

            var batch = ledger.FindBatch("b1")!;
            Assert.False(batch.IsCracked);
            Assert.Equal(GlassPurityTier.PrecisionOptic, batch.ResultingTier);
            Assert.Equal(1, ledger.GetCensus().PrecisionOpticBatches);
        }

        [Fact]
        public void Grinding_ConsumesGritStockAndRecordsPrescription()
        {
            var ledger = new GlassworksLedger();
            ledger.AddBatch("b1", 980);
            for (int i = 0; i < PrecisionGlassworksOpticsEngine.MaxAnnealingStages; i++)
                ledger.AdvanceAnnealing("b1", 900);

            int before = ledger.GritStockPermille;
            var result = ledger.GrindCorrectionLens("b1", VisionCorrectionBand.MildMyopia, 900, 42);
            Assert.True(result.MeetsPrescriptionTolerance);
            Assert.True(ledger.GritStockPermille < before);

            ledger.SetVisionPrescription("survivor_1", VisionCorrectionBand.ModerateMyopia);
            Assert.Equal(VisionCorrectionBand.ModerateMyopia, ledger.GetVisionPrescription("survivor_1"));
            Assert.Equal(VisionCorrectionBand.NoCorrectionNeeded, ledger.GetVisionPrescription("unknown"));
        }

        [Fact]
        public void CaptureRestore_RoundTripsAndSchemaGates()
        {
            var ledger = new GlassworksLedger();
            ledger.AddBatch("b1", 850);
            ledger.AdvanceAnnealing("b1", 900);
            ledger.SetVisionPrescription("s1", VisionCorrectionBand.Presbyopia);
            var state = ledger.CaptureState();

            var restored = new GlassworksLedger();
            restored.RestoreState(state);
            Assert.Equal(ledger.BatchCount, restored.BatchCount);
            Assert.Equal(VisionCorrectionBand.Presbyopia, restored.GetVisionPrescription("s1"));

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));
        }

        [Fact]
        public void Clear_ResetsBatchesPrescriptionsAndGrit()
        {
            var ledger = new GlassworksLedger();
            ledger.AddBatch("b1", 980);
            ledger.SetVisionPrescription("s1", VisionCorrectionBand.MildMyopia);
            ledger.GrindCorrectionLens("b1", VisionCorrectionBand.MildMyopia, 900, 1);

            ledger.Clear();
            var census = ledger.GetCensus();
            Assert.Equal(0, census.BatchCount);
            Assert.Equal(0, census.PrescriptionCount);
            Assert.Equal(1000, census.GritStockPermille);
        }
    }
}
