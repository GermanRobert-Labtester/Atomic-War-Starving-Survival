// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : UnifiedEndingSaveStore
// Core State : Ashfall.Core.Endgame.UnifiedEndingSaveState
// Host Caller: Main.UnifiedEnding (SetupUnifiedEnding / SaveUnifiedEnding)
// Purpose    : Plan 145 — Unified ending resolution & epilogue personalization:
//              political, social, moral, personal, and expedition resolution.
// ============================================================================

using System;
using System.Collections.Generic;
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
        private Ashfall.Core.Endgame.EpilogueChronicle? _lastChronicle;

        public event Action<UnifiedEndingResult>? EndingResolved;

        public UnifiedEndingResolver Resolver => _resolver;
        public string LastEvent => _lastEvent;
        public bool IsResolved => _resolver.IsResolved;
        public UnifiedEndingResult? LastResult => _resolver.LastResult;
        public UnifiedEndingCensus Census => _resolver.GetCensus();

        /// <summary>
        /// Projects a resolved ending into the ordered epilogue chronicle the
        /// ending UI reads back. Pure projection over <see cref="EpilogueChronicleBuilder"/>:
        /// slides in index order, fate cards by survivor id, metrics by metric id,
        /// ending-title mapping and deterministic ordering. No gameplay decision.
        /// </summary>
        public static Ashfall.Core.Endgame.EpilogueChronicle BuildChronicle(
            UnifiedEndingResult result, int buildSeed)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            var slides = new List<Ashfall.Core.Endgame.EpilogueSlide>
            {
                new Ashfall.Core.Endgame.EpilogueSlide(0, "The Campaign", result.durationProse ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(1, "Political Outcome", result.politicalProse ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(2, "Social Outcome", result.socialProse ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(3, "Moral Outcome", result.moralProse ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(4, "Personal Summary", result.personalSummary ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(5, "Expeditions", result.expeditionProse ?? string.Empty),
                new Ashfall.Core.Endgame.EpilogueSlide(6, "The Shelter", result.shelterProse ?? string.Empty)
            };

            var fates = new List<Ashfall.Core.Endgame.SurvivorFateCard>();
            if (result.survivorEpilogues != null)
            {
                foreach (var e in result.survivorEpilogues)
                {
                    if (e == null) continue;
                    fates.Add(new Ashfall.Core.Endgame.SurvivorFateCard
                    {
                        SurvivorId = e.survivorId ?? string.Empty,
                        DisplayName = e.survivorName ?? string.Empty,
                        Fate = e.epilogueText ?? string.Empty,
                        Survived = e.status == Ashfall.Core.Endgame.SurvivorFateStatus.Alive
                    });
                }
            }

            var metrics = new List<Ashfall.Core.Endgame.EpilogueMetric>();
            if (result.legacyTraitsAwarded != null && result.legacyTraitsAwarded.Count > 0)
                metrics.Add(new Ashfall.Core.Endgame.EpilogueMetric(
                    "legacy_traits", result.legacyTraitsAwarded.Count, "Legacy Traits Awarded"));
            if (result.survivorEpilogues != null)
                metrics.Add(new Ashfall.Core.Endgame.EpilogueMetric(
                    "survivors_recorded", result.survivorEpilogues.Count, "Survivors Recorded"));

            return new EpilogueChronicleBuilder().Build(new EpilogueChronicleInput
            {
                EndingKey = result.resolutionId ?? string.Empty,
                Day = 0,
                BuildSeed = buildSeed,
                Slides = slides,
                FateCards = fates,
                Metrics = metrics
            });
        }

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

            // The epilogue chronicle builder is the presentation authority for
            // the ordered chronicle readback of an already-resolved ending.
            // It holds no state and makes no gameplay decision — it projects
            // the resolver's own result into slides/fate-cards/metrics.
            _lastChronicle = BuildChronicle(result, AdBuildSeed(context));

            _lastEvent = $"Campaign resolved with ending: {result.overallTitle}";
            RaiseStateChanged();
            return result;
        }

        /// <summary>The last built epilogue chronicle, or null before an ending resolves.</summary>
        public Ashfall.Core.Endgame.EpilogueChronicle? LastChronicle => _lastChronicle;

        /// <summary>
        /// Deterministic chronicle seed derived from the same resolver output the
        /// builder renders — never wall-clock and never iteration order.
        /// </summary>
        private static int AdBuildSeed(UnifiedEndingContext context)
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 31) + Math.Abs(context.totalDaysSurvived);
                hash = (hash * 31) + Math.Abs(context.livingDwellerCount);
                hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(context.factionBranchId ?? string.Empty);
                hash = (hash * 31) + StringComparer.Ordinal.GetHashCode(context.moralChoiceBand ?? string.Empty);
                return hash & 0x7FFFFFFF;
            }
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
