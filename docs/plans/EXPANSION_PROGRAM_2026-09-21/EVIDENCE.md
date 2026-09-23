# EVIDENCE — Expansion & Integration Program audit (2026-09-21)

Read-only audit of HEAD `5be1a30a`. No production file was modified by the
audit that produced this file and the six plans in this directory.

## 1. Method — host reachability closure

Grep-only scans produce false positives (a Core system composed inside another
Core system that *is* host-wired never appears in `src/`). The audit therefore
computes a reachability closure:

1. Parse every type definition (`class/record/struct/interface/enum/delegate`,
   public or internal) across `Assets/Ashfall.Core/**` (1,168 `.cs` files) and
   map type → defining file.
2. Build a file→file edge graph from identifier mentions between Core files.
3. **Roots** = Core type names that appear anywhere in `src/**`
   (844 `.cs` files).
4. BFS the closure. Any authority file (`*System|*Engine|*Coordinator|*Manager`)
   outside the closure is **host-unreachable**.
5. Cross-check references in `Ashfall.Core.Tests/**` (1,378 `.cs` files) and
   whether the authority is exercised by a CLI selftest.

Result: **99 unreachable authority files**, all with at least one test
reference. A second, type-level pass over all 442 Core authority types finds
**5 fully dead authorities** — the type name is referenced by no host file, no
test, and no other Core file (these sit inside files that are otherwise
reachable through a catalog loader):

| Authority | File | Triage |
|---|---|---|
| `DraisineRecoverySystem`, `ArmoredDraisineRecoverySystem` | `Expeditions/DraisineRerailingSystem.cs` | retire or fold into rail maintenance |
| `PowderMetallurgyEngine` | `Foundry/PowderMetallurgySystem.cs` | wire through `SilentFoundryHostSession` |
| `LyophilizationEngine` | `Medical/LyophilizationSystem.cs` | wire in the table/water package |
| `NvisC4ISystem` | `Radio/NvisCommunicationsSystem.cs` | wire in the comms package or retire |

## 2. Registry statistics (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| Core `.cs` files | 1,168 | `find Assets/Ashfall.Core -name '*.cs' \| wc -l` |
| Host `.cs` files | 844 | `find src -name '*.cs' \| wc -l` |
| Test `.cs` files | 1,378 | `find Ashfall.Core.Tests -name '*.cs' \| wc -l` |
| Data catalogs | 703 | `find Assets/StreamingAssets/Data -name '*.json' \| wc -l` |
| Host `Setup*` methods | 226 | `grep -rhoP 'private void Setup\w+' src/Main*.cs \| sort -u \| wc -l` |
| Host session/store files | 780 | `ls src/Host \| wc -l` |
| CLI selftest verbs | 217 | `grep -rhoP '"--[a-z0-9-]*selftest"' src/ \| sort -u \| wc -l` |
| `SubsystemManifest` entries | 18 | `grep -cE '^\s+new\($' Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` |
| Decision register rows | 301 | `grep -c '^\| `DEC-' docs/governance/DECISION_REGISTER.md` |
| Non-terminal register rows | 2 | DEC-11 (VO), DEC-13 (localization) |
| Ownership claims | 148 | `grep -c '^\| claim-' WORKTREE_OWNERSHIP.md` |

## 3. Plan-number collision evidence

The UNBLOCK waves reused numbers that the historical `piagentsplans/` corpus
and the host `Main.PlansNNN.cs` partials already used for different features:

| Host partial | Host feature | Ledger feature for the same number |
|---|---|---|
| `src/Main.Plans152.cs` | Black Projects Intelligence Archive | Plan 152 Vehicle Customization (`VehicleCustomizationSystem`) |
| `src/Main.Plans157.cs` | Grain Milling Discovery | Plan 157 Communications & Radio Network (`CommunicationsSystem`) |
| `src/Main.Plans158.cs` | Cordage/Cable/Technical Textiles | Plan 158 Disaster & Emergency Response (`DisasterResponseSystem`) |
| `src/Main.Plans159.cs` | Tanning & Leather | (ledger wave row) |
| `src/Main.Plans162_165.cs` | Agriculture + Nutrition Diversity | Plans 162–165 Shelter Archive / Cartography / Nuclear Winter / Modding |
| `src/Main.Plans163_210.cs` | Cartography projection (Plan 163) + belongings | matches partially — the only collision pair that was noticed and named in-file |

Consequence: a subsystem must be identified by subsystem name in claims and
handoffs, never by plan number alone.

## 4. Signed dispositions that are still host-pending

| Signed artifact | Host refs | Tests |
|---|---:|---:|
| `FundsLedger` (DEC-26) | 0 | 5 |
| `TradeRouteContract` (DEC-30) | 0 | 3 |
| `CrossRunProfileStore` (DEC-33) | 0 | 1 |
| `SeasonalHumanMigrationEngine` (DEC-34) | 0 | 2 |
| `RestockAllocationEngine` (DEC-37) | 0 | 1 |
| `SurvivorBodyState` (DEC-38) | 0 | 3 |
| `DifficultyConsequenceWeave` (DEC-40) | 0 | 1 |
| `RehabilitationSlateProjection` (DEC-42) | 0 | 1 |
| `UndergroundEconomyPressure` (DEC-43) | 0 | 1 |
| `BootstrapLifecycleGate` (DEC-44) | 0 | 1 |
| `SurvivorOriginModifier` (DEC-32) | 0 | service tests |
| `SleepNarrativeProjection` | 1 | 2 — proof the adapter pattern is cheap |

