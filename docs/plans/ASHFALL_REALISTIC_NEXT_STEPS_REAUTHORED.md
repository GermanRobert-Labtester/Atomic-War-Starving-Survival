# ASHFALL — Reauthored Realistic Next Steps

> **Status:** Godot-first pre-integration roadmap
> **Active roadmap:** 200 steps
> **Source horizon:** Reauthored from the original 10,000-step generated roadmap and its criticality audit
> **Authority rule:** Godot + .NET is the active runtime; `Ashfall.Core` is the engine-neutral simulation authority; Unity is deprecated migration history.

## Purpose

This document intentionally does **not** preserve the 10,000-step count. It converts the generated horizon into a bounded, dependency-friendly backlog that fits the game's current developmental state. The raw roadmap remains useful as an idea archive, but it is not safe to use directly as an implementation plan.

The primary goal of this rewrite is to move ASHFALL toward a stable, visually complete, testable Godot survival-management build before introducing large new simulation domains.

## Current working assumptions

- Godot is the authoritative engine after the Unity migration.
- `Ashfall.Core` remains the intended simulation/source-of-truth layer.
- The Godot runtime/UI, shared shell/components, and snapshot infrastructure already exist and should be extended rather than replaced.
- The project is systems-heavy and has substantial visual/catalog coverage, but still has a meaningful active missing-art backlog.
- The near-term value is in integration, state correctness, visual completion, content wiring, and playable vertical slices—not civilization-scale feature expansion.

## What was removed from the active roadmap

The following classes of generated work are **not active next steps**:

- orbital warfare, spaceport/sub-orbital restoration, artificial magnetospheres and planetary geoengineering;
- quantum/black-project technology that materially changes the grounded survival genre;
- directed human speciation, gene-editing progression, brain-machine interfaces and post-human systems;
- continental megacity/civilization simulation, millennium-scale simulation and premature franchise-preservation work;
- dedicated servers, matchmaking, anti-cheat and multiplayer synchronization;
- repeated per-feature CI, localization, ObjectDB, packaging, release-tagging and architectural-freeze pseudo-steps;
- industrial/thermodynamic/machining template tasks attached to social, narrative, UI, economy or unrelated systems;
- Gold Master / permanent freeze milestones that occur before the game has reached a stable alpha/beta;
- duplicate or near-duplicate epics that can be represented by one canonical future module.

## Parked for later, not deleted forever

These ideas may be reconsidered after the active roadmap is substantially complete, but they should not enter the current integration DAG: deep-ocean expeditions, armored rail networks, large-scale mining/metallurgy, hydro-dam megaprojects, major civil engineering, advanced industrial chemistry, full geopolitical simulation, extensive maritime warfare, megacity governance, broad modding ecosystem, and large post-launch DLC infrastructure.

## Integration-planning rule

A planning model should process these steps in dependency groups of roughly **5–12 tightly coupled items**. It must inspect the live repository read-only first, verify named files/classes/nodes/resources, and may reorder or merge steps when repository evidence requires it. It must not reintroduce discarded raw-roadmap scope.

---

## Phase 0 — Godot Authority, Migration Closure & Roadmap Sanitation (NS-001–020)

Make the migrated Godot architecture authoritative, remove Unity ambiguity, and establish an evidence-based baseline before feature integration.

### NS-001 — Lock Godot as the Engine Authority

- **Priority:** P0
- **Action:** Add a project architecture note declaring Godot + .NET the only supported runtime host and `Ashfall.Core` the engine-agnostic simulation authority; explicitly mark Unity as deprecated migration history.
- **Exit gate:** Repository docs, agent instructions, and build scripts contain no ambiguity about the active engine.

### NS-002 — Inventory Remaining Unity References

- **Priority:** P0
- **Action:** Search code, data, docs, CI, and tooling for `Unity`, `UnityEngine`, `JsonUtility`, `MonoBehaviour`, `ScriptableObject`, prefab/scene assumptions, and obsolete Unity paths; classify each as delete, port, or archival-only.
- **Exit gate:** Every explicit Unity dependency has an owner and disposition; none is silently reachable by the Godot runtime.

### NS-003 — Replace the Unity Serializer Adapter

- **Priority:** P0
- **Action:** Keep `IJsonSerializer` in Core but move the concrete serializer implementation to the Godot/.NET host boundary; remove any requirement for Core to know Unity serialization types.
- **Exit gate:** Core tests run without Unity assemblies and Godot save/load smoke tests serialize representative state correctly.

### NS-004 — Create a Migration Debt Register

- **Priority:** P0
- **Action:** Create one machine-readable register for unresolved Unity-era files, abandoned assets, stale scripts, duplicate data models, and compatibility shims.
- **Exit gate:** The register is referenced by CI and has zero unowned entries.

### NS-005 — Verify the Godot Version Contract

- **Priority:** P0
- **Action:** Confirm the actual Godot/.NET versions used by the project and reconcile generated roadmap assumptions such as `Godot 4.7+` with the installed/buildable version.
- **Exit gate:** Headless editor launch, .NET build, and project import pass on the authoritative version.

### NS-006 — Establish Canonical Repository Roots

- **Priority:** P0
- **Action:** Document which roots own engine-neutral Core, Godot host code, data catalogs, tests, visual assets, generated artifacts, and archival legacy files.
- **Exit gate:** Every active source file maps to exactly one documented ownership root.

### NS-007 — Quarantine Legacy Unity Assets

- **Priority:** P0
- **Action:** Move or mark Unity-only metadata and unusable import artifacts so Godot import scanning and agent searches cannot mistake them for active assets.
- **Exit gate:** Godot import output contains no Unity-only asset errors and active searches do not resolve to quarantined files.

### NS-008 — Repair the Step 3251–3300 Indexing Defect

- **Priority:** P0
- **Action:** Correct the roadmap metadata where Espionage steps are placed beneath an SMR header; preserve the ideas only as future backlog references.
- **Exit gate:** All retained source-roadmap ranges have internally consistent numbering and phase ownership.

### NS-009 — Remove Premature Gold-Master Milestones

- **Priority:** P0
- **Action:** Delete release freezes, permanent archival seals, and Gold Master milestones from the active development sequence until a playable release candidate actually exists.
- **Exit gate:** No active dependency plan can reach a release-freeze node before alpha/beta acceptance gates.

### NS-010 — Remove Per-Feature Release Tagging

- **Priority:** P0
- **Action:** Replace repeated `Release Candidate Version Tagging` and `Master Phase Architectural Freeze` pseudo-steps with one central release-governance workstream.
- **Exit gate:** Feature plans end in feature acceptance gates, not Git release tags.

### NS-011 — Collapse Repeated 50-Step Matrices

- **Priority:** P0
- **Action:** Treat each generated 50-step late-horizon block as a candidate epic rather than 50 implementation tasks; retain only concerns that actually apply to that feature.
- **Exit gate:** No active feature inherits irrelevant thermodynamics, machining, expedition mapping, ObjectDB, or localization work by template.

### NS-012 — Extract Shared Cross-Cutting Capabilities

- **Priority:** P0
- **Action:** Create a shared list for serialization, save migration, deterministic RNG, event dispatch, test harnesses, localization, input, performance telemetry, packaging, and release automation.
- **Exit gate:** Feature steps reference shared capabilities rather than reimplementing them.

### NS-013 — Define Active / Parked / Rejected Scope States

- **Priority:** P0
- **Action:** Require every roadmap item to be classified as Active-Now, Post-Alpha, Expansion-Candidate, Lore-Only, or Rejected.
- **Exit gate:** No unclassified generated step can enter integration planning.

### NS-014 — Reject Multiplayer and Server Infrastructure

- **Priority:** P0
- **Action:** Remove matchmaking, dedicated server, anti-cheat, online leaderboard, server-cluster, and multiplayer synchronization work from the active single-player roadmap.
- **Exit gate:** Build graph contains no multiplayer/server dependencies.

### NS-015 — Park Civilization-Scale Reconstruction

