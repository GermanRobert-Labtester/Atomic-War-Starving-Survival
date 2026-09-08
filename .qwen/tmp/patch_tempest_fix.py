# Round 5 — CANON FIX for content committed in round 4 (04806918).
#
# VIOLATION: door_encounter_tempest_barograph_seller characterized faction_the_tempest as
# storm-chasers ("Kessa Vane, storm-chaser (The Tempest)", "fuel cells for the chase rig").
# AUTHORITATIVE CANON — Assets/StreamingAssets/Data/currents.json (live via CurrentsCatalog /
# MusterHostSession / CurrentsRosterWidget):
#   display_name "The Tempest" | alignment conditional | home_region the_spine | is_active false
#   wants:  maintenance_time, readings, a_presented_count
#   offers: machine_log_access, scheduled_q, archive_proof
#   quote:  "Service shall be metered, and the meter shall be read."
#   access: "The Tempest is not alive and has no preferences. It serves, it meters, and it waits
#            for a human to read the meter."
# Corroborated by the pre-existing door_encounter_verdict_relay_repair (a Tempest technician in
# insulated gauntlets servicing an exterior conduit array).
#
# FIX: keep the encounter and its gameplay value (storm-track intel for fuel cells) but re-ground
# it: the visitor is the human the meter waits for, and the barograph drum is her OWN off-contract
# log, which the Tempest's ledger has no column for. This turns the contradiction into canon depth.
# visitorFaction stays faction_the_tempest (she is on Tempest business); the storm-chasing framing
# and the "chase rig" are removed.

TEMPEST_FIX = {
  "encounterId": "door_encounter_tempest_barograph_seller",
  "visitorName": "Meter-Reader Kessa Vane (The Tempest)",
  "description": "She comes to read the meter on your exterior conduit, because the meter is read on a schedule and the schedule is the entire reason for the visit. Under her other arm is a barograph drum that is not on any contract: two years of her own traces, inked nightly, showing the storms walking a fixed bearing like a patrol. The Tempest has never once mentioned weather. It meters service and waits for a human to read the meter. She is the human, and she has kept opinions the machine does not have.",
  "choices": [
    {
      "choiceId": "choice_buy_the_storm_record",
      "text": "Two fuel cells for the drum. The meter gets read either way; that part is not a transaction.",
      "outcomeDescription": "The copied trace goes up beside the hatch chart and, for the first time, the sky has a schedule. Your watch starts calling storms by number instead of by dread, and the first one she predicts lands inside four hours of her window. She writes the sale in a private book, because the Tempest's ledger has no column for weather and never will. The meter reading she files is accurate to the digit."
    },
    {
      "choiceId": "choice_decline_the_drum",
      "text": "Storms do not commute. Let her read the meter and keep her own book.",
      "outcomeDescription": "She reads the meter, stamps the card, and taps the drum once on the way back down the ladder: not offended, only filing it. Three weeks later a storm turns inside a window she could have sold you, and the watch rides it out on reflex and luck. Afterwards nobody says her name in the corridor. Not saying it is its own kind of saying."
    }
  ]
}
