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
    /// Flagship Task 19: Distress Signal Localization Readiness Suite.
    ///
    /// Validates:
    /// - Every signal identifier is localization-key safe (snake_case lowercase alphanumeric).
    /// - Source names are non-empty, trimmed, and free of formatting control codes.
    /// - Message fragments contain zero forbidden real-world countries, cities, or historical entities.
    /// - Sentence punctuation adheres to translation unit boundaries.
    /// - Sentence length and character length respect UI localization expansion budgets.
    /// - Warning texts, outcome summaries, and tone registers are structured translatable strings.
    /// - UTF-8 compliance and non-printable control character hygiene.
    /// - Full localization key scheme generates 100% distinct, deterministically indexed keys.
    /// </summary>
    public sealed class RadioSignalLocalizationTests : CatalogTestBase
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

        private static readonly string[] ForbiddenRealWorldTerms =
        {
            "America", "United States", "USA", "Russia", "Soviet", "USSR",
            "China", "Chinese", "Britain", "United Kingdom", "UK", "Germany",
            "France", "Japan", "NATO", "Warsaw Pact", "Berlin", "Moscow",
            "Washington", "Beijing", "London", "Tokyo", "Paris"
        };

        [Fact]
        public void SignalIdentifiers_AreLocalizationKeySafe()
        {
            var signals = LoadSignals();
            var keyRegex = new Regex(@"^[a-z0-9_]+$", RegexOptions.Compiled);

            foreach (var sig in signals)
            {
                Assert.False(string.IsNullOrWhiteSpace(sig.FrequencyId));
                Assert.Matches(keyRegex, sig.FrequencyId);
            }
        }

        [Fact]
        public void SourceNames_AreNonEmpty_Trimmed_AndClean()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                Assert.False(string.IsNullOrWhiteSpace(sig.SourceName),
                    $"Signal '{sig.FrequencyId}' has empty or whitespace source name");
                Assert.Equal(sig.SourceName.Trim(), sig.SourceName);
                Assert.DoesNotContain("\t", sig.SourceName);
                Assert.DoesNotContain("\n", sig.SourceName);
                Assert.DoesNotContain("\r", sig.SourceName);
            }
        }

        [Fact]
        public void MessageFragments_ContainNoForbiddenRealWorldEntities()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                foreach (var frag in sig.MessageFragments)
                {
                    foreach (var forbidden in ForbiddenRealWorldTerms)
                    {
                        var pattern = $@"\b{Regex.Escape(forbidden)}\b";
                        Assert.False(Regex.IsMatch(frag.Text, pattern, RegexOptions.IgnoreCase),
                            $"Signal '{sig.FrequencyId}' Day {frag.Day} contains forbidden real-world entity '{forbidden}'");
                    }
                }
            }
        }

        [Fact]
        public void MessageFragments_HaveValidTerminalPunctuation()
        {
            var signals = LoadSignals();
            char[] validTerminators = { '.', '!', '?', '*', '"', '\'' };

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                foreach (var frag in sig.MessageFragments)
                {
                    string trimmed = frag.Text.Trim();
                    Assert.NotEmpty(trimmed);
                    char lastChar = trimmed[^1];
                    Assert.Contains(lastChar, validTerminators);
                }
            }
        }

        [Fact]
        public void MessageFragments_AdhereToSentenceAndCharacterBudgets()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                foreach (var frag in sig.MessageFragments)
                {
                    // Character length limit for dialogue panels (max 350 chars before localization)
                    Assert.True(frag.Text.Length <= 350,
                        $"Signal '{sig.FrequencyId}' Day {frag.Day} exceeds 350 chars ({frag.Text.Length})");

                    // Max 3 sentences per fragment (handling ellipses properly)
                    int sentenceCount = CountSentences(frag.Text);
                    Assert.True(sentenceCount <= 3,
                        $"Signal '{sig.FrequencyId}' Day {frag.Day} exceeds 3 sentences ({sentenceCount})");
                }
            }
        }

        private static int CountSentences(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string cleaned = text.Trim('*').Trim();
            // Replace decimal coordinates like 41.7
            cleaned = Regex.Replace(cleaned, @"\b\d+\.\d+\b", "DECIMAL");
            // Replace ellipses
            cleaned = Regex.Replace(cleaned, @"\.{3,}", " ");
            // Split on sentence terminators
            var raw = cleaned.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            return raw.Count(s => s.Any(char.IsLetterOrDigit));
        }

        [Fact]
        public void TranslatableMetadata_AreStructuredStrings()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                if (!string.IsNullOrEmpty(sig.WarningText))
                {
                    Assert.Equal(sig.WarningText.Trim(), sig.WarningText);
                    Assert.True(sig.WarningText.Length <= 200,
                        $"Signal '{sig.FrequencyId}' warning text exceeds 200 chars");
                }

                if (!string.IsNullOrEmpty(sig.ToneRegister))
                {
                    Assert.Matches(@"^[a-z0-9_]+$", sig.ToneRegister);
                }
            }
        }

        [Fact]
        public void UTF8Encoding_ContainsNoUnprintableControlCharacters()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                CheckStringClean(sig.SourceName, sig.FrequencyId, "source_name");
                CheckStringClean(sig.WarningText, sig.FrequencyId, "warning_text");

                if (sig.MessageFragments != null)
                {
                    foreach (var frag in sig.MessageFragments)
                    {
                        CheckStringClean(frag.Text, sig.FrequencyId, $"fragment_{frag.Day}");
                    }
                }
            }
        }

        private static void CheckStringClean(string text, string signalId, string fieldName)
        {
            if (string.IsNullOrEmpty(text)) return;

            foreach (char c in text)
            {
                if (char.IsControl(c) && c != '\n' && c != '\r' && c != '\t')
                {
                    Assert.Fail($"Signal '{signalId}' field '{fieldName}' contains non-printable control char U+{(int)c:X4}");
                }
            }
        }

        [Fact]
        public void LocalizationKeyInventory_IsCompleteAndUnique()
        {
            var signals = LoadSignals();
            var keys = new HashSet<string>(StringComparer.Ordinal);

            foreach (var sig in signals)
            {
                string sourceKey = $"distress.{sig.FrequencyId}.source_name";
                Assert.True(keys.Add(sourceKey), $"Duplicate localization key: {sourceKey}");

                if (!string.IsNullOrEmpty(sig.WarningText))
                {
                    string warnKey = $"distress.{sig.FrequencyId}.warning";
                    Assert.True(keys.Add(warnKey), $"Duplicate localization key: {warnKey}");
                }

                if (sig.MessageFragments != null)
                {
                    foreach (var frag in sig.MessageFragments)
                    {
                        string fragKey = $"distress.{sig.FrequencyId}.frag_{frag.Day}";
                        Assert.True(keys.Add(fragKey), $"Duplicate localization key: {fragKey}");
                    }
                }
            }

            // Exactly 25 source names + fragments across all signals
            Assert.True(keys.Count >= 25 + 25);
        }
    }
}
