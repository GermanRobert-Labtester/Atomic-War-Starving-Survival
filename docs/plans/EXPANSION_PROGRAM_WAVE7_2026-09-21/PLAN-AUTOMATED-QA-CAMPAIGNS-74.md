# PLAN-AUTOMATED-QA-CAMPAIGNS-74 — Scenario Matrix, Long Runs & Regression Fuzz

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DETERMINISM-REPLAY-13, PLAN-SELFTEST-TRUTH-23,
PLAN-RUNTIME-PERF-16.
**Expanded appendix:** [`PLAN-AUTOMATED-QA-CAMPAIGNS-74_APPENDIX-A_MATRIX_RUNNERS.md`](PLAN-AUTOMATED-QA-CAMPAIGNS-74_APPENDIX-A_MATRIX_RUNNERS.md)
— the matrix **focus axis**: all registry verbs grouped into runner domains
(core 134, save 14, shelter 12, expansion 6, expedition 5, …), so each tier
rotates preset × seed × domain with the exact runner commands.
**Non-goals:** no full-suite default, no cloud farm, no UI screenshot farm
beyond the existing snapshot corpus.

## Outcome
The repo has strong focused gates but no **scheduled campaign matrix** that
plays many seeds/configurations end-to-end and asserts survival invariants.
The precedent exists (`--campaign-fuzz-selftest`, `--real-campaign-journey-selftest`,
`--7-day-smoke-selftest`, golden saves). This plan turns one-off runs into a
bounded, rotating matrix.

| Matrix axis | Values |
|---|---|
| Preset | easy / normal / hard (after Plan 73) |
| Seed | a fixed rotating set (e.g. 12 seeds) |
| Length | 7 / 30 / 180 days (tiered) |
| Focus | economy, medical, defence, expeditions, narrative, politics |
| Platform | host headless (release-tier adds one export run) |
| Assertion | invariants (below) |

**Invariants:** no NaN/negative resources; no dead-lock day; no null survivor;
save/reload equality at day N; no unbounded log growth; no owner over budget;
no unreachable required content; no silent catch in the run log.

## Evidence
- Selftests: `--campaign-fuzz-selftest`, `--7-day-smoke-selftest`, `--real-campaign-journey-selftest`, `--deterministic-smoke-selftest`, `--day1-*`, `--checksum-sweep-selftest`.
- Golden saves under `artifacts/golden_saves/`; runtime baseline `artifacts/runtime-scale-results.json`.
- Test policy: focused, 180 s cap; larger runs need an explicit window.
- Skills: `ashfall-seed-replay`, `ashfall-save-fuzz`, `ashfall-time-travel-debugger`.

## Packages
- **QA-74A** matrix definition + runner: one command runs a tier (7-day fast, 30-day nightly, 180-day weekly) across seeds/presets, emitting a summary artifact.
- **QA-74B** invariant assertions implemented once and reused across tiers.
- **QA-74C** failure triage: dump seed + config + owner trace + last 50 day events on failure; no giant raw logs.
- **QA-74D** rotation policy: seeds rotate per release; unresolved failures are quarantined records with an owner.
- **QA-74E** replay tie-in: every failure must reproduce deterministically (hash + save) before it is filed.
- **QA-74F** reporting: `artifacts/qa/matrix-<tier>-<date>.json` + a one-page markdown summary.

## Acceptance & verification
- 7-day tier runs bounded (≤ a few minutes); a seeded injected defect fails with a reproducible dump.
- `godot --headless --path . -- --qa-matrix --tier=7d`; `--campaign-fuzz-selftest`; nightly workflow artifact.

## Risks
Runtime creep → tiers bounded; only the fast tier runs on PRs; long tiers on schedule.

---

## 6. Expanded census (bespoke: QA campaign surface)

This plan rotates QA campaigns, so the census covers the test tree, the case
count, and the CI gate set.

| Metric | Value |
|---|---:|
| Test regions | 108 |
| Test files | 818 |
| `[Fact]`/`[Theory]` cases | 11864 |
| CI gates | 57 |

**Largest regions:**

| Region | Files |
|---|---:|
| `Shelter` | 87 |
| `World` | 65 |
| `Survivors` | 57 |
| `Medical` | 49 |
| `Radio` | 47 |
| `Economy` | 41 |
| `Expeditions` | 41 |
| `Tooling` | 35 |
| `Campaign` | 32 |
| `Narrative` | 26 |
| `UI` | 18 |
| `Integration` | 16 |

## 7. Expanded surface: campaign matrix

