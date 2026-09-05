# Utility Action Regression Matrix

> **Regression & Verification Matrix:** 20 test scenarios validating the 20-action catalog, trait vetoes, scoring, curves, and determinism.

---

| # | Test Scenario | Subsystem | Expected Outcome | Status |
|---|---|---|---|---|
| 1 | **Catalog Load Count** | `UtilityActionCatalogLoader` | Exactly 20 actions loaded from `utility_actions.json` | Verified |
| 2 | **Baseline 6 Preserved** | `UtilityActionCatalogLoader` | First 6 actions match original baseline IDs and fields | Verified |
| 3 | **14 New Actions Present** | `UtilityActionCatalogLoader` | Exactly 14 new action IDs present and unique | Verified |
| 4 | **No Duplicate IDs** | `UtilityActionCatalogLoader` | All 20 action IDs are unique with prefix `action_` | Verified |
| 5 | **Positive Base Scores** | `UtilityActionDef` | All 20 actions have `baseScore > 0` | Verified |
| 6 | **Positive Weights** | `UtilityActionDef` | All 20 actions have `weight > 0` | Verified |
| 7 | **Valid Fatigue Gates** | `UtilityActionDef` | All 20 actions have `fatigueGate >= 0` and <= 100 | Verified |
| 8 | **Response Curve Integrity** | `ResponseCurve` | All curves sort ascending `x` and interpolate monotonically | Verified |
| 9 | **Dead Survivor Gating** | `UtilityActionScorer` | Dead survivor (`IsAlive == false`) scores 0 on all actions | Verified |
| 10 | **Fatigue Gating** | `UtilityActionScorer` | Survivor with fatigue > gate scores 0 on gated action | Verified |
| 11 | **Rest Exemption** | `UtilityActionScorer` | `action_rest` has gate 0 and scores positively under high fatigue | Verified |
| 12 | **Coward Trait Veto** | `UtilityActionScorer` | Coward refuses `action_repair_equipment` (`loud_labor`) | Verified |
| 13 | **God Complex Trait Veto** | `UtilityActionScorer` | God Complex refuses `action_preserve_food` (`menial_labor`) | Verified |
| 14 | **Pacifist Trait Veto** | `UtilityActionScorer` | Pacifist refuses `action_stand_watch` (`weapon`) | Verified |
| 15 | **Hitman Trait Veto** | `UtilityActionScorer` | Hitman refuses `action_treat_wounded` (`medical_triage`) | Verified |
| 16 | **Germaphobe Hazmat Gate** | `UtilityActionScorer` | Germaphobe refuses triage without hazmat, allows with hazmat | Verified |
| 17 | **Ex-Con Order Veto** | `UtilityActionScorer` | Ex-Con refuses `action_resolve_conflict` (`order`) | Verified |
| 18 | **Skill Bonus Scaling** | `UtilityActionScorer` | Skilled survivor scores higher than unskilled on skill actions | Verified |
| 19 | **Deterministic Noise Tie** | `UtilityAiSystem` | Same seed and state produce identical chosen action | Verified |
| 20 | **Host Session Evaluation** | `UtilityAiHostSession` | Evaluates demo survivor across all 20 actions without errors | Verified |
