# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

## Stub Content Scripts, Scene-Binding Truthfulness & CI Guard (P089–P096)

STATUS: APPROVED BY USER (2026-10-01)

User directive (2026-10-01): "Please start working, coding on these tasks aswell as
repair any leaks, bugs, missing tool calls or recieving tool calls, missing panels,
missing UI animations, warnings, errors! make sure that after coding in these tasks
you run a sweep loop of find issue fix issue repeat for 5 loops!" — rows P089–P096.

## Verified premise (Rule 7 — evidence before change)

All six `src/UI/*Content.cs` shims are one-line `public partial class X : Control { }`
stubs. A headless Godot probe (`PackedScene.instantiate()` + `get_script()`) proved
which scenes actually **attach** their declared script:

| Scene | declared `ext_resource` script | root `script =` assignment | live production consumer | verdict |
|---|---|---|---|---|
| `panels/WaterTreatmentPanel.tscn` | `WaterTreatmentPanelContent.cs` | **YES** | `WaterTreatmentPanel._Ready()` → `PanelSceneLoader.Load<WaterTreatmentPanelContent>` + `SceneBinder` | **LIVE shim — keep** |
| `modals/DailyBriefingModal.tscn` | `DailyBriefingModalContent.cs` | **NO** (`script=<none>`) | none — production asks for `DailyBriefingModal` | **DEAD** |
| `modals/OpeningProtocolModal.tscn` | `OpeningProtocolModalContent.cs` | **NO** | none — `new OpeningProtocolModal()` (`Main.UiPanels.cs:1115`) | **DEAD** |
| `modals/SafeCrackModal.tscn` | `SafeCrackModalContent.cs` | **NO** | none — `SafeCrackModal` builds UI in C# | **DEAD** |
| `panels/KitchenNutritionPanel.tscn` | `KitchenNutritionPanelContent.cs` | **NO** | none — `new KitchenNutritionPanel()` (`Main.ShelterBatch3.cs:155`) | **DEAD** |
| `panels/PharmaLabPanel.tscn` | `PharmaLabPanelContent.cs` | **NO** | none — `new PharmaLabPanel()` (`Main.UiPanels.cs:383`) | **DEAD** |

Known-good comparison (`CraftingPanel.tscn`, `WorkshopPanel.tscn`, `MapDetailPanel.tscn`):
each declares **and assigns** its script, and each is loaded with the matching type.

### Root-cause defect found (P091) — worse than "silently empty"

`src/Main.Campaign.cs:115`
`PanelSceneLoader.Load<DailyBriefingModal>("res://assets/ui/modals/DailyBriefingModal.tscn")`
asks for `DailyBriefingModal`, but that scene's root is a **plain `Control` with no
script**. IL proof from `GodotSharp 4.7.1` (`ikdasm`):

- `PackedScene.Instantiate<T>` → `unbox.any !!T` = **hard cast, throws `InvalidCastException`**
- `PackedScene.InstantiateOrNull<T>` → `isinst` = returns null

So the call throws `InvalidCastException` before `PanelSceneLoader`'s own null-check can
produce its actionable `SceneBindingException`. `SetupDailyBriefingModal()` has no
`try/catch`, and its caller `ShowBriefingForDay()` is invoked from `Main.Holdfast.cs:304`
inside `try { … } finally { … }` — **no catch**. Every player-facing day advance throws
and the briefing never renders.

`SceneBindingSelfTest` did not catch it: `Run()` loads as `Node` and passes `RootType`
only as a *diagnostic label* to `SceneBinder`; it never asserts the root's actual script
type matches what production requests. That is the blind spot P096 must close.

## Bounded outcome

- **P089/P090** — Delete the five truly-dead shims; keep the one proven-live shim
  (`WaterTreatmentPanelContent`). Remove each dead scene's dangling
  `[ext_resource type="Script"]` (a reference to a file that no longer exists) and fix
  `load_steps`.
- **P091** — `Main.Campaign.cs` constructs `new DailyBriefingModal()`, matching every
  other modal and `UiAccessibilitySelfTest`. Its C# `_Ready()` already builds the
  complete real UI (scrim, frame, title, skip, scroll, BBCode body, deep-link row,
  footer, ack) with typewriter reveal and keyboard/controller handling.
- **P092** — Verified true, no change needed: the scene supplies `ContentStack`,
  `DetailText`, `CharcoalButton`, `DistillButton`, `OsmosisButton`,
  `ReplaceFilterButton` (all `unique_name_in_owner = true`) and the host uses
  `ShowPanelLifecycle`, so the absent `Open()` is not a defect. Pinned by an assertion.
- **P093** — Verified: `KitchenNutritionPanel` builds its own UI in C# and hosts the
  cooking strip through `BindCooking` (T19, released). Placeholder cleanup only —
  `src/UI/KitchenNutritionPanel.cs`, `src/Main.Cooking.cs`, `src/Main.ShelterBatch3.cs`
  are **not touched**.
- **P094** — Empty-surface guard: `PanelSceneLoader.Load<T>` uses `InstantiateOrNull<T>`
  and throws a truthful `SceneBindingException` naming the scene root's *actual* script
  type instead of leaking a raw `InvalidCastException`; new
  `SceneBinder.RequireNonEmptySurface()` makes an empty bound content root fail loudly.
