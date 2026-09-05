# ASHFALL — Unified Master Execution Plan

**Derived from:** `masterplans.md` (30 repository-grounded implementation plans, generated 2026-08-29)
**Purpose:** replace a large collection of overlapping plans with one dependency-ordered, execution-grade program.
**Status:** planning authority only. It is **not** a claim that any listed task is still incomplete in the current checkout.
**Execution rule:** current source, current data, current Git state, and current verification output override stale plan-era observations.

---

## 0. Executive directive

ASHFALL must converge on a single production architecture:

- **Godot is the active host.**
- **Core is engine-agnostic and owns gameplay rules.**
- **JSON is the gameplay/content data authority.**
- **The host presents, adapts, binds, and routes; it does not become a second simulation authority.**
- **Campaign persistence has one aggregate authority and one explicit compatibility/migration policy.**
- **Campaign-day progression has one clock and one transaction/tick authority.**
- **Survivors have one stable identity model and one live authority per component.**
- **Cross-domain changes use explicit typed results/events/revisions and lifecycle-safe subscriptions.**
- **Verification must prove current source, not stale assemblies or historical test totals.**
- **No Unity runtime/build path or removed bridge shim may be resurrected.**
- **Unrelated user WIP must be preserved. No broad reset/clean/checkout operation is an acceptable implementation technique.**

The source plan collection was intentionally based on a dirty local repository. Therefore every source-era count, failure, line number, percentage, and “current gap” is a **revalidation target**, not a permanent acceptance criterion.

---

## 1. Truth hierarchy

When authorities disagree, use this order:

1. current checked-out source and data;
2. current executable tests/selftests from a freshly built host;
3. current generated registries/manifests derived from source;
4. current architecture contracts and migration matrices;
5. this unified master plan;
6. the original `masterplans.md` evidence text;
7. historical reports, archived counts, old screenshots, comments, and prose.

Never alter source merely to make a historical report true.

### Mandatory stale-plan handling

Before executing any plan, classify it as:

- `ACTIVE` — source evidence shows the work remains necessary;
- `PARTIAL` — part is complete; execute only the unresolved acceptance gaps;
- `COMPLETE_PENDING_PROOF` — implementation appears complete but final gates/evidence are missing;
- `RETIRED` — current source already satisfies the plan and its relevant gates;
- `SUPERSEDED` — the responsibility is intentionally absorbed by another master workstream;
- `BLOCKED` — safe implementation requires an owner/architecture decision or protected-path clearance.

Do not reimplement a `RETIRED` plan.

---

## 2. Master completion chain

Every production capability must pass the same chain:

`DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → MUTATES → OBSERVED → PERSISTED → RESTORED → VERIFIED`

A loader, class, UI button, or passing compiler does **not** prove a feature is live.

For non-persistent derived/presentation state, replace `PERSISTED → RESTORED` with an explicit documented exemption and prove that the state is correctly reconstructed from authoritative persisted inputs.

---

## 3. Global non-negotiable invariants

### Architecture

1. One gameplay authority per domain.
2. Core rules do not migrate into Godot panels/nodes for convenience.
3. Godot adapters may assemble snapshots/providers but must not fork formulas.
4. No hidden construction from panel-open/bind paths.
5. No second campaign clock, inventory, survivor roster, radiation model, combat state, faction standing authority, narrative memory authority, or save authority.
6. Cross-system dependencies must be explicit and testable.
7. Optional dependencies fail visibly or use an explicit disabled adapter; they do not silently instantiate demo defaults in production.

### Determinism

1. All simulation randomness uses governed seeded RNG streams.
2. No process-randomized hashes, wall-clock values, unordered map iteration, or environment-specific IDs may influence deterministic state.
3. Same seed + same initial state + same command sequence must produce the same authoritative projection across fresh processes.
4. Save/reload must not alter the subsequent deterministic trajectory unless a documented migration intentionally does so.

### Persistence

1. Save capture is aggregate, versioned, checksummed, and deterministic.
2. A failed save/load/recovery cannot partially replace a known-good campaign.
3. Restore does not replay one-time consequences.
4. Persisted identity is stable across sessions and migrations.
5. New persisted fields require old-save behavior, migration/default semantics, round-trip proof, and deterministic ordering.
6. Derived UI/audio/cache state is not persisted unless it is genuinely gameplay state.

### Data/content

1. Canonical gameplay definitions live in JSON where the project contract says they do.
2. Static ID lists and generated constants are projections/validators, not second authorities.
3. Unknown IDs, mixed aliases, invalid versions, bad references, and malformed required catalogs fail clearly.
4. Existing IDs are not renamed without compatibility analysis.
5. Content utilization distinguishes parsing/loading from actual runtime reachability.

### Player-facing behavior

1. UI confirmation cannot report success without a verified authoritative mutation/revision.
2. Invalid commands return stable typed refusal/failure information.
3. Panels unsubscribe/rebind cleanly and never retain dead campaign/session references.
4. Accessibility, focus, keyboard/controller routing, feedback, and recovery are part of production acceptance.
5. Fallback presentation is explicit and bounded; release-critical content cannot silently use placeholders.

### Verification

1. Historical test totals are never acceptance criteria.
2. Report the exact tests/gates actually run.
3. A Godot selftest is trusted only after a current-source host build/import is proven in the same flow.
4. PASS, FAIL, BLOCKED, DIAGNOSTIC, and SKIPPED are distinct.
5. Performance exclusions and policy exemptions must be visible.
6. Verification may not be weakened to make a feature appear complete.

---

## 4. Execution organization — six-worker model

The plan assumes one coordinator and six logical lanes. One physical agent may execute several lanes sequentially, but write ownership must remain non-overlapping.

| Lane | Responsibility | Protected boundary |
|---|---|---|
| **W1 — Governance / Verification / Release** | verification authority, manifests, runners, replay proof, performance lane, export/release | does not implement gameplay to satisfy gates |
| **W2 — Persistence / Data / Assets** | save slots, envelopes/recovery, schema/content governance, asset manifest | does not own Main lifecycle or gameplay formulas |
| **W3 — Composition / Campaign Lifecycle / Events** | composition root, day transaction shell, typed command/result infrastructure, event lifecycle | sole lease authority for shared `Main` composition/lifecycle paths |
| **W4 — Core Simulation** | survivor identity/components, world, shelter, medical, skills, Utility AI Core integration | host remains thin; no Godot gameplay fork |
| **W5 — Gameplay Features / Player Surfaces** | economy, factions, narrative, expansions, expeditions, combat, UI, controls, audio | mutates Core through approved commands, never direct UI authority |
| **W6 — Independent Semantic Reviewer** | reviews each accepted slice, reruns targeted/canonical proof, classifies blockers | never implements the slice it reviews |

### Lease rules

- No two workers write the same path concurrently.
- Shared `src/Main.*` paths require a W3 lease even when the feature owner is W4/W5.
- Save registry/envelope/recovery files require W2 ownership.
- Verification policy/manifest changes require W1 ownership.
- Cross-lane changes are handed off as explicit integration slices rather than simultaneous editing.
- W6 has read-only review ownership and must be independent of implementation.
- If Task #131 or another current WIP owns a path, that path remains quarantined until G0 classifies it.

---

## 5. Consolidation decisions

The original 30 plans contain deliberate overlap. This master plan resolves it as follows.

### #4 and #25 — one Verification Program

Plan #25 is the governing verification authority. Plan #4 contributes mandatory requirements for:

- clean/dirty/current-source provenance;
- deterministic replay fingerprints;
- performance test inclusion policy;
- failure reporting/timeouts;
- local/CI parity.

Do **not** build two gate ecosystems.

### #9 and #17 — separate expedition logistics from tactical combat

- **#9 owns:** dispatch, travel, encounter transition, camp, retreat, return, loot deposit, expedition state machine.
- **#17 owns:** tactical combat readiness, action/effect authority, damage/morale/trauma/equipment consequences, in-combat save/restore.
- Integration boundary: encounter enters combat through one typed contract and receives one typed terminal combat result.

### #2 and #26 — providers before transaction

- **#2 owns:** live faction/economy provider freshness and bridge/event contracts.
- **#26 owns:** trade quote → validate → commit transaction and UI confirmation semantics.
- No provider or transaction formula is duplicated in the panel.

### #11 and #8 — identity before medicine

- **#11 owns:** survivor identity, roster/component lifecycle and typed-store migration.
- **#8 owns:** patient/radiation/disease/treatment continuum using that identity.
- Medical work cannot invent a parallel patient identity/state store.

### #5 with #16/#19/#24 — one final player-slice gate

Plans #16, #19, and #24 produce control/settings/accessibility, audio feedback, and asset coverage. Plan #5 is the integration/release-quality proof that these surfaces work with authoritative simulation. It must not become another independent UI architecture.

### #6/#13/#23/#14 — one campaign-day authority

- #6 owns the transaction and owner ordering.
- #13 owns world snapshot/evolution.
- #23 owns shelter operating graph.
- #14 owns expansion registry participation.
- Each registers into one day pipeline exactly once.

### #21 — revalidate the “Utility AI fork” claim

