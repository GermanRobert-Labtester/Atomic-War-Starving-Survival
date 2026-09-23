# PLAN-DESPERATION-TRUTH-232 — Breaking Points, Risky Choices & Recovery

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MENTAL-HEALTH-THERAPY-64, PLAN-RATIONING-TRUTH-174, PLAN-MORALE-UNREST-TRUTH-129, PLAN-JUSTICE-LAW-37.
**Non-goals:** no rationing model (Plan 174), no morale marks (Plan 129), no
legal system (Plan 37).

## 1. Outcome
`Survivors/DesperationSystem.cs` (**289 lines**) is reachable and unaddressed:
what a survivor does when pushed past their limits — theft, desertion, betrayal,
or silent endurance. It is a high-risk system because it converts scarcity into
behavior; unstated, it is either absent or a random punishment.

| Deliverable | Detail |
|---|---|
| Desperation model | level derived from documented pressures (hunger Plan 39/174, exposure Plan 117, grief Plan 64); no hidden input |
| Behavior table | per level, documented risk-behaviors with owners (theft → Plan 44/93, desertion → roster Plan 101, confrontation → Plan 37) |
| Thresholds | visible warnings before a threshold; a survivor under care (Plan 144) is protected per rule |
| Recovery | restoring pressure relieves desperation per the documented curve |
| Save truth | level and pending behaviors restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/DesperationSystem.cs` (289 lines; unaddressed — Wave 17 audit).
- Plan 174 owns rationing pressure; Plan 39 food; Plan 117 exposure.
- Plan 44/93/101/37 receive behaviors.
- Plan 129 receives collective reaction where applicable.

## 3. Packages
- **DSP-232A** pressure model + input table.
- **DSP-232B** behavior table + owner routing tests.
- **DSP-232C** warning-before-threshold fixtures.
- **DSP-232D** recovery curve test.
- **DSP-232E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Desperation derives from listed pressures; behaviors route to owners with a notice.
- Warnings precede thresholds; recovery follows the curve.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Random punishment → inputs, thresholds, and warnings are all explicit.
Behavior overlap → each behavior names its owner; no parallel action system.

---

## 6. Expanded census (1 files · 289 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DesperationSystem.cs` | 289 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `desperation_events.json` | object[2 keys] |

**State surfaces:** `DesperationSystem.cs`.

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
Governed artifacts: 4. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-FOOD-CUISINE-39` | 1 |
| `PLAN-RATIONING-TRUTH-174` | 1 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/Survivors/DesperationSystem.cs` |
| `DesperationSystem.cs` |
| `Survivors/DesperationSystem.cs` |
| `desperation_events.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DSP-232A` | no name match — resolve at claim time |
| `DSP-232B` | no name match — resolve at claim time |
| `DSP-232C` | no name match — resolve at claim time |
| `DSP-232D` | no name match — resolve at claim time |
| `DSP-232E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **6** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ExpansionAudioBridge.cs`, `src/Host/NarrativeQuestlineHostSession.cs`, `src/Main.Audio.cs`, `src/Main.Plans186_189.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Integration/Plans186_189_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Survivors/DesperationSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `desperation` |
| `events` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **10** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--narrative-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnQuestlineResolved` | `Assets/Ashfall.Core/Muster/MusterSystem.cs` |
| `OnQuestlineStarted` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (198 files, 1601 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Events` | 1 | 6 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 1601 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Campaign`, `Events`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **358**
(20 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **40**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `campaign` | no |
| `campaign_day` | no |
| `crossing` | no |
| `deep_well` | no |
| `desperation` | no |
| `disease` | no |
| `dynamic_quests` | no |
| `encounters` | no |
| `equipment_condition` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `aquaponics_disease` |
| `black_market_debt_event` |
| `cupola_foundry` |
| `deep_coast` |
| `disease` |
| `events` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **364**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 56, OPTIONAL 5, UNRESOLVED 24).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 24 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_become_warlord` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 40 (laddered 0) · RNG streams 13 · host files 23 · catalogs 22 · test regions 10 · flags 10

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DESPERATION-TRUTH-232
wave: 17
status: PROPOSED — foreman claim required
packages: DSP-232A, DSP-232B, DSP-232C, DSP-232D, DSP-232E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --audio-selftest
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
