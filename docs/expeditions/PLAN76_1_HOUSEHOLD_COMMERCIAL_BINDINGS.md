# Plan 76.1 — Household / Commercial Family Scavenging-Table Bindings

Third family of the 42-destination `lootCategories`→Plan 46 table migration.

## Bindings (21 → 26 of 53)

| Destination | Binding | Rationale |
|---|---|---|
| `loc_department_store` | `table_loot_shopping_center` (existing) | Vansen's — six floors picked over, only fittings left. Broad-commercial ecology is the shopping-center table exactly. |
| `loc_motel_verity` | `table_loot_apartment_block` (existing) | Twelve rented units on neutral ground: room-by-room left-behind household loot. (Warlord neutrality fee is runtime economy, not loot.) |
| `family_bunker_backyard_shed` | `table_loot_apartment_block` (existing) | A family cache stocked "with care and dread": tinned food, radio, tape recorder, letter. The apartment table's document set (family photograph, last letter, child's drawing, father's tapes) *is* this destination's identity. |
| `concert_hall_ruins` | **`table_loot_concert_hall`** (new) | Venue identity — drapery cloth, stage timber, programs, stage-light batteries, record library, and the meeting-place leavings. No existing table covers it. |
| `loc_ration_queue_plaza` | **`table_loot_ration_plaza`** (new) | Civic distribution point — queue lines, dropped rations, crowd-leavings, shelter-rejection notices. No existing table covers it. |

No new item ids invented (Plan 76 §1.10). Codex refs reuse existing ids only
(`codex_personal_journal`, `codex_ration_theft`).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_concert_hall` | 7 | 133 | 90% | none 0.00 | cloth/screws-and-timber salvage, books, rare vinyl collection |
| `table_loot_ration_plaza` | 8 | 138 | 94% | none 0.00 | dropped rations, queue leavings, civic paperwork |

Balance notes (§53): both tables are lean, low-danger d2/d4 sites — broad but
shallow yield, matching their tier. The rare document entries
(`item_vinyl_collection`, `item_document_ration_record`,
`item_document_shelter_rejection_list`) carry the sites' narrative identity
without inflating material value. Distinctiveness (§52): venue culture-goods
≠ civic distribution ≠ household rooms ≠ broad retail.

## Progression

11 original + 5 medical + 5 mechanical/fuel + 5 household/commercial =
**26 / 53** bound. Remaining unbound: **27** — administrative/knowledge,
military/ammunition, electrical/communications, agricultural remainder,
water/chemical, settlements, deep/endgame families.
