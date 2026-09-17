# ASHFALL — First-Week World, Dedicated Asset Coverage, and High-Traffic UI Integration Plan

**Document type:** Flagship implementation and integration plan<br>
**Scope:** New sequential integration batch containing three coupled production tasks<br>
**Primary outcome:** Replace visible first-week placeholder presentation, raise dedicated player-facing art coverage, and make the six highest-traffic UI surfaces visually cohesive without weakening deterministic state, accessibility, or existing registry/theme contracts.<br>
**Repository mode:** Existing worktree contains substantial unrelated changes; this package must be isolated, narrowly claimed, and ledgered before implementation.<br>
**Target implementation order:** Task 1 → Task 2 → Task 3, with carefully defined overlap windows rather than three parallel unbounded art/UI edits.<br>
**Target review mode:** Runtime-first visual approval plus existing automated checks. Snapshot replacement is never accepted solely because a test changed.<br>

> The central rule for this batch is simple: improve what the player actually sees in the first minutes without creating a second art system, a second UI styling system, or a second source of truth. Existing runtime contracts remain authoritative.

## 0. Executive integration intent

This batch addresses a specific maturity gap: the game already has functional world, panel, registry, and theme infrastructure, but the first-session presentation still exposes placeholder world art, fallback-heavy player-facing assets, and visual drift between high-traffic interfaces. Treating those as three isolated cosmetic tickets would create rework. The correct integration shape is one sequential package in which Task 1 establishes the environmental art language, Task 2 expands that language across the identities players repeatedly encounter, and Task 3 consumes both through the existing shared UI shell and theme.

The expected player-facing result is not “more art.” It is a coherent first-week visual contract. A player should be able to move from Main Menu to Shelter, open Inventory or Survivors, inspect the Map or Expedition layer, then check Factions or Radio without encountering a conspicuous change in rendering language, icon scale, contrast logic, border weight, lighting temperature, or asset quality. Dedicated images should support the underlying game state instead of obscuring it. Warnings, unavailable actions, selection, injury, scarcity, allegiance, and night/day phase must remain legible when color information is absent or reduced.

The batch is deliberately bounded. It does not attempt a whole-game art replacement, a UI framework rewrite, a redesign of navigation, a new content-addressing layer, or a generalized visual effects pass. The “first 150” asset replacement slice is selected by exposure and gameplay relevance; unresolved identities remain ranked for later batches. Likewise, the UI work targets six surfaces because they dominate early-session use and share enough infrastructure for a theme-level intervention to produce broad value without uncontrolled scope.

## 1. Non-negotiable integration rules

1. **Ledger before implementation.** The current batch is closed. Determine the latest integration-ledger sequence number, allocate the next three contiguous task entries as one new batch, and commit or otherwise persist those ledger rows before any production asset or UI code edit. If the ledger uses a batch header plus children, create the batch header first and make all three tasks children of that batch. Never reuse an old number simply because a ticket looks related.
2. **Worktree isolation.** The existing tree contains substantial unrelated work. The foreman package may claim only the paths explicitly needed for this batch, plus narrowly justified test/manifest/generated-artifact paths. A file outside the claim set is read-only until the package is amended.
3. **No parallel source of truth.** `BackdropArt`, `AssetRegistry`, `DesignTheme`, `AshfallDashboardShell`, the shared sidebar/status rail/helpers, and their existing contracts remain the owners. Do not introduce a sidecar JSON map just to make generated art resolve, screen-local color constants to avoid using theme tokens, or phase-specific hardcoding that bypasses the existing switch contract.
4. **Reuse before generation.** Every image request begins with repository inspection. An existing viable illustration, texture, crop, or partial can be repaired or recomposed. “AI generation” is an option, not the default first action.
5. **Exact runtime dimensions and semantics.** Surface backdrops are delivered at the dimensions already consumed by the game, not arbitrary generator dimensions that are stretched later. Item/portrait/location/faction art follows the dimensions, transparency expectations, and search-path behavior already supported by the registry.
6. **Runtime approval outranks contact-sheet appeal.** A candidate that looks attractive in isolation but fails behind text, at 1280×720, under selection chrome, or in a dark phase is rejected or corrected.
7. **Deterministic visual QA.** Populated fixtures must be deterministic. Tests and captures may not depend on wall clock, random inventory rolls, random survivor order, or external asset-generation availability.
8. **Snapshot discipline.** Do not replace a baseline just because a new rendering differs. First determine whether the difference is intentional, accessible, and stable. Baseline approval requires human visual review.
9. **Accessibility is part of art direction.** Status information cannot be color-only. Focus, warning, disabled, selected, hostile/friendly, and critical states require shape, text, icon, weight, border, or pattern cues in addition to hue.
10. **No fake readable text in generated imagery.** Environmental signs, crates, walls, radios, posters, labels, and faction marks may contain abstract glyph-like marks only when appropriate. Generated nonsense lettering is treated as an artifact and removed.
11. **No neon drift.** Atomic green and plutonium purple may exist elsewhere in the broader project language, but this batch’s first-week backdrop brief is explicitly cold blue-grey concrete, rust, dirty bone, and restrained amber. Any high-chroma accent must be justified by actual state UI, not baked indiscriminately into scenery.
12. **Manifest/status truthfulness.** Placeholder manifests, coverage reports, and status docs are updated only after runtime acceptance. A generated file existing on disk is not equivalent to “done.”

## 2. Batch creation and foreman/worktree protocol

### 2.1 Allocate the new sequential batch

Before implementation, inspect the integration ledger and identify its actual latest sequence. Represent the tasks provisionally as `N+1`, `N+2`, and `N+3` until the numbers are known. Do not guess the current sequence from filenames, issue numbers, or older plans. Once discovered, reserve all three consecutive entries in one edit so another concurrent package cannot interleave itself between them.

The ledger rows should record at minimum: batch identifier; task sequence; concise task title; target outcome; claimed primary paths; explicitly allowed generated/status paths; dependencies; test gates; runtime capture requirement; reviewer state; and a status of `PLANNED` or the repository’s equivalent. A useful dependency shape is `Task N+1 -> Task N+2 -> Task N+3` for final approval, while allowing Task N+2’s inventory/repository audit to begin after Task N+1’s visual brief is frozen.

The ledger description must mention that the worktree already contains unrelated changes. This is not incidental context: it is an operational constraint. The package must refuse broad cleanups, mass formatting, opportunistic refactors, or regenerated artifacts unrelated to the claimed surfaces.

### 2.2 Foreman package claim set

Start with the narrowest explicit claim. Task 1 may claim the first-week backdrop and shelter-art paths plus the code owners necessary to inspect and validate their consumption. Task 2 may claim the four dedicated sprite families and registry/coverage reporting paths. Task 3 may claim theme/shell/helper/snapshot-fixture paths. Tests and manifests are claimed only as they become necessary.

Recommended initial claim groups:

| Group | Read/write intent | Initial paths |
|---|---|---|
| T1 world backdrop runtime | inspect + targeted edit | `src/UI/BackdropArt.cs`, `src/World/HoldfastInteriorView.cs` |
| T1 world backdrop art | replace placeholders only | `assets/sprites/Surface/`, `assets/sprites/Shelter/` |
| T2 registry runtime | inspect + targeted edit only if required | `src/Host/AssetRegistry.cs`, `src/Host/AssetCoverageScanner.cs` |
| T2 dedicated art | add/repair selected identities | `assets/sprites/Items/`, `assets/sprites/Portraits/`, `assets/sprites/Locations/`, `assets/sprites/Factions/` |
| T3 shared UI system | shared-token/component edits | `Assets/Ashfall.Core/UI/Theme.cs`, `src/UI/AshfallDashboardShell.cs`, `src/UI/AshfallUiHelpers.cs` |
| T3 visual QA | fixture/manifest updates after approval | `docs/ui/snapshot_manifest.json` and the repository’s existing fixture/snapshot directories |

If path casing differs in the repository, use the actual casing discovered on disk; do not duplicate a path under a differently cased tree. If generated reports such as `artifacts/asset_registry.*` are tracked or required evidence, add those exact generated paths to the package only when regeneration is scheduled.

### 2.3 Dirty-tree containment

Create a preflight inventory of changed and untracked files before touching the batch. Store the output in the package notes or a temporary evidence file outside committed product assets. Classify each changed path as: unrelated pre-existing; directly in claim set; generated by this batch; or suspicious overlap. Any overlap with unrelated work is a stop condition until ownership is resolved.

Do not use destructive cleanup commands to obtain a clean tree. Do not reset another package’s work, discard untracked art, or overwrite a generated manifest without confirming whether it belongs to another task. For files that must be edited despite pre-existing modifications, capture a diff before the batch edit and preserve the unrelated hunks exactly.

### 2.4 Commit and review boundaries

Prefer reviewable checkpoints rather than one enormous mixed commit. A robust sequence is: ledger/preflight; Task 1 art brief/specs; Task 1 approved asset replacement; Task 1 runtime/tests/status; Task 2 ranked batch manifest; Task 2 asset additions/repairs; Task 2 registry/coverage evidence; Task 3 theme/shared-component changes; Task 3 deterministic fixtures; Task 3 approved snapshots and final cross-batch evidence. If the repository convention requires squashing, preserve these logical boundaries in commit messages or review notes.

## 3. Cross-task dependency map and critical path

The three tasks are tightly related but should not all mutate the same presentation layer at once. The critical path is:

`Ledger + preflight -> T1 capture/audit -> art-language lock -> T1 replacements -> T1 runtime approval -> T2 ranked slice -> T2 generation/repair -> registry proof -> T3 shared-token pass -> populated fixtures -> T3 visual/a11y approval -> final cross-batch sweep`.

Two overlaps are safe. First, after the Task 1 art brief is frozen, Task 2 can begin its coverage report, exposure ranking, and do-not-regenerate audit while Task 1 asset correction continues. Second, Task 3 can capture current populated UI states before Task 2 is finished, because those captures are baselines; however, final Task 3 layout/chrome approval should use the real Task 2 art so that cell density, contrast, and portrait/icon treatment are evaluated with production assets rather than fallbacks.

The main prohibited overlap is styling high-traffic panels while their asset dimensions/search behavior are still changing. Otherwise, UI snapshots churn for reasons unrelated to the shared visual treatment, making regression review noisy and encouraging baseline replacement rather than diagnosis.

