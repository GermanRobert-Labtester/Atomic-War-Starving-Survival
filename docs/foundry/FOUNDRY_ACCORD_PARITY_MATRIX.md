# Foundry Accord Baseline Parity Matrix

**Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Status:** Baseline District 8 accords frozen and preserved byte-for-byte.

---

## 1. Baseline Four District 8 Accords

| Treaty ID | Ratified Day | Title | Signatories | Demarcated Territory | Water (lpm) | Power (kW) | Tariff Summary | Tags |
|---|---:|---|---|---|---:|---:|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | 280 | The Brine Pipe & Iodine Exchange | `faction_silent_foundry`, `faction_the_office` | The smelter bay casting floor to the saltworks membrane hall | 40.0 | 12.0 | Four brine pipes and two valve bodies per cycle for iodine and salt. | `foundry`, `saltworks`, `brine`, `iodine`, `exchange`, `district8` |
| `treaty_cluster_labour_schedule` | 305 | The Cluster Labour Schedule | `faction_silent_foundry`, `faction_the_office`, `faction_the_cutters` | The charging floor, the school bell, and the eight hours between them | 25.0 | 8.0 | Half-litre clean water per stoker per shift, boiled at Cluster expense. | `foundry`, `labour`, `cluster`, `school`, `schedule`, `district8` |
| `treaty_road_iron_charter` | 330 | The Road Iron Charter | `faction_silent_foundry`, `faction_the_cutters`, `faction_the_fleet` | The casting floor to the Cut, by whatever lane is marked | 15.0 | 6.0 | Sixty ice anchors and three winch drums per cycle; coal/scrap hauled at column rates. | `foundry`, `road`, `ice`, `anchors`, `charter`, `district8` |
| `treaty_the_cluster_charter` | 365 | The Cluster Charter | `faction_silent_foundry`, `faction_the_office`, `faction_the_cutters`, `faction_the_fleet` | The smelter bay, its ledger, and the schedule it answers to | 0.0 | 0.0 | None. The charter is not a trade; it is a signature. | `foundry`, `charter`, `cluster`, `schedule`, `district8` |

---

## 2. Parity Verification

- **ID Invariant:** Zero renaming of the 4 baseline IDs.
- **Day Invariant:** Ratified days remain Day 280, 305, 330, and 365.
- **Content Invariant:** Articles, penalties, water allocations, and power quotas match original authored values identically.
- **Test Gate:** Verified by `FoundryAccordExpansionTests.Parity_BaselineFourDistrict8AccordsPreserved`.

## 3. Plan 102 Additions

The six additions are appended after the 12 pre-existing regional records so
the original 12 records are not rewritten or reordered:

| Treaty ID | Ratified Day | Distinct Function | Foundry Signatory | Live Consequence Policy |
|---|---:|---|---|---|
| `treaty_saltworks_access` | 285 | Measured saltworks right-of-way and water access | Yes | Dependency-ready |
| `treaty_membrane_repair` | 300 | Technical membrane-hall repair and acceptance | Yes | Dependency-ready |
| `treaty_coal_window` | 320 | Safe ice-road haul timing | Yes | Dependency-ready |
| `treaty_apprentice_exchange` | 345 | Supervised casting/salvage training | Yes | Dependency-ready |
| `treaty_crisis_mutual_aid` | 350 | Bounded emergency service pool | Yes | Dependency-ready |
| `treaty_the_incident_book` | 355 | Witnessed incident reporting and amendment control | Yes | Dependency-ready |

The catalog therefore has 18 records overall and exactly 10 records returned by
`RegionalTreatyCatalog.GetByExactSignatoryFaction("faction_silent_foundry")`.
