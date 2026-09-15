// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Plan §5.8 — persistent dispatch association. Mission selection at
    /// dispatch time and arrival resolution are Core-owned rules: the
    /// persisted ExpeditionId is authoritative, destination matching is only
    /// a legacy fallback, and unrelated arrivals can never resolve a rescue.
    /// </summary>
    public sealed class RescueSignalDispatchAssociationTests
    {
        private readonly DistressRescueMissionManager _manager = new DistressRescueMissionManager();

        private const string MechanicQuest = "quest_distress_trapped_mechanic";
        private const string MechanicSignal = "freq_distress_88_3";
        private const string MechanicDest = "loc_recovery_yard";

        [Fact]
        public void IdentifiedOutranksHeard_OnDestinationCollision()
        {
            // Synthetic collision: two active missions share a destination.
            var heard = _manager.GetMissionByQuest("quest_distress_family_shelter")!;
            heard.DestinationId = MechanicDest;
            _manager.RecordSignalHeard("freq_distress_445_2", 1);
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordSignalIdentified(MechanicSignal); // triangulated

            var selected = _manager.GetActiveMissionForDispatch(MechanicDest);
            Assert.NotNull(selected);
            Assert.Equal(MechanicQuest, selected!.QuestId);
        }

        [Fact]
        public void HeardOnlyMission_StillSelectable()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            var selected = _manager.GetActiveMissionForDispatch(MechanicDest);
            Assert.Equal(MechanicQuest, selected!.QuestId);
        }

        [Fact]
        public void AlreadyDispatchedMission_NotReselected()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_a_loc");
            Assert.Null(_manager.GetActiveMissionForDispatch(MechanicDest));
        }

        [Fact]
        public void Arrival_MatchesPersistedAssociation_OverDestinationCollision()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_survivorA_loc_recovery_yard");

            // Synthetic collision: another mission shares the destination and
            // is also dispatched (different survivor).
            var other = _manager.GetMissionByQuest("quest_distress_injured_trader")!;
            other.DestinationId = MechanicDest;
            other.Stage = DistressRescueMissionStage.Dispatched;
            other.ExpeditionId = "exp_survivorB_loc_recovery_yard";

            var match = _manager.GetMissionForArrival(MechanicDest, "exp_survivorA_loc_recovery_yard");
            Assert.Equal(MechanicQuest, match!.QuestId);

            var matchB = _manager.GetMissionForArrival(MechanicDest, "exp_survivorB_loc_recovery_yard");
            Assert.Equal("quest_distress_injured_trader", matchB!.QuestId);
        }

        [Fact]
        public void UnrelatedArrival_DoesNotResolveRescue_PlanTest13()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_survivorA_loc_recovery_yard");

            // A different survivor's expedition to the same shared location is
            // NOT the rescue expedition — no resolution.
            Assert.Null(_manager.GetMissionForArrival(MechanicDest, "exp_survivorZ_loc_recovery_yard"));
        }

        [Fact]
        public void LegacySaves_WithoutAssociation_KeepDestinationFallback()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_survivorA_loc_recovery_yard");
            // Simulate a legacy save that never persisted the association.
            _manager.GetMissionByQuest(MechanicQuest)!.ExpeditionId = string.Empty;

            var match = _manager.GetMissionForArrival(MechanicDest, "exp_survivorZ_loc_recovery_yard");
            Assert.Equal(MechanicQuest, match!.QuestId);
        }

        [Fact]
        public void AssociationSurvivesSaveReload_ArrivalResolvesAfterRestore()
        {
            _manager.RecordSignalHeard(MechanicSignal, 2);
            _manager.RecordExpeditionDispatched(MechanicQuest, "exp_survivorA_loc_recovery_yard");

            var fresh = new DistressRescueMissionManager();
            fresh.RestoreState(_manager.CaptureState());
            var match = fresh.GetMissionForArrival(MechanicDest, "exp_survivorA_loc_recovery_yard");
            Assert.Equal(MechanicQuest, match!.QuestId);
            Assert.Equal(DistressRescueMissionStage.TerminalRescued,
                fresh.RecordDestinationReached(MechanicQuest, 6));
        }

        [Fact]
        public void DoubleDispatchToSameDestination_TiebreakIsDeterministic()
        {
            // Two heard missions at the same destination, heard the same day:
            // ordinal QuestId order breaks the tie, deterministically.
            var a = _manager.GetMissionByQuest("quest_distress_family_shelter")!;
            a.DestinationId = MechanicDest;
            _manager.RecordSignalHeard("freq_distress_445_2", 2);
            _manager.RecordSignalHeard(MechanicSignal, 2);

            var selected = _manager.GetActiveMissionForDispatch(MechanicDest);
            // Same InterceptedDay → ordinal QuestId order ("family_shelter" <
            // "trapped_mechanic") breaks the tie, deterministically.
            Assert.Equal("quest_distress_family_shelter", selected!.QuestId);
        }
    }
}
