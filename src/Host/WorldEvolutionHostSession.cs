// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
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
        public static string TryCapturePersisted(WorldEvolutionState state) => s_store.CaptureBare(state);
        public static WorldEvolutionState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WorldEvolutionState state) => s_store.TrySave(state);
        public static WorldEvolutionState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 227: World Evolution — host session binding the Core engine to
    /// the campaign lifecycle. The engine is the single authority for
    /// world-state evolution events; it reads the authoritative wasteland map
    /// passed in at tick time and never keeps a second map graph.
    /// </summary>
    public sealed class WorldEvolutionHostSession : HostSessionBase
    {
        private readonly WorldEvolutionEngine _engine;

        public WorldEvolutionEngine Engine => _engine;
        public WorldEvolutionState State => _engine.CaptureState();
        public IReadOnlyList<WorldEvolutionEventDef> Events => _engine.Events;
        public IReadOnlyCollection<string> TriggeredEventIds => _engine.TriggeredEventIds;

        public WorldEvolutionHostSession(string? dataDir = null, WorldEvolutionEngine? engine = null)
        {
            _engine = engine ?? new WorldEvolutionEngine(
                dataDir ?? Path.Combine("Assets", "StreamingAssets", "Data"));
        }

        /// <summary>
        /// Advance one campaign day. Collaborators other than the map are
        /// passed null when their own owners are not yet campaign-bound; the
        /// engine handles that explicitly.
        /// </summary>
        public void Tick(int day, System.Collections.Generic.HashSet<string>? activeWorldFlags, WastelandMapSystem? map)
        {
            _engine.TickDay(day, activeWorldFlags, null, null, map);
            RaiseStateChanged();
        }

        public WorldEvolutionState CaptureState()
        {
            return _engine.CaptureState();
        }

        public void RestoreState(WorldEvolutionState state, WastelandMapSystem? map)
        {
            _engine.RestoreState(state, map);
            RaiseStateChanged();
        }

        public void Reset()
        {
            // The Core contract treats a null payload as "do not restore", so a
            // reset must restore an EMPTY state, which clears the triggered set.
            _engine.RestoreState(new WorldEvolutionState(), null);
            RaiseStateChanged();
        }
    }
}
