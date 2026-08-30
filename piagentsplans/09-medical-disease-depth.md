# Plan 09 — Medical & Disease Depth: Pathogens, Pharma Purpose & Palliative Care

> **Theme:** The medical loop is mechanically rich (4 epidemic vectors, 6 ARS phases, 7-phase
> pharma lab, 5 ward bed classes, 4 dependency classes) but its *content* is the thinnest in
> the game: **7 diseases** total. This plan deepens medicine without adding parallel systems.
>
> **Key evidence:** `disease_catalog.json` = 7 diseases; `pharma_recipes.json` = 25 recipes
> (already healthy — do NOT batch more blindly); `ChemicalDependencySystem`,
> `RespiratoryDegenerationSystem`, `VigilStateMachine`, `SickListSystem` all live.

---

## Task 9A — Exotic pathogen catalog (7 → 15 diseases)

**Goal:** Double the disease catalog with grounded, post-exchange pathogens that exercise the
four existing transmission vectors and the ward/triage pipeline.

**Files:** `Assets/StreamingAssets/Data/disease_catalog.json`,
`Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` (read-only schema reference),
possibly `disease_catalog`-adjacent item entries (cures/vectors in `items.json`).

**Substeps:**
1. Read `DiseaseSystem.cs` + `DiseaseCatalog.cs` for the full disease schema (vector, incubation, phases, lethality, treatment windows).
2. Map the 7 existing diseases to their vectors to find uncovered design space (e.g. spore vector likely underused).
3. Author 8 new diseases across: 2 waterborne (post-flood dysentery strain, well-borne parasite), 2 airborne (silo lung, bunker flu variant), 2 blood/contact (septic rust-wound fever, needle-borne), 2 spore/fungal (deep-excavation mold lung — pairs with Plan 11A excavation).
4. For each: 3-phase progression, a treatment path using **existing** pharma outputs, and one diagnostic tell (symptom text the player learns to read).
5. Ground everything in real pathology; no zombies, no fantasy plagues (tone rules).
6. Tie 2 diseases to world triggers (flood aftermath, deep dig) so they arrive as *events*, not random draws.
7. Cross-check treatment item ids against `items.json` + `pharma_recipes.json` outputs.
8. Run `--data-integrity-selftest` 0 errors.
9. Add xUnit coverage: each new disease parses, progresses, and responds to its treatment.
10. Run the disease headless selftest gate (`godot-asset-gate.sh` includes disease gate) green.

**Next steps:** disease-specific radio warnings (Plan 07 audio hook); regional outbreak events.

---

## Task 9B — Chemical-dependency & detox clinic depth

**Goal:** Make the 4-class `ChemicalDependencySystem` a managed-care loop (detox protocols,
relapse risk, withdrawal triage) instead of a passive debuff — content + config on the
existing system.

**Files:** `chemical_dependency_items.json`, `disease_catalog.json` (withdrawal states if
modeled as afflictions), read-only `ChemicalDependencySystem.cs`, `PharmaLabSystem.cs`.

**Substeps:**
1. Read `ChemicalDependencySystem` to map existing knobs: tolerance, withdrawal severity, craving triggers, relapse rules.
2. Inventory `chemical_dependency_items.json` — note how few substances/remedies exist.
3. Author 4–6 detox-support items (taper kits, substitution therapy, anti-craving) using existing pharma-lab outputs where possible.
4. Author withdrawal protocols as staged care plans (day-by-day dose-down schedules) consumable by the medical ward.
5. Add relapse triggers keyed to existing stress sources (guilt spikes, combat trauma, ration cuts) — **only if the system already reads them**; otherwise file as a small Core extension task, don't hack it.
6. Write 4–6 survivor-specific dependency backstories into narrative docs (who came in dependent, on what, why).
7. Ensure every dependency class has at least one viable non-abstinence management path (moral texture: maintenance vs cold turkey).
8. Validate ids + data-integrity selftest.
9. xUnit: dependency accrual, withdrawal staging, relapse trigger, treatment effect.
10. Balance check with `ashfall-equipment-balance`/`ashfall-balance-sim` (coupled variables: health, morale, labor) — cross-tool QA applies.

**Next steps:** dependency-driven trade demand (pharma becomes diplomatic currency); withdrawal during expeditions.

---

## Task 9C — Palliative care, vigils & end-of-life protocol

**Goal:** Give the `VigilStateMachine`, caregiving, and sick-list triage a humane endgame:
comfort care for the terminal, so death is a *managed, meaningful* event — reinforcing the
final-wish content from Plan 06A.

**Files:** read-only `VigilStateMachine.cs`, `CaregivingSystem.cs`, `SickListSystem.cs`,
`MemorialSystem.cs`; data in `final_wishes.json`, possibly new comfort-care items in `items.json`.

**Substeps:**
1. Read the vigil + caregiving state machines; document what comfort actions already exist vs. are implied.
2. Author comfort-care items (analgesia protocol, sedative comfort dose, familiar-object comfort) mapped to existing pharma/medical items.
3. Define palliative triage band behavior: when prognosis = terminal, what does the ward surface? (confirm `SickListSystem` exposes this band; if not, file a micro Core extension).
4. Wire vigil events to family/bonded survivors (`TraumaBondSystem`, `SurvivorRelationsSystem`) so a vigil affects the living.
5. Author 6 vigil vignettes (short texts surfaced at the bedside) in the restrained house voice.
6. Connect fulfilled final wishes (06A) to a measurable comfort/peace modifier on the vigil.
7. Author 3 memorial outcome variants (burial, memorial wall entry, ash scattering) feeding `MemorialSystem`.
8. Ensure grief cascade (`SurvivorRelationsSystem`) responds to *how* a death was managed (peaceful vigil vs unattended).
9. Validate + data-integrity selftest.
10. xUnit: vigil state transitions, comfort modifiers, grief deltas by death quality; save round-trip.

**Next steps:** memorial-wall decor (white space #18) displays vigil outcomes; leadership
morale effect of "good deaths" vs "bad deaths."
