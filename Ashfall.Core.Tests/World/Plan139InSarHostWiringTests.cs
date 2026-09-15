// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Plan139World
{
    /// <summary>
    /// Plan 139 Phase 2 — host/save registration contract without Godot runtime.
    /// </summary>
    public sealed class Plan139InSarHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesInSarDeformation()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "insar_deformation");
            Assert.NotNull(section);
            Assert.Equal("SaveInSarMapping", section!.SaveMethod);
            Assert.Equal("SetupInSarMapping", section.SetupMethod);
            Assert.Equal("world", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("insar_deformation", out var file));
            Assert.Equal("insar_deformation_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_InSarDeformation_IsUnique()
        {
            // One authority per concern: exactly one InSAR section, never a
            // second deformation ledger beside world/seismic truth.
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "insar_deformation"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveInSarMapping"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupInSarMapping"));
        }

        [Fact]
        public void CampaignStreamIds_InSarDeformation_IsDistinct()
        {
            var all = typeof(Ashfall.Core.Random.CampaignStreamIds)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
            Assert.Contains("insar_deformation", all);
        }
    }
}
