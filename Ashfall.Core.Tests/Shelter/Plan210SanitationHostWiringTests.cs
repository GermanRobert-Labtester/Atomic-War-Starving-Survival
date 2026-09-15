// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 210 host/save registration contract without a Godot runtime:
    /// registry row, filename mapping, uniqueness, and stream-space hygiene.
    /// </summary>
    public sealed class Plan210SanitationHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesSanitation()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "sanitation");
            Assert.NotNull(section);
            Assert.Equal("SaveSanitation", section!.SaveMethod);
            Assert.Equal("SetupSanitation", section.SetupMethod);
            Assert.Equal("shelter", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("sanitation", out var file));
            Assert.Equal("sanitation_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_Sanitation_IsUnique()
        {
            // One authority per concern: exactly one sanitation section, never
            // a second waste/hygiene ledger beside it.
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "sanitation"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveSanitation"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupSanitation"));
        }
    }
}
