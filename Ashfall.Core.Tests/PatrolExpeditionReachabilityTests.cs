// SPDX-License-Identifier: MIT
// ASHFALL Patrol Expedition Reachability Tests (PAT-F4)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public class PatrolExpeditionReachabilityTests
    {
        private readonly string _dataDir;
        private readonly TravelEncounterCatalog _patrolCatalog;
        private readonly IReadOnlyList<EncounterDefinition> _narrativeCatalog;

        public PatrolExpeditionReachabilityTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            _patrolCatalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, fileIO);
            _narrativeCatalog = NarrativeEncounterCatalogLoader.Load(_dataDir, fileIO, serializer);
        }

        [Fact]
        public void MergedCandidatePool_IncludesBothNarrativeAndPatrols()
        {
            var narrativeSys = new NarrativeEncounterSystem();
            narrativeSys.RegisterRange(_narrativeCatalog);
            var travelSys = new TravelEncounterSystem(_patrolCatalog);
            var rng = new SeededRng(42);

            var bridge = new ExpeditionEncounterBridge(narrativeSys, rng)
            {
                TravelEngine = travelSys,
                CurrentDay = 10,
                CurrentSeason = "window_deep_freeze",
                RegionResolver = _ => "the_toll"
            };

            var trigger = new ExpeditionState
            {
                locationId = "loc_denial_cut_substation",
                displayName = "The Denial Cut Substation",
                survivorId = "survivor_test",
                dangerLevel = 3,
                stance = "Balanced",
                encounterCount = 1
            };

            // Surface should run without errors and set LastSurfaced
            bridge.Surface(trigger);
            var surfaced = bridge.LastSurfaced;
            Assert.NotNull(surfaced);
            Assert.False(string.IsNullOrEmpty(surfaced!.encounter_id));
        }

        [Fact]
        public void CooldownTracking_PreventsImmediateResurfacing()
        {
            var travelSys = new TravelEncounterSystem(_patrolCatalog);
            var inv = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            inv.TryProduce("canned_food", 10);
            travelSys.Inventory = inv;

            // Resolve enc_patrol_garrison_checkpoint on Day 1 (default cooldown 5 days -> available Day 6)
            bool ok = travelSys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var res);
            Assert.True(ok);

            // On Day 2, resolving again should fail due to cooldown
            bool okDay2 = travelSys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 2, out _);
            Assert.False(okDay2);

            // On Day 6, cooldown expires and choice can be taken again
            bool okDay6 = travelSys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 6, out var resDay6);
            Assert.True(okDay6);
            Assert.NotNull(resDay6);
        }

        [Fact]
        public void DeterministicSingleRngDraw_IdenticalSeedProducesIdenticalSurfaced()
        {
            var narrativeSys1 = new NarrativeEncounterSystem();
            narrativeSys1.RegisterRange(_narrativeCatalog);
            var travelSys1 = new TravelEncounterSystem(_patrolCatalog);
            var bridge1 = new ExpeditionEncounterBridge(narrativeSys1, new SeededRng(999))
            {
                TravelEngine = travelSys1,
                CurrentDay = 5,
                RegionResolver = _ => "high_scarp"
            };

            var narrativeSys2 = new NarrativeEncounterSystem();
            narrativeSys2.RegisterRange(_narrativeCatalog);
            var travelSys2 = new TravelEncounterSystem(_patrolCatalog);
            var bridge2 = new ExpeditionEncounterBridge(narrativeSys2, new SeededRng(999))
            {
                TravelEngine = travelSys2,
                CurrentDay = 5,
                RegionResolver = _ => "high_scarp"
            };

            var trigger = new ExpeditionState
            {
                locationId = "loc_high_scarp",
                dangerLevel = 2,
                stance = "Cautious",
                encounterCount = 1
            };

            bridge1.Surface(trigger);
            bridge2.Surface(trigger);
            var s1 = bridge1.LastSurfaced;
            var s2 = bridge2.LastSurfaced;

            Assert.NotNull(s1);
            Assert.NotNull(s2);
            Assert.Equal(s1!.encounter_id, s2!.encounter_id);
            Assert.Equal(s1.title, s2.title);
        }
    }
}
