// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Relations;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan44SurvivorRelationsIntegrationTests
    {
        [Theory]
        [InlineData(-80f, "hostile", -0.25f, 0.25f)]
        [InlineData(-25f, "strained", -0.10f, 0.10f)]
        [InlineData(5f, "cordial", 0.0f, 0.0f)]
        [InlineData(40f, "close", 0.10f, -0.08f)]
        [InlineData(75f, "bonded", 0.20f, -0.15f)]
        public void BandResolution_AffinityMapsToCorrectBand(
            float affinity,
            string expectedBandId,
            float expectedWorkMod,
            float expectedErrorRiskMod)
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            relations.GetOrCreateRelationship("dweller_a", "dweller_b").affinity = affinity;

            var effect = relations.GetRelationEffect("dweller_a", "dweller_b");

            Assert.Equal(expectedBandId, effect.BandId);
            Assert.Equal(expectedWorkMod, effect.WorkingModifier, 2);
            Assert.Equal(expectedErrorRiskMod, effect.ErrorRiskModifier, 2);
        }

        [Fact]
        public void ModifyAffinity_RecordsTraceableHistoryEntry()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            relations.TickDay(3);
            relations.ModifyAffinity("sarah", "jonah", 30f);

            var rel = relations.GetOrCreateRelationship("sarah", "jonah");
            Assert.Single(rel.history);

            var entry = rel.history[0];
            Assert.Equal(3, entry.Day);
            Assert.Equal(30f, entry.Delta);
            Assert.Equal("close", entry.ResultingBand);
            Assert.Contains("sarah", entry.EventId);
            Assert.Contains("jonah", entry.EventId);
        }

        [Fact]
        public void History_BoundedAtMaxEntries_PrunesOldest()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));

            for (int i = 1; i <= 15; i++)
            {
                relations.TickDay(i);
                relations.RecordPairHistory(
                    "elena", "marcus",
                    $"cause_{i}",
                    "shared_shift",
                    5f,
                    "duty_roster",
                    "relations.shift_success"
                );
            }

            var rel = relations.GetOrCreateRelationship("elena", "marcus");
            Assert.Equal(SurvivorRelationsSystem.MaxHistoryPerPair, rel.history.Count);
            // Oldest entries 1..5 should be pruned, remaining should be 6..15
            Assert.Equal(6, rel.history[0].Day);
            Assert.Equal(15, rel.history[^1].Day);
        }

        [Fact]
        public void TeamAggregation_CalculatesAverageWorkAndDominantRisk()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            // Sarah & Jonah are hostile (work: -0.25, risk: 0.25)
            relations.GetOrCreateRelationship("sarah", "jonah").affinity = -60f;
            // Sarah & Marcus are close (work: 0.10, risk: -0.08)
            relations.GetOrCreateRelationship("sarah", "marcus").affinity = 40f;
            // Jonah & Marcus are cordial (work: 0.0, risk: 0.0)
            relations.GetOrCreateRelationship("jonah", "marcus").affinity = 0f;

            var team = new[] { "sarah", "jonah", "marcus" };
            var agg = relations.AggregateTeamEffect(team);

            Assert.Equal(3, agg.TotalPairs);
            Assert.Equal(1, agg.HostilePairsCount);
            Assert.Equal(0, agg.BondedPairsCount); // 'close' is not bonded
            // Avg work = (-0.25 + 0.10 + 0.0) / 3 = -0.05
            Assert.Equal(-0.05f, agg.AverageWorkingModifier, 2);
            // Dominant risk = max(0.25, -0.08, 0.0) = 0.25
            Assert.Equal(0.25f, agg.DominantRiskModifier, 2);
            Assert.Equal("relations.band.hostile", agg.DominantNoteKey);
        }

        [Fact]
        public void DelegateSeams_TriggerOnEvaluationAndRecord()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            RelationEffect? capturedEffect = null;
            PairRelationHistoryEntry? capturedHistory = null;

            relations.RelationEffectAppliedSeam = (a, b, effect) => capturedEffect = effect;
            relations.HistoryEntryRecordedSeam = entry => capturedHistory = entry;

            relations.GetOrCreateRelationship("clara", "david").affinity = 80f;
            var effect = relations.EffectOf("clara", "david");

            Assert.NotNull(capturedEffect);
            Assert.Equal("bonded", capturedEffect.Value.BandId);

            relations.RecordPairHistory("clara", "david", "gift", "gift_given", 10f);
            Assert.NotNull(capturedHistory);
            Assert.Equal("gift", capturedHistory.CauseId);
        }

        [Fact]
        public void CustomBandsCatalog_LoadsAndApplies()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var customCatalog = new RelationshipBandsCatalog
            {
                Bands = new List<RelationshipBandDefinition>
                {
                    new RelationshipBandDefinition
                    {
                        BandId = "custom_neutral",
                        DisplayName = "Custom Neutral",
                        MinAffinity = -100f,
                        MaxAffinity = 100f,
                        WorkingModifier = 0.05f,
                        ErrorRiskModifier = -0.02f,
                        NoteKey = "custom.band"
                    }
                }
            };

            relations.LoadBandsCatalog(customCatalog);
            relations.GetOrCreateRelationship("alpha", "beta").affinity = -50f;
            var effect = relations.GetRelationEffect("alpha", "beta");

            Assert.Equal("custom_neutral", effect.BandId);
            Assert.Equal(0.05f, effect.WorkingModifier, 2);
        }

        [Fact]
        public void PairHistoryIds_AreDeterministic_AcrossIdenticalRuns()
        {
            var a = new SurvivorRelationsSystem(new SeededRng(42));
            var b = new SurvivorRelationsSystem(new SeededRng(42));

            var entryA = a.RecordPairHistory("clara", "david", "gift", "gift_given", 10f);
            var entryB = b.RecordPairHistory("clara", "david", "gift", "gift_given", 10f);
            Assert.Equal(entryA.EventId, entryB.EventId);

            var secondA = a.RecordPairHistory("clara", "david", "meal", "shared_meal", 3f);
            Assert.NotEqual(entryA.EventId, secondA.EventId);
        }
    }
}
