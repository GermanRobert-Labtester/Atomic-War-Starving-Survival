# ASHFALL — Quality Roadmap Batch 98

## Theme: Hot Reload & Live Tuning — Runtime Data Refresh Without Restart

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM |
| **Risk** | Medium — must handle state consistency during reload without corrupting running systems |
| **Blocking** | No — development velocity improvement, not a correctness issue |
| **Depends on** | Catalog loaders in Core (`Assets/Ashfall.Core/`), Godot host file access, `IFileIO` port |
| **Estimated effort** | 5–7 task blocks — **[FLAGGED, see Review Notes]** this estimate assumes a uniform catalog-loader architecture that does not exist; the real effort is materially higher once loader heterogeneity (below) is accounted for |

---

## Problem Statement

The data authority (`Assets/StreamingAssets/Data/`) contains **296** JSON files (verified: `find Assets/StreamingAssets/Data -iname "*.json" | wc -l`), including **196** narrative files (verified) — the "130+ JSON files" figure in earlier drafts undercounted; 296 is the correct current count. These are loaded **once** at startup by catalog loaders. Any change — tweaking an item stat, adjusting an encounter probability, fixing a typo in dialogue — requires a full game restart to see the effect.

For a project with this volume of data-driven content, the restart cycle is a severe iteration bottleneck:

- Edit `items.json` to adjust radiation resistance on a hazmat suit → restart (8–12 seconds)
- Edit `encounters.json` to tweak probability weights → restart
- Edit narrative dialogue → restart
- Edit recipe ingredients → restart

**Hot reload** enables: edit JSON → press F5 → see changes immediately in the running game.

### Challenge

Reloading catalogs at runtime must not corrupt live game state:
- An item referenced by a survivor's inventory must not vanish if its definition is temporarily removed
- A quest in progress must not break if its encounter chain is modified
- Invalid JSON must log an error, not crash the game

### [CORRECTED] Loader architecture reality check — this changes the plan's scope

This document (and the codebase it describes) does **not** have a uniform "catalog loader" pattern. Verified counts:

- Only **5** files are literally named `*CatalogLoader.cs`: `YearOfAsh/DoorEncounterCatalogLoader.cs`, `YearOfAsh/YearOfAshCatalogLoader.cs`, `Maritime/DeepLoreLocationCatalogLoader.cs`, `Verdict/VerdictQuestCatalogLoader.cs`, `Verdict/VerdictCatalogLoader.cs`.
- Broadening to any file/class with "Catalog" in the name (the real population this plan needs to touch) gives **~101 files** under `Assets/Ashfall.Core/`, matching the wave-2 corrected estimate of roughly two dozen *loaders* once you filter out catalogs that are pure in-memory lookup tables with no file-loading responsibility of their own.
- Their load-entry-point signatures are **not uniform**. Confirmed variety, sampled directly from the source:
  - `GoodsCatalogLoader.Load(string dataDir, IFileIO fileIO, IJsonSerializer json)` → `GoodsCatalogLoadResult`
  - `CurrentsCatalogLoader.LoadCurrents(...)` → `List<CurrentDefinition>`
  - `WitnessCatalogLoader.LoadWitnesses(...)` → `List<WitnessDefinition>`
  - Roughly 20+ `Narrative/*Catalog.cs` classes each expose `LoadFromDirectory(string directoryPath)` returning their own concrete catalog type
  - `BunkerContrabandCatalog` exposes both `LoadFromJson(string json)` *and* `LoadFromFile(string path)`
  - `HardcoreEconomyTuningLoader.Load(string json)` → its own result type
  - `TradeScreenScenarios.LoadFromJson(string json)`, `TradeTellEngine.LoadFromJson(string json)` — different classes, different return shapes
- **The three classes this plan's Step 5 names to modify do not exist.** There is no `ItemsCatalog.cs`, `LocationsCatalog.cs`, or `EncountersCatalog.cs` anywhere in `Assets/Ashfall.Core/`. Items are defined via `Inventory/ItemDefinitions.cs` and `HoldfastItemsCatalog.cs`; there is no single unified item catalog class matching the `IReloadable`-friendly shape this plan assumes. There is no dedicated `LocationsCatalog` (`locations.json` exists as data, but the closest loader-named class is `Maritime/DeepLoreLocationCatalogLoader.cs`, a different, narrower thing). There is no unified `EncountersCatalog` (`Narrative/EncounterCatalog.cs` exists but is one of several encounter-related types alongside `NarrativeEncounterSystem.cs` and `YearOfAsh/DoorEncounterCatalogLoader.cs`).

**What this means for the plan:** `IReloadable.Reload(string rawJson, ILog log)` as a single retrofittable interface assumes every catalog already exposes (or can trivially be refactored to expose) a `(raw JSON in) → (parsed definitions out)` seam that is separable from *how* the file was read (`LoadFromDirectory` vs `LoadFromJson` vs `LoadFromFile` vs a custom `(dataDir, IFileIO, IJsonSerializer)` signature). That seam does not exist uniformly today. Step 5 (and the "5–7 task blocks" estimate) must be rescoped: either (a) pick genuinely representative, currently-shipping catalog classes by their real names before writing any adapter code, or (b) treat "retrofit `IReloadable` onto an existing catalog" as its own per-catalog task with its own effort estimate, because a `LoadFromDirectory`-based catalog (~20 of the Narrative catalogs) needs a materially different adapter than a `Load(dataDir, IFileIO, IJsonSerializer)`-based one (`GoodsCatalog`). This plan currently treats all catalogs as interchangeable, which is not true.

---

## Architecture Overview

```
┌─────────────────────┐      ┌───────────────────────┐
│  FileWatcher        │─────▶│  CatalogReloadOrch.   │
│  (Godot host,      │      │  (coordinates reload)  │
│   monitors Data/)   │      └──────────┬────────────┘
└─────────────────────┘                 │
                                        ▼
                            ┌───────────────────────┐
                            │  CatalogReloader      │
                            │  (re-parses JSON,     │
                            │   validates, swaps)    │
                            └──────────┬────────────┘
                                       │
                                       ▼
                            ┌───────────────────────┐
                            │  StateReconciler      │
                            │  (patches live state   │
                            │   without corruption)  │
                            └───────────────────────┘
```

**Layer placement:**

| Component | Layer | Why |
|-----------|-------|-----|
| `FileWatcher` | Godot host (`src/`) | Uses Godot `FileSystemWatcher` / OS polling |
| `CatalogReloadOrchestrator` | Core (`Assets/Ashfall.Core/`) | Engine-agnostic reload coordination |
| `CatalogReloader` | Core | Re-runs existing catalog loader logic |
| `StateReconciler` | Core | Reconciliation rules are game logic |
| `HotReloadHostBridge` | Godot host (`src/`) | Wires file events to orchestrator, triggers UI refresh |

---

## Step 1 — Design Hot Reload Architecture

### Goal

Define the reload contract, reconciliation rules, and safety guarantees before writing code. Establish what "safe reload" means for each catalog type.

### Implementation

**Design document: `docs/hot-reload-design.md`**

**Core reload contract (`Assets/Ashfall.Core/HotReload/IReloadable.cs`):**

```csharp
namespace Ashfall.Core.HotReload;

/// Implemented by any catalog that supports hot reload.
public interface IReloadable
{
    /// Unique catalog identifier (e.g., "items", "locations", "encounters").
    string CatalogId { get; }

    /// Re-parses the catalog from raw JSON. Returns validation result.
    ReloadResult Reload(string rawJson, ILog log);

    /// Returns IDs currently in use by live game state (cannot be removed).
    IReadOnlySet<string> GetActiveIds();
}
```

**Reconciliation rules (formalized):**

| Change type | Rule | Rationale |
|-------------|------|-----------|
| New item added | Add to catalog immediately | No state references it yet — safe |
| Existing item modified | Update definition in-place | Instances reference by ID; they get updated stats next access |
| Item removed from JSON | Mark `deprecated`, do NOT delete | Live state may reference it; deletion = null reference crash |
| Invalid JSON | Reject entire reload, keep previous | Partial catalog is worse than stale catalog |
| Schema version mismatch | Reject with warning | Loader expects specific schema; don't silently corrupt |
| ID renamed | Treat as: old removed (deprecated) + new added | Cannot safely rename IDs that state references |

