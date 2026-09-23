# PLAN-STARTING-LEVEL-TRUTH-145 — Campaign Start Conditions, Grants & First-Day State

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-BALANCE-DIFFICULTY-INTEGRATION-73, PLAN-SCENARIO-AUTHORING-102, PLAN-LAUNCH-FACE-06.
**Implementation scaffold:** [`PLAN-STARTING-LEVEL-TRUTH-145_APPENDIX-A_SCAFFOLD.md`](PLAN-STARTING-LEVEL-TRUTH-145_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-BALANCE-DIFFICULTY-INTEGRATION-73` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no difficulty preset changes (Plan 73), no scenario documents
(Plan 102), no tutorial content (Plan 55).

## 1. Outcome
`StartingLevel/` holds `StartingLevelState.cs` and `StartingLevelSystem.cs`:
what a new campaign begins with. Difficulty presets already define starting
bonus items (`difficulty_presets.json` → `starting_bonus_item_ids`, Plan 73
Appendix A), and scenarios (Plan 102) can set a start state. Nothing states how
these three combine, or that the first-day state is deterministic and testable.

| Deliverable | Detail |
|---|---|
| Precedence rule | preset grants + scenario overrides + system defaults combine in one documented order; no double grants |
| Grant truth | granted items enter through the inventory seam; the first-day snapshot is reproducible from inputs |
| Scenario interplay | a scenario may tighten or loosen grants only through declared fields (schema-checked in Plan 102) |
| First-day invariants | shelter, roster, and needs begin in valid ranges; a validator asserts the invariant set |
| Save truth | a campaign saved on day one reloads byte-equivalent to a fresh start with the same inputs |

## 2. Evidence
- `Assets/Ashfall.Core/StartingLevel/`: the two files above (verified).
- `Assets/StreamingAssets/Data/difficulty_presets.json`: `starting_bonus_item_ids` per preset (Plan 73 Appendix A: SPARING grants `canned_food`, `iodine_pills`).
- Plan 102's scenario schema is the override gate; this plan consumes it.
- Plan 6 owns the launch surface; this plan supplies its state.

## 3. Packages
- **SLT-145A** precedence rule doc + combination tests (preset×scenario matrix sample).
- **SLT-145B** grant path through inventory + no-double-grant test.
- **SLT-145C** scenario override field list + schema check.
- **SLT-145D** first-day invariant validator + fixtures (one per preset).
- **SLT-145E** day-one save/reload equivalence test.

## 4. Acceptance & verification
- Same preset + same scenario → identical first-day state across runs.
- No grant appears twice from preset + scenario overlap.
- Invariants hold for every preset fixture.
- `bash scripts/run_test.sh Ashfall.Core.Tests/StartingLevel/` (create if absent).

## 5. Risks
Preset duplication → precedence table is the contract; a new grant path must add a row.
Scenario looseness → overrides are declared fields only, validated by Plan 102's schema.

---

## 6. Expanded census (2 files · 557 lines)

Scope: `Assets/Ashfall.Core/StartingLevel/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `StartingLevelState.cs` | 79 | DTO/Type | **yes** | 0 | 0 | 0 |
| `StartingLevelSystem.cs` | 478 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `starting_survivors.json` | object[2 keys] |
| `starting_supplies.json` | object[3 keys] |
| `starting_survivor_cohorts.json` | object[3 keys] |

**State surfaces:** `StartingLevelSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/StartingLevel/` (create if absent) |
| Test references | 48 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SLT-145A` | no name match — resolve at claim time |
| `SLT-145B` | no name match — resolve at claim time |
| `SLT-145C` | no name match — resolve at claim time |
| `SLT-145D` | no name match — resolve at claim time |
| `SLT-145E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 7. Host files: **9** · Test files: **55** · Data files: **3**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterAudioController.cs`, `src/Host/AutopsyHostSession.cs`, `src/Host/DecontaminationHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 55 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsyBridgeTests.cs`, `Ashfall.Core.Tests/AutopsyIntegrationTests.cs`, `Ashfall.Core.Tests/AutopsySystemTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` |
| Data (`StreamingAssets/Data/`) | 3 | `Assets/StreamingAssets/Data/legacy_traits.json`, `Assets/StreamingAssets/Data/shelter_machine_identities.json`, `Assets/StreamingAssets/Data/starting_survivors.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **26** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `autopsy` |
| `campaign` |
| `campaign_day` |
| `decontamination` |
| `expanded_shelter` |
| `host_event` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--difficulty-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnAutopsyChanged` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnCampSuppliesReserved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnRadonLevelChanged` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshRadonSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/starting_supplies.json` |
| `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` |
| `Assets/StreamingAssets/Data/starting_survivors.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **4**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/HostCli.StartingSupplies.cs` |
| `src/Host/StartingLevelHostSession.cs` |
| `src/Main.UiTests.StartingCohortLifecycle.cs` |
| `src/UI/StartingCohortSetupPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `starting_level` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `starting_supplies.json` | GAMEPLAY_CONSUMED |
| `starting_survivors.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 4 · catalogs 5 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-STARTING-LEVEL-TRUTH-145
wave: 11
status: PROPOSED — foreman claim required
packages: SLT-145A, SLT-145B, SLT-145C, SLT-145D, SLT-145E
claim paths:
  - src/Host/HostCli.StartingSupplies.cs  # §19 candidate host surface
  - src/Host/StartingLevelHostSession.cs  # §19 candidate host surface
  - src/Main.UiTests.StartingCohortLifecycle.cs  # §19 candidate host surface
  - src/UI/StartingCohortSetupPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/starting_supplies.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/starting_survivor_cohorts.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --audio-selftest
dependencies:
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
