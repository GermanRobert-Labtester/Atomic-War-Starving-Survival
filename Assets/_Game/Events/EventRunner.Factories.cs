using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Events
{
    public partial class EventRunner
    {
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
