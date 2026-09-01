# Settlement Authority Decision & Identity Model

## 1. Model Selection: Model A (First-Class Settlement Linked to Location)

We adopted **Model A**:
```text
settlement_x (Community, Population, Trade Profile, Faction Allegiance)
      ↓ location_link
loc_settlement_x (Physical map position, travel hours, danger level, rads/hr)
```

### Key Decisions
1. **Settlement Definitions as First-Class Entities:** `settlements.json` defines living social communities with population, governance, trade goods, needs, and allegiance.
2. **Physical Location Decoupling:** Physical topology and destination data reside in `locations.json` under `loc_settlement_*` IDs.
3. **Prefix Authority:** `settlement_*` is registered as an authoritative Tier-1 prefix in `CatalogIntegrityValidator.cs` and `CatalogIntegrityRules.cs`.
4. **No Runtime Overhead:** Settlement definitions are loaded once into memory via `SettlementCatalog.cs` for query resolution by caravans, expeditions, and future territory systems without running a frame-by-frame city simulation.
