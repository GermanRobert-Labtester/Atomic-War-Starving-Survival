# D3 — Handoff

**Task:** Wave 8 Part 2, TASK D3 — Shutdown Cleanliness
**Status:** DONE (classified benign; documented)
**Date:** 2026-09-17
**Next owner:** Foreman/ledger. No production owner action required.

## Outcome

- The a11y-selftest shutdown `Resource`/`RID`/`ObjectDB` warnings are classified
  as **known-benign diagnostic-harness specimen lifetime**: a fixed 8 objects
  per run (flat ×3), while the production lifecycle path
  (`--panel-bind-lifecycle-selftest`) emits zero.
- Documented with the exact signature and a concrete distinguisher in
  `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` §4; Plan 24's routed bullet
  is closed with the classification.
- No production/test code and no rendering policy changed; no global exit-time
  cleanup introduced.

## Remaining debt

| Item | Disposition |
|---|---|
| `--player-panels-uitest` shutdown leaks (269 ObjectDB / 103 CanvasItem) | separate, larger diagnostic class; not implicated by D3; candidate for a future panel-lifetime package |
| a11y harness specimen leak | accepted Tier 3 noise; the guide's distinguisher guards against blanket-ignore |

## Rollback

- Revert the docs-only commit. No code/save/render impact.

## Verification rerun

```
godot --headless --path . -- --ui-accessibility-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-docs-index.py --check
```
