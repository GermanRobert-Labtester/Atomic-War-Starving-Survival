# PLAN-YEAR-OF-ASH-TRUTH-146 — Season Timeline, Environmental Arcs & Faction War Chains

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-TEMPORAL-AUTHORITY-33, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Implementation scaffold:** [`PLAN-YEAR-OF-ASH-TRUTH-146_APPENDIX-A_SCAFFOLD.md`](PLAN-YEAR-OF-ASH-TRUTH-146_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-WEATHER-ATMOSPHERE-28` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no second calendar (Plan 33 owns the clock), no weather simulation
(Plan 28), no new quest authoring.

## 1. Outcome
`YearOfAsh/` is a fifteen-file domain with almost no plan coverage:
`QuestlineSystem`, `YearOfAshTimelineSystem`, `YearOfAshSave`,
`YearOfAshStormCatalog`, `YearOfAshDeepFreezeSystem`, `YearOfAshIceRoadSystem`,
`YearOfAshRadonSystem`, `FactionWarSystem`/`FactionWarChainRunner`/`FactionWarContentCatalog`,
`DoorEncounterSystem`(+Loader), `BuiltInQuestlineCatalog`,
`DynamicQuestlineCatalogLoader`, `YearOfAshCatalogLoader`. The save registry
already versions the section (`year_of_ash` at ladder 4 in `SchemaVersions`).
Nothing states how the timeline, the environmental arcs, and the war chains
interlock — or what the day-365/end-of-year state must be.

| Deliverable | Detail |
|---|---|
| Timeline model | the year's phases and their day boundaries on the canonical clock; one owner per phase transition |
| Environmental arcs | storm, deep freeze, ice road, radon each define onset/offset conditions, effects on existing owners, and their catalogs |
| War chain integrity | `FactionWarChainRunner` stages gate on world state; content catalog ids resolve; a stalled chain has a documented fallback |
| Door encounters | `DoorEncounterSystem` + whitelist (Plan 155) read world state; encounter selection is seeded and save-stable |
| Save truth | `YearOfAshSave` round-trips all four arcs + chain progress at ladder 4; a load resumes mid-arc exactly |

## 2. Evidence
- `Assets/Ashfall.Core/YearOfAsh/` fifteen-file listing (verified file names).
- `SaveSectionRegistry.SchemaVersions`: `year_of_ash` → 4 (Plan 87 Appendix input) — this domain already owns a versioned ladder.
- Plan 28 supplies weather state the arcs consume; Plan 33 supplies the clock.
- Plan 155 owns the knock whitelist this domain's door encounters use.

## 3. Packages
- **YAT-146A** timeline model + phase boundary table.
- **YAT-146B** arc contracts (onset/offset/effects) + one scripted arc test each.
- **YAT-146C** war chain stage gates + catalog resolution + stall fallback.
- **YAT-146D** door encounter selection determinism + whitelist integration.
- **YAT-146E** ladder-4 save round-trip mid-arc.

## 4. Acceptance & verification
- Phase transitions occur exactly at their day boundaries; no double transition across save/load.
- Each arc's effects are visible in the named owners and stop at offset.
- Chain stages cannot skip; a stalled chain reaches the documented fallback.
- `bash scripts/run_test.sh Ashfall.Core.Tests/YearOfAsh/`.

## 5. Risks
Domain size → packages are per-arc so each stays reviewable.
Calendar duplication → all day math reads Plan 33's clock; the phase test uses game days only.

---

## 6. Expanded census (10 files · 2,943 lines)

Scope: `Assets/Ashfall.Core/YearOfAsh/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Loader 1 · Save 1 · Support 1 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `FactionWarChainRunner.cs` | 604 | Support | — | 0 | 0 | 2 |
| `FactionWarContentCatalog.cs` | 452 | Catalog | — | 0 | 0 | 0 |
| `FactionWarSystem.cs` | 338 | System | — | 0 | 0 | 2 |
| `YearOfAshCatalogLoader.cs` | 448 | Loader | — | 0 | 0 | 0 |
| `YearOfAshDeepFreezeSystem.cs` | 132 | System | — | 0 | 0 | 2 |
| `YearOfAshIceRoadSystem.cs` | 129 | System | — | 0 | 0 | 2 |
| `YearOfAshRadonSystem.cs` | 144 | System | — | 0 | 0 | 2 |
| `YearOfAshSave.cs` | 372 | Save | **yes** | 0 | 0 | 21 |
| `YearOfAshStormCatalog.cs` | 133 | Catalog | — | 0 | 0 | 0 |
| `YearOfAshTimelineSystem.cs` | 191 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 7 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `apprenticeship_catalog.json` | object[2 keys] |
| `journal_voice_prose.json` | object[2 keys] |
| `moral_choice_faction_reactions.json` | object[3 keys] |
| `year_of_ash_storm_windows.json` | array[14] |
| `moral_choice_chains.json` | object[9 keys] |
| `moral_choice_flags.json` | object[3 keys] |

**State surfaces:** `FactionWarChainRunner.cs`, `FactionWarSystem.cs`, `YearOfAshDeepFreezeSystem.cs`, `YearOfAshIceRoadSystem.cs`, `YearOfAshRadonSystem.cs`, `YearOfAshSave.cs`, `YearOfAshTimelineSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/YearOfAsh/` (create if absent) |
| Test references | 81 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 15 domain files: **42 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `BuiltInQuestlineCatalog.cs` | 1295 | 1 | 7 |
| `FactionWarChainRunner.cs` | 604 | 3 | 5 |
| `FactionWarContentCatalog.cs` | 452 | 4 | 2 |
| `YearOfAshCatalogLoader.cs` | 448 | 1 | 4 |
| `QuestlineSystem.cs` | 433 | 14 | 1 |
| `DoorEncounterSystem.cs` | 401 | 4 | 0 |
| `YearOfAshSave.cs` | 372 | 0 | 16 |
| `FactionWarSystem.cs` | 338 | 5 | 0 |
| `YearOfAshTimelineSystem.cs` | 191 | 3 | 0 |
| `YearOfAshRadonSystem.cs` | 144 | 2 | 0 |
| `YearOfAshStormCatalog.cs` | 133 | 1 | 0 |
| `YearOfAshDeepFreezeSystem.cs` | 132 | 2 | 0 |

**Highest-coupling files (in×2 + out):**

- `QuestlineSystem.cs` — in 14, out 1
- `YearOfAshSave.cs` — in 0, out 16
- `FactionWarChainRunner.cs` — in 3, out 5
- `FactionWarContentCatalog.cs` — in 4, out 2
- `FactionWarSystem.cs` — in 5, out 0
- `BuiltInQuestlineCatalog.cs` — in 1, out 7
- `DoorEncounterSystem.cs` — in 4, out 0
- `YearOfAshCatalogLoader.cs` — in 1, out 4
- `YearOfAshIceRoadSystem.cs` — in 2, out 2
- `YearOfAshTimelineSystem.cs` — in 3, out 0

**Ordering implication:** wire in-dependent files first (high in-degree, low
out-degree), then the terminal consumers. A file with many outgoing edges is a
dependency: it should be sealed or verified before its dependents claim work.

---

## 12. Cross-plan coupling

Domain files: 12. Other plans referencing their names: **5**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |
| `PLAN-TEMPORAL-AUTHORITY-33` | 1 |
| `PLAN-KNOCK-WHITELIST-TRUTH-155` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `YAT-146A` | `YearOfAshTimelineSystem.cs` |
| `YAT-146B` | no name match — resolve at claim time |
| `YAT-146C` | `DoorEncounterCatalogLoader.cs`, `FactionWarChainRunner.cs`, `FactionWarContentCatalog.cs` |
| `YAT-146D` | `DoorEncounterCatalogLoader.cs`, `DoorEncounterSystem.cs` |
| `YAT-146E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 14. Host files: **19** · Test files: **71** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 19 | `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/DoseLedgerHostSession.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/ShelterThermalHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 71 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/CaravanPatrolIntegrationTests.cs`, `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `deep_well` |
| `encounter_choice` |
| `faction_espionage` |
| `year_of_ash` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-selftest` |
| `--ice-road-tick-demo` |
| `--patrol-encounter-selftest` |
| `--travel-encounter-selftest` |
| `--year-of-ash-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **22**.

| Event | First declaration |
|---|---|
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFreezeAlarmTriggered` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshDeepFreezeSystem.cs` |
| `OnIceRoadClosed` | `Assets/Ashfall.Core/IceRoadSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/door_encounters.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |
| `Assets/StreamingAssets/Data/faction_war_journal.json` |

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

Host files (`src/`) whose names share a domain token: **33**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |
| `src/Host/NarrativeQuestlineSaveStore.cs` |
| `src/Host/TravelEncounterSaveStore.cs` |
| `src/Main.DeepWell.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `deep_well` | no |
| `encounter_choice` | no |
| `faction_espionage` | no |
| `year_of_ash` | yes |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `deep_coast` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **34**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 23, OPTIONAL 1, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `door_encounters.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_mercy_road_locked` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 4 (laddered 1) · RNG streams 1 · host files 16 · catalogs 22 · test regions 0 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-YEAR-OF-ASH-TRUTH-146
wave: 12
status: PROPOSED — foreman claim required
packages: YAT-146A, YAT-146B, YAT-146C, YAT-146D, YAT-146E
claim paths:
  - src/Host/DeepCoastHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellHostSession.cs  # §19 candidate host surface
  - src/Host/DeepWellSaveStore.cs  # §19 candidate host surface
  - src/Host/EncounterChoiceSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/deep_lore_locations.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --deep-coast-host-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
