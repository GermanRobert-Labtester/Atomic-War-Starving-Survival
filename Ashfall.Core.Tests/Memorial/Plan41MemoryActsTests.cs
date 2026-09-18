// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Phantoms;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public class Plan41MemoryActsTests
    {
        [Fact]
        public void MemorialSystem_WithEulogyEngine_ComposesAndPersistsEulogy()
        {
            var eulogyEngine = new ProceduralEulogyEngine();
            var state = new MemorialState();
            var system = new MemorialSystem(state)
            {
                EulogyEngine = eulogyEngine
            };

            var input = new MemorialInput
            {
                SurvivorId = "survivor_viktor",
                Cause = "exhaustion",
                Day = 120,
                BirthDay = 10,
                HeirloomItemId = "pocket_watch",
                LifeRecord = new DwellerLifeRecord
                {
                    dwellerId = "survivor_viktor",
                    dwellerName = "Viktor",
                    preWarProfession = "Watchmaker",
                    daysSurvived = 110,
                    shiftsCompleted = 60,
                    causeOfDeath = "exhaustion",
                    favoriteRelicName = "Silver Pocket Watch",
                    memorableBarkSnippets = new List<string> { "Keep the springs wound." }
                }
            };

            var entry = system.Memorialize(input);

            Assert.NotNull(entry);
            Assert.NotEmpty(entry.EulogyText);
            Assert.Contains("MEMORIAL INSCRIPTION: VIKTOR", entry.EulogyText);
            Assert.Contains("Watchmaker", entry.EulogyText);
            Assert.Contains("Silver Pocket Watch", entry.EulogyText);
            Assert.Contains("Keep the springs wound.", entry.EulogyText);

            // Test capture/restore preserves eulogy text
            var captured = system.CaptureState();
            var restoredSystem = new MemorialSystem(new MemorialState());
            restoredSystem.RestoreState(captured);

            Assert.Single(restoredSystem.Entries);
            Assert.Equal(entry.EulogyText, restoredSystem.Entries[0].EulogyText);
        }

        [Fact]
        public void HeirloomSystem_HolderModifiers_CalculatedAndBounded()
        {
            var catalog = new HeirloomCatalog();
            var serializer = new SystemTextJsonSerializer();
            string json = @"{
                ""schema_version"": 1,
                ""items"": [
                    {
                        ""heirloom_id"": ""watch_gold"",
                        ""title"": ""Gold Watch"",
                        ""holder_memories"": [
                            { ""affinity_key"": ""generic"", ""memory_text"": ""Ticking."", ""morale_effect"": 8.0, ""guilt_effect"": 0.0 }
                        ]
                    },
                    {
                        ""heirloom_id"": ""locket_silver"",
                        ""title"": ""Silver Locket"",
                        ""holder_memories"": [
                            { ""affinity_key"": ""generic"", ""memory_text"": ""A face inside."", ""morale_effect"": 6.0, ""guilt_effect"": 0.0 }
                        ]
                    }
                ]
            }";
            catalog.Load(json, serializer);
            var system = new HeirloomSystem(catalog);

            // Create instances and assign to holder
            var inst1 = system.CreateInstance("watch_gold", "dweller_anna", 10);
            var inst2 = system.CreateInstance("locket_silver", "dweller_anna", 12);

            float morale = system.GetHolderMoraleModifier("dweller_anna");
            Assert.Equal(14.0f, morale);

            float fatigueRelief = system.GetHolderFatigueRelief("dweller_anna");
            Assert.Equal(1.0f, fatigueRelief); // 2 heirlooms * 0.5f

            // Transfer one away: modifier updates dynamically
            system.AssignHolder(inst1.instance_id, "dweller_boris", 20);
            Assert.Equal(6.0f, system.GetHolderMoraleModifier("dweller_anna"));
            Assert.Equal(8.0f, system.GetHolderMoraleModifier("dweller_boris"));
            Assert.Equal(0.5f, system.GetHolderFatigueRelief("dweller_anna"));
            Assert.Equal(0.5f, system.GetHolderFatigueRelief("dweller_boris"));
        }

        [Fact]
        public void LocationMemorySystem_TracksAndPersistsVisits()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new LocationMemorySystem(files, json);

            Assert.False(system.HasVisited("bunker_outpost_3"));
            Assert.Equal(0, system.GetSiteVisitCount("bunker_outpost_3"));
            Assert.Equal(0, system.GetLastVisitDay("bunker_outpost_3"));

            system.RecordSiteVisit("bunker_outpost_3", 15);
            Assert.True(system.HasVisited("bunker_outpost_3"));
            Assert.Equal(1, system.GetSiteVisitCount("bunker_outpost_3"));
            Assert.Equal(15, system.GetLastVisitDay("bunker_outpost_3"));

            system.RecordSiteVisit("bunker_outpost_3", 25);
            Assert.Equal(2, system.GetSiteVisitCount("bunker_outpost_3"));
            Assert.Equal(25, system.GetLastVisitDay("bunker_outpost_3"));

            // Round-trip save
            var saved = system.CaptureState();
            var restoredSystem = new LocationMemorySystem(files, json);
            restoredSystem.RestoreState(saved);

            Assert.True(restoredSystem.HasVisited("bunker_outpost_3"));
            Assert.Equal(2, restoredSystem.GetSiteVisitCount("bunker_outpost_3"));
            Assert.Equal(25, restoredSystem.GetLastVisitDay("bunker_outpost_3"));
        }

        [Fact]
        public void CohortSystem_CheckCohortMaturation_MaturesEligibleChildren()
        {
            var system = new CohortSystem();
            system.BookChild("child_1", new List<string> { "p1" }, "medium", 10);
            system.BookChild("child_2", new List<string> { "p1" }, "low", 200);

            // Day 300: child_1 is 290 days old (< 365), child_2 is 100 days old (< 365)
            int matured = system.CheckCohortMaturation(300, 365);
            Assert.Equal(0, matured);
            Assert.False(system.GetChild("child_1")!.isMatured);

            // Day 375: child_1 is 365 days old (>= 365) -> matures!
            matured = system.CheckCohortMaturation(375, 365);
            Assert.Equal(1, matured);
            Assert.True(system.GetChild("child_1")!.isMatured);
            Assert.Equal(375, system.GetChild("child_1")!.maturationDay);
            Assert.False(system.GetChild("child_2")!.isMatured);

            // Subsequent call on day 376 does not re-mature child_1
            matured = system.CheckCohortMaturation(376, 365);
            Assert.Equal(0, matured);
        }
    }
}