- **Priority:** P0
- **Action:** Move megacities, continental infrastructure, planetary-scale industry, and millennium simulation concepts out of the active roadmap.
- **Exit gate:** Active roadmap remains bounded to the current survival-management game and immediate expansions.

### NS-016 — Reject Off-Direction High Technology

- **Priority:** P0
- **Action:** Remove or quarantine quantum systems, artificial magnetospheres, post-human speciation, brain-machine interfaces, advanced orbital warfare, and similar high-tech drift unless separately approved as lore.
- **Exit gate:** No active gameplay dependency requires speculative high technology.

### NS-017 — Create a Live-Repo Evidence Rule

- **Priority:** P0
- **Action:** Require future planners to prove file/class/node/resource existence before naming it in an implementation plan; unverifiable claims must be marked as hypotheses.
- **Exit gate:** Planning documents distinguish observed repository facts from proposed additions.

### NS-018 — Create a Step Acceptance Template

- **Priority:** P0
- **Action:** Standardize each real step to include objective, affected subsystem, dependencies, implementation boundary, save/data impact, validation command, and exit criteria.
- **Exit gate:** No active step consists only of a title plus a generic boilerplate implementation sentence.

### NS-019 — Create a Dependency-ID Convention

- **Priority:** P0
- **Action:** Assign stable IDs to shared capabilities, feature epics, migration tasks, tests, and content packs so Qwen/DeepSeek can build a DAG without relying on prose order.
- **Exit gate:** All 200 steps in this document have stable IDs and dependency references can be added without renumbering.

### NS-020 — Freeze This 200-Step Roadmap as the Planning Input

- **Priority:** P0
- **Action:** Use this reauthored document—not the raw 10,000-step generation—as the default source for master integration planning; retain the raw files only as an idea archive.
- **Exit gate:** Planning agents are explicitly instructed not to resurrect discarded raw steps unless asked.

---

## Phase 1 — Core Contracts, Save/Restore, Determinism & Data Integrity (NS-021–040)

Stabilize the simulation substrate so later UI/content work does not build on unsafe state, migration, RNG, or catalog behavior.

### NS-021 — Audit Core Engine Neutrality

- **Priority:** P0
- **Action:** Sweep `Ashfall.Core` for Godot host types as well as Unity remnants; keep nodes/resources/presentation code outside simulation assemblies.
- **Exit gate:** Core unit tests compile and execute without starting Godot.

### NS-022 — Finalize the Serialization Contract

- **Priority:** P0
- **Action:** Define supported primitives, collections, enums, IDs, nullable values, version fields, and error behavior for the host serializer adapter used by save and catalog layers.
- **Exit gate:** Round-trip tests pass for representative save DTOs and malformed payloads fail deterministically.

### NS-023 — Migrate Remaining Save I/O Through the Contract

- **Priority:** P0
- **Action:** Remove direct serializer calls from active save paths and route capture/restore through one versioned serialization boundary.
- **Exit gate:** Repository search shows no active bypass of the canonical save serializer.

### NS-024 — Complete Save Capture for World Evolution

- **Priority:** P0
- **Action:** Implement and test capture/restore for location evolution, wildlife, landmarks, and any other runtime state currently omitted from snapshots.
- **Exit gate:** Save → mutate → load restores each tested subsystem byte/logically equivalent to its captured state.

### NS-025 — Define Save Restore Ordering

- **Priority:** P0
- **Action:** Document and enforce the restore sequence for clock, catalogs, survivor state, shelter state, economy, quests, world state, and presenters.
- **Exit gate:** Automated restore tests prove no subsystem reads dependent state before it is restored.

### NS-026 — Suppress Runtime Events During Restore

- **Priority:** P0
- **Action:** Centralize event suppression or restore-mode semantics so loading a save cannot emit gameplay side effects such as quest progress, trades, injuries, or notifications.
- **Exit gate:** Instrumented load tests produce zero gameplay-domain events until restore completion.

### NS-027 — Unify Event Dispatch Semantics

- **Priority:** P0
- **Action:** Resolve parallel string-based and generic event buses into one canonical Core event-dispatch contract with typed payloads or stable IDs.
- **Exit gate:** No active system requires both legacy event mechanisms.

### NS-028 — Consolidate Simulation Clocks

- **Priority:** P0
- **Action:** Unify day-based and tick-based clocks behind one hierarchy that supports deterministic simulation, UI time, scheduled events, and save/restore.
- **Exit gate:** Clock tests reproduce identical scheduled outcomes across save/load and different frame rates.

### NS-029 — Verify Seeded RNG Stream Isolation

- **Priority:** P0
- **Action:** Assign deterministic RNG streams to combat, encounters, economy, loot, weather/world evolution, and narrative selection where randomness is intended.
- **Exit gate:** Replaying the same seed and inputs produces identical outcomes; consuming one stream does not perturb another.

### NS-030 — Eliminate Unseeded Simulation Randomness

- **Priority:** P0
- **Action:** Search Core for system/random calls that can alter game state and replace them with the appropriate seeded stream.
- **Exit gate:** Determinism test suite flags zero uncontrolled state-affecting RNG calls.

### NS-031 — Standardize ID Comparison

- **Priority:** P0
- **Action:** Use ordinal, culture-independent comparisons for catalog IDs, quest IDs, faction IDs, location IDs, and save keys.
- **Exit gate:** Tests under alternate locales resolve the same IDs and ordering.

### NS-032 — Unify Worn Gear Models

- **Priority:** P0
- **Action:** Collapse radiation/inventory duplicate worn-gear representations into one canonical equipment model with explicit radiation, armor, durability, and slot data.
- **Exit gate:** A survivor has one authoritative equipment state across inventory, health, combat, and save/load.

### NS-033 — Normalize Catalog Schemas

- **Priority:** P0
- **Action:** Standardize active JSON field naming, required schema versioning, ID prefixes, nullability, and backward-compatible defaults.
- **Exit gate:** Catalog validation runs across all active data with zero schema violations.

### NS-034 — Build Catalog Migration Utilities

- **Priority:** P0
- **Action:** Provide deterministic migration for legacy JSON records rather than hand-editing incompatible files during runtime integration.
- **Exit gate:** Representative old catalogs migrate to current schema with stable IDs and no data loss.

### NS-035 — Harden Catalog Referential Integrity

- **Priority:** P0
- **Action:** Validate every cross-reference among items, recipes, factions, quests, locations, encounters, survivors, NPCs, and visual asset IDs.
- **Exit gate:** CI rejects dangling active references while allowing explicitly marked optional/archival references.

### NS-036 — Create Data Provenance Flags

- **Priority:** P0
- **Action:** Mark records as core-game, expansion, test fixture, deprecated, or archival so agents cannot accidentally wire dormant content into the live game.
- **Exit gate:** Runtime loaders include only intended provenance classes for the selected build.

### NS-037 — Add Save Corruption Envelope Tests

- **Priority:** P0
- **Action:** Test truncated payloads, invalid checksums, wrong schema versions, unknown IDs, duplicate IDs, and impossible state combinations.
- **Exit gate:** Corrupt saves fail safely with actionable diagnostics and never partially apply state.

### NS-038 — Add Save Migration Chain Tests

- **Priority:** P0
- **Action:** Verify supported migrations in sequence rather than only current-version round trips.
- **Exit gate:** Every supported historical fixture reaches the current schema or fails with an explicit unsupported-version error.

### NS-039 — Define Core Performance Budgets

- **Priority:** P0
- **Action:** Measure tick-loop allocations and execution time for the current simulation size; optimize only demonstrated hotspots rather than enforcing universal zero-allocation dogma.
- **Exit gate:** Budgets are hardware/test-scene specific and regressions are detectable in CI or profiling runs.

### NS-040 — Replace Test-Count Ratchets with Behavior Gates

- **Priority:** P0
- **Action:** Track required scenario coverage, critical invariants, and regression fixtures instead of requiring raw test counts to monotonically increase.
- **Exit gate:** Removing redundant tests does not fail CI if required behavior coverage remains intact.