## 4. Definition of Done for the entire batch

The batch is complete only when all of the following are true:

- The three tasks exist as contiguous new entries in the integration ledger and are marked complete using evidence from the current implementation, not plan intent.
- The first-week Map, Map Detail, Expedition, and Shelter views show approved non-placeholder art across day/dawn/dusk/night wherever phase variation is part of the current contract.
- The 12 surface images and four shelter-interior lighting variants are present at the exact dimensions consumed by runtime and are resolved through existing naming/phase logic.
- A focused orphan scan and scene lint show no newly orphaned first-week assets or broken references attributable to the batch.
- The curated 150-identity slice is generated or repaired and lives in the registry’s existing search paths, with no parallel mapping database.
- The dedicated-asset coverage report shows the expected material improvement from the pre-batch 29.91% baseline toward roughly 39%. The exact achieved percentage is reported rather than forced; if the target is missed, the reason is explained with loaded/fallback counts.
- Inventory, Survivors, Map, Factions, and relevant detail views demonstrably resolve the correct selected asset identities in populated states.
- Main Menu, Shelter HUD, Inventory, Survivors, Map/Expedition, and Faction/Radio share a coherent terminal visual hierarchy through `DesignTheme`, `AshfallDashboardShell`, common rails/sidebar, and helpers.
- The six surfaces pass keyboard/controller focus, close/back behavior, readable text, non-color status-cue, and colorblind-oriented checks.
- Any new populated snapshot fixture is deterministic and does not replace a legitimate empty-state baseline unless both states are intentionally represented.
- Snapshot diffs are reviewed by a human. Approved baselines correspond to reviewed intent; unreviewed regressions remain failures.
- The final bounded Godot capture demonstrates the complete flow at both 1920×1080 and 1280×720 for the representative states named in this plan.
- Placeholder/coverage/status manifests are updated last, after runtime approval, and their counts/labels match what the game actually resolves.

# Task 1 — Replace the first-week placeholder world

## 5. Task 1 outcome and boundaries

**Target:** make the first minutes read as a coherent, finished survival game rather than a functional build with temporary presentation.

Task 1 owns the environmental “visual grammar” for this batch. Its job is not to create maximal illustration variety. It is to establish a family of first-week backdrops that supports legible UI overlays, time-of-day phase switching, and a grounded post-collapse tone. The player should recognize that Map, Map Detail, Expedition, and Shelter belong to the same world even though their framing and dimensions differ.

The implementation must preserve `BackdropArt` and the shelter interior’s existing selection logic. If the code already resolves phase variants by filename stem or an enum-to-path mapping, that contract is treated as public API for this batch. Prefer making the art fit the contract. Code changes are justified only when there is a demonstrated bug or a clearly labelled placeholder path cannot be reached correctly.

Out of scope: adding new world locations, redesigning weather systems, changing day-phase duration, introducing parallax just for this task, procedural image composition, adding shaders to conceal weak art, or changing gameplay values to make screenshots look better.

## 5.1 Open and constrain the foreman package

Create the new package after the ledger rows exist. Claim only the first-week backdrop and shelter-art directories plus the two runtime owners and the exact QA/manifests needed. Record a pre-edit dirty-tree inventory. Establish a package note that generated candidates outside the selected final set are working material and must not be copied into runtime search paths.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Package identifier linked to the new ledger batch
- Claim-set table with read/write paths
- Dirty-tree evidence and overlap check
- Stop rule for pre-existing edits in claimed files

**Acceptance conditions**

- No unrelated file is modified by package initialization
- The package can name every writable path before asset replacement starts

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.2 Capture the current first-week presentation

Capture Map, Map Detail, Expedition, and Shelter in representative first-week state at day, dawn, dusk, and night where each phase is supported. Use the same deterministic save/fixture where practical so differences represent time phase rather than different content. Capture both 1920×1080 and 1280×720 for any view whose crop/overlay behavior changes materially between resolutions.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Baseline capture contact sheet
- Per-screen notes for current placeholder visibility
- Overlay safe-area observations
- Known crop/stretch artifacts

**Acceptance conditions**

- Every target screen has enough evidence to compare before/after
- Phase labels and resolution are unambiguous in filenames or metadata

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.3 Inspect and reuse existing art

Search the existing art and sprite trees by visual family, location identity, filename stem, and likely historical naming. Review compositions rather than assuming all unused files are obsolete. A viable old environment may need crop, value correction, texture cleanup, or lighting variants rather than replacement. Classify candidates as reuse-as-is, repair, derive-variant, reference-only, or reject.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Reuse inventory with source path and decision
- Duplicate/near-duplicate notes
- List of images that must not be regenerated
- Gap list for genuinely missing masters

**Acceptance conditions**

- Every planned generation request has an explicit reason existing art cannot satisfy it
- No selected old asset introduces a new incompatible style family

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.4 Lock the one-page art brief

Freeze the environmental language before requesting final candidates. The brief is a production constraint, not inspiration prose: hand-painted charcoal/gouache; cold blue-grey concrete; rust; dirty bone; restrained amber practical light; desaturated weathered surfaces; believable late-civilian/post-industrial construction; no neon; no readable fake text; no glossy concept-art finish; no over-sharp photobash edges; no cinematic bloom that destroys UI contrast.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- One-page art brief
- Positive visual attributes
- Explicit rejection attributes
- Value/contrast guidance for UI-safe regions

**Acceptance conditions**

- All reviewers can use the brief to make consistent accept/reject decisions
- The brief is stable before Task 2 family specs are finalized

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.5 Write mini-specs for 12 surface images + 4 shelter variants

Identify the exact runtime-consumed slots from `BackdropArt` and shelter interior code, then write a mini-spec for each slot. The spec records existing filename/stem, dimensions, phase semantics, focal placement, required negative space, horizon/edge constraints, allowed light sources, and overlay hazards. Do not invent a new naming scheme. If there are exactly twelve surface slots in code, the spec list must map one-to-one to them.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Twelve surface slot specs
- Four shelter lighting specs
- Filename-contract crosswalk
- Dimension and crop matrix

**Acceptance conditions**

- Every runtime path maps to exactly one intended deliverable
- No deliverable requires code-side guessing to distinguish phase

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.6 Generate candidates as families

Generate two or three candidates per master scene, not per arbitrary crop, using one coherent provider/style family for the selected set. Variants should share brush texture, edge softness, material language, horizon treatment, and palette. If multiple providers are explored, compare them at the family level and choose one family rather than mixing the best isolated image from each provider.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Candidate sheets by master scene
- Provider/settings provenance where relevant
- Selection rationale
- Rejected-artifact notes

**Acceptance conditions**

- Selected masters look related when viewed together
- No master is selected solely because it is individually dramatic

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.7 Correct, normalize, and deliver exact dimensions

Treat raw generation as source material. Remove malformed architecture, impossible perspective, repeated object hallucinations, pseudo-text, watermark-like marks, broken silhouettes, and noisy microdetail. Normalize overall value distribution and palette against the art brief. Deliver the exact existing dimensions: 1920×1080 map sky, 1280×720 expedition/detail, and 760×420 shelter. Resize/crop intentionally; do not let the engine become the primary resampler.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Clean master exports
- Runtime-ready exact-dimension files
- Artifact-cleanup checklist
- Palette/value consistency review

**Acceptance conditions**

- No target relies on runtime stretch to fix composition
- Important landmarks and negative-space regions survive both supported display resolutions

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.8 Replace labelled placeholders without contract drift

Replace only the clearly identified placeholder assets. Preserve filename stems and phase-switch naming when those are the current contract. If version control benefits from keeping source masters, store them only in an established source-art location that is not scanned as runtime content. Avoid leaving `final2`, `new`, provider names, or prompt text in runtime filenames.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Minimal runtime asset diff
- Optional source-art provenance in approved location
- No duplicate scanned stems

**Acceptance conditions**

- `BackdropArt` and shelter code resolve the replacements without a parallel lookup
- The diff does not contain abandoned candidates in runtime folders

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.9 Validate all time phases inside real Godot panels

Open each target panel in runtime, not only a texture viewer. Evaluate text legibility, aspect coverage, dark/bright region collisions, foreground silhouette contrast, selection/highlight readability, and the emotional transition across dawn/day/dusk/night. Where phase is represented by distinct images, ensure the sequence feels like the same place under different conditions rather than four unrelated scenes.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Approved runtime captures
- Per-phase contrast notes
- Corrections list with before/after evidence

**Acceptance conditions**

- No phase makes essential UI text or markers unreadable
- Shelter variants preserve spatial continuity and practical light logic

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 5.10 Run focused validation and update status last

Execute the repository’s existing focused asset-orphan sweep, scene lint, relevant panel self-tests, and a bounded Godot capture. Review failures rather than suppressing them. Only after runtime approval should the placeholder manifest/status be updated to mark the replaced slots complete.

**Required implementation actions**

- Establish the current behavior with code/path evidence before editing.
- Record any assumption that could affect naming, dimensions, phase selection, or rendering order.
- Keep the production change limited to this step's claimed paths.
- Review the result in runtime before promoting it to the next phase.

**Deliverables**

- Orphan-scan result
- Scene-lint result
- Panel self-test result
- Bounded capture evidence
- Updated placeholder/status manifest

**Acceptance conditions**

- All new failures are resolved or explicitly attributed to pre-existing unrelated work
- Status documentation matches the files actually loaded in runtime

**Failure/rollback rule**

If the step cannot be completed without modifying an unclaimed subsystem, stop and amend the package/ledger rather than smuggling the dependency into the diff. If a replacement degrades readability or continuity, restore the prior runtime asset while keeping the diagnostic evidence; do not force progression because generation effort has already been spent.

## 6. Task 1 — detailed art-direction contract

### 6.1 Material language

Concrete should read as cold, porous, chipped, damp, or smoke-stained rather than clean architectural visualization. Rust belongs on exposed ferrous edges, improvised fasteners, abandoned vehicles, railings, fixtures, and water paths, but should not tint the entire image orange. Dirty bone is the principal pale neutral for cloth, plaster remnants, paper, dusted signage shapes, aged plastics, and highlight separation. Restrained amber is reserved for believable tungsten lamps, sheltered windows, stove/fire spill, or select utility indicators. It should function as a focal temperature counterpoint to the cold environment.