## 5. Full host-unreachable authority inventory (99 files)
| Authority | Core file (relative to `Assets/Ashfall.Core/`) | Test files |
|---|---|---:|
| `CascadeTargetSystem` | `Weather/WeatherGameplayCascadeEngine.cs` | 2 |
| `SeasonalHumanMigrationEngine` | `Economy/SeasonalHumanMigrationEngine.cs` | 1 |
| `RestockAllocationEngine` | `Economy/RestockAllocationEngine.cs` | 1 |
| `TradeRouteRiskBindingEngine` | `Economy/TradeRouteRiskBindingEngine.cs` | 1 |
| `BlackMarketContrabandEngine` | `Economy/BlackMarketContrabandEngine.cs` | 1 |
| `ChitPurityAssayEngine` | `Economy/ChitPurityAssayEngine.cs` | 1 |
| `LoanSharkEnforcerEngine` | `Economy/LoanSharkEnforcerEngine.cs` | 1 |
| `TradeRouteMonopolyEngine` | `Economy/TradeRouteMonopolyEngine.cs` | 1 |
| `MigrationConsequenceEngine` | `Economy/MigrationConsequenceEngine.cs` | 1 |
| `BlackMarketHeatAttentionEngine` | `Economy/BlackMarketHeatAttentionEngine.cs` | 1 |
| `SurvivorBarterSystem` | `Economy/SurvivorBarterSystem.cs` | 1 |
| `ColonySystem` | `Expeditions/ColonySystem.cs` | 1 |
| `AerialReconWindowEngine` | `Expeditions/AerialReconWindowEngine.cs` | 1 |
| `ClothingWarmthSystem` | `Inventory/ClothingWarmthSystem.cs` | 1 |
| `RehabilitationProgressionEngine` | `Medical/RehabilitationProgressionEngine.cs` | 1 |
| `ProstheticConditionWearEngine` | `Medical/ProstheticConditionWearEngine.cs` | 1 |
| `SurgicalGraftRejectionEngine` | `Medical/SurgicalGraftRejectionEngine.cs` | 1 |
| `PalliativeCareDignityEngine` | `Medical/PalliativeCareDignityEngine.cs` | 1 |
| `DependencyTaperWithdrawalEngine` | `Medical/DependencyTaperWithdrawalEngine.cs` | 1 |
| `ClinicalWardTriageEngine` | `Medical/ClinicalWardTriageEngine.cs` | 1 |
| `SurvivorLetterDeliverySystem` | `Narrative/SurvivorLetterDeliverySystem.cs` | 1 |
| `LetterDeliverySystem` | `Narrative/LetterDeliverySystem.cs` | 1 |
| `NpcMemorySystem` | `Narrative/NpcMemorySystem.cs` | 1 |
| `RadioPropagationEngine` | `Radio/RadioPropagation.cs` | 1 |
| `CupolaFoundryEngine` | `Shelter/CupolaFoundryEngine.cs` | 1 |
| `TrophySystem` | `Shelter/TrophySystem.cs` | 1 |
| `PowerLoadSheddingEngine` | `Shelter/PowerLoadSheddingEngine.cs` | 1 |
| `EmergencyMusterReadinessEngine` | `Shelter/EmergencyMusterReadinessEngine.cs` | 1 |
| `KilnFiringEngine` | `Shelter/KilnFiringEngine.cs` | 1 |
| `ChemicalReagentSynthesisEngine` | `Shelter/ChemicalReagentSynthesisEngine.cs` | 1 |
| `MechanicalPowerDrivelineEngine` | `Shelter/MechanicalPowerDrivelineEngine.cs` | 1 |
| `ShelterIdentitySystem` | `Shelter/ShelterIdentitySystem.cs` | 1 |
| `ShelterExpansionSystem` | `Shelter/ShelterExpansionSystem.cs` | 1 |
| `DisasterResponseSystem` | `Shelter/DisasterResponseSystem.cs` | 1 |
| `ShelterMaintenanceSystem` | `Shelter/ShelterMaintenanceSystem.cs` | 1 |
| `HobbySystem` | `Survivors/HobbySystem.cs` | 1 |
| `AntenatalMaternalHealthEngine` | `Survivors/AntenatalMaternalHealthEngine.cs` | 1 |
| `SurvivorAgingProgressionEngine` | `Survivors/SurvivorAgingProgressionEngine.cs` | 1 |
| `SurvivorAutonomySystem` | `Survivors/SurvivorAutonomySystem.cs` | 1 |
| `BackstorySystem` | `Survivors/BackstorySystem.cs` | 1 |
| `AgingSystem` | `Survivors/AgingSystem.cs` | 1 |
| `SurvivorRoutineSystem` | `Survivors/SurvivorRoutineSystem.cs` | 1 |
| `SurvivorRoleSystem` | `Survivors/SurvivorRoleSystem.cs` | 1 |
| `RecruitmentSystem` | `Survivors/RecruitmentSystem.cs` | 1 |
| `WeatherForecastReliabilityEngine` | `World/WeatherForecastReliabilityEngine.cs` | 1 |
| `ModalTravelDispatchEngine` | `World/ModalTravelDispatchEngine.cs` | 1 |
| `WildlifeHarvestQuotaEngine` | `World/WildlifeHarvestQuotaEngine.cs` | 1 |
| `StormForecastReadinessEngine` | `World/StormForecastReadinessEngine.cs` | 1 |
| `NightWatchPatrolReadinessEngine` | `World/NightWatchPatrolReadinessEngine.cs` | 1 |
| `SeasonalCelebrationSystem` | `Events/SeasonalCelebrationSystem.cs` | 1 |
| `MaritimeExplorationSystem` | `Maritime/MaritimeExplorationSystem.cs` | 1 |
| `ChemicalPlumeDispersionEngine` | `Combat/ChemicalPlumeDispersionEngine.cs` | 1 |
| `ContentOrphanCertificationEngine` | `Content/ContentOrphanCertificationEngine.cs` | 1 |
| `AudioAccessibilityCoordinator` | `Audio/AudioAccessibilityCoordinator.cs` | 1 |
| `CassettePlaybackSystem` | `Audio/CassettePlaybackSystem.cs` | 1 |
| `SubterraneanSubsidenceEngine` | `Excavation/SubterraneanSubsidenceEngine.cs` | 1 |
| `TerritoryControlSystem` | `Factions/TerritoryControlSystem.cs` | 1 |
| `SoilReclamationProfileEngine` | `Farming/SoilReclamationProfileEngine.cs` | 1 |
| `OilseedPressingEngine` | `Farming/OilseedPressingEngine.cs` | 1 |
| `CampaignLegacySystem` | `Legacy/CampaignLegacySystem.cs` | 1 |
| `ConfessionSecretSystem` | `Phantoms/ConfessionSecretSystem.cs` | 1 |
| `SessionDurabilityManager` | `Save/SessionDurabilityManager.cs` | 1 |
| `SpiritualRitualCalendarEngine` | `Spiritual/SpiritualRitualCalendarEngine.cs` | 1 |
| `CultureCreationSystem` | `Culture/CultureCreationSystem.cs` | 1 |
| `ShelterFestivalEngine` | `Culture/ShelterFestivalEngine.cs` | 1 |
| `ShelterMuseumSystem` | `Culture/ShelterMuseumSystem.cs` | 1 |
| `PerimeterEarlyWarningEngine` | `Defense/PerimeterEarlyWarningEngine.cs` | 1 |
| `FactionDiplomacySystem` | `Diplomacy/FactionDiplomacySystem.cs` | 1 |
| `ModSupportSystem` | `Mods/ModDataContract.cs` | 1 |
| `SleepAcousticRestEngine` | `Needs/SleepAcousticRestEngine.cs` | 1 |
| `CommitmentSystem` | `Commitments/CommitmentSystem.cs` | 1 |
| `ShelterGovernanceEngine` | `Governance/ShelterGovernanceEngine.cs` | 1 |
| `DifficultySettingsSystem` | `Difficulty/DifficultySettingsSystem.cs` | 1 |
| `InternalCommunicationSystem` | `Communication/InternalCommunicationSystem.cs` | 1 |
| `SecondGenerationMilestoneEngine` | `Generations/SecondGenerationMilestoneEngine.cs` | 1 |
| `WaterQualityProfileEngine` | `Water/WaterQualityProfileEngine.cs` | 1 |
| `WaterSourceSystem` | `Water/WaterSourceSystem.cs` | 1 |
| `CommonTableRationingEngine` | `Nutrition/CommonTableRationingEngine.cs` | 1 |
| `VoiceLineSelectionEngine` | `Voice/VoiceLineSelectionEngine.cs` | 1 |
| `VoiceLineDispatchCoordinator` | `Voice/VoiceLineDispatchCoordinator.cs` | 1 |
| `SurvivorVoiceSystem` | `Voice/SurvivorVoiceSystem.cs` | 1 |
| `PlayableMetricsAggregationEngine` | `Telemetry/PlayableMetricsAggregationEngine.cs` | 1 |
| `InformantNetworkTradecraftEngine` | `Espionage/InformantNetworkTradecraftEngine.cs` | 1 |
| `GarmentLayeringThermalEngine` | `Textiles/GarmentLayeringThermalEngine.cs` | 1 |
| `ApprenticeshipCurriculumEngine` | `Education/ApprenticeshipCurriculumEngine.cs` | 1 |
| `SurvivorEducationSystem` | `Education/SurvivorEducationSystem.cs` | 1 |
| `PrecisionGlassworksOpticsEngine` | `Optics/PrecisionGlassworksOpticsEngine.cs` | 1 |
| `PublicBroadsheetPressEngine` | `Print/PublicBroadsheetPressEngine.cs` | 1 |
| `OutpostSettlementSystem` | `Settlements/OutpostSettlementSystem.cs` | 1 |
| `WeatherCascadeSystem` | `Weather/WeatherCascadeSystem.cs` | 1 |
| `NuclearWinterProgressionSystem` | `Weather/NuclearWinterProgressionSystem.cs` | 1 |
| `CookingSystem` | `Cooking/CookingSystem.cs` | 1 |
| `CommunicationsSystem` | `Communications/CommunicationsSystem.cs` | 1 |
| `PsychologicalProfileSystem` | `Psychology/PsychologicalProfileSystem.cs` | 1 |
| `AccessibilitySettingsSystem` | `Accessibility/AccessibilitySettingsSystem.cs` | 1 |
| `BestiarySystem` | `Bestiary/BestiarySystem.cs` | 1 |
| `EmergencyAlertSystem` | `Emergency/EmergencyAlertSystem.cs` | 1 |
| `FoodTypeSystem` | `Kitchen/FoodTypeSystem.cs` | 1 |
| `VisitorIntegrationSystem` | `Visitors/VisitorIntegrationSystem.cs` | 1 |

