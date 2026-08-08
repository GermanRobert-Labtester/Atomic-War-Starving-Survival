using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    public enum TankPhase
    {
        TreadsIntact,
        TreadsDestroyed,
        CrewFlushing,
        Destroyed
    }

    [Serializable]
    public sealed class WarlordTankState
    {
        public string encounterId = "encounter_warlord_tank";
        public TankPhase currentPhase = TankPhase.TreadsIntact;
        public bool smallArmsImmune = true;
    }

    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public sealed class Encounter_WarlordTank
    {
        // Phase-transition events
        public event Action<string> OnTreadsDestroyed;  // (attackerId)
        public event Action<string> OnCrewFlushed;      // (attackerId)
        public event Action<string> OnTankDestroyed;    // (attackerId)

        private WarlordTankState _state = new WarlordTankState();

        // Weapon types that can destroy treads (phase 0 → 1).
        private static readonly HashSet<string> AntiTreadWeapons = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "thermite", "c4", "rpg"
        };

        // Weapon types that flush the crew (phase 1 → 2 → 3).
        private static readonly HashSet<string> AntiCrewWeapons = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gas", "fire", "incendiary", "molotov"
        };

        // Attack with a specialised weapon. Returns true if the phase advanced.
        public bool AttackWithWeapon(string attackerId, string weaponType)
        {
            if (string.IsNullOrEmpty(attackerId))
                throw new ArgumentNullException(nameof(attackerId));
            if (string.IsNullOrEmpty(weaponType))
                return false;

            switch (_state.currentPhase)
            {
                case TankPhase.TreadsIntact:
                    if (AntiTreadWeapons.Contains(weaponType))
                    {
                        _state.currentPhase = TankPhase.TreadsDestroyed;
                        OnTreadsDestroyed?.Invoke(attackerId);
                        return true;
                    }
                    break;

                case TankPhase.TreadsDestroyed:
                    if (AntiCrewWeapons.Contains(weaponType))
                    {
                        _state.currentPhase = TankPhase.CrewFlushing;
                        OnCrewFlushed?.Invoke(attackerId);
                        // The crew-flush phase immediately transitions to destroyed.
                        _state.currentPhase = TankPhase.Destroyed;
                        OnTankDestroyed?.Invoke(attackerId);
                        return true;
                    }
                    break;

                case TankPhase.CrewFlushing:
                case TankPhase.Destroyed:
                    // Nothing more to do.
                    break;
            }

            return false;
        }

        // Small-arms fire always does 0 damage regardless of phase.
        public float AttackWithSmallArms()
        {
            return 0f;
        }

        public TankPhase GetPhase() => _state.currentPhase;

        public bool IsDestroyed() => _state.currentPhase == TankPhase.Destroyed;

        // --- Save / Load -----------------------------------------------------
        public WarlordTankState CaptureState() => new WarlordTankState
        {
            encounterId = _state.encounterId,
            currentPhase = _state.currentPhase,
            smallArmsImmune = _state.smallArmsImmune
        };

        public void RestoreState(WarlordTankState saved)
        {
            _state.encounterId = saved.encounterId;
            _state.currentPhase = saved.currentPhase;
            _state.smallArmsImmune = saved.smallArmsImmune;
        }
    }
}