---

## Phase 2 — Godot Host, Bridge, Snapshot Harness & Runtime Lifecycle (NS-041–060)

Make the Godot presentation/runtime layer a stable consumer of Core state with reliable lifecycle, bridge, and headless validation.

### NS-041 — Modularize the Godot Main Entry Point

- **Priority:** P0
- **Action:** Keep startup orchestration thin; move save bootstrap, data loading, input, audio, UI shell, and simulation hosting into explicit services/autoloads or owned nodes.
- **Exit gate:** Main entry code has clear responsibilities and can be smoke-tested headlessly.

### NS-042 — Define the Core↔Godot Bridge Boundary

- **Priority:** P0
- **Action:** Specify which DTOs/events cross from Core to Godot and which user actions cross back; prevent UI nodes from directly mutating simulation internals.
- **Exit gate:** Bridge API surface is documented and covered by contract tests.

### NS-043 — Audit Existing Bridge Shims

- **Priority:** P0
- **Action:** Verify every compatibility shim is still required after migration; remove Unity-era or temporary adapters that duplicate the canonical bridge.
- **Exit gate:** No active shim lacks an owner, removal condition, or test.

### NS-044 — Harden Autoload Initialization Order

- **Priority:** P0
- **Action:** Make dependencies between data registry, simulation host, save service, input service, audio service, and UI shell explicit.
- **Exit gate:** Cold boot and load-game boot succeed repeatedly without order-dependent null/state errors.

### NS-045 — Create a Runtime Readiness State

- **Priority:** P0
- **Action:** Expose boot stages such as DataLoaded, SimulationReady, SaveRestored, UIReady, and Playable so screens do not race initialization.
- **Exit gate:** Automated startup asserts valid state transitions and rejects invalid ones.

### NS-046 — Centralize Godot Error Reporting

- **Priority:** P0
- **Action:** Route uncaught domain/bridge/presenter failures into a structured error sink with subsystem, step, current scene/screen, and save context.
- **Exit gate:** Headless and interactive failures produce actionable logs without swallowing exceptions.

### NS-047 — Validate SnapshotHarness Ownership

- **Priority:** P0
- **Action:** Document the existing snapshot harness entry points, expected fixtures, output paths, and test-scene dependencies.
- **Exit gate:** All known snapshot commands run from a clean checkout without manual editor state.

### NS-048 — Stabilize SnapshotOrchestrator

- **Priority:** P0
- **Action:** Ensure snapshot orchestration waits for deterministic UI-ready conditions instead of arbitrary delays.
- **Exit gate:** Repeated runs produce stable captures and no intermittent missing-widget frames.

### NS-049 — Expand Snapshot Coverage by Screen State

- **Priority:** P0
- **Action:** Capture empty, populated, selected, warning, failure, modal, and overflow states for high-priority screens rather than one happy-path image.
- **Exit gate:** Each critical UI screen has representative state coverage.

### NS-050 — Add Snapshot Metadata

- **Priority:** P0
- **Action:** Record viewport, locale, UI scale, data fixture, theme version, and Git revision beside each capture.
- **Exit gate:** Visual diffs can be reproduced from metadata alone.

### NS-051 — Create a Headless UI Smoke Suite

- **Priority:** P0
- **Action:** Instantiate high-priority scenes/screens, bind representative DTOs, navigate focus, and close them without rendering-dependent crashes.
- **Exit gate:** Suite completes with zero unhandled Godot errors.

### NS-052 — Add Node/Signal Leak Checks

- **Priority:** P0
- **Action:** Use Godot-appropriate lifecycle/ObjectDB diagnostics as a shared QA capability, not duplicated feature steps.
- **Exit gate:** Repeated screen open/close and save/load loops return tracked node/resource counts to expected baselines.

### NS-053 — Audit Signal Connections

- **Priority:** P0
- **Action:** Detect duplicate connections, stale callbacks, and signals connected to freed nodes across reusable screens.
- **Exit gate:** Runtime diagnostics show no duplicate signal invocation in tested flows.

### NS-054 — Centralize Presenter Subscription Lifetime

- **Priority:** P0
- **Action:** Make presenter/event subscriptions explicitly disposable or lifecycle-bound so scene changes cannot leave ghost listeners.
- **Exit gate:** Navigation stress tests show no duplicated notifications after repeated transitions.

### NS-055 — Define Scene Transition Contracts

- **Priority:** P0
- **Action:** Specify which state is preserved across main menu, shelter, map, location/encounter, combat, and results transitions.
- **Exit gate:** Transition tests preserve intended state and clear transient state.

### NS-056 — Add Crash-Safe Autosave Boundaries

- **Priority:** P0
- **Action:** Choose deterministic points where autosave is allowed and prohibit saves during partial transitions/restores.
- **Exit gate:** Forced termination after each tested boundary recovers to a valid save.

### NS-057 — Create Godot Resource Loading Rules

- **Priority:** P0
- **Action:** Standardize synchronous/asynchronous loading expectations, caching, fallbacks, and missing-resource diagnostics.
- **Exit gate:** Missing active resources produce explicit errors or approved placeholders, never silent blank UI.

### NS-058 — Audit Input Routing

- **Priority:** P0
- **Action:** Ensure global shortcuts, focused controls, modal dialogs, map navigation, and gameplay commands do not compete for the same events.
- **Exit gate:** Automated input tests show one intended consumer per action.

### NS-059 — Create Runtime Feature Flags

- **Priority:** P0
- **Action:** Gate incomplete systems and experimental content behind explicit flags rather than partially wiring them into normal play.
- **Exit gate:** Release/default configuration cannot enter unfinished feature paths.

### NS-060 — Document the Godot Runtime Architecture

- **Priority:** P0
- **Action:** Produce a concise host diagram covering autoloads/services, Core bridge, UI shell, save flow, scene flow, and test harnesses.
- **Exit gate:** A new planning agent can understand runtime ownership without reverse-engineering `Main` first.

---

## Phase 3 — UI Shell, Navigation & High-Priority Screen Integration (NS-061–085)

Finish a coherent playable interface using the existing Godot UI shell/components before adding more simulation breadth.

### NS-061 — Audit the Existing UI Component Inventory

- **Priority:** P1
- **Action:** Confirm current reusable shell, sidebar, status rail, metric card, data grid, frame helpers, dialogs, buttons, tabs, and typography primitives; mark duplicates for removal.
- **Exit gate:** Every high-priority screen has a defined component composition path.

### NS-062 — Lock Theme Tokens

- **Priority:** P1
- **Action:** Centralize palette, spacing, typography, border, focus, warning, radiation, scarcity, and disabled-state tokens in the Godot theme layer.
- **Exit gate:** Snapshot diff shows no screen-specific hard-coded styling where tokens exist.

### NS-063 — Finish Main Menu Integration

- **Priority:** P1
- **Action:** Wire New Game, Load, Settings, Credits, and Quit to actual runtime services with disabled/error states.
- **Exit gate:** All menu actions work in headless/input smoke tests and interactive Linux run.

### NS-064 — Finish New Game Setup

- **Priority:** P1
- **Action:** Provide difficulty/profile selection only for settings currently supported by Core; avoid speculative setup options.
- **Exit gate:** Starting a new game creates a valid deterministic initial state and enters the shelter.

### NS-065 — Finish Load Game Browser

- **Priority:** P1
- **Action:** Display slot metadata, schema/version compatibility, corruption state, and safe failure messages.
- **Exit gate:** Valid fixtures load; incompatible/corrupt fixtures are non-destructive and clearly explained.

### NS-066 — Finish Settings Screen

- **Priority:** P1
- **Action:** Wire audio, display/window mode, UI scale, accessibility, input remapping, and persistence to real configuration storage.
- **Exit gate:** Restarting the game preserves supported settings.

### NS-067 — Finish Shelter Dashboard

