using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Combat
{
    public enum WeaponHeadspaceState
    {
        Normal = 0,
        Warning = 1,
        Unsafe = 2,
        Catastrophic = 3
    }

    [Serializable]
    public sealed class BallisticsWorkbenchDefinition
    {
        [JsonPropertyName("profile_id")]
        public string ProfileId { get; set; } = string.Empty;

        [JsonPropertyName("weapon_tag")]
        public string WeaponTag { get; set; } = string.Empty;

        [JsonPropertyName("base_dispersion_moa")]
        public float BaseDispersionMoa { get; set; } = 4f;

        [JsonPropertyName("wear_per_shot_factor")]
        public float WearPerShotFactor { get; set; } = 0.1f;

        [JsonPropertyName("corrosive_ammo_wear_modifier")]
        public float CorrosiveAmmoWearModifier { get; set; } = 1.5f;

        [JsonPropertyName("overpressure_wear_modifier")]
        public float OverpressureWearModifier { get; set; } = 1.8f;

        [JsonPropertyName("headspace_warning_threshold")]
        public float HeadspaceWarningThreshold { get; set; } = 0.55f;

        [JsonPropertyName("headspace_failure_threshold")]
        public float HeadspaceFailureThreshold { get; set; } = 0.85f;

        [JsonPropertyName("max_calibration_bonus")]
        public float MaxCalibrationBonus { get; set; } = 0.2f;

        [JsonPropertyName("max_optic_bonus")]
        public float MaxOpticBonus { get; set; } = 0.15f;

        [JsonPropertyName("maintenance_recipe_id")]
        public string MaintenanceRecipeId { get; set; } = string.Empty;

        [JsonPropertyName("supported_ammo_tags")]
        public List<string> SupportedAmmoTags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class BallisticsWorkbenchCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("profiles")]
        public List<BallisticsWorkbenchDefinition> Profiles { get; set; } =
            new List<BallisticsWorkbenchDefinition>();
    }

    [Serializable]
    public sealed class WeaponBallisticsProfile
    {
        public string WeaponInstanceId = string.Empty;
        public string ProfileId = string.Empty;
        public float ThroatWearIndex;
        public float HeadspaceIndex;
        public float MuzzleVelocityVariance;
        public float DispersionMoa;
        public float CrownCondition = 1f;
        public float RiflingCondition = 1f;
        public float CalibrationQuality;
        // B99: quality of the one optic mounted to this weapon. The
        // ballistics profile is the persisted home; the optics bench owns
        // manufacture, not mounted-equipment state.
        public float OpticQuality;
        public WeaponHeadspaceState HeadspaceState;
        public bool Inspected;
        public bool CatastrophicFailureResolved;
        public int LastServiceDay = -1;
        public int TotalRounds;
        public string LastAmmoBatchId = string.Empty;
        public List<string> ResolvedFailureEventIds = new List<string>();
    }

    [Serializable]
    public sealed class CustomAmmoBatchState
    {
        public string BatchId = string.Empty;
        public string AmmoItemId = string.Empty;
        public float ChargeQuality;
        public float ProjectileUniformity;
        public float VelocityConsistency;
        public float PressureRisk;
        public string CreatorSurvivorId = string.Empty;
        public int CreatedDay;
    }

    [Serializable]
    public sealed class BallisticsWorkbenchState
    {
        public string SystemId = BallisticsWorkbenchSystem.SystemId;
        public int NextBatchSequence = 1;
        public Dictionary<string, WeaponBallisticsProfile> Profiles =
            new Dictionary<string, WeaponBallisticsProfile>(StringComparer.Ordinal);
        public List<CustomAmmoBatchState> AmmoBatches = new List<CustomAmmoBatchState>();
    }

    /// <summary>Bounded combat projection; it owns no combat resolution.</summary>
    [Serializable]
    public sealed class WeaponCombatModifier
    {
        public float AccuracyMultiplier = 1f;
        public float RangeMultiplier = 1f;
        public float PenetrationMultiplier = 1f;
        public float CriticalMultiplier = 1f;
        public float MalfunctionMultiplier = 1f;
        public float DispersionMoa = 0f;
    }

    public sealed class BallisticsFireResult
    {
        public bool Accepted;
        public bool CatastrophicFailure;
        public string FailureCode = string.Empty;
        public WeaponHeadspaceState HeadspaceState;
        public float DispersionMoa;
    }

    /// <summary>
    /// Persistent calibration layer over EquipmentConditionSystem and combat.
    /// It stores precision metadata by canonical weapon instance ID, then
    /// returns a bounded modifier for TacticalCombatSystem.
    /// </summary>
    public sealed class BallisticsWorkbenchSystem
    {
        public const string SystemId = "ballistics_workbench";

        private BallisticsWorkbenchState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly EquipmentConditionSystem? _equipment;
        private readonly Inventory.Inventory? _inventory;
        private readonly Dictionary<string, BallisticsWorkbenchDefinition> _definitions =
            new Dictionary<string, BallisticsWorkbenchDefinition>(StringComparer.Ordinal);

        public BallisticsWorkbenchSystem(
            ISeededRng rng,
            BallisticsWorkbenchState? state = null,
            EquipmentConditionSystem? equipment = null,
            Inventory.Inventory? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state = state ?? new BallisticsWorkbenchState();
            _equipment = equipment;
            _inventory = inventory;
            _log = log ?? NullLog.Instance;
            NormalizeState();
        }

        public BallisticsWorkbenchState State => _state;
        public IReadOnlyDictionary<string, BallisticsWorkbenchDefinition> Definitions => _definitions;
        public IReadOnlyDictionary<string, WeaponBallisticsProfile> Profiles => _state.Profiles;
        public IReadOnlyList<CustomAmmoBatchState> AmmoBatches => _state.AmmoBatches;

        public event Action<WeaponBallisticsProfile>? OnProfileChanged;
        public event Action<string>? OnCatastrophicFailure;

        public void LoadCatalog(BallisticsWorkbenchCatalog? catalog)
        {
            if (catalog?.Profiles == null) return;
            _definitions.Clear();
            foreach (var definition in catalog.Profiles)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.ProfileId))
                    continue;
                if (definition.BaseDispersionMoa <= 0f ||
                    definition.HeadspaceFailureThreshold <= definition.HeadspaceWarningThreshold)
                    continue;
                _definitions[definition.ProfileId] = definition;
                if (!string.IsNullOrWhiteSpace(definition.WeaponTag))
                    _definitions[definition.WeaponTag] = definition;
            }
        }

        public void RegisterDefinition(BallisticsWorkbenchDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.ProfileId))
                return;
            _definitions[definition.ProfileId] = definition;
            if (!string.IsNullOrWhiteSpace(definition.WeaponTag))
                _definitions[definition.WeaponTag] = definition;
        }

        public WeaponBallisticsProfile EnsureProfile(string weaponInstanceId, string profileId)
        {
            if (string.IsNullOrWhiteSpace(weaponInstanceId))
                throw new ArgumentException("Weapon instance ID is required.", nameof(weaponInstanceId));
            if (_state.Profiles.TryGetValue(weaponInstanceId, out var existing))
                return existing;

            var definition = ResolveDefinition(profileId);
            var profile = new WeaponBallisticsProfile
            {
                WeaponInstanceId = weaponInstanceId,
                ProfileId = definition?.ProfileId ?? profileId ?? string.Empty,
                DispersionMoa = definition?.BaseDispersionMoa ?? 4f,
                CrownCondition = 1f,
                RiflingCondition = 1f
            };
            Recalculate(profile, definition);
            _state.Profiles[weaponInstanceId] = profile;
            OnProfileChanged?.Invoke(profile);
            return profile;
        }

        public ActionResult Inspect(string weaponInstanceId, int day)
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null)
                return ActionResult.Failed("unknown_weapon", "ballistics.unknown_weapon");
            profile.Inspected = true;
            profile.LastServiceDay = day;
            UpdateHeadspace(profile, ResolveDefinition(profile.ProfileId));
            OnProfileChanged?.Invoke(profile);
            return ActionResult.Success("ballistics.inspected");
        }

        public ActionResult Calibrate(
            string weaponInstanceId,
            float operatorSkill,
            float toolingCalibration,
            int day)
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null)
                return ActionResult.Failed("unknown_weapon", "ballistics.unknown_weapon");
            var definition = ResolveDefinition(profile.ProfileId);
            float ceiling = definition?.MaxCalibrationBonus ?? 0.2f;
            float boundedSkill = Math.Clamp(operatorSkill, 0f, 1f);
            float boundedTooling = Math.Clamp(toolingCalibration, 0f, 1f);
            float variance = (float)_rng.NextDouble() * 0.06f;
            profile.CalibrationQuality = Math.Clamp(
                boundedSkill * 0.55f + boundedTooling * 0.35f + 0.10f - variance,
                0f, 1f);
            profile.CrownCondition = Math.Clamp(profile.CrownCondition + 0.08f * boundedSkill, 0f, 1f);
            profile.RiflingCondition = Math.Clamp(profile.RiflingCondition + 0.05f * boundedTooling, 0f, 1f);
            profile.LastServiceDay = day;
            profile.Inspected = true;
            Recalculate(profile, definition);
            profile.DispersionMoa = Math.Max(
                definition?.BaseDispersionMoa ?? 4f * (1f - ceiling),
                profile.DispersionMoa);
            OnProfileChanged?.Invoke(profile);
            return ActionResult.Success("ballistics.calibrated");
        }

        /// <summary>
        /// Mount one completed optical element on a weapon profile. This is a
        /// bounded projection only; combat resolution remains in TacticalCombatSystem.
        /// </summary>
        public ActionResult AttachOptic(string weaponInstanceId, float opticQuality)
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null)
                return ActionResult.Failed("unknown_weapon", "ballistics.unknown_weapon");
            if (profile.OpticQuality > 0f)
                return ActionResult.Blocked("optic_already_attached", "ballistics.optic_already_attached");

            profile.OpticQuality = Math.Clamp(opticQuality, 0f, 1f);
            Recalculate(profile, ResolveDefinition(profile.ProfileId));
            OnProfileChanged?.Invoke(profile);
            return ActionResult.Success("ballistics.optic_attached");
        }

        public ActionResult Refurbish(
            string weaponInstanceId,
            IReadOnlyList<string>? parts,
            float serviceQuality,
            int day)
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null)
                return ActionResult.Failed("unknown_weapon", "ballistics.unknown_weapon");
            if (_equipment != null)
            {
                var result = _equipment.RepairItem(
                    weaponInstanceId,
                    MaintenanceType.Calibrate,
                    parts != null ? new List<string>(parts) : new List<string>(),
                    Math.Clamp(serviceQuality, 0f, 1f));
                if (!result.IsSuccess) return result;
            }
            else if (_inventory != null && parts != null && parts.Count > 0 &&
                     !_inventory.TryConsumeBill(parts))
            {
                return ActionResult.Blocked("missing_parts", "ballistics.missing_parts");
            }

            profile.ThroatWearIndex = Math.Max(0f, profile.ThroatWearIndex - 0.35f);
            profile.HeadspaceIndex = Math.Max(0f, profile.HeadspaceIndex - 0.30f);
            profile.CrownCondition = Math.Min(1f, profile.CrownCondition + 0.25f);
            profile.RiflingCondition = Math.Min(1f, profile.RiflingCondition + 0.25f);
            profile.CatastrophicFailureResolved = false;
            profile.LastServiceDay = day;
            profile.Inspected = true;
            Recalculate(profile, ResolveDefinition(profile.ProfileId));
            OnProfileChanged?.Invoke(profile);
            return ActionResult.Success("ballistics.refurbished");
        }

        public BallisticsFireResult RecordFiring(
            string weaponInstanceId,
            string eventId,
            int rounds,
            bool corrosiveAmmo,
            bool overpressureAmmo)
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null)
                return new BallisticsFireResult { FailureCode = "unknown_weapon" };
            if (profile.ResolvedFailureEventIds.Contains(eventId))
                return new BallisticsFireResult
                {
                    Accepted = true,
                    CatastrophicFailure = profile.CatastrophicFailureResolved,
                    HeadspaceState = profile.HeadspaceState,
                    DispersionMoa = profile.DispersionMoa
                };

            int count = Math.Max(1, rounds);
            var definition = ResolveDefinition(profile.ProfileId);
            float wearMultiplier = 1f;
            if (corrosiveAmmo) wearMultiplier *= definition?.CorrosiveAmmoWearModifier ?? 1.5f;
            if (overpressureAmmo) wearMultiplier *= definition?.OverpressureWearModifier ?? 1.8f;
            profile.TotalRounds += count;
            profile.ThroatWearIndex = Math.Clamp(
                profile.ThroatWearIndex + count * (definition?.WearPerShotFactor ?? 0.1f) *
                wearMultiplier * 0.01f, 0f, 1.5f);
            profile.HeadspaceIndex = Math.Clamp(
                profile.HeadspaceIndex + count * 0.0008f * wearMultiplier, 0f, 1.5f);
            profile.MuzzleVelocityVariance = Math.Clamp(
                profile.MuzzleVelocityVariance + count * 0.0005f * wearMultiplier, 0f, 1f);
            profile.CrownCondition = Math.Max(0f,
                profile.CrownCondition - count * 0.0003f * wearMultiplier);
            profile.RiflingCondition = Math.Max(0f,
                profile.RiflingCondition - count * 0.0004f * wearMultiplier);
            _equipment?.UseItem(weaponInstanceId,
                count * (definition?.WearPerShotFactor ?? 0.1f) * wearMultiplier * 0.1f);
            UpdateHeadspace(profile, definition);

            bool catastrophic = false;
            if (profile.HeadspaceState == WeaponHeadspaceState.Unsafe &&
                !profile.CatastrophicFailureResolved &&
                _rng.NextDouble() < 0.08 + profile.HeadspaceIndex * 0.12)
            {
                catastrophic = true;
                profile.HeadspaceState = WeaponHeadspaceState.Catastrophic;
                profile.CatastrophicFailureResolved = true;
                OnCatastrophicFailure?.Invoke(weaponInstanceId);
            }
            if (!string.IsNullOrWhiteSpace(eventId))
                profile.ResolvedFailureEventIds.Add(eventId);
            Recalculate(profile, definition);
            OnProfileChanged?.Invoke(profile);
            return new BallisticsFireResult
            {
                Accepted = true,
                CatastrophicFailure = catastrophic,
                HeadspaceState = profile.HeadspaceState,
                DispersionMoa = profile.DispersionMoa
            };
        }

        public CustomAmmoBatchState? CreateAmmoBatch(
            string ammoItemId,
            string creatorSurvivorId,
            float skill,
            float toolingQuality,
            int day,
            int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(ammoItemId) || amount <= 0) return null;
            float boundedSkill = Math.Clamp(skill, 0f, 1f);
            float boundedTooling = Math.Clamp(toolingQuality, 0f, 1f);
            string batchId = $"ammo_batch_{day}_{_state.NextBatchSequence++}";
            var batch = new CustomAmmoBatchState
            {
                BatchId = batchId,
                AmmoItemId = ammoItemId,
                ChargeQuality = Math.Clamp(
                    boundedSkill * 0.65f + boundedTooling * 0.25f +
                    (float)_rng.NextDouble() * 0.10f, 0f, 1f),
                ProjectileUniformity = Math.Clamp(
                    boundedSkill * 0.45f + boundedTooling * 0.45f +
                    (float)_rng.NextDouble() * 0.10f, 0f, 1f),
                CreatorSurvivorId = creatorSurvivorId ?? string.Empty,
                CreatedDay = day
            };
            batch.VelocityConsistency = Math.Clamp(
                (batch.ChargeQuality + batch.ProjectileUniformity) * 0.5f, 0f, 1f);
            batch.PressureRisk = Math.Clamp(
                1f - (batch.ChargeQuality * 0.65f + batch.ProjectileUniformity * 0.35f),
                0f, 1f);
            _state.AmmoBatches.Add(batch);
            return batch;
        }

        public WeaponCombatModifier GetCombatModifier(
            string weaponInstanceId,
            string ammoBatchId = "")
        {
            var profile = FindProfile(weaponInstanceId);
            if (profile == null) return new WeaponCombatModifier();
            var definition = ResolveDefinition(profile.ProfileId);
            float baseDispersion = Math.Max(0.1f, definition?.BaseDispersionMoa ?? 4f);
            float accuracy = Math.Clamp(baseDispersion / Math.Max(0.1f, profile.DispersionMoa), 0.65f, 1.2f);
            float calibration = 1f + Math.Clamp(profile.CalibrationQuality *
                (definition?.MaxCalibrationBonus ?? 0.2f), 0f, 0.25f);
            float optic = 1f + Math.Clamp(profile.OpticQuality *
                (definition?.MaxOpticBonus ?? 0.15f), 0f, 0.20f);
            float unsafePenalty = profile.HeadspaceState >= WeaponHeadspaceState.Unsafe ? 0.75f : 1f;
            var modifier = new WeaponCombatModifier
            {
                AccuracyMultiplier = Math.Clamp(accuracy * calibration * optic * unsafePenalty, 0.5f, 1.2f),
                RangeMultiplier = Math.Clamp(0.9f + profile.RiflingCondition * 0.1f, 0.75f, 1f),
                PenetrationMultiplier = Math.Clamp(0.85f + profile.CrownCondition * 0.15f, 0.75f, 1f),
                CriticalMultiplier = Math.Clamp(0.9f + profile.CalibrationQuality * 0.15f, 0.8f, 1.1f),
                MalfunctionMultiplier = Math.Clamp(
                    0.75f + profile.HeadspaceIndex * 0.9f +
                    (profile.HeadspaceState >= WeaponHeadspaceState.Unsafe ? 0.5f : 0f),
                    0.5f, 3f),
                DispersionMoa = profile.DispersionMoa
            };
            if (!string.IsNullOrWhiteSpace(ammoBatchId))
            {
                var batch = _state.AmmoBatches.Find(x => x.BatchId == ammoBatchId);
                if (batch != null)
                {
                    modifier.AccuracyMultiplier = Math.Clamp(
                        modifier.AccuracyMultiplier *
                        (0.85f + batch.ProjectileUniformity * 0.15f), 0.5f, 1.2f);
                    modifier.MalfunctionMultiplier = Math.Clamp(
                        modifier.MalfunctionMultiplier * (0.85f + batch.PressureRisk * 0.5f),
                        0.5f, 3f);
                }
            }
            return modifier;
        }

        public void ApplyToCombatWeapon(
            WeaponInstanceState weapon,
            string ammoBatchId = "")
        {
            if (weapon == null || string.IsNullOrWhiteSpace(weapon.InstanceId)) return;
            var modifier = GetCombatModifier(weapon.InstanceId, ammoBatchId);
            weapon.BallisticsAccuracyMultiplier = modifier.AccuracyMultiplier;
            weapon.BallisticsRangeMultiplier = modifier.RangeMultiplier;
            weapon.BallisticsPenetrationMultiplier = modifier.PenetrationMultiplier;
            weapon.BallisticsCriticalMultiplier = modifier.CriticalMultiplier;
            weapon.BallisticsMalfunctionMultiplier = modifier.MalfunctionMultiplier;
        }

        public WeaponBallisticsProfile? FindProfile(string weaponInstanceId)
        {
            if (string.IsNullOrWhiteSpace(weaponInstanceId)) return null;
            return _state.Profiles.TryGetValue(weaponInstanceId, out var profile) ? profile : null;
        }

        public BallisticsWorkbenchState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<BallisticsWorkbenchState>(serializer.Serialize(_state))
                ?? new BallisticsWorkbenchState();
        }

        public void RestoreState(BallisticsWorkbenchState? saved)
        {
            if (saved == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<BallisticsWorkbenchState>(serializer.Serialize(saved))
                ?? new BallisticsWorkbenchState();
            NormalizeState();
        }

        private BallisticsWorkbenchDefinition? ResolveDefinition(string? key)
        {
            if (!string.IsNullOrWhiteSpace(key) && _definitions.TryGetValue(key, out var byId))
                return byId;
            return null;
        }

        private static void UpdateHeadspace(
            WeaponBallisticsProfile profile,
            BallisticsWorkbenchDefinition? definition)
        {
            float warning = definition?.HeadspaceWarningThreshold ?? 0.55f;
            float failure = definition?.HeadspaceFailureThreshold ?? 0.85f;
            profile.HeadspaceState = profile.HeadspaceIndex >= failure
                ? WeaponHeadspaceState.Unsafe
                : profile.HeadspaceIndex >= warning
                    ? WeaponHeadspaceState.Warning
                    : WeaponHeadspaceState.Normal;
        }

        private static void Recalculate(
            WeaponBallisticsProfile profile,
            BallisticsWorkbenchDefinition? definition)
        {
            float baseDispersion = definition?.BaseDispersionMoa ?? 4f;
            profile.DispersionMoa = Math.Max(0.1f,
                baseDispersion *
                (1f + profile.ThroatWearIndex * 0.8f +
                 profile.MuzzleVelocityVariance * 0.25f) *
                (1f + (1f - profile.CrownCondition) * 0.35f) *
                (1f - Math.Clamp(profile.CalibrationQuality *
                    (definition?.MaxCalibrationBonus ?? 0.2f), 0f, 0.25f)));
        }

        private void NormalizeState()
        {
            _state.Profiles ??= new Dictionary<string, WeaponBallisticsProfile>(StringComparer.Ordinal);
            _state.AmmoBatches ??= new List<CustomAmmoBatchState>();
            _state.NextBatchSequence = Math.Max(1, _state.NextBatchSequence);
            foreach (var profile in _state.Profiles.Values)
            {
                profile.ResolvedFailureEventIds ??= new List<string>();
                profile.ThroatWearIndex = Math.Clamp(profile.ThroatWearIndex, 0f, 1.5f);
                profile.HeadspaceIndex = Math.Clamp(profile.HeadspaceIndex, 0f, 1.5f);
                profile.CrownCondition = Math.Clamp(profile.CrownCondition, 0f, 1f);
                profile.RiflingCondition = Math.Clamp(profile.RiflingCondition, 0f, 1f);
                profile.CalibrationQuality = Math.Clamp(profile.CalibrationQuality, 0f, 1f);
                profile.OpticQuality = Math.Clamp(profile.OpticQuality, 0f, 1f);
            }
        }
    }

    public static class BallisticsWorkbenchCatalogLoader
    {
        public const string FileName = "ballistics_workbench_catalog.json";

        public static BallisticsWorkbenchCatalog? Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(dataDir))
                throw new ArgumentNullException(nameof(dataDir));
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            string path = Path.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                return serializer.Deserialize<BallisticsWorkbenchCatalog>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load {FileName}: {ex.Message}", ex);
            }
        }
    }
}
