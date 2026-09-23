# PLAN-CHEMICAL-RECON-TRUTH-183 — Hazard Plumes, Sampling & Protective Posture

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SIGNALS-REMOTE-SENSING-49, PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-ACUTE-TRAUMA-CARE-124.
**Implementation scaffold:** [`PLAN-CHEMICAL-RECON-TRUTH-183_APPENDIX-A_SCAFFOLD.md`](PLAN-CHEMICAL-RECON-TRUTH-183_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SIGNALS-REMOTE-SENSING-49` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no weather model (Plan 28), no signal systems (Plan 49), no
medical care (Plan 124).

## 1. Outcome
`Expeditions/ChemicalReconEngine.cs` (**593 lines**) is reachable and
unaddressed, and `Combat/ChemicalPlumeDispersionEngine.cs` is host-unreachable
(Plan 1 Appendix A). Reconnaissance of chemical hazards needs a stated
contract: what a sample proves, how plume state is read, and what protection
buys — or expeditions walk into invisible danger.

| Deliverable | Detail |
|---|---|
| Hazard model | plume/source state with dispersion driven by Plan 28's weather; read-only to this system |
| Sampling | samples have documented quality (Plan 112 tiers) and prove hazard class + intensity band, not exact numbers |
| Protection posture | documented gear classes reduce exposure; exposure routes to Plan 124/47 owners per band |
| Detection vs delay | a reading has a turnaround; delayed warnings are visible, never silently perfect |
| Save truth | samples and known hazard state restore; a load never re-dispatches a recon |

## 2. Evidence
- `Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs` (593 lines; unaddressed — Wave 13 audit).
- Plan 1 Appendix A: `ChemicalPlumeDispersionEngine` (Combat/) is host-unreachable — a sibling seam this plan names.
- Plan 28 supplies dispersion input; Plan 49's sensing supplies long-range detection.
- Plan 124 and 47 are the exposure-consequence owners.

## 3. Packages
- **CRT-183A** hazard model + dispersion input contract.
- **CRT-183B** sampling quality/band tests.
- **CRT-183C** protection posture table + exposure routing tests.
- **CRT-183D** turnaround/delay path + visible-warning test.
- **CRT-183E** save round-trip; no re-dispatch on load.

## 4. Acceptance & verification
- A sample proves its band per quality tier; exact numbers are never exposed.
- Exposure appears in the named owners per band; protection shifts the band.
- Delayed warnings surface as such; no perfect instantaneous reading.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`.

## 5. Risks
Invisible danger → detection and posture are visible; delayed warnings are explicit.
Overlap with 49 → long-range sensing vs on-site sampling; the boundary is stated.

---

## 6. Expanded census (1 files · 593 lines)

Scope: `Assets/Ashfall.Core/Expeditions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ChemicalReconEngine.cs` | 593 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `ChemicalReconEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Expeditions/` |
| Test references | 3 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |
| `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CRT-183A` | no name match — resolve at claim time |
| `CRT-183B` | no name match — resolve at claim time |
| `CRT-183C` | no name match — resolve at claim time |
| `CRT-183D` | no name match — resolve at claim time |
| `CRT-183E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ChemicalReconHostSession.cs`, `src/Main.Plans78_81.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Combat/ChemicalPlumeDispersionEngineTests.cs`, `Ashfall.Core.Tests/Expeditions/ChemicalReconEngineTests.cs`, `Ashfall.Core.Tests/Expeditions/ChemicalReconSaveTests.cs`, `Ashfall.Core.Tests/Integration/PrecisionHazardInfrastructureTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `infrastructure` |
| `precision_metrology` |
| `precision_optics` |
| `recon_telemetry` |
| `route_infrastructure` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **9** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--chemical-dependency-save-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--precision-metrology-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/fluid_infrastructure.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/precision_broaching_catalog.json` |
| `Assets/StreamingAssets/Data/precision_optics_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (149 files, 1123 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Education` | 2 | 11 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Optics` | 1 | 5 |
| `PlayerCommand` | 1 | 1 |
| `Shelter` | 87 | 754 |

**Verdict:** 1123 cases sit under matching regions — run those first (`Campaign`, `Education`, `Excavation`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **304**
(14 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **31**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `bio_fermentation` | no |
| `black_market` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **73**
(CODEX_ONLY 46, GAMEPLAY_CONSUMED 20, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_preserved_archive` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 31 (laddered 0) · RNG streams 13 · host files 24 · catalogs 22 · test regions 9 · flags 9

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CHEMICAL-RECON-TRUTH-183
wave: 14
status: PROPOSED — foreman claim required
packages: CRT-183A, CRT-183B, CRT-183C, CRT-183D, CRT-183E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