Brushwork should imply charcoal underdrawing, gouache blocking, and hand-restored edges. Avoid the visual signature of generative “hyper-detailed post-apocalypse” art: excessive random cables, hundreds of micro-props, dramatic orange/teal grading, pristine volumetric shafts, or every surface carrying decals and invented typography. Texture is welcome when it supports material; texture noise that competes with small UI copy is not.

### 6.2 Composition and overlay safety

Each scene mini-spec should define three zones: focal zone, quiet UI-safe zone, and expendable crop zone. Focal content should not sit directly beneath recurring headers, sidebars, status rails, or action affordances. UI-safe zones are not empty; they simply maintain low-frequency value structure and controlled contrast. Expendable crop zones allow `keep_aspect_covered` or equivalent behavior to remove peripheral content without destroying scene identity.

Avoid putting the brightest practical light directly behind white body text, placing high-contrast diagonal structures behind compact metrics, or creating face-like/high-saliency objects behind selectors. If the engine applies additional darkening or phase tint, evaluate both raw and composited values because a theoretically safe image may crush to black in runtime.

### 6.3 Time-of-day continuity

A phase family should preserve the major geometry, camera location, and important world landmarks. Dawn may introduce cool diffuse sky with small warm practical remnants; day should reveal material information without becoming cheerful; dusk can increase local amber contrast while preserving cold ambient fill; night should deepen negative space but retain enough structural separation for navigation. Do not create four radically different weather conditions merely to make phases distinct unless the runtime already couples those concepts.

### 6.4 Pseudo-text and symbolic detail

Generated readable words are prohibited. If a scene logically needs signs or papers, use blank plates, worn strips, abstract blocks, hand-painted strokes, symbols too fragmentary to read, or later-authored real text rendered by the UI where appropriate. Any accidental letterform sequence that looks like a word should be painted out during correction.

## 7. Task 1 — mini-spec schema for the 16 consumed images

Do not populate final filenames from memory. Derive them from `BackdropArt`, `HoldfastInteriorView`, and the existing assets. For each of the 12 surface images and four shelter lighting variants, create a row with the following fields:

| Field | Required content |
|---|---|
| Runtime slot | Exact code-side role or phase key |
| Existing filename/stem | Exact current contract, including suffix pattern |
| Directory | Existing runtime search path |
| Required dimensions | 1920×1080, 1280×720, or 760×420 as actually consumed |
| Scene identity | What physical place/angle this image represents |
| Phase | day/dawn/dusk/night or non-phase role |
| Focal zone | Main landmark/subject and allowed screen region |
| UI-safe zone | Area that must remain low-noise/readable |
| Crop tolerance | Edges that can be lost at aspect cover |
| Material emphasis | Concrete/rust/bone/fabric/soil/metal balance |
| Lighting rule | Ambient/practical source logic |
| Prohibited artifacts | Pseudo-text, neon, glossy finish, geometry errors, etc. |
| Reuse source | Existing art to repair/derive, if any |
| Candidate IDs | Working candidate references outside runtime folders |
| Approved master | Selected source candidate |
| Runtime approval | Reviewer/date/evidence capture |

The shelter variants need an additional continuity field recording unchanged furniture/structural anchors. If the current runtime uses only lighting variants rather than full redraws, prefer deriving them from one approved shelter master so object placement cannot drift between phases.

# Task 2 — Replace the highest-exposure fallback asset slice

## 8. Task 2 outcome and scope logic

**Target:** increase dedicated-asset coverage from the stated 29.91% baseline toward roughly 39% with a curated first batch of 150 real, player-facing assets: 70 items, 25 portraits, 35 locations, and 20 faction emblems.

The number 150 is a production bound, not a license to choose arbitrary easy IDs. The selected slice should maximize visible improvement per asset by favoring identities the player encounters early and repeatedly. A low-frequency late-game codex object with a fallback is less valuable than a starting food item, a recurring survivor, an early map location, or a faction whose emblem appears in several screens.

Coverage percentage is an outcome metric, not the selection algorithm. If some curated identities share a legitimate reusable asset or if the scanner counts categories differently, the final percentage may not land exactly on 39%. Report the actual loaded and fallback counts before and after. Do not create meaningless unique art solely to manipulate the percentage.

## 9. Asset selection scoring model

Build a ranking table from the existing full coverage report. Each unresolved identity receives evidence-backed scores, for example on a 0–5 scale, across the following factors:

- **First-session probability:** likelihood the asset appears during the first 30–60 minutes under normal play.
- **Recurrence:** how often the identity appears after introduction.
- **Screen prominence:** thumbnail, card portrait, large vignette, emblem, or background-level usage.
- **Decision relevance:** whether the player makes inventory, survivor, travel, trade, allegiance, or risk decisions while looking at it.
- **Cross-screen reuse:** number of distinct high-traffic surfaces that consume the same identity.
- **Narrative salience:** named survivor, major faction, signature location, or quest-critical object.
- **Fallback ugliness penalty:** how conspicuous or semantically misleading the current fallback is.
- **Existing-art repairability:** positive weight when a near-complete asset can be repaired cheaply without sacrificing quality.

Use category quotas as hard caps for the first slice: 70 items, 25 portraits, 35 locations, 20 faction marks. Within each quota, sort by weighted exposure score, then manually inspect the top candidates for duplicates, variants, and IDs that should share visual treatment. Record the reason for any lower-ranked inclusion or higher-ranked deferral.

## 10.1 Regenerate the authoritative coverage report

Run the existing full asset-coverage workflow before changing any relevant asset. Preserve the report as the batch baseline. Export unresolved identities by category with the scanner’s exact ID/stem and current resolution reason so later comparisons do not rely on memory.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Baseline `loaded` count
- Baseline `fallback` count
- Baseline dedicated coverage percentage
- Unresolved list by category

**Acceptance conditions**

- The stated 29.91% can be reconciled to scanner counts or a discrepancy is documented
- No old report is silently treated as current

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.2 Rank by actual player exposure

Join scanner output to gameplay knowledge and repository references. Starting inventory, tutorial/first-week quests, recurring survivor cards, common map nodes, and factions present in the early radio/diplomacy loop receive priority. Use code/content references or deterministic playthrough data where available rather than ranking from names alone.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Exposure-scored candidate table
- Evidence field per high-priority ID
- Deferred-high-rank rationale when applicable

**Acceptance conditions**

- The top of each category is dominated by genuinely player-facing content
- Selection can be explained without reference to generation convenience

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.3 Freeze the 150-ID batch

Select exactly 70 item IDs, 25 portrait IDs, 35 location IDs, and 20 faction emblem IDs. Freeze the manifest before production so the batch does not drift toward assets that are easier to generate. Permit substitutions only through an explicit manifest change with reason, especially if an ID is discovered to have viable existing art.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Versioned 150-ID manifest
- Category counts
- Selection score and rationale
- Substitution log field

**Acceptance conditions**

- Counts equal 150 and match category quotas
- Each selected identity is unresolved or legitimately needs repair at freeze time

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.4 Perform do-not-regenerate audit for every selected ID

Search `assets/art/` and all relevant sprite/runtime directories by exact ID, normalized stem, aliases, likely display name, and visually related family. Inspect candidate images, not just filenames. Mark each selected ID as generate-new, repair-existing, crop/derive-existing, or already-viable-and-rewire if a legitimate registry resolution bug is the real problem.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- 150-row do-not-regenerate result
- Existing source paths
- Decision per identity

**Acceptance conditions**

- No generation job is launched for an identity with an acceptable existing asset
- Registry bugs are not disguised as missing-art problems

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.5 Define four reusable family specs

Create concrete production specs for item icon, survivor portrait, location vignette, and faction mark. Each spec defines canvas/aspect expectations from current UI, palette relationship to Task 1, edge treatment, lighting direction, background/transparency behavior, detail density at rendered size, and forbidden artifacts.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Item icon family spec
- Portrait family spec
- Location vignette family spec
- Faction mark family spec

**Acceptance conditions**

- Specs support batch consistency without making all assets look identical
- Each spec is compatible with current registry and cell rendering

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.6 Produce/repair assets in coherent batches

Group work by family and related visual sets. For items, process material/usage families so scale and silhouette remain comparable. For portraits, preserve a shared framing and light logic while allowing identity. For locations, use the Task 1 environment language. For factions, design readable silhouettes and value structure that work at small emblem sizes.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Working batch contact sheets
- Selected finals
- Repair notes for reused art

**Acceptance conditions**

- A random sample from each family appears stylistically related
- No selected asset depends on readable AI-generated lettering

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.7 Normalize technical delivery

Remove malformed details, pseudo-text, stray halos, compression artifacts, matte fringes, inconsistent crop, and accidental near-transparent noise. Standardize alpha handling, filename stems, texture dimensions/resolution according to existing conventions, and edge padding so UI scaling does not clip important silhouettes.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Runtime-ready normalized files
- Technical QC report
- Filename/stem verification

**Acceptance conditions**

- Transparent assets have clean edges on both dark and light checker backgrounds
- No runtime file includes generator/provider metadata in its stem

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.8 Place assets only in existing registry search paths

Install the selected files under the existing `Items`, `Portraits`, `Locations`, and `Factions` search paths using stems the `AssetRegistry` already resolves. If a selected ID cannot resolve despite following the contract, investigate normalization/alias behavior in `AssetRegistry`; do not create a second hand-maintained mapping file.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Installed asset paths
- Registry resolution trace for exceptions
- No sidecar mapping system

**Acceptance conditions**

- Each selected ID resolves to the intended file through existing registry behavior
- Unselected IDs continue to use their prior behavior

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.9 Exercise populated player-facing panels

Use deterministic populated states to verify Inventory, Survivors, Map, Factions, and any identity detail panel that materially consumes the selected art. Confirm semantic correctness, not merely non-fallback rendering: the water container must not resolve to a different item, a survivor portrait must not be shifted to another ID, and faction marks must match faction identity.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Panel capture set
- Spot-check table linking ID -> expected asset -> observed asset
- Mismatch log and fixes

**Acceptance conditions**

