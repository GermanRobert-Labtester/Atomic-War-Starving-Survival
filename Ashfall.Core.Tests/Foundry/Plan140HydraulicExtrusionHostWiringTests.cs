// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Plan140Foundry
{
    /// <summary>
    /// Plan 140 Phase 2 — host/save registration contract without Godot runtime.
    /// </summary>
    public sealed class Plan140HydraulicExtrusionHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesHydraulicExtrusion()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "hydraulic_extrusion");
            Assert.NotNull(section);
            Assert.Equal("SaveHydraulicExtrusion", section!.SaveMethod);
            Assert.Equal("SetupHydraulicExtrusion", section.SetupMethod);
            Assert.Equal("foundry", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("hydraulic_extrusion", out var file));
            Assert.Equal("hydraulic_extrusion_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_HydraulicExtrusion_IsUnique()
        {
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "hydraulic_extrusion"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveHydraulicExtrusion"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupHydraulicExtrusion"));
        }

        [Fact]
        public void CampaignStreamIds_HydraulicExtrusion_IsDistinct()
        {
            var all = typeof(Ashfall.Core.Random.CampaignStreamIds)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
            Assert.Contains("hydraulic_extrusion", all);
        }
    }
}
