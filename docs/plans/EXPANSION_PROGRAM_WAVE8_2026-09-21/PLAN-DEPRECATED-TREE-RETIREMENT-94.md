# PLAN-DEPRECATED-TREE-RETIREMENT-94 — Retire `Assets/_Game/` Shims & Empty Legacy Markers

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ARCHITECTURE-BOUNDARY-31, PLAN-ORIGINALITY-LICENSING-60.
**Implementation scaffold:** [`PLAN-DEPRECATED-TREE-RETIREMENT-94_APPENDIX-A_SCAFFOLD.md`](PLAN-DEPRECATED-TREE-RETIREMENT-94_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ORPHAN-SEAL-01` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no removal of historical data files, no git history rewrite, no
change to engine-neutral Core.

## 1. Outcome
The Unity-era tree is nearly gone, but two files remain under `Assets/_Game/`:

| File | State |
|---|---|
| `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` | compiled **only** by `Ashfall.Core.Tests.csproj` (explicit `<Compile Include>`, line 29); contains `#if UNITY_5_3_OR_NEWER using UnityEngine; #endif` with a shim `Vector2Int`; exercised by `Performance/NoiseDisciplineBenchmarkTests.cs` |
| `Assets/_Game/Survivors/BunkerSocialSystems.cs` | compiled by **nothing** (the game csproj includes only `src/**` and `Assets/Ashfall.Core/**`) |

`src/Bridge/` is empty (the `bridge_selftest` in `HostCli.PanelTests.cs`
reports the UnityEngine shim removed), and `Assets/art/` contains a single
`.gdignore`. Repo-wide, the only non-comment engine-token matches are the
selftest string and the ban list in `CoreInvariantSourceTests.cs` — there is
no live `UnityEngine` usage left. What is missing is the retirement itself and
a gate that keeps it retired.

| Deliverable | Detail |
|---|---|
| Port decision | `NoiseDisciplineSystem` behaviour is ported to an engine-free Core/benchmark form or the benchmark is retired with its result recorded |
| Uncompiled file | `BunkerSocialSystems.cs` is ported or retired explicitly — never left as an uncompiled no-man's file |
| Test csproj | the `..\Assets\_Game\` compile link is removed once the port/retire lands |
| Gate | static scan (extend `CoreInvariantSourceTests`) bans `UNITY_5_3_OR_NEWER`, `using UnityEngine`, `UnityEngine.` beyond the existing ban-list literals; no new files under `Assets/_Game/` |
| Marker policy | `src/Bridge/` and `Assets/art/.gdignore` documented as intentional or removed |

## 2. Evidence
- `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: explicit `Compile Include` for the `_Game` file.
- `Ashfall.csproj`: compiles `src/**` + `Assets/Ashfall.Core/**` only.
- `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`: `using AtomicWar._Game.Shelter;`.
- `Ashfall.Core/.../SkillAtrophySystem.cs`, `SkillDef.cs`, and others document "NO `UnityEngine.*`" — the doc-comment convention already exists.
- `src/Host/HostCli.PanelTests.cs`: `"UnityEngine.* shim removed — src/Bridge/ is empty"`.

## 3. Packages
- **DTR-94A** reference map: who compiles/uses each `_Game` file (this table, verified in the package).
- **DTR-94B** port-or-retire: smallest engine-free port of the noise benchmark, or retire with recorded rationale.
- **DTR-94C** csproj cleanup: remove the link in the same change that lands the port.
- **DTR-94D** gate: extend the invariant source test; fail on new `UNITY_*` symbols or new `_Game` files.
- **DTR-94E** marker policy note in the architecture doc (`docs/CURRENT_AUTHORITY.md` or its successor).

## 4. Acceptance & verification
- No `Assets/_Game/` file is compiled by any csproj; the two files are ported or removed.
- Gate fails on an injected `UNITY_5_3_OR_NEWER` fixture and passes on the tree.
- Focused: `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/` (and the invariant test file).

## 5. Risks
Silent benchmark loss → the port keeps the measured behavior or the retirement records the numbers.
Stale links elsewhere → the reference map is repo-wide before edits.

---

## 6. Expanded census (bespoke: deprecation surface)

This plan retires the Unity-era residue, so the census covers the `_Game`
tree, engine-token references, project includes, and the bridge directory.

| Asset | Detail |
|---|---|
| `Assets/_Game/` files | 4: `NoiseDisciplineSystem.cs`, `NoiseDisciplineSystem.cs.uid`, `BunkerSocialSystems.cs`, `BunkerSocialSystems.cs.uid` |
| Non-comment Unity token lines (repo) | 3 |
| Game csproj compile globs | 2 |
| Test csproj `Compile Include` rows | 1 |
| Test csproj `_Game` include | **yes** |
| `src/Bridge/` | absent |

## 7. Expanded surface: retirement contract

| Rule | Detail |
|---|---|
| Port or retire | each `_Game` file is ported engine-free or retired with its rationale recorded |
| Compile link | the test project's `_Game` include is removed in the same change as the port |
| Gate | no new file under `Assets/_Game/`; no new `UNITY_*` symbol |
| Doc comments | the existing "no UnityEngine" comment convention is allowed (it is documentation, not a reference) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Token scan | non-comment Unity references = 3 (target: 0 or justified) |
| Compile scan | no csproj compiles `_Game` after the change |
| Gate fixture | an injected `UNITY_5_3_OR_NEWER` symbol fails the scan |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and per-file port/retire decisions.
2. Port the benchmark dependency or retire it with numbers.
3. Remove the test-project `_Game` compile link.
4. Wire the gate into the invariant source test.
5. Regression: token scan + project scan.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| `_Game` file | ported or retired with recorded rationale |
| Compile link | removed; no project compiles `_Game` |
| Gate | fails on injected Unity symbol; passes on the tree |
| Marker | `src/Bridge/` decision recorded (absent/present) |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not rewrite history.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 16. Other plans referencing them: **12**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-BUILD-ERGONOMICS-56` | 3 |
| `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 3 |
| `PLAN-TEST-WELFARE-17` | 2 |
| `PLAN-SKILL-PROGRESSION-TRUTH-113` | 2 |
| `PLAN-LATENT-EXPERT-TRUTH-239` | 2 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-DEBT-DRAIN-24` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Ashfall.Core.Tests.csproj` |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` |
| `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs` |
| `Ashfall.Core/.../SkillAtrophySystem.cs` |
| `Ashfall.csproj` |
| `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `Assets/_Game/Survivors/BunkerSocialSystems.cs` |
| `BunkerSocialSystems.cs` |
| `CoreInvariantSourceTests.cs` |
| `HostCli.PanelTests.cs` |
| `NoiseDisciplineSystem.cs` |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94_APPENDIX-A_SCAFFOLD.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DTR-94A` | no name match — resolve at claim time |
| `DTR-94B` | `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`, `PLAN-DEPRECATED-TREE-RETIREMENT-94_APPENDIX-A_SCAFFOLD.md`, `Performance/NoiseDisciplineBenchmarkTests.cs` |
| `DTR-94C` | `Ashfall.Core.Tests.csproj`, `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`, `Ashfall.csproj` |
| `DTR-94D` | `CoreInvariantSourceTests.cs` |
| `DTR-94E` | `docs/CURRENT_AUTHORITY.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 11. Host files: **807** · Test files: **1376** · Data files: **13**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 807 | `src/Audio/AudioConditionHostBridge.cs`, `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs`, `src/Audio/ExpansionAudioBridge.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1376 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs`, `Ashfall.Core.Tests/ActionResultTests.cs` |
| Data (`StreamingAssets/Data/`) | 13 | `Assets/StreamingAssets/Data/ceremonies.json`, `Assets/StreamingAssets/Data/events.json`, `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json`, `Assets/StreamingAssets/Data/narrative/expedition_briefs_expansion.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `equipment_condition` |
| `events` |
| `expedition` |
| `expedition_stealth` |
| `faction_espionage` |
| `host_event` |
| `microfluidic_diagnostic` |
| `mutation_tree` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **21** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--journal-weather-panel-selftest` |
| `--microfluidic-diagnostic-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnAtrophyDangerPassed` | `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/anomalies.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/ceremonies.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (218 files, 1723 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Crafting` | 1 | 11 |
| `Economy` | 41 | 329 |
| `Equipment` | 1 | 4 |
| `Events` | 1 | 6 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Radio` | 47 | 354 |

**Verdict:** 1723 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Combat`, `Crafting`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **349**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **40**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `combat` | no |
| `crafting` | no |
| `economy` | no |
| `encounters` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `events` | no |
| `expanded_shelter` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **17**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |
| `economy` |
| `events` |
| `expedition` |
| `foundry` |
| `medical_microfluidic_diagnostics` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **166**
(CODEX_ONLY 98, GAMEPLAY_CONSUMED 43, OPTIONAL 5, UNRESOLVED 20).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `ceremonies.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |

**Verdict:** 20 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 40 (laddered 0) · RNG streams 17 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DEPRECATED-TREE-RETIREMENT-94
wave: 8
status: PROPOSED — foreman claim required
packages: DTR-94A, DTR-94B, DTR-94C, DTR-94D, DTR-94E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalies.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
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