- No sampled selected ID resolves to another identity
- Art remains legible at actual cell/card size

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 10.10 Regenerate registry artifacts and report coverage delta

Run the canonical registry/coverage artifact generation. Compare the new counts against the preserved baseline and retain the unresolved remainder as a ranked next-batch input. Generated artifacts are evidence, not the sole test; cross-check a representative runtime sample.

**Execution controls**

- Do not change category quota or selected identity simply because a generation attempt is difficult.
- Keep source/repaired provenance in the batch manifest when the repository has an established place for it.
- Validate semantics in runtime after each manageable tranche instead of waiting until all 150 are installed.
- Treat registry exceptions as code/path investigations, not an invitation to bypass the registry.

**Deliverables**

- Updated `artifacts/asset_registry.*` where applicable
- Before/after loaded count
- Before/after fallback count
- Coverage percentage delta
- Next ranked unresolved list

**Acceptance conditions**

- Coverage improves materially toward the target
- Counts are internally consistent with the 150-ID manifest and shared-asset behavior

**Rollback rule**

If an asset causes semantic ambiguity, visual failure at runtime size, or registry collision, remove that runtime file and return the identity to its previous fallback until corrected. The batch manifest may retain it as in-progress, but coverage reports must reflect actual resolvable state.

## 11. Task 2 family specification — item icons

Item icons should optimize silhouette recognition at the size the Inventory actually renders, not at 1024-pixel review size. Maintain a consistent virtual camera and approximate object scale within meaningful families: handheld consumables, tools, medical supplies, ammunition containers, clothing, components, and larger equipment may have different framing rules, but two similar small bottles should not randomly differ by 3× scale.

Background behavior must follow the existing UI contract. If icons are transparent, retain enough painted edge/light separation to survive the dark terminal panel without thick sticker-like outlines. If the current system expects a framed painted tile, use that established frame rather than converting one family to transparency. Rust, chipped enamel, dirty plastic, fabric, glass, and paper should derive their color/value language from the first-week world so the inventory looks like it belongs to the same environment.

Reject icons with impossible handles, duplicate caps, nonsensical gauge text, fake brand labels, decorative post-apocalyptic spikes, or ambiguous silhouettes. “Interesting” is not better than identifiable. Critical consumables should remain recognizable in peripheral vision and under selected/disabled overlays.

## 12. Task 2 family specification — survivor portraits

Use a stable portrait crop and eye-line so survivor cards do not jump visually as selection changes. Favor documentary, worn, charcoal/gouache portraiture consistent with the world rather than glossy character-concept art. Clothing and skin should carry believable environmental wear without turning every survivor into the same dirt-smudged archetype.

Lighting should come from a consistent broad cool source with restrained warm practical bounce where appropriate. Avoid hard rim lights, neon backlights, beauty lighting, excessive depth-of-field, and backgrounds containing pseudo-text. Distinguish identities through face, age, hair, clothing, posture, injury/scar details when canonically supported, and small personal cues—not through unrelated art styles.

At card scale, faces must remain readable after any status badges, morale/injury overlays, or crop masks are applied. Where the UI applies tinting for state, verify that it does not erase darker skin tones, hair silhouettes, or medical indicators.

## 13. Task 2 family specification — location vignettes

Location art extends Task 1’s environmental language into smaller, identity-rich compositions. Each vignette should answer “where is this?” through one or two strong landmarks rather than environmental clutter. Preserve UI-safe edges for labels, threat markers, travel costs, or lock-state indicators used by the Map/Expedition panels.

The family should share cold blue-grey, bone, rust, and selectively warm practical light, but geography/materials can shift appropriately. Avoid giving every location the same ruined-concrete silhouette. Industrial, residential, medical, civic, transport, rural, and improvised sites need distinct structural vocabularies while remaining in one painted world.

Where a location has a dedicated full backdrop and a vignette, preserve landmark continuity so the player can connect map card to detail view. Do not simply crop a large backdrop if the crop destroys identity at card size; derive a purposeful vignette from the same master or visual concept.

## 14. Task 2 family specification — faction marks

Faction marks must be legible at the smallest size they appear in lists/radio headers. Favor a strong silhouette, limited internal complexity, and value separation that works in monochrome. The emblem should not rely on literal readable words. If a faction identity is canonically typographic, author the text deliberately with the project’s real font pipeline rather than accepting generated lettering.

Marks should communicate organizational character through geometry, wear, and symbol choice while avoiding generic military-shield repetition. Test each on dark and light-value backgrounds, under selection, disabled/unknown state, and grayscale. Friendly/hostile/unknown status must come from UI state treatment rather than painting permanent green/red allegiance into the base emblem.

## 15. Task 2 coverage accounting and evidence protocol

Capture both numerator and denominator, not only the percentage. A coverage change is only interpretable if the scanner’s eligible identity count is stable or its changes are explained. The before/after evidence should include:

| Metric | Before | After | Delta | Explanation required? |
|---|---:|---:|---:|---|
| Eligible identities | record | record | compute | yes if changed |
| Dedicated loaded | record | record | compute | yes |
| Fallback resolved | record | record | compute | yes |
| Missing/error | record | record | compute | yes |
| Dedicated coverage | 29.91% stated baseline | compute | compute | yes |
| Selected 150 resolving dedicated | 0 baseline for selected unresolved set | compute | compute | yes |

If the denominator changes because the scanner discovers previously uncounted identities, do not massage the report. State that the batch delivered the selected 150 assets but the percentage moved differently because the denominator changed. If multiple IDs legitimately share one dedicated art file under current registry behavior, document that explicitly so file count is not confused with identity coverage.

# Task 3 — Make the six highest-traffic UI surfaces visually cohesive

## 16. Task 3 outcome and implementation philosophy

**Target:** turn the existing functional dashboard UI into a readable, consistent terminal experience with populated-state QA across Main Menu, Shelter HUD, Inventory, Survivors, Map/Expedition, and Faction/Radio.

The task is a shared-system refinement, not six independent reskins. If a header weight, divider style, selected-row treatment, warning panel, metric card, or icon size appears on several screens, its first implementation home is the theme or shared helper/component that already owns that concept. Screen-specific overrides are reserved for genuine layout constraints such as a unique map viewport or shelter HUD composition.

The goal is not to decorate every empty region. Information density should become easier to parse: clear hierarchy, consistent spacing rhythm, predictable action placement, stable focus states, and purposeful use of art. Task 2 imagery fills semantic cells; it should not become wallpaper that lowers readability.

## 17. UI audit dimensions

For each of the six surfaces, compare populated 1920×1080 and 1280×720 captures against the tokens and shared components currently provided by `DesignTheme`, `AshfallDashboardShell`, the sidebar, status rail, and `AshfallUiHelpers`. Record drift under these dimensions:

- **Hierarchy:** title, section header, card title, primary value, metadata, body, hint/help text.
- **Spacing:** outer safe padding, panel gutters, row height, card internal padding, section rhythm, icon-text gap.
- **Contrast:** surface layers, disabled state, selected state, warning/critical state, focus ring, image/text interaction.
- **Chrome:** borders, corner treatment, divider weight, header bars, tabs, rail states, modal/action regions.
- **Typography:** size, weight, uppercase usage, numeric alignment, truncation behavior, multi-line density.
- **Iconography:** standard icon box, baseline alignment, semantic consistency, fallback behavior, resolution at scale.
- **State cues:** available/unavailable, selected, focused, critical, damaged, hostile/friendly/unknown, locked, newly changed.
- **Interaction:** keyboard order, controller order, close/back semantics, wrap behavior, selection persistence.
- **Responsive behavior:** 1280×720 compression, minimum widths, clipping, scroll behavior, map viewport preservation.
- **Art integration:** portrait/icon/vignette/emblem crop, overlay hierarchy, fallback parity, legibility on variable imagery.

This audit becomes the change list. Do not begin by editing the six screens one by one; that approach bakes drift into the solution.

## 18.1 Scope and freeze the six surfaces

Record the exact scene/panel owners and routes for Main Menu, Shelter HUD, Inventory, Survivors, Map/Expedition, and Faction/Radio. If Map and Expedition are separate panels but share a surface family, list both and define which shared tokens they consume. Freeze the scope so lower-traffic screens do not enter merely because a theme change reveals their old styling.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Surface inventory
- Scene/controller owner paths
- Shared-component dependency map

**Acceptance conditions**

- Each surface has a deterministic navigation path for QA
- Out-of-scope screens are listed rather than opportunistically restyled

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.2 Capture representative populated states

Create or select deterministic states that show real density: multiple inventory rows with differing availability/condition; several survivors with statuses; map nodes with available/locked/selected states; factions with different standings; radio content; shelter metrics with at least one warning. Capture 1920×1080 and 1280×720 before changes.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Twelve-plus baseline captures
- Fixture/save identifier per capture
- State coverage notes

**Acceptance conditions**

- Every surface demonstrates more than an empty-state layout
- Resolution differences are visible and reproducible

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.3 Compare against current design tokens

Trace actual runtime values back to `DesignTheme` and helpers. Identify duplicate screen-local colors, margins, border styles, typography sizes, and icon dimensions. Separate legitimate layout exceptions from accidental drift. Prefer evidence such as repeated constants or visibly inconsistent captures.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Token drift matrix
- Local override inventory
- Candidate shared-token changes

**Acceptance conditions**

- Every proposed shared token has at least one clear consumer
- No global token is changed solely to fix one unusual screen without checking collateral impact

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.4 Preserve shared owners and change them first

Implement the terminal treatment in the existing theme/shell/helpers before panel-local edits. Reuse or extend existing component APIs rather than spawning alternative header/card/row implementations. Keep shell, sidebar, status rail, and focus behavior authoritative.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Shared component/token diff
- Consumer list
- Compatibility notes

**Acceptance conditions**

- Multiple target screens improve from one shared change
- No new parallel UI framework is introduced

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.5 Define compact terminal treatment

Formalize headers, metric cards, dividers, selected rows, disabled actions, warnings, icon boxes, focus states, and status chips. The treatment should be compact but not cramped, favoring strong alignment and restrained contrast over decorative noise. Include non-color state cues by design.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Terminal treatment spec
- Token/component mapping
- State visual matrix

**Acceptance conditions**

