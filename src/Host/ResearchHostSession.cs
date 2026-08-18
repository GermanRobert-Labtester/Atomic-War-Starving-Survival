using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Research / R&D host session. Thin Godot wrapper
    /// around the engine-agnostic <see cref="ResearchSystem"/>.
    /// Captures / restores the unified <see cref="ResearchState"/>
    /// envelope. No gameplay rules here — hosts only present the
    /// engine's read surface to the dashboard.
    /// </summary>
    public sealed class ResearchHostSession
    {
        public ResearchSystem Engine { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public static ResearchHostSession Create()
        {
            return new ResearchHostSession();
        }

        private ResearchHostSession()
        {
            Engine = new ResearchSystem(log: new NullLog());
            Engine.RegisterDefaults();
        }

        public bool IsUnlocked => Engine.State.expansionUnlocked;
        public int CurrentDay => Engine.State.currentDay;
        public int CatalogCount => Engine.CatalogCount;
        public int CompletedCount => Engine.State.completedIds.Count;
        public int UnlockedCount => Engine.State.unlockedIds.Count;
        public string ActiveResearchId => Engine.State.activeResearchId ?? string.Empty;
        public int ActiveResearchDays => Engine.State.activeResearchDays;
        public IReadOnlyDictionary<string, ResearchKnowledgeDef> Catalog => Engine.Catalog;

        public ResearchKnowledgeDef GetActiveResearch() => Engine.GetActiveResearch();

        public void Unlock(int day)
        {
            if (Engine.State.expansionUnlocked) return;
            Engine.State.expansionUnlocked = true;
            Engine.State.currentDay = day;
            LastEvent = "Research unlocked @ day " + day;
            RaiseStateChanged();
        }

        public bool StartResearch(string id, int day)
        {
            bool ok = Engine.StartResearch(id, day);
            if (ok)
            {
                LastEvent = $"Research started: {id} @ day {day}";
                RaiseStateChanged();
            }
            return ok;
        }

        public void AdvanceDay(int day)
        {
            Engine.Tick(day);
            LastEvent = "Tick @ day " + day;
            RaiseStateChanged();
        }

        public bool CompleteResearch(string id)
        {
            bool ok = Engine.CompleteResearch(id);
            if (ok)
            {
                LastEvent = $"Research completed: {id}";
                RaiseStateChanged();
            }
            return ok;
        }

        public ResearchSave CaptureSave()
        {
            return new ResearchSave
            {
                systemId = ResearchSystem.SystemId,
                state = Engine.CaptureState(),
            };
        }

        public void RestoreSave(ResearchSave save)
        {
            if (save == null || save.state == null) return;
            Engine.RestoreState(save.state);
            LastEvent = "Research state restored.";
            RaiseStateChanged();
        }

        private void RaiseStateChanged()
        {
            try { StateChanged?.Invoke(); }
            catch { }
        }
    }

    [Serializable]
    public sealed class ResearchSave
    {
        public string systemId = ResearchSystem.SystemId;
        public ResearchState state = new ResearchState();
    }
}
