# PLAN-CAREGIVING-TRUTH-203 — Dependents, Care Load & Carer Burnout

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ACUTE-TRAUMA-CARE-124, PLAN-FAMILY-DYNASTY-43, PLAN-DUTY-ROSTER-TRUTH-101, PLAN-MENTAL-HEALTH-THERAPY-64.
**Implementation scaffold:** [`PLAN-CAREGIVING-TRUTH-203_APPENDIX-A_SCAFFOLD.md`](PLAN-CAREGIVING-TRUTH-203_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SURVIVOR-ROSTER-TRUTH-244` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no clinical model (Plan 64/124), no family graph (Plan 43), no
roster (Plan 101).

## 1. Outcome
`Survivors/CaregivingSystem.cs` (**393 lines**) is reachable and unaddressed:
who cares for the injured, the very young, and the infirm — and what that
costs the carer. Care facilities (Plan 144) and acute care (Plan 124) exist;
the **home care load** and its effect on the carer's time and state are not
modeled, so dependents are either free upkeep or invisible.

| Deliverable | Detail |
|---|---|
| Care-load model | dependents generate a care load; a carer covers it through roster time (Plan 101), reducing other availability |
| Carer selection | assignment by relation (Plan 43) with a documented fallback (any qualified adult) |
| Carer strain | sustained care without relief raises strain, routed to Plan 64's model as a typed input — no private meter |
| Relief paths | facility care (Plan 144), rotation, and shared care reduce load per documented rules |
| Save truth | assignments and strain restore; a load never reassigns silently |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` (393 lines; unaddressed — Wave 13/15 audit).
- Plan 101's coverage must show the carer's time as occupied.
- Plan 64 receives strain; Plan 144 is a relief path.
- Plan 124 refers patients; the boundary: acute treatment vs home care — stated.

## 3. Packages
- **CGT-203A** care-load model + dependent classes.
- **CGT-203B** carer assignment/fallback tests.
- **CGT-203C** roster-time occupancy test (no double availability).
- **CGT-203D** strain input to Plan 64 + relief-path tests.
- **CGT-203E** save round-trip; no silent reassignment.

## 4. Acceptance & verification
- Carers appear occupied in coverage exactly as modeled.
- Strain appears in Plan 64; relief reduces it per rule.
- Save/load preserves assignments and strain.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Invisible dependents → the load is visible in coverage and the care view.
Meter duplication → strain is a typed input; the boundary test asserts it.

---

## 6. Expanded census (1 files · 393 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CaregivingSystem.cs` | 393 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `CaregivingSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CGT-203A` | no name match — resolve at claim time |
| `CGT-203B` | no name match — resolve at claim time |
| `CGT-203C` | no name match — resolve at claim time |
| `CGT-203D` | no name match — resolve at claim time |
| `CGT-203E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/CaregivingHostSession.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `src/Main.ShelterSocial.cs`, `src/UI/CaregivingPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/CaregivingCommandTests.cs`, `Ashfall.Core.Tests/CaregivingSystemTests.cs`, `Ashfall.Core.Tests/SurvivorFateSystemTests.cs`, `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **22** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caregiving` |
| `expanded_shelter` |
| `nuclear_core_lifecycle` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |
| `shelter_prisoners` |
| `shelter_reputation` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **22** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnCaregivingBondDeepened` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingDialogueUnlocked` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingEnded` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnCaregivingStarted` | `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorJoined` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (156 files, 1290 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Lifecycle` | 1 | 5 |
| `PlayerCommand` | 1 | 1 |
| `Shelter` | 87 | 754 |

**Verdict:** 1290 cases sit under matching regions — run those first (`Audio`, `Combat`, `Economy`, `Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **572**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
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
| `src/Host/AnomalyHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **41**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `anomaly_hazard` | no |
| `black_market` | no |
| `caravan_trade_network` | no |
| `caregiving` | no |
| `combat` | no |
| `deep_well` | no |
| `disease` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **18**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **92**
(CODEX_ONLY 46, GAMEPLAY_CONSUMED 30, OPTIONAL 5, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 41 (laddered 0) · RNG streams 18 · host files 25 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CAREGIVING-TRUTH-203
wave: 15
status: PROPOSED — foreman claim required
packages: CGT-203A, CGT-203B, CGT-203C, CGT-203D, CGT-203E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --asset-coverage-report
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
