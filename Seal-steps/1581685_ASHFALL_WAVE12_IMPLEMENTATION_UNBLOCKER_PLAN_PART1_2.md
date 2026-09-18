# ASHFALL — GENERATION WAVE 12 — IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 1.2
// SPDX-License-Identifier: MIT

**Document role:** execution-grade Part 1.2 continuation for Generation Wave 12, derived from the Antigravity-authored B-series task set and the final A-series handoff requirements established by Wave 12 Part 1.1.

**Scope of Part 1.2**
1. **Task B1 — C2[15] Input Reality, Focus Navigation, Controller Parity & Legibility (Plan 37A/37B/37C)**
2. **Task B2 — C2[16] Session Durability, Long-Run Soak & Player-Safe Save Recovery (Plan 39A/39B/39C)**
3. **Task B3 — C2[17] Authored Survivor Identity, Declared Item Tags & Diegetic Personality (Plan 40A/40B/40C)**
4. **Task B4 — C2[18] Deterministic Survivor Voice, Delivery Contracts & Social Speech (Plan 42A/42B/42C)**

**Part 1.2 purpose:** close the second half of the Wave 12 Part 1 frontier by making ASHFALL genuinely playable without mouse dependence, durable under long-running deterministic sessions and save failures, explicit about authored survivor/item identity, and coherent in survivor voice delivery without runtime-generated dialogue.

**Critical relationship to Part 1.1:** Part 1.2 must consume the final merged outputs of A1–A4:
- A1 narrative/content rails and utilization truth;
- A2 focusable spatial surfaces and lifecycle discipline;
- A3 tooling/verification and Core engine-free gates;
- A4 medical ownership/read-model/save contracts.

Part 1.2 must not assume the draft examples are still exact at repository `HEAD`.

---

# 0. OPERATING CONTRACT

## 0.1 Allowed terminal states

Each B-series task must terminate as exactly one of:

- **IMPLEMENTED** — missing mechanism, contract, binding, or surface is authored through the canonical owner and all required focused gates are green.
- **DECIDED-DEFERRED** — execution depends on a signed product/architecture decision and the exact remainder is explicitly recorded.
- **RETIRED** — obsolete action, shim, provider, catalog row, or duplicate mechanism is deleted/unregistered with reference proof.
- **VERIFIED-RESOLVED** — current `HEAD` already satisfies the blocker; redundant implementation is skipped and evidence is recorded.
- **ROUTED-REPAIR** — investigation exposes a real defect outside the bounded plan; a repair package with characterization evidence is registered.

No task may close as “mostly done,” “works on my machine,” “future,” “manual QA only,” or “TBD”.

## 0.2 Non-negotiable hard rules

1. **Godot remains authoritative; Unity stays retired.**
2. **Core stays engine-free.** No Godot/engine references may enter `Assets/Ashfall.Core/`.
3. **Deterministic domain behavior remains deterministic.** No wall-clock timing, frame rate, controller hardware jitter, or filesystem ordering may influence Core simulation results.
4. **UI input handling remains presentation/host concern.** Panels may not mutate domain state directly.
5. **One input authority.** No per-panel rogue keycode systems if a central action/route contract exists.
6. **One save authority.** Session durability extends the existing save orchestration; it does not add a parallel save store.
7. **One identity authority.** Survivor beliefs/professions/keepsakes/item tags must be canonical data or canonical runtime state, not inferred in multiple systems.
8. **One voice authority.** B4 must not create a second narrative dispatcher, second audio manager, or dynamic dialogue generator.
9. **Claims before edits.**
10. **Each task retains exactly 20 procedural substeps.**
11. **Each named mini-task retains exactly four mini-substeps.**
12. **Focused tests first.**
13. **Production code preserves the warning baseline.**
14. **Save recovery must never silently mutate valid player state.**
15. **Corrupt-save fallback must be explicit and auditable.**
16. **Real-time UI timing may use engine time; Core simulation may not.**
17. **Content/data examples are illustrative until reverified against current catalogs.**
18. **No hardcoded “O(1)”, “200-hour”, “60 seconds”, “28 surfaces”, or catalog-count acceptance unless the current plan/test policy defines it.**
19. **Generated docs/indexes are regenerated through source tooling.**
20. **Every task writes an implementation log and updates census/ledger truth.**

## 0.3 Universal pre-edit evidence bundle

Capture:
- current commit;
- worktree status;
- active claims;
- source-plan revision;
- current census row;
- Part 1.1 handoff packet;
- current focused-test baseline;
- current input map/save store/identity/voice owner maps;
- current generated index status.

## 0.4 Universal stop conditions

Stop and route if:
- a required path is actively claimed;
- current owner differs materially from historical plan assumptions;
- controller parity requires a new gameplay action not defined by design;
- save recovery semantics could overwrite a valid primary with uncertain fallback data;
- belief/profession/keepsake data lacks an authoritative schema;
- item tags would change economy/medical behavior without signed data semantics;
- voice selection requires wall-clock state in Core;
- a voice line needs dynamic runtime text generation not allowed by Plan 42.

---

# 1. DEPENDENCY GRAPH

## 1.1 Recommended order

**B1 → B2 → B3 → B4**

Why:
- B1 depends directly on A2’s focusable UI surfaces.
- B2 validates the durability of all Part 1.1 save/lifecycle changes before identity/voice add more long-lived state.
- B3 establishes authored survivor identity and tags that B4 may consume for voice filtering/personality.
- B4 is safest after identity, narrative rails, and audio lifecycle are stable.

The current census DAG may override this order.

## 1.2 Part 1.1 dependencies

### B1 consumes
- A2 focus topology;
- A2 route/host command patterns;
- A2 lifecycle cleanup;
- A3 verification gate stability.

### B2 consumes
- A1/A4 final save contracts;
- A2 lifecycle stress;
- A3 save/build verification tooling.

### B3 consumes
- A1 content acceptance/reachability rails;
- A3 data/tag integrity gates;
- A4 survivor-state authority matrix.

### B4 consumes
- A1 narrative/radio reachability;
- B3 authored identity if Plan 42 uses traits/beliefs;
- A2/B1 UI accessibility/input conventions;
- existing audio ducking/concurrency authority.

---

# 2. TASK B1 — C2[15] INPUT REALITY, FOCUS NAVIGATION, CONTROLLER PARITY & LEGIBILITY

**Source:** `C-integration-plans/C2_planintegration[15].md` — Plan 37A/37B/37C\
**Blocker class:** ACCESSIBILITY & INPUT DEFECT / CONTROLLER GAP\
**Canonical ownership:** current `AshfallInputActions`, GameFlow/input router, Godot UI navigation layer\
**Primary acceptance:** a complete representative campaign-day journey can be performed without mouse input, with deterministic command outcomes, stable focus, accessible state cues, and persistent user rebindings.

## 2.1 Objective

Make input a coherent semantic contract rather than a collection of local shortcuts. The task must reconcile action declarations, focus routing, modal containment, controller parity, text legibility, dynamic help prompts, and persistent rebindings.

The central goal is **mouse-free functional parity**, not merely “controller buttons exist”.

## 2.2 Exact 20 procedural substeps

1. Reverify the current input architecture: `AshfallInputActions`, `project.godot` input map, GameFlow/router, panel-local handlers, and any rebinding/profile store.
2. Define the canonical representative mouse-free campaign-day journey and acceptance actions from Plan 37/current UX.
3. Claim current input-action, game-flow/router, focus-controller, rebinding, prompt-glyph, test, and documentation paths.
4. Audit every declared `ashfall_*` semantic action and classify it as handled, reserved, orphaned, duplicate, or retired.
5. Consolidate global semantic input routing through the current central GameFlow/input dispatcher while preserving legitimate control-local Godot navigation behavior.
6. Build/validate deterministic focus graphs so every visible enabled interactive control has a legal focus path and no hidden/background escape.
7. Map `ui_up/down/left/right` and current semantic navigation actions cleanly across lists, grids, tab containers, and spatial surfaces.
8. Standardize back/cancel behavior across modals, panels, overlays, and HUD return using the current route stack rather than panel-specific destruction.
9. Implement/align focus styling with current design tokens and non-color-only indicators; do not hardcode a 1px border if the current design system defines another token.
10. Preserve/restore focus origin across panel/modal transitions where the router supports it, with deterministic fallback if the origin is gone/disabled.
11. Reconcile keyboard/controller bindings for all required semantic actions, including D-pad, stick, face buttons, shoulder/trigger actions only where Plan 37 defines them.
12. Apply controller deadzone/repeat handling through Godot/current input settings so analog drift cannot cause focus jitter or repeated activation.
13. Implement/verify supported UI text scaling levels from current accessibility policy and fix clipping/layout regressions without creating a second layout mode.
14. Bind input-help glyphs to the current active-device detector and action map so keyboard/controller prompts update without hardcoded strings.
15. Add focused input/focus-navigation tests in the current UI/host test layer, keeping Godot-specific tests out of engine-free Core.
16. Add a headless/engine integration mouse-free campaign-day journey that dispatches semantic input actions and verifies committed domain outcomes.
17. Add modal focus-isolation tests proving background/hidden controls cannot receive focus or activation while a modal owns input.
18. Run the current UI accessibility/navigation gate and report the actual current surface count rather than assuming “28+”.
19. Verify rebinding persistence using the existing user/profile settings store, including reset/default and invalid/conflicting binding behavior from Plan 37.
20. Update the current input/navigation guide and hand off with action map, focus graph evidence, mouse-free journey results, rebinding persistence proof, and zero-warning build.

## 2.3 Mini-task B1.1 — Input Map Reconciliation & Cleanup

### B1.1.a
Compare `project.godot` action declarations against the canonical `AshfallInputActions` identifiers and build a declaration/handler matrix.

### B1.1.b
Remove or route duplicated hardcoded gameplay key/button checks through semantic actions while leaving legitimate text-entry/control-local Godot behavior intact.

### B1.1.c
Add only plan-required missing controller bindings for secondary semantic actions such as inspect/quick-assign or current equivalents.

### B1.1.d
Add a verification test/gate proving every canonical action ID exists in the Godot InputMap and every required declared action has an intended handler/consumer.

## 2.4 Mini-task B1.2 — Deterministic Focus Graph Traversal

### B1.2.a
Implement/reuse a focus-graph helper only where container defaults cannot express the required deterministic navigation; do not recursively overwrite hand-authored special-case routes blindly.

### B1.2.b
Clamp modal focus/navigation to the modal hierarchy and restore background focus only after the modal closes.

### B1.2.c
Assign deterministic default focus for every panel/popup based on current visible/enabled controls and router state.

