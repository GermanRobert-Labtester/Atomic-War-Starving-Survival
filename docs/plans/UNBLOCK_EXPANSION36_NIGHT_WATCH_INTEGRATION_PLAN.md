# UNBLOCK — Expansion 36: The Watch / Night Watch Patrol Readiness

**Status:** COMPLETE — user-authorized full host integration sealed 2026-09-24
**Claim:** `claim-unblock-expansion-36-night-watch-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine must be reachable
from campaign composition, daily simulation, persistence, a real player command
surface, and focused runtime verification.

## Outcome

Make `NightWatchPatrolReadinessEngine` the live readiness/read-model authority
for the shelter watch while preserving the existing owners:

- `PerimeterDefenseSystem` remains the owner of physical perimeter emplacements,
  sector alarms, false alarms, and intrusion history.
- `SoundRangingThreatEngine` remains the owner of acoustic observations and
  threat estimates; the watch never manufactures contacts.
- `TerritoryControlSystem` remains the owner of territory control, claims,
  contest state, and supply lines.
- `DutyRosterSystem` remains the owner of survivor watch assignments and
  general fitness/eligibility validation.
- `ShelterSecuritySystem` remains the owner of gate locks, access, and lockdown.
- `JournalSystem` remains the owner of human-readable incident/debrief facts.
- `NightWatchPatrolReadinessEngine` remains pure and engine-free; the new
  operations layer owns only watch-specific posts, routes, drills, readiness
  projections, and the host adapters.

## Current premise evidence (2026-09-24)

- `Assets/Ashfall.Core/World/NightWatchPatrolReadinessEngine.cs` contains the
  signed pure calculations, but has no production caller outside its focused
  Core test.
- `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs` is test/orphan
  infrastructure; current live territory ownership is
  `Assets/Ashfall.Core/Factions/TerritoryControlSystem.cs`, already restored and
  saved by the Plan 134 host seam.
- `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` already owns
  perimeter sectors, emplacements, alarm state, false alarms, and bounded
  intrusion history. The Watch must compose it, not replace it.
- `Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs` and
  `src/Host/SoundRangingHostSession.cs` already own acoustic estimates and
  expose a typed threat-estimate event.
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` owns survivor eligibility,
  role assignment, save state, and fitness hooks; its state can accept an
  additive watch-shift sub-object without a parallel roster.
- `Assets/Ashfall.Core/Shelter/ShelterSecuritySystem.cs` and
  `src/Host/ShelterSecurityHostSession.cs` own gate locks, access, and lockdown.
- `Assets/StreamingAssets/Data/perimeter_defenses.json` and
  `faction_territory.json` are already canonical gameplay catalogs. The
  authored Watch operations catalog will reference only existing canonical
  location/sector vocabulary and will not create rival territory or combat data.
- `NightWatchCatalog` currently has test-only consumers; the host integration
  will load the existing narrative logbook and surface it in the watch panel.

## Material plan divergence and resolution

The design bible names `PatrolTerritoryAuthority` and non-existent “night watch
catalog state” as persistence owners. Current source disproves that premise.
The integration therefore uses the current single authorities above and adds the
watch-specific state to the existing `perimeter_defense` and `duty_roster` save
owners. This honors the design's “no new save section” rule and avoids a second
territory, perimeter, roster, gate, or detection authority.

## Implementation phases

1. **Catalog and pure contracts** — strict `night_watch_operations.json`
   loader; post/route/drill/gate/detection/alarm definitions; bounded state
   shapes; deterministic IDs and validation.
2. **Canonical owner extensions** — additive watch shifts in
   `DutyRosterSystem`; watch operations sub-state and catalog binding in
   `PerimeterDefenseSystem`; restore/clone/schema guards and daily maintenance.
3. **Readiness projection and host session** — live evaluation from roster,
   perimeter, sound-ranging, security, weather, and territory facts; journal
   bridges; no shadow calculations in UI.
4. **Campaign lifecycle** — compose on both fresh and restored paths, daily
   advance through the existing expanded-shelter day path, save through the
   existing owner sections, and reset cleanly on slot/campaign change.
5. **Player surface and verification** — routed `NightWatchPanel`, real commands,
   accessibility-safe text/status presentation, CLI/runtime probe, focused Core
   and host tests, save/determinism checks, and generator/integrity gates.

## Acceptance criteria

- The pure engine is called by a live host/session and a routed panel; no
  production orphan references remain.
- A player can inspect current watch/gate readiness, assign a valid watch shift,
  activate/repair a post, walk a route, run a drill, and seal/open the gate
  through commands that mutate the canonical owners.
- Daily campaign advancement updates watch maintenance/readiness and preserves
  state through the existing `perimeter_defense` and `duty_roster` save owners;
  old saves restore with safe empty/default watch operations.
- Acoustic/territory/perimeter facts are read from their owners; the Watch never
  creates a second contact, combat, territory, or alarm registry.
- Focused tests prove strict catalog rejection, normal/empty/boundary behavior,
  owner composition, persistence round-trip, deterministic daily progression,
  and real host/panel/CLI wiring.

## Focused verification

```text
bash scripts/run_test.sh Ashfall.Core.Tests/World/NightWatchPatrolReadinessEngineTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/World/NightWatchOperationsTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/World/NightWatchHostIntegrationTests.cs
godot --headless --path . -- --patrol-encounter-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-save-store-matrix.py --check
python3 scripts/ci/generate-selftest-manifest.py --check
```

## Non-goals

