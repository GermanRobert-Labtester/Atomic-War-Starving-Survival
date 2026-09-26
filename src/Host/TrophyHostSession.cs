// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : TrophySaveStore
// Core State : Ashfall.Core.Shelter.TrophySaveState
// Host Caller: Main.Trophies
// Purpose    : Trophy mount pipeline host session & persistence. The Core
//              system remains the exactly-once award authority; the host
//              composes the authored trophies.json catalog and forwards
//              quarry-preservation facts from the hunting/trapping owners.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class TrophySaveStore
    {
        public const string FileName = "trophies_save.json";
        public const string SectionName = "trophies";

        private static readonly SaveStore<TrophySaveState> s_store =
            SaveStoreHub.Checksummed<TrophySaveState>(FileName, nameof(TrophySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(TrophySaveState state) => s_store.CaptureBare(state);
        public static TrophySaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(TrophySaveState state) => s_store.TrySave(state);
        public static TrophySaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Host session composing the Core <see cref="TrophySystem"/>.</summary>
    public sealed class TrophyHostSession : HostSessionBase
    {
        private readonly TrophySystem _system;

        public TrophySystem System => _system;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public TrophyHostSession(TrophySaveState? state = null)
        {
            _system = new TrophySystem();
            if (state != null) _system.RestoreState(state);
        }

        public static TrophyHostSession Create(TrophySaveState? state = null) => new TrophyHostSession(state);

        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "trophies.json");
            if (!File.Exists(path)) return false;
            try
            {
                CatalogReady = _system.LoadCatalogFromJson(File.ReadAllText(path));
                return CatalogReady;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public TrophyAwardRecord? RecordQuarryPreserved(string speciesId, int day = 1)
        {
            var record = _system.RecordQuarryPreserved(speciesId, day);
            if (record != null)
            {
                LastEvent = $"Trophy ready: {record.DisplayName}.";
                RaiseStateChanged();
            }
            return record;
        }

        public bool IsAwarded(string trophyId) => _system.IsAwarded(trophyId);
        public TrophyDefinition? GetTrophy(string trophyId) => _system.GetTrophy(trophyId);

        public TrophySaveState CaptureState() => _system.CaptureState();

        public void RestoreState(TrophySaveState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored trophy state.";
            RaiseStateChanged();
        }

        public bool TrySave() => TrophySaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = TrophySaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
