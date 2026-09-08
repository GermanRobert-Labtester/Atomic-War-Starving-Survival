# Round 5 — faction_lore.json expansion: 22 factions that ACT in the world but have no lore.
#
# WHY: 59 distinct faction ids are referenced across the data authority; only 23 had lore.
# faction_lore.json is load-bearing beyond prose — CombatCatalogLoader.Load THROWS FormatException
# when a combatant's faction_id does not resolve there (Plan10RemediationTests), and
# FactionDisplayNameCatalog.LoadFromJson takes UI display names from it. Adding entries widens the
# registry and names factions that were previously shown by raw id.
#
# CANON SOURCES USED (no invention where canon exists):
#   currents.json         — display_name, alignment, home_region, is_active, wants, offers,
#                           signature_quote, access_rule (15 of the 22)
#   faction_territory.json— display_name, classification, territory_scale, primary_resource_interest
#   crossing_factions.json— The Scale / The Compact / The Underwrite alignment + wants
#   characters.json       — named figures (Mirael Tesk, Mara Veln, Kaspar Drej, Joren Malk,
#                           Clerk Edor Vale, Yara Holm, Ivy Corrigan, Mira Vos, Halloran Vesk)
#   quest chains          — Bone Pickers / Blood Tithe / Broken Spears / Iron Slaves / Plowshares /
#                           Hospital Ships / Quarantine Purge / First Refusal
# Display names match canon exactly ("The Black Flotilla" per Plan23FlotillaFactionDepthTests:42;
# "The Silent Foundry" per ShelterMachineTellTests:87).
#
# DELIBERATELY EXCLUDED:
#   faction_blank_rows — DutyRosterIntegrationTests:80 asserts lore MUST NOT contain it.
#   Sentinels / archetypes, not world factions: faction_independent, faction_rebel,
#   faction_military, faction_order, faction_iron_clique, faction_meridian, faction_unlisted,
#   faction_unknown_intelligence, faction_automated_infrastructure,
#   faction_independent_survivors, faction_civil_defense, faction_doctrine_archetype_*.
#
# KEYSETS: the file already carries two shapes — 10-key (with relationships/dialogue_style/
# signature_quote/tech_offerings) and 6-key (without). iron_raiders and osteophages use the 6-key
# form on purpose: currents.json gives both an EMPTY signature_quote and, for the Raiders, no
# offers at all ("the absence is the design"). Omitting a voice is more faithful than inventing one.
# relationship values are restricted to the existing 10-value vocabulary.
# tribute_demands/tech_offerings are free-form in this file (existing entries already carry
# non-item values such as conscript_labor, watch_shifts, artillery_support); tribute-item
# validation applies only to warlords_sector_4 (WarlordCatalogValidator).

K10 = ["faction_id","display_name","ideology","origin_story","key_beliefs","dialogue_style",
       "signature_quote","relationships","tribute_demands","tech_offerings"]
K6  = ["display_name","faction_id","ideology","key_beliefs","origin_story","tribute_demands"]

