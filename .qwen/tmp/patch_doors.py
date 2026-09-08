# Round 4 — NEW door encounters (Year of Ash late-campaign visitors).
# Schema: encounterId, visitorName, visitorFaction, description, minDay, maxDay, threatLevel, choices[]
# Choice schema (all 10 keys): choiceId, text, requiredTrait, requiredItemId, requiredItemQuantity,
#   baseMoraleDelta, baseGuiltDelta, targetFaction, factionStandingDelta, outcomeDescription

NEW = [
{
  "encounterId": "door_encounter_garrison_cartographer",
  "visitorName": "Adjutor Vess (Central Garrison)",
  "visitorFaction": "faction_central_garrison",
  "description": "One man, no escort, a survey tripod slung like a rifle he is not allowed to carry. His papers say the Garrison is updating its civil maps: water points, ventilation intakes, doors. He wants ninety seconds at your hatch with a theodolite and a notebook. His boots are too clean for the road he claims to have walked, and he does not look at the camera when he speaks. He looks at the intake pipe.",
  "minDay": 190,
  "maxDay": 260,
  "threatLevel": 1,
  "choices": [
    {
      "choiceId": "choice_allow_the_survey",
      "text": "Ninety seconds. Let him measure what can be measured from outside.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_central_garrison",
      "factionStandingDelta": 10,
      "outcomeDescription": "He works fast and writes small. Before he leaves he says, 'Maps protect people,' in the tone of a man repeating something he was told to say. The copy that reaches the Garrison will show your intake, your door, and your depth. You have ninety seconds of company and a permanent address in somebody's filing cabinet."
    },
    {
      "choiceId": "choice_refuse_the_theodolite",
      "text": "The map ends at our door. Tell them the shelter declined.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_central_garrison",
      "factionStandingDelta": -10,
      "outcomeDescription": "He folds the tripod without arguing, which is worse than arguing. At the treeline he stops and writes one line in the notebook without looking at the page. You will never know what the line said. The bunker feels smaller that night, and better defended, and nobody can explain both feelings at once."
    },
    {
      "choiceId": "choice_feed_him_false_numbers",
      "text": "Give him ninety seconds — of the old intake, the collapsed one, two hundred meters east.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 0,
      "baseGuiltDelta": 5,
      "targetFaction": "faction_central_garrison",
      "factionStandingDelta": 5,
      "outcomeDescription": "You walk him to the collapsed east intake yourself and hold the tape while he measures a hole that serves nobody. He thanks you. The lie is now load-bearing: any Garrison surveyor who comes after him will carry the wrong map, and wrong maps get corrected by people standing at doors, asking questions."
    }
  ]
},
{
  "encounterId": "door_encounter_freeholder_brine_well_offer",
  "visitorName": "Driller Ossy Renn (Salt Freeholders)",
  "visitorFaction": "faction_salt_freeholders",
  "description": "She arrives with a rig that is mostly rope and stubbornness, and a contract wrapped in oilcloth. Her terms: the Freeholders drill you a second brine well on your own land, free. In exchange, the contract's last clause assigns her apprentice 'lodging and ration' at your shelter for one year. The apprentice is fourteen, does not speak, and watches the hatch like it might close on her.",
  "minDay": 210,
  "maxDay": 280,
  "threatLevel": 0,
  "choices": [
    {
      "choiceId": "choice_sign_the_well_contract",
      "text": "Sign. Water outlives every clause anyone remembers to enforce.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 5,
      "targetFaction": "faction_salt_freeholders",
      "factionStandingDelta": 15,
      "outcomeDescription": "The well takes eleven days and strikes brine at nine meters. The apprentice takes a bunk near the pump and, true to form, does not speak for a week. On the eighth day she teaches the cook's boy a card game. Ossy Renn watches from the hatch and writes something in her own oilcloth. The clause was never about labor. You suspect it was never really about the well, either."
    },
    {
      "choiceId": "choice_refuse_the_clause",
      "text": "We buy water, we don't board children. Decline the contract.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_salt_freeholders",
      "factionStandingDelta": -10,
      "outcomeDescription": "Ossy does not get angry. She rolls the contract slow, the way you'd put away a tool you'll need again. 'Water's the same either way,' she says. 'Only who drinks it changes.' The rig leaves at noon. Your single well keeps its schedule, and the schedule keeps its math, and the math has started to frighten the cook."
    }
  ]
},
{
  "encounterId": "door_encounter_guild_brake_rider_wounded",
  "visitorName": "Brake-rider Holl (Railway Guild)",
  "visitorFaction": "faction_railway_guild",
  "description": "He comes down the embankment half-carrying, half-dragging a boy in Guild colors whose shin bone has opinions of its own. Their brake-car threw a wheel three kilometers back. Holl is not asking for much: the bone set, the boy fevered down, a splint that will hold a handcar ride home. He is also, pointedly, not mentioning the sealed crate on the handcar, stamped with a Garrison eagle that has been filed down and poorly.",
  "minDay": 220,
  "maxDay": 300,
  "threatLevel": 2,
  "choices": [
    {
      "choiceId": "choice_treat_the_brakeman",
      "text": "Set the bone. Feed the fever. Say nothing about the crate.",
      "requiredTrait": "",
      "requiredItemId": "medkit",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_railway_guild",
      "factionStandingDelta": 20,
      "outcomeDescription": "The splint holds. The boy sleeps two days and wakes asking about the crate before he asks about his leg. When they leave, Holl sets a Guild chit on your rail spike — good for one passage, one crate, one favor, in that order of your choosing. Nobody from the Garrison ever comes asking about a filed-down eagle. This time."
    },
    {
      "choiceId": "choice_turn_them_to_the_road",
      "text": "The bunker is not a hospital. Point them down the line.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -10,
      "baseGuiltDelta": 10,
      "targetFaction": "faction_railway_guild",
      "factionStandingDelta": -15,
      "outcomeDescription": "Holl looks at your door for a long moment, then lifts the boy's arm over his shoulder and walks. The Guild keeps records the way the dead keep graves: permanently, and with interest. Three weeks later your name comes up at a water point and the room goes quiet in the specific way that means it was not praised."
    },
    {
      "choiceId": "choice_report_the_crate",
      "text": "Treat the boy — and send word of the crate to the Garrison relay.",
      "requiredTrait": "",
      "requiredItemId": "medkit",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 15,
      "targetFaction": "faction_railway_guild",
      "factionStandingDelta": -25,
      "outcomeDescription": "The Garrison patrol meets the handcar at the crossing. What is in the crate never becomes public, which is its own answer. Holl is not on the line anymore; the boy is, on crutches, and he looks at your hatch with the flat patience of someone building a ledger of his own. Your standing with the Garrison improves by exactly the amount your sleep deteriorates."
    }
  ]
},
{
  "encounterId": "door_encounter_scavenger_child_courier",
  "visitorName": "A child, unaccompanied (Scavengers)",
  "visitorFaction": "faction_scavengers",
  "description": "Small, filthy, composed. The child has walked four kilometers alone to deliver a dead man's map: Picker Tallow died at the rail cut last week, and his last haul is marked on the oiled paper in the child's fist. Tallow once bunked here, one winter, before the roads took him back. The child wants the map's value paid in one thing only — a name cut proper, on a stone, where the bunker's dead are kept.",
  "minDay": 185,
  "maxDay": 250,
  "threatLevel": 0,
  "choices": [
    {
      "choiceId": "choice_honor_tallow",
      "text": "Cut the name. Feed the child. Keep the map as payment.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_scavengers",
      "factionStandingDelta": 10,
      "outcomeDescription": "The stone takes TALLOW, PICKER. WINTER GUEST. The child watches every stroke, then eats two bowls and falls asleep upright against the pump housing. The map's mark turns out to be a flooded pharmacy basement — picked over by now, but the shelves below the waterline still hold. Word reaches the rail cut that your door pays in names. Doors that pay in names get told things."
    },
    {
      "choiceId": "choice_send_the_child_away",
      "text": "The dead don't need stones. Send the child back with a tin and no promises.",
      "requiredTrait": "",
      "requiredItemId": "canned_soup",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": -10,
      "baseGuiltDelta": 15,
      "targetFaction": "faction_scavengers",
      "factionStandingDelta": -5,
      "outcomeDescription": "The child takes the tin without arguing — that is the part that stays with the watch. The map goes back into the small fist and out to the cut, where Tallow will stay a coordinate instead of a name. For a week, the corridor near the hatch is quieter than usual, and not because anyone decided it should be."
    }
  ]
},
{
  "encounterId": "door_encounter_ash_sign_door_witness",
  "visitorName": "The Door Witness (Cult of the Ash Sign)",
  "visitorFaction": "faction_ash_sign",
  "description": "Grey robe, chalk bag, no weapon anyone can find. The Witness counts doors. That is the whole office: after every storm they walk the sector and tally which doors still answer. Yours is number eleven today — they say it out loud, to nobody, and chalk a small sign at the corner of your hatch frame. They claim the storm spoke, and that what it said is being written down, door by door.",
  "minDay": 250,
  "maxDay": 330,
  "threatLevel": 1,
  "choices": [
    {
      "choiceId": "choice_let_the_door_be_counted",
      "text": "Let the chalk stand. Eleven is a number; numbers are not threats.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_ash_sign",
      "factionStandingDelta": 10,
      "outcomeDescription": "The Witness nods once, like a clerk paid in full, and walks on. The chalk sign weathers but never quite washes off. Some nights the younger survivors touch it going out, the way people touch wood. You did not approve a ritual. The ritual happened anyway, and morale is up four points that nobody can source."
    },
    {
      "choiceId": "choice_scrub_the_sign",
      "text": "Scrub the chalk off the hatch. Our door keeps its own accounts.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_ash_sign",
      "factionStandingDelta": -10,
      "outcomeDescription": "Wire brush, ten minutes. The Witness watches without expression, writes something in a book that is mostly water damage, and leaves. Next storm season, a door two sectors over answers when yours would have wanted a witness. The Ash Sign does not punish. The Ash Sign subtracts."
    },
    {
      "choiceId": "choice_ask_to_be_recorded_empty",
      "text": "Ask them to write this door down as empty. A ghost is a kind of armor.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 0,
      "baseGuiltDelta": 10,
      "targetFaction": "faction_ash_sign",
      "factionStandingDelta": 5,
      "outcomeDescription": "The Witness considers this the way a ledger-keeper considers a forged line: not morally, professionally. Then they write it. Somewhere in the Sign's tallies, your door is now a door nobody answered. Raiders who buy that book will skip you. So will caravans. So will, one day, rescuers — and the entry will outlive everyone who knows it is a lie."
    }
  ]
},
{
  "encounterId": "door_encounter_foundry_bell_caster",
  "visitorName": "Caster Mabe Ilk (Ordnance Foundry)",
  "visitorFaction": "faction_ordnance_foundry",
  "description": "Her cart is a crucible on wheels and her pitch is nearly sane: the Foundry wants to cast a bell from melted shell casings, one per shelter that trades, and hang them along the road. A rung bell means the door is open for business. A silent bell means the shelter is gone. She is asking for your casings — twenty of them — and offering, in return, to be a neighbor you can hear coming.",
  "minDay": 230,
  "maxDay": 310,
  "threatLevel": 0,
  "choices": [
    {
      "choiceId": "choice_give_casings_for_the_bell",
      "text": "Twenty casings for one bell. Melt down the argument.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_ordnance_foundry",
      "factionStandingDelta": 15,
      "outcomeDescription": "The bell is hung in spring on a frame your own survivors help raise. It sounds wrong at first — casings make a bright, thin note, like a coin dropped in a bucket — and then it sounds like the road. The first time it rings for a trade party, three people in your bunker stand still in the corridor and just listen to it."
    },
    {
      "choiceId": "choice_refuse_the_meltdown",
      "text": "Casings are currency. The Foundry can cast its own neighbors.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_ordnance_foundry",
      "factionStandingDelta": -10,
      "outcomeDescription": "Mabe shrugs — a smith's shrug, all shoulders, no offense — and wheels the crucible on. The next shelter down the road gives casings and gets a bell. Trade parties start timing their route by it. Your door stays quiet in a landscape that is learning to ring, and quiet, lately, has started to mean gone."
    }
  ]
},
{
  "encounterId": "door_encounter_hydro_meter_falsifier",
  "visitorName": "Sub-Auditor Pell (Hydro Barons)",
  "visitorFaction": "faction_hydro_barons",
  "description": "He reads your intake meter for eleven minutes, writes nothing, and then makes his actual offer in a voice trained for boiler rooms: he can register your draw one tier lower than it is. The Barons' tithe drops. His clipboard gains a smudge that will never be audited. In exchange, the Hydro Barons will, someday, ask you for something, and the something will not be water.",
  "minDay": 290,
  "maxDay": 355,
  "threatLevel": 2,
  "choices": [
    {
      "choiceId": "choice_accept_the_false_ledger",
      "text": "Let him smudge the ledger. Debts you can't see are debts you can survive.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 15,
      "targetFaction": "faction_hydro_barons",
      "factionStandingDelta": 10,
      "outcomeDescription": "The tithe notice arrives next season eleven liters lighter. Pell never returns. The favor, when it comes, will come as a man at your door who already knows your meter is a lie, and that man will not be negotiating — he will be collecting. You have bought water with a door you cannot close."
    },
    {
      "choiceId": "choice_refuse_and_report",
      "text": "Refuse — and send the Barons' own audit office the offer, unsigned.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_hydro_barons",
      "factionStandingDelta": -5,
      "outcomeDescription": "Pell's route reassigns within the month; the Barons' audit office does not thank you, but a clean-meter shelter is a shelter their honest clerks defend in rooms you will never see. Your tithe stays full and stays honest. Honest is expensive. Expensive is at least a number."
    },
    {
      "choiceId": "choice_bribe_with_water_instead",
      "text": "No smudges. Five liters of clean, off the books, for the walk home.",
      "requiredTrait": "",
      "requiredItemId": "clean_water",
      "requiredItemQuantity": 2,
      "baseMoraleDelta": 0,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_hydro_barons",
      "factionStandingDelta": 5,
      "outcomeDescription": "Pell looks at the canteen like a man shown a language he studied once. He takes it, drinks half on your steps — deliberately, so both of you know there is no evidence — and registers your meter exactly as read. No favor is owed in either direction. This is, somehow, the most expensive five liters you have ever spent, and the only transaction this season that lets everyone sleep."
    }
  ]
},
{
  "encounterId": "door_encounter_unaligned_well_diviner",
  "visitorName": "Old Ferrow (unaligned)",
  "visitorFaction": "faction_unaligned",
  "description": "A fork of bent rebar, two wire handles, and a claim: there is water under your floor. Not the well you drilled — another, deeper, running where the old municipal line used to run. Ferrow has divined four wells in three settlements and been wrong exactly once, a record repeated at every door on request. The price for a night's bunk and a hot tin is that you let him walk your corridors with the fork and mark where it pulls.",
  "minDay": 200,
  "maxDay": 270,
  "threatLevel": 0,
  "choices": [
    {
      "choiceId": "choice_bunk_the_diviner",
      "text": "One bunk, one tin, one night. Let the fork walk.",
      "requiredTrait": "",
      "requiredItemId": "canned_soup",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 0,
      "targetFaction": "",
      "factionStandingDelta": 0,
      "outcomeDescription": "The fork pulls hard under the pump room — hard enough that Ferrow marks the concrete with a nail and sits down on the mark like a man who has arrived. Whether there is water down there will cost a drill rig to find out. What the night costs is less measurable: three survivors slept better hearing an old man snore in the spare bunk, and one of them says so out loud, which nobody expected."
    },
    {
      "choiceId": "choice_turn_the_diviner_out",
      "text": "Superstition doesn't dig wells. The road, Old Ferrow.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 5,
      "targetFaction": "",
      "factionStandingDelta": 0,
      "outcomeDescription": "He goes without a word, fork shouldered like a yoke. In the morning there is a small chalk mark on your hatch frame anyway — not the Ash Sign's, just an old man's habit, a line with a dot under it. The cook says it means water. The cook also says she doesn't believe it. She keeps looking at it anyway."
    }
  ]
},
{
  "encounterId": "door_encounter_rebuilder_seed_tin",
  "visitorName": "Botanist Sarn Vey (Rebuilders)",
  "visitorFaction": "faction_rebuilders",
  "description": "The tin is the size of a ration box, unmarked, dented at one corner, and it came out of a vault that the Rebuilders' inventory says does not exist. Sarn Vey does not know what is in it. That is the entire pitch, delivered with a botanist's terrible honesty: pre-war stock, sealed, unlabeled. It might be wheat. It might be a blight that your greenhouse will spend two years dying of. She will not open it anywhere but here, and she will not pretend the odds are better than they are.",
  "minDay": 240,
  "maxDay": 320,
  "threatLevel": 1,
  "choices": [
    {
      "choiceId": "choice_plant_the_unknown",
      "text": "Open it in the quarantine bed. Plant what comes.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_rebuilders",
      "factionStandingDelta": 10,
      "outcomeDescription": "The quarantine bed gets the tin, the botanist gets a cot, and for nine days the greenhouse holds its breath. What comes up is a squat, angry-looking legume that fixes nitrogen and tastes, per the cook, like wet pencil. It is not wheat. It is a crop that grows in ash-soil without argument, and the seed you save from it will outlive everyone who was afraid of the tin."
    },
    {
      "choiceId": "choice_burn_the_tin",
      "text": "Some doors stay shut. Burn the tin in the barrel, unopened.",
      "requiredTrait": "",
      "requiredItemId": "fuel_cell",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_rebuilders",
      "factionStandingDelta": -5,
      "outcomeDescription": "It takes forty minutes and smells like a battery fire. Sarn Vey watches the whole thing without arguing, then writes the shelter's name in her field book under a heading you don't get to see. The greenhouse is exactly as safe as it was yesterday. The word the survivors use for that feeling, tonight, is not 'safe.'"
    },
    {
      "choiceId": "choice_hand_the_tin_to_the_rebuilders",
      "text": "Not our soil, not our odds. Escort the tin back to Rebuilder ground.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 0,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_rebuilders",
      "factionStandingDelta": 15,
      "outcomeDescription": "You walk Sarn Vey to the treeline with the tin unopened between you like a child neither of you is adopting. The Rebuilders log the escort, and logs count. Eighteen months later a Rebuilder courier leaves a packet at your hatch: labeled, inventoried, nitrogen-fixing legume stock, with a note that says only that the vault was not a myth."
    }
  ]
},
{
  "encounterId": "door_encounter_supply_corps_filter_requisition",
  "visitorName": "Quartermaster Adale Grist (Supply Corps)",
  "visitorFaction": "faction_supply_corps",
  "description": "The requisition is stamped, numbered, and correct in every way that makes it monstrous: the Corps' field hospital two valleys over is running its ventilator room on filters past service, and the nearest serviceable spares are yours. Grist offers medical stores against the exchange — real ones, not promises — and a signed line in the Corps' book, which is the only currency that has never devalued. Your own filtration is at eighty percent and falling on its own schedule.",
  "minDay": 260,
  "maxDay": 340,
  "threatLevel": 2,
  "choices": [
    {
      "choiceId": "choice_hand_over_the_filters",
      "text": "Give the filters. The hospital's math is worse than ours.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -10,
      "baseGuiltDelta": -5,
      "targetFaction": "faction_supply_corps",
      "factionStandingDelta": 20,
      "outcomeDescription": "The medkit stores come off the truck first — Grist insists on it, so nobody can say the Corps took and left. Your intake fans pull harder that night against filters running at the edge of service, and the sound is a small rasp everybody hears and nobody mentions. Two valleys over, a ventilator room runs clean. You will never meet the lungs it runs for. That is the whole shape of the trade."
    },
    {
      "choiceId": "choice_refuse_the_requisition",
      "text": "Our air is not spare. Deny the requisition, stamped or not.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": 5,
      "baseGuiltDelta": 10,
      "targetFaction": "faction_supply_corps",
      "factionStandingDelta": -20,
      "outcomeDescription": "Grist files the denial on your hatch step, in writing, per regulation — a small mercy or a small threat, the form does not distinguish. Your air stays yours. The Corps' book closes around your shelter's name with the quiet finality of a ledger that will be consulted, someday, by someone deciding whether your door is worth walking to."
    },
    {
      "choiceId": "choice_split_the_difference",
      "text": "Half the spares now, for half the medical stores and the whole signed line.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_supply_corps",
      "factionStandingDelta": 10,
      "outcomeDescription": "Grist does the arithmetic on the truck's tailgate, twice, and nods once. Half a hospital breathes easier; your own scrubbers lose a month of margin. The signed line in the Corps' book says your shelter answered a requisition in part and in good faith, which, in the Corps' grammar, is very nearly love."
    }
  ]
},
{
  "encounterId": "door_encounter_tempest_barograph_seller",
  "visitorName": "Kessa Vane, storm-chaser (The Tempest)",
  "visitorFaction": "faction_the_tempest",
  "description": "She carries a barograph drum under one arm like a stolen hat, and two years of ink traces inside it. Her claim, delivered flat and fast: the storms are migrating. Same bearings, same intervals, walking a route like a patrol — and your sector sits on the line. The drum record proves it or doesn't. She wants fuel cells for the chase rig, and she is willing to be paid in warnings.",
  "minDay": 300,
  "maxDay": 360,
  "threatLevel": 1,
  "choices": [
    {
      "choiceId": "choice_buy_the_storm_record",
      "text": "Two fuel cells for the drum and a copy of the route.",
      "requiredTrait": "",
      "requiredItemId": "fuel_cell",
      "requiredItemQuantity": 2,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_the_tempest",
      "factionStandingDelta": 10,
      "outcomeDescription": "The copied trace goes up beside the hatch chart, and for the first time the sky has a schedule. Your watch starts calling storms by number instead of by dread. The first one she predicts lands within four hours of her window; the bunker seals with eleven minutes to spare and nobody panics, which is the actual purchase price of the fuel."
    },
    {
      "choiceId": "choice_send_the_chaser_on",
      "text": "Storms don't commute. Keep the fuel.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_the_tempest",
      "factionStandingDelta": -5,
      "outcomeDescription": "Kessa Vane taps the drum once — not offended, just filing it — and walks back into the grey. Three weeks later a storm turns inside a window she could have sold you, and the watch rides it out on old reflex and luck. Afterwards nobody says her name in the corridor. Not saying it is its own kind of saying."
    }
  ]
},
{
  "encounterId": "door_encounter_penal_battalion_escapee",
  "visitorName": "A deserter in stolen boots (Penal Battalion)",
  "visitorFaction": "faction_penal_battalion",
  "description": "He arrives after dark, which is either cunning or all the luck he had left. Garrison-issue boots two sizes wrong, wrist branded with the Battalion's chain-and-number, eyes that keep measuring the distance back to the treeline. He is not asking to stay. He is asking, very carefully, what your door does with people like him — and watching your hands while you answer.",
  "minDay": 310,
  "maxDay": 360,
  "threatLevel": 3,
  "choices": [
    {
      "choiceId": "choice_shelter_the_deserter",
      "text": "Bunk him. Brand goes under the sleeve; the sleeve stays buttoned.",
      "requiredTrait": "",
      "requiredItemId": "dried_rations",
      "requiredItemQuantity": 2,
      "baseMoraleDelta": 10,
      "baseGuiltDelta": 0,
      "targetFaction": "faction_penal_battalion",
      "factionStandingDelta": -10,
      "outcomeDescription": "He works the pump shift for a month before anyone learns his name, and two months before anyone learns the brand. What the bunker learns first is that he checks the hatch seals every night, twice, without being asked. If the Battalion ever comes reading brands, your door becomes a door that hid one — and everyone in it will already have decided, privately, what they'd say."
    },
    {
      "choiceId": "choice_turn_him_over",
      "text": "Send a runner to the Battalion relay. A deserter is a currency.",
      "requiredTrait": "",
      "requiredItemId": "",
      "requiredItemQuantity": 0,
      "baseMoraleDelta": -15,
      "baseGuiltDelta": 20,
      "targetFaction": "faction_penal_battalion",
      "factionStandingDelta": 10,
      "outcomeDescription": "The Battalion patrol collects him at first light and does not thank you; chains do not thank doors. Your standing with them improves by the exact measure that the mess hall's noise level drops. He went out through the hatch the same way everyone goes out, and for a week, everybody uses the word 'properly' a little too loudly."
    },
    {
      "choiceId": "choice_night_send_with_rations",
      "text": "Not our war, not our bunk. Rations for the road, out the back, tonight.",
      "requiredTrait": "",
      "requiredItemId": "dried_rations",
      "requiredItemQuantity": 1,
      "baseMoraleDelta": -5,
      "baseGuiltDelta": 5,
      "targetFaction": "faction_penal_battalion",
      "factionStandingDelta": 0,
      "outcomeDescription": "He takes the tin and the bearings north without arguing, and at the treeline he does something deserters are not supposed to have time for: he turns and salutes, wrong-handed, the brand showing white in the dark. Nobody in the bunker claims to have seen it. The watch log that night has one entry in three different handwritings."
    }
  ]
}
]
