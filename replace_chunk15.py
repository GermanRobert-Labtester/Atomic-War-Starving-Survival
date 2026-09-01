import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/moral_choice_quests.json"

with open(file_path, "r") as f:
    lines = f.readlines()

new_content = """    {
      "id": "quest_moral_trust_soldier", "display_name": "The Pre-War Combatant", "category": "trust",
      "trigger": "An aged combatant performs weapon maintenance and offers data.",
      "discovery": "An older unit in degraded military textiles strips a pre-exchange rifle with mechanical precision. 'Your operational posture is flawed,' they state without looking up.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Integrate tactical data", "moral_delta": 15, "empathy_delta": 2, "outcome_text": "You drill urban clearing tactics until nightfall. Their mechanical memory is flawless. They join your movement the next day.", "epitaph": "Integrated pre-war clearing tactics. Acquired veteran combatant asset." },
        { "label": "Extract summary data", "moral_delta": 8, "empathy_delta": 1, "outcome_text": "You accept a rapid briefing on sight alignment and proceed. The combatant correctly assesses you as a short-term asset.", "epitaph": "Extracted summary tactical briefing. Combatant retained operational independence." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "Your current posture has ensured survival thus far. You decline the overhead of re-training.", "epitaph": "Bypassed pre-war combatant. Relied on existing operational posture." },
        { "label": "Challenge competency", "moral_delta": -11, "empathy_delta": 0, "outcome_text": "You dismiss their methods as obsolete. Their resulting physical demonstration proves otherwise.", "epitaph": "Challenged veteran competency. Sustained physical correction." }
      ]
    },
    {
      "id": "quest_moral_trust_runaway", "display_name": "The Pursued Asset", "category": "trust",
      "trigger": "A high-stress asset seeks immediate concealment from pursuers.",
      "discovery": "An unarmed juvenile breaches your perimeter. 'Tracker units are approaching. I am a designated labor asset. Conceal me,' they demand.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute concealment", "moral_delta": 15, "empathy_delta": 2, "outcome_text": "You divert the pursuers with false telemetry. The asset departs before dawn, leaving a high-caloric ration on your pack.", "epitaph": "Executed concealment for pursued asset. Acquired high-caloric ration." },
        { "label": "Authorize minimal cover", "moral_delta": 8, "empathy_delta": 1, "outcome_text": "You allow them to utilize your blind spot but deny active misdirection. The pursuers eventually bypass the sector.", "epitaph": "Authorized passive cover for pursued asset. Sector bypassed by trackers." },
        { "label": "Deny entry", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You refuse to assume their operational debt. They flee into the adjacent ruin. The trackers follow shortly after.", "epitaph": "Denied entry to pursued asset. Trackers proceeded through sector." },
        { "label": "Execute apprehension", "moral_delta": -12, "empathy_delta": 0, "outcome_text": "You detain the asset and transfer custody to the pursuers. The faction logs a bounty credit to your ID.", "epitaph": "Executed apprehension of pursued asset. Bounty credit logged." }
      ]
    },
    {
      "id": "quest_moral_trust_silent", "display_name": "The Non-Verbal Asset", "category": "trust",
      "trigger": "A non-verbal unit signals an unknown intent.",
      "discovery": "A unit tracking parallel to your route maintains visual contact but refuses vocalization. They perform a complex series of hand signals.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Mirror signals and observe", "moral_delta": 10, "empathy_delta": 2, "outcome_text": "You return neutral gestures. They establish a forward scouting position and clear three ambush points over the next transit cycle.", "epitaph": "Mirrored non-verbal asset signals. Acquired forward scouting capability." },
        { "label": "Maintain passive observation", "moral_delta": 5, "empathy_delta": 1, "outcome_text": "You monitor their vector without engagement. They eventually break contact at a major intersection.", "epitaph": "Maintained passive observation of non-verbal asset. Contact broken cleanly." },
        { "label": "Bypass aggressively", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You increase pace and alter route to lose them. Unknown variables are unacceptable risks.", "epitaph": "Aggressively bypassed non-verbal asset. Route security maintained." },
        { "label": "Initiate hostiles", "moral_delta": -8, "empathy_delta": 0, "outcome_text": "You designate their tracking as hostile intent. They evade your initial volley with extreme efficiency and vanish.", "epitaph": "Initiated hostiles on non-verbal asset. Target evaded successfully." }
      ]
    },
    {
      "id": "quest_moral_trust_signal", "display_name": "The Rhythmic Distress", "category": "trust",
      "trigger": "You detect a rhythmic mechanical distress signal.",
      "discovery": "Metallic impacts echo from a sealed subterranean vent. The rhythm is mathematical: three strikes, pause, three strikes. It is not environmental.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute extraction", "moral_delta": 15, "empathy_delta": 2, "outcome_text": "You expend heavy labor to pry the grate. The trapped asset shares half their subterranean salvage yield in compensation.", "epitaph": "Executed heavy extraction on subterranean asset. Acquired salvage yield." },
        { "label": "Confirm presence", "moral_delta": 8, "empathy_delta": 1, "outcome_text": "You signal back to confirm their coordinates, then mark the location for better-equipped logistics units.", "epitaph": "Confirmed subterranean presence. Marked coordinates for extraction teams." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You lack the leverage tools. The acoustic pattern fades as you clear the grid.", "epitaph": "Bypassed subterranean distress signal. Lacked leverage tools." },
        { "label": "Exploit extraction zone", "moral_delta": -9, "empathy_delta": 0, "outcome_text": "You establish a sniper blind to ambush whatever rescue team responds to the noise. Your yield is substantial.", "epitaph": "Exploited extraction zone for ambush. Salvage yield substantial." }
      ]
    },
    {
      "id": "quest_moral_trust_borrower", "display_name": "The Leverage Request", "category": "trust",
      "trigger": "An unknown unit requests temporary acquisition of your primary tool.",
      "discovery": "A scavenger requests your prybar. 'A cache is accessible. My unit requires leverage. The statistical return is high,' they state, maintaining eye contact.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Authorize transfer", "moral_delta": 11, "empathy_delta": 2, "outcome_text": "The tool is returned within 48 hours, upgraded with synthetic grips. They also provide coordinates to a secondary, smaller cache.", "epitaph": "Authorized temporary tool transfer. Tool upgraded. Secondary cache acquired." },
        { "label": "Transfer secondary tool", "moral_delta": 4, "empathy_delta": 1, "outcome_text": "You hand over a degraded spare. The unit logs the lack of trust but executes their operation.", "epitaph": "Transferred secondary degraded tool. Operation executed." },
        { "label": "Deny request", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "Your equipment remains on your person. The unit logs the denial and seeks other options.", "epitaph": "Denied tool transfer request. Equipment security maintained." },
        { "label": "Track and exploit", "moral_delta": -13, "empathy_delta": 0, "outcome_text": "You lend the tool, track their vector, and appropriate the entire cache via force. The tool is recovered.", "epitaph": "Tracked tool transfer. Exploited cache via force. Tool recovered." }
      ]
    },
    {
      "id": "quest_moral_trust_messenger", "display_name": "The Courier's Protocol", "category": "trust",
      "trigger": "A failing unit attempts a data handoff.",
      "discovery": "A unit approaches your perimeter and collapses. They present a sealed polymer envelope. 'Deliver to the Northern Node. Do not break the seal,' they command, before expiring.",
      "location_id": "", "min_day": 200, "max_day": 0,
      "choices": [
        { "label": "Execute delivery protocol", "moral_delta": 20, "empathy_delta": 4, "outcome_text": "You carry the envelope through hostile sectors. The Northern Node processes the intact seal and grants you permanent high-priority access.", "epitaph": "Executed delivery protocol. Seal intact. Granted high-priority node access." },
        { "label": "Break seal and deliver", "moral_delta": 8, "empathy_delta": 2, "outcome_text": "You analyze the data before delivery. The receiving node detects the breach and processes the intel with zero reward.", "epitaph": "Breached courier seal. Delivered compromised intel. Zero reward acquired." },
        { "label": "Log as inventory", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You pack the envelope. It remains unopened, occupying mass in your inventory without providing utility.", "epitaph": "Logged courier envelope as inventory. Utility remains zero." },
        { "label": "Liquidate data", "moral_delta": -15, "empathy_delta": 0, "outcome_text": "You sell the encrypted envelope to a local data broker. The payout covers three days of rations.", "epitaph": "Liquidated courier data. Acquired three days caloric rations." }
      ]
    },
    {
      "id": "quest_moral_env_scavenger_child", "display_name": "The Juvenile Forager", "category": "share",
      "trigger": "A micro-asset conducts inefficient salvage operations.",
      "discovery": "A pre-adolescent asset with severe caloric deficit is attempting to extract metals from a collapsed concrete slab. Their mechanical efficiency is near zero.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute joint extraction", "moral_delta": 12, "empathy_delta": 2, "outcome_text": "You provide caloric support and apply leverage to the slab. The extracted metals are divided. The juvenile transfers a non-functional gear as 'luck.'", "epitaph": "Executed joint extraction with juvenile forager. Extracted metals divided." },
        { "label": "Transfer minimal calories", "moral_delta": 5, "empathy_delta": 1, "outcome_text": "You transfer a low-value ration. The juvenile consumes it without ceasing their inefficient extraction attempt.", "epitaph": "Transferred minimal calories to juvenile forager. Inefficient extraction continued." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You inform them the structure is unstable. They ignore the data. You proceed.", "epitaph": "Bypassed juvenile forager. Unstable structure warning ignored." },
        { "label": "Requisition extracted assets", "moral_delta": -8, "empathy_delta": 0, "outcome_text": "You appropriate the minor scrap they have already uncovered. The asset lacks the mass to contest the seizure.", "epitaph": "Requisitioned extracted assets from juvenile forager. Contest impossible." }
      ]
    },
    {
      "id": "quest_moral_env_buried_letters", "display_name": "The Archival Documents", "category": "listen",
      "trigger": "An older unit requests data processing for pre-war texts.",
      "discovery": "An elderly unit with severe ocular degradation holds a bundle of pre-exchange paper documents. 'I require audio translation of these civilian texts,' they state.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Process complete archive", "moral_delta": 10, "empathy_delta": 2, "outcome_text": "You read the civilian correspondence aloud. The data is entirely non-tactical. The older unit logs the audio and confirms 'historical accuracy.'", "epitaph": "Processed complete archive of civilian texts. Confirmed historical accuracy." },
        { "label": "Process data sample", "moral_delta": 4, "empathy_delta": 1, "outcome_text": "You translate two documents to confirm their non-tactical nature. The unit accepts the sample and terminates the request.", "epitaph": "Processed sample of civilian texts. Request terminated." },
        { "label": "Deny processing request", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You refuse to expend operational time on non-actionable data. The unit secures the documents.", "epitaph": "Denied processing request for non-actionable data." },
        { "label": "Requisition paper assets", "moral_delta": -5, "empathy_delta": 0, "outcome_text": "You seize the documents for use as combustion fuel. The elderly unit cannot mount a defense.", "epitaph": "Requisitioned archival documents for combustion fuel." }
      ]
    },
    {
      "id": "quest_moral_env_shelter_refugee", "display_name": "The Environmental Refugee", "category": "trust",
      "trigger": "An unverified unit requests thermal sheltering during an extreme weather event.",
      "discovery": "A severe pressure drop is imminent. An unknown asset arrives at your perimeter. 'Requesting temporary structural cover. No aggressive intent,' they state.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Authorize structural entry", "moral_delta": 8, "empathy_delta": 2, "outcome_text": "You permit entry. The unit powers down in a corner. At dawn, they have departed, leaving a bundle of dry combustion fuel as payment.", "epitaph": "Authorized structural entry for refugee. Acquired combustion fuel payment." },
        { "label": "Authorize perimeter cover", "moral_delta": 3, "empathy_delta": 1, "outcome_text": "You allow them to utilize the exterior structural overhang. They survive the thermal drop and depart at first light.", "epitaph": "Authorized perimeter cover for refugee. Thermal drop survived." },
        { "label": "Deny cover", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You secure your perimeter. The unit is forced back into the hazard zone. The probability of their survival is statistically low.", "epitaph": "Denied cover to environmental refugee. Survival probability low." },
        { "label": "Execute perimeter defense", "moral_delta": -7, "empathy_delta": 0, "outcome_text": "You present lethal force to ensure they do not compromise your structure. They retreat into the lethal weather.", "epitaph": "Executed perimeter defense. Forced refugee into lethal weather." }
      ]
    },
    {
      "id": "quest_moral_env_dead_explorer", "display_name": "The Failed Surveyor", "category": "dead",
      "trigger": "You locate the remains of a surveyor with intact cartographic data.",
      "discovery": "A unit rests against a wall, expired from dehydration. A logbook sits open. 'Grid 47 invalid. Fluid zero. Transmit data to dependent unit,' the final entry reads.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Extract data and inter", "moral_delta": 8, "empathy_delta": 2, "outcome_text": "You bury the unit and secure the logbook. The invalid grid data is logged. You will attempt transmission if the dependent unit is located.", "epitaph": "Extracted cartographic data and interred surveyor. Dependent unit pending." },
        { "label": "Acknowledge and proceed", "moral_delta": 4, "empathy_delta": 1, "outcome_text": "You confirm the invalidity of Grid 47 for your own routing and leave the remains in situ.", "epitaph": "Acknowledged invalid grid data. Left surveyor remains in situ." },
        { "label": "Extract cartography only", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You strip the map section of the logbook and leave the personal data behind. The map is flawed, but provides baseline geography.", "epitaph": "Extracted baseline cartography. Discarded personal surveyor data." },
        { "label": "Execute full requisition", "moral_delta": -4, "empathy_delta": 0, "outcome_text": "You strip the unit of all functional gear, including footwear and textiles. The biological frame is left to the elements.", "epitaph": "Executed full requisition of surveyor assets. Biological frame discarded." }
      ]
    },
    {
      "id": "quest_moral_env_wounded_scavenger", "display_name": "The Degraded Forager", "category": "comfort",
      "trigger": "A scavenger with a severe structural injury refuses to relocate.",
      "discovery": "A unit sits in a debris field, applying pressure to a severe laceration. 'Do not engage. Operational capability is zero. Awaiting termination,' they state.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute medical intervention", "moral_delta": 10, "empathy_delta": 2, "outcome_text": "You expend medical supplies to stabilize the laceration. The unit processes the intervention and provides actionable safe-routing data for the adjacent block.", "epitaph": "Executed medical intervention on degraded forager. Acquired safe-routing data." },
        { "label": "Transfer medical supplies", "moral_delta": 5, "empathy_delta": 1, "outcome_text": "You throw a sterilized bandage pack to their coordinates. They initiate self-repair protocols.", "epitaph": "Transferred medical supplies to degraded forager. Self-repair initiated." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You obey the directive and do not engage. Their termination is highly probable.", "epitaph": "Bypassed degraded forager per directive. Termination probable." },
        { "label": "Requisition inventory", "moral_delta": -6, "empathy_delta": 0, "outcome_text": "You strip their supply pack. They lack the structural integrity to mount a defense and observe the requisition silently.", "epitaph": "Requisitioned inventory from degraded forager. Defense capability zero." }
      ]
    }
  ]
}
"""

start_idx = 652
lines_len = len(lines)
new_lines = new_content.splitlines(True)

lines[start_idx:lines_len] = new_lines

with open(file_path, "w") as f:
    f.writelines(lines)
