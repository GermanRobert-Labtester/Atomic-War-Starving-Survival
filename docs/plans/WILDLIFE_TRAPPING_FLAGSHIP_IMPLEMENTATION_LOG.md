# Wildlife Trapping Flagship Implementation Log

Date: 2026-09-10

## Phase 0 — Contract audit

Status: PASS

The live calendar has ten windows. `window_spring_storms` begins on day 90;
day 120 begins `window_dry_ash`. The old plan wording that calls day 120 the
thaw boundary does not match the current `weather_seasons.json` authority.

The existing replacement contract is `!hasCatch && setDay > 0 && !isBroken`
blocks replacement; pending-catch, broken, and never-armed legacy sites are
replaceable.

## Phase 1 — Replacement and ecology contracts

Status: PASS

Changed:

- `WildlifeTrappingSystem.CanSetTrapAtSite` is the shared replacement query.
- Host `TrySetTrap()` and `WildlifeTrappingPanel` delegate replacement state to
  that query.
- Quarry eligibility now applies the authored trap compatibility matrix in
  addition to skill, season, and migration gates.
- The host session exposes selection-context forwarding, and the campaign
  ecology composer uses it.
- Catalog integrity checks migration-species references and trap-recipe item
  result/ingredient references.

Tests/selftests:

- Replacement, season/migration, and trap identity tests pass.
- Godot trapping host selftest: 10/10.
- Godot panel lifecycle/UI selftest: 16/16.
- Godot wildlife selftest: 4/4.
- Data-integrity selftest: 299/299 catalogs, 0 findings.

## Phase 2 — Verification

Status: PARTIAL

Passed:

- `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore`
- `dotnet build Ashfall.csproj --no-restore`
- `--filter WildlifeTrapping`: 274 passed, 0 failed; focused trap catalog
  mapping/recipe tests bring the combined tranche total to 305 passed, 0 failed
- `godot --headless --path . -- --trapping-selftest`
- `godot --headless --path . -- --panel-bind-lifecycle-selftest`
- `godot --headless --path . -- --wildlife-selftest`
- `godot --headless --path . -- --data-integrity-selftest`
- `godot --headless --path . --quit-after 2` booted and exited cleanly

Repository-wide pre-existing failures remain outside this tranche:

- full `dotnet test`: 10,751 passed, 2 failed in modified Codex tests
  (`CodexEntryCatalogTests.Catalog_UnlockRefsMatchTheirConditionShape` and
  `Projection_EmptyAndNullAuthoredSourcesAreSafe`);
- `--evolving-world-selftest`: 2 stale seeding-count assertions fail
  (runtime reports 24/24/30/40 while the selftest expects 11/13/10/12).

No trapping/crafting test failed in the full suite. The repository was already
heavily dirty before this tranche; unrelated changes were preserved.

## Phase 3 — Flagship V integration

Status: IMPLEMENTED

Architecture:

- Localization model: trapping keeps the existing raw `displayName` and
  `description` catalog contract, with stable ID-derived keys and authored
  English/German entries in `assets/l10n/strings.csv`. There is no separate
  trapping localization service; UI resolves through `AshfallLocalization.Tr`.
- Tutorial authority: `OnboardingJourney` owns contextual seen IDs and the
  persisted FIFO queue. `Main` adapts trapping domain events to that authority;
  `TutorialPanel` only renders and acknowledges the active lesson.
- Map authority: `WastelandMapSystem` owns canonical trap markers. The Godot
  map views project marker state and resolve labels at render time.
- Morale authority: `WildlifeTrappingHostSession` resolves `moraleEffect` and
  routes the mutation through `SurvivorsHostSession.Needs.Modify` with
  `NeedKind.Morale`.

Schema/content changes:

- `PreyDefinition.moraleEffect` defaults to zero for old content and validates
  as finite and within the authoring range.
- Authored deltas are rabbit `+1`, rat `-1`, `rad_dog` `-3`, and
  `contaminated_fowl` `-2`.
- Trap deployment sequences are persisted for deterministic butchery identity;
  legacy sites receive deterministic sequence repair on restore.
- Map state persists marker DTOs and canonical trap-site location mappings;
  restore reconciliation removes orphans and repairs missing markers.
- Contextual tutorial seen IDs and queue entries are persisted by onboarding,
  not by `TrapSite`.

Map lifecycle:

| Event | Marker operation |
|---|---|
| deploy | deterministic `trap:{siteId}` upsert |
| break | retain marker and set broken state |
| repair | update the same marker to healthy |
| remove | delete marker |
| restore | reconcile active trap sources and remove orphan markers |

Morale evidence:

