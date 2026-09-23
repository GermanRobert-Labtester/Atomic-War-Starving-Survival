# PLAN-FOOD-CUISINE-39 — Appendix A: Orphan Dossiers (food & cuisine)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **4 host-unreachable authorities** wired by this plan's
packages (see the parent plan's seam map; the same systems appear in
`PLAN-ORPHAN-SEAL-01` Appendix A with wave assignment).

**Use:** each dossier lists the authority, file, known tests, candidate
catalogs, and the parent-plan mechanic that consumes it. A package claim covers
one or more systems end-to-end (host path, day owner if stateful, save path,
one player surface, focused tests).

## Dossiers

### 01. `FoodTypeSystem`
- **File:** `Kitchen/FoodTypeSystem.cs` · **Types:** `FoodTypeSystem`
- **Known tests (2):** `Kitchen/Plan196FoodTypeIntegrationTests.cs`, `Kitchen/Plan22_40FoodIdentityIntegrationTests.cs`
- **Candidate catalogs:** `food_preservation.json`, `food_types.json`
- **Parent-plan mechanic:** Preservation
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `CommonTableRationingEngine`
- **File:** `Nutrition/CommonTableRationingEngine.cs` · **Types:** `CommonTableRationingEngine`
- **Known tests (1):** `Nutrition/CommonTableRationingEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Rationing
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `CookingSystem`
- **File:** `Cooking/CookingSystem.cs` · **Types:** `CookingSystem`
- **Known tests (1):** `Cooking/Plan136WildlifeCookingIntegrationTests.cs`
- **Candidate catalogs:** `recipes_cooking.json`
- **Parent-plan mechanic:** Kitchen & menus
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 04. `OilseedPressingEngine`
- **File:** `Farming/OilseedPressingEngine.cs` · **Types:** `OilseedPressingEngine`
- **Known tests (1):** `Farming/OilseedPressingTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Specialty production
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
