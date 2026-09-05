# Hardcore Caravan Handoff

## 1. Integration Boundary

This document defines how roaming merchants and automated caravans consume `HardcoreEconomyTuning`:

### 1.1 Caravan Valuation Queries
- Roaming caravans evaluate regional goods against `GetScarcityMultiplier(currentDay, itemId)` to calculate their rolling trade offers.
- When an active price shock occurs along a caravan route (e.g. `ConvoyAmbush` or `PlumePassing`), caravans modify their inventory markup via `TryGetPriceShock`.

### 1.2 Route Hazard Coupling
- Caravans that encounter `ConvoyAmbush` lose a percentage of their cargo and double their remaining fuel valuation for 3 days.
- Caravans encountering `PlumePassing` refuse to linger in open staging grounds, demanding immediate closure of trades.
