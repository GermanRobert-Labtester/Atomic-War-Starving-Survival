import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/moral_choice_quests.json"

with open(file_path, "r") as f:
    lines = f.readlines()

new_content = """    {
      "id": "quest_moral_dead_suicide", "display_name": "The Self-Terminated Unit", "category": "dead",
      "trigger": "You discover a casualty that executed its own exit protocol.",
      "discovery": "A body in a sealed room. The mechanism of termination is clear and close at hand. A paper document rests nearby, weighted by a stone.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute burial with document", "moral_delta": 14, "empathy_delta": 2, "outcome_text": "You inter the unit and the unread document. Their operational logic, whatever it was, is concluded.", "epitaph": "Interred self-terminated unit with documentation. Operational logic concluded." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You leave the room sealed. The termination requires no further processing.", "epitaph": "Bypassed self-terminated unit. Termination required no processing." },
        { "label": "Log as failure", "moral_delta": -12, "empathy_delta": 0, "outcome_text": "You formally record the unit as a failure of resilience metrics. The data is available to nearby observers.", "epitaph": "Logged unit as resilience failure. Data observed by local assets." },
        { "label": "Requisition staging kit", "moral_delta": -10, "empathy_delta": 0, "outcome_text": "The termination was well-supplied. You strip the kit for your own ongoing operations.", "epitaph": "Requisitioned staging kit from terminated unit. Supplies integrated into ongoing operations." }
      ]
    },
    {
      "id": "quest_moral_dead_massacre", "display_name": "The Liquidation Zone", "category": "dead",
      "trigger": "You enter a zone of total asset liquidation.",
      "discovery": "Dozens of casualties distributed across a sector. The posture of the remains indicates rapid, coordinated liquidation by an organized force.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Execute mass burial", "moral_delta": 22, "empathy_delta": 2, "outcome_text": "You expend two full daylight cycles digging. The caloric cost is severe, but the biological hazard is entirely neutralized.", "epitaph": "Executed mass burial over two cycles. Neutralized sector biohazard." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You route around the primary hazard zones and leave the liquidation site as found.", "epitaph": "Bypassed liquidation zone. Maintained route integrity." },
        { "label": "Execute systematic extraction", "moral_delta": -18, "empathy_delta": 0, "outcome_text": "You strip the sector of all remaining material assets. The yield is high, though decontamination will be extensive.", "epitaph": "Executed systematic extraction of liquidation zone. High material yield acquired." },
        { "label": "Execute thermal sterilization", "moral_delta": -15, "empathy_delta": 0, "outcome_text": "You ignite the sector. The smoke column is highly visible and draws immediate reconnaissance from neighboring factions.", "epitaph": "Executed thermal sterilization of liquidation zone. Drew faction reconnaissance." }
      ]
    },
    {
      "id": "quest_moral_trust_fire", "display_name": "The Unverified Contact", "category": "trust",
      "trigger": "An unverified asset requests proximity to your heat source.",
      "discovery": "An unknown unit stops at the perimeter of your thermal radius. They display empty hands. 'Thermal sharing requested,' they state.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Authorize proximity", "moral_delta": 10, "empathy_delta": 2, "outcome_text": "You permit entry. They transfer a portion of their caloric reserves and provide updated topographical data for the northern route.", "epitaph": "Authorized thermal proximity. Acquired updated topographical data." },
        { "label": "Authorize at maximum range", "moral_delta": 5, "empathy_delta": 1, "outcome_text": "You assign them the furthest functional radius. Both units maintain readiness. No data is exchanged.", "epitaph": "Authorized maximum-range thermal proximity. Maintained combat readiness." },
        { "label": "Deny proximity", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You refuse entry. The unit departs, returning to suboptimal thermal conditions.", "epitaph": "Denied thermal proximity. Contact returned to suboptimal conditions." },
        { "label": "Execute preemptive strike", "moral_delta": -8, "empathy_delta": 0, "outcome_text": "You engage immediately. The unit displays higher-than-expected combat readiness despite thermal degradation.", "epitaph": "Executed preemptive strike on unverified contact. Contact demonstrated high readiness." }
      ]
    },
    {
      "id": "quest_moral_trust_wounded", "display_name": "The Compromised Unknown", "category": "trust",
      "trigger": "An unknown asset requests medical intervention on a clear route.",
      "discovery": "A unit lies immobile in the transit path, actively losing fluid. They signal for assistance. The total lack of cover suggests a potential ambush vector.",
      "location_id": "", "min_day": 0, "max_day": 0,
      "choices": [
        { "label": "Provide comprehensive aid", "moral_delta": 14, "empathy_delta": 2, "outcome_text": "You expend medical supplies and carry their mass. They integrate into your operation and provide ongoing tactical analysis.", "epitaph": "Provided comprehensive aid to compromised unknown. Unit integrated into operations." },
        { "label": "Provide basic triage", "moral_delta": 7, "empathy_delta": 1, "outcome_text": "You apply standard coagulation protocols and direct them to the nearest known medical facility.", "epitaph": "Provided basic triage. Directed unit to medical facility." },
        { "label": "Bypass", "moral_delta": 0, "empathy_delta": 0, "outcome_text": "You maintain a wide perimeter and proceed. The signaling ceases shortly after.", "epitaph": "Bypassed compromised unknown. Signaling ceased." },
        { "label": "Extract immediate assets", "moral_delta": -10, "empathy_delta": 0, "outcome_text": "You detach their supply pack and proceed. The ambush, if one existed, does not trigger.", "epitaph": "Extracted assets from compromised unknown. Evaded potential ambush vector." }
      ]
    },
    {
"""

start_idx = 555
end_idx = 604
new_lines = new_content.splitlines(True)

lines[start_idx:end_idx] = new_lines

with open(file_path, "w") as f:
    f.writelines(lines)
