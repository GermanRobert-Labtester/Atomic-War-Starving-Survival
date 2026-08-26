---
name: ashfall-export-build
description: Runs headless Godot exports for ASHFALL's configured presets (Linux/Windows), verifies the PCK includes the JSON data authority, smoke-boots the binary, and reports size/regressions. The bridge between passing selftests and a shippable build.
---

# ASHFALL Export & Build Verifier

## ROLE

Passing selftests does not mean a shipped binary boots. `export_presets.cfg` defines Linux/X11 and Windows Desktop presets with `include_filter="*.json"` — if the data authority (`Assets/StreamingAssets/Data/`) misses the PCK, the game dies silently at runtime. You verify exports end to end.

## WORKFLOW

### PHASE 1 — Preset Audit
- Parse `export_presets.cfg`: presets, `export_path`, filters, texture formats, `embed_pck`.
- Confirm `include_filter` covers every data root and no `exclude_filter` swallows JSON.

### PHASE 2 — Pre-Export Gate
- `dotnet build Ashfall.csproj` must be 0 errors / 0 warnings first.
- `godot --headless --path . -- --data-integrity-selftest` must report 0 errors.
- Do not export with a dirty failure; report and stop.

### PHASE 3 — Export
- `godot --headless --path . --export-release "<preset>" <out>` for each configured preset (export templates must be present; report clearly if missing).
- Capture warnings; any `*.json` resource-skip warning is a finding.

### PHASE 4 — Content Verification
- For embedded PCK: list contents (`godot --headless ... --import` or PCK inspection) and confirm all `StreamingAssets/Data/**/*.json` present.
- For directory builds: diff the data tree against the source authority — file-by-file presence check.

### PHASE 5 — Smoke Boot
- Boot the exported binary headless/briefly where the platform allows; confirm the main scene initializes and no load-time fatal appears in the log.
- Record boot time and first-frame errors.

### PHASE 6 — Regression Table
- Build size per preset vs previous run (store prior sizes in `docs/builds/BUILD_SIZES.md`).

## RULES
- Headless everywhere. Never open the editor UI.
- Never edit `export_presets.cfg` beyond flagging issues, unless explicitly asked.
- Never commit binaries; `Builds/` is not versioned.

## OUTPUT
`docs/builds/EXPORT_REPORT.md` — per-preset: export status, data-in-PCK check, boot result, size table, warnings.

## QUALITY GATE
- Every data JSON accounted for in every shipped preset.
- Binary boots to first frame with 0 fatal errors, or report blocks release.
