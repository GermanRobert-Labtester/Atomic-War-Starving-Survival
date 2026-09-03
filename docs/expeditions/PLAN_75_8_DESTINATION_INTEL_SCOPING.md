# Plan 75.8 — Destination-Discovery Intel Layer — Scoping (pre-implementation)

Status: **SCOPED, NOT IMPLEMENTED.** This document turns Plan 76 §75.8
("radio/codex/scouting systems can reveal destination, expected loot, danger,
weather concerns") into an implementable design for owner approval.

## What exists today (validated)

- Stable destination IDs: 65 authored destinations, all table-bound
  (Plan 76/76.1 + Plan 85).
- Full data for intel content: `distanceTicks`, `dangerLevel`,
  `encounterChancePerTick`, bound `scavenging_table_id` (loot identity),
  weather gates (weather concerns), `locations.json` descriptions.
- Display surfaces: `ExpeditionPanel` destination cards (name, travel,
  danger, dispatch bar), `MapPanel` location list.
- No discovery/intel state, no reveal mechanism, no gating — every
  destination's full data is visible from turn one.

## Design options

### Option A — Campaign intel ledger (recommended first slice)

New Core state: `DestinationIntelState { knownDestinationIds }` captured in
the campaign envelope (one new save section via the Plan 76.1
`SaveStore<T>`/registry pattern — atomic, checksummed, backward-compatible:
old saves default to "all known" so nothing regresses).

- Reveal sources (each a one-line call on the ledger):
  - radio broadcasts referencing a destination id (existing radio corpus),
  - returning expeditions (a visited site is known — trivially true at start
    for the original shelter-adjacent sites),
  - survivor traits/quests that grant survey knowledge.
- Panel behavior: unknown destinations render name as "Unsurveyed site" with
  danger shown as "?" — distance and weather-gate status still visible (the
  map is public knowledge; detail is earned). Known destinations render fully.
- Data: none new (intel reads existing catalogs).

### Option B — Scouting sorties

A lightweight expedition stance ("recon") that reveals intel without looting.
Builds on Option A's ledger; adds a stance flag + panel copy. Larger UI
surface; defer until A ships.

### Option C — Full fog-of-war map

Reveal-by-proximity across the map atlas. Largest surface; conflicts with the
current all-visible map policy. Not recommended now.

## Open owner decisions

1. Initial knowledge set: all 65 known (pure display feature) vs
   shelter-adjacent only (Option A's default proposal)?
2. Does unknown-ness block dispatch, or only hide detail? (A assumes
   dispatch stays allowed — exploring IS how you learn.)
3. Radio-corpus integration: which broadcasts reveal which destinations
   (needs a `reveals_destination_id` field on radio entries — small data
   addition to the radio authority, coordinate with the radio workstream).

## Estimated surface

Core: 1 state class + save section + ledger API (~150 lines, testable).
Host: 1 event subscription + 1 composition line. UI: label swap in
`ExpeditionPanel`. Data: none mandatory; radio field optional.

## Sequencing

After the concurrent workstream's commit lands (the intel panel edit touches
`ExpeditionPanel.cs`, currently shared-uncommitted).
