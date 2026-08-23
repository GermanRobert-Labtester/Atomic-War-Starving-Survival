using System;
using System.Collections.Generic;

namespace Ashfall.Core.Quests
{
    /// <summary>
    /// Holdfast-specific quest definitions with hostile elements, faction interactions,
    /// and creative writing for the District 8 expansion.
    /// </summary>
    public static class HoldfastQuests
    {
        // Main Questline
        public static class Main
        {
            public static QuestDefinition TheSheet => new QuestDefinition(
                id: "quest_holdfast_the_sheet",
                displayName: "The Sheet That Shouldn't",
                type: QuestType.Expedition,
                description: "Bram will not say who walked the estuary. The waxed paper shows a road that shouldn't exist on a Sector 4 map.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_sheet_acquire",
                        description: "Obtain the ice road map from Ostrowski or the Toll",
                        completionText: "The map is in your hands, showing a route that defies Sector 4's geography.",
                        requiresItem: "item_map_sheet_ice_road"
                    ),
                    new QuestObjective(
                        id: "obj_sheet_compare",
                        description: "Compare the map to your Kittiwake log if you have one",
                        completionText: "The discrepancies are glaring. The estuary is supposed to be a dead zone in winter.",
                        requiresKnowledge: "lore_hf_sheet"
                    ),
                    new QuestObjective(
                        id: "obj_sheet_ask_lamplighter",
                        description: "Ask a Lamplighter about Kilometre 19",
                        completionText: "Ivy Corrigan won't cross it. She confirms the post exists, but won't say why it's significant.",
                        requiresNpcInteraction: "npc_ivy_corrigan"
                    ),
                    new QuestObjective(
                        id: "obj_sheet_survive",
                        description: "Survive the questioning",
                        completionText: "The Lamplighter's silence is heavier than the ash on your boots."
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_map_sheet_ice_road",
                        quantity: 1
                    ),
                    new QuestReward(
                        type: QuestRewardType.TravelTimeHint,
                        id: "ice_road_travel_hours",
                        value: "6.0-8.0"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_hf_sheet"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Ostrowski becomes suspicious of future dealings",
                    "The Tollman marks your name in his ledger",
                    "Ivy Corrigan refuses to acknowledge you in future"
                },
                hostileElements: new List<string>
                {
                    "Ostrowski's curiosity turns to paranoia",
                    "The map may be a forgery designed to lure you into a trap",
                    "Ivy Corrigan's refusal carries a warning"
                }
            );

            public static QuestDefinition TheClerk => new QuestDefinition(
                id: "quest_holdfast_the_clerk",
                displayName: "The Return",
                type: QuestType.Shelter,
                description: "Edor Vale at the weighbridge with a census form. He offers to read it twice, but his eyes linger on the blank spaces.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_clerk_hear_form",
                        description: "Hear the census form read aloud",
                        completionText: "Edor's voice is polite, but his pencil hovers over the blank spaces like a judge's gavel.",
                        requiresNpcInteraction: "npc_edor_vale"
                    ),
                    new QuestObjective(
                        id: "obj_clerk_confirm_occupations",
                        description: "Confirm or deny three survivor occupations",
                        completionText: "The truth or lie is now on paper. Edor's smile doesn't reach his eyes.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_clerk_wait_near_hatch",
                        description: "Choose whether Edor may wait near your hatch",
                        completionText: "Edor's presence is a silent accusation. The bunker feels smaller with him inside.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_clerk_show_sela",
                        description: "Optional: Show Sela's card if you have it",
                        completionText: "Edor's pencil pauses. The Archivist's card changes everything.",
                        requiresItem: "item_archivist_card"
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_census_return_blank",
                        quantity: 1
                    ),
                    new QuestReward(
                        type: QuestRewardType.FactionTrust,
                        id: "faction_the_office_trust",
                        value: 0.2f
                    ),
                    new QuestReward(
                        type: QuestRewardType.TollReceipt,
                        id: "toll_receipt_census"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Edor files an incomplete return",
                    "The Office marks your bunker for audit",
                    "Sela's status becomes more complicated"
                },
                hostileElements: new List<string>
                {
                    "Edor's pencil is a weapon",
                    "The census form is a legal document",
                    "Your survivors' occupations are now on record"
                }
            );