### B1.2.d
Add traversal tests for representative management surfaces such as roster, inventory, trade, medical, Holdfast, and map panels using current route names.

## 2.5 Mini-task B1.3 — Gamepad Input Parity & Deadzones

### B1.3.a
Map stick/D-pad navigation through Godot semantic UI actions and current repeat/pulse handling rather than synthesizing domain commands from raw axes.

### B1.3.b
Configure deadzone thresholds from current project accessibility/input policy; treat `0.25` as a draft example unless Plan 37 explicitly defines it.

### B1.3.c
Map shoulder/trigger controls to tab/page switching only on surfaces where the UX contract defines those actions.

### B1.3.d
Verify active-device detection updates controller/keyboard glyph prompts without changing gameplay state or consuming domain RNG.

## 2.6 Mini-task B1.4 — Mouse-Free Day Simulation Gate

### B1.4.a
Create the mouse-free journey in the Godot/host integration test layer, not in engine-free Core if synthesized input requires Godot APIs.

### B1.4.b
Exercise the plan-defined representative actions: advance day, assign a worker, perform a medical action, inspect briefing, navigate spatial/map surface, and return to HUD.

### B1.4.c
Assert committed domain state through canonical owners and verify no direct mouse event is required by the journey.

### B1.4.d
Compare terminal domain state with an equivalent canonical command/UI path and require semantic equality, not byte equality of transient UI state.

## 2.7 Input action classification

| Action | Declared? | Canonical constant? | Handler? | Required? | Classification | Action |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

Classifications:
- LIVE;
- RESERVED;
- ORPHANED;
- DUPLICATE;
- RETIRED;
- DECISION-BLOCKED.

## 2.8 Central routing boundary

Central router owns:
- global semantic commands;
- route-level back/cancel;
- active modal/panel input ownership.

Controls may still own:
- text editing;
- slider/list intrinsic behavior;
- local Godot focus traversal.

Do not funnel every raw event through one monolith if the engine already provides correct control semantics.

## 2.9 Focus graph contract

Every visible enabled interactive control must be:
- reachable;
- escapable unless inside a modal;
- deterministic under repeated navigation;
- excluded when hidden/disabled.

Focus should never land on an invisible background control.

## 2.10 Spatial surfaces handoff

Consume A2’s focus topology rather than rebuilding spatial navigation from scratch.

B1 owns:
- controller parity;
- semantic action consistency;
- rebinding/help prompts.

## 2.11 Cancel/back precedence

Recommended plan-driven precedence:
1. close top modal;
2. close current subpanel/detail;
3. return to prior panel/HUD;
4. only then open pause/back-to-menu semantics if current UX says.

Use current route stack.

## 2.12 Focus restoration

Store an ephemeral control/route origin reference, not save state.

If origin disappears:
- use deterministic route default.

## 2.13 Deadzone and repeat semantics

Deadzone/repeat are presentation/input concerns.

They must not:
- alter campaign simulation;
- depend on frame rate in a way that can double-commit commands;
- convert one held axis into repeated activation without controlled navigation repeat.

## 2.14 Text scaling

Scale support must preserve:
- readable labels;
- no clipped critical controls;
- scroll where needed;
- focus visibility.

Do not promise fixed-layout perfection if the current design system permits adaptive containers.

## 2.15 Active-device glyphs

Device detector changes **presentation glyphs only**.

No persistent gameplay effect.

## 2.16 Rebinding store

Use the current user/profile settings store.

Test:
- custom binding;
- restart/reload;
- conflict handling;
- reset default;
- invalid action ID;
- current version migration if applicable.

## 2.17 Mouse-free parity oracle

Mouse-free journey is accepted if:
- all required actions reachable;
- all domain outcomes match canonical command semantics;
- no hidden focus trap;
- no modal leak;
- no mouse-specific handler is required.

## 2.18 B1 save impact

Only user input settings persist.

No campaign save changes.

## 2.19 B1 deterministic boundary

Input timing may vary in presentation, but domain commands must commit exactly once and produce the same simulation outcome when the same semantic action sequence is applied to the same state.

## 2.20 B1 terminal acceptance

B1 reaches IMPLEMENTED when the action map is reconciled, orphan/duplicate actions are classified, representative mouse-free gameplay succeeds, modal/focus isolation is correct, controller prompts/rebindings persist, accessibility gates pass, and no rogue panel input path bypasses canonical command routing.

---

# 3. TASK B2 — C2[16] SESSION DURABILITY, LONG-RUN SOAK & PLAYER-SAFE SAVE RECOVERY

**Source:** `C-integration-plans/C2_planintegration[16].md` — Plan 39A/39B/39C\
**Blocker class:** PERSISTENCE INTEGRITY / LONG-RUN STABILITY\
**Canonical ownership:** existing Core Save orchestration + Host persistence layer\
**Primary acceptance:** deterministic long-run campaign stability, valid primary/backup behavior, atomic writes, corruption rejection, migration safety, and player-safe recovery with no silent data invention.

## 3.1 Objective

Prove ASHFALL can survive long-running campaigns and save failures without drift, leaks, duplicate event replay, or destructive recovery.

The Antigravity draft mixes “200-hour-class” durability with “200 simulated days.” Part 1.2 must resolve the plan’s actual soak contract:
- if Plan 39 defines **200 simulated campaign days**, test that;
- if it defines a **200-hour-class equivalent**, use the repository-approved accelerated surrogate plus any required wall-time soak tier;
- do not claim 200 real hours from a short unit test.

## 3.2 Exact 20 procedural substeps

1. Reverify the current save architecture: checksums, envelope/versioning, atomic temp-write/replace behavior, backup rotation, store ownership, and recovery UX.
2. Read Plan 39's exact durability target and distinguish simulated campaign-duration soak from real wall-clock soak requirements.
3. Claim current save orchestrator, persistence harness, backup/recovery tests, migration tests, save docs, and any host notification seam.
4. Implement/extend the deterministic long-run soak harness for the plan-defined campaign duration and scenario mix.
5. Measure managed allocations, retained object/node counts, open handles/resources, and other supported leak indicators using the correct runtime layer; do not require Core tests to measure Godot node counts.
6. Save at the terminal soak point and restore into a **fresh composition/session**, then compare authoritative state/fingerprint against continuous execution.
7. Verify/implement backup fallback so a checksum-invalid primary can recover from a **validated** backup according to current save policy.
8. Return typed/user-visible recovery/corruption outcomes rather than crashing, freezing, or silently replacing state.
9. Reconcile “missing section” handling with schema migration/default policy; do not generically invent defaults for sections whose absence is invalid corruption.
10. Verify all supported legacy envelope migrations preserve valid progress and fail closed on unsupported/corrupt versions.
11. Add corruption fuzz/byte-mutation tests over test payloads and require safe rejection or documented fallback.
12. Verify atomic write semantics using injected I/O failures at controlled phases and prove the last known-good save remains loadable.
13. Audit all textual numeric serialization/parsing for culture independence where custom formatting exists; serializer-native invariant numeric behavior need not be redundantly rewritten.
14. Reconcile save metadata timestamps with current UTC/ISO contract, keeping wall-clock metadata separate from deterministic campaign state.
15. Add focused Plan 39 durability/recovery tests at the current save-test location.
16. Test saves taken during representative high-state-density situations using deterministic scenarios; avoid coupling save logic to UI/combat presentation.
17. Run the current comprehensive corruption/migration battery and report actual case counts/results rather than assuming “100%” or a fixed filename.
18. Verify save/reload does not duplicate one-shot events, scheduled effects, quest rewards, voice/journal entries, or replay state.
19. Run data-integrity checks for save-coupled IDs/catalog references and confirm no new invalid references.
20. Update the save-store contract matrix with durability/recovery guarantees and hand off with soak methodology, recovery matrix, atomic-write proof, migration results, and zero-warning build.

## 3.3 Mini-task B2.1 — Deterministic Long-Run Soak

### B2.1.a
Create/extend a headless campaign soak harness in the correct Core/integration test layer using the plan-defined duration and deterministic seed.

### B2.1.b
Exercise representative weather, needs/consumption, expeditions, production, events, medical state, and save checkpoints according to current campaign systems.

### B2.1.c
Record supported memory/resource metrics and verify stabilization/no monotonic leak using a repository-approved threshold/method rather than an invented absolute number.

### B2.1.d
Compare terminal authoritative state/fingerprint against an equivalent repeated run and a fresh-session restore at the terminal checkpoint.

## 3.4 Mini-task B2.2 — Backup Slot Rotation & Fallback Loading

### B2.2.a
Verify the exact current write/rotation sequence for primary and backup, ensuring only a **validated last-known-good** save becomes backup.

### B2.2.b
When primary validation fails, attempt the backup only after validating its checksum/version/schema; never load an unvalidated backup.

### B2.2.c
Surface a typed high-priority recovery notification through the current player-notification/briefing host seam without mutating domain history.

### B2.2.d
Test recovery from truncated, zero-byte, invalid syntax, checksum-invalid, and unsupported-version primaries with both valid and invalid backups.

## 3.5 Mini-task B2.3 — Mid-Write Crash Simulation & Atomic Protection

### B2.3.a
Verify current save writing uses temp/staging plus atomic replace/rename semantics appropriate to supported platforms.

### B2.3.b
Inject deterministic I/O failures before write completion, before replace, and during cleanup using a test filesystem abstraction where available.

### B2.3.c
Assert the previous valid primary remains byte-valid/loadable whenever the atomic contract says it should.

### B2.3.d
Verify stale temp/scratch files are ignored or safely cleaned on next boot without being mistaken for authoritative saves.

## 3.6 Mini-task B2.4 — Culture-Invariant Number Formatting Enforcement

### B2.4.a
Audit custom string formatting/parsing in save code for floats/decimals/dates; do not rewrite serializer-native JSON numeric handling unnecessarily.

### B2.4.b
Apply `InvariantCulture` only to custom textual numeric/date serialization where the current contract requires it.

### B2.4.c
Run round-trip tests under at least one comma-decimal locale and one dot-decimal locale.

### B2.4.d
Require semantic/canonical serialization parity according to the current checksum contract; byte-identical output is required only if canonical serialization guarantees it.

## 3.7 Save architecture census

| Store/section | Owner | Version | Checksum | Backup? | Migration | Recovery semantics |
|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |

## 3.8 Soak terminology

Do not use “200-hour soak” unless real wall-clock 200h execution occurred or Plan 39 explicitly defines an accelerated equivalent.

Preferred reporting:
- **200 simulated campaign days**;
- **N accelerated deterministic cycles**;
- **wall-clock soak tier** if separately run.

## 3.9 Memory/resource measurement boundary

