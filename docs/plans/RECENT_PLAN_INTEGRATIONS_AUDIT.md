# Recent Plan Integrations — Programmatic Audit

**Generated:** 2026-09-24

**Method:** programmatic source scan (Core type declarations, src references, SaveSectionRegistry, HostCli, SELFTEST_MANIFEST, test fixtures)

**Scope:** 44 recently integrated plans (UNBLOCK-OLDEST batches,
flagship integration commits, and the 2026-09-23 Plans 210/214 full-integration package).

Every verdict below is re-measured from current source: Core type declarations,
`src/` references, `SaveSectionRegistry`, `HostCli`/`SELFTEST_MANIFEST.json`, UI panel
files, route registration, and test fixtures. Ledger claims are not trusted.

## 1. Verdict summary

| Verdict | Plans |
|---|---:|
| **INTEGRATED** | 44 |

## 2. Per-plan verification

| Plan | Title | Save section | Host refs | Triad | CLI | UI | Tests | Verdict |
|---|---|---|---|---|---|---|---|---|
| 38 | Commitments & Deadlines | `commitment` | ✅ 2/2 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 39 | Session Durability | `session_durability` | ✅ 3/3 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 42 | Survivor Voice | `survivor_voice` | ✅ 3/3 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 46 | Playable Metrics | `playable_metrics` | ✅ 4/4 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 49 | Content Orphan Certification | read model | ✅ 1/1 identifiers referenced | — | ✅ | — | — | **INTEGRATED** |
| 51 | Holdfast Presentation Slate | read model | ✅ 1/1 identifiers referenced | — | ✅ | — | — | **INTEGRATED** |
| 52 | Scarcity Audio | read model | ✅ 2/2 identifiers referenced | — | ✅ | — | — | **INTEGRATED** |
| 54 | Seven-Day Slice | `seven_day_slice` | ✅ 3/3 identifiers referenced | ✅ | ✅ | — | — | **INTEGRATED** |
| 55 | Retention & 400-Year Campaign | `retention` | ✅ 3/4 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 58 | Outposts & Second Holdfast | `outpost_settlement` | ✅ 3/5 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 59 | Standing Gates Retrospective | read model | ✅ 2/2 identifiers referenced | — | ✅ | — | ✅ | **INTEGRATED** |
| 132 | Hidden Agendas & Betrayal | `hidden_agenda` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 134 | Territory & Supply Lines | `territory_control` | ✅ 3/7 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 135 | Weather Gameplay Cascade | `weather_cascade` | ✅ 5/5 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 136 | Cooking Pipeline | `cooking` | ✅ 7/8 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 137 | Needs→Performance Cascade | read model | ✅ 1/1 identifiers referenced | — | ✅ | — | ✅ | **INTEGRATED** |
| 138 | Shelter Defense & Security | `shelter_security` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 140 | Generational Legacy | `campaign_legacy` | ✅ 6/7 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 141 | Research Unlock Bridge | `research_unlock` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 145 | Unified Ending & Epilogue | `unified_ending` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 147 | NPC Memory & Relationships | `npc_memory` | ✅ 5/5 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 148 | Ideological Friction | `ideological_friction` | ✅ 5/5 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 150 | Romance & Family | `romance_family` | ✅ 7/7 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 152 | Vehicle Customization | `vehicle_customization` | ✅ 6/6 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 165 | Mod & Content-Pack Contract | read model | ✅ 1/1 identifiers referenced | — | ✅ | — | ✅ | **INTEGRATED** |
| 166 | Shelter Identity & Origin | `shelter_identity` | ✅ 3/3 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 167 | Tunnel Network | inside `wasteland_map` | ✅ 1/1 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 168 | Propaganda & Morale Warfare | `propaganda_campaigns` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 169 | Audio Accessibility | read model | ✅ 1/1 identifiers referenced | — | ✅ | — | ✅ | **INTEGRATED** |
| 171 | Dynamic Quest Generation | inside `procedural_narrative` | ✅ 1/1 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 174 | Survivor Backstories | `backstory` | ✅ 3/3 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 181 | Difficulty Settings *(uncommitted)* | `difficulty_settings` | ✅ 5/5 identifiers referenced | ✅ | ✅ | ✅ | — | **INTEGRATED** |
| 182 | Relationship Decay | `relationship_decay` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 186 | Shelter Maintenance *(uncommitted)* | `shelter_maintenance` | ✅ 4/4 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 188 | Survivor Daily Routines *(uncommitted)* | `survivor_routines` | ✅ 4/4 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 200 | Personal Quests | `personal_quests` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | — | **INTEGRATED** |
| 203 | Rumor Network | `wasteland_rumors` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | — | **INTEGRATED** |
| 205 | Shelter Noise | `shelter_noise` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | — | **INTEGRATED** |
| 206 | Death & Legacy | `death_legacy` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 207 | Shelter Reputation | `shelter_reputation` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 210 | Personal Belongings & Effects *(uncommitted)* | inside `survivor_social` | ✅ 2/2 identifiers referenced | ✅ | ✅ | — | ✅ | **INTEGRATED** |
| 212 | Time Capsules | `time_capsules` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 214 | Visitor Integration & Housing *(uncommitted)* | `visitor_integration` | ✅ 2/3 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |
| 220 | Shelter Atmosphere | `shelter_atmosphere` | ✅ 2/2 identifiers referenced | ✅ | ✅ | ✅ | ✅ | **INTEGRATED** |

