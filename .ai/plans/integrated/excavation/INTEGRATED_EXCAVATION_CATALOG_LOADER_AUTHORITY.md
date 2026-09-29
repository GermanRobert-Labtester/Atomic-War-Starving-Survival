# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-27** — verified zero production consumers before wiring; built, gated, and reachability-proved.

---

## `ExcavationCatalogLoader`

`ExcavationCatalogLoader` reads `excavation_sites.json` (20+ authored sites with
`max_depth_meters`, `structural_risk`, `required_progress`, `loot_table`,
`relic_reward_id`, depth bands, hazard type). `ExcavationHostSession.AddSite`
had **no production caller at all** — only HostCli probes — so the entire
excavation system was unreachable gameplay.

**What shipped.**

- **`src/Main.ShelterSocial.cs`** `SetupExcavation()` — after restore, authored
  sites are registered through the existing `AddSite` seam. Registration skips
  any `siteId` already present in the save, and `AddSite` itself refuses
  duplicates, so the step is idempotent across save/load and cannot duplicate a
  restored site. A load failure is reported, never swallowed.

**Honest note on `room_blueprint_id`:** the authored def has no
`room_blueprint_id` column, so the authored `site_id` is used for both fields.
No blueprint id was invented.

**Evidence.** `grep -rlw ExcavationCatalogLoader src/ --include=*.cs | grep -v HostCli`
→ `src/Main.ShelterSocial.cs`. Passing: `ExcavationSitesCatalogTests`,
`ExcavationSystemTests`.

**Rule 5:** `ExcavationSystem` remains the owner of site state; the loader only
feeds it. No parallel site registry.

---
