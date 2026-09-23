# PLAN-MATERIAL-SHIELDING-TRUTH-257 — Shielding Materials: Grades, Sourcing & Siting

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIATION-BACKGROUND-TRUTH-189, PLAN-METROLOGY-TRUTH-172, PLAN-INDUSTRY-AUTOMATION-45.
**Non-goals:** no shielding curve model (Plan 189), no standards (Plan 172), no
industry engines (Plan 45).

## 1. Outcome
`Shelter/MaterialShieldingSystem.cs` (**109 lines**) is reachable and
unaddressed: the **materials** side of shielding — which grades exist, where
they come from, and where they may be sited. Plan 189 owns the reduction curve;
this plan owns supply and placement, so absorption is not a free property.

| Deliverable | Detail |
|---|---|
| Grade model | shielding grades with documented absorption class and mass; source paths (salvage, industry Plan 45) |
| Siting | placement rules per structure/room (Plan 40/103); a grade in the wrong place delivers its documented (lower) value |
| Contamination | shielding can carry contamination (Plan 189's rule); handling routes to its owner |
| Stock truth | held shielding is inventory via Plan 93; consumption at siting balances |
| Save truth | sited shielding and stock restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs` (109 lines; unaddressed — Wave 18 audit).
- Plan 189's curve consumes the grade's absorption class.
- Plan 172's standards validate material claims.
- Plan 45 supplies production; Plan 93 stock.

## 3. Packages
- **MST-257A** grade model + absorption table.
- **MST-257B** siting rules + wrong-place fixture.
- **MST-257C** contamination handling hand-off.
- **MST-257D** stock/conservation tests.
- **MST-257E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Absorption follows the grade table; mis-sited material delivers its documented lower value.
- Stock balances; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Free absorption → grade/siting table is the contract.
Contamination invisibility → handled through Plan 189's rule and recorded.

---

## 6. Expanded census (2 files · 414 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MaterialShieldingSystem.cs` | 109 | System | **yes** | 0 | 0 | 2 |
| `ShelterShieldingModel.cs` | 305 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `shelter_shielding.json` | object[15 keys] |

**State surfaces:** `MaterialShieldingSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 4 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MST-257A` | `ShelterShieldingModel.cs` |
| `MST-257B` | no name match — resolve at claim time |
| `MST-257C` | no name match — resolve at claim time |
| `MST-257D` | no name match — resolve at claim time |
| `MST-257E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/SurvivorsHostSession.cs`, `src/Main.Survivors.cs`, `src/Main.UiPanels.cs`, `src/UI/ShelterPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/MaterialShieldingSystemTests.cs`, `Ashfall.Core.Tests/Radiation/ShelterRadQuerySeamTests.cs`, `Ashfall.Core.Tests/Shelter/Plan20BShelterShieldingTests.cs`, `Ashfall.Core.Tests/Shelter/Plan20BShieldingBalanceSweepTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **17** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
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
| `shelter_security` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **25** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--checksum-sweep-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--player-panels-ui-test` |
| `--player-panels-uitest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |

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
shares a domain token: **1** (87 files, 754 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Shelter` | 87 | 754 |

**Verdict:** 754 cases sit under matching regions — run those first (`Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **48**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/ShelterBarterSaveStore.cs` |
| `src/Host/ShelterDecorHostSession.cs` |
| `src/Host/ShelterDecorSaveStore.cs` |
| `src/Host/ShelterDecorSelfTest.cs` |
| `src/Host/ShelterEspionageSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |
| `shelter_reputation` | no |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 16 (laddered 0) · RNG streams 1 · host files 13 · catalogs 19 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MATERIAL-SHIELDING-TRUTH-257
wave: 18
status: PROPOSED — foreman claim required
packages: MST-257A, MST-257B, MST-257C, MST-257D, MST-257E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/ShelterAssignmentHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
  - godot --headless --path . -- --checksum-sweep-selftest
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