- Selected vs focused vs disabled vs warning are distinguishable in grayscale
- Text remains readable over every supported surface layer

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.6 Apply shared changes, then minimal local overrides

Roll shared components through the six surfaces. Only after that pass, handle legitimate screen-specific constraints such as map viewport padding, survivor portrait aspect, or shelter HUD density. Local overrides should reference theme tokens whenever possible rather than hard-coded colors/spacing.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Six-surface implementation
- Explicit local-override list and rationale

**Acceptance conditions**

- No duplicated local style block exists where a shared component can express the same state
- 1280×720 remains functional without hidden actions

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.7 Integrate Task 2 art into populated cells

Replace fallback visuals in the representative states with the actual item icons, portraits, location vignettes, and faction marks from Task 2. Preserve data/state hierarchy: art does not cover condition, quantity, injury, standing, travel risk, or action availability.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Populated production-art captures
- Crop/fit rules per family
- Fallback parity check

**Acceptance conditions**

- Cells remain understandable when art is temporarily missing
- State overlays do not make the art identity ambiguous

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.8 Verify input, accessibility, and readability

Exercise keyboard and controller focus from entry to exit. Verify close/back behavior, focus restoration, no focus traps, visible focus on every actionable control, non-color status cues, minimum practical text readability, and contrast across image-backed surfaces. Review grayscale and a color-vision-deficiency simulation if available in the existing toolchain.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Focus-order notes
- A11y self-test results
- Manual grayscale/non-color review

**Acceptance conditions**

- All actionable controls can be reached and exited
- Critical state is not communicated by hue alone

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.9 Add deterministic populated fixtures without erasing empty-state coverage

Where a screen only has an empty snapshot, add a populated deterministic fixture/target instead of replacing the empty baseline. Freeze ordering and state values. Avoid current-date/time, RNG, network, and unseeded content selection. Fixture data should represent plausible game state without needing save migration or unrelated system changes.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- New populated fixture definitions
- Existing empty-state baseline retained where still meaningful
- Determinism notes

**Acceptance conditions**

- Repeated runs produce byte/visually stable state absent rendering nondeterminism
- Fixtures fail loudly when required test data stops resolving

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 18.10 Run snapshots, panel/a11y tests, and human visual review

Execute affected snapshot targets and panel/a11y self-tests. Triage diffs into intentional design changes, bugs, nondeterminism, or unrelated pre-existing differences. Human review approves intentional changes before baseline files are updated. Record resolution/state for every approved capture.

**Implementation discipline**

- Make shared ownership explicit before editing a target panel.
- Compare 1920×1080 and 1280×720 after any spacing or typography change that affects layout.
- Keep interaction semantics stable unless a documented bug is being corrected.
- Validate with production art and with at least one fallback/missing-art state.

**Deliverables**

- Snapshot diff set
- Test results
- Reviewer approval record
- Approved baseline updates

**Acceptance conditions**

- No baseline is updated solely to make CI green
- Both target resolutions receive visual approval

**Rollback rule**

If a shared token improves one screen but materially degrades another consumer, do not paper over the regression with a new local constant. Re-evaluate the token scope or introduce a semantically named variant in the existing theme system.

## 19. Proposed terminal treatment semantics

The precise numeric values must be derived from the current `DesignTheme`; this plan defines relationships and semantics rather than hardcoding unverified constants.

### 19.1 Surface hierarchy

Use a small, stable number of surface levels: base/background; primary panel; nested card/row; active/raised selection; modal or critical interruption. Adjacent layers must separate through value, border, spacing, or texture—not by assigning each screen a different hue. Background art should sit beneath a controlled overlay/scrim when necessary, with the scrim strength defined by semantic context rather than adjusted ad hoc per screenshot.

### 19.2 Headers

Screen title, section title, and card title need distinct but related typography. The terminal feel should come from alignment, restrained uppercase, mono/technical accents where already supported, and compact metadata—not from covering every heading in brackets or fake command syntax. Header bars should reserve stable positions for back/close affordances and context metrics.

### 19.3 Metric cards

Metric cards should expose a label, primary value, optional unit/trend/state icon, and a clear warning treatment. Numeric alignment and spacing should be consistent. Critical metrics require a non-color cue such as icon shape, border weight/pattern, or explicit status text. Avoid mini-cards whose decorative border consumes more area than their data.

### 19.4 Rows and selection

Separate `hover` (if applicable), keyboard/controller `focus`, and committed `selection`. A focused row needs a visible focus cue even when not selected. A selected row should remain recognizable when focus moves to its action buttons or detail pane. Disabled rows/actions should preserve labels and reason context where available rather than disappearing.

### 19.5 Warnings and critical states

Warnings use restrained amber as a state accent, supported by icon/text/border shape. Critical danger may use the project’s existing critical token, but the UI must remain usable under red/green color-vision deficiency. Avoid pulsing/glowing unless already part of the system and justified by urgency; terminal cohesion depends more on hierarchy than animation.

### 19.6 Icon sizing

Define one or a small semantic set of icon boxes: inline/small, row/default, card/feature. Images should fit these boxes predictably using family-specific contain/cover behavior. An item silhouette and a faction emblem usually benefit from containment; location vignette may use cover with safe crop; portraits should use a stable aspect-aware crop.

## 20. Six-surface implementation notes

### 20.1 Main Menu

Use the menu as the cleanest expression of the system: strong title hierarchy, disciplined action stack, visible focus, and environmental presentation that does not compete with interaction. Confirm controller/keyboard default focus and that disabled/unavailable entries state why when appropriate. Avoid overloading the menu with faction/location art simply because new assets exist.

### 20.2 Shelter HUD

The HUD is the densest continuous surface and should prioritize survivability data. Standardize status rail/card spacing, metric urgency, and contextual actions. The new shelter interior art must sit beneath the HUD with controlled contrast in all lighting variants. Ensure cold/dark night imagery does not make already-muted disabled controls disappear.

### 20.3 Inventory

Task 2 item icons should improve scanning while quantity, condition, weight, category, equipped state, and action availability remain primary information. Align icon boxes and text baselines. Distinguish row selection from item condition. Test large inventory counts and long names at 1280×720, including scrolling and focus restoration after an action.

### 20.4 Survivors

Portraits anchor identity, but current condition/status must remain obvious. Use consistent portrait crop and badge placement. Test several simultaneous statuses, long names, and different portrait value ranges. Do not tint the entire portrait so heavily for status that identity is lost; prefer edge/badge/text state cues.

### 20.5 Map / Expedition

Location vignettes and world backdrops must cooperate with route/travel information. Preserve viewport area and action legibility at 1280×720. Selected location, reachable/unreachable, threat, cost, and current player position need distinct non-color cues. The first-week backdrop family should make Map Detail/Expedition feel connected to the broader Map rather than like separate game modes.

### 20.6 Faction / Radio

Faction emblems provide quick identity but must not encode dynamic standing permanently. Standing, hostility, trust, message freshness, and transmission state belong to UI treatment. Radio content should remain text-forward. Verify long message bodies, faction switching, unread/new states, and focus movement between list and detail panes.

# Cross-batch integration, QA, and rollout

## 21. Automated and manual gate ladder

Use the narrowest relevant checks first, then widen. The exact commands should be taken from repository documentation/CI rather than invented in this plan.

**Gate A — static/path integrity.** Verify filenames, dimensions, duplicate stems, invalid transparency, missing expected phase members, and registry search-path compatibility. Run the focused asset orphan sweep and scene lint used by the repository.

**Gate B — component/panel tests.** Run `BackdropArt`/shelter-related self-tests, registry/coverage scanner tests, target panel tests, and accessibility/focus tests. If the repository groups these differently, invoke its canonical target names.

**Gate C — deterministic visual targets.** Run the target snapshots for the six surfaces and first-week panels using the designated deterministic fixtures. Compare before/after and classify each diff.

**Gate D — bounded Godot runtime capture.** Launch the game using the repository’s bounded capture approach and traverse the representative first-week flow. Capture the required day-phase/world and populated UI states at 1920×1080 and 1280×720.

**Gate E — cross-batch smoke.** Verify Main Menu → game load/new-game fixture → Shelter → Inventory → Survivors → Map/Expedition → Faction/Radio → return/close path. Confirm no missing textures, registry exceptions, focus traps, broken back navigation, or phase-switch failures.

**Gate F — report/manifest finalization.** Only after A–E pass, regenerate final coverage artifacts and update placeholder/status/snapshot manifests. Re-run any check whose inputs include those manifests to ensure documentation updates did not introduce mismatch.

## 22. Runtime capture matrix

Use a capture matrix rather than ad hoc screenshots. Minimum evidence:

| Surface | State | Time phase | 1920×1080 | 1280×720 | Key review focus |
|---|---|---|---|---|---|
| Main Menu | standard | n/a | yes | yes | hierarchy, focus, backdrop contrast |
| Shelter HUD | populated + warning | day | yes | yes | metrics, art/UI separation |
| Shelter HUD | populated | dawn | yes | optional if unchanged layout | lighting continuity |
| Shelter HUD | populated | dusk | yes | optional if unchanged layout | amber restraint |
| Shelter HUD | populated | night | yes | yes | dark-phase legibility |
| Inventory | populated, mixed states | current | yes | yes | icon scale, rows, disabled/selected |
| Survivors | 4+ survivors, mixed statuses | current | yes | yes | portrait crop, badges, focus |
| Map | several nodes/states | day | yes | yes | vignette identity, route data |
| Map Detail | selected early location | each supported phase | yes | as needed | backdrop crop/overlays |
| Expedition | representative travel | each supported phase | yes | yes for day/night | foreground contrast |
| Faction | 3+ standings | current | yes | yes | emblem + dynamic standing |
| Radio | message list + detail | current | yes | yes | text hierarchy, identity |

For each image, store deterministic fixture/save ID, build/commit identifier, resolution, phase, and reviewer state. If the repository has an established screenshot naming convention, use it instead of inventing a parallel folder scheme.

## 23. Accessibility acceptance matrix

Accessibility review is not postponed until after visual polish because shared-token decisions can create systemic issues.

