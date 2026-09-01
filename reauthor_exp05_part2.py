import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_expansion_05.json"

with open(file_path, "r") as f:
    data = json.load(f)

updates = {
    "quest_scavenger_alliance": {
        "title": "Local Salvage Conglomeration",
        "description": "An independent salvage team requests a formal integration of operations.",
        "synopsis": "A semi-organized group of local scavengers has proposed merging logistics. They offer raw labor in exchange for structural protection and medical overhead.",
        "choices": [
            { "text": "Execute formal integration", "consequences": "You absorb their numbers. The caloric strain spikes, but the immediate influx of raw materials stabilizes winter projections." },
            { "text": "Deny integration", "consequences": "You refuse the overhead. The team attempts independent operations and is subsequently liquidated by regional hostiles." },
            { "text": "Exploit as disposable assets", "consequences": "You accept the merger but assign the new units exclusively to high-rad zones. The material yield is massive; the units expire shortly after." }
        ]
    },
    "quest_farmers_bargain": {
        "title": "Agricultural Output Deficit",
        "description": "Hydroponic yields have dropped below baseline. A local agrarian collective offers seeds for security.",
        "synopsis": "A nearby agrarian outpost claims to have radiation-resistant seed stock. They require armed details to secure their perimeter in exchange for a percentage of the yield.",
        "choices": [
            { "text": "Deploy security detail", "consequences": "You commit armed units to their perimeter. The resulting crop yield offsets the caloric cost of the security detail." },
            { "text": "Deny support", "consequences": "You keep your arms local. The agrarian outpost is overrun within a week; the seed stock is lost to the sector." },
            { "text": "Requisition seed stock", "consequences": "You dispatch the armed detail to seize the seeds directly. The outpost is neutralized. The seeds are acquired without ongoing obligations." }
        ]
    },
    "quest_radio_signal": {
        "title": "Unidentified Encrypted Broadcast",
        "description": "The communication array has locked onto a repeating, encrypted transmission.",
        "synopsis": "A high-powered signal is broadcasting on a loop. It may contain coordinates to an intact pre-war cache, or it may be a standard hostile lure protocol.",
        "choices": [
            { "text": "Allocate processing cycles", "consequences": "You burn fuel to run the decryption subroutines. The resulting coordinates lead to a functional medical cache. The expenditure is justified." },
            { "text": "Ignore transmission", "consequences": "You save the fuel. The signal degrades and is eventually lost to background radiation." },
            { "text": "Broadcast response", "consequences": "You ping the source. The signal ceases immediately. Two days later, a heavily armed recon element probes your perimeter." }
        ]
    },
    "quest_medical_supply_run": {
        "title": "Pharmaceutical Acquisition",
        "description": "Medical reserves are insufficient to manage projected winter ailments. A high-risk acquisition is necessary.",
        "synopsis": "A ruined clinical facility in Sector 3 is confirmed to hold intact pharmaceutical stocks. The structure is heavily irradiated and structurally unsound.",
        "choices": [
            { "text": "Execute acquisition", "consequences": "The team returns with the pharmaceuticals. One unit sustains lethal structural trauma. A net gain in medical capacity versus a net loss in labor." },
            { "text": "Abort acquisition", "consequences": "You preserve the labor pool. The medical deficit remains, and projected winter attrition rates are adjusted upward." },
            { "text": "Contract independent operatives", "consequences": "You hire unaffiliated scavengers to clear the clinic. They deliver the supplies but retain a significant operational cut." }
        ]
    },
    "quest_survivor_leadership": {
        "title": "Command Structure Challenge",
        "description": "An asset is challenging current operational directives, creating inefficiencies.",
        "synopsis": "Unit Marcus-2 has begun openly questioning the caloric distribution models and labor assignments, causing measurable drops in shift productivity.",
        "choices": [
            { "text": "Re-assert command hierarchy", "consequences": "You enforce strict disciplinary protocols. Efficiency returns to baseline, though the unit continues to harbor unauthorized opinions." },
            { "text": "Execute expulsion", "consequences": "The unit is stripped of gear and expelled. The labor pool decreases, but operational cohesion reaches 100%." },
            { "text": "Re-assign to command", "consequences": "You integrate the unit into the logistics planning team. Their models prove mathematically sound, increasing overall output." }
        ]
    },
    "quest_raider_ambush": {
        "title": "Hostile Interception",
        "description": "An acquisition team has engaged a hostile element in transit.",
        "synopsis": "A surface team returning from Sector 4 reports active engagement with an armed hostile force. They request tactical authorization.",
        "choices": [
            { "text": "Authorize lethal force", "consequences": "The team engages and neutralizes the hostiles. Significant ammunition is expended, but the acquired materials are secured." },
            { "text": "Order tactical retreat", "consequences": "The team drops the heavy salvage to increase mobility and breaks contact. The personnel are preserved; the materials are lost." },
            { "text": "Authorize material concession", "consequences": "The team abandons half their load as a distraction and withdraws. A mathematically acceptable compromise between life and asset loss." }
        ]
    },
    "quest_bunker_defense": {
        "title": "Perimeter Readiness Drill",
        "description": "Current combat readiness metrics are theoretical. A physical stress test is required.",
        "synopsis": "Simulating a breach will verify the integrity of internal bulkheads and the response time of armed units. It will also consume operational time and minor resources.",
        "choices": [
            { "text": "Execute full simulation", "consequences": "The drill reveals critical flaws in Sector 2's firing lines. The flaws are corrected. Operational time is permanently lost." },
            { "text": "Execute theoretical review", "consequences": "You review the defensive models on paper. It saves calories, but leaves the physical execution unverified." },
            { "text": "Cancel simulation", "consequences": "You prioritize continuous labor over combat readiness. The perimeter remains untested." }
        ]
    },
    "quest_survivor_memorial": {
        "title": "Post-Termination Protocol",
        "description": "Units are requesting an allocation of time and resources to process recent casualties.",
        "synopsis": "Several assets are requesting a 'memorial' for recently terminated units. This requires suspending labor schedules and expending localized power for non-utilitarian purposes.",
        "choices": [
            { "text": "Authorize limited protocol", "consequences": "You allocate one hour of downtime. The units complete their psychological processing and return to labor with a minor efficiency boost." },
            { "text": "Deny protocol", "consequences": "You refuse to halt operations. The units remain at their posts, but error rates in complex tasks increase measurably." },
            { "text": "Mandate attendance", "consequences": "You enforce a mandatory processing session. The forced cessation of labor breeds resentment and fails to achieve the desired psychological reset." }
        ]
    },
    "quest_food_rationing": {
        "title": "Caloric Distribution Revision",
        "description": "Current reserves require an immediate downward revision of standard caloric intake.",
        "synopsis": "Winter projections indicate a 20% shortfall in grain reserves. A revised rationing model must be implemented immediately to prevent total depletion.",
        "choices": [
            { "text": "Implement baseline reduction", "consequences": "All units receive a 20% reduction. General fatigue increases, but the math balances. No units face immediate starvation." },
            { "text": "Implement tiered reduction", "consequences": "Non-essential units receive a 40% reduction; critical assets maintain standard intake. The non-essential pool suffers severe physical degradation." },
            { "text": "Acquire external assets", "consequences": "You authorize a raid on a weaker neighboring outpost. You secure sufficient calories to maintain the current distribution model." }
        ]
    },
    "quest_technology_scavenging": {
        "title": "High-Value Hardware Acquisition",
        "description": "A structurally compromised tower is confirmed to hold intact silicon architecture.",
        "synopsis": "An unstable ruin contains intact computing hardware. The retrieval requires units to navigate failing concrete at high elevation.",
        "choices": [
            { "text": "Deploy specialized team", "consequences": "The hardware is secured. One unit suffers a complex fracture from falling debris. The tech provides a net gain in automated system control." },
            { "text": "Deploy minimal team", "consequences": "A two-unit team retrieves only the most accessible components. The risk is minimized; the yield is suboptimal." },
            { "text": "Cancel acquisition", "consequences": "You deem the structural risk unacceptable. The hardware remains in the ruin until the tower inevitably collapses." }
        ]
    },
    "quest_medical_training": {
        "title": "Triage Proficiency Initiative",
        "description": "The primary medical officer requests operational time to instruct secondary units in basic triage.",
        "synopsis": "Distributing medical knowledge reduces the single-point-of-failure risk of the primary medic. It requires pulling active units from their standard shifts.",
        "choices": [
            { "text": "Authorize comprehensive instruction", "consequences": "Three units achieve basic medical proficiency. Mortality rates from minor trauma decrease, offsetting the initial labor loss." },
            { "text": "Authorize limited instruction", "consequences": "Only one unit is trained as a backup. The operational impact is minimal, but the systemic risk remains high." },
            { "text": "Deny instruction", "consequences": "You refuse to divert labor. The primary medic remains the sole point of failure for all biological repairs." }
        ]
    },
    "quest_survivor_skills_assessment": {
        "title": "Labor Optimization Audit",
        "description": "A comprehensive audit of unit capabilities is proposed to maximize efficiency.",
        "synopsis": "Current labor assignments are based on initial intake data. A full reassessment could uncover misallocated skills, but requires halting production for the duration of the audit.",
        "choices": [
            { "text": "Execute comprehensive audit", "consequences": "The audit reveals several inefficiencies. Reassigning units based on the new data results in a 12% increase in overall structural output." },
            { "text": "Execute targeted audit", "consequences": "You audit only the mechanical and medical teams. The minor reassignments provide a negligible boost to efficiency." },
            { "text": "Deny audit", "consequences": "Operations continue uninterrupted. Hidden inefficiencies remain permanently embedded in the labor structure." }
        ]
    },
    "quest_bunker_medical_bay": {
        "title": "Clinical Facility Expansion",
        "description": "The current medical bay lacks the physical space to process projected winter casualties.",
        "synopsis": "Expanding the clinic into an adjacent storage zone requires stripping the zone of its shelving and redirecting environmental controls to maintain sterilization protocols.",
        "choices": [
            { "text": "Authorize structural expansion", "consequences": "The clinic is expanded. The increased capacity allows for simultaneous surgical procedures, reducing overall mortality rates." },
            { "text": "Deny expansion", "consequences": "The clinic remains at capacity. Surplus casualties are treated in standard quarters, drastically increasing infection rates." },
            { "text": "Authorize rapid, unsanitized expansion", "consequences": "You clear the space quickly but fail to route proper environmental controls. The new beds are usable, but post-operative infection remains a constant hazard." }
        ]
    },
    "quest_survivor_romance": {
        "title": "Unauthorized Biological Bonding",
        "description": "Two units have established an exclusive biological and psychological bond outside operational parameters.",
        "synopsis": "Units Alex-3 and Jamie-4 are dedicating off-shift time to each other. This exclusive bonding can lead to localized favoritism and compromised decision-making during crisis events.",
        "choices": [
            { "text": "Log and monitor", "consequences": "You officially record the bond but take no action. The units maintain standard efficiency, though the risk of compromised crisis response remains active." },
            { "text": "Ignore the anomaly", "consequences": "You take no official notice. The bond continues, occasionally causing minor scheduling friction among other units." },
            { "text": "Enforce strict separation", "consequences": "You reassign the units to opposing shifts and separate sectors. The bond is broken, resulting in a temporary but severe drop in their individual output." }
        ]
    },
    "quest_fuel_crisis": {
        "title": "Combustible Liquid Deficit",
        "description": "Diesel reserves are nearing the zero-line. The primary generators will fail in 72 hours.",
        "synopsis": "Without a rapid influx of combustible liquids, the settlement will lose all thermal and electrical generation. A high-risk acquisition run is mandatory.",
        "choices": [
            { "text": "Execute high-risk acquisition", "consequences": "The team secures a functional fuel tanker. The casualty rate is high, but the settlement's power grid is secured for the fiscal quarter." },
            { "text": "Initiate emergency conservation", "consequences": "You shut down all non-essential power. The bunker goes dark and freezing. You stretch the remaining fuel, but the cold claims the weakest units." },
            { "text": "Liquidate assets for fuel", "consequences": "You sell a massive portion of your medical and ammunition reserves to a passing caravan in exchange for their fuel supply. You are powered, but defenseless." }
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
