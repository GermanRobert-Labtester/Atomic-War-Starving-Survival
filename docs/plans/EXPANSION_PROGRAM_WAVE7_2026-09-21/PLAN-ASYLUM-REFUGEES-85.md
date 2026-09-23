# PLAN-ASYLUM-REFUGEES-85 — Sanctuary, Arrivals, Integration & Turning People Away

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-POLITICS-69, PLAN-NOMADS-CARAVAN-CULTURE-82,
PLAN-PANDEMIC-PUBLIC-HEALTH-47.
**Expanded appendix:** [`PLAN-ASYLUM-REFUGEES-85_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-ASYLUM-REFUGEES-85_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's asylum & arrivals systems.

**Non-goals:** no real migration or humanitarian politics; fictional peoples
and settlements; no dehumanising framing — arrivals are individuals with names,
needs, and agency.

## Outcome
Arrivals exist in fragments: `Visitors/VisitorIntegrationSystem.cs` (orphan),
`RecruitmentSystem` (orphan), migration engines (orphans), `SettlementCatalog`
(12 settlements), `crossing_factions`, rumor/briefing systems, survivor
enrichment (`DEC-32`), and the cohort/roster owners. This plan makes the
shelter's **door policy** a first-class decision with consequences.

| Decision | Options | Consequence |
|---|---|---|
| Admit | full member, guest, work-for-stay | population, food, skills, morale |
| Screen | health, intent, ties | disease risk, spies (Plan 41), rejection |
| House | quarters, temporary, camp | crowding, cohesion, cost |
| Integrate | work, school, faith, bonds | loyalty, skills, friction |
| Refuse | turn away, barter only | reputation, desperation events, guilt |
| Protect | escort, asylum from factions | faction friction, standing, raids |
| Lose | they leave, die, betray | consequences, memorial, escalation |

## Evidence
- Core: `Visitors/VisitorIntegrationSystem.cs` (orphan, 5 tests), `Survivors/RecruitmentSystem.cs` (orphan, 5), `Economy/SeasonalHumanMigrationEngine` + `MigrationConsequenceEngine` (orphans), `CohortSystem`, `SurvivorEnrichmentService` (DEC-32), `SettlementCatalog`, `Reputation/ShelterReputation*`.
- Data: `settlements.json` (12), `crossing_factions.json`, `faction_lore.json`, `family_name_templates.json`, `rumor_hubs.json`.
- Sealed prior: Plan 43 settlements, Plan 120 crossing factions, Plan 148 ideology friction, `DEC-32` origin modifiers, Plan 207 reputation.
- Contracts: one roster (cohort/survivor owners); screening uses disease/medical; espionage uses Plan 41; no duplicate visitor store.

## Packages
- **AR-85A** arrival events: individuals/groups with authored dossiers; deterministic per seed; weather/route dependent.
- **AR-85B** screening: health check (disease exposure), background questions, intelligence cross-check with uncertainty.
- **AR-85C** admission policy: shelter policy (ties Plan 69) with capacity and resource math; guests vs members.
- **AR-85D** integration: quarters, work assignment, school/belief paths, bond formation; friction via relations.
- **AR-85E** refusal/asylum: turning people away has reputation and narrative cost; asylum from factions triggers diplomacy.
- **AR-85F** outcomes: members gain skills/traits; betrayal/spy path uses hidden agendas; departure/death recorded.
- **AR-85G** content volumes: +10 arrival dossiers, +8 screening events, +6 policy rows, +8 integration beats; fictional.

## Acceptance & verification
- Admission math is visible before deciding; arrivals never duplicate roster entries; spies route to hidden agendas; determinism.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Visitors/`; survivors suites; `godot --headless --path . -- --survivors-selftest`; `--infection-*` as applicable.

## Risks
Roster bloat → capacity, food, and cohesion limit growth; departures possible; narrative value over numbers.

---

## 6. Expanded census (1 files · 611 lines)

Scope: `Assets/Ashfall.Core/Visitors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `VisitorIntegrationSystem.cs` | 611 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `visitor_templates.json` | object[2 keys] |

**State surfaces:** `VisitorIntegrationSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Visitors/` |
| Test references | 1 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AR-85A` | no name match — resolve at claim time |
| `AR-85B` | no name match — resolve at claim time |
| `AR-85C` | no name match — resolve at claim time |
| `AR-85D` | `VisitorIntegrationSystem.cs` |
| `AR-85E` | no name match — resolve at claim time |
| `AR-85F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 9. Host files: **9** · Test files: **17** · Data files: **42**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.WastelandInhabitants.cs`, `src/Host/HostCli.cs` |
| Tests (`Ashfall.Core.Tests/`) | 17 | `Ashfall.Core.Tests/DutyRosterIntegrationTests.cs`, `Ashfall.Core.Tests/FactionDisplayNameCatalogTests.cs`, `Ashfall.Core.Tests/FactionIconCatalogTests.cs`, `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`, `Ashfall.Core.Tests/InformationFlow/Plan131RumorNetworkIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 42 | `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/door_encounters.json`, `Assets/StreamingAssets/Data/economy_goods.json`, `Assets/StreamingAssets/Data/faction_lore.json`, `Assets/StreamingAssets/Data/field_guide.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `contractor_roster` |
| `crossing` |
| `duty_roster` |
| `economy` |
| `encounters` |
| `faction_espionage` |
| `factions` |
| `field_guide` |
| `oral_lore` |
| `piezometer_network` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--crossing-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--rumor-network-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnNameErased` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnNameRecited` | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` |
| `OnNameWritten` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/confession_secrets.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (86 files, 604 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Communication` | 3 | 17 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Integration` | 16 | 74 |
| `Quests` | 4 | 25 |
| `Settlements` | 1 | 5 |

**Verdict:** 604 cases sit under matching regions — run those first (`Audio`, `Balance`, `Communication`, `DutyRoster`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **85**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/AssetRegistry.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **23**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `communication` | no |
| `contractor_roster` | no |
| `crossing` | no |
| `duty_roster` | no |
| `dynamic_quests` | no |
| `economy` | no |
| `encounters` | no |
| `expedition` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `acoustic_detection` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `duty_roster` |
| `economy` |
| `expedition` |
| `world_evolution` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **117**
(CODEX_ONLY 29, GAMEPLAY_CONSUMED 61, OPTIONAL 5, UNRESOLVED 22).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `confession_secrets.json` | OPTIONAL |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 22 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **6**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_broken_compact_locked` |
| `flag_branch_iron_way_locked` |
| `flag_branch_listener_locked` |
| `flag_branch_mercy_road_locked` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 23 (laddered 0) · RNG streams 8 · host files 26 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ASYLUM-REFUGEES-85
wave: 7
status: PROPOSED — foreman claim required
packages: AR-85A, AR-85B, AR-85C, AR-85D, AR-85E, AR-85F, AR-85G
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/AssetCoverageReport.cs  # §19 candidate host surface
  - src/Host/AssetCoverageScanner.cs  # §19 candidate host surface
  - src/Host/AssetRegistry.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --asset-coverage-report
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
