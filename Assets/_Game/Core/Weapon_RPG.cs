using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RPGState
    {
        public string weaponId = "weapon_rpg";
        public float securityDamagePercent = 0.5f;
        public bool concussesAirlockOccupants = true;
    }

    public class Weapon_RPG
    {
        public event Action<string, float> OnHatchHit;
        public event Action<string> OnAirlockOccupantConcussed;
        public event Action<string> OnSurvivorDeafened;

        private RPGState _state;

        public Weapon_RPG()
        {
            _state = new RPGState();
        }

        public Weapon_RPG(RPGState state)
        {
            _state = state ?? new RPGState();
        }

        public RPGState CaptureState() => _state;

        public void RestoreState(RPGState state)
        {
            _state = state ?? new RPGState();
        }

        /// <summary>
        /// Fires an RPG at the shelter hatch during a raid.
        /// Drops ShelterSecurity by 50%. Anyone inside the airlock is
        /// concussed and deafened by the blast wave.
        /// </summary>
        /// <param name="attackerId">The warlord/raider firing the RPG.</param>
        /// <param name="airlockOccupantIds">Survivors currently inside the airlock.</param>
        public void FireAtHatch(string attackerId, List<string> airlockOccupantIds)
        {
            if (string.IsNullOrEmpty(attackerId))
                return;

            // Hatch hit — 50% security drop
            OnHatchHit?.Invoke(attackerId, _state.securityDamagePercent);

            // Concuss and deafen everyone in the airlock
            if (_state.concussesAirlockOccupants
                && airlockOccupantIds != null)
            {
                for (int i = 0; i < airlockOccupantIds.Count; i++)
                {
                    string occupantId = airlockOccupantIds[i];
                    if (string.IsNullOrEmpty(occupantId))
                        continue;

                    OnAirlockOccupantConcussed?.Invoke(occupantId);
                    OnSurvivorDeafened?.Invoke(occupantId);
                }
            }
        }

        /// <summary>
        /// Returns the fraction of current shelter security destroyed by an RPG hit.
        /// </summary>
        public float GetSecurityDamage() => _state.securityDamagePercent;
    }
}
