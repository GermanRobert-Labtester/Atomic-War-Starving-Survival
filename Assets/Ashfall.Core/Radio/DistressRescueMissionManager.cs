// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    public enum DistressRescueMissionStage
    {
        None = 0,
        Heard = 1,
        Identified = 2,
        Dispatched = 3,
        Reached = 4,
        TerminalRescued = 5,
        TerminalFailed = 6,
        TerminalAmbush = 7,
        TerminalSurvived = 8
    }

    [Serializable]
    public sealed class DistressRescueMission
    {
        public string QuestId { get; set; } = string.Empty;
        public string SignalId { get; set; } = string.Empty;
        public string DestinationId { get; set; } = string.Empty;
        public string ExpeditionId { get; set; } = string.Empty;
        public DistressRescueMissionStage Stage { get; set; } = DistressRescueMissionStage.None;
        public int InterceptedDay { get; set; }
        public int DaysToTrace { get; set; } = 3;
        public int DeadlineDays { get; set; } = 5;
        public int ExpiryDay => InterceptedDay + DeadlineDays;
        public bool IsTerminal => Stage == DistressRescueMissionStage.TerminalRescued ||
                                  Stage == DistressRescueMissionStage.TerminalFailed ||
                                  Stage == DistressRescueMissionStage.TerminalSurvived;
        public bool IsTrap { get; set; }
        public string FactionTag { get; set; } = string.Empty;
        public List<string> RewardItems { get; set; } = new List<string>();
        public int RewardReputation { get; set; }
        public bool RewardClaimed { get; set; }
        public bool ReputationClaimed { get; set; }
        public string OutcomeSummary { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DistressMissionSaveState
    {
        public int Version { get; set; } = 1;
        public List<DistressRescueMission> Missions { get; set; } = new List<DistressRescueMission>();
        public List<string> ClaimedReceipts { get; set; } = new List<string>();
    }

    /// <summary>
    /// Coordinates the 5 flagship distress radio rescue missions through their complete staged progression:
    /// Heard -> Identified -> Dispatched -> Reached -> Terminal (Rescued | Failed | Ambush/Survived).
    /// Guarantees strictly monotonic stage transitions, deterministic deadline math, and idempotent rewards.
    /// Invariant: Pure C#, zero engine references.
    /// </summary>
    public sealed class DistressRescueMissionManager
    {
        public const string SystemId = "distress_rescue_mission_manager";

        private readonly Dictionary<string, DistressRescueMission> _missionsByQuest =
            new Dictionary<string, DistressRescueMission>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, DistressRescueMission> _missionsBySignal =
            new Dictionary<string, DistressRescueMission>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> _claimedReceipts =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly DistressDestinationResolver _destinationResolver;

        public event Action<DistressRescueMission, DistressRescueMissionStage>? OnStageChanged;
        public event Action<DistressRescueMission, List<string>, int>? OnRewardsGranted;

        public DistressRescueMissionManager(DistressDestinationResolver? destinationResolver = null)
        {
            _destinationResolver = destinationResolver ?? DistressDestinationResolver.Default;
            RegisterAuthoredRescueMissions();
        }

        public IReadOnlyCollection<DistressRescueMission> AllMissions => _missionsByQuest.Values;

        public DistressRescueMission? GetMissionByQuest(string questId)
        {
            if (string.IsNullOrWhiteSpace(questId)) return null;
            return _missionsByQuest.TryGetValue(questId.Trim(), out var m) ? m : null;
        }

        public DistressRescueMission? GetMissionBySignal(string signalId)
        {
            if (string.IsNullOrWhiteSpace(signalId)) return null;
            return _missionsBySignal.TryGetValue(signalId.Trim(), out var m) ? m : null;
        }

        public bool IsReceiptClaimed(string questId, string signalId)
        {
            string key = $"{questId}:{signalId}";
            return _claimedReceipts.Contains(key);
        }

        /// <summary>
        /// Transitions a mission to Heard stage when its frequency is intercepted on the tuner.
        /// </summary>
        public bool RecordSignalHeard(string signalId, int day)
        {
            var mission = GetMissionBySignal(signalId);
            if (mission == null) return false;
            if (mission.Stage != DistressRescueMissionStage.None) return false; // Already progressed

            mission.Stage = DistressRescueMissionStage.Heard;
            mission.InterceptedDay = day;
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Heard);
            return true;
        }

        /// <summary>
        /// Transitions a mission to Identified stage when its message fragments are resolved and destination triangulated.
        /// </summary>
        public bool RecordSignalIdentified(string signalId)
        {
            var mission = GetMissionBySignal(signalId);
            if (mission == null) return false;
            if (mission.Stage != DistressRescueMissionStage.Heard) return false; // Must be Heard first

            mission.Stage = DistressRescueMissionStage.Identified;
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Identified);
            return true;
        }

        /// <summary>
        /// Transitions a mission to Dispatched stage when an expedition is launched toward its destination.
        /// </summary>
        public bool RecordExpeditionDispatched(string questId, string expeditionId)
        {
            var mission = GetMissionByQuest(questId);
            if (mission == null) return false;
            if (mission.Stage != DistressRescueMissionStage.Identified &&
                mission.Stage != DistressRescueMissionStage.Heard) // Allow dispatch if already identified or heard
            {
                return false;
            }

            mission.ExpeditionId = expeditionId;
            mission.Stage = DistressRescueMissionStage.Dispatched;
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Dispatched);
            return true;
        }

        /// <summary>
        /// Transitions a mission to Reached stage when an expedition arrives at the destination site.
        /// Evaluates whether the arrival occurred within the deadline or too late.
        /// </summary>
        public DistressRescueMissionStage RecordDestinationReached(string questId, int currentDay)
        {
            var mission = GetMissionByQuest(questId);
            if (mission == null) return DistressRescueMissionStage.None;
            if (mission.Stage != DistressRescueMissionStage.Dispatched) return mission.Stage;

            // Check deadline
            if (currentDay > mission.ExpiryDay)
            {
                mission.Stage = DistressRescueMissionStage.TerminalFailed;
                mission.OutcomeSummary = $"Expedition arrived on Day {currentDay}, past the rescue deadline (Day {mission.ExpiryDay}). The beacon was dead.";
                OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalFailed);
                return mission.Stage;
            }

            mission.Stage = DistressRescueMissionStage.Reached;
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Reached);

            // Auto-transition to terminal based on mission kind
            if (mission.IsTrap)
            {
                mission.Stage = DistressRescueMissionStage.TerminalAmbush;
                mission.OutcomeSummary = "The signal was bait. Raider ambush encountered upon arrival!";
                OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalAmbush);
            }
            else
            {
                mission.Stage = DistressRescueMissionStage.TerminalRescued;
                mission.OutcomeSummary = "Survivor reached in time and successfully rescued.";
                OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalRescued);
            }

            return mission.Stage;
        }

        /// <summary>
        /// Resolves an ambush encounter into TerminalSurvived.
        /// </summary>
        public bool ResolveAmbushSurvived(string questId, string outcomeNotes = "Ambush repelled.")
        {
            var mission = GetMissionByQuest(questId);
            if (mission == null) return false;
            if (mission.Stage != DistressRescueMissionStage.TerminalAmbush) return false;

            mission.Stage = DistressRescueMissionStage.TerminalSurvived;
            mission.OutcomeSummary = outcomeNotes;
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalSurvived);
            return true;
        }

        /// <summary>
        /// Claims mission rewards and faction reputation exactly once (idempotent).
        /// Returns granted items and reputation delta. Subsequent calls return empty list and 0 delta.
        /// </summary>
        public (List<string> Items, int ReputationDelta) ClaimIdempotentRewards(string questId)
        {
            var mission = GetMissionByQuest(questId);
            if (mission == null) return (new List<string>(), 0);

            string receiptKey = $"{mission.QuestId}:{mission.SignalId}";
            if (_claimedReceipts.Contains(receiptKey) || mission.RewardClaimed)
            {
                return (new List<string>(), 0); // Already claimed, zero double grants
            }

            if (mission.Stage != DistressRescueMissionStage.TerminalRescued &&
                mission.Stage != DistressRescueMissionStage.TerminalSurvived)
            {
                return (new List<string>(), 0); // Not successfully resolved
            }

            var grantedItems = new List<string>(mission.RewardItems);
            int rep = mission.RewardReputation;

            mission.RewardClaimed = true;
            mission.ReputationClaimed = true;
            _claimedReceipts.Add(receiptKey);

            OnRewardsGranted?.Invoke(mission, grantedItems, rep);
            return (grantedItems, rep);
        }

        /// <summary>
        /// Daily tick to evaluate expiration of unreached missions.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission.IsTerminal) continue;
                if (mission.Stage == DistressRescueMissionStage.Heard ||
                    mission.Stage == DistressRescueMissionStage.Identified)
                {
                    if (currentDay > mission.ExpiryDay)
                    {
                        mission.Stage = DistressRescueMissionStage.TerminalFailed;
                        mission.OutcomeSummary = $"Distress signal expired on Day {currentDay} (deadline: Day {mission.ExpiryDay}).";
                        OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalFailed);
                    }
                }
            }
        }

        public DistressMissionSaveState CaptureState()
        {
            var state = new DistressMissionSaveState();
            foreach (var m in _missionsByQuest.Values)
            {
                state.Missions.Add(new DistressRescueMission
                {
                    QuestId = m.QuestId,
                    SignalId = m.SignalId,
                    DestinationId = m.DestinationId,
                    ExpeditionId = m.ExpeditionId,
                    Stage = m.Stage,
                    InterceptedDay = m.InterceptedDay,
                    DaysToTrace = m.DaysToTrace,
                    DeadlineDays = m.DeadlineDays,
                    IsTrap = m.IsTrap,
                    FactionTag = m.FactionTag,
                    RewardItems = new List<string>(m.RewardItems),
                    RewardReputation = m.RewardReputation,
                    RewardClaimed = m.RewardClaimed,
                    ReputationClaimed = m.ReputationClaimed,
                    OutcomeSummary = m.OutcomeSummary
                });
            }
            state.ClaimedReceipts = new List<string>(_claimedReceipts);
            state.ClaimedReceipts.Sort(StringComparer.Ordinal);
            return state;
        }

        public void RestoreState(DistressMissionSaveState? state)
        {
            if (state == null) return;
            _claimedReceipts.Clear();
            if (state.ClaimedReceipts != null)
            {
                foreach (var r in state.ClaimedReceipts)
                    if (!string.IsNullOrEmpty(r))
                        _claimedReceipts.Add(r);
            }

            if (state.Missions != null)
            {
                foreach (var saved in state.Missions)
                {
                    if (saved == null || string.IsNullOrEmpty(saved.QuestId)) continue;
                    if (_missionsByQuest.TryGetValue(saved.QuestId, out var existing))
                    {
                        existing.Stage = saved.Stage;
                        existing.InterceptedDay = saved.InterceptedDay;
                        existing.DaysToTrace = saved.DaysToTrace;
                        existing.DeadlineDays = saved.DeadlineDays;
                        existing.ExpeditionId = saved.ExpeditionId;
                        existing.DestinationId = saved.DestinationId;
                        existing.RewardClaimed = saved.RewardClaimed;
                        existing.ReputationClaimed = saved.ReputationClaimed;
                        existing.OutcomeSummary = saved.OutcomeSummary;
                    }
                    else
                    {
                        _missionsByQuest[saved.QuestId] = saved;
                        if (!string.IsNullOrEmpty(saved.SignalId))
                            _missionsBySignal[saved.SignalId] = saved;
                    }
                }
            }
        }

        private void RegisterAuthoredRescueMissions()
        {
            void Add(string qId, string sigId, string rawDest, int trace, int deadline, bool isTrap, string faction, int rep, params string[] items)
            {
                var res = _destinationResolver.Resolve(rawDest);
                var m = new DistressRescueMission
                {
                    QuestId = qId,
                    SignalId = sigId,
                    DestinationId = res.DestinationId,
                    DaysToTrace = trace,
                    DeadlineDays = deadline,
                    IsTrap = isTrap,
                    FactionTag = faction,
                    RewardReputation = rep,
                    RewardItems = new List<string>(items)
                };
                _missionsByQuest[qId] = m;
                _missionsBySignal[sigId] = m;
            }

            // 1. Trapped Mechanic
            Add("quest_distress_trapped_mechanic", "freq_distress_88_3", "loc_recovery_yard", 3, 5, false, "neutral", 5, "scrap_metal", "mechanical_parts");

            // 2. Injured Trader
            Add("quest_distress_injured_trader", "freq_distress_156_8", "rural_gas_station", 2, 3, false, "neutral", 3, "bandage", "battery", "iodine_pills");

            // 3. Family Shelter
            Add("quest_distress_family_shelter", "freq_distress_445_2", "family_bunker_backyard_shed", 2, 4, false, "neutral", 8, "clean_water", "canned_food");

            // 4. Raider Trap
            Add("quest_distress_raider_trap", "freq_distress_192_4", "loc_denial_cut_substation", 3, 5, true, "raiders", 0);

            // 5. Military Patrol
            Add("quest_distress_military_patrol", "freq_distress_901_2", "checkpoint_kilo_armory", 3, 5, false, "military", 10, "ammo_556", "field_dressing_kit");
        }
    }
}
