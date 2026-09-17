# ASHFALL — Quality Roadmap Batch 66

## Theme: Catalog Loader Unification — DRY Pattern & Generic Base Class

**Priority:** MEDIUM-HIGH (25 loaders with duplicated patterns — see corrected count below)
**Risk:** Medium, not "Low" — a prior draft of this document rated this "Low — structural refactor of loaders only," but the audit below establishes that 21 of 25 loaders require a `static class` → instance class conversion (a call-site-breaking change across every caller), a Core port (`IFileIO`) must gain a new method to support multi-file directory scanning, and at least 4 loaders (`WarlordDoctrineCatalogLoader`, `VerdictCatalogLoader`, `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`) do not fit the proposed base class's shape at all and need bespoke handling. This is not a purely mechanical, risk-free refactor.
**Blocked by:** Nothing (all loaders already functional)
**Unlocks:** Uniform schema_version enforcement, centralized error logging, single point for retry/fallback policy

---

## Context

The project has **25** Core catalog loaders in `Assets/Ashfall.Core/` (not 21 — see corrected list below), scattered across per-domain subfolders (`Combat/`, `Crossing/`, `Disease/`, `DutyRoster/`, `Economy/`, `Foundry/`, `Maritime/`, `Muster/`, `Narrative/`, `StandingRecord/`, `Survivors/`, `UtilityAI/`, `Verdict/`, `Warlords/`, `YearOfAsh/`) or loose at the `Assets/Ashfall.Core/` root. There is **no existing `Data/` subfolder or shared base class of any kind** — every loader is hand-written and independent.

**Corrected loader inventory (25, confirmed by `grep -rlE "class\s+\w*CatalogLoader" Assets/Ashfall.Core --include=*.cs`):**
CombatCatalogLoader, CrossingQuestCatalogLoader, CrossingCatalogLoader, DiseaseCatalogLoader, DoseContentCatalogLoader, DoseRegistersCatalogLoader, DutyRosterCatalogLoader, GoodsCatalogLoader, SilentFoundryCatalogLoader, SilentFoundryConsequenceCatalogLoader, HoldfastCatalogLoader, DeepLoreLocationCatalogLoader, DiveSiteCatalogLoader, CurrentsCatalogLoader, WitnessCatalogLoader, NarrativeEncounterCatalogLoader, StandingRecordCatalogLoader, SurvivorCatalogLoader, UtilityActionCatalogLoader, VerdictCatalogLoader, WarlordDoctrineCatalogLoader — plus **four loaders missing from the original list**: `VerdictNpcCatalogLoader` (`Verdict/VerdictNpcSystem.cs:112`), `VerdictQuestCatalogLoader` (`Verdict/VerdictQuestCatalogLoader.cs:14`), `DoorEncounterCatalogLoader` (`YearOfAsh/DoorEncounterCatalogLoader.cs:16`), and `YearOfAshCatalogLoader` (`YearOfAsh/YearOfAshCatalogLoader.cs:124`). The original "21" figure appears to have been transcribed from AGENTS.md's C6/H4 known-issues table ("21 remaining" `JsonUtility` call sites in the *Unity* `Assets/_Game/Data/*CatalogLoader.cs` tree — a completely different, unrelated set of files), not derived from the actual `Assets/Ashfall.Core/` listing.

**Critical correction — the loaders do NOT share an identical constructor signature.** This is the single most important fact this batch must account for, since it invalidates the "thin subclass" premise as originally scoped. Verified by reading the actual constructor/factory-method code of **all 25** loaders (a prior review pass of this document checked only 10 and undercounted the instance-class tier as a result — corrected below):

- **21 of 25 loaders are `static class`es with a `static` `Load(...)` method — they have no instance constructor at all.** E.g. `GoodsCatalogLoader.Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`Economy/GoodsCatalog.cs:88`), `SurvivorCatalogLoader.Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`Survivors/SurvivorCatalog.cs:207`), `DoseContentCatalogLoader.Load(...)` (`DoseContentCatalog.cs:86`), `DoseRegistersCatalogLoader.Load(...)` (`DoseRegistersCatalog.cs:58`), `NarrativeEncounterCatalogLoader.Load(...)` (`Narrative/NarrativeEncounterSystem.cs:245`). `VerdictCatalogLoader` doesn't even have one `Load` method — it exposes several parallel static methods (e.g. `LoadLocations(...)`) for different data slices. `WarlordDoctrineCatalogLoader.Load(...)` (`Warlords/WarlordDoctrineCatalog.cs:136`) further diverges by **throwing `InvalidOperationException`** on missing/empty/malformed data, instead of returning an empty result like the others — confirmed by direct read: it throws on missing ports/directory, missing file, empty file, failed parse, `<3` doctrines, and `<2` territory nodes (six distinct throw sites, not just "on missing/malformed"). `VerdictQuestCatalogLoader.LoadAndRegister(QuestlineSystem system, string dataDir, IFileIO fileIO, IJsonSerializer json)` and `VerdictNpcCatalogLoader.LoadAndRegister(VerdictNpcSystem system, ...)` take a domain system as their *first* argument and mutate it directly (registration-style), rather than returning a catalog.
- **4 of 25 (`DutyRosterCatalogLoader`, `HoldfastCatalogLoader`, `CrossingCatalogLoader`, `StandingRecordCatalogLoader`) are true instance classes with a real constructor — not 2, as an earlier pass of this document claimed.** `CrossingCatalogLoader` (`CrossingCatalog.cs:144,163`) and `StandingRecordCatalogLoader` (`StandingRecord/StandingRecordCatalog.cs:56,64`) were missed entirely in the prior "only 2" count; both are `sealed class`es with the identical `(IFileIO files, IJsonSerializer json, ILog log = null)` constructor and a separate `Load(string dataDirectory)` instance method, exactly matching the shape already identified for `DutyRosterCatalogLoader`/`HoldfastCatalogLoader`. All four instance-class loaders do **not** match the originally claimed `(string dataDirectory, IFileIO files, IJsonSerializer json)` shape. Their actual constructor is `(IFileIO files, IJsonSerializer json, ILog log = null)` — `dataDirectory` is **not** a constructor parameter at all; it's passed later to a separate instance method, `Load(string dataDirectory)` (`DutyRosterCatalogLoader`, `CrossingCatalogLoader`, `StandingRecordCatalogLoader`) or `Load(string dataDirectory, bool expansionUnlocked = true)` (`HoldfastCatalogLoader`, which additionally carries a unique `expansionUnlocked` parameter no other loader has). All four also depend on `ILog`, which the 21 static-method loaders don't take at all. `CrossingCatalogLoader` additionally diverges from `DutyRosterCatalogLoader`/`HoldfastCatalogLoader`/`StandingRecordCatalogLoader` by loading **five** separate files per call (`crossing_factions.json`, `crossing_locations.json`, `crossing_quests.json`, `crossing_items.json`, `crossing_encounters.json`) inside one `Load()` — it is a genuine multi-file loader, unlike the other three instance-class loaders, which each load one file.

