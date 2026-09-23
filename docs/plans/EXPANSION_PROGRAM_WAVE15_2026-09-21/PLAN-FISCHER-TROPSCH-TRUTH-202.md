# PLAN-FISCHER-TROPSCH-TRUTH-202 — Synthetic Fuel: Feedstock, Conversion & Quality

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-PLASTIC-PYROLYSIS-TRUTH-187, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no industry family (Plan 45), no quality tiers (Plan 112), no
pyrolysis line (Plan 187).

## 1. Outcome
`Shelter/FischerTropschSynthesisEngine.cs` (**396 lines**) is reachable and
unaddressed: converting gas/char feedstock into liquid fuel. Pyrolysis (Plan
187) produces some feedstock; industry (Plan 45) provides stations — but the
**conversion contract** (yield, catalyst condition, fuel grade) is unstated, so
the process is either lossless fuel alchemy or inert.

| Deliverable | Detail |
|---|---|
| Conversion model | inputs (gas/char classes) → fuel grades with a yield table; mass balance within documented loss |
| Catalyst/condition | catalyst degrades per Plan 119's contract; a worn catalyst changes yield/grade, visibly |
| Fuel quality | output grade maps to Plan 112's tiers; low-grade fuel has documented engine effects (Plan 30/119) |
| Power/heat coupling | the process draws on the grid (Plan 48) and produces waste heat with a documented recovery path or loss |
| Save truth | batches and catalyst condition restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/FischerTropschSynthesisEngine.cs` (396 lines; unaddressed — Wave 13/15 audit).
- Plan 187 supplies one feedstock class; Plan 45 owns the industrial family.
- Plan 112 grades the output; Plan 48/119 receive engine effects.
- Plan 93 verifies input/output counts.

## 3. Packages
- **FTT-202A** conversion/yield table + balance test.
- **FTT-202B** catalyst degradation effect fixtures.
- **FTT-202C** grade mapping to Plan 112 + low-grade engine effect test.
- **FTT-202D** heat recovery/loss documentation + test.
- **FTT-202E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Balance closes; worn catalyst lowers yield per the table.
- Output grades match Plan 112; low-grade effects appear in the engine owners.
- Save/load preserves batches and catalyst state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Fuel alchemy → yields + catalyst condition + balance are the constraints.
Grade invisibility → grades are surfaced on the fuel item and in engine effects.

---

## 6. Expanded census (5 files · 1,796 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ChemicalReagentSynthesisEngine.cs` | 276 | System | — | 0 | 0 | 0 |
| `ChlorAlkaliSynthesisEngine.cs` | 400 | System | — | 0 | 0 | 2 |
| `CvdDiamondSynthesisEngine.cs` | 527 | System | — | 0 | 0 | 2 |
| `FischerTropschCatalog.cs` | 197 | Catalog | — | 0 | 0 | 0 |
| `FischerTropschSynthesisEngine.cs` | 396 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | object[2 keys] |
| `fischer_tropsch_catalog.json` | object[4 keys] |
| `mineral_acid_synthesis_catalog.json` | object[2 keys] |

**State surfaces:** `ChlorAlkaliSynthesisEngine.cs`, `CvdDiamondSynthesisEngine.cs`, `FischerTropschSynthesisEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 6 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **4**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 2 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-CHEMICAL-SYNTHESIS-TRUTH-226` | 1 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FTT-202A` | no name match — resolve at claim time |
| `FTT-202B` | no name match — resolve at claim time |
| `FTT-202C` | `FischerTropschSynthesisEngine.cs` |
| `FTT-202D` | no name match — resolve at claim time |
| `FTT-202E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **6** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/ChlorAlkaliHostSession.cs`, `src/Host/CvdDiamondHostSession.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.Plans122to125.cs`, `src/Main.Plans110_113.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Save/Plans122to125PersistenceTests.cs`, `Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs`, `Ashfall.Core.Tests/Shelter/FischerTropschSynthesisEngineTests.cs`, `Ashfall.Core.Tests/Shelter/Plan124CvdDiamondSynthesisEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `ChemicalReagentSynthesisEngine` | `ChlorAlkaliSynthesisEngine` |
| `ChemicalReagentSynthesisEngine` | `FischerTropschSynthesisEngine` |
| `FischerTropschSynthesisEngine` | `FischerTropschCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 5 (laddered 0) · RNG streams 2 · host files 14 · catalogs 14 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FISCHER-TROPSCH-TRUTH-202
wave: 15
status: PROPOSED — foreman claim required
packages: FTT-202A, FTT-202B, FTT-202C, FTT-202D, FTT-202E
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
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