**Reload result DTO:**

```csharp
public record ReloadResult
{
    public bool Success { get; init; }
    public int Added { get; init; }
    public int Modified { get; init; }
    public int Deprecated { get; init; }
    public int Rejected { get; init; }
    public string? ErrorMessage { get; init; }
    public TimeSpan Duration { get; init; }
}
```

**Safety invariants:**

1. A reload NEVER throws to the caller — all errors are captured in `ReloadResult`
2. A reload NEVER removes an ID that `GetActiveIds()` reports as in-use
3. A reload is atomic: either the entire new catalog is accepted or the old one is retained
4. A reload fires a `CatalogReloaded` event so UI can refresh bindings
5. Reload is disabled during save/load operations (race condition prevention)

### Verification

- Design document reviewed and approved
- `IReloadable` interface compiles in Core (no engine references)
- Reconciliation rules cover all 5 change types
- Safety invariants documented and testable

### Done-when

- [ ] `docs/hot-reload-design.md` written with full architecture
- [ ] `Assets/Ashfall.Core/HotReload/IReloadable.cs` created with interface + DTOs
- [ ] Reconciliation rules formalized in code comments
- [ ] Safety invariants enumerated and each mapped to a future test

---

## Step 2 — Implement FileWatcher in Godot Host

### Goal

Monitor `Assets/StreamingAssets/Data/` for `.json` file changes and emit events that the reload orchestrator can consume. Debounce rapid successive writes (common with text editors that write-rename-delete).

**[VERIFIED]** No file-watching capability exists in the Godot host today — confirmed by searching `src/` and `Assets/Ashfall.Core/` for `FileSystemWatcher` (zero hits). Every existing "reload" reference in this codebase (`Main.cs`, `HostCli.cs`) refers to save-file reload-after-write-for-round-trip-testing (e.g. `--holdfast-save-selftest`, `PressReload`), which is an unrelated, already-solved problem (save/load round-trips) — not live data hot reload. This step is genuinely new work with no existing code to build on, which the original document did not make explicit.

**[FLAGGED]** `System.IO.FileSystemWatcher` on Linux (the platform this repo is being developed/verified on per this session) is backed by inotify and has well-known reliability gaps that should be called out as a risk, not discovered later: it can silently drop events under high-volume writes (inotify queue overflow), it does not reliably fire `Changed` for atomic write-via-rename patterns used by some editors (write to temp file, rename over target — which looks like `Created` + `Deleted` + nothing for the target path, not `Changed`), and `IncludeSubdirectories` performance degrades with large directory trees. `Assets/StreamingAssets/Data/` has 296 JSON files across a `narrative/` subtree with 196 files — verify empirically during this step that `Changed`/`Renamed` actually fire for the editors the team uses (VS Code, JetBrains Rider, vim) before building the rest of the pipeline on top of it, and keep the F5 force-reload path (Step 6) as more than a "fallback" — treat it as the primary trigger until watcher reliability is empirically confirmed.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `src/HotReload/GodotFileWatcher.cs` | Monitors data directory, emits change events |
| `src/HotReload/FileChangeEvent.cs` | Event DTO: path, change type, timestamp |
| `src/HotReload/DebounceTimer.cs` | Coalesces rapid writes into single event |

**`GodotFileWatcher` design:**

```csharp
namespace AtomicWar.GodotApp.HotReload;

public partial class GodotFileWatcher : Node
{
    private FileSystemWatcher? _watcher;
    private readonly ConcurrentQueue<FileChangeEvent> _pendingChanges = new();
    private readonly DebounceTimer _debounce = new(debounceMs: 300);

    public event Action<FileChangeEvent>? OnFileChanged;

    private string _watchPath = "";

    public override void _Ready()
    {
        _watchPath = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data");
        StartWatching();
    }

    private void StartWatching()
    {
        _watcher = new FileSystemWatcher(_watchPath)
        {
            Filter = "*.json",
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        _watcher.Changed += (_, e) => EnqueueChange(e.FullPath, ChangeType.Modified);
        _watcher.Created += (_, e) => EnqueueChange(e.FullPath, ChangeType.Created);
        _watcher.Deleted += (_, e) => EnqueueChange(e.FullPath, ChangeType.Deleted);
        _watcher.Renamed += (_, e) => EnqueueChange(e.FullPath, ChangeType.Renamed);
    }

    private void EnqueueChange(string fullPath, ChangeType type)
    {
        _pendingChanges.Enqueue(new FileChangeEvent
        {
            FullPath = fullPath,
            RelativePath = Path.GetRelativePath(_watchPath, fullPath),
            Type = type,
            Timestamp = DateTime.UtcNow
        });
    }

    public override void _Process(double delta)
    {
        // Drain queue on main thread (Godot thread safety)
        while (_pendingChanges.TryDequeue(out var change))
        {
            _debounce.Register(change);
        }

        // Emit debounced events
        foreach (var ready in _debounce.GetReady())
        {
            OnFileChanged?.Invoke(ready);
        }
    }

    public override void _ExitTree()
    {
        _watcher?.Dispose();
        _watcher = null;
    }
}
```

**`DebounceTimer` design:**

```csharp
public class DebounceTimer
{
    private readonly int _debounceMs;
    private readonly Dictionary<string, (FileChangeEvent Event, DateTime LastSeen)> _pending = new();

    public DebounceTimer(int debounceMs) => _debounceMs = debounceMs;

    public void Register(FileChangeEvent e)
    {
        _pending[e.RelativePath] = (e, DateTime.UtcNow);
    }

    public IEnumerable<FileChangeEvent> GetReady()
    {
        var now = DateTime.UtcNow;
        var ready = _pending
            .Where(kv => (now - kv.Value.LastSeen).TotalMilliseconds >= _debounceMs)
            .Select(kv => kv.Value.Event)
            .ToList();

        foreach (var e in ready)
            _pending.Remove(e.RelativePath);

        return ready;
    }
}
```

### Verification

- FileWatcher compiles in `dotnet build Ashfall.csproj`
- Debounce timer has unit test in `Ashfall.Core.Tests` (timer logic is engine-agnostic)
- Manual test: edit a JSON file while game runs → event fires after 300ms debounce
- No events fire during save/load (watcher paused)

### Done-when

- [ ] `GodotFileWatcher` monitors `StreamingAssets/Data/` for JSON changes
- [ ] Debounce coalesces rapid writes (300ms window)
- [ ] Events drain on main thread (Godot thread safety)
- [ ] Watcher disposes cleanly on exit
- [ ] `DebounceTimer` unit-tested in Core tests

---

## Step 3 — Implement CatalogReloader (Re-parse on Change)

### Goal

When a file change event arrives, identify which catalog it belongs to, re-parse the JSON, validate it, and produce a new catalog version — without touching live state yet.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `Assets/Ashfall.Core/HotReload/CatalogReloadOrchestrator.cs` | Routes file changes to the correct reloadable catalog |
| `Assets/Ashfall.Core/HotReload/ReloadableRegistry.cs` | Maps file paths to `IReloadable` implementations |
| `Assets/Ashfall.Core/HotReload/CatalogSnapshot.cs` | Immutable snapshot of a catalog's pre- and post-reload state |

**`CatalogReloadOrchestrator` design:**

