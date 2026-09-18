# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 1 master plan for Generation Wave 12, derived from the verified post-Wave 11 unclaimed corpus ledger and dependency DAG.

**Purpose:** convert the eight highest-priority unsealed architecture and content-rail plans beyond the Wave 11 frontier into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Premise:** Waves 8 through 11 stabilized the foundation: Plan 24 labor/needs was closed-with-deferrals; Wave 9 sealed crisis prediction, cloud seeding, trophies, +30% economy/trade content, Plan 31 SemanticKind, and audio ducking; Wave 10 established corpus census truth, test rigor, and release pipelines; Wave 11 settled year-turn chronologies, memory graphs, content acceptance gates, and geographic travel. Wave 12 directly attacks the **operational presentation, depth-content rails, spatial holdfast view, input/focus reality, session durability, authored identity, deterministic voice, and longitudinal medicine**.

**Part 1 Execution Set (Exactly 8 Tasks):**
1. **Task A1 — C1[16] Depth Passes: Dead-Bucket Content Rails (Plan 49A/49B/49C)**
2. **Task A2 — C1[17] The Presented Game: Spatial Holdfast & Wasteland Map Bounded State (Plan 51A/51B/51C)**
3. **Task A3 — C1[18] Weight & Hygiene: Asset Budgets, LFS Retention & Reproducible Tooling (Plan 56A/56B/56C)**
4. **Task A4 — C1[19] Medicine Made Legible: Disease Vectors, Dependency Care & Palliative Continuity (Plan 60A/60B/60C)**
5. **Task B1 — C2[15] Input Reality, Focus Navigation, Controller Parity & Legibility (Plan 37A/37B/37C)**
6. **Task B2 — C2[16] Session Durability, 200-Hour Soak & Player-Safe Save Recovery (Plan 39A/39B/39C)**
7. **Task B3 — C2[17] Authored Survivor Identity, Declared Item Tags & Diegetic Personality (Plan 40A/40B/40C)**
8. **Task B4 — C2[18] Deterministic Survivor Voice, Delivery Contracts & Social Speech (Plan 42A/42B/42C)**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 1 must reach one of these terminal states:

- **IMPLEMENTED** — Missing mechanism, consumer, contract, or surface is authored, wired to canonical owners, and verified green across all required test gates.
- **DECIDED-DEFERRED** — Product/architecture authority requires a signature; an implementation-selective decision memo is signed and recorded, leaving the remainder explicitly deferred without claiming false completion.
- **RETIRED** — An obsolete or duplicate file/shim/mechanism is deleted and unregistered with reference proof and architecture map regeneration.
- **VERIFIED-RESOLVED** — Re-verification at repository `HEAD` proves the blocker was already sealed by concurrent work; evidence is recorded and redundant implementation is skipped.
- **ROUTED-REPAIR** — Investigation exposes a genuine production defect outside the task's bounded scope; an isolated repair package with reproducible characterization test is registered.

## 0.2 Non-Negotiable Hard Rules

1. **Godot is authoritative; Unity is retired.** No Unity dependencies, shims, or references may be added.
2. **Core stays engine-free.** `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot or engine types.
3. **JSON data is authoritative.** Authoritative data resides in `Assets/StreamingAssets/Data/` with valid schema policy.
4. **Preserve determinism and persistence.** No `System.Random` or unseeded wall-clock RNG in Core domain logic.
5. **One authority per concern.** Never create duplicate registries, parallel save stores, or shadow managers.
6. **Claims before edits.** Check and record file path claims in `WORKTREE_OWNERSHIP.md` before touching code.
7. **Substeps are instructions, not tasks.** The 20 substeps per task represent ordered procedural instructions.
8. **Mini-tasks require 4 mini-substeps.** Any mini-task (e.g. `.1`, `.2`) must contain exactly 4 subsequent execution instructions.
9. **Focused testing first.** Use `scripts/run_test.sh` for bounded xUnit runs; do not run broad suites unprompted.
10. **Zero warning tolerance.** Production code edits must maintain a 0-error, 0-warning baseline on build.

---

# 1. DETAILED TASK SPECIFICATIONS

## TASK A1 — C1[16] Depth Passes: Dead-Bucket Content Rails (Plan 49A/49B/49C)

- **Source Plan:** `C-integration-plans/C1_planintegration[16].md` (Plan 49)
- **Blocker Class:** UNCONSUMED CONTENT / REACHABILITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Narrative/`, `Assets/Ashfall.Core/Quests/`
- **Target Subsystem:** Orphan content sweep, encounter/quest hookup, dead-bucket reachability

