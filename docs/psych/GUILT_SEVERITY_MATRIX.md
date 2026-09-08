# Plan 66 — Guilt Severity Calibration Matrix

**Authority:** `Assets/StreamingAssets/Data/guilt_sources.json` (40 entries)

---

## 1. Calibration Framework

Guilt severity in ASHFALL operates in a continuous range from `0.10` to `1.00`, categorized into four balanced operational bands:

| Band | Range | Description | Catalog Count | % of Catalog |
|---|---|---|---|---|
| **Minor / Moderate** | `0.10 – 0.50` | Avoidable harshness, non-fatal concealment, selfish convenience, petty theft. | 10 | 25.0% |
| **Moderate / High** | `0.55 – 0.70` | Deprivation under pressure, mission retreat, unshared resources, triage triage. | 13 | 32.5% |
| **Severe** | `0.75 – 0.80` | Deliberate abandonment, rationing cuts, killing former allies, severe breach of trust. | 9 | 22.5% |
| **Devastating** | `0.85 – 0.90` | Cold execution, civilian sacrifice, intentional starvation, ordering certain death. | 8 | 20.0% |
| **Total** | | | **40** | **100%** |

---

## 2. Complete 40-Source Severity Roster

| # | Choice Pattern | Severity | Band | Title | Category |
|---|---|---|---|---|---|
| 1 | `harsh` | 0.20 | Minor | The Harsh Word | Social |
| 2 | `refuse` | 0.25 | Minor | The Refusal | Social |
| 3 | `deny` | 0.25 | Minor | The Denial | Social |
| 4 | `burn_critical_fuel_for_comfort` | 0.30 | Moderate | One Warm Room | Resource |
| 5 | `lie` | 0.35 | Moderate | The Lie | Social |
| 6 | `deceive` | 0.40 | Moderate | The Deception | Social |
| 7 | `hoard` | 0.40 | Moderate | The Hoard | Resource |
| 8 | `steal` | 0.45 | Moderate | The Theft | Scavenging |
| 9 | `hide_cache_from_allies` | 0.45 | Moderate | Behind the Panel | Shelter |
| 10 | `refuse_help` | 0.50 | Moderate | The Turned Back | Shelter |
| 11 | `barter_away_needed_food` | 0.55 | Moderate-High | Weight of the Crates | Resource |
| 12 | `turn_away` | 0.55 | Moderate-High | The Closed Hatch | Shelter |
| 13 | `take_all` | 0.60 | Moderate-High | The Empty Cache | Scavenging |
| 14 | `abandon_committed_rescue` | 0.60 | Moderate-High | Turned Back | Expedition |
| 15 | `betray_faction_trust` | 0.60 | Moderate-High | Terms Broken | Social |
| 16 | `refuse_refugee_entry` | 0.65 | Moderate-High | Closed Door | Shelter |
| 17 | `retreat_from_rescue` | 0.65 | Moderate-High | Radio Still Calling | Expedition |
| 18 | `leave_behind` | 0.70 | Moderate-High | The Abandoned | Expedition |
| 19 | `issue_known_contaminated_supplies` | 0.70 | Moderate-High | Marked Unsafe | Resource |
| 20 | `expel_survivor_for_efficiency` | 0.70 | Moderate-High | The Empty Bunk | Shelter |
| 21 | `inform_on_survivor` | 0.70 | Moderate-High | Name Given | Social |
| 22 | `withhold_pain_relief` | 0.70 | Moderate-High | Saved for Later | Medical |
| 23 | `take_family_last_supplies` | 0.70 | Moderate-High | Nothing Left | Scavenging |
| 24 | `reduce_food` | 0.75 | Severe | The Empty Bowls | Resource |
| 25 | `abandon` | 0.75 | Severe | The Betrayal of Trust | Social |
| 26 | `betray` | 0.75 | Severe | The Betrayal | Social |
| 27 | `triage_by_utility` | 0.75 | Severe | Useful Enough | Medical |
| 28 | `shoot` | 0.80 | Severe | The Shot | Combat |
| 29 | `cut_ration` | 0.80 | Severe | The Ration Cut | Resource |
| 30 | `hoard_medicine_while_needed` | 0.80 | Severe | The Last Dose | Resource |
| 31 | `leave_wounded_behind` | 0.80 | Severe | One Less Footstep | Expedition |
| 32 | `kill_former_ally` | 0.80 | Severe | Known Face | Combat |
| 33 | `kill` | 0.85 | Devastating | The Taking of Life | Combat |
| 34 | `sacrifice_other` | 0.85 | Devastating | The Sacrifice of Another | Social |
| 35 | `execute_surrendered_enemy` | 0.85 | Devastating | Hands Visible | Combat |
| 36 | `break_final_wish_promise` | 0.85 | Devastating | The Promise | Social |
| 37 | `starve` | 0.90 | Devastating | The Starvation Order | Resource |
| 38 | `execute` | 0.90 | Devastating | The Execution | Combat |
| 39 | `use_civilians_as_bait` | 0.90 | Devastating | The Safer Route | Combat |
| 40 | `order_survivor_to_death` | 0.90 | Devastating | The Order Given | Leadership |
