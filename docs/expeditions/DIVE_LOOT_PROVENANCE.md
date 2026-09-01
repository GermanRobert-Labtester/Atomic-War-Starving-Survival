# Deep-Coast Dive Loot Provenance

**Document:** `docs/expeditions/DIVE_LOOT_PROVENANCE.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/dive_sites.json`, `items.json`
**Runtime System:** [`Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`](../../Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs)

---

## 1. Provenance & Economy Bounding

Every high-value dive reward originates from an authentic pre-war wreck context and feeds existing crafting, medical, or trade loops without creating unbounded farm sources:

| Salvage Category | Item References | Primary Dive Sources | Economic Purpose & Loop |
|---|---|---|---|
| **Technical Electronics & Ciphers** | `circuit_board`, `radio_vacuum_tube`, `cipher_cylinder` | `site_exp09_offshore_relay`, `site_exp09_sunken_submarine` | Unlocks advanced research blueprints and radio frequencies. |
| **Industrial Fuel & Fittings** | `fuel`, `scrap_metal`, `mechanical_parts` | `site_exp09_drowned_fuel_depot`, `site_exp09_submerged_siphon` | Maintains vehicle fleet and shelter water filtration machinery. |
| **Hermetic Medical Supplies** | `antibiotics`, `antiseptic`, `surgical_kit`, `iodine_pills` | `site_exp09_flooded_field_hospital` | Treats chronic infections and radiation trauma without infinite pharma farming. |
| **Naval & Marine Military Gear** | `ammo_556`, `ammo_762x54r`, `weapon_service_rifle` | `site_exp09_submerged_convoy`, `site_exp09_naval_patrol`, `site_exp09_wrecked_patrol_craft` | High-risk retrieval of pristine cartridges and military-spec components. |
| **Historical & Faction Relics** | `logbook_fragment`, `flotilla_insignia`, `prewar_manifest` | `site_exp09_ss_sovereign`, `site_exp09_ferry_terminal` | Resolves narrative quests and advances standing with coastal factions. |
