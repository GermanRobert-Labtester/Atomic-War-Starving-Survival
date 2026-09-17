# ASHFALL — Quality Roadmap Batch 87

## Theme: API Versioning & Backward Compatibility Contract for Core Systems

**Priority:** MEDIUM
**Risk:** Low for Steps 1, 2, 6 (attributes + docs, purely additive). Medium for Steps 3–5 (reflection-based extractor/differ/CI gate) — see Review Notes for scope concerns before committing to "3 sessions."
**Estimated Effort:** Originally "3 focused sessions" — likely optimistic once the real surface size (~1,110 public types, not ~82) is accounted for. Recommend re-estimating after the Step-4 scope decision (full surface vs. annotated-tier-only) is made; annotated-tier-only is achievable in ~3 sessions, full-surface extraction/diffing is more likely 5+ sessions given the size and the need to hand-tune the diff format around real generic/nested-type edge cases in this codebase.
**Rollback:** All new types (`StabilityTier`, `StabilityAttribute`, checkers, extractor, CI test) are additive and unused by any existing code path — deleting the new files and reverting the 20 `[Stability]` annotations fully reverts this batch with no cascading effects. If the CI gate (Step 5) produces false positives after merge, the fastest safe rollback is to mark `ApiCompatibilityTests` as `[Fact(Skip = "...")]` rather than deleting it, so the intent is preserved for a follow-up fix.
**Depends On:** Nothing (can run in parallel with Batches 85–86)
**Unlocks:** Safe refactoring confidence, contributor onboarding clarity, automated break detection in CI

---

## Motivation

`Assets/Ashfall.Core/` contains roughly 300 source files and ~1,110 public type declarations (classes/interfaces/structs/enums/records), including ~107 types with `System` in the name. This is a large, high-traffic surface, but the batch's original "82+ public systems" figure was a rough guess, not a verified count — see Review Notes for the corrected figures. Consumers include:
- 30 host session files in `src/Host/` (+ a handful more in `src/YearOfAsh/`, `src/Foundry/`, `src/Disease/`) — not 34
- 199 test files in `Ashfall.Core.Tests/` (~2,120 `[Fact]`/`[Theory]` tests) — not 173 files / 1941 tests
- Expansion modules (Holdfast, YearOfAsh, Maritime, Muster, etc.)
- There is **no legacy Unity host to consider** — `Assets/_Game/` has been fully deleted (migration complete per `AGENTS.md`). Any reference to it as a "read-only consumer during migration" is stale.

Today, **any public member can change without notice**. There is:
- No API baseline snapshot
- No `[Obsolete]` migration path convention — confirmed by repo-wide grep: **zero** `[Obsolete]` attributes exist anywhere in the codebase today. This batch is establishing the convention from scratch, not building on precedent.
- No CI gate that detects public surface changes
- No stability tier classification (which APIs are safe to depend on vs. experimental)
- No contributor documentation on backward compatibility expectations

The "real incidents" listed below (`ISeededRng.NextRange()`, `NeedsSystem.GetNeed()`, `SaveChecksum.Compute()`, `CatalogRegistry.Resolve()`) could not be verified against the current codebase — none of these exact signatures currently exist in `Assets/Ashfall.Core/` (e.g. there is no `CatalogRegistry` class, and `NeedsSystem` exposes no `GetNeed()`/`NeedValue` API today). Treat these as illustrative, hypothetical scenarios, not documented past incidents. If real incident data exists, cite the actual commit/PR; otherwise reword this section as "the kind of incident this would catch" rather than asserting it already happened.

With ~2,120 tests and ~30 host sessions consuming Core, **even small public API changes cascade**. A formal compatibility contract prevents accidental breaks and provides a clear deprecation path for intentional changes.

---

## Step 1 — Define API Stability Tiers

**Goal:** Establish a three-tier classification system for public APIs, with clear rules for each tier about what changes are allowed and what process is required.

**Implementation:**

Create `Assets/Ashfall.Core/ApiStability/StabilityTier.cs`:

```csharp
namespace Ashfall.Core.ApiStability;

/// <summary>
/// Classifies the stability guarantee of a public API surface.
/// Applied to types, interfaces, methods, and properties.
/// </summary>
public enum StabilityTier
{
    /// <summary>
    /// Cannot break without a major version bump.
    /// Removal requires [Obsolete] for at least one release cycle.
    /// Signature changes are never allowed — add new overloads instead.
    /// Return type changes are never allowed — create a new method.
    /// </summary>
    Stable,

    /// <summary>
    /// May change with deprecation notice (one release cycle).
    /// [Obsolete("Use X instead. Will be removed in vN+1")] required before removal.
    /// Signature changes allowed if [Obsolete] marks the old signature.
    /// </summary>
    Beta,

    /// <summary>
    /// No backward compatibility guarantees.
    /// May change or be removed without notice.
    /// Should not be consumed by external code (host sessions, tests) unless accepted risk.
    /// </summary>
    Internal
}
```

Create `Assets/Ashfall.Core/ApiStability/StabilityAttribute.cs`:

```csharp
namespace Ashfall.Core.ApiStability;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method
    | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Struct,
    Inherited = false, AllowMultiple = false)]
public sealed class StabilityAttribute : Attribute
{
    public StabilityTier Tier { get; }
    public string Since { get; }  // Version when stability was declared (e.g., "0.9.0")
    public string Notes { get; set; }  // Optional context

    public StabilityAttribute(StabilityTier tier, string since)
    {
        Tier = tier;
        Since = since;
    }
}
```

Tier classification guidelines (documented in `Assets/Ashfall.Core/ApiStability/POLICY.md`):

