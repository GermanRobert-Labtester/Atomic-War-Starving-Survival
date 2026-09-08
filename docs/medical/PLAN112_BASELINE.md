# Plan 112 baseline

**Status:** Reconciled against the live repository
**Authority:** `Assets/StreamingAssets/Data/disease_catalog.json`
**Catalog schema:** 3
**Baseline before this change:** 16 diseases
**Target after this change:** 20 diseases

## Repository truth

The original Plan 112 brief says 7 diseases. That is stale. The live
catalog already contains the seven legacy rows plus eight Plan 09/9A rows and
`disease_prion_tremor`. All 16 existing IDs and their order are preserved.
Four rows are appended; no existing values were rewritten.

| # | ID | Vector | Lethality | Incubation | Illness | Infectivity |
|---:|---|---|---:|---:|---:|---:|
| 1 | `disease_cholera` | water | .30 | 2 | 4 | .40 |
| 2 | `disease_zoonotic_flu` | air | .18 | 1 | 5 | .55 |
| 3 | `disease_blood_fever` | blood | .45 | 3 | 6 | .25 |
| 4 | `disease_spore_blight` | spore | .40 | 2 | 7 | .30 |
| 5 | `disease_acute_radiation_syndrome` | water | .80 | 0 | 14 | .00 |
| 6 | `disease_fungal_respiratory` | air | .30 | 5 | 10 | .40 |
| 7 | `disease_typhoid_waterborne` | water | .50 | 3 | 8 | .30 |
| 8 | `disease_wellspring_cramps` | water | .20 | 2 | 4 | .45 |
| 9 | `disease_silt_jaundice` | water | .55 | 7 | 14 | .10 |
| 10 | `disease_condemned_air_cough` | air | .25 | 4 | 9 | .35 |
| 11 | `disease_dry_bunker_hiss` | air | .15 | 4 | 12 | .50 |
| 12 | `disease_septic_rust_wound_fever` | blood | .40 | 2 | 7 | .30 |
| 13 | `disease_reused_needle_fever` | blood | .20 | 1 | 5 | .40 |
| 14 | `disease_deep_excavation_mold_lung` | spore | .65 | 6 | 16 | .20 |
| 15 | `disease_silo_lung` | spore | .30 | 9 | 11 | .35 |
| 16 | `disease_prion_tremor` | blood | .85 | 14 | 21 | .00 |

## Runtime facts

- `DiseaseCatalogLoader` accepts schema 3 and four vectors only: `water`,
  `air`, `blood`, and `spore`.
- `DiseaseSystem.BindCatalog` registers one simulation row for every catalog
  disease, including rows appended after an older save is loaded.
- `Infect` is the deterministic direct seed seam. `TryExpose` adds vector,
  immunity, source probability, and seeded-roll handling.
- `spread_radius` caps the number of host-provided candidate survivors
  selected per spread attempt. It is not room geometry.
- `countermeasure_item_id` is catalog metadata for the camp-wide vector
  protocol. Patient treatment is separately authorized by `treatments[]`.
- Existing source adapters remain contracted to their existing disease IDs.
  No speculative source field or second disease authority was added.
