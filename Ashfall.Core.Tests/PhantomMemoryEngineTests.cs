using System;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class PhantomMemoryEngineTests
    {
        [Fact]
        public void OnItemScavenged_WithMatchingBackground_ReturnsOutcome()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f; // always trigger
            engine.RegisterRule("former_soldier", "military", 1.0f, "desc", "motivation", "breakdown");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_test",
                displayName = "Test",
                backgroundId = "former_soldier",
                isAlive = true
            };

            var rng = new SeededRng(42);
            var outcome = engine.OnItemScavenged(sv, "item_dog_tags", rng);
            Assert.Equal(TriggerOutcome.Motivation, outcome);

            var records = engine.Records;
            Assert.Single(records);
            Assert.Equal("sv_test", records[0].survivorId);
            Assert.Equal(1, records[0].triggersExperienced);
        }

        [Fact]
        public void OnItemScavenged_WithDeadSurvivor_ReturnsNone()
        {
            var engine = new PhantomMemoryEngine();
            engine.RegisterRule("generic", "photograph", 0.5f, "desc");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_dead",
                backgroundId = "generic",
                isAlive = false
            };

            var outcome = engine.OnItemScavenged(sv, "photo_album", new SeededRng(0));
            Assert.Equal(TriggerOutcome.None, outcome);
        }

        [Fact]
        public void TickHour_DecaysMotivationBoost()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("teacher", "correspondence", 1.0f, "desc", "motivation", "breakdown");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_teacher",
                backgroundId = "teacher",
                isAlive = true
            };

            var rng = new SeededRng(1);
            var outcome = engine.OnItemScavenged(sv, "letter_unsent", rng);
            Assert.Equal(TriggerOutcome.Motivation, outcome);

            Assert.True(engine.HasMotivationBoost("sv_teacher"));
            engine.TickHour("sv_teacher", 4f);
            Assert.True(engine.HasMotivationBoost("sv_teacher"));

            engine.TickHour("sv_teacher", 5f);
            Assert.False(engine.HasMotivationBoost("sv_teacher"));
        }

        [Fact]
        public void ResolveTriggerText_SubstitutesName()
        {
            var engine = new PhantomMemoryEngine();
            engine.RegisterRule("nurse", "medical", 0.5f, "desc",
                "{name} the medic is inspired.", "{name} the medic breaks down.");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_nurse",
                displayName = "Anna",
                backgroundId = "nurse",
                isAlive = true
            };

            string motivationText = engine.ResolveTriggerText(sv, "medical_bandage", true);
            Assert.Contains("Anna", motivationText);

            string breakdownText = engine.ResolveTriggerText(sv, "medical_bandage", false);
            Assert.Contains("Anna", breakdownText);
        }

        [Fact]
        public void CaptureRestore_RoundTripsRecords()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("child_refugee", "childhood", 1.0f, "desc");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_child",
                backgroundId = "child_refugee",
                isAlive = true
            };

            engine.OnItemScavenged(sv, "toy_bear", new SeededRng(1));
            engine.TickHour("sv_child", 2f);

            var state = engine.CaptureState();
            var engineB = new PhantomMemoryEngine();
            engineB.TriggerChanceOverride = 1.0f;
            engineB.RegisterRule("child_refugee", "childhood", 1.0f, "desc");
            engineB.RestoreState(state);

            Assert.Equal(1, engineB.GetTriggersExperienced("sv_child"));
            Assert.True(engineB.HasMotivationBoost("sv_child"));
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("former_soldier", "military", 1.0f, "desc");
            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_soldier",
                backgroundId = "former_soldier",
                isAlive = true
            };
            engine.OnItemScavenged(sv, "dog_tags", new SeededRng(1));

            var snapshot = engine.CaptureState();
            Assert.Single(snapshot.records);

            // Mutating the captured envelope must not touch the live system,
            // and a later trigger must not leak into the captured envelope.
            snapshot.records[0].triggeredItemIds.Add("injected");
            engine.OnItemScavenged(sv, "military_patch", new SeededRng(2));
            Assert.True(snapshot.records.Count == 1,
                "mutating the live system must not grow the captured envelope");

            // The envelope mutation must not leak into the live engine, and the
            // later live trigger must not leak into the earlier envelope.
            var fresh = engine.CaptureState();
            Assert.Single(fresh.records);
            Assert.DoesNotContain(fresh.records[0].triggeredItemIds, id => id == "injected");
            Assert.Contains(fresh.records[0].triggeredItemIds, id => id == "military_patch");
            Assert.DoesNotContain(snapshot.records[0].triggeredItemIds, id => id == "military_patch");
        }

        [Fact]
        public void CaptureState_EmitsRecordsInOrdinalOrder()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("generic", "personal_item", 1.0f, "desc");

            // Insert in non-ordinal order; the capture must still sort.
            var svB = new PhantomSurvivorSnapshot { survivorId = "sv_beta", backgroundId = "generic", isAlive = true };
            var svA = new PhantomSurvivorSnapshot { survivorId = "sv_alpha", backgroundId = "generic", isAlive = true };
            engine.OnItemScavenged(svB, "ring", new SeededRng(1));
            engine.OnItemScavenged(svA, "ring", new SeededRng(1));

            var snapshot = engine.CaptureState();
            Assert.Equal(2, snapshot.records.Count);
            Assert.Equal("sv_alpha", snapshot.records[0].survivorId);
            Assert.Equal("sv_beta", snapshot.records[1].survivorId);
        }

        [Fact]
        public void GetWorkEfficiencyMultiplier_BoostedWhileMotivated()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("nurse", "medical", 1.0f, "desc", "motivated", "breakdown");
            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_mot", backgroundId = "nurse", isAlive = true };

            var outcome = engine.OnItemScavenged(sv, "medical_bandage", new Ashfall.Core.SeededRng(5));
            Assert.Equal(TriggerOutcome.Motivation, outcome);

            Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus,
                engine.GetWorkEfficiencyMultiplier("sv_mot"), 4);

            engine.TickHour("sv_mot", 9f);
            Assert.Equal(1f, engine.GetWorkEfficiencyMultiplier("sv_mot"), 4);
        }

        [Fact]
        public void GetWorkRefusalHours_SetOnBreakdownAndDecays()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            // motivationChance 0 → any trigger is a breakdown.
            engine.RegisterRule("former_soldier", "military", 0f, "desc", "motivated", "breakdown");
            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_brk", backgroundId = "former_soldier", isAlive = true };

            var outcome = engine.OnItemScavenged(sv, "item_dog_tags", new Ashfall.Core.SeededRng(7));
            Assert.Equal(TriggerOutcome.Breakdown, outcome);

            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours,
                engine.GetWorkRefusalHours("sv_brk"), 4);

            engine.TickHour("sv_brk", 2f);
            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours - 2f,
                engine.GetWorkRefusalHours("sv_brk"), 4);

            engine.TickHour("sv_brk", 3f);
            Assert.Equal(0f, engine.GetWorkRefusalHours("sv_brk"), 4);
        }

        [Fact]
        public void GetWorkRefusalHours_UnknownSurvivor_Zero()
        {
            var engine = new PhantomMemoryEngine();
            Assert.Equal(0f, engine.GetWorkRefusalHours("nobody"));
            Assert.Equal(1f, engine.GetWorkEfficiencyMultiplier("nobody"), 4);
        }

        [Fact]
        public void CaptureRestore_RoundTripsRefusalHours()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRule("former_soldier", "military", 0f, "desc", "motivated", "breakdown");
            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_brk", backgroundId = "former_soldier", isAlive = true };
            engine.OnItemScavenged(sv, "item_dog_tags", new Ashfall.Core.SeededRng(7));
            engine.TickHour("sv_brk", 2f);

            var state = engine.CaptureState();
            var fresh = new PhantomMemoryEngine();
            fresh.RestoreState(state);

            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours - 2f,
                fresh.GetWorkRefusalHours("sv_brk"), 4);
        }

        [Fact]
        public void PhantomTriggerCatalogJson_Deserialization_Passes()
        {
            string sampleJson = @"{
                ""schema_version"": 1,
                ""items"": [
                    {
                        ""background_id"": ""former_soldier"",
                        ""triggers"": [
                            {
                                ""item_category"": ""military"",
                                ""motivation_chance"": 0.4,
                                ""description"": ""Dog tags."",
                                ""motivation_text"": ""Pockets tags."",
                                ""breakdown_text"": ""Goes pale.""
                            }
                        ]
                    }
                ]
            }";

            var serializer = new SystemTextJsonSerializer();
            var catalog = serializer.Deserialize<Ashfall.Core.Phantoms.PhantomTriggerCatalogJson>(sampleJson);
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog.schema_version);
            Assert.Single(catalog.items);
            Assert.Equal("former_soldier", catalog.items[0].background_id);
            Assert.Single(catalog.items[0].triggers);
            Assert.Equal("military", catalog.items[0].triggers[0].item_category);
        }

        [Fact]
        public void TaxonomyCategoryInference_RecognizesCategories()
        {
            Assert.Equal("childhood", PhantomMemoryEngine.GetCategoryFromId("childs_mitten"));
            Assert.Equal("work_tool", PhantomMemoryEngine.GetCategoryFromId("machinist_caliper"));
            Assert.Equal("ordinary_object", PhantomMemoryEngine.GetCategoryFromId("bus_ticket"));
            Assert.Equal("medical", PhantomMemoryEngine.GetCategoryFromId("stethoscope"));
            Assert.Equal("military", PhantomMemoryEngine.GetCategoryFromId("dog_tags"));
        }

        [Fact]
        public void RegisterRuleDetailed_ResolvesSpecificItemIdOverCategory()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_generic_tool",
                itemCategory = "work_tool",
                itemId = "",
                motivationChance = 0.5f,
                descriptionKey = "generic tool description"
            }, "generic");

            engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_specific_caliper",
                itemCategory = "work_tool",
                itemId = "machinist_caliper",
                motivationChance = 1.0f,
                descriptionKey = "specific caliper description"
            }, "generic");

            var rules = engine.GetRules("generic");
            Assert.Equal(2, rules.Count);
            Assert.Contains(rules, r => r.triggerId == "rule_specific_caliper" && r.itemId == "machinist_caliper");
        }

        [Fact]
        public void LoreOnlyTrigger_DoesNotRollMotivationOrBreakdown()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_lore",
                itemCategory = "photograph",
                itemId = "family_photograph",
                loreOnly = true,
                descriptionKey = "A photograph from before."
            }, "generic");

            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_1", backgroundId = "generic", isAlive = true };
            var outcome = engine.OnItemScavenged(sv, "family_photograph", new SeededRng(10));
            Assert.Equal(TriggerOutcome.None, outcome);
            Assert.False(engine.HasMotivationBoost("sv_1"));
            Assert.Equal(0f, engine.GetWorkRefusalHours("sv_1"));
        }

        [Fact]
        public void OneShotTrigger_DoesNotRepeat()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_oneshot",
                itemCategory = "personal_item",
                itemId = "wedding_ring",
                repeatable = false,
                motivationChance = 1.0f,
                descriptionKey = "A wedding ring."
            }, "generic");

            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_1", backgroundId = "generic", isAlive = true };
            var outcome1 = engine.OnItemScavenged(sv, "wedding_ring", new SeededRng(1));
            Assert.Equal(TriggerOutcome.Motivation, outcome1);

            // Second scavenge of same item must return None because seen
            var outcome2 = engine.OnItemScavenged(sv, "wedding_ring", new SeededRng(2));
            Assert.Equal(TriggerOutcome.None, outcome2);
        }

        [Fact]
        public void OnPhantomMemoryEvent_RaisesMemoryEventWithPayloads()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_payload",
                itemCategory = "military",
                itemId = "dog_tags",
                motivationChance = 1.0f,
                affinityTrait = "former_soldier",
                moralePayload = 20.0f,
                guiltPayload = 10.0f,
                descriptionKey = "Dog tags found."
            }, "former_soldier");

            string caughtSurvivor = null;
            string caughtItem = null;
            float caughtMorale = 0f;
            engine.OnPhantomMemoryResolved += (survivorId, itemId, isMotivation, moraleDelta, guiltDelta) =>
            {
                caughtSurvivor = survivorId;
                caughtItem = itemId;
                caughtMorale = moraleDelta;
            };

            var sv = new PhantomSurvivorSnapshot { survivorId = "sv_1", backgroundId = "former_soldier", isAlive = true };
            engine.OnItemScavenged(sv, "dog_tags", new SeededRng(1));

            Assert.Equal("sv_1", caughtSurvivor);
            Assert.Equal("dog_tags", caughtItem);
            Assert.True(caughtMorale > 0);
        }

        private sealed class SeededRng : ISeededRng
        {
            private readonly System.Random _rng;
            public int Seed { get; }
            public SeededRng(int seed) { Seed = seed; _rng = new System.Random(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => (float)_rng.NextDouble();
            public double NextDouble() => _rng.NextDouble();
        }
    }
}
