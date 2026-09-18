# C3 — Endgame Portfolio Disposition — Premise Evidence

**Task:** Wave 8 Part 2, TASK C3 (`Seal-steps/847219_ASHFALL_WAVE8_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md`)
**Status:** DISPOSITIONED (signed 2026-09-17 by user authorization)
**Date:** 2026-09-17
**Base commit:** `033df2b7` + prior C2/D1/D2/D3 work
**Nature:** documentation/disposition only — **zero production changes**.

## 1. Premise re-check

| Plan | Historical record | Re-verified at HEAD |
|---|---|---|
| 174 Procedural Survivor Backstories | UNSTARTED; "requires a new narrative ownership decision" | No `BackstorySystem`/`backstory_templates.json`; only `YearOfAshCatalogLoader.backstory` (flavor) + `SurvivorEnrichmentService` + `EchoSystem`/`NarrativeArcEventSystem` |
| 175 Meta Profile / NG+ | BLOCKED; depends on separate completion/achievement owners | No meta-profile or cross-run store; no `CampaignCompletionRecord`/achievement authority found in Core |
| 191 Item Identification / Appraisal | UNSTARTED; "requires inventory instance and economy pricing contracts" | No unidentified-item state; **but** `ItemInspectionModel` is player-visible (`InventoryHostSession.GetInspection` → `InventoryDetailPanel.CurrentInspection`) and `ShelterBarterSystem` already rolls an appraisal skill |
| 192 Player Trade Routes | PARTIAL; player route authority absent | `CaravanTradeRouteCatalog` + `CaravanTradeNetworkSystem` + `TravelingCaravanSystem` exist; no player route authority; `trade_route_disrupted` feedback key is defined but never fired |
| 199 Seasonal Human/Faction Migration | PARTIAL; wildlife only | `WildlifeMigrationSystem` exists (fauna); no human/faction migration; `PLAN_192_199_ROUTES_MIGRATION_AUTHORITY_MAP.md` already signed "implement OUT until product asks" |

## 2. Collision table

| Plan | Planned capability | Current owner | Overlap | Missing behavior | Disposition |
|---|---|---|---|---|---|
| 174 | Mechanically-relevant procedural backstories (occupation→skills, traumas, relationship hooks, quest hooks) | `SurvivorEnrichmentService`, `TradeSpecialtySystem`, `SurvivorRelationsSystem`, `EchoSystem`, `NarrativeArcEventSystem`, survivor `bio`/`pre_war_profession` data | PARTIAL (flavor fully covered; mechanical origin effects absent) | A signed ownership decision for mechanical origin effects + a consumed surface | **HOLD** |
| 175 | Cross-run meta profile store + NG+ option application | none (achievements are run-local UI; no profile store) | NONE | Meta-state owner with versioning/reset/compatibility; upstream Plan 34/149 producers | **HOLD** |
| 191 | Unidentified salvage → appraisal reveals properties/value | `ItemInspectionModel` (display metadata), `ShelterBarterSystem` (appraisal skill roll) | PARTIAL — inspection + valuation already player-visible | Persistent unidentified inventory-instance state (a new scope) | **RETIRE** (extension-first recheck) |
| 192 | Player establishes/defends trade routes, dispatches caravans, agrees tariffs | `CaravanTradeRouteCatalog`, `CaravanTradeNetworkSystem`, `TravelingCaravanSystem` | PARTIAL (NPC routes) | Player-route DTO + standing/raid/save seams | **HOLD** |
| 199 | Seasonal human/faction migration + refugee flows | `WildlifeMigrationSystem` (fauna only) | NONE (humans) | Human population owner distinct from fauna | **HOLD** |

## 3. Authority boundaries

- Documentation/portfolio truth only; the integration ledger receives first
  packages only for PROMOTE (none here).
- Existing survivor arc/echo/enrichment owners constrain 174.
- Existing item inspection/valuation owners constrain 191.
- Campaign ending/generational owners constrain 175.
- Caravan/economy and wildlife/faction-ecology owners constrain 192/199.
- No seasonal clock, migration state, route state, or hidden-item state added.
