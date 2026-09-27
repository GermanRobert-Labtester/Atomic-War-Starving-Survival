// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Localization
{
    /// <summary>
    /// Locale contract gate over the full assets/l10n/strings.csv catalog:
    /// header contract, key uniqueness, non-empty English and German for every
    /// row, and {placeholder} parity between en and de. Extends the pilot-prefix
    /// coverage of LocalizationPilotTests to the entire catalog; the runtime
    /// loader (LocalizationService.LoadFromCsv) consumes exactly this shape.
    /// </summary>
    public sealed class StringsCsvLocaleGateTests
    {
        private static readonly Regex PlaceholderPattern = new(
            @"\{([A-Za-z_][A-Za-z0-9_]*|\d+)(?::[^}]*)?\}", RegexOptions.Compiled);

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

        private static List<string[]> ReadRows()
        {
            string path = Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv");
            var rows = new List<string[]>();
            foreach (string line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                    continue;
                rows.Add(ParseCsvLine(line));
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

        private static HashSet<string> Placeholders(string value)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (Match match in PlaceholderPattern.Matches(value))
                set.Add(match.Groups[1].Value);
            return set;
        }

        [Fact]
        public void Catalog_HeaderIsKeyEnDeSource()
        {
            var rows = ReadRows();
            Assert.NotEmpty(rows);
            Assert.Equal(new[] { "key", "en", "de", "source" }, rows[0]);
        }

        [Fact]
        public void Catalog_EveryRowHasFourFields_UniqueKeys_NonEmptyEnglishAndGerman_PlaceholderParity()
        {
            var data = ReadRows().Skip(1).ToList();
            Assert.NotEmpty(data);

            var errors = new List<string>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (string[] row in data)
            {
                string key = row.Length > 0 ? row[0] : "<empty row>";
                if (row.Length != 4)
                {
                    errors.Add($"{key}: expected 4 fields, found {row.Length}");
                    continue;
                }
                if (!seen.Add(row[0]))
                {
                    errors.Add($"{row[0]}: duplicate key");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row[1]))
                    errors.Add($"{row[0]}: missing English");
                if (string.IsNullOrWhiteSpace(row[2]))
                    errors.Add($"{row[0]}: missing German");
                var enPlaceholders = Placeholders(row[1]);
                var dePlaceholders = Placeholders(row[2]);
                if (!enPlaceholders.SetEquals(dePlaceholders))
                    errors.Add(
                        $"{row[0]}: placeholder mismatch en=[{string.Join(",", enPlaceholders)}] " +
                        $"de=[{string.Join(",", dePlaceholders)}]");
            }

            Assert.True(
                errors.Count == 0,
                "strings.csv locale gate failures: " + string.Join("; ", errors.Take(25)) +
                (errors.Count > 25 ? $" (+{errors.Count - 25} more)" : string.Empty));
        }
    }
}
