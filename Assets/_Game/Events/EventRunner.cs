using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    [Serializable]
    public class ActiveDelayedConsequence
    {
        public string EventId;
        public string ChoiceId;
        public float RemainingHours;
        public DelayedConsequence Consequence;
    }

    /// <summary>
    /// Data-driven engine that evaluates, triggers, and resolves GameEvents based on
    /// weighted conditions and presents choices with immediate and delayed consequences.
    /// Save/load safe.
    /// </summary>
    public class EventRunner
    {
        private readonly List<GameEvent> _pool = new List<GameEvent>();
        private readonly Dictionary<string, float> _cooldowns = new Dictionary<string, float>();
        private readonly List<ActiveDelayedConsequence> _activeConsequences = new List<ActiveDelayedConsequence>();

        // ── Scheduled-event queue (Prompt #43 — delayed narrative chains) ──
        private readonly List<ScheduledEvent> _scheduledEvents = new List<ScheduledEvent>();

        public IReadOnlyList<GameEvent> Pool => _pool;
        public IReadOnlyList<ActiveDelayedConsequence> ActiveConsequences => _activeConsequences;
        /// <summary>Read-only view of the pending deferred narrative events.</summary>
        public IReadOnlyList<ScheduledEvent> ScheduledEvents => _scheduledEvents;

        public event Action<GameEvent, EventContext> OnEventTriggered;
        public event Action<GameEvent, EventChoice, EventContext> OnChoiceApplied;
        public event Action<ActiveDelayedConsequence, EventContext> OnDelayedConsequenceResolved;
        /// <summary>Fired when a scheduled narrative event is dequeued and triggered on its day.</summary>
        public event Action<ScheduledEvent, GameEvent, EventContext> OnScheduledEventFired;

        public float DefaultCooldownHours = 24f;

        public void SetPool(IReadOnlyList<GameEvent> pool)
        {
            _pool.Clear();
            if (pool != null)
            {
                _pool.AddRange(pool);
            }
        }

        public bool CanTrigger(GameEvent gameEvent, EventContext context)
        {
            if (gameEvent == null || string.IsNullOrEmpty(gameEvent.id)) return false;

            if (_cooldowns.TryGetValue(gameEvent.id, out float remaining) && remaining > 0f)
            {
                return false;
            }

            return gameEvent.CanTrigger(context);
        }

        public GameEvent SelectEvent(EventContext context)
        {
            if (_pool.Count == 0) return null;

            List<GameEvent> validEvents = new List<GameEvent>();
            float totalWeight = 0f;

            for (int i = 0; i < _pool.Count; i++)
            {
                var ev = _pool[i];
                if (CanTrigger(ev, context))
                {
                    validEvents.Add(ev);
                    totalWeight += Mathf.Max(0.01f, ev.weight);
                }
            }

            if (validEvents.Count == 0 || totalWeight <= 0f) return null;

            double roll = context?.Random != null ? context.Random.NextDouble() * totalWeight : UnityEngine.Random.Range(0f, totalWeight);
            float accum = 0f;

            for (int i = 0; i < validEvents.Count; i++)
            {
                var ev = validEvents[i];
                accum += Mathf.Max(0.01f, ev.weight);
                if (roll <= accum)
                {
                    return ev;
                }
            }

            return validEvents[0];
        }

        /// <summary>
        /// Choices actually offered: drops trait/trust/flag gates and BeliefCheck.HideIfFails.
        /// Callers presenting choices should use this (or <see cref="GetPresentedChoices"/>)
        /// instead of iterating gameEvent.choices directly.
        /// </summary>
        public static List<EventChoice> GetAvailableChoices(GameEvent gameEvent, EventContext context)
        {
            var presented = GetPresentedChoices(gameEvent, context);
            var result = new List<EventChoice>();
            for (int i = 0; i < presented.Count; i++)
            {
                if (presented[i] != null && presented[i].IsAvailable && !presented[i].IsHidden)
                    result.Add(presented[i].Choice);
            }
            return result;
        }

        /// <summary>
        /// Full presentation list: available, grayed-out (gate fail + HideIfGatesFail=false),
        /// or omitted when hidden. Powers branching event UI.
        /// </summary>
        public static List<PresentedEventChoice> GetPresentedChoices(GameEvent gameEvent, EventContext context)
        {
            var result = new List<PresentedEventChoice>();
            if (gameEvent?.choices == null) return result;

            var survivor = context?.PrimarySurvivor;
            for (int i = 0; i < gameEvent.choices.Count; i++)
            {
                var choice = gameEvent.choices[i];
                if (choice == null) continue;

                bool beliefOk = choice.PassesBeliefCheck(survivor);
                bool beliefBlocks = choice.BeliefCheck != null
                    && choice.BeliefCheck.HideIfFails
                    && !beliefOk;
                bool traitTrustOk = choice.PassesTraitAndTrustGates(context);

                string failReason = null;
                if (!traitTrustOk)
                {
                    if (!string.IsNullOrEmpty(choice.RequiredTrait)
                        && (context == null || !context.HasTraitInBunker(choice.RequiredTrait)))
                        failReason = $"Requires {choice.RequiredTrait} in the bunker.";
                    else if (!string.IsNullOrEmpty(choice.RequiredTrustFactionId))
                        failReason = "Faction trust gate not met.";
                    else if (choice.RequiredEventFlags != null && choice.RequiredEventFlags.Count > 0)
                        failReason = "Missing event flag prerequisite.";
                    else
                        failReason = "Gate not met.";
                }
                else if (beliefBlocks)
                {
                    failReason = "Belief check failed.";
                }

                bool gatesOk = traitTrustOk && !beliefBlocks;
                if (!gatesOk && choice.HideIfGatesFail)
                {
                    // Hidden — not added (or add as Hidden for debug consumers).
                    result.Add(new PresentedEventChoice
                    {
                        Choice = choice,
                        IsAvailable = false,
                        IsGrayedOut = false,
                        IsHidden = true,
                        GateFailReason = failReason
                    });
                    continue;
                }

                result.Add(new PresentedEventChoice
                {
                    Choice = choice,
                    IsAvailable = gatesOk,
                    IsGrayedOut = !gatesOk,
                    IsHidden = false,
                    GateFailReason = failReason
                });
            }
            return result;
        }

        /// <summary>Visible rows only (available + grayed), never hidden.</summary>
        public static List<PresentedEventChoice> GetVisibleChoices(GameEvent gameEvent, EventContext context)
        {
            var all = GetPresentedChoices(gameEvent, context);
            var visible = new List<PresentedEventChoice>();
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i] != null && !all[i].IsHidden)
                    visible.Add(all[i]);
            }
            return visible;
        }

        /// <summary>
        /// Find a choice by id among available (non-hidden, gate-passed) options.
        /// </summary>
        public static EventChoice FindAvailableChoice(GameEvent gameEvent, EventContext context, string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) return null;
            var available = GetAvailableChoices(gameEvent, context);
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i] != null && available[i].ChoiceId == choiceId)
                    return available[i];
            }
            return null;
        }

        /// <summary>
        /// Belief-weighted auto-selection among a game event's choices, for callers that
        /// pick on a survivor's behalf (e.g. AI-controlled companions) rather than
        /// presenting a player with a menu. Choices whose BeliefCheck passes get their
        /// weight scaled by BeliefCheck.WeightMultiplier — this is how a Paranoid
        /// survivor "demands iodine, just in case." Not used for player-facing choice UI.
        /// </summary>
        public static EventChoice PickWeightedChoice(GameEvent gameEvent, EventContext context, System.Random rng)
        {
            var choices = GetAvailableChoices(gameEvent, context);
            if (choices.Count == 0) return null;

            var survivor = context?.PrimarySurvivor;
            float totalWeight = 0f;
            var weights = new float[choices.Count];
            for (int i = 0; i < choices.Count; i++)
            {
                float weight = 1f;
                var check = choices[i].BeliefCheck;
                if (check != null && choices[i].PassesBeliefCheck(survivor))
                {
                    weight *= Mathf.Max(0.01f, check.WeightMultiplier);
                }
                weights[i] = weight;
                totalWeight += weight;
            }

            if (totalWeight <= 0f) return choices[0];

            double roll = rng != null ? rng.NextDouble() * totalWeight : UnityEngine.Random.Range(0f, totalWeight);
            float accum = 0f;
            for (int i = 0; i < choices.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum)
                {
                    return choices[i];
                }
            }
            return choices[choices.Count - 1];
        }

        public void Run(GameEvent gameEvent, EventContext context = null)
        {
            if (gameEvent == null) return;
            _cooldowns[gameEvent.id] = DefaultCooldownHours;
            gameEvent.Apply();
            OnEventTriggered?.Invoke(gameEvent, context);
        }

        // ── Scheduled narrative event queue (Prompt #43) ──────────────────

        /// <summary>
        /// Enqueue a GameEvent by id to fire on a specific campaign day.
        /// Duplicate schedules for the same eventId+day are silently ignored.
        /// </summary>
        public void ScheduleEvent(string eventId, int executeOnDay, string originFlag = null)
        {
            if (string.IsNullOrEmpty(eventId) || executeOnDay <= 0) return;
            // Prevent duplicate scheduling of the same event on the same day.
            for (int i = 0; i < _scheduledEvents.Count; i++)
            {
                if (_scheduledEvents[i].EventId == eventId && _scheduledEvents[i].ExecuteOnDay == executeOnDay)
                    return;
            }
            _scheduledEvents.Add(new ScheduledEvent(eventId, executeOnDay, originFlag));
        }

        /// <summary>
        /// Called once per campaign day. Dequeues and fires all ScheduledEvents
        /// whose ExecuteOnDay == currentDay. Events are looked up by id in the pool.
        /// </summary>
        public void TickDay(int currentDay, EventContext context = null)
        {
            if (_scheduledEvents.Count == 0) return;

            for (int i = _scheduledEvents.Count - 1; i >= 0; i--)
            {
                var scheduled = _scheduledEvents[i];
                if (scheduled.ExecuteOnDay != currentDay) continue;

                _scheduledEvents.RemoveAt(i);

                // Propagate the origin flag into context before triggering.
                if (context != null && !string.IsNullOrEmpty(scheduled.OriginFlag))
                    context.SetFlag(scheduled.OriginFlag, true);

                // Look up the GameEvent in the pool.
                GameEvent gameEvent = FindInPool(scheduled.EventId);

                // Even if not in pool, raise the signal so tests / bootstrap can hear it.
                OnScheduledEventFired?.Invoke(scheduled, gameEvent, context);

                if (gameEvent != null)
                {
                    Run(gameEvent, context);
                }
                else
                {
                    // Raise a bare EventBus signal so systems can react even without a pool entry.
                    // (e.g., bootstrap injects an ad-hoc GameEvent for the chain part.)
                    UnityEngine.Debug.LogWarning(
                        $"[EventRunner] Scheduled event '{scheduled.EventId}' fired on day {currentDay} " +
                        "but was not found in the pool. Wiring OnScheduledEventFired only.");
                }
            }
        }

        /// <summary>Find a GameEvent by id in the current pool (null if not present).</summary>
        public GameEvent FindInPool(string eventId)
        {
            if (string.IsNullOrEmpty(eventId)) return null;
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] != null && _pool[i].id == eventId)
                    return _pool[i];
            }
            return null;
        }

        // ── Save / restore for the scheduled-event queue ──────────────────

        public ScheduledEventSave CaptureScheduledState()
        {
            return new ScheduledEventSave { Queue = _scheduledEvents.ToArray() };
        }

        public void RestoreScheduledState(ScheduledEventSave save)
        {
            _scheduledEvents.Clear();
            if (save?.Queue == null) return;
            for (int i = 0; i < save.Queue.Length; i++)
            {
                if (!string.IsNullOrEmpty(save.Queue[i].EventId))
                    _scheduledEvents.Add(save.Queue[i]);
            }
        }

        public void ApplyChoice(GameEvent gameEvent, EventChoice choice, EventContext context)
        {
            if (choice == null || context == null) return;

            // Trait / trust / eventFlag gates — never apply a blocked or grayed choice.
            if (!choice.PassesAllGates(context))
                return;

            // Stateful eventFlags (prerequisites for future events / SaveSystem).
            if (choice.SetEventFlags != null)
            {
                for (int i = 0; i < choice.SetEventFlags.Count; i++)
                {
                    string flag = choice.SetEventFlags[i];
                    if (!string.IsNullOrEmpty(flag))
                        context.SetEventFlag(flag, true);
                }
            }

            // Apply immediate choice effects (with scheduling support — Prompt #43)
            if (choice.Effects != null)
            {
                for (int i = 0; i < choice.Effects.Count; i++)
                {
                    ApplyEffect(choice.Effects[i], context, enableScheduling: true);
                }
            }

            // Apply Morale Delta
            if (choice.MoraleDelta != 0f && context.PrimarySurvivor != null)
            {
                context.PrimarySurvivor.Needs.Morale = Mathf.Clamp(context.PrimarySurvivor.Needs.Morale + choice.MoraleDelta, 0f, 100f);
            }

            // Register Delayed Consequence if present
            if (choice.DelayedConsequence != null && choice.DelayedConsequence.DelayHours > 0f)
            {
                _activeConsequences.Add(new ActiveDelayedConsequence
                {
                    EventId = gameEvent != null ? gameEvent.id : "unknown",
                    ChoiceId = choice.ChoiceId,
                    RemainingHours = choice.DelayedConsequence.DelayHours,
                    Consequence = choice.DelayedConsequence
                });
            }

            OnChoiceApplied?.Invoke(gameEvent, choice, context);
        }

        public void Tick(float gameHours, EventContext context = null)
        {
            if (gameHours <= 0f) return;

            // Decrement cooldown timers
            List<string> keys = new List<string>(_cooldowns.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                float remaining = _cooldowns[key] - gameHours;
                if (remaining <= 0f)
                {
                    _cooldowns.Remove(key);
                }
                else
                {
                    _cooldowns[key] = remaining;
                }
            }

            // Decrement active delayed consequences
            for (int i = _activeConsequences.Count - 1; i >= 0; i--)
            {
                var active = _activeConsequences[i];
                active.RemainingHours -= gameHours;
                if (active.RemainingHours <= 0f)
                {
                    _activeConsequences.RemoveAt(i);
                    if (active.Consequence != null && active.Consequence.Effects != null && context != null)
                    {
                        for (int j = 0; j < active.Consequence.Effects.Count; j++)
                        {
                            ApplyEffect(active.Consequence.Effects[j], context);
                        }
                    }
                    OnDelayedConsequenceResolved?.Invoke(active, context);
                }
            }
        }

        private static void ApplyEffect(EventEffect effect, EventContext context)
        {
            if (effect == null || context == null) return;

            // Apply Need Delta
            if (!string.IsNullOrEmpty(effect.TargetNeed) && context.PrimarySurvivor != null)
            {
                switch (effect.TargetNeed.ToLowerInvariant())
                {
                    case "hunger": context.PrimarySurvivor.Needs.Hunger = Mathf.Clamp(context.PrimarySurvivor.Needs.Hunger + effect.NeedDelta, 0f, 100f); break;
                    case "thirst": context.PrimarySurvivor.Needs.Thirst = Mathf.Clamp(context.PrimarySurvivor.Needs.Thirst + effect.NeedDelta, 0f, 100f); break;
                    case "fatigue": context.PrimarySurvivor.Needs.Fatigue = Mathf.Clamp(context.PrimarySurvivor.Needs.Fatigue + effect.NeedDelta, 0f, 100f); break;
                    case "warmth": context.PrimarySurvivor.Needs.Warmth = Mathf.Clamp(context.PrimarySurvivor.Needs.Warmth + effect.NeedDelta, 0f, 100f); break;
                    case "morale": context.PrimarySurvivor.Needs.Morale = Mathf.Clamp(context.PrimarySurvivor.Needs.Morale + effect.NeedDelta, 0f, 100f); break;
                    case "health": context.PrimarySurvivor.Needs.Health = Mathf.Clamp(context.PrimarySurvivor.Needs.Health + effect.NeedDelta, 0f, 100f); break;
                    case "radiation": context.PrimarySurvivor.RadiationDose = Mathf.Clamp(context.PrimarySurvivor.RadiationDose + effect.NeedDelta, 0f, 100f); break;
                    // Mental-status cures: a talk/comfort event effect. 0..1 range,
                    // unlike the 0..100 Needs fields above, so clamp separately.
                    case "radiationanxiety": context.PrimarySurvivor.RadiationAnxiety = Mathf.Clamp01(context.PrimarySurvivor.RadiationAnxiety + effect.NeedDelta); break;
                    case "numbness": context.PrimarySurvivor.Numbness = Mathf.Clamp01(context.PrimarySurvivor.Numbness + effect.NeedDelta); break;
                }
            }

            // Apply Inventory changes
            if (!string.IsNullOrEmpty(effect.ItemId) && context.Inventory != null)
            {
                var itemDef = ScriptableObject.CreateInstance<Inventory.ItemDefinition>();
                itemDef.id = effect.ItemId;
                itemDef.displayName = effect.ItemId;

                if (effect.ItemAmount > 0)
                {
                    context.Inventory.Add(itemDef, effect.ItemAmount);
                }
                else if (effect.ItemAmount < 0)
                {
                    context.Inventory.Remove(itemDef, Math.Abs(effect.ItemAmount));
                }
            }

            // Apply World Flags
            if (!string.IsNullOrEmpty(effect.SetWorldFlag))
            {
                context.SetFlag(effect.SetWorldFlag, effect.WorldFlagValue);
            }

            // Apply Interpersonal Affinity (Prompt #29). SurvivorAId empty
            // means the primary survivor; SurvivorBId must resolve to a
            // survivor in the EventContext's AllSurvivors list.
            if (effect.AffinityDelta != 0f && context.MentalBreak != null
                && context.PrimarySurvivor != null)
            {
                string aId = string.IsNullOrEmpty(effect.SurvivorAId)
                    ? context.PrimarySurvivor.Id
                    : effect.SurvivorAId;
                string bId = effect.SurvivorBId;
                if (!string.IsNullOrEmpty(aId) && !string.IsNullOrEmpty(bId) && aId != bId)
                {
                    context.MentalBreak.Affinity.Adjust(aId, bId, effect.AffinityDelta);
                }
            }
        }

        // ApplyEffect is static but scheduling requires the runner instance.
        // For the scheduleEvent effect we take a runner ref from the ApplyChoice call path.
        // Static overload preserved for backward compat; instance overload adds scheduling.
        private void ApplyEffect(EventEffect effect, EventContext context, bool enableScheduling)
        {
            ApplyEffect(effect, context);

            // Deferred narrative chain scheduling (Prompt #43 — Outcome.scheduleEvent)
            if (enableScheduling && effect != null && !string.IsNullOrEmpty(effect.ScheduleEventId))
            {
                ScheduleEvent(effect.ScheduleEventId, effect.ScheduleOnDay, effect.SetWorldFlag);
            }
        }

        // -----------------------------------------------------------------
        // Diegetic journal / discovery tutorial (no UI popups)
        // -----------------------------------------------------------------

        /// <summary>
        /// Scan world state and, for each new discovery, generate a
        /// <see cref="JournalEntry"/> in a survivor's voice. First-time only
        /// (KnowledgeBase). Returns how many entries were written this call.
        /// </summary>
        public int ObserveDiscoveries(JournalSystem journal, EventContext context)
        {
            if (journal == null || context == null) return 0;
            int added = 0;

            // high_co2 — Atmosphere system (#20) foul air / diesel CO
            float air = context.Shelter != null ? context.Shelter.AirQuality : 100f;
            bool foulAir = air <= SleepQualitySystem.HighCo2AirQualityThreshold
                || context.CarbonMonoxidePpm >= SleepQualitySystem.HighCo2PpmThreshold;
            if (foulAir && TryRecordDiscovery(journal, KnowledgeKeys.HighCo2, context) != null)
                added++;

            // has_seen_radiation — first meaningful dose
            var doseAuthor = PickAuthor(context);
            if (doseAuthor != null && doseAuthor.RadiationDose >= 5f
                && TryRecordDiscovery(journal, KnowledgeKeys.HasSeenRadiation, context, doseAuthor) != null)
                added++;

            // has_experienced_storm
            if (context.IsFalloutStorm
                && TryRecordDiscovery(journal, KnowledgeKeys.HasExperiencedStorm, context) != null)
                added++;

            // filter_failing — air filtration degrading but not yet full high_co2
            if (context.Shelter != null)
            {
                var airMod = context.Shelter.GetModule("air_filtration");
                if (airMod != null && airMod.IsOperational && airMod.FilterHealth > 0f
                    && airMod.FilterHealth <= 40f
                    && airMod.FilterHealth > SleepQualitySystem.HighCo2AirQualityThreshold
                    && TryRecordDiscovery(journal, KnowledgeKeys.FilterFailing, context) != null)
                    added++;
            }

            // freezing_shelter
            if (context.IndoorTemperatureC <= SleepQualitySystem.FreezingTempC + 0.001f
                && TryRecordDiscovery(journal, KnowledgeKeys.FreezingShelter, context) != null)
                added++;

            return added;
        }

        /// <summary>
        /// Generate a journal entry for a knowledge key if not yet known.
        /// EventRunner owns the generation path; JournalSystem owns storage.
        /// </summary>
        public JournalEntry TryRecordDiscovery(
            JournalSystem journal,
            string knowledgeKey,
            EventContext context,
            Survivor authorOverride = null)
        {
            if (journal == null || context == null || string.IsNullOrEmpty(knowledgeKey))
                return null;
            var author = authorOverride ?? PickAuthor(context);
            return journal.TryDiscover(
                knowledgeKey,
                author,
                context.CurrentDay,
                context.CurrentHour);
        }

        /// <summary>Prefer primary alive survivor; else first alive in AllSurvivors.</summary>
        public static Survivor PickAuthor(EventContext context)
        {
            if (context == null) return null;
            if (context.PrimarySurvivor != null && context.PrimarySurvivor.IsAlive)
                return context.PrimarySurvivor;
            if (context.AllSurvivors == null) return null;
            for (int i = 0; i < context.AllSurvivors.Count; i++)
            {
                var s = context.AllSurvivors[i];
                if (s != null && s.IsAlive) return s;
            }
            return context.PrimarySurvivor;
        }

        // -----------------------------------------------------------------
        // Factories — trait/trust-gated branching events
        // -----------------------------------------------------------------

        public const string EmissaryEventId = "the_emissary";
        public const string EmissaryFactionId = "scavenger_camp";
        public const string EmissaryLieChoiceId = "lie_purifier_broken";
        public const string EmissaryFireChoiceId = "preemptive_fire_hatch";
        public const string EmissaryShareChoiceId = "share_water";
        public const string EmissaryRefuseChoiceId = "refuse_water";

        public const string FlagSharedWaterWithEmissary = "shared_water_with_emissary";
        public const string FlagLiedPurifierBroken = "lied_purifier_broken";
        public const string FlagFiredOnEmissary = "fired_on_emissary_hatch";
        public const string FlagRefusedEmissaryWater = "refused_emissary_water";

        /// <summary>
        /// Faction emissary at the hatch demanding water.
        /// Variance: Paranoid + trust ≥ -20 → lie about the purifier (no water cost, no trust hit).
        /// Paranoid + trust &lt; -20 → preemptive fire through the hatch (replaces the lie).
        /// Choices inject eventFlags for future narrative gates.
        /// </summary>
        public static GameEvent CreateEmissaryEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EmissaryEventId;
            ev.title = "The Emissary";
            ev.bodyText =
                "Someone from the scavenger camp stands at the hatch. Hands empty, voice dry. " +
                "They want water — enough for three, they say. The canteen at their hip is dented and light.";
            ev.threateningBodyText =
                "The same voice at the hatch, but the tone has changed. Not asking. " +
                "They know what you have. The words are short: open up, or they come back with friends.";
            ev.threateningFactionId = fid;
            ev.threateningTrustBelow = -20f;
            ev.weight = 1.5f;
            ev.conditions = new EventConditions { MinDay = 5 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = EmissaryShareChoiceId,
                    Text = "Pass a jug of clean water through the hatch.",
                    MoraleDelta = 4f,
                    FactionId = fid,
                    TrustDelta = 15f,
                    RelationshipDelta = 15f,
                    SetEventFlags = new List<string> { FlagSharedWaterWithEmissary },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water", ItemAmount = -1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = EmissaryRefuseChoiceId,
                    Text = "Keep the seal. Tell them we have nothing to spare.",
                    MoraleDelta = -3f,
                    FactionId = fid,
                    TrustDelta = -12f,
                    RelationshipDelta = -12f,
                    SetEventFlags = new List<string> { FlagRefusedEmissaryWater }
                },
                // Variance 1: Paranoid crew, non-hostile trust — lie, keep water, no trust penalty.
                new EventChoice
                {
                    ChoiceId = EmissaryLieChoiceId,
                    Text = "Lie and say the purifier is broken.",
                    MoraleDelta = -1f,
                    FactionId = fid,
                    TrustDelta = 0f,
                    RequiredTrait = "Paranoid",
                    RequiredTrustFactionId = fid,
                    RequiredTrustMin = -20f, // trust >= -20
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagLiedPurifierBroken }
                },
                // Variance 2: Paranoid + trust < -20 — open fire (replaces the lie via mutual gates).
                new EventChoice
                {
                    ChoiceId = EmissaryFireChoiceId,
                    Text = "Preemptively open fire through the hatch.",
                    MoraleDelta = -12f,
                    FactionId = fid,
                    TrustDelta = -40f,
                    RelationshipDelta = -40f,
                    RequiredTrait = "Paranoid",
                    RequiredTrustFactionId = fid,
                    RequiredTrustMaxExclusive = -20f, // trust < -20
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagFiredOnEmissary }
                }
            };
            return ev;
        }
    }
}
