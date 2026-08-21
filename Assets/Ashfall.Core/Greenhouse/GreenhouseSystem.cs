using System;
using System.Collections.Generic;

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

            plot.seedItemId = seedItemId;
            plot.stage = (int)GreenhouseStage.Sprouting;
            plot.growth = 0f;
            plot.blight = 0f;
            plot.plantedDay = currentDay;
            consumedSeedId = seedItemId;
            OnCropPlanted?.Invoke(plotIndex, seedItemId, currentDay);
            return true;
        }

        public void Water(int plotIndex, float waterUnits, bool tainted)
        {
            var plot = PlotAt(plotIndex);
            if (plot == null) return;
            float add = Math.Max(0f, waterUnits);
            plot.water = Math.Min(MaxWater, plot.water + add);
            if (tainted && add > 0f)
                plot.soilContamination = Math.Min(MaxContamination,
                    plot.soilContamination + add * TaintedWaterContaminationPerUnit);
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
            float chance = BaseBlightChancePerDay
                           * (1f - def.BlightResistance)
                           * contamFactor
                           * droughtFactor;
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
            p.soilContamination = Math.Max(0f, p.soilContamination * ResidualContaminationAfterHarvest);
            p.plantedDay = 0;
        }
    }
}
