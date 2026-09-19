# Fifteen Partial Plans — Production-Unblock Integration Plan

**Status:** PROPOSED / READ-ONLY AUDIT RESULT

**Prepared:** 2026-09-19

**Scope:** exactly 15 partially implemented plans with current Core code and focused tests

**Authority:** this document proposes the next batches; it does not activate them, assign claims, or supersede `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, or `KNOWN_DEBT.md`.

## 1. Objective

Move 15 partially implemented plans from isolated, test-only Core code to truthful production integration while preserving ASHFALL's existing owners, save sections, deterministic behavior, JSON authority, and Godot presentation boundary.

The desired outcome is not "15 new systems compile." Each plan is complete only when the current canonical owner, typed event or command seam, host lifecycle, existing save owner, and an observable Godot or headless outcome agree.

The portfolio deliberately excludes:

- fully greenfield plans with no current implementation;
- plans sealed or retired by the 2026-09-19 production-islands work;
- any path currently owned by the active XP difficulty or Wave 11 Part 2 claims;
- speculative UI or duplicate save stores created only to make a partial system appear reachable.

## 2. Current Reality

The repository contains a coherent partial tranche:

- fourteen selected systems exist as untracked Core source plus focused test pairs;
- Plan 208 extends the tracked `LeadershipSystem` and adds an untracked focused test;
- `Ashfall.Core.Tests.csproj` builds with zero warnings and zero errors;
- all 15 focused test files pass independently: **101/101 tests**;
- every selected system type, and each new Plan 208 command, has **zero references under `src/`**;
- several candidates duplicate or compete with already authoritative systems and therefore must be narrowed before they are wired;
- current port-contract policy classifies many of these seams as `TEST_ONLY`, with the production-islands log explicitly recording zero production callers.

In this audit, "blocked" means blocked from production acceptance by a concrete integration or ownership gap. It does not assign the formal `BLOCKED` status in the live foreman ledger.

The existing tests prove local behavior. They do not prove production reachability, ownership safety, persistence through the canonical host, or player-observable outcomes.

The live foreman ledger currently has another active batch. Under the repository's batch rules, this plan remains a proposal until that batch is accepted or blocked and the foreman activates no more than three packages at a time.

## 3. Required Delta

Each selected plan needs the same six-part closure, adjusted to its owner:

1. **Reconcile authority.** Delete, merge, or adapt any state that competes with an existing owner.
2. **Define a typed seam.** Consume existing facts or commands; do not infer gameplay truth in UI callbacks.
3. **Compose in the host.** Construct, attach, tick, refresh, and dispose through the current lifecycle owner.
4. **Persist through the existing section.** Add state to the current aggregate or save store; do not create a parallel section without an explicit architecture decision.
5. **Expose one truthful outcome.** Add a current panel/read-model surface or a bounded headless observable where player UI is not yet warranted.
6. **Prove the route.** Add focused integration coverage for producer -> adapter -> owner -> save/restore -> observable result.

## 4. Evidence and Ranked Selection

All source and test paths below exist in the current workspace. `U` means untracked; `M` means a tracked file is modified. Test counts are from individual `scripts/run_test.sh` runs on 2026-09-19.

| Rank | Plan | Current partial | Focused proof | Current production blocker | Minimum safe unblock |
|---:|---|---|---:|---|---|
| 1 | 208 / C2[42], Leadership Succession | `M Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`; `U Ashfall.Core.Tests/Survivors/LeadershipSuccessionTests.cs` | 6 | New designate/deputy/challenge commands have no host callers or observable journal/UI route | Keep `LeadershipSystem` inside `SurvivorSocialCoordinator`; add typed host commands, persisted event facts, and a current governance read model |
| 2 | 143 / C1[23], Afflictions -> Duty | `U Assets/Ashfall.Core/Medical/AfflictionDutyBridge.cs` plus test | 6 | Stateless bridge is not called; hard-coded role/affliction string policy competes with duty and fitness authorities | Convert to typed capability input for `FitnessForDutyModel`/duty assignment; data-backed mappings; never store a second duty verdict |
| 3 | 139 / C1[22], Combat -> Faction Standing | `U Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` plus test | 7 | No combat subscriber; current bridge can modify both faction authorities for one incident; idempotency lacks a complete capture path | Choose one standing command owner, subscribe to final combat incidents once, persist processed incident IDs in its existing section |
| 4 | 133 / C1[20], Discovery Consequences | `U Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` plus test | 6 | Discovery consequences never receive expedition/world facts and never reach host/save/UI | Consume canonical discovery-completed facts, write stable consequence facts through the campaign consequence owner, expose summary through an existing expedition/world surface |
| 5 | 171 / C1[30], Dynamic Quest Generation | `U Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs` plus test | 6 | Generator owns templates and lifecycle beside `DynamicQuestlines`, `QuestRuntimeCoordinator`, and the existing `dynamic_quests` section | Reduce to deterministic candidate/parameter generation; source templates from JSON; hand accepted candidates to the canonical quest runtime and save owner |
| 6 | 215 / C2[44], Resource Rationing | `U Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` plus test | 5 | Arbitrary resource strings, multipliers, morale, and crisis state have no canonical inventory/water/power consumers | Make rationing a policy/protocol owner only; validate canonical resource IDs; route allocations to existing resource consumers; persist policy, not duplicate stock |
| 7 | 183 / D1[13], Child Development | `U Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` plus test | 15 | Parallel age/stage truth beside `CohortSystem`, `GenerationalSystem`, `SurvivorLifecycle`, and existing `child_development` save section | Derive stages from the canonical age/birth fact; adapt binary maturation; attach one idempotent adult handoff to the existing generational lifecycle |
| 8 | 185 / C1[33], Memory Decay | `U Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs` plus test | 7 | Universal memory state is isolated and overlaps phantom memory, skills, relations, and journal owners | Narrow to a deterministic coordinator/projection over typed source facts; source-specific owners retain their state and mutations |
| 9 | 162 / C2[34], Shelter Archive | `U Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs` plus test | 7 | Parallel archive records overlap `JournalSystem`, `CulturalArchiveVaultSystem`, `CampaignConsequenceLedger`, and `MemorialSystem` | Make it a searchable read index of canonical record IDs, or merge it into the current archive owner; no copied campaign-history truth |
| 10 | 216 / C1[41], Exercise | `U Assets/Ashfall.Core/Survivors/ExerciseSystem.cs` plus test | 6 | Independent fitness profile/baseline can compete with fitness-for-duty, needs, age, and medical state | Persist conditioning only; compose its contribution through the current capability model; use canonical schedule/needs/medical facts |
| 11 | 202 / C1[38], Interpersonal Conflict | `U Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs` plus test | 5 | Daily conflict authority overlaps shelter social dynamics, ration conflict, and survivor relations | Consume source-backed grievances and emit typed conflict outcomes; relations/morale owners apply effects; no unbacked random conflict source |
| 12 | 163 / C2[35], Wasteland Cartography | `U Assets/Ashfall.Core/Exploration/CartographySystem.cs` plus test | 6 | Own discovery/progress/skill truth competes with `WastelandMapSystem`, fog/knowledge, and `SkillProgressionSystem` | Convert to survey-quality and map-provenance overlay on the canonical world map; route skill gain to skill progression |
| 13 | 210 / C1[40], Personal Belongings | `U Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs` plus test | 7 | Owns inheritance and morale beside inventory, death/legacy, and morale authorities | Store stable survivor-to-item claims and keepsake metadata only; inventory owns items; death orchestrator distributes; morale consumes emitted facts |
| 14 | 219 / C2[45], Documentation | `U Assets/Ashfall.Core/Culture/DocumentationSystem.cs` plus test | 6 | Standalone records/albums/morale have no inventory provenance, archive/journal IDs, action costs, or production consumer | Create documentation through a costed host command; reference canonical inventory and archive records; emit morale/culture facts to their owners |
| 15 | 167 / C2[36], Tunnel Network | `U Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs` plus test | 6 | A second junction/segment graph is isolated from canonical map knowledge, route planning, travel, hazards, and save | Represent tunnels as a typed canonical map/route layer; keep tunnel condition/hazards local; expose routes only after cartographic discovery |

Selection rationale: every row already has material implementation and a passing focused contract. The rank favors existing-owner extensions and narrow adapters over isolated systems that require more authority surgery.

## 5. Existing Extension Seams

The implementation must extend these seams rather than create replacements:

| Concern | Existing authority or seam to preserve |
|---|---|
| Leadership/social state | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs`, `SurvivorSocialCoordinator.cs`, `src/Main.SurvivorSocial.cs`, `src/Host/SurvivorSocialSaveStore.cs`, save section `survivor_social` |
| Duty capability | `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`, `Assets/Ashfall.Core/DutyRoster/`, `src/Host/DutyRosterHostSession.cs`, `src/Main.DutyRoster.cs` |
| Combat/factions | `Assets/Ashfall.Core/Combat/TacticalCombatSystem*.cs`, `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`, `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs`, `src/Host/FactionBranchHostSession.cs` |
| Expeditions/consequences | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`, `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Main.Expeditions.cs` |
| Quests | `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`, `QuestRuntimeCoordinator.cs`, `src/Host/DynamicQuestSaveStore.cs`, save section `dynamic_quests` |
| Resource stocks | Existing inventory, food, water, power, needs, and morale owners; rationing may command them but may not mirror their quantities |
| Generations | `Assets/Ashfall.Core/CohortSystem.cs`, `Survivors/GenerationalSystem.cs`, `SurvivorLifecycle.cs`, `src/Host/GenerationalSaveStore.cs`, save section `child_development` |
| Memory/skills/relations | `PhantomMemoryEngine.cs`, `SkillProgressionSystem.cs`, `SurvivorRelationsSystem.cs`, `Journal/JournalSystem.cs` |
| Archive/history | `Culture/CulturalArchiveVaultSystem.cs`, `Journal/JournalSystem.cs`, `Flags/CampaignConsequenceLedger.cs`, `Memorial/MemorialSystem.cs` |
| Physical capability | `Survivors/FitnessForDutyModel.cs`, canonical needs/medical state, duty scheduler, skill progression |
| Social conflict | `Shelter/ShelterSocialDynamicsSystem.cs`, `Survivors/RationConflictSystem.cs`, `SurvivorRelationsSystem.cs`, `SurvivorSocialCoordinator.cs` |
| World topology | `World/WastelandMapSystem.cs`, `src/Host/WorldHostSession.cs`, current map/travel routing and world-knowledge read models |
| Items and inheritance | Inventory authority/host, survivor fate/death orchestration, memorial/legacy facts |
| Culture/documentation | Inventory provenance, journal/archive authorities, culture/morale fact consumers |

## 6. Proposed Architecture

Use one integration pattern across the tranche:

```text
canonical producer
    -> immutable typed fact / validated command
    -> narrow partial-plan adapter or coordinator
    -> canonical mutation owner(s)
    -> existing aggregate capture/restore
    -> host read model
    -> existing Godot panel/route or bounded new panel