- **Priority:** P1
- **Action:** Bind core survivor, resource, shelter-condition, time, radiation, power, water, food, and alert summaries to live state.
- **Exit gate:** Dashboard values update from simulation events without polling-related drift.

### NS-068 — Finish Survivor Roster Screen

- **Priority:** P1
- **Action:** Show survivor condition, assignment, injuries/illness, fatigue, morale, equipment, and availability using one canonical survivor DTO.
- **Exit gate:** Roster matches Core state across save/load and assignment changes.

### NS-069 — Finish Survivor Detail Screen

- **Priority:** P1
- **Action:** Expose actionable health/status information without leaking internal implementation fields.
- **Exit gate:** All displayed statuses have source-of-truth mappings and tooltips where needed.

### NS-070 — Finish Room Assignment UI

- **Priority:** P1
- **Action:** Bind shelter rooms/workstations and survivor shifts with capacity, skill, health, and availability validation.
- **Exit gate:** Invalid assignments are blocked consistently in UI and Core.

### NS-071 — Finish Workbench/Crafting UI

- **Priority:** P1
- **Action:** Present recipe requirements, tools, time, output, unavailable reasons, and queue state.
- **Exit gate:** Crafting actions produce exactly the Core-defined inventory/resource changes.

### NS-072 — Finish Inventory UI

- **Priority:** P1
- **Action:** Unify item filtering, stack quantities, condition/durability, weight/space if supported, equipment transfers, and item detail presentation.
- **Exit gate:** Inventory interactions survive save/load with no duplication or item loss.

### NS-073 — Finish Medical Triage UI

- **Priority:** P1
- **Action:** Present wounds, disease, radiation burden, treatment options, resource costs, urgency, and contraindications supported by current systems.
- **Exit gate:** Treatments invoke canonical medical actions and update state once.

### NS-074 — Finish Trade Screen

- **Priority:** P1
- **Action:** Bind merchant/faction inventory, valuation, scarcity modifiers, reputation effects, and transaction validation.
- **Exit gate:** Trade totals exactly match Core economy calculations.

### NS-075 — Finish Map Screen

- **Priority:** P1
- **Action:** Display discovered locations, travel cost/time, hazard summary, accessibility, quest markers, and currently supported route state.
- **Exit gate:** Selecting a valid destination creates the same expedition/travel request Core expects.

### NS-076 — Finish Location/Encounter Screen

- **Priority:** P1
- **Action:** Render encounter text, participants, available actions, requirements, consequences preview where intended, and resolved outcomes.
- **Exit gate:** Every offered choice maps to a valid encounter action ID.

### NS-077 — Finish Quest Journal

- **Priority:** P1
- **Action:** Show active/completed/failed quests, objectives, dependencies, deadlines, and faction/NPC context without revealing hidden branches.
- **Exit gate:** Journal state exactly follows quest system state across save/load.

### NS-078 — Finish Radio/Intercept Screen

- **Priority:** P1
- **Action:** Wire currently supported faction broadcasts/intercepts and related quest hooks; defer unnecessary signal-simulation complexity.
- **Exit gate:** Known intercept fixtures display, acknowledge, and persist correctly.

### NS-079 — Finish Power/Utilities Panel

- **Priority:** P1
- **Action:** Expose power generation/load, water status, heating/ventilation and critical failures only to the level already modeled by Core.
- **Exit gate:** Panel warnings correspond to actual simulation thresholds.

### NS-080 — Finish End-of-Day Summary

- **Priority:** P1
- **Action:** Summarize resource deltas, injuries, events, completed work, quest changes, and alerts with links to relevant screens.
- **Exit gate:** Summary is generated from recorded day events, not recomputed inconsistently from final state.

### NS-081 — Finish Modal/Notification System

- **Priority:** P1
- **Action:** Standardize confirmations, irreversible actions, warnings, errors, and non-blocking notices.
- **Exit gate:** Keyboard/controller focus cannot escape active modal boundaries.

### NS-082 — Finish Tooltip System

- **Priority:** P1
- **Action:** Use one tooltip pipeline for items, statuses, icons, statistics, and disabled-action reasons.
- **Exit gate:** No critical icon-only state lacks an accessible text explanation.

### NS-083 — Finish Keyboard Navigation

- **Priority:** P1
- **Action:** Define predictable focus order and shortcuts for every P0/P1 screen.
- **Exit gate:** A full shelter-management loop is operable without a mouse.

### NS-084 — Finish Controller Navigation

- **Priority:** P1
- **Action:** Map major navigation/actions to controller input where feasible without compromising desktop controls.
- **Exit gate:** Core UI loop passes controller smoke navigation with no focus traps.

### NS-085 — Validate Multi-Resolution Layout

- **Priority:** P1
- **Action:** Test the supported desktop/Steam Deck viewport set for clipping, overlap, unreadable text, and inaccessible controls.
- **Exit gate:** Snapshot suite has zero blocker layout regressions at target resolutions.

---

## Phase 4 — Visual Asset Coverage & Runtime Presentation (NS-086–110)

Close the real missing-art gaps and make existing content visually complete before generating speculative new domains.

### NS-086 — Rebuild the Visual Asset Manifest

- **Priority:** P1
- **Action:** Generate one authoritative manifest mapping active catalog records to runtime visual IDs, resolved files, aliases, placeholders, and missing status.
- **Exit gate:** Manifest totals reproduce the active runtime asset audit deterministically.

### NS-087 — Separate Missing Art from Reference-Only Entries

- **Priority:** P1
- **Action:** Preserve the distinction between assets the player can actually encounter and documentation/legacy references that do not require art.
- **Exit gate:** No reference-only record is counted as a shipping art blocker.

### NS-088 — Prioritize Runtime-Visible Missing Art

- **Priority:** P1
- **Action:** Rank missing visuals by frequency and player exposure: shelter/UI essentials first, then survivors/NPCs, then frequently visited locations, then low-frequency items.
- **Exit gate:** Asset backlog has a clear P0/P1/P2 priority based on runtime usage.

### NS-089 — Close Survivor Portrait Gaps

- **Priority:** P1
- **Action:** Create/import and bind missing survivor portraits to canonical survivor IDs using the established ASHFALL art direction.
- **Exit gate:** Every active selectable survivor resolves to the intended portrait with no fallback.

### NS-090 — Close NPC Portrait Gaps

- **Priority:** P1
- **Action:** Create/import and bind portraits for NPCs that appear in active quests, trade, radio, or encounters before background-only NPCs.
- **Exit gate:** Every currently reachable named NPC presentation resolves correctly.

### NS-091 — Close High-Frequency Item Icon Gaps

- **Priority:** P1
- **Action:** Prioritize food, water, medicine, tools, materials, weapons/ammunition already reachable in gameplay, and critical quest items.
- **Exit gate:** Top runtime inventory fixtures show no missing icons.

### NS-092 — Close Active Location Art Gaps

- **Priority:** P1
- **Action:** Prioritize locations already reachable from the map/quest graph instead of trying to illustrate every generated future location.
- **Exit gate:** All locations in the current playable route graph have approved visual presentation.

### NS-093 — Create Placeholder Governance

- **Priority:** P1
- **Action:** Allow deliberate placeholders only when explicitly tagged with owner/replacement milestone; prohibit silent runtime fallback.
- **Exit gate:** Runtime can report every placeholder currently visible to the player.

### NS-094 — Deduplicate Near-Identical Assets

- **Priority:** P1
- **Action:** Merge literal duplicates and approved semantic aliases while preserving distinct art that communicates different state or identity.
- **Exit gate:** Manifest has no redundant identical runtime assets under different canonical IDs.

### NS-095 — Normalize Import Settings

- **Priority:** P1
- **Action:** Standardize filtering, mipmaps, compression, pixel/sprite handling, texture size, and alpha treatment by asset class.
- **Exit gate:** Godot import warnings are clean and comparable assets use consistent presets.

### NS-096 — Fix 9-Slice UI Assets

