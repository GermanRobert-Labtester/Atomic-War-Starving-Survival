# PLAN-FOUNDRY-FAMILY-TRUTH-278 — Silent Foundry Partials & Catalogs

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-TREATY-CONSEQUENCES-TRUTH-151.
**Non-goals:** no foundry redesign; the family is audited for partial
completeness and policy wiring.

## 1. Outcome
**9 `Foundry/` files** are referenced by no plan: the `SilentFoundrySystem.*`
partials (Glassworks, Heat, Material, Metallurgy, TreatyLabor), catalogs,
`SilentFoundryConsequencePolicy`, and a headless demo. Partial-class families
hide gaps: a stage can be declared and never implemented.

| Deliverable | Detail |
|---|---|
| Partial completeness | every `SilentFoundrySystem.*` partial lists its public surface; unimplemented stages reported |
| Consequence policy | policy outcomes route through named owners (Plans 45/151) |
| Heat/material coupling | stages reading power/materials use their owners |
| Catalog wiring | catalogs resolve to their stage |
| Demo truth | demo resolves to a verb |

## 2. Evidence
- 9 `Foundry/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 45 owns the industry family; Plan 151 treaty labor terms.
- Plan 1 Appendix A includes dead foundry-type authorities — cross-linked.

## 3. Packages
- **FDF-278A** partial surface inventory + gap report.
- **FDF-278B** consequence policy routing tests.
- **FDF-278C** coupling audit (power/materials).
- **FDF-278D** catalog wiring tests.
- **FDF-278E** demo→verb check.

## 4. Acceptance & verification
- Every declared stage has an implementation or a gap finding; policies route to owners.
- `bash scripts/run_test.sh` on the foundry region.

## 5. Risks
Partial-family gaps → inventory is exhaustive by file list.
Policy leakage → routing tests.

---

## 6. Expanded census (17 family files · 4,849 lines)

Scope: files under `Assets/Ashfall.Core/Foundry/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: Support 7 · Catalog 4 · System 4 · Demo 1 · DTO/Type 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `FoundryActionSurface.cs` | 110 | Support | 0 | 0 | 0 |
| `GlassworksCatalog.cs` | 166 | Catalog | 0 | 0 | 0 |
| `HydraulicExtrusionEngine.cs` | 377 | System | 0 | 0 | 2 |
| `MaterialProfileCatalog.cs` | 239 | Catalog | 0 | 0 | 0 |
| `MetallurgyHeavyCatalog.cs` | 177 | Catalog | 0 | 0 | 0 |
| `PowderMetallurgySystem.cs` | 330 | System | 0 | 0 | 2 |
| `SaltMineExtractionSystem.cs` | 455 | System | 0 | 0 | 2 |
| `SilentFoundryCatalog.cs` | 304 | Catalog | 0 | 0 | 0 |
| `SilentFoundryConsequencePolicy.cs` | 227 | Support | 0 | 0 | 0 |
| `SilentFoundryHeadlessDemo.cs` | 188 | Demo | 0 | 0 | 2 |
| `SilentFoundrySystem.Glassworks.cs` | 20 | Support | 0 | 0 | 0 |
| `SilentFoundrySystem.Heat.cs` | 549 | Support | 0 | 0 | 0 |
| `SilentFoundrySystem.Material.cs` | 333 | Support | 0 | 0 | 0 |
| `SilentFoundrySystem.Metallurgy.cs` | 222 | Support | 0 | 0 | 0 |
| `SilentFoundrySystem.TreatyLabor.cs` | 179 | Support | 0 | 0 | 4 |
| `SilentFoundrySystem.cs` | 690 | System | 1 | 0 | 0 |
| `SilentFoundryTypes.cs` | 283 | DTO/Type | 0 | 0 | 0 |

**Census totals:** 1 banned nondeterministic references · 0 empty-catch sites · 5 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `foundry_faction.json` | object[9 keys] |
| `foundry_items.json` | array[30] |
| `cupola_foundry_catalog.json` | object[4 keys] |
| `foundry_accords.json` | object[3 keys] |
| `foundry_production.json` | object[3 keys] |
| `foundry_treaty_consequences.json` | object[3 keys] |

