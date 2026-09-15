// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Radio;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan167EspionageConsequenceRoutingTests
    {
        private sealed class FixedRng : ISeededRng
        {
            private readonly Queue<double> _values;
            public FixedRng(params double[] values) => _values = new Queue<double>(values);
            public int Seed => 167;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)NextDouble();
            public double NextDouble() => _values.Count > 0 ? _values.Dequeue() : 0.01;
        }

        private static CaravanTradeNetworkSystem CreateCaravan()
            => new CaravanTradeNetworkSystem(
                Array.Empty<CaravanRouteDefinition>(),
                new Inventory.Inventory(),
                new SeededRng(167));

        private static EspionageSystem CreateEspionage(ISeededRng rng, params string[] consequenceIds)
        {
            var system = new EspionageSystem();
            system.BindRng(rng);
            system.BindAgentAvailability(_ => true, _ => true);
            system.BindAgentCapability(_ => 1f);
            system.BindResearchGate(_ => true);
            system.LoadMissionCatalog(new[]
            {
                new EspionageMissionDef
                {
                    Id = "espionage_test_sabotage",
                    MissionType = EspionageMissionType.AbstractSabotage,
                    BaseDurationDays = 1,
                    BaseDetectionChance = 0.05f,
                    MinimumIntelLevel = 1,
                    PossibleConsequenceIds = consequenceIds?.ToList() ?? new List<string>()
                }
            });
            return system;
        }

        private static void SeedNetwork(EspionageSystem system, string networkId = "network_1")
        {
            system.State.networks.Add(new SpyNetworkState
            {
                networkId = networkId,
                infiltratorId = "survivor_1",
                targetFactionId = "faction_central_garrison",
                intelLevel = 3,
                status = EspionageMissionStatus.Operating
            });
        }

        [Fact]
        public void SupplyDisruptionRoutesOnceToCaravanAndSurvivesRestore()
        {
            var espionage = CreateEspionage(new FixedRng(0.01, 0.0), EspionageConsequenceRouter.SupplyDisruption);
            SeedNetwork(espionage);
            var caravan = CreateCaravan();
            var router = new EspionageConsequenceRouter();
            router.BindConsumers(
                intent => caravan.TryApplySupplyDisruption(
                    intent.targetFactionId, intent.magnitude, intent.startDay, intent.expiryDay, intent.incidentId),
                null,
                null);
            router.Attach(espionage);

            Assert.True(espionage.ExecuteAbstractSabotage("network_1", "espionage_test_sabotage", 10).IsSuccess);
            Assert.Equal("supply_applied", router.LastRouteResult);
            Assert.True(caravan.GetActiveSupplyDisruptionMagnitude("faction_central_garrison", 10) > 0f);

            // Second route of same incident is blocked by fired-set.
            var replay = new EspionageConsequenceIntent
            {
                incidentId = router.CaptureFired()[0],
                targetFactionId = "faction_central_garrison",
                consequenceType = EspionageConsequenceRouter.SupplyDisruption,
                magnitude = 0.2f,
                startDay = 10,
                expiryDay = 13
            };
            Assert.False(router.TryRoute(replay));
            Assert.Equal("already_fired", router.LastRouteResult);

            espionage.SetFiredConsequenceIncidentIds(router.CaptureFired());
            var saved = espionage.CaptureState();
            var caravanSaved = caravan.CaptureState();

            var restoredEspionage = new EspionageSystem();
            restoredEspionage.RestoreState(saved);
            var restoredRouter = new EspionageConsequenceRouter();
            restoredRouter.RestoreFired(restoredEspionage.GetFiredConsequenceIncidentIds());
            var restoredCaravan = CreateCaravan();
            restoredCaravan.RestoreState(caravanSaved);

            Assert.Contains(replay.incidentId, restoredRouter.FiredIncidentIds);
            Assert.True(restoredCaravan.GetActiveSupplyDisruptionMagnitude("faction_central_garrison", 10) > 0f);
            Assert.False(restoredRouter.TryRoute(replay));
        }

        [Fact]
        public void CommunicationsDisruptionStartsPsyOpsJamming()
        {
            var espionage = CreateEspionage(new FixedRng(0.01, 0.0), EspionageConsequenceRouter.CommunicationsDisruption);
            SeedNetwork(espionage);
            var psyops = new PsyOpsSystem(new PsyOpsCatalogContainer());
            var router = new EspionageConsequenceRouter();
            router.BindConsumers(
                null,
                intent =>
                {
                    int days = Math.Max(1, intent.expiryDay - intent.startDay);
                    return psyops.StartJamming(intent.targetFactionId, intent.magnitude, days, intent.startDay);
                },
                null);
            router.Attach(espionage);

            Assert.True(espionage.ExecuteAbstractSabotage("network_1", "espionage_test_sabotage", 5).IsSuccess);
            Assert.Equal("comms_applied", router.LastRouteResult);
            Assert.Contains(psyops.State.jamming, j =>
                j != null && j.targetFactionId == "faction_central_garrison" && j.daysRemaining >= 1);
        }

        [Fact]
        public void DefenseReadinessRoutesToFactionWarPressure()
        {
            var espionage = CreateEspionage(new FixedRng(0.01, 0.0), EspionageConsequenceRouter.DefenseReadinessReduced);
            SeedNetwork(espionage);
            var war = new FactionWarSystem();
            var router = new EspionageConsequenceRouter();
            router.BindConsumers(
                null,
                null,
                intent => war.TryApplyDefenseReadinessPressure(
                    intent.targetFactionId, intent.magnitude, intent.startDay, intent.expiryDay, intent.incidentId));
            router.Attach(espionage);

            Assert.True(espionage.ExecuteAbstractSabotage("network_1", "espionage_test_sabotage", 20).IsSuccess);
            Assert.Equal("defense_applied", router.LastRouteResult);
            Assert.True(war.GetDefenseReadiness01("faction_central_garrison", 20) < 1f);
        }

        [Fact]
        public void UnboundConsumerDoesNotConsumeFiredSlot()
        {
            var router = new EspionageConsequenceRouter();
            // No consumers bound.
            var intent = new EspionageConsequenceIntent
            {
                incidentId = "espionage_incident_orphan",
                targetFactionId = "faction_central_garrison",
                consequenceType = EspionageConsequenceRouter.SupplyDisruption,
                magnitude = 0.2f,
                startDay = 1,
                expiryDay = 4
            };

            Assert.False(router.TryRoute(intent));
            Assert.Equal("supply_consumer_missing", router.LastRouteResult);
            Assert.Empty(router.FiredIncidentIds);

            var caravan = CreateCaravan();
            router.BindConsumers(
                i => caravan.TryApplySupplyDisruption(
                    i.targetFactionId, i.magnitude, i.startDay, i.expiryDay, i.incidentId),
                null,
                null);
            Assert.True(router.TryRoute(intent));
            Assert.Equal("supply_applied", router.LastRouteResult);
        }
    }
}