| Concern | Required behavior | Manual proof | Automated proof where available |
|---|---|---|---|
| Keyboard focus | every actionable control reachable in logical order | traverse all six surfaces | focus/a11y self-test |
| Controller focus | directional movement predictable, no traps | controller pass | input navigation tests |
| Close/back | returns to expected prior context and restores focus | open/close each detail/action path | panel navigation tests |
| Selected vs focused | visually distinct concepts | move focus away from selection | snapshot/state tests |
| Disabled state | distinguishable without low-contrast disappearance | inspect on art and plain surfaces | contrast/state checks |
| Warning/critical | icon/text/border cue in addition to hue | grayscale review | semantic widget/state tests |
| Friendly/hostile/unknown | not red-vs-green only | grayscale/CVD review | state label/icon assertions |
| Text over imagery | readable in light/dark regions | phase/resolution capture review | contrast tooling if present |
| Icon meaning | not sole carrier of essential unfamiliar meaning | inspect tooltip/label context | component tests |
| Motion | no new unnecessary flashing/pulsing | runtime review | n/a unless existing test |

## 24. Failure taxonomy and response

Treat failures by category so visual work does not get “fixed” through the wrong subsystem.

**Asset path failure:** intended file exists but registry/backdrop code does not load it. Investigate stem normalization, search order, casing, extension support, and phase naming. Do not duplicate the file under multiple guesses without understanding which path is authoritative.

**Semantic mismatch:** an asset loads for the wrong identity. Stop the batch tranche, inspect aliases/normalized IDs, and correct mapping or filename semantics. This is higher severity than a fallback because it lies to the player.

**Art-style mismatch:** technically valid art breaks family cohesion. Remove it from runtime selection and return to candidate/repair stage. Do not “fix” it with a strong global color filter that degrades the rest.

**Contrast/layout failure:** art or shared styling causes unreadability. First determine whether the issue is composition/safe area, overlay strength, token choice, or screen-specific layout. Fix at the lowest correct layer.

**Snapshot drift:** classify intentional design change versus nondeterminism versus bug. Never baseline nondeterminism. Stabilize fixture/rendering first.

**Dirty-tree collision:** a target file contains unrelated pre-existing edits. Preserve the unrelated hunks, document overlap, and if necessary split the work or amend ownership. Do not reset or overwrite.

**Coverage target miss:** if curated 150 assets resolve correctly but coverage remains below expectation because denominator/aliases/scanner behavior differs, report the actual result and causes. Do not inflate coverage with low-value duplicates.

## 25. Review cadence and evidence bundles

Use three formal review bundles plus final sign-off.

**Review Bundle 1 — World language.** Contains Task 1 before captures, reuse audit, one-page art brief, 16-slot mini-spec matrix, selected master family, runtime phase captures, and focused validation results. Approval unlocks final environment family usage in Task 2.

**Review Bundle 2 — Dedicated 150.** Contains baseline coverage report, exposure ranking, frozen manifest, do-not-regenerate audit, four family specs, contact sheets/finals, populated panel proof, and before/after coverage counts. Approval unlocks final UI art-integration review.

**Review Bundle 3 — Shared UI.** Contains six-surface before/after captures at both resolutions, token drift matrix, shared-component diff summary, populated fixtures, accessibility/input results, snapshot diffs, and reviewer decisions on baseline changes.

**Final batch sign-off.** Contains ledger status updates, cross-batch smoke capture, final coverage/placeholder manifests, list of intentionally unresolved asset IDs, known non-blocking issues, and confirmation that unrelated dirty-tree changes were not absorbed.

## 26. Suggested implementation tranche sizes

Large art batches are safer when integrated incrementally.

For Task 1, integrate one master scene family at a time, but do not mark the task complete until all runtime-consumed slots are consistent. For Task 2, process assets in tranches such as 20–30 items, 8–12 portraits, 10–15 locations, and 5–10 faction marks. After each tranche, run a registry resolution sample and open at least one consuming panel. This catches naming/alpha/crop mistakes before they are repeated across 150 files.

For Task 3, sequence by shared layer rather than surface: token audit → headers/dividers → rows/selection/focus → metric/warning states → art boxes → screen-specific layout. After each shared-layer change, smoke all six screens because a theme-level improvement can have non-local consequences.

## 27. Metrics and success measures

Track success in three layers.

**Player exposure metrics:** number of first-week placeholder slots removed; percentage of selected early-session identities with dedicated art; number of high-traffic surfaces using production assets in populated states.

**Visual consistency metrics:** count of eliminated duplicate/local style constants where shared tokens now apply; count of shared components used across the six surfaces; number of snapshot targets with populated fixtures; number of art families that pass contact-sheet consistency review.

**Quality/safety metrics:** new orphan count (target zero); registry semantic mismatch count (zero); focus trap count (zero); color-only critical state count (zero); unresolved snapshot nondeterminism (zero); runtime missing-texture/error count attributable to batch (zero); unrelated worktree files modified (zero).

Coverage percentage is important but should not be the only KPI. The batch succeeds if the player sees materially fewer fallbacks in important contexts, even if scanner denominator changes make the percentage slightly different from the rough 39% projection.

## 28. Explicit non-goals and anti-patterns

- Do not redesign every game screen to match the six target surfaces in this batch.
- Do not convert the project to a new UI toolkit or styling framework.
- Do not add a bespoke runtime “AI asset loader.” Generated assets are ordinary curated game assets once approved.
- Do not keep prompt fragments, provider IDs, or generation seeds in player-facing filenames.
- Do not create multiple copies of the same image under different stems just to satisfy resolution uncertainty.
- Do not apply heavy global LUTs/filters as a substitute for correcting inconsistent source art.
- Do not add readable fake signage/text to scenery.
- Do not use neon green/purple as universal post-nuclear decoration in this first-week family.
- Do not remove existing empty-state snapshots when adding populated-state coverage unless the empty state has ceased to exist by design.
- Do not treat snapshot approval as automatic after a code change.
- Do not fix a registry miss by hardcoding a path in one panel.
- Do not embed faction allegiance in base emblem color if standing is dynamic.
- Do not encode injury, hostility, warning, or disabled state only through hue.
- Do not bulk-format unrelated C# or scene files while making focused visual changes.
- Do not update “placeholder complete” documentation before runtime verification.

## 29. Final sign-off checklist

### Ledger and isolation
- [ ] New sequential batch allocated from the actual current ledger tail.
- [ ] All three tasks entered contiguously before implementation.
- [ ] Foreman package claim set documented.
- [ ] Pre-existing dirty worktree captured and protected.
- [ ] No unrelated cleanup or formatting included.

### Task 1
- [ ] Baseline Map/Map Detail/Expedition/Shelter captures archived.
- [ ] Existing art reuse audit complete.
- [ ] One-page first-week art brief frozen.
- [ ] Twelve surface mini-specs map to real runtime slots.
- [ ] Four shelter lighting mini-specs map to real runtime slots.
- [ ] One style-consistent candidate family selected.
- [ ] Artifacts/pseudo-text corrected.
- [ ] Exact runtime dimensions delivered.
- [ ] Existing phase-switch naming contract preserved.
- [ ] All phases reviewed in real panels.
- [ ] Orphan sweep and scene lint pass.
- [ ] Relevant panel self-tests pass.
- [ ] Bounded Godot capture approved.
- [ ] Placeholder/status manifest updated last.

### Task 2
- [ ] Fresh baseline coverage report preserved.
- [ ] Exposure ranking completed.
- [ ] Frozen batch contains 70 items, 25 portraits, 35 locations, 20 faction marks.
- [ ] Do-not-regenerate audit complete for all 150 IDs.
- [ ] Four family specs frozen.
- [ ] Selected assets generated/repaired in coherent batches.
- [ ] Transparency/crop/resolution/stems normalized.
- [ ] All assets use existing registry search paths.
- [ ] Inventory/Survivors/Map/Factions/detail runtime states verified.
- [ ] Semantic ID-to-asset spot checks pass.
- [ ] Registry artifacts regenerated after installation.
- [ ] Before/after loaded/fallback counts recorded.
- [ ] Actual final coverage percentage recorded.
- [ ] Unresolved remainder retained as ranked next batch.

### Task 3
- [ ] Six target surfaces frozen and owner paths recorded.
- [ ] Populated 1920×1080 and 1280×720 baseline captures taken.
- [ ] Token/chrome drift matrix completed.
- [ ] Shared owners modified before local overrides.
- [ ] Terminal treatment documented and implemented.
- [ ] Task 2 art integrated into populated cells.
- [ ] Keyboard/controller focus validated.
- [ ] Close/back and focus restoration validated.
- [ ] Non-color state cues validated.
- [ ] Deterministic populated fixtures added where needed.
- [ ] Empty-state snapshots retained when still valid.
- [ ] Snapshot and panel/a11y tests pass.
- [ ] Human visual sign-off obtained before baseline replacement.

### Final cross-batch
- [ ] Main Menu → Shelter → Inventory → Survivors → Map/Expedition → Faction/Radio smoke flow passes.
- [ ] No missing-texture or semantic-registry errors attributable to batch.
- [ ] No new orphan assets attributable to batch.
- [ ] All final manifests/status artifacts match runtime truth.
- [ ] Ledger rows moved to complete only after evidence is attached/referenced.

# Appendix A — Asset production manifest templates

## A.1 Task 1 surface/backdrop manifest

Use one row per runtime slot. Keep the actual code-derived filenames; the placeholders below are schema labels, not proposed stems.

| Slot | Code key | Existing stem | Phase | Dimensions | Reuse source | Candidate set | Approved master | UI-safe notes | Runtime result |
|---|---|---|---|---|---|---|---|---|---|
| Surface 01 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 02 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 03 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 04 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 05 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 06 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 07 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 08 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 09 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 10 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 11 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Surface 12 | discover | discover | discover | discover | path/none | IDs | ID | note | pending |
| Shelter light 01 | discover | discover | day/dawn/etc. | 760×420 | master | IDs | ID | anchors | pending |
| Shelter light 02 | discover | discover | day/dawn/etc. | 760×420 | master | IDs | ID | anchors | pending |
| Shelter light 03 | discover | discover | day/dawn/etc. | 760×420 | master | IDs | ID | anchors | pending |
| Shelter light 04 | discover | discover | day/dawn/etc. | 760×420 | master | IDs | ID | anchors | pending |

## A.2 Task 2 selected-ID manifest