```csharp
namespace Ashfall.Core.HotReload;

public class CatalogReloadOrchestrator
{
    private readonly ReloadableRegistry _registry;
    private readonly IFileIO _fileIo;
    private readonly ILog _log;
    private bool _reloadEnabled = true;

    public event Action<string, ReloadResult>? OnReloadCompleted;

    public CatalogReloadOrchestrator(ReloadableRegistry registry, IFileIO fileIo, ILog log)
    {
        _registry = registry;
        _fileIo = fileIo;
        _log = log;
    }

    /// Pause reloads during save/load to prevent race conditions.
    public void SetEnabled(bool enabled) => _reloadEnabled = enabled;

    public ReloadResult HandleFileChange(string relativePath)
    {
        if (!_reloadEnabled)
        {
            _log.Info($"[HotReload] Ignored change to '{relativePath}' (reload paused)");
            return new ReloadResult { Success = false, ErrorMessage = "Reload paused" };
        }

        var reloadable = _registry.Resolve(relativePath);
        if (reloadable == null)
        {
            _log.Warn($"[HotReload] No reloadable registered for '{relativePath}'");
            return new ReloadResult { Success = false, ErrorMessage = "Unknown catalog" };
        }

        string rawJson;
        try
        {
            // [CORRECTED] Use the IFileIO port's own Combine, not System.IO.Path.Combine —
            // the original sample violated Invariant 1 (zero engine/BCL-path coupling
            // assumptions in Core) by hardcoding System.IO.Path here. IFileIO.Combine
            // exists precisely so Core code never assumes a filesystem path separator.
            rawJson = _fileIo.ReadAllText(
                _fileIo.Combine("Assets/StreamingAssets/Data", relativePath));
        }
        catch (Exception ex)
        {
            _log.Error($"[HotReload] Failed to read '{relativePath}': {ex.Message}");
            return new ReloadResult { Success = false, ErrorMessage = ex.Message };
        }

        var result = reloadable.Reload(rawJson, _log);

        if (result.Success)
            _log.Info($"[HotReload] Reloaded '{reloadable.CatalogId}': +{result.Added} ~{result.Modified} -{result.Deprecated}");
        else
            _log.Error($"[HotReload] Reload failed for '{reloadable.CatalogId}': {result.ErrorMessage}");

        OnReloadCompleted?.Invoke(reloadable.CatalogId, result);
        return result;
    }
}
```

**`ReloadableRegistry` design:**

```csharp
public class ReloadableRegistry
{
    private readonly Dictionary<string, IReloadable> _byFile = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IReloadable> _byDirectory = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string filePattern, IReloadable reloadable)
    {
        _byFile[filePattern] = reloadable;
    }

    public void RegisterDirectory(string directory, IReloadable reloadable)
    {
        _byDirectory[directory] = reloadable;
    }

    public IReloadable? Resolve(string relativePath)
    {
        // Exact file match first
        if (_byFile.TryGetValue(relativePath, out var exact))
            return exact;

        // Directory prefix match (for narrative/ subdirectories)
        // NOTE: Path.GetDirectoryName below is System.IO, used here only for
        // string manipulation of an already-relative path (no filesystem I/O
        // performed) — this is a narrower, defensible use than the ReadAllText
        // path-join fixed above, but still worth a second look during
        // implementation: prefer a manual '/'-split if this class must stay
        // provably free of System.IO usage per Invariant 1.
        var dir = Path.GetDirectoryName(relativePath)?.Replace('\\', '/') ?? "";
        foreach (var (prefix, reloadable) in _byDirectory)
        {
            if (dir.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return reloadable;
        }

        return null;
    }
}
```

**Registration example (in Godot host wiring):**

```csharp
// In Main.cs or host setup
var registry = new ReloadableRegistry();
registry.Register("items.json", itemsCatalog);
registry.Register("locations.json", locationsCatalog);
registry.Register("encounters.json", encountersCatalog);
registry.Register("recipes.json", recipesCatalog);
registry.Register("factions.json", factionsCatalog);
registry.RegisterDirectory("narrative/", narrativeCatalog);
registry.RegisterDirectory("expeditions/", expeditionCatalog);
```

### Verification

- `CatalogReloadOrchestrator` compiles in Core (no engine references)
- Unit test: orchestrator routes `items.json` change to items reloadable
- Unit test: orchestrator rejects unknown file path gracefully
- Unit test: orchestrator ignores changes when paused
- `dotnet test Ashfall.Core.Tests/` passes with new tests

### Done-when

- [ ] `CatalogReloadOrchestrator` routes file changes to correct `IReloadable`
- [ ] `ReloadableRegistry` supports file and directory-prefix matching
- [ ] Orchestrator pauses during save/load operations
- [ ] Failed reads/parses logged and reported (never crash)
- [ ] 5+ unit tests covering routing, pause, error handling

---

## Step 4 — Design and Implement State Reconciliation

### Goal

Define how live game state adapts when a catalog is reloaded. The reconciler must uphold the safety invariant: no runtime crash from stale references, no silent data loss.

### [CORRECTED] A deeper problem than signature mismatch: the real item catalog is append-only

**[FLAGGED — most significant finding of this review]** The architecture-reality note in the Problem Statement above covers *loader* signature diversity, but there's a more fundamental issue specific to items. The actual engine-agnostic item catalog class in this codebase — `Ashfall.Core.Inventory.ItemCatalog` (defined in `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs`, not in any file named `ItemsCatalog.cs`) — has this `Register` method:

```csharp
public void Register(ItemDefinition def)
{
    if (def != null && !string.IsNullOrEmpty(def.id) && !_byId.ContainsKey(def.id))
        _byId[def.id] = def;
}
```

This is **append-only, first-write-wins**: if `def.id` already exists in `_byId`, the call silently does nothing. There is no `Unregister`/`Remove` method, and no way to overwrite an existing definition short of reaching into the private `_byId` field. This directly contradicts the "Modified" reconciliation rule this plan depends on (`## Modify existing item stat → instances reference by ID; they get updated stats next access`) and the atomic-swap pattern (`_definitions = merged;`) shown in Step 5's illustrative code: **the real class as written cannot update or remove an item definition at all**, hot reload or otherwise. Any implementation of `IReloadable` against the real item catalog must first add mutation capability to `ItemCatalog` itself (a `Replace`/`Upsert` and a `Remove`/`Deprecate` method) — that is prerequisite scope this plan does not currently account for, and it is a change to a shared, currently-append-only class that other calling code may be relying on the immutability of (worth an explicit check: does anything depend on `Register` being a safe no-op for duplicate ids, e.g. idempotent startup registration from multiple sources?).

### Edge case: item removed from JSON while a survivor holds an instance — is it properly analyzed?

