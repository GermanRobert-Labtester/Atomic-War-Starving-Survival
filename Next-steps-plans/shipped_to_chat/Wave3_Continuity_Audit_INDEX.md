# Continuity Wave 3 — Audit Index (Plans 25–29): *Ship It Intact*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Gates I ran this wave (with results):**

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `dotnet test Ashfall.Core.Tests` | PASS — **5303 passed, 0 failed** |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 138 catalogs, 5563 ids, 0 errors |
| `bash scripts/ci/triad-drift-gate.sh` | PASS |
| `bash scripts/ci/doc-link-gate.sh` | PASS (1,173 files) |
| `bash scripts/ci/warning-baseline-gate.sh` | PASS (0 warnings, all targets) |
| `python3 scripts/ci/sync-agent-rulebooks.py --check` | 🔴 **FAIL — 12 rulebooks drifted from `AGENTS.md`** |
| `python3 scripts/ci/generate-docs-index.py --check` | 🔴 **FAIL — `docs/INDEX.md` out of sync** |
| `python3 scripts/ci/generate-agent-skills-catalog.py --check` | 🔴 **FAIL — `docs/agents/AGENT_SKILLS_INDEX.md` out of date** |

Waves 1 and 2 asked *"does the game connect to itself?"* (story, then physics). Wave 3 asks
*"can this thing be shipped, tested, and trusted by the next person who reads the docs?"* — and the
answer today is **no on all three**, in ways that are cheap to fix and get expensive every week.

---

## Wave 3 findings: the 10 highest-impact production-continuity gaps

