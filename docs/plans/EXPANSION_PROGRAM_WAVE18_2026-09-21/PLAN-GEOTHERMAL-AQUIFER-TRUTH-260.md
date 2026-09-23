# PLAN-GEOTHERMAL-AQUIFER-TRUTH-260 — Heat–Water Coupling: Warm Aquifers & Well Interference

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-AQUIFER-MONITORING-TRUTH-164, PLAN-GEOTHERMAL-PLANT-TRUTH-191, PLAN-FLUID-LOGISTICS-TRUTH-179.
**Non-goals:** no aquifer stock model (Plan 164), no plant model (Plan 191), no
pipe network (Plan 179).

## 1. Outcome
`Shelter/GeothermalAquiferSystem.cs` (**303 lines**) is reachable and
unaddressed: the **coupling** where geothermal heat and groundwater meet —
warm aquifers, thermal drawdown, and well interference between the plant
(Plan 191) and water wells (Plan 164). Neither plan owns the interaction, so
the two can silently double-book the same underground resource.

| Deliverable | Detail |
|---|---|
| Coupling model | shared subsurface state read by both plans; one owner for temperature and one for water level, explicitly linked |
| Interference rule | plant extraction and well drawdown interact per a documented table; effects are visible in both systems' surfaces |
| Thermal limits | over-extraction lowers the plant's source temperature and the aquifer's yield together, per the table |
| Monitoring | the interaction is observable through Plan 164's piezometry readings |
| Save truth | coupled state restores atomically; no desync between the two systems on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs` (303 lines; unaddressed — Wave 18 audit).
- Plan 164 owns water level readings; Plan 191 owns plant output; this plan is their shared resource — a documented seam, not a new resource.
- `SaveSectionRegistry`: `deep_well` already tracks well state; the coupling must ride one section, not two.
- Plan 179's network carries the water the coupling affects.

## 3. Packages
- **GAT-260A** coupling model + shared-state owner table.
- **GAT-260B** interference table + fixture per quadrant.
- **GAT-260C** atomic save test (no desync).
- **GAT-260D** monitoring visibility through Plan 164.
- **GAT-260E** bounds/limits fixtures.

## 4. Acceptance & verification
- Interference follows the table; both surfaces show the coupled effect.
- Save/load restores atomically; no cross-system disagreement after reload.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Double-booked resource → the coupling table and the atomic save test are the guards.
Silent interaction → both surfaces display it; a fixture asserts visibility.

---

## 6. Expanded census (7 files · 1,926 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Loader 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AquiferPiezometerEngine.cs` | 829 | System | — | 1 | 0 | 2 |
| `GeothermalAquiferHeadlessDemo.cs` | 119 | Demo | — | 0 | 0 | 1 |
| `GeothermalAquiferState.cs` | 27 | DTO/Type | — | 0 | 0 | 0 |
| `GeothermalAquiferSystem.cs` | 303 | System | **yes** | 0 | 0 | 2 |
| `GeothermalCatalog.cs` | 58 | Catalog | — | 0 | 0 | 0 |
| `GeothermalCatalogLoader.cs` | 34 | Loader | — | 0 | 0 | 0 |
| `GeothermalOrcSystem.cs` | 556 | System | — | 0 | 0 | 2 |

**Totals:** 1 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `geothermal_drilling_depths.json` | object[2 keys] |
| `geothermal_strata_catalog.json` | object[2 keys] |
| `geothermal_borehole_logs.json` | array[7] |
| `geothermal_steam_vent_diagnostics.json` | array[7] |
| `geothermal_steam_well_logs.json` | array[8] |

**State surfaces:** `AquiferPiezometerEngine.cs`, `GeothermalAquiferHeadlessDemo.cs`, `GeothermalAquiferSystem.cs`, `GeothermalOrcSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 9 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

Domain files: 7. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DEEP-STRATA-83` | 6 |
| `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 6 |
| `PLAN-AQUIFER-MONITORING-TRUTH-164` | 4 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 3 |
| `PLAN-ENERGY-NUCLEAR-48` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `GAT-260A` | `GeothermalAquiferState.cs` |
| `GAT-260B` | no name match — resolve at claim time |
| `GAT-260C` | no name match — resolve at claim time |
| `GAT-260D` | no name match — resolve at claim time |
| `GAT-260E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 7; intra-domain edges: **3**; isolated files:
**4**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `GeothermalAquiferHeadlessDemo` | `GeothermalAquiferSystem` |
| `GeothermalAquiferState` | `GeothermalAquiferSystem` |
| `GeothermalAquiferSystem` | `GeothermalAquiferState` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `GeothermalAquiferSystem` | 2 |
| `GeothermalAquiferState` | 1 |
| `AquiferPiezometerEngine` | 0 |
| `GeothermalAquiferHeadlessDemo` | 0 |
| `GeothermalCatalog` | 0 |
| `GeothermalCatalogLoader` | 0 |
| `GeothermalOrcSystem` | 0 |

**Class split:** hub 2 · sink 0 · source 1 · isolated 4.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 7. Host files: **6** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/GeothermalAquiferHostSession.cs`, `src/Host/GeothermalAquiferSaveStore.cs`, `src/Host/PiezometerHostSession.cs`, `src/Host/Plans74To77HostSessions.cs`, `src/Main.Piezometer.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Plans74To77SystemsTests.cs`, `Ashfall.Core.Tests/Shelter/GeothermalAquiferIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/GeothermalAquiferSystemTests.cs`, `Ashfall.Core.Tests/Water/Plan189IntakeAdvisoryBridgeTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `geothermal_aquifer` |
| `geothermal_orc` |
| `piezometer_network` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/geothermal_drilling_depths.json` |
| `Assets/StreamingAssets/Data/geothermal_strata_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_borehole_logs.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json` |
| `Assets/StreamingAssets/Data/narrative/geothermal_steam_well_logs.json` |
| `Assets/StreamingAssets/Data/piezometer_network_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **15**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/GeothermalAquiferHostSession.cs` |
| `src/Host/GeothermalAquiferSaveStore.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/PiezometerHostSession.cs` |
| `src/Host/PiezometerSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.Piezometer.cs` |
| `src/UI/AquiferTreatyConcessionPanel.cs` |
| `src/UI/GeothermalAquiferPanel.cs` |
| `src/UI/GeothermalSteamTurbinePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |
| `piezometer_network` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **4**
(CODEX_ONLY 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `geothermal_drilling_depths.json` | UNRESOLVED |
| `narrative/geothermal_borehole_logs.json` | CODEX_ONLY |
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |
| `narrative/geothermal_steam_well_logs.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 12 · catalogs 10 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-GEOTHERMAL-AQUIFER-TRUTH-260
wave: 18
status: PROPOSED — foreman claim required
packages: GAT-260A, GAT-260B, GAT-260C, GAT-260D, GAT-260E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferHostSession.cs  # §19 candidate host surface
  - src/Host/GeothermalAquiferSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/geothermal_drilling_depths.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/geothermal_strata_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --ice-road-tick-demo
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
