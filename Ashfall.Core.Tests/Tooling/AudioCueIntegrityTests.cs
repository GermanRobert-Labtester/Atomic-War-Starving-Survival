using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Mechanically verifies that all audio cue constants, buses, and registrations
    /// in AudioCueCatalog.cs adhere to canonical naming, uniqueness, and bus topology.
    /// </summary>
    public class AudioCueIntegrityTests
    {
        private static string GetRepositoryRoot()
        {
            string current = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(current))
            {
                if (File.Exists(Path.Combine(current, "project.godot")))
                    return current;
                current = Directory.GetParent(current)?.FullName ?? string.Empty;
            }
            throw new InvalidOperationException("Could not locate repository root from BaseDirectory: " + AppContext.BaseDirectory);
        }

        private static string CatalogPath => Path.Combine(GetRepositoryRoot(), "src", "Audio", "AudioCueCatalog.cs");

        private static readonly HashSet<string> ValidBusIdentifiers = new(StringComparer.OrdinalIgnoreCase)
        {
            "Master", "Music", "Ambience", "SFX", "UI", "Voice",
            "Alerts", "Generator", "Ventilation", "Radio", "Medical", "Surface",
            "Machinery", "ShelterSocial", "Subterranean"
        };

        [Fact]
        public void AudioCueCatalog_FileExists()
        {
            Assert.True(File.Exists(CatalogPath), $"AudioCueCatalog.cs must exist at {CatalogPath}");
        }

        [Fact]
        public void AudioCueCatalog_AllConstants_AreSnakeCase()
        {
            var content = File.ReadAllText(CatalogPath);
            var constPattern = new Regex(@"public\s+const\s+string\s+(\w+)\s*=\s*""([^""]+)"";");
            var matches = constPattern.Matches(content);

            Assert.NotEmpty(matches);

            var snakePattern = new Regex(@"^[a-z0-9_]+$");

            foreach (Match match in matches)
            {
                var val = match.Groups[2].Value;
                // Exclude bus name constants which use PascalCase/Uppercase
                if (ValidBusIdentifiers.Contains(val)) continue;

                Assert.True(snakePattern.IsMatch(val),
                    $"Audio cue constant '{match.Groups[1].Value}' with value '{val}' must be snake_case.");
            }
        }

        private static string DataCatalogPath => Path.Combine(GetRepositoryRoot(), "Assets", "StreamingAssets", "Data", "audio_cues.json");

        [Fact]
        public void AudioCueCatalog_AllRegistrations_ReferenceValidBuses()
        {
            Assert.True(File.Exists(DataCatalogPath), $"audio_cues.json must exist at {DataCatalogPath}");
            using var doc = JsonDocument.Parse(File.ReadAllText(DataCatalogPath));
            var cues = doc.RootElement.GetProperty("cues");
            Assert.True(cues.GetArrayLength() > 0, "audio_cues.json must contain registered cues");

            foreach (var cue in cues.EnumerateArray())
            {
                var bus = cue.GetProperty("bus").GetString();
                Assert.NotNull(bus);
                Assert.Contains(bus, ValidBusIdentifiers);
            }
        }

        [Fact]
        public void AudioCueCatalog_AllJsonCues_HaveConstantsInCode()
        {
            var constants = new HashSet<string>(StringComparer.Ordinal);
            var constPattern = new Regex(@"public\s+const\s+string\s+\w+\s*=\s*""([^""]+)"";");
            foreach (Match match in constPattern.Matches(File.ReadAllText(CatalogPath)))
                constants.Add(match.Groups[1].Value);

            using var doc = JsonDocument.Parse(File.ReadAllText(DataCatalogPath));
            foreach (var cue in doc.RootElement.GetProperty("cues").EnumerateArray())
            {
                string id = cue.GetProperty("id").GetString()!;
                Assert.Contains(id, constants);
            }
        }

        [Fact]
        public void AudioCueCatalog_NoDuplicateCueValues()
        {
            var content = File.ReadAllText(CatalogPath);
            var constPattern = new Regex(@"public\s+const\s+string\s+\w+\s*=\s*""([^""]+)"";");
            var matches = constPattern.Matches(content);

            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (Match match in matches)
            {
                var val = match.Groups[1].Value;
                if (ValidBusIdentifiers.Contains(val)) continue;

                Assert.DoesNotContain(val, seen);
                seen.Add(val);
            }
        }

        [Fact]
        public void AuthoredRadioAudioCues_ResolveToCatalogEntries()
        {
            var constants = new HashSet<string>(StringComparer.Ordinal);
            var constPattern = new Regex(@"public\s+const\s+string\s+\w+\s*=\s*""([^""]+)"";");
            foreach (Match match in constPattern.Matches(File.ReadAllText(CatalogPath)))
                constants.Add(match.Groups[1].Value);

            var files = new[] { "year_of_ash_radio.json", "verdict_radio.json" };
            int authoredCueCount = 0;
            foreach (string file in files)
            {
                string path = Path.Combine(GetRepositoryRoot(), "Assets", "StreamingAssets", "Data", file);
                using var doc = JsonDocument.Parse(File.ReadAllText(path));
                foreach (JsonElement broadcast in doc.RootElement.GetProperty("broadcasts").EnumerateArray())
                {
                    if (!broadcast.TryGetProperty("audio_cue", out JsonElement cue))
                        continue;

                    string? cueId = cue.GetString();
                    Assert.False(string.IsNullOrWhiteSpace(cueId), $"{file} has an empty audio_cue");
                    Assert.Contains(cueId!, constants);
                    authoredCueCount++;
                }
            }

            Assert.Equal(10, authoredCueCount);
        }
    }
}
