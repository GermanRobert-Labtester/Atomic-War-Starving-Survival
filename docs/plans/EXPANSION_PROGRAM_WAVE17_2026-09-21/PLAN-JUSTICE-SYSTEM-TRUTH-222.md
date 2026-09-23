# PLAN-JUSTICE-SYSTEM-TRUTH-222 — Narrative Justice: Cases, Hearings & Records

**Wave 17 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-JUSTICE-LAW-37, PLAN-INVESTIGATION-EVIDENCE-TRUTH-121, PLAN-PRISONER-TRUTH-197.
**Non-goals:** no verdict/trial model (Plan 37), no evidence chain (Plan 121), no
holding state (Plan 197).

## 1. Outcome
`Narrative/JusticeSystem.cs` (**445 lines**) is reachable and unaddressed: the
narrative instances of justice — cases arising from story events, hearings, and
the records they leave. Plan 37 owns law/verdict, Plan 121 evidence, Plan 197
custody; the **case lifecycle from story hook to record** is unowned.

| Deliverable | Detail |
|---|---|
| Case model | cases opened by story events with parties, claim class, and a state (`open → heard → resolved`) |
| Hearing hook | a hearing consumes Plan 121's chain state; it never re-derives evidence |
| Outcome routing | outcomes write to Plan 37/197 owners (verdict, custody); no private ledger |
| Records | case records persist with dates and are readable where other records live |
| Save truth | case state restores; a load never re-opens or re-decides |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/JusticeSystem.cs` (445 lines; unaddressed — Wave 17 audit).
- Plan 121's chain is the hearing input; Plan 37 the outcome owner.
- Plan 197 consumes custody outcomes; Plan 110's codex can surface records.
- Plan 18 owns story events that open cases.

## 3. Packages
- **JST-222A** case model + claim classes.
- **JST-222B** hearing→chain contract test.
- **JST-222C** outcome routing (no private ledger).
- **JST-222D** record persistence + readability test.
- **JST-222E** save round-trip; no re-open/re-decide on load.

## 4. Acceptance & verification
- A hearing consumes chain state without recomputation; outcomes appear in 37/197.
- Records persist and render; save/load preserves case state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Second verdict path → outcomes route to Plan 37 only; the boundary is tested.
Evidence drift → hearings read the chain; no local copies.

---

## 6. Expanded census (1 files · 445 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `JusticeSystem.cs` | 445 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `verdict_data.json` | object[9 keys] |
| `verdict_items.json` | array[15] |
| `verdict_locations.json` | object[2 keys] |
| `verdict_npcs.json` | array[18] |
| `verdict_questlines.json` | object[2 keys] |
| `verdict_radio.json` | object[2 keys] |

**State surfaces:** `JusticeSystem.cs`.

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
| `PLAN-JUSTICE-LAW-37` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `JST-222A` | no name match — resolve at claim time |
| `JST-222B` | no name match — resolve at claim time |
| `JST-222C` | no name match — resolve at claim time |
| `JST-222D` | no name match — resolve at claim time |
| `JST-222E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 7. Host files: **9** · Test files: **12** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.ExpansionDepth.cs`, `src/Host/HostCli.SelfTests.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs`, `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`, `Ashfall.Core.Tests/Integration/Plans190_193_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Narrative/JusticeSystemTests.cs`, `Ashfall.Core.Tests/Narrative/NarrativeContinuityTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `faction_espionage` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `verdict` |
| `wasteland_justice` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--campaign-journey-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--narrative-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnVerdictResolved` | `Assets/Ashfall.Core/Verdict/ReckoningSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (197 files, 1500 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Flagship11` | 7 | 63 |
| `Integration` | 16 | 74 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 1500 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **181**
(27 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/AssetRegistry.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/CompletionHistorySelfTest.cs` |
| `src/Host/ContentUtilizationRuntimeCollector.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **29**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `crossing` | no |
| `deep_well` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `deep_coast` |
| `economy` |
| `mineral_chemical` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **379**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 73, OPTIONAL 5, UNRESOLVED 22).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |

**Verdict:** 22 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 29 (laddered 0) · RNG streams 13 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-JUSTICE-SYSTEM-TRUTH-222
wave: 17
status: PROPOSED — foreman claim required
packages: JST-222A, JST-222B, JST-222C, JST-222D, JST-222E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --asset-coverage-report
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