```

Rules:

- A partial system may own only state unique to its concern.
- Cross-system effects are facts or commands, not direct mutation of two competing owners.
- Host composition owns attachment and detachment. Core types remain engine-free.
- Existing save sections absorb new state where the concern is already represented.
- Presentation reads Core outcomes and never recomputes them.
- A production route is required before a seam can move from `TEST_ONLY` to production policy.

## 7. Ownership Matrix

| Plan | Unique state it may own | State it must not own | Target aggregate/save owner |
|---|---|---|---|
| 208 | successor/deputy designation, challenge lifecycle, legitimacy facts | survivor roster, relations, shelter-wide morale | `survivor_social` |
| 143 | none, or data-backed capability reason codes | duty assignment, medical diagnosis, fitness total | duty/medical owners; preferably stateless |
| 139 | processed combat incident IDs if not already in faction consequence ledger | duplicate trust and duplicate standing totals | existing faction/campaign consequence state |
| 133 | discovery consequence application/provenance IDs | discovery truth, expedition state, generic campaign flags | expedition/campaign consequence aggregate |
| 171 | deterministic generator cursor/cooldowns only if canonical runtime lacks them | active quest lifecycle, objectives, reward settlement | `dynamic_quests` |
| 215 | ration policy, priority bands, effective-from day | resource quantities, needs, morale totals | existing economy/resource save aggregate |
| 183 | stage/milestone provenance not derivable from canonical age | second age, skills, traits, child/adult rosters | `child_development` |
| 185 | reinforcement/decay scheduling and applied fact IDs | skills, relationships, journal entries, phantom memories | source owners plus one coordinator state in the appropriate aggregate |
| 162 | search/index metadata and canonical record references | copied journal, memorial, consequence, or archive records | current archive/journal aggregate |
| 216 | conditioning adaptation and recovery state | health, needs, duty fitness, skill totals | survivor/fitness aggregate chosen in preflight |
| 202 | grievance/conflict case lifecycle and applied outcome IDs | relationship/morale totals or independent ration incidents | `survivor_social` |
| 163 | survey quality, map provenance, traded-map provenance | discovered-region truth, topology, cartography skill total | world map/knowledge aggregate |
| 210 | ownership/attachment claim and keepsake metadata | item stacks, inheritance transfer, morale | inventory/survivor legacy aggregate |
| 219 | document metadata, album ordering, source record IDs | item stacks, journal/archive copies, morale | archive/culture aggregate |
| 167 | tunnel-specific condition, blockage, hazard, maintenance state | a second world graph, general travel state, discovery truth | world map/travel aggregate |

If preflight cannot name a single existing target aggregate for a row, that package is blocked for an architecture decision; it must not create a new save section by default.

## 8. Data Flow

### Command path

1. Godot/host validates a player or simulation command against a current Core read model.
2. The current Core owner accepts or rejects it with a typed result.
3. A partial-plan coordinator derives only its unique state change.
4. Cross-owner consequences are emitted as immutable facts with stable IDs.
5. Other canonical owners consume those facts once.
6. The host refreshes the read model and persists through the existing section.

### Tick/event path

1. The campaign day or authoritative producer emits a stable day/event fact.
2. The host calls each registered owner once in the established phase order.
3. The partial system evaluates source facts using deterministic ordering and the seeded RNG contract where randomness is required.
4. Applied outcome IDs prevent duplicate effects across refresh, reconnect, and restore.

### Read path

Panels receive a read-only projection containing IDs, labels, status, reasons, and available commands. Panels do not derive standing, fitness, discovery, eligibility, inheritance, resource supply, or route truth.

## 9. State Model

Every stateful package must use:

- an explicit schema version;
- stable IDs for entities, incidents, outcomes, and applied effects;
- deterministic collections or explicit sorting at decision boundaries;
- capture and restore that preserve idempotency markers;
- backward-compatible defaults for saves without the new fields;
- no engine types or engine serialization in Core;
- no copied mutable state from another owner.

State models should be reduced before wiring. In particular:

- Plan 171 loses quest lifecycle state already held by the quest runtime.
- Plan 183 loses independent age truth when a canonical birth/age fact exists.
- Plan 163 loses independent discovered-region truth.
- Plan 210 loses physical item/inheritance authority.
- Plan 162 and Plan 219 store references to canonical records rather than record copies.
- Plan 167 stores tunnel-specific edge condition, not a parallel universal graph.

## 10. API and Contract Changes

Each package should expose the smallest contract from this set:

- `TryHandle(command, sourceSnapshot)` for validated player/host commands;
- `Apply(fact)` for exactly-once cross-system consequences;
- `Tick(day, orderedFacts, ISeededRng)` only where time progression is intrinsic;
- `CaptureState()` / `RestoreState(state)` for unique persistent state;
- `BuildReadModel()` for presentation;
- `Attach(producer)` / `Detach()` only in a host/session adapter, not hidden in constructors.

Avoid broad service locators, direct panel-to-system mutation, stringly typed resource/role/affliction routing, and adapters that mutate two authorities for the same effect.

For Plan 139 specifically, the public contract must select either faction trust or branch standing as the mutation authority and define the other as a projection/consumer. Applying both from one combat incident is not acceptable.

## 11. Data Changes

JSON under `Assets/StreamingAssets/Data/` remains the authored authority. Data changes are conditional on preflight evidence and must pass the current integrity pipeline.

Expected data work:

- Plan 143: replace substring role matching and hard-coded affliction IDs with existing catalog IDs or one validated mapping catalog.
- Plan 171: move generator templates, weights, prerequisites, cooldowns, and canonical quest-template references out of hard-coded defaults.
- Plan 183: reconcile against the planned/available development-stage catalog; do not introduce one if the canonical age/stage source already provides it.
- Plan 215: validate rationable resource IDs against existing resource catalogs and define policy bands as data only where designers need authorship.
- Plans 163/167: use existing world region/node/edge IDs; no duplicate topology data file.
- Plan 219: reference existing item, survivor, location, journal, and archive IDs.

Every new or changed file must use `schema_version`, snake_case IDs, reference validation, range validation, and a confirmed runtime consumer.

## 12. Save and Load Strategy

Default save placement:

- Plans 208 and 202: extend `survivor_social` capture/restore.
- Plans 171: extend the existing `dynamic_quests` section only after removing duplicate lifecycle state.
- Plan 183: extend `child_development` through `GenerationalSaveStore` and the current setup/capture path.
- Plans 133 and 139: place applied consequence/incident IDs in the existing expedition, faction, or campaign consequence owner selected during preflight.
- Plans 143: remain stateless unless a genuine unique state is proven.
- Plans 185, 216, 210: add only unique state to the existing survivor/inventory aggregate selected by the owner audit.
- Plans 162 and 219: one archive/culture aggregate, with canonical record references.
- Plans 163 and 167: one world/map aggregate, with the existing world save host.
- Plan 215: existing resource/economy aggregate; never duplicate quantities.

Every stateful package must prove:

1. new-save default;
2. capture -> restore equivalence;
3. legacy save without new fields;
4. restore followed by replay does not duplicate effects;
5. corrupted/unknown IDs fail through the current safe fallback;
6. migration is deterministic and records provenance where estimation occurs.

## 13. Determinism Strategy

- Use the existing `ISeededRng` contract; never `System.Random`, wall-clock time, `Guid.NewGuid()`, or unordered dictionary iteration at decision points.
- Derive event/outcome IDs from stable source IDs and deterministic ordinal data.
- Sort survivor, faction, quest-template, resource, region, and route candidates before seeded selection.
- Persist generator cursors/cooldowns and applied-event IDs when their omission would change replay.
- Paired same-seed runs must match before a package is accepted.
- UI refresh, read-model construction, save capture, and panel reopen must consume no RNG and mutate no Core state.

## 14. System and Event Wiring

Required producer-to-consumer contracts:

| Plan | Producer | Consumer/effect owner | Required event property |
|---|---|---|---|
| 208 | survivor fate, governance command, campaign day | leadership inside survivor social | stable command/challenge ID; one terminal result |
| 143 | medical status snapshot/change | fitness-for-duty and duty roster | typed restriction/reason; no role-name substring inference |
| 139 | finalized tactical combat incident | selected faction standing authority | stable incident/faction IDs; exactly once |
| 133 | finalized expedition discovery | campaign consequence ledger and relevant world owners | stable discovery/outcome IDs |
| 171 | campaign-state opportunity request | canonical quest runtime | deterministic candidate ID and template reference |
| 215 | ration policy command and resource-consumption request | inventory/water/power/needs owners | canonical resource ID, requested/approved amounts, policy revision |
| 183 | campaign day/age advance and survivor lifecycle | cohort/generational/adult handoff owners | stable child ID, derived stage, idempotent transition ID |
| 185 | typed memory/skill/relation facts and campaign day | source-specific owners | source ID, reinforcement strength, applied outcome ID |
| 162 | journal/memorial/consequence/archive facts | archive index | canonical record ID; no copied payload authority |
| 216 | scheduled exercise plus needs/medical snapshot | capability/fitness consumer | survivor ID, completed load, recovery result |
| 202 | source-backed grievance facts | relations/morale/social owners | grievance/conflict ID and typed resolution |
| 163 | world discovery/survey/trade facts | canonical map and skill progression | region/edge ID, quality delta, provenance |
| 210 | inventory assignment/gift and survivor death | inventory/death/legacy/morale owners | item instance ID, survivor ID, transfer reason |
| 219 | documentation command and canonical source facts | inventory cost, archive/journal, culture/morale | document ID, source IDs, creator, day, consumed inputs |
| 167 | map knowledge, route request, hazard/maintenance facts | canonical route/travel/world owners | canonical node/edge IDs and tunnel-specific availability |

All subscriptions must have explicit teardown and must not multiply after reload or scene re-entry.

## 15. Godot Integration

Godot remains a thin presenter/adapter:

- Prefer extending an existing panel that already presents the canonical owner.
- Create a new panel only when no truthful surface exists and the foreman assigns the path.
- Every command must return visible success/rejection feedback.
- Preserve close/back, keyboard/controller focus, refresh after mutation, and subscription disposal.
- Use read models generated by Core/host; panels do not calculate eligibility, severity, standing deltas, fitness, allocation, discovery, or route availability.
- A headless host route may be the first observable acceptance for backend-only packages, but player-facing plans are not portfolio-complete until their assigned Godot route exists.
- Any Godot runtime check uses 15 FPS unless explicitly overridden.

Likely existing surfaces to inspect before creating UI include the expedition, duty roster, faction, dynamic quest, survivor relations/social, map/subterranean cartography, inventory, journal/archive, and governance/politics panels.

## 16. Narrative and Content Integration

- Journal/archive entries must be derived from typed facts and stable IDs.
- Dynamic quest text references canonical quest templates; the generator does not author final prose at runtime.
- Child, conflict, memory, rationing, and inheritance language must remain restrained and non-punitive.
- Documentation/archive records distinguish player-known presentation from canonical hidden state.
- No real wars, countries, people, copied text, copied art, or copied UI layouts.
- Localization keys are added through the current localization path when a player-facing surface is assigned.

## 17. Failure Modes and Guards

| Failure | Guard |
|---|---|
| A locally tested island is declared complete | Require a production caller plus end-to-end focused proof |
| Duplicate authority is retained for convenience | Ownership preflight must list every overlapping field and disposition before edit |
| One event applies the same effect twice | One mutation owner, stable event IDs, persisted idempotency set |
| Save capture exists but setup/restore does not | Trace both methods through the registered section and test a host round trip |
| New save section forks an existing concern | Architecture decision required; default is extend/merge |
| UI computes a missing Core answer | Add a Core/host read-model field; keep panel passive |
| Hard-coded strings silently drift from catalogs | Resolve validated canonical IDs at load time |
| Tick order changes outcomes | Register with the current campaign owner phase and test ordering |
| Restore or panel refresh consumes RNG | Separate mutation from projection and assert stable replay |
| Untracked source is overwritten by another agent | Foreman claims exact files before any builder starts |
| Active XP/Wave 11 work is raced | Do not activate this portfolio until current claims close or transfer |
| Port policy is changed before reachability exists | Update generated policy only after source evidence and owner verification |

## 18. Test Strategy

Baseline already observed:

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore --nologo` — PASS, 0 warnings, 0 errors.
- Each selected focused test file run alone through `bash scripts/run_test.sh` — PASS, 101/101 total.

