// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class PatrolTerritoryIntegrationTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolTerritoryIntegrationTests()
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
        public void PatrolTerritoryResolver_MapsRegionsToCanonicalLocations()
        {
            Assert.Equal("loc_toll_house", PatrolTerritoryResolver.ResolveLocationForRegion("the_toll"));
            Assert.Equal("pass_mount_karkov", PatrolTerritoryResolver.ResolveLocationForRegion("high_scarp"));
            Assert.Equal("loc_denial_cut_substation", PatrolTerritoryResolver.ResolveLocationForRegion("industrial_belt"));
            Assert.Equal("settlement_13", PatrolTerritoryResolver.ResolveLocationForRegion("dead_suburbs"));

            // Explicit locationId overrides region fallback
            Assert.Equal("custom_node", PatrolTerritoryResolver.ResolveLocation("custom_node", "the_toll"));
        }

        [Fact]
        public void IsClaimedBy_LadderStateRules_EnforcedCorrectly()
        {
            var territoryAuth = new DynamicTerritoryAuthority();
            string loc = "loc_toll_house";
            string warlord = "warlords_sector_4";
            string garrison = "faction_central_garrison";

            // 1. None -> Ineligible for all
            territoryAuth.SetTerritory(loc, WarlordTerritoryState.None);
            Assert.False(territoryAuth.IsClaimedBy(loc, warlord));
            Assert.False(territoryAuth.IsClaimedBy(loc, garrison));

            // 2. Claimed -> Eligible if claimant
            territoryAuth.SetTerritory(loc, WarlordTerritoryState.Claimed, controllerFactionId: null, claimantFactions: new[] { warlord });
            Assert.True(territoryAuth.IsClaimedBy(loc, warlord));
            Assert.False(territoryAuth.IsClaimedBy(loc, garrison));

            // 3. Contested -> Eligible for all active claimants
            territoryAuth.SetTerritory(loc, WarlordTerritoryState.Contested, controllerFactionId: null, claimantFactions: new[] { warlord, garrison });
            Assert.True(territoryAuth.IsClaimedBy(loc, warlord));
            Assert.True(territoryAuth.IsClaimedBy(loc, garrison));
            Assert.False(territoryAuth.IsClaimedBy(loc, "faction_ash_sign"));

            // 4. Controlled -> Eligible ONLY if controller
            territoryAuth.SetTerritory(loc, WarlordTerritoryState.Controlled, controllerFactionId: garrison, claimantFactions: new[] { warlord, garrison });
            Assert.True(territoryAuth.IsClaimedBy(loc, garrison));
            Assert.False(territoryAuth.IsClaimedBy(loc, warlord)); // Even though warlord is a claimant, controlled is strictly controller-only
        }

        [Fact]
        public void DynamicTerritory_GatesWarlordPatrolEligibility()
        {
            var territoryAuth = new DynamicTerritoryAuthority();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: null, territoryAuthority: territoryAuth);

            var warlordRaid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            Assert.NotNull(warlordRaid);
            Assert.Equal("warlords_sector_4", warlordRaid.RequiredTerritoryOwner);

            // In "the_toll" (maps to "loc_toll_house")
            // A: Territory is None -> Ineligible
            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.None);
            Assert.False(sys.IsEncounterEligible(warlordRaid, "the_toll", 2.0f, "all", 10));

            // B: Territory is Contested with Warlord as claimant -> Eligible
            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.Contested, controllerFactionId: null, claimantFactions: new[] { "warlords_sector_4" });
            Assert.True(sys.IsEncounterEligible(warlordRaid, "the_toll", 2.0f, "all", 10));

            // C: Garrison captures and Controls the node -> Warlord Ineligible, Garrison Border Inspection Eligible
            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.Controlled, controllerFactionId: "faction_central_garrison");
            Assert.False(sys.IsEncounterEligible(warlordRaid, "the_toll", 2.0f, "all", 10));

            var garrisonBorder = _catalog.GetEncounter("enc_patrol_central_garrison_border")!;
            Assert.NotNull(garrisonBorder);
            Assert.Equal("faction_central_garrison", garrisonBorder.RequiredTerritoryOwner);
            // Garrison requires required_territory_owner = faction_central_garrison
            Assert.True(territoryAuth.IsClaimedBy("loc_toll_house", "faction_central_garrison"));
        }

        [Fact]
        public void TerritoryChange_ImmediatelyUpdatesEligibilityWithoutDelay()
        {
            var territoryAuth = new DynamicTerritoryAuthority();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: null, territoryAuthority: territoryAuth);
            var enc = _catalog.GetEncounter("enc_patrol_warlord_raid")!;

            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.Claimed, controllerFactionId: "warlords_sector_4");
            Assert.True(sys.IsEncounterEligible(enc, "the_toll", 2.0f, "all", 1));

            // Immediate transition
            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.None);
            Assert.False(sys.IsEncounterEligible(enc, "the_toll", 2.0f, "all", 1));

            // Immediate restore
            territoryAuth.SetTerritory("loc_toll_house", WarlordTerritoryState.Controlled, controllerFactionId: "warlords_sector_4");
            Assert.True(sys.IsEncounterEligible(enc, "the_toll", 2.0f, "all", 1));
        }

        [Fact]
        public void FactionAgnosticPatrol_UnaffectedByTerritoryOwnership()
        {
            var territoryAuth = new DynamicTerritoryAuthority();
            var sys = new TravelEncounterSystem(_catalog, inventory: null, factionWar: null, territoryAuthority: territoryAuth);

            var penal = _catalog.GetEncounter("enc_patrol_penal_battalion")!;
            Assert.NotNull(penal);
            Assert.True(string.IsNullOrEmpty(penal.RequiredTerritoryOwner));

            // Regardless of whether territory is None, Contested, or Controlled by another faction
            territoryAuth.SetTerritory("loc_denial_cut_substation", WarlordTerritoryState.None);
            Assert.True(sys.IsEncounterEligible(penal, "industrial_belt", 2.0f, "all", 1));

            territoryAuth.SetTerritory("loc_denial_cut_substation", WarlordTerritoryState.Controlled, controllerFactionId: "warlords_sector_4");
            Assert.True(sys.IsEncounterEligible(penal, "industrial_belt", 2.0f, "all", 1));
        }
    }
}
