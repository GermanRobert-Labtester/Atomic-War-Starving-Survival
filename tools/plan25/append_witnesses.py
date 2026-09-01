import json
path = 'Assets/StreamingAssets/Data/muster_witnesses.json'
data = json.load(open(path))

existing = {w["id"] for w in data["witnesses"]}

def witness(id, name, loc, day_min, priority, variants, faction=""):
    w = {"id": id, "witness_name": name, "location_id": loc,
         "knowledge_key": "history_" + id, "day_min": day_min, "priority": priority}
    if faction:
        w["faction_id"] = faction
    w["testimonies"] = variants
    return w

def v(vid, body, any_flags=None, all_flags=None, forbids=None):
    t = {"variant_id": vid, "body": body}
    if any_flags: t["requires_any_flags"] = any_flags
    if all_flags: t["requires_all_flags"] = all_flags
    if forbids: t["forbids_flags"] = forbids
    return t

new = [
    witness("witness_messengers_keeper", "The Messenger's Keeper", "loc_d9_cache_bunker_delta", 250, 35, [
        v("helped", "He shows the message tube like a relic: the collector's route, carried and kept instead of burned. 'The shelter held the door for a courier of the man who taxes them. You want to know what the shelter is? That. Somebody had to be last to stop believing in messages, and it wasn't them.'",
          all_flags=["flag_messenger_kept"]),
        v("failed", "He will not say what was in the last tube. He says what happened to the man who carried it, in the winter after the shelter stopped pretending the collector was a burden it had chosen. 'Mercy you can take back was never mercy. It was inventory.'",
          all_flags=["flag_become_warlord"]),
        v("absent", "He catalogues the undelivered: names, last directions, the words that arrived too late to matter. 'The war is not the shells. The war is all the letters nobody carried.'"),
    ]),
    witness("witness_claimant_auditor", "The Claimant Auditor", "loc_terrace_pumphouse", 210, 30, [
        v("helped", "She unfolds the audit with both hands, careful of the crease. 'The intake numbers, published, with the shelter's mark beside them. My claim was in that audit. Eight seasons of water I was owed. You made it arithmetic instead of a grudge. Arithmetic can be paid.'",
          all_flags=["flag_favor_hydro_intake_audited"]),
        v("failed", "She speaks like someone reading a file that never closes. 'The audit was buried. My claim went with it, and the shelter stood in the room where the burying happened and said nothing people could use.'",
          all_flags=["flag_grievance_hydro_intake_disputed"]),
        v("absent", "She keeps other people's claims the way monks keep hours: water owed, water delivered, water taken. 'A claim is just a promise with a date on it. Most promises drown first.'"),
    ], faction="faction_hydro_barons"),
    witness("witness_hydro_envoy", "The Hydro Envoy", "loc_terrace_pumphouse", 200, 30, [
        v("helped", "She pours two measures and sets one in front of the empty chair, which is the Barons' way of saying it to someone who is not there. 'Accords are easy to sign and hard to keep. The shelter kept one. At the pumphouse that is the whole of the law and most of the religion.'",
          any_flags=["flag_favor_hydro_water_accord_honored", "flag_favor_hydro_toll_paid"]),
        v("failed", "She does not accuse. She recites. The defaulted toll, the refused appeal at Unit Four, the dates. 'I am not telling you what you are. I am telling you what the ledger says, and the ledger is never finished with anyone.'",
          any_flags=["flag_grievance_hydro_toll_defaulted", "flag_grievance_hydro_appeal_refused"]),
        v("absent", "She explains the rate card to no one in particular: intake, purification, delivery, counted twice. 'Water is only free where nobody has ever counted it. We count.'"),
    ], faction="faction_hydro_barons"),
    witness("witness_raider_parley_survivor", "The Parley Survivor", "loc_iron_raiders_den", 200, 30, [
        v("helped", "She turns her cup over to show it is empty, which in the den's sign means the talk is done and honest. 'I walked out of a parley the shelter kept. Their word held when it would have been cheaper not to. The code remembers. The code is the only thing about us that will outlive the war.'",
          all_flags=["flag_favor_raider_parley_honored"]),
        v("failed", "She holds up three fingers, then folds one. 'Three of us went to terms with the shelter's people. Terms the shelter broke, or terms the shelter let get widened into none at all. The den does not forget a thing like that. It recalculates.'",
          any_flags=["flag_grievance_raider_parley_broken", "flag_grievance_raider_code_widened"]),
        v("absent", "She recites the code like a litany with teeth in it: what the Toll takes, what it refuses, what it does to the ones who break parley. 'You wanted to know what separates us from the winter. That is it. That is all of it.'"),
    ], faction="faction_iron_raiders"),
    witness("witness_camp_medic", "The Camp Medic", "loc_denial_cut_substation", 262, 30, [
        v("helped", "Her hands are busy the whole time she talks, folding bandages that are already square. 'When the pot was down to water and memory, the shelter's share came over the wire with the roster kept open. Shared supply. Shared names on the sick list. Do you know how many camps ever got that far?'",
          any_flags=["flag_favor_coalition_supply_shared", "flag_favor_coalition_mediation_served"]),
        v("failed", "She finally stops moving her hands. 'We asked for the pot and the peace, both small, both spelled out. The shelter counted its margin and let the truce find out what it was worth. I buried the arithmetic.'",
          any_flags=["flag_grievance_coalition_supply_refused", "flag_grievance_coalition_mediation_refused"]),
        v("absent", "She describes triage under the camp's tarp: four banners, one table, the same rules for everyone or no rules at all. 'Neutrality is not a feeling. It is who gets the cot first.'"),
    ], faction="faction_deserter_coalition"),
    witness("witness_camp_dissenter", "The Camp Dissenter", "loc_denial_cut_substation", 265, 25, [
        v("helped", "'I was the one who said the rules were rot - that the witness protections would get someone killed before the wire ever did. Half the camp wanted me gone. The shelter stood in the meeting and said dissent was a thing neutral ground existed for.' She shrugs. 'I am still here. So are the rules, fixed.'",
          all_flags=["flag_favor_coalition_rules_first"]),
        v("failed", "'There was a meeting. I spoke against the wire going up first, and the shelter's people were in that meeting, and the shelter chose the wire. Now the camp has a fence and I have a chapter in somebody's security file.'",
          all_flags=["flag_grievance_coalition_security_backed"]),
        v("absent", "She keeps a list of every rule the camp has changed and why. 'A truce is a machine for disagreeing without dying. Ours needs fixing every month. That is what winning looks like out here.'"),
    ]),
    witness("witness_deserter_elder", "The Deserter Elder", "loc_denial_cut_substation", 268, 30, [
        v("helped", "He unfolds the pencil list, soft with handling. 'The quiet faction's gathering - the shelter carried it when carrying it was not safe. Old men, widows, quartermasters. We asked for a gathering, not a surrender. The shelter heard the difference.'",
          all_flags=["flag_peace_faction_forms"]),
        v("failed", "'The requisition came and the shelter signed it. Fuel, filters, names for the labor lists. My boy walked to the front behind their signature.' He folds his hands. 'I do not say the shelter was wrong. I say I know what the signature bought, because I buried what it cost.'",
          all_flags=["flag_war_requisition_refused"]),
        v("absent", "He says what the camp is, plainly: exhausted people who agreed to sit near their enemies because the alternative had stopped having a name. 'Nobody here is peaceful. We are done. There is a difference and the difference is the whole point.'"),
    ], faction="faction_deserter_coalition"),
    witness("witness_queue_singer", "The Queue Singer", "loc_grain_silo", 270, 20, [
        v("helped", "'You were at the grain queue. You heard it - eleven minutes with no war in it, and then your kettle came down the line like a promise. A song does not end a war. It remembers what the war is interrupting. That is not nothing.'",
          all_flags=["flag_peace_bread_before_bullets"]),
        v("failed", "'The willing ledger had one name in eleven days, and the queue knew it, and the shelter walked past the queue like it was weather. Eleven minutes was all the peace we got to hold.'",
          any_flags=["flag_peace_volunteers_dry"]),
        v("absent", "She says queues are the truest map of a district: what it lacks, what it fears, who it lets jump ahead. 'The war queues are the same as the bread queues, except nobody sings in those.'"),
    ]),
    witness("witness_overflow_medic", "The Overflow Medic", "loc_st_brigids_almshouse", 265, 25, [
        v("helped", "'The almshouse filled, then the crypt, then my hallway. The shelter opened its doors to the overflow - both banners, no book, no questions at the cot. In eleven months of war I have seen two doors like that. I have seen fewer that stayed open.'",
          all_flags=["flag_war_shelter_took_wounded"]),
        v("failed", "'The requisition took the shelter's filters and medicine to the front, receipted and polite. My overflow ward ran short that same week. I am sure the form said victory somewhere on it. The forms always do.'",
          all_flags=["flag_war_requisition_met"]),
        v("absent", "She talks about bodies, which is how she talks about war: how many, how fast, how young. 'Doctors do not get to choose the argument. We get the arithmetic after.'"),
    ]),
    witness("witness_summit_envoy", "The Summit Envoy", "loc_garrison_checkpoint_gamma", 245, 30, [
        v("helped", "'Every summit fails for the same reason: nobody in the room can be believed. Then the shelter published what it found - the water, the flow logs, the truth at the prisoner's gate - and for one season the word of a shelter was worth more than the word of a banner. That is a kind of diplomacy. It is the only kind left that works.'",
          any_flags=["flag_escalation_bitter_water_investigated", "flag_escalation_cistern_published", "flag_escalation_prisoner_truth_told"]),
        v("failed", "'I set the chairs myself. The empty one was the Toll's answer, and the convoy question went unanswered, and the shelter - which could have made the room expensive for liars - chose its own accounts instead. Summits do not fail from silence. They fail from everyone choosing their own accounts.'",
          any_flags=["flag_escalation_empty_chair", "flag_escalation_stopped_convoy"]),
        v("absent", "He describes what a ceasefire actually is: a shape the exhaustion makes. 'Peace is not signed. It is the last thing left standing when everyone is too tired to hold a weapon and too proud to say so. My job is the paperwork in between.'"),
    ]),
    witness("witness_levy_party_chief", "The Levy Party Chief", "loc_iron_raiders_den", 240, 20, [
        v("complicated", "He weighs a tenth-share of chalk in his palm like he is checking an old scar. 'The crossing. The shelter paid, or ran it in the dark, or stood and fought - we keep honest books on all three. Whichever it was, the Toll remembers the shelter as a crossing that answered back. Around here that is a kind of respect. It is also a reason to count your convoys.'",
          any_flags=["flag_grievance_raider_passage_evaded", "flag_grievance_raider_passage_fought"]),
        v("absent", "He explains the levy without shame: a road, a price, a chalk receipt. 'Bandits take. The Toll prices. You can hate both, but only one of them gives you paper.'"),
    ], faction="faction_iron_raiders"),
]

added = [w for w in new if w["id"] not in existing]
data["witnesses"].extend(added)
json.dump(data, open(path, 'w'), indent=2, ensure_ascii=False)
print("witnesses now:", len(data["witnesses"]), "| added:", [w["id"] for w in added])
