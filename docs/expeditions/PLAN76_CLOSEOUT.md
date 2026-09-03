# Plan 76 — Closeout Report: Expedition Destination Catalog

## Summary

Plan 76 was scoped against a stale baseline ("2 destinations, expand to 15").
Repository truth at execution: **53 authored destinations** in
`expeditions.json` and a **263-id** loader-merged dispatchable surface, with
every §14 family role already covered by a distinct physical site. Per §1.1
(repository truth overrides the planning grammar), the expansion objective was
**superseded**; the executed objective is the plan's secondary one — prove the
roster is loadable, unique, in-range, loot-valid, dispatchable, balanced, and
save-compatible, and repair the one genuine data defect class found (3 invalid
loot refs on 5 destinations).

Zero new gameplay systems. Zero new destinations. One new data-regression test
file. Two data files touched (one JSON + one host fallback mirror) and one
allowlist doc extended.

## Baseline

See `PLAN76_BASELINE.md`. All four canonical gates PASS before any change
(build 0/0, tests 6680/6680, integrity selftest 0 findings / 208 catalogs,
expedition selftest 19/19).

## Schema

Documented in `docs/expeditions/EXPEDITION_SCHEMA_CONTRACT.md` (pre-existing,
verified accurate against `ExpeditionCatalogLoader.cs`). Fields: `id`,
`displayName`, `distanceTicks` (≥1, 1 tick = 0.5 h), `dangerLevel` (1–10),
`encounterChancePerTick` (0.05–0.50), `baseStaminaDrainPerHour` (1.0–5.0),
`lootCategories` (item ids), optional `scavenging_table_id` (Plan 46).

## Location Authority

Model A at scale: every authored destination id is the canonical
`locations.json`-family id. The loader merges four location files behind
`expeditions.json` (first-seen wins), making every located site
expedition-capable with loader defaults. No expedition-only ids exist; none
were added.

## Destination Roster

53 authored destinations across urban / industrial / military / scientific /
wilderness-linked families and 3 settlement sites — full roster with
per-destination values in `Assets/StreamingAssets/Data/expeditions.json`
(count, uniqueness, ranges, and tier distribution pinned by
`Plan32ExpeditionDestinationWiringTests`). Family-role coverage evidence in
`PLAN76_BASELINE.md` §7.

## Distance / Danger / Stamina

Full pressure/stamina matrix and distribution analysis:
`PLAN76_BALANCE_AUDIT.md`. No pathological encounter pressure; stamina burden
feasible across all tiers; progression supported.

## Loot Authority

Dual-mode: Plan 46 `scavenging_table_id` (11 destinations, all resolving) is
authoritative when present; otherwise `lootCategories` are used directly as
item ids. **Repaired 3 invalid refs** (`bandages`→`bandage`,
`food_rations`→`dried_rations`, `copper_wire`→`copper_wire_10m_of_10m`) on 5
destinations, mirrored in the `ExpeditionHostSession` no-catalog fallbacks.
Details: `PLAN76_LOOT_AUTHORITY_AUDIT.md`. Gate:
`Plan76DestinationLootReferenceTests` (4 tests).

## Micro-Locations (Plan 49)

**Deferred — seam not present.** No destination-level micro-location binding
field exists in the current schema. Stable destination ids remain available
for the future approach-discovery seam. No dangling refs authored (§36 rule
honoured).

## Weather Gates (Plan 48)

**Deferred — seam not present at destination level.** Weather blocking belongs
to the host `ExtraBlocked` / route-blocking seam; the destination loader owns
no weather semantics. No bindings authored.

## Save

Old saves: the two original destination ids are unchanged and pinned by test.
Mid-expedition save/restore: pinned by
`Plan32ExpeditionDestinationWiringTests.MidExpeditionSaveAndRestore_MaintainsDestinationIntegrity`.
No save-format changes. Availability policy is additive-visible; new entries
(if ever added) would not retroactively alter old-save state.

## Determinism

No code or data change touches RNG, ordering, or tick semantics. Destination
lookup is by id from a registry; loot rolls route through `ISeededRng`.
Seeded-behaviour tests untouched and green.

## Validation

| Gate | Command | Result |
|---|---|---|
| Core tests (Plan 76 scope) | `dotnet test … --filter "FullyQualifiedName~Plan76DestinationLootReference|FullyQualifiedName~Plan32ExpeditionDestinationWiring|FullyQualifiedName~ExpeditionSystemTests|FullyQualifiedName~ExpeditionVehicleLogistics"` | **58 / 58 PASS** (54 pre-existing + 4 new) |
| Core tests (full suite) | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6732 run, 2 FAIL — both in `FactionRadioBroadcastExpansionTests`, an unrelated **concurrent workstream active in the same tree** (radio expansion files modified after the Plan 76 baseline); not caused by, and not repairable within, the Plan 76 delta |
| Godot host build | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings (208 catalogs) |
| Expedition selftest | `godot --headless --path . -- --expedition-selftest` | PASS — 19/19 |

> **Concurrency note:** a second agent modified unrelated files during execution
> (radio expansion, `items.json`, `scavenging_tables.json`, several test
> files). Plan 76's scoped gates were re-run after those changes landed and
> remained green. Full-suite health must be judged after that workstream
> completes, not attributed to Plan 76.

## Deferred (explicit follow-ups only)

1. **Plan 76.1 — table bindings:** author Plan 46 scavenging tables for the
   42 lootCategories-only destinations (content migration; do not invent ids).
2. **Plan 76.2 — subway depot identity merge:** reconcile
   `location_flooded_subway_depot` vs `loc_flooded_subway_depot` with a save
   migration.
3. **Plan 76.3 — id-prefix normalization:** `location_*` / bare ids → `loc_*`
   (cosmetic; requires a save-reference sweep).
4. **Plan 76.4 — tone review:** "Ministry of Truth Bunker" / "The Dead Hand
   Core" naming register.
5. Plan 48/49 destination-level bindings once those seams exist.

## Definition-of-Done reconciliation

DoD items 1–15, 18–21, 40–56, 62–66, 69–90: satisfied (validated, not
authored). DoD items 16–17, 22–39, 57–61, 67–68, 70–71 (the 13-destination
expansion and live Plan 48/49 bindings): **superseded by repository truth** —
the catalog already exceeds the target and the binding seams do not exist.
Recording them as "done by prior work / not applicable" rather than shipping
duplicate destinations is the only outcome consistent with the plan's own
§1.4, §33, §52 and §76 rules.
