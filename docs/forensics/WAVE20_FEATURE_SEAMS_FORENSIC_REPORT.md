# Wave 20 Feature-Seam Forensic Report

**Date:** 2026-09-21  
**Scope:** candidate systems supporting Expansions 98–101; Expansion 97 is pre-existing and left intact.  
**Method:** static source/data inspection only; no tests or builds run.

## 1. Target

Current runtime reachability, ownership, data, state, save/load, determinism, feedback, tests and equivalents for four new Wave 20 candidates plus the pre-existing Expansion 97.

## 2. Executive finding

Expansions 98–101 target four Core systems whose direct Godot type references were not found: education, autonomy, internal communication and peer barter. Expansion 97 already exists as an in-progress faction/work-history proposal and is left untouched. Core/data delivery is not proof of player reachability.

## 3. Evidence summary

Education curriculum: {'stages': 3, 'subjects': 12}; autonomy actions: {'actions': 20}; communication templates: {'templates': 7}; barter rules: {'rules': 3}; visitor templates (additional future candidate): {'templates': 5}. Focused Core integration test files exist for education/autonomy/comms/barter; none were run.

## 4. Architecture placement

Core is engine-free, JSON under Assets/StreamingAssets/Data is authoritative, Godot UI/host/save adapters live in src. Exact-name searches found no direct src references for the four planned Core types.

## 5. Current implementation

Education supports curriculum, learner records, sessions, skill unlocks, graduation and capture/restore. Autonomy supports evaluation, override, goals and capture/restore. Communication supports boards/messages/intercom/read/ack/expiry and capture/restore. Barter supports offers/favors/trust/disputes and capture/restore.

## 6. Runtime wiring

Adjacent live routes have separate owners: NurseryPanel/Main.Plans178_181 for GenerationalSystem; UtilityAiHostSession for UtilityAI; radio/briefing/journal; ShelterBarterPanel/CaravanBarterLedger. No direct planned-type construction/use was located. Indirect/generated composition remains unknown.

## 7. Data flow

Catalogs are education {'stages': 3, 'subjects': 12}, autonomy {'actions': 20}, communications {'templates': 7}, barter {'rules': 3}. Core loaders exist; direct Godot consumer was not found. The visitor template count {'templates': 5} is included as a later candidate, not one of Plans 98–101.

## 8. State ownership

DTOs exist for each planned Core system. Neighbors: GenerationalSystem, SurvivorSocialCoordinator, PersonalBelongingsSystem, shared Inventory/ShelterBarter owners and skill/apprenticeship consumers. Keep each fact in its current owner.

## 9. Save/load

Core capture/restore methods exist. Direct host references or obvious matching save registrations were not found. Child learning already appears in child_development via GenerationalSaveStore. Trace registry, orchestrator and Main setup/save/flush before implementation.

## 10. Determinism

Autonomy accepts ISeededRng but has SeededRng(144) fallback. Education takes RNG per session but no session/day identity. Messages and barter use incremental IDs; host retry semantics require review. Barter transfer callbacks are optional and sequential.

## 11. UI/player feedback

No direct route for the four planned Core types was found. NurseryPanel shows GenerationalSystem state; UtilityAI scores actions; radio/journal cover separate channels; ShelterBarterPanel covers shared trade.

## 12. Tests and verification

Focused Core test files exist for Plans 154, 144, 211 and 213. Prior ledger results are not re-certified; no tests or builds were run for this documentation task.

## 13. Duplicates, legacy and forks

Education overlaps child learning and apprenticeship; autonomy is adjacent to UtilityAI and roster; internal messages differ from radio/journal/briefing/letters; peer barter differs from shared market/caravan stock. Visitor integration is another distinct gap. No matching _quarantine_legacy implementation was found; retired behavior is not authoritative.

## 14. Extension seams

Education assignment/session/graduation/knowledge hooks; autonomy action events/sink/goals; communication post/intercom/read/ack APIs; barter offer/trade/favor/dispute events and optional item delegates. None proves host composition.

## 15. Functional equivalents

Education PARTIAL: child education and vocational training. Autonomy PARTIAL: UtilityAI and duty commands. Communication PARTIAL: radio, journal, briefing and letters. Barter PARTIAL: shared trade. Visitor integration PARTIAL across airlock, housing and recruitment, held for a later plan.

## 16. Confirmed gaps

No direct src type references found. Education has overlapping child state and repeatable sessions. Autonomy has unproven consent/save path and default RNG fallback. Intercom has split IDs/acks. Barter callbacks remain optional/sequential despite a recent atomicity ledger claim.

## 17. Risks

HIGH: duplicated education authority, phantom/one-sided trades, coercive work override without proven consent. MEDIUM: split intercom acknowledgement; repeat daily side effects. Risks are based on source signatures/call order, not runtime verification.

## 18. Constraints for planning

Recheck source and exact save/host seams, reread live governance docs, claim exact paths. Stop for education owner, coercive consent or atomic transfer decisions. No parallel ledger, inventory, roster, message or save authority.

## 19. Evidence index

Core source: Education/SurvivorEducationSystem.cs; Survivors/SurvivorAutonomySystem.cs; Communication/InternalCommunicationSystem.cs; Economy/SurvivorBarterSystem.cs. Adjacent source includes Main.Plans178_181.cs, NurseryPanel, Main.SurvivorSocial.cs, ShelterBarterPanel, PersonalBelongingsSystem. Tests/catalogs are linked from each plan.

## 20. Confidence and unknowns

High confidence in Core API and catalog presence; moderate-high confidence exact static src references are absent. Indirect construction, host save registration, runtime reachability and current test status remain unknown. Existing Expansion 97 remains in-progress and is not audited here beyond its stated scope/status.

## Evidence paths

- Education: Assets/Ashfall.Core/Education/SurvivorEducationSystem.cs; src/Main.Plans178_181.cs; src/UI/NurseryPanel.cs; Plan154EducationIntegrationTests.
- Autonomy: Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs; src/Main.SurvivorSocial.cs; Plan144SurvivorAutonomyIntegrationTests.
- Communication: Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs; Plan211InternalCommunicationIntegrationTests.
- Barter: Assets/Ashfall.Core/Economy/SurvivorBarterSystem.cs; Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs; Plan213SurvivorBarterIntegrationTests.
- Governance: INTEGRATION_PLANS.md; WORKTREE_OWNERSHIP.md; TEST_POLICY.md; KNOWN_DEBT.md; AI_AGENT_WORKFLOW.md.
