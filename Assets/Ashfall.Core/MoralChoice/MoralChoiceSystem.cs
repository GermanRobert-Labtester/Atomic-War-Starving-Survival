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
    /// one-time threshold events, and ending selection. The score is never
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
        private MoralChoiceState _state = new MoralChoiceState();

        public event Action<MoralChoiceResolution>? OnQuestResolved;
        public event Action<string>? OnThresholdEventFired;

        public MoralChoiceSystem(ISeededRng rng, ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public MoralChoiceState State => _state;
        public int MoralScore => _state.moralScore;
        public int EmpathyPoints => _state.empathyPoints;
        public int QuestsResolved => _state.resolutions.Count;
        public MoralPathBand CurrentBand => BandForScore(_state.moralScore);
        public IReadOnlyList<MoralChoiceResolution> Resolutions => _state.resolutions;

        public bool IsListener => _state.empathyPoints >= ListenerEmpathyThreshold;
        public bool IsConfidant => _state.empathyPoints >= ConfidantEmpathyThreshold;

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

            return resolution;
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
                resolutions = new List<MoralChoiceResolution>()
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