The master intent is to productionize Utility AI **without moving Core scoring authority into Godot**. If current source already has one Core scoring authority, preserve it and focus on real context assembly, action availability, typed commit, explanation, lifecycle, and replay. Do not invent a fictional second scoring rewrite just because older prose called it a fork.

---

## 6. Master workstreams

### Workstream A — Repository authority, lifecycle and command contracts

Establish one composition boundary, one lifecycle model, typed command outcomes, and lifecycle-safe event delivery. This is the structural spine for every later plan.

**Source plans:** #1, #10, #22

### Workstream B — Persistence, data authority and verification truth

Make save/recovery, JSON/content classification, current-source verification, replay evidence, and asset manifests authoritative. Plan #4 is absorbed into #25 rather than run as a competing verification framework.

**Source plans:** #18, #3, #25, #4, #24

### Workstream C — Campaign transaction and environmental simulation

Create one transactional day advance, one world snapshot, and one shelter operating graph. No duplicate clocks, ticks, or derived hazard authorities.

**Source plans:** #6, #13, #23

### Workstream D — Survivor state, health, skill and decision systems

Stabilize survivor identity/components first, then medical continuity and skills, then production Utility AI integration against those authoritative read models.

**Source plans:** #11, #8, #29, #21

### Workstream E — Economy, faction and trade integration

Make live faction providers and atomic economy transactions the base for Holdfast and PRPF faction loops.

**Source plans:** #2, #26, #7, #30

### Workstream F — Expedition, tactical combat and maritime operations

Separate expedition logistics ownership from tactical combat authority, then reuse both for deterministic maritime/Deep Coast operations.

**Source plans:** #9, #17, #27

### Workstream G — Narrative, expansions and endgame continuity

Unify narrative facts/projections, registry-drive expansions, and resolve one authoritative endgame/succession chronicle.

**Source plans:** #12, #14, #28

### Workstream H — Player control, presentation and playable-slice quality

Unify controls/settings/accessibility, audio feedback and asset coverage, then prove them through one coherent end-to-end player journey.

**Source plans:** #16, #19, #24, #5

### Workstream I — Performance and release

Measure only after correctness is trustworthy, then prove clean-clone reproducible exports and isolated-artifact execution.

**Source plans:** #15, #20

---

## 7. Dependency-ordered execution waves

### Wave 0 — G0 Repository truth freeze

**Purpose:** determine which source plans are still real before any implementation.

Required outputs:

1. exact branch/HEAD/source fingerprint;
2. complete dirty/staged/untracked/quarantined-path inventory;
3. current Task #131/composition status;
4. fresh Core build/test result and exact test inclusion policy;
5. fresh host build result;
6. current Godot import/build freshness status;
7. current save registry and schema/catalog counts derived from source;
8. current selftest/gate manifest crosswalk;
9. current plan status table (`ACTIVE`, `PARTIAL`, `COMPLETE_PENDING_PROOF`, `RETIRED`, `SUPERSEDED`, `BLOCKED`);
10. non-overlapping W1–W5 file leases and W6 review scope.

**Hard stop:** if current user-owned WIP cannot be safely isolated, classify the affected plans `BLOCKED`; do not “clean” the tree.

**G0 exit:** repository state is understood well enough that implementation can preserve current work and stale plan claims are not treated as facts.

### Wave 1 — G1 Authority backbone

Primary plans:

- **#25** verification authority begins and remains continuous;
- **#1** composition/lifecycle truth — execute only unresolved portions;
- **#10** typed result/diagnostic primitives and high-risk migrations;
- **#11** survivor identity/component lifecycle;
- **#22** event/subscription/revision lifecycle;
- read-only inventories for **#3**, **#18**, and **#24** may run in parallel.

Key outputs:

- one construction boundary;
- one lifecycle policy;
- stable typed command outcomes;
- one survivor identity boundary;
- detachable/scoped event delivery;
- current-source gate/fingerprint policy;
- no hidden duplicate authorities.

**G1 exit:** Core/host ownership is explicit enough for persistence and daily simulation to be changed safely.

### Wave 2 — G2 Persistence, data and evidence trust

Primary plans:

- **#18** save slots/corruption/recovery/profile isolation;
- **#3** JSON/content utilization governance;
- **#25/#4** verification/replay policy consolidation;
- **#24 Phase 0–1** asset graph and deterministic manifest;
- performance test lane policy from **#15** is decided, but optimization does not begin.

Key outputs:

- atomic slot inspection/recovery;
- current catalog classification and schema/reference evidence;
- no stale-green host selftests;
- one gate manifest/runner policy;
- asset/data authority inventories;
- visible test/performance inclusion.

**G2 exit:** a failed save/data/verification path cannot masquerade as success, and later simulation work has trustworthy recovery/evidence.

### Wave 3 — G3 Campaign simulation backbone

Recommended sequence:

1. **#6** campaign-day transaction;
2. **#13** world snapshot/evolution;
3. **#23** shelter operating graph;
4. **#8** medical/radiation/disease/treatment;
5. **#29** skill progression/competency;
6. continue #25 targeted verification throughout.

Key outputs:

- one day command and deterministic owner graph;
- one current world hazard answer per query;
- one shelter resource/failure pipeline;
- one patient continuum tied to canonical survivor identity;
- one skill authority shared by consumers;
- save/reload and same-seed replay remain stable.

**G3 exit:** the simulation can advance a day transactionally with world, shelter, survivor and patient state coherent and persisted.

### Wave 4 — G4 Cross-system contracts, economy, narrative and expansions

Recommended sequence/parallelism:

- **#2** live provider/bridge contracts;
- **#26** atomic trade transaction after #2 provider freshness is proven;
- **#12** narrative fact/projection pipeline;
- **#14** expansion registry/tick/save/UI ownership;
- **#21** Utility AI production integration after survivor/shelter/skill read models are stable.

Key outputs:

- live faction/economy context;
- quote/validate/commit trade semantics;
- idempotent narrative consequences;
- expansions tick/save exactly once;
- AI decisions select and commit through real typed commands;
- cross-domain delivery uses the event/revision lifecycle from #22.

**G4 exit:** cross-system reactions are real, deterministic, observable, and do not create duplicate mutation paths.

### Wave 5 — G5 Complete gameplay loops

Recommended order:

1. **#17** tactical combat production authority;
2. **#9** expedition logistics integration against #17;
3. **#27** maritime/Deep Coast operations;
4. **#7** Holdfast loop;
5. **#30** PRPF faction loop;
6. **#28** generational succession/endgame chronicle.

Where source evidence permits, independent read-only design/inventory phases may overlap, but mutation phases must honor leases.

**G5 exit:** the major campaign loops reach real authoritative mutation, player-visible outcomes, save/restore and deterministic proof.

### Wave 6 — G6 Player control and presentation integration

Primary plans:

- **#16** controls/settings/accessibility/onboarding;
- **#19** reactive audio;
- **#24** release-critical asset remediation/manifest enforcement;
- **#5** coherent end-to-end player vertical slice.

Key outputs:

- conflict-safe controls and settings recovery;
- presentation reacts to committed events without becoming authority;
- release-critical visual/audio resources resolve deliberately;
- panels refresh/rebind without leaks;
- one complete New Game → play → day advance → save → menu → Continue journey works through real Godot surfaces.

**G6 exit:** the game is not merely simulated correctly; the player can operate and understand the authoritative state.

### Wave 7 — G7 Performance, verification closure and release

Primary plans:

- **#15** measured performance hardening;
- **#25/#4** final current-source verification/replay closure;
- **#20** clean-clone reproducible export and isolated artifact proof.

Order:

1. capture stable correctness baselines;
2. optimize measured hotspots only;
3. rerun deterministic/save proofs after each optimization slice;
4. freeze release manifests;
5. clean-clone build/import/export;
6. execute the exported artifact with a fresh user-data directory;
7. W6 performs final semantic/release review.

**G7 exit:** clean source produces a reproducible artifact whose own runtime proof agrees with source/data/save/verification contracts.

---

## 8. Master dependency graph

```text
G0 TRUTH FREEZE
 |
 +--> #25 verification authority ------------------------------------------+
 |                                                                         |
 +--> #1 composition/lifecycle --> #10 typed results --> #22 events -------+----+
 |              |                    |                    |                    |
 |              +--> #11 survivor ---+--------------------+                    |
 |                                                                         |    |
 +--> #3 data/content --> #24 asset manifest                               |    |
 |                                                                         |    |
 +--> #18 persistence/recovery <-------------------------------------------+    |
                                                                                |
                 #6 day transaction <-------------------------------------------+
                    |
        +-----------+-------------+
        |                         |
       #13 world                  #14 expansion registry
        |                         |
     +--+------+                  +---------------------+
     |         |                                        |
    #23       #8 medical                                #12 narrative
     |         |                                        |
     +----+----+                                        |
          |                                             |
         #29 skills                                     |
          |                                             |
         #21 Utility AI                                 |
                                                        |
#2 live providers --> #26 trade --> #7 Holdfast --------+
       |                                                |
       +---------------------------------------------> #30 PRPF
                                                        |
#17 tactical combat --> #9 expedition --> #27 maritime |
                                                        |
#11/#12/#14/#29 -------------------------------------> #28 endgame

#16 controls + #19 audio + #24 assets + stable loops --> #5 vertical slice
#25 + stable correctness -----------------------------> #15 performance
#5 + #15 + #18 + #24 + #25 --------------------------> #20 release
```

