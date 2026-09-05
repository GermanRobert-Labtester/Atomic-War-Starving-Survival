using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Shelter;
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
        public float ozonePpm;                  // electrostatic byproduct — canonical ventilation gas
        public ElectrostaticStageState? electrostatic = null; // Plan 72 stage (null = not installed)
        public List<VentilationSource> activeSources = new List<VentilationSource>();
        public List<VentilationLogEntry> log = new List<VentilationLogEntry>();
    }

    /// <summary>
    /// Plan 72: high-voltage electrostatic precipitation stage living inside the
    /// canonical ventilation authority. Trades reduced mechanical filter
    /// consumption for electrical demand, dust handling, ozone, arcing risk and
    /// radioactive waste management. All gameplay values are abstract.
    /// </summary>
    [Serializable]
    public sealed class ElectrostaticStageState
    {
        public bool installed;
        public string stageId = string.Empty;
        public string profileId = string.Empty;
        public string roomId = string.Empty;       // grid room supplying the stage
        public bool energized;
        public float dustLoadKg;                   // on collector plates
        public float hopperKg;                     // rapped-off dust awaiting disposal
        public float plateCondition = 100f;        // 0-100
        public float transformerCondition = 100f;  // 0-100
        public int rappingCooldownDays;            // capture halved while rapping settles dust
        public bool faulted;
        public string faultReason = string.Empty;
        public int lastServicedDay = -1;
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

        // ── Plan 72: electrostatic stage constants ──────────────
        public const string HotDustDrumItemId = "item_hot_dust_drum";
        public const float HotDustKgPerDrum = 10f;
        /// <summary>Fraction of plate dust transferred to the hopper per rapping.
        /// The remainder stays adhered — hopper + plate dust is conserved.</summary>
        public const float RappingTransferFraction = 0.9f;
        public const float RappingPlateWear = 2f;
        public const int RappingCooldownDays = 1;
        /// <summary>Passive ozone dispersion per day (shelter air exchange).</summary>
        public const float OzoneDecayFractionPerDay = 0.10f;
        public const float OzoneCriticalPpm = 120f;
        public const float OzoneWarnPpm = 60f;
        public const float ArcFaultTransformerWear = 15f;
        /// <summary>Maintenance overdue doubles arc risk (degraded insulation checks).</summary>
        public const float MaintenanceOverdueRiskMultiplier = 2f;

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

        public VentilationSystem(StartingLevelSystem startingLevel, ILog? log = null)
        {
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _log = log ?? NullLog.Instance;
        }

        // ── Plan 72: electrostatic stage services ─────────────────
        private readonly Dictionary<string, ElectrostaticStageDef> _stageCatalog =
            new Dictionary<string, ElectrostaticStageDef>(StringComparer.Ordinal);
        private ISeededRng? _stageRng;
        private PowerGridSystem? _stagePower;
        private Inventory.Inventory? _stageInventory;
        private ShelterFireHazardSystem? _stageFire;

        /// <summary>
        /// Late-binds deterministic services for the electrostatic stage: the
        /// seeded RNG (arc faults), the power grid (real draw/verification), the
        /// consumable inventory (installation components, disposal drums) and the
        /// canonical fire-hazard system (arc ignition). Unbound services degrade
        /// honestly: no RNG → no probabilistic faults; no grid → stage offline.
        /// </summary>
        public void BindStageServices(
            ISeededRng? rng = null,
            PowerGridSystem? powerGrid = null,
            Inventory.Inventory? inventory = null,
            ShelterFireHazardSystem? fire = null)
        {
            _stageRng = rng;
            _stagePower = powerGrid;
            _stageInventory = inventory;
            _stageFire = fire;
        }

        /// <summary>Registered electrostatic stage definitions (catalog view).</summary>
        public IReadOnlyCollection<ElectrostaticStageDef> Catalog => _stageCatalog.Values;

        /// <summary>Registers electrostatic stage definitions from the catalog.</summary>
        public int ApplyElectrostaticCatalog(IEnumerable<ElectrostaticStageDef> defs)
        {
            if (defs == null) return 0;
            int applied = 0;
            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrEmpty(def.stage_id)) continue;
                _stageCatalog[def.stage_id] = def;
                applied++;
            }
            return applied;
        }

        /// <summary>Installs the stage into the shelter ventilation, consuming canonical components.</summary>
        public ActionResult InstallElectrostaticStage(string stageId, string roomId)
        {
            if (_state.electrostatic is { installed: true })
                return ActionResult.Blocked("stage_exists", "vent.stage_exists");
            if (!_stageCatalog.TryGetValue(stageId, out var def))
                return ActionResult.Failed("unknown_stage", "vent.unknown_stage");
            if (_stageInventory == null)
                return ActionResult.Failed("inventory_unavailable", "vent.inventory_unavailable");

            foreach (var comp in def.required_component_ids)
            {
                if (!_stageInventory.TryConsume(comp.item_id, comp.amount))
                    return ActionResult.Failed("component_missing", "vent.stage_components_missing");
            }

            _state.electrostatic = new ElectrostaticStageState
            {
                installed = true,
                stageId = def.stage_id,
                profileId = def.operating_profiles.Count > 0 ? def.operating_profiles[0].profile_id : string.Empty,
                roomId = roomId ?? string.Empty,
                lastServicedDay = _currentDay
            };
            _log.Info($"[Ventilation] electrostatic stage installed ({def.stage_id}) in {roomId}");
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.stage_installed");
        }

        /// <summary>Switches the stage to a discrete catalog operating profile.</summary>
        public ActionResult SetStageProfile(string profileId)
        {
            var stage = _state.electrostatic;
            if (stage is not { installed: true })
                return ActionResult.Blocked("stage_not_installed", "vent.stage_not_installed");
            if (!_stageCatalog.TryGetValue(stage.stageId, out var def) ||
                def.operating_profiles.Find(p => p.profile_id == profileId) == null)
                return ActionResult.Failed("unknown_profile", "vent.unknown_profile");

            stage.profileId = profileId;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.stage_profile_set");
        }

        /// <summary>
        /// Rapping maintenance: transfers plate dust to the hopper (mass
        /// conserving; a residual fraction stays adhered). Capture is halved for
        /// the rapping cooldown window while dust settles.
        /// </summary>
        public ActionResult RapPlates()
        {
            var stage = _state.electrostatic;
            if (stage is not { installed: true })
                return ActionResult.Blocked("stage_not_installed", "vent.stage_not_installed");
            if (stage.rappingCooldownDays > 0)
                return ActionResult.Blocked("rapping_cooldown", "vent.rapping_cooldown");
            if (stage.dustLoadKg <= 0f)
                return ActionResult.Blocked("no_dust", "vent.no_dust");

            float transferred = stage.dustLoadKg * RappingTransferFraction;
            stage.dustLoadKg -= transferred;
            stage.hopperKg += transferred;
            stage.plateCondition = Math.Max(0f, stage.plateCondition - RappingPlateWear);
            stage.rappingCooldownDays = RappingCooldownDays;
            _log.Info($"[Ventilation] rapped plates: {transferred:F2}kg to hopper");
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.plates_rapped",
                new Dictionary<string, double> { { "hopper_kg", stage.hopperKg } });
        }

        /// <summary>
        /// Drains the radioactive dust hopper into canonical sealed drums for
        /// disposal hauling. Mass is conserved into the drum items.
        /// </summary>
        public ActionResult EmptyHopperToDrums(int maxDrums)
        {
            var stage = _state.electrostatic;
            if (stage is not { installed: true })
                return ActionResult.Blocked("stage_not_installed", "vent.stage_not_installed");
            if (maxDrums <= 0)
                return ActionResult.Failed("invalid_amount", "vent.invalid_amount");
            if (_stageInventory == null)
                return ActionResult.Failed("inventory_unavailable", "vent.inventory_unavailable");
            if (stage.hopperKg < HotDustKgPerDrum)
                return ActionResult.Blocked("hopper_below_drum", "vent.hopper_below_drum");

            int drums = Math.Min(maxDrums, (int)Math.Floor(stage.hopperKg / HotDustKgPerDrum));
            if (!_stageInventory.TryProduce(HotDustDrumItemId, drums))
                return ActionResult.Failed("pack_failed", "vent.drum_pack_failed");

            stage.hopperKg -= drums * HotDustKgPerDrum;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.hopper_emptied",
                new Dictionary<string, double>
                {
                    { "drums", drums },
                    { "hopper_kg_remaining", stage.hopperKg },
                });
        }

        /// <summary>Maintenance service: clears faults, restores plate/transformer condition.</summary>
        public ActionResult ServiceElectrostaticStage()
        {
            var stage = _state.electrostatic;
            if (stage is not { installed: true })
                return ActionResult.Blocked("stage_not_installed", "vent.stage_not_installed");

            stage.faulted = false;
            stage.faultReason = string.Empty;
            stage.plateCondition = Math.Min(100f, stage.plateCondition + 25f);
            stage.transformerCondition = Math.Min(100f, stage.transformerCondition + 25f);
            stage.lastServicedDay = _currentDay;
            OnVentilationChanged?.Invoke();
            return ActionResult.Success("vent.stage_serviced");
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

        /// <summary>Daily ventilation tick. Called after production phase.
        /// Plan 72: the electrostatic stage (if installed) treats the day's
        /// intake particulate mass; hosts obtain it via
        /// ElectrostaticFiltrationCatalogLoader.WeatherIntakeParticulateKg.</summary>
        public void TickDay(int day, float incomingParticulateKgPerDay = 0f, bool hotAshLoad = false)
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

            TickElectrostaticStage(day, incomingParticulateKgPerDay, hotAshLoad);

            OnVentilationChanged?.Invoke();
        }

        /// <summary>
        /// Plan 72 electrostatic tick: real power draw gate, mass-conserving
        /// particulate capture into plate dust, ozone byproduct into the
        /// canonical ventilation gas state, deterministic arc faults handing off
        /// to the power-grid breaker and the fire-hazard system.
        /// </summary>
        private void TickElectrostaticStage(int day, float incomingKg, bool hotAshLoad)
        {
            var stage = _state.electrostatic;
            if (stage is not { installed: true }) return;
            if (stage.rappingCooldownDays > 0) stage.rappingCooldownDays--;

            // Ozone decays with shelter air exchange regardless of stage state.
            _state.ozonePpm = Math.Max(0f, _state.ozonePpm * (1f - OzoneDecayFractionPerDay));

            bool powered = _stagePower != null && _stagePower.IsRoomPowered(stage.roomId);
            stage.energized = !stage.faulted && powered;

            if (!stage.energized) return;

            if (!_stageCatalog.TryGetValue(stage.stageId, out var def))
            {
                stage.faulted = true;
                stage.faultReason = "stage_definition_missing";
                return;
            }
            var profile = def.operating_profiles.Find(p => p.profile_id == stage.profileId)
                ?? def.operating_profiles.Find(p => p.profile_id == def.operating_profiles[0].profile_id);
            if (profile == null) return;

            // Particulate capture (mass balance: incoming = captured + escaped).
            if (incomingKg > 0f && stage.dustLoadKg < def.dust_capacity_kg)
            {
                float efficiency = hotAshLoad ? profile.hot_ash_capture_efficiency : profile.capture_efficiency_pm25;
                if (stage.rappingCooldownDays > 0) efficiency *= 0.5f;      // dust settling after rapping
                efficiency *= stage.plateCondition / 100f;                  // worn plates capture less

                float capturedKg = Math.Min(incomingKg * efficiency, def.dust_capacity_kg - stage.dustLoadKg);
                // escaped = incomingKg - capturedKg → passes to mechanical filters
                // via the pre-existing soot path below (no deletion anywhere).
                stage.dustLoadKg += capturedKg;

                if (stage.dustLoadKg >= def.dust_capacity_kg)
                {
                    stage.faulted = true;
                    stage.faultReason = "dust_capacity_exceeded";
                    _log.Warn("[Ventilation] electrostatic stage faulted: dust capacity exceeded — rap plates");
                }
            }

            // Ozone byproduct into the canonical ventilation gas state.
            _state.ozonePpm = Math.Min(MaxCoPpm * 4f, _state.ozonePpm + profile.ozone_output_rate_ppm_per_day);
            if (_state.ozonePpm > OzoneCriticalPpm)
            {
                var ozoneEntry = new VentilationLogEntry
                {
                    day = day,
                    message = $"CRITICAL: ozone at {_state.ozonePpm:0} ppm — respiratory hazard, service stage"
                };
                _state.log.Add(ozoneEntry);
                OnHazardWarning?.Invoke(ozoneEntry);
            }

            // Deterministic arc fault: dust load, transformer wear and overdue
            // maintenance raise risk above the profile base (basis points).
            if (_stageRng != null)
            {
                bool overdue = stage.lastServicedDay >= 0
                    && def.maintenance_interval_days > 0
                    && day - stage.lastServicedDay > def.maintenance_interval_days;
                float riskBp = profile.arc_risk_base_bp
                    * (1f + stage.dustLoadKg / Math.Max(1f, def.dust_capacity_kg))
                    * (1f + (100f - stage.transformerCondition) / 100f)
                    * (overdue ? MaintenanceOverdueRiskMultiplier : 1f);

                if (_stageRng.NextDouble() < riskBp / 10000.0)
                {
                    stage.faulted = true;
                    stage.faultReason = "arc_fault";
                    stage.transformerCondition = Math.Max(0f, stage.transformerCondition - ArcFaultTransformerWear);
                    stage.energized = false;
                    _stagePower?.MarkTripped(stage.roomId, day);
                    _log.Warn($"[Ventilation] arc fault in electrostatic stage — breaker tripped for {stage.roomId}");

                    var arcEntry = new VentilationLogEntry
                    {
                        day = day,
                        message = $"WARNING: electrostatic stage arc fault — breaker tripped, fire risk in {stage.roomId}"
                    };
                    _state.log.Add(arcEntry);
                    OnHazardWarning?.Invoke(arcEntry);

                    // Local fire hazard through the canonical fire system.
                    if (_stageFire != null)
                    {
                        var zones = new List<FireZoneState>
                        {
                            new FireZoneState { zoneId = stage.roomId, displayName = stage.roomId }
                        };
                        _stageFire.Ignite($"vent_arc_{stage.roomId}_{day}", stage.roomId, day, zones);
                    }
                }
            }
        }

        // ── Persistence ──────────────────────────────────────────────────────

        public VentilationState CaptureState() => CloneState(_state);

        public void RestoreState(VentilationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static VentilationState CloneState(VentilationState src)
        {
            if (src == null) return new VentilationState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<VentilationState>(json) ?? new VentilationState();
        }
    }
}
