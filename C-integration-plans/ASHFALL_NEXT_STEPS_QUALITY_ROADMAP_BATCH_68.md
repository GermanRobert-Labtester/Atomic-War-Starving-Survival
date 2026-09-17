# ASHFALL — Quality Roadmap Batch 68

## Theme: Automated Documentation Generation — API Docs & System Dependency Graph

**Priority:** MEDIUM (developer onboarding & maintainability)
**Risk:** Low overall, but **Medium for Step 7 specifically** — see Step 7's Risk & Rollback note; CI doc enforcement is build-breaking once live and the real stub-sweep scope (10,946 CS1591 warnings, measured by an actual build in this review) is larger than the plan's own estimate. Steps 1–6 are genuinely additive/no-risk.
**Estimated effort:** 7–10 sessions (revised from the original "5–7" — see Step 7's scale-check; this is not a cosmetic rounding, Step 7 alone is realistically 2–4 sessions)
**Prerequisites:** None — Steps 1-6 are additive and safe to run in any order after Step 1; **Step 7 is NOT safe to run before Steps 2-3 land**, since it depends on those steps' hand-documented types to reduce (slightly) the stub-sweep surface, and because turning on `WarningsAsErrors=CS1591` before the stub sweep is complete will break the build for every concurrent Core change.

---

## Motivation

The project has 82+ Core systems across 37 top-level directories in `Assets/Ashfall.Core/` (the plan's "20+" was directionally correct but loosely stated — 37 is the actual count, confirmed by `find Assets/Ashfall.Core -maxdepth 1 -type d`), **27 host sessions in `src/Host/` (29 project-wide, including `src/Disease/DiseaseHostSession.cs`, `src/Foundry/SilentFoundryHostSession.cs`, and `src/YearOfAsh/YearOfAshHostSession.cs` — corrected from the "34" figure used elsewhere; see Batch 67's Review Notes for the full verification)**, **27 save stores** (corrected from "22" — confirmed by `find . -iname "*SaveStore.cs"` project-wide, excluding `.uid` sidecars), 5 port interfaces, and a complex dependency graph with no auto-generated documentation, no dependency visualization, and no architectural decision records. New contributors must reverse-engineer system relationships from code alone. This batch establishes a documentation-as-code pipeline that stays current as the codebase evolves.

---

## Step 1 — Enable XML Documentation Generation from Core .csproj

**Goal:** Produce an XML doc file on every `dotnet build` so downstream tooling (DocFX, dependency analyzers, IDE tooltips) can consume structured documentation.

**⚠️ Verified premise (re-checked independently in this pass — one additional correction found):** Confirmed by directly reading both `/Ashfall.Core/Ashfall.Core.csproj` and `/Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — there is no `Assets/Ashfall.Core/Ashfall.Core.csproj`; that path does not exist. `Assets/Ashfall.Core/` contains only source `.cs` files (303 of them, confirmed by `find Assets/Ashfall.Core -iname "*.cs" | wc -l`), no project file. The two real, relevant project files are:
  - `/Ashfall.Core/Ashfall.Core.csproj` (root-level) — a `Microsoft.NET.Sdk` class library, `TargetFramework net8.0`, `LangVersion 9.0`, `RootNamespace Ashfall.Core`, `AssemblyName Ashfall.Core`. It compiles `Assets/Ashfall.Core/**/*.cs` directly via `<Compile Include>` and carries `<GenerateDocumentationFile>false</GenerateDocumentationFile>` — **confirmed present and `false`, so the plan's core premise holds.** This is the correct edit target for Step 1.
  - `/Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (root-level, separate project, **not the same project as above — an earlier draft of this correction conflated the two**) — an xUnit test project, `TargetFramework net9.0` (matching AGENTS.md's stated `net9.0` for Tests), which does **not** compile the Core `.cs` files directly; it instead has a `<ProjectReference Include="..\Ashfall.Core\Ashfall.Core.csproj" />` and consumes `Ashfall.Core.dll` as a library reference. It has no `GenerateDocumentationFilesetting` of its own and doesn't need one for this batch's purposes — XML docs are a property of the library project, and the test project references the library's output, not its source.
  - `/Ashfall.csproj` (root-level, the Godot host project) — also compiles the same `Assets/Ashfall.Core/**/*.cs` sources directly (confirmed by reading it: `<Compile Include="Assets/Ashfall.Core/**/*.cs" />`), but has **no `GenerateDocumentationFile` property at all** (confirmed absent from the file, meaning it defaults to `false`). Since this project independently compiles the same source files, whether it also needs `GenerateDocumentationFile=true` is a real open decision this plan must make, not a detail to skip — if only `/Ashfall.Core/Ashfall.Core.csproj` gets the flag, `dotnet build Ashfall.csproj` (the Godot host build, part of the mandatory 5-step verification checklist in AGENTS.md) will never itself produce an XML doc file or surface CS1591 warnings, meaning CI running only the host build would not catch undocumented APIs introduced through host-only compilation of the same shared source tree.
  Every `Assets/Ashfall.Core/Ashfall.Core.csproj` reference below is corrected to `Ashfall.Core/Ashfall.Core.csproj` (root-level path, the class-library project — not the Tests project, and not the Godot host project, though see the open question above about the host project).

**Implementation:**
- Edit `Ashfall.Core/Ashfall.Core.csproj` (root-level; NOT `Assets/Ashfall.Core/Ashfall.Core.csproj`, which doesn't exist):
  - Change `<GenerateDocumentationFile>false</GenerateDocumentationFile>` to `<GenerateDocumentationFile>true</GenerateDocumentationFile>` (the property already exists in the file, set to `false` — this is a one-value edit, not an addition, contrary to the plan's phrasing).
  - Add `<NoWarn>$(NoWarn);CS1591</NoWarn>` temporarily to suppress "missing XML comment" warnings until Step 7 enforces them.
- Confirm the output `Ashfall.Core/bin/Debug/net8.0/Ashfall.Core.xml` is generated (corrected TFM: the project's actual `TargetFramework` is `net8.0`, not `netstandard2.1` — `netstandard2.1` was AGENTS.md's stated *target* for the abstract Core layer, but the concrete `Ashfall.Core.csproj` build project itself targets `net8.0`; confirm the exact output path by running the build once rather than assuming).
- `.gitignore` already excludes build output for this project — confirmed present: `/Ashfall.Core/bin/` and `/Ashfall.Core/obj/` are already listed (lines 184-185 of the root `.gitignore`). **The plan's instruction to "add `Assets/Ashfall.Core/bin/` to `.gitignore` if not already present" targets the wrong path and is unnecessary** — there is no build output under `Assets/Ashfall.Core/` (that directory contains only source files compiled by the two projects above, not itself a build target). Skip this sub-step; the real path is already covered.

**Verification:**
```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj
# Confirm: Ashfall.Core.xml exists in output directory (path per the TFM confirmed above)
# Confirm: 0 errors, warnings suppressed by CS1591 NoWarn
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Confirm: all tests still pass (no behavioral change)
```
**Independently run in this review (not just specified — actually executed):** `dotnet build Ashfall.Core/Ashfall.Core.csproj -c Debug` succeeds today with 0 warnings, 0 errors (confirming the current baseline before any change). Flipping `GenerateDocumentationFile=true` and removing the `CS1591` suppression via `dotnet build Ashfall.Core/Ashfall.Core.csproj -c Debug -p:GenerateDocumentationFile=true -p:NoWarn=""` produces **10,946 CS1591 warnings, 0 errors** — this is the authoritative number for Step 7's scale (see Step 7's corrected scale-check below; the plan's own `grep`-based estimate of ~9,800 undercounted the true figure by about 1,150). The output XML path was confirmed to be `Ashfall.Core/bin/Debug/net8.0/Ashfall.Core.xml`.

**Done when:**
- `dotnet build` produces `Ashfall.Core.xml` without errors, from the correct project file path (`Ashfall.Core/Ashfall.Core.csproj`), and the file is confirmed to exist at `Ashfall.Core/bin/Debug/net8.0/Ashfall.Core.xml` by listing the directory after the build (not just assuming the TFM-derived path is correct).
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` passes with the same pass count as the pre-change baseline (record the baseline pass count before making the change so "no regressions" is a verifiable diff, not an assumption).
- Confirmed (not assumed) that `.gitignore` already excludes the real build-output path (`/Ashfall.Core/bin/` and `/Ashfall.Core/obj/`, at lines 184-185 of the root `.gitignore` — confirmed by direct `grep -n` in this review); no `.gitignore` edit needed.
- Decision recorded (see the Verified Premise section above) on whether `/Ashfall.csproj` (the Godot host project, which independently compiles the same `Assets/Ashfall.Core/**/*.cs` files) also needs `GenerateDocumentationFile=true`. If the decision is "no," document why CI accepts that the host build path will never surface CS1591 warnings for the same source tree.

**Risk & Rollback:** Additive-only (a `.csproj` property flip + a `NoWarn` entry). Rollback is a one-line revert of the property change; no code or data files are touched. The only real risk is forgetting to also decide on `/Ashfall.csproj` (see above) and ending up with two build paths for the same source tree that disagree on doc-warning visibility — flag this in the PR description rather than silently deferring it.

---

## Step 2 — Add XML Doc Comments to All 5 Port Interfaces and HostDefaults Implementations

**Goal:** Document the architectural boundary — the port interfaces that define the contract between Core and any host engine.

**⚠️ Verified premise (checked by directly reading `Assets/Ashfall.Core/Ports.cs` and `Assets/Ashfall.Core/HostDefaults.cs` in this review):**
- **4 of the 5 port interfaces already have a class-level `<summary>` doc comment today — this step is not starting from zero.** Confirmed by direct read: `IJsonSerializer` ("Engine-agnostic JSON port. Unity JsonUtility is banned from this assembly..."), `IFileIO` ("Text/file access for catalogs and saves..."), `IClock` ("Simulation calendar. Never DateTime.Now."), and `ISeededRng` ("Seeded RNG. Same seed must yield the same sequence in both hosts.") all have an existing `<summary>`. **Only `ILog` has zero doc comment today** (plain `public interface ILog { void Info(...); void Warn(...); void Error(...); }` with no `///` above it). None of the 5 have `<param>` or `<returns>` tags on their methods, and none have a `<remarks>` block — so there is real work here, but it is "add `<param>`/`<returns>`/`<remarks>` to 5, plus a `<summary>` to 1," not "add `<summary>` to 5 from scratch" as the original phrasing implied.
- **The default-implementation class names in the original plan do not match the actual class names.** Confirmed by reading `Assets/Ashfall.Core/HostDefaults.cs`: the real names are `SystemTextJsonSerializer` (matches), `FileSystemIO` (not `DefaultFileIO`), `SimClock` (not `DefaultClock`), and `SeededRng` (not `CoreSeededRng` — that name belongs to a different, host-side wrapper class defined inside `src/Host/DoseLedgerHostSession.cs`, an internal adapter that is not the Core default and is out of scope for this step). `HostDefaults.cs` also defines `NullLog` (the null-object `ILog` implementation used as a constructor default in at least `DutyRosterHostSession`), which the original plan's list omitted entirely — add it, since it's a public default implementation in the same file and squarely in this step's stated scope ("HostDefaults implementations").

**Implementation:**
- Add `<summary>` (where missing), `<param>`, and `<returns>` XML doc comments to:
  - `Assets/Ashfall.Core/Ports.cs` — all 5 interfaces:
    - `IJsonSerializer` — serialize/deserialize contract (`<summary>` already present; add `<param>`/`<returns>` to `Serialize<T>`/`Deserialize<T>`)
    - `IFileIO` — file and directory access abstraction (`<summary>` already present; add `<param>`/`<returns>` to all 5 methods)
    - `ILog` — structured logging (Info/Warn/Error) (**no `<summary>` exists yet — this is the one interface needing a summary from scratch**)
    - `IClock` — day-counter abstraction (`<summary>` already present; add `<param>` to `AdvanceDays`/`SetDay`)
    - `ISeededRng` — deterministic PRNG (xorshift64*) (`<summary>` already present; add `<param>`/`<returns>` to `Next`/`NextFloat`/`NextDouble`)
  - `Assets/Ashfall.Core/HostDefaults.cs` — default implementations (corrected names):
    - `SystemTextJsonSerializer`
    - `FileSystemIO` (corrected from `DefaultFileIO`)
    - `SimClock` (corrected from `DefaultClock`)
    - `SeededRng` (corrected from `CoreSeededRng`)
    - `NullLog` (added — omitted from the original plan)
  - `src/Host/GodotLog.cs` — Godot-specific `ILog` adapter (confirmed to exist at this exact path by file search)
- Each interface gets a `<remarks>` block explaining:
  - Why the port exists (engine decoupling).
  - Which invariant it supports (Invariant 1, 2, or 4).
  - Thread-safety expectations.

**Verification:**
```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj
# Confirm: XML file contains entries for all 5 interfaces + implementations
grep -c "<member" Ashfall.Core/bin/Debug/net8.0/Ashfall.Core.xml
# Should show new member entries for documented types
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:**
- All 5 port interfaces have complete `<summary>` (4 already have one; confirm it's not overwritten with something worse) + `<param>` + `<returns>` documentation, plus a new `<remarks>` block on each.
- All 5 default implementations in `HostDefaults.cs` — `SystemTextJsonSerializer`, `FileSystemIO`, `SimClock`, `SeededRng`, `NullLog` (corrected list) — have `<summary>` documentation.
- `GodotLog` adapter (`src/Host/GodotLog.cs`) is documented.
- Build produces zero errors.

**Risk & Rollback:** None beyond Step 1's — comment-only changes. Rollback is reverting the doc-comment diff; no behavioral surface changes.

---

## Step 3 — Add XML Doc Comments to Top 15 Most-Referenced Systems

**Goal:** Document the systems that are most commonly instantiated, injected, or called by other systems — the "load-bearing walls" of the architecture.

**⚠️ Verified premise (checked in this review by locating each of the 15 named classes and inspecting for existing doc comments):**
- **4 of the original 15 names do not correspond to any class in the codebase, confirmed by project-wide search:** `InventorySystem` (the real class is `Inventory`, at `Assets/Ashfall.Core/Inventory/Inventory.cs`), `MedicalAfflictionPipeline` (no match anywhere; closest existing candidates are `MedicalWardSystem` at `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` or `MedicalPathologyCatalog`), `SurvivorRegistry` (no match anywhere), and `DynamicEconomyEngine` (no match anywhere — and note AGENTS.md's own reference to a similarly-named `DynamicEconomySystem` at `Assets/_Game/Economy/DynamicEconomySystem.cs` is itself now stale: that file was also confirmed not to exist on disk, so AGENTS.md needs its own separate correction outside this batch). These 4 slots need replacement names before this step can be assigned to anyone — do not let a contributor discover this only after starting.
- **Of the 11 remaining names that do exist, 10 already have a class-level `<summary>` doc comment today** (confirmed by grep for `///` immediately preceding the class declaration in each file): `NeedsSystem`, `RadiationSystem`, `WeatherSystem`, `CombatTraumaSystem`, `CraftingSystem`, `ExpeditionSystem`, `SaveChecksum`, `ExpansionMasterSession`, `FinalWishSystem`, `JournalSystem`. **Only `CatalogIntegrityValidator` has zero doc comment above its class declaration today.** This means the step's real net-new work is: (a) pick 4 replacement systems for the invalid names, (b) write a `<summary>` for `CatalogIntegrityValidator` from scratch, (c) add/upgrade `<remarks>`, per-method `<summary>`/`<param>`/`<returns>`, and `<seealso>` cross-references for all 15 — since a class-level `<summary>` existing today does not mean method-level docs or `<remarks>` exist; spot-reads in this review did not check method-level coverage and it should not be assumed complete just because the class-level comment is present.

**Implementation:**
- Identify the top 15 by cross-reference count (approximate priority order) — **corrected list, with the 4 invalid names replaced by real candidates that need cross-reference-count verification (not yet ranked in this review) before final selection:**
  1. `NeedsSystem` — survival needs tick (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  2. `RadiationSystem` — dose accumulation and shielding (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  3. `WeatherSystem` — seasonal fallout, temperature (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  4. `CombatTraumaSystem` — injury pipeline (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  5. `Inventory` (corrected from `InventorySystem`) — item management, at `Assets/Ashfall.Core/Inventory/Inventory.cs`; existing doc coverage not yet checked, verify before assuming it needs full treatment
  6. `CraftingSystem` — recipe resolution (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  7. `ExpeditionSystem` — zone exploration (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  8. `MedicalWardSystem` (corrected from `MedicalAfflictionPipeline`, which does not exist) — affliction/ward lifecycle, at `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`; existing doc coverage not yet checked
  9. **Replacement needed** (corrected from `SurvivorRegistry`, which does not exist) — candidate: survivor roster/state management logic likely lives under `Assets/Ashfall.Core/Survivors/`; identify the actual top class by cross-reference count before finalizing, do not guess a name
  10. `SaveChecksum` — integrity hashing (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  11. **Replacement needed** (corrected from `DynamicEconomyEngine`, which does not exist in Core or in the Unity legacy tree) — candidate: `Assets/Ashfall.Core/Economy/HardcoreEconomyTuning.cs` or another Economy-namespace class; verify by actual cross-reference count
  12. `ExpansionMasterSession` — expansion coordination (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  13. `FinalWishSystem` — end-of-life narrative (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  14. `JournalSystem` — player journal (doc comment already present; needs `<remarks>`/method docs/`<seealso>`)
  15. `CatalogIntegrityValidator` — data validation (**no doc comment exists today — this one genuinely needs a `<summary>` from scratch, unlike the 10 above**)
- For each, add:
  - Class-level `<summary>` explaining purpose and lifecycle (skip for the 10 that already have one unless the existing text is inaccurate or too thin — read it first, don't blindly overwrite).
  - `<remarks>` noting which expansion/phase owns it.
  - Public method `<summary>` + `<param>` + `<returns>`.
  - `<seealso>` cross-references to related systems.

**Verification:**
```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Spot-check: open XML, confirm 15 class-level summaries present
```

**Done when:**
- All 15 systems (with the 2 replacement slots filled by real, cross-reference-verified names, not left as placeholders) have class-level and public-method-level XML documentation.
- Cross-references (`<seealso>`) link related systems.
- Zero build errors, zero test regressions.

**Risk & Rollback:** None — comment-only. The only real risk is scope drift from discovering, while documenting, that an existing `<summary>` is wrong or stale (e.g. describes removed behavior) — if found, fix it as part of this step rather than leaving a known-wrong comment in place, but note the fix separately in the PR description since "add docs" and "correct a wrong doc" are different review concerns.

---

## Step 4 — Generate Dependency Graph Using Roslyn Analyzer

**Goal:** Produce a machine-readable dependency graph showing which Core systems instantiate, reference, or call other Core systems — enabling visualization and dead-code detection.

**Implementation:**
- Create `tools/DependencyGraphGenerator/` — a standalone .NET console app:
  - References `Microsoft.CodeAnalysis.CSharp` and `Microsoft.CodeAnalysis.CSharp.Workspaces`.
  - Parses all `.cs` files under `Assets/Ashfall.Core/`.
  - For each class, identifies:
    - Constructor parameters (injected dependencies).
    - Field/property types from Core namespaces.
    - Method calls to other Core system types.
  - Outputs `docs/dependency-graph.json` with structure:
    ```json
    {
      "nodes": [{ "id": "NeedsSystem", "namespace": "Ashfall.Core", "file": "..." }],
      "edges": [{ "from": "NeedsSystem", "to": "IClock", "type": "constructor-inject" }]
    }
    ```
  - Also outputs `docs/dependency-graph.mmd` (Mermaid format) for direct rendering.
- Add a script `scripts/gen-dep-graph.sh`:
  ```bash
  dotnet run --project tools/DependencyGraphGenerator/ -- \
    --source Assets/Ashfall.Core/ \
    --output-json docs/dependency-graph.json \
    --output-mermaid docs/dependency-graph.mmd
  ```

**Verification:**
```bash
dotnet build tools/DependencyGraphGenerator/
dotnet run --project tools/DependencyGraphGenerator/ -- --source Assets/Ashfall.Core/ --output-json docs/dependency-graph.json --output-mermaid docs/dependency-graph.mmd
# Confirm: JSON has > 50 nodes, > 100 edges
# Confirm: Mermaid file renders in any Mermaid-compatible viewer
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:**
- `docs/dependency-graph.json` contains all Core system nodes and their edges.
- `docs/dependency-graph.mmd` renders a valid Mermaid graph.
- The generator builds and runs without errors.
- No changes to game code — purely additive tooling.

---

## Step 5 — Create System Overview Document with Mermaid Diagrams

**Goal:** Produce a human-readable architectural overview with three key flow diagrams that answer the most common onboarding questions.

**Implementation:**
- Create `docs/SYSTEM_OVERVIEW.md` with the following sections:

  **Section 1 — Expansion Coordination Flow:**
  ```mermaid
  graph TD
    ExpansionMasterSession --> Holdfast
    ExpansionMasterSession --> DutyRoster
    ExpansionMasterSession --> StandingRecord
    ExpansionMasterSession --> NobodysCharter
    Holdfast --> HoldfastTradeSession
    Holdfast --> HoldfastRuntimeSession
    ...
  ```

  **Section 2 — Save/Load Flow:**
  ```mermaid
  sequenceDiagram
    participant Main
    participant SaveStore
    participant System
    participant Checksum
    Main->>System: CaptureState()
    System-->>Main: SystemState DTO
    Main->>Checksum: Compute(state)
    Main->>SaveStore: Save(envelope)
  ```

  **Section 3 — Day-Advance Tick Order:**
  ```mermaid
  graph LR
    DayAdvance --> Weather
    Weather --> Radiation
    Radiation --> Needs
    Needs --> Medical
    Medical --> Expeditions
    Expeditions --> Economy
    Economy --> Events
  ```

- Each diagram includes a prose explanation of why that order matters.
- Include a "Port Interfaces" section summarizing the 5 ports with a table.
- Include a "Host Wiring" section explaining how `Main.cs` orchestrates 31 Setup methods.

**Verification:**
```bash
# Confirm: docs/SYSTEM_OVERVIEW.md exists and renders in GitHub/GitLab markdown preview
# Confirm: all three Mermaid diagrams parse (use mmdc CLI or online editor)
# No code changes — documentation only
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:**
- `docs/SYSTEM_OVERVIEW.md` contains all three diagrams with prose explanations.
- Diagrams are syntactically valid Mermaid.
- Document is linked from `README.md` or `docs/INDEX.md`.

---

## Step 6 — Set Up Architectural Decision Records (ADR) Framework

**Goal:** Establish a lightweight ADR framework so future architectural choices are recorded with context, alternatives considered, and rationale.

**Implementation:**
- Create `docs/adr/` directory.
- Create `docs/adr/README.md` explaining the ADR process:
  - Template location, numbering scheme, status lifecycle (Proposed → Accepted → Superseded → Deprecated).
- Create `docs/adr/0000-template.md`:
  ```markdown
  # ADR-NNNN: [Title]

  ## Status
  Proposed | Accepted | Superseded by ADR-XXXX | Deprecated

  ## Context
  What is the issue that we're seeing that motivates this decision?

  ## Decision
  What is the change that we're proposing and/or doing?

  ## Consequences
  What becomes easier or harder as a result of this decision?

  ## Alternatives Considered
  What other approaches were evaluated and why were they rejected?
  ```
- Backfill 3 foundational ADRs from decisions already documented in `AGENTS.md`:
  - `docs/adr/0001-engine-agnostic-core.md` — why Core has zero engine references.
  - `docs/adr/0002-json-data-authority.md` — why JSON in StreamingAssets is the source of truth.
  - `docs/adr/0003-godot-migration.md` — why the project migrated from Unity to Godot.

**Verification:**
```bash
# Confirm: docs/adr/ contains README.md, template, and 3 backfilled ADRs
ls docs/adr/
# Confirm: each ADR follows the template structure
# No code changes — documentation only
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:**
- `docs/adr/` directory exists with README, template, and 3 foundational ADRs.
- Each ADR has all required sections filled in.
- ADR numbering is sequential (0001, 0002, 0003).

---

## Step 7 — Add Documentation Enforcement to CI

**Goal:** Prevent documentation regression by failing CI when public API members lack XML doc comments.

**⚠️ Scale check — the "stub the rest" step is the actual bulk of this batch, not a footnote, and the real number is worse than this batch's own grep-based estimate:** the plan's `grep -c "public "` proxy returns ~9,800 occurrences — **independently re-run in this review** (`grep -rc "public " Assets/Ashfall.Core --include="*.cs" | awk -F: '{sum+=$2} END{print sum}'` → 9,779, consistent with the plan's estimate) — but a `grep` count of the word "public" was never going to be the authoritative number, since it both overcounts (matches `public class`/`public interface` declarations and any occurrence inside comments/strings) and undercounts (misses implicit public members like auto-property backing, and doesn't reflect what the compiler actually flags). **This review went further and actually measured it: running `dotnet build Ashfall.Core/Ashfall.Core.csproj -p:GenerateDocumentationFile=true -p:NoWarn=""` (i.e. genuinely flipping the flag Step 1 proposes and removing the CS1591 suppression, then reverting) produces exactly 10,946 CS1591 warnings, 0 errors, on the current codebase.** That is roughly **1,150 more than the plan's own estimate suggested**, and it is the authoritative number — not a proxy. Even accounting for Steps 2–3 hand-documenting ~20 types (5 ports + ~5 default impls + 15 systems, 2 of which need replacement names per Step 3's correction), that leaves on the order of 10,900 undocumented public members needing at least a stub `<summary>` before `WarningsAsErrors=CS1591` can be turned on without breaking the build. This is not a "temporarily add stubs" afterthought — it is the single largest-effort step in this batch, almost certainly larger than Steps 1–6 combined, and the original plan's implicit "5-7 sessions total" estimate for the whole batch is not credible once this number is known. Do not re-derive this number by grep again in the future — the 10,946 figure above was obtained by actually building with the flag on; re-run the same build command if the codebase has changed materially since this review.

**Implementation:**
- Remove `CS1591` from `<NoWarn>` in `Ashfall.Core/Ashfall.Core.csproj` (added in Step 1).
- Add `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` scoped to CS1591 only:
  ```xml
  <PropertyGroup>
    <WarningsAsErrors>CS1591</WarningsAsErrors>
  </PropertyGroup>
  ```
- This means any `public` member without an XML doc comment will fail the build.
- **The one-time "get the exact count" build has already been run in this review — use 10,946 as the starting figure, not a fresh unknown** (re-run the same command if material time has passed or the codebase has changed: `dotnet build Ashfall.Core/Ashfall.Core.csproj -p:GenerateDocumentationFile=true -p:NoWarn=""`, then revert — do not leave these properties set from the measurement run). Given a figure in the ten-thousands, manual stubbing is not realistic within any reasonable session budget — this must be scripted. A one-off Roslyn-based codemod that inserts `/// <summary>TODO: Document this system.</summary>` above every public member lacking a doc comment is the only realistic approach at this scale.
- For systems not yet documented (beyond the 15 from Step 3 — corrected to 13 confirmed + 2 replacement slots), add minimal `<summary>` stubs:
  ```csharp
  /// <summary>TODO: Document this system.</summary>
  ```
  This satisfies the compiler while making undocumented APIs visible via search.
- Update CI script to include:
  ```bash
  dotnet build Ashfall.Core/Ashfall.Core.csproj -warnaserror:CS1591
  ```
  **Do not route this into `docs/CI.md` as the original plan suggested as an alternative to a CI script.** `docs/CI.md` was read in this review and is entirely Unity-oriented — it documents `UNITY_LICENSE`/`UNITY_EMAIL`/`UNITY_PASSWORD` GitHub secrets and a `unity -batchmode -nographics -runTests` local verification example. Per AGENTS.md's non-negotiable rule #1/#2 (Unity is not a target editor; verification is `dotnet` + `godot --headless` only), adding a `dotnet build ... -warnaserror:CS1591` instruction into that specific file would sit inside Unity-batchmode documentation that this project's own rules say should not be invoked. Either add the doc-enforcement note to a new or different doc (e.g. a new "Core doc enforcement" section in `docs/architecture/` or a small addition to whatever `.github/workflows/*.yml` already runs the `dotnet build`/`dotnet test` steps) or update `docs/CI.md`'s scope as a separate, explicitly-called-out cleanup task — not silently folded into this step.
- Add a doc coverage report step. **Scope note:** the original plan bolts this onto `tools/DependencyGraphGenerator/` (built in Step 4) via a `--doc-coverage` flag. Counting documented-vs-undocumented public members from the XML doc file is a different concern from parsing dependency edges between classes, and conflating the two inside one tool risks an awkward, dual-purpose CLI. Prefer a separate, small script/tool (e.g. `tools/DocCoverageReport/` or even a short `dotnet-script`/shell script that parses the XML doc file's `<member>` entries against a Roslyn-derived public-member list) unless there's a concrete reason to share code with the dependency graph generator (e.g. both need the same Roslyn symbol-walk, in which case factor that walk into a shared library both tools reference, rather than merging the two CLIs):
  ```bash
  # Count public members vs documented members from XML
  dotnet run --project tools/DocCoverageReport/ -- --doc-coverage
  ```
  Output: `docs/doc-coverage-report.txt` with percentage and list of undocumented members.

**Verification:**
```bash
dotnet build Ashfall.Core/Ashfall.Core.csproj
# Confirm: builds without CS1591 warnings (all public APIs documented or stubbed)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Confirm: all tests pass
# Test enforcement: temporarily remove a <summary> tag, confirm build fails
```

**Done when:**
- `CS1591` is treated as an error in `Ashfall.Core/Ashfall.Core.csproj` (corrected path — see Step 1's Verified Premise).
- All public API members have at least a stub `<summary>` — confirmed by an actual zero-warning build (re-running the exact command used in this review to get 10,946 as the baseline, then confirming it returns 0), not by "the 15 systems from Step 3 are done so the rest must be fine."
- CI fails if a new public member is added without documentation, via a location that is not `docs/CI.md`'s existing Unity-secrets content unless that file is explicitly re-scoped first (see correction above).
- Doc coverage report is generated and tracked.

**Risk & Rollback:** This is the one step in the batch where "Risk: None" is wrong (see the Summary Table correction below). Once `WarningsAsErrors=CS1591` is live, **every future PR touching any public member in `Assets/Ashfall.Core/` will fail to build if it forgets a doc comment** — this blocks unrelated work, not just this batch's own scope. Rollback is straightforward (revert the `<NoWarn>`/`<WarningsAsErrors>` property change, which is a single, easily-identified diff), but the real risk is in the interim: if the codemod stub sweep is incomplete or missed a file added concurrently by another task, CI breaks for everyone touching Core until the gap is found. Mitigate by running the stub sweep and turning on `WarningsAsErrors` in the same commit/PR (never landing the strict flag before the stub sweep is verified complete via a green `dotnet build`), and by announcing the change (this flips a previously-lax build into a strict one for the whole team, not just this batch's contributors).

---

## Summary Table

| Step | Title | Risk | Touches Game Code | Output |
|------|-------|------|-------------------|--------|
| 1 | Enable XML doc generation | None | .csproj only (path corrected: `Ashfall.Core/Ashfall.Core.csproj`, not `Assets/Ashfall.Core/Ashfall.Core.csproj`; open question on whether `Ashfall.csproj`, the Godot host, also needs the flag) | `Ashfall.Core.xml` build artifact |
| 2 | Document port interfaces | None | Comments only (4 of 5 interfaces already have a `<summary>`; only `ILog` starts from zero; implementation class names corrected: `FileSystemIO`/`SimClock`/`SeededRng`/`NullLog`, not `DefaultFileIO`/`DefaultClock`/`CoreSeededRng`) | XML docs for 5 interfaces + defaults |
| 3 | Document top 15 systems | None | Comments only (4 of 15 original names don't exist — corrected to 13 confirmed + 2 replacement slots pending cross-reference verification; 10 of the 13 confirmed names already have a class-level `<summary>`) | XML docs for 15 systems |
| 4 | Dependency graph generator | None | No game code | `docs/dependency-graph.json`, `.mmd` |
| 5 | System overview with Mermaid | None | No code (note: `docs/architecture/dual_engine_plan.md` already exists and is stale/contradictory — states Unity is primary — cross-link or supersede it, don't let two contradictory architecture docs coexist silently) | `docs/SYSTEM_OVERVIEW.md` |
| 6 | ADR framework | None | No code | `docs/adr/` with 3 foundational ADRs |
| 7 | CI doc enforcement | **Medium** (build-breaking once live — every future PR touching a public member in `Assets/Ashfall.Core/` fails without a doc comment; the real stub-sweep scope is 10,946 CS1591 warnings, an actually-measured build result, not the ~9,800 grep estimate; not "None" as originally rated) | .csproj + a scripted stub sweep across ~303 files, not a handful | Build fails on undocumented public API |

**Corrected effort note:** the plan gives no overall estimate beyond the header's "5–7 sessions." Given Step 7's real, measured scope (10,946 CS1591 warnings on a real build, not an estimate), this batch is more realistically **7–10 sessions**, with Step 7 alone consuming 2–4 of those depending on how much of the stub sweep is scripted vs. reviewed by hand.

---

## Exit Criteria (Batch 68 Complete)

- [ ] `dotnet build` produces XML documentation file from `Ashfall.Core/Ashfall.Core.csproj` (corrected path), confirmed present on disk at `Ashfall.Core/bin/Debug/net8.0/Ashfall.Core.xml` (path independently confirmed in this review).
- [ ] All 5 port interfaces fully documented (4 already have a partial `<summary>`; all 5 need `<param>`/`<returns>`/`<remarks>` added; `ILog` needs a `<summary>` from scratch).
- [ ] Top 15 systems fully documented, with the 2 corrected replacement slots (for the non-existent `SurvivorRegistry`/`DynamicEconomyEngine`) filled by real, cross-reference-verified classes before this is checked off.
- [ ] Dependency graph generator produces valid JSON + Mermaid output.
- [ ] System overview document renders correctly with 3 Mermaid diagrams, and is cross-linked with (or explicitly supersedes) the existing, stale `docs/architecture/dual_engine_plan.md`.
- [ ] ADR framework established with 3 backfilled records.
- [ ] CI enforces documentation on all public APIs (confirmed via an actual zero-CS1591-warning build across all ~303 Core files — this review already ran the pre-fix baseline and got 10,946 warnings, so "zero" here is independently checkable against that number, not assumed from the ~20 hand-documented systems).
- [ ] Zero test regressions throughout.
- [ ] Zero changes to game logic or behavior.


---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings:

1. **Wrong project file path throughout.** The plan repeatedly references `Assets/Ashfall.Core/Ashfall.Core.csproj` — that file does not exist. `Assets/Ashfall.Core/` contains only source `.cs` files (303 of them, confirmed by `find Assets/Ashfall.Core -iname "*.cs" | wc -l`), no `.csproj`. The actual project that owns `GenerateDocumentationFile` and compiles those sources is the root-level `Ashfall.Core/Ashfall.Core.csproj` (confirmed by reading it: `TargetFramework net8.0`, `LangVersion 9.0`, `RootNamespace Ashfall.Core`, and a `<Compile Include="../Assets/Ashfall.Core/**/*.cs">` wildcard with an explicit comment warning not to duplicate sources into that folder). Every build/verification command referencing the fictional path was corrected to `Ashfall.Core/Ashfall.Core.csproj`.

2. **Second-pass correction (this review): an earlier draft of this same review conflated two different projects.** It described `Ashfall.Core/Ashfall.Core.csproj` as "the root-level test/build project," implying it IS the xUnit test project. That is wrong — `Ashfall.Core/Ashfall.Core.csproj` is the **class library** (net8.0, `Microsoft.NET.Sdk`, compiles the Core `.cs` sources directly). `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` is a **separate** project (net9.0, matching AGENTS.md's stated Tests TFM, xUnit) that does **not** compile the Core sources itself — it references the class library via `<ProjectReference Include="..\Ashfall.Core\Ashfall.Core.csproj" />` and consumes its compiled output. This distinction matters for this batch because `GenerateDocumentationFile` only needs to be set on the class library project, not the test project — the corrected plan text now reflects this rather than treating the two as one project.

3. **Confirmed premise (the one part that was right from the start):** `Ashfall.Core/Ashfall.Core.csproj` does currently have `<GenerateDocumentationFile>false</GenerateDocumentationFile>` — so the batch's core motivation (docs generation is currently off) is accurate; only the file path and the exact XML output path (`net8.0`, not `netstandard2.1`, since that's the concrete project's TFM) needed correcting. Also confirmed: `/Ashfall.csproj` (the Godot host) independently compiles the same `Assets/Ashfall.Core/**/*.cs` files and has no `GenerateDocumentationFile` property at all (defaults to false) — this batch should make an explicit decision about that project too, not silently ignore it, since AGENTS.md's mandatory verification checklist runs `dotnet build Ashfall.csproj` as a separate step from the Tests build.

4. **`.gitignore` claim was backwards.** The plan says to "add `Assets/Ashfall.Core/bin/` to `.gitignore` if not already present." Checked: `.gitignore` already excludes `/Ashfall.Core/bin/` and `/Ashfall.Core/obj/` (the correct root-level path) at lines 184-185 (confirmed by `grep -n` in this review, not just visual inspection). There is no `bin/` under `Assets/Ashfall.Core/` to ignore in the first place, since that directory holds only source files, not a build output tree. This sub-step is now marked as unnecessary/already-satisfied rather than an action item.

5. **Severely underestimated the CI-enforcement step (Step 7) — and the earlier grep-based estimate itself undercounted the true figure.** The plan originally treated "add stub `<summary>` comments for everything not covered by Steps 2–3" as a minor mop-up. A `grep -c "public "` scan returns ~9,779 hits across 303 files — but **this review went further and actually ran the build with `GenerateDocumentationFile=true` and `CS1591` unsuppressed (then reverted): the real, compiler-measured count is 10,946 CS1591 warnings, 0 errors.** That is the authoritative figure, roughly 1,150 higher than the grep-based proxy, because the proxy both over- and under-counts in ways that don't cancel out cleanly. Steps 2-3 only hand-document on the order of 20 types (and even then, this review found most of those 20 already had partial docs — see finding 7 below) before Step 7 needs the entire remaining codebase stubbed to flip `WarningsAsErrors=CS1591` on without breaking the build. Step 7 is flagged as likely the single largest-effort step in the batch, a scripted/codemod approach is recommended over manual stubbing at this scale, and the overall batch effort estimate is raised from "5-7 sessions" (implied, not stated) to an explicit 7-10 sessions. Step 7's Risk rating is also corrected from "None" to "Medium" in this pass (see finding 9 below) — a genuinely missing risk/rollback callout in the original plan.

6. **Scope-creep flagged, not silently accepted:** Step 7's doc-coverage reporting was originally bolted onto `tools/DependencyGraphGenerator/` (built in Step 4) via a `--doc-coverage` flag, conflating two unrelated concerns (dependency-edge parsing vs. doc-coverage counting) inside one CLI tool. Corrected to recommend a separate small tool/script unless a concrete code-sharing reason (shared Roslyn symbol walk) justifies combining them.

7. **New finding (not caught by the earlier draft of this review): Step 2's port-interface docs and Step 3's top-15-systems docs are not starting from a blank slate, and Step 2's implementation-class names are wrong.** Direct reads of `Assets/Ashfall.Core/Ports.cs` and `Assets/Ashfall.Core/HostDefaults.cs` found: (a) 4 of the 5 port interfaces (`IJsonSerializer`, `IFileIO`, `IClock`, `ISeededRng`) already have a class-level `<summary>` — only `ILog` has none; (b) the default-implementation class names in the original plan (`DefaultFileIO`, `DefaultClock`, `CoreSeededRng`) do not match the real names (`FileSystemIO`, `SimClock`, `SeededRng`), and the plan omitted the real `NullLog` default entirely; (c) of the 15 systems targeted in Step 3, 4 names (`InventorySystem`, `MedicalAfflictionPipeline`, `SurvivorRegistry`, `DynamicEconomyEngine`) do not exist anywhere in the codebase under those exact names (`InventorySystem` should be `Inventory`; the other three have no clear existing equivalent and need real replacements chosen by actual cross-reference count, not guessed); and (d) of the 11 remaining valid names, 10 already have a class-level `<summary>` today — only `CatalogIntegrityValidator` has none. Both steps were corrected to state precisely what already exists vs. what is net-new work, and Step 3's target list was corrected to flag the 2 replacement slots as open decisions rather than filled-in names.

8. **New finding: `docs/CI.md`, which Step 7 originally offered as an alternative destination for the CI doc-enforcement instructions, is entirely Unity-oriented** (documents `UNITY_LICENSE`/`UNITY_EMAIL`/`UNITY_PASSWORD` secrets and a Unity-batchmode local verification example) — routing new `dotnet build ... -warnaserror:CS1591` guidance into that file without first re-scoping it would place Godot/dotnet-only instructions inside a Unity-specific doc, in mild tension with AGENTS.md's non-negotiable rule that Unity is not the active verification path. Step 7 was corrected to flag this rather than silently pointing at `docs/CI.md` as a clean target.

9. **Missing risk/rollback — added for Step 7 specifically (Steps 1-6 remain genuinely low/no-risk and needed no addition).** The original "Risk: None" framing for the whole batch was accurate for Steps 1-6 (pure additive comments/tooling/docs) but wrong for Step 7: once `WarningsAsErrors=CS1591` is live, every future PR touching a public member in `Assets/Ashfall.Core/` fails to build without a doc comment — a genuinely build-breaking, team-wide-blast-radius change, not a "None" risk. Rollback for Step 7 is a one-line property revert, but the interim risk (an incomplete stub sweep breaking concurrent PRs) needed an explicit mitigation note, which Step 7 now has: land the stub sweep and the strict flag in the same change, verified green, never in two separate steps.

10. **Motivational stats in this batch's own opening paragraph were also wrong and are corrected:** "34 host sessions" is corrected to 27 (in `src/Host/`) / 29 (project-wide, matching Batch 67's independently-verified count); "22 save stores" is corrected to 27 (confirmed via project-wide `find . -iname "*SaveStore.cs"`, excluding `.uid` sidecars); "20+ directories" for Core is technically true but loosely stated — the real count is 37 top-level directories under `Assets/Ashfall.Core/`.

11. **Everything else held up:** `GodotLog.cs` at `src/Host/GodotLog.cs` (Step 2) is correctly located — confirmed by file search. The Mermaid diagram content in Step 5, the ADR template in Step 6, and the `>50 nodes, >100 edges` sizing target in Step 4 (a modest, easily-achievable bar for 303 files) needed no correction. `tools/` does not yet exist as a directory in this repo (confirmed by search), so Step 4's `tools/DependencyGraphGenerator/` is a genuinely clean, non-colliding addition. `docs/architecture/dual_engine_plan.md` already exists and is stale (claims "Unity 6 LTS is the primary... engine," directly contradicted by AGENTS.md's current non-negotiable rules) — Step 5's new `docs/SYSTEM_OVERVIEW.md` should cross-link or explicitly supersede it rather than let two contradictory architecture docs coexist silently; this is noted in the Summary Table and Exit Criteria above.
