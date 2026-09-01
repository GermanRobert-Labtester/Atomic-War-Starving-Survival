# Plan 23 Completion Report — Maritime & Black Flotilla: The Drowned Coast

## Summary

Tasks 23A (Flotilla faction & item depth), 23B (dive catalog to 14 + mechanic
utilization), 23C (deterministic tides, storm surges, coastal dynamics), 23D
(cross-layer integration), and 23E (long-campaign boundedness) are implemented and
verified. All runtime additions are minimal reusable extensions of the existing
authorities; the content layer is data-first.

**Files changed:** 4 Core maritime files (DiveSiteCatalog, MaritimeDiveSystem,
PsychologicalContaminationSystem, new TideCalendar), 1 Core standing file
(BlackFlotillaStanding), District8DeepCoastSystem (surge), FactionIconCatalog,
5 data catalogs (dive_sites, holdfast_factions, characters,
faction_radio_corpus, hardcore_economy_tuning, world_evolution_events,
environmental_texts), 2 UI/host presentation files, 5 test files, 16 docs.
No new runtime systems beyond the plan-sanctioned tide calendar.

## Baseline vs final counts

| Metric | Baseline | Final |
|---|---|---|
| Flotilla items | 24 | 36 (+12 non-duplicate niches) |
| Flotilla NPCs | 0 | 6 named roles |
| Flotilla radio bands | 0 | 1 (14 total corpus bands, 8 broadcast categories) |
| Fleets/divisions | — | 3 content-level fleet cultures (Salvage / Escort / Deep-Dive), no new faction authorities |
| Standing consumers | 0 | thresholds + tiers + trade preference + intel gates |
| Dive sites | 12 (worktree; 4 committed) | 14 |
| Safe-crack sites | 0 | 3 (sovereign purser, picket armory, payroll strongroom) |
| Contamination sites | 0 | 4 |
| Variable/procedural loot sites | 4 hardcoded host nodes | all 14 sites via catalog tables |
| Currents audited | 0 | 17 (roster truth documented; Undertow + Coastal Hydro-Barons wired) |
| Tide-window sites | 0 | 6+ (7 non-any windows) |
| Surge crisis patterns | 0 | 3 (world-evolution events on verified flags) |

## Plan 10 reconciliation

- Found 12 live sites (Plan 10 delta uncommitted in the worktree); preserved all 12
  stable IDs, rooms, air, and noise values verbatim; added exactly 2 sites
  (`site_exp23_payroll_strongroom`, `site_exp23_brine_cistern`) to reach 14.
- No renames, no deleted concepts; `bandages`→`medical_kit` was the only loot-ref
  correction (bandages is not an authored item).

## Authority decisions (final owners)

standing → `FactionStanceEngine` + `BlackFlotillaStanding`; trade → `FactionTradePreference`
(hardcore_economy_tuning.json); radio → `FactionRadioEngine`; dive/air/noise →
`MaritimeDiveSystem`; safe → `SafeCrackingSystem`; loot → `ProceduralScavengeSystem`/
`VariableLootNode`; contamination → `PsychologicalContaminationSystem` (site-scoped keys);
tides → `TideCalendar` (derived from campaign day); weather → `WeatherSystem`; surge →
`District8DeepCoastSystem.TickDaily` (single producer); map mutation → `WorldEvolutionEngine`;
persistence → `MaritimeSaveStore` + `SaveSectionRegistry`/HoldfastSave envelope.

## Flotilla depth

Three fleet cultures (Salvage/Escort/Deep) legible through 6 NPCs, 12 new items, 8+
broadcast lines in 4 engine pools, one trade preference, and a six-tier standing map —
no parallel faction architecture (fleets are content classification, not authorities).

## Dive mechanic coverage

All 14 sites: procedural salvage (decay-bounded); 3 safe sites (real SafeCrackingSystem,
persisted, reroll-blocked); 4 contamination sites (site-scoped keys, overworld clean);
noise/air per site; 2 hard gear gates; 6 tide-window sites; unique
access+hazard+mechanic+reward combination per site.

## Coastal dynamics

17 currents audited (people, not water) with two neglected coastal currents wired;
6 varied tide windows; deterministic 4-day tide calendar; 3 surge crises through the
single weather→deep-coast producer; 6 environmental state texts; map mutations via the
existing world-evolution machinery only.

## Risk/reward results

Deep sites best-single-recovery but bounded by travel (10.5–15.5 h), gear, air, noise,
decomposition of repeat yields (visit + world-phase decay), one-time safes, and
contamination exposure. No infinite arbitrage loop found; premium-buy list contains
only recoverable salvage; refuses luxuries; no Flotilla seller table to loop against.

## Persistence

Old saves load clean; surge state defaults to none and round-trips; resolved safes and
variable nodes never reroll; standing desync impossible (single stance authority).

## Verification

`dotnet build` (Core+Host, 0 errors/0 warnings); `dotnet test` 5,819 PASS;
`--data-integrity-selftest` PASS; `--maritime-selftest` PASS; `--deep-coast-selftest`
PASS (72/72); `--bridge-selftest` retained verb (PASS, shim removed). Exact outputs in
`PLAN23_REGRESSION_MATRIX.md`.

## Remaining risks (evidence-backed)

- This session ran on a shared working tree with live parallel workstreams
  (Plans 27/28/29/60). Several of their in-flight files required one-token syntax
  repairs to keep the shared build compilable (noted in commits). Final full runs green.
- `Keeper thread` (`q_keeper_of_logs`) remains a narrative-thread reference; no quest
  runtime consumes it beyond observation props (pre-existing debt, documented).

## Deferred follow-ups (evidence-backed)

- Dedicated deepest-wreck capstone quest arc (hook documented, sequencing deferred).
- Undertow/Flotilla salvage-claim rivalry quest arc (hooks verified, prose deferred).
- Code-ribbon collectible/decryption chain beyond the implemented context re-read.
- Dedicated tide-forecast tooltip surface beyond the atlas detail row.
- Storm-stranded-expedition rescue quest (strand state currently modeled through
  dock-operation reference only, per repository semantics).