This means a generic `CatalogLoader<T>` base class with the constructor originally proposed cannot represent any of the 25 real loaders as-is — 21 aren't even instance classes, and the 4 that are use a different parameter set and a different call pattern (constructor + separate `Load(path)` call, vs. one static `Load(dataDir, ...)` call). Step 2's design is corrected below to account for this. **Every subsequent reference to "23 static-tier loaders" or "2 instance-class loaders" elsewhere in this document is likewise corrected to 21 and 4 respectively — see the per-step corrections below.**

**Additional correction:** `IFileIO` (`Assets/Ashfall.Core/Ports.cs:19-24`) has **no `GetFiles`/`ListFiles`/directory-enumeration method** — only `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine` (5 methods, confirmed by direct read of the interface). Existing multi-file loaders (`DoseContentCatalogLoader`, `HoldfastCatalogLoader`, `DutyRosterCatalogLoader`, `CrossingCatalogLoader`, `StandingRecordCatalogLoader`, `VerdictCatalogLoader`) each hardcode filenames as `const string XxxFile = "..."` and call `Combine`+`FileExists`+`ReadAllText` per file explicitly — there is no directory globbing anywhere in the existing catalog-loading code. The only `Directory.GetFiles` call in the whole Core project is in `CatalogIntegrityValidator.cs:384`, which calls `System.IO.Directory` **directly**, bypassing `IFileIO` (a separate, pre-existing engine-coupling issue, unrelated to the loaders). A `MultiFileCatalogLoader<T>.ResolveDirectory()` + `IFileIO.GetFiles(directory, "*.json")` design, as originally proposed, requires **adding a new method to the `IFileIO` port itself** before it can work — this is a port change, not just a loader refactor, and must be scoped and reviewed as such.

**Correction to the port-change blast-radius claim:** `FileSystemIO` (`Assets/Ashfall.Core/HostDefaults.cs:13`) is **not** "the Godot host's default `FileSystemIO` adapter" as an earlier pass of this document repeatedly called it — it is defined directly in `Assets/Ashfall.Core/`, i.e. it is Core's own default BCL-backed `IFileIO` implementation (`public sealed class FileSystemIO : IFileIO`), not a Godot-specific host adapter. It is used both by `Ashfall.Core.Tests` (as the concrete `IFileIO` in most test files, e.g. `DoseContentCatalogTests.cs`, `DutyRosterIntegrationTests.cs`) and reused as-is by the Godot host — there is no separate Godot-specific `IFileIO` implementation to update, because the Godot host does not appear to have written its own; it consumes Core's `FileSystemIO` directly. This narrows (not widens) the port-change blast radius described elsewhere in this document: adding a method to `IFileIO` mainly means updating `Assets/Ashfall.Core/HostDefaults.cs` itself, plus any standalone test-double/mock `IFileIO` implementations found via `grep -rn ": IFileIO"` — not a distinct Godot-side class. Every reference below to "the Godot host's `FileSystemIO` adapter in `HostDefaults.cs`" should be read as "Core's own default `FileSystemIO` adapter (`Assets/Ashfall.Core/HostDefaults.cs`), which the Godot host also happens to consume."

**Bare-catch counts corrected again — a prior review pass of this document undercounted these too.** Direct line-by-line inspection of each file's `catch` sites:
- `VerdictCatalogLoader` (`Verdict/VerdictCatalogLoader.cs`) has **4** exception-swallowing `catch` blocks, not "1": lines 51, 105, 141 are `catch { return result; }` and line 166 is a truly empty `catch { }`. All four discard the caught exception without logging it — a prior "corrected" pass of this document counted only line 166 and missed the other three because `catch { return result; }` doesn't look bare at a glance, but it swallows the exception identically.
- `YearOfAshCatalogLoader` (`YearOfAsh/YearOfAshCatalogLoader.cs`) has **10** exception-swallowing `catch` blocks, not "5": lines 152, 183, 255, 263, 294 are empty `catch { }`, and lines 158, 189, 269, 300, 331 are `catch { return new List<...>(); }` — each paired with the empty catch immediately above it as a two-tier fallback (try structured deserialize → catch and try flat-list deserialize → catch and return empty). Both tiers discard the exception object; a prior pass of this document counted only the 5 empty ones and missed the 5 paired `catch { return ...; }` sites.
- `DoseContentCatalog.cs` has **3** such sites, not "2": line 101 (`catch { /* tolerate a malformed editorial file; integrity gate flags it */ }`) in addition to the previously-noted lines 113 and 132.

AGENTS.md's own H4 entry is flagged "unchanged," implying these counts were set once and never re-verified — treat any bare-catch count in this document (including the corrected ones above) as a snapshot to re-check at implementation time, not a fixed target. The pattern across all three files is consistent: every swallowed exception discards the `Exception` object without logging, regardless of whether the catch body is literally empty or contains a `return` — any migration or cleanup work should treat "discards the exception without logging" as the defect, not the narrower "catch block has an empty body" as originally framed.

---

## Step 1 — Audit All Loaders: Identify Shared Code, Variations, and Unique Logic

### Goal

Produce a complete inventory of every catalog loader's structure: constructor signature, file resolution strategy, deserialization approach, post-processing logic, and error handling. Classify each loader as simple (direct deserialize), moderate (multi-file or nested), or complex (custom parsing, multi-pass, or conditional logic).

### Implementation

