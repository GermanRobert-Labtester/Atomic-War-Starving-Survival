# Continuity Wave 2 — Audit Index (Plans 20–24): *The Bunker Machine*

**Snapshot audited:** `ccac926e` (branch `main`, 95 uncommitted paths) · **Date:** 2026-08-31
**Gates re-run this wave:** `dotnet build Ashfall.csproj` 0 err / 0 warn ·
`dotnet test` **5303 passed / 0 failed** · `--data-integrity-selftest` **PASS, 138 catalogs,
5563 ids, 0 errors**.
**Predecessor:** [Wave 1 — Plans 15–19](Wave1_Continuity_Audit_INDEX.md) (*the story machine*:
choices, consequences, honest navigation, a derived ending).

Wave 1 asked *"does anything the player do matter narratively?"* — mostly, no.
Wave 2 asked *"does anything the player do matter physically?"* — and found the bunker's causal
loop is held together by hardcoded constants.

---

## Wave 2 findings: the 10 highest-impact physical-continuity gaps

| # | Gap | Category | Severity | Why it matters to the player | Closes in |
|---|---|---|---|---|---|
| 1 | **Radiation exposure is not environmental** — `ZoneRadLevel` has exactly one writer, `src/Host/SurvivorsHostSession.cs:240`: `state.Id == "survivor_gunner_mikhail" ? 40f : 2f`. Weather, sector contamination, and position never enter the dose math | core loop / balance | **critical** | In a game about a nuclear winter, the central threat is a literal per-survivor constant. "Where do I send them" is not a radiation question | 20A |
| 2 | **Protective gear never wears out** — `Inventory.FillWornGear` (`Inventory.cs:951`) sets `DegradeRate = 0f` **and** hands `RadiationSystem.DegradeWornGear` a throwaway copy (`:189`, rebuilt each tick at `SurvivorsHostSession.cs:248`). `items.json` authors `durability` + `radProtection` on 166/212 items | system connection / balance | **critical** | The genre's core scarcity — *"your mask is failing"* — does not exist. A day-1 gas mask is a day-300 gas mask | 21A |
| 3 | **Eating does nothing** — the single correct consume API is called with every effect callback omitted (`src/Host/InventoryHostSession.cs:303` → `Inventory.Consume(def, therapeuticScale: …)`, so `applyNeed/applyRadCleanse/applyIodine/applyContamination` are all null) | core loop | **critical** | Consuming food, water, iodine, anti-rad, or a contaminated meal removes the item and changes nothing | 22A |
| 4 | **The only EAT/DRINK buttons play a different game** — `HoldfastTerminalPanel.cs:392,396` → `HoldfastRuntimeSession.cs:233–239`: draws from the **trade ledger** stock, applies a hardcoded `-30f` hunger (ignoring `hungerRestore` 40/30/…), to `PlayerSurvivorId` only, from a literal item list (`:212`) | system connection / UX | **critical** | The crew can't be fed, tins are wrong, and the store that empties isn't the one the panels show | 22A, 22B |
| 5 | **Power is decorative** — `PowerGridSystem.IsRoomPowered` has one gameplay reader (`SumpFloodingSystem.cs:171`) plus schedule injection; water plant, air handling, grow lamps (`item_grow_lamp`, `LightHoursPerDay`), refrigeration, ward, and heating all run at full output in a blackout. Two systems keep private power booleans that never ask the grid (`SaltMineExtractionSystem.cs:48`, `LibraryStudySystem.cs:31`) | system connection | **critical** | The bunker is a machine that doesn't switch off; "can I afford the lights?" is not a decision | 23A, 23B |
| 6 | **The duty roster never asks the bodies** — `ValidateAssign` (`DutyRosterAssignmentEngine.cs:59–74`) checks only role/id/row/status; **zero** references to health, needs, afflictions, sickness or quarantine anywhere in `DutyRoster/*.cs` | core loop / progression | **critical** | You can assign a starving, irradiated, quarantined person to precision night work and the game won't object — which makes people an abstraction | 24A |
| 7 | **The good meal pipeline is never served** — `KitchenNutritionSystem` is complete (bill consumption `:114`, spoilage `:242–246`, `ServeMeal` `:204` → `_needs.Modify` `:221`), ticks daily (`Main.ExpandedShelterSystems.cs:248`), and has 0 callers for `ServeMeal`/`SetCellar`/`SetRefrigeration` | system connection / content | **important** | Cooking, cellars, refrigeration and spoilage exist as text; meals never reach a bowl | 22B |
| 8 | **Sleep, fatigue, morale are unsteerable** — `NeedsSystem` accepts exactly one external input (`_isNearHeatSource`, `:73`); `ShelterScheduleSystem` models `fatigueRecoveryModifier`, `lightingDemand`, `SleepAssignment`, and its only reader is a label (`ShelterSchedulePanel.cs:113`); `DutyRosterRow.lastSleptDay` is written and never read | system connection | **important** | Six needs move on a fixed decay curve no decision touches — no rest, no routine, no reward for it | 24B |
| 9 | **Skill cannot reach work** — `WildlifeTrappingSystem.SetHunterSkill` (`:110`) has **0 callers**, so trapping runs forever at its 0.5× floor and per-quarry `minSkillLevel` gates are dead; `ApprenticeshipHostSession.cs:20` constructs its own `SkillProgressionSystem` | progression / system connection | **important** | Training, apprenticeship and years of practice change nothing about output | 24B, 24C |
| 10 | **Illness is a side room** — `SickListSystem` is reachable only through the dose ledger session; `MedicalWardSystem` models patients, bed classes, and staffing with no roster seam; quarantine events exist and remove nobody from duty | system connection / content | **important** | Getting sick costs the player nothing structurally — no vacated shift, no re-split rations, no carer burden | 24C |

