// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store + Host Session : ChronicCondition
// Core State : Ashfall.Core.Medical.ChronicConditionState
// Host Caller: Main.ChronicConditions
// Purpose    : Plan 193 — Chronic Conditions & Accommodations host session.
//              One truthful path from a diagnosed condition / documented
//              injury outcome to an approved accommodation and a capability
//              query on the real consumers. `ChronicConditionSystem` keeps
//              the rule ownership; this session owns only the host lifetime:
//              catalog load, host-session custody, capture/restore, and the
//              read model for the care surface. No second health ledger:
//              records are written only from committed clinical producer
//              events, never re-derived from mood.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ChronicConditionSaveStore
    {
        public const string FileName = "chronic_condition_save.json";
        public const string SectionName = "chronic_condition";

        private static readonly SaveStore<ChronicConditionState> s_store =
            SaveStoreHub.Checksummed<ChronicConditionState>(FileName, nameof(ChronicConditionSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static bool TrySave(ChronicConditionState state) => s_store.TrySave(state);
        public static ChronicConditionState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ChronicConditionState state) => s_store.CaptureBare(state);
    }

    /// <summary>
    /// Plan 193 host session over <see cref="ChronicConditionSystem"/>.
    /// Conditions arrive only from committed clinical producers; accommodations
    /// are assigned/removed through explicit host commands; the capability
    /// modifier is a read-only projection consumed by duty/needs owners.
    /// </summary>
    public sealed class ChronicConditionHostSession : HostSessionBase
    {
        private readonly ChronicConditionSystem _system;
        private string _dataDir = string.Empty;
        private bool _catalogLoaded;

        public ChronicConditionSystem System => _system;
        public string LastEvent { get; private set; } = string.Empty;
        public bool CatalogLoaded => _catalogLoaded;

        public event Action<string>? OnConditionRecorded; // (message)

        public ChronicConditionHostSession()
        {
            _system = new ChronicConditionSystem();
        }

        /// <summary>
        /// Creates the session and loads the authored catalog. Missing or
        /// malformed catalog is a hard observable (catalog missing disarms the
        /// capability projection so no silent zero-penalty path exists).
        /// </summary>
        public static ChronicConditionHostSession Create(string dataDir)
        {
            var session = new ChronicConditionHostSession { _dataDir = dataDir ?? string.Empty };
            var path = Path.Combine(
                string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir,
                "chronic_conditions.json");
            if (File.Exists(path))
            {
                try
                {
                    session._system.LoadCatalog(File.ReadAllText(path));
                    session._catalogLoaded = true;
                    session.LastEvent = "Chronic-condition catalog loaded.";
                }
                catch (Exception ex)
                {
                    session.LastEvent = $"Chronic catalog load warning: {ex.Message}";
                }
            }
            else
            {
                session.LastEvent = "Chronic catalog missing; capability projection unarmed.";
            }
            return session;
        }

        /// <summary>
        /// Single producer seam: a committed clinical fact (a confirmed
        /// diagnosis or documented permanent-injury outcome) records exactly
        /// one condition for the survivor. Idempotent on survivor+condition.
        /// </summary>
        public SurvivorConditionRecord? RecordCondition(string survivorId, string conditionId, int day, string cause)
        {
            var rec = _system.AddCondition(survivorId, conditionId, day, cause);
            if (rec != null)
            {
                LastEvent = $"Condition {conditionId} recorded for {survivorId} (day {day}, cause {rec.Cause}).";
                OnConditionRecorded?.Invoke(LastEvent);
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Condition {conditionId} refused for {survivorId} (already tracked or invalid ids).";
            }
            return rec;
        }

        /// <summary>
        /// Accommodation assignment refuses when the condition has no def or
        /// the accommodation id is unknown — the plan's explicit refusal case.
        /// </summary>
        public SurvivorAccommodationRecord? AssignAccommodation(string survivorId, string accommodationId, string conditionId, int day)
        {
            if (_system.GetAccommodationDef(accommodationId) == null)
            {
                LastEvent = $"Accommodation {accommodationId} refused for {survivorId} (unknown accommodation id).";
                return null; // explicit refusal — no silent acceptance
            }
            var rec = _system.AssignAccommodation(survivorId, accommodationId, conditionId, day);
            if (rec != null)
            {
                LastEvent = $"Accommodation {accommodationId} assigned to {survivorId} for {conditionId} (day {day}).";
                OnConditionRecorded?.Invoke(LastEvent);
                RaiseStateChanged();
            }
            return rec;
        }

        public bool RemoveAccommodation(string survivorId, string accommodationId)
        {
            bool ok = _system.RemoveAccommodation(survivorId, accommodationId);
            if (ok)
            {
                LastEvent = $"Accommodation {accommodationId} removed from {survivorId}.";
                OnConditionRecorded?.Invoke(LastEvent);
                RaiseStateChanged();
            }
            return ok;
        }

        public IReadOnlyList<SurvivorConditionRecord> GetSurvivorConditions(string survivorId) =>
            _system.GetSurvivorConditions(survivorId);

        public IReadOnlyList<SurvivorAccommodationRecord> GetSurvivorAccommodations(string survivorId) =>
            _system.GetSurvivorAccommodations(survivorId);

        public float CalculateCapabilityModifier(string survivorId, string capability) =>
            _system.CalculateCapabilityModifier(survivorId, capability);

        public float GetTotalImpairmentScore(string survivorId) => _system.GetTotalImpairmentScore(survivorId);

        public IReadOnlyCollection<ChronicConditionDef> GetAllConditionDefs() => _system.GetAllConditionDefs();
        public IReadOnlyCollection<AccommodationDef> GetAllAccommodationDefs() => _system.GetAllAccommodationDefs();

        public ChronicConditionState CaptureState() => _system.CaptureState();

        public void RestoreState(ChronicConditionState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored chronic-condition state.";
            RaiseStateChanged();
        }

        public bool TrySave() => ChronicConditionSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = ChronicConditionSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
