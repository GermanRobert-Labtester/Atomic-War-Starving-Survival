# PLAN-FACTION-BRANCH-STATUS-TRUTH-228 — Military, Rebel & Independent Branches

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FACTION-BRANCH-TRUTH-171, PLAN-WARLORDS-DIPLOMACY-29, PLAN-MUSTER-COALITION-TRUTH-130, PLAN-BASE-DEFENSE-RAIDS-61.
**Non-goals:** no branch lineage/merger (Plan 171), no diplomacy model (Plan 29),
no muster camp (Plan 130).

## 1. Outcome
Three reachable branch systems are unaddressed: `MilitaryBranchSystem.cs`
(**271**), `RebelBranchSystem.cs` (**255**), `IndependentBranchSystem.cs`
(**311**). Plan 171 owns split/merge lineage; each branch **type** still needs
its own status semantics: how a military branch mobilizes, how a rebel branch
recruits and hides, how independents trade passage.

| Deliverable | Detail |
|---|---|
| Branch-state model | per type: strength band, posture, and posture costs, reading Plan 29 relations |
| Mobilization/recruitment | military draft and rebel recruit paths consume population/roster through existing owners; no private heads count |
| Independent posture | independents trade passage/trade via Plan 96/151 rows; hostile posture routes to Plan 61 |
| Transitions | status changes on documented triggers with hysteresis; no oscillation |
| Save truth | branch status restores; no re-roll on load |

## 2. Evidence
- The three files above (unaddressed — Wave 17 audit); Plan 171 owns their lineage.
- Plan 29 owns relations; Plan 96 value rows for trade.
- Plan 61 consumes hostile posture; Plan 101 roster for drafts.
- Plan 130 may consume branch strength at the muster — boundary stated.

