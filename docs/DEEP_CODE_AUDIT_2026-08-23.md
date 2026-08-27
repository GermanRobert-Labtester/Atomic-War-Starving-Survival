# ASHFALL Deep Code Audit and Remediation Plan

**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Audit baseline:** `main` at `b4763049037495649ac854358e0f333988827b99`
**Audit date:** 2026-08-23
**Focus:** defects, correctness risks, architecture pressure points, persistence/determinism, CI/test integrity, performance, data/tooling, dependency surface, and maintainability.
**Related work:** `sources.md` is the broader architecture/codebase exploration report. This document is intentionally more defect- and remediation-oriented.

## Verification model

This audit is based on static inspection of the repository at the baseline commit plus GitHub Actions evidence from the active remediation work. It does **not** claim that this audit independently executed the full build locally.

The baseline and in-flight remediation must be kept distinct:

- `main` at the audit snapshot is the state being audited.
- PR #24 (`maintenance: restore Core build authority and align engine policy`) is open remediation work and is **not** treated as already merged.
- Issues #25 and #26 already track two major architectural remediations identified below.
- Where PR #24 already addresses a finding, this report marks it as **in-flight** instead of recommending duplicate work.

## Executive assessment

ASHFALL has several unusually strong engineering foundations for a migration-heavy game codebase: one physical engine-agnostic Core source tree, explicit simulation ports, seeded RNG and simulation-clock abstractions, state-based save checksums, fail-loud compatibility gaps, JSON data authority, extensive Core tests, and a large headless Godot self-test surface.

The largest risk is not lack of testing. It is **authority drift between layers**. The repository currently has multiple partially overlapping sources of truth: Unity legacy code, Godot host code, engine-agnostic Core, generated visual manifests, historical documentation, and two CI workflows that communicate different engine policies. This creates failure modes where each subsystem looks reasonable in isolation but the aggregate repository is not buildable or a derived artifact/test contract has become stale.

The immediate objective should be: **restore a genuinely green canonical branch, make that gate mandatory, then reduce the number of places that must be changed together.**

## Severity model

- **P0 — release/blocking correctness:** repository cannot be reliably built, loaded, saved, or verified.
- **P1 — high correctness/architecture risk:** likely to produce regressions, partial state, false-green CI, or high change amplification.
- **P2 — material maintainability/reliability risk:** should be scheduled, but can follow restoration of the canonical gate.
- **P3 — optimization/hygiene:** worthwhile after correctness and ownership are stable.

Confidence is reported as **High**, **Medium**, or **Conditional**. Conditional findings describe a real hazardous code path whose impact depends on how it is used.

---

# Priority findings

## AUD-001 — `main` contains an incomplete Core integration and does not represent a trustworthy build baseline

**Severity:** P0
**Confidence:** High
**Status:** In-flight remediation in PR #24

### Evidence

Multiple newly merged Core systems reference shared types that are absent from the baseline Core tree, including the shared `ActionResult` contract and several collaborator authorities. The baseline also contains `src/Main.ExpandedShelterSystems.cs`, an expanded shelter wiring partial that references additional authority/host types that are not present. The remediation branch established that this partial's setup/save/tick/open surface has no active callers.

The Godot aggregate project compiles `Assets/Ashfall.Core/**/*.cs` and `Assets/_Game/**/*.cs`, so a missing shared contract is not isolated to one optional feature: it breaks the aggregate compile surface.

### Impact

- The default branch can be non-buildable even though prior historical test evidence was green.
- Downstream Godot self-tests cannot provide meaningful assurance until compilation is restored.
- New contributors cannot distinguish an intentional migration gap from an accidentally incomplete merge.

### Recommended fix

Finish the focused repair already underway in PR #24 rather than adding more parallel repair branches:

1. restore the missing shared Core contracts required by already-merged systems;
2. remove or quarantine unreachable WIP host wiring that references authorities that never landed;
3. require Core build + tests + Godot aggregate build before merge;
4. add a merge checklist rule: a feature PR that introduces a type reference must include the authority/contract or explicitly depend on another already-merged PR.