### 20 Procedural Substeps:
1. Re-verify the current content utilization baseline using `godot --headless --path . -- --content-utilization-selftest`.
2. Inspect `Assets/StreamingAssets/Data/` narrative files (`encounters.json`, `events.json`, `radio.json`, `quests.json`) for unreferenced IDs.
3. Cross-reference unreferenced rows against `docs/data/CATALOG_REGISTRY.md` to identify genuinely dead versus expansion-reserved entries.
4. Verify that no new gameplay subsystem or second choice resolver is created; content must route through existing authorities.
5. For narrative encounter fragments lacking a trigger edge, connect them to the canonical `DayEventVocabulary` semantic dispatch.
6. For item recipes with zero crafting consumers, register them in `recipes.json` under existing workstation tags.
7. Implement validation in `CatalogIntegrityValidator.cs` to ensure all encounter preconditions reference existing flag IDs.
8. Check that all quest completion rewards resolve to valid IDs in `items.json` or active currency pools.
9. Verify that radio distress signals in `radio_distress_signals.json` with dead stages are properly bound to `DistressStageResolver`.
10. Wire orphaned companion rescue events to the `WildlifeTrappingSystem` and `CompanionAnimalSystem` discovery tables.
11. Ensure that moral choice dilemmas without a registered outcome record map to valid faction standing or relationship deltas.
12. Audit dialog strings and event text to confirm strict adherence to ASHFALL's restrained, grounded tone.
13. Verify that all newly reachable content items have valid `schema_version: 1` declarations and pass schema validation.
14. Add unit test coverage in `Ashfall.Core.Tests/Narrative/Plan49DepthPassTests.cs` validating that every unblocked row is reachable.
15. Verify that save/restore preserves newly active quest and encounter stage states without creating new save sections.
16. Run deterministic replay tests asserting that identical campaign seeds produce identical encounter selection distributions.
17. Run `--data-integrity-selftest` to prove zero validation errors across all 333+ data catalogs.
18. Run `--content-utilization-selftest` and record the reduction in orphan content count.
19. Update `docs/data/CATALOG_REGISTRY.md` and `docs/INDEX.md` with the new reachability metrics.
20. Hand off the task with before/after utilization diffs, test evidence, and zero compile warnings.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### A1.1 Encounter Dead-Bucket Reconnection
- (a) Query all encounter rows in `encounters.json` that lack inbound links from campaign event triggers.
- (b) Map orphaned weather-related encounters to the corresponding `WeatherKind` transition events in `WeatherSystem`.
- (c) Connect medical hazard encounters to the disease outbreak dispatch in `DiseaseQuarantineCoordinator`.
- (d) Author characterization unit tests asserting that triggered conditions fire the previously dormant encounters.

#### A1.2 Orphan Item & Recipe Activation
- (a) Enumerate all items in `items.json` that appear in zero recipe ingredients, drops, or shop inventories.
- (b) Assign missing items to appropriate regional shop tiers or salvage breakdown tables in `recipes.json`.
- (c) Verify that newly added recipe bills strictly enforce existing workstation tier and tool requirements.
- (d) Run `CatalogIntegrityValidator.cs` in test harness to guarantee zero dangling item or recipe identifiers.

#### A1.3 Radio Broadcast Stage Binding
- (a) Trace all broadcast frequencies in `radio.json` that terminate prematurely without triggering stage progressions.
- (b) Wire terminal broadcast stages to emit journal discovery events through `JournalSystem`.
- (c) Ensure follow-up broadcast scheduling adheres to the validated `DistressFollowUpScheduler` grammar.
- (d) Execute `RadioDistressSystemTests` to confirm stage progression and journal entry parity.

#### A1.4 Content Integrity & Metrics Gate
- (a) Execute the automated content utilization scanner to calculate the percentage of reachable authored assets.
- (b) Validate that no circular quest prerequisites or impossible trigger flags were introduced.
- (c) Assert that all modified JSON documents conform to the UTF-8 without BOM encoding standard.
- (d) Regenerate `artifacts/asset_registry.json` and verify zero missing asset references.

---

## TASK A2 — C1[17] The Presented Game: Spatial Holdfast & Wasteland Map Bounded State (Plan 51A/51B/51C)

- **Source Plan:** `C-integration-plans/C1_planintegration[17].md` (Plan 51)
- **Blocker Class:** PRESENTATION GAP / READ-MODEL VIEW BOUNDING
- **Canonical Owner:** `src/UI/`, `src/Host/`
- **Target Subsystem:** Spatial shelter room representation, wasteland map node views, campaign state binding