Per-package verification order:

1. Run the existing focused file alone.
2. Add authority-reconciliation tests: the duplicate state is gone or proven a projection.
3. Add producer -> adapter -> canonical owner integration tests.
4. Add capture/restore and replay-idempotency tests for stateful packages.
5. Add same-seed paired replay where RNG or candidate ordering exists.
6. Add the smallest host wiring test proving construction, event/command reachability, and observable output.
7. Add one focused Godot headless lifecycle/UI check only when the package touches that runtime path.
8. Run the owning data validator for changed JSON.
9. Run `git diff --check` for the package.

Tests remain below the normal 100-case builder ceiling. Do not run the full suite. Do not re-enable quarantined tests as a side effect. New test files run alone first.

Suggested acceptance targets by wave are the three existing focused files plus the new integration/host test files for those packages; aggregation is allowed only for homogeneous static catalog cases with per-row failures.

## 19. Dependency-Ordered Phases

### Phase 0 — Foreman activation and preservation gate

1. Finish, block, or explicitly transfer the currently active batch.
2. Reconcile this proposal against the live ownership table.
3. Claim the exact Core, test, host, UI, data, and save paths for the first wave.
4. Preserve all current untracked partial files; record hashes/status before edits.
5. Assign one integrator for shared composition/save seams.
6. Permit no more than three concurrent packages.

