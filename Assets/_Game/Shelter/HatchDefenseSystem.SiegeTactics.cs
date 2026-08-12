using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Siege Tactics — extends HatchDefenseSystem with tactical commands,
    /// deployable traps, and siege state tracking for Expansion 4 (#81-90).
    ///
    /// Partial class of HatchDefenseSystem.
    /// </summary>
    public partial class HatchDefenseSystem
    {
        // ── Siege state ────────────────────────────────────────────────
        public float HatchIntegrityPct = 1f;
        public int ReinforcementTier = 1; // 1=Wood, 2=Steel, 3=Composite
        public bool IsUnderSiege;
        public float BreachProgress;
        public int SurfaceSentryCount;
        public bool IsDeconFlushReady;

        public event Action<float> OnHatchIntegrityChanged;
        public event Action<int> OnReinforcementUpgraded;
        public event Action<bool> OnSiegeStateChanged;
        public event Action<float> OnBreachProgressChanged;
        public event Action<string, float> OnTacticalEffectActivated;

        /// <summary>Enter or leave siege. Fires <see cref="OnSiegeStateChanged"/>.</summary>
        public void SetUnderSiege(bool under)
        {
            if (IsUnderSiege == under) return;
            IsUnderSiege = under;
            if (!under) BreachProgress = 0f;
            OnSiegeStateChanged?.Invoke(under);
        }

        // ── Tactical effects tracking ──────────────────────────────────
        private readonly Dictionary<string, float> _activeEffects =
            new Dictionary<string, float>();

        public IReadOnlyDictionary<string, float> ActiveEffects => _activeEffects;

        /// <summary>Take damage to hatch integrity from breaching charges.</summary>
        public void TakeHatchDamage(float damage)
        {
            float tierMultiplier = ReinforcementTier switch
            {
                1 => 1.0f,
                2 => 0.6f,
                3 => 0.35f,
                _ => 1.0f
            };
            HatchIntegrityPct = Mathf.Clamp01(
                HatchIntegrityPct - (damage / 100f) * tierMultiplier);
            OnHatchIntegrityChanged?.Invoke(HatchIntegrityPct);
        }

        /// <summary>Repair hatch integrity.</summary>
        public void RepairHatch(float repairAmount)
        {
            HatchIntegrityPct = Mathf.Clamp01(
                HatchIntegrityPct + repairAmount / 100f);
            OnHatchIntegrityChanged?.Invoke(HatchIntegrityPct);
        }

        /// <summary>Upgrade hatch reinforcement tier.</summary>
        public bool UpgradeReinforcement(int newTier)
        {
            if (newTier <= ReinforcementTier || newTier > 3) return false;
            ReinforcementTier = newTier;
            OnReinforcementUpgraded?.Invoke(newTier);
            return true;
        }

        /// <summary>Deploy methane trap in surface entry tunnels.</summary>
        public bool DeployMethaneTrap(float fuelConsumed,
            Action<float> applyCO2Penalty)
        {
            if (fuelConsumed < 10f) return false;
            applyCO2Penalty?.Invoke(15f); // indoor CO spike
            _activeEffects["methane_trap"] = 2f; // 2 turns
            OnTacticalEffectActivated?.Invoke("methane_trap", 2f);
            return true;
        }

        /// <summary>Deploy gunports for defender cover bonus.</summary>
        public bool DeployGunports()
        {
            if (_activeEffects.ContainsKey("gunports")) return false;
            _activeEffects["gunports"] = 3f;
            OnTacticalEffectActivated?.Invoke("gunports", 3f);
            return true;
        }

        /// <summary>Deploy tear gas in entry corridors.</summary>
        public bool DeployTearGas()
        {
            if (_activeEffects.ContainsKey("tear_gas")) return false;
            _activeEffects["tear_gas"] = 2f;
            OnTacticalEffectActivated?.Invoke("tear_gas", 2f);
            return true;
        }

        /// <summary>Trigger controlled collapse to seal a tunnel permanently.</summary>
        public bool TriggerControlledCollapse(string tunnelId,
            Action<string> sealRoomAccess)
        {
            sealRoomAccess?.Invoke(tunnelId);
            _activeEffects["controlled_collapse"] = 999f; // permanent
            OnTacticalEffectActivated?.Invoke("controlled_collapse", 999f);
            return true;
        }

        /// <summary>Deploy barbed wire obstacles.</summary>
        public bool DeployBarbedWire()
        {
            if (_activeEffects.ContainsKey("barbed_wire")) return false;
            _activeEffects["barbed_wire"] = 3f;
            OnTacticalEffectActivated?.Invoke("barbed_wire", 3f);
            return true;
        }

        /// <summary>Deploy auto-turret consuming power and ammo.</summary>
        public bool DeployAutoTurret(float powerCost, int ammoCost,
            Func<float, bool> consumePower, Func<int, bool> consumeAmmo)
        {
            if (!consumePower(powerCost) || !consumeAmmo(ammoCost))
                return false;
            _activeEffects["auto_turret"] = 4f;
            OnTacticalEffectActivated?.Invoke("auto_turret", 4f);
            return true;
        }

        /// <summary>Decontamination flush on occupied airlock.</summary>
        public bool TriggerDeconFlush()
        {
            if (!IsDeconFlushReady) return false;
            IsDeconFlushReady = false;
            _activeEffects["decon_flush"] = 1f;
            OnTacticalEffectActivated?.Invoke("decon_flush", 1f);
            return true;
        }

        /// <summary>Assign sniper overwatch from surface watchtower.</summary>
        public bool AssignSniperOverwatch(string survivorId)
        {
            SurfaceSentryCount++;
            _activeEffects["sniper_overwatch"] = 6f;
            OnTacticalEffectActivated?.Invoke("sniper_overwatch", 6f);
            return true;
        }

        /// <summary>Issue a tactical command during defense.</summary>
        public void IssueCommand(string commandType)
        {
            switch (commandType)
            {
                case "hold_the_line":
                    _activeEffects["hold_the_line"] = 2f;
                    break;
                case "tactical_retreat":
                    _activeEffects["tactical_retreat"] = 1f;
                    break;
                case "suppressive_fire":
                    _activeEffects["suppressive_fire"] = 2f;
                    break;
            }
            OnTacticalEffectActivated?.Invoke(commandType,
                _activeEffects.GetValueOrDefault(commandType, 0f));
        }

        /// <summary>Tick active tactical effects and siege state.</summary>
        public void TickSiege(float gameHours)
        {
            if (!IsUnderSiege) return;

            var expired = new List<string>();
            foreach (var kv in _activeEffects)
            {
                float remaining = kv.Value - gameHours;
                if (remaining <= 0f)
                    expired.Add(kv.Key);
                else
                    _activeEffects[kv.Key] = remaining;
            }
            foreach (var key in expired)
                _activeEffects.Remove(key);
        }

        /// <summary>Get the current defense multiplier from active effects.</summary>
        public float GetTacticalDefenseMultiplier()
        {
            float mult = 1f;
            if (_activeEffects.ContainsKey("gunports")) mult += 0.3f;
            if (_activeEffects.ContainsKey("hold_the_line")) mult += 0.2f;
            if (_activeEffects.ContainsKey("barbed_wire")) mult += 0.15f;
            if (_activeEffects.ContainsKey("sniper_overwatch")) mult += 0.25f;
            if (_activeEffects.ContainsKey("auto_turret")) mult += 0.4f;
            if (_activeEffects.ContainsKey("suppressive_fire")) mult += 0.15f;
            if (_activeEffects.ContainsKey("tactical_retreat")) mult -= 0.1f;
            return Mathf.Max(0.5f, mult);
        }
    }
}
