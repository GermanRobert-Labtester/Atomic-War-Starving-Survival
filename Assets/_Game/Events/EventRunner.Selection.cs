using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
        private PersonalQuestSystem _personalQuests;

        /// <summary>Prompt #221 — Peacekeeper blocks Internal Saboteur / Ration Thief.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        public bool CanTrigger(GameEvent gameEvent, EventContext context)
        {
            if (gameEvent == null || string.IsNullOrEmpty(gameEvent.id)) return false;

            if (_cooldowns.TryGetValue(gameEvent.id, out float remaining) && remaining > 0f)
            {
                return false;
            }

            // Prompt #221 — Peacekeeper present: no Internal Saboteur / Ration Thief events.
            if (_personalQuests != null
                && _personalQuests.BlocksInternalCrimeEvent(gameEvent.id, context?.AllSurvivors))
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

    }
}
