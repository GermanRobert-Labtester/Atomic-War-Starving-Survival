# Plan 33 — Skill Catalog Migration Guide

## 1. Migration Overview
- **Origin:** Hardcoded C# method registrations in `SkillProgressionSystem.cs`.
- **Destination:** Pure JSON catalog in `Assets/StreamingAssets/Data/skills.json`.
- **Loader:** Engine-agnostic `SkillCatalogLoader` using `IFileIO` and `IJsonSerializer` ports.

---

## 2. Step-by-Step Migration Record
1. **Catalog Construction:** Built `skills.json` containing:
   - 9 Action-driven baseline skills.
   - 32 Milestone-driven domain skills.
   - 104 Latent expert skills.
   - 3 Grounded additions (`skill_field_surgery`, `skill_water_filtration`, `skill_radio_repair`).
   - Total: 148 skills.
2. **Schema & Integrity Validation:** Added schema verification and uniqueness constraints into `SkillCatalogLoader`. Verified via `CatalogIntegrityValidator`.
3. **C# Decoupling:**
   - Deleted inline hardcoded methods: `RegisterCombatMilestones`, `RegisterSurvivalMilestones`, `RegisterShelterMilestones`, `RegisterMedicalMilestones`, `RegisterExpeditionMilestones`, `RegisterSocialMilestones`, `RegisterLatentExpertTraits`.
   - Updated `RegisterDefaultSkills()` to act as a zero-op fallback.
   - Added architectural comment pointing to JSON authority.
4. **Test Suite Modernization:**
   - Updated `SkillProgressionSystemTests` and `LatentExpertAwakeningSystemTests` to populate from `skills.json`.
   - Added dedicated `Plan33SkillCatalogExternalizationTests` suite.
