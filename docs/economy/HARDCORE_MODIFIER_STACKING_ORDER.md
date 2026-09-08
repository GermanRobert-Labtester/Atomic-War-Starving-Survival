# Hardcore Modifier Stacking Order

> This is the composition contract for consumers of the hardcore overlay. It
> does not create a second `MarketSystem` price authority. The current live
> market still calculates base price × demand; the trade presentation seam
> queries scarcity and shock values from `HardcoreEconomyTuning`.

## 1. Mathematical Stacking Model

When a future trade valuation consumer composes the overlay with an item quote,
economic modifiers stack multiplicatively according to this strict hierarchy:

$$\text{FinalPrice} = \text{BaseValue} \times M_{\text{ScarcityTier}} \times M_{\text{FactionPreference}} \times M_{\text{PriceShock}}$$

```mermaid
flowchart TD
    BV[Base Item Value: items.json] --> ST[Scarcity Tier Multiplier: 1.0x - 2.5x]
    ST --> FP[Faction Preference Multiplier: 1.0x - 1.5x]
    FP --> PS[Price Shock Multiplier: 1.0x - 2.0x]
    PS --> EFF[Effective Trade Price: Bounded < 10.0x Base]
```

### Modifier Layers:
1. **Layer 1 — Base Trade Value:** Defined in `Assets/StreamingAssets/Data/items.json` (`tradeValue`).
2. **Layer 2 — Scarcity Tier ($M_{\text{ScarcityTier}}$):** Derived from `HardcoreEconomyTuning.GetScarcityMultiplier(day, itemId)` (ranging from `1.3x` to `2.5x`).
3. **Layer 3 — Faction Preference ($M_{\text{FactionPreference}}$):** Read from `TryGetFactionPreference`; the preference lists are policy inputs, not a second price table.
4. **Layer 4 — Transient Price Shock ($M_{\text{PriceShock}}$):** Read from `TryGetPriceShock` when an event owner supplies the shock's day offset.

---

## 2. Hard Ceiling & Worst-Case Bound

To prevent runaway hyperinflation and economy breakdown:
- **Maximum Theoretical Stack:**
  $$\text{Critical Tier (2.5x)} \times \text{Faction Premium (1.5x)} \times \text{Disease Outbreak (2.0x)} = 7.5\times \text{Base Value}$$
- **Safety Ceiling:** The effective compounded multiplier is strictly guaranteed to remain below **`10.0x`** across all valid combinations.
