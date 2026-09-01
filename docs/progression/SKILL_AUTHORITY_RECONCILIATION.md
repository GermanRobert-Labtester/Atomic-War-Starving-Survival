# Skill Authority Reconciliation

> **Document Status:** Authoritative Architectural Reconciliation
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Problem Statement

`SkillDef.cs` line 9 had documented:
> "the canonical ids live in `skills.json` in `Assets/StreamingAssets/Data/`."

However, forensic audits discovered that `skills.json` did not exist on disk, and `SkillProgressionSystem.cs` was initializing 9 action-driven skills, 28 domain milestones, and 73 latent milestone traits directly inside C# code (`RegisterDefaultSkills()`).

---

## 2. Reconciliation Solution

1. **Created `skills.json`:** Formatted authoritative JSON catalog in `Assets/StreamingAssets/Data/skills.json` with `schema_version: 1`, covering all 110 skills and latent milestone capabilities.
2. **Created `SkillCatalogLoader.cs`:** Engine-agnostic loader in `Assets/Ashfall.Core/Survivors/SkillCatalogLoader.cs` using `IFileIO` and `IJsonSerializer` ports.
3. **Deterministic Zero-Drift Guarantee:** All skill IDs, display names, disciplines, XP thresholds, and mechanical bonuses strictly mirror the baseline logic.
4. **Resilient Fallback:** `SkillProgressionSystem.RegisterDefaultSkills()` remains available as a zero-dependency fallback for headless unit tests.
