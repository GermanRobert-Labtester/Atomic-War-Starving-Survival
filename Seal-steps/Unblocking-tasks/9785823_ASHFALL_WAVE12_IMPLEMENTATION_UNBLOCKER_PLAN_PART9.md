# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 9
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 9 master plan for Generation Wave 12, derived from the verified high-priority quality and infrastructure roadmaps.

**Purpose:** convert the eight highest-priority unsealed architecture, responsive UI, headless tooling, regression testing, and security hardening plans into an execution-grade unblocking package. Each task provides 20 procedural execution instructions and structured mini-tasks with 4 additional mini-substeps each, eliminating guesswork for coding agents and foremen.

**Wave 12 Part 9 Premise:** Parts 1 through 8 established the entire narrative, systemic, economic, and late-game civilizational surface of ASHFALL. Part 9 delivers the **production infrastructure and platform engineering foundation**: responsive UI layout across resolutions (Steam Deck, ultrawide, high-DPI), complete headless management CLI tools, automated golden-file regression detection, architectural dependency graph enforcement, in-game contextual encyclopedia and tooltips, automated content reachability and orphan auditing, sub-second incremental compilation optimization, and cryptographic save tampering protection with input sanitization.

**Part 9 Execution Set (Exactly 8 Tasks):**
1. **Task Q1 — Roadmap Batch 105: Responsive UI Layout & Resolution Independence Engine**
2. **Task Q2 — Roadmap Batch 106: Complete Headless CLI & Automated Shelter Management Tooling**
3. **Task Q3 — Roadmap Batch 107: Automated Golden-File Regression Detection & Replayable Diagnostics**
4. **Task Q4 — Roadmap Batch 108: Architectural Layering Guardrails & Dependency Graph Enforcement**
5. **Task R1 — Roadmap Batch 109: Contextual In-Game Encyclopedia & Semantic Tooltip Knowledge Base**
6. **Task R2 — Roadmap Batch 110: Automated Content Reachability & Orphan Asset Coverage Auditor**
7. **Task R3 — Roadmap Batch 111: Developer Ergonomics: Incremental Compilation & Build Optimization**
8. **Task R4 — Roadmap Batch 112: Save Tampering Hardening & Cryptographic Input Sanitization**

---

# 0. OPERATING CONTRACT

## 0.1 Allowed Terminal States

Every task in Wave 12 Part 9 must reach one of these terminal states:

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

## TASK Q1 — Roadmap Batch 105: Responsive UI Layout & Resolution Independence Engine

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_105.md`
- **Blocker Class:** MULTI-RESOLUTION UI / DISPLAY ADAPTATION GAP
- **Canonical Owner:** `src/UI/`, `src/Theme.cs`
- **Target Subsystem:** Breakpoint-based layout tiers, font-scale factors, Steam Deck & Ultrawide panel adapters

### 20 Procedural Substeps:
1. Audit all 83 UI panel classes implementing `Control` in `src/UI/` to catalog hardcoded 1920x1080 pixel coordinates.
2. Enforce core invariant: UI layouts project viewport scale factors; Core domain logic remains completely resolution-agnostic.
3. Claim `src/UI/ResponsiveLayoutCoordinator.cs` and `src/UI/LayoutTier.cs`.
4. Define `DisplayResolutionTier` enum: CompactDeck (1280x800), Standard1080p (1920x1080), Ultrawide (2560x1080, 3440x1440), 4KRetina (3840x2160).
5. Implement dynamic viewport breakpoint listeners recalculating panel bounds on window resize events.
6. Author anchor-based layout helpers in `ResponsiveLayoutCoordinator` replacing absolute coordinate assignments with relative fractions.
7. Implement font scaling profiles adjusting font sizes dynamically to preserve legibility on small handheld screens.
8. Author collapsible sidebar panels for CompactDeck mode, preserving dashboard operational space on 1280x800 screens.
9. Support ultrawide letterboxing vs wide dashboard stretching, preventing stretched distorted UI canvases.
10. Ensure modal dialogs and popup alert cards auto-center and clamp strictly within visible viewport margins.
11. Build a headless layout test harness asserting zero control bounding-box overflows across all 83 panels.
12. Author `ui_layout_profiles.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
13. Wire DPI detection to automatically apply appropriate font-scale multipliers on high-density displays.
14. Ensure mouse and gamepad navigation focus rectangles scale proportionally with responsive control boundaries.
15. Save and restore user-selected resolution preferences and UI scale overrides inside user configuration files.
16. Author unit tests in `Ashfall.Tests/UI/ResponsiveLayoutTests.cs` verifying breakpoint classification logic.
17. Verify that responsive layout recomputations execute exclusively on window resize events, avoiding per-frame allocations.
18. Validate that all critical gameplay HUD gauges remain visible and uncluttered on 1366x768 and Steam Deck viewports.
19. Inspect build output to confirm zero compiler warnings, zero float precision drift, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/ui/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### Q1.1 Viewport Breakpoints & Tier Classification
- (a) Author `LayoutBreakpointDetector` classifying active screen dimensions into discrete display tiers.
- (b) Connect window resize signal from Godot Viewport to layout change notifications.
- (c) Provide query API `GetActiveLayoutTier()` allowing UI controls to query active layout profiles.
- (d) Write unit tests verifying that standard resolutions (1080p, 1440p, Steam Deck) map to expected tiers.