**State surfaces (capture/restore present):**

- `HydraulicExtrusionEngine.cs`
- `PowderMetallurgySystem.cs`
- `SaltMineExtractionSystem.cs`
- `SilentFoundryHeadlessDemo.cs`
- `SilentFoundrySystem.TreatyLabor.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Foundry/` |
| Family files referenced by tests | 45 name references across the test tree |
| Determinism scan | 1 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(12 files). Other plans referencing those names: **7**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-INDUSTRY-AUTOMATION-45` | 7 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-DEV-TOOLING-TRUTH-75` | 1 |
| `PLAN-DEEP-STRATA-83` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FDF-278A` | `FoundryActionSurface.cs` |
| `FDF-278B` | `SilentFoundryConsequencePolicy.cs` |
| `FDF-278C` | no name match — resolve at claim time |
| `FDF-278D` | `GlassworksCatalog.cs`, `MaterialProfileCatalog.cs`, `MetallurgyHeavyCatalog.cs` |
| `FDF-278E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 12. Host files: **10** · Test files: **21** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/ExpansionHostSession.cs`, `src/Host/HostCli.Plans139_141.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/HydraulicExtrusionHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 21 | `Ashfall.Core.Tests/ApicultureAndSaltExpansionTests.cs`, `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/ContentDeepChainGateTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs`, `Ashfall.Core.Tests/ExpansionHubSaveV5Tests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/shelter_machine_identities.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `foundry` |
| `hydraulic_extrusion` |
| `mine_clearing_flail` |
| `powder_metallurgy` |
| `silent_foundry` |
| `technical_material_archive` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--hydraulic-extrusion-selftest` |
| `--ice-road-tick-demo` |
| `--mine-flail-selftest` |
| `--mine-flail-uitest` |
| `--salt-steam-selftest` |
| `--silent-foundry-selftest` |
| `--silent-foundry-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnHeavyMetalExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnMineClosed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnMineOpened` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |
| `Assets/StreamingAssets/Data/glassworks_recipes.json` |
| `Assets/StreamingAssets/Data/hydraulic_extrusion_catalog.json` |
| `Assets/StreamingAssets/Data/metallurgy_recipes.json` |
| `Assets/StreamingAssets/Data/mine_flail_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/salt_mine_inscriptions.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (9 files, 93 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Foundry` | 8 | 73 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 93 cases sit under matching regions — run those first (`Foundry`, `NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **21**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/HydraulicExtrusionHostSession.cs` |
| `src/Host/HydraulicExtrusionSaveStore.cs` |
| `src/Host/MineClearingFlailHostSession.cs` |
| `src/Host/MineClearingFlailSaveStore.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/SilentFoundrySaveStore.cs` |
| `src/Host/TechnicalMaterialArchiveSaveStore.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.HydraulicExtrusion.cs` |
| `src/Main.UiTests.SilentFoundry.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **6**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `foundry` | no |
| `hydraulic_extrusion` | no |
| `mine_clearing_flail` | no |
| `powder_metallurgy` | no |
| `silent_foundry` | no |
| `technical_material_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `cupola_foundry` |
| `foundry` |
| `hydraulic_extrusion` |
| `route_engineering_mine_flail` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 5, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `foundry_accords.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |
| `foundry_items.json` | GAMEPLAY_CONSUMED |
| `foundry_production.json` | GAMEPLAY_CONSUMED |
| `foundry_treaty_consequences.json` | GAMEPLAY_CONSUMED |
| `narrative/salt_mine_inscriptions.json` | CODEX_ONLY |
| `powder_metallurgy_catalog.json` | UNRESOLVED |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 6 (laddered 0) · RNG streams 4 · host files 16 · catalogs 20 · test regions 2 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FOUNDRY-FAMILY-TRUTH-278
wave: 19
status: PROPOSED — foreman claim required
packages: FDF-278A, FDF-278B, FDF-278C, FDF-278D, FDF-278E
claim paths:
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/HydraulicExtrusionHostSession.cs  # §19 candidate host surface
  - src/Host/HydraulicExtrusionSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/cupola_foundry_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/food_types.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/
  - godot --headless --path . -- --hydraulic-extrusion-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
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
