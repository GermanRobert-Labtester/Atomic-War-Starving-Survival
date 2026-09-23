# PLAN-MORAL-BRANCHING-TRUTH-231 — Personal Moral Turns: Decisions & Drift

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MORAL-CHOICE-TRUTH-136, PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132, PLAN-MENTAL-HEALTH-THERAPY-64.
**Non-goals:** no choice catalog/rules (Plan 136), no consequence graph
(Plan 132), no psychological model (Plan 64).

## 1. Outcome
`Survivors/MoralBranchingSystem.cs` (**295 lines**) is reachable and
unaddressed: how a survivor's moral trajectory branches over the campaign —
hardening, compromise, or adherence — distinct from a single moral choice's
resolve. Plan 136 owns choices; this system is the **accumulated drift**.

| Deliverable | Detail |
|---|---|
| Trajectory model | states (adherent, pragmatic, hardened, broken) derived from choice history (Plan 136) and witnessed events |
| Transitions | documented thresholds with hysteresis; a survivor does not oscillate on a single event |
| Effect routing | trajectory feeds Plan 64 and dialogue/story gates as a typed input; no private moral score |
| Reversibility | documented recovery path (care, relationships) toward earlier states |
| Save truth | trajectory state restores; no re-derivation that could differ on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` (295 lines; unaddressed — Wave 17 audit).
- Plan 136's resolve results are the history input.
- Plan 64 owns psychological state; Plan 43/122 events inform transitions.
- Plan 170 can catch contradictions the trajectory might create.

## 3. Packages
- **MBT-231A** trajectory model + threshold table.
- **MBT-231B** transition/hysteresis fixtures.
- **MBT-231C** routing to Plan 64/gates (no private score).
- **MBT-231D** recovery path tests.
- **MBT-231E** save round-trip; no re-derivation drift on load.

## 4. Acceptance & verification
- Trajectory derives from stored history; single events do not flip states.
- Recovery follows the documented path; save/load preserves state exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Score duplication → routing only; a proof test asserts it.
Oscillation → hysteresis is a fixture.

---

## 6. Expanded census (1 files · 295 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MoralBranchingSystem.cs` | 295 | System | **yes** | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `MoralBranchingSystem.cs`.

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

Domain method: plan-body artifact list.
Governed artifacts: 3. Other plans referencing them: **1**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `MoralBranchingSystem.cs` |
| `Survivors/MoralBranchingSystem.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `MBT-231A` | no name match — resolve at claim time |
| `MBT-231B` | no name match — resolve at claim time |
| `MBT-231C` | no name match — resolve at claim time |
| `MBT-231D` | no name match — resolve at claim time |
| `MBT-231E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/Phase0HostSession.cs`, `src/UI/MoralChoiceModal.cs`, `src/UI/Phase0Panel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`, `Ashfall.Core.Tests/MoralBranchingSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounter_choice` |
| `moral_choice` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--moral-choice-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/confession_secrets.json` |
| `Assets/StreamingAssets/Data/moral_choice_chains.json` |
| `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` |
| `Assets/StreamingAssets/Data/moral_choice_flags.json` |
| `Assets/StreamingAssets/Data/moral_choice_gossip.json` |
| `Assets/StreamingAssets/Data/moral_choice_quests.json` |
| `Assets/StreamingAssets/Data/moral_choice_quests_branching.json` |
| `Assets/StreamingAssets/Data/moral_choice_quests_distress.json` |
| `Assets/StreamingAssets/Data/moral_choice_quests_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/artesian_well_contamination_logs.json` |
| `Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (184 files, 1453 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `MoralChoice` | 4 | 33 |
| `Quests` | 4 | 25 |
| `Shelter` | 87 | 754 |

**Verdict:** 1453 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Economy`, `Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **410**
(229 of them panels/HUD).

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

Matched save sections: **45**, of which versioned-ladder sections:
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
| `deep_well` | no |
| `disease` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **159**
(CODEX_ONLY 82, GAMEPLAY_CONSUMED 50, OPTIONAL 7, UNRESOLVED 20).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `confession_secrets.json` | OPTIONAL |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** 20 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **5**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_ignored_distress` |
| `flag_preserved_archive` |
| `flag_responded_distress` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 45 (laddered 1) · RNG streams 15 · host files 27 · catalogs 22 · test regions 9 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MORAL-BRANCHING-TRUTH-231
wave: 17
status: PROPOSED — foreman claim required
packages: MBT-231A, MBT-231B, MBT-231C, MBT-231D, MBT-231E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/confession_secrets.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/moral_choice_chains.json  # §17 catalog (verify schema + consumer)
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
