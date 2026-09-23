// SPDX-License-Identifier: MIT
// Tests for Plan 144: Survivor Autonomy & Initiative System

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class Plan144SurvivorAutonomyIntegrationTests
    {
        private readonly string _catalogPath;

        public Plan144SurvivorAutonomyIntegrationTests()
        {
            _catalogPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/autonomy_actions.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/autonomy_actions.json");
            }
        }

        private sealed class TestAutonomySink : IAutonomySink
        {
            public AutonomyAction? LastRecordedAction { get; private set; }
            public int RecordedCount { get; private set; }

            public void RecordAutonomyAction(AutonomyAction action)
            {
                LastRecordedAction = action;
                RecordedCount++;
            }
        }

        [Fact]
        public void Catalog_Loads20AutonomyTemplatesSuccessfully()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file missing at {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);

            var system = new SurvivorAutonomySystem(new SeededRng(144));
            system.LoadCatalog(json);

            Assert.Equal(20, system.Templates.Count);
            Assert.Contains(system.Templates, t => t.action_type == "help");
            Assert.Contains(system.Templates, t => t.action_type == "refuse");
            Assert.Contains(system.Templates, t => t.action_type == "initiate");
            Assert.Contains(system.Templates, t => t.action_type == "express");
            Assert.Contains(system.Templates, t => t.action_type == "pursue");
        }

        [Fact]
        public void Autonomy_HelpAction_ComfortsCompanion_TriggersSeamsAndSink()
        {
            string json = File.ReadAllText(_catalogPath);
            var rng = new SeededRng(42);
            var system = new SurvivorAutonomySystem(rng, cooldownDays: 2);
            system.LoadCatalog(json);

            var sink = new TestAutonomySink();
            system.AutonomySink = sink;

            string helpedActor = "";
            string helpedTarget = "";
            int helpedAffinity = 0;
            system.OnSurvivorHelpedSeam = (actor, target, aff) =>
            {
                helpedActor = actor;
                helpedTarget = target;
                helpedAffinity = aff;
            };

            // Survivor with "social" trait, high pair affinity
            AutonomyAction? triggered = null;
            for (int day = 1; day <= 20; day++)
            {
                var action = system.EvaluateDailyAutonomy(
                    actorId: "survivor_nadia",
                    currentDay: day,
                    morale: 60f,
                    fatigue: 10f,
                    traits: new[] { "social" },
                    targetId: "survivor_vask",
                    pairAffinity: 30);

                if (action != null && action.actionType == AutonomyActionType.Help)
                {
                    triggered = action;
                    break;
                }
            }

            Assert.NotNull(triggered);
            Assert.Equal(AutonomyActionType.Help, triggered!.actionType);
            Assert.Equal("survivor_nadia", triggered.actorId);
            Assert.Equal("survivor_vask", triggered.targetId);
            Assert.True(triggered.affinityDelta > 0);
            Assert.Equal(1, sink.RecordedCount);
            Assert.Equal("survivor_nadia", helpedActor);
            Assert.Equal("survivor_vask", helpedTarget);
            Assert.True(helpedAffinity > 0);
        }

        [Fact]
        public void Autonomy_RefuseAction_PlayerOverride_EnforcesPenalty()
        {
            string json = File.ReadAllText(_catalogPath);
            var rng = new SeededRng(77);
            var system = new SurvivorAutonomySystem(rng, cooldownDays: 1);
            system.LoadCatalog(json);

            string overriddenActor = "";
            string overriddenTitle = "";
            system.OnWorkRefusalOverriddenSeam = (actor, title) =>
            {
                overriddenActor = actor;
                overriddenTitle = title;
            };

            // Exhausted, low morale, stubborn survivor
            AutonomyAction? refuseAction = null;
            for (int day = 1; day <= 30; day++)
            {
                var action = system.EvaluateDailyAutonomy(
                    actorId: "survivor_marcus",
                    currentDay: day,
                    morale: 15f,
                    fatigue: 80f,
                    traits: new[] { "stubborn", "anxious" });

                if (action != null && action.actionType == AutonomyActionType.Refuse)
                {
                    refuseAction = action;
                    break;
                }
            }

            Assert.NotNull(refuseAction);
            Assert.Equal(AutonomyActionType.Refuse, refuseAction!.actionType);
            Assert.Equal(AutonomyOutcome.Accepted, refuseAction.outcome);

            // Player overrides refusal to enforce duty
            bool overrideSuccess = system.OverrideRefusal(refuseAction.actionId, enforceWork: true);
            Assert.True(overrideSuccess);
            Assert.Equal(AutonomyOutcome.Overridden, refuseAction.outcome);
            Assert.Equal(1, system.TotalOverridesEnforced);
            Assert.Equal("survivor_marcus", overriddenActor);
            Assert.False(string.IsNullOrEmpty(overriddenTitle));
        }

        [Fact]
        public void Autonomy_Cooldown_EnforcesActionSpacing()
        {
            string json = File.ReadAllText(_catalogPath);
            var rng = new SeededRng(100);
            var system = new SurvivorAutonomySystem(rng, cooldownDays: 3);
            system.LoadCatalog(json);

            // Find an action on day 1
            AutonomyAction? firstAction = null;
            while (firstAction == null)
            {
                firstAction = system.EvaluateDailyAutonomy(
                    actorId: "survivor_elena",
                    currentDay: 5,
                    morale: 70f,
                    fatigue: 5f,
                    traits: new[] { "social", "ambitious" },
                    targetId: "survivor_kane",
                    pairAffinity: 25);
            }

            Assert.NotNull(firstAction);
            Assert.True(system.IsOnCooldown("survivor_elena", 6)); // Day 6 is within 3 days of day 5
            Assert.True(system.IsOnCooldown("survivor_elena", 7)); // Day 7 is within 3 days of day 5

            var blockedAction = system.EvaluateDailyAutonomy(
                actorId: "survivor_elena",
                currentDay: 6,
                morale: 70f,
                fatigue: 5f,
                traits: new[] { "social", "ambitious" });
            Assert.Null(blockedAction);

            // On Day 8 (5 + 3 = 8), cooldown is finished
            Assert.False(system.IsOnCooldown("survivor_elena", 8));
        }

        [Fact]
        public void Autonomy_PursueAction_TracksAndCompletesGoal()
        {
            var system = new SurvivorAutonomySystem(new SeededRng(144));
            system.AssignGoal("survivor_alec", "goal_welding", "Master Advanced TIG Welding", targetProgress: 3);

            var goal = system.GetGoal("survivor_alec");
            Assert.NotNull(goal);
            Assert.Equal(0, goal!.currentProgress);
            Assert.False(goal.isCompleted);

            system.AdvanceGoal("survivor_alec", 1);
            Assert.Equal(1, goal.currentProgress);
            Assert.False(goal.isCompleted);

            system.AdvanceGoal("survivor_alec", 2);
            Assert.Equal(3, goal.currentProgress);
            Assert.True(goal.isCompleted);
        }

        [Fact]
        public void Autonomy_SaveState_RoundTripsAccurately()
        {
            string json = File.ReadAllText(_catalogPath);
            var system = new SurvivorAutonomySystem(new SeededRng(99), cooldownDays: 2);
            system.LoadCatalog(json);

            // Execute an action
            system.EvaluateDailyAutonomy(
                actorId: "survivor_tomas",
                currentDay: 12,
                morale: 80f,
                fatigue: 0f,
                traits: new[] { "independent" });

            system.AssignGoal("survivor_tomas", "goal_foraging", "Map Berry Patches", targetProgress: 2);
            system.AdvanceGoal("survivor_tomas", 1);

            var state = system.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.True(state.totalActionsTriggered >= 0);
            Assert.True(state.survivorGoals.ContainsKey("survivor_tomas"));

            var restoredSystem = new SurvivorAutonomySystem(new SeededRng(99));
            restoredSystem.RestoreState(state);

            Assert.Equal(state.totalActionsTriggered, restoredSystem.TotalActionsTriggered);
            var restoredGoal = restoredSystem.GetGoal("survivor_tomas");
            Assert.NotNull(restoredGoal);
            Assert.Equal(1, restoredGoal!.currentProgress);
            Assert.Equal(2, restoredGoal.targetProgress);
            Assert.False(restoredGoal.isCompleted);
        }
    }
}
