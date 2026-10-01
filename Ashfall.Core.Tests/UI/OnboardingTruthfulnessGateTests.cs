// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Onboarding truthfulness (T03 — regression guard for the
// ae6e54387 fix class).
//
// ae6e54387 fixed the Duty/Dose first-hour stages rendering "HINT: —" by
// adding the missing onboarding.* localization rows and the two hint switch
// arms. That class of silent failure — a stage present in OnboardingCatalog
// but absent from the hint copy or the localization catalog — is what this
// gate now prevents: every stage in either profile must have authored,
// localized title, objective, and hint copy, and the hint panel must never
// fabricate the empty "HINT: —" sentinel for a real stage.
//
// The guard is deliberately catalog-driven (Enum.GetValues<OnboardingStage>())
// rather than a hardcoded key list, so adding a stage without its copy fails
// here instead of shipping a blank objective or a sentinel hint.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using Ashfall.Core.Onboarding;

namespace Ashfall.Core.Tests.UI
{
    public sealed class OnboardingTruthfulnessGateTests
    {
        private static readonly Regex HintEntryPattern = new Regex(
            @"\[\s*OnboardingStage\.(?<stage>[A-Za-z0-9_]+)\s*\]\s*=\s*\(\s*""(?<key>[^""]+)""\s*,\s*""(?<fallback>[^""]*)""\s*\)",
            RegexOptions.Compiled);

        private static readonly Regex BlockComment = new Regex(@"/\*.*?\*/", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex LineComment = new Regex(@"//.*$", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>Removes comments so the sentinel scan reads code, not prose.</summary>
        private static string StripComments(string code)
        {
            code = BlockComment.Replace(code, string.Empty);
            return LineComment.Replace(code, string.Empty);
        }

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "assets", "l10n", "strings.csv"))
                    && File.Exists(Path.Combine(dir.FullName, "src", "UI", "OnboardingHintPanel.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string PanelSource() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "src", "UI", "OnboardingHintPanel.cs"));

        /// <summary>Mirrors OnboardingHintPanel.StageLocalizationId (the pinned contract).</summary>
        private static string StageLocalizationId(OnboardingStage id) => id switch
        {
            OnboardingStage.InventoryUse => "inventory_use",
            OnboardingStage.DayAdvance => "day_advance",
            _ => id.ToString().ToLowerInvariant(),
        };

        private static Dictionary<string, (string En, string De)> ReadCatalog()
        {
            string path = Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv");
            var rows = new Dictionary<string, (string, string)>(StringComparer.Ordinal);
            foreach (string line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] fields = ParseCsvLine(line);
                if (fields.Length < 3) continue;
                string key = fields[0];
                if (key.Length == 0 || key.Equals("key", StringComparison.Ordinal)) continue;
                rows[key] = (fields[1], fields[2]);
            }
            return rows;
        }

        /// <summary>Mirrors LocalizationService.ParseCsvLine (quote-aware; "" escapes a quote).</summary>
        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            fields.Add(current.ToString());
            return fields.ToArray();
        }

        /// <summary>
        /// Parses the single authoritative StageHintCopy map from the panel
        /// source. Returns an empty map if the map is absent or renamed, which
        /// the callers treat as a hard failure (never a silent pass).
        /// </summary>
        private static Dictionary<OnboardingStage, (string Key, string Fallback)> ReadHintMap()
        {
            var map = new Dictionary<OnboardingStage, (string, string)>();
            foreach (Match match in HintEntryPattern.Matches(PanelSource()))
            {
                if (!Enum.TryParse(match.Groups["stage"].Value, out OnboardingStage stage))
                    continue;
                map[stage] = (match.Groups["key"].Value, match.Groups["fallback"].Value);
            }
            return map;
        }

        private static void CheckRow(
            Dictionary<string, (string En, string De)> catalog,
            string key,
            OnboardingStage stage,
            List<string> errors)
        {
            if (!catalog.TryGetValue(key, out var row))
            {
                errors.Add($"{stage}: missing localization row {key}");
                return;
            }
            if (string.IsNullOrWhiteSpace(row.En))
                errors.Add($"{stage}: empty English in {key}");
            if (string.IsNullOrWhiteSpace(row.De))
                errors.Add($"{stage}: empty German in {key}");
        }

        [Fact]
        public void EveryOnboardingStage_HasNonEmptyLocalizedTitleObjectiveAndHint()
        {
            var catalog = ReadCatalog();
            var hintMap = ReadHintMap();
            var stages = Enum.GetValues<OnboardingStage>();
            var errors = new List<string>();

            foreach (var stage in stages)
            {
                string id = StageLocalizationId(stage);
                CheckRow(catalog, $"onboarding.{id}.title", stage, errors);
                CheckRow(catalog, $"onboarding.{id}.objective", stage, errors);

                if (!hintMap.TryGetValue(stage, out var hint))
                    errors.Add($"{stage}: no hint mapping in OnboardingHintPanel");
                else
                    CheckRow(catalog, hint.Key, stage, errors);
            }

            Assert.True(errors.Count == 0,
                "Onboarding stages without complete localized copy (the ae6e54387 failure class):\n"
                + string.Join("\n", errors));
        }

