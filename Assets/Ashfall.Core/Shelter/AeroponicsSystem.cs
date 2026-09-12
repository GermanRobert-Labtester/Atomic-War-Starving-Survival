// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public enum AeroponicLightMode
    {
        Vegetative = 0,
        Fruiting = 1,
        Balanced = 2
    }

    public enum RootDiseaseStage
    {
        Clear = 0,
        Early = 1,
        Established = 2,
        Severe = 3,
        CropLoss = 4
    }

    [Serializable]
    public sealed class AeroponicNutrientDefinition
    {
        [JsonPropertyName("nutrient_profile_id")]
        public string NutrientProfileId { get; set; } = string.Empty;

        [JsonPropertyName("crop_tag")]
        public string CropTag { get; set; } = string.Empty;

        [JsonPropertyName("nitrogen")]
        public float Nitrogen { get; set; }

        [JsonPropertyName("phosphorus")]
        public float Phosphorus { get; set; }

        [JsonPropertyName("potassium")]
        public float Potassium { get; set; }

        [JsonPropertyName("iron")]
        public float Iron { get; set; }

        [JsonPropertyName("calcium")]
        public float Calcium { get; set; }

        [JsonPropertyName("magnesium")]
        public float Magnesium { get; set; }

        [JsonPropertyName("optimal_ec_min")]
        public float OptimalEcMin { get; set; } = 1f;

        [JsonPropertyName("optimal_ec_max")]
        public float OptimalEcMax { get; set; } = 2.5f;

        [JsonPropertyName("optimal_ph_min")]
        public float OptimalPhMin { get; set; } = 5.5f;

        [JsonPropertyName("optimal_ph_max")]
        public float OptimalPhMax { get; set; } = 6.5f;

        [JsonPropertyName("yield_item_id")]
        public string YieldItemId { get; set; } = string.Empty;

        [JsonPropertyName("yield_amount")]
        public int YieldAmount { get; set; } = 1;

        [JsonPropertyName("medicinal")]
        public bool Medicinal { get; set; }

        [JsonPropertyName("base_growth_per_day")]
        public float BaseGrowthPerDay { get; set; } = 10f;
    }

    [Serializable]
    public sealed class AeroponicsNutrientCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("profiles")]
        public List<AeroponicNutrientDefinition> Profiles { get; set; } =
            new List<AeroponicNutrientDefinition>();
    }

    [Serializable]
    public sealed class AeroponicChamberState
    {
        public string ChamberId = string.Empty;
        public string RoomId = string.Empty;
        public string CropCycleId = string.Empty;
        public string NutrientProfileId = string.Empty;
        public float MistIntervalSeconds = 30f;
        public float ReservoirEcMsCm = 1.5f;
        public float ReservoirPh = 6f;
        public float NutrientBalance = 1f;
        public float RootBiomass;
        public float GrowthPct;
        public float NozzleConditionPct = 100f;
        public float ReservoirTemperatureC = 20f;
        public RootDiseaseStage DiseaseStage;
        public AeroponicLightMode LightMode = AeroponicLightMode.Balanced;
        public float WaterQuality = 1f;
        public float ReservoirLitres;
        public float PowerAvailability01 = 1f;
        public int PlantedDay = -1;
        public int LastMaintenanceDay = -1;
        public int LastDiseaseRollDay = -1;
        public int LastHarvestDay = -1;
        public bool HarvestResolved;
    }

    [Serializable]
    public sealed class AeroponicsState
    {
        public string SystemId = AeroponicsSystem.SystemId;
        public List<AeroponicChamberState> Chambers = new List<AeroponicChamberState>();
        public int LastProcessedDay = -1;
    }

    public sealed class AeroponicsSnapshot
    {
        public string ChamberId = string.Empty;
        public string CropCycleId = string.Empty;
        public float GrowthPct;
        public float RootBiomass;
        public float ReservoirEcMsCm;
        public float ReservoirPh;
        public float NozzleConditionPct;
        public RootDiseaseStage DiseaseStage;
        public AeroponicLightMode LightMode;
        public float PowerAvailability01;
        public float ReservoirLitres;
        public bool HarvestReady;
    }

    public sealed class AeroponicHarvestResult
    {
        public bool Success;
        public string ItemId = string.Empty;
        public int Amount;
        public bool Medicinal;
    }

    /// <summary>
    /// Chamber-local aeroponic ecology. Crop inventory remains owned by
    /// Inventory/Greenhouse/Kitchen/Pharma authorities.
    /// </summary>
    public sealed class AeroponicsSystem
    {
        public const string SystemId = "aeroponics";
        public const float MaximumGrowthMultiplier = 3f;
        public const float BaseWaterPerDayLitres = 4f;

        private AeroponicsState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory? _inventory;
        private readonly Func<string, float> _powerAvailability;
        private readonly Dictionary<string, AeroponicNutrientDefinition> _profiles =
            new Dictionary<string, AeroponicNutrientDefinition>(StringComparer.Ordinal);

        public AeroponicsSystem(
            ISeededRng rng,
            Inventory.Inventory? inventory = null,
            Func<string, float>? powerAvailability = null,
            AeroponicsState? state = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory;
            _powerAvailability = powerAvailability ?? (_ => 1f);
            _state = state ?? new AeroponicsState();
            _log = log ?? NullLog.Instance;
            NormalizeState();
        }

        public AeroponicsState State => _state;
        public IReadOnlyDictionary<string, AeroponicNutrientDefinition> Profiles => _profiles;
        public IReadOnlyList<AeroponicChamberState> Chambers => _state.Chambers;

        public event Action<AeroponicChamberState>? OnChamberChanged;
        public event Action<string>? OnDiseaseMilestone;
        public event Action<AeroponicHarvestResult>? OnHarvested;

        public void LoadCatalog(AeroponicsNutrientCatalog? catalog)
        {
            if (catalog?.Profiles == null) return;
            _profiles.Clear();
            foreach (var profile in catalog.Profiles)
            {
                if (profile == null || string.IsNullOrWhiteSpace(profile.NutrientProfileId))
                    continue;
                if (profile.OptimalEcMin > profile.OptimalEcMax ||
                    profile.OptimalPhMin > profile.OptimalPhMax ||
                    profile.YieldAmount <= 0 || profile.BaseGrowthPerDay <= 0f)
                    continue;
                _profiles[profile.NutrientProfileId] = profile;
                if (!string.IsNullOrWhiteSpace(profile.CropTag))
                    _profiles[profile.CropTag] = profile;
            }
        }

        public void RegisterProfile(AeroponicNutrientDefinition profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.NutrientProfileId))
                return;
            _profiles[profile.NutrientProfileId] = profile;
            if (!string.IsNullOrWhiteSpace(profile.CropTag))
                _profiles[profile.CropTag] = profile;
        }

        public ActionResult AddChamber(string chamberId, string roomId)
        {
            if (string.IsNullOrWhiteSpace(chamberId))
                return ActionResult.Failed("invalid_chamber", "aeroponics.invalid_chamber");
            if (FindChamber(chamberId) != null)
                return ActionResult.Blocked("chamber_exists", "aeroponics.chamber_exists");
            _state.Chambers.Add(new AeroponicChamberState
            {
                ChamberId = chamberId,
                RoomId = roomId ?? string.Empty,
                ReservoirLitres = 20f
            });
            return ActionResult.Success("aeroponics.chamber_added");
        }

        public ActionResult Plant(string chamberId, string cropCycleId, string nutrientProfileId, int day)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            if (string.IsNullOrWhiteSpace(cropCycleId) ||
                ResolveProfile(nutrientProfileId) == null)
                return ActionResult.Failed("unknown_crop", "aeroponics.unknown_crop");
            if (!string.IsNullOrEmpty(chamber.CropCycleId))
                return ActionResult.Blocked("chamber_busy", "aeroponics.chamber_busy");

            chamber.CropCycleId = cropCycleId;
            chamber.NutrientProfileId = nutrientProfileId;
            chamber.GrowthPct = 0f;
            chamber.RootBiomass = 0.1f;
            chamber.DiseaseStage = RootDiseaseStage.Clear;
            chamber.PlantedDay = day;
            chamber.HarvestResolved = false;
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.planted");
        }

        public ActionResult SetChemistry(string chamberId, float ecMsCm, float ph)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            if (float.IsNaN(ecMsCm) || float.IsInfinity(ecMsCm) ||
                float.IsNaN(ph) || float.IsInfinity(ph))
                return ActionResult.Failed("invalid_chemistry", "aeroponics.invalid_chemistry");
            chamber.ReservoirEcMsCm = Math.Clamp(ecMsCm, 0f, 6f);
            chamber.ReservoirPh = Math.Clamp(ph, 2f, 10f);
            chamber.NutrientBalance = ChemistryFit(chamber, ResolveProfile(chamber.NutrientProfileId));
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.chemistry_set");
        }

        public ActionResult SetLightMode(string chamberId, AeroponicLightMode mode)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            chamber.LightMode = mode;
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.light_mode_set");
        }

        public ActionResult AddWater(string chamberId, float litres, float quality01)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            if (litres <= 0f || float.IsNaN(litres))
                return ActionResult.Failed("invalid_water", "aeroponics.invalid_water");
            chamber.ReservoirLitres = Math.Min(200f, chamber.ReservoirLitres + litres);
            chamber.WaterQuality = Math.Clamp(quality01, 0f, 1f);
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.water_added");
        }

        public ActionResult MaintainNozzles(string chamberId, float amount, int day)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            chamber.NozzleConditionPct = Math.Clamp(chamber.NozzleConditionPct + amount, 0f, 100f);
            chamber.LastMaintenanceDay = day;
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.nozzles_serviced");
        }

        public ActionResult TreatRootDisease(string chamberId, int day)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null)
                return ActionResult.Failed("unknown_chamber", "aeroponics.unknown_chamber");
            if (chamber.DiseaseStage == RootDiseaseStage.Clear)
                return ActionResult.Blocked("no_disease", "aeroponics.no_disease");
            chamber.DiseaseStage = chamber.DiseaseStage == RootDiseaseStage.CropLoss
                ? RootDiseaseStage.Early
                : RootDiseaseStage.Clear;
            chamber.ReservoirTemperatureC = Math.Min(chamber.ReservoirTemperatureC, 22f);
            chamber.LastMaintenanceDay = day;
            OnChamberChanged?.Invoke(chamber);
            return ActionResult.Success("aeroponics.disease_treated");
        }

        public void TickDay(int day, float operatorSkill = 0f)
        {
            if (day <= _state.LastProcessedDay) return;
            _state.LastProcessedDay = day;
            foreach (var chamber in _state.Chambers)
            {
                if (string.IsNullOrWhiteSpace(chamber.CropCycleId)) continue;
                float power = Math.Clamp(_powerAvailability(chamber.RoomId), 0f, 1f);
                chamber.PowerAvailability01 = power;
                var profile = ResolveProfile(chamber.NutrientProfileId);
                if (profile == null) continue;
                bool misting = power > 0.05f && chamber.ReservoirLitres > 0.05f &&
                    chamber.NozzleConditionPct > 0f;
                if (misting)
                {
                    chamber.ReservoirLitres = Math.Max(0f,
                        chamber.ReservoirLitres - BaseWaterPerDayLitres * power);
                    chamber.NozzleConditionPct = Math.Max(0f,
                        chamber.NozzleConditionPct -
                        (0.45f + (1f - chamber.WaterQuality) * 0.4f));
                    float chemistry = ChemistryFit(chamber, profile);
                    chamber.NutrientBalance = chemistry;
                    float light = LightModifier(chamber.LightMode);
                    float disease = chamber.DiseaseStage switch
                    {
                        RootDiseaseStage.Clear => 1f,
                        RootDiseaseStage.Early => 0.82f,
                        RootDiseaseStage.Established => 0.55f,
                        RootDiseaseStage.Severe => 0.2f,
                        _ => 0f
                    };
                    float skill = 1f + Math.Clamp(operatorSkill, 0f, 1f) * 0.15f;
                    float growthMultiplier = Math.Clamp(
                        power * chemistry * light * disease * skill, 0f, MaximumGrowthMultiplier);
                    chamber.GrowthPct = Math.Min(100f,
                        chamber.GrowthPct + profile.BaseGrowthPerDay * growthMultiplier);
                    chamber.RootBiomass = Math.Max(0f,
                        chamber.RootBiomass + growthMultiplier * 0.5f);
                }

                float diseaseRisk = 0.01f;
                if (chamber.ReservoirTemperatureC > 24f) diseaseRisk += 0.08f;
                if (chamber.NozzleConditionPct < 35f) diseaseRisk += 0.06f;
                if (chamber.WaterQuality < 0.55f) diseaseRisk += 0.08f;
                if (!misting) diseaseRisk += 0.05f;
                if (chamber.LastDiseaseRollDay != day && chamber.DiseaseStage < RootDiseaseStage.Severe)
                {
                    chamber.LastDiseaseRollDay = day;
                    if (_rng.NextDouble() < Math.Clamp(diseaseRisk, 0f, 0.5f))
                    {
                        chamber.DiseaseStage = chamber.DiseaseStage < RootDiseaseStage.Established
                            ? RootDiseaseStage.Established
                            : RootDiseaseStage.Severe;
                        OnDiseaseMilestone?.Invoke(chamber.ChamberId);
                    }
                }

                if (chamber.NozzleConditionPct <= 0f && chamber.DiseaseStage == RootDiseaseStage.Severe)
                    chamber.DiseaseStage = RootDiseaseStage.CropLoss;
                OnChamberChanged?.Invoke(chamber);
            }
        }

        public AeroponicHarvestResult Harvest(string chamberId, int day)
        {
            var result = new AeroponicHarvestResult();
            var chamber = FindChamber(chamberId);
            if (chamber == null || string.IsNullOrWhiteSpace(chamber.CropCycleId) ||
                chamber.GrowthPct < 100f || chamber.HarvestResolved)
                return result;
            var profile = ResolveProfile(chamber.NutrientProfileId);
            if (profile == null || _inventory == null) return result;
            int amount = Math.Max(1, (int)Math.Round(profile.YieldAmount *
                Math.Clamp(chamber.RootBiomass / 5f, 0.5f, 2f)));
            if (!_inventory.AddById(profile.YieldItemId, amount))
                return result;
            result.Success = true;
            result.ItemId = profile.YieldItemId;
            result.Amount = amount;
            result.Medicinal = profile.Medicinal;
            chamber.HarvestResolved = true;
            chamber.LastHarvestDay = day;
            chamber.CropCycleId = string.Empty;
            chamber.GrowthPct = 0f;
            chamber.RootBiomass = 0f;
            OnHarvested?.Invoke(result);
            OnChamberChanged?.Invoke(chamber);
            return result;
        }

        public AeroponicsSnapshot Snapshot(string chamberId)
        {
            var chamber = FindChamber(chamberId);
            if (chamber == null) return new AeroponicsSnapshot();
            return new AeroponicsSnapshot
            {
                ChamberId = chamber.ChamberId,
                CropCycleId = chamber.CropCycleId,
                GrowthPct = chamber.GrowthPct,
                RootBiomass = chamber.RootBiomass,
                ReservoirEcMsCm = chamber.ReservoirEcMsCm,
                ReservoirPh = chamber.ReservoirPh,
                NozzleConditionPct = chamber.NozzleConditionPct,
                DiseaseStage = chamber.DiseaseStage,
                LightMode = chamber.LightMode,
                PowerAvailability01 = chamber.PowerAvailability01,
                ReservoirLitres = chamber.ReservoirLitres,
                HarvestReady = chamber.GrowthPct >= 100f && !chamber.HarvestResolved
            };
        }

        public AeroponicsState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<AeroponicsState>(serializer.Serialize(_state))
                ?? new AeroponicsState();
        }

        public void RestoreState(AeroponicsState? saved)
        {
            if (saved == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<AeroponicsState>(serializer.Serialize(saved))
                ?? new AeroponicsState();
            NormalizeState();
        }

        private AeroponicChamberState? FindChamber(string chamberId)
        {
            if (string.IsNullOrWhiteSpace(chamberId)) return null;
            for (int i = 0; i < _state.Chambers.Count; i++)
                if (string.Equals(_state.Chambers[i].ChamberId, chamberId, StringComparison.Ordinal))
                    return _state.Chambers[i];
            return null;
        }

        private AeroponicNutrientDefinition? ResolveProfile(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id) && _profiles.TryGetValue(id, out var profile))
                return profile;
            return null;
        }

        private static float ChemistryFit(
            AeroponicChamberState chamber,
            AeroponicNutrientDefinition? profile)
        {
            if (profile == null) return 0f;
            float ec = chamber.ReservoirEcMsCm >= profile.OptimalEcMin &&
                        chamber.ReservoirEcMsCm <= profile.OptimalEcMax
                ? 1f
                : Math.Clamp(1f - Math.Min(
                    Math.Abs(chamber.ReservoirEcMsCm - profile.OptimalEcMin),
                    Math.Abs(chamber.ReservoirEcMsCm - profile.OptimalEcMax)) / 2f, 0.2f, 1f);
            float ph = chamber.ReservoirPh >= profile.OptimalPhMin &&
                       chamber.ReservoirPh <= profile.OptimalPhMax
                ? 1f
                : Math.Clamp(1f - Math.Min(
                    Math.Abs(chamber.ReservoirPh - profile.OptimalPhMin),
                    Math.Abs(chamber.ReservoirPh - profile.OptimalPhMax)) / 2f, 0.2f, 1f);
            return Math.Clamp((ec + ph) * 0.5f * chamber.WaterQuality, 0f, 1f);
        }

        private static float LightModifier(AeroponicLightMode mode) => mode switch
        {
            AeroponicLightMode.Vegetative => 1.05f,
            AeroponicLightMode.Fruiting => 1.10f,
            _ => 1f
        };

        private void NormalizeState()
        {
            _state.Chambers ??= new List<AeroponicChamberState>();
            foreach (var chamber in _state.Chambers)
            {
                chamber.CropCycleId ??= string.Empty;
                chamber.NozzleConditionPct = Math.Clamp(chamber.NozzleConditionPct, 0f, 100f);
                chamber.WaterQuality = Math.Clamp(chamber.WaterQuality, 0f, 1f);
                chamber.ReservoirLitres = Math.Max(0f, chamber.ReservoirLitres);
                chamber.GrowthPct = Math.Clamp(chamber.GrowthPct, 0f, 100f);
                chamber.RootBiomass = Math.Max(0f, chamber.RootBiomass);
            }
        }
    }

    public static class AeroponicsCatalogLoader
    {
        public const string FileName = "aeroponics_nutrient_catalog.json";

        public static AeroponicsNutrientCatalog? Load(
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
                return serializer.Deserialize<AeroponicsNutrientCatalog>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load {FileName}: {ex.Message}", ex);
            }
        }
    }
}
