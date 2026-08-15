# Ashfall — Deep Code Audit (Game + Godot Port)

Date: 2026-08-14 · Scope: `Assets/_Game` (Unity, 228,489 LOC), `src/` (Godot, 2,118 LOC),
`Ashfall.Core` (924 LOC), `Ashfall.Core.Tests`.

Audited against the invariants declared in `AGENTS.md`. Every claim below was verified by running
the build, the self-test, or a scan — commands are given so findings can be re-checked.

> **Note:** `Ashfall.Core` grew from 3 files to 6 during this audit (IceRoadSystem, HoldfastCatalog,
> IceRoadHeadlessDemo were added). Numbers reflect the state at the end of the pass.

---

## REMEDIATION STATUS — 2026-08-14 (same day)

| ID | Finding | Status |
|---|---|---|
| C1 | `Ashfall.Core` orphaned | **FIXED** — core source moved to `Assets/Ashfall.Core/` with `AtomicWar.Ashfall.Core.asmdef`; consumed by the Godot host, 6 Unity asmdefs, and the xunit suite |
| C2 | `WeatherKind` forked | **FIXED** — Unity's duplicate enum deleted; `Ashfall.Core.WeatherKind` is now the only definition (was 2, now 1) |
| C2 | `IceRoadSystem` forked | **PARTIAL** — duplicate deleted, but `Assets/_Game/Core/IceRoadSystem.cs` was restored by parallel work and has since diverged ~521 lines from the core copy. Still needs arbitration. |
| C3 | Cross-host save incompatibility | **OPEN** — `JournalSave` still defined twice; Unity `JsonUtility` vs Godot `System.Text.Json` |
| C4 | Core tests could not run | **FIXED** — retargeted to `net9.0`; 109/109 pass |
| H1 | Godot port lacked test coverage | **IMPROVED** — core suite grew 12 → 109 tests; journal + ice-road self-tests pass headless |
| M1 | Diagnostics bar overlapped whole UI | **FIXED** — `MarginContainer` now has exactly one child (`rootColumn` VBox) |
| M2 | Per-frame allocation in `_Process` | **FIXED** — engine version cached in `s_engineVersion`; label throttled to 4 Hz |
| M3 | Full save written per journal entry | **FIXED** — `_journalDirty` flag, coalesced flush on tick/close/quit |
| M4 | `DateTime.Now` (banned by `Ports.cs`) | **FIXED** — `DateTime.UtcNow` + `CultureInfo.InvariantCulture`, labelled host-diagnostic |
| M5 | `null!` / nullability contradictions | **SUPPRESSED, not fixed** — `<NoWarn>` in `Ashfall.csproj` hides CS8618 etc. rather than resolving them |
| M6 | Empty `catch {}` in self-test | **FIXED** — now logs via `GD.PrintErr` |

**New fork discovered during remediation:** `HoldfastLocationEntry` exists in both `Ashfall.Core`
and `AtomicWar._Game.Data`. Pinned via alias in `HoldfastMapSeeder.cs` to preserve behaviour;
needs unification.

Verification at time of writing: `dotnet build Ashfall.csproj` → **0 errors, 0 warnings**;
`dotnet test` → **109/109 pass**; `--journal-selftest` → **20/20 PASS**;
`--ice-road-selftest` → **PASS 21/21**.

---

## Verified healthy

Worth stating plainly, because most of this audit is critical:

| Check | Result |
|---|---|
| `dotnet build Ashfall.csproj` | **0 errors** (56 nullability warnings) |
| `dotnet build Ashfall.Core` | **0 errors, 0 warnings** |
| `godot --headless --quit-after 2` | boots clean |
| `godot --headless -- --journal-selftest` | **20/20 PASS** |
| Empty `catch {}` blocks in `Assets/_Game` | **0** |
| Culture-sensitive numeric parsing | **effectively clean** — 2 hits, both custom `EquipSlots.Parse`, non-numeric |
| Shared JSON catalogs | **genuinely shared**, not forked (Godot reads `res://Assets/StreamingAssets/Data`) |

The Journal port is real, working, and self-tested. The data layer being unforked is the single
biggest asset the migration has.

---

## CRITICAL

### C1 — `Ashfall.Core` is orphaned; nothing consumes it

`AGENTS.md` calls `Ashfall.Core` the "ONE SOURCE OF TRUTH". It is referenced by **nothing**:

- `Ashfall.csproj` (Godot) compiles only `src/**` and `scripts/**` — no `ProjectReference` to Core.
- Unity `.asmdef` references to `Ashfall.Core`: **0**.
- `using Ashfall.Core` across `Assets/_Game` and `src`: **0**.

924 LOC of well-designed port interfaces and extracted systems that **no shipping code executes**.
Its only consumer is its own test project. The architectural centerpiece the whole migration plan
rests on is currently disconnected from both engines.

```bash
grep -c "Ashfall.Core" Ashfall.csproj              # 0
grep -rl "Ashfall.Core" Assets/_Game --include=*.asmdef | wc -l   # 0
```

### C2 — Logic is being FORKED, not moved (the migration is going backwards)

`AGENTS.md`: *"Never fork or duplicate logic per engine."* This is being violated systematically.

**`IceRoadSystem`** — 393 LOC, exists **twice**, byte-identical except 5 lines:

| | |
|---|---|
| `Assets/_Game/Core/IceRoadSystem.cs` | 393 LOC — **still the one Unity uses** |
| `Ashfall.Core/IceRoadSystem.cs` | 393 LOC — used by nobody |

Total diff: 14 lines — `using UnityEngine` removed, namespace changed, one `Mathf.Clamp` →
`Math.Clamp`. Unity was never switched over, so the extraction **added** a maintenance burden
instead of removing one. Both copies will now drift.

**`JournalSystem`** — same pattern, and **already drifted**:

| | |
|---|---|
| `Assets/_Game/Events/JournalSystem.cs` | 377 LOC |
| `src/Journal/JournalSystem.cs` | 294 LOC |
| Divergence | **170 diff lines** |

