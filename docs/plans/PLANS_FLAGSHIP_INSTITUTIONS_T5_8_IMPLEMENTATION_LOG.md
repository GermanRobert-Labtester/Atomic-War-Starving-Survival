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
