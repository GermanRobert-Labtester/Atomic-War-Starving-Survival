using System;
using System.Collections.Generic;

using Ashfall.Core.IO;
namespace Ashfall.Core.UtilityAI
{
    /// <summary>
    /// Engine-agnostic Utility AI selection core (audit A1/A2 fixed in the
    /// port): picks the highest-scoring candidate with deterministic noise
    /// from the caller-supplied ISeededRng (the Unity original used a hidden
    /// System.Random). Ties are first-wins over the caller's candidate order —
    /// the candidate list order IS the deterministic contract. Stateless:
    /// no save state exists (audit A6); contexts are per-call host data.
    /// </summary>
    public class UtilityAiSystem
    {
        public const double NoiseScale = 0.0001d; // Unity parity: score noise amplitude

        public event Action<string, string, float> OnActionSelected; // survivorId, actionId, score

        public UtilityActionDef? SelectAction(
            AIActionContext context,
            IReadOnlyList<UtilityActionDef> candidates,
            ISeededRng rng,
            UtilityActionScorer scorer = null)
        {
            if (context == null || candidates == null || candidates.Count == 0) return null;
            scorer = scorer ?? new UtilityActionScorer();

            UtilityActionDef? best = null;
            float bestScore = -1f;

            for (int i = 0; i < candidates.Count; i++)
            {
                var candidate = candidates[i];
                if (candidate == null) continue;

                float score = scorer.Score(candidate, context);

                // Deterministic noise: seeded per call, same scale as Unity.
                if (score > 0f && rng != null)
                    score += (float)(rng.NextDouble() * NoiseScale);

                // Only positive scores compete: a hard-vetoed (0) action must
                // never win just because bestScore started at -1 (latent Unity
                // defect fixed in the port — audit A9).
                if (score > 0f && score > bestScore)
                {
                    bestScore = score;
                    best = candidate;
                }
            }

            if (best != null && !string.IsNullOrEmpty(context.SurvivorId))
                OnActionSelected?.Invoke(context.SurvivorId, best.id, Math.Max(0f, bestScore));

            return best;
        }

        /// <summary>
        /// Score all candidates (for UI/debugging), ordinal-stable.
        /// </summary>
        public List<KeyValuePair<UtilityActionDef, float>> ScoreAll(
            AIActionContext context,
            IReadOnlyList<UtilityActionDef> candidates,
            UtilityActionScorer scorer = null)
        {
            var result = new List<KeyValuePair<UtilityActionDef, float>>();
            if (context == null || candidates == null) return result;
            scorer = scorer ?? new UtilityActionScorer();
            for (int i = 0; i < candidates.Count; i++)
            {
                var c = candidates[i];
                if (c == null) continue;
                result.Add(new KeyValuePair<UtilityActionDef, float>(c, scorer.Score(c, context)));
            }
            return result;
        }
    }

    /// <summary>Engine-agnostic loader for utility_actions.json (data-driven action definitions).</summary>
    public static class UtilityActionCatalogLoader
    {
        public const string FileName = "utility_actions.json";

        public static List<UtilityActionDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<UtilityActionDef>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var parsed = json.Deserialize<UtilityActionDef[]>(raw);
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var def = parsed[i];
                    if (def == null || string.IsNullOrEmpty(def.id)) continue;
                    if (def.tags == null) def.tags = Array.Empty<string>();
                    if (def.displayName == null) def.displayName = def.id;
                    result.Add(def);
                }
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return result;
                                }
            return result;
        }
    }
}
