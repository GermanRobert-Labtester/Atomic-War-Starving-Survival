using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {

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

    }
}
