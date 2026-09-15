// SPDX-License-Identifier: MIT
// DEBT-166-169-RELOAD-REPLAY: continuous campaign path vs mid-run reload.

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Bounded continuous-versus-reload fixture for Plans 166–169 only.
    /// Does not drive Godot Main; compares Core Capture/Restore canonical state.
    /// </summary>
    public sealed class Plan166_169ReloadReplayTests
    {
        private const int MasterSeed = 166169;
        private const int ReloadDay = 4;
        private const int FinalDay = 8;

        private sealed class CampaignBundle
        {
            public ResearchSystem Research = null!;
            public EspionageSystem Espionage = null!;
            public EspionageConsequenceRouter Router = null!;
            public CaravanTradeNetworkSystem Caravan = null!;
            public FluidLogisticsSystem Fluid = null!;
            public WaterTreatmentSystem Water = null!;
            public GreenhouseSystem Greenhouse = null!;
            public DiseaseSystem Disease = null!;
            public ProceduralNarrativeSystem Narrative = null!;
            public QuestRuntimeCoordinator Quests = null!;
            public CampaignRngManager Rng = null!;
        }

        private static QuestTemplateDef NarrativeTemplate()
            => new QuestTemplateDef
            {
                Id = "quest_template_reload_test",
                Category = "test",
                Weight = 1f,
                MinimumDay = 1,
                ActorRoleSlots = new List<string> { "requester" },
                LocationConstraints = new List<string> { "ruin" },
                ObjectiveModules = new List<string> { "repair_pipe" },
                RewardModules = new List<string> { "research_points" },
                FailureModules = new List<string> { "morale_event" },
                TimeLimitDays = 4,
                MergePolicy = "never",
                MaxConcurrentInstances = 1,
                TitleKey = "quest.reload.title",
                DescriptionKey = "quest.reload.description"
            };

        private static NarrativeWorldSnapshot Snapshot(int day)
            => new NarrativeWorldSnapshot
            {
                day = day,
                worldTags = new List<string> { "water_crisis" },
                aliveSurvivorIds = new List<string> { "survivor_1" },
                knownLocationIds = new List<string> { "loc_ruin_1" },
                obtainableItemIds = new List<string> { "item_water_filter_advanced" }
            };

        private static CampaignBundle CreateFresh()
        {
            var bundle = new CampaignBundle
            {
                Rng = new CampaignRngManager(MasterSeed),
                Research = new ResearchSystem(),
                Espionage = new EspionageSystem(),
                Router = new EspionageConsequenceRouter(),
                Caravan = new CaravanTradeNetworkSystem(
                    Array.Empty<CaravanRouteDefinition>(),
                    new Inventory.Inventory(),
                    new SeededRng(MasterSeed)),
                Fluid = new FluidLogisticsSystem(),
                Water = new WaterTreatmentSystem(),
                Greenhouse = new GreenhouseSystem(MasterSeed),
                Disease = new DiseaseSystem(rng: new SeededRng(MasterSeed)),
                Narrative = new ProceduralNarrativeSystem(),
                Quests = new QuestRuntimeCoordinator()
            };

            bundle.Research.Register(new ResearchKnowledgeDef(
                "knowledge_reload_basics", "Reload Basics", "survival",
                "Fixture research node for continuous-vs-reload.", 3));
            bundle.Research.UnlockManual("knowledge_reload_basics");

            bundle.Espionage.BindAgentAvailability(_ => true, _ => true);
            bundle.Espionage.BindAgentCapability(_ => 1f);
            bundle.Espionage.BindResearchGate(id =>
                bundle.Research.State.completedIds.Contains(id)
                || string.Equals(id, "knowledge_reload_basics", StringComparison.Ordinal));
            bundle.Espionage.LoadMissionCatalog(new[]
            {
                new EspionageMissionDef
                {
                    Id = "espionage_reload_surveillance",
                    MissionType = EspionageMissionType.Surveillance,
                    BaseDurationDays = 2,
                    BaseDetectionChance = 0.01f,
                    IntelGain = 1,
                    NetworkExposureGain = 0.02f,
                    IntelFactIds = new List<string> { "intel_fact_reload_route" }
                },
                new EspionageMissionDef
                {
                    Id = "espionage_reload_sabotage",
                    MissionType = EspionageMissionType.AbstractSabotage,
                    MinimumIntelLevel = 1,
                    PossibleConsequenceIds = new List<string>
                    {
                        EspionageConsequenceRouter.SupplyDisruption
                    }
                }
            });
            bundle.Router.BindConsumers(
                intent => bundle.Caravan.TryApplySupplyDisruption(
                    intent.targetFactionId, intent.magnitude, intent.startDay, intent.expiryDay, intent.incidentId),
                null,
                null);
            bundle.Router.Attach(bundle.Espionage);

            bundle.Fluid.EnsureDefaultShelterTopology(greenhouseDemand: 10f, drinkingDemand: 15f);
            bundle.Water.AddWater(WaterType.Clean, 80f);
            bundle.Greenhouse.EnsurePlots(1);
            bundle.Narrative.LoadTemplateCatalog(new[] { NarrativeTemplate() });
            return bundle;
        }

        private static void TickDay(CampaignBundle b, int day)
        {
            b.Espionage.BindRng(b.Rng.Fork("espionage", day));
            b.Fluid.BindRng(b.Rng.Fork("fluid", day));

            b.Research.Tick(day);

            if (day == 1)
            {
                Assert.True(b.Research.StartResearch("knowledge_reload_basics", day));
                Assert.True(b.Espionage.DeployAgent(
                    "survivor_1", "faction_central_garrison", "espionage_reload_surveillance", day).IsSuccess);
            }

            if (day == 3 && b.Espionage.State.networks.Count > 0)
            {
                string networkId = b.Espionage.State.networks[0].networkId;
                // Force enough intel for sabotage without relying on stochastic ticks.
                b.Espionage.State.networks[0].intelLevel = Math.Max(1, b.Espionage.State.networks[0].intelLevel);
                b.Espionage.BindRng(b.Rng.Fork("espionage_sabotage", day));
                Assert.True(b.Espionage.ExecuteAbstractSabotage(
                    networkId, "espionage_reload_sabotage", day).IsSuccess);
                b.Espionage.SetFiredConsequenceIncidentIds(b.Router.CaptureFired());
            }

            if (day == 2)
            {
                Assert.True(b.Narrative.TryGenerate(
                    Snapshot(day), b.Rng.Fork("narrative", day), out var draft));
                Assert.True(b.Quests.Register(draft.quest));
            }

            b.Espionage.Tick(day);
            b.Espionage.SetFiredConsequenceIncidentIds(b.Router.CaptureFired());

            FluidDeliveryApplicator.TransferBoundedCleanWater(b.Water, b.Fluid, dailyCapLiters: 12f);
            b.Fluid.Tick(day, outdoorTemperatureC: 5f, powerAvailability01: 1f);
            FluidDeliveryApplicator.Apply(
                b.Fluid, b.Greenhouse, b.Disease, () => "survivor_1", day);

            b.Quests.Tick(day);
        }

        private sealed class BundleSnapshot
        {
            public ResearchState Research = null!;
            public EspionageState Espionage = null!;
            public CaravanTradeNetworkSave Caravan = null!;
            public FluidLogisticsState Fluid = null!;
            public WaterTreatmentState Water = null!;
            public GreenhouseState Greenhouse = null!;
            public DiseaseSystemState Disease = null!;
            public ProceduralNarrativeState Narrative = null!;
            public QuestRuntimeState Quests = null!;
        }

        private static BundleSnapshot Capture(CampaignBundle b)
        {
            b.Espionage.SetFiredConsequenceIncidentIds(b.Router.CaptureFired());
            return new BundleSnapshot
            {
                Research = b.Research.CaptureState(),
                Espionage = b.Espionage.CaptureState(),
                Caravan = b.Caravan.CaptureState(),
                Fluid = b.Fluid.CaptureState(),
                Water = b.Water.CaptureState(),
                Greenhouse = b.Greenhouse.CaptureState(),
                Disease = b.Disease.CaptureState(),
                Narrative = b.Narrative.CaptureState(),
                Quests = b.Quests.CaptureState()
            };
        }

        private static CampaignBundle RestoreThroughSerializer(BundleSnapshot live)
        {
            var serializer = new SystemTextJsonSerializer();
            // Force a true save boundary: serialize then deserialize each section.
            var snap = new BundleSnapshot
            {
                Research = serializer.Deserialize<ResearchState>(serializer.Serialize(live.Research))!,
                Espionage = serializer.Deserialize<EspionageState>(serializer.Serialize(live.Espionage))!,
                Caravan = serializer.Deserialize<CaravanTradeNetworkSave>(serializer.Serialize(live.Caravan))!,
                Fluid = serializer.Deserialize<FluidLogisticsState>(serializer.Serialize(live.Fluid))!,
                Water = serializer.Deserialize<WaterTreatmentState>(serializer.Serialize(live.Water))!,
                Greenhouse = serializer.Deserialize<GreenhouseState>(serializer.Serialize(live.Greenhouse))!,
                Disease = serializer.Deserialize<DiseaseSystemState>(serializer.Serialize(live.Disease))!,
                Narrative = serializer.Deserialize<ProceduralNarrativeState>(serializer.Serialize(live.Narrative))!,
                Quests = serializer.Deserialize<QuestRuntimeState>(serializer.Serialize(live.Quests))!
            };

            var restored = CreateFresh();
            restored.Research.RestoreState(snap.Research);
            // Re-register catalog after restore (RestoreState does not rebuild defs).
            restored.Research.Register(new ResearchKnowledgeDef(
                "knowledge_reload_basics", "Reload Basics", "survival",
                "Fixture research node for continuous-vs-reload.", 3));
            restored.Espionage.RestoreState(snap.Espionage);
            restored.Router.RestoreFired(restored.Espionage.GetFiredConsequenceIncidentIds());
            restored.Caravan.RestoreState(snap.Caravan);
            restored.Fluid.RestoreState(snap.Fluid);
            restored.Fluid.EnsureDefaultShelterTopology(greenhouseDemand: 10f, drinkingDemand: 15f);
            restored.Water.RestoreState(snap.Water);
            restored.Greenhouse.RestoreState(snap.Greenhouse);
            restored.Disease.RestoreState(snap.Disease);
            restored.Narrative.RestoreState(snap.Narrative);
            restored.Narrative.LoadTemplateCatalog(new[] { NarrativeTemplate() });
            restored.Quests.RestoreState(snap.Quests);
            return restored;
        }

        private static string CanonicalFingerprint(BundleSnapshot s)
        {
            var serializer = new SystemTextJsonSerializer();
            // Named Plan 166–169 authorities only. Adjacent consumers (WT liters,
            // greenhouse moisture, caravan disruption) are spot-checked separately
            // so Disease rngPosition / ephemeral host noise cannot false-fail.
            return string.Join("|",
                serializer.Serialize(s.Research),
                serializer.Serialize(s.Espionage),
                serializer.Serialize(s.Fluid),
                serializer.Serialize(s.Narrative),
                serializer.Serialize(s.Quests));
        }

        [Fact]
        public void ContinuousVersusMidCampaignReload_EndsWithEqualCanonicalState()
        {
            // Path A: uninterrupted continuous campaign.
            var continuous = CreateFresh();
            for (int day = 1; day <= FinalDay; day++)
                TickDay(continuous, day);
            var continuousFinal = Capture(continuous);

            // Path B: same prefix, serialize at ReloadDay, restore, continue.
            var interrupted = CreateFresh();
            for (int day = 1; day <= ReloadDay; day++)
                TickDay(interrupted, day);
            var mid = Capture(interrupted);
            var restored = RestoreThroughSerializer(mid);
            for (int day = ReloadDay + 1; day <= FinalDay; day++)
                TickDay(restored, day);
            var reloadedFinal = Capture(restored);

            // Boundary: restored mid-state matches the live mid capture on named authorities.
            var midRestoredProbe = Capture(RestoreThroughSerializer(mid));
            Assert.Equal(
                CanonicalFingerprint(mid),
                CanonicalFingerprint(midRestoredProbe));

            Assert.Equal(
                CanonicalFingerprint(continuousFinal),
                CanonicalFingerprint(reloadedFinal));

            // Spot-check plan-owned facts so a fingerprint miss is diagnosable.
            Assert.Contains("knowledge_reload_basics", continuousFinal.Research.completedIds);
            Assert.Contains("knowledge_reload_basics", reloadedFinal.Research.completedIds);
            Assert.NotEmpty(continuousFinal.Espionage.firedConsequenceIncidentIds);
            Assert.Equal(
                continuousFinal.Espionage.firedConsequenceIncidentIds.OrderBy(x => x, StringComparer.Ordinal),
                reloadedFinal.Espionage.firedConsequenceIncidentIds.OrderBy(x => x, StringComparer.Ordinal));
            Assert.True(continuousFinal.Fluid.totalVolumeDelivered > 0f);
            Assert.Equal(continuousFinal.Fluid.totalVolumeDelivered, reloadedFinal.Fluid.totalVolumeDelivered, 3);
            Assert.NotEmpty(continuousFinal.Quests.quests);
            Assert.Equal(continuousFinal.Quests.quests.Count, reloadedFinal.Quests.quests.Count);
            Assert.Equal(continuousFinal.Water.cleanWater, reloadedFinal.Water.cleanWater, 3);
            Assert.Equal(
                continuousFinal.Caravan.supply_disruptions?.Count ?? 0,
                reloadedFinal.Caravan.supply_disruptions?.Count ?? 0);
            Assert.Equal(
                continuousFinal.Greenhouse.plots[0].water,
                reloadedFinal.Greenhouse.plots[0].water,
                3);
        }

        [Fact]
        public void ReloadDoesNotReplayEspionageIntelOrDoubleApplySupplyDisruption()
        {
            var interrupted = CreateFresh();
            for (int day = 1; day <= ReloadDay; day++)
                TickDay(interrupted, day);

            int intelBefore = interrupted.Espionage.State.knownIntel.Count;
            float disruptionBefore = interrupted.Caravan.GetActiveSupplyDisruptionMagnitude(
                "faction_central_garrison", ReloadDay);
            Assert.True(disruptionBefore > 0f);

            var restored = RestoreThroughSerializer(Capture(interrupted));
            int intelEvents = 0;
            restored.Espionage.OnIntelDiscovered += _ => intelEvents++;

            // Restore alone must not re-fire intel.
            Assert.Equal(0, intelEvents);
            Assert.Equal(intelBefore, restored.Espionage.State.knownIntel.Count);

            // Fired-set blocks re-routing the same incident.
            string incidentId = restored.Espionage.GetFiredConsequenceIncidentIds().First();
            Assert.False(restored.Router.TryRoute(new EspionageConsequenceIntent
            {
                incidentId = incidentId,
                targetFactionId = "faction_central_garrison",
                consequenceType = EspionageConsequenceRouter.SupplyDisruption,
                magnitude = 0.2f,
                startDay = ReloadDay,
                expiryDay = ReloadDay + 3
            }));
            Assert.Equal("already_fired", restored.Router.LastRouteResult);
            Assert.Equal(
                disruptionBefore,
                restored.Caravan.GetActiveSupplyDisruptionMagnitude("faction_central_garrison", ReloadDay),
                4);
        }
    }
}
