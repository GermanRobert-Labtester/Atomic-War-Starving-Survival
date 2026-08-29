# ASHFALL Player-Surface Contract & Coverage Manifest

> **Total Surfaces**: 78 | **Routed**: 78/78 (100%) | **Bound**: 78/78 (100%) | **Closeable**: 78/78 (100%)
> **Interactive Command Surfaces**: 41 | **ReadOnly/Observational**: 37 | **Visual Snapshot Covered**: 21

---

## 1. Overview and Invariants

This document publishes the formal player-surface contract and coverage manifest for **ASHFALL**.
Every player-navigable screen, sub-system dashboard, and modal overlay is registered in [`PanelRegistry`](../Assets/Ashfall.Core/UI/PanelRegistry.cs) and backed by the typed manifest in [`PlayerSurfaceManifest`](../Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs).

### Mechanical CI Gates Enforced
1. **Zero Dead Routes**: Every panel id emitted in UI or HUD triggers must resolve to a valid registered panel in `PanelRegistry`.
2. **Zero Orphan Panels**: Every registered surface must be reachable via at least one valid player route (`OpenPlayerPanel` or `OpenExpandedPanel`).
3. **Full Binding & Lifecycle**: Every production panel must declare setup dependencies or a host session binding target, and provide a defined close behavior (`DashboardShellCloseButton`, `OverlayCloseButton`, or `ModalDismiss`).
4. **Honest Action Tracking**: Interactive command surfaces are tracked separately from read-only observational surfaces.
5. **No Fabricated Fallbacks**: Production panels mount clean domain state or render honest empty states rather than generating synthetic demo entities.

---

## 2. Player Surfaces Matrix

