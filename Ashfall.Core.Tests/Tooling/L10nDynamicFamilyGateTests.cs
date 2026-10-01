// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// L01 gate: the localization drift gate must derive every
    /// dynamically-constructed key family from its authoritative catalog, so a
    /// new onboarding stage / achievement / micro-location without its en+de
    /// rows fails the gate instead of silently shipping untranslated (the
    /// ae6e54387 "HINT: —" class, generalised to all families).
    /// </summary>
    public sealed class L10nDynamicFamilyGateTests
    {
        private static string RepoRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                if (File.Exists(Path.Combine(current, "docs", "ci", "CI_GATE_MANIFEST.json")))
                    return current;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("repository root not found");
        }

        private static string Read(string relative) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relative));

        [Fact]
        public void DriftGate_DerivesEveryDynamicFamilyFromItsCatalog()
        {
            string gate = Read("scripts/ci/l10n_drift_gate.py");
            foreach (string needle in new[]
            {
                "DYNAMIC_FAMILIES",
                "achievements.json",
                "micro_locations.json",
                "OnboardingJourney.cs",
                "onboarding_stage", "onboarding_hint", "achievement", "micro_discovery",
                // Sweep-5 guard: a family that enumerates zero keys must fail
                // the gate rather than silently dropping its whole coverage.
                "enumerated zero keys",
            })
            {
                Assert.Contains(needle, gate, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void EveryAchievement_HasNameAndDescriptionRows()
        {
            var csv = CsvKeys();
            using var doc = JsonDocument.Parse(Read("Assets/StreamingAssets/Data/achievements.json"));
            var missing = new List<string>();
            foreach (var a in doc.RootElement.GetProperty("achievements").EnumerateArray())
            {
                string id = a.GetProperty("id").GetString()!;
                if (!csv.Contains($"achievement.{id}.name")) missing.Add($"achievement.{id}.name");
                if (!csv.Contains($"achievement.{id}.description")) missing.Add($"achievement.{id}.description");
            }
            Assert.True(missing.Count == 0, "achievement rows missing: " + string.Join(", ", missing));
        }

        [Fact]
        public void EveryMicroLocation_HasTitleDescriptionAndChoiceRows()
        {
            var csv = CsvKeys();
            using var doc = JsonDocument.Parse(Read("Assets/StreamingAssets/Data/micro_locations.json"));
            var missing = new List<string>();
            foreach (var e in doc.RootElement.GetProperty("encounters").EnumerateArray())
            {
                string id = e.GetProperty("id").GetString()!;
                if (!csv.Contains($"discovery.{id}.title")) missing.Add($"discovery.{id}.title");
                if (!csv.Contains($"discovery.{id}.description")) missing.Add($"discovery.{id}.description");
                foreach (var c in e.GetProperty("choices").EnumerateArray())
                {
                    string cid = c.GetProperty("choiceId").GetString()!;
                    if (!csv.Contains($"discovery.{id}.choice.{cid}")) missing.Add($"discovery.{id}.choice.{cid}");
                }
            }
            Assert.True(missing.Count == 0, "micro-location rows missing: " + string.Join(", ", missing));
        }

        [Fact]
        public void DynamicFamilyRows_CarryNonEmptyGerman()
        {
            var german = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string line in Read("assets/l10n/strings.csv").Split('\n').Skip(1))
            {
                string l = line.TrimEnd('\r');
                if (l.Length == 0) continue;
                var cols = SplitCsv(l);
                if (cols.Count >= 3 && cols[0].Length > 0)
                    german[cols[0]] = cols[2];
            }

            using var doc = JsonDocument.Parse(Read("Assets/StreamingAssets/Data/achievements.json"));
            var missing = new List<string>();
            foreach (var a in doc.RootElement.GetProperty("achievements").EnumerateArray())
            {
                string id = a.GetProperty("id").GetString()!;
                foreach (string suffix in new[] { ".name", ".description" })
                {
                    string key = $"achievement.{id}{suffix}";
                    if (!german.TryGetValue(key, out string? de) || string.IsNullOrWhiteSpace(de))
                        missing.Add(key);
                }
            }
            Assert.True(missing.Count == 0, "achievement German missing: " + string.Join(", ", missing));
        }

        private static HashSet<string> CsvKeys()
        {
            var keys = new HashSet<string>(StringComparer.Ordinal);
            foreach (string line in Read("assets/l10n/strings.csv").Split('\n').Skip(1))
            {
                string l = line.TrimEnd('\r');
                int comma = l.IndexOf(',');
                if (comma > 0) keys.Add(l[..comma]);
            }
            return keys;
        }

        private static List<string> SplitCsv(string line)
        {
            var cols = new List<string>();
            var current = new System.Text.StringBuilder();
            bool quoted = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (quoted && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
                    else quoted = !quoted;
                }
                else if (c == ',' && !quoted) { cols.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            cols.Add(current.ToString());
            return cols;
        }
    }
}