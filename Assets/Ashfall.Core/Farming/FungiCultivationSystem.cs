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
        // Plan 204: thermal band (°C). Broad defaults keep legacy catalogs simulating as before.
        public float temperature_min { get; set; } = 4f;
        public float temperature_max { get; set; } = 32f;
        public bool darkness_required { get; set; } = true;
        public float toxicity { get; set; } = 0.0f;
        public float spore_hazard { get; set; } = 0.1f;
        public float light_output { get; set; } = 0.0f;
        public string yield_item_id { get; set; } = "harvested_mushrooms_subterranean";
        public int yield_count { get; set; } = 4;
        // Plan 204: number of harvests per inoculation. Legacy default 1 = old single-harvest behavior.
        public int flush_count { get; set; } = 1;
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
        // Plan 204: substrate preparation state ("untreated" | "prepared" | "clean" | "compromised").
        // Old saves restore as "untreated" — the historical baseline.
        public string substratePreparation { get; set; } = SubstratePreparation.Untreated;
        public float growthStage { get; set; } = 0.0f;
        public float moisture { get; set; } = 0.6f;
        public float sporeDensity { get; set; } = 0.0f;
        public float contamination { get; set; } = 0.0f;
        public bool isHarvestReady { get; set; } = false;
        public bool hasToxicBloom { get; set; } = false;
        // Plan 204: remaining harvests before the plot returns to fallow.
        // 0 on an old-save active plot means "legacy single-flush plot" (harvest behaves as before).
        public int remainingFlushes { get; set; } = 0;
        // Plan 204: sealed plot — no growth, no spore emission, blocks contamination spread.
        public bool isQuarantined { get; set; } = false;
        public int plantedDay { get; set; } = 0;
    }

    [Serializable]
    public sealed class FungiCultivationState
    {
        public int schema_version { get; set; } = 2;
        public List<FungiPlotState> plots { get; set; } = new List<FungiPlotState>();
        public int totalHarvests { get; set; } = 0;
        public int totalBlooms { get; set; } = 0;
        public int totalPreparations { get; set; } = 0;
        public int totalDisposals { get; set; } = 0;
    }

    /// <summary>
    /// Plan 204: abstract substrate preparation states and their gameplay modifiers.
    /// Deliberately non-procedural — no real-world sterilization parameters.
    /// </summary>
    public static class SubstratePreparation
    {
        public const string Untreated = "untreated";
        public const string Prepared = "prepared";
        public const string Clean = "clean";
        public const string Compromised = "compromised";

        /// <summary>Multiplier applied to the substrate's base contamination risk at inoculation.</summary>
        public static float RiskMultiplier(string preparation) => preparation switch
        {
            Prepared => 0.5f,
            Clean => 0.25f,
            Compromised => 1.5f,
            _ => 1.0f // Untreated / unknown legacy values
        };

        /// <summary>Multiplier applied to daily colonization growth.</summary>
        public static float GrowthMultiplier(string preparation) => preparation switch
        {
            Prepared => 1.1f,
            Clean => 1.2f,
            Compromised => 0.8f,
            _ => 1.0f
        };

        public static bool IsValid(string preparation) =>
            preparation == Untreated || preparation == Prepared ||
            preparation == Clean || preparation == Compromised;
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
        public event Action<string, float>? OnSporeExposure;
        // Plan 204 additions:
        public event Action<string, string, bool>? OnSubstratePrepared;   // plotId, preparation state, usedHeat
        public event Action<string, string>? OnSubstrateDisposed;         // plotId, disposal method
        public event Action<string, string>? OnContaminationSpread;       // source plotId, affected roomId

        public FungiCultivationState State => _state;
        public IReadOnlyDictionary<string, FungusStrainDef> Strains => _strains;
        public IReadOnlyDictionary<string, SubstrateDef> Substrates => _substrates;

        // Plan 204: per-flush yield curve — flush 1 full, later flushes taper. Deterministic.
        private const float FirstFlushYieldFraction = 1.0f;
        private const float SecondFlushYieldFraction = 0.6f;
        private const float LaterFlushYieldFraction = 0.4f;
        // Growth stage a plot returns to after a non-final flush (still colonized, must re-fruit).
        private const float ReflushGrowthStage = 0.5f;
        // Same-room spread pressure applied per day to neighbors of a blooming plot.
        private const float BloomSpreadPerDay = 0.05f;
        // Contamination at or above this threshold blooms the plot outright (deterministic threshold).
        private const float ContaminationBloomThreshold = 1.0f;
        // Daily contamination self-cleaning drift on healthy, ventilated plots.
        private const float ContaminationDailyDecay = 0.02f;
        // Disposal costs (canonical items).
        private const string BurnFuelItemId = "fuel";
        private const int QuarantineSealCost = 2;

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

        // ── Plan 204: substrate preparation ─────────────────────────────

        /// <summary>
        /// Prepares the substrate of a fallow plot as an abstract processing action.
        /// Costs 1 clean_water, plus 1 fuel when heat is requested. Heat raises the chance
        /// of a "clean" result; failure yields a "compromised" substrate. Deterministic
        /// under the seeded RNG. No real-world sterilization procedure is modeled.
        /// </summary>
        public ActionResult PrepareSubstrate(string plotId, bool useHeat)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (plot.isQuarantined) return ActionResult.Blocked("plot_quarantined", "fungi.plot_quarantined");
            if (!string.IsNullOrEmpty(plot.strainId))
                return ActionResult.Blocked("plot_occupied", "fungi.prep_plot_occupied");
            if (plot.hasToxicBloom)
                return ActionResult.Blocked("plot_bloomed", "fungi.prep_plot_bloomed");

            // Substrate selection: the last substrate stocked on this plot, else the first known.
            var substrate = ResolveSubstrateForPlot(plot);
            if (substrate == null)
                return ActionResult.Blocked("unknown_substrate", "fungi.unknown_substrate");

            if (_inventory.CountById("clean_water") < 1)
                return ActionResult.Blocked("insufficient_water", "fungi.prep_missing_water");

            if (useHeat && _inventory.CountById(BurnFuelItemId) < 1)
                return ActionResult.Blocked("insufficient_fuel", "fungi.prep_missing_fuel");

            // Atomic consumption.
            _inventory.RemoveById("clean_water", 1);
            if (useHeat)
                _inventory.RemoveById(BurnFuelItemId, 1);

            string preparation;
            double roll = _rng.NextDouble();
            if (roll < substrate.contamination_risk)
            {
                preparation = SubstratePreparation.Compromised;
            }
            else
            {
                // Heat gives an additional cleanliness band: the lower half of the clean roll is "clean".
                preparation = useHeat && roll < 0.5 + (0.5 - substrate.contamination_risk)
                    ? SubstratePreparation.Clean
                    : SubstratePreparation.Prepared;
            }

            plot.substratePreparation = preparation;
            plot.substrateId = substrate.substrate_id;
            _state.totalPreparations++;

            OnSubstratePrepared?.Invoke(plotId, preparation, useHeat);
            _log.Info($"[Fungi] Plot {plotId} substrate prepared: {preparation}{(useHeat ? " (heated)" : "")}.");
            return ActionResult.Success("fungi.substrate_prepared");
        }

        private SubstrateDef? ResolveSubstrateForPlot(FungiPlotState plot)
        {
            if (!string.IsNullOrEmpty(plot.substrateId) &&
                _substrates.TryGetValue(plot.substrateId, out var stocked))
                return stocked;
            foreach (var kvp in _substrates)
                return kvp.Value;
            return null;
        }

        public ActionResult CultivateSpores(string plotId, string strainId, string substrateId, int currentDay)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (plot.isQuarantined) return ActionResult.Blocked("plot_quarantined", "fungi.plot_quarantined");
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

            bool carriedPreparation = plot.substratePreparation != SubstratePreparation.Untreated &&
                                      plot.substratePreparation != SubstratePreparation.Compromised;

            plot.strainId = strainId;
            plot.substrateId = substrateId;
            plot.growthStage = 0.0f;
            plot.sporeDensity = 0.1f;
            // Plan 204: preparation quality directly shapes the starting contamination load.
            plot.contamination = Math.Clamp(
                substrate.contamination_risk * SubstratePreparation.RiskMultiplier(plot.substratePreparation),
                0f, 1f);
            if (plot.substratePreparation == SubstratePreparation.Compromised)
            {
                // A compromised prep is worse than doing nothing; reset it so the player must re-prep.
                plot.substratePreparation = SubstratePreparation.Untreated;
            }
            plot.isHarvestReady = false;
            plot.hasToxicBloom = false;
            plot.remainingFlushes = Math.Max(1, strain.flush_count);
            plot.plantedDay = currentDay;

            OnSporesCultivated?.Invoke(plotId, strainId);
            return ActionResult.Success("fungi.spores_cultivated");
        }

        public ActionResult WaterPlot(string plotId, float amount = 0.3f)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (plot.isQuarantined) return ActionResult.Blocked("plot_quarantined", "fungi.plot_quarantined");

            if (_inventory.CountById("clean_water") < 1)
                return ActionResult.Blocked("insufficient_water", "fungi.insufficient_water");

            _inventory.RemoveById("clean_water", 1);
            plot.moisture = Math.Min(1.0f, plot.moisture + amount);

            return ActionResult.Success("fungi.plot_watered");
        }

        /// <summary>
        /// Advances all plots one day. <paramref name="roomTemperatureC"/> is the default shelter
        /// interior temperature; <paramref name="roomTemperatureOverride"/> lets the host project
        /// real per-room thermal state (ThermalRoomNode.currentTempC) by room id.
        /// </summary>
        public void TickDay(int currentDay, bool roomIsDark = true, float roomTemperatureC = 15f,
            Func<string, float>? roomTemperatureOverride = null)
        {
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var plot = _state.plots[i];

                // Quarantined plots are sealed: no growth, no spore accumulation, hazard decays.
                if (plot.isQuarantined)
                {
                    plot.sporeDensity = Math.Max(0f, plot.sporeDensity - 0.1f);
                    continue;
                }

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

                // Plan 204: thermal band — growth stalls outside the strain's comfort range.
                float roomTemp = roomTemperatureOverride?.Invoke(plot.roomId) ?? roomTemperatureC;
                float tempMod = 1.0f;
                if (roomTemp < strain.temperature_min || roomTemp > strain.temperature_max)
                {
                    tempMod = 0.35f;
                }

                // Substrate nutrition
                float subMod = sub?.nutrition_multiplier ?? 1.0f;

                // Plan 204: preparation quality feeds daily growth.
                float prepMod = SubstratePreparation.GrowthMultiplier(plot.substratePreparation);

                // Daily growth increment
                float baseRate = 1.0f / Math.Max(1, strain.growth_days);
                float growthDelta = baseRate * moistureMod * darkMod * tempMod * subMod * prepMod;

                plot.growthStage = Math.Min(1.0f, plot.growthStage + growthDelta);
                plot.moisture = Math.Max(0.0f, plot.moisture - 0.15f);
                plot.sporeDensity = Math.Min(1.0f, plot.sporeDensity + (strain.spore_hazard * 0.25f));

                if (plot.growthStage >= 1.0f)
                {
                    plot.isHarvestReady = true;
                }

                // Plan 204: contamination blooms a plot outright once it saturates
                // (deterministic threshold — no RNG). Evaluated before the daily
                // decay so spread pressure that reaches the threshold actually fires.
                if (plot.contamination >= ContaminationBloomThreshold)
                {
                    DeclareBloom(plot);
                    continue;
                }
                if (plot.contamination > 0f)
                {
                    plot.contamination = Math.Max(0f, plot.contamination - ContaminationDailyDecay);
                }

                // Toxic bloom evaluation (seeded risk for saturated/wet toxic-prone beds).
                if (plot.moisture >= 0.85f && (strain.category == "Toxic" || _rng.NextDouble() < 0.08))
                {
                    DeclareBloom(plot);
                }
            }

            // ── Plan 204: contamination spread — same room only, never across rooms ──
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var bloom = _state.plots[i];
                if (!bloom.hasToxicBloom || bloom.isQuarantined) continue;

                for (int j = 0; j < _state.plots.Count; j++)
                {
                    var neighbor = _state.plots[j];
                    if (j == i || neighbor.isQuarantined) continue;
                    if (neighbor.hasToxicBloom) continue;
                    if (!string.Equals(neighbor.roomId, bloom.roomId, StringComparison.Ordinal)) continue;
                    if (string.IsNullOrEmpty(neighbor.strainId)) continue;

                    neighbor.contamination = Math.Min(ContaminationBloomThreshold, neighbor.contamination + BloomSpreadPerDay);
                    OnContaminationSpread?.Invoke(bloom.plotId, neighbor.roomId);
                }
            }

            // ── Spore lung exposure: aggregate per-room hazard and emit ──
            // Quarantined plots no longer contribute (sealed beds emit nothing).
            var roomHazards = new Dictionary<string, float>(StringComparer.Ordinal);
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var plot = _state.plots[i];
                if (string.IsNullOrEmpty(plot.roomId) || plot.isQuarantined) continue;
                float hazard = GetSporeHazardInRoom(plot.roomId);
                if (hazard > 0f)
                    roomHazards[plot.roomId] = hazard;
            }
            foreach (var kvp in roomHazards)
            {
                OnSporeExposure?.Invoke(kvp.Key, kvp.Value);
            }
        }

        private void DeclareBloom(FungiPlotState plot)
        {
            if (plot.hasToxicBloom) return;
            plot.hasToxicBloom = true;
            _state.totalBlooms++;
            OnToxicBloom?.Invoke(plot.plotId, plot.roomId);
            _log.Warn($"[Fungi] Toxic bloom declared at plot {plot.plotId} ({plot.roomId}).");
        }

        // ── Plan 204: disposal of contaminated substrate ────────────────

        /// <summary>
        /// Disposes of a bloom-contaminated plot through a real, costed transaction.
        /// "discard" clears the bed for free (contents wasted). "burn" consumes 1 fuel and
        /// clears the bed. "quarantine" consumes sealant materials and seals the plot —
        /// no growth, no spore emission, no spread — recoverable later via PurgeToxicBloom.
        /// </summary>
        public ActionResult DisposeInfectedSubstrate(string plotId, string method)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (!plot.hasToxicBloom && plot.contamination < 0.75f)
                return ActionResult.Blocked("not_contaminated", "fungi.dispose_not_contaminated");

            switch (method)
            {
                case "discard":
                    ClearPlot(plot);
                    break;

                case "burn":
                    if (_inventory.CountById(BurnFuelItemId) < 1)
                        return ActionResult.Blocked("insufficient_fuel", "fungi.dispose_missing_fuel");
                    _inventory.RemoveById(BurnFuelItemId, 1);
                    ClearPlot(plot);
                    break;

                case "quarantine":
                    if (_inventory.CountById("scrap_wood") < QuarantineSealCost)
                        return ActionResult.Blocked("missing_sealant", "fungi.dispose_missing_sealant");
                    _inventory.RemoveById("scrap_wood", QuarantineSealCost);
                    plot.isQuarantined = true;
                    plot.hasToxicBloom = false; // contained; hazard held under seal, recoverable via purge
                    break;

                default:
                    return ActionResult.Failed("unknown_method", "fungi.dispose_unknown_method");
            }

            _state.totalDisposals++;
            OnSubstrateDisposed?.Invoke(plotId, method);
            _log.Info($"[Fungi] Plot {plotId} contaminated substrate disposed via '{method}'.");
            return ActionResult.Success("fungi.substrate_disposed");
        }

        private void ClearPlot(FungiPlotState plot)
        {
            plot.strainId = null;
            plot.substrateId = null;
            plot.substratePreparation = SubstratePreparation.Untreated;
            plot.growthStage = 0f;
            plot.sporeDensity = 0f;
            plot.contamination = 0f;
            plot.isHarvestReady = false;
            plot.hasToxicBloom = false;
            plot.isQuarantined = false;
            plot.remainingFlushes = 0;
        }

        public ActionResult HarvestPlot(string plotId)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (plot.isQuarantined) return ActionResult.Blocked("plot_quarantined", "fungi.plot_quarantined");
            if (!plot.isHarvestReady || string.IsNullOrEmpty(plot.strainId))
                return ActionResult.Blocked("not_ready", "fungi.not_ready");

            if (!_strains.TryGetValue(plot.strainId, out var strain))
                return ActionResult.Blocked("unknown_strain", "fungi.unknown_strain");

            // Plan 204: flush cycle — later flushes taper deterministically.
            // Legacy plots (remainingFlushes == 0) behave as a single full flush.
            int remaining = plot.remainingFlushes > 0 ? plot.remainingFlushes : 1;
            float fraction;
            if (remaining == strain.flush_count)
                fraction = FirstFlushYieldFraction;
            else if (remaining == strain.flush_count - 1)
                fraction = SecondFlushYieldFraction;
            else
                fraction = LaterFlushYieldFraction;
            int yieldCount = Math.Max(1, (int)MathF.Round(strain.yield_count * fraction));

            _inventory.AddById(strain.yield_item_id, yieldCount);
            _state.totalHarvests++;

            string harvestedStrain = plot.strainId;
            int count = yieldCount;
            remaining--;

            if (remaining > 0)
            {
                // Still colonized: re-fruit from the reflush stage with the remaining flushes.
                plot.remainingFlushes = remaining;
                plot.growthStage = ReflushGrowthStage;
                plot.isHarvestReady = false;
                plot.sporeDensity = Math.Min(1.0f, plot.sporeDensity);
            }
            else
            {
                // Final flush: reset plot to fallow (historical behavior).
                plot.strainId = null;
                plot.substrateId = null;
                plot.substratePreparation = SubstratePreparation.Untreated;
                plot.growthStage = 0f;
                plot.isHarvestReady = false;
                plot.sporeDensity = 0f;
                plot.remainingFlushes = 0;
            }

            OnFungiHarvested?.Invoke(plotId, harvestedStrain, count);
            return ActionResult.Success("fungi.harvested");
        }

        public ActionResult PurgeToxicBloom(string plotId)
        {
            var plot = _state.plots.Find(p => p.plotId == plotId);
            if (plot == null) return ActionResult.Blocked("plot_not_found", "fungi.plot_not_found");
            if (!plot.hasToxicBloom && !plot.isQuarantined) return ActionResult.Blocked("no_bloom", "fungi.no_bloom");

            if (_inventory.CountById("clean_water") < 2)
                return ActionResult.Blocked("insufficient_clean_water", "fungi.insufficient_clean_water");

            _inventory.RemoveById("clean_water", 2);

            plot.hasToxicBloom = false;
            plot.isQuarantined = false;
            plot.growthStage = 0f;
            plot.strainId = null;
            plot.substrateId = null;
            plot.substratePreparation = SubstratePreparation.Untreated;
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
                if (p.roomId == roomId && !string.IsNullOrEmpty(p.strainId) && !p.hasToxicBloom && !p.isQuarantined)
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
                    if (p.isQuarantined) continue; // sealed beds emit nothing
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
