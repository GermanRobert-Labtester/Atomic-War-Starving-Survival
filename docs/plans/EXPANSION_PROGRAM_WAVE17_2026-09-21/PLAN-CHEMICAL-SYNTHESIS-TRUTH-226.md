# PLAN-CHEMICAL-SYNTHESIS-TRUTH-226 — Bench Synthesis: Reagents, Purity & Handling

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CHLOR-ALKALI-TRUTH-199, PLAN-PHARMACEUTICAL-TRUTH-167, PLAN-METROLOGY-TRUTH-172.
**Non-goals:** no bulk industrial chemistry (Plan 199), no pharma dosing
(Plan 167), no standards set (Plan 172).

## 1. Outcome
`Crafting/ChemicalSynthesisSystem.cs` (**321 lines**) is reachable and
unaddressed: bench-scale reagent preparation. Bulk chemistry (Plan 199) and
pharma (Plan 167) exist; the **bench layer** that turns bulk chemicals into
reagents for other crafts is unowned, so reagent quality is either flat or
invisible.

| Deliverable | Detail |
|---|---|
| Recipe model | reagent recipes with inputs (bulk chemicals, water, power) and outputs; conservation via Plan 93 |
| Purity/quality | purity from input purity, bench capability (Plan 172), and process care; maps to Plan 112 tiers |
| Handling | documented handling class per reagent; a mishap routes to Plan 183's hazard state |
| Consumer table | each reagent names its consumers (167 pharma, 184 ballistics, 187 pyrolysis) — no orphan reagent |
| Save truth | in-progress batches and purity restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Crafting/ChemicalSynthesisSystem.cs` (321 lines; unaddressed — Wave 17 audit).
- Plan 199 supplies bulk feedstock; Plan 172 capability; Plan 112 grades.
- Plan 183 owns hazard state mishaps feed.
- Plan 167/184/187 are consumer candidates to verify.

## 3. Packages
- **CST-226A** recipe model + input table.
- **CST-226B** purity derivation + tier mapping tests.
- **CST-226C** handling class + mishap route fixture.
- **CST-226D** consumer table + per-consumer routing test.
- **CST-226E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Purity follows its declared inputs; tiers match Plan 112.
- Mishaps raise hazard state; consumers accept only declared reagents.
- Save/load preserves batches and purity.
- `bash scripts/run_test.sh` on the crafting test region.

## 5. Risks
Reagent alchemy → purity chain and balance tests.
Orphan reagents → consumer table closed by review.

---

## 6. Expanded census (6 files · 2,119 lines)

Scope: `Assets/Ashfall.Core/Crafting/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ChemicalSynthesisCatalog.cs` | 199 | Catalog | — | 0 | 0 | 0 |
| `ChemicalSynthesisSystem.cs` | 321 | System | **yes** | 0 | 0 | 2 |
| `ChemicalReagentSynthesisEngine.cs` | 276 | System | — | 0 | 0 | 0 |
| `ChlorAlkaliSynthesisEngine.cs` | 400 | System | — | 0 | 0 | 2 |
| `CvdDiamondSynthesisEngine.cs` | 527 | System | — | 0 | 0 | 2 |
| `FischerTropschSynthesisEngine.cs` | 396 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | object[2 keys] |
| `mineral_acid_synthesis_catalog.json` | object[2 keys] |

**State surfaces:** `ChemicalSynthesisSystem.cs`, `ChlorAlkaliSynthesisEngine.cs`, `CvdDiamondSynthesisEngine.cs`, `FischerTropschSynthesisEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crafting/` |
| Test references | 8 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-TRIO-FAMILY-TRUTH-280` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-FISCHER-TROPSCH-TRUTH-202` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CST-226A` | no name match — resolve at claim time |
| `CST-226B` | no name match — resolve at claim time |
| `CST-226C` | no name match — resolve at claim time |
| `CST-226D` | no name match — resolve at claim time |
| `CST-226E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **3**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ChemicalReagentSynthesisEngine` | `ChlorAlkaliSynthesisEngine` |
| `ChemicalReagentSynthesisEngine` | `FischerTropschSynthesisEngine` |
| `ChemicalSynthesisSystem` | `ChemicalSynthesisCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `ChemicalSynthesisCatalog` | 1 |
| `ChlorAlkaliSynthesisEngine` | 1 |
| `FischerTropschSynthesisEngine` | 1 |
| `ChemicalReagentSynthesisEngine` | 0 |
| `ChemicalSynthesisSystem` | 0 |
| `CvdDiamondSynthesisEngine` | 0 |

**Class split:** hub 0 · sink 3 · source 2 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **9** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Host/ChemicalSynthesisHostSession.cs`, `src/Host/ChlorAlkaliHostSession.cs`, `src/Host/CvdDiamondHostSession.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.Plans122to125.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/FlagshipIntegrationIxSmokeTests.cs`, `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Save/Plans122to125PersistenceTests.cs`, `Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs`, `Ashfall.Core.Tests/Shelter/FischerTropschSynthesisEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |
| `cvd_diamond` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--chemical-dependency-save-selftest` |
| `--cvd-diamond-selftest` |

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

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/cvd_diamond_catalog.json` |
| `Assets/StreamingAssets/Data/fischer_tropsch_catalog.json` |
| `Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/toxic_chemical_catalog.json` |

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

Host files (`src/`) whose names share a domain token: **16**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/ChlorAlkaliHostSession.cs` |
| `src/Host/ChlorAlkaliSaveStore.cs` |
| `src/Host/CvdDiamondHostSession.cs` |
| `src/Host/CvdDiamondSaveStore.cs` |
| `src/Main.ChemicalSynthesis.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `cvd_diamond` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `cvd_diamond` |
| `mineral_chemical` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(GAMEPLAY_CONSUMED 3, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `mineral_acid_synthesis_catalog.json` | UNRESOLVED |
| `toxic_chemical_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 5 (laddered 0) · RNG streams 2 · host files 14 · catalogs 14 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CHEMICAL-SYNTHESIS-TRUTH-226
wave: 17
status: PROPOSED — foreman claim required
packages: CST-226A, CST-226B, CST-226C, CST-226D, CST-226E
claim paths:
  - src/Host/ChemicalDependencyHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveSelfTest.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveStore.cs  # §19 candidate host surface
  - src/Host/ChemicalReconHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --chemical-dependency-save-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
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
