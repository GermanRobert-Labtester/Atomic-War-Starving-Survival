# Plan 33 — Regression Test Matrix

## 1. Test Verification Suite

| Test Method | Test Class | Verification Objective | Result |
|---|---|---|---|
| `Catalog_LoadsExactExpectedCount_FromAuthoritativeJson` | `Plan33SkillCatalogExternalizationTests` | Verifies 148 total skills loaded from `skills.json` | PASS |
| `AllSkills_HaveValidIdsPrefixAndNonNegativeThresholds` | `Plan33SkillCatalogExternalizationTests` | Validates prefixes, uniqueness, and attribute bounds | PASS |
| `BaselineSkills_MatchExpectedValues` | `Plan33SkillCatalogExternalizationTests` | Exact match for baseline action skills and milestones | PASS |
| `NewGroundedSkills_PresentAndConfigured` | `Plan33SkillCatalogExternalizationTests` | Asserts presence of 3 new skills | PASS |
| `Loader_HandlesMissingOrCorruptedPathGracefully` | `Plan33SkillCatalogExternalizationTests` | Safe degradation when file missing/corrupt | PASS |
| `Progression_ActionDrivenXp_UnlocksTierAndAppliesBonus` | `Plan33SkillCatalogExternalizationTests` | Action XP unlocking and bonus application | PASS |
| `MilestoneGranting_WorksForNewGroundedSkills` | `Plan33SkillCatalogExternalizationTests` | Direct milestone skill grants | PASS |
| `SaveAndRestore_RoundTripsWithLoadedCatalog` | `Plan33SkillCatalogExternalizationTests` | Save state round-trip preservation | PASS |
| `CatalogLoader_PopulatesKnownDisciplineSkills` | `SkillProgressionSystemTests` | Acceptance test for catalog population | PASS |
| `RecordProgress_IncrementsProgressAndAwakensAtThreshold` | `LatentExpertAwakeningSystemTests` | Latent trait awakening integration | PASS |
| `MultiStepProgress_AwakensOnlyAfterFullThreshold` | `LatentExpertAwakeningSystemTests` | Multi-step progress threshold awakening | PASS |
| `SaveAndRestore_PreservesProgressAndAwakenedStatus` | `LatentExpertAwakeningSystemTests` | Awakening state save/restore | PASS |

---

## 2. CI Verification Matrix Execution

- `dotnet test Ashfall.Core.Tests`: 5,812 / 5,812 tests passed (0 failures).
- `CatalogIntegrityValidator`: 0 findings across 159 catalogs.