The Godot copy is missing, entirely: `BindPersonalQuests`, `ApplyLorekeeperMoraleTick`,
`SetNeedsSystem`, `TickNewsAnchorJournalSpam`, and four public events (`OnEntryAdded`,
`OnNotificationPing`, `OnTabChanged`, `OnCodexUnlocked` exist in Unity's richer form).

Critically: Unity's `JournalSystem.cs` has **no `using UnityEngine`** — it was *already* portable.
Its single Unity dependency is one inline `UnityEngine.Mathf.Clamp` call. It could have been moved
to Core and shared. It was copied instead.

**The rule this implies:** extraction must be *move + reference*, never *copy*. A copy is only
progress if the original is deleted in the same change.

### C3 — Cross-host saves are incompatible (stated invariant violated)

`AGENTS.md`: *"A save written by one host MUST load in the other."* They cannot.

| | Unity | Godot |
|---|---|---|
| Serializer | `JsonUtility` | `System.Text.Json` (`IncludeFields`) |
| Shape | `JournalSave` nested in whole-game `SaveSystem.Dtos` snapshot | standalone `JournalSave` |
| File | game save + `Checksum` field | `user://journal_save.json` |
| Class | `AtomicWar._Game.Events.JournalSave` | `AtomicWar.Journal.JournalSave` |

Two independently-defined `JournalSave` DTOs in two namespaces. Neither host can read the other's
file — different container, different path, different serializer, and Unity's carries a checksum
Godot never writes.

The doc comment at `src/Journal/JournalSaveStore.cs:9-11` — *"so the save shape stays portable
between the Godot and Unity implementations"* — **is false as written** and should not be trusted.

### C4 — The Core test suite cannot run on this machine

`AGENTS.md` mandates: *"the `Ashfall.Core` test suite must run WITHOUT Unity (plain `dotnet test`)."*

```
Ashfall.Core.Tests → <TargetFramework>net8.0</TargetFramework>
Installed runtimes → Microsoft.NETCore.App 9.0.18, 10.0.10
Result → "You must install or update .NET to run this application."
```

It **compiles** (0 errors) but **cannot execute**. The engine-independent verification gate the
migration depends on is currently non-functional. Fix: retarget to `net9.0`/`net10.0`, or install
the .NET 8 runtime. `Ashfall.Core` itself targets `netstandard2.1` and is fine.

---

## HIGH

### H1 — The Godot port has no externally-runnable test coverage

`src/Journal/JournalSelfTest.cs` is good — 20 real assertions, all passing — but it lives *inside*
the game assembly and only runs via a manual launch flag. It is not `dotnet test`-discoverable and
cannot be gated in CI alongside the Unity suite (246 test files).

The self-test is also invisible by default: `OS.GetCmdlineUserArgs()` returns only args **after
`--`**, so `godot --headless --journal-selftest` silently does nothing. The working invocation is:

```bash
godot --headless --path . -- --journal-selftest
```

This is not documented in `CLAUDE.md`.

### H2 — 24 files exceed the 800-line limit

Against the project's own `<800` guidance:

| File | LOC |
|---|---|
| `Assets/_Game/Survivors/PersonalQuestSystem.cs` | **4,936** |
| `Assets/_Game/Inventory/Items/Item_WorldCatalog.Expanded.cs` | 3,117 |
| `Assets/_Game/Core/SaveSystem.Wiring.cs` | 2,660 |
| `Assets/_Game/Economy/DynamicEconomySystem.cs` | 1,816 |
| `Assets/_Game/Data/EncounterEventFactory.cs` | 1,553 |

`PersonalQuestSystem.cs` at 4,936 LOC is 6× the limit and is a direct migration blocker — it is a
dependency of `JournalSystem`, which is why the Godot Journal copy had to drop those features.

---

## MEDIUM — Godot port code quality (`src/Main.cs`)

| # | Location | Issue |
|---|---|---|
| M1 | `Main.cs:193` | `_diagnosticsLabel` is added as a **second child of `MarginContainer`**. MarginContainer gives every child the same full rect, so the diagnostics bar renders **overlapping the entire UI** instead of docking at the bottom. Needs a `VBoxContainer` wrapper. |
| M2 | `Main.cs:40-49` | `_Process` rebuilds an interpolated string **every frame** and calls `Engine.GetVersionInfo()` (allocates a Godot `Dictionary`) 60×/sec — for a value that never changes. Cache the version; throttle the label to ~2 Hz. |
| M3 | `Main.cs:216-217` | Full serialize + file write on **every** entry added and every tab change. During `JournalDemoHarness.Seed` this rewrites the save once per seeded entry, then again at line 249. Should debounce or save on close only. |
| M4 | `Main.cs:303` | Uses `DateTime.Now` — the exact call `Ashfall.Core/Ports.cs` bans (*"Simulation calendar. Never DateTime.Now."*). Display-only here, but it is the banned pattern and will be copied. |
| M5 | `Main.cs:12-22` | Fields declared `= null!` then null-checked anyway (`if (_diagnosticsLabel != null)`). The `null!` suppression is lying to the compiler; this is the source of much of the 56-warning noise. |
| M6 | `src/Journal/JournalSelfTest.cs` | The codebase's only empty `catch (Exception) { }`. Silent failure in the one place that reports correctness. |

---

## Migration reality check

Strict portability re-measured (counting fully-qualified `UnityEngine.`, `MonoBehaviour`,
`ScriptableObject`, `[SerializeField]` — not just `using` lines):

**244 / 1,307 files = 18.7% engine-agnostic** (the naive `using`-only scan reports 19.5%; 11 files
hide fully-qualified Unity references, `JournalSystem.cs` among them).

| Metric | Value |
|---|---|
| Godot share of total C# | ~0.9% |
| Subsystems with a Godot host | 1 of 24 (Journal) |
| Subsystems consuming `Ashfall.Core` | **0** |

---

## Recommended order of work

1. **Make `Ashfall.Core` real** (fixes C1). Add `<ProjectReference>` to `Ashfall.csproj`; add an
   `Ashfall.Core` asmdef reference on the Unity side. Until something consumes it, every further
   extraction is dead code.
2. **Unfork `IceRoadSystem`** (fixes C2). Delete `Assets/_Game/Core/IceRoadSystem.cs`, point Unity
   at the Core copy. This is the cheapest possible proof that move-not-copy works — the files are
   already 98% identical.
3. **Retarget `Ashfall.Core.Tests` to net9.0/net10.0** (fixes C4). One line; restores the
   verification gate.
4. **Unify `JournalSave`** (fixes C3), then move `JournalSystem` into Core and delete both copies.
   Requires breaking the `PersonalQuestSystem` dependency first.
5. **Split `PersonalQuestSystem.cs`** (4,936 LOC) — it is the gatekeeper for Journal, Survivors and
   Narrative migration.
6. Fix the Godot UI/perf items (M1–M3); they are small and self-contained.

## Standing rule to adopt

> Extraction to `Ashfall.Core` is **move + reference**, never copy. A change that leaves the
> original in place has not migrated anything — it has doubled the maintenance surface. If the
> original cannot be deleted in the same change, the extraction is not ready.
