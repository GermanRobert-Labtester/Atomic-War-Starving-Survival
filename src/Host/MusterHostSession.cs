using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Muster (Expansion 06) escalation layer.
    /// Wraps MusterSystem, loads the 15-current roster from currents.json,
    /// escalates the day clock, and persists to user:// via MusterSaveStore.
    /// No gameplay rules here — hosts only present.
    /// </summary>
    public sealed class MusterHostSession
    {
        public MusterSystem Engine { get; }
        public List<CurrentDefinition> Roster { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public MusterHostSession(MusterSystem engine = null, List<CurrentDefinition> roster = null)
        {
            Engine = engine ?? new MusterSystem();
            Roster = roster ?? new List<CurrentDefinition>();
            Engine.OnQuestlineResolved += record =>
            {
                LastEvent = $"Resolved {record.questlineId} via approach {record.selectedApproach} → {record.endingKey}";
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
        }

        public static MusterHostSession Create(string dataDir)
        {
            var roster = new List<CurrentDefinition>();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                roster = CurrentsCatalogLoader.LoadCurrents(dataDir, fileIO, serializer);
            }

            var session = new MusterHostSession(roster: roster);
            var save = MusterSaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Muster state restored from save.";
            }
            return session;
        }

        // ── Day escalation ─────────────────────────────────────────────

        /// <summary>Feed the sector clock. The Muster triggers at Day 260+.</summary>
        public string Escalate(int day)
        {
            Engine.SetEscalationDay(day);
            LastEvent = Engine.MusterTriggered
                ? $"Day {day}: the Muster is open."
                : $"Day {day}: escalation tracked (Muster opens Day {MusterSystem.MusterOpeningDay}).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Approach selection ─────────────────────────────────────────

        public string SelectApproach(string questlineId, QuestApproach approach)
        {
            bool ok = Engine.SelectApproachFor(questlineId, approach);
            LastEvent = ok
                ? $"Approach {approach} selected for {questlineId}."
                : $"Rejected: {questlineId} does not offer {approach} or is resolved.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MusterState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(MusterState state) => Engine.RestoreState(state);
    }
}
