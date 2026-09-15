# Plans 170–199 Forensic Audit

Status: `AUDIT COMPLETE` · Date: 2026-09-12 · Owner: Foreman

## Decision

Plans 170–199 are historical feature proposals, not a single executable
backlog. Their numbers must not be used as implementation identifiers: the
current `src/Main.Plans178_181.cs` grouping, for example, owns unrelated
expansion systems. This audit therefore compares each plan's intended authority
with current Core, data, host, save, and focused-test evidence.

Classifications:

- `CURRENT EQUIVALENT` — current architecture provides the plan's primary
  authority, even where class names or plan numbers differ.
- `PARTIAL / DO NOT DUPLICATE` — an adjacent authority exists, but it does not
  satisfy the requested contract. Extend it only through a new package.
- `UNSTARTED` — neither the proposed authority nor a safe equivalent exists.
- `BLOCKED` — the proposal needs an upstream owner or product decision first.

## Thirty-plan evidence table

| Plan | Classification | Current evidence | Foreman disposition |
|---|---|---|---|
| 170 Seasonal events | CURRENT EQUIVALENT | `Assets/Ashfall.Core/World/SeasonalEventSystem.cs`, `SeasonalEventCatalog.cs`, `WeatherIntelligenceCoordinator.cs`, and `Plan19DynamicWorldTests.cs` | Keep current authority; do not create a second calendar system. |
| 171 Dynamic quests | CURRENT EQUIVALENT | `Quests/QuestRuntimeCoordinator.cs`, `Quests/DynamicQuestlines.cs`, `Narrative/ProceduralNarrativeSystem.cs`, `Plan169ProceduralNarrativeTests.cs`, and `DynamicQuestlineTests.cs` | Reconcile old proposal only if a concrete missing quest contract is found. |
| 172 Radiation mutation | CURRENT EQUIVALENT | `Medical/MutationSystem.cs`, `mutations.json`, `MutationSaveStore.cs`, `MutationTreePanel.cs`, and `MutationSystemTests.cs` | Preserve the current medical mutation owner; old proposed path is stale. |
| 173 Radio production | PARTIAL / DO NOT DUPLICATE | `Radio/ShelterRadioStationSystem.cs` and station catalog/tests own reception and stations; no `RadioProgramProductionSystem` or `radio_programs.json` exists. | Promotion candidate: design against existing schedule/delivery/propaganda interfaces. |
| 174 Backstories | UNSTARTED | No `BackstorySystem`, `backstory_templates.json`, or host/save route. | Requires a new narrative ownership decision; do not infer one from survivor data. |
| 175 Meta/New Game+ | BLOCKED | No meta-profile/New Game+ authority or independent profile store; the proposal depends on separate completion and achievement owners. | First prove current completion/achievement contracts and profile storage boundary. |
| 176 Aging | PARTIAL / DO NOT DUPLICATE | `Legacy/GenerationalSuccessionEngine.cs` exists, but no current `AgingSystem` or elder lifecycle authority exists. | Do not extend legacy migration code into gameplay. |
| 177 Dreams | PARTIAL / DO NOT DUPLICATE | Trauma, insomnia, and psychological systems exist, but no `DreamSystem` or dream-template data exists. | New narrative/mental-health contract required. |
| 178 Art and culture | PARTIAL / DO NOT DUPLICATE | `Culture/CulturalArchiveVaultSystem.cs`, catalog, host composition, and tests own archive material; no art-creation authority or templates exist. | Extend only through a separate creation-to-archive interface design. |
| 179 Psychology/phobias | PARTIAL / DO NOT DUPLICATE | `Survivors/PsychologicalArcSystem.cs`, sanatorium therapy, and focused tests exist; no unified profile/phobia authority or catalog exists. | Keep treatment/arc owner intact; profile model requires a new scope decision. |
| 180 Certification | PARTIAL / DO NOT DUPLICATE | Skill progression is current (`skills.json` is consumed), but no certification system/catalog exists. | Define whether certification is a skill-progression extension before coding. |
| 181 Difficulty | UNSTARTED | No `DifficultySettingsSystem` or preset data is present. | Global balancing authority requires explicit product approval and bounded simulation acceptance. |
| 182 Relationship drift | PARTIAL / DO NOT DUPLICATE | `SurvivorRelationsSystem.cs`, duty roster, and shelter assignment exist; no decay/drift authority exists. | Extend relation authority only after save/determinism review. |
| 183 Child development | PARTIAL / DO NOT DUPLICATE | `CohortSystem.cs` and starting-cohort data are setup systems, not child-development stages. | Do not repurpose starting cohort state as lifecycle state. |
| 184 Accessibility settings | PARTIAL / DO NOT DUPLICATE | Direct rebase inspection found persisted `Settings/UserSettingsData`, recovery codec, `src/Settings/UserSettingsStore`, and `SettingsPanel` controls for high contrast, hazard text, reduced motion, and large fonts. Those flags have no runtime presentation consumer outside settings/recovery tests. | Extend the existing preference path; first make the four exposed options effective through a bounded presentation bridge. Do not create a duplicate settings store or claim unimplemented accessibility modalities. |
| 185 Memory decay | PARTIAL / DO NOT DUPLICATE | Phantom-memory and knowledge-adjacent systems exist, but no memory/knowledge decay authority or rate data exists. | Requires a bounded definition of mutable knowledge before promotion. |
| 186 Shelter maintenance | PARTIAL / DO NOT DUPLICATE | Individual degradation systems exist (equipment, landmarks, thermal/power/water owners); no unified shelter-maintenance authority exists. | Do not create a second health/degradation ledger. |
| 187 Bestiary tracking | CURRENT EQUIVALENT | Direct rebase inspection found `World/WildlifeEcosystemSystem.RecordObservation`, persisted `WildlifeEcosystemState.observations`, `WildlifeEcosystemSaveStore`, `BestiaryPanel` knowledge/population gating, and `WildlifeEcosystemSystemTests.BestiaryKnowledge_IsObservationGated`. The narrative catalog is separate flavor data, not the gameplay authority. | Keep the ecology-owned observation ledger and panel; do not add a second bestiary progress save. |
| 188 Daily routines | PARTIAL / DO NOT DUPLICATE | `ShelterScheduleSystem.cs`, duty roster, and survivor needs are current; no individual routine authority/templates exist. | Decide whether routines are a schedule extension or a new deterministic owner. |
| 189 Water sources | PARTIAL / DO NOT DUPLICATE | Water treatment, sump, and contamination-adjacent systems exist; no `WaterSourceSystem` or source catalog exists. | Coordinate with deferred Plan 168 water delivery; never create a second bulk-water ledger. |
| 190 Item lore | PARTIAL / DO NOT DUPLICATE | `InventoryProvenance.cs`, campaign/map provenance, and relic provenance catalog exist; no `ItemLoreSystem` or lore-template data exists. | Separate immutable item lore from mutation provenance before promotion. |
| 191 Identification | UNSTARTED | No item-identification/appraisal authority, catalog, or targeted tests are present. | Requires inventory instance and economy pricing contracts. |
| 192 Trade routes | PARTIAL / DO NOT DUPLICATE | `CaravanTradeRouteCatalog.cs` exists; no player-owned route authority or agreement catalog exists. | Extend caravan/economy ownership only with standing/raid/save seams named first. |
| 193 Chronic conditions | PARTIAL / DO NOT DUPLICATE | `MedicalPipelineCoordinator.cs`, disease, radiation, and needs are current; no chronic-condition authority/catalog exists. | Requires medical save ownership and non-stigmatizing player-facing design. |
| 194 Emergency alerts | PARTIAL / DO NOT DUPLICATE | `EmergencyResponseHud.cs` exists, but no canonical Core alert controller or alert save state is evidenced. | Promote only after mapping weather, fire, radiation, flood, disease, and radio producers. |
| 195 Specialization roles | PARTIAL / DO NOT DUPLICATE | Apprenticeship, duty roster, skills, and lifecycle owners exist; no survivor-role authority/catalog exists. | Must extend existing skills/duty contracts rather than add a parallel role counter. |
| 196 Food type/spoilage | PARTIAL / DO NOT DUPLICATE | `Shelter/FoodPreservationSystem.cs`, `food_preservation.json`, host save path, and focused tests exist; no food-type/temperature authority or catalog exists. | Promotion candidate: define one inventory/thermal/preservation seam with deterministic spoilage rules. |
| 197 Diplomacy/treaties | CURRENT EQUIVALENT | `RegionalTreatySystem.cs`, narrative feed, host session/panel/save path, integration tests, and host catalog load are live. | Do not create `FactionDiplomacySystem`; promote only an evidence-backed missing treaty consequence. |
| 198 Health history | PARTIAL / DO NOT DUPLICATE | Medical pipeline and dose ledger are current, but no health-history/medical-record authority or template catalog exists. | Define record privacy, retention, and event consumers before promotion. |
| 199 Human/faction migration | PARTIAL / DO NOT DUPLICATE | Wildlife migration exists; no human/faction seasonal migration system or route catalog exists. | Keep wildlife migration separate; needs world/caravan/faction authority map. |

