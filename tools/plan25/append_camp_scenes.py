import json
path = 'Assets/StreamingAssets/Data/muster_camp_scenes.json'
data = json.load(open(path))

def scene(sid, name, min_day, variants):
    return {"id": sid, "scene": name, "min_day": min_day, "requires_flags": [], "variants": variants}

def v(vid, body, path_req="", any_flags=None, all_flags=None):
    t = {"variant_id": vid, "body": body}
    if path_req: t["requires_path"] = path_req
    if any_flags: t["requires_flags"] = any_flags
    if all_flags: t["requires_all_flags"] = all_flags
    return t

data["scenes"].extend([
    scene("camp_scene_old_enemies", "old_enemies", 260, [
        v("blood_price", "They meet at the wire and neither pretends. The Coalition men who stayed know the shelter's messenger sold their ground to the Garrison for a resupply; the shelter's people know they know. Nobody raises a voice. The Toll's detachment watches from the ridge with the professional interest of people who keep books on every grudge at this fire. What passes between the camp and the shelter is a nod that is not a peace, and the camp's ledger - kept honestly, the way the Toll taught the whole district to fear - gains a line.",
            path_req="", all_flags=["flag_escalation_prisoner_truth_told"]),
        v("unpaid_toll", "The Hydro water-detail and the shelter's convoy meet at the trough, and the trough is where the account stands. Payments missed, appeals refused, a season of chalk receipts between them. The Barons' man does not threaten; he counts, out loud, slowly, the way you count in front of witnesses. The shelter's people count along. That is the whole conversation, and it is not a small one.",
            any_flags=["flag_grievance_hydro_toll_defaulted", "flag_grievance_hydro_appeal_refused"]),
        v("crossing_grudge", "The levy chief and the shelter's expedition lead stand either side of the camp's fire ring, both publicly unarmed, both remembering a crossing that went wrong in the old way. Around the Toll's people that memory is called an account; around the camp it is called a grievance; the difference is that here, for three days, nobody can collect either.",
            any_flags=["flag_grievance_raider_passage_evaded", "flag_grievance_raider_passage_fought"]),
        v("negotiated_quiet", "Old enemies arrange themselves by distance. A Garrison sergeant and a Rebuilders' scout share a map at arm's length, trading schoolroom decency. The camp's mediators float with tea nobody drinks. Everyone has been told the rules, and the remarkable thing - the thing the camp exists for - is that on this ground the rules are holding."),
        v("victors_order", "They meet in the order the victor permits: defeated detachments first, disarmed, logged; the dominant banner's people last, armed, unhurried. An old Rebuilder looks at the checkpoint roster and finds the shelter's name on the wrong column. He says nothing. On this ground even silence has a column."),
    ]),
    scene("camp_scene_shared_meal", "shared_meal", 261, [
        v("peace_in_the_pot", "The quiet faction's list made it to the pot: bread from the Rebuilders' ovens, a Garrison cookhouse kettle carried three miles by women who would not say which side they buried. The shelter's contribution goes in without a banner on it, and nobody asks. For eleven minutes, again, there is no war in it. The camp's quartermaster keeps the roster open anyway. Rosters outlast songs.",
            any_flags=["flag_peace_bread_before_bullets", "flag_peace_refusal_at_dawn"]),
        v("war_rations", "The meal is field bread and lentils cooked at the camp because nobody's home is safe to cook in. Refugees from the line eat first - camp custom, not law - and the shelter's people serve the line. A Garrison conscript's mother thanks a Rebuilders' scout for the water. The scout says it was the shelter's water, and the mother looks over, and nods once. It is not forgiveness. It is accounting, done gently for once.",
            all_flags=["flag_war_refugees_arrived"]),
        v("thin_pot", "The pot is thin and everyone can see the bottom. The shelter's share is noted in the roster with the date and the words 'asked, not given' written plainly, because the camp's honesty is its only weapon. The meal happens anyway. Nobody eats last. That is the custom, and customs are all the camp has.",
            all_flags=["flag_grievance_coalition_supply_refused"]),
        v("negotiated_pot", "One pot, four banners, and the serving order negotiated down to the ladle: wounded first, children second, elders third, everyone fourth. The shelter eats in the fourths and pays in the firsts, and the quartermaster marks the shelter's name beside two words the camp does not spend lightly: kept faith."),
        v("victors_rations", "The victor's quartermaster allots the pot, and the portions say what the flags at the gate were too polite to: full bowls inside the wire's favored half, thin ones outside it. The shelter is offered the favored half and the seat that goes with it. The seat is watching."),
    ]),
    scene("camp_scene_confrontation", "confrontation", 263, [
        v("well_poisoning", "It comes to a head over water, the way it always does. The bitter-water families stand at the mediation fire with the Barons' flow logs and the shelter's published test results, and someone says the word revenge in a voice that is not loud. The camp's mediators work the crowd like people bailing a boat. Whatever the shelter says next, half the camp will believe it forever.",
            any_flags=["flag_escalation_bitter_water", "flag_escalation_bitter_water_investigated", "flag_escalation_cistern_blockade", "flag_escalation_cistern_published"]),
        v("chalked_doors", "A family the shelter took in after the silo doors were chalked is recognized at the meal line by a man from the other faction, and the camp gets very quiet, the way a room gets quiet before furniture breaks. The mediators put themselves in the middle, which is their whole job and their whole authority. The shelter's name is in the middle with them.",
            all_flags=["flag_war_sheltered_retaliation_families"]),
        v("requisition_row", "The confrontation arrives as paperwork, because this camp was founded by people who had seen what fists decide. A veteran of the requisition stands with his copy of the shelter's signed form and reads it aloud at the fire - fuel, filters, names for the labor lists - and asks the camp's rules to say whether a signature given to a war counts as a gift or a theft. The camp has no rule for it yet. It is about to write one.",
            any_flags=["flag_war_requisition_met", "flag_war_requisition_demand"]),
        v("negotiated_fire", "The confrontation is small and survivable: a woman accuses, a man denies, the mediators separate them into two tents and the camp exhales. The shelter is asked to sit the hearing, because its name still means fair to people who have run out of other words for it."),
        v("victors_verdict", "There is no hearing. The victor's security detail decides the dispute in nine minutes and posts the verdict at the gate, and the camp reads it in silence and throws its potato peels in the correct trench. Justice here is fast, and the speed is the message. The shelter is asked - quietly, by people who do not use that word anymore - to remember this when the verdicts start having names attached."),
    ]),
])

json.dump(data, open(path, 'w'), indent=2, ensure_ascii=False)
print("scenes now:", len(data["scenes"]), [s["id"] for s in data["scenes"]])
