using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using System.Linq;

namespace Ashfall.Core.YearOfAsh
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  DATA CONTRACTS  (serializable via IJsonSerializer — no engine references)
    // ─────────────────────────────────────────────────────────────────────────────

    [Serializable]
    public enum QuestlineStatus
    {
        NotStarted,
        Active,
        Completed,
        Failed,
        Abandoned
    }

    [Serializable]
    public class QuestCondition
    {
        /// <summary>Arbitrary tag the host evaluates (e.g. "faction_rebuilders_standing>=30").</summary>
        public string conditionTag = string.Empty;
        public bool isBlocker = false;  // false = soft warning, true = hard gate
    }

    [Serializable]
    public class QuestChoice
    {
        public string choiceId   = string.Empty;
        public string text       = string.Empty;

        // Where this choice leads in the questline graph
        public string nextStageId = string.Empty;   // empty = quest ends

        // Rewards / penalties applied when this choice is taken
        public int moraleDelta   = 0;
        public int guiltDelta    = 0;
        public string grantItemId = string.Empty;    // empty = no item
        public int grantItemQuantity = 0;
        public string targetFactionId   = string.Empty;
        public int factionStandingDelta = 0;

        // Optional: unlocks a door-encounter later
        public string unlockEncounterId = string.Empty;

        public List<QuestCondition> conditions = new List<QuestCondition>();

        /// <summary>Flavour text shown after resolution. Tone: cold, restrained.</summary>
        public string outcomeNarrative = string.Empty;
    }

    [Serializable]
    public class QuestStage
    {
        public string stageId          = string.Empty;
        public string title            = string.Empty;
        public string narrativePrompt  = string.Empty;  // What the player sees
        public int    unlockOnDay      = 0;             // Day >= this to surface
        public bool   isTerminal       = false;         // true = no further choices
        public QuestlineStatus terminalOutcome = QuestlineStatus.Completed;

        public List<QuestChoice> choices = new List<QuestChoice>();
    }

    [Serializable]
    public class QuestlineDefinition
    {
        public string questlineId  = string.Empty;
        public string title        = string.Empty;
        public string synopsis     = string.Empty;
        public string factionTag   = string.Empty;     // primary faction context
        public string firstStageId = string.Empty;
        public int    minDay       = 180;
        public int    maxDay       = 360;

        public List<QuestStage> stages = new List<QuestStage>();

        public QuestStage? FindStage(string id)
        {
            foreach (var s in stages)
                if (s.stageId == id) return s;
            return null;
        }
    }

    [Serializable]
    public class ActiveQuestlineRecord
    {
        public string questlineId     = string.Empty;
        public string currentStageId  = string.Empty;
        public QuestlineStatus status = QuestlineStatus.Active;
        public List<string> choiceHistory = new List<string>();   // ordered choice IDs taken
        public int dayStarted = 0;
        public int dayResolved = -1;
    }

    [Serializable]
    public class QuestlineSystemState
    {
        public List<ActiveQuestlineRecord> active = new List<ActiveQuestlineRecord>();
        public List<string> completedQuestlineIds = new List<string>();
        public List<string> failedQuestlineIds    = new List<string>();
        public int totalMoraleDeltaFromQuests     = 0;
        public int totalGuiltDeltaFromQuests      = 0;
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  RESOLUTION RESULT
    // ─────────────────────────────────────────────────────────────────────────────

    public class QuestChoiceResult
    {
        public string questlineId    = string.Empty;
        public string stageId        = string.Empty;
        public string choiceId       = string.Empty;
        public string nextStageId    = string.Empty;
        public int    moraleDelta    = 0;
        public int    guiltDelta     = 0;
        public string grantItemId    = string.Empty;
        public int    grantItemQty   = 0;
        public string factionId      = string.Empty;
        public int    factionDelta   = 0;
        public string unlockedEncounterId = string.Empty;
        public string outcomeNarrative    = string.Empty;
        public QuestlineStatus newQuestStatus = QuestlineStatus.Active;
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  SYSTEM
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Engine-agnostic branching questline manager for Days 180–360.
    /// Maintains a directed graph of stages per questline; all state is serializable.
    /// Zero references to UnityEngine or Godot.
    /// </summary>
    public class QuestlineSystem
    {
        private readonly QuestlineSystemState _state;
        private readonly List<QuestlineDefinition> _catalog = new List<QuestlineDefinition>();

        public QuestlineSystemState State => _state;
        public IReadOnlyList<QuestlineDefinition> Catalog => _catalog;

        // Events — host layers subscribe to drive UI, audio, journal
        public event Action<QuestlineDefinition>  OnQuestlineStarted;
        public event Action<QuestChoiceResult>    OnQuestChoiceTaken;
        public event Action<string, QuestlineStatus> OnQuestlineResolved;

        public QuestlineSystem(QuestlineSystemState? state = null)
        {
            _state = state ?? new QuestlineSystemState();
            if (_state.active == null) _state.active = new List<ActiveQuestlineRecord>();
            if (_state.completedQuestlineIds == null) _state.completedQuestlineIds = new List<string>();
            if (_state.failedQuestlineIds == null) _state.failedQuestlineIds = new List<string>();
            PopulateBuiltInCatalog();
        }

        // ── Catalog management ──────────────────────────────────────────────────

        public void RegisterQuestline(QuestlineDefinition def)
        {
            if (def != null && !_catalog.Exists(q => q.questlineId == def.questlineId))
                _catalog.Add(def);
        }

        public QuestlineDefinition? FindDefinition(string questlineId)
        {
            foreach (var q in _catalog)
                if (q.questlineId == questlineId) return q;
            return null;
        }

        // ── Lifecycle ───────────────────────────────────────────────────────────

        /// <summary>
        /// Offer all questlines whose unlock window contains <paramref name="currentDay"/>
        /// and that have not yet been started or completed.
        /// </summary>
        public List<QuestlineDefinition> GetAvailableQuestlines(int currentDay)
        {
            var result = new List<QuestlineDefinition>();
            foreach (var def in _catalog)
            {
                if (currentDay < def.minDay || currentDay > def.maxDay) continue;
                bool alreadyDone =
                    _state.completedQuestlineIds.Contains(def.questlineId) ||
                    _state.failedQuestlineIds.Contains(def.questlineId) ||
                    _state.active.Exists(a => a.questlineId == def.questlineId);
                if (!alreadyDone)
                    result.Add(def);
            }
            return result;
        }

        /// <summary>
        /// True when a questline can actually be traversed: its first stage exists and
        /// offers at least one choice. A definition that fails this can be started but
        /// never advanced — <see cref="TakeChoice"/> finds no matching choice and returns
        /// null, stranding the record in <see cref="QuestlineStatus.Active"/> forever.
        /// The JSON catalog shape (stageIndex/objective/requiredItemId) carries no
        /// choices, so every questline loaded from it fails this until choices are
        /// authored. Hosts offer <see cref="GetPlayableQuestlines"/>, not the raw list.
        /// </summary>
        public bool IsPlayable(QuestlineDefinition def)
        {
            if (def == null) return false;
            var first = def.FindStage(def.firstStageId);
            return first != null && first.choices.Count > 0;
        }

        /// <summary>
        /// <see cref="GetAvailableQuestlines"/> minus the ones that cannot be advanced.
        /// This is what a host should offer the player.
        /// </summary>
        public List<QuestlineDefinition> GetPlayableQuestlines(int currentDay)
        {
            var result = new List<QuestlineDefinition>();
            foreach (var def in GetAvailableQuestlines(currentDay))
                if (IsPlayable(def)) result.Add(def);
            return result;
        }

        /// <summary>
        /// How many otherwise-available questlines were withheld for having no authored
        /// choices. Hosts surface this so the content gap stays visible instead of the
        /// catalog silently looking smaller than it is.
        /// </summary>
        public int WithheldQuestlineCount(int currentDay)
        {
            int withheld = 0;
            foreach (var def in GetAvailableQuestlines(currentDay))
                if (!IsPlayable(def)) withheld++;
            return withheld;
        }

        /// <summary>Starts a questline on <paramref name="day"/>. No-op if already active.</summary>
        public bool StartQuestline(string questlineId, int day)
        {
            var def = FindDefinition(questlineId);
            if (def == null) return false;
            if (_state.active.Exists(a => a.questlineId == questlineId)) return false;

            var record = new ActiveQuestlineRecord
            {
                questlineId    = questlineId,
                currentStageId = def.firstStageId,
                status         = QuestlineStatus.Active,
                dayStarted     = day
            };
            _state.active.Add(record);
            OnQuestlineStarted?.Invoke(def);
            return true;
        }

        /// <summary>
        /// Player picks a choice in the current stage of an active questline.
        /// Returns null if questline not found or choice invalid.
        /// </summary>
        public QuestChoiceResult? TakeChoice(string questlineId, string choiceId, int day)
        {
            var record = _state.active.Find(a => a.questlineId == questlineId);
            if (record == null || record.status != QuestlineStatus.Active) return null;

            var def   = FindDefinition(questlineId);
            if (def == null) return null;

            var stage = def.FindStage(record.currentStageId);
            if (stage == null) return null;

            QuestChoice? choice = null;
            foreach (var c in stage.choices)
                if (c.choiceId == choiceId) { choice = c; break; }
            if (choice == null) return null;

            // Build result
            var result = new QuestChoiceResult
            {
                questlineId  = questlineId,
                stageId      = stage.stageId,
                choiceId     = choiceId,
                nextStageId  = choice.nextStageId,
                moraleDelta  = choice.moraleDelta,
                guiltDelta   = choice.guiltDelta,
                grantItemId  = choice.grantItemId,
                grantItemQty = choice.grantItemQuantity,
                factionId    = choice.targetFactionId,
                factionDelta = choice.factionStandingDelta,
                unlockedEncounterId = choice.unlockEncounterId,
                outcomeNarrative    = choice.outcomeNarrative
            };

            // Persist history
            record.choiceHistory.Add(choiceId);
            _state.totalMoraleDeltaFromQuests += choice.moraleDelta;
            _state.totalGuiltDeltaFromQuests  += choice.guiltDelta;

            // Advance to next stage or terminal
            if (string.IsNullOrEmpty(choice.nextStageId))
            {
                // Questline ends — derive outcome from current stage terminal flag
                var outcome = stage.isTerminal ? stage.terminalOutcome : QuestlineStatus.Completed;
                result.newQuestStatus = outcome;
                FinalizeQuestline(record, outcome, day);
            }
            else
            {
                var nextStage = def.FindStage(choice.nextStageId);
                if (nextStage == null || nextStage.isTerminal)
                {
                    var outcome = nextStage?.terminalOutcome ?? QuestlineStatus.Completed;
                    record.currentStageId = choice.nextStageId;
                    result.newQuestStatus = outcome;
                    FinalizeQuestline(record, outcome, day);
                }
                else
                {
                    record.currentStageId = choice.nextStageId;
                    result.newQuestStatus = QuestlineStatus.Active;
                }
            }

            OnQuestChoiceTaken?.Invoke(result);
            return result;
        }

        public ActiveQuestlineRecord? GetActiveRecord(string questlineId)
        {
            return _state.active.Find(a => a.questlineId == questlineId);
        }

        // ── State capture ───────────────────────────────────────────────────────

        public QuestlineSystemState CaptureState()
        {
            var copy = new QuestlineSystemState
            {
                totalMoraleDeltaFromQuests = _state.totalMoraleDeltaFromQuests,
                totalGuiltDeltaFromQuests  = _state.totalGuiltDeltaFromQuests,
                completedQuestlineIds = new List<string>(_state.completedQuestlineIds),
                failedQuestlineIds    = new List<string>(_state.failedQuestlineIds),
                active = new List<ActiveQuestlineRecord>()
            };
            foreach (var r in _state.active)
            {
                copy.active.Add(new ActiveQuestlineRecord
                {
                    questlineId    = r.questlineId,
                    currentStageId = r.currentStageId,
                    status         = r.status,
                    dayStarted     = r.dayStarted,
                    dayResolved    = r.dayResolved,
                    choiceHistory  = new List<string>(r.choiceHistory)
                });
            }
            return copy;
        }

        /// <summary>
        /// Rebuilds live questline progress from a snapshot. Deep-copies like its
        /// siblings so the restored system never aliases the save object, and
        /// tolerates a null section (a save written before quests were persisted).
        /// </summary>
        public void RestoreState(QuestlineSystemState state)
        {
            if (state == null) return;

            _state.totalMoraleDeltaFromQuests = state.totalMoraleDeltaFromQuests;
            _state.totalGuiltDeltaFromQuests  = state.totalGuiltDeltaFromQuests;

            _state.completedQuestlineIds.Clear();
            if (state.completedQuestlineIds != null)
                _state.completedQuestlineIds.AddRange(state.completedQuestlineIds);

            _state.failedQuestlineIds.Clear();
            if (state.failedQuestlineIds != null)
                _state.failedQuestlineIds.AddRange(state.failedQuestlineIds);

            _state.active.Clear();
            if (state.active != null)
            {
                foreach (var r in state.active)
                {
                    if (r == null || string.IsNullOrEmpty(r.questlineId)) continue;
                    _state.active.Add(new ActiveQuestlineRecord
                    {
                        questlineId    = r.questlineId,
                        currentStageId = r.currentStageId,
                        status         = r.status,
                        dayStarted     = r.dayStarted,
                        dayResolved    = r.dayResolved,
                        choiceHistory  = r.choiceHistory != null
                            ? new List<string>(r.choiceHistory)
                            : new List<string>()
                    });
                }
            }
        }

        // ── Private ─────────────────────────────────────────────────────────────

        private void FinalizeQuestline(ActiveQuestlineRecord record, QuestlineStatus outcome, int day)
        {
            record.status      = outcome;
            record.dayResolved = day;

            if (outcome == QuestlineStatus.Completed)
                _state.completedQuestlineIds.Add(record.questlineId);
            else if (outcome == QuestlineStatus.Failed)
                _state.failedQuestlineIds.Add(record.questlineId);

            OnQuestlineResolved?.Invoke(record.questlineId, outcome);
        }

        /// <summary>
        /// Built-in questlines covering all 5 factions across Days 180–360.
        /// Narrative tone: cold, exhausted, human. No magic, no real-world nations.
        /// </summary>
        private void PopulateBuiltInCatalog()
        {
            foreach (var q in BuiltInQuestlineCatalog.CreateAll())
            {
                RegisterQuestline(q);
            }
        }
    }
}