| Panel ID | Display Name | Group | Route Method | Binding Target | Close Behavior | Rail | Shell | Actions | Snapshots |
|---|---|---|---|---|---|---|---|---|---|
| `achievements` | Achievements | Dashboard | `OpenPlayerPanel("achievements")` | `survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `afflictions` | Afflictions | Dashboard | `OpenPlayerPanel("afflictions")` | `survivors, inventory, medical, phase0` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `airlock_security` | Airlock Security | Expanded | `OpenExpandedPanel("airlock_security")` | `airlock_securityHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `apprenticeship` | Apprenticeship | Expanded | `OpenExpandedPanel("apprenticeship")` | `apprenticeshipHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `archive_desk` | Archive Desk | Expanded | `OpenExpandedPanel("archive_desk")` | `archive_deskHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `autopsy_report` | Autopsy Report | Expanded | `OpenExpandedPanel("autopsy_report")` | `autopsy_reportHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `caregiving` | Caregiving | Expanded | `OpenExpandedPanel("caregiving")` | `caregivingHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `century_seed` | Century Seed Panel | Dashboard | `OpenPlayerPanel("century_seed")` | `expansions, survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `chemical_dependency` | Chemical Dependency | Expanded | `OpenExpandedPanel("chemical_dependency")` | `chemical_dependencyHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `combat` | Combat Panel | Dashboard | `OpenPlayerPanel("combat")` | `combatHost` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `combat_detail` | Combat Detail | Secondary | `OpenPlayerPanel("combat_detail")` | `combat_detailHost` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `combat_history` | Combat History | Secondary | `OpenPlayerPanel("combat_history")` | `combat_historyHost` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `contractor_roster` | Contractor Roster | Expanded | `OpenExpandedPanel("contractor_roster")` | `contractor_rosterHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `crafting` | Crafting Panel | Dashboard | `OpenPlayerPanel("crafting")` | `crafting, inventory` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `crossing_quests` | Crossing Quest Panel | Dashboard | `OpenPlayerPanel("crossing_quests")` | `expansions` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `decontamination` | Decontamination | Expanded | `OpenExpandedPanel("decontamination")` | `decontaminationHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `deep_coast` | Deep Coast Panel | Dashboard | `OpenPlayerPanel("deep_coast")` | `deep_coast, core` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `duty_roster` | Duty Roster Panel | Dashboard | `OpenPlayerPanel("duty_roster")` | `duty_roster, survivors` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `duty_roster_detail` | Duty Roster Detail | Secondary | `OpenPlayerPanel("duty_roster_detail")` | `duty_roster` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `economy_detail` | Economy Detail | Dashboard | `OpenPlayerPanel("economy_detail")` | `economy` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `epilogue` | Epilogue Panel | Dashboard | `OpenPlayerPanel("epilogue")` | `expansions, survivors` | DashboardShellCloseButton | — | Yes | ReadOnly | — |
| `equipment_condition` | Equipment Condition | Expanded | `OpenExpandedPanel("equipment_condition")` | `equipment_conditionHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `event_detail` | Event Detail | Dashboard | `OpenPlayerPanel("event_detail")` | `events` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `events_log` | Events Log | Dashboard | `OpenPlayerPanel("events_log")` | `events` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `excavation` | Excavation | Expanded | `OpenExpandedPanel("excavation")` | `excavationHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `expansions` | Expansions Hub | Dashboard | `OpenPlayerPanel("expansions")` | `expansions, greenhouse, duty_roster, muster, maritime, deep_coast, world, medical, verdict` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `expeditions` | Expeditions Panel | Dashboard | `OpenPlayerPanel("expeditions")` | `expeditions, expansions, survivors, inventory` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `faction_detail` | Faction Detail | Secondary | `OpenPlayerPanel("faction_detail")` | `factions` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `factions` | Factions Panel | Dashboard | `OpenPlayerPanel("factions")` | `core, muster, expansions` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `greenhouse` | Greenhouse Panel | Dashboard | `OpenPlayerPanel("greenhouse")` | `greenhouse` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `help` | Tutorial / Help | Dashboard | `OpenPlayerPanel("help")` | `helpHost` | DashboardShellCloseButton | — | Yes | ReadOnly | — |
| `holdfast` | Holdfast Terminal | Dashboard | `OpenPlayerPanel("holdfast")` | `core` | OverlayCloseButton | — | — | ReadOnly | — |
| `inventory` | Inventory Panel | Dashboard | `OpenPlayerPanel("inventory")` | `inventory` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `inventory_detail` | Inventory Detail | Dashboard | `OpenPlayerPanel("inventory_detail")` | `inventory` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `journal` | Journal Panel | Dashboard | `OpenPlayerPanel("journal")` | `journal` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `journal_detail` | Journal Detail | Dashboard | `OpenPlayerPanel("journal_detail")` | `journal` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `kitchen_nutrition` | Kitchen Nutrition | Expanded | `OpenExpandedPanel("kitchen_nutrition")` | `kitchen_nutritionHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `library_study` | Library Study | Expanded | `OpenExpandedPanel("library_study")` | `library_studyHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `map` | Map Panel | Dashboard | `OpenPlayerPanel("map")` | `core, expeditions, expansions, world, journal, deep_coast, year_of_ash` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `map_detail` | Map Location Detail | Secondary | `OpenPlayerPanel("map_detail")` | `world` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `maritime` | Maritime / Black Flotilla | Dashboard | `OpenPlayerPanel("maritime")` | `maritime, survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `medical` | Medical Panel | Dashboard | `OpenPlayerPanel("medical")` | `survivors, inventory, medical, phase0` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `medical_ward` | Medical Ward | Expanded | `OpenExpandedPanel("medical_ward")` | `medical_wardHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `mental_health_crisis` | Mental Health Crisis | Expanded | `OpenExpandedPanel("mental_health_crisis")` | `mental_health_crisisHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `muster` | The Muster Panel | Dashboard | `OpenPlayerPanel("muster")` | `muster` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `phantom_memory` | Phantom Memory | Expanded | `OpenExpandedPanel("phantom_memory")` | `phantom_memoryHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `pharma` | Pharma Lab (alias) | Dashboard | `OpenPlayerPanel("pharma")` | `crafting, inventory, survivors` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `pharma_lab` | Pharma Lab | Dashboard | `OpenPlayerPanel("pharma_lab")` | `crafting, inventory, survivors` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `phase0` | Phase 0 Panel | Dashboard | `OpenPlayerPanel("phase0")` | `phase0` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `protocol` | Opening Protocol | Dashboard | `OpenPlayerPanel("protocol")` | `starting_level` | ModalDismiss | — | — | ReadOnly | — |
| `quest_detail` | Quest Detail | Secondary | `OpenPlayerPanel("quest_detail")` | `quests` | OverlayCloseButton | — | Yes | ReadOnly | — |
| `quests` | Quests Panel | Dashboard | `OpenPlayerPanel("quests")` | `core, expansions, duty_roster` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `radiation_detail` | Radiation Detail | Dashboard | `OpenPlayerPanel("radiation_detail")` | `survivors, phase0` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `radiation_history` | Radiation History | Dashboard | `OpenPlayerPanel("radiation_history")` | `phase0` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `radio` | Radio Panel | Dashboard | `OpenPlayerPanel("radio")` | `radio` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `regional_treaty` | Regional Treaty | Expanded | `OpenExpandedPanel("regional_treaty")` | `regional_treatyHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `research` | Research | Dashboard | `OpenPlayerPanel("research")` | `research` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `save` | Save / Load Panel | Dashboard | `OpenPlayerPanel("save")` | `saveHost` | DashboardShellCloseButton | — | Yes | Interactive | — |
| `shelter` | Shelter Panel | Dashboard | `OpenPlayerPanel("shelter")` | `survivors, world, inventory` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `shelter_schedule` | Shelter Schedule | Expanded | `OpenExpandedPanel("shelter_schedule")` | `shelter_scheduleHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `shelter_thermal` | Shelter Thermal | Expanded | `OpenExpandedPanel("shelter_thermal")` | `shelter_thermalHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `silent_foundry` | Silent Foundry Panel | Dashboard | `OpenPlayerPanel("silent_foundry")` | `expansions, silent_foundry` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `standing_record` | Standing Record Panel | Dashboard | `OpenPlayerPanel("standing_record")` | `expansions` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `status` | Survival Status | Dashboard | `OpenPlayerPanel("status")` | `survivors, world, inventory` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `sump_flooding` | Sump Flooding | Expanded | `OpenExpandedPanel("sump_flooding")` | `sump_floodingHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `survival_detail` | Survival Detail | Dashboard | `OpenPlayerPanel("survival_detail")` | `survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `survivor_detail` | Survivor Detail | Dashboard | `OpenPlayerPanel("survivor_detail")` | `survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `survivor_relations` | Survivor Relations | Expanded | `OpenExpandedPanel("survivor_relations")` | `survivor_relationsHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `survivors` | Survivors Panel | Dashboard | `OpenPlayerPanel("survivors")` | `survivors` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `trade` | Trade / Economy Panel | Dashboard | `OpenPlayerPanel("trade")` | `economy, silent_foundry` | DashboardShellCloseButton | Yes | Yes | Interactive | Yes |
| `traveling_caravan` | Traveling Caravan | Expanded | `OpenExpandedPanel("traveling_caravan")` | `traveling_caravanHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `verdict` | Verdict Panel | Dashboard | `OpenPlayerPanel("verdict")` | `verdict` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `vinyl_morale` | Vinyl Morale | Expanded | `OpenExpandedPanel("vinyl_morale")` | `vinyl_moraleHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `water_treatment` | Water Treatment | Expanded | `OpenExpandedPanel("water_treatment")` | `water_treatmentHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `waystation_network` | Waystation Network | Expanded | `OpenExpandedPanel("waystation_network")` | `waystation_networkHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `weather` | Weather Panel | Dashboard | `OpenPlayerPanel("weather")` | `world` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `weather_detail` | Weather Detail | Dashboard | `OpenPlayerPanel("weather_detail")` | `world` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `weather_forecast` | Weather Forecast | Dashboard | `OpenPlayerPanel("weather_forecast")` | `world` | DashboardShellCloseButton | Yes | Yes | ReadOnly | — |
| `wildlife_trapping` | Wildlife Trapping | Expanded | `OpenExpandedPanel("wildlife_trapping")` | `wildlife_trappingHostSession` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `workshop` | Relic Workshop | Dashboard | `OpenPlayerPanel("workshop")` | `crafting, inventory, survivors` | DashboardShellCloseButton | Yes | Yes | Interactive | — |
| `codex` | Codex (Journal from menu) | MainMenu | `OpenPlayerPanel("codex")` | `journal` | DashboardShellCloseButton | Yes | Yes | ReadOnly | Yes |
| `settings` | Settings Panel | MainMenu | `OpenPlayerPanel("settings")` | `settingsHost` | OverlayCloseButton | — | Yes | Interactive | — |
