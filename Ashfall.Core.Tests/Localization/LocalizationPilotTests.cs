// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Localization;
using Ashfall.Core.Settings;
using Xunit;

namespace Ashfall.Core.Tests.Localization
{
    public sealed class LocalizationPilotTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "assets", "l10n", "strings.csv")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL localization catalog.");
        }

        [Fact]
        public void Wave1Catalog_HasUniquePilotKeysAndGermanTranslations()
        {
            string path = Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv");
            var rows = File.ReadAllLines(path)
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseCsv)
                .ToList();

            Assert.Equal(rows.Count, rows.Select(row => row[0]).Distinct(StringComparer.Ordinal).Count());

            string[] pilotPrefixes = { "research.", "onboarding.", "ui.common.close_short" };
            var pilotRows = rows.Where(row => pilotPrefixes.Any(prefix =>
                row[0].Equals(prefix, StringComparison.Ordinal) ||
                row[0].StartsWith(prefix, StringComparison.Ordinal))).ToList();

            Assert.NotEmpty(pilotRows);
            Assert.All(pilotRows, row =>
            {
                Assert.False(string.IsNullOrWhiteSpace(row[1]), $"English missing for {row[0]}");
                Assert.False(string.IsNullOrWhiteSpace(row[2]), $"German missing for {row[0]}");
            });
        }

        [Fact]
        public void Service_LoadsEnglishAndGermanPilotStringsWithFallback()
        {
            var loc = new LocalizationService();
            loc.RegisterString("research.title", "RESEARCH & TECHNOLOGY // QUEUE");
            loc.RegisterTranslation("de", "research.title", "FORSCHUNG & TECHNOLOGIE // WARTESCHLANGE");

            Assert.Equal("RESEARCH & TECHNOLOGY // QUEUE", loc.Get("research.title"));
            loc.SetLocale("de");
            Assert.Equal("FORSCHUNG & TECHNOLOGIE // WARTESCHLANGE", loc.Get("research.title"));
            Assert.Equal("English fallback", loc.Get("missing.secondary", "English fallback"));
        }

        [Fact]
        public void FormatNamed_PreservesTranslatedWordOrder()
        {
            var loc = new LocalizationService();
            loc.RegisterString("research.progress", "Day {day} / {total}: {remaining} remaining");
            loc.RegisterTranslation("de", "research.progress", "{remaining} verbleiben — Tag {day} / {total}");
            loc.SetLocale("de");

            string text = loc.FormatNamed("research.progress", new Dictionary<string, object?>
            {
                ["day"] = 2,
                ["total"] = 5,
                ["remaining"] = "3 days"
            });

            Assert.Equal("3 days verbleiben — Tag 2 / 5", text);
        }

        [Fact]
        public void SettingsCodec_AcceptsGermanLocale()
        {
            var data = new UserSettingsData { Locale = "de" };
            var sanitized = UserSettingsCodec.Sanitize(data, out string? diagnostic);

            Assert.Equal("de", sanitized.Locale);
            Assert.Null(diagnostic);
        }

        private static string[] ParseCsv(string line)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool quoted = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"') { quoted = !quoted; continue; }
                if (c == ',' && !quoted)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                    continue;
                }
                current.Append(c);
            }
            fields.Add(current.ToString());
            while (fields.Count < 4) fields.Add(string.Empty);
            return fields.ToArray();
        }
    }
}
