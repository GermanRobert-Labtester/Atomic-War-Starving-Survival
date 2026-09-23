# PLAN-INTERNAL-SECURITY-TRUTH-224 — Shelter Counter-Intelligence & Internal Watch

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ESPIONAGE-SYSTEM-TRUTH-161, PLAN-ESPIONAGE-COUNTERINTEL-41, PLAN-BLACK-PROJECTS-TRUTH-205, PLAN-INVESTIGATION-EVIDENCE-TRUTH-121.
**Non-goals:** no field operations (Plan 161), no informant tradecraft (Plan 41),
no sealed records (Plan 205).

## 1. Outcome
Two reachable Factions systems are unaddressed: `ShelterEspionageSystem.cs`
(**341 lines**) and `CounterIntelligenceSystem.cs` (**315 lines**). Plan 161
covers outward operations; these are the **inward** side — detecting, tracing,
and managing internal leaks and foreign agents. Without a contract, suspicion is
either absent or a silent accusation generator.

| Deliverable | Detail |
|---|---|
| Detection model | suspicion signals from documented facts (access patterns, contradictions, seized items) — never a hidden random flag |
| Investigation link | a suspected case enters Plan 121's evidence chain; no parallel investigation |
| Action set | documented responses (watch, restrict access, confront) with owners; consequences route to Plans 37/29 |
| False positives | suspicion can be wrong; a cleared case records its resolution |
| Save truth | suspicion and case state restore; a load never re-accuses or clears |

## 2. Evidence
- `Assets/Ashfall.Core/Factions/ShelterEspionageSystem.cs` (341) and `Factions/CounterIntelligenceSystem.cs` (315) — both unaddressed (Wave 17 audit).
- Plan 161's operations are the threat side; Plan 41's tradecraft the informant side.
- Plan 121 owns investigation; Plan 37 outcomes.
- Plan 205's sealed records may be targets.

## 3. Packages
- **IST-224A** detection signal table (no hidden flags).
- **IST-224B** case hand-off to Plan 121.
- **IST-224C** action set + consequence routing.
- **IST-224D** false-positive/cleared-case fixtures.
- **IST-224E** save round-trip; no re-accuse on load.

## 4. Acceptance & verification
- Suspicion traces to listed signals; cases enter the evidence chain intact.
- Cleared cases persist as cleared; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/`.

## 5. Risks
Suspicion as hidden RNG → signal table is the contract.
Accusation loops → case states are terminal per path; the fixture proves it.

---

## 6. Expanded census (5 files · 1,587 lines)

Scope: `Assets/Ashfall.Core/Factions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · Support 1 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CounterIntelligenceState.cs` | 77 | DTO/Type | — | 0 | 0 | 0 |
| `CounterIntelligenceSystem.cs` | 315 | System | **yes** | 0 | 0 | 2 |
| `EspionageConsequenceRouter.cs` | 120 | Support | — | 0 | 0 | 2 |
| `EspionageSystem.cs` | 734 | System | — | 0 | 0 | 7 |
| `ShelterEspionageSystem.cs` | 341 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `espionage_missions.json` | object[3 keys] |
| `espionage_operations.json` | object[3 keys] |

**State surfaces:** `CounterIntelligenceSystem.cs`, `EspionageConsequenceRouter.cs`, `EspionageSystem.cs`, `ShelterEspionageSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Factions/` |
| Test references | 14 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 5 |
| `PLAN-ESPIONAGE-COUNTERINTEL-41` | 3 |
| `PLAN-ESPIONAGE-SYSTEM-TRUTH-161` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `IST-224A` | no name match — resolve at claim time |
| `IST-224B` | no name match — resolve at claim time |
| `IST-224C` | `EspionageConsequenceRouter.cs` |
| `IST-224D` | no name match — resolve at claim time |
| `IST-224E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **7** · Test files: **8** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CounterIntelligenceHostSession.cs`, `src/Host/CounterIntelligenceSaveStore.cs`, `src/Host/EspionageHostSession.cs`, `src/Main.FactionBranch.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Expeditions/PlanE1_29VehicleEspionageTests.cs`, `Ashfall.Core.Tests/Factions/CounterIntelligenceIntegrationTests.cs`, `Ashfall.Core.Tests/Factions/CounterIntelligenceSystemTests.cs`, `Ashfall.Core.Tests/Factions/ShelterEspionageSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **4**; isolated: **1**.

| From | → To |
|---|---|
| `CounterIntelligenceState` | `CounterIntelligenceSystem` |
| `CounterIntelligenceSystem` | `CounterIntelligenceState` |
| `EspionageConsequenceRouter` | `EspionageSystem` |
| `EspionageSystem` | `EspionageConsequenceRouter` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **19** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `airlock_security` |
| `counter_intelligence` |
| `espionage` |
| `expanded_shelter` |
| `faction_espionage` |
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

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--security-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |
| `--shelter-ops-selftest` |
| `--shelter-physics-selftest` |
| `--shelter-reputation-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnSecurityChanged` | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/espionage_missions.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/narrative/security_incident_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |
| `Assets/StreamingAssets/Data/shelter_construction.json` |
| `Assets/StreamingAssets/Data/shelter_governance_blocs.json` |
| `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (89 files, 778 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Espionage` | 1 | 4 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 778 cases sit under matching regions — run those first (`Espionage`, `NarrativeConsequence`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **56**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/EspionageHostSession.cs` |
| `src/Host/EspionageSaveStore.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `airlock_security` | no |
| `counter_intelligence` | no |
| `espionage` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `narrative/security_incident_reports_batch_2.json` | CODEX_ONLY |
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
**Surface:** save sections 19 (laddered 0) · RNG streams 1 · host files 13 · catalogs 20 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INTERNAL-SECURITY-TRUTH-224
wave: 17
status: PROPOSED — foreman claim required
packages: IST-224A, IST-224B, IST-224C, IST-224D, IST-224E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/espionage_missions.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/espionage_operations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Espionage/
  - godot --headless --path . -- --security-selftest
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