### 20 Procedural Substeps:
1. Review `src/Main.cs` and existing room panels to establish current presentation boundaries.
2. Confirm the core invariant: spatial views are pure projections of domain state and never maintain independent simulation.
3. Claim `src/UI/HoldfastSpatialPresenter.cs`, `src/UI/WastelandMapView.cs`, and corresponding host session seams.
4. Implement `HoldfastSpatialPresenter.cs` as a bounded Node2D/Control composite mapping shelter grid coordinates to rooms.
5. Bind shelter room visual states (Normal, Damaged, Flooded, Powerless) strictly to `ShelterRoomSystem` events.
6. Connect room occupancy indicators to `DutyRosterSystem` assignments, rendering active worker counts without polling.
7. Ensure all room interaction hotspots route commands directly through `ShelterHostSession` without panel-side math.
8. Implement `WastelandMapView.cs` as a discrete vector/node display rendering canonical locations from `LocationRegistry`.
9. Bind travel route connections on the wasteland map to `ExpeditionRouteSystem` distance and terrain records.
10. Render active expedition tokens on the wasteland map using `ExpeditionState.ProgressFraction` projections.
11. Subscribe view updates to `OnExpeditionAdvanced` and `OnExpeditionCompleted` domain events; eliminate per-frame processing.
12. Ensure keyboard navigation permits focusing every interactive room hotspot and map node sequentially.
13. Implement high-contrast visual focus rings adhering to `DESIGN.md` guidelines for 1920x1080 fixed layout.
14. Implement proper lifecycle disposal in both views: unbind all domain event subscriptions on `_ExitTree`.
15. Add headless integration tests in `Ashfall.Core.Tests/UI/SpatialViewLifecycleTests.cs` testing instantiate/open/close cycles.
16. Verify that closing and reopening spatial views ×100 in `PanelBindLifecycleSelfTest.cs` leaks zero event handlers.
17. Ensure neither view introduces any new save file sections, reading exclusively from existing campaign save stores.
18. Test deterministic view state reconstruction after a save/load cycle across mid-expedition saves.
19. Run `--panel-bind-lifecycle-selftest` and `--ui-a11y-selftest` to ensure 100% gate compliance.
20. Document view architecture in `docs/ui/UI_PANEL_ARCHITECTURE_GUIDE.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### A2.1 Holdfast Room Node Binding
- (a) Construct the 2D room cell layout based on canonical shelter room grid definitions in `shelter_rooms.json`.
- (b) Create visual status overlays for environmental hazards (radiation leak, electrical arc, flood level).
- (c) Wire the room click/activate handler to dispatch `OpenRoomDetailCommand` to the central UI router.
- (d) Verify in test harness that room status changes in Core instantly trigger visual state transitions.

#### A2.2 Wasteland Map Route Projection
- (a) Parse canonical world locations into discrete vector coordinates with fixed bounding box boundaries.
- (b) Draw route vectors between discoverable nodes with thickness and color indicating hazard/travel tier.
- (c) Connect node click events to open `ExpeditionPreparationPanel` with the target location pre-selected.
- (d) Validate that undiscovered locations remain hidden or fog-masked according to `WorldExplorationSystem` flags.

#### A2.3 Event-Driven Refresh Discipline
- (a) Unsubscribe any polling or timer-based refresh loops from the spatial presentation nodes.
- (b) Wire `OnPowerGridStateChanged` to update electrical blackout visual shaders across all affected rooms.
- (c) Wire `OnWaterSpillUpdated` to update room drainage and contamination decals.
- (d) Verify through profiler metrics that idle spatial views consume zero CPU cycles when no events fire.

#### A2.4 Accessibility & Navigation Integration
- (a) Assign explicit `focus_neighbor_*` paths between adjacent shelter rooms and wasteland map nodes.
- (b) Implement gamepad/arrow key traversal allowing comprehensive navigation without mouse input.
- (c) Ensure all state-dependent color indicators are paired with explicit text labels or iconic glyphs.
- (d) Run `--ui-a11y-selftest` to verify focus reachability and contrast ratio compliance across all nodes.

---

## TASK A3 — C1[18] Weight & Hygiene: Asset Budgets, LFS Retention & Reproducible Tooling (Plan 56A/56B/56C)

- **Source Plan:** `C-integration-plans/C1_planintegration[18].md` (Plan 56)
- **Blocker Class:** REPOSITORY BLOAT / TOOLING HYGIENE
- **Canonical Owner:** `scripts/ci/`, `.gitattributes`, `docs/ci/`
- **Target Subsystem:** Working tree mass, Git LFS verification, asset budget enforcement, clean tooling

### 20 Procedural Substeps:
1. Measure the current repository disk footprint by directory (`.godot`, `artifacts`, `docs`, `assets`).
2. Verify all Git LFS tracked patterns in `.gitattributes` against active binary assets (PNG, WAV, OGG, TTF).
3. Audit the repository for stray build artifacts, temporary log files, or uncommitted Unity-era remnants.
4. Execute `scripts/ci/uid-sidecar-gate.sh` to confirm every `.cs` source file has a matching `.cs.uid` file.
5. Create `scripts/ci/check_asset_budgets.py` to enforce per-asset and per-category size limits (audio < 2MB, textures < 4MB).
6. Verify that imported Godot `.import` files correctly match source files and do not point to deleted assets.
7. Audit `artifacts/asset_registry.json` for orphaned entries pointing to non-existent disk paths.
8. Re-run `scripts/ci/generate-asset-registry.py` with `--check` flag to verify generator purity.
9. Inspect `docs/INDEX.md` and ensure all 2,400+ indexed documentation files exist and have valid frontmatter.
10. Check that no API keys, credentials, or private configurations exist in any committed text or JSON file.
11. Audit `.gitignore` to guarantee temporary IDE files, test outputs, and crash logs are safely excluded.
12. Verify that `scripts/run_test.sh` strictly enforces the 180-second timeout and passes flags properly.
13. Implement an automated check verifying that no file in `Assets/Ashfall.Core/` imports Godot or engine assemblies.
14. Ensure all shell scripts in `scripts/` have executable permissions (`+x`) and clean bash syntax.
15. Verify that `scripts/ci/verify-fast.sh` executes all tier-1 verification gates cleanly in a single pass.
16. Measure before-and-after repository clone weight and document clean checkout steps.
17. Ensure pre-commit hook scripts in `.githooks/` or `.git/hooks/` enforce JSON schema policy and whitespace checks.
18. Validate that `CatalogIntegrityValidator.cs` executes in under 2.0 seconds during headless boot.
19. Run full `./scripts/ci/verify-fast.sh` to prove that hygiene checks pass without regressing build or tests.
20. Document repository hygiene policy in `docs/tools/REPOSITORY_HYGIENE_GUIDE.md` and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### A3.1 Git LFS & Binary Tracking Audit
- (a) Inspect `.gitattributes` to verify all binary extensions (`*.png`, `*.wav`, `*.ogg`, `*.ttf`, `*.pck`) are LFS-declared.
- (b) Scan git history for improperly committed binary blobs exceeding 500KB stored directly in git object database.
- (c) Verify that `git lfs status` reports clean tracking across all working directory assets.
- (d) Author an automated CI script to fail if any binary asset is added without matching LFS attributes.

#### A3.2 UID Sidecar Integrity Sweep
- (a) Run `scripts/ci/uid-sidecar-gate.sh` across all C# source files in `src/` and `Assets/Ashfall.Core/`.
- (b) Identify any orphaned `.cs.uid` files where the corresponding `.cs` file has been renamed or deleted.
- (c) Automatically generate missing `.cs.uid` files using Godot's canonical UID assignment algorithm.
- (d) Verify the UID gate passes with 0 missing and 0 dangling sidecar files.

#### A3.3 Asset Size & Budget Gate
- (a) Establish maximum size thresholds: UI icons <= 128KB, textures <= 4MB, SFX <= 1MB, ambient music <= 5MB.
- (b) Script a validation runner `scripts/ci/asset_budget_check.sh` scanning `assets/` against these thresholds.
- (c) Identify any oversized assets and log optimization recommendations without downsampling active art.
- (d) Integrate the asset budget check into the permanent verification suite.

#### A3.4 Core Engine-Free Barrier Verification
- (a) Create a static analysis script scanning all `.cs` files in `Assets/Ashfall.Core/` for forbidden references.
- (b) Assert zero occurrences of `using Godot;`, `using UnityEngine;`, or engine-specific attributes.
- (c) Assert zero references to Godot node or scene lifecycle methods (`_Ready`, `_Process`, `Node2D`).
- (d) Ensure the barrier check runs as an automated gate in `verify-fast.sh`.

---

## TASK A4 — C1[19] Medicine Made Legible: Disease Vectors, Dependency Care & Palliative Continuity (Plan 60A/60B/60C)

- **Source Plan:** `C-integration-plans/C1_planintegration[19].md` (Plan 60)
- **Blocker Class:** MEDICAL CONTINUITY / CAUSAL TRANSMISSION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Medical/`, `Assets/Ashfall.Core/Dose/`
- **Target Subsystem:** Vector transmission, addiction/dependency lifecycle, palliative comfort, medical legibility