| Tier | When to apply | Change rules | Examples |
|------|---------------|-------------|----------|
| **Stable** | Consumed by 3+ host sessions OR 10+ tests OR is a port interface | No breaks ever; additive only; deprecate before removal | `IJsonSerializer`, `ILog`, `NeedsSystem`, `SaveChecksum`, `CatalogIntegrityValidator` |
| **Beta** | New systems < 2 releases old OR actively being redesigned | Deprecate one release before removal; document migration | `ExpansionMasterSession`, new expansion systems, recently added methods |
| **Internal** | Helper utilities, implementation details exposed for testing | May change freely; consumers accept risk | Builder internals, test-only hooks, private-but-public-for-testing |

Default rule: **Unattributed public APIs are treated as Beta.** This incentivizes annotation — stable APIs get protection, internal APIs get freedom, unmarked APIs get a middle ground.

**Verification:**
- Attribute compiles in Core
- Policy document is clear and actionable
- Tier assignment is unambiguous for any given public member

**Done when:** `StabilityTier` enum, `StabilityAttribute`, and `POLICY.md` exist; no engine references; the tier table in `POLICY.md` gives an unambiguous, checkable rule for each tier (e.g. "consumed by 3+ host sessions OR 10+ tests" — a number you can `grep -c` for) rather than a subjective judgment call. *(Corrected: "team agrees on tier definitions" is not a verifiable Done-when — replace consensus with a mechanically checkable rule.)*

---

## Step 2 — Annotate Top 20 Core Interfaces and Classes

**Goal:** Apply `[Stability]` attributes to the 20 most-consumed public APIs, establishing the initial annotated surface that the CI gate will protect.

**Implementation:**

Priority annotation targets. **Note:** the per-type consumer counts below (host session / test counts) in the original batch were unverified estimates and have been removed — do not annotate types with fabricated numbers. Before annotating, run `grep -rl "<TypeName>" src/ Ashfall.Core.Tests/ --include="*.cs" | wc -l` per type and record the *actual* count in this table. Two type names below did not match any real class and have been corrected: `MedicalSystem` (no such class exists — the real class is `Medical/MedicalWardSystem.cs`) and `InventorySystem` (no such class exists — the real class is `Inventory/Inventory.cs` in namespace `Ashfall.Core.Inventory`) and `NarrativeSystem` (no single class by this name — narrative logic is split across multiple `Narrative*` catalogs/systems; pick a concrete anchor type such as `JournalSystem` or name a specific narrative class once one is chosen).

| # | Type | Tier | Rationale | Verified to exist? |
|---|------|------|-----------|---------------------|
| 1 | `IJsonSerializer` | Stable | Port interface, cross-host contract | Yes |
| 2 | `IFileIO` | Stable | Port interface | Yes |
| 3 | `ILog` | Stable | Port interface, universal dependency | Yes |
| 4 | `IClock` | Stable | Port interface | Yes |
| 5 | `ISeededRng` | Stable | Port interface, determinism contract | Yes |
| 6 | `SaveChecksum` | Stable | Integrity contract | Yes |
| 7 | `CatalogIntegrityValidator` | Stable | Validator contract | Yes |
| 8 | `NeedsSystem` | Stable | Core survival | Yes (`Survivors/NeedsSystem.cs`) |
| 9 | `RadiationSystem` | Stable | Core survival | Yes (`Radiation/RadiationSystem.cs`) |
| 10 | `MarketSystem` | Stable | Economy | Yes (`Economy/MarketSystem.cs`) |
| 11 | `WeatherSystem` | Stable | Environmental | Yes (`World/WeatherSystem.cs`) |
| 12 | `ExpeditionSystem` | Beta | Still evolving | Yes (`Expeditions/ExpeditionSystem.cs`) |
| 13 | `TacticalCombatSystem` | Beta | Combat redesign ongoing | Yes (`Combat/TacticalCombatSystem.cs`) |
| 14 | `MedicalWardSystem` | Beta | Affliction pipeline expanding | Yes — corrected from fictional `MedicalSystem` (`Medical/MedicalWardSystem.cs`) |
| 15 | *(pick one concrete narrative class)* | Beta | Content still growing | No `NarrativeSystem` class exists — choose a real anchor (e.g. `JournalSystem`, or a specific `Narrative*` catalog) before annotating |
| 16 | `ExpansionMasterSession` | Beta | Expansion API unstable | Yes (`ExpansionMasterSession.cs`) |
| 17 | `JournalSystem` | Beta | Coverage still low (H11) | Yes (`Journal/JournalSystem.cs`) |
| 18 | `CraftingSystem` | Stable | Mature, rarely changes | Yes (`Crafting/CraftingSystem.cs`) |
| 19 | `Ashfall.Core.Inventory.Inventory` | Stable | Core, well-tested | Yes — corrected from fictional `InventorySystem` (`Inventory/Inventory.cs`); note the class is literally named `Inventory`, not `InventorySystem` |
| 20 | `SaveWireContract` | Stable | Cross-host save compatibility | Yes (`SaveWireContract.cs`) |

Since #15 has no real target and #14/#19 needed name corrections, verify all 20 chosen types compile and exist via `grep -rn "public.*class <Name>" Assets/Ashfall.Core/` before writing the annotation PR — don't annotate from memory or from this table without checking.

Annotation example:
```csharp
namespace Ashfall.Core;

[Stability(StabilityTier.Stable, "0.9.0")]
public interface IJsonSerializer
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string json);
}
```

