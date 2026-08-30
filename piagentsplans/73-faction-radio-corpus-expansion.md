# Plan 73 — Faction Radio Corpus Expansion (silence events → 30 broadcasts)

## Goal (2 lines)
Expand `faction_radio_corpus.json` from mostly silence events to 30 faction radio
broadcasts. The faction radio system feeds the `RadioTuner` and the HUD chatter system
— each broadcast is a faction transmission the player can intercept on shortwave,
providing intel, propaganda, distress calls, or military traffic. The corpus is mostly
static/silence with almost no actual content.

## Why (P2)
- Verified: `faction_radio_corpus.json` has silence_events (static descriptions) but
  almost no actual faction broadcasts. `radio.json` has 50 broadcasts but the faction
  corpus is separate and nearly empty.
- Creates the radio-intel pillar: faction radio is the player's window into what
  factions are doing — patrol reports, supply requests, propaganda broadcasts,
  distress calls, encrypted traffic, intercepted communications. This is how the
  player learns about faction activity without leaving the shelter.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/faction_radio_corpus.json` (add 30 broadcasts)
- Read-only: confirm the faction radio consumer — `grep -rn "faction_radio\|FactionRadio"
  Assets/Ashfall.Core/` to find the loader and confirm the broadcast schema

## Content grammar (per broadcast)
- snake_case `id` with prefix `radio_` or `broadcast_` (confirm accepted prefix).
- faction: `faction_*` id — which faction's transmission this is.
- broadcast_type: patrol_report / supply_request / propaganda / distress_call /
  encrypted_traffic / military_traffic / civilian_intercept / dead_hand_ping /
  weather_report / supply_inventory.
- frequency: shortwave frequency (matches `RadioTuner` tuning range).
- content: 1-3 sentences of grounded radio prose (military brevity, faction jargon,
  exhausted human voice). Skill `ashfall-write`.
- intel_value: what the player learns (patrol movement, supply shortage, faction
  weakness, location of interest, upcoming raid — feeds Plan 44/45/59).
- signal_strength: weak / medium / strong — affects whether the player can hear it
  clearly.
- minDay: when this broadcast starts appearing on the airwaves.

## Steps
1. Find the faction radio consumer to confirm the broadcast schema and how broadcasts
   are selected for interception.
2. Read the existing silence_events to understand the corpus structure.
3. Read `radio.json` to avoid duplicating broadcast ids.
4. Author 30 faction broadcasts across 10 types (3 per type):
   - 3 patrol reports (faction patrol checking in — reveals patrol location, feeds
     Plan 45).
   - 3 supply requests (faction requesting supplies — reveals faction shortage, feeds
     Plan 56 economy).
   - 3 propaganda (faction broadcasting ideology — reveals faction philosophy, feeds
     existing 25A).
   - 3 distress calls (faction unit in trouble — feeds Plan 50 distress signals).
   - 3 encrypted traffic (cipher bursts — feeds existing 11B cipher hunts).
   - 3 military traffic (faction military communications — reveals troop movements,
     feeds Plan 44 territory).
   - 3 civilian intercepts (survivors broadcasting to each other — reveals settlement
     activity, feeds Plan 43).
   - 3 dead_hand pings (automated military system pings — feeds Plan 39 telemetry).
   - 3 weather reports (faction weather monitoring — feeds Plan 48 weather gates).
   - 3 supply inventories (faction logistics — reveals what a faction has and needs,
     feeds Plan 61 trade scenarios).
5. Give each broadcast: faction, type, frequency, content, intel_value, signal_strength,
   minDay.
6. Cross-reference: every `faction_*` id resolves; every frequency is within the
   `RadioTuner` range; every intel_value reference (location, patrol, supply) resolves.
7. Wire 8 broadcasts into the radio broadcast schedule (existing 24A) — these appear
   as tunable frequencies.
8. Wire 5 broadcasts to Plan 44/45 faction territory/patrols — the intel reveals patrol
   movements and territory changes.
9. Wire 3 broadcasts to Plan 59 questlines — a broadcast triggers a questline (patrol
   report reveals a missing caravan → investigation quest).
10. Validate: `--data-integrity-selftest`; confirm broadcasts appear on shortwave in a
    headless boot; confirm signal strength affects clarity.
11. xUnit: broadcast catalog loads, all references resolve, broadcasts appear on
    schedule, signal strength applies, intel_value provides valid information, save
    round-trip preserves intercepted broadcasts.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `faction_radio_corpus.json` has 30 broadcasts (plus existing silence events), all
  references resolving, 8 wired to radio schedule, 5 wired to territory/patrols, 3
  wired to questlines, broadcasts appear on shortwave, signal strength applies, save
  round-trip green, integrity + tests green.

## Follow-on
- Existing 24A (radio schedule) — faction broadcasts appear as tunable frequencies.
- Plan 44/45 (faction territory/patrols) — broadcasts reveal faction activity.
- Plan 50 (distress signals) — faction distress calls overlap with the distress catalog.
- Plan 39 (orbital harrow) — dead_hand pings feed the telemetry system.
- Existing 11B (cipher hunts) — encrypted traffic feeds the cipher-decode loop.
- Plan 59 (questlines) — broadcasts trigger questlines.