The review brief asks specifically whether this case is properly analyzed. Partial credit: the plan does correctly identify the *rule* (deprecate, don't delete) and the *mechanism* (`GetActiveIds()` + `StateReconciler.Reconcile` routes it to `Deprecated` not `Removed`), and the `DeprecatedDefinition<TDef>` tombstone type is a reasonable data shape. But the analysis has three concrete unaddressed holes:

1. **`GetActiveIds()` is a snapshot, and the plan never states how fresh it must be.** The illustrative implementation walks `_survivorRegistry.All` and every inventory slot at the moment `Reload()` is called. If a reload happens mid-tick (Step 4's own "Safety invariants" list item 5 says reload is disabled during save/load, but says nothing about *other* concurrent core-simulation ticks that mutate inventories), an item could be picked up or dropped in the same frame as the reload, racing the `GetActiveIds()` snapshot against the actual mutation. The "Reload applied only between frames" mitigation in the Risks table addresses *frame-spike* risk, not this specific *snapshot-staleness* race — these are different problems and the plan conflates them.
2. **What happens when the deprecated item is later used, dropped, or crafted with?** The plan says a deprecated item "survives" and is "still resolvable" via `GetDeprecated()`, but never specifies runtime behavior: can a player still use a deprecated med-kit? Craft with a deprecated component? Sell it in a `TradeDetailPanel`? Or is it inert-but-visible? This is exactly the kind of player-facing behavior that "no crash" doesn't answer, and it directly affects `DynamicEconomySystem`/`SurvivorWorkShiftSystem`-style code paths noted elsewhere in this project's known issues as already having 0 core references — i.e., some of the systems most likely to hold onto a stale item reference are also the ones least likely to have been audited for how they react to a `DeprecatedDefinition` tombstone appearing where a live `ItemDefinition` used to be.
3. **Deprecated-item cleanup is hand-waved.** The Risks table says "Periodic cleanup: remove deprecated entries when `GetActiveIds()` no longer references them" but no step implements this — there's no scheduled sweep, no hook into the day/tick clock, and no test for it. Without it, "Deprecated items accumulate forever" (the risk's own stated impact) is not actually mitigated by anything in Steps 1–7; the mitigation is aspirational, not implemented. This should either become its own numbered sub-task with a Done-when, or the risk entry should be downgraded from "mitigated" to "open," since right now nothing in the plan closes it.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `Assets/Ashfall.Core/HotReload/StateReconciler.cs` | Core reconciliation logic |
| `Assets/Ashfall.Core/HotReload/ReconciliationReport.cs` | What changed, what was deprecated, what was rejected |
| `Assets/Ashfall.Core/HotReload/DeprecatedDefinition.cs` | Tombstone for removed-but-referenced definitions |

**`StateReconciler` design:**

```csharp
namespace Ashfall.Core.HotReload;

public class StateReconciler
{
    private readonly ILog _log;

    public StateReconciler(ILog log) => _log = log;

    /// Reconciles a catalog reload against live state.
    /// Returns which IDs were added, modified, deprecated, or rejected.
    public ReconciliationReport Reconcile<TDef>(
        IReadOnlyDictionary<string, TDef> oldCatalog,
        IReadOnlyDictionary<string, TDef> newCatalog,
        IReadOnlySet<string> activeIds) where TDef : class
    {
        var report = new ReconciliationReport();

        // 1. Find additions (in new, not in old)
        foreach (var (id, def) in newCatalog)
        {
            if (!oldCatalog.ContainsKey(id))
            {
                report.Added.Add(id);
            }
        }

        // 2. Find modifications (in both, but different)
        foreach (var (id, newDef) in newCatalog)
        {
            if (oldCatalog.TryGetValue(id, out var oldDef))
            {
                if (!ReferenceEquals(oldDef, newDef) && !oldDef.Equals(newDef))
                {
                    report.Modified.Add(id);
                }
            }
        }

        // 3. Find removals (in old, not in new)
        foreach (var (id, _) in oldCatalog)
        {
            if (!newCatalog.ContainsKey(id))
            {
                if (activeIds.Contains(id))
                {
                    // SAFETY: cannot remove, deprecate instead
                    report.Deprecated.Add(id);
                    _log.Warn($"[Reconcile] '{id}' removed from JSON but still active — deprecated, not deleted");
                }
                else
                {
                    // Safe to fully remove — nothing references it
                    report.Removed.Add(id);
                }
            }
        }

        return report;
    }
}

public class ReconciliationReport
{
    public List<string> Added { get; } = new();
    public List<string> Modified { get; } = new();
    public List<string> Deprecated { get; } = new();  // Removed from JSON but still in use
    public List<string> Removed { get; } = new();     // Safely removed (unreferenced)

    public int TotalChanges => Added.Count + Modified.Count + Deprecated.Count + Removed.Count;
    public bool HasDeprecations => Deprecated.Count > 0;
}
```

**Deprecated definition handling:**

```csharp
/// A definition that was removed from JSON but is still referenced by live state.
/// Kept in memory with a deprecation marker until all references are cleared.
public class DeprecatedDefinition<TDef> where TDef : class
{
    public string Id { get; }
    public TDef Definition { get; }
    public DateTime DeprecatedAt { get; }
    public string Reason { get; }

    public DeprecatedDefinition(string id, TDef definition, string reason)
    {
        Id = id;
        Definition = definition;
        DeprecatedAt = DateTime.UtcNow;
        Reason = reason;
    }
}
```

**Active ID resolution per system:**

```csharp
// Example: ItemsCatalog implements IReloadable
public IReadOnlySet<string> GetActiveIds()
{
    var active = new HashSet<string>();
    // Items in any survivor's inventory
    foreach (var survivor in _survivorRegistry.All)
        foreach (var slot in survivor.Inventory.AllSlots)
            active.Add(slot.ItemId);
    // Items in any recipe as input/output
    foreach (var recipe in _recipeCatalog.All)
    {
        active.UnionWith(recipe.Inputs.Select(i => i.ItemId));
        active.Add(recipe.OutputItemId);
    }
    return active;
}
```

### Verification

- `StateReconciler` is engine-agnostic (lives in Core)
- Unit test: adding new ID → reported in `Added`
- Unit test: modifying existing ID → reported in `Modified`
- Unit test: removing unreferenced ID → reported in `Removed`
- Unit test: removing active ID → reported in `Deprecated` (NOT removed)
- Unit test: empty new catalog with all IDs active → all deprecated, none removed
- `dotnet test Ashfall.Core.Tests/` passes

### Done-when

- [ ] `StateReconciler` handles add/modify/remove/deprecate correctly
- [ ] Active-ID resolution implemented for items, locations, encounters
- [ ] Deprecated definitions kept in memory with tombstone marker
- [ ] Safety invariant proven by tests: active IDs never deleted
- [ ] 8+ unit tests covering all reconciliation paths
- [ ] `ItemCatalog` (or whichever real class is confirmed) has its append-only `Register` extended with an explicit `Replace`/`Upsert` and `Remove`/`Deprecate` method — a written, tested prerequisite change, not assumed to already support this
- [ ] Runtime behavior for a deprecated-but-still-referenced item (usable? craftable? tradeable? inert?) is explicitly decided and documented, not left as "survives" with unspecified semantics
- [ ] A deprecated-definition cleanup sweep is implemented (not just mentioned in the Risks table) with a test proving a deprecated entry is actually removed once `GetActiveIds()` stops referencing it — or, if this is deferred to a later batch, the Risks table entry is downgraded from "mitigated" to "open" and the deferral is explicit

---

## Step 5 — Implement Reloadable Catalogs (Items, Locations, Encounters)

### Goal

**[CORRECTED — see Review Notes and the architecture-reality callout above]** The original goal named three classes — `ItemsCatalog`, `LocationsCatalog`, `EncountersCatalog` — that do not exist in this codebase under those names. Before this step can be scoped for real, the implementer must:

1. Identify the actual class(es) responsible for items (candidates: `Inventory/ItemDefinitions.cs`, `HoldfastItemsCatalog.cs` — confirm which one is the runtime source of truth for `items.json` vs. which is Holdfast-expansion-specific)
2. Identify the actual class(es) responsible for locations (no dedicated `LocationsCatalog` exists; `locations.json` is consumed by something in `World/` — locate it)
3. Identify the actual class(es) responsible for encounters (candidates: `Narrative/EncounterCatalog.cs`, `Narrative/NarrativeEncounterSystem.cs`, `YearOfAsh/DoorEncounterCatalogLoader.cs` — these are not interchangeable; pick the one that actually owns general-purpose encounter data, or state that this plan targets Narrative encounters specifically, not YearOfAsh's)

Once real classes are identified, make the three most frequently-edited catalogs implement `IReloadable`. These should still cover the majority of designer iteration cycles, but the specific classes and their existing load-signatures (see architecture-reality note above) must be confirmed against source before writing adapter code — do not assume a `Reload(string rawJson, ILog log)` retrofit is a small change until you've read the target class.

### Implementation

**Modified files (placeholder — replace with confirmed real class names before starting this step):**

| File | Change |
|------|--------|
| *(confirm real item-catalog class — candidates: `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs`, `Assets/Ashfall.Core/HoldfastItemsCatalog.cs`)* | Implement `IReloadable` |
| *(confirm real location-catalog class — no `LocationsCatalog.cs` exists; locate the consumer of `locations.json`)* | Implement `IReloadable` |
| *(confirm real encounter-catalog class — candidates: `Assets/Ashfall.Core/Narrative/EncounterCatalog.cs`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`)* | Implement `IReloadable` |

**Illustrative reload implementation (class name is a placeholder — see Goal above; do not create a new class named `ItemsCatalog` that duplicates existing item-definition code):**

```csharp
// Illustrative only. Apply this Reload() pattern to whichever real class is
// confirmed as the item-definition source of truth (see Goal). Do not
// introduce a parallel "ItemsCatalog" type alongside the existing
// ItemDefinitions/HoldfastItemsCatalog code — that would create the exact
// dual-authority problem Invariant 6 warns against.
public class ExistingItemDefinitionsClass : IReloadable
{
    private Dictionary<string, ItemDefinition> _definitions = new();
    private readonly List<DeprecatedDefinition<ItemDefinition>> _deprecated = new();
    private readonly StateReconciler _reconciler;
    private readonly ILog _log;

    public string CatalogId => "items";

    public ReloadResult Reload(string rawJson, ILog log)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        // 1. Parse new catalog
        ItemDefinition[] newDefs;
        try
        {
            newDefs = ParseItemDefinitions(rawJson);
        }
        catch (Exception ex)
        {
            return new ReloadResult
            {
                Success = false,
                ErrorMessage = $"JSON parse error: {ex.Message}",
                Duration = sw.Elapsed
            };
        }

        // 2. Validate (CatalogIntegrityValidator rules)
        var validationErrors = ValidateDefinitions(newDefs);
        if (validationErrors.Count > 0)
        {
            log.Error($"[ItemsCatalog] Reload rejected: {validationErrors.Count} validation errors");
            foreach (var err in validationErrors.Take(5))
                log.Error($"  - {err}");
            return new ReloadResult
            {
                Success = false,
                ErrorMessage = $"{validationErrors.Count} validation errors",
                Rejected = validationErrors.Count,
                Duration = sw.Elapsed
            };
        }

        // 3. Reconcile against live state
        var newDict = newDefs.ToDictionary(d => d.Id);
        var activeIds = GetActiveIds();
        var report = _reconciler.Reconcile(_definitions, newDict, activeIds);

        // 4. Apply changes atomically
        var merged = new Dictionary<string, ItemDefinition>(newDict);

        // Re-insert deprecated items (removed from JSON but still active)
        foreach (var depId in report.Deprecated)
        {
            if (_definitions.TryGetValue(depId, out var old))
            {
                merged[depId] = old;
                _deprecated.Add(new DeprecatedDefinition<ItemDefinition>(depId, old, "Removed during hot reload"));
            }
        }

        // Atomic swap
        _definitions = merged;

        sw.Stop();
        return new ReloadResult
        {
            Success = true,
            Added = report.Added.Count,
            Modified = report.Modified.Count,
            Deprecated = report.Deprecated.Count,
            Duration = sw.Elapsed
        };
    }

    public IReadOnlySet<string> GetActiveIds()
    {
        // Delegate to active-state query (injected survivor registry, recipes, etc.)
        return _activeIdResolver.GetActiveItemIds();
    }
}
```

**Locations and encounters follow the same pattern** with domain-specific:
- Parse logic (locations have coordinates, zone references)
- Validation rules (locations need valid sector IDs, encounters need valid reward item IDs)
- Active-ID resolution (locations active if any survivor/expedition is there; encounters active if any quest references them)

**[FLAGGED]** "Follow the same pattern" assumes the real location and encounter classes have a shape similar to whatever item-definition class is chosen — this is not guaranteed given the confirmed heterogeneity of loader signatures across the codebase (see architecture-reality callout). Treat each of the three catalogs as requiring its own investigation, not a copy-paste of the first one's adapter.

### Verification

- All three catalogs implement `IReloadable` and compile cleanly
- Unit test per catalog: reload with valid JSON → success, definitions updated
- Unit test per catalog: reload with invalid JSON → failure, old definitions retained
- Unit test per catalog: reload removing active ID → deprecated, not deleted
- Integration test: reload items → inventory still resolves all slot IDs
- `dotnet test` + `dotnet build Ashfall.csproj` pass

### Done-when

- [ ] Real item/location/encounter catalog classes identified by name and confirmed to exist in the current tree (this replaces the fictional `ItemsCatalog.cs`/`LocationsCatalog.cs`/`EncountersCatalog.cs` from the original draft) — this is a prerequisite sub-task, not optional groundwork
- [ ] The three identified classes implement `IReloadable`
- [ ] Each catalog validates JSON before accepting reload
- [ ] Active-ID protection prevents runtime crashes from stale references
- [ ] Deprecated definitions accessible via `GetDeprecated()` for debugging UI
- [ ] 9+ unit tests (3 per catalog: valid reload, invalid JSON, active-ID protection)
- [ ] Existing catalog loader tests still pass (reload is additive, not breaking)
- [ ] No new class was introduced that duplicates an existing catalog's authority (Invariant 6 check)

---

## Step 6 — Developer Hotkey and Reload UI

### Goal

Provide a developer-facing interface for triggering reloads: F5 hotkey for force-reload-all, and a small overlay showing reload status/results.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `src/HotReload/HotReloadHostBridge.cs` | Wires FileWatcher → Orchestrator → UI refresh |
| `src/HotReload/ReloadOverlay.cs` | Small status overlay (bottom-right) showing reload results |
| `src/HotReload/HotReloadConfig.cs` | Enable/disable, hotkey binding, auto-reload toggle |

**`HotReloadHostBridge` design:**

```csharp
namespace AtomicWar.GodotApp.HotReload;

public partial class HotReloadHostBridge : Node
{
    // [CORRECTED] Godot requires signals to be declared with [Signal] before
    // EmitSignal(SignalName.X) will compile — the original sample called
    // EmitSignal(SignalName.CatalogsReloaded) with no matching declaration
    // anywhere in this document. Added below.
    [Signal]
    public delegate void CatalogsReloadedEventHandler();

    private GodotFileWatcher _watcher = null!;
    private CatalogReloadOrchestrator _orchestrator = null!;
    private ReloadOverlay _overlay = null!;
    private HotReloadConfig _config = null!;

    public override void _Ready()
    {
        _config = HotReloadConfig.Load();
        if (!_config.Enabled) return;

        _watcher = new GodotFileWatcher();
        AddChild(_watcher);

        _overlay = new ReloadOverlay();
        GetTree().Root.AddChild(_overlay);

        _watcher.OnFileChanged += HandleFileChange;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F5)
        {
            ForceReloadAll();
            GetViewport().SetInputAsHandled();
        }
    }

    private void HandleFileChange(FileChangeEvent e)
    {
        if (!_config.AutoReload) return;

        var result = _orchestrator.HandleFileChange(e.RelativePath);
        _overlay.ShowResult(e.RelativePath, result);
    }

    public void ForceReloadAll()
    {
        _overlay.ShowStatus("Reloading all catalogs...");

        // [FLAGGED] GetAllReloadables() and GetFilePath(catalogId) are called
        // here but neither CatalogReloadOrchestrator nor ReloadableRegistry
        // (Step 3) declares them. ReloadableRegistry only exposes Register /
        // RegisterDirectory / Resolve(relativePath) — there is no reverse
        // lookup from catalogId back to file path, and no enumeration of all
        // registered reloadables. Step 3 or Step 6 must add these two methods
        // to ReloadableRegistry (and thread them through the orchestrator)
        // before this method can compile. This is scope Step 3 did not budget for.
        var results = new List<(string catalog, ReloadResult result)>();
        foreach (var reloadable in _orchestrator.GetAllReloadables())
        {
            var path = _orchestrator.GetFilePath(reloadable.CatalogId);
            var result = _orchestrator.HandleFileChange(path);
            results.Add((reloadable.CatalogId, result));
        }

        var total = results.Count;
        var succeeded = results.Count(r => r.result.Success);
        var failed = total - succeeded;

        _overlay.ShowSummary(succeeded, failed);

        // Notify UI panels to refresh their data bindings
        EmitSignal(SignalName.CatalogsReloaded);
    }

    /// Pause during save/load
    public void PauseReloads() => _orchestrator.SetEnabled(false);
    public void ResumeReloads() => _orchestrator.SetEnabled(true);
}
```

**`ReloadOverlay` design:**

```csharp
public partial class ReloadOverlay : Control
{
    private Label _statusLabel = null!;
    private Timer _fadeTimer = null!;
    private const float DISPLAY_DURATION = 3.0f;

    public override void _Ready()
    {
        // Position: bottom-right corner, semi-transparent background
        AnchorLeft = 1.0f; AnchorTop = 1.0f;
        AnchorRight = 1.0f; AnchorBottom = 1.0f;
        OffsetLeft = -320; OffsetTop = -60;
        OffsetRight = -10; OffsetBottom = -10;

        var bg = new ColorRect { Color = new Color(0.1f, 0.1f, 0.1f, 0.8f) };
        bg.SetAnchorsPreset(LayoutPreset.FullRect);
        AddChild(bg);

        _statusLabel = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center
        };
        _statusLabel.AddThemeFontSizeOverride("font_size", 12);
        AddChild(_statusLabel);

        _fadeTimer = new Timer { OneShot = true, WaitTime = DISPLAY_DURATION };
        _fadeTimer.Timeout += () => Visible = false;
        AddChild(_fadeTimer);

        Visible = false;
    }

    public void ShowResult(string file, ReloadResult result)
    {
        var icon = result.Success ? "[OK]" : "[FAIL]";
        _statusLabel.Text = $"{icon} {Path.GetFileName(file)} — +{result.Added} ~{result.Modified} dep:{result.Deprecated}";
        _statusLabel.Modulate = result.Success ? Colors.Green : Colors.Red;
        Visible = true;
        _fadeTimer.Start();
    }

    public void ShowStatus(string message)
    {
        _statusLabel.Text = message;
        _statusLabel.Modulate = Colors.Yellow;
        Visible = true;
    }

    public void ShowSummary(int succeeded, int failed)
    {
        _statusLabel.Text = $"Reload complete: {succeeded} ok, {failed} failed";
        _statusLabel.Modulate = failed == 0 ? Colors.Green : Colors.Orange;
        _fadeTimer.Start();
    }
}
```

**Configuration (`HotReloadConfig`):**

```csharp
public class HotReloadConfig
{
    public bool Enabled { get; set; } = true;        // Master switch
    public bool AutoReload { get; set; } = true;     // Auto-reload on file change
    public float DebounceMs { get; set; } = 300f;    // Debounce window
    public Key Hotkey { get; set; } = Key.F5;        // Force-reload-all key

