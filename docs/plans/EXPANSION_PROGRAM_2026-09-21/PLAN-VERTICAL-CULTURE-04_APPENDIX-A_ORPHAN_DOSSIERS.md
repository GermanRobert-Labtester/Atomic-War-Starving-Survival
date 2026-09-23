# PLAN-VERTICAL-CULTURE-04 — Appendix A: Orphan Dossiers (culture & memory)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **16 host-unreachable authorities** that this plan's
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
- **Parent-plan mechanic:** 6
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `LetterDeliverySystem`
- **File:** `Narrative/LetterDeliverySystem.cs` · **Types:** `LetterDeliverySystem`
- **Known tests (1):** `LetterDeliverySystemTests.cs`
- **Candidate catalogs:** `letters_expansion.json`, `survivor_letters_lost_kin.json`, `unsent_letters_batch_2.json`
- **Parent-plan mechanic:** 6
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `NpcMemorySystem`
- **File:** `Narrative/NpcMemorySystem.cs` · **Types:** `NpcMemorySystem`
- **Known tests (2):** `Narrative/NpcMemorySystemTests.cs`, `Narrative/Plan147_151NpcAnimalIntegrationTests.cs`
- **Candidate catalogs:** `standing_record_memory.json`, `wasteland_settlement_npcs.json`, `narrative_encounters_npc_arcs.json`, `npc_arcs.json`
- **Parent-plan mechanic:** 7
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 04. `RadioPropagationEngine`
- **File:** `Radio/RadioPropagation.cs` · **Types:** `RadioPropagationEngine`
- **Known tests (1):** `Radio/RadioPropagationTests.cs`
- **Candidate catalogs:** `faction_war_radio.json`, `radio_intercepts.json`, `year_of_ash_radio.json`, `faction_radio_corpus.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 05. `AudioAccessibilityCoordinator`
- **File:** `Audio/AudioAccessibilityCoordinator.cs` · **Types:** `AudioAccessibilityCoordinator`
- **Known tests (1):** `Audio/Plan169AudioAccessibilityIntegrationTests.cs`
- **Candidate catalogs:** `audio_logs_expansion_05.json`, `audio_cues.json`, `shelter_audio_cues.json`, `audio_accessibility_cues.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 06. `CassettePlaybackSystem`
- **File:** `Audio/CassettePlaybackSystem.cs` · **Types:** `CassettePlaybackSystem`
- **Known tests (1):** `Verdict/Plan82_67VerdictCassetteIntegrationTests.cs`
- **Candidate catalogs:** `cassette_sets.json`
- **Parent-plan mechanic:** 5
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 07. `CampaignLegacySystem`
- **File:** `Legacy/CampaignLegacySystem.cs` · **Types:** `CampaignLegacySystem`
- **Known tests (1):** `Legacy/Plan140GenerationalLegacyIntegrationTests.cs`
- **Candidate catalogs:** `campaign_epilogues.json`, `propaganda_campaigns.json`, `death_legacy_templates.json`, `legacy_traits.json`
- **Parent-plan mechanic:** 9
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 08. `CultureCreationSystem`
- **File:** `Culture/CultureCreationSystem.cs` · **Types:** `CultureCreationSystem`
- **Known tests (2):** `Survivors/Plan178ArtCultureIntegrationTests.cs`, `Culture/CultureCreationSystemTests.cs`
- **Candidate catalogs:** `recreation.json`, `agriculture_items.json`, `muster_faction_culture.json`, `apiculture_red_light_audits.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 09. `ShelterFestivalEngine`
- **File:** `Culture/ShelterFestivalEngine.cs` · **Types:** `ShelterFestivalEngine`
- **Known tests (1):** `Culture/ShelterFestivalEngineTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Parent-plan mechanic:** 1
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 10. `ShelterMuseumSystem`
- **File:** `Culture/ShelterMuseumSystem.cs` · **Types:** `ShelterMuseumSystem`
- **Known tests (1):** `Culture/Plan218MuseumIntegrationTests.cs`
- **Candidate catalogs:** `shelter_machine_identities.json`, `shelter_room_identities.json`, `shelter_social_events.json`, `shelter_audio_cues.json`
- **Parent-plan mechanic:** 2
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 11. `InternalCommunicationSystem`
- **File:** `Communication/InternalCommunicationSystem.cs` · **Types:** `InternalCommunicationSystem`
- **Known tests (1):** `Communication/Plan211InternalCommunicationIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`, `communication_templates.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 12. `VoiceLineSelectionEngine`
- **File:** `Voice/VoiceLineSelectionEngine.cs` · **Types:** `VoiceLineSelectionEngine`
- **Known tests (1):** `Voice/VoiceLineSelectionEngineTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Parent-plan mechanic:** 5
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 13. `VoiceLineDispatchCoordinator`
- **File:** `Voice/VoiceLineDispatchCoordinator.cs` · **Types:** `VoiceLineDispatchCoordinator`
- **Known tests (1):** `Voice/VoiceLineDispatchCoordinatorTests.cs`
- **Candidate catalogs:** `survivor_voice_lines.json`
- **Parent-plan mechanic:** 5
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 14. `SurvivorVoiceSystem`
- **File:** `Voice/SurvivorVoiceSystem.cs` · **Types:** `SurvivorVoiceSystem`
- **Known tests (2):** `Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Voice/SurvivorVoiceSystemTests.cs`
- **Candidate catalogs:** `antigravity_survivor_fields.json`, `deep_lore_survivor_fields.json`, `starting_survivors.json`, `year_of_ash_survivors.json`
- **Parent-plan mechanic:** 5
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 15. `PublicBroadsheetPressEngine`
- **File:** `Print/PublicBroadsheetPressEngine.cs` · **Types:** `PublicBroadsheetPressEngine`
- **Known tests (1):** `Print/PublicBroadsheetPressEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** 4
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 16. `CommunicationsSystem`
- **File:** `Communications/CommunicationsSystem.cs` · **Types:** `CommunicationsSystem`
- **Known tests (1):** `Communications/Plan157CommunicationsIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
