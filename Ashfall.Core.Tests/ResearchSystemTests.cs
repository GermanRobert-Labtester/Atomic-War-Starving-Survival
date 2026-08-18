using System;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ResearchSystemTests
    {
        private static ResearchSystem BuildEngine(ResearchState state = null)
        {
            var log = new NullLog();
            var engine = new ResearchSystem(log, state);
            engine.RegisterDefaults();
            return engine;
        }

        [Fact]
        public void RegisterDefaults_Builds15NodeCatalog()
        {
            var engine = BuildEngine();
            Assert.Equal(15, engine.CatalogCount);
        }

        [Fact]
        public void StartResearch_SetsActiveId()
        {
            var engine = BuildEngine();
            Assert.True(engine.StartResearch("knowledge_water_basics", day: 5));
            var active = engine.GetActiveResearch();
            Assert.NotNull(active);
            Assert.Equal("knowledge_water_basics", active.id);
        }

        [Fact]
        public void Tick_CompletesNodeAfterDaysBudget()
        {
            var engine = BuildEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // 6 days elapsed, water_basics needs 5
            Assert.Null(engine.GetActiveResearch()); // completed
            var def = engine.GetKnowledge("knowledge_water_basics");
            Assert.True(def.isCompleted);
            Assert.Contains("knowledge_water_basics", engine.State.completedIds);
        }

        [Fact]
        public void StartResearch_PrerequisiteGated_Rejects()
        {
            var engine = BuildEngine();
            // water_advanced requires water_basics completed, but basics is not completed yet.
            Assert.False(engine.StartResearch("knowledge_water_advanced", day: 1));
        }

        [Fact]
        public void StartResearch_PrerequisiteGated_AcceptsAfterPrereqCompleted()
        {
            var engine = BuildEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // completes water_basics
            Assert.True(engine.StartResearch("knowledge_water_advanced", day: 7));
            Assert.Equal("knowledge_water_advanced", engine.State.activeResearchId);
        }

        [Fact]
        public void StartResearch_AlreadyCompleted_Rejected()
        {
            var engine = BuildEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // completed
            Assert.False(engine.StartResearch("knowledge_water_basics", day: 8));
        }

        [Fact]
        public void CaptureState_RoundTrip_PreservesState()
        {
            var engine = BuildEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // completed
            engine.StartResearch("knowledge_radiation_basics", day: 7);
            engine.Tick(newDay: 10); // 3 days in, 2 remaining

            var saved = engine.CaptureState();
            Assert.Contains("knowledge_water_basics", saved.completedIds);
            Assert.Equal("knowledge_radiation_basics", saved.activeResearchId);

            // Round-trip into a fresh engine.
            var engine2 = BuildEngine(saved);
            Assert.True(engine2.GetKnowledge("knowledge_water_basics").isCompleted);
            Assert.Equal("knowledge_radiation_basics", engine2.State.activeResearchId);
            Assert.Equal(3, engine2.State.activeResearchDays);
        }

        [Fact]
        public void Tick_IsDeterministicUnderSameSeed()
        {
            var engineA = BuildEngine();
            engineA.StartResearch("knowledge_water_basics", day: 1);
            engineA.Tick(newDay: 20);

            var engineB = BuildEngine();
            engineB.StartResearch("knowledge_water_basics", day: 1);
            engineB.Tick(newDay: 20);

            Assert.Equal(engineA.State.completedIds.Count, engineB.State.completedIds.Count);
            Assert.Equal(engineA.State.activeResearchId, engineB.State.activeResearchId);
        }
    }
}