### 20 Procedural Substeps:
1. Re-verify the current disease baseline: 15 diseases in `diseases.json` across 4 vectors (water, air, blood, spore).
2. Trace the infection pipeline from source contact through incubation, active illness, and convalescence in `MedicalWardSystem`.
3. Ensure no parallel disease runtime or shadow medical system is created; extend existing `MedicalWardSystem`.
4. Connect contaminated water consumption directly to waterborne disease infection checks in `Inventory.Consume`.
5. Connect airborne dust storms and toxic fallout weather events to respiratory infection checks in `WeatherSystem`.
6. Implement disease incubation tracking with discrete, reproducible progression ticks tied to campaign days.
7. Connect chemical dependency progression to daily dose history in `DoseLedgerSystem` for medical treatments.
8. Implement withdrawal symptom triggers in `NeedsSystem` when dependent survivors miss scheduled doses.
9. Wire palliative comfort care actions into `MedicalWardSystem` for terminal patients, mitigating shelter morale penalties.
10. Ensure medical treatments consume authoritative medicine items (`antibiotics`, `rad_cleanser`, `antiviral_dose`).
11. Implement diagnostic clarity: `MedicalWardHostSession` exposes typed infection stage, vector, and effective treatment.
12. Bind `MedicalWardPanel.cs` to display infection etiology, incubation horizon, and recommended pharmaceutical courses.
13. Implement quarantine isolation rules: contagious airborne infections prevent assignment to shared kitchen/bunk duty.
14. Ensure clinical state changes emit canonical day events (`OnSurvivorInfected`, `OnSurvivorRecovered`, `OnDependencyFormed`).
15. Add comprehensive unit tests in `Ashfall.Core.Tests/Medical/Plan60MedicalContinuityTests.cs` covering all 15 diseases.
16. Validate deterministic infection resolution: identical seeds produce identical contagion spreads under fixed exposure.
17. Verify that disease save state restores cleanly across mid-incubation and mid-treatment save/load cycles.
18. Run `--data-integrity-selftest` to ensure `diseases.json` and treatment recipes validate without warnings.
19. Run focused medical test suites to confirm zero regressions in existing health and triage systems.
20. Update `docs/systems/SURVIVOR_STATE_AUTHORITY_MATRIX.md` with complete medical ownership and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### A4.1 Causal Vector Transmission Hookup
- (a) Wire unboiled water drinking events to roll infection against `waterborne_pathogen_index` using seeded RNG.
- (b) Wire toxic fog and particulate storm exposure to roll inhalation risks when masks are unequipped or degraded.
- (c) Wire combat laceration and surgical procedures to roll bloodborne infection chances based on sanitization level.
- (d) Author unit tests asserting that proper protective gear (HEPA filter, boiled water) strictly prevents vector transmission.

#### A4.2 Chemical Dependency & Withdrawal Loop
- (a) Extend `DoseLedgerSystem` to track consecutive days of narcotic and strong anti-rad pharmaceutical administration.
- (b) Implement dependency threshold checks triggering the `ChemicalDependency` condition when doses exceed safety bands.
- (c) Route missed-dose withdrawal effects into `NeedsModifierStack` as named fatigue, nausea, and agitation penalties.
- (d) Author unit tests validating safe tapering schedules versus cold-turkey withdrawal journeys.

#### A4.3 Palliative Care & Terminal Comfort
- (a) Add a palliative care protocol in `MedicalWardSystem` for afflictions with zero surviving treatment paths.
- (b) Wire comfort administration (herbal balm, analgesics) to suppress pain debuffs and prevent despair morale cascades.
- (c) Connect palliative patient status to the `MemorialSystem` so deaths under hospice care produce attenuated grief.
- (d) Validate that palliative resource consumption records properly in the daily shelter expenditure ledger.

#### A4.4 Medical Ward Panel Diagnostic Projection
- (a) Update `MedicalWardPanel.cs` to present symptoms, suspected vector, and confirmed pathogen classification.
- (b) Add a visual treatment timeline indicating remaining incubation hours and expected recovery curve.
- (c) Expose explicit quarantine toggle buttons that enforce movement restrictions through the duty roster.
- (d) Run `--panel-bind-lifecycle-selftest` to prove the extended medical UI conforms to lifecycle and disposal gates.

---

## TASK B1 — C2[15] Input Reality, Focus Navigation, Controller Parity & Legibility (Plan 37A/37B/37C)