- **Priority:** P1
- **Action:** Validate margins and scaling for panels, headers, frames, buttons, and status elements at all supported UI scales.
- **Exit gate:** Snapshot suite shows no stretched corners or seam artifacts.

### NS-097 — Normalize Icon Alpha and Padding

- **Priority:** P1
- **Action:** Apply consistent transparent bounds, optical centering, and silhouette readability to active item/status icons.
- **Exit gate:** Inventory/data-grid rows maintain stable alignment across mixed icons.

### NS-098 — Lock Portrait Crop Rules

- **Priority:** P1
- **Action:** Standardize aspect ratio, safe crop, focal placement, border treatment, and fallback behavior for survivor/NPC portraits.
- **Exit gate:** Portraits do not jump in framing between roster/detail/dialog views.

### NS-099 — Integrate Shelter Interior Cutaway

- **Priority:** P1
- **Action:** Bind room states, damage/repair overlays, occupancy and work activity to the current shelter presentation without requiring a full new simulation system.
- **Exit gate:** Representative shelter fixtures display correct room/state overlays.

### NS-100 — Integrate Room Degradation Overlays

- **Priority:** P1
- **Action:** Visualize existing damage/repair/contamination states with bounded overlay variants rather than separate bespoke scenes for every state.
- **Exit gate:** Changing room state updates the intended overlay deterministically.

### NS-101 — Integrate Radiation Presentation

- **Priority:** P1
- **Action:** Use restrained radiation cues tied to real exposure/environment thresholds; avoid constant full-screen effects.
- **Exit gate:** Visual severity tracks configured state bands and is disabled by accessibility setting where appropriate.

### NS-102 — Integrate Weather/Ash Presentation

- **Priority:** P1
- **Action:** Add lightweight ash/snow/wind/visibility effects driven by existing world/weather state rather than a new climate simulator.
- **Exit gate:** Effects switch correctly under deterministic weather fixtures and remain within performance budget.

### NS-103 — Integrate Day/Night Lighting States

- **Priority:** P1
- **Action:** Drive shelter/surface lighting variants from the canonical simulation clock.
- **Exit gate:** Snapshot fixtures at target times reproduce expected lighting states.

### NS-104 — Integrate Map Fog/Discovery Presentation

- **Priority:** P1
- **Action:** Bind discovery state and inaccessible/unknown regions to the map visuals.
- **Exit gate:** New discoveries reveal only intended nodes/paths.

### NS-105 — Integrate Combat Feedback Assets

- **Priority:** P1
- **Action:** Use readable, restrained hit/miss/cover/suppression/injury feedback without requiring advanced ballistic visual simulation.
- **Exit gate:** Combat test fixtures display the correct feedback for each resolved event.

### NS-106 — Integrate Medical Feedback Assets

- **Priority:** P1
- **Action:** Add wound/status/treatment indicators required to understand medical decisions; defer graphic surgical effects.
- **Exit gate:** Medical UI communicates urgency and treatment result without missing visuals.

### NS-107 — Create Visual Asset Validation CI

- **Priority:** P1
- **Action:** Fail CI for missing files, invalid manifest IDs, bad dimensions where constrained, unreadable formats, and active fallback usage.
- **Exit gate:** Asset validation runs headlessly and produces actionable record-level diagnostics.

### NS-108 — Add Runtime Missing-Asset Telemetry

- **Priority:** P1
- **Action:** In dev builds, log exact catalog ID, requested visual ID, screen/location, and fallback path when a visual lookup fails.
- **Exit gate:** QA can reproduce a missing visual from the log alone.

### NS-109 — Build Representative Visual QA Fixtures

- **Priority:** P1
- **Action:** Create curated fixture saves/scenes containing diverse survivors, inventory, locations, alerts, trade, medical and encounter states.
- **Exit gate:** One command populates stable visual-regression scenes.

### NS-110 — Declare Visual-Complete Criteria for Alpha

- **Priority:** P1
- **Action:** Define which active screens, characters, items, locations, states, and overlays must have final or approved temporary art before alpha.
- **Exit gate:** Art progress can be measured against a finite, runtime-derived list.

---

## Phase 5 — Playable Survival Loop & Shelter Management (NS-111–130)

Turn the stable core/UI into a reliable day-to-day survival loop before expanding simulation breadth.

### NS-111 — Define the Canonical Day Loop

- **Priority:** P1
- **Action:** Document the playable sequence from morning state review through assignments, crafting/trade/medical decisions, expedition actions, event resolution, and end-of-day advancement.
- **Exit gate:** A scripted fixture can execute the full loop without entering undefined states.

### NS-112 — Verify Survivor Needs Progression

- **Priority:** P1
- **Action:** Audit hunger, thirst, fatigue, temperature/exposure, morale/stress, and other currently modeled needs for update cadence and bounds.
- **Exit gate:** Multi-day deterministic tests show no negative values, runaway growth, or contradictory status bands.

### NS-113 — Verify Resource Consumption

- **Priority:** P1
- **Action:** Ensure food, water, fuel/power and medical/tool consumption occurs exactly once at documented phases of the day/tick loop.
- **Exit gate:** Resource accounting reconciles starting stock, gains, consumption and ending stock.

### NS-114 — Harden Work Shift Assignment

- **Priority:** P1
- **Action:** Validate availability, capacity, skill effects, injury restrictions, fatigue cost, and interruption behavior.
- **Exit gate:** Invalid assignments cannot enter Core through UI, save restore, or scripted actions.

### NS-115 — Harden Crafting Queue

- **Priority:** P1
- **Action:** Define start, progress, pause, cancel, completion, missing-resource, worker-unavailable and save/load behavior.
- **Exit gate:** Crafting queue survives save/load at each lifecycle point without duplication.

### NS-116 — Harden Shelter Maintenance

- **Priority:** P1
- **Action:** Use current room/equipment condition to generate repair tasks, resource costs and consequences; avoid simulating industrial engineering beyond gameplay needs.
- **Exit gate:** Damage → repair → restored-state loop is deterministic and visible in UI.

### NS-117 — Harden Power Management

- **Priority:** P1
- **Action:** Define generator/source capacity, prioritized loads, shortages and failure consequences at the abstraction level supported by the game.
- **Exit gate:** Power deficits disable/degrade the correct consumers and recover predictably.

### NS-118 — Harden Water Management

- **Priority:** P1
- **Action:** Define storage, daily consumption, contamination/quality if currently modeled, purification actions and shortage consequences.
- **Exit gate:** Water accounting and health consequences reconcile in deterministic tests.

### NS-119 — Harden Heating/Ventilation

- **Priority:** P1
- **Action:** Connect environmental shelter state to survivor risk using simple documented thresholds rather than full thermodynamic simulation.
- **Exit gate:** Cold/ventilation fixtures produce bounded, explainable health/morale effects.

### NS-120 — Harden Food/Ration Management

- **Priority:** P1
- **Action:** Ensure ration selection, nutritional abstraction, spoilage if supported, and survivor-specific restrictions are consistent.
- **Exit gate:** Ration decisions produce expected inventory and need-state changes.

### NS-121 — Integrate Bunker Maintenance Events

- **Priority:** P1
- **Action:** Add a bounded pool of failures—leaks, broken fixtures, blocked ventilation, damaged wiring, sanitation problems—using existing repair/task systems.
- **Exit gate:** Events create solvable tasks with clear consequences and no bespoke subsystem required per event.

### NS-122 — Integrate Greenhouse as a Focused Shelter Module

- **Priority:** P1
- **Action:** Implement only the existing/near-term crop loop: plant, tend, resource input, growth stages, harvest, failure; defer genetic/agronomic mega-simulation.
- **Exit gate:** A crop can progress through all supported states across save/load.

### NS-123 — Integrate Basic Desalination/Purification if Already Supported

- **Priority:** P1
- **Action:** Expose water-processing recipes/actions through the existing resource/crafting framework rather than creating an industrial fluid simulator.
- **Exit gate:** Inputs, time, outputs and failure states are testable and balanced.

### NS-124 — Integrate Basic Charcoal/Retort Crafting if Already Supported

