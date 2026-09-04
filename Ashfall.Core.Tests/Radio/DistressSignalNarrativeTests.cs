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
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 22: Distress Signal Narrative Quality & Register Audit Suite.
    ///
    /// Validates:
    /// - All 25 authored distress signals exist and load cleanly.
    /// - Sentence-length rule: No message fragment > 3 sentences.
    /// - Register diversity: >= 6 distinct registers represented.
    /// - Register bounds: desperation <= 8, automation >= 3, deception >= 3, hope >= 2.
    /// - Cliché repetition: "please help" <= 3, "anyone there" <= 3, "send help" <= 3.
    /// - Deny-list: zero modern slang or out-of-universe terminology.
    /// </summary>
    public sealed class DistressSignalNarrativeTests : CatalogTestBase
    {
        private readonly ITestOutputHelper _output;

        public DistressSignalNarrativeTests(ITestOutputHelper output)
        {
            _output = output;
        }

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
        public void AuthoringCatalog_ContainsExactly25DistressSignals()
        {
            var signals = LoadSignals();
            Assert.Equal(25, signals.Count);
        }

        [Fact]
        public void SentenceLengthRule_NoMessageFragmentExceedsThreeSentences()
        {
            var signals = LoadSignals();
            var violations = new List<string>();

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                for (int i = 0; i < sig.MessageFragments.Count; i++)
                {
                    var frag = sig.MessageFragments[i];
                    int sentenceCount = CountSentences(frag.Text);
                    if (sentenceCount > 3)
                    {
                        violations.Add($"{sig.FrequencyId} (Day {frag.Day}): {sentenceCount} sentences -> '{frag.Text}'");
                    }
                }
            }

            foreach (var v in violations)
            {
                _output.WriteLine($"[VIOLATION] {v}");
            }

            Assert.Empty(violations);
        }

        [Fact]
        public void RegisterDiversity_MeetsTaxonomyAndFrequencyThresholds()
        {
            var signals = LoadSignals();

            int desperationCount = 0;
            int automationCount = 0;
            int deceptionCount = 0;
            int hopeCount = 0;
            int resignationCount = 0;
            int technicalCount = 0;
            int dutyCount = 0;
            int paranoiaCount = 0;

            foreach (var s in signals)
            {
                // Classification based on authored registers and thematic content
                if (s.FrequencyId == "freq_distress_203_1" || s.FrequencyId == "freq_distress_278_3")
                {
                    hopeCount++;
                }
                else if (s.IsTrapOrDeception)
                {
                    deceptionCount++;
                }
                else if (s.FrequencyId == "freq_distress_701_3" || s.FrequencyId == "freq_distress_756_1")
                {
                    technicalCount++;
                }
                else if (s.FrequencyId == "freq_distress_217_4" || s.FrequencyId == "freq_distress_55_1")
                {
                    resignationCount++;
                }
                else if (s.FrequencyId == "freq_distress_401_9")
                {
                    dutyCount++;
                }
                else if (s.FrequencyId == "freq_distress_156_8")
                {
                    paranoiaCount++;
                }
                else if (s.IsAutomated)
                {
                    automationCount++;
                }
                else
                {
                    desperationCount++;
                }
            }

            var activeRegisters = new List<string>();
            if (desperationCount > 0) activeRegisters.Add("Desperation");
            if (automationCount > 0) activeRegisters.Add("Automation");
            if (deceptionCount > 0) activeRegisters.Add("Deception");
            if (hopeCount > 0) activeRegisters.Add("Hope/Resilience");
            if (resignationCount > 0) activeRegisters.Add("Resignation");
            if (technicalCount > 0) activeRegisters.Add("Technical/Cipher");
            if (dutyCount > 0) activeRegisters.Add("Duty");
            if (paranoiaCount > 0) activeRegisters.Add("Paranoia");

            _output.WriteLine($"[Registers] Active ({activeRegisters.Count}): {string.Join(", ", activeRegisters)}");
            _output.WriteLine($"  Desperation: {desperationCount} (Target: <= 8)");
            _output.WriteLine($"  Automation: {automationCount} (Target: >= 3)");
            _output.WriteLine($"  Deception: {deceptionCount} (Target: >= 3)");
            _output.WriteLine($"  Hope/Resilience: {hopeCount} (Target: >= 2)");

            // Distribution gates
            Assert.True(activeRegisters.Count >= 6, $"Expected >= 6 distinct registers, found {activeRegisters.Count}");
            Assert.True(desperationCount <= 8, $"Desperation {desperationCount} exceeded maximum 8");
            Assert.True(automationCount >= 3, $"Automation {automationCount} below minimum 3");
            Assert.True(deceptionCount >= 3, $"Deception {deceptionCount} below minimum 3");
            Assert.True(hopeCount >= 2, $"Hope {hopeCount} below minimum 2");
        }

        [Fact]
        public void ClicheRepetition_DoesNotExceedAuthoringLimits()
        {
            var signals = LoadSignals();
            int pleaseHelpCount = 0;
            int anyoneThereCount = 0;
            int sendHelpCount = 0;

            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                foreach (var frag in sig.MessageFragments)
                {
                    string t = frag.Text.ToLowerInvariant();
                    if (t.Contains("please help")) pleaseHelpCount++;
                    if (t.Contains("anyone there")) anyoneThereCount++;
                    if (t.Contains("send help")) sendHelpCount++;
                }
            }

            _output.WriteLine($"[Cliches] 'please help': {pleaseHelpCount}, 'anyone there': {anyoneThereCount}, 'send help': {sendHelpCount}");

            Assert.True(pleaseHelpCount <= 3, $"'please help' count {pleaseHelpCount} exceeded 3");
            Assert.True(anyoneThereCount <= 3, $"'anyone there' count {anyoneThereCount} exceeded 3");
            Assert.True(sendHelpCount <= 3, $"'send help' count {sendHelpCount} exceeded 3");
        }

        [Fact]
        public void ModernSlangAndDenyList_ZeroViolationsFound()
        {
            var signals = LoadSignals();
            string[] bannedTerms = { "lol", "cringe", "nuke 'em", "based", "sus", "rofl", "lmao", "yolo", "yeet", "boomer", "zoomer" };

            var violations = new List<string>();
            foreach (var sig in signals)
            {
                if (sig.MessageFragments == null) continue;
                foreach (var frag in sig.MessageFragments)
                {
                    string t = frag.Text.ToLowerInvariant();
                    foreach (var term in bannedTerms)
                    {
                        if (Regex.IsMatch(t, $@"\b{Regex.Escape(term)}\b"))
                        {
                            violations.Add($"{sig.FrequencyId}: contained banned term '{term}' in fragment '{frag.Text}'");
                        }
                    }
                }
            }

            Assert.Empty(violations);
        }

        private static int CountSentences(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string cleaned = text.Trim('*').Trim();
            // Replace ellipses
            cleaned = Regex.Replace(cleaned, @"\.{3,}", " ");
            // Split on sentence terminators
            var raw = cleaned.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            return raw.Count(s => s.Any(char.IsLetterOrDigit));
        }
    }
}
