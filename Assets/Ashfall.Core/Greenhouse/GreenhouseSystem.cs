// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.PlayerCommand;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    public enum GreenhouseStage
    {
        Fallow = 0,
        Sprouting = 1,
        Growing = 2,
        Mature = 3,
        Failed = 4
    }

    [Serializable]
    public class GreenhousePlotState
    {
        public int plotIndex;
        public string seedItemId;
        public int stage;
        public float growth;
        public float water;
        public float soilContamination;
        public float blight;
        public int plantedDay;

        /// <summary>
        /// B5–B8 Phase 4 (Plan 64): nutrient band 0..1 (additive field —
        /// legacy saves restore 0). Raised by canonical
        /// <c>item_hydroponic_nutrients</c> application; decays daily. A fed
        /// crop sits in the correct nutrient band, which lowers blight risk
        /// (visible prevention contributor — see
        /// <see cref="GreenhouseSystem.NutrientBlightRiskReduction"/>).
        /// </summary>
        public float nutrientLevel;

        /// <summary>
        /// B5–B8 expansion (§27 crop rotation): consecutive plantings of the
        /// same crop on this plot (additive field — legacy saves restore 0).
        /// Monoculture exhausts the bed's disease resistance; planting a
        /// different crop resets it. Visible as a blight-risk contributor.
        /// </summary>
        public int sameCropStreak;

        /// <summary>The previous crop planted on this bed (rotation ledger —
        /// survives harvest/reset because the soil, not the tray state,
        /// remembers the monoculture; additive, legacy restores empty).</summary>
        public string lastCropId = string.Empty;
    }

    [Serializable]
    public class GreenhouseState
    {
        public string saveId = GreenhouseExpansionCatalog.SaveId;
        public List<GreenhousePlotState> plots = new List<GreenhousePlotState>();
        public bool preWarWheatUnlocked;
        public int totalHarvests;
        /// <summary>A11: deterministic blight-roll count (reseed pattern).</summary>
        public long blightRollCount;
        public ApicultureState? apiculture;
    }

    public struct GreenhouseHarvest
    {
        public bool success;
        public int plotIndex;
        public string yieldItemId;
        public int amount;
        public bool contaminated;
    }

    /// <summary>
    /// B5–B8 Phase 4 (Plan 64): read-only blight-risk decomposition for UI
    /// projection and balance harnesses (flagship §7.7 — visible prevention
    /// contributors, no fake precision: this is exactly the TickDay roll's
    /// input, not a forecast of the roll itself).
    /// chance = Base·Resistance·Contamination·Drought − Nutrient, clamped [0,1].
    /// </summary>
    public struct BlightRiskProfile
    {
        public int PlotIndex;
        public bool PlotExists;
        public float BaseChancePerDay;
        public float ResistanceFactor;
        public float ContaminationPressure;
        public float DroughtStress;
        public float NutrientReduction;
        public float RotationPressure;
        public int RotationStreak;
        public float NutrientLevel;
        public float FinalChancePerDay;
    }

    /// <summary>
    /// ASHFALL: THE GLASS ORCHARD (Expansion 05 / XI).
    /// Pure C# save-safe agricultural simulation engine under lead-glass and grow-lights.
    /// </summary>
    public class GreenhouseSystem
    {
        public const float MaxWater = 100f;
        public const float MaxContamination = 100f;
        public const float GrowingThreshold = 33f;
        public const float DroughtBlightRatePerDay = 0.25f;
        public const float OutbreakBlightStep = 0.3f;
        public const float BaseBlightChancePerDay = 0.06f;
        public const float TaintedWaterContaminationPerUnit = 1.5f;
        public const float ResidualContaminationAfterHarvest = 0.5f;

        // ---- B5–B8 Phase 4 (Plan 64): nutrient band + visible blight prevention

        /// <summary>Canonical nutrient item consumed by
        /// <see cref="ApplyNutrients"/> (host checks inventory first).</summary>
        public const string NutrientItemId = "item_hydroponic_nutrients";

        /// <summary>Nutrient-band level one application adds (two applications
        /// saturate the band).</summary>
        public const float NutrientApplicationLevel = 0.5f;

        /// <summary>Daily nutrient decay — dosing is a recurring cost, not a
        /// permanent buff.</summary>
        public const float NutrientDecayPerDay = 0.1f;

        /// <summary>Bounded blight-risk reduction at a full nutrient band
        /// (subtractive, applied after the legacy multiplicative factors; the
        /// final chance clamps at 0 — prevention can never make risk negative).</summary>
        public const float NutrientBlightRiskReduction = 0.04f;

        /// <summary>Nutrient level at which the full risk reduction applies.</summary>
        public const float NutrientFullBandLevel = 0.5f;

        /// <summary>B5–B8 expansion (§27): blight-risk step per consecutive
        /// planting of the same crop on one plot (monoculture pressure).
        /// Bounded by the final clamp; rotation (a different crop) resets the
        /// streak to zero.</summary>
        public const float RotationBlightStepPerStreak = 0.015f;

        /// <summary>Hard cap on the streak's risk contribution (ten same-crop
        /// plantings reach the ceiling).</summary>
        public const int MaxRotationStreakCount = 10;

        private readonly GreenhouseState _state;
        private readonly int _seed;

        public GreenhouseSystem(int seed = 1)
        {
            _state = new GreenhouseState();
            _seed = seed;
        }

        public string SaveId => _state.saveId;

        /// <summary>
        /// Deep copy: the live plots must never alias the save envelope, or a
        /// later tick mutates the snapshot that is still on its way to disk.
        /// </summary>
        public GreenhouseState CaptureState()
        {
            var copy = new GreenhouseState();
            CopyInto(copy, _state);
            return copy;
        }

        public void RestoreState(GreenhouseState gs)
        {
            if (gs != null)
                CopyInto(_state, gs);
        }

        private static void CopyInto(GreenhouseState dst, GreenhouseState src)
        {
            if (src == null) return;
            dst.preWarWheatUnlocked = src.preWarWheatUnlocked;
            dst.totalHarvests = src.totalHarvests;
            dst.blightRollCount = Math.Max(0L, src.blightRollCount);
            dst.apiculture = src.apiculture;
            dst.plots = new List<GreenhousePlotState>(src.plots != null ? src.plots.Count : 0);
            if (src.plots == null) return;
            for (int i = 0; i < src.plots.Count; i++)
            {
                var s = src.plots[i];
                if (s == null) continue;
                dst.plots.Add(new GreenhousePlotState
                {
                    plotIndex = s.plotIndex,
                    seedItemId = s.seedItemId,
                    stage = s.stage,
                    growth = s.growth,
                    water = s.water,
                    soilContamination = s.soilContamination,
                    blight = s.blight,
                    plantedDay = s.plantedDay,
                    nutrientLevel = Math.Clamp(s.nutrientLevel, 0f, 1f),
                    sameCropStreak = Math.Clamp(s.sameCropStreak, 0, MaxRotationStreakCount),
                    lastCropId = s.lastCropId ?? string.Empty
                });
            }
        }

        public event Action<int, string, int> OnCropPlanted;
        public event Action<int, string> OnCropMatured;
        public event Action<GreenhouseHarvest> OnCropHarvested;
        public event Action<int> OnBlightOutbreak;
        public event Action<int> OnPlotDriedOut;
        public event Action<int> OnCropFailed;

        public GreenhouseState State => _state;
        public int PlotCount => _state.plots.Count;
        public int TotalHarvests => _state.totalHarvests;
        public bool IsPreWarWheatUnlocked => _state.preWarWheatUnlocked;
        public IReadOnlyList<GreenhousePlotState> Plots => _state.plots;

        public void EnsurePlots(int planterBoxCount)
        {
            if (planterBoxCount < 0) planterBoxCount = 0;
            while (_state.plots.Count < planterBoxCount)
                _state.plots.Add(NewPlot(_state.plots.Count));
            while (_state.plots.Count > planterBoxCount)
            {
                int last = _state.plots.Count - 1;
                if (!IsFallow(_state.plots[last])) break;
                _state.plots.RemoveAt(last);
            }
        }

        private static GreenhousePlotState NewPlot(int index) =>
            new GreenhousePlotState { plotIndex = index, water = 0f };

        public static bool IsFallow(GreenhousePlotState p) =>
            p == null || string.IsNullOrEmpty(p.seedItemId);

        public bool Plant(int plotIndex, string seedItemId, int currentDay, out string consumedSeedId)
        {
            consumedSeedId = null!;
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            var def = GreenhouseExpansionCatalog.CropCatalog.Get(seedItemId);
            if (def == null) return false;
            if (def.RequiresUnlock && !_state.preWarWheatUnlocked) return false;
            if (!IsFallow(plot)) return false;

            // B5–B8 expansion (§27): rotation ledger — the same crop again on
            // the same bed builds monoculture pressure; rotating resets it.
            // The bed remembers via lastCropId, which survives harvest/reset.
            plot.sameCropStreak = string.Equals(plot.lastCropId, seedItemId, StringComparison.Ordinal)
                ? Math.Min(MaxRotationStreakCount, plot.sameCropStreak + 1)
                : 0;
            plot.lastCropId = seedItemId;
            plot.seedItemId = seedItemId;
            plot.stage = (int)GreenhouseStage.Sprouting;
            plot.growth = 0f;
            plot.blight = 0f;
            plot.plantedDay = currentDay;
            consumedSeedId = seedItemId;
            OnCropPlanted?.Invoke(plotIndex, seedItemId, currentDay);
            return true;
        }

        public bool Water(int plotIndex, float waterUnits, bool tainted)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            float add = Math.Max(0f, waterUnits);
            plot.water = Math.Min(MaxWater, plot.water + add);
            if (tainted && add > 0f)
                plot.soilContamination = Math.Min(MaxContamination,
                    plot.soilContamination + add * TaintedWaterContaminationPerUnit);
            return true;
        }

        /// <summary>
        /// B5–B8 Phase 4 (Plan 64): apply one canonical
        /// <see cref="NutrientItemId"/> dose to a planted plot. The host
        /// checks/consumes the inventory item first (same discipline as the
        /// treatment path); this raises the plot's nutrient band toward
        /// saturation. Blocked on fallow/failed plots — nutrients feed a
        /// crop, not the soil.
        /// </summary>
        public bool ApplyNutrients(int plotIndex, out string consumedItemId)
        {
            consumedItemId = NutrientItemId;
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            if (IsFallow(plot)) return false;
            if (plot.stage == (int)GreenhouseStage.Failed) return false;
            plot.nutrientLevel = Math.Min(1f, plot.nutrientLevel + NutrientApplicationLevel);
            return true;
        }

        /// <summary>
        /// B5–B8 Phase 4: read-only blight-risk decomposition for one plot
        /// (flagship §7.7 — visible prevention contributors, no RNG consumed,
        /// no state mutated). The final chance matches the TickDay outbreak
        /// roll exactly, so UI projections are truthful.
        /// </summary>
        public BlightRiskProfile GetBlightRiskProfile(int plotIndex, bool hasWater)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null || IsFallow(plot) ||
                plot.stage == (int)GreenhouseStage.Failed)
            {
                return new BlightRiskProfile { PlotIndex = plotIndex, PlotExists = false };
            }

            var def = GreenhouseExpansionCatalog.CropCatalog.Get(plot.seedItemId);
            if (def == null)
                return new BlightRiskProfile { PlotIndex = plotIndex, PlotExists = false };

            float contaminationPressure = Math.Clamp(plot.soilContamination / MaxContamination, 0f, 1f);
            float droughtStress = hasWater ? 1f : 2.5f;
            float nutrientReduction = NutrientBlightRiskReduction
                                      * Math.Min(1f, plot.nutrientLevel / NutrientFullBandLevel);
            float rotationPressure = RotationBlightStepPerStreak
                                     * Math.Min(MaxRotationStreakCount, plot.sameCropStreak);
            float core = BaseBlightChancePerDay * (1f - def.BlightResistance)
                         * contaminationPressure * droughtStress;
            return new BlightRiskProfile
            {
                PlotIndex = plotIndex,
                PlotExists = true,
                BaseChancePerDay = BaseBlightChancePerDay,
                ResistanceFactor = 1f - def.BlightResistance,
                ContaminationPressure = contaminationPressure,
                DroughtStress = droughtStress,
                NutrientReduction = nutrientReduction,
                RotationPressure = rotationPressure,
                RotationStreak = plot.sameCropStreak,
                FinalChancePerDay = Math.Clamp(core - nutrientReduction + rotationPressure, 0f, 1f),
                NutrientLevel = plot.nutrientLevel
            };
        }

        public GreenhouseHarvest Harvest(int plotIndex)
        {
            var res = new GreenhouseHarvest { plotIndex = plotIndex, success = false };
            var plot = PlotAt(plotIndex);
            if (plot == null || plot.stage != (int)GreenhouseStage.Mature) return res;

            var def = GreenhouseExpansionCatalog.CropCatalog.Get(plot.seedItemId);
            if (def == null)
            {
                ResetPlot(plot);
                return res;
            }

            bool contaminated = plot.soilContamination >= def.ContaminationTolerance;
            res.success = true;
            res.yieldItemId = contaminated ? def.YieldTaintedId : def.YieldCleanId;
            res.amount = def.BaseYield;
            res.contaminated = contaminated;

            _state.totalHarvests++;
            ResetPlot(plot);
            OnCropHarvested?.Invoke(res);
            return res;
        }

        public bool Clear(int plotIndex)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            ResetPlot(plot);
            return true;
        }

        /// <summary>
        /// Side-effect-free preview of a blight treatment command.
        /// Shares the same validation path as <see cref="TreatBlight"/>.
        /// </summary>
        public CommandPreview PreviewTreatBlight(int plotIndex, long stateVersion = 0)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null)
                return CommandPreview.Unavailable(PlayerCommandCode.GreenhouseTreatBlight, "invalid_plot", "greenhouse.invalid_plot", stateVersion);
            if (plot.stage == (int)GreenhouseStage.Failed)
                return CommandPreview.Unavailable(PlayerCommandCode.GreenhouseTreatBlight, "plot_failed", "greenhouse.plot_failed", stateVersion);
            if (plot.blight <= 0f)
                return CommandPreview.Unavailable(PlayerCommandCode.GreenhouseTreatBlight, "no_blight", "greenhouse.no_blight", stateVersion);

            return CommandPreview.Available(
                PlayerCommandCode.GreenhouseTreatBlight,
                stateVersion,
                new Dictionary<string, double> { { "plot_index", plotIndex }, { "blight_cured", 1 } },
                isIrreversible: false,
                messageKey: "greenhouse.preview_treat_blight");
        }

        /// <summary>
        /// Execute a blight treatment using the same validation path as <see cref="PreviewTreatBlight"/>.
        /// Stale previews are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteTreatBlight(int plotIndex, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewTreatBlight(plotIndex, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.GreenhouseTreatBlight, preview.StateVersion, currentStateVersion);

            string consumedTreatmentId;
            bool ok = TreatBlight(plotIndex, out consumedTreatmentId);
            if (!ok)
                return new CommandResult(
                    PlayerCommandCode.GreenhouseTreatBlight,
                    ActionResult.Failed("execute_failed", "greenhouse.execute_failed"),
                    expectedStateVersion,
                    currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.GreenhouseTreatBlight,
                ActionResult.Success("greenhouse.blight_treated",
                    new Dictionary<string, double> { { "plot_index", plotIndex }, { "consumed_treatment", 1 } }),
                expectedStateVersion,
                currentStateVersion + 1);
        }

        public bool TreatBlight(int plotIndex, out string consumedTreatmentId)
        {
            consumedTreatmentId = GreenhouseExpansionCatalog.Items.BlightTreatment;
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            if (plot.stage == (int)GreenhouseStage.Failed) return false;
            if (plot.blight <= 0f) return false;
            plot.blight = 0f;
            return true;
        }

        public void SurgeContamination(float amount)
        {
            amount = Math.Max(0f, amount);
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p == null) continue;
                p.soilContamination = Math.Min(MaxContamination, p.soilContamination + amount);
            }
        }

        public void UnlockPreWarWheat()
        {
            _state.preWarWheatUnlocked = true;
        }

        public void TickDay(int currentDay, float growLightHours, float ashContaminationRate)
        {
            for (int i = 0; i < _state.plots.Count; i++)
                TickPlot(i, currentDay, growLightHours, ashContaminationRate);
        }

        private void TickPlot(int i, int currentDay, float growLightHours, float ashContaminationRate)
        {
            var p = _state.plots[i];
            if (IsFallow(p)) return;
            if (p.stage == (int)GreenhouseStage.Mature) return;
            if (p.stage == (int)GreenhouseStage.Failed) return;

            var def = GreenhouseExpansionCatalog.CropCatalog.Get(p.seedItemId);
            if (def == null) return;

            float prevWater = p.water;
            p.water = Math.Max(0f, p.water - def.WaterPerDay);
            bool hasWater = p.water > 0f;
            if (prevWater > 0f && !hasWater)
                OnPlotDriedOut?.Invoke(i);

            if (hasWater)
            {
                float lightFactor = def.LightHoursPerDay <= 0f
                    ? 1f
                    : Math.Clamp(growLightHours / def.LightHoursPerDay, 0f, 1f);
                float daysToMature = Math.Max(1f, def.GrowthHoursToMature / 24f);
                p.growth += lightFactor * (100f / daysToMature);

                if (p.stage == (int)GreenhouseStage.Sprouting && p.growth >= GrowingThreshold)
                    p.stage = (int)GreenhouseStage.Growing;

                if (p.stage == (int)GreenhouseStage.Growing && p.growth >= 100f)
                {
                    p.growth = 100f;
                    p.stage = (int)GreenhouseStage.Mature;
                    OnCropMatured?.Invoke(i, p.seedItemId);
                }
            }

            if (ashContaminationRate > 0f)
                p.soilContamination = Math.Min(MaxContamination,
                    p.soilContamination + ashContaminationRate);

            if (!hasWater)
                ApplyBlight(i, p, DroughtBlightRatePerDay);

            float droughtFactor = hasWater ? 1f : 2.5f;
            float contamFactor = Math.Clamp(p.soilContamination / MaxContamination, 0f, 1f);
            // B5–B8 Phase 4: nutrient-band prevention — subtractive, bounded,
            // never negative (flagship §7.7 probability decomposition). At
            // nutrientLevel 0 the legacy chance is untouched (parity).
            float nutrientReduction = NutrientBlightRiskReduction
                                      * Math.Min(1f, p.nutrientLevel / NutrientFullBandLevel);
            // B5–B8 expansion (§27): monoculture pressure — consecutive same-
            // crop plantings raise disease pressure (visible contributor).
            float rotationPressure = RotationBlightStepPerStreak
                                     * Math.Min(MaxRotationStreakCount, p.sameCropStreak);
            float chance = Math.Clamp(
                BaseBlightChancePerDay * (1f - def.BlightResistance) * contamFactor * droughtFactor
                - nutrientReduction + rotationPressure,
                0f, 1f);
            // Daily nutrient decay: dosing is a recurring cost.
            p.nutrientLevel = Math.Max(0f, p.nutrientLevel - NutrientDecayPerDay);
            // A11: deterministic reseed-per-roll (seed + roll count); the count
            // is persisted so restored saves continue, not replay, the stream.
            var blightRng = new SeededRng(unchecked(_seed * 397 + (int)(_state.blightRollCount & 0x7FFFFFFF)));
            _state.blightRollCount++;
            if (blightRng.NextDouble() < chance)
                ApplyBlight(i, p, OutbreakBlightStep);
        }

        private void ApplyBlight(int plotIndex, GreenhousePlotState p, float amount)
        {
            if (p.stage == (int)GreenhouseStage.Failed) return;
            float before = p.blight;
            p.blight = Math.Min(1f, p.blight + Math.Max(0f, amount));
            if (p.blight >= 1f)
            {
                p.stage = (int)GreenhouseStage.Failed;
                OnBlightOutbreak?.Invoke(plotIndex);
                OnCropFailed?.Invoke(plotIndex);
            }
            else if (before <= 0f && p.blight > 0f)
            {
                OnBlightOutbreak?.Invoke(plotIndex);
            }
        }

        private GreenhousePlotState? PlotAt(int plotIndex)
        {
            if (plotIndex < 0 || plotIndex >= _state.plots.Count) return null;
            return _state.plots[plotIndex];
        }

        private static void ResetPlot(GreenhousePlotState p)
        {
            p.seedItemId = "";
            p.stage = (int)GreenhouseStage.Fallow;
            p.growth = 0f;
            p.blight = 0f;
            p.water = 0f;
            p.nutrientLevel = 0f;
            // sameCropStreak persists through harvest/clear — the soil
            // remembers the monoculture; only planting a different crop
            // resets it (rotation ledger in Plant).
            p.soilContamination = Math.Max(0f, p.soilContamination * ResidualContaminationAfterHarvest);
            p.plantedDay = 0;
        }
    }
}
