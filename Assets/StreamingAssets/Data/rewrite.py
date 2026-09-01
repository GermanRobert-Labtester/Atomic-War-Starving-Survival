import json
import random
import re

with open("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_faction_branching.json", "r") as f:
    data = json.load(f)

# Faction Themes
themes = {
    "bone_pickers": {"name": "The Bone Pickers", "danger": "The Guild doesn't forgive waste."},
    "blood_tithe": {"name": "The Blood Tithe", "danger": "The Raiders demand blood."},
    "drought_cartel": {"name": "The Drought Cartel", "danger": "The Barons thirst for control."},
    "martial_law": {"name": "Martial Law", "danger": "The Coalition sees leniency as treason."},
    "generic": {"name": "Command", "danger": "Disobedience means death in the ash."}
}

def get_theme(quest_id):
    for key in themes.keys():
        if key in quest_id:
            return themes[key]
    return themes["generic"]

def generate_texts(quest_id, briefing, stage_idx):
    theme = get_theme(quest_id)

    # We want calculated pragmatism for the evil choice:
    advance_texts = [
        "Proceed. This is a liability we cannot afford.",
        "Do it. Sentimentality won't keep us alive; resources will.",
        "Execute the order. The survival of the many outweighs the few.",
        "Follow protocol. We need what they have more than they do.",
        "Take it all. If we don't, the wasteland will just claim them anyway.",
        "Complete the task. In this ash, morality is a luxury we can't afford.",
        "Enforce the mandate. Weakness here means death for us tomorrow.",
        "We do what we must. Leave the guilt for those who survive the winter."
    ]

    # We want danger and moral weight for the good choice:
    abort_texts = [
        "Stand down. If we do this, we're no better than the monsters in the ash.",
        f"Stop. I won't cross this line, even if {theme['danger'].lower()}",
        "Refuse the order. Let them be. We take the consequences.",
        "Lower your weapons. There has to be another way to survive.",
        "Hold. I'm countermanding the order. Prepare for backlash.",
        "Turn away. Some things cost more than they pay out.",
        "Defy them. We retain our humanity, whatever the price.",
        "Abort. I'd rather die with a clean conscience than live like this."
    ]

    # Stage texts for atmosphere
    atmospheres = [
        "The wind carries the bitter scent of ozone and desperation.",
        "A heavy silence falls over the squad. The click of a safety being disengaged sounds like a thunderclap.",
        "Dust swirls around your boots. The eyes of your squad lock onto you, waiting for the command.",
        "The Geiger counter clicks rhythmically, a ticking clock against your conscience.",
        "Shadows stretch long in the harsh light. The reality of the wasteland presses down on you.",
        "The air is thick with tension. Every second of hesitation is a dangerous gamble.",
        "The cold logic of the faction's orders hangs in the air, venomous and clear."
    ]

    random.seed(quest_id) # deterministic

    adv = random.choice(advance_texts)
    ab = random.choice(abort_texts)
    stg = random.choice(atmospheres) + f" {theme['name']} expects results, not hesitation."

    return adv, ab, stg

for q in data['quests']:
    if 'faction' not in q.get('type', ''):
        continue

    stage_num = q['id'].split('_')[-1]
    theme = get_theme(q['id'])

    # Better display names based on ID
    base_name = " ".join([w.capitalize() for w in q['id'].split('_')[1:-1]])
    if base_name == "":
        base_name = "Operation"

    q['display_name'] = f"{base_name} - Stage {int(stage_num)}"

    adv, ab, stg = generate_texts(q['id'], q['briefing'], stage_num)

    if q['stages']:
        q['stages'][0]['text'] = stg

    for c in q['choices']:
        if 'advance' in c['id']:
            c['text'] = adv
        elif 'abort' in c['id']:
            c['text'] = ab

with open("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/quests_faction_branching.json", "w") as f:
    json.dump(data, f, indent=2)

print("Done rewrite 2")
