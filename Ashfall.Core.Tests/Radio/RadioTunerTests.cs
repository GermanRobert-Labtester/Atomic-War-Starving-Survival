using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioTunerTests
    {
        private static List<RadioBroadcast> MakeBroadcasts()
        {
            return new List<RadioBroadcast>
            {
                new RadioBroadcast
                {
                    BroadcastId = "broadcast_a",
                    FrequencyKHz = 90.0f,
                    SignalStrength = 0.7f,
                    LockThreshold = 0.4f,
                    Headline = "Civil defense broadcast",
                    TranscriptLines = new List<string> { "Day 12 advisory", "Day 13 advisory" }
                },
                new RadioBroadcast
                {
                    BroadcastId = "broadcast_b",
                    FrequencyKHz = 105.5f,
                    SignalStrength = 0.9f,
                    LockThreshold = 0.5f,
                    Headline = "Merchant caravan chatter",
                    TranscriptLines = new List<string> { "Caravan leaving Sector 4", "Trade caravan en route" }
                }
            };
        }

        [Fact]
        public void IsTunedTo_TrueWhenWithinTolerance()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            Assert.True(t.IsTunedTo(90.0f));
            Assert.True(t.IsTunedTo(90.4f));
            Assert.False(t.IsTunedTo(91.0f));
            Assert.False(t.IsTunedTo(100.0f));
        }

        [Fact]
        public void TuneBy_AdjustsFrequency()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 88.0f });
            t.TuneBy(2.5f);
            Assert.Equal(90.5f, t.TunedFrequencyKHz, 3);
        }

        [Fact]
        public void TuneTo_NegativeClampsToZero()
        {
            var t = new RadioTuner(new RadioTunerState());
            t.TuneTo(-5f);
            Assert.Equal(0f, t.TunedFrequencyKHz);
        }

        [Fact]
        public void Evaluate_NoBroadcastsReturnsNoSignal()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            var r = t.Evaluate(new List<RadioBroadcast>(), 0.2f, new SeededRng(7));
            Assert.False(r.IsLocked);
            Assert.Equal(0f, r.VuStrength);
        }

        [Fact]
        public void Evaluate_LocksWhenTunedToBroadcast()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            var r = t.Evaluate(MakeBroadcasts(), 0.1f, new SeededRng(11));
            Assert.True(r.IsLocked);
            Assert.Equal("broadcast_a", r.Broadcast.BroadcastId);
            Assert.True(r.VuStrength > r.Broadcast.LockThreshold);
            Assert.False(string.IsNullOrEmpty(r.DecodedContent));
        }

        [Fact]
        public void Evaluate_OffFrequencyReturnsNoLock()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 75.0f });
            var r = t.Evaluate(MakeBroadcasts(), 0.1f, new SeededRng(11));
            Assert.False(r.IsLocked);
        }

        [Fact]
        public void Evaluate_StrongStaticSuppressesLock()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            var r = t.Evaluate(MakeBroadcasts(), 0.99f, new SeededRng(11));
            Assert.False(r.IsLocked);
        }

        [Fact]
        public void Evaluate_DeterministicForSameSeed()
        {
            var t1 = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            var t2 = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            var r1 = t1.Evaluate(MakeBroadcasts(), 0.1f, new SeededRng(99));
            var r2 = t2.Evaluate(MakeBroadcasts(), 0.1f, new SeededRng(99));
            Assert.Equal(r1.DecodedContent, r2.DecodedContent);
            Assert.Equal(r1.IsLocked, r2.IsLocked);
            Assert.Equal(r1.VuStrength, r2.VuStrength, 3);
        }

        [Fact]
        public void Evaluate_StrongestBroadcastWins()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 105.0f });
            // 105 is 0.5 away from broadcast_b (105.5) and 15 from broadcast_a (90).
            var r = t.Evaluate(MakeBroadcasts(), 0.05f, new SeededRng(7));
            Assert.True(r.IsLocked);
            Assert.Equal("broadcast_b", r.Broadcast.BroadcastId);
        }

        [Fact]
        public void Events_FireOnEvaluate()
        {
            var t = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 90.0f });
            int fired = 0;
            t.OnSignalChanged += _ => fired++;
            t.Evaluate(MakeBroadcasts(), 0.1f, new SeededRng(11));
            Assert.True(fired >= 1);
        }
    }
}
