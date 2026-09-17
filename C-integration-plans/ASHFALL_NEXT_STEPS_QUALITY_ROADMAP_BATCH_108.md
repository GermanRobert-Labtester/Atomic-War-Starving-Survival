# ASHFALL — Quality Roadmap Batch 108

## Theme: System Dependency Graph Enforcement — Prevent Circular Dependencies & Layering Violations

**Priority:** MEDIUM-HIGH<br>
**Risk:** Low for Steps 1–4, 6–7 (analysis/reporting/CI-gating only, no runtime changes). **Medium for Step 5** (Step 5 performs actual production refactors to break cycles — see Step 5 risk note).<br>
**Scope:** `Assets/Ashfall.Core/` (87 classes named `*System`/`*Session` at time of this review — re-verified independently via three separate counting methods (raw grep, unique-name sort, declaration count) that all agree at 87; re-run the count at implementation time since this number will drift as Core grows, see Review Notes), `Ashfall.Core.Tests/`, CI pipeline<br>
**Estimated effort:** 5–7 focused sessions if Step 4 finds few/no cycles. **Add 2–4 sessions if Step 4 finds actual cycles**, since Step 5's effort is unknowable until the baseline report exists — do not commit to a fixed total effort estimate before Step 4 completes.
**Rollback:** Steps 1–4, 6, 7 only add new files (manifests, extractor, tests, docs) and are trivially revertible via `git revert` with zero production impact. Step 5 changes production `Assets/Ashfall.Core/` code to break cycles — treat each cycle-breaking change as its own reviewable commit so it can be reverted independently if it destabilizes a system; do not batch multiple cycle fixes into one commit.

---

## Motivation

With 87 classes named `*System`/`*Session` in `Assets/Ashfall.Core/` (re-verified independently at review time — see Review Notes), constructor injection creates an implicit dependency graph that is never validated. Today nothing prevents:

- **Circular dependencies:** Survivors/ → Economy/ → Inventory/ → Survivors/ (untestable tangle) — **hypothetical example, not a confirmed existing cycle.** No cycle has actually been detected yet because no extractor exists (see Step 4). Treat this as an illustrative risk, not a known finding.
- **Layering violations:** Domain systems depending on host/orchestration types
- **Hidden coupling:** two domain systems taking on a growing, architecturally-unreviewed number of cross-domain constructor dependencies over time
- **Upward references:** Foundation-layer types importing from Domain or Orchestration

As the system count grows, these violations compound into compile-order fragility, test-setup nightmares, and refactoring paralysis. Enforcement must be automated — human code review does not scale indefinitely as constructor count grows.

**Correction — the original motivating examples were fabricated/wrong and have been removed from this section (see Review Notes at the bottom for the verification detail):**
- The claim "DutyRoster depending on Survivors + Expeditions + Expansions" is false. `DutyRosterSystem`'s only constructors are `DutyRosterSystem()` and `DutyRosterSystem(int seedSalt)` — zero system dependencies, one `int`. If DutyRoster is coupled to other domains, it happens through host wiring or shared save-store patterns, not constructor injection, and that's a materially different (and arguably fine) kind of coupling than what this roadmap is meant to catch.
- The claim "Phase0HostSession depends on 5 systems" is false and also mislocated — `Phase0HostSession` lives in `src/Host/` (Godot host layer), not `Assets/Ashfall.Core/`, so it is out of scope for a Core dependency-graph tool in the first place. Its actual constructor is `Phase0HostSession(int seed = DefaultSeed, ChemicalDependencySystem dependency = null)` — one primitive, one optional system reference. The "5" figure appears to conflate "systems this session internally constructs and coordinates" (it does construct several engines internally, e.g. `RadiationPhaseProgression`, `PhantomMemoryEngine`) with "constructor-injected dependencies," which are not the same metric for a graph-extraction tool that reads constructor parameter lists.
- `GameBootstrap.Phase0Expansion.cs`, referenced by AGENTS.md/other docs as "six systems constructed/registered/ticked," does not exist as a file anywhere in the repo, and no `class GameBootstrap` exists at all — the actual Godot host entry point with this kind of per-domain Setup/Save/Flush triad structure is `src/Main.cs` (per AGENTS.md's own H7 entry). Any step in this roadmap that assumes a `GameBootstrap` class is inspectable will fail; use `src/Main.cs` if a host-layer example is needed, and note that `src/Main.cs` is Godot host code, outside the `Assets/Ashfall.Core/` scope this batch targets.

---

## Architectural Layers (Proposed)

```
┌─────────────────────────────────────────────┐
│  HOST (Godot nodes, UI, input)              │  ← src/
├─────────────────────────────────────────────┤
│  ORCHESTRATION (sessions, coordinators,     │  ← *Session.cs,
│    ExpansionMasterSession)                   │     src/Main.cs (host-side, out of scope)
├─────────────────────────────────────────────┤
│  DOMAIN (per-domain systems: Medical/,      │  ← Ashfall.Core/<Domain>/
│    Economy/, Combat/, Survivors/, etc.)      │
├─────────────────────────────────────────────┤
│  FOUNDATION (Ports, Clock, Rng, Flags,      │  ← Ashfall.Core/Ports.cs,
│    SaveChecksum, CatalogIntegrity, DTOs)     │     Ashfall.Core/Clock/, etc.
└─────────────────────────────────────────────┘
```

**Correction:** there is no `GameBootstrap` or `*Bootstrap.cs` type inside `Assets/Ashfall.Core/` — the earlier version of this diagram named one that doesn't exist. The only confirmed Orchestration-layer type in Core is `ExpansionMasterSession`. `src/Main.cs` is the closest thing to a "bootstrap," but it is Godot host code (outside this batch's `Assets/Ashfall.Core/` scope) — do not classify it inside `LayerManifest.cs`.