### Acceptance criteria

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --configuration Release` succeeds;
- all Core tests pass;
- `dotnet build Ashfall.csproj` succeeds;
- `./scripts/ci/godot-asset-gate.sh` succeeds from a fresh checkout;
- no unreachable host partial references missing Core authorities.

---

## AUD-002 — `main` is nominally protected, but required CI checks are not enforced

**Severity:** P0/P1
**Confidence:** High

### Evidence

GitHub reports `main` as protected, but its protection metadata has required-status-check enforcement disabled and an empty required-check/context list.

### Impact

This converts CI from a merge gate into advisory telemetry. A repository can therefore merge a change that breaks Core compilation or generated-art invariants even though a strong workflow exists.

### Recommended fix

Configure the repository ruleset/branch rule so the canonical `ASHFALL CI` jobs are required before merge. At minimum require:

- Repository / Data Validation;
- Ashfall.Core Tests;
- Godot 4.7.1 .NET Build / Asset / Self-Test gate (or the stable aggregate job name if the workflow uses a single required job).

Also prevent force-push/bypass for routine feature work. If administrator bypass remains enabled for emergencies, document that bypassed merges require a follow-up incident issue.

### Acceptance criteria

- GitHub branch/ruleset metadata lists the canonical check(s) as required;
- a deliberately failing test PR cannot be merged normally;
- merge queue/auto-merge, if used, waits for the same gate.

---

## AUD-003 — production-art tests contain stale phase assumptions and currently reject legitimate regenerated state

**Severity:** P1
**Confidence:** High
**Status:** Current blocker exposed after PR #24 restored compilation

### Evidence

`Ashfall.Core.Tests/ProductionArtManifestTests.cs` still documents the historical manifest law `478 actionable + 136 skipped = 614` and requires `skipped > 0` even though the test otherwise computes counts dynamically.

`tools/production_manifest.py` builds the manifest from the **current** `WIRING_MATRIX.json` entries whose `resolved_path == "MISSING"`. Reference-only rows are emitted only when a currently missing entry belongs to a `Reference-Skip` catalog. It is therefore valid for the current missing set to contain zero reference-only rows.

The companion `docs/visual/runtime_context_top_ids.json` is also stale: it claims `manifest_actionable: 478` and `surfaced_count: 39`. After canonical manifest regeneration the active manifest is smaller, so the test recomputation no longer equals the historical claimed count.

### Impact

- Correctly regenerated artifacts can make CI red.
- Developers are incentivized to edit generated JSON or weaken assertions rather than repair the generator dependency graph.
- Derived files can disagree while each file remains syntactically valid.

### Recommended fix

Treat generated visual artifacts as an atomic dependency graph.

1. Replace historical absolute/positivity assumptions with relational invariants:
   - every row has a recognized generation status;
   - `PENDING + SKIP_REFERENCE_ONLY == total`;
   - zero rows in either category are allowed unless the schema explicitly requires otherwise;
   - every manifest row corresponds to the current wire-matrix missing set.
2. Regenerate `runtime_context_top_ids.json` whenever the production manifest changes, or generate both in one command.
3. Add a CI `--check` mode that runs all visual generators into a temporary directory and diffs the committed derived outputs.
4. Never fix this class of failure by manually editing generated counts.

### Acceptance criteria

- canonical regeneration followed by tests is idempotent and green;
- running the generator twice produces no diff;
- zero `SKIP_REFERENCE_ONLY` rows is accepted when the source data produces zero;
- `runtime_context_top_ids.json` derives its counts from the same current manifest in the same generation pipeline.

---

## AUD-004 — `TimeSystem` can violate its own “one event per crossed hour” contract

**Severity:** P1
**Confidence:** High

### Evidence

`Assets/_Game/Core/TimeSystem.cs` states that `OnHourTick` fires once per integer hour crossed and that large time spans do not skip hourly consumers. `TickHours()` splits work using public mutable `MaxGameHoursPerStep`. `Advance()` then captures only `prevHour`, adds the entire step, and emits at most one `OnHourTick` if the final integer hour differs.

With the default `MaxGameHoursPerStep = 1f`, the normal path is safe. But the public tuning value can be set above one, at which point a 2+ hour substep can cross multiple integer hours and emit only the final one.

There is a second edge inconsistency: `RestoreState()` clamps `hourAccumulator` to `0..24` inclusive, while `CurrentHour` is documented as `0..23`. A save containing exactly `24f` can temporarily restore `CurrentHour == 24` without incrementing the day until a later advance normalizes it.

### Impact

Hourly consumers may silently under-run after tuning changes, fast-forward changes, or malformed/legacy save state. Time-driven needs, hazards, production, or scheduled systems can diverge from expected deterministic behavior.

### Recommended fix

Make boundary iteration independent of the tuning step size.

Preferred approach:

- advance to each next integer-hour/day boundary in a loop;
- emit one hourly callback for every boundary crossed;
- normalize restored `(day, hourAccumulator)` into canonical day + `0 <= hour < 24` before exposing state.

A simpler short-term guard is to clamp `MaxGameHoursPerStep <= 1`, but boundary-correct code is more robust and makes the public tuning less dangerous.

### Acceptance criteria

Add regression tests for:

- `TickHours(5.5f)` with `MaxGameHoursPerStep = 8f` emits all crossed hour ticks in order;
- a multi-day jump emits every day boundary and appropriate hourly boundaries;
- restoring `hourAccumulator == 24f` normalizes to the next day at hour 0;
- restoring NaN/negative/out-of-range values yields canonical deterministic state.

---

## AUD-005 — `src/Main.cs` is an orchestration god object

**Severity:** P1
**Confidence:** High
**Status:** Tracked by issue #25

### Evidence

`src/Main.cs` owns a very large set of host sessions, gameplay coordinators, UI panels/detail panels, dirty/save-coalescing flags, diagnostics state, navigation, lifecycle state, and a large CLI/self-test dispatch path.

This is composition code, not simulation logic, but its responsibility count is still excessive.

### Impact

- initialization order becomes implicit;
- forgotten dirty/flush wiring can lose state;
- teardown/subscription ownership is hard to prove;
- unrelated features collide in the same file;
- migration work increases merge-conflict rate;
- CLI verification and interactive runtime wiring become coupled.

### Recommended fix

Follow issue #25's staged extraction rather than a one-shot rewrite:

1. `HostCliRunner` / self-test lifecycle;
2. save dirty/flush coordinator;
3. diagnostics coordinator;
4. UI composition/navigation;
5. game-session lifecycle coordinator.

Keep `Main` as the Godot composition root, but make it compose explicit coordinators rather than contain their policies.

### Acceptance criteria

- no simulation authority is moved into Godot nodes;
- every stateful host session has one obvious owner;
- quit/close still flushes all dirty state;
- every existing CLI flag preserves behavior and exit semantics;
- extracted coordinators receive focused tests/self-tests.

---

## AUD-006 — persistence coordination has extreme concrete dependency fan-out and insufficiently explicit transactional semantics

**Severity:** P1
**Confidence:** High
**Status:** Tracked by issue #26

### Evidence

`Assets/_Game/Core/SaveSystem.cs` contains a very large concrete field set spanning environment, shelter, survivors, radiation, inventory, medicine, economy, events, combat, quests, expansions, hazards, items, locations, endgame systems, and many other feature families.

The file already points toward an `ISaveable` registry model, but the central coordinator still knows a large fraction of concrete gameplay types.

### Impact

- every new persistent feature amplifies changes in the central persistence surface;
- restore ordering becomes implicit and fragile;
- one invalid participant can risk leaving a partially restored live session;
- migration/version compatibility is difficult to reason about globally;
- omissions are easy to introduce when a subsystem lands without persistence wiring.

### Recommended fix

Continue toward a versioned participant registry with a two-phase restore:

**Phase A — detached:** decode, checksum, migrate, validate all participants into detached snapshots.
**Phase B — apply:** mutate live systems only after every required participant has passed Phase A.

Each participant should expose a stable ID, participant schema version, capture, decode/validate, migration chain where needed, and apply function.

### Acceptance criteria

- duplicate participant IDs fail at startup/test time;
- unsupported required versions fail before live mutation;
- one corrupt participant cannot leave earlier systems restored and later systems stale;
- supported old saves continue to load through explicit migrations;
- adding a migrated participant does not require another concrete field on the central coordinator.

---

## AUD-007 — the active Godot aggregate project still compiles the entire Unity-coupled legacy `_Game` tree

**Severity:** P1
**Confidence:** High

### Evidence

`Ashfall.csproj` compiles `src/**/*.cs`, `scripts/**/*.cs`, `Assets/Ashfall.Core/**/*.cs`, **and** `Assets/_Game/**/*.cs` into the Godot aggregate.

That approach accelerates strangler migration because old gameplay can be bridged quickly, but it also means legacy compile failures remain active-host failures.

### Impact

- migrated and non-migrated ownership is ambiguous;
- dead Unity-era feature code can break Godot builds;
- compatibility shims have to grow to satisfy code that may not be reachable;
- migration completion does not automatically reduce dependency surface.

### Recommended fix

Introduce explicit migration ownership rather than compiling `_Game` wholesale forever.

For each domain classify files as:

- **Core authority** — `Assets/Ashfall.Core`;
- **Godot host/presentation** — `src`;
- **legacy compatibility required** — `_Game` included deliberately;
- **retired/quarantined** — excluded from the Godot aggregate.

Move from wildcard `_Game/**/*.cs` inclusion toward explicit compatibility include groups or explicit exclusions. Add a machine-readable ownership manifest if practical.

### Acceptance criteria

- a migrated domain is compiled from one gameplay authority;
- retired Unity-only files cannot break the Godot aggregate;
- bridge coverage shrinks as domains migrate;
- CI detects duplicate/forked authority types.

---

## AUD-008 — engine/source authority is contradictory across README and workflows

**Severity:** P1
**Confidence:** High
**Status:** In-flight remediation in PR #24

### Evidence

The baseline `README.md` describes Unity 6 LTS as the project runtime, documents Unity scenes as the current simulation boot path, and gives Unity batch-mode verification commands.

At the same time `.github/workflows/ci.yml` states that Godot is the only active engine and must not invoke Unity tooling. `.github/workflows/build.yml` still contains a Unity build pipeline with stale comments about CI behavior.

### Impact

Developers can follow official-looking instructions and verify the wrong host. More importantly, the project cannot answer a basic engineering question consistently: **which engine is authoritative for merge correctness?**

### Recommended fix

Complete PR #24's policy cleanup and retain one canonical policy document describing:

- Core gameplay authority;
- active Godot host;
- authored JSON data authority;
- Unity legacy/compatibility status;
- which checks are canonical merge gates versus optional compatibility artifacts.

### Acceptance criteria

README, engine-support policy, workflow comments, and release process all state the same ownership model.

---

## AUD-009 — compiler-warning policy hides too much signal

**Severity:** P1/P2
**Confidence:** High

### Evidence

The Godot aggregate enables nullable analysis but suppresses a broad group of warnings including `CS8600`, `CS8601`, `CS8602`, `CS8603`, `CS8604`, `CS8618`, `CS8625`, and others. The standalone Core project disables nullable analysis globally even though some files already contain nullable annotations.

Recent repaired Core CI built successfully but emitted a large warning volume (189 warnings in the observed run), including nullability-context warnings and serializer DTO field warnings.

### Impact

A warning flood makes genuinely new defects indistinguishable from accepted legacy noise. Broad project-level nullable suppression is particularly risky in persistence, catalog loading, and host wiring, where null handling is correctness-critical.

### Recommended fix

Use a ratchet rather than a flag-day cleanup:

1. establish a warning baseline;
2. make **new/changed production code** warning-clean;
3. enable nullable by directory/file for Core domains as they are touched;
4. isolate serializer DTO warnings with narrow pragmas or DTO-specific project rules;
5. eventually enable warnings-as-errors for Core/new Godot host code while keeping documented legacy exceptions.

### Acceptance criteria

- warning count cannot increase on a PR without explicit justification;
- new Core files compile with nullable enabled;
- serializer DTO warnings are localized rather than suppressing nullability across the whole project.

---

## AUD-010 — the headless CLI verification surface is much larger than the canonical gate, and command registration is manually duplicated

**Severity:** P2
**Confidence:** High

### Evidence

`src/Host/HostCli.cs` contains a very large `HostCliAction` enum and a long ordered parser mapping many aliases to dozens of self-tests and UI tests. The canonical `godot-asset-gate.sh` runs only seven host gates:

- asset registry;
- data integrity;
- bridge;
- disease;
- expansions;
- black flotilla;
- radio.

The CLI exposes many additional checks for combat, medical, narrative, world, economy, RNG wiring, day-1/day-2 milestones, playable shell, shelter operations/hazards, deep coast, warlords, UI layouts, settings, audio, expedition UI, snapshots, and others.

### Impact

- substantial regression coverage exists but is not exercised on every merge;
- parser/help/dispatch can drift independently because registration is manual;
- aliases can shadow or silently fall through to interactive mode.

### Recommended fix

1. replace enum + long `if (Has(...))` chains + separate help text with a declarative command registry containing action, aliases, description, and handler metadata;
2. add a parser test asserting all aliases are unique and every action is reachable;
3. keep a fast merge gate, but add a scheduled/nightly “full headless matrix” that executes all non-interactive self-tests;
4. promote critical gameplay milestone tests (day1→day2, save/load, RNG wiring) into the merge gate if runtime remains reasonable.

### Acceptance criteria

- one registration record drives parsing and help;
- duplicate aliases fail tests;
- every non-interactive action is exercised by either merge CI or scheduled full-matrix CI;
- unknown flags fail loudly instead of silently becoming interactive where appropriate.

---

## AUD-011 — canonical Godot asset import failure is currently tolerated

**Severity:** P2
**Confidence:** High

### Evidence

`scripts/ci/godot-asset-gate.sh` runs `godot --headless --path . --import`, but if import exits non-zero it only prints a warning and continues to later gates.

### Impact

The script's stated purpose is to verify that a fresh checkout can build and load catalog-referenced assets. Ignoring import failure weakens that guarantee. Later asset tests may catch most failures, but the import command itself can reveal editor/importer failures not represented by current catalog coverage.

### Recommended fix

Make import failure fail the canonical gate by default. If there are known benign Godot import exit cases, classify and filter those narrowly rather than accepting any non-zero import result.

### Acceptance criteria

- deliberately breaking an importer/resource causes CI failure;
- a successful fresh-checkout gate proves both import completion and runtime asset resolution.

---

## AUD-012 — `SaveChecksum` is a strong cross-host design, but its reflection contract needs explicit schema guards

**Severity:** P2
**Confidence:** High for the contract risk; Conditional for performance impact

### Evidence

`Assets/Ashfall.Core/SaveChecksum.cs` deliberately hashes canonical object state rather than serializer output, sorts public fields ordinally, uses invariant formatting, normalizes known cross-serializer null differences, and uses SHA-256. This is a strong solution to Unity-vs-System.Text.Json formatting incompatibility.

However, the canonicalization contract is reflection-based and explicitly covers **public instance fields only**. A future serialized property/private serializer field can therefore fall outside integrity coverage. Generic `IEnumerable` sequences are hashed in enumeration order; unordered collection types would need special handling to remain semantically canonical across construction/runtime differences.

### Impact

The risk is future schema evolution silently escaping checksum coverage or introducing ordering-dependent hashes.

### Recommended fix

Preserve the current design but harden its contract:

- add a test that reflects all save DTO serialized members and asserts they are checksum-covered;
- prohibit unordered collections in save DTOs unless the checksum canonicalizer sorts them explicitly;
- add golden cross-host checksum vectors for representative saves;
- profile checksum time and allocation on large campaign saves before considering an explicit generated canonical writer.

Do **not** replace SHA-256 or return to hashing serializer text.

### Acceptance criteria

- adding a serialized member that is not checksum-covered fails a test;
- dictionary/set-like save members are either prohibited or canonically ordered;
- representative save checksum vectors remain stable across supported hosts/runtimes.

---

## AUD-013 — generated-data tooling lacks a single freshness boundary

**Severity:** P2
**Confidence:** High

### Evidence

The production-art manifest, wiring matrix, runtime-context summaries, generated prompts, ledgers, and related visual documents are produced by multiple tooling phases. Current failures demonstrate that one derived file can be regenerated while another still carries counts from an older phase snapshot.

### Impact

- stale generated documents look authoritative;
- tests end up embedding old generation counts;
- large generated diffs obscure the small source-data change that caused them.

### Recommended fix

Create one top-level command such as:

```text
python3 tools/regenerate_visual_state.py
```

that runs generators in dependency order and supports:

```text
--check   # regenerate to temp and fail on diff
--write   # update committed outputs
```

Each generated file should contain generator name/version or source digest where practical.

### Acceptance criteria

- one command reproduces every committed derived visual state artifact;
- `--check` is in CI;
- generated outputs are deterministic and idempotent.

---

## AUD-014 — legacy dependency/service surface is much broader than the active-host story suggests

**Severity:** P2
**Confidence:** High

### Evidence

`Packages/manifest.json` retains a broad Unity package set including Remote Config, Analytics, Cloud Code, Cloud Save, Economy, Friends, Leaderboards, LevelPlay, Moderation, Push Notifications, Cloud Build/CCD management, ECS/storytelling feature packs, Visual Scripting, and other modules. `Ashfall.csproj` also references Sentry for the Godot/.NET host.

This may be intentional compatibility residue, but the repository currently lacks a single clear active-vs-legacy dependency inventory.

### Impact

- larger supply-chain/update surface;
- unclear privacy/telemetry behavior;
- longer Unity restore/import times;
- difficulty knowing which packages may be safely removed as migration advances.

### Recommended fix

Maintain a dependency ownership table with columns: package, host, purpose, runtime-active?, telemetry/network behavior, removal condition. Remove Unity service packages when no supported compatibility path needs them. Make telemetry opt-in/explicit where applicable and document Sentry event/data policy.

### Acceptance criteria

- every network/telemetry dependency has an owner and documented purpose;
- unused Unity services are removed from the compatibility project;
- release/privacy documentation matches runtime behavior.

---

## AUD-015 — repository hygiene mixes source, audit/process material, generated state, and migration quarantine at the root

**Severity:** P3
**Confidence:** High

### Impact

This is not primarily aesthetic. A crowded root and already-tracked generated/historical files make code search noisier, increase accidental edit surface, and obscure which artifacts are authoritative.

### Recommended fix

Adopt an explicit repository-artifact policy:

- source/config at root only when build tooling requires it;
- long-lived technical docs under `docs/`;
- historical audits under `docs/audits/`;
- generated reports under a documented generated directory;
- test outputs never tracked unless they are deliberate golden fixtures;
- quarantine directories either have a retirement issue/date or live outside active compile globs.

Perform cleanup in a dedicated PR with no gameplay changes.

---

# Cross-cutting design strengths to preserve

The remediation should not erase the parts that are already working well.

## One physical Core source tree

`Ashfall.Core/Ashfall.Core.csproj` compiles `../Assets/Ashfall.Core/**/*.cs` rather than maintaining a copied Core tree. Preserve this. Duplicate engine-specific copies would recreate authority drift at the gameplay layer.

## Explicit deterministic ports

`Assets/Ashfall.Core/Ports.cs` defines `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, and `ISeededRng`, explicitly banning Unity serializer dependence in Core and wall-clock time for simulation. This is the correct migration boundary.

