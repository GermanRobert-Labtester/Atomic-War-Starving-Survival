# PLAN-SHELTER-PRISONER-TRUTH-243 — Holding Facility: Bays, Watches & Treatment

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PRISONER-TRUTH-197, PLAN-INTERNAL-SECURITY-TRUTH-224, PLAN-SHELTER-CAPACITY-AUTHORITY-103.
**Non-goals:** no holding record/lifecycle (Plan 197), no internal security cases
(Plan 224), no room capacity model (Plan 103).

## 1. Outcome
`Shelter/ShelterPrisonerSystem.cs` (**319 lines**) is reachable and unaddressed:
the **facility** side of captivity — bays, watch coverage, and treatment
conditions inside the holdfast. Plan 197 owns the holding record; this system
physically houses it. Without a contract, custody consumes no room and watch
duty is invisible.

| Deliverable | Detail |
|---|---|
| Bay model | holding bays from rooms (Plan 103) with capacity and security level |
| Watch coverage | guarding consumes roster time (Plan 101); uncovered custody degrades per a documented rule |
| Treatment conditions | bands from Plan 197 applied to facility state (food, warmth, medical) through existing owners |
| Incidents | escape/incident events derive from documented inputs (security, watch, opportunity), seeded |
| Save truth | placements and watch state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/ShelterPrisonerSystem.cs` (319 lines; unaddressed — Wave 18 audit).
- Plan 197 owns the record; boundaries asserted.
- Plan 101 supplies guard time; Plan 103 rooms.
- Plan 224's cases may trigger transfers — boundary noted.

## 3. Packages
- **SPT-243A** bay model + security-level table.
- **SPT-243B** watch-coverage tests (no double availability).
- **SPT-243C** treatment-band application through owners.
- **SPT-243D** incident rule + seeded fixture.
- **SPT-243E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Bays occupied exactly; guards occupy roster coverage.
- Incidents follow their rule; save/load preserves facility state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Invisible custody → occupancy and watch are visible.
Tone → administrative framing only; no depiction beyond state.

---

## 6. Expanded census (3 files · 819 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `PrisonerSystem.cs` | 403 | System | — | 0 | 0 | 2 |
| `CaptiveInterrogationCatalog.cs` | 97 | Catalog | — | 0 | 0 | 0 |
| `ShelterPrisonerSystem.cs` | 319 | System | **yes** | 0 | 0 | 3 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `captive_interrogations.json` | object[3 keys] |

**State surfaces:** `PrisonerSystem.cs`, `ShelterPrisonerSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 11 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-PRISONER-TRUTH-197` | 1 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SPT-243A` | no name match — resolve at claim time |
| `SPT-243B` | no name match — resolve at claim time |
| `SPT-243C` | no name match — resolve at claim time |
| `SPT-243D` | no name match — resolve at claim time |
| `SPT-243E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/DefenseHostSession.cs`, `src/Main.Plans162_165.cs`, `src/Main.Plans178_181.cs`, `src/Main.Plans62_65.cs`, `src/UI/PrisonerPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Factions/PrisonerSystemTests.cs`, `Ashfall.Core.Tests/Integration/Plans178_181_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Shelter/PlanE1_28CaptivePreservationTests.cs`, `Ashfall.Core.Tests/Shelter/ShelterPrisonerSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `ShelterPrisonerSystem` | `CaptiveInterrogationCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **16** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
| `prisoner_management` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |
| `shelter_prisoners` |
| `shelter_reputation` |
| `shelter_schedule` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
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
| `--shelter-security-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/captive_interrogations.json` |
| `Assets/StreamingAssets/Data/interrogation_tactics.json` |
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

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (87 files, 754 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Shelter` | 87 | 754 |

**Verdict:** 754 cases sit under matching regions — run those first (`Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **49**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/PrisonerSaveStore.cs` |
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
| `prisoner_management` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 3, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `interrogation_tactics.json` | GAMEPLAY_CONSUMED |
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

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_executed_prisoner` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 16 (laddered 0) · RNG streams 1 · host files 14 · catalogs 20 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-PRISONER-TRUTH-243
wave: 18
status: PROPOSED — foreman claim required
packages: SPT-243A, SPT-243B, SPT-243C, SPT-243D, SPT-243E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/PrisonerSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/captive_interrogations.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/interrogation_tactics.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
  - godot --headless --path . -- --shelter-actor-physics-selftest
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