## 3. Evidence detail

### Plan 38 — Commitments & Deadlines — INTEGRATED

- ✅ `authority`: CommitmentSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `commitment`
- ✅ `triad`: SetupCommitments / SaveCommitments
- ✅ `save_file`: commitment_save.json
- ✅ `save_store`: CommitmentSaveStore
- ✅ `host_session`: CommitmentHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --commitments-selftest
- ✅ `tests`: Plan38CommitmentHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.Commitments.cs`, `src/Host/CommitmentHostSession.cs`, `src/Host/HostCli.Commitments.cs`, `src/Host/HostCli.SliceScenario.cs`

### Plan 39 — Session Durability — INTEGRATED

- ✅ `authority`: SessionDurabilityManager [Core]
- ✅ `host_refs`: 3/3 identifiers referenced
- ✅ `save_section`: `session_durability`
- ✅ `triad`: SetupSessionDurability / SaveSessionDurability
- ✅ `save_file`: session_durability_save.json
- ✅ `save_store`: SessionDurabilitySaveStore
- ✅ `host_session`: SessionDurabilityHostSession, SaveLoadHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --session-durability-selftest
- ✅ `tests`: Plan39SessionDurabilityHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.SessionDurability.cs`, `src/Host/SessionDurabilityHostSession.cs`, `src/Host/HostCli.SessionDurability.cs`

### Plan 42 — Survivor Voice — INTEGRATED

- ✅ `authority`: SurvivorVoiceSystem [Core], VoiceLineDispatchCoordinator [Core]
- ✅ `host_refs`: 3/3 identifiers referenced
- ✅ `save_section`: `survivor_voice`
- ✅ `triad`: SetupSurvivorVoice / SaveSurvivorVoice
- ✅ `save_file`: survivor_voice_save.json
- ✅ `save_store`: SurvivorVoiceSaveStore
- ✅ `host_session`: SurvivorVoiceHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --survivor-voice-selftest
- ✅ `tests`: Plan42SurvivorVoiceHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.SurvivorVoice.cs`, `src/Main.ContentCertification.cs`, `src/Host/SurvivorVoiceHostSession.cs`, `src/Host/HostCli.SurvivorVoice.cs`, `src/Host/ContentCertificationHostSession.cs`

### Plan 46 — Playable Metrics — INTEGRATED

- ✅ `authority`: PlaySessionRecorder [Core], FirstHourFunnel [Core], PlayableMetricsAggregationEngine [Core]
- ✅ `host_refs`: 4/4 identifiers referenced
- ✅ `save_section`: `playable_metrics`
- ✅ `triad`: SetupPlayMetrics / SavePlayMetrics
- ✅ `save_file`: playable_metrics_save.json
- ✅ `save_store`: PlayMetricsSaveStore
- ✅ `host_session`: PlayMetricsHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --playable-metrics-selftest
- ✅ `tests`: Plan46PlayMetricsHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/PlayMetricsHostSession.cs`, `src/Host/HostCli.PlayMetrics.cs`

### Plan 49 — Content Orphan Certification — INTEGRATED

