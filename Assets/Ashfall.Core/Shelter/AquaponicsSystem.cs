// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    // ── Catalog DTOs ─────────────────────────────────────────────────

    [Serializable]
    public sealed class AquaponicsTankClassDef
    {
        public string tank_class_id = string.Empty;
        public string display_name = string.Empty;
        public float volume_litres = 400f;
        public float max_biomass_kg = 12f;
        public float plant_bed_capacity = 4f;
        public float power_draw_watts = 90f;
        public float aeration_rate = 1.4f;
        public float do_sat = 9f;
        public float do_crash_rate = 1.8f;
        public float do_respiration_per_kg = 0.12f;
        public float heat_kw_per_power = 0.04f;
        public string room_id_default = "room_greenhouse";
    }

    [Serializable]
    public sealed class AquaponicsSpeciesDef
    {
        public string species_id = string.Empty;
        public string display_name = string.Empty;
        public float fcr_inverse = 0.5f;
        public float ration_kg_per_kg = 0.03f;
        public float n_excretion_frac = 0.35f;
        public float temp_band_min = 0.5f;
        public float temp_band_max = 1.1f;
        public float do_warn = 3f;
        public float n_warn = 8f;
        public string harvest_item_id = string.Empty;
        public float harvest_kg_per_item = 0.5f;
        public float min_harvest_biomass_kg = 2f;
    }

    [Serializable]
    public sealed class AquaponicsFeedDef
    {
        public string feed_id = string.Empty;
        public string item_id = string.Empty;
        public float nutrition = 1f;
        public float n_factor = 1f;
    }

    [Serializable]
    public sealed class AquaponicsBiofilterDef
    {
        public string biofilter_id = string.Empty;
        public string media_item_id = string.Empty;
        public float media_capacity = 2.5f;
        public float fouling_rate = 0.7f;
        public float stress_fouling = 1.4f;
        public float backwash_restore = 35f;
        public int media_cost_per_backwash = 1;
    }

    [Serializable]
    public sealed class AquaponicsDiseaseBandDef
    {
        public string band_id = string.Empty;
        public int ordinal;
        public float growth_penalty = 1f;
        public float mortality;
    }

    [Serializable]
    public sealed class AquaponicsPlantProfileDef
    {
        public string plant_profile_id = string.Empty;
        public string yield_item_id = string.Empty;
        public int yield_amount = 1;
        public float nitrate_per_yield = 1f;
    }

    [Serializable]
    public sealed class AquaponicsCatalog
    {
        public int schema_version = 1;
        public List<AquaponicsTankClassDef> tank_classes = new List<AquaponicsTankClassDef>();
        public List<AquaponicsSpeciesDef> species = new List<AquaponicsSpeciesDef>();
        public List<AquaponicsFeedDef> feeds = new List<AquaponicsFeedDef>();
        public List<AquaponicsBiofilterDef> biofilters = new List<AquaponicsBiofilterDef>();
        public List<AquaponicsDiseaseBandDef> disease_bands = new List<AquaponicsDiseaseBandDef>();
        public List<AquaponicsPlantProfileDef> plant_profiles = new List<AquaponicsPlantProfileDef>();
    }

    // ── Runtime / save DTOs ──────────────────────────────────────────

    [Serializable]
    public sealed class AquaponicsTankState
    {
        public string tankId = string.Empty;
        public string tankClassId = string.Empty;
        public string roomId = string.Empty;
        public string speciesId = string.Empty;
        public string biofilterId = string.Empty;
        public string plantProfileId = string.Empty;
        public float biomassKg;
        public float feedStockKg;
        public float dissolvedOxygen = 7f;
        public float nLoad;
        public float nitratePool;
        public float biofilterHealth = 100f;
        public float plantHealth = 1f;
        public float tankTempProxyC = 20f;
        public float powerAvailability01 = 1f;
        public int diseaseBandOrdinal;
        public int commissionedDay = -1;
        public int lastTickDay = -1;
        public int lastDiseaseRollDay = -1;
        public int lastHarvestDay = -1;
        public int lastBackwashDay = -1;
        public bool powerStarved;
    }

    [Serializable]
    public sealed class AquaponicsState
    {
        public string systemId = AquaponicsSystem.SystemId;
        public int schemaVersion = 1;
        public int lastProcessedDay = -1;
        public float exportedNitrate;
        public List<AquaponicsTankState> tanks = new List<AquaponicsTankState>();
    }

    public sealed class AquaponicsSnapshot
    {
        public string TankId = string.Empty;
        public string SpeciesId = string.Empty;
        public float BiomassKg;
        public float FeedStockKg;
        public float DissolvedOxygen;
        public float NLoad;
        public float NitratePool;
        public float BiofilterHealth;
        public float PlantHealth;
        public float PowerAvailability01;
        public int DiseaseBandOrdinal;
        public bool PowerStarved;
        public bool FishHarvestReady;
        public bool PlantHarvestReady;
    }

    public sealed class AquaponicHarvestResult
    {
        public bool Success;
        public string TankId = string.Empty;
        public string ItemId = string.Empty;
        public int Amount;
        public string Kind = string.Empty;
        public string FailureCode = string.Empty;
    }

    /// <summary>
    /// Explicit nutrient export for greenhouse consumers. Aquaponics never
    /// mutates greenhouse plots directly.
    /// </summary>
    public sealed class AquaponicNutrientSource
    {
        public float NitrateAvailable { get; set; }
        public int Day { get; set; }
        public string SourceSystemId { get; set; } = AquaponicsSystem.SystemId;
    }

    /// <summary>
    /// Plan B87 — closed-loop aquaponics ecology authority.
    /// Owns fish biomass, local feed stock, DO, N-load, biofilter health,
    /// disease pressure, and nutrient export. Does not own global water,
    /// power, room temperature, greenhouse plots, or kitchen meal prep.
    /// </summary>
    public sealed class AquaponicsSystem
    {
        public const string SystemId = "aquaponics";
        public const float MaxDiseaseRisk = 0.55f;

        private readonly InventoryContainer? _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Func<string, float> _powerAvailability;

        private AquaponicsCatalog _catalog = new AquaponicsCatalog();
        private AquaponicsState _state = new AquaponicsState();

        private readonly Dictionary<string, AquaponicsTankClassDef> _tankClasses =
            new Dictionary<string, AquaponicsTankClassDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, AquaponicsSpeciesDef> _species =
            new Dictionary<string, AquaponicsSpeciesDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, AquaponicsFeedDef> _feeds =
            new Dictionary<string, AquaponicsFeedDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, AquaponicsBiofilterDef> _biofilters =
            new Dictionary<string, AquaponicsBiofilterDef>(StringComparer.Ordinal);
        private readonly Dictionary<int, AquaponicsDiseaseBandDef> _diseaseByOrdinal =
            new Dictionary<int, AquaponicsDiseaseBandDef>();
        private readonly Dictionary<string, AquaponicsPlantProfileDef> _plants =
            new Dictionary<string, AquaponicsPlantProfileDef>(StringComparer.Ordinal);

        public AquaponicsSystem(
            ISeededRng rng,
            InventoryContainer? inventory = null,
            Func<string, float>? powerAvailability = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory;
            _powerAvailability = powerAvailability ?? (_ => 1f);
            _log = log ?? NullLog.Instance;
        }

        public AquaponicsState State => _state;
        public IReadOnlyList<AquaponicsTankState> Tanks => _state.tanks;
        public float ExportedNitrate => _state.exportedNitrate;
        public IReadOnlyDictionary<string, AquaponicsTankClassDef> TankClasses => _tankClasses;
        public IReadOnlyDictionary<string, AquaponicsSpeciesDef> Species => _species;

        public event Action? OnStateChanged;
        public event Action<string>? OnDiseaseMilestone;
        public event Action<AquaponicHarvestResult>? OnHarvested;

        public void LoadCatalog(AquaponicsCatalog? catalog)
        {
            _catalog = catalog ?? new AquaponicsCatalog();
            _tankClasses.Clear();
            _species.Clear();
            _feeds.Clear();
            _biofilters.Clear();
            _diseaseByOrdinal.Clear();
            _plants.Clear();

            foreach (var tank in _catalog.tank_classes ?? new List<AquaponicsTankClassDef>())
            {
                if (tank == null || string.IsNullOrWhiteSpace(tank.tank_class_id)) continue;
                if (tank.volume_litres <= 0f || tank.max_biomass_kg <= 0f) continue;
                _tankClasses[tank.tank_class_id] = tank;
            }

            foreach (var sp in _catalog.species ?? new List<AquaponicsSpeciesDef>())
            {
                if (sp == null || string.IsNullOrWhiteSpace(sp.species_id)) continue;
                if (sp.fcr_inverse <= 0f || sp.ration_kg_per_kg <= 0f) continue;
                _species[sp.species_id] = sp;
            }

            foreach (var feed in _catalog.feeds ?? new List<AquaponicsFeedDef>())
            {
                if (feed == null || string.IsNullOrWhiteSpace(feed.feed_id)) continue;
                _feeds[feed.feed_id] = feed;
            }

            foreach (var bf in _catalog.biofilters ?? new List<AquaponicsBiofilterDef>())
            {
                if (bf == null || string.IsNullOrWhiteSpace(bf.biofilter_id)) continue;
                _biofilters[bf.biofilter_id] = bf;
            }

            foreach (var band in _catalog.disease_bands ?? new List<AquaponicsDiseaseBandDef>())
            {
                if (band == null || string.IsNullOrWhiteSpace(band.band_id)) continue;
                _diseaseByOrdinal[band.ordinal] = band;
            }

            foreach (var plant in _catalog.plant_profiles ?? new List<AquaponicsPlantProfileDef>())
            {
                if (plant == null || string.IsNullOrWhiteSpace(plant.plant_profile_id)) continue;
                if (plant.yield_amount <= 0 || string.IsNullOrWhiteSpace(plant.yield_item_id)) continue;
                _plants[plant.plant_profile_id] = plant;
            }
        }

        public ActionResult CommissionTank(
            string tankId,
            string tankClassId,
            string roomId,
            string biofilterId = "",
            string plantProfileId = "",
            int day = 0)
        {
            if (string.IsNullOrWhiteSpace(tankId))
                return ActionResult.Failed("invalid_tank", "aquaponics.invalid_tank");
            if (FindTank(tankId) != null)
                return ActionResult.Blocked("tank_exists", "aquaponics.tank_exists");
            if (!_tankClasses.TryGetValue(tankClassId ?? string.Empty, out var tankClass))
                return ActionResult.Failed("unknown_tank_class", "aquaponics.unknown_tank_class");

            string resolvedBio = string.IsNullOrWhiteSpace(biofilterId)
                ? FirstKey(_biofilters)
                : biofilterId;
            if (string.IsNullOrEmpty(resolvedBio) || !_biofilters.ContainsKey(resolvedBio))
                return ActionResult.Failed("unknown_biofilter", "aquaponics.unknown_biofilter");

            string resolvedPlant = string.IsNullOrWhiteSpace(plantProfileId)
                ? FirstKey(_plants)
                : plantProfileId;
            if (string.IsNullOrEmpty(resolvedPlant) || !_plants.ContainsKey(resolvedPlant))
                return ActionResult.Failed("unknown_plant_profile", "aquaponics.unknown_plant_profile");

            string room = string.IsNullOrWhiteSpace(roomId) ? tankClass.room_id_default : roomId;
            _state.tanks.Add(new AquaponicsTankState
            {
                tankId = tankId,
                tankClassId = tankClassId,
                roomId = room ?? string.Empty,
                biofilterId = resolvedBio,
                plantProfileId = resolvedPlant,
                dissolvedOxygen = Math.Clamp(tankClass.do_sat * 0.75f, 0f, tankClass.do_sat),
                biofilterHealth = 100f,
                plantHealth = 1f,
                commissionedDay = day,
                lastTickDay = -1
            });
            RaiseChanged();
            return ActionResult.Success("aquaponics.tank_commissioned");
        }

        public ActionResult StockFish(string tankId, string speciesId, float biomassKg, int day)
        {
            var tank = FindTank(tankId);
            if (tank == null)
                return ActionResult.Failed("unknown_tank", "aquaponics.unknown_tank");
            if (!_species.TryGetValue(speciesId ?? string.Empty, out var species))
                return ActionResult.Failed("unknown_species", "aquaponics.unknown_species");
            if (!_tankClasses.TryGetValue(tank.tankClassId, out var tankClass))
                return ActionResult.Failed("unknown_tank_class", "aquaponics.unknown_tank_class");
            if (biomassKg <= 0f || float.IsNaN(biomassKg) || float.IsInfinity(biomassKg))
                return ActionResult.Failed("invalid_biomass", "aquaponics.invalid_biomass");
            if (!string.IsNullOrEmpty(tank.speciesId) && tank.biomassKg > 0.01f)
                return ActionResult.Blocked("tank_stocked", "aquaponics.tank_stocked");

            float frySurvival = 0.55f + (float)_rng.NextDouble() * 0.4f;
            float stocked = Math.Min(tankClass.max_biomass_kg, biomassKg * Math.Clamp(frySurvival, 0.2f, 1f));
            tank.speciesId = speciesId;
            tank.biomassKg = stocked;
            tank.diseaseBandOrdinal = 0;
            RaiseChanged();
            return ActionResult.Success("aquaponics.stocked");
        }

        public ActionResult Feed(string tankId, string feedId, float feedKg, int day)
        {
            var tank = FindTank(tankId);
            if (tank == null)
                return ActionResult.Failed("unknown_tank", "aquaponics.unknown_tank");
            if (!_feeds.TryGetValue(feedId ?? string.Empty, out var feed))
                return ActionResult.Failed("unknown_feed", "aquaponics.unknown_feed");
            if (feedKg <= 0f || float.IsNaN(feedKg))
                return ActionResult.Failed("invalid_feed", "aquaponics.invalid_feed");
            if (_inventory == null)
                return ActionResult.Failed("no_inventory", "aquaponics.no_inventory");

            int units = Math.Max(1, (int)Math.Ceiling(feedKg));
            var bill = new InventoryBill().AddCost(feed.item_id, units);
            bool ok = _inventory.TryExecuteTransaction(bill, () =>
            {
                tank.feedStockKg += feedKg * Math.Clamp(feed.nutrition, 0.1f, 2f);
            });
            if (!ok)
                return ActionResult.Blocked("insufficient_feed", "aquaponics.insufficient_feed");

            RaiseChanged();
            return ActionResult.Success("aquaponics.fed");
        }

        public ActionResult TopUpWater(string tankId, float litres, float quality01)
        {
            var tank = FindTank(tankId);
            if (tank == null)
                return ActionResult.Failed("unknown_tank", "aquaponics.unknown_tank");
            if (litres <= 0f || float.IsNaN(litres))
                return ActionResult.Failed("invalid_water", "aquaponics.invalid_water");

            // Local ecology response only — water authority deduction is host-owned.
            float q = Math.Clamp(quality01, 0f, 1f);
            tank.dissolvedOxygen = Math.Clamp(tank.dissolvedOxygen + litres * 0.01f * q, 0f, 12f);
            tank.nLoad = Math.Max(0f, tank.nLoad - litres * 0.02f * q);
            RaiseChanged();
            return ActionResult.Success("aquaponics.water_topped");
        }

        public ActionResult BackwashBiofilter(string tankId, int day)
        {
            var tank = FindTank(tankId);
            if (tank == null)
                return ActionResult.Failed("unknown_tank", "aquaponics.unknown_tank");
            if (!_biofilters.TryGetValue(tank.biofilterId, out var bf))
                return ActionResult.Failed("unknown_biofilter", "aquaponics.unknown_biofilter");
            if (_inventory == null)
                return ActionResult.Failed("no_inventory", "aquaponics.no_inventory");

            int cost = Math.Max(0, bf.media_cost_per_backwash);
            if (cost > 0)
            {
                var bill = new InventoryBill().AddCost(bf.media_item_id, cost);
                bool ok = _inventory.TryExecuteTransaction(bill, () =>
                {
                    tank.biofilterHealth = Math.Clamp(
                        tank.biofilterHealth + bf.backwash_restore, 0f, 100f);
                    tank.lastBackwashDay = day;
                });
                if (!ok)
                    return ActionResult.Blocked("insufficient_media", "aquaponics.insufficient_media");
            }
            else
            {
                tank.biofilterHealth = Math.Clamp(
                    tank.biofilterHealth + bf.backwash_restore, 0f, 100f);
                tank.lastBackwashDay = day;
            }

            RaiseChanged();
            return ActionResult.Success("aquaponics.biofilter_backwashed");
        }

        public ActionResult TreatDisease(string tankId, int day)
        {
            var tank = FindTank(tankId);
            if (tank == null)
                return ActionResult.Failed("unknown_tank", "aquaponics.unknown_tank");
            if (tank.diseaseBandOrdinal <= 0)
                return ActionResult.Blocked("no_disease", "aquaponics.no_disease");

            tank.diseaseBandOrdinal = Math.Max(0, tank.diseaseBandOrdinal - 1);
            RaiseChanged();
            return ActionResult.Success("aquaponics.disease_treated");
        }

        public AquaponicHarvestResult HarvestFish(string tankId, float biomassKg, int day)
        {
            var result = new AquaponicHarvestResult { TankId = tankId ?? string.Empty, Kind = "fish" };
            var tank = FindTank(tankId);
            if (tank == null)
            {
                result.FailureCode = "unknown_tank";
                return result;
            }
            if (!_species.TryGetValue(tank.speciesId, out var species))
            {
                result.FailureCode = "unknown_species";
                return result;
            }
            if (tank.biomassKg < species.min_harvest_biomass_kg)
            {
                result.FailureCode = "biomass_too_low";
                return result;
            }
            if (_inventory == null)
            {
                result.FailureCode = "no_inventory";
                return result;
            }

            float take = biomassKg <= 0f ? tank.biomassKg * 0.35f : biomassKg;
            take = Math.Clamp(take, 0.1f, tank.biomassKg);
            float perItem = Math.Max(0.1f, species.harvest_kg_per_item);
            int amount = Math.Max(1, (int)Math.Floor(take / perItem));
            if (amount <= 0)
            {
                result.FailureCode = "yield_too_small";
                return result;
            }

            var bill = new InventoryBill().AddGrant(species.harvest_item_id, amount);
            bool ok = _inventory.TryExecuteTransaction(bill, () =>
            {
                tank.biomassKg = Math.Max(0f, tank.biomassKg - amount * perItem);
                tank.lastHarvestDay = day;
            });
            if (!ok)
            {
                result.FailureCode = "inventory_full";
                return result;
            }

            result.Success = true;
            result.ItemId = species.harvest_item_id;
            result.Amount = amount;
            OnHarvested?.Invoke(result);
            RaiseChanged();
            return result;
        }

        public AquaponicHarvestResult HarvestPlants(string tankId, int day)
        {
            var result = new AquaponicHarvestResult { TankId = tankId ?? string.Empty, Kind = "plants" };
            var tank = FindTank(tankId);
            if (tank == null)
            {
                result.FailureCode = "unknown_tank";
                return result;
            }
            if (!_plants.TryGetValue(tank.plantProfileId, out var plant))
            {
                result.FailureCode = "unknown_plant_profile";
                return result;
            }
            if (tank.nitratePool < plant.nitrate_per_yield || tank.plantHealth < 0.35f)
            {
                result.FailureCode = "plants_not_ready";
                return result;
            }
            if (_inventory == null)
            {
                result.FailureCode = "no_inventory";
                return result;
            }

            int cycles = Math.Max(1, (int)Math.Floor(tank.nitratePool / Math.Max(0.1f, plant.nitrate_per_yield)));
            cycles = Math.Min(cycles, 4);
            int amount = plant.yield_amount * cycles;
            var bill = new InventoryBill().AddGrant(plant.yield_item_id, amount);
            bool ok = _inventory.TryExecuteTransaction(bill, () =>
            {
                tank.nitratePool = Math.Max(0f, tank.nitratePool - plant.nitrate_per_yield * cycles);
                tank.lastHarvestDay = day;
            });
            if (!ok)
            {
                result.FailureCode = "inventory_full";
                return result;
            }

            result.Success = true;
            result.ItemId = plant.yield_item_id;
            result.Amount = amount;
            OnHarvested?.Invoke(result);
            RaiseChanged();
            return result;
        }

        public void TickDay(int day, float temperatureModifier = 1f, float pumpReliability01 = 1f)
        {
            if (day <= _state.lastProcessedDay) return;
            _state.lastProcessedDay = day;
            _state.exportedNitrate = 0f;

            float tempMod = Math.Clamp(temperatureModifier, 0.1f, 1.5f);
            float pumpRel = Math.Clamp(pumpReliability01, 0f, 1f);

            foreach (var tank in _state.tanks)
            {
                if (!_tankClasses.TryGetValue(tank.tankClassId, out var tankClass)) continue;
                float power = Math.Clamp(_powerAvailability(tank.roomId), 0f, 1f) * pumpRel;
                tank.powerAvailability01 = power;
                tank.powerStarved = power <= 0.05f;
                tank.lastTickDay = day;
                tank.tankTempProxyC = 12f + tempMod * 14f;

                // Dissolved oxygen: aeration vs respiration; power loss crashes DO.
                float doDelta = tankClass.aeration_rate * power
                    - tank.biomassKg * tankClass.do_respiration_per_kg
                    - (tank.powerStarved ? tankClass.do_crash_rate : 0f);
                tank.dissolvedOxygen = Math.Clamp(
                    tank.dissolvedOxygen + doDelta, 0f, tankClass.do_sat);

                AquaponicsSpeciesDef? species = null;
                if (!string.IsNullOrEmpty(tank.speciesId))
                    _species.TryGetValue(tank.speciesId, out species);

                float diseasePenalty = ResolveDisease(tank.diseaseBandOrdinal)?.growth_penalty ?? 1f;
                float mortalityFrac = ResolveDisease(tank.diseaseBandOrdinal)?.mortality ?? 0f;

                if (species != null && tank.biomassKg > 0f)
                {
                    float ration = Math.Max(0.001f, species.ration_kg_per_kg) * tank.biomassKg;
                    float fed = Math.Min(tank.feedStockKg, ration);
                    tank.feedStockKg = Math.Max(0f, tank.feedStockKg - fed);

                    float tempFit = TempFit(tempMod, species.temp_band_min, species.temp_band_max);
                    float doFit = DoFit(tank.dissolvedOxygen, species.do_warn, tankClass.do_sat);
                    float growth = fed * Math.Clamp(species.fcr_inverse, 0.05f, 1.5f)
                        * tempFit * doFit * diseasePenalty;
                    float mortality = tank.biomassKg * mortalityFrac;
                    if (fed < ration * 0.25f) mortality += tank.biomassKg * 0.03f;
                    if (tank.powerStarved) mortality += tank.biomassKg * 0.05f;

                    tank.biomassKg = Math.Clamp(
                        tank.biomassKg + growth - mortality, 0f, tankClass.max_biomass_kg);
                    tank.nLoad = Math.Max(0f,
                        tank.nLoad + fed * Math.Clamp(species.n_excretion_frac, 0f, 1f)
                        + mortality * 0.25f);
                }

                if (_biofilters.TryGetValue(tank.biofilterId, out var bf))
                {
                    float capacity = Math.Max(0.1f, bf.media_capacity)
                        * (tank.biofilterHealth / 100f)
                        * TempFit(tempMod, 0.4f, 1.2f);
                    float converted = Math.Min(tank.nLoad, capacity);
                    tank.nLoad = Math.Max(0f, tank.nLoad - converted);
                    tank.nitratePool += converted;

                    float fouling = bf.fouling_rate;
                    if (species != null && tank.nLoad > species.n_warn)
                        fouling += bf.stress_fouling;
                    if (tank.powerStarved) fouling += bf.fouling_rate * 0.5f;
                    tank.biofilterHealth = Math.Clamp(tank.biofilterHealth - fouling, 0f, 100f);
                }

                if (_plants.TryGetValue(tank.plantProfileId, out var plant)
                    && tankClass.plant_bed_capacity > 0f)
                {
                    float uptake = Math.Min(
                        tank.nitratePool,
                        tankClass.plant_bed_capacity * Math.Clamp(tank.plantHealth, 0f, 1.5f) * 0.35f);
                    tank.nitratePool = Math.Max(0f, tank.nitratePool - uptake);
                    _state.exportedNitrate += uptake;
                    float plantStress = 0f;
                    if (tank.nitratePool < 0.2f) plantStress += 0.04f;
                    if (tank.powerStarved) plantStress += 0.06f;
                    tank.plantHealth = Math.Clamp(tank.plantHealth - plantStress + uptake * 0.02f, 0.05f, 1.5f);
                }

                // Disease pressure (seeded).
                if (species != null && tank.lastDiseaseRollDay != day
                    && tank.diseaseBandOrdinal < MaxDiseaseOrdinal())
                {
                    tank.lastDiseaseRollDay = day;
                    float risk = 0.015f;
                    if (tank.dissolvedOxygen < species.do_warn) risk += 0.12f;
                    if (tank.nLoad > species.n_warn) risk += 0.1f;
                    if (tempMod < species.temp_band_min || tempMod > species.temp_band_max) risk += 0.08f;
                    if (tank.powerStarved) risk += 0.1f;
                    if (tank.biofilterHealth < 35f) risk += 0.07f;
                    if (_rng.NextDouble() < Math.Clamp(risk, 0f, MaxDiseaseRisk))
                    {
                        tank.diseaseBandOrdinal = Math.Min(MaxDiseaseOrdinal(), tank.diseaseBandOrdinal + 1);
                        OnDiseaseMilestone?.Invoke(tank.tankId);
                    }
                }

                RaiseChanged();
            }
        }

        public AquaponicNutrientSource GetNutrientExport()
        {
            return new AquaponicNutrientSource
            {
                NitrateAvailable = _state.exportedNitrate,
                Day = _state.lastProcessedDay,
                SourceSystemId = SystemId
            };
        }

        public AquaponicsSnapshot Snapshot(string tankId)
        {
            var tank = FindTank(tankId);
            if (tank == null) return new AquaponicsSnapshot();
            bool fishReady = false;
            if (_species.TryGetValue(tank.speciesId, out var species))
                fishReady = tank.biomassKg >= species.min_harvest_biomass_kg;
            bool plantReady = false;
            if (_plants.TryGetValue(tank.plantProfileId, out var plant))
                plantReady = tank.nitratePool >= plant.nitrate_per_yield && tank.plantHealth >= 0.35f;

            return new AquaponicsSnapshot
            {
                TankId = tank.tankId,
                SpeciesId = tank.speciesId,
                BiomassKg = tank.biomassKg,
                FeedStockKg = tank.feedStockKg,
                DissolvedOxygen = tank.dissolvedOxygen,
                NLoad = tank.nLoad,
                NitratePool = tank.nitratePool,
                BiofilterHealth = tank.biofilterHealth,
                PlantHealth = tank.plantHealth,
                PowerAvailability01 = tank.powerAvailability01,
                DiseaseBandOrdinal = tank.diseaseBandOrdinal,
                PowerStarved = tank.powerStarved,
                FishHarvestReady = fishReady,
                PlantHarvestReady = plantReady
            };
        }

        public AquaponicsState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<AquaponicsState>(serializer.Serialize(_state))
                ?? new AquaponicsState();
        }

        public void RestoreState(AquaponicsState? saved)
        {
            if (saved == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<AquaponicsState>(serializer.Serialize(saved))
                ?? new AquaponicsState();
            if (_state.tanks == null) _state.tanks = new List<AquaponicsTankState>();
            _state.systemId = SystemId;
        }

        private AquaponicsTankState? FindTank(string tankId)
        {
            if (string.IsNullOrWhiteSpace(tankId)) return null;
            for (int i = 0; i < _state.tanks.Count; i++)
            {
                if (string.Equals(_state.tanks[i].tankId, tankId, StringComparison.Ordinal))
                    return _state.tanks[i];
            }
            return null;
        }

        private AquaponicsDiseaseBandDef? ResolveDisease(int ordinal)
        {
            return _diseaseByOrdinal.TryGetValue(ordinal, out var band) ? band : null;
        }

        private int MaxDiseaseOrdinal()
        {
            int max = 0;
            foreach (var kv in _diseaseByOrdinal)
                if (kv.Key > max) max = kv.Key;
            return max;
        }

        private static float TempFit(float tempMod, float min, float max)
        {
            if (tempMod < min) return Math.Clamp(tempMod / Math.Max(0.01f, min), 0.1f, 1f);
            if (tempMod > max) return Math.Clamp(max / Math.Max(0.01f, tempMod), 0.1f, 1f);
            return 1f;
        }

        private static float DoFit(float dissolvedOxygen, float warn, float sat)
        {
            if (dissolvedOxygen <= 0.1f) return 0.05f;
            if (dissolvedOxygen < warn)
                return Math.Clamp(dissolvedOxygen / Math.Max(0.1f, warn), 0.1f, 0.85f);
            return Math.Clamp(0.85f + 0.15f * (dissolvedOxygen / Math.Max(0.1f, sat)), 0.1f, 1.1f);
        }

        private static string FirstKey<T>(Dictionary<string, T> map)
        {
            foreach (var kv in map) return kv.Key;
            return string.Empty;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }

    /// <summary>Loads <c>aquaponics_system_catalog.json</c>.</summary>
    public static class AquaponicsCatalogLoader
    {
        public const string DefaultFileName = "aquaponics_system_catalog.json";

        public static AquaponicsCatalog Load(
            string dataDirectory,
            IFileIO files,
            IJsonSerializer json,
            ILog? log = null)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));
            string path = Path.Combine(dataDirectory ?? string.Empty, DefaultFileName);
            if (!files.FileExists(path))
            {
                log?.Warn($"[Aquaponics] catalog missing at {path}; idle empty catalog.");
                return new AquaponicsCatalog();
            }

            string text = files.ReadAllText(path);
            var catalog = json.Deserialize<AquaponicsCatalog>(text)
                          ?? throw new InvalidOperationException("Failed to deserialize aquaponics_system_catalog.json");
            return catalog;
        }

        public static void Validate(AquaponicsCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (catalog.schema_version < 1)
                throw new InvalidOperationException("aquaponics_system_catalog schema_version must be >= 1");

            RejectDupes(catalog.tank_classes, t => t.tank_class_id, "tank_class_id");
            RejectDupes(catalog.species, s => s.species_id, "species_id");
            RejectDupes(catalog.feeds, f => f.feed_id, "feed_id");
            RejectDupes(catalog.biofilters, b => b.biofilter_id, "biofilter_id");
            RejectDupes(catalog.disease_bands, d => d.band_id, "band_id");
            RejectDupes(catalog.plant_profiles, p => p.plant_profile_id, "plant_profile_id");

            if (catalog.tank_classes == null || catalog.tank_classes.Count == 0)
                throw new InvalidOperationException("aquaponics_system_catalog requires at least one tank class");
            if (catalog.species == null || catalog.species.Count == 0)
                throw new InvalidOperationException("aquaponics_system_catalog requires at least one species");
            if (catalog.biofilters == null || catalog.biofilters.Count == 0)
                throw new InvalidOperationException("aquaponics_system_catalog requires at least one biofilter");
            if (catalog.plant_profiles == null || catalog.plant_profiles.Count == 0)
                throw new InvalidOperationException("aquaponics_system_catalog requires at least one plant profile");
        }

        private static void RejectDupes<T>(List<T>? items, Func<T, string> idOf, string label)
        {
            if (items == null) return;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in items)
            {
                if (item == null) continue;
                string id = idOf(item) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id)) continue;
                if (!seen.Add(id))
                    throw new InvalidOperationException($"Duplicate aquaponics {label}: {id}");
            }
        }
    }
}
