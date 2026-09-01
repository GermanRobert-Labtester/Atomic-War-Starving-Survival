import json
path = 'Assets/StreamingAssets/Data/faction_war_events.json'
data = json.load(open(path))

def chain(cid, band, title, factions, loc, stages):
    return {"chainId": cid, "band": band, "title": title, "factionsInvolved": factions,
            "locationId": loc, "stages": stages}

def stage(sid, min_day, trigger, title, body, requires_flag="", produces_flag="", choices=()):
    st = {"stageId": sid, "minDay": min_day, "triggerCondition": trigger, "title": title,
          "bodyText": body}
    if requires_flag:
        st["requiresFlag"] = requires_flag
    if produces_flag:
        st["producesFlag"] = produces_flag
    st["choices"] = list(choices)
    return st

def choice(cid, text, morale=0, leads="", produces="", faction="", delta=0):
    c = {"choiceId": cid, "text": text, "moraleDelta": morale, "leadsToStageId": leads}
    if produces:
        c["producesFlag"] = produces
    if faction:
        c["standingFactionId"] = faction
        c["standingDelta"] = delta
    return c

G = "faction_central_garrison"
R = "faction_rebuilders"

# ── Pre-war escalation (E-P2..P6): grievance-gated, tension backdrop ─────
data["chains"].append(chain(
    "evt_p25_stopped_convoy", "escalation", "The Stopped Convoy", [G, R],
    "loc_garrison_checkpoint_gamma",
    [stage("evt_p25_stopped_convoy_s1", 220,
        "Plan 25 (E-P2): fires while the shelter's defaulted Hydro purification toll is on the account ledger.",
        requires_flag="flag_grievance_hydro_toll_defaulted",
        produces_flag="flag_escalation_stopped_convoy",
        title="Held at the Checkpoint",
        body="A Barons' water convoy never made it past Checkpoint Gamma. The Garrison's manifest clerk calls it contraband; the Barons' tally-keeper calls it an unpaid protection grudge wearing a uniform. The convoy's driver sits on her running board and refuses to leave the cargo, and the queue behind her grows by the hour. Everyone looks at the shelter, whose own toll account is marked in the same book.",
        choices=[
            choice("evt_p25_stopped_convoy_s1_c1", "Post the shelter's own arrears as proof the tolls, not the cargo, are the issue", morale=1, leads="evt_p25_stopped_convoy_s2"),
            choice("evt_p25_stopped_convoy_s1_c2", "Back the checkpoint - contraband rules are the only rules left", morale=-1, leads="evt_p25_stopped_convoy_s2", faction=G, delta=4)])]))

data["chains"].append(chain(
    "evt_p25_bitter_water", "escalation", "Bitter Water", ["faction_hydro_barons", R],
    "loc_terrace_pumphouse",
    [stage("evt_p25_bitter_water_s1", 230,
        "Plan 25 (E-P3): fires while the shelter's refusal of the Barons' emergency appeal stands on the record.",
        requires_flag="flag_grievance_hydro_appeal_refused",
        produces_flag="flag_escalation_bitter_water",
        title="What the Well Did to the Children",
        body="Three families at the terrace settlement drank from the lower well for a week and spent the next week doing nothing else. The Barons say the aquifer shifted. The terrace elders say the Barons throttled the upper intake to punish non-payers, and the timing fits so well it doesn't need to be true. Nobody can prove either story, and the shelter's refusal of Unit Four's appeal is in everyone's mouth.",
        choices=[
            choice("evt_p25_bitter_water_s1_c1", "Send the shelter's test gear and publish what it finds", morale=2, leads="evt_p25_bitter_water_s2", produces="flag_escalation_bitter_water_investigated"),
            choice("evt_p25_bitter_water_s1_c2", "Repeat the Barons' aquifer line and leave the accusation standing", morale=-2, leads="evt_p25_bitter_water_s2")])]))

