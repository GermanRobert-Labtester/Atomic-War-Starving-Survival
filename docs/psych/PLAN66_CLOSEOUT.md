# Plan 66 — Guilt Sources Expansion: Closeout

## Status: **COMPLETE**

## Summary

`Assets/StreamingAssets/Data/guilt_sources.json` has been successfully expanded from **20 baseline entries to exactly 40 guilt sources** (20 original preserved + 20 new additions). The expansion spans 8 distinct gameplay decision classes, maintains calibrated severity ratings (0.20 to 0.90), strictly avoids moralizing prose by grounding memory in physical artifacts and routines, integrates with `GuiltInsomniaSystem`, and passes all test and integrity gates.

---

## Counts & Roster

```text
Baseline:    20  (cut_ration, reduce_food, starve, leave_behind, abandon,
                 refuse_help, turn_away, execute, kill, shoot, steal, hoard,
                 take_all, lie, deceive, betray, harsh, refuse, deny,
                 sacrifice_other)
New:         20  (hoard_medicine_while_needed, barter_away_needed_food,
                 issue_known_contaminated_supplies, burn_critical_fuel_for_comfort,
                 refuse_refugee_entry, expel_survivor_for_efficiency,
                 hide_cache_from_allies, abandon_committed_rescue,
                 leave_wounded_behind, retreat_from_rescue,
                 execute_surrendered_enemy, use_civilians_as_bait,
                 kill_former_ally, betray_faction_trust, inform_on_survivor,
                 break_final_wish_promise, withhold_pain_relief,
                 triage_by_utility, take_family_last_supplies,
                 order_survivor_to_death)
Total:       40  — 40 unique choice patterns and 40 unique titles
```

---

## Category & Calibration Breakdown

| Category | Count | Patterns & Severities |
|---|---|---|
| **Resource (4)** | 4 | `hoard_medicine_while_needed` (0.80), `barter_away_needed_food` (0.55), `issue_known_contaminated_supplies` (0.70), `burn_critical_fuel_for_comfort` (0.30) |
| **Shelter (3)** | 3 | `refuse_refugee_entry` (0.65), `expel_survivor_for_efficiency` (0.70), `hide_cache_from_allies` (0.45) |
| **Expedition (3)** | 3 | `abandon_committed_rescue` (0.60), `leave_wounded_behind` (0.80), `retreat_from_rescue` (0.65) |
| **Combat (3)** | 3 | `execute_surrendered_enemy` (0.85), `use_civilians_as_bait` (0.90), `kill_former_ally` (0.80) |
| **Social (3)** | 3 | `betray_faction_trust` (0.60), `inform_on_survivor` (0.70), `break_final_wish_promise` (0.85) |
| **Medical (2)** | 2 | `withhold_pain_relief` (0.70), `triage_by_utility` (0.75) |
| **Scavenging (1)** | 1 | `take_family_last_supplies` (0.70) |
| **Leadership (1)** | 1 | `order_survivor_to_death` (0.90) |

---

## Complete 20 New Sources Table

