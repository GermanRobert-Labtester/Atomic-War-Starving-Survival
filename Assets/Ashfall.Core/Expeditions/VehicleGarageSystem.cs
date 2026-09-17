// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class VehicleCustomizationRecord
    {
        public string vehicleId = string.Empty;
        public Dictionary<string, string> installedSlots = new Dictionary<string, string>(StringComparer.Ordinal);
        public int chassisStressPermille;
        public int engineFoulingPermille;
        public int transmissionWearPermille;
        public bool isImmobilized;
        public string immobilizedReason = string.Empty;
    }

    [Serializable]
    public sealed class VehicleRecoveryMission
    {
        public string missionId = string.Empty;
        public string strandedVehicleId = string.Empty;
        public string locationId = string.Empty;
        public int requiredFuelUnits = 10;
        public int progressTicks;
        public int requiredTicks = 120;
        public bool isComplete;
    }

    [Serializable]
    public sealed class VehicleGarageState
    {
        public string systemId = VehicleGarageSystem.SystemId;
        public Dictionary<string, VehicleCustomizationRecord> vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>(StringComparer.Ordinal);
        public Dictionary<string, VehicleRecoveryMission> activeRecoveries = new Dictionary<string, VehicleRecoveryMission>(StringComparer.Ordinal);
        public int nextRecoveryCounter = 1;
    }

    public sealed class VehicleGarageSystem
    {
        public const string SystemId = "vehicle_garage";

        public const string SlotCargo = "cargo";
        public const string SlotProtection = "protection";
        public const string SlotMobility = "mobility";
        public const string SlotEngine = "engine";
        public const string SlotUtility = "utility";

        private readonly Dictionary<string, VehicleModificationDefinition> _modCatalog =
            new Dictionary<string, VehicleModificationDefinition>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private VehicleGarageState _state = new VehicleGarageState();

        public VehicleGarageSystem(VehicleGarageCatalog? catalog = null, ISeededRng? rng = null)
        {
            _rng = rng ?? new SeededRng(1337);
            if (catalog != null)
            {
                LoadCatalog(catalog);
            }
        }

        public void LoadCatalog(VehicleGarageCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            _modCatalog.Clear();
            foreach (var mod in catalog.modifications)
            {
                if (!string.IsNullOrEmpty(mod.id))
                {
                    _modCatalog[mod.id] = mod;
                }
            }
        }

        public bool HasModification(string modId) => _modCatalog.ContainsKey(modId);

        public VehicleModificationDefinition? GetModification(string modId)
        {
            _modCatalog.TryGetValue(modId, out var def);
            return def;
        }

        public IReadOnlyDictionary<string, VehicleModificationDefinition> GetAllModifications() => _modCatalog;

        /// <summary>Read-only customization record for a vehicle, or null when it has none yet.</summary>
        public VehicleCustomizationRecord? GetRecord(string vehicleId) =>
            !string.IsNullOrEmpty(vehicleId) && _state.vehicleRecords.TryGetValue(vehicleId, out var record) ? record : null;

        /// <summary>True when the vehicle is stranded/mission-critical-down in the garage.</summary>
        public bool IsImmobilized(string vehicleId) => GetRecord(vehicleId)?.isImmobilized == true;

        /// <summary>Active recovery missions (read-only).</summary>
        public IReadOnlyDictionary<string, VehicleRecoveryMission> ActiveRecoveries => _state.activeRecoveries;

        private static readonly Dictionary<string, string> EmptySlots = new Dictionary<string, string>(StringComparer.Ordinal);

        /// <summary>Installed slot→modification map for a vehicle (empty when none).</summary>
        public IReadOnlyDictionary<string, string> GetInstalledSlots(string vehicleId) =>
            GetRecord(vehicleId)?.installedSlots ?? EmptySlots;

        /// <summary>
        /// Apply the installed-modification effects to an expedition profile
        /// built by <see cref="ExpeditionVehicleSystem.CreateExpeditionProfile"/>.
        /// The garage decorates; the expedition core stays decoupled and owns
        /// travel. Read-only over persisted state; zero RNG.
        /// </summary>
        public void DecorateProfile(ExpeditionVehicleProfile? profile)
        {
            if (profile == null) return;
            string vehicleId = profile.vehicleId;
            if (string.IsNullOrEmpty(vehicleId)) return;

            profile.cargoCapacityKg += GetEffectiveCargoCapacityDelta(vehicleId);
            profile.speedMultiplier = Math.Max(0.01f, profile.speedMultiplier * (1f + GetEffectiveSpeedMultiplierDelta(vehicleId)));
            profile.fuelPerTravelTick = Math.Max(0f, profile.fuelPerTravelTick * GetEffectiveFuelConsumptionMultiplier(vehicleId));
        }

        /// <summary>
        /// Advance every active recovery mission by the given ticks. Missions
        /// complete at their authored <see cref="VehicleRecoveryMission.requiredTicks"/>
        /// threshold; completion itself remains the player's command
        /// (<see cref="CompleteRecoveryMission"/>). Deterministic; no RNG.
        /// </summary>
        public int AdvanceRecoveries(int deltaTicks)
        {
            if (deltaTicks <= 0) return 0;
            int advanced = 0;
            foreach (var mission in _state.activeRecoveries.Values)
            {
                if (mission == null || mission.isComplete) continue;
                mission.progressTicks += deltaTicks;
                if (mission.progressTicks >= mission.requiredTicks)
                    mission.isComplete = true;
                advanced++;
            }
            return advanced;
        }

        public VehicleCustomizationRecord GetOrCreateRecord(string vehicleId)
        {
            if (string.IsNullOrEmpty(vehicleId))
                throw new ArgumentException("Vehicle ID cannot be null or empty.", nameof(vehicleId));

            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record))
            {
                record = new VehicleCustomizationRecord { vehicleId = vehicleId };
                _state.vehicleRecords[vehicleId] = record;
            }
            return record;
        }

        public bool HasVehicleRecord(string vehicleId) =>
            !string.IsNullOrEmpty(vehicleId) && _state.vehicleRecords.ContainsKey(vehicleId);

        public bool CanInstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason)
        {
            reason = string.Empty;
            if (string.IsNullOrEmpty(vehicleId))
            {
                reason = "Invalid vehicle ID.";
                return false;
            }
            if (string.IsNullOrEmpty(slotType))
            {
                reason = "Invalid slot type.";
                return false;
            }
            if (!_modCatalog.TryGetValue(modId, out var modDef))
            {
                reason = $"Modification '{modId}' not found in catalog.";
                return false;
            }
            if (!string.Equals(modDef.slot_type, slotType, StringComparison.OrdinalIgnoreCase))
            {
                reason = $"Modification '{modId}' cannot be installed in '{slotType}' slot (requires '{modDef.slot_type}').";
                return false;
            }

            var record = GetOrCreateRecord(vehicleId);
            if (record.isImmobilized)
            {
                reason = $"Vehicle '{vehicleId}' is immobilized and cannot be modified until recovered.";
                return false;
            }

            if (inventory != null)
            {
                foreach (var cost in modDef.install_cost)
                {
                    if (!inventory.HasSufficient(cost.item_id, cost.amount))
                    {
                        reason = $"Insufficient material: requires {cost.amount} of '{cost.item_id}'.";
                        return false;
                    }
                }
            }

            return true;
        }

        public bool InstallModification(string vehicleId, string slotType, string modId, IPlayerInventoryPort? inventory, out string reason)
        {
            if (!CanInstallModification(vehicleId, slotType, modId, inventory, out reason))
            {
                return false;
            }

            var modDef = _modCatalog[modId];
            if (inventory != null && modDef.install_cost.Count > 0)
            {
                var bill = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (var cost in modDef.install_cost)
                {
                    bill[cost.item_id] = cost.amount;
                }
                if (!inventory.TryConsumeBill(bill))
                {
                    reason = "Failed to consume installation materials atomically from inventory.";
                    return false;
                }
            }

            var record = GetOrCreateRecord(vehicleId);
            record.installedSlots[slotType] = modId;
            return true;
        }

        public bool UninstallModification(string vehicleId, string slotType, IPlayerInventoryPort? inventory, out string reason)
        {
            reason = string.Empty;
            if (!HasVehicleRecord(vehicleId))
            {
                reason = $"Vehicle '{vehicleId}' has no customization record.";
                return false;
            }

            var record = GetOrCreateRecord(vehicleId);
            if (!record.installedSlots.TryGetValue(slotType, out var currentModId))
            {
                reason = $"Slot '{slotType}' has no modification installed.";
                return false;
            }

            record.installedSlots.Remove(slotType);

            // Refund 50% scrap metal if catalog defines install cost
            if (inventory != null && _modCatalog.TryGetValue(currentModId, out var modDef))
            {
                foreach (var cost in modDef.install_cost)
                {
                    int refund = Math.Max(1, cost.amount / 2);
                    inventory.TryProduce(cost.item_id, refund);
                }
            }

            return true;
        }

        public float GetEffectiveCargoCapacityDelta(string vehicleId)
        {
            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0f;
            float total = 0f;
            foreach (var kvp in record.installedSlots)
            {
                if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
                    total += modDef.effects.cargo_capacity_delta;
            }
            return total;
        }

        public float GetEffectiveSpeedMultiplierDelta(string vehicleId)
        {
            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0f;
            float total = 0f;
            foreach (var kvp in record.installedSlots)
            {
                if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
                    total += modDef.effects.speed_multiplier_delta;
            }
            return total;
        }

        public float GetEffectiveFuelConsumptionMultiplier(string vehicleId)
        {
            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 1f;
            float mult = 1f;
            foreach (var kvp in record.installedSlots)
            {
                if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
                    mult *= modDef.effects.fuel_consumption_multiplier;
            }
            return Math.Max(0.1f, mult);
        }

        public float GetEffectiveWearRateMultiplier(string vehicleId)
        {
            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 1f;
            float mult = 1f;
            foreach (var kvp in record.installedSlots)
            {
                if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
                    mult *= modDef.effects.wear_rate_multiplier;
            }
            return Math.Max(0.1f, mult);
        }

        public int GetEffectiveRadiationProtectionPermille(string vehicleId)
        {
            if (!_state.vehicleRecords.TryGetValue(vehicleId, out var record)) return 0;
            int total = 0;
            foreach (var kvp in record.installedSlots)
            {
                if (_modCatalog.TryGetValue(kvp.Value, out var modDef))
                    total += modDef.effects.radiation_protection_permille;
            }
            return Math.Clamp(total, 0, 950);
        }

        public void RecordTripWear(string vehicleId, float distanceKm, float roadRoughnessMultiplier = 1f)
        {
            if (string.IsNullOrEmpty(vehicleId) || distanceKm <= 0f) return;
            var record = GetOrCreateRecord(vehicleId);
            if (record.isImmobilized) return;

            float wearMult = GetEffectiveWearRateMultiplier(vehicleId) * Math.Max(0.5f, roadRoughnessMultiplier);
            int baseWear = (int)Math.Round(distanceKm * 2f * wearMult);

            record.chassisStressPermille = Math.Clamp(record.chassisStressPermille + baseWear, 0, 1000);
            record.engineFoulingPermille = Math.Clamp(record.engineFoulingPermille + (int)Math.Round(baseWear * 0.8f), 0, 1000);
            record.transmissionWearPermille = Math.Clamp(record.transmissionWearPermille + (int)Math.Round(baseWear * 0.9f), 0, 1000);

            if (record.chassisStressPermille >= 1000 || record.engineFoulingPermille >= 1000 || record.transmissionWearPermille >= 1000)
            {
                record.isImmobilized = true;
                record.immobilizedReason = "Critical component catastrophic failure during overland transit.";
            }
        }

        public bool ServiceChassis(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
        {
            reason = string.Empty;
            var record = GetOrCreateRecord(vehicleId);
            if (record.chassisStressPermille <= 0)
            {
                reason = "Chassis is already at nominal condition.";
                return false;
            }

            int toRepair = Math.Min(repairPermille, record.chassisStressPermille);
            int costScrap = Math.Max(1, (int)Math.Ceiling(toRepair / 50.0));

            if (inventory != null)
            {
                if (!inventory.HasSufficient("scrap_metal", costScrap))
                {
                    reason = $"Requires {costScrap} scrap metal to service chassis.";
                    return false;
                }
                if (!inventory.TryConsume("scrap_metal", costScrap))
                {
                    reason = "Failed to consume scrap metal for chassis service.";
                    return false;
                }
            }

            record.chassisStressPermille = Math.Max(0, record.chassisStressPermille - toRepair);
            CheckClearImmobilization(record);
            return true;
        }

        public bool ServiceEngine(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
        {
            reason = string.Empty;
            var record = GetOrCreateRecord(vehicleId);
            if (record.engineFoulingPermille <= 0)
            {
                reason = "Engine is already at nominal condition.";
                return false;
            }

            int toRepair = Math.Min(repairPermille, record.engineFoulingPermille);
            int costParts = Math.Max(1, (int)Math.Ceiling(toRepair / 100.0));

            if (inventory != null)
            {
                if (!inventory.HasSufficient("mechanical_parts", costParts))
                {
                    reason = $"Requires {costParts} mechanical parts to service engine.";
                    return false;
                }
                if (!inventory.TryConsume("mechanical_parts", costParts))
                {
                    reason = "Failed to consume mechanical parts for engine service.";
                    return false;
                }
            }

            record.engineFoulingPermille = Math.Max(0, record.engineFoulingPermille - toRepair);
            CheckClearImmobilization(record);
            return true;
        }

        public bool ServiceTransmission(string vehicleId, IPlayerInventoryPort? inventory, int repairPermille, out string reason)
        {
            reason = string.Empty;
            var record = GetOrCreateRecord(vehicleId);
            if (record.transmissionWearPermille <= 0)
            {
                reason = "Transmission is already at nominal condition.";
                return false;
            }

            int toRepair = Math.Min(repairPermille, record.transmissionWearPermille);
            int costParts = Math.Max(1, (int)Math.Ceiling(toRepair / 100.0));

            if (inventory != null)
            {
                if (!inventory.HasSufficient("mechanical_parts", costParts))
                {
                    reason = $"Requires {costParts} mechanical parts to service transmission.";
                    return false;
                }
                if (!inventory.TryConsume("mechanical_parts", costParts))
                {
                    reason = "Failed to consume mechanical parts for transmission service.";
                    return false;
                }
            }

            record.transmissionWearPermille = Math.Max(0, record.transmissionWearPermille - toRepair);
            CheckClearImmobilization(record);
            return true;
        }

        private static void CheckClearImmobilization(VehicleCustomizationRecord record)
        {
            if (record.isImmobilized &&
                record.chassisStressPermille < 900 &&
                record.engineFoulingPermille < 900 &&
                record.transmissionWearPermille < 900)
            {
                record.isImmobilized = false;
                record.immobilizedReason = string.Empty;
            }
        }

        public bool RegisterRecoveryMission(string vehicleId, string locationId, int requiredFuelUnits, out string missionId, out string reason)
        {
            missionId = string.Empty;
            reason = string.Empty;
            var record = GetOrCreateRecord(vehicleId);
            if (!record.isImmobilized)
            {
                reason = $"Vehicle '{vehicleId}' is not immobilized.";
                return false;
            }

            foreach (var mission in _state.activeRecoveries.Values)
            {
                if (string.Equals(mission.strandedVehicleId, vehicleId, StringComparison.Ordinal))
                {
                    missionId = mission.missionId;
                    reason = "Recovery mission already active for this vehicle.";
                    return false;
                }
            }

            missionId = $"recov_{_state.nextRecoveryCounter++}_{vehicleId}";
            var newMission = new VehicleRecoveryMission
            {
                missionId = missionId,
                strandedVehicleId = vehicleId,
                locationId = locationId,
                requiredFuelUnits = requiredFuelUnits,
                progressTicks = 0,
                requiredTicks = 120,
                isComplete = false
            };
            _state.activeRecoveries[missionId] = newMission;
            return true;
        }

        public bool AdvanceRecoveryMission(string missionId, int deltaTicks, out bool completed)
        {
            completed = false;
            if (!_state.activeRecoveries.TryGetValue(missionId, out var mission))
                return false;

            if (mission.isComplete)
            {
                completed = true;
                return true;
            }

            mission.progressTicks += Math.Max(0, deltaTicks);
            if (mission.progressTicks >= mission.requiredTicks)
            {
                mission.isComplete = true;
                completed = true;
            }
            return true;
        }

        public bool CompleteRecoveryMission(string missionId, IPlayerInventoryPort? inventory, out string reason)
        {
            reason = string.Empty;
            if (!_state.activeRecoveries.TryGetValue(missionId, out var mission))
            {
                reason = $"Recovery mission '{missionId}' not found.";
                return false;
            }
            if (!mission.isComplete)
            {
                reason = $"Recovery mission '{missionId}' is still in progress ({mission.progressTicks}/{mission.requiredTicks} ticks).";
                return false;
            }

            var record = GetOrCreateRecord(mission.strandedVehicleId);
            // Hauling vehicle back resets catastrophic wear to 800 permille so it can be serviced in garage
            record.chassisStressPermille = Math.Min(800, record.chassisStressPermille);
            record.engineFoulingPermille = Math.Min(800, record.engineFoulingPermille);
            record.transmissionWearPermille = Math.Min(800, record.transmissionWearPermille);
            record.isImmobilized = false;
            record.immobilizedReason = string.Empty;

            _state.activeRecoveries.Remove(missionId);
            return true;
        }

        public VehicleGarageState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<VehicleGarageState>(json) ?? new VehicleGarageState();
        }

        public void RestoreState(VehicleGarageState? state)
        {
            if (state == null)
            {
                _state = new VehicleGarageState();
                return;
            }
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<VehicleGarageState>(json) ?? new VehicleGarageState();
        }
    }
}
