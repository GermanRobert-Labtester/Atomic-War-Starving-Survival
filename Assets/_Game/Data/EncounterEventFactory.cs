using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Encounter & Event Factory for Prompts #95-#104.
    /// Every method's event id MUST be unique across the whole factory
    /// + the catalog + every Ensure* helper in GameBootstrap. Use
    /// <c>Tools/ASHFALL/Validate Event Ids</c> to verify.
    /// Creates GameEvent instances for expedition encounters and shelter events.
    /// Wired into EventRunner pool by GameBootstrap at initialization.
    /// </summary>
    public static class EncounterEventFactory
    {
        // ───────────────────────────────────────────────────────────────
        // Prompt #95 — Encounter: The Child Sniper
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateChildSniper()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_child_sniper";
            ev.title = "The Child Sniper";
            ev.bodyText =
                "A shot cracks past your ear and buries itself in the ash behind you. " +
                "Through the scope-glint you see them: twelve years old, rifle shaking, " +
                "face set in the blank mask of someone who stopped crying weeks ago.";
            ev.weight = 1.5f;
            ev.conditions = new EventConditions
            {
                MinDay = 10,
                RequiredFlagId = "ring_city_outskirts"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "return_fire",
                    Text = "Return fire. End the threat.",
                    MoraleDelta = -25f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "rifle", ItemAmount = 1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "flee_child_sniper",
                    Text = "Drop half your loot and run.",
                    MoraleDelta = -8f
                    // DropLoot handled by ExpeditionSystem
                },
                new EventChoice
                {
                    ChoiceId = "talk_down",
                    Text = "Lower your weapon. Talk. You were their age once.",
                    MoraleDelta = 5f,
                    RequiredTrait = "Charisma",
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { "child_sniper_spared" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "reveal_2_nodes", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #96 — Encounter: The Feral Dog Pack
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateFeralDogPack()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_dog_pack";
            ev.title = "The Feral Dog Pack";
            ev.bodyText =
                "Four of them. Ribs showing through patchy fur. They don't bark — " +
                "the ones that survived learned not to. They just growl, low and steady, " +
                "guarding a torn-open supply crate they can't open themselves.";
            ev.weight = 1.8f;
            ev.conditions = new EventConditions { MinDay = 5 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "shoot_dogs",
                    Text = "Shoot them. They're suffering anyway.",
                    MoraleDelta = -10f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "ammo_rifle", ItemAmount = -2 },
                        new EventEffect { ItemId = "mutated_meat", ItemAmount = 4 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "throw_food",
                    Text = "Throw them a ration. They need it more.",
                    MoraleDelta = 3f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = -1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "tame_alpha",
                    Text = "Kneel. Let the alpha come to you.",
                    RequiredTrait = "AnimalHandling",
                    HideIfGatesFail = true,
                    MoraleDelta = 10f,
                    SetEventFlags = new List<string> { "dog_recruited" }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #97 — Encounter: The Blind Wanderer
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateBlindWanderer()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_blind_wanderer";
            ev.title = "The Blind Wanderer";
            ev.bodyText =
                "A man sits on a collapsed overpass, legs dangling. His eyes are " +
                "milky white — the flash took them. He heard your boots in the ash. " +
                "\"Is there still a settlement east? Please. I just need to know.\"";
            ev.weight = 1.2f;
            ev.conditions = new EventConditions
            {
                MinDay = 31,
                RequireFalloutStorm = true
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "point_east",
                    Text = "Tell him east. What's left of it.",
                    MoraleDelta = 4f,
                    SetEventFlags = new List<string> { "wanderer_helped" }
                },
                new EventChoice
                {
                    ChoiceId = "lead_him",
                    Text = "Take his arm. Lead him there yourself.",
                    MoraleDelta = 12f
                    // Adds 12h to expedition + rad risk handled by ExpeditionSystem
                },
                new EventChoice
                {
                    ChoiceId = "rob_blind",
                    Text = "He won't see you coming. Take the geiger on his belt.",
                    MoraleDelta = -20f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "geiger_counter", ItemAmount = 1 }
                    }
                    // Survivor gains Sociopath trait handled by EventRunner
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #98 — Encounter: The Deserter's Cache
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateDeserterCache()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_deserter_cache";
            ev.title = "The Deserter's Cache";
            ev.bodyText =
                "Under a loose floorboard: a medkit, still sealed. Morphine, bandages, " +
                "two vials of anti-rad — enough to save a life. A folded note reads: " +
                "\"Gone to find them. If I'm not back by Tuesday, use these.\" " +
                "There's a wedding ring tucked in the kit.";
            ev.weight = 1.0f;
            ev.conditions = new EventConditions { MinDay = 3 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "steal_cache",
                    Text = "Take the meds. Tuesday never came.",
                    MoraleDelta = -8f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "morphine", ItemAmount = 2 },
                        new EventEffect { ItemId = "anti_rad", ItemAmount = 2 },
                        new EventEffect { ItemId = "bandage", ItemAmount = 3 },
                        new EventEffect
                        {
                            ScheduleEventId = "revenge_raid",
                            ScheduleDelayDays = 5,
                            SetWorldFlag = "deserter_cache_stolen"
                        }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "leave_cache",
                    Text = "Leave it. Someone might still come back.",
                    MoraleDelta = 2f,
                    SetEventFlags = new List<string> { "deserter_cache_respected" }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #99 — Encounter: The Mutated Bear
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateMutatedBear()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_mutated_bear";
            ev.title = "The Mutated Bear";
            ev.bodyText =
                "It was a bear. The shape is still there under the tumors. Hairless, " +
                "covered in weeping sores, breathing in wet, ragged gasps. It charges " +
                "not out of hunger — the thing can't eat anymore — but out of pain. " +
                "Everything it sees is the thing that did this to it.";
            ev.weight = 2.5f;
            ev.conditions = new EventConditions
            {
                MinDay = 31,
                RequiredItemId = "firearm"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "fight_bear",
                    Text = "Stand your ground. Empty the magazine.",
                    MoraleDelta = -5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "ammo_rifle", ItemAmount = -3 },
                        new EventEffect { ItemId = "mutated_meat_large", ItemAmount = 2 },
                        new EventEffect { TargetNeed = "health", NeedDelta = -30f }
                    },
                    RequiredItemId = "firearm",
                    HideIfGatesFail = false // Shows grayed-out if no firearm so player sees why they auto-flee
                },
                new EventChoice
                {
                    ChoiceId = "flee_bear",
                    Text = "You have no firearm. Drop everything and run.",
                    MoraleDelta = -12f
                    // DropLoot + Laceration handled by ExpeditionSystem
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #100 — Encounter: The Sinking Mud
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateSinkingMud()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_sinking_mud";
            ev.title = "The Sinking Mud";
            ev.bodyText =
                "The ground gives way without warning. One leg sinks to the thigh in " +
                "thick, radioactive silt — the kind that settled here after the floods " +
                "carried everything downstream. The geiger screams. You have seconds.";
            ev.weight = 1.3f;
            ev.conditions = new EventConditions
            {
                MinDay = 20,
                RequireFalloutStorm = true
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "abandon_boots",
                    Text = "Cut the straps. Leave the boots. Climb out.",
                    MoraleDelta = -3f
                    // HazmatSuit durability loss handled by ExpeditionSystem
                },
                new EventChoice
                {
                    ChoiceId = "dig_out",
                    Text = "Dig. Four hours. Every minute the silt soaks deeper.",
                    MoraleDelta = -8f
                    // 4h rad dose + leg radiation handled by ExpeditionSystem
                }
            };
            return ev;
        }

        // ═══════════════════════════════════════════════════════════════
        // SHELTER EVENTS (Prompts #101-#104)
        // ═══════════════════════════════════════════════════════════════

        // ───────────────────────────────────────────────────────────────
        // Prompt #101 — Shelter Event: The Tainted Rain
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateTaintedRain()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_tainted_rain";
            ev.title = "The Tainted Rain";
            ev.bodyText =
                "The rain started an hour ago. It's not water — it's acid mixed with " +
                "fallout particulate. The catchment surface on the roof is sizzling. " +
                "Through the periscope you can see the metal warping. If it fails, " +
                "that's weeks of work gone. Somebody has to go up there.";
            ev.weight = 0f; // Triggered by weather system, not random pool
            ev.conditions = new EventConditions
            {
                MinDay = 40,
                RequiredFlagId = "weather_rain"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "cover_catchment",
                    Text = "Send someone up. Suit up. Cover the catchment before it melts.",
                    MoraleDelta = 6f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -20f },
                        new EventEffect { TargetNeed = "radiation", NeedDelta = 10f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "let_it_burn",
                    Text = "We can't risk a person for a piece of metal. Let it dissolve.",
                    MoraleDelta = -10f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "catchment_destroyed", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #102 — Shelter Event: The Knocking from Below
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateKnockingBelow()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_knocking_below";
            ev.title = "The Knocking from Below";
            ev.bodyText =
                "It started at 3 AM. Three knocks. Pause. Three knocks. Coming from " +
                "under the concrete floor of the basement. There is nothing under " +
                "the basement — the blueprints confirm it. But the knocking continues. " +
                "Elena has stopped sleeping. Marcus is reinforcing the hatch.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 25,
                RequiredFlagId = "low_morale"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "ignore_knocking",
                    Text = "It's just the pipes. The earth settling. Go back to bed.",
                    MoraleDelta = -4f
                    // RadiationAnxiety rises
                },
                new EventChoice
                {
                    ChoiceId = "comforting_lie",
                    Text = "Tell them it's a trapped animal. You'll handle it tomorrow.",
                    MoraleDelta = -2f,
                    RequiredTrait = "Charisma",
                    HideIfGatesFail = true
                },
                new EventChoice
                {
                    ChoiceId = "reinforce_floor",
                    Text = "Waste the scrap. Pour concrete. Make the knocking stop.",
                    MoraleDelta = 5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "mechanical_parts", ItemAmount = -20 }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #103 — Shelter Event: Carbon Monoxide Leak
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateCOLeak()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_co_leak";
            ev.title = "Silent Killer";
            ev.bodyText =
                "No popup. No warning. The generator has been running for days and " +
                "the air filter is choking. The CO builds without color, without smell. " +
                "Survivors grow tired. Headaches. Then sleep. Then nothing. " +
                "Check the generator room.";
            ev.weight = 0f; // Silent — no UI popup. Detected by manual inspection.
            ev.conditions = new EventConditions
            {
                MinDay = 10,
                RequiredFlagId = "generator_running"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "ventilate",
                    Text = "Open the hatch. Ventilate. Fix the filter.",
                    MoraleDelta = 3f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = 15f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "ignore_co",
                    Text = "The headaches will pass. Keep the hatch sealed.",
                    MoraleDelta = -5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -25f },
                        new EventEffect { TargetNeed = "fatigue", NeedDelta = 30f }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #104 — Shelter Event: The Saboteur
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateSaboteur()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_saboteur";
            ev.title = "The Saboteur";
            ev.bodyText =
                "The water purifier is in pieces. Someone took a pipe wrench to it. " +
                "The hatch is still locked from the inside. There is no sign of forced " +
                "entry. Whoever did this sleeps three feet from you. The question is: " +
                "was it a spy, or was it one of us who finally broke?";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 15,
                RequiredFlagId = "faction_trust_low"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "interrogate_crew",
                    Text = "Line everyone up. Ask hard questions.",
                    MoraleDelta = -12f
                    // Affinity damage applied by EventRunner
                },
                new EventChoice
                {
                    ChoiceId = "blame_outside",
                    Text = "It was a faction raid. No one here did this. Say it until you believe it.",
                    MoraleDelta = -4f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "saboteur_uncaught", WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "forgive_silently",
                    Text = "Fix the purifier. Say nothing. Watch everyone a little more carefully.",
                    MoraleDelta = 0f,
                    RequiredTrait = "Paranoid",
                    HideIfGatesFail = true
                }
            };
            return ev;
        }

        // ═══════════════════════════════════════════════════════════════
        // PROMPTS #105–#118 — ADDITIONAL ENCOUNTERS & EVENTS
        // ═══════════════════════════════════════════════════════════════

        // ───────────────────────────────────────────────────────────────
        // Prompt #105 — Encounter: The Dying Doctor
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateDyingDoctor()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_dying_doctor";
            ev.title = "The Dying Doctor";
            ev.bodyText =
                "A man in a white coat — gray now, with ash — leans against a " +
                "pharmacy counter. A bullet wound in his side,三天 old. He's been " +
                "waiting. His bag is open beside him: morphine, scalpels, antibiotics. " +
                "He asks for water. Just water.";
            ev.weight = 1.6f;
            ev.conditions = new EventConditions { MinDay = 5 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "take_supplies",
                    Text = "Take the bag. He won't need it.",
                    MoraleDelta = -25f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "morphine", ItemAmount = 3 },
                        new EventEffect { ItemId = "bandage", ItemAmount = 5 },
                        new EventEffect { ItemId = "anti_rad", ItemAmount = 2 },
                        new EventEffect { ItemId = "surgical_tools", ItemAmount = 1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "ease_passing",
                    Text = "Give him water. Morphine. Hold his hand until he stops shaking.",
                    MoraleDelta = 18f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "morphine", ItemAmount = -1 },
                        new EventEffect { ItemId = "clean_water", ItemAmount = -1 }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #106 — Encounter: The Cannibal's Trap
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateCannibalTrap()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_cannibal_trap";
            ev.title = "The Cannibal's Trap";
            ev.bodyText =
                "The grocery store still has cans on the shelves — too many. " +
                "You see the tripwire a half-second before your boot catches it. " +
                "Above the door: a loaded shotgun rigged to a pulley. Whoever set " +
                "this has been eating better than you.";
            ev.weight = 1.4f;
            ev.conditions = new EventConditions { MinDay = 8 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "disarm_trap",
                    Text = "Step over. Disarm it. Take the shotgun shells.",
                    RequiredTrait = "Agility",
                    HideIfGatesFail = true,
                    MoraleDelta = 3f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "ammo_shotgun", ItemAmount = 4 },
                        new EventEffect { ItemId = "canned_food", ItemAmount = 3 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "back_away",
                    Text = "Back away slowly. Leave the cans. Live.",
                    MoraleDelta = -2f
                },
                new EventChoice
                {
                    ChoiceId = "trigger_trap",
                    Text = "You didn't see it in time.",
                    MoraleDelta = -8f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -35f }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #107 — Encounter: The Broken Drone
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateBrokenDrone()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_broken_drone";
            ev.title = "The Broken Drone";
            ev.bodyText =
                "A military surveillance drone, crashed into the side of a parking " +
                "garage. One wing sheared off. The camera housing is cracked but the " +
                "battery light still blinks green. The data core is intact. You can " +
                "take one — not both — before the capacitors discharge.";
            ev.weight = 1.3f;
            ev.conditions = new EventConditions { MinDay = 3 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "harvest_battery",
                    Text = "Pull the battery. Power for weeks.",
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "battery", ItemAmount = 3 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "download_data",
                    Text = "Download the memory core. See what it saw.",
                    RequiredTrait = "Science",
                    HideIfGatesFail = true,
                    SetEventFlags = new List<string> { "drone_data_downloaded" },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "reveal_3_nodes", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #108 — Encounter: The Minefield
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateMinefield()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_minefield";
            ev.title = "The Minefield";
            ev.bodyText =
                "Red stakes in the ash. Faded yellow tape: MINEN. The path through " +
                "is a narrow corridor between two apartment blocks. On the other side: " +
                "a supply cache dropped by a helicopter that never landed. But the " +
                "mines are still live. Every step is a question.";
            ev.weight = 1.8f;
            ev.conditions = new EventConditions { MinDay = 2 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "navigate_slow",
                    Text = "Take it slow. Twelve hours. One step at a time.",
                    MoraleDelta = 2f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = 4 },
                        new EventEffect { ItemId = "ammo_rifle", ItemAmount = 6 }
                    }
                    // 12h added to expedition handled by ExpeditionSystem
                },
                new EventChoice
                {
                    ChoiceId = "navigate_fast",
                    Text = "Run it. The mines are old. Most won't fire.",
                    MoraleDelta = -2f
                    // 30% death chance handled by ExpeditionSystem
                },
                new EventChoice
                {
                    ChoiceId = "go_around",
                    Text = "Find another route. Lose the day, keep your legs.",
                    MoraleDelta = -3f
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #109 — Encounter: The Rival Scavenger
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateRivalScavenger()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_rival_scavenger";
            ev.title = "The Rival Scavenger";
            ev.bodyText =
                "You both round the corner at the same time. A woman in a patched " +
                "leather coat, hand on her pistol. Between you: a medical supply box, " +
                "still sealed. Her eyes flick from you to the box. She's calculating " +
                "the same math you are.";
            ev.weight = 1.5f;
            ev.conditions = new EventConditions { MinDay = 7 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "shoot_first",
                    Text = "Draw first. She's doing the same math.",
                    MoraleDelta = -15f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "ammo_pistol", ItemAmount = -1 },
                        new EventEffect { ItemId = "bandage", ItemAmount = 4 },
                        new EventEffect { ItemId = "anti_rad", ItemAmount = 2 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "intimidate",
                    Text = "We both know how this ends. Walk away.",
                    RequiredTrait = "Intimidating",
                    HideIfGatesFail = true,
                    MoraleDelta = -4f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "bandage", ItemAmount = 4 },
                        new EventEffect { ItemId = "anti_rad", ItemAmount = 2 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "split_it",
                    Text = "Split it. Half each. We're both still human.",
                    MoraleDelta = 4f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "bandage", ItemAmount = 2 },
                        new EventEffect { ItemId = "anti_rad", ItemAmount = 1 }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #110 — Encounter: Overturned Ambulance
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateOverturnedAmbulance()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_overturned_ambulance";
            ev.title = "Overturned Ambulance";
            ev.bodyText =
                "An ambulance on its side, rear doors crushed but intact. The red " +
                "cross is still visible under the ash. Something inside is banging. " +
                "Could be supplies shifting in the wreck. Could be the paramedics " +
                "never made it out. You need a crowbar to find out.";
            ev.weight = 1.7f;
            ev.conditions = new EventConditions
            {
                MinDay = 2,
                RequiredItemId = "crowbar"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "pry_open",
                    Text = "Wedge the crowbar in. Pop the doors.",
                    RequiredItemId = "crowbar",
                    MoraleDelta = 2f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "morphine", ItemAmount = 2 },
                        new EventEffect { ItemId = "bandage", ItemAmount = 3 },
                        new EventEffect { ItemId = "surgical_tools", ItemAmount = 1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "infected_inside",
                    Text = "Three infected paramedics burst out. They've been trapped for days.",
                    MoraleDelta = -10f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "health", NeedDelta = -20f }
                    }
                    // 50% chance — handled by EventRunner random
                },
                new EventChoice
                {
                    ChoiceId = "walk_away",
                    Text = "You don't have a crowbar. Whatever's inside can stay inside.",
                    MoraleDelta = -4f
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #111 — Event: The Final Broadcast (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateFinalBroadcast()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_final_broadcast";
            ev.title = "The Final Broadcast";
            ev.bodyText =
                "The radio catches a voice — calm, professional, impossibly far away. " +
                "It's the ISS. The astronaut lists the names of the crew. Then: " +
                "'We can see the fires. The whole hemisphere. Tell them we were here.' " +
                "Static. Then nothing. The bunker is silent for a long time.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 5,
                RequiredFlagId = "is_on_radio"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "record_names",
                    Text = "Write down the names. Someone should remember.",
                    MoraleDelta = -8f
                },
                new EventChoice
                {
                    ChoiceId = "turn_off_radio",
                    Text = "Turn off the radio. Some things you can't carry.",
                    MoraleDelta = -15f
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #112 — Event: The Mutated Flora (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateMutatedCrops()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_mutated_crops";
            ev.title = "The Mutated Flora";
            ev.bodyText =
                "The greenhouse potatoes have come up wrong. The tubers are the size " +
                "of fists but they glow faintly in the dark — a soft green luminescence " +
                "that pulses like a heartbeat. Suki tested one with the geiger. The " +
                "needle jumped. But they're food. Real food. The first fresh thing " +
                "anyone has seen in months.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 40,
                RequiredFlagId = "greenhouse_active"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "eat_them",
                    Text = "Eat them. Food is food. We'll deal with the rads later.",
                    MoraleDelta = 5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = 4 },
                        new EventEffect { TargetNeed = "radiation", NeedDelta = 15f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "burn_them",
                    Text = "Burn the crop. Start over. Some things aren't worth the cost.",
                    MoraleDelta = -12f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "mutated_crops_burned", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #113 — Event: A Moment of Peace (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateMomentOfPeace()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_moment_of_peace";
            ev.title = "A Moment of Peace";
            ev.bodyText =
                "The ash has settled. For the first time in weeks, the sky is visible — " +
                "a pale blue, washed out, but unmistakably the sky. Elena is already " +
                "climbing the ladder to the roof hatch. 'Just for a minute. Just to " +
                "feel the sun.' The geiger reads low. This might not last.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 35,
                RequiredFlagId = "weather_clear"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "go_to_roof",
                    Text = "Open the hatch. Let everyone up. Five minutes of sunlight.",
                    MoraleDelta = 20f
                    // Cures Listless; small sniper risk handled by system
                },
                new EventChoice
                {
                    ChoiceId = "stay_inside",
                    Text = "Keep the hatch sealed. The sun is a lie. The ash will be back.",
                    MoraleDelta = -8f
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #114 — Event: Generator Fire (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateGeneratorFire()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_generator_fire";
            ev.title = "Generator Fire";
            ev.bodyText =
                "Smoke pours from the generator room. The diesel caught — the old " +
                "fuel line finally gave out. Flames are licking the wall where the " +
                "spare fuel cans are stored. You have maybe sixty seconds to decide.";
            ev.weight = 0f;
            ev.conditions = new EventConditions
            {
                MinDay = 15,
                RequiredFlagId = "generator_running"
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "fight_fire_water",
                    Text = "Grab the water buckets. Fight it.",
                    MoraleDelta = 4f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "clean_water", ItemAmount = -3 },
                        new EventEffect { TargetNeed = "health", NeedDelta = -10f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "seal_room",
                    Text = "Seal the bulkhead. Let the fire eat the oxygen and die.",
                    MoraleDelta = -6f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = "generator_destroyed", WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #115 — Event: The Stray Cat (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateStrayCat()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_stray_cat";
            ev.title = "The Stray Cat";
            ev.bodyText =
                "A cat — thin, one ear torn, covered in ash — slipped through the " +
                "hatch during the last entry. It's been living in the storage room " +
                "for three days before anyone noticed. The rat problem is gone. " +
                "Marcus is sneezing uncontrollably. Elena has already named it.";
            ev.weight = 0f;
            ev.conditions = new EventConditions { MinDay = 10 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "keep_cat",
                    Text = "Let it stay. Every bunker needs a cat.",
                    MoraleDelta = 8f,
                    SetEventFlags = new List<string> { "cat_adopted" }
                },
                new EventChoice
                {
                    ChoiceId = "release_cat",
                    Text = "Put it back outside. Marcus can't breathe.",
                    MoraleDelta = -5f
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #116 — Encounter: The Hanging Man
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateHangingMan()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_hanging_man";
            ev.title = "The Hanging Man";
            ev.bodyText =
                "A body hangs from a highway overpass, a rope around the chest — not " +
                "the neck. Someone tied him there. Below him, arranged neatly: a pair " +
                "of military-grade boots, a canteen, and a note. 'Take what you need. " +
                "I don't need it anymore. Tell someone I was here.' The boots are " +
                "exactly your size.";
            ev.weight = 1.2f;
            ev.conditions = new EventConditions { MinDay = 15 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "take_boots",
                    Text = "Take the boots. Read the note one more time.",
                    MoraleDelta = -18f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "military_boots", ItemAmount = 1 },
                        new EventEffect { ItemId = "canteen", ItemAmount = 1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "leave_him",
                    Text = "Leave him. Leave everything. Walk away.",
                    MoraleDelta = -4f
                },
                new EventChoice
                {
                    ChoiceId = "bury_him",
                    Text = "Cut him down. Bury what you can. Take nothing.",
                    MoraleDelta = 6f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "fatigue", NeedDelta = 15f }
                    }
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #117 — Encounter: The Flooded Crater
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateFloodedCrater()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_flooded_crater";
            ev.title = "The Flooded Crater";
            ev.bodyText =
                "An artillery crater, fifty meters across, filled with black water. " +
                "The geiger screams when you approach the edge. On the far side: the " +
                "pharmacy you came for. You can swim it — the water is bitterly cold " +
                "and humming with radiation — or walk the long way around.";
            ev.weight = 1.4f;
            ev.conditions = new EventConditions { MinDay = 20 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "swim_crater",
                    Text = "Swim. It's just water. Cold water. Radioactive water.",
                    MoraleDelta = -6f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { TargetNeed = "radiation", NeedDelta = 20f },
                        new EventEffect { TargetNeed = "warmth", NeedDelta = -40f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "go_around",
                    Text = "Walk around. Eight hours. Dry feet, less cancer.",
                    MoraleDelta = -2f
                    // 8h added to expedition
                }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Prompt #118 — Event: The Forgotten Birthday (Shelter)
        // ───────────────────────────────────────────────────────────────
        public static GameEvent CreateForgottenBirthday()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_forgotten_birthday";
            ev.title = "The Forgotten Birthday";
            ev.bodyText =
                "Marcus is staring at the wall. You ask what's wrong. He doesn't " +
                "answer at first. Then: 'It's my birthday. I just realized. I don't " +
                "know why that matters. It shouldn't matter. But it's my birthday " +
                "and we're thirty feet underground eating cold beans.' He laughs, " +
                "but it's the wrong kind of laugh.";
            ev.weight = 0f;
            ev.conditions = new EventConditions { MinDay = 10 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "give_ration",
                    Text = "Give him an extra ration. Find a candle. Sing.",
                    MoraleDelta = 12f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { ItemId = "canned_food", ItemAmount = -1 }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "shut_up",
                    Text = "We don't have time for birthdays. Get back to work.",
                    MoraleDelta = -15f
                }
            };
            return ev;
        }

        // ═══════════════════════════════════════════════════════════════
        // PROMPTS #144–#153 — ADDITIONAL ENCOUNTERS
        // ═══════════════════════════════════════════════════════════════

        public static GameEvent CreateRedFlare()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_red_flare"; ev.title = "The Flare in the Ash"; ev.weight = 1.5f;
            ev.bodyText = "A red flare pops through the nuclear fog, maybe two klicks east. " +
                "Someone is alive out there — or someone wants you to think they are.";
            ev.conditions = new EventConditions { MinDay = 5 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "investigate_flare", Text = "Investigate. Someone might need help.",
                    MoraleDelta = 4f, SetEventFlags = new List<string> { "flare_investigated" } },
                new EventChoice { ChoiceId = "ignore_flare", Text = "Ignore it. It's probably a trap.",
                    MoraleDelta = -10f, SetEventFlags = new List<string> { "guilt_debuff" } }
            };
            return ev;
        }

        public static GameEvent CreateGeigerSpike()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_geiger_spike"; ev.title = "The Hot Pocket"; ev.weight = 1.3f;
            ev.bodyText = "The geiger counter screams — a sound you've learned to fear. " +
                "An invisible dust eddy, hyper-irradiated, swirls around your boots. " +
                "Every second you stand here is another chest X-ray.";
            ev.conditions = new EventConditions { MinDay = 10, RequiredFlagId = "weather_windy" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "drop_backpack", Text = "Drop the backpack. Sprint.",
                    MoraleDelta = -8f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "radiation", NeedDelta = 10f } } },
                new EventChoice { ChoiceId = "trudge_through", Text = "Keep the loot. Walk. Don't breathe.",
                    MoraleDelta = -2f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "radiation", NeedDelta = 40f } } }
            };
            return ev;
        }

        public static GameEvent CreateHazmatCorpse()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_hazmat_corpse"; ev.title = "The Pristine Suit"; ev.weight = 1.8f;
            ev.bodyText = "A body in a sealed MilitaryHazmatSuit. The suit is intact — " +
                "the person inside is not. Two hours to peel it off. The smell will stay with you.";
            ev.conditions = new EventConditions { MinDay = 3 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "take_suit", Text = "Spend two hours. Get the suit.",
                    MoraleDelta = -20f, Effects = new List<EventEffect> { new EventEffect { ItemId = "military_hazmat_suit", ItemAmount = 1 } } },
                new EventChoice { ChoiceId = "leave_suit", Text = "Leave it. Some things cost more than they're worth.", MoraleDelta = 2f }
            };
            return ev;
        }

        public static GameEvent CreateTheStroller()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_the_stroller"; ev.title = "The Bait"; ev.weight = 1.6f;
            ev.bodyText = "A baby stroller sits in the middle of a bridge. Perfectly positioned. " +
                "Too perfectly. The ash around it is undisturbed — no footprints, no tracks.";
            ev.conditions = new EventConditions { MinDay = 5 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "disarm_ied", Text = "See the wire. Disarm it. Take the scrap.",
                    RequiredTrait = "Perception", HideIfGatesFail = true, MoraleDelta = 3f,
                    Effects = new List<EventEffect> { new EventEffect { ItemId = "explosives_scrap", ItemAmount = 3 } } },
                new EventChoice { ChoiceId = "trigger_ied", Text = "Approach without looking. The wire catches your boot.",
                    MoraleDelta = -12f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "health", NeedDelta = -50f } } },
                new EventChoice { ChoiceId = "avoid_stroller", Text = "Cross the street. Don't even look at it.", MoraleDelta = -3f }
            };
            return ev;
        }

        public static GameEvent CreateVendingMachine()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_vending_machine"; ev.title = "The Working Vending Machine"; ev.weight = 1.1f;
            ev.bodyText = "An intact vending machine in a subway station. The lights are on. " +
                "Somewhere, a generator still runs. Behind the glass: candy bars, chips, " +
                "sealed bottles of water. Pre-war money is useless now — unless you have some.";
            ev.conditions = new EventConditions { MinDay = 2 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "smash_machine", Text = "Smash the glass. Grab everything.",
                    MoraleDelta = -2f, Effects = new List<EventEffect> { new EventEffect { ItemId = "junk_food", ItemAmount = 5 } } },
                new EventChoice { ChoiceId = "use_money", Text = "Insert pre-war bills. Buy one item silently.",
                    RequiredItemId = "pre_war_money", MoraleDelta = 2f,
                    Effects = new List<EventEffect> { new EventEffect { ItemId = "junk_food", ItemAmount = 1 } } }
            };
            return ev;
        }

        public static GameEvent CreateCultProcession()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_cult_procession"; ev.title = "Cult Procession"; ev.weight = 1.4f;
            ev.bodyText = "Twenty members of the Cult of the Glow march silently through the ash, " +
                "robes stained with irradiated mud. They carry a pallet of supplies — water, medicine — " +
                "and a Geiger counter that clicks like a metronome. They haven't seen you yet.";
            ev.conditions = new EventConditions { MinDay = 20 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "hide_cult", Text = "Hide in the rubble. Wait them out.",
                    MoraleDelta = -2f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "fatigue", NeedDelta = 10f } } },
                new EventChoice { ChoiceId = "ambush_cult", Text = "Ambush the stragglers. Take their water.",
                    MoraleDelta = -10f, Effects = new List<EventEffect> { new EventEffect { ItemId = "irradiated_water", ItemAmount = 8 } } }
            };
            return ev;
        }

        public static GameEvent CreateParatrooper()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_paratrooper"; ev.title = "The Paratrooper"; ev.weight = 1.9f;
            ev.bodyText = "A pilot from the Day 30 exchange hangs in a tree, parachute tangled, " +
                "both legs broken at ugly angles. He's been here for days. His flight suit is " +
                "still pristine — insulated, radiation-rated. He sees you and whispers: 'Please.'";
            ev.conditions = new EventConditions { MinDay = 31 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "rescue_pilot", Text = "Cut him down. Use the medkit. Save him.",
                    MoraleDelta = 15f, Effects = new List<EventEffect> { new EventEffect { ItemId = "bandage", ItemAmount = -2 } },
                    SetEventFlags = new List<string> { "pilot_rescued" } },
                new EventChoice { ChoiceId = "kill_pilot", Text = "Slit his throat. Take the flight suit and rations.",
                    MoraleDelta = -30f, Effects = new List<EventEffect> { new EventEffect { ItemId = "flight_suit", ItemAmount = 1 }, new EventEffect { ItemId = "military_rations", ItemAmount = 3 } } }
            };
            return ev;
        }

        public static GameEvent CreateTaintedStream()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_tainted_stream"; ev.title = "Tainted Stream"; ev.weight = 1.0f;
            ev.bodyText = "A running stream cuts through the ash. The water is clear — " +
                "impossibly clear. But it smells like copper. Every survival instinct " +
                "screams at you. Your throat doesn't care.";
            ev.conditions = new EventConditions { MinDay = 5, RequiredFlagId = "thirst_critical" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "drink_stream", Text = "Drink. Deal with the consequences later.",
                    MoraleDelta = -4f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "thirst", NeedDelta = -60f } } },
                new EventChoice { ChoiceId = "refuse_stream", Text = "Walk on. Better to die dry than poisoned.",
                    MoraleDelta = 3f }
            };
            return ev;
        }

        public static GameEvent CreateRivalTrader()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_rival_trader"; ev.title = "The Rival Scavenger"; ev.weight = 1.5f;
            ev.bodyText = "A heavily armed figure blocks the hallway. Bandoliers, a rifle, " +
                "a pack bulging with supplies. Their finger is on the trigger guard. " +
                "Neither of you moves. The air between you is three meters of pure calculation.";
            ev.conditions = new EventConditions { MinDay = 8 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "shoot_rival", Text = "Shoot first. End the calculation.",
                    MoraleDelta = -15f, Effects = new List<EventEffect> { new EventEffect { ItemId = "ammo_rifle", ItemAmount = -2 }, new EventEffect { ItemId = "canned_food", ItemAmount = 3 } } },
                new EventChoice { ChoiceId = "back_away", Text = "Back away slowly. Abandon the node. Live.", MoraleDelta = -4f },
                new EventChoice { ChoiceId = "trade_rival", Text = "Lower your weapon. 'What do you have?'",
                    MoraleDelta = 3f, SetEventFlags = new List<string> { "field_trade_opened" } }
            };
            return ev;
        }

        public static GameEvent CreateAmbulanceGamble()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "enc_ambulance_gamble"; ev.title = "Overturned Ambulance"; ev.weight = 1.7f;
            ev.bodyText = "An ambulance on its side. The rear doors are jammed — need a crowbar. " +
                "Inside, something is scratching. Could be a survivor tapping for help. " +
                "Could be what's left of them, still moving.";
            ev.conditions = new EventConditions { MinDay = 3, RequiredItemId = "crowbar" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "pry_ambulance", Text = "Crowbar the doors. Take the gamble.",
                    RequiredItemId = "crowbar", MoraleDelta = 2f,
                    Effects = new List<EventEffect> { new EventEffect { ItemId = "morphine", ItemAmount = 3 }, new EventEffect { ItemId = "surgical_tools", ItemAmount = 2 } } },
                new EventChoice { ChoiceId = "ghouls_inside", Text = "Four infected paramedics burst out, starved and rabid.",
                    MoraleDelta = -10f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "health", NeedDelta = -25f } } },
                new EventChoice { ChoiceId = "leave_ambulance", Text = "No crowbar. Whatever's inside stays inside.", MoraleDelta = -3f }
            };
            return ev;
        }

        // ═══════════════════════════════════════════════════════════════
        // PROMPTS #154–#163 — SHELTER HORRORS & CONFLICTS
        // ═══════════════════════════════════════════════════════════════

        public static GameEvent CreateCabinFever()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_cabin_fever"; ev.title = "Cabin Fever Break"; ev.weight = 0f;
            ev.bodyText = "Elena hasn't been outside in fifteen days. She stands at the hatch, " +
                "hands on the wheel, eyes wide. 'I just need to feel the wind. Just for a second.' " +
                "She's not wearing a suit. She doesn't care.";
            ev.conditions = new EventConditions { MinDay = 20, RequiredFlagId = "survivor_not_left_15d" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "restrain", Text = "Grab her. Hold her down until she stops shaking.",
                    RequiredTrait = "Strength", HideIfGatesFail = true, MoraleDelta = -5f },
                new EventChoice { ChoiceId = "hatch_opens", Text = "She's too fast. The hatch wheel turns. Cold air floods the room.",
                    MoraleDelta = -10f, Effects = new List<EventEffect> { new EventEffect { SetWorldFlag = "contamination_flood", WorldFlagValue = true } } }
            };
            return ev;
        }

        public static GameEvent CreateSpoiledMeat()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_spoiled_meat"; ev.title = "The Spoiled Fridge"; ev.weight = 0f;
            ev.bodyText = "The power was out for twenty-four hours. The meat locker thawed. " +
                "The smell hits you before you open the door — sweet and wrong. " +
                "Flies. Maggots. Food that was supposed to last months, gone in a day.";
            ev.conditions = new EventConditions { MinDay = 10, RequiredFlagId = "power_blackout_24h" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "throw_out", Text = "Throw it all into the waste compactor. Start over.",
                    MoraleDelta = -15f, Effects = new List<EventEffect> { new EventEffect { ItemId = "canned_food", ItemAmount = -6 } } },
                new EventChoice { ChoiceId = "salvage_meat", Text = "Cook it. Salt it. Cut away the worst parts. Eat what's left.",
                    MoraleDelta = -8f, Effects = new List<EventEffect> { new EventEffect { ItemId = "canned_food", ItemAmount = 3 } } }
            };
            return ev;
        }

        public static GameEvent CreateTheHum()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_the_hum"; ev.title = "The Hum"; ev.weight = 0f;
            ev.bodyText = "Marcus hasn't slept in four days. He sits in the corner, hands over his ears. " +
                "'You don't hear it? The hum. It's in the walls. It's in my teeth.' The generator " +
                "is running at max load. The frequency is exactly 60 hertz.";
            ev.conditions = new EventConditions { MinDay = 10, RequiredFlagId = "power_max_load" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "shut_generator", Text = "Turn off the generator. Give him twenty-four hours of silence.",
                    MoraleDelta = 3f, Effects = new List<EventEffect> { new EventEffect { SetWorldFlag = "generator_off_24h", WorldFlagValue = true } } },
                new EventChoice { ChoiceId = "craft_earplugs", Text = "Craft earplugs from cloth. He'll sleep — but won't hear a raid.",
                    MoraleDelta = 2f, Effects = new List<EventEffect> { new EventEffect { ItemId = "cloth", ItemAmount = -2 } } }
            };
            return ev;
        }

        public static GameEvent CreateHoarder()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_hoarder"; ev.title = "The Hoarder"; ev.weight = 0f;
            ev.bodyText = "Suki's coat is bulging. She's been edgy all week. When she reaches for " +
                "a ration, two iodine pill wrappers fall out of her sleeve. She freezes. " +
                "Everyone sees.";
            ev.conditions = new EventConditions { MinDay = 5, RequiredFlagId = "medical_supplies_low" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "search_bunk", Text = "Search her bunk. Find the stash.",
                    MoraleDelta = -10f, SetEventFlags = new List<string> { "trust_shattered", "iodine_recovered" },
                    Effects = new List<EventEffect> { new EventEffect { ItemId = "iodine_pills", ItemAmount = 2 } } },
                new EventChoice { ChoiceId = "ignore_hoarder", Text = "Look away. Everyone's scared. Everyone copes differently.",
                    MoraleDelta = 2f }
            };
            return ev;
        }

        public static GameEvent CreateFilterChoke()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_filter_choke"; ev.title = "Filter Choke"; ev.weight = 0f;
            ev.bodyText = "The air filter seized during the storm. Dust pours through the vents — " +
                "fine, gray, radioactive. Someone has to crawl into the duct and unclog it. " +
                "It'll take an hour. They'll have to hold their breath.";
            ev.conditions = new EventConditions { MinDay = 20, RequiredFlagId = "fallout_storm_active" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "fix_filter", Text = "Send someone in. Hold your breath. Fix it.",
                    MoraleDelta = 8f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "fatigue", NeedDelta = 100f }, new EventEffect { TargetNeed = "health", NeedDelta = -15f } } },
                new EventChoice { ChoiceId = "pass_out", Text = "They pass out in the duct. Inhale the dust. Severe rad burn to the lungs.",
                    MoraleDelta = -12f, Effects = new List<EventEffect> { new EventEffect { TargetNeed = "health", NeedDelta = -40f }, new EventEffect { TargetNeed = "radiation", NeedDelta = 30f } } }
            };
            return ev;
        }

        public static GameEvent CreateThePreacher()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_the_preacher"; ev.title = "The Preacher"; ev.weight = 0f;
            ev.bodyText = "Three days after waking from the coma, he started talking about what he saw. " +
                "'There's a light. A warm light. It filters through the ash and it chooses who lives.' " +
                "His eyes are too bright. His smile doesn't reach them. But his Morale is unbreakable.";
            ev.conditions = new EventConditions { MinDay = 15, RequiredFlagId = "survivor_recovered_coma" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "allow_preaching", Text = "Let him preach. His certainty is contagious — until it's exhausting.",
                    MoraleDelta = -5f, SetEventFlags = new List<string> { "preacher_active" } },
                new EventChoice { ChoiceId = "silence_him", Text = "Tell him to keep it to himself. We can't afford division.",
                    MoraleDelta = -8f }
            };
            return ev;
        }

        public static GameEvent CreateDustLung()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_dust_lung"; ev.title = "Dust Lung"; ev.weight = 0f;
            ev.bodyText = "The cough started weeks ago. Now it's everyone. A wet, rattling sound " +
                "that starts in the morning and doesn't stop until sleep. The air quality " +
                "has been below forty percent for ten days. Their lungs are filling with silt.";
            ev.conditions = new EventConditions { MinDay = 20, RequiredFlagId = "air_quality_low_10d" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "acknowledge_dust", Text = "There's nothing to do. The damage is done. Live with it.",
                    MoraleDelta = -10f, SetEventFlags = new List<string> { "dust_lung_active" } }
            };
            return ev;
        }

        public static GameEvent CreateBatteryLeak()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_battery_leak"; ev.title = "Battery Acid Leak"; ev.weight = 0f;
            ev.bodyText = "A scream from the power room. One of the jury-rigged batteries ruptured — " +
                "sulfuric acid across the floor, up the wall, onto Marcus's legs. The smell " +
                "is chemical and hot. The power grid flickers.";
            ev.conditions = new EventConditions { MinDay = 15, RequiredFlagId = "jury_rigged_batteries" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "treat_burns", Text = "Get him to the medical bed. Flush the burns. Salvage what you can.",
                    MoraleDelta = -5f, Effects = new List<EventEffect> { new EventEffect { ItemId = "bandage", ItemAmount = -3 }, new EventEffect { TargetNeed = "health", NeedDelta = -25f } } }
            };
            return ev;
        }

        public static GameEvent CreateRadioSpy()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_radio_spy"; ev.title = "The Radio Spy"; ev.weight = 0f;
            ev.bodyText = "Three AM. The radio room. You find them hunched over the dial, whispering " +
                "coordinates into the mic. They're talking to the raiders. The raiders who " +
                "have been finding our caches. They turn, and their face says everything.";
            ev.conditions = new EventConditions { MinDay = 20, RequiredFlagId = "faction_trust_low" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "execute_spy", Text = "Execute them. Make an example. End the leaks.",
                    MoraleDelta = -25f, SetEventFlags = new List<string> { "spy_executed" } },
                new EventChoice { ChoiceId = "banish_spy", Text = "Open the hatch. Push them into the ash. Let the wastes decide.",
                    MoraleDelta = -15f },
                new EventChoice { ChoiceId = "flip_spy", Text = "Feed them false intel. Make the raiders walk into an ambush.",
                    RequiredTrait = "Charisma", HideIfGatesFail = true, MoraleDelta = -5f,
                    SetEventFlags = new List<string> { "false_intel_active" } }
            };
            return ev;
        }

        public static GameEvent CreateRatBite()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "shelter_rat_bite"; ev.title = "Rat Bite"; ev.weight = 0f;
            ev.bodyText = "A scream in the dark. The survivor sleeping on the floor jerks awake — " +
                "a rat the size of a boot is clamped onto their forearm. It took three people " +
                "to pry it off. The bite is already swelling, red lines tracking toward the elbow.";
            ev.conditions = new EventConditions { MinDay = 10, RequiredFlagId = "pest_level_high" };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "treat_bite", Text = "Clean the wound. Bandage it. Watch for fever.",
                    MoraleDelta = -4f, SetEventFlags = new List<string> { "rat_bite_infected", "agoraphobia_trait" },
                    Effects = new List<EventEffect> { new EventEffect { ItemId = "bandage", ItemAmount = -2 } } }
            };
            return ev;
        }

        // ───────────────────────────────────────────────────────────────
        // Pool: all 44 events
        // ───────────────────────────────────────────────────────────────
        public static List<GameEvent> CreateAll()
        {
            return new List<GameEvent>
            {
                CreateChildSniper(), CreateFeralDogPack(), CreateBlindWanderer(),
                CreateDeserterCache(), CreateMutatedBear(), CreateSinkingMud(),
                CreateTaintedRain(), CreateKnockingBelow(), CreateCOLeak(), CreateSaboteur(),
                CreateDyingDoctor(), CreateCannibalTrap(), CreateBrokenDrone(), CreateMinefield(),
                CreateRivalScavenger(), CreateOverturnedAmbulance(), CreateFinalBroadcast(),
                CreateMutatedCrops(), CreateMomentOfPeace(), CreateGeneratorFire(), CreateStrayCat(),
                CreateHangingMan(), CreateFloodedCrater(), CreateForgottenBirthday(),
                CreateRedFlare(), CreateGeigerSpike(), CreateHazmatCorpse(), CreateTheStroller(),
                CreateVendingMachine(), CreateCultProcession(), CreateParatrooper(), CreateTaintedStream(),
                CreateRivalTrader(), CreateAmbulanceGamble(),
                CreateCabinFever(), CreateSpoiledMeat(), CreateTheHum(), CreateHoarder(),
                CreateFilterChoke(), CreateThePreacher(), CreateDustLung(), CreateBatteryLeak(),
                CreateRadioSpy(), CreateRatBite()
            };
        }
    }
}