- **Source Plan:** `C-integration-plans/C2_planintegration[15].md` (Plan 37)
- **Blocker Class:** ACCESSIBILITY & INPUT DEFECT / CONTROLLER GAP
- **Canonical Owner:** `src/Host/AshfallInputActions.cs`, `src/Main.GameFlow.cs`, `src/UI/`
- **Target Subsystem:** Centralized input dispatch, focus graph navigation, controller parity, mouse-free gameplay

### 20 Procedural Substeps:
1. Review `src/Host/AshfallInputActions.cs` and `project.godot` input map bindings for all `ashfall_*` action names.
2. Confirm the core input objective: prove a full campaign day is completely playable without touching a mouse.
3. Claim `src/Host/AshfallInputActions.cs`, `src/Main.GameFlow.cs`, and active navigation controllers in `src/UI/`.
4. Audit all declared `ashfall_*` input actions and remove or implement any orphaned actions lacking handlers.
5. Centralize input event handling in `Main.GameFlow.cs` to prevent rogue per-panel input interception.
6. Establish deterministic focus navigation rules: every visible interactive control must have valid focus neighbours.
7. Implement standard spatial focus traversal: `ui_up`, `ui_down`, `ui_left`, `ui_right` map cleanly to grid/list items.
8. Wire `ui_cancel` / `ashfall_back` to consistently dismiss modal dialogs, close panels, and return to the HUD.
9. Implement a visual focus indicator with distinct styling (1px high-contrast border) that never relies on color alone.
10. Ensure focus state is preserved when transitioning between panels and restored to the origin control upon closing.
11. Map gamepad/controller inputs (D-Pad, Left Stick, Face Buttons, Shoulder Triggers) to `ashfall_*` semantic actions.
12. Implement controller deadzones and input smoothing to prevent focus jitter and accidental multiple activations.
13. Implement UI text scaling support (100%, 125%, 150%) that preserves fixed 1920x1080 layout without text clipping.
14. Ensure input help overlays render dynamic glyphs matching the currently active input device (Keyboard vs Controller).
15. Add unit tests in `Ashfall.Core.Tests/UI/InputFocusNavigationTests.cs` verifying focus loops and traversal graphs.
16. Implement a headless automated test simulating a 24-hour campaign cycle solely through synthesized input actions.
17. Verify that opening and closing modals does not cause input focus to drop into background/hidden controls.
18. Run `--ui-a11y-selftest` to ensure all 28+ player-facing surfaces satisfy accessibility and navigation gates.
19. Verify that user input rebinding settings save to disk and restore cleanly without corrupting gameplay keymaps.
20. Document input architecture and control mappings in `docs/ui/INPUT_AND_NAVIGATION_GUIDE.md` and hand off.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### B1.1 Input Map Reconciliation & Cleanup
- (a) Inspect `project.godot` and synchronize all action declarations with the `AshfallInputActions` C# constant class.
- (b) Eliminate duplicated action strings and hardcoded keycode checks scattered across panel source files.
- (c) Add missing gamepad button assignments for secondary actions (`ashfall_inspect`, `ashfall_quick_assign`).
- (d) Author a verification test ensuring that all action identifiers in `AshfallInputActions` exist in the Godot project.

#### B1.2 Deterministic Focus Graph Traversal
- (a) Implement a recursive helper assigning `FocusNeighbor` properties across composite containers automatically.
- (b) Prevent focus escape: configure modal dialog containers to clamp navigation within their bounding hierarchy.
- (c) Guarantee that default focus targets are set automatically when any panel or popup becomes visible.
- (d) Test focus traversal paths across top-tier management panels (`DutyRosterPanel`, `InventoryPanel`, `TradePanel`).

#### B1.3 Gamepad Input Parity & Deadzones
- (a) Wire gamepad left analog stick and directional pad to generate semantic directional navigation pulses.
- (b) Configure radial deadzone thresholds (default 0.25) to filter hardware drift on analog thumbsticks.
- (c) Map controller triggers to tab switching actions in multi-page dashboard panels.
- (d) Verify that controller button prompts automatically display when controller input is detected.

#### B1.4 Mouse-Free Day Simulation Gate
- (a) Create an automated integration test script in `Ashfall.Core.Tests/UI/MouseFreeDayJourneyTests.cs`.
- (b) Simulate key inputs to advance the day, assign a worker, dispense medicine, and review daily briefing.
- (c) Assert that every state change is successfully committed without triggering mouse cursor events.
- (d) Validate that the simulated day completes with identical end-state results to standard UI interactions.

---

## TASK B2 — C2[16] Session Durability, 200-Hour Soak & Player-Safe Save Recovery (Plan 39A/39B/39C)

- **Source Plan:** `C-integration-plans/C2_planintegration[16].md` (Plan 39)
- **Blocker Class:** PERSISTENCE INTEGRITY / LONG-RUN STABILITY
- **Canonical Owner:** `Assets/Ashfall.Core/Save/`, `src/Host/`
- **Target Subsystem:** Long-run deterministic soak, slot recovery, backup rotation, save corruption protection