Core soak may measure:
- managed heap;
- object counts exposed by harness;
- collection sizes;
- open streams if instrumented.

Godot node/unmanaged resource leak checks belong to host/engine integration tests.

## 3.10 Golden-state policy

Prefer structural/fingerprint comparison over a giant brittle serialized golden file.

If a golden exists:
- canonical;
- versioned;
- reviewed when changed.

## 3.11 Backup rotation invariant

A corrupt current primary must never overwrite the last valid backup.

Define write order from current contract.

## 3.12 Fallback transparency

Backup recovery should be visible:
- typed result;
- player notice;
- logs.

No silent “success” that hides corruption.

## 3.13 Missing-section policy

Classify each missing section:
- optional/defaultable;
- legacy migration;
- required/corrupt.

A generic “repair all missing keys” utility is unsafe unless the save schema explicitly defines defaults.

## 3.14 Corruption fuzz boundary

Mutation tests:
- test copies only;
- deterministic mutation seed;
- bounded input count;
- no production saves touched.

Expected:
- safe rejection/fallback;
- no crash/hang.

## 3.15 Atomic-write proof

Test last-known-good behavior under injected failures.

Do not infer atomicity from a temp filename alone.

## 3.16 Timestamp boundary

UTC timestamp metadata may be nondeterministic and excluded from gameplay fingerprints/checksums according to current contract.

Do not use timestamps for simulation decisions.

## 3.17 High-stress save scenarios

Use canonical domain fixtures representing:
- severe weather;
- active expeditions;
- dense medical/events;
- production/economy changes.

Avoid requiring “active combat” if combat state is not saveable/currently not a campaign-save boundary.

## 3.18 Event replay duplication audit

After restore verify no duplicate:
- quest rewards;
- deadline/follow-up firings;
- medical transition events;
- memorial/grief events;
- voice/journal entries,
according to their existing exactly-once contracts.

## 3.19 B2 terminal acceptance

B2 reaches IMPLEMENTED when the plan-defined long-run soak is stable/deterministic, terminal restore equals continuous state, primary/backup validation and atomic writes are proven, corruption/migration batteries pass, recovery is explicit and safe, and no new parallel save store exists.


# 4. TASK B3 — C2[17] AUTHORED SURVIVOR IDENTITY, DECLARED ITEM TAGS & DIEGETIC PERSONALITY

**Source:** `C-integration-plans/C2_planintegration[17].md` — Plan 40A/40B/40C\
**Blocker class:** INFERRED STATS DEFECT / UNWIRED IDENTITY DATA\
**Canonical ownership:** current survivor identity/catalog owner + inventory/item tag authority\
**Primary acceptance:** gameplay systems read explicit authored survivor/item identity instead of heuristic inference or hardcoded IDs, with save compatibility and diegetic presentation.

## 4.1 Objective

Replace implicit identity inference with explicit authored data where Plan 40 defines it. The task must distinguish:
- authored immutable definition data;
- runtime survivor state;
- generated/randomized survivors;
- item taxonomy;
- presentation.

The core rule is **authored data is explicit; runtime state derives from or references it; panels do not infer identity**.

## 4.2 Exact 20 procedural substeps

1. Reverify the live survivor catalog architecture and actual survivor-definition count; do not hardcode “129” unless current data confirms it.
2. Locate all runtime `InferBeliefProfile`-style heuristics, profession inference, background inference, hardcoded item-ID behavior, keepsake/heirloom identity, and current survivor save references.
3. Claim current survivor identity/catalog, item-tag catalog, social/morale integration, identity UI, data, tests, and authority-matrix paths.
4. Define/load the authored belief-profile schema from the current Plan 40 data source, creating `survivor_beliefs.json` only if the plan/live architecture actually calls for that catalog.
5. Wire authored profession identity into the current fitness/duty suitability model without creating a second profession stat table.
6. Reconcile personal keepsakes/heirlooms against current memory/heirloom systems and connect only the missing Plan 40 behavioral consumers.
7. Replace hardcoded item-ID behavior checks with canonical declared tags where the tag expresses an actual semantic category and migration is behavior-preserving.
8. Migrate medical/nutrition/repair/production consumers to tags only where their current behavior is category-based; preserve truly item-specific exceptions explicitly.
9. Reconcile phantom/background traits with current exploration/expedition consequence rails and wire only plan-defined identity effects.
10. Replace ideological/social friction inference with authored belief/core-value data, preserving the current social/morale owner as the calculator.
11. Update survivor-detail presentation to render authored identity/personality diegetically without exposing raw hidden compatibility numbers unless current UX permits.
12. Validate item tags against the current item schema, naming convention, duplicate/unknown tag policy, and canonical tag registry.
13. Connect personal heirloom/keepsake effects through the current memory/morale modifier rails only where Plan 40 or already-sealed C1[12] contracts define them.
14. Preserve save compatibility by storing stable authored-definition IDs/references or existing generated identity state; do not duplicate immutable catalog fields into every save unless required.
15. Add focused Plan 40 survivor identity tests covering authored beliefs, professions, tags, keepsakes, and social-friction consumption.
16. Verify generated/random survivors use the existing deterministic identity-generation path and do not accidentally depend on catalog enumeration order.
17. Benchmark item-tag lookup only if Plan 40 specifies a performance requirement; use a cached/indexed design without claiming strict O(1) unless the implementation actually guarantees it.
18. Run data-integrity checks for survivor belief/profession/keepsake/tag references and record zero new invalid IDs.
19. Run focused survivor, social/morale, inventory, crafting, medical, and expedition tests touched by the migration.
20. Update the survivor-state authority matrix and hand off with identity schema, inference-removal map, tag-migration matrix, save compatibility, deterministic generation proof, UI/a11y evidence, and zero-warning build.

## 4.3 Mini-task B3.1 — Authored Belief Profile Plumb-Through

### B3.1.a
Load/resolve authored belief profiles through the current survivor catalog loader and bind them to stable survivor definition/runtime identity records.

### B3.1.b
Deprecate/remove heuristic belief inference only after every production consumer has been migrated and characterization tests pin legacy-vs-authored behavior where intended.

### B3.1.c
Update ideological/social friction calculation to consume authored belief/core-value data through the existing morale/social authority.

### B3.1.d
Add tests proving known authored belief combinations produce predictable plan-defined friction while identical profiles remain neutral according to the current model.

## 4.4 Mini-task B3.2 — Item Tag Authority Migration

### B3.2.a
Audit inventory, crafting, medical, nutrition, repair, fuel, expedition, and production code for hardcoded item-ID checks used as category proxies.

### B3.2.b
Replace eligible category checks with canonical `ItemTagCatalog`/current tag-query APIs while retaining explicitly item-specific behavior.

### B3.2.c
Ensure every migrated semantic tag is declared in the authoritative tag schema/catalog and applied to all intended base items.

### B3.2.d
Add contract tests showing a novel test/custom item with the same standard tag participates identically in category-based consumers.

## 4.5 Mini-task B3.3 — Keepsake & Heirloom Mechanics

### B3.3.a
Resolve personal keepsake definitions to stable survivor/background identities using the current heirloom/memory data owner.

### B3.3.b
Wire passive morale/psychological effects through existing modifier/memory rails using Plan 40/C1[12] values, not invented bonuses.

### B3.3.c
Wire loss/stolen/destroyed consequences only where the current item-loss/expedition/memory contract defines those events and effects.

### B3.3.d
Verify keepsake identity/effects survive save/load without duplicating immutable definition data or expanding payload unnecessarily.

## 4.6 Mini-task B3.4 — Diegetic Personality Presentation

### B3.4.a
Update the current survivor-detail read model/panel to resolve authored traits, background, profession, and belief descriptors through localization/content IDs.

### B3.4.b
Replace raw hidden compatibility numbers with narrative summaries only where current UX/Plan 40 specifies diegetic presentation.

### B3.4.c
Ensure identity text supports current text scaling, contrast, focus, and screen-reader/accessibility patterns.

### B3.4.d
Run UI accessibility/lifecycle/snapshot gates for the survivor identity surface.

## 4.7 Identity authority layers

### Definition data
Owns:
- authored belief profile;
- profession;
- background;
- authored keepsake relation;
- stable descriptive traits.

### Runtime survivor state
Owns:
- current health/needs;
- relationships;
- inventory/possession;
- generated identity result for procedural survivors if supported.

### Presentation
Reads both.
Does not infer identity.

## 4.8 Authored vs generated survivors

If the game has authored and generated survivors:
- authored definitions use explicit identity catalog values;
- generated survivors use a deterministic generator with canonical options;
- the same runtime identity shape should be consumable by social/duty/UI systems.

Do not force generated survivors into authored file IDs if the existing model distinguishes them.

## 4.9 Belief schema

Use Plan 40/live schema.

Possible dimensions such as Technological/Pragmatic/Zealous/Communal are examples until verified.

Store:
- stable IDs/enums;
- bounded values/category if current contract.

Do not encode display prose as authority.

## 4.10 Inference removal migration

Build:

| Heuristic/inference | Consumers | Authored replacement | Characterization | Removal point |
|---|---|---|---|---|
|  |  |  |  |  |

Remove only after all consumers migrate.

## 4.11 Profession suitability

Profession data informs current `FitnessForDutyModel` or equivalent.

Rules:
- profession is one input;
- current health/skill/needs remain authoritative;
- no profession-only bypass of eligibility.

## 4.12 Item tag authority

Tags are semantic categories.

Examples only after verification:
- medical sterile;
- raw food;
- precision tool;
- volatile fuel.

Tag IDs should follow current snake_case policy if that is the live schema.

## 4.13 Hardcoded ID migration classification

Each hardcoded ID check becomes one of:
- CATEGORY → migrate to tag;
- UNIQUE_ITEM_BEHAVIOR → keep explicit;
- LEGACY/DEAD → retire;
- AMBIGUOUS → decision/owner review.

Do not indiscriminately tag-migrate every equality check.

## 4.14 Tag lookup performance

Use:
- precomputed item-definition tag sets;
- indexed catalog;
- immutable/hash-set structures where current architecture supports.

Report measured complexity/performance, not marketing claims.

## 4.15 Custom/mod item parity

A custom/modded item carrying a standard public tag should work in category consumers if the mod contract permits.

This is a key proof that behavior is tag-driven.

## 4.16 Keepsake authority

Reuse the C1[12] memory/heirloom owner if already sealed.

B3 may connect identity data to that rail; it must not create a second keepsake effect system.

## 4.17 Keepsake loss

Loss event must come from canonical inventory/expedition destruction/theft path.

No polling inventory every day solely to detect loss if an event exists.

## 4.18 Social friction

