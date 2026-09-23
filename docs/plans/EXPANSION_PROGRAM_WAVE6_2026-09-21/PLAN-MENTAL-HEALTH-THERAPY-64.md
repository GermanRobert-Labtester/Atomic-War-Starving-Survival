# PLAN-MENTAL-HEALTH-THERAPY-64 — Therapy, Trauma Care, Crisis & Recovery Arcs

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RECREATION-MORALE-50, PLAN-ANOMALY-PHANTOM-63,
PLAN-FAMILY-DYNASTY-43.
**Implementation scaffold:** [`PLAN-MENTAL-HEALTH-THERAPY-64_APPENDIX-A_SCAFFOLD.md`](PLAN-MENTAL-HEALTH-THERAPY-64_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no clinical claims, no real diagnoses or medications, no
replacement for the existing crisis authority.

## Outcome
`MentalHealthCrisisSystem`, `Psychology/PsychologicalProfileSystem` (orphan),
`TraumaBondSystem` (orphan), `GuiltInsomnia`/`PhantomPain` sleep beats,
`SleepNarrativeProjection` (sealed), grief dispersion, and moral-choice scars
exist, but care is binary (crisis or not). This plan adds a **care continuum**:
stress → strain → crisis → treatment → recovery → resilience.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Stress model | mental-health owner | watch, talk, rest | strain level, tells |
| Counselling | therapist survivor/skill | schedule sessions | strain reduction, trust |
| Trauma care | trauma systems + sleep beats | address a memory | nightmares reduce, scars persist |
| Crisis response | `MentalHealthCrisisSystem` | intervene, sedate, restrain | safety, aftermath |
| Recovery arc | psychology profile | set a goal, celebrate | resilience, new outlook |
| Group care | social/faith/recreation | vigils, rituals, groups | bonds, shared coping |
| Carer strain | staff needs | rotate carers | carer burnout risk |

## Evidence
- Core: `MentalHealthCrisisSystem.cs`, `Psychology/PsychologicalProfileSystem.cs` (orphan), `TraumaBondSystem` (orphan), `SleepNarrativeProjection`, `SurvivorFateSystem`, grief dispersion via `RelationsGriefSink`.
- Sealed prior: Plan 177 sleep narrative (8/8), Guilt/Insomnia 15/15, MentalHealthCrisis 10/10, PsychologicalArc 13/13, Plan 43 guilt.
- Contracts: crisis events bounded; morale consequences via marks; no parallel trauma store.

## Packages
- **MH-64A** strain model: continuous strain with visible tells; no instant crisis from one event.
- **MH-64B** counselling: skill-gated sessions with a time cost; effect bounded and diminishing.
- **MH-64C** trauma care: targeted work on a memory (phantom/guilt/bereavement) with progress and relapse risk.
- **MH-64D** crisis response: de-escalation options, safety outcomes, aftermath care and a mandatory review.
- **MH-64E** recovery arc: a survivor can gain a resilience trait; scars remain but soften.
- **MH-64F** carer welfare: counsellors accumulate strain; rotation and support.
- **MH-64G** content volumes: +10 therapy interactions, +8 crisis events, +6 resilience traits; fictional/abstract.

## Acceptance & verification
- Crisis rate responds measurably to care; no cure-by-button; determinism; all effects bounded.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Psychology/`; mental-health suites; `godot --headless --path . -- --survivors-selftest`.

## Risks
Stigma/tone → careful, non-judgemental framing; care is normalised, not punished.

---

## 6. Expanded census (3 files · 958 lines)

Scope: `Assets/Ashfall.Core/Psychology/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PsychologicalProfileSystem.cs` | 355 | System | **yes** | 0 | 1 | 2 |
| `PsychologicalSanatoriumSystem.cs` | 453 | System | **yes** | 0 | 0 | 2 |
| `PsychologicalTherapyCatalog.cs` | 150 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 1 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `psychological_therapies.json` | object[3 keys] |
| `psychological_trauma.json` | object[4 keys] |
| `psychology_profiles.json` | object[3 keys] |
| `dweller_psychological_journals.json` | array[8] |

**State surfaces:** `PsychologicalProfileSystem.cs`, `PsychologicalSanatoriumSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Psychology/` (create if absent) |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 3. Other plans referencing their names: **4**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SANATORIUM-TRUTH-144` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MH-64A` | no name match — resolve at claim time |
| `MH-64B` | no name match — resolve at claim time |
| `MH-64C` | no name match — resolve at claim time |
| `MH-64D` | no name match — resolve at claim time |
| `MH-64E` | no name match — resolve at claim time |
| `MH-64F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/MentalHealthCrisisHostSession.cs`, `src/Main.FlagshipInstitutions.cs`, `src/Main.ShelterBatch3.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/DecontaminationSystemTests.cs`, `Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`, `Ashfall.Core.Tests/PsychologicalSanatoriumTests.cs`, `Ashfall.Core.Tests/Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`, `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `mental_health_crisis` |
| `psychological_arcs` |
| `psychological_sanatorium` |
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

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnCrisisResolved` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnEnvironmentalCrisisTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnHealthDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMentalBreakFromContamination` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnMentalHealthChanged` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/mental_arcs.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json` |
| `Assets/StreamingAssets/Data/psychological_therapies.json` |
| `Assets/StreamingAssets/Data/psychological_trauma.json` |

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

Host files (`src/`) whose names share a domain token: **6**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/MentalHealthCrisisHostSession.cs` |
| `src/Host/PsychologicalSanatoriumSaveStore.cs` |
| `src/Host/SurvivorMentalHealthSaveStore.cs` |
| `src/Main.BriefingCrisis.cs` |
| `src/UI/DesperationCrisisPanel.cs` |
| `src/UI/MentalHealthCrisisPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `mental_health_crisis` | no |
| `psychological_arcs` | no |
| `psychological_sanatorium` | no |
| `survivor_mental_health` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/dweller_psychological_journals.json` | CODEX_ONLY |
| `psychological_therapies.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 4 (laddered 0) · RNG streams 0 · host files 6 · catalogs 6 · test regions 0 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MENTAL-HEALTH-THERAPY-64
wave: 6
status: PROPOSED — foreman claim required
packages: MH-64A, MH-64B, MH-64C, MH-64D, MH-64E, MH-64F, MH-64G
claim paths:
  - src/Host/MentalHealthCrisisHostSession.cs  # §19 candidate host surface
  - src/Host/PsychologicalSanatoriumSaveStore.cs  # §19 candidate host surface
  - src/Host/SurvivorMentalHealthSaveStore.cs  # §19 candidate host surface
  - src/Main.BriefingCrisis.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/mental_arcs.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh <focused-region>/  # resolve target at claim time
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
