# PLAN-FACTIONS-STATE-FAMILY-TRUTH-268 — Branch State, Saves & Consequence Routers

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FACTION-BRANCH-TRUTH-171, PLAN-FACTION-BRANCH-STATUS-TRUTH-228, PLAN-INTERNAL-SECURITY-TRUTH-224.
**Non-goals:** no diplomacy model; the family is audited for state/save/router
wiring.

## 1. Outcome
**23 `Factions/` files** are referenced by no plan: state types
(`CounterIntelligenceState`), routers (`EspionageConsequenceRouter`), catalogs
(`FactionDisplayNameCatalog`, `FactionIntelligenceCatalog`,
`IndependentBranchCatalog`), ids/saves (`IndependentBranchIds`,
`IndependentBranchSave`), and standing resolvers
(`FactionStandingIdResolver`). State/save pairs must agree with their systems'
records (Plans 171/228).

| Deliverable | Detail |
|---|---|
| State/save agreement | each `*Save`/`*State` type matches its system's capture/restore (Plan 1 Appendix AC pattern) |
| Router audit | consequence routers write only through named owners (Plans 29/224) |
| Catalog resolution | display/intelligence/branch catalogs resolve through loaders |
| Id integrity | id resolvers map to catalog ids (Plan 34 family) |
| Test presence | fixtures for state round-trip and routers |

## 2. Evidence
- 23 `Factions/` basenames absent from every plan body (Wave 19 file-level audit).
- Plans 171/228/224 own the systems; this plan verifies their data plumbing.
- Plan 1 Appendix AC's signature census lists related save types.

## 3. Packages
- **FSF-268A** state/save agreement table + round-trip tests.
- **FSF-268B** router write-path audit.
- **FSF-268C** catalog→loader tests.
- **FSF-268D** id resolver coverage.

