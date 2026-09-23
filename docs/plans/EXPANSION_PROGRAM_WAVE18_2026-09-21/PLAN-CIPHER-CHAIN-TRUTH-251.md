# PLAN-CIPHER-CHAIN-TRUTH-251 — Puzzle Chains: Clues, Solutions & Failure Paths

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DOCUMENT-DISCOVERY-TRUTH-192, PLAN-NARRATIVE-GRAPH-18, PLAN-BLACK-PROJECTS-TRUTH-205.
**Non-goals:** no document discovery (Plan 192), no graph storage (Plan 18), no
sealed records (Plan 205).

## 1. Outcome
`Narrative/CipherQuestChainEngine.cs` (**192 lines**) is reachable and
unaddressed: multi-step puzzles where clues combine into a solution. Puzzle
chains are notorious for unsolvable states and brute-force guessing; the
contract must state clue independence, solution evaluation, and failure paths.

| Deliverable | Detail |
|---|---|
| Clue model | clues with sources (Plan 192 documents, Plan 205 records, sites) and a documented combination rule |
| Solution evaluation | solutions checked against the stored puzzle definition; partial answers give documented feedback, not silence |
| Failure paths | wrong attempts have bounded costs; a chain cannot be permanently bricked (a reset/alternative path exists) |
| Guessing resistance | the engine documents how brute force is discouraged (attempt costs/time), never a hidden timer |
| Save truth | clue set and attempt state restore; no re-roll or reset on load |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/CipherQuestChainEngine.cs` (192 lines; unaddressed — Wave 18 audit).
- Plan 192 supplies document clues; Plan 205 sealed records; Plan 18 stores flags.
- Plan 138 carries attempt feedback.

## 3. Packages
- **CCT-251A** clue model + combination rule.
- **CCT-251B** solution evaluation + partial-feedback tests.
- **CCT-251C** failure/brick-resistance fixtures.
- **CCT-251D** brute-force cost rule tests.
- **CCT-251E** save round-trip; no reset on load.

## 4. Acceptance & verification
- Every chain has at least one documented solution path to a terminal state.
- Attempt costs apply per rule; save/load preserves clue/attempt state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Bricked chains → reset/alternative path is a fixture.
Guessing bypass → documented attempt costs, no hidden timers.

---

## 6. Expanded census (1 files · 192 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CipherQuestChainEngine.cs` | 192 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `numbers_station_ciphers.json` | array[11] |

**State surfaces:** `CipherQuestChainEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
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

Domain files: 1. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-GRAPH-18` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CCT-251A` | no name match — resolve at claim time |
| `CCT-251B` | no name match — resolve at claim time |
| `CCT-251C` | no name match — resolve at claim time |
| `CCT-251D` | no name match — resolve at claim time |
| `CCT-251E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.WorldExploration.cs`, `src/UI/TroposphericRadioRelayPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Combat/Plan10_11CombatExplorationIntegrationTests.cs`, `Ashfall.Core.Tests/World/Plan11ExplorationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `combat` |
| `expansion_quest` |
| `radio` |
| `radio_program_production` |
| `radio_station` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--personal-quest-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnStationStateChanged` | `Assets/Ashfall.Core/WeatherStationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (134 files, 982 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Exploration` | 2 | 14 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `PlayerCommand` | 1 | 1 |
| `Quests` | 4 | 25 |
| `Radio` | 47 | 354 |

**Verdict:** 982 cases sit under matching regions — run those first (`Audio`, `Combat`, `DutyRoster`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **307**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/ChemicalDependencyHostSession.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/ChemicalDependencySaveStore.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/ContractorRosterHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `combat` | no |
| `contractor_roster` | no |
| `duty_roster` | no |
| `dynamic_quests` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `duty_roster` |
| `economy` |
| `mineral_chemical` |
| `radio` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **113**
(CODEX_ONLY 23, GAMEPLAY_CONSUMED 66, OPTIONAL 4, UNRESOLVED 20).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 20 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 22 (laddered 1) · RNG streams 9 · host files 23 · catalogs 22 · test regions 10 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CIPHER-CHAIN-TRUTH-251
wave: 18
status: PROPOSED — foreman claim required
packages: CCT-251A, CCT-251B, CCT-251C, CCT-251D, CCT-251E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencyHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/chemical_dependency_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/chemical_syntheses.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --combat-breaching-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