## Fail-loud bridge policy

`src/Bridge/BridgeGap.cs` explicitly distinguishes semantic gaps (throw), cosmetic gaps (warn once), and true no-ops. Preserve this policy. Returning plausible defaults from compatibility shims is one of the easiest ways to convert compile-time migration gaps into silent gameplay corruption.

## State-based save integrity

`SaveChecksum` correctly avoids hashing pretty-printed JSON bytes. Preserve the host-independent state checksum invariant while adding schema-coverage tests.

## Broad regression culture

The repository contains a large Core test suite and many headless host/self-test entry points. The main improvement needed is **gate selection and authority**, not abandoning the current test strategy.

---

# Performance and scalability review

No performance rewrite is justified before measuring real campaign behavior, but several pressure points deserve instrumentation.

## Save path

The combination of a high-fan-out persistence coordinator and reflection-based checksum generation can become expensive as campaign state grows.

Add metrics/self-test diagnostics for:

- capture duration per participant/system;
- checksum duration;
- serialized byte size;
- restore decode/validate/apply duration;
- largest persistent collections;
- save-size growth over 1, 30, 100, and long-campaign day counts.

Set budgets only after observing representative data.

## Persistent collection growth

Any queue/history/log stored in saves should have an explicit retention rule. Existing regression work has already fixed at least one unbounded completed-job style pattern; turn that principle into a reusable invariant test for persistent histories.

