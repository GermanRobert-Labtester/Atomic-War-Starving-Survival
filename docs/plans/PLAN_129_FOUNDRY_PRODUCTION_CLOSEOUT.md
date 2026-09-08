# Plan 129 Foundry Production Closeout

## Outcome

Plan 129 is complete against the reconciled live baseline:

- baseline: 26 products already present in `foundry_production.json`;
- additions: 9 validated products;
- final count: exactly 35;
- runtime: unchanged, all products use `SilentFoundrySystem`'s existing heat,
  charge, quality, inventory, save, and treaty paths.

The original 11 → 20 statement was stale repository context. No existing
product was removed or rewritten.

## Runtime contract

`FoundryProductEntry` remains the sole production DTO. Each product supplies
`product_id`, `display_name`, `category`, `result_item_id`, `result_amount`,
`ingredients`, labor, cast, fuel, water, skill, quality, treaty/quota, sink,
notes, and tags. `StartProduction` validates and consumes charge materials,
coal/charcoal, and clean water before saving `activeProductId`. Completion
adds the result through the bound inventory callback and applies any authored
treaty quota. No new production system or save field was introduced.

## Added-product parity matrix

| Product | Canonical output | Owner / consumer | Loot |
|---|---|---|---|
| `foundry_prod_bronze_datum_plate` | `item_datum_plate_bronze` | Geodetic survey monuments | — |
| `foundry_prod_flywheel_rotor_shaft` | `item_forged_rotor_shaft` | Kinetic storage flywheels | Eastern Power Substation |
| `foundry_prod_flywheel_containment_ring` | `item_containment_ring_steel` | Kinetic storage containment | — |
| `foundry_prod_culvert_brace` | `item_high_tensile_steel_culvert_brace` | Access and culvert repair | Existing steelworks loot |
| `foundry_prod_sealed_lead_pig` | `item_sealed_lead_pig` | Radiation/source containment | — |
| `foundry_prod_ground_anchor_spikes` | `item_hardened_ground_anchor_spikes` | Barrier anchoring | — |
| `foundry_prod_turbine_blade_blank` | `item_superalloy_turbine_blade_blank` | EB-PVD turbine coating | Riverside Steelworks |
| `foundry_prod_rail_grinding_head` | `item_rail_grinding_head` | Railway rail reprofiling | — |
| `foundry_prod_press_tooling_set` | `item_press_tooling_set` | Tablet press maintenance | Riverside Steelworks |

All nine outputs and every ingredient resolve through `items.json` or
`foundry_items.json`. The nine IDs are unique and do not collide with the
separate B66 metallurgy recipe IDs or the glassworks catalog.

## Category and sink vocabulary

The existing loader intentionally treats `category` and `sink` as authored
strings, not closed runtime enums. Plan 129 adds only domain-specific
vocabulary:

- categories: `survey_datum`, `rotor_shaft`, `containment_ring`,
  `structural_brace`, `radiation_container`, `defense_anchor`,
  `turbine_blank`, `rail_tooling`, `press_tooling`;
- sinks: `survey`, `kinetic_storage`, `infrastructure`, `containment`,
  `defense`, `power`, `railway`, `medical_manufacturing`.

These labels are descriptive routing metadata. They do not create a second
consumer or economy authority.

## Treaty and economy integration

The existing treaty products remain authoritative:

- `treaty_road_iron_charter`: ice anchors and winch drums;
- `treaty_brine_pipe_and_iodine_exchange`: brine-resistant pipe;
- all other treaty rows continue to resolve from `foundry_accords.json`.

No new quota was attached to Plan 129 additions. The current runtime creates
compliance rows for the four authored obligations only; adding a quota for a
treaty outside that runtime set would pass a foreign-key check but never be
assessed. This is intentionally documented rather than hidden.

The two new Plan 116 references are in existing industrial loot tables:
`item_superalloy_turbine_blade_blank` at Riverside Steelworks and
`item_forged_rotor_shaft` at Eastern Power Substation. Existing salvage
remains available; Foundry production is the repeatable industrial route.

## Plan 55 crafting audit

The original candidate outputs (`water_filter`, gas-mask filter, battery, and
surgical kit) already have general crafting or assembly authorities in
`recipes.json`. Reusing those outputs in Foundry would duplicate or blur
ownership. No recipe was changed. The current recipe authority remains the
consumer for those complete goods, while Plan 129 supplies distinct
industrial components to their real owner systems.

## Verification and regression matrix

| Check | Result |
|---|---|
| JSON parse, product count, product ID uniqueness | PASS, 35 / 35 |
| Focused Plan 129 and expansion tests | PASS, 39 tests |
| Result/ingredient/treaty foreign-key integration test | PASS |
| Plan 116 steelworks loot references | PASS |
| Full data-integrity, Core suite, host build, headless demo | Run in final verification phase |

## Implementation log

### Phase 1 — Authority reconciliation

Status: PASS

- Reclassified the live 26-product catalog as the accepted baseline.
- Rejected stale candidate IDs and complete-goods duplicates.
- Mapped nine outputs to existing canonical industrial item owners.

### Phase 2 — Additive data and integration

Status: PASS

- Appended nine products only.
- Added two steelworks loot references.
- Preserved concurrent recipe and location content.

### Phase 3 — Regression coverage

Status: PASS

- Pinned exact cardinality at 35.
- Pinned all nine result IDs and their canonical item resolution.
- Pinned the two Plan 116 loot entries.
