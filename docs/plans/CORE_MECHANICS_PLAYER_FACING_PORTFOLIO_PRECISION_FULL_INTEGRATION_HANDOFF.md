# ASHFALL TWO-PLAN PORTFOLIO — PRECISION PASS + FULL-INTEGRATION HANDOFF

**Document type:** coordination and implementation handoff; this is not a third subject plan
**Evidence date:** 2026-09-25 working-tree pass
**Repository branch:** `integration/all-latest-2026-09-24`
**Current HEAD inspected:** `b1a8d08d10210253b1b64cef3bd52cffede06758`
**Authority read:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` (read-only reference)
**Production code changed by this handoff:** none
**Ledger/ownership changed by this handoff:** none

## 0. Executive contract

The requested work is already represented by two large, evidence-dense planning artifacts. This handoff does not manufacture a third competing plan and does not repeat their wave prose. It records the precision pass, the current integration truth, the remaining blockades, and the exact safe route to a real full-integration closeout.

The two canonical planning surfaces are:

1. `docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md` — Core mechanics gap-seal program `CORE-MECH-2026-09-25`. Current size: **296,357 characters**. It covers the missing pressure → consequence → counterplay links: foodborne illness, Year-of-Ash dose and winter pressure, vehicle breakdown consequences, defense legibility, migration encounters, quest reopening, gossip, belief/stance, rites/Reckoning, the one-shot trigger primitive, and balance-baseline closure.
2. `docs/plans/PLAYER_FACING_GAMEPLAY_LOOPS_MASTER_INTEGRATION_PLAN.md` — Player-facing gameplay-loops program `PFGL-MASTER-2026-09-25`. Current size: **211,799 characters**. It covers the host-complete/player-thin and true-orphan surfaces: romance, recruitment, vehicle customization, trade/colony, governance/cooking/disaster, legacy/ideology/NPC memory, difficulty/maintenance/keepsakes, chronic-condition decision handling, survivor routines, and sleep-acoustic recovery.

Both exceed the requested **125,000-character minimum per plan**. The Core plan exceeds the **250,000-character soft polish goal**. The player-facing plan is intentionally not padded to reach 250,000: the current precision pass found live drift and missing integration evidence, and manufacturing another 38,000 characters would violate the master authority's anti-padding rule. The current size is a measured artifact size, not a quality score.

The user's 500,000 maximum is treated as a **per-plan hard ceiling** in this handoff. If it was intended as a combined portfolio ceiling, the two existing files total **508,156 characters**, so a foreman-approved trim of at least 8,156 characters is required. No trim is performed here because both files are active planning surfaces and their owners must decide which historical appendix is redundant. The correct action is precision pruning, not new prose.

The portfolio is not yet a full-integration acceptance. Current evidence says:

- Core W1–W10 are recorded as DONE/SEALED in `WORKTREE_OWNERSHIP.md`; Core W12 is still a planned balance-baseline closeout, not a closed ledger package.
- The player-facing plan has an active Phase A claim. Three board routes are reported implemented, but the active owner reports a host build blocker and has not claimed the five remaining Core-only host/save integrations.
- `INTEGRATION_PLANS.md` contains no current row for either portfolio program. A plan file, a CLI probe, or a closeout appendix is not a substitute for the foreman's ledger acceptance.
- The worktree is heavily dirty and contains concurrent agent work. No shared host path is edited by this handoff.

## 1. Objective and bounded outcome

### 1.1 Objective

Finish the two-plan portfolio without creating duplicate gameplay authorities:

- preserve the current Core owners and sealed W1–W10 consequences;
- run the missing Core W12 balance baseline honestly;
- turn the player-facing plan's live, host-complete systems into truthful operable surfaces one wave at a time;
- resolve every technical blockade with a minimal owner-first change or an explicit no-op/deferred disposition;
- keep all state, save, RNG, event, and UI paths aligned with current repository contracts;
- produce a final precision certificate that distinguishes planned, claimed, implemented, sealed, and accepted states.

### 1.2 Non-goals

- No Unity work, Unity dependency, or restoration of retired Unity architecture.
- No new parallel gameplay system, save store, flag ledger, scheduler, rumor network, governance engine, or UI-side authority.
- No broad refactor of `Main`, no mass formatting, and no hand editing of generated indexes or manifests.
- No full-suite run by default. Focused tests only, with a new test file run alone first.
- No decision-blocked item is opened by implication. DP-01, DP-02, DP-03, DP-04, DP-05, the quarantine drain, XP-04/XP-06, EN-01…EN-08, and string-freeze work remain governed by their named signatures.
- No retuning is smuggled into W12. W12 measures and files proposals; it does not change balance numbers.
- No implementation is claimed by this document. Production work requires a current owner, exact path claim, and a passing pre-integration readiness record.

## 2. Current reality and evidence labels

The labels below are deliberate. `[VERIFIED NOW]` means observed in the current working tree during this pass. `[OWNERSHIP]` means recorded in `WORKTREE_OWNERSHIP.md`. `[PLAN]` means a proposal in one of the two canonical plan files, not a live API guarantee. `[PROPOSED]` means a future path or action that must be claimed and rechecked. `[BLOCKED]` means the repository explicitly requires a decision or a competing owner.

### 2.1 Architecture and persistence

- `[VERIFIED NOW]` `Assets/Ashfall.Core/` remains the engine-neutral Core layer; `src/` remains the Godot host/presentation layer; `Assets/StreamingAssets/Data/` remains the authored JSON authority.
- `[VERIFIED NOW]` `SaveSectionRegistry.All` is still pinned to 266 in `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` at the two current assertions.
- `[VERIFIED NOW]` Core W1–W10 ownership rows state that no new save section was added and the pin remained 266. This is a current claim/evidence fact, not permission for another package to add a section.
- `[OWNERSHIP]` the active Core claims explicitly exclude the player-facing, Triad-B, combat, and most UI hubs. Those exclusions are architectural boundaries, not invitations to bypass ownership.

### 2.2 Current surface and generated evidence

- `[VERIFIED NOW]` `docs/player_surface_manifest.json` reports **222 total surfaces, 60 interactive, 162 read-only**. Both large plans contain older snapshots of 219/58/161. Those older numbers remain valid historical evidence at their old HEAD, but they are not current acceptance numbers.
- `[OWNERSHIP]` the active PFGL claim says its Phase A board routes and generated manifest are current at 266 architecture subsystems, but the host acceptance is blocked by dirty navigation compilation. The manifest count must be re-read after the owner stabilizes the worktree; do not infer that all 60 interactive routes are accepted.
- `[VERIFIED NOW]` `git rev-parse HEAD` is `b1a8d08d10210253b1b64cef3bd52cffede06758`. Both plan headers stamp the older `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1`. Every current-tense claim in both plans must therefore be treated as stale until its P0 recheck is recorded.
- `[VERIFIED NOW]` the two plan files are untracked in this worktree. They are present planning artifacts, not committed authority. The foreman must index, accept, or archive them explicitly.
- `[VERIFIED NOW]` `INTEGRATION_PLANS.md` has no `CORE-MECH` or `PFGL-MASTER` row. The live queue and ownership ledger, not either plan's draft status table, determine whether a package is accepted.

### 2.3 Core program state

- `[OWNERSHIP]` W1 foodborne disease bridge: DONE/SEALED at MECHANIC tier. The actual implementation is routed through the existing disease authority and a thin food-consumption seam; no UI claim was taken.
- `[OWNERSHIP]` W2 dose storm window: DONE/SEALED. The single dose-conditioning site and shared fallout provider are recorded as landed.
- `[OWNERSHIP]` W3 winter pressure: DONE/SEALED for the water consumer. The power consumer remains deliberately deferred as W3b because the mature power tick has no external demand seam.
- `[OWNERSHIP]` W4 breakdown consequences: DONE/SEALED. Injury, exposure, contamination, journal facts, and the exactly-once guard are recorded as landed.
- `[OWNERSHIP]` W5 defense/siege coupling: DONE/SEALED as a legibility and power-truth repair. The real `DefenseSystem.ResolvePreCombatRaid` resolver was found; the plan's premise that defense was entirely unwired was corrected.
- `[OWNERSHIP]` W6 migration/encounter bridge: DONE/SEALED. The focused test caught and fixed a uniform-multiplier design flaw before integration.
- `[OWNERSHIP]` W7 quest reopening: DONE/SEALED at MECHANIC tier. The clone/capture persistence hole was fixed and tested.
- `[OWNERSHIP]` W8 gossip propagation and W11 one-shot primitive: DONE/SEALED together. W11 was built inline to remove the ordering blockade rather than deferred.
- `[OWNERSHIP]` W9 belief/stance bridge: DONE/SEALED. The bridge proposes bounded movement; `FactionStanceEngine.ModifyTrust` remains the sole standing writer.
- `[OWNERSHIP]` W10 rites/Reckoning: DONE/SEALED. The plan's assumed typed evidence vocabulary did not exist; the safe solution was a distinct `riteTraceTotal` trace in the existing Reckoning state, not machine-log evidence.
- `[PLAN]` W12 is the remaining Core package: deterministic paired balance runs, a report, and separate signed retune targets. No W12 ownership row or ledger closeout was found in the current evidence.

### 2.4 Player-facing program state

- `[OWNERSHIP]` the active PFGL claim is `PFGL-CODEX-LUNA6-OCTET`, not a blanket authorization for every PFGL wave. It explicitly excludes `src/Main.CampaignOwners.cs` while another claim owns that path and says five Core-only host/save integrations are not yet claimed.
- `[OWNERSHIP]` the active owner reports three Phase A board routes implemented, `PfglOctetBoardRouteTests` 2/2, and `PlayerSurfaceCoverageGateTests` 8/8, but host acceptance is blocked by an unresolved `Ashfall.Core.WeatherKind` reference in dirty `src/Main.PlayerSurfaces.cs` and an earlier active-C1 error in `src/Main.Expeditions.cs`.
- `[VERIFIED NOW]` the dirty navigation file contains references to `Ashfall.Core.WeatherKind` and `Ashfall.Core.World.WeatherSystem?`. This is evidence of a current compile/premise conflict, not permission to add a compatibility alias or edit the file from this handoff.
- `[PLAN]` PFGL W1–W10 are contracts, not accepted implementation. The plan itself says CLI-complete is not player-operable and a provider is not a board.
- `[VERIFIED NOW]` some paths named in the PFGL plan are correctly marked CREATE for future work, while others are stale path forms. Proposed files such as `RecruitmentHostSession.cs`, `ChronicConditionHostSession.cs`, and new panel classes are not current owners and must not be cited as live evidence.

## 3. Precision-pass findings and required corrections

The following are the concrete changes required before the next implementation session. They are deliberately narrow; historical appendices are not rewritten merely because they mention old numbers.

### P-01 — Evidence-head drift

Both plan headers use `1678c074…`; current HEAD is `b1a8d08d…`. The next owner must either:

1. add a current evidence-head addendum to the relevant plan, preserving the old stamp as historical; or
2. create a separate foreman-owned rebaseline record that points to the plan and lists the current HEAD.

Do not silently replace historical evidence. Every current-tense path, pin, verb, and count must be marked `RECHECK AT CURRENT HEAD` until verified.

### P-02 — Player-surface count drift

The plans' 219/58/161 snapshot is stale relative to the current 222/60/162 manifest. The precision correction is:

- retain 219/58/161 as the historical R4/R5 snapshot;
- record 222/60/162 as the current generated artifact;
- do not infer that the three new interactive routes are accepted until the active owner resolves the host build and runs the owning route/lifecycle gates;
- regenerate `docs/player_surface_manifest.json` through its owner/generator, never by hand.

### P-03 — Core W12 status ambiguity

The Core plan contains W12 as a required future wave, draft ledger rows, historical appendices, and text that can be read as a closeout template. The ownership ledger records W1–W10 only. The precise status is therefore:

`W12 = PLANNED / NOT ACCEPTED / NOT EXECUTED IN CURRENT EVIDENCE`.

The integrator must not mark the Core program complete until a W12 report exists, its harness inputs are identified, the paired runs are reproducible, and the foreman adds the ledger row.

### P-04 — Test-path drift in the Core plan

The Core plan's original test matrix contains several paths that do not exist under the names currently used by the landed focused tests. The following corrections are required in the next plan revision or a current evidence appendix:

- Foodborne: `Ashfall.Core.Tests/Disease/FoodborneExposureBridgeTests.cs`, not the earlier `Shelter/` path.
- Defense: `Ashfall.Core.Tests/Defense/DefenseProjectionTruthTests.cs`, not the earlier proposed `DefenseSiegeCouplingTests.cs` name.
- Migration: `Ashfall.Core.Tests/Expeditions/MigrationEncounterBridgeTests.cs`, not the earlier `Ecology/` path.
- Gossip: `Ashfall.Core.Tests/MoralChoice/GossipPropagationTests.cs`, not the earlier `Narrative/` path.
- Rites: `Ashfall.Core.Tests/Verdict/RitesReckoningEvidenceTests.cs`, not the earlier `Endgame/RitesReckoningTests.cs` name.
- Seasonal pressure remains `Ashfall.Core.Tests/Shelter/SeasonalPressureProviderTests.cs`.
- Quest reopening remains `Ashfall.Core.Tests/Quests/QuestReopenGrammarTests.cs`.
- One-shot remains `Ashfall.Core.Tests/Flags/OneShotTriggerLedgerTests.cs`.
- The census reference contains the typo `UNCLAIMED_CORUS_CENSUS.md`; the live candidate is `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`. Correct the reference, not the historical source.

These are path corrections, not permission to create duplicate tests. Existing focused tests are reused.

### P-05 — PFGL proposed-versus-live path classification

A missing path in a PFGL plan is not automatically a defect. It must be classified as one of:

- `LIVE`: the path exists and the cited public API was read;
- `MOVE_CORRECTION`: the owner exists under another current path;
- `CREATE_PROPOSED`: the plan explicitly owns a future file and no live file is claimed;
- `STALE_DECISION`: the path was part of a rejected or retired design and must be removed;
- `CONFLICT`: another active claim owns the path.

The current path scan found proposed files such as `src/Host/RecruitmentHostSession.cs`, `src/Main.Recruitment.cs`, `src/UI/RecruitmentPanel.cs`, `src/UI/SleepAcousticPanel.cs`, and `src/UI/SurvivorRoutinesBoardPanel.cs`. They are not current evidence. The active owner must claim only the files needed for the selected wave and label the rest `CREATE_PROPOSED`.

The canonical registry path is `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`. Any PFGL reference to `src/UI/PanelRegistryBootstrap.cs` is a path correction, not a second registry.

### P-06 — Status must distinguish plan, claim, code, and acceptance

Use these exact states in every handoff:

- `PROPOSED`: described in a plan, no claim.
- `CLAIMED`: exact paths reserved by an owner.
- `IMPLEMENTED`: code exists in the working tree.
- `FOCUSED-VERIFIED`: the smallest required checks pass.
- `SEALED`: the owning integrator accepts the package contract.
- `LEDGER-ACCEPTED`: the foreman has added the live ledger row.
- `BLOCKED`: a named decision, claim, API premise, or environment gate prevents progress.
- `DEFERRED`: intentionally not in this package, with a recheck condition.

Never use `INTEGRATED` for a plan-only artifact. Never use `COMPLETE` for W12 until its balance report and closeout exist.

### P-07 — Shared-hub ownership is an integration blocker, not a defect to route around

The following paths are shared composition seams and must be serialized:

- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
- `Assets/Ashfall.Core/HostCliRegistry.cs`
- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`
- `src/Main.CampaignOwners.cs`
- `src/Main.SaveOrchestrator.cs`
- `src/Main.Lifecycle.cs`
- `src/Main.Application.cs`
- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`
- `src/Main.GameFlow.cs`
- `src/Main.PlayerSurfaces.cs`
- `src/Main.PanelLifecycle.cs`
- `docs/player_surface_manifest.json`
- `scripts/ci/generate-architecture-map.py`
- `docs/architecture/ARCHITECTURE_TEST_MAP.md`
- `docs/ci/SELFTEST_MANIFEST.json`
- `docs/INDEX.md` and other generated indexes.

A package that needs a shared hub waits for the current owner or transfers the claim through the foreman. It does not create a second route, a second registry entry, or a second lifecycle participant to avoid waiting.

### P-08 — Generated artifacts are outputs, not hand-edited authorities

The current worktree has dirty generated and documentation files. The only safe flow is:

1. identify the owning generator or claim;
2. make the smallest source change;
3. run the generator;
4. run its `--check` mode;
5. attach the output path and result to the handoff.

If a generated file is dirty because another agent is writing it, mark the check `BLOCKED BY CONCURRENT OWNER`; do not overwrite it.

## 4. Authority and ownership map for the remaining work

### 4.1 Core mechanics owners

- Spoilage truth: `FoodPreservationSystem`; illness truth: `DiseaseSystem`; the food-consumption seam translates one meal into one exposure attempt.
- Radiation truth: `DoseLedgerSystem`; seasonal context: the existing Year-of-Ash catalog and `FalloutWindowProvider`; conditioning occurs once at the host booking seam.
- Winter pressure: the shared seasonal provider; water consumer is landed; power consumer is a separate W3b premise because `PowerGridSystem.TickDay` currently has no external demand seam.
- Expedition risk and repair: `ExpeditionVehicleSystem`; injury, dose, and contamination consequences route into their existing medical/dose/disease owners.
- Defense projection and siege: `DefenseSystem.ResolvePreCombatRaid` remains the sole resolver; the defense panel is a truthful projection, not a second combat model.
- Migration/encounter selection: `TravelEncounterSystem.GetEffectiveWeight` remains the sole selection seam; no second weighted picker.
- Quest lifecycle: `QuestRuntimeCoordinator`; reopen state rides the existing procedural narrative save family.
- Gossip: the existing `RumorSystem`; `OneShotTriggerLedger` is the reusable persisted one-shot primitive and does not own rumor truth.
- Belief/stance: `FactionStanceEngine.ModifyTrust` is the only standing write.
- Rites/Reckoning: `SpiritualMeaningCoordinator.OnMemorialRitePerformed` is the canonical event; `ReckoningSystem.riteTraceTotal` is a distinct trace, not machine evidence.
- W12 balance: an existing deterministic harness or a foreman-approved focused harness; no gameplay tuning authority is created by the report.

### 4.2 Player-facing owners

- Romance: `RomanceFamilySystem` and `RomanceFamilyHostSession`; the board must call live verbs, not invented `BeginCourtship`/`AdvanceRomance` names.
- Recruitment: `RecruitmentSystem` is the Core authority; any host session/save store remains an adapter around it.
- Vehicle customization: `VehicleCustomizationSystem` plus the existing garage seam; Core W4's vehicle consequence work must remain separate from PFGL's module surface.
- Trade/colony: `PlayerTradeRouteSystem`, `TradeRouteHostSession`, `ColonySystem`, and the existing outpost/settlement host path; no apiary-colony collision and no invented `Abandon` verb.
- Governance: `ShelterGovernanceEngine` is the proposed authority, but the existing `PoliticsSystem`/UI mismatch and `DEC-GOV` decision must be resolved before a live write surface is claimed.
- Cooking/disaster: use `CookingSystem` and `DisasterResponseSystem`; do not borrow `ResolveIncident` from `AirlockSecuritySystem` or let a kitchen nutrition panel silently become the cooking authority.
- Legacy/meta: `CampaignLegacySystem` and `MetaProgressionSystem` remain separate owners; no parallel New Game+ store.
- Ideology/NPC: `IdeologicalFrictionEvents` and `NpcMemorySystem`; social consequences route through existing relationship/stance owners.
- Difficulty: `DifficultySettingsSystem` and the active XP authority; PFGL must not create another scalar registry.
- Maintenance: `ShelterMaintenanceSystem`; use `PerformMaintenance`, `GetWarningComponents`, and `GetFailedComponents`, not invented `ScheduleRepair`/`ClearAlert` verbs.
- Chronic conditions: `ChronicConditionSystem` only after the named decision; if the decision rejects wiring, retire the proposal rather than leaving a fake host.
- Routines: `SurvivorRoutineSystem`; the host/save/day owner already exist; add an interactive command surface without creating a second schedule or satisfaction store.
- Sleep acoustics: `SleepAcousticLedger` plus `ShelterAtmosphere`; `ShelterNoiseSystem` is the quiet-hours write authority, while Sleep Acoustic reads/evaluates the synchronized state. The result must reach `NeedsSystem` with the repository's verified polarity.

## 5. Full-integration architecture

The integration is a sequence of narrow owner-to-owner edges. The following is the canonical data flow for any selected wave:

`player input or campaign event → Main/host command → existing host session → Core owner → typed domain fact → existing receiver/event → existing save owner → read-only projection/UI → journal/feedback`

No panel may calculate a probability, cost, conflict result, or consequence that the Core owner already owns. No Core module may import Godot. No host-only cache may become the source of truth for a later day tick.

### 5.1 State and save contract

For each selected wave, the owner must answer:

- Which existing state fields are read?
- Which existing state fields are written?
- Does the state survive capture/restore, including mid-event save points?
- Is the state already carried by an existing section, or is a codec migration genuinely required?
- What is the old-save default?
- What is the exactly-once key for a consequence?
- What happens when a referenced catalog row, owner, or receiver is absent?

The default answer for the Core program remains “reuse the existing section.” A new section is an escalation, not a convenience. If a new section is truly required, the claim must include the registry row, filename, codec migration, checksum behavior, comprehensive save test, and a foreman decision before code lands.

### 5.2 Determinism contract

- Use the existing seeded campaign stream or a named fork.
- Never use `System.Random`, wall-clock time, hash iteration order, or UI order to decide gameplay.
- Sort candidate ids with `StringComparer.Ordinal` before weighted selection or replay-sensitive iteration.
- Name the stream at the call site in a comment or handoff.
- A new consequence gets a two-pass same-seed replay and a continuous-versus-mid-reload assertion when it has a one-shot or delayed effect.
- Missing catalogs fail closed to the documented neutral behavior; they do not silently invent a favorable outcome.

### 5.3 UI and accessibility contract

A surface is not complete merely because its class exists. The minimum feature seal is:

- an interactive route is registered through the canonical registry and surface manifest, or an existing surface is truthfully extended;
- each control calls a live owner verb;
- the surface reads a Core/read-model value and does not recompute gameplay;
- state refreshes after command, day tick, load, and relevant event;
- close/back and controller focus behavior match neighboring panels;
- status is text plus optional icon/color, never color alone;
- subscriptions and transient nodes are released on close;
- an ordinary player session can observe cause and effect.

A CLI probe is evidence. It is not a substitute for a panel, a route, a mutation test, or a manual player script.

## 6. Dependency-ordered full-integration sequence

The following is the safe serial order. It is a handoff recommendation, not an authorization to edit the listed paths.

### Phase 0 — Evidence and ownership freeze

1. Re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, and `AI_AGENT_WORKFLOW.md`.
2. Record current HEAD, save pin, manifest counts, and the exact active claims.
3. Re-probe every current-tense API cited by the selected wave.
4. Mark missing proposed files `CREATE_PROPOSED`, not `LIVE`.
5. Do not touch the active PFGL dirty navigation, expedition, or campaign-owner paths.

**Exit gate:** the selected wave has a current evidence card, exact claim list, owner decision status, and a focused baseline.

### Phase 1 — Stabilize the active PFGL Phase A owner

The active owner, not this handoff, must resolve the `WeatherKind` navigation compile issue and any active-C1 expedition error. The owner should run the smallest host build and the already named focused route tests, then regenerate the manifest through its owner.

**Exit gate:** host build is attributable to the active claim, the route/lifecycle checks pass, the manifest is generated and current, and the claim is either accepted or explicitly handed off. Until then, no second PFGL wave may claim `Main.PlayerSurfaces`, `PanelLifecycle`, `GameFlow`, or the registry.

### Phase 2 — Core W12 balance closure, at the correct baseline point

W12 is safe to run when the current code base is stable and no unmeasured balance-affecting PFGL wave is in flight. If PFGL W7 changes difficulty scalars or any other gameplay consumer, W12 must run after that change or the report must be explicitly labeled core-only. Do not compare a post-W7 campaign to a pre-W7 baseline without saying so.

W12's first task is to identify the existing deterministic harness and its seed/manifest contract. The current scan found existing balance reports and focused simulation tests, but no obvious single dedicated core-mechanics W12 entry point. That absence is a premise to resolve, not a license to invent a new harness.

The report must include:

- exact source commit/working-tree state;
- seed set and campaign duration;
- initial cohort, difficulty, weather, food, dose, vehicle, and route fixtures;
- metrics for illness exposure, dose bands, winter water draw, breakdown outcomes, and any downstream effects;
- paired-run fingerprints;
- direction and magnitude deltas;
- separate, signed retune proposals only;
- no production-code diff in W12.

**Exit gate:** report is published, reproducible, and attached to a new foreman/integrator ledger row. If no harness exists, the package stops at a bounded harness-design decision rather than creating an unreviewed simulator.

### Phase 3 — PFGL reliability-first wave

After Phase 1, choose one of the following only after its claim is free:

- PFGL W10 sleep-acoustic recovery, because it closes a known silent result-discard defect;
- PFGL W9 routines, because its Core/host/save path already exists and the missing link is player operability;
- PFGL W1 romance, if the social board is the foreman's chosen product priority.

Do not start all three. They touch overlapping survivor detail, panel, and lifecycle surfaces.

### Phase 4 — PFGL true-orphan and host-complete waves

After the reliability wave is accepted, sequence the remaining packages by dependency and claim availability:

1. W2 recruitment host/save integration, only after the overlapping survivor/campaign-owner claim is transferred or released;
2. W3 vehicle customization, after Core W4 vehicle consequences are sealed and the current garage owner is re-probed;
3. W4 trade/colony, with the existing outpost/settlement owner and live verbs;
4. W5 cooking/disaster as a separable subpackage, with governance held behind its decision;
5. W6 legacy/ideology/NPC memory, after social and relationship seams are stable;
6. W7 difficulty/maintenance/content, coordinated with the active XP authority and shared UI hubs;
7. W8 chronic only after its named decision; otherwise record a no-op/retirement.

### Phase 5 — Optional Core satellites

Only after the main portfolio is stable:

- W3b power winter pressure may proceed if a live external demand seam is found and the power owner is claimed. The minimum change is a read-only provider boundary and one consumer application with a regression test. If no seam exists, record W3b as deferred; do not rewrite mature fuel math.
- W5b sky-armor/orbital coupling may proceed only if the current telemetry owner and a real consumer are proven. A catalog mention or a UI label is not enough. If not proven, retain the named deferral.
- Any other master-authority seed requires a fresh premise sweep and a new bounded plan. Do not expand the two canonical plans merely to consume every seed.

### Phase 6 — Final precision and acceptance pass

Re-run the evidence sweep after all selected waves. Update the two plan headers or addenda with current HEAD, current pins, current manifest counts, and actual status. Correct only stale current-tense claims; preserve historical snapshots. Run the owning generators' `--check` modes. Prepare a ledger handoff for the foreman, but do not edit the ledger from a builder session.

## 7. Blockade resolution register

Each item below has a concrete resolution and a stop condition. A blockade is not “solved” by writing more plan prose.

### B-01 — Active PFGL shared-path collision

**Evidence:** the active PFGL claim owns `Main.PlayerSurfaces.cs`, `Main.PanelLifecycle.cs`, `PanelRegistryBootstrap`, and the generated manifest; another active claim owns overlapping expedition/campaign seams.

**Resolution:** the active owner either closes Phase A or transfers the exact paths through the foreman. The next PFGL wave receives only disjoint owner/session/test files. If the build remains red, the next wave is `BLOCKED BY ACTIVE OWNER`, not allowed to add a fallback route.

**Stop condition:** current host build and route/lifecycle checks are attributable to the owning package.

### B-02 — `WeatherKind` namespace/compile drift

**Evidence:** dirty `src/Main.PlayerSurfaces.cs` contains `Ashfall.Core.WeatherKind` and `Ashfall.Core.World.WeatherSystem?` references while the active owner reports an unresolved `WeatherKind` error.

**Resolution:** the active owner reads the current Core weather type and updates the reference to the live namespace/type. No compatibility alias, duplicate weather enum, or new Core dependency is permitted. Re-run the smallest host build and the route test.

**Stop condition:** compile error is gone for the claimed reason and no unrelated path was rewritten.

### B-03 — Recruitment is a true Core orphan

**Evidence:** `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs` exists, but the PFGL plan's host/session/save/UI files are proposed and the active C1 claim overlaps survivor/campaign seams.

**Resolution:** claim a thin `RecruitmentHostSession` and save adapter only after transfer of the relevant lifecycle/save seams. Reuse the Core system's existing state and verbs. Add only the interactive surface after host/save/day owner agreement. If the claim cannot be transferred, record W2 as blocked and do not implement a board over an unhosted system.

**Stop condition:** real campaign setup, save/restore, day tick, player command, and focused host wiring all agree.

### B-04 — Vehicle customization versus Core vehicle consequences

**Evidence:** Core W4 is sealed on `ExpeditionVehicleSystem`; PFGL W3 targets vehicle customization and the garage seam.

**Resolution:** sequence PFGL W3 after W4 is accepted. Read the current `VehicleCustomizationSystem` and `VehicleGarageSystem` APIs. Do not edit breakdown consequence math, vehicle save shape, or armor tuning from the customization board. A new module must use the existing garage command seam.

**Stop condition:** no overlap in claim or diff; module install/remove changes only the customization owner and its intended garage projection.

### B-05 — Governance decision and wrong-bound politics surface

**Evidence:** the PFGL plan records a `PoliticsSystem` UI mismatch and a `DEC-GOV` decision gate around `ShelterGovernanceEngine`.

**Resolution:** split the current bundled W5 into W5A governance and W5B cooking/disaster. W5A remains blocked until the named decision selects the write authority. W5B may proceed separately if its owners and claims are free. The UI must bind to the selected owner, not display a read model while writing another system.

**Stop condition:** signed decision or explicit no-op; no dual elections, dual write paths, or fake political surface.

### B-06 — Chronic-condition decision gate

**Evidence:** `ChronicConditionSystem` exists in Core, but the PFGL plan explicitly marks W8 as decision-gated.

**Resolution:** prepare a one-page decision packet describing the current owner, save impact, capability semantics, and alternatives. Do not create a host/session/panel before the decision. If the decision rejects live wiring, close the wave as a documented non-adoption and update the queue through the foreman; do not leave a permanently “partial” feature.

**Stop condition:** signed decision and a truthful implementation or retirement disposition.

### B-07 — Difficulty and maintenance shared-hub pressure

**Evidence:** the active XP program owns difficulty authority; PFGL W7 proposes a runtime panel and maintenance board; the current debt row names shared route seams.

**Resolution:** PFGL may extend the existing difficulty owner only after reading the current XP package and active consumer list. Use the existing difficulty save/section. Schedule any new route as one integrator-owned hub burst. Maintenance remains a separate owner; do not make difficulty panel callbacks perform maintenance.

**Stop condition:** one difficulty authority, one maintenance authority, current manifest counts, focused route tests, and a manual difficulty/lock script.

### B-08 — Routines satisfaction port and polarity

**Evidence:** `SurvivorRoutineSystem` has live `DetectConflicts` and `ResolveConflict`; its satisfaction result must reach existing Needs/morale owners without becoming a second wellbeing ledger.

**Resolution:** add a read-only/attributed adapter at the existing day-owner seam. Verify the repository's sign convention from the live `NeedsSystem` contract; do not infer that a higher satisfaction value means a positive `Modify` delta. Prevent duplicate conflict rows on retick and test both conflict resolution and non-resolution paths.

**Stop condition:** routine assignment, conflict display, resolution, save/restore, deterministic day replay, and correct Needs/morale polarity all pass.

### B-09 — Sleep-acoustic silent result discard

**Evidence:** `SleepAcousticLedger.AdvanceDay` computes `SleepQualityResult`; the current PFGL evidence says the result does not reach `NeedsSystem`. The ledger also has quiet-hour fields that can diverge from `ShelterNoiseSystem`.

**Resolution:** expand the existing `ShelterAtmospherePanel` or use the existing sleep surface only if it is already registered; do not create a second quiet-hours writer. `ShelterNoiseSystem` owns quiet hours. Sleep Acoustic reads synchronized state and ports the computed result into `NeedsSystem` through the existing mutator. Test clean/violated quiet hours, kit installation, persistence, and exact-once day advancement.

**Stop condition:** a normal player can change soundproofing/quiet hours, observe sleep quality, and see the resulting Fatigue/Morale state after a save/load.

### B-10 — Core W3b power consumer has no external seam

**Evidence:** the Core ownership row says `PowerGridSystem.TickDay` computes fuel need internally and lacks an external demand seam.

**Resolution:** do not inject a seasonal multiplier into an unrelated private calculation. First identify a public, testable demand/draw seam and the power owner's write contract. If one exists, add the smallest provider read and one consumer application, with a neutral default and a single-application test. If it does not, retain W3b as deferred with a precise recheck condition.

**Stop condition:** either a verified owner seam and focused test, or a recorded no-op; never a speculative second power model.

### B-11 — Core W5b sky/orbital coupling lacks a proven consumer

**Evidence:** the Core plan names sky-armor/orbital telemetry as a possible follow-on, but the current Core claim does not claim a live receiving owner.

**Resolution:** run a read-only consumer census. A catalog row, UI label, or plan mention is insufficient. Only promote W5b if a current owner accepts a typed projection and the coupling changes an actual outcome. Otherwise keep the deferral visible and do not inflate the plan with speculative physics.

**Stop condition:** live owner, typed input/output, deterministic test, and counterplay evidence, or a documented deferral.

### B-12 — W12 has no verified dedicated harness entry point

**Evidence:** the current tree contains many focused balance tests and reports, but the static scan did not identify a single canonical harness for the combined W1–W4 deltas.

**Resolution:** the W12 owner must first search current tests, artifacts, and host CLI registration for an existing deterministic entry point. If one exists, pin its seeds and inputs and produce a report. If none exists, stop and request a bounded harness-design decision. A new harness is not a hidden prerequisite of W12 and must not be written as an unreviewed mega-test.

**Stop condition:** reproducible report, no product-code diff, and separate retune proposals.

### B-13 — Plan files are not ledger authority

**Evidence:** both plan files are untracked; `INTEGRATION_PLANS.md` has no matching program rows.

**Resolution:** the foreman decides whether to accept, index, or archive each plan. Builders attach handoffs and evidence; they do not self-promote a plan to integrated. A plan can be technically excellent and still be `PROPOSED` in the live queue.

**Stop condition:** explicit ledger row or a documented archive/no-op disposition.

### B-14 — Sealed and signature-gated surfaces

The following remain closed unless their named conditions are met:

- distress-signal content and rescue runtime beyond recorded seams;
- merchant restock priority under `DEC-05`;
- `DP-01` flooded-route topology tags;
- `DP-02` FactionWar per-strike emitters;
- `DP-03` black-market funds authority;
- `DP-04` semantic-kind regrouping and its parity registration;
- `DP-05` Archivists faction custody;
- quarantine drain, XP-04 economy legs, XP-06 body-integrity schema, EN-01…EN-08, and the string freeze.

A plan may reference these as dependencies, but it may not implement them, add speculative catalogs, or call them “resolved” without the required signature.

### B-15 — Generated manifest/index drift

**Evidence:** current manifest counts differ from plan snapshots and `docs/INDEX.md` is dirty in the shared worktree.

**Resolution:** the owning integrator runs the generator and `--check` after the last writer finishes. The handoff records the output timestamp/commit and leaves unrelated generated drift untouched. A manual edit to generated JSON/Markdown is forbidden.

**Stop condition:** owning generator check passes, or the exact concurrent-writer blocker is recorded.

## 8. Focused verification contract

No broad suite is part of this handoff. Each selected wave uses the smallest file or directly affected directory, normally below 100 cases. New test files run alone first. The following commands are the permitted shape:

```text
bash scripts/run_test.sh <exact-focused-test-file>
bash scripts/run_test.sh <small-focused-directory>
godot --headless --path . -- --<registered-selftest>
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-selftest-manifest.py --check
python3 scripts/ci/generate-docs-index.py --check
```

The exact command must be copied from the current owner/registry at execution time. A plan's old command is not proof that the flag still exists.

For Core W12, do not run a guessed harness command. First identify the entry point and record it in the claim. For PFGL UI waves, include:

- Core focused tests for the owner;
- host wiring test for the selected wave;
- `PanelRouteGateTests` and `PlayerSurfaceCoverageGateTests` when a route/manifest changes;
- panel bind/lifecycle and accessibility checks when a panel is touched;
- save round-trip for persistent state;
- deterministic replay for seeded or delayed effects;
- a 15 FPS manual player script.

A passing probe alone is never a feature seal. A compile-green result is never a runtime integration proof.

## 9. File-impact and handoff discipline

### 9.1 Planning-only changes in this handoff

- CREATE: `docs/plans/CORE_MECHANICS_PLAYER_FACING_PORTFOLIO_PRECISION_FULL_INTEGRATION_HANDOFF.md` (this file).
- READ ONLY: the two canonical plan files.
- READ ONLY: `AGENTS.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`, and the master expansion authority.
- INTENTIONALLY UNTOUCHED: all production, data, tests, generated indexes, manifests, ledgers, and active claims.

### 9.2 Future wave file rule

Every future package must list exact paths before editing. A path can be `CREATE`, `MODIFY`, `READ ONLY`, `MIGRATE`, `DEPRECATE`, or `DELETE`. A path cannot appear in a claim merely because a plan mentions it. Proposed files are not evidence until they exist and their public APIs are read.

The first safe implementation step for either portfolio is not a broad refactor. It is a current-head, owner-specific PIR record:

1. re-read the owner and receiver APIs;
2. paste live signatures into the claim;
3. confirm save and event ownership;
4. confirm the UI bind-vs-new-route decision;
5. confirm focused baseline tests;
6. identify the smallest reversible slice;
7. stop if any item is false or claimed by another owner.

## 10. Rollback and recovery

- Planning corrections are additive evidence addenda or explicit current-tense replacements; historical snapshots remain historical.
- A Core or PFGL wave must land in small slices: owner contract, host/save, lifecycle/event, UI, then verification.
- A failed slice is reverted before the next slice begins. Do not stack speculative fixes on a red baseline.
- A new save section, new authority, changed difficulty scalar, or new catalog is never silently rolled back by deleting a panel; the state and data owner must provide a migration/recovery path.
- If a shared-hub edit is interrupted, preserve the other agent's changes, return the hub claim, and leave a handoff naming the last verified state.
- If a plan premise fails, mark it `STALE_PLAN` and return it to the factory premise sweep. Do not adapt a deprecated API to make a plan compile.
- If a save is corrupt, use the existing save quarantine/recovery owner. This handoff does not invent a repair store.
- If a deterministic replay diverges, stop the wave and preserve the first divergent state. Do not reseed, reorder, or change the report to hide divergence.

## 11. Definition of done for the portfolio

The portfolio is fully integrated only when all of the following are true:

1. The foreman has accepted or explicitly rejected each of the two plans in `INTEGRATION_PLANS.md`.
2. Every selected Core wave has a current focused test, host/save/event evidence where applicable, a truthful player observation, and a handoff. W12 has a reproducible balance report and no inline tuning.
3. Every selected PFGL wave is player-operable on the named owner, not merely host-complete or CLI-complete.
4. All decision-gated work is either signed and implemented, or explicitly deferred/retired with a recheck condition; no half-wired feature is left in the active queue.
5. Current HEAD, save pin, manifest counts, surface classifications, and generated checks are recorded in the final handoff. Historical counts are labeled historical.
6. No active claim overlaps another claim at the final state; all temporary shared-hub claims are released.
7. Focused verification commands and results are attached. Known pre-existing failures are named and not silently folded into a green claim.
8. Deterministic replays, save round-trips, lifecycle checks, and manual 15 FPS player scripts prove the declared seal tier.
9. No parallel authority, speculative catalog, Unity dependency, unbound host effect, or unclaimed content row was introduced.
10. The final closeout states what remains deferred and why. “Full integration” never means pretending a blocked decision is solved.

## 12. Implementation handoff contract

### MUST PRESERVE

- Godot as the active engine; Core remains engine-free.
- JSON as authored data authority.
- One mutable owner per concern and existing save ownership.
- Seeded determinism and exact-once consequence guards.
- Current owner APIs and current generated manifests.
- Existing sealed plans, retired debts, and decision gates.
- Unrelated dirty worktree changes.

### MUST ADD

- Current-head evidence refreshes before any future implementation.
- Correct live test paths and `CREATE_PROPOSED` labels.
- One selected wave claim at a time.
- Focused tests, save/determinism/lifecycle evidence, and a truthful player script.
- A foreman-owned ledger row and decision disposition for every accepted or blocked package.
- W12 measurement and any separate signed retune proposals.

### MUST NOT DO

- Do not edit active PFGL or C1 shared paths from this handoff.
- Do not add `RecruitmentHostSession`, `ChronicConditionHostSession`, new panels, new registries, or new save sections without a claim and live API read.
- Do not use stale `WeatherKind`, `CancelCampaign`, `AcceptCandidate`, `Abandon`, `ScheduleRepair`, `ClearAlert`, or other invented/fictional verbs.
- Do not call a plan complete because its characters exceed a threshold.
- Do not run the full suite or a broad build as a default response.
- Do not hand-edit generated indexes or manifests.
- Do not open a signature-gated surface.

### VERIFY WITH

- Current `git rev-parse HEAD`.
- Current save pin and player-surface manifest read.
- Exact focused test file(s) through `scripts/run_test.sh`.
- Registered headless selftest(s), where the current registry names them.
- Save round-trip, deterministic replay, route/lifecycle/a11y checks for the selected surface.
- Owning generator `--check` after generated output changes.
- A 15 FPS manual player script for player-facing claims.

### FIRST SAFE IMPLEMENTATION STEP

The first safe implementation step is **Phase 0 evidence and ownership freeze**, not a code edit. The current active PFGL owner must first stabilize or hand off its dirty shared navigation/expedition paths. Then a foreman/integrator may claim exactly one next wave—preferably PFGL W10 sleep-acoustic recovery or Core W12 baseline closure, according to the current queue—and paste a fresh PIR record before implementation.

## Appendix A — Evidence commands run for this handoff

These are read-only/static checks performed during this pass. They are not a claim that the full project is green.

```text
git rev-parse HEAD
→ b1a8d08d10210253b1b64cef3bd52cffede06758

