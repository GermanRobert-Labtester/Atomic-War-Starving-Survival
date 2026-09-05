# Foundry Accord Tag Vocabulary & Normalization

**Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

---

## 1. Normalized Tag Vocabulary

The 12 treaties use a normalized vocabulary of 42 lowercase keyword tags. Synonyms (such as `labor` vs `labour` or `aid` vs `mutual_aid`) are strictly avoided:

| Semantic Family | Normalized Tags |
|---|---|
| **Institutions & Factions** | `foundry`, `garrison`, `rebuilders`, `flotilla`, `cutters`, `ash_sign`, `forward_roster`, `the_scale` |
| **Locations & Geography** | `district8`, `cluster`, `verge`, `coast`, `scarp`, `suburbs`, `neutral_ground` |
| **Commodities & Resources** | `brine`, `iodine`, `grain`, `fuel`, `scrap`, `water`, `ice`, `anchors` |
| **Infrastructure & Works** | `saltworks`, `school`, `road`, `saline`, `switchback`, `salvage`, `aquifer`, `observatory`, `sanctuary` |
| **Governance & Diplomacy** | `exchange`, `schedule`, `charter`, `tithe`, `convention`, `demilitarization`, `border`, `industrial` |

---

## 2. Invariants Enforced

- All tags are lowercase snake_case strings.
- Pinned by `FoundryAccordExpansionTests.Tags_FollowNormalizedVocabularyWithoutSynonymSplits`.
- Enables reliable querying via `RegionalTreatyCatalog.GetByTag(tag)`.
