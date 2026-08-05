using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
        public void Tick(float gameHours, EventContext context = null)
        {
            if (gameHours <= 0f) return;
            TickCooldowns(gameHours);
            TickDelayedConsequences(gameHours, context);
        }

        private void TickCooldowns(float gameHours)
        {
            if (_cooldowns.Count == 0) return;

            // Reuse key buffer — no per-tick List alloc.
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

        private void TickDelayedConsequences(float gameHours, EventContext context)
        {
            for (int i = _activeConsequences.Count - 1; i >= 0; i--)
            {
                var active = _activeConsequences[i];
                active.RemainingHours -= gameHours;
                if (active.RemainingHours > 0f) continue;

                _activeConsequences.RemoveAt(i);
                ResolveDelayedConsequence(active, context);
            }
        }

        private void ResolveDelayedConsequence(ActiveDelayedConsequence active, EventContext context)
        {
            if (active.Consequence?.Effects != null && context != null)
            {
                for (int j = 0; j < active.Consequence.Effects.Count; j++)
                    ApplyEffect(active.Consequence.Effects[j], context);
            }
            OnDelayedConsequenceResolved?.Invoke(active, context);
        }

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
                MakeBloodForWaterBleedChoice(new BloodForWaterBleedSpec
                {
                    ChoiceId = "bleed_willing_survivor",
                    Text = "Pick a survivor who can spare the blood. Run the line.",
                    MoraleDelta = -10f,
                    FactionId = fid,
                    TrustDelta = 18f,
                    RequiredTrait = "Fatalist"
                }),
                // Alias: non-Fatalist with a medic or tech in the bunker
                // (someone to vouch for the procedure). Hidden if Fatalist
                // already satisfies the row above OR no one is in the bunker.
                MakeBloodForWaterBleedChoice(new BloodForWaterBleedSpec
                {
                    ChoiceId = "bleed_willing_survivor_under_care",
                    Text = "A medic or tech supervises. A survivor agrees under care.",
                    MoraleDelta = -6f,
                    FactionId = fid,
                    TrustDelta = 12f,
                    RequiredTrait = "Medical"
                }),
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

    }
}
