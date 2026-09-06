using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan169ProceduralNarrativeTests
    {
        private sealed class FixedRng : ISeededRng
        {
            private readonly double _value;
            public FixedRng(double value) => _value = value;
            public int Seed => 169;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)_value;
            public double NextDouble() => _value;
        }

        private static QuestTemplateDef Template(string id, string module, string mergePolicy = "never")
            => new QuestTemplateDef
            {
                Id = id,
                Category = "test",
                Weight = 1f,
                MinimumDay = 1,
                ActorRoleSlots = new List<string> { "requester" },
                LocationConstraints = new List<string> { "ruin" },
                ObjectiveModules = new List<string> { module },
                RewardModules = new List<string> { "research_points" },
                FailureModules = new List<string> { "morale_event" },
                TimeLimitDays = 3,
                MergePolicy = mergePolicy,
                MaxConcurrentInstances = 1,
                TitleKey = "quest.test.title",
                DescriptionKey = "quest.test.description"
            };

        private static NarrativeWorldSnapshot Snapshot(params string[] tags)
            => new NarrativeWorldSnapshot
            {
                day = 5,
                worldTags = tags.ToList(),
                aliveSurvivorIds = new List<string> { "survivor_1" },
                knownLocationIds = new List<string> { "loc_ruin_1" },
                obtainableItemIds = new List<string> { "item_water_filter_advanced" }
            };

        [Fact]
        public void EligibilityIsPureAndGeneratedQuestBindsCanonicalState()
        {
            var system = new ProceduralNarrativeSystem();
            system.LoadTemplateCatalog(new[] { Template("quest_template_test", "repair_pipe") });
            var snapshot = Snapshot("water_crisis");

            int rngCalls = 0;
            var rng = new CountingRng(() => rngCalls++);
            var eligible = system.GetEligibleTemplates(snapshot);
            Assert.Single(eligible);
            Assert.Equal(0, rngCalls);

            Assert.True(system.TryGenerate(snapshot, rng, out var draft));
            Assert.Equal("quest_template_test", draft.quest.definitionId);
            Assert.Equal("survivor_1", draft.quest.actorBindings["requester"]);
            Assert.Equal("loc_ruin_1", draft.quest.locationBindings[0]);
            Assert.Equal("repair_pipe", draft.quest.objectiveStates[0].moduleId);
            Assert.Equal(8, draft.quest.expiryDay);
        }

        [Fact]
        public void ProtectedCanonTargetsAreRejectedBeforeRegistration()
        {
            var system = new ProceduralNarrativeSystem();
            system.LoadTemplateCatalog(new[] { Template("quest_template_test", "repair_pipe") });
            var snapshot = Snapshot("water_crisis");
            snapshot.protectedActorIds.Add("survivor_1");
            snapshot.protectedLocationIds.Add("loc_ruin_1");

            Assert.Empty(system.GetEligibleTemplates(snapshot));
            Assert.False(system.TryGenerate(snapshot, new FixedRng(0), out var draft));
            Assert.Equal("no_eligible_template", draft.rejectionReason);
        }

        [Fact]
        public void SharedCoordinatorExposesProceduralQuestAndExpiresOnce()
        {
            var narrative = new ProceduralNarrativeSystem();
            narrative.LoadTemplateCatalog(new[] { Template("quest_template_test", "repair_pipe") });
            Assert.True(narrative.TryGenerate(Snapshot("water_crisis"), new FixedRng(0), out var draft));
            var runtime = new QuestRuntimeCoordinator();
            int expired = 0;
            runtime.OnQuestExpired += _ => expired++;
            Assert.True(runtime.Register(draft.quest));
            Assert.Single(runtime.BuildReadModel());

            runtime.Tick(8);
            runtime.Tick(9);

            Assert.Equal(1, expired);
            Assert.Equal(QuestLifecycleState.Expired, runtime.Find(draft.quest.instanceId)!.status);
            var saved = runtime.CaptureState();
            var restored = new QuestRuntimeCoordinator();
            restored.RestoreState(saved);
            Assert.Equal(QuestLifecycleState.Expired, restored.Find(draft.quest.instanceId)!.status);
        }

        [Fact]
        public void CompatibleProceduralQuestsMergeWithoutLosingProvenance()
        {
            var narrative = new ProceduralNarrativeSystem();
            narrative.LoadTemplateCatalog(new[]
            {
                Template("quest_template_a", "repair_pipe", "same_location"),
                Template("quest_template_b", "trace_contamination", "same_location")
            });
            Assert.True(narrative.TryGenerate(Snapshot("water_crisis"), new FixedRng(0), out var first));
            Assert.True(narrative.TryGenerate(Snapshot("water_crisis"), new FixedRng(0), out var second));
            first.quest.locationBindings[0] = "loc_shared";
            second.quest.locationBindings[0] = "loc_shared";
            second.quest.definitionId = "quest_template_b";

            Assert.True(narrative.TryMerge(first.quest, second.quest, out var merged));
            Assert.Equal(2, merged.childInstanceIds.Count);
            Assert.Equal(2, merged.objectiveStates.Count);
        }

        [Fact]
        public void NarrativeStateRoundTripPreservesCooldownAndRivalry()
        {
            var system = new ProceduralNarrativeSystem();
            system.LoadTemplateCatalog(new[] { Template("quest_template_test", "repair_pipe") });
            Assert.True(system.TryGenerate(Snapshot("water_crisis"), new FixedRng(0), out _));
            system.CreateRivalry("survivor_1", "npc_rival", "incident_1", 5, 0.8f);
            var saved = system.CaptureState();

            var restored = new ProceduralNarrativeSystem();
            restored.RestoreState(saved);
            Assert.Equal(1, restored.State.rivalries.Count);
            Assert.Equal(0.8f, restored.State.rivalries[0].intensity01, 4);
            Assert.Equal(saved.generationSequence, restored.State.generationSequence);
            Assert.Equal(saved.templateCooldowns["quest_template_test"], restored.State.templateCooldowns["quest_template_test"]);
        }

        [Fact]
        public void TemplateCatalogLoadsAndValidatesAuthoritativeData()
        {
            string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            string dataDir = System.IO.Path.Combine(root, "Assets", "StreamingAssets", "Data");
            var templates = QuestTemplateCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(3, templates.Count);
            Assert.True(QuestTemplateCatalogLoader.Validate(templates, out var error), error);
        }

        private sealed class CountingRng : ISeededRng
        {
            private readonly Action _onCall;
            public CountingRng(Action onCall) => _onCall = onCall;
            public int Seed => 169;
            public int Next(int minInclusive, int maxExclusive) { _onCall(); return minInclusive; }
            public float NextFloat() { _onCall(); return 0f; }
            public double NextDouble() { _onCall(); return 0d; }
        }
    }
}
