# PLAN-INDUSTRY-AUTOMATION-45 — Production Lines, Quality, Shifts & Exports

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05 (shelter engine),
PLAN-DATA-CONSUMER-22.
**Expanded appendix:** [`PLAN-INDUSTRY-AUTOMATION-45_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-INDUSTRY-AUTOMATION-45_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's industry & automation
systems (7 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no real industrial process data, no real weapons/ammunition
recipes, no second production or inventory authority.

---

## 1. Outcome

The Silent Foundry is the game's industrial heart (Plan 213 sealed: profiles,
purity ladder, forging sequence, provenance; 35 products across 5 lines per
Plan 129), and the shelter has a half-wired engine room: `CupolaFoundryEngine`,
`KilnFiringEngine`, `ChemicalReagentSynthesisEngine`,
`MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`,
`HydraulicExtrusionEngine`, `SaltMineExtractionSystem`,
`PrecisionGlassworksOpticsEngine`, plus `FoundryActionSurface`,
`GlassworksCatalog`, `MaterialProfileCatalog`, `MetallurgyHeavyCatalog`,
`GrainProcessingSystem` and textile systems.

This plan makes industry **schedulable, skilful and contractual**:
**queue work → staff shifts → control quality → honour treaties → expand
lines → survive breakdowns**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Queues & scheduling | `SilentFoundrySystem`, `FoundryActionSurface` | queue jobs, set line | throughput, input draw |
| Quality | purity ladder + forging sequence | inspect, rework | quality bands, reject rate |
| Shifts & labour | duty roster, driveline, load shedding | staff, prioritize power | output, fatigue, accidents |
| Treaties | `foundry_accords.json`, consequence policies | accept/miss obligations | quotas, standing, market effects |
| Specialties | glassworks, salt mine, extrusion, textiles | run a specialty | optics, salt, shapes, cloth |
| Maintenance | machinery condition, `ShelterMaintenanceSystem` | service, replace | breakdown risk, efficiency |
| Provenance | material records | trace a batch | equipment/vehicle quality handoff |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `Foundry/` (14+ files: `SilentFoundrySystem`, `FoundryActionSurface`, `MaterialProfileCatalog`, `MetallurgyHeavyCatalog`, `GlassworksCatalog`, `HydraulicExtrusionEngine`, `SaltMineExtractionSystem`), `Shelter/CupolaFoundryEngine.cs`, `KilnFiringEngine`, `ChemicalReagentSynthesisEngine`, `MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`, `Optics/PrecisionGlassworksOpticsEngine.cs`, `Textiles/GarmentLayeringThermalEngine.cs` |
| Data | `foundry_production.json`, `foundry_accords.json`, `foundry_treaty_consequences.json`, `metallurgy_recipes.json`, `alloys_and_ores.json`, `glassworks_recipes.json`, `workshop_recipes.json` |
| Sealed prior | Plan 213 metallurgy (11/11 + restore bug sealed), Plan 129 products (12/12 + 5/5), Plan 103 treaty consequences (14/14), Plan 102 accords (11/11) |
| Guarantees | provenance additive on production records; no recalculation on old saves; ZERO RNG forging |

---

## 3. Packages

### IN-45A — Queues and scheduling
- A production queue with per-line slots, input reservation at queue time, and
  cancellation refunds; `FoundryActionSurface` becomes the single command
  surface.
- **Acceptance:** atomic input consumption; no phantom reservations on reload;
  queue visible per line.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/`.

### IN-45B — Quality control and rework
- Purity/contamination/slag bands produce quality outcomes; inspection reveals
  faults; rework consumes time/material but improves a band; rejects can be
  scrapped or sold as lower grade.
- **Acceptance:** quality math explainable; no quality inflation; a failed
  inspect is informative, not punishing.
- **Verify:** Plan 213 suite extends.

### IN-45C — Shifts, power and automation
- Shift staffing from the duty roster with fatigue; driveline wear reduces
  efficiency; load shedding allocates power to lines by player priority
  (Power Board from PLAN-SHELTER-ARCHITECTURE-40).
- **Acceptance:** output = f(staff, power, condition) and visible; no
  unattended infinite production; brownouts are explainable.
- **Verify:** duty/power focused suites.

### IN-45D — Treaties, quotas and exports
- Foundry accords become enforceable obligations (quotas, delivery windows,
  price bands) with met/missed/violated consequences already authored; exports
  use the canonical economy.
- **Acceptance:** obligations visible before the deadline; breach consequences
  bounded; no parallel contract store.
- **Verify:** Plan 102/103 suites extend.

### IN-45E — Specialty lines
- Glassworks (optics/lenses), salt mine (preservation input), hydraulic
  extrusion (shapes/pipes), textiles (warmth/clothing) each as a small line
  with facility and skill gates.
- **Acceptance:** every output resolves to items with consumers; no orphan
  output; facility gating tested.
- **Verify:** optics/textiles focused suites.

### IN-45F — Machinery health and accidents
- Condition wear per machine; maintenance tasks; breakdown events with
  material loss and injury risk routed to medical; no instant full repair.
- **Acceptance:** wear visible; breakdowns recoverable; injury path canonical.
- **Verify:** maintenance + medical focused suites.

### IN-45G — Content volumes
- +12 products, +6 processes, +8 accords clauses, +6 machine rows, +6 specialty
  recipes; abstract/industrial, fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Idle-game production | inputs, staffing, power and demand constrain throughput |
| Quality systems become opaque | one inspect readout; bands named not numeric only |
| Treaty obligations snowball | at most N active accords; renegotiation path |
| Duplicate production authorities | extend `SilentFoundrySystem`; specialties hook it, never fork |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/
godot --headless --path . -- --foundry-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
```

---

## 6. Expanded census (10 files · 3,718 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Loader 1 · System 8

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ChemicalReagentSynthesisEngine.cs` | 276 | System | **yes** | 0 | 0 | 0 |
| `ChlorAlkaliSynthesisEngine.cs` | 400 | System | — | 0 | 0 | 2 |
| `CupolaFoundryCatalog.cs` | 167 | Catalog | — | 0 | 0 | 0 |
| `CupolaFoundryEngine.cs` | 445 | System | **yes** | 0 | 0 | 2 |
| `CvdDiamondSynthesisEngine.cs` | 527 | System | — | 0 | 0 | 2 |
| `EbPvdCoatingCatalogLoader.cs` | 176 | Loader | — | 0 | 0 | 0 |
| `EbPvdCoatingEngine.cs` | 537 | System | — | 0 | 0 | 2 |
| `FischerTropschSynthesisEngine.cs` | 396 | System | — | 0 | 0 | 2 |
| `KilnFiringEngine.cs` | 241 | System | **yes** | 0 | 0 | 0 |
| `PlasticPyrolysisSystem.cs` | 553 | System | — | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `foundry_faction.json` | object[9 keys] |
| `foundry_items.json` | array[30] |
| `chlor_alkali_synthesis_catalog.json` | object[2 keys] |
| `cupola_foundry_catalog.json` | object[4 keys] |
| `ebpvd_coating_catalog.json` | object[6 keys] |
| `plastic_pyrolysis_catalog.json` | object[8 keys] |

**State surfaces:** `ChlorAlkaliSynthesisEngine.cs`, `CupolaFoundryEngine.cs`, `CvdDiamondSynthesisEngine.cs`, `EbPvdCoatingEngine.cs`, `FischerTropschSynthesisEngine.cs`, `PlasticPyrolysisSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 13 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 10 domain files: **11 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `PlasticPyrolysisSystem.cs` | 553 | 0 | 1 |
| `EbPvdCoatingEngine.cs` | 537 | 2 | 0 |
| `CvdDiamondSynthesisEngine.cs` | 527 | 3 | 0 |
| `CupolaFoundryEngine.cs` | 445 | 1 | 3 |
| `ChlorAlkaliSynthesisEngine.cs` | 400 | 1 | 1 |
| `FischerTropschSynthesisEngine.cs` | 396 | 1 | 1 |
| `ChemicalReagentSynthesisEngine.cs` | 276 | 0 | 2 |
| `KilnFiringEngine.cs` | 241 | 0 | 1 |
| `EbPvdCoatingCatalogLoader.cs` | 176 | 0 | 2 |
| `CupolaFoundryCatalog.cs` | 167 | 3 | 0 |

**Highest-coupling files (in×2 + out):**

- `CupolaFoundryCatalog.cs` — in 3, out 0
- `CvdDiamondSynthesisEngine.cs` — in 3, out 0
- `CupolaFoundryEngine.cs` — in 1, out 3
- `EbPvdCoatingEngine.cs` — in 2, out 0
- `ChlorAlkaliSynthesisEngine.cs` — in 1, out 1
- `FischerTropschSynthesisEngine.cs` — in 1, out 1
- `ChemicalReagentSynthesisEngine.cs` — in 0, out 2
- `EbPvdCoatingCatalogLoader.cs` — in 0, out 2

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 10. Other plans referencing their names: **11**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FISCHER-TROPSCH-TRUTH-202` | 4 |
| `PLAN-CHEMICAL-SYNTHESIS-TRUTH-226` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-COATING-TECH-TRUTH-188` | 3 |
| `PLAN-ENERGY-NUCLEAR-48` | 2 |
| `PLAN-CRAFT-QUALITY-TRUTH-112` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `IN-45A` | no name match — resolve at claim time |
| `IN-45B` | no name match — resolve at claim time |
| `IN-45C` | no name match — resolve at claim time |
| `IN-45D` | no name match — resolve at claim time |
| `IN-45E` | no name match — resolve at claim time |
| `IN-45F` | no name match — resolve at claim time |
| `IN-45G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 10. Host files: **11** · Test files: **11** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Host/ChlorAlkaliHostSession.cs`, `src/Host/CvdDiamondHostSession.cs`, `src/Host/EbPvdCoatingHostSession.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.Plans122to125.cs` |
| Tests (`Ashfall.Core.Tests/`) | 11 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs`, `Ashfall.Core.Tests/Integration/Plans202To205CampaignIntegrationTests.cs`, `Ashfall.Core.Tests/Save/Plans122to125PersistenceTests.cs`, `Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **10** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |
| `cvd_diamond` |
| `ebpvd_coating` |
| `foundry` |
| `industry` |
| `plastic_pyrolysis` |
| `silent_foundry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--chemical-dependency-save-selftest` |
| `--cvd-diamond-selftest` |
| `--ebpvd-coating-selftest` |
| `--ebpvd-coating-uitest` |
| `--silent-foundry-selftest` |
| `--silent-foundry-uitest` |

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
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/cvd_diamond_catalog.json` |
| `Assets/StreamingAssets/Data/ebpvd_coating_catalog.json` |
| `Assets/StreamingAssets/Data/fischer_tropsch_catalog.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (8 files, 73 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Foundry` | 8 | 73 |

**Verdict:** 73 cases sit under matching regions — run those first (`Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **30**
(9 of them panels/HUD).

| Host file |
|---|
| `src/Foundry/SilentFoundryHostSession.cs` |
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

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **10**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `cvd_diamond` | no |
| `ebpvd_coating` | no |
| `foundry` | no |
| `industry` | no |
| `plastic_pyrolysis` | no |
| `silent_foundry` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **5**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `cupola_foundry` |
| `cvd_diamond` |
| `foundry` |
| `mineral_chemical` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **19**
(CODEX_ONLY 7, GAMEPLAY_CONSUMED 8, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `foundry_accords.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |
| `foundry_items.json` | GAMEPLAY_CONSUMED |
| `foundry_production.json` | GAMEPLAY_CONSUMED |
| `foundry_treaty_consequences.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 11
**Surface:** save sections 10 (laddered 0) · RNG streams 5 · host files 17 · catalogs 22 · test regions 1 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INDUSTRY-AUTOMATION-45
wave: —
status: PROPOSED — foreman claim required
packages: IN-45A, IN-45B, IN-45C, IN-45D, IN-45E, IN-45F, IN-45G
claim paths:
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencyHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveSelfTest.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/
  - godot --headless --path . -- --chemical-dependency-save-selftest
dependencies:
  - coordinate: 11 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
