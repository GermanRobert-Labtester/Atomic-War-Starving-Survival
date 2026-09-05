# Autopsy Tool & Consumable Inventory

> **Supply Authority:** `Assets/StreamingAssets/Data/items.json` and `Assets/StreamingAssets/Data/autopsy_procedures.json`.

---

## 1. Inventory Alignment

All tools and consumables specified across the 12 autopsy procedures resolve directly against canonical item definitions in `items.json`.

### Reusable Tools
| Item ID | Display Name | Stack Max | Weight | Category / Type | Durability | Acquisition Source |
|---|---|:---:|:---:|---|:---:|---|
| `medical_scissors` | Medical Scissors | 5 | 0.20 kg | Medical / Surgical | 80 | Clinic salvage, crafting, triage kit |
| `protective_rubber_gloves`| Protective Rubber Gloves | 5 | 0.20 kg | Medical / PPE | 40 | Lab salvage, hazmat lockers |
| `field_surgical_kit` | Field Surgical Kit | 5 | 1.00 kg | Medical / Surgical | 0 (Fixed) | Hospital ruins, military ambulance |
| `surgical_mask` | Surgical Mask | 10 | 0.05 kg | Component / PPE | 0 (Fixed) | Clinic drawers, emergency caches |
| `scalpel` | Scalpel | 5 | 0.10 kg | Component / Tool | 0 (Fixed) | Clinic blister packs, surgical trays |
| `forceps` | Forceps | 3 | 0.20 kg | Component / Tool | 0 (Fixed) | Medical lockers, field surgeon kit |

### Consumable Supplies
| Item ID | Display Name | Stack Max | Weight | Health Effect | Acquisition Source |
|---|---|:---:|:---:|:---:|---|
| `bandage` | Bandage | 10 | 0.10 kg | +15 HP | Cloth crafting, scavenged first aid |
| `sterilised_bandage` | Sterilised Bandage | 10 | 0.10 kg | +25 HP | Medkit salvage, pharmaceutical sealed stock |
| `clean_water` | Clean Water | 10 | 0.50 kg | +15 Thirst | Water treatment plant, filtration still |
| `antibiotics` | Antibiotics | 10 | 0.05 kg | Infection Cure | Pharmacy ruins, clinic safes, rare trade |

---

## 2. Item Discipline & Preservation Principles

1. **No Portable Workstation Fiction:** Autopsies do not model an "autopsy table" as an inventory item. Autopsies are performed using hand tools and sterile prep at the clinic/infirmary station.
2. **Consumable Prudence:** Antibiotics are strictly reserved for high-containment biohazard dissections (`procedure_containment_autopsy` and `procedure_spore_infection_isolation`) where active chemical sterilization of tissue samples is mandatory to prevent room contamination. Routine examinations consume only standard clean water, bandages, and sterilised dressings.
