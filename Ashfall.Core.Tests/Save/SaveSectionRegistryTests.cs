using System;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class SaveSectionRegistryTests
    {
        [Fact]
        public void Registry_IsPopulatedAndNonEmpty()
        {
            Assert.NotEmpty(SaveSectionRegistry.All);
            Assert.True(SaveSectionRegistry.All.Count >= 60, $"Expected at least 60 sections, found {SaveSectionRegistry.All.Count}");
        }

        [Fact]
        public void Registry_SectionKeysAreUnique()
        {
            var keys = SaveSectionRegistry.All.Select(s => s.SectionKey).ToList();
            var duplicateKeys = keys.GroupBy(k => k).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            Assert.Empty(duplicateKeys);
        }

        [Fact]
        public void Registry_MetadataFieldsAreWellFormed()
        {
            var snakeCaseRegex = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);

            foreach (var section in SaveSectionRegistry.All)
            {
                Assert.False(string.IsNullOrWhiteSpace(section.SectionKey), "SectionKey must not be empty");
                Assert.Matches(snakeCaseRegex, section.SectionKey);

                Assert.False(string.IsNullOrWhiteSpace(section.SaveMethod), $"SaveMethod must not be empty for {section.SectionKey}");
                Assert.StartsWith("Save", section.SaveMethod);

                Assert.False(string.IsNullOrWhiteSpace(section.Owner), $"Owner must not be empty for {section.SectionKey}");
                Assert.False(string.IsNullOrWhiteSpace(section.Description), $"Description must not be empty for {section.SectionKey}");

                if (section.SetupMethod != null)
                {
                    Assert.StartsWith("Setup", section.SetupMethod);
                }
            }
        }

        [Fact]
        public void TryGetSection_ResolvesKnownKeys_AndRejectsUnknown()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("survivors", out var survivors));
            Assert.NotNull(survivors);
            Assert.Equal("SaveSurvivors", survivors!.SaveMethod);
            Assert.Equal("SetupSurvivors", survivors.SetupMethod);

            Assert.True(SaveSectionRegistry.TryGetSection("wasteland_map", out var map));
            Assert.NotNull(map);
            Assert.False(map!.RequiresSetup);

            Assert.False(SaveSectionRegistry.TryGetSection("non_existent_section_key", out var unknown));
            Assert.Null(unknown);
        }

        [Fact]
        public void SectionKeys_MatchesAllList()
        {
            var keys = SaveSectionRegistry.SectionKeys;
            Assert.Equal(SaveSectionRegistry.All.Count, keys.Count);

            for (int i = 0; i < keys.Count; i++)
            {
                Assert.Equal(SaveSectionRegistry.All[i].SectionKey, keys[i]);
            }
        }
    }
}
