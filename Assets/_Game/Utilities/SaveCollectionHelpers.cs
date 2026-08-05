using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Shared capture/restore helpers for dictionary + set save DTOs.
    /// Keeps CeilingCollapse / Mutagenesis-style snapshots free of duplicated loops.
    /// </summary>
    public static class SaveCollectionHelpers
    {
        public struct IntClamp
        {
            public int DefaultValue;
            public int Min;
            public int Max;

            public static IntClamp None => new IntClamp
            {
                DefaultValue = 0,
                Min = int.MinValue,
                Max = int.MaxValue
            };

            public static IntClamp Range(int min, int max, int defaultValue = 0) => new IntClamp
            {
                DefaultValue = defaultValue,
                Min = min,
                Max = max
            };
        }

        public static void CaptureStringFloatDict(
            Dictionary<string, float> source,
            out string[] keys,
            out float[] values)
        {
            CaptureDict(source, out keys, out values, 0f);
        }

        public static void CaptureStringIntDict(
            Dictionary<string, int> source,
            out string[] keys,
            out int[] values)
        {
            CaptureDict(source, out keys, out values, 0);
        }

        private static void CaptureDict<T>(
            Dictionary<string, T> source,
            out string[] keys,
            out T[] values,
            T empty)
        {
            if (source == null || source.Count == 0)
            {
                keys = Array.Empty<string>();
                values = Array.Empty<T>();
                return;
            }

            keys = new string[source.Count];
            values = new T[source.Count];
            int i = 0;
            foreach (var kv in source)
            {
                keys[i] = kv.Key;
                values[i] = kv.Value;
                i++;
            }
        }

        public static string[] CaptureStringSet(HashSet<string> source)
        {
            if (source == null || source.Count == 0)
                return Array.Empty<string>();

            var arr = new string[source.Count];
            source.CopyTo(arr);
            return arr;
        }

        public static void RestoreStringFloatDict(
            Dictionary<string, float> target,
            string[] keys,
            float[] values,
            float defaultValue = 1f)
        {
            target.Clear();
            if (keys == null) return;
            for (int i = 0; i < keys.Length; i++)
            {
                if (string.IsNullOrEmpty(keys[i])) continue;
                float v = values != null && i < values.Length ? values[i] : defaultValue;
                target[keys[i]] = v;
            }
        }

        public static void RestoreStringIntDict(
            Dictionary<string, int> target,
            string[] keys,
            int[] values,
            IntClamp clamp)
        {
            target.Clear();
            if (keys == null) return;
            for (int i = 0; i < keys.Length; i++)
            {
                if (string.IsNullOrEmpty(keys[i])) continue;
                int v = values != null && i < values.Length ? values[i] : clamp.DefaultValue;
                if (v < clamp.Min) v = clamp.Min;
                if (v > clamp.Max) v = clamp.Max;
                target[keys[i]] = v;
            }
        }

        public static void RestoreStringSet(HashSet<string> target, string[] ids)
        {
            target.Clear();
            if (ids == null) return;
            for (int i = 0; i < ids.Length; i++)
            {
                if (!string.IsNullOrEmpty(ids[i]))
                    target.Add(ids[i]);
            }
        }
    }
}
