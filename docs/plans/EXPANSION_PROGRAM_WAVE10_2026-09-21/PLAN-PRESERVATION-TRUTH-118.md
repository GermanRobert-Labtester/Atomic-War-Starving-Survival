# PLAN-PRESERVATION-TRUTH-118 — Food, Seed & Specimen Preservation with Spoil Curves

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FOOD-CUISINE-39, PLAN-WATER-AGRICULTURE-46, PLAN-DATA-SCHEMA-COVERAGE-90.
**Non-goals:** no new food catalog, no cooking rewrite (Plan 39), no parallel
inventory store (Plan 93).

## 1. Outcome
Preservation exists in fragments: `Farming/OilseedPressingEngine.cs` is
host-unreachable; `Medical/LyophilizationSystem.cs` holds a **fully dead**
`LyophilizationEngine` (Plan 1 dead list); data catalogs
`Narrative/CryoPreservationCatalog.cs` and `Narrative/SeedBankPreservationCatalog.cs`
describe preservation knowledge; storage/spoilage rules are unstated. The gap
is a spoil contract: what rots, how fast under which storage, and what
preservation methods actually buy.

| Deliverable | Detail |
|---|---|
| Spoil classes | per food class, a rate curve keyed to storage condition (cool/dry/sealed/powered) — deterministic, day-based |
| Preservation methods | pressing, drying, sealing, cryo each modify the curve with a documented factor and an owner per method |
| Dead-engine decision | `LyophilizationEngine` is either revived through a named owner or its catalog knowledge stays codex-only (explicit decision, Plan 118's finding) |
| Inventory marking | spoilage reduces quantity/quality through the Plan 93 seam; no separate decay ledger |
| Panel truth | storage UI shows current curve state from the authority |

## 2. Evidence
- Plan 1 Appendix A: `OilseedPressingEngine` host-unreachable; dead-type list includes `LyophilizationEngine` (`Medical/LyophilizationSystem.cs`).
- `Narrative/CryoPreservationCatalog.cs`, `Narrative/SeedBankPreservationCatalog.cs` exist as knowledge catalogs (re-verify per package).
- `Kitchen/FoodTypeSystem`, `Cooking/CookingSystem` are host-unreachable (Plan 1) — food classes are already modeled.
- Plan 93's conservation wrapper measures quantity deltas, which spoilage must balance.

## 3. Packages
- **PRT-118A** spoil class + curve table (data-backed, schema-checked per Plan 90).
- **PRT-118B** condition factors + method factors with one test per method.
- **PRT-118C** dead-engine decision record (revive via owner or codex-only).
- **PRT-118D** inventory integration through Plan 93's seam; conservation test includes spoilage.
- **PRT-118E** panel truth check (display equals authority curve state).

## 4. Acceptance & verification
- Same storage + same day count → same spoilage across runs (no wall clock).
- Each preservation method changes the curve by its documented factor.
- Spoilage deltas balance in the conservation wrapper.
- `bash scripts/run_test.sh` on the food/inventory regions.

## 5. Risks
Curve tuning by hand → curves are data rows; this plan validates mechanics, balance stays in Plan 73's lane.
Dead code revival → the decision record either names an owner or keeps the knowledge codex-only.

---

## 6. Expanded census (5 files · 1,599 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `OilseedPressingEngine.cs` | 246 | System | **yes** | 0 | 0 | 0 |
| `LyophilizationSystem.cs` | 332 | System | **yes** | 0 | 0 | 2 |
| `CryoVaultSystem.cs` | 554 | System | — | 0 | 0 | 3 |
| `FoodPreservationCatalog.cs` | 140 | Catalog | — | 0 | 0 | 0 |
| `FoodPreservationSystem.cs` | 327 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `cryo_cultivars.json` | object[4 keys] |
| `cryogenic_air_separation.json` | object[3 keys] |
| `lyophilization_catalog.json` | object[2 keys] |
| `food_preservation.json` | object[4 keys] |
| `cryo_germplasm_viability_audits.json` | array[8] |
| `cryo_seed_ampoule_logs.json` | array[8] |

**State surfaces:** `LyophilizationSystem.cs`, `CryoVaultSystem.cs`, `FoodPreservationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 15 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 5. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-FOOD-CUISINE-39` | 2 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 1 |
| `PLAN-WATER-AGRICULTURE-46` | 1 |
| `PLAN-CRYO-VAULT-TRUTH-206` | 1 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `PRT-118A` | no name match — resolve at claim time |
| `PRT-118B` | no name match — resolve at claim time |
| `PRT-118C` | `OilseedPressingEngine.cs` |
| `PRT-118D` | no name match — resolve at claim time |
| `PRT-118E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/Plans130To133HostSessions.cs`, `src/Main.Plans130_133.cs`, `src/Main.Plans62_65.cs`, `src/Main.PlansB68_B69.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Farming/OilseedPressingTests.cs`, `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs`, `Ashfall.Core.Tests/Plans130To133CoreTests.cs`, `Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `FoodPreservationSystem` | `FoodPreservationCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `cryo_vault` |
| `food_preservation` |
| `lyophilization` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/cryo_cultivars.json` |
| `Assets/StreamingAssets/Data/food_preservation.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/lyophilization_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/architect_vault_audits.json` |
| `Assets/StreamingAssets/Data/narrative/cryo_germplasm_viability_audits.json` |
| `Assets/StreamingAssets/Data/narrative/cryo_seed_ampoule_logs.json` |
| `Assets/StreamingAssets/Data/narrative/vault_seal_breach_logs.json` |

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

Host files (`src/`) whose names share a domain token: **3**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/CryoVaultSaveStore.cs` |
| `src/Host/FoodPreservationSaveStore.cs` |
| `src/UI/VaultDoorBreachingPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `cryo_vault` | no |
| `food_preservation` | no |
| `lyophilization` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 4, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `lyophilization_catalog.json` | UNRESOLVED |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/cryo_germplasm_viability_audits.json` | CODEX_ONLY |
| `narrative/cryo_seed_ampoule_logs.json` | CODEX_ONLY |
| `narrative/vault_seal_breach_logs.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 3 · catalogs 13 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PRESERVATION-TRUTH-118
wave: 10
status: PROPOSED — foreman claim required
packages: PRT-118A, PRT-118B, PRT-118C, PRT-118D, PRT-118E
claim paths:
  - src/Host/CryoVaultSaveStore.cs  # §19 candidate host surface
  - src/Host/FoodPreservationSaveStore.cs  # §19 candidate host surface
  - src/UI/VaultDoorBreachingPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/cryo_cultivars.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_preservation.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