This graph expresses minimum sequencing, not a ban on parallel read-only inventory or isolated Core tests.

---

## 9. Global slice execution protocol

Every implementation slice follows this loop:

### A. Preflight

- capture HEAD/status/diff scope;
- identify pre-existing WIP;
- verify current plan status and dependencies;
- acquire file leases;
- inspect current tests/contracts before editing;
- record the exact failure/gap being closed.

### B. Contract freeze

Before production changes, write down:

- authority/owner;
- input snapshot/provider;
- mutation boundary;
- event/revision semantics;
- persistence owner;
- failure/refusal semantics;
- deterministic RNG/ordering rules;
- UI/presentation contract;
- compatibility/migration behavior.

### C. Failing proof first

Add or identify the smallest regression/fault-injection test that demonstrates the gap. Do not use a broad rewrite as the first experiment.

### D. Core implementation

Implement domain rules/state in Core where applicable. Keep changes narrow and deterministic.

### E. Host integration

Bind live authoritative providers/commands. No demo fallback in production paths. No hidden service construction.

### F. Player surface

Present immutable/read-only projections. A UI success state must follow a real typed success and authoritative revision.

### G. Persistence/migration

Capture/restore new or changed state, preserve old saves, prove idempotency and deterministic ordering.

### H. Targeted verification

Run the smallest relevant tests first, then dependency/cross-domain gates.

### I. W6 semantic review

Review actual behavior and diff. Reject duplicate authority, stale-binary proof, false success, leaked subscriptions, undocumented data changes, hidden skips, or touched protected WIP.

### J. Canonical gate

Only after targeted proof passes may the slice advance to the next dependency wave.

---

## 10. Canonical verification baseline

Commands must be confirmed against the current repository before execution, but the source plan consistently requires the following proof family:

