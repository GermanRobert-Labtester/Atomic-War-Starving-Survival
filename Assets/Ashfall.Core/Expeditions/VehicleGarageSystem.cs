// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Foundry;
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
        // CF-P6 — additive armor state. Missing fields in old saves deserialize
        // to the stock/default values and therefore preserve legacy behavior.
        public string armorGradeId = string.Empty;
        public int armorIntegrityPermille;
        public int armorIntegrityMaxPermille;
        public string armorMaterialProfileId = string.Empty;
        public string armorPurity = FoundryPurityNames.Standard;
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
        private readonly Dictionary<string, VehicleArmorGradeDefinition> _armorCatalog =
            new Dictionary<string, VehicleArmorGradeDefinition>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private VehicleGarageState _state = new VehicleGarageState();

        public delegate bool TryGetArmorMaterialQuality(out FoundryMaterialQuality quality);

        /// <summary>Optional foundry handoff. Unset/false produces a neutral stamp.</summary>
        public TryGetArmorMaterialQuality? ArmorMaterialQualitySource { get; set; }

        /// <summary>Optional vehicle classifier. Unset keeps pure-Core callers neutral.</summary>
        public Func<string, string?>? VehicleTerrainResolver { get; set; }

        private string _armorDefaultGradeId = string.Empty;

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

        public void LoadArmorCatalog(VehicleArmorGradeCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            _armorCatalog.Clear();
            foreach (var grade in catalog.grades)
            {
                if (grade != null && !string.IsNullOrEmpty(grade.id))
                    _armorCatalog[grade.id] = grade;
            }
            _armorDefaultGradeId = catalog.default_grade_id ?? string.Empty;
        }

        public bool HasArmorGrade(string gradeId) => !string.IsNullOrEmpty(gradeId) && _armorCatalog.ContainsKey(gradeId);

        public VehicleArmorGradeDefinition? GetArmorGrade(string gradeId)
        {
            _armorCatalog.TryGetValue(gradeId, out var definition);
            return definition;
        }

        public IReadOnlyDictionary<string, VehicleArmorGradeDefinition> GetAllArmorGrades() => _armorCatalog;

        public VehicleArmorProfile GetArmorProfile(string vehicleId)
        {
            var record = GetRecord(vehicleId);
            if (record == null || string.IsNullOrEmpty(record.armorGradeId)
                || !_armorCatalog.TryGetValue(record.armorGradeId, out var grade)
                || grade.is_default)
            {
                return CreateDefaultArmorProfile();
            }

            int integrityMax = Math.Max(0, record.armorIntegrityMaxPermille);
            int integrity = Math.Clamp(record.armorIntegrityPermille, 0, integrityMax);
            bool active = integrity > 0;
            return new VehicleArmorProfile
            {
                GradeId = grade.id,
                DisplayName = grade.display_name,
                Tier = grade.tier,
                IsDefault = false,
                MitigationPermille = active ? Math.Clamp(grade.mitigation_permille, 0, 400) : 0,
                WearAbsorptionPermille = active ? Math.Clamp(grade.wear_absorption_permille, 0, 500) : 0,
                IntegrityPermille = integrity,
                IntegrityMaxPermille = integrityMax,
                MaterialProfileId = record.armorMaterialProfileId ?? string.Empty,
                Purity = string.IsNullOrEmpty(record.armorPurity) ? FoundryPurityNames.Standard : record.armorPurity,
                ConditionBand = ArmorConditionBand(integrity, integrityMax),
                SpeedMultiplierDelta = grade.speed_multiplier_delta,
                FuelConsumptionMultiplier = grade.fuel_consumption_multiplier
            };
        }

        public static string ArmorConditionBand(int integrityPermille, int maxPermille)
        {
            if (maxPermille <= 0) return "none";
            if (integrityPermille <= 0) return "depleted";
            if (integrityPermille * 100 >= maxPermille * 75) return "nominal";
            if (integrityPermille * 100 >= maxPermille * 25) return "worn";
            return "critical";
        }

        private VehicleArmorProfile CreateDefaultArmorProfile()
        {
            VehicleArmorGradeDefinition? grade = null;
            if (!string.IsNullOrEmpty(_armorDefaultGradeId))
                _armorCatalog.TryGetValue(_armorDefaultGradeId, out grade);
            return new VehicleArmorProfile
            {
                GradeId = grade?.id ?? _armorDefaultGradeId,
                DisplayName = grade?.display_name ?? "Stock Plating",
                Tier = grade?.tier ?? 0,
                IsDefault = true,
                ConditionBand = "none",
                MaterialProfileId = string.Empty,
                Purity = FoundryPurityNames.Standard,
                SpeedMultiplierDelta = grade?.speed_multiplier_delta ?? 0f,
                FuelConsumptionMultiplier = grade?.fuel_consumption_multiplier ?? 1f
            };
        }

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

            // CF-P6 armor is an additive second decorator. Mass remains present
            // when a plate is depleted; mitigation alone is gated by integrity.
            var armor = GetArmorProfile(vehicleId);
            if (!armor.IsDefault)
            {
                profile.speedMultiplier = Math.Max(0.01f, profile.speedMultiplier * (1f + armor.SpeedMultiplierDelta));
                profile.fuelPerTravelTick = Math.Max(0f, profile.fuelPerTravelTick * armor.FuelConsumptionMultiplier);
                if (profile.breakdownChancePerTick > 0f && armor.MitigationPermille > 0)
                {
                    profile.breakdownChancePerTick = Math.Clamp(
                        profile.breakdownChancePerTick * (1000 - armor.MitigationPermille) / 1000f,
                        0f, 1f);
                }
            }
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

        public bool CanInstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason)
        {
            reason = string.Empty;
            if (string.IsNullOrEmpty(vehicleId))
            {
                reason = "Invalid vehicle ID.";
                return false;
            }
            if (!_armorCatalog.TryGetValue(gradeId, out var grade))
            {
                reason = $"Armor grade '{gradeId}' not found in catalog.";
                return false;
            }
            if (VehicleTerrainResolver != null && VehicleTerrainResolver(vehicleId) == null)
            {
                reason = $"Vehicle '{vehicleId}' was not found.";
                return false;
            }
            if (grade.is_default || string.Equals(grade.id, _armorDefaultGradeId, StringComparison.Ordinal))
            {
                reason = "Stock plating is the absence of armor — nothing to install.";
                return false;
            }

            var record = GetOrCreateRecord(vehicleId);
            if (string.Equals(record.armorGradeId, grade.id, StringComparison.Ordinal))
            {
                reason = $"Vehicle already wears grade '{grade.id}'.";
                return false;
            }
            if (record.isImmobilized)
            {
                reason = $"Vehicle '{vehicleId}' is immobilized and cannot be modified until recovered.";
                return false;
            }

            string? terrain = VehicleTerrainResolver?.Invoke(vehicleId);
            if (!string.IsNullOrEmpty(terrain)
                && (grade.compatible_terrain_types == null
                    || !grade.compatible_terrain_types.Contains(terrain, StringComparer.Ordinal)))
            {
                reason = $"Grade '{grade.id}' cannot be fitted to '{terrain}' terrain vehicles.";
                return false;
            }

            if (inventory != null)
            {
                foreach (var cost in grade.install_cost)
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

        public bool InstallArmorGrade(string vehicleId, string gradeId, IPlayerInventoryPort? inventory, out string reason)
        {
            if (!CanInstallArmorGrade(vehicleId, gradeId, inventory, out reason)) return false;

            var grade = _armorCatalog[gradeId];
            var bill = BuildArmorBill(grade.install_cost);
            if (inventory != null && bill.Count > 0 && !inventory.TryConsumeBill(bill))
            {
                reason = "Failed to consume armor installation materials atomically from inventory.";
                return false;
            }

            var record = GetOrCreateRecord(vehicleId);
            if (!string.IsNullOrEmpty(record.armorGradeId)
                && _armorCatalog.TryGetValue(record.armorGradeId, out var oldGrade))
            {
                foreach (var cost in oldGrade.install_cost)
                {
                    if (cost.item_id == "scrap_metal" && inventory != null)
                        inventory.TryProduce(cost.item_id, Math.Max(1, cost.amount / 2));
                }
            }

            int armorBp = 1000;
            FoundryPurityTier purity = FoundryPurityTier.Standard;
            string materialProfileId = string.Empty;
            if (ArmorMaterialQualitySource != null
                && ArmorMaterialQualitySource(out var quality))
            {
                armorBp = Math.Clamp(quality.ArmorModifierBp, 500, 2000);
                purity = quality.Purity;
                materialProfileId = quality.MaterialProfileId ?? string.Empty;
            }

            int purityBp = PurityModifierBp(purity);
            int stampedMax = (int)Math.Round(
                grade.integrity_pool_permille * armorBp / 1000.0 * purityBp / 1000.0,
                MidpointRounding.AwayFromZero);
            int minimum = (int)Math.Ceiling(grade.integrity_pool_permille * 0.5);
            int maximum = (int)Math.Ceiling(grade.integrity_pool_permille * 1.3);
            stampedMax = Math.Clamp(stampedMax, minimum, maximum);

            record.armorGradeId = grade.id;
            record.armorIntegrityMaxPermille = stampedMax;
            record.armorIntegrityPermille = stampedMax;
            record.armorMaterialProfileId = materialProfileId;
            record.armorPurity = FoundryPurityNames.Name(purity);
            reason = string.Empty;
            return true;
        }

        public bool ReforgeArmorPlate(string vehicleId, IPlayerInventoryPort? inventory, out string reason)
        {
            reason = string.Empty;
            var record = GetRecord(vehicleId);
            if (record == null || string.IsNullOrEmpty(record.armorGradeId)
                || !_armorCatalog.TryGetValue(record.armorGradeId, out var grade)
                || grade.is_default)
            {
                reason = "No fitted armor plate to re-forge.";
                return false;
            }
            if (record.isImmobilized)
            {
                reason = $"Vehicle '{vehicleId}' is immobilized and cannot be modified until recovered.";
                return false;
            }
            if (record.armorIntegrityPermille >= record.armorIntegrityMaxPermille)
            {
                reason = "Plate is already at full integrity.";
                return false;
            }

            if (inventory != null)
            {
                foreach (var cost in grade.reforge_cost)
                {
                    if (!inventory.HasSufficient(cost.item_id, cost.amount))
                    {
                        reason = $"Insufficient material: requires {cost.amount} of '{cost.item_id}'.";
                        return false;
                    }
                }
            }
            var bill = BuildArmorBill(grade.reforge_cost);
            if (inventory != null && bill.Count > 0 && !inventory.TryConsumeBill(bill))
            {
                reason = "Failed to consume re-forge materials atomically from inventory.";
                return false;
            }
            record.armorIntegrityPermille = Math.Max(0, record.armorIntegrityMaxPermille);
            return true;
        }

        private static Dictionary<string, int> BuildArmorBill(IReadOnlyList<VehicleArmorGradeCost> costs)
        {
            var bill = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var cost in costs)
            {
                if (string.IsNullOrEmpty(cost.item_id) || cost.amount <= 0) continue;
                bill[cost.item_id] = bill.TryGetValue(cost.item_id, out int current)
                    ? current + cost.amount : cost.amount;
            }
            return bill;
        }

        private static int PurityModifierBp(FoundryPurityTier purity) => purity switch
        {
            FoundryPurityTier.Poor => 850,
            FoundryPurityTier.High => 1100,
            FoundryPurityTier.Exceptional => 1200,
            _ => 1000
        };

        private int GetEffectiveArmorMitigationPermille(string vehicleId)
        {
            var profile = GetArmorProfile(vehicleId);
            return profile.IsDefault ? 0 : profile.MitigationPermille;
        }

        private int GetEffectiveArmorWearAbsorptionPermille(string vehicleId)
        {
            var profile = GetArmorProfile(vehicleId);
            return profile.IsDefault ? 0 : profile.WearAbsorptionPermille;
        }

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

            int absorbed = 0;
            int absorption = GetEffectiveArmorWearAbsorptionPermille(vehicleId);
            if (absorption > 0)
            {
                absorbed = Math.Min(
                    Math.Max(0, record.armorIntegrityPermille),
                    (int)Math.Round(baseWear * absorption / 1000.0, MidpointRounding.AwayFromZero));
                record.armorIntegrityPermille = Math.Max(0, record.armorIntegrityPermille - absorbed);
            }

            record.chassisStressPermille = Math.Clamp(record.chassisStressPermille + baseWear - absorbed, 0, 1000);
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
