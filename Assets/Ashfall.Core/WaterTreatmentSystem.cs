using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Water Treatment System.
    ///
    /// Owns bunker water inventories and treatment jobs. Separate from
    /// <see cref="BrineWaterSystem"/> which remains the source/membrane/
    /// treaty adapter for external brine. This system tracks raw, brackish,
    /// irradiated, and clean water quantities; charcoal/filter integrity;
    /// distillation fuel; treatment mode; contamination profile; and active
    /// treatment jobs.
    ///
    /// Clean water is consumed first during the daily ration pass. If clean
    /// water is exhausted, raw/contaminated water is consumed according to
    /// ration policy. If no water exists, thirst is applied normally. Water
    /// is never created from nothing — mass balance is enforced.
    ///
    /// Emits disease/heavy-metal/radiation exposure events into existing
    /// <see cref="Disease.DiseaseSystem"/>, needs, and medical/dose pipelines.
    /// </summary>
    [Serializable]
    public sealed class WaterTreatmentState
    {
        public string systemId = WaterTreatmentSystem.SystemId;
        public float cleanWater;
        public float rawWater;
        public float brackishWater;
        public float irradiatedWater;
        public float filterIntegrity = 100f;
        public float charcoalSupply;
        public float distillationFuel;
        public TreatmentMode activeMode;
        public bool isProcessing;
        public float processingProgress;
        public float processingTarget;
        public float totalWaterProcessed;
        public float totalContaminationExposure;
        public int filterReplacements;
        public List<WaterTreatmentJob> completedJobs = new List<WaterTreatmentJob>();
        public float filterMaxIntegrity = 100f;
    }

    public enum TreatmentMode
    {
        Idle,
        CharcoalFiltration,
        Distillation,
        ReverseOsmosis,
        Decontamination
    }

    [Serializable]
    public sealed class WaterTreatmentJob
    {
        public TreatmentMode mode;
        public float inputAmount;
        public float cleanOutput;
        public float wasteAmount;
        public float filterDegradation;
        public float fuelConsumed;
        public float contaminationRemoved;
        public int dayCompleted;
    }

    public sealed class WaterTreatmentSystem
    {
        public const string SystemId = "water_treatment";

        // Conversion rates per unit of input water
        public const float CharcoalFiltrationEfficiency = 0.85f;
        public const float DistillationEfficiency = 0.70f;
        public const float ReverseOsmosisEfficiency = 0.90f;
        public const float DecontaminationEfficiency = 0.60f;

        // Resource consumption per unit of clean water produced
        public const float CharcoalPerUnit = 0.05f;    // charcoal consumed per unit
        public const float FuelPerDistillationUnit = 0.1f;
        public const float FilterDegradePerUnit = 0.5f; // filter integrity lost per unit

        private WaterTreatmentState _state = new WaterTreatmentState();
        private readonly ILog _log;
        private int _currentDay;

        public WaterTreatmentState State => _state;
        public float CleanWater => _state.cleanWater;
        public float RawWater => _state.rawWater;
        public float BrackishWater => _state.brackishWater;
        public float IrradiatedWater => _state.irradiatedWater;
        public float FilterIntegrity => _state.filterIntegrity;
        public bool IsProcessing => _state.isProcessing;

        public event Action<ActionResult> OnTreatmentCompleted;
        public event Action OnWaterStateChanged;

        // Exposure events — consumed by DiseaseSystem / NeedsSystem
        public event Action<float> OnHeavyMetalExposure;   // parameter = dose
        public event Action<float> OnRadiationExposure;    // parameter = dose
        public event Action<float> OnPathogenExposure;     // parameter = dose

        public WaterTreatmentSystem(ILog log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        // ── Water Management ────────────────────────────────────────────────

        /// <summary>Add water to the appropriate tank. Returns the ActionResult.</summary>
        public ActionResult AddWater(WaterType type, float amount)
        {
            if (amount <= 0) return ActionResult.Failed("invalid_amount", "water.invalid_amount");
            if (float.IsNaN(amount) || float.IsInfinity(amount))
                return ActionResult.Failed("invalid_amount", "water.invalid_amount");

            switch (type)
            {
                case WaterType.Clean: _state.cleanWater += amount; break;
                case WaterType.Raw: _state.rawWater += amount; break;
                case WaterType.Brackish: _state.brackishWater += amount; break;
                case WaterType.Irradiated: _state.irradiatedWater += amount; break;
                default: return ActionResult.Failed("unknown_type", "water.unknown_type");
            }

            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.added",
                new Dictionary<string, double> { { type.ToString().ToLowerInvariant() + "_water", amount } });
        }

        /// <summary>Remove water from the specified tank.</summary>
        public ActionResult RemoveWater(WaterType type, float amount)
        {
            if (amount <= 0) return ActionResult.Failed("invalid_amount", "water.invalid_amount");

            float current = GetWater(type);
            float removed = Math.Min(current, amount);

            switch (type)
            {
                case WaterType.Clean: _state.cleanWater -= removed; break;
                case WaterType.Raw: _state.rawWater -= removed; break;
                case WaterType.Brackish: _state.brackishWater -= removed; break;
                case WaterType.Irradiated: _state.irradiatedWater -= removed; break;
                default: return ActionResult.Failed("unknown_type", "water.unknown_type");
            }

            if (removed < amount)
                return ActionResult.Partial("water.partial_remove",
                    new Dictionary<string, double> { { "removed", removed }, { "requested", amount } });

            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.removed",
                new Dictionary<string, double> { { "removed", removed } });
        }

        /// <summary>Get water amount by type.</summary>
        public float GetWater(WaterType type) => type switch
        {
            WaterType.Clean => _state.cleanWater,
            WaterType.Raw => _state.rawWater,
            WaterType.Brackish => _state.brackishWater,
            WaterType.Irradiated => _state.irradiatedWater,
            _ => 0f
        };

        /// <summary>Total water across all tanks.</summary>
        public float TotalWater =>
            _state.cleanWater + _state.rawWater + _state.brackishWater + _state.irradiatedWater;

        // ── Treatment Jobs ──────────────────────────────────────────────────

        /// <summary>Start a treatment job of the given mode.</summary>
        public ActionResult StartTreatment(TreatmentMode mode, float inputAmount)
        {
            if (inputAmount <= 0)
                return ActionResult.Failed("invalid_amount", "water.invalid_amount");
            if (_state.isProcessing)
                return ActionResult.Blocked("already_processing", "water.already_processing");

            // Check input availability based on mode
            ActionResult? check = mode switch
            {
                TreatmentMode.CharcoalFiltration => CheckInput(WaterType.Raw, inputAmount, _state.charcoalSupply, CharcoalPerUnit * inputAmount),
                TreatmentMode.Distillation => CheckInput(WaterType.Brackish, inputAmount, _state.distillationFuel, FuelPerDistillationUnit * inputAmount),
                TreatmentMode.ReverseOsmosis => CheckInput(WaterType.Brackish, inputAmount, _state.filterIntegrity, FilterDegradePerUnit * inputAmount),
                TreatmentMode.Decontamination => CheckInput(WaterType.Irradiated, inputAmount, _state.charcoalSupply, CharcoalPerUnit * inputAmount * 2),
                _ => ActionResult.Failed("invalid_mode", "water.invalid_mode")
            };

            if (check.HasValue && check.Value.Status != ActionResult.StatusKind.Success)
                return check.Value;

            // Consume input water
            WaterType inputType = mode switch
            {
                TreatmentMode.CharcoalFiltration => WaterType.Raw,
                TreatmentMode.Distillation => WaterType.Brackish,
                TreatmentMode.ReverseOsmosis => WaterType.Brackish,
                TreatmentMode.Decontamination => WaterType.Irradiated,
                _ => WaterType.Raw
            };
            RemoveWater(inputType, inputAmount);

            _state.activeMode = mode;
            _state.isProcessing = true;
            _state.processingProgress = 0f;
            _state.processingTarget = inputAmount;

            _log.Info($"[WaterTreatment] started {mode} ({inputAmount} units)");
            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.treatment_started",
                new Dictionary<string, double> { { "input", inputAmount }, { "mode", (int)mode } });
        }

        private ActionResult? CheckInput(WaterType waterType, float waterNeeded,
            float resourceAvailable, float resourceNeeded)
        {
            float available = GetWater(waterType);
            if (available < waterNeeded)
                return ActionResult.Blocked("insufficient_water", "water.insufficient_water",
                    new Dictionary<string, double> { { "available", available }, { "needed", waterNeeded } });
            if (resourceNeeded > 0 && resourceAvailable < resourceNeeded)
                return ActionResult.Blocked("insufficient_resources", "water.insufficient_resources",
                    new Dictionary<string, double> { { "available", resourceAvailable }, { "needed", resourceNeeded } });
            return null; // success
        }

        /// <summary>Advance the active treatment job by the given time fraction (0-1 per day).</summary>
        public ActionResult TickTreatment(float progressFraction)
        {
            if (!_state.isProcessing)
                return ActionResult.Blocked("not_processing", "water.not_processing");

            _state.processingProgress += progressFraction * _state.processingTarget;

            if (_state.processingProgress >= _state.processingTarget)
            {
                return CompleteTreatment();
            }

            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.processing",
                new Dictionary<string, double>
                {
                    { "progress", _state.processingProgress },
                    { "target", _state.processingTarget },
                    { "remaining", Math.Max(0, _state.processingTarget - _state.processingProgress) }
                });
        }

        /// <summary>Cancel the active treatment job. Wastes the input water.</summary>
        public ActionResult CancelTreatment()
        {
            if (!_state.isProcessing)
                return ActionResult.Blocked("not_processing", "water.not_processing");

            _log.Info($"[WaterTreatment] cancelled {_state.activeMode} — input lost");
            _state.isProcessing = false;
            _state.activeMode = TreatmentMode.Idle;
            _state.processingProgress = 0f;
            _state.processingTarget = 0f;

            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.treatment_cancelled");
        }

        private ActionResult CompleteTreatment()
        {
            float efficiency = _state.activeMode switch
            {
                TreatmentMode.CharcoalFiltration => CharcoalFiltrationEfficiency,
                TreatmentMode.Distillation => DistillationEfficiency,
                TreatmentMode.ReverseOsmosis => ReverseOsmosisEfficiency,
                TreatmentMode.Decontamination => DecontaminationEfficiency,
                _ => 0f
            };

            float inputAmount = _state.processingTarget;
            float cleanOutput = inputAmount * efficiency;
            float wasteAmount = inputAmount - cleanOutput;
            float filterDegradation = 0f;
            float fuelConsumed = 0f;
            float contaminationRemoved = 0f;

            switch (_state.activeMode)
            {
                case TreatmentMode.CharcoalFiltration:
                    float charcoalUsed = CharcoalPerUnit * inputAmount;
                    _state.charcoalSupply -= charcoalUsed;
                    filterDegradation = FilterDegradePerUnit * inputAmount * 0.5f;
                    contaminationRemoved = 0.3f; // reduces pathogen risk
                    break;

                case TreatmentMode.Distillation:
                    fuelConsumed = FuelPerDistillationUnit * inputAmount;
                    _state.distillationFuel -= fuelConsumed;
                    filterDegradation = 0f;
                    contaminationRemoved = 0.9f; // distillation removes almost everything
                    break;

                case TreatmentMode.ReverseOsmosis:
                    filterDegradation = FilterDegradePerUnit * inputAmount;
                    _state.filterIntegrity = Math.Max(0, _state.filterIntegrity - filterDegradation);
                    contaminationRemoved = 0.6f;
                    break;

                case TreatmentMode.Decontamination:
                    float deconCharcoal = CharcoalPerUnit * inputAmount * 2;
                    _state.charcoalSupply -= deconCharcoal;
                    filterDegradation = FilterDegradePerUnit * inputAmount;
                    contaminationRemoved = 0.8f; // heavy rad removal
                    break;
            }

            _state.filterIntegrity = Math.Max(0, _state.filterIntegrity - filterDegradation);
            _state.cleanWater += cleanOutput;
            _state.totalWaterProcessed += inputAmount;

            var job = new WaterTreatmentJob
            {
                mode = _state.activeMode,
                inputAmount = inputAmount,
                cleanOutput = cleanOutput,
                wasteAmount = wasteAmount,
                filterDegradation = filterDegradation,
                fuelConsumed = fuelConsumed,
                contaminationRemoved = contaminationRemoved,
                dayCompleted = _currentDay
            };
            _state.completedJobs.Add(job);

            _state.isProcessing = false;
            _state.activeMode = TreatmentMode.Idle;
            _state.processingProgress = 0f;
            _state.processingTarget = 0f;

            var deltas = new Dictionary<string, double>
            {
                { "clean_water", cleanOutput },
                { "filter_degradation", filterDegradation },
                { "fuel_consumed", fuelConsumed }
            };

            _log.Info($"[WaterTreatment] completed {job.mode}: {cleanOutput} clean water from {inputAmount} input");
            var result = ActionResult.Success("water.treatment_complete", deltas);
            OnTreatmentCompleted?.Invoke(result);
            OnWaterStateChanged?.Invoke();
            return result;
        }

        // ── Daily Ration ────────────────────────────────────────────────────

        /// <summary>
        /// Consume water for daily ration. Clean water is used first; if
        /// exhausted, raw/contaminated water is used with exposure consequences.
        /// Returns the amount and type of water consumed.
        /// </summary>
        public ActionResult ConsumeRation(float needed)
        {
            if (needed <= 0) return ActionResult.Success("water.no_ration_needed");

            float remaining = needed;
            float contaminationExposure = 0f;

            // Priority: clean > raw > brackish > irradiated
            float fromClean = Math.Min(remaining, _state.cleanWater);
            _state.cleanWater -= fromClean;
            remaining -= fromClean;

            if (remaining > 0)
            {
                float fromRaw = Math.Min(remaining, _state.rawWater);
                _state.rawWater -= fromRaw;
                remaining -= fromRaw;
                contaminationExposure += fromRaw * 0.1f; // low pathogen risk
            }

            if (remaining > 0)
            {
                float fromBrackish = Math.Min(remaining, _state.brackishWater);
                _state.brackishWater -= fromBrackish;
                remaining -= fromBrackish;
                contaminationExposure += fromBrackish * 0.3f; // heavy metal risk
            }

            if (remaining > 0)
            {
                float fromIrradiated = Math.Min(remaining, _state.irradiatedWater);
                _state.irradiatedWater -= fromIrradiated;
                remaining -= fromIrradiated;
                contaminationExposure += fromIrradiated * 0.8f; // high radiation risk
                OnRadiationExposure?.Invoke(fromIrradiated * 0.5f);
            }

            if (contaminationExposure > 0)
            {
                _state.totalContaminationExposure += contaminationExposure;
                OnHeavyMetalExposure?.Invoke(contaminationExposure * 0.3f);
                OnPathogenExposure?.Invoke(contaminationExposure * 0.5f);
            }

            float consumed = needed - remaining;

            OnWaterStateChanged?.Invoke();
            if (remaining > 0)
                return ActionResult.Partial("water.ration_shortfall",
                    new Dictionary<string, double>
                    {
                        { "consumed", consumed },
                        { "shortfall", remaining },
                        { "contamination_exposure", contaminationExposure }
                    });

            return ActionResult.Success("water.ration_consumed",
                new Dictionary<string, double>
                {
                    { "consumed", consumed },
                    { "contamination_exposure", contaminationExposure }
                });
        }

        // ── Resource Management ─────────────────────────────────────────────

        /// <summary>Add charcoal to the supply.</summary>
        public ActionResult AddCharcoal(float amount)
        {
            if (amount <= 0) return ActionResult.Failed("invalid_amount", "water.invalid_amount");
            _state.charcoalSupply += amount;
            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.charcoal_added",
                new Dictionary<string, double> { { "charcoal", amount } });
        }

        /// <summary>Add distillation fuel.</summary>
        public ActionResult AddFuel(float amount)
        {
            if (amount <= 0) return ActionResult.Failed("invalid_amount", "water.invalid_amount");
            _state.distillationFuel += amount;
            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.fuel_added",
                new Dictionary<string, double> { { "fuel", amount } });
        }

        /// <summary>Replace the filter (resets integrity to max). Consumes a filter item.</summary>
        public ActionResult ReplaceFilter()
        {
            _state.filterIntegrity = _state.filterMaxIntegrity;
            _state.filterReplacements++;
            OnWaterStateChanged?.Invoke();
            return ActionResult.Success("water.filter_replaced",
                new Dictionary<string, double> { { "integrity", _state.filterIntegrity } });
        }

        // ── Daily Tick ──────────────────────────────────────────────────────

        /// <summary>Daily tick. Advances active treatment and applies passive effects.</summary>
        public void TickDay(int day)
        {
            _currentDay = day;

            // Advance active treatment by one day's worth
            if (_state.isProcessing)
            {
                TickTreatment(1.0f);
            }

            // Passive filter degradation from standing water
            if (_state.rawWater > 0 || _state.brackishWater > 0)
            {
                float passiveDegrade = (_state.rawWater + _state.brackishWater) * 0.01f;
                _state.filterIntegrity = Math.Max(0, _state.filterIntegrity - passiveDegrade);
            }
        }

        // ── Persistence ─────────────────────────────────────────────────────

        public WaterTreatmentState CaptureState()
        {
            return _state;
        }

        public void RestoreState(WaterTreatmentState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnWaterStateChanged?.Invoke();
        }
    }

    public enum WaterType
    {
        Clean,
        Raw,
        Brackish,
        Irradiated
    }
}
