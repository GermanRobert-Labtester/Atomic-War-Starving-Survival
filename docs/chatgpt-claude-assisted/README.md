# ChatGPT–Claude Assisted Draft Plans

**State:** eight documentation drafts across two directions. These are proposals, not approved implementation plans, ownership claims, or evidence that the described behavior is already integrated.

## Selected directions

1. **The Green Return** — recover damaged land and let ecological recovery alter what the map reports over time.
2. **The Trading House** — make a durable institution for coordinating exchange across existing market, contract, credit, and production owners.

Four substantial drafts are provided for each direction, with a target of 25–30k tokens per plan. Each plan should earn its length through evidence, decisions, phased work, acceptance cases, and risks; repeated prose and copied source plans do not count as depth. The split follows distinct responsibilities and acceptance outcomes; it does not create parallel gameplay authorities.

## Evidence and duplicate check

Repository search on 2026-09-29 found no existing plan named **The Green Return** or **The Trading House**, and the requested destination did not exist before this draft. The evidence does show substantial adjacent work, so these plans are additive and bounded:

| Direction | Existing evidence | Boundary for this draft set |
|---|---|---|
| Green Return | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` and `.Live.cs`; `WildlifeMigrationSystem`; `WildlifeEcosystemSystem`; `AgricultureSystem`; `SoilReclamationProfileEngine`; `src/Host/SoilReclamationProfileHostSession.cs`; and the phase-4 `EvolvingWorldDayOwner` in `src/Main.CampaignOwners.cs`. `GreenhousePanel` already reports soil contamination for managed beds; `narrative_questlines.json` contains `quest_the_irradiated_soil`. The active design companions include `.ai/plans/living-region-2026-09-29.md`, `.ai/plans/reconstruction-tree-2026-09-29.md`, and `docs/plans/story-expansion-batch-2/second-nature-and-ruins-of-the-before-2026-09-29.md`. | Cultivated plot fertility, soil amendment, greenhouse contamination, the irradiated-soil quest, settlement condition/refugees/prices, changed wildlife/crops/food webs, and recovered knowledge all have adjacent owners or proposals. These drafts must not recreate those. Their proposed delta is a regional readout and evidence path for existing land facts, with optional migration suitability only after a P0 proves no current seam can serve it. |
| Trading House | `MarketSystem`, `PlayerTradeRouteSystem`, `TradeRouteContract`, `TradeCreditCoordinator`, `BlackMarketSystem`, `BlackMarketSettlementService`, and `TradeRouteMonopolyEngine` exist under `Assets/Ashfall.Core/Economy/`. Relevant plans include `.ai/plans/long-line-freight-2026-09-29.md`, `.ai/plans/underworld-2026-09-29.md`, `docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CONTRACT-BOARD-109.md`, and `docs/plans/CONTRABAND_TRADE_AND_ARBITRAGE_AUDIT.md`. | Freight owns the player's company runs; the contract board owns its offers; credit and black-market owners keep their own state. These drafts define a thin institution-level coordination and records layer. They do not add another market, route, loan, fence, or production simulator. |

The new filenames and scopes were checked against the current plan corpus before writing. Related plans are dependencies and constraints, not material to copy. No source, data, integration ledger, ownership claim, or existing plan was edited. A future implementation must repeat the premise and ownership audit because this evidence pass is a planning snapshot.

### Additional collision checks

- Green Return also sits near `Assets/Ashfall.Core/Exploration/CartographySystem.cs` (`ProjectCanonicalMap`, survey quality from map/fog evidence) and `Assets/Ashfall.Core/World/InSarDeformationEngine.cs` (repeat-pass ground deformation; save section registered as `insar_deformation`). Neither is an ecological recovery owner. The broad DRAFT `docs/plans/expansion_wave1/PLAN_01_WILDLAND_FIRE_AND_BURN_RECOVERY.md` contains repeated generic “wildlife corridor journals” boilerplate. GR-3 is scoped to wildlife migration/corridor facts and excludes that plan's outdoor-fire, burn-recovery, and narrative incident proposal.
- `docs/production/PRODUCTION_TRADE_FLOW.md` claims canonical production/trade-flow authority and names `ProductionTradeFlowSystem.cs`, `RegionalPriceCurveCalculator.cs`, and `trade_flows.json`. A search of current `Assets/Ashfall.Core`, `src`, and `Assets/StreamingAssets/Data` found none of those named paths. This is a documentation lead whose authority claim needs reconciliation; TH-1/2/4 explicitly stop short of treating it as a live API.

## Draft size after continued expansion

Word counts are reproducible; token counts are estimates because tokenizer choice changes the result. At roughly 1.5 tokens per word, the Green Return drafts are about 16.8k–19.5k tokens each; Trading House drafts are about 25.8k–30.6k tokens each. The four Green Return drafts remain below the 25–30k token target. Their current-source contracts, adjacent-owner boundaries, and acceptance cases are documented; adding more without new decisions or evidence risks repeating those constraints.

| Draft | Words |
|---|---:|
| GR-1 — Land Condition and Recovery Baseline | 11,195 |
| GR-2 — Recovery Trajectories and Field Evidence | 12,342 |
| GR-3 — Wildlife Corridors and Return Pressure | 13,004 |
| GR-4 — Stewardship Choices and Map Readout | 11,283 |
| TH-1 — Charter, Membership, and House Ledger | 19,470 |
| TH-2 — Consignments and Exchange Clearing | 20,396 |
| TH-3 — Credit Exposure and Settlement | 19,418 |
| TH-4 — Production Commitments and Closeout | 17,183 |

## Draft set

### The Green Return

- [GR-1 — Land Condition and Recovery Baseline](green-return-1-land-condition.md)
- [GR-2 — Recovery Trajectories and Field Evidence](green-return-2-recovery-trajectories.md)
- [GR-3 — Wildlife Corridors and Return Pressure](green-return-3-wildlife-corridors.md)
- [GR-4 — Stewardship Choices and Map Readout](green-return-4-stewardship-map.md)

### The Trading House

- [TH-1 — Charter, Membership, and House Ledger](trading-house-1-charter-ledger.md)
- [TH-2 — Consignments and Exchange Clearing](trading-house-2-consignments.md)
- [TH-3 — Credit Exposure and Settlement](trading-house-3-credit.md)
- [TH-4 — Production Commitments and Closeout](trading-house-4-production-closeout.md)

## Shared implementation gates

- Each future package starts with a read-only premise audit and claims exact paths before editing.
- Reuse canonical Core owners and existing save ownership; add no parallel mutable market, ecology, or settlement authority.
- Keep optional cross-plan links dark unless both owners and their integrator approve the seam.
- Use seeded randomness only where an existing deterministic contract permits it.
- Follow `bin/run-scoped-tests` and the focused verification rules in `TEST_POLICY.md`.
- Do not treat this draft set as `STATUS: APPROVED BY USER`; no implementation or commit is authorized by these documents.
