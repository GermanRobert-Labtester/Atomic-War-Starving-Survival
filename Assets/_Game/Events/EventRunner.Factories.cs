using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using Ashfall.Core.Journal;

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

        // ─────────────────────────────────────────────────────────────────
        // Allocation 12 — the Day-200 arrival (lore bible 02_THE_LIST)
        //
        // Six people at the hatch. One laminated card in a freezer bag.
        // The game presents the card and the temperature outside, and then
        // stops talking. No branch is adjudicated — not here, not later.
        // ─────────────────────────────────────────────────────────────────

        public const string Allocation12ClaimEventId = "alloc12_the_claim";
        public const string Allocation12BagFoundEventId = "alloc12_the_bag_found";
        /// <summary>Layer-4 gate: the player has met the Archivists (the Vault holds the Schedule).</summary>
        public const string Allocation12GateKnowledgeKey = "lore_bs_the_vault_holds";
        public const string FlagAlloc12Honoured = "alloc12_honoured";
        public const string FlagAlloc12LetterOnly = "alloc12_letter_only";
        public const string FlagAlloc12Refused = "alloc12_refused";
        public const string FlagAlloc12Terms = "alloc12_terms";
        public const string FlagAlloc12BagFound = "alloc12_bag_found";
        public const int Allocation12BagFoundDelayDays = 40;

        /// <summary>
        /// Day 200+ (Layer-4 knowledge required): the allocated party arrives.
        /// Frostbitten, escorted, carrying the paperwork. Not raiders. Polite,
        /// and by the law of a country that no longer exists, correct.
        /// </summary>
        public static GameEvent CreateAllocation12ClaimEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = Allocation12ClaimEventId;
            ev.title = "The Claim";
            ev.bodyText =
                "Six people at the outer hatch. They do not attempt entry. They have been " +
                "walking for eleven days, they know exactly which hatch this is, and one of " +
                "them — thirteen, frostbitten — is carrying a laminated card in a freezer bag. " +
                "The card names this shelter. It names it for fourteen people who are not here.\n\n" +
                "The girl says the five adults kept her alive to get her this far. She does not " +
                "say what she has worked out about what the card is worth.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 200,
                RequiredKnowledgeKey = Allocation12GateKnowledgeKey
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "honour_in_full",
                    Text = "Open the hatch. All six of them.",
                    MoraleDelta = -6f,
                    SetEventFlags = new List<string> { FlagAlloc12Honoured },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagAlloc12Honoured, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "honour_the_letter",
                    Text = "Honour the letter. The girl only.",
                    MoraleDelta = -14f,
                    SetEventFlags = new List<string> { FlagAlloc12LetterOnly },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagAlloc12LetterOnly, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "refuse",
                    Text = "Close it. There is no list any more.",
                    MoraleDelta = -4f,
                    SetEventFlags = new List<string> { FlagAlloc12Refused },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagAlloc12Refused, WorldFlagValue = true },
                        new EventEffect
                        {
                            ScheduleEventId = Allocation12BagFoundEventId,
                            ScheduleDelayDays = Allocation12BagFoundDelayDays
                        }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "labour_terms",
                    Text = "Admit them on labour terms.",
                    MoraleDelta = -10f,
                    FactionId = "military_remnants",
                    TrustDelta = 15f,
                    SetEventFlags = new List<string> { FlagAlloc12Terms },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagAlloc12Terms, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        /// <summary>
        /// Refusal aftermath (~40 days later): a scavenging party finds the freezer
        /// bag. The card is still in it. Nothing happens. Nobody retaliates.
        /// </summary>
        public static GameEvent CreateAllocation12BagFoundEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = Allocation12BagFoundEventId;
            ev.title = "A Card in a Bag";
            ev.bodyText =
                "A scavenging party finds the freezer bag in the ash. The bag is intact. " +
                "The card is still in it: a child's name, an allocation number, a date of " +
                "birth recorded twice, once correctly.\n\n" +
                "They bring it back. Nobody touches it for a long time.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredFlagId = FlagAlloc12Refused
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "bury_it",
                    Text = "Bury it where you found it.",
                    MoraleDelta = -5f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagAlloc12BagFound, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Lore bible 04_ENCOUNTERS — trust-reactive scenes (II-a)
        //
        // The scene does not change. The temperature does. When trust drops
        // below the threshold the threateningBodyText replaces the body;
        // nothing mechanical differs. Faction ids use the ECONOMY namespace
        // (FactionSO.Ids) because ResolveBodyText reads trust from
        // DynamicEconomySystem — the lore-namespace ids have no trust data.
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Trust-reactive checkpoint / Grange / toll / shrine scenes.</summary>
        public static List<GameEvent> CreateTrustReactiveScenes()
        {
            return new List<GameEvent>
            {
                CreateTrustScene(
                    id: "event_checkpoint_papers",
                    title: "The Checkpoint",
                    factionId: "military_remnants",
                    trustBelow: 30f,
                    minDay: 12,
                    body:
                        "The corporal at the barrier checks the manifest against the crate count, " +
                        "finds them equal, and waves you through without looking up. Behind him " +
                        "somebody is frying something in a mess tin and arguing about it.",
                    threateningBody:
                        "The corporal at the barrier checks the manifest against the crate count, " +
                        "finds them equal, and does not move. He reads it again. Behind him the " +
                        "frying has stopped. He asks you to state your shelter designation, which " +
                        "is printed on the manifest, in his hand."),
                CreateTrustScene(
                    id: "event_grange_welcome",
                    title: "The Grange",
                    factionId: "upland_militia",
                    trustBelow: 35f,
                    minDay: 20,
                    body:
                        "Somebody takes your coat. Somebody else is already pouring. Three people ask " +
                        "after your survivors by name and one of them gets a name wrong and is " +
                        "corrected by the other two.",
                    threateningBody:
                        "Somebody takes your coat and hangs it by the door rather than the stove. " +
                        "The conversation does not stop when you enter, which you notice, because " +
                        "it did not use to continue."),
                CreateTrustScene(
                    id: "event_toll_price",
                    title: "The Toll",
                    factionId: "scavenger_camp",
                    trustBelow: 25f,
                    minDay: 18,
                    body:
                        "The Tollman's man quotes the posted rate, takes it, writes a receipt, and " +
                        "gives you the receipt. The transaction is complete and slightly friendly.",
                    threateningBody:
                        "The Tollman's man quotes the posted rate. Then he quotes it again, with a " +
                        "figure attached that is not on the board, and explains - without threat " +
                        "and without apology - that the board is for people whose passage is routine."),
                CreateTrustScene(
                    id: "event_shrine_reading",
                    title: "The Shrine",
                    factionId: "cult_of_the_glow",
                    trustBelow: 30f,
                    minDay: 45,
                    body:
                        "The reading is taken at eye height and spoken aloud. Someone offers you " +
                        "water. It is the same water they are drinking.",
                    threateningBody:
                        "The reading is taken at eye height and spoken aloud. Someone offers you " +
                        "water. It is not from the same jug, and the person who hands it to you " +
                        "watches you hold it.")
            };
        }

        private static GameEvent CreateTrustScene(
            string id, string title, string factionId, float trustBelow, int minDay,
            string body, string threateningBody)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.title = title;
            ev.bodyText = body;
            ev.threateningBodyText = threateningBody;
            ev.threateningFactionId = factionId;
            ev.threateningTrustBelow = trustBelow;
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = minDay };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "move_on",
                    Text = "Move on.",
                    MoraleDelta = 0f
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Lore bible 04_ENCOUNTERS — hazard / pressure events (II-c)
        // Short, mechanical, no moral content. Location references are in
        // the prose; day gates are author-chosen (bible gives none except
        // the standby cycle's ~Day 190).
        // ─────────────────────────────────────────────────────────────────

        public const string FlagStandbyCycleSeen = "standby_cycle_seen";

        /// <summary>Eight short pressure events bound to dangerous places.</summary>
        public static List<GameEvent> CreateHazardEvents()
        {
            return new List<GameEvent>
            {
                CreateHazard("event_gallery_settle", "The Shed Groans", 40,
                    "The snow shed over the pass road groans, low and long. The uphill side is " +
                    "load-bearing and the downhill side is not, any more.",
                    "Keep going. It has held this long.", 0f,
                    "Lose the day. Go around.", -3f),
                CreateHazard("event_paint_stick_gap", "The Marking Stops", 35,
                    "Two kilometres of hard shoulder marked with paint sticks, and then the " +
                    "marking stops, partway. Beyond it is unsurveyed and looks identical.",
                    "Proceed. Ground looks the same.", 0f,
                    "Turn back. Nobody marks without reason.", -4f),
                CreateHazard("event_hull_knock", "Under the Boat", 90,
                    "Something under the boat. Almost certainly debris. Almost.",
                    "Look over the side.", 0f,
                    "Keep rowing. Debris is debris.", -2f),
                CreateHazard("event_ice_creak", "The Roof Creaks", 90,
                    "You are standing on the roof of the thing you came to open. The ice under " +
                    "you creaks once, and then is quiet, which is worse.",
                    "Back up, slowly.", 0f,
                    "Hold still. Wait it out.", -3f),
                CreateHazard("event_relay_current", "The Hut Is Warm", 40,
                    "The equipment hut at the base of Relay Mast 12 is drawing power. The door " +
                    "is not locked. It was not locked yesterday either, which is worth thinking about.",
                    "Open it.", 0f,
                    "Leave it. Powered things have owners.", -2f),
                CreateHazard("event_pump_prime", "One Pump Turns Over", 60,
                    "One pump turns over. Then stops. It can be done. That is all the evidence " +
                    "in the world, and it is enough.",
                    "Sound the note. It can be done.", 0f,
                    "Say nothing. Another day.", -2f),
                CreateHazard("event_low_background_null", "The Counter Reads Clean", 60,
                    "The counter reads clean. It has never read clean. Check the sample or " +
                    "check the instrument.",
                    "Check the sample.", 0f,
                    "Check the instrument.", 0f),
                CreateStandbyCycleEvent()
            };
        }

        private static GameEvent CreateHazard(
            string id, string title, int minDay, string body,
            string choiceAText, float choiceADelta, string choiceBText, float choiceBDelta)
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = id;
            ev.title = title;
            ev.bodyText = body;
            ev.weight = 0.8f;
            ev.conditions = new EventConditions { MinDay = minDay };
            ev.choices = new List<EventChoice>
            {
                new EventChoice { ChoiceId = "choice_a", Text = choiceAText, MoraleDelta = choiceADelta },
                new EventChoice { ChoiceId = "choice_b", Text = choiceBText, MoraleDelta = choiceBDelta }
            };
            return ev;
        }

        /// <summary>
        /// The spine's alarm clock (lore bible II-c): the outer hatch reports
        /// standby, briefly, for the first time in five years, around Day 190.
        /// Fires once — BlockedFlagId suppresses re-fires after the choice
        /// sets standby_cycle_seen.
        /// </summary>
        private static GameEvent CreateStandbyCycleEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "event_standby_cycle";
            ev.title = "Standby";
            ev.bodyText =
                "The outer hatch reports standby, briefly, for the first time in five years. " +
                "It is the hatch doing exactly what it did on the afternoon everyone walked in.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 190,
                BlockedFlagId = FlagStandbyCycleSeen
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "log_it",
                    Text = "Log it.",
                    MoraleDelta = 0f,
                    SetEventFlags = new List<string> { FlagStandbyCycleSeen },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagStandbyCycleSeen, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Lore bible 04_ENCOUNTERS Part I — Ivor Lasko, the deserter vote
        //
        // Not a dialogue choice: an actual show of hands at the Grange Hall,
        // counted, with the player's hand visible to everyone in the room.
        // Branches per the bible: returned / sheltered / abstained.
        // ─────────────────────────────────────────────────────────────────

        public const string LaskoVoteEventId = "event_lasko_vote";
        public const string LaskoAftermathEventId = "event_lasko_aftermath";
        public const string FlagLaskoReturned = "lasko_returned";
        public const string FlagLaskoSheltered = "lasko_sheltered";
        public const string FlagLaskoAbstained = "lasko_abstained";
        public const string FlagLaskoVoteCast = "lasko_vote_cast";
        public const int LaskoAftermathDelayDays = 11;

        /// <summary>Day 40+ one-shot chain: the Militia votes on returning a deserter.</summary>
        public static GameEvent CreateLaskoVoteEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = LaskoVoteEventId;
            ev.title = "The Vote";
            ev.bodyText =
                "The Grange Hall is full. At the front stands a Garrison deserter named Lasko, " +
                "hands loose at his sides. The Militia is holding a vote on whether to return him, " +
                "and Voss's standing order is unambiguous and publicly posted.\n\n" +
                "As a resident, you get a vote. It is a show of hands, counted, and your hand " +
                "will be visible to everyone in the room.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 40,
                BlockedFlagId = FlagLaskoVoteCast
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "returned",
                    Text = "Raise your hand for returning him.",
                    MoraleDelta = -6f,
                    FactionId = "military_remnants",
                    TrustDelta = 15f,
                    SetEventFlags = new List<string> { FlagLaskoReturned, FlagLaskoVoteCast },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagLaskoReturned, WorldFlagValue = true },
                        new EventEffect { FactionId = "upland_militia", TrustDelta = -15f },
                        new EventEffect
                        {
                            ScheduleEventId = LaskoAftermathEventId,
                            ScheduleDelayDays = LaskoAftermathDelayDays
                        }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "sheltered",
                    Text = "Raise your hand for sheltering him.",
                    MoraleDelta = -4f,
                    FactionId = "military_remnants",
                    TrustDelta = -15f,
                    SetEventFlags = new List<string> { FlagLaskoSheltered, FlagLaskoVoteCast },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagLaskoSheltered, WorldFlagValue = true },
                        new EventEffect { FactionId = "upland_militia", TrustDelta = 15f }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "abstained",
                    Text = "Keep your hand down.",
                    MoraleDelta = -8f,
                    FactionId = "military_remnants",
                    TrustDelta = -6f,
                    SetEventFlags = new List<string> { FlagLaskoAbstained, FlagLaskoVoteCast },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagLaskoAbstained, WorldFlagValue = true },
                        new EventEffect { FactionId = "upland_militia", TrustDelta = -6f }
                    }
                }
            };
            return ev;
        }

        /// <summary>
        /// Returned branch aftermath (eleven days later). Very short.
        /// The bible specifies only the shape; the game does not comment.
        /// </summary>
        public static GameEvent CreateLaskoAftermathEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = LaskoAftermathEventId;
            ev.title = "Eleven Days";
            ev.bodyText =
                "They came for Lasko on the eleventh day. He did not run.\n\n" +
                "The report is one line long.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredFlagId = FlagLaskoReturned
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "file_it",
                    Text = "File it.",
                    MoraleDelta = -3f
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Lore bible 05_FACTIONS §8 — the Kittiwake chart (Undertow interlock)
        //
        // The survey launch's logbook holds the only accurate chart of the
        // Drown. Copy it and distribute it and the whole late game opens —
        // and the Undertow's business model ends, which they notice
        // immediately. They do not attack. They have never attacked anyone.
        // ─────────────────────────────────────────────────────────────────

        public const string KittiwakeChartEventId = "event_kittiwake_chart";
        public const string FlagKittiwakeChartFound = "kittiwake_chart_found";
        public const string FlagKittiwakeChartResolved = "kittiwake_chart_resolved";
        public const string FlagKittiwakeChartDistributed = "kittiwake_chart_distributed";
        public const string FlagKittiwakeChartKept = "kittiwake_chart_kept";
        public const string FlagColdStoreOpen = "loc_cold_store_atlantic_open";
        public const string FlagRecordsAnnexOpen = "loc_records_annex_open";

        /// <summary>Fires after arrival at loc_bathymetric_boat sets kittiwake_chart_found.</summary>
        public static GameEvent CreateKittiwakeChartEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = KittiwakeChartEventId;
            ev.title = "The Kittiwake Chart";
            ev.bodyText =
                "The survey launch's logbook holds the only accurate chart of the Drown: eleven " +
                "days of soundings, in metres, with timestamps, kept as the flooding happened. " +
                "It is the reason any of the Drown can be navigated at all.\n\n" +
                "Copied and distributed, it would make the Drown navigable for everyone. " +
                "The cold store would open. The Archivists would stop being isolated. " +
                "And somebody whose business is accidents would notice the moment the " +
                "first copy circulates.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 1,
                RequiredFlagId = FlagKittiwakeChartFound,
                BlockedFlagId = FlagKittiwakeChartResolved
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "distribute",
                    Text = "Copy the chart. Give it to everyone.",
                    MoraleDelta = 0f,
                    SetEventFlags = new List<string> { FlagKittiwakeChartDistributed, FlagKittiwakeChartResolved },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagKittiwakeChartDistributed, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = FlagKittiwakeChartResolved, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "keep_it",
                    Text = "Keep the chart. The Drown stays ours.",
                    MoraleDelta = 0f,
                    SetEventFlags = new List<string> { FlagKittiwakeChartKept, FlagKittiwakeChartResolved },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagKittiwakeChartKept, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = FlagKittiwakeChartResolved, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }

        // ─────────────────────────────────────────────────────────────────
        // Lore bible 05_FACTIONS interlocks — the two ends a dying survivor
        // can be sent to. The Quiet House and the Osteophages both accept
        // them, and the contrast between the two is the argument. The game
        // presents both and does not adjudicate.
        // ─────────────────────────────────────────────────────────────────

        public const string TwoEndsEventId = "event_two_ends";
        public const string FlagSentToQuietHouse = "sent_to_quiet_house";
        public const string FlagSentToOsteophages = "sent_to_osteophages";
        public const string FlagKeptAtHome = "two_ends_kept_home";
        public const string FlagTwoEndsResolved = "two_ends_resolved";

        public static GameEvent CreateTwoEndsEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = TwoEndsEventId;
            ev.title = "The Two Ends";
            ev.bodyText =
                "One of your people is past treatment. Not in pain, or not saying so, but past " +
                "treatment, and everyone in the shelter knows it.\n\n" +
                "There is a house in the Grid that takes the dying. It asks for a name and one " +
                "true thing about them, and nothing else.\n\n" +
                "There is also a chute in the Drown, and a bell, and a wait.";
            ev.weight = 1f;
            ev.conditions = new EventConditions
            {
                MinDay = 30,
                BlockedFlagId = FlagTwoEndsResolved
            };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "quiet_house",
                    Text = "Take them to the Quiet House.",
                    MoraleDelta = -6f,
                    SetEventFlags = new List<string> { FlagSentToQuietHouse, FlagTwoEndsResolved },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagSentToQuietHouse, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = FlagTwoEndsResolved, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "osteophages",
                    Text = "Take them to the airlock.",
                    MoraleDelta = -12f,
                    SetEventFlags = new List<string> { FlagSentToOsteophages, FlagTwoEndsResolved },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagSentToOsteophages, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = FlagTwoEndsResolved, WorldFlagValue = true }
                    }
                },
                new EventChoice
                {
                    ChoiceId = "keep_home",
                    Text = "Keep them home.",
                    MoraleDelta = -4f,
                    SetEventFlags = new List<string> { FlagKeptAtHome, FlagTwoEndsResolved },
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SetWorldFlag = FlagKeptAtHome, WorldFlagValue = true },
                        new EventEffect { SetWorldFlag = FlagTwoEndsResolved, WorldFlagValue = true }
                    }
                }
            };
            return ev;
        }
    }
}
