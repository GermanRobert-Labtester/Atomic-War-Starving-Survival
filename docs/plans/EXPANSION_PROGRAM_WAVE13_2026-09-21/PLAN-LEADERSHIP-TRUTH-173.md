# PLAN-LEADERSHIP-TRUTH-173 — Authority, Succession & Command Legitimacy

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INSTITUTIONS-TRUTH-141, PLAN-SHELTER-POLITICS-69, PLAN-MORALE-CONTAGION-TRUTH-162.
**Implementation scaffold:** [`PLAN-LEADERSHIP-TRUTH-173_APPENDIX-A_SCAFFOLD.md`](PLAN-LEADERSHIP-TRUTH-173_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-INSTITUTIONS-TRUTH-141` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no office/berth ledger (Plan 141 owns it), no policy surfaces
(Plan 69), no mood model (Plan 64/129/162).

## 1. Outcome
`Survivors/LeadershipSystem.cs` (663 lines) is reachable and unaddressed.
Institutions (Plan 141) assign offices, and politics (Plan 69) expresses
collective will — but **authority itself** (who is in command, whether their
orders are obeyed, how succession works, when legitimacy collapses) has no
stated owner. That matters because rosters, musters, and crises all assume
someone can decide.

| Deliverable | Detail |
|---|---|
| Authority model | a leader record with a legitimacy value derived from documented inputs (election, appointment, mandate, crisis performance) — never a hidden roll |
| Order compliance | which decisions require authority, and the documented effect when legitimacy is low (delay, refusal through an owner, not silent failure) |
| Succession | death/incapacity/removal paths resolve per a stated order (named deputy, election, contested) with the institutions ledger updated |
| Legitimacy sources | each source is an existing fact (Plan 141 assignment, Plan 129 marks, Plan 162 contagion as input); no parallel score |
| Save truth | leader, legitimacy, and succession state restore; a load never re-runs an election |

## 2. Evidence
- `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` (663 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 141's ledger is where a succession writes the new holder.
- Plan 129/162 supply collective mood inputs; Plan 69 presents the outcome.
- Plan 101's roster and Plan 130's muster consume authority for decisions.

## 3. Packages
- **LST-173A** authority model + legitimacy input table.
- **LST-173B** order-compliance rules + low-legitimacy fixture per effect.
- **LST-173C** succession paths + ledger write tests (Plan 141).
- **LST-173D** legitimacy source audit (no parallel score proof).
- **LST-173E** save round-trip; no re-run on load.

## 4. Acceptance & verification
- Legitimacy changes only from documented sources; a scripted crisis shows the written response.
- Each succession path updates the institutions ledger exactly once.
- Save/load preserves state; an election cannot re-fire.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Hidden authority → legitimacy is a stored, sourced value; panels read it.
Overlap with 141 → offices vs command; the boundary is asserted both ways.

---

## 6. Expanded census (1 files · 663 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `LeadershipSystem.cs` | 663 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `leadership_policies.json` | object[2 keys] |

**State surfaces:** `LeadershipSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 9 name references across the test tree |
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
| `LST-173A` | no name match — resolve at claim time |
| `LST-173B` | no name match — resolve at claim time |
| `LST-173C` | no name match — resolve at claim time |
| `LST-173D` | no name match — resolve at claim time |
| `LST-173E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **9** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Plans182_185.cs` |
| Tests (`Ashfall.Core.Tests/`) | 9 | `Ashfall.Core.Tests/Governance/Plan159_190GovernanceProvenanceIntegrationTests.cs`, `Ashfall.Core.Tests/Governance/Plan43GoverningTogetherTests.cs`, `Ashfall.Core.Tests/Governance/ShelterGovernanceEngineTests.cs`, `Ashfall.Core.Tests/LeadershipSystemTests.cs`, `Ashfall.Core.Tests/Production/Plan35_43ProductionGovernanceIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **16** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
| `radio_program_production` |
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

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnProductionCompleted` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnProductionTick` | `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` |
| `OnProvenanceComplete` | `Assets/Ashfall.Core/Muster/ColdCountSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |
| `Assets/StreamingAssets/Data/leadership_policies.json` |
| `Assets/StreamingAssets/Data/narrative/apiculture_red_light_audits.json` |
| `Assets/StreamingAssets/Data/narrative/cold_process_soap_curing_reports.json` |
| `Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (138 files, 1085 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Foundry` | 8 | 73 |
| `Governance` | 5 | 27 |
| `Integration` | 16 | 74 |
| `MoralChoice` | 4 | 33 |
| `Production` | 3 | 12 |
| `Shelter` | 87 | 754 |

**Verdict:** 1085 cases sit under matching regions — run those first (`Audio`, `Combat`, `Foundry`, `Governance`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **213**
(25 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |
| `src/Host/Plans130To133HostSessions.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `combat` | no |
| `encounter_choice` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `foundry` | no |
| `moral_choice` | no |
| `radio_program_production` | no |
| `regional_treaty` | no |
| `shelter` | no |
| `shelter_assignment` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `acoustic_detection` |
| `combat` |
| `cupola_foundry` |
| `foundry` |
| `moral_choice` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **85**
(CODEX_ONLY 35, GAMEPLAY_CONSUMED 37, OPTIONAL 2, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 23 (laddered 0) · RNG streams 6 · host files 21 · catalogs 22 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LEADERSHIP-TRUTH-173
wave: 13
status: PROPOSED — foreman claim required
packages: LST-173A, LST-173B, LST-173C, LST-173D, LST-173E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/cupola_foundry_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --plans-122-125-balance-soak
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