## UI/host orchestration

As `Main` is decomposed, prefer version/dirty-driven UI refreshes rather than broad per-frame reconstruction. Do not optimize individual panels without profiling; the architectural objective is to make refresh ownership observable.

---

# CI/test architecture recommendation

Use three verification tiers.

## Tier 1 — required on every PR

1. JSON/data syntax and schema validation;
2. Core restore/build/test;
3. Godot aggregate build;
4. Godot import must succeed;
5. critical headless gates:
   - data integrity;
   - bridge semantic gaps;
   - asset registry;
   - RNG wiring;
   - save/tamper round-trip;
   - day1→day2 or equivalent playable milestone;
   - expansion aggregate smoke.

## Tier 2 — scheduled/nightly

Run the complete non-interactive `HostCli` self-test matrix, all UI headless tests that are stable in CI, long-campaign deterministic scenarios, and save-growth/performance diagnostics.

## Tier 3 — compatibility/release

Run Unity compatibility artifacts only if Unity remains a supported migration/release requirement. These jobs must not be described as substitutes for the canonical Core/Godot gameplay gate.

---

# Recommended remediation sequence

## Phase 0 — restore green authority immediately

1. Finish PR #24's baseline compile repair.
2. Fix the two current production-art test/derived-data mismatches without manually editing generated counts.
3. Run full Core + Godot canonical gate.
4. Merge only when green.
5. Enable required CI checks on `main` immediately after/with the repair.

