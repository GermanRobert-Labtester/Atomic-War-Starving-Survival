using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.IO;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// One data-driven storm window entry from year_of_ash_storm_windows.json.
    /// Deterministic — no runtime randomness; the catalog is loaded once and queried by day.
    /// </summary>
    [Serializable]
    public class StormWindowEntry
    {
        public string id = string.Empty;
        public string phase = string.Empty;        // deep_freeze | faction_siege | great_thaw
        public int day_start;
        public int day_end;
        public string type = string.Empty;         // black_blizzard | ash_fallout | thermal_inversion | artillery_dust | ice_fog | thaw_flood
        public float intensity;                    // 0.0 – 1.0
        public float caloric_penalty;              // added on top of phase base caloric demand
        public float radon_spike;                  // additional radon infiltration rate while active
        public float faction_morale_penalty;       // faction morale decrement per active day
        public string description = string.Empty;
    }

    /// <summary>
    /// Loader for year_of_ash_storm_windows.json using the standard CatalogLocator pattern.
    /// </summary>
    public static class YearOfAshStormCatalogLoader
    {
        public const string FileName = "year_of_ash_storm_windows.json";

        public static List<StormWindowEntry> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(dataDir) || fileIO == null || json == null)
                return new List<StormWindowEntry>();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return new List<StormWindowEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return new List<StormWindowEntry>();

            try
            {
                var list = CatalogLocator.LoadWrappedList<StormWindowEntry>(raw, SystemTextJsonSerializer.Options);
                return list ?? new List<StormWindowEntry>();
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "StormWindowEntry list", ex_CATDIAG);
                return new List<StormWindowEntry>();
            }
        }
    }

    /// <summary>
    /// Side-effect-free queries over a loaded storm catalog.
    /// All methods are deterministic given the same catalog and day.
    /// </summary>
    public static class StormWindowQuery
    {
        /// <summary>Returns all storm windows that are active on the given day (inclusive range).</summary>
        public static List<StormWindowEntry> GetActiveWindowsForDay(IReadOnlyList<StormWindowEntry> catalog, int day)
        {
            var result = new List<StormWindowEntry>();
            if (catalog == null) return result;
            for (int i = 0; i < catalog.Count; i++)
            {
                var e = catalog[i];
                if (e != null && day >= e.day_start && day <= e.day_end)
                    result.Add(e);
            }
            return result;
        }

        /// <summary>
        /// Returns the combined caloric penalty multiplier for all active storm windows on a day.
        /// Penalties are additive — if two storms overlap the penalties sum.
        /// </summary>
        public static float GetCaloricPenaltyForDay(IReadOnlyList<StormWindowEntry> catalog, int day)
        {
            float total = 0f;
            var active = GetActiveWindowsForDay(catalog, day);
            for (int i = 0; i < active.Count; i++)
                total += active[i].caloric_penalty;
            return total;
        }

        /// <summary>
        /// Returns the combined radon spike for all active storm windows on a day.
        /// Added to the phase's base radon infiltration rate.
        /// </summary>
        public static float GetRadonSpikeForDay(IReadOnlyList<StormWindowEntry> catalog, int day)
        {
            float total = 0f;
            var active = GetActiveWindowsForDay(catalog, day);
            for (int i = 0; i < active.Count; i++)
                total += active[i].radon_spike;
            return total;
        }

        /// <summary>Returns the combined faction morale penalty for all active storm windows on a day.</summary>
        public static float GetFactionMoralePenaltyForDay(IReadOnlyList<StormWindowEntry> catalog, int day)
        {
            float total = 0f;
            var active = GetActiveWindowsForDay(catalog, day);
            for (int i = 0; i < active.Count; i++)
                total += active[i].faction_morale_penalty;
            return total;
        }

        /// <summary>
        /// Returns true if any active storm of type "thaw_flood" or "thermal_inversion" is
        /// present on the given day (used by the ice road system to close the road).
        /// </summary>
        public static bool HasIceRoadBlockingStorm(IReadOnlyList<StormWindowEntry> catalog, int day)
        {
            var active = GetActiveWindowsForDay(catalog, day);
            for (int i = 0; i < active.Count; i++)
            {
                string t = active[i].type;
                if (t == "thaw_flood" || t == "thermal_inversion")
                    return true;
            }
            return false;
        }
    }
}
