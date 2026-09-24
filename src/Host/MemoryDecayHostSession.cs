// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MemoryDecaySaveStore
// Core State : Ashfall.Core.Cognition.MemoryDecayState
// Host Caller: Main.MemoryDecay
// Purpose    : Plan 185 — Memory & Knowledge Decay host session & persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Cognition;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class MemoryDecaySaveStore
    {
        public const string FileName = "memory_decay_save.json";
        public const string SectionName = "memory_decay";

        private static readonly SaveStore<MemoryDecayState> s_store =
            SaveStoreHub.Checksummed<MemoryDecayState>(FileName, nameof(MemoryDecaySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(MemoryDecayState state) => s_store.CaptureBare(state);
        public static MemoryDecayState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(MemoryDecayState state) => s_store.TrySave(state);
        public static MemoryDecayState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 185 host session. Wraps <see cref="MemoryDecaySystem"/>.
    /// Exposes memory tracking, reinforcement, certification, and archive preservation.
    /// </summary>
    public sealed class MemoryDecayHostSession : HostSessionBase
    {
        private readonly MemoryDecaySystem _system;

        public MemoryDecaySystem System => _system;
        public MemoryDecayCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public MemoryDecayHostSession(MemoryDecayState? state = null)
        {
            _system = new MemoryDecaySystem(state);
        }

        public static MemoryDecayHostSession Create(MemoryDecayState? state = null) =>
            new MemoryDecayHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = "Loaded memory decay catalog.";
            RaiseStateChanged();
        }

        public MemoryRecord RegisterOrUpdate(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            float initialStrength = 100f,
            bool isCertified = false,
            bool isPreserved = false)
        {
            var record = _system.RegisterOrUpdate(survivorId, domain, referenceId, day, initialStrength, isCertified, isPreserved);
            LastEvent = $"Registered memory {referenceId} ({domain}) for {survivorId}.";
            RaiseStateChanged();
            return record;
        }

        public bool Reinforce(
            string survivorId,
            MemoryDomain domain,
            string referenceId,
            int day,
            ReinforcementType type = ReinforcementType.Review,
            float boostAmount = 25f)
        {
            bool ok = _system.Reinforce(survivorId, domain, referenceId, day, type, boostAmount);
            if (ok)
            {
                LastEvent = $"Reinforced memory {referenceId} ({domain}) for {survivorId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            LastEvent = $"Ticked memory decay on day {currentDay}.";
            RaiseStateChanged();
        }


        public MemoryDecayState CaptureState() => _system.CaptureState();
        public void RestoreState(MemoryDecayState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored memory decay state.";
            RaiseStateChanged();
        }

        public bool TrySave() => MemoryDecaySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = MemoryDecaySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