**Allowed dependency direction:** Foundation ← Domain ← Orchestration ← Host<br>
**Forbidden:** Any arrow pointing downward (Host→Domain is fine; Domain→Host is forbidden).<br>
**Cross-domain rule:** Domain/A may depend on Domain/B only through an interface declared in Foundation, or through an explicit "bridge" type registered in a cross-domain manifest.

---

## Step 1 — Define Architectural Layers Formally

### Goal
Establish a machine-readable layer classification for every type in `Assets/Ashfall.Core/` so automated tools can enforce boundaries.

### Implementation
- Create `Assets/Ashfall.Core/Architecture/LayerManifest.cs` — a static class with `HashSet<string>` per layer containing namespace prefixes. **Before writing this file, run `grep -rho "^namespace [A-Za-z.]*" Assets/Ashfall.Core --include="*.cs" | sort -u` (or equivalent) to get the real, current namespace list — do not hand-guess it.** Starting point based on this batch's own verification pass (confirm against your own fresh grep, since Core evolves):
  - `Foundation`: `Ashfall.Core` (root-level types in `Ports.cs`, `HostDefaults.cs`), `Ashfall.Core.Clock`, `Ashfall.Core.Flags`, `Ashfall.Core.Save`, `Ashfall.Core.Events` (`IEventBus`/`SimpleEventBus` — string-based pub/sub, see Step 2 correction)
  - `Domain`: `Ashfall.Core.Medical`, `Ashfall.Core.Economy`, `Ashfall.Core.Combat`, `Ashfall.Core.Survivors`, `Ashfall.Core.Inventory`, `Ashfall.Core.Radiation`, `Ashfall.Core.Narrative`, `Ashfall.Core.Expeditions`, `Ashfall.Core.Weather`, `Ashfall.Core.Journal`, `Ashfall.Core.UtilityAI`, `Ashfall.Core.DutyRoster`, etc. — enumerate exhaustively from the grep output, not from this illustrative subset.
  - `Orchestration`: `Ashfall.Core.Expansions`, types matching `*MasterSession` (e.g. `ExpansionMasterSession`). **Note:** `*Bootstrap*` is not a useful pattern inside `Assets/Ashfall.Core/` — there is no `GameBootstrap` class in Core; the Setup/Save/Flush orchestrator lives in `src/Main.cs` (Godot host, out of this batch's scope). Do not write a rule expecting to classify a Core `Bootstrap` type that doesn't exist.
- Create `Assets/Ashfall.Core/Architecture/CrossDomainManifest.cs` — a static registry of sanctioned cross-domain dependencies (e.g., `Radiation` may reference `Inventory.WornGear` via the existing `Radiation.WornGear.FromInventory(Inventory.WornGear)` bridge — confirmed real, per AGENTS.md H2 and `InventoryGearBridgeTests`. **Precision note:** `InventoryGearBridgeTests` is not a standalone test file — it is a nested class defined inside `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs:364`, immediately after a comment referencing `SurvivorsHostSession` gear wiring. If this batch's documentation or `CrossDomainManifest.cs` comments cite it as evidence of a sanctioned bridge, cite the containing file (`NeedsRadiationSystemTests.cs`), not a nonexistent standalone `InventoryGearBridgeTests.cs`).
- Document layer rules in a comment block at top of `LayerManifest.cs`.

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly with new files.
- Every existing namespace in Core maps to exactly one layer (write a quick xUnit test that enumerates all types and asserts layer membership).

### Done when
- [ ] `LayerManifest.cs` exists, and its namespace list was generated from a fresh grep of the actual tree at implementation time (not copied from this doc's illustrative list)
- [ ] `CrossDomainManifest.cs` lists known sanctioned cross-domain deps, including at minimum the confirmed `Radiation`↔`Inventory.WornGear` bridge
- [ ] Classification test passes — no unclassified namespace, and the test fails loudly (not silently skips) if a new namespace is added later without classification

---

## Step 2 — Implement Dependency Extractor

### Goal
Build an analysis tool that reads all `*.cs` files in `Assets/Ashfall.Core/`, extracts constructor parameters for every system class, and produces a directed dependency graph.

### Implementation
- **Add `Microsoft.CodeAnalysis.CSharp` as a new `PackageReference` to `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`** — confirmed this package is not currently referenced anywhere in the project (only `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio` exist today). **Correction — the `4.11.0` example version in an earlier draft was an unverified guess, not a checked value.** `dotnet --version` in this environment reports **10.0.302**, and the project has no `global.json` pinning an older SDK, so a version chosen to "match the .NET SDK's Roslyn version" needs to be picked deliberately rather than copied from this doc: `Microsoft.CodeAnalysis.CSharp`'s NuGet version is decoupled from the target `TargetFramework` (`Ashfall.Core.Tests.csproj` targets `net9.0` with `RollForward: LatestMajor`) — it only needs to be a version whose minimum MSBuild/SDK requirement is satisfied by whatever SDK actually builds this project in CI and locally. Do not hardcode `4.11.0` (or any other specific version) in the implementation without first running `dotnet --version` on the actual build machine and checking that version's compatibility on the [Roslyn NuGet package compatibility table](https://learn.microsoft.com/en-us/visualstudio/extensibility/roslyn-version-support) — pin whatever that check produces, and record the checked SDK version in a code comment next to the `PackageReference` so it's clear why that version was chosen.
- Create `Ashfall.Core.Tests/Architecture/DependencyGraphExtractor.cs`:
  - Uses Roslyn `Microsoft.CodeAnalysis.CSharp` to parse syntax trees.
  - For each class with a constructor accepting other system types: emit an edge `(ConsumingType) → (DependencyType)`.
  - **Correction — the event-bus scan step as originally written was based on a false premise and has been rewritten:** the codebase's `IEventBus` (`Assets/Ashfall.Core/Events/IEventBus.cs`) is entirely string-keyed — `Publish(string eventName, object payload = null)`, `Subscribe(string eventName, Action<object> handler)`, `Unsubscribe(string eventName, Action<object> handler)`. There is no generic `Subscribe<T>`/`Publish<T>` overload on this interface, and the type-safe generic static `EventBus` class described in AGENTS.md's "Event System" table (described there as "Unity side, the real decoupler") does not exist anywhere in the current repository — it was either removed during the Unity→Godot migration or never present in this snapshot. Since edges from string-keyed events can't be resolved to a concrete producer/consumer type pair without also indexing every string literal used as an event name (a much larger, separate piece of work with a high false-edge rate), **event-based coupling is out of scope for this extractor.** Scan constructor parameters only. If event-bus coupling visibility becomes a real need later, track it as a separate, explicitly-scoped follow-up batch — do not fold it into this one under a false premise.
  - Output: `DependencyGraph` object with `Nodes` (types) and `Edges` (directed dependencies).
- Add helper: `DependencyGraph.DetectCycles()` — Tarjan's SCC algorithm, returns list of cycles.
- Add helper: `DependencyGraph.GetLayerViolations(LayerManifest)` — returns edges that point in the forbidden direction.

### Verification
- Unit test with a known small graph (3 nodes, 1 cycle) passes `DetectCycles()`.
- Unit test with a layering violation (Domain→Orchestration edge) is caught.
- Full extraction runs against `Assets/Ashfall.Core/` without crashing (even if violations exist).

### Done when
- [ ] `Microsoft.CodeAnalysis.CSharp` added to `Ashfall.Core.Tests.csproj` with a pinned exact version; `dotnet restore` succeeds
- [ ] `DependencyGraphExtractor.cs` exists and compiles, scanning constructor parameters only (no event-bus string-edge scanning, per correction above)
- [ ] Tarjan cycle detection works on synthetic graphs
- [ ] Full graph extraction produces a non-empty graph from actual source

---

## Step 3 — Define Allowed Dependency Rules

### Goal
Codify the architectural rules into a testable format so the extractor can report violations with clear explanations.

### Implementation
- Create `Ashfall.Core.Tests/Architecture/ArchitectureRules.cs` with rule definitions:
  - **Rule 1 — No upward layer references:** A type in layer N may only depend on types in layer N or below.
  - **Rule 2 — No circular dependencies:** The dependency graph among system types must be a DAG (directed acyclic graph).
  - **Rule 3 — Cross-domain via interface:** If Domain/A depends on Domain/B, the dependency must be through an interface declared in Foundation, OR the pair must be listed in `CrossDomainManifest`.
  - **Rule 4 — Host isolation:** Nothing in Core may reference `Godot.*` or `UnityEngine.*`. **Correction — the original "already enforced by `.asmdef`" caveat is stale/inaccurate:** no `.asmdef` file exists anywhere under `Assets/Ashfall.Core/` today (confirmed by direct directory search — zero `*.asmdef` files found). `Ashfall.Core.csproj` even contains a leftover exclusion for one (`Exclude="../Assets/Ashfall.Core/AtomicWar.Ashfall.Core.asmdef"`), but that path doesn't resolve to a real file — it's dead configuration from before the Unity legacy tree was removed. The only current enforcement of engine-isolation is `Ashfall.Core.Tests/CoreInvariantSourceTests.cs`'s `Core_HasZeroEngineCoupling` source-scan test (bans `UnityEngine.`, `Godot.`, `GodotSharp`, `JsonUtility`, etc. as raw text patterns). This makes Rule 4 **more valuable, not redundant** — it's currently the second and only other line of defense besides a text-pattern scanner, not a "double-check" of an asmdef that doesn't exist. Also note: `Ashfall.Core.csproj` targets `net8.0` (confirmed by direct read), not `netstandard2.1` as AGENTS.md's Stack table claims — flag this drift if this batch's docs cite AGENTS.md's target-framework table verbatim.
  - **Rule 5 — Orchestration fan-in limit:** No orchestration type may depend on more than 12 domain systems directly (prevents god-object growth — grandfather any exceptions found in Step 4's baseline with a documented exception count; there is no `GameBootstrap` class to grandfather by name, see Motivation corrections above — if a real orchestration type in Core exceeds the limit, name it explicitly once Step 4's report identifies it). **Precision gap not previously flagged: the "12" threshold has no stated justification anywhere in this document** — it reads as an arbitrary round number, not a value derived from the actual codebase (e.g. "N+1 above the current maximum observed fan-in" or "matches a known team convention documented elsewhere"). Before implementing this rule, either (a) derive the number from Step 4's baseline report (e.g. set it to the current max observed fan-in, so the rule starts as a no-op ratchet that only prevents growth beyond today's worst offender), or (b) cite a concrete rationale in a code comment next to the rule. Do not ship a bare magic number with no derivation — it will be unreviewable and arbitrary to whoever reads it later. **Data point for calibration:** `ExpansionMasterSession` — the only confirmed Orchestration-layer type in Core — takes 6 constructor parameters (`HoldfastSession`, `DutyRosterSystem`, `DutyRosterCatalog`, `LocationLayoutSystem`, `CrossingSession`, `SimClock`, plus optional `ILog`), well under 12. If Step 4's baseline finds no orchestration type anywhere close to 12, that's a signal the limit is currently unfalsifiable and won't catch anything for a long time — consider a tighter starting number (e.g. 8) if the goal is to actually constrain near-term growth rather than only distant-future growth.
- Each rule has: `Name`, `Description`, `Evaluate(DependencyGraph) → List<Violation>`.

### Verification
- Synthetic test graphs with deliberate violations trigger each rule.
- Rules produce human-readable violation messages (example format only, not a claim about real code: `"<TypeA> depends on <TypeB> — cross-domain dependency not in manifest"`).

### Done when
- [ ] 5 rules defined and testable
- [ ] Each rule has at least 2 unit tests (one pass, one fail)
- [ ] Violation messages are clear and actionable

---

## Step 4 — Detect Existing Cycles and Violations

### Goal
Run the full extractor + rules against the actual codebase and produce a baseline report of all current violations.

### Implementation
- Create `Ashfall.Core.Tests/Architecture/ArchitectureBaselineTests.cs`:
  - `[Fact] ReportCurrentCycles()` — runs extractor, prints all SCCs with >1 node. Does NOT fail (baseline capture).
  - `[Fact] ReportLayerViolations()` — prints all upward-reference violations. Does NOT fail yet.
  - `[Fact] ReportCrossDomainViolations()` — prints unsanctioned cross-domain deps.
- Output a `ARCHITECTURE_BASELINE.md` report at repo root with:
  - Total nodes (system count), total edges (dependency count)
  - List of cycles (if any)
  - List of layering violations
  - List of unsanctioned cross-domain deps
  - Fan-in counts for orchestration types
- This is diagnostic only — tests print but pass. The goal is visibility.

### Verification
- Tests run without failure.
- `ARCHITECTURE_BASELINE.md` is generated and contains meaningful data.
- Review output manually against a fresh, independent read of a handful of actual constructors. **Correction — the originally-suggested example classes were wrong:** `MedicalSystem` and `DynamicEconomySystem` do not exist anywhere in `Assets/Ashfall.Core/` (confirmed by full-repo class search — zero hits, including under the legacy `Assets/_Game/` tree, where AGENTS.md's H5 offender list originally placed similarly-named classes that also no longer resolve). Use confirmed real Core classes instead: `CraftingSystem` (`Assets/Ashfall.Core/Crafting/CraftingSystem.cs:13`), `ExpeditionSystem` (`Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:79`), `SilentFoundrySystem` (`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs:270`), and `ExpansionMasterSession` (`Assets/Ashfall.Core/ExpansionMasterSession.cs:23`, constructor at lines 46-53) — pick 3–5 of these (or others confirmed by a fresh source read at implementation time) to sanity-check the extractor isn't silently dropping edges or misparsing generics/nullables.

### Done when
- [ ] Baseline tests exist and run green
- [ ] `ARCHITECTURE_BASELINE.md` generated with real data
- [ ] At least one manually-verified real dependency edge (confirmed by directly reading the constructor source, not assumed) appears correctly in the report — do not rely on the DutyRoster/Phase0HostSession examples from this doc's earlier draft; those were checked and found inaccurate, so they must not be used as the validation baseline

---

## Step 5 — Fix Detected Cycles

### Goal
Break any circular dependencies found in Step 4 by extracting shared types to Foundation or introducing interfaces at cycle boundaries.

### Implementation
- For each cycle detected:
  - Identify the "weakest" edge (the dependency that is most incidental / easiest to invert).
  - Apply one of these patterns:
    - **Extract interface to Foundation:** If A depends on B and B depends on A, extract `IB` to Foundation, have A depend on `IB`, and B implement `IB`.
    - **Extract shared DTO:** If the cycle is caused by both systems using the same data type, move that type to Foundation.
    - **Introduce mediator event:** If the coupling is "A notifies B", replace with event publication through `IEventBus` (no direct reference).
- Update `CrossDomainManifest.cs` for any newly sanctioned cross-domain dependencies.
- Do NOT refactor systems beyond what's needed to break cycles — minimize blast radius.

### Verification
- Re-run `DependencyGraphExtractor` — cycle count drops to 0 (or to a documented, grandfathered count with tech-debt ticket).
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all tests pass. **Correction:** the "1941+ tests" figure in earlier drafts is stale — a static count of `[Fact]`/`[Theory]` attributes in the current tree returns 2116 `[Fact]` + 4 `[Theory]` (≥2120 declared test methods before Theory data-row expansion). Do not hardcode a specific test count in this Done-when criterion; use "all tests pass" and let `dotnet test`'s own summary line be the source of truth, since the count will keep growing as the codebase grows.
- No behavioral change — this is purely structural.

### Risk / Rollback
Step 5 is the one step in this batch with real risk: it changes production code in `Assets/Ashfall.Core/` to break cycles (extracting interfaces, moving DTOs to Foundation, or introducing event-based mediation). Each cycle fix must land as its own commit, scoped to exactly the types involved in that one cycle, with `dotnet test` run and green before moving to the next cycle. If a cycle fix causes unexpected test failures or save-format changes (e.g. moving a DTO changes its serialized shape and breaks `SaveChecksum`), revert that single commit via `git revert <sha>` rather than attempting a forward fix under time pressure — the cycle can stay documented as a grandfathered exception in `CrossDomainManifest.cs` until it gets its own dedicated task.

### Done when
- [ ] Zero new cycles (or all remaining cycles have documented exception in manifest)
- [ ] Extracted interfaces/types are in Foundation layer
- [ ] All existing tests still pass (per `dotnet test`'s own reported count, not a hardcoded number)
- [ ] No runtime behavior change
- [ ] No change to any save-format golden/checksum test if one exists (cross-check against Batch 107 if it has landed by this point) — moving a DTO to a new namespace can change its serialized type discriminator under some serializers; verify save round-trip tests still pass, not just build+compile

---

## Step 6 — Add CI Enforcement

### Goal
Ensure no future commit can introduce a new cycle or layering violation without the CI pipeline catching it.

**Ordering correction:** this step does not strictly require Step 5 to be complete. If Step 4's baseline finds zero cycles/violations, the hard-failing tests in this step can be turned on immediately after Step 4, with Step 5 skipped entirely. If Step 4 finds violations that Step 5 doesn't fully resolve, this step's tests must assert against the *documented grandfathered count*, not zero — do not write `Count == 0` if Step 5 left known exceptions on the table; that would make CI red from day one. Confirm the actual post-Step-5 state before writing the assertions.

### Implementation
- Convert baseline tests from Step 4 into hard-failing `[Fact]` tests:
  - `[Fact] NoCyclesInDependencyGraph()` — asserts `graph.DetectCycles().Count == 0` (or ≤ grandfathered count — check which applies per the ordering correction above).
  - `[Fact] NoLayeringViolations()` — asserts zero upward references.
  - `[Fact] NoCrossDomainWithoutManifest()` — asserts all cross-domain deps are sanctioned.
  - `[Fact] OrchestrationFanInWithinLimit()` — asserts no orchestration type exceeds 12 direct domain deps (with exception list).
- These tests run as part of `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — already in CI.
- Add a comment in each test pointing to this roadmap for context.
- Update `ARCHITECTURE_BASELINE.md` to reflect enforced state (violations = 0, or the documented grandfathered count).

### Verification
- Deliberately introduce a cycle in a local branch (not merged) → test fails with clear message, then revert the deliberate change before continuing.
- Deliberately add an unsanctioned cross-domain dep (local, unmerged) → test fails, then revert.
- Full verification checklist (all 5 canonical steps: `dotnet build` ×2, `dotnet test`, `godot --headless -- --data-integrity-selftest`, `godot --headless -- --bridge-selftest`) passes.

### Done when
- [ ] Architecture tests are hard-failing (not just reporting)
- [ ] A deliberate, local, unmerged violation triggers a failure whose message names the specific offending type pair (not just "violation found")
- [ ] CI runs these tests as part of the existing `dotnet test` invocation — no new CI job needed
- [ ] `ARCHITECTURE_BASELINE.md` updated to reflect enforced state, matching whatever Step 5 actually achieved (zero or a named grandfathered set)

---

## Step 7 — Write Layering Documentation & Onboarding Tests

### Goal
Make the architectural rules discoverable for new contributors and provide "pit of success" tests that guide developers toward correct dependency patterns.

### Implementation
- Create `Assets/Ashfall.Core/Architecture/README.md` documenting:
  - The four layers and their responsibilities
  - Dependency direction rules
  - How to add a new system (which layer, what constructor params are allowed)
  - How to add a sanctioned cross-domain dependency
  - How to run the architecture tests locally
- Create `Ashfall.Core.Tests/Architecture/ArchitectureOnboardingTests.cs`:
  - `[Fact] AllSystemsHaveExactlyOneLayer()` — no type is unclassified.
  - `[Fact] FoundationTypesHaveNoDomainDependencies()` — Foundation stays pure.
  - `[Fact] NewSystemsFollowNamingConvention()` — systems end in `System`, sessions end in `Session`. **Gap not previously flagged: this convention is already violated by at least 6 real classes today** — `FactionStanceEngine`, `TradeTellEngine`, `ProceduralEulogyEngine`, `GenerationalSuccessionEngine`, `PhantomMemoryEngine`, `FactionRadioEngine` (all confirmed via source search, all in `Assets/Ashfall.Core/`, all ending in `Engine`, none matching `*System`/`*Session`). Writing this test as a blanket "every domain class must end in System or Session" assertion will fail on day one against real code, not just hypothetical future violations. Either (a) scope the test to only classes matching an explicit allow-list of suffix patterns (`System`, `Session`, and `Engine` as a third sanctioned suffix, documented in `Architecture/README.md`), or (b) grandfather the 6 existing `*Engine` classes by name in the test itself the same way Step 5/6 grandfather cycle exceptions — do not write a rule that's false on the day it ships.
  - `[Fact] DependencyGraphIsDocumented()` — exports a Mermaid diagram to `ARCHITECTURE_GRAPH.md` for visual inspection.
- Generate `ARCHITECTURE_GRAPH.md` with Mermaid `graph TD` syntax showing all system dependencies (auto-generated, gitignored or committed as snapshot).

### Verification
- New contributor can read `Architecture/README.md` and understand where to place a new system.
- Onboarding tests pass.
- Mermaid diagram syntax is valid — paste into a Mermaid live editor or GitHub markdown preview and confirm it renders without syntax errors (there is no automated Mermaid linter in this repo's toolchain today; this is a manual check unless one is added).
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj && dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes.

### Done when
- [ ] `Architecture/README.md` exists with clear guidance
- [ ] Onboarding tests pass and cover all current systems
- [ ] Mermaid dependency diagram generated and manually confirmed to render (see verification note above — this is not automatically checked)
- [ ] A test exists that would fail if a hypothetical new system were added to the wrong layer (write and run this as a temporary local scratch test during development to prove it catches the case, then remove the scratch test — do not commit a permanently-hypothetical test)

---

## Summary

| Step | Deliverable | Layer | Risk | Depends on |
|------|-------------|-------|------|------------|
| 1 | `LayerManifest.cs` + `CrossDomainManifest.cs` | Core | None | — |
| 2 | `DependencyGraphExtractor.cs` (Roslyn-based, new NuGet dep) | Tests | None | Step 1 |
| 3 | `ArchitectureRules.cs` (5 rules) | Tests | None | Step 2 |
| 4 | `ARCHITECTURE_BASELINE.md` (diagnostic report) | Tests/Docs | None | Steps 1–3 |
| 5 | Cycle-breaking refactors (interfaces, DTOs) — **conditional, only if Step 4 finds cycles** | Core | Medium | Step 4 |
| 6 | Hard-failing CI tests | Tests/CI | None | Step 4 (Step 5 only if cycles were found) |
| 7 | Documentation + onboarding tests + Mermaid graph | Tests/Docs | None | Step 6 |

**Exit criteria:** `dotnet test` enforces zero cycles (or a documented, named, grandfathered set), zero layering violations, and zero unsanctioned cross-domain dependencies. New violations fail CI before merge.

---

## Review Notes (Corrected)

This document was adversarially reviewed against the actual ASHFALL codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following factual errors were found and corrected in place above:

1. **"DutyRoster depends on Survivors + Expeditions + Expansions"** — false. `DutyRosterSystem` (`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs:216-223`) has only `DutyRosterSystem()` and `DutyRosterSystem(int seedSalt)` constructors — zero system dependencies. This was the headline motivating example in the original doc and was fabricated or based on indirect/host-wiring coupling that isn't constructor coupling.
2. **"Phase0HostSession depends on 5 systems"** — false. `Phase0HostSession` (`src/Host/Phase0HostSession.cs:170-172`) takes `(int seed = DefaultSeed, ChemicalDependencySystem dependency = null)` — 1 system dependency, optional. It's also in `src/Host/`, not `Assets/Ashfall.Core/`, so out of this batch's stated scope entirely.
3. **`GameBootstrap.Phase0Expansion.cs` does not exist** — no `class GameBootstrap` exists anywhere in the repository (confirmed via symbol search). It is referenced only in prose docs (AGENTS.md, `.clinerules`, `REPO_REVIEW_REPORT.md`), not in actual source. The real Godot host entry point with per-domain Setup/Save/Flush structure is `src/Main.cs`. All diagram/rule references to `GameBootstrap`/`*Bootstrap.cs` inside the Core layer diagram were removed.
4. **"82+ systems" was an unverified figure** — a fresh regex count of classes named `*System`/`*Session` in `Assets/Ashfall.Core/` returns ~86, in the right ballpark but not sourced from any committed methodology found in the repo. Updated to "~86 classes... verified by regex count" with an instruction to re-verify at implementation time since the count will drift.
5. **Step 2's "scan for `IEventBus.Subscribe<T>`/`Publish<T>`" was based on a nonexistent API shape.** The real `IEventBus` (`Assets/Ashfall.Core/Events/IEventBus.cs`) is entirely string-keyed: `Subscribe(string, Action<object>)`, `Publish(string, object)`. There is no generic overload. The type-safe static generic `EventBus` class described in AGENTS.md's Event System table does not exist in the current repo at all. Event-based coupling scanning was removed from Step 2's scope as a result — it would have scanned for a call pattern that cannot exist, silently producing zero results and giving false confidence.
6. **`Ashfall.Core.Tests.csproj` has no Roslyn/`Microsoft.CodeAnalysis.CSharp` package today** (confirmed by reading the actual `<ItemGroup>` — only `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio` exist). Step 2 now explicitly calls out this is a **new** dependency addition requiring a pinned exact version, not an assumed-already-present capability.
7. **Stale test count ("1941+ tests")** carried over from another doc — a static count of `[Fact]`/`[Theory]` attributes in the current tree returns 2116+4 (≥2120 declared test methods, before Theory data-row expansion), meaningfully higher than 1941. Hardcoded test counts were replaced with "all tests pass" per `dotnet test`'s own summary, since any hardcoded number will go stale again.
8. **Missing risk/rollback section** — the original doc rated the entire batch "Risk: Low" uniformly, but Step 5 performs real production refactors in `Assets/Ashfall.Core/` (extracting interfaces, moving DTOs) which is not zero-risk to a save-format-sensitive codebase (see AGENTS.md Invariant 3, `SaveChecksum`, `SaveWireContract`). Added a differentiated risk rating (Low for Steps 1–4/6–7, Medium for Step 5) and an explicit per-cycle-commit rollback strategy.
9. **Illogical dependency ordering in Step 6** — the original doc made Step 6 (CI enforcement) unconditionally depend on Step 5 (cycle fixing), which is backwards if Step 4 finds zero cycles: CI enforcement of "zero cycles" can and should ship immediately after Step 4 in that case, without waiting on a Step 5 that may not even be needed. Corrected the dependency to be conditional on Step 4's findings.
10. **Unverified illustrative examples presented as fact** — the circular-dependency example ("Survivors/ → Economy/ → Inventory/ → Survivors/") and the rule-violation example ("MedicalSystem depends on CombatTraumaSystem") were written as if they were confirmed findings. Neither was verified (no extractor exists yet to confirm either). Both are now explicitly labeled as hypothetical/illustrative, not findings.
11. **Step 4's Done-when criterion asked reviewers to confirm output against the DutyRoster/Phase0HostSession examples**, which is circular since those examples are the ones proven wrong in this review. Replaced with an instruction to manually verify against freshly, independently-read real constructors instead.


## Second-Pass Adversarial Review — Additional Findings (New Corrections)

The "Review Notes (Corrected)" section above was itself independently re-verified against the
codebase for this second pass (not trusted at face value), and all 11 of its claims were confirmed
accurate via fresh, independent source reads and grep counts. The following **additional** errors
and gaps — not caught by the first correction pass — were found and fixed in place above:

12. **`MedicalSystem` and `DynamicEconomySystem` (Step 4's verification examples) do not exist
    anywhere in the repository** — confirmed by full-repo class search, including under the legacy
    `Assets/_Game/` tree where AGENTS.md's own H5 "god object" offender list places similarly-named
    classes. Even the legacy versions no longer resolve to a real file. Step 4's instruction to
    manually verify the extractor's output against these two specific classes was unusable as
    written. Replaced with four classes confirmed to exist in `Assets/Ashfall.Core/` today:
    `CraftingSystem`, `ExpeditionSystem`, `SilentFoundrySystem`, `ExpansionMasterSession`.
13. **The "12" orchestration fan-in limit (Rule 5) has no stated justification** and is not derived
    from any baseline data — it reads as an arbitrary round number. Cross-checked against the one
    confirmed real Orchestration-layer type, `ExpansionMasterSession`, which has a fan-in of only 6
    (constructor takes `HoldfastSession`, `DutyRosterSystem`, `DutyRosterCatalog`,
    `LocationLayoutSystem`, `CrossingSession`, `SimClock`, plus optional `ILog`) — well under 12,
    meaning the rule as stated would not catch anything for a long time. Added a note to either
    derive the number from Step 4's actual baseline or document a concrete rationale, and flagged
    that a tighter number (e.g. 8) may better serve the rule's stated purpose.
14. **Step 7's `NewSystemsFollowNamingConvention()` test ("systems end in `System`, sessions end in
    `Session`") is already false against real code.** Confirmed via source search: at least 6 classes
    in `Assets/Ashfall.Core/` end in neither suffix — `FactionStanceEngine`, `TradeTellEngine`,
    `ProceduralEulogyEngine`, `GenerationalSuccessionEngine`, `PhantomMemoryEngine`,
    `FactionRadioEngine` (all end in `Engine`). Writing this as a blanket assertion would fail on
    day one, not just for hypothetical future violations — the opposite of what Step 7's own
    Done-when criteria call for ("Onboarding tests pass and cover all current systems"). Added a
    note to either add `Engine` as a third sanctioned suffix or explicitly grandfather these 6
    classes by name.
15. **Rule 4 ("Host isolation... already enforced by `.asmdef`") cites enforcement infrastructure
    that no longer exists.** Confirmed by direct directory search: zero `.asmdef` files exist
    anywhere under `Assets/Ashfall.Core/` today. `Ashfall.Core.csproj` itself contains a stale
    `Exclude="../Assets/Ashfall.Core/AtomicWar.Ashfall.Core.asmdef"` path that doesn't resolve to a
    real file — dead configuration left over from before the Unity legacy tree was removed. The
    only current enforcement of Core's engine-isolation invariant is
    `Ashfall.Core.Tests/CoreInvariantSourceTests.cs`'s text-pattern source scan. This makes Rule 4
    *more* valuable than the original phrasing implied (it's the second, not a redundant, check),
    and the "double-check via graph" framing undersold that. Also surfaced in the same pass:
    `Ashfall.Core.csproj` targets `net8.0` (confirmed by direct read of the `.csproj`), not
    `netstandard2.1` as AGENTS.md's own Stack table claims — a discrepancy in AGENTS.md itself,
    noted here since this batch's docs could otherwise silently inherit the wrong target framework
    if copied from that table.
16. **The `Microsoft.CodeAnalysis.CSharp` version example (`4.11.0`) was presented as if already
    checked against the build environment, but was not.** The actual installed SDK in this
    environment is **10.0.302** (`dotnet --version`), with no `global.json` pinning an older
    version — meaning `4.11.0` was a generic example, not a value derived from this repo's actual
    toolchain. Corrected the implementation note to require an actual `dotnet --version` check
    against whichever machine builds the project (local + CI, which may differ) before pinning any
    specific version, and to record which SDK version was checked in a code comment next to the
    `PackageReference` for future auditability.
17. **`InventoryGearBridgeTests` (cited in Step 1 as evidence for the sanctioned `Radiation`↔`Inventory.WornGear`
    bridge) is not a standalone test file** — it is a nested class defined inside
    `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs:364`. The bridge itself and its test coverage
    are both confirmed real; only the implied file/class location was imprecise. Citation corrected.
18. **The 87-class count (previously "~86") was tightened to an exact, triple-verified number** —
    three independent counting methods (raw grep, unique-name sort, declaration count) all agree at
    87 classes matching `*System`/`*Session` in `Assets/Ashfall.Core/` as of this review. The
    original "~86" approximation was close but not exact; replaced with the precise figure while
    retaining the instruction to re-count at implementation time since Core will keep growing.

None of these new findings overturn the original correction pass's conclusions (DutyRosterSystem,
Phase0HostSession, GameBootstrap, IEventBus, package references, and test counts all check out
exactly as previously stated) — they are additional precision/scoping issues the first pass did not
catch, surfaced by independently re-deriving every checkable claim rather than trusting the existing
"Review Notes" section at face value.
