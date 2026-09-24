// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : InterpersonalConflictSaveStore
// Core State : Ashfall.Core.Survivors.InterpersonalConflictState
// Host Caller: Main.InterpersonalConflict
// Purpose    : Plan 202 — Interpersonal Conflict & Grievance host session & persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class InterpersonalConflictSaveStore
    {
        public const string FileName = "interpersonal_conflict_save.json";
        public const string SectionName = "interpersonal_conflict";

        private static readonly SaveStore<InterpersonalConflictState> s_store =
            SaveStoreHub.Checksummed<InterpersonalConflictState>(FileName, nameof(InterpersonalConflictSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(InterpersonalConflictState state) => s_store.CaptureBare(state);
        public static InterpersonalConflictState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(InterpersonalConflictState state) => s_store.TrySave(state);
        public static InterpersonalConflictState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 202 host session. Wraps <see cref="InterpersonalConflictSystem"/>.
    /// Exposes conflict escalation, grievances, mediation, and resolution tracking.
    /// </summary>
    public sealed class InterpersonalConflictHostSession : HostSessionBase
    {
        private readonly InterpersonalConflictSystem _system;

        public InterpersonalConflictSystem System => _system;
        public InterpersonalConflictCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public InterpersonalConflictHostSession(InterpersonalConflictState? state = null)
        {
            _system = new InterpersonalConflictSystem(state);
            _system.OnConflictInitiated += c =>
            {
                LastEvent = $"Conflict erupted: {c.InitiatorId} vs {c.TargetId} ({c.Type}).";
                RaiseStateChanged();
            };

            _system.OnConflictEscalated += (c, score) =>
            {
                LastEvent = $"Conflict {c.ConflictId} escalated to {score:F1}.";
                RaiseStateChanged();
            };
            _system.OnConflictResolved += (c, r) =>
            {
                LastEvent = $"Conflict {c.ConflictId} resolved via {r.Method}.";
                RaiseStateChanged();
            };
            _system.OnPhysicalFightRisk += c =>
            {
                LastEvent = $"CRISIS: Physical fight risk between {c.InitiatorId} and {c.TargetId}!";
                RaiseStateChanged();
            };
        }

        public static InterpersonalConflictHostSession Create(InterpersonalConflictState? state = null) =>
            new InterpersonalConflictHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = $"Loaded {_system.GetAllTemplates().Count} conflict templates.";
            RaiseStateChanged();
        }

        public InterpersonalConflict InitiateConflict(
            string initiatorId,
            string targetId,
            ConflictType type,
            string triggerDescription,
            ConflictSeverity severity = ConflictSeverity.Mild,
            int currentDay = 1)
        {
            var conflict = _system.InitiateConflict(initiatorId, targetId, type, triggerDescription, severity, currentDay);
            RaiseStateChanged();
            return conflict;
        }

        public InterpersonalConflict? InitiateConflictFromTemplate(
            string templateId, string initiatorId, string targetId, int currentDay = 1)
        {
            var conflict = _system.InitiateConflictFromTemplate(templateId, initiatorId, targetId, currentDay);
            if (conflict != null) RaiseStateChanged();
            return conflict;
        }

        public SurvivorGrievance AddGrievance(

            string holderId,
            string accusedId,
            string reason,
            float intensity = 30f,
            int currentDay = 1)
        {
            return _system.AddGrievance(holderId, accusedId, reason, intensity, currentDay);
        }

        public float EscalateConflict(string conflictId, float delta, int currentDay) =>
            _system.EscalateConflict(conflictId, delta, currentDay);

        public bool MediateConflict(string conflictId, string mediatorId, int currentDay) =>
            _system.MediateConflict(conflictId, mediatorId, currentDay);

        public bool ApologizeAndResolve(string conflictId, int currentDay) =>
            _system.ApologizeAndResolve(conflictId, currentDay);

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            LastEvent = $"Ticked interpersonal conflicts on day {currentDay}.";
            RaiseStateChanged();
        }

        public InterpersonalConflictState CaptureState() => _system.CaptureState();
        public void RestoreState(InterpersonalConflictState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored interpersonal conflict state.";
            RaiseStateChanged();
        }

        public bool TrySave() => InterpersonalConflictSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = InterpersonalConflictSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
