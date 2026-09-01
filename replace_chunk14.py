import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/moral_choice_quests.json"

with open(file_path, "r") as f:
    lines = f.readlines()

new_content = """    {
      "id": "quest_moral_trust_merchant", "display_name": "The Mobile Logistics Node", "category": "trust",
      "trigger": "An independent logistics asset offers material exchange.",
      "discovery": "A scavenger operating a manual transport cart signals a halt. 'Exchange rates are favorable today,' they state, keeping one hand on their sidearm.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute standard exchange", "moral_delta": 8, "empathy_delta": 2, "outcome_text": "You meet the stated caloric price without friction. The asset provides the coordinates of a stable trading hub as a bonus.", "epitaph": "Executed standard exchange. Acquired coordinates for stable logistics hub." },
        { "label": "Negotiate deficit", "moral_delta": 4, "empathy_delta": 1, "outcome_text": "You aggressively contest the valuation. The asset yields slightly to expedite the transaction.", "epitaph": "Negotiated material deficit. Secured minor caloric advantage." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You signal refusal. The transport cart resumes its operational route.", "epitaph": "Bypassed mobile logistics node. Conserved material." },
        { "label": "Requisition inventory", "moral_delta": -6, "empathy_delta": 0, "outcome_text": "You initiate hostiles and seize the cart. The asset's distress signals are ignored by the sector.", "epitaph": "Requisitioned logistics inventory via force. Sector ignored distress signals." }
      ]
    },
    {
      "id": "quest_moral_trust_child", "display_name": "The Unattached Juvenile", "category": "trust",
      "trigger": "You encounter a pre-adolescent asset separated from its unit.",
      "discovery": "A small juvenile occupies a structural ruin, emitting low-volume distress vocalizations. 'My primary attachment figure is absent,' they report.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Reunite with unit", "moral_delta": 16, "empathy_delta": 2, "outcome_text": "You expend significant operational time locating their camp. The family unit registers a permanent positive standing for your faction.", "epitaph": "Reunited juvenile with primary unit. Secured positive faction standing." },
        { "label": "Transfer to local authority", "moral_delta": 8, "empathy_delta": 1, "outcome_text": "You deliver the asset to a functioning settlement's intake officer. The officer logs your ID.", "epitaph": "Transferred unattached juvenile to settlement intake. Identification logged." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You lack the caloric overhead to manage dependents. You proceed. The vocalizations fade with distance.", "epitaph": "Bypassed unattached juvenile. Conserved caloric overhead." },
        { "label": "Re-allocate asset", "moral_delta": -12, "empathy_delta": 0, "outcome_text": "You transfer the juvenile to an unsanctioned labor camp. The asset comprehends the destination prior to arrival.", "epitaph": "Re-allocated juvenile to labor camp. Subject comprehended destination." }
      ]
    },
    {
      "id": "quest_moral_trust_deserter", "display_name": "The AWOL Combatant", "category": "trust",
      "trigger": "An AWOL combatant requests concealment from their unit.",
      "discovery": "A combatant lacking standard insignia emerges from cover, raising empty hands. 'I abandoned my post. Do not broadcast my coordinates,' they request.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Maintain concealment", "moral_delta": 12, "empathy_delta": 2, "outcome_text": "You omit their presence from your logs. In exchange, the combatant provides advanced weapon maintenance training.", "epitaph": "Maintained concealment for AWOL combatant. Acquired weapon maintenance data." },
        { "label": "Process tactical data", "moral_delta": 6, "empathy_delta": 1, "outcome_text": "You extract their unit's patrol routes and deployment status before allowing them to pass.", "epitaph": "Processed tactical data from AWOL combatant prior to release." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "Their factional disputes are outside your parameters. You proceed without engagement.", "epitaph": "Bypassed AWOL combatant. Ignored factional dispute." },
        { "label": "Broadcast coordinates", "moral_delta": -9, "empathy_delta": 0, "outcome_text": "You flag their location for the pursuing patrol. You receive a standard ration chit as compensation.", "epitaph": "Broadcasted AWOL combatant coordinates. Received standard ration chit." }
      ]
    },
    {
      "id": "quest_moral_trust_woman", "display_name": "The Data Broker", "category": "trust",
      "trigger": "A high-awareness asset offers classified intelligence.",
      "discovery": "A survivor maintains a secure perimeter over a sealed courier bag. 'My threat assessment categorizes you as stable,' they state. 'I hold actionable intelligence.'",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Process intelligence", "moral_delta": 18, "empathy_delta": 2, "outcome_text": "The data details hostile rotation schedules for the eastern sector. The broker integrates into your logistics network.", "epitaph": "Processed actionable intelligence. Integrated broker into logistics network." },
        { "label": "Verify data integrity", "moral_delta": 9, "empathy_delta": 1, "outcome_text": "You cross-reference the intelligence before acting. The verified portions prove highly valuable.", "epitaph": "Verified data integrity. Extracted value from confirmed intelligence." },
        { "label": "Decline transaction", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You refuse the operational risk. The broker seals their courier bag and departs.", "epitaph": "Declined intelligence transaction. Avoided operational risk." },
        { "label": "Execute forced extraction", "moral_delta": -14, "empathy_delta": 0, "outcome_text": "You attempt to secure the bag by force. The broker evades, and your ID is flagged on regional hostile lists.", "epitaph": "Attempted forced extraction of intelligence. Identification flagged regionally." }
      ]
    },
"""

start_idx = 604
end_idx = 652
new_lines = new_content.splitlines(True)

lines[start_idx:end_idx] = new_lines

with open(file_path, "w") as f:
    f.writelines(lines)
