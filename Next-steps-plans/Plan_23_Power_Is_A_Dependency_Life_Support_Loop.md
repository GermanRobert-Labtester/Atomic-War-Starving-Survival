# Plan 23 — Power Is a Dependency: The Bunker Runs on Watts

> **Wave:** Continuity Wave 2 — *The Bunker Machine*
> **Depends on:** 22B (cold storage), 20B (filtration/ventilation), 24 (lighting and heat change
> people). Consumed by Plan 24C (a blackout is a stress event).
>
> **Theme:** `PowerGridSystem` is a competent little simulator — generation, draw, battery, brownout
> hours, breaker trips, per-room power. **Exactly two systems in the whole game consult it.** The
> greenhouse's grow lamps, the water plant, the air filtration, refrigerated storage, the medical
> ward, and the heating loop all behave as if the generator were decorative. Two other systems carry
> their own private `isPowered` booleans that never ask the grid, and one doc-comment promises power
> constraints that don't exist. Meanwhile the *audio* already knows more about power than the
> simulation does.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| Fact | Evidence |
|---|---|
| The grid models what it should | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — `IsBrownout` (`:57`), `IsRoomPowered(roomId)` (`:72`), room allocation (`PowerGridRoom`), `brownoutHours` (`:145–190`), breaker trips after 4 h of unmet load producing a `"brownout_overload"` incident (`:191–202`) |
| **Only two consumers outside the grid** | `grep -rn "PowerGrid" Assets/Ashfall.Core src/` → `ShelterScheduleSystem.cs:56,69` (constructor injection) and `SumpFloodingSystem.cs:171` (`_powerGrid.IsRoomPowered(node.nodeId)`) — of ~128 Core systems |
| Water plant ignores power | no `PowerGrid`/watt reference in `WaterTreatmentSystem.cs` or `BrineWaterSystem.cs`; the 4-stage treatment chain runs at full output with the generator off |
| Air handling ignores power | `Assets/Ashfall.Core/StartingLevel/*` has no power input (only match is a flavour string, `StartingLevelSystem.cs:162`) — so filtration/venting continues through a blackout (this directly blocks 20B's shielding model) |
| Greenhouse ignores power, and *says* it doesn't | `Greenhouse/GreenhouseExpansionCatalog.cs:23` authors `item_grow_lamp` and `:75 LightHoursPerDay` per crop, with **no** power draw; `Greenhouse/ApicultureSystem.cs:48` doc-comment claims *"disease, radiation, **water/power**, and queen health constrain output"* — zero `power` references in the file |
| Heating ignores power | no grid reference in `ShelterThermalSystem.cs`, though it models pipe wear and temperature |
| Cold storage is unconfigured | `KitchenNutritionSystem.cs:93–100` `SetCellar` / `SetRefrigeration` have 0 callers (22B wires them; **this plan makes refrigeration actually depend on watts**) |
| Two systems keep private power flags | `Foundry/SaltMineExtractionSystem.cs:48,151,319` (`isPowered = true`, early-returns when false, its own setter) and `LibraryStudySystem.cs:31` (`requiresPower = true`) — neither consults `PowerGridSystem` |
| The generator is audible before it is simulatable | `docs/audio/SILENCE_AUDIT.md` §12: `shelter_generator` loops "while the live grid has generation and fuel; it stops on fuel loss, a zeroed generator, session replacement, or shutdown" (`ShelterAudioController`) — the *speaker* knows the grid state, the *simulation* does not |
| Fuel already competes | generator fuel, foundry, vehicles (`ExpeditionHostSession` refuel gate), and heating all consume combustible stock, so the load-shedding decision has real stakes |
| Load tiers exist as a concept | `PowerGridSystem` per-room states + `AGENTS.md` "Power Grid Load Shedding (5 Tiers)" capability entry — the tier vocabulary is there to hang consequences on |

---

## Task 23A — One power authority: every draw and every dependency goes through the grid

**Goal:** make `PowerGridSystem` the single answer to "is this room live, and how much can it
pull", and delete the private booleans.

**Files:** `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, `Foundry/SaltMineExtractionSystem.cs`,
`LibraryStudySystem.cs`, `Greenhouse/*`, `WaterTreatmentSystem.cs`, `BrineWaterSystem.cs`,
`StartingLevel/*`, `ShelterThermalSystem.cs`, `MedicalWardSystem.cs`,
`KitchenNutritionSystem.cs`, `src/Main.ShelterInfrastructure.cs`, `src/Main.ExpandedShelterSystems.cs`.

### Substeps

1. **Publish the load table first** (no code): one row per powered consumer —
   `system | room(s) | draw watts | what fails without power | grace window | restart cost`. Rows
   with no meaningful failure must be marked `DECORATIVE` and *not* wired (do not invent penalties).
2. **Introduce a tiny Core interface** — e.g. `IPowerConsumer { string RoomId; float DrawWatts;
   void OnPowerLost(int hours); void OnPowerRestored(); }` — implemented by the consumers rather
   than each holding a reference to the concrete grid. Keeps `PowerGridSystem` from becoming a
   god-object and keeps systems unit-testable with a stub.
3. **Register consumers with the grid** in a single host setup method (one place, so triad drift is
   impossible), and make registration order deterministic.
4. **Delete the private flags**: `SaltMineExtractionSystem.isPowered` and
   `LibraryStudySystem.requiresPower` become `IPowerConsumer` implementations reading the grid.
   Behaviour must be preserved step-for-step — write a before/after equivalence test first.
5. **Charge the lamps**: `GreenhouseExpansionCatalog`'s `LightHoursPerDay` and `item_grow_lamp`
   become a real draw with a real yield dependency: no power → light hours collapse → growth,
   blight resistance, and apiculture output degrade on the curves the catalog already authors
   (`:93,:95,:106`), and fix the `ApicultureSystem.cs:48` doc-comment to match reality.
6. **Charge the water plant**: treatment stages have throughput tied to power; without it, only
   the passive stage (boiling on a stove — items for which already exist) works, so thirst pressure
   becomes immediate and recoverable.
7. **Charge the air**: filtration and ventilation draw power and degrade indoor dose —
   **coordinate with 20B step 3–5** so exactly one model decides indoor atmosphere; if 20B hasn't
   landed, this substep is where the model is *created*, not duplicated.
8. **Charge the cold room**: refrigeration from 22B stops on power loss; `GetSpoilageDays` degrades
   toward cellar/none, with a thermal-mass grace window (a closed fridge holds for hours, not
   seconds).
9. **Charge the ward**: lighting, powered procedures, and refrigerated pharmaceuticals
   (22C's storage) depend on power; the ward's reservations must fail honestly, not silently.
10. **Heat is a draw, not just a state**: `ShelterThermalSystem` fans/pumps consume watts, so
    "keep warm" and "keep the lights on" become the same decision in winter — the single best
    pressure this plan can create.
11. **Cap total draw against generation + battery** so over-subscribing the grid is possible and
    forces prioritisation; expose the deficit number in the panel.
12. **Tests**: per-consumer failure behaviour, the equivalence tests from step 4, a load-shedding
    priority test, save round-trip of consumer state, determinism of brownout ordering.
13. **Run the checklist** + `bash scripts/ci/triad-drift-gate.sh` (new consumers mean new
    Setup/Save/Flush obligations).

**DoD:** every watt is accounted for by exactly one authority, and no system has its own opinion
about electricity.

---

## Task 23B — Load shedding as a player decision

**Goal:** the player, not an algorithm, decides what loses power — with a legible, reversible,
consequential interface built on the tiers that already exist.

**Files:** `PowerGridSystem.cs` (tiers/rooms), `src/UI/PowerGridPanel.cs`,
`src/Main.PowerGrid*`/`Main.ShelterInfrastructure.cs`, `ShelterScheduleSystem.cs`,
`SumpFloodingSystem.cs`, briefing (17A), data catalog for priorities.

### Substeps

1. **Model the 5 documented tiers explicitly** in data (life support → pumping → medical →
   production → comfort). The vocabulary already exists in `AGENTS.md`; give it ids.
2. **Make shedding a player action**: per-room enable/disable plus a "shed to tier N" command, each
   consuming a duty action so it isn't free, and each recorded.
3. **Automate only as a fallback**: on true deficit the grid sheds by authored priority — and the
   briefing says *"the grid shed the workshop; you didn't"* (17A event), so automatic action stays
   attributable.
4. **Sump pumping has the sharpest consequence** (`SumpFloodingSystem` already reads
   `IsRoomPowered(node.nodeId)`): no power → water rises → damage, contamination, and possibly an
   air-quality event. Show a rising-water clock in the shelter surface.
5. **Schedule consequence**: `ShelterScheduleSystem` already takes the grid; make unpowered hours
   shift work/lessons/sleep honestly, feeding 24.
6. **Battery and recharge strategy**: capacity, depth-of-discharge wear, and recharge rate under
   available fuel; make "save the battery for tonight" a real call, with the battery's own ageing.
7. **Brownout must be visible in the room, not just the number**: lights dim (existing UI accents),
   `shelter_generator`/`shelter_ventilation` loops already respond to state, and alerts follow the
   17C ducking rules so a brownout doesn't stack four klaxons.
8. **Recovery has cost**: breaker resets after `brownout_overload` incidents (`:191–202`) consume a
   shift + a part, so over-drawing is expensive rather than merely annoying.
9. **Fuel chain legibility**: generator fuel draw competes with vehicles (`ExpeditionHostSession`
   refuel gate), foundry, and heating — show the competing claims on one screen so the player sees
   the whole argument.
10. **Repair and expansion**: generator condition, part failures, and additional capacity via
    construction/knowledge gates (`geothermal_turbine`, `heavy_marine_diesel_gen` routes already
    exist as *consoles* — see Plan 16A: give them authority before giving them buttons).
11. **Tests**: shed → consequence per consumer; automatic shed attribution; battery endurance in a
    long cold storm; recovery cost; save round-trip; snapshot of the panel in each tier.
12. **Balance**: `ashfall-balance-sim` storm-frequency × fuel-supply sweep. A blackout must be a
    bad evening, not a lost campaign, until the player is deep enough to deserve the knife.
13. **Run the checklist.**

**DoD:** the grid screen is a decision screen, and every shed is one the player can explain
afterwards.

---

## Task 23C — Failure cascades: when the machine fails as a whole

**Goal:** compose the single-system failures into situations with names — the things a player
remembers years later — without scripting them.

**Files:** new `Assets/Ashfall.Core/Shelter/CascadeCoordinator.cs`, `PowerGridSystem.cs`,
`SumpFloodingSystem.cs`, `ShelterThermalSystem.cs`, `StartingLevelSystem.cs`,
`FireIncident` owner, `AirlockSecuritySystem.cs`, `DiseaseSystem.cs`,
`GuiltInsomniaSystem.cs`, briefing/events data, `src/Main.ExpandedShelterSystems.cs`.

### Substeps

1. **Define the input facts** each system already emits (brownout, sump level, temperature,
   filtration state, fire, hatch state, outbreak) — the cascade coordinator **reads only** these;
   it never writes into them.
2. **Express cascades as data**: small rule records (`when sustained_brownout && winter &&
   heating_on → thermal_drop; when thermal_drop && sump_rising → pipe_burst; when
   ventilation_down && storm → indoor_dose_spike`) with snake_case ids validated by the integrity
   gate. This keeps authors adding cascades without touching C#.
3. **Guarantee recovery paths**: every cascade rule must declare its own off-ramp in data; a test
   asserts no reachable cascade lacks one. No unwinnable spirals.
4. **Time to act**: every cascade has a minimum warning window (a rising tide visible at least a
   day before it floods), enforced by a data floor and tested.
5. **Human response routes into existing systems**: cold → sleep quality and morale (needs +
   shelter schedule), contaminated water → disease vectors (`DiseaseSystem` already has 4
   transmission vectors), darkness → foundry/maintenance incidents, which `GuiltInsomniaSystem`
   already links to accidents.
6. **Emit one attributable line per cascade stage** into 17A so the story of the bad week reads
   end to end: *"power shed from pumping → sump rose → cold stored → two fell ill."*
7. **Surface risk before the fact**: a compact "strain" readout on the shelter surface listing
   active conditions and their next likely consequence — using the same Core functions the
   simulation uses, never a second forecast.
8. **Respect performance**: the coordinator is a daily evaluation over existing state, not a
   per-frame or per-hour scan. Confirm the day-advance cost stays inside the budget tracked in
   `artifacts/runtime-scale-results.json` (baseline `day_advance_30d` median ≈ 0.61 s).
9. **Determinism**: pure rule evaluation over a fixed iteration order (ordinal, documented) plus
   `ISeededRng` for any roll.
10. **Tests**: one test per cascade (trigger, effect, off-ramp, warning window), plus a scripted
    "worst winter" integration run asserting the player is never left with zero legal actions.
11. **QA**: manual `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` pass and an
    `ashfall-expansion-qa-playthrough`-style automated check that each cascade is reachable and
    recoverable in a seeded run.
12. **Docs**: `docs/systems/POWER_CASCADES.md` with the rule catalogue and the ownership table from
    23A step 1.
13. **Run the checklist** + `bash scripts/ci/verify-fast.sh`.

**DoD:** a storm, a brownout, and a bad shift combine into one nameable event the player can trace
backwards — and always has a way out.

---

## Cross-Task Dependencies

```
20B (indoor atmosphere model) ◄──── 23A step 7 (air needs watts)
22B (cellar/refrigeration)    ◄──── 23A step 8 (cold storage is a load)
22C (pharma storage)          ◄──── 23A step 9
24  (people: light/heat/sleep)◄──── 23A step 10, 23B step 5
                                   │
23A (one authority) ──► 23B (player sheds) ──► 23C (cascades)
```

**Execution order:** 23A → 23B → 23C, and **23A after 22B** (so refrigeration exists to be
powered) and **around 20B** (one atmosphere model, agreed once).

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --audio-selftest                 # generator/vent loops track state
7. bash scripts/ci/triad-drift-gate.sh
8. ashfall-balance-sim (winter × storm × fuel sweep)
9. perf: day-advance cost within artifacts/runtime-scale-results.json budget
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 23A | 6–9 | 2 | 1 | 0 | 12–16 | Medium–High | MEDIUM (equivalence tests contain it) |
| 23B | 1 | 2 | 1 | 1 | 9–12 | Medium | MEDIUM (balance-wide) |
| 23C | 1 new | 1 | 1 new | 1 | 10–14 | High (composition) | MEDIUM (off-ramp test is the guard) |

**Guardrails:** no new power system, no new tier vocabulary, no scripted disaster scenarios, and no
consumer that reads `PowerGridSystem` directly instead of implementing `IPowerConsumer`. Where a
system genuinely does not need electricity, record it as `DECORATIVE` and leave it alone — a fake
dependency is the same bug as a fake console.
