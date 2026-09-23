# PLAN-MORALE-UNREST-TRUTH-129 — Morale Marks, Unrest Thresholds & Grievance Records

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-RECREATION-MORALE-50, PLAN-MENTAL-HEALTH-THERAPY-64.
**Non-goals:** no second mood model (Plan 64 owns individual state), no policy
system (Plan 69), no recreation content (Plan 50).

## 1. Outcome
`DutyRoster/MoraleMarkSystem.cs` exists inside the roster subsystem, Plan 50
owns recreation, Plan 69 owns politics, and Plan 64 owns individual
psychology. A **holdfast-level morale/unrest** record — marks that accumulate
from shared grievances (unfair rosters, deaths, shortages) and thresholds that
trigger collective consequences — has no stated owner or contract.

| Deliverable | Detail |
|---|---|
| Mark model | mark types (grievance, celebration, fear) with sources; each source is an existing system's fact |
| Unrest thresholds | documented bands (settled → strained → unrest) and what each band changes at the collective owner |
| Individual hand-off | individual effects enter Plan 64's model; collective effects stay at the holdfast owner (Plan 69's surface) |
| Records | a grievance record (who/what/when) persists and is visible; no anonymous counter |
| Decay truth | marks expire or fade by documented rule on the canonical clock, not wall time |

## 2. Evidence
- `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` exists (verified in the DutyRoster listing).
- Plan 64 owns individual psychological state; this plan feeds it.
- Plan 69 owns collective/political surfaces; the unrest band is presented there.
- Plan 50 owns recreation as a mark source (celebration), not a modifier store.

## 3. Packages
- **MUT-129A** mark model + source table.
- **MUT-129B** threshold bands + collective effect table at their owners.
- **MUT-129C** individual typed hand-off to Plan 64.
- **MUT-129D** grievance records + persistence round-trip.
- **MUT-129E** decay rule on the canonical clock + catch-up test.

## 4. Acceptance & verification
- Every mark traces to a real fact source; no panel-created marks.
- Threshold crossing produces the documented collective effect once, not repeatedly per tick.
- Save/load preserves marks and records; catch-up fades correctly.
- `bash scripts/run_test.sh` on the roster/shelter regions.

## 5. Risks
Mood duplication → individual state stays in Plan 64; the boundary is tested.
Threshold oscillation → crossing requires hysteresis documented in the band table.

---

## 6. Expanded census (1 files · 176 lines)

Scope: `Assets/Ashfall.Core/DutyRoster/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `MoraleMarkSystem.cs` | 176 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** `MoraleMarkSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/DutyRoster/` |
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

Domain method: plan-body artifact list.
Governed artifacts: 3. Other plans referencing them: **4**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 1 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-RECREATION-MORALE-50` | 1 |
| `PLAN-DUTY-ROSTER-TRUTH-101` | 1 |

**Governed artifacts (first 12):**

| Path |
|---|
| `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `DutyRoster/MoraleMarkSystem.cs` |
| `MoraleMarkSystem.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `MUT-129A` | no name match — resolve at claim time |
| `MUT-129B` | no name match — resolve at claim time |
| `MUT-129C` | no name match — resolve at claim time |
| `MUT-129D` | no name match — resolve at claim time |
| `MUT-129E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/DutyRosterHostSession.cs`, `src/UI/KitchenNutritionPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/Collectibles/CollectibleVinylIntegrationTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterHostSessionTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSaveStoreTests.cs`, `Ashfall.Core.Tests/DutyRoster/DutyRosterSeasonCatalogTests.cs`, `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `contractor_roster` |
| `duty_roster` |
| `kitchen_nutrition` |
| `morale` |
| `morale_contagion` |
| `nutrition` |
| `vinyl_morale` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnKitchenChanged` | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` |
| `OnMarkCleared` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMarkSet` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMoraleApplied` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnMoraleDelta` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` |
| `OnMoraleDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMoraleDrainRequested` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnPermanentMoraleBuffApplied` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |
| `OnRosterBurned` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnRosterChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnRosterUpdated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/final_wishes.json` |
| `Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json` |
| `Assets/StreamingAssets/Data/narrative/culinary_ration_batch_2.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (136 files, 946 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Kitchen` | 2 | 10 |
| `Nutrition` | 1 | 5 |
| `Progression` | 11 | 83 |

**Verdict:** 946 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **553**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **30**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `black_market` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **115**
(CODEX_ONLY 51, GAMEPLAY_CONSUMED 48, OPTIONAL 4, UNRESOLVED 12).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 12 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 4
**Surface:** save sections 30 (laddered 0) · RNG streams 15 · host files 22 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MORALE-UNREST-TRUTH-129
wave: 10
status: PROPOSED — foreman claim required
packages: MUT-129A, MUT-129B, MUT-129C, MUT-129D, MUT-129E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --duty-roster-loop-selftest
dependencies:
  - coordinate: 4 other plan(s) name these artifacts (§12)
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
