# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plans 62/64/65 runtime wiring — 2026-09-29

> **STATUS: APPROVED BY USER** (chose "Wire Plans 62/64/65" in session, 2026-09-29)

## 0. Framing — The Uninvoked (editorial polish pass — commentary only)

*(Post-hoc, non-contractual editorial block; no scope, claim, decision, acceptance criterion or
recorded status changes.)*

> "Declared but never called is the saddest state a system can be in."

Three systems existed only as declarations: the archive's decryption never decrypted, food
preservation never preserved, and the epilogue engine never ended anything. Wiring them through
the canonical coordinator is small work with a large meaning — it is the difference between a
corpus that *contains* a game and a runtime that *is* one.

- **A method with no caller is a promise with no one to keep it.** This plan is the meeting of
  the two.

---

## Outcome
`SetupPlans62To65` / `TickPlans62To65` were declared but never invoked (no git history of a caller), so
FoodPreservationSystem (Plan 64), PrewarArchiveDecryptionSystem (Plan 62) and CampaignEpilogueEngine (Plan 65)
never existed at runtime. Wire them through the canonical campaign-day coordinator.

## Changes
1. `src/Main.CampaignOwners.cs` — register `plans_62_65` day owner (phase 2, production/storage, before phase-3 needs)
   implementing `IPreDaySnapshotRestore` over both stateful systems; emits `food_preservation_ticked`.
2. `src/Main.SubsystemComposition.cs` — make `SetupPlans62To65` idempotent for the `FoodConsumed` bridge
   (was subscribed outside the null guard → duplicate disease exposure per repeat call); add `ResetPlans62To65`.
3. `src/Main.Lifecycle.cs` — call `ResetPlans62To65` in the reset list.
4. `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` + `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` — heartbeat row.

## Non-goals
No new save section (food_preservation / prewar_archives already registered and captured in SaveOrchestrator).
No UI. No change to Core system behavior. `TickPlans50To53` (also uncalled) left for a separate package.

## Verification
Host build; DayEventVocabularyTests, DayEventParitySourceGateTests, CompositionRootArchitectureGateTests,
SaveSectionRegistryTests, existing FoodPreservation/PrewarArchive Core tests; headless boot.

## Result (2026-09-29)
Host build 0 errors / 14 warnings (unchanged). Scoped: CampaignDayCoordinatorSourceGate 4/4, DayEventParitySourceGate 2/2,
DayEventVocabulary 8/8, DayEventSemanticKind 39/39, FoodPreservationSystem 8/8, PrewarArchiveDecryption 6/6,
CompositionRootArchitectureGate 3/3, ArchitectureTestMapGate 6/6. Headless: --7-day-smoke-selftest 10/10,
--real-campaign-journey-selftest PASS (real TickSimDay through coordinator). Architecture map regenerated
(also fixed stale encounter_choice setup name in scripts/ci/generate-architecture-map.py); Constructed 308→311.
Known limit: coordinator retry does not roll back inventory (pre-existing, all inventory producers), so a
curing job completing on a failed-then-retried day can add its output twice.
