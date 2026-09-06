// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Research
{
    public class ResearchQueueEligibilityTests
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

        private static ResearchSystem BuildCatalogEngine(ResearchState? state = null)
        {
            var log = new NullLog();
            var engine = new ResearchSystem(log, state);
            ResearchKnowledgeCatalogLoader.LoadAndRegister(
                engine, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            return engine;
        }

        [Fact]
        public void EmptyCatalog_ReturnsUnknownNode()
        {
            var engine = new ResearchSystem();
            var elig = engine.GetEligibility("knowledge_water_basics");
            Assert.False(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.UnknownNode, elig.Code);
        }

        [Fact]
        public void UnknownNode_ReturnsUnknownCode()
        {
            var engine = BuildCatalogEngine();
            var elig = engine.GetEligibility("non_existent_node_xyz");
            Assert.False(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.UnknownNode, elig.Code);
        }

        [Fact]
        public void MissingPrerequisites_ReportsLockedAndListsPrereqs()
        {
            var engine = BuildCatalogEngine();
            // knowledge_water_advanced requires knowledge_water_basics
            var elig = engine.GetEligibility("knowledge_water_advanced");
            Assert.False(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.MissingPrerequisites, elig.Code);
            Assert.Contains("knowledge_water_basics", elig.MissingPrerequisites);
        }

        [Fact]
        public void RootNode_WithoutPrerequisites_IsEligible()
        {
            var engine = BuildCatalogEngine();
            var elig = engine.GetEligibility("knowledge_water_basics");
            Assert.True(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.Eligible, elig.Code);
            Assert.Empty(elig.MissingPrerequisites);
        }

        [Fact]
        public void AlreadyActive_ReturnsAlreadyActiveCode()
        {
            var engine = BuildCatalogEngine();
            Assert.True(engine.StartResearch("knowledge_water_basics", day: 1));

            var elig = engine.GetEligibility("knowledge_water_basics");
            Assert.False(elig.CanStart);
            Assert.True(elig.IsActive);
            Assert.Equal(ResearchEligibilityCode.AlreadyActive, elig.Code);
        }

        [Fact]
        public void AnotherResearchActive_BlocksOtherNodes()
        {
            var engine = BuildCatalogEngine();
            Assert.True(engine.StartResearch("knowledge_water_basics", day: 1));

            // knowledge_hydroponics is another root node with met prerequisites
            var elig = engine.GetEligibility("knowledge_hydroponics");
            Assert.False(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.AnotherResearchActive, elig.Code);
            Assert.Equal("knowledge_water_basics", elig.ConflictingActiveResearchId);

            // Calling StartResearch while another is active must be rejected
            Assert.False(engine.StartResearch("knowledge_hydroponics", day: 1));
        }

        [Fact]
        public void AlreadyCompleted_ReturnsAlreadyCompletedCode()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // 5 days needed, completes

            var elig = engine.GetEligibility("knowledge_water_basics");
            Assert.False(elig.CanStart);
            Assert.True(elig.IsCompleted);
            Assert.Equal(ResearchEligibilityCode.AlreadyCompleted, elig.Code);

            // Starting already completed node fails
            Assert.False(engine.StartResearch("knowledge_water_basics", day: 8));
        }

        [Fact]
        public void PrerequisiteSatisfied_UnlocksDependentNode()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", day: 1);
            engine.Tick(newDay: 7); // completes water_basics

            var elig = engine.GetEligibility("knowledge_water_advanced");
            Assert.True(elig.CanStart);
            Assert.Equal(ResearchEligibilityCode.Eligible, elig.Code);
            Assert.Empty(elig.MissingPrerequisites);

            Assert.True(engine.StartResearch("knowledge_water_advanced", day: 7));
            Assert.Equal("knowledge_water_advanced", engine.State.activeResearchId);
        }

        [Fact]
        public void SameDay_RepeatedTick_DoesNotAdvanceProgress()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", day: 5);
            Assert.Equal(0, engine.State.activeResearchDays);

            // Repeated ticks on the exact same day
            engine.Tick(newDay: 5);
            Assert.Equal(0, engine.State.activeResearchDays);
            engine.Tick(newDay: 5);
            Assert.Equal(0, engine.State.activeResearchDays);

            // Tick back in time does not advance
            engine.Tick(newDay: 4);
            Assert.Equal(0, engine.State.activeResearchDays);
        }

        [Fact]
        public void SkippedDayTick_DerivesProgressFromDayDelta()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_water_basics", day: 2); // 5 days needed
            Assert.Equal(5, engine.GetDaysRemaining("knowledge_water_basics"));

            // Advance 3 days at once (from day 2 to day 5)
            engine.Tick(newDay: 5);
            Assert.Equal(3, engine.State.activeResearchDays);
            Assert.Equal(2, engine.GetDaysRemaining("knowledge_water_basics"));
            Assert.Equal("knowledge_water_basics", engine.State.activeResearchId);

            // Advance remaining 2 days at once (from day 5 to day 7)
            engine.Tick(newDay: 7);
            Assert.Equal(string.Empty, engine.State.activeResearchId);
            Assert.Contains("knowledge_water_basics", engine.State.completedIds);
        }

        [Fact]
        public void SaveRestore_MidProgress_PreservesExactDaysRemaining()
        {
            var engine = BuildCatalogEngine();
            engine.StartResearch("knowledge_radiation_basics", day: 10); // 5 days needed
            engine.Tick(newDay: 13); // 3 days in, 2 remaining

            Assert.Equal(2, engine.GetDaysRemaining("knowledge_radiation_basics"));
            var saved = engine.CaptureState();

            var restored = BuildCatalogEngine(saved);
            Assert.Equal("knowledge_radiation_basics", restored.State.activeResearchId);
            Assert.Equal(3, restored.State.activeResearchDays);
            Assert.Equal(2, restored.GetDaysRemaining("knowledge_radiation_basics"));

            // Restored engine finishes on day 15
            restored.Tick(newDay: 15);
            Assert.Contains("knowledge_radiation_basics", restored.State.completedIds);
            Assert.Equal(string.Empty, restored.State.activeResearchId);
        }

        [Fact]
        public void GetDependents_HandlesDiamondGraphAndEmpty()
        {
            var engine = new ResearchSystem();
            // Build diamond: Root -> Left, Root -> Right; Left -> Peak, Right -> Peak
            engine.Register(new ResearchKnowledgeDef("root", "Root", "general", "Root desc", 5));
            engine.Register(new ResearchKnowledgeDef("left", "Left", "general", "Left desc", 5, new[] { "root" }));
            engine.Register(new ResearchKnowledgeDef("right", "Right", "general", "Right desc", 5, new[] { "root" }));
            engine.Register(new ResearchKnowledgeDef("peak", "Peak", "general", "Peak desc", 5, new[] { "left", "right" }));

            var rootDependents = engine.GetDependents("root");
            // Downstream: left, right, peak (diamond must contain peak only once)
            Assert.Equal(3, rootDependents.Count);
            Assert.Equal(new[] { "left", "peak", "right" }, rootDependents);

            // Peak has no dependents
            var peakDependents = engine.GetDependents("peak");
            Assert.Empty(peakDependents);

            // Unknown node has no dependents
            var unknownDependents = engine.GetDependents("non_existent");
            Assert.Empty(unknownDependents);
        }

        [Fact]
        public void Projections_AvailableLockedCompleted_ArePartitionedAndSorted()
        {
            var engine = BuildCatalogEngine();
            var available = engine.GetAvailableNodes();
            var locked = engine.GetLockedNodes();
            var completed = engine.GetCompletedNodes();

            Assert.NotEmpty(available);
            Assert.NotEmpty(locked);
            Assert.Empty(completed);

            // All available nodes must have met prerequisites
            foreach (var node in available)
            {
                var elig = engine.GetEligibility(node.id);
                Assert.Empty(elig.MissingPrerequisites);
            }

            // All locked nodes must have missing prerequisites
            foreach (var node in locked)
            {
                var elig = engine.GetEligibility(node.id);
                Assert.NotEmpty(elig.MissingPrerequisites);
            }

            // Total non-active should match catalog
            Assert.Equal(engine.CatalogCount, available.Count + locked.Count + completed.Count);
        }
    }
}
