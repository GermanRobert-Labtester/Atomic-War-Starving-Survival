# Room Variant Matrix

## 1. Resolution of the 18 vs. 20 Roster Gap
The source plan listed 18 explicit slots while specifying 20 room definitions. To resolve this without filler, two evidence-driven specialist variants were added:
- **Slot 19**: `room_workshop_precision` (Precision Tooling Bench for electronics/spectroscopy maintenance, distinct from general and heavy machinery).
- **Slot 20**: `room_ward_quarantine` (Isolation Quarantine Bay with negative pressure and UV lamps for outbreak containment).

## 2. Multi-Variant Functional Differentiation
- **Dormitories (3 variants)**:
  - `room_bunks_crowded`: Capacity 6, cheap build cost, lower comfort.
  - `room_bunks`: Capacity 4, standard balance of space and comfort.
  - `room_quarters_private`: Capacity 2, high comfort/rest bonus, higher cloth/wood cost.
- **Workshops (3 variants)**:
  - `room_workshop`: General fabrication and shelter patching.
  - `room_workshop_heavy`: Engine overhauls, motor repairs, vehicle support.
  - `room_workshop_precision`: Fine electronics, soldering, radio calibration.
- **Medical Bays (3 variants)**:
  - `room_clinic`: Emergency trauma triage and dressings.
  - `room_ward_clinical`: Intensive surgical suite.
  - `room_ward_quarantine`: Biological isolation and decontamination ward.
- **Storage (2 variants)**:
  - `room_storage_bay`: Bulk scrap and dry ration pallets.
  - `room_storage_secure`: Blast-armored safe for munitions and pharmaceuticals.
- **Common Areas (2 variants)**:
  - `room_common_mess_hall`: Communal gathering and group dining.
  - `room_reading_quiet_room`: Technical study and stress reduction.
