# Plan 92 — Faction War Dialogue Expansion Completion Report

> **Mission:** Expand `faction_war_dialogue.json` from 18 verified snippets to 40 without modifying the runtime contract or inventing location IDs.
> **Status:** **COMPLETE** (100% Verified)

---

## 1. Baseline

- **Starting catalog count:** 18 snippets
- **Starting revision/commit:** `d37406a765af964c4ee8176ccee9cc8413cd5389`
- **Existing selector semantics:** `FactionWarContentCatalog.GetDialogueForLocation(string locationId, int day)` evaluates `s.locationId == locationId` and `s.minDay <= day` in a stateless loop.
- **Actual campaign horizon:** Days 480 to 605+ (the Year of Ash Faction War Arc, as documented in `NARRATIVE_NEEDS.md` and evidenced by existing event chains, broadcasts, and journals).

---

## 2. Final Catalog Summary

- **Final entry count:** **40 snippets**
- **Number added:** **22 snippets**
- **Existing entries modified:** **0** (all 18 original IDs, locations, tags, and bodies were preserved verbatim).

---

## 3. New Content by Context

The 22 additions satisfy the requested faction context distribution:
- **Garrison (5):**
  1. `dlg_d486_garrison_crate_seal` (Day 486, `loc_garrison_motor_pool`) — Missing clips and cut seals on ammunition crates.
  2. `dlg_d494_garrison_boot_leather` (Day 494, `loc_snowline_station`) — Tallow for breech blocks vs mess tin lard for frozen boots.
  3. `dlg_d516_garrison_kerosene_stove` (Day 516, `loc_garrison_checkpoint_gamma`) — Command staff heater smoke vs freezing sentries.
  4. `dlg_d542_garrison_sick_list_billet` (Day 542, `loc_conscription_office`) — Conscript with rattling lungs assigned to culvert guard watch.
  5. `dlg_d562_garrison_fuel_drum_tare` (Day 562, `loc_garrison_motor_pool`) — Sludge in diesel drum dipsticks and cold fuel lines.
- **Exchange (4):**
  1. `dlg_d485_exchange_wet_grain_scale` (Day 485, `loc_grain_silo`) — Deducting transit tare for wet grain corners on the brass scale.
  2. `dlg_d508_exchange_axle_grease_delay` (Day 508, `loc_weighbridge`) — Axle grease freezing in wheel hubs, delaying the supply caravan.
  3. `dlg_d530_exchange_stamped_chits` (Day 530, `loc_supply_corps_highway_redoubt`) — Garrison changing stamp ink colors, invalidating old transit passes.
  4. `dlg_d489_exchange_drum_bung_dispute` (Day 489, `loc_water_station`) — Leaking drum bung dripping grey water into clean intake troughs.
- **Understory (4):**
  1. `dlg_d492_understory_porcelain_insulator` (Day 492, `loc_understory_transmitter`) — Hairline crack in ceramic insulator bell and transmitter wattage limits.
  2. `dlg_d518_understory_log_overrun` (Day 518, `loc_radio_relay_mast`) — Casualty roster reading overrunning the scheduled transmission window.
  3. `dlg_d546_understory_smudged_pad_entry` (Day 546, `loc_sub_level_maintenance_shaft_9`) — Condensation-smudged one-time pad digits (1 vs 7).
  4. `dlg_d576_understory_copper_splice_tale` (Day 576, `loc_continental_radio_beacon`) — Three wraps, solder, and pitch on high-mast wire joints.
- **Independent (3):**
  1. `dlg_d498_independent_chalk_boundary` (Day 498, `loc_sector_4_rail_switchyard`) — Disputed chalk claim mark on a derailed freight car axle.
  2. `dlg_d534_independent_tripwire_slack` (Day 534, `loc_forward_roster_camp`) — Adjusting slack tin-can warning lines with frozen stones.
  3. `dlg_d566_independent_blanket_tally` (Day 566, `loc_mountain_tunnel_refuge`) — Four blankets for six cots in a winter bus shelter.
- **Foundry (3):**
  1. `dlg_d502_foundry_cracked_flask_sand` (Day 502, `loc_granite_arsenal_foundry`) — Venting greensand molds to prevent steam scabs during bronze pours.
  2. `dlg_d528_foundry_crucible_heat_window` (Day 528, `loc_granite_arsenal_foundry`) — Crucible pot cooling while pour team lines ingot troughs with loam.
  3. `dlg_d556_foundry_slag_billet_reject` (Day 556, `loc_rebuilder_brickworks_kiln`) — Scrap rebar flagged for sulfur inclusion repurposed for ditch stakes.
