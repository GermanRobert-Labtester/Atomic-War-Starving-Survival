# Flagship Institutions (Tasks 5–8) — Implementation Log

CulturalArchiveVaultSystem · DiplomaticSummitSystem · SkyDefenseBatterySystem · PsychologicalSanatoriumSystem

---

## Phase A — Reconnaissance

Status: PASS (with recorded baseline divergence)

### Baseline gates (2026-09-05, branch `feat/asset-pipeline-flagship`, HEAD 86e5f698)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` (host) | PASS — 0 errors |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **FAIL — 620 errors, PRE-EXISTING** |

Test-suite compile failure is **not attributable to this work**: 48 files error —
43 **untracked** test files from a concurrent stream (Debt*/WildlifeTrapping*/…,
written against Core APIs that don't exist on this branch yet) plus 5 tracked
`DistressSignal*Tests.cs` files (commit a75ceef4) referencing
`RadioDistressSystem.TryTriggerMoralChoice` etc., absent from Core here.
Working tree also carries ~17 modified Core/src files from concurrent streams.
**Foreign work is untouched.** Focused gates for this plan run via a
gitignored `_verify_flagship.csproj` that compiles Core + only this plan's
test files; the new test files also live in `Ashfall.Core.Tests/` so they run
in the canonical suite once the tree heals.

### Authority map

| Authority | Owner | Key API |
|---|---|---|
| Save sections | `Ashfall.Core/Save/SaveSectionRegistry.cs` | add entry to `All` + `SectionFileNames`; `SchemaVersions` only for codec ladders |
| Save store façade | `src/Host/SaveStoreHub.cs` | `SaveStoreHub.FromCodec(fileName, tag, SchemaVersionedEnvelope<T>.Encode, .Decode)`; newest example `src/Host/PoliticsSaveStore.cs` |
| Save orchestration | `src/Main.SaveOrchestrator.cs` | `SaveAll` → per-section `SaveXxx()` → `CaptureSection(key, payload)`; restore via `RestoreAllSubsystemsFromDisk()` ordered `SetupXxx` chain |
| Inventory (atomic) | `Ashfall.Core/Inventory/Inventory.cs` | `TryExecuteTransaction(InventoryBill)` / `TryConsumeBill(dict)`; snapshot-isolated, all-or-nothing, `OnInventoryChanged` once |
| Item catalog | `Inventory/ProceduralItemInstance.cs:ItemCatalog` | `Get(id)`; loaded by `ItemCatalogLoader` from `items.json` (bare ids, no `item_` prefix for common goods) |
| Content utilization | `Ashfall.Core/Content/ContentUtilizationScanner.cs` | register in `AuthoritativeCatalogs` + `loaderPatterns` + `consumerMap`; runtime evidence via `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Data integrity | `Ashfall.Core/CatalogIntegrityValidator.cs` | auto-scans Data dir; new prefixes → `IdPrefixes`, def keys → `DefinitionKeys`, ref keys → `ReferenceKeys`; object roots need `schema_version` |
| Campaign time | `TickDay(int day)` convention | all systems day-based; no wall clock |
| RNG | `ISeededRng` (`Ports.cs`) | `Seed/Next(min,max)/NextFloat()/NextDouble()`; injected |
| Survivor identity | `Survivors/SurvivorId.cs`, `SurvivorAggregate.cs` | stable ids; `Lifecycle` (Resident/Deployed/Deceased); `ActiveExpeditionId` |
| Morale/needs | `Survivors/NeedsSystem.cs` | `Modify(survivor, NeedKind.Morale, delta, hours)`; `Morale` 0–100 |
| Trauma (canonical) | `Survivors/CombatTraumaSystem.cs` (hypervigilance), `SomaticFlashbackSystem`, `GuiltInsomniaSystem` | sanatorium maps onto these, no second trauma model |
| Relations | `SurvivorRelationsSystem.cs` | query API for therapist–patient trust |
| Skills | `Survivors/SkillCatalogLoader.cs`, `skills.json` | `skill_cold_analysis`, `skill_watchful` present |
| Orbital telemetry | `OrbitalHarrowTelemetrySystem.cs` | `OnImpactWarning(OrbitalWarningEntry{day,targetGridX,energyMj,eventId,severity})`; `warningLeadDays=3`; `TickDay`→`ResolveImpact()`→`_armor.EvaluateKineticImpact(cellX, energy, out dmg)`; Capture/Restore |
| Sky armor | `Shelter/SkyLayerArmorSystem.cs` | `EvaluateKineticImpact` is the single damage handoff — interception must reduce energy BEFORE resolution |
| Vinyl/media | `VinylMoraleSystem.cs` | `LoadCatalog(List<VinylRecordDefinition>)`, `AcquireRecord(id)`, `Play(id, day)`, `ApplyDailyEffect` — record cutting registers here, playback morale stays owned by vinyl |
| Journal/codex | `Journal/JournalSystem.cs` | `TryAddRawEntry`, `UnlockCodex(knowledgeKey)` |
| Flags | `Flags/IFlagLedger.cs`, `CampaignConsequenceLedger` | unlock records |
| Memorial | `Memorial/MemorialSystem.cs` | oral-history linkage by ID |
| Factions/war | `YearOfAsh/FactionWarSystem.cs` (+ Factions/) | standing/war authority — diplomacy publishes rules, never duplicates |
| Neutral summit site | `locations.json` | `loc_waystation_crossing` DOES NOT EXIST → use `loc_neutral_ground` (verify flags in Phase D) |
| Humidity | `World/WeatherSondeSystem.cs` | authoritative humidity query (verify signature in Phase C) |

### Item-ID reality vs plan text (authored-data divergence, recorded)

Plan fixtures name items that don't exist in `items.json` (bare-id convention):

| Plan says | Resolution |
|---|---|
| `chemical_sedative` | use existing `sedative_draught` |
| `mineral_salts` | use existing `item_preservation_salt` |
| `paper_stock` | author new item `paper_stock` |
| microfiche/acetate media | author `microfiche_film`, `acetate_blank_disc` |
| `machine_oil`, `scrap_chemical`, `clean_water`, `mechanical_parts` | exist as-is |

### Key design locks

- **Sky defense hook**: subscribe `OnImpactWarning` → engagement track; on successful
  interception call new `OrbitalHarrowTelemetrySystem.ApplyInterceptionMitigation(eventId, residualFraction)`
  (only public API added to telemetry; energy reduced before `ResolveImpact` → armor pipeline unchanged).
- **Ammo ownership**: loaded-magazine model — atomic transfer inventory→magazine on load;
  volley consumes from magazine. Ordnance ids double as item ids (single countable authority).
- **Salon modifier**: one shelter-wide active modifier, duration+cooldown persisted, applied
  through a `SalonMoraleTick` event consumed by host→NeedsSystem; never stacks.
- **Guarantees/hostages**: survivor stays in roster; availability via diplomacy-owned status +
  event; identity never deleted.
- **Diplomacy**: publishes `TreatyPolicySnapshot`/`IsArmedPatrolAllowed`; violations consumed
  from patrol/raid movement reports; standing changes routed via port bound to FactionWarSystem.
- **Sanatorium conditions**: authored `condition_*` ids in `psychological_therapies.json`
  mapped onto canonical trauma surfaces (hypervigilance/flashback/guilt-insomnia) at admission
  and discharge; no duplicate condition enums.
- **Save**: 4 new sections `cultural_archives`, `diplomatic_summits`, `sky_defense_battery`,
  `psychological_sanatorium`; host façades via `SaveStoreHub.FromCodec` +
  `SchemaVersionedEnvelope<T>`; empty defaults for old saves.

---

## Phases B–H — Execution record

| Phase | Status | Commit | Result |
|---|---|---|---|
| B — catalogs+validation | PASS | 4969eaf9 | 4 catalogs authored; 9 items added; validators registered; 15 tests |
| C — culture core | PASS | b8b3db8c | CulturalArchiveVaultSystem + 16 tests |
| D — diplomacy core | PASS | 2bb13049 | DiplomaticSummitSystem + 15 tests |
| E — sky defense core | PASS | 12cd4b9b | SkyDefenseBatterySystem + telemetry mitigation API + 13 tests |
| F — sanatorium core | PASS | 8a4bfdd0 | PsychologicalSanatoriumSystem + 14 tests |
| G — cross-system | PASS | 6ea06806 | InstitutionAssignmentLedger + contention tests + 30-day replay harness (8 tests) |
| H — save/content closure | PASS | dac83eb8 | 4 save sections + host facades + day owner + content utilization (Orphaned: 0) |

Note: another stream's `04884519 chore: sync working tree` commit swept up
Phase G work-in-progress files verbatim; content verified intact in HEAD.

### Divergences from plan (all recorded, all data-driven)

1. `loc_waystation_crossing` does not exist → neutral site is `loc_neutral_ground`.
2. `chemical_sedative` → `sedative_draught`; `mineral_salts` → `item_preservation_salt`;
   `paper_stock`/`microfiche_film`/`acetate_blank_disc` authored as new items;
   ordnance ids double as item ids (single countable authority).
3. Contention fixtures use the real item ids above (plan §14 list adjusted).
4. Skills/standing/condition ports left null in v1 host wiring (systems are
   null-permissive; humidity provider null — no shelter-humidity authority
   exists, plan §5.9 "if available").
5. Validator needed VocabularyKeys repairs for 5 label columns, 2 of them
   pre-existing FAILs on this branch (Plans 90-93) — repaired alongside.

### Remaining known limitations

1. Shared `Ashfall.Core.Tests` suite remains uncompilable from concurrent
   streams' in-flight files (43 untracked + 5 committed radio tests vs absent
   Core APIs). Flagship gates run via gitignored `_verify_flagship.csproj`;
   the 6 flagship test files are globbed by the canonical suite and run once
   the tree heals.
2. UI panels for the four institutions are out of scope (plan §24: core-system
   tasks; scene lint not required). Systems expose events + state for a future
   Stitch-designed panel wave.
3. Culture salon morale surfaces as an event (`OnSalonMoraleTick`) awaiting a
   host consumer into NeedsSystem (single line when wired).
4. Diplomacy standing deltas route through `IFactionStandingPort`; host
   binding to FactionWarSystem pending the same null-port note as (4).

---

## §27 Handoff note

```text
Culture:
- tome count: 12 (cultural_archive_tomes.json)
- archive degradation authority: CulturalArchiveVaultSystem (authored formula)
- humidity authority: none authoritative; injected Func<float> provider (null = dry)
- knowledge unlock authority: knowledge_preserved flag + OnMicroficheCreated /
  OnTomeTranscribed events (JournalSystem consumes at host when wired)
- recording/media integration: culture authors VinylRecordDefinition; host merges
  into VinylMoraleSystem catalog (LoadCatalog replaces — never merge inside culture)
- salon modifier authority: single shelter-wide state, OnSalonMoraleTick event
- save version: 1 (cultural_archives_save.json)

Diplomacy:
- treaty count: 8 (diplomatic_treaties.json)
- faction authority: canonical faction systems via IFactionContextPort / IFactionStandingPort
- territory/DMZ authority: DiplomaticSummitSystem publishes IsArmedPatrolAllowed;
  zone ids validated against live world catalogs at load
- grievance/war authority: violations route via IFactionStandingPort AdjustStanding
- guarantee survivor status authority: availability claim + GuaranteeState (identity never removed)
- save version: 1 (diplomatic_summits_save.json)

Sky Defense:
- ordnance count: 6 (sky_defense_ordnance.json; ids = item ids)
- telemetry authority: OrbitalHarrowTelemetrySystem (world-owned instance)
- intercept probability authority: SkyDefenseBatterySystem.ComputeInterceptChance, clamp 15-85
- ammo ownership model: loaded magazine (atomic inventory<->magazine transfer)
- armor/damage handoff: ApplyInterceptionMitigation reduces pending energy;
  residual always flows through SkyLayerArmorSystem.EvaluateKineticImpact
- heat timing authority: per-volley heat + daily dissipation (TickDay)
- save version: 1 (sky_defense_battery_save.json)

Sanatorium:
- therapy count: 8 + 6 conditions (psychological_therapies.json)
- canonical condition authority: ISurvivorConditionPort (canonical trauma systems)
- survivor availability authority: InstitutionAssignmentLedger (one claim per survivor)
- treatment effect authority: single ApplyTherapyOutcome applier
- relapse RNG stream: keyed stream seed = FNV(masterSeed, survivorId, day); no persisted continuation
- inventory authority: global Inventory via atomic transactions (sedatives not duplicated)
- save version: 1 (psychological_sanatorium_save.json)

Shared:
- campaign time authority: TickDay(day) convention
- RNG stream strategy: keyed FNV-derived SeededRng streams; diplomacy keyed per
  (seed, summit, round); defense per (seed, track, volley); sanatorium per (seed, survivor, day)
- content utilization result: PASS, Orphaned 0
- data integrity result: PASS, 0 errors, 231 catalogs
- old-save fixture result: RestoreState(null) defaults pinned per system (5 tests)
- 30-day replay hash/trace result: uninterrupted == restored fingerprint (seed 42)
- dotnet build result: PASS 0 errors 0 warnings
- dotnet test result: 92/92 flagship gates (_verify_flagship.csproj)
```