#### Q1.2 Dynamic Font & Control Scaling
- (a) Model font-scale factors: 0.85x for CompactDeck, 1.0x for Standard1080p, 1.5x for 4K displays.
- (b) Author recursive font updater traversing Godot scene trees to apply responsive font overrides.
- (c) Clamp minimum font sizes to 12pt to prevent unreadable text on small screens.
- (d) Author tests validating that font scaling maintains clear label text bounds without clipping.

#### Q1.3 Compact Deck & Handheld Optimization
- (a) Author collapsible tabbed navigation bars specifically for compact 1280x800 viewports.
- (b) Increase minimum touch and click target areas to 44x44 pixels for handheld comfort.
- (c) Reposition secondary telemetry gauges into expandable pull-out drawers.
- (d) Author tests proving that compact layout panels fit completely within 800px vertical limits.

#### Q1.4 Automated Overflow Verification Harness
- (a) Build headless layout runner rendering panels at 720p, 1080p, 1440p, and 4K resolutions.
- (b) Assert that zero `Control` bounding boxes overlap or extend beyond parent viewport boundaries.
- (c) Generate visual artifact screenshots comparing panel compositions across standard test resolutions.
- (d) Author characterization tests confirming that responsive resizing produces zero null pointer exceptions.

---

## TASK Q2 — Roadmap Batch 106: Complete Headless CLI & Automated Shelter Management Tooling

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_106.md`
- **Blocker Class:** HEADLESS CLI / AUTOMATION HARNESS GAP
- **Canonical Owner:** `src/Host/`, `src/CLI/`
- **Target Subsystem:** Godot headless command-line entry points, batch day stepping, automated campaign exports

### 20 Procedural Substeps:
1. Review `src/Main.cs` and existing CLI argument parsing switches (`--run-tests`, `--headless`) to map command dispatch.
2. Enforce core invariant: CLI commands interact exclusively through public Core domain APIs; no direct database mutations.
3. Claim `src/CLI/HeadlessCommandDispatcher.cs` and `src/CLI/CommandLineArguments.cs`.
4. Implement standard CLI argument parser supporting `--step-days <N>`, `--campaign-seed <S>`, and `--export-save <PATH>`.
5. Provide headless campaign initialization allowing batch simulation runs without launching windowed Godot display servers.
6. Build a headless day stepper command: advance simulation state by N days deterministically, logging daily milestone summaries.
7. Support automated telemetry exports outputting campaign economic reserves, casualty counts, and tech progress to JSON.
8. Implement headless save file inspection: parse and dump save file metadata, checksums, and survivor censuses to stdout.
9. Provide automated integrity checks callable from shell scripts (`--validate-data`, `--verify-save <FILE>`).
10. Implement headless disaster injection flags for QA fuzzing (`--inject-disaster <KIND> --day <D>`).
11. Support batch scenario benchmarking measuring wall-clock execution time per 100 simulated campaign days.
12. Ensure all CLI error conditions exit with distinct standard non-zero POSIX return codes for CI script piping.
13. Author `cli_command_reference.md` in `docs/tooling/` documenting all supported command-line switches and flags.
14. Ensure CLI commands execute safely in headless Linux CI environments with zero X11/Wayland display dependencies.
15. Wire headless command output formatting: provide both human-readable terminal text and machine-readable JSON modes.
16. Author integration tests in `Ashfall.Tests/CLI/HeadlessCommandTests.cs` verifying argument parsing and execution.
17. Verify that stepping days headlessly produces bitwise-identical state checksums to interactive gameplay stepping.
18. Validate that all CLI execution paths respect the 180-second timeout policy and terminate cleanly.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/tooling/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### Q2.1 CLI Argument Parser & Router
- (a) Author robust command-line tokenizer parsing long and short switches (e.g. `-s` and `--seed`).
- (b) Provide automated `--help` switch listing all available commands with concise usage summaries.
- (c) Enforce strict validation rejecting unknown switches with informative syntax error messages.
- (d) Write unit tests verifying that valid argument combinations parse cleanly into structured command objects.

#### Q2.2 Headless Day Stepper & Fast Simulation
- (a) Author `HeadlessSimulationRunner` executing non-graphical game loop updates in a tight loop.
- (b) Support stepping arbitrary campaign day counts (`--step-days 30`) without graphics rendering overhead.
- (c) Emit formatted daily progression indicators to stdout displaying active day, food count, and casualties.
- (d) Author tests validating that headless simulation advances campaign calendar state with 100% determinism.

#### Q2.3 Automated Telemetry & Report Exporter
- (a) Author `--export-metrics <FILE>` switch dumping structured campaign telemetry to disk.
- (b) Capture resource consumption curves, disease progression metrics, and faction relationship deltas.
- (c) Format exported metrics in clean schema-valid JSON suitable for automated graph plotting.
- (d) Author tests proving that exported metrics accurately reflect canonical internal simulation counters.

#### Q2.4 Headless Save File Inspector & Repair
- (a) Author `--inspect-save <FILE>` switch reading save files and printing header summaries to stdout.
- (b) Display save file schema version, creation timestamp, campaign seed, and survivor count.
- (c) Provide `--verify-checksum <FILE>` validating HMAC save integrity without loading full game sessions.
- (d) Author characterization tests confirming that corrupted save files are detected and reported accurately.

---

## TASK Q3 — Roadmap Batch 107: Automated Golden-File Regression Detection & Replayable Diagnostics

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_107.md`
- **Blocker Class:** REGRESSION TESTING / GOLDEN STATE PROOF GAP
- **Canonical Owner:** `Ashfall.Core.Tests/`, `Assets/Ashfall.Core/Diagnostics/`
- **Target Subsystem:** Deterministic simulation snapshots, golden JSON diff comparisons, CI regression gate