- **Civilian (3):**
  1. `dlg_d487_civilian_parsnip_stew_scrap` (Day 487, `loc_the_allotments`) — Greenhouse workers sharing a heel of bread and roasted turnip.
  2. `dlg_d520_civilian_valve_handle_toy` (Day 520, `loc_ration_queue_plaza`) — Mother mending gloves while child spins an old dry radiator valve wheel.
  3. `dlg_d574_civilian_kettle_scouring_mutter` (Day 574, `loc_second_winter_homestead`) — Solitary elder scouring kettle, reciting stove flue settings.

---

## 4. Location Coverage

All 22 new snippets reference existing canonical `loc_*` IDs from `locations.json` or `year_of_ash_locations.json`:
- `loc_garrison_motor_pool` (2 new snippets)
- `loc_snowline_station` (1 new snippet)
- `loc_garrison_checkpoint_gamma` (1 new snippet)
- `loc_conscription_office` (1 new snippet)
- `loc_grain_silo` (1 new snippet)
- `loc_weighbridge` (1 new snippet)
- `loc_supply_corps_highway_redoubt` (1 new snippet)
- `loc_water_station` (1 new snippet)
- `loc_understory_transmitter` (1 new snippet)
- `loc_radio_relay_mast` (1 new snippet)
- `loc_sub_level_maintenance_shaft_9` (1 new snippet)
- `loc_continental_radio_beacon` (1 new snippet)
- `loc_sector_4_rail_switchyard` (1 new snippet)
- `loc_forward_roster_camp` (1 new snippet)
- `loc_mountain_tunnel_refuge` (1 new snippet)
- `loc_granite_arsenal_foundry` (2 new snippets)
- `loc_rebuilder_brickworks_kiln` (1 new snippet)
- `loc_the_allotments` (1 new snippet)
- `loc_ration_queue_plaza` (1 new snippet)
- `loc_second_winter_homestead` (1 new snippet)

---

## 5. Temporal Coverage & Reachability

- **Earliest new minDay:** Day 485 (`dlg_d485_exchange_wet_grain_scale`)
- **Latest new minDay:** Day 576 (`dlg_d576_understory_copper_splice_tale`)
- **Unreachable entries:** **0** (well within the Day 480–605 narrative window).
- **Eligible pool expansion:** Monotonically increases across campaign progression (3 eligible at Day 485, 13 at Day 500, 20 at Day 525, 29 at Day 550, 36 at Day 575, 40 at Day 600+).

---

## 6. Repetition & Selector Findings

- **Seen-state:** None in Core. The selector is stateless.
- **Cooldowns / Weighting:** None in Core.
- **Deterministic RNG:** Selection remains strictly order-preserving and deterministic.
- **Residual Repetition Risk:** Because the selector is stateless, snippets were authored under the **Evergreen-After-Onset Rule**, avoiding transient headlines or specific dates.

---

## 7. Cross-Plan Reconciliation

- **Plan 73 (Faction Radio):** Overheard dialogue represents internal, sideways speech (shortages, grease, cracked molds) contrasting with outward public radio broadcasts.
- **Plan 44 (Faction Territory):** Snippets avoid brittle territorial claims ("we will hold this forever"), focusing on physical operations that remain plausible under contested control.
- **Plan 52 (Recurring NPCs):** Anonymous occupational roles were used exclusively (`speakerTag`), avoiding simulated named continuity where no runtime speaker state exists.
- **Plan 84 (Muster Witnesses):** Ambient dialogue foreshadows conditions without replacing formal evidentiary testimony.
- **Plan 25 (Faction Ecology):** Reused established faction terminology (The Tally, D/9, Harven's decrees, Sella Krenn, etc.).

---

## 8. Verification Results

| Suite | Command | Exit Code | Result |
|---|---|---|---|
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | 0 | **PASS — 0 errors, 0 warnings across 208 catalogs** |
| **Faction War Dialogue Suite** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~FactionWarDialogueExpansionTests` | 0 | **PASS — 9 passed, 0 failed** |
| **Content Utilization Gate** | `godot --headless --path . -- --content-utilization-selftest` | 0 | **PASS — CI gate PASS** |
| **Scene Binding Gate** | `godot --headless --path . -- --scene-binding-selftest` | 0 | **PASS — 22/22 passed** |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | 0 | **PASS — 0 errors across 27 scenes** |
| **Host Application Build** | `dotnet build Ashfall.csproj` | 0 | **PASS — 0 errors, 0 warnings** |

---

## 9. Deviations & Follow-Ons

- **Deviations:** None. The final count is exactly 40, all 18 baseline records are preserved, all 22 requested additions were delivered across the exact requested faction distributions, and zero Core code was modified.
- **Follow-Ons:**
  - Future implementation of a host-session UI presenter could optionally track seen snippet IDs across a playthrough session to suppress immediate repeats.
