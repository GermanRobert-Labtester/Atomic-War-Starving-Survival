// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SickListSystemTests
    {
        [Fact]
        public void Identity_IsTrimmedCaseInsensitiveAndReDiagnosisKeepsOneRow()
        {
            var sys = new SickListSystem();

            Assert.True(sys.Diagnose(" SV_MAE ", DoseLedgerSystem.BandAmber, 10));
            Assert.True(sys.Diagnose("sv_mae", DoseLedgerSystem.BandRed, 11));

            Assert.Single(sys.Bands);
            Assert.Equal("SV_MAE", sys.GetBand("SV_MAE")!.survivorId);
            Assert.Equal(DoseLedgerSystem.BandRed, sys.GetBand("sv_mae")!.band);
        }

        [Fact]
        public void InvalidBandDayAndReleaseTransitions_AreRejected()
        {
            var sys = new SickListSystem();

            Assert.False(sys.Diagnose("sv_bad_band", 99, 5));
            Assert.False(sys.Diagnose("sv_bad_day", DoseLedgerSystem.BandRed, -1));
            Assert.Empty(sys.Bands);

            sys.Diagnose("sv_valid", DoseLedgerSystem.BandRed, 20);
            Assert.False(sys.Release("sv_valid", 19));
            Assert.Equal(-1, sys.GetBand("sv_valid")!.releaseDay);
            Assert.True(sys.Release("SV_VALID", 21));
        }

        [Fact]
        public void UnsupportedSeveritySource_FailsBackToDoseProvenance()
        {
            var sys = new SickListSystem();

            sys.Diagnose("sv_source", DoseLedgerSystem.BandAmber, 5, "unknown_source", " source_id ");

            var band = sys.GetBand("sv_source")!;
            Assert.Equal(SickListSystem.SourceDose, band.severitySource);
            Assert.Equal("source_id", band.sourceId);
        }

        [Fact]
        public void Restore_FiltersMalformedAndDuplicateRows()
        {
            var sys = new SickListSystem();
            var state = new SickListSystemState { systemId = "  " };
            state.bands.Add(null!);
            state.bands.Add(new SickBand { survivorId = "  ", band = DoseLedgerSystem.BandRed });
            state.bands.Add(new SickBand
            {
                survivorId = " SV_BAD ",
                band = 99,
                diagnosedDay = -4,
                releaseDay = -8,
                severitySource = "unknown",
                sourceId = " x "
            });
            state.bands.Add(new SickBand
            {
                survivorId = "sv_good",
                band = DoseLedgerSystem.BandAmber,
                diagnosedDay = 7,
                releaseDay = 9,
                severitySource = SickListSystem.SourceIllness,
                sourceId = " disease_x "
            });
            state.bands.Add(new SickBand
            {
                survivorId = "SV_GOOD",
                band = DoseLedgerSystem.BandBlack,
                diagnosedDay = 8,
                releaseDay = -1
            });

            sys.RestoreState(state);
            var captured = sys.CaptureState();

            Assert.Equal(SickListSystem.SystemId, captured.systemId);
            Assert.Single(captured.bands);
            Assert.Equal("sv_good", captured.bands[0].survivorId);
            Assert.Equal(DoseLedgerSystem.BandAmber, captured.bands[0].band);
            Assert.Equal("disease_x", captured.bands[0].sourceId);
        }

        [Fact]
        public void StateBandAndChangeEventQueries_AreDetachedSnapshots()
        {
            var sys = new SickListSystem();
            SickListSystemState? changed = null;
            sys.OnStateChanged += state => changed = state;
            sys.Diagnose("sv_snapshot", DoseLedgerSystem.BandRed, 8);

            changed!.bands.Clear();
            sys.State.bands.Clear();
            sys.GetBand("sv_snapshot")!.band = DoseLedgerSystem.BandGreen;

            Assert.Single(sys.Bands);
            Assert.Equal(DoseLedgerSystem.BandRed, sys.GetBand("sv_snapshot")!.band);
            Assert.Single(sys.CaptureState().bands);
        }

        [Fact]
        public void Diagnose_AddsBandAndFiresEvent()
        {
            var sys = new SickListSystem();
            int fired = 0;
            sys.OnDiagnosed += (sv, band) => fired++;
            Assert.True(sys.Diagnose("sv_mae", DoseLedgerSystem.BandRed, 200));
            Assert.Equal(1, fired);
            var band = sys.GetBand("sv_mae");
            Assert.NotNull(band);
            Assert.Equal(DoseLedgerSystem.BandRed, band.band);
            Assert.Equal(200, band.diagnosedDay);
            Assert.Equal(-1, band.releaseDay);
        }

        [Fact]
        public void Diagnose_MovesBandKeepsRow()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_mae", DoseLedgerSystem.BandAmber, 200);
            Assert.True(sys.Diagnose("sv_mae", DoseLedgerSystem.BandBlack, 240));
            Assert.Single(sys.Bands);
            Assert.Equal(DoseLedgerSystem.BandBlack, sys.GetBand("sv_mae").band);
        }

        [Fact]
        public void Diagnose_NullOrEmptySurvivorRejected()
        {
            var sys = new SickListSystem();
            Assert.False(sys.Diagnose(null, DoseLedgerSystem.BandRed, 200));
            Assert.False(sys.Diagnose("", DoseLedgerSystem.BandRed, 200));
            Assert.Empty(sys.Bands);
        }

        [Fact]
        public void Release_SetsDayAndKeepsRow()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_ged", DoseLedgerSystem.BandAmber, 210);
            Assert.False(sys.Release("sv_unknown", 220));
            Assert.True(sys.Release("sv_ged", 220));
            Assert.Equal(220, sys.GetBand("sv_ged").releaseDay);
        }

        [Fact]
        public void AssignPalliative_RequiresDiagnosedSurvivor()
        {
            var sys = new SickListSystem();
            Assert.False(sys.AssignPalliative("sv_mae", "plan_morphine_tray"));
            sys.Diagnose("sv_mae", DoseLedgerSystem.BandRed, 250);
            Assert.False(sys.AssignPalliative("sv_mae", ""));
            Assert.True(sys.AssignPalliative("sv_mae", "plan_morphine_tray"));
            Assert.Equal("plan_morphine_tray", sys.GetBand("sv_mae").palliativePlan);
        }

        [Fact]
        public void BlackBand_RemainsOnRoster()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_wren", DoseLedgerSystem.BandBlack, 260);
            Assert.NotNull(sys.GetBand("sv_wren"));
            Assert.Equal(DoseLedgerSystem.BandBlack, sys.GetBand("sv_wren").band);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_mae", DoseLedgerSystem.BandRed, 200);
            var snapshot = sys.CaptureState();
            snapshot.bands[0].band = DoseLedgerSystem.BandGreen;
            Assert.Equal(DoseLedgerSystem.BandRed, sys.GetBand("sv_mae").band);
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_zed", DoseLedgerSystem.BandRed, 200);
            sys.Diagnose("sv_a", DoseLedgerSystem.BandAmber, 200);
            var snapshot = sys.CaptureState();
            for (int i = 1; i < snapshot.bands.Count; i++)
                Assert.True(string.CompareOrdinal(snapshot.bands[i - 1].survivorId, snapshot.bands[i].survivorId) <= 0);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_mae", DoseLedgerSystem.BandRed, 250);
            sys.AssignPalliative("sv_mae", "plan_morphine_tray");
            sys.Diagnose("sv_iora", DoseLedgerSystem.BandAmber, 260);
            sys.Release("sv_iora", 270);

            var restored = new SickListSystem();
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(2, restored.Bands.Count);
            Assert.Equal(DoseLedgerSystem.BandRed, restored.GetBand("sv_mae").band);
            Assert.Equal("plan_morphine_tray", restored.GetBand("sv_mae").palliativePlan);
            Assert.Equal(270, restored.GetBand("sv_iora").releaseDay);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = new SickListSystem();
            sys.Diagnose("sv_a", DoseLedgerSystem.BandAmber, 200);
            sys.Diagnose("sv_b", DoseLedgerSystem.BandBlack, 300);
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new SickListSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }
    }
}
