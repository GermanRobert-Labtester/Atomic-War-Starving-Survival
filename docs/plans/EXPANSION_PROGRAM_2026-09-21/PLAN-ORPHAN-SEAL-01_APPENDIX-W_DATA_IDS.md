# PLAN-ORPHAN-SEAL-01 — Appendix W: Data-Id Reference Audit

**Generated:** 2026-09-21. String literals in orphan sources that look like
catalog ids (`item_`, `loc_`, `faction_`, `quest_`, `recipe_`, …) checked
against **4750 ids found anywhere under `Data/` (recursive)**.
Appendix 34 mapped the id families; this appendix shows which orphans reference
them directly and whether the literal resolves.
**Important:** an unresolved literal is not automatically stale — ids are often
assembled at runtime (prefix + variable) or defined in a catalog the literal
does not name. The package classifies each row before wiring.

### `CommunicationsSystem`

| Id literal | Resolves |
|---|:---:|
| `faction_garrison` | yes |

### `ShelterFestivalEngine`

| Id literal | Resolves |
|---|:---:|
| `item_fuel` | yes |
| `item_rations` | **no** |

### `TerritoryControlSystem`

| Id literal | Resolves |
|---|:---:|
| `faction_territory` | **no** |

### `OilseedPressingEngine`

| Id literal | Resolves |
|---|:---:|
| `item_fat_confit` | yes |
| `item_preservation_salt` | yes |

### `ClothingWarmthSystem`

| Id literal | Resolves |
|---|:---:|
| `item_arctic_survival_suit` | **no** |
| `item_fur_boots` | **no** |
| `item_hazmat_cold_suit` | **no** |
| `item_ragged_coat` | **no** |
| `item_thermal_balaclava` | **no** |
| `item_thermal_underwear` | **no** |
| `item_winter_coat` | **no** |
| `item_wool_scarf` | **no** |

### `CampaignLegacySystem`

| Id literal | Resolves |
|---|:---:|
| `faction_standing` | yes |

### `SurvivorLetterDeliverySystem`

| Id literal | Resolves |
|---|:---:|
| `survivor_letter_delivery` | **no** |

### `SessionDurabilityManager`

| Id literal | Resolves |
|---|:---:|
| `survivor_count` | yes |

### `OutpostSettlementSystem`

| Id literal | Resolves |
|---|:---:|
| `radio_relay_range` | yes |

### `CupolaFoundryEngine`

| Id literal | Resolves |
|---|:---:|
| `trait_foundry_master` | **no** |
| `trait_patternmaker` | **no** |

### `TrophySystem`

| Id literal | Resolves |
|---|:---:|
| `item_decor_trophy_ash_hound_pelt` | yes |
| `item_decor_trophy_beetle_carapace` | yes |
| `item_decor_trophy_boar_tusks` | yes |
| `item_decor_trophy_crow_feathers` | yes |
| `item_decor_trophy_deer_antlers` | yes |
| `item_decor_trophy_fox_pelt` | yes |
| `item_decor_trophy_gulden_wolf` | yes |
| `item_decor_trophy_kestrel_wings` | yes |
| `item_decor_trophy_molerat_skull` | yes |
| `item_decor_trophy_pheasant_plume` | yes |

### `BackstorySystem`

| Id literal | Resolves |
|---|:---:|
| `trait_modifier` | yes |

### `SurvivorRoleSystem`

| Id literal | Resolves |
|---|:---:|
| `survivor_id_required` | **no** |

### `SurvivorVoiceSystem`

| Id literal | Resolves |
|---|:---:|
| `survivor_last_uttered_day` | **no** |

