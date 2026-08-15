using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CohortSystemTests
    {
        [Fact]
        public void BookChild_AddsToBoardAndFiresEvent()
        {
            var sys = new CohortSystem();
            int fired = 0;
            sys.OnChildBooked += (child, parents) => fired++;
            Assert.True(sys.BookChild("child_1", new List<string> { "sv_mae", "sv_ged" }, "high", 300));
            Assert.Equal(1, fired);
            var child = sys.GetChild("child_1");
            Assert.NotNull(child);
            Assert.Equal("high", child.guessBand);
            Assert.Equal(300, child.birthDay);
            Assert.False(child.baselineCorrected);
            Assert.Equal(2, child.parentIds.Count);
        }

        [Fact]
        public void BookChild_EmptyParentsAllowed()
        {
            var sys = new CohortSystem();
            Assert.True(sys.BookChild("child_2", new List<string>(), "medium", 301));
            Assert.Empty(sys.GetChild("child_2").parentIds);
        }

        [Fact]
        public void BookChild_NullChildIdRejected()
        {
            var sys = new CohortSystem();
            Assert.False(sys.BookChild(null, new List<string>(), "low", 300));
            Assert.False(sys.BookChild("", new List<string>(), "low", 300));
            Assert.Empty(sys.Children);
        }

        [Fact]
        public void CorrectBaseline_StoresCorrectionWithoutPostingToLedger()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_1", new List<string>(), "low", 300);
            int fired = 0;
            sys.OnBaselineCorrected += (child, band) => fired++;
            Assert.True(sys.CorrectBaseline("child_1", "band_red"));
            Assert.Equal(1, fired);
            var child = sys.GetChild("child_1");
            Assert.True(child.baselineCorrected);
            Assert.Equal("band_red", child.trueBand);
            // The bible: correction never auto-posts to the dose ledger.
            Assert.Equal("low", child.guessBand);
        }

        [Fact]
        public void CorrectBaseline_UnknownChildRejected()
        {
            var sys = new CohortSystem();
            Assert.False(sys.CorrectBaseline("child_missing", "band_amber"));
        }

        [Fact]
        public void Children_AreNeverPruned()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_1", new List<string>(), "medium", 300);
            sys.CorrectBaseline("child_1", "band_black");
            Assert.Single(sys.Children);
            Assert.NotNull(sys.GetChild("child_1"));
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_1", new List<string>(), "high", 300);
            var snapshot = sys.CaptureState();
            snapshot.children[0].guessBand = "injected";
            Assert.Equal("high", sys.GetChild("child_1").guessBand);
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_zed", new List<string>(), "low", 300);
            sys.BookChild("child_a", new List<string>(), "low", 300);
            var snapshot = sys.CaptureState();
            for (int i = 1; i < snapshot.children.Count; i++)
                Assert.True(string.CompareOrdinal(snapshot.children[i - 1].survivorId, snapshot.children[i].survivorId) <= 0);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_1", new List<string> { "sv_mae" }, "low", 300);
            sys.CorrectBaseline("child_1", "band_red");

            var restored = new CohortSystem();
            restored.RestoreState(sys.CaptureState());

            var child = restored.GetChild("child_1");
            Assert.NotNull(child);
            Assert.True(child.baselineCorrected);
            Assert.Equal("band_red", child.trueBand);
            Assert.Single(child.parentIds);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_1", new List<string>(), "medium", 300);
            sys.BookChild("child_2", new List<string>(), "high", 310);
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new CohortSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }
    }
}
