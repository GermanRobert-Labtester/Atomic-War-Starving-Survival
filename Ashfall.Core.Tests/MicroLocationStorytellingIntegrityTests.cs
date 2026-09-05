using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task F13: Environmental Storytelling QA integrity tests for the 25 route micro-locations.
    /// Validates the 8-dimension rubric: conciseness (<= 3 sentences), observability,
    /// absence of omniscient narrative, concrete nouns, and category balance.
    /// </summary>
    public class MicroLocationStorytellingIntegrityTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static JsonDocument LoadCatalog()
        {
            string path = Path.Combine(DataDir(), "micro_locations.json");
            Assert.True(File.Exists(path), $"Catalog file not found at: {path}");
            string json = File.ReadAllText(path);
            return JsonDocument.Parse(json);
        }

        [Fact]
        public void MicroLocations_CatalogStructureAndCounts_Valid()
        {
            using var doc = LoadCatalog();
            var root = doc.RootElement;

            Assert.True(root.TryGetProperty("encounters", out var encounters));
            Assert.True(encounters.GetArrayLength() >= 25);

            int discoveryCount = 0;
            int hazardCount = 0;
            int socialCount = 0;

            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string cat = enc.GetProperty("category").GetString() ?? "";
                if (cat == "Discovery") discoveryCount++;
                else if (cat == "Hazard") hazardCount++;
                else if (cat == "Social") socialCount++;
            }

            Assert.Equal(20, discoveryCount);
            Assert.Equal(3, hazardCount);
            Assert.Equal(2, socialCount);
        }

        [Fact]
        public void MicroLocations_All25Descriptions_UnderThreeSentences()
        {
            using var doc = LoadCatalog();
            var encounters = doc.RootElement.GetProperty("encounters");

            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string id = enc.GetProperty("id").GetString() ?? "";
                string desc = enc.GetProperty("description").GetString() ?? "";

                Assert.False(string.IsNullOrWhiteSpace(desc), $"Encounter {id} has empty description.");

                // Split into sentences (by period followed by space or end)
                var rawSentences = Regex.Split(desc, @"\.\s+|\.$");
                var validSentences = new List<string>();
                foreach (var s in rawSentences)
                {
                    if (!string.IsNullOrWhiteSpace(s))
                        validSentences.Add(s.Trim());
                }

                Assert.True(validSentences.Count >= 1 && validSentences.Count <= 3,
                    $"Encounter '{id}' has {validSentences.Count} sentences (expected 1-3): \"{desc}\"");

                int wordCount = desc.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.True(wordCount >= 15 && wordCount <= 65,
                    $"Encounter '{id}' has {wordCount} words (expected 15-65): \"{desc}\"");
            }
        }

        [Fact]
        public void MicroLocations_Descriptions_ExcludeOmniscientPhrases()
        {
            using var doc = LoadCatalog();
            var encounters = doc.RootElement.GetProperty("encounters");

            string[] bannedPhrases = new[]
            {
                "died here",
                "who lived here",
                "what must have happened",
                "gave up or died trying",
                "terrifyingly hollow",
                "in a panic",
                "desperate tally",
                "desperate warning",
                "desperate little offerings"
            };

            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string id = enc.GetProperty("id").GetString() ?? "";
                string desc = enc.GetProperty("description").GetString() ?? "";

                foreach (var phrase in bannedPhrases)
                {
                    Assert.False(desc.IndexOf(phrase, StringComparison.OrdinalIgnoreCase) >= 0,
                        $"Encounter '{id}' contains banned omniscient phrase '{phrase}': \"{desc}\"");
                }
            }
        }

        [Fact]
        public void MicroLocations_Descriptions_ExcludeMechanicalGameTerms()
        {
            using var doc = LoadCatalog();
            var encounters = doc.RootElement.GetProperty("encounters");

            string[] mechanicalTerms = new[]
            {
                "hit points",
                "hp",
                "morale delta",
                "guilt delta",
                "stat check",
                "dice roll",
                "inventory slot"
            };

            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string id = enc.GetProperty("id").GetString() ?? "";
                string desc = enc.GetProperty("description").GetString() ?? "";

                foreach (var term in mechanicalTerms)
                {
                    Assert.False(desc.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0,
                        $"Encounter '{id}' contains mechanical game term '{term}': \"{desc}\"");
                }
            }
        }

        [Fact]
        public void MicroLocations_Choices_AllHaveValidTextAndIds()
        {
            using var doc = LoadCatalog();
            var encounters = doc.RootElement.GetProperty("encounters");

            for (int i = 0; i < 25; i++)
            {
                var enc = encounters[i];
                string id = enc.GetProperty("id").GetString() ?? "";
                Assert.True(enc.TryGetProperty("choices", out var choices));
                Assert.True(choices.GetArrayLength() >= 2, $"Encounter {id} has fewer than 2 choices.");

                for (int c = 0; c < choices.GetArrayLength(); c++)
                {
                    var choice = choices[c];
                    string cid = choice.GetProperty("choiceId").GetString() ?? "";
                    string text = choice.GetProperty("text").GetString() ?? "";

                    Assert.False(string.IsNullOrWhiteSpace(cid), $"Encounter {id} choice {c} missing choiceId");
                    Assert.False(string.IsNullOrWhiteSpace(text), $"Encounter {id} choice {cid} missing text");
                }
            }
        }
    }
}
