# Plan 28 Baseline — reconciled (post-collision)

> This is the authoritative baseline after the two-session Plan 28 collision was
> reconciled on 2026-09-01. The duplicate ecology runtime island
> (`Assets/Ashfall.Core/Ecology/` + `wildlife_migration.json` +
> `ecological_infestations.json`) was retired; its authored content is preserved in
> `RETIRED_ECOLOGY_ISLAND.md`. The false "100% complete" completion report was replaced
> by `PLAN28_COMPLETION_REPORT.md` (now the reconciliation record).

## 1. Authoritative implementation (live, verified)

**One ecology authority, built by extending the existing runtimes — no second engine.**

| Concern | Authority | State |
|---|---|---|
| Wildlife population & movement | `WildlifeMigrationSystem(.Live.cs)` — pack ledger, hunger-driven sector-graph walk, starvation/rabies/recovery bounds | live, tick-registered, save-supported (since task 122) |
| Seasonal rhythm | `WildlifeSeasonalCalendar.cs` — 7 archetypes × 6 Plan 19 windows, pure functions | Plan 28 |
| Migration/corridor catalog | `world_evolution_seeds.json` (13 packs, 11 sectors, `water` flags) | authoritative |
| Season authority | `weather_seasons.json` via `SeasonProfileDef` | Plan 19, untouched |
| Trapping | `WildlifeTrappingSystem` density multiplier | consumed, not modified |
| Market | `MarketSystem` demand deltas | existing path |
| Radio/map/codex | Plan 24/16/20A surfaces | projections only |

Plan 28 code delta (all in the authoritative path):
- `Assets/Ashfall.Core/WildlifeSeasonalCalendar.cs` (new — pure archetype calendar),
- `WildlifeMigrationSystem.Live.cs` (optional season binding, hunger pacing, water-bound
  neighbor filter; unbound = legacy-identical),
- `EvolvingWorldCatalog.cs` (+`water` sector flag, seeder passes water sectors),
- `world_evolution_seeds.json` (+2 packs: mirror carp run 18, ghost moth bloom 16),
- `src/Host/WorldHostSession.cs` (season→wildlife bind),
- `src/Main.EvolvingWorld.cs` (seasonal abundance × trapping density),
- `src/Main.CampaignOwners.cs` (archetype-flavored, capped radio intercepts).

## 2. What the retired island was (see RETIRED_ECOLOGY_ISLAND.md)

A second session landed `EcologyCoordinator`/`EcologyModels`/`EcologyCatalogLoader` +
`wildlife_migration.json` + `ecological_infestations.json` + 6 tests. Clean code, but:
zero host consumers (runtime island), a second migration definition layer duplicating the
seeds authority, and a loader bypassing the `IJsonSerializer` port convention. Retired
2026-09-01; authored content preserved in `RETIRED_ECOLOGY_ISLAND.md`.

**Kept from that session** (live through existing authorities, verified by integrity gate):
8 `event_eco_*` events in `events.json` (state-gating = follow-up), `loc_dead_zone`
location, `CatalogIntegrityRules` prefix additions their kept content requires.

## 3. Verification at reconciliation

- `dotnet build Ashfall.Core.Tests` — clean (as of last stable tree)
- `dotnet test` — 5757/5757 PASS pre-retirement; re-verified after retirement below
- `--data-integrity-selftest` — PASS (was 159 catalogs with the duplicate files; recheck)
- `--bridge-selftest`, `--evolving-world-selftest` — PASS
- Plan-28 scoped suites: 49/49 (Wildlife/EvolvingWorld/Greenhouse/Market)

## 4. Task ledger after reconciliation + Phases 3–4 (this pass)

**Phase 3 additions (this session):**
- **28N war-blocked corridors** — `WildlifeMigrationSystem.SetSectorBlocked/ClearSectorBlockages`
  (stateless projection); binding = `LocationSeedRecord.sector_id` (seeds authority);
  dominant-faction sectors close to movement; enclosed packs siege in place (hunger grows).
  Host projection: `EvolvingWorldDayOwner` re-projects daily from `DominantFactionId`.
