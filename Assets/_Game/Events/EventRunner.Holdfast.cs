using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
        public const string HoldfastClerkEventId = "event_holdfast_the_clerk";
        public const string HoldfastLevyEventId = "event_holdfast_the_levy";
        public const string HoldfastWindowEventId = "event_holdfast_the_window";
        public const string HoldfastHatchEventId = "event_holdfast_the_hatch";
        public const string FlagHoldfastClerkHeard = "holdfast_clerk_heard";
        public const string FlagHoldfastLevyResolved = "holdfast_levy_event_resolved";
        public const string FlagHoldfastWindowSeen = "holdfast_window_seen";
        public const string FlagHoldfastHatchResolved = "holdfast_hatch_resolved";

        public static List<GameEvent> CreateHoldfastEvents()
        {
            return new List<GameEvent>
            {
                CreateHoldfastClerkEvent(),
                CreateHoldfastWindowEvent(),
                CreateHoldfastLevyEvent(),
                CreateHoldfastHatchEvent()
            };
        }

        public static GameEvent CreateHoldfastClerkEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = HoldfastClerkEventId;
            ev.title = "The Return";
            ev.bodyText =
                "Edor stands off the weigh-plate with the pink copy folded against the wind. " +
                "He asks if you want the heading first or the names. He says most people want it read again. He waits.";
            ev.threateningBodyText =
                "He is still off the plate. He does not offer the heading. He says the white copy has already gone north. " +
                "He says he can read you what it will sound like when it comes back. He still does not step onto your threshold.";
            ev.threateningFactionId = "faction_the_office";
            ev.threateningTrustBelow = -20f;
            ev.weight = 1.4f;
            ev.conditions = new EventConditions
            {
                MinDay = 90,
                RequiredFlagId = "exp_holdfast_unlocked",
                BlockedFlagId = FlagHoldfastClerkHeard
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "let_wait_hatch",
                    Text = "Let him wait near the hatch. He keeps off the step with a stove-tin.",
                    MoraleDelta = 0f,
                    SetEventFlags = new List<string> { FlagHoldfastClerkHeard, "holdfast_edor_wait_hatch" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastClerkHeard, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "holdfast_edor_wait_hatch", WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "send_to_boom",
                    Text = "Send him as far as the boom. He will not call it a refusal.",
                    MoraleDelta = -2f,
                    SetEventFlags = new List<string> { FlagHoldfastClerkHeard },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastClerkHeard, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        public static GameEvent CreateHoldfastWindowEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = HoldfastWindowEventId;
            ev.title = "When the Cut Takes";
            ev.bodyText =
                "Yara Holm at the Gate. The boom is up for a freeze that has a length. " +
                "Lit hours on the board. Dark ice is not a metaphor.";
            ev.weight = 1.2f;
            ev.conditions = new EventConditions
            {
                MinDay = 90,
                RequiredFlagId = "ice_road_open",
                BlockedFlagId = FlagHoldfastWindowSeen
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "wait_dark",
                    Text = "Wait out a dark stretch. Hours and cold. The beacon stays honest.",
                    MoraleDelta = -3f,
                    SetEventFlags = new List<string> { FlagHoldfastWindowSeen },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastWindowSeen, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "kit_column",
                    Text = "Kit a column. Glass on faces. The axle ledger takes your mass.",
                    MoraleDelta = 0f,
                    SetEventFlags = new List<string> { FlagHoldfastWindowSeen },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastWindowSeen, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        public static GameEvent CreateHoldfastLevyEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = HoldfastLevyEventId;
            ev.title = "Reconstruction Pool";
            ev.bodyText =
                "Ormund stands with the blotter between you. He turns a page with two fingers. " +
                "He names three of your people by occupation. He says the ice has a length. " +
                "He does not ask if you understand. He waits until you say whether you do.";
            ev.threateningBodyText =
                "The same blotter. The same two fingers. He does not name occupations. He names the hatch. " +
                "He says the quiet interval is forty days and is already counting. He does not say what happens on day forty-one. " +
                "The next form is already in the tray, face down.";
            ev.threateningFactionId = "faction_the_office";
            ev.threateningTrustBelow = -20f;
            ev.weight = 1.6f;
            ev.conditions = new EventConditions
            {
                MinDay = 110,
                RequiredFlagId = "exp_holdfast_unlocked",
                BlockedFlagId = FlagHoldfastLevyResolved
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "holdfast_levy_honour",
                    Text = "Honour the levy. Three names. Thirty days.",
                    MoraleDelta = -6f,
                    SetEventFlags = new List<string> { FlagHoldfastLevyResolved, "holdfast_levy_honour" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastLevyResolved, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "holdfast_levy_honour", WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "holdfast_levy_substitute",
                    Text = "Send three other people. The names will be irregular.",
                    MoraleDelta = -8f,
                    SetEventFlags = new List<string> { FlagHoldfastLevyResolved, "holdfast_levy_substitute" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastLevyResolved, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "holdfast_levy_substitute", WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "holdfast_levy_refuse",
                    Text = "Refuse in writing. Edor waits. No shots.",
                    MoraleDelta = -4f,
                    SetEventFlags = new List<string> { FlagHoldfastLevyResolved, "holdfast_levy_refuse" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastLevyResolved, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "holdfast_levy_refuse", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        public static GameEvent CreateHoldfastHatchEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = HoldfastHatchEventId;
            ev.title = "The Claim, Reversed";
            ev.bodyText =
                "Forms at the outer hatch. Escort in faded Continuity jackets. Temperature. " +
                "The game stops talking. Open or keep shut.";
            ev.threateningBodyText =
                "He names the hatch. The quiet interval is forty days and is already counting. " +
                "The next form is already in the tray, face down.";
            ev.threateningFactionId = "faction_the_office";
            ev.threateningTrustBelow = -20f;
            ev.weight = 1.8f;
            ev.conditions = new EventConditions
            {
                MinDay = 130,
                RequiredFlagId = "holdfast_order_12c",
                BlockedFlagId = FlagHoldfastHatchResolved
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "open_honour",
                    Text = "Open. Honour terms in part. Some live numbered in Block C.",
                    MoraleDelta = -5f,
                    SetEventFlags = new List<string> { FlagHoldfastHatchResolved, "ending_holdfast_schedule" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastHatchResolved, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "ending_holdfast_schedule", WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "keep_shut",
                    Text = "Keep shut. Forty days. No combat. Then a receipt.",
                    MoraleDelta = -8f,
                    SetEventFlags = new List<string> { FlagHoldfastHatchResolved, "ending_holdfast_dark_road" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagHoldfastHatchResolved, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = "ending_holdfast_dark_road", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }
    }
}