### 20 Procedural Substeps:
1. Review existing test fixtures in `Ashfall.Core.Tests/Fixtures/` to map existing simulation baselines.
2. Enforce core invariant: golden files prove deterministic simulation invariance; they are updated only with intentional approval.
3. Claim `Ashfall.Core.Tests/Regression/GoldenFileRegressionHarness.cs` and `Assets/Ashfall.Core/Diagnostics/StateSnapshot.cs`.
4. Define standard 30-day, 60-day, and 120-day benchmark scenarios with fixed seeds and scripted input sequences.
5. Capture full canonical state serialization snapshots at designated milestone days in normalized JSON format.
6. Implement deterministic JSON normalization: sort dictionary keys, format floats culture-invariantly, strip timestamps.
7. Author golden file comparison engine computing line-by-line diffs between current simulation runs and committed golden files.
8. Store baseline golden snapshots in `Ashfall.Core.Tests/Golden/` committed directly into version control.
9. Enforce CI regression gate: any unapproved semantic state drift in golden scenarios fails the test build immediately.
10. Provide an explicit golden file regeneration utility (`dotnet test --filter Golden -- -update-golden`) for intentional changes.
11. Generate clear, human-readable diff reports highlighting exact variables that drifted (e.g. `Survivor[3].Health: 85 -> 80`).
12. Ensure golden file tests run completely in memory without touching persistent user save directories.
13. Author `golden_scenarios.json` declaring benchmark seeds, starting roster configurations, and decision scripts.
14. Enforce that golden file tests complete within established execution budgets (<10 seconds per scenario).
15. Support snapshot masking for deliberately non-deterministic or expansion-reserved fields.
16. Author unit tests in `Ashfall.Core.Tests/Regression/GoldenDiffEngineTests.cs` verifying diff detection accuracy.
17. Verify that running golden tests on different operating systems (Linux vs Windows) yields zero false-positive diffs.
18. Validate that golden snapshots cover economy, needs, health, radiation, and faction relationships comprehensively.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/testing/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### Q3.1 State Serialization & JSON Normalization
- (a) Author `DeterministicSnapshotSerializer` outputting complete Core simulation state to formatted JSON.
- (b) Enforce strict alphabetical key sorting across all dictionaries and JSON object structures.
- (c) Format floating-point values with fixed decimal precision (`F4`) using invariant culture rules.
- (d) Write unit tests verifying that two identical game states produce identical byte-for-byte JSON strings.

#### Q3.2 Benchmark Scenarios & Input Scripts
- (a) Author Scenario A (Temperate Famine), Scenario B (Nuclear Winter Blizzard), Scenario C (Wasteland Raid).
- (b) Script deterministic player input actions (duty assignments, food rationing, research selections) per scenario.
- (c) Execute scenarios headlessly for 30 consecutive days from fixed starting seeds.
- (d) Author tests validating that scripted scenarios execute identically across repeated local runs.

#### Q3.3 Diff Comparison Engine & Failure Reporting
- (a) Author `GoldenDiffComparer` comparing active test run snapshots against committed baseline files.
- (b) Detect added, removed, or modified state properties, formatting differences into readable text diffs.
- (c) Emit rich failure messages detailing exact property paths and value divergences to test runners.
- (d) Author tests proving that modifying a single survivor hunger value triggers immediate test failure.

#### Q3.4 Golden File Maintenance & Update Tooling
- (a) Provide guarded `-update-golden` flag allowing authorized developers to re-baseline golden files.
- (b) Author validation script ensuring that updating golden files requires an explicit git commit explanation.
- (c) Verify that golden file updates cleanly stage and format without trailing whitespace.
- (d) Author characterization tests confirming that updated golden files immediately pass verification gates.

