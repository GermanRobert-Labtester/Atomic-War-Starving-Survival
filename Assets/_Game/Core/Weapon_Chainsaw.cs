using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public sealed class ChainsawState
    {
        public string weaponId = "weapon_chainsaw";
        public float fuelPerUse = 0.1f;
        public int noiseDecibels = 120;
        public bool ignoresArmor = true;
    }

    public readonly struct ChainsawAttackResult
    {
        public readonly float damage;
        public readonly bool fuelConsumed;

        public ChainsawAttackResult(float damage, bool fuelConsumed)
        {
            this.damage = damage;
            this.fuelConsumed = fuelConsumed;
        }
    }

    public sealed class Weapon_Chainsaw
    {
        // Base damage dealt per attack (armor-ignored).
        private const float BaseDamage = 80f;

        public event Action<string> OnChainsawUsed;   // (survivorId)
        public event Action<string> OnHordeSpawned;   // (nodeId)

        private ChainsawState _state = new ChainsawState();
        private string _pendingHordeNodeId = "";

        // Perform a chainsaw attack.
        // Returns damage dealt and whether fuel was consumed.
        // After combat ends the caller should invoke FlushHordeSpawn(nodeId).
        public ChainsawAttackResult Attack(string survivorId, string targetId, float currentFuel)
        {
            if (string.IsNullOrEmpty(survivorId))
                throw new ArgumentNullException(nameof(survivorId));

            if (currentFuel < _state.fuelPerUse)
            {
                // Not enough fuel — chainsaw sputters, no damage.
                return new ChainsawAttackResult(0f, false);
            }

            // Armor is fully ignored; deal base damage.
            float damage = BaseDamage;
            OnChainsawUsed?.Invoke(survivorId);
            return new ChainsawAttackResult(damage, true);
        }

        // Call this when the fight at the node ends to trigger the horde.
        // The 120 dB noise guarantees an instant Horde encounter.
        public void FlushHordeSpawn(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            _pendingHordeNodeId = "";
            OnHordeSpawned?.Invoke(nodeId);
        }

        public float GetFuelCost() => _state.fuelPerUse;
        public int GetNoiseLevel() => _state.noiseDecibels;

        public ChainsawState CaptureState() => new ChainsawState
        {
            weaponId = _state.weaponId,
            fuelPerUse = _state.fuelPerUse,
            noiseDecibels = _state.noiseDecibels,
            ignoresArmor = _state.ignoresArmor
        };

        public void RestoreState(ChainsawState saved)
        {
            _state.weaponId = saved.weaponId;
            _state.fuelPerUse = saved.fuelPerUse;
            _state.noiseDecibels = saved.noiseDecibels;
            _state.ignoresArmor = saved.ignoresArmor;
        }
    }
}
