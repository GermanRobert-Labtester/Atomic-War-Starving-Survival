# ASHFALL Implementation Gap Audit

**Status:** Pass 1 portfolio audit; incomplete, static and read-only  
**Audit date:** 2026-09-21  
**HEAD:** 5be1a30a63cd86cf23e4034473b739ac514f0f2a  
**Production files changed:** none

## 1. Scope

This pass surveyed the active Core, Godot host, authored JSON, live integration ledger, ownership ledger, known-debt register, and plan indexes. It used focused source searches and manually traced selected candidates. It did not read the bodies of plans above the user-specified 155,000-character cutoff. The inspected plan corpus contains 663 Markdown plans across C-integration-plans, Next-steps-plans, and piagentsplans; one known plan body exceeds the cutoff. Indexes and short titles were used for collision screening.

The working tree is already dirty across gameplay, tests, data, UI, governance, and generated documentation. Visible changes include a newer Wave 41 batch claim. Findings below describe the current shared working tree, not just HEAD.

This is not yet an exhaustive line-by-line audit of every Core and Godot file. Runtime execution, build, and tests were not run. No tests were requested, and this was not an implementation change.

## 2. Git SHA

HEAD at audit time: 5be1a30a63cd86cf23e4034473b739ac514f0f2a.

## 3. Executive Summary

ASHFALL already has extensive survival, medical, weather, ecology, economy, faction, expedition, narrative, shelter, and UI systems. The live integration ledger records many recent end-to-end seals. Several older “missing feature” descriptions are obsolete: cloud seeding and shelter decoration/trophies are live; genealogy and vehicle armor are recorded as integrated in recent Wave 36 work; trade routes and some survivor-aging proposals are explicitly held, superseded, or represented by a different current authority.

No new production gap is confirmed by this first pass. Four candidate expansion plans were drafted as premise-gated proposals: world-scale wildland fire, mobile medical outreach, community public-works projects, and a general scenario campaign library. Targeted searches found no corresponding full Core/host owner for the first three; the fourth has specialized scenario code, so its premise must be verified before adding a general scenario authority. These are design hypotheses, not findings that the full runtime lacks the features.

The strongest audit finding is governance drift: the supplied AGENTS.md queue lists some work as available while the current shared ledger and ownership file contain later completions and a newer active batch. Queue entries must be reconciled against those live files before integration is scheduled.

## 4. Completion Chain Model

Every feature claim should be traced through declared data or API → catalog validation → Core construction → active host registration → player command → authoritative state change → visible result → save capture and restore → focused verification. A class, JSON row, panel, test name, or old plan does not prove the whole chain.

## 5. Unimplemented Findings

No confirmed production-level unimplemented finding is claimed in this pass. Candidate expansion hypotheses are recorded in sections 21 and 24 and in the four Wave 1 plans.

## 6. Partially Implemented Findings

The plan corpus contains many historical partial descriptions; the current UNCLAIMED_CORPUS_CENSUS.md and live integration ledger show that several were later sealed. Historical partial status was not treated as current evidence. This pass did not audit every current system to identify all remaining partial chains.

## 7. Unwired Systems

No whole-system wiring defect is confirmed by the current sample. The live ledger itself records recent repairs to real wiring gaps, including a previously unconstructed ShelterOperationsAudioBridge and a missing wind input to ReconTelemetrySystem. These are closed items and are excluded from the new backlog.

## 8. Missing Registrations

Not exhaustively checked. The current ledger records repair of a fresh-game bootstrap path and lifecycle gate, but this audit did not trace every registration or scene route.

## 9. Dead Callbacks

Not exhaustively checked. The live ledger reports recent removal or repair of dead events in AnomalyHazardSystem, BlackMarketSystem, CohortSystem, and the chronicle UI path. These should not be rediscovered as open findings without a fresh source trace.

## 10. Missing Consumers

Not exhaustively checked. A source-name screen found authored references to wildfire and mobile medical outreach without matching full Core/host class names. That is only a search result; it does not establish an absent consumer or unreachable content.

## 11. Missing Producers

Not exhaustively checked. The integration ledger records earlier confirmed missing producers for excavation methane/flood events and filter breakthrough, now sealed. No new producer defect is confirmed here.

## 12. Silent Failures

A broad placeholder/error-pattern search found multiple Core loaders using catch (Exception) with empty bodies, including FoodTypeSystem, EmergencyAlertSystem, WaterSourceSystem, NpcMemorySystem, and others. These are audit leads, not confirmed player-facing failures: each needs a loader-to-validator-to-host trace and a check for fallback logging or pre-validation. The next audit pass should classify them individually, prioritizing catalogs loaded outside the strict integrity pipeline.

## 13. False Success Paths

Not exhaustively checked. Do not infer false success from a method returning a default value without tracing its callers and contract.

## 14. Data/Runtime Gaps

No authored row was proven unreachable in this pass. Current ledgers report prior content-utilization and data-integrity gates, but their reported historic results do not replace a fresh run. The candidate wildland-fire plan notes that fire is referenced in authored content and that a shelter fire owner exists; it proposes only a distinct outdoor firefront if the missing runtime chain is verified.

## 15. Core/Godot Wiring Gaps

Not exhaustively checked. Broad checks should compare Core owners, host sessions, route registries, panels, and scene bindings. A panel or host class alone is not sufficient evidence.

