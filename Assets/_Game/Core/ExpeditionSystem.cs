using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
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

        private readonly RadiationSystem _radSystem;
        private readonly Inventory.Inventory _inventory;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly WeatherSystem _weatherSystem;
        private readonly RadiationKnowledgeMap _knowledgeMap;
        private readonly System.Random _rng;

        private readonly List<ExpeditionState> _activeExpeditions = new List<ExpeditionState>();
        private readonly List<EncounterSO> _encounterPool = new List<EncounterSO>();

        public IReadOnlyList<ExpeditionState> ActiveExpeditions => _activeExpeditions;
        public IReadOnlyList<EncounterSO> EncounterPool => _encounterPool;

        // Events
        public event Action<ExpeditionState> OnExpeditionStarted;
        public event Action<ExpeditionState> OnExpeditionTick;
        public event Action<ExpeditionState, EncounterSO> OnEncounterTriggered;
        public event Action<ExpeditionState, EncounterSO, EventChoice> OnEncounterResolved;
        public event Action<ExpeditionState, List<ItemDefinition>> OnExpeditionCompleted;
        public event Action<ExpeditionState, string> OnExpeditionFailed;

        public ExpeditionSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            WeatherSystem weatherSystem = null,
            RadiationKnowledgeMap knowledgeMap = null,
            int seed = 42)
        {
            _radSystem = radSystem;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            _weatherSystem = weatherSystem;
            _knowledgeMap = knowledgeMap;
            _rng = new System.Random(seed);

            CreateDefaultEncounters();
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

        /// <summary>
        /// Start an expedition for a survivor to a target location node.
        /// Returns false if survivor is invalid, dead, or already on an expedition.
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
            int distanceTicks = Mathf.Max(1, Mathf.RoundToInt(location.travelHours));

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
                        float returnStep = exp.Stance == ExpeditionStance.Speed ? 1.5f : 1.0f;
                        exp.TravelTicksCompleted -= Mathf.RoundToInt(returnStep);

                        if (exp.TravelTicksCompleted <= 0)
                        {
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