**Exit condition:** a clean checkout of `main` passes the documented canonical commands.

## Phase 1 — eliminate policy ambiguity

1. Merge the engine-support/source-authority policy.
2. Align README and workflow comments.
3. Define Unity as active, compatibility-only, or retired—never all three depending on the file being read.
4. Add CI validation for critical documentation/build paths if practical.

## Phase 2 — reduce host orchestration concentration

Execute issue #25 in small behavior-preserving PRs: CLI runner, save coordinator, diagnostics, UI router, session lifecycle.

## Phase 3 — make restore transactional and versioned

Execute issue #26 incrementally. Migrate one low-risk participant first, prove all-or-nothing failure semantics, then expand.

## Phase 4 — reduce migration coupling

1. stop compiling migrated/retired `_Game` domains into the Godot aggregate;
2. shrink bridge surface;
3. adopt per-domain authority metadata;
4. ratchet nullable/warning policy on Core and new host code.

## Phase 5 — make generated state reproducible

1. single visual-state generation entry point;
2. CI freshness check;
3. relational tests instead of historical count assertions;
4. generated-file source digest/version metadata.

## Phase 6 — performance/soak hardening

1. deterministic long-campaign scenario digest tests;
2. save-size growth tests;
3. save/restore timing metrics;
4. UI refresh profiling;
5. optimize only measured bottlenecks.

