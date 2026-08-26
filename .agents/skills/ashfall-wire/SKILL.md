---
name: ashfall-wire
description: Automates Godot scene/panel wiring — adds SetupXxx/SaveXxx/FlushXxx triad to Main.cs, registers in Main.UiPanels.cs, creates UiPreview scene, and adds --*selftest verb. For when the AI already knows the panel/scene structure.
---

# ASHFALL Godot Wiring Assistant

## ROLE

You eliminate the repetitive boilerplate of wiring new panels/scenes into the Godot host. The AI already knows the panel's purpose, layout, and dependencies — you just execute the wiring pattern.

## SCOPE

- **Input**: Panel/scene name (e.g., `MedicalTriagePanel`), dependencies (e.g., `MedicalSystem`, `InventorySystem`)
- **Output**: Wired `Main.cs` triad, `UiPreview` scene, `--*selftest` verb, and verification that the panel loads headless
- **Constraints**: `dotnet` + `godot --headless` only; never Unity

## WORKFLOW

### PHASE 1 — Triad Wiring
- Add `SetupXxx()`, `SaveXxx()`, `FlushXxxIfDirty()` to `Main.cs` (partial class per domain)
- Register in `Main.UiPanels.cs` (panel lifecycle wiring)
- Add to `Main.SaveAll()` and `Main.FlushAllIfDirty()`

### PHASE 2 — UiPreview Scene
- Create `scenes/UiPreview_Xxx.tscn` with the panel as root node
- Wire dependencies via mocks/fixtures (deterministic state)

### PHASE 3 — Selftest Verb
- Add `--xxx-selftest` verb to `Main.cs` (headless render + snapshot capture)
- Register in `scripts/ci/godot-asset-gate.sh`

### PHASE 4 — Verify
- `dotnet build Ashfall.csproj` (0 errors/0 warnings)
- `godot --headless --path . -- --xxx-selftest` (0 errors)
- `./scripts/ci/godot-asset-gate.sh` (asset registry green)

## CONSTRAINTS
- Never invent gameplay logic — only presentation wiring
- Never touch `Assets/Ashfall.Core/` — Core is authoritative
- Always use `ISeededRng` for deterministic state in mocks

## OUTPUT
`docs/wiring/WIRE_REPORT_<name>.md` — triad diff, UiPreview scene path, selftest verb, verification results

## QUALITY GATE
- Triad wired (Setup/Save/Flush)
- UiPreview scene loads headless (0 errors)
- Selftest verb exits 0
- No gameplay logic added
