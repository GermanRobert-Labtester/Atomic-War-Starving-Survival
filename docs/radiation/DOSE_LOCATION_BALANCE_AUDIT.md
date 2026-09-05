# Dose Location Balance Audit

> **Calibration:** Mathematical evaluation of all 14 dose locations against dwell times, campaign health thresholds, and survival progression.

---

## 1. Dwell Duration Benchmark Calculations

| Location ID | Sector | Rate (µSv/h) | 15-Minute Hop (µSv) | 1-Hour Shift (µSv) | 4-Hour Sortie (µSv) | 10-Hour Mission (mSv) |
|---|---|:---:|:---:|:---:|:---:|:---:|
| `loc_the_dose_room` | `bunker` | 0.02 | 0.005 | 0.02 | 0.08 | 0.0002 |
| `loc_the_calibration_bench` | `bunker` | 0.02 | 0.005 | 0.02 | 0.08 | 0.0002 |
| `loc_the_childrens_baseline_board` | `bunker` | 0.02 | 0.005 | 0.02 | 0.08 | 0.0002 |
| `loc_the_register_hall` | `bunker` | 0.02 | 0.005 | 0.02 | 0.08 | 0.0002 |
| `loc_the_screening_station` | `bunker` | 0.04 | 0.010 | 0.04 | 0.16 | 0.0004 |
| `loc_shelter_exterior_approach` | `surface` | 0.85 | 0.213 | 0.85 | 3.40 | 0.0085 |
| `loc_surface_observation_post` | `surface` | 1.75 | 0.438 | 1.75 | 7.00 | 0.0175 |
| `loc_contaminated_water_access` | `surface` | 3.50 | 0.875 | 3.50 | 14.00 | 0.0350 |
| `loc_garrison_checkpoint_gamma_exterior` | `faction` | 4.10 | 1.025 | 4.10 | 16.40 | 0.0410 |
| `loc_frozen_wetland_crossing` | `external` | 6.20 | 1.550 | 6.20 | 24.80 | 0.0620 |
| `loc_burned_woodland_ridge` | `external` | 8.40 | 2.100 | 8.40 | 33.60 | 0.0840 |
| `loc_irradiated_forest_edge` | `expedition` | 18.50 | 4.625 | 18.50 | 74.00 | 0.1850 |
| `loc_ruined_hospital_grounds` | `expedition` | 28.00 | 7.000 | 28.00 | 112.00 | 0.2800 |
| `loc_military_depot_perimeter` | `expedition` | 45.00 | 11.250 | 45.00 | 180.00 | 0.4500 |

---

## 2. Campaign Pacing Findings

1. **Bunker Living Is Sustainable:** Even after 365 days (8,760 hours) in the shelter, a survivor accumulates only:
   $$0.02 \times 8760 = 175.2\,\mu\text{Sv} = 0.175\,\text{mSv}$$
   This is well below the 100 mSv Amber threshold, proving the bunker affords true long-term sanctuary.
2. **Routine Surface Work Has Measurable Costs:** Daily 2-hour water pumping shifts at `loc_contaminated_water_access` generate:
   $$3.5 \times 2 \times 30\,\text{days} = 210\,\mu\text{Sv} = 0.21\,\text{mSv/month}$$
   Over a 3-year campaign, this yields ~7.5 mSv — noticeable on the ledger, incentivizing water filtration upgrades and labor rotation.
3. **Expeditions Drive Progression Pressure:** Repeated expeditions to `loc_military_depot_perimeter` (0.18 mSv per 4-hour sortie) combined with external route travel (0.05–0.08 mSv) produce ~0.25 mSv per mission. While safe for occasional runs, daily sorties into hot zones will degrade dosimeter tags and push scavengers into Amber and Red bands unless managed with anti-rads and rest shifts.
