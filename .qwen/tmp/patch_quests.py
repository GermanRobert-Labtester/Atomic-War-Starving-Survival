# Round 4 — NEW expansion quests.
# 05: eight quests incl. a three-link archive chain (quest-id prerequisites; verified pattern).
# 06: six quests incl. the two-link Nine Rails spur chain.
# Effect types restricted to the observed vocabulary: morale, resource, survivor_health,
#   survivor_count, faction_relation, labor_efficiency, medical_efficiency, bunker_defense_rating,
#   power, air_quality, bunker_capacity.

NEW_05 = [
{
  "id": "quest_the_drowned_archive",
  "title": "The Drowned Archive",
  "description": "A flooded records office two kilometers out holds the valley's civil registry: births, deeds, water rights, names. The paper is dying faster than the water is falling.",
  "type": "exploration",
  "minDay": 40,
  "maxDay": 180,
  "factionTag": "neutral",
  "synopsis": "The pre-war records office for the whole valley stands knee-deep in grey water, and every week the bottom shelves lose another year of the dead. A salvage team can go in light and fast for whatever is readable, or heavy and slow with shoring and pumps for the whole archive — or the shelter can decide that paper does not feed anyone and let the valley forget on schedule.",
  "prerequisites": [],
  "choices": [
    {
      "id": "archive_names_first",
      "text": "Send the light team. Names before infrastructure — the registry of the dead goes first.",
      "effects": [
        { "type": "morale", "value": 15 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "They come back with nine waterproof bags of birth and death records and one of water deeds. That night the cook finds her mother's marriage entry, still legible, and reads it aloud to whoever is in the mess hall. Nobody suggests stopping. The deeds could have bought a pump; the pump would not have known her mother's name."
    },
    {
      "id": "archive_blueprints_first",
      "text": "Infrastructure first. Municipal blueprints tell us where every pipe and cache in the valley runs.",
      "effects": [
        { "type": "bunker_defense_rating", "value": 5 },
        { "type": "resource", "value": 20 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "The blueprints are worth exactly what you hoped: a sealed maintenance cache nobody remembered, and the true map of the valley's water mains. The registry stays on the bottom shelf. Six months from now, when someone asks what became of the town's names, the honest answer will be that the shelter traded them for pipe."
    },
    {
      "id": "archive_full_slow_salvage",
      "text": "Shore it, pump it, take it all. Slow, expensive, complete.",
      "effects": [
        { "type": "resource", "value": -15 },
        { "type": "morale", "value": 10 },
        { "type": "survivor_health", "value": -5 }
      ],
      "consequences": "Three weeks of pump fuel and two cases of trench foot buy the whole archive: names, deeds, blueprints, and a box of school photographs nobody requisitioned. The shelter becomes, quietly, the valley's memory — a position that pays nothing and that people will walk four kilometers to consult."
    },
    {
      "id": "archive_leave_it_drown",
      "text": "Paper doesn't feed anyone. Let the water finish its filing.",
      "effects": [
        { "type": "morale", "value": -10 }
      ],
      "consequences": "No team goes out, no fuel is spent, and the registry completes its slow drowning on the valley's own schedule. The decision is defensible in every ledger. The survivors who grew up here know what was on those shelves, and they know the shelter chose the pump fuel over their grandparents' names."
    }
  ]
},
{
  "id": "quest_names_on_the_door",
  "title": "Names on the Door",
  "description": "Refugees from the north have heard the shelter salvaged the valley's records. They are at the treeline with a list of relatives, asking what the paper says.",
  "type": "community",
  "minDay": 60,
  "maxDay": 220,
  "factionTag": "neutral",
  "synopsis": "Eleven strangers, one folded list of names, and a question the archive can actually answer: who in their families was registered alive in the valley's last year of records. Reading names aloud from a doorway is free and changes everything; keeping the registry's contents to the shelter turns a salvage into a hoard.",
  "prerequisites": ["quest_the_drowned_archive"],
  "choices": [
    {
      "id": "names_read_aloud",
      "text": "Bring the ledger to the door. Read every name they ask for, whatever it says.",
      "effects": [
        { "type": "morale", "value": 10 },
        { "type": "survivor_count", "value": 2 }
      ],
      "consequences": "The reading takes two hours. Four names come back alive — registered at settlements north of the rail cut — and two women sit down in the mud when they hear it. Seven names come back dead, and the list-holder nods at each one like a clerk, because grief with paperwork is grief somebody witnessed. Two of the eleven ask to stay. The shelter's door acquires a reputation it cannot afford and will not trade away: the place that reads names."
    },
    {
      "id": "names_sell_copies",
      "text": "The archive is property now. Copies cost rations.",
      "effects": [
        { "type": "resource", "value": 15 },
        { "type": "morale", "value": -10 }
      ],
      "consequences": "Nine of the eleven pay. Two walk north without answers rather than buy their own dead. The ration take is real and lands in the ledger that night; so does the fact that the mess hall went quiet when the count was read aloud. The archive has become an economy, and economies of grief have a way of being remembered at the worst times."
    },
    {
      "id": "names_refuse_the_door",
      "text": "The registry is the shelter's. Send them on.",
      "effects": [
        { "type": "morale", "value": -15 }
      ],
      "consequences": "The treeline empties by dusk. Inside, the salvage team asks what the list was for and is told, accurately, that it was nothing. Two of them stop volunteering for archive duty within the week. Paper that answers nobody is just dry kindling with better penmanship."
    }
  ]
},
{
  "id": "quest_the_registry_deed",
  "title": "The Registry Deed",
  "description": "The water deeds pulled from the archive name a pre-war cistern field behind the rail cut — municipal reserve, sealed, unmapped since the war. The paperwork is the only map that survived.",
  "type": "exploration",
  "minDay": 90,
  "maxDay": 260,
  "factionTag": "neutral",
  "synopsis": "Three deeds, one survey sketch, and a cistern field that the valley's own government forgot on purpose or by catastrophe — the marginalia does not say which. The sketch shows six access shafts; five are marked with the municipal seal and one with a handwritten X and the word DON'T in the same ink. The last link of the archive's chain leads underground.",
  "prerequisites": ["quest_names_on_the_door"],
  "choices": [
    {
      "id": "registry_open_five",
      "text": "Work the five sealed shafts. Leave the X alone.",
      "effects": [
        { "type": "resource", "value": 30 },
        { "type": "morale", "value": 10 }
      ],
      "consequences": "Five shafts, five intact municipal cisterns, and water that tests clean enough to make the medic laugh once, briefly, like a cough. The sixth shaft stays welded the way someone wanted it. Your crews develop the superstition of not looking at it. The water lasts a season and the question lasts longer."
    },
    {
      "id": "registry_open_the_sixth",
      "text": "DON'T is advice, not law. Open the sixth shaft.",
      "effects": [
        { "type": "resource", "value": -10 },
        { "type": "survivor_health", "value": -15 },
        { "type": "morale", "value": -10 }
      ],
      "consequences": "The sixth cistern is not empty and never held water. What it held is not in any registry: the team comes up fast, three of them dosed, and the shaft gets its weld back within the hour at your order. The archived deeds were a map to a reserve; the handwritten X was a map to the reason the reserve needed guarding. The valley forgot the cistern field on purpose. Now so would you, if the dosimeter readings would let you."
    },
    {
      "id": "registry_deed_to_the_freeholders",
      "text": "Hand the deeds to the Salt Freeholders. Let the people who work water own it.",
      "effects": [
        { "type": "faction_relation", "value": 20 },
        { "type": "resource", "value": 10 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "The Freeholders read the deeds in silence, the way people read wills. Their crews open the five sealed shafts by winter and your shelter buys first refusal at friend rates — less water than you could have taken, forever, instead of a season's flood of it. The sixth shaft's X they honor without being told, and their foreman writes DON'T into their own books in the same hand."
    }
  ]
},
{
  "id": "quest_the_chalk_school",
  "title": "The Chalk School",
  "description": "The bunker's seven children have a school now: the teacher writes lessons on the corridor wall in chalk, four hours a day, in the space between the pump room and the sick bay.",
  "type": "community",
  "minDay": 50,
  "maxDay": 200,
  "factionTag": "neutral",
  "synopsis": "It started as babysitting and became arithmetic. The teacher — refugee, formerly nobody's business — writes on the wall because paper is rationed, and the children learn because the alternative is the corridors. The labor board has noticed that four adult-hours a day are going into reading instead of pumps. The leadership table has noticed that the wall is covered in the only sentences in the bunker that aren't about survival.",
  "prerequisites": [],
  "choices": [
    {
      "id": "chalk_sanction_the_school",
      "text": "Sanction it. Four hours a day, on the roster, rationed like any shift.",
      "effects": [
        { "type": "morale", "value": 15 },
        { "type": "labor_efficiency", "value": -5 }
      ],
      "consequences": "The school goes on the duty roster between pump and sick bay, and the corridor wall becomes, officially, the most protected surface in the bunker. Labor is four hours lighter for a season. The children learn to read the ration board before they learn cursive, which the teacher calls an acceptable curriculum for the century."
    },
    {
      "id": "chalk_shut_it_down",
      "text": "Every hand belongs to the pumps. End the lessons.",
      "effects": [
        { "type": "labor_efficiency", "value": 5 },
        { "type": "morale", "value": -15 }
      ],
      "consequences": "The wall gets scrubbed on a Sunday, which somebody will remember as a deliberate cruelty and somebody else will swear was routine maintenance. The four hours go to the pumps and the pumps do not notice. The children go back to the corridors, and the corridors, within a month, teach them things."
    },
    {
      "id": "chalk_let_it_run_unlisted",
      "text": "Look the other way, officially. The school runs when the roster doesn't.",
      "effects": [
        { "type": "morale", "value": 5 }
      ],
      "consequences": "The roster keeps its fiction and the wall keeps its chalk. It works right up until the first inspection argument, when the labor chief asks why the pump logs show four hours unaccounted and nobody can answer without naming the school — at which point looking the other way becomes the same decision as sanctioning, only without the credit."
    }
  ]
},
{
  "id": "quest_numbers_in_the_dark",
  "title": "Numbers in the Dark",
  "description": "The bunker's radio has started receiving a numbers station after the storms: a flat voice, groups of five, the same interval every ninth night. Your operator has been writing them down instead of sleeping.",
  "type": "investigation",
  "minDay": 120,
  "maxDay": 300,
  "factionTag": "neutral",
  "synopsis": "Nobody still transmits for fun. The groups of five repeat with a patience that machinery doesn't have and people don't either, and the interval — every ninth night, ten minutes after the hour — survived two storms that flattened relay towers. The operator thinks it is a schedule for somebody. The question is whether the shelter wants to be the somebody who answers, the somebody who decodes, or the somebody who never heard it.",
  "prerequisites": [],
  "choices": [
    {
      "id": "numbers_answer_blind",
      "text": "Transmit the next group back, on their frequency, after their interval.",
      "effects": [
        { "type": "morale", "value": -5 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "You answer with their own numbers and the flat voice stops mid-group — the first human thing it has ever done. The ninth-night schedule ends. Three weeks later a different station, same interval, reads a single group once: your bunker's grid square, spoken aloud, correctly. Somebody now knows who is listening. The operator sleeps worse and transcribes faster."
    },
    {
      "id": "numbers_decode_in_house",
      "text": "No transmission. Decode it in-house — the ledger-keeper and the operator, one sheet a night.",
      "effects": [
        { "type": "morale", "value": 10 },
        { "type": "resource", "value": -10 }
      ],
      "consequences": "It takes two months and most of the shelter's spare lamp oil. The decode is partial — a call sign, a bearing, and what the ledger-keeper swears is a quantity list in pre-war municipal codes — but it is yours, and it is real, and the bunker has a secret that belongs to everyone in it. The numbers keep coming every ninth night. You have started, without deciding to, thinking of the voice as a neighbor."
    },
    {
      "id": "numbers_log_and_ignore",
      "text": "Log the intervals. Change nothing. The dark talks to everybody.",
      "effects": [
        { "type": "morale", "value": -5 }
      ],
      "consequences": "The sheets go in the drawer and the drawer goes in the desk and the ninth nights pass uneventfully, which is precisely what was ordered. The operator keeps transcribing anyway — off duty, in private, in a notebook that is not the shelter's. Some schedules, once heard, do not require permission to be kept."
    }
  ]
},
{
  "id": "quest_the_voice_answers",
  "title": "The Voice Answers",
  "description": "The numbers station broke pattern for the first time: instead of groups of five, the flat voice read one word, three times, on the ninth-night interval. The word was a bearing. The bearing points at the rail cut.",
  "type": "investigation",
  "minDay": 150,
  "maxDay": 330,
  "factionTag": "neutral",
  "synopsis": "Whoever — whatever — kept the ninth-night schedule for two years has changed it because of the shelter: the decode, the answer, or the listening itself. The bearing resolves to the flooded rail cut, where the water stands over the tracks and the old signal hut still has a roof. The station is asking a question in the only grammar it has. Walking out to the cut is the only way to hear whether it was an invitation, a warning, or a test with no intended survivors.",
  "prerequisites": ["quest_numbers_in_the_dark"],
  "choices": [
    {
      "id": "voice_walk_the_bearing",
      "text": "Send the two-person team at dawn. Light, fast, transcribe everything.",
      "effects": [
        { "type": "resource", "value": 25 },
        { "type": "morale", "value": 15 }
      ],
      "consequences": "The signal hut holds a pre-war emergency cache — unopened, inventoried, dry — and a hand-crank field set still wired to the tower, its logbook open to a final entry dated the week the war ended: STATION KEPT. SCHEDULE HELD. AWAITING ANSWER. Two years of numbers were a lighthouse with no sea, kept by a machine and a promise. The shelter answers it now, every ninth night, in the clear. The voice reads the groups back. Neither side has said anything useful yet. Both sides keep the schedule."
    },
    {
      "id": "voice_watch_from_the_treeline",
      "text": "Nobody walks it. Glass and patience from the treeline, one week.",
      "effects": [
        { "type": "morale", "value": -5 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "A week of lenses shows the hut, the tower, the water, and nothing that moves except weather. The station's next broadcast drops the bearing and returns to groups of five, unchanged, as if the question had been heard and withdrawn. The treeline team comes in convinced of two opposite things and the shelter adopts, by exhaustion, the safer one: the cut stays off the route map, marked with a radio symbol nobody explains to the new arrivals."
    }
  ]
},
{
  "id": "quest_the_green_water_week",
  "title": "The Green Water Week",
  "description": "The well has been drawing green-tinged water for six days. The filter stack says it is within tolerance. The cook says she has been stretching the grey tins with boiled leather and nobody's stomach has noticed, because nobody's stomach is reporting for duty anymore.",
  "type": "medical",
  "minDay": 70,
  "maxDay": 240,
  "factionTag": "neutral",
  "synopsis": "Two problems arrived together and are pretending to be unrelated: the water table has shifted since the storms, and the cook has been quietly rationing around a shortage she never reported. The medic wants the well closed and tested; the labor board wants the week survived; the cook wants nobody to know about the leather. All three wants are correct, and the shelter can only fund one of them openly.",
  "prerequisites": [],
  "choices": [
    {
      "id": "green_close_and_test",
      "text": "Close the well. Full filter test. Ration the reserves and tell everyone why.",
      "effects": [
        { "type": "resource", "value": -10 },
        { "type": "survivor_health", "value": 10 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "Four days on reserves while the medic runs the stack through every reagent the shelter owns. The verdict: a mineral bloom, not contamination — ugly, safe, and seasonal. The truth costs a week of short rations and buys something the bunker has not had since the storms: a week where nobody whispered. The cook's leather secret comes out on its own, third night, and is absorbed without incident, which is its own kind of test result."
    },
    {
      "id": "green_boil_and_bluff",
      "text": "Double-boil, say nothing, keep the schedule. Panic is also a contaminant.",
      "effects": [
        { "type": "survivor_health", "value": -5 },
        { "type": "morale", "value": -10 }
      ],
      "consequences": "The double-boil holds the week and the green fades on its own, which the shelter will never know for certain — certainty was the thing you declined to buy. Two cases of stomach trouble get treated as what they look like. The cook keeps her secret and her shifts. The water log for that week says nothing, and nothing is what the log will say the next time the well runs strange."
    },
    {
      "id": "green_trade_for_clean",
      "text": "Buy clean water from the Freeholders' next run. Scrap for liters, one week only.",
      "effects": [
        { "type": "resource", "value": -15 },
        { "type": "survivor_health", "value": 5 },
        { "type": "faction_relation", "value": 10 }
      ],
      "consequences": "The Freeholders sell at road prices, which means fair, which means expensive. The week passes on clean water and the well recovers its color on the eighth day like nothing happened. The trade line to the pans is warmer now — water sellers remember water buyers — and the shelter's scrap pile is a season lighter, which is a problem for a different week, honestly earned."
    }
  ]
},
{
  "id": "quest_the_census_taker",
  "title": "The Census Taker",
  "description": "A clerk of the Meridian Compact has walked your road with a ledger, a pencil stub, and no escort, to count the valley's doors. Your door is fourteenth on the route. The questions are polite. The columns are not.",
  "type": "diplomacy",
  "minDay": 100,
  "maxDay": 280,
  "factionTag": "meridian_compact",
  "synopsis": "The Compact claims it is rebuilding civil record: how many, how fed, how armed, how governed. The clerk takes the answers without expression and writes in a hand too small to read across a table. A counted door is a door the Compact considers part of something; an uncounted door is a door the Compact has filed under 'pending.' The valley's other shelters have all, apparently, found time for the pencil stub.",
  "prerequisites": [],
  "choices": [
    {
      "id": "census_sign_honest",
      "text": "Answer every column honestly. Sign at the bottom.",
      "effects": [
        { "type": "faction_relation", "value": 10 },
        { "type": "morale", "value": 5 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "The clerk's columns close over your true numbers — population, stores, arms, the works — and the signature goes in beside them in your own hand. Within a season a Compact aid convoy adjusts its route to include you, which is worth more than the grain it carries: it means the road to your door is now somebody's official business. What else the numbers are worth, in rooms you will never see, is the price of the convoy, paid in advance."
    },
    {
      "id": "census_sign_light",
      "text": "Sign, but the columns get a smaller shelter than the one that's breathing.",
      "effects": [
        { "type": "faction_relation", "value": 5 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "Two-thirds of your population, half your stores, one armed watch instead of three. The clerk writes it all without a flicker — clerks are trained, or born, not to count what isn't offered. The lie works exactly as well as the truth would have, minus the convoy's full attention, and it sits in the Compact's archive now with your signature under it. Signatures outlive the reasons for them."
    },
    {
      "id": "census_refuse_the_ledger",
      "text": "The shelter declines to be counted. Show the clerk the road, politely.",
      "effects": [
        { "type": "faction_relation", "value": -15 },
        { "type": "morale", "value": 5 }
      ],
      "consequences": "The clerk makes one mark — you do not get to see whether it is 'refused' or 'pending,' and the difference will be decided in a building you will never enter. The Compact's next aid route skips your turn-off. So does its next tax route, and, for now, its next patrol. An uncounted door is free, and free is a flavor of alone that the valley is slowly running out of."
    }
  ]
}
]

NEW_06 = [
{
  "id": "quest_chalk_supply",
  "title": "Chalk and Arithmetics",
  "description": "The corridor school is down to nine sticks of chalk and the teacher has started writing smaller. Nine sticks is three weeks of lessons, or one week of large letters that the youngest can read from the back of the line.",
  "type": "community",
  "minDay": 60,
  "maxDay": 240,
  "factionTag": "neutral",
  "synopsis": "A trivial shortage with no trivial solutions: chalk can be cut from the archive's lime stores, synthesized from the medical lab's slaked lime at real cost, bought from the next Compact caravan at a price that will make the labor chief say the word 'school' like a diagnosis — or the lessons can shrink to fit the supply, which is the arithmetic the teacher refuses to teach.",
  "prerequisites": [],
  "choices": [
    {
      "id": "chalk_cut_from_archive",
      "text": "Cut sticks from the archive's lime shelving. The dead can sponsor the living.",
      "effects": [
        { "type": "morale", "value": 10 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "The shelving lime is soft and writes grey, and the teacher rationing it feels, she says, like eating the library. The children do not notice the difference between grey and white, which is the entire moral of the week. The lessons stay large enough to read from the back of the line for two more months."
    },
    {
      "id": "chalk_synthesize_in_lab",
      "text": "Slaked lime, settle tanks, cut and dry. Real chalk from the medical lab.",
      "effects": [
        { "type": "resource", "value": -10 },
        { "type": "morale", "value": 5 }
      ],
      "consequences": "Three days of lab time and a batch of reagents the sick bay had plans for produce forty clean white sticks — enough for a term. The medic signs the requisition without comment, which from her is a standing ovation. The first lesson written in the new chalk is the water cycle. The pump room crew stands in the corridor reading it for a week."
    },
    {
      "id": "chalk_shrink_the_lessons",
      "text": "Write smaller. Teach less. Make nine sticks last.",
      "effects": [
        { "type": "morale", "value": -10 }
      ],
      "consequences": "The teacher's handwriting shrinks to a hand-span tall and the youngest move to the front of the corridor, which means the oldest stop coming. Nine sticks last eleven weeks. Nobody ordered the school to end; the school just got quiet one letter at a time, which is how everything ends here."
    }
  ]
},
{
  "id": "quest_grave_markers",
  "title": "The Grave-Marker Carpenter",
  "description": "An old carpenter from the north settlements has asked for scrap wood, a corner of the workshop, and permission. He makes grave markers — proper ones, joined and planed — for the valley's dead, who have been getting stakes and stones.",
  "type": "community",
  "minDay": 80,
  "maxDay": 280,
  "factionTag": "traders",
  "synopsis": "The valley buries its dead with whatever is at hand: rebar stakes, door-plank slabs, stones with scratches. The carpenter wants to change that with offcuts and hand tools, at the cost of wood the shelter could burn, and at a second cost nobody has priced: a workshop corner where an old man planes names into boards, in earshot of everyone. The dead have no vote. The living have started to notice they're watching the door for him.",
  "prerequisites": [],
  "choices": [
    {
      "id": "markers_give_the_wood",
      "text": "Scrap wood and the bench by the hatch. Names deserve joinery.",
      "effects": [
        { "type": "resource", "value": -10 },
        { "type": "morale", "value": 15 }
      ],
      "consequences": "The first marker leaves the workshop for a picker who died at the rail cut, and the burial stops being a disposal. Word goes up and down the valley within a month: your door sponsors the marker-maker. Settlements that were your competitors start walking their dead your direction for the plane and the chisel, and the labor board discovers that the wood it wrote off as fuel has been buying something the fuel never could."
    },
    {
      "id": "markers_refuse_the_bench",
      "text": "Wood is heat and shoring. The dead are past needing corners.",
      "effects": [
        { "type": "resource", "value": 5 },
        { "type": "morale", "value": -15 }
      ],
      "consequences": "The carpenter shoulders his tools and goes north without an argument; he has heard no before now, and keeps his routes. The offcuts go to the stove and the stove does its job. The next burial uses a stake, and the person who digs it says, out loud, at the graveside, that it isn't right — and everybody at the grave knows he isn't talking about the digging."
    },
    {
      "id": "markers_trade_the_bench",
      "text": "Bench and wood for a share of his trade. The valley pays in markers; we take in rations.",
      "effects": [
        { "type": "resource", "value": 5 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "The arrangement works on paper and in rations: settlements up-valley pay the carpenter in food, the carpenger pays the shelter its cut, and the markers keep getting made. What the ledger doesn't carry is the phrase that starts following your traders up the road — the door that sells the dead's furniture. The rations are clean. The sentence isn't."
    }
  ]
},
{
  "id": "quest_spur_of_nine_rails",
  "title": "The Spur of Nine Rails",
  "description": "The winter storm scoured the embankment south of the bunker and uncovered a rail spur that no map in the shelter owns: nine spans of buried track, gauge intact, running toward the old quarry cut.",
  "type": "exploration",
  "minDay": 50,
  "maxDay": 200,
  "factionTag": "neutral",
  "synopsis": "Rail is the valley's hardest currency — shoring stock, trade bar, and the only metal the Railway Guild has ever begged for. The storm did the digging; the shelter holds the claim by squatter's arithmetic, which is to say by being the ones standing on it. The Guild will hear about the spur within the month regardless. The only open question is what your name is attached to when they do.",
  "prerequisites": [],
  "choices": [
    {
      "id": "spur_survey_quietly",
      "text": "Survey it ourselves, quietly, before anyone else's boot prints get into the snow.",
      "effects": [
        { "type": "resource", "value": -5 },
        { "type": "morale", "value": 5 }
      ],
      "consequences": "Two days of cold walking produce a real survey: nine spans, eleven tons by the engineer's slide rule, fishplates intact on seven. The shelter now knows what every other party in the valley will want to know, and knows it first. First is a position, not a plan — the plan comes when the Guild's scout walks the same embankment, which the snow says will be within three weeks."
    },
    {
      "id": "spur_call_the_guild",
      "text": "Send the Guild the bearings now. Let them find it under our welcome, not our silence.",
      "effects": [
        { "type": "faction_relation", "value": 15 },
        { "type": "morale", "value": 5 }
      ],
      "consequences": "The Guild's response arrives in four days, which means a brake-car was already within a day's walk and merely hadn't been invited. Their surveyors call the spur 'nine rails' in the log, and the log lists your shelter as the finder of record — a line that will be read aloud at Guild water points for a year. Finders, in the Guild's grammar, ride free."
    },
    {
      "id": "spur_bury_it_again",
      "text": "Snow, spoil, and silence. Some claims aren't worth the company they bring.",
      "effects": [
        { "type": "morale", "value": -5 },
        { "type": "resource", "value": -5 }
      ],
      "consequences": "Two days of re-burial and the embankment looks scoured and empty again, eleven tons of rail sleeping under the spoil like the rest of the valley's decisions. The shelter keeps its quiet and loses a claim it will hear about eventually anyway — the next storm scours honestly. The engineer marks the true bearings in his private book regardless. Private books are how the valley actually remembers."
    }
  ]
},
{
  "id": "quest_the_rails_decision",
  "title": "Eleven Tons of Argument",
  "description": "The spur is dug out, surveyed, and known: nine spans, eleven tons, and three parties with positions — the Guild wants it laid, the shelter's shoring wants it cut, and the labor board wants the argument to end before the ground freezes again.",
  "type": "politics",
  "minDay": 80,
  "maxDay": 260,
  "factionTag": "neutral",
  "synopsis": "Every ton of the spur can only be spent once. Laid and connected, it puts the shelter on the Guild's water-and-freight line permanently — with the inspections, tariffs, and attention that permanence invites. Cut for shoring, it reinforces the bunker's deep gallery for a generation and leaves the valley's rail map without you on it. Split down the middle satisfies the labor board and nobody's grandchildren.",
  "prerequisites": ["quest_spur_of_nine_rails"],
  "choices": [
    {
      "id": "rails_lay_the_line",
      "text": "Lay it. Join the Guild's map and take the freight, the tariffs, and the attention.",
      "effects": [
        { "type": "faction_relation", "value": 25 },
        { "type": "resource", "value": 20 },
        { "type": "bunker_defense_rating", "value": -5 }
      ],
      "consequences": "The first Guild brake-car rolls to your hatch in autumn, and after that one rolls every ninth day — the valley's whole economy runs on schedules somebody else keeps, and yours is now among them. Freight in, tariffs out, and a spur junction that puts your door on the map permanently. Maps, the census taker would tell you, cut both directions; the Guild's maps mostly cut toward the Guild."
    },
    {
      "id": "rails_cut_for_shoring",
      "text": "Cut it for the deep gallery. Rail we stand under beats rail that rolls past.",
      "effects": [
        { "type": "bunker_defense_rating", "value": 15 },
        { "type": "resource", "value": 10 },
        { "type": "faction_relation", "value": -10 }
      ],
      "consequences": "Eleven tons becomes forty feet of gallery shoring that the engineer will still be signing off on when everyone at this table is in the ground. The Guild's log records the spur as 'consumed locally,' which is their coldest possible prose. Your deep walls hold against everything the valley can decide to send them, which, this winter, feels like the same sentence."
    },
    {
      "id": "rails_split_the_spur",
      "text": "Five spans to the Guild, four to the shoring. Nobody's grandchildren get everything.",
      "effects": [
        { "type": "faction_relation", "value": 10 },
        { "type": "resource", "value": 10 },
        { "type": "bunker_defense_rating", "value": 5 },
        { "type": "morale", "value": -5 }
      ],
      "consequences": "The split holds because both sides wanted it more than they wanted to win: a stub junction on the Guild's map and twenty feet of shoring in the deep gallery. The labor board calls it balance. The engineer calls it a compromise with the frost line, and watches the stub junction every spring like it might change its mind."
    }
  ]
},
{
  "id": "quest_the_springs_audit",
  "title": "The Unregistered Spring",
  "description": "A Hydro Barons audit team is working the sector well by well, and their route sheet includes you. It does not include the spring the cook's crew has been drawing from since the green-water week — small, cold, unregistered, and by the Barons' water law, technically theirs already.",
  "type": "trade",
  "minDay": 110,
  "maxDay": 300,
  "factionTag": "traders",
  "synopsis": "The Barons' auditor arrives with a meter, a ledger, and the patience of an institution. The spring can be registered — legal, taxed, permanent, and forever after a line item in somebody else's book — or it can be sealed and hidden before the audit walk, which keeps it free and makes every draw from it an act of quiet theft. The third option is the oldest one in the valley: sell the spring's location to the audit before the audit finds it, and buy goodwill with water you were never going to keep anyway.",
  "prerequisites": [],
  "choices": [
    {
      "id": "springs_register_it",
      "text": "Walk the auditor to the spring yourself. Register it, tax and all.",
      "effects": [
        { "type": "faction_relation", "value": 10 },
        { "type": "resource", "value": -10 },
        { "type": "morale", "value": 5 }
      ],
      "consequences": "The spring gets a number, a meter, and a quarterly tithe — modest, because the Barons' modesty is a business strategy. What the registration buys is the thing money can't in this valley: a legal draw. No audit will ever surprise the cook's crew at the spring again, and no auditor will ever need to be wondered about. The water tastes the same. The looking over the shoulder stops."
    },
    {
      "id": "springs_seal_and_hide",
      "text": "Seal the spring chamber before the audit walk. The draw continues, quietly.",
      "effects": [
        { "type": "resource", "value": 10 },
        { "type": "morale", "value": -5 },
        { "type": "faction_relation", "value": -15 }
      ],
      "consequences": "The new seal plate goes on at night and the audit team walks past forty liters an hour of hidden water without slowing down. The spring keeps filling the shelter's jugs off every ledger in the valley — until the day a Barons patrol finds the plate, and the day that plate is found, the whole quiet season becomes evidence. The cook's crew has started drawing water at odd hours. Odd hours have witnesses."
    },
    {
      "id": "springs_sell_the_bearing",
      "text": "Sell the audit the spring's bearing before they find it. Goodwill is water too.",
      "effects": [
        { "type": "faction_relation", "value": 15 },
        { "type": "resource", "value": -5 },
        { "type": "morale", "value": -10 }
      ],
      "consequences": "You give the auditor the bearing before his meter does, and the ledger records your shelter as cooperative to the point of initiative. The Barons' standing with you warms by exactly the temperature of the spring they now own. The cook's crew learns what happened at the next draw, when the meter is already on the chamber: that the water was theirs for a season, and then it was sold, and that those two facts are the whole history of the valley in one afternoon."
    }
  ]
},
{
  "id": "quest_the_long_shift_mutt",
  "title": "The Long-Shift Mutt",
  "description": "A dog has adopted the night shift. It appeared three weeks ago at the hatch grille, does not beg, does not bark, and walks the pump corridor at 0300 with the regularity of a rostered hand. The night crew has started leaving it a corner of tin. The labor board has noticed it is not on the roster.",
  "type": "survival",
  "minDay": 90,
  "maxDay": 280,
  "factionTag": "neutral",
  "synopsis": "Mouths are arithmetic and the arithmetic says no. But the dog has made itself useful in the only way available to it — the night crew is a third less jumpy with a second set of ears on the corridor, and twice now it has stood at the hatch grille ten minutes before the watch heard anything at all. Formally sheltering it costs rations; formally turning it out costs the thing the night shift has that the day shift doesn't: the sense that somebody else is listening too.",
  "prerequisites": [],
  "choices": [
    {
      "id": "mutt_roster_the_dog",
      "text": "Roster it. Half ration, own bunk corner, name chosen by the night crew.",
      "effects": [
        { "type": "resource", "value": -5 },
        { "type": "morale", "value": 15 },
        { "type": "bunker_defense_rating", "value": 5 }
      ],
      "consequences": "The night crew names it Gauge, after the pressure instrument it sleeps under, and the labor board's objection dies the first time Gauge stands at the hatch grille whining at nothing — and nothing turns out to be two scavengers forty minutes from the treeline. Half a ration a day buys the bunker's only early-warning system with four legs and no shift complaints. The day crew pretends to resent it and takes turns walking it."
    },
    {
      "id": "mutt_turn_it_out",
      "text": "The arithmetic says no. Open the hatch at dusk and let the road re-adopt it.",
      "effects": [
        { "type": "morale", "value": -15 }
      ],
      "consequences": "Gauge goes out without a fight, which is the worst available outcome — a dog that argues could be argued back. It sits at the treeline until full dark, then walks the embankment south. The night crew's 0300 corridor walk continues for a week out of habit before anybody admits out loud that they're walking it alone. The half ration goes back into the common pot where nobody can taste it."
    },
    {
      "id": "mutt_unofficial_bunk",
      "text": "Not on the roster, not out the door. The dog exists between the lines, like half this bunker.",
      "effects": [
        { "type": "morale", "value": 5 },
        { "type": "resource", "value": -3 }
      ],
      "consequences": "Gauge becomes an institution of the gap between rules: fed from private tins, bedded in the pump corner, invisible on every ledger and present at every 0300. It works until the first hard month, when the ration board counts mouths and finds one that answers to no line — and then the shelter will have to decide, out loud, what it decided silently three weeks ago."
    }
  ]
}
]
