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
        public CoalitionCampSystem Camp { get; }
        public List<CurrentDefinition> Roster { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public MusterHostSession(
            MusterSystem engine = null,
            CoalitionCampSystem camp = null,
            List<CurrentDefinition> roster = null)
        {
            Engine = engine ?? new MusterSystem();
            Camp = camp ?? new CoalitionCampSystem();
            Roster = roster ?? new List<CurrentDefinition>();
            Engine.OnQuestlineResolved += record =>
            {
                LastEvent = $"Resolved {record.questlineId} via approach {record.selectedApproach} → {record.endingKey}";
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
            Camp.OnStateChanged += _ => StateChanged?.Invoke();
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

        /// <summary>Feed the sector clock. The Muster triggers at Day 260+,
        /// and with it the Coalition's holding ground forms.</summary>
        public string Escalate(int day)
        {
            Engine.SetEscalationDay(day);
            if (Engine.MusterTriggered && !Camp.Formed)
                Camp.Form(day);
            LastEvent = Engine.MusterTriggered
                ? $"Day {day}: the Muster is open. Coalition camp holds {Camp.MembersRallied}."
                : $"Day {day}: escalation tracked (Muster opens Day {MusterSystem.MusterOpeningDay}).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Coalition camp ─────────────────────────────────────────────

        public string RallyDeserter()
        {
            bool ok = Camp.RallyDeserter();
            LastEvent = ok
                ? $"A deserter has walked in. Camp holds {Camp.MembersRallied}."
                : "No holding ground yet — the Muster has not opened.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string SetStrategy(QuestApproach strategy)
        {
            bool ok = Camp.SetStrategy(strategy);
            LastEvent = ok
                ? $"Strategy {strategy} chosen. Lockout risk {Camp.GarrisonLockoutRisk}%."
                : "Strategy rejected: not formed, or already chosen.";
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

        public MusterHostSave CaptureSave() => new MusterHostSave
        {
            Muster = Engine.CaptureState(),
            Camp = Camp.CaptureState()
        };

        public void RestoreSave(MusterHostSave save)
        {
            if (save == null) return;
            Engine.RestoreState(save.Muster);
            Camp.RestoreState(save.Camp);
        }
    }
}