### 20 Procedural Substeps:
1. Review current save architecture: checksummed stores, atomic write-rename, `.bak` rotation, envelope versioning.
2. Confirm the primary mission: prove campaign stability under 200-hour-class equivalent simulation cycles.
3. Claim `Assets/Ashfall.Core/Save/SaveOrchestrator.cs` and related persistence verification harnesses.
4. Implement a deterministic long-run soak harness executing 200 continuous simulated campaign days.
5. Assert that memory consumption, node counts, and unmanaged resources remain bounded throughout the soak.
6. Verify that save state serialized at Day 200 produces identical state when deserialized into a fresh session.
7. Implement automated backup recovery: if a primary save fails checksum validation, automatically load `.bak`.
8. Ensure corruption detection returns a clear, user-facing error code rather than causing process crashes or freezes.
9. Implement a safe slot recovery utility that repairs missing section keys with deterministic fallback defaults.
10. Verify that save schema migrations handle legacy envelopes smoothly without discarding valid player progress.
11. Add a stress test mutating random bytes in a test save payload and proving the parser safely rejects it.
12. Implement atomic save write guards ensuring that sudden process termination mid-write leaves the original save intact.
13. Audit all active `SaveStore` classes to ensure culture-invariant formatting (`CultureInfo.InvariantCulture`) on all floats.
14. Ensure save timestamps use UTC ISO 8601 formatting and never rely on local machine timezone offsets.
15. Add unit tests in `Ashfall.Core.Tests/Save/Plan39SessionDurabilityTests.cs` verifying slot recovery fallbacks.
16. Verify that saving during high-stress game events (severe fallout storm, active combat) captures complete state.
17. Run `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` to verify 100% compliance across all stores.
18. Validate that saving and reloading does not cause duplicate event emissions or replay state drift.
19. Run `--data-integrity-selftest` and confirm that all save-coupled data catalogs pass integrity gates.
20. Update `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` with durability guarantees and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### B2.1 200-Day Deterministic Soak Test
- (a) Author a headless soak script in `Ashfall.Core.Tests/Save/Soak200DaySimulationTests.cs`.
- (b) Execute 200 continuous simulated campaign days with simulated weather, consumption, and expeditions.
- (c) Log heap allocations and verify that memory growth stabilizes after initial catalog and cache initialization.
- (d) Compare final Day 200 state against a reference golden run with the same seed, asserting 100% state identity.

#### B2.2 Backup Slot Rotation & Fallback Loading
- (a) Validate that `SaveOrchestrator` rotates the existing valid save to `.bak` prior to overwriting disk state.
- (b) Implement an automated fallback: when `primary.save` is truncated or corrupted, attempt reading `primary.bak`.
- (c) Emit a high-priority user briefing notification informing the player that a backup save was recovered.
- (d) Author unit tests verifying recovery from truncated files, 0-byte files, and invalid JSON syntax.

#### B2.3 Mid-Write Crash Simulation & Atomic Protection
- (a) Verify that save writing always writes to a temporary file (`save.tmp`) before calling atomic file rename.
- (b) Inject artificial I/O exceptions mid-stream to simulate sudden process crashes during serialization.
- (c) Assert that failed write attempts leave the original save file unmodified and fully loadable.
- (d) Author automated tests verifying that temporary scratch files are cleaned up on subsequent boots.

#### B2.4 Culture Invariant Number Formatting Enforcement
- (a) Scan all serialization code for string formatting calls (`ToString()`, `float.Parse`) lacking culture arguments.
- (b) Enforce `CultureInfo.InvariantCulture` across all coordinates, multipliers, and floating-point scalars.
- (c) Run save round-trip tests under European locale simulation (comma decimal separator) to ensure 0 format errors.
- (d) Confirm that save file checksums remain byte-identical regardless of host OS language configuration.

---

## TASK B3 — C2[17] Authored Survivor Identity, Declared Item Tags & Diegetic Personality (Plan 40A/40B/40C)

- **Source Plan:** `C-integration-plans/C2_planintegration[17].md` (Plan 40)
- **Blocker Class:** INFERRED STATS DEFECT / UNWIRED IDENTITY DATA
- **Canonical Owner:** `Assets/Ashfall.Core/Survivors/`, `Assets/Ashfall.Core/Inventory/`
- **Target Subsystem:** Authored belief profiles, profession data, keepsakes, declared item tags

### 20 Procedural Substeps:
1. Re-verify the current survivor identity architecture: 129 survivor definitions with biography and traits.
2. Identify the core defect: social friction currently computes via `InferBeliefProfile()` rather than authored identity.
3. Claim `Assets/Ashfall.Core/Survivors/SurvivorIdentity.cs` and `Assets/Ashfall.Core/Inventory/ItemTagCatalog.cs`.
4. Replace runtime trait inference with explicit authored belief profiles loaded from `survivor_beliefs.json`.
5. Wire authored survivor professions from `professions.json` into `FitnessForDutyModel` suitability multipliers.
6. Connect survivor personal keepsakes (`keepsakes.json`) into morale recovery and grief mitigation systems.
7. Replace hardcoded item ID lists in game systems with validated tag queries from `ItemTagCatalog`.
8. Ensure all consumable item behaviors (medical, nutrition, repair) query declared tags rather than string prefixes.
9. Wire phantom background traits into exploration and expedition event outcome calculations.
10. Ensure survivor ideological friction reflects authored core values (Technological, Pragmatic, Zealous, Communal).
11. Update `SurvivorDetailPanel.cs` to present authored personality traits diegetically without raw numeric stats.
12. Ensure item tags in `items.json` conform strictly to schema specifications and snake_case formatting.
13. Wire heirloom items to grant unique survivor-specific morale and psychological stabilization bonuses.
14. Ensure newly loaded identity fields restore cleanly from existing survivor save records without schema breaks.
15. Add characterization tests in `Ashfall.Core.Tests/Survivors/Plan40SurvivorIdentityTests.cs`.
16. Validate that identical seeds produce consistent survivor trait and identity generation.
17. Verify that item tag lookups are cached and execute with O(1) complexity during tight simulation loops.
18. Run `--data-integrity-selftest` to ensure `survivor_beliefs.json` and item tags validate cleanly.
19. Run survivor and inventory focused test suites to confirm zero regressions in needs or crafting.
20. Update `docs/systems/SURVIVOR_STATE_AUTHORITY_MATRIX.md` with authored identity rules and hand off.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### B3.1 Authored Belief Profile Plumb-Through
- (a) Parse `survivor_beliefs.json` in `SurvivorCatalogLoader` and bind beliefs directly to survivor state records.
- (b) Deprecate and remove heuristic `InferBeliefProfile()` methods from `SurvivorSocialCoordinator`.
- (c) Update ideological friction calculations in `MoraleContagionSystem` to compute deltas between authored beliefs.
- (d) Author unit tests asserting that survivors with opposing authored beliefs generate predictable social friction.

