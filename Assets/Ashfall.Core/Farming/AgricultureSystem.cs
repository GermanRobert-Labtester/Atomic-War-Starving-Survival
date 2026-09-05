// SPDX-License-Identifier: MIT
// ============================================================================
// System     : AgricultureSystem (Plan 162 — Hydroponics & Advanced Agriculture)
// Layer      : Advanced agriculture ABOVE the canonical GreenhouseSystem.
// Ownership  : strain state per plot, growing-medium quality/toxicity, pest
//              infestations (distinct from greenhouse blight), authored
//              mutation outcomes, compost batches, first-harvest narrative.
// Delegates  : plot growth/stages/water to GreenhouseSystem (single growth
//              authority — agriculture derives its per-day grow-light and
//              ash inputs from power/weather and ticks the greenhouse once).
// RNG        : injected ISeededRng per tick (host forks
//              agriculture.pest / agriculture.mutation streams per day).
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Farming
{
    /// <summary>Water quality band derived by the host from WaterTreatmentSystem.</summary>
    public enum AgriWaterBand
    {
        Clean = 0,
        Marginal = 1,
        Contaminated = 2,
        Unsafe = 3
    }

    public enum AgriMutationOutcome
    {
        None = 0,
        YieldBoost = 1,
        YieldPenalty = 2,
        ToxicHarvest = 3,
        HardyStrain = 4,
        SterileSeed = 5
    }

    /// <summary>External + shelter environment inputs for one agriculture day.</summary>
    public struct AgricultureEnvironmentSnapshot
    {
        public float TemperaturePenaltyC;
        public float OutdoorRadModifier;
        public float LightingAvailabilityPermille;
        public float AshContaminationRate;
        public string SeasonWindowId;

        public static AgricultureEnvironmentSnapshot Default() => new AgricultureEnvironmentSnapshot
        {
            TemperaturePenaltyC = 0f,
            OutdoorRadModifier = 100f,
            LightingAvailabilityPermille = 1000f,
            AshContaminationRate = 0.04f,
            SeasonWindowId = "any"
        };
    }

    [Serializable]
    public class AgriPlotState
    {
        public int plot_index;
        public string strain_id = "";
        /// <summary>Substrate/medium condition 0..100; persists between crops.</summary>
        public float medium_quality = 100f;
        /// <summary>Normalized plot toxicity 0..1000 permille.</summary>
        public int toxicity_permille;
        /// <summary>Active pest severity 0..1 (0 = clear).</summary>
        public float pest_severity;
        public string pest_id = "";
        /// <summary>Mutation outcome rolled once at maturity (lifecycle check).</summary>
        public int mutation_outcome;
        /// <summary>Resolved variant strain when outcome is HardyStrain.</summary>
        public string mutation_strain_id = "";
        public int planted_day;
    }

    [Serializable]
    public class CompostBatchState
    {
        public string recipe_id = "";
        public int started_day;
        public bool collected;
    }

    [Serializable]
    public class AgricultureState
    {
        public string system_id = "agriculture";
        public int schema_version = 1;
        public List<AgriPlotState> plots = new List<AgriPlotState>();
        public List<CompostBatchState> compost_batches = new List<CompostBatchState>();
        public bool first_harvest_narrative_fired;
        /// <summary>Plots with an open blight narrative episode (one-shot per outbreak).</summary>
        public List<int> blight_narrative_plots = new List<int>();
        /// <summary>Strain ids unlocked through HardyStrain mutations / discovery.</summary>
        public List<string> unlocked_strains = new List<string>();
        public int last_tick_day;
    }

    public struct YieldBreakdown
    {
        public int BaseYield;
        public float StrainModifier;
        public float WaterModifier;
        public float LightModifier;
        public float ToxicityModifier;
        public float PestModifier;
        public float MutationModifier;
        public float MediumModifier;
        public float FinalMultiplier;
        public int FinalYield;
        public string QualityTier;
        public bool Contaminated;
    }

    public struct AgricultureHarvest
    {
        public bool success;
        public int plotIndex;
        public string yieldItemId;
        public int baseAmount;
        public int finalAmount;
        public bool contaminated;
        public string qualityTier;
        public YieldBreakdown Breakdown;
        public NutritionProfileDef Nutrition;
    }

    /// <summary>
    /// Plan 162 flagship agriculture engine. One instance per campaign; wraps a
    /// GreenhouseSystem it does not own (the host constructs and saves both).
    /// Plots without a strain keep plain greenhouse behaviour, so pre-feature
    /// saves load and play unchanged.
    /// </summary>
    public sealed class AgricultureSystem
    {
        public const float MediumDecayPerPlantedDay = 0.5f;
        public const float MediumDecayPerFallowDay = 0.1f;
        public const float ToxicityNaturalDecayPerDay = 2f;
        public const float CompostMediumBoost = 40f;
        public const int CompostToxicityReduction = 200;
        public const float BaseGrowLightHours = 6f;
        /// <summary>Greenhouse water input is fixed at 50-unit watering pulses.</summary>
        public const float WaterUnitsPerWatering = 50f;
        public static readonly int[] ToxicityPerUnitByBand = { 0, 2, 8, 15 };

        private readonly AgricultureState _state;
        private readonly GreenhouseSystem _greenhouse;
        private CropStrainCatalogContainer _catalog = new CropStrainCatalogContainer();

        public AgricultureSystem(GreenhouseSystem greenhouse)
        {
            _greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
            _state = new AgricultureState();
        }

        public string SystemId => _state.system_id;
        public AgricultureState State => _state;
        public GreenhouseSystem Greenhouse => _greenhouse;
        public CropStrainCatalogContainer Catalog => _catalog;
        public int LastTickDay => _state.last_tick_day;

        public void LoadCatalog(CropStrainCatalogContainer catalog)
        {
            _catalog = catalog ?? new CropStrainCatalogContainer();
        }

        // ------------------------------------------------------------------
        // Events (typed; host maps to journal/radio/UI)
        // ------------------------------------------------------------------
        public event Action<int, string, int> OnStrainPlanted;
        public event Action<int, string> OnPlotInfested;
        public event Action<int, string> OnPestTreated;
        public event Action<int, AgriMutationOutcome, string> OnMutationDetermined;
        public event Action<AgricultureHarvest> OnHarvest;
        public event Action<string, int> OnCompostStarted;
        public event Action<string, int> OnCompostCollected;
        public event Action<int> OnFirstHarvest;
        public event Action<int> OnBlightNarrative;

        // ------------------------------------------------------------------
        // Save
        // ------------------------------------------------------------------
        public AgricultureState CaptureState()
        {
            var copy = new AgricultureState
            {
                first_harvest_narrative_fired = _state.first_harvest_narrative_fired,
                last_tick_day = _state.last_tick_day,
                blight_narrative_plots = new List<int>(_state.blight_narrative_plots),
                unlocked_strains = new List<string>(_state.unlocked_strains),
                compost_batches = new List<CompostBatchState>(_state.compost_batches.Count)
            };
            foreach (var b in _state.compost_batches)
                copy.compost_batches.Add(new CompostBatchState
                {
                    recipe_id = b.recipe_id,
                    started_day = b.started_day,
                    collected = b.collected
                });
            foreach (var p in _state.plots)
                copy.plots.Add(new AgriPlotState
                {
                    plot_index = p.plot_index,
                    strain_id = p.strain_id,
                    medium_quality = p.medium_quality,
                    toxicity_permille = p.toxicity_permille,
                    pest_severity = p.pest_severity,
                    pest_id = p.pest_id,
                    mutation_outcome = p.mutation_outcome,
                    mutation_strain_id = p.mutation_strain_id,
                    planted_day = p.planted_day
                });
            return copy;
        }

        public void RestoreState(AgricultureState state)
        {
            if (state == null) return;
            _state.first_harvest_narrative_fired = state.first_harvest_narrative_fired;
            _state.last_tick_day = state.last_tick_day;
            _state.blight_narrative_plots = state.blight_narrative_plots != null
                ? new List<int>(state.blight_narrative_plots)
                : new List<int>();
            _state.unlocked_strains = state.unlocked_strains != null
                ? new List<string>(state.unlocked_strains)
                : new List<string>();
            _state.compost_batches = new List<CompostBatchState>(state.compost_batches?.Count ?? 0);
            if (state.compost_batches != null)
                foreach (var b in state.compost_batches)
                    _state.compost_batches.Add(new CompostBatchState
                    {
                        recipe_id = b.recipe_id,
                        started_day = b.started_day,
                        collected = b.collected
                    });
            _state.plots = new List<AgriPlotState>(state.plots?.Count ?? 0);
            if (state.plots != null)
                foreach (var p in state.plots)
                    _state.plots.Add(new AgriPlotState
                    {
                        plot_index = p.plot_index,
                        strain_id = p.strain_id,
                        medium_quality = p.medium_quality,
                        toxicity_permille = p.toxicity_permille,
                        pest_severity = p.pest_severity,
                        pest_id = p.pest_id,
                        mutation_outcome = p.mutation_outcome,
                        mutation_strain_id = p.mutation_strain_id,
                        planted_day = p.planted_day
                    });
        }

        // ------------------------------------------------------------------
        // Catalog lookups
        // ------------------------------------------------------------------
        public CropStrainDef Strain(string strainId)
        {
            if (string.IsNullOrEmpty(strainId)) return null;
            return _catalog.strains.Find(s => s != null && string.Equals(s.id, strainId, StringComparison.Ordinal));
        }

        /// <summary>The effective strain for a plot: the planted strain, or its
        /// resolved hardy variant once a HardyStrain mutation has rolled.</summary>
        public CropStrainDef EffectiveStrain(int plotIndex)
        {
            var p = PlotAt(plotIndex);
            if (p == null || string.IsNullOrEmpty(p.strain_id)) return null;
            var id = !string.IsNullOrEmpty(p.mutation_strain_id) ? p.mutation_strain_id : p.strain_id;
            return Strain(id);
        }

        public PestDef Pest(string pestId)
        {
            if (string.IsNullOrEmpty(pestId)) return null;
            return _catalog.pests.Find(p => p != null && string.Equals(p.id, pestId, StringComparison.Ordinal));
        }

        public CompostRecipeDef CompostRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId)) return null;
            return _catalog.compost_recipes.Find(r => r != null && string.Equals(r.id, recipeId, StringComparison.Ordinal));
        }

        // ------------------------------------------------------------------
        // Plot management
        // ------------------------------------------------------------------
        private AgriPlotState PlotAt(int plotIndex)
        {
            return _state.plots.Find(p => p != null && p.plot_index == plotIndex);
        }

        private AgriPlotState EnsurePlot(int plotIndex)
        {
            var p = PlotAt(plotIndex);
            if (p == null)
            {
                p = new AgriPlotState { plot_index = plotIndex };
                _state.plots.Add(p);
                _state.plots.Sort((a, b) => a.plot_index.CompareTo(b.plot_index));
            }
            return p;
        }

        /// <summary>Validation-only check; the host consumes the seed item first.</summary>
        public bool CanPlantStrain(int plotIndex, string strainId)
        {
            var strain = Strain(strainId);
            if (strain == null) return false;
            if (plotIndex < 0 || plotIndex >= _greenhouse.PlotCount) return false;
            var ghPlot = _greenhouse.Plots[plotIndex];
            return GreenhouseSystem.IsFallow(ghPlot);
        }

        /// <summary>
        /// Plant through the canonical greenhouse authority and attach the
        /// advanced strain layer. The host consumes the seed item beforehand.
        /// </summary>
        public bool PlantWithStrain(int plotIndex, string strainId, int currentDay)
        {
            var strain = Strain(strainId);
            if (strain == null) return false;
            string consumed;
            if (!_greenhouse.Plant(plotIndex, strain.seed_item_id, currentDay, out consumed))
                return false;

            var p = EnsurePlot(plotIndex);
            p.strain_id = strainId;
            p.pest_severity = 0f;
            p.pest_id = "";
            p.mutation_outcome = (int)AgriMutationOutcome.None;
            p.mutation_strain_id = "";
            p.planted_day = currentDay;
            OnStrainPlanted?.Invoke(plotIndex, strainId, currentDay);
            return true;
        }

        public bool ClearPlot(int plotIndex)
        {
            var p = PlotAt(plotIndex);
            if (p == null) return _greenhouse.Clear(plotIndex);
            ResetCropFields(p);
            CloseBlightNarrative(plotIndex);
            return _greenhouse.Clear(plotIndex);
        }

        /// <summary>
        /// Water through the greenhouse authority; agriculture accumulates plot
        /// toxicity from the quality band. Unsafe water is routed as tainted so
        /// the greenhouse contamination model also applies.
        /// </summary>
        public void Water(int plotIndex, AgriWaterBand band)
        {
            var p = EnsurePlot(plotIndex);
            bool tainted = band == AgriWaterBand.Unsafe;
            _greenhouse.Water(plotIndex, WaterUnitsPerWatering, tainted);
            int idx = (int)band;
            if (idx < 0) idx = 0;
            if (idx >= ToxicityPerUnitByBand.Length) idx = ToxicityPerUnitByBand.Length - 1;
            p.toxicity_permille = Math.Min(1000,
                p.toxicity_permille + (int)(WaterUnitsPerWatering * ToxicityPerUnitByBand[idx]));
        }

        /// <summary>Treatment validation; the host consumes the item first.</summary>
        public bool CanTreatPest(int plotIndex, string treatmentItemId)
        {
            var p = PlotAt(plotIndex);
            if (p == null || p.pest_severity <= 0f || string.IsNullOrEmpty(p.pest_id)) return false;
            var pest = Pest(p.pest_id);
            if (pest == null) return false;
            return pest.treats_with_item_ids != null
                   && pest.treats_with_item_ids.Contains(treatmentItemId);
        }

        public bool TryTreatPestInfestation(int plotIndex, string treatmentItemId)
        {
            if (!CanTreatPest(plotIndex, treatmentItemId)) return false;
            var p = PlotAt(plotIndex);
            p.pest_severity = 0f;
            p.pest_id = "";
            OnPestTreated?.Invoke(plotIndex, treatmentItemId);
            return true;
        }

        /// <summary>Validation for the host: compost consumes inputs atomically first.</summary>
        public bool CanStartCompost(string recipeId)
        {
            var r = CompostRecipe(recipeId);
            return r != null;
        }

        public bool TryStartCompostBatch(string recipeId, int currentDay)
        {
            var r = CompostRecipe(recipeId);
            if (r == null) return false;
            _state.compost_batches.Add(new CompostBatchState
            {
                recipe_id = recipeId,
                started_day = currentDay,
                collected = false
            });
            OnCompostStarted?.Invoke(recipeId, currentDay);
            return true;
        }

        public bool IsCompostReady(string recipeId, int currentDay)
        {
            var r = CompostRecipe(recipeId);
            if (r == null) return false;
            for (int i = 0; i < _state.compost_batches.Count; i++)
            {
                var b = _state.compost_batches[i];
                if (b.collected || !string.Equals(b.recipe_id, recipeId, StringComparison.Ordinal)) continue;
                if (currentDay - b.started_day >= r.duration_days) return true;
            }
            return false;
        }

        /// <summary>Collect a finished batch; returns output count (host produces the items).</summary>
        public int TryCollectCompost(string recipeId, int currentDay)
        {
            var r = CompostRecipe(recipeId);
            if (r == null) return 0;
            for (int i = 0; i < _state.compost_batches.Count; i++)
            {
                var b = _state.compost_batches[i];
                if (b.collected || !string.Equals(b.recipe_id, recipeId, StringComparison.Ordinal)) continue;
                if (currentDay - b.started_day < r.duration_days) continue;
                b.collected = true;
                _state.compost_batches.RemoveAt(i);
                OnCompostCollected?.Invoke(recipeId, r.output_count);
                return r.output_count;
            }
            return 0;
        }

        /// <summary>Apply compost to a plot's medium (host consumes the item first).</summary>
        public bool TryApplyCompost(int plotIndex)
        {
            var p = PlotAt(plotIndex);
            if (p == null) return false;
            p.medium_quality = Math.Min(100f, p.medium_quality + CompostMediumBoost);
            p.toxicity_permille = Math.Max(0, p.toxicity_permille - CompostToxicityReduction);
            return true;
        }

        // ------------------------------------------------------------------
        // Yield (pure calculator + instance wrappers)
        // ------------------------------------------------------------------
        public struct YieldInputs
        {
            public int BaseYield;
            public float StrainModifier;
            public float LightFactor;          // 0..1 grow-light availability
            public float WaterBandFactor;      // 1 clean .. 0.55 unsafe
            public int ToxicityPermille;
            public int ToxicityTolerancePermille;
            public float PestSeverity;
            public float PestYieldDamageAtFull;
            public int MutationOutcome;
            public float MediumQuality;
            public bool SoilContaminated;
        }

        /// <summary>Pure, bounded yield math. Every modifier is clamped so no
        /// NaN/negative/runaway multiplication can escape (plan §5.13).</summary>
        public static YieldBreakdown CalculateYield(in YieldInputs inputs)
        {
            float strain = Clamp01(inputs.StrainModifier <= 0f ? 1f : inputs.StrainModifier);
            float light = 0.25f + 0.75f * Clamp01(inputs.LightFactor);
            float water = ClampRange(inputs.WaterBandFactor <= 0f ? 1f : inputs.WaterBandFactor, 0.5f, 1f);
            float toxTolerance = Math.Max(1, inputs.ToxicityTolerancePermille);
            float toxicity = 1f - Math.Min(0.5f, Clamp01(inputs.ToxicityPermille / toxTolerance) * 0.5f);
            float pest = 1f - Clamp01(inputs.PestSeverity) * Clamp01(inputs.PestYieldDamageAtFull);
            float mutation = MutationMultiplier((AgriMutationOutcome)inputs.MutationOutcome);
            float medium = 0.75f + 0.25f * Clamp01(inputs.MediumQuality / 100f);

            float total = strain * light * water * toxicity * pest * mutation * medium;
            int final = (int)Math.Round(Math.Max(0, inputs.BaseYield) * total);
            int cap = Math.Max(0, inputs.BaseYield) * 2;
            if (final > cap) final = cap;

            bool toxic = inputs.SoilContaminated
                         || (AgriMutationOutcome)inputs.MutationOutcome == AgriMutationOutcome.ToxicHarvest;

            return new YieldBreakdown
            {
                BaseYield = inputs.BaseYield,
                StrainModifier = strain,
                LightModifier = light,
                WaterModifier = water,
                ToxicityModifier = toxicity,
                PestModifier = pest,
                MutationModifier = mutation,
                MediumModifier = medium,
                FinalMultiplier = total,
                FinalYield = final,
                QualityTier = QualityTierFor(total, toxic),
                Contaminated = toxic
            };
        }

        public YieldBreakdown ForecastYield(int plotIndex, in AgricultureEnvironmentSnapshot env)
        {
            var ghPlot = GreenhousePlot(plotIndex);
            var p = PlotAt(plotIndex);
            var strain = EffectiveStrain(plotIndex);
            var cropDef = ghPlot == null
                ? null
                : GreenhouseExpansionCatalog.CropCatalog.Get(ghPlot.seedItemId);
            int baseYield = cropDef?.BaseYield ?? 0;
            var pest = Pest(p?.pest_id);
            return CalculateYield(new YieldInputs
            {
                BaseYield = baseYield,
                StrainModifier = strain?.yield_modifier ?? 1f,
                LightFactor = Clamp01(env.LightingAvailabilityPermille / 1000f),
                WaterBandFactor = 1f,
                ToxicityPermille = p?.toxicity_permille ?? 0,
                ToxicityTolerancePermille = strain?.toxicity_tolerance_permille ?? 300,
                PestSeverity = p?.pest_severity ?? 0f,
                PestYieldDamageAtFull = pest?.yield_damage_at_full ?? 0.6f,
                MutationOutcome = p?.mutation_outcome ?? 0,
                MediumQuality = p?.medium_quality ?? 100f,
                SoilContaminated = ghPlot != null && cropDef != null
                                   && ghPlot.soilContamination >= cropDef.ContaminationTolerance
            });
        }

        /// <summary>
        /// Harvest through the greenhouse authority, then apply the advanced
        /// agriculture modifiers. A plot without a strain harvests with plain
        /// greenhouse numbers (multiplier 1 on every agriculture factor).
        /// All plot state is read BEFORE the greenhouse call because
        /// GreenhouseSystem.Harvest resets the plot.
        /// </summary>
        public AgricultureHarvest Harvest(int plotIndex)
        {
            var result = new AgricultureHarvest { plotIndex = plotIndex, success = false };
            var ghPlot = GreenhousePlot(plotIndex);
            if (ghPlot == null) return result;
            if (ghPlot.stage != (int)GreenhouseStage.Mature) return result;

            var cropDef = GreenhouseExpansionCatalog.CropCatalog.Get(ghPlot.seedItemId);
            if (cropDef == null) return result;

            var strain = EffectiveStrain(plotIndex);
            var p = PlotAt(plotIndex);
            var pest = Pest(p?.pest_id);
            bool soilContaminated = ghPlot.soilContamination >= cropDef.ContaminationTolerance;

            var breakdown = CalculateYield(new YieldInputs
            {
                BaseYield = cropDef.BaseYield,
                StrainModifier = strain?.yield_modifier ?? 1f,
                LightFactor = 1f,
                WaterBandFactor = 1f,
                ToxicityPermille = p?.toxicity_permille ?? 0,
                ToxicityTolerancePermille = strain?.toxicity_tolerance_permille ?? 300,
                PestSeverity = p?.pest_severity ?? 0f,
                PestYieldDamageAtFull = pest?.yield_damage_at_full ?? 0.6f,
                MutationOutcome = p?.mutation_outcome ?? 0,
                MediumQuality = p?.medium_quality ?? 100f,
                SoilContaminated = soilContaminated
            });

            var gh = _greenhouse.Harvest(plotIndex);
            if (!gh.success) return result;

            result.success = true;
            result.baseAmount = cropDef.BaseYield;
            result.finalAmount = breakdown.FinalYield;
            result.contaminated = breakdown.Contaminated;
            result.qualityTier = breakdown.QualityTier;
            result.Breakdown = breakdown;
            result.Nutrition = strain?.nutrition_profile ?? new NutritionProfileDef();
            result.yieldItemId = breakdown.Contaminated ? cropDef.YieldTaintedId : cropDef.YieldCleanId;

            if (p != null)
            {
                // Hardy variant carries over as an unlocked strain (host offers
                // its seed through the UI); the plot's crop fields reset below.
                if ((AgriMutationOutcome)p.mutation_outcome == AgriMutationOutcome.HardyStrain
                    && !string.IsNullOrEmpty(p.mutation_strain_id)
                    && Strain(p.mutation_strain_id) != null)
                {
                    UnlockStrain(p.mutation_strain_id);
                }
                ResetCropFields(p);
            }
            CloseBlightNarrative(plotIndex);

            if (!_state.first_harvest_narrative_fired)
            {
                _state.first_harvest_narrative_fired = true;
                OnFirstHarvest?.Invoke(plotIndex);
            }
            OnHarvest?.Invoke(result);
            return result;
        }

        public bool IsStrainUnlocked(string strainId) => _state.unlocked_strains.Contains(strainId);

        public void UnlockStrain(string strainId)
        {
            if (string.IsNullOrEmpty(strainId) || _state.unlocked_strains.Contains(strainId)) return;
            _state.unlocked_strains.Add(strainId);
        }

        // ------------------------------------------------------------------
        // Daily tick
        // ------------------------------------------------------------------
        /// <summary>
        /// One agriculture day. Order: medium/toxicity drift → pest progression
        /// (pest stream) → derive greenhouse inputs → greenhouse growth tick
        /// (exactly once) → maturity mutation checks (mutation stream) →
        /// ready events. Idempotent per day (CLOCK_POLICY double-advance guard).
        /// Draw budget: pests — one draw per susceptible planted plot per day;
        /// mutation — one draw per newly-mature plot whose pressure meets the
        /// strain threshold; plots below threshold consume no mutation RNG.
        /// </summary>
        public void TickDay(
            int day,
            in AgricultureEnvironmentSnapshot env,
            ISeededRng pestRng,
            ISeededRng mutationRng,
            string seasonWindowId = "any")
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;

            // 1. Medium quality / toxicity drift (stable plot order).
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p == null) continue;
                var gh = GreenhousePlot(p.plot_index);
                bool planted = gh != null && !GreenhouseSystem.IsFallow(gh);
                p.medium_quality = Math.Max(0f, p.medium_quality - (planted ? MediumDecayPerPlantedDay : MediumDecayPerFallowDay));
                p.toxicity_permille = Math.Max(0, p.toxicity_permille - (int)ToxicityNaturalDecayPerDay);
            }

            // 2. Pest progression: existing infestations worsen deterministically
            //    (no RNG); new infestations roll only for susceptible planted plots.
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p == null) continue;
                var gh = GreenhousePlot(p.plot_index);
                if (gh == null || GreenhouseSystem.IsFallow(gh)) continue;
                var strain = EffectiveStrain(p.plot_index);
                float susceptibility = strain?.pest_susceptibility ?? 0f;

                if (p.pest_severity > 0f)
                {
                    var pestDef = Pest(p.pest_id);
                    float step = pestDef?.severity_step ?? 0.1f;
                    p.pest_severity = Math.Min(1f, p.pest_severity + step);
                    continue;
                }

                if (susceptibility <= 0f || _catalog.pests.Count == 0 || pestRng == null) continue;

                // Candidate pests: season + strain-tag match.
                var candidates = _catalog.pests.FindAll(x =>
                    x != null
                    && (string.Equals(x.season_tag, "any", StringComparison.Ordinal)
                        || string.Equals(x.season_tag, seasonWindowId, StringComparison.Ordinal))
                    && (x.target_strain_tags == null || x.target_strain_tags.Count == 0
                        || TagMatch(x.target_strain_tags, strain)));
                if (candidates.Count == 0) continue;

                // Stable ordering before any draw.
                candidates.Sort((a, b) => string.CompareOrdinal(a.id, b.id));

                float sanitation = 1f + (100f - Clamp01(p.medium_quality / 100f) * 100f) / 200f;
                float chance = 0f;
                string chosen = null;
                foreach (var cand in candidates)
                {
                    float c = cand.base_chance_per_day * susceptibility * sanitation;
                    if (pestRng.NextDouble() < c)
                    {
                        chosen = cand.id;
                        chance = c;
                        break;
                    }
                }
                if (chosen != null)
                {
                    p.pest_id = chosen;
                    p.pest_severity = Math.Min(1f, (Pest(chosen)?.severity_step ?? 0.1f));
                    OnPlotInfested?.Invoke(p.plot_index, chosen);
                }
            }

            // 3. Derive greenhouse inputs from power/weather and tick the
            //    canonical growth authority exactly once.
            float lightingFactor = Clamp01(env.LightingAvailabilityPermille / 1000f);
            float growLightHours = BaseGrowLightHours * lightingFactor;
            float ashRate = Math.Max(0f, env.AshContaminationRate);
            var stagesBefore = CaptureStages();
            _greenhouse.TickDay(day, growLightHours, ashRate);

            // 4. Mutation lifecycle check: only plots that BECAME mature this
            //    tick, only when radiation pressure meets the strain threshold.
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p == null || string.IsNullOrEmpty(p.strain_id)) continue;
                var gh = GreenhousePlot(p.plot_index);
                if (gh == null || gh.stage != (int)GreenhouseStage.Mature) continue;
                if (stagesBefore.TryGetValue(p.plot_index, out int before) && before == (int)GreenhouseStage.Mature)
                    continue; // was already mature — checked in a previous tick

                var strain = EffectiveStrain(p.plot_index);
                if (strain == null || mutationRng == null) continue;

                float pressure = Clamp01(env.OutdoorRadModifier / 300f)
                                 + Clamp01(p.toxicity_permille / 2000f);
                float threshold = Math.Clamp(strain.mutation_threshold, 0f, 1f);
                if (pressure < threshold || strain.mutation_outcomes.Count == 0)
                    continue; // ineligible: no mutation draw consumed

                var outcome = RollMutationOutcome(strain, mutationRng);
                p.mutation_outcome = (int)outcome;
                p.mutation_strain_id = outcome == AgriMutationOutcome.HardyStrain
                    ? RollHardyVariant(strain, mutationRng)
                    : "";
                OnMutationDetermined?.Invoke(p.plot_index, outcome, p.mutation_strain_id);
            }

            // 5. Ready/failed narrative events are raised on demand by the host
            //    through the greenhouse events + the flags above.
        }

        private Dictionary<int, int> CaptureStages()
        {
            var map = new Dictionary<int, int>();
            for (int i = 0; i < _greenhouse.Plots.Count; i++)
            {
                var gh = _greenhouse.Plots[i];
                if (gh != null) map[i] = gh.stage;
            }
            return map;
        }

        private static AgriMutationOutcome RollMutationOutcome(CropStrainDef strain, ISeededRng rng)
        {
            float total = 0f;
            for (int i = 0; i < strain.mutation_outcomes.Count; i++)
                total += Math.Max(0f, strain.mutation_outcomes[i].weight);
            if (total <= 0f) return AgriMutationOutcome.None;

            double roll = rng.NextDouble() * total;
            double acc = 0f;
            for (int i = 0; i < strain.mutation_outcomes.Count; i++)
            {
                var m = strain.mutation_outcomes[i];
                acc += Math.Max(0f, m.weight);
                if (roll < acc) return ParseOutcome(m.outcome);
            }
            return AgriMutationOutcome.None;
        }

        private static string RollHardyVariant(CropStrainDef strain, ISeededRng rng)
        {
            var variants = new List<MutationOutcomeDef>();
            float total = 0f;
            for (int i = 0; i < strain.mutation_outcomes.Count; i++)
            {
                var m = strain.mutation_outcomes[i];
                if (m != null && m.outcome == "hardy_strain" && !string.IsNullOrEmpty(m.result_strain_id))
                {
                    variants.Add(m);
                    total += Math.Max(0f, m.weight);
                }
            }
            if (variants.Count == 0) return "";
            variants.Sort((a, b) => string.CompareOrdinal(a.result_strain_id, b.result_strain_id));
            double roll = rng.NextDouble() * total;
            double acc = 0f;
            foreach (var v in variants)
            {
                acc += Math.Max(0f, v.weight);
                if (roll < acc) return v.result_strain_id;
            }
            return variants[variants.Count - 1].result_strain_id;
        }

        public static AgriMutationOutcome ParseOutcome(string outcome) => outcome switch
        {
            "yield_boost" => AgriMutationOutcome.YieldBoost,
            "yield_penalty" => AgriMutationOutcome.YieldPenalty,
            "toxic_harvest" => AgriMutationOutcome.ToxicHarvest,
            "hardy_strain" => AgriMutationOutcome.HardyStrain,
            "sterile_seed" => AgriMutationOutcome.SterileSeed,
            _ => AgriMutationOutcome.None
        };

        private static float MutationMultiplier(AgriMutationOutcome outcome) => outcome switch
        {
            AgriMutationOutcome.YieldBoost => 1.25f,
            AgriMutationOutcome.YieldPenalty => 0.7f,
            AgriMutationOutcome.ToxicHarvest => 0.9f,
            AgriMutationOutcome.SterileSeed => 0.5f,
            _ => 1f
        };

        private static string QualityTierFor(float multiplier, bool toxic)
        {
            if (toxic) return "tainted";
            if (multiplier >= 1.2f) return "prime";
            if (multiplier >= 0.9f) return "standard";
            if (multiplier >= 0.6f) return "poor";
            return "blighted";
        }

        private static bool TagMatch(List<string> tags, CropStrainDef strain)
        {
            if (strain == null) return false;
            for (int i = 0; i < tags.Count; i++)
                if (strain.tags != null && strain.tags.Contains(tags[i])) return true;
            return false;
        }

        private GreenhousePlotState GreenhousePlot(int plotIndex)
        {
            if (plotIndex < 0 || plotIndex >= _greenhouse.Plots.Count) return null;
            return _greenhouse.Plots[plotIndex];
        }

        private void ResetCropFields(AgriPlotState p)
        {
            p.strain_id = "";
            p.pest_severity = 0f;
            p.pest_id = "";
            p.mutation_outcome = (int)AgriMutationOutcome.None;
            p.mutation_strain_id = "";
            p.planted_day = 0;
        }

        // ------------------------------------------------------------------
        // Blight narrative one-shot (episode tracking over greenhouse events)
        // ------------------------------------------------------------------
        public void NotifyBlightOutbreak(int plotIndex)
        {
            if (_state.blight_narrative_plots.Contains(plotIndex)) return;
            _state.blight_narrative_plots.Add(plotIndex);
            OnBlightNarrative?.Invoke(plotIndex);
        }

        public void CloseBlightNarrative(int plotIndex)
        {
            _state.blight_narrative_plots.Remove(plotIndex);
        }

        private static float Clamp01(float v)
        {
            if (float.IsNaN(v)) return 0f;
            return v < 0f ? 0f : (v > 1f ? 1f : v);
        }

        private static float ClampRange(float v, float min, float max)
        {
            if (float.IsNaN(v)) return min;
            return v < min ? min : (v > max ? max : v);
        }
    }
}
