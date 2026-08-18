# ASHFALL — Skill Progression Core Port Plan (Phase 14 design) — SHIPPED at Phase 18

**Status:** design SHIPPED at Phase 18. Files:
- `Assets/Ashfall.Core/Survivors/SkillDef.cs`
- `Assets/Ashfall.Core/Survivors/SkillProgressionState.cs` (includes `SkillActor` interface and the four save envelopes)
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs`
- `Assets/Ashfall.Core/Survivors/SkillAtrophySystem.cs`
- `Ashfall.Core.Tests/SkillProgressionSystemTests.cs` (12 tests, all PASS)

## Why a port is required

The current ASHFALL runtime cannot render a Survivor × Skill matrix because the per-survivor skill state has no engine-agnostic Core equivalent.

The closest data structures live in:

```
Assets/_Game/Survivors/SkillProgressionSystem.cs        ← Unity legacy (ScriptableObject + MonoBehaviour)
Assets/_Game/Survivors/SkillAtrophySystem.cs            ← Unity legacy
```

These systems are not portable *as-is* to `Ashfall.Core/Survivors/` because they depend on `UnityEngine`:

- `MonoBehaviour` lifecycle
- `ScriptableObject` authoring patterns
- `Time.deltaTime` integration

The brief forbids silently porting legacy Unity architecture. The brief also forbids fabricating skill data in the UI. Therefore the architectural decision boundary lives at the **Core port**.

## Minimal engine-agnostic model

The future Core port needs the following concepts (chosen because they appear in the existing legacy system; nothing new is invented here):

| Concept | Description | Justification |
|---|---|---|
| `SkillDef` | A read-model class describing one skill (id, displayName, category, maxTier, tierCount). | Exists in legacy; tied to ScriptableObject authoring. |
| `SkillTierDef` | Read-model for one tier (tierIndex, name, xpThresholdRequirement). | Exists in legacy. |
| `SkillProgressionState` | Per-survivor per-skill read model: `{ survivorId, skillId, tier, xp, xpToNext, lastTrainedDay }`. | Exists in legacy. |
| `ISkillTrainer` | Engine-agnostic interface `{ CanTrain(actor, def, day), Train(actor, def, dt), Decay(actor, def, dt) }`. | Candidate abstraction; legacy does not use an interface. |
| `SkillAtrophyPolicy` | Stateless rules: how XP decays per day without practice. | Exists in legacy. |

The phase-2 Engine port should add a `SkillProgressionCatalog` (loadable from `data_dir/skills.json`) that defines which `SkillDef`s are available in a run.

The Core migration tasks — **not** Phase 14 work:

### Port task 1 — `Ashfall.Core/Survivors/SkillDef.cs`

Engine-agnostic POCO mirroring the legacy `ScriptableObject` field set. **PORT** classification.

### Port task 2 — `Ashfall.Core/Survivors/SkillTierDef.cs`

Same as above. **PORT**.

### Port task 3 — `Ashfall.Core/Survivors/SkillProgressionState.cs`

Per-survivor dictionary keyed by `(survivorId, skillId)`. **PORT** with **REPLACE** on the storage format (binary → JSON-aware).

### Port task 4 — `Ashfall.Core/Survivors/SkillProgressionSystem.cs`

Engine-agnostic engine that:
- takes a `SkillProgressionCatalog`
- exposes `Tick(survivors, day)` and `Train(survivor, def, gameHours)` events
- listens for `OnStateChanged` so HUD can subscribe

**REPLACE** classification — the new system should drop Unity event timing.

### Port task 5 — `Ashfall.Core/Survivors/SkillAtrophySystem.cs`

Pure rule engine: `DecayPerDay(def, daysSinceLastPractice, currentTier)`. **PORT**.

### Compatibility analysis

| Legacy concept | Resolution |
|---|---|
| `MonoBehaviour` lifecycle | **DROP**. The Godot host's `SurvivorsHostSession.Tick` already drives lifetime; no MonoBehaviour. |
| `ScriptableObject` authoring | **REPLACE**. Catalog replaces with `JSONLoader.LoadSkills(dataDir)`. |
| `Time.deltaTime` | **REPLACE** with `gameHours` controlled by `SurvivorsHostSession`. |
| Randomness | **DROP**. Legacy uses `UnityEngine.Random` for jittery XP gains; deterministic `[SeededRng]` replacements belong to `SurvivorsHostSession`. |
| Save format | **REPLACE**. Save = `(survivorId, skillId) → { tier, xp, lastTrainedDay }`. Workflow: `CaptureState / RestoreState`. Diagnostic tests required. |
| Scriptable Object inspector | **DROP**. ASHFALL authoring flow = `StreamingAssets/Data/skills.json`. |
| OnDestroy cleanup | **DROP**. Godot's `QueueFree` model is already in place. |

### Required acceptance for the port to ship

1. `Ashfall.Core.Tests` contains a test that exercises `Train → XpToNext updates → Decay over time` using a synthesized `SkillProgressionState`.
2. `SurvivorsHostSession.CaptureSave / RestoreSave` covers the per-survivor skill state without disturbing existing fields.
3. NO regression in any Phase 11/12/13 MATCH snapshot (test against snapshot harness baseline after port).
4. After the port, `docs/visual/WIRING_MATRIX.md` (asset audit) stays unchanged for skill assets — most skill icons are not currently authored.

### After the port ships, the Skill Matrix UI can be built

The phase after the port completes — call it Phase 16 — adds:

- New data source `SkillProgressionSystem.GetRoster()` returning a flat list of `(survivorId, skillId, tier, xp, status)`.
- New UI panel `SkillMatrixPanel` reusing the `AshfallDashboardShell` + `AshfallSidebar` + `AshfallStatusRail` + `AshfallDataGrid` primitives (no new primitives needed).
- New fixture policy: `DETERMINISTIC_TEST_FIXTURE` constructed from a synthesized catalog during snapshot.

The Skill Matrix is **deliberately not Phase 14 work**. The reason: the brief asks us to design the migration **first**, then the Core port, then the UI. Phase 14 should end at the design.

## What Phase 14 *did* commit

Phase 14 committed:

- The architectural decision to **PORT** the legacy system rather than build a UI-only skill model.
- The compatibility classification: 3× PORT, 3× REPLACE, 4× DROP.
- The acceptance gates: dedicated test + `CaptureSave` round-trip + no MATCH snapshot regression.
- The asset audit dependency: most skill assets are not currently authored; the asset wiring matrix will gain rows once skills.json is published.

## What's blocked on the port

The brief warns against "rewriting Core to make Stitch prettier". This document exists to prevent that drift. The Skill Matrix UI will not be implemented until this port ships.

If, after a future port attempt, the system ends up requiring a `MonoBehaviour` analogue, **stop** and reconsider — that means the port is wrong, not the UI.