- No new save section or parallel perimeter, territory, acoustic, combat, or
  gate authority.
- No automatic combat, hostile inference, or fabricated contact resolution.
- No Unity dependency or engine-specific Core API.
- No unseeded randomness or wall-clock-derived state.

## Implementation log — 2026-09-24

### Phase 1 — Catalog and pure contracts: PASS

- Added strict `night_watch_operations.json` loader with 15 posts, 12 routes,
  10 gate rules, 12 detection profiles, 8 alarm protocols, and 10 drills.
- Added duplicate/range/sector/reference validation and canonical location
  validation. Old/missing data remains a safe empty watch surface in isolated
  validator fixtures; the shipped catalog is strict.
- Added pure `NightWatchReadinessProjection`; all arithmetic delegates to
  `NightWatchPatrolReadinessEngine`.
- Evidence: `NightWatchOperationsTests` 7/7; Core build 0/0.

### Phase 2 — Canonical owner extensions: PASS

- Added bounded, clone-safe watch operations state under the existing
  `PerimeterDefenseSave` (`watch_operations`) and watch shifts under the
  existing `DutyRosterSystemState` (`watch_shifts`).
- Added schema guards, old-save defaults, deterministic shift completion,
  post condition/maintenance, route/debrief, drill, and readiness projections.
- Physical perimeter equipment/alarms/logs, roster fitness, gate locks,
  acoustic estimates, and territory claims remain owned by their original
  systems.
- Evidence: `NightWatchOperationsTests` 7/7, `NightWatchHostIntegrationTests`
  6/6, existing PerimeterDefense 24/24 and DutyRoster 49/49.

### Phase 3 — Host/session and campaign lifecycle: PASS

- Added `NightWatchHostSession` and `Main.NightWatch.cs`.
- Fresh and restored campaign composition loads the catalog and existing
  `night_watch_logbook.json`; current day is set before the panel is opened.
- Daily perimeter advancement now advances watch maintenance/readiness after
  the canonical perimeter tick.
- Watch mutations route through canonical roster/perimeter/security/journal
  owners; save is additive to existing owner sections.
- Real campaign journey confirms `[NightWatch] 15 posts, 12 routes, 10 drills
  loaded` on both fresh and restored composition.

### Phase 4 — Player surface and runtime verification: PASS

- Added routed `The Watch // Patrol Readiness` panel with live readiness,
  coverage, gate, posts, shifts, routes, drills, incident log, and real
  commands for assignment, repair, activation, patrol, debrief, drill, gate
  alarm, and gate seal/open.
- Added runtime checks to the existing patrol-encounter verb (the design's
  named owning verb), including panel bind/unbind and source wiring checks.
- Evidence: `night_watch_selftest` 16/16; `--patrol-encounter-selftest` PASS;
  `--panel-bind-lifecycle-selftest` 21/21; `--player-panels-uitest` PASS;
  `--real-campaign-journey-selftest` PASS.

### Phase 5 — Integrity and closure: PASS

- `--data-integrity-selftest`: PASS, 0 errors, 423 catalogs (5 documented
  pre-existing warnings).
- `--content-utilization-selftest`: PASS; the new catalog is gameplay-consumed.
- `--port-contract-selftest`: PASS, 301 seams.
- Architecture, catalog, selftest-manifest, plan-integration-audit, docs-index,
  and triad-drift generators/gates are synchronized.
- Core and Godot host builds: 0 errors / 0 warnings.

### Follow-up correctness seal — PASS

- Fixed the live UI refresh cycle: derived `watch.readiness_evaluated` events
  mark persistence dirty but do not request another panel refresh, and unchanged
  derived snapshots no longer emit duplicate events.
- Preserved the survivor's actual fatigue at watch-shift assignment and rejected
  shifts crossing the 24-hour boundary.
- Added deterministic normalization/deduplication for watch state and roster
  shift records, plus legacy checksum verification for duty-roster saves written
  before the additive watch fields existed.
- Evidence: `NightWatchHostIntegrationTests` 6/6; `NightWatchOperationsTests`
  7/7; `DutyRosterIntegrationTests` 40/40; host build and patrol runtime probe
  remain green; the runtime probe now also verifies first-use alarm relay setup.

## Definition of done

- [x] Pure engine has a live production caller.
- [x] Authored operations data is strict, reachable, and integrity-validated.
- [x] Patrol/readiness, fatigue, route, drill, gate, and acoustic projections
  use canonical owners rather than shadow state.
- [x] Fresh and restored campaign composition are live.
- [x] Daily campaign advancement and save/load are wired without a new save
  section.
- [x] Player can discover and operate the watch through a routed panel.
- [x] Focused Core, host, runtime, integrity, determinism-adjacent lifecycle,
  and panel checks pass.
- [x] No Unity dependency, no second territory/combat/detection/gate authority,
  no wall-clock/unseeded randomness, and no partial closeout.

## Reconciled external baseline

The active 186/188 claim was stale/incomplete on its focused test paths; its
shared composition seams were transferred to this integrator package without
reverting any landed files. A concurrent untracked dependency-taper probe was
also observed; it was left outside this claim and the host build is green after
its own compile fix. Expansion 39 (The Reagent) and the later concurrent 40/41 plus Plan 162/177/184
packages have since landed their shared save/architecture seams. During the
final integration pass we repaired the missing reagent envelope capture and
the resulting shared compile/selftest drift, then regenerated the global
matrices; unrelated worktree changes remain preserved.