```bash
git status --short --untracked-files=all
git rev-parse HEAD

dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo
dotnet build Ashfall.csproj --nologo

godot --headless --path . --import
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Then run all plan-specific save, deterministic, lifecycle, UI, content, replay, performance, asset and release gates that are registered and current.

For every final report include:

- exact HEAD/source fingerprint;
- dirty-file state;
- commands and exit codes;
- actual passed/failed/skipped test counts;
- warning counts;
- performance inclusion/exclusion policy;
- current host/import fingerprint;
- plan-specific test IDs/flags;
- fresh-process proof where determinism/save/settings/audio/export behavior is claimed;
- remaining fallbacks/stubs/placeholders/exemptions;
- generated artifacts and whether they are authoritative or diagnostic.

No fixed historical total is accepted.

---

## 11. Master rollback and safety policy

1. Preserve unrelated user work.
2. Never use broad destructive cleanup as a repair strategy:
   - no `git reset --hard`;
   - no `git clean`;
   - no `git restore .`;
   - no broad checkout over user WIP.
3. Roll back one logical slice at a time.
4. Keep schema/save migration changes separable from UI polish.
5. Keep correctness changes separable from performance tuning.
6. Keep generated artifacts coupled to the source inputs that generate them.
7. A rollback must not resurrect a second authority, demo fallback, or stale save format.
8. Do not create commits unless the execution context explicitly requests commits.
9. If a current protected path conflicts with the plan, stop that slice and return `BLOCKED` with the exact path/owner decision required.

---

## 12. Portfolio risk register

| Risk | Failure mode | Required control |
|---|---|---|
| Stale plan baseline | agents redo already completed Task #131 or old fixes | G0 status classification; source wins |
| Duplicate authority | UI/host creates second simulation state | ownership contract + W6 review |
| Save corruption | partial generation or replayed consequence | atomic save/recovery + fault injection |
| Event leaks | rebind/reset accumulates callbacks | disposable scopes + lifecycle tests |
| Stale binaries | headless PASS from old host assembly | current-source build/import fingerprint |
| Data drift | JSON, constants and loaders disagree | schema/reference/utilization gates |
| Determinism drift | hash/order/wall-clock randomness changes outcomes | fresh-process replay fingerprints |
| False success | command/UI reports success without mutation | typed result + revision assertion |
| Hidden test loss | performance/domain tests silently excluded | explicit lane/source inclusion gates |
| Over-optimization | benchmark improvement breaks saves/determinism | correctness gates before/after tuning |
| Placeholder release | fallback assets/audio ship unintentionally | deterministic asset manifest/policy |
| Feature-local clocks | expansions/shelter/world tick twice | #6 owner graph and tick-once tests |
| Identity leakage | stale survivor references after load/death | #11 roster/component lifecycle gate |
| Restore side effects | load replays trade/faction/narrative consequences | restore projection is idempotent |
| Release checkout dependency | exported game reads repository data | isolated-artifact execution |

---

## 13. Integrated plan registry

The following registry is the authoritative mapping of all 30 source plans into this program. Dependencies are normalized by the unified architecture and should be revalidated at G0.

| # | Priority | Wave | Primary lane | Minimum prerequisites | Master role |
|---:|---|---|---|---|---|
| 1 | P0 | Wave 1 | W3 Composition/Lifecycle | G0 / none | Foundation; if Task #131 is already completed, revalidate and retire repair-only steps rather than redoing them. |
| 2 | P1 | Wave 4 | W5 Economy/Bridges | #1, #10, #11, #13, #22 | Provider/bridge contract. Must precede trade and faction-heavy loops. |
| 3 | P0 | Wave 1→2 | W2 Data/Persistence | #1, #25 | Read-only corpus census can start at G0; mutating data work waits for current-source verification. |
| 4 | P0 | Continuous | W1 Verification | #1 | Consolidated into the authoritative Verification Program with #25; retain replay/performance requirements, do not build a second gate framework. |
| 5 | P1 | Wave 6 | W5 Player Surfaces | #6, #10, #16, #18, #19, #24, #25 | Acts as the integrated playable-slice release gate, not a new simulation workstream. |
| 6 | P0 | Wave 3 | W3 Campaign Transaction | #1, #10, #18, #22, #25 | Establishes the one-day transaction/tick authority used by world, shelter and expansions. |
| 7 | P1 | Wave 5 | W5 Holdfast | #2, #6, #10, #12, #18, #26 | Feature loop after economy, day, narrative and persistence contracts are stable. |
| 8 | P1 | Wave 3 | W4 Medical/Survivors | #6, #10, #11, #13, #18, #22 | Clinical continuum depends on canonical survivor identity and world exposure. |
| 9 | P1 | Wave 5 | W5 Expedition Logistics | #6, #10, #11, #13, #17, #18, #22 | Owns expedition state machine/logistics; #17 owns tactical combat internals. |
| 10 | P0 | Wave 1 | W3 Command Contracts | #1 | Cross-cutting typed result/diagnostic contract; implement narrowly and migrate highest-risk commands first. |
| 11 | P0 | Wave 1 | W4 Survivor State | #1, #25 | Canonical identity/component lifecycle. Required before medical, AI, skills and several UI projections. |
| 12 | P1 | Wave 4 | W5 Narrative Progression | #3, #10, #11, #18, #22 | One idempotent narrative fact/projection pipeline. |
| 13 | P0 | Wave 3 | W4 World Simulation | #3, #6, #10, #18, #22 | One deterministic world snapshot consumed by expedition, shelter and medical. |
| 14 | P1 | Wave 4 | W5 Expansion Registry | #1, #3, #6, #10, #18, #22, #25 | Registry-driven expansion composition/tick/save/UI contract. |
| 15 | P1 | Wave 7 | W1 Performance | #4, #25 | Measure after correctness architecture is stable; never optimize by weakening tests or determinism. |
| 16 | P1 | Wave 6 | W5 Controls/Settings | #1, #10, #18, #25 | Player preference/control transaction and recovery contract. |
| 17 | P1 | Wave 5 | W5 Tactical Combat | #10, #11, #13, #18, #22 | Owns combat authority/effects. Coordinate boundary with #9; do not duplicate expedition state. |
| 18 | P0 | Wave 2 | W2 Persistence | #1, #10, #25 | Save-slot inspection, validation, atomic recovery and profile isolation. |
| 19 | P1 | Wave 6 | W5 Audio | #16, #22, #24 | Presentation-only event-to-audio feedback; no gameplay authority. |
| 20 | P0 Release | Wave 7 | W1 Release | #5, #15, #18, #24, #25 | Final clean-clone/export/isolated-artifact proof. |
| 21 | P1 | Wave 4 | W4 Utility AI | #10, #11, #22, #23, #29 | Productionize Core-authoritative decision integration. Revalidate any stale 'fork' claim before refactoring scoring. |
| 22 | P0 | Wave 1 | W3 Event Lifecycle | #1, #10 | Lifecycle-safe delivery/revision contract; incremental migration, not a flag-day event-bus rewrite. |
| 23 | P0 | Wave 3 | W4 Shelter OS | #3, #6, #11, #13, #22 | Integrated shelter dependency graph under the one campaign-day owner. |
| 24 | P1 | Wave 2→6 | W2 Assets/Data | #3, #25 | Inventory/manifest can run early; release-critical asset remediation can proceed in parallel. |
| 25 | P0 | Continuous | W1 Verification | G0 / none | Governing verification authority. Starts at G0 and remains mandatory through release. |
| 26 | P1 | Wave 4→5 | W5 Economy | #2, #10, #18, #22 | Atomic trade transaction. #2 supplies live providers; #26 owns quote/validate/commit. |
| 27 | P1 | Wave 5 | W5 Maritime | #9, #10, #13, #18, #22 | Deterministic maritime/Deep Coast operation loop; tactical combat dependency only where route enters combat. |
| 28 | P1 | Wave 5 | W5 Endgame | #6, #11, #12, #14, #18, #29 | Authoritative outcome/succession/chronicle pipeline. |
| 29 | P1 | Wave 3→4 | W4 Skills | #10, #11, #18, #22 | Shared skill identity, XP and consumer effects; feed Utility AI rather than host heuristics. |
| 30 | P1 | Wave 5 | W5 PRPF/Factions | #2, #3, #6, #10, #12, #18, #22 | Authored deterministic faction branch with reachability, reload-farming resistance and save continuity. |

---

## 14. Detailed execution cards

### Plan #1 — Finish and seal the campaign composition root

**Program placement:** Wave 1 · **Priority:** P0 · **Primary lane:** W3 Composition/Lifecycle

**Prerequisites:** G0 baseline only

**Master interpretation:** Foundation; if Task #131 is already completed, revalidate and retire repair-only steps rather than redoing them.

**Outcome from the source plan:**

Turn the current `Main` composition-root WIP into a compiling, deterministic,
single-owner campaign startup/lifecycle path. Every campaign service should be
constructed exactly once, in dependency order, and reset/rebuilt safely for
New Game, Continue, Return to Menu, and reload. Panels must consume
already-composed services and must not become hidden composition roots.

**Architecture reconciliation (resolved):** The original wording above
referenced a typed `CampaignServices` container as the composition authority.
That container never existed in the repository (a stale claim also present in
`docs/forensics/survivor_aggregate_FORENSIC_REPORT.md`, already flagged as
incorrect by `docs/architecture/survivor_identity_inventory.json`). The actual,
currently-passing canonical gate
(`Ashfall.Core.Tests/UI/CompositionRootArchitectureGateTests.cs`) asserts and
enforces that `Main.ComposeCampaign()` in `src/Main.CampaignServices.cs` — a
plain method, not a container class — is the single authoritative composition
root. No second composition model exists or is required; this plan card is
updated to match the repository rather than the reverse. The two dead
placeholder methods `ConstructSettlementBuilder()`/`ConstructScavengerBuilder()`
(no-ops with no referencing test, data catalog, or expansion system) have been
retired from `ComposeCampaign()`.

**First safe step:**

Capture current Git/build state, inspect the exact Task #131/composition diff, and decide whether Plan #1 is still active, partially complete, or retired before touching composition files.

**Completion evidence:**

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes with the
  intended test set explicitly reported.
- `dotnet build Ashfall.csproj` passes with 0 errors and 0 warnings after a
  clean/incremental warning comparison.
- `scripts/ci/triad-drift-gate.sh` passes.
- `--playable-shell-selftest`, `--day1-selftest`, and
  `--panel-bind-lifecycle-selftest` pass from the freshly built host.
- New Game/Continue/Menu isolation and save capture are covered by automated
  tests.
- Still outstanding: a real Main-composed New Game → day advance → SaveAll →
  Continue → restored-state journey (tracked under Plan #5), since the
  existing standalone playable-shell/Day-1 selftests construct host sessions
  directly rather than driving them through `ComposeCampaign()`.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #2 — Complete live cross-system provider wiring and bridge contracts

**Program placement:** Wave 4 · **Priority:** P1 · **Primary lane:** W5 Economy/Bridges

**Prerequisites:** #1, #10, #11, #13, #22

**Master interpretation:** Provider/bridge contract. Must precede trade and faction-heavy loops.

**Outcome from the source plan:**

Make cross-system effects reflect live campaign state instead of construction-time
snapshots or default provider values. The first vertical target is the Silent
Foundry Guild’s faction stance and trade access, followed by a contract audit of
the existing shelter/world/narrative bridges. Every bridge must have one source of
truth, explicit event ordering, deterministic behavior, persistence coverage, and
player-visible feedback.

**First safe step:**

Write the provider/bridge matrix first: producer, consumer, freshness rule, event trigger, persistence owner, and runtime proof for each claimed bridge.

**Completion evidence:**

- No `GAP-STUB-03` remains without an explicit, approved reason.
- Day and radiation provider values are live, not construction-time snapshots.
- Faction classification does not fork authoritative JSON IDs.
- Foundry consequences are once-only and save-safe.
- Existing bridge paths are classified and tested from producer to consumer.
- `--silent-foundry-selftest`, economy/trade UI tests, relevant bridge selftests,
  and the full Core suite pass after the host rebuild.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #3 — Govern the JSON corpus and close content reachability gaps

**Program placement:** Wave 1→2 · **Priority:** P0 · **Primary lane:** W2 Data/Persistence

**Prerequisites:** #1, #25

**Master interpretation:** Read-only corpus census can start at G0; mutating data work waits for current-source verification.

**Outcome from the source plan:**

Make every authoritative catalog intentional: gameplay-consumed, UI-consumed,
codex-only, optional/infrastructure, test-only, or explicitly unresolved with an
owner and rationale. Then close genuine orphan paths so authored content can be
reached from Core state through a live Godot host surface. The task is about
content utilization and data authority, not bulk content generation.

**First safe step:**

Generate a current corpus/utilization inventory and classify discrepancies before editing JSON or baselines.

**Completion evidence:**

- A current report covers all 411 JSON files.
- No required catalog is `UNRESOLVED` without a named owner and follow-up plan.
- Every promoted gameplay/UI/codex catalog has a real consumer and runtime probe.
- Narrative graph and continuity checks pass for touched content.
- `--data-integrity-selftest` reports zero errors.
- `--content-utilization-selftest` fails on an injected unapproved orphan and
  passes with the reviewed baseline.
- No engine-specific or duplicate data authority is introduced.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #4 — Restore a trustworthy verification, deterministic replay, and performance pipeline

**Program placement:** Continuous · **Priority:** P0 · **Primary lane:** W1 Verification

**Prerequisites:** #1

**Master interpretation:** Consolidated into the authoritative Verification Program with #25; retain replay/performance requirements, do not build a second gate framework.

**Outcome from the source plan:**

Make the repository’s quality claims reproducible from a clean checkout and make
the test suite prove behavior rather than merely compilation. The finished task
must distinguish Core tests, host build, headless runtime gates, deterministic
replay, and performance budgets. It must not silently exclude test directories or
allow a selftest to print PASS after an unhandled failure.

**First safe step:**

Crosswalk current manifests, runners, selftests, warning policy, performance inclusion, and build freshness before changing CI behavior.

**Completion evidence:**

- A clean host build passes before any Godot selftest is trusted.
- Manifest and implementation are bijective for all critical selftests.
- The performance suite is either included or explicitly run in a named CI lane;
  no `Compile Remove` silently hides it.
- Full rebuild warning policy is stable and the sanctioned wall-clock abstraction
  is handled consistently.
- Same-seed replay hashes match across fresh runs and save/reload boundaries.
- Fault injection yields a named failure, nonzero exit, and artifact.
- The canonical local runner and GitHub Actions consume the same manifest.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #5 — Ship a player-facing vertical slice with coherent UI state, accessibility, and feedback

**Program placement:** Wave 6 · **Priority:** P1 · **Primary lane:** W5 Player Surfaces

**Prerequisites:** #6, #10, #16, #18, #19, #24, #25

**Master interpretation:** Acts as the integrated playable-slice release gate, not a new simulation workstream.

**Outcome from the source plan:**

Deliver and verify one coherent player journey from New Game through several day
advances, shelter management, a meaningful expedition or event, save/reload, and
an end-state surface. The UI should read authoritative state through stable host
projections, route actions through Core commands, refresh predictably, remain
accessible by keyboard/focus, and provide visual/audio feedback for success,
warning, and failure.

This is a product-quality integration task, not a request to add 30 more panels.

**First safe step:**

Freeze one representative player journey and map every action to its Core mutation, host projection, save section, panel refresh, and verification proof.

**Completion evidence:**

- The selected New Game → action → day advance → save → Continue journey passes
  on the real Godot scene and in a headless selftest.
- Panels display authoritative state and update without reopen/reconstruct.
- Bind/unbind/rebind has no duplicate handlers or orphan nodes.
- All slice actions have keyboard/focus access and no conflicting primary shortcut.
- H-key Help/Holdfast conflict is resolved and regression-tested.
- Default/warning/error/restored snapshots are generated and reconciled with the
  coverage document.
- Asset and audio selftests pass with no unexpected missing/fallback entries.
- Corrupt/missing save flows are recoverable and clearly communicated.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #6 — Make campaign-day advancement a production-grade transaction and briefing pipeline

**Program placement:** Wave 3 · **Priority:** P0 · **Primary lane:** W3 Campaign Transaction

**Prerequisites:** #1, #10, #18, #22, #25

**Master interpretation:** Establishes the one-day transaction/tick authority used by world, shelter and expansions.

**Outcome from the source plan:**

Every player day advance becomes one auditable transaction:

`player command → validation → pre-day snapshot → deterministic owner tick order → typed report → one durable campaign save → briefing projection → UI acknowledgement`

The implementation must guarantee that a failed day cannot be presented as a
successful day, cannot partially persist a new generation, and cannot double-tick
an owner on retry. The daily briefing must describe the same committed report
that was persisted, rather than independently recomputing a potentially
different result.

**First safe step:**

Map and lock the active `_campaignDay.Advance` call graph before changing any owner behavior.

**Completion evidence:**

- Exactly one active Godot day-advance entry point exists.
- Every mutable production owner has a real snapshot/restore strategy or is
  explicitly moved behind a transaction-safe aggregate.
- A successful turn persists once, advances the calendar once, emits one
  committed event, and shows one matching briefing.
- A failed turn does not advance the day or replace the previous envelope.
- Same-seed clean run and retry-after-failure produce identical fingerprints.
- The real owner graph is printed and tested in deterministic order.
- Relevant Core, host, save, UI, data, and canonical headless gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #7 — Complete the Holdfast operational survival and economy loop

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 Holdfast

**Prerequisites:** #2, #6, #10, #12, #18, #26

**Master interpretation:** Feature loop after economy, day, narrative and persistence contracts are stable.

**Outcome from the source plan:**

Turn the Holdfast feature from a collection of functioning Core systems and
terminal actions into one coherent, player-facing S1 loop:

`unlock → establish the holdfast → manage census/levy → operate brine and ice road → trade and provision → survive daily deterioration → resolve quest branches → reach a documented ending`

The loop must use one authoritative inventory/economy state, connect the daily
turn to the Holdfast systems, expose meaningful consequences in the terminal and
dashboard, and persist/reload without resetting progress.

**First safe step:**

Trace the rendered Holdfast controls and map each command to its actual Core mutation and inventory object before changing behavior.

**Completion evidence:**

- Holdfast actions mutate the same canonical inventory/economy objects used by
  the rest of the game.
- The ice-road, census, brine, trade, quest, and ending loop advances through the
  real campaign day owner exactly once.
- No visible “not wired” claim remains for a capability that is supposed to be
  live.
- Every accepted action produces a typed result, state event, UI update, and
  dirty-save signal.
- All S1 branches/ends are reachable or explicitly marked design-only with an
  owner and data contract.
- Save/reload, old-save migration, checksum, deterministic, and headless loop
  tests pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #8 — Complete the patient, radiation, disease, and treatment continuum

**Program placement:** Wave 3 · **Priority:** P1 · **Primary lane:** W4 Medical/Survivors

**Prerequisites:** #6, #10, #11, #13, #18, #22

**Master interpretation:** Clinical continuum depends on canonical survivor identity and world exposure.

**Outcome from the source plan:**

Create one understandable and technically coherent care loop:

`exposure → dose/needs state → symptoms/affliction → diagnosis → treatment/resource consumption → recovery or chronic outcome → daily report → save/reload`

The result must connect the existing medical ward, disease expansion, dose
ledger, radiation, survivor needs, respiratory degeneration, chemical dependency,
and inventory systems without forking patient state or placing medical rules in
Godot panels.

**First safe step:**

Build the patient field ownership table and trace one iodine/anti-rad click from button to inventory mutation, patient mutation, event, and save dirty flag.

**Completion evidence:**

- One canonical patient state is read by every medical/radiation surface.
- Daily exposure, disease, needs, ward, treatment, and death ordering is
  documented and tested.
- Treatment commands are atomic, deterministic where randomized, and return
  stable refusal codes.
- Needs/Radiation and medical ward state survive deep save/reload.
- The patient UI displays real state and explains unavailable actions.
- Missing/dead/quarantined patients, corrupt saves, zero stock, and repeated
  actions are safe and visible.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #9 — Seal the expedition → encounter → combat → return logistics loop

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 Expedition Logistics

**Prerequisites:** #6, #10, #11, #13, #17, #18, #22

**Master interpretation:** Owns expedition state machine/logistics; #17 owns tactical combat internals.

**Outcome from the source plan:**

Make expeditions a complete gameplay loop rather than separate travel, encounter,
combat, and camp APIs:

`dispatch → real roster/equipment preparation → deterministic travel → encounter → tactical combat or choice → loot/wear/trauma → return/retreat → inventory deposit → save/reload`

The production path must use live survivors, canonical equipment condition, real
encounter/location data, and the same campaign clock. Demo fallbacks may remain
in explicitly named harnesses but must not silently supply production combatants
or weapons.

**First safe step:**

Trace one real dispatch through every expedition/combat callback and document the first state owner at each transition before editing APIs.

**Completion evidence:**

- Dispatch, travel, encounter, combat, camp, retreat, return, and loot are one
  legal state machine with no double-resolution paths.
- Production combat never uses demo survivors/weapons or unbound required ports.
- Weapon condition, ammo, trauma, morale, health, and loot write back exactly
  once to authoritative systems.
- Active travel/encounter/combat/camp/pending-return saves restore correctly.
- Same seed gives identical full-route state; different seed can diverge.
- The real Godot panels and headless selftest complete the route.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #10 — Replace silent command failures with typed results and actionable diagnostics

**Program placement:** Wave 1 · **Priority:** P0 · **Primary lane:** W3 Command Contracts

**Prerequisites:** #1

**Master interpretation:** Cross-cutting typed result/diagnostic contract; implement narrowly and migrate highest-risk commands first.

**Outcome from the source plan:**

Make runtime failures explainable, testable, and recoverable across the most
important player commands. A failed operation must identify what was refused,
whether state changed, whether a retry is safe, and what the UI should display.

The objective is not noisy logging. It is a consistent contract that prevents
false success, silent no-op ports, hidden loader/save failures, and unhelpful
string parsing across campaign, save/load, trade, medical, expedition, combat,
and shelter operations.

**First safe step:**

Inventory the first 20 high-risk command methods and record whether each can return success without a verified state mutation or durable write.

**Completion evidence:**

- Prioritized high-risk commands expose typed, stable results and no longer rely
  on prose parsing.
- Save/load, campaign advance, combat readiness, medical treatment, trade,
  expedition return, and shelter resource failures are visible and retry-safe.
- Required unbound providers fail closed before mutation.
- CI catches ignored results/unconditional success in the governed paths.
- Diagnostics are structured, bounded, redacted, and deterministic.
- Existing intentional validation refusals remain valid and are not converted
  into noisy errors.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #11 — Complete survivor identity, typed component migration, and roster lifecycle

**Program placement:** Wave 1 · **Priority:** P0 · **Primary lane:** W4 Survivor State

**Prerequisites:** #1, #25

**Master interpretation:** Canonical identity/component lifecycle. Required before medical, AI, skills and several UI projections.

**Outcome from the source plan:**

Create one authoritative survivor identity and lifecycle boundary from roster
creation through hourly simulation, death/fate/social changes, save capture,
slot restore, and UI projection. Complete the current typed Needs component
migration without creating a second gameplay authority. A survivor must have one
stable canonical ID, one live component owner per component type, and one
well-defined lifecycle when a campaign is loaded, replaced, or ended.

**First safe step:**

finish the producer/consumer ID inventory and run the existing typed-store/parity tests before changing the authority switch.

**Completion evidence:**

- One canonical identity lookup works across all audited survivor systems.
- The typed component authority passes parity across load, tick, mutation, and
  restore before legacy writes are removed.
- Roster replacement is atomic from the perspective of Core and UI consumers.
- Save compatibility is proven for old and new formats with checksummed campaign
  envelopes and deterministic output.
- No host-only gameplay authority is introduced.
- The survivor selftest and the full canonical verification checklist pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #12 — Unify narrative memory and progression across quests, flags, radio, journal, and knowledge

**Program placement:** Wave 4 · **Priority:** P1 · **Primary lane:** W5 Narrative Progression

**Prerequisites:** #3, #10, #11, #18, #22

**Master interpretation:** One idempotent narrative fact/projection pipeline.

**Outcome from the source plan:**

Build one reliable narrative progression pipeline in which an authored or
system-generated event can update flags, knowledge, quest state, radio history,
journal entries, and player-facing UI exactly once, in a deterministic order,
with save/load continuity. The goal is not to rewrite narrative content; it is
to make the existing content produce durable, explainable consequences.

**First safe step:**

generate the producer/consumer/event/save matrix and add a failing duplicate-application test before moving any existing handler.

**Completion evidence:**

- Representative narrative, quest, radio, knowledge, and journal paths use one
  typed, idempotent progression contract.
- Same-day and same-seed runs produce identical ordered facts and projections.
- Save/load and repeated restore do not duplicate or lose consequences.
- Radio history, triangulation, quest state, and journal state retain their
  intended distinctions.
- Player-facing UI shows committed outcomes and actionable refusals.
- Reachability and headless progression gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #13 — Make world hazards, map routes, and environmental evolution one authoritative simulation

**Program placement:** Wave 3 · **Priority:** P0 · **Primary lane:** W4 World Simulation

**Prerequisites:** #3, #6, #10, #18, #22

**Master interpretation:** One deterministic world snapshot consumed by expedition, shelter and medical.

**Outcome from the source plan:**

Integrate weather, fallout, nuclear-winter conditions, visibility, outdoor
radiation, hydro-geology, evolving locations, wildlife, landmarks, maritime and
deep-coast state, and map route validation into one deterministic world snapshot
that drives expeditions, shelter exposure, encounters, medical dose, and UI.
The world should not be a collection of independently ticking systems with
slightly different answers for the same day and location.

**First safe step:**

produce the world producer/consumer/RNG/tick matrix and write one failing test asserting expedition and shelter receive the same hazard snapshot for a fixed seed/day/location.

**Completion evidence:**

- One world snapshot answers all audited hazard/map queries.
- Same seed/day/location/route produces identical values across processes.
- Expedition, encounters, shelter, medical, and UI consume the shared authority.
- World evolution persists and migrates correctly without duplicate ticking.
- Invalid/stale queries fail explicitly and safely.
- Scenario, save, determinism, performance, and headless world gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #14 — Productionize the expansion suite and eliminate canonical, tick, and save contract drift

**Program placement:** Wave 4 · **Priority:** P1 · **Primary lane:** W5 Expansion Registry

**Prerequisites:** #1, #3, #6, #10, #18, #22, #25

**Master interpretation:** Registry-driven expansion composition/tick/save/UI contract.

**Outcome from the source plan:**

Make the expansion suite a registry-driven, production-ready composition with
one canonical expansion list, one construction path, one daily-tick authority,
one save aggregation path, and explicit cross-expansion dependency contracts.
Every enabled expansion must be reachable from data and UI, tick exactly once,
persist its state, restore it, and expose a meaningful player-facing result.

**First safe step:**

reconcile the exact-ten test with the 01–11 generator/data claim and publish the canonical registry decision before code movement.

**Completion evidence:**

- The canonical registry decision is documented and mechanically enforced.
- Every enabled expansion has one construction, tick, save, restore, and UI
  ownership path.
- A complete day advances each expansion exactly once.
- Aggregate save/load round-trips all enabled expansion state atomically.
- Cross-expansion dependencies and failures are typed and observable.
- Expansion completeness, data integrity, save, and headless suite gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #15 — Make startup, day-tick, UI refresh, asset lookup, and persistence scale trustworthy

**Program placement:** Wave 7 · **Priority:** P1 · **Primary lane:** W1 Performance

**Prerequisites:** #4, #25

**Master interpretation:** Measure after correctness architecture is stable; never optimize by weakening tests or determinism.

**Outcome from the source plan:**

Establish measured performance budgets for startup, catalog loading, campaign
day advancement, UI projection, save capture, save load, and representative
large-roster/large-world scenarios. Restore the currently excluded performance
test lane deliberately, remove the highest-impact allocation and repeated-work
hotspots, and prove optimizations preserve determinism and save correctness.

**First safe step:**

resolve the `Performance/*` compile policy and capture a clean baseline for one normal and one stress workload before tuning code.

**Completion evidence:**

- Performance test inclusion/exclusion is explicit, observable, and documented.
- Budgets exist for startup, day tick, UI projection, save/load, memory, and
  stress scale, with baseline artifacts.
- Top measured hotspots have improvements backed by before/after evidence.
- Asset/catalog caches and UI invalidation have tests and bounded lifetimes.
- Optimized runs preserve seeded state, event, projection, and save determinism.
- Full canonical verification and the performance lane pass with no hidden skip.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #16 — Ship a coherent player control plane: input rebinding, settings, accessibility, and onboarding

**Program placement:** Wave 6 · **Priority:** P1 · **Primary lane:** W5 Controls/Settings

**Prerequisites:** #1, #10, #18, #25

**Master interpretation:** Player preference/control transaction and recovery contract.

**Outcome from the source plan:**

Deliver one reliable player-preference and control layer covering keyboard,
mouse, controller, navigation, rebinding, display, audio preferences,
accessibility, onboarding assistance, and safe recovery from malformed settings.
A preference must be represented once, applied consistently at runtime, saved
atomically, restored on a fresh process, and reflected in every affected UI
surface. A player must be able to discover, change, cancel, reset, and recover
controls without becoming trapped or losing access to confirmation/cancel.

**First safe step:**

complete the action/field/store/consumer matrix and add a test proving a custom keyboard binding cannot remove the confirm/cancel escape route.

**Completion evidence:**

- One documented preference and action authority exists, with migrations for
  existing files and no silent duplicate audio authority.
- Rebinding supports conflict detection, defaults, persistence, and recovery.
- Confirm/cancel/navigation always remain available after bad input data.
- Accessibility settings reach all governed panels and preserve focus/legibility.
- Onboarding signals are emitted by real domain actions and survive restore.
- Settings, input, accessibility, onboarding, and canonical verification gates
  pass on a fresh process.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #17 — Close tactical combat authority, effect binding, and encounter resolution

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 Tactical Combat

**Prerequisites:** #10, #11, #13, #18, #22

**Master interpretation:** Owns combat authority/effects. Coordinate boundary with #9; do not duplicate expedition state.

**Outcome from the source plan:**

Make tactical combat a production-authoritative Core domain from encounter
creation through targeting, stance, fire/suppress/repair/move/trap/bandage/
retreat, damage, morale, trauma, weapon wear, loot, survivor lifecycle, save,
restore, and UI. Production combat must never silently run on demo roster,
no-op ports, literal ammo, or unbound effects. Every accepted action must have a
typed result, a deterministic state mutation, an observable combat event, and a
durable outcome when the encounter ends.

**First safe step:**

turn the current unbound-port log into a failing production readiness test while keeping the isolated Core demo explicitly available.

**Completion evidence:**

- Production combat cannot start without required real providers.
- Every action has stable typed success/refusal semantics and deterministic
  mutation/event behavior.
- Survivor, inventory, medical/trauma, equipment, expedition, and narrative
  outcomes commit exactly once.
- Save/load works at in-flight and terminal phases without duplication.
- Demo behavior is isolated to an explicit demo/selftest entry point.
- Combat UI and all combat verification gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #18 — Make save slots, corruption recovery, profile isolation, and restore UX production-safe

**Program placement:** Wave 2 · **Priority:** P0 · **Primary lane:** W2 Persistence

**Prerequisites:** #1, #10, #25

**Master interpretation:** Save-slot inspection, validation, atomic recovery and profile isolation.

**Outcome from the source plan:**

Deliver a save-slot subsystem that protects player campaigns through atomic
capture, validated manifests, backup selection, corruption quarantine, legacy
import, profile/slot isolation, terminal-run policy, and truthful UI status. A
failed read or restore must preserve the live session. A successful load must
restore one validated campaign generation without hidden partial disk rewrites,
stale slot roots, or ambiguous “exists but invalid” cards.

**First safe step:**

implement the slot-state inspection matrix against the existing in-memory file test double before changing publication behavior.

**Completion evidence:**

- Slot inspection distinguishes missing, valid, corrupt, recoverable, legacy,
  manifest-only, and terminal states.
- Core and host filesystem access is isolated behind the approved boundary.
- Aggregate, manifest, and backup generations are atomic and validated.
- Failed load/recovery preserves live state and slot-root correctness.
- Recovery and import are explicit, idempotent, and player-visible.
- Slot isolation, fault injection, save compatibility, and headless UX gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #19 — Turn audio conditions and domain events into coherent, reactive, recoverable game feedback

**Program placement:** Wave 6 · **Priority:** P1 · **Primary lane:** W5 Audio

**Prerequisites:** #16, #22, #24

**Master interpretation:** Presentation-only event-to-audio feedback; no gameplay authority.

**Outcome from the source plan:**

Create one diegetic audio feedback layer that maps committed domain events and
state revisions to music, ambience, radio, alerts, UI, medical, generator,
ventilation, and surface cues without duplicates, stale subscriptions, or
unbounded resource growth. Active audio conditions must reconcile after scene
reload, settings change, slot load, pause/resume, and headless execution. Audio
must communicate survival state clearly while remaining presentation-only and
never becoming a gameplay authority.

**First safe step:**

add a repeated subscribe/unsubscribe test for `AudioEventBridge` and `AudioManager` before expanding event coverage.

**Completion evidence:**

- Every governed event has a documented cue/intent policy, including intentional
  silence and missing-resource behavior.
- Audio bridges can subscribe/unsubscribe/rebind without duplicates or leaks.
- Conditions restore into correct playback state without replaying history.
- User preferences control all buses through one effective authority.
- Cache/pool behavior is bounded and measured.
- Audio, headless, scene lifecycle, save/load, and full verification gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #20 — Make Godot release exports reproducible, self-contained, and policy-consistent

**Program placement:** Wave 7 · **Priority:** P0 Release · **Primary lane:** W1 Release

**Prerequisites:** #5, #15, #18, #24, #25

**Master interpretation:** Final clean-clone/export/isolated-artifact proof.

**Outcome from the source plan:**

Create a release pipeline that produces verifiable Linux and Windows Godot
artifacts from a clean clone, packages the JSON data authority and imported
assets correctly, executes the exported artifact’s smoke gates, emits a
machine-readable manifest/checksums, and rejects stale or contradictory policy.
The release job must prove the artifact works independently of the source
checkout. All normal verification remains .NET + Godot; no Unity command or
Unity-only artifact is part of the active release path.

**First safe step:**

run a read-only workflow/export/data-resolver inventory and write the artifact manifest schema before changing staging or CI behavior.

**Completion evidence:**

- Clean clone → build/import → export → isolated artifact run is automated for
  every supported release platform.
- Release artifact contains and resolves the authoritative JSON/assets without
  source-checkout dependency.
- Manifest, hashes, version/build identity, package inventory, and exit results
  are uploaded and internally consistent.
- Exported artifact passes data, startup, save/load, and representative headless
  selftests with a fresh user-data directory.
- Stale Unity policy is removed or explicitly historical without violating the
  Godot-only rules.
- Release, fast CI, data integrity, and clean-room gates pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #21 — Promote Utility AI from demo fork to the authoritative survivor decision pipeline

**Program placement:** Wave 4 · **Priority:** P1 · **Primary lane:** W4 Utility AI

**Prerequisites:** #10, #11, #22, #23, #29

**Master interpretation:** Productionize Core-authoritative decision integration. Revalidate any stale 'fork' claim before refactoring scoring.

**Outcome from the source plan:**

Turn `Assets/Ashfall.Core/UtilityAI/` plus
`src/Host/UtilityAiHostSession.cs` from an isolated selection demo into a
production decision service used by real survivor work, duty, medical,
expedition, crisis, and optional autonomous actions. The Core owns context
contracts, scoring, availability, deterministic choice, and action results. The
Godot host only assembles snapshots, exposes player commands, and presents
explanations. Every selected action must either commit through a typed Core
command or be rejected with a visible reason; a successful score alone must
never mutate gameplay.

**First safe step:**

produce the decision-path inventory and current demo fixture; do not begin by rewriting `UtilityAiSystem`.

**Completion evidence:**

- At least one real survivor action traverses context, deterministic selection,
  typed commit, event notification, UI observation, save, and restore.
- All other integrated action domains either use the same command path or are
  explicitly marked proposal-only; no hidden host heuristic remains.
- Identical seed, snapshot, catalog, and command sequence produces identical
  decision traces and Core state.
- Every rejected action exposes a stable reason code and actionable message.
- `UtilityAiHostSession` is a thin adapter rather than a synthetic gameplay
  implementation.
- Canonical .NET + Godot verification and the new Utility AI selftest pass.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #22 — Unify domain events, subscription lifecycle, and revision delivery

**Program placement:** Wave 1 · **Priority:** P0 · **Primary lane:** W3 Event Lifecycle

**Prerequisites:** #1, #10

**Master interpretation:** Lifecycle-safe delivery/revision contract; incremental migration, not a flag-day event-bus rewrite.

**Outcome from the source plan:**

Create one dependable Core-to-Godot event delivery contract for cross-system
state changes while preserving incremental migration. The dormant
`IEventBus`/`SimpleEventBus` and the many direct C# events must either converge
behind one lifecycle-aware facade or have a documented boundary. Subscriptions
must be disposable, scoped to a campaign/panel/session, safe across reset and
rebind, deterministic in ordering, and observable without retaining destroyed
Godot nodes. Events must notify consumers of committed state changes; they must
not become a second gameplay authority.

**First safe step:**

generate the producer/consumer/lifecycle matrix and select one bridge for parity testing before changing `SimpleEventBus` semantics.

**Completion evidence:**

- One documented delivery contract covers direct-event compatibility, bus users,
  UI, audio, journal, and campaign lifecycle.
- Campaign reset and panel rebind leave zero live subscriptions in closed scopes.
- Critical bridge chains deliver once, in deterministic order, with revision
  metadata and actionable diagnostics on handler failure.
- Existing domain behavior is unchanged except for explicitly tested ordering or
  duplicate-refresh fixes.
- Event lifecycle selftest and canonical verification pass in a fresh process.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #23 — Close the shelter operating system: power, air, thermal, water, food, fire, and staffing

**Program placement:** Wave 3 · **Priority:** P0 · **Primary lane:** W4 Shelter OS

**Prerequisites:** #3, #6, #11, #13, #22

**Master interpretation:** Integrated shelter dependency graph under the one campaign-day owner.

**Outcome from the source plan:**

Make the shelter an integrated daily operating system with one authoritative
dependency graph and one campaign-day execution path. Power generation and
load-shedding must affect air filtration, thermal systems, greenhouse, foundry,
medical cold storage, radio, and water operations. Water treatment, sump
contamination, airlock/security, fire/smoke, thermal conditions, kitchen
nutrition, crafting, assignments, and overnight survivor outcomes must consume
and produce explicit resources. Failures must propagate in a deterministic,
player-observable order and persist in the campaign envelope.

**First safe step:**

produce the complete shelter tick-owner/dependency matrix and a read-only duplicate-tick report before adding the coordinator.

**Completion evidence:**

- One campaign-day owner advances all integrated shelter systems exactly once.
- A generated day ledger explains resources, shed loads, unmet demand, incidents,
  staffing, and survivor-impacting outcomes.
- Power/air/thermal/water/fire/food/crafting/assignment dependencies are live,
  deterministic, observable, and persisted.
- Required shelter configuration is loaded from authoritative JSON with no
  hidden production defaults.
- Brownout, contamination, fire, cold, staffing shortage, save/load, and
  malformed-catalog scenarios have passing tests and headless proof.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #24 — Replace fallback-accepted presentation with production asset coverage and a deterministic asset manifest

**Program placement:** Wave 2→6 · **Priority:** P1 · **Primary lane:** W2 Assets/Data

**Prerequisites:** #3, #25

**Master interpretation:** Inventory/manifest can run early; release-critical asset remediation can proceed in parallel.

**Outcome from the source plan:**

Move ASHFALL from “the runtime can display a canonical fallback” to “every
player-facing referenced asset has a deliberate production status.” Create one
deterministic asset manifest linking canonical IDs to imported Godot resources,
aliases, source provenance, import settings, visual role, and fallback policy.
Close the highest-impact visual gaps in batches, especially locations and
portraits, while preserving safe fallback behavior for explicitly optional or
procedurally generated content. Promote coverage from report-only diagnostics to
measured quality gates without blocking legitimate development fixtures.

**First safe step:**

regenerate the current data-to-asset baseline and manifest without moving or deleting files.

**Completion evidence:**

- A deterministic manifest explains every referenced visual asset and its status.
- All release-critical IDs resolve to authored/imported resources with no silent
  fallback; optional/generated categories are explicit and bounded.
- Location and portrait coverage improves materially from the recorded 18.45%
  and 52.68% baselines, with exact before/after numbers in the report.
- Clean import, case-sensitive lookup, asset selftest, and isolated exported
  artifact checks pass.
- The replacement queue, docs, registry, and CI gate are generated from one
  authority and show no drift.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #25 — Make the verification ecosystem authoritative, current-source-safe, and fail-closed

**Program placement:** Continuous · **Priority:** P0 · **Primary lane:** W1 Verification

**Prerequisites:** G0 baseline only

**Master interpretation:** Governing verification authority. Starts at G0 and remains mandatory through release.

**Outcome from the source plan:**

Make every green verification result trustworthy: the manifest, runner, workflow,
CLI catalog, documentation, source gates, build outputs, and domain selftests
must describe the same system and run against current source. A single-gate run
must include its prerequisites; stale Godot assemblies, skipped performance
tests, contradictory warning policies, and undocumented domain selftests must be
visible or fail closed. Reports may remain diagnostic only when their policy is
explicit and their output cannot be mistaken for release proof.

**First safe step:**

generate a read-only crosswalk of manifest, runner, workflow, CLI catalog, and docs; preserve the existing dependency-resolution fix before changing runner behavior.

**Completion evidence:**

- Manifest, runner, workflow, CLI catalog, and docs agree on gate IDs, counts,
  tiers, dependencies, commands, and expected results.
- Any host selftest is blocked unless a current-source host build/import
  fingerprint is present in the same verification flow.
- PASS/BLOCKED/FAIL/DIAGNOSTIC/SKIPPED are distinct and machine-readable.
- Domain selftests that exist and matter are registered, fresh-process tested,
  and executed in the intended tier.
- Warning, performance exclusion, forbidden API, data, save, determinism, and
  asset policies have explicit evidence and no unexplained drift.
- Canonical `.NET + godot --headless` verification passes, or the report clearly
  identifies the first real blocker without stale-green claims.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #26 — Make the trade screen a real, data-driven economy transaction

**Program placement:** Wave 4→5 · **Priority:** P1 · **Primary lane:** W5 Economy

**Prerequisites:** #2, #10, #18, #22

**Master interpretation:** Atomic trade transaction. #2 supplies live providers; #26 owns quote/validate/commit.

**Outcome from the source plan:**

Make the visible trade surface one authoritative transaction loop. Quotes must
use the live market, faction stance, scarcity, radio shocks, inventory, and
current day. Confirmation must atomically mutate market, inventory, and ledger
state. Invalid IDs, stale quotes, insufficient resources, refusal, cancellation,
and duplicate input must have no partial effects. The UI must not report
success when it merely clears offers.

**First safe step:**

add the failing regression that proves a panel confirmation cannot claim success without a state revision change, then publish the trace.

**Completion evidence:**

Likely files: src/Main.Economy.cs, src/Economy/TradeScreenGodotPanel.cs,
src/Foundry/SilentFoundryHostSession.cs, Core trade/presenter/seam/market
classes, tuning loaders, save tests, and transaction tests. Completion is
quote → validate → commit → revision/event → UI observation → save → restore,
with zero false-success confirmations and fresh-process proof.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #27 — Productionize maritime and Deep Coast operations

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 Maritime

**Prerequisites:** #9, #10, #13, #18, #22

**Master interpretation:** Deterministic maritime/Deep Coast operation loop; tactical combat dependency only where route enters combat.

**Outcome from the source plan:**

Make maritime scavenging, safe cracking, diving, salvage, fleet preparation,
and Deep Coast routing deterministic campaign operations. Inputs must come from
live day, radiation, weather, hazard, survivor, vehicle, inventory, and catalog
state. Replace demo loot and fixed context values in production. Equal seeds
must produce equal results in fresh processes.

**First safe step:**

create the operation matrix and failing fresh-process GetHashCode determinism test before changing loot or balance.

**Completion evidence:**

Likely files: MaritimeHostSession, DeepCoastHostSession, Main.Maritime.cs,
Core maritime/safe/expedition/vehicle/equipment/catalog/save classes, maritime
JSON, and tests. Completion requires no fixed production context, no unstable
seed, catalog-backed loot, atomic mutation, save continuity, and two-process
proof.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #28 — Make generational succession and the endgame chronicle authoritative

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 Endgame

**Prerequisites:** #6, #11, #12, #14, #18, #29

**Master interpretation:** Authoritative outcome/succession/chronicle pipeline.

**Outcome from the source plan:**

Replace hardcoded epilogue inputs and UI-created population state with one
authoritative outcome snapshot. Demographics, regional fate, moral outcome,
succession, ending key, chronicle, epilogue, century view, journal, and saves
must all agree.

**First safe step:**

add the incomplete-campaign epilogue test and panel-open population-mutation test, then publish the matrix source inventory.

**Completion evidence:**

Likely files: Main.PlayerSurfaces.cs, Main.GameFlow.cs, EpiloguePanel.cs,
CenturySeedPanel.cs, Core endgame/chronicle/generational classes,
ExpansionHostSession, muster, save codecs, and tests. Completion requires no
hardcoded final inputs, no UI-created authority, one ending resolver, one
succession owner, deterministic chronicle output, and save proof.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #29 — Unify skill progression and competency effects across the campaign

**Program placement:** Wave 3→4 · **Priority:** P1 · **Primary lane:** W4 Skills

**Prerequisites:** #10, #11, #18, #22

**Master interpretation:** Shared skill identity, XP and consumer effects; feed Utility AI rather than host heuristics.

**Outcome from the source plan:**

Create one authoritative skill service from action attribution through XP,
levels, bonuses, atrophy, dormant state, epiphany, consumer queries, UI, and
save/load. Workshop, Pharma Lab, apprenticeship, library study, social work,
Utility AI, combat, medical, and expedition checks must share identity.

**First safe step:**

publish the producer/consumer/identity matrix and add the failing apprenticeship-to-Workshop shared-state test.

**Completion evidence:**

Likely files: Core SkillProgressionSystem, Main.ShelterSocial.cs,
Main.ShelterBatch3.cs, Main.World.cs, SurvivorSocialCoordinator,
Phase0HostSession, Utility AI, skill UI, save DTOs, and integration tests.
Completion requires one skill authority, real consumer effects, no-op removal,
lifecycle-safe identity, persistence, and replay proof.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

### Plan #30 — Complete the PRPF hidden third-power faction and its live branch loop

**Program placement:** Wave 5 · **Priority:** P1 · **Primary lane:** W5 PRPF/Factions

**Prerequisites:** #2, #3, #6, #10, #12, #18, #22

**Master interpretation:** Authored deterministic faction branch with reachability, reload-farming resistance and save continuity.

**Outcome from the source plan:**

Turn PRPF from a standing-only stub into a deterministic, authored,
discoverable faction path with recruitment pressure, hidden-HQ progression,
alignment, moral gates, branch consequences, radio/journal presentation, and
save/load continuity. It must be reachable through normal play, resist reload
farming, and remain balanced against other faction outcomes.

**First safe step:**

publish the PRPF state/data/producer matrix and add the failing normal-play reachability test before adding hidden random rolls.

**Completion evidence:**

Likely files: PrpfStandingSystem.cs, FactionBranchCoordinator,
WeightOfChoicesSaveCodec, Main.FactionBranch.cs, FactionBranchHostSession,
faction/encounter/journal/radio systems, PRPF data,
independent_faction_branch.json, UI, and integration tests. Completion
requires an authored reachable path, deterministic opportunities, truthful
hidden/known state, live branch consequences, checksummed persistence, seeded
simulation evidence, and a gate against the stub returning.

**W6 rejection triggers:** duplicate authority; unproven/stale runtime path; unrelated WIP modification; false-success behavior; missing save/determinism proof where applicable; undocumented compatibility/data change; leaked lifecycle subscription; hidden test/gate exclusion.

---

## 15. Wave exit criteria

### G0 — Truth freeze accepted

- plan status table is current;
- protected/quarantined paths are explicit;
- current verification provenance is known;
- no stale source-era count is used as a target;
- file leases are non-overlapping.

### G1 — Authority backbone accepted

- composition/lifecycle is single-owner;
- typed result/failure contracts exist on governed paths;
- survivor identity/component lifecycle is coherent;
- event subscriptions are scoped/rebind-safe;
- current-source verification prerequisites are enforced.

### G2 — Persistence/data trust accepted

- save-slot inspection/recovery is atomic and truthful;
- required catalogs have owners/classification and schema/reference proof;
- verification manifests/runners do not produce stale green;
- asset/content inventories are current and deterministic.

### G3 — Simulation backbone accepted

- day advances once transactionally;
- world and shelter queries share authoritative state;
- patient/needs/radiation state uses canonical survivor identity;
- skill state is shared by producers/consumers;
- save/reload and replay trajectories remain valid.

### G4 — Cross-system contracts accepted

- live providers are fresh and lifecycle-safe;
- trade mutates atomically;
- narrative consequences apply exactly once;
- expansions compose/tick/save once;
- Utility AI commits real actions through typed commands.

### G5 — Gameplay loops accepted

- expedition/combat/maritime transitions are legal and deterministic;
- Holdfast and PRPF are reachable through normal play;
- feature outcomes mutate authoritative state once and survive restore;
- endgame/succession reads a real campaign outcome snapshot.

### G6 — Player integration accepted

- controls/settings are recoverable;
- accessibility/focus paths are complete;
- audio reacts to committed state and detaches safely;
- release-critical assets resolve intentionally;
- the selected end-to-end player journey works without hidden re-composition or false success.

### G7 — Release accepted

- measured performance budgets are met or consciously approved with evidence;
- current-source verification is green with no hidden skips;
- deterministic replay/save compatibility gates pass;
- clean clone can build/import/export;
- exported artifact runs independently with authoritative data/assets;
- release manifest/hashes/build identity are internally consistent.

---

## 16. Final release definition

ASHFALL is ready for a production release candidate only when all of the following are true:

1. every still-relevant Plan #1–#30 is `RETIRED`, `SUPERSEDED_WITH_PROOF`, or `COMPLETE`;
2. no `BLOCKED` P0/P1 plan remains on the release-critical path;
3. one authority exists for each simulation/persistence/data domain;
4. the campaign-day pipeline advances every registered owner exactly once;
5. representative same-seed replay matches across fresh runs and save/reload;
6. save-slot corruption/recovery tests preserve known-good state;
7. no release-critical player action can report success without authoritative mutation;
8. no closed panel/session/campaign scope retains live subscriptions;
9. required JSON/content/assets are reachable and validated;
10. controls/accessibility/audio/player feedback work in the real Godot surface;
11. performance tests are visible and their policy is explicit;
12. the verification runner proves current source/build identity;
13. the release artifact is self-contained and passes isolated smoke/save/data checks;
14. W6 reports zero unresolved blocking semantic findings.

---

## 17. Coordinator reporting template

After each accepted slice, update one compact program ledger:

```text
Plan:
Slice:
Status: ACTIVE | REVIEW | COMPLETE | BLOCKED | RETIRED
HEAD/source fingerprint:
Pre-existing WIP preserved:
Files changed:
Authority changed:
Schema/save impact:
Migration impact:
Determinism impact:
Targeted tests:
Fresh-process proof:
Canonical gates:
W6 findings:
Remaining blockers:
Next dependency-unlocked slice:
```

After each wave, publish:

- completed/retired plans;
- still-active gaps;
- dependency changes;
- new risks;
- current verification health;
- exact unresolved protected paths;
- next wave file leases.

---

## 18. Recommended immediate action

Do **not** immediately start Plan #1 just because it is numbered first.

Run **G0 Repository Truth Freeze** against the current checkout and reconcile the 2026-08-29 source-plan snapshot with current reality. In particular, re-evaluate composition/Task #131, survivor migration, save registry/counts, Utility AI characterization, performance test inclusion, current host build health, and verification manifest truth.

Then execute only the unresolved slices in the dependency order defined above.

This turns the 30-plan archive into a living program rather than a queue that blindly repeats completed work.