## 4. Acceptance & verification
- Save types round-trip with their systems; routers never write foreign state; ids resolve.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/`.

## 5. Risks
Save drift → round-trip fixtures per type.
Router leakage → write-path audit with owners named.

---

## 6. Expanded census (36 files in scope · 7,241 lines · 0 still unmentioned)

Scope: files under `Assets/Ashfall.Core/Factions/`. The plan's premise was the family **at authoring time**; later
plans have since referenced some files, so the table marks each row
`still unmentioned` or `since-mentioned`. Both are in scope for this plan: the
since-mentioned files are re-verified for owner discipline, and the still-
unmentioned ones are the original audit target. Class distribution:
System 13 · DTO/Type 9 · Catalog 6 · Save 5 · Support 2 · Loader 1.

| File | Lines | Class | Banned | Empty catches | Capture/Restore | Status |
|---|---:|---|---:|---:|---:|---|
| `CounterIntelligenceState.cs` | 77 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `CounterIntelligenceSystem.cs` | 315 | System | 0 | 0 | 2 | since-mentioned |
| `EspionageConsequenceRouter.cs` | 120 | Support | 0 | 0 | 2 | since-mentioned |
| `EspionageSystem.cs` | 734 | System | 0 | 0 | 7 | since-mentioned |
| `FactionBountySystem.cs` | 254 | System | 0 | 0 | 2 | since-mentioned |
| `FactionBranchCoordinator.cs` | 668 | System | 0 | 0 | 4 | since-mentioned |
| `FactionCovertOpsCoordinator.cs` | 518 | System | 0 | 0 | 2 | since-mentioned |
| `FactionDisplayNameCatalog.cs` | 118 | Catalog | 0 | 0 | 0 | since-mentioned |
| `FactionIntelligenceCatalog.cs` | 57 | Catalog | 0 | 0 | 0 | since-mentioned |
| `FactionStandingIdResolver.cs` | 131 | Support | 0 | 0 | 0 | since-mentioned |
| `ForcedLaborSystem.cs` | 401 | System | 0 | 0 | 2 | since-mentioned |
| `IndependentBranchCatalog.cs` | 95 | Catalog | 0 | 0 | 0 | since-mentioned |
| `IndependentBranchIds.cs` | 162 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `IndependentBranchSave.cs` | 76 | Save | 0 | 0 | 4 | since-mentioned |
| `IndependentBranchState.cs` | 70 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `IndependentBranchSystem.cs` | 311 | System | 0 | 0 | 2 | since-mentioned |
| `InfiltratorCatalog.cs` | 52 | Catalog | 0 | 0 | 0 | since-mentioned |
| `InfiltratorCatalogLoader.cs` | 34 | Loader | 0 | 0 | 0 | since-mentioned |
| `MilitaryBranchCatalog.cs` | 85 | Catalog | 0 | 0 | 0 | since-mentioned |
| `MilitaryBranchIds.cs` | 153 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `MilitaryBranchSave.cs` | 84 | Save | 0 | 0 | 5 | since-mentioned |
| `MilitaryBranchState.cs` | 73 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `MilitaryBranchSystem.cs` | 271 | System | 0 | 0 | 2 | since-mentioned |
| `PrisonerSystem.cs` | 403 | System | 0 | 0 | 2 | since-mentioned |
| `PrpfIds.cs` | 30 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `PrpfSave.cs` | 76 | Save | 0 | 0 | 4 | since-mentioned |
| `PrpfStandingSystem.cs` | 204 | System | 0 | 0 | 2 | since-mentioned |
| `PrpfState.cs` | 57 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `RebelBranchCatalog.cs` | 84 | Catalog | 0 | 0 | 0 | since-mentioned |
| `RebelBranchIds.cs` | 144 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `RebelBranchSave.cs` | 85 | Save | 0 | 0 | 5 | since-mentioned |
| `RebelBranchState.cs` | 63 | DTO/Type | 0 | 0 | 0 | since-mentioned |
| `RebelBranchSystem.cs` | 255 | System | 0 | 0 | 2 | since-mentioned |
| `ShelterEspionageSystem.cs` | 341 | System | 0 | 0 | 2 | since-mentioned |
| `TerritoryControlSystem.cs` | 451 | System | 0 | 0 | 1 | since-mentioned |
| `WeightOfChoicesSave.cs` | 189 | Save | 0 | 0 | 10 | since-mentioned |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 19 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `holdfast_factions.json` | object[2 keys] |
| `crossing_factions.json` | object[2 keys] |
| `standing_record_factions.json` | object[2 keys] |

**State surfaces (capture/restore present):**

- `CounterIntelligenceSystem.cs`
- `EspionageConsequenceRouter.cs`
- `EspionageSystem.cs`
- `FactionBountySystem.cs`
- `FactionBranchCoordinator.cs`
- `FactionCovertOpsCoordinator.cs`
- `ForcedLaborSystem.cs`
- `IndependentBranchSave.cs`
- `IndependentBranchSystem.cs`
- `MilitaryBranchSave.cs`
- `MilitaryBranchSystem.cs`
- `PrisonerSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Factions/` |
| Files referenced by tests | 118 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Drift | 36 of 36 files became plan-referenced since authoring — re-verify their owners |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every file in scope; no edits.
2. Still-unmentioned files: the original audit target — consumer or ownerless verdict.
3. Since-mentioned files: confirm the new plan's claim actually owns them; no double ownership.
4. Catalogs and loaders: justify or report inert.
5. Systems and saves: one owner per state; keys per Plan 1 Appendix Q.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(36 files). Other plans referencing those names: **14**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-FACTION-BRANCH-TRUTH-171` | 16 |
| `PLAN-FACTION-BRANCH-STATUS-TRUTH-228` | 16 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 8 |
| `PLAN-INTERNAL-SECURITY-TRUTH-224` | 5 |
| `PLAN-ESPIONAGE-COUNTERINTEL-41` | 4 |
| `PLAN-ESPIONAGE-SYSTEM-TRUTH-161` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FSF-268A` | `CounterIntelligenceState.cs`, `IndependentBranchState.cs`, `MilitaryBranchState.cs` |
| `FSF-268B` | `EspionageConsequenceRouter.cs` |
| `FSF-268C` | `InfiltratorCatalogLoader.cs`, `FactionDisplayNameCatalog.cs`, `FactionIntelligenceCatalog.cs` |
| `FSF-268D` | `FactionStandingIdResolver.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 36; intra-domain edges: **61**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CounterIntelligenceState` | `CounterIntelligenceSystem` |
| `CounterIntelligenceSystem` | `CounterIntelligenceState` |
| `CounterIntelligenceSystem` | `InfiltratorCatalog` |
| `EspionageConsequenceRouter` | `EspionageSystem` |
| `EspionageSystem` | `EspionageConsequenceRouter` |
| `EspionageSystem` | `FactionStandingIdResolver` |
| `FactionBountySystem` | `FactionStandingIdResolver` |
| `FactionBranchCoordinator` | `IndependentBranchCatalog` |
| `FactionBranchCoordinator` | `IndependentBranchIds` |
| `FactionBranchCoordinator` | `IndependentBranchSystem` |
| `FactionBranchCoordinator` | `MilitaryBranchCatalog` |
| `FactionBranchCoordinator` | `MilitaryBranchIds` |
| `FactionBranchCoordinator` | `MilitaryBranchSystem` |
| `FactionBranchCoordinator` | `PrpfIds` |
| `FactionBranchCoordinator` | `PrpfStandingSystem` |
| `FactionBranchCoordinator` | `RebelBranchCatalog` |
| `FactionBranchCoordinator` | `RebelBranchIds` |
| `FactionBranchCoordinator` | `RebelBranchSystem` |
| `FactionBranchCoordinator` | `WeightOfChoicesSave` |
| `IndependentBranchIds` | `PrpfIds` |
| `IndependentBranchSave` | `IndependentBranchSystem` |
| `IndependentBranchState` | `IndependentBranchSystem` |
| `IndependentBranchState` | `MilitaryBranchIds` |
| `IndependentBranchState` | `MilitaryBranchSystem` |
| `IndependentBranchState` | `RebelBranchIds` |
| `IndependentBranchState` | `RebelBranchSystem` |
| `IndependentBranchSystem` | `IndependentBranchCatalog` |
| `IndependentBranchSystem` | `IndependentBranchIds` |
| `IndependentBranchSystem` | `MilitaryBranchIds` |
| `IndependentBranchSystem` | `MilitaryBranchSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MilitaryBranchSystem` | 9 |
| `RebelBranchSystem` | 7 |
| `MilitaryBranchIds` | 6 |
| `PrpfStandingSystem` | 6 |
| `RebelBranchIds` | 5 |
| `IndependentBranchSystem` | 4 |
| `PrpfIds` | 4 |
| `FactionStandingIdResolver` | 2 |
| `IndependentBranchCatalog` | 2 |
| `IndependentBranchIds` | 2 |

**Class split:** hub 15 · sink 6 · source 10 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 36. Host files: **20** · Test files: **40** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 20 | `src/Host/BlackMarketHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/CounterIntelligenceHostSession.cs`, `src/Host/CounterIntelligenceSaveStore.cs`, `src/Host/DefenseHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 40 | `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs`, `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Economy/LoanSharkEnforcerEngineTests.cs`, `Ashfall.Core.Tests/Economy/Plan211BlackMarketTests.cs`, `Ashfall.Core.Tests/Expeditions/PlanE1_29VehicleEspionageTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/black_market_inventory.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **22** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `counter_intelligence` |
| `espionage` |
| `expanded_shelter` |
| `faction_espionage` |
| `factions` |
| `forced_labor` |
| `prisoner_management` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
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

Events whose name shares a domain token: **22**.

| Event | First declaration |
|---|---|
| `OnBountyRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBountyRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnBranchDecided` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLaborDisputeChanged` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnLaborObligation` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnLaborObligationCreated` | `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs` |
| `OnLaborObligationDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/espionage_missions.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (99 files, 850 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Espionage` | 1 | 4 |
| `Factions` | 10 | 72 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 850 cases sit under matching regions — run those first (`Espionage`, `Factions`, `NarrativeConsequence`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **80**
(24 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/EspionageHostSession.cs` |
| `src/Host/EspionageSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/ForcedLaborSaveStore.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `counter_intelligence` | no |
| `espionage` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `factions` | no |
| `forced_labor` | no |
| `prisoner_management` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `black_market_bounty` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **36**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 24, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **7**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_broken_compact_locked` |
| `flag_branch_iron_way_locked` |
| `flag_branch_listener_locked` |
| `flag_branch_mercy_road_locked` |
| `flag_chosen_faction_side` |
| `flag_executed_prisoner` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 14
**Surface:** save sections 22 (laddered 1) · RNG streams 2 · host files 21 · catalogs 22 · test regions 4 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FACTIONS-STATE-FAMILY-TRUTH-268
wave: 19
status: PROPOSED — foreman claim required
packages: FSF-268A, FSF-268B, FSF-268C, FSF-268D
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bounty_board.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Espionage/
  - godot --headless --path . -- --faction-communique-board-selftest
dependencies:
  - coordinate: 14 other plan(s) name these artifacts (§12)
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
