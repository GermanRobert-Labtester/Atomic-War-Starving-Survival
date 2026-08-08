using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
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
                    case "health":
                        // MISC-006 — prefer NeedsSystem so EvaluateDeath still runs.
                        if (context.NeedsSystem != null)
                            context.NeedsSystem.AdjustHealth(context.PrimarySurvivor, effect.NeedDelta);
                        else
                            AtomicWar._Game.Survivors.SurvivorNeedWrite.AdjustHealth(
                                context.PrimarySurvivor, effect.NeedDelta);
                        break;
                    case "radiation":
                        // MISC-007 — prefer RadiationSystem so OnDoseChanged fires.
                        if (context.RadiationSystem != null)
                            context.RadiationSystem.AdjustDose(context.PrimarySurvivor, effect.NeedDelta);
                        else
                            context.PrimarySurvivor.RadiationDose = Mathf.Clamp(
                                context.PrimarySurvivor.RadiationDose + effect.NeedDelta, 0f, 100f);
                        break;
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
    }
}
