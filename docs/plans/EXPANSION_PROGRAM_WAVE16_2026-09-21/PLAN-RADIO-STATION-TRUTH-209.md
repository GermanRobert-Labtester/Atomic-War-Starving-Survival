# PLAN-RADIO-STATION-TRUTH-209 — Station Operations, Broadcast Schedule & Reach

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIO-MEDIA-42, PLAN-PRINT-MEDIA-TRUTH-128, PLAN-POWER (Plan 48), PLAN-PSYOPS-TRUTH-210.
**Non-goals:** no broadcast content authoring (Plan 42), no print (Plan 128), no
influence operations (Plan 210).

## 1. Outcome
`Radio/ShelterRadioStationSystem.cs` (**430 lines**) and
`Radio/RadioScheduleCoordinator.cs` (**427 lines**) are reachable and
unaddressed: running a station and scheduling its output. Plan 42 owns the
media/content side; the **operations** side — power draw, schedule, reach,
maintenance — is unowned, so a station is either always on or inert.

| Deliverable | Detail |
|---|---|
| Station model | transmitter with condition (Plan 119 contract), power draw (Plan 48), and a documented reach by antenna class |
| Schedule | time slots on the canonical clock with content classes from Plan 42; overlaps/conflicts are visible |
| Reach truth | who receives a broadcast derives from reach + receivers in the area; no global guarantee |
| Failure modes | power loss, transmitter failure, jamming (Plan 210 input) with documented degraded states |
| Save truth | schedule, condition, and power state restore; a load never re-broadcasts |

## 2. Evidence
- `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs` and `Radio/RadioScheduleCoordinator.cs` — both unaddressed (Wave 16 audit).
- Plan 42 owns content/media; Plan 128 print is the parallel medium.
- Plan 48 supplies power; Plan 119 the decay contract.
- Plan 210's influence operations may jam or use the station — boundary stated.

## 3. Packages
- **RST-209A** station model + reach table.
- **RST-209B** schedule + conflict fixtures.
- **RST-209C** power/condition integration tests.
- **RST-209D** failure modes + jamming input contract.
- **RST-209E** save round-trip; no re-broadcast on load.

## 4. Acceptance & verification
- Schedule conflicts are visible; reach determines receipt in a two-receiver fixture.
- Power loss degrades per the documented state; no broadcast without power.
- Save/load preserves schedule and condition.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/`.

## 5. Risks
Always-on station → power draw and condition are the constraints, both tested.
Reach inflation → reach is a table with a fixture, not a global flag.

---

## 6. Expanded census (5 files · 1,509 lines)

Scope: `Assets/Ashfall.Core/Radio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Loader 1 · Support 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DistressFollowUpScheduler.cs` | 366 | Support | — | 0 | 0 | 2 |
| `RadioScheduleCoordinator.cs` | 427 | System | **yes** | 0 | 0 | 0 |
| `RadioStationCatalog.cs` | 163 | Catalog | — | 0 | 0 | 0 |
| `RadioStationCatalogLoader.cs` | 123 | Loader | — | 0 | 0 | 0 |
| `ShelterRadioStationSystem.cs` | 430 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `radio_stations.json` | object[2 keys] |
| `ecological_infestations.json` | object[5 keys] |
| `shelter_schedules.json` | object[3 keys] |
| `waystations.json` | object[2 keys] |
| `bunker_shift_schedules_and_notices.json` | object[4 keys] |
| `load_shed_schedule_001.json` | object[16 keys] |

**State surfaces:** `DistressFollowUpScheduler.cs`, `ShelterRadioStationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Radio/` |
| Test references | 25 name references across the test tree |
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
| `PLAN-RADIO-MEDIA-42` | 5 |
| `PLAN-RADIO-FAMILY-TRUTH-266` | 5 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `RST-209A` | `RadioStationCatalog.cs`, `RadioStationCatalogLoader.cs`, `ShelterRadioStationSystem.cs` |
| `RST-209B` | `DistressFollowUpScheduler.cs`, `RadioScheduleCoordinator.cs` |
| `RST-209C` | no name match — resolve at claim time |
| `RST-209D` | no name match — resolve at claim time |
| `RST-209E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **8** · Test files: **21** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Host/RadioCatalogSelfTest.cs`, `src/Host/RadioHostSession.cs`, `src/Main.Audio.cs` |
| Tests (`Ashfall.Core.Tests/`) | 21 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs`, `Ashfall.Core.Tests/FactionRadioBroadcastExpansionTests.cs`, `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **3**; isolated: **2**.

| From | → To |
|---|---|
| `RadioScheduleCoordinator` | `RadioStationCatalog` |
| `RadioStationCatalog` | `RadioStationCatalogLoader` |
| `RadioStationCatalogLoader` | `RadioStationCatalog` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **19** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `schedule` |
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

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--radio-catalog-selftest` |
| `--radio-selftest` |
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

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnScheduleChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnStationStateChanged` | `Assets/Ashfall.Core/WeatherStationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/moral_choice_quests_distress.json` |
| `Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json` |
| `Assets/StreamingAssets/Data/narrative/numbers_station_ciphers.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (134 files, 1108 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radio` | 47 | 354 |
| `Shelter` | 87 | 754 |

**Verdict:** 1108 cases sit under matching regions — run those first (`Radio`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **64**
(15 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |
| `src/Host/RadioSaveStore.cs` |
| `src/Host/RadioStationSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `schedule` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `radio` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **25**
(CODEX_ONLY 10, GAMEPLAY_CONSUMED 10, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `moral_choice_quests_distress.json` | UNRESOLVED |
| `narrative/load_shed_schedule_001.json` | CODEX_ONLY |
| `narrative/numbers_station_ciphers.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |
| `narrative/radio_scriptbook.json` | CODEX_ONLY |
| `narrative/radio_scripts_expansion.json` | CODEX_ONLY |
| `narrative/radio_transcripts_batch_2.json` | CODEX_ONLY |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_ignored_distress` |
| `flag_responded_distress` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 19 (laddered 0) · RNG streams 2 · host files 16 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RADIO-STATION-TRUTH-209
wave: 16
status: PROPOSED — foreman claim required
packages: RST-209A, RST-209B, RST-209C, RST-209D, RST-209E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_radio_corpus.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_war_radio.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
  - godot --headless --path . -- --radio-catalog-selftest
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
