using System;
using System.Collections.Generic;
using Ashfall.Core.Foundry;
using Xunit;

namespace Ashfall.Core.Tests.Foundry
{
    public class FoundryActionSurfaceTests
    {
        [Fact]
        public void TapAndCast_ReturnsResultWithStableCode()
        {
            var sys = new SilentFoundrySystem();
            var surf = new FoundryActionSurface(sys);
            var r = surf.TapAndCast(5);
            Assert.NotNull(r);
            // Outcome label is whatever the existing TapAndCast produced; reason code
            // is "ok" when the string is non-empty, "cast_failed" otherwise.
            Assert.Contains(r.ReasonCode, new[] { "ok", "cast_failed" });
        }

        [Fact]
        public void AddCharge_ValidatesInputs()
        {
            var surf = new FoundryActionSurface(new SilentFoundrySystem());
            Assert.False(surf.AddCharge(null, 1).Succeeded);
            Assert.False(surf.AddCharge("scrap", 0).Succeeded);
            Assert.False(surf.AddCharge("scrap", -1).Succeeded);
            Assert.True(surf.AddCharge("scrap", 5).Succeeded);
            Assert.Equal(5, surf.AddCharge("scrap", 5).IntDeltas["material_added"]);
        }

        [Fact]
        public void SelectRecipe_ValidatesInput()
        {
            var surf = new FoundryActionSurface(new SilentFoundrySystem());
            Assert.False(surf.SelectRecipe(null).Succeeded);
            Assert.True(surf.SelectRecipe("recipe_a").Succeeded);
        }

        [Fact]
        public void Preheat_ValidatesRange()
        {
            var surf = new FoundryActionSurface(new SilentFoundrySystem());
            Assert.False(surf.Preheat(-1).Succeeded);
            Assert.False(surf.Preheat(5000).Succeeded);
            Assert.True(surf.Preheat(800).Succeeded);
            Assert.Equal(800, surf.Preheat(800).IntDeltas["temperature_c"]);
        }

        [Fact]
        public void ResolveStrike_ValidatesInput()
        {
            var surf = new FoundryActionSurface(new SilentFoundrySystem());
            Assert.False(surf.ResolveStrike(null).Succeeded);
            Assert.True(surf.ResolveStrike("resolution_a").Succeeded);
        }

        [Fact]
        public void Actions_ReturnOkOrFailWithReasonCode()
        {
            var surf = new FoundryActionSurface(new SilentFoundrySystem());
            var r = surf.AddCharge("scrap", 1);
            Assert.True(r.Succeeded);
            Assert.False(string.IsNullOrEmpty(r.ReasonCode));
        }
    }
}
