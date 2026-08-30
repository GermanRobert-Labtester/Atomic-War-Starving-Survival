# Plan 107 — Radio Distress Signals Expansion (5 → 20 interceptable distress signals)

## Goal (2 lines)
Expand `radio_distress_signals.json` from 5 verified signals to 20. The
radio distress signal system (confirmed live via ContentUtilizationScanner)
defines interceptable distress broadcasts the player traces over multiple
days — each signal has message fragments with increasing clarity, an outcome
type, and revealed locations/items/knowledge. 5 signals is too few for a
radio-interception pillar spanning a 300+ day campaign.

## Why (P2)
- Verified: `radio_distress_signals.json` has 5 entries (frequency_id,
  frequency_mhz, source_name, outcome_type, days_to_trace, message_fragments
  with day, clarity, text, revealed_location, revealed_items,
  revealed_knowledge, knowledge_points, narrative_id, warning_text,
  location_reference). The system is confirmed live.
- Creates the radio-interception pillar: distress signals are the game's
  signal-intelligence layer — each is a multi-day broadcast the player
  traces, with increasing clarity, leading to a location, a cache, a
  community, or a trap. 5 signals covers one playthrough session; 20 covers
  the full campaign with diverse outcomes (rescue, supply, knowledge,
  narrative, trap, military, civilian, scientific).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/radio_distress_signals.json` (expand 5 → 20)
- Read-only: grep for the consuming system to confirm how frequency_id,
  revealed_location, and revealed_items resolve
- `Assets/StreamingAssets/Data/items.json` (revealed_items must resolve)

## Content grammar (per signal)
- `frequency_id`: snake_case with prefix `freq_distress_` (confirmed prefix).
- `frequency_mhz`: string (e.g. "217.4", "148.2" — realistic VHF/UHF
  frequencies).
- `source_name`: evocative name ("Checkpoint Kilo Automated Beacon",
  "The Pianist's Last Broadcast").
- `outcome_type`: survivor_community / bait_trap / knowledge / narrative /
  supply_cache / military_remnant / disease_warning / refugee_caravan /
  scientific_data / false_signal.
- `days_to_trace`: 2–6 (number of days to fully resolve the signal).
- `message_fragments`: array of days_to_trace objects, each with:
  - `day`: integer (1 to days_to_trace).
  - `clarity`: 0.0–1.0 (signal clarity on this day — increases over time).
  - `text`: 1–3 sentences of broadcast text. Match the existing quality —
    each fragment adds information, the final fragment resolves the signal.
- `revealed_location` (optional): location id revealed when fully traced.
- `revealed_items` (optional): array of item ids found at the revealed
  location.
- `revealed_knowledge` (optional): knowledge id unlocked.
- `knowledge_points` (optional): integer knowledge points granted.
- `warning_text` (optional): for bait_trap signals, the text warning the
  player.
- `narrative_id`: narrative event id triggered on completion.
- `location_reference` (optional): location referenced but not revealed.
- Diversity: cover rescue, supply, knowledge, narrative, trap, military,
  civilian, scientific, disease, and false-signal outcomes.

## Steps
1. Grep for the consuming system to confirm how frequency_id,
   revealed_location, and revealed_items resolve.
2. Read the existing 5 signals to confirm the quality bar (Checkpoint Kilo,
   Bunker 4-East, Weather Station Gamma, The Pianist, Convoy Echo-7 — each
   is a richly written multi-day broadcast with a distinct outcome).
3. Read `items.json` to confirm which item ids exist for revealed_items.
4. Author 15 new signals across 10 outcome types:
   - `survivor_community` (2): a fishing village holding out on the coast;
     a school basement with children and a teacher.
   - `supply_cache` (2): a military fuel depot cache; a pre-war pharmacy
     stockpile.
   - `knowledge` (2): a geological survey data burst; a weather pattern
     update revealing a safe corridor.
   - `narrative` (2): a mother searching for her child; a priest's final
     sermon broadcast.
   - `bait_trap` (2): a "trader" luring scavengers; a "military rescue"
     that's a warlord press gang.
   - `military_remnant` (1): a holdout bunker still following pre-war
     orders.
   - `disease_warning` (1): a medical team warning of a cholera outbreak.
   - `refugee_caravan` (1): a caravan of 40 survivors heading north.
   - `scientific_data` (1): a research station's final radiation data.
   - `false_signal` (1): a signal that resolves to static and silence —
     nothing was ever there.
5. Each signal: distinct frequency, source, outcome, days_to_trace, and
   message_fragments. Match the existing quality — each fragment adds
   information, the final fragment resolves the signal with clarity.
6. Cross-reference: every frequency_id unique; every revealed_location
   resolves to an existing location; every revealed_item resolves in
   items.json; every narrative_id follows existing conventions.
7. Wire 3 signals into Plan 76 (expedition destinations — revealed locations
   become expedition-reachable).
8. Wire 2 signals into Plan 50 (radio distress signal expansion — these
   signals ARE the radio distress signal expansion).
9. Wire 2 signals into Plan 82 (Verdict investigation sites — some signals
   reference Verdict locations).
10. Validate: `--data-integrity-selftest` (all ids resolve).
11. xUnit: radio distress signal catalog loads 20 signals, all frequency_ids
    unique, all message_fragments have increasing clarity, all
    revealed_items resolve, all narrative_ids non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is revealed_location resolution (step 6):
confirm the revealed location exists in a location catalog before
authoring.

## Definition of Done
- `radio_distress_signals.json` has 20 signals, all ids resolving, 3 wired
  to expedition destinations, 2 wired to Verdict sites, integrity + tests
  green.

## Follow-on
- Plan 76 (expedition destinations) — revealed locations become
  expedition-reachable.
- Plan 50 (radio distress signal expansion) — this plan IS that expansion.
- Plan 82 (Verdict locations) — some signals reference Verdict sites.
- Plan 73 (faction radio) — distress signals and faction broadcasts
  complement each other.
- Plan 84 (muster witnesses) — some signals corroborate or contradict
  witness testimony.
