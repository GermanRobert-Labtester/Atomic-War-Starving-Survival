# Code health sweep — 2026-09-05

**Mode:** read-only current-source assessment.
**Companion remediation:** T009–T020, T101–T129, and T181–T194 in the 200-task plan.

## Hotspot census

| Area | Current evidence | Risk / next move |
|---|---|---|
| Main host partials | `src/Main*.cs` totals about 20,190 lines; largest files are `Main.UiPanels.cs` (1,252), `Main.CampaignOwners.cs` (878), `Main.GameFlow.cs` (729), and `Main.Application.cs` (673). | Ownership is split by filename, not a constrained runtime interface. Extract domain owners after P0 composition repair. |
| Lifecycle triads | Registry has 112 save sections; host contains 111 save methods and 117 setup methods; triad gate passes with documented exceptions. | Keep registry-based validation, but repair generated architecture-map omission of `dynamic_quests`. |
| Nullability | 290 Core/host files suppress CS8618; global build settings suppress several nullable warnings. | Green compile is not enough; remove suppressions by DTO/session boundary. |
| Domain duplication | Inventory and radiation both model `WornGear`; a sanctioned conversion exists. | Consolidate semantics and retain one tested bridge. |
| Error policy | Catch policy passes: 483 catches checked; no undocumented empty catches. | Preserve this positive baseline while reviewing best-effort observability. |
| Engine boundary | Static Core scan found no active `using Godot`/`UnityEngine` violation. | Preserve core engine independence while refactoring host ownership. |

## Structural defects confirmed by composition

- The survivor exposure resolver has a circular provider path in Main composition.
- Dynamic UI panels can fail despite scene-binding/lifecycle subsets passing.
- Save/restore state-change events can synchronously invoke host save work before aggregate restoration is complete.

These are ownership/lifecycle defects, not candidates for a broad cosmetic refactor. Repair and test the causal paths first.

## Guardrails for refactors

1. Keep Core engine-agnostic and place only presentation/wiring in Godot host owners.
2. Move a vertical owner (setup, command routes, save/restore, event lifecycle, tests) together; do not scatter a partial-class extraction across unrelated files.
3. Add dependency/contract tests before changing public session ownership.
4. Require full-shell and campaign save/resume verification for every extracted owner.