Social/morale owner computes the effect.

Identity provides inputs only.

No UI-calculated compatibility.

## 4.19 Diegetic summaries

Narrative summary:
- derived from authored descriptors and current relationship state;
- localization-ready;
- not a hidden numeric leak unless design allows.

## 4.20 Save compatibility

Immutable authored identity should normally be referenced by stable survivor definition ID.

For generated survivors:
- persist generated identity fields according to current save owner.

Old saves:
- use migration/default rules;
- do not silently re-roll identity on every load.

## 4.21 Deterministic generation

For generated identity:
- seeded stream;
- stable option ordering;
- save/restore does not regenerate differently.

## 4.22 Data-integrity gates

Validate:
- belief IDs;
- profession IDs;
- keepsake refs;
- survivor refs;
- item tags;
- tag registry membership.

## 4.23 B3 terminal acceptance

B3 reaches IMPLEMENTED when authored identity replaces heuristic inference where Plan 40 requires, category behavior uses declared tags rather than ID proxies, keepsake identity routes through existing memory rails, generated identity remains deterministic/save-stable, and UI presents personality diegetically without becoming a simulation authority.

---

# 5. TASK B4 — C2[18] DETERMINISTIC SURVIVOR VOICE, DELIVERY CONTRACTS & SOCIAL SPEECH

**Source:** `C-integration-plans/C2_planintegration[18].md` — Plan 42A/42B/42C\
**Blocker class:** NARRATIVE COHESION / VOICE DELIVERY GAP\
**Canonical ownership:** current narrative/event authority + Host/Audio presentation bridge\
**Primary acceptance:** authored voice lines resolve deterministically from semantic events and survivor state, obey bounded attention policies, route to text/audio sinks safely, and never generate runtime dialogue dynamically.

## 5.1 Objective

Introduce/complete a deterministic **authored voice delivery contract**:
- semantic trigger;
- eligible speakers;
- authored line candidates;
- deterministic line/speaker resolution;
- attention/cooldown policy;
- multi-sink delivery;
- accessible caption fallback;
- audio cue routing;
- save/replay continuity where history/cooldowns affect future selection.

No conversational AI, dialogue tree engine, LLM generation, or runtime free-text synthesis.

## 5.2 Exact 20 procedural substeps

1. Reverify current survivor voice/narrative assets, radio/briefing/journal delivery, audio cue catalog, social barks, and any existing voice-selection code.
2. Confirm Plan 42's architectural guardrail: authored finite content only, no conversational AI/dynamic runtime text generation.
3. Claim current voice-delivery coordinator/equivalent, audio event bridge, authored voice data, tests, UI/audio integrations, and documentation paths.
4. Implement or adapt the voice-delivery coordinator as an engine-free deterministic resolver over typed semantic events and survivor/read-model state.
5. Create or extend the authoritative survivor-voice catalog only if a separate `survivor_voices.json` is the current Plan 42 contract; otherwise use the existing narrative catalog owner.
6. Bind voice trigger eligibility to Plan 31 SemanticKind/current canonical semantic events and explicit shelter/social state transitions.
7. Implement the plan-defined attention budget using **campaign/simulation time for Core-relevant cooldowns** and presentation-time suppression only in Host/UI where appropriate.
8. Filter speaker eligibility using canonical survivor state: living, conscious, present, and any Plan 42 restrictions such as quarantine/silence.
9. Route resolved lines to plan-defined sinks such as daily briefing quote, HUD caption/ticker, journal/radio/social log, keeping sink selection out of Core simulation where possible.
10. Route optional audio cues through the current AudioManager/audio bridge and current bus/priority/ducking authority.
11. Make missing/unavailable audio cues fall back to accessible text/caption delivery without runtime exception or dropped semantic line.
12. Use the current registered narrative RNG stream or a stable deterministic hash contract exactly as Plan 42 defines; do not mix both ad hoc.
13. Bind line topics to typed recent-history facts/events rather than parsing prose logs or UI strings.
14. Ensure line resolution/delivery is nonblocking and bounded; preloaded/catalog IDs are preferred over runtime disk scanning.
15. Unsubscribe/tear down Host/UI/audio delivery bindings when sessions/scenes change; Core resolver itself remains engine-free and lifecycle-neutral.
16. Add focused Plan 42 voice-delivery tests covering eligibility, deterministic speaker/line resolution, cooldowns, priority, and fallback.
17. Run repeated deterministic multi-day simulations and compare the **semantic voice-history sequence**; require byte-identical serialized history only if canonical serialization defines it.
18. Verify save/restore for any voice history/cooldown state that affects future selection using the existing journal/narrative save owner rather than a new parallel store.
19. Run the current audio selftest plus targeted ducking/concurrency/caption accessibility gates for changed delivery paths.
20. Update narrative voice guidelines with taxonomy, trigger contract, attention policy, deterministic selection, sink routing, caption fallback, and handoff evidence.

## 5.3 Mini-task B4.1 — Semantic Voice Line Catalog Authoring

### B4.1.a
Define/verify the voice-row schema using stable fields such as ID, semantic trigger, speaker/trait requirements, cooldown/priority metadata, localization text key, and optional cue ID according to Plan 42.

### B4.1.b
Author/activate representative voice content across the **current** SemanticKind taxonomy; do not hardcode “10 domains” if the live taxonomy differs.

### B4.1.c
Review only the touched voice content for ASHFALL's restrained survivor tone and localization structure.

### B4.1.d
Run catalog/integrity validation for duplicate IDs, invalid semantic kinds, invalid trait/belief requirements, and missing cue/text references.

## 5.4 Mini-task B4.2 — Deterministic Speaker Selection Engine

### B4.2.a
Filter candidate speakers using canonical survivor state and Plan 42 eligibility requirements.

### B4.2.b
Resolve speaker ordering/selection using the plan-defined narrative RNG stream or stable deterministic ranking based on canonical IDs/state.

### B4.2.c
Resolve the highest-priority eligible authored line using current environmental/history preconditions and attention state.

### B4.2.d
Add tests proving identical canonical state, seed, history, and event yield the same speaker and line.

## 5.5 Mini-task B4.3 — Attention Budget & Fatigue Prevention

### B4.3.a
Implement the **plan-defined** global/per-survivor attention limits; treat “60 seconds / one per day” as draft examples unless Plan 42 explicitly specifies them.

### B4.3.b
Allow critical semantic categories to pre-empt lower-priority atmospheric lines according to the current audio/narrative priority policy.

### B4.3.c
Suppress or defer presentation delivery during combat/modals only through Host/UI state where Plan 42 requires; do not make Core selection depend on real-time scene timing.

### B4.3.d
Add high-frequency trigger tests proving emitted/delivered lines remain within the configured deterministic attention contract.

## 5.6 Mini-task B4.4 — Multi-Sink Presentation Routing

### B4.4.a
Route resolved barks to the current HUD caption/ambient text surface with presentation-owned fade timing and no domain-state mutation.

### B4.4.b
Route significant reflections to the existing daily briefing/journal pipeline through typed line/event data rather than direct builder string injection where possible.

### B4.4.c
Route optional audio cue IDs through the current AudioManager/audio event bridge using established bus, concurrency, and ducking rules.

### B4.4.d
Run UI accessibility/caption and audio selftests proving every audible line has accessible text and missing audio degrades to text-only.

## 5.7 Voice authority split

### Core/narrative resolver owns
- semantic trigger interpretation;
- candidate line IDs;
- candidate speaker IDs;
- deterministic selection;
- simulation-relevant cooldown/attention state if Plan 42 makes it persistent.

### Host/UI/audio owns
- active input/device;
- modal/combat presentation suppression if purely presentational;
- caption rendering;
- fade timers;
- actual audio playback/bus.

No Godot/AudioManager types in Core.

## 5.8 Voice catalog authority

If current narrative catalogs already support survivor barks:
- extend them.

Only create a dedicated `survivor_voices.json` if Plan 42/current architecture specifies one.

No duplicate voice catalog merely because the draft names a file.

## 5.9 SemanticKind integration

Voice rows reference stable semantic IDs/kinds.

No parsing daily briefing prose to infer a topic.

## 5.10 Speaker eligibility

At minimum, according to live plan:
- alive;
- conscious;
- in relevant location/context.

Quarantine does **not automatically imply silence** unless Plan 42 says quarantined survivors cannot be delivery candidates. Reverify this draft assumption.

## 5.11 Identity integration

If Plan 42 uses traits/beliefs/profession:
- consume B3's canonical authored identity.

Do not duplicate trait inference.

## 5.12 Selection algorithm

One authoritative strategy.

Either:
- seeded RNG stream with stable candidate order;
- deterministic hash/rank.

Do not combine a hash rank and RNG in a way that makes replay reasoning opaque unless the plan explicitly defines it.

## 5.13 Narrative RNG stream

If using `CampaignRngStream.Narrative` or current equivalent:
- stable fork/stream ID;
- selection order documented;
- no UI reads consume stream.

## 5.14 Recent-history context

Use typed records/events:
- rationing conflict;
- death;
- severe weather;
- medical crisis;
- morale state,
only if these are current semantic events/records.

No free-text log parsing.

## 5.15 Attention-budget time model

This is critical.

### Simulation-relevant cooldown
Use campaign day/tick/state.

### Presentation-only cooldown
May use engine monotonic time, but must not alter which domain events occurred or campaign replay fingerprint.

If the cooldown determines future selected line history and that history is part of deterministic replay, use simulation time.

## 5.16 Critical priority

Priority escalation uses existing semantic/audio priority.

Critical lines may:
- pre-empt/suppress casual output;
- respect audio concurrency/ducking.

No new independent priority system if RADIO_ALERT_PRIORITY/audio coordination already exists.

## 5.17 Combat/modal suppression

If combat/modal is a presentation condition:
- Host may suppress playback/caption and optionally queue/drop according to Plan 42.

Core narrative history should not become nondeterministic due frame timing.

## 5.18 Multi-sink contract

A resolved line may have one/more sinks per policy:
- HUD caption;
- briefing;
- journal;
- radio/social log;
- audio.

Avoid duplicate “same line” history entries when delivered to multiple sinks.

## 5.19 Audio fallback

Missing cue:
- text still delivered;
- one log-once diagnostic according to current audio acquisition policy;
- no exception;
- no repeated spam.

## 5.20 Caption accessibility

Every audible bark:
- accessible text;
- high contrast/current caption style;
- text scaling;
- no audio-only critical information.

## 5.21 Voice history persistence

Persist only if history/cooldown affects future behavior or if Plan 42 requires an archive.

Prefer existing journal/narrative save owner.

No new top-level voice save store.

