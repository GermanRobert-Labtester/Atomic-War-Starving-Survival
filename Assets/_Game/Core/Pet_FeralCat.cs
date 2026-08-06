using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FeralCatState
    {
        public string petId = "pet_feral_cat";
        public float pestReductionRate = 1.0f;
        public bool bringsDeadRats = true;
    }

    public class Pet_FeralCat
    {
        public event Action<string> OnPestReduced;
        public event Action<string, string> OnDeadRatDelivered;

        private FeralCatState _state;

        private const float DeadRatDeliveryChance = 0.25f;

        public Pet_FeralCat()
        {
            _state = new FeralCatState();
        }

        public Pet_FeralCat(FeralCatState state)
        {
            _state = state ?? new FeralCatState();
        }

        public FeralCatState CaptureState() => _state;

        public void RestoreState(FeralCatState state)
        {
            _state = state ?? new FeralCatState();
        }

        /// <summary>
        /// Called once per day. Reduces pests in the shelter and has a random
        /// chance to deliver a dead rat to one sleeping survivor.
        /// </summary>
        /// <param name="shelterId">The shelter the cat is dwelling in.</param>
        /// <param name="sleepingSurvivors">IDs of survivors currently asleep.</param>
        /// <param name="rng">Deterministic random source.</param>
        public void TickDay(string shelterId, List<string> sleepingSurvivors, Random rng)
        {
            if (string.IsNullOrEmpty(shelterId))
                return;

            // Passively reduce pest level to 0
            OnPestReduced?.Invoke(shelterId);

            // Random chance to deliver a dead rat to a sleeping survivor
            if (_state.bringsDeadRats
                && sleepingSurvivors != null
                && sleepingSurvivors.Count > 0
                && rng != null
                && rng.NextDouble() < DeadRatDeliveryChance)
            {
                int targetIndex = rng.Next(sleepingSurvivors.Count);
                string survivorId = sleepingSurvivors[targetIndex];
                OnDeadRatDelivered?.Invoke(_state.petId, survivorId);
            }
        }

        /// <summary>
        /// Returns the full pest-reduction value — the cat eliminates all pests.
        /// </summary>
        public float GetPestReduction() => _state.pestReductionRate;
    }
}
