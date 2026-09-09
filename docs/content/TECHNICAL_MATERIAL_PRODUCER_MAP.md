# TECHNICAL MATERIAL PRODUCER MAP — Plan 158 Workstreams D–H

26 of 60 records activated across **9 producer paths**, all existing
deep-lore sites (no invented locations), spanning the plan's required
domains: maritime, maintenance, protective gear, vehicles and archives.

## Producer paths

| Producer (deep-lore site) | Domain | Records | Families |
|---|---|---|---|
| `location_drainage_network` | Maritime (ferry/barge hawsers in the flooded district) | 3 | ManilaHawser |
| `location_frozen_wetland` | Maritime/rescue (winch line) | 1 | ManilaHawser |
| `location_steelworks` | Maintenance/industrial (hoist wire rope + forge trip-hammer drive) | 4 | WireRope, TransmissionSplice |
| `location_power_substation` | Maintenance (ventilation-fan drive, tension carriage) | 2 | TransmissionSplice |
| `location_chemical_plant` | Protective equipment (gasket aging) | 3 | NeopreneGasket |
| `location_ammunition_depot` | Vehicles + armor caches (tire workshop logs, aramid cache rot) | 4 | TireRetread, Aramid |
| `location_police_station` | Protective/armor evidence lockers | 2 | Aramid |
| `location_television_studio` | Archives (nitrate film hazard) | 3 | Celluloid |
| `location_municipal_library` | Archives (safety-film/documentary) | 2 | Celluloid |
| `location_agricultural_research` | Agriculture (hemp fiber plots) | 2 | HempFiber |

## Domain coverage vs plan requirements

- Maritime (§10): hawser breakage reports surface at the flooded drainage
  network — foreshadowing only; expedition equipment authority untouched.
- Maintenance (§11): wire-rope assays and transmission-drive audits surface
  at the steelworks/substation; Plan 148 incident state is never caused by
  these records.
- Protective gear (§12): gasket degradation at the chemical plant; item links
  to `gas_mask`/`item_gas_mask_improved`/`protective_rubber_gloves` feed the
  item-inspection query surface (`RecordsForItem`).
- Vehicles (§13): tire retread logs at the ammunition depot; no tire
  condition subsystem created; no vehicle speed/condition effect.
- Archives (§14): celluloid decomposition at the television studio and
  municipal library; combustion temperatures stay descriptive — no fire
  mechanic.

## Deferred (34 records)

All remaining records (5 hemp, 5 wire, 3 manila, 4 splice, 5 gasket, 5 aramid,
4 tire, 2 film) stay archival depth: `deferred` — no producer, invisible to
discovery, fail-closed on direct discovery attempts. Dispositions per family
in TECHNICAL_MATERIAL_MEASUREMENT_DISPOSITION.md. Bulk-deferral is deliberate:
broad contextual coverage over forcing all 60 records into a single campaign
(§19).

## Content-utilization summary (§19)

- Reachable by family: all 8 families represented (2–5 records each).
- Canonical item links: 10 records → 7 canonical item ids.
- Producer distribution: maritime 4 · maintenance 6 · protective 5 ·
  vehicles 3 · armor-history 2 · archive 5 · agriculture 2 — no duplicate
  producer triggers; each record has exactly one primary producer.
- Ordinary-campaign exposure: site discovery surfaces 3–4 records per
  relevant location; item inspection surfaces linked records once found.