---

## TASK Q4 — Roadmap Batch 108: Architectural Layering Guardrails & Dependency Graph Enforcement

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_108.md`
- **Blocker Class:** ARCHITECTURAL BOUNDARY / ENGINE COUPLING LEAK
- **Canonical Owner:** `Assets/Ashfall.Core/`, `Ashfall.Core.Tests/Tooling/`
- **Target Subsystem:** Roslyn architecture analyzers, Core 0-engine-ref verification, circular dependency static assertions

### 20 Procedural Substeps:
1. Review `AGENTS.md` Non-Negotiable Rule 2 ("Core stays engine-free") and current `.csproj` references across the solution.
2. Enforce core invariant: `Assets/Ashfall.Core/` (`netstandard2.1`) must never reference Godot, Unity, or engine serialization APIs.
3. Claim `Ashfall.Core.Tests/Tooling/ArchitectureLayeringTests.cs` and `scripts/check_architecture_rules.py`.
4. Build static reflection tests scanning all compiled types in `Ashfall.Core.dll` for forbidden assembly references.
5. Assert that zero types in `Ashfall.Core.dll` reference `GodotSharp`, `UnityEngine`, or `System.Drawing`.
6. Implement circular dependency detection analyzing namespace coupling within `Ashfall.Core` to prevent architectural cycles.
7. Enforce strict downward layering: Domain Models -> Coordinators -> Host Adapters -> UI Panels.
8. Assert that zero classes in `src/UI/` are directly referenced by Core domain simulation coordinators.
9. Assert that all cross-subsystem communication routes through public interfaces or the semantic event bus.
10. Build a Roslyn analyzer or build-time script failing compilation immediately upon detection of forbidden `using Godot;` in Core.
11. Scan Core domain logic for forbidden non-deterministic calls (`System.Random`, `DateTime.Now`, `Guid.NewGuid()`).
12. Verify that `Ashfall.Core.Tests` references `Ashfall.Core` through public APIs without internal access bypasses.
13. Author `architecture_layering_rules.md` in `docs/architecture/` documenting strict namespace boundaries.
14. Integrate architecture validation into the standard CI pull request gate, blocking boundary-violating PRs.
15. Verify that all save DTOs implement explicit versioning and serialization contracts.
16. Author unit tests in `Ashfall.Core.Tests/Tooling/AssemblyReferenceTests.cs` asserting 0-engine-ref compliance.
17. Verify that architecture checks execute rapidly (<1.5 seconds) to avoid slowing local developer test cycles.
18. Validate that newly authored Wave 12 classes strictly obey all established layering guardrails.
19. Inspect build output to confirm zero compiler warnings, zero namespace collisions, and strict netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/architecture/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### Q4.1 Zero-Engine-Reference Static Assertion
- (a) Inspect `Assembly.GetReferencedAssemblies()` on `typeof(SurvivorLifecycle).Assembly`.
- (b) Assert that `GodotSharp` and `Godot` do not appear anywhere in referenced assembly lists.
- (c) Scan all compiled types for fields or methods returning Godot engine types (`Node`, `Control`, `Vector2`).
- (d) Write unit tests verifying that adding a dummy Godot reference to Core immediately fails test assertions.

#### Q4.2 Circular Namespace Dependency Detection
- (a) Build directed dependency graph mapping namespace references within `Assets/Ashfall.Core/`.
- (b) Run Tarjan's strongly connected components algorithm to detect any circular dependency loops.
- (c) Report exact cyclic paths (e.g. `Needs -> Survivors -> Needs`) to developer terminal output.
- (d) Author tests validating that current Core namespaces form a strict directed acyclic graph (DAG).

#### Q4.3 Determinism & Randomness Scanner
- (a) Scan Core IL bytecode for invocations of `System.Random` constructor or methods.
- (b) Scan Core IL bytecode for calls to `DateTime.Now`, `DateTime.UtcNow`, and `Environment.TickCount`.
- (c) Assert that all random operations route through the canonical `ISeededRng` interface.
- (d) Author tests proving that non-deterministic calls in Core domain logic are intercepted and rejected.

#### Q4.4 CI Integration & Pre-Commit Enforcement
- (a) Author lightweight Python verification script `scripts/verify_core_boundaries.py`.
- (b) Wire verification script into local git pre-commit hooks and GitHub Actions PR workflows.
- (c) Provide automated fix suggestions guiding developers to move engine code into `src/` adapters.
- (d) Author characterization tests confirming that CI boundary checks execute cleanly in under 2 seconds.

---

## TASK R1 — Roadmap Batch 109: Contextual In-Game Encyclopedia & Semantic Tooltip Knowledge Base

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_109.md`
- **Blocker Class:** DIEGETIC ONBOARDING / CONTEXTUAL HELP GAP
- **Canonical Owner:** `src/UI/`, `Assets/Ashfall.Core/Narrative/`
- **Target Subsystem:** In-game survival compendium, contextual right-click help cards, term cross-referencing

