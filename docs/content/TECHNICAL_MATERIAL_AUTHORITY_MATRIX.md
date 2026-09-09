# TECHNICAL MATERIAL AUTHORITY MATRIX — Plan 158 Workstream C (§5 firewall)

One row per authored measurement field class: owner, disposition, and the
firewall that keeps it from becoming a mechanic.

| Field class (families) | Units | Owner authority | Disposition | Firewall |
|---|---|---|---|---|
| `fiber_tensile_tenacity_cn_tex` (hemp) | cN/tex | Items.json owns the canonical `rope` item; no rope-quality mechanic exists | **Descriptive** | Tenacity never gates rope use or expedition equipment |
| `retting_duration_days` (hemp) | days | Crafting/production authority (Plan 55 family) | **Descriptive** | No retting recipe derived; process time is historical |
| `breaking_strength_metric_tons` (wire) | metric tons | Expedition/maritime equipment authority | **Descriptive** (display: "breaking X t") | Ultimate breaking load NEVER presented as safe working load (pinned by test); cannot snap or protect player rope |
| `nominal_diameter_mm`, `wire_rope_construction` (wire) | mm / spec string | — | **Descriptive** | — |
| `tensile_break_load_kn` (manila) | kN | Maritime/haulage authority | **Descriptive** | A breakage REPORT foreshadows risk; it cannot snap the player's line or alter expedition success |
| `transmitted_power_kilowatts` (splice) | kW | Power systems (`PowerSupplyContext` family) | **Descriptive** | No power generation/consumption is created or modified |
| `splice_length_diameters` (splice) | ×Ø | — | **Descriptive** | — |
| `ozone_exposure_ppm` (gasket) | ppm | — | **Descriptive** | Never infers current seal integrity |
| `degradation_severity` (gasket) | status string | Item condition/durability authority (if wired) | **Display-only** | Not a hidden gas-mask condition field; no radiation/chemical protection change |
| `residual_tensile_strength_pct` (aramid) | % | Armor/combat authority | **Display-only** | Never reduces armor protection; no combat stat mutation |
| `failure_phenomenon` (aramid) | string | — | **Descriptive** | — |
| `rubber_compound_formula` (tire) | string | Crafting authority (Plan 55) | **Descriptive** | Not a recipe; no crafting input |
| `vulcanization_temp_celsius` (tire) | °C | — | **Descriptive** | Not a recipe input |
| `road_wear_rating` (tire) | status string | Vehicle system (condition/speed) | **Display-only** | No vehicle speed/condition change |
| `combustion_temperature_celsius` (film) | °C | Fire/environment systems | **Descriptive** | No fire event, hazard or ignition from prose; archival fire RISK stays flavor unless a typed hazard producer is authored |
| `decomposition_stage` (film) | stage string | — | **Descriptive** | — |
| `timestamp_relative` (all) | archival strings | — | **Descriptive** | Never campaign time; no countdown from decay values |

## Enforcement

- The projection's public API surface is pinned by
  `Firewall_ProjectionSurface_HasNoMechanicalAPIs` (read-only archive:
  registration, discovery, queries, persistence — nothing else).
- `Firewall_ViewingMeasurements_MutatesNothingButDiscovery` proves the state
  shape is the discovered-id ledger and nothing else.
- Discovery is the ONLY mutation path (`DiscoverAtProducer`/
  `DiscoverRecord`), both idempotent and producer-validated.
