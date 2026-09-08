# Final Wish Content Utilization Report

**Document:** `docs/survivors/FINAL_WISH_CONTENT_UTILIZATION.md`

---

## 1. Catalog Utilization Baseline

In `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`:
- `final_wishes.json` is registered under `consumerSystems = new[] { "FinalWishSystem" }`.
- In `artifacts/content-utilization-baseline.json`, `final_wishes.json` is tracked with active gameplay consumption.

---

## 2. Plan 65 Utilization Audit

- **Authored Definitions:** 30 complete wish objects in `final_wishes.json`.
- **Selectability:** 30/30 wishes map to unique archetypes; all 22 new archetypes are present in `survivors.json`.
- **Reachable Objectives:** All step objectives reference verified canonical items (14/14), locations (4/4), NPCs (6/6), or skills (3/3).
- **Orphan / Dead Content:** 0 unparsed, 0 dead wishes.
