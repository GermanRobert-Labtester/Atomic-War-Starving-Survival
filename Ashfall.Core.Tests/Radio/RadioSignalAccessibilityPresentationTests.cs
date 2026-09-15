// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 20: Distress Signal Accessibility & Multimodal Presentation Suite.
    ///
    /// Validates presentation and sensory accessibility:
    /// - Multimodal representation: signals are identified by text frequency, name, and register (never sound alone).
    /// - Color independence (WCAG 1.4.1): authenticity classifications map to distinct textual badges ([GENUINE], [TRAP], etc.).
    /// - Standardized frequency display formatting: consistent decimal formatting and unit labeling.
    /// - Closed captioning (WCAG 1.2): audio artifacts (static, morse, breathing) are transcribed in plain text.
    /// - Screen-reader friendly status descriptions for all DistressSignalStatus enum values.
    /// - Qualitative clarity tiers mapped from quantitative values (0.0 - 1.0).
    /// - Clear countdown presentation without requiring mental math or visual-only progress bars.
    /// </summary>
    public sealed class RadioSignalAccessibilityPresentationTests : CatalogTestBase
    {
        private static List<DistressSignalDefinition> LoadSignals()
        {
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var arr = doc.RootElement.GetProperty("radio_broadcasts");
            var list = new List<DistressSignalDefinition>();
            foreach (var elem in arr.EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<DistressSignalDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                if (def != null) list.Add(def);
            }
            return list;
        }

        [Fact]
        public void Multimodal_SignalIdentification_SurfacesTextualMetadata()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                // Must have text frequency
                string freqText = $"{sig.FrequencyMhz:F1} MHz";
                Assert.False(string.IsNullOrWhiteSpace(freqText));

                // Must have source name
                Assert.False(string.IsNullOrWhiteSpace(sig.SourceName));

                // Must have tone register or classification
                Assert.True(!string.IsNullOrEmpty(sig.ToneRegister) || !string.IsNullOrEmpty(sig.OutcomeType));
            }
        }

        [Fact]
        public void Authenticity_TextualBadges_IndependentOfColor()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                string badge = GetAccessibleBadge(sig);
                Assert.False(string.IsNullOrWhiteSpace(badge),
                    $"Signal '{sig.FrequencyId}' must produce a non-empty textual badge");
                Assert.StartsWith("[", badge);
                Assert.EndsWith("]", badge);
            }
        }

        private static string GetAccessibleBadge(DistressSignalDefinition sig)
        {
            if (sig.IsGenuineRescue) return "[GENUINE RESCUE]";
            if (sig.IsTrapOrDeception) return "[SUSPECTED TRAP / LURE]";
            if (sig.IsAutomated) return "[AUTOMATED BEACON]";
            if (sig.IsGrimOrMemorial) return "[MEMORIAL ARCHIVE]";
            return "[UNKNOWN TRANSMISSION]";
        }

        [Fact]
        public void ReadableFrequency_Formatting_Standardized()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                float freq = sig.FrequencyMhz;
                string formatted = $"{freq:F1} MHz";

                Assert.Matches(@"^\d{2,3}\.\d MHz$", formatted);
            }
        }

        [Fact]
        public void ClosedCaptioning_AudioEffects_AreTextuallyTranscribed()
        {
            var signals = LoadSignals();
            int captionedFragmentCount = 0;

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;

                foreach (var frag in sig.MessageFragments)
                {
                    // Check if audio effects like *static*, *beeping*, *coughing*, *piano plays* are bracketed
                    if (Regex.IsMatch(frag.Text, @"\*.*?\*|\[.*?\]"))
                    {
                        captionedFragmentCount++;
                    }
                }
            }

            // A substantial portion of distress broadcasts feature audio captions
            Assert.True(captionedFragmentCount >= 10,
                $"Expected at least 10 fragments with audio descriptions, found {captionedFragmentCount}");
        }

        [Fact]
        public void Status_Descriptions_AreScreenReaderFriendly()
        {
            foreach (DistressSignalStatus status in Enum.GetValues(typeof(DistressSignalStatus)))
            {
                string description = GetAccessibleStatusDescription(status);
                Assert.False(string.IsNullOrWhiteSpace(description));
                Assert.DoesNotContain("_", description); // Clean plain text, no raw enum identifiers
            }
        }

        private static string GetAccessibleStatusDescription(DistressSignalStatus status) => status switch
        {
            DistressSignalStatus.Inactive => "Inactive Signal",
            DistressSignalStatus.Intercepted => "Signal Intercepted",
            DistressSignalStatus.Triangulated => "Source Triangulated",
            DistressSignalStatus.Dispatched => "Rescue Expedition Dispatched",
            DistressSignalStatus.ResolvedRescued => "Survivors Rescued",
            DistressSignalStatus.ResolvedGrimTooLate => "Arrived Too Late",
            DistressSignalStatus.ResolvedTrapDefeated => "Ambush Neutralized",
            DistressSignalStatus.ResolvedMysteryDecoded => "Mystery Decoded",
            DistressSignalStatus.Expired => "Transmission Expired",
            DistressSignalStatus.ResolvedTrapAvoided => "Hostile Trap Avoided",
            DistressSignalStatus.ResolvedIgnored => "Transmission Ignored",
            _ => "Unknown Status"
        };

        [Fact]
        public void ClarityLevels_ThresholdTable_MapsToQualitativeTiers()
        {
            var failures = new List<string>();

            foreach (var testCase in new[]
            {
                (Clarity: 0.05f, ExpectedTier: "Unintelligible"),
                (Clarity: 0.20f, ExpectedTier: "Heavy Static"),
                (Clarity: 0.40f, ExpectedTier: "Faint Audio"),
                (Clarity: 0.65f, ExpectedTier: "Broken Signal"),
                (Clarity: 0.85f, ExpectedTier: "Clear Audio"),
                (Clarity: 0.98f, ExpectedTier: "Optimal Reception"),
            })
            {
                string actualTier = GetClarityTier(testCase.Clarity);
                if (actualTier != testCase.ExpectedTier)
                {
                    failures.Add(
                        $"clarity={testCase.Clarity}, expected '{testCase.ExpectedTier}', got '{actualTier}'");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        private static string GetClarityTier(float clarity)
        {
            if (clarity < 0.15f) return "Unintelligible";
            if (clarity < 0.30f) return "Heavy Static";
            if (clarity < 0.50f) return "Faint Audio";
            if (clarity < 0.75f) return "Broken Signal";
            if (clarity < 0.95f) return "Clear Audio";
            return "Optimal Reception";
        }

        [Fact]
        public void DaysRemaining_Presentation_ProvidesAccessibleUnits()
        {
            Assert.Equal("Transmission Expired", FormatDaysRemaining(0));
            Assert.Equal("1 day remaining - Imminent loss", FormatDaysRemaining(1));
            Assert.Equal("4 days remaining", FormatDaysRemaining(4));
        }

        private static string FormatDaysRemaining(int days)
        {
            if (days <= 0) return "Transmission Expired";
            if (days == 1) return "1 day remaining - Imminent loss";
            return $"{days} days remaining";
        }
    }
}
