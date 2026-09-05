// SPDX-License-Identifier: MIT
// ============================================================================
// System     : NutritionDiversitySystem (Plan 162 §5.19-5.20)
// Data       : Assets/StreamingAssets/Data/nutrition_profiles.json
// Purpose    : Rolling dietary-diversity log per survivor. Agriculture (and
//              any food source) records meals; the system tracks category
//              coverage over a 14-day window and reports deficiency state.
//              Consequences are NOT applied here — the host applies them
//              through the canonical NeedsSystem.Modify surface.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Farming
{
    [Serializable]
    public sealed class FoodNutritionEntry
    {
        public string item_id { get; set; } = string.Empty;
        public float calories { get; set; } = 1f;
        public float protein { get; set; }
        public float vitamin_c { get; set; }
        public float micronutrients { get; set; }
        public float fats { get; set; }
        public float fiber { get; set; }
    }

    [Serializable]
    public sealed class NutritionProfileCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<FoodNutritionEntry> profiles { get; set; } = new List<FoodNutritionEntry>();
    }

    public static class NutritionProfileCatalogLoader
    {
        public const string DefaultFileName = "nutrition_profiles.json";

        public static NutritionProfileCatalogContainer Load(
            string dataDir, IFileIO files, IJsonSerializer json)
        {
            var path = Path.Combine(dataDir, DefaultFileName);
            if (!files.FileExists(path)) return new NutritionProfileCatalogContainer();
            try
            {
                var text = files.ReadAllText(path);
                return json.Deserialize<NutritionProfileCatalogContainer>(text)
                       ?? new NutritionProfileCatalogContainer();
            }
            catch (Exception)
            {
                return new NutritionProfileCatalogContainer();
            }
        }
    }

    [Serializable]
    public sealed class DietLogEntry
    {
        public int day;
        public float calories;
        public float protein;
        public float vitamin_c;
        public float micronutrients;
        public float fats;
        public float fiber;

        public float Get(string category) => category switch
        {
            "calories" => calories,
            "protein" => protein,
            "vitamin_c" => vitamin_c,
            "micronutrients" => micronutrients,
            "fats" => fats,
            "fiber" => fiber,
            _ => 0f
        };
    }

    [Serializable]
    public sealed class SurvivorDietState
    {
        public string survivor_id = "";
        public List<DietLogEntry> entries = new List<DietLogEntry>();
        /// <summary>Categories currently deficient (subset of the 5 non-calorie categories).</summary>
        public List<string> deficient_categories = new List<string>();
    }

    [Serializable]
    public sealed class NutritionDiversityState
    {
        public string system_id = "nutrition_diversity";
        public int schema_version = 1;
        public List<SurvivorDietState> survivors = new List<SurvivorDietState>();
        public int last_tick_day;
    }

    /// <summary>
    /// Dietary diversity tracker. Calories are owned by the NeedsSystem hunger
    /// model and excluded from diversity judgement; the five quality
    /// categories (protein, vitamin_c, micronutrients, fats, fiber) must each
    /// appear in the rolling window or the survivor's diet is deficient in
    /// that category. Deterministic: no RNG anywhere in this system.
    /// </summary>
    public sealed class NutritionDiversitySystem
    {
        public const int WindowDays = 14;
        /// <summary>A category counts as covered by an entry at this weight or above.</summary>
        public const float CoverageThreshold = 0.25f;

        public static readonly string[] DiversityCategories =
        {
            "protein", "vitamin_c", "micronutrients", "fats", "fiber"
        };

        private readonly NutritionDiversityState _state =
            new NutritionDiversityState();
        private readonly Dictionary<string, FoodNutritionEntry> _profiles =
            new Dictionary<string, FoodNutritionEntry>(StringComparer.Ordinal);

        public string SystemId => _state.system_id;
        public NutritionDiversityState State => _state;

        public event Action<string, string> OnDeficiencyStarted;
        public event Action<string, string> OnDeficiencyCleared;

        public void LoadCatalog(NutritionProfileCatalogContainer catalog)
        {
            _profiles.Clear();
            if (catalog?.profiles == null) return;
            foreach (var p in catalog.profiles)
                if (p != null && !string.IsNullOrEmpty(p.item_id))
                    _profiles[p.item_id] = p;
        }

        public FoodNutritionEntry ProfileFor(string itemId)
        {
            if (!string.IsNullOrEmpty(itemId) && _profiles.TryGetValue(itemId, out var p))
                return p;
            // Unmapped food: bare calories — monotonous by definition.
            return new FoodNutritionEntry { item_id = itemId ?? "", calories = 1f };
        }

        private SurvivorDietState Survivor(string survivorId)
        {
            var s = _state.survivors.Find(x => x != null && string.Equals(x.survivor_id, survivorId, StringComparison.Ordinal));
            if (s == null)
            {
                s = new SurvivorDietState { survivor_id = survivorId ?? "" };
                _state.survivors.Add(s);
                _state.survivors.Sort((a, b) => string.CompareOrdinal(a.survivor_id, b.survivor_id));
            }
            return s;
        }

        /// <summary>Record one consumed food item. Called by the host on the
        /// canonical consumption path (inventory Consume / kitchen serving).</summary>
        public void RecordMeal(string survivorId, string itemId, int day)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var profile = ProfileFor(itemId);
            var s = Survivor(survivorId);
            s.entries.Add(new DietLogEntry
            {
                day = day,
                calories = profile.calories,
                protein = profile.protein,
                vitamin_c = profile.vitamin_c,
                micronutrients = profile.micronutrients,
                fats = profile.fats,
                fiber = profile.fiber
            });
        }

        /// <summary>Prune the window and re-evaluate deficiencies. Idempotent per day.</summary>
        public void TickDay(int day)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;

            var order = new List<SurvivorDietState>(_state.survivors);
            order.Sort((a, b) => string.CompareOrdinal(a.survivor_id, b.survivor_id));
            foreach (var s in order)
            {
                if (s == null) continue;
                if (s.entries.Count > WindowDays * 4)
                {
                    s.entries.RemoveAll(e => e != null && e.day < day - WindowDays);
                }
                else
                {
                    for (int i = s.entries.Count - 1; i >= 0; i--)
                        if (s.entries[i] != null && s.entries[i].day < day - WindowDays)
                            s.entries.RemoveAt(i);
                }

                foreach (var category in DiversityCategories)
                {
                    bool covered = false;
                    for (int i = 0; i < s.entries.Count; i++)
                    {
                        var e = s.entries[i];
                        if (e == null || e.day < day - WindowDays) continue;
                        if (e.Get(category) >= CoverageThreshold) { covered = true; break; }
                    }
                    bool was = s.deficient_categories.Contains(category);
                    if (!covered && !was)
                    {
                        s.deficient_categories.Add(category);
                        OnDeficiencyStarted?.Invoke(s.survivor_id, category);
                    }
                    else if (covered && was)
                    {
                        s.deficient_categories.Remove(category);
                        OnDeficiencyCleared?.Invoke(s.survivor_id, category);
                    }
                }
            }
        }

        public IReadOnlyList<string> Deficiencies(string survivorId)
        {
            var s = _state.survivors.Find(x => x != null && string.Equals(x.survivor_id, survivorId, StringComparison.Ordinal));
            return s?.deficient_categories ?? (IReadOnlyList<string>)Array.Empty<string>();
        }

        /// <summary>0..1 — fraction of diversity categories currently deficient.</summary>
        public float DeficiencyPressure(string survivorId)
        {
            int count = Deficiencies(survivorId).Count;
            return Math.Min(1f, count / (float)DiversityCategories.Length);
        }

        /// <summary>0..1 — fraction of diversity categories covered in-window.</summary>
        public float DiversityRatio(string survivorId)
        {
            var s = _state.survivors.Find(x => x != null && string.Equals(x.survivor_id, survivorId, StringComparison.Ordinal));
            if (s == null || s.entries.Count == 0) return 0f;
            int covered = 0;
            foreach (var category in DiversityCategories)
            {
                for (int i = 0; i < s.entries.Count; i++)
                {
                    var e = s.entries[i];
                    if (e == null || e.day < s.entries[s.entries.Count - 1].day - WindowDays) continue;
                    if (e.Get(category) >= CoverageThreshold) { covered++; break; }
                }
            }
            return covered / (float)DiversityCategories.Length;
        }

        // ------------------------------------------------------------------
        // Save
        // ------------------------------------------------------------------
        public NutritionDiversityState CaptureState()
        {
            var copy = new NutritionDiversityState { last_tick_day = _state.last_tick_day };
            foreach (var s in _state.survivors)
            {
                if (s == null) continue;
                var cs = new SurvivorDietState
                {
                    survivor_id = s.survivor_id,
                    deficient_categories = new List<string>(s.deficient_categories)
                };
                foreach (var e in s.entries)
                    cs.entries.Add(new DietLogEntry
                    {
                        day = e.day,
                        calories = e.calories,
                        protein = e.protein,
                        vitamin_c = e.vitamin_c,
                        micronutrients = e.micronutrients,
                        fats = e.fats,
                        fiber = e.fiber
                    });
                copy.survivors.Add(cs);
            }
            return copy;
        }

        public void RestoreState(NutritionDiversityState state)
        {
            if (state == null) return;
            _state.last_tick_day = state.last_tick_day;
            _state.survivors = new List<SurvivorDietState>(state.survivors?.Count ?? 0);
            if (state.survivors == null) return;
            foreach (var s in state.survivors)
            {
                if (s == null) continue;
                var cs = new SurvivorDietState
                {
                    survivor_id = s.survivor_id,
                    deficient_categories = s.deficient_categories != null
                        ? new List<string>(s.deficient_categories)
                        : new List<string>()
                };
                if (s.entries != null)
                    foreach (var e in s.entries)
                        cs.entries.Add(new DietLogEntry
                        {
                            day = e.day,
                            calories = e.calories,
                            protein = e.protein,
                            vitamin_c = e.vitamin_c,
                            micronutrients = e.micronutrients,
                            fats = e.fats,
                            fiber = e.fiber
                        });
                _state.survivors.Add(cs);
            }
        }
    }
}