### Honesty corrections found while auditing (stale docs to fix — Wave 2, 24C step 13)

| Claim in the repo | Reality @ `ccac926e` |
|---|---|
| `AGENTS.md` **H5** "Utility AI forked — Core vs Godot host (`src/UtilityAI/`)" | Resolved: `Assets/Ashfall.Core/UtilityAI/` holds the 4 source files; `src/UtilityAI/` holds only `UtilityAiPanel.cs` |
| `AGENTS.md` **H11** "JournalSystem core behaviour still untested" | Resolved: `Ashfall.Core.Tests/JournalSystemTests.cs` **+** `JournalSystemCoreBehaviorTests.cs` |
| `Greenhouse/ApicultureSystem.cs:48` "*water/power … constrain output*" | Zero `power` references in the file — a documented mechanic that isn't implemented |
| `docs/data/DATA_GAP_AUDIT.md` `questline_master.json` "ORPHAN — no C# loader" | Loaded: `src/Main.Application.cs:392`, held at `src/Main.cs:42`, 362 defs, `QuestlineSystem` consumer |

---

## Plans in this wave

| Plan | Title | Closes | DoD in one line |
|---|---|---|---|
| [20](Plan_20_Exposure_Environmental_Weather_Zone_Position.md) | Exposure Is Environmental | 1 | Dose is a function of where you are, what the shelter is made of, and what the sky is doing. |
| [21](Plan_21_Protection_Wears_Out_Condition_Ledger.md) | Protection Wears Out | 2 | Gear is a consumable with one condition number that everything reads. |
| [22](Plan_22_One_Food_Authority_Consumption.md) | One Food Authority | 3, 4, 7 | One consume call does what the data says, for the crew, from the real larder. |
| [23](Plan_23_Power_Is_A_Dependency_Life_Support_Loop.md) | Power Is a Dependency | 5 | Every watt is accounted for; shedding load is a player decision with a way out. |
| [24](Plan_24_People_Not_Abstraction_Health_Duty_Ledger.md) | People Are Not Abstraction | 6, 8, 9, 10 | Fitness gates duty, seven outside forces move the needs, and a death reorganises the shift. |

---

## Cross-wave ordering

