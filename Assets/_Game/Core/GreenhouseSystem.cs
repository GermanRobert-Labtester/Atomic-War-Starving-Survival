using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Crop lifecycle stages for a greenhouse plot.
    /// </summary>
    public enum GreenhouseStage
    {
        Fallow = 0,
        Sprouting = 1,
        Growing = 2,
        Mature = 3,
        Failed = 4
    }

    /// <summary>
    /// One plot's mutable state. Plain [Serializable] so JsonUtility can snapshot
    /// it through <see cref="GreenhouseSystem"/>'s <see cref="ISaveable"/> contract.
    /// </summary>
    [Serializable]
    public class GreenhousePlotState
    {
        public int plotIndex;
        public string seedItemId;        // empty => Fallow
        public int stage;                // GreenhouseStage value
        public float growth;             // 0..100 cumulative growth
        public float water;              // 0..100
        public float soilContamination;  // 0..100 (ash/rads-equivalent drift)
        public float blight;             // 0..1
        public int plantedDay;
    }

    /// <summary>
    /// Whole-greenhouse save state. The object returned by
    /// <see cref="GreenhouseSystem.CaptureState"/> and consumed by
    /// <see cref="GreenhouseSystem.RestoreState"/>.
    /// </summary>
    [Serializable]
    public class GreenhouseState
    {
        public string saveId = GreenhouseExpansionCatalog.SaveId;
        public List<GreenhousePlotState> plots = new List<GreenhousePlotState>();
        public bool preWarWheatUnlocked;
        public int totalHarvests;
    }

    /// <summary>
    /// Result of a harvest attempt. The host grants <see cref="yieldItemId"/>
    /// × <see cref="amount"/> to inventory when <see cref="success"/> is true.
    /// </summary>
    public struct GreenhouseHarvest
    {
        public bool success;
        public int plotIndex;
        public string yieldItemId;
        public int amount;
        public bool contaminated;
    }

    /// <summary>
    /// Expansion XI — "The Glass Orchard".
    ///
    /// A save-safe, plain-C# agriculture system: grow food under lead-glass and
    /// grow-lights in nuclear winter. The system owns the plot simulation and
    /// is deliberately inventory-agnostic — <see cref="Plant"/> /
    /// <see cref="Harvest"/> / <see cref="TreatBlight"/> return the item ids to
    /// spend or grant, and the host mediates the actual inventory. This keeps
    /// the growth/contamination math deterministic and unit-testable without a
    /// host, the same way <c>NutrientDripAutomation</c> returns its yield via an
    /// <c>out</c> parameter.
    ///
    /// Ticking is driven by the host's daily pass
    /// (<see cref="TickDay"/>) with two host-computed inputs: available
    /// grow-light hours (photoperiod + owned lamps) and the net ash-contamination
    /// drift (weather, reduced by lead-glass shielding). The system applies
    /// water consumption, growth accrual, contamination drift, and blight rolls
    /// (seeded RNG for determinism). Every state change raises a C# event the
    /// host can route to the journal / event runner / HUD.
    /// </summary>
    public class GreenhouseSystem : ISaveable
    {
        // ── Tuning (consts, like MutatedEcosystemSystem) ───────────────
        public const float MaxWater = 100f;
        public const float MaxContamination = 100f;
        /// <summary>Growth (0..100) at which Sprouting becomes Growing.</summary>
        public const float GrowingThreshold = 33f;
        /// <summary>Blight added per drought day (no rng — deterministic path).</summary>
        public const float DroughtBlightRatePerDay = 0.25f;
        /// <summary>Blight added when the random blight roll hits.</summary>
        public const float OutbreakBlightStep = 0.3f;
        /// <summary>Base daily chance of a blight outbreak before resistance/contamination/drought factors.</summary>
        public const float BaseBlightChancePerDay = 0.06f;
        /// <summary>Contamination added per unit of tainted irrigation water.</summary>
        public const float TaintedWaterContaminationPerUnit = 1.5f;
        /// <summary>Fraction of soil contamination retained after a harvest (soil memory).</summary>
        public const float ResidualContaminationAfterHarvest = 0.5f;

        private readonly GreenhouseState _state;
        private readonly System.Random _rng;

        public GreenhouseSystem(int seed)
        {
            _state = new GreenhouseState();
            _rng = new System.Random(seed);
        }

        // ── ISaveable ──────────────────────────────────────────────────
        public string SaveId => _state.saveId;

        public object CaptureState() => _state;

        public void RestoreState(object state)
        {
            if (state is GreenhouseState gs)
                CopyInto(_state, gs);
        }

        private static void CopyInto(GreenhouseState dst, GreenhouseState src)
        {
            if (src == null) return;
            dst.preWarWheatUnlocked = src.preWarWheatUnlocked;
            dst.totalHarvests = src.totalHarvests;
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
                    plantedDay = s.plantedDay
                });
            }
        }

        // ── Events (raised on state change, for UI + save + journal) ───
        public event Action<int, string, int> OnCropPlanted;   // plotIndex, seedItemId, currentDay
        public event Action<int, string> OnCropMatured;         // plotIndex, seedItemId
        public event Action<GreenhouseHarvest> OnCropHarvested;
        public event Action<int> OnBlightOutbreak;              // plotIndex
        public event Action<int> OnPlotDriedOut;                // plotIndex
        public event Action<int> OnCropFailed;                  // plotIndex

        // ── Read accessors ─────────────────────────────────────────────
        public GreenhouseState State => _state;
        public int PlotCount => _state.plots.Count;
        public int TotalHarvests => _state.totalHarvests;
        public bool IsPreWarWheatUnlocked => _state.preWarWheatUnlocked;
        public IReadOnlyList<GreenhousePlotState> Plots => _state.plots;

        // ── Plot management ────────────────────────────────────────────

        /// <summary>
        /// Grow/shrink the plot list to match owned planter boxes. New plots
        /// start fallow. Excess plots are only trimmed from the end while they
        /// are fallow — a growing crop is never silently destroyed.
        /// </summary>
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

        /// <summary>Plant a seed in a fallow plot. On success returns the seed id to consume.</summary>
        public bool Plant(int plotIndex, string seedItemId, int currentDay, out string consumedSeedId)
        {
            consumedSeedId = null;
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            var def = GreenhouseExpansionCatalog.CropCatalog.Get(seedItemId);
            if (def == null) return false;
            if (def.RequiresUnlock && !_state.preWarWheatUnlocked) return false;
            if (!IsFallow(plot)) return false;

            plot.seedItemId = seedItemId;
            plot.stage = (int)GreenhouseStage.Sprouting;
            plot.growth = 0f;
            plot.blight = 0f;
            plot.plantedDay = currentDay;
            consumedSeedId = seedItemId;
            OnCropPlanted?.Invoke(plotIndex, seedItemId, currentDay);
            return true;
        }

        /// <summary>
        /// Irrigate a plot. The host decides how much water to give (and from
        /// which inventory source). Tainted water grows the crop but adds
        /// contamination that taints the eventual harvest.
        /// </summary>
        public void Water(int plotIndex, float waterUnits, bool tainted)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null) return;
            float add = Mathf.Max(0f, waterUnits);
            plot.water = Mathf.Min(MaxWater, plot.water + add);
            if (tainted && add > 0f)
                plot.soilContamination = Mathf.Min(MaxContamination,
                    plot.soilContamination + add * TaintedWaterContaminationPerUnit);
        }

        /// <summary>
        /// Harvest a Mature plot. Returns the yield to grant; resets the plot
        /// to fallow while retaining <see cref="ResidualContaminationAfterHarvest"/>
        /// of its soil contamination (the soil remembers).
        /// </summary>
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

        /// <summary>
        /// Clear a blighted/failed or fallow plot for reuse. Returns false on an
        /// out-of-range index (no-op).
        /// </summary>
        public bool Clear(int plotIndex)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null) return false;
            ResetPlot(plot);
            return true;
        }

        /// <summary>
        /// Apply blight treatment to a non-failed plot with active blight.
        /// Returns true (and the treatment id to consume) when applied. A Failed
        /// crop is dead — treatment cannot revive it; <see cref="Clear"/> it instead.
        /// </summary>
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

        /// <summary>
        /// Add a uniform contamination surge to every plot (e.g. a cracked
        /// lead-glass pane letting ash in). Used by the host when the
        /// "glass breaks" event resolves unfixed.
        /// </summary>
        public void SurgeContamination(float amount)
        {
            amount = Mathf.Max(0f, amount);
            for (int i = 0; i < _state.plots.Count; i++)
            {
                var p = _state.plots[i];
                if (p == null) continue;
                p.soilContamination = Mathf.Min(MaxContamination, p.soilContamination + amount);
            }
        }

        /// <summary>Unlock pre-war wheat (the Svalbard Seed Ledger reward).</summary>
        public void UnlockPreWarWheat()
        {
            _state.preWarWheatUnlocked = true;
        }

        // ── Daily tick ─────────────────────────────────────────────────

        /// <summary>
        /// Advance every planted plot by one day. Inputs are host-computed:
        /// <paramref name="growLightHours"/> = photoperiod + grow-lamp bonus;
        /// <paramref name="ashContaminationRate"/> = net ash drift after
        /// lead-glass shielding. Deterministic via the seeded RNG.
        /// </summary>
        public void TickDay(int currentDay, float growLightHours, float ashContaminationRate)
        {
            for (int i = 0; i < _state.plots.Count; i++)
                TickPlot(i, currentDay, growLightHours, ashContaminationRate);
        }

        private void TickPlot(int i, int currentDay, float growLightHours, float ashContaminationRate)
        {
            var p = _state.plots[i];
            if (IsFallow(p)) return;
            // Mature and Failed plots are stable — nothing to tick.
            if (p.stage == (int)GreenhouseStage.Mature) return;
            if (p.stage == (int)GreenhouseStage.Failed) return;

            var def = GreenhouseExpansionCatalog.CropCatalog.Get(p.seedItemId);
            if (def == null) return;

            // ── Water consumption ──────────────────────────────────────
            float prevWater = p.water;
            p.water = Mathf.Max(0f, p.water - def.WaterPerDay);
            bool hasWater = p.water > 0f;
            if (prevWater > 0f && !hasWater)
                OnPlotDriedOut?.Invoke(i);

            // ── Growth (only while watered) ────────────────────────────
            if (hasWater)
            {
                float lightFactor = def.LightHoursPerDay <= 0f
                    ? 1f
                    : Mathf.Clamp01(growLightHours / def.LightHoursPerDay);
                float daysToMature = Mathf.Max(1f, def.GrowthHoursToMature / 24f);
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

            // ── Contamination drift from ambient ash ───────────────────
            if (ashContaminationRate > 0f)
                p.soilContamination = Mathf.Min(MaxContamination,
                    p.soilContamination + ashContaminationRate);

            // ── Blight: deterministic drought accrual + random outbreak ─
            if (!hasWater)
                ApplyBlight(i, p, DroughtBlightRatePerDay);

            // Blight pressure comes from soil contamination (driven by ash) and
            // drought — not from pristine soil. This keeps a clean, well-tended
            // plot deterministic and makes lead-glass shielding (which cuts ash
            // drift) meaningfully reduce blight over time.
            float droughtFactor = hasWater ? 1f : 2.5f;
            float contamFactor = Mathf.Clamp01(p.soilContamination / MaxContamination);
            float chance = BaseBlightChancePerDay
                           * (1f - def.BlightResistance)
                           * contamFactor
                           * droughtFactor;
            if (_rng.NextDouble() < chance)
                ApplyBlight(i, p, OutbreakBlightStep);
        }

        /// <summary>
        /// Add blight to a plot and fire the relevant events. The outbreak
        /// warning (<see cref="OnBlightOutbreak"/>) fires exactly once per
        /// episode — at the first sign of blight — so the host's blight modal
        /// is not raised twice as a plot decays. Reaching 1.0 blight fails the
        /// crop and fires <see cref="OnCropFailed"/> (a journal entry, not a
        /// second modal). The only exception is an instant-kill from clean
        /// soil, which warns and fails in the same step.
        /// </summary>
        private void ApplyBlight(int plotIndex, GreenhousePlotState p, float amount)
        {
            if (p.stage == (int)GreenhouseStage.Failed) return;
            float before = p.blight;
            p.blight = Mathf.Min(1f, p.blight + Mathf.Max(0f, amount));

            if (p.blight >= 1f)
            {
                p.stage = (int)GreenhouseStage.Failed;
                if (before <= 0f) OnBlightOutbreak?.Invoke(plotIndex); // instant-kill from clean: still warn once
                OnCropFailed?.Invoke(plotIndex);
            }
            else if (before <= 0f && p.blight > 0f)
            {
                // First sign of blight on a previously clean plot — the one warning.
                OnBlightOutbreak?.Invoke(plotIndex);
            }
        }

        // ── Helpers ────────────────────────────────────────────────────
        private GreenhousePlotState PlotAt(int plotIndex)
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
            p.soilContamination = Mathf.Max(0f, p.soilContamination * ResidualContaminationAfterHarvest);
            p.plantedDay = 0;
        }
    }
}
