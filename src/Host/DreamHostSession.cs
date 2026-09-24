// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DreamSaveStore
// Core State : Ashfall.Core.Survivors.DreamSystemState
// Host Caller: Main.DreamSystem
// Purpose    : Plan 177 — Survivor Dream & Sleep Event System host session & persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class DreamSaveStore
    {
        public const string FileName = "survivor_dreams_save.json";
        public const string SectionName = "survivor_dreams";

        private static readonly SaveStore<DreamSystemState> s_store =
            SaveStoreHub.Checksummed<DreamSystemState>(FileName, nameof(DreamSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(DreamSystemState state) => s_store.CaptureBare(state);
        public static DreamSystemState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(DreamSystemState state) => s_store.TrySave(state);
        public static DreamSystemState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 177 host session. Wraps <see cref="DreamSystem"/>.
    /// Manages nocturnal dream generation, nightmare compounding, and psychological dream interpretations.
    /// </summary>
    public sealed class DreamHostSession : HostSessionBase
    {
        private readonly DreamSystem _system;

        public DreamSystem System => _system;
        public DreamCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public DreamHostSession(DreamSystemState? state = null)
        {
            _system = new DreamSystem(state);
        }

        public static DreamHostSession Create(DreamSystemState? state = null) =>
            new DreamHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = $"Loaded {_system.AuthoredTemplates.Count} dream templates.";
            RaiseStateChanged();
        }

        public DreamSleepResult ProcessSleepCycle(string survivorId, float trauma, float morale, int currentDay, ISeededRng rng, bool forceDream = false)
        {
            var result = _system.ProcessSleepCycle(survivorId, trauma, morale, currentDay, rng, forceDream);
            LastEvent = result.HadDream
                ? $"Survivor '{survivorId}' experienced {result.Record?.dream_type} dream '{result.Record?.display_name}' on Day {currentDay}."
                : $"Survivor '{survivorId}' had dreamless sleep on Day {currentDay}.";
            RaiseStateChanged();
            return result;
        }

        public bool InterpretDream(string survivorId, string recordId, string interpretationChoice)
        {
            bool interpreted = _system.InterpretDream(survivorId, recordId, interpretationChoice);
            LastEvent = interpreted
                ? $"Dream '{recordId}' interpreted for '{survivorId}' with choice '{interpretationChoice}'."
                : $"Failed to interpret dream '{recordId}' for '{survivorId}'.";
            RaiseStateChanged();
            return interpreted;
        }

        public List<DreamRecord> GetDreamHistory(string survivorId) => _system.GetDreamHistory(survivorId);
        public int GetConsecutiveNightmares(string survivorId) => _system.GetConsecutiveNightmares(survivorId);
    }
}
