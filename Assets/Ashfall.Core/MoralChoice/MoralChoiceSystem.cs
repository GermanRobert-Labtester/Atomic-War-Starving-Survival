using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    public enum MoralPathBand
    {
        VeryEvil,
        Evil,
        SlightlyEvil,
        Neutral,
        SlightlyPositive,
        Positive,
        VeryPositive
    }

    public enum MoralEndingKind
    {
        Warlord,
        SurvivorKing,
        NeutralSurvivor,
        BalancedSurvivor,
        CommunityBuilder,
        Savior,
        SaintOfWasteland,
        Storykeeper
    }

    /// <summary>
    /// In-code quest shape. Phase 2 loads these from
    /// Assets/StreamingAssets/Data/moral_choice_quests.json; ids must use the
    /// canonical quest_moral_ prefix (the design doc drafts them as
    /// qst_moral_* — that spelling is rejected on purpose).
    /// </summary>
    public sealed class MoralChoiceQuestDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>share | listen | comfort | dead | trust</summary>
        public string Category { get; set; } = string.Empty;

        public string Trigger { get; set; } = string.Empty;

        /// <summary>Encounter prose shown when the quest is discovered.</summary>
        public string Discovery { get; set; } = string.Empty;

        public string LocationId { get; set; } = string.Empty;

        /// <summary>First day the quest may be offered; 0 = always.</summary>
        public int MinDay { get; set; }

        /// <summary>Last day the quest may be offered; 0 or negative = unbounded.</summary>
        public int MaxDay { get; set; }

        public List<MoralChoiceOption> Choices { get; set; } = new List<MoralChoiceOption>();
    }

    public sealed class MoralChoiceOption
    {
        /// <summary>UI text for the choice, e.g. "Give all your food".</summary>
        public string Label { get; set; } = string.Empty;

        public int MoralDelta { get; set; }
        public int EmpathyDelta { get; set; }
        public string OutcomeText { get; set; } = string.Empty;
        public string Epitaph { get; set; } = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic moral choice ledger: invisible moral + empathy
    /// accumulators, band computation, overnight reconciliation with
    /// one-time threshold events, ending selection, branch tracking,
    /// quest gating, and echo quest availability. The score is never
    /// surfaced to the player — the world is the UI (host layers read
    /// CurrentBand / events, never the raw number).
    /// </summary>
    public sealed class MoralChoiceSystem
    {
        public const string SystemId = "moral_choice";
        public const string QuestIdPrefix = "quest_moral_";

        public const int MinScore = -200;
        public const int MaxScore = 200;

        public const int ListenerEmpathyThreshold = 15;
        public const int ConfidantEmpathyThreshold = 30;
        public const int StorykeeperEmpathyThreshold = 45;
        public const int StorykeeperQuestThreshold = 25;

        /// <summary>Below this many resolved quests no ending band locks; mild endings fire instead.</summary>
        public const int EndingLockMinQuests = 20;

        public const string EventLegendPositive = "moral_event_legend_positive";
        public const string EventLegendNegative = "moral_event_legend_negative";
        public const string EventBountyIssued = "moral_event_bounty_issued";
        public const string EventContractTaken = "moral_event_contract_taken";
        public const string EventContractRaised = "moral_event_contract_raised";
        public const string EventPatrolDefense = "moral_event_patrol_defense";

        /// <summary>Pending-overflow bits settled at the next Reconcile (bit 1 = positive, bit 2 = negative).</summary>
        public const int LegendPositiveFlag = 1;
        public const int LegendNegativeFlag = 2;

        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Flags.IFlagLedger? _flags;
        private MoralChoiceState _state = new MoralChoiceState();

        /// <summary>Branch architecture from moral_choice_chains.json; null until InitializeChainData.</summary>
        private MoralChoiceChainData? _chainData;
        private Dictionary<string, string> _questToBranch = new Dictionary<string, string>();
        private HashSet<string> _entryQuestSet = new HashSet<string>();

        public event Action<MoralChoiceResolution>? OnQuestResolved;
        public event Action<string>? OnThresholdEventFired;
        public event Action<string>? OnBranchLocked;

        public MoralChoiceSystem(ISeededRng rng, ILog? log = null, Flags.IFlagLedger? flags = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _flags = flags;
        }

        public MoralChoiceState State => _state;
        public int MoralScore => _state.moralScore;
        public int EmpathyPoints => _state.empathyPoints;
        public int QuestsResolved => _state.resolutions.Count;
        public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
        public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;

        public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
        public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;

        /// <summary>Chain data reference; null if not yet initialized.</summary>
        public MoralChoiceChainData? ChainData => _chainData;

        /// <summary>
        /// Load the branching architecture (moral_choice_chains.json). Builds
        /// internal lookup maps for branch ownership and entry-quest tracking.
        /// Safe to call once at startup; subsequent calls are no-ops.
        /// </summary>
        public void InitializeChainData(MoralChoiceChainData chainData)
        {
            if (chainData == null || _chainData != null) return;
            _chainData = chainData;

            foreach (var branch in chainData.Branches)
            {
                foreach (var entryQuest in branch.EntryQuests)
                {
                    _questToBranch[entryQuest] = branch.Id;
                    _entryQuestSet.Add(entryQuest);
                }
            }
            foreach (var gate in chainData.QuestGates)
            {
                if (!string.IsNullOrEmpty(gate.Branch) && !string.IsNullOrEmpty(gate.QuestId))
                {
                    _questToBranch[gate.QuestId] = gate.Branch;
                }
            }
        }

        public static bool IsCanonicalQuestId(string questId) =>
            questId.StartsWith(QuestIdPrefix, StringComparison.Ordinal);

        /// <summary>MaxDay &lt;= 0 means unbounded; a malformed window (max &lt; min) is never available.</summary>
        public static bool IsAvailableOnDay(MoralChoiceQuestDefinition quest, int day) =>
            day >= quest.MinDay && (quest.MaxDay <= 0 || (day <= quest.MaxDay && quest.MaxDay >= quest.MinDay));

        public bool IsResolved(string questId) => TryGetResolution(questId, out _);

        public bool TryGetResolution(string questId, out MoralChoiceResolution? resolution)
        {
            resolution = _state.resolutions.FirstOrDefault(r => string.Equals(r.questId, questId, StringComparison.Ordinal));
            return resolution != null;
        }

        // ── Branch tracking ────────────────────────────────────────────

        /// <summary>Whether a branch has been permanently locked by the lockout mechanic.</summary>
        public bool IsBranchLocked(string branchId) =>
            _state.lockedBranches.Contains(branchId);

        /// <summary>How many entry quests the player has resolved for a branch.</summary>
        public int GetBranchProgress(string branchId) =>
            _state.branchProgress.TryGetValue(branchId, out int v) ? v : 0;

        /// <summary>Which branch owns a quest (by chain data); empty string if not a chain quest.</summary>
        public string GetQuestBranch(string questId) =>
            _questToBranch.TryGetValue(questId, out var b) ? b : string.Empty;

        /// <summary>
        /// Whether a chain quest is accessible: branch not locked, gate
        /// prerequisites met, day window valid, and not already resolved.
        /// Non-chain quests (base/expansion) only check day + resolved.
        /// </summary>
        public bool IsChainQuestAccessible(string questId, int day)
        {
            if (IsResolved(questId)) return false;

            if (_questToBranch.TryGetValue(questId, out var branchId))
            {
                if (IsBranchLocked(branchId)) return false;
            }

            var gate = _chainData?.QuestGates.FirstOrDefault(
                g => string.Equals(g.QuestId, questId, StringComparison.Ordinal));
            if (gate != null && !EvaluateGate(gate)) return false;

            return true;
        }

        /// <summary>
        /// Evaluate a quest gate's prerequisites: prior quests resolved,
        /// moral/empathy thresholds, and flag requirements.
        /// </summary>
        public bool EvaluateGate(MoralQuestGate gate)
        {
            if (gate == null) return true;

            foreach (var req in gate.Requires)
            {
                if (!IsResolved(req)) return false;
            }

            if (gate.RequiresMinMoral.HasValue && _state.moralScore < gate.RequiresMinMoral.Value) return false;
            if (gate.RequiresMaxMoral.HasValue && _state.moralScore > gate.RequiresMaxMoral.Value) return false;
            if (gate.RequiresMinEmpathy.HasValue && _state.empathyPoints < gate.RequiresMinEmpathy.Value) return false;

            if (!string.IsNullOrEmpty(gate.RequiresFlag) && !_state.activeFlags.Contains(gate.RequiresFlag)) return false;

            return true;
        }

        /// <summary>Set a moral flag (idempotent).</summary>
        public void SetFlag(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            if (!_state.activeFlags.Contains(flagId))
                _state.activeFlags.Add(flagId);
            _flags?.Set(flagId, "moral_choice");
        }

        /// <summary>Whether a moral flag is currently set.</summary>
        public bool HasFlag(string flagId) =>
            !string.IsNullOrEmpty(flagId) && (_flags != null ? (_flags.IsSet(flagId) || _state.activeFlags.Contains(flagId)) : _state.activeFlags.Contains(flagId));

        // ── Echo quests ────────────────────────────────────────────────

        /// <summary>
        /// Find echo quests that should fire given the current state and day.
        /// An echo quest fires when: its trigger quest was resolved with the
        /// matching choice, enough days have passed, it hasn't fired yet, and
        /// its branch (if any) is not locked.
        /// </summary>
        public List<MoralEchoQuestDefinition> FindAvailableEchoQuests(int currentDay)
        {
            if (_chainData == null) return new List<MoralEchoQuestDefinition>();

            var result = new List<MoralEchoQuestDefinition>();
            foreach (var echo in _chainData.EchoQuests)
            {
                if (_state.firedEchoQuests.Contains(echo.QuestId)) continue;
                if (!TryGetResolution(echo.TriggeredBy, out var trigger)) continue;
                if (trigger!.choiceIndex != echo.TriggeredByChoice) continue;
                if (currentDay < trigger.resolvedDay + echo.MinDaysAfter) continue;

                if (!string.IsNullOrEmpty(echo.Branch) && IsBranchLocked(echo.Branch)) continue;

                result.Add(echo);
            }
            return result;
        }

        /// <summary>Mark an echo quest as fired (called by the host when the echo is presented).</summary>
        public void MarkEchoQuestFired(string echoQuestId)
        {
            if (string.IsNullOrEmpty(echoQuestId)) return;
            if (!_state.firedEchoQuests.Contains(echoQuestId))
                _state.firedEchoQuests.Add(echoQuestId);
        }

        // ── Quest resolution ───────────────────────────────────────────

        /// <summary>
        /// Resolve a quest choice. One resolution per quest per save: repeat
        /// calls return the stored resolution without re-applying deltas or
        /// re-rolling. Band-crossing consequences never land here — they
        /// settle overnight in Reconcile.
        /// </summary>
        public MoralChoiceResolution Resolve(MoralChoiceQuestDefinition quest, int choiceIndex, string locationId, int day)
        {
            if (quest == null) throw new ArgumentNullException(nameof(quest));
            if (!IsCanonicalQuestId(quest.Id))
            {
                throw new ArgumentException(
                    $"Moral quest id '{quest.Id}' must use the canonical '{QuestIdPrefix}' prefix " +
                    "(the design doc drafts ids as 'qst_moral_*'; register them as 'quest_moral_*').",
                    nameof(quest));
            }
            if (choiceIndex < 0 || choiceIndex >= quest.Choices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(choiceIndex),
                    $"Choice index {choiceIndex} is outside 0..{quest.Choices.Count - 1} for '{quest.Id}'.");
            }
            if (day < 0) throw new ArgumentOutOfRangeException(nameof(day));

            if (TryGetResolution(quest.Id, out var existing))
            {
                _log.Warn($"Moral quest '{quest.Id}' already resolved on day {existing!.resolvedDay}; replaying stored outcome.");
                return existing;
            }

            var choice = quest.Choices[choiceIndex];
            int unclamped = _state.moralScore + choice.MoralDelta;
            int clamped = Math.Clamp(unclamped, MinScore, MaxScore);
            _state.moralScore = clamped;
            _state.empathyPoints += choice.EmpathyDelta;

            var resolution = new MoralChoiceResolution
            {
                questId = quest.Id,
                locationId = locationId ?? string.Empty,
                resolvedDay = day,
                choiceIndex = choiceIndex,
                moralDelta = choice.MoralDelta,
                empathyDelta = choice.EmpathyDelta,
                impactMark = MarkFor(choice.MoralDelta),
                outcomeRoll = _rng.Next(0, 100),
                propagatesOnDay = day + 1 + _rng.Next(0, 3),
                epitaph = choice.Epitaph
            };
            _state.resolutions.Add(resolution);
            OnQuestResolved?.Invoke(resolution);

            // Overflow never lands mid-scene: flag it, settle it overnight.
            if (unclamped > MaxScore) _state.pendingLegendFlags |= LegendPositiveFlag;
            else if (unclamped < MinScore) _state.pendingLegendFlags |= LegendNegativeFlag;

            // Track branch progress for entry quests and check lockout.
            TrackBranchProgress(quest.Id);

            return resolution;
        }

        /// <summary>
        /// If the resolved quest is a branch entry quest, increment that
        /// branch's progress. When the lock threshold is reached, lock out
        /// the opposing branches and set the branch-locked flags.
        /// </summary>
        private void TrackBranchProgress(string questId)
        {
            if (!_entryQuestSet.Contains(questId)) return;
            if (!_questToBranch.TryGetValue(questId, out var branchId)) return;
            if (_chainData == null) return;

            var branch = _chainData.Branches.FirstOrDefault(
                b => string.Equals(b.Id, branchId, StringComparison.Ordinal));
            if (branch == null) return;

            if (!_state.branchProgress.ContainsKey(branchId))
                _state.branchProgress[branchId] = 0;
            _state.branchProgress[branchId]++;

            if (_state.branchProgress[branchId] >= branch.LockThreshold)
            {
                foreach (var lockedId in branch.LocksOut)
                {
                    if (!_state.lockedBranches.Contains(lockedId))
                    {
                        _state.lockedBranches.Add(lockedId);

                        var lockedBranch = _chainData.Branches.FirstOrDefault(
                            b => string.Equals(b.Id, lockedId, StringComparison.Ordinal));
                        if (lockedBranch != null && !string.IsNullOrEmpty(lockedBranch.LockedFlag))
                        {
                            SetFlag(lockedBranch.LockedFlag);
                        }

                        OnBranchLocked?.Invoke(lockedId);
                    }
                }
            }
        }

        /// <summary>
        /// Overnight settlement: pending legend overflow, then band crossings
        /// and their one-time faction events, so an act's consequences always
        /// land overnight, never mid-scene. Every band crossed between the
        /// last reconcile and now settles its event (dedup keeps each
        /// one-time). Out-of-order days are ignored. A never-reconciled save
        /// starts from the Neutral band.
        /// </summary>
        public void Reconcile(int day)
        {
            if (day < _state.lastReconciledDay) return;
            _state.lastReconciledDay = day;

            if ((_state.pendingLegendFlags & LegendPositiveFlag) != 0) FireThresholdEvent(EventLegendPositive);
            if ((_state.pendingLegendFlags & LegendNegativeFlag) != 0) FireThresholdEvent(EventLegendNegative);
            _state.pendingLegendFlags = 0;

            int from = _state.bandAtLastReconcile < 0 ? (int)MoralPathBand.Neutral : _state.bandAtLastReconcile;
            int to = (int)CurrentBand;
            if (to == from) return;

            int step = to > from ? 1 : -1;
            for (int band = from + step; ; band += step)
            {
                FireBandEvents((MoralPathBand)band);
                if (band == to) break;
            }
            _state.bandAtLastReconcile = to;
        }

        private void FireBandEvents(MoralPathBand band)
        {
            switch (band)
            {
                case MoralPathBand.VeryEvil:
                    FireThresholdEvent(EventBountyIssued);
                    break;
                case MoralPathBand.Positive:
                    FireThresholdEvent(EventContractTaken);
                    break;
                case MoralPathBand.VeryPositive:
                    FireThresholdEvent(EventContractRaised);
                    FireThresholdEvent(EventPatrolDefense);
                    break;
            }
        }

        public MoralEndingKind SelectEnding() =>
            SelectEnding(_state.moralScore, _state.empathyPoints, _state.resolutions.Count);

        /// <summary>
        /// Priority: Storykeeper threshold overrides band; below the quest
        /// lock the mild endings fire (the band has not earned the right to
        /// define the run yet); otherwise band decides.
        /// </summary>
        public static MoralEndingKind SelectEnding(int moralScore, int empathyPoints, int questsResolved)
        {
            if (empathyPoints >= StorykeeperEmpathyThreshold && questsResolved >= StorykeeperQuestThreshold)
            {
                return MoralEndingKind.Storykeeper;
            }

            if (questsResolved < EndingLockMinQuests)
            {
                return moralScore < 0 ? MoralEndingKind.NeutralSurvivor
                    : moralScore == 0 ? MoralEndingKind.BalancedSurvivor
                    : MoralEndingKind.CommunityBuilder;
            }

            return BandForScore(moralScore) switch
            {
                MoralPathBand.VeryEvil => MoralEndingKind.Warlord,
                MoralPathBand.Evil => MoralEndingKind.SurvivorKing,
                MoralPathBand.SlightlyEvil => MoralEndingKind.NeutralSurvivor,
                MoralPathBand.Neutral => MoralEndingKind.BalancedSurvivor,
                MoralPathBand.SlightlyPositive => MoralEndingKind.CommunityBuilder,
                MoralPathBand.Positive => MoralEndingKind.Savior,
                _ => MoralEndingKind.SaintOfWasteland
            };
        }

        public static MoralPathBand BandForScore(int score)
        {
            score = Math.Clamp(score, MinScore, MaxScore);
            if (score <= -100) return MoralPathBand.VeryEvil;
            if (score <= -50) return MoralPathBand.Evil;
            if (score < 0) return MoralPathBand.SlightlyEvil;
            if (score == 0) return MoralPathBand.Neutral;
            if (score < 50) return MoralPathBand.SlightlyPositive;
            if (score < 100) return MoralPathBand.Positive;
            return MoralPathBand.VeryPositive;
        }

        public MoralChoiceState CaptureState() => Clone(_state);

        public void RestoreState(MoralChoiceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (!string.Equals(state.systemId, SystemId, StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"State belongs to system '{state.systemId}', expected '{SystemId}'.", nameof(state));
            }
            if (state.schemaVersion > 1)
            {
                throw new NotSupportedException(
                    $"Future moral choice save schema {state.schemaVersion}; supported schema is 1.");
            }
            if (state.schemaVersion < 1)
            {
                throw new ArgumentException("Moral choice save is missing a valid schemaVersion.", nameof(state));
            }
            _state = Clone(state);
            if (_flags != null && _state.activeFlags != null)
            {
                foreach (var f in _state.activeFlags)
                    _flags.Set(f, "moral_choice");
            }
        }

        private void FireThresholdEvent(string eventId)
        {
            if (_state.firedThresholdEvents.Contains(eventId)) return;
            _state.firedThresholdEvents.Add(eventId);
            OnThresholdEventFired?.Invoke(eventId);
        }

        private static string MarkFor(int moralDelta) =>
            moralDelta > 0 ? "up" : moralDelta < 0 ? "down" : "flat";

        /// <summary>Deep copy so captured/restored states never alias the live ledger.</summary>
        private static MoralChoiceState Clone(MoralChoiceState source)
        {
            var copy = new MoralChoiceState
            {
                systemId = source.systemId,
                schemaVersion = source.schemaVersion,
                moralScore = source.moralScore,
                empathyPoints = source.empathyPoints,
                lastReconciledDay = source.lastReconciledDay,
                bandAtLastReconcile = source.bandAtLastReconcile,
                pendingLegendFlags = source.pendingLegendFlags,
                firedThresholdEvents = new List<string>(source.firedThresholdEvents ?? new List<string>()),
                resolutions = new List<MoralChoiceResolution>(),
                branchProgress = new Dictionary<string, int>(source.branchProgress ?? new Dictionary<string, int>()),
                lockedBranches = new List<string>(source.lockedBranches ?? new List<string>()),
                firedEchoQuests = new List<string>(source.firedEchoQuests ?? new List<string>()),
                activeFlags = new List<string>(source.activeFlags ?? new List<string>())
            };
            if (source.resolutions != null)
            {
                foreach (var r in source.resolutions)
                {
                    copy.resolutions.Add(new MoralChoiceResolution
                    {
                        questId = r.questId,
                        locationId = r.locationId,
                        resolvedDay = r.resolvedDay,
                        choiceIndex = r.choiceIndex,
                        moralDelta = r.moralDelta,
                        empathyDelta = r.empathyDelta,
                        impactMark = r.impactMark,
                        outcomeRoll = r.outcomeRoll,
                        propagatesOnDay = r.propagatesOnDay,
                        epitaph = r.epitaph
                    });
                }
            }
            return copy;
        }
    }
}
