// SPDX-License-Identifier: MIT
// Tests for Plan 148: Ideological Friction → Events & Quests System

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class Plan148IdeologicalFrictionIntegrationTests
    {
        private readonly string _catalogPath;

        public Plan148IdeologicalFrictionIntegrationTests()
        {
            _catalogPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/ideological_events.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/ideological_events.json");
            }
        }

        [Fact]
        public void Catalog_LoadsIdeologicalEventTemplatesSuccessfully()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file missing at {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);

            var system = new IdeologicalFrictionEvents();
            system.LoadCatalog(json);

            Assert.NotEmpty(system.Templates);
            Assert.Contains(system.Templates, t => t.event_type == "confrontation");
            Assert.Contains(system.Templates, t => t.event_type == "conversion");
            Assert.Contains(system.Templates, t => t.event_type == "quest");
        }

        [Fact]
        public void Friction_Confrontation_TriggersAtNegativeThreshold()
        {
            string json = File.ReadAllText(_catalogPath);
            var system = new IdeologicalFrictionEvents();
            system.LoadCatalog(json);

            IdeologicalEventInstance? triggered = null;
            system.OnIdeologicalEventTriggeredSeam = evt => triggered = evt;

            // Two conflicting beliefs: religious_faith vs atheist_rationalist with -60 affinity
            var rng = new SeededRng(10);
            var instance = system.CheckDailyFriction(
                survivorA: "survivor_pastor",
                beliefA: "religious_faith",
                survivorB: "survivor_engineer",
                beliefB: "atheist_rationalist",
                pairAffinity: -60f,
                currentDay: 5,
                rng: rng,
                forceTrigger: true);

            Assert.NotNull(instance);
            Assert.Equal(IdeologicalEventType.Confrontation, instance!.eventType);
            Assert.Equal("survivor_pastor", instance.actorId);
            Assert.Equal("survivor_engineer", instance.targetId);
            Assert.Same(instance, triggered);
            Assert.True(system.IsPairOnCooldown("survivor_pastor", "survivor_engineer", 6));
        }

        [Fact]
        public void Friction_Confrontation_MediationChoices_ApplyDeltas()
        {
            string json = File.ReadAllText(_catalogPath);
            var system = new IdeologicalFrictionEvents();
            system.LoadCatalog(json);

            var rng = new SeededRng(10);
            var instance = system.CheckDailyFriction(
                survivorA: "survivor_sentry",
                beliefA: "military_discipline",
                survivorB: "survivor_medic",
                beliefB: "pacifist",
                pairAffinity: -55f,
                currentDay: 2,
                rng: rng,
                forceTrigger: true);

            Assert.NotNull(instance);

            IdeologicalEventInstance? resolvedEvt = null;
            IdeologicalMediationChoice chosenChoice = IdeologicalMediationChoice.StayNeutral;
            system.OnIdeologicalMediationResolvedSeam = (evt, choice) =>
            {
                resolvedEvt = evt;
                chosenChoice = choice;
            };

            // Broker compromise
            bool success = system.ResolveConfrontation(
                instance!.instanceId,
                IdeologicalMediationChoice.BrokerCompromise,
                out float deltaA,
                out float deltaB,
                out float moraleDelta);

            Assert.True(success);
            Assert.Equal(10f, deltaA);
            Assert.Equal(10f, deltaB);
            Assert.Equal(5f, moraleDelta);
            Assert.True(instance.isResolved);
            Assert.Same(instance, resolvedEvt);
            Assert.Equal(IdeologicalMediationChoice.BrokerCompromise, chosenChoice);
        }

        [Fact]
        public void Friction_ConversionAttempt_CanSucceedWithPositiveAffinity()
        {
            var system = new IdeologicalFrictionEvents();
            var rng = new SeededRng(77);

            string convertedTarget = "";
            string adoptedBelief = "";
            system.OnBeliefConversionSucceededSeam = (target, oldB, newB) =>
            {
                convertedTarget = target;
                adoptedBelief = newB;
            };

            // High affinity conversion
            bool success = system.AttemptConversion(
                actorId: "survivor_elena",
                targetId: "survivor_marcus",
                actorBelief: "religious_faith",
                successProbability: 1.0f, // Guaranteed in unit test
                rng: rng,
                out string msg);

            Assert.True(success);
            Assert.Equal(1, system.TotalConversionsSucceeded);
            Assert.Equal("survivor_marcus", convertedTarget);
            Assert.Equal("religious_faith", adoptedBelief);
            Assert.Contains("survivor_marcus was deeply moved", msg);
        }

        [Fact]
        public void Friction_BunkerFactions_DetectsPolarizedSchism()
        {
            var system = new IdeologicalFrictionEvents();

            int escalationsDetected = 0;
            system.OnBunkerSplitEscalatedSeam = (belief, count) => escalationsDetected++;

            var survivorBeliefs = new Dictionary<string, string>
            {
                // 3 collectivists
                { "s1", "collectivist_solidarity" },
                { "s2", "collectivist_solidarity" },
                { "s3", "collectivist_solidarity" },
                // 3 individualists (conflicting with collectivist_solidarity)
                { "s4", "pragmatic_individualism" },
                { "s5", "pragmatic_individualism" },
                { "s6", "pragmatic_individualism" },
                // 1 neutral
                { "s7", "pacifist" }
            };

            var factions = system.UpdateBunkerFactions(survivorBeliefs);

            Assert.Equal(2, factions.Count);
            Assert.Contains(factions, f => f.beliefId == "collectivist_solidarity" && f.memberIds.Count == 3);
            Assert.Contains(factions, f => f.beliefId == "pragmatic_individualism" && f.memberIds.Count == 3);
            Assert.True(escalationsDetected >= 2);
        }

        [Fact]
        public void Friction_SaveState_RoundTripsAccurately()
        {
            string json = File.ReadAllText(_catalogPath);
            var system = new IdeologicalFrictionEvents();
            system.LoadCatalog(json);

            var rng = new SeededRng(10);
            system.CheckDailyFriction("s1", "religious_faith", "s2", "atheist_rationalist", -55f, 10, rng, forceTrigger: true);

            var state = system.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Equal(1, state.totalEventsFired);
            Assert.NotEmpty(state.pairCooldowns);

            var restored = new IdeologicalFrictionEvents();
            restored.RestoreState(state);

            Assert.Equal(1, restored.TotalEventsFired);
            Assert.True(restored.IsPairOnCooldown("s1", "s2", 12));
        }
    }
}
