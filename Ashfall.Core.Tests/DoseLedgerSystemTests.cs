using System;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DoseLedgerSystemTests
    {
        [Fact]
        public void BookReading_WithoutTag_IsNotBooked()
        {
            var dl = new DoseLedgerSystem();
            var result = dl.BookReading("sv_x", 1, 50f, "sampling", false, false, false, new SeededRng(1));
            Assert.Equal(DoseBandResult.NoEntry, result);
            Assert.Equal(0f, dl.GetCumulative("sv_x"));
        }

        [Fact]
        public void BookReading_CrossesAmberBand_AndFiresEvent()
        {
            var dl = new DoseLedgerSystem();
            dl.AssignDosimeter("sv_x", "tag1");
            int band = -1;
            dl.OnBandReached += (id, b) => band = b;

            dl.BookReading("sv_x", 1, 120f, "sampling", false, false, false, new SeededRng(1));
            Assert.Equal(DoseLedgerSystem.BandAmber, band);
            Assert.Equal(DoseLedgerSystem.BandAmber, DoseLedgerSystem.BandFor(dl.GetCumulative("sv_x")));
        }

        [Fact]
        public void AntiRadAfter_ReducesBookedDose()
        {
            var dl = new DoseLedgerSystem();
            dl.AssignDosimeter("sv_x", "tag1");
            dl.BookReading("sv_x", 1, 100f, "x", false, false, true, new SeededRng(1)); // anti-rad after
            Assert.True(dl.GetCumulative("sv_x") < 60.01f); // 100 * 0.6 = 60
        }

        [Fact]
        public void FluxAmbiguity_IsDeterministicPerSeed()
        {
            var a = new DoseLedgerSystem();
            a.AssignDosimeter("sv_a", "t");
            var b = new DoseLedgerSystem();
            b.AssignDosimeter("sv_b", "t");

            a.BookReading("sv_a", 1, 80f, "storm", true, false, false, new SeededRng(7));
            b.BookReading("sv_b", 1, 80f, "storm", true, false, false, new SeededRng(7));

            Assert.Equal(a.GetCumulative("sv_a"), b.GetCumulative("sv_b"));
        }

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var dl = new DoseLedgerSystem();
            dl.AssignDosimeter("sv_x", "tag1", 20f);
            dl.BookReading("sv_x", 1, 200f, "sampling", false, false, true, new SeededRng(1));

            var state = dl.CaptureState();
            var dlB = new DoseLedgerSystem();
            dlB.RestoreState(state);

            Assert.Equal(dl.GetCumulative("sv_x"), dlB.GetCumulative("sv_x"));
        }
    }

    public class CohortAndVolunteerTests
    {
        [Fact]
        public void BookChild_ThenCorrectBaseline()
        {
            var cohort = new CohortSystem();
            Assert.True(cohort.BookChild("sv_child", new[] { "sv_a", "sv_b" }, "low", 12, "told a kind number"));
            Assert.False(cohort.BookChild("sv_child", null, "low", 12)); // booked twice refused
            Assert.True(cohort.CorrectBaseline("sv_child", "high"));
            Assert.True(cohort.GetChild("sv_child").baselineCorrected);
            Assert.Equal("high", cohort.GetChild("sv_child").trueBand);
        }

        [Fact]
        public void Volunteer_SignAndComplete_BanksDose()
        {
            var v = new VoluntaryRegisterSystem();
            Assert.True(v.Volunteer("sv_a", "vented reactor", 20, "I worked the corridor before."));
            Assert.True(v.CompleteVolunteer("sv_a", "vented reactor", 250f, 21));
            Assert.False(v.CompleteVolunteer("sv_a", "vented reactor", 0f, 22)); // already done
            Assert.Equal(250f, v.GetEntry("sv_a", "vented reactor").doseIncurred);
        }
    }

    internal sealed class SeededRng : ISeededRng
    {
        private readonly System.Random _rng;
        public int Seed { get; }
        public SeededRng(int seed) { Seed = seed; _rng = new System.Random(seed); }
        public int Next(int min, int max) => _rng.Next(min, max);
        public float NextFloat() => (float)_rng.NextDouble();
        public double NextDouble() => _rng.NextDouble();
    }
}