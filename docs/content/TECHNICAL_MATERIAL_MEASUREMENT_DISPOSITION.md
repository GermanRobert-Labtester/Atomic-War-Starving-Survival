# TECHNICAL MATERIAL MEASUREMENT DISPOSITION — Plan 158 Workstream J

Per-measurement classification: **descriptive** (display data),
**display-only comparison** (may contextualize but never change state), or
**typed-link candidate** (already linked through a proven canonical item).
No measurement is executable.

## Dispositions

| Measurement | Class | Notes |
|---|---|---|
| `fiber_tensile_tenacity_cn_tex` | Descriptive | Historical lab value; comparative display only (higher = better hemp batch, as archival data) |
| `retting_duration_days` | Descriptive | Process history; zero-day entries (batching-oil penetration) are legitimately non-retting process steps — kept as authored |
| `nominal_diameter_mm` | Descriptive | — |
| `wire_rope_construction` | Descriptive | 6×19/6×36/18×7 nomenclature reviewed (Workstream J); prose and summaries preserve construction/spec language |
| `breaking_strength_metric_tons` | Descriptive | Ultimate breaking load; NEVER rendered as safe working load (test-pinned) |
| `rope_diameter_inches` | Descriptive | Imperial units kept as authored (historical hawser stock); no unit conversion performed at runtime |
| `tensile_break_load_kn` | Descriptive | kN metric alongside imperial diameter — historical mixed-unit provenance preserved deliberately (workshop reality), never converted |
| `transmitted_power_kilowatts` | Descriptive | Mechanical-drive spec; no power-grid coupling |
| `splice_length_diameters` | Descriptive | Traditional splice-length convention (× diameters) preserved |
| `ozone_exposure_ppm` | Descriptive | Historical exposure estimate; no live seal-aging model exists |
| `degradation_severity` | Display-only comparison | Renders as the record's failure summary; no mask-condition field |
| `residual_tensile_strength_pct` | Display-only comparison | Historical armor-cloth aging; no armor stat mutation |
| `failure_phenomenon` | Descriptive | Chemistry reviewed: hydrolysis, photolysis, enzymatic digestion are plausible aramid failure modes — kept |
| `rubber_compound_formula` | Descriptive | Provenance strings; never parsed as recipes |
| `vulcanization_temp_celsius` | Descriptive | 20–25 °C cold-cure entries are plausible (self-vulcanizing cement) — kept as authored |
| `road_wear_rating` | Display-only comparison | Workshop defect labels; no vehicle mobility effect |
| `combustion_temperature_celsius` | Descriptive | Cellulose-nitrate ignition ≈ 150–170 °C claims are broadly plausible; no fire event |
| `decomposition_stage` | Display-only comparison | Stage One→Five vinegar-syndrome progression language preserved as authored |

## Technical corrections applied (Task J)

- **Breaking vs working load:** the projection labels wire-rope values
  "breaking X t" and never "safe working load" (pinned by
  `Firewall_BreakingStrength_NotPresentedAsSafeWorkingLoad`).
- **Mixed units:** hawser diameters in inches with kN break loads are
  historical workshop reality; the projection renders both as-authored with
  unit labels — no silent conversion (risk 12 mitigated by not converting).
- **Timestamps:** all `timestamp_relative` strings (YEAR_03 … FINAL ORDER)
  render as archival dating; the archive state carries no timestamps and no
  campaign-clock integration exists (risk 13 mitigated).