- ✅ `authority`: ContentOrphanCertificationEngine [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --content-certification-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Host/ContentCertificationHostSession.cs`

### Plan 51 — Holdfast Presentation Slate — INTEGRATED

- ✅ `authority`: HoldfastPresentationHostSession [Host]
- ✅ `host_refs`: 1/1 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- ✅ `host_session`: HoldfastPresentationHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --holdfast-presentation-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Main.HoldfastPresentation.cs`, `src/Host/HoldfastPresentationHostSession.cs`, `src/Host/HostCli.HoldfastPresentation.cs`

### Plan 52 — Scarcity Audio — INTEGRATED

- ✅ `authority`: ScarcityAudioController [Host], ScarcityAudioStateMachine [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- ✅ `host_session`: ScarcityAudioController
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --scarcity-audio-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Audio/ScarcityAudioController.cs`, `src/Audio/HostCli.ScarcityAudio.cs`

### Plan 54 — Seven-Day Slice — INTEGRATED

- ✅ `authority`: SliceScenario [Core], SliceScenarioCatalogLoader [Core]
- ✅ `host_refs`: 3/3 identifiers referenced
- ✅ `save_section`: `seven_day_slice`
- ✅ `triad`: SetupSevenDaySlice / SaveSevenDaySlice
- ✅ `save_file`: seven_day_slice_save.json
- ✅ `save_store`: SliceScenarioSaveStore
- ✅ `host_session`: SliceScenarioHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --seven-day-slice-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Main.SliceScenario.cs`, `src/Host/SliceScenarioHostSession.cs`

### Plan 55 — Retention & 400-Year Campaign — INTEGRATED

- ✅ `authority`: RetentionPolicyCatalog [Core], RetentionPolicyCatalogLoader [Core], RollingLog [Core]
- ✅ `host_refs`: 3/4 identifiers referenced
- ✅ `save_section`: `retention`
- ✅ `triad`: SetupRetention / SaveRetention
- ✅ `save_file`: retention_save.json
- ✅ `save_store`: RetentionSaveStore
- ✅ `host_session`: RetentionHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --retention-selftest
- ✅ `tests`: Plan55RetentionHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/RetentionHostSession.cs`

### Plan 58 — Outposts & Second Holdfast — INTEGRATED

- ✅ `authority`: OutpostSettlementSystem [Core], OutpostDef [Core], OutpostInstance [Core], OutpostSettlementState [Core]
- ✅ `host_refs`: 3/5 identifiers referenced
- ✅ `save_section`: `outpost_settlement`
- ✅ `triad`: SetupOutpostSettlement / SaveOutpostSettlement
- ✅ `save_file`: outpost_settlement_save.json
- ✅ `save_store`: OutpostSettlementSaveStore
- ✅ `host_session`: OutpostSettlementHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --outpost-settlement-selftest
- ✅ `tests`: Plan58OutpostHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.OutpostSettlement.cs`, `src/Host/OutpostSettlementHostSession.cs`

### Plan 59 — Standing Gates Retrospective — INTEGRATED

- ✅ `authority`: StandingGateRegistry [Core], StandingGatesHostSession [Host]
- ✅ `host_refs`: 2/2 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- ✅ `host_session`: StandingGatesHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --standing-gates-selftest
- ✅ `tests`: Plan59StandingGateHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/StandingGatesHostSession.cs`

### Plan 132 — Hidden Agendas & Betrayal — INTEGRATED

- ✅ `authority`: HiddenAgendaSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `hidden_agenda`
- ✅ `triad`: SetupHiddenAgenda / SaveHiddenAgenda
- ✅ `save_file`: hidden_agenda_save.json
- ✅ `save_store`: HiddenAgendaSaveStore
- ✅ `host_session`: HiddenAgendaHostSession
- ✅ `ui_panel`: HiddenAgendaPanel
- ✅ `route`: hidden_agenda
- ✅ `cli_flag`: --hidden-agenda-selftest
- ✅ `tests`: Plan132HiddenAgendaIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.HiddenAgenda.cs`, `src/Host/HiddenAgendaHostSession.cs`, `src/Host/HiddenAgendaSelfTest.cs`

### Plan 134 — Territory & Supply Lines — INTEGRATED

- ✅ `authority`: TerritoryControlSystem [Core], FactionTerritoryDef [Core], SupplyLineDef [Core], LocationTerritoryState [Core], SupplyLineState [Core], TerritoryControlSaveState [Core]
- ✅ `host_refs`: 3/7 identifiers referenced
- ✅ `save_section`: `territory_control`
- ✅ `triad`: SetupTerritoryControl / SaveTerritoryControl
- ✅ `save_file`: territory_control_save.json
- ✅ `save_store`: TerritoryControlSaveStore
- ✅ `host_session`: TerritoryControlHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --territory-control-selftest
- ✅ `tests`: Plan134TerritoryControlHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.TerritoryControl.cs`, `src/Host/TerritoryControlHostSession.cs`

### Plan 135 — Weather Gameplay Cascade — INTEGRATED

- ✅ `authority`: WeatherCascadeSystem [Core], WeatherGameplayCascadeEngine [Core], WeatherCascadeSeverity [Core], WeatherCascadeCatalogLoader [Core]
- ✅ `host_refs`: 5/5 identifiers referenced
- ✅ `save_section`: `weather_cascade`
- ✅ `triad`: SetupWeatherCascade / SaveWeatherCascade
- ✅ `save_file`: weather_cascade_save.json
- ✅ `save_store`: WeatherCascadeSaveStore
- ✅ `host_session`: WeatherCascadeHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --weather-cascade-selftest
- ✅ `tests`: Plan135WeatherCascadeHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/WeatherCascadeHostSession.cs`, `src/Host/HostCli.WeatherCascade.cs`

### Plan 136 — Cooking Pipeline — INTEGRATED

- ✅ `authority`: CookingSystem [Core], CookingRecipe [Core], CookingOperation [Core], CookingState [Core], CookingCensus [Core], CookingRecipeCatalogLoader [Core], InventoryCookingSource [Core]
- ✅ `host_refs`: 7/8 identifiers referenced
- ✅ `save_section`: `cooking`
- ✅ `triad`: SetupCooking / SaveCooking
- ✅ `save_file`: cooking_save.json
- ✅ `save_store`: CookingSaveStore
- ✅ `host_session`: CookingHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --cooking-selftest
- ✅ `tests`: Plan136CookingHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.Cooking.cs`, `src/Host/CookingHostSession.cs`, `src/Host/HostCli.Cooking.cs`

### Plan 137 — Needs→Performance Cascade — INTEGRATED

- ✅ `authority`: NeedsPerformanceBridge [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --needs-performance-selftest
- ✅ `tests`: NeedsPerformanceBridgeTests
- Host reference files (first authority, up to 8): `src/Host/NeedsPerformanceHostSession.cs`, `src/Host/HostCli.NeedsPerformance.cs`, `src/UI/SurvivorDetailPanel.cs`

### Plan 138 — Shelter Defense & Security — INTEGRATED

- ✅ `authority`: ShelterSecuritySystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `shelter_security`
- ✅ `triad`: SetupShelterSecurity / SaveShelterSecurity
- ✅ `save_file`: shelter_security_save.json
- ✅ `save_store`: ShelterSecuritySaveStore
- ✅ `host_session`: ShelterSecurityHostSession
- ✅ `ui_panel`: ShelterSecurityPanel
- ✅ `route`: shelter_security
- ✅ `cli_flag`: --shelter-security-selftest
- ✅ `tests`: Plan138ShelterSecurityIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.ShelterSecurity.cs`, `src/Host/ShelterSecurityHostSession.cs`, `src/Host/ShelterSecuritySelfTest.cs`

### Plan 140 — Generational Legacy — INTEGRATED

- ✅ `authority`: CampaignLegacySystem [Core], CampaignLegacy [Core], CampaignLegacyCensus [Core], CampaignLegacyState [Core], StartingCampaignContext [Core], LegacyTrait [Core]
- ✅ `host_refs`: 6/7 identifiers referenced
- ✅ `save_section`: `campaign_legacy`
- ✅ `triad`: SetupCampaignLegacy / SaveCampaignLegacy
- ✅ `save_file`: campaign_legacy_save.json
- ✅ `save_store`: CampaignLegacySaveStore
- ✅ `host_session`: CampaignLegacyHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --campaign-legacy-selftest
- ✅ `tests`: Plan140CampaignLegacyHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/CampaignLegacyHostSession.cs`, `src/Host/HostCli.CampaignLegacy.cs`

### Plan 141 — Research Unlock Bridge — INTEGRATED

- ✅ `authority`: ResearchUnlockBridge [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `research_unlock`
- ✅ `triad`: SetupResearchUnlockBridge / SaveResearchUnlock
- ✅ `save_file`: research_unlock_save.json
- ✅ `save_store`: ResearchUnlockSaveStore
- ✅ `host_session`: ResearchUnlockHostSession
- ✅ `ui_panel`: ResearchPanel
- ✅ `route`: research
- ✅ `cli_flag`: --research-unlock-selftest
- ✅ `tests`: Plan141ResearchUnlockHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/ResearchUnlockHostSession.cs`

### Plan 145 — Unified Ending & Epilogue — INTEGRATED

- ✅ `authority`: UnifiedEndingResolver [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `unified_ending`
- ✅ `triad`: SetupUnifiedEnding / SaveUnifiedEnding
- ✅ `save_file`: unified_ending_save.json
- ✅ `save_store`: UnifiedEndingSaveStore
- ✅ `host_session`: UnifiedEndingHostSession
- ✅ `ui_panel`: EpiloguePanel, ChroniclePanel
- ✅ `route`: epilogue, chronicle
- ✅ `cli_flag`: --unified-ending-selftest
- ✅ `tests`: Plan145UnifiedEndingHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.UnifiedEnding.cs`, `src/Host/UnifiedEndingHostSession.cs`, `src/Host/HostCli.UnifiedEnding.cs`

### Plan 147 — NPC Memory & Relationships — INTEGRATED

- ✅ `authority`: NpcMemorySystem [Core], NpcMemoryEntry [Core], NpcRelationship [Core], NpcMemoryCensus [Core]
- ✅ `host_refs`: 5/5 identifiers referenced
- ✅ `save_section`: `npc_memory`
- ✅ `triad`: SetupNpcMemory / SaveNpcMemory
- ✅ `save_file`: npc_memory_save.json
- ✅ `save_store`: NpcMemorySaveStore
- ✅ `host_session`: NpcMemoryHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --npc-memory-selftest
- ✅ `tests`: Plan147NpcMemoryHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/NpcMemoryHostSession.cs`

### Plan 148 — Ideological Friction — INTEGRATED

- ✅ `authority`: IdeologicalFrictionEvents [Core], IdeologicalFrictionSystem [Core], IdeologicalEventInstance [Core], IdeologicalFrictionCensus [Core]
- ✅ `host_refs`: 5/5 identifiers referenced
- ✅ `save_section`: `ideological_friction`
- ✅ `triad`: SetupIdeologicalFriction / SaveIdeologicalFriction
- ✅ `save_file`: ideological_friction_save.json
- ✅ `save_store`: IdeologicalFrictionSaveStore
- ✅ `host_session`: IdeologicalFrictionHostSession
- ✅ `ui_panel`: SurvivorsPanel, SurvivorDetailPanel
- ✅ `route`: survivors, survivor_detail
- ✅ `cli_flag`: --ideological-friction-selftest
- ✅ `tests`: Plan148IdeologicalFrictionHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/IdeologicalFrictionHostSession.cs`

### Plan 150 — Romance & Family — INTEGRATED

- ✅ `authority`: RomanceFamilySystem [Core], RomanticRelationship [Core], FamilyUnit [Core], RomanceCourtshipCatalog [Core], RomanceFamilyCensus [Core], RomanceCourtshipCatalogLoader [Core]
- ✅ `host_refs`: 7/7 identifiers referenced
- ✅ `save_section`: `romance_family`
- ✅ `triad`: SetupRomanceFamily / SaveRomanceFamily
- ✅ `save_file`: romance_family_save.json
- ✅ `save_store`: RomanceFamilySaveStore
- ✅ `host_session`: RomanceFamilyHostSession
- ✅ `ui_panel`: SurvivorDetailPanel
- ✅ `route`: survivor_detail
- ✅ `cli_flag`: --romance-family-selftest
- ✅ `tests`: Plan150RomanceFamilyHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/RomanceFamilyHostSession.cs`

### Plan 152 — Vehicle Customization — INTEGRATED

- ✅ `authority`: VehicleCustomizationSystem [Core], VehicleCustomizationCatalog [Core], VehicleModule [Core], VehicleCustomizationCensus [Core], VehicleModuleCatalogLoader [Core]
- ✅ `host_refs`: 6/6 identifiers referenced
- ✅ `save_section`: `vehicle_customization`
- ✅ `triad`: SetupVehicleCustomization / SaveVehicleCustomization
- ✅ `save_file`: vehicle_customization_save.json
- ✅ `save_store`: VehicleCustomizationSaveStore
- ✅ `host_session`: VehicleCustomizationHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --vehicle-customization-selftest
- ✅ `tests`: Plan152VehicleCustomizationHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/VehicleCustomizationHostSession.cs`

### Plan 165 — Mod & Content-Pack Contract — INTEGRATED

- ✅ `authority`: ModSupportSystem [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --mod-support-selftest
- ✅ `tests`: Plan165ModdingIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/ModRuntime.cs`, `src/Host/ModSupportHostSession.cs`

### Plan 166 — Shelter Identity & Origin — INTEGRATED

- ✅ `authority`: ShelterIdentitySystem [Core], ShelterOriginCatalogLoader [Core]
- ✅ `host_refs`: 3/3 identifiers referenced
- ✅ `save_section`: `shelter_identity`
- ✅ `triad`: SetupShelterIdentity / SaveShelterIdentity
- ✅ `save_file`: shelter_identity_save.json
- ✅ `save_store`: ShelterIdentitySaveStore
- ✅ `host_session`: ShelterIdentityHostSession
- ✅ `ui_panel`: ShelterPanel
- ✅ `route`: shelter
- ✅ `cli_flag`: --shelter-identity-selftest
- ✅ `tests`: ShelterIdentitySystemTests
- Host reference files (first authority, up to 8): `src/Main.ShelterIdentity.cs`, `src/Host/ShelterIdentityHostSession.cs`

### Plan 167 — Tunnel Network — INTEGRATED

- ✅ `authority`: TunnelNetworkSystem [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- ✅ `save_section`: `wasteland_map` (nested)
- ✅ `triad`: — / SaveWastelandMap
- ✅ `save_file`: persisted inside parent aggregate
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --tunnel-network-selftest
- ✅ `tests`: Plan167TunnelNetworkIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.TunnelNetwork.cs`, `src/Host/HostCli.TunnelNetwork.cs`

### Plan 168 — Propaganda & Morale Warfare — INTEGRATED

- ✅ `authority`: PropagandaSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `propaganda_campaigns`
- ✅ `triad`: SetupPropaganda / SavePropaganda
- ✅ `save_file`: propaganda_save.json
- ✅ `save_store`: PropagandaSaveStore
- ✅ `host_session`: PropagandaHostSession
- ✅ `ui_panel`: PropagandaPanel
- ✅ `route`: propaganda
- ✅ `cli_flag`: --propaganda-selftest
- ✅ `tests`: Plan168PropagandaIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.Propaganda.cs`, `src/Host/PropagandaHostSession.cs`, `src/Host/PropagandaSelfTest.cs`

### Plan 169 — Audio Accessibility — INTEGRATED

- ✅ `authority`: AudioAccessibilityCoordinator [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- — `save_section`: read model / no own section
- — `triad`: —
- — `save_file`: —
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --audio-accessibility-selftest
- ✅ `tests`: Plan169AudioAccessibilityIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.AudioAccessibility.cs`, `src/Host/AudioAccessibilityHostSession.cs`, `src/Host/HostCli.AudioAccessibility.cs`

### Plan 171 — Dynamic Quest Generation — INTEGRATED

- ✅ `authority`: DynamicQuestGenerator [Core]
- ✅ `host_refs`: 1/1 identifiers referenced
- ✅ `save_section`: `procedural_narrative` (nested)
- ✅ `triad`: SetupPlans166To169 / SaveProceduralNarrative
- ✅ `save_file`: persisted inside parent aggregate
- — `save_store`: —
- — `host_session`: wired through Main
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --dynamic-quest-selftest
- ✅ `tests`: DynamicQuestGeneratorTests
- Host reference files (first authority, up to 8): `src/Main.DynamicQuestGeneration.cs`, `src/Host/ProceduralNarrativeHostSession.cs`, `src/Host/DynamicQuestHostSession.cs`, `src/Host/HostCli.DynamicQuest.cs`

### Plan 174 — Survivor Backstories — INTEGRATED

- ✅ `authority`: BackstorySystem [Core], BackstoryCensus [Core]
- ✅ `host_refs`: 3/3 identifiers referenced
- ✅ `save_section`: `backstory`
- ✅ `triad`: SetupBackstory / SaveBackstory
- ✅ `save_file`: backstory_save.json
- ✅ `save_store`: BackstorySaveStore
- ✅ `host_session`: BackstoryHostSession
- ✅ `ui_panel`: SurvivorDetailPanel
- ✅ `route`: survivor_detail, survivors
- ✅ `cli_flag`: --backstory-selftest
- ✅ `tests`: Plan174BackstoryHostIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/BackstoryHostSession.cs`

### Plan 181 — Difficulty Settings — INTEGRATED

- ✅ `authority`: DifficultySettingsSystem [Core], DifficultySettingsCensus [Core], DifficultyPresetCatalog [Core], DifficultyScalarsProvider [Core]
- ✅ `host_refs`: 5/5 identifiers referenced
- ✅ `save_section`: `difficulty_settings`
- ✅ `triad`: SetupDifficultySettings / SaveDifficultySettings
- ✅ `save_file`: difficulty_settings_save.json
- ✅ `save_store`: DifficultySettingsSaveStore
- ✅ `host_session`: DifficultySettingsHostSession
- ✅ `ui_panel`: StartingCohortSetupPanel
- ✅ `route`: protocol
- ✅ `cli_flag`: --difficulty-settings-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Main.DifficultySettings.cs`, `src/Host/DifficultySettingsHostSession.cs`, `src/Host/HostCli.DifficultySettings.cs`

### Plan 182 — Relationship Decay — INTEGRATED

- ✅ `authority`: RelationshipDecaySystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `relationship_decay`
- ✅ `triad`: SetupRelationshipDecay / SaveRelationshipDecay
- ✅ `save_file`: relationship_decay_save.json
- ✅ `save_store`: RelationshipDecaySaveStore
- ✅ `host_session`: RelationshipDecayHostSession
- ✅ `ui_panel`: RelationshipDecayPanel
- ✅ `route`: relationship_decay
- ✅ `cli_flag`: --relationship-decay-selftest
- ✅ `tests`: Plan182RelationshipDecayIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.RelationshipDecay.cs`, `src/Host/RelationshipDecayHostSession.cs`, `src/Host/RelationshipDecaySelfTest.cs`

### Plan 186 — Shelter Maintenance — INTEGRATED

- ✅ `authority`: ShelterMaintenanceSystem [Core], ShelterComponentCatalogLoader [Core], ShelterMaintenanceCensus [Core]
- ✅ `host_refs`: 4/4 identifiers referenced
- ✅ `save_section`: `shelter_maintenance`
- ✅ `triad`: SetupShelterMaintenance / SaveShelterMaintenance
- ✅ `save_file`: shelter_maintenance_save.json
- ✅ `save_store`: ShelterMaintenanceSaveStore
- ✅ `host_session`: ShelterMaintenanceHostSession
- ✅ `ui_panel`: SurvivorDetailPanel
- ✅ `route`: survivor_detail
- ✅ `cli_flag`: --shelter-maintenance-selftest
- ✅ `tests`: Plan186ShelterMaintenanceIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.ShelterMaintenance.cs`, `src/Host/ShelterMaintenanceHostSession.cs`

### Plan 188 — Survivor Daily Routines — INTEGRATED

- ✅ `authority`: SurvivorRoutineSystem [Core], RoutineTemplateCatalogLoader [Core], SurvivorRoutineCensus [Core]
- ✅ `host_refs`: 4/4 identifiers referenced
- ✅ `save_section`: `survivor_routines`
- ✅ `triad`: SetupSurvivorRoutines / SaveSurvivorRoutines
- ✅ `save_file`: survivor_routines_save.json
- ✅ `save_store`: SurvivorRoutineSaveStore
- ✅ `host_session`: SurvivorRoutineHostSession
- ✅ `ui_panel`: SurvivorDetailPanel
- ✅ `route`: survivor_detail
- ✅ `cli_flag`: --survivor-routines-selftest
- ✅ `tests`: Plan188SurvivorRoutineIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.SurvivorRoutines.cs`, `src/Host/SurvivorRoutineHostSession.cs`

### Plan 200 — Personal Quests — INTEGRATED

- ✅ `authority`: PersonalQuestSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `personal_quests`
- ✅ `triad`: SetupPersonalQuests / SavePersonalQuests
- ✅ `save_file`: personal_quests_save.json
- ✅ `save_store`: PersonalQuestSaveStore
- ✅ `host_session`: PersonalQuestHostSession
- ✅ `ui_panel`: PersonalQuestPanel
- ✅ `route`: personal_quests
- ✅ `cli_flag`: --personal-quests-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Host/PersonalQuestHostSession.cs`

### Plan 203 — Rumor Network — INTEGRATED

- ✅ `authority`: RumorSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `wasteland_rumors`
- ✅ `triad`: SetupRumorNetwork / SaveRumorNetwork
- ✅ `save_file`: rumor_network_save.json
- ✅ `save_store`: RumorNetworkSaveStore
- ✅ `host_session`: RumorNetworkHostSession
- ✅ `ui_panel`: RumorBoardPanel
- ✅ `route`: rumors
- ✅ `cli_flag`: --rumor-network-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Main.RumorNetwork.cs`, `src/Host/RumorNetworkHostSession.cs`, `src/Host/RumorNetworkSelfTest.cs`

### Plan 205 — Shelter Noise — INTEGRATED

- ✅ `authority`: ShelterNoiseSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `shelter_noise`
- ✅ `triad`: SetupShelterAtmosphere / SaveShelterAtmosphere
- ✅ `save_file`: shelter_noise_save.json
- ✅ `save_store`: ShelterNoiseSaveStore
- ✅ `host_session`: ShelterAtmosphereHostSession
- ✅ `ui_panel`: ShelterAtmospherePanel
- ✅ `route`: shelter_atmosphere
- ✅ `cli_flag`: --shelter-atmosphere-selftest
- — `tests`: no named fixture (host selftest only)
- Host reference files (first authority, up to 8): `src/Main.ShelterAtmosphere.cs`, `src/Host/ShelterAtmosphereHostSession.cs`, `src/Host/ShelterAtmosphereSelfTest.cs`

### Plan 206 — Death & Legacy — INTEGRATED

- ✅ `authority`: SurvivorDeathLegacySystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `death_legacy`
- ✅ `triad`: SetupDeathLegacy / SaveDeathLegacy
- ✅ `save_file`: death_legacy_save.json
- ✅ `save_store`: SurvivorDeathLegacySaveStore
- ✅ `host_session`: SurvivorDeathLegacyHostSession
- ✅ `ui_panel`: SurvivorDeathLegacyPanel
- ✅ `route`: death_legacy
- ✅ `cli_flag`: --death-legacy-selftest
- ✅ `tests`: Plan206SurvivorDeathLegacyIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.SurvivorDeathLegacy.cs`, `src/Host/SurvivorDeathLegacyHostSession.cs`, `src/Host/SurvivorDeathLegacySelfTest.cs`

### Plan 207 — Shelter Reputation — INTEGRATED

- ✅ `authority`: ShelterReputationSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `shelter_reputation`
- ✅ `triad`: SetupShelterReputation / SaveShelterReputation
- ✅ `save_file`: shelter_reputation_save.json
- ✅ `save_store`: ShelterReputationSaveStore
- ✅ `host_session`: ShelterReputationHostSession
- ✅ `ui_panel`: ShelterReputationPanel
- ✅ `route`: shelter_reputation
- ✅ `cli_flag`: --shelter-reputation-selftest
- ✅ `tests`: Plan207ShelterReputationIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.ShelterReputation.cs`, `src/Host/ShelterReputationHostSession.cs`, `src/Host/ShelterReputationSelfTest.cs`

### Plan 210 — Personal Belongings & Effects — INTEGRATED

- ✅ `authority`: PersonalBelongingsSystem [Core], PersonalBelongingsHostSession [Host]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `survivor_social` (nested)
- ✅ `triad`: SetupSurvivorSocial / SaveSurvivorSocial
- ✅ `save_file`: persisted inside parent aggregate
- — `save_store`: —
- ✅ `host_session`: PersonalBelongingsHostSession
- — `ui_panel`: no dedicated panel (read model / detail rows)
- — `route`: —
- ✅ `cli_flag`: --personal-belongings-selftest
- ✅ `tests`: Plan210PersonalBelongingsIntegrationTests
- Host reference files (first authority, up to 8): `src/Host/PersonalBelongingsHostSession.cs`, `src/Host/PersonalBelongingsSelfTest.cs`

### Plan 212 — Time Capsules — INTEGRATED

- ✅ `authority`: TimeCapsuleSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `time_capsules`
- ✅ `triad`: SetupTimeCapsules / SaveTimeCapsules
- ✅ `save_file`: time_capsules_save.json
- ✅ `save_store`: TimeCapsuleSaveStore
- ✅ `host_session`: TimeCapsuleHostSession
- ✅ `ui_panel`: TimeCapsulePanel
- ✅ `route`: time_capsule
- ✅ `cli_flag`: --time-capsule-selftest
- ✅ `tests`: Plan212TimeCapsuleIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.TimeCapsule.cs`, `src/Host/TimeCapsuleHostSession.cs`, `src/Host/TimeCapsuleSelfTest.cs`

### Plan 214 — Visitor Integration & Housing — INTEGRATED

- ✅ `authority`: VisitorIntegrationSystem [Core], VisitorCatalogData [Core]
- ✅ `host_refs`: 2/3 identifiers referenced
- ✅ `save_section`: `visitor_integration`
- ✅ `triad`: SetupVisitorIntegration / SaveVisitorIntegration
- ✅ `save_file`: visitor_integration_save.json
- ✅ `save_store`: VisitorIntegrationSaveStore
- ✅ `host_session`: VisitorIntegrationHostSession
- ✅ `ui_panel`: VisitorIntegrationPanel
- ✅ `route`: visitor_integration
- ✅ `cli_flag`: --visitor-integration-selftest
- ✅ `tests`: Plan214VisitorIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.VisitorIntegration.cs`, `src/Host/VisitorIntegrationHostSession.cs`, `src/Host/VisitorIntegrationSelfTest.cs`

### Plan 220 — Shelter Atmosphere — INTEGRATED

- ✅ `authority`: ShelterAtmosphereSystem [Core]
- ✅ `host_refs`: 2/2 identifiers referenced
- ✅ `save_section`: `shelter_atmosphere`
- ✅ `triad`: SetupShelterAtmosphere / SaveShelterAtmosphere
- ✅ `save_file`: shelter_atmosphere_save.json
- ✅ `save_store`: ShelterAtmosphereSaveStore
- ✅ `host_session`: ShelterAtmosphereHostSession
- ✅ `ui_panel`: ShelterAtmospherePanel
- ✅ `route`: shelter_atmosphere
- ✅ `cli_flag`: --shelter-atmosphere-selftest
- ✅ `tests`: Plan220ShelterAtmosphereIntegrationTests
- Host reference files (first authority, up to 8): `src/Main.ShelterAtmosphere.cs`, `src/Host/ShelterAtmosphereHostSession.cs`, `src/Host/ShelterAtmosphereSelfTest.cs`

## 4. Notes and constraints

- Plans 181/186/188 (difficulty, shelter maintenance, survivor routines) and
  210/214 are present in the current worktree but not yet committed; their
  evidence is measured against the working tree, which is the current truth.
- Read-model plans (49, 51, 52, 59, 137, 165, 169) intentionally own no save
  section; persistence belongs to the canonical owners they project from.
- Nested persistence plans: 167 (`wasteland_map`), 171 (`procedural_narrative`),
  210 (`survivor_social`) — one aggregate owner, no parallel save section.

