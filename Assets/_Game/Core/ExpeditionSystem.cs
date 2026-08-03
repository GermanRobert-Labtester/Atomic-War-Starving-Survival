using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Decoupled, background-simulated Expedition Engine.
    /// Manages node-based travel ticks, stance selection, stamina drain,
    /// psychological encounter auto-resolution, and push-your-luck looting.
    /// </summary>
    public class ExpeditionSystem
    {
        public const float BaseStaminaDrainPerHour = 5f;
        public const float MaxCarryingCapacityDefault = 30f;

        /// <summary>AcuteDoseWindow added to the survivor when caught in the flashpoint.</summary>
        public const float FlashpointAcuteDoseSpike = 30f;

        /// <summary>Default shelter delay (ticks) for the Cautious flashpoint behavior.</summary>
        public const int DefaultCautiousShelterDelayTicks = 18; // 12-24 hour range; midpoint

        /// <summary>Return speed multiplier for the Paranoid sprint-home-empty-handed behavior.</summary>
        public const float ParanoidSprintMultiplier = 2.0f;

        /// <summary>Return speed divisor for the Fatalist slow-walk behavior.</summary>
        public const float FatalistSlowWalkDivisor = 2.0f;

        // Hatch-dilemma consequence magnitudes (Prompt #26 follow-up).
        // Tuned for designer iteration; log a one-liner on each apply so
        // the values can be re-tuned from the Editor console.
        /// <summary>Bunker contamination (rads/hr) added when the player picks "let them in".</summary>
        public const float LetThemInContaminationRadsPerHour = 50f;
        /// <summary>Smaller bunker contamination (rads/hr) added when the player picks "force decon" (small spill during strip-and-decon).</summary>
        public const float ForceDeconContaminationRadsPerHour = 10f;
        /// <summary>Morale hit applied to every OTHER living survivor when the player picks "deny entry".</summary>
        public const float DenyEntryMoralePenaltyForOtherSurvivors = 20f;

        private readonly RadiationSystem _radSystem;
        private readonly Inventory.Inventory _inventory;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly WeatherSystem _weatherSystem;
        private readonly RadiationKnowledgeMap _knowledgeMap;
        private readonly System.Random _rng;
        private readonly MedicalSystem _medicalSystem;
        private readonly Shelter.Shelter _shelter;
        private readonly IReadOnlyList<Survivor> _survivors;
        private GeneratedMap _generatedMap;

        private readonly List<ExpeditionState> _activeExpeditions = new List<ExpeditionState>();
        private readonly List<EncounterSO> _encounterPool = new List<EncounterSO>();

        public IReadOnlyList<ExpeditionState> ActiveExpeditions => _activeExpeditions;
        public IReadOnlyList<EncounterSO> EncounterPool => _encounterPool;
        public GeneratedMap GeneratedMap => _generatedMap;

        // Events
        public event Action<ExpeditionState> OnExpeditionStarted;
        public event Action<ExpeditionState> OnExpeditionTick;
        public event Action<ExpeditionState, EncounterSO> OnEncounterTriggered;
        public event Action<ExpeditionState, EncounterSO, EventChoice> OnEncounterResolved;
        public event Action<ExpeditionState, List<ItemDefinition>> OnExpeditionCompleted;
        public event Action<ExpeditionState, string> OnExpeditionFailed;

        // Day-30 Flashpoint intercept events
        public event Action<ExpeditionState> OnFlashpointIntercepted;
        public event Action<ExpeditionState> OnHatchDilemmaReady;
        public event Action<ExpeditionState> OnHatchDilemmaResolved;

        public ExpeditionSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            WeatherSystem weatherSystem = null,
            RadiationKnowledgeMap knowledgeMap = null,
            MedicalSystem medicalSystem = null,
            Shelter.Shelter shelter = null,
            IReadOnlyList<Survivor> survivors = null,
            int seed = 42)
        {
            _radSystem = radSystem;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            _weatherSystem = weatherSystem;
            _knowledgeMap = knowledgeMap;
            _medicalSystem = medicalSystem;
            _shelter = shelter;
            _survivors = survivors;
            _rng = new System.Random(seed);

            CreateDefaultEncounters();

            // Subscribe to the typed intercept signal published by the
            // FlashpointChoreographer's EMP step. Idempotent: the
            // EventBus uses a singleton dictionary so duplicate
            // subscriptions are ignored.
            EventBus.Subscribe<FlashpointInterceptSignal>(HandleFlashpointIntercept);
            EventBus.Subscribe<HatchDilemmaResolvedSignal>(HandleHatchDilemmaResolved);
        }

        /// <summary>
        /// Day-30 intercept: sever comms on every active expedition, apply
        /// trauma + acute-dose spike, resolve the survivor's trait-driven
        /// behavior. Idempotent per expedition: re-firing on the same
        /// signal is a no-op (the first call sets the behavior).
        /// </summary>
        private void HandleFlashpointIntercept(FlashpointInterceptSignal signal)
        {
            if (signal.InterceptedExpeditions == null) return;

            for (int i = 0; i < signal.InterceptedExpeditions.Count; i++)
            {
                var exp = signal.InterceptedExpeditions[i];
                if (exp == null || exp.isCommsSevered) continue;
                ApplyFlashpointIntercept(exp);
            }
        }

        private void ApplyFlashpointIntercept(ExpeditionState exp)
        {
            if (exp.Survivor == null) return;

            // 1. Sever comms
            exp.isCommsSevered = true;

            // 2. Acute-dose spike
            exp.Survivor.AcuteDoseWindow += FlashpointAcuteDoseSpike;

            // 3. Trauma affliction (broken bones / shockwave)
            if (_medicalSystem != null)
            {
                _medicalSystem.Inflict(exp.Survivor, AfflictionSO.Ids.BrokenBone);
            }

            // 4. Trait-driven resolution
            ResolveFlashpointBehavior(exp);

            // 5. Cache original ETA so the UI / save can show "halved" / "delayed"
            exp.originalEtaTicks = exp.TravelTicksCompleted;

            // 6. Force into the Inbound phase so the survivor begins the return.
            // If they were Looting, abandon the loot site immediately; if they
            // were Outbound, the inbound leg is the full distance.
            exp.Phase = ExpeditionPhase.Inbound;
            if (exp.IsPushingLuck) exp.IsPushingLuck = false;

            OnFlashpointIntercepted?.Invoke(exp);
        }

        private void ResolveFlashpointBehavior(ExpeditionState exp)
        {
            var sv = exp.Survivor;
            if (sv == null) return;

            switch (sv.RiskBias)
            {
                case RiskBiasTrait.Paranoid:
                    // Drop all loot, sprint home
                    exp.DropLoot(1.0f);
                    exp.returnSpeedMultiplier = ParanoidSprintMultiplier;
                    exp.flashpointBehavior = FlashpointBehavior.ParanoidSprint;
                    // Halve the visual ETA so the UI shows progress
                    if (exp.originalEtaTicks <= 0f) exp.originalEtaTicks = exp.TravelTicksCompleted;
                    exp.TravelTicksCompleted = Mathf.Max(0, Mathf.RoundToInt(exp.TravelTicksCompleted * 0.5f));
                    break;

                case RiskBiasTrait.Cautious:
                    // Take temporary shelter, gain radiation anxiety
                    exp.shelterDelayTicksRemaining = DefaultCautiousShelterDelayTicks;
                    exp.flashpointBehavior = FlashpointBehavior.CautiousShelter;
                    sv.HasRadiationAnxietyStatus = true;
                    break;

                case RiskBiasTrait.Fatalist:
                    // Numb walk: keep loot, half speed, gain Numbness
                    exp.returnSpeedDivisor = FatalistSlowWalkDivisor;
                    exp.flashpointBehavior = FlashpointBehavior.FatalistNumbWalk;
                    sv.IsNumb = true;
                    break;

                case RiskBiasTrait.Reckless:
                case RiskBiasTrait.Realist:
                case RiskBiasTrait.Denialist:
                default:
                    // Keep all loot, normal return speed, full rad accumulation
                    exp.flashpointBehavior = FlashpointBehavior.RecklessPushThrough;
                    break;
            }
        }

        /// <summary>
        /// Resolve a hatch dilemma choice. Called by the GameBootstrap (or
        /// any handler that runs the dilemma GameEventSO through the
        /// EventRunner) once the player picks a choice.
        ///
        /// Applies both the state-machine consequence (complete/fail the
        /// expedition, kill the survivor) and the bunker-wide side effects
        /// (contamination spike on let-them-in / force-decon; morale
        /// propagation to the rest of the bunker on deny-entry). The side
        /// effects are intentionally co-located here so they are testable
        /// without a full GameBootstrap setup.
        /// </summary>
        public void ApplyHatchDilemmaChoice(string expeditionId, HatchDilemmaResolvedSignal.Resolution choice)
        {
            var exp = FindExpeditionById(expeditionId);
            if (exp == null || exp.Phase != ExpeditionPhase.AtHatchDilemma) return;

            string survivorName = exp.Survivor != null ? exp.Survivor.DisplayName : "the survivor";

            switch (choice)
            {
                case HatchDilemmaResolvedSignal.Resolution.LetThemIn:
                    // Complete the expedition. The bunker's ambient contamination
                    // spikes by the configured amount (their gear and clothes are
                    // soaked in fallout). The survivor is sick but alive.
                    if (_shelter != null)
                    {
                        _shelter.AddBunkerContamination(LetThemInContaminationRadsPerHour);
                        Debug.Log($"[Flashpoint] LetThemIn: bunker contamination +{LetThemInContaminationRadsPerHour} rph " +
                                  $"(now {_shelter.BunkerContamination:F1}) after admitting {survivorName}.");
                    }
                    CompleteExpedition(exp);
                    RemoveExpedition(exp);
                    break;

                case HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside:
                    // Strip outside in the ash: 2 hours of rad damage, big morale
                    // hit, plus a small contamination spill (the gear is set
                    // down just inside the airlock for decon).
                    if (exp.Survivor != null && _radSystem != null)
                    {
                        _radSystem.Expose(exp.Survivor, 10f, 2f);
                        exp.Survivor.Needs.Morale = Mathf.Clamp(exp.Survivor.Needs.Morale - 15f, 0f, 100f);
                    }
                    if (_shelter != null)
                    {
                        _shelter.AddBunkerContamination(ForceDeconContaminationRadsPerHour);
                        Debug.Log($"[Flashpoint] ForceDecon: bunker contamination +{ForceDeconContaminationRadsPerHour} rph " +
                                  $"(now {_shelter.BunkerContamination:F1}) from {survivorName}'s strip-down.");
                    }
                    CompleteExpedition(exp);
                    RemoveExpedition(exp);
                    break;

                case HatchDilemmaResolvedSignal.Resolution.DenyEntry:
                    // Survivor dies outside. Massive morale penalty propagates
                    // to every OTHER living survivor in the bunker.
                    if (exp.Survivor != null)
                    {
                        exp.Survivor.State = SurvivorState.Dead;
                    }
                    int affected = PropagateDenyEntryMoralePenalty(exp.SurvivorId);
                    exp.Phase = ExpeditionPhase.Failed;
                    OnExpeditionFailed?.Invoke(exp, "denied_entry");
                    RemoveExpedition(exp);
                    Debug.Log($"[Flashpoint] DenyEntry: {survivorName} died outside the hatch; " +
                              $"{affected} other survivor(s) lost {DenyEntryMoralePenaltyForOtherSurvivors} morale.");
                    break;
            }

            OnHatchDilemmaResolved?.Invoke(exp);
        }

        /// <summary>
        /// Apply the deny-entry morale hit to every other living survivor in
        /// the configured survivor list. Returns the count of survivors
        /// affected. Skips the dying survivor (by id) and any non-alive or
        /// null entries. Logs a one-liner so designers can tune the magnitude.
        /// </summary>
        private int PropagateDenyEntryMoralePenalty(string dyingSurvivorId)
        {
            if (_survivors == null) return 0;
            int affected = 0;
            for (int i = 0; i < _survivors.Count; i++)
            {
                var sv = _survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!string.IsNullOrEmpty(dyingSurvivorId) && sv.Id == dyingSurvivorId) continue;
                sv.Needs.Morale = Mathf.Clamp(
                    sv.Needs.Morale - DenyEntryMoralePenaltyForOtherSurvivors,
                    0f, 100f);
                affected++;
            }
            return affected;
        }

        private void HandleHatchDilemmaResolved(HatchDilemmaResolvedSignal signal)
        {
            ApplyHatchDilemmaChoice(signal.ExpeditionId, signal.Choice);
        }

        private ExpeditionState FindExpeditionById(string expeditionId)
        {
            if (string.IsNullOrEmpty(expeditionId)) return null;
            for (int i = 0; i < _activeExpeditions.Count; i++)
            {
                if (_activeExpeditions[i] != null && _activeExpeditions[i].ExpeditionId == expeditionId)
                    return _activeExpeditions[i];
            }
            return null;
        }

        private void RemoveExpedition(ExpeditionState exp)
        {
            _activeExpeditions.Remove(exp);
        }

        public void SetEncounterPool(IEnumerable<EncounterSO> encounters)
        {
            _encounterPool.Clear();
            if (encounters != null)
            {
                _encounterPool.AddRange(encounters);
            }
            if (_encounterPool.Count == 0)
            {
                CreateDefaultEncounters();
            }
        }

        /// <summary>Inject proc-gen wasteland map (visit/reveal on arrival).</summary>
        public void SetGeneratedMap(GeneratedMap map)
        {
            _generatedMap = map;
        }

        /// <summary>
        /// Start an expedition for a survivor to a target location node.
        /// Returns false if survivor is invalid, dead, or already on an expedition.
        /// Travel hours are multiplied by current weather (blizzards ×2).
        /// </summary>
        public bool StartExpedition(
            Survivor survivor,
            LocationDefinitionSO location,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            float maxLootCapacity = MaxCarryingCapacityDefault)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;
            if (IsOnExpedition(survivor.Id)) return false;

            float trueRad = ResolveTrueRad(location);
            float travelHours = location.travelHours * CurrentWeatherTravelMultiplier();
            int distanceTicks = Mathf.Max(1, Mathf.RoundToInt(travelHours));

            var state = new ExpeditionState
            {
                ExpeditionId = Guid.NewGuid().ToString("N"),
                SurvivorId = survivor.Id,
                Survivor = survivor,
                TargetLocationId = location.id,
                TargetLocationName = location.displayName,
                Stance = stance,
                Phase = ExpeditionPhase.Outbound,
                TotalDistanceTicks = distanceTicks,
                CarryingCapacity = maxLootCapacity,
                TrueRadPerHour = trueRad,
                DangerLevel = location.dangerLevel,
                Stamina = 100f,
                SuitDegradation = 0f
            };

            // Remove survivor from shelter active state
            survivor.State = SurvivorState.Working;

            _activeExpeditions.Add(state);
            OnExpeditionStarted?.Invoke(state);
            return true;
        }

        /// <summary>
        /// Start an expedition to a proc-gen <see cref="MapNode"/>.
        /// Path travel uses weather-scaled hours from the generated map graph.
        /// </summary>
        public bool StartExpedition(
            Survivor survivor,
            MapNode node,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            float maxLootCapacity = MaxCarryingCapacityDefault)
        {
            if (survivor == null || !survivor.IsAlive || node == null || node.IsShelter) return false;
            if (IsOnExpedition(survivor.Id)) return false;

            float trueRad = node.TrueRad;
            if (_knowledgeMap != null)
            {
                var tile = _knowledgeMap.GetTile(node.NodeId);
                if (tile != null) trueRad = tile.TrueRad;
            }

            float travelHours;
            if (_generatedMap != null)
            {
                var weather = _weatherSystem != null ? _weatherSystem.Current : WeatherKind.Clear;
                travelHours = _generatedMap.GetTravelHoursFromShelter(node.NodeId, weather);
                if (travelHours <= 0f)
                    travelHours = node.DistanceFromShelter * CurrentWeatherTravelMultiplier();
            }
            else
            {
                travelHours = node.DistanceFromShelter * CurrentWeatherTravelMultiplier();
            }

            int distanceTicks = Mathf.Max(1, Mathf.RoundToInt(travelHours));

            var state = new ExpeditionState
            {
                ExpeditionId = Guid.NewGuid().ToString("N"),
                SurvivorId = survivor.Id,
                Survivor = survivor,
                TargetLocationId = node.NodeId,
                TargetLocationName = node.DisplayName,
                Stance = stance,
                Phase = ExpeditionPhase.Outbound,
                TotalDistanceTicks = distanceTicks,
                CarryingCapacity = maxLootCapacity,
                TrueRadPerHour = trueRad,
                DangerLevel = node.DangerLevel,
                Stamina = 100f,
                SuitDegradation = 0f
            };

            survivor.State = SurvivorState.Working;
            _activeExpeditions.Add(state);
            OnExpeditionStarted?.Invoke(state);
            return true;
        }

        /// <summary>
        /// Start expedition from a MapScreenUI path request (precomputed weather hours).
        /// </summary>
        public bool StartExpeditionFromPath(
            Survivor survivor,
            string nodeId,
            float travelHours,
            float trueRad,
            float dangerLevel,
            string displayName,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            float maxLootCapacity = MaxCarryingCapacityDefault)
        {
            if (survivor == null || !survivor.IsAlive || string.IsNullOrEmpty(nodeId)) return false;
            if (IsOnExpedition(survivor.Id)) return false;
            if (nodeId == GeneratedMap.ShelterNodeId) return false;

            // Prefer live map data when available
            MapNode node = _generatedMap?.GetNode(nodeId);
            if (node != null)
            {
                return StartExpedition(survivor, node, stance, maxLootCapacity);
            }

            int distanceTicks = Mathf.Max(1, Mathf.RoundToInt(Mathf.Max(0.1f, travelHours)));
            var state = new ExpeditionState
            {
                ExpeditionId = Guid.NewGuid().ToString("N"),
                SurvivorId = survivor.Id,
                Survivor = survivor,
                TargetLocationId = nodeId,
                TargetLocationName = string.IsNullOrEmpty(displayName) ? nodeId : displayName,
                Stance = stance,
                Phase = ExpeditionPhase.Outbound,
                TotalDistanceTicks = distanceTicks,
                CarryingCapacity = maxLootCapacity,
                TrueRadPerHour = Mathf.Max(0f, trueRad),
                DangerLevel = dangerLevel,
                Stamina = 100f,
                SuitDegradation = 0f
            };

            survivor.State = SurvivorState.Working;
            _activeExpeditions.Add(state);
            OnExpeditionStarted?.Invoke(state);
            return true;
        }

        private float CurrentWeatherTravelMultiplier()
        {
            if (_weatherSystem == null) return 1f;
            return GeneratedMap.WeatherTravelMultiplier(_weatherSystem.Current);
        }

        /// <summary>Manually order an expedition survivor to begin returning to bunker.</summary>
        public bool RecallExpedition(string survivorId)
        {
            var state = GetExpeditionBySurvivor(survivorId);
            if (state == null || state.Phase == ExpeditionPhase.Inbound || state.Phase == ExpeditionPhase.Completed)
                return false;

            state.IsRetreating = true;
            state.IsPushingLuck = false;
            state.Phase = ExpeditionPhase.Inbound;
            return true;
        }

        public ExpeditionState GetExpeditionBySurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _activeExpeditions.Count; i++)
            {
                if (_activeExpeditions[i].SurvivorId == survivorId) return _activeExpeditions[i];
            }
            return null;
        }

        public bool IsOnExpedition(string survivorId)
        {
            return GetExpeditionBySurvivor(survivorId) != null;
        }

        /// <summary>
        /// Advance active expeditions by gameHours (each 1 hour = 1 tick).
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Process tick steps (1 hour per tick loop)
            int ticksToProcess = Mathf.Max(1, Mathf.FloorToInt(gameHours));
            float hoursPerTick = gameHours / ticksToProcess;

            for (int t = 0; t < ticksToProcess; t++)
            {
                ProcessSingleTick(hoursPerTick);
            }
        }

        private void ProcessSingleTick(float tickHours)
        {
            for (int i = _activeExpeditions.Count - 1; i >= 0; i--)
            {
                var exp = _activeExpeditions[i];
                if (exp == null || exp.Phase == ExpeditionPhase.Completed || exp.Phase == ExpeditionPhase.Failed)
                {
                    continue;
                }

                // Verify survivor lives
                if (exp.Survivor == null || !exp.Survivor.IsAlive)
                {
                    exp.Phase = ExpeditionPhase.Failed;
                    OnExpeditionFailed?.Invoke(exp, "Survivor died during expedition.");
                    _activeExpeditions.RemoveAt(i);
                    continue;
                }

                exp.CurrentTick++;

                // 1. Calculate & Apply Stamina Drain
                float staminaDrain = CalculateStaminaDrain(exp, tickHours);
                exp.Stamina = Mathf.Clamp(exp.Stamina - staminaDrain, 0f, 100f);

                // Draining stamina affects fatigue
                exp.Survivor.Needs.Fatigue = Mathf.Clamp(exp.Survivor.Needs.Fatigue + staminaDrain * 0.5f, 0f, 100f);

                if (exp.Stamina <= 0f)
                {
                    // Exhaustion penalty: drop half loot, take health hit
                    exp.DropLoot(0.5f);
                    exp.Survivor.Needs.Health = Mathf.Clamp(exp.Survivor.Needs.Health - 5f, 0f, 100f);
                }

                // 2. Radiation & Weather Exposure (Push-Your-Luck modifier in Looting phase)
                float radRate = exp.TrueRadPerHour;
                if (_weatherSystem != null)
                {
                    radRate += _weatherSystem.OutdoorRadModifier;
                }

                // Push-your-luck radiation escalation during looting phase
                if (exp.Phase == ExpeditionPhase.Looting)
                {
                    float pushMultiplier = 1f + (exp.LootingTicksCompleted * 0.15f);
                    radRate *= pushMultiplier;

                    // Push-your-luck fallout storm risk
                    if (_weatherSystem != null && _weatherSystem.Current != WeatherKind.FalloutStorm)
                    {
                        float stormChance = 0.02f + (exp.LootingTicksCompleted * 0.05f);
                        if (_rng.NextDouble() < stormChance)
                        {
                            _weatherSystem.ForceWeather(WeatherKind.FalloutStorm);
                        }
                    }
                }

                if (_radSystem != null)
                {
                    _radSystem.Expose(exp.Survivor, radRate, tickHours);
                }

                // 3. Phase Advancement
                switch (exp.Phase)
                {
                    case ExpeditionPhase.Outbound:
                        float travelStep = exp.Stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
                        exp.TravelTicksCompleted += Mathf.RoundToInt(travelStep);

                        if (exp.TravelTicksCompleted >= exp.TotalDistanceTicks)
                        {
                            // First arrival: mark proc-gen node visited + reveal fog-of-war
                            _generatedMap?.MarkVisited(exp.TargetLocationId);
                            exp.Phase = ExpeditionPhase.Looting;
                        }
                        break;

                    case ExpeditionPhase.Looting:
                        exp.LootingTicksCompleted++;
                        PerformLootRoll(exp);

                        // Check psychological auto-retreat trigger
                        if (exp.Survivor.RiskBias == RiskBiasTrait.Paranoid || exp.Survivor.HasRadiationAnxietyStatus)
                        {
                            if (exp.LootingTicksCompleted >= 2 || exp.Survivor.RadiationAnxiety > 0.6f)
                            {
                                // Paranoid flees, dropping some loot
                                exp.DropLoot(0.3f);
                                exp.Phase = ExpeditionPhase.Inbound;
                            }
                        }
                        else if (exp.Survivor.RiskBias == RiskBiasTrait.Cautious && exp.Stamina < 30f)
                        {
                            exp.Phase = ExpeditionPhase.Inbound;
                        }
                        else if (!exp.IsPushingLuck && exp.LootingTicksCompleted >= 3)
                        {
                            // Default return after 3 looting ticks unless pushing luck
                            exp.Phase = ExpeditionPhase.Inbound;
                        }
                        break;

                    case ExpeditionPhase.Inbound:
                        // Cautious shelter delay: pause the return until the
                        // shelter timer counts down.
                        if (exp.shelterDelayTicksRemaining > 0)
                        {
                            exp.shelterDelayTicksRemaining--;
                            OnExpeditionTick?.Invoke(exp);
                            continue;
                        }

                        // Effective return step: stance x trait multiplier / divisor
                        float baseReturnStep = exp.Stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
                        float returnStep = baseReturnStep * exp.returnSpeedMultiplier / Mathf.Max(0.01f, exp.returnSpeedDivisor);
                        exp.TravelTicksCompleted -= Mathf.RoundToInt(returnStep);

                        if (exp.TravelTicksCompleted <= 0)
                        {
                            // Comms-severed expeditions don't complete cleanly:
                            // they pause at the hatch and fire the dilemma.
                            if (exp.isCommsSevered)
                            {
                                exp.Phase = ExpeditionPhase.AtHatchDilemma;
                                bool alive = exp.Survivor != null && exp.Survivor.IsAlive;
                                EventBus.Raise(new HatchDilemmaReadySignal(exp, alive));
                                OnHatchDilemmaReady?.Invoke(exp);
                                continue;
                            }

                            CompleteExpedition(exp);
                            _activeExpeditions.RemoveAt(i);
                            continue;
                        }
                        break;
                }

                // 4. Roll for EncounterSO
                RollAndResolveEncounter(exp);

                OnExpeditionTick?.Invoke(exp);
            }
        }

        private float CalculateStaminaDrain(ExpeditionState exp, float hours)
        {
            float drain = BaseStaminaDrainPerHour * hours;

            // Carry weight penalty: up to +15/hr at full capacity
            float loadRatio = exp.CarryingCapacity > 0f ? Mathf.Clamp01(exp.CurrentWeight / exp.CarryingCapacity) : 0f;
            drain += loadRatio * 15f * hours;

            // Weather penalty
            if (_weatherSystem != null)
            {
                if (_weatherSystem.Current == WeatherKind.Blizzard || _weatherSystem.Current == WeatherKind.FalloutStorm)
                {
                    drain += 10f * hours;
                }
            }

            // Suit wear & degradation
            if (exp.Survivor.HasFullSuitEquipped)
            {
                exp.SuitDegradation = Mathf.Clamp(exp.SuitDegradation + 2f * hours, 0f, 100f);
                drain += 3f * hours; // suit heat & movement restriction
            }

            // Limp disability: permanently doubles stamina drain during expeditions
            if (exp.Survivor != null && exp.Survivor.HasDisability("limp"))
            {
                drain *= 2f;
            }

            return drain;
        }

        private void PerformLootRoll(ExpeditionState exp)
        {
            if (_itemCatalog == null || _itemCatalog.items == null || _itemCatalog.items.Count == 0) return;

            float chance = 0.5f + (exp.DangerLevel * 0.05f);
            if (_rng.NextDouble() < chance)
            {
                var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
                if (item != null)
                {
                    exp.TryAddLoot(item);
                }
            }
        }

        private void RollAndResolveEncounter(ExpeditionState exp)
        {
            if (_encounterPool.Count == 0) return;

            // Base encounter chance per tick: 30% modified by danger level and stance
            float encounterChance = 0.25f + (exp.DangerLevel * 0.05f);
            if (exp.Stance == ExpeditionStance.Speed) encounterChance *= 1.4f;
            else if (exp.Stance == ExpeditionStance.Stealth) encounterChance *= 0.6f;

            if (_rng.NextDouble() >= encounterChance) return;

            // Pick weighted encounter
            float totalWeight = 0f;
            List<EncounterSO> validEncounters = new List<EncounterSO>();
            List<float> weights = new List<float>();

            for (int i = 0; i < _encounterPool.Count; i++)
            {
                var enc = _encounterPool[i];
                float w = enc.GetEffectiveWeight(exp.Stance, exp.DangerLevel);
                if (w > 0f)
                {
                    validEncounters.Add(enc);
                    weights.Add(w);
                    totalWeight += w;
                }
            }

            if (validEncounters.Count == 0 || totalWeight <= 0f) return;

            double roll = _rng.NextDouble() * totalWeight;
            float accum = 0f;
            EncounterSO selected = validEncounters[0];

            for (int i = 0; i < validEncounters.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum)
                {
                    selected = validEncounters[i];
                    break;
                }
            }

            OnEncounterTriggered?.Invoke(exp, selected);

            // Psychological auto-resolution
            ResolveEncounterWithPsychology(exp, selected);
        }

        private void ResolveEncounterWithPsychology(ExpeditionState exp, EncounterSO selected)
        {
            EventChoice chosen = null;
            var survivor = exp.Survivor;

            // 1. Trait-based forced auto-resolution
            if (selected.enableAutoResolution && survivor != null)
            {
                if (survivor.RiskBias == selected.autoEngageTrait)
                {
                    // Reckless: engage directly! Gain extra loot or take damage
                    if (selected.choices != null && selected.choices.Count > 0)
                    {
                        chosen = selected.choices[0]; // First aggressive choice
                    }
                    // Direct outcome for Reckless engagement
                    PerformLootRoll(exp);
                }
                else if (survivor.RiskBias == selected.autoFleeTrait || survivor.HasRadiationAnxietyStatus)
                {
                    // Paranoid: flee and drop loot
                    exp.DropLoot(0.5f);
                    exp.Phase = ExpeditionPhase.Inbound;
                    if (selected.choices != null && selected.choices.Count > 1)
                    {
                        chosen = selected.choices[selected.choices.Count - 1]; // Retreat choice
                    }
                }
            }

            // 2. Fallback to belief-weighted choice selection
            if (chosen == null && selected.choices != null && selected.choices.Count > 0)
            {
                var eventContext = new EventContext(survivor, null, _inventory, _rng);
                var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
                gameEvent.choices = selected.choices;

                chosen = EventRunner.PickWeightedChoice(gameEvent, eventContext, _rng);
            }

            // Apply choice effects to survivor / state
            if (chosen != null)
            {
                if (chosen.MoraleDelta != 0f && survivor != null)
                {
                    survivor.Needs.Morale = Mathf.Clamp(survivor.Needs.Morale + chosen.MoraleDelta, 0f, 100f);
                }
            }

            OnEncounterResolved?.Invoke(exp, selected, chosen);
        }

        private void CompleteExpedition(ExpeditionState exp)
        {
            exp.Phase = ExpeditionPhase.Completed;

            // Transfer collected loot into bunker Inventory
            if (_inventory != null && exp.CollectedLoot != null)
            {
                for (int i = 0; i < exp.CollectedLoot.Count; i++)
                {
                    if (exp.CollectedLoot[i] != null)
                    {
                        _inventory.Add(exp.CollectedLoot[i], 1);
                    }
                }
            }

            // Return survivor to Idle state in shelter
            if (exp.Survivor != null)
            {
                exp.Survivor.State = SurvivorState.Idle;
            }

            OnExpeditionCompleted?.Invoke(exp, exp.CollectedLoot);
        }

        private float ResolveTrueRad(LocationDefinitionSO location)
        {
            if (_knowledgeMap != null)
            {
                var tile = _knowledgeMap.GetTile(location.id);
                if (tile != null) return tile.TrueRad;
            }
            return location.baseRadsPerHour;
        }

        private void CreateDefaultEncounters()
        {
            // 1. Feral Dogs
            var feralDogs = ScriptableObject.CreateInstance<EncounterSO>();
            feralDogs.id = "enc_feral_dogs";
            feralDogs.title = "Feral Dog Pack";
            feralDogs.description = "A hungry pack of mutated dogs guards the alley ahead.";
            feralDogs.category = EncounterCategory.Combat;
            feralDogs.baseWeight = 1.0f;
            feralDogs.stealthWeightMultiplier = 0.4f;
            feralDogs.speedWeightMultiplier = 1.6f;
            feralDogs.autoEngageTrait = RiskBiasTrait.Reckless;
            feralDogs.autoFleeTrait = RiskBiasTrait.Paranoid;
            feralDogs.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "engage", Text = "Drive them off aggressively", MoraleDelta = +5f },
                new EventChoice { ChoiceId = "sneak", Text = "Sneak past silently", MoraleDelta = 0f },
                new EventChoice { ChoiceId = "flee", Text = "Drop loot and flee", MoraleDelta = -5f }
            };
            _encounterPool.Add(feralDogs);

            // 2. Civil War Deserters
            var deserters = ScriptableObject.CreateInstance<EncounterSO>();
            deserters.id = "enc_deserters";
            deserters.title = "Civil War Deserters";
            deserters.description = "Armed scavengers demand a toll to pass through their sector.";
            deserters.category = EncounterCategory.Combat;
            deserters.baseWeight = 0.8f;
            deserters.minDangerLevel = 2f;
            deserters.stealthWeightMultiplier = 0.3f;
            deserters.speedWeightMultiplier = 1.8f;
            deserters.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "fight", Text = "Fight your way through", MoraleDelta = +10f },
                new EventChoice { ChoiceId = "pay", Text = "Pay them off with rations", MoraleDelta = -10f }
            };
            _encounterPool.Add(deserters);

            // 3. Collapsed Rubble
            var rubble = ScriptableObject.CreateInstance<EncounterSO>();
            rubble.id = "enc_collapsed_rubble";
            rubble.title = "Collapsed Concrete Rubble";
            rubble.description = "A heavy slab blocks the quick path forward.";
            rubble.category = EncounterCategory.Hazard;
            rubble.baseWeight = 1.2f;
            rubble.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "clear", Text = "Force your way through rubble", MoraleDelta = -2f },
                new EventChoice { ChoiceId = "detour", Text = "Take a long detour", MoraleDelta = -5f }
            };
            _encounterPool.Add(rubble);
        }
    }
}
