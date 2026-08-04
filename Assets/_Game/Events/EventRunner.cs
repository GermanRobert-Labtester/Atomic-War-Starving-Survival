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

        // Hot-path scratch buffers — avoid per-tick / per-SelectEvent GC.
        private readonly List<GameEvent> _selectValidBuffer = new List<GameEvent>(32);
        private readonly List<string> _cooldownKeyBuffer = new List<string>(16);

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

            var validEvents = _selectValidBuffer;
            validEvents.Clear();
            float totalWeight = 0f;

            for (int i = 0; i < _pool.Count; i++)
            {
                var ev = _pool[i];
                // weight <= 0 = scheduled-only / tracker-fired — never random-pick.
                if (ev == null || ev.weight <= 0f) continue;
                if (!CanTrigger(ev, context)) continue;

                validEvents.Add(ev);
                totalWeight += ev.weight;
            }

            if (validEvents.Count == 0 || totalWeight <= 0f) return null;

            double roll = context?.Random != null ? context.Random.NextDouble() * totalWeight : UnityEngine.Random.Range(0f, totalWeight);
            float accum = 0f;

            for (int i = 0; i < validEvents.Count; i++)
            {
                var ev = validEvents[i];
                accum += ev.weight;
                if (roll <= accum)
                {
                    return ev;
                }
            }

            return validEvents[validEvents.Count - 1];
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
        /// If the event's <see cref="GameEvent.CanTrigger"/> fails (missing eventFlags,
        /// day/hour gates, etc.), it is dequeued without presenting.
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

                // Flag / condition gates on multi-stage arcs (eventFlags, minDay, …).
                if (gameEvent != null && context != null && !gameEvent.CanTrigger(context))
                {
                    UnityEngine.Debug.Log(
                        $"[EventRunner] Scheduled event '{scheduled.EventId}' on day {currentDay} " +
                        "skipped — CanTrigger failed (eventFlags / conditions).");
                    OnScheduledEventFired?.Invoke(scheduled, null, context);
                    continue;
                }

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

            // Prompt #46 — radio intel side effects live on the runner so unit
            // tests (and any host without GameBootstrap) still flip reliability.
            ApplySafeHavenIntelEffects(gameEvent, choice, context);

            OnChoiceApplied?.Invoke(gameEvent, choice, context);
        }

        /// <summary>
        /// Safe Haven Broadcast intel variance (Prompt #46). Analyze choices
        /// mark the loop as a Trap; send_expedition leaves the reliability as-is
        /// so hosts can decide whether to inject the ambush encounter.
        /// </summary>
        public static void ApplySafeHavenIntelEffects(
            GameEvent gameEvent,
            EventChoice choice,
            EventContext context)
        {
            if (gameEvent == null || choice == null || context == null) return;
            if (gameEvent.id != SafeHavenBroadcastEventId) return;

            string id = choice.ChoiceId ?? string.Empty;
            if (id == "analyze_audio" || id == "analyze_audio_science")
            {
                // Scrubber hum is a recorded loop — verified as trap.
                context.ActiveIntelReliability = IntelReliability.Trap;
                context.SetEventFlag(FlagSafeHavenVerified, true);
            }
        }

        /// <summary>
        /// True when send_expedition on Safe Haven should inject the sniper
        /// ambush: player did not analyze first (reliability still Unverified).
        /// After analyze, reliability is Trap and the empty-cache outcome is earned.
        /// </summary>
        public static bool ShouldInjectSafeHavenAmbush(EventContext context)
        {
            if (context == null) return true;
            // Analyzed → Trap reliability → empty cache, no ambush inject.
            if (context.ActiveIntelReliability == IntelReliability.Trap) return false;
            if (context.HasEventFlag(FlagSafeHavenVerified)) return false;
            return true;
        }

        public void Tick(float gameHours, EventContext context = null)
        {
            if (gameHours <= 0f) return;

            // Decrement cooldown timers (reuse key buffer — no per-tick List alloc).
            if (_cooldowns.Count > 0)
            {
                _cooldownKeyBuffer.Clear();
                foreach (var key in _cooldowns.Keys)
                    _cooldownKeyBuffer.Add(key);
                for (int i = 0; i < _cooldownKeyBuffer.Count; i++)
                {
                    string key = _cooldownKeyBuffer[i];
                    float remaining = _cooldowns[key] - gameHours;
                    if (remaining <= 0f)
                        _cooldowns.Remove(key);
                    else
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
                int day = ResolveScheduleDay(effect, context);
                if (day > 0)
                    ScheduleEvent(effect.ScheduleEventId, day, effect.SetWorldFlag);
            }
        }

        /// <summary>
        /// Absolute ScheduleOnDay, or CurrentDay + ScheduleDelayDays when delay &gt; 0.
        /// </summary>
        public static int ResolveScheduleDay(EventEffect effect, EventContext context)
        {
            if (effect == null) return 0;
            if (effect.ScheduleDelayDays > 0)
            {
                int baseDay = context != null ? Mathf.Max(1, context.CurrentDay) : 1;
                return baseDay + effect.ScheduleDelayDays;
            }
            return effect.ScheduleOnDay;
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

        // Multi-stage follow-ups (Prompt #43)
        public const string EmissaryReturnFavorId = "emissary_return_favor";
        public const string EmissaryReturnCaughtId = "emissary_return_caught";
        public const string EmissaryReturnGrudgeId = "emissary_return_grudge";
        public const string EmissaryReturnRaidWarningId = "emissary_return_raid_warning";

        public const int EmissaryFavorDelayDays = 2;
        public const int EmissaryCaughtDelayDays = 2;
        public const int EmissaryGrudgeDelayDays = 3;
        public const int EmissaryRaidWarningDelayDays = 1;

        public const string FlagSharedWaterWithEmissary = "shared_water_with_emissary";
        public const string FlagLiedPurifierBroken = "lied_purifier_broken";
        public const string FlagFiredOnEmissary = "fired_on_emissary_hatch";
        public const string FlagRefusedEmissaryWater = "refused_emissary_water";
        public const string FlagAcceptedEmissaryGift = "accepted_emissary_gift";
        public const string FlagDoubledDownPurifierLie = "doubled_down_purifier_lie";
        public const string FlagAdmittedPurifierLie = "admitted_purifier_lie";

        /// <summary>
        /// Faction emissary at the hatch demanding water.
        /// Variance: Paranoid + trust ≥ -20 → lie about the purifier (no water cost, no trust hit).
        /// Paranoid + trust &lt; -20 → preemptive fire through the hatch (replaces the lie).
        /// Choices inject eventFlags and schedule day-gated follow-ups.
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
                        new EventEffect { ItemId = "clean_water", ItemAmount = -1 },
                        new EventEffect
                        {
                            ScheduleEventId = EmissaryReturnFavorId,
                            ScheduleDelayDays = EmissaryFavorDelayDays,
                            SetWorldFlag = FlagSharedWaterWithEmissary,
                            WorldFlagValue = true
                        }
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
                    SetEventFlags = new List<string> { FlagRefusedEmissaryWater },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect
                        {
                            ScheduleEventId = EmissaryReturnGrudgeId,
                            ScheduleDelayDays = EmissaryGrudgeDelayDays,
                            SetWorldFlag = FlagRefusedEmissaryWater,
                            WorldFlagValue = true
                        }
                    }
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
                    SetEventFlags = new List<string> { FlagLiedPurifierBroken },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect
                        {
                            ScheduleEventId = EmissaryReturnCaughtId,
                            ScheduleDelayDays = EmissaryCaughtDelayDays,
                            SetWorldFlag = FlagLiedPurifierBroken,
                            WorldFlagValue = true
                        }
                    }
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
                    SetEventFlags = new List<string> { FlagFiredOnEmissary },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect
                        {
                            ScheduleEventId = EmissaryReturnRaidWarningId,
                            ScheduleDelayDays = EmissaryRaidWarningDelayDays,
                            SetWorldFlag = FlagFiredOnEmissary,
                            WorldFlagValue = true
                        }
                    }
                }
            };
            return ev;
        }

        /// <summary>
        /// Full emissary multi-stage arc: Part 1 + all day-gated follow-ups
        /// (flag-gated CanTrigger + TraitGates on aftermath choices).
        /// </summary>
        public static List<GameEvent> CreateEmissaryChain(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            return new List<GameEvent>
            {
                CreateEmissaryEvent(fid),
                CreateEmissaryReturnFavorEvent(fid),
                CreateEmissaryReturnCaughtEvent(fid),
                CreateEmissaryReturnGrudgeEvent(fid),
                CreateEmissaryReturnRaidWarningEvent(fid)
            };
        }

        /// <summary>Part 2 after sharing water — they return with a gift.</summary>
        public static GameEvent CreateEmissaryReturnFavorEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EmissaryReturnFavorId;
            ev.title = "The Favor";
            ev.bodyText =
                "Two days later, the same voice at the hatch — softer. A half-crate of canned goods " +
                "sits on the threshold. Payment for the water, they say. No weapons in sight.";
            ev.weight = 0f; // scheduled only
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredEventFlags = new List<string> { FlagSharedWaterWithEmissary }
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "accept_gift",
                    Text = "Take the crate. Nod once. Close the hatch.",
                    MoraleDelta = 6f,
                    FactionId = fid,
                    TrustDelta = 8f,
                    SetEventFlags = new List<string> { FlagAcceptedEmissaryGift },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = 2 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "refuse_gift",
                    Text = "Leave it. We don't take debts we can't see.",
                    MoraleDelta = -2f,
                    FactionId = fid,
                    TrustDelta = -4f
                },
                new EventChoice
                {
                    ChoiceId = "search_first",
                    Text = "Search them for weapons before anything comes in.",
                    MoraleDelta = -1f,
                    FactionId = fid,
                    TrustDelta = -6f,
                    RequiredTrait = "Paranoid",
                    HideIfGatesFail = true
                }
            };
            return ev;
        }

        /// <summary>Part 2 after lying about the purifier — they brought a mechanic.</summary>
        public static GameEvent CreateEmissaryReturnCaughtEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EmissaryReturnCaughtId;
            ev.title = "The Mechanic";
            ev.bodyText =
                "They came back with a thin man who smells of solder. He listens at the hatch for the " +
                "purifier's hum. The lie is thin now. They wait.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredEventFlags = new List<string> { FlagLiedPurifierBroken }
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "admit_and_share",
                    Text = "Admit it. Pass a jug through and call it a misunderstanding.",
                    MoraleDelta = -4f,
                    FactionId = fid,
                    TrustDelta = 5f,
                    SetEventFlags = new List<string> { FlagAdmittedPurifierLie },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water", ItemAmount = -1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "double_down_lie",
                    Text = "Double down. Blame the filters. Blame the weather. Blame anything.",
                    MoraleDelta = -6f,
                    FactionId = fid,
                    TrustDelta = -18f,
                    RequiredTrait = "Paranoid",
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagDoubledDownPurifierLie }
                },
                new EventChoice
                {
                    ChoiceId = "offer_filter_help",
                    Text = "Offer to check their canteen filter — real help, no water lost.",
                    MoraleDelta = 3f,
                    FactionId = fid,
                    TrustDelta = 10f,
                    RequiredTrait = "Medical",
                    HideIfGatesFail = true
                },
                new EventChoice
                {
                    ChoiceId = "seal_and_wait",
                    Text = "Say nothing. Seal the hatch. Wait them out.",
                    MoraleDelta = -2f,
                    FactionId = fid,
                    TrustDelta = -10f
                }
            };
            return ev;
        }

        /// <summary>Part 2 after refusing water — the tone hardens.</summary>
        public static GameEvent CreateEmissaryReturnGrudgeEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EmissaryReturnGrudgeId;
            ev.title = "The Grudge";
            ev.bodyText =
                "Three days. Same hatch. Fewer words. They want water or they want a reason " +
                "to stop asking politely.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredEventFlags = new List<string> { FlagRefusedEmissaryWater }
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "pay_up_late",
                    Text = "Pay up late. One jug, no apology.",
                    MoraleDelta = -2f,
                    FactionId = fid,
                    TrustDelta = 6f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water", ItemAmount = -1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "keep_sealed",
                    Text = "Keep it sealed. Let the grudge sit.",
                    MoraleDelta = 0f,
                    FactionId = fid,
                    TrustDelta = -15f
                }
            };
            return ev;
        }

        /// <summary>Part 2 after opening fire — quiet warning before the world notices.</summary>
        public static GameEvent CreateEmissaryReturnRaidWarningEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? EmissaryFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = EmissaryReturnRaidWarningId;
            ev.title = "After the Hatch";
            ev.bodyText =
                "No knock. Just bootprints in the ash leading away from the hatch, then a radio " +
                "burst on the scavenger band that cuts off mid-word. Someone will come back heavier.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredEventFlags = new List<string> { FlagFiredOnEmissary }
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "brace_hatch",
                    Text = "Brace the hatch. Double the watch.",
                    MoraleDelta = -3f,
                    FactionId = fid,
                    TrustDelta = -5f
                },
                new EventChoice
                {
                    ChoiceId = "leave_it",
                    Text = "Leave it. Hope the ash covers the prints.",
                    MoraleDelta = -8f,
                    FactionId = fid,
                    TrustDelta = -8f
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #46 — Radio-triggered events + Intel reliability variance.
        // The radio airwaves are full of desperate liars: broadcasts that
        // promise a "safe haven" can be a pre-positioned ambush. GameEvents
        // gated on the radio must (a) only fire when a survivor is actively
        // listening (IsOnRadio), and (b) branch on IntelReliability so that
        // sending an expedition on a Trap is a casualty-producing decision.
        // ─────────────────────────────────────────────────────────────────

        public const string SafeHavenBroadcastEventId = "radio_safe_haven_broadcast";

        // Encounter id injected into the ExpeditionSystem's encounter pool when
        // the player launches an expedition on a Trap broadcast. Mirrors
        // NarrativeChainEngine.EncounterIdAmbush but is sourced from the radio
        // pipeline (factionalized "claimed safe haven" → sniper ambush).
        public const string SafeHavenAmbushEncounterId = "enc_safe_haven_ambush";

        // Item id the player must own to unlock the "warn other survivors"
        // choice. Defined in StreamingAssets/items.json; the choice gates on
        // RequiredItemId and pays Power via a downstream System.PayForBroadcast
        // delegate wired by GameBootstrap.
        public const string RadioTransmitterItemId = "radio_transmitter";

        // World flags written by Safe Haven choices. Read by tests and by
        // GameBootstrap when materializing the ambush encounter.
        public const string FlagSafeHavenSentExpedition  = "safe_haven_sent_expedition";
        public const string FlagSafeHavenVerified       = "safe_haven_verified_as_trap";
        public const string FlagSafeHavenBroadcasted    = "safe_haven_warned_others";
        public const string FlagSafeHavenIgnored        = "safe_haven_ignored";

        // Result location id written into the ambush encounter's
        // TargetLocationId; resolved by GameBootstrap when synthesizing the
        // sniper node. Kept in one place so the encounter factory and the
        // location injector agree.
        public const string SafeHavenTargetLocationId  = "safe_haven_20mi_north";

        /// <summary>
        /// Radio-triggered GameEvent: a looped broadcast claims a working
        /// military bunker 20 miles north. Variance:
        ///  - With a high-skill survivor (Medical OR Science) in the bunker,
        ///    an "Analyze the audio background" choice unlocks and reveals the
        ///    scrubber hum as a recorded loop (Verified=Trap, no trust cost).
        ///  - With a <c>radio_transmitter</c> in the bunker, a "Warn other
        ///    wastelanders" choice unlocks, costs power, and raises global
        ///    karma via the PayForBroadcast delegate (verified broadcasts
        ///    only).
        ///  - Sending an expedition on Unverified intel biases the
        ///    ExpeditionSystem toward a sniper ambush encounter
        ///    (<see cref="SafeHavenAmbushEncounterId"/>).
        /// Choice conditions:
        ///  - analyze_audio: RequiredTrait "Medical" or "Science" (gated via
        ///    HideIfGatesFail so the row is hidden when no qualified survivor
        ///    is in the bunker).
        ///  - warn_others: RequiredItemId "radio_transmitter" (gated similarly).
        ///  - send_expedition / ignore: always available.
        /// Trigger: requires the player to be at the radio (IsOnRadio=true on
        /// the EventContext) and the broadcast to be in the Unverified state
        /// (Verified broadcasts re-fire with safer outcomes; Trap broadcasts
        /// never re-fire — the audio analysis is terminal).
        /// </summary>
        public static GameEvent CreateSafeHavenBroadcastEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = SafeHavenBroadcastEventId;
            ev.title = "Safe Haven Broadcast";
            ev.bodyText =
                "A looped broadcast cuts through the static. A woman's voice, calm, almost rehearsed: " +
                "safe haven at grid 4-7-North, twenty miles. Working scrubbers. Hot food. " +
                "Come in on 107.0. The loop is on a six-minute cycle. It does not stutter. " +
                "The background hum — the 'scrubbers' — sits at exactly the same pitch every time.";
            ev.weight = 1.2f;
            ev.conditions = new EventConditions
            {
                MinDay = 31,
                RequiredFlagId = "is_on_radio"
            };
            ev.choices = new List<EventChoice>
            {
                // ── Default: trust the broadcast, send an expedition. ──
                // If the broadcast turns out to be a Trap, GameBootstrap reads
                // FlagSafeHavenSentExpedition + the Unverified reliability on
                // EventContext and injects SafeHavenAmbushEncounterId into
                // ExpeditionSystem.EncouterPool with a heavy weight.
                new EventChoice
                {
                    ChoiceId = "send_expedition",
                    Text = "Pack rucks. Send the team north to grid 4-7.",
                    MoraleDelta = 8f,
                    SetEventFlags = new List<string> { FlagSafeHavenSentExpedition }
                },

                // ── Variance: high-skill survivor at the dial can hear the loop. ──
                // Gates on the union of "Medical" and "Science" trait strings:
                // the bunker needs a medic or a tech to expose the recording.
                // The effect sets FlagSafeHavenVerified and flips the context's
                // ActiveIntelReliability to Trap so subsequent reads of the
                // event inherit the new reliability.
                new EventChoice
                {
                    ChoiceId = "analyze_audio",
                    Text = "Tell the medic to put a stethoscope to the speaker. Tell the tech to spectrum-analyze the hum.",
                    MoraleDelta = -2f,
                    RequiredTrait = "Medical", // OR-gate: see TryRevealTrap below.
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagSafeHavenVerified }
                },
                // Science-only alias: a tech can also do the spectrum analysis
                // alone, but the medic-only row above does not cover them when
                // no medic is in the bunker. Hidden if Medical is present (so
                // we don't double-show); shown if only Science is present.
                // (HideIfGatesFail keeps the union semantics: any qualified
                // survivor in the bunker reveals the row.)
                new EventChoice
                {
                    ChoiceId = "analyze_audio_science",
                    Text = "Run the broadcast through a bandpass filter and a spectrum analyzer.",
                    MoraleDelta = -2f,
                    RequiredTrait = "Science",
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagSafeHavenVerified }
                },

                // ── Variance: radio_transmitter lets the player warn other
                //    wastelanders on the frequency. Costs power; raises global
                //    karma/trust. Gated on the craftable transmitter item.
                new EventChoice
                {
                    ChoiceId = "warn_others",
                    Text = "Use the radio transmitter. Cut into the loop. Tell anyone listening it's a trap.",
                    MoraleDelta = 12f,
                    RequiredItemId = RadioTransmitterItemId,
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagSafeHavenBroadcasted }
                },

                // ── Always-available: ignore the broadcast. ──
                new EventChoice
                {
                    ChoiceId = "ignore_broadcast",
                    Text = "Static and lies. Change the frequency.",
                    MoraleDelta = -1f,
                    SetEventFlags = new List<string> { FlagSafeHavenIgnored }
                }
            };
            return ev;
        }

        /// <summary>
        /// Test/run-time helper: a survivor is qualified to expose the Safe
        /// Haven trap if they have a Medical or Science skill at or above the
        /// standard trait threshold (0.5). Returns the first such survivor in
        /// the bunker, or null. Mirrors the union of the two
        /// RequiredTrait-gated choices on <see cref="CreateSafeHavenBroadcastEvent"/>.
        /// </summary>
        public static Survivor FindSafeHavenAnalyst(IReadOnlyList<Survivor> bunker)
        {
            if (bunker == null) return null;
            for (int i = 0; i < bunker.Count; i++)
            {
                var s = bunker[i];
                if (s == null || !s.IsAlive) continue;
                if (s.MedicalSkill >= EventContext.MedicalSkillTraitThreshold) return s;
                if (s.ScienceSkill  >= EventContext.ScienceSkillTraitThreshold)  return s;
            }
            return null;
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #47 — biological trade economy. When the bunker has
        // nothing left to trade, factions will ask for pieces of the
        // player. "Blood for Water" is the entry point: a heavily-armed
        // medical convoy from a wealthy faction demands O-negative blood
        // for their dying commander. Accepting costs one survivor the
        // BloodLossAffliction and a chance of infection; refusing costs
        // trust and may escalate to a hatch raid.
        //
        // Trait variance:
        //  - Fatalist volunteers outright.
        //  - Paranoid refuses outright (and bleeds the affinity if forced).
        //  - Cautious / Realist / Reckless / Denialist will agree if the
        //    player has a med-skill survivor present to vouch for safety.
        //
        // EventRunner applies the choice effects (inventory + flag), but
        // the actual MedicalSystem.Inflict(...) call lives in
        // GameBootstrap.HandleBloodForWaterChoiceApplied — same hook
        // pattern as Safe Haven. Tests assert the inventory + flag delta;
        // the bootstrap-level integration is exercised by PlayMode tests.
        // ─────────────────────────────────────────────────────────────────

        public const string BloodForWaterEventId = "blood_for_water";

        // Faction id the convoy belongs to. Defaults to the wealthy prepper
        // faction (the doomsday_preppers have stockpiled medicine and would
        // be the natural asker for blood).
        public const string BloodForWaterFactionId = "doomsday_preppers";

        // Reward magnitudes (Prompt #47 acceptance criteria).
        public const int BloodForWaterCleanWaterReward = 10;
        public const int BloodForWaterIodinePillsReward = 5;

        // World flags written by Blood for Water choices. Read by tests
        // and by GameBootstrap to know whether to inject the
        // BloodLossAffliction or slam the affinity matrix.
        public const string FlagBloodDrawn       = "blood_for_water_drawn";
        public const string FlagBloodRefused     = "blood_for_water_refused";
        public const string FlagBloodForced      = "blood_for_water_forced";
        public const string FlagBloodIgnoresSummons = "blood_for_water_ignored";

        // Forced-bleed affinity floor: forcing a Paranoid survivor to give
        // blood slams their affinity with the bunker leader to the bottom
        // of the [-100, +100] scale, which is the input to MentalBreakSystem
        // (Prompt #29) that can fire a ViolentParanoia break.
        public const float ForcedBleedAffinityFloor = -100f;

        /// <summary>
        /// Faction convoy at the hatch demanding O-negative blood for their
        /// dying commander. The four choices are gated by both bunker-level
        /// traits and inventory state — a low-inventory player has no
        /// out-of-blood option, so the trade-off is forced.
        ///
        /// Choice semantics:
        ///  - <c>bleed_willing_survivor</c>: requires a Fatalist, OR a
        ///    non-Paranoid survivor with a medic/tech in the bunker to
        ///    vouch. Reward: 10 clean_water + 5 iodine_pills. Cost:
        ///    BloodLossAffliction on the donor.
        ///  - <c>bleed_paranoid_force</c>: requires a Paranoid survivor in
        ///    the bunker. Reward: 10 clean_water + 5 iodine_pills. Cost:
        ///    BloodLossAffliction + affinity floor (-100) between the
        ///    forced survivor and the bunker leader — MentalBreak risk.
        ///  - <c>refuse_convoy</c>: always available. Cost: -5 trust with
        ///    the convoy's faction.
        ///  - <c>ignore_summons</c>: always available. Cost: -10 trust, no
        ///    reward, no relationship change.
        ///
        /// The event is gated by <c>is_blood_for_water_offered</c> (set by
        /// the bootstrap when a faction at Rob/HostileRaid trade-stance
        /// visits the hatch with an empty inventory). This keeps the event
        /// out of the random pool — it is a faction-triggered event, like
        /// the Emissary.
        /// </summary>
        public static GameEvent CreateBloodForWaterEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId) ? BloodForWaterFactionId : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = BloodForWaterEventId;
            ev.title = "Blood for Water";
            ev.bodyText =
                "Six vehicles. Armored. White markings over rust. A lieutenant at the hatch, " +
                "polite as a knife: their commander is dying of a perforated ulcer and the only " +
                "thing keeping him alive is O-negative whole blood. They have iodine. They have " +
                "clean water in drums. They do not have a donor. The lieutenant does not blink. " +
                "There is enough tubing in the convoy to take a pint from the hatch and put a " +
                "jug of clean water through it. The tubing is not sterile. Nobody in the convoy " +
                "pretends otherwise.";
            ev.weight = 1.3f;
            ev.conditions = new EventConditions
            {
                MinDay = 25,                          // any time the convoy is rolling
                RequiredFlagId = "is_blood_for_water_offered"
            };
            ev.choices = new List<EventChoice>
            {
                // ── Default: trade a pint for water + iodine. The actual
                //    MedicalSystem.Inflict(blood_loss) call lives in the
                //    bootstrap's HandleBloodForWaterChoiceApplied — the
                //    effect here is the inventory + flag delta the runner
                //    can apply directly.
                new EventChoice
                {
                    ChoiceId = "bleed_willing_survivor",
                    Text = "Pick a survivor who can spare the blood. Run the line.",
                    MoraleDelta = -10f,
                    FactionId = fid,
                    TrustDelta = 18f,
                    RequiredTrait = "Fatalist",   // Fatalist volunteers outright
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagBloodDrawn },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water",    ItemAmount = BloodForWaterCleanWaterReward },
                        new EventEffect { ItemId = "iodine_pills",   ItemAmount = BloodForWaterIodinePillsReward }
                    }
                },
                // Alias: non-Fatalist with a medic or tech in the bunker
                // (someone to vouch for the procedure). Hidden if Fatalist
                // already satisfies the row above OR no one is in the bunker.
                new EventChoice
                {
                    ChoiceId = "bleed_willing_survivor_under_care",
                    Text = "A medic or tech supervises. A survivor agrees under care.",
                    MoraleDelta = -6f,
                    FactionId = fid,
                    TrustDelta = 12f,
                    RequiredTrait = "Medical",    // OR-gate: see HasTraitInBunker
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagBloodDrawn },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water",    ItemAmount = BloodForWaterCleanWaterReward },
                        new EventEffect { ItemId = "iodine_pills",   ItemAmount = BloodForWaterIodinePillsReward }
                    }
                },
                // ── Force: a Paranoid survivor is dragged to the line. Reward
                //    same, but the affinity hit is the real cost (MentalBreak
                //    risk).
                new EventChoice
                {
                    ChoiceId = "bleed_paranoid_force",
                    Text = "The Paranoid one will not agree. Hold them down anyway.",
                    MoraleDelta = -22f,
                    FactionId = fid,
                    TrustDelta = 10f,
                    RequiredTrait = "Paranoid",
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { FlagBloodDrawn, FlagBloodForced },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water",    ItemAmount = BloodForWaterCleanWaterReward },
                        new EventEffect { ItemId = "iodine_pills",   ItemAmount = BloodForWaterIodinePillsReward }
                    }
                },

                // ── Refuse: keep the blood, lose trust. The convoy drives
                //    off; the next time they come back the trade-stance
                //    will be one step closer to Rob.
                new EventChoice
                {
                    ChoiceId = "refuse_convoy",
                    Text = "Seal the hatch. The blood stays in the bunker.",
                    MoraleDelta = 2f,
                    FactionId = fid,
                    TrustDelta = -8f,
                    SetEventFlags = new List<string> { FlagBloodRefused }
                },

                // ── Ignore: pretend no one heard the lieutenant. Worst
                //    trust outcome (the convoy returns to a closed hatch).
                new EventChoice
                {
                    ChoiceId = "ignore_summons",
                    Text = "Don't answer. Pretend no one heard.",
                    MoraleDelta = -3f,
                    FactionId = fid,
                    TrustDelta = -14f,
                    SetEventFlags = new List<string> { FlagBloodIgnoresSummons }
                }
            };
            return ev;
        }

        /// <summary>
        /// Test helper: pick the survivor who would be bled if the
        /// <c>bleed_willing_survivor</c> choice resolves right now. Returns
        /// the first living bunker survivor matching the gate priority:
        /// Fatalist first (volunteers outright), then non-Paranoid (a medic
        /// or tech vouching), then null. Mirrors the union of the two
        /// gated rows on <see cref="CreateBloodForWaterEvent"/>.
        /// </summary>
        public static Survivor FindBloodDonor(IReadOnlyList<Survivor> bunker)
        {
            if (bunker == null) return null;
            // 1. Fatalist volunteers.
            for (int i = 0; i < bunker.Count; i++)
            {
                var s = bunker[i];
                if (s == null || !s.IsAlive) continue;
                if (s.RiskBias == RiskBiasTrait.Fatalist) return s;
            }
            // 2. Anyone who is not Paranoid (the medic/tech-row gate
            //    covers this: HasTraitInBunker("Medical") and the survivor
            //    is the donor).
            for (int i = 0; i < bunker.Count; i++)
            {
                var s = bunker[i];
                if (s == null || !s.IsAlive) continue;
                if (s.RiskBias != RiskBiasTrait.Paranoid) return s;
            }
            return null;
        }

        /// <summary>
        /// Test helper: pick the first Paranoid survivor in the bunker.
        /// Used by the <c>bleed_paranoid_force</c> row and by tests that
        /// assert the forced-bleed affinity-floor consequence.
        /// </summary>
        public static Survivor FindParanoidSurvivor(IReadOnlyList<Survivor> bunker)
        {
            if (bunker == null) return null;
            for (int i = 0; i < bunker.Count; i++)
            {
                var s = bunker[i];
                if (s == null || !s.IsAlive) continue;
                if (s.RiskBias == RiskBiasTrait.Paranoid) return s;
            }
            return null;
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #48 — weather-driven hatch entrapment ("Buried Alive")
        // ─────────────────────────────────────────────────────────────────

        public const string BuriedAliveEventId = "buried_alive";
        public const string FactionDigOutEventId = "faction_dig_out";
        public const string ChoiceDigOut = "dig_out";
        public const string ChoiceWaitOutStorm = "wait_out_storm";
        public const string ChoiceAcceptFactionRescue = "accept_faction_rescue";

        /// <summary>
        /// Opening beat of the Buried Alive chain: continuous blizzard sealed
        /// the hatch. Expeditions are hard-locked until DigOut (or outside rescue).
        /// </summary>
        public static GameEvent CreateBuriedAliveEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = BuriedAliveEventId;
            ev.title = "Buried Alive";
            ev.bodyText =
                "The hatch will not open. We are snowed in. " +
                "The wheel turns half a degree and stops. Snow has packed the shaft " +
                "into a single white mass. Outside, the wind is a continuous pressure " +
                "on the metal. No one leaves. No one comes. The air already tastes thinner.";
            ev.weight = 2f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequireExtremeWeather = true,
                RequiredFlagId = "is_buried_alive_offered"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoiceDigOut,
                    Text = "Dig out from the inside. Heavy work. Bad air.",
                    MoraleDelta = -8f,
                    SetEventFlags = new List<string> { "hatch_dig_out_started" }
                },
                new EventChoice
                {
                    ChoiceId = ChoiceWaitOutStorm,
                    Text = "Wait. Conserve air. Hope the filter holds.",
                    MoraleDelta = -4f,
                    SetEventFlags = new List<string> { "hatch_wait_out_storm" }
                }
            };
            return ev;
        }

        /// <summary>
        /// Faction arrives and digs the hatch open from outside — saves lives,
        /// demands a massive debt in return (trust slam + debt flag).
        /// Scheduled when any faction trust is strictly above 80.
        /// </summary>
        public static GameEvent CreateFactionDigOutEvent(string factionId = null)
        {
            string fid = string.IsNullOrEmpty(factionId)
                ? "scavenger_camp"
                : factionId;
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = FactionDigOutEventId;
            ev.title = "Outside the Hatch";
            ev.bodyText =
                "Shovels. Voices. Someone is cutting a path down to the wheel from above. " +
                "They do not ask permission. When the light comes through, the first face " +
                "is not kind. They have the debt ledger open before the snow is cleared. " +
                "You will pay. That is not a request.";
            ev.weight = 1.5f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredFlagId = "faction_dig_out_debt"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = ChoiceAcceptFactionRescue,
                    Text = "Take the debt. Open the hatch.",
                    MoraleDelta = 6f,
                    FactionId = fid,
                    TrustDelta = -45f,
                    SetEventFlags = new List<string> { "faction_dig_out_accepted", "faction_dig_out_debt" }
                }
            };
            return ev;
        }
    }
}
