# PLAN-UI-SURFACE-15 — Appendix A: Route Inventory

**Generated:** 2026-09-21 from `src/Main.PlayerSurfaces.cs`
(192 distinct quoted ids; expanded surface list = 59 ids).
**Verdicts:** `EXPANDED` (in `expandedIds`) · `REFERENCED` (string used
elsewhere in host code — modal/stream/panel) · `DECLARED_ONLY` (no other
host reference — review for dead route or dynamic use).
**Use:** UP-15A/15B — the surface inventory input; every `DECLARED_ONLY`
row needs a classification or retirement.

## Inventory

| Route id | Expanded | Referenced in (≤3 files) | Verdict |
|---|---|---|---|
| `achievements` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `aeroponics` | no | `src/Main.CampaignOwners.cs`, `src/Main.Plans74_77.cs`, `src/Host/Plans74To77HostSessions.cs` | REFERENCED |
| `afflictions` | no | `src/Main.UiTests.PlayerPanels.cs`, `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `airlock_security` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterInfrastructure.cs`, `src/Main.CampaignOwners.cs` | EXPANDED |
| `amphibious_draisine` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/AmphibiousDraisineSaveStore.cs` | EXPANDED |
| `amputation_surgery` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `anomaly_watch` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `apprenticeship` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `archaeology_excavation` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `archive_desk` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `autopsy_report` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `aviation` | no | `src/Main.Plans182_185.cs`, `src/Host/AviationSaveStore.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `ballistics_workbench` | no | `src/Main.PlansB86_B89.cs`, `src/Main.Plans74_77.cs`, `src/Host/HostCli.PlansB86_B89.cs` | REFERENCED |
| `bandage` | no | `src/Main.GameFlow.cs`, `src/Host/CraftingHostSession.cs`, `src/Host/ExpeditionHostSession.cs` | REFERENCED |
| `beliefs_panel` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `bestiary` | yes | `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `bio_fermentation` | no | `src/Main.Plans126_129.cs`, `src/Host/BioFermentationSaveStore.cs` | REFERENCED |
| `black_market` | yes | `src/Main.BlackMarket.cs`, `src/Main.ExpandedShelterSystems.cs`, `src/Main.Lifecycle.cs` | EXPANDED |
| `black_projects_archive` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Plans152.cs`, `src/Host/BlackProjectsArchiveSaveStore.cs` | REFERENCED |
| `brine_extraction` | no | — | DECLARED_ONLY |
| `caravan_barter` | no | — | DECLARED_ONLY |
| `caregiving` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Host/CaregivingSaveStore.cs` | EXPANDED |
| `cargo_airdrop` | no | `src/Main.Plans202_205.cs`, `src/Host/CargoAirdropSaveStore.cs` | REFERENCED |
| `century_seed` | no | `src/Main.GameFlow.cs`, `src/UI/ExpansionsHubPanel.cs` | REFERENCED |
| `ceremony_ritual` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `chem_warfare_defense` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `chemical_dependency` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `chronicle` | no | `src/Main.GameFlow.cs`, `src/Main.Plans167_219.cs` | REFERENCED |
| `codex` | no | `src/Main.UiPanels.cs` | REFERENCED |
| `combat` | no | `src/Main.Expeditions.cs`, `src/Main.Lifecycle.cs`, `src/Host/CombatSaveStore.cs` | REFERENCED |
| `combat_detail` | no | — | DECLARED_ONLY |
| `combat_history` | no | — | DECLARED_ONLY |
| `combat_hud` | no | — | DECLARED_ONLY |
| `comms_array_transceiver` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `companion_kennel` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `contractor_roster` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `crafting` | no | `src/Main.Piezometer.cs`, `src/Main.Sanitation.cs`, `src/Main.Lifecycle.cs` | REFERENCED |
| `crossing_quests` | no | `src/Main.GameFlow.cs`, `src/UI/ExpansionsHubPanel.cs` | REFERENCED |
| `cvd_diamond` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/CvdDiamondSaveStore.cs` | EXPANDED |
| `cybernetics` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `death_legacy` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.SurvivorDeathLegacy.cs`, `src/Host/SurvivorDeathLegacySaveStore.cs` | EXPANDED |
| `decontamination` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.CampaignOwners.cs`, `src/Main.ShelterBatch3.cs` | EXPANDED |
| `deep_coast` | no | `src/Main.GameFlow.cs`, `src/UI/ExpansionsHubPanel.cs` | REFERENCED |
| `defense_grid` | yes | `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `desperation_crisis` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `dose_geography` | no | `src/Main.UiTests.Dose.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `dose_ledger` | no | `src/Main.Phase0.cs`, `src/Host/DoseLedgerSaveStore.cs` | REFERENCED |
| `duty_roster` | no | `src/Main.DutyRoster.cs`, `src/Main.UiTests.DutyRoster.cs`, `src/Main.UiHandlers.cs` | REFERENCED |
| `duty_roster_detail` | no | `src/Main.UiTests.DutyRoster.cs`, `src/Main.UiPanels.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `dynamic_quests` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Plans46_49.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `ebpvd_coating` | no | `src/Main.Plans146_149.cs`, `src/Host/EbPvdCoatingSaveStore.cs` | REFERENCED |
| `economy_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `electrostatic_scrubber` | no | — | DECLARED_ONLY |
| `emergency_response` | no | — | DECLARED_ONLY |
| `epilogue` | no | `src/Main.GameFlow.cs`, `src/UI/ExpansionsHubPanel.cs` | REFERENCED |
| `equipment_condition` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `event_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `events_log` | no | `src/Main.Application.cs`, `src/Host/AshfallInputActions.cs` | REFERENCED |
| `excavation` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `expansion_fallout_plume` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `expansions` | no | `src/Main.Lifecycle.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `expedition_camp` | no | — | DECLARED_ONLY |
| `expedition_radar` | no | — | DECLARED_ONLY |
| `expeditions` | no | `src/Main.Application.cs`, `src/Main.Lifecycle.cs`, `src/Main.UiPanels.cs` | REFERENCED |
| `faction_communique_board` | no | `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `faction_culture_codex` | no | `src/Main.UiPanels.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `faction_detail` | no | — | DECLARED_ONLY |
| `faction_matrix` | no | — | DECLARED_ONLY |
| `factions` | no | `src/Main.Codex.cs`, `src/Main.UiTests.SilentFoundry.cs`, `src/Main.Lifecycle.cs` | REFERENCED |
| `factions_narrative` | no | — | DECLARED_ONLY |
| `fallout_detail` | no | `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `farming` | yes | `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `fire_incident` | no | — | DECLARED_ONLY |
| `forced_labor` | no | `src/Main.Plans182_185.cs`, `src/Host/ForcedLaborSaveStore.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `fungi_cultivation` | no | `src/Main.Plans190_193.cs`, `src/Host/FungiSaveStore.cs` | REFERENCED |
| `geiger_calibration` | no | — | DECLARED_ONLY |
| `geothermal_orc` | no | `src/Main.CampaignOwners.cs`, `src/Main.Plans74_77.cs`, `src/Host/Plans74To77HostSessions.cs` | REFERENCED |
| `greenhouse` | no | `src/Main.Lifecycle.cs`, `src/Main.UiTests.CompositionRoot.cs`, `src/Main.World.cs` | REFERENCED |
| `guidance` | no | `src/Main.Application.cs`, `src/Main.GameFlow.cs`, `src/Host/AshfallInputActions.cs` | REFERENCED |
| `help` | no | `src/Main.Application.cs`, `src/Main.GameFlow.cs`, `src/Host/AshfallInputActions.cs` | REFERENCED |
| `hidden_agenda` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.HiddenAgenda.cs`, `src/Host/HiddenAgendaSaveStore.cs` | EXPANDED |
| `holdfast` | no | `src/Main.Holdfast.cs`, `src/Main.Application.cs`, `src/Main.Campaign.cs` | REFERENCED |
| `hydraulic_extrusion` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/HydraulicExtrusionSaveStore.cs` | EXPANDED |
| `insar_mapping` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `inventory` | no | `src/Main.FlagshipInstitutions.cs`, `src/Main.Inventory.cs`, `src/Main.Lifecycle.cs` | REFERENCED |
| `inventory_detail` | no | `src/Main.GameFlow.cs` | REFERENCED |
| `journal` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Application.cs`, `src/Main.Lifecycle.cs` | REFERENCED |
| `journal_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `justice_tribunal` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `kitchen_nutrition` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `library_study` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `low_background_metrology` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/LowBackgroundMetrologySaveStore.cs` | EXPANDED |
| `map` | no | `src/Main.GameFlow.cs`, `src/Host/HostCli.SelfTests.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `map_atlas` | no | — | DECLARED_ONLY |
| `map_detail` | no | — | DECLARED_ONLY |
| `maritime` | no | `src/Main.Lifecycle.cs`, `src/Main.Maritime.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `maritime_atlas` | no | — | DECLARED_ONLY |
| `medical` | no | `src/Main.Sanitation.cs`, `src/Main.UiTests.PlayerPanels.cs`, `src/Main.CampaignOwners.cs` | REFERENCED |
| `medical_office` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.UiHandlers.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `medical_ward` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Medical.cs`, `src/Main.UiTests.CompositionRoot.cs` | EXPANDED |
| `mental_health_crisis` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `mercenary_bounty_board` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `microfluidic_diagnostic` | no | `src/Main.Plans146_149.cs`, `src/Host/MicrofluidicDiagnosticSaveStore.cs` | REFERENCED |
| `mine_clearing_flail` | no | `src/Main.Plans146_149.cs`, `src/Host/MineClearingFlailSaveStore.cs` | REFERENCED |
| `moral_choice` | no | `src/Main.MoralChoice.cs`, `src/Main.GameFlow.cs`, `src/Host/MoralChoiceSaveStore.cs` | REFERENCED |
| `muster` | no | `src/Main.Muster.cs`, `src/Main.UiPanels.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `muster_atlas` | no | — | DECLARED_ONLY |
| `mutation_tree` | no | `src/Main.Plans178_181.cs`, `src/Host/MutationSaveStore.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `narcotics` | no | `src/Main.Plans182_185.cs`, `src/Host/NarcoticsSaveStore.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `narrative_arc` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `nursery` | no | `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `personal_quests` | yes | `src/Main.PersonalQuests.cs`, `src/Main.ExpandedShelterSystems.cs`, `src/Host/PersonalQuestSaveStore.cs` | EXPANDED |
| `phantom_memory` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Phase0.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `pharma` | no | — | DECLARED_ONLY |
| `pharma_lab` | no | `src/Main.UiPanels.cs` | REFERENCED |
| `phase0` | no | `src/Main.Lifecycle.cs`, `src/Main.Phase0.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `plans_130_133` | yes | `src/Main.ExpandedShelterSystems.cs` | EXPANDED |
| `plans_94_97` | yes | `src/Main.ExpandedShelterSystems.cs` | EXPANDED |
| `plastic_pyrolysis` | no | `src/Main.Plans202_205.cs`, `src/Host/PlasticPyrolysisSaveStore.cs` | REFERENCED |
| `pneumatic_dispatch` | no | `src/Main.CampaignOwners.cs`, `src/Main.Plans74_77.cs`, `src/Host/Plans74To77HostSessions.cs` | REFERENCED |
| `politics` | no | `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `power_grid` | no | `src/Main.Campaign.cs`, `src/Main.CampaignOwners.cs`, `src/Main.World.cs` | REFERENCED |
| `prisoners` | no | `src/Main.Plans178_181.cs`, `src/UI/GameDashboardPanel.cs`, `src/UI/JusticeTribunalPanel.cs` | REFERENCED |
| `propaganda` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `protocol` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `psychology_arcs` | yes | `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `quest_detail` | no | — | DECLARED_ONLY |
| `quests` | no | `src/Main.UiTests.DutyRoster.cs`, `src/Main.GameFlow.cs`, `src/Host/HostCli.SelfTests.cs` | REFERENCED |
| `quests_atlas` | no | — | DECLARED_ONLY |
| `radiation_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `radiation_history` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `radio` | no | `src/Main.UiTests.PlayerPanels.cs`, `src/Main.Lifecycle.cs`, `src/Main.Narrative.cs` | REFERENCED |
| `radio_intelligence` | no | — | DECLARED_ONLY |
| `rail_grinding` | no | `src/Main.Plans146_149.cs`, `src/Host/RailGrindingSaveStore.cs` | REFERENCED |
| `railway_logistics` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `regional_treaty` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `relationship_decay` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.RelationshipDecay.cs`, `src/Host/RelationshipDecaySaveStore.cs` | EXPANDED |
| `research` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Lifecycle.cs`, `src/Main.UiPanels.cs` | REFERENCED |
| `research_atlas` | no | — | DECLARED_ONLY |
| `robotics_assembly` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `room_laboratory_research` | no | `src/Main.ShelterBatch3.cs` | REFERENCED |
| `rumors` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `runflat_tire` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/RunFlatTireSaveStore.cs` | EXPANDED |
| `sanitation` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Sanitation.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `save` | no | `src/Main.GameFlow.cs` | REFERENCED |
| `settings` | no | — | DECLARED_ONLY |
| `shelter` | no | `src/Main.EcologicalInfestations.cs`, `src/Main.UiTests.PlayerPanels.cs`, `src/Main.Plans162_185.cs` | REFERENCED |
| `shelter_atmosphere` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterAtmosphere.cs`, `src/Host/ShelterAtmosphereSaveStore.cs` | EXPANDED |
| `shelter_barter` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Plans147.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `shelter_decor` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.CampaignOwners.cs`, `src/Main.ShelterBatch3.cs` | EXPANDED |
| `shelter_records` | no | `src/Main.UiHandlers.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `shelter_reputation` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterReputation.cs`, `src/Host/ShelterReputationSaveStore.cs` | EXPANDED |
| `shelter_schedule` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterInfrastructure.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `shelter_security` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSecurity.cs`, `src/Host/ShelterSecuritySaveStore.cs` | EXPANDED |
| `shelter_social` | no | `src/Main.Plans46_49.cs` | REFERENCED |
| `shelter_thermal` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterInfrastructure.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `silent_foundry` | no | `src/Main.UiTests.SilentFoundry.cs`, `src/Main.Economy.cs`, `src/Main.UiPanels.cs` | REFERENCED |
| `skill_matrix` | no | — | DECLARED_ONLY |
| `sky_defense_battery` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.FlagshipInstitutions.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `slurry_dewatering_sump` | no | — | DECLARED_ONLY |
| `sofc_power` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/SofcPowerSaveStore.cs` | EXPANDED |
| `sound_ranging` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs`, `src/Host/SoundRangingSaveStore.cs` | EXPANDED |
| `standing_record` | no | `src/Main.GameFlow.cs`, `src/UI/ExpansionsHubPanel.cs` | REFERENCED |
| `standing_record_atlas` | no | — | DECLARED_ONLY |
| `status` | no | `src/Main.UiTests.PlayerPanels.cs`, `src/Main.GameFlow.cs`, `src/Host/HostCli.Summary.cs` | REFERENCED |
| `stealth` | no | `src/Main.Plans178_181.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `subterranean_operations` | no | — | DECLARED_ONLY |
| `sump_flooding` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterBatch3.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `survival_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `survival_workstation` | no | — | DECLARED_ONLY |
| `survivor_detail` | no | `src/Main.GameFlow.cs` | REFERENCED |
| `survivor_downtime` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `survivor_relations` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `survivors` | no | `src/Main.Survivors.cs`, `src/Main.UiTests.PlayerPanels.cs`, `src/Main.UiTests.StartingCohortLifecycle.cs` | REFERENCED |
| `time_capsule` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/UI/GameDashboardPanel.cs` | EXPANDED |
| `trade` | no | `src/Main.UiTests.SilentFoundry.cs`, `src/Main.GameFlow.cs`, `src/Host/HostCli.SelfTests.cs` | REFERENCED |
| `traveling_caravan` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `triangulation` | no | — | DECLARED_ONLY |
| `vehicle_garage` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Lifecycle.cs`, `src/Main.Plans50_53.cs` | EXPANDED |
| `verdict` | no | `src/Main.Verdict.cs`, `src/Main.Lifecycle.cs`, `src/Main.GameFlow.cs` | REFERENCED |
| `verdict_dashboard` | no | — | DECLARED_ONLY |
| `vinyl_morale` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `water_treatment` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterInfrastructure.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `waystation_network` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `weather` | no | `src/Main.ExpandedShelterSystems.cs`, `src/Main.UiTests.PlayerPanels.cs`, `src/Main.Lifecycle.cs` | REFERENCED |
| `weather_detail` | no | `src/Main.GameFlow.cs`, `src/UI/GameDashboardPanel.cs` | REFERENCED |
| `weather_forecast` | no | `src/Main.Application.cs`, `src/Host/AshfallInputActions.cs` | REFERENCED |
| `weather_history` | no | `src/Main.Application.cs`, `src/Host/AshfallInputActions.cs` | REFERENCED |
| `weather_sonde` | no | — | DECLARED_ONLY |
| `wildlife_trapping` | yes | `src/Main.ExpandedShelterSystems.cs`, `src/Main.ShelterSocial.cs`, `src/Main.GameFlow.cs` | EXPANDED |
| `winter_freeze` | no | `src/Main.UiTests.Plans198_201.cs` | REFERENCED |
| `workshop` | no | `src/Main.UiTests.WorkshopRelic.cs`, `src/Main.UiPanels.cs`, `src/Main.ShelterAtmosphere.cs` | REFERENCED |

## Verdict counts

DECLARED_ONLY: 32, EXPANDED: 59, REFERENCED: 101