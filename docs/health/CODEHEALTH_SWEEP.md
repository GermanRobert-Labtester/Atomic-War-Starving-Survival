# Codehealth Sweep — ThemeHelper Facade & harvestable_materials Vocabulary Regression Check

**Scope:** Read-only structural-health sweep triggered after two changes landed:
1. `src/UI/ThemeHelper.cs` — new compatibility facade over `AshfallUiHelpers`, plus `MarginContainer.ThemeOverrideConstants` → `AshfallUiHelpers.MakeMargins(16)` across 7 panels, plus a `entry.P0` → `entry.confidence:P0` typo fix in `WeatherSondePanel.cs`.
2. `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` — `harvestable_materials` added to `VocabularyKeys`; `flag_exp07_vel_vigil_knock` added to `KnownRuntimeIds`.

**Question to answer:** Did these changes introduce structural-health regressions, and should the 7 panels be refactored to call `AshfallUiHelpers` directly (dropping the facade)?

---

## Health Score Table

| Area | Metric | Before | After | Verdict |
|---|---|---|---|---|
| God-class growth | `src/Main.cs` lines | 715 | 715 | unchanged |
| New file size | `src/UI/ThemeHelper.cs` | — | 69 | small, acceptable |
| Duplication | `ThemeHelper` vs `AshfallUiHelpers` | — | forward-only | no logic fork (unlike `WornGear`) |
| Fan-in | `ThemeHelper.*` call sites | 0 (broken) | 126 (compiles) | restored, not new coupling |
| Fan-in | `AshfallUiHelpers.*` direct callers | 132 | 132 | unchanged |
| Core engine refs | `using Godot/UnityEngine` in `Assets/Ashfall.Core` | 0 | 0 | Invariant 1 holds |
| Bare catches | in changed files | 0 | 0 | none added |
| Build warnings | `dotnet build Ashfall.csproj` | 124 (pre-existing) | 0 (clean) | improved |
| Build errors | `dotnet build Ashfall.csproj` | 134 | 0 | fixed |
| Data integrity | `--data-integrity-selftest` | PASS (TopOnly) | PASS (TopOnly) | holds |
| Test suite | `dotnet test` Core | 2796 pass | 2838 pass | holds, no regressions |

---

## Hotspot Map

### `ThemeHelper` facade — NOT a new `WornGear`
`WornGear` is the sanctioned duplication case (`Inventory.WornGear` + `Radiation.WornGear` with a single `FromInventory()` bridge). `ThemeHelper` is **not** in that category:
- It contains **zero** logic of its own. Every method body is a single forward to `AshfallUiHelpers` (`MakeBody`, `MakeSeparator`, `MakeButton`) or a 3-line composition of `AshfallUiHelpers.ToColor` + `ApplyFont` + `Theme` tokens.
- No second source of truth for fonts/colors/sizes: tokens come from `Ashfall.Core.UI.Theme`, same as `AshfallUiHelpers`.
- Risk surface is the facade's 4 methods vs the canonical 30+ — it is strictly smaller.

### Fan-in balance
- `ThemeHelper` is consumed by **7 panels, 126 call sites**.
- `AshfallUiHelpers` is consumed directly by **132 call sites** across ~10 panels/helpers.
- The facade did **not** grow `AshfallUiHelpers`'s fan-in; it absorbed an already-broken dependency. No new coupling was introduced — the coupling already existed, it just didn't compile.

### `CatalogIntegrityValidator` change — vocabulary, not semantics
- `harvestable_materials` moved to `VocabularyKeys`: this is a *classification* correction, not a relaxation. The field is opaque `string[]` flavor in `WastelandBestiaryCatalog` (confirmed: no code resolves it against `items.json`). Treating it as TIER-1 cross-refs was a false positive — the validator was over-checking narrative flavor.
- `flag_exp07_vel_vigil_knock` added to `KnownRuntimeIds`: matches the existing `flag_verdict_*` pattern for runtime-set flags gated by future expansion code. Documented inline.
- Coverage: `WastelandBestiaryCatalogTests` (8 tests) still pass and already assert `harvestable_materials.Length > 0`; `CatalogIntegrityValidatorTests` pass.

---

## Duplication Clusters (unchanged from known debt)

