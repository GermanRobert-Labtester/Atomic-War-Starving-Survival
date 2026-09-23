# PLAN-ORPHAN-SEAL-01 — Appendix AL: Compile-Surface Verification

**Generated:** 2026-09-21. Are the unreachable files actually built? Checked
against the game project's compile globs (`src/**/*.cs, Assets/Ashfall.Core/**/*.cs`) and the
test project's explicit includes.
**Finding:** the game project compiles `Assets/Ashfall.Core/**`, so **all 99
orphan files and all 147 blob files are compiled into the shipping
assembly** — they are not build-excluded. There are no csproj-only orphans.
**Consequence:** dead/unreachable code is present at runtime (types loadable,
potentially instantiated by reflection-free code paths only if referenced —
none are). Retirement of a blob family is therefore a build-size and clarity
change, not a wiring change; the seal path is unchanged.

| Check | Result |
|---|---|
| Game globs | 2 include pattern(s) |
| Test globs | 1 include pattern(s) |
| Orphan files under game glob | 99 / 99 |
| Blob files under game glob | 147 / 147 |
| Files excluded from every project | 0 |