Required columns: category; canonical ID; display name; exposure score; evidence references; current fallback reason; exact existing search results; decision (`generate`, `repair`, `derive`, `rewire`); source path; final runtime path; registry resolved path; runtime panel proof; QC state; substitution reason if any.

The manifest should be machine-readable if the repository already uses CSV/JSON/TSV for asset audits, but do not introduce a new runtime dependency on it. It is production evidence, not a game data source.

# Appendix B — Visual review rubric

Score each selected master/family from 1–5 in the categories below. The score does not replace human judgment; it exposes why a visually impressive candidate might still be unsuitable.

| Dimension | 1 — reject | 3 — usable with work | 5 — approve |
|---|---|---|---|
| Style fit | different visual language | mostly aligned | unmistakably same family |
| Material credibility | generic/incorrect | plausible | strongly grounded |
| Composition | conflicts with overlays | manageable | designed around runtime |
| Value hierarchy | noisy/crushed | acceptable | clear focal + quiet zones |
| Palette discipline | neon/overgraded | minor drift | cold/rust/bone/amber balance |
| Artifact cleanliness | obvious AI errors/text | repairable | clean at review zoom |
| Runtime-size readability | loses identity | adequate | clear at actual size |
| Phase continuity | unrelated image | same place loosely | same place, convincing light shift |
| Accessibility interaction | destroys state cues | manageable | supports UI state cues |
| Crop resilience | focal loss | some risk | safe at both target resolutions |

Any score of 1 in artifact cleanliness, semantic identity, or runtime-size readability is an automatic rejection until repaired. For surface backdrops, any phase-continuity score below 3 is also a rejection when the images are supposed to represent the same place.

# Appendix C — Deterministic populated fixture requirements

A populated fixture is part of the test contract. It should be minimal enough to understand and rich enough to expose the visual states this batch changes.

**Inventory fixture:** include at least one stackable consumable, one tool/weapon-equivalent item if present in the legal gameplay schema, one damaged/low-condition item, one disabled/unusable action case, one item with a long localized name, and enough rows to exercise scrolling at 1280×720. The fixture must reference selected Task 2 item IDs where possible.

**Survivor fixture:** include at least four survivors with distinct selected Task 2 portraits and varied statuses that exercise badge placement, warning treatment, and long/short names. Use stable canonical IDs and explicit ordering.

**Map/Expedition fixture:** include several visible locations with at least one selected, one reachable, one unavailable/locked or otherwise restricted, and one with notable risk/cost. Reference selected Task 2 location vignettes and Task 1 backdrops where the real flow consumes them.

**Faction/Radio fixture:** include at least three factions with different dynamic standings, selected Task 2 emblems, and radio entries of short and long body length. Include unread/new and previously read state if the UI supports them.

**Shelter fixture:** expose representative resource/condition metrics and at least one warning without putting the game into a pathological error state. The same shelter geometry/state should be capturable under day/dawn/dusk/night lighting.

Fixtures must not call current time, external network services, random name generation, random loot, or unseeded event systems. If state construction currently requires those systems, introduce a test seam through existing fixture/composition infrastructure rather than hardcoding UI-only fake state in production panels.

# Appendix D — Filename, path, and alpha hygiene

Filename errors are a common source of false “missing art” reports. For each runtime asset:

1. Use the canonical ID-derived stem format already expected by `AssetRegistry` or the existing exact backdrop phase stem.
2. Match repository extension conventions. Do not mix formats merely because one generator exported them by default.
3. Verify case behavior on Linux, because a path that happens to work on a case-insensitive workstation can fail in the target environment.
4. Avoid trailing spaces, Unicode lookalike punctuation, provider suffixes, version labels, and human review notes in runtime filenames.
5. Verify alpha edges against both near-black and pale checker backgrounds. Remove light halos from premultiplied/matted exports.
6. Remove hidden near-transparent canvas noise that can expand bounding boxes or create unexpected texture import behavior.
7. Confirm crop/pivot expectations for any component that assumes centered content.
8. Ensure source masters and rejected candidates do not sit in registry-scanned runtime directories.
9. Re-run duplicate-stem or collision detection after each tranche.
10. Verify that generated metadata does not create import side effects if the engine tracks import settings separately.

# Appendix E — Human visual sign-off protocol

A reviewer should evaluate screenshots in a fixed order to avoid being biased by polished closeups.

First, review the complete 1280×720 screen at normal viewing scale. This is the harshest density/crop test. Second, review 1920×1080 at normal scale. Third, inspect high-resolution crop details for artifacts. Fourth, compare related screens side by side for family cohesion. Fifth, review grayscale or a desaturated preview for state-cue dependence. Finally, toggle between before/after captures to confirm that improved style did not remove useful information.

For phase art, compare dawn/day/dusk/night in a row. Ask: is this clearly the same place? Does the time shift make physical sense? Does any phase create a bright collision behind text? Does the night version still expose enough world structure? Does amber remain an accent rather than an orange wash?

For portraits/icons/emblems/vignettes, review both the contact sheet and actual panel size. A family that is coherent on a contact sheet can still fail when cropped by cards; conversely, subtle texture variation that looks inconsistent at 400% zoom may disappear at runtime and be irrelevant.

The reviewer decision per capture should be `APPROVE`, `APPROVE WITH NON-BLOCKING NOTE`, or `REJECT`. A rejection records the specific cause and expected correction layer: source art, technical normalization, shared token, local layout, or fixture/data problem.

# Appendix F — Handoff package for the next ranked asset batch

Task 2 intentionally leaves unresolved identities. Preserve the selection work so the next batch starts from evidence rather than repeating discovery. The handoff should include the post-batch unresolved report, exposure scores, IDs deferred because of missing canon/reference, assets discovered but not yet repairable, style-family notes, and any registry normalization edge cases found during this batch.

Mark which candidates are likely to become high exposure after the first week, which are quest-specific, which belong to uncommon factions, and which are low-value codex/static content. This makes the next 150 (or another size) a deliberate continuation rather than a random fallback-clearing exercise.

# Appendix G — Suggested ledger entry content

Because the actual sequence numbers must be discovered at implementation time, use the following content structure after substituting the real IDs.

**`[NEXT] First-week backdrop and shelter-art replacement`** — Replace the first-week surface placeholder family and four shelter lighting variants using the existing `BackdropArt`/phase naming contract; exact dimensions; runtime phase capture; orphan/lint/panel gates; update placeholder status only after approval. Depends on new batch preflight.

**`[NEXT+1] High-exposure dedicated asset slice (150 identities)`** — Fresh coverage baseline; exposure-ranked 70 items/25 portraits/35 locations/20 faction marks; reuse audit; four family specs; existing `AssetRegistry` search paths; populated panel proof; regenerate coverage artifacts and retain ranked unresolved remainder. Depends on Task 1 art-language freeze for final family alignment.

**`[NEXT+2] Six-surface shared UI cohesion pass`** — Main Menu, Shelter HUD, Inventory, Survivors, Map/Expedition, Faction/Radio; populated dual-resolution baselines; shared theme/shell/helper changes before local overrides; production art integration; focus/a11y/non-color cues; deterministic populated fixtures; human-approved snapshot updates. Depends on Task 2 production asset tranche for final approval.

All three rows should reference the same batch identifier and dirty-worktree isolation note.

# Appendix H — Completion report template

At the end of implementation, replace placeholders with actual evidence.

## H.1 Batch summary
- Ledger batch/sequence: `<actual>`
- Commits/review IDs: `<actual>`
- Dirty-tree overlaps encountered: `<none/list>`
- New unrelated modifications introduced: `0 expected`

## H.2 Task 1 result
- Surface placeholder slots replaced: `<x/12>`
- Shelter lighting variants replaced: `<x/4>`
- Existing art reused/repaired: `<count>`
- Newly generated masters: `<count>`
- Phase-switch contract changed?: `<no expected / justified change>`
- Orphan/lint/panel tests: `<results>`
- Runtime capture review: `<approval>`

## H.3 Task 2 result
- Eligible identities before: `<count>`
- Dedicated loaded before: `<count>`
- Fallback before: `<count>`
- Coverage before: `<29.91% or reconciled actual>`
- Selected identities delivered: `<x/150>`
- Dedicated loaded after: `<count>`
- Fallback after: `<count>`
- Coverage after: `<actual %>`
- Semantic resolution mismatches remaining: `<0 expected>`
- Ranked unresolved IDs retained: `<count>`

## H.4 Task 3 result
- Target surfaces completed: `<x/6>`
- Populated fixtures added: `<count>`
- Shared token/component changes: `<summary>`
- Local overrides added: `<count + rationale>`
- Snapshot targets reviewed: `<count>`
- New baselines approved by human: `<count>`
- Focus/a11y failures remaining: `<0 expected>`

## H.5 Final known issues
List only real residual issues with owner, severity, and next action. Do not hide incomplete items behind “polish later.” If a target is intentionally deferred, state why it was removed from this batch and ensure ledger/status reflects the deferral.

# Appendix I — End-to-end implementation runbook

This runbook is the recommended execution order for an implementation agent or human developer. It is intentionally explicit so the work can span sessions without losing state.

### Phase 0 — establish control
1. Read the integration ledger and identify the latest completed/planned sequence.
2. Allocate the next three contiguous entries as one batch.
3. Record the worktree dirty-state inventory.
4. Create the foreman package and initial claim set.
5. Verify no claimed path is simultaneously owned by unrelated active work.
6. Create a batch evidence directory only if the repository already has an accepted place for such artifacts; otherwise keep temporary evidence outside tracked runtime assets.
7. Confirm the repository’s canonical commands/targets for asset coverage, orphan scanning, scene lint, panel self-tests, snapshots, a11y checks, and bounded Godot capture.

### Phase 1 — establish the current first-week world
8. Launch the deterministic first-week state.
9. Capture Map at all supported phases.
10. Capture Map Detail at all supported phases.
11. Capture Expedition at all supported phases.
12. Capture Shelter at all supported phases.
13. Repeat key captures at 1280×720.
14. Mark every visible placeholder, crop issue, contrast problem, and style discontinuity.
15. Trace each displayed image back to exact runtime path and selecting code.

