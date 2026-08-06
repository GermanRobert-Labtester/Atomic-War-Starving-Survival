using System;

namespace AtomicWar._Game.Core
{
    public enum CraftResult
    {
        Success,
        Detonation,
        Fail
    }

    [Serializable]
    public class ExplosiveCraftingState
    {
        public string craftingId = "hazard_explosive_crafting";
        public float fatigueThreshold = 0.7f;
        public float skillThreshold = 0.3f;
        public float detonationChance = 0.1f;
        public bool isActive;
    }

    public class Hazard_ExplosiveCrafting
    {
        public event Action<string, string> OnDetonation;
        public event Action<string> OnSafeCraft;

        private ExplosiveCraftingState _state;

        public Hazard_ExplosiveCrafting()
        {
            _state = new ExplosiveCraftingState();
        }

        public Hazard_ExplosiveCrafting(ExplosiveCraftingState state)
        {
            _state = state ?? new ExplosiveCraftingState();
        }

        public ExplosiveCraftingState CaptureState() => _state;

        public void RestoreState(ExplosiveCraftingState state)
        {
            _state = state ?? new ExplosiveCraftingState();
        }

        public CraftResult TryCraft(string survivorId, float fatigue, float chemistrySkill, string roomId, Random rng)
        {
            if (!_state.isActive)
                return CraftResult.Fail;

            bool isRisky = fatigue > _state.fatigueThreshold || chemistrySkill < _state.skillThreshold;

            if (isRisky)
            {
                double roll = rng.NextDouble();
                if (roll < _state.detonationChance)
                {
                    OnDetonation?.Invoke(survivorId, roomId);
                    return CraftResult.Detonation;
                }
            }

            OnSafeCraft?.Invoke(survivorId);
            return CraftResult.Success;
        }
    }
}
