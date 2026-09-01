# Plan 10 — Combat & Expedition Depth: Bestiary, Armory & the Fleet

**Status:** COMPLETE & FULLY INTEGRATED
**Closeout Report:** [`docs/combat/PLAN10_COMPLETION_REPORT.md`](../docs/combat/PLAN10_COMPLETION_REPORT.md)

---

## 1. Executive Summary

Plan 10 integrates combat bestiary, warlord doctrines, armory, ammunition, expedition vehicles, and deep-coast dive sites into a cohesive, deep gameplay layer without replacing the proven Core architectures. All authored content is strictly data-authority-backed, engine-agnostic, and verified through both .NET tests and Godot headless self-tests.

---

## 2. Key Deliverables & Targets Achieved

- **Enemy Bestiary:** 10 authored combatants (`combat_catalog.json`) — 6 fauna/mutant (`combatant_burrower_mite`, `combatant_spore_hound`, `combatant_armored_boar`, `combatant_feral_mutt`, `combatant_pale_crawler`, `combatant_chrome_loper`) with distinct AI stances and moves, plus 4 human archetypes (`combatant_conscript_levy`, `combatant_warlord_veteran`, `combatant_flotilla_marine`, `combatant_desperate_scavenger`) with non-combat exits (surrender, bribery, retreat).
- **Warlord Doctrines (4 → 8):** 8 strategic doctrines (`warlord_doctrines.json`) — 4 core + 4 expanded (`warlord_doctrine_besiege` / Brenner, `warlord_doctrine_traffic` / Mireles, `warlord_doctrine_ashprophet` / Asha, `warlord_doctrine_procedure` / Okov) with live response actions and dynamic transition signals.
- **Armory Expansion (5 → 15):** 15 weapons across improvised, civilian, military, and degraded relic tiers.
- **Ammunition Expansion (5 → 14):** 14 ammo types across standard calibers, hand-loaded specialty loads (.357 JHP, 12ga Buckshot, .308 Incendiary, 5.56 Subsonic), and improvised charges.
- **Vehicle Fleet (3 → 8):** 8 expedition vehicles (`vehicles.json`) with specialized logistics profiles.
- **Deep-Coast Dive Sites (4 → 12):** 12 dive sites (`dive_sites.json`) with tiered noise floors, oxygen budgets, and hazards.

---

## 3. Verification

- `dotnet test Ashfall.Core.Tests` -> 5,317 passed, 0 failed
- `godot --headless --path . -- --data-integrity-selftest` -> 0 errors across 138 catalogs (5,563 IDs)
- `godot --headless --path . -- --content-utilization-selftest` -> CI Gate PASS (413 catalogs)
- `godot --headless --path . -- --scene-binding-selftest` -> 22/22 scenes bound
- `python3 scripts/ci/scene-lint.py` -> 26 scenes checked, 0 errors
- `python3 scripts/ci/generate-audio-catalog.py --check` -> 74 cues in sync