## 5.22 Save/restore

If cooldown/history affects selection:
- save;
- restore fresh session;
- same future eligible/selected sequence.

No repeated bark caused by restore.

## 5.23 Performance

Resolve from in-memory catalogs/indexes.

No synchronous disk load for each bark.

Do not assert “no frame drops” without measurement; verify bounded/nonblocking execution.

## 5.24 B4 terminal acceptance

B4 reaches IMPLEMENTED when authored semantic voice content resolves deterministically, eligible speakers come from canonical survivor state/identity, attention rules are deterministic where simulation-relevant, sinks/audio/captions use current Host/audio authorities, missing audio falls back safely, restore preserves behavior when necessary, audio/a11y gates pass, and no runtime-generated dialogue system exists.

---

# 6. CROSS-TASK VERIFICATION MATRIX

| Task | Primary domain | Focused verification | Build | Data integrity | Determinism / continuity | UI/audio |
|---|---|---|---|---|---|---|
| B1 | input/focus | action map, focus traversal, mouse-free journey | required | unchanged | semantic command parity | a11y/focus/rebinding |
| B2 | save durability | soak, corruption, migration, atomic recovery | required | save-coupled refs | long-run + restore equality | recovery notification |
| B3 | identity/tags | authored beliefs, tag consumers, keepsakes | required | required | generated identity stability | survivor detail a11y |
| B4 | voice delivery | eligibility, selection, attention, routing | required | voice refs required | multi-day voice sequence | audio + captions |

## 6.1 Count/version rule

Do not hardcode:
- “333 catalogs”;
- “28 surfaces”;
- “129 survivors”;
- “10 SemanticKind domains”;
- fixed test counts.

Measure current truth and report exact observed values.

---

# 7. CROSS-TASK SAVE / USER-PERSISTENCE CONTRACT

## 7.1 B1
User input/rebinding preferences persist in user/profile settings, not campaign save.

## 7.2 B2
Extends existing campaign save/backup/migration ownership only.

## 7.3 B3
Immutable authored survivor/item identity remains in catalogs. Generated survivor identity persists under current survivor save owner when necessary.

## 7.4 B4
Voice history/cooldown persists only if future deterministic behavior requires it, under existing narrative/journal owner.

No new parallel store.

---

# 8. CROSS-TASK DETERMINISM CONTRACT

## B1
Raw input timing can vary; committed semantic action sequence must result in the same domain state.

## B2
Same seed + same scenario + same save/reload schedule produces the same authoritative terminal state.

## B3
Authored identity is stable; generated identity uses registered deterministic generation and stable option ordering.

## B4
Same semantic event/history/survivor state/seed produces the same speaker/line sequence where Plan 42 requires deterministic voice history.

---

# 9. CLAIM AND MERGE DISCIPLINE

## 9.1 B1 claims
- input constants/map;
- central input dispatcher;
- focus helpers/controllers;
- rebinding/user settings;
- glyph/help overlay;
- UI tests/docs.

## 9.2 B2 claims
- save orchestrator;
- filesystem/save writer abstraction;
- backup/recovery loader;
- migration/corruption tests;
- save contract docs.

## 9.3 B3 claims
- survivor identity/catalog;
- belief/profession/keepsake data;
- item tag catalog;
- social/morale/tag consumers;
- survivor detail read model/UI;
- tests/docs.

## 9.4 B4 claims
- voice catalog/source;
- narrative voice resolver;
- host/audio bridge;
- caption/briefing routing;
- tests/guidelines.

## 9.5 Shared files
If B3/B4 both touch:
- survivor identity types;
- narrative registries;
- docs index,
serialize changes and regenerate after final merged source.

---

# 10. IMPLEMENTATION LOG STANDARD

Every B task log includes:

1. plan identity;
2. current census status;
3. Part 1.1 dependency evidence;
4. claims;
5. historical premise;
6. current premise;
7. authority map;
8. exact 20-step completion matrix;
9. mini-task completion matrix;
10. sealed/retired/resolved items;
11. files/data changed;
12. save/user-store impact;
13. determinism/time/RNG model;
14. focused commands/results;
15. broader gates;
16. docs/generated outputs;
17. terminal state;
18. remaining blocker;
19. census/ledger update;
20. claim handoff.

---

# 11. ROLLBACK / ROUTING MATRIX

| Task | Finding | Action |
|---|---|---|
| B1 | action exists but no product semantics | retire/reserve/decision |
| B1 | panel raw key handling is legitimate text/control input | leave local |
| B1 | deadzone threshold unspecified | use current engine/project policy; no invention |
| B1 | text scaling breaks fixed layout fundamentally | UI repair package if broader |
| B2 | backup itself invalid | explicit failure, never silently load |
| B2 | missing save section is required | corruption/migration failure, not fake default |
| B2 | real 200h soak required | schedule appropriate long-tier process; do not mislabel accelerated test |
| B2 | existing valid save endangered by migration | stop/revert |
| B3 | belief schema absent/unsigned | decision/data package |
| B3 | hardcoded ID is truly unique item behavior | keep explicit |
| B3 | keepsake values absent | do not invent bonuses |
| B4 | separate voice catalog would duplicate existing narrative catalog | extend current catalog |
| B4 | cooldown uses real time but affects deterministic history | move to simulation-time contract |
| B4 | quarantine/silence assumption not in plan | do not enforce |
| B4 | missing audio cue | text fallback + diagnostic |

---

# 12. WAVE 12 PART 1 MASTER ACCEPTANCE MATRIX

| Task | Core blocker | Terminal proof |
|---|---|---|
| B1 | controller/focus parity gap | reconciled actions + stable focus + mouse-free day + rebindings |
| B2 | session/save durability | long-run deterministic soak + atomic recovery + migration/corruption proof |
| B3 | inferred identity/hardcoded tag behavior | authored identity + declared tags + save/deterministic identity + diegetic UI |
| B4 | incoherent survivor voice delivery | authored semantic lines + deterministic selection + attention/sinks/audio/captions |

Combined with Part 1.1 A1–A4, Wave 12 Part 1 has eight tasks.

---

# 13. WAVE 12 PART 1 CLOSEOUT

Wave 12 Part 1 closes only when all eight A/B tasks are in allowed terminal states and the final merged `HEAD` satisfies:

1. Godot-only runtime boundary.
2. Core engine-free gate.
3. canonical Data/schema integrity.
4. deterministic simulation/replay.
5. additive save compatibility.
6. no duplicate authorities.
7. focused task tests green.
8. required UI lifecycle/a11y gates green.
9. required audio gate green.
10. current catalog count/data-integrity result reported, not hardcoded.
11. build errors/warnings meet current zero-baseline contract.
12. docs/indexes regenerated.
13. implementation logs complete.
14. census/ledger current.
15. claims handed off.
16. Wave 12 Part 2 queue derived from the current DAG, not presumed numeric order.

---

# 14. WAVE-LEVEL STOP POLICY

Stop work and route when:

1. **Missing owner:** behavior has no canonical authority and implementing it would create a duplicate manager.
2. **Unsigned choice:** a balance, policy, save-recovery, identity, or voice delivery decision lacks signed semantics.
3. **Claim collision:** required path actively claimed.
4. **Premise invalidation:** current source already solved the issue or architecture materially differs.
5. **Determinism break:** same-seed/reload runs diverge.
6. **Engine infiltration:** Core gains engine dependency.
7. **Recovery risk:** save changes could destroy valid player data.
8. **Dynamic-dialogue drift:** B4 work trends toward runtime-generated text/conversational AI.

---

# 15. WAVE 12 PART 1 NON-GOALS

- no new engine/runtime;
- no second input manager;
- no second save store;
- no inferred identity replacement with another hidden heuristic;
- no blanket conversion of all item-specific behavior to tags;
- no conversational AI/dialogue tree system;
- no wall-clock-dependent Core voice selection;
- no claiming accelerated simulations are literal 200-hour tests;
- no arbitrary fixed counts/thresholds from draft examples when current repository differs;
- no full-suite churn as a substitute for focused verification.


# APPENDIX A — B1 INPUT / FOCUS EXECUTION PACKET

## A.1 Input-action reconciliation table

| Action ID | Declared in project | Constant/source symbol | Primary handler | Default keyboard | Default controller | Rebindable? | Status |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

Statuses:
- LIVE;
- RESERVED;
- DUPLICATE;
- ORPHANED;
- RETIRED;
- DECISION-BLOCKED.

No action may remain unclassified.

## A.2 Raw-input exception policy

Not every raw input check is automatically wrong.

Legitimate local raw/control handling may include:
- text entry;
- slider drag;
- scroll behavior;
- Godot-native control navigation;
- pointer hover.

Gameplay commands should route through semantic actions.

Document each retained exception.

## A.3 Global vs local action boundary

Central dispatcher owns:
- pause/back;
- global panel shortcuts;
- semantic management commands;
- route-level navigation.

Panel/control owns:
- local focus;
- selection within its control contract;
- text editing.

Avoid a giant `_UnhandledInput` switch that reimplements Godot Controls.

## A.4 Focus graph discovery

For each surface:
- collect visible enabled focusable controls;
- inspect current neighbors;
- find dead ends;
- find cycles;
- find background escapes;
- find missing default focus.

Output graph/route table.

## A.5 Focus traversal oracle

A focus graph is valid when:
- every visible enabled control is reachable from default;
- directional moves are deterministic;
- traversal never lands on hidden/disabled nodes;
- modal graph is closed;
- cancel/back exits as expected.

## A.6 Container helper constraints

An automatic helper may fill ordinary list/grid neighbors.

It must not overwrite:
- bespoke spatial relationships;
- hand-authored modal boundaries;
- deliberate tab order.

Run helper only within a declared container subtree.

## A.7 Default focus

On panel open:
- choose current intended primary action/first item;
- if absent/disabled, deterministic fallback;
- no focus on invisible placeholder.

## A.8 Modal focus ownership

While modal active:
- background controls remain inaccessible to focus;
- global shortcuts that should be suppressed are suppressed;
- back/cancel closes modal before parent panel.

## A.9 Focus return

After modal/detail close:
- restore origin control if alive/visible;
- otherwise route default.

Do not persist focus into campaign save.

## A.10 Controller map audit

For each required action:
- D-pad;
- stick;
- face buttons;
- shoulder/trigger where appropriate.

Do not create conflicting defaults.

## A.11 Deadzone policy

Record current Godot project deadzones.

If Plan 37 specifies a value:
- set/verify.

If not:
- keep current project policy or route accessibility decision.

## A.12 Repeat suppression

Held navigation axis should:
- move focus at a controlled repeat cadence;
- never fire an activation command repeatedly.