1. Read every loader file in `Assets/Ashfall.Core/` matching `*CatalogLoader.cs` — confirmed 25 real files exist (see corrected Context list above), not the "20+" originally estimated. Include the 4 loaders missing from the original scope: `VerdictNpcCatalogLoader`, `VerdictQuestCatalogLoader`, `DoorEncounterCatalogLoader`, `YearOfAshCatalogLoader`.
2. For each, document:
   - **Actual** construction pattern — first classify as (a) `static class` with a `static Load(...)` method (21 of 25 loaders, confirmed), (b) `sealed class` with an instance constructor + separate `Load(path)` method (`DutyRosterCatalogLoader`, `HoldfastCatalogLoader`, `CrossingCatalogLoader`, `StandingRecordCatalogLoader` — 4 of 25, confirmed), or (c) `LoadAndRegister(system, ...)` registration-style static method that mutates a passed-in domain system (`VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`). Do not assume category (a) applies to all — verify each one.
   - Exact parameter list (names, types, defaults) — note where `ILog` appears (only the 4 instance-class loaders) and where `dataDirectory` is a constructor param vs. a separate method param vs. absent (registration-style loaders take no directory at all in some cases — verify per loader).
   - File path resolution (single file vs. directory scan vs. glob pattern) — note that **no loader currently uses directory globbing via `IFileIO`**, since `IFileIO` has no such method; multi-file loaders hardcode each filename as a `const string` and call `Combine`/`FileExists`/`ReadAllText` per file.
   - Deserialization target type (single object, array, dictionary, nested wrapper).
   - Post-processing (filtering, cross-referencing, ID assignment, sorting).
   - Error handling (bare catch, specific exception, logging, return empty, or **throw** — flag `WarlordDoctrineCatalogLoader` specifically, which throws `InvalidOperationException` rather than returning empty, a behavioral outlier from every other loader).
   - Return type and collection shape.
