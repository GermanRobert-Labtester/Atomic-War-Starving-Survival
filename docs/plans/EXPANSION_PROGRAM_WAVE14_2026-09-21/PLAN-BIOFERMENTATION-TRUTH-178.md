# PLAN-BIOFERMENTATION-TRUTH-178 — Cultures, Batches & Contamination Outcomes

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-PRESERVATION-TRUTH-118, PLAN-PHARMACEUTICAL-TRUTH-167.
**Implementation scaffold:** [`PLAN-BIOFERMENTATION-TRUTH-178_APPENDIX-A_SCAFFOLD.md`](PLAN-BIOFERMENTATION-TRUTH-178_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PRESERVATION-TRUTH-118` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no factory engines (Plan 45), no spoilage model (Plan 118), no
pharma dosing (Plan 167).

## 1. Outcome
`Shelter/BioFermentationEngine.cs` (**786 lines**) is reachable and unaddressed:
fermentation produces food, feed, and industrial inputs through living cultures
that can fail. Nothing states batch timing, culture health, or contamination —
so a fermentation vat is either a slow recipe or a source of free goods.

| Deliverable | Detail |
|---|---|
| Culture model | cultures with health/vitality and a documented propagation rule; a dead culture requires a new sample |
| Batch lifecycle | prepared → fermenting → done/spoiled with day-based timing on the canonical clock |
| Contamination | documented contamination inputs (hygiene, temperature, bad input) with a visible spoiled outcome, never a silent loss |
| Outputs | outputs enter inventory via Plan 93; pharmaceutical-grade outputs feed Plan 167's inputs |
| Save truth | culture health and in-progress batches restore; a load never re-rolls fermentation |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs` (786 lines; unaddressed — Wave 13 audit).
- Plan 118 owns spoilage of finished goods; fermentation spoilage is the production-side sibling with a stated boundary.
- Plan 167 consumes gradeable outputs; Plan 93 verifies counts.
- Plan 119's decay contract applies to equipment, not cultures; the plan states that distinction.

## 3. Packages
- **BFT-178A** culture model + propagation rule.
- **BFT-178B** batch lifecycle + clock tests.
- **BFT-178C** contamination inputs + spoiled-outcome fixture.
- **BFT-178D** output routing (inventory/pharma) + conservation check.
- **BFT-178E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Batch timing uses game days; a culture's health changes only through documented inputs.
- Contamination produces a visible spoiled batch and consumes the input (no silent loss or preserve).
- Outputs balance; pharma-grade outputs appear in Plan 167's inputs.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Free-goods loop → culture propagation cost and contamination are the constraints.
Overlap with 118 → production-side vs storage-side spoilage; both plans state the boundary.

---

## 6. Expanded census (1 files · 786 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BioFermentationEngine.cs` | 786 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `bio_fermentation_catalog.json` | object[3 keys] |
| `fermentation_crock_airlock_assays.json` | array[7] |

**State surfaces:** `BioFermentationEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 1 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `BFT-178A` | no name match — resolve at claim time |
| `BFT-178B` | no name match — resolve at claim time |
| `BFT-178C` | no name match — resolve at claim time |
| `BFT-178D` | no name match — resolve at claim time |
| `BFT-178E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/BioFermentationHostSession.cs`, `src/Main.Plans126_129.cs`, `src/UI/BioFermentationPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `airlock_security` |
| `bio_fermentation` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **10** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
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

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bio_fermentation_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/beeswax_rendering_dipping_assays.json` |
| `Assets/StreamingAssets/Data/narrative/candle_dip_mould_assays.json` |
| `Assets/StreamingAssets/Data/narrative/chrome_alum_tanning_assays.json` |
| `Assets/StreamingAssets/Data/narrative/currying_burnishing_assays.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/fermentation_crock_airlock_assays.json` |
| `Assets/StreamingAssets/Data/narrative/forge_charcoal_ash_assays.json` |
| `Assets/StreamingAssets/Data/narrative/fulling_trough_nap_assays.json` |
| `Assets/StreamingAssets/Data/narrative/green_sand_bentonite_assays.json` |
| `Assets/StreamingAssets/Data/narrative/kiln_draw_trial_assays.json` |
| `Assets/StreamingAssets/Data/narrative/mill_dampener_tempering_assays.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (101 files, 688 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 688 cases sit under matching regions — run those first (`Campaign`, `Economy`, `Education`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **513**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **12**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **56**
(CODEX_ONLY 38, GAMEPLAY_CONSUMED 13, OPTIONAL 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `foundry_accords.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 22 (laddered 0) · RNG streams 12 · host files 23 · catalogs 22 · test regions 7 · flags 10

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BIOFERMENTATION-TRUTH-178
wave: 14
status: PROPOSED — foreman claim required
packages: BFT-178A, BFT-178B, BFT-178C, BFT-178D, BFT-178E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bio_fermentation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/beeswax_rendering_dipping_assays.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
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
