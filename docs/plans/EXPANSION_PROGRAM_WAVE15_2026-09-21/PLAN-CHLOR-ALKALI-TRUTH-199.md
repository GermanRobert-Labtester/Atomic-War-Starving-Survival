# PLAN-CHLOR-ALKALI-TRUTH-199 — Basic Chemicals: Feedstock, Yields & Containment

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CHEMICAL-RECON-TRUTH-183, PLAN-PHARMACEUTICAL-TRUTH-167, PLAN-FLUID-LOGISTICS-TRUTH-179.
**Implementation scaffold:** [`PLAN-CHLOR-ALKALI-TRUTH-199_APPENDIX-A_SCAFFOLD.md`](PLAN-CHLOR-ALKALI-TRUTH-199_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PLASTIC-PYROLYSIS-TRUTH-187` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no industry family (Plan 45), no hazard sampling (Plan 183), no
pharma dosing (Plan 167).

## 1. Outcome
`Shelter/ChlorAlkaliSynthesisEngine.cs` (**400 lines**) is reachable and
unaddressed: the process that yields basic industrial chemicals (treatment,
sanitation, and reagent inputs). It sits upstream of water treatment, pharma,
and preservation, with hazardous intermediates — and nothing states yields,
containment, or who consumes its output.

| Deliverable | Detail |
|---|---|
| Process model | inputs (salt, water, power) and outputs (documented chemical classes) with a yield table; conservation via Plan 93 |
| Containment | handling requires documented equipment/ventilation; a failure emits hazard state through Plan 183 and is visible |
| Output consumption | each output names its consumers (water treatment Plan 46, pharma Plan 167, preservation Plan 118) |
| Power coupling | the process draws from Plan 48's grid; a brownout lowers output, never silently |
| Save truth | in-progress batches and containment state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs` (400 lines; unaddressed — Wave 13/15 audit).
- Plan 183 owns the hazard state an emission feeds.
- Plan 179 supplies water/fluid inputs; Plan 48 power.
- Plan 46/167/118 are consumers named per output class.

## 3. Packages
- **CAT-199A** process/yield table + conservation test.
- **CAT-199B** containment requirement + emission fixture.
- **CAT-199C** consumer table + per-consumer routing tests.
- **CAT-199D** power coupling test (brownout → lower output).
- **CAT-199E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Yields balance within tolerance; a missing containment raises hazard state.
- Outputs appear at their named consumers; no orphan output class.
- Save/load preserves batches and containment.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Hazmat invisible → containment state and emissions are surfaced.
Orphan outputs → the consumer table is closed; an unconsumed class fails review.

---

## 6. Expanded census (1 files · 400 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ChlorAlkaliSynthesisEngine.cs` | 400 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | object[2 keys] |
| `calcium_hypochlorite_titration_reports.json` | array[7] |

**State surfaces:** `ChlorAlkaliSynthesisEngine.cs`.

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

Domain files: 1. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-INDUSTRY-AUTOMATION-45` | 1 |
| `PLAN-FISCHER-TROPSCH-TRUTH-202` | 1 |
| `PLAN-CHEMICAL-SYNTHESIS-TRUTH-226` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CAT-199A` | no name match — resolve at claim time |
| `CAT-199B` | no name match — resolve at claim time |
| `CAT-199C` | no name match — resolve at claim time |
| `CAT-199D` | no name match — resolve at claim time |
| `CAT-199E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ChlorAlkaliHostSession.cs`, `src/Main.Plans110_113.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
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

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/calcium_hypochlorite_titration_reports.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (59 files, 358 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |

**Verdict:** 358 cases sit under matching regions — run those first (`Campaign`, `Education`, `Foundry`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **290**
(9 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/CaregivingHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **60**
(CODEX_ONLY 46, GAMEPLAY_CONSUMED 9, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `foundry_accords.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 23 (laddered 0) · RNG streams 12 · host files 23 · catalogs 18 · test regions 5 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CHLOR-ALKALI-TRUTH-199
wave: 15
status: PROPOSED — foreman claim required
packages: CAT-199A, CAT-199B, CAT-199C, CAT-199D, CAT-199E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