### 20 Procedural Substeps:
1. Review `TutorialPanel.cs` and existing UI tooltips to map current player guidance coverage.
2. Enforce core invariant: the encyclopedia explains canonical game rules and lore; it never stores mutable game state.
3. Claim `src/UI/CompendiumPanel.cs`, `src/UI/ContextualTooltip.cs`, and compendium catalogs.
4. Define `CompendiumCategory` enum: SurvivalBasics, RadiationAndMedicine, ShelterEngineering, FactionsAndTrade, WastelandEcology.
5. Author authored compendium entries in `compendium_entries.json` detailing game mechanics in grounded, diegetic prose.
6. Build a contextual right-click inspection system: right-clicking any stat icon or UI term opens its compendium article.
7. Implement hyperlinked cross-referencing: keywords within compendium text link directly to related articles (e.g. `[Sieverts]` links to Radiation).
8. Author rich tooltip cards rendering formatted item stats, preservation shelf-life, and required workbench tiers.
9. Support dynamic discovery: mark advanced topics (e.g. Pre-War Codexes, Nuclear Winter) as unlocked only upon encountering them.
10. Build a full-text search index allowing players to search the compendium by keyword, item name, or syndrome.
11. Support gamepad and keyboard navigation allowing players to cycle hyperlinked terms and browse articles easily.
12. Author `compendium_topics.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
13. Enforce catalog integrity validation confirming all hyperlinked cross-reference keys link to valid article IDs.
14. Ensure compendium UI screens scale responsively and preserve readability across all supported resolutions.
15. Save and restore unlocked compendium topic discovery flags inside `PlayerPreferencesSaveSection`.
16. Author unit tests in `Ashfall.Tests/UI/CompendiumTests.cs` verifying article cross-referencing integrity.
17. Verify that opening and searching the compendium produces zero memory churn or garbage collection pauses.
18. Validate that compendium articles adhere strictly to ASHFALL's restrained, authentic survival tone.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/ui/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### R1.1 Article Schema & Hyperlink Architecture
- (a) Define `CompendiumArticleDefinition` schema capturing TopicId, Category, TitleKey, BodyText, and RelatedTopicIds.
- (b) Parse inline markdown hyperlinks (`[text](topic:topic_id)`) into clickable UI link controls.
- (c) Provide broken link detector ensuring zero dangling cross-references exist in authored articles.
- (d) Write unit tests verifying that clicking hyperlinked keywords navigates cleanly to target articles.

#### R1.2 Contextual Right-Click Tooltip Dispatcher
- (a) Attach `ContextualHelpTag` components to UI labels, status icons, and inventory equipment slots.
- (b) Listen for secondary mouse clicks or gamepad Help button presses, spawning rich popup summary cards.
- (c) Display immediate summary tips with an option to "Read Full Dossier in Compendium".
- (d) Author tests validating that right-clicking radiation gauges opens the Radiation & Chelation article.

#### R1.3 Dynamic Discovery & Knowledge Progression
- (a) Model article visibility: basic survival rules are visible at start; advanced tech articles unlock upon discovery.
- (b) Intercept game event milestones (e.g. first survivor radiation sickness, first caravan encounter) to unlock articles.
- (c) Emit UI notification badges alerting players when new knowledge compendium pages become available.
- (d) Author tests proving that hidden lore topics remain concealed until triggered by canonical gameplay events.

#### R1.4 In-Memory Full-Text Search Engine
- (a) Build in-memory inverted text index indexing article titles, keywords, and body paragraphs.
- (b) Provide real-time search field filtering article lists as players type query strings.
- (c) Highlight matching search terms within displayed article body text.
- (d) Author characterization tests confirming that search queries return accurate results in under 5 milliseconds.

---

## TASK R2 — Roadmap Batch 110: Automated Content Reachability & Orphan Asset Coverage Auditor

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_110.md`
- **Blocker Class:** CONTENT UTILIZATION / DEAD ASSET DETECTION GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Diagnostics/`, `scripts/`
- **Target Subsystem:** Graph reachability analyzer, unreferenced item/quest detector, content audit report generator

### 20 Procedural Substeps:
1. Review `CatalogIntegrityValidator.cs` and all 333+ data files in `Assets/StreamingAssets/Data/` to establish baseline catalogs.
2. Enforce core invariant: reachability checks verify that authored content can actually be encountered during normal play.
3. Claim `Assets/Ashfall.Core/Diagnostics/ContentReachabilityAuditor.cs` and `scripts/audit_content_reachability.py`.
4. Construct a unified directed reachability graph linking Triggers -> Quests -> Locations -> Encounters -> Items -> Recipes.
5. Traverse the graph starting from root campaign nodes (Starting Shelter, Initial Recruits, Default Crafting Bills).
6. Detect completely unreachable items: items that appear in zero recipe outputs, loot tables, merchant stocks, or starting inventories.
7. Detect dead-end quests: quests that trigger but whose prerequisites cannot be fulfilled under any game state.
8. Detect orphaned dialogue nodes: narrative dialogue branches that lack inbound response transitions.
9. Detect disconnected map locations: world nodes with zero travel route connections from the starting shelter.
10. Calculate content utilization metrics: percentage of authored assets that are actively reachable in gameplay.
11. Generate automated Markdown reachability reports summarizing orphaned rows and broken prerequisite chains.
12. Distinguish intentional expansion-reserved content from accidental orphan bugs using `@reserved` metadata tags.
13. Wire reachability auditing into `godot --headless -- --content-utilization-selftest` command.
14. Enforce reachability thresholds: fail CI builds if reachable content percentage drops below established benchmarks (e.g. 95%).
15. Provide actionable remediation hints in audit logs suggesting missing loot table or recipe bindings.
16. Author unit tests in `Ashfall.Core.Tests/Diagnostics/ContentReachabilityTests.cs` verifying graph traversal logic.
17. Verify that reachability analysis executes rapidly (<3 seconds) across all 333+ JSON catalogs.
18. Validate that all newly unblocked Wave 12 items and quests register as 100% reachable.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/data/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### R2.1 Unified Content Graph Construction
- (a) Author graph builder parsing items, recipes, quests, encounters, and world nodes into node-edge models.
- (b) Create directed edges representing acquisition paths (e.g. `LootTable -> Item`, `Recipe -> Item`).
- (c) Create directed edges representing quest progression (e.g. `QuestTrigger -> QuestStage -> QuestReward`).
- (d) Write unit tests verifying that standard item crafting loops form closed, reachable graph structures.

#### R2.2 Orphan & Dead-End Detection Algorithms
- (a) Run breadth-first search from root nodes to classify all reachable vertices in the content graph.
- (b) Enumerate unreachable nodes, categorizing them by asset type (OrphanItem, DeadQuest, IsolatedLocation).
- (c) Check for cyclic quest prerequisite deadlocks where two quests mutually require each other's completion.
- (d) Author tests validating that an item with zero drop sources is flagged as an orphan asset.

#### R2.3 Expansion Reservation & Metadata Tagging
- (a) Support `@reserved: true` tag in JSON catalogs to explicitly exempt unreleased expansion content.
- (b) Filter reserved rows out of orphan alert tallies while recording them in the unreleased inventory section.
- (c) Flag obsolete or deprecated content rows with `@deprecated` tags for safe retirement triage.
- (d) Author tests proving that reserved items do not trigger reachability CI build failures.

#### R2.4 Automated Reporting & CI Quality Gate
- (a) Author report generator producing formatted markdown tables in `artifacts/reachability_audit.md`.
- (b) Output concise terminal summary displaying total items, reachable percentage, and orphan count.
- (c) Assert non-zero exit code in CI when orphan asset counts exceed maximum allowed debt thresholds.
- (d) Author characterization tests confirming that reachability reports execute cleanly in headless CI.

---

## TASK R3 — Roadmap Batch 111: Developer Ergonomics: Incremental Compilation & Build Optimization

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_111.md`
- **Blocker Class:** BUILD PERFORMANCE / DEVELOPER VELOCITY GAP
- **Canonical Owner:** `Ashfall.Core.csproj`, `Ashfall.csproj`
- **Target Subsystem:** MSBuild compilation caching, project reference trimming, sub-second incremental build times

