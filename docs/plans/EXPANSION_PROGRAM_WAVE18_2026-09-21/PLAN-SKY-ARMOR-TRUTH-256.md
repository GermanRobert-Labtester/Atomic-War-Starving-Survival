# PLAN-SKY-ARMOR-TRUTH-256 — Overhead Protection: Layers, Gaps & Weather Load

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BASE-DEFENSE-RAIDS-61, PLAN-SKY-DEFENSE-TRUTH-135, PLAN-WEATHER-INTELLIGENCE-TRUTH-218, PLAN-SHELTER-ARCHITECTURE-40.
**Non-goals:** no raid resolution (Plan 61), no aerial battery (Plan 135), no
hardening policy (Plan 218), no build costs (Plan 40).

## 1. Outcome
`Shelter/SkyLayerArmorSystem.cs` (**144 lines**) is reachable and unaddressed:
overhead armor layers protecting against falling debris and aerial attack. It
is a third protection layer beside perimeter (Plan 165) and sky defense
(Plan 135) — and overlaps are likely unless its coverage contract is explicit.

| Deliverable | Detail |
|---|---|
| Layer model | armor layers with coverage cells from built state (Plan 40); gaps are visible |
| Impact reduction | falling debris/ordnance damage is reduced per layer coverage, routed to the damage owner (Plan 61/193/28) |
| Weather load | heavy weather (Plan 28) stresses layers via Plan 119's contract; weak layers fail visibly |
| Repair | patching consumes materials via Plan 93 |
| Save truth | coverage and conditions restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` (144 lines; unaddressed — Wave 18 audit).
- Plan 165 covers the perimeter; Plan 135 the battery; this plan is the roof layer — three distinct surfaces stated.
- Plan 40 built state supplies coverage; Plan 119 decay.

## 3. Packages
- **SAT-256A** layer/coverage model + gap visibility.
- **SAT-256B** impact-reduction routing tests.
- **SAT-256C** weather-load/decay fixtures.
- **SAT-256D** repair path + conservation.
- **SAT-256E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Coverage gaps are visible; impact damage routes with reduction per coverage.
- Weather load follows Plan 119's contract; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Three overlapping defense layers → the boundary table names all three; each plan asserts it.
Invisible roof → gaps and conditions are surfaced in the structure view.

---

## 6. Expanded census (2 files · 366 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SkyLayerArmorCatalog.cs` | 222 | Catalog | — | 0 | 0 | 0 |
| `SkyLayerArmorSystem.cs` | 144 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `sky_layer_armor_catalog.json` | object[2 keys] |
| `sky_defense_ordnance.json` | object[2 keys] |
| `armored_crawler_modules.json` | object[2 keys] |
| `vehicle_armor_grades.json` | object[3 keys] |
| `armored_cockroach_hive_logs.json` | array[8] |
| `armored_locomotive_manifests.json` | array[7] |

**State surfaces:** `SkyLayerArmorSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 17 name references across the test tree |
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
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SAT-256A` | `SkyLayerArmorCatalog.cs`, `SkyLayerArmorSystem.cs` |
| `SAT-256B` | no name match — resolve at claim time |
| `SAT-256C` | no name match — resolve at claim time |
| `SAT-256D` | no name match — resolve at claim time |
| `SAT-256E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 8. Host files: **5** · Test files: **15** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/HostCli.DynamicWorld.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SkyDefense.cs`, `src/Host/WorldHostSession.cs`, `src/Main.FlagshipInstitutions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`, `Ashfall.Core.Tests/Integration/Roadmap42Batch1IntegrationTests.cs`, `Ashfall.Core.Tests/IslandBridgesTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `armored_crawlers` |
| `dynamic_quests` |
| `mental_health_crisis` |
| `perimeter_defense` |
| `sky_defense_battery` |
| `vehicle_garage` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--defense-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--sky-defense-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnHiveInstalled` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnVehicleBreakdown` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnVehicleStateChanged` | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/sky_layer_armor_catalog.json` |
| `Assets/StreamingAssets/Data/vehicle_armor_grades.json` |

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

Host files (`src/`) whose names share a domain token: **0**
(0 of them panels/HUD).

| Host file |
|---|
| — | no host filename shares a token with this domain |

**Verdict:** no host file shares a token with this domain — the surface may be driven through a generic panel, or may not be surfaced at all. Verify before claiming a route.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

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
(UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `sky_layer_armor_catalog.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 0 · catalogs 3 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SKY-ARMOR-TRUTH-256
wave: 18
status: PROPOSED — foreman claim required
packages: SAT-256A, SAT-256B, SAT-256C, SAT-256D, SAT-256E
claim paths:
  - Assets/StreamingAssets/Data/sky_layer_armor_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/vehicle_armor_grades.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --defense-selftest
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