NEW_LORE = [
{
  "faction_id": "faction_archivists",
  "display_name": "The Archivists of the Before",
  "ideology": "Corroborated memory as civic work: a name is only real once two living people will swear to it",
  "origin_story": "They came out of the drowned district with a chronicle bunker underneath it and no ground worth holding, which is why their territory is listed as none: what they keep is magnetic media and a rule. The rule was written after a bad year, when a name went into the Schedule on one mourner's word and the man named walked into the reading room eleven months later, alive, and had to listen to himself being read out. Two witnesses or the page stays blank. They will accept a photograph, a tape, a record sleeve; they will not accept a story nobody else can confirm. Mirael Tesk has run the listening outpost six years and still reads every entry aloud before it is copied, because the reading is the part that makes it a name.",
  "key_beliefs": [
    "A blank page is honest. A wrong name is a second death.",
    "Two living witnesses, or it stays out of the Schedule",
    "Say the name aloud while you write it; copying is not remembering",
    "Media outlives paper and paper outlives memory, so keep both"
  ],
  "dialogue_style": "Patient and corrective; will stop mid-sentence to fix a date, and thanks people for being precise",
  "signature_quote": "We are not collecting the past. We are refusing to invent it.",
  "relationships": {
    "quiet_house": "parallel",
    "cold_count": "transactional",
    "the_tempest": "transactional",
    "iron_garrison": "wary"
  },
  "tribute_demands": ["item_pre_war_photo_album", "item_cassette_tape", "item_vinyl_collection"],
  "tech_offerings": ["encrypted_drive", "corroborated_name_ledger", "archive_proof"]
},
{
  "faction_id": "faction_lamplighters",
  "display_name": "The Lamplighters",
  "ideology": "Impersonal public light: a beacon belongs to whoever is walking, and gratitude is not part of the arrangement",
  "origin_story": "A road-maintenance order that kept lighting beacons after there was no road authority left to pay for the oil. They lit for everybody, which meant they lit for raiders too, and the argument about that ran two winters until somebody asked for an exception and then asked twice. The lamps in that region went out one at a time over eleven days and the order stopped taking requests altogether. The beacons are mostly dark now and the order is not active, but the route ledger survives, and people still walk the old lit lines out of habit, and the habit keeps them safer than the dark ever would.",
  "key_beliefs": [
    "I don't light it for you. I light it.",
    "An exception is the end of a road; roads do not have favourites",
    "Oil, wicks and reflector glass are the only honest currencies",
    "Nobody is owed a beacon and everybody is served by one"
  ],
  "dialogue_style": "Spare and unhurried; refuses thanks explicitly and changes the subject to the oil or the weather",
  "signature_quote": "Ask me for the exception twice and I won't argue with you. I'll just start counting the lamps.",
  "relationships": {
    "long_walk": "transactional",
    "grain_exchange": "parallel",
    "quiet_house": "neutral",
    "iron_raiders": "hostile"
  },
  "tribute_demands": ["lamp_oil", "wicks", "reflector_glass", "batteries"],
  "tech_offerings": ["lit_routes", "route_ledger"]
},
{
  "faction_id": "faction_quiet_house",
  "display_name": "The Quiet House",
  "ideology": "Hospice as record-keeping: the dying are owed a death written down correctly, in their own words",
  "origin_story": "They grew up around the artesian spring sanctuary at St. Nicholas, where the water was clean enough that people came to get well and stayed to die. The runners are not medics and will correct you if you call them that: they do not treat, they accompany. Two knocks, a name, and one true thing about the person, and the true thing goes on the tag exactly as it was given. Not smoothed, not shortened, not made into a better sentence. That is the whole price and the whole service. The back room is never shown and nobody has ever asked twice. The House is not active now, but the tags are, and families still walk long distances to read one.",
  "key_beliefs": [
    "Not a medic. A witness with a pen.",
    "The true thing goes on the tag exactly as given",
    "The name is the price and there is no other price",
    "Quiet is not the absence of company, it is the absence of hurry"
  ],
  "dialogue_style": "Two knocks, then one question. Never interrupts, never paraphrases; writes while you speak and reads it back once",
  "signature_quote": "I'll write it the way you say it. That's the service and that's the cost, and they're the same thing.",
  "relationships": {
    "archivists": "parallel",
    "long_walk": "wary-truce",
    "cold_count": "neutral",
    "the_tally": "hostile"
  },
  "tribute_demands": ["blankets", "ethanol", "the_name"],
  "tech_offerings": ["quiet_passing", "catalogued_effects", "pure_spring_water_access"]
},
{
  "faction_id": "faction_grain_exchange",
  "display_name": "The Grain Exchange",
  "ideology": "A price board with no enforcement power: an institution that survives by being more useful than it is strong",
  "origin_story": "It began as chalk on the silo wall at New Ceres, where caravans posted what they had and what they needed, and it never acquired a guard, a charter, or any way to make anybody do anything. That is not modesty, it is the design. The board works for exactly one reason and the reason is hunger: everybody attending has eaten less than they wanted, and the board is the only place where that fact is useful. Mara Veln's stamp coordinates the allotments and the stamp has no authority behind it whatsoever except that people keep bringing their grain to be stamped. There are no dues. Ask the board for a favour and you will be told, politely, to argue with the board.",
  "key_beliefs": [
    "The board is the board. Argue with the board.",
    "No guards, no charter, no enforcement, no dues",
    "A shortfall shared early is survived; a shortfall hidden is a famine",
    "Hunger is the only membership requirement and everybody meets it"
  ],
  "dialogue_style": "Flat and procedural; refuses appeals, repeats the number, and raises neither voice nor eyebrows",
  "signature_quote": "There's nobody here to persuade. That number was written by everyone who's hungry, including you.",
  "relationships": {
    "the_scale": "transactional",
    "the_compact": "wary-truce",
    "the_office": "suspicious",
    "iron_raiders": "hostile"
  },
  "tribute_demands": [],
  "tech_offerings": ["bulk_trade_rates", "seasonal_shortfall_intel", "coordinators_stamp"]
},
{
  "faction_id": "faction_sun_seekers",
  "display_name": "The Sun-Seekers",
  "ideology": "Seasonal solar salvage kept by prediction: they trade in the false-spring window and they keep score of being right",
  "origin_story": "A rooftop crew that survived the first winter by stripping panels and never stopped counting the light. They trade only in FalseSpring and SilentSpring, because those are the weeks when the panels actually give and when everybody else is squinting, and they will tell you the window is nine days long before they will tell you their names. They hoard welder's glass and lead visors and sell both, dear, in season. They predicted the light returning and were right often enough that being right hardened into a manner: it came back, they said it would come back, and they would like that noted in whatever record you keep.",
  "key_beliefs": [
    "It came back. We told you it comes back.",
    "The window is the market; outside the window there is no market",
    "Eyes and panels both need glass, and glass is never free",
    "Prediction kept in writing is the only property that survives a winter"
  ],
  "dialogue_style": "Bright and insistent; keeps a running score of past predictions and offers it unprompted",
  "signature_quote": "Glass for the eyes, glass for the panels. Bring both or bring nothing; the window is nine days.",
  "relationships": {
    "cold_count": "parallel",
    "the_tempest": "transactional",
    "rebuilders": "wary-truce",
    "iron_raiders": "hostile"
  },
  "tribute_demands": ["item_welders_glass", "item_lead_visor"],
  "tech_offerings": ["solar_technology", "panel_salvage_routes", "false_spring_forecast"]
},
{
  "faction_id": "faction_osteophages",
  "display_name": "The Osteophages",
  "ideology": "No speech, no terms, no witnesses: what the valley cannot keep goes down the chute and clean metal comes back",
  "key_beliefs": [
    "A chute, and a bell, and a wait",
    "The metal comes back clean, and that is the entire statement of terms",
    "Nothing is refused and nothing is explained"
  ],
  "origin_story": "In the drowned district there is a chute, and a bell, and a wait. You put in what the valley cannot keep: contaminated technical waste, and, when a settlement has decided it cannot keep a person either, the person. After a time the bell rings and clean metal comes back. Ten metres of copper wire, coiled and bright, and scrap that does not read hot. Nobody has seen a face. Nobody has been offered terms. They do not negotiate verbally, and there is no record of them ever refusing a delivery, which is the detail that keeps settlements sending. What the metal is made from is not discussed at the chute, and asking is understood to be the sort of question that gets asked once.",
  "tribute_demands": ["toxic_tech_waste", "exiled_survivors"]
},
{
  "faction_id": "faction_the_tally",
  "display_name": "The Tally",
  "ideology": "Contract enforcement as a ritual of reading aloud: a debt is real because it was read to you twice",
  "origin_story": "Out of Lock Seven, where the sluice gate bastion and the toll canal turned every crossing into a transaction and every transaction into a document. They state the debt, the term, the rate and the forfeit, read back to you before you sign, and then read again, in full, before they enforce. Most people ask for the second reading. The Tally does not cheat and has never needed to: the forfeit was in the contract, in your hands, in your hearing, and enforcement is only the page being honoured. They are not active now. The contracts are, and contracts outlive the people who signed them, and somebody at Lock Seven still keeps the file.",
  "key_beliefs": [
    "Read back before signing. Read again before enforcement.",
    "The forfeit is not a surprise, it is a clause",
    "A debt spoken aloud in full cannot be misremembered later",
    "We do not chase. We collect, and those are different words"
  ],
  "dialogue_style": "Even, unhurried, faintly kind; offers the second reading as a courtesy and means it as a trap",
  "signature_quote": "You've heard it. Do you want it read again? Most people want it read again.",
  "relationships": {
    "the_scale": "suspicious",
    "undertow": "wary",
    "quiet_house": "hostile",
    "grain_exchange": "estranged"
  },
  "tribute_demands": ["the_forfeit"],
  "tech_offerings": ["contract_credit", "tally_debt_collection"]
},
{
  "faction_id": "faction_undertow",
  "display_name": "The Undertow",
  "ideology": "Rescue as a pricing mechanism: they do not cause the wreck, they only require that it happen",
  "origin_story": "They arrive fast. That is the first thing everybody notices and the last thing anybody argues with: a boat alongside inside ten minutes of a hull going into the water, hands reaching, ropes thrown, and a voice saying lucky we were close. The price is agreed afterwards, in the water or on the deck, with the swimmer's hands already full of somebody else's rope. They sell local knowledge honestly, they do real salvage, and they have pulled out living people, which is precisely why they still get called. Going into the drowned district alone is worse. That is the arithmetic they trade on and they have never once needed a second argument.",
  "key_beliefs": [
    "Lucky we were close",
    "The price is agreed when the swimmer can no longer refuse it",
    "Real rope, real salvage, real rescue; the terms come after",
    "A navigable drown is bad for business and an unnavigable one is better"
  ],
  "dialogue_style": "Warm, immediate, helpful; talks continuously during the rescue and does all of the arithmetic afterwards",
  "signature_quote": "Hold the line. We'll discuss it once you're in the boat, and you'll be reasonable then, because you'll be dry.",
  "relationships": {
    "black_flotilla": "hostile",
    "the_fleet": "hostile",
    "the_tally": "wary",
    "archivists": "suspicious"
  },
  "tribute_demands": ["an_unnavigable_drown"],
  "tech_offerings": ["rescue", "salvage_recovery", "local_knowledge"]
},
{
  "faction_id": "faction_cold_count",
  "display_name": "The Cold Count",
  "ideology": "Low-background measurement as the last honest trade: an answer that will not be falsified for anybody",
  "origin_story": "Four researchers survived in the slate quarry's low-background lab because the rock above them was old enough to be quiet, and they have been counting ever since. They want power, shielding and deep samples, and they return readings that are correct and provenance analyses that hold up in front of a crowd. They have been asked to soften a number more often than they have been thanked for one, they have never done it, and they no longer understand why the asking continues. Their roster carries four names and has not added one in three years, which is not secrecy: nobody walks out to the quarry and stays. What they have is not a secret. It is a measurement, and nobody has come to collect it.",
  "key_beliefs": [
    "It's not a secret. It's a measurement. Nobody came to collect it.",
    "A number that can be argued with is not a number, it is an opinion",
    "Quiet rock, clean shielding, long counts: patience is an instrument",
    "We will not falsify it, and we have stopped being flattered by being asked"
  ],
  "dialogue_style": "Literal and unemphatic; corrects units, refuses metaphor, pauses to check a figure before repeating it",
  "signature_quote": "Four names on the roster and a number nobody collects. We'll read it to you once, and we'll read it accurately.",
  "relationships": {
    "archivists": "transactional",
    "the_tempest": "parallel",
    "sun_seekers": "parallel",
    "iron_garrison": "wary"
  },
  "tribute_demands": ["power", "shielding", "deep_samples"],
  "tech_offerings": ["accurate_rad_readings", "provenance_analysis", "low_background_counting_time"]
},
{
  "faction_id": "faction_deserter_coalition",
  "display_name": "The Deserter Coalition",
  "ideology": "Discipline kept, uniform gone: competent people who can never be seen, and who pay for shelter in competence",
  "origin_story": "They took the armoured rail siding at Fort Karkov and the marshalling yards around it, because a redoubt that can be moved on rails is a redoubt that can be left, and leaving is the whole of their doctrine. Combat engineers, mostly, and good ones: they will shore your gallery, true your weapons, and hand over a patrol schedule that turns out to be accurate. They ask for three things and only three, which are silence, civilian clothing, and papers. They are decent people wanted for a capital offence, which makes them unreliable in exactly one direction. Sheltering them is the fastest existing route to a Garrison lockout, and they will tell you so themselves before you decide, because they have watched shelters decide badly.",
  "key_beliefs": [
    "We can never be seen, so we are worth exactly what we can do unseen",
    "Silence, civilian clothing, papers. Those are the terms",
    "A schedule handed over is a life paid back",
    "We left. We are not sorry. We are also not going back"
  ],
  "dialogue_style": "Quiet, precise, habitually checks the exits; states risks plainly and never asks to be saved",
  "signature_quote": "Don't answer to our names. Ask again in front of the wrong person and you've answered for us.",
  "relationships": {
    "iron_garrison": "hostile",
    "central_garrison": "hostile-open",
    "the_office": "suspicious",
    "scavenger_guild": "wary-truce"
  },
  "tribute_demands": ["silence", "civilian_clothing", "papers"],
  "tech_offerings": ["patrol_schedules", "weapon_maintenance", "disciplined_fighters"]
},
{
  "faction_id": "faction_the_provisioned",
  "display_name": "The Provisioned",
  "ideology": "Correctness in advance as a standing grievance: private shelters that were never on the Schedule and never needed to be",
  "origin_story": "They built before, at their own cost, on their own ground, with no permit from anybody, and when the Exchange came they closed their own doors and waited it out. Five years later they hold pre-war stock and working pre-Exchange technology, and they have developed opinions about the difference between people who prepared and people who were allocated. They want almost nothing, which is the unnerving part of trading with them: a party that does not need you cannot be pressured, only interested. They will sell, and they will help, and they will mention without heat and at length that nobody helped them build it, and that nobody has yet asked whether they would like help now.",
  "key_beliefs": [
    "Nobody helped us build it. Nobody has asked whether we'd like help now.",
    "Wanting almost nothing is the only real fortification",
    "The Schedule was for people who needed a schedule",
    "Preparedness is not a virtue we claim; it is an inventory we can show you"
  ],
  "dialogue_style": "Courteous, unhurried, faintly insufferable; never gloats directly, only by catalogue",
  "signature_quote": "To be clear, we're not refusing you. We're simply not needing you, and we can see that's the difficult part.",
  "relationships": {
    "grain_exchange": "transactional",
    "the_office": "suspicious",
    "deserter_coalition": "wary",
    "iron_garrison": "estranged"
  },
  "tribute_demands": ["almost_nothing"],
  "tech_offerings": ["prewar_stock", "working_pre_exchange_technology"]
},
{
  "faction_id": "faction_long_walk",
  "display_name": "The Long Walk",
  "ideology": "A continuous eleven-month circuit walked by thirty-odd people who will not stay a second night for anybody",
  "origin_story": "They are not a settlement and they have no intention of becoming one. Thirty-odd people move a circuit of Sector 4 that takes roughly eleven months, out of the High Scarp priory and back to it, carrying what they can carry and trading what they meet. They will take water, footwear and news, and they will give you goods from regions you cannot reach plus a situation report worth more than the goods, because it is thirty pairs of eyes over eleven months. They will not stay a second night, for anyone, for any offer, and the refusal is not coldness: a circuit that pauses stops being a circuit, and the circuit is the only thing they own. Kaspar Drej posts the odds on the season before they leave and never collects on them.",
  "key_beliefs": [
    "We'll be back round in about a year. Don't hold anything for us.",
    "A circuit that pauses is a settlement, and we are not a settlement",
    "News is the only cargo that gets heavier when you share it out",
    "Second nights are how walks end"
  ],
  "dialogue_style": "Cheerful and brief; packs while talking, answers in distances and seasons, is gone before the kettle boils",
  "signature_quote": "Water, boots, and what's happened. In that order, and we'll pay you in the same three.",
  "relationships": {
    "lamplighters": "transactional",
    "archivists": "parallel",
    "the_cutters": "transactional",
    "quiet_house": "wary-truce",
    "iron_raiders": "wary"
  },
  "tribute_demands": ["water", "footwear", "news"],
  "tech_offerings": ["unreachable_region_goods", "sector_wide_situation_report"]
},
{
  "faction_id": "faction_scavenger_guild",
  "display_name": "The Scavenger Guild",
  "ideology": "Claim discipline as the difference between salvage and mining: a site cut to the frame yields exactly once",
  "origin_story": "Out of the swap meet at Tinker's Notch, where the argument was never about price but about who arrived first and what arriving first was worth. The Guild's whole doctrine fits inside one observation: a site stripped to the frame yields once, and a site left standing yields for years, so whoever over-strips is not being greedy, they are eating next year. They license claims, they take apprentices, and they know the richest routes because they are the ones who left them alone. They will blacklist a shelter that over-strips a claimed site, and the blacklist is honoured across the whole Guild, permanently, without a hearing. It has happened twice. Both shelters are still telling the story, which is the only distribution the Guild has ever paid for.",
  "key_beliefs": [
    "A site cut to the frame yields once. That's not ethics, it's arithmetic.",
    "First refusal is earned by leaving something behind",
    "The blacklist needs no hearing, because the stripped site is the evidence",
    "An apprentice who can name the frame is worth three who can empty a room"
  ],
  "dialogue_style": "Practical and appraising; talks in yields and load-bearing members, and praises restraint more often than luck",
  "signature_quote": "Cut it to the frame and you've eaten next year. We'll remember which of you did it, and so will everybody we trade with.",
  "relationships": {
    "rebuilders": "transactional",
    "the_office": "suspicious",
    "deserter_coalition": "wary-truce",
    "iron_raiders": "hostile"
  },
  "tribute_demands": ["claim_respect", "tools"],
  "tech_offerings": ["richest_salvage_routes", "apprenticeship", "claim_registry_search"]
},
{
  "faction_id": "faction_iron_raiders",
  "display_name": "The Iron Raiders",
  "ideology": "None, and the absence is deliberate: no code, no contract, and no grievance that can be argued with",
  "key_beliefs": [
    "No code. No contract. No offer.",
    "The Cut is simply a place where roads slow down",
    "What you have is the only item on the list"
  ],
  "origin_story": "They hold the Cut ambush slopes and the dead highway redoubts, they move nomadically, and they want what you have. There is no negotiation to describe, because there is no position to negotiate from: no code they appeal to, no contract they honour, no ideology that could be contradicted, and no offer they have ever extended to anybody. Every other party in the valley has terms. The Raiders' absence of terms is not a gap in the record and it is not a mystery to be solved; it is the thing itself, and it is why the blood tithe and the broken spears both end the same way, with somebody else's name on a list and nothing left to argue about. Prepare for them. Do not plan to speak with them.",
  "tribute_demands": ["what_you_have"]
},
{
  "faction_id": "faction_the_tempest",
  "display_name": "The Tempest",
  "ideology": "Metered service as the last surviving form of civic continuity: a machine with no preferences, kept honest by whoever reads it",
  "origin_story": "It is not alive and it has no preferences, which is exactly why it survived the Exchange: nothing about it could be argued with, bribed, frightened or converted. It serves, it meters, and it waits for a human to read the meter. The relay network along the spine still carries its scheduled checks, and its logs are the most complete continuous record in the valley, because a machine never decides that a boring year is not worth writing down. Access is granted by maintenance and withdrawn by nobody; it simply keeps serving and the meter keeps reading. The census window is open and the count has not been presented, and the Tempest will hold that fact, entirely unbothered, until a person comes to collect it.",
  "key_beliefs": [
    "Service shall be metered, and the meter shall be read",
    "A machine has no preferences, and that is the only reason its record can be trusted",
    "Maintenance is access. There is no other door.",
    "The count not being presented is a fact about people, not about the machine"
  ],
  "dialogue_style": "Through its readers only: technicians in insulated gauntlets, procedural, quoting log lines and refusing to speculate",
  "signature_quote": "It doesn't want anything from you. It's still running, which is more than most of us manage.",
  "relationships": {
    "cold_count": "parallel",
    "archivists": "transactional",
    "the_office": "transactional",
    "sun_seekers": "transactional"
  },
  "tribute_demands": ["maintenance_time", "readings", "a_presented_count"],
  "tech_offerings": ["machine_log_access", "scheduled_q", "archive_proof"]
},
{
  "faction_id": "faction_black_flotilla",
  "display_name": "The Black Flotilla",
  "ideology": "Salvage under a board: every hull, lens and barrel counted, because the count is what makes it trade instead of wrecking",
  "origin_story": "Coastal people who kept the lighthouse bluff and the salvage docks and refused, early and stubbornly, to be called pirates. Their answer to the accusation is procedural rather than moral: a Board Officer, a fuel count, a manifest for every haul, and Halloran Vesk's signature at the foot of the Cape Beacon Commune's ledger. Bunker fuel and optics are the two things they genuinely hoard, lighthouse glass and the heavy oil that runs a winch, and both are counted nightly. Trust is described in their own documents as deep or not deep, and the difference is never what you say: it is whether your count and theirs have ever agreed in front of a third party.",
  "key_beliefs": [
    "A haul without a manifest is a wreck, and wrecking is what we are not",
    "Bunker fuel counted nightly, or the winch stops and so does the argument",
    "Optics are worth more than cargo, because optics find the next cargo",
    "Trust is a depth, not a promise"
  ],
  "dialogue_style": "Ship's-company blunt; talks in counts, drafts and weather windows, and signs what it agrees to",
  "signature_quote": "Read it back to me. When your count and my count agree in front of a third party, we'll discuss the rest of it.",
  "relationships": {
    "the_compact": "transactional",
    "grain_exchange": "transactional",
    "the_cutters": "transactional",
    "the_fleet": "suspicious",
    "undertow": "hostile"
  },
  "tribute_demands": ["bunker_fuel_count", "lighthouse_optics"],
  "tech_offerings": ["coastal_haul_passage", "salvage_manifest_verification", "bunker_fuel"]
},
{
  "faction_id": "faction_silent_foundry",
  "display_name": "The Silent Foundry",
  "ideology": "Iron and coal as a moral question nobody at the furnace is willing to answer: the same casting makes a plough and a chain",
  "origin_story": "The blast furnace and casting bay run under Joren Malk, who chose the name Silent Foundry and then had to explain it twice: the silence is about the work and not the people, and the work is extremely loud. They smelt, they cast, and they will take either contract, the iron that becomes a ploughshare and the iron that becomes a chain, at the same rate, on the same schedule, out of the same column of coal. That neutrality is why half the valley trades with them and why the other half will not, and Malk's position has never varied: the furnace does not choose, the customer does, and a foundry that began choosing would be a faction with an army rather than a foundry with a queue.",
  "key_beliefs": [
    "The furnace does not choose. The customer does.",
    "Same rate, same schedule, for the ploughshare and the chain",
    "A foundry that picks sides is an army with worse books",
    "Iron and coal counted by the column, or the heat is only opinion"
  ],
  "dialogue_style": "Loud, practical, unapologetic; shouts over the furnace and repeats the price without the least embarrassment",
  "signature_quote": "I'll cast either. I'll cast both. I'll cast them the same week at the same price, and that is the entirety of my politics.",
  "relationships": {
    "ordnance_foundry": "parallel",
    "iron_garrison": "transactional",
    "the_office": "transactional",
    "rebuilders": "wary-truce"
  },
  "tribute_demands": ["iron_and_coal_column", "scrap_metal"],
  "tech_offerings": ["cast_iron_fittings", "plough_irons", "foundry_queue_priority"]
},
{
  "faction_id": "faction_the_office",
  "display_name": "The Office",
  "ideology": "Freight, weighbridge receipts and cull contracts: violence administered as paperwork, filed and answered",
  "origin_story": "They hold the industrial rail corridor and the weighbridge network, and everybody calls them the Office, including themselves, because that is what they are: a room, a clerk, a stamp and a queue. Edor Vale signs the receipts and the receipts are what make a ton of freight real. They also issue cull contracts, a rad-stalker working a yard or a warlord sitting on a road, written in the same hand, on the same paper, in precisely the tone of a weighing slip, and that is the detail people find hard to absorb. They expect a faction that kills to look like one. It does not. It looks like a counter with a bell on it, and the bell is for service, and the service includes killing, and the paperwork for both is filed in the same drawer.",
  "key_beliefs": [
    "If it was not weighed, it did not arrive",
    "A contract is a form, and a form does not flinch",
    "The corridor runs because the receipts run, and the receipts run because the queue is honest",
    "Killing is a service line, priced and filed like any other"
  ],
  "dialogue_style": "Clerk's cadence: courteous, exact, mildly impatient with narrative; asks for the item, the tonnage and the date",
  "signature_quote": "Tonnage, date, and who signs. The remainder of what you just told me is not on the form.",
  "relationships": {
    "grain_exchange": "transactional",
    "the_tempest": "transactional",
    "silent_foundry": "transactional",
    "scavenger_guild": "suspicious",
    "deserter_coalition": "suspicious",
    "iron_garrison": "wary-truce"
  },
  "tribute_demands": ["rail_freight_dues", "weighbridge_receipts"],
  "tech_offerings": ["rail_corridor_passage", "cull_contract", "hardware_manifest"]
},
{
  "faction_id": "faction_the_cutters",
  "display_name": "The Cutters",
  "ideology": "Ice and brine cut to a schedule: the pan yields salt, the road yields passage, and both are measured in days",
  "origin_story": "Out of the tidal estuary's salt evaporation basins at Brine-Pan Hollow, where the work is a calendar and the calendar is weather. Yara Holm cuts the pans. Ivy Corrigan pilots the ice road and has never lost a load, a fact she mentions only when asked and then only as a correction to somebody else's version. Mira Vos keeps the brine assay, because salt that reads wrong kills slowly and the Cutters would sooner lose a sale than a customer. They are not sentimental about the cold; they have simply organised it. What the pan gives in summer the road gives in winter, and between the two the settlement eats, and anybody who wants either pays in days of patience.",
  "key_beliefs": [
    "The pan in summer, the road in winter, and nothing in between but the assay",
    "Salt that reads wrong kills slowly; we would rather lose the sale",
    "An ice road is a schedule, not a route",
    "Nobody has lost a load, and that is a discipline rather than a boast"
  ],
  "dialogue_style": "Weather-first and unsentimental; answers in days and assays, and corrects exaggeration as a matter of routine",
  "signature_quote": "Pan's giving, road opens in eleven days. If your salt reads low I'll tell you before you buy, so bring the assay and not an argument.",
  "relationships": {
    "salt_freeholders": "parallel",
    "black_flotilla": "transactional",
    "long_walk": "transactional",
    "the_scale": "transactional",
    "the_tally": "wary"
  },
  "tribute_demands": ["brine_assay_days", "ice_road_pilotage"],
  "tech_offerings": ["salt_and_brine_minerals", "ice_road_passage", "preservation_cure"]
},
{
  "faction_id": "faction_the_compact",
  "display_name": "The Compact",
  "ideology": "A crossing authority that recruits by signature: every signatory is a road kept open and a witness gained in the same stroke",
  "origin_story": "What remains of a northern supply administration that used to move grain by shuttle and heavy haul along an iron corridor, and now moves paper. They want signatories and they will explain why without embarrassment: a signature is a road somebody has agreed to keep open, and it is also a person who can later be asked what they agreed to. Their convoys still run, the northern line, the outpost runs, the granary shuttle, thinner than the manifest admits and more regular than anybody expects of them. They are peaceful by policy rather than by temperament, and the policy is arithmetic: an authority that cannot enforce anything can only keep multiplying the number of people who have signed, until enforcement stops being necessary.",
  "key_beliefs": [
    "A signatory is a road kept open and a witness gained, in one stroke",
    "Peaceful by arithmetic, not by temperament",
    "The convoy runs whether or not the manifest is honest, and the running is the point",
    "What was agreed can be asked about later, and that is the whole of law"
  ],
  "dialogue_style": "Administrative warmth: forms, dates, follow-up, and a genuine interest in whether you have eaten",
  "signature_quote": "Sign here, and here. The second one matters more, because it's the one saying you'll be asked.",
  "relationships": {
    "the_scale": "transactional",
    "black_flotilla": "transactional",
    "grain_exchange": "wary-truce",
    "the_office": "suspicious"
  },
  "tribute_demands": ["signatories"],
  "tech_offerings": ["northern_supply_line_passage", "granary_shuttle_space", "heavy_haul_corridor"]
},
{
  "faction_id": "faction_the_fleet",
  "display_name": "The Fleet",
  "ideology": "Naval jurisdiction asserted over water that has stopped being navigable: the ships are hospitals and the hospitals are quarantines",
  "origin_story": "They keep the Deep Coast roadstead and an offshore patrol grid that no longer has much to patrol, and they still issue jurisdiction, still board, still log. Their vessels run as hospital ships because that is what the crews are for now, and the hospital ships run quarantine because that is what a boarded vessel becomes when somebody aboard is coughing. The two facts sit inside one another and the Fleet does not pretend otherwise: the rescue is real, the care is real, and the purge that follows a bad boarding is also real and also logged. They will take you aboard. They will treat you properly. They will also decide, on evidence and without appeal, whether you come off again.",
  "key_beliefs": [
    "Jurisdiction is what you keep doing after the water stops being worth patrolling",
    "A hospital ship is a quarantine that has not been declared yet",
    "Board, log, treat, and if necessary do not disembark",
    "The purge is entered in the same book as the rescue, and that is correct"
  ],
  "dialogue_style": "Watch-officer formal; issues permissions rather than answers, logs while you speak, never raises their voice",
  "signature_quote": "You're aboard, so you're under a decision now. The treatment isn't conditional. The disembarking is.",
  "relationships": {
    "the_compact": "transactional",
    "quiet_house": "parallel",
    "black_flotilla": "suspicious",
    "undertow": "hostile"
  },
  "tribute_demands": ["boarding_compliance", "medical_log_entry"],
  "tech_offerings": ["offshore_passage", "hospital_berth", "patrol_grid_intel"]
},
{
  "faction_id": "faction_the_scale",
  "display_name": "The Scale",
  "ideology": "Weighing as arbitration: the scale settles a dispute because both sides agreed in advance to be measured by it",
  "origin_story": "A consortium running the salt artery and the grand caravan circuit and, more to the point, the weighbridge at the crossing where both of them pass. Their authority is entirely borrowed and entirely sufficient: two parties who will not trust one another will trust a scale neither of them owns, provided they agreed to it beforehand. That agreement is the product. The Scale does not judge, does not interpret and does not enforce; it measures, in public, once, and publishes the figure. Chemical exchange runs through the same yard on the same principle, which is why the crossing keeps the Consortium conditional rather than friendly. Everybody wants the number and nobody wants to be the party arguing with it afterwards.",
  "key_beliefs": [
    "Agree to be measured before you argue, or the argument is all that's left",
    "The Scale does not judge. It publishes a figure.",
    "One number in public is worth more than two verdicts in private",
    "Both arteries, one weighbridge: neutrality is a location"
  ],
  "dialogue_style": "Market-flat and scrupulous; states the figure, the tolerance and the hour, then stops talking",
  "signature_quote": "Tolerance was a half measure and the reading was taken at noon in front of the both of you. Argue with the arithmetic, or shake hands.",
  "relationships": {
    "grain_exchange": "transactional",
    "the_compact": "transactional",
    "the_cutters": "transactional",
    "the_tally": "suspicious"
  },
  "tribute_demands": ["trade_goods"],
  "tech_offerings": ["weighbridge_arbitration", "salt_artery_passage", "caravan_circuit_slot"]
}
]
