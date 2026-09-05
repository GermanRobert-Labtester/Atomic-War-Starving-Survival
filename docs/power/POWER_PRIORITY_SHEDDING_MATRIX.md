# Power Priority & Shedding Matrix

> **Shedding Strategy:** Rules, priority bands, and deterministic load management.

---

## 1. Load Shedding Tiers

```
[Available Power Decreases]
        │
        ▼
   Tier 1: Low Priority Shedding (Foundry, Main Lighting, Common Mess, Dormitory)
        │
        ▼
   Tier 2: Standard Priority Shedding (Greenhouse, Workshop, Kitchen, Radio, Lab, Armory, Cold Storage, Surveillance)
        │
        ▼
   Tier 3: Critical Core Preservation (Filtration, Water Pump, Treatment, Clinic, Quarantine, Airlock)
        │
        ▼
   Tier 4: Total Grid Collapse (Battery Depleted, All Circuits Dark)
```

---

## 2. Priority Band Definitions

1. **`Critical` (3):** Life-support systems essential for ongoing survival.
   - Preserved longest during brownout and deficit.
   - Total demand (760 W) intentionally kept under baseline dynamo generation (800 W).
2. **`Standard` (2):** Operational and productive shelter rooms.
   - Core production and tactical facilities that require deliberate player scheduling under constrained generation.
3. **`Low` (1):** Comfort, non-critical lighting, and heavy batch manufacturing.
   - Intended as first sacrificial loads when available watts drop.
4. **`Disabled` (0):** Circuit breaker opened or manually isolated by player.
   - Completely excluded from `ComputeTotalDraw()`.

---

## 3. Determinism & Tie-Breaking

- When total load exceeds generation plus battery discharge capacity, `IsBrownout` becomes true.
- If brownout exceeds 4 hours during a tick, breakers evaluate a 10% trip check (`rng.NextDouble() < 0.10`).
- Because all rolls consume the injected `ISeededRng`, trip outcomes and load shedding are **100% deterministic** across identical seeds.
- The order of room evaluation in `PowerGridSystem` is based on the immutable list of `_rooms`, preserving identical behavior regardless of dictionary hashing.
