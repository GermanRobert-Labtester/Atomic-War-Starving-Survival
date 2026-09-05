# Holdfast Loop Map

## Authoritative flow

```text
story key / authored lore
        ↓
HoldfastQuestSystem.TryStart
        ↓
stage progression / branch selection
        ↓
IceRoadSystem + CensusClaimSystem + BrineWaterSystem
        ↓
HoldfastSaveCodec / campaign section
```

`HoldfastQuestSystem` owns quest progress and branch state. It does not own
ice-road physics, census claims, water chemistry, inventory, or trade value.
Those systems remain separate authorities and are reached through host wiring.

## Quest reachability rules

- The Sheet requires day 90 and a story key through `TickDaily`.
- Clerk fallback begins at day 110 when the story key is present.
- Later spine quests require the preceding quest state.
- A catalog-bound runtime refuses unknown non-built-in quest IDs.
- Failed or completed quests cannot be started again.
- A failed start does not create a placeholder progress record.

Stage prose comes from `holdfast_quests.json`; the runtime uses a bounded
stage index and clamps display reads at the final authored stage.

## Trade boundary

`HoldfastTradeSession` owns transaction validation and the player's trade
projection. The host supplies the canonical inventory and embargo query. UI
previews must use `PreviewBuy`/`PreviewSell`; execution must use the matching
`ExecuteBuy`/`ExecuteSell` command so stale previews cannot commit silently.

## Save boundary

Quest restore deep-copies progress records. Trade state is captured through its
existing save store and must not clear a shared backing inventory during restore.
The campaign envelope remains the persistence owner.

## Remaining work

Faction stance-aware pricing, authored why-lines, and arbitrage property
coverage remain a separate trade-balance phase. They must extend the current
catalog/transaction authority rather than add host-side price math.
