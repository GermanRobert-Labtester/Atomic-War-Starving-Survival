# Plans 192 / 199 — Trade routes + human migration authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_192_*`, `Plan_199_*`. **Never reuse wildlife migration state.**

---

## 1. Premise

| Concern | Owner | Save |
|---|---|---|
| Faction route defs + NPC manifests | `CaravanTradeNetworkSystem` + `CaravanTradeRouteCatalog` | `caravan_trade_network` |
| Wandering merchants | `TravelingCaravanSystem` | `caravan` |
| Narrative 18-route flavor | `TradeCaravanCatalog` | **CODEX_ONLY** |
| Physical corridors | `RouteInfrastructureSystem` | `route_infrastructure` |
| Static faction territory | `FactionTerritoryCatalog` | not a migration save |
| Fauna packs | `WildlifeMigrationSystem` | **`world`.Wildlife** |
| Ecology / bestiary | `WildlifeEcosystemSystem` | `wildlife_ecosystem` |

Player-owned routes: **none**. Seasonal human population: **none**. `SeasonalEventSystem` is hazards, not demography.

---

## 2. Ownership (proposed)

Player standing/raid outcomes extend **caravan/economy**, not a new route ledger, until a signed player-route DTO exists. Human/faction seasonal movement, if ever, is a **separate population owner** — never `WildlifeSaveState`.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| `PlayerTradeRouteSystem` / `SeasonalMigrationSystem` / `migration_routes.json` | **OUT** (this map) |
| Writing humans into wildlife packs or ecosystem observations | **OUT** |
| Promoting narrative caravan JSON to gameplay | **OUT** |
| Joint world/economy/faction map (this document) | **IN** |
| Player route ownership | **DEFER** until standing/raid/save seams are named in a later amendment |
| Human population owner distinct from fauna | **IN** as a rule; implement **OUT** until product asks |

**Next implement:** none until a product decision names player-owned routes or human demography. This map only freezes the forbid-wildlife-reuse rule.

---

## 4. Evidence paths

`Economy/CaravanTradeRouteCatalog.cs`, `CaravanTradeNetworkSystem.cs`, `TravelingCaravanSystem.cs`, `WildlifeMigrationSystem.cs`, `World/WildlifeEcosystemSystem.cs`, `WorldSaveStore.cs`, `caravan_trade_routes.json`, `FactionTerritoryCatalog.cs`, `RouteInfrastructureSystem.cs`, `docs/ecology/WILDLIFE_MIGRATION_SCHEMA.md`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