---

# Suggested concrete issues/PRs

The following work units are small enough to review independently:

1. **fix(time): make hourly/day boundary dispatch invariant to step size** — implement AUD-004 tests/fix.
2. **fix(ci): require successful Godot import** — implement AUD-011.
3. **fix(art): make manifest tests relational and regenerate coupled runtime-context state** — implement AUD-003/AUD-013.
4. **ci: require ASHFALL CI on main** — repository ruleset change for AUD-002.
5. **test(cli): declarative HostCli registration and alias uniqueness** — first slice of AUD-010.
6. **build: introduce explicit legacy compatibility compile ownership** — first slice of AUD-007.
7. **quality: nullable/warning ratchet for new Core files** — first slice of AUD-009.
8. **test(save): assert checksum coverage for all serialized DTO members** — AUD-012.
9. **docs/deps: classify runtime vs legacy packages and telemetry** — AUD-014.
10. Continue existing **#25** and **#26** rather than opening duplicates for Main/persistence decomposition.

---

# “Do not fix it this way” guardrails

Several tempting shortcuts would make the repository look greener while reducing confidence:

- Do **not** disable or delete failing generated-state tests just because counts changed; replace stale absolute assumptions with current relational invariants.
- Do **not** add plausible default implementations to semantic Unity bridge gaps; preserve fail-loud behavior.
- Do **not** create a second copy of Core for Godot.
- Do **not** move simulation logic into Godot UI/nodes while decomposing `Main`.
- Do **not** rewrite the entire SaveSystem in one PR; migrate participant-by-participant with compatibility tests.
- Do **not** make all warnings errors until legacy/serializer warnings are isolated; use a warning ratchet.
- Do **not** treat Unity artifact success as proof that the canonical Godot gameplay host is correct, or vice versa.
- Do **not** optimize `SaveChecksum` before measuring it; first protect its schema/canonicalization contract.