Activation buttons should use edge/press semantics.

## A.13 Device detection

Device switch signal:
- keyboard/mouse;
- controller.

It changes glyph/help presentation.

It does not rewrite bindings automatically.

## A.14 Glyph mapping

Glyph lookup key:
- semantic action;
- active device;
- current binding.

Do not hardcode “Press X” strings in panel copy.

## A.15 Rebinding schema

Record:
- user settings owner;
- action ID;
- binding representation;
- version;
- conflict policy;
- reset policy.

## A.16 Rebinding conflict cases

Test:
- bind two actions to same key if allowed/disallowed;
- unbind required action;
- invalid device code;
- removed action from older settings;
- reset defaults.

## A.17 Text scaling

For each supported scale:
- roster;
- inventory;
- trade;
- medical;
- briefing;
- Holdfast;
- map.

Check:
- clipping;
- overflow;
- scroll;
- focus visibility;
- button min size.

## A.18 Mouse-free journey definition

The representative journey should prove:
1. navigate HUD;
2. open roster;
3. assign worker;
4. open medical;
5. perform valid medical action;
6. inspect briefing;
7. navigate Holdfast/map;
8. advance day;
9. return to HUD.

Use exact Plan 37 journey if it differs.

## A.19 Mouse-free journey evidence

Assert:
- domain command results;
- final state;
- no mouse event injection;
- no focus trap;
- no background modal activation;
- no duplicate commands.

## A.20 B1 merge-readiness checklist

- [ ] action matrix complete;
- [ ] no unhandled required actions;
- [ ] raw-input exceptions documented;
- [ ] focus graphs valid;
- [ ] modal isolation green;
- [ ] controller defaults non-conflicting;
- [ ] deadzone/repeat policy sourced;
- [ ] text scaling green;
- [ ] glyph prompts dynamic;
- [ ] rebinding persists/restores;
- [ ] mouse-free journey green;
- [ ] UI a11y/lifecycle green;
- [ ] zero warnings;
- [ ] implementation log/census updated.

---

# APPENDIX B — B2 SESSION DURABILITY EXECUTION PACKET

## B.1 Save-store architecture matrix

| Store/section | Owner | Version | Checksum | Atomic write | Backup | Migration | Critical? |
|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |

This matrix is the source for the durability test plan.

## B.2 Soak definition packet

**Plan 39 target:**\
**Simulation duration:**\
**Real wall-clock requirement:**\
**Seed(s):**\
**Scenario mix:**\
**Save checkpoints:**\
**Resource metrics:**\
**Acceptance:**\

Never leave “200 hours” ambiguous.

## B.3 Deterministic scenario composition

A representative long-run soak should exercise:
- needs;
- weather;
- production;
- expeditions;
- economy/trade where stable;
- medical;
- quests/events;
- social state;
- save checkpoints.

Do not add systems solely for test complexity.

## B.4 Continuous vs interrupted runs

Run A:
- continuous.

Run B:
- save at selected checkpoints;
- destroy session;
- restore fresh composition;
- continue.

Final authoritative fingerprints must match where the save contract guarantees full determinism.

## B.5 Multi-seed strategy

Primary regression:
- fixed seed.

Optional additional seeds:
- broaden state space;
- each compared only against its own repeat.

Do not compare different seeds.

## B.6 Memory growth evidence

Measure using current supported instrumentation.

Classify:
- startup/cache warmup growth;
- bounded steady-state;
- monotonic suspicious growth.

Use repeated checkpoints.

## B.7 Node/resource leak evidence

Godot host test may measure:
- node counts;
- subscriptions;
- resource handles if exposed.

Core xUnit should not pretend to measure Godot nodes.

## B.8 Backup write order

Document exact sequence.

A safe model often resembles:
1. serialize staged content;
2. validate/canonicalize if applicable;
3. write temp;
4. flush;
5. preserve last-known-good backup;
6. atomic replace;
7. cleanup.

Actual current contract wins.

## B.9 Invalid primary + valid backup

Expected:
- primary rejected;
- backup validated;
- backup loaded;
- typed recovery result;
- player notified;
- original corrupt primary preserved/quarantined according to policy.

## B.10 Invalid primary + invalid backup

Expected:
- no fabricated recovery;
- typed fatal/recovery-required result;
- no crash/hang;
- no overwrite of evidence.

## B.11 Truncated primary cases

Test:
- zero bytes;
- half JSON/envelope;
- valid syntax but missing tail/checksum;
- random mutation.

## B.12 Missing section classification

For every save section:
- optional/defaultable;
- legacy-migratable;
- required.

Only the first two may be repaired/defaulted.

## B.13 Migration matrix

| From version | To version | Migration | Data preserved | Characterization |
|---|---|---|---|---|
|  |  |  |  |  |

Unsupported future version fails clearly.

## B.14 Atomic failure injection

Inject at:
- before temp write;
- during temp write;
- after temp complete;
- before replace;
- after backup;
- during cleanup.

Assert last known-good state according to contract.

## B.15 Temp file recovery

On boot:
- stale temp ignored/cleaned;
- never preferred over validated primary/backup unless recovery contract explicitly says.

## B.16 Culture test

Run serialization under:
- `en-US`/dot decimal;
- `lv-LV` or another comma-decimal culture.

Canonical save semantics remain equal.

## B.17 Canonical bytes caveat

Byte identity across cultures is only required when the checksum/canonical serializer uses a canonical byte representation.

Otherwise require semantic equality plus valid checksums.

## B.18 Time metadata

Save metadata timestamp:
- UTC/ISO if current contract;
- excluded from deterministic state comparison if nonsemantic.

Campaign day/time remains simulation state.

## B.19 High-state-density checkpoint

Choose deterministic scenario with many active sections:
- medical state;
- expedition;
- severe weather;
- production;
- scheduled events.

Save and restore.

## B.20 Exactly-once audit after restore

Verify no duplicates:
- quest reward;
- deadline;
- follow-up;
- memorial;
- medical transitions;
- narrative/voice if persisted.

## B.21 Recovery UX

User-visible recovery message should:
- say backup was used;
- identify slot/run without exposing technical junk;
- advise next action only if current UX defines.

No domain mutation from notification.

## B.22 B2 merge-readiness checklist

- [ ] soak target truthfully defined;
- [ ] deterministic long-run run green;
- [ ] continuous vs restored parity green;
- [ ] resource growth bounded per policy;
- [ ] valid backup recovery green;
- [ ] invalid backup rejection green;
- [ ] corruption fuzz safe;
- [ ] atomic failure tests green;
- [ ] migration matrix green;
- [ ] culture tests green;
- [ ] exactly-once restore audit green;
- [ ] save docs updated;
- [ ] zero warnings;
- [ ] implementation log/census updated.

---

# APPENDIX C — B3 AUTHORED IDENTITY EXECUTION PACKET

## C.1 Survivor identity census

| Definition type | Current source | Authored? | Generated? | Saved? | Consumers |
|---|---|---|---|---|---|
| biography |  |  |  |  |  |
| background |  |  |  |  |  |
| traits |  |  |  |  |  |
| beliefs |  |  |  |  |  |
| profession |  |  |  |  |  |
| keepsake |  |  |  |  |  |

## C.2 Heuristic inventory

Find:
- `InferBeliefProfile`;
- trait-to-belief heuristics;
- profession inference;
- string-prefix/category checks;
- background inference.

For each:
`Heuristic | Caller | Intended replacement | Remove? | Test`.

## C.3 Authored belief schema review

Record:
- catalog file;
- ID shape;
- dimensions/categories;
- localization/display descriptors;
- validation;
- current consumers.

No new dimensions solely because the draft listed examples.

## C.4 Authored definition linkage

Stable survivor definition should reference:
- belief profile ID;
- profession ID;
- background ID;
- keepsake ID,
according to live schema.

Avoid copy-paste duplicate blobs if normalized references already exist.

## C.5 Runtime identity shape

A runtime survivor exposes the identity values needed by:
- social;
- duty;
- expedition;
- UI;
- voice.

Whether values are references or materialized fields follows current architecture.

## C.6 Generated survivor identity

If procedural survivors exist:
- generation uses registered RNG;
- options sorted/stable;
- chosen identity saved;
- reload does not reroll.

## C.7 Belief friction migration

Characterize old heuristic outcomes for representative survivors.

After migration:
- authored data defines expected outcome;
- intentional differences documented;
- social owner remains calculator.

## C.8 Profession fitness integration

Profession provides a named suitability contribution.

It does not replace:
- skills;
- health;
- fatigue;
- roster restrictions.

## C.9 Item-tag registry

| Tag | Semantic meaning | Declared? | Base items | Public/mod-stable? |
|---|---|---|---|---|
|  |  |  |  |  |

No free-form unknown tags if schema forbids.

## C.10 Hardcoded item-ID audit

| File/system | Item ID check | Category or unique? | Replacement | Test |
|---|---|---|---|---|
|  |  |  |  |  |

## C.11 Category migration rule

Migrate only if behavior is intended to apply to every item in a semantic category.

Example:
- “any sterile medical item” → tag;
- “the unique quest artifact” → explicit ID.

## C.12 Novel custom item contract test

Create test-only item definition carrying a standard tag.

Assert:
- medical/crafting/repair/nutrition consumer accepts it as appropriate;
- no base-item name special case required.

## C.13 Tag lookup design

Prefer immutable indexed sets/maps.

Measure hot-loop lookup if Plan 40 has a performance acceptance.

Do not assert theoretical O(1) as a test unless implementation supports it.

## C.14 Keepsake mapping

Use stable survivor/background ID.

Do not use display name.

## C.15 Keepsake possession

Possession query uses inventory owner.

Effect uses memory/morale modifier rail.

No daily scanning if inventory events/read model support event-driven recompute.

## C.16 Keepsake loss

Loss causes Plan-defined reaction exactly once.

Different loss causes may be:
- destroyed;
- stolen;
- expedition loss.

Use existing events.

## C.17 Keepsake save behavior

If item ownership already saved:
- effect derives after restore.

Do not duplicate a “has keepsake” bool unless current save model requires.

## C.18 Diegetic personality read model

Expose:
- authored trait labels;
- background vignette;
- profession descriptor;
- relationship/friction summary.

Do not expose hidden raw numbers unless UX says.

## C.19 Accessibility

Identity text:
- scalable;
- readable;
- localized;
- focusable/scrollable where long;
- screen-reader compatible under current framework.

## C.20 B3 merge-readiness checklist

