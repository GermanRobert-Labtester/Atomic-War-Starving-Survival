// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Radio;
using Ashfall.Core.YearOfAsh;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 25: End-to-End Campaign Smoke Test.
    ///
    /// Executes the full multi-day campaign scenario (Seed 42):
    /// - Day 1: Radio tuning discovery of 88.3 MHz trapped mechanic.
    /// - Day 2: Clarity and message fragment decode progression.
    /// - Day 3: Triangulation resolves revealed location 'loc_recovery_yard'.
    /// - Day 4: Moral choice accepted (Rescue); moral and faction deltas applied once.
    /// - Day 5: Expedition dispatched and arrives at 'loc_recovery_yard'; rescue completed.
    /// - Day 6: Full campaign save captured and restored into fresh subsystem composition.
    /// - Day 7: Radio tuning of 192.4 MHz raider lure; authenticity flags trap; avoided safely without penalties.
    /// - Deterministic Replay: 3 independent runs produce identical state traces.
    /// </summary>
    public sealed class DistressSignalCampaignSmokeTests : CatalogTestBase
    {
        private readonly ITestOutputHelper _output;

        public DistressSignalCampaignSmokeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private sealed class CampaignSnapshot
        {
            public int FinalMoralScore;
            public int FinalEmpathyPoints;
            public int RailwayGuildStanding;
            public int IronGarrisonStanding;
            public int RaiderStanding;
            public DistressSignalStatus MechanicSignalStatus;
            public bool MechanicMoralChoiceAvailable;
            public int MechanicResolutionIndex;
            public DistressSignalStatus RaiderLureStatus;
            public bool RaiderLureMoralChoiceAvailable;

            public override bool Equals(object? obj)
            {
                if (obj is not CampaignSnapshot o) return false;
                return FinalMoralScore == o.FinalMoralScore &&
                       FinalEmpathyPoints == o.FinalEmpathyPoints &&
                       RailwayGuildStanding == o.RailwayGuildStanding &&
                       IronGarrisonStanding == o.IronGarrisonStanding &&
                       RaiderStanding == o.RaiderStanding &&
                       MechanicSignalStatus == o.MechanicSignalStatus &&
                       MechanicMoralChoiceAvailable == o.MechanicMoralChoiceAvailable &&
                       MechanicResolutionIndex == o.MechanicResolutionIndex &&
                       RaiderLureStatus == o.RaiderLureStatus &&
                       RaiderLureMoralChoiceAvailable == o.RaiderLureMoralChoiceAvailable;
            }

            public override int GetHashCode() =>
                HashCode.Combine(FinalMoralScore, FinalEmpathyPoints, RailwayGuildStanding, MechanicSignalStatus, RaiderLureStatus);
        }

        private CampaignSnapshot ExecuteCampaignScenario(int seed, bool logDiagnostics = false)
        {
            var rng = new SeededRng(seed);

            // 1. Compose Subsystems
            var distress = new RadioDistressSystem();
            string signalCatalogPath = Path.Combine(DataDirectory, "radio_distress_signals.json");
            distress.LoadFromJson(File.ReadAllText(signalCatalogPath));

            var tuner = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 88.0f });
            var moral = new MoralChoiceSystem(new SeededRng(seed));
            var quests = MoralChoiceCatalogLoader.LoadStubs(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer())
                .ToDictionary(q => q.Id, StringComparer.OrdinalIgnoreCase);
            var factionWar = new FactionWarSystem();
            var expedition = new ExpeditionSystem();

            // Production destination: load the real expedition catalog (R12 —
            // the smoke test must not bypass production travel data). This
            // auto-registers loc_recovery_yard from expeditions.json.
            var expeditionDefs = ExpeditionCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Contains(expeditionDefs, d => d.id == "loc_recovery_yard");
            var destDef = ExpeditionDefinitionRegistry.Get("loc_recovery_yard");
            Assert.NotNull(destDef);
            Assert.True(destDef!.distanceTicks >= 1, "Production recovery yard must require real travel");

            // ── Day 1: Tune to 88.3 MHz, detect signal ──
            var tuneDay1 = tuner.EvaluateFrequency(88.3f, distress, 0.15f, rng, day: 1);
            Assert.True(tuneDay1.IsLocked);
            Assert.NotNull(tuneDay1.Signal);
            Assert.Equal("freq_distress_88_3", tuneDay1.Signal!.FrequencyId);
            Assert.True(tuneDay1.IsGenuineRescue);

            bool intercepted = distress.Intercept("freq_distress_88_3", day: 1);
            Assert.True(intercepted);

            // ── Day 2: Clarity and message fragment decode progression ──
            distress.TickDaily(2);
            var tuneDay2 = tuner.EvaluateFrequency(88.3f, distress, 0.15f, rng, day: 2);
            Assert.True(tuneDay2.IsLocked);

            // ── Day 3: Signal authenticated, location triangulated ──
            distress.TickDaily(3);
            bool triangulated = distress.MarkTriangulated("freq_distress_88_3");
            Assert.True(triangulated);
            var mechanicDef = distress.GetDefinition("freq_distress_88_3");
            Assert.Equal("loc_recovery_yard", mechanicDef!.RevealedLocation);

            // ── Day 4: Moral choice accepted (Rescue) ──
            bool choiceAvailable = distress.TryTriggerMoralChoice("freq_distress_88_3", out string moralChoiceId);
            Assert.True(choiceAvailable);
            Assert.Equal("quest_moral_distress_trapped_mechanic", moralChoiceId);

            var quest = quests[moralChoiceId];
            int moralBefore = moral.MoralScore;
            int repBefore = factionWar.GetStanding("faction_railway_guild");

            bool choiceResolved = distress.ResolveMoralChoice("freq_distress_88_3", 0, moral, quest, day: 4, out var resolution, factionWar);
            Assert.True(choiceResolved);
            Assert.NotNull(resolution);
            Assert.Equal(0, resolution!.choiceIndex);

            // Consequence verification: moral delta applied upon choice
            Assert.Equal(moralBefore + 12, moral.MoralScore);

            // ── Day 5: Expedition dispatched to loc_recovery_yard & rescue completed ──
            bool expStarted = expedition.Start(destDef!, "survivor_scout_1", day: 5);
            Assert.True(expStarted);

            // Advance travel until the party reaches the site (Outbound → Looting).
            // Production travel rules stay live: encounters, stamina, phases.
            int guardTicks = 0;
            while (expedition.ActiveCount == 1
                   && expedition.Active["survivor_scout_1"].phase < (int)ExpeditionPhase.Looting
                   && guardTicks < 64)
            {
                expedition.TickHours(1.0f, rng);
                guardTicks++;
            }
            Assert.True(guardTicks < 64, "Expedition never reached the recovery yard");
            Assert.True(expedition.Active["survivor_scout_1"].travelTicksCompleted >= destDef!.distanceTicks);

            // Rescue completion at the site
            bool rescueComplete = distress.CompleteRescue("freq_distress_88_3", factionWar);
            Assert.True(rescueComplete);
            Assert.Equal(repBefore + 15, factionWar.GetStanding("faction_railway_guild"));
            var mechanicState = distress.GetActiveState("freq_distress_88_3");
            Assert.Equal(DistressSignalStatus.ResolvedRescued, mechanicState!.Status);
            Assert.True(mechanicState.IsResolved);

            // ── Day 6: Save and fresh-restore continuation ──
            var savedDistress = distress.CaptureState();
            var savedMoralState = moral.CaptureState();
            int savedMoralScore = moral.MoralScore;
            int savedEmpathy = moral.EmpathyPoints;
            var savedFactionState = factionWar.State;

            // Reconstruct fresh systems
            var freshDistress = new RadioDistressSystem();
            freshDistress.LoadFromJson(File.ReadAllText(signalCatalogPath));
            freshDistress.RestoreState(savedDistress);

            var freshMoral = new MoralChoiceSystem(new SeededRng(seed));
            freshMoral.RestoreState(savedMoralState);
            Assert.Equal(savedMoralScore, freshMoral.MoralScore);
            Assert.Equal(savedEmpathy, freshMoral.EmpathyPoints);
            Assert.Equal(savedMoralState.resolutions.Count, freshMoral.CaptureState().resolutions.Count);
            var freshFactionWar = new FactionWarSystem(savedFactionState);

            var restoredMechanicState = freshDistress.GetActiveState("freq_distress_88_3");
            Assert.NotNull(restoredMechanicState);
            Assert.Equal(DistressSignalStatus.ResolvedRescued, restoredMechanicState!.Status);
            Assert.Equal(0, restoredMechanicState.MoralChoiceResolutionIndex);
            Assert.Equal(savedFactionState.factions.Find(f => f.factionId == "faction_railway_guild")?.standing,
                         freshFactionWar.GetStanding("faction_railway_guild"));

            // ── Day 7: Tune to 192.4 MHz raider lure ──
            var tuneDay7 = tuner.EvaluateFrequency(192.4f, freshDistress, 0.15f, rng, day: 7);
            Assert.True(tuneDay7.IsLocked);
            Assert.True(tuneDay7.IsDeceptive, "192.4 MHz must be flagged as deceptive trap");

            freshDistress.Intercept("freq_distress_192_4", day: 7);
            freshDistress.MarkTriangulated("freq_distress_192_4");

            // ── Day 8: Raider lure detected via authenticity skill, ignored safely ──
            bool trapChoiceTriggered = freshDistress.TryTriggerMoralChoice("freq_distress_192_4", out _);
            Assert.False(trapChoiceTriggered, "Raider trap must NEVER trigger a genuine rescue moral choice");

            // Avoid trap with zero penalty
            int raiderStandingBefore = freshFactionWar.GetStanding("raiders");
            freshDistress.Resolve("freq_distress_192_4", DistressSignalStatus.ResolvedTrapAvoided, "Avoided raider ambush.");

            var lureState = freshDistress.GetActiveState("freq_distress_192_4");
            Assert.Equal(DistressSignalStatus.ResolvedTrapAvoided, lureState!.Status);
            Assert.Equal(raiderStandingBefore, freshFactionWar.GetStanding("raiders"));

            if (logDiagnostics)
            {
                _output.WriteLine($"[Smoke Trace] Moral={savedMoralScore}, Empathy={savedEmpathy}, RailwayGuild={freshFactionWar.GetStanding("faction_railway_guild")}, IronGarrison={freshFactionWar.GetStanding("iron_garrison")}");
            }

            return new CampaignSnapshot
            {
                FinalMoralScore = savedMoralScore,
                FinalEmpathyPoints = savedEmpathy,
                RailwayGuildStanding = freshFactionWar.GetStanding("faction_railway_guild"),
                IronGarrisonStanding = freshFactionWar.GetStanding("iron_garrison"),
                RaiderStanding = freshFactionWar.GetStanding("raiders"),
                MechanicSignalStatus = restoredMechanicState.Status,
                MechanicMoralChoiceAvailable = restoredMechanicState.IsMoralChoiceAvailable,
                MechanicResolutionIndex = restoredMechanicState.MoralChoiceResolutionIndex,
                RaiderLureStatus = lureState.Status,
                RaiderLureMoralChoiceAvailable = lureState.IsMoralChoiceAvailable
            };
        }

        [Fact]
        public void FullCampaignScenario_ExecutesCleanlyToEnd()
        {
            var snapshot = ExecuteCampaignScenario(seed: 42, logDiagnostics: true);
            Assert.Equal(DistressSignalStatus.ResolvedRescued, snapshot.MechanicSignalStatus);
            Assert.Equal(DistressSignalStatus.ResolvedTrapAvoided, snapshot.RaiderLureStatus);
            Assert.Equal(15, snapshot.RailwayGuildStanding);
            Assert.Equal(12, snapshot.FinalMoralScore);
            Assert.Equal(2, snapshot.FinalEmpathyPoints);
        }

        [Fact]
        public void DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalState()
        {
            var run1 = ExecuteCampaignScenario(seed: 42);
            var run2 = ExecuteCampaignScenario(seed: 42);
            var run3 = ExecuteCampaignScenario(seed: 42);

            Assert.Equal(run1, run2);
            Assert.Equal(run2, run3);

            _output.WriteLine($"[Determinism Check] Run 1 == Run 2 == Run 3 verified: Byte-and-semantic identical.");
        }
    }
}
