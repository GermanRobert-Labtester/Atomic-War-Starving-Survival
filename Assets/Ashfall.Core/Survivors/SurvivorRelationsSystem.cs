using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class SurvivorRelation
    {
        public string survivorAId = string.Empty;
        public string survivorBId = string.Empty;
        public float affinity;
    }

    [Serializable]
    public sealed class SurvivorRelationsState
    {
        public List<SurvivorRelation> relations = new List<SurvivorRelation>();
    }

    /// <summary>
    /// Engine-agnostic pair-affinity authority used by social systems such as
    /// apprenticeship. Pair identity is order-independent and affinity is
    /// bounded to keep downstream modifiers stable.
    /// </summary>
    public sealed class SurvivorRelationsSystem
    {
        private SurvivorRelationsState _state = new SurvivorRelationsState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public SurvivorRelationsState State => _state;
        public event Action OnRelationsChanged;

        public SurvivorRelationsSystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public float GetAffinity(string survivorAId, string survivorBId)
        {
            var relation = Find(survivorAId, survivorBId);
            return relation == null ? 0f : relation.affinity;
        }

        public float ModifyAffinity(string survivorAId, string survivorBId, float delta)
        {
            if (string.IsNullOrEmpty(survivorAId) || string.IsNullOrEmpty(survivorBId) || survivorAId == survivorBId)
                return 0f;

            var relation = Find(survivorAId, survivorBId);
            if (relation == null)
            {
                CanonicalPair(survivorAId, survivorBId, out string a, out string b);
                relation = new SurvivorRelation { survivorAId = a, survivorBId = b };
                _state.relations.Add(relation);
            }

            relation.affinity = Math.Clamp(relation.affinity + delta, -100f, 100f);
            _log.Info($"[Relations] {relation.survivorAId}<->{relation.survivorBId}: {relation.affinity:F1}");
            OnRelationsChanged?.Invoke();
            return relation.affinity;
        }

        public void TickDay(int day)
        {
            // Reserved for deterministic daily social drift. Keeping the method
            // explicit lets host orchestration advance this authority without
            // introducing wall-clock behavior.
        }

        public SurvivorRelationsState CaptureState() => CloneState(_state);

        public void RestoreState(SurvivorRelationsState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            OnRelationsChanged?.Invoke();
        }

        private SurvivorRelation Find(string survivorAId, string survivorBId)
        {
            CanonicalPair(survivorAId, survivorBId, out string a, out string b);
            return _state.relations.Find(r => r.survivorAId == a && r.survivorBId == b);
        }

        private static void CanonicalPair(string first, string second, out string a, out string b)
        {
            if (string.CompareOrdinal(first, second) <= 0)
            {
                a = first ?? string.Empty;
                b = second ?? string.Empty;
            }
            else
            {
                a = second ?? string.Empty;
                b = first ?? string.Empty;
            }
        }

        private static SurvivorRelationsState CloneState(SurvivorRelationsState source)
        {
            var clone = new SurvivorRelationsState();
            if (source?.relations == null) return clone;
            foreach (var relation in source.relations)
            {
                if (relation == null) continue;
                clone.relations.Add(new SurvivorRelation
                {
                    survivorAId = relation.survivorAId ?? string.Empty,
                    survivorBId = relation.survivorBId ?? string.Empty,
                    affinity = relation.affinity
                });
            }
            return clone;
        }
    }
}
