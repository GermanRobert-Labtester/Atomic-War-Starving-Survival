# ASHFALL — Plans for Fixation: 30 Additional Forensic Findings

**File:** `plans-forfixation.md`
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Branch audited:** `main`
**Pinned snapshot:** `9b4985d0122d707c31f6078050df5877b69b607b`
**Prepared:** 2026-09-04
**Relationship to prior report:** This is a continuation of `game_repository_remediation__plan.md`. Its 30 issues are intentionally selected outside the previous 20-item remediation set wherever possible.

---

## 0. Audit method and confidence policy

This pass was performed as a repository-forensics continuation, not as a random TODO list.

The investigation cross-checked:

- current `main` source at commit `9b4985d...`;
- current Godot host code under `src/`;
- Core authority under `Assets/Ashfall.Core/`;
- current JSON data under `Assets/StreamingAssets/Data/`;
- current GitHub Actions export workflow;
- current selftests and UI smoke tests;
- current project audits and semantic-review records;
- recent continuity plans only when their findings still matched current source.

### Important rejected stale findings

Several older audit findings were explicitly **rejected** because current source already repairs them:

- **Faction war no longer appears frozen:** `YearOfAshHostSession.TickDay()` now calls `_factionWar.SimulateDailyFriction(day)` and `_warRunner.TickDay(day)`.
- **The save transaction buffer is now reset per save:** `Main.SaveOrchestrator.cs` clears `_sectionPayloads` and `_sectionCaptureFailed`.
- **Save commit acknowledgement is now checked:** `SaveAllDirect(...)` returns a Boolean and current orchestrator checks it.
- **Fresh-new-game slot reset was strengthened:** `ResetAllSessions()` now calls `ResetSlotForNewGame(new SaveSlotId("slot_1"))`.
- **Controller support is no longer accurately described as “zero”:** current `AshfallInputActions.cs` creates `InputEventJoypadButton` bindings.

This report therefore does **not** pad the list with those stale claims.

### Confidence labels

- **CURRENT-CONFIRMED** — directly re-found in current source or current workflow.
- **CURRENT-STRUCTURAL** — current search proves the relevant seam/absence, but runtime reproduction is still required before changing behavior.
- **COVERAGE-GAP** — the problem is missing proof/gating rather than a proven runtime malfunction.
- **ARCHITECTURE-GAP** — a missing unifying authority produces foreseeable drift but may not yet manifest as a user-visible bug.

---

# 1. Executive priority table

| ID | Finding | Severity | Confidence | Primary risk |
|---|---|---:|---|---|
| FX-01 | Epilogue still receives hardcoded campaign outcomes | P0 | RESOLVED | ending lies about the run |
| FX-02 | Radiation exposure is still keyed to one survivor ID | P0 | CURRENT-CONFIRMED | central hazard is fabricated |
| FX-03 | PowerGrid is only partially consumed by life-support systems | P0 | CURRENT-STRUCTURAL | blackout does not propagate |
| FX-04 | Onboarding/guidance remains effectively unreachable | P1 | CURRENT-STRUCTURAL | first-hour UX failure |
| FX-05 | Localization runtime layer is still absent | P1 | CURRENT-STRUCTURAL | release/accessibility blocker |
| FX-06 | `WornGear` remains duplicated across domains | P2 | CURRENT-CONFIRMED | state drift/bridge complexity |
| FX-07 | No shared commitment/deadline authority exists | P1 | ARCHITECTURE-GAP | obligations fragment |
| FX-08 | Seasons remain weakly coupled to survival mechanics | P1 | CURRENT-STRUCTURAL | winter is partly cosmetic |
| FX-09 | No difficulty-preset authority is present | P2 | CURRENT-STRUCTURAL | balance cannot be selected/reproduced |
| FX-10 | Keyboard focus/navigation policy is not broadly applied | P1 | CURRENT-STRUCTURAL | inaccessible management UI |
| FX-11 | Bare `InventoryHostSession` silently creates a demo catalog | P1 | CURRENT-CONFIRMED | tests/runtime can use false data |
| FX-12 | Missing starting-supply JSON silently falls back to literals | P1 | CURRENT-CONFIRMED | missing authority is masked |
| FX-13 | Demo seed catalog contains duplicate item IDs | P1 | CURRENT-CONFIRMED | order-dependent fixture behavior |
| FX-14 | Multiple selftests still instantiate the demo inventory host | P1 | CURRENT-CONFIRMED | green tests can test wrong game |
| FX-15 | Utility-AI UI smoke expects 4 actions while JSON defines 6 | P1 | CURRENT-CONFIRMED | smoke test/data contradiction |
| FX-16 | Linux export “data verification” can pass with no data | P0 | CURRENT-CONFIRMED | broken artifact can ship green |
| FX-17 | Windows export verifies executable only, not data/boot | P0 | CURRENT-CONFIRMED | Windows artifact can be unusable |
| FX-18 | CI bypasses the more complete canonical export wrapper | P1 | CURRENT-CONFIRMED | local/CI artifact drift |
| FX-19 | Catalog-path resolver is bypassed at multiple call sites | P0/P1 | CURRENT-STRUCTURAL | editor works, export fails |
| FX-20 | `OpenPlayerPanel` keeps a giant legacy fallback switch | P1 | CURRENT-CONFIRMED | missing registry wiring stays hidden |
| FX-21 | Legacy panel fallback fabricates “first”/placeholder context | P1 | CURRENT-CONFIRMED | UI displays wrong entity/item |
| FX-22 | Persisted logs have no retention policy | P1 | CURRENT-STRUCTURAL | long-run save growth |
| FX-23 | Seven-day deterministic smoke exists but is not mandatory | P1 | CURRENT-STRUCTURAL | strongest journey proof is optional |
| FX-24 | Performance headline is advisory and statistically weak | P1 | CURRENT-STRUCTURAL | regressions remain green |
| FX-25 | Save durability failure matrix is incomplete | P0/P1 | COVERAGE-GAP | campaign-loss risk |
| FX-26 | Repository has no Git tags or GitHub releases | P1 | CURRENT-CONFIRMED | no known-good release anchor |
| FX-27 | Historical save corpus is not a committed compatibility gate | P1 | COVERAGE-GAP | migrations proven synthetically only |
| FX-28 | Balance telemetry exists without declared design targets | P1 | CURRENT-STRUCTURAL | tuning lacks pass/fail meaning |
| FX-29 | Player funnel/session telemetry is absent | P2 | CURRENT-STRUCTURAL | first-hour problems unmeasured |
| FX-30 | Data override/mod surface exists without a compatibility contract | P2 | ARCHITECTURE-GAP | accidental public API drift |

