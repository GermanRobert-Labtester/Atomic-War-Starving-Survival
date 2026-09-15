// SPDX-License-Identifier: MIT
# EVENT SEMANTIC PARITY MATRIX

> C2 / Plan 17A-S §6.4 — generated producer/consumer parity matrix.
> Measured: 2026-09-15. Regenerate after vocabulary changes;
> `DayEventParitySourceGateTests` fails when this document is stale
> relative to emitted or handled kinds.

Authority: `DailyBriefingReportBuilder` (consumer) + `DayEventVocabulary`
(classification). This is the documented consumer contract C2 depends on;
Plan 31 may replace it with a typed semantic-kind vocabulary.

| Kind | Producer(s) | Briefing handler | Route | Status |
|---|---|---|---|---|
| `aeroponics_ticked` | Main.Plans74_77.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `aquaponics_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `consumed_rations` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `crafting_completed` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `crafting_production` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `cryo_vault_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `debt_ledger_ticked` | Main.DebtCredit.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `duty_roster_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `echo_consequence_due` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible — echo-consequence surface from in-flight worktree work) |
| `echo_surfaced` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible — echo-consequence surface from in-flight worktree work) |
| `espionage_ticked` | Main.Plans166_169.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `events_evaluated` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `expedition_milestone` | HostCli.WorldPlaytest.cs, Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `expedition_ticked` | HostCli.WorldPlaytest.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `expeditions_caravans_ticked` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `expeditions_ticked` | PerformanceCampaignHarness.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `flagship_institutions_ticked` | Main.FlagshipInstitutions.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `fluid_logistics_ticked` | Main.Plans166_169.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `geothermal_orc_ticked` | Main.Plans74_77.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `greenhouse_foundry_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `hazard_warning` | HostCli.WorldPlaytest.cs, Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `holdfast_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `journal_ticked` | PerformanceCampaignHarness.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `maritime_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `market_shocks_active` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `market_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `medical_disease_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `memorial_checked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `morale_contagion_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `narrative_arc_selected` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `narrative_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `needs_ticked` | PerformanceCampaignHarness.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `nuclear_generation_published` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `personal_quest_progressed` | PersonalQuestSystem.cs | generic default | via briefing panel | GENERIC (visible) |
| `pneumatic_dispatch_ticked` | Main.Plans74_77.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `power_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `precision_metrology_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `procedural_narrative_ticked` | Main.Plans166_169.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `psychology_arcs_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `psyops_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `radio_distress_active` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `radio_distress_expiring` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `radio_intercept` | HostCli.WorldPlaytest.cs, Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `radio_intercept_decrypted` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `radio_location_triangulated` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `radio_program_production_active_delta` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `radio_program_production_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `radio_transmission` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `research_ticked` | Main.Plans166_169.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `resource_delta` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `sanitation_disease_sweep` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `sanitation_spill` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `sanitation_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `seismic_geology_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `shelter_consequence` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `shelter_decon_completed` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `shelter_decon_started` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `shelter_decor_morale` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (visible) |
| `shelter_facilities_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `shelter_filter_degraded` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `shelter_fire_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `shelter_hatch_unsealed` | Main.CampaignOwners.cs | yes | via briefing panel | HANDLED |
| `social_dispute_mediated` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `social_dispute_unresolved` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `social_privacy_warning` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_cave_in` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `subterranean_flood_warning` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_methane_warning` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_rescue_active` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_rescue_completed` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `subterranean_rescue_failed` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_shoring_warning` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `subterranean_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `survivor_condition` | Main.MedicalTriage.cs | yes | via briefing panel | HANDLED |
| `survivor_perished` | SurvivorFateSystem.cs | yes | via briefing panel | HANDLED |
| `survivor_social_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `survivors_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `trapping_harvest` | HostCli.WorldPlaytest.cs | generic default | via briefing panel | GENERIC (visible) |
| `trapping_ticked` | HostCli.WorldPlaytest.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `underworld_ticked` | Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `weather_condition` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `weather_ticked` | HostCli.WorldPlaytest.cs, Main.CampaignOwners.cs, PerformanceCampaignHarness.cs | yes | via briefing panel | HANDLED |
| `workshop_job_completed` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `workshop_machine_degraded` | Main.Plans46_49.cs | yes | via briefing panel | HANDLED |
| `workshop_machine_overhauled` | — (no current emitter — Plan 31 scope) | yes | via briefing panel | HANDLED |
| `world_evolution_ticked` | HostCli.WorldPlaytest.cs, Main.CampaignOwners.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |
| `world_ticked` | PerformanceCampaignHarness.cs | generic default | via briefing panel | GENERIC (internal heartbeat — intentionally non-player-facing) |

## Heartbeat classification (intentionally retained, non-player-facing)

Suffix rule: `*_ticked` + curated internal aggregates (`events_evaluated`, `world_ticked`).
Tested by `DayEventVocabularyTests` / `DayEventParitySourceGateTests`.

