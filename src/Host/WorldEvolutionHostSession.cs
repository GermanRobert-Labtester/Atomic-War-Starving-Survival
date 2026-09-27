// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 227: World Evolution — save store.
    /// </summary>
    public static class WorldEvolutionSaveStore
    {
        public const string FileName = "world_evolution_save.json";
        public const string SectionName = "world_evolution";

        private static readonly SaveStore<WorldEvolutionState> s_store =
            SaveStoreHub.Checksummed<WorldEvolutionState>(FileName, nameof(WorldEvolutionSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(WorldEvolutionState state) => s_store.TrySave(state);
        public static WorldEvolutionState? TryLoad() => s_store.TryLoad();
        // Coordination repair 2026-09-27 (rumor/memorial seal session): added the
        // canonical capture method Main.WorldEvolution.SaveWorldEvolution calls
        // (Checksummed stores expose CaptureBare).
        public static string TryCapturePersisted(WorldEvolutionState state) => s_store.CaptureBare(state);
    }

    /// <summary>
    /// Plan 227: World Evolution — host session binding the Core engine to
    /// the campaign lifecycle. Loads event catalog, ticks daily, persists state.
    /// </summary>
    public sealed class WorldEvolutionHostSession : HostSessionBase
    {
        private readonly WorldEvolutionEngine _engine;
        private readonly string _dataDir;

        public WorldEvolutionEngine Engine => _engine;
        /// <summary>Read-only projection of the engine's current state.</summary>
        public WorldEvolutionState State => _engine.CaptureState();

        public WorldEvolutionHostSession(WorldEvolutionEngine? engine = null, string? dataDir = null)
        {
            // Coordination repair 2026-09-27 (rumor/memorial seal session): the
            // live engine ctor requires a dataDir and loads its authored event
            // catalog there (falling back to built-in defaults).
            _dataDir = dataDir ?? Path.Combine("Assets", "StreamingAssets", "Data");
            _engine = engine ?? new WorldEvolutionEngine(_dataDir);
        }

        public void Setup()
        {
            // The engine loads world_evolution_events.json from _dataDir in its
            // own constructor (defaults on absence/parse failure); nothing to
            // read a second time here.
            if (_engine.Events.Count == 0)
                Console.Error.WriteLine("[WORLD_EVOLUTION] event catalog empty after construction");
        }

        public void Tick(int day)
        {
            // Optional collaborators (flags/evolution/landmarks/map) are
            // null-guarded inside Core; the canonical clock is always stamped.
            _engine.TickDay(day, null, null, null, null);
            RaiseStateChanged();
        }

        public WorldEvolutionState CaptureState()
        {
            return _engine.CaptureState();
        }

        public void RestoreState(WorldEvolutionState state)
        {
            _engine.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _engine.RestoreState(new WorldEvolutionState());
            RaiseStateChanged();
        }
    }
}
