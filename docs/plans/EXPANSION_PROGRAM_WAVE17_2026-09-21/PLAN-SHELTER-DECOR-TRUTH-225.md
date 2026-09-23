# PLAN-SHELTER-DECOR-TRUTH-225 — Comfort, Memory Markers & Shared Spaces

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-SHELTER-CAPACITY-AUTHORITY-103, PLAN-MORALE-UNREST-TRUTH-129, PLAN-CULTURAL-ARCHIVE-TRUTH-169.
**Non-goals:** no culture content (Plan 4), no occupancy (Plan 103), no mark
model (Plan 129), no vault (Plan 169).

## 1. Outcome
`Shelter/ShelterDecorSystem.cs` (**340 lines**) is reachable and unaddressed:
what the holdfast does with its shared spaces — decoration, markers, memorials
on walls. Culture (Plan 4) and archives (Plan 169) own content; the **placement
and comfort effect** of decoration is unowned, so decor is either cosmetic or a
hidden morale lever.

| Deliverable | Detail |
|---|---|
| Placement model | decor items placed in rooms with an owner (the room's built state); placement consumes the item via Plan 93 |
| Comfort effect | comfort derives from documented decor classes and room use (Plan 103); routes to Plan 129 as a typed input |
| Memory markers | markers tied to Plan 169/123 records render their record; no invented text |
| Decay/repair | decor condition via Plan 119; a ruined piece is visibly ruined |
| Save truth | placed decor and condition restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` (340 lines; unaddressed — Wave 17 audit).
- Plan 103 owns rooms the decor attaches to; Plan 129 receives comfort.
- Plan 169/123 supply record-backed markers.
- Plan 119 supplies condition decay.

## 3. Packages
- **DCR-225A** placement model + room attachment.
- **DCR-225B** comfort table + routing test to Plan 129.
- **DCR-225C** record-backed marker rendering test.
- **DCR-225D** decay/ruin fixtures.
- **DCR-225E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Placement consumes items and attaches to a room; comfort appears in Plan 129 only.
- Markers render their records; ruined decor is visibly ruined.
- Save/load preserves placement and condition.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Hidden morale lever → comfort classes and routing are fixtures.
Text invention → markers read records; a fixture asserts no generated prose.

---

## 6. Expanded census (3 files · 1,202 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ShelterFestivalEngine.cs` | 323 | System | — | 0 | 0 | 4 |
| `ShelterMuseumSystem.cs` | 539 | System | — | 0 | 0 | 2 |
| `ShelterDecorSystem.cs` | 340 | System | **yes** | 0 | 0 | 6 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `museum_collection_templates.json` | object[3 keys] |

**State surfaces:** `ShelterFestivalEngine.cs`, `ShelterMuseumSystem.cs`, `ShelterDecorSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 7 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-CREATIVE-WORKS-66` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DCR-225A` | no name match — resolve at claim time |
| `DCR-225B` | no name match — resolve at claim time |
| `DCR-225C` | no name match — resolve at claim time |
| `DCR-225D` | no name match — resolve at claim time |
| `DCR-225E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/ShelterDecorHostSession.cs`, `src/Host/ShelterDecorSelfTest.cs`, `src/Main.ShelterBatch3.cs`, `src/UI/ShelterDecorPanel.cs`, `src/UI/ShelterDecorSnapshotFixture.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Culture/Plan218MuseumIntegrationTests.cs`, `Ashfall.Core.Tests/Culture/ShelterFestivalEngineTests.cs`, `Ashfall.Core.Tests/Plan12CDecorTests.cs`, `Ashfall.Core.Tests/Plan12DCrossSystemContinuityTests.cs`, `Ashfall.Core.Tests/Plan12EBalanceSimulationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
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
| `Assets/StreamingAssets/Data/museum_collection_templates.json` |
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
(11 of them panels/HUD).

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

Matched save sections: **15**, of which versioned-ladder sections:
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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 15 (laddered 0) · RNG streams 1 · host files 13 · catalogs 19 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-DECOR-TRUTH-225
wave: 17
status: PROPOSED — foreman claim required
packages: DCR-225A, DCR-225B, DCR-225C, DCR-225D, DCR-225E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/ShelterAssignmentHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/museum_collection_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
  - godot --headless --path . -- --shelter-actor-physics-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
