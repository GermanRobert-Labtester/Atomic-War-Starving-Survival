# PLAN-WAYSTATION-NETWORK-TRUTH-153 — Route Nodes, Services & Network State

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-NOMADS-CARAVAN-CULTURE-82, PLAN-SPATIAL-SIM-AUTHORITY-95.
**Implementation scaffold:** [`PLAN-WAYSTATION-NETWORK-TRUTH-153_APPENDIX-A_SCAFFOLD.md`](PLAN-WAYSTATION-NETWORK-TRUTH-153_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-TRANSPORT-EXPEDITION-30` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no travel resolution (Plan 30), no caravan culture content
(Plan 82), no route topology (Plan 95 owns it).

## 1. Outcome
`Waystation/` holds `WaystationCatalogLoader.cs` and
`WaystationNetworkSystem.cs`: a network of stops along routes. Nothing states
what a waystation provides, how network state (open/stocked/hostile) is owned,
or how nodes connect to Plan 95's route topology — so waystations become either
decoration or an unowned service layer.

| Deliverable | Detail |
|---|---|
| Node model | each waystation with services (water, rest, repair, trade), stock, and status; catalog-backed via its loader |
| Network semantics | nodes connect along existing route edges from Plan 95; no independent map |
| Status ownership | open/closed/hostile/hostile-cleared transitions with an event and a day; one owner per transition |
| Service effects | resting/repair/trade effects route to existing owners (needs, maintenance Plan 119, economy Plan 96) |
| Persistence | node stock/status restores; a visited network does not re-seed on load |

## 2. Evidence
- `Assets/Ashfall.Core/Waystation/`: the two files above (verified).
- Plan 95 owns route edges and spatial facts; waystations attach to them.
- Plan 30 owns expedition travel that uses nodes as legs/milestones.
- Plan 119 owns maintenance effects a repair service may grant.

## 3. Packages
- **WNT-153A** node model + service table (catalog-backed).
- **WNT-153B** network attachment to Plan 95 route edges + no-orphan-node check.
- **WNT-153C** status transition table + event/day tests.
- **WNT-153D** service effect routing tests (needs/maintenance/economy owners).
- **WNT-153E** persistence round-trip; no re-seed on load.

## 4. Acceptance & verification
- Every node attaches to at least one route edge; an unattached node is reported, not silently visible.
- Status transitions occur once per event; services effect the named owners.
- Save/load preserves stock/status exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Waystation/` (create if absent).

## 5. Risks
Second map → nodes reference Plan 95 edges; the attachment check enforces it.
Service duplication → effects route to existing owners; a test asserts no local counters.

---

## 6. Expanded census (2 files · 366 lines)

Scope: `Assets/Ashfall.Core/Waystation/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Loader 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `WaystationCatalogLoader.cs` | 165 | Loader | **yes** | 0 | 0 | 0 |
| `WaystationNetworkSystem.cs` | 201 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `waystations.json` | object[2 keys] |

**State surfaces:** `WaystationNetworkSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Waystation/` (create if absent) |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-TRANSPORT-EXPEDITION-30` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WNT-153A` | `WaystationCatalogLoader.cs` |
| `WNT-153B` | `WaystationNetworkSystem.cs` |
| `WNT-153C` | no name match — resolve at claim time |
| `WNT-153D` | no name match — resolve at claim time |
| `WNT-153E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **5** · Test files: **3** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/HostCli.Cartography.cs`, `src/Host/HostCli.cs`, `src/Host/WaystationHostSession.cs`, `src/Main.ShelterInfrastructure.cs`, `src/UI/WaystationNetworkPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Plan56Phase3Tests.cs`, `Ashfall.Core.Tests/Plan56Phase6Tests.cs`, `Ashfall.Core.Tests/World/Plan16CartographyTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/waystations.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **20** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `expanded_shelter` |
| `infrastructure` |
| `piezometer_network` |
| `route_infrastructure` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--gpr-cartography-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |
| `--rumor-network-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |
| `Assets/StreamingAssets/Data/pneumatic_network_catalog.json` |
| `Assets/StreamingAssets/Data/rail_network.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **10**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/RumorNetworkHostSession.cs` |
| `src/Host/RumorNetworkSaveStore.cs` |
| `src/Host/RumorNetworkSelfTest.cs` |
| `src/Host/WaystationHostSession.cs` |
| `src/Host/WaystationSaveStore.cs` |
| `src/Main.RumorNetwork.cs` |
| `src/UI/PanelSceneLoader.cs` |
| `src/UI/WaystationNetworkPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `caravan_trade_network` | no |
| `piezometer_network` | no |
| `waystation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `rail_network.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 10 · catalogs 4 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WAYSTATION-NETWORK-TRUTH-153
wave: 12
status: PROPOSED — foreman claim required
packages: WNT-153A, WNT-153B, WNT-153C, WNT-153D, WNT-153E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/Host/RumorNetworkHostSession.cs  # §19 candidate host surface
  - src/Host/RumorNetworkSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/piezometer_network_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/pneumatic_network_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
