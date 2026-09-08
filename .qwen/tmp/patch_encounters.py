# Round 4 — NEW character-driven expedition set-pieces for narrative_encounters.json.
#
# WHY THIS FILE: the core catalog held only 3 set-pieces (dead letter office, weather station,
# the pianist) while micro_locations.json carries 28 quick site vignettes. The core slot is for
# larger character scenes; it was the thinnest prose-bearing player-facing catalog in the game.
#
# SAFETY (verified against EncounterCatalog.cs + MicroLocationCatalogLoaderTests):
#   * EncounterCatalogBuilder.Add THROWS on duplicate id across narrative_encounters.json and
#     micro_locations.json -> ids checked against all 31 existing.
#   * minDangerLevel is a FLOOR (dangerLevel < min => weight 0), so all new entries use 0.0 to
#     stay reachable; requiredLocationId "" => location-agnostic; forceOnArrival false so new
#     entries never fire unconditionally (2 of the existing 3 do).
#   * Choices only carry moraleDelta/guiltDelta — no item/flag references, so nothing new needs
#     registering in the hazard registry or item catalogs.
#   * stealth/speed multipliers authored per scene: a person waiting quietly is found by careful
#     travel and missed by rushing (stealth >1, speed <1), inverting the old site defaults.
#
# CROSS-CONTENT TIES: census carrier <-> quest_the_census_taker; ninth-night listener <->
# quest_numbers_in_the_dark / quest_the_voice_answers + radio_broadcast_numbers_*; the tower
# classroom <-> 91.3 radio_broadcast_classroom_*; the glass blower <-> 104.2 crater sermons;
# the surveyor <-> radio_broadcast_classroom_valley_geography; the deserter's grave <->
# door_encounter_penal_battalion_escapee.

