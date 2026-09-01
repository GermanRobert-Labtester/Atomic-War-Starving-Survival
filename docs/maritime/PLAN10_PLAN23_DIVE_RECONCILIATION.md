# Plan 10 ↔ Plan 23 Dive-Site Reconciliation

Verified against working tree (live truth) and `git show HEAD` (committed truth), 2026-08.

## 1. Lived inventory (12 sites, working tree)

All sites use the schema-v2 grammar only: `site_id`, `name`, `oxygen_budget_ticks`,
`base_noise_floor`, `keeper_thread_id` (site 1 only), `rooms[4]{room_type, hazard_level,
search_difficulty}`. No safe, loot, contamination, tide, current, location, or gear fields
exist in the schema — every mechanic below is exercised through the runtime, not the file.

| # | site_id | Concept | Plan 10 concept | Air | Noise floor | Mechanic notes |
|---|---|---|---|---|---|---|
| 1 | `site_exp09_ss_sovereign` | S.S. Sovereign Wreck (steamer) | Plan 09/10 | 120 | 0.50 | Keeper thread `q_keeper_of_logs`; Sovereign hold choice (flood/burn iodine) via `DiveInstanceRunner` |
| 2 | `site_exp09_ferry_terminal` | Drowned Ferry Terminal | Plan 10 | 90 | 0.60 | Structure, civilian |
| 3 | `site_exp09_barge_flotilla` | Barge Flotilla mooring | Plan 09 | 100 | 0.40 | Flotilla home water, low noise floor |
| 4 | `site_exp09_naval_patrol` | Patrol craft | Plan 09/10 | 80 | 0.70 | Deep-coast dock anchor site |
| 5 | `site_exp09_sunken_submarine` | Half-Submerged Barrik (submarine) | Plan 10 | 70 | 0.80 | Tightest air, highest floor — deep/hazard |
| 6 | `site_exp09_flooded_metro` | Floored Metro Concourse | Plan 10 | 110 | 0.45 | Flooded transit structure |
| 7 | `site_exp09_submerged_convoy` | Submerged Convoy (three trucks) | Plan 10 | 95 | 0.55 | Cargo/cargo-hulk analogue |
| 8 | `site_exp09_drowned_fuel_depot` | Drowned Fuel Depot | Plan 10 | 85 | 0.65 | Industrial |
| 9 | `site_exp09_offshore_relay` | Offshore Radio Relay Station | Plan 10 | 75 | 0.70 | Relay/structure |
| 10 | `site_exp09_flooded_field_hospital` | Flooded Quarantine Barge | Plan 10 | 90 | 0.50 | Medical/quarantine |
| 11 | `site_exp09_wrecked_patrol_craft` | Wrecked Flotilla Picket Craft | Plan 10 | 85 | 0.65 | Flotilla provenance |
| 12 | `site_exp09_submerged_siphon` | Submerged Siphon & Pump Station | Plan 10 | 105 | 0.40 | Industrial intake |

Plan 10 concepts from the source plan that are already represented: sunken submarine (1),
flooded metro (2), submerged convoy (3), drowned fuel depot (4), collapsed ferry terminal (2),
relay station (5), field hospital/quarantine barge (6), patrol-craft battlefield (4, 11).
**No Plan 23 site may duplicate any of these concepts.**

## 2. Verdict — Case C

Working tree has 12 sites; git HEAD has 4 (Plan 10 delta is uncommitted in-flight work).
Target semantics: **14 total live sites** ⇒ Plan 23 authors exactly **2 new sites**, chosen to
fill archetype gaps, and deepens existing sites with the missing mechanic consumers.

### Chosen additions (no archetype duplication)

| New site | Archetype | Why it is not a duplicate | Primary mechanics |
|---|---|---|---|
| `site_exp23_payroll_strongroom` | Safe-heavy: drowned harbor-master payroll strongroom | No bank/vault/payroll concept exists; first true `SafeCrackingSystem` consumer in the catalog | Safe runtime, noise budget, bounded currency/ledger loot |
| `site_exp23_brine_cistern` | Deep/contamination: sealed chemically-poisoned cistern gallery under the cape | No contamination-heavy site exists; `PsychologicalContaminationSystem` has zero maritime consumers | Contamination mapping, abort-before-max decision, gear gate |

### Deepening (no new IDs)

- All 12 existing sites gain their missing **location anchor + discovery path + loot provenance**
  via the shared dive-mechanic extension (see `DIVE_MECHANIC_COVERAGE.md`).
- Sites 4 (`drowned_fuel_depot`) and 8-adjacent sites gain Flotilla faction hooks; the
  picket craft (`site_exp09_wrecked_patrol_craft`) becomes the Flotilla war-grave/memorial site.

## 3. Duplicate-resolution rule applied

- Preserve already-live stable IDs (`site_exp09_*`); enrich, never replace.
- No renames: 4 sites are save/content contracts (committed); 8 more are the in-flight
  Plan 10 contract — Plan 23 preserves their IDs, rooms, air, and noise values verbatim.
- Loot, safes, contamination, tides, and faction hooks are added **around** existing site
  definitions, not by editing their tuned profiles.
