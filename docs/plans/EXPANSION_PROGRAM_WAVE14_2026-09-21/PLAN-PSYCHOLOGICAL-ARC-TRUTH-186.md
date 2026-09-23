# PLAN-PSYCHOLOGICAL-ARC-TRUTH-186 — Personal Change Over Time: Stages & Guardrails

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-SANATORIUM-TRUTH-144, PLAN-NPC-ARCS-TRUTH-143.
**Implementation scaffold:** [`PLAN-PSYCHOLOGICAL-ARC-TRUTH-186_APPENDIX-A_SCAFFOLD.md`](PLAN-PSYCHOLOGICAL-ARC-TRUTH-186_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-MENTAL-HEALTH-THERAPY-64` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no clinical model (Plan 64), no facility (Plan 144), no story arc
(Plan 143).

## 1. Outcome
`Survivors/PsychologicalArcSystem.cs` (**560 lines**) is reachable and
unaddressed: long-horizon psychological change (hardening, deterioration,
recovery arcs). Plan 64 owns the psychological model, Plan 144 treats, Plan 143
tracks story arcs. This system is the **trajectory** layer — and null
guardrails, it either loops a survivor through the same arc forever or locks
them into a decline.

| Deliverable | Detail |
|---|---|
| Stage model | arcs with stages, entry conditions from Plan 64's state, and exit conditions; one owner per transition |
| Progression/regression | documented move rules (progress, stall, regress) with bounded cycles per survivor |
| Guardrails | a survivor cannot be trapped in an unexitable stage; an arc has a terminal or reversible state declared |
| Interaction with treatment | sanatorium treatment (Plan 144) modifies progression through the documented input, not a parallel meter |
| Save truth | stage and cycle counts restore; a load never re-rolls a transition |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs` (560 lines; unaddressed — Wave 13 audit).
- Plan 64 owns the values arcs read/write; Plan 144 modifies progression.
- Plan 143's completion records are a separate axis (story vs psyche) — the boundary is stated.
- Plan 170 catches narrative contradictions arcs might create.

## 3. Packages
- **PAT-186A** stage model + transition table.
- **PAT-186B** move rules + cycle bound tests.
- **PAT-186C** guardrail fixtures (no unexitable stage; terminal/reversible declared).
- **PAT-186D** treatment-input contract with Plan 144.
- **PAT-186E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Every arc reaches a declared terminal or reversible state within its cycle bound.
- Treatment advances progression only through the documented input.
- Save/load preserves stage and counters.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Doom loops → cycle bounds and terminal states are fixtures.
Meter duplication → Plan 64 remains the value owner; the boundary test enforces it.

---

## 6. Expanded census (1 files · 560 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PsychologicalArcSystem.cs` | 560 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `PsychologicalArcSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 2 name references across the test tree |
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
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PAT-186A` | no name match — resolve at claim time |
| `PAT-186B` | no name match — resolve at claim time |
| `PAT-186C` | no name match — resolve at claim time |
| `PAT-186D` | no name match — resolve at claim time |
| `PAT-186E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **7** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/HostCli.Plans162_165.cs`, `src/Host/Phase0HostSession.cs`, `src/Host/PsychologyArcHostSession.cs`, `src/Main.Plans162_165.cs`, `src/UI/PhantomMemoryPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/PsychologicalArcSystemTests.cs`, `Ashfall.Core.Tests/Survivors/Plan24NeedsSourceMigrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `phantom_memory` |
| `psychological_arcs` |
| `psychological_sanatorium` |
| `psychology` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--psychology-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnPhantomKnock` | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/needs_performance.json` |
| `Assets/StreamingAssets/Data/npc_memory_dialogue.json` |
| `Assets/StreamingAssets/Data/phantom_heirlooms.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (200 files, 1541 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Needs` | 4 | 21 |
| `Performance` | 10 | 47 |
| `Radiation` | 10 | 78 |
| `Shelter` | 87 | 754 |

**Verdict:** 1541 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Economy`, `Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **561**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **41**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `disease` | no |
| `draisine_recovery` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **19**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **95**
(CODEX_ONLY 58, GAMEPLAY_CONSUMED 24, OPTIONAL 4, UNRESOLVED 9).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |

**Verdict:** 9 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 41 (laddered 1) · RNG streams 19 · host files 23 · catalogs 22 · test regions 10 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PSYCHOLOGICAL-ARC-TRUTH-186
wave: 14
status: PROPOSED — foreman claim required
packages: PAT-186A, PAT-186B, PAT-186C, PAT-186D, PAT-186E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/memory_decay_rates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