| Axis | Values |
|---|---|
| Preset | the difficulty presets (Plan 73) |
| Seed | a fixed small set per rotation; recorded |
| Length | bounded day counts (e.g. 7 / 30 / 90) |
| Focus | needs, economy, defense, narrative — one focus per run |
| Invariants | checked every run; a violation fails the campaign |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Determinism | same seed + preset → same checksum |
| Triage dump | on failure: day, system, diff, seed |
| Rotation cost | bounded wall-clock per slice (TEST_POLICY) |
| Coverage | each focus exercised at least once per rotation |

## 9. Rollout sequence

1. Matrix definition and seed set.
2. Invariant checks per focus.
3. Triage dump format.
4. Rotation schedule with budget.
5. Regression: one campaign slice run alone first.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Scenario | preset × seed × length × focus recorded |
| Invariant | failure names day/system/diff |
| Triage | dump reproducible from the recorded seed |
| Rotation | bounded; no overlap with active builders |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the test suite.

---

## 12. Cross-plan coupling

This plan governs QA campaign tiers and their artifacts rather than a `.cs`
domain, so the domain proxy is the campaign-tier set the plan defines.

**Governed surface (14):**

| Tier / artifact |
|---|
| `Campaign` |
| `Economy` |
| `Expeditions` |
| `Integration` |
| `Medical` |
| `Narrative` |
| `Radio` |
| `Shelter` |
| `Survivors` |
| `Tooling` |
| `World` |
| `artifacts/golden_saves/` |
| `artifacts/qa/matrix-<tier>-<date>.json` |
| `artifacts/runtime-scale-results.json` |

Other plans referencing these names (mostly via their own test regions):
**171**.

**Incoming plan edges (top 8):**

| Plan | Tier/artifact mentions |
|---|---:|
| `EVIDENCE` | 11 |
| `PLAN-ORPHAN-SEAL-01` | 9 |
| `PLAN-TEST-WELFARE-17` | 9 |
| `PLAN-THREADING-ASYNCHRONY-72` | 7 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 6 |
| `PLAN-SILENT-FAILURE-35` | 5 |
| `PLAN-DETERMINISM-REPLAY-13` | 4 |
| `PLAN-UNBLOCK-03` | 3 |

**Package → candidate QA surface (heuristic by name overlap):**

| Package | Candidate tier |
|---|---|
| `QA-74A` | no tier name match — assign at claim time |
| `QA-74B` | no tier name match — assign at claim time |
| `QA-74C` | no tier name match — assign at claim time |
| `QA-74D` | no tier name match — assign at claim time |
| `QA-74E` | no tier name match — assign at claim time |
| `QA-74F` | no tier name match — assign at claim time |

**Reading:** campaign tiers are shared test infrastructure; a claim here lands before or alongside the packages whose tests it aggregates.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **1** · Data files: **6**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/UI/PropagandaPanel.cs`, `src/UI/SurfaceShrapnelAegisPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Radio/RadioSignalAccessibilityPresentationTests.cs` |
| Data (`StreamingAssets/Data/`) | 6 | `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/narrative/dead_hand_directives.json`, `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json`, `Assets/StreamingAssets/Data/radio.json`, `Assets/StreamingAssets/Data/radio_intercepts.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `faction_espionage` |
| `propaganda_campaigns` |
| `radio` |
| `radio_program_production` |
| `radio_station` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--propaganda-campaign-selftest` |
| `--propaganda-selftest` |
| `--radio-catalog-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
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
shares a domain token: **11** (111 files, 848 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Events` | 1 | 6 |
| `Holdfast` | 1 | 13 |
| `Lifecycle` | 1 | 5 |
| `Presentation` | 1 | 5 |
| `Propaganda` | 2 | 13 |

**Verdict:** 848 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **254**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Host/PropagandaHostSession.cs` |
| `src/Host/PropagandaSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `combat` | no |
| `counter_intelligence` | no |
| `economy` | no |
| `events` | no |
| `faction_espionage` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `journal` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `economy` |
| `events` |
| `radio` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **84**
(CODEX_ONLY 27, GAMEPLAY_CONSUMED 38, OPTIONAL 6, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `desperation_events.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `events.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 0
**Surface:** save sections 16 (laddered 1) · RNG streams 7 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AUTOMATED-QA-CAMPAIGNS-74
wave: 7
status: PROPOSED — foreman claim required
packages: QA-74A, QA-74B, QA-74C, QA-74D, QA-74E, QA-74F
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/EconomyHostSession.cs  # §19 candidate host surface
  - src/Host/EconomySaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
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
