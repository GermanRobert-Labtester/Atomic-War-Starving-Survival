# Plan 99 — Hardcore Economy Tuning Baseline

## 1. Initial State Inventory

Prior to Plan 99 expansion, `hardcore_economy_tuning.json` contained a minimal early-game configuration:
- **Scarcity Tiers:** 2 tiers (`Critical` for Days 1–15, `High` for Days 15–40).
- **Faction Preferences:** 2 entries (`central_garrison_remnants` and `faction_black_flotilla`).
- **Price Shock Rules:** 1 rule (`PlumePassing`).

```mermaid
graph LR
    subgraph Baseline State [2 Tiers / 2 Factions / 1 Shock]
        T1[Critical: Days 1-15 x2.5]
        T2[High: Days 15-40 x2.0]
        F1[central_garrison_remnants]
        F2[faction_black_flotilla]
        S1[PlumePassing x1.8 3d]
    end

    subgraph Expanded State [8 Tiers / 8 Factions / 6 Shocks]
        ET[8 Full-Campaign Tiers]
        EF[8 Major Faction Profiles]
        ES[6 Dynamic Event Shocks]
    end

    Baseline State --> Expanded State
```

---

## 2. Baseline Record Audit

### 2.1 Baseline Scarcity Tiers
1. `Critical`: Multiplier `2.5`, Range `Days 1-15`, Items: `["clean_water", "iodine_pills", "anti_rad", "air_filter"]`. Rationale: "Immediate survival. Everyone needs them. Nobody has enough."
2. `High`: Multiplier `2.0`, Range `Days 15-40`, Items: `["antibiotics", "medical_kit", "fuel", "water_filter"]`. Rationale: "Infections set in. Filters clog. Fuel runs low."

### 2.2 Baseline Faction Preferences
1. `central_garrison_remnants`:
   - `buys_at_premium`: `["ammo_*", "body_armour_military", "fuel", "mre_military"]`
   - `refuses`: `["jewelry", "book", "cigarette"]`
   - `trade_currency`: `"Fuel, ammunition, obedience"`
2. `faction_black_flotilla` (added in Plan 23):
   - `buys_at_premium`: `["item_marine_sealant_kit", "item_descent_line", "item_sealed_dive_lamp", "item_rebreather_canister", "brass_fittings", "scrap_mechanical", "item_process_barrel", "item_ro_resin", "chart_*", "paper_scrap"]`
   - `refuses`: `["jewelry", "book", "family_photograph", "item_teddy_bear"]`
   - `trade_currency`: `"Dry cloth, medicine, fuel, and salvage with paper on it"`

### 2.3 Baseline Price Shock
1. `PlumePassing`:
   - `kind`: `PlumePassing`
   - `multiplier`: `1.8`
   - `duration_days`: `3`
   - `affected_item_ids`: `["*"]`
   - `trigger`: `"fallout storm crosses a trade route"`

---

## 3. Defects & Limitations Identified in Baseline

1. **Horizon Truncation:** Scarcity tuning stopped at Day 40, leaving days 41–365+ completely untuned.
2. **Wildcard Matching Defect:** `HardcoreEconomyTuning.MatchesItem` previously evaluated `trimmed == "*"` or `string.Equals`, failing to expand prefix wildcards like `ammo_*` and `chart_*`.
3. **Day Range Boundary Parsing:** `MatchesDay` failed on open-ended ranges like `"Days 341+"` when no trailing space preceded `+`.
4. **Enum Restriction:** `ScarcityTier` and `PriceShockKind` were restricted to 4 enum members each, preventing full-campaign expansion without enum modernization.
