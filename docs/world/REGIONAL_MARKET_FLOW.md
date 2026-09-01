# Regional Goods Flow & Market Dynamics

**Authority Catalog:** `Assets/StreamingAssets/Data/caravans.json`
**Market System:** `Assets/Ashfall.Core/Economy/MarketSystem.cs` / `Assets/Ashfall.Core/Economy/CaravanAtomicTrader.cs`

---

## 1. Regional Commodity Specialization

Commodity price and availability differ across the six wasteland regions, creating natural arbitrage opportunities and driving diplomatic treaty pressures.

```
[ Region 4: Deep Coast ] ──(Salt, Iodine, Fuel)──► [ Region 1: The Holdfast ] ◄──(Tools, Scrap)── [ Region 3: Industrial Belt ]
                                                           ▲
                                                           │ (Medical, Radios, Rations)
                                                           ▼
[ Region 5: Ash Flats ] ──(Grain, Timber, Honey)──► [ Region 2: Dead Suburbs ] ◄──(Cold Gear)── [ Region 6: High Scarp ]
```

---

## 2. Supply, Demand & Embargo Influences

1. **Caravan Arrivals:** When a caravan docks at a waystation or settlement market, local supply of its specialty items increases by +30–50%, reducing local prices.
2. **Treaty Embargoes:** When an active treaty policy triggers an embargo (e.g. Garrison Fuel Embargo or Scale Medical Embargo), affected regional goods see +100% price penalties and -80% volume availability.
3. **Decay to Equilibrium:** Supply adjustments decay back to base rates at a steady 5% per simulation day if no new deliveries occur.