3. Produce a classification table: Simple / Moderate / Complex, **cross-tabulated against the three construction-pattern categories above** — a loader can be "simple" in its deserialization logic but still need special-case handling in the base-class design because of its construction pattern (e.g. the registration-style loaders don't return a catalog at all, so `CatalogLoader<T>.Load()` returning `IReadOnlyList<T>` cannot represent them without a separate abstraction).
4. Identify the minimal shared surface that covers all loaders (the template method skeleton) — given the finding above, this will likely need at minimum two base abstractions (one for the static-`Load`-returning-a-collection pattern, one for the instance-constructor-plus-separate-`Load(path)` pattern), not one.
5. Note any loader that deviates significantly (candidates for composition over inheritance) — explicitly flag `WarlordDoctrineCatalogLoader` (throws instead of returning empty), `VerdictCatalogLoader` (multiple parallel `Load*` methods instead of one), and the two `LoadAndRegister` loaders (mutate a passed system, don't return a catalog) as likely non-fits for any single template method.

### Risk / Rollback

None — this step is read-only analysis producing a document; it touches no production code.

### Verification

- The audit document lists all **25** loaders with their classification (not "20+" — use the confirmed count).
- No loader is left unclassified, including the 4 previously-missing ones.
- Shared and divergent patterns are explicitly called out, **especially the three distinct construction-pattern categories** (static Load, instance constructor + separate Load, LoadAndRegister) — this audit's primary deliverable is proving or disproving the "identical signature" assumption, since that assumption is what the rest of this batch's design depends on and it does not hold as originally stated.

### Done when

A markdown table or inline summary exists showing every one of the 25 loaders' classification (construction pattern + complexity tier), and the team agrees on which loaders fall into each tier — including explicit sign-off on how (or whether) `WarlordDoctrineCatalogLoader`, `VerdictCatalogLoader`, `VerdictQuestCatalogLoader`, and `VerdictNpcCatalogLoader` fit any proposed base class, given their confirmed deviations from the common pattern.

---

## Step 2 — Design CatalogLoader<T> Generic Base Class (Template Method Pattern)

### Goal

Design an abstract `CatalogLoader<T>` base class that encapsulates the shared load-and-validate pipeline while leaving extension points for loader-specific behavior.

**Corrected scope, given Step 1's findings:** the constructor below (`(string dataDirectory, IFileIO files, IJsonSerializer json, ILog log)`) matches **zero** of the 25 real loaders exactly. It's closest to the 4 instance-class loaders (`DutyRosterCatalogLoader`, `HoldfastCatalogLoader`, `CrossingCatalogLoader`, `StandingRecordCatalogLoader`), but even those defer `dataDirectory` to a separate `Load(path)` call rather than the constructor. It does not fit the 21 static-`Load`-method loaders at all, since a `static class` cannot inherit from an instance base class — those loaders would need to be converted from `static class` to instance class as part of migration (a bigger, more disruptive change than "migrate to inherit," since every call site that currently does `XxxCatalogLoader.Load(...)` as a static call becomes `new XxxCatalogLoader(...).Load(...)` and every caller must be updated). This conversion cost was not accounted for anywhere in the original plan's effort estimates and must be treated as in-scope for every "simple" and "complex" loader migration step (4, 5, 6), not just a design nicety.

### Implementation

1. Define the class in `Assets/Ashfall.Core/Data/CatalogLoader.cs` (this is a **new** directory — no `Data/` subfolder exists in `Assets/Ashfall.Core/` today):

```csharp
namespace Ashfall.Core.Data;

public abstract class CatalogLoader<T>
{
    protected readonly string DataDirectory;
    protected readonly IFileIO Files;
    protected readonly IJsonSerializer Json;
    protected readonly ILog Log;

    protected CatalogLoader(string dataDirectory, IFileIO files, IJsonSerializer json, ILog log)
    {
        DataDirectory = dataDirectory;
        Files = files;
        Json = json;
        Log = log;
    }

    /// <summary>Template method — orchestrates the full load pipeline.</summary>
    public IReadOnlyList<T> Load()
    {
        var path = ResolvePath();
        if (!Files.FileExists(path))
        {
            Log.Warn($"[{GetType().Name}] File not found: {path}");
            return Array.Empty<T>();
        }

        string raw = ReadContent(path);
        if (string.IsNullOrWhiteSpace(raw))
            return Array.Empty<T>();

        if (!ValidateSchemaVersion(raw))
            return Array.Empty<T>();

        var items = Deserialize(raw);
        return PostProcess(items);
    }

    // --- Extension points (override in subclasses) ---

    /// <summary>Returns the canonical file path for this catalog.</summary>
    protected abstract string ResolvePath();

    /// <summary>Deserializes raw JSON into a collection of T.</summary>
    protected abstract IReadOnlyList<T> Deserialize(string json);

    /// <summary>Optional post-processing (filtering, cross-refs). Default: pass-through.</summary>
    protected virtual IReadOnlyList<T> PostProcess(IReadOnlyList<T> items) => items;

    /// <summary>Read file content. Override for multi-file loaders.</summary>
    protected virtual string ReadContent(string path) => Files.ReadAllText(path);

    /// <summary>Schema version gate. Default: warn if missing, allow load.</summary>
    protected virtual bool ValidateSchemaVersion(string raw) { /* ... */ }
}
```

Note the constructor takes `dataDirectory` directly — this is a deliberate design choice for the *new* base class, not a claim that it matches existing code. Migrating `DutyRosterCatalogLoader`/`HoldfastCatalogLoader` to this shape means changing their call sites from `new DutyRosterCatalogLoader(files, json).Load(dataDirectory)` to `new DutyRosterCatalogLoader(dataDirectory, files, json).Load()` — a small but real call-site change to audit for every caller.

2. **This base class only fits the "returns a collection of T from one file" shape.** It does not accommodate:
   - `WarlordDoctrineCatalogLoader` (throws `InvalidOperationException` on error instead of returning empty) — either normalize its error handling as part of migration (a behavior change requiring its own test coverage and sign-off, not a transparent refactor), or explicitly exclude it and document why.
   - `VerdictCatalogLoader` (multiple parallel `Load*` methods, e.g. `LoadLocations(...)`, for different data slices) — likely needs either multiple `CatalogLoader<T>` instances (one per slice) or exclusion from this migration entirely.
   - `VerdictQuestCatalogLoader`/`VerdictNpcCatalogLoader` (`LoadAndRegister(system, ...)` — mutates a passed-in system, doesn't return a catalog) — fundamentally different shape; do not force these into `CatalogLoader<T>`, exclude them from this batch and note them as a separate future concern.
3. Define a companion `MultiFileCatalogLoader<T>` for directory-scanning loaders. **This requires adding a directory-enumeration method to `IFileIO` first** (e.g. `IEnumerable<string> GetFiles(string directory, string searchPattern)`), since the port has no such method today (confirmed: `Ports.cs:19-24` defines only `DirectoryExists`, `FileExists`, `ReadAllText`, `WriteAllText`, `Combine`). This is a **port change**, not a Core-internal addition — it requires updating every `IFileIO` implementer, at minimum Core's own default `FileSystemIO` adapter in `Assets/Ashfall.Core/HostDefaults.cs` (which the Godot host consumes as-is — there is no separate Godot-specific adapter to update), and should be scoped/reviewed as a distinct sub-task with its own test coverage before `MultiFileCatalogLoader<T>` can be built on top of it. Note `CrossingCatalogLoader` is already a genuine multi-file loader today (five files, hardcoded filenames, no globbing) and is a good real-world validation case for whatever `MultiFileCatalogLoader<T>` design emerges, alongside `DoseContentCatalogLoader`.
4. Document the contract: every subclass must implement `ResolvePath()` and `Deserialize()`.
5. Ensure no `UnityEngine.*` or `Godot.*` references — pure `Ashfall.Core`.

### Risk / Rollback

Low for the design itself (new, uncoupled file), but flag the two real risks surfaced above for review before implementation proceeds: (1) the `IFileIO.GetFiles` port addition affects every `IFileIO` implementer and must be treated as a breaking-surface change to the port contract, reviewed independently of the loader refactor — though the blast radius is narrower than it first appears, since Core's own `FileSystemIO` (`HostDefaults.cs`) is the only production implementer and the Godot host does not maintain a separate one; (2) converting 21 `static class` loaders to instance classes changes every call site that currently calls them statically — this is a mechanical but wide-reaching change across whatever code constructs catalogs, and each call-site change needs its own compile-and-test verification, not just the loader file itself.

### Verification

- Design review: base class compiles in isolation with no engine references.
- Template method covers all shared steps identified in Step 1 for the loaders it's actually scoped to cover (the static-`Load`-returning-`IReadOnlyList<T>` shape) — explicitly does NOT claim to cover `WarlordDoctrineCatalogLoader`, `VerdictCatalogLoader`, or the two `LoadAndRegister` loaders without a documented plan for each.
- Extension points accommodate the loader tiers actually found in Step 1's audit, not an assumed uniform set.

### Done when

`CatalogLoader<T>` is defined, compiles cleanly in the Core project, the design is documented in code comments, and there is an explicit, written decision (not a silent omission) for each of the 4 divergent loaders identified above: migrate-with-behavior-change, exclude-and-document, or defer-to-future-batch.

---

## Step 3 — Implement Base Class with File Resolution, Deserialization, Schema Validation, and Error Logging

### Goal

Complete the production implementation of `CatalogLoader<T>` with robust error handling, `schema_version` validation, and structured logging.

### Implementation

1. Implement `ValidateSchemaVersion`:
   - Parse the root JSON object looking for `schema_version` field.
   - If missing: log warning, allow load (backward compat).
   - If present but below minimum: log error, return false.
   - Use `IJsonSerializer` for partial parse (avoid double-deserialize where possible).
2. Wrap `Deserialize` call in try/catch:
   - On `JsonException` or `FormatException`: log error with file path and exception message, return empty.
   - Never use bare `catch {}` — always log.
3. Implement `MultiFileCatalogLoader<T>`:
   - `ResolveDirectory()` instead of `ResolvePath()`.
   - Iterates files via `IFileIO.GetFiles(directory, "*.json")` — **this method does not exist on `IFileIO` yet** (confirmed: `Ports.cs:19-24` has no directory-enumeration method). Add it to the port first (`IEnumerable<string> GetFiles(string directory, string searchPattern)`), implement it on Core's own `FileSystemIO` adapter (`Assets/Ashfall.Core/HostDefaults.cs` — reused as-is by the Godot host, no separate Godot-side class exists), and add dedicated tests for the new port method before building `MultiFileCatalogLoader<T>` on top of it. Treat this as its own reviewable sub-step, not a one-line implementation detail.
   - Aggregates results from per-file `Deserialize`.
4. Add unit tests in `Ashfall.Core.Tests/Data/CatalogLoaderBaseTests.cs`:
   - Test missing file returns empty.
   - Test malformed JSON returns empty with log call.
   - Test schema_version below minimum rejects.
   - Test valid file deserializes correctly.
   - Test the new `IFileIO.GetFiles` port method against both the real `FileSystemIO` adapter and a test double, confirming it returns exactly the files matching the pattern (not more, not fewer) in a temp directory fixture.

### Risk / Rollback

Medium — the `IFileIO.GetFiles` addition is a port-contract change with a blast radius beyond this batch: any other code that later implements `IFileIO` (test doubles included) must add this method or fail to compile. Land the port addition as its own commit, separate from `CatalogLoader<T>`'s implementation, so a problem with the port change doesn't block or get conflated with the base-class work. Rollback: since no production loader currently calls `GetFiles` (confirmed — no existing multi-file loader uses directory globbing today, they all hardcode filenames), reverting the port addition has zero impact on any existing loader's behavior.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # compiles
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "CatalogLoaderBase"  # all pass
```

### Done when

Base class is fully implemented, 4+ unit tests pass, and no engine references exist in the file.

---

## Step 4 — Migrate 5 Simple Loaders to Inherit from Base

### Goal

Convert the five simplest loaders (single-file, direct deserialize, no post-processing) to inherit from `CatalogLoader<T>`, proving the pattern works end-to-end.

### Implementation

Target loaders (simple tier):
1. `GoodsCatalogLoader` — currently a **static class** with `static Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`Economy/GoodsCatalog.cs:91`). No instance constructor exists.
2. `SurvivorCatalogLoader` — currently a **static class** with `static Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`Survivors/SurvivorCatalog.cs:207`). No instance constructor exists.
3. `DutyRosterCatalogLoader` — the **only one of these 5 that already has an instance constructor**: `(IFileIO files, IJsonSerializer json, ILog log = null)` (`DutyRoster/DutyRosterCatalog.cs:129-134`), with `dataDirectory` deferred to a separate `Load(string dataDirectory)` method, not the constructor.
4. `DoseContentCatalogLoader` — currently a **static class** with `static Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`DoseContentCatalog.cs:85`). Also has 2 bare `catch {}` blocks (lines 113, 132) to fix as part of this migration, not deferred to Batch 53.
5. `DoseRegistersCatalogLoader` — currently a **static class** with `static Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`DoseRegistersCatalog.cs:59`). No instance constructor exists.

**Corrected per-loader migration steps** — 4 of these 5 require converting from `static class` to instance class, which is a bigger change than "replace constructor body":

For `GoodsCatalogLoader`, `SurvivorCatalogLoader`, `DoseContentCatalogLoader`, `DoseRegistersCatalogLoader` (static → instance conversion):
1. Change class declaration from `public static class XxxCatalogLoader` to `public class XxxCatalogLoader : CatalogLoader<XxxDefinition>`.
2. Add an instance constructor calling `base(dataDirectory, files, json, log)`.
3. Implement `ResolvePath()` — return the same file path the old static `Load` method used to combine (verify the exact filename constant from the existing code before assuming it, e.g. `"goods.json"`).
4. Implement `Deserialize(string json)` — move the old static method's deserialization logic into this override.
5. Remove duplicated file-exists checks, try/catch, empty-return boilerplate from the old static method, then delete the old static `Load` method entirely.
6. **Find and update every call site that currently calls `XxxCatalogLoader.Load(dataDir, fileIO, json)` statically** — change each to `new XxxCatalogLoader(dataDir, fileIO, json).Load()`. Grep for `XxxCatalogLoader.Load(` across `src/` and `Assets/Ashfall.Core/` to find every caller before starting; do not assume there is only one call site.
7. Run existing tests for the loader to confirm behavior is unchanged.

For `DutyRosterCatalogLoader` (already an instance class, smaller change):
1. Change class declaration to inherit `CatalogLoader<DutyRosterDefinition>` (or the correct element type — verify from the existing `DutyRosterCatalog` return shape).
2. Change the constructor signature from `(IFileIO files, IJsonSerializer json, ILog log = null)` to `(string dataDirectory, IFileIO files, IJsonSerializer json, ILog log = null)`, calling `base(...)`.
3. Implement `ResolvePath()` using the `dataDirectory` now available at construction time (previously it was a `Load(string dataDirectory)` parameter — moving it to the constructor changes the call pattern for every caller).
4. Fold the existing `Load(string dataDirectory)` method's logic into `Deserialize()`.
5. **Update every call site** — this loader's constructor signature is changing (`dataDirectory` moves from `Load()` to the constructor), so `new DutyRosterCatalogLoader(files, json).Load(dataDirectory)` becomes `new DutyRosterCatalogLoader(dataDirectory, files, json).Load()` everywhere it's called. Grep for `new DutyRosterCatalogLoader(` to find every construction site.
6. Run existing tests to confirm behavior is unchanged.

### Risk / Rollback

Medium, not "Low" as the batch header's blanket risk rating implies — 4 of these 5 loaders require a static-to-instance class conversion that changes their call signature at every call site, not just their internal implementation. A missed call site is a compile error (safe — caught immediately), but a call site that's updated incorrectly (e.g. wrong argument order after the signature change) is a silent behavior change that only existing tests would catch. Migrate and verify one loader at a time, in its own commit, rather than batching all 5 together — if `dotnet test` regresses after one loader's migration, it's immediately attributable. Rollback: since `dotnet build` fails loudly on any missed/incorrect call site (both patterns above change the call signature), a broken migration is caught at compile time, not silently shipped; revert the single loader's commit if its migration doesn't compile or its tests regress.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done when

All five loaders that are actually compatible with `CatalogLoader<T>`'s shape (all 5, per the corrected per-loader analysis above) inherit from it, existing tests pass, data integrity selftest reports 0 errors, every static-to-instance call-site update compiles and is verified, and each loader file is reduced in line count (target 50%+ is aspirational — do not treat it as a hard gate; report the actual reduction achieved, since the static-to-instance conversion for 4 of the 5 loaders adds a constructor and removes a static-method wrapper, which may reduce line count less than a pure "delete boilerplate" refactor would).

---

## Step 5 — Migrate Complex Loaders with Custom Parsing

### Goal

Convert loaders that have non-trivial parsing (multi-file, conditional logic, cross-references, nested structures) to use the base class, leveraging override points.

### Implementation

**Corrected target list, given Step 2's findings on structural fit:**

1. `NarrativeEncounterCatalogLoader` — multi-file directory scan, nested encounter structure. Currently a **static class** with `static Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` (`Narrative/NarrativeEncounterSystem.cs:247`) — requires the same static-to-instance conversion as Step 4's static-tier loaders, plus depends on the new `IFileIO.GetFiles` port method from Step 3 if it truly directory-scans (verify this — "multi-file directory scan" was asserted in the original plan but not independently confirmed during this review; check the actual implementation before assuming it needs `MultiFileCatalogLoader<T>`).
2. `HoldfastCatalogLoader` — expansion-gated loading, conditional file paths. This is one of the **4 loaders that already has an instance constructor** (`(IFileIO files, IJsonSerializer json, ILog log = null)`, `HoldfastCatalog.cs:133-138`), but its `Load(string dataDirectory, bool expansionUnlocked = true)` method (line 143) has an **extra `bool expansionUnlocked` parameter that no other loader has and that `CatalogLoader<T>.Load()`'s parameterless signature cannot represent**. Migrating this loader requires either extending the base class's `Load()` signature to accept optional loader-specific parameters (weakening the "one uniform template method" premise) or keeping `HoldfastCatalogLoader` as a documented exception that overrides `Load()` entirely rather than relying on the inherited template method. Decide explicitly which approach before starting.
3. `CrossingQuestCatalogLoader` — quest chain resolution, cross-references between quests. Confirmed by direct inspection: `public static class CrossingQuestCatalogLoader` with `static List<CrossingQuestDef> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? serializer = null)` (`Crossing/CrossingQuestSystem.cs:354,358`) — static tier, per the 21/25 majority pattern, requiring the same static-to-instance conversion as Step 4. Note the nullable reference annotations (`IFileIO?`, `IJsonSerializer?`) with default `null` values are unique to this loader among the ones sampled — no other loader's `Load` signature defaults its ports to null; decide whether the base class's `Deserialize`/constructor should reject null ports (as most loaders implicitly do via `ArgumentNullException` in the instance-class tier) or preserve this loader's more permissive null-tolerant behavior, since silently changing it either way is a behavior change worth flagging in review. Do not confuse this with `CrossingCatalogLoader` (`CrossingCatalog.cs`) — a different, unrelated instance-class loader for a different data domain despite the similar name.
4. `StandingRecordCatalogLoader` — record linkage, ID cross-validation. Confirmed by direct inspection: `public sealed class StandingRecordCatalogLoader` with constructor `(IFileIO files, IJsonSerializer json, ILog log = null)` and separate `Load(string dataDirectory)` method (`StandingRecord/StandingRecordCatalog.cs:56,64,71`) — this is one of the **4 instance-class loaders** (see corrected Context inventory above), not a pattern needing separate verification; its shape matches `DutyRosterCatalogLoader` exactly (single file, no `expansionUnlocked`-style extra parameter), so it should migrate the same way Step 4 handled `DutyRosterCatalogLoader`.
5. **`VerdictCatalogLoader` is REMOVED from this tier's target list.** Per Step 2's audit, it is not one `Load` method but several parallel static methods (e.g. `LoadLocations(...)`) for different data slices, and per this review's direct code inspection it has **4** exception-swallowing `catch` blocks (lines 51, 105, 141 as `catch { return result; }`, and line 166 as a truly empty `catch { }`) — not "1 bare catch" as an earlier pass of this document claimed, and not "already addressed in Batch 53" as the original plan claimed (this review found no evidence the fix was applied to any of the four; all are still present at HEAD). This loader either needs a fundamentally different composition-based approach (e.g. one `CatalogLoader<T>` instance per data slice, each wrapping one of `VerdictCatalogLoader`'s existing methods) or should be explicitly deferred to a follow-up batch. Do not silently migrate it using the same steps as the other 4 — its shape doesn't fit.

For the 4 remaining loaders (`NarrativeEncounterCatalogLoader`, `HoldfastCatalogLoader`, `CrossingQuestCatalogLoader`, `StandingRecordCatalogLoader`):
1. Determine if it fits `CatalogLoader<T>` or `MultiFileCatalogLoader<T>` — verify against its actual current implementation, not an assumption.
2. Move custom parsing into `Deserialize()` override.
3. Move cross-reference/validation into `PostProcess()` override.
4. For multi-file loaders: override `ResolveDirectory()` and per-file hooks (requires Step 3's `IFileIO.GetFiles` port addition to exist first).
5. Ensure the migrated loader passes all existing tests without behavior change.
6. Update all call sites per the static-to-instance or constructor-signature-change patterns established in Step 4, as applicable to each loader's actual current shape.

### Risk / Rollback

Medium — same call-site risk as Step 4, plus the added risk that `HoldfastCatalogLoader`'s `expansionUnlocked` parameter and `VerdictCatalogLoader`'s multi-method shape may force a base-class design change mid-migration if Step 2 didn't already resolve how to handle them. If either surfaces a design gap not caught in Step 2, stop and revise the base class design rather than hacking around it in the subclass — a hacky per-subclass workaround defeats the DRY purpose of this whole batch.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
```

### Done when

All 4 migrated loaders (`NarrativeEncounterCatalogLoader`, `HoldfastCatalogLoader`, `CrossingQuestCatalogLoader`, `StandingRecordCatalogLoader`) inherit from the base or a documented composition-based variant, custom logic lives in well-named overrides, and all tests pass. `VerdictCatalogLoader`'s disposition (migrate-with-redesign, exclude, or defer) is explicitly recorded — not silently skipped — along with the true current state of its exception-swallowing blocks (4 occurrences, lines 51, 105, 141, 166, all unresolved, not previously fixed in Batch 53 as originally claimed).

---

## Step 6 — Migrate Remaining Loaders

### Goal

Complete the migration so that every remaining catalog loader in the project inherits from `CatalogLoader<T>` or `MultiFileCatalogLoader<T>`, or has an explicitly documented exception.

### Implementation

**Corrected remaining list** — the original plan's "Remaining 10+ Loaders" omitted `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`, `DoorEncounterCatalogLoader`, and `YearOfAshCatalogLoader` entirely (all 4 are real loaders confirmed in Step 1's corrected inventory). Full remaining list after Steps 4 and 5:

1. `CombatCatalogLoader`
2. `CrossingCatalogLoader`
3. `DiseaseCatalogLoader`
4. `SilentFoundryCatalogLoader`
5. `SilentFoundryConsequenceCatalogLoader`
6. `DeepLoreLocationCatalogLoader`
7. `DiveSiteCatalogLoader`
8. `CurrentsCatalogLoader`
9. `WitnessCatalogLoader`
10. `UtilityActionCatalogLoader`
11. `WarlordDoctrineCatalogLoader` — **flag specifically**: this loader throws `InvalidOperationException` on missing/empty/malformed data (confirmed, `Warlords/WarlordDoctrineCatalog.cs`), unlike every other loader's "return empty" convention. Per Step 2's design note, decide explicitly whether to normalize this behavior (a real behavior change requiring dedicated test coverage for callers that may depend on the throw) or keep it as a documented exception that overrides the base class's default error handling.
12. `YearOfAshCatalogLoader` (`YearOfAsh/YearOfAshCatalogLoader.cs:124`) — **newly added to scope**; has 10 confirmed exception-swallowing `catch` blocks (5 empty `catch {}` at lines 152, 183, 255, 263, 294, plus 5 paired `catch { return new List<...>(); }` at lines 158, 189, 269, 300, 331 — not "5" or "7" as variously cited elsewhere in project docs) that must be fixed as part of this migration.
13. `DoorEncounterCatalogLoader` (`YearOfAsh/DoorEncounterCatalogLoader.cs:16`) — **newly added to scope**; verify its construction pattern and bare-catch status before migrating.
14. `VerdictQuestCatalogLoader` (`Verdict/VerdictQuestCatalogLoader.cs:14`) — **newly added to scope**, but per its `LoadAndRegister(QuestlineSystem system, ...)` shape (mutates a passed system, doesn't return a catalog), this loader likely does **not** fit `CatalogLoader<T>` at all — treat as a documented exclusion like `VerdictCatalogLoader`'s `LoadAndRegister` sibling, not a migration target, unless a redesign is explicitly agreed first.
15. `VerdictNpcCatalogLoader` (`Verdict/VerdictNpcSystem.cs:112`) — **newly added to scope**, same `LoadAndRegister(VerdictNpcSystem system, ...)` shape and same likely-exclusion caveat as above.
16. Any other loaders discovered during audit (Step 1) not already covered.

For each loader that fits the base class (items 1-10, 12-13 above, pending verification):
1. Classify as simple or moderate based on Step 1 audit.
2. Apply the same migration pattern established in Steps 4/5: convert static-to-instance if needed, implement `ResolvePath`/`Deserialize`, override `PostProcess` if needed, update every call site.
3. Remove all duplicated boilerplate.
4. Run tests after each batch of 3-4 migrations (fail fast).

For the loaders that likely don't fit (`WarlordDoctrineCatalogLoader`'s throw behavior, `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`'s registration shape): make an explicit, documented decision per Step 2's design note rather than forcing a fit or silently excluding them without a record.

### Risk / Rollback

Medium — same call-site risk as Steps 4-5, scaled across more files. Continue the one-loader-per-commit discipline established in Step 4 so any regression is immediately attributable and revertible in isolation.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

### Done when

Every one of the 25 loaders identified in Step 1 has an explicit, documented disposition: migrated to `CatalogLoader<T>`/`MultiFileCatalogLoader<T>`, migrated with a documented behavior change (e.g. `WarlordDoctrineCatalogLoader`'s throw-vs-empty decision), or explicitly excluded with a written reason (e.g. `VerdictCatalogLoader`, `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader` if their shape is confirmed incompatible). "Zero loaders remain that duplicate the pipeline manually" is the goal for loaders that fit the base class — it is not achievable for loaders explicitly excluded by design, and this step's Done-when must not claim 100% migration if Step 2/5's audit already identified structural non-fits. Full test suite green for whatever subset was actually migrated.

---

## Step 7 — Remove Duplicated Code, Final Cleanup, Verify All Tests Pass

### Goal

Delete all dead code left behind by the migration, ensure no orphaned helper methods remain, and confirm the entire project builds and passes all verification gates.

### Implementation

1. Search for any standalone helper methods that were only used by the old loader pattern (e.g., `TryLoadFile`, `SafeDeserialize` utility methods duplicated across loaders).
2. Remove unused `using` directives from migrated files.
3. Verify no loader still has inline file-exists/try-catch/return-empty boilerplate — except the explicitly-documented exclusions from Steps 5-6.
4. Update `CatalogIntegrityValidator` if it references loader internals that changed. Note: `CatalogIntegrityValidator.cs` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`) is 657 lines (not the 603 cited elsewhere in project docs — that figure is stale) and is the project's central data-validation gate backing `--data-integrity-selftest`; treat any change here as touching shared, high-blast-radius infrastructure, and verify it in its own commit separate from loader migrations.
5. Run the full 5-step verification checklist.
6. Measure line-count reduction across all migrated loaders (report the actual number achieved — do not target a specific percentage as a pass/fail gate; the static-to-instance conversion required for 21 of 25 loaders adds constructor/base-call boilerplate that a pure "delete duplicate code" refactor wouldn't, so actual reduction may reasonably land below any number picked in advance).
7. Document the pattern in a code comment on `CatalogLoader<T>` for future contributors, **explicitly listing which loaders were excluded and why** (this is the most valuable documentation for future contributors, since the "identical signature" assumption that motivated this whole batch turned out to be false for the majority of loaders).

### Risk / Rollback

Low for this step itself (cleanup only), but this is the step where an incomplete Step 4-6 migration becomes visible — if call-site updates were missed in earlier steps, this step's full verification checklist is what catches it. Do not skip straight to this step's line-count/doc-comment tasks before confirming the full checklist passes; a "cleanup" commit that also happens to fix a missed call site from 3 steps ago makes the regression much harder to bisect later.

### Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # Must compile cleanly
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass
dotnet build Ashfall.csproj                                  # Godot host: 0 errors, 0 warnings
godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
godot --headless --path . -- --bridge-selftest               # Exits 0
```

### Done when

- All 25 loaders (confirmed count, not "20+") have an explicit, documented disposition per the Step 6 correction above — migrated, migrated-with-behavior-change, or excluded-with-reason.
- No duplicated load-validate-deserialize boilerplate remains among the loaders that were migrated.
- Line count reduction across migrated loader files is measured and reported (not gated on a specific pre-committed percentage — see the Implementation note above on why the static-to-instance conversion overhead makes an advance target unreliable).
- Full verification checklist passes.
- Base class has clear documentation for future loader authors, including the explicit exclusion list.

------

## Summary Table

| Step | Description | Tier | Est. Effort | Key Risk |
|------|-------------|------|-------------|----------|
| 1 | Audit all loaders — classify shared/divergent patterns | Analysis | 2-3 hours | Undiscovered loader variants (this review already found 2 previously-missed instance-class loaders during a supposedly-complete prior audit, so budget time for a genuinely exhaustive pass, not a sampled one) |
| 2 | Design `CatalogLoader<T>` generic base class | Design | 3-4 hours (up from 2-3 — the corrected Step 1 findings mean the design must explicitly account for 4 non-fitting loaders and a port change before any code is written) | Over-abstraction / insufficient extension points; base class design does not actually fit the majority pattern without the static→instance conversion decision made explicitly |
| 3 | Implement base class with validation, logging, tests | Implementation | 3-4 hours + 2-3 hours for the `IFileIO.GetFiles` port addition and its own tests (tracked as a separate reviewable sub-step per Step 3's Risk/Rollback note) | Schema version backward compat; port-contract breaking change blast radius |
| 4 | Migrate 5 simple loaders | Migration | 3-4 hours (up from 2-3 — 4 of the 5 require static→instance conversion plus a call-site audit, not just a constructor swap) | Caller signature changes; missed call sites are compile errors (safe) but incorrectly-updated call sites are silent behavior changes |
| 5 | Migrate 4 complex loaders (`VerdictCatalogLoader` removed from this tier — see Step 5 correction) | Migration | 4-5 hours | Custom parsing doesn't fit template; `HoldfastCatalogLoader`'s `expansionUnlocked` parameter has no home in the base class's parameterless `Load()` |
| 6 | Migrate remaining 16 loaders (25 total − 5 in Step 4 − 4 in Step 5 = 16, including the 3 loaders documented as likely-exclusions: `WarlordDoctrineCatalogLoader`, `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`, plus `VerdictCatalogLoader` itself deferred from Step 5) | Migration | 5-7 hours (up from 4-5 — "10+" undercounted the real remaining list, and 3-4 of these loaders need an explicit exclusion decision rather than a migration, which is its own deliverable) | Undiscovered edge cases; loaders that don't fit `CatalogLoader<T>` at all and need a documented, reviewed exclusion rather than a silent skip |
| 7 | Final cleanup, dead code removal, verification | Cleanup | 2-3 hours | Missed orphaned code; this step is also where an incomplete Steps 4-6 migration becomes visible via the full verification checklist |

**Total estimated effort:** 24-33 hours (up from the original 19-26 — the increase reflects the port-change sub-task, the static-to-instance call-site audits required for 21 of 25 loaders, and the explicit-exclusion documentation work for the loaders that don't fit the base class, none of which were accounted for in the original estimate).
**Net outcome:** Loaders that fit the base class's shape (a subset of the 25, not "20+" uniformly) are reduced to thin subclasses; new cross-cutting concerns (retry, metrics, schema gates) are added once in the base class for that subset; the loaders that don't fit (`WarlordDoctrineCatalogLoader`, `VerdictCatalogLoader`, `VerdictQuestCatalogLoader`, `VerdictNpcCatalogLoader`) get an explicit, documented disposition rather than being silently forced into an ill-fitting shape or silently skipped.


---

## Review Notes (Corrected)

This batch had already gone through one adversarial review pass before this one (visible in the inline "corrected"/"critical correction" annotations throughout the document). This second pass verified those prior corrections against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and found that the prior pass, while broadly careful, itself contained factual errors. Findings from this second pass:

1. **The prior "only 2 instance-class loaders" claim was itself wrong — the real count is 4.** The prior review checked 10 of 25 loaders' construction patterns and concluded only `DutyRosterCatalogLoader` and `HoldfastCatalogLoader` were instance classes. Direct inspection of all 25 loader files in this pass found two more: `CrossingCatalogLoader` (`CrossingCatalog.cs:144,163`) and `StandingRecordCatalogLoader` (`StandingRecord/StandingRecordCatalog.cs:56,64`), both `sealed class`es with the exact same `(IFileIO files, IJsonSerializer json, ILog log = null)` constructor + separate `Load(string dataDirectory)` method shape. This means the static-tier count is 21, not 23, and every downstream reference to "23 static loaders" / "2 instance loaders" in Steps 2, 3, 4, and 5 has been corrected to 21/4 in place. This is significant because Step 5 already listed `StandingRecordCatalogLoader` as a migration target without recognizing it was actually in the *easier* instance-class tier rather than needing "verification first" as originally written — it should have been grouped with Step 4's simpler conversions, or at minimum flagged as already-known-instance rather than unverified.
2. **Bare-catch / exception-swallowing counts were undercounted for both `VerdictCatalogLoader` and `YearOfAshCatalogLoader`.** The prior pass reported `VerdictCatalogLoader` has "1 bare `catch {}`" (line 166) — direct inspection found 4 exception-swallowing catch blocks: lines 51, 105, 141 (`catch { return result; }`) plus line 166 (`catch { }`). All four discard the caught exception without logging; the prior pass's narrower definition of "bare" (literally empty body) missed 3 of the 4 real defects. Likewise, `YearOfAshCatalogLoader` was reported as having "5" bare catches (lines 152, 183, 255, 263, 294) — correct as far as it goes, but the file has 5 more `catch { return new List<...>(); }` blocks (lines 158, 189, 269, 300, 331) that are functionally identical exception-swallowing sites, paired one-for-one with the 5 already counted as a two-tier fallback pattern. Real total: 10, not 5. `DoseContentCatalog.cs` was undercounted by one as well (3 sites — line 101 in addition to 113, 132). Every catch-count reference in this document has been corrected in place, and the underlying lesson — count "discards the exception without logging" as the defect, not "literally empty catch body" — is now stated explicitly in the Context section.
3. **`FileSystemIO` was mischaracterized throughout as "the Godot host's `FileSystemIO` adapter."** It is not — `Assets/Ashfall.Core/HostDefaults.cs` defines `FileSystemIO` directly inside `Ashfall.Core`, as Core's own default BCL-backed `IFileIO` implementation. It is used by `Ashfall.Core.Tests` directly and reused as-is by the Godot host; there is no separate Godot-specific `IFileIO` adapter class in the codebase to distinguish from it. This actually *narrows* the port-change blast radius the document worried about (Steps 1 and 3's proposed `IFileIO.GetFiles` addition): there's one production implementer to update, not "the Core port plus a separate Godot adapter." All references to this have been corrected in place to say Core's own adapter, reused by the Godot host, rather than implying a distinct Godot-side class exists.
4. **What held up under this pass:** the corrected 25-loader count (verified independently via the same grep the prior pass used, plus manual confirmation of each class declaration and constructor), the confirmed absence of `IFileIO.GetFiles`/`FlushToDisk`/`MoveReplace` (`Ports.cs` has exactly the 5 methods cited, `DirectoryExists`/`FileExists`/`ReadAllText`/`WriteAllText`/`Combine`), `WarlordDoctrineCatalogLoader`'s throw-instead-of-empty behavior (confirmed — six distinct `InvalidOperationException` throw sites, more even than the prior pass's general description implied, now itemized), and the four newly-discovered loaders (`VerdictNpcCatalogLoader`, `VerdictQuestCatalogLoader`, `DoorEncounterCatalogLoader`, `YearOfAshCatalogLoader`) all held up on direct inspection.
5. **Risk rating and effort estimate corrected.** The document's top-level header still said "Risk: Low" and estimated 19-26 hours despite the body text's own findings (port change, 21-of-25 static→instance conversions, 4 structurally-non-fitting loaders) clearly describing a Medium-risk, higher-effort undertaking. Both have been revised in place — Risk to Medium with an explicit rationale, effort to 24-33 hours — so the document's summary framing matches what its own analysis concluded.
6. **Ordering issue flagged (not previously caught):** Step 3 implements `MultiFileCatalogLoader<T>` depending on the new `IFileIO.GetFiles` port method "before it can work," and Step 3's own Risk/Rollback note says to land the port addition as its own separate commit — but Step 2 (design) is where the port gap is first identified, and neither step explicitly says who decides *when* the port-addition commit lands relative to the rest of Step 3's work. Recommendation for the implementer: land and merge the `IFileIO.GetFiles` port change (with its own tests, per Step 3) as a complete, separate, reviewed unit before starting `MultiFileCatalogLoader<T>`'s implementation, not in parallel — this avoids a half-landed port change blocking or getting tangled with base-class work if the port review surfaces issues.