## 6. Re-runnable audit commands

```bash
# host reachability closure (this audit's method)
# run: python3 /tmp/reach.py with the algorithm in section 1
# focused spot checks
for n in CommunicationsSystem SeasonalCelebrationSystem TerritoryControlSystem \
         CampaignLegacySystem ModSupportSystem CookingSystem ColonySystem \
         SurvivorEducationSystem FactionCovertOpsCoordinator; do
  echo "$n src=$(grep -rl \"\\b$n\\b\" src/ --include='*.cs' | wc -l)"
done

# fully-dead candidates
for n in DraisineRecoverySystem ArmoredDraisineRecoverySystem PowderMetallurgyEngine \
         LyophilizationEngine NvisC4ISystem; do
  echo "$n host=$(grep -rl \"\\b$n\\b\" src/ --include='*.cs' | wc -l) tests=$(grep -rl \"\\b$n\\b\" Ashfall.Core.Tests/ --include='*.cs' | wc -l)"
done

# registry stats
grep -cE '^\s+new\($' Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs
grep -rhoP 'private void Setup\w+' src/Main*.cs | sort -u | wc -l

# register terminality
grep -E '^\| `DEC-' docs/governance/DECISION_REGISTER.md | grep -v 'SIGNED' | grep -v 'RETIRED' | grep -v 'DECLINED'