- **Priority:** P1
- **Action:** Treat charcoal/pyrolysis as recipes/workstation jobs with hazards/outputs appropriate to survival gameplay, not chemical-plant simulation.
- **Exit gate:** Representative jobs complete through the normal crafting/work system.

### NS-125 — Integrate Memorial/Funeral Consequences

- **Priority:** P1
- **Action:** Connect survivor death to roster removal, morale/relationship effects, memorial log and relevant quest/event hooks.
- **Exit gate:** Death resolution occurs once and survives save/load without resurrecting or double-processing the survivor.

### NS-126 — Integrate Caregiving/Dependent Events

- **Priority:** P1
- **Action:** Use existing survivor/task/event systems for caregiving trade-offs rather than building a separate society simulator.
- **Exit gate:** Events impose clear time/resource/morale choices and resolve deterministically.

### NS-127 — Integrate Ration Compounding Recipes

- **Priority:** P1
- **Action:** Add grounded food preparation/ration variants through the canonical recipe/catalog system.
- **Exit gate:** Recipes obey schema, economy and inventory validation.

### NS-128 — Add Shortage Escalation Events

- **Priority:** P1
- **Action:** Create bounded event sets for low food, water, medicine, fuel/power and shelter condition.
- **Exit gate:** Events trigger from documented thresholds and do not spam repeatedly without state change.

### NS-129 — Add Shelter Crisis Recovery Paths

- **Priority:** P1
- **Action:** For every critical shortage/failure that can occur in the current loop, define at least one player-accessible recovery path or explicit loss condition.
- **Exit gate:** Automated scenario audit finds no unavoidable soft-lock caused solely by missing actions.

### NS-130 — Build the First 30-Day Survival Validation

- **Priority:** P1
- **Action:** Run deterministic campaigns through the pre-nuclear/civil-war early arc with scripted policy variants to identify impossible economies or dead-end progression.
- **Exit gate:** At least several distinct strategies remain viable through the intended early milestone.

---

## Phase 6 — Medical, Radiation, Psychology & Survivor Consequences (NS-131–150)

Deepen survivor stakes using bounded, readable systems that reuse current health/status infrastructure.

### NS-131 — Unify the Health-State Model

- **Priority:** P2
- **Action:** Ensure wounds, disease, radiation, fatigue, stress/morale and treatment state have one authoritative survivor-health representation.
- **Exit gate:** UI, save, quests and encounters read the same canonical state.

### NS-132 — Prioritize Clinically Distinct Injury Classes

- **Priority:** P2
- **Action:** Keep gameplay-relevant categories such as bleeding, fracture, burn, infection risk and general trauma rather than over-modeling anatomy.
- **Exit gate:** Each class has distinct treatment/penalty behavior and clear UI communication.

### NS-133 — Harden Bleeding Resolution

- **Priority:** P2
- **Action:** Define severity bands, ongoing loss/penalty, stabilization actions, treatment resource use and failure outcomes.
- **Exit gate:** Bleeding fixtures progress and resolve deterministically without duplicate damage ticks.

### NS-134 — Harden Fracture/Mobility Consequences

- **Priority:** P2
- **Action:** Connect serious injury to work/travel/combat availability and recovery state at a manageable abstraction.
- **Exit gate:** Assignment and expedition validation correctly respects mobility restrictions.

### NS-135 — Harden Infection Progression

- **Priority:** P2
- **Action:** Link untreated wounds/illness to infection risk, treatment availability and recovery/failure without simulating unnecessary microbiology.
- **Exit gate:** Risk and treatment outcomes are bounded and reproducible.

### NS-136 — Harden Disease/Epidemic Events

- **Priority:** P2
- **Action:** Use a small set of transmissible conditions with exposure, symptoms, isolation/quarantine decisions and treatment/support.
- **Exit gate:** Outbreak fixtures demonstrate spread control and avoid exponential simulation cost.

### NS-137 — Harden Acute Radiation Sickness

- **Priority:** P2
- **Action:** Map accumulated exposure into understandable ARS severity bands, symptoms, work/combat penalties and treatment/support.
- **Exit gate:** Exposure fixtures cross expected thresholds and recover/decline according to documented rules.

### NS-138 — Clarify Potassium Iodide Behavior

- **Priority:** P2
- **Action:** Represent KI only for the intended thyroid-protection context; do not present it as a universal radiation cure.
- **Exit gate:** Tooltip/mechanics match the specific preventive effect implemented.

### NS-139 — Remove Premature Gene-Editing Systems

- **Priority:** P2
- **Action:** Drop DNA synthesis, radiation-resistance gene editing, directed speciation and post-human progression from the active health roadmap.
- **Exit gate:** No current medical progression depends on speculative genetic technology.

### NS-140 — Harden Stress/Morale Accumulation

- **Priority:** P2
- **Action:** Define sources, recovery, caps/floors and interaction with survivor traits/relationships at the level already supported.
- **Exit gate:** Long-run tests avoid permanent runaway stress from repeated minor events.

### NS-141 — Integrate Trauma/Flashback Events

- **Priority:** P2
- **Action:** Use authored events and temporary statuses for trauma consequences rather than a full psychiatric simulation engine.
- **Exit gate:** Events respect cooldowns, survivor history and save persistence.

### NS-142 — Integrate Mourning

- **Priority:** P2
- **Action:** Connect deaths/major losses to affected survivors and bounded morale/relationship consequences.
- **Exit gate:** Mourning triggers once per relevant loss and decays/resolves as designed.

### NS-143 — Integrate Addiction Only Where Content Supports It

- **Priority:** P2
- **Action:** Keep dependency/withdrawal mechanics only for substances already present and narratively meaningful; discard generic chemical-simulation expansion.
- **Exit gate:** Every dependency state has obtainable causes, effects, and recovery/management routes.

### NS-144 — Build Treatment Decision Validation

- **Priority:** P2
- **Action:** Centralize eligibility, cost, contraindication and expected effect checks so UI and encounters cannot disagree.
- **Exit gate:** The same treatment request yields the same validation result from every caller.

### NS-145 — Add Medical Resource Scarcity Hooks

- **Priority:** P2
- **Action:** Tie medicine/clean supplies availability into trade, crafting/loot, shelter decisions and quest consequences without multiplying item variants unnecessarily.
- **Exit gate:** Scarcity influences decisions but does not make baseline recovery impossible.

### NS-146 — Add Quarantine Room/Assignment Hooks

- **Priority:** P2
- **Action:** Reuse room/work assignment systems to isolate contagious survivors if such shelter space exists.
- **Exit gate:** Isolation changes exposure risk and assignment availability consistently.

### NS-147 — Add Survivor Incapacitation State

- **Priority:** P2
- **Action:** Define when a survivor cannot work/travel/fight but remains alive and treatable.
- **Exit gate:** All systems respect incapacitation without removing the survivor from save/roster incorrectly.

### NS-148 — Add Medical Outcome Logging

- **Priority:** P2
- **Action:** Record major diagnoses, treatment, deterioration and recovery events for end-of-day summary and debugging.
- **Exit gate:** Health transitions can be reconstructed from logs in deterministic fixtures.

### NS-149 — Balance Recovery Times

- **Priority:** P2
- **Action:** Calibrate recovery against campaign length and gameplay tempo rather than real-time medical simulation detail.
- **Exit gate:** Injuries matter for multiple decisions without routinely removing characters for impractically long periods.

### NS-150 — Build Medical/Radiation Regression Scenarios

- **Priority:** P2
- **Action:** Create fixtures for injury, infection, outbreak, radiation exposure, treatment, death and recovery paths.
- **Exit gate:** Medical suite catches state-ordering, save/load and UI synchronization regressions.

---

## Phase 7 — Economy, Factions, Trade, Radio & Social Pressure (NS-151–170)

Create meaningful external pressure and faction choice without attempting continent-scale geopolitics.

### NS-151 — Audit the Existing Dynamic Economy

