# Installation Loot Provenance (Plan 85)

## Semantics classification (§85D.5)

`revealed_items` is implemented as the installation expedition destination's **`lootCategories`** — the guaranteed-eligible signature salvage surfaced by the standard expedition loot loop. It is:

- **not** a direct grant on map completion (§0.8, §1.6 — the treasure is never free);
- **not** a preview-only list (the items genuinely drop at the site);
- **not** a unique one-time cache (see below).

## How site loot resolves

During the sortie's Looting phase, each tick rolls `PerformLootRoll` (chance `0.5 + danger×0.05`, seeded `ISeededRng`):

1. The destination's `scavenging_table_id` (a themed Plan 46 table) is rolled for ambient finds.
2. If no table roll resolves, `PickLootCategory` draws from `lootCategories` — the `revealed_items` — so signature items are **guaranteed-eligible** across a sortie without being deterministic handouts.
3. Finds obey the existing bounds: per-item weight 1 kg, `maxLootCapacityKg`, stamina drain, encounter risk, and the return-home inventory unload.

## Why there are no unique one-time caches in v1

A fixed unique cache requires a persistent claimed/depleted container state at the site. The repository's loot authority (expedition loot rolls + Plan 46 tables) is stateless per site — depletion_model fields exist in table data but no runtime tracks site-depletion state today. Building persistent container-claim state would be a new loot system, which Plan 85 explicitly forbids (§14).

Therefore every installation's reward set is composed **exclusively of pre-existing, multi-producer catalog items** (verified against `items.json` by test). Consequences, by construction:

- no revisit/reload duplication is possible — the items are ordinary trade goods elsewhere in the economy;
- no reroll exploit exists — rolls are seeded (`SeededRng`) and site loot is not state;
- economy bounds: quantities per sortie are small (capacity- and roll-limited); no installation grants ammunition, medicine jackpots, or free-fuel loops beyond single `fuel_canister`/`diesel_fuel` tier items.

If a later plan wants true unique caches (e.g. `faraday_pack`-class singles), the sanctioned path is extending the existing location/loot save state with a claimed-flags section — not Plan 85 scope.

## Per-installation reward roles

| Installation | revealed_items (= lootCategories) | Role | Value bound |
|---|---|---|---|
| Underground Fuel Depot | mechanical_parts, diesel_fuel | repair + fuel top-up | low, trade-backed |
| Municipal Seed Vault | seed_packets, family_heirloom_seeds, growing_manual | agriculture progression | low-medium, multi-producer |
| Blacksite Armory 7 | faraday_pack, military_radio, night_vision_scope, ammo_308 | comms/electronics + bounded ammo (pre-existing zone) | medium; single ammo line |
| Collapsed Command Vault | military_radio, military_rations, ammo_308 | comms + rations (pre-existing zone) | low-medium |
| Dead-Drop Command Shelter | diesel_fuel, water_filter, scrap_metal | utility | low |
| Hidden Relay Bunker 09 | item_comm_codebook_alpha, electronic_scrap, medical_kit | comms knowledge | low-medium |
| Sealed Triage Annex | surgical_suture, antiseptic_1l_of_1l, protective_rubber_gloves, splint, painkillers | medical consumables (no antibiotics — no rare-medicine flooding) | medium, spread over 5 common items |
| Evidence Sub-Basement | typewriter_ribbon, sealed_government_document, item_collectible_local_newspaper | documents/records | low; information-tier |
| Quarantine Barn | item_seed_hardy_tuber, item_seed_cold_legume, rope, mechanic_gloves | agriculture + tools | low |
| Forestry Emergency Store | fuel_canister, rope, item_insulated_boots | field kit | low |
| Materials Research Sublevel | machinist_caliper, item_reagent_clean, copper_wire_10m_of_10m | research/precision | medium |
| Electrical Maintenance Exchange | battery_pack, item_battery_reconditioned, copper_wire_10m_of_10m | shelter power | medium |

**New item definitions added: zero.** Every reward resolves in the existing catalog (test-enforced).
