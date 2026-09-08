# Phantom Trigger Category Inventory

## Runtime vocabulary

The category authority is `PhantomMemoryEngine.GetCategoryFromId`. It
classifies item IDs by the existing ordered substring matcher and returns
`generic` for any non-empty item ID that has no earlier match.

| Category | Approx. `items.json` definitions | Existing examples |
|---|---:|---|
| `childhood` | 8 | `childs_mitten`, `childs_drawing`, `childrens_books` |
| `photograph` | 9 | `family_photograph`, `photo_album`, `item_pre_war_photo_album` |
| `correspondence` | 11 | `farm_ledger`, `pocket_notebook`, `item_document_radio_log` |
| `personal_item` | 22 | `family_apartment_key`, `engraved_lighter`, `wedding_ring` |
| `military` | 13 | `dog_tags`, `item_dog_tags_scavenged`, `tarnished_medal` |
| `medical` | 14 | `field_dressing_kit`, `medical_kit`, `stethoscope` |
| `work_tool` | 10 | `miners_tag`, `radio_headset`, `item_theodolite_brass_precision` |
| `ordinary_object` | 9 | `recipe_tin`, `bus_ticket`, `enamel_mug` |
| `generic` | 563 | all other item IDs, including exact-ID authored objects |

Counts are an audit of the current `items.json` ID corpus using the live
matcher, not a second category authority.

## Plan 111 usage

Every Plan 111 trigger uses one of the nine live categories and references an
existing `items.json` ID. Several profession-specific objects classify as
`generic`; this is intentional because `MatchesRule` accepts an exact
`item_id` match even when the broad inferred category is generic.

No speculative category entered the catalog. The following plan-draft terms
were not authored as category values:

```text
seed, rope, fish, water, vehicle_part, fuel, map, religious_object,
bandage, utensil, radio, battery, rock, lamp, book, furniture, chemical
```

Where those concepts were useful, the implementation uses an existing
runtime category plus an exact item ID, for example:

- `fuel_canister` under `generic`;
- `radio_headset` under `work_tool`;
- `item_signal_lamp_module` under `generic`;
- `item_collectible_prayer_beads` under `generic`;
- `item_collectible_topo_map` under `correspondence`.

This preserves the current matcher contract and gives every authored trigger a
real item source.