    // Only active in debug/development builds
    public static HotReloadConfig Load()
    {
        if (!OS.IsDebugBuild()) return new HotReloadConfig { Enabled = false };
        // Load from user://hot_reload_config.json or return defaults
        return new HotReloadConfig();
    }
}
```

### Verification

- F5 reloads all catalogs and overlay shows results
- Auto-reload triggers on file save (with 300ms debounce)
- Overlay disappears after 3 seconds
- Hot reload disabled in release builds (no performance cost)
- Reloads paused during save/load (no race conditions)

### Done-when

- [ ] F5 triggers force-reload-all in development builds
- [ ] Overlay shows per-file reload status with fade
- [ ] Auto-reload on file change (configurable, default: on)
- [ ] Reload paused during save/load operations
- [ ] Hot reload completely disabled in release builds
- [ ] Configuration persisted in `user://` directory

---

## Step 7 — Hot Reload Tests

### Goal

Write comprehensive tests proving hot reload correctness: valid changes apply, invalid changes are rejected, active-state protection works, and concurrent access is safe.

### Implementation

**New test files:**

| File | Purpose |
|------|---------|
| `Ashfall.Core.Tests/HotReload/CatalogReloadOrchestratorTests.cs` | Orchestrator routing and error handling |
| `Ashfall.Core.Tests/HotReload/StateReconcilerTests.cs` | All reconciliation paths |
| `Ashfall.Core.Tests/HotReload/ItemsCatalogReloadTests.cs` | Items-specific reload behavior |
| `Ashfall.Core.Tests/HotReload/LocationsCatalogReloadTests.cs` | Locations-specific reload behavior |
| `Ashfall.Core.Tests/HotReload/EncountersCatalogReloadTests.cs` | Encounters-specific reload behavior |
| `Ashfall.Core.Tests/HotReload/ReloadSafetyTests.cs` | Edge cases, concurrency, corruption resistance |

