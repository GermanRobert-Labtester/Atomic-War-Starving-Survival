using System;
using System.Collections.Generic;
using UnityEngine; // for Mathf (Prompt #123 — early-detection chance)
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #123 — Perimeter traps: BearTraps, TinCanAlarms, Tripwires. 2h raid warning.
    /// Prompt #186 — Trap Setter: no misfire, 2× damage, perfect wasteland disarm.</summary>
    public class PerimeterTrapSystem
    {
        public const string BearTrapItemId = "bear_trap";
        public const string TinCanAlarmItemId = "tin_can_alarm";
        public const string TripwireItemId = "tripwire";
        public const float RaidWarningHours = 2f;
        public const float BaseTrapDamage = 20f;

        private int _bearTraps, _tinCanAlarms, _tripwires;
        private float _raidWarningRemaining;
        private bool _raidWarningActive;
        private string _lastDeployerId;
        private CombatPerkSystem _combatPerks;
        private Func<string, Survivor> _getSurvivor;
        private System.Random _rng = new System.Random(123);

        public int BearTraps => _bearTraps;
        public int TinCanAlarms => _tinCanAlarms;
        public int Tripwires => _tripwires;
        public bool HasActiveWarning => _raidWarningActive;
        public float WarningHoursRemaining => _raidWarningRemaining;
        public string LastDeployerId => _lastDeployerId;

        public event Action OnTrapDeployed;
        public event Action OnRaidEarlyWarning;
        public event Action<string, int> OnTrapDeployedBy; // survivorId, count
        public event Action<float> OnTrapDamagedRaiders; // damage dealt

        public void BindCombatPerks(CombatPerkSystem combatPerks, Func<string, Survivor> getSurvivor = null)
        {
            _combatPerks = combatPerks;
            _getSurvivor = getSurvivor;
        }

        public void SetRng(System.Random rng) => _rng = rng ?? new System.Random(123);

        public void DeployTrap(string trapItemId, int count = 1)
        {
            DeployTrap(trapItemId, deployer: null, count: count, currentDay: 0);
        }

        /// <summary>Deploy traps, optionally attributing to a survivor for Trap Setter milestones.</summary>
        public void DeployTrap(string trapItemId, Survivor deployer, int count = 1, int currentDay = 0)
        {
            if (count <= 0) return;
            switch (trapItemId)
            {
                case BearTrapItemId: _bearTraps += count; break;
                case TinCanAlarmItemId: _tinCanAlarms += count; break;
                case TripwireItemId: _tripwires += count; break;
                default: return;
            }
            if (deployer != null)
            {
                _lastDeployerId = deployer.Id;
                _combatPerks?.RecordTrapDeployed(deployer, count, currentDay);
                OnTrapDeployedBy?.Invoke(deployer.Id, count);
            }
            OnTrapDeployed?.Invoke();
        }

        /// <summary>Chance to detect an incoming raid early. Scales with deployed traps.</summary>
        public float EarlyDetectionChance => Mathf.Clamp01((_bearTraps * 0.15f + _tinCanAlarms * 0.25f + _tripwires * 0.2f) / 5f);

        /// <summary>Trap Setter: 0% premature misfire; others use default chance.</summary>
        public float GetMisfireChance()
        {
            var deployer = ResolveLastDeployer();
            return _combatPerks != null
                ? _combatPerks.GetTrapMisfireChance(deployer)
                : CombatPerkSystem.DefaultTrapMisfireChance;
        }

        /// <summary>Roll premature misfire for a deployed trap stack. True = wasted.</summary>
        public bool RollPrematureMisfire()
        {
            float chance = GetMisfireChance();
            if (chance <= 0f) return false;
            return _rng.NextDouble() < chance;
        }

        /// <summary>Damage dealt to a raiding party by traps (2× with Trap Setter).</summary>
        public float GetTrapDamageAgainstRaiders()
        {
            int total = _bearTraps + _tripwires; // alarms warn, don't shred
            if (total <= 0) return 0f;
            var deployer = ResolveLastDeployer();
            float mult = _combatPerks != null
                ? _combatPerks.GetTrapDamageMultiplier(deployer)
                : 1f;
            float dmg = BaseTrapDamage * total * mult;
            OnTrapDamagedRaiders?.Invoke(dmg);
            return dmg;
        }

        /// <summary>Wasteland trap disarm (100% with Trap Setter).</summary>
        public bool TryDisarmWastelandTrap(Survivor scavenger)
        {
            if (_combatPerks != null)
                return _combatPerks.TryDisarmWastelandTrap(scavenger, _rng);
            return _rng.NextDouble() < CombatPerkSystem.DefaultDisarmSuccess;
        }

        /// <summary>When a raid is detected early, set the warning timer.</summary>
        public void TriggerEarlyWarning()
        {
            _raidWarningActive = true;
            _raidWarningRemaining = RaidWarningHours;
            // Traps are consumed on use.
            _bearTraps = Math.Max(0, _bearTraps - 1);
            _tinCanAlarms = Math.Max(0, _tinCanAlarms - 1);
            _tripwires = Math.Max(0, _tripwires - 1);
            OnRaidEarlyWarning?.Invoke();
        }

        public void Tick(float gameHours)
        {
            if (!_raidWarningActive) return;
            _raidWarningRemaining -= gameHours;
            if (_raidWarningRemaining <= 0f) _raidWarningActive = false;
        }

        private Survivor ResolveLastDeployer()
        {
            if (string.IsNullOrEmpty(_lastDeployerId) || _getSurvivor == null) return null;
            return _getSurvivor(_lastDeployerId);
        }

        public PerimeterTrapSave CaptureState()
        {
            return new PerimeterTrapSave
            {
                BearTraps = _bearTraps, TinCanAlarms = _tinCanAlarms, Tripwires = _tripwires,
                RaidWarningActive = _raidWarningActive, RaidWarningRemaining = _raidWarningRemaining,
                LastDeployerId = _lastDeployerId
            };
        }
        public void RestoreState(PerimeterTrapSave save)
        {
            _bearTraps = _tinCanAlarms = _tripwires = 0;
            _raidWarningActive = false; _raidWarningRemaining = 0f;
            _lastDeployerId = null;
            if (save == null) return;
            _bearTraps = save.BearTraps; _tinCanAlarms = save.TinCanAlarms; _tripwires = save.Tripwires;
            _raidWarningActive = save.RaidWarningActive; _raidWarningRemaining = save.RaidWarningRemaining;
            _lastDeployerId = save.LastDeployerId;
        }
    }
    [Serializable] public class PerimeterTrapSave
    {
        public int BearTraps, TinCanAlarms, Tripwires;
        public bool RaidWarningActive;
        public float RaidWarningRemaining;
        public string LastDeployerId;
    }
}