Acceptance: no claim overlap; each selected partial is attributed to an owner; first-wave source/tests still pass unchanged.

### Wave 1 — Narrow existing-owner integrations

Packages:

1. Plan 208 Leadership Succession.
2. Plan 143 Affliction -> Duty.
3. Plan 139 Combat -> Faction Standing.

Why first: one extends an existing aggregate, one should remain almost stateless, and one is a bounded event bridge. These expose the portfolio's shared integration discipline at relatively low data/UI cost.

Acceptance:

- leadership commands reach the current survivor-social owner and survive its existing save round trip;
- duty restrictions use typed medical/capability inputs and alter the canonical assignment result without a second verdict;
- one finalized combat incident changes one chosen faction authority exactly once across save/restore;
- each has one host-observable route and no new parallel save section.

### Wave 2 — Campaign consequence and policy pipelines

Packages:

4. Plan 133 Discovery Consequences.
5. Plan 171 Dynamic Quest Generation.
6. Plan 215 Resource Rationing.

Dependencies: Wave 1 establishes exactly-once bridge and command patterns. Plan 171 must reconcile with the already persisted dynamic quest runtime before template migration. Plan 215 must enumerate canonical resource consumers before policy wiring.

Acceptance:

- discovery facts create persistent, idempotent canonical consequences visible through a current surface;
- the generator only proposes deterministic canonical quest candidates and the existing runtime owns lifecycle/rewards;
- ration policy affects real resource-consumption requests without storing duplicate stock or morale totals;
- all three restore without replay duplication.