## Promotion queue — maximum three, no implementation yet

1. **Plan 184 — Accessibility preference application.** The preference state,
   persistence, and panel are already current, but the exposed presentation
   flags are not consumed at runtime. Bind only those existing flags through a
   bounded central presentation seam before considering new settings.
2. **Plan 196 — Food type and temperature contract.** Current preservation
   authority, weather, thermal, and inventory exist. The package must first
   name the single perishable-item authority and avoid a parallel food ledger.
3. **Plan 173 — Radio program production.** Highest player-facing value, but
   only after an API map proves how it consumes (rather than duplicates) the
   existing broadcast schedule, delivery, and propaganda results.

The Plan 187 candidate was removed during the active partial rebase: the
current wildlife ecosystem already owns its persisted observation ledger and
the Bestiary UI consumes it directly.

Plans 175, 181, 189, 193, 194, and 199 are intentionally not promotion
candidates: each crosses multiple authority boundaries and needs a product or
architecture decision before implementation.

## Findings suitable for cheap sweep agents

| Finding | Severity | Confidence | Evidence | Expected contract | Owner |
|---|---|---|---|---|---|
| F-170-199-NUMBER-DRIFT | High process risk | High | `src/Main.Plans178_181.cs` groups systems unrelated to historical Plans 178–181 | Plan numbers are not implementation identity | Foreman |
| F-173-PROGRAM-GAP | Feature gap | High | Current `ShelterRadioStationSystem`/catalog/tests; absent production system/data | Programs consume existing radio authorities | Integrator |
| F-184-PREFERENCE-APPLICATION-GAP | Feature gap | High | `UserSettingsData` and `SettingsPanel` persist/expose four accessibility flags, while source search finds no presentation consumer | Existing flags must alter runtime presentation through one bounded central seam | Integrator |
| F-187-DISCOVERY-GAP | Resolved classification error | High | `WildlifeEcosystemSystem.RecordObservation`, saved observations, `BestiaryPanel`, and the focused knowledge-gate test | Existing ecology authority remains the only discovery ledger | Foreman |
| F-196-THERMAL-SPOILAGE-GAP | Feature gap | High | Preservation system/data/tests exist; proposed food-type system/data absent | One deterministic inventory/thermal/preservation seam | Integrator |

## Audit limits

This is a source/data/host/test ownership audit, not a declaration that every
existing equivalent is feature-complete. It does not run broad suites, create
tests, alter production code, re-enable quarantined tests, or restore Unity.