---

# 2. Detailed fixation plans

## FX-01 — Epilogue still receives hardcoded campaign outcomes

**Severity:** P0
**Category:** gameplay correctness / narrative continuity
**Confidence:** CURRENT-CONFIRMED
**Primary files:** `src/Main.GameFlow.cs`, `src/Main.PlayerSurfaces.cs`, epilogue Core/UI classes.

### Specific problem

The current ending route still binds the epilogue using a mixture of live values and literal outcome values. The current call shape includes current day and survivor count, followed by a literal `0` and multiple literal `true` flags.

That means the ending presentation is not a faithful evaluation of what happened during the campaign. Two campaigns with radically different political, moral, medical, regional, generational, or economic histories can receive the same outcome flags.

This is not cosmetic. The ending is where every prior system is supposed to cash out.

### Failure modes

1. A failed treaty can be presented as successful.
2. A lost region can be treated as saved.
3. Debt, faction relations, children, memorial state, Verdict state, or world condition may never affect ending selection.
4. Adding more ending slides increases the amount of content driven by fabricated inputs.
5. Tests can validate epilogue rendering while never testing epilogue truth.

### Fix plan

1. Create a Core `CampaignOutcomeSnapshot` DTO.
2. Build one `CampaignOutcomeEvaluator` that derives the snapshot exclusively from campaign-owned authorities.
3. Required inputs should include, where applicable: surviving/dead population, campaign day/season, faction standing/treaties, major quest/moral outcomes, Verdict/Year-of-Ash state, region/world outcomes, debt/default status, shelter viability, memorial/death history, children/generation state, and critical endgame flags.
4. Persist authoritative source state only; do not create a second independently mutable ending authority.
5. Both automatic game-over epilogue and manually opened epilogue panel consume the same snapshot.
6. Delete all literal ending outcome arguments.
7. Add an outcome trace explaining why every ending predicate is true or false.
8. Add deterministic tests with at least five deliberately different campaigns.

### Acceptance tests

- Change one source condition and prove the derived ending changes.
- Save before ending, reload, derive identical snapshot.
- No `Bind(..., 0, true, true...)` style literal outcome bundle remains in production.
- Epilogue evaluator contains no UI dependencies.

### Resolution (2026-09-05)
- **Status:** RESOLVED
- **Core Architecture:** Added `CampaignOutcomeSnapshot` DTO (`Assets/Ashfall.Core/Endgame/CampaignOutcomeSnapshot.cs`) and `CampaignOutcomeEvaluator` (`Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs`) with 0 engine coupling (Invariant 1).
- **Orphan Sidecar Fixed:** Restored `Assets/Ashfall.Core/Endgame/EpilogueContextFactory.cs` matching `EpilogueContextFactory.cs.uid`.
- **Host Integration:** Implemented `Main.BuildCampaignOutcomeSnapshot()` in `src/Main.Endgame.cs` querying living survivors, memorial deaths, regional treaties, Verdict reckoning state, debt ledger status, cohort/generational children, and consequence flags. Replaced literal `0, true, true, true, true, true` calls in `src/Main.GameFlow.cs` and `src/Main.PlayerSurfaces.cs` with `BuildCampaignOutcomeSnapshot()`.
- **UI Trace Display:** Updated `src/UI/EpiloguePanel.cs` with `Bind(CampaignOutcomeSnapshot snapshot)` and dynamic evaluation trace display under `AUTHORITATIVE CAMPAIGN PROVENANCE & EVALUATION TRACE`.
- **Verification:** 9 dedicated unit tests in `Ashfall.Core.Tests/Endgame/CampaignOutcomeEvaluatorTests.cs` covering 5 distinct campaign fixtures, sensitivity, provenance trace, determinism, and production source scanning. All 9 passed. Verification matrix (`dotnet build Ashfall.csproj`, `scene-lint.py`, `--data-integrity-selftest`, `--scene-binding-selftest`, `--content-utilization-selftest`, `--endings-selftest`, `--player-panels-uitest`) all PASS with 0 errors.

---

## FX-02 — Radiation exposure is still keyed to one survivor ID

**Severity:** P0
**Category:** core survival simulation
**Confidence:** CURRENT-CONFIRMED
**Primary file:** `src/Host/SurvivorsHostSession.cs`

### Specific problem

Current code still contains an identity-specific branch where `survivor_gunner_mikhail` receives a high literal zone value while other survivors receive a low literal value. Shelter shielding is also conditioned around that special case.

This makes the game's defining environmental hazard partly a scripted character demo rather than a function of world state.

### Failure modes

- Moving survivors between shelter/surface may not produce correct dose changes.
- Two survivors standing in the same location can receive different environmental exposure merely because of ID.
- Weather, fallout intensity, route/sector contamination and actual position can be bypassed.
- Tests built around Mikhail can pass while generic survivor exposure is wrong.

### Fix plan

