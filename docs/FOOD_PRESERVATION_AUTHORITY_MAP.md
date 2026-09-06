# ASHFALL Plan 64 — Food Spoilage, Cryogenic Refrigeration & Meat-Smoker Curing Authority Map

**Subsystem:** Food Spoilage, Preservation Cohorts, Smokehouse & Cryogenics
**Core Authority:** `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`
**Data Authority:** `Assets/StreamingAssets/Data/food_preservation.json` (`schema_version: 1`)
**Save Store:** `src/Host/FoodPreservationSaveStore.cs` (`food_preservation`)
**Host & Presentation:** `src/UI/FoodPreservationPanel.cs`, `assets/ui/panels/FoodPreservationPanel.tscn`, `src/Main.Plans62_65.cs`

---

## 1. Subsystem Architecture

Plan 64 refines food management from a raw calorie counter into a realistic, deterministic spoilage and preservation model:
1. **Freshness Cohorts:** Food items are tracked in discrete age and preservation cohorts (e.g. `RawMeat:Batch#14:Fresh`, `SmokedRations:Batch#12:Cured`) rather than individual objects, keeping heap allocations near zero.
2. **Preservation Tiers:**
   - **Unpreserved / Ambient:** Rapid decay (3–5 days). Vulnerable to warm summer temperatures and vermin.
   - **Root Cellar:** Cool subterranean storage, 2.5× lifespan multiplier. No power draw.
   - **Salt-Cured & Smoked:** Wood/salt processing jobs convert raw protein into long-life cured provisions (30–60 days).
   - **Pickled / Fermented:** Acidic brine fermentation in crocks (45–90 days).
   - **Cryogenic Freezing:** High-tech electric freezers with indefinite preservation, but contingent on continuous power from `PowerGridSystem`.
3. **Power Outage Failure Modes:**
   - If the power grid suffers a blackout or brownout, cryogenic lockers lose thermal stability.
   - Food begins rapid thawing and spoils if power is not restored within 48 hours.
4. **Spoilage & Disease Vector:**
   - Spoiled cohorts produce rancid waste or hazardous rations.
   - If survivors consume contaminated rations during acute famine, the medical pipeline triggers foodborne illness (`disease_cholera`, `disease_wellspring_cramps`).

---

## 2. Catalog Schema (`food_preservation.json`)

Each preservation technique and perishable category definition includes:
- `id`: Unique string key (e.g., `preservation_ambient`, `preservation_root_cellar`, `preservation_salt_cured`, `preservation_smokehouse`, `preservation_cryogenic`).
- `display_name`: Human-readable label.
- `shelf_life_multiplier`: Multiplier applied to base food shelf-life.
- `power_draw_per_unit`: Active kilowatt load per 50 ration capacity.
- `required_room`: `room_storage_bay`, `room_workshop_precision`, or kitchen extension.
- `processing_recipe`: Required inputs (e.g. salt, fire wood, chemical preservative) and labor ticks.
- `morale_modifier`: Impact on survivor culinary morale when eating this preserved food tier.

---

## 3. Core Domain Classes

```csharp
namespace Ashfall.Core.Shelter
{
    public enum FoodPreservationTier
    {
        Ambient = 0,
        RootCellar = 1,
        SaltCured = 2,
        WoodSmoked = 3,
        Fermented = 4,
        Cryogenic = 5
    }

    public sealed class FoodBatchCohort
    {
        public string CohortId { get; set; } = string.Empty;
        public string FoodItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public FoodPreservationTier Tier { get; set; }
        public int DayStored { get; set; }
        public float FreshnessPercent { get; set; } = 100f;
        public bool IsSpoiled { get; set; }
    }
}
```

---

## 4. Invariants & Determinism

- Decay per day = `BaseDecayRate / (TierMultiplier * TemperatureFactor)`.
- If power outage occurs: Cryogenic drops to ambient decay after a 1-day thermal buffer.
- Consumption order prioritizes lowest freshness (FIFO on shelf life) unless player overrides.
- Zero UnityEngine or Godot dependencies in Core.
