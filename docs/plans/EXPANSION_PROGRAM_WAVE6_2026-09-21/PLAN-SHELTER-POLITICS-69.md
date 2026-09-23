# PLAN-SHELTER-POLITICS-69 — Blocs, Policies, Legitimacy & Governance

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-LABOUR-PROFESSIONS-68, PLAN-WARLORDS-DIPLOMACY-29,
PLAN-BELIEF-IDEOLOGY-36.
**Expanded appendix:** [`PLAN-SHELTER-POLITICS-69_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-SHELTER-POLITICS-69_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's governance & politics systems.

**Non-goals:** no real political systems/parties; fictional blocs and
principles only; no second morale or relations authority.

## Outcome
Governance is authored but idle: `Governance/PolicySystem.cs` (10 tests, 0 host
refs), `ShelterGovernanceEngine` (10 tests, 0 host refs), `StandingGateRegistry`
(Plan 59, sealed), `shelter_governance_blocs.json`, `shelter_origins.json`,
`shelter_social_events.json`, `PolicySystem` decision rows, plus the moral
choice path and shelter reputation (Plan 207). This plan makes the shelter a
**small polity**: blocs form, legitimacy matters, policy is a live decision.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Blocs | `ShelterGovernanceEngine`, blocs catalog | court, balance, ignore | support, opposition, cohesion |
| Legitimacy | reputation + policy outcomes | govern fairly / by force | obedience vs resentment |
| Policy | `PolicySystem` | propose, debate, decree | effects across systems |
| Petitions | survivor autonomy + grievances | hear, grant, refuse | loyalty, precedent |
| Council | social/relations | appoint, rotate | advice, bias, coup risk |
| Precedent | moral choice + records | set rules | long-term identity |
| Succession | family/legacy | name a successor | continuity, conflict |

## Evidence
- Core: `Governance/PolicySystem.cs`, `Governance/ShelterGovernanceEngine.cs`, `Governance/StandingGateRegistry.cs`, `Reputation/ShelterReputation*`, `MoralChoice/*`.
- Data: `shelter_governance_blocs.json`, `shelter_origins.json`, `shelter_social_events.json`, `policy` rows, `moral_choice_flags.json` (25).
- Sealed prior: Plan 59 gates (5/5), Plan 207 reputation, Plan 138 security, Plan 144 autonomy, Plan 148 ideology.
- Contracts: policies mutate via existing owners; no parallel legitimacy stat (derive from morale/reputation).

## Packages
- **SP-69A** blocs: authored blocs with principles, members, and demands; support shifts with policy.
- **SP-69B** legitimacy read model: derived from fairness, safety, prosperity, and fear; visible band.
- **SP-69C** policy loop: propose → debate (cost/time) → enact → measure; reversible; effects across shelter/economy/labour.
- **SP-69D** petitions/council: survivor requests with precedents; council advice can be biased.
- **SP-69E** succession: name a second; a contested succession creates a narrative arc, not a game over.
- **SP-69F** content volumes: +6 blocs, +15 policies, +12 petitions, +6 succession events; fictional.

## Acceptance & verification
- Bloc support and legitimacy changes are explainable; no policy dead-ends; determinism.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/`; `--shelter-reputation-selftest`; shelter social suites.

## Risks
Politics taking over the game → policies are decision points, not a second UI game; delegate/auto options exist.

---

## 6. Expanded census (2 files · 924 lines)

Scope: `Assets/Ashfall.Core/Governance/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PolicySystem.cs` | 255 | System | **yes** | 0 | 0 | 2 |
| `ShelterGovernanceEngine.cs` | 669 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `shelter_governance_blocs.json` | object[2 keys] |

**State surfaces:** `PolicySystem.cs`, `ShelterGovernanceEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Governance/` |
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

Domain files: 2. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-LABOUR-PROFESSIONS-68` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SP-69A` | `PolicySystem.cs` |
| `SP-69B` | no name match — resolve at claim time |
| `SP-69C` | `PolicySystem.cs`, `ShelterGovernanceEngine.cs` |
| `SP-69D` | no name match — resolve at claim time |
| `SP-69E` | no name match — resolve at claim time |
| `SP-69F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 8. Host files: **2** · Test files: **9** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Main.Plans46_49.cs` |
| Tests (`Ashfall.Core.Tests/`) | 9 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`, `Ashfall.Core.Tests/Governance/Plan43GoverningTogetherTests.cs`, `Ashfall.Core.Tests/Governance/Plan59StandingGateIntegrationTests.cs`, `Ashfall.Core.Tests/Governance/ShelterGovernanceEngineTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **21** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounter_choice` |
| `events` |
| `expanded_shelter` |
| `moral_choice` |
| `settlement_politics` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--moral-choice-selftest` |
| `--operations-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--seven-day-smoke-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnStandingCalled` | `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` |
| `OnStandingPenalty` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |
| `Assets/StreamingAssets/Data/shelter_construction.json` |
| `Assets/StreamingAssets/Data/shelter_governance_blocs.json` |
| `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` |
| `Assets/StreamingAssets/Data/shelter_machine_identities.json` |
| `Assets/StreamingAssets/Data/shelter_origins.json` |
| `Assets/StreamingAssets/Data/shelter_room_identities.json` |
| `Assets/StreamingAssets/Data/shelter_rooms.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (92 files, 781 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Governance` | 5 | 27 |
| `Shelter` | 87 | 754 |

**Verdict:** 781 cases sit under matching regions — run those first (`Governance`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **50**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/PoliticsSaveStore.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/ShelterBarterSaveStore.cs` |
| `src/Host/ShelterDecorHostSession.cs` |
| `src/Host/ShelterDecorSaveStore.cs` |
| `src/Host/ShelterDecorSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `settlement_politics` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |
| `shelter_schedules.json` | GAMEPLAY_CONSUMED |
| `shelter_social_events.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 16 (laddered 0) · RNG streams 1 · host files 13 · catalogs 19 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-POLITICS-69
wave: 6
status: PROPOSED — foreman claim required
packages: SP-69A, SP-69B, SP-69C, SP-69D, SP-69E, SP-69F
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/PoliticsSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Governance/
  - godot --headless --path . -- --moral-choice-selftest
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
