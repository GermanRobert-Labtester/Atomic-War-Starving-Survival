# Plan 112 new-thirteen roster, repository-truth amendment

The source brief requested 13 additions against a seven-row baseline. That
baseline was stale, so adding 13 would produce 29 rows and duplicate live
content. The final implementation adds four rows to the live 16-row catalog.

## Added

| ID | Source concept retained | Runtime reason |
|---|---|---|
| `disease_dysentery` | failed latrines / contaminated water | maps to supported `water` vector |
| `disease_meningococcal_fever` | crowded winter camp | maps to supported `air` vector |
| `disease_bloodborne_hepatitis` | shared needles / field clinic | maps to supported `blood` vector |
| `disease_spore_wound_dermatitis` | mold-heavy work through damaged skin | maps to supported `spore` vector |

## Disposition of the other requested concepts

| Requested concept | Disposition |
|---|---|
| frost pneumonia | already covered by multiple live air respiratory rows; no duplicate |
| radiation sickness | already `disease_acute_radiation_syndrome`; infectivity remains zero |
| generic dysentery | added as the waterborne communicable row |
| tetanus | deferred; wound-local, not a proximity-spread disease in this runtime |
| typhus | deferred; would require a vector/source contract not present |
| hepatitis | added as `disease_bloodborne_hepatitis` |
| chemical pneumonitis | deferred; toxic exposure is not communicable disease spread |
| lead poisoning | deferred; toxicity is not communicable disease spread |
| fungal pneumonia | already covered by fungal respiratory and mold-lung rows |
| food poisoning | deferred to the existing water exposure model until food has a disease vector |
| frostbite infection | deferred; frostbite is a cold injury, not a catalog pathogen |
| rabies | deferred; would need an animal-bite source and a different treatment contract |
| scurvy | deferred; nutritional deficiency is not communicable |