data["chains"].append(chain(
    "evt_p25_empty_chair", "escalation", "The Empty Chair", [G, "faction_black_ops"],
    "loc_d9_cache_bunker_delta",
    [stage("evt_p25_empty_chair_s1", 240,
        "Plan 25 (E-P4): fires while the shelter's broken raider parley stands in the Toll's memory.",
        requires_flag="flag_grievance_raider_parley_broken",
        produces_flag="flag_escalation_empty_chair",
        title="A Summit With One Seat Turned Away",
        body="The Garrison called a district summit about the checkpoint tariffs and asked every banner to send a voice. The Toll sent a chair. Just a chair, set at the table's end, facing the door - their sign that a parley was broken once and the den does not sit twice. The Garrison chairwoman will not start without the seat filled and will not fill it herself. The shelter holds the only debt both sides will still mention.",
        choices=[
            choice("evt_p25_empty_chair_s1_c1", "Ask the Garrison to send the debt-settler the den names", morale=1, leads="evt_p25_empty_chair_s2"),
            choice("evt_p25_empty_chair_s1_c2", "Let the chair sit empty and start without the Toll", morale=-1, leads="evt_p25_empty_chair_s2", faction=G, delta=3)])]))

data["chains"].append(chain(
    "evt_p25_cistern_toll_blockade", "escalation", "Cistern Toll", ["faction_hydro_barons", "faction_ash_sign"],
    "loc_terrace_pumphouse",
    [stage("evt_p25_cistern_toll_blockade_s1", 250,
        "Plan 25 (E-P5): fires while the shelter's siding against the intake audit stands in the Barons' books.",
        requires_flag="flag_grievance_hydro_intake_disputed",
        produces_flag="flag_escalation_cistern_blockade",
        title="The Valve That Turns Slowly",
        body="The Barons deny, in writing, that the terrace cistern's feed has been reduced. The feed has been reduced. Ash Sign runners clocked it: four fingers of flow where there were six, adjusted the week the audit fight went against the technicians. No one says blockade. The valve simply turns, slowly, in a room nobody is allowed into, and the paperwork calls it maintenance.",
        choices=[
            choice("evt_p25_cistern_toll_blockade_s1_c1", "Publish the runners' flow log at the Exchange", morale=1, leads="evt_p25_cistern_toll_blockade_s2", produces="flag_escalation_cistern_published"),
            choice("evt_p25_cistern_toll_blockade_s1_c2", "Buy silence with a season's paid toll", morale=-1, leads="evt_p25_cistern_toll_blockade_s2")])]))

data["chains"].append(chain(
    "evt_p25_prisoner_at_the_gate", "escalation", "Prisoner at the Gate", [G, "faction_iron_raiders"],
    "loc_iron_raiders_den",
    [stage("evt_p25_prisoner_at_the_gate_s1", 230,
        "Plan 25 (E-P6): fires while the shelter's fought levy stands unsettled with the Toll.",
        requires_flag="flag_grievance_raider_passage_fought",
        produces_flag="flag_escalation_prisoner_gate",
        title="One of Theirs, One of Ours",
        body="The Toll took a Garrison courier at the crossing the shelter fought over, and now they are holding him under the code - fed, unharmed, and priced. The Garrison, in turn, holds one of the den's young hands caught at the rail yard. The exchange should be simple. It is not, because the den's price is not the prisoner: it is the shelter's public word that the crossing fight was the shelter's doing, not the den's break of parley. The Garrison is listening. So is everyone.",
        choices=[
            choice("evt_p25_prisoner_at_the_gate_s1_c1", "Say the truth plainly: the crossing was the shelter's fight", morale=1, leads="evt_p25_prisoner_at_the_gate_s2", produces="flag_escalation_prisoner_truth_told"),
            choice("evt_p25_prisoner_at_the_gate_s1_c2", "Let the Garrison believe the den broke parley", morale=-2, leads="evt_p25_prisoner_at_the_gate_s2", faction=G, delta=3)])]))

