using System;
using System.Collections.Generic;
using System.IO;
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
            "Alerts", "Generator", "Ventilation", "Radio", "Medical", "Surface"
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

        [Fact]
        public void AudioCueCatalog_AllRegistrations_ReferenceValidBuses()
        {
            var content = File.ReadAllText(CatalogPath);
            var regPattern = new Regex(@"Reg\(\s*\w+\s*,\s*""res://[^""]+""\s*,\s*(?:AudioBusNames\.)?(\w+)");
            var matches = regPattern.Matches(content);

            Assert.NotEmpty(matches);

            foreach (Match match in matches)
            {
                var bus = match.Groups[1].Value;
                Assert.Contains(bus, ValidBusIdentifiers);
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
    }
}