1. Introduce an immutable `RadiationExposureContext` assembled by the host.
2. Context inputs: current world/location ID, indoor/outdoor state, ambient zone dose rate, current weather/fallout multiplier, shelter attenuation, air/contamination contribution, equipped protection and exposure duration.
3. Resolve location from canonical survivor/expedition/duty state, not identity.
4. Remove all survivor-ID branches from environmental-dose calculation.
5. Add diagnostics logging the components of computed dose.
6. Create deterministic position-transfer tests.
7. Add a source gate forbidding survivor IDs inside environmental hazard formulas.

### Acceptance tests

Same survivor:
- bunker -> low/attenuated dose;
- surface clear -> higher;
- surface storm -> higher still;
- protected gear -> reduced;
- return inside -> dose source changes immediately.

Two survivors in identical context receive identical base environmental dose before individual protection.

---

## FX-03 — PowerGrid is only partially consumed by life-support systems

**Severity:** P0
**Category:** cross-system integration
**Confidence:** CURRENT-STRUCTURAL
**Primary files:** `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, powered subsystems.

### Specific problem

Current production search shows real power checks in a limited set of places, notably sump flooding and Silent Foundry. The PowerGrid UI naturally reads `IsRoomPowered`, but many systems that logically require power do not appear among current consumers.

High-risk omissions include water treatment, air handling/filtration, refrigeration/cold storage, greenhouse lighting, medical ward equipment, heating/thermal support and selected research/workshop machinery.

### Why this matters

If a blackout only changes a panel and a few systems, energy allocation is not a survival decision. It becomes presentation state.

### Fix plan

1. Create a small Core `IPowerAvailability` interface.
2. Every power-dependent system declares room/load ID, minimum watts, degradation behavior under brownout and behavior at zero power.
3. Inject the interface rather than query global/singleton state.
4. Remove private Boolean “powered” authorities where PowerGrid should decide.
5. Define brownout semantics: off, reduced throughput, damaged or safe shutdown.
6. Add a power dependency registry so CI can enumerate powered systems.
7. Add a blackout journey covering water, air, heat, refrigeration, medicine and production.

### Acceptance tests

Cut generation to zero:
- each registered powered system changes behavior;
- no powered system continues full output;
- restoring power returns systems deterministically;
- UI status derives from the same state.

---

## FX-04 — Onboarding/guidance remains effectively unreachable

**Severity:** P1
**Category:** UX / first-hour usability
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

The project has an onboarding state/panel, but current searches still do not reveal a normal player route that explicitly opens/reopens the onboarding hint surface. Earlier audit evidence showed the panel starts hidden and is persisted, yet no ordinary visibility action reopens it.

A management game with over a hundred routes cannot treat help as a one-time invisible state.

### Fix plan

1. Define canonical player action `guidance`.
2. Add visible dashboard affordance: `GUIDANCE // F1`.
3. Add `Open()`, `Close()`, `OpenTopic(topicId)` APIs.
4. Make it reopenable at any point.
5. Persist dismissed tutorial steps separately from visibility.
6. Support contextual deep-links from panels.
7. Add first-run and returning-player modes.
8. Add keyboard and controller access.

### Acceptance tests

Fresh game opens intended initial guidance; close it; reopen using UI and key; save/reload and reopen again; no modal trap; every critical first-day system has a guidance anchor.

---

## FX-05 — Localization runtime layer is still absent

**Severity:** P1
**Category:** release readiness / accessibility / content architecture
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Current source search does not expose a real translation API integration in production UI. Existing project plans identified hundreds of inline English UI assignments and no active translation resources.

This creates a future migration explosion because content, UI, warnings and buttons remain authored as rendered English rather than stable keys.

### Fix plan

1. Introduce `ILocalizer` or a thin Godot translation adapter.
2. UI code renders keys + parameters, not literal English.
3. Add extraction for C# UI strings, `.tscn` text and JSON player-facing fields.
4. Create `assets/l10n/en.csv` or equivalent Godot-supported resource.
5. Store narrative identifiers separately from localized text.
6. Add plural/parameter handling.
7. Gate new player-facing literal strings.
8. Add pseudo-localization CI for long strings, accented strings and missing keys.

### Acceptance tests

Switch locale without restarting campaign state. No missing key displays in the flagship beta journey. All key UI panels render under pseudo-localization without clipping critical actions.

---

## FX-06 — `WornGear` remains duplicated across Inventory and Radiation

**Severity:** P2
**Category:** duplicate state model / technical debt
**Confidence:** CURRENT-CONFIRMED

### Specific problem

The repository still documents two `WornGear` representations with a conversion bridge between Inventory and Radiation. A sanctioned bridge reduces immediate damage, but it does not remove the structural risk: there are still two representations of the same fact.

### Risks

- New durability/protection fields may be added to one and forgotten in the other.
- Copy semantics can make radiation mutate a projection rather than authoritative equipment.
- Every additional equipment mechanic must understand the bridge.

### Fix plan

1. Select one canonical `WornGearState` under equipment/inventory.
2. Radiation accepts a read-only view/interface.
3. Move protection calculation to a pure function if possible.
4. Update all tests to use the canonical type.
5. Add compile-time/source gate preventing a second `WornGear` class.
6. Delete `FromInventory()` after all callers migrate.

### Acceptance tests

One type owns maximum durability, current durability, protection and degradation characteristics. Radiation cannot mutate an unpersisted copy.

---

## FX-07 — No shared commitment/deadline authority exists

**Severity:** P1
**Category:** progression architecture
**Confidence:** ARCHITECTURE-GAP

### Specific problem

The repository has multiple systems that conceptually create promises with deadlines—tribute, debt, delivery, treaty, census or contract obligations—but current source does not expose a common `CommitmentSystem`.

This encourages each subsystem to invent due-day math, overdue handling, notifications, save representation and default consequences.

### Fix plan