**Test categories:**

```csharp
namespace Ashfall.Core.Tests.HotReload;

public class StateReconcilerTests
{
    [Fact] public void AddNewItem_ReportsAdded() { /* ... */ }
    [Fact] public void ModifyExistingItem_ReportsModified() { /* ... */ }
    [Fact] public void RemoveUnreferencedItem_ReportsRemoved() { /* ... */ }
    [Fact] public void RemoveActiveItem_ReportsDeprecated_NotRemoved() { /* ... */ }
    [Fact] public void EmptyCatalog_AllActive_AllDeprecated() { /* ... */ }
    [Fact] public void NoChanges_EmptyReport() { /* ... */ }
    [Fact] public void RenameId_TreatedAsRemovePlusAdd() { /* ... */ }
}

public class CatalogReloadOrchestratorTests
{
    [Fact] public void RouteToCorrectReloadable_ByFilePath() { /* ... */ }
    [Fact] public void UnknownFile_ReturnsFailure_NoCrash() { /* ... */ }
    [Fact] public void PausedState_IgnoresChanges() { /* ... */ }
    [Fact] public void FileReadError_ReturnsFailure_KeepsOldCatalog() { /* ... */ }
    [Fact] public void EmitsEvent_OnSuccessfulReload() { /* ... */ }
    [Fact] public void DirectoryPrefix_MatchesSubdirectoryFiles() { /* ... */ }
}

public class ItemsCatalogReloadTests
{
    [Fact] public void ValidJson_UpdatesDefinitions() { /* ... */ }
    [Fact] public void InvalidJson_RejectsReload_KeepsOld() { /* ... */ }
    [Fact] public void ValidationFailure_RejectsReload_LogsErrors() { /* ... */ }
    [Fact] public void ActiveItemRemoved_Deprecated_StillResolvable() { /* ... */ }
    [Fact] public void AddNewItem_ImmediatelyAvailable() { /* ... */ }
    [Fact] public void ModifyItemStat_ExistingInstancesGetNewValue() { /* ... */ }
    [Fact] public void AtomicSwap_PartialFailure_NoHalfState() { /* ... */ }
    [Fact] public void SchemaVersionMismatch_Rejected() { /* ... */ }
}

public class ReloadSafetyTests
{
    [Fact] public void ReloadDuringSave_Paused_NoEffect() { /* ... */ }
    [Fact] public void RapidSuccessiveReloads_OnlyLastApplied() { /* ... */ }
    [Fact] public void EmptyJsonFile_RejectedGracefully() { /* ... */ }
    [Fact] public void MalformedUtf8_RejectedWithMessage() { /* ... */ }
    [Fact] public void VeryLargeFile_CompletesWithinTimeout() { /* ... */ }
    [Fact] public void ConcurrentGetActiveIds_DuringReload_NoRace() { /* ... */ }
}
```

**Godot-host integration test (headless):**

```csharp
// Runs via: godot --headless --path . -- --hot-reload-selftest
// 1. Boot game with known items.json
// 2. Modify items.json on disk (add item_test_new)
// 3. Trigger reload
// 4. Assert item_test_new is now in catalog
// 5. Modify items.json again (remove item_test_new, modify item_bandage stats)
// 6. Trigger reload
// 7. Assert item_test_new removed (not active), item_bandage updated
// 8. Remove item that IS in a survivor's inventory
// 9. Trigger reload
// 10. Assert item deprecated (still resolvable), not crashed
```

**CLI verb:**

```csharp
case "--hot-reload-selftest":
    // Runs the headless integration test above
    RunHotReloadSelfTest();
    return;
```

### Verification

- `dotnet test Ashfall.Core.Tests/` passes all new hot reload tests
- `godot --headless --path . -- --hot-reload-selftest` exits 0
- No engine references in Core hot reload code
- Tests cover: valid, invalid, edge cases, safety invariants
- No flaky tests (deterministic seeds, no timing-dependent assertions)

### Done-when

- [ ] 30+ unit tests across 6 test files
- [ ] All reconciliation paths tested (add, modify, remove, deprecate)
- [ ] All error paths tested (invalid JSON, file read error, schema mismatch, empty file)
- [ ] Safety tests prove: no crash from active-ID removal, no corruption from paused reload
- [ ] `--hot-reload-selftest` CLI verb runs headless integration test
- [ ] All tests pass: `dotnet test` + `godot --headless --hot-reload-selftest`

---

## Summary Table