            public static QuestDefinition TheWindow => new QuestDefinition(
                id: "quest_holdfast_the_window",
                displayName: "When the Cut Takes",
                type: QuestType.Expedition,
                description: "Yara Holm opens the Gate. The first freeze window is 14 days. The ice road is a death sentence for the unprepared.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_window_outfit_run",
                        description: "Outfit a 3-person expedition with warmth gear, iodine, food, and welders glass",
                        completionText: "The supplies are packed. The ice road awaits. Yara's ledger is already marked with your names.",
                        requiresItems: new List<string> { "item_warmth_gear", "iodine_pills", "canned_food", "item_welders_glass" }
                    ),
                    new QuestObjective(
                        id: "obj_window_cross_to_waystation",
                        description: "Cross to Waystation A",
                        completionText: "The Gate is behind you. The Cutters' light is ahead. The ice groans beneath your boots.",
                        requiresLocation: "loc_cut_waystation_a"
                    ),
                    new QuestObjective(
                        id: "obj_window_no_dark_ice",
                        description: "Do not walk marked-dark ice",
                        completionText: "Yara's warning echoes in your ears. The dark ice is a trap.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_window_return_or_winter",
                        description: "Return or winter the last bunk",
                        completionText: "The window is still open. Or is it? The ice road doesn't forgive mistakes.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.LocationUnlock,
                        id: "loc_cut_waystation_a_unlocked"
                    ),
                    new QuestReward(
                        type: QuestRewardType.FactionAccess,
                        id: "faction_the_cutters_access"
                    ),
                    new QuestReward(
                        type: QuestRewardType.InjuryRisk,
                        id: "freeze_injury_risk"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Expedition members suffer freeze injuries",
                    "Waystation A is marked as unsafe",
                    "Yara withdraws her light from your section of the road"
                },
                hostileElements: new List<string>
                {
                    "The ice road is a death trap",
                    "Yara's ledger is a contract",
                    "Dark ice is a Cutter's warning"
                }
            );

            public static QuestDefinition ThePlant => new QuestDefinition(
                id: "quest_holdfast_the_plant",
                displayName: "In Situ Essential",
                type: QuestType.Expedition,
                description: "The desalination plant is staffed. Leva Quist's minutes are current. The plant is failing, and the Cluster's warmth depends on it.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_plant_enter_grade_hut",
                        description: "Enter the Grade Hut",
                        completionText: "The air smells of iodine and failure. Leva's minutes are spread across the table like a death sentence.",
                        requiresLocation: "loc_salt_grade_hut"
                    ),
                    new QuestObjective(
                        id: "obj_plant_tour_membrane_hall",
                        description: "Tour the Membrane Hall (accept rad and fume exposure)",
                        completionText: "The membranes are failing. The air is thick with resin fumes. Leva's cough is a metronome.",
                        requiresLocation: "loc_salt_membrane_hall"
                    ),
                    new QuestObjective(
                        id: "obj_plant_deliver_or_refuse_resin",
                        description: "Deliver or refuse a resin gift",
                        completionText: "Leva's eyes narrow. The plant's fate is in your hands.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_plant_see_steam_line",
                        description: "See the steam line toward the Cluster",
                        completionText: "The steam is visible on the horizon. The Cluster's warmth depends on it. The plant's failure is the Cluster's failure.",
                        requiresLocation: "loc_salt_cooling_canal"
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.TradeUnlock,
                        id: "salt_trade_unlocked"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_ro_resin_spent_sample",
                        quantity: 1
                    ),
                    new QuestReward(
                        type: QuestRewardType.LocationFlag,
                        id: "location_abandoned_desalination_recast"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Plant integrity drops further",
                    "Leva's trust in you evaporates",
                    "The Cluster's warmth clock starts ticking"
                },
                hostileElements: new List<string>
                {
                    "Resin fumes are toxic",
                    "Leva's minutes are a countdown",
                    "The plant's failure is inevitable"
                }
            );

            public static QuestDefinition Authentication => new QuestDefinition(
                id: "quest_holdfast_authentication",
                displayName: "Take a Number",
                type: QuestType.Expedition,
                description: "Cluster Gatehouse. Allocation 12 is a known discrepancy. The Office will not be denied.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_auth_state_number",
                        description: "State a number or none",
                        completionText: "The Gatehouse clerk's expression doesn't change. The discrepancy is noted.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_auth_accept_block_c",
                        description: "Accept Block C or sleep the Gatehouse floor",
                        completionText: "The numbered apartment is sterile. The playground chains are still there. The brass nameplates are missing.",
                        requiresLocation: "loc_cluster_block_c"
                    ),
                    new QuestObjective(
                        id: "obj_auth_walk_quad",
                        description: "Walk the Quad",
                        completionText: "The hydroponic troughs are failing. The noticeboard has a labor rota and a strip of missing trades. One of them is yours.",
                        requiresLocation: "loc_cluster_quad"
                    ),
                    new QuestObjective(
                        id: "obj_auth_no_brass_from_playground",
                        description: "Do not take brass from the playground (or do)",
                        completionText: "The brass is in your pack. The Chain is still there. The choice is yours.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.GuestHousing,
                        id: "block_c_guest_housing"
                    ),
                    new QuestReward(
                        type: QuestRewardType.ClinicAccess,
                        id: "cluster_clinic_access"
                    ),
                    new QuestReward(
                        type: QuestRewardType.MoraleEvent,
                        id: "morale_event_guest_housing"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Guest housing is revoked",
                    "Clinic access is restricted",
                    "Morale drops across your survivors"
                },
                hostileElements: new List<string>
                {
                    "The numbered apartment is a gilded cage",
                    "The playground brass is a temptation",
                    "The Office's records are immutable"
                }
            );

            public static QuestDefinition TheDrawer => new QuestDefinition(
                id: "quest_holdfast_the_drawer",
                displayName: "The Drawer",
                type: QuestType.Exploration,
                description: "Ormund opens the Sector 4 Schedule. Names you know. Names you buried. The Office's Schedule is complete. Yours is not.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_drawer_read_sole",
                        description: "Read Sole's entry",
                        completionText: "Margit Sole's name is there. Not allocated. Score 41.2. The Office's Schedule is complete.",
                        requiresKnowledge: "lore_pre_allocation_letters"
                    ),
                    new QuestObjective(
                        id: "obj_drawer_read_renn",
                        description: "Read Renn's entry",
                        completionText: "Halvard Renn's name is there. Allocated. Not arrived. 12-B unconfirmed. The discrepancy is official.",
                        requiresKnowledge: "lore_af_the_claim"
                    ),
                    new QuestObjective(
                        id: "obj_drawer_search_frayne",
                        description: "Search for Frayne's entry (it's absent)",
                        completionText: "Ottilie Frayne's name is not there. RUR 11. The Office's Schedule is a lie.",
                        requiresKnowledge: "lore_bs_the_vault_holds"
                    ),
                    new QuestObjective(
                        id: "obj_drawer_ask_about_12c",
                        description: "Optional: Ask Ormund about Order 12-C",
                        completionText: "Ormund's smile is a knife. The Order is real. The unlisted are a labor reserve.",
                        requiresNpcInteraction: "npc_cael_ormund"
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_hf_two_schedules"
                    ),
                    new QuestReward(
                        type: QuestRewardType.CodexDump,
                        id: "codex_drawer_contents"
                    ),
                    new QuestReward(
                        type: QuestRewardType.StressEvent,
                        id: "stress_event_parent_survivors"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Ormund marks your bunker for audit",
                    "The Office's suspicion grows",
                    "Your survivors' stress levels rise"
                },
                hostileElements: new List<string>
                {
                    "The Schedule is a legal weapon",
                    "Ormund's knife is a smile",
                    "The discrepancy is official"
                }
            );

            public static QuestDefinition TheLevy => new QuestDefinition(
                id: "quest_holdfast_the_levy",
                displayName: "Reconstruction Pool",
                type: QuestType.Faction,
                description: "Three names. Thirty days. The ice will not wait for a better feeling. Ormund wants his labor reserve.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_levy_review_names",
                        description: "Review the three named survivors",
                        completionText: "The names are yours. The choice is not.",
                        requiresSurvivorSelection: true
                    ),
                    new QuestObjective(
                        id: "obj_levy_honor_substitute_refuse",
                        description: "Honor / substitute / refuse the levy",
                        completionText: "The ink is dry. The ice road is open. The choice is final.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_levy_kit_for_salt",
                        description: "If sending survivors: kit them for salt and UV exposure",
                        completionText: "The survivors are prepared. The ice road awaits. Ormund's ledger is marked.",
                        requiresItems: new List<string> { "iodine_pills", "item_welders_glass", "canned_food" }
                    ),
                    new QuestObjective(
                        id: "obj_levy_inform_remaining",
                        description: "Inform the remaining shelter of the levy (morale impact)",
                        completionText: "The bunker is quieter. The survivors who remain are uneasy. The choice was yours.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.BranchFlags,
                        id: "levy_honor_substitute_refuse_flags"
                    ),
                    new QuestReward(
                        type: QuestRewardType.TradeRates,
                        id: "salt_trade_rates_improved"
                    ),
                    new QuestReward(
                        type: QuestRewardType.CompanionLockIn,
                        id: "companion_lock_in_possible"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Ormund's suspicion grows",
                    "The Office marks your bunker for audit",
                    "The named survivors may refuse to return"
                },
                hostileElements: new List<string>
                {
                    "The levy is a legal order",
                    "The ice road is a death trap",
                    "Ormund's ledger is a contract"
                }
            );

            public static QuestDefinition TheMembrane => new QuestDefinition(
                id: "quest_holdfast_the_membrane",
                displayName: "Forty-Eight Hours",
                type: QuestType.Crisis,
                description: "Membrane bank trips. Cluster steam clock starts. The plant is failing. The Cluster's warmth is a countdown.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_membrane_diagnose",
                        description: "Diagnose the membrane failure with Leva",
                        completionText: "Leva's face is grim. The plant has 48 hours. The Cluster's warmth is a countdown.",
                        requiresNpcInteraction: "npc_leva_quist"
                    ),
                    new QuestObjective(
                        id: "obj_membrane_gather_resources",
                        description: "Gather resin, brass, iodine, and 2 workers for the outfall shift",
                        completionText: "The resources are gathered. The outfall shift is staffed. The plant's fate is in your hands.",
                        requiresItems: new List<string> { "item_ro_resin", "brass_fittings", "iodine_pills" }
                    ),
                    new QuestObjective(
                        id: "obj_membrane_outfall_shift",
                        description: "Work the outfall shift (health risk)",
                        completionText: "The outfall shift is complete. The plant's integrity is restored. The Cluster's warmth is safe. For now.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_membrane_sector4_strip_let_drop",
                        description: "Choose: strip Sector 4 or let the steam die",
                        completionText: "The choice is made. The Cluster lives. Or the Cluster dies. The plant's failure is inevitable.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.PlantState,
                        id: "plant_integrity_restored"
                    ),
                    new QuestReward(
                        type: QuestRewardType.ClusterIndoorTemp,
                        id: "cluster_indoor_temp_safe"
                    ),
                    new QuestReward(
                        type: QuestRewardType.RebuildersHegemonyDelta,
                        id: "rebuilders_hegemony_delta"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Cluster indoor temperature drops",
                    "Plant integrity drops further",
                    "Medical market shock"
                },
                hostileElements: new List<string>
                {
                    "The plant's failure is inevitable",
                    "The outfall shift is a health risk",
                    "The choice is a death sentence"
                }
            );

            public static QuestDefinition TheSecondList => new QuestDefinition(
                id: "quest_holdfast_the_second_list",
                displayName: "Order 12-C",
                type: QuestType.Story,
                description: "The labor reserve clause. Ormund will come south. The Order is real. The unlisted are a legal fiction.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_second_list_obtain_copy",
                        description: "Obtain a copy of Order 12-C",
                        completionText: "The Order is in your hands. The ink is dry. The unlisted are a labor reserve.",
                        requiresItem: "item_order_12c"
                    ),
                    new QuestObjective(
                        id: "obj_second_list_carry_to_sole",
                        description: "Optional: Carry the copy to Sole",
                        completionText: "Sole files it. She does not sign it. The Order is real. The unlisted are a legal fiction.",
                        requiresNpcInteraction: "npc_margit_sole"
                    ),
                    new QuestObjective(
                        id: "obj_second_list_show_voss",
                        description: "Optional: Show the Order to Voss",
                        completionText: "Voss wants the reconstruction pool. The Order is a weapon.",
                        requiresNpcInteraction: "npc_colonel_voss"
                    ),
                    new QuestObjective(
                        id: "obj_second_list_prepare_hatch",
                        description: "Prepare the hatch for Ormund's arrival",
                        completionText: "The hatch is ready. The Order is real. The unlisted are a legal fiction.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_order_12c"
                    ),
                    new QuestReward(
                        type: QuestRewardType.ThreateningProse,
                        id: "threatening_body_text_unlocked"
                    ),
                    new QuestReward(
                        type: QuestRewardType.VossOrmundTriangle,
                        id: "voss_ormund_triangle"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Ormund's arrival is accelerated",
                    "The threatening prose is unlocked",
                    "Voss and Ormund's rivalry intensifies"
                },
                hostileElements: new List<string>
                {
                    "The Order is a legal weapon",
                    "Ormund's arrival is inevitable",
                    "The unlisted are a legal fiction"
                }
            );

            public static QuestDefinition TheHatch => new QuestDefinition(
                id: "quest_holdfast_the_hatch",
                displayName: "The Claim, Reversed",
                type: QuestType.Shelter,
                description: "Forms at the outer hatch. Escort in faded Continuity jackets. Temperature. The choice is final.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_hatch_open_or_keep_shut",
                        description: "Open or keep the hatch shut",
                        completionText: "The choice is made. The forms are signed. The temperature is noted. The choice is final.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_hatch_authenticate_house_or_levy",
                        description: "If open: authenticate, house the escort, or levy them",
                        completionText: "The escort is housed. The forms are signed. The temperature is noted. The choice is final.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_hatch_wait_40_days",
                        description: "If shut: wait 40 days (quiet)",
                        completionText: "The hatch remains shut. The 40 days pass. The choice is final.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_hatch_write_nothing_or_write",
                        description: "Write nothing on the duty roster, or write the names",
                        completionText: "The duty roster is blank. Or it is not. The choice is final.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.EndingFlag,
                        id: "ending_flag"
                    ),
                    new QuestReward(
                        type: QuestRewardType.HistorySecondParagraph,
                        id: "world_history_second_paragraph"
                    ),
                    new QuestReward(
                        type: QuestRewardType.VictorySlide,
                        id: "victory_the_holdfast_slide"
                    )
                },
                failureConsequences: new List<string>
                {
                    "The ending is locked",
                    "The history second paragraph is lost",
                    "The victory slide is not shown"
                },
                hostileElements: new List<string>
                {
                    "The forms are a legal weapon",
                    "The temperature is a countdown",
                    "The choice is final"
                }
            );
        }

        // Side Quests
        public static class Side
        {
            public static QuestDefinition SaltResinCount => new QuestDefinition(
                id: "quest_salt_resin_count",
                displayName: "Audit the Resin",
                type: QuestType.Expedition,
                description: "The drum count is short every Tuesday. Nobody is stealing. The spent stack is growing. The plant's failure is a countdown.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_resin_audit_drums",
                        description: "Audit the resin drums",
                        completionText: "The count is short. The discrepancy is noted. Leva's minutes are updated.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_resin_follow_night_shift",
                        description: "Follow a night shift",
                        completionText: "The night shift is uneventful. The discrepancy is not theft. The plant is failing.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_resin_find_evaporation",
                        description: "Find the evaporation, not theft",
                        completionText: "The evaporation is confirmed. The plant's failure is inevitable. The resin is gone.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_resin_recoat_or_write_off",
                        description: "Recoat membranes or write off the loss",
                        completionText: "The choice is made. The plant's integrity is restored. For now.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_ro_resin_x2",
                        quantity: 2
                    ),
                    new QuestReward(
                        type: QuestRewardType.FactionTrust,
                        id: "faction_the_office_trust_increased"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Recipe,
                        id: "recipe_resin_recoat_low_yield"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Plant integrity drops further",
                    "Leva's trust in you evaporates",
                    "Salt/Office friction increases"
                },
                hostileElements: new List<string>
                {
                    "The resin drums are a countdown",
                    "The plant's failure is inevitable",
                    "Leva's minutes are a death sentence"
                }
            );

            public static QuestDefinition SaltOutfallLimit => new QuestDefinition(
                id: "quest_salt_outfall_limit",
                displayName: "Outfall Shift Limits",
                type: QuestType.Expedition,
                description: "Shift limits exist on paper. They are not kept. Salt-rash is up. The outfall workers are getting sick.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_outfall_work_limited_shift",
                        description: "Work one limited shift",
                        completionText: "The shift limit is respected. The salt-rash cases drop. The outfall workers are healthier.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_outfall_work_unlimited",
                        description: "Work one unlimited shift (or refuse)",
                        completionText: "The shift limit is ignored. The salt-rash cases rise. The outfall workers are sicker.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_outfall_bring_iodine_protocol",
                        description: "Bring iodine protocol from Cluster Clinic or Ianov",
                        completionText: "The protocol is shared. The salt-rash cases drop. The outfall workers are healthier.",
                        requiresItems: new List<string> { "iodine_pills" }
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.AfflictionKnowledge,
                        id: "affliction_salt_rash_knowledge"
                    ),
                    new QuestReward(
                        type: QuestRewardType.ClinicSaltFriction,
                        id: "clinic_salt_friction"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Antiseptic,
                        id: "antiseptic_item"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Salt-rash cases rise",
                    "Clinic/Salt friction increases",
                    "Outfall workers' health declines"
                },
                hostileElements: new List<string>
                {
                    "The outfall is a health risk",
                    "The shift limits are a legal fiction",
                    "The salt-rash is a countdown"
                }
            );

            public static QuestDefinition SaltBrassSeats => new QuestDefinition(
                id: "quest_salt_brass_seats",
                displayName: "The Brass Seats",
                type: QuestType.Faction,
                description: "Valve seats. Playground. Tin behind your filter. Leva will not ask where the brass comes from. She will not say what it's for.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_brass_deliver_8_fittings",
                        description: "Deliver 8 brass fittings",
                        completionText: "The fittings are delivered. The valve seats are repaired. The plant's integrity is restored. For now.",
                        requiresItems: new List<string> { "brass_fittings" },
                        quantity: 8
                    ),
                    new QuestObjective(
                        id: "obj_brass_deliver_none",
                        description: "Deliver none and watch a leak scheduled",
                        completionText: "The leak is scheduled. The plant's failure is inevitable. The choice is yours.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.SteamStability,
                        id: "plant_steam_stable"
                    ),
                    new QuestReward(
                        type: QuestRewardType.SilentNameplateFlag,
                        id: "nameplate_sold_north_flag"
                    ),
                    new QuestReward(
                        type: QuestRewardType.WorksPriceShock,
                        id: "ottilie_frayne_price_shock"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Plant integrity drops further",
                    "Leva's trust in you evaporates",
                    "Works price shock"
                },
                hostileElements: new List<string>
                {
                    "The brass fittings are a mystery",
                    "The valve seats are a countdown",
                    "Leva's silence is a weapon"
                }
            );

            public static QuestDefinition OfficeMissingStrip => new QuestDefinition(
                id: "quest_office_missing_strip",
                displayName: "The Missing Strip",
                type: QuestType.Exploration,
                description: "Sector 4 trades listed as missing on the Cluster noticeboard. One name is a survivor you have, living. The Office's bureaucracy is complete. Yours is not.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_strip_match_name",
                        description: "Match the strip to a survivor you have",
                        completionText: "The name matches. The survivor is living. The Office's bureaucracy is complete. Yours is not.",
                        requiresSurvivorSelection: true
                    ),
                    new QuestObjective(
                        id: "obj_strip_tell_or_dont",
                        description: "Tell them, or don't",
                        completionText: "The choice is made. The Office's bureaucracy is complete. Yours is not.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_strip_file_retrieval",
                        description: "If told, they will file a retrieval",
                        completionText: "The retrieval is filed. The Office's bureaucracy is complete. Yours is not.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Morale,
                        id: "morale_event_strip"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Codex,
                        id: "codex_entry_strip"
                    ),
                    new QuestReward(
                        type: QuestRewardType.PossibleRetrievalEvent,
                        id: "retrieval_event_possible"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Morale drops",
                    "Codex entry is lost",
                    "Retrieval event is delayed"
                },
                hostileElements: new List<string>
                {
                    "The noticeboard is a legal weapon",
                    "The Office's bureaucracy is complete",
                    "Your bureaucracy is a lie"
                }
            );

            public static QuestDefinition OfficeSchoolSum => new QuestDefinition(
                id: "quest_office_school_sum",
                displayName: "School Summation",
                type: QuestType.Exploration,
                description: "Children adding RUR scores as homework. A dependent is worth points. The Office's ideology is complete. Yours is not.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_school_sit_lesson",
                        description: "Sit the lesson",
                        completionText: "The children's sums are neat. The ideology is complete. Yours is not.",
                        requiresLocation: "loc_cluster_school"
                    ),
                    new QuestObjective(
                        id: "obj_school_correct_sum",
                        description: "Correct a sum or let it stand",
                        completionText: "The sum is corrected. The ideology is complete. Yours is not.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_school_wren_present",
                        description: "If Wren is present: record what you tell her",
                        completionText: "Wren's eyes are wide. The ideology is complete. Yours is not.",
                        requiresNpcInteraction: "npc_wren"
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.MoraleSplit,
                        id: "morale_split_school"
                    ),
                    new QuestReward(
                        type: QuestRewardType.WrenTruthFlag,
                        id: "wren_truth_flag"
                    ),
                    new QuestReward(
                        type: QuestRewardType.NoItems,
                        id: "no_items_reward"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Morale splits",
                    "Wren's trust is broken",
                    "No items are rewarded"
                },
                hostileElements: new List<string>
                {
                    "The children's sums are a weapon",
                    "The ideology is complete",
                    "Your ideology is a lie"
                }
            );

            public static QuestDefinition OfficeFortyRooms => new QuestDefinition(
                id: "quest_office_forty_rooms",
                displayName: "The Forty Rooms",
                type: QuestType.Exploration,
                description: "Forty apartments kept for arrivals. Dusty. The Office's hope is complete. Yours is not.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_forty_walk_three",
                        description: "Walk three of the forty rooms",
                        completionText: "The rooms are dusty. The hope is complete. Yours is not.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_forty_find_children_boots",
                        description: "Find children's boots sizes 1-4 (mirrors your crate)",
                        completionText: "The boots are found. The hope is complete. Yours is not.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_forty_leave_them_or_take",
                        description: "Leave them or take them",
                        completionText: "The choice is made. The hope is complete. Yours is not.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_hf_forty_rooms"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Morale,
                        id: "morale_event_forty_rooms"
                    ),
                    new QuestReward(
                        type: QuestRewardType.BootsAsWarmthItems,
                        id: "boots_as_warmth_items"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Morale drops",
                    "Knowledge key is lost",
                    "Boots are not rewarded"
                },
                hostileElements: new List<string>
                {
                    "The forty rooms are a countdown",
                    "The hope is complete",
                    "Your hope is a lie"
                }
            );

            public static QuestDefinition CutDarkLamp => new QuestDefinition(
                id: "quest_cut_dark_lamp",
                displayName: "The Dark Lamp",
                type: QuestType.Expedition,
                description: "A beacon is dark during a window. Accident 12's cousin. Yara's rule is absolute: dark means do not cross.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_lamp_walk_dark_stretch",
                        description: "Walk the dark stretch",
                        completionText: "The dark stretch is treacherous. The beacon is dark. Yara's rule is absolute.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_lamp_relight_or_leave",
                        description: "Relight the beacon or leave it dark",
                        completionText: "The choice is made. The beacon is relit. Yara's trust is restored. Or the beacon remains dark. Yara withdraws.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_lamp_trap",
                        description: "If relight for a trap, Yara withdraws (Corrigan rule, northern)",
                        completionText: "The trap is sprung. Yara withdraws. The Cutters' light is gone. The ice road is closed.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.RoadSafety,
                        id: "ice_road_safe"
                    ),
                    new QuestReward(
                        type: QuestRewardType.CutterTrust,
                        id: "faction_the_cutters_trust"
                    ),
                    new QuestReward(
                        type: QuestRewardType.AccessLost,
                        id: "cutters_access_lost"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Yara withdraws permanently",
                    "Ice Road access lost",
                    "Accident risk increases"
                },
                hostileElements: new List<string>
                {
                    "Yara's rule is absolute",
                    "The beacon is a contract",
                    "Dark ice is a death sentence"
                }
            );

            // Fleet faction side quests
            public static QuestDefinition FleetSchedule => new QuestDefinition(
                id: "quest_fleet_schedule",
                displayName: "Fixed Frequency",
                type: QuestType.Expedition,
                description: "A voice on a fixed frequency, fixed time. Not D/9's. The Fleet was told to wait for a stand-up order that never came.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_fleet_listen_three_nights",
                        description: "Listen three nights at the scheduled time",
                        completionText: "The voice is consistent. The schedule is real. The wait is not over.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_fleet_answer_once",
                        description: "Answer once with authentication or without",
                        completionText: "The voice acknowledges. The Fleet is still waiting. The stand-up is still not signed.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_fleet_meet_mire_ashore",
                        description: "Meet Mire ashore or not",
                        completionText: "Mire is ashore. The Fleet is still waiting. The choice is yours.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_fleet_schedule"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Item,
                        id: "item_fleet_schedule_fragment"
                    ),
                    new QuestReward(
                        type: QuestRewardType.CompanionUnlock,
                        id: "companion_mire_unlocked"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Mire's trust is broken",
                    "The Fleet's schedule remains unanswered",
                    "No companion unlock"
                },
                hostileElements: new List<string>
                {
                    "The Fleet's wait is not over",
                    "The stand-up order is still unsigned",
                    "Mire's patience is finite"
                }
            );

            public static QuestDefinition FleetPad => new QuestDefinition(
                id: "quest_fleet_pad",
                displayName: "Stand-Up Authentication",
                type: QuestType.Exploration,
                description: "He wants a stand-up. Sole's D/9 form will not verify. The Fleet's authentication system is the same family as D/9's, but it is not the same.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_fleet_show_sole_paper",
                        description: "Show Sole's paper if owned",
                        completionText: "The pad does not authenticate. The Fleet's stand-up is still not signed.",
                        requiresItem: "item_archivist_card"
                    ),
                    new QuestObjective(
                        id: "obj_fleet_find_fleet_annex",
                        description: "Find the Fleet annex in Ministry files",
                        completionText: "The annex is found. The Fleet's authentication system is documented. The stand-up is still not signed.",
                        requiresLocation: "loc_ministry_fleet_annex"
                    ),
                    new QuestObjective(
                        id: "obj_fleet_accept_wait",
                        description: "Accept that some waits do not end",
                        completionText: "The choice is made. The Fleet continues to wait. The stand-up is still not signed.",
                        requiresPlayerChoice: true
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_fleet_authentication"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Morale,
                        id: "morale_event_fleet_wait"
                    )
                },
                failureConsequences: new List<string>
                {
                    "Mire's arrival is accelerated",
                    "The Fleet's wait becomes more desperate",
                    "No morale event"
                },
                hostileElements: new List<string>
                {
                    "The Fleet's authentication is not the same",
                    "The stand-up order is still unsigned",
                    "Some waits do not end"
                }
            );

            public static QuestDefinition FleetBoarding => new QuestDefinition(
                id: "quest_fleet_boarding",
                displayName: "Without Blasting",
                type: QuestType.Expedition,
                description: "Boarding *Hearth-4* without blasting. The hatch wants a number. The Fleet does not forget.",
                objectives: new List<QuestObjective>
                {
                    new QuestObjective(
                        id: "obj_fleet_authenticate_with_companion",
                        description: "Authenticate with an allocated companion or fail",
                        completionText: "The hatch opens. The Fleet is aboard. The choice is yours.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_fleet_inventory_living",
                        description: "Inventory the living aboard",
                        completionText: "The living are counted. The Fleet is aboard. The choice is yours.",
                        requiresPlayerChoice: true
                    ),
                    new QuestObjective(
                        id: "obj_fleet_offer_cluster_beds",
                        description: "Offer Cluster beds or leave them",
                        completionText: "The beds are offered. The Fleet is aboard. The choice is yours.",
                        completionText: "The beds are left. The Fleet is aboard. The choice is yours."
                    )
                },
                rewards: new List<QuestReward>
                {
                    new QuestReward(
                        type: QuestRewardType.EndingFlag,
                        id: "ending_flag_fleet_boarded"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Knowledge,
                        id: "knowledge_key_lore_fleet_boarded"
                    ),
                    new QuestReward(
                        type: QuestRewardType.Morale,
                        id: "morale_event_fleet_boarded"
                    )
                },
                failureConsequences: new List<string>
                {
                    "The Fleet's trust is broken",
                    "The hatch remains sealed",
                    "No ending flag"
                },
                hostileElements: new List<string>
                {
                    "The hatch wants a number",
                    "The Fleet does not forget",
                    "The choice is final"
                }
            );
        }
    }
}
