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

        public IReadOnlyList<GameEvent> Pool => _pool;
        public IReadOnlyList<ActiveDelayedConsequence> ActiveConsequences => _activeConsequences;

        public event Action<GameEvent, EventContext> OnEventTriggered;
        public event Action<GameEvent, EventChoice, EventContext> OnChoiceApplied;
        public event Action<ActiveDelayedConsequence, EventContext> OnDelayedConsequenceResolved;

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
        /// Choices actually offered to the given survivor: drops any choice whose
        /// BeliefCheck.HideIfFails is true and the check fails. Implements "a Denialist
        /// may not even see the 'wear the suit' option as costly." Callers presenting
        /// choices should use this instead of iterating gameEvent.choices directly.
        /// </summary>
        public static List<EventChoice> GetAvailableChoices(GameEvent gameEvent, EventContext context)
        {
            var result = new List<EventChoice>();
            if (gameEvent?.choices == null) return result;

            var survivor = context?.PrimarySurvivor;
            for (int i = 0; i < gameEvent.choices.Count; i++)
            {
                var choice = gameEvent.choices[i];
                if (choice == null) continue;
                if (choice.BeliefCheck != null && choice.BeliefCheck.HideIfFails && !choice.PassesBeliefCheck(survivor))
                {
                    continue;
                }
                result.Add(choice);
            }
            return result;
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

        public void ApplyChoice(GameEvent gameEvent, EventChoice choice, EventContext context)
        {
            if (choice == null || context == null) return;

            // Apply immediate choice effects
            if (choice.Effects != null)
            {
                for (int i = 0; i < choice.Effects.Count; i++)
                {
                    ApplyEffect(choice.Effects[i], context);
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
    }
}
