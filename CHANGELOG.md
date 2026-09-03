# Changelog

All notable changes to ASHFALL. Format: Keep a Changelog.

## [Unreleased]

### Plan 76 series — Expedition destination catalog & scavenging-table migration

- **Plan 76 — destination catalog validation & loot-ref repair** (`docs/expeditions/PLAN76_CLOSEOUT.md`):
  catalog verified at 53 authored destinations / 263-id merged dispatchable surface
  (expansion target already exceeded; quantitative objective superseded per
  repository-truth rule); repaired 3 invalid loot references on 5 destinations
  (`bandages`→`bandage`, `food_rations`→`dried_rations`,
  `copper_wire`→`copper_wire_10m_of_10m`) including the host no-catalog
  fallbacks; new `Plan76DestinationLootReferenceTests` regression gate.
- **Plan 76.1 — full Plan 46 scavenging-table migration** (`docs/expeditions/PLAN76_1_CLOSEOUT.md`):
  all 53 destinations bound to `scavenging_tables.json` tables (11 → 53 of 53);
  29 new tables authored across 7 family passes (medical, mechanical/fuel,
  household/commercial, military, electrical, water/chemical, remainder);
  renewable trade-stock model introduced for living settlements; one-time
  cache model introduced for supply caches; zero new item ids; all codex
  references reuse existing ids. Catalog: 20 → 49 tables.
- **Plan 76.2 — deterministic balance simulation** (`docs/balance/BALANCE_SIM_EXPEDITION_DESTINATIONS.md`):
  seeded harness (200 runs × 53 destinations, byte-identical two-pass
  determinism proof) over real runtime math; flagged and fixed the
  `loc_ordnance_shoulder` economy outlier (E[value] 216.9 → 114.4, ratio to
  next destination 3.5× → 1.86×, best-ammo identity retained) via
  owner-approved quantity-band trim; documented `collapsed_building` yield
  bump and the Denial Cut dominance decision (accepted — warlord-layer
  encounter multiplier and existing narrative hooks are the differentiators).
- **Plan 76.3 — low-priority pass & series closeout**: `collapsed_building`
  bulk-band yield bump (+45% E[value], identity preserved); Denial Cut
  decision recorded; this changelog.

### Notes

- Verification basis for the series: `dotnet build Ashfall.csproj` (0/0),
  scoped `dotnet test` green, `--data-integrity-selftest` (0 findings),
  `--expedition-selftest` (19/19), `--content-utilization-selftest` (gate PASS).
- Two unrelated test failures in `FactionRadioBroadcastExpansionTests` belong
  to a concurrent workstream active in the same tree and are outside the
  scope of this series.
- `src/Host/ExpeditionHostSession.cs` loot-ref fallback fixes are present in
  the working tree but intentionally left uncommitted with that file's other
  in-flight changes.