```
Wave 1 (narrative continuity)                Wave 2 (physical continuity)
15A moral valve ─┐                           20A environmental dose ─┐
16A honest nav   ├─► 19A derived ending     22A real consumption ────┼─► 23A watts are a dependency
15C liveness gate┘                          24A fitness verdict ─────┘        │
        │                                                                     ▼
        └──────────── feeds 17A event vocabulary, consumed by ──────► 20B/21A/22B/23B/24B/24C
```

**Hard prerequisites:** 17A (attribution vocabulary) should land before 20B, 22A, 23B, 24B — the
physical causality this wave creates is invisible to the player without it. 16B before 24B step 10
(same "fresh system instance" defect, different file).

> **Wave 4 update:** every "17A vocabulary" reference in Plans 20–24 now means
> **`DayEventKinds` from [Plan 31](Plan_31_Event_Layer_Semantic_Kinds_No_Silent_Drops.md)**. Wave 1's
> 17A premise ("only one producer") was disproved — the briefing's problem is a dropped-vocabulary
> `switch`, not missing producers. The plans below are otherwise unaffected.

**If capacity allows only three tasks:** **22A → 24A → 20A.** Eating works, the roster stops
lying, and the fallout decides itself. Everything else in this wave compounds on those three.

## Metrics to report at wave close

1. Distinct `ZoneRadLevel` values fed to survivors at runtime: **2 (both literals) → f(position, weather)**
2. `EquippedItem.CurrentDurability` after 100 h hot-zone exposure: **unchanged → decremented**
3. Consumed-item effects applied: **0 of 4 callbacks → 4 of 4**
4. Systems consulting `PowerGridSystem`: **2 → every row in the load table marked non-decorative**
5. `ValidateAssign` blocking codes: **4 → 8+** (condition-aware)
6. Callers of `ServeMeal`: **0 → ≥1** · callers of `SetHunterSkill`: **0 → the trapping day owner**
7. Stale claims in `AGENTS.md` / audits corrected: **4 → 0**

## Deferred to Wave 3 (production continuity — now planned)

These items are picked up by **[Continuity Wave 3 — Plans 25–29, *Ship It Intact*](Wave3_Continuity_Audit_INDEX.md)**
(localization layer, export/boot gate, test fidelity & coverage, orchestration registration,
documentation truth — including three CI gates that were red at the time of this audit):

| Item | Evidence |
|---|---|
| **No localization layer at all** | `grep "TranslationServer\|tr(" src/` → **0 hits**; no `.csv`/`.po`/`.translations`; `project.godot` has no locale/translation entries. 164 UI files with inline English + diegetic text in the authority. Store release blocker (`ashfall-localize`, `ashfall-string-extractor`) |
| **Selftest fidelity drift** | selftests construct `new InventoryHostSession()` (`InventorySaveSelfTest.cs:12`, `PanelBindLifecycleSelfTest.cs:211`, `HostCli.PanelTests.cs:627…`) which seeds the **hardcoded demo catalog** (`InventoryHostSession.cs:30`), while the live campaign loads `items.json` via `Create(_dataDir)` — gates pass against an item set the shipped game does not use |
| **Export/ship gate** | `export_presets.cfg` defines Linux + Windows presets; no CI export/PCK-includes-data gate recorded in `docs/CI.md`'s 14 gates (`ashfall-export-build` exists as a skill, not a pipeline) |
| **Perf budget enforcement** | `artifacts/runtime-scale-results.json`: `day_advance_30d` median **0.609 s**, p95 **1.145 s**, max **1.265 s** over 5 iterations — recorded, not gated. Per-tick list allocations from gap 2/3 fixes land here |
| **Main.cs decomposition (H7)** | still the accepted end-state; Wave 1's 15C + Wave 2's ownership tables make the seams explicit so the split becomes mechanical rather than architectural |
| **Doc-atlas pass** | `ashfall-docs-atlas` over 119 `docs/*.md` + 14 root `*.md` + 2 plan folders with three competing numbering schemes (14, 15–19, 131–138) |
