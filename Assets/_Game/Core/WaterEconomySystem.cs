using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Weather-driven bunker water economy: advances the roof catchment (fills
    /// WaterStorage from Rain/FalloutStorm while open) and the water purifier's
    /// 3-tier conversion queue (irradiated -&gt; dirty -&gt; clean). Mirrors
    /// RadioTunerSystem's shape (plain Tick(gameHours, weather, day) call from
    /// GameBootstrap) rather than living in ShelterModuleInstance.Tick, since it
    /// needs current weather/day/WaterStorage that generic module ticking
    /// doesn't have access to.
    /// </summary>
    
    [Serializable]
    public class WaterEconomySystemSave
    {
        public string systemId = "water_economy_system";
        public int purifierQueueMode = (int)PurifierQueueMode.Auto;
    }
public class WaterEconomySystem
    {
        /// <summary>Day on which Rain collection turns bacterial (dirty) instead of clean.</summary>
        public const int ContaminationOnsetDay = 30;

        public const string CatchmentModuleId = "catchment_surface";
        public const string PurifierModuleId = "water_purifier";

        /// <summary>Default catchment collection rate used when no module definition is attached.</summary>
        private const float DefaultCollectionRatePerHour = 5f;
        private const float DefaultConversionHoursPerUnit = 2f;
        private const float DefaultFilterDegradationPerUnitConverted = 5f;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<float> _getPurifierHoursPerUnitMultiplier;

        public PurifierQueueMode CurrentPurifierQueue { get; private set; } = PurifierQueueMode.Auto;

        /// <summary>Raised only when cistern contents or purifier runtime state changes.</summary>
        public event Action OnWaterStateChanged;
        /// <summary>Raised when the player changes the purifier work queue.</summary>
        public event Action<PurifierQueueMode> OnPurifierQueueChanged;
        /// <summary>Raised when clean water is produced by the purifier (Expansion IV Lethe hook).</summary>
        public event Action<float, IReadOnlyList<Survivor>> OnWaterPurified;

        /// <summary>Prompt #225 — Hydraulic Master purifier speed + humidity extract.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        /// <summary>Bind a transient multiplier for staffed purifier supervision.</summary>
        public void SetPurifierHoursPerUnitMultiplierProvider(Func<float> provider)
        {
            _getPurifierHoursPerUnitMultiplier = provider;
            OnWaterStateChanged?.Invoke();
        }

        public void Tick(float gameHours, WeatherKind weather, int currentDay, Shelter.Shelter shelter, WaterStorage storage)
        {
            if (gameHours <= 0f || shelter == null || storage == null) return;

            var before = CaptureStateStamp(shelter, storage);

            CollectCatchment(gameHours, weather, currentDay, shelter, storage);
            RunPurifier(gameHours, shelter, storage);
            ExtractHumidityWater(gameHours, shelter, storage);
            ApplyMakeupWaterBurn(gameHours, storage);

            if (HasStateChanged(before, CaptureStateStamp(shelter, storage)))
                OnWaterStateChanged?.Invoke();
        }

        /// <summary>Set a player-selected work priority for the powered purifier.</summary>
        public bool SetPurifierQueueMode(PurifierQueueMode queueMode)
        {
            queueMode = ClampQueueMode(queueMode);
            if (CurrentPurifierQueue == queueMode) return false;
            CurrentPurifierQueue = queueMode;
            OnPurifierQueueChanged?.Invoke(CurrentPurifierQueue);
            OnWaterStateChanged?.Invoke();
            return true;
        }

        /// <summary>Cycle the terminal queue one step in either direction.</summary>
        public bool CyclePurifierQueue(int direction)
        {
            if (direction == 0) return false;
            int count = Enum.GetValues(typeof(PurifierQueueMode)).Length;
            int next = ((int)CurrentPurifierQueue + (direction > 0 ? 1 : -1) + count) % count;
            return SetPurifierQueueMode((PurifierQueueMode)next);
        }

        /// <summary>Build a display-only cistern, purifier, and next-work projection.</summary>
        public WaterPurificationSnapshot GetSnapshot(Shelter.Shelter shelter, WaterStorage storage)
        {
            var purifier = shelter != null ? shelter.GetModule(PurifierModuleId) : null;
            bool operational = purifier != null && purifier.IsOperational && purifier.FilterHealth > 0f;
            var source = SelectConversionSource(storage);
            float hoursPerUnit = purifier != null ? GetEffectiveHoursPerUnit(purifier) : DefaultConversionHoursPerUnit;
            float degradationPerUnit = purifier != null
                ? GetFilterDegradationPerUnit(purifier)
                : DefaultFilterDegradationPerUnitConverted;
            int unitsQueued = UnitsForSource(storage, source);
            float filterBurnPerHour = operational && unitsQueued > 0 && hoursPerUnit > 0f
                ? degradationPerUnit / hoursPerUnit
                : 0f;
            float filterRuntimeHours = filterBurnPerHour > 0f
                ? Mathf.Max(0f, purifier.FilterHealth) / filterBurnPerHour
                : -1f;
            return new WaterPurificationSnapshot
            {
                CleanWater = storage != null ? Mathf.Max(0f, storage.CleanWater) : 0f,
                DirtyWater = storage != null ? Mathf.Max(0f, storage.DirtyWater) : 0f,
                IrradiatedWater = storage != null ? Mathf.Max(0f, storage.IrradiatedWater) : 0f,
                QueueMode = CurrentPurifierQueue,
                PurifierOperational = operational,
                FilterHealth = purifier != null ? Mathf.Max(0f, purifier.FilterHealth) : 0f,
                ConversionProgressHours = purifier != null ? Mathf.Max(0f, purifier.WaterConversionProgress) : 0f,
                HoursPerUnit = hoursPerUnit,
                FilterDegradationPerUnit = degradationPerUnit,
                FilterBurnPerHour = filterBurnPerHour,
                FilterRuntimeHours = filterRuntimeHours,
                NextSourceLabel = SourceLabel(source),
                NextOutputLabel = OutputLabel(source),
                UnitsQueued = unitsQueued
            };
        }

        private static void CollectCatchment(float gameHours, WeatherKind weather, int currentDay, Shelter.Shelter shelter, WaterStorage storage)
        {
            var catchment = shelter.GetModule(CatchmentModuleId);
            if (catchment == null || !catchment.IsOperational) return; // trap closed or not installed

            // Black Rain (Prompt #11): open catchment is instantly ruined — any
            // clean/dirty cistern water becomes irradiated, then oily rain fills
            // only the irradiated tier.
            if (weather == WeatherKind.BlackRain)
            {
                storage.RuinCleanAndDirtyToIrradiated();
                var blackDef = catchment.Definition as CatchmentSurfaceModuleSO;
                float blackRate = blackDef != null ? blackDef.CollectionRatePerHour : DefaultCollectionRatePerHour;
                float blackCollected = blackRate * gameHours;
                if (blackCollected > 0f) storage.AddIrradiated(blackCollected);
                return;
            }

            if (weather != WeatherKind.Rain && weather != WeatherKind.FalloutStorm) return;

            var def = catchment.Definition as CatchmentSurfaceModuleSO;
            float rate = def != null ? def.CollectionRatePerHour : DefaultCollectionRatePerHour;
            float collected = rate * gameHours;
            if (collected <= 0f) return;

            if (weather == WeatherKind.FalloutStorm)
            {
                storage.AddIrradiated(collected);
            }
            else if (currentDay < ContaminationOnsetDay)
            {
                storage.AddClean(collected);
            }
            else
            {
                storage.AddDirty(collected);
            }
        }

        private void RunPurifier(float gameHours, Shelter.Shelter shelter, WaterStorage storage)
        {
            var purifier = shelter.GetModule(PurifierModuleId);
            if (purifier == null || !purifier.IsOperational || purifier.FilterHealth <= 0f) return;

            float hoursPerUnit = GetEffectiveHoursPerUnit(purifier);
            float degradePerUnit = GetFilterDegradationPerUnit(purifier);

            purifier.WaterConversionProgress += gameHours;

            float cleanProduced = 0f;
            int safety = 0;
            while (purifier.WaterConversionProgress >= hoursPerUnit && purifier.FilterHealth > 0f && safety < 10000)
            {
                var source = SelectConversionSource(storage);
                if (source == ConversionSource.None) break;
                if (source == ConversionSource.Irradiated)
                {
                    storage.ConsumeIrradiated(1f);
                    storage.AddDirty(1f);
                }
                else
                {
                    storage.ConsumeDirty(1f);
                    storage.AddClean(1f);
                    cleanProduced += 1f;
                }

                purifier.WaterConversionProgress -= hoursPerUnit;
                purifier.FilterHealth = Mathf.Max(0f, purifier.FilterHealth - degradePerUnit);
                safety++;
            }

            if (cleanProduced > 0f)
                OnWaterPurified?.Invoke(cleanProduced, _getSurvivors?.Invoke());
        }

        /// <summary>
        /// Prompt #225 — Hydraulic Master extracts CleanWater from room humidity
        /// (negates need for rainfall catchment).
        /// </summary>
        private void ExtractHumidityWater(float gameHours, Shelter.Shelter shelter, WaterStorage storage)
        {
            if (_personalQuests == null || shelter?.Rooms == null || storage == null) return;
            var survivors = _getSurvivors?.Invoke();
            if (!_personalQuests.AnyHydraulicMaster(survivors)) return;

            float humidity = 0f;
            int n = 0;
            for (int i = 0; i < shelter.Rooms.Count; i++)
            {
                var r = shelter.Rooms[i];
                if (r == null) continue;
                humidity += r.Humidity;
                n++;
            }
            if (n <= 0) return;
            humidity /= n;
            if (humidity < 0.2f) return;

            float extracted = PersonalQuestSystem.HumidityWaterExtractPerHour * gameHours * humidity;
            if (extracted > 0f)
                storage.AddClean(extracted);
        }

        /// <summary>Prompt #299 — Prom Queen's makeup/hygiene ritual burns clean water daily.</summary>
        private void ApplyMakeupWaterBurn(float gameHours, WaterStorage storage)
        {
            if (_personalQuests == null || storage == null) return;
            var survivors = _getSurvivors?.Invoke();
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                float burnPerDay = _personalQuests.GetMakeupCleanWaterBurn(sv);
                if (burnPerDay <= 0f) continue;
                float burn = burnPerDay * (gameHours / 24f);
                if (burn > 0f) storage.ConsumeClean(burn);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public WaterEconomySystemSave CaptureState() => new WaterEconomySystemSave
        {
            purifierQueueMode = (int)CurrentPurifierQueue
        };

        public void RestoreState(WaterEconomySystemSave saved)
        {
            if (saved == null) return;
            CurrentPurifierQueue = ClampQueueMode((PurifierQueueMode)saved.purifierQueueMode);
            OnPurifierQueueChanged?.Invoke(CurrentPurifierQueue);
            OnWaterStateChanged?.Invoke();
        }

        private float GetEffectiveHoursPerUnit(ShelterModuleInstance purifier)
        {
            var def = purifier != null ? purifier.Definition as WaterPurifierModuleSO : null;
            float hoursPerUnit = def != null && def.ConversionHoursPerUnit > 0f
                ? def.ConversionHoursPerUnit
                : DefaultConversionHoursPerUnit;
            float speedMult = _personalQuests != null
                ? _personalQuests.GetPurifierSpeedMultiplier(_getSurvivors?.Invoke())
                : 1f;
            float perkAdjustedHours = speedMult > 1f
                ? Mathf.Max(0.01f, hoursPerUnit / speedMult)
                : hoursPerUnit;
            float staffingMultiplier = _getPurifierHoursPerUnitMultiplier != null
                ? _getPurifierHoursPerUnitMultiplier()
                : 1f;
            return Mathf.Max(0.01f, perkAdjustedHours * Mathf.Clamp(staffingMultiplier, 0.01f, 1f));
        }

        private static float GetFilterDegradationPerUnit(ShelterModuleInstance purifier)
        {
            var definition = purifier != null ? purifier.Definition as WaterPurifierModuleSO : null;
            return definition != null && definition.FilterDegradationPerUnitConverted > 0f
                ? definition.FilterDegradationPerUnitConverted
                : DefaultFilterDegradationPerUnitConverted;
        }

        private ConversionSource SelectConversionSource(WaterStorage storage)
        {
            if (storage == null) return ConversionSource.None;
            bool dirtyFirst = CurrentPurifierQueue == PurifierQueueMode.DirtyFirst;
            if (dirtyFirst && storage.DirtyWater > 0f) return ConversionSource.Dirty;
            if (storage.IrradiatedWater > 0f) return ConversionSource.Irradiated;
            if (storage.DirtyWater > 0f) return ConversionSource.Dirty;
            return ConversionSource.None;
        }

        private static int UnitsForSource(WaterStorage storage, ConversionSource source)
        {
            if (storage == null) return 0;
            switch (source)
            {
                case ConversionSource.Irradiated: return Mathf.FloorToInt(Mathf.Max(0f, storage.IrradiatedWater));
                case ConversionSource.Dirty: return Mathf.FloorToInt(Mathf.Max(0f, storage.DirtyWater));
                default: return 0;
            }
        }

        private static string SourceLabel(ConversionSource source)
        {
            switch (source)
            {
                case ConversionSource.Irradiated: return "IRRADIATED";
                case ConversionSource.Dirty: return "DIRTY";
                default: return "NONE";
            }
        }

        private static string OutputLabel(ConversionSource source)
        {
            switch (source)
            {
                case ConversionSource.Irradiated: return "DIRTY";
                case ConversionSource.Dirty: return "CLEAN";
                default: return "--";
            }
        }

        private static PurifierQueueMode ClampQueueMode(PurifierQueueMode queueMode)
        {
            return (PurifierQueueMode)Mathf.Clamp((int)queueMode,
                (int)PurifierQueueMode.Auto, (int)PurifierQueueMode.DirtyFirst);
        }

        private static WaterStateStamp CaptureStateStamp(Shelter.Shelter shelter, WaterStorage storage)
        {
            var purifier = shelter != null ? shelter.GetModule(PurifierModuleId) : null;
            return new WaterStateStamp
            {
                Clean = storage != null ? storage.CleanWater : 0f,
                Dirty = storage != null ? storage.DirtyWater : 0f,
                Irradiated = storage != null ? storage.IrradiatedWater : 0f,
                FilterHealth = purifier != null ? purifier.FilterHealth : 0f,
                Progress = purifier != null ? purifier.WaterConversionProgress : 0f
            };
        }

        private static bool HasStateChanged(WaterStateStamp before, WaterStateStamp after)
        {
            return !Mathf.Approximately(before.Clean, after.Clean)
                || !Mathf.Approximately(before.Dirty, after.Dirty)
                || !Mathf.Approximately(before.Irradiated, after.Irradiated)
                || !Mathf.Approximately(before.FilterHealth, after.FilterHealth)
                || !Mathf.Approximately(before.Progress, after.Progress);
        }

        private enum ConversionSource
        {
            None,
            Irradiated,
            Dirty
        }

        private struct WaterStateStamp
        {
            public float Clean;
            public float Dirty;
            public float Irradiated;
            public float FilterHealth;
            public float Progress;
        }

}
}
