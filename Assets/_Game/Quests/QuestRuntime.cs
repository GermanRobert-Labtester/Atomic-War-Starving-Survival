using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Quests
{
    /// <summary>
    /// Lightweight runtime for the new 7 quests (faction/personal/shelter).
    /// Each quest has a small state machine, stage progress, and three
    /// terminal outcomes (success / failure / locked). Quest definitions
    /// live as data (QuestDef) and are executed by concrete quest
    /// classes that consume the existing systems (NeedsSystem, Faction,
    /// Expedition, Medical, PersonalQuestSystem, etc.) via injected
    /// callbacks — the same no-hard-refs pattern used by the new
    /// systems above.
    ///
    /// This is intentionally a sibling of the long-standing
    /// PersonalQuestSystem / LocationQuestSystem: those own the
    /// archetype-driven "latent expert trait" track; this one owns the
    /// morally-weighted narrative arc track. Both save/load safe.
    /// </summary>
    public enum QuestStatus { NotStarted, InProgress, Success, Failure, Locked }

    [Serializable]
    public class QuestDef
    {
        public string Id;            // e.g. "quest_garrison_last_order"
        public string DisplayName;
        [TextArea(2, 4)] public string Description;
        public string FactionId;     // nullable: "" for personal/shelter
        public int MaxStages = 4;
        public bool Personal;        // one survivor owns the quest
        public string OwnerSurvivorId;
    }

    [Serializable]
    public class QuestRuntimeState
    {
        public string QuestId;
        public int Stage;            // 0 = not started, 1..MaxStages = active
        public QuestStatus Status = QuestStatus.NotStarted;
        public int StartedOnDay;
        public int CompletedOnDay;
        // Free-form progress bag, e.g. { cipherFragments: 3 } or { treatments: 4 }.
        public List<QuestProgressEntry> Progress = new List<QuestProgressEntry>();
    }

    [Serializable]
    public class QuestProgressEntry
    {
        public string Key;
        public float Value;
        public QuestProgressEntry() {}
        public QuestProgressEntry(string key, float value) { Key = key; Value = value; }
    }

    public abstract class QuestRuntime
    {
        public QuestDef Def;
        public QuestRuntimeState State;
        public event Action<QuestRuntimeState, int> OnStageChanged;       // state, newStage
        public event Action<QuestRuntimeState, QuestStatus> OnStatusChanged;
        public event Action<QuestRuntimeState, string> OnChoiceOffered;   // choiceId

        // Host callbacks the quest uses to talk to the world.
        public Func<float> GetDay;
        public Func<string, int> GetInt;            // arbitrary int getter
        public Action<string, float> AddFactionTrust;             // factionId, reason, delta
        public Action<string, float> SubtractFactionTrust;
        public Action<string, string, float> BroadcastRadioMessage;       // faction, msg, confidence
        public Action<string, float> ApplyMorale;
        public Action<string, float> ApplyRadiationDose;
        public Action<string, string> AddAffliction;
        public Action<string, string> RemoveAffliction;
        public Action<string, string, int> GrantPerk;
        public Action<string, string, int> TakeItem;
        public Action<string, string, int> GiveItem;
        public Action<string, string> MarkLocationDestroyed;              // locId, kind
        public Action<string> KillSurvivor;
        public Action<string> CaptureSurvivor;
        public Action<string> ReleaseSurvivor;
        public Action<string> RecordMoralEntry;                   // day, text
        public Action<string, int> TriggerRaidSoon;                       // faction, hours

        public void Start(int currentDay)
        {
            State = State ?? new QuestRuntimeState { QuestId = Def?.Id };
            State.Stage = 1;
            State.Status = QuestStatus.InProgress;
            State.StartedOnDay = currentDay;
            OnStageChanged?.Invoke(State, 1);
            OnStatusChanged?.Invoke(State, QuestStatus.InProgress);
            OnBegin();
        }

        public void Advance()
        {
            if (State == null) return;
            State.Stage++;
            OnStageChanged?.Invoke(State, State.Stage);
            if (State.Stage >= Def.MaxStages)
            {
                Complete();
            }
            else
            {
                OnStageEnter(State.Stage);
            }
        }

        public void Complete()
        {
            State.Status = QuestStatus.Success;
            State.CompletedOnDay = Mathf.FloorToInt(GetDay?.Invoke() ?? 0);
            OnStatusChanged?.Invoke(State, QuestStatus.Success);
            OnSuccess();
        }

        public void Fail()
        {
            State.Status = QuestStatus.Failure;
            OnStatusChanged?.Invoke(State, QuestStatus.Failure);
            OnFailure();
        }

        public void Lock()
        {
            State.Status = QuestStatus.Locked;
            OnStatusChanged?.Invoke(State, QuestStatus.Locked);
        }

        public void SetProgress(string key, float value)
        {
            if (State == null) return;
            for (int i = 0; i < State.Progress.Count; i++)
            {
                if (State.Progress[i].Key == key)
                {
                    State.Progress[i].Value = value;
                    return;
                }
            }
            State.Progress.Add(new QuestProgressEntry(key, value));
        }

        public float GetProgress(string key, float fallback = 0f)
        {
            if (State == null) return fallback;
            for (int i = 0; i < State.Progress.Count; i++)
                if (State.Progress[i].Key == key) return State.Progress[i].Value;
            return fallback;
        }

        protected abstract void OnBegin();
        protected abstract void OnStageEnter(int stage);
        protected abstract void OnSuccess();
        protected abstract void OnFailure();

        // Host-agnostic helpers subclasses can call.
        protected void OfferChoice(string choiceId) => OnChoiceOffered?.Invoke(State, choiceId);
    }
}
