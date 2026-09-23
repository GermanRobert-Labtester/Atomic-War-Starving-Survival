// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : UnifiedEndingSaveStore
// Core State : Ashfall.Core.Endgame.UnifiedEndingSaveState
// Host Caller: Main.UnifiedEnding (SetupUnifiedEnding / SaveUnifiedEnding)
// Purpose    : Plan 145 — Unified ending resolution & epilogue personalization:
//              political, social, moral, personal, and expedition resolution.
// ============================================================================

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class UnifiedEndingSaveStore
    {
        public const string FileName = "unified_ending_save.json";
        public const string SectionName = "unified_ending";

        private static readonly SaveStore<UnifiedEndingSaveState> s_store =
            SaveStoreHub.Checksummed<UnifiedEndingSaveState>(FileName, nameof(UnifiedEndingSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(UnifiedEndingSaveState state) => s_store.CaptureBare(state);
        public static UnifiedEndingSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(UnifiedEndingSaveState state) => s_store.TrySave(state);
        public static UnifiedEndingSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 145 (Unified Ending Resolution & Epilogue Personalization).
    /// Unifies Holdfast endings, Muster epilogues, and Epilogue Matrix into a single coherent
    /// endgame resolution that evaluates full campaign state and generates a personalized chronicle.
    /// </summary>
    public sealed class UnifiedEndingHostSession : HostSessionBase
    {
        private readonly UnifiedEndingResolver _resolver;
        private string _lastEvent = string.Empty;

        public event Action<UnifiedEndingResult>? EndingResolved;

        public UnifiedEndingResolver Resolver => _resolver;
        public string LastEvent => _lastEvent;
        public bool IsResolved => _resolver.IsResolved;
        public UnifiedEndingResult? LastResult => _resolver.LastResult;
        public UnifiedEndingCensus Census => _resolver.GetCensus();

        public UnifiedEndingHostSession(string? dataDir = null, UnifiedEndingResolver? resolver = null)
        {
            _resolver = resolver ?? new UnifiedEndingResolver();

            _resolver.OnUnifiedEndingResolvedSeam = result =>
            {
                _lastEvent = $"Ending resolved: {result.overallTitle} ({result.resolutionId})";
                RaiseStateChanged();
                EndingResolved?.Invoke(result);
            };

            _resolver.OnSurvivorEpilogueGeneratedSeam = fate =>
            {
                _lastEvent = $"Survivor epilogue generated: {fate.survivorName} ({fate.status})";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static UnifiedEndingHostSession Create(string dataDir, UnifiedEndingResolver? resolver = null)
        {
            return new UnifiedEndingHostSession(dataDir, resolver);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, "epilogue_personalization.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _resolver.LoadCatalog(json);
                _lastEvent = "Loaded epilogue personalization catalog.";
                RaiseStateChanged();
            }
        }

        public UnifiedEndingResult Resolve(UnifiedEndingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var result = _resolver.ResolveEnding(context);
            _lastEvent = $"Campaign resolved with ending: {result.overallTitle}";
            RaiseStateChanged();
            return result;
        }

        public UnifiedEndingSaveState CaptureState() => _resolver.CaptureState();

        public void RestoreState(UnifiedEndingSaveState state)
        {
            _resolver.RestoreState(state);
            _lastEvent = "Restored unified ending state.";
            RaiseStateChanged();
        }
    }
}
