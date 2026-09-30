# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> STATUS: APPROVED BY USER ("continue with the next open!", 2026-09-30)

# Travel-encounter combat escalation + field-guide unlock — 2026-09-30

## Outcome
- `ResolveTravelChoiceWithCombat` (zero callers) folded into the live path: `EncounterApplyChoice` now, after a
  committed resolve, escalates hostile travel/patrol choices via `TravelEncounterCombatBinder` →
  `OnTravelEncounterCombatTriggered` → existing Main combat handoff. Danger = surfacing sortie, else authored
  destination danger, else 1. At-most-once via the encounter-choice ledger.
- Travel `unlocks_field_guide_id` was dropped by the bridge (copied only morale/guilt). Additive Core field
  `NarrativeEncounterResolutionResult.FieldGuideUnlockId`; bridge copies it; session applies it through the new
  `FieldGuideUnlock` seam (Main binds `UnlockFieldGuideObservation`, which now uses `SetupFieldGuide()` so saved
  unlocks are restored instead of a bare catalog reload).
- `docs/combat/PATROL_RAID_BINDINGS.md` updated.

## Verification
Build 0 errors / 14 warnings. ExpeditionEncounterBridge 11/11, NarrativeEncounterSystem 13/13, Plan45Phase2Binding 13/13,
CompositionRootArchitectureGate 4/4, EncounterChoiceResolver 14/14. Headless --expedition-selftest PASS incl. new M10c
(rifle ambush raises combat once; flare does not; both unlock the wolf entry); journey + 7-day smoke PASS; arch map OK.
