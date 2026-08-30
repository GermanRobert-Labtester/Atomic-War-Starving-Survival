# Plan 50 — Radio Distress Signal Expansion (5 → 25 signals)

## Goal (2 lines)
Expand `radio_distress_signals.json` from 5 verified entries to 25 distress signals, each
with a multi-stage rescue mission hook. Distress signals are the radio-driven quest entry
point: the player hears a signal on shortwave, triangulates the source, and dispatches a
rescue expedition — but some signals are traps, some are stale, and some lead to
encounters the player isn't prepared for.

## Why (P2)
- Verified: `radio_distress_signals.json` has 5 entries; `radio.json` has 50 broadcasts;
  `RadioTuner` is fully implemented. The distress-signal → rescue-mission loop is the most
  underused radio content path (existing 24B expands 5 → 21; this plan provides the signal
  data that loop consumes).
- Creates the radio-as-quest-hook pillar: the radio isn't just ambient — it's a source of
  missions, moral dilemmas, and world-state consequences. Some signals lead to survivors
  worth rescuing; some are ambushes; some are stale (the sender is already dead); some
  are false flags planted by hostile factions.
- Pure DATA work — extends the existing distress-signal catalog.

## Files to touch
- `Assets/StreamingAssets/Data/radio_distress_signals.json` (expand 5 → 25)
- Read-only: `Assets/StreamingAssets/Data/radio.json` (50 broadcasts — distress signals may
  reference broadcast ids), `Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs`
  (confirm signal schema: frequency, signal type, coordinates, age, authenticity),
  `Assets/StreamingAssets/Data/expeditions.json` (Plan 32 — rescue missions dispatch to
  the signal source location), `Assets/StreamingAssets/Data/questline_master.json` (362
  quests — rescue missions may be quest entries)
- Check: `grep -rn "distress\|Distress\|rescue\|Rescue" Assets/Ashfall.Core/`

## Content grammar (per signal)
- snake_case `id` with prefix `signal_` or `radio_` (confirm accepted prefix — do not invent).
- frequency: shortwave frequency value (matches `RadioTuner` tuning range).
- signal_type: voice_distress / morse_code / automated_beacon / intermittent_carrier /
  encrypted_burst / dead_air_with_background / child_voice / military_freq.
- coordinates: `loc_*` id (Plan 32) — where the signal originates; the rescue expedition
  dispatches here.
- age: how old the signal is (fresh / hours_old / days_old / weeks_old / stale) — affects
  whether the sender is still alive.
- authenticity: genuine / stale / trap / false_flag / ambiguous — determines the outcome
  of the rescue mission.
- sender: optional `npc_*` id (Plan 52) — if genuine, a named survivor is rescued and may
  join the shelter or become a recurring NPC.
- reward: `item_*` ids or reputation delta if the rescue succeeds.
- consequence_on_trap: if the signal is a trap, what happens (ambush encounter, faction
  capture, resource loss — feeds existing 14 raids + Plan 45 patrols).
- consequence_on_ignore: what happens if the player ignores the signal (sender dies,
  faction reputation loss, later world-state consequence).

## Steps
1. Read `radio_distress_signals.json` to confirm the existing 5-entry schema; the 20 new
   entries must match it exactly.
2. Read `SignalIntelligenceCatalog.cs` to confirm signal-type and authenticity fields.
3. Read `radio.json` to confirm the broadcast schema; distress signals may appear as
   broadcasts the player hears on shortwave.
4. Author 20 new distress signals across 8 signal types:
   - 5 genuine voice distress (trapped survivors, injured traders, lost children —
     rescue rewards a named NPC or valuable loot).
   - 3 stale signals (days/weeks old — the sender is already dead; the expedition finds
     a body and environmental storytelling, not a rescue).
   - 3 traps (hostile faction ambush — the signal lures the player into a raid
     encounter; feeds existing 14 + Plan 45).
   - 2 false flags (planted by a faction to lure rivals — the player stumbles into a
     faction conflict; moral choice: pick a side or flee).
   - 2 automated beacons (pre-war emergency systems still transmitting — lead to
     military/scientific locations with unique loot; feeds Plan 37 excavation).
   - 2 encrypted bursts (cipher signals — feeds existing 11B cipher hunts; decode to
     reveal coordinates).
   - 2 child voices (genuine but the child is alone and injured — high moral stakes;
     rescue or leave; feeds existing 12A generational arcs).
   - 1 military frequency (a surviving military unit requesting extraction — faction
     hook; feeds existing 25A faction life).
5. Give each signal: frequency, type, coordinates, age, authenticity, sender (where
   applicable), reward, consequence on trap, consequence on ignore.
6. Cross-reference: every `loc_*` coordinate resolves to Plan 32; every `npc_*` sender
   resolves to `characters.json` or Plan 52; every `item_*` reward exists; every
   `faction_*` reference resolves.
7. Wire 8 signals into the radio broadcast schedule (existing 24A) — the player hears
   them while tuning; the signal appears as a tunable frequency.
8. Wire 5 signals into `questline_master.json` as rescue-mission quest entries with
   `flag_` progression (signal heard → expedition dispatched → rescue attempted → outcome).
9. Validate: `--data-integrity-selftest`; confirm a signal is heard on shortwave, the
   expedition dispatches to the coordinates, and the outcome (rescue/trap/stale) fires
   correctly in a headless boot.
10. xUnit: signal catalog loads, all references resolve, authenticity determines outcome,
    age determines sender survival, consequences fire on trap/ignore, save round-trip
    preserves resolved-signal state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data, extending an existing catalog. The one trap is the signal-type field
matching `SignalIntelligenceCatalog` (step 2 prevents this).

## Definition of Done
- `radio_distress_signals.json` has 25 entries (5 existing + 20 new), all references
  resolving, 8 wired into the radio schedule, 5 wired as quest entries, authenticity
  determines outcome, age determines sender survival, save round-trip green, integrity +
  tests green.

## Follow-on
- Existing 24B (rescue missions 5 → 21) — this plan provides the signal data.
- Existing 24A (radio schedule) — distress signals appear as tunable frequencies.
- Plan 32 (expedition wiring) — rescue missions dispatch to signal coordinates.
- Plan 52 (recurring NPCs) — rescued senders become recurring characters.
- Existing 11B (cipher hunts) — encrypted bursts feed the cipher-decode loop.
- Existing 14 (raids) — trap signals generate ambush encounters.
