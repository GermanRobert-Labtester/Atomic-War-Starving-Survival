# Crossing Faction Wants & Offers Semantics

## 1. Design & Interpretation Philosophy

In `crossing_factions.json`, the `wants` and `offers` fields represent **macro-economic commodity and institutional service profiles** rather than direct single-item trade inventories. They define:
- What resources the faction requires from the settlement to sustain its operations (`wants`).
- What civic infrastructure, security guarantees, or commercial access the faction grants in return (`offers`).

---

## 2. Wants Matrix

| Faction | Primary Want | Secondary Want | Economic Rationale |
|---|---|---|---|
| The Scale | `trade_goods` | — | Broad commercial salvage to weigh and tax at the deck scale. |
| The Underwrite | `pledged_goods` | — | Collateral contracts, bonded items, and debt pledges. |
| The Compact | `signatories` | — | Citizen endorsements and charter affirmations for assembly quorum. |
| The Lamplighters | `fuel_stores` | `reflector_glass` | Fuel to burn in public lanterns and curved glass to focus night light. |
| The Granary Wardens | `staple_grain` | `burlap_sacks` | Caloric reserve stock and clean dry sacking for grain storage. |
| The Water Committee | `filter_media` | `sanitation_salts` | Activated charcoal and chemical salts for sand bed filtration. |
| The Quarantine Post | `medical_tinctures` | `respirator_masks` | Antiseptics for triage and particulate masks for pulmonary protection. |
| The Smugglers' Court | `unchartered_salvage` | `route_intelligence` | Off-book high-value goods and unmapped route information. |

---

## 3. Offers Matrix

| Faction | Primary Offer | Secondary Offer | Tertiary Offer |
|---|---|---|---|
| The Scale | `stallrow_trade_access` | `verification` | — |
| The Underwrite | `seed_stock` | `covered_loss` | `favour_bank` |
| The Compact | `charter_draft` | `ratification` | — |
| The Lamplighters | `street_lighting` | `night_watch_escort` | `route_visibility` |
| The Granary Wardens | `ration_distribution` | `emergency_grain_draw` | `spoilage_inspection` |
| The Water Committee | `clean_water_rights` | `well_head_access` | `quality_certification` |
| The Quarantine Post | `gate_health_screening` | `quarantine_clearance` | `outbreak_warning` |
| The Smugglers' Court | `off_ledger_trade` | `culvert_transit` | `discreet_arbitration` |
