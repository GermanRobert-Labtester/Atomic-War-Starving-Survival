// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

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

        // ── Rescue-signal runtime extension (Tasks 1–4). Additive, legacy-safe
        // defaults: all defaults reproduce the pre-extension behavior exactly.

        /// <summary>Live-sender model: true while the sender is believed alive.
        /// Defaults true; a mission with no survival model never ticks this off
        /// (stale/no-sender signals must not receive live-sender processing).</summary>
        public bool SenderAlive { get; set; } = true;

        /// <summary>Day the sender physically dies (InterceptedDay + survival).
        /// 0 = no live-sender model — legacy deadline rules apply.</summary>
        public int SenderDeathDay { get; set; }

        /// <summary>Authored survival days from first-heard (0 = none).</summary>
        public int SenderSurvivalDays { get; set; }

        /// <summary>Deadline passed for a consequence-bearing signal without
        /// dispatch. Consequence applied once; mission stays dispatchable for
        /// the remains/recovery branch. Signals without a consequence keep the
        /// legacy TerminalFailed expiry instead.</summary>
        public bool Expired { get; set; }

        /// <summary>Idempotence guard: the configured ignore consequence has
        /// already been applied (exactly once, survives save/load).</summary>
        public bool IgnoreConsequenceApplied { get; set; }

        /// <summary>Campaign day the consequence fired (-1 = never).</summary>
        public int IgnoreConsequenceAppliedDay { get; set; } = -1;

        /// <summary>Authored consequence token(s), e.g. "sender_death",
        /// "faction_standing_loss". Empty = legacy expiry semantics.</summary>
        public List<string> IgnoreConsequenceTokens { get; set; } = new List<string>();

        /// <summary>Canonical faction id for the standing-loss consequence.
        /// Empty = no faction penalty (Core never mutates standing itself).</summary>
        public string FactionStandingLossFactionId { get; set; } = string.Empty;

        /// <summary>True when an expedition actually reached the destination —
        /// prevents duplicate recruit/salvage across reloads and callbacks.</summary>
        public bool ArrivalResolved { get; set; }

        /// <summary>First valid authenticity analysis persisted (anti-reroll).</summary>
        public bool AuthenticityChecked { get; set; }

        /// <summary>Persisted assessment (int of SignalAuthenticityCategory).</summary>
        public int AuthenticityAssessment { get; set; }

        public bool AssessmentThreatDetected { get; set; }
        public bool AssessmentStalenessDetected { get; set; }
        public string AssessmentSkillId { get; set; } = string.Empty;
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

        /// <summary>
        /// Canonical fingerprint over the mission list (public field so the
        /// reflection-walking SaveChecksum covers the property-based mission
        /// DTO). Empty on legacy saves — never enforced there.
        /// </summary>
        public string missionsFingerprint = string.Empty;

        /// <summary>Deterministic invariant-culture canonical text of the state.</summary>
        public static string ComputeFingerprint(DistressMissionSaveState state)
        {
            if (state == null) return string.Empty;
            var sb = new System.Text.StringBuilder(512);
            var missions = state.Missions ?? new List<DistressRescueMission>();
            var sorted = new List<DistressRescueMission>(missions);
            sorted.Sort((a, b) => string.Compare(a?.QuestId, b?.QuestId, StringComparison.Ordinal));
            foreach (var m in sorted)
            {
                if (m == null) continue;
                sb.Append(m.QuestId).Append('|').Append(m.SignalId).Append('|').Append(m.DestinationId)
                  .Append('|').Append(m.ExpeditionId).Append('|').Append((int)m.Stage)
                  .Append('|').Append(m.InterceptedDay).Append('|').Append(m.DaysToTrace)
                  .Append('|').Append(m.DeadlineDays).Append('|').Append(m.IsTrap ? '1' : '0')
                  .Append('|').Append(m.RewardClaimed ? '1' : '0').Append(m.ReputationClaimed ? '1' : '0')
                  .Append('|').Append(m.SenderAlive ? '1' : '0').Append('|').Append(m.SenderDeathDay)
                  .Append('|').Append(m.SenderSurvivalDays).Append('|').Append(m.Expired ? '1' : '0')
                  .Append('|').Append(m.IgnoreConsequenceApplied ? '1' : '0').Append('|').Append(m.IgnoreConsequenceAppliedDay)
                  .Append('|');
                var tokens = m.IgnoreConsequenceTokens ?? new List<string>();
                var tokSorted = new List<string>(tokens);
                tokSorted.Sort(StringComparer.Ordinal);
                sb.Append(string.Join(",", tokSorted)).Append('|').Append(m.FactionStandingLossFactionId)
                  .Append('|').Append(m.ArrivalResolved ? '1' : '0')
                  .Append('|').Append(m.AuthenticityChecked ? '1' : '0').Append('|').Append(m.AuthenticityAssessment)
                  .Append('|').Append(m.AssessmentThreatDetected ? '1' : '0').Append(m.AssessmentStalenessDetected ? '1' : '0')
                  .Append('|').Append(m.AssessmentSkillId).Append('\n');
            }
            var receipts = state.ClaimedReceipts ?? new List<string>();
            var recSorted = new List<string>(receipts);
            recSorted.Sort(StringComparer.Ordinal);
            sb.Append("receipts:").Append(string.Join(",", recSorted));
            return sb.ToString();
        }

        /// <summary>Recomputes and stores the fingerprint on this state.</summary>
        public void RefreshFingerprint() => missionsFingerprint = ComputeFingerprint(this);
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

        /// <summary>Optional definition source for authenticity analysis.</summary>
        private readonly RadioDistressSystem? _distressSystem;

        /// <summary>Tasks 9–12 Wave 2 — optional signal-trust ledger. When bound,
        /// the manager records trust events at its exactly-once guarded
        /// lifecycle transitions (answered dispatch, actionable expiry, trap
        /// ambush encountered, successful rescue). Null = trust tracking off
        /// (legacy behavior, byte-identical).</summary>
        private readonly SignalTrustLedger? _signalTrust;

        public event Action<DistressRescueMission, DistressRescueMissionStage>? OnStageChanged;
        public event Action<DistressRescueMission, List<string>, int>? OnRewardsGranted;

        /// <summary>
        /// Core fact raised exactly once when a configured ignore consequence is
        /// applied (deadline reached on a discovered, unresolved signal).
        /// Core owns the state change; the host applies presentation and
        /// faction-standing effects. <paramref name="factionId"/> is empty when
        /// the consequence carries no standing loss.
        /// </summary>
        public event Action<DistressRescueMission, List<string>, string, int>? OnIgnoreConsequence;

        public DistressRescueMissionManager(
            DistressDestinationResolver? destinationResolver = null,
            RadioDistressSystem? distressSystem = null,
            SignalTrustLedger? signalTrust = null)
        {
            _destinationResolver = destinationResolver ?? DistressDestinationResolver.Default;
            _distressSystem = distressSystem;
            _signalTrust = signalTrust;
            RegisterAuthoredRescueMissions();
        }

        /// <summary>Closed consequence-token vocabulary (catalog validation).</summary>
        public static readonly string[] KnownConsequenceTokens = { "sender_death", "faction_standing_loss", "faction_ambush" };

        /// <summary>Consequence tokens that affect faction standing (the host
        /// applies the standing change for any of them).</summary>
        private static bool AffectsStanding(string token) =>
            token == "faction_standing_loss" || token == "faction_ambush";

        /// <summary>True when every token in the list is a known consequence.</summary>
        public static bool AreConsequenceTokensValid(IEnumerable<string>? tokens)
        {
            if (tokens == null) return true;
            foreach (var t in tokens)
                if (!string.IsNullOrEmpty(t) && Array.IndexOf(KnownConsequenceTokens, t) < 0)
                    return false;
            return true;
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

        /// <summary>
        /// Active (non-terminal) mission whose destination matches a location id,
        /// used by the expedition host bridge.
        /// </summary>
        public DistressRescueMission? GetActiveMissionByDestination(string destinationId)
        {
            if (string.IsNullOrWhiteSpace(destinationId)) return null;
            string key = destinationId.Trim();
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission == null || mission.IsTerminal) continue;
                if (string.Equals(mission.DestinationId, key, StringComparison.OrdinalIgnoreCase))
                    return mission;
            }
            return null;
        }

        /// <summary>
        /// Selects the mission an expedition dispatch toward a location should
        /// associate with (plan §5.8). Active non-terminal missions only; a
        /// triangulated (Identified) mission outranks a merely heard one, and
        /// ties break deterministically (InterceptedDay, then QuestId).
        /// </summary>
        public DistressRescueMission? GetActiveMissionForDispatch(string destinationId)
        {
            if (string.IsNullOrWhiteSpace(destinationId)) return null;
            string key = destinationId.Trim();
            DistressRescueMission? identified = null, heard = null;
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission == null || mission.IsTerminal) continue;
                if (!string.Equals(mission.DestinationId, key, StringComparison.OrdinalIgnoreCase)) continue;
                if (mission.Stage == DistressRescueMissionStage.Identified)
                {
                    if (identified == null || PreferMission(mission, identified)) identified = mission;
                }
                else if (mission.Stage == DistressRescueMissionStage.Heard)
                {
                    if (heard == null || PreferMission(mission, heard)) heard = mission;
                }
            }
            return identified ?? heard;
        }

        /// <summary>
        /// Resolves the mission an ARRIVING expedition is associated with
        /// (plan §5.8: never destination-only). The persisted dispatch
        /// association (ExpeditionId written at dispatch) is authoritative;
        /// the destination fallback applies only when no mission tied to this
        /// destination carries an association — so an unrelated expedition to
        /// a shared rescue location can never resolve the rescue quest
        /// (plan §5.9 test 13). Legacy saves without associations keep the
        /// old destination behavior.
        /// </summary>
        public DistressRescueMission? GetMissionForArrival(string destinationId, string expeditionId)
        {
            if (string.IsNullOrWhiteSpace(destinationId)) return null;
            string key = destinationId.Trim();

            DistressRescueMission? associated = null;
            bool anyAssociationForDestination = false;
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission == null || mission.IsTerminal) continue;
                if (!string.Equals(mission.DestinationId, key, StringComparison.OrdinalIgnoreCase)) continue;
                if (string.IsNullOrEmpty(mission.ExpeditionId)) continue;
                anyAssociationForDestination = true;
                if (!string.IsNullOrEmpty(expeditionId) &&
                    string.Equals(mission.ExpeditionId, expeditionId, StringComparison.OrdinalIgnoreCase))
                {
                    if (associated == null || PreferMission(mission, associated)) associated = mission;
                }
            }
            if (associated != null) return associated;
            if (anyAssociationForDestination) return null; // unrelated arrival

            // Legacy fallback: destination match on dispatched/ambush missions.
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission == null) continue;
                if (!string.Equals(mission.DestinationId, key, StringComparison.OrdinalIgnoreCase)) continue;
                if (mission.Stage == DistressRescueMissionStage.Dispatched ||
                    mission.Stage == DistressRescueMissionStage.TerminalAmbush)
                    return mission;
            }
            return null;
        }

        /// <summary>Deterministic tiebreak: earlier first-heard day, then QuestId.</summary>
        private static bool PreferMission(DistressRescueMission candidate, DistressRescueMission incumbent)
        {
            if (candidate.InterceptedDay != incumbent.InterceptedDay)
                return candidate.InterceptedDay < incumbent.InterceptedDay;
            return string.Compare(candidate.QuestId, incumbent.QuestId, StringComparison.Ordinal) < 0;
        }

        public bool IsReceiptClaimed(string questId, string signalId)
        {
            string key = $"{questId}:{signalId}";
            return _claimedReceipts.Contains(key);
        }

        /// <summary>
        /// Transitions a mission to Heard stage when its frequency is intercepted on the tuner.
        /// First-heard day is initialized exactly once — replaying a transmission
        /// never moves the deadline or the sender death day.
        /// </summary>
        public bool RecordSignalHeard(string signalId, int day)
        {
            var mission = GetMissionBySignal(signalId);
            if (mission == null) return false;
            if (mission.Stage != DistressRescueMissionStage.None) return false; // Already progressed

            mission.Stage = DistressRescueMissionStage.Heard;
            mission.InterceptedDay = day;

            // Live-sender model: senderDeathDay = firstHeardDay + survivalDays.
            // Stale/no-sender missions (0 days) never receive death ticking.
            if (mission.SenderSurvivalDays > 0)
            {
                mission.SenderAlive = true;
                mission.SenderDeathDay = day + mission.SenderSurvivalDays;
            }

            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Heard);
            return true;
        }

        /// <summary>
        /// Survivor-driven authenticity analysis (Task 2). Deterministic:
        /// the check sub-stream is derived from (campaign seed, signal,
        /// survivor) — never the shared session RNG — and the first valid
        /// result is persisted; repeat calls return the stored result without
        /// rerolling (anti-save-scum). Staleness is a deterministic function
        /// of trace progress, re-derived on each call, never rolled.
        /// Returns null when the signal or mission is unknown.
        /// </summary>
        public SignalAuthenticityCheckResult? RecordAuthenticityCheck(
            string signalId,
            string survivorId,
            int day,
            int seedBase,
            Func<string, string, bool>? hasSkill = null)
        {
            if (string.IsNullOrWhiteSpace(signalId) || string.IsNullOrWhiteSpace(survivorId))
                return null;

            var mission = GetMissionBySignal(signalId);
            if (mission == null) return null;

            var def = _distressSystem?.GetDefinition(signalId);
            if (def == null) return null;

            int daysTraced = Math.Max(0, day - mission.InterceptedDay);

            // Anti-reroll policy: the deception verdict is persisted on the
            // first valid analysis. Staleness re-derives from live trace state.
            if (mission.AuthenticityChecked)
            {
                bool staleNow = def.Authenticity.Equals("stale", StringComparison.OrdinalIgnoreCase)
                    && daysTraced >= def.DaysToTrace;
                return new SignalAuthenticityCheckResult
                {
                    CheckPerformed = true,
                    ThreatDetected = mission.AssessmentThreatDetected,
                    StalenessDetected = staleNow || mission.AssessmentStalenessDetected,
                    Assessment = staleNow && mission.AuthenticityAssessment == (int)SignalAuthenticityCategory.Uncertain
                        ? SignalAuthenticityCategory.Stale
                        : (SignalAuthenticityCategory)mission.AuthenticityAssessment,
                    SkillId = mission.AssessmentSkillId,
                    Roll = -1 // persisted result — no reroll performed
                };
            }

            var rng = new SeededRng(StableHash.Of(
                $"distress_authenticity:{seedBase}:{signalId}:{survivorId}"));
            var result = SignalAuthenticityEvaluator.Evaluate(def, daysTraced, survivorId, hasSkill, rng);

            mission.AuthenticityChecked = true;
            mission.AuthenticityAssessment = (int)result.Assessment;
            mission.AssessmentThreatDetected = result.ThreatDetected;
            mission.AssessmentStalenessDetected = result.StalenessDetected;
            mission.AssessmentSkillId = result.SkillId;
            return result;
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
            // Tasks 9–12 Wave 2: the authoritative response action — the player
            // dispatched to an actionable signal. Exactly once per signal
            // (ledger dedupes; the stage guard prevents re-entry anyway).
            _signalTrust?.RecordAnswered(mission.SignalId);
            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.Dispatched);
            return true;
        }

        /// <summary>
        /// Transitions a mission to Reached stage when an expedition arrives at
        /// the destination site. Resolution authority is the **arrival day**:
        /// with a live-sender model, arrival on/after the sender death day
        /// resolves the dead branch (remains/recovery), regardless of when the
        /// expedition was dispatched. Missions without a survival model keep
        /// the legacy deadline check. Guarded by ArrivalResolved so reload or
        /// repeat callbacks can never double-resolve.
        /// </summary>
        public DistressRescueMissionStage RecordDestinationReached(string questId, int currentDay)
        {
            var mission = GetMissionByQuest(questId);
            if (mission == null) return DistressRescueMissionStage.None;
            if (mission.ArrivalResolved) return mission.Stage; // duplicate side-effect guard
            if (mission.Stage != DistressRescueMissionStage.Dispatched) return mission.Stage;

            mission.ArrivalResolved = true;

            if (mission.SenderDeathDay > 0)
            {
                // Live-sender model: physical death governs the outcome.
                if (currentDay >= mission.SenderDeathDay)
                {
                    mission.SenderAlive = false;
                    mission.Stage = DistressRescueMissionStage.TerminalFailed;
                    mission.OutcomeSummary = $"Expedition arrived on Day {currentDay}; the sender died on Day {mission.SenderDeathDay}. Only remains remain.";
                    OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalFailed);
                    return mission.Stage;
                }
            }
            else if (currentDay > mission.ExpiryDay)
            {
                // Legacy deadline rule for missions without a survival model.
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
                // Tasks 9–12 Wave 2: the authoritative trap outcome — the player
                // responded and the ambush was actually encountered. Exactly
                // once (ArrivalResolved guard above). Surviving the ambush
                // afterwards (TerminalSurvived) adds NO second trust event.
                _signalTrust?.RecordAmbushEncountered(mission.SignalId);
                OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalAmbush);
            }
            else
            {
                mission.Stage = DistressRescueMissionStage.TerminalRescued;
                mission.OutcomeSummary = "Survivor reached in time and successfully rescued.";
                // Tasks 9–12 Wave 2: the rescue authority resolved the rescue as
                // successful (arrival in time). Exactly once. Late arrivals
                // (TerminalFailed branches above) never reach this line and
                // count only as answered.
                _signalTrust?.RecordRescueSuccessful(mission.SignalId);
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
                // Dead-arrival recovery (plan §8.5): the expedition actually
                // reached the site after the sender died. Remains/salvage is
                // granted through the same idempotent receipt — exactly once,
                // with zero reputation (a late arrival earns no standing).
                // Requires a real arrival (ArrivalResolved) on a mission with a
                // live-sender model, so legacy deadline expiry (no expedition,
                // no survival model) never grants salvage.
                if (mission.Stage == DistressRescueMissionStage.TerminalFailed &&
                    !mission.SenderAlive &&
                    mission.ArrivalResolved &&
                    mission.SenderDeathDay > 0)
                {
                    var salvage = new List<string>(mission.RewardItems);
                    mission.RewardClaimed = true;
                    mission.ReputationClaimed = true;
                    _claimedReceipts.Add(receiptKey);
                    OnRewardsGranted?.Invoke(mission, salvage, 0);
                    return (salvage, 0);
                }
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
        /// Daily tick: evaluates deadline expiry of discovered-but-undispatched
        /// missions and applies the configured ignore consequence exactly once.
        /// Eligibility (plan §7.3): heard/identified, actionable, not resolved,
        /// consequence not already applied, deadline exists. Undiscovered
        /// signals (Stage None) and dispatched missions are never punished.
        /// Signals with no authored consequence keep the legacy TerminalFailed
        /// expiry; consequence-bearing signals stay dispatchable for the
        /// later remains/recovery expedition.
        /// </summary>
        public void TickDaily(int currentDay)
        {
            foreach (var mission in _missionsByQuest.Values)
            {
                if (mission == null || mission.IsTerminal || mission.Expired) continue;
                if (mission.Stage == DistressRescueMissionStage.Heard ||
                    mission.Stage == DistressRescueMissionStage.Identified)
                {
                    // Plan §7.4 boundary: currentDay >= deadlineDay.
                    if (currentDay >= mission.ExpiryDay)
                    {
                        if (mission.IgnoreConsequenceTokens.Count > 0)
                        {
                            mission.Expired = true;
                            ApplyIgnoreConsequence(mission, currentDay);
                            // Tasks 9–12 Wave 2: discovered, actionable, response
                            // window expired unanswered → ignored. Exactly once
                            // per signal (Expired guard + ledger dedupe). Trap-class
                            // signals are excluded below — letting a lure expire is
                            // authored as wisdom, not a trust deficit.
                            if (!mission.IsTrap)
                                _signalTrust?.RecordIgnored(mission.SignalId);
                        }
                        else
                        {
                            // Legacy expiry semantics (no authored consequence).
                            // Same trust semantics: the opportunity was actionable
                            // and expired unanswered (trap-class excluded).
                            mission.Stage = DistressRescueMissionStage.TerminalFailed;
                            mission.OutcomeSummary = $"Distress signal expired on Day {currentDay} (deadline: Day {mission.ExpiryDay}).";
                            if (!mission.IsTrap)
                                _signalTrust?.RecordIgnored(mission.SignalId);
                            OnStageChanged?.Invoke(mission, DistressRescueMissionStage.TerminalFailed);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Structured dispatch preflight (plan §10): a truthful projection of
        /// the persisted signal state informing the player's dispatch choice.
        /// Analysis informs agency — it never auto-disables dispatch, and this
        /// projection performs no side effects and consumes no randomness.
        /// Returns null when the signal has no registered rescue mission.
        /// </summary>
        public RescueDispatchPreflight? GetDispatchPreflight(string signalId)
        {
            var mission = GetMissionBySignal(signalId);
            if (mission == null) return null;

            var p = new RescueDispatchPreflight
            {
                SignalId = mission.SignalId,
                QuestId = mission.QuestId,
                Analyzed = mission.AuthenticityChecked,
                Assessment = (SignalAuthenticityCategory)mission.AuthenticityAssessment,
                ThreatDetected = mission.AssessmentThreatDetected,
                SenderAlive = mission.SenderAlive,
                Expired = mission.Expired
            };

            if (mission.IsTerminal)
            {
                p.Recommendation = RescueDispatchRecommendation.NotApplicable;
                p.Note = mission.OutcomeSummary;
                return p;
            }

            if (mission.Expired || (!mission.SenderAlive && mission.SenderDeathDay > 0))
            {
                p.Recommendation = RescueDispatchRecommendation.RecoveryInvestigation;
                p.Note = "The call has gone unanswered past its deadline; expect a recovery, not a rescue.";
                return p;
            }

            if (mission.AuthenticityChecked && mission.AssessmentThreatDetected)
            {
                p.Recommendation = RescueDispatchRecommendation.DispatchWithWarning;
                p.Note = p.Assessment == SignalAuthenticityCategory.FalseFlag
                    ? "Signal flagged as a false flag; dispatch at your own risk."
                    : "Signal flagged as a trap; dispatch at your own risk.";
                return p;
            }

            if (p.Assessment == SignalAuthenticityCategory.Stale)
            {
                p.Recommendation = RescueDispatchRecommendation.RecoveryInvestigation;
                p.Note = "Signal confirmed stale; any expedition is an investigation.";
                return p;
            }

            p.Recommendation = RescueDispatchRecommendation.DispatchRescue;
            p.Note = mission.AuthenticityChecked
                ? "No deception detected; dispatch normally."
                : "Signal not yet analyzed; dispatch normally.";
            return p;
        }

        /// <summary>
        /// Applies the mission's authored ignore consequence exactly once.
        /// Core-owned facts: sender death state. Standing loss and journal
        /// effects are raised as events for the host to apply.
        /// </summary>
        private void ApplyIgnoreConsequence(DistressRescueMission mission, int currentDay)
        {
            if (mission.IgnoreConsequenceApplied) return; // idempotent
            mission.IgnoreConsequenceApplied = true;
            mission.IgnoreConsequenceAppliedDay = currentDay;

            foreach (var token in mission.IgnoreConsequenceTokens)
            {
                if (token == "sender_death" && mission.SenderAlive)
                {
                    mission.SenderAlive = false;
                    if (mission.SenderDeathDay <= 0)
                        mission.SenderDeathDay = currentDay;
                }
            }

            string standingFaction = mission.IgnoreConsequenceTokens.Any(AffectsStanding)
                ? mission.FactionStandingLossFactionId
                : string.Empty;
            OnIgnoreConsequence?.Invoke(mission, new List<string>(mission.IgnoreConsequenceTokens), standingFaction, currentDay);
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
                    OutcomeSummary = m.OutcomeSummary,
                    // Rescue-signal runtime extension (additive)
                    SenderAlive = m.SenderAlive,
                    SenderDeathDay = m.SenderDeathDay,
                    SenderSurvivalDays = m.SenderSurvivalDays,
                    Expired = m.Expired,
                    IgnoreConsequenceApplied = m.IgnoreConsequenceApplied,
                    IgnoreConsequenceAppliedDay = m.IgnoreConsequenceAppliedDay,
                    IgnoreConsequenceTokens = new List<string>(m.IgnoreConsequenceTokens),
                    FactionStandingLossFactionId = m.FactionStandingLossFactionId,
                    ArrivalResolved = m.ArrivalResolved,
                    AuthenticityChecked = m.AuthenticityChecked,
                    AuthenticityAssessment = m.AuthenticityAssessment,
                    AssessmentThreatDetected = m.AssessmentThreatDetected,
                    AssessmentStalenessDetected = m.AssessmentStalenessDetected,
                    AssessmentSkillId = m.AssessmentSkillId
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
                        // Rescue-signal runtime extension (additive; legacy saves
                        // deserialize neutral defaults — never retroactively punished)
                        existing.SenderAlive = saved.SenderAlive;
                        existing.SenderDeathDay = saved.SenderDeathDay;
                        existing.SenderSurvivalDays = saved.SenderSurvivalDays;
                        existing.Expired = saved.Expired;
                        existing.IgnoreConsequenceApplied = saved.IgnoreConsequenceApplied;
                        existing.IgnoreConsequenceAppliedDay = saved.IgnoreConsequenceAppliedDay;
                        if (saved.IgnoreConsequenceTokens != null)
                            existing.IgnoreConsequenceTokens = new List<string>(saved.IgnoreConsequenceTokens);
                        existing.FactionStandingLossFactionId = saved.FactionStandingLossFactionId;
                        existing.ArrivalResolved = saved.ArrivalResolved;
                        existing.AuthenticityChecked = saved.AuthenticityChecked;
                        existing.AuthenticityAssessment = saved.AuthenticityAssessment;
                        existing.AssessmentThreatDetected = saved.AssessmentThreatDetected;
                        existing.AssessmentStalenessDetected = saved.AssessmentStalenessDetected;
                        existing.AssessmentSkillId = saved.AssessmentSkillId;
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
            void Add(string qId, string sigId, string rawDest, int trace, int deadline, bool isTrap, string faction, int rep, string[] consequences, string standingFaction, int survivalDays, params string[] items)
            {
                if (!AreConsequenceTokensValid(consequences))
                    throw new ArgumentException($"Unknown ignore-consequence token on {qId}.");
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
                    RewardItems = new List<string>(items),
                    IgnoreConsequenceTokens = new List<string>(consequences),
                    FactionStandingLossFactionId = standingFaction,
                    SenderSurvivalDays = survivalDays,
                    SenderAlive = true,
                    SenderDeathDay = 0
                };
                _missionsByQuest[qId] = m;
                _missionsBySignal[sigId] = m;
            }

            // 1. Trapped Mechanic — ignored call: the sender dies.
            Add("quest_distress_trapped_mechanic", "freq_distress_88_3", "loc_recovery_yard", 3, 5, false, "neutral", 5,
                new[] { "sender_death" }, "", 5, "scrap_metal", "mechanical_parts");

            // 2. Injured Trader — ignored call: the sender dies.
            Add("quest_distress_injured_trader", "freq_distress_156_8", "rural_gas_station", 2, 3, false, "neutral", 3,
                new[] { "sender_death" }, "", 3, "bandage", "battery", "iodine_pills");

            // 3. Family Shelter — no authored consequence: legacy expiry semantics.
            Add("quest_distress_family_shelter", "freq_distress_445_2", "family_bunker_backyard_shed", 2, 4, false, "neutral", 8,
                Array.Empty<string>(), "", 0, "clean_water", "canned_food");

            // 4. Raider Trap — no consequence: ignoring a lure is not a failure.
            Add("quest_distress_raider_trap", "freq_distress_192_4", "loc_denial_cut_substation", 3, 5, true, "raiders", 0,
                Array.Empty<string>(), "", 0);

            // 5. Military Patrol — ignored call: patrol lost + standing loss.
            Add("quest_distress_military_patrol", "freq_distress_901_2", "checkpoint_kilo_armory", 3, 5, false, "military", 10,
                new[] { "sender_death", "faction_standing_loss" }, "faction_civil_defense", 5, "ammo_556", "field_dressing_kit");

            // ── Expansion wave (plan §24 candidates): hostage, infected, convoy ──

            // 6. Verity Motel Hostage — ignored call: the hostage dies.
            Add("quest_distress_hostage_call", "freq_distress_726_5", "loc_motel_verity", 3, 4, false, "neutral", 8,
                new[] { "sender_death" }, "", 4, "canned_food", "clean_water");

            // 7. Fever Ward Warden — ignored call: the ward runs out of names.
            Add("quest_distress_infected_survivor", "freq_distress_609_4", "loc_st_brigids_almshouse", 4, 5, false, "neutral", 10,
                new[] { "sender_death" }, "", 5, "antibiotics", "clean_water");

            // 8. Salvage Crew Collapse — ignored call: the column does not hold.
            Add("quest_distress_convoy_sos", "freq_distress_455_7", "loc_warehouse_district", 2, 4, false, "neutral", 6,
                new[] { "sender_death" }, "", 4, "mechanical_parts", "battery");

            // ── Second content tranche (§24): ransom, false evacuation, beacon, crossing ──

            // 9. Ransom Demand — ignored call: the courier dies + the nomads count.
            Add("quest_distress_ransom_demand", "freq_distress_555_0", "loc_dentists_row", 3, 4, false, "river_nomads", 8,
                new[] { "sender_death", "faction_ambush" }, "faction_river_nomads", 4, "clean_water", "bandage");

            // 10. False Evacuation — trap-class: heeding it is the ambush; ignoring
            //     it is wise, so no ignore consequence (no moral choice either).
            Add("quest_distress_false_evacuation", "freq_distress_380_2", "collapsed_building", 3, 3, true, "raiders", 0,
                Array.Empty<string>(), "", 0);

            // 11. Command Post Echo — ignored call: the beacon runs alone.
            Add("quest_distress_military_beacon", "freq_distress_318_0", "loc_ordnance_shoulder", 4, 5, false, "military", 12,
                new[] { "sender_death" }, "", 5, "iodine_pills", "battery");

            // 12. Winter Crossing — ignored call: the ice road closes and keeps.
            Add("quest_distress_winter_crossing", "freq_distress_269_3", "loc_the_shallows_market", 3, 4, false, "neutral", 7,
                new[] { "sender_death" }, "", 4, "clean_water", "bandage");
        }
    }
}
