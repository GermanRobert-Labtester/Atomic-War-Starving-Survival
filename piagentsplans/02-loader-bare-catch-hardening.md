# Plan 02 — Eliminate Bare `catch { }` in Catalog Loaders (closes H4)

## Goal (2 lines)
Close known issue H4: replace the 13 silent `catch { }` blocks in
`YearOfAshCatalogLoader.cs` (7) and `VerdictCatalogLoader.cs` (3) with logged, surfaced
failures so malformed JSON can never disappear silently.

## Files to touch
- `Assets/Ashfall.Core/YearOfAshCatalogLoader.cs`
- `Assets/Ashfall.Core/VerdictCatalogLoader.cs`
- New/extended tests in `Ashfall.Core.Tests/` (loader failure-surfacing tests)

## Rules (invariants)
- Core stays engine-agnostic: report through the injected `ILog` port, never `Console`.
- Do not change load-success behavior or DTO shapes — failure observability only.
- Malformed file ⇒ `ILog.Error` with file name + JSON path; valid file ⇒ identical output to today.

## Steps
1. Locate every bare `catch` in both loaders (`grep -n "catch" …`); classify each: parse
   error, missing file, or per-entry skip.
2. Replace with `catch (Exception ex)` → `_log.Error($"<loader>: failed to load <file>: {ex.Message}")`
   and keep the documented fallback (skip entry / empty catalog) so runtime behavior is
   unchanged for valid data.
3. Decide per site: should the error also fail `--data-integrity-selftest`? Prefer: missing
   optional file = Warn; malformed present file = Error counted by the validator.
4. Add tests: (a) malformed JSON → error logged, catalog empty/partial per policy;
   (b) valid JSON → byte-identical load result to pre-change baseline;
   (c) missing optional file → Warn only, no throw.

## Verification
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest   # still 0 errors on valid data
godot --headless --path . -- --bridge-selftest           # exits 0
```

## Risk
LOW — but verify no selftest depended on silent swallowing (run the full 118-flag battery if
any failure surfaces).

## Definition of Done
- 0 bare `catch { }` in both files; all failures observable via `ILog`.
- H4 marked RESOLVED in `AGENTS.md`.
