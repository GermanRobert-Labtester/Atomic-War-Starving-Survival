// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Plan138Radiation
{
    /// <summary>
    /// Plan 138 Phase 2 — host/save registration contract without Godot runtime.
    /// </summary>
    public sealed class Plan138LowBackgroundHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesLowBackgroundMetrology()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "low_background_metrology");
            Assert.NotNull(section);
            Assert.Equal("SaveLowBackgroundMetrology", section!.SaveMethod);
            Assert.Equal("SetupLowBackgroundMetrology", section.SetupMethod);
            Assert.Equal("radiation", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("low_background_metrology", out var file));
            Assert.Equal("low_background_metrology_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_LowBackgroundMetrology_IsUnique()
        {
            // One authority per concern: exactly one metrology section, and the
            // section count moved by exactly one over the pre-Plan-138 baseline.
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "low_background_metrology"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveLowBackgroundMetrology"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupLowBackgroundMetrology"));
        }

        [Fact]
        public void CampaignStreamIds_LowBackgroundMetrology_IsDistinct()
        {
            // A forked stream cannot collide with any existing stream id.
            var all = typeof(Ashfall.Core.Random.CampaignStreamIds)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
            Assert.Contains("low_background_metrology", all);
        }
    }
}