# ── Mid-war context (E-W1..W6): gated on real 06C battle chains ─────────
data["chains"].append(chain(
    "evt_p25_refugees_from_the_line", "war_context", "Refugees From the Line", [G, R],
    "loc_st_brigids_almshouse",
    [stage("evt_p25_refugees_from_the_line_s1", 512,
        "Plan 25 (E-W1): fires once the border clash at Span 44 has run its course and the displaced start walking.",
        produces_flag="flag_war_refugees_arrived",
        title="Walking Wounded From Span 44",
        body="They come up the almshouse road in family clumps, carrying what was nearest when the shelling started: a sewing machine, a dog, a child asleep mid-scream. The almshouse takes the hurt ones. The rest stand in the yard doing arithmetic on the shelter's windows. The battle that made them is three days old and already a place name.",
        choices=[
            choice("evt_p25_refugees_from_the_line_s1_c1", "Open the shelter gate for the worst hurt", morale=2),
            choice("evt_p25_refugees_from_the_line_s1_c2", "Point them to the almshouse and send what medicine the shelter can spare", morale=1),
            choice("evt_p25_refugees_from_the_line_s1_c3", "Keep the gate shut. The shelter cannot feed another war.", morale=-2)])]))

data["chains"].append(chain(
    "evt_p25_requisition", "war_context", "Requisition", [G],
    "loc_garrison_checkpoint_gamma",
    [stage("evt_p25_requisition_s1", 515,
        "Plan 25 (E-W2): fires once conscription lists are posted and the Garrison's paperwork reaches the shelter door.",
        produces_flag="flag_war_requisition_demand",
        title="A Form, in Duplicate, for Everything",
        body="The requisition notice is polite the way a summons is polite: the shelter's fuel stock, half its filter inventory, and a list of souls fit for labor, receipted against a victory the form does not date. The clerk who delivers it waits with a pen. Everyone in the district has signed it or made a problem of themselves, and both lists are shorter than last year's.",
        choices=[
            choice("evt_p25_requisition_s1_c1", "Sign and let the shelter's stores walk to the front", morale=-1, faction=G, delta=4, produces="flag_war_requisition_met"),
            choice("evt_p25_requisition_s1_c2", "Sign the fuel, hide the filters", morale=0, produces="flag_war_requisition_met"),
            choice("evt_p25_requisition_s1_c3", "Refuse, politely, with a copy kept", morale=1, faction=G, delta=-4, produces="flag_war_requisition_refused")])]))

data["chains"].append(chain(
    "evt_p25_broken_route", "war_context", "Broken Route", [R, "faction_forward_roster"],
    "loc_shrine_switchback_waystation",
    [stage("evt_p25_broken_route_s1", 525,
        "Plan 25 (E-W3): fires once the switchback toll chain has played out and the artillery has had its say.",
        title="The Waystation Is a Memory",
        body="The switchback waystation is a crater with a roof propped on it, and the road under it is two ruts and a rumor. Caravans are turning back at the shrine. The Roster's toll-takers have moved their line twice in a week, chasing a crossing that keeps being shelled, and every route the shelter uses now is longer, closer to the line, or both.",
        choices=[
            choice("evt_p25_broken_route_s1_c1", "Pay the Roster's new long-route toll and keep the convoys moving", morale=0),
            choice("evt_p25_broken_route_s1_c2", "Stockpile instead of travel through the line's shadow", morale=1)])]))

