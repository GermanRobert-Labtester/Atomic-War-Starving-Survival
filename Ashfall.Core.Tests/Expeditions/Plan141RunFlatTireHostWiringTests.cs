// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Plan141Expeditions
{
    /// <summary>
    /// Plan 141 Phase 2 — host/save registration contract without Godot runtime.
    /// </summary>
    public sealed class Plan141RunFlatTireHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesRunFlatTire()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "runflat_tire");
            Assert.NotNull(section);
            Assert.Equal("SaveRunFlatTire", section!.SaveMethod);
            Assert.Equal("SetupRunFlatTire", section.SetupMethod);
            Assert.Equal("expeditions", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("runflat_tire", out var file));
            Assert.Equal("runflat_tire_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_RunFlatTire_IsUnique()
        {
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "runflat_tire"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveRunFlatTire"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupRunFlatTire"));
        }

        [Fact]
        public void CampaignStreamIds_RunFlatTire_IsDistinct()
        {
            var all = typeof(Ashfall.Core.Random.CampaignStreamIds)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
            Assert.Contains("runflat_tire", all);
        }
    }
}
