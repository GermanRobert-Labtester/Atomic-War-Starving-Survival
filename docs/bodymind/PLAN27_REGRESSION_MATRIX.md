# Plan 27 Regression Matrix & Verification Checklist

| Subsystem / Gate | Required Standard | Target Metric | Verification Command |
| :--- | :--- | :--- | :--- |
| **Dose Catalogs** | All 12+ quests, 9 items, 5 locations, 4 NPCs parse and validate | 0 catalog errors | `dotnet test Ashfall.Core.Tests --filter Dose` |
| **Dose Forgery Invariant** | Forged chits and overrides alter ledger state, never physical dose | 100% test pass | `dotnet test Ashfall.Core.Tests --filter DoseLedgerSystemTests` |
| **Autopsy Procedures** | 9 procedures, required tools/skills, canonical findings, research grants | 100% test pass | `dotnet test Ashfall.Core.Tests --filter Autopsy` |
| **Forensic Cases** | 3 non-natural death cases produce valid evidence without RNG murderer generation | 100% test pass | `dotnet test Ashfall.Core.Tests --filter Forensic` |
| **Psychological Contamination** | 5 disaster locations produce contextual exposure; no sanity meter duplication | 100% test pass | `dotnet test Ashfall.Core.Tests --filter Psychological` |
| **Data Integrity Self-Test** | All 153 catalogs validate across all 5 tiers | 0 errors | `godot --headless --path . -- --data-integrity-selftest` |
| **Content Utilization** | Dose and Autopsy catalogs recognized as GAMEPLAY_CONSUMED | Gate PASS | `godot --headless --path . -- --content-utilization-selftest` |
| **Scene Binding Self-Test** | All UI panels bind correctly to host contracts | 22/22 passed | `godot --headless --path . -- --scene-binding-selftest` |
| **Scene Lint** | Production scenes pass tree structure and binding lint | 0 errors | `python3 scripts/ci/scene-lint.py` |
| **Dose UI Test** | Dose register surface, sick list triage, cohort, voluntary registers execute clean | Exit 0 | `godot --headless --path . -- --dose-uitest` |
