// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Farming
{
    [Serializable]
    public sealed class FungusStrainDef
    {
        public string strain_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string category { get; set; } = "Edible";
        public int growth_days { get; set; } = 4;
        public float moisture_min { get; set; } = 0.4f;
        public float moisture_max { get; set; } = 0.9f;
        public bool darkness_required { get; set; } = true;
        public float toxicity { get; set; } = 0.0f;
        public float spore_hazard { get; set; } = 0.1f;
        public float light_output { get; set; } = 0.0f;
        public string yield_item_id { get; set; } = "harvested_mushrooms_subterranean";
        public int yield_count { get; set; } = 4;
    }

    [Serializable]
    public sealed class SubstrateDef
    {
        public string substrate_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public float nutrition_multiplier { get; set; } = 1.0f;
        public float moisture_retention { get; set; } = 0.7f;
        public float contamination_risk { get; set; } = 0.05f;
    }

    [Serializable]
    public sealed class UndergroundFloraCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<FungusStrainDef> strains { get; set; } = new List<FungusStrainDef>();
        public List<SubstrateDef> substrates { get; set; } = new List<SubstrateDef>();
    }

    [Serializable]
    public sealed class FungiPlotState
    {
        public string plotId { get; set; } = string.Empty;
        public string roomId { get; set; } = string.Empty;
        public string? strainId { get; set; } = null;
        public string? substrateId { get; set; } = null;
        public float growthStage { get; set; } = 0.0f;
        public float moisture { get; set; } = 0.6f;
        public float sporeDensity { get; set; } = 0.0f;
        public float contamination { get; set; } = 0.0f;
        public bool isHarvestReady { get; set; } = false;
        public bool hasToxicBloom { get; set; } = false;
        public int plantedDay { get; set; } = 0;
    }

    [Serializable]
    public sealed class FungiCultivationState
    {
        public int schema_version { get; set; } = 1;
        public List<FungiPlotState> plots { get; set; } = new List<FungiPlotState>();
        public int totalHarvests { get; set; } = 0;
        public int totalBlooms { get; set; } = 0;
    }

    public sealed class FungiCultivationSystem
    {
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, FungusStrainDef> _strains = new Dictionary<string, FungusStrainDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, SubstrateDef> _substrates = new Dictionary<string, SubstrateDef>(StringComparer.Ordinal);
        private FungiCultivationState _state = new FungiCultivationState();

        public event Action<string, string>? OnSporesCultivated;
        public event Action<string, string, int>? OnFungiHarvested;
        public event Action<string, string>? OnToxicBloom;
        public event Action<string>? OnBloomPurged;

        public FungiCultivationState State => _state;
        public IReadOnlyDictionary<string, FungusStrainDef> Strains => _strains;
        public IReadOnlyDictionary<string, SubstrateDef> Substrates => _substrates;

        public FungiCultivationSystem(
            ISeededRng? rng = null,
            Inventory.Inventory? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? new SeededRng(192);
            _inventory = inventory ?? new Inventory.Inventory();
            _log = log ?? NullLog.Instance;
        }

        public void RegisterCatalog(UndergroundFloraCatalog catalog)
        {
            if (catalog == null) return;
            foreach (var s in catalog.strains) _strains[s.strain_id] = s;
            foreach (var sub in catalog.substrates) _substrates[sub.substrate_id] = sub;
        }

        public FungiPlotState EnsurePlot(string plotId, string roomId)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null)
            {
                plot = new FungiPlotState
                {
                    plotId = plotId,
                    roomId = roomId,
                    moisture = 0.6f
                };
                _state.plots.Add(plot);
            }
            return plot;
        }

        public ActionResult CultivateSpores(string plotId, string strainId, string substrateId, int currentDay)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (plot.strainId != null && !plot.isHarvestReady)
                return ActionResult.Blocked("plot_occupied", "fungi.plot_occupied");

            if (!_strains.TryGetValue(strainId, out var strain))
                return ActionResult.Blocked("unknown_strain", "fungi.unknown_strain");

            if (!_substrates.TryGetValue(substrateId, out var substrate))
                return ActionResult.Blocked("unknown_substrate", "fungi.unknown_substrate");

            // Verify spore items in inventory
            string sporeItem = strain.yield_item_id;
            if (_inventory.CountById(sporeItem) <= 0 && _inventory.CountById("fungus_spores_common") <= 0)
                return ActionResult.Blocked("missing_spores", "fungi.missing_spores");

            // Consume spores
            if (_inventory.CountById(sporeItem) > 0)
                _inventory.RemoveById(sporeItem, 1);
            else
                _inventory.RemoveById("fungus_spores_common", 1);

            plot.strainId = strainId;
            plot.substrateId = substrateId;
            plot.growthStage = 0.0f;
            plot.sporeDensity = 0.1f;
            plot.contamination = substrate.contamination_risk;
            plot.isHarvestReady = false;
            plot.hasToxicBloom = false;
            plot.plantedDay = currentDay;

            OnSporesCultivated?.Invoke(plotId, strainId);
            return ActionResult.Success("fungi.spores_cultivated");
        }

        public ActionResult WaterPlot(string plotId, float amount = 0.3f)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");

            if (_inventory.CountById("clean_water") < 1)
                return ActionResult.Blocked("insufficient_water", "fungi.insufficient_water");

            _inventory.RemoveById("clean_water", 1);
            plot.moisture = Math.Min(1.0f, plot.moisture + amount);

            return ActionResult.Success("fungi.plot_watered");
        }

        public void TickDay(int currentDay, bool roomIsDark = true)
        {
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var plot = _state.plots[i];
                if (string.IsNullOrEmpty(plot.strainId) || plot.hasToxicBloom) continue;

                if (!_strains.TryGetValue(plot.strainId, out var strain)) continue;
                _substrates.TryGetValue(plot.substrateId ?? "", out var sub);

                // Check moisture conditions
                float moistureMod = 1.0f;
                if (plot.moisture < strain.moisture_min || plot.moisture > strain.moisture_max)
                {
                    moistureMod = 0.35f;
                }

                // Check darkness condition
                float darkMod = (!strain.darkness_required || roomIsDark) ? 1.0f : 0.2f;

                // Substrate nutrition
                float subMod = sub?.nutrition_multiplier ?? 1.0f;

                // Daily growth increment
                float baseRate = 1.0f / Math.Max(1, strain.growth_days);
                float growthDelta = baseRate * moistureMod * darkMod * subMod;

                plot.growthStage = Math.Min(1.0f, plot.growthStage + growthDelta);
                plot.moisture = Math.Max(0.0f, plot.moisture - 0.15f);
                plot.sporeDensity = Math.Min(1.0f, plot.sporeDensity + (strain.spore_hazard * 0.25f));

                if (plot.growthStage >= 1.0f)
                {
                    plot.isHarvestReady = true;
                }

                // Toxic bloom evaluation
                if (plot.moisture >= 0.85f && (strain.category == "Toxic" || _rng.NextDouble() < 0.08))
                {
                    plot.hasToxicBloom = true;
                    _state.totalBlooms++;
                    OnToxicBloom?.Invoke(plot.plotId, plot.roomId);
                }
            }
        }

        public ActionResult HarvestPlot(string plotId)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (!plot.isHarvestReady || string.IsNullOrEmpty(plot.strainId))
                return ActionResult.Blocked("not_ready", "fungi.not_ready");

            if (!_strains.TryGetValue(plot.strainId, out var strain))
                return ActionResult.Blocked("unknown_strain", "fungi.unknown_strain");

            // Award yield
            _inventory.AddById(strain.yield_item_id, strain.yield_count);
            _state.totalHarvests++;

            string harvestedStrain = plot.strainId;
            int count = strain.yield_count;

            // Reset plot to fallow
            plot.strainId = null;
            plot.substrateId = null;
            plot.growthStage = 0f;
            plot.isHarvestReady = false;
            plot.sporeDensity = 0f;

            OnFungiHarvested?.Invoke(plotId, harvestedStrain, count);
            return ActionResult.Success("fungi.harvested");
        }

        public ActionResult PurgeToxicBloom(string plotId)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (!plot.hasToxicBloom) return ActionResult.Blocked("no_bloom", "fungi.no_bloom");

            if (_inventory.CountById("clean_water") < 2)
                return ActionResult.Blocked("insufficient_clean_water", "fungi.insufficient_clean_water");

            _inventory.RemoveById("clean_water", 2);

            plot.hasToxicBloom = false;
            plot.growthStage = 0f;
            plot.strainId = null;
            plot.substrateId = null;
            plot.sporeDensity = 0f;

            OnBloomPurged?.Invoke(plotId);
            return ActionResult.Success("fungi.bloom_purged");
        }

        public float GetBioluminescentLightOutput(string roomId)
        {
            float totalLight = 0f;
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p.roomId == roomId && !string.IsNullOrEmpty(p.strainId) && !p.hasToxicBloom)
                {
                    if (_strains.TryGetValue(p.strainId, out var s))
                    {
                        totalLight += s.light_output * p.growthStage;
                    }
                }
            }
            return totalLight;
        }

        public float GetSporeHazardInRoom(string roomId)
        {
            float totalHazard = 0f;
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p.roomId == roomId)
                {
                    totalHazard += p.sporeDensity;
                    if (p.hasToxicBloom) totalHazard += 1.5f;
                }
            }
            return totalHazard;
        }

        public void RestoreState(FungiCultivationState state)
        {
            if (state == null) return;
            _state = state;
        }
    }
}
