// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class PatrolWarStateVariantTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolWarStateVariantTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        [Fact]
        public void WarState_FieldParsingAndValidation()
        {
            Assert.Equal(TravelEncounterWarState.Any, TravelEncounterDefinition.ParseWarState("any"));
            Assert.Equal(TravelEncounterWarState.Peacetime, TravelEncounterDefinition.ParseWarState("peacetime"));
            Assert.Equal(TravelEncounterWarState.Wartime, TravelEncounterDefinition.ParseWarState("wartime"));
            Assert.Equal(TravelEncounterWarState.Any, TravelEncounterDefinition.ParseWarState(""));

            Assert.True(TravelEncounterDefinition.IsValidWarState("any"));
            Assert.True(TravelEncounterDefinition.IsValidWarState("peacetime"));
            Assert.True(TravelEncounterDefinition.IsValidWarState("wartime"));
            Assert.False(TravelEncounterDefinition.IsValidWarState("nuclear_winter"));
        }

        [Fact]
        public void WarStateGating_PeacetimeBlocksWartime_WartimeAllowsWartime()
        {
            var warSys = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys);

            var warlordRaidAny = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var warlordAdvanceWartime = _catalog.GetEncounter("enc_patrol_warlord_advancing")!;

            Assert.NotNull(warlordRaidAny);
            Assert.NotNull(warlordAdvanceWartime);
            Assert.Equal(TravelEncounterWarState.Any, warlordRaidAny.WarState);
            Assert.Equal(TravelEncounterWarState.Wartime, warlordAdvanceWartime.WarState);

            // 1. In peacetime:
            warSys.SetWarActive(false);
            Assert.False(warSys.IsAtWar);
            Assert.True(sys.IsEncounterEligible(warlordRaidAny, "the_toll", 3.0f, "all", 10));
            Assert.False(sys.IsEncounterEligible(warlordAdvanceWartime, "the_toll", 3.0f, "all", 10));

            // 2. In wartime:
            warSys.SetWarActive(true);
            Assert.True(warSys.IsAtWar);
            Assert.True(sys.IsEncounterEligible(warlordRaidAny, "the_toll", 3.0f, "all", 10));
            Assert.True(sys.IsEncounterEligible(warlordAdvanceWartime, "the_toll", 3.0f, "all", 10));
        }

        [Fact]
        public void WarWeightMultiplier_OnlyAppliedDuringWartime()
        {
            var warSys = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys);

            var advance = _catalog.GetEncounter("enc_patrol_warlord_advancing")!;
            Assert.NotNull(advance);
            Assert.Equal(1.5f, advance.WarWeightMultiplier);
            float baseWeight = advance.BaseWeight; // 0.5

            // During peacetime
            warSys.SetWarActive(false);
            float peacetimeWeight = sys.GetEffectiveWeight(advance, "Balanced");
            // Balanced stance deliberately suppresses raid-party recurrence:
            // 0.5 * 0.2 = 0.1.
            Assert.Equal(baseWeight * 0.2f, peacetimeWeight, 3);

            // During wartime: multiplied by WarWeightMultiplier (1.5)
            warSys.SetWarActive(true);
            float wartimeWeight = sys.GetEffectiveWeight(advance, "Balanced");
            Assert.Equal(peacetimeWeight * 1.5f, wartimeWeight, 3);
        }

        [Fact]
        public void SiblingVariants_ShareCooldownGroup()
        {
            var warSys = new FactionWarSystem();
            warSys.SetWarActive(true);
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys);

            var raid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var advance = _catalog.GetEncounter("enc_patrol_warlord_advancing")!;

            Assert.Equal(raid.CooldownGroup, advance.CooldownGroup);
            Assert.Equal("patrol_warlord_raid", raid.CooldownGroup);

            // Both eligible initially on day 10
            Assert.True(sys.IsEncounterEligible(raid, "the_toll", 3.0f, "all", 10));
            Assert.True(sys.IsEncounterEligible(advance, "the_toll", 3.0f, "all", 10));

            // Resolve choice on advance
            sys.ResolveChoice(advance.Id, advance.Choices[0].ChoiceId, 10, out _);

            // Both must now be on cooldown on day 11 (cooldown is 7 days -> available day 17)
            Assert.False(sys.IsEncounterEligible(raid, "the_toll", 3.0f, "all", 11));
            Assert.False(sys.IsEncounterEligible(advance, "the_toll", 3.0f, "all", 11));

            // Expired on day 17
            Assert.True(sys.IsEncounterEligible(raid, "the_toll", 3.0f, "all", 17));
            Assert.True(sys.IsEncounterEligible(advance, "the_toll", 3.0f, "all", 17));
        }

        [Fact]
        public void ImmediateUpdate_OnWarStartAndEnd()
        {
            var warSys = new FactionWarSystem();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: warSys);
            var recon = _catalog.GetEncounter("enc_patrol_ash_sign_wartime_recon")!;
            Assert.NotNull(recon);

            warSys.SetWarActive(false);
            Assert.False(sys.IsEncounterEligible(recon, "the_toll", 2.0f, "all", 1));

            // Immediate reaction to war activation
            warSys.SetWarActive(true);
            Assert.True(sys.IsEncounterEligible(recon, "the_toll", 2.0f, "all", 1));

            // Immediate reaction to war end
            warSys.SetWarActive(false);
            Assert.False(sys.IsEncounterEligible(recon, "the_toll", 2.0f, "all", 1));
        }
    }
}