## 3. Packages
- **FBT-228A** branch-state model + band table.
- **FBT-228B** draft/recruit path tests (no private heads count).
- **FBT-228C** independent trade/hostility routing tests.
- **FBT-228D** transition/hysteresis fixtures.
- **FBT-228E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Strength derives from roster/population owners; trade flows through Plan 96.
- Hostile posture reaches Plan 61; status transitions hold hysteresis.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/`.

## 5. Risks
Duplicate force counts → owners only; a fixture asserts no private headcount.
Branch-type sprawl → the three types are the closed set for this plan.

---

## 6. Expanded census (16 files · 2,679 lines)

Scope: `Assets/Ashfall.Core/Factions/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 3 · DTO/Type 6 · Save 3 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FactionBranchCoordinator.cs` | 668 | System | — | 0 | 0 | 4 |
| `IndependentBranchCatalog.cs` | 95 | Catalog | — | 0 | 0 | 0 |
| `IndependentBranchIds.cs` | 162 | DTO/Type | — | 0 | 0 | 0 |
| `IndependentBranchSave.cs` | 76 | Save | — | 0 | 0 | 4 |
| `IndependentBranchState.cs` | 70 | DTO/Type | — | 0 | 0 | 0 |
| `IndependentBranchSystem.cs` | 311 | System | **yes** | 0 | 0 | 2 |
| `MilitaryBranchCatalog.cs` | 85 | Catalog | — | 0 | 0 | 0 |
| `MilitaryBranchIds.cs` | 153 | DTO/Type | — | 0 | 0 | 0 |
| `MilitaryBranchSave.cs` | 84 | Save | — | 0 | 0 | 5 |
| `MilitaryBranchState.cs` | 73 | DTO/Type | — | 0 | 0 | 0 |
| `MilitaryBranchSystem.cs` | 271 | System | **yes** | 0 | 0 | 2 |
| `RebelBranchCatalog.cs` | 84 | Catalog | — | 0 | 0 | 0 |
| `RebelBranchIds.cs` | 144 | DTO/Type | — | 0 | 0 | 0 |
| `RebelBranchSave.cs` | 85 | Save | — | 0 | 0 | 5 |
| `RebelBranchState.cs` | 63 | DTO/Type | — | 0 | 0 | 0 |
| `RebelBranchSystem.cs` | 255 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 7 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `quests_faction_branching.json` | object[2 keys] |
| `quests_moral_branching_expansion.json` | object[2 keys] |
| `independent_faction_branch.json` | object[3 keys] |
| `military_faction_branch.json` | object[3 keys] |
| `moral_choice_quests_branching.json` | object[2 keys] |
| `rebel_faction_branch.json` | object[3 keys] |

**State surfaces:** `FactionBranchCoordinator.cs`, `IndependentBranchSave.cs`, `IndependentBranchSystem.cs`, `MilitaryBranchSave.cs`, `MilitaryBranchSystem.cs`, `RebelBranchSave.cs`, `RebelBranchSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Factions/` |
| Test references | 70 name references across the test tree |
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

Domain files: 16. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FACTION-BRANCH-TRUTH-171` | 16 |
| `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 16 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FBT-228A` | `IndependentBranchState.cs`, `MilitaryBranchState.cs`, `RebelBranchState.cs` |
| `FBT-228B` | no name match — resolve at claim time |
| `FBT-228C` | `IndependentBranchCatalog.cs`, `IndependentBranchIds.cs`, `IndependentBranchSave.cs` |
| `FBT-228D` | no name match — resolve at claim time |
| `FBT-228E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 16; intra-domain edges: **35**; isolated files:
**0**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `FactionBranchCoordinator` | `IndependentBranchCatalog` |
| `FactionBranchCoordinator` | `IndependentBranchIds` |
| `FactionBranchCoordinator` | `IndependentBranchSystem` |
| `FactionBranchCoordinator` | `MilitaryBranchCatalog` |
| `FactionBranchCoordinator` | `MilitaryBranchIds` |
| `FactionBranchCoordinator` | `MilitaryBranchSystem` |
| `FactionBranchCoordinator` | `RebelBranchCatalog` |
| `FactionBranchCoordinator` | `RebelBranchIds` |
| `FactionBranchCoordinator` | `RebelBranchSystem` |
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
| `IndependentBranchSystem` | `RebelBranchIds` |
| `IndependentBranchSystem` | `RebelBranchSystem` |
| `MilitaryBranchIds` | `MilitaryBranchSystem` |
| `MilitaryBranchSave` | `MilitaryBranchSystem` |
| `MilitaryBranchState` | `MilitaryBranchIds` |
| `MilitaryBranchState` | `MilitaryBranchSystem` |
| `MilitaryBranchSystem` | `MilitaryBranchCatalog` |
| `MilitaryBranchSystem` | `MilitaryBranchIds` |
| `RebelBranchIds` | `MilitaryBranchIds` |
| `RebelBranchSave` | `MilitaryBranchSave` |
| `RebelBranchSave` | `RebelBranchSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MilitaryBranchSystem` | 7 |
| `MilitaryBranchIds` | 6 |
| `RebelBranchIds` | 5 |
| `RebelBranchSystem` | 5 |
| `IndependentBranchSystem` | 3 |
| `IndependentBranchCatalog` | 2 |
| `IndependentBranchIds` | 2 |
| `MilitaryBranchCatalog` | 2 |
| `RebelBranchCatalog` | 2 |
| `MilitaryBranchSave` | 1 |

**Class split:** hub 6 · sink 4 · source 6 · isolated 0.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 16. Host files: **3** · Test files: **15** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/FactionBranchHostSession.cs`, `src/UI/FactionsPanel.cs`, `src/UI/QuestsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs`, `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`, `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs`, `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`, `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `faction_espionage` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **9**.

| Event | First declaration |
|---|---|
| `OnBranchDecided` | `Assets/Ashfall.Core/Survivors/MoralBranchingSystem.cs` |
| `OnContractorStatusChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnOfferStatusChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnStatusGained` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnStatusLost` | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` |
| `OnTreatyStatusChanged` | `Assets/Ashfall.Core/RegionalTreatySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |
| `Assets/StreamingAssets/Data/faction_war_location_overrides.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |

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

Host files (`src/`) whose names share a domain token: **15**
(7 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Main.FactionBranch.cs` |
| `src/Muster/FactionActionPanel.cs` |
| `src/Radio/FactionRadioHudPanel.cs` |
| `src/Radio/FactionRadioSelfTest.cs` |
| `src/UI/AshfallStatusRail.cs` |
| `src/UI/FactionCommuniqueBoardPanel.cs` |
| `src/UI/FactionCultureCodexPanel.cs` |
| `src/UI/FactionDetailPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `faction_espionage` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **20**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 13, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |
| `faction_war_location_overrides.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `foundry_faction.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 18 · catalogs 22 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FACTION-BRANCH-STATUS-TRUTH-228
wave: 17
status: PROPOSED — foreman claim required
packages: FBT-228A, FBT-228B, FBT-228C, FBT-228D, FBT-228E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/FactionBranchHostSession.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/HostCli.FactionCommuniqueSelfTests.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/faction_combat_thresholds.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/faction_intelligence.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --faction-communique-board-selftest
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
