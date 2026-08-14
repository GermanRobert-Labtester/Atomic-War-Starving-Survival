# UTILITY AI — Phase B Audit (pre-port)

Date: 2026-08-15. Source of truth: `Assets/_Game/AI/` (829 LOC core + 93 action files).

## Dependency map
- `UtilityAI.cs` (UnityEngine, Survivor) — selection loop; ONLY `Mathf.Max` + event + context.Random couple it to Unity.
- `ActionScorer.cs` (UnityEngine, Survivor) — pure scoring pipeline; only `Mathf.Clamp01/Max` couple it.
- `SurvivorAction.cs` (UnityEngine/ScriptableObject, Survivor) — `EvaluateRaw(AIContext)` abstract; AnimationCurve response curve.
- `AIContext.cs` (UnityEngine, Survivor/Shelter/Inventory/Medical/Radiation/Simulation) — fat context; needs + traits + flags.
- 93 `Actions/Action_*.cs` — the 4 crossing actions (WeighGoods/ReadContract/CanvasSupport/RunVouch, ~168 LOC total) are data-shaped (baseScore + skill bonus + fatigue gate).
- Engine-agnostic already: none of the AI layer. Consumers: GameBootstrap (Unity), ExpeditionSystem (Unity).

## Findings

| # | Layer | Severity | Location | Finding | Disposition |
|---|---|---|---|---|---|
| A1 | Determinism | HIGH | UtilityAI.cs:56-59 | Selection noise uses `context.Random` (System.Random) — hidden RNG, not host-deterministic, not seedable cross-process. | Fixed in port: caller-supplied ISeededRng with the same 0.0001 scale. |
| A2 | Determinism | MED | UtilityAI.cs:41-67 | Tie-breaking depends on candidate list order + noise; cross-host list order is a caller contract, not guaranteed. | Port documents candidate order as the caller's deterministic contract; ties are first-wins. |
| A3 | Correctness | MED | ActionScorer.cs:30-35 | Override actions return `Max(0, score)` unclamped — by design (must beat any 0..1 action) but untested. | Port keeps semantics; test asserts override wins. |
| A4 | Correctness | LOW | ActionScorer.cs:23-25 | `responseCurve.length > 0` guard; null/empty curve passes raw through. | Port: empty curve -> identity passthrough, tested at bounds. |
| A5 | Correctness | LOW | ActionScorer.cs:46-49 | Listless penalty 0.08 applied after curve — cannot inflate low-urgency scores. | Port keeps exact constant; boundary test at score 0.05 (penalty floors at 0 via clamp). |
| A6 | Save/load | INFO | — | Stateless system (actions are definitions; selection has no mutable state). No versioned save needed. | Port adds no save state; documented. |
| A7 | Error handling | LOW | ActionScorer.cs:12-13 | Null action/context -> 0f. Null quests -> no vetoes. | Port keeps null-safety; tests cover. |
| A8 | Test quality | HIGH | — | Zero test coverage for the entire AI layer (829 LOC + 93 actions). | Port builds full suite; probes in Phase F. |

Critical/high (A1, A8): fixed by the port itself (ISeededRng + full test suite).
Medium (A2, A3): semantics preserved + tested.
Low (A4, A5, A7): preserved + tested.

## Post-port finding (Phase C slice 1)

| # | Layer | Severity | Location | Finding | Disposition |
|---|---|---|---|---|---|
| A9 | Correctness | HIGH | UtilityAI.cs:63-66 (Unity) | Selection accepts score 0 as a winner: `bestScore` starts at -1, so a hard-vetoed action (score 0) beats the initial sentinel and is selected when ALL candidates are vetoed — a survivor with a hard veto executes the vetoed action. | Fixed in the port (`score > 0f && score > bestScore`); regression test `Selection_AllVetoedReturnsNull`; documented as an intentional Unity-defect fix. |
