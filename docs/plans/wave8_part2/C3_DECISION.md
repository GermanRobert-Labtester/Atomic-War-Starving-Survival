# C3 — Signed Disposition Decision

**Task:** Wave 8 Part 2, TASK C3
**Date:** 2026-09-17
**Signature:** user authorized C3 ("Proceed i authorise wawe 8 part 2 C3!")
**Scope:** disposition only — no production implementation.

## Five-row decision table

| Plan | Disposition | Evidence | Exact consequence |
|---|---|---|---|
| **174** Procedural Survivor Backstories | **HOLD** | Flavor is fully covered by `YearOfAshCatalogLoader.backstory` + `SurvivorEnrichmentService` + echoes/arcs; the only unique capability is *mechanical* origin effects, which would duplicate `TradeSpecialtySystem`/`SurvivorRelationsSystem` unless a single seam is named. | No `BackstorySystem`/`backstory_templates.json`. Recheck condition: **a signed extension point on an existing owner (`SurvivorEnrichmentService` or `TradeSpecialtySystem`) with a consumed gameplay surface**; otherwise retire on the next pass. |
| **175** Meta Profile / New Game+ | **HOLD** | No cross-run profile store, no `CampaignCompletionRecord`/achievement authority in Core; run-local achievements are UI-only. | No `MetaProgressionSystem`/`meta_unlockables.json`/profile store. Recheck condition: **a signed cross-run profile-store owner (versioned, checksummed, outside campaign slots) plus verified Plan 34/149 completion-fact producers.** |
| **191** Item Identification / Appraisal | **RETIRE** (as a standalone system) | `ItemInspectionModel` is already player-visible (`InventoryHostSession.GetInspection` → `InventoryDetailPanel.CurrentInspection`); `ShelterBarterSystem` already rolls an appraisal skill. Persistent unidentified state would require inventing an inventory-instance scope. | No `ItemIdentificationSystem`/`identification_difficulty.json`. Status banner added to both historical Plan 191 docs. Reopen only with a signed inventory-instance unidentified-state owner + consumed reveal surface. |
| **192** Player Trade Routes | **HOLD** | NPC caravan route owners exist (`CaravanTradeRouteCatalog`/`CaravanTradeNetworkSystem`/`TravelingCaravanSystem`); no player route authority; `trade_route_disrupted` key is orphaned but the gap is real. | No `PlayerTradeRouteSystem`/`trade_route_agreements.json`. Recheck condition: **a signed player-route DTO with standing/raid/save seams named in a map amendment.** |
| **199** Seasonal Human/Faction Migration | **HOLD** | Wildlife migration exists; humans must never reuse wildlife state; the signed family map already says "implement OUT until product asks". | No `SeasonalMigrationSystem`/`migration_routes.json`; no seasonal human clock. Recheck condition: **product names a human population owner distinct from fauna.** |

**Result:** 0 PROMOTE · 1 RETIRE · 4 HOLD. No first packages are created
because nothing is promoted.

## Why no PROMOTE

The forensic audit's promotion queue (184, 196, 173) is out of C3's scope. Of
the five C3 plans, each either duplicates an existing owner (174/191) or crosses
an unowned save/authority boundary (175/192/199) — so no production package can
be named without a fresh signed decision.

## Non-goals honored

- No route creation, migration state, seasonal clock, meta store, or
  hidden-item state.
- No second narrative generator, achievement catalog, or appraisal engine.
- No production file changed.
