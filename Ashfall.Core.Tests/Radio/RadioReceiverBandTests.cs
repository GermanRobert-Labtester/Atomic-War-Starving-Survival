// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class RadioReceiverBandTests
    {
        private static readonly float[] s_all46Frequencies =
        {
            // VHF-Low
            55.1f, 55.6f, 67.8f, 77.3f, 82.1f, 88.3f, 88.4f, 88.5f, 88.9f, 89.6f,
            91.8f, 93.4f, 94.2f, 97.8f, 98.5f, 98.6f, 101.3f, 103.4f, 104.2f, 104.7f,
            105.6f, 108.9f, 115.2f, 119.3f, 120.4f, 124.7f, 127.6f, 128.5f, 129.6f, 131.0f,
            134.5f, 138.9f, 141.2f, 142.8f, 142.85f, 144.1f, 148.2f,
            // VHF-High
            152.4f, 156.5f, 156.8f, 162.1f, 162.8f, 166.2f, 174.5f, 192.4f, 203.1f, 217.4f,
            278.3f, 288.1f,
            // UHF-Low
            311.0f, 311.5f, 333.6f, 367.9f, 392.7f, 401.9f, 410.7f, 445.2f, 478.2f, 512.4f,
            // UHF-High
            623.8f, 701.3f, 756.1f, 812.5f, 867.9f, 901.2f
        };

        [Fact]
        public void ReceiverBands_CoversAll46FrequenciesWithoutGaps()
        {
            foreach (float freq in s_all46Frequencies)
            {
                var band = RadioReceiverPlan.GetBandForFrequencyMhz(freq);
                Assert.NotNull(band);
                Assert.True(band.ContainsMhz(freq),
                    $"Frequency {freq:0.0} MHz was not contained in resolved band {band.BandId} ({band.MinMhz}..{band.MaxMhz} MHz)");
            }
        }

        [Fact]
        public void ReceiverBands_StepTuningCanReachAllFrequencies()
        {
            // Verify each frequency is reachable by starting at band min and stepping by 0.1 MHz
            foreach (float freq in s_all46Frequencies)
            {
                var band = RadioReceiverPlan.GetBandForFrequencyMhz(freq);
                float current = band.MinMhz;
                float target = (float)Math.Round(freq, 1);
                int steps = (int)Math.Round((target - current) / band.StepMhz);
                float reached = (float)Math.Round(current + (steps * band.StepMhz), 1);
                Assert.True(Math.Abs(reached - target) < 0.06f,
                    $"Could not reach target frequency {target:0.0} MHz via standard step tuning from {band.MinMhz:0.0} MHz (reached {reached:0.0})");
            }
        }

        [Fact]
        public void ReceiverBands_CyclicalNavigationWorksCorrectly()
        {
            var b0 = RadioReceiverPlan.BandVhfLow;
            var b1 = RadioReceiverPlan.NextBand(b0);
            var b2 = RadioReceiverPlan.NextBand(b1);
            var b3 = RadioReceiverPlan.NextBand(b2);
            var b4 = RadioReceiverPlan.NextBand(b3);

            Assert.Equal("vhf_high", b1.BandId);
            Assert.Equal("uhf_low", b2.BandId);
            Assert.Equal("uhf_high", b3.BandId);
            Assert.Equal("vhf_low", b4.BandId);

            var prev = RadioReceiverPlan.PreviousBand(b0);
            Assert.Equal("uhf_high", prev.BandId);
        }

        [Theory]
        [InlineData(88.3f, 88300f)]
        [InlineData(156.8f, 156800f)]
        [InlineData(445.2f, 445200f)]
        [InlineData(901.2f, 901200f)]
        public void UnitConversion_MhzAndKHzAreSymmetric(float mhz, float khz)
        {
            float convertedKhz = RadioReceiverPlan.MhzToKHz(mhz);
            Assert.Equal(khz, convertedKhz);

            float convertedMhz = RadioReceiverPlan.KHzToMhz(convertedKhz);
            Assert.Equal(mhz, convertedMhz);
        }

        [Fact]
        public void FiveRescueMissionFrequencies_DistributeAcrossAllBands()
        {
            // Trapped Mechanic: 88.3 MHz -> VHF-Low
            Assert.Equal("vhf_low", RadioReceiverPlan.GetBandForFrequencyMhz(88.3f).BandId);

            // Injured Trader: 156.8 MHz -> VHF-High
            Assert.Equal("vhf_high", RadioReceiverPlan.GetBandForFrequencyMhz(156.8f).BandId);

            // Raider Trap: 192.4 MHz -> VHF-High
            Assert.Equal("vhf_high", RadioReceiverPlan.GetBandForFrequencyMhz(192.4f).BandId);

            // Family Shelter: 445.2 MHz -> UHF-Low
            Assert.Equal("uhf_low", RadioReceiverPlan.GetBandForFrequencyMhz(445.2f).BandId);

            // Military Patrol: 901.2 MHz -> UHF-High
            Assert.Equal("uhf_high", RadioReceiverPlan.GetBandForFrequencyMhz(901.2f).BandId);
        }
    }
}