| Primary prey | Effect | Route |
|---|---:|---|
| rabbit | +1 | canonical survivor morale authority |
| rat | -1 | canonical survivor morale authority |
| rad_dog | -3 | canonical survivor morale authority |
| contaminated_fowl | -2 | canonical survivor morale authority |

Exactly-once behavior uses the persisted deployment sequence in the butchery
identity and a host-side applied-action set; bycatch is never included in the
primary species lookup or morale source. The authored morale effect performs no
RNG draw and is independent of disease/contamination application.

Compatibility:

- Legacy prey definitions without `moraleEffect` load as zero.
- Legacy active traps receive deterministic deployment identity and map-marker
  reconciliation when a canonical site coordinate exists.
- Saves retain IDs/state, while localized labels, descriptions, tutorial copy,
  and marker labels resolve at render time.
- Old campaigns can receive the first-snare lesson through the deployment
  fallback path; onboarding seen state prevents replay.

## Phase 4 — Verification closure

Status: PASS

Final repository-native results:

- `dotnet build Ashfall.csproj --no-restore --verbosity:minimal`: PASS,
  0 warnings, 0 errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore`: PASS,
  10,760 passed, 0 failed, 0 skipped.
- `godot --headless --path . -- --data-integrity-selftest`: PASS, 299 catalogs,
  0 errors, 0 warnings.
- `godot --headless --path . -- --content-utilization-selftest`: PASS, 583
  catalogs, 0 orphaned, 70 unresolved baseline entries.
- `godot --headless --path . -- --player-panels-uitest`: PASS, 16/16 lifecycle
  gates.
- `python3 scripts/ci/run-gates.py --tier fast`: PASS, all 47/47 gates.
- `git diff --check`: PASS.

The earlier Phase 2 failures are retained as historical baseline notes; the
current closure run supersedes them.

## Phase 5 — Flagship VI: Bycatch, narrative incidents, and trade verification

Status: IMPLEMENTED

Architecture:

- Bycatch remains Core-owned. `WildlifeTrappingSystem` keeps the six-argument
  compatibility callback and emits the typed `OnBycatchResolved` payload only
  after secondary species, yield, toxicity, disease, and contamination state
  is committed. `WildlifeTrappingHostSession` forwards the typed fact to the
  existing event authority.
- Bycatch yield and toxicity use the bycatch species definition; butchery
  outputs primary plus secondary food and routes primary/bycatch health
  consequences through the existing disease and contamination authorities.
  Morale remains primary-catch-only.
- Miss-only atmospheric incidents use the authored IDs
  `trap_sprung_blood_trail`, `trap_bait_stolen`, and
  `trap_human_bootprints`. Their pending IDs are persisted and delivered via
  the existing `HostEventAdapter` using stable source identities.
- `regionalSupply` validation now shares the live
  `RegionalSupplyRouter` vocabulary, so authored trade entries cannot silently
  become unreachable vendor stock.

Persistence and determinism:

- `bycatchYield`, `bycatchToxic`, bycatch health results, and
  `pendingNarrativeEvent` have safe legacy defaults. Restore does not reroll
  resolved bycatch or pending incidents.
- Primary/bycatch rolls use the injected seeded stream; encounter and
  miss-incident rolls use deterministic derived streams. The current
  successful-check order is primary success/species/yield/toxicity, bycatch
  chance/species/yield/toxicity, bycatch health, then primary health. Pending
  narrative delivery clears only after event-authority acceptance, with a
  second stable source ledger preventing duplicate dispatch.

Trade evidence:

| Trap | regionalSupply | Vendor price | Craft input value | Policy |
|---|---|---:|---:|---|
| improvised wire | general | 10.0 | 8.0 | convenience premium |
| box | settlement | 22.0 | 7.2 | convenience premium |
| fish | coastal | 16.0 | 12.0 | convenience premium |
| body grip | none | unavailable | — | craft/loot-only |

The invalid `4.0 > 8.0` assertion is not used; the verified policy is a
minimum 1.1× purchase-price floor over craft input value.

Plan VI verification:

- `WildlifeTrappingPlanVITests`: 10/10 passed.
- `dotnet build Ashfall.csproj --no-restore --verbosity:minimal`: PASS,
  0 warnings, 0 errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore`:
  PASS, 10,776 passed, 0 failed, 0 skipped.
- `godot --headless --path . -- --trapping-selftest`: PASS, 10/10 host
  checks.
- `godot --headless --path . -- --data-integrity-selftest`: PASS, 299
  catalogs, 0 findings.
- `godot --headless --path . -- --content-utilization-selftest`: PASS,
  583 catalogs, 0 orphaned catalogs.
- `bash scripts/ci/generate-cli-catalog.sh --check`: PASS.
- `python3 scripts/ci/run-gates.py --tier fast`: PASS, all 47/47 gates.
- `git diff --check`: PASS.