1. Introduce Core `CommitmentLedger`.
2. Commitment fields: id, source type/source id, accepted day, due day, amount/requirement, status, completion action, default consequence and reminder thresholds.
3. Adapt warlord/debt/delivery/treaty obligations into the common ledger.
4. Keep domain-specific consequence logic outside the generic ledger via callbacks/ports.
5. Add one “Obligations” read model rather than separate hidden clocks.
6. Save commitments once.
7. Emit due-soon/overdue semantic events.

### Acceptance tests

Same deadline math across save/reload. Missed obligations fire exactly once. Completed obligation never defaults later.

---

## FX-08 — Seasons remain weakly coupled to survival mechanics

**Severity:** P1
**Category:** world simulation integration
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Current `GetSeasonForDay(...)` usage is visible in WeatherSystem, WeatherIntelligenceCoordinator, WeatherPanel and tests. That is better than the earlier state, but major survival systems do not appear to consume the seasonal authority directly.

A nuclear-winter season model should affect more than weather weights and a label.

### Expected mechanical consumers

Shelter heating demand, crop growth, food spoilage/cold preservation, disease/respiratory risk, wildlife/migration, expedition travel, water freezing/thaw behavior and fuel demand.

### Fix plan

1. Expose one `ICampaignSeason` read model.
2. Ban subsystem-local day-range season inference.
3. Author a season-effect matrix.
4. Each affected subsystem reads modifiers from data.
5. Add boundary-day tests around every season transition.
6. Add one 365-day deterministic matrix test.

### Acceptance tests

At least six retained systems exhibit a measurable season-dependent state difference under identical non-season inputs.

---

## FX-09 — No difficulty-preset authority is present

**Severity:** P2
**Category:** balance / player configuration
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Current source searches do not identify a canonical `DifficultyPreset`/`DifficultyLevel` authority. Without it, balance tuning is effectively one compulsory mode, and any attempt at easy/hard risks becoming ad-hoc conditionals scattered across systems.

### Fix plan

1. Define JSON `difficulty_presets.json`.
2. Presets contain modifier references, not duplicated whole catalogs.
3. Supported axes: resource scarcity, need decay, hazard intensity, disease severity, combat risk, economic pressure and recovery generosity.
4. Persist only preset ID plus explicit custom overrides.
5. Core systems receive normalized tuning, not the enum itself.
6. Add deterministic paired-seed comparisons.

### Acceptance tests

Changing preset changes declared modifiers only. Same preset + same seed gives identical simulation. Save/load preserves preset.

---

## FX-10 — Keyboard focus/navigation policy is not broadly applied

**Severity:** P1
**Category:** accessibility / input
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Controller button registration has improved, but current searches for explicit `FocusMode` implementation primarily surface the accessibility selftest and modal logic rather than a broad per-panel focus policy.

Controller bindings alone do not make a 100+ panel UI operable. Controls need focusability, deterministic reading order and focus restoration.

### Fix plan

1. Put focus defaults in shared UI factories: interactive controls focusable, decorative controls not focusable.
2. Define consistent tab/navigation order.
3. Ensure modals capture focus, trap focus intentionally and return focus to opener on close.
4. Add directional focus for grids/lists.
5. Add visible focus styling independent of color.
6. Exercise the top 20 gameplay panels keyboard-only and controller-only.
7. Expand `UiAccessibilitySelfTest` from linting to a real traversal test.

### Acceptance tests

A full day-1 journey can be completed without mouse input. No modal leaves focus lost. Every interactive control in live routed panels is reachable.

---

## FX-11 — Bare `InventoryHostSession` silently creates a demo catalog

**Severity:** P1
**Category:** data authority / test fidelity
**Confidence:** CURRENT-CONFIRMED
**Primary file:** `src/Host/InventoryHostSession.cs`

### Specific problem

The constructor accepts an optional catalog and, when that catalog is empty, calls a large hardcoded `SeedCatalog(...)`. The production factory `Create(dataDir)` uses JSON, but bare construction creates a different item universe without making that fact explicit to the caller.

### Fix plan

1. Make normal constructor require a catalog.
2. Add `CreateForFixture()` for tests only.
3. Rename seed helper `SeedCatalogForTest`.
4. Mark fixture API internal or otherwise clearly non-production.
5. Add source gate forbidding bare `new InventoryHostSession()` outside approved fixture/selftest paths.
6. Mandatory campaign tests use `Create(dataDir)`.

### Acceptance tests

Attempting production construction without authority fails loudly. Test fixture creation states its fixture nature in code.

---

## FX-12 — Missing starting-supply JSON silently falls back to literals

**Severity:** P1
**Category:** silent fallback / data authority
**Confidence:** CURRENT-CONFIRMED

### Specific problem

`LoadOrSeedStartingSupplies` falls back to `SeedStartingSupplies()` when authored starting-supply data is absent/empty. This converts “required data is broken” into “game starts with a plausible-looking alternative inventory.”

### Fix plan

1. Classify starting supplies as required beta data.
2. In production strict mode: missing file, malformed file or empty required list is a startup failure.
3. Keep fallback only for explicit test fixture mode.
4. Make fallback values generated from a checked-in fixture, not hand-maintained duplicate literals.
5. Emit actionable diagnostic with resolved path.

### Acceptance tests

Delete/rename starting supply file in an isolated artifact: boot must fail with one clear error rather than silently seed.

---

## FX-13 — Demo seed catalog contains duplicate item IDs

**Severity:** P1
**Category:** fixture correctness / duplicate authority
**Confidence:** CURRENT-CONFIRMED

### Specific problem

The current hardcoded seed catalog defines several seed item IDs more than once in separate themed blocks, including mushroom/tuber/grain/wheat seed IDs. Depending on catalog registration semantics, later entries may overwrite earlier entries or duplicate registration may fail/behave differently.

### Fix plan

