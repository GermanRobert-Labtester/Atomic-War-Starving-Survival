# Plan 26 Regression Matrix

> **Document Status:** Authoritative Regression Prevention Matrix
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Regression Guard Verification

| Subsystem / Surface | Potential Regression Risk | Guard & Verification Mechanism | Status |
|---|---|---|---|
| **Research Node IDs** | Changing snake_case IDs breaking saves | Locked `knowledge_*` ID constants and parity tests | PASS |
| **Research DAG** | Introducing cyclic dependencies or orphan prereqs | `ResearchKnowledgeCatalogLoader.ValidateDag()` | PASS |
| **Skill Progression** | Altering XP thresholds or discipline IDs | 1:1 parity tests against baseline defaults | PASS |
| **Trade Specialty** | Breaking existing save records or milestone counts | `TradeSpecialtySystem` 3-milestone rule & schema preserved | PASS |
| **Library Manuals** | Field deserialization failure due to casing mismatch | `[JsonPropertyName]` mappings on `ManualDefinition` | PASS |
| **Autopsy Procedures** | Dropping required tools/consumables on load | `[JsonPropertyName]` mappings on `AutopsyProcedure` | PASS |
| **Catalog Integrity** | Unknown IDs causing `--data-integrity-selftest` failure | Full snake_case validator pass | PASS |