- **Priority:** P2
- **Action:** Identify current price inputs, scarcity modifiers, faction modifiers, stock refresh, barter rules and save state; remove duplicate experimental calculators.
- **Exit gate:** One canonical function produces transaction values.

### NS-152 — Bound Price Shock Behavior

- **Priority:** P2
- **Action:** Keep shortages/surpluses within configurable ranges and add damping/cooldowns to prevent oscillation exploits.
- **Exit gate:** Stress tests remain inside defined price bands unless a scripted crisis overrides them.

### NS-153 — Integrate Scarcity Warnings

- **Priority:** P2
- **Action:** Expose only meaningful scarcity levels in trade/shelter UI with reasons based on actual economy state.
- **Exit gate:** Warning badges correspond to reproducible thresholds.

### NS-154 — Harden Merchant Inventory Refresh

- **Priority:** P2
- **Action:** Define deterministic stock generation/refresh using faction/location context and seeded randomness.
- **Exit gate:** Save/load and repeated seeds preserve expected merchant stock.

### NS-155 — Harden Transaction Atomicity

- **Priority:** P2
- **Action:** Validate complete barter/trade result before moving any items/reputation/currency-equivalent value.
- **Exit gate:** Rejected transactions produce zero partial inventory changes.

### NS-156 — Define Faction Reputation Bands

- **Priority:** P2
- **Action:** Use a finite set of relationship bands with explicit unlocks/penalties rather than continuous hidden complexity everywhere.
- **Exit gate:** Actions cross thresholds predictably and UI reflects the current band.

### NS-157 — Integrate Faction Trade Modifiers

- **Priority:** P2
- **Action:** Apply reputation, scarcity and faction policy through one valuation path.
- **Exit gate:** The same item/merchant state produces one price regardless of screen or caller.

### NS-158 — Integrate Radio Broadcasts as Faction Signals

- **Priority:** P2
- **Action:** Use authored/procedural broadcast entries to foreshadow shortages, conflict, quests and reputation changes; defer full radio-physics simulation.
- **Exit gate:** Broadcast state can unlock/update content and persists across save/load.

### NS-159 — Integrate Radio Intercepts

- **Priority:** P2
- **Action:** Allow selected intercepts to reveal locations, quests, warnings or faction intel when conditions are met.
- **Exit gate:** Intercept rewards/hooks fire once and are recorded.

### NS-160 — Integrate Courier/Dispatch Events

- **Priority:** P2
- **Action:** Use the existing event/quest system for messages, deliveries and timed decisions instead of building a national courier simulator.
- **Exit gate:** Dispatches have sender, deadline, payload, resolution and failure behavior.

### NS-161 — Add Basic Parley/Surrender Choices

- **Priority:** P2
- **Action:** Where encounters support negotiation, define a small reusable action contract for parley, intimidation, payment, retreat or surrender.
- **Exit gate:** Encounter choices validate against faction/reputation/inventory state.

### NS-162 — Add Basic Faction Succession Events

- **Priority:** P2
- **Action:** Represent leadership change as authored state transitions that alter reputation/quests/trade modifiers, not a full political simulation.
- **Exit gate:** Succession fixtures update affected content without orphaned references.

### NS-163 — Add Refugee/Shelter Request Events

- **Priority:** P2
- **Action:** Create bounded decisions involving food, space, security, reputation and survivor morale.
- **Exit gate:** Each event has explicit state changes and no permanent unresolved guest entities.

### NS-164 — Add Smuggling/Black-Market Encounters

- **Priority:** P2
- **Action:** Use trade/encounter systems to model risk/reward and faction consequences without a separate macro-war economy.
- **Exit gate:** Outcomes affect inventory/reputation and are auditable.

### NS-165 — Add Bribe/Corruption Choices Where Narratively Supported

- **Priority:** P2
- **Action:** Treat corruption as authored encounter/quest choices with explicit costs and consequences.
- **Exit gate:** No universal corruption subsystem is required to support isolated content.

### NS-166 — Add Faction Conflict World Flags

- **Priority:** P2
- **Action:** Track a small set of conflict states that gate encounters, travel risk, trade availability and quest branches.
- **Exit gate:** Flags have defined transitions and save persistence.

### NS-167 — Reject Territory-Conquest Simulation for Now

- **Priority:** P2
- **Action:** Do not implement annexation, vassalization, grand strategy doctrine or continent-wide war AI in the current roadmap.
- **Exit gate:** Current faction systems remain compatible with authored regional changes.

### NS-168 — Build Economy Exploit Tests

- **Priority:** P2
- **Action:** Test buy/sell loops, rounding, repeated reloads, stock refresh, barter bundles, negative/overflow values and reputation-price feedback.
- **Exit gate:** No tested loop creates unbounded value from zero-risk repetition.

### NS-169 — Build Faction State Regression Fixtures

- **Priority:** P2
- **Action:** Create saves covering hostile/neutral/friendly factions, leadership change, active embargo/shortage, and quest-gated trade.
- **Exit gate:** Trade, radio and quest UI render correct behavior for each fixture.

### NS-170 — Define the Faction Expansion Boundary

- **Priority:** P2
- **Action:** Document which faction mechanics are core now and which macro-geopolitical concepts remain expansion candidates.
- **Exit gate:** Integration planners cannot silently promote parked grand-strategy features.

---

## Phase 8 — Quests, Locations, Encounters, Exploration & Focused Combat (NS-171–185)

Convert authored content into a reliable playable route graph and keep combat tactical but appropriately scoped.

### NS-171 — Audit the Reachable Quest Graph

- **Priority:** P2
- **Action:** Enumerate quests currently reachable from new game through the intended early/mid campaign; identify orphaned objectives, impossible prerequisites and dead branches.
- **Exit gate:** Every active quest has at least one valid entry and terminal outcome.

### NS-172 — Harden Quest State Transitions

- **Priority:** P2
- **Action:** Centralize start, objective progress, branch, complete, fail, cancel/expire and reward behavior.
- **Exit gate:** Quest transitions are idempotent and survive save/load.

### NS-173 — Verify Expansion Quest Packs Individually

- **Priority:** P2
- **Action:** Treat Holdfast, Duty Roster, Standing Record, Nobody's Charter, Verdict, Year of Ash and other existing packs as separate content modules with explicit prerequisites.
- **Exit gate:** Each enabled pack passes its own graph/reference validation before activation.

### NS-174 — Audit Active Location Graph

- **Priority:** P2
- **Action:** List locations reachable in the current campaign, their unlock conditions, travel links, hazards, encounter tables and required visuals.
- **Exit gate:** No reachable location lacks required data or an intentional fallback.

### NS-175 — Normalize Location IDs and Metadata

- **Priority:** P2
- **Action:** Standardize name, region, discovery state, travel cost/time, hazard tags, faction control if used, visual ID and encounter references.
- **Exit gate:** All active locations pass catalog integrity validation.

### NS-176 — Harden Travel Resolution

- **Priority:** P2
- **Action:** Define departure validation, time/resource cost, survivor availability, interruption/encounter hooks, arrival and return state.
- **Exit gate:** Travel cannot duplicate survivors/items or leave the party between locations after save/load.

### NS-177 — Create a Focused Encounter Contract

- **Priority:** P2
- **Action:** Standardize participants, conditions, choices, checks, costs, rewards, faction/quest effects and terminal outcomes.
- **Exit gate:** Encounter definitions validate before runtime and every presented action has a handler.

### NS-178 — Audit Encounter Reachability

- **Priority:** P2
- **Action:** Detect encounters whose conditions can never be satisfied or whose rewards/requirements reference unavailable content.
- **Exit gate:** Active encounter pool contains no known unreachable definitions.

### NS-179 — Add Encounter Cooldowns/Uniqueness Rules

- **Priority:** P2
- **Action:** Prevent one-time story events from repeating and routine encounters from spamming without intended cooldowns.
- **Exit gate:** Seeded long-run tests respect frequency/uniqueness constraints.

### NS-180 — Integrate Shelter Return Resolution

