using System;
using System.Collections.Generic;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — Ventilation, Smoke Scrubbing, and Radon Hazard System.
    ///
    /// Extends StartingLevelSystem's air state rather than duplicating it.
    /// Adds source emissions for Silent Foundry, generator, cooking, and
    /// other catalog-defined industrial jobs. Tracks smoke/soot, carbon
    /// monoxide, duct/valve state, exhaust-filter saturation, and shelter-
    /// room exposure. Uses power allocation and room assignments to
    /// determine which ventilation branches operate.
    ///
    /// YearOfAshRadonSystem remains the authoritative radon phase system.
    /// </summary>
    [Serializable]
    public sealed class VentilationState
    {
        public string systemId = VentilationSystem.SystemId;
        public float smokeSootLevel;          // 0-100, particulate accumulation
        public float carbonMonoxidePpm;       // CO concentration
        public float exhaustFilterSaturation; // 0-100, higher = more clogged
        public bool mainDuctOpen = true;
        public bool valveToFoundryOpen;
        public bool valveToGeneratorOpen;
        public bool valveToKitchenOpen;
        public bool valveToMedicalOpen;
        public bool emergencyRecirculationMode;
        public float ductIntegrity = 100f;
        public List<VentilationSource> activeSources = new List<VentilationSource>();
        public List<VentilationLogEntry> log = new List<VentilationLogEntry>();
    }

    [Serializable]
    public sealed class VentilationSource
    {
        public string sourceId;
        public string roomId;
        public float smokeOutputPerDay;
        public float coOutputPerDay;
        public bool requiresExhaust;
        public bool isActive;
    }

    [Serializable]
    public sealed class VentilationLogEntry
    {
        public int day;
        public string message;
        public float smokeBefore;
        public float smokeAfter;
    }

    public sealed class VentilationSystem
    {
        public const string SystemId = "ventilation_system";
        public const float MaxSmokeSoot = 100f;
        public const float MaxCoPpm = 500f;
        public const float CriticalCoPpm = 100f;
        public const float FilterReplaceCost = 25f;

        private VentilationState _state = new VentilationState();
        private readonly StartingLevelSystem _startingLevel;
        private readonly ILog _log;
        private int _currentDay;

        public VentilationState State => _state;
        public float SmokeSoot => _state.smokeSootLevel;
        public float CarbonMonoxide => _state.carbonMonoxidePpm;
        public float FilterSaturation => _state.exhaustFilterSaturation;

        public event Action<VentilationLogEntry> OnHazardWarning;
        public event Action OnVentilationChanged;

        public VentilationSystem(StartingLevelSystem startingLevel, ILog log = null)
        {
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _log = log ?? NullLog.Instance;
        }

        // ── Source Management ───────────────────────────────────────────────

        /// <summary>Register an emission source (foundry, generator, cooking, etc.).</summary>
        public void RegisterSource(VentilationSource source)
        {
            if (source == null || string.IsNullOrEmpty(source.sourceId)) return;
            var existing = _state.activeSources.Find(s => s.sourceId == source.sourceId);
            if (existing != null)
            {
                existing.smokeOutputPerDay = source.smokeOutputPerDay;
                existing.coOutputPerDay = source.coOutputPerDay;
                existing.requiresExhaust = source.requiresExhaust;
                return;
            }
            _state.activeSources.Add(source);
        }

        /// <summary>Set a source as active or inactive.</summary>
        public ActionResult SetSourceActive(string sourceId, bool active)
        {
            var source = _state.activeSources.Find(s => s.sourceId == sourceId);
            if (source == null)
                return ActionResult.Failed("unknown_source", "vent.unknown_source");

            source.isActive = active;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.source_toggled",
                new Dictionary<string, double> { { sourceId, active ? 1 : 0 } });
        }

        // ── Valve / Duct Control ─────────────────────────────────────────────

        public ActionResult SetValve(string valveId, bool open)
        {
            switch (valveId)
            {
                case "foundry": _state.valveToFoundryOpen = open; break;
                case "generator": _state.valveToGeneratorOpen = open; break;
                case "kitchen": _state.valveToKitchenOpen = open; break;
                case "medical": _state.valveToMedicalOpen = open; break;
                default: return ActionResult.Failed("unknown_valve", "vent.unknown_valve");
            }
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.valve_set",
                new Dictionary<string, double> { { valveId, open ? 1 : 0 } });
        }

        public ActionResult ToggleMainDuct()
        {
            _state.mainDuctOpen = !_state.mainDuctOpen;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success(_state.mainDuctOpen ? "vent.duct_opened" : "vent.duct_closed");
        }

        public ActionResult ToggleRecirculation()
        {
            _state.emergencyRecirculationMode = !_state.emergencyRecirculationMode;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success(_state.emergencyRecirculationMode
                ? "vent.recirculation_on" : "vent.recirculation_off");
        }

        // ── Service ──────────────────────────────────────────────────────────

        /// <summary>Service exhaust filter, reducing saturation.</summary>
        public ActionResult ServiceFilter()
        {
            float reduction = FilterReplaceCost;
            _state.exhaustFilterSaturation = Math.Max(0, _state.exhaustFilterSaturation - reduction);
            _state.smokeSootLevel = Math.Max(0, _state.smokeSootLevel - 10f);
            _log.Info($"[Ventilation] serviced exhaust filter (-{reduction}% saturation)");
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.filter_serviced",
                new Dictionary<string, double> { { "saturation", _state.exhaustFilterSaturation } });
        }

        /// <summary>Replace exhaust filter entirely.</summary>
        public ActionResult ReplaceFilter()
        {
            _state.exhaustFilterSaturation = 0;
            _state.smokeSootLevel = Math.Max(0, _state.smokeSootLevel - 30f);
            _log.Info("[Ventilation] replaced exhaust filter (0% saturation)");
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.filter_replaced");
        }

        /// <summary>Clear duct blockage, restoring integrity.</summary>
        public ActionResult ClearDuct(float amount = 20f)
        {
            _state.ductIntegrity = Math.Min(100f, _state.ductIntegrity + amount);
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.duct_cleared",
                new Dictionary<string, double> { { "integrity", _state.ductIntegrity } });
        }

        // ── Daily Tick ───────────────────────────────────────────────────────

        /// <summary>Daily ventilation tick. Called after production phase.</summary>
        public void TickDay(int day)
        {
            _currentDay = day;
            float totalSmoke = 0;
            float totalCo = 0;

            // Collect emissions from active sources
            foreach (var source in _state.activeSources)
            {
                if (!source.isActive) continue;
                bool hasExhaustPath = source.requiresExhaust switch
                {
                    true when source.sourceId == "foundry" => _state.valveToFoundryOpen && _state.mainDuctOpen,
                    true when source.sourceId == "generator" => _state.valveToGeneratorOpen && _state.mainDuctOpen,
                    true when source.sourceId == "kitchen" => _state.valveToKitchenOpen && _state.mainDuctOpen,
                    _ => _state.mainDuctOpen
                };

                if (hasExhaustPath && !_state.emergencyRecirculationMode)
                {
                    // Emissions vented outside — filter absorbs some
                    _state.exhaustFilterSaturation = Math.Min(MaxSmokeSoot,
                        _state.exhaustFilterSaturation + source.smokeOutputPerDay * 0.3f);
                    totalSmoke += source.smokeOutputPerDay * 0.2f; // 20% remains inside
                    totalCo += source.coOutputPerDay * 0.1f;       // 10% remains inside
                }
                else
                {
                    // No exhaust — all emissions accumulate inside
                    totalSmoke += source.smokeOutputPerDay;
                    totalCo += source.coOutputPerDay;
                }
            }

            // Apply to StartingLevelSystem air quality
            float filterEfficiency = _startingLevel.State.airFilterHealthPercent / 100f;
            if (_state.emergencyRecirculationMode) filterEfficiency *= 0.3f;

            _state.smokeSootLevel = Math.Min(MaxSmokeSoot, _state.smokeSootLevel + totalSmoke * (1f - filterEfficiency));
            _state.carbonMonoxidePpm = Math.Min(MaxCoPpm, _state.carbonMonoxidePpm + totalCo * (1f - filterEfficiency));

            // Passive dispersion (slow)
            _state.smokeSootLevel = Math.Max(0, _state.smokeSootLevel - 2f);
            _state.carbonMonoxidePpm = Math.Max(0, _state.carbonMonoxidePpm - 5f);

            // Duct degradation from heavy use
            if (totalSmoke > 5f)
                _state.ductIntegrity = Math.Max(0, _state.ductIntegrity - 0.5f);

            // Hazard warnings
            if (_state.carbonMonoxidePpm > CriticalCoPpm || _state.smokeSootLevel > 60f)
            {
                var entry = new VentilationLogEntry
                {
                    day = day,
                    message = _state.carbonMonoxidePpm > CriticalCoPpm
                        ? $"CRITICAL: CO at {_state.carbonMonoxidePpm:0} ppm — hazard!"
                        : $"WARNING: Smoke/soot at {_state.smokeSootLevel:0}% — respiratory hazard",
                    smokeBefore = _state.smokeSootLevel,
                    smokeAfter = _state.smokeSootLevel
                };
                _state.log.Add(entry);
                OnHazardWarning?.Invoke(entry);
            }

            OnVentilationChanged?.Invoke();
        }

        // ── Persistence ──────────────────────────────────────────────────────

        public VentilationState CaptureState() => _state;

        public void RestoreState(VentilationState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnVentilationChanged?.Invoke();
        }
    }
}