#### B3.2 Item Tag Authority Migration
- (a) Audit `Inventory.cs`, `CraftingSystem.cs`, and `MedicalWardSystem.cs` for hardcoded item ID equality checks.
- (b) Replace string literals with `ItemTagCatalog.HasTag(itemId, tag)` queries across all consumption paths.
- (c) Ensure tags such as `medical_sterile`, `food_raw`, `tool_precision`, and `fuel_volatile` are fully declared.
- (d) Author unit tests proving that novel custom items possessing standard tags function identically in all systems.

#### B3.3 Keepsake & Heirloom Mechanics
- (a) Associate unique keepsake items from `keepsakes.json` with matching survivor background identifiers.
- (b) Implement passive morale decay reduction when a survivor possesses their personal keepsake in inventory.
- (c) Implement severe distress events if a survivor's personal keepsake is lost, stolen, or destroyed on expeditions.
- (d) Validate keepsake persistence across save/load cycles and confirm zero save payload expansion.

#### B3.4 Diegetic Personality Presentation
- (a) Update `SurvivorDetailPanel.cs` to format personality and background through authored descriptive vignettes.
- (b) Replace raw numeric compatibility ratings with narrative summaries ("Shared pragmatic outlook", "Tense friction").
- (c) Ensure high-contrast visual display and screen-reader accessibility for all narrative personality text.
- (d) Execute `--ui-a11y-selftest` to confirm that survivor identity screens meet all visual and contrast standards.

---

## TASK B4 — C2[18] Deterministic Survivor Voice, Delivery Contracts & Social Speech (Plan 42A/42B/42C)

- **Source Plan:** `C-integration-plans/C2_planintegration[18].md` (Plan 42)
- **Blocker Class:** NARRATIVE COHESION / VOICE DELIVERY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Narrative/`, `src/Audio/`
- **Target Subsystem:** Deterministic line selection, attention budgeting, social commentary, speech routing

### 20 Procedural Substeps:
1. Review current voice and narrative assets: radio scripts, briefing logs, audio cues, and survivor inspection barks.
2. Confirm the architectural guardrail: no conversational AI, no dialogue trees, no dynamic runtime text generation.
3. Claim `Assets/Ashfall.Core/Narrative/VoiceDeliveryCoordinator.cs` and `src/Host/AudioEventBridge.cs`.
4. Implement `VoiceDeliveryCoordinator.cs` as a pure, deterministic speaker-selection and line-resolution engine.
5. Create `Assets/StreamingAssets/Data/survivor_voices.json` cataloging authored voice lines keyed to semantic triggers.
6. Connect voice trigger evaluation strictly to Plan 31 `SemanticKind` day events and shelter status changes.
7. Implement an attention budget manager preventing voice fatigue: enforce cooldowns between spoken survivor barks.
8. Filter voice candidates based on speaker physical state: unconscious, quarantined, or dead survivors never speak.
9. Route chosen voice lines to appropriate presentation sinks: Daily Briefing quote, HUD status ticker, or Radio log.
10. Wire audio cue playback: if a voice line has an associated audio sample, trigger it through `AudioManager`.
11. Ensure missing audio samples fallback gracefully to text-only display without raising runtime exceptions.
12. Ensure speaker selection uses seeded RNG (`CampaignRngStream.Narrative`), producing identical lines on replay.
13. Bind voice line topics to recent shelter history: rationing conflicts, recent deaths, severe weather arrival.
14. Ensure voice line dispatch never causes frame drops or blocks game-loop simulation execution.
15. Implement disposal discipline: unhook voice delivery subscriptions when switching scenes or closing sessions.
16. Add unit tests in `Ashfall.Core.Tests/Narrative/Plan42VoiceDeliveryTests.cs` verifying line selection algorithms.
17. Verify that identical campaign seeds produce byte-identical voice log histories across 30 simulated days.
18. Validate that voice line history saves and restores cleanly through existing campaign journal persistence.
19. Run `--audio-selftest` to ensure voice cue routing does not violate audio concurrency or ducking policies.
20. Update `docs/narrative/NARRATIVE_VOICE_GUIDELINES.md` with authored voice taxonomy and complete handoff.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### B4.1 Semantic Voice Line Catalog Authoring
- (a) Structure `survivor_voices.json` with fields: `id`, `semantic_kind`, `required_traits`, `cooldown_days`, `text`, `cue_id`.
- (b) Author voice entries for all 10 `SemanticKind` domains (Weather, Hazard, Needs, Medical, Grief, Morale, etc.).
- (c) Validate that all line texts maintain ASHFALL's restrained, post-apocalyptic survivor tone.
- (d) Run `CatalogIntegrityValidator.cs` to verify zero duplicate IDs or invalid trait requirements in voice data.

#### B4.2 Deterministic Speaker Selection Engine
- (a) Implement candidate filtering: identify all living, conscious shelter survivors possessing matching traits.
- (b) Rank eligible candidates using a seeded deterministic pseudo-random hash of campaign day and survivor ID.
- (c) Select the highest-priority line that satisfies all environmental preconditions and cooldown timers.
- (d) Author unit tests proving that re-running candidate selection with identical state yields the exact same speaker.

#### B4.3 Attention Budget & Fatigue Prevention
- (a) Enforce global and per-survivor bark cooldowns: max 1 global bark per 60 seconds; max 1 per survivor per day.
- (b) Implement priority escalation: critical warnings (generator failure, breach) supersede casual atmospheric lines.
- (c) Suppress voice line emissions entirely during active combat scenes or modal dialog interactions.
- (d) Author unit tests verifying that high-frequency trigger events do not exceed configured attention budgets.

#### B4.4 Multi-Sink Presentation Routing
- (a) Connect resolved voice lines to HUD ambient bark floating labels with automatic fade-out timers.
- (b) Route significant survivor reflections to the daily briefing summary report builder.
- (c) Forward matching audio cue triggers to `AudioManager.PlayOneShot` with proper audio bus assignment.
- (d) Run `--ui-a11y-selftest` to verify that all spoken barks provide accessible, high-contrast closed-caption text.

---

# 2. CROSS-TASK VERIFICATION MATRIX

| Task | Domain | Focused Test Suite | Build Gate | Data Integrity | Replay / Determinism | UI / Audio Lifecycle | Handoff Target |
|---|---|---|---|---|---|---|---|
| **A1** | Content Rails | `Plan49DepthPassTests.cs` | Required (0/0) | Required (333 catalogs) | Replay seed stability | N/A | `CATALOG_REGISTRY.md` |
| **A2** | Spatial Presentation | `SpatialViewLifecycleTests.cs` | Required (0/0) | Unchanged | Deterministic view sync | `--panel-bind-lifecycle` | `UI_PANEL_GUIDE.md` |
| **A3** | Repo Hygiene | `scripts/ci/verify-fast.sh` | Required (0/0) | Required | Static tooling check | Zero file leaks | `REPOSITORY_HYGIENE.md` |
| **A4** | Longitudinal Medicine | `Plan60MedicalContinuityTests.cs` | Required (0/0) | Required (diseases.json) | Deterministic contagion | `--ui-a11y-selftest` | `AUTHORITY_MATRIX.md` |
| **B1** | Focus & Input | `InputFocusNavigationTests.cs` | Required (0/0) | Unchanged | Input replay journey | `--ui-a11y-selftest` | `INPUT_GUIDE.md` |
| **B2** | Session Durability | `Plan39SessionDurabilityTests.cs` | Required (0/0) | Required | 200-day soak equality | Atomic write check | `SAVE_CONTRACT.md` |
| **B3** | Authored Identity | `Plan40SurvivorIdentityTests.cs` | Required (0/0) | Required (beliefs.json) | Seeded identity roll | `--ui-a11y-selftest` | `AUTHORITY_MATRIX.md` |
| **B4** | Voice Delivery | `Plan42VoiceDeliveryTests.cs` | Required (0/0) | Required (voices.json) | Seeded line selection | `--audio-selftest` | `VOICE_GUIDELINES.md` |

---

# 3. TASK HANDOFF TEMPLATE

Every completed task must publish a handoff document using this standardized structure:

```markdown
## TASK <ID> HANDOFF: <TITLE>