NEW_ENCOUNTERS = [
{
  "id": "enc_census_carrier_on_the_road",
  "title": "The Last Door on the Route",
  "description": "A census carrier sits against a culvert with his back straight and his route book open on his knees, dead long enough that the weather has started on him. The book is complete: thirteen doors, each with a count, a date, and a tick. The thirteenth line has a tick and no count. The thirteenth line is your door.",
  "category": "Social",
  "baseWeight": 1.6,
  "stealthWeightMultiplier": 1.2,
  "speedWeightMultiplier": 0.6,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "census_carrier_bury_and_take",
      "text": "Bury him under the culvert with the book facing up. Take the route home.",
      "moraleDelta": 6,
      "guiltDelta": 0
    },
    {
      "choiceId": "census_carrier_take_the_book",
      "text": "Take the book. The road does not need a grave and the shelter needs the numbers.",
      "moraleDelta": 2,
      "guiltDelta": 8
    },
    {
      "choiceId": "census_carrier_finish_the_route",
      "text": "Walk the thirteenth door yourself. Count what he came to count, and write it in his hand.",
      "moraleDelta": 9,
      "guiltDelta": 0
    },
    {
      "choiceId": "census_carrier_leave_him",
      "text": "Leave him and the book. Somebody else's route, somebody else's weather.",
      "moraleDelta": -5,
      "guiltDelta": 3
    }
  ]
},
{
  "id": "enc_ninth_night_listener",
  "title": "The Other Listener",
  "description": "A relay hut with a lamp in it and a wall papered in transcripts: groups of five, in columns, dated by hand. The woman who keeps it has been listening two years longer than you and has a theory she will not say aloud until she knows you hear it too. Her sheets agree with yours on the interval. They disagree on what the interval is for.",
  "category": "Social",
  "baseWeight": 1.4,
  "stealthWeightMultiplier": 1.3,
  "speedWeightMultiplier": 0.5,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "ninth_night_compare_sheets",
      "text": "Put your transcripts beside hers and find the night they differ.",
      "moraleDelta": 7,
      "guiltDelta": 0
    },
    {
      "choiceId": "ninth_night_take_her_wall",
      "text": "Take the wall. Two years of groups is worth more in your bunker than on hers.",
      "moraleDelta": 3,
      "guiltDelta": 9
    },
    {
      "choiceId": "ninth_night_tell_her_to_stop",
      "text": "Tell her the schedule is a machine and she is losing winters to it.",
      "moraleDelta": -4,
      "guiltDelta": 4
    },
    {
      "choiceId": "ninth_night_stay_and_listen",
      "text": "Stay the night. Two people hearing the same thing is the only instrument either of you has.",
      "moraleDelta": 10,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_the_tower_classroom",
  "title": "The Tower With a Lamp On",
  "description": "Ninety-one point three comes out of this mast: a schoolroom in a relay gallery, two chairs, a slate, and a man splicing antenna wire with his teeth. They broadcast to nobody they can see and have never once confirmed a listener. The teacher keeps a register. The register has four names in it and none of them are present.",
  "category": "Social",
  "baseWeight": 1.5,
  "stealthWeightMultiplier": 1.3,
  "speedWeightMultiplier": 0.5,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "tower_classroom_give_batteries",
      "text": "Give them batteries. A lesson that stops is the only kind of silence that carries.",
      "moraleDelta": 8,
      "guiltDelta": 0
    },
    {
      "choiceId": "tower_classroom_ask_for_a_lesson",
      "text": "Sit down and be counted. Ask to be taught something you do not already know.",
      "moraleDelta": 9,
      "guiltDelta": 0
    },
    {
      "choiceId": "tower_classroom_strip_the_transmitter",
      "text": "Strip the transmitter for parts while the lineman is down the mast.",
      "moraleDelta": -6,
      "guiltDelta": 12
    },
    {
      "choiceId": "tower_classroom_sign_the_register",
      "text": "Give them your shelter's name for the register, so the four is a five.",
      "moraleDelta": 7,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_glass_blower_of_the_rim",
  "title": "The Man Who Works the Crater Glass",
  "description": "He melts the fused valley floor in a brick furnace and draws it out into lenses, beads, and one long rod he will not explain. The rim has asked for him three times and he has refused three times, which is why the furnace is banked and the door is barred. He sells. He does not go.",
  "category": "Discovery",
  "baseWeight": 1.3,
  "stealthWeightMultiplier": 1.2,
  "speedWeightMultiplier": 0.7,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "glass_blower_buy_a_lens",
      "text": "Buy a lens and ask nothing about the rod.",
      "moraleDelta": 5,
      "guiltDelta": 0
    },
    {
      "choiceId": "glass_blower_warn_him",
      "text": "Tell him the rim has stopped asking and started counting the refusals.",
      "moraleDelta": 6,
      "guiltDelta": 0
    },
    {
      "choiceId": "glass_blower_take_his_fuel",
      "text": "Take the furnace fuel. Glass is a luxury and your winter is not.",
      "moraleDelta": -3,
      "guiltDelta": 10
    },
    {
      "choiceId": "glass_blower_offer_the_bench",
      "text": "Offer him a bench and a hatch that closes, and mean the offer.",
      "moraleDelta": 8,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_the_surveyor_still_working",
  "title": "The Surveyor at the Boundary",
  "description": "An old man is running a line across the ash with a theodolite and a chain, marking boundaries for parcels whose owners are eleven years dead. He is not mad and he will tell you so, then explain that the plat has to be accurate for whoever comes back, and that the coming back is not his department.",
  "category": "Social",
  "baseWeight": 1.4,
  "stealthWeightMultiplier": 1.3,
  "speedWeightMultiplier": 0.5,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "surveyor_walk_the_line",
      "text": "Hold the chain for a day. Walk the line to the stone and set the stone.",
      "moraleDelta": 8,
      "guiltDelta": 0
    },
    {
      "choiceId": "surveyor_ask_him_to_teach",
      "text": "Ask him to teach you the boundaries. Somebody should be able to give directions here.",
      "moraleDelta": 7,
      "guiltDelta": 0
    },
    {
      "choiceId": "surveyor_take_the_theodolite",
      "text": "Take the theodolite. Your bunker needs the glass more than the dead need their corners.",
      "moraleDelta": 2,
      "guiltDelta": 11
    },
    {
      "choiceId": "surveyor_tell_him_nobody_comes_back",
      "text": "Tell him the truth: nobody is coming back to this plat.",
      "moraleDelta": -6,
      "guiltDelta": 5
    }
  ]
},
{
  "id": "enc_the_orchard_with_stakes",
  "title": "The Orchard That Is Marked",
  "description": "Fourteen trees still fruit here, which should be impossible and is only expensive. Every outer row has a stake at its foot, cut fresh, and the keeper sits between the rows with a sprayer and no interest in being argued with. The inner rows have no stakes. He says the outer rows are for people who do not ask.",
  "category": "Hazard",
  "baseWeight": 1.2,
  "stealthWeightMultiplier": 1.4,
  "speedWeightMultiplier": 0.6,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "orchard_trade_honestly",
      "text": "Ask, and trade for what he is willing to give.",
      "moraleDelta": 6,
      "guiltDelta": 0
    },
    {
      "choiceId": "orchard_take_the_inner_rows",
      "text": "Take the unmarked inner rows with his hand on the sprayer and your hand off the fruit.",
      "moraleDelta": -2,
      "guiltDelta": 6
    },
    {
      "choiceId": "orchard_pick_the_staked_rows",
      "text": "Pick the staked outer rows at night. The stakes mean something and you decide it means nothing.",
      "moraleDelta": -8,
      "guiltDelta": 12
    },
    {
      "choiceId": "orchard_keep_the_secret",
      "text": "Leave, and tell nobody at the water point that the orchard is here.",
      "moraleDelta": 5,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_ferryman_toll_of_stories",
  "title": "The Toll Is One True Thing",
  "description": "The crossing is a raft, a cable, and an old man who does not want your goods. His toll is one true thing, told once, and he will not take a second telling of the same one. He has heard nine hundred of them and can tell you which settlements are lying about their dead, because the dead are what people tell him about.",
  "category": "Social",
  "baseWeight": 1.5,
  "stealthWeightMultiplier": 1.2,
  "speedWeightMultiplier": 0.6,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "ferryman_pay_a_true_story",
      "text": "Pay the toll. Tell him the one you have not told at home.",
      "moraleDelta": 9,
      "guiltDelta": 0
    },
    {
      "choiceId": "ferryman_pay_a_lie",
      "text": "Pay him a lie and see whether the raft moves anyway.",
      "moraleDelta": -5,
      "guiltDelta": 7
    },
    {
      "choiceId": "ferryman_walk_the_long_way",
      "text": "Refuse and walk the long crossing. Your story is not currency.",
      "moraleDelta": -3,
      "guiltDelta": 0
    },
    {
      "choiceId": "ferryman_carry_one_for_him",
      "text": "Take a story from him to somebody downriver, and promise to bring the answer back.",
      "moraleDelta": 7,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_dog_at_the_sealed_door",
  "title": "The Door the Dog Is Keeping",
  "description": "A blast door in a hillside, sealed from the outside, with a dog lying at it. The dog is not starving, which means somebody feeds it, and nobody is coming out, which means the feeding is one-directional. It does not bark. It watches the road the way something watches a kettle.",
  "category": "Discovery",
  "baseWeight": 1.3,
  "stealthWeightMultiplier": 1.3,
  "speedWeightMultiplier": 0.6,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "sealed_door_feed_and_leave",
      "text": "Feed the dog and go. The keeping is not yours to take over.",
      "moraleDelta": 5,
      "guiltDelta": 0
    },
    {
      "choiceId": "sealed_door_take_the_dog",
      "text": "Call it off the door and take it home. It comes, which is the worst part.",
      "moraleDelta": 7,
      "guiltDelta": 4
    },
    {
      "choiceId": "sealed_door_open_the_door",
      "text": "Work the seals. Whatever the dog was keeping, you would rather know than wonder.",
      "moraleDelta": -4,
      "guiltDelta": 8
    },
    {
      "choiceId": "sealed_door_mark_it_on_the_map",
      "text": "Mark the door and the dog on your route map and change nothing.",
      "moraleDelta": 3,
      "guiltDelta": 0
    }
  ]
},
{
  "id": "enc_quarry_witness",
  "title": "The Witness From the Quarry",
  "description": "She was in the quarry the night the water went and she will tell you what she saw for the price of a bunk and a name on a roster. Her account is internally consistent and contradicts two other accounts your shelter has already heard. She is not asking you to believe her. She is asking you to stop the other two from being the only version.",
  "category": "Social",
  "baseWeight": 1.4,
  "stealthWeightMultiplier": 1.2,
  "speedWeightMultiplier": 0.6,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "quarry_witness_bring_her_in",
      "text": "Bring her in. Let the shelter hold three versions of one night instead of two.",
      "moraleDelta": 6,
      "guiltDelta": 0
    },
    {
      "choiceId": "quarry_witness_hear_it_roadside",
      "text": "Hear it on the roadside and walk on with the account and without the witness.",
      "moraleDelta": 0,
      "guiltDelta": 9
    },
    {
      "choiceId": "quarry_witness_ask_for_proof",
      "text": "Ask for proof first. She has it, and it is worse than the story.",
      "moraleDelta": -3,
      "guiltDelta": 2
    },
    {
      "choiceId": "quarry_witness_refuse",
      "text": "Refuse. Your roster is full and the quarry is not your night.",
      "moraleDelta": -5,
      "guiltDelta": 3
    }
  ]
},
{
  "id": "enc_grave_with_the_wrong_name",
  "title": "The Grave With the Wrong Name",
  "description": "The frost has lifted one corner of this grave out of the ground and what it lifted is a pair of boots: penal battalion issue, chain-and-number on the sole plate. The stone above them carries a garrison name, cut carefully, by somebody who took the trouble. Whoever buried him did not want him found as what he was.",
  "category": "Discovery",
  "baseWeight": 1.3,
  "stealthWeightMultiplier": 1.2,
  "speedWeightMultiplier": 0.7,
  "minDangerLevel": 0.0,
  "requiredLocationId": "",
  "forceOnArrival": False,
  "choices": [
    {
      "choiceId": "wrong_name_rebury_and_leave",
      "text": "Push the corner back in and leave the name exactly as it was cut.",
      "moraleDelta": 5,
      "guiltDelta": 0
    },
    {
      "choiceId": "wrong_name_correct_the_stone",
      "text": "Cut the truth under the garrison name, small, where the frost will find it.",
      "moraleDelta": 8,
      "guiltDelta": 0
    },
    {
      "choiceId": "wrong_name_take_the_boots",
      "text": "Take the boots. Sole plates are tradeable and the dead do not walk.",
      "moraleDelta": -4,
      "guiltDelta": 11
    },
    {
      "choiceId": "wrong_name_report_the_grave",
      "text": "Report the grave to the battalion relay and collect whatever a report is worth.",
      "moraleDelta": -7,
      "guiltDelta": 10
    }
  ]
}
]