---

# Target end state

A healthy post-remediation repository should have the following properties:

- `main` is always buildable and canonical CI is required;
- `Assets/Ashfall.Core` is the single engine-independent gameplay authority for migrated domains;
- `src` composes/presents Core behavior but does not own simulation rules;
- `_Game` is an explicitly shrinking compatibility surface, not an implicit second authority;
- save participants are versioned and restored transactionally;
- deterministic clock/RNG/save invariants have boundary-focused regression tests;
- generated visual/data state is reproducible by one documented command and CI freshness checks;
- README, workflow comments, support policy, and release process agree about engine authority;
- warning count trends downward and new production code is warning-clean;
- the complete headless verification surface runs on an appropriate merge/nightly cadence;
- dependencies and telemetry/network behavior have explicit owners.

## Final assessment

The project does **not** need an architectural reset. Its strongest abstractions—Core ports, deterministic boundaries, state checksum, bridge failure policy, JSON authority, and headless testing—are directionally correct.

The dominant problem is **integration discipline across a very large migration surface**. Incomplete feature batches, manually synchronized generated artifacts, non-required CI, and oversized coordinators allow locally reasonable changes to become globally inconsistent.

The highest-return strategy is therefore:

> **Make authority executable:** one required canonical gate, one owner per gameplay domain, one reproducible generator path per derived artifact family, and transactional/versioned persistence boundaries.

Once those controls are in place, the existing codebase can be improved incrementally without a risky rewrite.