### Phase 2 — audit existing environmental art
16. Search the art tree for candidate map/sky/environment work.
17. Search by current filename stems and likely historical aliases.
18. Inspect all plausible images visually.
19. Classify each as reuse, repair, derive, reference, or reject.
20. Document why each genuinely missing master needs new generation.
21. Freeze the one-page art brief.
22. Confirm the brief does not conflict with actual UI contrast needs discovered in Phase 1.

### Phase 3 — specify and produce Task 1
23. Derive the exact 12 surface slots from runtime code.
24. Derive the four shelter lighting slots and continuity anchors.
25. Populate the 16-row mini-spec manifest.
26. Generate two or three candidates for each missing master or produce repaired candidate variants for existing art.
27. Review candidates by family/contact sheet.
28. Select one coherent family.
29. Correct geometry, pseudo-text, and other generation artifacts.
30. Normalize value/palette.
31. Crop/resize to exact runtime dimensions.
32. Verify alpha/format/import expectations.
33. Install one small tranche using existing filename contracts.
34. Run the panel and inspect runtime before installing the rest.
35. Install remaining approved Task 1 assets.
36. Review day/dawn/dusk/night continuity and legibility.
37. Run focused orphan scan, scene lint, panel self-tests, and bounded capture.
38. Update placeholder/status manifest only after approval.
39. Mark Task 1 ledger row complete only when evidence exists.

### Phase 4 — baseline and select Task 2
40. Run the authoritative full asset coverage report.
41. Preserve loaded/fallback/eligible counts and the raw unresolved list.
42. Split unresolved IDs into items, portraits, locations, faction marks, and other categories.
43. Score candidates by player exposure and salience.
44. Inspect gameplay/content references for the top candidates.
45. Freeze 70 item IDs.
46. Freeze 25 portrait IDs.
47. Freeze 35 location IDs.
48. Freeze 20 faction IDs.
49. Verify total equals 150.
50. Perform do-not-regenerate searches for all 150.
51. Reclassify any identity whose real problem is registry resolution rather than missing art.
52. Freeze the four family specs.

### Phase 5 — produce and integrate Task 2
53. Process item assets in manageable material/usage tranches.
54. Process portraits in consistent framing/light tranches.
55. Process location vignettes using Task 1 environmental language.
56. Process faction marks with small-size/grayscale tests.
57. Clean artifacts and pseudo-text.
58. Normalize alpha/crop/resolution/stems.
59. Install the first tranche into existing registry paths.
60. Run registry resolution checks and open consuming panels.
61. Correct collisions/mis-resolutions before continuing.
62. Repeat until all selected identities are installed or explicitly blocked.
63. Build deterministic populated Inventory/Survivor/Map/Faction states.
64. Spot-check canonical ID -> expected file -> observed runtime art.
65. Regenerate registry/coverage artifacts.
66. Compare before/after numerator, denominator, fallbacks, errors, and percentage.
67. Preserve the ranked unresolved remainder for the next batch.
68. Mark Task 2 ledger row complete only after runtime and report evidence agree.

### Phase 6 — audit Task 3
69. Freeze exact owner paths for the six target surfaces.
70. Capture representative populated states at 1920×1080.
71. Capture the same states at 1280×720.
72. Build the hierarchy/spacing/contrast/chrome drift matrix.
73. Trace local values back to current theme/helper owners.
74. Separate accidental drift from legitimate layout exceptions.
75. Draft the compact terminal treatment and non-color state matrix.

### Phase 7 — implement Task 3 shared-first
76. Change shared theme tokens/components first.
77. Smoke all six surfaces after each substantial shared-layer change.
78. Standardize headers and divider semantics.
79. Standardize row selection/focus/disabled semantics.
80. Standardize warning/critical and metric-card semantics.
81. Standardize icon/art box sizing behavior by family.
82. Integrate Task 2 production art in representative populated cells.
83. Add only the screen-local overrides that remain genuinely necessary.
84. Check 1280×720 after each density-sensitive change.
85. Verify Main Menu input entry/default focus.
86. Verify Shelter HUD navigation and art contrast.
87. Verify Inventory scrolling, selection, disabled actions, and item art.
88. Verify Survivors portrait/state treatment.
89. Verify Map/Expedition route/location states and backdrop interaction.
90. Verify Faction/Radio identity versus dynamic-standing treatment.

### Phase 8 — deterministic visual/a11y closure
91. Add populated snapshot fixtures where missing.
92. Preserve meaningful empty-state fixtures.
93. Freeze fixture ordering and remove sources of nondeterminism.
94. Run panel self-tests.
95. Run accessibility/focus tests.
96. Run snapshot targets.
97. Classify every snapshot diff.
98. Correct bugs/nondeterminism before considering baseline updates.
99. Perform human visual review at 1280×720 and 1920×1080.
100. Review grayscale/non-color state cues.
101. Approve intended changes.
102. Update only approved baselines.
103. Re-run snapshot targets after baseline update.

### Phase 9 — final cross-batch closure
104. Run the Main Menu → Shelter → Inventory → Survivors → Map/Expedition → Faction/Radio smoke flow.
105. Repeat key flow at 1280×720.
106. Verify no missing-texture, registry, or scene errors.
107. Re-run focused orphan/scene lint if final asset movement occurred.
108. Re-run final coverage report if any selected asset changed after previous report.
109. Confirm placeholder and registry manifests match runtime truth.
110. Review the final worktree diff against the foreman claim set.
111. Confirm unrelated pre-existing changes remain intact.
112. Complete Task 3 and batch ledger statuses with evidence references.
113. Produce the final completion report using Appendix H.
114. Hand off the ranked unresolved asset list and any non-blocking visual notes to the next batch.

# Appendix J — Risk register

| Risk | Likelihood | Impact | Detection | Mitigation |
|---|---|---|---|---|
| Mixed provider styles in final environmental family | medium | high | contact sheet looks inconsistent | select family-level winner; normalize only after coherent base selection |
| Existing viable art accidentally regenerated | medium | medium | duplicate concepts discovered late | mandatory 150-ID do-not-regenerate audit and Task 1 reuse audit |
| Phase variants depict different geometry | medium | high | side-by-side phase review | derive from shared master where possible; continuity anchors in mini-spec |
| Pseudo-text slips into runtime scenery | medium | medium | zoomed review/runtime | explicit artifact checklist; paint out before install |
| Asset stems collide after normalization | medium | high | wrong semantic art loads | duplicate-stem scan; tranche integration; canonical ID spot checks |
| Linux casing mismatch | medium | high | works elsewhere, fails in target | exact-case path verification on target environment |
| Coverage percentage fails rough 39% target | medium | low/medium | post-report | report numerator/denominator honestly; prioritize exposure over metric gaming |
| Snapshot churn from simultaneous art + layout changes | high if unmanaged | medium | huge diffs | sequence Task 2 art integration before final Task 3 approval; classify diffs |
| Shared token degrades untargeted screens | medium | medium/high | smoke reveals regression | inspect consumers; semantic variants instead of local hardcodes |
| 1280×720 clips actions after compact redesign | medium | high | dual-resolution capture | use target resolution continuously, not at end |
| Focus and selection become visually indistinguishable | medium | high | keyboard/controller pass | separate semantic tokens/cues |
| Color-only warning/standing state persists | medium | high | grayscale review | icons/text/border pattern/labels in addition to hue |
| Unrelated dirty work is overwritten | low/medium | very high | diff mismatch | preflight dirty inventory; no destructive cleanup; claim-set enforcement |
| Generated candidates pollute runtime directories | medium | medium | orphan/registry noise | keep working candidates outside scanned runtime paths |
| Baselines updated to hide regression | medium | high | review audit | human approval required before baseline replacement |
| Faction emblem bakes in dynamic allegiance | medium | medium | standing changes look contradictory | neutral identity mark; dynamic state belongs to UI |
| Portrait state tint obscures identity | medium | medium | runtime populated capture | prefer badges/edges; bounded tinting |
| Night backdrop crushes UI and environment | medium | high | phase runtime capture | maintain structural value separation; tune source/overlay correctly |
| Overdecorated terminal chrome reduces usable space | medium | medium | 1280 capture | compact data-first treatment; avoid decorative frames |
| Scanner report stale after late asset change | medium | medium | counts mismatch | regenerate final report after last runtime asset movement |

# Appendix K — Decision rules for ambiguous cases

**If an existing image is “almost right”:** repair it when the composition and style family are viable. Generate new only when repair cost or identity mismatch exceeds a fresh master.

**If a generated image is beautiful but too detailed behind UI:** reject or recompose it. Runtime fitness outranks portfolio quality.

**If only one phase variant is weak:** repair/derive that phase from the approved master. Do not replace the whole family with a stylistically different source just to solve one weak frame.

**If an asset resolves only after adding a special-case alias:** first determine whether that alias reflects a real canonical naming discrepancy used elsewhere. Put normalization in `AssetRegistry` only when it is a generally correct rule. Do not add one-off mappings for art-production convenience.

**If a global theme change fixes five screens and weakens one:** define a semantic variant in the existing theme if the sixth screen truly represents a different semantic role; otherwise re-evaluate the global value. Avoid raw local constants.

**If a snapshot changes because a selected Task 2 asset replaces a fallback:** that can be intentional, but still review crop, contrast, and semantic correctness before approving the diff.

**If the final coverage exceeds 39% substantially:** report it; do not remove valid art to match the estimate. Conversely, if it reaches only 37–38% despite all 150 being valid, report the denominator/alias reason rather than generating low-value filler.

**If the first-week art suggests lore not supported by canon:** remove or neutralize the unsupported narrative detail. Environmental storytelling should reinforce known worldbuilding rather than silently create new faction symbols, dates, place names, or technologies.

# Appendix L — Quality bar in one page

A finished first-week world image is technically correct, style-consistent, artifact-clean, readable behind UI, and clearly part of the same place/time family. A finished dedicated identity asset is semantically correct, recognizable at runtime size, cleanly normalized, and resolved by the existing registry. A finished UI surface uses shared tokens/components, presents production art without hiding state, behaves correctly with keyboard/controller, and remains understandable without relying on color alone.

The implementation is rejected if it “looks more polished” but introduces registry special cases, new local UI styling silos, unreadable night phases, misleading identity art, uncontrolled snapshot baseline churn, or edits unrelated dirty-tree work. The batch is integration work, not a screenshot beautification exercise.