### 20 Procedural Substeps:
1. Profile current `dotnet build` wall-clock duration across cold rebuilds and incremental single-file changes.
2. Enforce core invariant: build optimizations must preserve 100% binary determinism and zero compiler warnings.
3. Claim `Assets/Ashfall.Core/Ashfall.Core.csproj`, `Ashfall.csproj`, and `Directory.Build.props`.
4. Configure solution-wide `Directory.Build.props` setting standardized compiler optimization and warning levels.
5. Enable MSBuild accelerated build checking (`<AcceleratedBuildsInVisualStudio>true</AcceleratedBuildsInVisualStudio>`).
6. Enable deterministic source compilation flags (`<Deterministic>true</Deterministic>`) across all project files.
7. Audit project references to ensure `Ashfall.Core.csproj` maintains zero circular or redundant dependencies.
8. Optimize C# nullable reference analysis compilation passes, resolving suppressed warning debt in tests.
9. Configure incremental compilation cache directories ensuring object files are not unnecessarily re-emitted.
10. Remove dead and obsolete Unity-era build targets and post-build copy scripts from solution configuration files.
11. Enable parallel compilation across CPU cores using the `/m` MSBuild switch in development build scripts.
12. Measure and benchmark incremental build times, targeting <1.5 seconds for single-file Core modifications.
13. Author `build_performance_guide.md` in `docs/tooling/` documenting fast developer iteration workflows.
14. Ensure Godot C# project compilation hooks cleanly trigger incremental MSBuild without full solution rebuilds.
15. Verify that running `dotnet test` reuses cached build outputs rather than rebuilding unchanged assemblies.
16. Author build benchmark script `scripts/benchmark_build_speed.sh` measuring compilation times automatically.
17. Verify that build optimizations function identically on developer workstations and headless Linux CI containers.
18. Validate that cleaning the solution (`dotnet clean`) restores a completely fresh, unpolluted workspace.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with benchmark comparison tables, documentation, and updated entries in `docs/tooling/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### R3.1 Solution-Wide Directory.Build.props Standardization
- (a) Author `Directory.Build.props` in repository root declaring common C# 12 language version and nullable policies.
- (b) Centralize common compiler warnings and treat-warnings-as-errors flags across all subprojects.
- (c) Eliminate duplicate package and property declarations across individual `.csproj` files.
- (d) Write unit tests verifying that all projects inherit standardized build properties cleanly.

#### R3.2 Incremental Caching & Reference Trimming
- (a) Audit project references in `Ashfall.csproj` and `Ashfall.Core.Tests.csproj` to remove unused assembly imports.
- (b) Enable MSBuild compiler caching flags ensuring unchanged source files skip re-compilation.
- (c) Configure intermediate output paths cleanly separating Debug and Release build artifacts.
- (d) Author tests validating that touching a single file re-compiles only its parent assembly.

#### R3.3 Build Timing Profiling & Benchmarking
- (a) Author `scripts/benchmark_build_speed.sh` executing cold build, incremental build, and test runs with timing metrics.
- (b) Log duration breakdowns for MSBuild target steps (CoreCompile, CopyFilesToOutputDirectory).
- (c) Establish CI regression threshold failing builds if cold compilation exceeds 15 seconds.
- (d) Author characterization tests confirming that incremental compilation completes in under 1.5 seconds.

#### R3.4 Godot Host Integration & Fast Reload
- (a) Optimize `.godot/mono/temp/` build bridge ensuring Godot editor detects C# assembly updates immediately.
- (b) Eliminate redundant file copy steps between `Ashfall.Core` output folders and Godot bin directories.
- (c) Support fast developer edit-test-run cycles without restarting Godot editor sessions.
- (d) Verify through test automation that fast-reloaded assemblies preserve full runtime stability.

---

## TASK R4 — Roadmap Batch 112: Save Tampering Hardening & Cryptographic Input Sanitization

- **Source Plan:** `C-integration-plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_112.md`
- **Blocker Class:** SAVE INTEGRITY / INPUT SECURITY GAP
- **Canonical Owner:** `Assets/Ashfall.Core/Save/`, `Assets/Ashfall.Core/Security/`
- **Target Subsystem:** HMAC-SHA256 save checksum verification, save payload sanitization, path traversal guards

### 20 Procedural Substeps:
1. Review `SaveStore.cs`, `SavePayload.cs`, and `SaveMigrationService.cs` to map save serialization security boundaries.
2. Enforce core invariant: save integrity protects player save files from corruption and tampering; it never requires external servers.
3. Claim `Assets/Ashfall.Core/Security/SaveIntegrityCoordinator.cs` and `Assets/Ashfall.Core/Security/InputSanitizer.cs`.
4. Implement cryptographic HMAC-SHA256 checksum generation across serialized save file payloads using campaign-seeded keys.
5. Embed checksum signatures into save envelope headers with non-breaking backward compatibility for legacy saves.
6. Verify checksum signatures during save deserialization: detect corrupted or hand-tampered save payloads before loading.
7. Provide graceful recovery: if save checksum verification fails, offer to load the nearest verified automatic backup save.
8. Implement strict path traversal guards on save slot names preventing directory escape attacks (e.g. `../../etc/passwd`).
9. Sanitize all survivor names and authored text inputs against control characters, null bytes, and script injection strings.
10. Guard JSON deserialization against unbounded object allocation attacks by enforcing maximum JSON nesting depth limits.
11. Build an automatic rolling save backup system maintaining 3 rotating backups per campaign slot (`slot_1.sav.bak1`).
12. Implement atomic save file writing: write save data to a temporary `.tmp` file before renaming atomically to replace active saves.
13. Author `save_security_policy.json` in `Assets/StreamingAssets/Data/` with standard `schema_version: 1`.
14. Ensure save sanitization runs completely offline with zero network connectivity or external telemetry requirements.
15. Support developer debug mode: allow explicit command-line override (`--allow-modified-saves`) for testing.
16. Author unit tests in `Ashfall.Core.Tests/Security/SaveIntegrityTests.cs` verifying tamper detection mechanics.
17. Verify that atomic save operations survive simulated sudden process termination without corrupting prior saves.
18. Validate that survivor rename dialogs reject invalid characters while displaying clear input formatting guidelines.
19. Inspect build output to confirm zero compiler warnings, zero float allocations, and clean netstandard2.1 compliance.
20. Hand off the task with passing test fixtures, documentation, and updated entries in `docs/security/`.

### Mini-Tasks (with 4 Mini-Substeps Each):

#### R4.1 HMAC-SHA256 Checksum Verification
- (a) Author `SaveChecksumCalculator` computing HMAC-SHA256 digests over UTF-8 save payload byte streams.
- (b) Embed calculated signatures in standard save envelope headers alongside schema version tags.
- (c) Compare recomputed checksums during deserialization, rejecting payloads with mismatched digests.
- (d) Write unit tests verifying that modifying a single character in a save file triggers checksum failure.

#### R4.2 Atomic Write & Rolling Backup Rotator
- (a) Author `AtomicSaveWriter` writing save bytes to temporary `.tmp` files before executing atomic OS rename.
- (b) Rotate prior save files into `.bak1`, `.bak2`, and `.bak3` archives before committing new save files.
- (c) Provide automated backup recovery restoring the newest valid backup if the primary save file is truncated.
- (d) Author tests validating that simulated power loss during save write leaves the previous backup completely intact.

#### R4.3 Path Traversal & File Name Sanitization
- (a) Author `FilePathSanitizer` stripping path separators (`/`, `\`), null bytes, and relative navigation (`..`).
- (b) Restrict save file access strictly to designated sandboxed `user://saves/` directory roots.
- (c) Reject save slot names containing illegal operating system characters (`:`, `*`, `?`, `"`, `<`, `>`, `|`).
- (d) Author tests proving that attempting to save to `../../test.sav` is safely sanitized and clamped.