# existing gates that should have caught this class of drift
python3 scripts/ci/generate-core-systems-catalog.py --check
python3 scripts/ci/generate-port-contract.py --check
python3 scripts/ci/generate-save-store-matrix.py --check
```

## 7. False-positive controls

- The inventory excludes types referenced only as JSON DTO field names by
  checking the defining file's own token set, not a global name search.
- A type whose *file* is reached via another Core system is correctly counted
  reachable — the culture/industry verticals therefore only list authorities
  whose defining file has no path to `src/`.
- The 5 fully-dead candidates were re-checked repo-wide (`all=1`, i.e. only the
  defining file mentions the type).

## 8. Limitations

- Reflection/manifest-string instantiation would appear as unreachable; no
  such consumer exists for the 99 (checked: no `Type.GetType`/`Activator`
  usage keyed to these names in `src/`), but the kit's allowlist is the
  durable place to record any future exception.
- The audit measures *reachability*, not *quality*: a reachable system can
  still have a stub consumer. The kit's liveness probes address that.

## Addendum 2026-09-21 — Plan 1 appendices D/F and Wave 8

- Re-ran `tools/reachability-audit.py` at HEAD `5be1a30a`: **99** host-unreachable
  authority files, **5** fully dead types — unchanged, so the orphan premise is
  current.
- Appendix D (save ownership): 99-row census; only systems with capture/restore
  methods or `SaveSectionRegistry` knowledge get save rows.
- Appendix E (determinism audit): **5 of 99** orphans reference a banned
  nondeterministic primitive (`CommunicationsSystem`, `SeasonalCelebrationSystem`,
  `SessionDurabilityManager`, `DisasterResponseSystem`, `ShelterExpansionSystem`).
- Appendix F (dependency clusters): 99 authorities, **94 clusters, 89 isolated**;
  non-isolated clusters carry a referenced-first seal order.
- Wave 8 premise corrections recorded: `Assets/_Game/` holds one test-compiled file
  and one uncompiled file (not a broad Unity tree); save registry declares 204
  sections with **5** explicit `SchemaVersions` entries; the data tree has **703**
  JSON files and **one** structural schema (`mod_manifest_schema.json`).

## Addendum 2026-09-21 (round 2) — Plan 1 appendices G–I and Wave 9

- Appendix G (host integration points): candidate `Main.*` partials, registry
  save sections, and CLI flags matched per orphan. Example finding:
  `BestiarySystem` has no candidate partial, no registry section, and no CLI
  flag — a surface decision, not a wiring detail.
- Appendix H (API surface): sizing census per orphan (lines, public methods,
  public properties, constructors, static members, base/interfaces).
- Appendix I (provenance): last-commit/date ledger per orphan file. Re-run
  before promoting any seal package; a changed hash invalidates that package's
  premise check.
- Wave 9 premises verified in source: `DutyRoster/` 13 files (incl.
  `DutyHourLedger`, `DutyRosterSave`, `DutyRosterHeadlessDemo`);
  `Campaign/SliceScenario.cs`; `Save/SaveSlotTypes.cs` (`SaveProfileId`,
  `SaveSlotId`); skill stack (`SkillProgressionSystem`, `SkillProgressionState`,
  `SkillAtrophySystem`, `SkillDef`, `SkillCatalogLoader`);
  `World/DamagedMapSystem.cs`, `DamagedMapCatalog.cs`, `FieldGuideCatalog.cs`;
  `Crafting/CraftingSystem.cs`; `docs/CURRENT_AUTHORITY.md`.

## Addendum 2026-09-21 (round 3) — Plan 1 appendices J–L and Wave 10

- Appendix J (test coverage): complete referencing-test inventory per orphan
  with case counts.
- Appendix K (API signatures): public member signatures per orphan; 41 types
  exceed 30 members and carry an omitted count.
- Appendix L (risk scorecard): formula stated in the appendix; top scores
  `CommunicationsSystem` 7, `SeasonalCelebrationSystem` 6,
  `DisasterResponseSystem` 5, `ShelterExpansionSystem` 5.
- Wave 10 premises verified in source: `Verdict/` subsystem (`EvidenceLedger`,
  `VerdictEvidenceChain`, `VerdictAccusationSystem`, `MachineLogSystem`,
  `ReckoningSystem`, `VerdictSave`); `Muster/` subsystem (camp scene catalog,
  coalition camp, cold count, currents, epilogue matrix, faction action board);
  `DutyRoster/MoraleMarkSystem.cs`; `Data/memorial_rites.json` and
  `memorials_expansion_05.json`; `Narrative/CryoPreservationCatalog.cs`,
  `SeedBankPreservationCatalog.cs`, `DwellerHeirloomCatalog.cs`;
  `Crafting/RecipeCatalogLoader.cs`; `Print/PublicBroadsheetPressEngine.cs`.

## Addendum 2026-09-21 (round 4) — Plan 1 appendices M–O and Wave 11

- Appendix M (catalog binding): 54 of 99 orphans name-match at least one catalog
  in `Data/`; existence, record counts (where array-shaped), and loader
  references verified per row.
- Appendix N (surface routes): 192 declared route ids in
  `src/Main.PlayerSurfaces.cs` matched per orphan domain.
- Appendix O (verification commands): 109 test regions and 218 `--*-selftest`
  flags indexed into a concrete command per orphan.
- Wave 11 selection method: directory audit of all 120 `Assets/Ashfall.Core/*/`
  directories against the 126 existing plans; the 15 lowest-coverage subsystems
  became plans 131–145 (`PlayerCommand`, `NarrativeConsequence`, `UtilityAI`,
  `Thirdonary`, `SkyDefense`, `MoralChoice`, `Endgame`, `Feedback`,
  `StandingRecord`, `AdvancedMachinery`, `Institutions`, `Cognition`, `NpcArcs`,
  `Sanatorium`, `StartingLevel`).

## Addendum 2026-09-21 (round 5) — Plan 1 appendices P–R and Wave 12

- Appendix P (inbound references): the closure was re-run inside the generator;
  result **0 reachability contradictions**, 2 orphans referenced only by
  dormant non-authority files, **97 referenced only by tests**, 0 true islands.
  This proves orphans are game-detached rather than partially wired.
- Appendix Q (save-key collisions): proposed snake_case keys checked against all
  registry keys and aliases: **0 collisions**.
- Appendix R (catalog shapes): top-level object keys / array element keys per
  matched catalog.
- Wave 12 selection: second-pass directory audit (after Wave 11 removed the
  fifteen weakest). Selected: `YearOfAsh` (15 files), `Orchestration`,
  `IO`, `Phantoms`, `Propaganda`, `Treaties`, `Archaeology`, `Waystation`,
  `Vehicles`, `Encounters`, `Journeys`, `Ports`, `Rail`, `Communication`,
  `Generations`.

## Addendum 2026-09-21 (round 6) — Plan 1 appendices S–U and Wave 13

- Type-level audit: 454 Core authority types; **135 reachable authorities were
  never addressed in any plan body** (excluding Plan 1's appendices). Wave 13
  takes the fifteen largest distinct ones.
- Appendix S (test-region inverse index): 50 of 109 test regions reference at
  least one orphan — natural claim bundles with ready commands.
- Appendix T (worked exemplars): four complete seal specifications assembled
  from Appendices A–U.
- Appendix U (data references): only 4 orphans hardcode JSON literals; a real
  finding — `ModSupportSystem` references `factions.json` and `quests.json`,
  which do not exist anywhere under `Data/` (recursive check).
- Premise corrections: Plan 116 (two noise systems — `ShelterNoiseSystem` was
  missing from its premise); Plan 119 (now cites `EquipmentConditionSystem.cs`).

## Addendum 2026-09-21 (round 7) — Plan 1 appendices V–X and Wave 14

- Type-level audit (round 2): the 135 unaddressed reachable authorities were
  re-derived; Wave 14 takes the next fifteen largest (490–1,200 lines).
- Appendix V (master worklist): combined risk/state/key/catalog/route/command
  table, one line per claim.
- Appendix W (data ids): the recursive data tree yields **4,750 ids**; 14
  orphans carry id literals, 15 rows unresolved for classification.
- Appendix X (static hazards): corrected generator (methods excluded) — only
  **1 of 99** orphans carries a flagged static.
- Generator corrections carried in this round: recursive data scanning for
  V/W (the first pass missed `Data/` subdirectories) and field-only static
  detection for X (the first pass flagged static methods returning collections).

## Addendum 2026-09-21 (round 8) — Plan 1 appendices Y–AA and Wave 15

- Type-level audit (round 3): the unaddressed pool is down to **102**
  authorities; Wave 15 takes the next fifteen largest, skipping five owned by
  existing/available programmes (distress seal, Plan 207, Plan 216, Plan 210,
  Plan 202).
- Appendix Y (batch plan): 10 risk-balanced batches (risk sums 16–19) with
  dominant test regions; supersedes an initial 51-batch heuristic that grouped
  too strictly by region.
- Appendix Z (shared shapes): **56 of 99 orphans implement
  `CaptureState()`/`RestoreState()`** — Appendix D's 58 (which included registry
  knowledge) refined to the strict pair; 34 `LoadCatalog`, 11 `TickDay`.
  Replaces an initial pruning census that found 0 candidates due to tree-wide
  method-name collisions.
- Appendix AA (time coupling): 37 orphans expose day-step methods, 2 hourly.
- Folded `Shelter/AeroponicsSystem.cs` (528 lines) into Plan 163's scope.

## Addendum 2026-09-21 (round 9) — Plan 1 appendices AB–AD and Wave 16

- Type-level audit (round 4): unaddressed pool down to **85** authorities; Wave
  16 takes the next fifteen largest distinct ones, skipping six owned elsewhere.
- Appendix AB (batch links): each of the ten batches mapped to the expansion
  plans its members unblock.
- Appendix AC (save signatures): of the 56 capture/restore orphans, **only one**
  names its DTO `*Save` (CupolaFoundryEngine) — the rest use divergent names;
  **0 signature types are undefined in Core**. Replaces a wrong `*Save`-naming
  premise.
- Appendix AD (batch verification): per-batch loop with the audit as acceptance
  witness (99 → 99−N); step list repaired after an edit collision.
- Wave 16 domains: cryo vault, defense command, craft archives, radio station
  ops, psyops, discovery consequences, dynamic questlines, surgical ward, social
  dynamics, narcotics, procedural narrative, solar concentration, weather
  intelligence/hardening, expedition vehicles, precision optics.

## Addendum 2026-09-21 (round 10) — Plan 1 appendices AE–AG and Wave 17

- Type-level audit (round 5): unaddressed pool down to **67** authorities; Wave
  17 takes twenty (with sibling pairs/trios bundled), skipping nine owned
  elsewhere (distress seal, Plans 202/207/210/216, Muster/Endgame, Plans 81/64/47).
- Appendix AE (surface decisions): **23 orphans** have neither a host partial
  nor a route candidate — an explicit EP-01 decision each.
- Appendix AF (seal order): batches scored by programme-unblock value per risk;
  top priority Batch 06 (Economy, score 128).
- Appendix AG (loader gaps): **22 catalog rows with zero loader references** —
  classify dynamically-pathed vs genuinely unloaded.
- Layout fix carried: AG's summary line moved above its table (same pattern
  corrected in AC last round).

## Addendum 2026-09-21 (round 11) — Plan 1 appendices AH–AJ and Wave 18

- Type-level audit (round 6): unaddressed pool down to **39** authorities; Wave
  18 takes twenty (with sibling bundles), skipping nineteen owned elsewhere
  (distress seal, Plans 202/207/210/216, Plans 47/64/81, Plan 130 Muster camp,
  Plans 79/212/137/164/191 domains).
- Appendix AH (lifecycle/files): 55 registry lifecycle groups; proposed group
  and `*_save.json` name per stateful orphan; **0 file collisions**.
- Appendix AI (method names): 228 host `Setup*` methods; proposed
  `Setup*`/`Save*` names for all 99 orphans; **0 collisions**.
- Appendix AJ (maintenance map): the 33-appendices' derivation sources and
  invalidation triggers — regeneration precedes claim promotion.
- Layout fix carried: AH/AI summary lines moved above their tables (the
  recurring insertion-order defect).

## Addendum 2026-09-21 (round 12) — Plan 1 appendices AK–AM and Wave 19

- Level shift: after 18 waves the authority-type pool is 39 (mostly owned
  elsewhere), so the audit moved to **file level** — **538 Core files are
  referenced by no plan body**. Wave 19 covers the twenty largest families as
  consumer/dead-file audits.
- Appendix AK (blob inventory): the unreachable set is **246 files** — the 99
  suffix-named authorities plus **147 blob files** (loaders, DTOs, catalogs,
  partials), enumerated by directory.
- Appendix AL (compile surface): the game project's `Assets/Ashfall.Core/**`
  glob compiles all 246; there are no build-excluded orphans — unreachable code
  ships, so retirement is a clarity/size change.
- Appendix AM (generators): the **14 appendix generators are now versioned**
  under `tools/generators/`, making every appendix reproducible; correction
  lineage recorded.
- Wave 19 families: Narrative 87, Core root 59, Medical 29, Survivors 29,
  Shelter 26, Radio 24 (distress excluded), World 24, Factions 23, Expeditions
  20, Economy 19, Inventory 17, Campaign 14, Combat 12, Content 12, Muster 11,
  MoralChoice 11, UI 10, Foundry 9, Performance 9, and a Craft/Journal/Disease
  trio (18).

## Addendum 2026-09-21 (round 13) — Scaffolding appendices for ten session plans

- Ten plans gained generated **implementation scaffolds** (Appendix A), built
  under the authority of `PLAN-INTEGRATION-KIT-02` (integration patterns) and
  the `PLAN-ORPHAN-SEAL-01` appendix convention: Combat Depth 62, Mental Health
  64, Collectibles 67, Labour 68, Cartography 70, Threading 72, Dev Tooling 75,
  Content Pipeline 77, Bionics 78, Autonomous Machines 79.
- Each scaffold carries: source inventory with line counts, data bindings with
  record counts, host attachment candidates and collision-free proposed method
  names, a package-derived xUnit fixture skeleton, commands, and scaffolding
  rules (no production file created; regenerate rather than edit).
- Plan 72's scaffold uses a content-based variant: **171 files carry threading
  references** (async/await/Task/Thread/lock/Interlocked) — the usage census
  replaces filename matching.
- The generator `gen_scaffolds.py` is versioned under `tools/generators/` and
  listed in Appendix AM.

## Addendum 2026-09-21 (round 14) — Paired scaffolds for ten more plans

- Plans 81, 82, 83, 84, 86, 87, 89, 90, 91, 92 each gained `APPENDIX-A_SCAFFOLD.md`
  paired with a **specific session-created plan as scaffolding authority**:
  Mutation←Pandemic 47, Nomads←Faction Branch 171, Deep Strata←Aquifer 164,
  Ruins←Archaeology 152, CLI←Host Composition 71, Save Migration←Save Governance
  12, Determinism←Determinism Replay 13, Data Schema←Data Authority 14, Event
  Archive←Telemetry Privacy 58, Mod Boundary←Content Pipeline 77.
- Each appendix lists the authority's own packages as advisory patterns, a
  host-inclusive source inventory, data bindings, host attachment with
  collision-free names, a package-derived test skeleton plus an
  authority-conformance fixture, and scaffolding rules.
- Host-inclusive regeneration fixed thin inventories (Plan 86: 1→30 files,
  Plan 89: 11→13, Plan 91: 16→20).
- Generator `gen_scaffolds2.py` versioned under `tools/generators/`; Appendix AM
  updated.

## Addendum 2026-09-21 (round 15) — Paired scaffolds, batch 3

- Plans 88, 93, 94, 95, 96, 97, 98, 99, 100, 131 each gained a paired
  `APPENDIX-A_SCAFFOLD.md`: Text Packs←Localization 52, Inventory←Craft Quality
  112, Deprecated Tree←Orphan Seal 01, Spatial←Waystation 153, Economy←Economy
  Data Family 270, Audio←Audio Condition 255, Save Fuzz←Save Migration 87,
  Hotfix←Release Ops 20, Closeout←Agent Workflow 59, Player Command←Silent
  Failure 35.
- Bespoke data sections this batch: l10n inventory (535 records, 472 hardcoded
  literals, top panels), deprecation surface (`Assets/_Game/` 4 files, 3
  non-comment Unity refs, test csproj `_Game` include yes, `src/Bridge/`
  absent), release tooling (4 scripts with line counts), and programme
  self-counts (19 directories, 276 plans, 114 appendices, 16 generators).
- Generator `gen_scaffolds3.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 16) — Paired scaffolds, batch 4

- Wave 11 subsystems gained paired scaffolds: Narrative Consequence←Narrative
  Family 261, Utility AI←Determinism Cross-Host 89, Thirdonary←Justice 37,
  Sky Defense←Base Defense 61, Moral Choice←MoralChoice Loader Family 276,
  Endgame←Campaign Epilogue 259, Institutions←Survivor Roster 244, Memory
  Decay←Backstory Reveal 126, NPC Arcs←Narrative Arc Events 176, Starting
  Level←Balance & Difficulty 73.
- Each appendix: authority packages as advisory patterns, Core+host source
  inventory, data bindings, host attachment with collision-free names, a
  package-derived xUnit skeleton plus a conformance fixture, commands, rules.
- Generator `gen_scaffolds4.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 17) — Paired scaffolds, batch 5

- Plans 138, 139, 140, 144, 146, 147, 148, 149, 150, 151 gained paired
  scaffolds: Feedback←Host Event Archive 91, Standing Record←Discovery State
  108, Machinery Contracts←Port Contract 157, Sanatorium←Mental Health 64,
  Year of Ash←Atmosphere 28, Bootstrap Gate←Programme Closeout 100, Catalog
  Boot←Data Schema 90, Heirloom/Phantom←Secrets & Confession 127,
  Propaganda←Rumor Propagation 120, Treaties←Warlords Diplomacy 29.
- Two contracts-first pairings are deliberate: 140 adopts 157 (the sibling
  contracts-first plan) and 147 adopts 100 (the governance index it feeds).
- Year of Ash's inventory is the largest this batch: **78 files**.
- Generator `gen_scaffolds5.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 18) — Paired scaffolds, batch 6

- Plans 152–161 gained paired scaffolds: Archaeology←Ancient Ruins 84 (the
  frontier and its dig model, mutually paired), Waystation←Transport 30,
  Vehicle Customization←Expedition Vehicle 219, Knock Whitelist←Year of Ash
  146, Journey Context←Travel Encounter 177, Port Contract←Machinery Contracts
  140 (the two contracts-first plans now reference each other),
  Rail Maintenance←Maintenance Decay 119, Internal Communication←Print Media
  128, Generational Milestone←Succession Legacy 252, Espionage←Counter-Intel 41.
- Largest inventory this batch: Espionage System at **40 files** (Factions +
  Espionage dirs).
- Generator `gen_scaffolds6.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 19) — Paired scaffolds, batch 7

- Plans 162–171 (Wave 13) gained paired scaffolds: Morale Contagion←Morale &
  Unrest 129, Aquaponics←Water & Agriculture 46, Aquifer←Fluid Logistics 179,
  Perimeter←Base Defense 61, Embargo←Economy Ledger 96, Pharmaceutical←
  Microfluidic Diagnostics 182, Weather Sonde←Weather Intelligence 218,
  Cultural Archive←Lore Archive 238, Narrative Continuity←Narrative Family 261,
  Faction Branch←Faction Branch Status 228 (the status sibling).
- Largest inventories: Narrative Continuity 116 files (Narrative dir + token
  breadth), Aquifer 90, Aquaponics 85, Morale Contagion 75.
- Generator `gen_scaffolds7.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 20) — Paired scaffolds, batch 8

- Plans 172–175 and 178/180/181/183/186/189 gained paired scaffolds:
  Metrology←Craft Quality 112, Leadership←Institutions 141, Rationing←Food &
  Cuisine 39, Workshop←Industry Automation 45, Biofermentation←Preservation
  118, Pneumatic Dispatch←Fluid Logistics 179 (sibling), Kinetic Storage←
  Energy 48, Chemical Recon←Signals 49, Psychological Arc←Mental Health 64,
  Radiation Background←Dosimeter Calibration 204 (sibling).
- Sibling pairings are deliberate: 180 adopts 179 (the other network plan) and
  189 adopts 204 (the instrument-accuracy plan).
- Inventories are large this batch (70–89 files) because Wave 14 systems sit in
  shared directories with broad token matches; the extra 3b sections remain
  absent (no bespoke data for this batch).
- Generator `gen_scaffolds8.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 21) — Paired scaffolds, batch 9

- Plans 191–205 (selected ten) gained paired scaffolds: Geothermal←Geothermal
  Aquifer 260 (sibling), Document Discovery←Codex Surface 110, Seismic←Cascade
  Coordinator 249, Tunnel Network←Deep Strata 83, Relationship Decay←Family 43,
  Health History←Acute Trauma 124, Chlor-Alkali←Plastic Pyrolysis 187
  (chemical sibling), Echo←Narrative Arc Events 176, Caregiving←Survivor Roster
  244, Black Projects←Cultural Archive 169.
- Generator `gen_scaffolds9.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 22) — Total content expansion for ten family plans

- Ten Wave 19 family plans (273, 278, 270, 275, 279, 265, 267, 266, 277, 276)
  gained a five-section content expansion appended to the plan body itself:
  expanded census (every family file with class, lines, banned refs, empty
  catches, capture/restore), data & state surface, verification baselines,
  rollout sequence, and a per-class acceptance matrix.
- Scope discipline: the family is the original Wave 19 definition — files under
  the plan's directory whose basename no plan body references — so counts match
  the plans' own premises (e.g. Shelter 21, UI 12, Performance 10).
- Baseline findings recorded per plan: e.g. Economy 48 files / 14,193 lines /
  18 save surfaces / 167 test references; Radio 37 files with 13 save surfaces;
  Combat 25 files with 8 save surfaces and 61 test references.
- Generator `gen_expand_plans.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 23) — Total content expansion, batch 2

- Remaining ten Wave 19 family plans (261, 262, 263, 264, 268, 269, 271, 272,
  274, 280) expanded with drift-aware censuses: every file in the directory
  scope annotated `still unmentioned` or `since-mentioned` relative to all plan
  bodies, plus class/lines/banned/empty-catch/capture counts, data shapes,
  verification baselines, rollout, and acceptance matrix.
- Drift finding: later waves have referenced most family files — only two plans
  retain unmentioned files (Narrative 38 of 115; Core Root 5 of 135); eight
  families show 0 still-unmentioned, so their plans now reduce to owner and
  re-verification work rather than discovery.
- Generators: `gen_expand_plans_driftaware.py` versioned (runnable CLI variant);
  Appendix AM updated.

## Addendum 2026-09-21 (round 24) — Total content expansion, batch 3

- Ten Wave 17–18 content plans expanded with domain-aware censuses: 255, 248,
  246, 247, 250, 239, 257, 258, 222, 229. Each gains premise-marked file
  tables, data/state surfaces, verification baselines, rollout, acceptance.
- Generator `gen_expand_plans_batch3.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 25) — Total content expansion, batch 4

- Ten more plans expanded (241, 253, 243, 251, 245, 235, 254, 231, 230, 217)
  with premise-marked domain censuses, data/state surfaces, verification
  baselines, rollout sequences, and acceptance matrices.
- Generator `gen_expand_plans_batch4.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 26) — Total content expansion, batch 5

- Ten more plans expanded (256, 252, 232, 259, 226, 242, 227, 236, 179, 225).
- Correction carried: batch 4 wrote Plan 235's expansion to a Wave 18 path (the
  plan is Wave 17), creating a stray file. The expansion was moved into the real
  plan and the stray removed; batch 5 verified every path before writing.
- Generator `gen_expand_plans_batch5.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 27) — Total content expansion, batch 6

- Ten more plans expanded (206, 249, 214, 244, 240, 190, 228, 184, 187, 223).
- All paths pre-verified against the plan tree (the batch-4 stray-file lesson).
- Generator `gen_expand_plans_batch6.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 28) — Total content expansion, batch 7

- Ten more plans expanded (233, 221, 224, 219, 215, 207, 211, 220, 237, 200).
- Generator `gen_expand_plans_batch7.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 29) — Total content expansion, batch 8

- Ten more plans expanded: nine source plans (213, 209, 234, 197, 188, 182,
  202, 212, 216) plus Plan 56 with a bespoke build-hygiene census (project
  files, compile includes, TFM, CI scripts/gates, legacy `_Game` include).
- Generator `gen_expand_plans_batch8.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 30) — Total content expansion, batch 9

- Ten more plans expanded (260, 204, 177, 198, 185, 238, 210, 129, 127, 157).
- Generator `gen_expand_plans_batch9.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 31) — Total content expansion, batch 10

- Ten more plans expanded (218, 58, 128, 176, 54, 196, 55, 201, 194, 126).
- Generator `gen_expand_plans_batch10.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 32) — Total content expansion, batch 11

- Ten more plans expanded (208, 191, 180, 114, 117, 203, 193, 181, 143, 122).
- Generator `gen_expand_plans_batch11.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 33) — Total content expansion, batch 12

- Ten more plans expanded (120, 166, 53, 123, 152, 112, 199, 205, 171, 102).
- Generator `gen_expand_plans_batch12.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 34) — Total content expansion, batch 13

- Ten more plans expanded: eight source plans (111, 125, 175, 107, 186, 183,
  151, 153) plus two bespoke censuses — Plan 115 (docs tree: root/docs/plans
  counts, INDEX, CURRENT_AUTHORITY) and Plan 72 (content-based threading census
  across Core+host).
- Generator `gen_expand_plans_batch13.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 35) — Total content expansion, batch 14

- Ten more plans expanded (189, 158, 156, 106, 103, 192, 160, 174, 164, 77).
- Plan 106's name-token census was empty (input bindings are code/config, not
  filenames), so it received a bespoke input-surface census: the declared
  `project.godot` action set plus the Settings codec files.
- Generator `gen_expand_plans_batch14.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 36) — Total content expansion, batch 15

- Ten more plans expanded: eight source plans (121, 178, 167, 150, 172, 154,
  155, 169) plus two bespoke censuses — Plan 71 (Main.* partials, setup/save
  counts, duplicates) and Plan 52 (l10n inventory artifact: records, literals,
  panels).
- Generator `gen_expand_plans_batch15.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 37) — Total content expansion, batch 16

- Ten more plans expanded: nine source plans (108, 165, 113, 142, 133, 195,
  134, 118, 104) plus a bespoke asset/license census for Plan 60.
- Generator `gen_expand_plans_batch16.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 38) — Total content expansion, batch 17

- Ten more plans expanded (144, 148, 105, 168, 57, 51, 162, 138, 145, 141).
- Generator `gen_expand_plans_batch17.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 39) — Total content expansion, batch 18

- Ten more plans expanded (132, 76, 75, 110, 109, 149, 173, 170, 139, 163).
- Generator `gen_expand_plans_batch18.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 40) — Total content expansion, batch 19

- Ten more plans expanded: eight source plans (161, 91, 124, 131, 135, 101,
  147, 130) plus bespoke censuses for Plan 74 (test tree, case count, gates)
  and Plan 100 (programme self-counts including expansion coverage).
- Generator `gen_expand_plans_batch19.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 41) — Total content expansion, batch 20

- Ten more plans expanded: seven source plans (119, 116, 159, 66, 136, 64, 63)
  plus bespoke censuses for Plan 59 (agent rules/skills/governance), Plan 73
  (preset × scalar matrix + balance artifacts), Plan 99 (release scripts).
- Generator `gen_expand_plans_batch20.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 42) — Total content expansion, batch 21

- Ten more plans expanded (137, 67, 69, 92, 89, 140, 90, 146, 93, 65).
- Generator `gen_expand_plans_batch21.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 43) — Total content expansion, batch 22

- Ten more plans expanded: nine source plans (82, 88, 98, 68, 97, 70, 79, 96,
  95) plus a bespoke CLI-surface census for Plan 86 (descriptors, flags,
  categories, host partials, CLI tests).
- Generator `gen_expand_plans_batch22.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 44) — Total content expansion, batch 23

- Ten more plans expanded: nine source plans (81, 85, 78, 80, 61, 84, 87, 83,
  62) plus a bespoke deprecation census for Plan 94 (`_Game` files, non-comment
  Unity tokens, csproj includes, Bridge directory).
- Generator `gen_expand_plans_batch23.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 45) — Total content expansion, batch 24

- Ten more plans expanded: seven source plans (33, 42, 41, 32, 50, 34, 27)
  plus bespoke censuses for Plan 23 (selftest flags/regions/gates), Plan 22
  (catalog consumption baseline), Plan 24 (debt and claims ledger rows).
- Generator `gen_expand_plans_batch24.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 46) — Total content expansion, batch 25

- Ten more plans expanded: eight source plans (43, 36, 37, 48, 44, 47, 49, 46)
  plus bespoke censuses for Plan 35 (catch shapes across Core) and Plan 31
  (IO boundary call sites).
- Correction: batch 24 wrote Plan 27's expansion to a Wave 5 path (the plan is
  Wave 3); the expansion was moved into the real plan and the stray removed.
- Generator `gen_expand_plans_batch25.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 47) — Total content expansion, batch 26

- Ten more plans expanded: eight source plans (45, 38, 39, 29, 30, 28, 26, 40)
  plus bespoke censuses for Plan 21 (event names across Core+host) and Plan 25
  (declared actions and input-handling call sites).
- Generator `gen_expand_plans_batch26.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 48) — Total content expansion, batch 27

- Ten more plans expanded: three source plans (16, 18, 15) plus bespoke
  censuses for Plan 17 (test tree/quarantine), Plan 19 (assets/imports), Plan 11
  (authority types), Plan 13 (RNG streams + banned primitives), Plan 14 (data
  tree/classification), Plan 12 (registry sections/ladders), Plan 20 (gates,
  scripts, workflows).
- Generator `gen_expand_plans_batch27.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 49) — Final expansion: plans 01–06 + tier-2 sections

- Plans 01–06 (the last six without in-body expansions) received census sections:
  01 (kit self-census), 02 (kit surface), 03 (decision rows), 04/05 (vertical
  domain censuses), 06 (launch surface: actions, routes, partials).
- Four flagship plans (30 Transport, 146 Year of Ash, 62 Combat, 48 Energy)
  received **tier-2 intra-domain reference graphs**: edge counts, in/out-degree
  tables, highest-coupling files, and an ordering implication.
- Generator `gen_expand_plans_batch28.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 50) — Tier-2 graphs, batch 2

- Ten more plans received tier-2 intra-domain reference graphs (96, 95, 101,
  121, 130, 137, 29, 47, 45, 49): edge counts, in/out-degree tables,
  highest-coupling files, ordering implications. Completes tier-2 for the
  fourteen most connected domains.

## Addendum 2026-09-21 (round 51) — Cross-plan coupling, batch 1

- Ten plans received §12 cross-plan coupling (28, 42, 44, 83, 87, 93, 61, 78,
  80, 27): incoming plan edges (which plans reference the domain files) and a
  heuristic package→candidate-file touch map.
- Generator `gen_cross_plan_coupling.py` versioned; Appendix AM updated.

## Addendum 2026-09-21 (round 51b) — Cross-plan fixes

- Package extraction corrected: some plans use `### XX-28A — Title` headings
  rather than `- **XX-28A**` bullets; four sections (28, 42, 44, 27) were
  regenerated with the heading pattern.
- Plan 27's regeneration recreated a stray Wave 5 file (the plan is Wave 3);
  the §12 section was moved into the real plan and the stray removed. The scan
  for headless files found no other strays.
- Versioned generator patched to support both package formats.

## Addendum 2026-09-21 (round 52) — Cross-plan coupling, batch 2

- Ten more plans received §12 cross-plan coupling (96, 95, 101, 121, 130, 137,
  29, 47, 45, 49) — the same domains that carry tier-2 graphs, completing the
  two-layer view (internal coupling + external coordination).

## Addendum 2026-09-21 (round 53) — Cross-plan coupling, batch 3

- Ten more plans received §12 cross-plan coupling (30, 146, 62, 48, 37, 50, 63,
  81, 82, 97). The four tier-2 flagships (30, 146, 62, 48) now carry the full
  two-layer view.

## Addendum 2026-09-21 (round 54) — Cross-plan coupling, batch 4

- Ten more plans received §12 cross-plan coupling (26, 38, 39, 40, 43, 64, 66,
  67, 68, 69).

## Addendum 2026-09-21 (round 55) — Cross-plan coupling, batch 5

- Ten more plans received §12 cross-plan coupling (70, 76, 79, 84, 85, 88, 90,
  92, 98, 107).

## Addendum 2026-09-21 (round 56) — Cross-plan coupling, batch 6

- Ten more plans received §12 cross-plan coupling (102, 108, 120, 122, 124,
  125, 131, 132, 133, 134).

## Addendum 2026-09-21 (round 57) — Cross-plan coupling, batch 7

- Ten more plans received §12 cross-plan coupling (135, 136, 138, 139, 141,
  142, 143, 144, 145, 148).

## Addendum 2026-09-21 (round 58) — Cross-plan coupling, batch 8

- Ten more plans received §12 cross-plan coupling (149, 150, 151, 152, 153,
  155, 156, 157, 158, 159).

## Addendum 2026-09-21 (round 59) — Cross-plan coupling, batch 9

- Ten more plans received §12 cross-plan coupling (161, 162, 163, 164, 165,
  166, 167, 168, 169, 170).

## Addendum 2026-09-21 (round 60) — Cross-plan coupling, batch 10

- Ten more plans received §12 cross-plan coupling (171, 172, 173, 174, 175,
  176, 177, 178, 179, 180).

## Addendum 2026-09-21 (round 61) — Cross-plan coupling, batch 11

- Ten more plans received §12 cross-plan coupling (181, 182, 183, 184, 185,
  186, 187, 188, 189, 190).

## Addendum 2026-09-21 (round 62) — Cross-plan coupling, batch 12

- Ten more plans received §12 cross-plan coupling (191, 192, 193, 194, 195,
  196, 197, 198, 199, 200).

## Addendum 2026-09-21 (round 63) — Cross-plan coupling, batch 13

- Ten more plans received §12 cross-plan coupling (201, 202, 203, 204, 205,
  206, 207, 208, 209, 210).

## Addendum 2026-09-21 (round 64) — Cross-plan coupling, batch 14

- Ten more plans received §12 cross-plan coupling (211, 212, 213, 214, 215,
  216, 217, 218, 219, 220).

## Addendum 2026-09-21 (round 65) — Cross-plan coupling, batch 15

- Ten more plans received §12 cross-plan coupling (221, 222, 223, 224, 225,
  226, 227, 228, 229, 230).

## Addendum 2026-09-21 (round 66) — Cross-plan coupling, batch 16

- Ten more plans received §12 cross-plan coupling (241, 242, 243, 244, 245,
  246, 247, 248, 249, 250).

## Addendum 2026-09-21 (round 67) — Cross-plan coupling, batch 17

- Ten more plans received §12 cross-plan coupling (251, 252, 253, 254, 255,
  256, 257, 258, 259, 260).

## Addendum 2026-09-21 (round 68) — Cross-plan coupling, batch 18

- Ten Wave 19 family-survey plans received §12 (261–270), using each plan's
  own `.cs` enumeration as its domain set.

## Addendum 2026-09-21 (round 69) — Cross-plan coupling, batch 19

- The last ten Wave 19 family plans received §12 (271–280). **Wave 19 §12
  coverage is now complete (20/20).**

## Addendum 2026-09-21 (round 70) — Cross-plan coupling, batch 20

- Wave 2 systemic plans received §12 cross-plan coupling where their bodies
  enumerate `.cs` files.

## Addendum 2026-09-21 (round 71) — Cross-plan coupling, batch 20 (artifact proxy)

- Six Wave 2 governance plans (11, 12, 13, 14, 17, 19, 20) received §12 using
  their own backticked artifact lists as the domain proxy, since they govern
  config/data/tests rather than a `.cs` domain.

## Addendum 2026-09-21 (round 72) — Cross-plan coupling, batch 21

- Wave 3 tails (21–25) and Wave 4 plans (31–35) received §12 using whichever
  domain proxy the plan body actually supports.

## Addendum 2026-09-21 (round 73) — Cross-plan coupling, batch 22

- Wave 6 plans (51–60) received §12 cross-plan coupling.

## Addendum 2026-09-21 (round 74) — Cross-plan coupling, batch 23

- Wave 7 tails (71–75, 77) and Wave 8–9 leftovers (86, 89, 91, 94) received §12.

## Addendum 2026-09-21 (round 74b) — Plan 74 §12 correction

- Plan 74 (Automated QA Campaigns) needed a bespoke domain proxy: it governs QA
  campaign tiers and artifacts, not `.cs` files. A generic scan returned 2
  artifacts; the corrected section names the 11 tiers plus 3 artifacts.

## Addendum 2026-09-21 (round 75) — Cross-plan coupling, batch 24

- Wave 8–9 leftovers (99, 100, 103–106, 109–112) received §12.

## Addendum 2026-09-21 (round 76) — Cross-plan coupling, batch 25

- Wave 9–10 leftovers (113–119, 126–128) received §12.

## Addendum 2026-09-21 (round 76b) — Plan 115 §12 correction

- Plan 115 (Doc Atlas Currency) governs the documentation tree, so its domain
  proxy is the document set it rules on plus the index generator; a generic
  scan returned 2 artifacts.

## Addendum 2026-09-21 (round 77) — Cross-plan coupling, batch 26

- Wave 1 governance plans (1–6) and plans 36, 41, 46, 65 received §12.

## Addendum 2026-09-21 (round 78) — Cross-plan coupling, batch 27

- Leftover plans 123, 129, 140, 147, 154, 160 and Wave 17 plans 231–234
  received §12.

## Addendum 2026-09-21 (round 79) — Mega wave: §12 completion + §11 batch

- **§12 complete:** plans 235–240 received cross-plan coupling; all 276 plans now
  carry §12.
- **§11 wave:** up to 60 plans received tier-2 intra-domain reference graphs,
  computed from real Core source (edges = sibling type-name references).

## Addendum 2026-09-21 (round 80) — Mega wave C/D

- **§13 authority binding map:** every plan received three-layer binding counts
  (host `src/`, tests, data JSON) computed from real identifier indexes.
- **§11 small-domain graphs:** plans with 3–5 file domains received tier-2
  graphs, extending §11 beyond the ≥6-file threshold.

## Addendum 2026-09-21 (round 81) — Mega wave E/F/G

- **§14 save-section ownership:** every plan received save-registry proximity
  matches (section keys adjacent to its symbols) computed from
  `Save/SaveSectionRegistry.cs`.
- **§15 CLI and selftest coverage:** every plan received matched CLI
  descriptors/flags from `HostCliRegistry.cs`.
- **§16 event-route reachability:** every plan received the event declarations
  that share a file with its symbols, from a full Core event census.

## Addendum 2026-09-21 (round 82) — §14/§15 correction

- The first §14/§15 pass matched class names against the save and CLI registries,
  which store snake_case keys and kebab-case flags — producing false zeros in
  258/274 plans. Both sections were regenerated for all 276 plans using the
  registries' real join: domain keywords against key tokens.

## Addendum 2026-09-21 (round 83) — Mega wave H/I/J

- **§17 data-catalog binding:** every plan mapped to catalog JSON by filename
  token overlap (703 catalogs indexed).
- **§18 test-region mapping:** every plan mapped to test regions with real file
  and case counts (108+ regions indexed).
- **§19 host-surface route map:** every plan mapped to host files and
  panel/HUD counts in `src/`.

## Addendum 2026-09-21 (round 83b) — §18 correction

- The first §18 pass treated every path under `Ashfall.Core.Tests/` as a region,
  inflating the count to 668 by counting root-level test files individually.
  §18 was regenerated for all 276 plans using true top-level region directories
  (plus a note on the root-level test files).

## Addendum 2026-09-21 (round 84) — Mega wave K/L/M/N

- **§20 save schema ladder:** every plan mapped to save sections and flagged
  against the five versioned ladders.
- **§21 determinism / RNG stream binding:** every plan mapped to the 64 seeded
  streams in `Random/CampaignRngStream.cs`.
- **§22 content-consumption classification:** every plan mapped to the 538
  catalogs and their gameplay-consumed/UNRESOLVED classification.
- **§23 flag wiring:** every plan mapped to persistent flags.

## Addendum 2026-09-21 (round 84b) — §20–§23 completeness

- The first §20–§23 pass skipped 69 plans whose title keywords were all short or
  generic (e.g. `UI-SURFACE`, `UNBLOCK`). Regenerated with a loosened fallback so
  all 276 plans carry §20–§23.

## Addendum 2026-09-21 (round 85) — Claim readiness finalisation

- **§24 Claim readiness:** all 276 plans received a readiness verdict, class,
  copy-paste claim block, verification commands, dependencies, and a 12-point
  structural checklist.
- **`CLAIM_READINESS_INDEX.md`:** master index with execution order (governance
  spine → seeds → hubs → standard → free starts), the full 276-row table, and
  the residual authoring gaps.
- Corrections applied during the pass: coupling phrase variants ("their names" /
  "those names"), heading-style detection, and the hub threshold.

## Addendum 2026-09-21 (round 86) — Wave 20: claim-readiness closure

- Four plans produced (`PLAN-READINESS-PACKAGE-IDS-281`,
  `PLAN-READINESS-VERIFICATION-CONTRACT-282`,
  `PLAN-READINESS-HEADER-NORMALISATION-283`,
  `PLAN-READINESS-AUDITOR-284`) in
  `docs/plans/EXPANSION_PROGRAM_WAVE20_2026-09-21/`.
- **Re-verification result:** of the three reported residual gaps, only one is
  real. Plans 04/05/06 do carry package IDs (`### C4-1` / `B5-1` / `F6-1`);
  all five "unverified" plans cite real `scripts/ci/*` commands; all twenty
  "malformed" headers use the legal `**Wave:** N (date) · **Kind:**` form.
- `CLAIM_READINESS_INDEX.md` §4 corrected to record the re-verified truth.
- Layer generator `tools/generators/gen_wave20_readiness_layers.py` v1 added;
  it carries the four rule corrections from rounds 79–85 forward.
- All four Wave 20 plans received the standard layers (§12–§24) and a §24 claim
  block.