| # | Gap | Category | Severity | Why it matters to the player | Smallest action | Deps | Timing |
|---|---|---|---|---|---|---|---|
| 1 | **Three critical CI gates are red right now** (`agent_rulebooks_sync`, `docs_index_drift`, `agent_skills_catalog_drift`) | testing / production | **critical** (process) | Red gates train humans and agents to ignore gates; the ignored ones are the ones that catch shipped breakage | run the generators + land the 95-path tree as reviewable commits | none | **immediately — 29A, today** |
| 2 | **`GEMINI.md` holds another client's rules and is outside the sync contract** | technical architecture | **important** | ~4.8 KB of Antigravity token-budget rules where AGENTS.md's invariants should be; that client never learns "Godot is authoritative / no Unity" | restore it as a derived copy **and add it to `sync-agent-rulebooks.py`'s list** (12 → 13 covered) | 29A | before/with |
| 3 | **`AGENTS.md` instructs agents to wire into a class that does not exist** (`GameBootstrap`, cited at `:236/:240/:241`; `find` → no files; `ASHFALL_CODE_INDEX.md:181` marks it Unity-only "NOT to be ported"), plus **H7** wrong (`~6.5k`-line single file vs 56 partials/14,361 lines; Setup/Save/Flush 31-24-17 vs real 72-69-26) and **H5/H11** resolved-but-advertised | technical architecture | **critical** (knowledge) | Every agent session in this repo loads it; agents keep "wiring Phase 4" into nothing and re-auditing dead issues | rewrite the expansion workflow + H7/H5/H11 rows against source, regenerate the 12 copies | 28A gives the real seam | **before** next expansion wave |
| 4 | **CI exports with raw `godot --export-release`, not `scripts/ci/godot-export-linux.sh`** (`build.yml:44–48`), so staging + loose deploy + representative-file checks never run in CI; the "verify data authority" step is `if [ -f ] … echo` and **cannot fail**; the Windows job stages and verifies **nothing** | production | **critical** | The downloaded build may not find its 413-catalog data authority; every gate runs from source, so a broken artifact is invisible | route both jobs through the scripts; assertions that can fail; add boot + load smoke | 26A | **before** release work |
| 5 | **The data authority is packed twice (or zero times)** — the export script assumes `Assets/.gdignore` ("prevents packing Assets/StreamingAssets/Data"), but no `.gdignore` exists under `Assets/` or `Assets/StreamingAssets`, with `include_filter="*.json"` + `all_resources`; the script also stages `assets/StreamingAssets/Data`, so both `res://Assets/…` and `res://assets/…` can land in one PCK (8.3 MB ×2, resolver coin-flip, on the case-alias hazard `setup-repo.sh` guards) | production | **critical** | Two copies means a build can read a different authority than the repo; on case-insensitive filesystems it can read neither reliably | add `Assets/StreamingAssets/.gdignore` (or drop lowercase staging) and assert one canonical path + a PCK file count | 26A/26B | with 4 |
| 6 | **Five data-path resolvers bypass the one good resolver** — `EventsHostSession.cs:40,49,61` literal `res://Assets/…` filenames, `Main.FactionBranch.cs:18–20` `GlobalizePath` + `AppContext`, `HoldfastTerminalPanel.cs:72` relative path, `RadioHostSession.cs:67` `GetCurrentDirectory`, `PanelBindLifecycleSelfTest.cs:366` — while `CatalogPath.cs` already handles env/exe/CWD/PCK and the `GodotFileIO` switch | technical architecture / testing | **important** | Content that loads fine in-editor can be absent in the shipped build (radio corpus, events, faction branch data) | `CatalogPath.ResolveCatalog(file)` + forbidden-path gate | 26B | before |
| 7 | **Selftests validate a demo copy of the game** — live uses `InventoryHostSession.Create(_dataDir)`→`items.json`; 6+ test paths use the ctor that calls `SeedCatalog()` with hardcoded defs (`InventoryHostSession.cs:30`) incl. `InventorySaveSelfTest.cs:12,21`, `PanelBindLifecycleSelfTest.cs:211`, `HostCli.PanelTests.cs:627…`, `Main.UiTests.Inventory.cs:90` | testing | **important** | 5,303 green tests coexisted with an inert `Consume`, a frozen `DegradeRate`, and a hardcoded ending — because the tests weren't running the shipped data | explicit `CreateForFixture()` naming + a fidelity assertion that fixture == authority | 26A | with 1 |
| 8 | **Coverage is not measured at all** — no `coverlet`/`CollectCoverage` anywhere in the test project or central packages, though the `ashfall-coverage-gate` skill prescribes it; and gates assert presence (`PanelRouteGateTests` = registered, `PlayerSurfaceCoverageGateTests` = metadata-as-binding) | testing | **important** | Nobody can say which survival invariant is untested until a player says so | baseline coverage + gate *only* save/determinism slices, monotonic; 100 % of `CaptureState/RestoreState` pairs round-tripped | 27A | during |
| 9 | **Content claims are grep-evidence, not observation** — `bestEvidence` STATIC 402 / **RUNTIME 9**; stages `DESERIALIZED 0 · REGISTERED 0 · SELECTED 0 · EFFECT_PRODUCED 4` of 411 catalogs, yet `Actionable Priorities` reports `0,0,0,0,0` | testing / content | **important** | "Content shipped" and "content reachable" are the same word in this project's metrics; the first-hour player can tell | collect during a real boot; gate `SELECTED`/`EFFECT_PRODUCED` monotonically | 27A, 26B | during |
| 10 | **Ship-readiness measures are advisory or missing** — `--runtime-scale-selftest` records `day_advance_30d` as `"advisory"` (median **0.609 s**, p95 **1.145 s**, max **1.265 s**, 5 iterations), 30 golden snapshots vs 135 routes, 470 KB of duplicated instruction text loaded into every agent session (37.4 KB × 13), and plan numbering split three ways across four folders | production / technical architecture | **later** (but ratchet early) | Frame pacing, UI regressions, and agent misbehaviour all surface as player complaints if they don't surface as gates | budgets from measured data; snapshots to the *live* route count; one wave ledger + numbering policy | 16A, 29A | after 1–6 |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [25](Plan_25_Localization_One_Language_Of_Strings.md) | One Language of Strings | 3 (text half), 10 | Zero user-facing English in `src/`, prose translatable without reshaping the authority, every script renders. |
| [26](Plan_26_Ship_Gate_Exported_Builds_Find_Their_Data.md) | The Ship Gate | 4, 5, 6 | One resolver, one packed copy, and a headless boot of the exported artifact on every push. |
| [27](Plan_27_Tests_That_Mean_It_Fidelity_Coverage_Journeys.md) | Tests That Mean It | 1 (process), 7, 8, 9 | Tests run the shipped data, coverage is a gated number, five journeys fail when a connection breaks. |
| [28](Plan_28_Orchestration_Spine_Registration_And_Lifecycle.md) | The Orchestration Spine | 3 (H7), 6 (root cause) | A subsystem that forgets its Save, owner, route, or teardown cannot register. |
| [29](Plan_29_One_Truth_Docs_Canon_Agent_Instructions.md) | One Truth | 1, 2, 3, 10 | Gates green, rulebooks whole, and every claim cites file:line or is marked unverified. |

