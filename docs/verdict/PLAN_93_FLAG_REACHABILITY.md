# Plan 93 — Gating Flag Reachability Matrix

> **Flag Authority:** `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` (`KnownRuntimeIds`)
> **Evaluation Engine:** `VerdictNpcSystem.GetAvailable(flags, phase, locationId)`

---

## 1. Complete Gating Flag Inventory

| NPC | Gating Flag | Trigger Condition / Setter | Earliest Phase | Reachable In-Game? |
|---|---|---|---|---|
| `npc_eden_vale` | `flag_verdict_eden_log_recovered` | Enrolling `evidence_eden_log` from comm-array | 1 | Yes |
| `npc_ferris_voss` | `flag_verdict_fuse_world_read` | Reading machine log or enrolling `evidence_fuse_linen` | 1 | Yes |
| `npc_iran_bell` | `flag_verdict_shift_charter_restored` | Enrolling `evidence_fuse_linen` | 2 | Yes |
| `npc_selya_saltmarsh` | `flag_verdict_clerk_met` | Enrolling `evidence_geophone_hymn` | 2 | Yes |
| `npc_maro_veen` | `flag_verdict_call_resolved` | Reckoning call resolved in tribunal loop | 3 | Yes |
| `npc_whisper_cipher` | `flag_verdict_relay_read` | Reading machine log at relay mast | 1 | Yes |
| `npc_tomas_reid` | `flag_verdict_reid_enrolled` | Day 170 courtroom tribunal enrollment | 1 | Yes |
| `npc_elena_vane` | `flag_verdict_vane_enrolled` | Day 170 courtroom tribunal enrollment | 2 | Yes |
| `npc_kasper_holt` | `flag_verdict_holt_enrolled` | Day 170 courtroom tribunal enrollment | 1 | Yes |
| `npc_mara_elsen` | `flag_verdict_tide_gauge_inspected` | Physical survey of the Greywater stilling well | 1 | Yes |
| `npc_ilya_venn` | `flag_verdict_weather_chart_recovered` | Recovery of manual barograph chart at Cape Wrath | 1 | Yes |
| `npc_garrick_daal` | `flag_verdict_cliff_signal_decoded` | Decryption of the North Cliff automated repeater log | 2 | Yes |
| `npc_sena_korr` | `flag_verdict_marine_samples_cataloged` | Accession of the St. Jude benthic specimen store | 2 | Yes |
| `npc_torin_rask` | `flag_verdict_forestry_grid_surveyed` | Inspection of the Blackwood forestry survey plots | 1 | Yes |
| `npc_oren_varek` | `flag_verdict_core_strata_verified` | Extraction of core seven from Highland repository | 2 | Yes |
| `npc_lena_rost` | `flag_verdict_river_stage_logged` | Logging high-water mud line at Karsk gorge tower | 1 | Yes |
| `npc_tessa_mirn` | `flag_verdict_seed_trials_audited` | Audit of radionuclide crop trial flats in Agronomy vault | 2 | Yes |
| `npc_karel_norn` | `flag_verdict_border_handoff_recorded` | Decoding demarcation carrier log at Pass 4 relay | 3 | Yes |

---

## 2. Integrity Verification
- All 18 gating flags are registered in `KnownRuntimeIds` in `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` and `Assets/Ashfall.Core/CatalogIntegrityRules.cs`.
- Validated via `--data-integrity-selftest`: 0 missing reference errors across all catalogs.
