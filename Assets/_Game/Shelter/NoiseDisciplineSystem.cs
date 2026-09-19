using System;
using System.Collections.Generic;
#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace AtomicWar._Game.Shelter
{
#if !UNITY_5_3_OR_NEWER
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int x;
        public int y;
        public Vector2Int(int x, int y) { this.x = x; this.y = y; }
        public bool Equals(Vector2Int other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is Vector2Int other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(x, y);
    }

    public static class Mathf
    {
        public static float Clamp01(float value) => Math.Clamp(value, 0f, 1f);
        public static float Max(float a, float b) => Math.Max(a, b);
        public static float Min(float a, float b) => Math.Min(a, b);
    }
#endif

    /// <summary>
    /// Noise Discipline (Spec #6 of Section VIII). The bunker makes noise.
    /// Aggregates all noise sources (generator, hammering, arguing, child,
    /// radio) with mitigation from depth, thumper module, and felt padding.
    /// Modifies raid probability as a function of current level.
    /// </summary>
    public class NoiseDisciplineSystem
    {
        [Serializable]
        public class Source
        {
            public string Id = string.Empty;
            public float Level;          // current contribution (0..100)
            public float BaseLevel;      // nominal contribution
            public Vector2Int Position;  // position in shelter grid
        }

        [Serializable]
        public class State
        {
            public List<Source> Sources = new List<Source>();
            public float MitigationFromDepth;     // 0..1
            public float MitigationFromThumper;    // 0..1
            public float MitigationFromPadding;    // 0..1
        }

        private State _state = new State();
        private readonly Dictionary<string, Source> _sourcesById = new Dictionary<string, Source>(StringComparer.Ordinal);

        public State Current => _state;

        public event Action<float> OnLevelChanged;          // current effective level
        public event Action<float> OnRaidProbabilityDelta;  // signed delta (e.g. +0.10 for +10%)

        public const float Silent = 0f;
        public const float Audible = 30f;
        public const float Loud = 60f;

        public NoiseDisciplineSystem()
        {
            RebuildLookup();
        }

        public void RegisterSource(string id, float noiseLevel, Vector2Int position)
        {
            if (string.IsNullOrEmpty(id)) return;

            if (_sourcesById.TryGetValue(id, out var existing))
            {
                existing.BaseLevel = noiseLevel;
                existing.Level = noiseLevel;
                existing.Position = position;
                Recompute();
                return;
            }

            var src = new Source { Id = id, BaseLevel = noiseLevel, Level = noiseLevel, Position = position };
            _state.Sources.Add(src);
            _sourcesById[id] = src;
            Recompute();
        }

        public void RegisterSource(string id, float baseLevel)
        {
            RegisterSource(id, baseLevel, default);
        }

        public Source GetSource(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _sourcesById.TryGetValue(id, out var src);
            return src;
        }

        public void SetSourceActive(string id, bool active)
        {
            if (string.IsNullOrEmpty(id)) return;

            if (!_sourcesById.TryGetValue(id, out var s)) return;
            s.Level = active ? s.BaseLevel : 0f;
            Recompute();
        }

        public void SetMitigationDepth(float fraction) { _state.MitigationFromDepth = Mathf.Clamp01(fraction); Recompute(); }
        public void SetMitigationThumper(float fraction) { _state.MitigationFromThumper = Mathf.Clamp01(fraction); Recompute(); }
        public void SetMitigationPadding(float fraction) { _state.MitigationFromPadding = Mathf.Clamp01(fraction); Recompute(); }

        public float CurrentLevel { get; private set; }

        public float GetRaidProbabilityDelta()
        {
            if (CurrentLevel >= Loud) return 0.35f;
            if (CurrentLevel >= Audible) return 0.10f;
            return 0f;
        }

        public string Severity
        {
            get
            {
                if (CurrentLevel < Audible) return "silent";
                if (CurrentLevel < Loud) return "audible";
                return "loud";
            }
        }

        public State CaptureState() => _state;

        public void RestoreState(State s)
        {
            _state = s ?? new State();
            RebuildLookup();
            Recompute();
        }

        private void RebuildLookup()
        {
            _sourcesById.Clear();
            if (_state.Sources == null)
            {
                _state.Sources = new List<Source>();
                return;
            }

            for (int i = 0; i < _state.Sources.Count; i++)
            {
                var src = _state.Sources[i];
                if (src != null && !string.IsNullOrEmpty(src.Id))
                {
                    _sourcesById[src.Id] = src;
                }
            }
        }

        private void Recompute()
        {
            float total = 0f;
            for (int i = 0; i < _state.Sources.Count; i++) total += _state.Sources[i].Level;
            float mit = Mathf.Clamp01(_state.MitigationFromDepth + _state.MitigationFromThumper + _state.MitigationFromPadding);
            float effective = Mathf.Max(0f, total * (1f - mit));
            CurrentLevel = effective;
            OnLevelChanged?.Invoke(CurrentLevel);
            OnRaidProbabilityDelta?.Invoke(GetRaidProbabilityDelta());
        }
    }
}
