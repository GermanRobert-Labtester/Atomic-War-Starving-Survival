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

        // TEST-AGGREGATION: source_rows=4 aggregate_cases=1 saved_cases=3
        [Fact]
        public void UnitConversion_MhzAndKHzAreSymmetric()
        {
            var failures = new List<string>();
            var cases = new[]
            {
                (Mhz: 88.3f, Khz: 88300f),
                (Mhz: 156.8f, Khz: 156800f),
                (Mhz: 445.2f, Khz: 445200f),
                (Mhz: 901.2f, Khz: 901200f),
            };

            foreach (var testCase in cases)
            {
                float convertedKhz = RadioReceiverPlan.MhzToKHz(testCase.Mhz);
                float convertedMhz = RadioReceiverPlan.KHzToMhz(convertedKhz);
                if (convertedKhz != testCase.Khz || convertedMhz != testCase.Mhz)
                {
                    failures.Add($"{testCase.Mhz} MHz: expected {testCase.Khz} kHz / {testCase.Mhz} MHz, got {convertedKhz} kHz / {convertedMhz} MHz");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        // TEST-AGGREGATION: source_rows=5 aggregate_cases=1 saved_cases=4
        public void FiveRescueMissionFrequencies_DistributeAcrossAllBands()
        {
            var failures = new List<string>();
            var cases = new[]
            {
                (FrequencyMhz: 88.3f, ExpectedBandId: "vhf_low"),
                (FrequencyMhz: 156.8f, ExpectedBandId: "vhf_high"),
                (FrequencyMhz: 192.4f, ExpectedBandId: "vhf_high"),
                (FrequencyMhz: 445.2f, ExpectedBandId: "uhf_low"),
                (FrequencyMhz: 901.2f, ExpectedBandId: "uhf_high"),
            };

            foreach (var testCase in cases)
            {
                var actualBandId = RadioReceiverPlan.GetBandForFrequencyMhz(testCase.FrequencyMhz).BandId;
                if (actualBandId != testCase.ExpectedBandId)
                {
                    failures.Add($"{testCase.FrequencyMhz} MHz: expected {testCase.ExpectedBandId}, got {actualBandId}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }
    }
}