### 1. Workspace & Authority
- Commit / HEAD tested:
- Claimed paths in `WORKTREE_OWNERSHIP.md`:
- Concurrent claims checked:

### 2. Premise Re-Verification
- Historical blocker statement:
- Verified state at execution start:
- Stale assumptions corrected:

### 3. Decisions & Signatures
- Signed decisions consumed:
- Deferred sub-scopes recorded:

### 4. Implementation Details
- Canonical authority modified:
- Old path / New path:
- Files created / modified / deleted:
- Generated assets regenerated:

### 5. Verification & Testing
- Focused test command & exact results:
- Headless selftest results (`--data-integrity`, `--audio`, `--ui-a11y`):
- Determinism / Replay fingerprint results:
- Build output (Errors: 0, Warnings: 0):

### 6. Ledger & Documentation Updates
- `INTEGRATION_PLANS.md` status:
- Architecture / Catalog maps updated:
- Next unblocked dependency:
```

---

# 4. WAVE-LEVEL FAILURE & STOP POLICY

Stop work immediately and report to the foreman/user if any of the following conditions arise:
1. **Missing Domain Owner:** A task requires behavior that lacks a clear canonical authority, tempting the creation of a duplicate manager or panel-side math.
2. **Signature Gate Unmet:** A decision-gated choice (e.g. balance re-weighting or permanent deferral) has no signed record.
3. **Claim Collision:** A file requiring modification is actively claimed by another agent in `WORKTREE_OWNERSHIP.md`.
4. **Premise Invalidation:** Code inspection proves that a problem described in a plan was already solved or that the code structure fundamentally diverged from plan assumptions.
5. **Determinism Break:** Seeded replay simulations diverge across runs or save/load interruptions.
6. **Engine Infiltration:** Any change causes Godot or engine namespaces to leak into `Assets/Ashfall.Core/`.

---

# 5. DEFINITION OF DONE FOR WAVE 12 PART 1

Wave 12 Part 1 is complete only when all eight tasks have achieved an allowed terminal state (`IMPLEMENTED` or `DECIDED-DEFERRED`), verified by:
- All 8 tasks have executed all 20 procedural substeps and structured mini-tasks without taking shortcuts.
- `dotnet build Ashfall.csproj` completes with **0 Errors and 0 Warnings**.
- `godot --headless --path . -- --data-integrity-selftest` passes with **0 Errors across 333+ catalogs**.
- `godot --headless --path . -- --audio-selftest` passes with **0 Failures**.
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` and `--ui-a11y-selftest` pass cleanly.
- No new parallel authorities, shadow registries, or duplicate save stores have been introduced.
- All modified markdown files and documentation indices are synchronized and free of trailing whitespace.
