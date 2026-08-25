using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    public sealed class MoralChoiceCatalogTests : CatalogTestBase
    {
        private static List<MoralChoiceQuestDefinition> Load() =>
            MoralChoiceCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        [Fact]
        public void LoadsAllSixtyQuestsAcrossFiveCategories()
        {
            var quests = Load();
            Assert.Equal(60, quests.Count);

            var byCategory = quests.GroupBy(q => q.Category).ToDictionary(g => g.Key, g => g.Count());
            Assert.Equal(12, byCategory["share"]);
            Assert.Equal(12, byCategory["listen"]);
            Assert.Equal(12, byCategory["comfort"]);
            Assert.Equal(12, byCategory["dead"]);
            Assert.Equal(12, byCategory["trust"]);
        }

        [Fact]
        public void IdsAreCanonicalAndUnique()
        {
            var quests = Load();
            Assert.All(quests, q => Assert.StartsWith("quest_moral_", q.Id, StringComparison.Ordinal));

            var ids = quests.Select(q => q.Id).ToList();
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }

        [Fact]
        public void ProseIsPopulated()
        {
            var quests = Load();
            Assert.All(quests, q =>
            {
                Assert.False(string.IsNullOrWhiteSpace(q.DisplayName), $"{q.Id} display name empty");
                Assert.False(string.IsNullOrWhiteSpace(q.Trigger), $"{q.Id} trigger empty");
                Assert.False(string.IsNullOrWhiteSpace(q.Discovery), $"{q.Id} discovery empty");
            });
        }

        [Fact]
        public void ChoicesAreWellFormed()
        {
            var quests = Load();
            foreach (var quest in quests)
            {
                Assert.InRange(quest.Choices.Count, 3, 4);
                foreach (var choice in quest.Choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(choice.Label), $"{quest.Id} choice label empty");
                    Assert.False(string.IsNullOrWhiteSpace(choice.OutcomeText), $"{quest.Id} outcome empty");
                    Assert.False(string.IsNullOrWhiteSpace(choice.Epitaph), $"{quest.Id} epitaph empty");
                    Assert.InRange(choice.MoralDelta, -20, 22);
                    Assert.InRange(choice.EmpathyDelta, 0, 4);
                }

                // Every quest must offer at least one kind and one unkind-or-flat option.
                Assert.Contains(quest.Choices, c => c.MoralDelta > 0);
                Assert.Contains(quest.Choices, c => c.MoralDelta <= 0);
            }
        }

        [Fact]
        public void DayWindowsOrderedAndMessengerIsLateGame()
        {
            var quests = Load();
            Assert.All(quests, q => Assert.True(q.MaxDay <= 0 || q.MaxDay >= q.MinDay, $"{q.Id} window inverted"));

            var messenger = quests.Single(q => q.Id == "quest_moral_trust_messenger");
            Assert.Equal(200, messenger.MinDay);
            Assert.False(MoralChoiceSystem.IsAvailableOnDay(messenger, 199));
            Assert.True(MoralChoiceSystem.IsAvailableOnDay(messenger, 200));
        }

        [Fact]
        public void AllQuestsResolveThroughTheSystem()
        {
            var quests = Load();
            var sys = new MoralChoiceSystem(new SeededRng(42));

            int expectedEmpathy = 0;
            foreach (var quest in quests)
            {
                sys.Resolve(quest, 0, quest.LocationId, 5);
                expectedEmpathy += quest.Choices[0].EmpathyDelta;
            }

            Assert.Equal(60, sys.QuestsResolved);
            Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore); // 782 raw clamps at +200
            Assert.Equal(expectedEmpathy, sys.EmpathyPoints);
        }

        [Fact]
        public void MissingFileReturnsEmptyList()
        {
            var missing = Path.Combine(Path.GetTempPath(), "ashfall_no_such_data_dir");
            var loaded = MoralChoiceCatalogLoader.Load(missing, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(loaded);
        }

        [Fact]
        public void StaticIdClassMatchesCatalogExactly()
        {
            var quests = Load();
            var catalogIds = quests.Select(q => q.Id).ToHashSet();

            Assert.Equal(MoralChoiceIds.QuestCount, catalogIds.Count);
            Assert.Equal(MoralChoiceIds.QuestCount, MoralChoiceIds.All.Length);
            Assert.Equal(MoralChoiceIds.All.Length, MoralChoiceIds.All.Distinct().Count());

            // Every static id is a real catalog quest, and no catalog quest is
            // missing from the static class.
            Assert.True(catalogIds.SetEquals(MoralChoiceIds.All),
                "MoralChoiceIds.All and the catalog quest ids must be the same set");

            // The flag ids carry the canonical flag_ prefix.
            Assert.StartsWith("flag_moral_", MoralChoiceIds.FlagMessengerKept, StringComparison.Ordinal);
        }

        [Fact]
        public void LoadsAreStable()
        {
            var first = Load();
            var second = Load();
            Assert.Equal(first.Count, second.Count);
            Assert.Equal(first[0].Id, second[0].Id);
            Assert.Equal(first[^1].Id, second[^1].Id);
            Assert.Equal(first[0].Choices[0].MoralDelta, second[0].Choices[0].MoralDelta);
        }
    }
}
