import json
path = 'Assets/StreamingAssets/Data/muster_faction_actions.json'
data = json.load(open(path))

def action(id, fid, title, text, min_day, variants, cooldown=0):
    return {"id": id, "faction_id": fid, "title": title, "text": text,
            "min_day": min_day, "max_day": 0, "once": False, "cooldown_days": cooldown,
            "requires_flags": [], "forbids_flags": [], "variants": variants}

def variant(band, text, choices):
    return {"band": band, "text": text, "choices": choices}

def choice(cid, text, trust=0, agg=0, flags=None, journal=""):
    return {"choice_id": cid, "text": text,
            "effects": {"trust_delta": trust, "aggression_delta": agg, "members_delta": 0,
                        "lockout_delta": 0, "item_id": "", "item_amount": 0,
                        "flags": flags or [], "journal": journal}}

parley_variants = [
    variant("hostile",
        "The emissary's crew watches from the tree line and she talks fast, like the parley is a thing that can be lost. The Toll's den is hungry this season; the list the shelter is on has a date next to it. Terms will be sharp and the guarantee is only her own skin.",
        [choice("honor_terms", "Take the sharp terms and stand by them.", agg=-0.15,
                flags=["flag_favor_raider_parley_honored"],
                journal="The shelter honored a raider parley on sharp terms."),
         choice("refuse", "Send her back with nothing.", agg=0.1,
                flags=["flag_grievance_raider_parley_broken"],
                journal="The shelter refused a raider parley.")]),
    variant("neutral",
        "Talk goes on for an hour, mostly about what the code does not allow: no burning stores, no touched wells, surrendered people walked to a road and released. The emissary does not dress up what the Toll is. She wants the shelter as a customer, not a friend, and the code she keeps is the only guarantee on the table.",
        [choice("honor_terms", "Agree to the exchange and keep it.", agg=-0.2,
                flags=["flag_favor_raider_parley_honored"],
                journal="The shelter kept a raider parley: scrap first-offer for the seasonal list."),
         choice("counter_terms", "Counter with a fixed term - one season, then renew.", agg=-0.1,
                journal="The shelter countered a raider parley with a one-season term."),
         choice("refuse", "Refuse. The shelter does not deal.", agg=0.05,
                journal="The shelter refused a raider parley without insult.")]),
    variant("allied",
        "The emissary leaves the weapon on the table, which among the Toll means something like trust. The den remembers the shelter kept the last bargain: routes that cross Toll ground pass with a nod, and the seasonal list is a formality the list-keepers skip. What she asks now is small - a debt settled between crews, not strangers.",
        [choice("honor_terms", "Renew the standing exchange, witnessed by both crews.", agg=-0.2,
                flags=["flag_favor_raider_parley_honored"],
                journal="The shelter renewed its standing parley with the Toll's den."),
         choice("broker_internal", "Broker between the den's hardliners and pragmatists.", agg=-0.25,
                flags=["flag_favor_raider_parley_honored"],
                journal="The shelter brokered between the Toll's hardliners and pragmatists.")]),
]

levy_variants = [
    variant("hostile",
        "The levy party has a captured truck behind it and men on it who look like the truck was captured recently. Rates double for shelters the den doesn't like, and the shelter is not liked. They will take the share or they will take the next convoy, and everyone at the crossing knows which is cheaper.",
        [choice("pay_levy", "Pay the doubled levy in goods.", agg=-0.1,
                journal="The shelter paid the Toll's doubled passage levy."),
         choice("run_convoy", "Run the crossing at night and take the risk.", agg=0.15,
                flags=["flag_grievance_raider_passage_evaded"],
                journal="The shelter ran the Toll crossing at night instead of paying."),
         choice("fight", "Refuse with force in the open.", agg=0.25,
                flags=["flag_grievance_raider_passage_fought"],
                journal="The shelter fought the Toll's levy party at the crossing.")]),
    variant("neutral",
        "The levy is a share in ten, weighed honestly, receipted in chalk on the convoy's own manifest. The party chief is bored and professional about it. Parley partners get the same rate as anyone; the difference is the den actually keeps its hands off partners' next crossing.",
        [choice("pay_levy", "Pay the honest tenth and keep the manifest chalk.", agg=-0.1,
                journal="The shelter paid the Toll's passage levy, receipted in chalk."),
         choice("negotiate_mark", "Buy the Toll's mark instead - a season of crossings.", agg=-0.2,
                journal="The shelter bought a season's passage under the Toll's mark."),
         choice("divert", "Divert the route and lose the days instead.", agg=0.0,
                journal="The shelter diverted its route around the Toll crossing.")]),
]

code_variants = [
    variant("neutral",
        "The old chief makes his case plainly: dead men keep no code, and the den ate from garbage two winters ago because mercy was on the wrong ledger. The emissary answers with the list of wells the Toll never touched and the crews that surrendered and walked away. The shelter's word will not settle the den. It will tip it.",
        [choice("back_code_holders", "Stand with the emissaries: the code holds.", agg=-0.15,
                flags=["flag_favor_raider_parley_honored"],
                journal="The shelter stood with the Toll's emissaries to hold the raider code."),
         choice("back_hardliners", "Agree with the old chief: winters like this spare nobody.", agg=0.2,
                flags=["flag_grievance_raider_code_widened"],
                journal="The shelter sided with the Toll's hardliners to widen the raider code."),
         choice("say_nothing", "Trade, and say nothing about another crew's law.", agg=0.0,
                journal="The shelter stayed out of the Toll's code dispute.")]),
]

data["actions"].append(action(
    "act_raider_parley", "faction_iron_raiders", "Parley Under Their Code",
    "A raider emissary sits down out of reach of the window and puts a weapon on the table, muzzle toward herself: the Toll's sign for talk. The code is simple and it is real - a parley opened honestly holds while the parties keep it, and the man who breaks one answers to his own crew. What she wants is a standing exchange: the shelter's scrap metal offered first to the den, in return for the shelter's name moving off the seasonal list.",
    90, parley_variants))

data["actions"].append(action(
    "act_raider_passage_levy", "faction_iron_raiders", "The Passage Levy",
    "The route the shelter's expeditions use crosses ground the Toll treats as theirs, and a levy party is waiting at the crossing with a hand-painted rate: one share in ten of what crosses, or the crossing stops being safe for anyone who doesn't fly their mark. They are not hiding that this is a toll, and they are not pretending it is a tax. It is a price for a road.",
    200, levy_variants, cooldown=20))

data["actions"].append(action(
    "act_raider_code_dispute", "faction_iron_raiders", "The Toll's Own Argument",
    "The den is not one voice either. An old crew chief named for the bridge he burned wants the code widened - stores, wells, medicine, nothing spared, because the war taught him what mercy costs. The emissaries and the young hands want the code held exactly because the war taught them the same. The dispute has come to the shelter's door because both sides trade there, and both sides asked.",
    120, code_variants))

json.dump(data, open(path, 'w'), indent=2, ensure_ascii=False)
print("actions now:", len(data["actions"]), [a["id"] for a in data["actions"]][-3:])