- **28P overhunt** — `WildlifeMigrationSystem.ApplyHarvestPressure(sector, n)` floors at a
  remnant pair (existing birth rule recovers); wired: `WildlifeTrappingHostSession.OnCatchPressure`
  → `ApplyHarvestPressure(shelterSector, caught)`. No hidden tracking system.
- **28M collapse notice** — `hazard_warning` + journal line when the global ratio ≤ 0.45,
  re-armed after 12 days (no spam).

**Phase 4 (infestations through owning systems):**
- `EcologicalInfestationSystem` (Core, engine-agnostic) + `ecological_infestations.json`
  (10 authored infestations: 6 location + 4 shelter; clear costs = live item ids;
  3 leave/harvest tradeoffs, bounded by `max_harvests`); loader via ports + CatalogDiagnostics.
- Save: `ecological_infestation_save.json` (SchemaVersionedEnvelope via SaveStoreHub) +
  `SaveSectionRegistry` entries + Main triad (Setup/Save/Flush) + SaveAll + restore path.
- Disease: `ecological_infestation` `IDiseaseOutbreakSource` contract
  (disease_spore_blight, disease_fungal_respiratory) — TriggerOutbreak rejects outside
  the contract (no second infection path).
- Shelter food loss routed through `InventoryHostSession.Remove/Add`
  (`canned_food` → `spoiled_canned_food`, capped by `MaxFoodLossPerDay`).
- Shelter triggers: grain_stores / low_filtration (HEPA health < 55) / greenhouse_planted /
  quiet_winter; location infestations: known ground only.

## 4b. Phase ledger (updated — Phase 5 complete)

**Phase 5 (this session):**
- **28AG/28AV** — six "reading the land" Ecology entries shipped in `field_guide.json`
  (scat/browsing/silence/carrion-circling/moth-drill/rut-tracks), each `subject_id` bound to a
  live seeded species. Observation wiring: `WildlifeSeasonalCalendar.FieldGuideEntryFor(speciesId)`
  + `Main.UnlockFieldGuideObservation` — a sighted species unlocks its teach entry (journal line).
  Persistence of unlocked entries = Plan 20A save store (their GAP row).
- **28AB/28AH** — four exploitation opportunities live as authored events through the existing
  event runtime: scavenger-kettle intercept, fresh boar wallow, vacated hornet comb,
  bio-remediation mat assay (28AI hook, grounded).
- **28BA–28BD balance sims** — `EcologyBalanceSimulationTests` (6 seeded sims):
  360-day migration year bounded (ratio ∈ (0, 2×seed]); heavy exploitation never out-yields
  the untouched baseline and floors at the remnant pair; infestation year cadence 2–40
  outbreaks with yearly food loss survivable (<400 units, hard-capped per day); market
  demand deltas reverse on recovery (no permanent collapse); same-seed trace fingerprint
  exact. **Finding filed:** global recovery after heavy exploitation takes longer than one
  season — the remnant-pair floor keeps the world alive, packs wander to fed ground.
- **28BB hardening** — seasonal die-off: blooms with authored season windows die naturally
  when the window ends (no permanent shelter crisis without player action).

## 4b. Phase ledger (updated)

DONE: 28A (contract = seeds catalog), 28B (7 archetypes/12 species), 28C (corridors,
pre-existing + water flag), 28D (seasonal calendar), 28E (traversal, pre-existing),
28F (trapping windows), 28G (fish run, waterway-bound), 28J-minimal (archetype notices),
28M (pre-existing bounds), 28Q (audit → retirement), 28AY (save), 28AZ (determinism).
PARTIAL: 28K/28L (radio live; map deferred), 28G (market easing live; coastal-harvest UI
= Plan 23), 28M-predator (modifier designed, capped).
DEFERRED with designs: 28H (greenhouse hook), 28I (taint — RAD_TAINT matrix), 28N
(war-closed corridors), 28R/28S/28T/28U infestations (contract + retired-content seed in
RETIRED_ECOLOGY_ISLAND.md), 28V–28Y, 28AH–28AN, 28AS–28BB+ (later phases).

## 5. Honest status

Plan 28 is **Phase 1–2 complete + Phase 3 partial**; Phases 4–7 (infestation wiring,
world integration, balance sims, parity, manual trace) remain. The plan is NOT 100%
complete despite the retired completion report's claim.