- **Priority:** P2
- **Action:** Ensure expedition results, injuries, loot, quest progress, time and faction consequences apply exactly once when returning.
- **Exit gate:** Save/load around return cannot duplicate rewards or consequences.

### NS-181 — Keep Combat at Tactical-Core Scope

- **Priority:** P2
- **Action:** Prioritize cover, accuracy/range, suppression/morale if already supported, wounds, ammo, equipment condition and retreat; defer advanced physics and combined-arms simulation.
- **Exit gate:** Combat decisions are readable and resolve through deterministic Core rules.

### NS-182 — Harden Combat Turn/Action Validation

- **Priority:** P2
- **Action:** Reject actions for dead/incapacitated actors, invalid targets, unavailable ammo/equipment, blocked ranges or already-consumed turns.
- **Exit gate:** Invalid scripted actions produce no partial state changes.

### NS-183 — Harden Cover and Hit Resolution

- **Priority:** P2
- **Action:** Use a bounded abstraction for cover/line-of-fire rather than full material/spall/ricochet simulation unless already implemented.
- **Exit gate:** Known combat fixtures reproduce expected hit/cover outcomes.

### NS-184 — Integrate Morale/Surrender Where Supported

- **Priority:** P2
- **Action:** Allow enemies to flee/surrender based on bounded morale rules and encounter context rather than elaborate ideological AI.
- **Exit gate:** Surrender/flee outcomes resolve combat and downstream encounter state correctly.

### NS-185 — Build Exploration/Combat Regression Scenarios

- **Priority:** P2
- **Action:** Create fixtures spanning travel → encounter → combat/negotiation → loot/injury → return → save/load.
- **Exit gate:** End-to-end scenarios reconcile survivor, inventory, time, quest and faction state.

---

## Phase 9 — Audio, Accessibility, QA, Performance & Playable-Milestone Exit Gates (NS-186–200)

Use the existing build/test infrastructure to prove the game is stable and presentable on Linux/Steam Deck before adding major new systems.

### NS-186 — Integrate Critical Diegetic Audio

- **Priority:** P0/P1 QA
- **Action:** Prioritize UI feedback, shelter ambience, Geiger/radiation cues, machinery warnings, weather and encounter/combat readability before expanding the soundtrack.
- **Exit gate:** Every critical warning has a distinguishable audio or accessible visual equivalent.

### NS-187 — Harden Dynamic Music State Changes

- **Priority:** P0/P1 QA
- **Action:** Drive music intensity/state from a small documented set of game states and prevent rapid oscillation.
- **Exit gate:** Deterministic state fixtures select expected music layers/cues.

### NS-188 — Finish Audio Settings and Dynamic-Range Options

- **Priority:** P0/P1 QA
- **Action:** Provide master/category volume and practical dynamic-range/night-mode controls if supported by the audio stack.
- **Exit gate:** Settings persist and can mute/reduce critical categories without errors.

### NS-189 — Finish Accessibility Controls

- **Priority:** P0/P1 QA
- **Action:** Cover UI scale, text readability, motion/shake reduction, flashing/glitch reduction, color-independent status cues, and input remapping where feasible.
- **Exit gate:** Accessibility settings alter presentation without changing simulation state.

### NS-190 — Expand Headless Core/Bridge Test Battery

- **Priority:** P0/P1 QA
- **Action:** Prioritize save/restore, catalog integrity, bridge contracts, day loop, medical, economy, quests, travel and combat regression scenarios.
- **Exit gate:** Critical-path suite passes from a clean checkout.

### NS-191 — Expand Visual Regression Battery

- **Priority:** P0/P1 QA
- **Action:** Use the existing snapshot harness for all P0/P1 screens and representative states at supported viewports.
- **Exit gate:** Diff review contains no unexplained layout/asset regressions.

### NS-192 — Run Long-Session Simulation Stress Tests

- **Priority:** P0/P1 QA
- **Action:** Exercise multi-day/month campaigns using seeded scripted decisions to find leaks, runaway values, event storms and state divergence.
- **Exit gate:** Runs complete within defined memory/time budgets and produce valid saves.

### NS-193 — Run Repeated Save/Load Stress Tests

- **Priority:** P0/P1 QA
- **Action:** Loop save/restore around day transitions, travel, encounters, crafting, medical treatment and quest updates.
- **Exit gate:** No tested boundary accumulates duplicated events, entities or resource changes.

### NS-194 — Profile Godot Runtime Hotspots

- **Priority:** P0/P1 QA
- **Action:** Measure actual frame-time, allocation, UI update and resource-loading costs in representative shelter/map/encounter scenes.
- **Exit gate:** Optimization work is backed by profiler evidence and target-hardware budgets.

### NS-195 — Validate Linux Wayland/XWayland

- **Priority:** P0/P1 QA
- **Action:** Test launch, input, window/fullscreen behavior, file paths, audio, controller, save paths and clean exit under supported Linux display modes.
- **Exit gate:** No blocker platform-specific runtime issue remains.

### NS-196 — Validate Steam Deck Target Profile

- **Priority:** P0/P1 QA
- **Action:** Test target resolution, controller navigation, font/UI scale, performance, suspend/resume expectations if supported, and power-sensitive settings.
- **Exit gate:** Representative gameplay remains usable within documented performance targets.

### NS-197 — Add Crash/Diagnostic Bundle Export

- **Priority:** P0/P1 QA
- **Action:** Provide a dev/QA path to collect logs, build/version, save metadata, recent events and relevant configuration without exposing unrelated user data.
- **Exit gate:** A bug report can be reproduced from the bundle more reliably than from screenshots alone.

### NS-198 — Build a Public-Demo Candidate Slice

- **Priority:** P0/P1 QA
- **Action:** Select a bounded starting scenario that demonstrates shelter management, survivor decisions, one expedition chain, trade/medical pressure and a narrative hook.
- **Exit gate:** The slice is completable from start to finish without dev console intervention.

### NS-199 — Define Alpha Exit Criteria

- **Priority:** P0/P1 QA
- **Action:** Require core loop completion, save stability, no active fallback art on critical paths, P0/P1 UI completion, validated quest/location slice, and blocker-free Linux runtime.
- **Exit gate:** Alpha status is a checklist of observable gates, not a date or subjective label.

### NS-200 — Run the Final Pre-Integration Planning Gate

- **Priority:** P0/P1 QA
- **Action:** Before creating another feature wave, feed only unresolved/accepted steps from this document plus live-repository evidence to the master planner and retire completed steps.
- **Exit gate:** The next roadmap is generated from actual remaining work rather than reusing the raw 10,000-step archive.

---

## Recommended master-planning batches

Do not hand all 200 items to an implementation agent at once. A master planner may see the full document to build the dependency DAG, but detailed implementation planning should be batched:

1. **Batch A — NS-001–020:** authority, migration closure, roadmap sanitation.
2. **Batch B — NS-021–040:** Core/save/data/determinism.
3. **Batch C — NS-041–060:** Godot host/bridge/snapshot/runtime lifecycle.
4. **Batch D — NS-061–085:** UI shell and high-priority screens.
5. **Batch E — NS-086–110:** visual asset remediation and runtime presentation.
6. **Batch F — NS-111–130:** playable survival/shelter loop.
7. **Batch G — NS-131–150:** medical/radiation/psychology.
8. **Batch H — NS-151–170:** economy/factions/trade/radio.
9. **Batch I — NS-171–185:** quests/locations/encounters/focused combat.
10. **Batch J — NS-186–200:** audio/accessibility/QA/performance/demo gates.

Within each batch, the detailed planner should reduce the work further into implementation slices that can be completed, compiled/tested, and validated before the next slice begins.

## Definition of 'ready for integration planning'

A step is ready only when the planner can identify: existing repository evidence; precise affected systems; dependencies; data/save impact; UI/presenter impact if any; implementation order; test/validation commands; rollback boundary; and measurable completion criteria.

Anything that cannot meet that standard remains a backlog idea, not an executable task.
