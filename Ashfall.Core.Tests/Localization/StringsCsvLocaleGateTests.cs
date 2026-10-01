// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ashfall.Core.Localization;
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

        /// <summary>
        /// Runtime-path parity: <see cref="Ashfall.Core.Localization.LocalizationService.LoadFromCsv"/>
        /// overwrites Core's registered defaults, so the Godot UI renders the
        /// CSV value for a key. The JSON catalog is authoritative for the
        /// source English text; a prose trim that lands in the JSON and in
        /// LocalizationService but not in the CSV silently regresses the panel
        /// (observed 2026-10-01: discovery.micro_frozen_bus.description).
        /// </summary>
        [Fact]
        public void Catalog_MicroLocationEnglishMatchesAuthoritativeJson()
        {
            string dataPath = Path.Combine(
                RepoRoot(), "Assets", "StreamingAssets", "Data", "micro_locations.json");
            Assert.True(File.Exists(dataPath), "missing authoritative micro_locations.json");

            var rows = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string[] row in ReadRows().Skip(1))
            {
                if (row.Length >= 2 && !string.IsNullOrEmpty(row[0]))
                    rows[row[0]] = row[1];
            }

            using var doc = JsonDocument.Parse(File.ReadAllText(dataPath));
            var encounters = doc.RootElement.GetProperty("encounters");
            var errors = new List<string>();
            foreach (var encounter in encounters.EnumerateArray())
            {
                string id = encounter.GetProperty("id").GetString() ?? string.Empty;
                CompareCsvToJson(rows, $"discovery.{id}.title",
                    encounter.GetProperty("title").GetString() ?? string.Empty, errors);
                CompareCsvToJson(rows, $"discovery.{id}.description",
                    encounter.GetProperty("description").GetString() ?? string.Empty, errors);

                if (!encounter.TryGetProperty("choices", out var choices))
                    continue;
                foreach (var choice in choices.EnumerateArray())
                {
                    string choiceId = choice.GetProperty("choiceId").GetString() ?? string.Empty;
                    CompareCsvToJson(rows, $"discovery.{id}.choice.{choiceId}",
                        choice.GetProperty("text").GetString() ?? string.Empty, errors);
                }
            }

            Assert.True(
                errors.Count == 0,
                "strings.csv drifted from micro_locations.json (JSON is authoritative): " +
                string.Join("; ", errors.Take(25)) +
                (errors.Count > 25 ? $" (+{errors.Count - 25} more)" : string.Empty));
        }

        private static void CompareCsvToJson(
            IReadOnlyDictionary<string, string> rows,
            string key,
            string expected,
            List<string> errors)
        {
            if (!rows.TryGetValue(key, out string? actual))
            {
                errors.Add($"{key}: missing from strings.csv");
                return;
            }

            if (!string.Equals(actual, expected, StringComparison.Ordinal))
                errors.Add($"{key}: strings.csv EN does not match JSON");
        }

        /// <summary>
        /// Authority-parity gate: the runtime catalog (strings.csv) overrides
        /// Core's registered defaults, so the two copies must agree. The Task 6
        /// finding (2026-09-27) recorded that Core duplicates the micro-location
        /// strings in code and that the CSV should be the single source; this
        /// gate makes any future divergence loud instead of silently changing
        /// what the player sees.
        /// </summary>
        [Fact]
        public void Catalog_MicroLocationMatchesCoreDefaults_EnglishAndGerman()
        {
            string csvPath = Path.Combine(RepoRoot(), "assets", "l10n", "strings.csv");
            string dataPath = Path.Combine(
                RepoRoot(), "Assets", "StreamingAssets", "Data", "micro_locations.json");

            var keys = MicroLocationKeys(dataPath);
            var core = new LocalizationService();
            var catalog = new LocalizationService();
            catalog.LoadFromCsv(File.ReadAllText(csvPath));

            var coreEn = Snapshot(core, "en", keys);
            var coreDe = Snapshot(core, "de", keys);
            var csvEn = Snapshot(catalog, "en", keys);
            var csvDe = Snapshot(catalog, "de", keys);

            var errors = new List<string>();
            foreach (string key in keys)
            {
                if (!coreEn.ContainsKey(key))
                {
                    errors.Add($"{key}: not registered in Core defaults");
                    continue;
                }

                if (!string.Equals(coreEn[key], csvEn[key], StringComparison.Ordinal))
                    errors.Add($"{key}: CSV EN overrides Core EN with different text");

                // Core falls back to EN when it has no German translation; only
                // a distinct German default is an authority the CSV must not
                // override (extra CSV-only German coverage is allowed).
                if (!string.Equals(coreDe[key], coreEn[key], StringComparison.Ordinal) &&
                    !string.Equals(coreDe[key], csvDe[key], StringComparison.Ordinal))
                {
                    errors.Add($"{key}: CSV DE overrides Core DE with different text");
                }
            }

            Assert.True(
                errors.Count == 0,
                "strings.csv and Core LocalizationService defaults diverged: " +
                string.Join("; ", errors.Take(25)) +
                (errors.Count > 25 ? $" (+{errors.Count - 25} more)" : string.Empty));
        }

        private static List<string> MicroLocationKeys(string dataPath)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(dataPath));
            var keys = new List<string>();
            foreach (var encounter in doc.RootElement.GetProperty("encounters").EnumerateArray())
            {
                string id = encounter.GetProperty("id").GetString() ?? string.Empty;
                keys.Add($"discovery.{id}.title");
                keys.Add($"discovery.{id}.description");
                if (!encounter.TryGetProperty("choices", out var choices))
                    continue;
                foreach (var choice in choices.EnumerateArray())
                {
                    string choiceId = choice.GetProperty("choiceId").GetString() ?? string.Empty;
                    keys.Add($"discovery.{id}.choice.{choiceId}");
                }
            }
            return keys;
        }

        private static Dictionary<string, string> Snapshot(
            LocalizationService service, string locale, IReadOnlyList<string> keys)
        {
            service.SetLocale(locale);
            var snapshot = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string key in keys)
                snapshot[key] = service.Get(key);
            return snapshot;
        }
    }
}