| Step | Deliverable | Key Files | Tests Added | Risk |
|------|-------------|-----------|-------------|------|
| 1 | Architecture design | `docs/hot-reload-design.md`, `Assets/Ashfall.Core/HotReload/IReloadable.cs` | 0 (design) | Low — interface only |
| 2 | FileWatcher | `src/HotReload/GodotFileWatcher.cs`, `DebounceTimer.cs` | 3 (debounce timer) | Medium — OS file-event edge cases |
| 3 | CatalogReloader | `Assets/Ashfall.Core/HotReload/CatalogReloadOrchestrator.cs`, `ReloadableRegistry.cs` | 6 (routing, error handling) | Low — pure logic |
| 4 | State reconciliation | `Assets/Ashfall.Core/HotReload/StateReconciler.cs`, `ReconciliationReport.cs` | 8 (all reconciliation paths) | Medium — must be provably safe |
| 5 | Reloadable catalogs | Real classes TBD after Step 5's identification sub-task (placeholders used in earlier drafts — `ItemsCatalog.cs`, `LocationsCatalog.cs`, `EncountersCatalog.cs` — do not exist and must not be created as new duplicate types) | 9 (3 per catalog) | Medium-High — must not break existing loaders; architecture heterogeneity (see Review Notes) raises this above the original "Medium" estimate |
| 6 | Developer hotkey + overlay | `src/HotReload/HotReloadHostBridge.cs`, `ReloadOverlay.cs`, `HotReloadConfig.cs` | 0 (UI, manual verify) | Low — development-only feature |
| 7 | Hot reload tests | 6 new test files in `Ashfall.Core.Tests/HotReload/` | 30+ tests + integration selftest | Low — test infrastructure |

**Total new tests:** 56+ (unit) + 1 headless integration selftest — **estimate only; see flag on Step 5 and the Success Criteria note about recomputing this once real target classes are confirmed**

---

## Dependencies and Sequencing

```
Step 1 (design) ─── Step 2 (FileWatcher) ─────┐
                                               │
Step 1 (design) ─── Step 3 (orchestrator) ────┼── Step 6 (hotkey/overlay)
                                               │
Step 1 (design) ─── Step 4 (reconciler) ──┐    │
                                          │    │
                    Step 3 + Step 4 ─────┴── Step 5 (catalogs) ── Step 7 (tests)
```

**[CORRECTED]** The original diagram routed Step 4 (reconciler) directly into Step 6 (hotkey/overlay); it does not — Step 6's `HotReloadHostBridge` only calls into `CatalogReloadOrchestrator` (Step 3) and `GodotFileWatcher` (Step 2). Step 4's reconciler is consumed by Step 5 (the catalogs call `StateReconciler.Reconcile` inside their own `Reload()`), not by Step 6 directly. Steps 2 and 3 can proceed in parallel after Step 1. Step 4 can also proceed in parallel after Step 1, but must land before Step 5 starts (Step 5 calls the reconciler). Step 5 requires Step 3 + Step 4, **and now also requires the class-identification sub-task from Step 5's corrected Goal, and the `ItemCatalog` mutation-capability change described in Step 4's corrected Goal** — both are new prerequisites this document did not originally call out. Steps 6 and 7 integrate everything.

---