1. Add uniqueness assertion before fixture registration.
2. Delete duplicate hand-authored fixture definitions.
3. Prefer generating the fixture from authoritative JSON with a whitelist of IDs.
4. Add parity hash for fixture fields used in tests.
5. Fail fixture construction on duplicate ID.

### Acceptance tests

Fixture ID count equals distinct ID count. Order reversal does not alter resulting definitions.

---

## FX-14 — Multiple selftests still instantiate the demo inventory host

**Severity:** P1
**Category:** test fidelity
**Confidence:** CURRENT-CONFIRMED

### Current examples

Current search still finds bare construction in:

- `src/Host/HostCli.Onboarding.cs`
- `src/Host/InventorySaveSelfTest.cs`
- `src/Main.UiTests.Inventory.cs`
- `src/Host/PanelBindLifecycleSelfTest.cs`
- `src/Host/HostCli.PanelTests.cs`

Because bare construction seeds the demo catalog, these tests can pass against data that the shipped campaign does not load.

### Fix plan

1. Split tests into `FIXTURE_UNIT` and `SHIPPED_DATA_INTEGRATION`.
2. Rename all intentional fixture construction.
3. Replace mandatory integration gates with real tracked data directory.
4. Add one test comparing critical fixture item fields against `items.json`.
5. Report in test summaries whether authority or fixture was used.

### Acceptance tests

No release gate can pass solely through demo inventory data.

---

## FX-15 — Utility-AI UI smoke expects 4 actions while JSON defines 6

**Severity:** P1
**Category:** test/data contradiction
**Confidence:** CURRENT-CONFIRMED

### Specific problem

Current `Main.UiTests.UtilityAi.cs` checks `_utilityAi.Actions.Count == 4`.

Current `utility_actions.json` defines six actions:

1. `action_weigh_goods`
2. `action_read_contract`
3. `action_canvas_support`
4. `action_run_vouch`
5. `action_audit_inventory`
6. `action_file_report`

This is a direct contradiction between data authority and smoke-test expectation.

### Fix plan

1. Remove hardcoded count assertion.
2. Assert authoritative expected IDs, ideally derived from parsed JSON.
3. Test unique IDs and required fields.
4. Make UI smoke verify all loaded actions render.
5. If only four actions are intended for UI, encode that as an explicit `visible_in_ui` property rather than assuming count.

### Acceptance tests

Changing the catalog deliberately changes the expected UI through data, without editing a magic count.

---

## FX-16 — Linux export “data verification” can pass with no data

**Severity:** P0
**Category:** CI/export correctness
**Confidence:** CURRENT-CONFIRMED
**Primary file:** `.github/workflows/build.yml`

### Specific problem

The Linux workflow exports the binary and then runs a “Verify export includes data authority” step using conditional `if [ -f ... ]` / `if [ -d ... ]` blocks. If neither expected PCK nor loose data directory is present, the step can still complete successfully.

### Fix plan

1. Determine one canonical shipped-data layout.
2. Assert it with `test -f` / `test -d`.
3. Count JSON files in source and artifact.
4. Compare a manifest/hash.
5. Point `ASHFALL_DATA` at exported artifact and run data-integrity selftest.
6. Headless-boot the exported binary.
7. Fail if data exists in multiple ambiguous locations.

### Acceptance tests

A deliberately removed catalog causes the export job to fail.

---

## FX-17 — Windows export verifies executable only, not data or boot

**Severity:** P0
**Category:** CI/export correctness
**Confidence:** CURRENT-CONFIRMED

### Specific problem

The current Windows job proves `ashfall.exe` exists. It does not prove data authority exists, PCK/layout is correct, startup can resolve catalogs, or the artifact can execute far enough to selftest.

### Fix plan

1. Mirror Linux artifact manifest validation.
2. Assert PCK/loose data count.
3. Validate catalog manifest without requiring Windows execution when necessary.
4. Prefer a Wine/headless smoke when stable in CI.
5. At minimum run a cross-platform artifact inspector over the Windows output.

### Acceptance tests

Windows build fails if data is omitted even when the executable exists.

---

## FX-18 — CI bypasses the more complete canonical export wrapper

**Severity:** P1
**Category:** build reproducibility
**Confidence:** CURRENT-CONFIRMED

### Specific problem

The repository contains `scripts/ci/godot-export-linux.sh` with staging and verification logic, while current GitHub Actions performs raw `godot --export-release`. That creates two artifact-production procedures which can drift silently.

### Fix plan

1. Make one script the single export authority.
2. CI calls the script.
3. Local release commands call the same script.
4. Add `--verify-only` mode.
5. Add a test for script assumptions, including `.gdignore` and staged paths.
6. Version the artifact-layout contract.

### Acceptance tests

Local and CI exports produce the same file manifest for the same commit.

---

## FX-19 — Catalog-path resolver is bypassed at multiple call sites

**Severity:** P0/P1
**Category:** exported-build compatibility
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

The repository already has a capable `CatalogPath` resolver for environment overrides, executable-relative paths, `res://`, CWD and PCK-aware IO. Yet multiple call sites route around it with literal or custom paths.

A particularly dangerous shape is passing `res://...` to BCL `System.IO` through `FileSystemIO`, which cannot read virtual PCK resources.

### Known risky areas to rehome

- `EventsHostSession`
- faction branch loading
- Holdfast terminal data loading
- radio data loading
- selected selftests

### Fix plan

1. All catalog loads call one resolver.
2. Resolver returns resolved directory/path and appropriate `IFileIO`.
3. Ban literal `StreamingAssets/Data` paths outside resolver/tests.
4. Ban `Directory.GetCurrentDirectory()` as catalog authority.
5. Add exported-PCK path tests.
6. Add a forbidden-path CI scanner.

### Acceptance tests

Editor, source-headless, loose exported data and PCK-only modes all resolve the same catalog ID set.

---

## FX-20 — `OpenPlayerPanel` keeps a giant legacy fallback switch

