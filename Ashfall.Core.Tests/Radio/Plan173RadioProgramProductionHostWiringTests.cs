// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Plan 173 Phase 2 — host/save registration contract without Godot runtime.
    /// </summary>
    public sealed class Plan173RadioProgramProductionHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesRadioProgramProduction()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "radio_program_production");
            Assert.NotNull(section);
            Assert.Equal("SaveRadioProgramProduction", section!.SaveMethod);
            Assert.Equal("SetupRadioProgramProduction", section.SetupMethod);
            Assert.Equal("radio", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("radio_program_production", out var file));
            Assert.Equal("radio_program_production_save.json", file);
        }
    }
}
