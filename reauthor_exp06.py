import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_expansion_06.json"

with open(file_path, "r") as f:
    data = json.load(f)

updates = {
    "quest_radiation_storm_warning": {
        "title": "Severe Atmospheric Hazard",
        "description": "Meteorological telemetry indicates an imminent high-density radioactive front.",
        "synopsis": "A level-four particulate storm is projected to strike the sector within 12 hours. Surface operations must be suspended, and all exterior seals require immediate reinforcement to prevent internal contamination.",
        "choices": [
            { "text": "Execute maximum lockdown", "consequences": "You expend structural sealants and halt all operations. The interior remains uncontaminated. Caloric output for the day is zero." },
            { "text": "Execute partial lockdown", "consequences": "You seal only primary vents and continue critical indoor labor. Ambient radiation levels spike internally, marginally increasing the long-term cancer risk for all personnel." },
            { "text": "Maintain standard operations", "consequences": "You ignore the telemetry to maintain production. The storm penetrates the primary filtration. Seven units suffer acute radiation sickness." }
        ]
    },
    "quest_survivor_leadership_election": {
        "title": "Command Structure Audit",
        "description": "Personnel are organizing to formally challenge the current command hierarchy.",
        "synopsis": "A coalition of units has demanded a formalized transition of command, citing recent caloric deficits. They are attempting to institute an electoral protocol to replace your authority.",
        "choices": [
            { "text": "Concede command", "consequences": "You step down. The coalition installs a new director. Your personal caloric allocation is immediately reduced to baseline." },
            { "text": "Deny command transition", "consequences": "You refuse to acknowledge the protocol. The coalition attempts a labor strike, which you break by withholding rations. Efficiency remains crippled for weeks." },
            { "text": "Manipulate outcome", "consequences": "You rig the ballot collection. You retain command, but the obvious manipulation permanently damages the settlement's operational cohesion." }
        ]
    },
    "quest_food_storage_reorganization": {
        "title": "Caloric Storage Optimization",
        "description": "The current organization of the cold-stores is resulting in unacceptable spoilage rates.",
        "synopsis": "Improper stacking and poor air circulation in the food reserves are causing baseline rations to rot before consumption. Reorganizing the space requires halting all other labor for two shifts.",
        "choices": [
            { "text": "Execute complete optimization", "consequences": "You divert the labor. The reorganization stops the spoilage, securing the winter supply, though the lost operational time delays structural repairs." },
            { "text": "Execute critical optimization", "consequences": "You re-stack only the most perishable items. The spoilage rate is halved, but remains a constant drain on the ledger." },
            { "text": "Deny optimization", "consequences": "Labor continues on schedule. The spoilage continues unchecked, forcing severe rationing later in the season." }
        ]
    },
    "quest_survivor_skill_training": {
        "title": "Cross-Disciplinary Labor Initiative",
        "description": "A proposal to cross-train units to reduce single-point-of-failure vulnerabilities.",
        "synopsis": "The settlement relies heavily on isolated specialists. A proposal has been filed to cross-train standard labor units in mechanical and agricultural maintenance.",
        "choices": [
            { "text": "Implement comprehensive cross-training", "consequences": "You divert operational time to the training. General efficiency drops temporarily, but the settlement becomes highly resilient to targeted casualties." },
            { "text": "Implement targeted cross-training", "consequences": "You train backups only for the water cycler and generator. The vulnerability is reduced, but the agricultural sector remains exposed." },
            { "text": "Deny cross-training", "consequences": "You maximize immediate output. A subsequent accident claims the primary botanist, and hydroponic yields plummet." }
        ]
    },
    "quest_raider_siege": {
        "title": "Sector Blockade",
        "description": "A hostile faction has established a perimeter blockade, cutting off scavenging routes.",
        "synopsis": "Armed hostiles have surrounded the primary exits. They are demanding a transfer of medical supplies and ammunition to lift the blockade. Refusal guarantees a breach attempt.",
        "choices": [
            { "text": "Execute defensive protocol", "consequences": "You man the barricades. The hostiles attempt a breach and are repelled, but you expend 40% of your ammunition reserves in the process." },
            { "text": "Authorize material transfer", "consequences": "You pay the toll. The hostiles withdraw. Your reserves are severely depleted, but the physical structure and personnel remain intact." },
            { "text": "Execute total surrender", "consequences": "You open the gates. The hostiles strip the settlement of all high-value assets and relegate your personnel to forced labor." }
        ]
    },
    "quest_medical_quarantine": {
        "title": "Biological Contagion Protocol",
        "description": "A unit is displaying symptoms of an unidentified, highly communicable pathogen.",
        "synopsis": "Unit Alex-9 is exhibiting acute respiratory distress and elevated temperature. The pathogen's transmission vector is unknown, but standard modeling suggests high virulency.",
        "choices": [
            { "text": "Enforce strict quarantine", "consequences": "You isolate the unit in a sealed sector. The pathogen does not spread. The unit's labor is lost for the duration of the quarantine." },
            { "text": "Maintain operational integration", "consequences": "You keep the unit on shift. The pathogen spreads rapidly. Production halts entirely as 30% of the settlement becomes symptomatic." },
            { "text": "Execute immediate expulsion", "consequences": "You force the unit out of the airlock. The contagion risk is eliminated instantly, though the permanent loss of the asset impacts long-term projections." }
        ]
    },
    "quest_survivor_memory_loss": {
        "title": "Neurological Degradation",
        "description": "An asset is experiencing severe cognitive decline, compromising operational efficiency.",
        "synopsis": "Unit Jamie-2 has begun failing to execute basic procedural tasks and cannot recall recent directives. The degradation appears neurological and irreversible.",
        "choices": [
            { "text": "Reassign to rudimentary labor", "consequences": "You assign the unit to basic hauling tasks. They require constant supervision, creating a net drag on efficiency, but continue to provide raw muscle." },
            { "text": "Execute expulsion", "consequences": "You deem the unit a liability and expel them. The settlement's overall efficiency stabilizes, though the raw labor pool decreases." },
            { "text": "Log as acceptable loss", "consequences": "You ignore the issue. The unit eventually causes a critical failure in the water filtration system due to a forgotten maintenance step." }
        ]
    },
    "quest_bunker_air_filter_upgrade": {
        "title": "Atmospheric Filtration Overhaul",
        "description": "The primary air scrubbers require a massive overhaul to maintain safe particulate levels.",
        "synopsis": "The carbon filters in the main ventilation shaft are fully saturated. Replacing them requires cannibalizing scarce activated charcoal and halting air circulation for twelve hours.",
        "choices": [
            { "text": "Execute full replacement", "consequences": "The new filters are installed. Ambient radiation drops to zero. The 12-hour air stoppage causes minor hypoxia in several units." },
            { "text": "Execute partial replacement", "consequences": "You replace only the intake filter. The air quality improves marginally, but long-term respiratory issues remain statistically probable." },
            { "text": "Deny replacement", "consequences": "You conserve the charcoal. The filters fail completely two weeks later, requiring an emergency evacuation of the lower levels." }
        ]
    },
    "quest_survivor_art_project": {
        "title": "Non-Essential Data Archival",
        "description": "Personnel are requesting resources to document pre-war cultural data.",
        "synopsis": "A coalition of units has requested paper, pigments, and operational downtime to record subjective memories of the pre-war era. This activity yields zero caloric or defensive value.",
        "choices": [
            { "text": "Authorize resource allocation", "consequences": "You provide the materials. The units complete the archival project. General efficiency increases slightly, though the consumed resources cannot be recovered." },
            { "text": "Deny allocation", "consequences": "You refuse the request. The materials are conserved for utilitarian purposes. A minor, persistent drop in operational focus is logged across the involved units." },
            { "text": "Mandate participation", "consequences": "You attempt to systematize the project, demanding rigid documentation. The units lose interest. The effort wastes time and produces unusable data." }
        ]
    },
    "quest_technology_sharing": {
        "title": "Meridian Compact Data Exchange",
        "description": "The Meridian Compact offers pre-war schematics in exchange for material resources.",
        "synopsis": "A Meridian envoy has presented a verifiable schematic for a high-efficiency water condenser. They demand a significant transfer of medical supplies and preserved food to release the data.",
        "choices": [
            { "text": "Execute data exchange", "consequences": "You transfer the supplies. The schematic is valid, and the new condenser permanently resolves your localized water deficit." },
            { "text": "Deny exchange", "consequences": "You refuse to part with the medical supplies. The envoy departs. You retain your immediate medical capacity, but the water deficit remains critical." },
            { "text": "Attempt data theft", "consequences": "You attempt to copy the schematic during the negotiation without payment. The envoy detects the breach, seals the data, and blacklists your settlement." }
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