#### R4.4 Input String Sanitization & Depth Protection
- (a) Sanitize player-entered survivor names, shelter names, and radio broadcast transcripts upon submission.
- (b) Strip non-printable ASCII and control characters while preserving international UTF-8 letters and diacritics.
- (c) Enforce maximum JSON deserialization depth (max 32 levels) to prevent stack overflow denial-of-service.
- (d) Author characterization tests confirming that malicious input strings are cleaned without crashing the UI.

---

# 2. QUALITY GATE & VERIFICATION MATRIX

| Gate ID | Target System | Focused Verification Command | Passing Criterion |
| :--- | :--- | :--- | :--- |
| **QG-Q1** | Responsive UI Layout | `bash scripts/run_test.sh Ashfall.Tests/UI/ResponsiveLayoutTests.cs` | 100% pass; breakpoint tiers and bounds verified |
| **QG-Q2** | Headless CLI Tooling | `bash scripts/run_test.sh Ashfall.Tests/CLI/HeadlessCommandTests.cs` | 100% pass; argument parsing and day stepping green |
| **QG-Q3** | Golden-File Regression | `bash scripts/run_test.sh Ashfall.Core.Tests/Regression/GoldenDiffEngineTests.cs` | 100% pass; deterministic state snapshots verified |
| **QG-Q4** | Architecture Layering | `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureLayeringTests.cs` | 100% pass; 0-engine-ref and DAG compliance verified |
| **QG-R1** | Compendium & Help System | `bash scripts/run_test.sh Ashfall.Tests/UI/CompendiumTests.cs` | 100% pass; article links and contextual tooltips green |
| **QG-R2** | Content Reachability Audit | `bash scripts/run_test.sh Ashfall.Core.Tests/Diagnostics/ContentReachabilityTests.cs` | 100% pass; graph traversal and orphan detection green |
| **QG-R3** | Build Optimization | `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/BuildPerformanceTests.cs` | 100% pass; compilation flags and cache validity verified |
| **QG-R4** | Save Security & Integrity | `bash scripts/run_test.sh Ashfall.Core.Tests/Security/SaveIntegrityTests.cs` | 100% pass; HMAC checksums and atomic writes green |
| **QG-INT** | Catalog Schema & Integrity | `dotnet test --filter "FullyQualifiedName~CatalogIntegrity"` | 100% pass; all newly added JSON catalogs pass schema |
| **QG-BLD** | Engine-Free Compilation | `dotnet build Ashfall.csproj` | 0 Errors, 0 Warnings across entire C# solution |

---

# 3. HANDOFF & CLOSURE CHECKLIST

- [ ] All 8 tasks (Q1–Q4, R1–R4) have documented terminal states with evidence recorded in commit history.
- [ ] No Unity dependencies, shims, or references were added to any files.
- [ ] `Assets/Ashfall.Core/` remains strictly engine-free (`netstandard2.1`).
- [ ] All authored JSON data contains `schema_version: 1` and adheres to snake_case field naming.
- [ ] No `System.Random` or unseeded `Guid.NewGuid()` calls exist in deterministic Core logic.
- [ ] No duplicate managers, shadow registries, or parallel save stores were introduced.
- [ ] File claims in `WORKTREE_OWNERSHIP.md` are audited and cleared upon task completion.
- [ ] Focused verification passes for all modified subsystems using `scripts/run_test.sh`.
- [ ] Build succeeds with 0 Errors and 0 Warnings across the entire repository.
