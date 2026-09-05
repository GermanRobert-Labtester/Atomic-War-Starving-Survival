// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public class TravelEncounterCooldownGroupTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public TravelEncounterCooldownGroupTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        private Inventory.Inventory CreateInventoryWithFood()
        {
            var inv = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            inv.TryProduce("canned_food", 20);
            return inv;
        }

        [Fact]
        public void CooldownKey_ReturnsGroupWhenSet_ElseEncounterId()
        {
            var encWithGroup = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            Assert.NotNull(encWithGroup);
            Assert.Equal("patrol_garrison_checkpoint", encWithGroup.GetCooldownKey());

            var variant2 = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v2")!;
            Assert.NotNull(variant2);
            Assert.Equal("patrol_garrison_checkpoint", variant2.GetCooldownKey());

            var legacyNoGroup = _catalog.GetEncounter("enc_travel_slag_beetle_slag_heap")!;
            Assert.NotNull(legacyNoGroup);
            Assert.Equal("enc_travel_slag_beetle_slag_heap", legacyNoGroup.GetCooldownKey());
        }

        [Fact]
        public void ResolveChoice_SetsGroupCooldown_AllVariantsLocked()
        {
            var inv = CreateInventoryWithFood();
            var sys = new TravelEncounterSystem(_catalog, inv);

            // Day 10: resolve base checkpoint encounter
            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 10, out var res);
            Assert.True(ok);
            Assert.NotNull(res);

            // Group cooldown key should have expiry day 15 (10 + 5)
            Assert.Equal(15, sys.GetCooldownExpiry("patrol_garrison_checkpoint"));

            // All variants in this group should now be on cooldown on day 11
            var baseEnc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var v2Enc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v2")!;
            var v3Enc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v3")!;

            Assert.False(sys.IsEncounterEligible(baseEnc, "high_scarp", 1.0f, "all", 11));
            Assert.False(sys.IsEncounterEligible(v2Enc, "high_scarp", 1.0f, "all", 11));
            Assert.False(sys.IsEncounterEligible(v3Enc, "high_scarp", 1.0f, "all", 11));

            // Direct resolution attempt on variant v2 during cooldown fails
            bool resolvedDuringCd = sys.ResolveChoice("enc_patrol_garrison_checkpoint_v2", "choice_pay_garrison_toll", 11, out _);
            Assert.False(resolvedDuringCd);
        }

        [Fact]
        public void ResolveChoice_WarlordVariants_ShareGroupCooldown()
        {
            var inv = CreateInventoryWithFood();
            var sys = new TravelEncounterSystem(_catalog, inv);

            // Resolve v2 warlord raid on day 20
            bool ok = sys.ResolveChoice("enc_patrol_warlord_raid_v2", "choice_warlord_bribe", 20, out var res);
            Assert.True(ok);
            Assert.NotNull(res);

            Assert.Equal(25, sys.GetCooldownExpiry("patrol_warlord_raid"));

            var baseRaid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var v2Raid = _catalog.GetEncounter("enc_patrol_warlord_raid_v2")!;
            var v3Raid = _catalog.GetEncounter("enc_patrol_warlord_raid_v3")!;

            Assert.False(sys.IsEncounterEligible(baseRaid, "the_toll", 3.0f, "all", 22));
            Assert.False(sys.IsEncounterEligible(v2Raid, "the_toll", 3.0f, "all", 22));
            Assert.False(sys.IsEncounterEligible(v3Raid, "the_toll", 3.0f, "all", 22));
        }

        [Fact]
        public void EncounterWithoutGroup_OnlyLocksItself()
        {
            var inv = CreateInventoryWithFood();
            var sys = new TravelEncounterSystem(_catalog, inv);

            var beetle = _catalog.GetEncounter("enc_travel_slag_beetle_slag_heap")!;
            Assert.True(string.IsNullOrEmpty(beetle.CooldownGroup));

            // Resolve choice
            bool ok = sys.ResolveChoice("enc_travel_slag_beetle_slag_heap", "choice_harvest_chitin", 5, out _);
            Assert.True(ok);

            Assert.Equal(10, sys.GetCooldownExpiry("enc_travel_slag_beetle_slag_heap"));
            Assert.False(sys.IsEncounterEligible(beetle, "industrial_belt", 1.0f, "all", 6));

            // An unrelated encounter is not locked
            var other = _catalog.GetEncounter("enc_patrol_militia_roadblock")!;
            Assert.True(sys.IsEncounterEligible(other, "industrial_belt", 1.0f, "all", 6));
        }

        [Fact]
        public void CooldownExpiry_AllowsVariantsAgainAfter5Days()
        {
            var inv = CreateInventoryWithFood();
            var sys = new TravelEncounterSystem(_catalog, inv);

            bool ok = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 10, out _);
            Assert.True(ok);

            var v2Enc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v2")!;

            // Day 14: still on cooldown
            Assert.False(sys.IsEncounterEligible(v2Enc, "high_scarp", 1.0f, "all", 14));

            // Day 15: cooldown expired, eligible again
            Assert.True(sys.IsEncounterEligible(v2Enc, "high_scarp", 1.0f, "all", 15));

            // Can resolve choice on day 15
            bool resolvedAfterExpiry = sys.ResolveChoice("enc_patrol_garrison_checkpoint_v2", "choice_pay_garrison_toll", 15, out _);
            Assert.True(resolvedAfterExpiry);
        }

        [Fact]
        public void StateRoundtrip_PreservesGroupCooldowns()
        {
            var inv = CreateInventoryWithFood();
            var sys1 = new TravelEncounterSystem(_catalog, inv);

            sys1.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 12, out _);
            Assert.Equal(17, sys1.GetCooldownExpiry("patrol_garrison_checkpoint"));

            var state = sys1.CaptureState();
            Assert.NotNull(state);
            Assert.True(state.EncounterAvailableDay.ContainsKey("patrol_garrison_checkpoint"));
            Assert.Equal(17, state.EncounterAvailableDay["patrol_garrison_checkpoint"]);

            var sys2 = new TravelEncounterSystem(_catalog, inv);
            sys2.RestoreState(state);

            Assert.Equal(17, sys2.GetCooldownExpiry("patrol_garrison_checkpoint"));
            var v3Enc = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v3")!;
            Assert.False(sys2.IsEncounterEligible(v3Enc, "high_scarp", 1.0f, "all", 14));
            Assert.True(sys2.IsEncounterEligible(v3Enc, "high_scarp", 1.0f, "all", 17));
        }

        [Fact]
        public void LegacySaveMigration_FoldsMemberIdsIntoGroupMaxExpiry()
        {
            var inv = CreateInventoryWithFood();
            var sys = new TravelEncounterSystem(_catalog, inv);

            // Construct legacy state with individual member IDs
            var legacyState = new TravelEncounterState
            {
                EncounterAvailableDay = new Dictionary<string, int>
                {
                    { "enc_patrol_garrison_checkpoint", 14 },
                    { "enc_patrol_garrison_checkpoint_v2", 18 },
                    { "enc_slag_beetle_nest", 9 }
                }
            };

            sys.RestoreState(legacyState);

            // Group key should exist and have max expiry: 18
            Assert.Equal(18, sys.GetCooldownExpiry("patrol_garrison_checkpoint"));

            // Member keys should be cleaned up
            Assert.Equal(0, sys.GetCooldownExpiry("enc_patrol_garrison_checkpoint"));
            Assert.Equal(0, sys.GetCooldownExpiry("enc_patrol_garrison_checkpoint_v2"));

            // Unrelated encounter cooldown preserved
            Assert.Equal(9, sys.GetCooldownExpiry("enc_slag_beetle_nest"));
        }
    }
}
