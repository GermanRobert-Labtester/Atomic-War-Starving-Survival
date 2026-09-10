# Plan 143 reference audit

Verified against current data and code on 2026-09-09.

## Survivors

`survivors.json` contains the four authored IDs: `aris_thorne`, `maya_lin`,
`victor_vance`, and `elena_rostov`. They are canonical survivor definitions,
but they are not in the default three-member demo roster. Runtime eligibility
therefore checks the live roster and blocks an event while its subject is
absent, dead, or away.

## Factions

The authored aliases resolve through `FactionStandingIdResolver`:

- `iron_garrison` → `faction_central_garrison`
- `ash_militia` → `faction_upland_militia`

The persistent mutation authority is `FactionWarSystem.ModifyStanding`,
available through the Year of Ash host. The arc adapter never writes a second
standing ledger and never uses display-only faction text as an authority.

## Location and expedition

`loc_missile_silo` resolves in `locations_expansion3.json`; `ExpeditionCatalogLoader`
folds that location catalog into the expedition definition registry even though
the ID is not a row in the primary `expeditions.json` file. Plan 143 records an
offer for that resolved destination. Actual dispatch remains in
`ExpeditionHostSession`, where party, supply, weather, route, discovery,
vehicle, and travel validation continue to run.

## Other references

- No current production search found an arc progression ledger for these four
  survivors.
- No current production search found a generic faction-intel score or an
  `Ash Rot Remedy` treatment/item authority.
- The existing radio system can present radio content, but the authored Maya
  effects do not create a new broadcast network.
- The existing journal `KnowledgeBase` is suitable for a bounded faction-intel
  discovery key; it is not used as a duplicate faction standing store.
