import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/survivors.json"
with open(file_path, "r") as f:
    data = json.load(f)

def get_base_bio(profession):
    p = profession.lower()

    if any(x in p for x in ["medic", "surgeon", "nurse", "pharmacist", "therapist", "veterinarian", "caregiver", "counselor"]):
        return f"Pre-war biological or psychological maintenance personnel ({profession}). Retains specialized knowledge of physiological repair and resource-efficient triage. Evaluated as a high-value operational asset. Prioritizes caloric efficiency over patient comfort."

    if any(x in p for x in ["engineer", "mechanic", "electrician", "hvac", "plumber", "pump", "cnc", "telecomm", "radio", "architect", "builder", "carpenter", "foreman", "hazmat"]):
        return f"Pre-war infrastructure technician ({profession}). Demonstrates proficiency in material salvage and mechanical repair under severe resource constraints. Required for ongoing maintenance of core settlement life-support systems."

    if any(x in p for x in ["soldier", "military", "sniper", "guard", "security", "police", "lawman", "eod", "heavy artillery", "commander"]):
        return f"Former tactical or security personnel ({profession}). Possesses conditioned responses to kinetic threats and structural vulnerability assessment. High utility for perimeter defense and enforcement of localized caloric rationing."

    if any(x in p for x in ["botanist", "farmer", "composter", "fungus", "hunter", "scavenger", "scrapper", "scout", "scuba", "tunnel", "cave", "meteorologist", "sonar", "uav"]):
        return f"Resource acquisition or environmental specialist ({profession}). Trained in identifying and exploiting marginal caloric inputs in high-radiation environments. Essential for extending settlement survival thresholds."

    if any(x in p for x in ["bureaucrat", "executive", "logistics", "supply", "politician", "reporter", "historian", "archivist", "teacher", "storyteller", "anchor", "watchmaker", "chef", "cook", "tailor", "athlete", "courier", "firefighter"]):
        return f"Pre-war administrative or civilian specialist ({profession}). Primary pre-war skillsets lack direct survival utility. Reassigned to general labor and logistical tracking. Assessed for potential re-specialization based on physical durability."

    if any(x in p for x in ["burglar", "arsonist", "convict", "rebel", "addict", "cult", "hermit", "nomad", "outsider", "mutant", "night watch", "bouncer"]):
        return f"Unregistered or irregular biological unit ({profession}). Demonstrates high adaptability to degraded societal structures. Utility assessed strictly on labor output and risk of insubordination. Monitored for resource diversion."

    if any(x in p for x in ["child", "patient"]):
        return f"Non-contributing biological unit ({profession}). Generates zero operational labor output while consuming a fractional caloric allowance. Maintained primarily for long-term demographic viability."

    if any(x in p for x in ["mother", "father", "parent", "neighbor", "preacher", "priest", "monk", "optimist"]):
        return f"Civilian unit identified primarily by relational or ideological traits ({profession}). Assigned to general labor pool. Psychological baseline currently stable, but requires observation for counter-productive sentimentality."

    return f"Unclassified biological unit ({profession}). Processed through standard intake. Assigned to general labor pool pending specialized aptitude testing."

for s in data.get("survivors", []):
    prof = s.get("profession", "Laborer")
    traits = s.get("traitIds", [])

    bio = get_base_bio(prof)
    if traits:
        trait_names = [t.replace("trait_", "").replace("_", " ") for t in traits]
        bio += f" Evaluation markers: {', '.join(trait_names)}."

    s["bio"] = bio

with open(file_path, "w") as f:
    json.dump(data, f, indent=4)
