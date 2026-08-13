#!/usr/bin/env python3
"""Generate Holdfast StreamingAssets JSON from the creative pack. Run from repo root."""
from __future__ import annotations

import json
import pathlib
import re

ROOT = pathlib.Path(__file__).resolve().parents[1]
PACK = ROOT / "docs/expansions/expansion_the_holdfast_creative_pack.md"
DATA = ROOT / "Assets/StreamingAssets/Data"

STATS = {
    "loc_ice_road_gate": ("the_cut", 5, 6.0, 28),
    "loc_cut_kilometre_19": ("the_cut", 5, 6.5, 30),
    "loc_cut_weigh_hut": ("the_cut", 5, 7.0, 26),
    "loc_cut_dredger_hulk": ("the_cut", 6, 7.5, 38),
    "loc_cut_brine_pool": ("the_cut", 7, 7.5, 44),
    "loc_cut_waystation_a": ("the_cut", 5, 8.0, 24),
    "loc_cut_accident_12": ("the_cut", 7, 8.5, 36),
    "loc_cut_south_beacon": ("the_cut", 6, 8.0, 32),
    "location_abandoned_desalination": ("the_saltworks", 7, 8.5, 40),
    "loc_salt_membrane_hall": ("the_saltworks", 7, 9.0, 40),
    "loc_salt_intake_caisson": ("the_saltworks", 8, 9.5, 52),
    "loc_salt_iodine_store": ("the_saltworks", 6, 8.5, 28),
    "loc_salt_outfall": ("the_saltworks", 7, 9.0, 48),
    "loc_salt_grade_hut": ("the_saltworks", 5, 8.0, 22),
    "loc_salt_cooling_canal": ("the_saltworks", 6, 8.5, 34),
    "loc_salt_scrap_membranes": ("the_saltworks", 6, 9.0, 42),
    "loc_cluster_gatehouse": ("the_cluster", 4, 8.5, 18),
    "loc_cluster_quad": ("the_cluster", 4, 8.5, 16),
    "loc_cluster_block_c": ("the_cluster", 5, 9.0, 20),
    "loc_cluster_clinic": ("the_cluster", 5, 9.0, 22),
    "loc_cluster_school": ("the_cluster", 4, 8.5, 16),
    "loc_cluster_office": ("the_cluster", 5, 9.0, 18),
    "loc_cluster_steam_substation": ("the_cluster", 6, 9.5, 28),
    "location_frozen_river_barge": ("the_shelf", 6, 10.0, 30),
    "location_crashed_icebreaker_convoy": ("the_shelf", 7, 11.0, 85),
    "loc_shelf_hearth4": ("the_shelf", 8, 12.0, 40),
    "loc_shelf_roadstead_crane": ("the_shelf", 7, 11.5, 36),
    "loc_shelf_pressure_ridge": ("the_shelf", 8, 12.5, 44),
    "loc_shelf_foghorn": ("the_shelf", 6, 10.5, 32),
}

RECAST_ALWAYS = {
    "location_abandoned_desalination",
    "location_frozen_river_barge",
    "location_crashed_icebreaker_convoy",
}
OVERLAY = {
    "loc_the_shallows_market",
    "loc_weighbridge",
    "loc_toll_house",
    "location_ministry_of_truth_bunker",
    "loc_the_allotments",
    "loc_low_background_lab",
}

DISPLAY_OVERRIDE = {
    "location_abandoned_desalination": "Municipal Desalination 8",
    "location_crashed_icebreaker_convoy": "Icebreaker Convoy",
}


def parse_location_cards(text: str) -> list[dict]:
    loc_section = text.split("# 2. NPC voice bibles")[0]
    pattern = re.compile(
        r"### `([^`]+)` — ([^\n]+)\n\n\*\*inspect:\*\* ([^\n]+)\n\n\*\*description:\*\*  \n(.+?)(?=\n### |\n---\n|\n# 2\. )",
        re.S,
    )
    out = []
    for m in pattern.finditer(loc_section):
        loc_id = m.group(1)
        name = re.sub(r" \*\(.*\)$", "", m.group(2)).strip()
        name = name.replace("*", "")
        inspect = m.group(3).strip()
        desc = " ".join(m.group(4).strip().split())
        region, danger, hours, rads = STATS.get(loc_id, ("the_cut", 5, 8.0, 28))
        if loc_id in OVERLAY:
            region = "sector_4_overlay"
            danger, hours, rads = 0, 0, 0
        entry = {
            "id": loc_id,
            "displayName": DISPLAY_OVERRIDE.get(loc_id, name),
            "inspect": inspect,
            "description": desc,
            "dangerLevel": float(danger) if danger else 0.0,
            "travelHours": float(hours) if hours else 0.0,
            "baseRadsPerHour": float(rads) if rads else 0.0,
            "region": region,
            "overlay_on_unlock": loc_id in OVERLAY,
            "recast_always": loc_id in RECAST_ALWAYS,
        }
        out.append(entry)
    return out


