# Plan 28 — Session Report (authoritative-runtime implementation)

> **RECONCILIATION OUTCOME (2026-09-01 14:10).** The collision below has been resolved:
> the authoritative-runtime path is kept; the duplicate runtime island
> (`EcologyCoordinator`, `EcologyModels`, `EcologyCatalogLoader`, `wildlife_migration.json`,
> `ecological_infestations.json`, their 6 tests) was retired with its authored content
> preserved in `RETIRED_ECOLOGY_ISLAND.md`; kept content (8 `event_eco_*` events,
> `loc_dead_zone`) runs through existing authorities; `PLAN28_BASELINE.md`,
> `WILDLIFE_MIGRATION_SCHEMA.md` and `PLAN28_COMPLETION_REPORT.md` were rewritten to the
> reconciled truth. Post-retirement: 0 orphaned code/data references; Plan-28 scoped
> suites 149/149; evolving-world + bridge selftests PASS. Remaining full-suite/
data-integrity noise is a concurrent session's in-flight faction-war flag content
> (`flag_grievance_scavenger_claim_*` unresolved in faction_war_events.json /
> muster_faction_actions.json / muster_witnesses.json — not Plan 28 files).

> **Read this before PLAN28_COMPLETION_REPORT.md.** Two agent sessions executed Plan 28
> concurrently with complementary-but-overlapping architectures. This report covers the
> live, host-wired implementation. The collision and required decision are in §6.

## 1. What is LIVE in the running game (host-wired, save-safe)

| Capability | Implementation | Proof |
|---|---|---|
| Wildlife migration runtime | `WildlifeMigrationSystem` + `.Live.cs` (pre-existing, extended) | tick-registered via `EvolvingWorldDayOwner`; save envelope round-trip |
| Migration catalog | `world_evolution_seeds.json` (13 packs, 11 sectors, 12 species) — the ONLY migration catalog with live consumers | `--evolving-world-selftest` steps 1–5 |
| **Seasonal ecology calendar (new)** | `WildlifeSeasonalCalendar.cs` — 7 archetypes × 6 Plan 19 season windows; pure functions; modulates hunger ±30%, abundance [0.2, 1.5] | 16 xUnit tests, `WildlifeSeasonalCalendarTests` |
| **Waterway-bound fish run** | `species_mirror_carp` + `species_gray_heron` confined to river⇄estuary (`water: true` flags) | `FishRun_NeverStandsOnDryGround` (360-day) |
| Trapping windows | seasonal abundance composes into `RefreshTrappingDensity` → `CheckTraps(densityMultiplier)` | density gate selftest step |
| Migration radio notices | 6 archetype-flavored `radio_intercept` projections, capped 3/day, no population exposure | `Main.CampaignOwners.cs` + notice tests |
| Market scarcity | wildlife ratio → `canned_food` demand deltas (existing, clamped) | selftest step 13 |
| Collapse/recovery | starvation floor 0, recovery ceiling 2× seed, rabies day-stamped | Live tick tests (pre-existing, still pinned) |
| Season binding | `WorldHostSession.Create` → `Wildlife.BindSeasonProfile(profile)` | legacy-neutral without binding (`UnboundCalendar_IsExactlyLegacyNeutral`) |

## 2. Verification (canonical gates, last stable tree ~13:41)

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   PASS (0 errors)
dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    PASS 5757/5757
dotnet build Ashfall.csproj                                  PASS 0 err 0 warn
godot --headless -- --data-integrity-selftest                PASS (159 catalogs, 0 errors)
godot --headless --path . -- --bridge-selftest               PASS
godot --headless --path . -- --evolving-world-selftest       PASS (18/18 steps)
```
Plan-28-scoped: 49/49 (Wildlife + EvolvingWorld); seasonal calendar suite 16/16.

## 3. Honest deferred list (documented, not silently skipped)

Fish-run coastal-harvest UI (Plan 23), rad-taint through corridors (needs sector-contamination
authority — RAD_TAINT matrix has the exact path), locust→blight eligibility (Plan 22 API),
war-blocked corridors (28N design), infestation packs (their `EcologyCoordinator` content
exists but is UNWIRED — see below), excavation nest disturbance, waystation/caravan hooks,
field-guide entry authoring (handed to 20A), balance simulations (28BA–28BD).

## 4. Plan corrections (§17 autonomy exercised)

1. The runtime's catalog was never missing — `world_evolution_seeds.json` is the contract;
   authoring `wildlife_migration.json` as a second pack catalog forks authority.
2. Plan 35's route-list schema does not match the adjacency-walk runtime — superseded.
3. Trapping abundance hook already existed (`densityMultiplier`); Task 28F was pre-wired.

## 5. This session's deliverables

**Code (live):** `WildlifeSeasonalCalendar.cs` (new, pure, engine-free),
`WildlifeMigrationSystem.Live.cs` (+BindSeasonProfile, +SetWaterSectors, hunger pacing,
water-bound neighbor filter), `EvolvingWorldCatalog.cs` (+`water` sector flag),
`world_evolution_seeds.json` (+2 packs, +2 water flags), `WorldHostSession.cs` (calendar
bind), `Main.EvolvingWorld.cs` (seasonal density), `Main.CampaignOwners.cs` (archetype
notices), `HostCli.EvolvingWorld.cs` (gate updates), `EvolvingWorldActivationTests.cs`.

**Tests:** 16 new seasonal-calendar tests + 33 wildlife/evolving-world assertions updated.

**Docs:** 18 files under `docs/ecology/` (baseline, schema, species/corridor/calendar
matrices, taint deferral with integration path, event/infestation/shelter contracts,
ecological web, predator–prey, market effects, field-guide handoff, map visibility, save
migration, balance/content-utilization/regression matrices).

## 6. ⚠️ COLLISION — Plan 28 was executed twice; reconciliation required

A second agent session landed, in parallel: `Assets/Ashfall.Core/Ecology/`
(`EcologyCoordinator`, `EcologyModels`, `EcologyCatalogLoader`),
`wildlife_migration.json` (12 patterns), `ecological_infestations.json`,
`Ashfall.Core.Tests/Ecology/Plan28LivingEcologyTests.cs`, and overwrote several
`docs/ecology/*.md` files (including this baseline) between 13:33–13:44.

Current facts:
- **Live & authoritative:** the seeds catalog + `WildlifeSeasonalCalendar` +
  `WildlifeMigrationSystem` host wiring (this pass) — ticked daily, save-supported,
  selftest-gated.
- **Runtime island:** `EcologyCoordinator` + `wildlife_migration.json` are instantiated
  **only by their own tests**; nothing in `src/` consumes them. Their 12-pattern catalog
  (corridor lists, abundance multipliers, predator_follow, rad_taint_risk) duplicates the
  seeds authority and is dead data until wired.
- **Salvageable:** their infestation/cascade content model (6 location + 4 shelter
  infestations, clear/harvest tradeoffs, cascade multipliers) fills exactly the scope this
  pass deferred — but per §1.9 it must be wired through the owning systems
  (VentilationSystem, food-spoilage authority, MarketSystem) or retired.

**Required human decision (cannot be made unilaterally):**
1. keep this pass's authoritative-runtime approach and treat `EcologyCoordinator` as
   wire-in-progress (its catalog becomes infestation/eligibility data only), or
2. retire `EcologyCoordinator`/`wildlife_migration.json` per plan §1.1/§15 and keep this
   pass's implementation as the whole of Plan 28 Phase 2.

Either way: **do not commit** until reconciled — the tree also holds a third party's
in-flight Muster/Maritime/Plan-32 edits (206 untracked files, active mid-write).
