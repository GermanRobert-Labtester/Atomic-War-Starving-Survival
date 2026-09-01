# Maritime Authority Map (Plan 23)

One authority per maritime fact. Provisional owners below were verified against
repository truth during the Plan 23 baseline (see `PLAN23_BASELINE.md`); where the
source plan's assumption differed, repository truth is recorded and binding.

| State / rule | Authority (verified) | Plan 23 use |
|---|---|---|
| Flotilla faction identity | `holdfast_factions.json` roster (same file that defines `faction_the_fleet`) + `ExpansionSuite` Exp 09 definition | `faction_black_flotilla` entry + lore |
| Standing / stance | `Ashfall.Core/Economy/FactionStanceEngine.cs` (`FactionThresholds`, trust −100..100, raid/rob/trade/intel thresholds) | Register Flotilla thresholds; deepen reactions only |
| Trade specialty | `hardcore_economy_tuning.json` `faction_preferences` (`FactionTradePreferenceDto`) + `settlements.json` `economy.trade_specialty` | Marine salvage buys-at-premium / refuses table |
| NPC identity | `characters.json` (54 entries, `npc_*`) | Six named Flotilla NPCs |
| Radio delivery | `Ashfall.Core/Radio/FactionRadioEngine.cs` + `faction_radio_corpus.json` bands | Eight Flotilla broadcasts |
| Dive-site definitions | `Ashfall.Core/Maritime/DiveSiteCatalog.cs` + `dive_sites.json` (schema v2) | 14-site target; new fields only if a live mechanic needs them |
| Active dive state | `MaritimeDiveSystem` (`StealthDiveInstance` is a thin subclass) | Site execution, air, noise, decompression, outcomes |
| Stealth/noise | `MaritimeDiveSystem.NoiseLevel` (0..100, compromise at 80, loss at 100+deep) | Tight-space/noise identities per site |
| Safe/vault state | `Ashfall.Core/Maritime/SafeCrackingSystem.cs` | Two safe-heavy sites; safes registered when their room/dive is entered |
| Variable loot | `Maritime/VariableLootNode.cs` tables via `ProceduralScavengeSystem.RollLootTable` | Deterministic site loot |
| Procedural scavenging | `Maritime/ProceduralScavengeSystem.cs` | Site replay/variation, degradation, decontamination |
| Psychological contamination | `Maritime/PsychologicalContaminationSystem.cs` (location-keyed) | Deep/hazard dive effects, action blocking, chronicle entries |
| Deep-coast orchestration | `District8DeepCoastSystem` (stages, route, bills, dock operation, fleet levy) | Dive-site access spine beyond the Shelf |
| Dive launch | `Expeditions/DiveInstanceRunner.cs` (flag/event runner) + `MaritimeDiveSystem` | Keeper thread, storm-masking noise model |
| Coastal location graph | `holdfast_locations.json` (`loc_shelf_*`) + `year_of_ash_locations.json` (`loc_maritime_icebreaker_dock`) | Site anchoring/discovery |
| Weather | `Ashfall.Core/World/WeatherSystem.cs` (`WeatherKind`, `OnWeatherChanged`) | Surge producer source (storm kinds), already consumed by deep-coast `TickDaily` |
| Map/location evolution | `WorldEvolutionEngine` + `world_evolution_events.json` (day/flag → node lock, owner, danger, contamination) | Surge/flood world-state changes |
| Tide phase | **does not exist** — minimal deterministic Core tide query is the sanctioned new piece (Plan §6.2) | Derived from campaign day; no wall clock, no serialized phase |
| Inventory/equipment | `items.json` + secondary item catalogs (`ItemCatalogLoader`) | Flotilla gear as real items |
| Save state | `MaritimeSaveStore` (checksummed envelope) + `SaveSectionRegistry` ("maritime") | Deterministic restore, old-save compatibility |
| Player commands | `PlayerCommandCode` + `CommandPreview/CommandResult` (deep-coast pattern) | Any new player-facing verbs follow the preview/execute contract |

## Non-owners (explicitly not duplicated)

- No second stance/standing meter (Muster trust, Foundry guild stance, Standing Record all keep their own authorities).
- No maritime currency; trade uses existing economy tuning.
- No second radio runtime (`FactionRadioEngine` is the only delivery path).
- No separate dive/oxygen/noise engine beyond `MaritimeDiveSystem`.
- No separate weather or map-evolution engine; surges are authored world-evolution events triggered by verified weather/flag producers.
- Muster currents are a people roster, not water; they are never treated as hydrology.
