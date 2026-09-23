# PLAN-FAMILY-DYNASTY-43 — Appendix A: Orphan Dossiers (family & dynasty)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **20 host-unreachable authorities** that this plan's
packages wire (see the parent plan's seam map; the same systems appear in
`PLAN-ORPHAN-SEAL-01` Appendix A with their wave assignment).
**Use:** each dossier lists the authority, its file, its known tests, candidate
catalogs, and the parent-plan mechanic row that consumes it. A package claim
covers one or more of these systems end-to-end (host path, day owner if
stateful, save path, one player surface, focused tests).

## Dossiers

### 01. `SurvivorLetterDeliverySystem`
- **File:** `Narrative/SurvivorLetterDeliverySystem.cs` · **Types:** `SurvivorLetterDeliverySystem`
- **Known tests (1):** `NarrativeAndFactionWarIntegrationTests.cs`
- **Candidate catalogs:** `survivor_letters_lost_kin.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `LetterDeliverySystem`
- **File:** `Narrative/LetterDeliverySystem.cs` · **Types:** `LetterDeliverySystem`
- **Known tests (1):** `LetterDeliverySystemTests.cs`
- **Candidate catalogs:** `letters_expansion.json`, `survivor_letters_lost_kin.json`, `unsent_letters_batch_2.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `NpcMemorySystem`
- **File:** `Narrative/NpcMemorySystem.cs` · **Types:** `NpcMemorySystem`
- **Known tests (2):** `Narrative/NpcMemorySystemTests.cs`, `Narrative/Plan147_151NpcAnimalIntegrationTests.cs`
- **Candidate catalogs:** `standing_record_memory.json`, `wasteland_settlement_npcs.json`, `narrative_encounters_npc_arcs.json`, `npc_arcs.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 04. `HobbySystem`
- **File:** `Survivors/HobbySystem.cs` · **Types:** `HobbySystem`
- **Known tests (2):** `Survivors/HobbySystemTests.cs`, `Survivors/Plan161HobbyIntegrationTests.cs`
- **Candidate catalogs:** `hobby_definitions.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 05. `AntenatalMaternalHealthEngine`
- **File:** `Survivors/AntenatalMaternalHealthEngine.cs` · **Types:** `AntenatalMaternalHealthEngine`
- **Known tests (1):** `Survivors/AntenatalMaternalHealthEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 06. `SurvivorAgingProgressionEngine`
- **File:** `Survivors/SurvivorAgingProgressionEngine.cs` · **Types:** `SurvivorAgingProgressionEngine`
- **Known tests (2):** `Survivors/SurvivorAgingProgressionEngineTests.cs`, `Survivors/Plan176AgingSystemIntegrationTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Aging
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 07. `SurvivorAutonomySystem`
- **File:** `Survivors/SurvivorAutonomySystem.cs` · **Types:** `SurvivorAutonomySystem`
- **Known tests (1):** `Survivors/Plan144SurvivorAutonomyIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 08. `BackstorySystem`
- **File:** `Survivors/BackstorySystem.cs` · **Types:** `BackstorySystem`
- **Known tests (1):** `Survivors/Plan174SurvivorBackstoriesIntegrationTests.cs`
- **Candidate catalogs:** `backstory_templates.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 09. `AgingSystem`
- **File:** `Survivors/AgingSystem.cs` · **Types:** `AgingSystem`
- **Known tests (1):** `Survivors/Plan176AgingSystemIntegrationTests.cs`
- **Candidate catalogs:** `lead_crystal_scintillator_aging_logs.json`
- **Parent-plan mechanic:** Aging
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 10. `SurvivorRoutineSystem`
- **File:** `Survivors/SurvivorRoutineSystem.cs` · **Types:** `SurvivorRoutineSystem`
- **Known tests (1):** `Survivors/Plan188SurvivorRoutineIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 11. `SurvivorRoleSystem`
- **File:** `Survivors/SurvivorRoleSystem.cs` · **Types:** `SurvivorRoleSystem`
- **Known tests (1):** `Survivors/Plan195SurvivorRoleIntegrationTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 12. `RecruitmentSystem`
- **File:** `Survivors/RecruitmentSystem.cs` · **Types:** `RecruitmentSystem`
- **Known tests (1):** `Survivors/Plan204RecruitmentIntegrationTests.cs`
- **Candidate catalogs:** `recruitment_templates.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 13. `ConfessionSecretSystem`
- **File:** `Phantoms/ConfessionSecretSystem.cs` · **Types:** `ConfessionSecretSystem`
- **Known tests (2):** `ConfessionSecretSystemTests.cs`, `Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs`
- **Candidate catalogs:** `confession_secrets.json`, `wire_confessions.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 14. `CommitmentSystem`
- **File:** `Commitments/CommitmentSystem.cs` · **Types:** `CommitmentSystem`
- **Known tests (2):** `Campaign/CommitmentSystemTests.cs`, `Campaign/Plan33_38IntelCalendarIntegrationTests.cs`
- **Candidate catalogs:** `commitments.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 15. `InternalCommunicationSystem`
- **File:** `Communication/InternalCommunicationSystem.cs` · **Types:** `InternalCommunicationSystem`
- **Known tests (1):** `Communication/Plan211InternalCommunicationIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`, `communication_templates.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 16. `SecondGenerationMilestoneEngine`
- **File:** `Generations/SecondGenerationMilestoneEngine.cs` · **Types:** `SecondGenerationMilestoneEngine`
- **Known tests (1):** `Generations/SecondGenerationMilestoneEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Coming of age
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 17. `VoiceLineSelectionEngine`
- **File:** `Voice/VoiceLineSelectionEngine.cs` · **Types:** `VoiceLineSelectionEngine`
- **Known tests (1):** `Voice/VoiceLineSelectionEngineTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 18. `VoiceLineDispatchCoordinator`
- **File:** `Voice/VoiceLineDispatchCoordinator.cs` · **Types:** `VoiceLineDispatchCoordinator`
- **Known tests (1):** `Voice/VoiceLineDispatchCoordinatorTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 19. `SurvivorVoiceSystem`
- **File:** `Voice/SurvivorVoiceSystem.cs` · **Types:** `SurvivorVoiceSystem`
- **Known tests (2):** `Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Voice/SurvivorVoiceSystemTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 20. `PsychologicalProfileSystem`
- **File:** `Psychology/PsychologicalProfileSystem.cs` · **Types:** `PsychologicalProfileSystem`
- **Known tests (1):** `Survivors/Plan179UnifiedPsychologyIntegrationTests.cs`
- **Candidate catalogs:** `infiltrator_profiles.json`, `psychological_therapies.json`, `psychological_trauma.json`, `nuclear_core_profiles.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
