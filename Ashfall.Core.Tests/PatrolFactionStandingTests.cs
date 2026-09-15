// SPDX-License-Identifier: MIT
// ASHFALL Patrol Faction Standing Tests (PAT-F1-001 through PAT-F1-010)

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class PatrolFactionStandingTests
    {
        private readonly string _dataDir;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolFactionStandingTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
        }

        [Fact]
        public void FactionStandingIdResolver_MappingTable_MapsLoreIdsToSystemsIds()
        {
            var failures = new List<string>();

            foreach (var testCase in new[]
            {
                (LoreId: "iron_garrison", ExpectedSystemId: "faction_central_garrison"),
                (LoreId: "ash_militia", ExpectedSystemId: "faction_upland_militia"),
                (LoreId: "cult_of_ash_sign", ExpectedSystemId: "faction_cult_of_the_glow"),
                (LoreId: "warlords_sector_4", ExpectedSystemId: "faction_scavenger_warlords"),
                (LoreId: "faction_black_ops", ExpectedSystemId: "faction_black_ops"),
                (LoreId: "unknown_faction", ExpectedSystemId: "unknown_faction"),
            })
            {
                string mapped = FactionStandingIdResolver.ToSystemsId(testCase.LoreId);
                if (mapped != testCase.ExpectedSystemId)
                {
                    failures.Add(
                        $"lore id '{testCase.LoreId}' expected '{testCase.ExpectedSystemId}', got '{mapped}'");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void FactionWarSystem_ModifyStanding_CanonicalizesLoreId()
        {
            var war = new FactionWarSystem();
            war.ModifyStanding("iron_garrison", 10);

            // Both lore ID and systems ID queries must read the exact same modified value
            Assert.Equal(10, war.GetStanding("iron_garrison"));
            Assert.Equal(10, war.GetStanding("faction_central_garrison"));
        }

        [Fact]
        public void FactionWarSystem_StandingClampsTo100AndMinus100()
        {
            var war = new FactionWarSystem();
            war.ModifyStanding("faction_black_ops", 120);
            Assert.Equal(100, war.GetStanding("faction_black_ops"));

            war.ModifyStanding("faction_black_ops", -250);
            Assert.Equal(-100, war.GetStanding("faction_black_ops"));
        }

        [Fact]
        public void FactionWarSystem_ZeroDelta_DoesNotAlterStanding()
        {
            var war = new FactionWarSystem();
            int initial = war.GetStanding("faction_upland_militia");
            war.ModifyStanding("ash_militia", 0);
            Assert.Equal(initial, war.GetStanding("faction_upland_militia"));
        }

        [Fact]
        public void TryBuildResolutionPlan_ResolvesCanonicalFactionId()
        {
            var sys = new TravelEncounterSystem(_catalog);
            bool ok = sys.TryBuildResolutionPlan("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var plan);
            Assert.True(ok);
            Assert.NotNull(plan);
            Assert.Equal("iron_garrison", plan!.RawFactionId);
            Assert.Equal("faction_central_garrison", plan.CanonicalFactionId);
            Assert.Equal(1, plan.FactionStandingDelta);
        }

        [Fact]
        public void FactionStanding_PatrolResolution_ModifiesStandingInWarSystem()
        {
            var war = new FactionWarSystem();
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("canned_food", 5);
            var sys = new TravelEncounterSystem(_catalog, inv, war);

            int before = war.GetStanding("faction_central_garrison");
            bool resolved = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.True(resolved);
            Assert.NotNull(res);
            Assert.Equal(before + 1, war.GetStanding("faction_central_garrison"));
        }
    }
}
