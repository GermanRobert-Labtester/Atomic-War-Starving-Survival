# PLAN-NARCOTICS-TRUTH-215 — Controlled Substances: Supply, Use & Consequences

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-PHARMACEUTICAL-TRUTH-167, PLAN-CRIME-SYNDICATES-44, PLAN-ACUTE-TRAUMA-CARE-124, PLAN-MENTAL-HEALTH-THERAPY-64.
**Non-goals:** no medicine production (Plan 167), no syndicates (Plan 44), no
clinical care (Plan 124), no graphic content.

## 1. Outcome
`Medical/NarcoticsSystem.cs` (**368 lines**) is reachable and unaddressed:
substances with legitimate medical use and a dependency/market dimension.
Pharma production (Plan 167), crime (Plan 44), and care (Plan 124) exist; the
**substance lifecycle** — medical indication, dependency progression, illicit
demand — is unowned, so the mechanic is either absent or a silent debuff.

| Deliverable | Detail |
|---|---|
| Substance model | classes with legitimate indications (Plan 124), dependency rates, and withdrawal behavior |
| Dependency | progression/withdrawal routes through `DependencyTaperWithdrawalEngine` (Plan 124 package) as the single owner |
| Market dimension | illicit demand and price are rows in Plan 96's ledger, with supply pressure to Plan 44's systems as an input |
| Access control | medical stock vs personal possession recorded via Plan 93; theft/misuse leaves a trace |
| Save truth | dependency state and stock restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Medical/NarcoticsSystem.cs` (368 lines; unaddressed — Wave 16 audit).
- Plan 124's taper engine is the dependency owner; Plan 167 produces stock.
- Plan 44 receives demand pressure; Plan 96 holds price rows.
- Plan 64 receives psychological effects.

## 3. Packages
- **NCT-215A** substance/indication table.
- **NCT-215B** dependency hand-off test to Plan 124's engine.
- **NCT-215C** market rows + pressure input to Plan 44.
- **NCT-215D** access/misuse trace tests.
- **NCT-215E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Dependency appears only in the taper engine; indications match Plan 124.
- Market effects reconcile in Plan 96; misuse leaves a record.
- Save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`.

## 5. Risks
Silent debuff → dependency and withdrawal are visible through the clinical surface.
Tone → systemic framing; state and consequence, no depiction.

---

## 6. Expanded census (1 files · 368 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NarcoticsSystem.cs` | 368 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `narcotics.json` | object[2 keys] |

**State surfaces:** `NarcoticsSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 2 name references across the test tree |
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
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NCT-215A` | no name match — resolve at claim time |
| `NCT-215B` | no name match — resolve at claim time |
| `NCT-215C` | no name match — resolve at claim time |
| `NCT-215D` | no name match — resolve at claim time |
| `NCT-215E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **6** · Test files: **2** · Data files: **5**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/NarcoticsSaveStore.cs`, `src/Main.Lifecycle.cs`, `src/Main.Plans182_185.cs`, `src/Main.PlayerSurfaces.cs`, `src/UI/ChemUI.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Integration/Plans182_185_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Medical/NarcoticsSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 5 | `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/narcotics.json`, `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json`, `Assets/StreamingAssets/Data/robotics.json`, `Assets/StreamingAssets/Data/shelter_security_zones.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **23** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `airlock_security` |
| `campaign` |
| `campaign_day` |
| `chem_warfare` |
| `contraband_stash` |
| `expanded_shelter` |
| `narcotics` |
| `nuclear_core_lifecycle` |
| `robotics` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **27** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--expedition-panel-lifecycle` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
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
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/confession_secrets.json` |
| `Assets/StreamingAssets/Data/damaged_map_zones.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/maritime_zones.json` |
| `Assets/StreamingAssets/Data/narcotics.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (197 files, 1502 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Codex` | 2 | 29 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `Lifecycle` | 1 | 5 |
| `Maritime` | 1 | 6 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 1502 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `Codex`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **395**
(33 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **36**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `aquaponics` | no |
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `aquaponics_fry_survival` |
| `black_market_bounty` |
| `black_market_debt_event` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **71**
(CODEX_ONLY 34, GAMEPLAY_CONSUMED 23, OPTIONAL 2, UNRESOLVED 12).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `codex_entries.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `confession_secrets.json` | OPTIONAL |
| `damaged_map_zones.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |

**Verdict:** 12 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 36 (laddered 0) · RNG streams 15 · host files 24 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NARCOTICS-TRUTH-215
wave: 16
status: PROPOSED — foreman claim required
packages: NCT-215A, NCT-215B, NCT-215C, NCT-215D, NCT-215E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/barter_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/bunker_graffiti_postings.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --campaign-journey-selftest
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
