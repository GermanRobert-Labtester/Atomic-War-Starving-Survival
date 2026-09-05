// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class ReactorCoreState
    {
        public string coreInstanceId = string.Empty;
        public string profileId = string.Empty;
        public string outputSetting = "Normal"; // Shutdown, Low, Normal, High
        public string coolantState = "Sufficient"; // Sufficient, Restricted, Depleted
        public string heatState = "Nominal"; // Nominal, Elevated, Critical, Runaway
        public float shieldingIntegrity = 100.0f; // 0..100
        public float embrittlementWear; // 0..100
        public bool isScrammed;
        public bool isInstalled = true;
        public string roomId = "reactor_vault";
        public int lastTickDay;

        public ReactorCoreState Clone()
        {
            return new ReactorCoreState
            {
                coreInstanceId = coreInstanceId,
                profileId = profileId,
                outputSetting = outputSetting,
                coolantState = coolantState,
                heatState = heatState,
                shieldingIntegrity = shieldingIntegrity,
                embrittlementWear = embrittlementWear,
                isScrammed = isScrammed,
                isInstalled = isInstalled,
                roomId = roomId,
                lastTickDay = lastTickDay
            };
        }
    }

    [Serializable]
    public sealed class NuclearCoreLifecycleSave
    {
        public List<ReactorCoreState> installedCores = new List<ReactorCoreState>();
        public List<ReactorCoreState> spentCoreStorage = new List<ReactorCoreState>();
        public int lastTickDay;

        public NuclearCoreLifecycleSave Clone()
        {
            return new NuclearCoreLifecycleSave
            {
                installedCores = installedCores.Select(c => c.Clone()).ToList(),
                spentCoreStorage = spentCoreStorage.Select(c => c.Clone()).ToList(),
                lastTickDay = lastTickDay
            };
        }
    }

    public sealed class NuclearCoreLifecycleSystem
    {
        private readonly Inventory.Inventory _inventory;
        private readonly NuclearCoreCatalog _catalog;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private readonly Func<float, bool>? _coolantProvider; // Consumes clean water / coolant
        private readonly Action<string, float>? _onRadiationLeakage; // (roomId, dose)

        private readonly List<ReactorCoreState> _installedCores = new List<ReactorCoreState>();
        private readonly List<ReactorCoreState> _spentCoreStorage = new List<ReactorCoreState>();
        private int _lastTickDay;

        public IReadOnlyList<ReactorCoreState> InstalledCores => _installedCores;
        public IReadOnlyList<ReactorCoreState> SpentCoreStorage => _spentCoreStorage;
        public int LastTickDay => _lastTickDay;

        public event Action<string, string>? OnCoreInstalled;
        public event Action<string>? OnReactorScrammed;
        public event Action<string, string>? OnHeatStateChanged;
        public event Action<string, float>? OnRadiationLeak;

        public NuclearCoreLifecycleSystem(
            Inventory.Inventory inventory,
            NuclearCoreCatalog catalog,
            ISeededRng rng,
            ILog? log = null,
            Func<float, bool>? coolantProvider = null,
            Action<string, float>? onRadiationLeakage = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _coolantProvider = coolantProvider;
            _onRadiationLeakage = onRadiationLeakage;
        }

        public ReactorCoreState? GetCore(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId)) return null;
            return _installedCores.FirstOrDefault(c => string.Equals(c.coreInstanceId, instanceId, StringComparison.OrdinalIgnoreCase));
        }

        public bool TryInstallCore(string instanceId, string profileId, string roomId = "reactor_vault")
        {
            if (string.IsNullOrWhiteSpace(instanceId)) return false;
            if (GetCore(instanceId) != null) return false; // Already installed

            var profile = _catalog.GetProfile(profileId);
            if (profile == null) return false;

            var core = new ReactorCoreState
            {
                coreInstanceId = instanceId,
                profileId = profileId,
                outputSetting = profile.powerClass == "RTG" || profile.powerClass == "SealedCell" ? "Normal" : "Shutdown",
                coolantState = "Sufficient",
                heatState = "Nominal",
                shieldingIntegrity = 100.0f,
                embrittlementWear = 0.0f,
                isScrammed = false,
                isInstalled = true,
                roomId = roomId ?? "reactor_vault",
                lastTickDay = _lastTickDay
            };

            _installedCores.Add(core);
            _log.Info($"[NuclearCore] Installed core '{instanceId}' (profile: {profileId}) in room '{roomId}'.");
            OnCoreInstalled?.Invoke(instanceId, profileId);
            return true;
        }

        public bool SetOutputSetting(string instanceId, string setting)
        {
            var core = GetCore(instanceId);
            if (core == null || core.isScrammed) return false;

            if (setting != "Shutdown" && setting != "Low" && setting != "Normal" && setting != "High")
                return false;

            core.outputSetting = setting;
            return true;
        }

        public float GetTotalGenerationWatts()
        {
            float total = 0f;
            foreach (var core in _installedCores)
            {
                if (!core.isInstalled || core.isScrammed) continue;

                var profile = _catalog.GetProfile(core.profileId);
                if (profile == null) continue;

                float mult = core.outputSetting switch
                {
                    "Shutdown" => 0.0f,
                    "Low" => 0.5f,
                    "Normal" => 1.0f,
                    "High" => 1.5f,
                    _ => 0.0f
                };

                total += profile.baseElectricalOutput * mult;
            }
            return total;
        }

        public bool TryRepairShielding(string instanceId)
        {
            var core = GetCore(instanceId);
            if (core == null) return false;

            var bill = new InventoryBill();
            bill.AddCost("lead_sheet", 2);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                core.shieldingIntegrity = 100.0f;
            });

            return committed;
        }

        public bool TryEmergencyScram(string instanceId)
        {
            var core = GetCore(instanceId);
            if (core == null || core.isScrammed) return false;

            var profile = _catalog.GetProfile(core.profileId);
            if (profile == null) return false;

            var bill = new InventoryBill();
            bill.AddCost(profile.emergencyShutdownItemId, 1);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                core.isScrammed = true;
                core.outputSetting = "Shutdown";
                core.heatState = "Nominal";
                _log.Warn($"[NuclearCore] EMERGENCY SCRAM ACTIVATED on core '{instanceId}'.");
                OnReactorScrammed?.Invoke(instanceId);
            });

            return committed;
        }

        public void TickDay(int currentDay, Func<float, bool>? coolantOverride = null)
        {
            _lastTickDay = currentDay;
            var coolantCheck = coolantOverride ?? _coolantProvider;

            foreach (var core in _installedCores)
            {
                core.lastTickDay = currentDay;
                if (!core.isInstalled || core.isScrammed || core.outputSetting == "Shutdown")
                    continue;

                var profile = _catalog.GetProfile(core.profileId);
                if (profile == null) continue;

                float mult = core.outputSetting switch
                {
                    "Low" => 0.5f,
                    "Normal" => 1.0f,
                    "High" => 1.5f,
                    _ => 1.0f
                };

                // 1. Wear
                core.embrittlementWear = Math.Min(100.0f, core.embrittlementWear + (profile.wearRate * mult));

                // 2. Cooling & Heat
                if (profile.coolingDemand > 0f)
                {
                    float need = profile.coolingDemand * mult;
                    bool cooled = coolantCheck?.Invoke(need) ?? true;
                    if (!cooled)
                    {
                        core.coolantState = "Depleted";
                        string prevHeat = core.heatState;
                        core.heatState = prevHeat == "Nominal" ? "Elevated" : "Critical";
                        if (prevHeat != core.heatState)
                            OnHeatStateChanged?.Invoke(core.coreInstanceId, core.heatState);
                    }
                    else
                    {
                        core.coolantState = "Sufficient";
                        if (core.heatState != "Nominal")
                        {
                            core.heatState = "Nominal";
                            OnHeatStateChanged?.Invoke(core.coreInstanceId, core.heatState);
                        }
                    }
                }

                // 3. Shielding wear
                float shieldLoss = profile.wearRate * (core.heatState == "Nominal" ? 1.0f : 3.0f);
                core.shieldingIntegrity = Math.Max(0f, core.shieldingIntegrity - shieldLoss);

                // 4. Radiation Leakage
                if (core.shieldingIntegrity < profile.shieldingRequirement || core.heatState == "Critical")
                {
                    float leakDose = Math.Max(0f, (profile.shieldingRequirement - core.shieldingIntegrity) * 0.5f);
                    if (core.heatState == "Critical")
                        leakDose += 20.0f;

                    if (leakDose > 0f)
                    {
                        _onRadiationLeakage?.Invoke(core.roomId, leakDose);
                        OnRadiationLeak?.Invoke(core.coreInstanceId, leakDose);
                    }
                }
            }
        }

        public NuclearCoreLifecycleSave CaptureState()
        {
            return new NuclearCoreLifecycleSave
            {
                installedCores = _installedCores.Select(c => c.Clone()).ToList(),
                spentCoreStorage = _spentCoreStorage.Select(c => c.Clone()).ToList(),
                lastTickDay = _lastTickDay
            };
        }

        public void RestoreState(NuclearCoreLifecycleSave? save)
        {
            if (save == null) return;

            _lastTickDay = save.lastTickDay;

            _installedCores.Clear();
            if (save.installedCores != null)
            {
                foreach (var c in save.installedCores)
                    _installedCores.Add(c.Clone());
            }

            _spentCoreStorage.Clear();
            if (save.spentCoreStorage != null)
            {
                foreach (var c in save.spentCoreStorage)
                    _spentCoreStorage.Add(c.Clone());
            }
        }
    }
}