### Wave 3 — Survivor continuity and institutional memory foundations

Packages:

7. Plan 183 Child Development.
8. Plan 185 Memory Decay.
9. Plan 162 Shelter Archive.

Dependencies: Plan 183 requires a written age/maturation authority decision. Plan 185 requires a source-owner matrix. Plan 162 must select merge-versus-index before code mutation.

Acceptance:

- one canonical age produces deterministic stages and one adult handoff;
- memory coordination mutates only typed source owners and replays deterministically;
- shelter archive queries reference canonical records without copying their authority;
- legacy save defaults and focused round trips pass.

### Wave 4 — Derived capability, social outcome, and world knowledge

Packages:

10. Plan 216 Exercise.
11. Plan 202 Interpersonal Conflict.
12. Plan 163 Wasteland Cartography.

Dependencies: exercise consumes the Wave 1 medical capability seam and Wave 3 age/stage fact. Conflict integrates with the current survivor-social owner after leadership state is stable. Cartography builds on the sealed canonical map/graph route and must not revive a second discovery graph.

Acceptance:

- conditioning contributes to, but does not replace, current fitness-for-duty evaluation;
- every conflict starts from a canonical grievance/source fact and applies typed relation/morale outcomes once;
- survey quality/provenance is visible on the canonical map while discovery and skill remain with their owners.