---

## Cross-wave map: what each wave was for

| Wave | Question it answers | Plans | Headline gaps |
|---|---|---|---|
| **1 — The Story Machine** | Does anything the player *choose* matter? | 15–19 | ending hardcoded; choices unmakeable; 30 fake consoles; content causally inert; guidance unreachable |
| **2 — The Bunker Machine** | Does anything the player *do* have a physical cause? | 20–24 | dose a literal; gear immortal; eating a no-op; power decorative; roster blind to bodies |
| **3 — Ship It Intact** | Can we build, test, and describe this honestly? | 25–29 | red gates; stale instructions; unbooted artifacts; demo-fixture tests; no coverage |

**One root cause runs through all three:** every measurement in this repository answers *"does it
exist?"* — routes, loaders, descriptors, rulebooks, exports — and almost none answer *"does it
act?"* Wave 1's 15C, Wave 2's 24A, and Wave 3's 27B/29B are the same correction applied at three
different altitudes.

**Interleaved execution order across Wave 3:** 29A (today) → 26A → 26B → 27A → 25A → 26C → 27B →
25B → 28A → 27C → 25C → 28B → 28C → 29B → 29C. If capacity allows three tasks only:
**29A, 26A+26B, 27A** — a green record, an artifact that boots with its data, and tests that
describe the shipped game.

## Metrics to report at wave close

1. Fast-tier gates failing: **3 → 0**, rulebooks covered by the sync contract: **12 → 13**
2. Exported-build boot smoke in CI: **absent → required**
3. Canonical packed copies of the data authority: **ambiguous → 1**, PCK JSON count asserted = source count
4. Data-path resolvers outside `CatalogPath`: **5 → 0**, gated
5. Selftest fixture-vs-authority divergence: **detected & named** (was invisible)
6. Coverage measured for save/determinism slices: **none → baselined, monotonic**
7. `RUNTIME`-evidence catalogs: **9 → real-campaign count** · `EFFECT_PRODUCED`: **4 → rising with Waves 1–2**
8. Snapshots vs live routed panels: **30 vs 30 → N vs N**
9. `day_advance_30d` budget: **advisory → gated with headroom**
10. Capability claims citing evidence: **0 → every registry row**

## Deferred to Wave 4 → **now planned**

**[Continuity Wave 4 — Plans 30–34, *The World Beyond the Gate*](Wave4_Continuity_Audit_INDEX.md)**
picked up: autonomous faction war (`SimulateDailyFriction` had zero game-path callers), the semantic
day-event layer (including an **erratum against Wave 1's Task 17A** — 20 of 27 emitted kinds are
silently dropped by a `switch` with no `default`), the travel graph (6 nodes, no distances), the
intel/radio economy (`radio_distress_signals.json` has no loader), and milestones/difficulty/legacy
(`hardcore_economy_tuning.json` applied as three empty arrays). Read that erratum before executing
`Plan_17` Task 17A. Remaining Wave-4 candidates (people-as-story depth, mod surface, first-hour
telemetry, accessibility conformance, long-session durability) are listed in the Wave 4 index under
"Deferred to Wave 5".

Original candidate list:

* **People as story depth** — survivor voice/memory/relationship continuity after Wave 2's health-and-duty ledger (and after the parallel expansion wave's 131/132 information + agenda systems, which this wave's authority fixes should land under).
* **Mod/creator surface** — `ashfall-mod-contract` is a skill, not a boundary; the JSON authority is already the mod-safe seam, and 25C's overlay pattern generalises to content packs.
* **Difficulty & onboarding telemetry at scale** — `ashfall-telemetry-playtest` funnel over 20+ seeds to place the first-day cliff, using Wave 2's event stream as the data source.
* **Accessibility conformance pass** — colour-independent status, key rebinding, subtitle/caption parity from 25C.
* **Long-session durability** — 360-day soak growth (26C) becomes a release gate with a saved-game corpus.
