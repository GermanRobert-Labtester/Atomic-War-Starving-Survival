# Audit of Existing 13 Verdict Radio Broadcasts

> **Audit Baseline:** 13 verified records in `verdict_radio.json` prior to Plan 94.

---

## 1. Inventory & Classification

| # | ID | Frequency | Day | Source | Kind | Strength | Anomaly / Clue Function |
|---|---|---|---|---|---|---|---|
| 1 | `radio_verdict_meter_reads_1142` | 99.0 MHz | 210 | Census Carrier, Machine Registers | `telemetry` | S1 | Static meter repetition; establishes the unmonitored machine voice. |
| 2 | `radio_verdict_fuse_serviced` | 99.0 MHz | 211 | Fuse World, Service Bay | `maintenance` | S2 | Automated scheduled service executed without any fault present. |
| 3 | `radio_verdict_wing_sleeps` | 99.0 MHz | 242 | Drone Hive, Draw Readout | `telemetry` | S2 | Zero flight activity; cold roost post-Call. |
| 4 | `radio_verdict_off_count_assessed` | 99.0 MHz | 240 | The Office of Censuses | `call` | S3 | First warning regarding custody penalties. |
| 5 | `radio_verdict_eden_was_here` | 88.5 MHz | 245 | Eden Vale, Tube Bleed | `witness` | S2 | Tube bleed of Eden Vale's weather service traffic on civilian band. |
| 6 | `radio_verdict_count_is_open` | 88.5 MHz | 240 | The Office of Censuses | `call` | S3 | Formal opening of the census count broadcast across civil band. |
| 7 | `radio_verdict_clock_disagrees` | 99.0 MHz | 213 | Machine Registers, Clock | `telemetry` | S1 | Machine calendar and civil calendar diverge by 3 days. |
| 8 | `radio_verdict_geophone_taps` | 99.0 MHz | 218 | Geophone Pit, Array 1 | `readings` | S2 | Ground array detecting human footfalls as anonymous agricultural rhythm. |
| 9 | `radio_verdict_valve_accessed_36` | 99.0 MHz | 250 | Water Plant, Valve S36 | `maintenance` | S2 | Shift 36 valve log keeping count of missing worker. |
| 10 | `radio_verdict_reels_matter` | 99.0 MHz | 255 | Archive Tape-Silo | `count` | S2 | 21 racks, 4 years per rack; 5-year archival schedule. |
| 11 | `radio_verdict_presentation_names_holders` | 99.0 MHz | 260 | The Office of Censuses | `call` | S3 | Requirement for custody presentation before the tribunal. |
| 12 | `radio_verdict_carrier_on_window` | 99.0 MHz | 210 | Census Carrier, Pilot Tone | `carrier` | S1 | 1-second A/B carrier pilot tone opening the Reckoning window. |
| 13 | `radio_verdict_reckoning_call` | 99.0 MHz | 241 | The Office of Censuses — Reckoning | `call` | S3 | Three-hour repeating convening summons before locking carrier. |

---

## 2. Structural & Stylistic Patterns
- **Average Sentence Count:** 2.1 sentences per broadcast.
- **Pacing:** Concentrated heavily in Day 210–260 (early Culpable phase).
- **Voice:** Declarative statements, absence of narrator persona, numbers presented without emotional valence.
- **Identified Coverage Gap:** Zero broadcasts beyond Day 260; lacks long-horizon coverage of deep investigation sites (Coastal, Interior, Border) and late Reckoning countdown.
