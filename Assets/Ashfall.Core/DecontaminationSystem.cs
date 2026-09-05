using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class DecontaminationState
    {
        public string systemId = DecontaminationSystem.SystemId;
        public List<DeconCase> queue = new List<DeconCase>();
        public DeconCase? activeCase;
        public bool shelterContaminated;
        public float shelterContaminationLevel;
        public List<DeconIncident> incidentLog = new List<DeconIncident>();

        // Plan 78: effluent tracking
        public float effluentTankVolume;
        public float effluentTankContamination;
        public float effluentTankCapacity = 200f;
        public float effluentFilterRemainingLiters = 500f;
        public bool effluentFilterInstalled;
        public float effluentSludgeVolume;

        // Plan 78: manual override log
        public bool manualOverrideEngaged;
        public List<DeconIncident> overrideLog = new List<DeconIncident>();

        // Plan 78: disposed gear tracking
        public List<string> disposedGearIds = new List<string>();
    }

    [Serializable]
    public sealed class DeconCase
    {
        public string caseId = string.Empty;
        public string survivorId = string.Empty;
        public string gearId = string.Empty;
        public float surfaceContamination;         // 0-1 (surface dust only, NOT lifetime dose)
        public float radiationDoseBeforeDecon;
        public DeconStatus status;
        public float progress;
        public int queuedDay = -1;
        public int startDay = -1;
        public int completeDay = -1;
        public bool bypassed;
        public string outcome = string.Empty;

        // Plan 78: multi-stage protocol support
        public string protocolId = string.Empty;
        public int currentStageIndex;
        public int totalStages;
        public string currentStageId = string.Empty;
        public int stageTicksRemaining;
        public float waterConsumedThisCycle;
        public float chelatorConsumedThisCycle;
        public float surfactantConsumedThisCycle;
        public float radiometricGateReading;
    }

    public enum DeconStatus { Queued, InProgress, Complete, Bypassed, Failed, RewashRequired, GearDisposalRequired, QuarantineRequired }

    [Serializable]
    public sealed class DeconIncident
    {
        public int day;
        public string caseId = string.Empty;
        public string description = string.Empty;
    }

    /// <summary>
    /// Result of a single decon stage tick. Returned by <see cref="DecontaminationSystem.TickActiveStage"/>.
    /// </summary>
    public sealed class DeconStageResult
    {
        public bool stageComplete;
        public bool cycleComplete;
        public string stageId = string.Empty;
        public string nextStageId = string.Empty;
        public string stageDisplayName = string.Empty;
        public string nextStageDisplayName = string.Empty;
        public int ticksRemaining;
        public float surfaceContamination;
        public float radiometricGateReading;
        public string outcome = string.Empty;
        public string error = string.Empty;
    }

    public sealed class DecontaminationSystem
    {
        public const string SystemId = "decontamination";

        // BUG-11 tunables: amount of surface contamination cleared per cycle,
        // and the symmetric transfer-from-surface-to-shelter-air delta when a
        // case is bypassed (positive shelter contamination) or completed
        // (negative shelter contamination). Previously magic 0.1 / 0.05 / 0.8
        // literals; now named so design intent is explicit and designers can
        // tune without touching the math.
        public const float SafeReleaseSurfaceDelta = -0.8f;
        public const float SafeReleaseShelterDelta = -0.05f;
        public const float BypassSurfaceDelta = -0.1f;
        // Bug-11: previously +0.1f (symmetric +0.1f/−0.1f transfer — shelter
        // air gained exactly what the surface lost; audit §8 "bypass should
        // at minimum NOT increase net shelter contamination"). By making this
        // 0 the bypassed surface dust is NOT transferred into shelter air:
        // the surface loses 0.1 surface contamination, shelter air stays put,
        // and the net movement is strictly less severe in-shelter terms.
        public const float BypassShelterDelta = 0f;

        private DecontaminationState _state = new DecontaminationState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly RadiationSystem _radiation;
        private readonly Inventory.Inventory _inventory;
        private readonly AirlockSecuritySystem _airlock;
        private readonly StartingLevelSystem _startingLevel;
        private readonly DeconProtocolCatalog _protocolCatalog;
        private int _currentDay;

        public DecontaminationState State => _state;
        public bool HasActiveCase => _state.activeCase != null && _state.activeCase.status == DeconStatus.InProgress;
        public event Action<DeconCase> OnCaseCompleted;
        public event Action OnDeconChanged;

        public DecontaminationSystem(
            ISeededRng rng,
            RadiationSystem radiation,
            Inventory.Inventory inventory,
            AirlockSecuritySystem airlock,
            StartingLevelSystem startingLevel,
            ILog? log)
            : this(rng, radiation, inventory, airlock, startingLevel, null, log)
        {
        }

        public DecontaminationSystem(
            ISeededRng rng,
            RadiationSystem radiation,
            Inventory.Inventory inventory,
            AirlockSecuritySystem airlock,
            StartingLevelSystem startingLevel,
            DeconProtocolCatalog? protocolCatalog = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _radiation = radiation ?? throw new ArgumentNullException(nameof(radiation));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _airlock = airlock ?? throw new ArgumentNullException(nameof(airlock));
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _protocolCatalog = protocolCatalog ?? new DeconProtocolCatalog();
            _log = log ?? NullLog.Instance;
            _state.effluentTankCapacity = _protocolCatalog.effluent_treatment?.default_tank_capacity_liters ?? 200f;
        }

        public ActionResult Enqueue(string survivorId, string gearId, float surfaceContamination)
        {
            var caseId = $"decon_{_currentDay}_{survivorId}";
            if (_state.queue.Exists(c => c.caseId == caseId))
                return ActionResult.Blocked("already_queued", "decon.already_queued");

            // CR3-06: caseId changes every day, so the caseId predicate alone
            // lets a survivor re-enqueue every new day forever, even with an
            // unresolved case on the queue or as the active case. Lock by
            // (survivorId + not-yet-resolved) — matches MentalHealthCrisisSystem's
            // survivor+status pattern. Keeps the caseId check as defense-in-depth.
            if (_state.queue.Exists(c => c.survivorId == survivorId
                                     && c.status != DeconStatus.Complete
                                     && c.status != DeconStatus.Bypassed
                                     && c.status != DeconStatus.Failed))
                return ActionResult.Blocked("survivor_busy", "decon.survivor_busy");
            if (_state.activeCase != null
                && _state.activeCase.survivorId == survivorId
                && _state.activeCase.status != DeconStatus.Complete
                && _state.activeCase.status != DeconStatus.Bypassed
                && _state.activeCase.status != DeconStatus.Failed)
                return ActionResult.Blocked("survivor_busy", "decon.survivor_busy");

            var deconCase = new DeconCase
            {
                caseId = caseId, survivorId = survivorId, gearId = gearId,
                surfaceContamination = Math.Clamp(surfaceContamination, 0f, 1f),
                radiationDoseBeforeDecon = _radiation.GetDosimeter(survivorId)?.CurrentReading ?? 0f,
                status = DeconStatus.Queued, queuedDay = _currentDay
            };
            _state.queue.Add(deconCase);
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.enqueued");
        }

        public ActionResult ProcessQueue()
        {
            if (_state.activeCase != null && _state.activeCase.status == DeconStatus.InProgress)
                return ActionResult.Blocked("active_case", "decon.active_case");

            if (_state.queue.Count == 0)
                return ActionResult.Blocked("empty_queue", "decon.empty_queue");

            // Atomic resource consumption: clean water + soap
            if (!_inventory.TryConsumeBill(new[] { "water_clean", "soap" }))
            {
                if (_inventory.CountById("water_clean") < 1)
                    return ActionResult.Blocked("no_water", "decon.no_water");
                return ActionResult.Blocked("no_soap", "decon.no_soap");
            }

            var next = _state.queue[0];
            next.status = DeconStatus.InProgress;
            next.startDay = _currentDay;
            _state.activeCase = next;
            _state.queue.RemoveAt(0);

            // Route to airlock
            _airlock.VisitorArrives(next.survivorId, "decon_subject");

            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.processing",
                new Dictionary<string, double> { { "queue_position", _state.queue.Count } });
        }

        public ActionResult CompleteCycle(bool safeRelease)
        {
            if (_state.activeCase == null)
                return ActionResult.Blocked("no_active", "decon.no_active");

            var c = _state.activeCase;
            if (c.status != DeconStatus.InProgress)
                return ActionResult.Blocked("not_in_progress", "decon.not_in_progress");

            if (safeRelease)
            {
                c.surfaceContamination = Math.Max(0, c.surfaceContamination + SafeReleaseSurfaceDelta);
                c.status = DeconStatus.Complete;
                c.completeDay = _currentDay;
                c.outcome = "decontaminated";

                // Reduce shelter air contamination slightly (air returns to baseline).
                _state.shelterContaminationLevel = Math.Max(0, _state.shelterContaminationLevel + SafeReleaseShelterDelta);
                if (_state.shelterContaminationLevel == 0)
                    _state.shelterContaminated = false;
            }
            else
            {
                c.status = DeconStatus.Bypassed;
                c.bypassed = true;
                c.outcome = "bypassed";
                c.surfaceContamination = Math.Max(0, c.surfaceContamination + BypassSurfaceDelta);

                // Shelter contamination consequence: surface contamination NOT
                // cleaned away is transferred to shelter air. Net shelter-level
                // change is BypassShelterDelta + BypassSurfaceDelta (currently
                // symmetric — see class-level constants for design notes).
                _state.shelterContaminationLevel = Math.Min(1f, _state.shelterContaminationLevel + BypassShelterDelta);
                _state.shelterContaminated = true;

                var incident = new DeconIncident
                {
                    day = _currentDay, caseId = c.caseId,
                    description = $"{c.survivorId} bypassed decon — shelter contamination increased"
                };
                _state.incidentLog.Add(incident);
            }

            _log.Info($"[Decon] case {c.caseId}: {c.outcome} (surface={c.surfaceContamination:F2})");
            OnCaseCompleted?.Invoke(c);
            _state.activeCase = null;
            OnDeconChanged?.Invoke();
            return ActionResult.Success($"decon.{c.outcome}",
                new Dictionary<string, double> { { "surface_contamination", c.surfaceContamination } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Passive shelter contamination decay
            if (_state.shelterContaminated && _state.shelterContaminationLevel > 0)
            {
                _state.shelterContaminationLevel = Math.Max(0, _state.shelterContaminationLevel - 0.01f);
                if (_state.shelterContaminationLevel == 0)
                    _state.shelterContaminated = false;
            }

            // Effluent settling (passive)
            if (_state.effluentTankVolume > 0 && _state.effluentTankContamination > 0)
            {
                // Natural settling reduces contamination slightly each day
                _state.effluentTankContamination = Math.Max(0, _state.effluentTankContamination - 0.005f);
            }
        }

        // ─── Plan 78: Protocol-based decontamination ───

        /// <summary>
        /// Start a protocol-based decontamination cycle for the given case.
        /// Consumes resources for the full protocol upfront.
        /// </summary>
        public ActionResult StartProtocolCycle(string protocolId, string survivorId, string gearId, float surfaceContamination, float operatorSkill = 0.5f)
        {
            var protocol = FindProtocol(protocolId);
            if (protocol == null)
                return ActionResult.Blocked("unknown_protocol", "decon.unknown_protocol");

            // Check queue/active locks (same as Enqueue)
            if (_state.queue.Exists(c => c.survivorId == survivorId
                                     && c.status != DeconStatus.Complete
                                     && c.status != DeconStatus.Bypassed
                                     && c.status != DeconStatus.Failed))
                return ActionResult.Blocked("survivor_busy", "decon.survivor_busy");
            if (_state.activeCase != null
                && _state.activeCase.survivorId == survivorId
                && _state.activeCase.status != DeconStatus.Complete
                && _state.activeCase.status != DeconStatus.Bypassed
                && _state.activeCase.status != DeconStatus.Failed
                && _state.activeCase.status != DeconStatus.RewashRequired)
                return ActionResult.Blocked("survivor_busy", "decon.survivor_busy");

            // Consume chelator FIRST — a failed precondition must consume
            // no resources (Plans 78-81 §15 resource transaction policy).
            if (protocol.total_chelator_units > 0)
            {
                if (!_inventory.TryConsume("item_decon_chelator_concentrate", protocol.total_chelator_units))
                    return ActionResult.Blocked("no_chelator", "decon.no_chelator");
            }

            // Check resources
            if (!_inventory.TryConsumeBill(new[] { "water_clean", "soap" }))
            {
                if (_inventory.CountById("water_clean") < 1)
                    return ActionResult.Blocked("no_water", "decon.no_water");
                return ActionResult.Blocked("no_soap", "decon.no_soap");
            }

            var caseId = $"decon_{_currentDay}_{survivorId}";
            var deconCase = new DeconCase
            {
                caseId = caseId,
                survivorId = survivorId,
                gearId = gearId,
                surfaceContamination = Math.Clamp(surfaceContamination, 0f, 1f),
                radiationDoseBeforeDecon = _radiation.GetDosimeter(survivorId)?.CurrentReading ?? 0f,
                status = DeconStatus.InProgress,
                queuedDay = _currentDay,
                startDay = _currentDay,
                protocolId = protocolId,
                currentStageIndex = 0,
                totalStages = protocol.stages.Count,
                currentStageId = protocol.stages[0].stage_id,
                stageTicksRemaining = protocol.stages[0].duration_ticks
            };

            _state.activeCase = deconCase;
            _airlock.VisitorArrives(survivorId, "decon_subject");

            _log.Info($"[Decon] protocol {protocolId} started for {survivorId} (surface={surfaceContamination:F2})");
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.protocol_started");
        }

        /// <summary>
        /// Advance the active case by one stage tick. Returns the stage result.
        /// Call once per tick while a case is active.
        /// </summary>
        public DeconStageResult TickActiveStage(float operatorSkill = 0.5f)
        {
            if (_state.activeCase == null || _state.activeCase.status != DeconStatus.InProgress)
                return new DeconStageResult { stageComplete = false, error = "no_active_case" };

            var c = _state.activeCase;
            var protocol = FindProtocol(c.protocolId);
            if (protocol == null || c.currentStageIndex >= protocol.stages.Count)
                return new DeconStageResult { stageComplete = false, error = "invalid_protocol_state" };

            var stage = protocol.stages[c.currentStageIndex];
            c.stageTicksRemaining--;

            if (c.stageTicksRemaining > 0)
            {
                // Stage still in progress
                return new DeconStageResult
                {
                    stageComplete = false,
                    stageId = stage.stage_id,
                    ticksRemaining = c.stageTicksRemaining,
                    stageDisplayName = stage.display_name
                };
            }

            // Stage complete — apply its effects
            float skillMod = stage.requires_operator ? 1f + (operatorSkill * stage.operator_skill_factor) : 1f;
            float removal = stage.external_contamination_multiplier * skillMod;
            removal = Math.Min(removal, c.surfaceContamination); // Don't go below zero
            c.surfaceContamination = Math.Max(0, c.surfaceContamination - removal);

            // Track resource consumption
            c.waterConsumedThisCycle += stage.water_liters;
            c.chelatorConsumedThisCycle += stage.chelator_units;
            c.surfactantConsumedThisCycle += stage.surfactant_units;

            // Effluent accumulation
            _state.effluentTankVolume = Math.Min(_state.effluentTankCapacity,
                _state.effluentTankVolume + stage.water_liters * 0.9f); // 90% captured
            _state.effluentTankContamination = Math.Min(1f,
                _state.effluentTankContamination + stage.effluent_contamination_contribution * removal);
            _state.effluentSludgeVolume += stage.water_liters * 0.02f;

            // Effluent filter degradation
            if (_state.effluentFilterInstalled && _state.effluentFilterRemainingLiters > 0)
            {
                _state.effluentFilterRemainingLiters = Math.Max(0,
                    _state.effluentFilterRemainingLiters - stage.water_liters);
            }

            c.currentStageIndex++;

            // Check if this was the last stage (radiometric gate)
            if (c.currentStageIndex >= protocol.stages.Count)
            {
                // Radiometric gate reading
                c.radiometricGateReading = c.surfaceContamination * 10f; // Game-scale reading
                float threshold = protocol.interlock_threshold_mSv_per_h;

                if (c.radiometricGateReading > threshold)
                {
                    if (_state.manualOverrideEngaged)
                    {
                        c.status = DeconStatus.Complete;
                        c.completeDay = _currentDay;
                        c.outcome = "decontaminated_override";
                        _state.manualOverrideEngaged = false;

                        var overrideIncident = new DeconIncident
                        {
                            day = _currentDay, caseId = c.caseId,
                            description = $"MANUAL OVERRIDE: {c.survivorId} cleared despite radiometric reading {c.radiometricGateReading:F2} > threshold {threshold:F2}"
                        };
                        _state.overrideLog.Add(overrideIncident);
                        _state.shelterContaminated = true;
                        _state.shelterContaminationLevel = Math.Min(1f, _state.shelterContaminationLevel + 0.05f);
                    }
                    else
                    {
                        c.status = DeconStatus.RewashRequired;
                        c.outcome = "rewash_required";
                    }
                }
                else
                {
                    c.status = DeconStatus.Complete;
                    c.completeDay = _currentDay;
                    c.outcome = "decontaminated";

                    // Reduce shelter air contamination
                    _state.shelterContaminationLevel = Math.Max(0, _state.shelterContaminationLevel - 0.05f);
                    if (_state.shelterContaminationLevel == 0)
                        _state.shelterContaminated = false;
                }

                _log.Info($"[Decon] case {c.caseId}: {c.outcome} (surface={c.surfaceContamination:F2}, gate={c.radiometricGateReading:F2})");
                OnCaseCompleted?.Invoke(c);
                if (c.status == DeconStatus.Complete)
                    _state.activeCase = null;
                OnDeconChanged?.Invoke();

                return new DeconStageResult
                {
                    stageComplete = true,
                    cycleComplete = true,
                    stageId = stage.stage_id,
                    stageDisplayName = stage.display_name,
                    outcome = c.outcome,
                    surfaceContamination = c.surfaceContamination,
                    radiometricGateReading = c.radiometricGateReading
                };
            }

            // Advance to next stage
            var nextStage = protocol.stages[c.currentStageIndex];
            c.stageTicksRemaining = nextStage.duration_ticks;
            c.currentStageId = nextStage.stage_id;

            OnDeconChanged?.Invoke();
            return new DeconStageResult
            {
                stageComplete = true,
                cycleComplete = false,
                stageId = stage.stage_id,
                nextStageId = nextStage.stage_id,
                stageDisplayName = stage.display_name,
                nextStageDisplayName = nextStage.display_name,
                surfaceContamination = c.surfaceContamination
            };
        }

        /// <summary>
        /// Engage manual override: forces the inner door open regardless of radiometric reading.
        /// Must be explicitly called — logged and dangerous.
        /// </summary>
        public ActionResult EngageManualOverride()
        {
            if (_state.activeCase == null)
                return ActionResult.Blocked("no_active", "decon.no_active");

            _state.manualOverrideEngaged = true;
            _log.Warn($"[Decon] MANUAL OVERRIDE engaged for case {_state.activeCase.caseId}");
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.override_engaged");
        }

        /// <summary>
        /// Dispose of gear that exceeds safe cleaning limits.
        /// </summary>
        public ActionResult DisposeContaminatedGear(string gearId)
        {
            if (string.IsNullOrEmpty(gearId))
                return ActionResult.Blocked("invalid_gear", "decon.invalid_gear");

            if (!_inventory.TryConsume("item_sealed_waste_bin", 1))
                return ActionResult.Blocked("no_waste_bin", "decon.no_waste_bin");

            // Remove gear from inventory
            if (!_inventory.TryConsume(gearId, 1))
                return ActionResult.Blocked("gear_not_found", "decon.gear_not_found");

            _state.disposedGearIds.Add(gearId);
            _log.Info($"[Decon] gear {gearId} disposed in sealed waste bin");
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.gear_disposed");
        }

        /// <summary>
        /// Check if gear should be disposed (contamination above threshold).
        /// </summary>
        public bool ShouldDisposeGear(float contaminationLevel)
        {
            var threshold = _protocolCatalog.gear_disposal?.disposal_threshold ?? 0.85f;
            return contaminationLevel >= threshold;
        }

        /// <summary>
        /// Treat effluent: settle, filter, recover water if possible.
        /// </summary>
        public ActionResult TreatEffluent()
        {
            if (_state.effluentTankVolume <= 0)
                return ActionResult.Blocked("empty_tank", "decon.empty_tank");

            if (_state.effluentFilterInstalled && _state.effluentFilterRemainingLiters <= 0)
                return ActionResult.Blocked("filter_exhausted", "decon.filter_exhausted");

            float waterRecovered = _state.effluentTankVolume * 0.15f;
            float sludgeProduced = _state.effluentTankVolume * 0.02f;

            _state.effluentTankVolume = 0;
            _state.effluentTankContamination = 0;
            _state.effluentSludgeVolume += sludgeProduced;

            // Return recovered water (limited, treated)
            if (waterRecovered > 0)
                _inventory.AddById("water_clean", (int)Math.Ceiling(waterRecovered));

            _log.Info($"[Decon] effluent treated: recovered {waterRecovered:F1}L water, {sludgeProduced:F2}L sludge");
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.effluent_treated");
        }

        /// <summary>
        /// Install an effluent filter into the tank.
        /// </summary>
        public ActionResult InstallEffluentFilter()
        {
            if (_state.effluentFilterInstalled)
                return ActionResult.Blocked("filter_installed", "decon.filter_installed");

            if (!_inventory.TryConsume("item_lead_lined_effluent_filter", 1))
                return ActionResult.Blocked("no_filter", "decon.no_filter");

            _state.effluentFilterInstalled = true;
            _state.effluentFilterRemainingLiters = 500f;
            _log.Info("[Decon] effluent filter installed");
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.filter_installed");
        }

        /// <summary>
        /// Returns whether the inner door can be safely opened.
        /// </summary>
        public bool CanOpenInnerDoor()
        {
            if (_state.activeCase == null) return true;
            if (_state.manualOverrideEngaged) return true;
            return _state.activeCase.status == DeconStatus.Complete;
        }

        public string InnerDoorFailureReason()
        {
            if (_state.activeCase == null) return string.Empty;
            if (_state.manualOverrideEngaged) return "OVERRIDE";
            if (_state.activeCase.status == DeconStatus.InProgress) return "CYCLE IN PROGRESS";
            if (_state.activeCase.status == DeconStatus.RewashRequired) return "REWASH REQUIRED";
            if (_state.activeCase.status == DeconStatus.GearDisposalRequired) return "GEAR DISPOSAL REQUIRED";
            if (_state.activeCase.status == DeconStatus.QuarantineRequired) return "QUARANTINE REQUIRED";
            return string.Empty;
        }

        public IReadOnlyList<DeconProtocolDef> Protocols => _protocolCatalog.protocols;

        public DeconProtocolDef? FindProtocol(string protocolId)
        {
            if (string.IsNullOrEmpty(protocolId)) return null;
            foreach (var p in _protocolCatalog.protocols)
                if (p.protocol_id == protocolId) return p;
            return null;
        }

        public DeconProtocolCatalog ProtocolCatalog => _protocolCatalog;

        public DecontaminationState CaptureState() => CloneState(_state);

        public void RestoreState(DecontaminationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static DecontaminationState CloneState(DecontaminationState src)
        {
            if (src == null) return new DecontaminationState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<DecontaminationState>(json) ?? new DecontaminationState();
        }
    }
}
