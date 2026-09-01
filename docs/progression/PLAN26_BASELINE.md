# Plan 26 — Knowledge, Research & Skills: Baseline Audit

> **Document Status:** Authoritative Baseline Audit
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026
> **Target:** Progression world unification, data authority repair, research tech tree externalization, skill catalog reconciliation, trade specialty expansion, latent expert awakening, library manuals repair, and autopsy knowledge expansion.

---

## 1. Executive Summary

ASHFALL contains rich primitives for deep survivor progression, communal knowledge, physical manuals, and forensic pathology. However, prior to Plan 26, significant portions of the progression authority were embedded directly in C# code:
1. `ResearchSystem.cs` registered 15 core base research nodes and 16 relic blueprint nodes inline in C# rather than loading them from JSON.
2. `SkillDef.cs` documented that canonical skill IDs live in `skills.json`, but `skills.json` was missing from `Assets/StreamingAssets/Data/`, with 47+ skills hardcoded in `SkillProgressionSystem.cs`.
3. `trade_specialties.json` contained only 4 authored specialties (`electrician`, `nurse`, `machinist`, `teacher`).
4. `library_manuals.json` contained only 3 manuals with property naming mismatches against C# DTOs.
5. `autopsy_procedures.json` contained only 3 procedures (`procedure_rad_pathology`, `procedure_toxicology`, `procedure_containment_autopsy`).
6. `survivors.json` listed 73 distinct `latentExpertTrait` values across ~129 survivors with no runtime awakening event loop.

Plan 26 establishes a single, deterministic, data-driven progression pipeline across all six domains.

---

## 2. Baseline Inventory

| Domain / Catalog | Baseline Authored Records | Storage Authority Prior to Plan 26 | Plan 26 Target |
|---|---|---|---|
| **Research Tech Tree** | 15 base + 16 relic nodes (31 total) | Hardcoded in `ResearchSystem.RegisterDefaults()` | `research_knowledge.json` (40+ nodes across 6 categories) |
| **Skill Definitions** | 9 action + 28 milestone + 73 latent (110 total) | Hardcoded in `SkillProgressionSystem.RegisterDefaultSkills()` | `skills.json` (Authoritative catalog + loader) |
| **Trade Specialties** | 4 specialties | `trade_specialties.json` | 16 specialties with 3 distinct qualitative tiers |
| **Latent Traits** | 73 distinct traits across 129 survivors | Metadata in `survivors.json` (unawakened) | 12 high-value traits with deterministic awakening triggers |
| **Library Manuals** | 3 manuals | `library_manuals.json` (schema drift) | 12 valid manuals binding to items, research, and skills |
| **Autopsy Procedures** | 3 procedures | `autopsy_procedures.json` | 9 procedures routing disease intel and Verdict evidence |

---

## 3. Core Architecture Placement

```text
                               +---------------------------------------------+
                               |        Assets/Ashfall.Core/Research/        |
                               |  - ResearchSystem.cs                        |
                               |  - ResearchKnowledgeDef.cs                  |
                               |  - ResearchKnowledgeCatalogLoader.cs (NEW)  |
                               |  - ResearchState.cs                         |
                               +----------------------+----------------------+
                                                      |
                   +----------------------------------+----------------------------------+
                   |                                                                     |
+------------------v------------------+                               +------------------v------------------+
|    Assets/Ashfall.Core/Survivors/   |                               |      Assets/Ashfall.Core/Medical/   |
|  - SkillProgressionSystem.cs        |                               |  - AutopsySystem.cs                 |
|  - SkillDef.cs                      |                               |  - AutopsyProcedure.cs              |
|  - SkillCatalogLoader.cs (NEW)      |                               |  - DiseaseSystem.cs (Plan 09)       |
|  - TradeSpecialtySystem.cs (NEW)    |                               +------------------+------------------+
|  - LatentExpertAwakeningSystem (NEW)|                                                  |
+------------------+------------------+                               +------------------v------------------+
                   |                                                  |    Assets/Ashfall.Core/Library/     |
+------------------v------------------+                               |  - LibraryStudySystem.cs            |
|       Data Authority (JSON)         |                               |  - LibraryManualCatalogLoader.cs    |
| - research_knowledge.json (NEW)     |                               +-------------------------------------+
| - skills.json (NEW)                 |
| - trade_specialties.json (16 items) |
| - library_manuals.json (12 items)   |
| - autopsy_procedures.json (9 items) |
+-------------------------------------+
```
