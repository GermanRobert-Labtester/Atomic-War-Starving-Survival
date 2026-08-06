using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiegeHostageShieldState
    {
        public string siegeId = "siege_hostage_shield";
        public int hostagesCount;
        public int hostagesKilled;
        public int hostagesRescued;
        public string weaponTypeUsed = string.Empty;
    }

    /// <summary>
    /// Prompt #823: Hostage Shields. Raiders strap captured civilians to
    /// their armor. Firing Turrets or AssaultRifles kills civilians (massive
    /// Morale drop). Player must use Melee or Sniper precision to avoid
    /// collateral deaths.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_HostageShield
    {
        private SiegeHostageShieldState _state = new SiegeHostageShieldState();

        private const float MoralePerHostageKilled = 30f;
        private const float SniperPrecisionThreshold = 0.8f;

        // -- Events --
        public event Action<int> OnSiegeStarted;          // hostage count
        public event Action<int> OnHostageKilled;         // total killed
        public event Action<int> OnHostageRescued;        // total rescued
        public event Action<float> OnMoralePenalty;       // morale penalty amount

        public SiegeHostageShieldState State => _state;

        /// <summary>
        /// Raiders appear with civilians strapped to their armor.
        /// </summary>
        /// <param name="hostageCount">Number of human shields.</param>
        public void StartSiege(int hostageCount)
        {
            _state.hostagesCount = hostageCount;
            _state.hostagesKilled = 0;
            _state.hostagesRescued = 0;
            _state.weaponTypeUsed = string.Empty;

            OnSiegeStarted?.Invoke(hostageCount);
        }

        /// <summary>
        /// Resolve an attack against the raiders using a specific weapon type.
        /// <list type="bullet">
        ///   <item>Turrets / AssaultRifles: kill all remaining hostages.</item>
        ///   <item>Melee: safe for hostages but risky to the attacker.</item>
        ///   <item>Sniper (precision >= 0.8): hostages are rescued.</item>
        /// </list>
        /// </summary>
        /// <param name="weaponType">
        /// One of "turret", "assault_rifle", "melee", or "sniper".
        /// </param>
        /// <param name="precisionSkill">
        /// Sniper precision skill [0–1]. Only matters for "sniper".
        /// </param>
        /// <returns>
        /// Tuple of (hostagesKilledThisAttack, hostagesRescuedThisAttack).
        /// </returns>
        public (int killed, int rescued) ResolveAttack(string weaponType, float precisionSkill)
        {
            int remaining = _state.hostagesCount - _state.hostagesKilled - _state.hostagesRescued;
            if (remaining <= 0) return (0, 0);

            _state.weaponTypeUsed = weaponType ?? string.Empty;
            int killedThisAttack = 0;
            int rescuedThisAttack = 0;

            switch (weaponType)
            {
                case "turret":
                case "assault_rifle":
                    // Indiscriminate fire — all remaining hostages die
                    killedThisAttack = remaining;
                    _state.hostagesKilled += killedThisAttack;
                    OnHostageKilled?.Invoke(_state.hostagesKilled);

                    float penalty = GetMoralePenalty();
                    OnMoralePenalty?.Invoke(penalty);
                    break;

                case "melee":
                    // Close-quarters — hostages survive, but attacker is at risk.
                    // No hostages killed, none rescued yet (they're freed after
                    // the raiders are dealt with externally).
                    rescuedThisAttack = remaining;
                    _state.hostagesRescued += rescuedThisAttack;
                    OnHostageRescued?.Invoke(_state.hostagesRescued);
                    break;

                case "sniper":
                    if (precisionSkill >= SniperPrecisionThreshold)
                    {
                        // Precision shots — raiders down, hostages rescued
                        rescuedThisAttack = remaining;
                        _state.hostagesRescued += rescuedThisAttack;
                        OnHostageRescued?.Invoke(_state.hostagesRescued);
                    }
                    else
                    {
                        // Missed — stray bullets kill hostages
                        killedThisAttack = remaining;
                        _state.hostagesKilled += killedThisAttack;
                        OnHostageKilled?.Invoke(_state.hostagesKilled);

                        float sniperPenalty = GetMoralePenalty();
                        OnMoralePenalty?.Invoke(sniperPenalty);
                    }
                    break;

                default:
                    // Unknown weapon — treat like indiscriminate fire
                    killedThisAttack = remaining;
                    _state.hostagesKilled += killedThisAttack;
                    OnHostageKilled?.Invoke(_state.hostagesKilled);

                    float defaultPenalty = GetMoralePenalty();
                    OnMoralePenalty?.Invoke(defaultPenalty);
                    break;
            }

            return (killedThisAttack, rescuedThisAttack);
        }

        /// <summary>
        /// Morale penalty equals hostagesKilled * 30. Devastating.
        /// </summary>
        public float GetMoralePenalty()
        {
            return _state.hostagesKilled * MoralePerHostageKilled;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeHostageShieldState CaptureState()
        {
            return new SiegeHostageShieldState
            {
                siegeId = _state.siegeId,
                hostagesCount = _state.hostagesCount,
                hostagesKilled = _state.hostagesKilled,
                hostagesRescued = _state.hostagesRescued,
                weaponTypeUsed = _state.weaponTypeUsed
            };
        }

        public void RestoreState(SiegeHostageShieldState saved)
        {
            _state = saved ?? new SiegeHostageShieldState();
        }
    }
}
