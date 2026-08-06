using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WeaponBurstState
    {
        public string hazardId = "hazard_weapon_burst";
        public float burstChance = 0.5f;
    }

    public class Hazard_WeaponBurst
    {
        public event Action<string, string> OnWeaponBurst;
        public event Action<string> OnHandCrippled;

        private WeaponBurstState _state;

        public Hazard_WeaponBurst()
        {
            _state = new WeaponBurstState();
        }

        public Hazard_WeaponBurst(WeaponBurstState state)
        {
            _state = state ?? new WeaponBurstState();
        }

        public WeaponBurstState CaptureState() => _state;

        public void RestoreState(WeaponBurstState state)
        {
            _state = state ?? new WeaponBurstState();
        }

        public bool TryFire(string survivorId, string weaponId, string ammoTier, Random rng)
        {
            bool isPipeWeapon = weaponId != null && weaponId.StartsWith("pipe_");
            bool isMilitaryAmmo = ammoTier == "military";

            if (isPipeWeapon && isMilitaryAmmo)
            {
                double roll = rng.NextDouble();
                if (roll < _state.burstChance)
                {
                    OnWeaponBurst?.Invoke(survivorId, weaponId);
                    OnHandCrippled?.Invoke(survivorId);
                    return true;
                }
            }

            return false;
        }
    }
}