```csharp
[Stability(StabilityTier.Beta, "0.9.0", Notes = "Expedition encounter API still evolving")]
public class ExpeditionSystem
{
    [Stability(StabilityTier.Stable, "0.9.0")]
    public List<ExpeditionState> CaptureState() { ... }

    [Stability(StabilityTier.Beta, "0.9.0")]
    public bool Start(ExpeditionDefinition def, string survivorId, int day,
        ExpeditionStance stance = ExpeditionStance.Stealth, /* ... */) { ... }
}
```
*(Corrected from the original example, which invented a `StartExpedition(ExpeditionParams)` method and an `ISaveable` base interface — neither exists. `ExpeditionSystem` is a plain class; its actual save method is `CaptureState()` returning `List<ExpeditionState>`, and its actual start method is `Start(...)` with the signature above — verify against `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` before using any code sample from this plan verbatim.)*

Mixed-tier rule: A class can be Beta while individual methods are Stable (e.g., `CaptureState` is always Stable because save/load depends on it).

**Verification:**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles (attributes don't break anything)
- `dotnet build Ashfall.csproj` — compiles
- Grep: 20 types have `[Stability(` attribute applied
- Each tier assignment is justified in the PR description by the mechanically-checked criterion from Step 1's `POLICY.md` rule (actual consumer count via grep), not a subjective "gut-check". *(Corrected: "No tier assignment feels wrong (gut-check each one against consumer count)" is not independently verifiable by a reviewer — replace with a concrete grep-derived number per type.)*

**Done when:** 20 types annotated; annotations compile cleanly; each annotation has defensible rationale documented in this file.

---

## Step 3 — Implement PublicApiAnalyzer (API Surface Snapshot Tool)

**Goal:** Build a tool that extracts the complete public API surface of `Assets/Ashfall.Core/` into a deterministic text baseline, and can diff the current surface against a saved baseline to detect changes.

**Implementation:**

Create `Ashfall.Core.Tests/ApiSurface/PublicApiExtractor.cs`:

```csharp
namespace Ashfall.Core.Tests.ApiSurface;

/// <summary>
/// Extracts public API surface from compiled Core assembly.
/// Output format: one line per public member, sorted deterministically.
/// </summary>
public static class PublicApiExtractor
{
    public static string Extract(Assembly assembly);
}
```

Output format (one line per member, alphabetically sorted):
```
Ashfall.Core.ApiStability.StabilityAttribute : Attribute [Stable 0.9.0]
  .ctor(StabilityTier tier, string since) → void
  Notes { get; set; } → string
  Since { get; } → string
  Tier { get; } → StabilityTier
Ashfall.Core.CatalogIntegrityValidator [Stable 0.9.0]
  .ctor(IFileIO fileIo, IJsonSerializer json, ILog log) → void
  Validate(string dataPath) → ValidationReport
  ValidateFile(string filePath) → FileValidationResult
Ashfall.Core.IClock [Stable 0.9.0]
  CurrentDay { get; } → int
  Advance() → void
Ashfall.Core.IJsonSerializer [Stable 0.9.0]
  Deserialize<T>(string json) → T
  Serialize<T>(T obj) → string
...
```

Format rules:
- Types: `Namespace.TypeName : BaseType [Tier Version]`
- Members indented: `  MethodName(ParamType paramName, ...) → ReturnType`
- Properties: `  PropertyName { get; set; } → PropertyType`
- Events: `  event EventName → EventHandlerType`
- Sorted: types alphabetically by full name, members alphabetically within type
- Generic parameters: `Method<T>(T item) → List<T>`
- Stability tier included if annotated, omitted if unannotated (treated as Beta)
- `[Obsolete]` members marked: `  [OBSOLETE] OldMethod(...) → void`

Implementation approach:
- Use `System.Reflection` on the compiled `Ashfall.Core.dll`
- Filter: only `public` and `protected` members (not `internal`, not `private`)
- Include: classes, interfaces, structs, enums, delegates
- Exclude: compiler-generated types, `[EditorBrowsable(Never)]` types
- Deterministic: identical output for identical source (no GUIDs, no timestamps)
- Fast: < 500ms for the actual public surface (~1,100 public types; reflection only, no source parsing) — the original "82+ types" estimate undercounted by roughly an order of magnitude; re-baseline the performance target once the extractor is running against the real assembly rather than assuming it holds

Diff engine (`PublicApiDiffer.cs`):
```csharp
public static class PublicApiDiffer
{
    public static ApiDiffResult Diff(string baseline, string current);
}

public sealed class ApiDiffResult
{
    public IReadOnlyList<string> Added { get; }      // New public members
    public IReadOnlyList<string> Removed { get; }    // Deleted public members
    public IReadOnlyList<string> Changed { get; }    // Signature changes
    public bool HasBreakingChanges { get; }          // Any Stable member removed/changed
    public bool HasDeprecations { get; }             // New [Obsolete] annotations
}
```

Breaking change classification:
- **Breaking** (fail CI): Stable member removed, Stable method signature changed, Stable return type changed
- **Warning** (log but pass): Beta member removed without `[Obsolete]`, new public member added to Stable interface (forces implementors to change)
- **Info** (log): Internal member changed, new types/members added, `[Obsolete]` annotations added

**Verification:**
- Extractor produces identical output for unchanged source (determinism test)
- Diff detects: added method, removed method, changed signature, changed return type
- Diff correctly classifies Stable vs. Beta vs. Internal changes
- Runs in < 1 second

**Done when:** `PublicApiExtractor` and `PublicApiDiffer` implemented; output is human-readable; diff correctly identifies breaking vs. non-breaking changes.

---

## Step 4 — Generate API Baseline Snapshot

**Goal:** Create the initial baseline snapshot file that represents the current public API surface. This becomes the reference against which all future changes are compared.

**Implementation:**

Baseline file: `Ashfall.Core.Tests/ApiSurface/api-baseline.txt`

Generation process:
1. Build `Ashfall.Core` project: `dotnet build Assets/Ashfall.Core/Ashfall.Core.csproj`
2. Run extractor against the compiled DLL
3. Write output to `api-baseline.txt`
4. Commit to git (this file is tracked — it's the contract)

Baseline maintenance rules:
- **Intentional changes:** Developer updates `api-baseline.txt` as part of their PR. The diff in the PR clearly shows what public API changed.
- **Accidental changes:** CI gate fails because current surface doesn't match baseline. Developer must either revert the accidental change or intentionally update the baseline.
- **Version bumps:** When baseline is updated, the PR description must explain: what changed, why, and what the migration path is for consumers.

Helper script (`scripts/update-api-baseline.sh`):
```bash
#!/bin/bash
set -euo pipefail
dotnet build Assets/Ashfall.Core/Ashfall.Core.csproj -c Release -o /tmp/ashfall-core-build
dotnet run --project Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -- --generate-api-baseline
echo "Baseline updated. Review the diff before committing."
```

Initial baseline characteristics — **the original estimates below were wrong and are corrected here.** Verified counts against `Assets/Ashfall.Core/` (grep-based, not compiled-assembly reflection, so treat as an upper-bound approximation until the extractor actually runs):
- ~1,110 public type declarations (classes/interfaces/structs/enums/records) across ~300 source files — not ~82. The "82+ systems" figure in this batch's motivation section referred loosely to types with "System" in the name (~107 of those), not the full public surface.
- 5 port interfaces (`IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng`) — confirmed
- Public method/property/enum counts were not independently verified and should not be quoted until the extractor (Step 3) actually runs against the compiled DLL — remove these numbers from planning docs until then
- 20 annotated with `[Stability(Stable/Beta)]` per Step 2 (assuming Step 2's corrected table of 19 real + 1 TBD type is used)
- Remainder (~1,090 types) unannotated (treated as Beta by the default rule)

**Scope decision needed before Step 4 can proceed:** should the baseline snapshot the *entire* ~1,110-type public surface, or only the 20 annotated types + port interfaces? Snapshotting the full surface means the baseline file will run to many thousands of lines (not "~800–1200") and any of the ~1,090 currently-unannotated public members changing will show up as a "Beta/unclassified" diff on nearly every PR that touches Core — likely too noisy to be useful. Recommendation: scope Step 3/4's extractor to only the annotated-tier types (Stable + Beta explicitly tagged) for this batch, and treat "extract everything" as a follow-up once more of the surface is annotated. This descoping should be a decision made explicitly, not discovered after the baseline file is already 5,000+ lines long.

Baseline format header:
```
# Ashfall.Core Public API Baseline
# Generated: 2025-01-XX
# Assembly: Ashfall.Core, Version=0.9.0.0
# Stability annotations: 20 Stable, 15 Beta, 47 unclassified (treated as Beta)
# WARNING: Do not edit manually. Regenerate with: scripts/update-api-baseline.sh
#
```
*(The "20 Stable, 15 Beta, 47 unclassified" line is illustrative only — with the corrected scope decision above, replace with actual counts once Step 2's annotation set is finalized.)*

**Verification:**
- Baseline file generated successfully
- Baseline is deterministic (regenerate twice → identical output)
- Baseline committed to git
- File is human-readable (can be reviewed in PR diffs)
- Line count matches the scope decided above — if full-surface, expect several thousand lines, not "~800–1200"; if annotated-tier-only, expect a much smaller file. State which scope was chosen in the PR description.

**Done when:** `api-baseline.txt` exists, is tracked in git, its scope (full surface vs. annotated-tier-only) is explicitly documented in the file header, and it accurately represents the chosen scope of the current public API surface.

---

## Step 5 — Add CI Gate (Fail on Undocumented Stable API Changes)

**Goal:** Add an automated test that fails if the current public API surface differs from the baseline in a breaking way (Stable members removed or changed without deprecation path).

**Implementation:**

Test in `Ashfall.Core.Tests/ApiSurface/ApiCompatibilityTests.cs`:

```csharp
namespace Ashfall.Core.Tests.ApiSurface;

public class ApiCompatibilityTests
{
    [Fact]
    public void PublicApi_MatchesBaseline_NoBreakingChanges()
    {
        // Arrange
        var assembly = typeof(Ashfall.Core.ILog).Assembly;
        // NOTE: `TestContext.SolutionRoot` does not exist in this codebase — no such
        // helper is defined anywhere in Ashfall.Core.Tests. Use a path relative to the
        // test assembly's output directory instead, e.g.:
        //   var baselinePath = Path.Combine(AppContext.BaseDirectory, "ApiSurface", "api-baseline.txt");
        // and mark api-baseline.txt as a Copy-to-output-directory content item in the
        // .csproj, OR walk up from AppContext.BaseDirectory to find the repo root by
        // locating Ashfall.slnx. Pick one approach and verify it resolves correctly
        // before relying on this test in CI.
        var baselinePath = Path.Combine(AppContext.BaseDirectory, "ApiSurface", "api-baseline.txt");
        var baseline = File.ReadAllText(baselinePath);

        // Act
        var current = PublicApiExtractor.Extract(assembly);
        var diff = PublicApiDiffer.Diff(baseline, current);

        // Assert
        if (diff.HasBreakingChanges)
        {
            var message = new StringBuilder();
            message.AppendLine("BREAKING API CHANGES DETECTED:");
            message.AppendLine();
            foreach (var removal in diff.Removed.Where(r => r.Contains("[Stable")))
                message.AppendLine($"  REMOVED: {removal}");
            foreach (var change in diff.Changed.Where(c => c.Contains("[Stable")))
                message.AppendLine($"  CHANGED: {change}");
            message.AppendLine();
            message.AppendLine("To fix: either revert the change, add [Obsolete] first,");
            message.AppendLine("or update api-baseline.txt with justification in PR description.");

            Assert.Fail(message.ToString());
        }
    }

    [Fact]
    public void PublicApi_BetaChanges_LoggedAsWarnings()
    {
        var assembly = typeof(Ashfall.Core.ILog).Assembly;
        var baseline = LoadBaseline();
        var current = PublicApiExtractor.Extract(assembly);
        var diff = PublicApiDiffer.Diff(baseline, current);

        // Beta changes don't fail, but are logged for visibility
        foreach (var removal in diff.Removed.Where(r => !r.Contains("[Stable")))
            _output.WriteLine($"WARNING - Beta API removed: {removal}");
        foreach (var change in diff.Changed.Where(c => !c.Contains("[Stable")))
            _output.WriteLine($"WARNING - Beta API changed: {change}");

        // Only Stable changes are breaking
        Assert.False(diff.HasBreakingChanges,
            "Stable API surface has breaking changes. See test output for details.");
    }

    [Fact]
    public void PublicApi_NewAdditions_DoNotBreakBaseline()
    {
        var assembly = typeof(Ashfall.Core.ILog).Assembly;
        var baseline = LoadBaseline();
        var current = PublicApiExtractor.Extract(assembly);
        var diff = PublicApiDiffer.Diff(baseline, current);

        // Additions are always safe (non-breaking)
        // Log them for awareness
        foreach (var addition in diff.Added)
            _output.WriteLine($"INFO - New public API: {addition}");

        // This test always passes — it's for visibility only
        Assert.True(true);
    }

    [Fact]
    public void Baseline_IsDeterministic()
    {
        var assembly = typeof(Ashfall.Core.ILog).Assembly;
        var run1 = PublicApiExtractor.Extract(assembly);
        var run2 = PublicApiExtractor.Extract(assembly);
        Assert.Equal(run1, run2);
    }

    [Fact]
    public void StableInterfaces_CannotGainNewMembers()
    {
        // Adding a method to IJsonSerializer forces all implementors to change
        // This is a breaking change even though it's "adding"
        var assembly = typeof(Ashfall.Core.ILog).Assembly;
        var baseline = LoadBaseline();
        var current = PublicApiExtractor.Extract(assembly);
        var diff = PublicApiDiffer.Diff(baseline, current);

        var stableInterfaceAdditions = diff.Added
            .Where(a => a.Contains("[Stable") && IsInterfaceMember(a))
            .ToList();

        Assert.Empty(stableInterfaceAdditions); // Adding to stable interface is breaking
    }
}
```

CI integration:
- These tests run as part of `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- They execute in < 2 seconds (pure reflection + string comparison)
- Failure message is actionable: tells developer exactly what broke and how to fix
- No external tooling required (no Roslyn analyzers, no build tasks — just xUnit tests)

Workflow for intentional breaking changes:
1. Add `[Obsolete("Use NewMethod instead. Removal in v1.0")]` to old member
2. Add new member alongside old one
3. Update `api-baseline.txt` (shows the `[OBSOLETE]` annotation)
4. Next release: remove old member, update baseline again
5. PR reviewer sees both: the `[Obsolete]` addition and the baseline update

**Verification:**
- `dotnet test` passes with current baseline (no false positives)
- Intentionally remove a Stable method → test fails with clear message
- Intentionally add a method → test passes (addition is non-breaking)
- Intentionally change a Beta method → test logs warning but passes

**Done when:** CI gate test exists; current codebase passes; intentional breaks fail with actionable messages; false positive rate is zero.

---

## Step 6 — Define Deprecation Protocol

**Goal:** Establish a clear, documented process for deprecating and removing public APIs, ensuring consumers have migration time and the path is obvious.

**Implementation:**

Create `Assets/Ashfall.Core/ApiStability/DEPRECATION_PROTOCOL.md`:

```markdown
# Ashfall.Core Deprecation Protocol

## Overview
When a public API must change or be removed, follow this protocol
to give consumers a clear migration path.

## Deprecation Timeline

### Stable APIs (minimum two release cycles)
1. Release N: Add [Obsolete] with message + alternative
2. Release N: Add new API alongside old
3. Release N+1: Old API still functional but warns
4. Release N+2: Remove old API, update baseline

### Beta APIs (minimum one release cycle)
1. Release N: Add [Obsolete] with message + alternative
2. Release N+1: Remove old API, update baseline

### Internal APIs (no cycle required)
- May be removed immediately
- No [Obsolete] required
- Baseline update in same PR

## [Obsolete] Message Format

[Obsolete("Use {NewMember} instead. Migration: {brief instructions}. Removal target: v{X.Y}")]

Examples:
- [Obsolete("Use GetNeedValue() instead. Returns NeedValue struct with clamp info. Removal target: v1.0")]
- [Obsolete("Use ISeededRng.NextInRange(min, max) instead. Removal target: v1.0")]

## Baseline Update Rules

Every PR that changes api-baseline.txt MUST include in the PR description:
1. What changed (added/removed/modified members)
2. Why (bug fix, design improvement, simplification)
3. Migration path (what should consumers do)
4. Stability tier of affected members

## Interface Evolution

Stable interfaces CANNOT gain new members (forces implementor changes).
Instead:
1. Create new interface extending old: IJsonSerializerV2 : IJsonSerializer
2. Add new methods to V2
3. Systems that need new behavior depend on V2
4. Old implementations still satisfy IJsonSerializer

Alternative: Default interface methods (C# 8+, netstandard2.1 does not support)
Since Core targets netstandard2.1: use extension methods or new interfaces.

## Communicating Deprecations

1. [Obsolete] attribute on the member (compiler warning)
2. api-baseline.txt shows [OBSOLETE] marker (visible in PR diff)
3. Release notes mention deprecated APIs
4. Test output logs Beta removals as warnings
```

Implement deprecation helpers in Core:

```csharp
namespace Ashfall.Core.ApiStability;

/// <summary>
/// Helper to document planned removals. Does not enforce at runtime —
/// enforcement is via [Obsolete] compiler warnings + CI baseline test.
/// </summary>
public static class DeprecationNotice
{
    /// <summary>
    /// Call at the top of a deprecated method to log usage for metrics.
    /// Only logs in debug builds. Zero cost in release.
    /// </summary>
    [Conditional("DEBUG")]
    public static void LogUsage(ILog log, string memberName, string alternative)
    {
        log.Warn($"DEPRECATED: {memberName} is obsolete. Use {alternative} instead.");
    }
}
```

**Verification:**
- Protocol document is clear and unambiguous
- Examples cover all three tiers
- Interface evolution guidance is practical for netstandard2.1 constraints
- `DeprecationNotice.LogUsage` compiles, is conditional, zero cost in release

**Done when:** Protocol document exists; deprecation helper compiles; the protocol document is reviewed and merged (not "clear enough that any contributor can follow it without asking," which no one can verify at merge time — instead require one worked example); a concrete example deprecation (pick one real, low-risk Beta method that exists today, e.g. a rarely-used method on `ExpeditionSystem`) is executed end-to-end in this batch — `[Obsolete]` added, new member added alongside, baseline updated — demonstrating the full flow with real code, not a hypothetical.

---

## Step 7 — Write API Stability Tests

**Goal:** Comprehensive test coverage verifying: baseline accuracy, diff detection, stability tier enforcement, deprecation protocol compliance, and edge cases.

**Implementation:**

Test file: `Ashfall.Core.Tests/ApiSurface/ApiStabilityTests.cs`

**Extractor tests:**

| Test | Asserts |
|------|---------|
| `Extract_IncludesPublicClasses` | All public classes in output |
| `Extract_ExcludesInternalClasses` | Internal types not in output |
| `Extract_IncludesPublicMethods` | Public methods listed under their type |
| `Extract_ExcludesPrivateMethods` | Private/internal methods excluded |
| `Extract_IncludesProperties` | Properties with correct get/set markers |
| `Extract_IncludesStabilityAnnotation` | `[Stability]` shown in output |
| `Extract_IncludesObsoleteAnnotation` | `[Obsolete]` marked as `[OBSOLETE]` |
| `Extract_SortedDeterministically` | Output identical across runs |
| `Extract_HandlesGenerics` | `Method<T>(T item) → List<T>` format correct |
| `Extract_HandlesNestedTypes` | Nested public types included with parent prefix |

**Differ tests:**

| Test | Asserts |
|------|---------|
| `Diff_IdenticalBaselines_NoDifferences` | Empty diff result |
| `Diff_AddedMethod_DetectedAsAddition` | `Added` list contains new method |
| `Diff_RemovedMethod_DetectedAsRemoval` | `Removed` list contains deleted method |
| `Diff_ChangedSignature_DetectedAsChange` | `Changed` list contains modified method |
| `Diff_StableRemoval_IsBreaking` | `HasBreakingChanges == true` |
| `Diff_BetaRemoval_NotBreaking` | `HasBreakingChanges == false` |
| `Diff_InternalRemoval_NotBreaking` | `HasBreakingChanges == false` |
| `Diff_StableInterfaceAddition_IsBreaking` | Adding to stable interface breaks implementors |
| `Diff_StableClassAddition_NotBreaking` | Adding to stable class is safe |
| `Diff_ObsoleteAnnotationAdded_IsDeprecation` | `HasDeprecations == true` |

**Stability attribute tests:**

| Test | Asserts |
|------|---------|
| `PortInterfaces_AreStable` | IJsonSerializer, IFileIO, ILog, IClock, ISeededRng all `[Stable]` |
| `CaptureState_AlwaysStable` | Every `CaptureState` method on annotated types is Stable |
| `RestoreState_AlwaysStable` | Every `RestoreState` method on annotated types is Stable |
| `StableTypes_HaveNoUnstablePublicMembers` | Stable class has no unannotated-Internal members (consistency) |

**Protocol compliance tests:**

| Test | Asserts |
|------|---------|
| `ObsoleteMembers_HaveAlternativeInMessage` | `[Obsolete]` message contains "Use" or "instead" |
| `ObsoleteMembers_HaveRemovalTarget` | `[Obsolete]` message contains version target |
| `ObsoleteMembers_StillCompile` | Deprecated code paths still functional |
| `NoStableMemberRemovedWithoutObsolete` | If baseline had it and current doesn't, `[Obsolete]` must have existed in prior baseline |

**Integration / regression tests:**

| Test | Asserts |
|------|---------|
| `CurrentApi_MatchesBaseline` | The primary CI gate test (from Step 5) |
| `Baseline_FileExists` | `api-baseline.txt` is present in expected location |
| `Baseline_IsNotEmpty` | Baseline has content (catch accidental truncation) |
| `Baseline_HasExpectedHeader` | Format header present and parseable |
| `AllStabilityAnnotations_UseValidSince` | `Since` field is parseable as semver |
| `AnnotatedTypeCount_AtLeast20` | Minimum annotation coverage (grows over time) |

**Verification:**
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all new tests pass
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors, 0 warnings
- Test count increases by 30+
- Tests run in < 3 seconds total (pure reflection + string ops)
- No false positives on current codebase

**Done when:** All 30+ tests pass; API stability system is fully validated; edge cases covered; no existing tests broken.

---

## Summary

| Step | Deliverable | Files | Risk | Depends On |
|------|-------------|-------|------|------------|
| 1 | Stability tier definitions | `StabilityTier.cs`, `StabilityAttribute.cs`, `POLICY.md` | None | — |
| 2 | Top 20 types annotated | 19 existing Core files + 1 TBD (add attribute) — see Step 2 corrections | None | Step 1 |
| 3 | API surface extractor + differ | `PublicApiExtractor.cs`, `PublicApiDiffer.cs` | Medium — scope depends on the full-surface-vs-annotated-tier decision (see Step 4); reflection over ~1,110 public types is more involved than reflection over ~82 | Step 1 |
| 4 | Baseline snapshot | `api-baseline.txt`, helper script | Low if annotated-tier-only; Medium if full-surface (noisy diffs on unrelated PRs) | Steps 1–3 |
| 5 | CI gate test | `ApiCompatibilityTests.cs` | Low, but the sample code's `TestContext.SolutionRoot` path resolution needs a real implementation first (see Step 5 correction) | Steps 3–4 |
| 6 | Deprecation protocol | `DEPRECATION_PROTOCOL.md`, `DeprecationNotice.cs` | None | Step 1 |
| 7 | Test suite | `ApiStabilityTests.cs` (30+ tests) | None | Steps 1–6 |

**Total new files:** ~8 (Core: 4, Tests: 3, Scripts: 1)
**Total modified files:** ~19–20 (adding `[Stability]` attribute to existing types — count depends on resolving the Step 2 #15 placeholder)
**Breaking changes:** None (purely additive infrastructure)
**Rollback plan:** Attribute is inert — removing it has no effect on runtime behavior; CI gate test can be `[Fact(Skip = "...")]`'d if needed. Because Step 3–5 touch no existing production code paths (only add new files + a new test), rollback is git-revert-safe at any step boundary; there is no partial-migration state that could leave the codebase inconsistent.
**Ordering note:** Steps 1→2→3→4→5 are correctly sequenced (each depends on the prior). Step 6 (deprecation protocol) has no code dependency on Steps 2–5 and could run in parallel with them if split across sessions — the "Depends On: Step 1" in this table is accurate but conservative.

---

## Long-Term Benefits

| Benefit | Impact | When Realized |
|---------|--------|---------------|
| Accidental breaks caught in CI | Prevents future accidental breaking changes to annotated Stable APIs (no historical incident rate was verifiable — the original "2–3 incidents per month" figure is unsupported by any data found in the repo and should not be quoted as fact) | Immediately after Step 5 |
| Clear contributor guidance | Faster onboarding, less review friction | Immediately after Step 1 |
| Deprecation path for refactors | Safe evolution of Core APIs | After Step 6 |
| API surface documentation | Baseline file shows public contract for the scope chosen in Step 4 | After Step 4 |
| Stability tier incentive | Teams annotate their systems for protection | Gradual adoption |
| Interface evolution without breaks | New capabilities without forcing implementors | After protocol established |

---

## Future Enhancements (Not in This Batch)

- **Automatic baseline regeneration in CI** (update baseline on `main` after merge)
- **API diff in PR comments** (bot posts what changed for reviewer awareness)
- **Coverage tracking** (percentage of public APIs annotated, trend over time)
- **Roslyn analyzer** (real-time IDE warnings when modifying Stable APIs)
- **Semantic versioning automation** (breaking change in diff → bump major version)
- **Cross-assembly tracking** (host sessions consuming Core — detect consumer-side breaks)

---

## Exit Criteria

- [ ] `StabilityTier` enum and `StabilityAttribute` exist in `Assets/Ashfall.Core/ApiStability/`
- [ ] 20 types annotated with appropriate stability tiers (per Step 2's corrected table — verify each type name exists via `grep` before annotating; #15 has no confirmed target yet)
- [ ] `PublicApiExtractor` produces deterministic, human-readable API surface output, with scope (full surface vs. annotated-tier-only) explicitly decided per Step 4
- [ ] `PublicApiDiffer` detects additions, removals, and changes with correct tier classification
- [ ] `api-baseline.txt` committed to git, accurately representing the chosen scope of the current public API
- [ ] CI gate test fails on undocumented Stable API breaks; baseline path resolution in `ApiCompatibilityTests.cs` uses `AppContext.BaseDirectory` (or an equivalent working mechanism) instead of the nonexistent `TestContext.SolutionRoot`
- [ ] Deprecation protocol documented with clear examples and timeline, demonstrated end-to-end with one real deprecated method
- [ ] 30+ new tests pass; 0 existing tests broken
- [ ] `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors *(added: this step was missing from the original exit criteria, even though it is verification step 1 in the project's own standard checklist)*
- [ ] `dotnet build Ashfall.csproj` — 0 errors
- [ ] `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass
- [ ] `godot --headless --path . -- --data-integrity-selftest` — 0 errors
- [ ] False positive rate: zero (current codebase passes without baseline changes)


## Review Notes (Corrected)

Adversarial pass against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. All fixes above were applied in place; this section documents what was wrong, why, and what still needs a human decision.

### Factual corrections applied

| Claim in original batch | Verified reality | Evidence |
|---|---|---|
| "82+ public systems" | ~1,110 public type declarations across ~300 files in `Assets/Ashfall.Core/`; ~107 types have "System" in the name | `grep -rE "public...(class|interface|struct|enum|record)" Assets/Ashfall.Core` |
| "34 host sessions in `src/`" | 30 files matching `*HostSession*.cs` under `src/Host/` (+3 more elsewhere: `YearOfAshHostSession`, `SilentFoundryHostSession`, `DiseaseHostSession` — 33 total by broad pattern, still not 34, and none are literally "in `src/`" as a flat count without qualification) | `find src -iname "*HostSession*.cs"` |
| "173 test files" / "1941 tests" | 199 test files; ~2,120 `[Fact]`/`[Theory]` attributes | `find Ashfall.Core.Tests -iname "*.cs" \| wc -l`; grep count of `[Fact]`/`[Theory]` |
| "Legacy Unity host (`Assets/_Game/`) — read-only but still a consumer" | `Assets/_Game/` does not exist on disk at all — fully deleted, consistent with `AGENTS.md`'s "BRIDGE SHIM — REMOVED" note | `ls Assets/_Game` → No such file or directory |
| Four "real incidents" (`ISeededRng.NextRange()`, `NeedsSystem.GetNeed()` returning `NeedValue`, `SaveChecksum.Compute()` param swap, `CatalogRegistry.Resolve()`) | None of these exact APIs exist in the current codebase (no `CatalogRegistry` class at all; `NeedsSystem` has no `GetNeed()`/`NeedValue`) — these read as invented illustrative scenarios, not documented incidents | grep across `Assets/Ashfall.Core/` for each symbol, zero matches |
| Zero prior `[Obsolete]` convention assumed to already exist informally | Confirmed: **zero** `[Obsolete]` attributes exist anywhere in the non-quarantined codebase today | `grep -r "\[Obsolete" --include="*.cs" .` (excluding `_quarantine_legacy/`) → no matches |
| Step 2 table: `MedicalSystem` | No such class. Real class is `MedicalWardSystem` in `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` | `grep -r "class MedicalSystem"` → no matches; `class MedicalWardSystem` found |
| Step 2 table: `InventorySystem` | No such class. Real class is `Ashfall.Core.Inventory.Inventory` (flat slot list, not a "system" suffix) | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| Step 2 table: `NarrativeSystem` | No single class by this name; narrative logic is spread across multiple `Narrative*` catalog/system files | grep found no `class NarrativeSystem` |
| Step 2 code sample: `ExpeditionSystem : ISaveable` with `StartExpedition(ExpeditionParams params)` | `ExpeditionSystem` implements no `ISaveable` interface (doesn't exist); its real methods are `CaptureState()` returning `List<ExpeditionState>` and `Start(ExpeditionDefinition def, string survivorId, int day, ExpeditionStance stance, ...)` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:79,143,393` |
| Step 4: "~82 public classes/interfaces," "~800–1200 lines for 82 types" baseline estimate | Real count is ~1,110 public types — an order of magnitude off. A full-surface baseline would run to several thousand lines, not 800–1200 | same type-count grep as above |
| Step 5 code sample: `TestContext.SolutionRoot` | No such helper exists anywhere in `Ashfall.Core.Tests` — this line would not compile as written | `grep -r "TestContext" Ashfall.Core.Tests` → no matches |
| "Prevents 2–3 incidents per month" (Long-Term Benefits) | No incident-tracking data was found anywhere in the repo to support this figure; treated as fabricated and removed | no incident log or issue tracker artifact found in repo root |

### Scope concern raised (not auto-resolved — needs a decision)

Step 3/4 originally assumed the API-surface extractor would run against "82+ types" and produce an "~800-1200 line" baseline. With the real surface at ~1,110 public types, a full-surface extraction is a materially bigger undertaking and would produce a baseline so large and so frequently touched that the CI gate risks being noisy on nearly every PR that touches Core (since ~1,090 of those types are currently unannotated and thus "Beta" by the default rule — any change to any of them shows a diff). **Recommendation, now embedded inline in Step 4:** scope the extractor/baseline to only the explicitly `[Stability]`-annotated types for this batch (the 20 from Step 2), and treat whole-assembly extraction as a distinct follow-up batch once annotation coverage is much higher. This is a real scope-creep risk in the original plan that would have only surfaced mid-implementation.

### Roslyn analyzer scoping — verdict

The review was asked whether a Roslyn-based API analyzer is realistic for a single batch. Good news: the original batch already gets this right — Step 3 uses plain `System.Reflection` over the compiled `Ashfall.Core.dll`, not a Roslyn source analyzer, and a Roslyn analyzer is explicitly deferred to "Future Enhancements (Not in This Batch)." No dependency on `Microsoft.CodeAnalysis` exists anywhere in this repo today (verified by grep across all `.csproj` files), so introducing one would be a nontrivial new toolchain addition — correctly out of scope here. No fix was needed for this; flagging it confirms the plan's existing judgment was sound on this specific point, even though several of its supporting numbers were wrong.

### Ordering / dependency check

Steps 1→2→3→4→5→ are correctly ordered (each genuinely depends on the previous). Step 6 (deprecation protocol) has no code dependency on Steps 2–5 and its "Depends On: Step 1" is conservative but not wrong. Step 7 (test suite) correctly depends on 1–6. No illogical ordering found.

### Risk / rollback — added

The original file stated overall risk as "Low" and had a one-line rollback note only in the Summary section. Rollback and per-step risk notes have been added inline (header, Summary table) reflecting that Steps 1/2/6 are genuinely low-risk (pure additive attributes/docs) while Steps 3–5 carry medium risk due to the corrected scope size, and that rollback is git-revert-safe at any point since no existing production code path is touched.
