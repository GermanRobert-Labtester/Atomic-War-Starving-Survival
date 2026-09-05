# Plan 61 — Save & Persistence Compatibility Report

**Document Version:** 1.0.0
**Authority:** `Assets/Ashfall.Core/Economy/TradeScreenPresenter.cs`, `TradeSelectionSnapshot`

---

## 1. Old-Save Compatibility Contract

1. **Preserved Baseline IDs:**
   - All three baseline scenario IDs (`fair_deal`, `offer_short`, `empty_table`) are preserved verbatim with identical schemas and semantics.
   - Any historical save, test fixture, or campaign snapshot referencing these IDs continues to load and evaluate identically.

2. **Additive Catalog Expansion:**
   - The 12 new scenarios are purely additive.
   - No fields were removed or renamed in `trade_screen_scenarios.json`.
   - The root object structure (`schema_version: 1`, `version: 1`, `scenarios: [...]`) is byte-compatible with the pre-expansion loader.

---

## 2. Active Session Save & Restore Lifecycle

`TradeScreenPresenter` implements fine-grained capture and restoration of user selections via `TradeSelectionSnapshot`:

```csharp
public sealed class TradeSelectionSnapshot
{
    public Dictionary<string, int> PlayerOffers { get; set; }
    public Dictionary<string, int> FactionAsks { get; set; }
    public Dictionary<BiologicalTradeItem, int> BiologicalOffers { get; set; }
}
```

### Verified Guarantees:
- **Zero-Mutation Invariant:** Presentation binding, recalculation, and snapshot restoration never modify underlying simulation providers (`IFactionStanceProvider`, `IPriceShockProvider`).
- **Idempotent Restoration:** Restoring a snapshot restores exact item counts, biological offering counts, qualitative worth labels, and fairness verdict.
- **Faction Independence:** Switching factions or closing the screen clears the transient offer table cleanly without cross-contaminating subsequent encounters.
- **Deterministic RNG:** Tell line rotation and radio ticker selection use `ISeededRng` seeded from the campaign seed and day; screen refresh never consumes random numbers out of order.
