# Plans 162–165 — Implementation Log

Journal per ashfall-implement discipline. Companion authority map:
`PLANS_162_165_RECONNAISSANCE.md`. Spec: pasted flagship integration plan
(Plans 162–165), 2026-09-05.

---

## Phase A — Reconnaissance

Status: PASS (pending baseline completion)

Changed:
- `docs/plans/PLANS_162_165_RECONNAISSANCE.md` (new — full authority map)

Result:
- 5 audits complete (agriculture, defense, psychology, wildlife, shared infra).
- 9 documented divergences from plan text; none architecture-invalidating.
  Composition decisions: agriculture layers on GreenhouseSystem; defense
  composes PerimeterDefenseSystem at the Main.Muster raid seam; psychology
  wires the existing Sanatorium as therapy authority; wildlife ecosystem
  extends WildlifeMigrationSystem (single population store).

Baseline:
- (recorded below when the background run completes)

Divergences: see recon doc §F.

---

## Phase B — Shared contracts

Status: NOT STARTED

---

## Phase C — Plan 162 (AgricultureSystem)

Status: PASS (Core + data + host + UI + tests + gates)

Changed:
- `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` (new — plot strain layer, medium/toxicity, pests, authored mutations, compost, harvest enrichment, one-shot narratives; greenhouse remains growth authority, ticked exactly once inside)
- `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs` (new — DTOs + loader + deterministic validator incl. compost value-loop rule)
- `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` (new — 14-day diet window, 5 diversity categories, deficiency events; consequences applied by host via NeedsSystem.Modify)
- Data: `crop_strains.json` (10 strains incl. 2 authored hardy variants, 3 pests, 2 lossy compost recipes), `nutrition_profiles.json` (14 food profiles), `agriculture_items.json` (compost humus + pest treatment dust; registered in ItemCatalogLoader)
- Save: `agriculture` section (`agriculture_save.json`, combined agri+nutrition envelope, checksum + legacy fallback); SaveOrchestrator Save/Restore hooks
- Host: `AgricultureHostSession` + `AgricultureSaveStore` + `Main.Plans162_165.cs` (Setup/Save/actions/environment snapshot); `GreenhouseFoundryDayOwner` now routes the greenhouse tick through agriculture (legacy fallback kept); `InventoryHostSession.OnConsumed` hook → RecordMeal; deficiency morale pressure ≤ 3/day via NeedsSystem
- RNG: per-day forks of `agriculture.pest` / `agriculture.mutation` (stateless continuation)
- UI: `FarmingPanel` (bound, plot-identity selection, real commands with costs, compost section, LastEvent, Escape close) + `farming` registry route (Live) + ConfigureActions + expandedIds + FARMING dashboard nav
- Selftest: `--agriculture-selftest` (11 gates)
- Utilization: crop_strains/nutrition_profiles/agriculture_items registered GAMEPLAY_CONSUMED

Tests:
- AgricultureSystemTests 21 + AgriculturePersistenceTests 8 = 29/29 PASS (`dotnet test -c Release --filter Agriculture`)

Result:
- growth composes power(light)+weather(ash/rad) → greenhouse phases; water bands accumulate plot toxicity once per watering; mutation drawn once per lifecycle at maturity only when pressure ≥ threshold (no draw otherwise — budget test); pests deterministic; compost lossy & atomic; first-harvest/blight narratives one-shot; save/restore continuation equals uninterrupted run; old saves (bare state) load with defaults.

Divergences:
- Plan's 7-phase Growth enum mapped onto existing GreenhouseStage (no parallel phase machine).
- Catalog named crop_strains.json; strains profile EXISTING CropCatalog seeds only.
- Water bands: Clean/Marginal/Unsafe derived from items + WaterTreatmentSystem.incomingContaminationLevel.

Remaining: Phase G cross-plan (weather already feeds env snapshot; raid/wildlife hooks come with Plans 163/165).

Baseline record (Phase A):
- Core build PASS; host build PASS; agriculture tests 29/29; data-integrity PASS (262 catalogs, 0 errors); utilization gate PASS; PanelRouteGateTests 19/19; bridge + save-store-checksum selftests PASS.
- FULL `dotnet test` (Debug) could NOT complete at baseline: two independent full-suite runs (mine + a concurrent stream's) both pegged a testhost at ~99% CPU >1h without finishing — consistent with the 2026-09-05 UI-audit verification record ("Full xUnit run: INCOMPLETE/UNKNOWN"). Filtered Release-config runs used for phase gates.

---

## Phase D — Plan 163 (DefenseSystem)

Status: NOT STARTED

---

## Phase E — Plan 164 (PsychologicalArcSystem)

Status: NOT STARTED

---

## Phase F — Plan 165 (WildlifeEcosystemSystem)

Status: NOT STARTED

---

## Phase G — Cross-plan integration

Status: NOT STARTED

---

## Phase H — Content closure

Status: NOT STARTED

---

## Phase I — Persistence/replay

Status: NOT STARTED

---

## Phase J — Full CI

Status: NOT STARTED