### Wave 5 — Provenance-rich dependent systems

Packages:

13. Plan 210 Personal Belongings.
14. Plan 219 Documentation.
15. Plan 167 Tunnel Network.

Dependencies: belongings needs stable survivor/death/social seams; documentation depends on the Wave 3 archive decision and canonical item provenance; tunnels depend on the Wave 4 cartography overlay and current graph travel.

Acceptance:

- belongings reference real item instances, and death inheritance is executed by the canonical death/inventory route;
- documentation consumes real inputs/action cost and produces canonical archive/journal references;
- tunnel edges participate in current route discovery and travel with tunnel-local hazards/condition only;
- all player-facing outcomes are reachable in Godot and survive save/restore.

### Phase 6 — Portfolio closure

1. Re-run each package's focused acceptance target.
2. Verify no selected production entry remains at zero `src/` callers.
3. Regenerate/check policy and documentation indexes only through their owning generators and only when the integrator owns those shared paths.
4. Update the live ledger/debt/ownership documents only by the foreman or named integrator.
5. Record per-package evidence: commits/diffs, exact tests, runtime check, save section, production caller, and remaining limitations.

## 20. File Impact Map

### Existing partial files — package-owned candidates

| Plan | Core source | Existing focused test |
|---|---|---|
| 208 | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | `Ashfall.Core.Tests/Survivors/LeadershipSuccessionTests.cs` |
| 143 | `Assets/Ashfall.Core/Medical/AfflictionDutyBridge.cs` | `Ashfall.Core.Tests/Medical/AfflictionDutyBridgeTests.cs` |
| 139 | `Assets/Ashfall.Core/Combat/CombatFactionStandingBridge.cs` | `Ashfall.Core.Tests/Combat/CombatFactionStandingBridgeTests.cs` |
| 133 | `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs` | `Ashfall.Core.Tests/Expeditions/DiscoveryConsequenceSystemTests.cs` |
| 171 | `Assets/Ashfall.Core/Quests/DynamicQuestGenerator.cs` | `Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs` |
| 215 | `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` | `Ashfall.Core.Tests/Economy/ResourceRationingSystemTests.cs` |
| 183 | `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` | `Ashfall.Core.Tests/Survivors/ChildDevelopmentSystemTests.cs` |
| 185 | `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs` | `Ashfall.Core.Tests/Cognition/MemoryDecaySystemTests.cs` |
| 162 | `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs` | `Ashfall.Core.Tests/Shelter/ShelterArchiveSystemTests.cs` |
| 216 | `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs` | `Ashfall.Core.Tests/Survivors/ExerciseSystemTests.cs` |
| 202 | `Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs` | `Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs` |
| 163 | `Assets/Ashfall.Core/Exploration/CartographySystem.cs` | `Ashfall.Core.Tests/Exploration/CartographySystemTests.cs` |
| 210 | `Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs` | `Ashfall.Core.Tests/Survivors/PersonalBelongingsSystemTests.cs` |
| 219 | `Assets/Ashfall.Core/Culture/DocumentationSystem.cs` | `Ashfall.Core.Tests/Culture/DocumentationSystemTests.cs` |
| 167 | `Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs` | `Ashfall.Core.Tests/Underground/TunnelNetworkSystemTests.cs` |