**Severity:** P1
**Category:** orchestration bloat / silent wiring mask
**Confidence:** CURRENT-CONFIRMED
**Primary file:** `src/Main.GameFlow.cs`

### Specific problem

Current code resolves a typed panel descriptor and explicitly says a missing `OpenAction` is a bug—but after logging that bug it still enters a large legacy `switch(panelId)` that manually sets up and opens many panels.

This fallback makes missing registry wiring survivable. An unregistered/misregistered panel should fail a gate, not quietly work through a second path.

### Fix plan

1. Generate list of panels still relying on fallback.
2. Migrate each to descriptor actions.
3. Add parity test: every live descriptor has bind/open/close action as required.
4. Replace production fallback with an error diagnostic and no open.
5. Keep optional developer diagnostic route in a separate debug-only file if needed.
6. Delete switch once count reaches zero.

### Acceptance tests

`OpenPlayerPanel` contains no domain-specific panel setup switch.

---

## FX-21 — Legacy panel fallback fabricates “first” or placeholder context

**Severity:** P1
**Category:** UI correctness
**Confidence:** CURRENT-CONFIRMED

### Specific problem

Within the legacy fallback path, detail views can select the first available survivor, a hardcoded item such as `bandage`, or an empty ID when no survivor exists. A detail screen should display the entity the player selected—not whichever entity happened to appear first.

### Fix plan

1. Introduce typed navigation context: `PanelRouteRequest(panelId, entityId, sourcePanelId)`.
2. Detail panels require a valid target ID.
3. Missing target renders explicit empty/no-selection state.
4. No hardcoded item/survivor fallback.
5. Return navigation preserves source selection.
6. Add route-context tests for survivor/item/quest/location details.

### Acceptance tests

Opening survivor B cannot display survivor A due to list ordering.

---

## FX-22 — Persisted logs have no retention policy

**Severity:** P1
**Category:** long-session durability / save growth
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Multiple persisted collections naturally grow over time—serving logs, decrees, journal entries, memorial/census/history records—and there is no broad retention/compaction policy visible in current source. A display cap is not a storage cap.

### Fix plan

1. Inventory every persisted list.
2. Classify permanent history, bounded recent history, aggregate-only and recomputable data.
3. Introduce `RollingLog<T>` for bounded histories.
4. Store aggregates separately where full event history is unnecessary.
5. Define per-list retention in data/config.
6. Add save-size budgets at 30, 365, 3,650 and extreme campaign days.
7. Migrate old saves without losing required semantic records.

### Acceptance tests

400-year synthetic campaign remains within documented save-size and load-time budgets.

---

## FX-23 — Seven-day deterministic smoke exists but is not mandatory

**Severity:** P1
**Category:** CI quality
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

`SevenDayDeterministicSmokeTest` is substantial and already covers multiple real systems and save/reload gates. It is exposed as a CLI selftest, but current project planning still identifies it as outside the mandatory gate manifest.

The repository's strongest “does the game hold together for a week?” proof is therefore optional.

### Fix plan

1. Register it in mandatory CI gate manifest.
2. Run on every main/PR where runtime-affecting paths change.
3. Upload trace on failure.
4. Run a longer nightly variant.
5. Add exported-artifact version after FX-16/17.

### Acceptance tests

A deliberately broken save/reload or day-owner transition fails required CI.

---

## FX-24 — Performance headline is advisory and statistically weak

**Severity:** P1
**Category:** performance regression control
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

The recorded `day_advance_30d` benchmark has historically used only five iterations and labels its headline result “advisory.” A p95 computed from five samples is not a useful tail statistic. Performance can regress significantly without failing release CI.

### Fix plan

1. Separate fast smoke benchmark and nightly statistically meaningful benchmark.
2. Warm up JIT/import effects.
3. Use at least 30–50 measured iterations for distribution metrics.
4. Record median, p90/p95, allocations, GC collections and max.
5. Establish hardware-normalized or CI-runner-specific budgets.
6. Gate relative regression against baseline plus absolute ceiling.
7. Publish trend artifact by commit.

### Acceptance tests

Inject an intentional 2x slowdown and verify the performance gate fails.

---

## FX-25 — Save durability failure matrix is incomplete

**Severity:** P0/P1
**Category:** persistence / campaign-loss prevention
**Confidence:** COVERAGE-GAP

### Specific problem

The save architecture is much stronger—checksums, atomic envelope writes, slot routing and transaction behavior exist—but release proof still needs failure-path coverage beyond ordinary round trips.

High-value scenarios include disk full/write failure, process termination during write/projection, close-request during save, corrupted primary with valid backup, corrupted backup, cross-slot rapid switching, rename/move failure and partial projection recovery.

### Fix plan

1. Build injectable failing `IFileIO`.
2. Enumerate failure matrix.
3. Assert old valid campaign survives failed save, UI reports failure, dirty state is not falsely cleared and retry can succeed.
4. Add crash-recovery marker tests if projections remain.
5. Exercise `NotificationWMCloseRequest`.
6. Add one real subprocess kill test nightly.

### Acceptance tests

No tested failure mode can turn one valid campaign into zero recoverable campaigns without an explicit unrecoverable-corruption diagnosis.

---

## FX-26 — Repository has no Git tags or GitHub releases

**Severity:** P1
**Category:** release engineering
**Confidence:** CURRENT-CONFIRMED

### Specific problem

Current GitHub API returns no matching tag refs and no GitHub releases. Without an immutable known-good release anchor, rollback, save-compatibility policy and user-facing changelog discipline are much harder to enforce.

### Fix plan

1. Define version axes: game binary, data schema and save schema.
2. Create release checklist.
3. Generate changelog from reviewed commits/PR labels.
4. Cut first beta tag only after required gates pass.
5. Attach artifact hashes and reports.
6. Define hotfix branch/tag procedure.
7. Never rewrite published release tags.

