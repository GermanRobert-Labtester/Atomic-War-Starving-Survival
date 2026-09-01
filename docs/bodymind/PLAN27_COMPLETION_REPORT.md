# Plan 27 Completion Report — The Body & the Mind: Dose Registers, Autopsies & Psychological Contamination

---

## 1. Executive Summary

Plan 27 has been successfully executed, reconciling and expanding ASHFALL's medical, forensic, and psychological interior world without inventing competing parallel health, radiation, grief, trauma, or sanity systems.

Key accomplishments:
- **Dose Registers Society Depth:** Expanded `dose_quests.json` from 4 to **12 fully authored, reachable questlines** featuring the four established NPCs (`dr_irina_vel`, `wyn_omah`, `piet_abar`, `saria_voss`) with triage-by-dose moral dilemmas. Expanded `dose_items.json` from 5 to **9 items** (including `item_calibrated_dosimeter`, `item_forged_clean_bill_chit`, `item_chelation_decorporation_course`, `item_shielded_badge_case`) and `dose_locations.json` from 3 to **5 locations** (`loc_the_register_hall`, `loc_the_screening_station`).
- **Physical vs. Administrative Invariant:** Established strict separation between biological physical radiation (`RadiationSystem`) and administrative kept records (`DoseLedgerSystem`). Forged chits and clerical adjustments change institutional clearances and checkpoint access without mutating true biological exposure.
- **Autopsy & Cause-of-Death Forensics:** Validated 9 comprehensive procedures in `autopsy_procedures.json` with canonical upstream finding provenance, research unlocks (`knowledge_radiation_basics`, `knowledge_field_trauma_surgery`, `knowledge_pharmacology_synthesis`), disease-intel hooks, and 3 authored non-natural forensic cases (poisoning, staged cave-in, concealed smothering) producing evidence without procedural random murder generation.
- **Psychological Contamination & Restrained Dread:** Standardized Scope C (contextual disaster/deep-place exposure with downstream handoff to existing trauma, flashback, and insomnia systems). Added a 5-stage qualitative threshold model, 6 restrained sensory dread texts, companion grounding, and safe shelter rest recovery.

---

## 2. Baseline vs. Final Metrics

| Metric | Baseline | Final | Delta |
| :--- | :--- | :--- | :--- |
| **Dose Quests** | 4 | 12 | +8 (+200%) |
| **Dose Items** | 5 | 9 | +4 (+80%) |
| **Dose Locations** | 3 | 5 | +2 (+67%) |
| **Dose Register NPCs** | 4 | 4 | 0 (Preserved voices & continuity) |
| **Autopsy Procedures** | 3 (pre-P26) / 9 | 9 | Fully validated & integrated |
| **Forensic Evidence Cases** | 0 | 3 | +3 authored cases |
| **Psychological Contamination Locations** | 5 | 5 | Reconciled with maritime/disaster |
| **Restrained Dread Texts** | 0 | 6 | +6 sensory atmosphere texts |
| **Authored IDs in Data Tier** | 6,710 | 6,804 | +94 valid IDs |
| **xUnit Unit & Determinism Tests** | 5,630 | 5,653 | +23 tests (All PASS) |

---

## 3. Authority & Invariant Mapping

1. **Radiation Exposure:** `Ashfall.Core.Radiation.RadiationSystem` owns biological acute and lifetime rem/mSv.
2. **Administrative Records:** `Ashfall.Core.DoseLedgerSystem` owns the kept document, dosimeter tags, calibration status, and forged clean-bill classifications.
3. **Dissection & Findings:** `Ashfall.Core.AutopsySystem` owns tools, consumables, durations, risks, and observations, routing knowledge into `ResearchSystem` and pathogens into `DiseaseSystem`.
4. **Contextual Dread:** `Ashfall.Core.Maritime.PsychologicalContaminationSystem` tracks site exposure and action exclusions, delegating sleep disruption to `GuiltInsomniaSystem` and stress to `CombatTraumaSystem`.

---

## 4. Verification Evidence

| Verification Command | Expected Outcome | Verified Result |
| :--- | :--- | :--- |
| `dotnet test Ashfall.Core.Tests` | 0 failed | **PASS** — 5,653 passed, 0 failed, 0 skipped (18s) |
| `godot --headless --path . -- --data-integrity-selftest` | 0 errors | **PASS** — 0 findings across 153 catalogs (6,804 authored IDs) |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS | **PASS** — 118 gameplay-consumed catalogs, 0 regressions |
| `godot --headless --path . -- --scene-binding-selftest` | 22/22 passed | **PASS** — 22/22 scenes passed |
| `python3 scripts/ci/scene-lint.py` | 0 errors | **PASS** — 26 scenes checked, 0 errors, 0 warnings |
| `godot --headless --path . -- --dose-uitest` | Exit 0 | **PASS** — surface, npcs, booking, triage, clinical, vigil all green |
