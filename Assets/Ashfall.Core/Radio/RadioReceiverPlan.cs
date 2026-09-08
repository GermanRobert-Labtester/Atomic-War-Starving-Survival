// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Definition of an operational frequency band on the ASHFALL multi-band receiver.
    /// Invariant: Pure C#, zero engine dependencies.
    /// </summary>
    [Serializable]
    public sealed class RadioReceiverBand
    {
        public string BandId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float MinMhz { get; set; }
        public float MaxMhz { get; set; }
        public float StepMhz { get; set; } = 0.1f;

        public float MinKHz => RadioReceiverPlan.MhzToKHz(MinMhz);
        public float MaxKHz => RadioReceiverPlan.MhzToKHz(MaxMhz);
        public float StepKHz => RadioReceiverPlan.MhzToKHz(StepMhz);

        public RadioReceiverBand() { }

        public RadioReceiverBand(string bandId, string displayName, float minMhz, float maxMhz, float stepMhz = 0.1f)
        {
            BandId = bandId;
            DisplayName = displayName;
            MinMhz = minMhz;
            MaxMhz = maxMhz;
            StepMhz = stepMhz;
        }

        public bool ContainsMhz(float mhz) => mhz >= MinMhz - 0.05f && mhz <= MaxMhz + 0.05f;
        public bool ContainsKHz(float khz) => ContainsMhz(RadioReceiverPlan.KHzToMhz(khz));

        public float ClampMhz(float mhz) => Math.Clamp(mhz, MinMhz, MaxMhz);
        public float ClampKHz(float khz) => Math.Clamp(khz, MinKHz, MaxKHz);
    }

    /// <summary>
    /// Authoritative frequency plan and multi-band receiver model covering all 46 distinct
    /// frequencies in ASHFALL airwaves (55.1 MHz to 901.2 MHz).
    /// </summary>
    public static class RadioReceiverPlan
    {
        public static float MhzToKHz(float mhz) => (float)Math.Round(mhz * 1000f, 1);
        public static float KHzToMhz(float khz) => (float)Math.Round(khz / 1000f, 4);

        public static readonly RadioReceiverBand BandVhfLow = new RadioReceiverBand(
            "vhf_low", "VHF-Low (50–150 MHz)", 50.0f, 150.0f, 0.1f);

        public static readonly RadioReceiverBand BandVhfHigh = new RadioReceiverBand(
            "vhf_high", "VHF-High (150–300 MHz)", 150.0f, 300.0f, 0.1f);

        public static readonly RadioReceiverBand BandUhfLow = new RadioReceiverBand(
            "uhf_low", "UHF-Low (300–550 MHz)", 300.0f, 550.0f, 0.1f);

        public static readonly RadioReceiverBand BandUhfHigh = new RadioReceiverBand(
            "uhf_high", "UHF-High (550–950 MHz)", 550.0f, 950.0f, 0.1f);

        private static readonly RadioReceiverBand[] s_bands =
        {
            BandVhfLow,
            BandVhfHigh,
            BandUhfLow,
            BandUhfHigh
        };

        public static IReadOnlyList<RadioReceiverBand> AllBands => s_bands;

        public static RadioReceiverBand GetBandByIndex(int index)
        {
            if (index < 0 || index >= s_bands.Length) return BandVhfLow;
            return s_bands[index];
        }

        public static int GetBandIndex(string bandId)
        {
            for (int i = 0; i < s_bands.Length; i++)
            {
                if (string.Equals(s_bands[i].BandId, bandId, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return 0;
        }

        public static RadioReceiverBand GetBandForFrequencyMhz(float mhz)
        {
            for (int i = 0; i < s_bands.Length; i++)
            {
                if (s_bands[i].ContainsMhz(mhz))
                    return s_bands[i];
            }
            if (mhz < BandVhfLow.MinMhz) return BandVhfLow;
            return BandUhfHigh;
        }

        public static RadioReceiverBand GetBandForFrequencyKHz(float khz)
        {
            return GetBandForFrequencyMhz(KHzToMhz(khz));
        }

        public static RadioReceiverBand NextBand(RadioReceiverBand current)
        {
            int idx = GetBandIndex(current.BandId);
            int nextIdx = (idx + 1) % s_bands.Length;
            return s_bands[nextIdx];
        }

        public static RadioReceiverBand PreviousBand(RadioReceiverBand current)
        {
            int idx = GetBandIndex(current.BandId);
            int prevIdx = (idx - 1 + s_bands.Length) % s_bands.Length;
            return s_bands[prevIdx];
        }
    }
}