### Acceptance tests

Given a bug report, team can identify exact artifact, commit, data schema and save compatibility from version output.

---

## FX-27 — Historical save corpus is not a committed compatibility gate

**Severity:** P1
**Category:** migration fidelity
**Confidence:** COVERAGE-GAP

### Specific problem

The repository has many migration/unit tests, but current search does not reveal a structured committed corpus of representative historical real-world saves loaded by CI across supported versions. Synthetic DTO tests cannot perfectly reproduce weird legacy saves accumulated through actual gameplay.

### Fix plan

1. Collect sanitized historical save fixtures.
2. Store under `Ashfall.Core.Tests/Fixtures/Saves/`.
3. Document source version, schema version, feature coverage and expected migration digest.
4. Add `load-all-fixtures` fast gate.
5. Add migrate -> save -> reload -> continue 30 days nightly.
6. Keep fixtures immutable; add new ones, do not rewrite history.

### Acceptance tests

Every declared supported save version has at least one fixture that successfully migrates to current.

---

## FX-28 — Balance telemetry exists without declared design targets

**Severity:** P1
**Category:** balance process
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

The repository now has `BalanceTelemetryHarnessTests` writing deterministic CSVs, which fixes the older “no producer” problem. However, measurement alone does not define good balance.

There is still no obvious canonical target document for expected day-7 survival rate, time to first serious scarcity, dose distribution, starvation frequency, economy inflation or first-death window.

### Fix plan

1. Create `docs/balance/TARGETS.md`.
2. Declare ranges, not single magic values.
3. Tie every target to scenario, seed set, metric and design rationale.
4. Make telemetry harness consume checked-in scenario definitions.
5. Add drift report.
6. Gate only mature targets; keep experimental metrics informational.

### Acceptance tests

A tuning PR states which target moved and why. No “looks okay” balance change lands without measured consequence.

---

## FX-29 — Player funnel/session telemetry is absent

**Severity:** P2
**Category:** UX measurement / playtest instrumentation
**Confidence:** CURRENT-STRUCTURAL

### Specific problem

Current searches do not identify a real `PlaySessionRecorder` or equivalent local player-funnel recorder. The code contains isolated instrumentation concepts such as sigils, but not a coherent session journey record.

This makes it difficult to answer where players abandon day 1, which panels are never opened, which warnings are repeatedly ignored, how long first expedition setup takes or whether onboarding reduces errors.

### Fix plan

1. Implement local opt-in `PlaySessionRecorder`.
2. Record semantic events only: panel opened, action attempted/succeeded/failed, day advanced, death, save/load, guidance opened and expedition dispatched.
3. No raw sensitive text.
4. Keep offline/local by default.
5. Add synthetic-player harness using same schema.
6. Produce first-hour funnel report per beta candidate.

### Acceptance tests

One deterministic synthetic session produces a stable event trace. Human-playtest recording can be disabled completely.

---

## FX-30 — Data override/mod surface exists without a compatibility contract

**Severity:** P2
**Category:** accidental public API / data architecture
**Confidence:** ARCHITECTURE-GAP

### Specific problem

`ASHFALL_DATA` already acts as a high-precedence data source; JSON catalogs are schema-versioned and integrity-validated. In practical terms, that is already the beginning of a mod/content-pack surface.

But without a written/generated contract, external data users cannot know which schemas are stable, which IDs may be overridden, precedence/merge semantics, save behavior when a pack disappears, whether adding/removing definitions is compatible, or what constitutes a breaking change.

### Fix plan

1. Decide explicitly whether modding is supported, experimental or internal-only.
2. Generate a data contract from schemas/registry.
3. Specify merge/override rules.
4. Add fixture packs: additive valid, override valid, duplicate invalid, missing dependency and removed-pack save.
5. Add breaking-schema diff detector.
6. Emit effective-content report at boot.
7. If modding is not supported yet, document `ASHFALL_DATA` as developer-only and make no public compatibility promise.

### Acceptance tests

Base game with no pack remains byte/determinism-equivalent. Invalid pack fails before campaign mutation. Removing a supported pack has defined save behavior.

---

# 3. Cross-cutting root causes

The 30 findings fall into seven recurrent failure families.

## A. Literal demo values surviving into authoritative paths

Examples: ending Boolean bundle, survivor-ID radiation, starting-supply fallback, demo catalog and detail-panel placeholder selection.

**Permanent control:** forbid identity/literal fixture data inside production authority adapters unless explicitly declared.

## B. Good subsystem exists, but dependencies do not propagate

Examples: PowerGrid, seasons and commitments.

**Permanent control:** dependency registry + integration journey.

## C. A fallback makes broken configuration look healthy

Examples: inventory demo catalog, starting supplies, panel fallback switch and export conditional verification.

**Permanent control:** strict beta mode where required data/wiring fails closed.

## D. Tests prove a different universe

Examples: demo inventory selftests, UtilityAI 4-vs-6 mismatch, seven-day harness optional and performance advisory.

**Permanent control:** test result declares authority source and artifact under test.

## E. Exported artifact is less verified than source

Examples: Linux data conditionals, Windows executable-only validation, resolver bypass and export-script divergence.

**Permanent control:** exported artifact runs the same data-integrity and journey checks.

## F. Long-lived project state lacks lifecycle policy

Examples: unbounded logs, historical saves not fixtures and no release anchors.

**Permanent control:** retention, compatibility and release policies become checked data.

## G. Product behavior is measured without a target

Examples: balance telemetry, performance samples, no player funnel and no difficulty modes.

**Permanent control:** metrics need explicit decision thresholds or must be labeled exploratory.

---

# 4. Recommended execution sequence

## Fixation Wave 1 — Stop the game from lying

1. FX-01 derived epilogue.
2. FX-02 environmental radiation.
3. FX-03 full power dependency.
4. FX-15 UtilityAI authority mismatch.
5. FX-20 remove panel fallback.
6. FX-21 typed route context.