- **P095** — Delete the five orphan `.cs.uid` sidecars with their scripts; keep
  `WaterTreatmentPanelContent.cs.uid`. `scripts/ci/uid-sidecar-gate.sh` re-run.
- **P096** — New `scripts/ci/scene-binding-truth-gate.sh` + xUnit gate covering three
  regression classes: (1) stub `*Content.cs` that is not a proven live scene root,
  (2) a scene declaring an `ext_resource` script it never assigns, (3) a production
  `PanelSceneLoader.Load<T>(scene)` whose requested `T` the scene root's attached script
  does not satisfy. Registered in `docs/ci/CI_GATE_MANIFEST.json`.

## Non-goals

- No rewrite of `OpeningProtocolModal`, `SafeCrackModal`, `KitchenNutritionPanel`,
  `PharmaLabPanel`, or `DailyBriefingModal` to scene-bound form (would duplicate their
  working C# UI and regress live, tested surfaces).
- No deletion of the five `.tscn` designer mirrors — `SceneBindingSelfTest` and
  `--ui-layout-selftest` still validate their unique-name contracts.
- No change to the T19 cooking wire, no new save section, no Core gameplay change.
- `SafeCrackModal` production reachability (never instantiated) is reported as a
  finding, not silently "fixed" — it needs its own authority check.

## Exact files

Delete: `src/UI/{DailyBriefingModalContent,OpeningProtocolModalContent,SafeCrackModalContent,KitchenNutritionPanelContent,PharmaLabPanelContent}.cs`
and their `.cs.uid` sidecars.
Edit: `assets/ui/modals/DailyBriefingModal.tscn`, `assets/ui/modals/OpeningProtocolModal.tscn`,
`assets/ui/modals/SafeCrackModal.tscn`, `assets/ui/panels/KitchenNutritionPanel.tscn`,
`assets/ui/panels/PharmaLabPanel.tscn`, `src/Main.Campaign.cs`, `src/UI/PanelSceneLoader.cs`,
`src/UI/SceneBinder.cs`, `src/UI/WaterTreatmentPanel.cs`, `src/Host/SceneBindingSelfTest.cs`,
`docs/ci/CI_GATE_MANIFEST.json`.
New: `scripts/ci/scene-binding-truth-gate.sh`,
`Ashfall.Core.Tests/Tooling/SceneBindingTruthGateTests.cs`.
Regenerate (never hand-edit): `docs/ui/ui_design_map.json`, `docs/ui/UI_DESIGN_MAP.md`.
Governance: this plan, `WORKTREE_OWNERSHIP.md` claim, `.ai/state.md`.

## Verification

`dotnet build Ashfall.csproj` 0/0; `scripts/ci/scene-binding-truth-gate.sh`;
`scripts/ci/uid-sidecar-gate.sh`; `--scene-binding-selftest`; `--ui-layout-selftest`;
`--ui-accessibility-selftest`; focused xUnit via `bin/run-scoped-tests`
(SceneBindingTruthGate, CiGateManifestDrift, ArchitectureTestMap);
`generate-ui-design-map.py --check`; `git diff --check`. No full suite.

## Follow-up integration — five scene-binding hardening tasks (2026-10-02)

1. **Runtime mismatch probe:** `--scene-binding-selftest` invokes the actual generic
   `PanelSceneLoader.Load<T>` through reflection with the intentionally unbound
   Daily Briefing scene, then verifies `SceneBindingException` reports the scene,
   requested type, and `<no Script>` actual root. The regression cannot silently
   become a bare `InvalidCastException`.
2. **Dynamic path coverage:** the static scene audit resolves literal arguments and
   preceding local/`const` string aliases, rejects unresolved runtime expressions,
   and has a fixture for both cases. The runtime self-test also loads Water Treatment
   through a path held in a local string variable.
3. **Post-bind visibility:** scene self-test calls `RequireNonEmptySurface` for each
   `*Content` root after its typed bindings resolve, catching hidden or childless bound
   content at runtime.
4. **Generated binding report:** the self-test emits `[SCENE_BIND_REPORT]` rows mapping
   each audited scene to its production request type, actual root type, and attached
   script path (or `<no script attached>`).
5. **Actionable diagnostics:** runtime mismatch output and unresolved dynamic-path
   failures name the scene/type/expression and explain the verification failure.

**Verification:** host build 0 warnings / 0 errors; `--scene-binding-selftest` 25/25
scene contracts plus both runtime probes passed; `--ui-layout-selftest` 0 failures;
`--ui-accessibility-selftest` 6/6; `scene-binding-truth-gate.sh` 0 violations / 27
scenes; UID sidecar gate 0 dangling; UI design map `--check` in sync (240 panels).
Scoped xUnit targets: `SceneBindingTruthGateTests` 6/6,
`CiGateManifestDriftTests` 7/7, `ArchitectureTestMapGateTests` 7/7. The first focused
xUnit attempt exposed the project's concurrent expedition test compile mismatch; that
worktree state subsequently compiled and all focused targets passed without editing
expedition paths. `git diff --check` clean. No full suite or commit.

Adjacent observation: accessibility self-test logged the missing localization key
`ui.expedition.railway_tooltip` while all six accessibility gates passed. This is
outside P089–P096 and was left to its owner.
