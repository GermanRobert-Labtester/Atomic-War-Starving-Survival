import json
path = 'Assets/StreamingAssets/Data/muster_faction_actions.json'
data = json.load(open(path))

def action(id, fid, title, text, min_day, variants, cooldown=0):
    return {"id": id, "faction_id": fid, "title": title, "text": text,
            "min_day": min_day, "max_day": 0, "once": False, "cooldown_days": cooldown,
            "requires_flags": [], "forbids_flags": [], "variants": variants}

def variant(band, text, choices):
    return {"band": band, "text": text, "choices": choices}

def choice(cid, text, trust=0, agg=0, members=0, lockout=0, flags=None, journal=""):
    return {"choice_id": cid, "text": text,
            "effects": {"trust_delta": trust, "aggression_delta": agg, "members_delta": members,
                        "lockout_delta": lockout, "item_id": "", "item_amount": 0,
                        "flags": flags or [], "journal": journal}}

mediation_variants = [
    variant("neutral",
        "Two caravans have claimed the same collapsed depot, and both threatened to arm before the week is out. The camp's mediators cannot be the arbiter - they house both. A caller comes up the slope with the camp's ask: lend the shelter's name and its record for fair dealing, and sit one afternoon of listening. The camp cannot pay. The camp can only remember.",
        [choice("sit_mediator", "Lend the shelter's name and sit the mediation.", lockout=-5,
                flags=["flag_favor_coalition_mediation_served"],
                journal="The shelter served as neutral sign in a Coalition Camp mediation."),
         choice("send_goods_peace", "Send goods as an offset instead of a person.", lockout=-3,
                journal="The shelter offset a camp dispute with goods instead of a mediator."),
         choice("decline", "Decline. The shelter is not neutral enough to pretend.", lockout=3,
                flags=["flag_grievance_coalition_mediation_refused"],
                journal="The shelter declined the Coalition Camp's mediation request.")]),
    variant("good",
        "The ask comes with the camp's own flag folded into the messenger's coat - the standing mark that the shelter's word holds on camp ground. Sitting the mediation under that mark costs the shelter an afternoon and buys it something rarer: a debt the camp keeps in its founding custom, written where the rules are burned into the plank.",
        [choice("sit_mediator", "Sit the mediation under the camp's mark.", lockout=-8,
                flags=["flag_favor_coalition_mediation_served"],
                journal="The shelter mediated under the Coalition's standing mark."),
         choice("decline", "Decline even the honored ask.", lockout=5,
                flags=["flag_grievance_coalition_mediation_refused"],
                journal="The shelter declined a honored Coalition mediation request.")]),
    variant("hostile",
        "The messenger will not come inside the wire. The camp's security detail has been counting the shelter's faces since the informant story made the rounds, and the ask for a mediator is thinner this time - more test than trust. Sitting it would mean working a crowd that has already decided what the shelter is.",
        [choice("sit_mediator_anyway", "Sit the mediation and take the crowd's suspicion.", lockout=-3,
                journal="The shelter mediated at the camp despite hostile suspicion."),
         choice("decline", "Decline, and say why: the mark has to mean something.", lockout=8,
                flags=["flag_grievance_coalition_mediation_refused"],
                journal="The shelter declined mediation while the camp counted its faces.")]),
]

supply_variants = [
    variant("neutral",
        "The camp's ledger of what it owes is short and what it holds is shorter. The mess tent is down to stretching one pot across four banners, and winter does not care how temporary a political invention is. The ask is plain: whatever the shelter can spare, logged openly, owed publicly.",
        [choice("give_supply", "Send what the shelter can spare, logged openly.", lockout=-5,
                flags=["flag_favor_coalition_supply_shared"],
                journal="The shelter answered the camp's shared-supply appeal."),
         choice("promise_later", "Promise a share after the next resupply.", lockout=0,
                journal="The shelter promised the camp a supply share after resupply."),
         choice("refuse", "Refuse. The shelter's margin is its own.", lockout=5,
                flags=["flag_grievance_coalition_supply_refused"],
                journal="The shelter refused the camp's shared-supply appeal.")]),
    variant("allied",
        "The appeal is barely an appeal anymore - it is a supply schedule between allies, with the shelter's name on the camp's mess roster and the camp's wounded carried toward the shelter's door. What is asked for is the next month's share, and what is offered back is written into the neutral-ground rules themselves.",
        [choice("give_supply", "Commit the month's share to the mess roster.", lockout=-8, members=2,
                flags=["flag_favor_coalition_supply_shared"],
                journal="The shelter committed a month's share to the camp mess; two wounded came in with it."),
         choice("give_and_join", "Commit the share and take a seat at the camp's own fire.", lockout=-8, members=3,
                flags=["flag_favor_coalition_supply_shared"],
                journal="The shelter shared the month's supply and was given a place at the camp fire.")]),
]

division_variants = [
    variant("neutral",
        "The camp's founding custom says every banner gets a voice, and the camp has discovered what that costs: the mediators want the witness rules tightened before someone's testimony gets someone killed, and the security crew wants the wire moved first, because dead neutrals need no rules. Both sides have asked the shelter - the only outside party both still talk to - which comes first.",
        [choice("back_mediators", "Back the mediators: rules before wire.", lockout=-5,
                flags=["flag_favor_coalition_rules_first"],
                journal="The shelter backed the camp mediators: rules before wire."),
         choice("back_security", "Back the security crew: wire before rules.", lockout=-10,
                flags=["flag_grievance_coalition_security_backed"],
                journal="The shelter backed the camp security crew: wire before rules."),
         choice("refuse_choice", "Tell them the order is theirs to choose.", lockout=0,
                journal="The shelter left the camp's rules-versus-wire choice to the camp.")]),
]

data["actions"].append(action(
    "act_coalition_mediation_request", "faction_deserter_coalition", "The Mediation Request",
    "The Coalition Camp exists because exhausted people needed somewhere to sit near their enemies, and it stays alive because somebody settles the arguments small enough to kill a truce. Today's argument is a depot, two caravans, and a deadline.",
    210, mediation_variants, cooldown=25))

data["actions"].append(action(
    "act_coalition_supply_appeal", "faction_deserter_coalition", "One Pot, Four Banners",
    "The shared meal is the camp's oldest custom and its most fragile: no banner eats while another banner watches, and the pot holds only what people put in it. The camp's quartermaster keeps the rosters honest. She has run out of honest things to put in the pot.",
    220, supply_variants, cooldown=25))

data["actions"].append(action(
    "act_camp_rules_dispute", "faction_deserter_coalition", "Rules Before Wire",
    "The camp is not one voice either. Its mediators and its security crew agree the camp must survive; they disagree on what surviving costs, and the disagreement has stopped being polite. The shelter, as the outside party both still deal with, has been asked to weigh in.",
    230, division_variants))

json.dump(data, open(path, 'w'), indent=2, ensure_ascii=False)
print("actions now:", len(data["actions"]), [a["id"] for a in data["actions"]][-3:])