| Cluster | Status | Action |
|---|---|---|
| `Radiation.WornGear` vs `Inventory.WornGear` | sanctioned bridge only | no change |
| `HoldfastRuntimeSession` vs Core survival | H1 known debt | no change |
| `ThemeHelper` vs `AshfallUiHelpers` | **forward-only facade, no logic** | see refactor recommendation below |

---

## Bare-Catch Inventory (unchanged)
H4 debt (13 bare `catch{}` in `YearOfAshCatalogLoader`/`VerdictCatalogLoader`) — not touched by this task. No bare catches added in changed files.

---

## Triad Drift (Main.cs)
`src/Main.cs` 715 lines, 4 triad-method matches (lower than the AGENTS.md H7 figure of 31 Setup/24 Save/17 Flush — the 6.5k-line god object appears to have been substantially decomposed in prior WIP). No drift introduced by this task; UI panel wiring is outside `Main.cs`'s triad.

---

## Ranked Refactor Backlog

| Prio | Item | Blast radius | Owner | Verify |
|---|---|---|---|---|
| **P2** | Drop `ThemeHelper` facade; migrate 7 panels to call `AshfallUiHelpers` directly | 7 files, 126 call sites | UI | `dotnet build Ashfall.csproj` + manual panel smoke |
| P3 | `WornGear` consolidation | Core, 2 files | Core | `InventoryGearBridgeTests` |
| P3 | `HoldfastRuntimeSession` mechanic dedup | host, 1 file | Host | `--survivors-selftest` |
| P3 | H4 bare-catch cleanup | 2 loaders | Core | `dotnet test` |

---

## Recommendation: Should the 7 panels drop the facade?

**Yes — as a follow-up, not now.**

### Why the facade was the right *landing* fix
- The 7 panels were **already broken** (134 build errors). The facade restored compilation with a 69-line, logic-free, forward-only shim in one new file, touching zero panel construction code.
- The alternative (rewriting 7 panels' `BuildUI()` + `.Pressed +=` wiring) is a **126-call-site** refactor with per-panel visual risk. Doing that *under a build-break* would have mixed "make it compile" with "make it idiomatic" — exactly the kind of change coupling the sweep flags.

### Why it should be retired as a follow-up
- The facade is a **second API surface** for the same design system. Even though it forwards, it is a name that new panels could be written against, slowly re-establishing the duplication the `AshfallUiHelpers` consolidation was meant to remove.
- The migration is mechanical and low-risk *now that the build is green*: each `ThemeHelper.CreateLabel(x)` → `AshfallUiHelpers.MakeBody(x)`, `CreateLabel(x, sz, bold)` → a direct `MakeLabel`/`MakeTitle` call, `CreateHSeparator()` → `MakeSeparator()`, `CreateButton(x)` → `MakeButton(x, handler)` (requires inlining the `.Pressed +=` into the `onPressed` arg — the only non-trivial part).
- The `CreateButton` overload is the one real friction point: `AshfallUiHelpers.MakeButton` takes `Action onPressed` inline, while the panels wire `btn.Pressed += ...` after. The migration must move each handler into the `MakeButton` call. This is local to each panel and reviewable per-file.

### Suggested follow-up sequencing
1. Migrate one panel (e.g. `BrineExtractionPanel`, smallest) to `AshfallUiHelpers` direct; delete its `ThemeHelper.*` calls.
2. `dotnet build Ashfall.csproj` + smoke the panel.
3. Repeat per panel.
4. Once all 7 are migrated, delete `src/UI/ThemeHelper.cs`.
5. Final `dotnet build` + `dotnet test` + `--data-integrity-selftest`.

---

## Quality Gate

- ✅ No new god-file growth (`Main.cs` unchanged at 715 lines).
- ✅ No unbridged duplication (`ThemeHelper` is forward-only, not a logic fork).
- ✅ Bare-catch count unchanged (0 added).
- ✅ Core still 0 `using Godot` / `UnityEngine` (Invariant 1 holds).
- ✅ Build warnings improved (134 errors → 0; warnings → 0 on clean build).
- ✅ Data integrity holds (`--data-integrity-selftest` PASS, 0/0 across 111 catalogs).
- ✅ Test suite holds (2838/2838 pass).

**Verdict: No structural-health regressions introduced.** The facade is an acceptable landing shim; retire it as a P2 follow-up by migrating the 7 panels to `AshfallUiHelpers` directly.
