using System;
using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>One path-sensitive staging variant of a camp scene. Selection is
    /// first authored match: the muster path gate is checked first (empty = any
    /// path), then flags; a variant with no gates is the terminal fallback.</summary>
    public class CampSceneVariant
    {
        public string variantId = string.Empty;
        public string requiresPath = string.Empty;   // MusterPaths value, or "" for any
        public List<string> requiresFlags = new List<string>();
        public List<string> forbidsFlags = new List<string>();
        public string body = string.Empty;
    }

    /// <summary>One authored Coalition Camp scene (muster_camp_scenes.json).</summary>
    public class CampSceneDefinition
    {
        public string id = string.Empty;
        public string scene = string.Empty;          // arrivals | old_enemies | shared_meal | confrontation
        public int minDay = MusterSystem.MusterOpeningDay;
        public List<string> requiresFlags = new List<string>();
        public List<CampSceneVariant> variants = new List<CampSceneVariant>();
    }

    /// <summary>A scene with the variant that matches the campaign right now.</summary>
    public class CampSceneSelection
    {
        public CampSceneDefinition Definition;
        public CampSceneVariant Variant;
        public string VariantId => Variant?.variantId ?? string.Empty;
    }

    /// <summary>
    /// Engine-agnostic loader for muster_camp_scenes.json — Plan 25's authored
    /// Coalition Camp social staging (Plan 25 · 25B.13/25F). Missing file → empty
    /// list; future schema → empty list (never partially parsed).
    /// </summary>
    public static class CampSceneCatalogLoader
    {
        public const string FileName = "muster_camp_scenes.json";
        public const int CurrentSchemaVersion = 1;

        public static List<CampSceneDefinition> LoadScenes(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<CampSceneDefinition>();
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
                var root = json.Deserialize<CampSceneRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.scenes;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id) || string.IsNullOrEmpty(e.scene))
                        continue;
                    var def = new CampSceneDefinition
                    {
                        id = e.id,
                        scene = e.scene,
                        minDay = e.min_day > 0 ? e.min_day : MusterSystem.MusterOpeningDay
                    };
                    CopyFlags(e.requires_flags, def.requiresFlags);
                    if (e.variants != null)
                    {
                        for (int v = 0; v < e.variants.Count; v++)
                        {
                            var ve = e.variants[v];
                            if (ve == null || string.IsNullOrEmpty(ve.body)) continue;
                            var variant = new CampSceneVariant
                            {
                                variantId = ve.variant_id ?? string.Empty,
                                requiresPath = ve.requires_path ?? string.Empty,
                                body = ve.body
                            };
                            CopyFlags(ve.requires_flags, variant.requiresFlags);
                            CopyFlags(ve.forbids_flags, variant.forbidsFlags);
                            def.variants.Add(variant);
                        }
                    }
                    if (def.variants.Count > 0) result.Add(def);
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "CampSceneRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        private static void CopyFlags(List<string> source, List<string> target)
        {
            if (source == null) return;
            for (int i = 0; i < source.Count; i++)
                if (!string.IsNullOrEmpty(source[i])) target.Add(source[i]);
        }

        private class CampSceneRoot
        {
            public int schema_version = 1;
            public List<SceneEntry> scenes = new List<SceneEntry>();
        }

        private class SceneEntry
        {
            public string id;
            public string scene;
            public int min_day;
            public List<string> requires_flags;
            public List<VariantEntry> variants;
        }

        private class VariantEntry
        {
            public string variant_id;
            public string requires_path;
            public List<string> requires_flags;
            public List<string> forbids_flags;
            public string body;
        }
    }

    /// <summary>
    /// Deterministic camp-scene staging (Plan 25 · 25F). The same campaign state
    /// always yields the same scene variant: scene id lookup (ordinal) → day gate
    /// → flag gates → first variant whose muster-path gate matches → first variant
    /// whose flag gates match. No RNG; absence (dead faction, no witness) is the
    /// author's job via variants, never a random substitution.
    /// </summary>
    public static class CampSceneDirector
    {
        /// <param name="isSceneSeen">Host-side progression guard: a scene already
        /// staged in this campaign does not restage (null = never seen).</param>
        public static CampSceneSelection? Select(
            IEnumerable<CampSceneDefinition> scenes,
            string sceneId,
            int day,
            string musterPath,
            Func<string, bool> isFlagSet,
            Func<string, bool> isSceneSeen = null)
        {
            if (scenes == null || string.IsNullOrEmpty(sceneId)) return null;

            CampSceneDefinition def = null;
            foreach (var s in scenes)
            {
                if (s == null || s.id != sceneId) continue;
                def = s;
                break;
            }
            if (def == null) return null;
            if (day >= 0 && day < def.minDay) return null;
            if (isSceneSeen != null && isSceneSeen(sceneId)) return null;
            for (int i = 0; i < def.requiresFlags.Count; i++)
                if (isFlagSet == null || !isFlagSet(def.requiresFlags[i])) return null;

            for (int i = 0; i < def.variants.Count; i++)
            {
                var v = def.variants[i];
                if (!PathMatches(v.requiresPath, musterPath)) continue;
                if (!FlagsMatch(v, isFlagSet)) continue;
                return new CampSceneSelection { Definition = def, Variant = v };
            }
            return null;
        }

        private static bool PathMatches(string requiresPath, string musterPath)
        {
            if (string.IsNullOrEmpty(requiresPath)) return true;
            return string.Equals(requiresPath, musterPath, StringComparison.Ordinal);
        }

        private static bool FlagsMatch(CampSceneVariant v, Func<string, bool> isFlagSet)
        {
            for (int i = 0; i < v.requiresFlags.Count; i++)
                if (isFlagSet == null || !isFlagSet(v.requiresFlags[i])) return false;
            for (int i = 0; i < v.forbidsFlags.Count; i++)
                if (isFlagSet != null && isFlagSet(v.forbidsFlags[i])) return false;
            return true;
        }
    }
}