- [ ] live survivor count measured;
- [ ] heuristics inventoried;
- [ ] belief schema authoritative;
- [ ] production consumers migrated;
- [ ] heuristic removed only after migration;
- [ ] professions wired through fitness owner;
- [ ] tag registry validated;
- [ ] category-vs-unique checks classified;
- [ ] custom-item parity test green;
- [ ] keepsakes use existing memory rail;
- [ ] save compatibility green;
- [ ] generated identity deterministic;
- [ ] UI/a11y green;
- [ ] integrity green;
- [ ] zero warnings;
- [ ] implementation log/census updated.

---

# APPENDIX D — B4 VOICE DELIVERY EXECUTION PACKET

## D.1 Current voice/narrative census

| Content family | Current source | Trigger | Speaker logic | Sink | Audio cue? | Persistence |
|---|---|---|---|---|---|---|
| briefing quote |  |  |  |  |  |  |
| survivor bark |  |  |  |  |  |  |
| radio |  |  |  |  |  |  |
| inspection line |  |  |  |  |  |  |

This prevents a duplicate catalog/dispatcher.

## D.2 Voice-row schema review

Possible fields:
- stable line ID;
- semantic kind/event ID;
- speaker requirements;
- identity/trait requirements;
- priority;
- cooldown;
- localization text key;
- optional cue ID;
- environment/history conditions.

Live Plan 42 schema wins.

## D.3 Text storage

Prefer localization/content key rather than raw display string if current narrative pipeline uses localization.

Do not duplicate voice text in code.

## D.4 Semantic trigger binding

Every line family must map to:
- registered SemanticKind;
- exact event ID;
- typed recent-history condition.

No prose parsing.

## D.5 Speaker eligibility matrix

| Condition | Required? | Source owner |
|---|---|---|
| alive |  | survivor |
| conscious |  | health |
| at shelter |  | location/expedition |
| quarantined | plan-specific | medical |
| identity traits | line-specific | B3 identity |
| relationship/morale | line-specific | current owners |

Do not assume quarantined = silent.

## D.6 Candidate ordering

Before seeded/random selection:
- stable sort by survivor ID / line ID as required.

Never feed unordered hash-set iteration into RNG.

## D.7 Selection method

Choose one:
- registered narrative RNG;
- deterministic rank/hash.

Document exact choice.

If current plan names `CampaignRngStream.Narrative`, use it.

## D.8 RNG consumption contract

Specify when the narrative stream advances:
- event arrival;
- only when eligible candidates exist;
- after attention suppression or before,
according to plan.

This matters for replay.

## D.9 Attention state

Separate:
- simulation-relevant cooldown history;
- presentation-only throttling.

Only simulation-relevant state belongs in Core/save.

## D.10 Real-time suppression

HUD fade or “do not overlap two spoken clips” can use presentation monotonic time.

It must not affect which campaign-semantic events existed.

## D.11 Critical priority

Reuse existing audio/radio priority/ducking policy.

Do not create independent “voice severity” if the current semantic/audio priority can represent it.

## D.12 Recent history

Use typed history:
- last death;
- rationing event;
- storm arrival;
- outage;
- medical crisis.

No string contains/search.

## D.13 Multi-sink deduplication

One resolved line can be presented in multiple sinks, but narrative history should record one semantic line occurrence unless Plan 42 defines sink-specific history.

## D.14 Briefing route

Significant reflection should enter briefing through current report-builder input contract.

Do not direct-inject formatted prose into unrelated UI fields.

## D.15 HUD caption route

Presentation owns:
- position;
- fade;
- animation.

Core supplies:
- line ID/text key;
- speaker ID;
- semantic metadata.

## D.16 Audio cue route

Host/audio bridge:
- resolves cue ID;
- invokes current AudioManager;
- applies bus/priority/ducking;
- catches missing asset according to acquisition policy.

## D.17 Missing cue fallback

Expected:
- text/caption still shown;
- optional log-once diagnostic;
- no exception;
- no retried spam every frame.

## D.18 Caption accessibility

Every audible semantic line has:
- visible caption;
- readable text;
- current scale;
- sufficient contrast;
- speaker identification if UX requires.

## D.19 Save/history

If cooldown/last-line history determines future selection:
- persist under current narrative/journal save owner;
- old save neutral;
- restore does not repeat already-delivered line.

## D.20 Multi-day replay

Run fixed seed/state for plan-defined duration.

Compare semantic sequence:
`day/tick | event | speaker | line ID`.

Serialized byte equality only if canonical serialization is part of the contract.

## D.21 Audio concurrency test

Trigger:
- casual bark;
- high-priority alert;
- radio line;
- modal/combat suppression if supported.

Verify current ducking/concurrency policy.

## D.22 Nonblocking behavior

Voice resolver should use in-memory indexes.

No synchronous file load per event.

Measure if performance acceptance exists.

## D.23 B4 merge-readiness checklist

- [ ] current voice sources inventoried;
- [ ] no duplicate catalog/dispatcher;
- [ ] finite authored content only;
- [ ] semantic triggers typed;
- [ ] eligibility canonical;
- [ ] identity consumes B3 if needed;
- [ ] candidate order stable;
- [ ] deterministic selection documented;
- [ ] attention time model separated;
- [ ] multi-sink dedup correct;
- [ ] audio fallback safe;
- [ ] captions accessible;
- [ ] save/replay stable if stateful;
- [ ] audio selftest green;
- [ ] zero warnings;
- [ ] implementation log/census updated.

---

# APPENDIX E — CROSS-TASK FAILURE-INJECTION REGISTRY

## E.1 B1
- action declared but missing constant;
- constant missing project action;
- duplicate action string;
- hidden control in focus graph;
- disabled origin on focus restore;
- modal background activation;
- stick drift;
- held activation repeat;
- conflicting rebind;
- removed legacy action in settings.

## E.2 B2
- zero-byte primary;
- truncated primary;
- invalid checksum;
- invalid JSON/envelope;
- future version;
- valid backup;
- invalid backup;
- failure before atomic replace;
- failure after backup;
- stale temp file;
- comma-decimal culture;
- duplicate event after restore.

## E.3 B3
- authored survivor missing belief;
- invalid profession ref;
- item unknown tag;
- category item with hardcoded ID path;
- custom tagged item;
- keepsake transfer/loss;
- generated survivor restore;
- old save missing new identity field.

## E.4 B4
- no eligible speaker;
- one dead/unconscious candidate;
- quarantined candidate when policy allows/disallows;
- missing text key;
- missing cue;
- attention budget exhausted;
- critical priority pre-emption;
- repeated same event;
- save/restore cooldown;
- UI scene teardown/reopen.

---

# APPENDIX F — WAVE 12 PART 1 TASK HANDOFF TEMPLATE

## TASK `<ID>` HANDOFF — `<TITLE>`

### 1. Workspace & Authority
- Commit / HEAD:
- Claimed paths:
- Concurrent claims checked:
- Canonical owner(s):

### 2. Premise Re-verification
- Historical blocker:
- Current state:
- Stale assumptions corrected:
- Terminal-state target:

### 3. Decisions
- Signed decisions consumed:
- Deferred decisions:
- Balance/policy findings:

### 4. Implementation
- Files created:
- Files modified:
- Files retired:
- Data/schema changes:
- Generated artifacts:

### 5. Persistence / Determinism
- Save/user settings changes:
- Old-save/default behavior:
- RNG/time model:
- Replay/restore proof:

### 6. Verification
- Focused command(s):
- Exact results:
- Selftests:
- Build:
- Warnings:
- UI/audio gates:

### 7. Governance
- Census status:
- Integration ledger:
- Claim handoff:
- Next dependency released:
- Remaining blocker:

---

# APPENDIX G — WAVE 12 PART 1 FINAL AUDIT

## G.1 A-series evidence consumed

Before closing the full Wave 12 Part 1:
- A1 content rails handoff exists;
- A2 input/focus surface handoff exists;
- A3 tooling gates handoff exists;
- A4 save/medical state handoff exists.

B tasks must reference final merged outputs.

## G.2 B1 audit

- [ ] action matrix complete;
- [ ] focus traversal deterministic;
- [ ] modal isolation proven;
- [ ] mouse-free journey green;
- [ ] rebindings persist;
- [ ] current surface count reported;
- [ ] accessibility green.

## G.3 B2 audit

- [ ] soak definition truthful;
- [ ] long-run deterministic;
- [ ] resource metrics bounded;
- [ ] terminal fresh restore matches;
- [ ] primary/backup recovery safe;
- [ ] atomic failures safe;
- [ ] corruption/migration tests green;
- [ ] locale tests green.

## G.4 B3 audit

- [ ] authored identity canonical;
- [ ] heuristic migration complete;
- [ ] professions integrated;
- [ ] category tags canonical;
- [ ] item-specific exceptions retained;
- [ ] keepsakes use memory owner;
- [ ] generated identity deterministic;
- [ ] UI diegetic/a11y green.

## G.5 B4 audit

- [ ] authored finite lines;
- [ ] typed semantic triggers;
- [ ] canonical survivor eligibility;
- [ ] deterministic selection;
- [ ] time/cooldown model correct;
- [ ] multi-sink routing;
- [ ] missing audio fallback;
- [ ] accessible captions;
- [ ] audio selftest;
- [ ] save/replay if stateful.

---

# APPENDIX H — WAVE 12 PART 2 PROMOTION FILTER

Wave 12 Part 2 must be generated from the **post-Part-1 merged census**, not from historic numbering.

For each candidate:
- current corpus key;
- current premise;
- dependencies;
- active claims;
- decision state;
- duplicate status;
- exact remainder;
- acceptance;
- downstream unlock.

Template:

| Rank | Corpus | Title | Status | Remainder | Dependencies | Claims | Decision | Acceptance |
|---:|---|---|---|---|---|---|---|---|
|  |  |  |  |  |  |  |  |  |

No automatic “next four numbers” scheduling.

---

# APPENDIX I — FINAL REVIEWER QUESTIONS

## I.1 B1
- Can a user complete the representative day without mouse?
- Are domain outcomes equivalent, not just UI clicks?
- Can focus escape a modal?
- Are rebindings actually restored after restart?
- Do glyphs reflect the active mapping?
- Did we centralize semantic input without breaking native controls?

## I.2 B2
- Is the soak duration described honestly?
- Does a backup get validated before load?
- Can an invalid backup overwrite anything?
- Does atomic failure preserve the last known-good save?
- Are missing sections repaired only when schema permits?
- Does restore duplicate one-shot events?
- Are timestamps metadata-only?

## I.3 B3
- Are beliefs truly authored rather than re-inferred elsewhere?
- Did we distinguish category behavior from unique-item behavior?
- Are generated survivors stable after save/load?
- Are keepsake effects reusing C1[12] rails?
- Is personality presentation derived, not authoritative?

