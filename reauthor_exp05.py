import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_expansion_05.json"

with open(file_path, "r") as f:
    data = json.load(f)

# Reauthor map: id -> { 'title': '', 'description': '', 'synopsis': '', 'choices': [ { 'text': '', 'consequences': '' } ] }
updates = {
    "quest_survivor_disappearance": {
        "title": "Unaccounted Personnel",
        "description": "An asset has failed to report for three consecutive shifts. Initiate search protocols or log as acceptable loss.",
        "synopsis": "Unit Maria-7 failed to report for the hydroponics shift. The asset represents a significant caloric investment. Options are limited to expending further calories on retrieval or writing off the deficit.",
        "choices": [
            { "text": "Deploy search detail", "consequences": "The search detail returns empty-handed, having burned two days of operational calories. The asset is officially logged as terminated." },
            { "text": "Execute sector quarantine", "consequences": "Sector lockdown prevents further immediate losses, but halts all external resource gathering. The asset remains unaccounted for." },
            { "text": "Log as acceptable loss", "consequences": "The asset is crossed off the manifest. The saved rations are redistributed. Efficiency improves marginally." }
        ]
    },
    "quest_bunker_power_crisis": {
        "title": "Primary Generator Degradation",
        "description": "The central turbine is operating at 40% efficiency. Immediate intervention required to prevent thermal collapse.",
        "synopsis": "The main diesel turbine is shedding load. If the core temperature drops below operational thresholds, the water cyclers will freeze and burst.",
        "choices": [
            { "text": "Execute emergency overhaul", "consequences": "You cannibalize secondary systems to repair the turbine. Power is restored, but the structural redundancy is permanently compromised." },
            { "text": "Reroute to auxiliary banks", "consequences": "You shift load to the chemical batteries. It buys time, but the voltage drop forces a strict rationing of the thermal output." },
            { "text": "Enforce brownout protocols", "consequences": "You shut down non-critical sectors. The temperature in the lower levels drops to lethal ranges. The resulting casualties reduce the overall caloric demand." }
        ]
    },
    "quest_survivor_mental_health": {
        "title": "Psychological Degradation Incident",
        "description": "An asset is exhibiting erratic behavior, threatening operational cohesion.",
        "synopsis": "Unit Jamie-4 is demonstrating severe psychological fracture, interfering with the structural repair schedule and consuming excess medical overhead.",
        "choices": [
            { "text": "Administer chemical restraint", "consequences": "You expend valuable pharmaceuticals to stabilize the unit. They return to minimal functionality, but the medical deficit remains." },
            { "text": "Execute expulsion protocol", "consequences": "The unit is escorted to the airlock and expelled. The physical threat is neutralized, though the labor pool is permanently reduced." },
            { "text": "Ignore degradation", "consequences": "The unit continues to degrade until a violent incident forces intervention. The resulting damage to infrastructure costs more than the initial treatment would have." }
        ]
    },
    "quest_exp09_sunken_submarine": {
        "title": "The Submerged Asset",
        "description": "The Black Flotilla has provided coordinates for a sunken pre-war vessel. Salvage operations require specialized gear.",
        "synopsis": "Coordinates and a Flotilla cipher indicate the location of the Half-Submerged Barrik. The vessel is a confirmed war grave. The Flotilla expects an accounting of the interior.",
        "choices": [
            { "text": "Execute interior survey", "consequences": "You penetrate the flooded hull and log the personnel tags on the bulkhead. You leave the biological remains undisturbed. The Flotilla logs your compliance." },
            { "text": "Abort descent", "consequences": "The structural risks exceed operational parameters. You return to the surface. The Flotilla notes your lack of commitment to the task." },
            { "text": "Extract external salvage", "consequences": "You strip the hull of functional brass fittings and pressure gauges. The interior remains sealed. The material yield is acceptable." }
        ]
    },
    "quest_neighboring_settlement_aid": {
        "title": "Adjacent Sector Distress",
        "description": "A neighboring outpost is broadcasting an unencrypted request for material support.",
        "synopsis": "Outpost 'Echo' is experiencing a severe caloric deficit and broadcasting on all open frequencies. Responding expends your own resources; ignoring them invites instability in the sector.",
        "choices": [
            { "text": "Transfer surplus calories", "consequences": "You dispatch a minimal ration shipment. The outpost stabilizes and transmits a cache of localized survey data in exchange." },
            { "text": "Maintain radio silence", "consequences": "You ignore the broadcast. The transmissions cease after four days. The sector grows quieter." },
            { "text": "Deploy acquisition team", "consequences": "You send an armed detail to secure whatever assets Echo has left. The yield is low, consisting mostly of degraded equipment and desperate units." }
        ]
    },
    "quest_black_flotilla_trade": {
        "title": "Flotilla Exchange Protocol",
        "description": "The Black Flotilla has arrived at the boundary markers, offering high-value technology for bulk calories.",
        "synopsis": "The Flotilla requires immense caloric input to maintain their maritime operations. They offer sealed pre-war hardware in exchange for your food reserves.",
        "choices": [
            { "text": "Execute bulk transfer", "consequences": "You transfer the requested tonnage of grain. The hardware is functional and significantly upgrades your water purification efficiency." },
            { "text": "Deny transaction", "consequences": "You refuse the terms. The Flotilla departs without incident, but your name is removed from their priority route." },
            { "text": "Attempt material deception", "consequences": "You cut the grain sacks with sawdust. The Flotilla inspectors detect the anomaly, confiscate the shipment, and designate your settlement hostile." }
        ]
    },
    "quest_bunker_upgrade": {
        "title": "Structural Expansion Initiative",
        "description": "Population density has reached critical limits. Expansion into Sector 4 is proposed.",
        "synopsis": "The current living quarters are at 140% capacity, leading to increased disease transmission. Expanding into the collapsed Sector 4 requires heavy labor and material investment.",
        "choices": [
            { "text": "Authorize rapid expansion", "consequences": "The sector is cleared quickly, but poor shoring leads to a minor cave-in. The space is usable, but requires constant maintenance." },
            { "text": "Execute reinforced clearance", "consequences": "The expansion is slow and consumes massive amounts of steel framing. The resulting sector is highly stable and reduces overall disease vectors." },
            { "text": "Deny expansion", "consequences": "Population density remains critical. You enforce stricter rationing to offset the inevitable medical overhead." }
        ]
    },
    "quest_radiation_study": {
        "title": "Meridian Compact Survey",
        "description": "A survey team from the Meridian Compact requests access to your local radiation telemetry.",
        "synopsis": "A well-equipped team from the Meridian Compact wishes to install monitoring equipment in your territory. They offer shared data access in return.",
        "choices": [
            { "text": "Authorize installation", "consequences": "The equipment is installed. The telemetry data allows you to optimize scavenging routes, avoiding unseen high-rad pockets." },
            { "text": "Deny access", "consequences": "You refuse their presence. The team departs smoothly, logging your settlement as uncooperative in their central database." },
            { "text": "Exploit hardware", "consequences": "You allow the installation, then strip the equipment for rare earth metals once the team departs. The Compact ceases all communication." }
        ]
    },
    "quest_survivor_rescue": {
        "title": "Perimeter Asset Recovery",
        "description": "An injured unit has collapsed near the outer warning wire.",
        "synopsis": "A unit lies motionless near the perimeter, displaying signs of severe dehydration and physical trauma. They represent both a potential labor asset and a medical burden.",
        "choices": [
            { "text": "Execute recovery", "consequences": "You expend medical supplies to stabilize the unit. Upon recovery, they are integrated into the labor pool, providing specialized mechanical skills." },
            { "text": "Maintain perimeter", "consequences": "You do not engage. The unit expires by nightfall. The biological remains are processed for salvage." },
            { "text": "Extract intelligence", "consequences": "You administer minimal stimulants to interrogate the unit about local hostiles before they expire. The intelligence is actionable." }
        ]
    },
    "quest_raider_peace_offer": {
        "title": "Hostile Faction Extortion",
        "description": "A local hostile element demands regular material tribute in exchange for operational stability.",
        "synopsis": "The hostile group controlling Sector 5 has proposed a 'security tariff'. Paying it depletes resources; refusing it guarantees armed conflict.",
        "choices": [
            { "text": "Authorize tariff payment", "consequences": "You transfer the requested materials. The hostiles cease perimeter harassment, but the economic drain severely impacts internal operations." },
            { "text": "Reject tariff", "consequences": "You refuse the demand. The hostiles initiate a coordinated assault on your perimeter, forcing a heavy expenditure of ammunition to repel them." },
            { "text": "Negotiate reduced tariff", "consequences": "You negotiate a lower payment rate. The hostiles accept, but maintain a threatening posture. The economic drain is manageable but persistent." }
        ]
    },
    "quest_expedition_equipment": {
        "title": "Expedition Hardware Overhaul",
        "description": "Current scavenging gear is degrading. An overhaul utilizing pre-war tech is proposed.",
        "synopsis": "Surface operations are suffering due to failing environmental seals and degraded optics. Overhauling the gear requires a significant investment of rare components.",
        "choices": [
            { "text": "Authorize full overhaul", "consequences": "You expend the components. The upgraded gear significantly reduces radiation exposure and increases the caloric yield of surface operations." },
            { "text": "Authorize partial overhaul", "consequences": "You repair only the critical seals. Surface teams remain functional, but continue to suffer minor environmental degradation." },
            { "text": "Deny resource expenditure", "consequences": "You conserve the components. Surface operations continue to suffer high attrition rates and low yields." }
        ]
    },
    "quest_medical_experiment": {
        "title": "Experimental Rad-Purge Protocol",
        "description": "The medical officer proposes testing an unverified chemical compound to treat radiation sickness.",
        "synopsis": "Medical Officer Vasquez has synthesized a compound from degraded pre-war pharmaceuticals. It may reverse severe radiation poisoning, or it may induce rapid organ failure.",
        "choices": [
            { "text": "Authorize human trials", "consequences": "The compound is administered. The radiation sickness is reversed, but the subjects suffer permanent, localized nerve damage. A net positive for the labor pool." },
            { "text": "Deny human trials", "consequences": "You refuse the protocol. The irradiated subjects degrade and expire according to standard medical projections." },
            { "text": "Test on hostile captives", "consequences": "The compound is tested on detained hostiles. The efficacy is confirmed with no risk to your own labor pool. The data is logged." }
        ]
    },
    "quest_scout_training": {
        "title": "Surface Orientation Program",
        "description": "Implement a structured training program for surface operatives to reduce attrition.",
        "synopsis": "Untrained units assigned to surface detail are expiring at a 40% rate. A formalized training program would reduce this, but requires experienced units to cease labor and instruct.",
        "choices": [
            { "text": "Implement full curriculum", "consequences": "The training consumes significant operational time, but the survival rate of new operatives increases dramatically. Long-term caloric yield improves." },
            { "text": "Implement minimal briefing", "consequences": "You provide basic hazard orientation. Attrition drops slightly, but remains a constant drain on the population." },
            { "text": "Maintain current protocol", "consequences": "You assign units directly to the surface. The high attrition rate continues, forcing you to constantly seek new personnel." }
        ]
    },
    "quest_food_storage_theft": {
        "title": "Caloric Inventory Discrepancy",
        "description": "An audit of the central storage reveals a persistent deficit in caloric reserves.",
        "synopsis": "High-density rations are being removed from secure storage outside of authorized distribution windows. The leak must be plugged before it impacts winter projections.",
        "choices": [
            { "text": "Execute covert surveillance", "consequences": "You identify a unit siphoning calories for an unregistered dependent. You confiscate the stolen mass and reassign the unit to heavy labor." },
            { "text": "Initiate aggressive search", "consequences": "A sweep of all quarters recovers the missing rations, but the indiscriminate search protocol damages operational cohesion." },
            { "text": "Write off the deficit", "consequences": "You adjust the ledger to account for the loss. The thefts continue, steadily eroding your operational margin." }
        ]
    },
    "quest_medical_triage": {
        "title": "Mass Casualty Triage",
        "description": "A localized thermal event has resulted in multiple casualties exceeding medical capacity.",
        "synopsis": "A structural fire has produced six severe burn casualties. You possess sufficient pharmaceutical assets to stabilize only two. Selection is required.",
        "choices": [
            { "text": "Prioritize high-value assets", "consequences": "You stabilize the chief engineer and the primary medic. The remaining four units expire. Operational capacity is maintained at the cost of raw labor." },
            { "text": "Prioritize likely survivors", "consequences": "You stabilize the two youngest, healthiest units. The specialized personnel expire, forcing you to rely on untrained replacements." },
            { "text": "Distribute assets equally", "consequences": "You divide the pharmaceuticals among all six. The dosage is insufficient. All six units expire within 48 hours." }
        ]
    }
}

for q in data.get("quests", []):
    qid = q.get("id")
    if qid in updates:
        q["title"] = updates[qid]["title"]
        q["description"] = updates[qid]["description"]
        q["synopsis"] = updates[qid]["synopsis"]
        for i, c in enumerate(q.get("choices", [])):
            if i < len(updates[qid]["choices"]):
                c["text"] = updates[qid]["choices"][i]["text"]
                c["consequences"] = updates[qid]["choices"][i]["consequences"]

with open(file_path, "w") as f:
    json.dump(data, f, indent=4)
