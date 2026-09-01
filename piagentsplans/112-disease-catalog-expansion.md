# Plan 112 — Disease Catalog Expansion (7 → 20 diseases)

## Goal (2 lines)
Expand `disease_catalog.json` from 7 diseases to 20. The DiseaseSystem
(`DiseaseSystem.cs` confirmed live) models infection spread by vector, radius,
and interval, with countermeasure items blocking transmission. 7 diseases
cover only the starter set; the expanded world (contaminated rivers, dead
livestock, frozen camps, industrial ruins) needs diseases tied to the
locations and hazards the player actually encounters.

## Why (P2)
- Verified: `disease_catalog.json` has 7 diseases (cholera, zoonotic flu,
  blood fever, and 4 more). Each has id, display_name, vector (water/air/
  blood/...), lethality, incubation_days, illness_days, infectivity,
  spread_interval_days, spread_radius, countermeasure_item_id, guidance,
  source_note. `DiseaseCatalog.cs` loads it; `DiseaseSystem.cs` simulates
  spread.
- 7 diseases for a world with contaminated water, irradiated food, dead
  livestock, industrial chemicals, frozen camps, and crowded shelters is
  thin. The disease system is fully wired and save-supported but has too few
  pathogens to make hygiene and quarantine feel urgent.
- Pure DATA work — zero new Core code. The catalog loader and system consume
  the array directly.

## Files to touch
- `Assets/StreamingAssets/Data/disease_catalog.json` (expand `diseases` 7 → 20)
- Read-only: `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` (confirm loader
  and any enum constraint on `vector`)
- Read-only: `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` (confirm how
  countermeasure_item_id blocks spread)

## Content grammar (per disease)
- `id`: snake_case, prefix `disease_` (confirmed convention).
- `display_name`: clinical but human name ("Cholera", "Zoonotic Flu").
- `vector`: one of the existing vector strings (water, air, blood, food,
  contact, fecal_oral — confirm valid set in step 1).
- `lethality`: 0.0–1.0 untreated mortality probability.
- `incubation_days`: days before symptoms appear (1–7).
- `illness_days`: days of active illness (3–10).
- `infectivity`: 0.0–1.0 per-contact transmission probability.
- `spread_interval_days`: days between spread rolls (1–3).
- `spread_radius`: shelter tiles/rooms the disease can reach (2–5).
- `countermeasure_item_id`: an item id that blocks the vector (must resolve
  in the item catalog).
- `guidance`: 1–2 sentences of survivor-facing medical advice.
- `source_note`: 1–2 sentences of worldbuilding prose on how this disease
  arrived in the post-exchange world.

## Steps
1. Read `DiseaseCatalog.cs` to confirm the loader and whether `vector` is an
   enum or free string (determines which vectors are valid).
2. Read `DiseaseSystem.cs` to confirm how `countermeasure_item_id` blocks
   spread (possession? equipped? consumed?) and how `spread_radius` /
   `spread_interval_days` are consumed.
3. Inventory existing 7 diseases to confirm the quality bar and avoid
   duplicating vectors or countermeasures.
4. Author 13 new diseases tied to world content:
   - `disease_frost_pneumonia`: vector air, countermeasure warm_clothing,
     from frozen camps and blizzard corridors (Plan 48 weather gates).
   - `disease_radiation_sickness`: vector contact, countermeasure iodine,
     from fallout zones and contaminated salvage.
   - `disease_dysentery`: vector fecal_oral, countermeasure clean_water,
     from failed latrines in crowded shelters.
   - `disease_tetanus`: vector blood, countermeasure antibiotics, from
     rusted scrap and unexploded ordnance wounds.
   - `disease_typhus`: vector contact, countermeasure soap, from lice in
     unwashed refugee populations.
   - `disease_hepatitis`: vector blood, countermeasure clean_water, from
     shared needles in field clinics.
   - `disease_chemical_pneumonitis`: vector air, countermeasure gas_mask,
     from industrial chemical plants (Plan 116 deep lore locations).
   - `disease_lead_poisoning`: vector water, countermeasure water_filter,
     from contaminated reservoirs and old pipes.
   - `disease_fungal_pneumonia`: vector air, countermeasure gas_mask, from
     moldy underground bunkers and sealed shelters.
   - `disease_food_poisoning`: vector food, countermeasure clean_water,
     from spoiled rations and contaminated canned goods.
   - `disease_frostbite_infection`: vector blood, countermeasure
     antibiotics, from untreated frostbite in winter expeditions.
   - `disease_rabies`: vector blood, countermeasure antibiotics, from
     diseased wildlife in irradiated forests.
   - `disease_scurvy`: vector food, countermeasure vitamin_c_source, from
     long-term ration monotony in late campaign.
5. Each disease: distinct vector where possible, distinct countermeasure,
   lethality/infectivity balanced against existing values, guidance and
   source_note in the established clinical-yet-human tone.
6. Cross-reference: every `countermeasure_item_id` resolves in the item
   catalog; every `id` unique; no two diseases share the same vector +
   countermeasure pair.
7. Wire 4 diseases to Plan 116 (deep lore locations — specific locations are
  disease vectors).
8. Wire 3 diseases to Plan 48 (weather gates — weather conditions trigger
  disease outbreaks).
9. Wire 3 diseases to Plan 79 (autopsy procedures — new diseases need
  autopsy entries).
10. Validate: `--data-integrity-selftest` (all countermeasure_item_ids
    resolve).
11. xUnit: disease catalog loads 20 diseases, all ids unique, all
    countermeasure_item_ids resolve, all vectors valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `countermeasure_item_id` resolution (step 6):
every countermeasure must be a real item id or the integrity validator will
reject the catalog. Confirm the item catalog has warm_clothing, soap,
water_filter, vitamin_c_source before authoring, or use existing items.

## Definition of Done
- `disease_catalog.json` has 20 diseases, all ids unique, all
  countermeasure_item_ids resolving, all vectors valid, 4 wired to deep lore
  locations, 3 to weather gates, 3 to autopsy procedures, integrity + tests
  green.

## Follow-on
- Plan 116 (deep lore locations) — locations are disease vectors.
- Plan 48 (weather gates) — weather triggers outbreaks.
- Plan 79 (autopsy procedures) — new diseases need autopsy entries.
- Plan 81 (dose locations) — radiation sickness overlaps dose-ledger.
- Plan 90 (dose registers) — chronic disease bands parallel dose bands.