data["chains"].append(chain(
    "evt_p25_field_hospital_overflow", "war_context", "Field Hospital Overflow", [G, R],
    "loc_st_brigids_almshouse",
    [stage("evt_p25_field_hospital_overflow_s1", 536,
        "Plan 25 (E-W4): fires once the garrison offensive at the grain silo has resolved and its wounded have found every doorstep.",
        title="Cots in the Nave",
        body="The almshouse has run out of almshouse. Cots stand in the nave, the crypt takes the ones the surgeons lost, and the wounded from the silo offensive keep arriving faster than the dressings. A Garrison medic and a Rebuilders medic work back to back in silence, because bodies do not check uniforms before they bleed. Someone has to decide whose shelter becomes the overflow ward.",
        choices=[
            choice("evt_p25_field_hospital_overflow_s1_c1", "Take the overflow wounded regardless of banner", morale=2, produces="flag_war_shelter_took_wounded"),
            choice("evt_p25_field_hospital_overflow_s1_c2", "Take only the shelter's own banner's hurt", morale=-1),
            choice("evt_p25_field_hospital_overflow_s1_c3", "Send dressings instead of doors", morale=0)])]))

data["chains"].append(chain(
    "evt_p25_deserter_column", "war_context", "Deserter Column", [G, "faction_deserter_coalition"],
    "loc_denial_cut_substation",
    [stage("evt_p25_deserter_column_s1", 548,
        "Plan 25 (E-W5): fires once the ration plaza strike chain has run and the first column of done fighters walks.",
        title="They Left Their Rifles Stacked",
        body="They come down the denial cut in loose file, no banners, rifles stacked and left at the crossroads with a chalk note nobody will claim: enough. Garrison patches under new jackets, Rebuilder boots, one kid carrying another's pack. The Coalition camp's sentries do not raise the wire and do not lower it. Every fighter who joins them is a smaller war, and everyone can count.",
        choices=[
            choice("evt_p25_deserter_column_s1_c1", "Feed the column through and say nothing to anyone", morale=1),
            choice("evt_p25_deserter_column_s1_c2", "Tell the Garrison the column came through, as the form requires", morale=-2, faction=G, delta=3),
            choice("evt_p25_deserter_column_s1_c3", "Offer the shelter's medic for the march-wounds", morale=1)])]))

data["chains"].append(chain(
    "evt_p25_retaliation", "war_context", "Retaliation", [R, "faction_ash_sign"],
    "loc_grain_silo",
    [stage("evt_p25_retaliation_s1", 555,
        "Plan 25 (E-W6): fires once the Rebuilders fractured and the people the fracture discarded started settling scores.",
        title="What the Fracture Left Behind",
        body="The Rebuilders' fracture did not stay political. A crew that chose the wrong side of it came back to the silo district and found their names chalked on doors, and by morning two of those doors were ash. The Ash Sign people are sheltering the chalked families; the Rebuilders' rump says provocateurs did it. Everyone knows the arithmetic of reprisal is still open, and the shelter is where the arithmetic comes to hide.",
        choices=[
            choice("evt_p25_retaliation_s1_c1", "Take the chalked families in, quietly", morale=2, produces="flag_war_sheltered_retaliation_families"),
            choice("evt_p25_retaliation_s1_c2", "Broker the rump's investigate-first promise to the families", morale=1),
            choice("evt_p25_retaliation_s1_c3", "Stay out of another faction's housekeeping", morale=-1)])]))

# ── War weariness (E-R1..R4): culmination, pressure toward the Muster ───
data["chains"].append(chain(
    "evt_p25_no_more_volunteers", "weariness", "No More Volunteers", [G],
    "loc_conscription_office",
    [stage("evt_p25_no_more_volunteers_s1", 568,
        "Plan 25 (E-R1): fires once the hydro leverage break has shown everyone what the next year of war costs.",
        produces_flag="flag_peace_volunteers_dry",
        title="The Ledger of the Willing",
        body="The conscription office has stopped pretending. The volunteer ledger, open on the counter where pride could read it, shows one name in eleven days, and the name is a man of sixty who signed because his son's name is already carved somewhere. The clerk keeps the pen inked anyway. Armies do not run on ledgers like this one, and the officers reading it know what it says better than anyone.",
        choices=[
            choice("evt_p25_no_more_volunteers_s1_c1", "Leave the ledger open where the street can read it", morale=1),
            choice("evt_p25_no_more_volunteers_s1_c2", "Ask the clerk to close the office early, before the officers notice", morale=0)])]))