**Why first:** these are direct player-state correctness problems.

## Fixation Wave 2 — Make tests test the shipped game

1. FX-11 explicit inventory fixture construction.
2. FX-12 strict starting supplies.
3. FX-13 unique fixture catalog.
4. FX-14 convert release selftests to shipped data.
5. FX-23 mandatory seven-day gate.
6. FX-24 real performance budget.

## Fixation Wave 3 — Prove exported artifacts

1. FX-19 one catalog resolver.
2. FX-18 one export wrapper.
3. FX-16 Linux artifact assertions.
4. FX-17 Windows artifact assertions.
5. FX-25 persistence failure matrix.

No public beta before this wave is green.

## Fixation Wave 4 — Make the simulation coherent over time

1. FX-07 commitment ledger.
2. FX-08 seasonal consumers.
3. FX-09 difficulty authority.
4. FX-22 retention policy.
5. FX-27 real save corpus.
6. FX-28 balance targets.

## Fixation Wave 5 — Operability and release discipline

1. FX-04 guidance reachability.
2. FX-05 localization.
3. FX-10 focus/navigation.
4. FX-26 version/tag/release discipline.
5. FX-29 session telemetry.
6. FX-30 data/mod contract.
7. FX-06 WornGear consolidation as lower-risk cleanup.

---

# 5. New CI gates proposed by this report

### `ending-outcome-truth`
Fails if the epilogue route contains literal campaign result parameters or evaluator inputs are not derived from current authorities.

### `environmental-exposure-source`
Fails on survivor-ID-specific zone/dose formula branches.

### `power-dependency-coverage`
Every declared powered system must bind the canonical power interface.

### `shipped-data-selftest`
Release selftests must use real tracked JSON authority.

### `fixture-uniqueness`
All explicit fixture catalogs have unique IDs.

### `utility-ai-authority-parity`
UI expectations are the exact loaded action ID set, not a magic count.

### `exported-data-single-authority`
Exactly one canonical data authority exists in an exported artifact.

### `export-smoke-boot`
Exported binary boots headlessly and loads catalogs.

### `catalog-path-policy`
No literal production `StreamingAssets/Data` path outside `CatalogPath`.

### `panel-registry-no-fallback`
No live player panel depends on `OpenPlayerPanel` switch fallback.

### `route-context-required`
Detail routes require target IDs.

### `retention-budget`
Long-haul save size stays under budget.

### `seven-day-required`
Seven-day deterministic smoke is a mandatory gate.

### `perf-budget`
Measured performance has statistically valid sample count and threshold.

### `save-failure-matrix`
Injected IO failures preserve last valid campaign.

### `historical-save-corpus`
All supported historical fixtures load and continue.

### `balance-target-drift`
Mature balance metrics stay within accepted ranges or require explicit approval.

---

# 6. A 30-finding completion matrix

| ID | Required closure evidence |
|---|---|
| FX-01 | derived-outcome trace + 5 campaign fixtures |
| FX-02 | context-based exposure matrix |
| FX-03 | blackout integration trace |
| FX-04 | reopenable guidance journey |
| FX-05 | pseudo-localization screenshot/gate |
| FX-06 | one `WornGear` definition |
| FX-07 | commitment save/overdue trace |
| FX-08 | season-boundary matrix |
| FX-09 | paired-seed difficulty report |
| FX-10 | keyboard/controller day-1 journey |
| FX-11 | no implicit demo construction |
| FX-12 | missing-data failure test |
| FX-13 | fixture unique-ID proof |
| FX-14 | shipped-data test manifest |
| FX-15 | 6 authoritative UtilityAI IDs rendered |
| FX-16 | Linux exported-data boot proof |
| FX-17 | Windows artifact data proof |
| FX-18 | one export path |
| FX-19 | resolver source gate |
| FX-20 | fallback switch deleted |
| FX-21 | typed route-context tests |
| FX-22 | 400-year save-size report |
| FX-23 | gate manifest includes seven-day smoke |
| FX-24 | thresholded benchmark report |
| FX-25 | failure-injection persistence report |
| FX-26 | immutable beta tag + release manifest |
| FX-27 | historical save fixtures + digest |
| FX-28 | documented balance target report |
| FX-29 | deterministic funnel trace |
| FX-30 | generated content-pack contract or explicit unsupported policy |

---

# 7. Flagship beta stop-ship subset

If the goal is a serious beta rather than internal experimentation, treat the following as **stop-ship** until resolved or consciously waived:

- FX-01 — ending truth
- FX-02 — radiation truth
- FX-03 — power/life-support propagation
- FX-14 — selftests using wrong data universe
- FX-15 — UtilityAI test/data contradiction
- FX-16 — Linux export verification
- FX-17 — Windows export verification
- FX-19 — catalog resolver bypass
- FX-23 — seven-day journey not mandatory
- FX-25 — save durability failure matrix

The remaining issues can be sequenced around beta scope, but none should disappear into an indefinite “later” list. Give each an owner, target release and explicit disposition.

---

# 8. Final recommendation

The repository continues to show the same high-level risk in increasingly specific forms:

**the implementation is often stronger than the proof that the implementation being tested, exported, displayed and saved is the same implementation the player actually uses.**

This second remediation pass should therefore optimize for **identity of authority**:

- the ending reads the real campaign;
- radiation reads the real world;
- powered systems read the real grid;
- tests read the real data;
- CI inspects the real exported artifact;
- detail panels read the real selected entity;
- benchmarks have real thresholds;
- compatibility tests read real historical saves.

Do not add another broad mechanics wave until the P0/P1 items above have dispositions. The highest return is not more subsystem code. It is making each existing subsystem impossible to substitute, bypass, fake, silently default, or validate against the wrong universe.
