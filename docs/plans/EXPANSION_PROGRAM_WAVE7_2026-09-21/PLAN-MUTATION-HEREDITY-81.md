# PLAN-MUTATION-HEREDITY-81 — Dose Effects, Screening & Generational Change

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05, PLAN-FAMILY-DYNASTY-43,
PLAN-PANDEMIC-PUBLIC-HEALTH-47.
**Implementation scaffold:** [`PLAN-MUTATION-HEREDITY-81_APPENDIX-A_SCAFFOLD.md`](PLAN-MUTATION-HEREDITY-81_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PANDEMIC-PUBLIC-HEALTH-47` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real genetics/biotech claims; no superpowers; respectful,
fictional, restrained handling of congenital conditions — reviewed by a human.

## Outcome
Radiation and medicine are deep (`RadiationSystem`, `DoseLedgerSystem`,
`DoseContentCatalog`, `DoseRegistersCatalog`, acute/chronic states, shielding,
`chronic_condition` systems, `AntenatalMaternalHealthEngine`, `AgingSystem`),
but dose consequences stop at the individual survivor. This plan adds a
**fictional heredity layer**: chronic effects, screening, and long-horizon
family outcomes — careful, bounded, and never punitive-by-surprise.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Chronic dose effects | `DoseLedgerSystem`, chronic conditions | monitor, treat | condition management over years |
| Screening | medical + records | test, counsel | knowledge, decisions |
| Maternal/child | `AntenatalMaternalHealthEngine` | care, plan | pregnancy risk, support |
| Heredity (fictional) | family/body state | track traits | trait expression, not "mutation points" |
| Exposure prevention | shielding, gear, shelter | protect | reduced risk (Plan 20B curves) |
| Records | medical record log, archive | document | continuity, consent |
| Care | clinic continuum, therapy | support | dignity, quality of life |
| Generations | dynasty | raise children | family stories, legacy traits |

## Evidence
- Core: `RadiationSystem`, `DoseLedgerSystem` (+ save), `DoseRegistersCatalog`, `Amputation/Bionics`, `Medical/ChronicConditionSystem` (orphan), `AntenatalMaternalHealthEngine` (orphan), `Survivors/AgingSystem`, `SurvivorAgingProgressionEngine` (orphans).
- Sealed prior: `DEC-07` radiation baselines, Plan 20B shielding sweeps (12/12), Plan 60 medicine legible, Plan 41/42 body state.
- Contracts: dose vs illness separation (`severitySource`); no real dose numbers claimed; baseline curves are the authority.

## Packages
- **MH-81A** chronic management: dose-driven conditions that progress slowly and respond to care; visible timeline.
- **MH-81B** screening/counselling: options with knowledge, not reveals; player consent in-world.
- **MH-81C** maternal/child care: support paths, complications handled with restraint and existing medical owners.
- **MH-81D** fictional heredity: a small trait table expressed through existing survivor traits; no new stat spam.
- **MH-81E** prevention: shielding/gear/shelter advice surfaced from existing curves.
- **MH-81F** records/consent: medical record log (Plan 198) + archive; privacy stated.
- **MH-81G** content volumes: +8 conditions, +6 screening steps, +8 care events; abstract/fictional, human-reviewed.

## Acceptance & verification
- No random congenital punishment; all effects explainable and treatable; determinism.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`; dose suites; `Plan20BShieldingBalanceSweepTests`.

## Risks
Sensitive content → human review gate before merge; tone rules enforced (no real-world conditions, no blame framing).

---

## 6. Expanded census (5 files · 1,599 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MutationSystem.cs` | 352 | System | **yes** | 0 | 0 | 2 |
| `RespiratoryDegenerationSystem.cs` | 287 | System | — | 0 | 0 | 2 |
| `AntenatalMaternalHealthEngine.cs` | 321 | System | **yes** | 0 | 0 | 0 |
| `GenealogyBridge.cs` | 183 | Support | — | 0 | 0 | 0 |
| `GenerationalSystem.cs` | 456 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `mutations.json` | object[2 keys] |
| `mutated_botanical_logs.json` | array[8] |

**State surfaces:** `MutationSystem.cs`, `RespiratoryDegenerationSystem.cs`, `GenerationalSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
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

Domain files: 5. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 3 |
| `PLAN-FAMILY-DYNASTY-43` | 2 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-RESPIRATORY-DEGENERATION-TRUTH-233` | 1 |
| `PLAN-SUCCESSION-LEGACY-TRUTH-252` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MH-81A` | no name match — resolve at claim time |
| `MH-81B` | no name match — resolve at claim time |
| `MH-81C` | `AntenatalMaternalHealthEngine.cs` |
| `MH-81D` | no name match — resolve at claim time |
| `MH-81E` | no name match — resolve at claim time |
| `MH-81F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **11** · Test files: **16** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `src/Host/Phase0HostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 16 | `Ashfall.Core.Tests/CraftingAfflictionLoopTests.cs`, `Ashfall.Core.Tests/Host/Phase0EffectsBridgeTests.cs`, `Ashfall.Core.Tests/Integration/Plans178_181_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Medical/MedicalPipelineArchitectureGateTests.cs`, `Ashfall.Core.Tests/Medical/MedicalPipelinePhase2Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **0**; isolated: **5**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `mental_health_crisis` |
| `mutation_tree` |
| `survivor_mental_health` |

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

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnHealthDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMentalHealthChanged` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnMutationDetermined` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnRespiratoryDegradationIncreased` | `Assets/Ashfall.Core/Medical/RespiratoryDegenerationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **0**.

| Catalog |
|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog filename shares a token with this domain — the authority is likely code-defined or its data lives in a broader catalog. Not a conclusion; check the owning loader.

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

Host files (`src/`) whose names share a domain token: **6**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/GenerationalSaveStore.cs` |
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Host/MutationSaveStore.cs` |
| `src/Host/SurvivorMentalHealthSaveStore.cs` |
| `src/UI/MentalHealthCrisisPanel.cs` |
| `src/UI/MutationTreePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `mental_health_crisis` | no |
| `mutation_tree` | no |
| `survivor_mental_health` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `agriculture_mutation` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

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
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 7 · catalogs 0 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MUTATION-HEREDITY-81
wave: 7
status: PROPOSED — foreman claim required
packages: MH-81A, MH-81B, MH-81C, MH-81D, MH-81E, MH-81F, MH-81G
claim paths:
  - src/Host/GenerationalSaveStore.cs  # §19 candidate host surface
  - src/Host/MentalHealthCrisisHostSession.cs  # §19 candidate host surface
  - src/Host/MutationSaveStore.cs  # §19 candidate host surface
  - src/Host/SurvivorMentalHealthSaveStore.cs  # §19 candidate host surface
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