## 16. Save Gaps

Not exhaustively checked. The live ledger describes several recent save/restore seals; no new save defect is claimed. Any approved expansion must reuse the current save-section owner and prove both capture and restore before it is called persistent.

## 17. Syntax/API Mismanagement

No compiler or analyzer pass was run. Static syntax/API conclusions are therefore withheld.

## 18. Reachability Problems

Not exhaustively checked. The four candidate plans contain explicit reachability gates: each new catalog row must resolve from authored data to an active command or event and produce observable state changes.

## 19. Branch/State Machine Gaps

Not exhaustively checked. This pass did not inspect all enum cases, event transitions, command failure paths, or cancellation and restore branches.

## 20. Test Coverage Gaps

The current test tree and recent ledger entries were used as indexes only. No test was added or run. A future focused audit should map each confirmed chain to the smallest relevant integration and persistence test; do not add speculative tests before the premise is established.

## 21. Cross-System Broken Chains

No new broken chain is confirmed. Four cross-system opportunities were selected for premise-audited design:

1. Outdoor firefront → weather and wind → evacuation and existing shelter fire response → ecological and location aftermath.
2. Mobile medical outreach → existing expedition dispatch and travel → medical authority → outpost/community demand.
3. Public works → existing map locations, inventory, work eligibility, power/water, and faction state.
4. Scenario starts → existing campaign bootstrap, difficulty authority, objective/quest owners, and save metadata.

Each proposal must extend existing owners where a concern already has one and stop for a signed architecture decision if it would introduce a new mutable authority.

## 22. Legacy/Migration Gaps

The source authority states that Godot is current and Unity is retired. This pass did not audit legacy asset parity or migration completeness. Do not use Unity-only code as proof that a current gameplay feature exists.

## 23. Rejected False Positives

- Cloud seeding is present in Core and the weather panel.
- Shelter decoration and trophies have Core, host, save, and UI paths.
- The old Plan 217 genealogy proposal was integrated in Wave 36.
- Vehicle armor grades and the Plan 50 garage path are recorded as integrated in Wave 36.
- Plan 176 campaign tenure is deliberately day-based; the aging-system proposal is retired/held and must not be revived from old text.
- Plan 192 player trade routes appears in accepted historical debt with a hold condition; it is not a greenfield authorization.
- Wave 40 expanded verdict NPCs/radio, dose quests, journal voice, epilogue slides, personal narrative questlines, faction-war dialogue, and foundry treaty content. Broad additions to those same surfaces were not selected.

## 24. Ranked Gap-Sealing Backlog

All four entries below are DRAFT hypotheses, not approved implementation packages:

| Rank | Candidate | Player impact | Main existing authorities | Promotion gate |
|---|---|---|---|---|
| G2 | Wildland firefront and burn recovery | Adds a persistent outdoor hazard with prevention, response, and recovery decisions | Weather, shelter fire, water, wildlife, location | Prove no existing world-fire owner; sign hazard-state and exposure contracts |
| G2 | Mobile medical outreach | Turns medical readiness into field operations and remote-care choices | Expedition, medical, inventory, outposts | Prove no existing dispatch/clinic owner; preserve one patient and medicine authority |
| G2 | Community public-works projects | Gives the player multi-day, visible repair goals at existing sites | Inventory, work fitness, workshop, power/water, locations | Prove project state is not already owned; authorize save ownership |
| G2 | Scenario campaign library | Adds replayable starting situations and authored objective packages | Campaign bootstrap, difficulty, quest/runtime, save metadata | Reconcile specialized scenario code and active XP W1 before design approval |

## 25. Evidence Index

- INTEGRATION_PLANS.md: live integration ledger, including CF-P5 marked DONE and recent Wave 40 seals.
- WORKTREE_OWNERSHIP.md: current claims; includes Wave 36 Plan 50–53 and Plan 217 integration records and a newer Wave 41 claim.
- KNOWN_DEBT.md: accepted/retired/held work; Plan 174/175/192/199 holds and retired aging decision.
- docs/roadmap/PLAN_REGISTER.md: plan index; useful for locating proposals, not a status authority.
- docs/plans/UNCLAIMED_CORPUS_CENSUS.md: historical index and current row state, to be rechecked against the live ledger.
- Assets/Ashfall.Core/World/CloudSeedingSystem.cs; src/UI/WeatherForecastPanel.cs.
- Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs; Assets/Ashfall.Core/Shelter/TrophySystem.cs; src/UI/ShelterDecorPanel.cs.
- Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs; src/Host/ShelterFireHostSession.cs; src/UI/FireIncidentPanel.cs.
- Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs; Assets/Ashfall.Core/Campaign/SliceScenario.cs; Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs.

## 26. Audit Confidence

High confidence in the cited exclusions because they have current source and integration evidence. Medium confidence in the current ledger/queue drift because the shared working tree is changing. Low confidence that the four candidate systems are absent: searches were targeted and name-based, and a full consumer/registration/save trace remains.

## 27. Handoff

Continue with the ashfall-scan passes on Core and Godot in bounded domain groups. First classify each empty catch, then trace construction/registration and save ownership for the four candidates. Re-run the plan-index collision scan after excluding all over-cutoff and fresh plan bodies. Update this report only when evidence changes; do not implement production code under this audit.
