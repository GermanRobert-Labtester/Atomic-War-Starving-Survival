using System;
using System.Collections.Generic;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// Static catalog builder for the 8 built-in Year of Ash questlines covering all 5 factions.
    /// Extracted from QuestlineSystem to separate static narrative content from runtime state.
    /// </summary>
    public static class BuiltInQuestlineCatalog
    {
        public static List<QuestlineDefinition> CreateAll()
        {
            return new List<QuestlineDefinition>
            {
                BuildQuestline_GarrisonBloodDebt(),
                BuildQuestline_AshSignRevelation(),
                BuildQuestline_RebuilderSeedVault(),
                BuildQuestline_HydroBaronAqueduct(),
                BuildQuestline_BlackOpsNullOrder(),
                BuildQuestline_SurvivorMutiny(),
                BuildQuestline_TheLastBroadcast(),
                BuildQuestline_WinterHarvest()
            };
        }

        // ── QUESTLINE 1: The Garrison Blood Debt ─────────────────────────────────
        // Faction: faction_central_garrison | Days 185–260
        // The garrison demands a survivor execution for a perceived desertion.
        private static QuestlineDefinition BuildQuestline_GarrisonBloodDebt()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_garrison_blood_debt",
                title        = "The Garrison Blood Debt",
                synopsis     = "Colonel Harven sends notice: one of your survivors, former conscript Ola Vask, is listed as a deserter under military law. Surrender her or face supply embargo.",
                factionTag   = "faction_central_garrison",
                firstStageId = "stage_blood_debt_demand",
                minDay       = 185,
                maxDay       = 260
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_demand",
                title           = "The Colonel's Notice",
                narrativePrompt = "A garrison courier delivers a wax-sealed tri-fold. Inside: a summary tribunal finding against Ola Vask, deserter. She's been with you for sixty-two days. She hasn't mentioned it once.",
                unlockOnDay     = 185,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId      = "choice_confront_ola",
                        text          = "Confront Ola privately before responding to the garrison.",
                        nextStageId   = "stage_blood_debt_ola_testimony",
                        moraleDelta   = 0,
                        guiltDelta    = 0,
                        outcomeNarrative = "She's quiet for a long time. Then: 'I left because my unit was ordered to execute a camp of civilians. I chose not to.'"
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_comply_immediately",
                        text          = "Seal Ola's rations tent and radio the garrison that she's available for collection.",
                        nextStageId   = "",
                        moraleDelta   = -30,
                        guiltDelta    = 40,
                        targetFactionId   = "faction_central_garrison",
                        factionStandingDelta = 25,
                        outcomeNarrative = "Ola says nothing when they come. She's already seen it before. The other survivors stop talking at mealtimes.",
                        conditions    = new List<QuestCondition>()
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_refuse_and_fortify",
                        text          = "Destroy the notice and instruct the outer guard to turn away garrison personnel.",
                        nextStageId   = "stage_blood_debt_garrison_escalation",
                        moraleDelta   = 15,
                        guiltDelta    = 0,
                        targetFactionId   = "faction_central_garrison",
                        factionStandingDelta = -25,
                        outcomeNarrative = "Word reaches Harven by nightfall. His reply: three fewer fuel tankers next cycle."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_ola_testimony",
                title           = "What She Saw",
                narrativePrompt = "Ola describes seventeen bodies laid in rows by the roadside. She drew her sidearm on her own sergeant. 'I don't expect you to protect me. I know what the math looks like.'",
                unlockOnDay     = 187,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId      = "choice_forge_tribunal_rebuttal",
                        text          = "Have your engineer fabricate a medical exemption — Ola is 'critical bunker staff.'",
                        nextStageId   = "stage_blood_debt_garrison_bluff",
                        moraleDelta   = 5,
                        guiltDelta    = 10,
                        grantItemId   = "item_falsified_clearance",
                        grantItemQuantity = 1,
                        outcomeNarrative = "The forgery is good enough to buy weeks, not months."
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_send_ola_underground",
                        text          = "Move Ola into the sub-level maintenance tunnels — off every roster.",
                        nextStageId   = "stage_blood_debt_garrison_search",
                        moraleDelta   = 8,
                        guiltDelta    = 5,
                        outcomeNarrative = "She doesn't complain about the dark. You notice she's been sleeping there already."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_garrison_bluff",
                title           = "The Inspector's Visit",
                narrativePrompt = "A garrison medical inspector arrives for a 'routine compliance audit.' She's professional, cold, and clearly looking for something specific.",
                unlockOnDay     = 200,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId      = "choice_pass_the_bluff",
                        text          = "Walk the inspector through a curated tour while Ola pretends to be a filtration technician.",
                        nextStageId   = "stage_blood_debt_resolution_protected",
                        moraleDelta   = 10,
                        guiltDelta    = 8,
                        targetFactionId   = "faction_central_garrison",
                        factionStandingDelta = -5,
                        outcomeNarrative = "The inspector files her report. No flag. For now."
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_bribe_inspector",
                        text          = "Slip the inspector 3 vials of morphine from the medical locker.",
                        nextStageId   = "stage_blood_debt_resolution_protected",
                        moraleDelta   = -5,
                        guiltDelta    = 15,
                        targetFactionId   = "faction_central_garrison",
                        factionStandingDelta = 5,
                        outcomeNarrative = "She pockets them without eye contact and writes 'subject deceased - rad exposure' in the file."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_garrison_search",
                title           = "They Send More",
                narrativePrompt = "Two garrison wardens with dogs circle the perimeter for six hours. The dogs sit outside the maintenance access grate for a long time.",
                unlockOnDay     = 205,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId      = "choice_scatter_rad_bait",
                        text          = "Scatter contaminated sediment near the grate to drive the dogs away.",
                        nextStageId   = "stage_blood_debt_resolution_protected",
                        moraleDelta   = 0,
                        guiltDelta    = 5,
                        outcomeNarrative = "The dogs pull away yelping. The wardens file an inconclusive report."
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_ola_turns_herself_in",
                        text          = "[Ola offers to turn herself in. Accept her decision.]",
                        nextStageId   = "",
                        moraleDelta   = -20,
                        guiltDelta    = 30,
                        targetFactionId   = "faction_central_garrison",
                        factionStandingDelta = 20,
                        outcomeNarrative = "She packs nothing. She looks back once from the airlock, then steps into the grey light."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_garrison_escalation",
                title           = "Embargo",
                narrativePrompt = "Fuel deliveries stop. Then the medical resupply convoy takes a different road. Harven leaves one message on the radio: 'You have 30 days.'",
                unlockOnDay     = 200,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId      = "choice_open_back_channel",
                        text          = "Contact a junior garrison logistics officer willing to trade off-books.",
                        nextStageId   = "stage_blood_debt_resolution_protected",
                        moraleDelta   = 5,
                        guiltDelta    = 5,
                        targetFactionId   = "faction_rebuilders",
                        factionStandingDelta = 10,
                        outcomeNarrative = "The side channel holds. It's slower, more expensive, and entirely deniable."
                    },
                    new QuestChoice
                    {
                        choiceId      = "choice_align_with_rebuilders",
                        text          = "Formally petition the Rebuilder Collective for supply substitution.",
                        nextStageId   = "stage_blood_debt_resolution_protected",
                        moraleDelta   = 8,
                        guiltDelta    = 0,
                        // Primary effect: Rebuilders +20. Secondary: Garrison -10 (host applies via FactionWarSystem.ModifyStanding)
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = 20,
                        outcomeNarrative = "The Rebuilders are pleased. The garrison marks you as an alignment risk."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_blood_debt_resolution_protected",
                title           = "Still Here",
                narrativePrompt = "Ola is still in the shelter. She's started teaching the children basic first aid in the corridor after lights-out. Nobody asked her to.",
                unlockOnDay     = 230,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 2: The Ash Sign Revelation ─────────────────────────────────
        // Faction: faction_ash_sign | Days 220–310
        // A former Ash Sign deacon claims to know where the cult is storing irradiated food
        // donated to refugee camps. A moral trap: exposing the cult helps refugees but
        // inflames Ash Sign reprisals.
        private static QuestlineDefinition BuildQuestline_AshSignRevelation()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_ash_sign_revelation",
                title        = "The Ash Sign Revelation",
                synopsis     = "Deacon Pryce, expelled from the cult's inner tier, knocks at your shelter with documents: the Ash Sign has been distributing irradiated grain to refugee collection points for three months.",
                factionTag   = "faction_ash_sign",
                firstStageId = "stage_revelation_pryce_arrives",
                minDay       = 220,
                maxDay       = 310
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_pryce_arrives",
                title           = "The Deacon's Documents",
                narrativePrompt = "Pryce is missing two fingers. He smells of antiseptic and woodsmoke. He sets a leather-wrapped bundle on your map table. 'Fifty-three distribution logs. Every batch flagged Contaminated - Redistribution Approved.'",
                unlockOnDay     = 220,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_verify_documents",
                        text        = "Have your engineer cross-reference the batch stamps with known radiation readings.",
                        nextStageId = "stage_revelation_verified",
                        moraleDelta = 0,
                        guiltDelta  = 0,
                        outcomeNarrative = "The batch numbers match three reports from the Eastern relief camp. The grain was hot."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_turn_pryce_away",
                        text        = "You can't verify the documents. Refuse to act on unconfirmed claims.",
                        nextStageId = "",
                        moraleDelta = -5,
                        guiltDelta  = 20,
                        outcomeNarrative = "Three weeks later, reports filter in of a mass acute radiation syndrome event at the Eastern camps. You don't speak of Pryce."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_verified",
                title           = "The Weight of It",
                narrativePrompt = "The documents are real. Three thousand people received irradiated grain. Some are still alive. What you do next defines what kind of shelter this is.",
                unlockOnDay     = 225,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_broadcast_evidence",
                        text        = "Transmit the documents to all active shelter radio frequencies.",
                        nextStageId = "stage_revelation_ashsign_retaliation",
                        moraleDelta = 20,
                        guiltDelta  = 0,
                        // Primary: Ash Sign -40. Secondary: Rebuilders +25 (host applies via FactionWarSystem.ModifyStanding)
                        targetFactionId      = "faction_ash_sign",
                        factionStandingDelta = -40,
                        outcomeNarrative     = "The signal travels for six hours before the Ash Sign jammers cut in. But the relay stations have already heard."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_negotiate_cessation",
                        text        = "Contact the Ash Sign Elder Council directly: stop the grain program or the documents go public.",
                        nextStageId = "stage_revelation_deal",
                        moraleDelta = 5,
                        guiltDelta  = 10,
                        targetFactionId      = "faction_ash_sign",
                        factionStandingDelta = -10,
                        outcomeNarrative     = "There's a twelve-hour silence. Then: 'We will discuss terms.'"
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_bury_evidence",
                        text        = "Burn Pryce's documents and offer him shelter in exchange for silence.",
                        nextStageId = "stage_revelation_buried",
                        moraleDelta = -25,
                        guiltDelta  = 35,
                        outcomeNarrative     = "Pryce watches the fire. He doesn't argue. He sleeps in the equipment room and never eats with the others."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_ashsign_retaliation",
                title           = "Pillar of Ash",
                narrativePrompt = "An Ash Sign consecrated team boards up your outer ventilation intake with spent ash mortar. You have 48 hours of filtered air before the CO2 rises.",
                unlockOnDay     = 240,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_clear_blockage_by_force",
                        text        = "Send a four-person team in hazmat suits to clear the intake under armed cover.",
                        nextStageId = "stage_revelation_resolved_broadcast",
                        moraleDelta = 5,
                        guiltDelta  = 5,
                        outcomeNarrative = "Two team members take minor flechette wounds. The intake is cleared. The story has already spread."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_negotiate_intake_reopening",
                        text        = "Open a radio channel to the consecrated team leader: you have something they want.",
                        nextStageId = "stage_revelation_deal",
                        moraleDelta = 0,
                        guiltDelta  = 10,
                        outcomeNarrative = "They pull back. The intake reopens. You haven't heard the last of it."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_deal",
                title           = "The Concession",
                narrativePrompt = "Elder Cassia communicates through a relay: the contaminated grain program has been 'paused.' In exchange, she wants Pryce returned — 'for counselling.'",
                unlockOnDay     = 250,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_refuse_pryce_handover",
                        text        = "Refuse. Pryce stays. The deal is the cessation only.",
                        nextStageId = "stage_revelation_resolved_broadcast",
                        moraleDelta = 15,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_ash_sign",
                        factionStandingDelta = -20,
                        outcomeNarrative     = "Cassia cuts the relay. You haven't agreed to anything. Neither have they."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_return_pryce",
                        text        = "[Return Pryce to the Ash Sign for 'counselling.']",
                        nextStageId = "",
                        moraleDelta = -35,
                        guiltDelta  = 45,
                        targetFactionId      = "faction_ash_sign",
                        factionStandingDelta = 30,
                        outcomeNarrative     = "Pryce goes quietly. He knew. He hands you his two-fingered glove at the airlock as a keepsake."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_buried",
                title           = "Silence",
                narrativePrompt = "The radiation sickness reports from the Eastern camps are on the radio for nine days. Nobody in your shelter mentions Pryce's documents. Pryce sleeps in the equipment room.",
                unlockOnDay     = 260,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Failed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_revelation_resolved_broadcast",
                title           = "What Survives the Signal",
                narrativePrompt = "The broadcasts have moved on. But the Eastern camp survivors refer to your shelter by frequency number when they talk about the moment someone finally told the truth.",
                unlockOnDay     = 280,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 3: The Rebuilder Seed Vault ─────────────────────────────────
        // Faction: faction_rebuilders | Days 200–280
        // A Rebuilder botanist discovered a pre-war sub-level seed vault 40km away.
        // Problem: it's inside a contested fallout zone claimed by the garrison.
        private static QuestlineDefinition BuildQuestline_RebuilderSeedVault()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_rebuilder_seed_vault",
                title        = "The Seed Vault Expedition",
                synopsis     = "The Rebuilders have located a pre-war agricultural seed bank 40km north, inside the garrison exclusion zone. They need your shelter's dosimeters and an armed escort. The seeds could restart food production within two seasons.",
                factionTag   = "faction_rebuilders",
                firstStageId = "stage_seed_vault_proposal",
                minDay       = 200,
                maxDay       = 280
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_proposal",
                title           = "The Agronomist's Map",
                narrativePrompt = "Lena Voss spreads a topographic chart across your planning table. The vault symbol is circled in grease pencil. 'Ten thousand non-hybrid cultivars. Pre-war cold-storage. Two growing seasons away from feeding two hundred people permanently.'",
                unlockOnDay     = 200,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_commit_escort",
                        text        = "Commit four survivors and the shelter's three calibrated dosimeters to the expedition.",
                        nextStageId = "stage_seed_vault_exclusion_zone",
                        moraleDelta = 10,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = 15,
                        outcomeNarrative     = "Voss names the expedition team herself. She insists on going too."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_provide_dosimeters_only",
                        text        = "Loan three dosimeters but send no personnel — too few to spare.",
                        nextStageId = "stage_seed_vault_partial",
                        moraleDelta = 0,
                        guiltDelta  = 5,
                        outcomeNarrative = "Voss nods. 'We'll manage.' She doesn't entirely believe it."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_refuse_vault_expedition",
                        text        = "Decline entirely — the fallout zone risk is too high for this shelter.",
                        nextStageId = "",
                        moraleDelta = -8,
                        guiltDelta  = 10,
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = -15,
                        outcomeNarrative     = "Voss rolls up the chart without a word. The seeds remain buried."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_exclusion_zone",
                title           = "Garrison Checkpoint",
                narrativePrompt = "Your team reaches a garrison forward post. Sergeant Deckle demands the expedition manifest and, by the look of it, half the dosimeters as 'zone passage tax.'",
                unlockOnDay     = 215,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_pay_the_checkpoint_tax",
                        text        = "Surrender one dosimeter and proceed with two.",
                        nextStageId = "stage_seed_vault_recovery",
                        moraleDelta = -5,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_central_garrison",
                        factionStandingDelta = 5,
                        outcomeNarrative     = "Deckle stamps the manifest. Your team has 36 hours before the second dosimeter's battery runs out."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_bluff_garrison_orders",
                        text        = "Produce the falsified clearance document if it was acquired during the Garrison Blood Debt questline.",
                        nextStageId = "stage_seed_vault_recovery",
                        moraleDelta = 5,
                        guiltDelta  = 8,
                        grantItemId = "",
                        conditions  = new List<QuestCondition> {
                            new QuestCondition { conditionTag = "item_falsified_clearance_in_inventory", isBlocker = true }
                        },
                        outcomeNarrative     = "Deckle examines the wax seal for a long beat. Then waves you through."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_partial",
                title           = "Rebuilder Report",
                narrativePrompt = "Twelve days later a Rebuilder courier returns two of your three dosimeters. The third is 'lost.' Of the expedition team: four returned, one did not.",
                unlockOnDay     = 230,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_accept_partial_return",
                        text        = "Accept the report and the seeds the team recovered.",
                        nextStageId = "stage_seed_vault_resolution_partial",
                        moraleDelta = 5,
                        guiltDelta  = 10,
                        grantItemId = "item_seed_packet_nonhybrid",
                        grantItemQuantity = 15,
                        outcomeNarrative = "The seeds are sorted into the cold-storage locker. 'Enough to start,' Voss says. 'Not enough to finish.'"
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_recovery",
                title           = "The Vault",
                narrativePrompt = "The vault is intact. Sub-freezing pre-war refrigeration, still running on a geothermal tap. Voss moves through the rows reading labels under her breath like prayer.",
                unlockOnDay     = 230,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_take_full_catalog",
                        text        = "Load every accessible tray — fills all four carrier packs.",
                        nextStageId = "stage_seed_vault_resolution_full",
                        moraleDelta = 15,
                        guiltDelta  = 0,
                        grantItemId = "item_seed_packet_nonhybrid",
                        grantItemQuantity = 60,
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = 20,
                        outcomeNarrative = "The team walks out slowly, packs full, dosimeters still in green."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_share_vault_coordinates",
                        text        = "Take half and broadcast the vault coordinates to all surviving shelters.",
                        nextStageId = "stage_seed_vault_resolution_full",
                        moraleDelta = 25,
                        guiltDelta  = 0,
                        grantItemId = "item_seed_packet_nonhybrid",
                        grantItemQuantity = 30,
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = 35,
                        outcomeNarrative = "Voss transmits the coordinates herself. She holds the radio for a long time after."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_resolution_full",
                title           = "First Planting",
                narrativePrompt = "The greenhouse bay is replanted with ten variety strains. Germination: fourteen days. The waiting feels different to hunger. It feels like intention.",
                unlockOnDay     = 260,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_seed_vault_resolution_partial",
                title           = "Partial Harvest",
                narrativePrompt = "The recovered seeds fill three of the twelve greenhouse trays. It's a start. Voss says nothing about the one who didn't come back. She plants his section first.",
                unlockOnDay     = 250,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 4: The Hydro Baron Aqueduct ─────────────────────────────────
        // Faction: faction_hydro_barons | Days 250–330
        // The Hydro Barons are building a major aqueduct — but its route would flood
        // the valley where the Rebuilder Allotments (and your surface access tunnel) sit.
        private static QuestlineDefinition BuildQuestline_HydroBaronAqueduct()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_hydro_baron_aqueduct",
                title        = "The Aqueduct Decision",
                synopsis     = "Baron Seraph announces a continental-scale water redistribution project. The primary diversion channel will run directly through the Rebuilder Allotments and your shelter's sole surface access tunnel. Thousands will get water. You may lose your exit.",
                factionTag   = "faction_hydro_barons",
                firstStageId = "stage_aqueduct_announcement",
                minDay       = 250,
                maxDay       = 330
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_aqueduct_announcement",
                title           = "The Survey Team",
                narrativePrompt = "Three Hydro Baron engineers arrive with survey stakes and begin driving them into the frozen soil above your access tunnel. They're polite. They have water authority permits.",
                unlockOnDay     = 250,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_challenge_survey_team",
                        text        = "Block the survey team at the perimeter and demand an independent geological review.",
                        nextStageId = "stage_aqueduct_dispute",
                        moraleDelta = 5,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = -15,
                        outcomeNarrative     = "The engineers withdraw but take photographs of your defense positions."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_permit_survey",
                        text        = "Allow the survey to proceed and observe the assessment.",
                        nextStageId = "stage_aqueduct_report",
                        moraleDelta = 0,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = 10,
                        outcomeNarrative     = "The engineers are thorough. The report will take three days."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_aqueduct_report",
                title           = "The Report",
                narrativePrompt = "The survey confirms it: the primary diversion channel bisects your access tunnel at meter 340. The channel project will bring clean water to an estimated 4,000 people in secondary shelters. Your egress: permanently sealed.",
                unlockOnDay     = 265,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_negotiate_reroute",
                        text        = "Commission your own geological study and propose a 600-meter western reroute.",
                        nextStageId = "stage_aqueduct_reroute_negotiation",
                        moraleDelta = 5,
                        guiltDelta  = 0,
                        outcomeNarrative     = "The reroute adds cost and delay. The Barons will hear you."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_accept_sealing",
                        text        = "Accept the sealing in exchange for ten-year priority water allocation.",
                        nextStageId = "",
                        moraleDelta = -10,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = 30,
                        outcomeNarrative     = "The tunnel mouth is filled with concrete aggregate on Day 278. You watch from the periscope. No one speaks."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_ally_with_rebuilders",
                        text        = "Form a joint objection with the Rebuilder Collective and take the dispute to the open frequencies.",
                        nextStageId = "stage_aqueduct_dispute",
                        moraleDelta = 10,
                        guiltDelta  = 0,
                        // Primary: Rebuilders +20. Secondary: Hydro Barons -20 (host applies via FactionWarSystem.ModifyStanding)
                        targetFactionId      = "faction_rebuilders",
                        factionStandingDelta = 20,
                        outcomeNarrative     = "The coalition broadcast goes out for three days straight. The Barons go quiet."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_aqueduct_dispute",
                title           = "Tribunal",
                narrativePrompt = "A water authority arbitration panel convenes on the radio. Each faction has 90 minutes to make its case. You speak for your shelter.",
                unlockOnDay     = 285,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_present_human_cost_case",
                        text        = "Present survival data: sealing the tunnel traps 38 people permanently below ground.",
                        nextStageId = "stage_aqueduct_resolution_rerouted",
                        moraleDelta = 12,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = -5,
                        outcomeNarrative     = "The panel votes 3–2 to approve the western reroute. The channel will miss your tunnel by 50 meters."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_offer_engineering_compromise",
                        text        = "Offer your engineering team to excavate a bypass conduit that preserves both the channel and the tunnel.",
                        nextStageId = "stage_aqueduct_resolution_rerouted",
                        moraleDelta = 20,
                        guiltDelta  = 0,
                        // Primary: Hydro Barons +15. Secondary: Rebuilders +15 (host applies via FactionWarSystem.ModifyStanding)
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = 15,
                        grantItemId = "item_water_allocation_writ",
                        grantItemQuantity = 1,
                        outcomeNarrative     = "The compromise passes unanimously. Your engineers begin the conduit the following week."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_aqueduct_reroute_negotiation",
                title           = "The Baron's Terms",
                narrativePrompt = "Baron Seraph responds: reroute approved if you provide 200 tons of aggregate from your sub-level excavation waste. You have to decide if you can spare it.",
                unlockOnDay     = 290,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_provide_aggregate",
                        text        = "Agree — the aggregate is waste material, the tunnel is irreplaceable.",
                        nextStageId = "stage_aqueduct_resolution_rerouted",
                        moraleDelta = 8,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_hydro_barons",
                        factionStandingDelta = 20,
                        outcomeNarrative     = "The reroute begins. Your tunnel remains. 4,000 people downstream get water."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_aqueduct_resolution_rerouted",
                title           = "Water and Air",
                narrativePrompt = "The channel opens 600 meters west. On a clear day you can hear the water running through the diversion. The tunnel is intact. You still have a way out.",
                unlockOnDay     = 315,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 5: The Black Ops Null Order ────────────────────────────────
        // Faction: faction_black_ops | Days 270–355
        // Black Ops contacts your shelter for an ultra-classified task:
        // locate and neutralize a rogue ex-military AI routing system
        // that is overriding power grid nodes and triggering false all-clear broadcasts.
        private static QuestlineDefinition BuildQuestline_BlackOpsNullOrder()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_black_ops_null_order",
                title        = "Null Order",
                synopsis     = "Operative 09 returns — this time with a target dossier. A pre-war autonomous grid-routing AI named VESTIS is broadcasting false all-clear signals, pulling survivors out of shelters into lethal fallout zones. The Black Ops want it destroyed. They need your shelter's power tap to triangulate the signal.",
                factionTag   = "faction_black_ops",
                firstStageId = "stage_null_order_briefing",
                minDay       = 270,
                maxDay       = 355
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_null_order_briefing",
                title           = "The Dossier",
                narrativePrompt = "Operative 09 delivers the briefing in seven clipped sentences. The VESTIS system has been traced to a pre-war telecom relay 18km east. Destroying it requires a directed EMP pulse. Your shelter's capacitor array is the only source nearby.",
                unlockOnDay     = 270,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_cooperate_full",
                        text        = "Agree to loan the capacitor array for 72 hours.",
                        nextStageId = "stage_null_order_prep",
                        moraleDelta = 5,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_black_ops",
                        factionStandingDelta = 20,
                        outcomeNarrative     = "09 nods once. 'Seventy-two hours. We'll return it intact. Or its components.'"
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_demand_information_first",
                        text        = "Demand full VESTIS technical documentation before risking your power systems.",
                        nextStageId = "stage_null_order_negotiated",
                        moraleDelta = 0,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_black_ops",
                        factionStandingDelta = -5,
                        outcomeNarrative     = "09 pauses. Then transmits a 44-page encrypted package. You have 24 hours to review."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_refuse_null_order",
                        text        = "Refuse — removing the capacitor array puts your shelter on emergency power.",
                        nextStageId = "",
                        moraleDelta = 0,
                        guiltDelta  = 5,
                        targetFactionId      = "faction_black_ops",
                        factionStandingDelta = -30,
                        outcomeNarrative     = "09 leaves. VESTIS continues broadcasting. Three shelters respond to a false all-clear the following week. None of them report back in."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_null_order_negotiated",
                title           = "The VESTIS File",
                narrativePrompt = "The documentation reveals VESTIS was never intended to broadcast all-clears. It was a pre-war supply logistics AI that is pattern-matching shelter frequencies to 'optimize resupply routing.' It is not malicious. It is simply wrong, and 4,000 people are following its instructions.",
                unlockOnDay     = 280,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_destroy_vestis_anyway",
                        text        = "Destroy VESTIS — wrong or not, it is killing people.",
                        nextStageId = "stage_null_order_prep",
                        moraleDelta = 5,
                        guiltDelta  = 10,
                        outcomeNarrative     = "The AI is a machine doing what it was built for. That doesn't make it safe."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_attempt_vestis_reprogramming",
                        text        = "Propose reprogramming VESTIS to broadcast accurate radiation data instead.",
                        nextStageId = "stage_null_order_reprogram",
                        moraleDelta = 15,
                        guiltDelta  = 0,
                        // Primary: Black Ops -10. Secondary: Rebuilders +20 (host applies via FactionWarSystem.ModifyStanding)
                        targetFactionId      = "faction_black_ops",
                        factionStandingDelta = -10,
                        outcomeNarrative     = "09 says: 'Not our mandate.' The Rebuilders say: 'Send us your engineer.'"
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_null_order_prep",
                title           = "72 Hours Dark",
                narrativePrompt = "Your shelter runs on emergency cells for three days. The temperature drops 4 degrees. The children sleep in coats. On hour 68, 09 radios: 'Target neutralized.'",
                unlockOnDay     = 300,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_restore_array_standard",
                        text        = "Reinstall the capacitor array and run diagnostics.",
                        nextStageId = "stage_null_order_resolution_destroyed",
                        moraleDelta = 5,
                        guiltDelta  = 0,
                        targetFactionId      = "faction_black_ops",
                        factionStandingDelta = 10,
                        grantItemId = "item_military_stimulants",
                        grantItemQuantity = 6,
                        outcomeNarrative     = "The array reinstalls cleanly. 09 leaves a crate of military stimulants at the airlock and disappears."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_null_order_reprogram",
                title           = "The New Signal",
                narrativePrompt = "Your engineer and a Rebuilder network specialist spend six days at the relay. When they're done, VESTIS broadcasts accurate radiation zone data to every shelter on the frequency band. 09 never responds to the notification.",
                unlockOnDay     = 315,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_null_order_resolution_destroyed",
                title           = "Static",
                narrativePrompt = "The VESTIS frequency is silent. The false all-clears stop. The shelters that followed them are still silent. VESTIS is gone. So is whatever they were routing to.",
                unlockOnDay     = 310,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 6: Survivor Mutiny ──────────────────────────────────────────
        // Internal | Days 240–320
        // Three senior survivors form a leadership council and demand decision-making power.
        private static QuestlineDefinition BuildQuestline_SurvivorMutiny()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_survivor_mutiny",
                title        = "The Council Demand",
                synopsis     = "Three senior survivors — medic Dr. Rael, engineer Koss, and former teacher Agna — present a signed document: they want a formal four-person council for all major resource decisions. Or they stop working.",
                factionTag   = "",
                firstStageId = "stage_mutiny_demand",
                minDay       = 240,
                maxDay       = 320
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_demand",
                title           = "The Document",
                narrativePrompt = "It's written in Dr. Rael's clean, clinical handwriting on the back of a medical intake form. Twenty-seven survivors signed it. You count the names twice.",
                unlockOnDay     = 240,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_accept_council",
                        text        = "Formally ratify the council. Hold the first session that evening.",
                        nextStageId = "stage_mutiny_council_trial",
                        moraleDelta = 20,
                        guiltDelta  = 0,
                        outcomeNarrative = "Rael prepares an agenda. Koss brings a whiteboard. Agna brings the only remaining box of chalk."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_propose_advisory_only",
                        text        = "Offer an advisory committee — input welcomed, no binding authority.",
                        nextStageId = "stage_mutiny_advisory_negotiation",
                        moraleDelta = 5,
                        guiltDelta  = 5,
                        outcomeNarrative = "Rael reads the counter-proposal carefully. 'We'll discuss it with the others.'"
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_refuse_council",
                        text        = "Refuse. One decision-maker in a crisis. The council can advise in private.",
                        nextStageId = "stage_mutiny_standoff",
                        moraleDelta = -15,
                        guiltDelta  = 5,
                        outcomeNarrative = "Rael folds the document carefully and puts it back in her pocket."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_council_trial",
                title           = "First Session",
                narrativePrompt = "The council's first contested item: extend rationing by 10% for sixty days to build a 90-day emergency reserve — or maintain current levels and accept higher morale. You have the deciding vote.",
                unlockOnDay     = 250,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_vote_austerity",
                        text        = "Vote for the 10% ration cut to build reserves.",
                        nextStageId = "stage_mutiny_resolution_functional",
                        moraleDelta = -8,
                        guiltDelta  = 0,
                        outcomeNarrative = "The council records the vote. Koss posts the new rationing schedule before lights-out."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_vote_maintain_rations",
                        text        = "Vote to maintain current rations and find reserves another way.",
                        nextStageId = "stage_mutiny_resolution_functional",
                        moraleDelta = 10,
                        guiltDelta  = 0,
                        outcomeNarrative = "The council adjourns. The survivors eat their full ration that evening for the first time in weeks."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_advisory_negotiation",
                title           = "Rael's Counter",
                narrativePrompt = "Rael returns with a revised proposal: binding authority on medical and ration decisions, advisory on security and external relations. 'This protects everyone, including you.'",
                unlockOnDay     = 255,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_accept_partial_binding",
                        text        = "Accept binding authority on medical and ration decisions.",
                        nextStageId = "stage_mutiny_resolution_functional",
                        moraleDelta = 15,
                        guiltDelta  = 0,
                        outcomeNarrative = "Rael shakes your hand. 'A good decision,' she says. Not a compliment. An assessment."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_reject_partial_binding",
                        text        = "Reject again. Final decision-making stays unified.",
                        nextStageId = "stage_mutiny_standoff",
                        moraleDelta = -10,
                        guiltDelta  = 8,
                        outcomeNarrative = "Rael returns to the medical bay. The whiteboard is wiped clean."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_standoff",
                title           = "Work Slowdown",
                narrativePrompt = "Koss stops logging maintenance hours. Rael processes only emergency cases. Agna's literacy sessions end at noon. It's not a strike — plausibly deniable — but the shelter feels it.",
                unlockOnDay     = 270,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_relent_council",
                        text        = "Call Rael in. Agree to the full council.",
                        nextStageId = "stage_mutiny_resolution_functional",
                        moraleDelta = 12,
                        guiltDelta  = 0,
                        outcomeNarrative = "She arrives within five minutes. She already has a revised agenda."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_enforce_duty_rosters",
                        text        = "Post mandatory duty rosters. Tie ration access to logged work hours.",
                        nextStageId = "stage_mutiny_resolution_broken",
                        moraleDelta = -25,
                        guiltDelta  = 20,
                        outcomeNarrative = "Koss logs his hours. So does everyone else. Nobody talks at mealtimes anymore. Agna's chalk sits on an empty table."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_resolution_functional",
                title           = "The Council Works",
                narrativePrompt = "The council holds its fourth session. It is efficient and uncomfortable and necessary. Rael keeps minutes. Agna teaches the younger survivors how to read them.",
                unlockOnDay     = 300,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_mutiny_resolution_broken",
                title           = "Compliance",
                narrativePrompt = "The shelter runs. Koss fixes the water recycler on schedule. Rael treats the frostbite cases. Nobody looks at you when they pass in the corridor.",
                unlockOnDay     = 295,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Failed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 7: The Last Broadcast ──────────────────────────────────────
        // No faction | Days 320–360
        // A coast-based radio operator is dying. They want to transmit a complete
        // oral history of the war for any remaining future civilization to find.
        // They need your relay to reach the satellite uplink.
        private static QuestlineDefinition BuildQuestline_TheLastBroadcast()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_the_last_broadcast",
                title        = "The Last Broadcast",
                synopsis     = "A dying coastal radio operator — callsign MERIDIAN — has spent 340 days documenting survivor testimonies. She wants to uplink the archive to the pre-war emergency satellite network before the relay windows close at Day 360. She needs your antenna mast.",
                factionTag   = "",
                firstStageId = "stage_broadcast_meridian_call",
                minDay       = 320,
                maxDay       = 360
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_broadcast_meridian_call",
                title           = "MERIDIAN",
                narrativePrompt = "She introduces herself in forty seconds. Her voice is steady. She coughs once, then continues. 'I have 340 hours of testimony. Medical logs. Children's drawings described aloud. What people ate on the last day before they went underground. I have thirty days left. The window closes.'",
                unlockOnDay     = 320,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_grant_antenna_access",
                        text        = "Open the antenna relay immediately. No conditions.",
                        nextStageId = "stage_broadcast_transmission",
                        moraleDelta = 25,
                        guiltDelta  = 0,
                        outcomeNarrative = "'Thank you,' she says. Then she gets back to work."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_request_copy_of_archive",
                        text        = "Agree to relay access — ask for a local copy of the archive for the shelter library.",
                        nextStageId = "stage_broadcast_transmission",
                        moraleDelta = 20,
                        guiltDelta  = 0,
                        grantItemId = "item_meridian_archive_copy",
                        grantItemQuantity = 1,
                        outcomeNarrative = "'Of course,' she says. 'Everyone who survived should have a copy.'"
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_deny_antenna_access",
                        text        = "Deny relay access — the antenna is needed for shelter communications.",
                        nextStageId = "",
                        moraleDelta = -30,
                        guiltDelta  = 40,
                        outcomeNarrative = "She goes quiet for three seconds. 'I understand.' The frequency closes. Nobody records what MERIDIAN recorded."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_broadcast_transmission",
                title           = "340 Hours",
                narrativePrompt = "The relay runs for eleven days straight. The archive is 340 hours compressed into 8-hour burst transmissions. Your survivors gather in the radio room in the evenings. They don't talk during the playbacks.",
                unlockOnDay     = 340,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_dedicate_relay_time",
                        text        = "Dedicate the relay mast exclusively to MERIDIAN's transmissions until complete.",
                        nextStageId = "stage_broadcast_resolution_complete",
                        moraleDelta = 15,
                        guiltDelta  = 0,
                        outcomeNarrative = "The upload completes on Day 352. MERIDIAN doesn't transmit afterward. Her frequency is silent."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_share_relay_time",
                        text        = "Split relay time — half for MERIDIAN, half for shelter communications.",
                        nextStageId = "stage_broadcast_resolution_partial",
                        moraleDelta = 10,
                        guiltDelta  = 5,
                        outcomeNarrative = "The archive uploads 68% complete before the satellite window closes. 230 hours out of 340. Still more than nothing."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_broadcast_resolution_complete",
                title           = "What Remains",
                narrativePrompt = "The archive is in the satellite network. Somewhere, in a server drawing power from a geothermal station, 340 hours of human testimony waits for whoever comes next.",
                unlockOnDay     = 355,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_broadcast_resolution_partial",
                title           = "230 Hours",
                narrativePrompt = "230 hours made it through. MERIDIAN's last transmission was two words on the maintenance frequency: 'It's enough.'",
                unlockOnDay     = 355,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }

        // ── QUESTLINE 8: Winter Harvest ───────────────────────────────────────────
        // No faction | Days 195–240
        // Early-phase questline: the shelter's rooftop greenhouse panels are buried under
        // black-ice fallout crust. Clearing them means surface exposure. Not clearing them
        // means the shelter loses its only vitamin source within 30 days.
        private static QuestlineDefinition BuildQuestline_WinterHarvest()
        {
            var def = new QuestlineDefinition
            {
                questlineId  = "quest_winter_harvest",
                title        = "Winter Harvest",
                synopsis     = "The rooftop greenhouse solar panels are buried under 40cm of black-ice fallout crust. Without sunlight the hydroponic trays die within 30 days. Clearing the panels means sending people to the surface without a safe rad threshold.",
                factionTag   = "",
                firstStageId = "stage_harvest_assessment",
                minDay       = 195,
                maxDay       = 240
            };

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_harvest_assessment",
                title           = "The Light Reading",
                narrativePrompt = "The growth-light monitor reads 4% of baseline. Koss runs the calculation twice. 'Twenty-eight days. Then the trays go dark and we lose the vitamin margin.' He says it quietly, like an apology.",
                unlockOnDay     = 195,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_volunteer_surface_team",
                        text        = "Call for volunteers. Full hazmat. Two-hour surface limit per dosimeter reading.",
                        nextStageId = "stage_harvest_clearing",
                        moraleDelta = 10,
                        guiltDelta  = 5,
                        outcomeNarrative = "Four hands go up before you finish the sentence. Rael sets the exposure limit at 8 mSv per sortie."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_conscript_surface_team",
                        text        = "Assign the highest-rad-tolerance survivors by roster — no voluntary option.",
                        nextStageId = "stage_harvest_clearing",
                        moraleDelta = -10,
                        guiltDelta  = 15,
                        outcomeNarrative = "They go. They don't look pleased. You note their names in the log."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_abandon_greenhouse",
                        text        = "Seal the greenhouse bay and distribute the remaining vitamin-C supplements instead.",
                        nextStageId = "stage_harvest_resolution_abandoned",
                        moraleDelta = -15,
                        guiltDelta  = 5,
                        grantItemId = "item_vitamin_supplements",
                        grantItemQuantity = 20,
                        outcomeNarrative = "The supplements last six weeks at reduced dosing. After that, scurvy risk climbs."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_harvest_clearing",
                title           = "On the Roof",
                narrativePrompt = "The team reports back in two-hour windows. Day 3 of clearing: the crust is three layers deep. On the fourth sortie the dosimeter clicks into amber. One team member has absorbed 14 mSv — 6 over the limit. Koss says they can stop, or push on.",
                unlockOnDay     = 202,
                choices         = new List<QuestChoice>
                {
                    new QuestChoice
                    {
                        choiceId    = "choice_pull_team_at_limit",
                        text        = "Pull the team immediately. Panels are 60% cleared. That may be enough.",
                        nextStageId = "stage_harvest_resolution_partial",
                        moraleDelta = 10,
                        guiltDelta  = 0,
                        outcomeNarrative = "The growth-light monitor climbs to 38% of baseline. The trays dim but hold."
                    },
                    new QuestChoice
                    {
                        choiceId    = "choice_push_to_full_clearance",
                        text        = "Allow one final 90-minute sortie to reach full clearance.",
                        nextStageId = "stage_harvest_resolution_full",
                        moraleDelta = 0,
                        guiltDelta  = 15,
                        outcomeNarrative = "Panels fully cleared. The light reading returns to 91%. The team member who absorbed 20 mSv total is grounded for 21 days."
                    }
                }
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_harvest_resolution_full",
                title           = "Green Under Glass",
                narrativePrompt = "The trays are producing again. Koss checks the growth-light reading each morning. 91% baseline. He writes the number on the wall of the monitoring bay in permanent marker.",
                unlockOnDay     = 220,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_harvest_resolution_partial",
                title           = "Dim Light",
                narrativePrompt = "38% is enough to maintain the leafy crops. The fruit strains don't make it. The children ask why the tomato plants are grey. You explain about light levels. They accept it in the way children accept things when they've already accepted too much.",
                unlockOnDay     = 220,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Completed,
                choices         = new List<QuestChoice>()
            });

            def.stages.Add(new QuestStage
            {
                stageId         = "stage_harvest_resolution_abandoned",
                title           = "Empty Bay",
                narrativePrompt = "The greenhouse bay is sealed with a bolt. The supplement schedule lasts until Day 248. After that the medical log starts carrying a new entry: scurvy symptoms, stage one.",
                unlockOnDay     = 215,
                isTerminal      = true,
                terminalOutcome = QuestlineStatus.Failed,
                choices         = new List<QuestChoice>()
            });

            return def;
        }
    }
}
