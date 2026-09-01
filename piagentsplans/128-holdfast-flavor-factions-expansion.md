# Plan 128 — Holdfast Flavor Factions Expansion (3 → 8 factions)

## Goal (2 lines)
Expand the `factions` dict in `holdfast_flavor.json` from 3 factions to 8.
The Holdfast flavor catalog (`HoldfastDispatchLog.cs` confirmed live in
`src/Host/`) defines the voice and transactional personality of each
Holdfast faction — register, voice, rejected, sold. 3 factions (The
Office, The Cutters, The Fleet) is too few for the ice-road and estuary
trade pillar.

## Why (P2)
- Verified: `holdfast_flavor.json` has `factions` (dict with 3 keys:
  faction_the_office, faction_the_cutters, faction_the_fleet) and `items`
  (40). Each faction entry has register, voice, rejected, sold (strings
  describing the faction's bureaucratic/trade personality).
  `HoldfastDispatchLog.cs` in `src/Host/` consumes it.
- The Holdfast expansion is the ice-road and estuary pillar. 3 faction
  voices means the trade-dispatch system sounds repetitive — every
  transaction with a non-covered Holdfast faction falls through to a
  generic voice. The new Holdfast quests (Plan 117) reference factions
  that need flavor.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/holdfast_flavor.json` (expand `factions`
  dict 3 → 8)
- Read-only: `src/Host/HoldfastDispatchLog.cs` (confirm how faction
  entries are keyed and consumed)

## Content grammar (per faction entry)
- Keyed by faction id (snake_case, `faction_` prefix — e.g.
  `faction_the_office`).
- `register`: string describing the faction's bureaucratic register
  ("bureaucratic", "maritime", "frontier" — the tone of their paperwork).
- `voice`: 2–3 sentences in the faction's voice describing how they
  speak and transact.
- `rejected`: 1–2 sentences in the faction's voice when a requisition
  is denied.
- `sold`: 1–2 sentences in the faction's voice when a transaction is
  accepted.

## Steps
1. Read `HoldfastDispatchLog.cs` to confirm how faction entries are
   keyed (by faction id) and how the four fields (register, voice,
   rejected, sold) are consumed in the dispatch log.
2. Inventory the 3 existing factions. Confirm the quality bar and the
   Holdfast voice (cold, salt, ice-road, bureaucratic).
3. Author 5 new faction entries:
   - `faction_the_lamplighters`: register "maritime"; voice about lamp
     oil, sector posts, and the road staying lit; rejected about missing
     fuel or wrong sector; sold about the lamp burning another night.
   - `faction_the_estuary_camp`: register "frontier"; voice about the
     camp, the catch, and survival; rejected about empty hands; sold
     about the camp fed for another day.
   - `faction_the_kittiwake`: register "maritime"; voice about the
     launch, the log, and the tide; rejected about wrong tide or
     weather; sold about the launch returning.
   - `faction_the_ice_road_guild`: register "bureaucratic"; voice about
     the road, the kilometres, and the toll; rejected about unpaid toll
     or unsafe ice; sold about passage granted.
   - `faction_the_quarantine_post`: register "bureaucratic"; voice
     about screening, the gate, and the count; rejected about failed
     screening; sold about clearance granted.
4. Each faction: distinct register, distinct voice, distinct
   rejected/sold lines. No two factions share the same register + voice.
5. Cross-reference: every faction id unique; every id follows `faction_`
   prefix.
6. Wire 3 new factions to Plan 117 (Holdfast quests — quests reference
  factions that now have flavor).
7. Wire 2 new factions to Plan 120 (crossing factions — some Holdfast
  factions overlap with Crossing trade).
8. Validate: `--data-integrity-selftest` (loads cleanly).
9. xUnit: Holdfast flavor catalog loads 8 factions, all keys unique,
   all four fields non-empty per faction.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the dict key structure (step 1): the
factions are keyed by id, not in an array. Confirm the loader expects a
dict before adding entries.

## Definition of Done
- `holdfast_flavor.json` has 8 factions, all keys unique, all four
  fields non-empty, 3 wired to Holdfast quests, 2 to crossing factions,
  integrity + tests green.

## Follow-on
- Plan 117 (Holdfast quests) — quests reference flavored factions.
- Plan 120 (crossing factions) — Holdfast/Crossing faction overlap.
- Plan 92 (faction war dialogue) — Holdfast faction dialogue.
- Plan 95 (journal voice) — Holdfast transactions trigger journal.
- Plan 89 (epilogues) — Holdfast faction standing feeds endings.