## I.4 B4
- Is every line authored?
- Is trigger context typed?
- Is speaker selection deterministic?
- Does real-time suppression contaminate replay?
- Are audio and text sinks separate from Core?
- Does missing audio still provide accessible text?
- Are cooldown/history fields saved only when needed?

---

# APPENDIX J — FINAL NO-FALSE-CLOSURE RULES

Wave 12 Part 1 is not complete if:

- B1 has controller bindings but a required action still needs mouse;
- B1 focus can land on hidden/background controls;
- B1 hardcodes deadzone/text-scale values not supported by current policy;
- B1 stores focus in campaign save;
- B2 calls 200 simulated days a literal 200-hour soak without contract;
- B2 loads an unvalidated backup;
- B2 fills required corrupt save sections with invented defaults;
- B2 uses timestamps in deterministic simulation;
- B3 replaces one heuristic belief system with another hidden inference layer;
- B3 converts truly unique item behavior into overly broad tags;
- B3 duplicates immutable authored identity in saves unnecessarily;
- B4 uses runtime-generated dialogue;
- B4 lets frame time determine deterministic campaign voice history;
- B4 creates a second audio priority/ducking system;
- B4 drops critical information when audio is missing;
- any B task lacks focused test evidence;
- any implementation log/census/claim handoff is stale;
- final Part 2 queue is derived from file numbering rather than the current DAG.

**Final invariant:** after Wave 12 Part 1, ASHFALL is mouse-free across the representative core day, durable under long-running deterministic save/recovery scenarios, explicit about survivor/item identity, and coherent in deterministic authored survivor voice—without introducing a second input, save, identity, narrative, or audio authority.


# APPENDIX K — FINAL MERGE-READINESS AUDIT

## K.1 B1 merge readiness

B1 is merge-ready only when:

- the canonical action declaration matrix has no unexplained orphan;
- global semantic actions route through the current central dispatcher;
- local control behavior that remains panel-owned is explicitly justified;
- every representative panel has deterministic initial focus;
- modal focus containment has a characterization test;
- focus returns sensibly when a modal/detail closes;
- controller defaults do not conflict with required actions;
- deadzone/repeat behavior is sourced from current policy;
- supported text scaling does not hide critical controls;
- dynamic glyphs reflect the currently active device/binding;
- rebindings survive a fresh application/session load;
- the mouse-free day commits the expected domain state without pointer input;
- the build remains at the warning baseline;
- the implementation log and census are current.

## K.2 B2 merge readiness

B2 is merge-ready only when:

- the durability target is described precisely and honestly;
- the accelerated deterministic soak does not masquerade as a literal real-time soak;
- repeated runs with the same seed produce the same authoritative terminal state;
- fresh-session restore at the terminal checkpoint matches continuous execution;
- managed/unmanaged leak evidence uses the correct test layer;
- primary/backup rotation never promotes an unvalidated save;
- primary-corrupt/backup-valid behavior is explicit and tested;
- primary-corrupt/backup-invalid behavior fails safely;
- missing-section defaults are schema-authorized;
- supported legacy migrations preserve valid progress;
- injected write failures leave the last known-good save intact according to contract;
- temp/staging files cannot become authoritative accidentally;
- locale changes do not alter semantic save values;
- nonsemantic timestamps remain outside deterministic state;
- restore does not duplicate one-shot events;
- save docs and the store contract matrix are current.

## K.3 B3 merge readiness

B3 is merge-ready only when:

- authored identity data has a single canonical schema/owner;
- every old belief/profile inference callsite is either migrated, retired, or explicitly justified;
- profession data reaches the existing duty/fitness owner without bypassing health/skill restrictions;
- hardcoded item checks have been classified before migration;
- category behavior uses declared tags;
- unique-item behavior stays explicit;
- tag definitions validate against the current schema;
- custom tagged-item contract tests prove extensibility;
- keepsake/heirloom effects reuse the memory/morale rails sealed earlier;
- generated survivor identity is deterministic and does not reroll on restore;
- old saves load under the current migration/default contract;
- survivor detail presentation remains read-only/diegetic;
- data integrity and focused survivor/inventory suites are green.

## K.4 B4 merge readiness

B4 is merge-ready only when:

- the current narrative/voice sources were inventoried before creating any new catalog;
- every delivered line is authored finite content;
- every semantic trigger is typed;
- survivor eligibility reads canonical state;
- B3 identity data is reused where required;
- candidate ordering is stable;
- one deterministic selection method is authoritative;
- the attention-budget time model is explicitly split between simulation and presentation time;
- critical priority reuses current semantic/audio priority;
- multi-sink delivery does not duplicate semantic history;
- missing audio yields text/caption fallback;
- every audible line is accessible as text;
- save/restore preserves future selection when cooldown/history is stateful;
- audio concurrency/ducking selftests are green;
- scene/session teardown leaks no subscriptions;
- build warnings remain at baseline.

---

# APPENDIX L — PART 1 FULL EIGHT-TASK CLOSEOUT PACKET

When A1–A4 and B1–B4 are all terminal, issue one consolidated Wave 12 Part 1 closeout.

## L.1 Task status table

| Task | Corpus | Terminal state | Key delivered contract | Remaining blocker |
|---|---|---|---|---|
| A1 | C1[16] |  | content reachability |  |
| A2 | C1[17] |  | bounded spatial presentation |  |
| A3 | C1[18] |  | repository/tooling hygiene |  |
| A4 | C1[19] |  | medical continuity |  |
| B1 | C2[15] |  | input/controller parity |  |
| B2 | C2[16] |  | session/save durability |  |
| B3 | C2[17] |  | authored identity/tags |  |
| B4 | C2[18] |  | deterministic authored voice |  |

## L.2 Architecture invariants

Confirm:
- Godot-only runtime;
- Core engine-free;
- JSON/catalog authority intact;
- no second content resolver;
- no simulation state in presentation;
- no second save store;
- no second identity model;
- no second voice/audio priority manager.

## L.3 Persistence summary

Record:
- A1 state changes, if any;
- A4 medical save changes;
- B1 user setting changes;
- B2 save/recovery changes;
- B3 identity/save migration;
- B4 narrative history/cooldown persistence.

Every change identifies:
- owner;
- old-save behavior;
- restore test.

## L.4 Determinism summary

Record:
- content selection;
- medical exposure/progression;
- mouse-free semantic command parity;
- long-run soak;
- generated survivor identity;
- survivor voice selection.

Every deterministic claim points to a test/fingerprint.

## L.5 Quality gates

Record actual current results for:
- build;
- warnings;
- data integrity;
- content utilization;
- panel lifecycle;
- UI accessibility;
- audio;
- verify-fast;
- save corruption/migration;
- any new provider/tooling gates.

Do not paste historic case counts as current truth.

## L.6 Governance

Record:
- active claims after handoff;
- decision-deferred items;
- routed repairs;
- census statuses;
- integration ledger;
- duplicate/superseded plans if discovered.

## L.7 Wave 12 Part 2 queue

Recompute from final merged `HEAD`.

List:
- top ready node;
- dependencies;
- claims;
- decisions;
- acceptance;
- why it outranks alternatives.

---

# APPENDIX M — FINAL RELEASE NOTE TEMPLATE

# ASHFALL Wave 12 Part 1 — Closeout

## Input & Accessibility
**B1 status:**\
**Canonical input owner:**\
**Actions classified:**\
**Mouse-free journey:**\
**Focus/modal isolation:**\
**Rebinding persistence:**\
**Accessibility:**\

## Session Durability
**B2 status:**\
**Soak contract:**\
**Terminal restore parity:**\
**Backup recovery:**\
**Atomic-write proof:**\
**Migration/corruption:**\
**Locale invariance:**\

## Survivor Identity
**B3 status:**\
**Authored belief owner:**\
**Profession integration:**\
**Tag authority:**\
**Hardcoded-ID migration:**\
**Keepsake integration:**\
**Generated identity replay:**\

## Survivor Voice
**B4 status:**\
**Voice catalog owner:**\
**Semantic trigger contract:**\
**Selection method:**\
**Attention model:**\
**Multi-sink routing:**\
**Audio fallback:**\
**Replay/history:**\

## Whole Part 1
**A1:**\
**A2:**\
**A3:**\
**A4:**\
**Build:**\
**Warnings:**\
**Data integrity:**\
**UI lifecycle/a11y:**\
**Audio:**\
**Save durability:**\
**Census/ledger:**\

## Next queue
**Wave 12 Part 2 head:**\
**Reason ready:**\
**Claims:**\
**Decision blockers:**\

---

# APPENDIX N — FINAL ACTUALITY HEARTBEAT

Immediately before starting each B task, re-answer:

1. Did Part 1.1 merge change the relevant authority?
2. Did a concurrent agent already close part of the plan?
3. Are the supplied class/file names still current?
4. Did a new claim appear?
5. Did the census status change?
6. Did a decision memo already resolve a draft assumption?
7. Are hardcoded counts/thresholds still true?
8. Is the package still bounded?
9. Did a prior task introduce a new save field requiring B2 coverage?
10. Did B3 change identity inputs B4 must consume?

Any changed answer updates the implementation-log premise before coding.

---

# APPENDIX O — FINAL IMPLEMENTER SIGN-OFF

- [ ] B1 retains exactly 20 procedural substeps.
- [ ] B1 retains exactly four mini-tasks with four mini-substeps each.
- [ ] B2 retains exactly 20 procedural substeps.
- [ ] B2 retains exactly four mini-tasks with four mini-substeps each.
- [ ] B3 retains exactly 20 procedural substeps.
- [ ] B3 retains exactly four mini-tasks with four mini-substeps each.
- [ ] B4 retains exactly 20 procedural substeps.
- [ ] B4 retains exactly four mini-tasks with four mini-substeps each.
- [ ] No Core engine dependency was added.
- [ ] No duplicate input/save/identity/voice authority was introduced.
- [ ] All draft numeric assumptions were reverified before enforcement.
- [ ] Long-run durability terminology is truthful.
- [ ] Rebinding persistence is user/profile scoped.
- [ ] Save recovery validates backups before loading.
- [ ] Authored identity replaces inference only where Plan 40 requires.
- [ ] Item-specific behavior was not accidentally generalized by tags.
- [ ] Voice content remains finite/authored.
- [ ] Simulation-relevant voice selection/cooldowns remain deterministic.
- [ ] All modified docs/indexes are synchronized.
- [ ] Implementation logs/census/claims agree at final `HEAD`.
- [ ] Wave 12 Part 2 will be generated from the refreshed DAG.

**End of Wave 12 Part 1.2 implementation-unblocker plan.**
