========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# C-3 Remediation Plan — AI Action Wiring

## Goal
Add a `SurvivorActionSO` for each player-facing system added in Prompts #119-#178 that
doesn't have one. The action must:
1. Score > 0 only when the system has work to do.
2. Call the right system method in `Execute(AIContext)`.
3. Be wired into `GameBootstrap.CreateAction<T>()` + `Actions` list.
4. Have a test that verifies `EvaluateRaw` + `Execute` behavior.

## Systems → Actions to add

| System | Action SO | File | Priority |
| --- | --- | --- | --- |
| ExcavationSystem | `ExcavateActionSO` | new | High — gameplay |
| CompostSystem | `CompostWasteActionSO` | new | Medium |
| SterilizationSystem | `BoilToolsActionSO` | new | Medium |
| ChelationSystem | `BeginChelationActionSO` | new | Low (5-day commitment) |
| WindTurbineSystem | `BuildWindTurbineActionSO` | new | Medium |
| InternalHaulingSystem | `HaulLootActionSO` | new | High — gameplay |
| AirlockSystem | `DeconAndEnterActionSO` | new | High — defense |
| EscapeHatchSystem | `ExcavateEscapeHatchActionSO` | new | Low (endgame) |
| HiddenStorageSystem | (storage only — no action needed) | n/a | n/a |
| MaterialShieldingSystem | `UpgradeShieldingActionSO` | new | Low |
| TunnelingSystem | `TunnelActionSO` | new | Medium |

That's 10 new Action SOs. Each is ~50-100 LOC. The existing AI action pattern is
`[CreateAssetMenu]` + constructor sets `id/displayName/description/basePriority` +
override `EvaluateRaw(AIContext)` + `Execute(AIContext)`.

## File plan
- `Assets/_Game/AI/Actions/ExcavateActionSO.cs` (new)
- `Assets/_Game/AI/Actions/CompostWasteActionSO.cs` (new)
- `Assets/_Game/AI/Actions/BoilToolsActionSO.cs` (new)
- `Assets/_Game/AI/Actions/BeginChelationActionSO.cs` (new)
- `Assets/_Game/AI/Actions/BuildWindTurbineActionSO.cs` (new)
- `Assets/_Game/AI/Actions/HaulLootActionSO.cs` (new)
- `Assets/_Game/AI/Actions/DeconAndEnterActionSO.cs` (new)
- `Assets/_Game/AI/Actions/ExcavateEscapeHatchActionSO.cs` (new)
- `Assets/_Game/AI/Actions/UpgradeShieldingActionSO.cs` (new)
- `Assets/_Game/AI/Actions/TunnelActionSO.cs` (new)
- `Assets/Tests/EditMode/AiActionTests.cs` (new — 10 tests, one per action)
- `Assets/_Game/Core/GameBootstrap.cs` (modify — add 10 to Actions list)
- `Assets/_Game/Core/AIContext.cs` (modify — add hooks for new systems)

## Risk
- AIContext is a shared scratch object in GameBootstrap; new bindings must not
  introduce a per-frame allocation.
- `Execute(AIContext)` is called for the top-scoring action each survivor each
  substep. A bad action that throws will sink the entire survivor decision pass.
- ExcavationSystem and TunnelingSystem need an "active" hook on AIContext (the
  survivor doing the work) so Execute can call them. The action can pull this
  from `context.Survivor`.
