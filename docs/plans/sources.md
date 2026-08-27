# Atomic War: Starving Survival — Comprehensive Codebase Exploration Report

> [!WARNING]
> **HISTORICAL ARCHITECTURAL REPORT — DO NOT USE FOR CURRENT IMPLEMENTATION**
> This exploration report records the state of the repository on **2026-08-22** (Git SHA `04b1f465`) during the active strangler migration.
> It describes legacy architecture and components that have since been **completely retired and removed** from the codebase, including the legacy Unity gameplay host (`Assets/_Game/`), the Unity compatibility bridge shim (`src/Bridge/`), and legacy Unity build workflows.
> **Current Project Authority:**
> - Master directives: [`AGENTS.md`](../../AGENTS.md)
> - Domain logic (single source of truth): [`Assets/Ashfall.Core/`](../../Assets/Ashfall.Core)
> - Data authority: [`Assets/StreamingAssets/Data/`](../../Assets/StreamingAssets/Data) (129 JSON catalogs)
> - Active runtime host: **Godot 4.7+ (.NET / C#)** (`src/`, `project.godot`)
> - Verification gate: `dotnet test` (3,244 tests) + `godot --headless` only.
> Do not restore or reintroduce Unity dependencies or compatibility shim layers described in this historical document.

**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Branch audited:** `main`
**Snapshot commit:** `04b1f465914b18d3b9c4bb8cd254802e2a3b6f30`
**Audit date:** 2026-08-22
**Audit type:** repository-wide static architecture/code/configuration review using the GitHub repository contents, current source files, build configuration, CI definitions, data layout, tests, and existing audit/design documents.

> **Important verification note:** I did **not** execute `dotnet`, Godot, or Unity in this review. Build/test results quoted from `10LOOP_AUDIT_REPORT.md` and similar files are historical repository evidence, not tests rerun for this report. Current-state findings are explicitly separated from historical/documented claims.

---

## 1. Executive summary

This is a very large survival-RPG codebase in the middle of a deliberate **strangler migration from a Unity-first architecture to a Godot-hosted, engine-agnostic core**. The repository contains three overlapping technical layers:

1. `Assets/_Game/` — the large Unity-coupled legacy/gameplay surface.
2. `Assets/Ashfall.Core/` — the engine-agnostic domain layer intended to become the single logic authority.
3. `src/` + `scenes/` + `project.godot` — the active Godot host, UI/composition layer, headless verification CLI, and Unity compatibility bridge.

The migration strategy itself is sound: the same core source files are compiled by Unity, Godot, and the xUnit test project. `Ashfall.Core/Ashfall.Core.csproj` intentionally globs `../Assets/Ashfall.Core/**/*.cs`, while `Ashfall.csproj` also compiles that same directory. This avoids a particularly dangerous failure mode in engine migrations: copied domain logic silently diverging between hosts.

The codebase also demonstrates unusually strong engineering discipline for a game project in several areas:

- deterministic RNG/clock interfaces;
- serializer-independent state hashing for cross-host save integrity;
- catalog/data authority in `Assets/StreamingAssets/Data/`;
- an explicit compatibility-bridge failure policy (`BridgeGap`) that prefers loud semantic failures over plausible but incorrect defaults;
- a large headless self-test command surface covering gameplay, persistence, bridge behavior, data integrity, UI layout, and expansion systems;
- CI that validates JSON catalogs, builds/runs the engine-agnostic core tests, builds the Godot aggregate assembly, imports Godot assets, and runs canonical headless gates;
- a large historical regression suite and audit trail documenting real bugs and their fixes.

The largest risks are not a lack of systems or tests. They are **scale, authority ambiguity, migration complexity, and concentration of orchestration responsibilities**. In particular:

- `src/Main.cs` is an extremely broad Godot composition root containing a very large number of host sessions, panels, dirty flags, CLI flows, and lifecycle responsibilities.
- `Assets/_Game/Core/SaveSystem.cs` is a high-coupling persistence hub whose partial type references a very large fraction of the gameplay model.
- the Godot aggregate project still compiles all of `Assets/_Game/**/*.cs` through the compatibility bridge, preserving a huge legacy semantic surface even as the engine-agnostic core grows.
- `Ashfall.csproj` suppresses a substantial nullable/compiler-warning set, including `CS8602`, `CS8603`, `CS8604`, and `CS8618`; historical audits indicate these classes were actively reviewed, but suppression remains a future-regression blind spot.
- documentation has drifted materially: the current README references `Assets/_Game/Runtime` and `docs/ARCHITECTURE.md`, neither of which exists on the audited `main` snapshot.
- the repository root contains many historical audits, agent-specific instruction files, committed test-result XMLs, transcripts, archives, generated material, and directories that current `.gitignore` rules say should be ignored. That creates navigational and source-of-truth noise.
- CI policy is inconsistent: `.github/workflows/ci.yml` states Godot is the only active engine and CI must not invoke Unity, while `.github/workflows/build.yml` performs Unity Windows/WebGL builds on `main`.
- the current branch metadata reports no enforced required status checks despite a strong CI workflow, so the quality gate appears policy-based rather than branch-protection-enforced.

Overall assessment: **technically ambitious and increasingly well-hardened, but carrying high integration entropy from simultaneous engine migration, content expansion, legacy compatibility, and a very broad feature surface.** The next engineering gains should come from reducing authority ambiguity and orchestration concentration rather than adding more systems.

---

## 2. Audit scope and evidence model

### Directly inspected current-main sources

The review directly inspected or enumerated the following current repository areas/files:

- repository metadata and current `main` head;
- root directory inventory;
- `README.md`;
- `.gitignore`;
- `.github/workflows/ci.yml`;
- `.github/workflows/build.yml`;
- `Ashfall.csproj`;
- `Ashfall.Core/Ashfall.Core.csproj`;
- `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`;
- `project.godot`;
- `scenes/Main.tscn`;
- `src/Main.cs`;
- `src/Host/HostCli.cs`;
- `src/Bridge/BridgeGap.cs`;
- `scripts/ci/godot-asset-gate.sh`;
- `ProjectSettings/ProjectVersion.txt`;
- `Packages/manifest.json`;
- `Assets/_Game/` topology;
- `Assets/_Game/Core/GameState.cs`;
- `Assets/_Game/Core/TimeSystem.cs`;
- `Assets/_Game/Core/SaveSystem.cs`;
- `Assets/Ashfall.Core/Ports.cs`;
- `Assets/Ashfall.Core/SaveChecksum.cs`;
- `Assets/StreamingAssets/Data/` catalog directory;
- `docs/ASHFALL_CODE_INDEX.md`;
- `10LOOP_AUDIT_REPORT.md`;
- latest current-main merge commit and regression-test diff.

The repository-wide directory/tree inventories were also used to identify the major gameplay, tooling, documentation, generated-asset, test, and migration surfaces.

### Confidence labels used in this report

- **Verified current:** directly observed in files or GitHub metadata at snapshot `04b1f465...`.
- **Documented historical:** recorded in an audit/code-index file in the repository, but not rerun here.
- **Static risk/inference:** architectural risk inferred from current structure; not a demonstrated runtime failure.

This distinction matters because this repository changes quickly and contains several documents that describe older branch states.

---

## 3. Repository identity and top-level shape

The repository is public, C#-dominant, MIT-licensed, and uses `main` as its default branch. It is not archived or a fork.

The top level is unusually dense. It includes:

- active source/build surfaces: `Assets/`, `src/`, `scenes/`, `scripts/`, `Packages/`, `ProjectSettings/`, `Ashfall.csproj`, `Ashfall.Core/`, `Ashfall.Core.Tests/`, `project.godot`;
- many project/architecture/design documents;
- many historical audits and phase reports;
- numerous AI-agent instruction variants (`AGENTS.md`, `CLAUDE.md`, `CODEX.md`, `QWEN.md`, `GOOSE.md`, `MIMOCODE.md`, `CRUSH.md`, `VIBE.md`, etc. plus agent-specific directories);
- generated/prompt assets and design material;
- committed Unity test-result XMLs;
- transcripts and temporary process notes;
- `_quarantine_legacy/` and `deprecated_audits/`;
- snapshots and archives.

This is not merely cosmetic. In a migration-heavy project, root-level noise increases the cost of answering basic questions such as “what is authoritative?”, “what is current?”, and “what must a contributor read first?”.

### Recommendation

Create a small, canonical root surface:

- `README.md`
- `AGENTS.md` (or one tool-neutral contributor policy)
- `LICENSE`
- build/project files
- active source directories
- `docs/`

Move historical audits to `docs/audits/archive/`, agent-specific prompt variants to a clearly non-authoritative `docs/agents/` or remove them, and move raw test-result XML/transcripts to CI artifacts rather than version control.

---

## 4. Architecture: the actual source-of-truth model

### 4.1 Engine-agnostic target: `Assets/Ashfall.Core/`

The cleanest architectural decision in the repository is that engine-agnostic gameplay source physically lives under `Assets/` so Unity can compile it, but is also consumed by normal .NET and Godot projects.

`Ashfall.Core/Ashfall.Core.csproj` explicitly disables default compile items and includes:

```xml
<Compile Include="../Assets/Ashfall.Core/**/*.cs" />
```

It targets `netstandard2.1`, is deterministic, and references `System.Text.Json`.

`Ashfall.csproj` includes the exact same core source:

```xml
<Compile Include="Assets/Ashfall.Core/**/*.cs" />
```

The xUnit project references the wrapper `Ashfall.Core.csproj`, so the same files are compiled for tests as well.

This establishes a strong intended rule:

> gameplay logic should move from engine-specific code into `Assets/Ashfall.Core/`, not be rewritten independently for Godot.

That rule should be treated as the primary architecture invariant.

### 4.2 Unity legacy/gameplay surface: `Assets/_Game/`

`Assets/_Game/` remains enormous and spans most gameplay domains, including current directories such as:

- `AI`
- `Audio`
- `Core`
- `Crafting`
- `Data`
- `Economy`
- `Editor`
- `Encounters`
- `Endgame`
- `Environment`
- `Events`
- `Expeditions`
- `Factions`
- `Inventory`
- `Medical`
- `Narrative`
- `Quests`
- `Radiation`
- `Settings`
- `Shelter`
- `Simulation`
- survivor/UI/world/supporting systems in the wider tree.

`docs/ASHFALL_CODE_INDEX.md` describes this as the legacy Unity body and `Assets/Ashfall.Core/` as the migration target. The code index gives historical file/LOC estimates; those figures are useful for scale but should not be treated as current exact counts without regeneration.

### 4.3 Godot host: `src/` + `scenes/`

`project.godot` is configured for Godot C# and starts from:

```ini
run/main_scene="res://scenes/Main.tscn"
```

`scenes/Main.tscn` exists on current `main` and binds `res://src/Main.cs`, so the default Godot boot path is internally coherent.

`src/Main.cs` is the active composition root. Its first few hundred lines alone reveal a very broad responsibility surface: Year of Ash, Phantom Memory, Phase 0, Dose Ledger, Muster, Verdict, Black Flotilla, Deep Coast, expeditions, combat, narrative, medical, world, radio, crafting, caravans, inventory, survivors, economy, utility AI, journal, Holdfast, Duty Roster, expansions, Silent Foundry, disease, many UI overlays/panels, game state, diagnostics, save dirty flags, and CLI dispatch.

This is functional, but it is now an architectural choke point.

### 4.4 Compatibility bridge: `src/Bridge/`

The bridge exists so the Godot aggregate assembly can compile Unity-coupled legacy code. `BridgeGap.cs` has a particularly good failure policy:

- **semantic gap:** throws by default;
- **cosmetic gap:** logs once and continues;
- **genuine headless no-op:** intentionally does nothing.

The design explicitly rejects the dangerous pattern of returning a plausible default from an unimplemented compatibility member. That is excellent defensive engineering for a compatibility layer.

However, a strong bridge does not make the bridge free. Every legacy subsystem compiled into the Godot assembly increases the semantic surface that must remain faithfully emulated or proven unused.

---

## 5. Build graph and toolchain

### 5.1 Godot aggregate project

`Ashfall.csproj` currently uses:

```xml
<Project Sdk="Godot.NET.Sdk/4.7.1">
<TargetFramework>net8.0</TargetFramework>
```

It compiles:

- `src/**/*.cs`
- `scripts/**/*.cs`
- `Assets/Ashfall.Core/**/*.cs`
- `Assets/_Game/**/*.cs`

and references Sentry `6.9.0`.

This means the Godot host is not merely a thin binary consumer of the migrated core. It still type-checks the complete `_Game` C# surface using the compatibility bridge.

Benefits:

- migration can happen incrementally;
- legacy code remains compile-checked;
- engine-specific systems can be reused temporarily.

Costs:

- very large compile graph;
- bridge completeness remains load-bearing;
- duplicate/same-name domain abstractions can coexist;
- compile success can be confused with behavioral equivalence;
- nullable/warning noise from legacy code pressures the project toward suppression.

### 5.2 Core project

`Ashfall.Core` targets `netstandard2.1`. This is a reasonable compatibility target for sharing logic across environments, though it means newer APIs must remain behind host adapters.

### 5.3 Tests

`Ashfall.Core.Tests` targets `net9.0` with `RollForward=LatestMajor`, using xUnit, Moq, and Microsoft.NET.Test.Sdk.

Using a newer test target than the Godot runtime is acceptable for pure-core tests, but cross-host behavior still needs Godot-level gates because a net9 test environment cannot prove Godot/bridge semantics.
## 6. Compiler warning policy

`Ashfall.csproj` enables nullable analysis but suppresses a wide set:

```text
CS0108 CS0114 CS0414 CS0067
CS8600 CS8601 CS8602 CS8603 CS8604
CS8618 CS8625 CS8765 CS8619 CS8620
CS0649 CS0169
```

The inline comment explains why: Unity legacy code, serializer-populated fields, tuple nullability drift, and inspector-driven fields generate noise outside Unity's own compilation model.

The rationale is understandable, and historical audit notes show that the project has manually hunted important nullable defects. Still, suppressing `CS8602`/`CS8603`/`CS8604` globally on the aggregate host creates a future blind spot.

### Recommended refinement

Do not try to turn every warning on globally in one step. Instead:

1. keep suppressions for clearly mechanical legacy categories at the aggregate boundary;
2. enable strict nullable analysis with no `CS8602/03/04` suppression in `Assets/Ashfall.Core/` and new `src/Host` code;
3. isolate `_Game` legacy compilation into a separate project/props scope if feasible;
4. fail CI on new high-signal nullable warnings in migrated code.

This turns nullable correctness into a migration quality gate instead of an all-or-nothing cleanup project.

---

## 7. Core simulation design

### 7.1 `GameState`

`Assets/_Game/Core/GameState.cs` is intentionally simple and save-friendly. It holds lifecycle phase, day, pause state, and accessibility safe-mode state, with behavior delegated to dedicated systems. This is a healthy pattern: a serializable state object should not become a hidden service locator.

### 7.2 `TimeSystem`

`Assets/_Game/Core/TimeSystem.cs` shows several good simulation properties:

- deterministic advancement;
- separate simulated time scale;
- clamped time scale;
- large deltas split into bounded game-hour steps;
- explicit hour/day events;
- save state uses public fields compatible with Unity `JsonUtility`;
- restore does not replay historical tick events;
- fast-forward state intentionally does not persist.

The comments demonstrate awareness of common simulation bugs: skipped daily ticks during hitches/fast-forward, auto-properties being ignored by Unity serialization, and replaying day-gated systems during load.

### 7.3 Event-driven architecture

Across the code index and source, many systems communicate through tick events, state events, and buses. This scales well when ownership is clear, but the repository itself has historically flagged dual-tick/double-registration risks. Any system migration should explicitly answer:

- who owns the system instance?
- who ticks it?
- who subscribes/unsubscribes it?
- is the system included in save capture/restore?
- is the same semantic system alive in both legacy and Core paths?

A small machine-readable system registry would help make these answers testable.

---

## 8. Determinism and cross-host portability

The strongest part of `Assets/Ashfall.Core` is its explicit host abstraction policy.

`Ports.cs` defines:

- `IJsonSerializer`
- `IFileIO`
- `ILog`
- `IClock`
- `ISeededRng`

The comments are not generic abstractions for abstraction's sake. They encode specific invariants:

- no direct `DateTime.Now` for simulation calendar;
- same RNG seed must produce the same sequence in both hosts;
- save serialization must be host-independent;
- file and logging behavior belongs to adapters.

This is exactly the right shape for an engine migration.

Historical audit material reports additional static tests against nondeterministic sources in Core and reproducibility tests for `SeededRng`. Those should remain permanent gates.

### Recommendation

Add one canonical “cross-host deterministic scenario” snapshot test that advances a representative simulation through a fixed seed/day sequence and compares a canonical state digest. Unit tests catch local regressions; a scenario digest catches unexpected composition-level divergence.

---

## 9. Save/persistence architecture

Persistence is both a major strength and one of the largest structural risks.

### 9.1 Serializer-independent checksum design

`Assets/Ashfall.Core/SaveChecksum.cs` is a strong solution to cross-host corruption false positives.

Instead of hashing serialized JSON text, it canonicalizes object state by:

- walking public instance fields;
- sorting field names ordinally;
- using invariant culture;
- length-prefixing strings;
- canonicalizing primitive values;
- normalizing null string vs empty string and null collection vs empty collection where host serializers differ;
- excluding `[NonSerialized]` fields;
- excluding the root checksum field;
- guarding against excessively deep/cyclic object graphs;
- SHA-256 hashing the canonical representation.

This directly addresses the known incompatibility between Unity `JsonUtility` formatting/null behavior and `System.Text.Json`.

The architecture principle here is excellent:

> integrity must be a function of game state, not serializer formatting.

### 9.2 Legacy `SaveSystem` coupling

`Assets/_Game/Core/SaveSystem.cs` is a partial class but its visible dependency registry is enormous. It references systems from environment, needs, radiation, shelter, inventory, survivors, medical, economy, events, AI, crafting, world, narrative, factions, encounters, endgame, expansion content, hazards, perks, quests, and many more.

This broad coverage is good for preventing omitted state, but it makes the type a classic high-coupling persistence coordinator. The risk is not necessarily runtime performance; it is change amplification:

- adding a system may require touching central persistence wiring;
- migration can leave a system represented in two save layers;
- partial restore failures can create hybrid state;
- state versioning/migration becomes difficult to reason about globally;
- testing every combination becomes combinatorial.

### Recommended persistence direction

Move toward a registry of explicit save participants with stable IDs and versioned envelopes:

```text
SaveCoordinator
  -> participant id
  -> schema version
  -> CaptureState()
  -> ValidateState()
  -> RestoreState()
  -> Migrate(oldVersion)
```

Keep restore transactional: deserialize/validate/migrate all participants first, then apply, or fail before mutating live systems.

Historical audit notes already emphasize deep-copy capture, tamper rejection, migration chains, and fail-fast/transactional restore. The next step is making those guarantees structural rather than coordinator-convention-based.

---

## 10. Godot composition root and host sessions

`src/Main.cs` is now a major architectural hotspot.

It owns or references a very large number of:

- expansion host sessions;
- subsystem host sessions;
- panels and detail panels;
- dirty/save-coalescing flags;
- core demo/runtime sessions;
- journal/event adapters;
- game flow state;
- diagnostics timers;
- setup/lifecycle behavior;
- command-line self-test dispatch.

The design has clearly grown organically as more systems were migrated and exposed through Godot.

### Why this matters

A giant composition root can still be valid if it only wires dependencies. Here, however, the file also coordinates UI, saves, diagnostics, session setup, state flow, and self-test entry points. That increases the probability of:

- initialization-order coupling;
- accidental duplicate session ownership;
- save dirty flags being forgotten;
- teardown/unsubscribe inconsistencies;
- hidden dependencies between UI and domain sessions;
- merge conflicts as multiple features integrate simultaneously.

### Recommended decomposition

Split by responsibility, not merely by file size:

- `GameCompositionRoot` — construct services/sessions only;
- `GameSessionCoordinator` — new/load/start/end lifecycle;
- `SaveFlushCoordinator` — dirty tracking and save flushing;
- `UiCompositionRoot` — panel creation and navigation;
- `DiagnosticsCoordinator` — diagnostics/logging refresh;
- `HostCliRunner` — command dispatch and test harness lifecycle;
- feature modules for expansion-specific session wiring.

Godot nodes should receive already-constructed domain/session dependencies where practical rather than construct the entire application graph themselves.

---

## 11. Headless test/diagnostic CLI

`src/Host/HostCli.cs` is a significant strength. It exposes dozens of targeted actions, including current entries for:

- Holdfast;
- Ice Road;
- Census;
- Core;
- save/tamper checks;
- Brine;
- combat;
- Muster;
- endings;
- journal/UI;
- dashboard/player panels;
- bridge;
- Duty Roster/Standing Record/Crossing;
- Greenhouse/Silent Foundry/Disease;
- expansion hub saves;
- expedition/encounter bridge;
- medical/narrative/survivors/world/economy;
- Utility AI;
- RNG wiring;
- data integrity;
- caravans;
- asset registry/coverage;
- Day 1 playable flow and Day 1→2 milestone;
- UI layout/settings/playable shell;
- shelter hazard/operations loops;
- audio;
- Deep Coast;
- Warlords;
- Black Flotilla;
- radio;
- UI snapshots.

This is the right strategy for proving migrated behavior without requiring a human to drive the full game.

### Improvement opportunity

The self-test list is now so large that discoverability itself needs structure. Replace or supplement the large enum/switch parser with a declarative command registry:

```csharp
record HostCommand(string Flag, string Category, Func<int> Run, string Description);
```

Then generate help output and CI command groups from the same registry. That prevents parser/help/CI drift.

---

## 12. Gameplay-domain coverage

The repository is not a prototype with a few isolated mechanics. It contains broad, interconnected systems.

### 12.1 Survivors and needs

The code index describes survivor state spanning identity/profession, health, hunger, thirst, fatigue, warmth, morale, radiation, skills, traits, assignments, afflictions, relationships, inventory/equipment, and narrative state.

Needs are systemic rather than cosmetic: hunger/thirst/fatigue/warmth/morale/health/hygiene interact with survival decisions and other systems.

### 12.2 Radiation

Radiation is a first-class simulation domain with dose accumulation, protection, contamination, equipment durability/protection, treatment/knowledge, and environmental exposure.

Historical audits identified and later report fixing a real Godot host gap where worn gear was not threaded into radiation exposure. This is a useful case study: domain logic was correct in Core, but the host composition layer omitted a dependency. It demonstrates why host-level integration tests are necessary even when Core unit tests are strong.

### 12.3 Environment/world

The environment layer contains weather, fallout/radiation mapping, temperature, photoperiod, hazards, world phase, map generation/evolution, and other environmental processes. This supports the project's central design identity: survival pressure emerges from interacting systems rather than scripted stat drains alone.

### 12.4 Shelter

Shelter systems include modules, shielding, power, water, atmosphere/filtration, structural integrity, maintenance, vermin/waste, flooding, tunneling/excavation, hatch/security and other infrastructure systems.

This is a natural candidate for bounded-context extraction because it has many internal subdomains but a relatively clear public surface: resources in, operations, hazards, state out.

### 12.5 Inventory/crafting/economy

The inventory/crafting/economy stack is deeply coupled to most other gameplay:

- equipment and carried items affect survivability;
- crafting consumes inventory and creates jobs/results;
- economy/trading uses catalog values and availability;
- maintenance and production systems consume resources;
- quests/encounters can gate or mutate inventory.

The latest merge commit contains regression tests for a particularly important invariant: **validate all preconditions before consuming resources**. The diff includes cases where transcription, kitchen prep, and maintenance previously risked consuming early ingredients/parts before later validation failed. Those tests should be generalized into a shared transactional-action pattern.

### 12.6 AI

The project uses utility AI rather than runtime LLM decision-making for survivor behavior. This is appropriate for deterministic, testable simulation.

The migration documentation records a historical fork between Core/Godot utility AI and legacy Unity AI; current host self-tests include Utility AI and RNG wiring commands.

### 12.7 Factions, narrative, events, quests, expansions

The repository has extensive faction NPC/state logic, event runners, radio/narrative systems, moral/knowledge tracking, quests, multiple named expansions, endgame/victory systems, and many catalog-driven content layers.

The most important architecture rule for this content volume is that narrative/content definitions should remain data-driven when possible and should reference stable IDs rather than embed duplicated gameplay data in code.

---

## 13. Data architecture

`Assets/StreamingAssets/Data/` is intended to be the content authority shared by engines.

The directory currently contains a large number of JSON catalogs, including character, combat, expansion, medical/dependency, item, radio/narrative and other content files. Existing architecture documentation lists core catalogs such as items, survivors, locations, events, characters, Holdfast content, crossing/duty-roster/standing-record content, greenhouse/year-of-ash content, currents, echoes, world history, faction lore, recipes, radio, phantom triggers, final wishes, and door encounters.

One important sign of documentation drift: `docs/ASHFALL_CODE_INDEX.md` refers to roughly ~55 catalogs in one section, while the later `10LOOP_AUDIT_REPORT.md` records parsing 94 top-level catalogs during a 2026-08-17 audit extension. The exact current count should therefore be generated automatically rather than documented manually.

### CI protection

`.github/workflows/ci.yml` directly parses every `*.json` under `Assets/StreamingAssets/Data` and fails if:

- the authoritative directory is missing;
- no JSON files exist;
- any JSON fails parsing.

The headless gate then runs `--data-integrity-selftest`, which is intended to validate semantic integrity beyond syntax.

### Recommendation

Generate a catalog manifest during CI containing:

- file count;
- entity count by catalog;
- authored ID count;
- reuse/foreign-reference count;
- duplicate/missing-reference errors;
- schema/version hash.

Commit only the schema, not the generated counts. Publish counts as CI artifacts or build metadata so documentation does not drift.

---

## 14. CI/CD assessment

### 14.1 Primary CI (`ci.yml`)

The primary workflow is well structured:

1. repository/data validation;
2. JSON syntax validation;
3. Godot project version pin checks;
4. .NET 8/9 setup;
5. restore/build/test of `Ashfall.Core.Tests`;
6. artifact upload for test results;
7. Godot .NET setup;
8. aggregate `Ashfall.csproj` build;
9. Godot import;
10. canonical headless asset/gameplay gates.

`scripts/ci/godot-asset-gate.sh` currently runs:

- `dotnet build Ashfall.csproj`;
- Godot import;
- asset registry self-test;
- data integrity self-test;
- bridge self-test;
- disease self-test;
- expansions self-test;
- Black Flotilla self-test;
- radio self-test.

This is a strong baseline.

### 14.2 Coverage gap between available self-tests and canonical gate

`HostCli` exposes far more self-tests than `godot-asset-gate.sh` currently runs. That is reasonable for runtime length, but it means many tests are only valuable if another workflow or scheduled gate invokes them.

Recommended split:

- **PR smoke gate:** current fast canonical set;
- **PR changed-area gate:** run additional commands based on touched paths;
- **nightly deep gate:** run all deterministic host self-tests and UI snapshots;
- **release gate:** full deep gate plus packaging/build verification.

### 14.3 Unity/Godot policy contradiction

`ci.yml` begins with:

> Godot is the only active engine. CI must not invoke Unity tooling.

But `.github/workflows/build.yml` performs Unity Windows and WebGL builds on pushes to `main`.

This may be intentional—Godot for active development, Unity for release/legacy packaging—but the policy text does not say that. The workflow's own comments are also stale: they refer to `ci.yml` running Unity EditMode/PlayMode/Linux build steps, while current `ci.yml` is Godot/.NET based.

The result is contributor ambiguity about whether Unity is:

- deprecated;
- compatibility-only;
- still a supported release target;
- required for production validation.

Write this policy once in `docs/ENGINE_SUPPORT_POLICY.md` and make workflow comments match it.

### 14.4 Unity build license asymmetry

In `build.yml`, the Windows job has:

```yaml
if: ${{ env.UNITY_LICENSE != '' }}
```

The WebGL job does not have the equivalent guard, despite using the same Unity credentials. If repository secrets are absent or unavailable in a context, the WebGL job can fail differently from Windows. Align the guards or deliberately fail both with a clear prerequisite message.

### 14.5 Branch protection

The current `main` branch metadata reports:

- branch marked protected;
- `protection.enabled: false`;
- no required status-check contexts/checks.

At minimum, verify repository rulesets. If there is no newer ruleset enforcing CI, require the primary `ASHFALL CI` checks before merge. A strong workflow that can be bypassed is weaker than its test quality suggests.

---

## 15. Historical test evidence

`10LOOP_AUDIT_REPORT.md` is valuable because it records actual previously executed batteries and concrete defect discoveries rather than generic claims.

Examples from the ledger include:

- data-integrity duplicate-ID false positives corrected;
- determinism/static coupling gates added;
- cross-host save checksum/tamper behavior hardened;
- nullable crash-risk categories reviewed;
- needs/radiation save round-trips added;
- Journal tests added;
- Utility AI fork reviewed;
- equipped-gear radiation bridge gap discovered and later wired/fixed;
- stable hashing introduced for cross-process determinism;
- data-rule compliance gates added;
- save-store completeness reviewed;
- repeated deterministic stability sweeps.

The ledger records thousands of xUnit tests passing at various checkpoints and multiple green Godot self-test batteries.

Again: these are **historical results**, not current reruns by this report.

### Key lesson from the audit history

The most important recurring failure mode is not “bad algorithm in a class.” It is **cross-system integration mismatch**:

- correct Core logic not wired by host;
- resource consumed before a later precondition fails;
- serializer differences across hosts;
- same-name duplicate types drifting;
- IDs reused/defined ambiguously;
- a system omitted from save/restore;
- host bridge returning an incorrect default.

Testing strategy should therefore emphasize composition/invariant tests, not only per-class unit tests.

---

## 16. Documentation accuracy and source-of-truth drift

This is one of the clearest current problems.

### Verified README drift

Current `README.md` says production systems live under:

```text
Assets/_Game/Runtime
```

That directory does not exist on the audited `main` snapshot.

The README also links:

```text
docs/ARCHITECTURE.md
```

That file does not exist on the audited snapshot.

The actual architecture is better represented by `docs/ASHFALL_CODE_INDEX.md`, though that document itself contains historical branch/state references and stale counts.

### Why this is high leverage

For a normal project, stale docs are annoying. For this project, they are dangerous because there are several plausible source authorities:

- Unity `_Game` legacy;
- migrated Core;
- Godot host;
- data catalogs;
- generated ScriptableObjects/assets;
- historical audits.

A contributor following the wrong document can easily add behavior to the wrong layer and increase migration debt.

### Recommendation

Replace the README's architecture section with a five-line authority table:

| Concern | Authority |
|---|---|
| Engine-agnostic gameplay logic | `Assets/Ashfall.Core/` |
| Active Godot host/UI | `src/`, `scenes/` |
| Shared content data | `Assets/StreamingAssets/Data/` |
| Unity legacy awaiting migration | `Assets/_Game/` |
| Historical/deprecated material | archive/quarantine only |

Then generate `docs/CODE_INDEX.generated.md` from the tree so file counts and paths cannot silently age.

---

## 17. Repository hygiene

Current `.gitignore` includes rules for many build outputs, Godot cache, test artifacts, generated AI assets, audit outputs, and `_quarantine_legacy/`.

However, the current repository still contains tracked examples of material matching the spirit of those ignore policies:

- `_quarantine_legacy/`;
- `generated_AIassets/`;
- multiple committed test-result XMLs (`editmode-*`, `playmode-*`, `uxml-probe.xml`);
- `session_transcript.txt`;
- `touched_files.txt`;
- small process/instruction text files;
- many top-level audit reports;
- a Unity archive and checksum;
- numerous AI-agent-specific prompt/rule files.

`.gitignore` does not remove already-tracked files, so adding ignore rules alone is not sufficient.

### Effects

- larger/cluttered checkout;
- harder search results;
- accidental use of stale audit information;
- contributors confuse generated assets with authored source;
- repeated tool-specific rules can contradict one another;
- root directory no longer communicates project architecture.

### Recommended cleanup

Perform a dedicated repository-hygiene PR:

1. classify each root artifact as `active`, `docs`, `generated`, `historical`, or `temporary`;
2. move historical docs under `docs/archive/`;
3. delete committed CI outputs and rely on workflow artifacts;
4. untrack ignored generated/quarantine directories where safe;
5. preserve required LFS source assets intentionally;
6. consolidate agent rules into one canonical source, generating tool-specific copies only if unavoidable.

Do this separately from gameplay changes so the diff is reviewable.

---

## 18. Dependency/service and privacy surface

The Unity package manifest includes a broad set of online/service SDKs. Even if many are inactive in the Godot host, the repository needs an explicit policy for which shipped target initializes which services.

Areas requiring deliberate review for any production release include:

- analytics/telemetry;
- remote configuration;
- ads/mediation/LevelPlay;
- cloud save/code/economy;
- friends/leaderboards;
- moderation;
- push notifications;
- Sentry in the Godot aggregate project.

For each service, document:

- target platforms;
- whether enabled by default;
- consent basis;
- initialization point;
- data sent;
- offline behavior;
- failure behavior;
- whether save/gameplay semantics depend on it.

The ideal survival simulation remains playable and deterministic without network services. Service outages should degrade ancillary features, not the simulation core.

---

## 19. Performance/scalability risks

No profiler was run, so this section is static-risk analysis.

### 19.1 Main composition/root UI scale

`src/Main.cs` has a huge number of sessions and panels. If many are eagerly constructed, startup cost, signal wiring, memory footprint, and UI update overhead can grow significantly.

Recommendation: lazy-load heavy feature panels and expansion sessions where state ownership allows it. Keep domain state resident; instantiate presentation only when needed.

### 19.2 Day/hour fan-out

The simulation has many systems driven by hourly/daily ticks. TimeSystem correctly avoids skipped boundaries, but a large fan-out can create frame spikes when fast-forward crosses many boundaries.

Recommendation: instrument tick duration per subsystem and produce a headless performance report for a 30/100/365-day deterministic simulation.

### 19.3 Utility AI

Utility AI generally scales as actors × candidate actions × scoring work. As the number of survivor actions grows, profile decision frequency and prune candidate sets using eligibility masks/context partitions before scoring expensive considerations.

### 19.4 Save size and frequency

The host already uses dirty flags/save coalescing in several areas, which is good. Continue measuring serialized save size by participant and reject accidental unbounded-history growth in tests.

The latest merge's regression around removing completed kitchen jobs from active state is exactly the type of long-campaign memory/save-bloat protection that should be applied to all queue/history systems.

---

## 20. Reliability invariants worth standardizing

The latest source and audit history repeatedly imply a common set of invariants. These should become reusable helpers/tests rather than being reimplemented ad hoc.

### 20.1 Transactional resource mutation

For any action consuming multiple resources:

1. validate every predicate;
2. compute full cost;
3. verify full availability;
4. apply mutation atomically;
5. create/advance state only after mutation succeeds.

Regression tests in the latest merge already protect this for several systems.

### 20.2 Capture/restore symmetry

Every stateful system should have a generic contract test:

- mutate all meaningful fields;
- `CaptureState`;
- serialize/deserialize;
- restore into fresh instance;
- compare canonical state;
- mutate captured object and prove live state is not aliased.

### 20.3 Determinism

For every random system:

- same seed + same inputs => same state digest;
- different seed => allowed divergence;
- no `DateTime.Now`, process-randomized hashes, or host iteration-order dependence in simulation state.

### 20.4 Host wiring completeness

Every required Core port should have a host integration test proving it is populated. The historical radiation-gear bug is the model case.

### 20.5 Bounded collections

Queues/logs/history arrays stored in save state must either:

- have a documented maximum;
- compact/archive terminal entries;
- or be excluded from persistent save state.

Long-campaign simulation tests should assert bounds.

---

## 21. Highest-priority findings

### P1 — Enforce architecture/source authority

**Finding:** current docs disagree with current tree and with each other about active engine/source locations.
**Evidence:** README references nonexistent `Assets/_Game/Runtime` and `docs/ARCHITECTURE.md`; CI says Godot-only while a Unity build workflow still runs.
**Risk:** new code lands in the wrong layer; migration debt increases.
**Action:** publish one engine-support/source-authority policy and make README/workflows/code index derive from it.

### P1 — Reduce `src/Main.cs` orchestration concentration

**Finding:** the active Godot root owns a very large number of sessions, UI panels, dirty flags, diagnostics, lifecycle and CLI flows.
**Risk:** initialization/save/UI coupling, merge conflicts, difficult ownership reasoning.
**Action:** split composition, session lifecycle, saves, UI navigation, diagnostics, and CLI dispatch into dedicated coordinators.

### P1 — Decompose persistence coordination

**Finding:** the `_Game` `SaveSystem` partial type aggregates an extremely broad dependency set.
**Risk:** omitted state, partial restore, migration/version complexity, large blast radius.
**Action:** continue toward stable-ID participant registry + schema version + transactional restore.

### P1 — Make CI actually required

**Finding:** current branch metadata exposes no enforced required status checks.
**Risk:** strong CI can be bypassed.
**Action:** verify rulesets and require primary data/core/Godot gate checks before merge.

### P2 — Narrow warning suppression

**Finding:** aggregate Godot project suppresses important nullable warnings globally.
**Risk:** new migrated-code defects can hide among intentional legacy suppressions.
**Action:** strict warning policy for Core/new host code; legacy-specific suppression scope.

### P2 — Resolve active-engine policy ambiguity

**Finding:** Godot-only primary CI coexists with Unity release builds.
**Risk:** unclear support obligations and release truth.
**Action:** explicitly define “active development host”, “legacy compatibility host”, and “shipping targets”.

### P2 — Repository cleanup

**Finding:** root tracks large amounts of historical/generated/test/process material, including paths now ignored by `.gitignore`.
**Risk:** source-of-truth confusion and maintenance noise.
**Action:** archive/delete/untrack in a dedicated hygiene PR.

### P2 — Expand deep-gate scheduling

**Finding:** HostCli exposes far more self-tests than the canonical PR gate executes.
**Risk:** rarely invoked checks silently rot.
**Action:** nightly full self-test matrix and changed-area PR routing.

---

## 22. Strengths to preserve

Do not lose these while simplifying the architecture:

1. **One physical Core source tree compiled by all consumers.** This is the migration's most important structural invariant.
2. **Data authority in JSON.** Keep content IDs/catalogs host-neutral.
3. **State-based SaveChecksum.** Serializer independence is essential for cross-host saves.
4. **Seeded RNG and clock ports.** Keep nondeterminism outside the simulation.
5. **BridgeGap fail-loud semantics.** Never replace an unimplemented semantic bridge with a plausible default.
6. **Headless host self-tests.** They catch wiring failures unit tests cannot.
7. **Regression tests attached to concrete bugs.** The latest merge demonstrates good practice around transactional resource mutation, roster APIs, expiry semantics, expedition availability, and bounded completed-job lists.
8. **Audit culture.** The repository has a useful history of converting findings into tests rather than merely documenting them.

---

## 23. Recommended roadmap

### Immediate: next 1–2 days

1. Fix README architecture paths and remove references to nonexistent `Assets/_Game/Runtime` / `docs/ARCHITECTURE.md`.
2. Add `docs/ENGINE_SUPPORT_POLICY.md` clarifying Godot vs Unity status.
3. Verify/enable branch rules requiring `ASHFALL CI`.
4. Align Unity build workflow comments with reality and fix the WebGL license guard asymmetry.
5. Add a CI check that every path linked from the README exists.
6. Add a CI check that generated/ignored test outputs are not newly tracked.

### Short term: next week

1. Split `src/Main.cs` into application coordinators.
2. Generate HostCli help/registry from declarative command metadata.
3. Add nightly full host self-test matrix.
4. Introduce stricter nullable warnings for `Assets/Ashfall.Core` and newly migrated `src` code.
5. Create a system-ownership manifest listing tick owner, save participant ID, host adapter, and source authority.
6. Run repository-hygiene cleanup in an isolated PR.

### Medium term

1. Stop compiling migrated legacy `_Game` systems into the Godot aggregate once a bounded context is fully ported.
2. Convert persistence to participant-based versioned registry with transactional restore.
3. Add deterministic scenario digest tests across major gameplay slices.
4. Add long-campaign performance/save-growth tests.
5. Generate architecture/code/catalog indexes in CI rather than hand-maintaining counts.
6. Reduce duplicate/forked types and keep explicit exceptions documented where host-specific behavior is intentional.

---

## 24. Suggested target architecture

```text
Assets/StreamingAssets/Data/
        |
        v
Assets/Ashfall.Core/                 <-- single gameplay/data-domain authority
  Domain/
  Application/
  Persistence/
  Ports/
        |
        +------------------------+
        |                        |
        v                        v
src/GodotHost/                 UnityLegacyAdapter/
  Composition/                   (only while required)
  PersistenceAdapters/
  UI/
  Diagnostics/
  CLI/
        |
        v
scenes/
```

The goal is not “rewrite everything for Godot.” The goal is:

> **Core owns game truth; hosts own presentation, filesystem/platform integration, and engine lifecycle.**

When that boundary is complete, the compatibility bridge becomes a shrinking migration tool instead of permanent infrastructure.

---

## 25. Files/documents that should become canonical

Recommended canonical documentation set:

- `README.md` — concise project status, boot/build commands, authority table.
- `docs/ARCHITECTURE.md` — create this for real, or stop linking it.
- `docs/ENGINE_SUPPORT_POLICY.md` — Godot/Unity support status.
- `docs/PERSISTENCE.md` — envelope/checksum/version/migration/transaction rules.
- `docs/DATA_AUTHORITY.md` — catalog schema/ID rules.
- `docs/TESTING.md` — xUnit + HostCli + PR/nightly/release gates.
- `docs/generated/CODE_INDEX.md` — generated tree/domain index.
- `docs/audits/archive/` — historical reports, clearly marked non-authoritative.

`docs/ASHFALL_CODE_INDEX.md` contains excellent engineering knowledge, especially around bridge behavior, checksums, determinism, and past failure modes. Preserve that knowledge, but separate timeless architecture rules from branch-specific “active work” notes and generated counts.

---

## 26. Final assessment

This repository's engineering problem is no longer “can the game systems be built?” There is already a very large and sophisticated simulation surface.

The central challenge is now **maintaining semantic coherence while the architecture migrates underneath a rapidly expanding game**.

The project has already built many of the right tools for that challenge:

- deterministic Core interfaces;
- shared data authority;
- serializer-independent save integrity;
- host-level self-tests;
- loud bridge semantics;
- rich regression testing;
- CI automation;
- documented debugging lessons.

The highest-value next move is to use those tools to **shrink the number of places where truth can live**.

If the project consistently enforces:

1. gameplay truth in `Assets/Ashfall.Core/`;
2. content truth in `Assets/StreamingAssets/Data/`;
3. Godot presentation/lifecycle in `src/` + `scenes/`;
4. legacy `_Game` as a shrinking compatibility source, not a parallel authority;
5. required CI gates and generated architecture indexes;

then the migration can converge without sacrificing the depth already present.

If those boundaries remain ambiguous, the greatest risk is not a single catastrophic bug. It is cumulative divergence: duplicated state, mismatched host wiring, stale docs, warning suppression, overlapping save paths, and multiple implementations that all compile but no longer mean exactly the same thing.

The codebase is therefore best characterized as **feature-rich, extensively hardened, and architecturally promising, but still carrying significant migration and orchestration debt that should now be reduced deliberately before further large-scale subsystem growth.**

---

## 27. Source index used for this report

### Current source/configuration

- `README.md`
- `.gitignore`
- `Ashfall.csproj`
- `Ashfall.Core/Ashfall.Core.csproj`
- `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `project.godot`
- `scenes/Main.tscn`
- `src/Main.cs`
- `src/Host/HostCli.cs`
- `src/Bridge/BridgeGap.cs`
- `scripts/ci/godot-asset-gate.sh`
- `.github/workflows/ci.yml`
- `.github/workflows/build.yml`
- `ProjectSettings/ProjectVersion.txt`
- `Packages/manifest.json`
- `Assets/_Game/Core/GameState.cs`
- `Assets/_Game/Core/TimeSystem.cs`
- `Assets/_Game/Core/SaveSystem.cs`
- `Assets/Ashfall.Core/Ports.cs`
- `Assets/Ashfall.Core/SaveChecksum.cs`
- `Assets/StreamingAssets/Data/`

### Architecture/audit evidence consulted

- `docs/ASHFALL_CODE_INDEX.md`
- `10LOOP_AUDIT_REPORT.md`
- current `main` merge commit `04b1f465914b18d3b9c4bb8cd254802e2a3b6f30` and its regression-test diff
- repository root/tree inventories and current branch metadata

### Explicitly not claimed

- No current build was executed in this review.
- No current test suite was executed in this review.
- No Godot scene was launched interactively.
- No Unity build/editor test was launched.
- Historical green counts are not treated as present-tense proof.
- Existing audit documents were treated as evidence to cross-check, not as unquestioned current truth.