def dump(path: pathlib.Path, obj) -> None:
    path.write_text(json.dumps(obj, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(f"wrote {path.relative_to(ROOT)} ({len(obj) if isinstance(obj, list) else 'obj'} entries)")


def main() -> None:
    pack = PACK.read_text(encoding="utf-8")
    locations = parse_location_cards(pack)
    ids = [e["id"] for e in locations]
    assert "loc_ice_road_gate" in ids
    assert "loc_cut_kilometre_19" in ids
    assert "loc_cut_waystation_a" in ids
    dump(DATA / "holdfast_locations.json", locations)

    factions = [
        {
            "id": "faction_the_office",
            "display_name": "The Office",
            "alignment": "conditional",
            "home_region": "the_cluster",
            "is_active": True,
            "trust": 0,
            "wants": ["item_census_return_blank", "named_occupancy", "item_order_12c"],
            "offers": ["process_water_credit", "block_c_guesting", "regular_rate"],
            "signature_quote": "I am not collecting you. I am scheduling you.",
            "access_rule": "Access plus named claims. You cannot conquer them. You can lose the Ice Road and gain a file. Threat is tone, not a seventh Power.",
            "badge_asset_id": "",
        },
        {
            "id": "faction_the_cutters",
            "display_name": "The Cutters",
            "alignment": "conditional",
            "home_region": "the_cut",
            "is_active": True,
            "trust": 0,
            "wants": ["item_beacon_oil", "item_ice_spike_bar", "calories"],
            "offers": ["lit_window", "accident_book", "waystation_overnight"],
            "signature_quote": "I don't open it for you. I open it. If it's dark, you wait.",
            "access_rule": "Dark and lit are moral words. Will not guide a column onto ice marked dark. Will not blast. Relight-for-a-trap is Ivy's exception, northern: lamps out over eleven days, access gone.",
            "badge_asset_id": "",
        },
        {
            "id": "faction_the_fleet",
            "display_name": "The Fleet",
            "alignment": "peaceful",
            "home_region": "the_shelf",
            "is_active": False,
            "trust": 0,
            "wants": ["stand_up_order", "allocation_number", "item_fleet_pad_copy"],
            "offers": ["hearth4_hatch", "roadstead_lift", "schedule_crystal"],
            "signature_quote": "The authenticator wanted a number. A number was found, or a person was asked ashore.",
            "access_rule": "Waiting for a stand-up that uses the same authentication family as a land pad. Some paper only works on land. Blasting is refused. The Ridge is hours and cold.",
            "badge_asset_id": "",
        },
    ]
    dump(DATA / "holdfast_factions.json", factions)

    def stages(*texts):
        return [{"id": f"stage_{i+1}", "text": t} for i, t in enumerate(texts)]

    def choices(*pairs):
        out = []
        for p in pairs:
            if len(p) == 3:
                cid, text, flag = p
            else:
                cid, text = p
                flag = ""
            out.append({"id": cid, "text": text, "set_flag": flag})
        return out

    quests = [
        {
            "id": "quest_holdfast_the_sheet",
            "display_name": "The Sheet That Shouldn't",
            "type": "expedition",
            "briefing": "Bram Ostrowski sells you a waxed sheet of the estuary. A road is drawn where summer water should be. He will not say who walked it. He will take calories or a favour-by-mass. The sheet smells of lamp oil and fish glue.",
            "prereq_quest_id": "",
            "min_day": 90,
            "knowledge_key": "lore_hf_sheet",
            "target_location_id": "loc_ice_road_gate",
            "stages": stages(
                "Bought / copied item_map_sheet_ice_road. The wax takes a fingerprint and keeps it. Channel markers on the sheet do not match the last Sector 4 lamp you know.",
                "Compared to Kittiwake log (if owned). The launch's hand is eleven days past the Exchange. Ostrowski's hand is this year. The road exists on only one of them. The ice has not been asked.",
                "Asked a Lamplighter about Kilometre 19. Ivy confirms the post. She does not confirm the road. She does not cross.",
                "Survived the asking. No exception was requested. The ledger still shows 19 lit. You have a fragment and a direction that is only a direction in winter.",
            ),
            "choices": choices(
                ("pay_bram", "He wraps the sheet in the same paper he uses for maps that exist in all seasons. He does not wish you luck. He wishes you a freeze."),
                ("copy_leave", "Your copy smears at Kilometre 19. His does not. He notices. He does not raise the price. He files you as a person who copies."),
                ("refuse_sale", "He puts the sheet away. The estuary remains a rumour with a smell. Ivy will still light 19. Yara will still open a road you cannot prove."),
            ),
        },
        {
            "id": "quest_holdfast_the_clerk",
            "display_name": "The Return",
            "type": "dialogue",
            "briefing": "A man stands off the weighbridge plate so he will not be charged as cargo. Census Clerk Grade III Edor Vale. A return in triplicate. Three of your people are already written, occupations wrong by one each. He offers to read it twice. The Tollman charges for the introduction.",
            "prereq_quest_id": "quest_holdfast_the_sheet",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_weighbridge",
            "stages": stages(
                "Heard the form. Heading, occupancy, reconstruction pool, ice window. He did not skip a field. You may ask him to start again.",
                "Confirmed or denied three occupations. Adjacent errors. Mason / caretaker. Clerk / clerk-grade. Vet assistant / vet. He corrects in the same hand. He thanks you for the truth without using the word.",
                "Hatch wait: allowed or refused. If allowed, he keeps off the step with a stove-tin. If refused, he goes as far as the boom.",
                "Sela's card (optional). He copies the number. He does not take the laminate. UNCONFIRMED until someone who can confirm, confirms.",
            ),
            "choices": choices(
                ("let_wait_hatch", "He sleeps in the ash with the tin. Survivors who cannot sleep will knock the tin, not the hatch. In the morning the pink copy has dew in the folds.", "holdfast_edor_wait_hatch"),
                ("send_to_boom", "He goes. He does not call it a refusal. Ormund will. The weighbridge keeps his stool for him."),
                ("show_the_card", "He writes the number as if it were a date of birth: once, slowly. He does not look at Sela as a line item while she is in the room. He looks after."),
            ),
        },
        {
            "id": "quest_holdfast_the_window",
            "display_name": "When the Cut Takes",
            "type": "expedition",
            "briefing": "Yara Holm at the Gate. The boom is up for a freeze that has a length. Fourteen days this window. Outfit three: warmth, iodine, food, welders' glass. Lit hours on the board. Dark ice is not a metaphor.",
            "prereq_quest_id": "quest_holdfast_the_clerk",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_ice_road_gate",
            "stages": stages(
                "Column kitted. Glass on faces. Iodine in a tin that will freeze shut if you leave it in a pocket. The axle ledger takes your mass.",
                "Waystation A reached. Four bunks. Filter notches. Stove regulator in place. Home still ticking.",
                "Dark ice not walked (or walked). If you waited: she marks you rare. If you did not: remarks column, and she does not fetch.",
                "Returned or wintered A4. The last bunk is wettest. The window does not care which you chose. The ice will.",
            ),
            "choices": choices(
                ("wait_dark", "Hours and cold. The beacon stays honest. Yara does not thank you. She puts you on the overnight."),
                ("walk_marked_dark", "The ice is thick. Thick is not lit. An accident is written whether or not you are in it."),
                ("winter_a4", "You spend the closed window on A4. Filter at eleven. Do not file a twelfth notch. Home degrades without you."),
            ),
        },
        {
            "id": "quest_holdfast_the_plant",
            "display_name": "In Situ Essential",
            "type": "expedition",
            "briefing": "The desalination plant is not abandoned. It is staffed, failing, named. Leva Quist's minutes are current. Steam visible toward a place with numbers. Resin is a gift or an insult depending on how you set it down.",
            "prereq_quest_id": "quest_holdfast_the_window",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "location_abandoned_desalination",
            "stages": stages(
                "Grade Hut entered. Volume 12 open. Motion: that we keep running. Carried. A failed valve-seat for a paperweight.",
                "Membrane Hall toured. Fume at chest height. Tuesday's difference on the drum rims. The dipper stays on the nail unless you steal it.",
                "Resin: delivered or refused. Delivered: she counts aloud. Refused: she counts anyway. The count is short.",
                "Steam line seen. Canal wheel painted red-white-red. Cluster on the far end of a pipe. Not a rumour.",
            ),
            "choices": choices(
                ("gift_resin", "She does not say thank you. She marks a drum. Salt trade opens like a valve, which is to say: slowly, with a gauge."),
                ("refuse_gift", "She needed it anyway. You are a visitor who watched. Watching is not a shift."),
                ("ask_abandoned", "That was your word. Ours is Municipal Desalination 8. The stop order didn't come. We didn't stop."),
            ),
        },
        {
            "id": "quest_holdfast_authentication",
            "display_name": "Take a Number",
            "type": "exploration",
            "briefing": "Cluster Gatehouse. Cream paint. A keypad under plastic. They will ask for an Allocation number. Twelve is a known discrepancy. They have a procedure. Guest housing is Block C. The Quad has chains and no seats.",
            "prereq_quest_id": "quest_holdfast_the_plant",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_cluster_gatehouse",
            "stages": stages(
                "Number stated, or none. Twelve opens a file. None opens a different tab of the same binder. Both are procedures.",
                "Block C accepted, or Gatehouse floor. Metal plates versus paper tags versus tiles. Steam or no steam.",
                "Quad walked. Four cultivars, two yellow. Missing-persons strip of Sector 4 trades. One of them is yours.",
                "Playground brass: left or taken. Silent. The chain hangs. The Office notes mass if mass leaves.",
            ),
            "choices": choices(
                ("give_twelve", "The bell does not ring. A clerk says known discrepancy as if it were a weather. Badge: GUEST / BLOCK C or UNSCHEDULED. Both print on paper."),
                ("give_none", "They do not invent a number for you. The floor of the gatehouse has a blanket that has been washed. The thermometer still reads like survival."),
                ("take_brass", "No alarm. No speech. Later, a mass note. The chain does not swing. You have brass in the pack."),
                ("leave_brass", "Children pass. Adults file you. The yellow leaves tick against the trough."),
            ),
        },
        {
            "id": "quest_holdfast_the_drawer",
            "display_name": "The Drawer",
            "type": "exploration",
            "briefing": "Ormund's right-hand drawer. The Sector 4 Schedule, complete. Names you know. Names you buried. He turns pages with two fingers. He does not ask how you feel about a column.",
            "prereq_quest_id": "quest_holdfast_authentication",
            "min_day": 90,
            "knowledge_key": "lore_hf_two_schedules",
            "target_location_id": "loc_cluster_office",
            "stages": stages(
                "Read Sole. SOLE, MARGIT J. Records Clerk II. RUR 9. Score 41.2. NOT ALLOCATED. He does not offer to fix her.",
                "Read Renn. RENN, HALVARD — water engineer — allocated — NOT ARRIVED — 12-B UNCONFIRMED. A dependent line that may be Sela if she is yours to name.",
                "Searched Frayne. Absent. RUR 11 does not produce an allocation. He will not write her in because you asked.",
                "Asked about 12-C (optional). He has a string-tied folder. He may show it. He may say it is for a later hour.",
            ),
            "choices": choices(
                ("read_sole", "He does. Completeness is not execution. He executes. He does not raise his voice at her number."),
                ("read_renn", "He does. If Sela is present he does not look at her until the line is finished. Then he does."),
                ("ask_frayne", "The Schedule is not a petition. He offers no further sentence."),
                ("ask_12c", "That is a different folder. You may see it after you have slept in a numbered building. Occupancy first."),
            ),
        },
        {
            "id": "quest_holdfast_the_levy",
            "display_name": "Reconstruction Pool",
            "type": "decision",
            "briefing": "Three names. Thirty days. Occupations as scored or as observed. The ice will not wait for a better feeling. Kit for salt and UV if they go. Tell the people who stay. The duty roster will have holes.",
            "prereq_quest_id": "quest_holdfast_the_drawer",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_cluster_office",
            "stages": stages(
                "Named survivors reviewed. Wrong-by-one occupations corrected or left. Faces. Tools. Who still has skin that will hate brine.",
                "Honour / substitute / refuse. Flags. No combat. The next paragraph of the form.",
                "If sending: kitted. Glass, iodine, salve, calories. Axle ledger. Yara's lit hours.",
                "Remaining shelter informed. Morale as a quiet room, not a speech. A blank on the roster where a name was.",
            ),
            "choices": choices(
                ("holdfast_levy_honour", "You send the three as written. Calories and medicine will come north-to-south on a rate the Office calls regular. The three take Ice Road fatigue and salt-rash risk. Cluster trust ticks up. The people who remain eat easier and sleep worse. One of the three may refuse to return. That refusal will also be filed.", "holdfast_levy_honour"),
                ("holdfast_levy_substitute", "You send three other people. Ormund notes irregular. Yara respects that the ice got three who would walk lit. Edor does not: the names are wrong, and names are his job. Later, an audit. Possibly a second levy. The roster at home still has holes, just different holes.", "holdfast_levy_substitute"),
                ("holdfast_levy_refuse", "You refuse in writing, or by not writing. No shots. Edor waits in the ash. Ice Road access withdraws after eleven days if the Office asks the Cutters to treat you as dark. Ormund does not say or else. The next form is 12-C. Threatening prose unlocks on Office scenes. The three named people hear that they were named.", "holdfast_levy_refuse"),
            ),
        },
        {
            "id": "quest_holdfast_the_membrane",
            "display_name": "Forty-Eight Hours",
            "type": "crisis",
            "briefing": "Bank two trips. Cluster steam clock starts. Leva has the forty-eight hour math in her mouth without making it a threat. Resin, brass, iodine, two workers, an outfall shift. Sector 4's thirst and District 8's thirst are one job until they are not.",
            "prereq_quest_id": "quest_holdfast_the_levy",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_salt_membrane_hall",
            "stages": stages(
                "Diagnosed. Gauge, difference, canal wheel, substation thermometer. Not ideology. Valves.",
                "Gathered. Resin drums, brass seats, iodine, two bodies who will stand the apron.",
                "Outfall shift. Health, salt, the clipboard that is ceremonial until you make it not.",
                "Strip / local salvage / let drop. The indoors will feel it or the Verge will, or both, or neither in time.",
            ),
            "choices": choices(
                ("holdfast_membrane_sector4", "You strip what Sector 4 can spare: Rebuilders brass, iodine, filters. The bank holds. Cluster lives. Allotments thirst clock shortens. Frayne's minutes record a shortage without naming you. Medical market shock. The playground chains do not get their seats back. The tin behind your filter, if you raided it, is lighter. Nobody mentions it.", "holdfast_membrane_sector4"),
                ("local_salvage", "Spent stack, recoating jig, Salt's hidden iodine key. Yield low. Integrity to forty percent if the apron is stood. Nobody in the Verge goes thirstier today. Tomorrow's Tuesday is still short."),
                ("holdfast_membrane_let_drop", "You let steam die. 211 people who have not practised this cold. Office legitimacy cracks. Salt may offer a separate bargain — plant over paper. Unifier path hardens because a treaty wants a room that can still hold heat. Children take coats. Attendance is still taken.", "holdfast_membrane_let_drop"),
            ),
        },
        {
            "id": "quest_holdfast_the_second_list",
            "display_name": "Order 12-C",
            "type": "decision",
            "briefing": "Reconstruction Order 12-C: unlisted occupants of authenticated Allocation 12 are a labour reserve. Published. Nobody in Sector 4 had a copy that survived. Ormund has one. He will come south when the ice allows. You may carry a copy to Sole. She will file it. She will not sign it. Voss will want the pool.",
            "prereq_quest_id": "quest_holdfast_the_membrane",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "loc_cluster_office",
            "stages": stages(
                "Copy obtained. item_order_12c. String tie. Civil-service present tense. No please. No or else.",
                "Sole (optional). Drown boat. She files. She does not sign. Completeness versus execution, same noun, opposite people.",
                "Voss (optional). He wants the pool. He will call it conscription and mean it as a compliment to himself. Intercept risk on a levy column.",
                "Hatch prepared. Roster. Temperature. Whether anyone writes a name. Whether the outer dog is thrown.",
            ),
            "choices": choices(
                ("carry_to_sole", "She reads it twice, which is not Edor's habit, it is hers. She blots the date. She does not blot the refusal."),
                ("show_voss", "He wants names. He will not pay the Office's rate. He will pay in patrols at the Gate. The Cutters will not like the patrols."),
                ("show_neither", "The paper lives in your pack like a spare filter. It does not clean anything. It is still a paper that moves people."),
                ("prepare_shut", "Forty days quiet. A receipt in the ash, cousin to a card in a freezer bag. Different district. Same temperature."),
            ),
        },
        {
            "id": "quest_holdfast_the_hatch",
            "display_name": "The Claim, Reversed",
            "type": "decision",
            "briefing": "Forms at the outer hatch. Escort in faded Continuity jackets. Temperature. The game stops talking. This is not Sela's arrival, or it is Sela's arrival and this. Open or keep shut. Authenticate, house, or levy. Or wait forty days. Write on the duty roster, or do not.",
            "prereq_quest_id": "quest_holdfast_the_second_list",
            "min_day": 90,
            "knowledge_key": "",
            "target_location_id": "player_shelter",
            "stages": stages(
                "Open or shut. The dog on the hatch is a fact. So is the quiet.",
                "If open: authenticate / house / levy. Numbers, Block C tags, three names. Edor asks to speak first if he is there.",
                "If shut: forty days. No combat. The escort remains on a rota you cannot see. Then a receipt.",
                "Roster. A name in a Cluster hand, a name in yours, or a blank that is also a decision.",
            ),
            "choices": choices(
                ("open_honour", "Some of yours live numbered in Block C. The bunker is easier to feed. The roster has names that did not sleep there last winter.", "ending_holdfast_schedule"),
                ("open_12c", "Columns both ways. Receipts in triplicate. Nobody is shot. Sela claimed as dependent if present and if she heard it.", "ending_holdfast_reserve"),
                ("keep_shut", "District 8 continues. Forty empty apartments stay empty. Edor's return may be found later, incomplete, in a good hand.", "ending_holdfast_dark_road"),
                ("write_roster", "Ink on a chart that was always blank. The people who see it in the morning will know which sentence you believed."),
                ("write_nothing", "The chart stays honest about who this hole was built for. It stays dishonest about who kept it."),
            ),
        },
    ]
    dump(DATA / "holdfast_quests.json", quests)

    def item(iid, name, first, inspect, typ="Quest", stack=1, weight=0.4, value=12.0, **kw):
        d = {
            "id": iid,
            "displayName": name,
            "description": first + " " + inspect,
            "type": typ,
            "stackMax": stack,
            "weight": weight,
            "tradeValue": value,
            "thirstRestore": kw.get("thirst", 0.0),
            "hungerRestore": kw.get("hunger", 0.0),
            "moraleEffect": kw.get("morale", 0.0),
        }
        return d

    items = [
        item("item_map_sheet_ice_road", "Ice Road Sheet", "A road that is not there in summer.", "Waxed. Fingerprints kept. Ostrowski will not say who walked it. The Moth will sell you a contradiction. Ivy will confirm a post, not a road."),
        item("item_census_return_blank", "Census Return (blank)", "Pink, yellow, white. White stays with them.", "Occupancy, occupations, dependents, DOB once. Edor will read it again. A blank in your pack is not anonymity. It is a form that wants names."),
        item("item_order_12c", "Reconstruction Order 12-C", "Unlisted occupants of an authenticated facility constitute a labour reserve.", "Published. Sector 4's copies died. This one did not. Sole will file and not sign. Voss will want the pool. The ice will carry a column."),
        item("item_allocation_tag", "Allocation Tag", "Paper. Not a plate.", "Block C guest grammar. Curls. Your name in an Office hand, or a Sector 4 occupation they guessed. Morale when visible in the shelter: some people sleep worse near paper that could become metal."),
        item("item_triplicate_carbon", "Triplicate Carbon", "The third copy is the one they keep.", "Three colours. Introductions by mass. A stolen stack makes the next receipt honest only twice. The Tollman will laugh. Ormund will note.", "Material", 20, 0.1, 4),
        item("item_ice_spike_bar", "Ice Spike Bar", "A bar for ice that is lying.", "Harbour steel, worn at the bite. Accident chance down on the Cut if someone who can read dark is holding it. Not a weapon. A question you ask the road.", "Tool", 1, 2.4, 18),
        item("item_beacon_oil", "Beacon Oil", "Finger-widths to the WINDOW line.", "Tithe and relight. The measuring-stick in the South Beacon cage is the honest clock. Steal it and the next column writes an accident with your mass.", "Fuel", 6, 1.2, 14),
        item("item_cutter_ledger_blank", "Cutter Ledger (blank)", "Date, origin, mass, remarks.", "Remarks are for the dead. A blank book is not hope. It is capacity. Yara will know if you invent a twelfth filter notch in the same hand."),
        item("item_ice_tyre_set", "Ice Tyre Set", "Without these, the Ice Road is walking.", "Vehicle component. Speed and accident chance. Not a driving game. A crate of rubber that smells like the Recovery Yard and salt.", "Material", 1, 18.0, 40),
        item("item_plant_suit_patched", "Plant Suit (patched)", "Never hazmat. Inner-tube at the knees.", "Grey canvas, visor clouded from the inside. Salt-rash down, fatigue up. Degrades faster in UV. The patch is a bicycle tube from a year that still had bicycles.", "Protective", 1, 3.5, 22),
        item("item_resin_gloves", "Resin Gloves", "Insides powdered. Outsides glazed.", "Spent stack handling. Bare hands are how Tuesdays get worse. One pair on the jig is communal. Taking it is a shift decision.", "Protective", 2, 0.3, 8),
        item("item_fume_rag", "Fume Rag", "Wet it. Don't pretend it is a mask.", "Chest-height fume in Hall 2. A rag is not a filter. It is the difference between a tour and a shift. Iodine after.", "Filter", 4, 0.1, 2),
        item("item_shift_whistle", "Shift Whistle", "The whistle is the limit. The limit is skin.", "Enforces outfall hours if someone blows it. Fatigue up because limits are work. Leva will give you one. Children should not think steam is a story.", "Tool", 1, 0.05, 6),
        item("item_work_ticket", "Work Ticket", "The queue is for this, not bread.", "Indoor access. A day of labour in a district that inventories you while you work. Steam if the pipe is live. Yellow cultivars if you are on trough duty."),
        item("item_steam_token", "Steam Token", "Eight hours of waystation warmth, if the substation agrees.", "Stamped fibre, not coin. Cluster currency-in-kind. You cannot steal heat. You can steal tokens. The wooden box at the valve house will be light.", "Trade", 12, 0.02, 9),
        item("item_block_c_key", "Block C Key", "A key for a door with a paper tag.", "Guest housing. The radiator ticks if the canal is honest. Children's boots in C-214 if you have not taken them yet. Home still ticks without you.", "Tool"),
        item("item_ro_resin", "RO Resin", "Brine becomes process. Process is not clean.", "Plant repair. Tuesday's short count. Virgin drums are brown-stencilled and heavy. Heat and iodine still required. District 8 will never make Sector 4 thirst irrelevant.", "Material", 4, 8.0, 35),
        item("item_ro_resin_spent", "Spent RO Resin", "Looks dry. Is not.", "Sample from Hall 2. Recoat yield low. Toxic to handle. Valuable to people who still believe. Ice crows will not land on the stack.", "Material", 4, 6.0, 8),
        item("item_iodine_crystal", "Iodine Crystal", "Thyroid and water in the same cage.", "Bulk. Process column, then thyroid, then clinic. The Office has a key. The Salt has a tea-tin. Lot numbers are Continuity. So is the stamp NOT FOR GENERAL ISSUE.", "Iodine", 8, 0.6, 28),
        item("item_process_barrel", "Process Barrel", "Transport. Twenty percent spoilage if the ice lies.", "Thirst at forty percent if drunk raw. Electrolyte salts after. Haul south and lose some to the Cut. Rebuilders still need tablets. You cannot pipe this to Allocation 12.", "Water", 4, 12.0, 16, thirst=40.0),
        item("item_schedule_crystal", "Schedule Crystal", "The hour, not the order.", "A crystal that keeps Hearth-4's schedule even when the foghorn is stolen. Hearing is not a stand-up. Mire will say so.", "Device"),
        item("item_fleet_pad_copy", "Fleet Pad Copy", "It does not authenticate.", "Same family as D/9. Wrong door. Show it to Mire and he will be interested. Interest is not a hatch. Voss cannot conscript a ship with it."),
        item("item_foghorn_key", "Foghorn Key", "Winds the spring. Does not decide who is coming.", "Plinth hook. Companion to the escapement. Cutters navigate by sounding. Silence it to hide and something on the water loses the coast as well.", "Tool"),
        item("item_kittiwake_copy", "Kittiwake Copy", "The log continues eleven days past the Exchange.", "If the chart was copied. Channel markers versus the sheet versus the Moth. Nomi goes quieter, not warmer. She already knew. She had not been paid."),
        item("item_weigh_receipt_hf", "Weigh Receipt", "Introduction — twelve kilograms equivalent.", "Tollman grammar meeting Office grammar. Honest paper. The destination field may say ESTUARY / SEASONAL. Someone may have written the Salt underneath."),
        item("item_schedule_sector4_copy", "The Other Schedule", "Every name is legible. Including yours, in a column you were not meant to see.", "Ribbon copy from Ormund's drawer, or a carbon that travelled. Sole is here. Renn is here. Frayne is not. 12-C is a different folder; this is only the occupancy that was decided in advance. Do not fold it. The crease would go through a score."),
        item("item_halvard_kit_notes", "Improvised Potable", "His handwriting gets smaller toward the end. The diagrams do not.", "Field notes from Allocation 12-B. Intake, cloth, iodine, heat, a barrel that was never a plant. Water-craft bonus at the waystation if someone can still read a small hand. If Sela is present she will leave the room if you call it salvage. She will stay if you call it engineering."),
        item("item_sole_unsigned", "Filed, Not Signed", "She blotted the date. She did not blot the refusal.", "12-C, Drown-stamped, unsigned. D/9 stand-down still works; Fleet pad still does not. Completeness versus execution on one sheet. The blot is ink, not tears. Do not describe it as tears."),
        item("item_playground_seat", "One Seat", "The chain is still there. The brass is in your pack.", "A swing seat, unscrewed. 1× brass_fittings that everyone notices: Quad, Grade Hut, Allotments board, the tin if you know the tin. Children do not ask. Auditors do. You can put it back. The chain will not swing.", "Material", 1, 2.0, 20),
        item("item_edor_return_self", "Clerk's Own Return", "The birth year is written twice. Once correctly.", "Pink copy. Two years. Convoy 12's training example in a living person. If the error is left, he will omit a name for you once and hate it. If struck, Ormund will see the strike. Keep it off the duty roster. It is not a trophy."),
        item("item_yara_dark_mark", "Dark Mark", "She did not raise her voice. The beacon is dark.", "A lath with black cloth, or the absence of oil in a cage. Ice Road access destroyed. Thick ice will still be ice. It will not be a road. You cannot talk this back on. Eleven days is for lesser darks. This is a withdrawal."),
        item("item_leva_minutes_vol12", "Volume 12", "Motion: that we keep running. Carried.", "Binder, tabs, a failed valve-seat that used to paperweight it. Steam-trip warning six hours early if you keep it in the hall or the waystation. The Office would like a copy. The copy would not hear the gauges."),
        item("item_hearth4_hatch_log", "Hatch Log", "They logged every refusal. There are a lot of refusals.", "Clipboard, pouch, dates, reasons: NO STAND-UP / NO NUMBER / BLASTING PARTY — DENIED. Icebreaker without a hundred explosives if a number authenticates. Stealing it does not empty his memory. He will say it again."),
        item("item_alloc7_ration_tin", "ALLOC-7 Tin", "NOT FOR GENERAL ISSUE. The issue is you.", "Olive, stencil, frozen rim if opened on the Cut. Food. Morale down if opened in Sector 4, where the stamp is a mirror. Accident 12 still has more. Ice crows know the timetable.", "Food", 6, 0.5, 12, hunger=18.0, morale=-4.0),
        item("item_cluster_formulary", "Human Formulary", "Dosage for a species the Verge has been approximating.", "Bound, pre-war, Clinic-kept. Ianov payoff. Surgery odds. They will not send a copy south unless the levy is honoured. A child's correction on the thyroid plate is in pencil and is correct.", "Medical"),
        item("item_foghorn_timer", "Foghorn Escapement", "It sounds whether anyone is coming or not.", "Brass clockwork from Foghorn 8. Shelf navigation. If owned, a faint sounding on Silence nights. If taken, Yara loses the coast in fog and so does the tender. Quiet is how columns vanish.", "Device"),
        item("item_tin_fourteenth", "The Fourteenth Plate", "The tin is lighter. Nobody mentions it.", "Only if you sold nameplates north. One plate missing from the fourteen behind the filtration stack. District 8 paid more than the Works. Still no comment."),
        item("item_salt_rash_salve", "Salt-Rash Salve", "Grit in the grease. Soothes. Does not cure.", "Two finger-scoops gone from the waystation tin. Iodine soothes not cures. The clipboard at the outfall will not thank you.", "Medical", 4, 0.2, 10),
        item("item_uv_grease", "UV Grease", "Albedo is a tax. This is a delay.", "One expedition of blistering down. Coastal ozone, ice shine. Sun-Seekers will want visors more than grease. Grease is what you have.", "Medical", 4, 0.2, 8),
        item("item_electrolyte_salts", "Electrolyte Salts", "For people who drank the process.", "Counters process-water drinking. Leva will still count you as a problem if you skip iodine. Salts are not a membrane.", "Medical", 8, 0.15, 7, thirst=8.0),
    ]
    dump(DATA / "holdfast_items.json", items)

    # Recast three existing location blocks in locations.json
    loc_path = DATA / "locations.json"
    locs = json.loads(loc_path.read_text(encoding="utf-8"))
    recast_by_id = {e["id"]: e for e in locations if e["id"] in RECAST_ALWAYS}
    for row in locs:
        rid = row.get("id")
        if rid in recast_by_id:
            src = recast_by_id[rid]
            row["displayName"] = src["displayName"]
            row["description"] = src["inspect"] + "\n\n" + src["description"]
            if src["travelHours"] > 0:
                row["travelHours"] = src["travelHours"]
            if src["dangerLevel"] > 0:
                row["dangerLevel"] = src["dangerLevel"]
            if src["baseRadsPerHour"] > 0:
                row["baseRadsPerHour"] = src["baseRadsPerHour"]
    loc_path.write_text(json.dumps(locs, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print("recast locations.json desalination / barge / convoy")

    chars_path = DATA / "characters.json"
    chars = json.loads(chars_path.read_text(encoding="utf-8"))
    existing = {c["id"] for c in chars}
    holdfast_npcs = [
        {
            "id": "npc_edor_vale",
            "display_name": "Edor Vale",
            "profession": "Census Clerk Grade III",
            "bio": "Junior enumerator. Score 60.4 — lowest allocated band. He knows it. Offers to read the form again. Will not enter the bunker uninvited. Will not falsify a date of birth.",
            "faction": "none",
            "region": "the_cut",
            "first_day": 90,
            "location_id": "loc_weighbridge",
            "wants": ["corrected_occupations", "dates_of_birth_once"],
            "offers": ["the_pink_copy", "to_wait_in_the_ash"],
            "will_not": ["enter_uninvited", "falsify_a_dob"],
            "signature_quote": "Most people want it read again. That's all right. There isn't a time limit on understanding it. There is a time limit on the ice.",
        },
        {
            "id": "npc_yara_holm",
            "display_name": "Yara Holm",
            "profession": "Cutter",
            "bio": "Harbour ice-pilot. Score 44. Unlisted. Hired because the allocated would not go out in year one. Dark and lit are moral words. Will not guide a column onto ice she has marked dark. Will not blast.",
            "faction": "none",
            "region": "the_cut",
            "first_day": 90,
            "location_id": "loc_cut_waystation_a",
            "wants": ["item_beacon_oil", "columns_that_wait"],
            "offers": ["lit_hours", "accident_book", "overnight_at_a"],
            "will_not": ["guide_dark_ice", "blast"],
            "signature_quote": "I don't open it for you. I open it. If it's dark, you wait. If you don't wait, I write the accident in the book and I don't fetch you.",
        },
        {
            "id": "npc_leva_quist",
            "display_name": "Leva Quist",
            "profession": "Shift Lead",
            "bio": "Municipal RO technician. Never allocated — in situ essential. Counts out loud. Will not shut the plant to spite Ormund. Corrects people who say abandoned.",
            "faction": "hydro_barons",
            "region": "the_saltworks",
            "first_day": 90,
            "location_id": "loc_salt_grade_hut",
            "wants": ["item_ro_resin", "brass_fittings", "outfall_hands"],
            "offers": ["salt_trade", "shift_whistle", "volume_12"],
            "will_not": ["shut_the_plant_to_spite"],
            "signature_quote": "They scored high enough to be continued. The membranes don't care.",
        },
        {
            "id": "npc_cael_ormund",
            "display_name": "Cael Ormund",
            "profession": "Registrar-General",
            "bio": "Logistics planner, Office of Continuity. RUR 34, score 62.1, ALLOCATED. Civil-service present tense. Will not falsify a score. Will not raise his voice. Will not call anyone a thief.",
            "faction": "none",
            "region": "the_cluster",
            "first_day": 90,
            "location_id": "loc_cluster_office",
            "wants": ["occupancy_return", "levy_honour", "item_order_12c"],
            "offers": ["the_schedule", "regular_rate", "discrepancy_noted"],
            "will_not": ["falsify_a_score", "raise_his_voice", "call_anyone_a_thief"],
            "signature_quote": "You are living in a facility that authenticated for fourteen. The fourteen did not arrive. I am not collecting you. I am scheduling you.",
        },
        {
            "id": "npc_halden_mire",
            "display_name": "Halden Mire",
            "profession": "Sparks",
            "bio": "Fleet radio. The authenticator above the boarding hatch is still a small green lamp. He will not open for unauthenticated boarding. He logs every refusal.",
            "faction": "none",
            "region": "the_shelf",
            "first_day": 110,
            "location_id": "loc_shelf_hearth4",
            "wants": ["stand_up_order", "allocation_number"],
            "offers": ["hatch_log", "schedule_crystal"],
            "will_not": ["open_unauthenticated", "blast_the_ice"],
            "signature_quote": "The authenticator wanted a number.",
        },
        {
            "id": "npc_cluster_teacher",
            "display_name": "Cluster Teacher",
            "profession": "Schoolteacher",
            "bio": "Unnamed. Nineteen children. Curriculum includes the Reconstruction Utility Rating, taught as arithmetic. A dependent is worth points. That is on the worksheet.",
            "faction": "none",
            "region": "the_cluster",
            "first_day": 90,
            "location_id": "loc_cluster_school",
            "wants": ["consistency"],
            "offers": ["the_hour", "the_rubric"],
            "will_not": ["allow_a_referendum"],
            "signature_quote": "A dependent is worth points. Sit the hour. Correct a sum or let it stand.",
        },
    ]
    added = 0
    for npc in holdfast_npcs:
        if npc["id"] not in existing:
            chars.append(npc)
            added += 1
    chars_path.write_text(json.dumps(chars, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(f"characters.json now {len(chars)} (+{added})")

    hist_path = DATA / "world_history.json"
    hist = json.loads(hist_path.read_text(encoding="utf-8"))
    existing_keys = {h.get("knowledge_key") for h in hist}
    lore = [
        {
            "era": "ashfall",
            "year_month": "Exchange+3Y",
            "title": "A Road That Is Not There in Summer",
            "body": "Bram Ostrowski sells waxed sheets of the estuary. A road is drawn where summer water should be. He will not say who walked it. Channel markers do not match the last Sector 4 lamp. Ivy confirms the post. She does not confirm the road. She does not cross.",
            "discovery_location_id": "loc_ice_road_gate",
            "discovery_trigger": "location_explore",
            "knowledge_key": "lore_hf_sheet",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+3Y",
            "title": "Two Schedules",
            "body": "Ormund's right-hand drawer holds the Sector 4 Schedule, complete. Sole is there, not allocated. Renn is there, allocated, not arrived. Frayne is not. Completeness is not execution. He executes. Margit Sole's copy in the Drown is the same Schedule in a different room.",
            "discovery_location_id": "loc_cluster_office",
            "discovery_trigger": "location_explore",
            "knowledge_key": "lore_hf_two_schedules",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+3Y",
            "title": "Forty Rooms",
            "body": "Forty apartments held for arrivals. Dusted. Walk three. You will find boots. Sizes one through four. You may leave them. You may not call them unclaimed. They are claimed by a timetable.",
            "discovery_location_id": "loc_cluster_block_c",
            "discovery_trigger": "location_explore",
            "knowledge_key": "lore_hf_forty_rooms",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+4Y",
            "title": "The Schedule Holds",
            "body": "The duty roster on the bunker wall has names on it that are not the names that slept there. Block C has plates where paper was. Process water comes south in barrels that lose a fifth to the Cut, and the Verge still boils what it has. In the Office drawer the discrepancy file is closed with a stamp that does not say resolved. It says entered. Children in the Cluster school add a working that includes a caretaker. The teacher circles the rubric anyway. The chains on the Quad have not found their seats. Someone has tied a rag to one of them, so that in wind it looks as if a swing were trying.",
            "discovery_location_id": "loc_cluster_office",
            "discovery_trigger": "journal",
            "knowledge_key": "lore_hf_ending_schedule",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+4Y",
            "title": "The Reserve",
            "body": "Receipts in triplicate: pink in a satchel that went south, yellow in a weigh hut, white in a drawer. Nobody was shot. Columns moved. If a dependent was claimed, a clinic autoclave cycled for a child who knew a tunnel better than a stairwell, and a kit of small handwriting stayed in a hole that no longer had its water memory. Margit Sole filed 12-C and did not sign it. Cael Ormund executed it and did not raise his voice. The ice took a column south and a column north on the same window. Yara wrote both masses. She did not write which one was fair.",
            "discovery_location_id": "loc_cluster_office",
            "discovery_trigger": "journal",
            "knowledge_key": "lore_hf_ending_reserve",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+4Y",
            "title": "The Road Goes Dark",
            "body": "District 8 continues without the unlisted hole. Forty apartments stay dusted. The yellow cultivars fail on their own timetable. In a weigh hut, after a thaw, a census return is found incomplete, in a good hand, occupations still adjacent-wrong, a date of birth written once. The remarks column of the accident book has a line that is not for the dead. It says incomplete. Lamps on the Cut go out in the ordinary way, one wick at a time, because windows close. Ivy's 19 stays lit. The seam between ash and salt is still a poorly taped join. Nobody crosses it who does not already know how.",
            "discovery_location_id": "player_shelter",
            "discovery_trigger": "journal",
            "knowledge_key": "lore_hf_ending_dark_road",
        },
        {
            "era": "ashfall",
            "year_month": "Exchange+4Y",
            "title": "Stand-Up",
            "body": "The Fleet stops being a rumour. Hearth-4's lamp is still green; green was current; current is now people on a quay that is a field. The Cluster votes on beds. Some vote with work tickets. Some vote with the indoor thermometer. Mire logs the vote and does not vote. A pad that would not verify a land form still will not. It did not have to. The authenticator wanted a number and a number was found, or a person was asked ashore, which was a different request. Migration and Icebreaker land in a place. The place has a playground with chains. The new coats on the school pegs are damp with salt.",
            "discovery_location_id": "loc_shelf_hearth4",
            "discovery_trigger": "journal",
            "knowledge_key": "lore_hf_ending_tender",
        },
    ]
    added_lore = 0
    for e in lore:
        if e["knowledge_key"] not in existing_keys:
            hist.append(e)
            added_lore += 1
    hist_path.write_text(json.dumps(hist, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(f"world_history.json lore_hf added {added_lore}")


if __name__ == "__main__":
    main()