        [Fact]
        public void EveryOnboardingStage_HasAnAuthoredHintFallback_NotTheEmptySentinel()
        {
            var hintMap = ReadHintMap();
            var stages = Enum.GetValues<OnboardingStage>();
            var errors = new List<string>();

            foreach (var stage in stages)
            {
                if (!hintMap.TryGetValue(stage, out var hint))
                {
                    errors.Add($"{stage}: no hint mapping");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(hint.Fallback))
                    errors.Add($"{stage}: empty hint fallback");
                if (!hint.Key.StartsWith("onboarding.hint.", StringComparison.Ordinal))
                    errors.Add($"{stage}: unexpected hint key {hint.Key}");
                if (string.Equals(hint.Key, "onboarding.hint.empty", StringComparison.Ordinal))
                    errors.Add($"{stage}: maps to the empty sentinel key");
            }

            // Distinct keys: a stage must not silently borrow another stage's hint.
            var borrowed = hintMap
                .GroupBy(pair => pair.Value.Key, StringComparer.Ordinal)
                .Where(group => group.Count() > 1)
                .Select(group => $"{group.Key} <- {string.Join(",", group.Select(p => p.Key))}");
            errors.AddRange(borrowed);

            Assert.True(errors.Count == 0,
                "Onboarding hint map is incomplete or untruthful:\n" + string.Join("\n", errors));
        }

        [Fact]
        public void HintPanel_NeverFabricatesTheEmptyHintSentinel()
        {
            string source = PanelSource();
            string code = StripComments(source);

            Assert.True(!code.Contains("\"HINT: —\"", StringComparison.Ordinal),
                "OnboardingHintPanel must not contain the fabricated \"HINT: —\" sentinel.");

            int start = source.IndexOf("private static string BuildHintLine", StringComparison.Ordinal);
            Assert.True(start >= 0, "BuildHintLine not found in OnboardingHintPanel.cs");
            int end = source.IndexOf("private static string T(", start, StringComparison.Ordinal);
            Assert.True(end > start, "BuildHintLine terminator not found in OnboardingHintPanel.cs");
            string body = StripComments(source.Substring(start, end - start));

            Assert.True(!body.Contains("onboarding.hint.empty", StringComparison.Ordinal),
                "BuildHintLine must not route a stage through the empty sentinel key.");
            Assert.True(body.Contains("StageHintCopy", StringComparison.Ordinal),
                "BuildHintLine must resolve stage copy through the single StageHintCopy map.");
        }

        [Fact]
        public void StatusBar_SharesThePanelStageLocalizationId()
        {
            string main = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Onboarding.cs"));

            Assert.True(
                main.Contains("OnboardingHintPanel.StageLocalizationId(def.Id)", StringComparison.Ordinal),
                "The onboarding status bar must derive its stage id from the panel's single "
                + "StageLocalizationId normalization so both surfaces name the same row.");
        }

        [Fact]
        public void StatusBar_OnJourneyCompletion_ShowsTheCompleteLineNotAStaleObjective()
        {
            string main = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Onboarding.cs"));

            // The completion branch must author the complete-state copy, never
            // silently return and leave the last "CURRENT: …" objective on the
            // status label as a live claim after the journey is done.
            Assert.True(
                main.Contains("\"onboarding.status.first_hour_complete\"", StringComparison.Ordinal),
                "RefreshOnboardingStatusBar must render the localized completion copy "
                + "(onboarding.status.first_hour_complete) once the journey is complete "
                + "instead of returning early and leaving a stale final objective.");

            int methodStart = main.IndexOf("private void RefreshOnboardingStatusBar()", StringComparison.Ordinal);
            Assert.True(methodStart >= 0, "RefreshOnboardingStatusBar not found in Main.Onboarding.cs.");
            int methodEnd = main.IndexOf("// ── ", methodStart, StringComparison.Ordinal);
            if (methodEnd <= methodStart)
                methodEnd = main.Length;
            string body = main.Substring(methodStart, methodEnd - methodStart);

            int completeIf = body.IndexOf("if (j.JourneyComplete)", StringComparison.Ordinal);
            Assert.True(completeIf >= 0, "Journey-complete branch not found in RefreshOnboardingStatusBar.");
            string afterComplete = body.Substring(completeIf);
            Assert.True(afterComplete.Contains("_statusLabel.Text", StringComparison.Ordinal),
                "The journey-complete branch must write the status label (completion copy), "
                + "not just return and abandon the last objective on the line.");
        }
    }
}