## Risks and Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| File watcher misses changes on some OS | Designer doesn't see updates | F5 force-reload as fallback; polling option for problematic OS. **[FLAGGED]** No `FileSystemWatcher` code exists in this repo today (verified) — this is not a case of an existing watcher having gaps, it's an unbuilt component whose reliability on this project's actual dev platforms (confirmed Linux via inotify in this session) should be spiked/prototyped before Step 2 is fully built out, not discovered after |
| Reconciliation misses an active-ID source | Runtime crash on next access | Comprehensive `GetActiveIds()` implementation; fail-closed (if in doubt, deprecate not delete) |
| Reload during tick processing | Inconsistent state within a frame | Reload applied only between frames (Godot `_Process` boundary). **[FLAGGED — distinct risk, not covered]** This mitigates frame-spike/mid-tick timing, but does not address `GetActiveIds()` snapshot staleness if an inventory mutation and a reload are scheduled for the same frame boundary — see the edge-case analysis added to Step 4 |
| Large catalog reload causes frame spike | Visible stutter | Profile; if > 16ms, defer to next frame or background thread parse |
| Concurrent file writes (git pull, branch switch) | Flood of reload events | Debounce (300ms) + force-reload coalesces to single pass |
| Deprecated items accumulate forever | Memory leak over long sessions | **[DOWNGRADED to "open" — see Step 4]** "Periodic cleanup" is described here but not implemented by any step in this document. Either add a concrete sub-task with a Done-when (now added to Step 4) or accept this risk as unmitigated for this batch and say so |
| **[NEW]** Real `ItemCatalog.Register` is append-only, first-write-wins | Hot reload of items cannot function against the existing class without a prerequisite change | Add `Replace`/`Upsert` and `Remove`/`Deprecate` methods to `ItemCatalog` as an explicit, reviewed, tested change before building `IReloadable` on top of it (now required in Step 4's Done-when) |
| **[NEW]** Named target classes (`ItemsCatalog`, `LocationsCatalog`, `EncountersCatalog`) don't exist | Step 5 cannot start as originally scoped; risk of a new duplicate-authority class being created by mistake | Class-identification sub-task added to Step 5's Goal; explicit Invariant 6 check added to Step 5's Done-when |

**Rollback plan:** Steps 1–4 are additive and engine-agnostic (new files under `Assets/Ashfall.Core/HotReload/`, plus the `ItemCatalog` mutation methods, which are additive API surface — existing callers of `Register`/`Get`/`Contains` are unaffected). Step 2 (`GodotFileWatcher`) and Step 6 (`HotReloadHostBridge`, `ReloadOverlay`, hotkey) are additive Godot-host-only files with no gameplay-code touchpoints; if file-watching proves unreliable on the team's platforms (see risk above), Steps 2 and 6 can be dropped or replaced with F5-only manual triggering without affecting Steps 1, 3, 4, 5, or 7. Step 5 is the one step with real rollback risk, because it modifies existing, shipping catalog classes (once identified) rather than adding new ones — land Step 5's changes as one small, reviewable commit per catalog (per the project's "one system per task" rule), verified independently by `dotnet test`, so that a regression in one catalog's reload path can be reverted without touching the other two or the framework underneath them. Hot reload is explicitly a development-time-only feature (`HotReloadConfig.Load()` returns `Enabled = false` on non-debug builds); confirm this gate is enforced by an actual build-configuration check (not just a runtime flag defaulting to true) before treating "zero cost to release builds" as satisfied.

---

## Success Criteria

- [ ] Edit `items.json` → press F5 → new item immediately available in-game (no restart)
- [ ] Invalid JSON shows error overlay → game continues with previous data (no crash)
- [ ] Item in survivor's inventory deleted from JSON → item survives as deprecated (no crash), **and its runtime usability (usable/craftable/tradeable/inert) is explicitly verified against the decision made in Step 4, not left implicit**
- [ ] 56+ automated tests prove reload safety — **recompute this number once Step 5's real target classes are known; it was derived assuming three easily-adaptable catalogs, which Step 5's corrected scope shows is not guaranteed**
- [ ] `--hot-reload-selftest` passes headlessly in CI
- [ ] Hot reload adds zero cost to release builds (verified via an actual release-configuration build check, not just `HotReloadConfig.Enabled` defaulting to false)
- [ ] Iteration time for data changes drops from 8–12s restart to < 1s reload — **this specific number was not measured against this project's actual startup time; re-verify the "8–12s" baseline empirically (e.g., time a real `godot --headless` boot) before using it as the improvement claim**


---

## Review Notes (Corrected)

This document was adversarially reviewed against the live codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and edited in place. Summary of findings:

### Factual corrections applied

1. **The catalog loader architecture is not uniform, and this plan originally assumed it was.** Verified counts: only 5 files are literally named `*CatalogLoader.cs`; broadening to any class with "Catalog" in the name gives ~101 files under `Assets/Ashfall.Core/`, consistent with the wave-2 corrected estimate of roughly two dozen real loaders once pure lookup-table catalogs are filtered out. Their load-entry-point signatures are heterogeneous — confirmed sampled examples: `GoodsCatalogLoader.Load(string dataDir, IFileIO fileIO, IJsonSerializer json)`, `CurrentsCatalogLoader.LoadCurrents(...)`, ~20+ `Narrative/*Catalog.cs` classes with `LoadFromDirectory(string)`, `BunkerContrabandCatalog` with both `LoadFromJson(string)` and `LoadFromFile(string)`, `HardcoreEconomyTuningLoader.Load(string json)`. A single retrofittable `IReloadable.Reload(string rawJson, ILog log)` seam does not exist uniformly across these; the document now flags this explicitly at the top (Challenge section) and again at each place where it assumed uniformity (Step 5).
2. **The three named target classes for Step 5 do not exist.** `ItemsCatalog.cs`, `LocationsCatalog.cs`, and `EncountersCatalog.cs` were named as files to modify; none exist anywhere in `Assets/Ashfall.Core/`. Items are handled by `Inventory/ItemDefinitions.cs` and a separate `HoldfastItemsCatalog.cs`; the actual engine-agnostic item catalog class is `Ashfall.Core.Inventory.ItemCatalog`, defined inside `ProceduralItemInstance.cs`. There is no dedicated `LocationsCatalog`; the closest loader-named class is the narrower `Maritime/DeepLoreLocationCatalogLoader.cs`. There is no unified `EncountersCatalog`; `Narrative/EncounterCatalog.cs`, `Narrative/NarrativeEncounterSystem.cs`, and `YearOfAsh/DoorEncounterCatalogLoader.cs` are three distinct, non-interchangeable candidates. Step 5 has been rewritten to require a class-identification sub-task before any adapter code is written, and to explicitly warn against creating new duplicate-authority classes with the originally-named fictional identifiers.
3. **The most significant finding: the real `ItemCatalog.Register` method is append-only and first-write-wins.** `Register(ItemDefinition def)` no-ops if `def.id` already exists in its dictionary — there is no `Unregister`/`Remove`, and no supported way to overwrite an existing entry. This directly breaks the plan's core assumption (an atomic dictionary swap on reload, and a "Modified" reconciliation path that updates existing definitions in place). Any real implementation must first add mutation capability (`Replace`/`Upsert`, `Remove`/`Deprecate`) to this shared class — a prerequisite change with its own review burden (does any existing caller rely on `Register` being a safe idempotent no-op?) that the original plan did not budget for at all. This has been added to Step 4's Goal and Done-when, and reflected in the Risks table and effort-estimate caveat at the top of the document.
4. **File-watching capability does not exist anywhere in this codebase today.** Confirmed via search: zero hits for `FileSystemWatcher` under `src/` or `Assets/Ashfall.Core/`. All existing "reload" references in `Main.cs` and `HostCli.cs` are save/load round-trip testing (e.g., `--holdfast-save-selftest`, `PressReload`), an unrelated and already-solved problem. This is now stated explicitly in Step 2 rather than left implicit, along with a call-out that `System.IO.FileSystemWatcher` on Linux (inotify-backed) has known reliability gaps — dropped events under load, unreliable firing for write-via-rename patterns — that should be spiked empirically against the team's actual editors before the rest of the pipeline is built on top of it.
5. **Minor data corrections:** "130+ JSON files" was updated to the verified count of 296 JSON files in `Assets/StreamingAssets/Data/` (196 of which are the narrative files, also verified). The "8–12 second restart" and "56+ tests" and "233+"-style figures throughout are flagged as unmeasured planning estimates rather than corrected to specific alternate numbers, since I did not have a basis to assert a different precise figure — the fix here is honesty about estimate-vs-measurement, not a fabricated replacement number.

### Code-level defects found and fixed

- **`CatalogReloadOrchestrator.HandleFileChange`** used raw `System.IO.Path.Combine` for a file read inside `Assets/Ashfall.Core/`, which is `Ports.cs`-violating: Core code should use the injected `IFileIO.Combine`, exactly the port that exists to avoid this. Fixed in the code sample and called out as an Invariant 1 concern.
- **`ReloadableRegistry.Resolve`** uses `System.IO.Path.GetDirectoryName` on an already-relative string — lower severity than the above (no actual I/O), but flagged as worth a second look if this class needs to be provably `System.IO`-free.
- **`HotReloadHostBridge.ForceReloadAll`** calls `EmitSignal(SignalName.CatalogsReloaded)` with no matching `[Signal]` declaration anywhere in the class or document — this would not compile. Added the missing `[Signal] public delegate void CatalogsReloadedEventHandler();` declaration.
- **`HotReloadHostBridge.ForceReloadAll`** also calls `_orchestrator.GetAllReloadables()` and `_orchestrator.GetFilePath(reloadable.CatalogId)`, neither of which is declared on `CatalogReloadOrchestrator` or `ReloadableRegistry` in Step 3 (which only exposes `Register`, `RegisterDirectory`, `Resolve(relativePath)`). This is scope Step 3 did not budget for — a reverse lookup (catalogId → file path) and an enumeration method are new API surface needed to make Step 6 compile. Flagged inline; Step 3 or Step 6's task estimate should absorb this.

### Edge case analysis: was "item removed while player holds instance" properly analyzed? — Partial, now completed

The review brief asked this directly. The original document gets the *rule* right (deprecate, don't delete) and has a reasonable tombstone data shape (`DeprecatedDefinition<TDef>`), but left three concrete gaps unaddressed, now filled in on Step 4:

1. **Snapshot staleness**: `GetActiveIds()` is called once per reload and nothing addresses the race between that snapshot and a concurrent inventory mutation (pickup/drop) happening in the same tick window — distinct from the "frame spike" and "mid-tick" risks the original document does address.
2. **Undefined runtime semantics for a deprecated-but-referenced item**: the plan never says whether a deprecated item is still usable, craftable with, or tradeable — "survives, doesn't crash" is not the same as "behaves correctly," and this project's own known-issues list (`AGENTS.md`) already flags several systems with 0 core references (`PersonalQuestSystem`, `MedicalSystem`, `SurvivorWorkShiftSystem`, `DynamicEconomySystem`) as exactly the kind of code most likely to hold a stale item reference without having been audited for how it reacts to a tombstone.
3. **The stated cleanup mitigation ("periodic cleanup... when `GetActiveIds()` no longer references them") is not implemented by any step** — it exists only as a sentence in the Risks table. Added as an explicit Done-when to Step 4, or, if genuinely out of scope for this batch, the Risks table entry should say "open" rather than imply it's handled.

### Ordering/dependency fix

The original dependency diagram routed Step 4 (reconciler) directly into Step 6 (hotkey/overlay); it doesn't — Step 6 only touches the orchestrator (Step 3) and file watcher (Step 2). Step 4 feeds Step 5 (catalogs call the reconciler inside their own `Reload()`), not Step 6. Diagram corrected, and the text now states the additional prerequisites Step 5 picked up from the class-identification and `ItemCatalog`-mutation findings above.

### Rollback plan (previously missing entirely)

No rollback or blast-radius discussion existed anywhere in the original document. Added one: Steps 1–4 are additive to Core (new `HotReload/` namespace, plus additive methods on `ItemCatalog`); Steps 2 and 6 are additive, Godot-host-only, gameplay-code-free, and can be dropped independently if file-watching proves unreliable (falling back to F5-only manual reload with no impact on Steps 1/3/4/5/7); Step 5 is the one step that touches existing shipping catalog classes and should land as one small, independently-revertible commit per catalog, consistent with the project's own "one system per task" git rule.

### Not changed

- The overall reconciliation *rules table* (new/modified/removed/deprecated/schema-mismatch/renamed) is sound game-design thinking and was left as-is — the issue was never the rules, it was the assumption that the existing code has a seam to hang those rules on.
- Step 1 (design doc + `IReloadable` interface) and Step 7 (test file list) are reasonable as scaffolding and were not restructured, beyond flagging that their downstream test-count estimates depend on Step 5's now-corrected scope.
