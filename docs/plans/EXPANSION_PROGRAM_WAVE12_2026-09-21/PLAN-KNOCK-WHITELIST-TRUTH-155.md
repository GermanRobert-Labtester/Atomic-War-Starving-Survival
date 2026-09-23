# PLAN-KNOCK-WHITELIST-TRUTH-155 — Who May Arrive at the Door, When & Why

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-YEAR-OF-ASH-TRUTH-146, PLAN-ASYLUM-REFUGEES-85, PLAN-BASE-DEFENSE-RAIDS-61.
**Implementation scaffold:** [`PLAN-KNOCK-WHITELIST-TRUTH-155_APPENDIX-A_SCAFFOLD.md`](PLAN-KNOCK-WHITELIST-TRUTH-155_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-YEAR-OF-ASH-TRUTH-146` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no arrival content (Plan 85), no raid resolution (Plan 61), no
door-encounter authoring (Plan 146 owns the system).

## 1. Outcome
`Encounters/OrphanKnockWhitelist.cs` is a single-file gate that decides who may
arrive at the shelter door. It is a safety-critical surface: too permissive and
raids become free introductions; too strict and legitimate arrivals (refugees,
traders, messengers) never occur. No plan states its semantics.

| Deliverable | Detail |
|---|---|
| Whitelist model | categories (refugee, trader, messenger, neighbor, hostile-probe) each with an allowed time window and prerequisites |
| Context inputs | world state read from existing owners (threat level Plan 61, refuge pressure Plan 85, war chains Plan 146) |
| Selection | which eligible category arrives is seeded and day-based; no wall clock |
| Refusal consequences | a refused arrival has a documented outcome (leaves, waits, turns hostile) — never silently vanishes |
| Persistence | pending arrivals and cooldowns restore; a load does not re-roll an arrival already resolved |

## 2. Evidence
- `Assets/Ashfall.Core/Encounters/OrphanKnockWhitelist.cs` (whole directory; verified).
- Plan 146's `DoorEncounterSystem` is the consumer; this plan defines its gate.
- Plan 85 owns refugee admission once someone arrives; this plan governs arrival itself.
- Plan 61 owns hostile raid resolution; a hostile probe routes there rather than resolving here.

## 3. Packages
- **KWT-155A** category table (windows, prerequisites).
- **KWT-155B** context input wiring (threat/refuge/war owners).
- **KWT-155C** seeded day-based selection + paired-run determinism test.
- **KWT-155D** refusal outcome table + test per outcome.
- **KWT-155E** persistence: pending/cooldown round-trip, no re-roll on load.

## 4. Acceptance & verification
- Only whitelisted categories arrive; each arrival satisfies its prerequisites (fixture per category).
- A refused arrival reaches exactly one documented outcome.
- Same seed + same day → same arrival set across runs.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Encounters/` (create if absent).

## 5. Risks
Permissiveness → prerequisites and windows are tested per category; hostiles route to Plan 61.
Save re-roll → resolved arrivals are stored, not recomputed on load.

---

## 6. Expanded census (1 files · 64 lines)

Scope: `Assets/Ashfall.Core/Encounters/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `OrphanKnockWhitelist.cs` | 64 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `anomalous_expedition_encounters.json` | object[2 keys] |
| `narrative_encounters_expansion.json` | object[2 keys] |
| `narrative_encounters_npc_arcs.json` | object[2 keys] |
| `crossing_encounters.json` | object[3 keys] |
| `door_encounters.json` | array[80] |
| `narrative_encounters.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Encounters/` (create if absent) |
| Test references | 0 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `KWT-155A` | no name match — resolve at claim time |
| `KWT-155B` | no name match — resolve at claim time |
| `KWT-155C` | no name match — resolve at claim time |
| `KWT-155D` | no name match — resolve at claim time |
| `KWT-155E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 8. Host files: **1** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Content/ContentOrphanCertificationEngineTests.cs`, `Ashfall.Core.Tests/Expeditions/MicroLocationRegressionMatrixTests.cs`, `Ashfall.Core.Tests/MicroLocationCatalogLoaderTests.cs`, `Ashfall.Core.Tests/NarrativeEncounterSystemTests.cs`, `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crossing` |
| `deep_well` |
| `encounter_choice` |
| `encounters` |
| `expedition` |
| `expedition_stealth` |
| `narrative` |
| `narrative_questlines` |
| `oral_lore` |
| `procedural_narrative` |
| `psychological_arcs` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--crossing-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--narrative-selftest` |
| `--patrol-encounter-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **24**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnEncounterArrived` | `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` |
| `OnEncounterEnded` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnEncounterResolved` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterSelected` | `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs` |
| `OnEncounterTriggered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/door_encounters.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (76 files, 629 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Factions` | 10 | 72 |
| `Integration` | 16 | 74 |
| `MoralChoice` | 4 | 33 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 629 cases sit under matching regions — run those first (`Audio`, `Combat`, `Factions`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **52**
(17 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/ExpeditionHostSession.cs` |
| `src/Host/ExpeditionSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **24**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `combat` | no |
| `crossing` | no |
| `deep_well` | no |
| `dynamic_quests` | no |
| `encounter_choice` | no |
| `encounters` | no |
| `expedition` | no |
| `expedition_stealth` | no |
| `faction_espionage` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `combat` |
| `deep_coast` |
| `expedition` |
| `moral_choice` |
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **359**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 58, OPTIONAL 6, UNRESOLVED 16).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 16 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 24 (laddered 0) · RNG streams 8 · host files 23 · catalogs 22 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-KNOCK-WHITELIST-TRUTH-155
wave: 12
status: PROPOSED — foreman claim required
packages: KWT-155A, KWT-155B, KWT-155C, KWT-155D, KWT-155E
claim paths:
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/CombatHostSession.cs  # §19 candidate host surface
  - src/Host/CombatSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --crossing-selftest
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
