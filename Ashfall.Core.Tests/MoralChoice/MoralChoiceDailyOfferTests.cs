// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;
using Xunit;

namespace Ashfall.Core.Tests.MoralChoice
{
    public sealed class MoralChoiceDailyOfferTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        private static MoralChoiceSystem BuildSystem(int seed = 42)
        {
            var system = new MoralChoiceSystem(new SeededRng(seed));
            string dataDir = ResolveDataDir();
            var quests = MoralChoiceCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            system.RegisterQuests(quests);
            return system;
        }

        [Fact]
        public void CatalogRegistration_PopulatesAuthoritativeQuests()
        {
            var system = BuildSystem();
            Assert.True(system.CatalogCount >= 40, $"Expected at least 40 moral choice quests, got {system.CatalogCount}");

            var quest = system.GetQuest("quest_moral_share_child");
            Assert.NotNull(quest);
            Assert.Equal("quest_moral_share_child", quest.Id);
            Assert.NotEmpty(quest.Choices);
        }

        [Fact]
        public void DailyOffers_AreDeterministicAcross30Days()
        {
            var system1 = BuildSystem(seed: 1337);
            var system2 = BuildSystem(seed: 1337);

            for (int day = 1; day <= 30; day++)
            {
                var offers1 = system1.GetDailyOffers(day, maxOffers: 2);
                var offers2 = system2.GetDailyOffers(day, maxOffers: 2);

                Assert.Equal(offers1.Count, offers2.Count);
                for (int i = 0; i < offers1.Count; i++)
                {
                    Assert.Equal(offers1[i].Id, offers2[i].Id);
                    Assert.True(MoralChoiceSystem.IsAvailableOnDay(offers1[i], day),
                        $"Offer {offers1[i].Id} must be available on day {day}");
                }
            }
        }

        [Fact]
        public void DailyOffers_ExcludesAlreadyResolvedQuests()
        {
            var system = BuildSystem(seed: 2026);
            var offersDay1 = system.GetDailyOffers(1, maxOffers: 1);
            Assert.NotEmpty(offersDay1);

            var target = offersDay1[0];
            bool resolved = system.TryResolve(target.Id, choiceIndex: 0, locationId: "loc_shelter", day: 1, out var res);
            Assert.True(resolved);
            Assert.Equal(MoralResolveResultCode.Success, res.Code);

            // Re-query offers for day 1
            var offersAfter = system.GetDailyOffers(1, maxOffers: 5);
            Assert.DoesNotContain(offersAfter, q => q.Id == target.Id);
        }

        [Fact]
        public void TryResolve_StrictSingleResolutionAndIdempotence()
        {
            var system = BuildSystem(seed: 9999);
            var quest = system.Catalog.Values.First(q => q.MinDay <= 1 && (q.MaxDay <= 0 || q.MaxDay >= 1));

            // First resolution
            bool first = system.TryResolve(quest.Id, choiceIndex: 0, locationId: "shelter", day: 1, out var res1);
            Assert.True(first);
            Assert.Equal(MoralResolveResultCode.Success, res1.Code);
            Assert.NotNull(res1.Resolution);

            int scoreAfterFirst = system.MoralScore;
            int empathyAfterFirst = system.EmpathyPoints;

            // Second resolution: idempotent rejection with stored outcome
            bool second = system.TryResolve(quest.Id, choiceIndex: 0, locationId: "shelter", day: 1, out var res2);
            Assert.False(second);
            Assert.Equal(MoralResolveResultCode.AlreadyResolved, res2.Code);
            Assert.NotNull(res2.Resolution);
            Assert.Equal(res1.Resolution.outcomeRoll, res2.Resolution.outcomeRoll);

            // Scores must not change
            Assert.Equal(scoreAfterFirst, system.MoralScore);
            Assert.Equal(empathyAfterFirst, system.EmpathyPoints);
        }

        [Fact]
        public void TryResolve_FailsWithUnknownChoice_ForNonExistentQuest()
        {
            var system = BuildSystem();
            bool ok = system.TryResolve("quest_moral_nonexistent_999", 0, "shelter", 1, out var res);
            Assert.False(ok);
            Assert.Equal(MoralResolveResultCode.UnknownChoice, res.Code);
        }

        [Fact]
        public void TryResolve_FailsWithUnknownOption_WhenIndexOutOfBounds()
        {
            var system = BuildSystem();
            var quest = system.Catalog.Values.First();
            bool ok = system.TryResolve(quest.Id, choiceIndex: 999, locationId: "shelter", day: 1, out var res);
            Assert.False(ok);
            Assert.Equal(MoralResolveResultCode.UnknownOption, res.Code);
        }

        [Fact]
        public void TryResolve_FailsWithChoiceNotAvailable_OutsideDayWindow()
        {
            var system = BuildSystem();
            var def = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_late_game",
                DisplayName = "Late Game Dilemma",
                MinDay = 50,
                MaxDay = 60,
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Do something", MoralDelta = 5, EmpathyDelta = 2 }
                }
            };
            system.RegisterQuest(def);

            bool ok = system.TryResolve(def.Id, 0, "shelter", day: 5, out var res);
            Assert.False(ok);
            Assert.Equal(MoralResolveResultCode.ChoiceNotAvailable, res.Code);
        }

        [Fact]
        public void SaveAndRestore_PreservesOffersAndResolutionsDeterministically()
        {
            var system1 = BuildSystem(seed: 777);
            var offers = system1.GetDailyOffers(5, maxOffers: 2);
            Assert.NotEmpty(offers);

            // Resolve first offer
            system1.TryResolve(offers[0].Id, 0, "loc_test", day: 5, out _);

            // Capture state
            var state = system1.CaptureState();

            // Restore in fresh system with same seed
            var system2 = BuildSystem(seed: 777);
            system2.RestoreState(state);

            // Verify state match
            Assert.Equal(system1.MoralScore, system2.MoralScore);
            Assert.Equal(system1.EmpathyPoints, system2.EmpathyPoints);
            Assert.Equal(system1.QuestsResolved, system2.QuestsResolved);

            // Re-query day 5 offers in both systems
            var offers1 = system1.GetDailyOffers(5, maxOffers: 2);
            var offers2 = system2.GetDailyOffers(5, maxOffers: 2);

            Assert.Equal(offers1.Count, offers2.Count);
            for (int i = 0; i < offers1.Count; i++)
            {
                Assert.Equal(offers1[i].Id, offers2[i].Id);
            }
        }

        [Fact]
        public void RestoreState_PreservesUnknownModQuestResolutions()
        {
            var system = BuildSystem(seed: 1234);
            var state = system.CaptureState();

            // Inject a resolution from an unknown/expansion quest
            state.resolutions.Add(new MoralChoiceResolution
            {
                questId = "quest_moral_mod_special_99",
                choiceIndex = 1,
                resolvedDay = 15,
                moralDelta = 10,
                empathyDelta = 5,
                impactMark = "up",
                outcomeRoll = 42
            });

            var restoredSystem = BuildSystem(seed: 1234);
            restoredSystem.RestoreState(state);

            Assert.True(restoredSystem.IsResolved("quest_moral_mod_special_99"));
            Assert.True(restoredSystem.TryGetResolution("quest_moral_mod_special_99", out var res));
            Assert.Equal(42, res!.outcomeRoll);
        }
    }
}