wc -c docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
→ 296357

wc -c docs/plans/PLAYER_FACING_GAMEPLAY_LOOPS_MASTER_INTEGRATION_PLAN.md
→ 211799

python3 manifest read
→ totalSurfaces=222, interactiveSurfaces=60, readOnlySurfaces=162

rg save-pin assertions
→ current assertions remain 266

rg current Core/PFGL ownership and ledger mentions
→ Core W1–W10 ownership rows present; no CORE-MECH/PFGL-MASTER row in INTEGRATION_PLANS.md

path-token existence scan
→ current landed tests were found under Disease/, Defense/DefenseProjectionTruthTests.cs,
  Expeditions/, MoralChoice/, Verdict/, and other live namespaces; the plan's older
  suggested paths require the corrections in §3.4.
```

At the time of the initial planning handoff, no build, full test suite, soak, or runtime session was run. That remains the historical record for the planning-only pass. A later user-requested integration preflight ran only focused checks and is recorded in Appendix A.1; it did not edit production paths.

## Appendix A.1 — User-requested integration preflight (2026-09-25)

After the user requested that integration begin, the repository was re-read and a bounded preflight was run before any production edit:

```text
bash scripts/run_test.sh Ashfall.Core.Tests/Needs/SleepAcousticLedgerTests.cs
→ 8/8 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Needs/SleepAcousticRestEngineTests.cs
→ 5/5 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs
→ 11/11 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Disease/FoodborneExposureBridgeTests.cs
→ 10/10 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Medical/DoseStormWindowTests.cs
→ 12/12 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/SeasonalPressureProviderTests.cs
→ 12/12 PASS

bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/BreakdownConsequenceTests.cs
→ 10/10 PASS

godot --headless --path . -- --sleep-acoustic-selftest
→ Expansion 41 probe 12/12 PASS

dotnet build Ashfall.csproj --no-restore --nologo
→ Build succeeded; 0 warnings; 0 errors
```

The preflight did **not** authorize a production edit. It found two material gates:

1. Core W1–W10 implementation paths are still dirty/uncommitted in the shared worktree. W12 cannot publish a valid pre-wave/post-wave delta report until that integrator freezes or commits the baseline.
2. PFGL W10 cannot safely add a direct Needs delta yet. `ShelterScheduleSystem` already applies a six-point nominal eight-hour fatigue recovery through `Main.ApplyScheduleNeedsModifiers`; `SleepAcousticLedger.AdvanceDay` also discards its `SleepQualityResult`. A direct acoustic delta would therefore require a signed decision about additive quality modifier versus replacement, plus an explicit mapping from the legacy `primary_shelter_dormitory` quarter to the canonical shelter assignment room. The current `ShelterAtmospherePanel` is also dirty and the shared PFGL route claim is active.

The safe implementation package is consequently `BLOCKED — claim/decision required`, not a code failure. The next valid action is for the foreman to freeze or transfer the active claims and choose either:

- `CORE-MECH-W12-BALANCE-BASELINE-REFRESH` after the W1–W10 baseline is stable; or
- `PFGL-W10-SLEEP-ACOUSTIC-RECOVERY` after the sleep-recovery composition decision and exact host/test paths are claimed.

No production, data, test, generated, ledger, or ownership file was changed during this preflight.

---

## Appendix B — Revision and ownership note

This handoff is a read-only precision artifact created without changing either claimed plan. The next plan revision should be made by the current owner after the active claims are released or transferred. It should contain only:

- current evidence-head and current pin refresh;
- current manifest count refresh;
- the test-path corrections in §3.4;
- the status split in §3.6;
- a pointer to this handoff for blockade sequencing;
- a final character-budget decision if the 500,000 cap is portfolio-wide.

It should not append another generic “quality pass” chapter merely to increase size. The master authority's anti-padding rule remains stronger than a character target.

## Appendix C — Final status statement

**Planning artifact:** complete for the requested two-plan portfolio.
**Precision pass:** complete as a current-tree audit; corrections are enumerated.
**Full production integration:** not claimed and not executed by this handoff because active claims, compile blockers, missing W12 evidence, and named decision gates remain.
**Blockades:** each has a bounded resolution path and stop condition above.
**Safe next action:** foreman releases or transfers the active shared-path claim, then selects exactly one wave for a fresh PIR and focused implementation.