data["chains"].append(chain(
    "evt_p25_bread_before_bullets", "weariness", "Bread Before Bullets", [R, G],
    "loc_grain_silo",
    [stage("evt_p25_bread_before_bullets_s1", 572,
        "Plan 25 (E-R2): fires while the refugees from the line are still on the district's conscience and the silo granaries are on its mind.",
        requires_flag="flag_war_refugees_arrived",
        produces_flag="flag_peace_bread_before_bullets",
        title="A Queue That Forgot the War",
        body="The grain queue outside the Exchange is the longest in district memory, and somewhere in its third hour the queue stopped being about grain. Women are comparing ration books across banners. A Garrison wife is holding a Rebuilder's baby. Somebody starts singing something old and unemployed, and the queue picks it up, and for eleven minutes there is no war in it at all. Then the whistles, and the tarpaulins, and the queues remember their manners.",
        choices=[
            choice("evt_p25_bread_before_bullets_s1_c1", "Send the shelter's kettle and whatever boils in it", morale=2),
            choice("evt_p25_bread_before_bullets_s1_c2", "Watch from the Exchange steps and remember the eleven minutes", morale=1)])]))

data["chains"].append(chain(
    "evt_p25_quiet_faction", "weariness", "The Quiet Faction", [R, G],
    "loc_forward_roster_camp",
    [stage("evt_p25_quiet_faction_s1", 578,
        "Plan 25 (E-R3): fires once the shrine strike has made the offensive's cost plain to the people paying it.",
        produces_flag="flag_peace_faction_forms",
        title="A List That Passes Hand to Hand",
        body="It starts as a tea circle and becomes something with rules. Mothers of the silo garrison, Roster widows, a couple of Rebuilders' quartermasters who have seen the stores. They call themselves nothing. They meet when the shelling pauses, and they pass a list from hand to hand: names of the living on one side, names they do not intend to add on the other. The list asks, in careful pencil, for a gathering. Not a surrender. A gathering.",
        choices=[
            choice("evt_p25_quiet_faction_s1_c1", "Carry the list one camp further, to the Coalition", morale=2),
            choice("evt_p25_quiet_faction_s1_c2", "Copy the list into the shelter's own book first", morale=1),
            choice("evt_p25_quiet_faction_s1_c3", "Refuse the list - paper like that gets people chalked", morale=-1)])]))

data["chains"].append(chain(
    "evt_p25_refusal_at_dawn", "weariness", "Refusal at Dawn", [G],
    "loc_railway_span_44_alpha",
    [stage("evt_p25_refusal_at_dawn_s1", 584,
        "Plan 25 (E-R4): fires while the quiet faction's list circulates and before the ceasefire anyone could name.",
        requires_flag="flag_peace_faction_forms",
        produces_flag="flag_peace_refusal_at_dawn",
        title="The Squad That Sits Down",
        body="At first light, at Span 44, the offensive's Fresh battalion does not form up. A sergeant reads the names of the last battalion out loud, all of them, and then sits on the rail. One by one, so does the squad. The officers have the numbers to make it an atrocity and the sense to know the escorts would not fire. By noon it is not a mutiny; it is a fact: this battalion will not march again. The war does not end here. But everyone at the span has heard the sound a war makes when it starts running out.",
        choices=[
            choice("evt_p25_refusal_at_dawn_s1_c1", "Bring water to the sitting squad, and carry word of it", morale=2),
            choice("evt_p25_refusal_at_dawn_s1_c2", "Keep the shelter's name out of it entirely", morale=0)])]))

json.dump(data, open(path, 'w'), indent=2, ensure_ascii=False)
print("chains now:", len(data["chains"]))
print("new:", [c["chainId"] for c in data["chains"] if c["chainId"].startswith("evt_p25")])