### Shared existing paths — integrator-only until exact claims are assigned

- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`
- `Assets/Ashfall.Core/CohortSystem.cs`
- `Assets/Ashfall.Core/Survivors/GenerationalSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs`
- `Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`
- `Assets/Ashfall.Core/SurvivorRelationsSystem.cs`
- `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs`
- `Assets/Ashfall.Core/Quests/QuestRuntimeCoordinator.cs`
- `Assets/Ashfall.Core/World/WastelandMapSystem.cs`
- `Assets/Ashfall.Core/Journal/JournalSystem.cs`
- `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs`
- `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs`
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`
- `src/Main.SurvivorSocial.cs`
- `src/Main.DutyRoster.cs`
- `src/Main.Expeditions.cs`
- `src/Main.FactionBranch.cs`
- `src/Main.ExpandedShelterSystems.cs`
- `src/Host/SurvivorSocialSaveStore.cs`
- `src/Host/DutyRosterHostSession.cs`
- `src/Host/ExpeditionHostSession.cs`
- `src/Host/FactionBranchHostSession.cs`
- `src/Host/DynamicQuestSaveStore.cs`
- `src/Host/GenerationalSaveStore.cs`
- `src/Host/InventoryHostSession.cs`
- `src/Host/WorldHostSession.cs`

The exact UI, data, host adapter, and new integration-test paths for a package must be named in its foreman claim after the preflight owner audit. This portfolio does not invent filenames before that evidence exists.

### Governance paths intentionally untouched by this proposal

- `INTEGRATION_PLANS.md`
- `WORKTREE_OWNERSHIP.md`
- `KNOWN_DEBT.md`
- generated documentation/policy outputs

## 21. Risks and Mitigations

| Risk | Severity | Mitigation |
|---|---:|---|
| Dirty/untracked partial tranche is lost or mixed with unrelated work | Critical | Foreman records status/hash and claims exact paths before edits; builders preserve unrelated changes |
| Partial implementations encode attractive but duplicate authority | Critical | Mandatory field-by-field owner disposition before wiring |
| Existing passing tests anchor the wrong architecture | High | Keep behavior tests only where contract remains valid; add owner and reachability assertions before refactor |
| Shared composition/save files cause agent races | High | One integrator owns shared seams; maximum three disjoint builders |
| Fifteen plans become one unreviewable mega-change | High | Five waves, three packages maximum, acceptance and ledger closure per wave |
| Save schema expands without migration | High | Existing section, schema version, legacy-default and replay tests |
| Cross-system events create loops | High | Directional producer/consumer table, stable IDs, no adapter re-emission of source fact |
| Data catalogs are added without consumers | Medium | Validator plus runtime-consumer evidence required in the same package |
| UI implies capability that Core rejects | Medium | UI commands use current read model and display typed rejection reason |
| Old census/plan documents misstate current status | Medium | Current source, 2026-09-19 debt log, and live ledgers outrank historical prose |

