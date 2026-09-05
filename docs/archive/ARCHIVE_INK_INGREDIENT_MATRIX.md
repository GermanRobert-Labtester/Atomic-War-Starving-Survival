# Archive Ink Ingredient Matrix

> **Ingredient Provenance:** Verification of all 9 referenced items in `items.json`, acquisition channels, and stack economics.

---

## 1. Ingredient Resolution & Sources

| Required Item ID | Item Category | Inks Consuming | Acquisition Sources | Scarcity Tier |
|---|---|:---:|---|---|
| `charcoal` | Fuel / Resource | 4 | Wood burner crafting, fire pit salvage, stove cleanings | Common |
| `cloth` | Resource / Material| 1 | Rummaging, wardrobe salvage, textile workshops | Common |
| `scrap_metal` | Material | 1 | Everywhere in wasteland, machinery dismantling | Very Common |
| `berries` | Food / Consumable | 1 | Greenhouse foraging, surface brambles, trade | Common |
| `mineral_chunk` | Material | 1 | Quarry, tunnel rubble, cave expeditions | Common |
| `organic_residue` | Material | 1 | Bioluminescent flora, filter sludge, compost | Uncommon |
| `chemical_solvent`| Chemical / Solvent | 1 | Laboratories, paint lockers, pharmacy ruins | Uncommon |
| `empty_toner_cartridge`| Salvage / Office | 1 | Administrative bunkers, school offices, print shops | Uncommon / Rare |
| `blood_sample` | Medical | 1 | Clinic phlebotomy, surgery byproduct, trauma salvage | Uncommon / Emergency |

---

## 2. Invariant Verification

- Every `required_item_id` resolves 100% cleanly against `items.json`.
- Zero orphan ingredients: all 9 items have defined drop tables, crafting paths, or container origins.
- Required quantities stay conservative (1 to 3 items), preventing inventory congestion while enforcing real material opportunity costs.
