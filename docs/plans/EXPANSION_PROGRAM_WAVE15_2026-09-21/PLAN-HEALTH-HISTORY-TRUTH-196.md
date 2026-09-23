# PLAN-HEALTH-HISTORY-TRUTH-196 — Medical Records, Consent & Care Continuity

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ACUTE-TRAUMA-CARE-124, PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-HEALTH-HISTORY-TRUTH-196_APPENDIX-A_SCAFFOLD.md`](PLAN-HEALTH-HISTORY-TRUTH-196_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ACUTE-TRAUMA-CARE-124` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no care model (Plans 47/124), no diagnosis engine (Plan 182), no
new medical content.

## 1. Outcome
`Medical/HealthHistorySystem.cs` (**411 lines**) is reachable and unaddressed:
the record of what happened to each survivor medically. Care (Plans 47, 124)
and diagnostics (Plan 182) produce events; nothing owns the **history** — past
conditions, treatments, doses, and allergies — which care continuity and
ending evaluation both need.

| Deliverable | Detail |
|---|---|
| Record model | per-survivor entries: condition class, treatment, day, outcome — written at the event source, never inferred |
| Read use | care systems (124/144) read history for continuity; the record is read-only to them |
| Privacy rule | who may read a record is documented (self, medic role via Plan 141); no free global access |
| Retention | entries persist for the campaign; a summary is not a replacement for the record |
| Save truth | history restores with the survivor; a load never appends or truncates entries |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs` (411 lines; unaddressed — Wave 13/15 audit).
- Plan 124/47 are the write sources; Plan 144 reads for treatment context.
- Plan 141's roles gate who reads.
- Plan 87's ladder rules apply if the record's shape changes.

## 3. Packages
- **HHT-196A** record model + write-source table.
- **HHT-196B** continuity read tests (care sees prior history).
- **HHT-196C** privacy/role access test per role class.
- **HHT-196D** retention test (no silent truncation).
- **HHT-196E** save round-trip; no append/truncate on load.

## 4. Acceptance & verification
- Every entry traces to an event source; no inferred entries.
- Unauthorized roles cannot read; authorized ones can.
- Save/load reproduces the exact record set.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Record drift → write-source table is closed; inferred entries fail review.
Privacy theater → access is tested per role, not documented only.

---

## 6. Expanded census (1 files · 411 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `HealthHistorySystem.cs` | 411 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `HealthHistorySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
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
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `HHT-196A` | no name match — resolve at claim time |
| `HHT-196B` | `HealthHistorySystem.cs` |
| `HHT-196C` | no name match — resolve at claim time |
| `HHT-196D` | no name match — resolve at claim time |
| `HHT-196E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **9** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Host/CompletionHistorySelfTest.cs`, `src/UI/CombatHistoryPanel.cs`, `src/UI/GameDashboardPanel.cs`, `src/UI/MedicalPanel.cs`, `src/UI/RadiationHistoryPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Medical/Plan198HealthHistoryIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `combat` |
| `medical` |
| `medical_pipeline` |
| `medical_ward` |
| `mental_health_crisis` |
| `radiation` |
| `survivor_mental_health` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--dashboard-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--medical-selftest` |
| `--medical-ward-save-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnHealthDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMedicalProcessingCompleted` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnMentalHealthChanged` | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnRadiationExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/medical_record_templates.json` |
| `Assets/StreamingAssets/Data/medical_texts.json` |
| `Assets/StreamingAssets/Data/mental_arcs.json` |
| `Assets/StreamingAssets/Data/narrative/apiculture_red_light_audits.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_dependency_backstories.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json` |
| `Assets/StreamingAssets/Data/narrative/medical_documents_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (148 files, 1153 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `Medical` | 49 | 435 |
| `Progression` | 11 | 83 |
| `Radiation` | 10 | 78 |
| `Water` | 5 | 37 |

**Verdict:** 1153 cases sit under matching regions — run those first (`Audio`, `Balance`, `Combat`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **296**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **21**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `combat` | no |
| `economy` | no |
| `faction_espionage` | no |
| `holdfast_trade` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `economy` |
| `medical` |
| `medical_microfluidic_diagnostics` |
| `mineral_chemical` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **107**
(CODEX_ONLY 53, GAMEPLAY_CONSUMED 34, OPTIONAL 7, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 21 (laddered 0) · RNG streams 8 · host files 22 · catalogs 22 · test regions 9 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HEALTH-HISTORY-TRUTH-196
wave: 15
status: PROPOSED — foreman claim required
packages: HHT-196A, HHT-196B, HHT-196C, HHT-196D, HHT-196E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencyHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --combat-breaching-selftest
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
