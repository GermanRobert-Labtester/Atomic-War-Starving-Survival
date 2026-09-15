// SPDX-License-Identifier: MIT
using System.Linq;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 211 host/save registration contract without a Godot runtime:
    /// registry row, filename mapping, uniqueness, and RNG-stream hygiene.
    /// </summary>
    public sealed class Plan211BlackMarketHostWiringTests
    {
        [Fact]
        public void SaveSectionRegistry_IncludesBlackMarket()
        {
            var section = SaveSectionRegistry.All.SingleOrDefault(s => s.SectionKey == "black_market");
            Assert.NotNull(section);
            Assert.Equal("SaveBlackMarket", section!.SaveMethod);
            Assert.Equal("SetupBlackMarket", section.SetupMethod);
            Assert.Equal("economy", section.Owner);
            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("black_market", out var file));
            Assert.Equal("black_market_save.json", file);
        }

        [Fact]
        public void SaveSectionRegistry_BlackMarket_IsUnique()
        {
            // One authority per concern: exactly one black-market section —
            // never a second underworld ledger beside it.
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "black_market"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveBlackMarket"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SetupMethod == "SetupBlackMarket"));
        }

        [Fact]
        public void CampaignStreamIds_UnderworldStreams_AreDistinctAndSnakeCase()
        {
            var all = typeof(CampaignStreamIds)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
            Assert.Equal(all.Count, all.Distinct().Count());
            Assert.Contains("black_market_stock", all);
            Assert.Contains("black_market_bounty", all);
            Assert.Contains("black_market_debt_event", all);
            foreach (var id in new[] { CampaignStreamIds.BlackMarketStock, CampaignStreamIds.BlackMarketBounty, CampaignStreamIds.BlackMarketDebtEvent })
                Assert.True(id.All(c => (char.IsLower(c) && char.IsAsciiLetterOrDigit(c)) || c == '_'), $"{id} must be snake_case");
        }
    }
}