## 22. Out of Scope

- Implementing any of the 15 packages in this planning task.
- Activating a batch or assigning ownership without the foreman.
- Editing production code, live ledgers, debt registers, quarantine manifests, or generated indexes.
- Reviving Plans 30, 32, 34, or 36 as blocked work; their recorded remainders are sealed/retired in current evidence.
- Selecting greenfield portfolio plans merely to reach a count of 15.
- Broad test-suite runs, quarantine changes, release work, balance tuning, visual redesign, or new architecture unrelated to the selected integrations.
- Creating a generic event bus, universal save store, second world graph, second inventory, second quest runtime, or second survivor-social aggregate.

## 23. Rollback Strategy

Rollback is package-local:

1. Detach the new host subscription/command route.
2. Restore the previous read model/panel route.
3. Keep new save fields optional and ignored by the prior reader where possible.
4. Revert only package-owned changes; do not reset the dirty worktree.
5. Preserve authored JSON until the owning package is removed through its migration policy.
6. Re-run the pre-package focused target and save compatibility test.

If authority reconciliation reveals that a partial system should be merged away, rollback means retaining the canonical owner and deleting only the redundant partial after its useful contracts/tests have been migrated and reviewed. No destructive cleanup occurs without exact ownership and approval.

## 24. Definition of Done

The portfolio is complete only when all 15 rows meet all applicable criteria:

- current source evidence identifies one authority for every mutable field;
- the partial Core logic is retained, narrowed, merged, or deliberately removed with rationale;
- a production host caller exists;
- producer and consumer are connected through a typed seam;
- host construction, tick/event phase, refresh, and disposal are correct;
- unique state captures/restores through an existing registered section;
- legacy save and replay idempotency pass;
- deterministic behavior passes same-seed verification where applicable;
- JSON changes pass the current integrity validator and have a runtime consumer;
- one truthful headless or Godot observable proves the outcome;
- player-facing plans have usable focus/back/feedback behavior;
- the focused package target passes under `TEST_POLICY.md`;
- no unrelated user changes were overwritten;
- port-contract classification reflects current source reachability;
- the foreman/integrator records acceptance and releases claims.

Compile-green Core code or passing unit tests alone do not satisfy this definition.

## 25. Implementation Handoff

### MUST PRESERVE

- Godot as the only active engine and `Assets/Ashfall.Core/` as engine-free.
- JSON as authored gameplay-data authority.
- Existing inventory, resource, survivor-social, faction, quest, world-map, journal/archive, save, RNG, and campaign-day owners.
- Stable IDs, deterministic ordering, seeded RNG, capture/restore, and idempotent event application.
- Current dirty/untracked user work unless the foreman explicitly assigns it to the package.
- Focused test discipline and the three-package concurrency ceiling.

### MUST ADD

- A written owner-disposition table for every package before editing.
- One typed production producer/consumer route per plan.
- Existing-section persistence for every unique stateful concern.
- Focused integration, save/replay, and host reachability proof.
- A truthful read model and observable outcome.
- Exact claim and acceptance evidence in the live foreman workflow.

### MUST NOT DO

- Do not wire a duplicate authority just because its unit tests pass.
- Do not create a new save section, resource ledger, quest lifecycle, inventory, world graph, relation total, fitness total, or campaign archive without an explicit architecture decision.
- Do not let panels calculate Core outcomes.
- Do not use `System.Random`, wall-clock seeds, random GUIDs, or unordered selection.
- Do not edit shared seams from builder packages.
- Do not modify `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, or `KNOWN_DEBT.md` unless acting as the named foreman/integrator.
- Do not activate this work while it overlaps the current live batch.

### VERIFY

- Existing focused test file alone, then package integration/save/host tests.
- Exact production caller under `src/`.
- Registered capture and setup/restore path.
- Legacy save and repeated-event idempotency.
- Same-seed replay where randomness or ordering exists.
- Data integrity for JSON changes.
- Focus/back/feedback/subscription disposal for Godot surfaces.
- `git diff --check` and a final claim-overlap audit.

### FIRST SAFE STEP

The foreman should activate only **Wave 1** after the current batch closes, record the present untracked files, and assign three disjoint claims: Plan 208 leadership Core/test work, Plan 143 affliction-duty Core/test work, and Plan 139 combat-faction Core/test work. A single integrator should separately own their shared host/save/event seams. Before any edit, each builder submits a one-page owner-disposition table showing which current fields and APIs are preserved, adapted, or removed.
