# PLAN-LABOUR-PROFESSIONS-68 — Shifts, Unions, Strikes & Professional Identity

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-SCIENCE-EDUCATION-38,
PLAN-RECREATION-MORALE-50.
**Implementation scaffold:** [`PLAN-LABOUR-PROFESSIONS-68_APPENDIX-A_SCAFFOLD.md`](PLAN-LABOUR-PROFESSIONS-68_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real unions/labour politics, no real-world ideological
references; fictional and restrained.

## Outcome
Work is modeled (duty roster, roles, fitness-for-duty, fatigue, hazard classes,
trade specialties, apprenticeships) but has no **politics or identity**: no
grievances, no negotiation, no professional craft pride beyond skill numbers.
This plan adds a labour layer that creates stories rather than just throughput.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Shifts & roles | duty roster | assign, rotate | output, fatigue, fairness |
| Grievances | relations + morale marks | hear, address, ignore | morale, refusal risk |
| Negotiation | `ShelterGovernanceEngine`/policy | set hours, pay, perks | productivity, loyalty |
| Professional pride | trade specialties | master a craft | quality bonus, teaching |
| Teaching | apprenticeship | pair, train | succession, knowledge spread |
| Safety | hazard classes, medical | enforce safety | injury rate, trust |
| Strikes/work refusal | `SurvivorAutonomySystem` | mediate, concede, force | output loss, legitimacy |
| Records | chronicle/archive | document | legacy, memory |

## Evidence
- Core: duty roster systems (sealed `ward` role, `StaffingPreflight`), `SurvivorAutonomySystem` (refusal override mechanics), `TradeSpecialtySystem` (16 professions), `ApprenticeshipSystem`, `MoraleMarkSystem`, `PolicySystem`/`ShelterGovernanceEngine` (orphans), `Governance/` (3 files).
- Data: `duty_roles.json`, `trade_specialties.json`, `shelter_governance_blocs.json`, `policy` catalogs.
- Sealed prior: Plan 24 ward, Plan 105 specialties (17/17), Plan 144 autonomy (6/6), Plan 185 skill dormancy tick.
- Contracts: one morale authority; refusal is typed and bounded; no second roster.

## Packages
- **LB-68A** grievance model: causes from fairness, danger, overwork, favouritism; visible and addressable.
- **LB-68B** negotiation: policy decisions (hours, rest, perks, safety) with costs and effects through morale/loyalty.
- **LB-68C** refusal/strike: staged escalation (complaint → refusal → strike), mediation options, resolution or suppression consequences.
- **LB-68D** craft pride: mastery grants quality/teaching bonuses; a master's departure costs knowledge (ties Plan 38 diffusion).
- **LB-68E** safety enforcement: hazard class policy reduces injuries but costs time; violations create events.
- **LB-68F** records: labour history in the archive; strikes/settlements become chronicle entries.
- **LB-68G** content volumes: +10 grievances, +8 policy rows, +6 settlement outcomes; fictional.

## Acceptance & verification
- Escalation is deterministic and reversible; no dead-end strike; morale effects bounded.
- `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/`; autonomy suites; `godot --headless --path . -- --duty-roster-selftest`.

## Risks
Politics fatigue → events are episodic and consequence-driven; the player can delegate via policy.

---

## 6. Expanded census (1 files · 321 lines)

Scope: `Assets/Ashfall.Core/DutyRoster/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SurvivorRoleSystem.cs` | 321 | System | — | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `duty_roles.json` | object[6 keys] |
| `survivor_roles.json` | object[2 keys] |

**State surfaces:** `SurvivorRoleSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/DutyRoster/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 1. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `LB-68A` | no name match — resolve at claim time |
| `LB-68B` | no name match — resolve at claim time |
| `LB-68C` | no name match — resolve at claim time |
| `LB-68D` | no name match — resolve at claim time |
| `LB-68E` | no name match — resolve at claim time |
| `LB-68F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 6. Host files: **2** · Test files: **5** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/Phase0HostSession.cs`, `src/Main.Phase0.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`, `Ashfall.Core.Tests/IntegrityScratchFixture.cs`, `Ashfall.Core.Tests/Progression/TradeSpecialtyCatalogMetadataTests.cs`, `Ashfall.Core.Tests/Survivors/Plan195SurvivorRoleIntegrationTests.cs`, `Ashfall.Core.Tests/TradeSpecialtySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/duty_roles.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **22** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `duty_roster` |
| `expanded_shelter` |
| `holdfast_trade` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |
| `shelter_prisoners` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **20** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--holdfast-trade-save-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSpecialtyMastered` | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` |
| `OnSpecialtyMilestone` | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (173 files, 1306 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `DutyRoster` | 5 | 49 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Governance` | 5 | 27 |
| `Integration` | 16 | 74 |
| `Quests` | 4 | 25 |
| `Shelter` | 87 | 754 |

**Verdict:** 1306 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Combat`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **345**
(31 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **52**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `black_projects_archive` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `combat` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `combat` |
| `cupola_foundry` |
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **109**
(CODEX_ONLY 26, GAMEPLAY_CONSUMED 57, OPTIONAL 5, UNRESOLVED 21).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |

**Verdict:** 21 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 52 (laddered 1) · RNG streams 14 · host files 26 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LABOUR-PROFESSIONS-68
wave: 6
status: PROPOSED — foreman claim required
packages: LB-68A, LB-68B, LB-68C, LB-68D, LB-68E, LB-68F, LB-68G
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/caravan_trade_routes.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