| Choice Pattern | Severity | Title | Category | Description |
|---|---|---|---|---|
| `hoard_medicine_while_needed` | 0.80 | The Last Dose | Resource | The locked cabinet still holds the vial that was deemed too valuable to dispense. {name} knows someone in the infirmary died counting the hours until the next dose. |
| `barter_away_needed_food` | 0.55 | Weight of the Crates | Resource | The trade crates were hauled away for ammunition and scrap. {name} stands in front of the ration pantry, staring at the dust rings where the flour bags sat. |
| `issue_known_contaminated_supplies` | 0.70 | Marked Unsafe | Resource | The radiation warning tape was peeled away before the tins were brought to the table. {name} watches the children eat, saying nothing about the grease pencil mark beneath the label. |
| `burn_critical_fuel_for_comfort` | 0.30 | One Warm Room | Resource | For one evening the stove burned hot enough to sleep without coats. {name} wakes to find black frost choking the hydroponics lines three rooms down. |
| `refuse_refugee_entry` | 0.65 | Closed Door | Shelter | The surveillance monitor kept flickering after {name} cut the exterior intercom. In the morning, the snow outside the heavy outer door is disturbed and empty. |
| `expel_survivor_for_efficiency` | 0.70 | The Empty Bunk | Shelter | Their bunk was reassigned before the sheets went cold. {name} avoids looking at the tally marks carved into the timber post beside the mattress. |
| `hide_cache_from_allies` | 0.45 | Behind the Panel | Shelter | The false floorboard sits flush with the subfloor. {name} looks away as an ally divides the remaining dried lentils into three equal, insufficient portions. |
| `abandon_committed_rescue` | 0.60 | Turned Back | Expedition | The grease pencil marker stays pinned to the sector map where the expedition turned around. {name} cannot bring themselves to wipe the glass clean. |
| `leave_wounded_behind` | 0.80 | One Less Footstep | Expedition | The head count on the way back had one fewer name. Through the long corridor walk home, {name} kept listening for footsteps that never caught up. |
| `retreat_from_rescue` | 0.65 | Radio Still Calling | Expedition | The distress signal was still cycling when {name} switched off the receiver to conserve battery. The speaker clicks softly in the dark before going dead. |
| `execute_surrendered_enemy` | 0.85 | Hands Visible | Combat | The rifle was already dropped in the dirt when {name} pulled the trigger. What stays in memory is how slowly the empty hands drifted downward. |
| `use_civilians_as_bait` | 0.90 | The Safer Route | Combat | The diversion drew the patrol away from the supply cache exactly as calculated. {name} got the team out alive, and that is the part that makes sleep impossible. |
| `kill_former_ally` | 0.80 | Known Face | Combat | The insignia on the jacket was new, but the voice across the barricade was not. {name} cleaned their weapon afterward without looking at the brass casing on the floor. |
| `betray_faction_trust` | 0.60 | Terms Broken | Social | The signed pact is still filed in the dispatch locker with {name}'s name on the seal. The people who honored it are no longer answering on the wire. |
| `inform_on_survivor` | 0.70 | Name Given | Social | The patrol only asked for a name once before handing over the supply voucher. {name} holds the canned meat in their palms, unable to open it. |
| `break_final_wish_promise` | 0.85 | The Promise | Social | There is no one left alive to ask whether the last promise was kept. {name} carries the unfulfilled words like lead in their chest. |
| `withhold_pain_relief` | 0.70 | Saved for Later | Medical | The ampoule remains unbroken in the medical kit for a future emergency. {name} remembers the sound of breathing in the dark ward after the lantern went out. |
| `triage_by_utility` | 0.75 | Useful Enough | Medical | The triage tag marked priority by work output rather than blood loss. {name} wrote the numbers down with steady hands that now shake when holding a pen. |
| `take_family_last_supplies` | 0.70 | Nothing Left | Scavenging | The pantry shelves were scraped bare down to the wood shavings. {name} noticed the pencil height marks on the doorframe only after the rucksack was already zipped. |
| `order_survivor_to_death` | 0.90 | The Order Given | Leadership | They asked only once if there was another way before stepping into the irradiated conduit. {name} gave the order, and the shelter went quiet. |

---

## Runtime Contract & Persistence

- **Guilt System (`GuiltInsomniaSystem.cs`):**
  - Per-survivor tracking (`GuiltSurvivorState`).
  - Stacking records with cumulative insomnia severity capped at `1.0`.
  - Expiration after `30` days.
  - Sedative relief (`-0.40` severity, 12h duration) and dialogue relief (removes newest record).
  - High severity threshold (`>= 0.70`) fires `OnGuiltInsomniaCritical`.
- **Save/Load Compatibility (`GuiltInsomniaSaveState`):**
  - Fully round-trips all survivors, records, timestamps, and severity levels.
- **Engine-Agnostic Core (`Invariant 1`):**
  - Pure data expansion in `Assets/StreamingAssets/Data/guilt_sources.json`.
  - Zero engine coupling added to `Ashfall.Core`.

---

## Verification Matrix Results

| Verification Gate | Command | Result | Notes |
|---|---|---|---|
| **Data Integrity Selftest** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 errors)** | 298 catalogs validated |
| **Plan 66 Dedicated Tests** | `dotnet test --filter GuiltSourcesPlan66CatalogTests` | **PASS (8/8 passed)** | Size, categories, uniqueness, calibration, system accumulation |
| **Full Guilt Test Suite** | `dotnet test --filter "FullyQualifiedName~Guilt"` | **PASS (34/34 passed)** | All catalog and system tests passing |
| **Full Core Test Suite** | `dotnet test Ashfall.Core.Tests` | **PASS (9,372 passed, 0 failed)** | Full regression suite green |
| **Scene Binding Selftest** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (25/25 passed)** | All production UI scenes verified |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** | CI gate verified |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (0 errors)** | 30 production scenes clean |
