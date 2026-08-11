using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Capture/restore helpers for the id-keyed state maps that the affliction,
    /// hazard, and vehicle systems keep (<c>Dictionary&lt;string, TState&gt;</c> keyed
    /// by survivorId / locationId / vehicleId / routeId).
    ///
    /// These systems all shipped with stub save methods — <c>CaptureState()</c>
    /// returned a DTO holding nothing but its own id and <c>RestoreState()</c>
    /// discarded its argument — so every tetanus case, frostbite, cataract, stranded
    /// vehicle, and blocked route silently vanished on load. The shape was identical
    /// in all of them, so it is written once here rather than fifteen times.
    ///
    /// JsonUtility cannot serialize a Dictionary, which is why the DTO side is a
    /// <c>List&lt;TState&gt;</c>: the key is already carried on each entry.
    /// </summary>
    public static class SaveMap
    {
        /// <summary>
        /// Flatten a state map into a serializable list. Null entries are dropped —
        /// they carry no key and would only fail on the way back in.
        /// </summary>
        public static List<T> Capture<T>(Dictionary<string, T> map) where T : class
        {
            var list = new List<T>(map?.Count ?? 0);
            if (map == null) return list;
            foreach (T value in map.Values)
            {
                if (value != null) list.Add(value);
            }
            return list;
        }

        /// <summary>
        /// Repopulate a state map from a saved list. The map is cleared first: loading
        /// a save replaces the world, it does not merge into whatever the current
        /// session happened to be holding.
        ///
        /// A null <paramref name="saved"/> still clears — that is what an older save
        /// without this section means, and leaving stale live entries behind would
        /// resurrect afflictions the save says are gone.
        /// </summary>
        public static void Restore<T>(Dictionary<string, T> map, List<T> saved, Func<T, string> keyOf)
            where T : class
        {
            if (map == null || keyOf == null) return;
            map.Clear();
            if (saved == null) return;

            for (int i = 0; i < saved.Count; i++)
            {
                T entry = saved[i];
                if (entry == null) continue;
                string key = keyOf(entry);
                if (string.IsNullOrEmpty(key)) continue; // unkeyed entry: nothing could ever look it up
                map[key] = entry;
            }
        }
    }
}
