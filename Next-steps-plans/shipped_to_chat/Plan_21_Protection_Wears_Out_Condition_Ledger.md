# Plan 21 — Protection Wears Out: One Condition Ledger

> **Wave:** Continuity Wave 2 — *The Bunker Machine*
> **Depends on:** Plan 20A (exposure must be environmental before wear from exposure means
> anything). Pairs with 22 (a worn filter and a eaten pill are the same accounting problem).
>
> **Theme:** AGENTS.md's own domain list advertises "gas mask, hazmat suit (**degrading**)". The
> data authority authors `durability` and `radProtection` for 166 of 212 items, Core implements
> durability-scaled protection, and **nothing in the running game ever decrements it**. Meanwhile a
> *second*, separate condition ledger (`EquipmentConditionSystem`) tracks weapons and vehicles and
> is the one that actually affects combat. Two ledgers, one of which is inert.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| Fact | Evidence |
|---|---|
| Protection is durability-scaled… | `Assets/Ashfall.Core/Inventory/Inventory.cs:35–37` — `EffectiveProtection() => max(0, RadProtection) * DurabilityFraction()` |
| …and degradation is implemented | `Inventory.cs:40–44` — `Degrade(gameHours) { CurrentDurability = max(0, CurrentDurability - DegradeRate * gameHours) }` |
| **The bridge hardcodes the rate to zero** | `Inventory.cs:938–954 FillWornGear` builds every `WornGear` with `DegradeRate = 0f` (line 951), `MaxDurability = equipped.Item.durability`, `CurrentDurability = equipped.CurrentDurability` |
| **…and degrades a throwaway copy** | `src/Host/SurvivorsHostSession.cs:248` calls `CollectWornGear()` → a brand-new `List<WornGear>` per tick; `RadiationSystem.cs:189 DegradeWornGear(worn!, gameHours)` mutates those copies and `RadiationSystem` holds no reference back to the inventory |
| Net effect | gear protection is **constant and permanent**: a gas mask scavenged on day 1 protects identically on day 300, in a hot sector, during a black-rain storm |
| The real durability field exists and persists | `EquippedItem.CurrentDurability` written on equip (`Inventory.cs:860`), saved/restored (`:814`, `:832`), read for condition display (`:924`) — it just never decreases |
| Durability is authored in the authority | `items.json`: 166/212 definitions carry `durability` and/or `radProtection` (`gas_mask`: "cuts airborne contamination by thirty percent…"; `hazmat_suit`: "stops eighty percent… every percent shows") |
| A **second, working** condition ledger exists | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` — own state, own save section, `OnConditionChanged`, its own internal repair at `:142` (`condition + 20f`) |
| …and it drives combat | `Assets/Ashfall.Core/Combat/WeaponEquipmentBridge.cs:34,51` picks the best-condition weapon and maps `condition/100 → 0..1` performance; vehicle wear is a third path (`ExpeditionVehicleSystem.cs:130,163–170` — breakdown under 20) |
| **No sync between them** | `grep "EquipmentCondition" Assets/Ashfall.Core/Inventory/ src/Host/SurvivorsHostSession.cs` → **0 hits**; pipes are a fourth condition counter (`ShelterThermalSystem.cs:281,359–360`) |
| Repair exists but is disconnected from crafting | crafting recognises medical items by hardcoded id list (`CraftingSystem.cs:336–337`) and Plan #101 gave weapons wear→repair; protective-gear repair has no data-driven recipe path |
| Consequence for the player | the game's central scarcity loop — *"your mask is failing, do you spend the filter, the patch kit, or the trip home?"* — does not exist |

---

## Task 21A — Make degradation real: rate from data, mutation on the authority

**Goal:** protective gear measurably wears during exposure, and the wear is the same number the
UI, the save, and the repair system all read.

**Files:** `Assets/Ashfall.Core/Inventory/Inventory.cs:938–954`,
`Assets/Ashfall.Core/Radiation/RadiationSystem.cs:175–200,325–331`,
`src/Host/SurvivorsHostSession.cs:235–265`, `items.json` (+ `ItemCatalogLoader`),
`Ashfall.Core.Tests/InventoryGearBridgeTests.cs`.

### Substeps

1. **Failing test first**: register a survivor with a fresh `gas_mask`, run N hours of hot-zone
   exposure, assert `EquippedItem.CurrentDurability < initial`. It must fail today.
2. **Choose the mutation target**: degradation must be applied to `EquippedItem`, not to a copy.
   Two viable shapes — (a) give `RadiationSystem` an injectable
   `Action<SurvivorRadState, float hours>` *degradation sink* the host implements against the
   inventory, or (b) have the host re-read `EffectiveProtection()` from the inventory after each
   tick and apply wear itself. Pick (a): it mirrors the existing `_applyNeed` / `_onExposed`
   callback style already in the `RadiationSystem` constructor (`:104–107`) — no new concept.
3. **Stop hardcoding the rate**: replace `DegradeRate = 0f` at `Inventory.cs:951` with a value
   derived from data — per-family base rate × zone contamination × weather modifier (reuse 20C's
   `WeatherEffects` table). Filter *canisters* and *suit shell* must be different families with
   different rates.
4. **Author the rates** in the authority: add `degradePerHour` (or reuse `durability` semantics
   with a documented lifespan in hours) to the protective-gear items in `items.json`; snake_case,
   `schema_version` bumped per the data rules, ids validated by `CatalogIntegrityValidator`.
5. **Define the write-back contract** in one sentence in the code comment above `FillWornGear`:
   the buffer is a *read projection*; mutation happens only through the sink. Prevent the next
   contributor from "fixing" this by mutating the copy again.
6. **Zero-protection behaviour**: at `CurrentDurability == 0`, protection must be exactly 0
   (`EffectiveProtection()` already guarantees it) **and** the player must be told — emit a
   `gear_failed` day event (17A vocabulary) and fire the existing hazard/alert cue family.
7. **Make it visible where it is decided**: expedition prep and the inventory/inventory_detail
   panels show remaining protection as a fraction with an honest "hours left at current exposure"
   estimate, computed by the same Core function the sim uses — never a second arithmetic path.
8. **Determinism**: wear is a deterministic function of hours × rates; any stochastic component
   uses `ISeededRng` via `CampaignStreamIds`, never `System.Random`.
9. **Save contract**: `EquippedItem.CurrentDurability` is already in the inventory save
   (`:814/:832`) — prove it round-trips with a mutated value, and add a checksum-sensitive test so
   the field can't silently drop.
10. **Performance note**: `CollectWornGear` currently allocates a new list per survivor per tick;
    reuse the buffer (`Inventory` already offers the `List<WornGear>` out-parameter shape) to keep
    the day-advance budget from 24C/3rd-party perf work honest.
11. **Tests**: rate table, zone/weather scaling, zero→no protection, save round-trip,
    paired-seed replay, one integration test that a mask fails mid-expedition and dose rises as a
    result.
12. **Run the five-step verification checklist.**

**DoD:** gear is a consumable. The player can watch it die and must act before it does.

---

## Task 21B — One condition ledger: unify gear, weapons, vehicles, and infrastructure wear

**Goal:** stop having four independent wear counters for one fiction ("things get used up"). A
single condition authority, with per-family behaviour, removes an entire class of drift.

**Files:** `Assets/Ashfall.Core/EquipmentConditionSystem.cs`,
`Assets/Ashfall.Core/Inventory/Inventory.cs`, `Assets/Ashfall.Core/Combat/WeaponEquipmentBridge.cs`,
`Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`, `Assets/Ashfall.Core/ShelterThermalSystem.cs`,
`src/Main.Expeditions.cs:113–123`, `src/Main.ShelterBatch3.cs:130`,
`docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`, `SaveSectionRegistry`.

### Substeps

1. **Tabulate the four existing wear systems** — inventory durability, `EquipmentConditionSystem`,
   `ExpeditionVehicleSystem`, `ShelterThermalSystem` pipes: fields, save locations, wear triggers,
   repair paths, and who reads the number. Publish the table before choosing a design.
2. **Decide the boundary explicitly and record it**: personal/weapon/vehicle condition =
   *instance condition on the item*; shelter infrastructure (pipes, filters as installed plant) =
   *structural condition on the shelter*. Two ledgers with **documented, disjoint** ownership beats
   one ledger with ambiguous ownership. Do not merge pipes into inventory.
3. **Make `EquippedItem.CurrentDurability` and `EquipmentConditionSystem` refer to the same
   instance** — one owner of the value, one save. The cleanest shape: `EquipmentConditionSystem`
   keys by item instance id and becomes the write authority; `Inventory` reads through it (or vice
   versa) rather than storing a parallel float.
4. **Migrate the read side first** (safe order): point `WeaponEquipmentBridge` at the unified value
   and prove combat wear→repair (#101's loop) is byte-identical before removing the duplicate
   field.
5. **Migrate the write side**: exposure wear (21A), crafting use, and combat wear all call one
   `ApplyWear(instanceId, amount, cause)` with a `cause` enum so "why did this break" is answerable
   and reportable.
6. **Save migration**: item-instance condition rides the existing inventory/combat sections; add a
   migration path for saves carrying the old duplicated field (V1→V2 discipline, never break a
   pinned checksum casually — extend `SaveWireContract` assertions deliberately).
7. **Reconcile with H2** in `AGENTS.md`: `WornGear` still exists in two namespaces with a sanctioned
   `Radiation.WornGear.FromInventory` bridge. If this task's unification removes the need for the
   bridge, delete the bridge and update the known-issues list — do not leave a "sanctioned" shim for
   a type that no longer has two homes.
8. **Repair loop, data-driven**: protective-gear repair recipes authored in `recipes.json`
   (patch kits, filter canister replacement, seal re-gluing) with consumption through the single
   `TryConsumeBill` path already used by expeditions (`Main.Expeditions.cs:141`) and deep coast
   (`DeepCoastHostSession.cs:337–344`) — replace the hardcoded medical-id list in
   `CraftingSystem.cs:336–337` with an item tag/type check (and see 22's tag work).
9. **Repair must be a trade-off, not a button**: repair consumes scarce materials and takes a duty
   shift (24's roster consumes the labour), so patching a suit competes with building a filter.
10. **Breakage consequences per family**: weapon → jam/reliability (existing), hazmat/gas mask →
    protection 0 (21A), vehicle → breakdown (existing), plus a new *irreversible* end state for
    gear past a wear threshold so "repair" and "replace" remain distinct decisions.
11. **Tests per family**: wear cause attribution, repair cost, save round-trip across the migration,
    determinism, and one long-campaign soak test that gear lifespans deplete at authored rates over
    200 in-game days.
12. **Update docs**: `docs/systems/` gets a `CONDITION_LEDGER_OWNERSHIP.md`; the save-store matrix
    regenerates via `scripts/ci/generate-save-store-matrix.sh --check`.
13. **Run the checklist** + triad drift gate.

**DoD:** exactly one number answers "how much life does this thing have left", for every family
that wears out.

---

## Task 21C — Gear decisions reach the places decisions are made

**Goal:** condition must be an input to the dispatch, trade, and expedition screens — otherwise the
player is only told about wear after it kills someone.

**Files:** `src/UI/ExpeditionPanel.cs`, `src/UI/ExpeditionRadarPanel.cs`,
`src/UI/InventoryPanel.cs` / `InventoryDetailPanel.cs`, `src/Host/HoldfastTerminalPanel.cs`,
`ExpeditionSystem.Estimate`, `TradeScreenSeam` / caravan trade UI, `MapPanel`.

### Substeps

1. **Read `ExpeditionSystem.Estimate`** (pure, already mirrors tick math for the UI per #101) and
   extend its signature to take the party's effective protection, rather than recomputing anything
   panel-side.
2. **Dispatch screen shows party protection per candidate** — best, median, and "N members have no
   working mask" — and a projected dose for the trip, not a generic risk word.
3. **Refuse-with-warning, never block silently**: dispatch into a sector whose requirement exceeds
   the party's protection must warn and require confirmation, and the confirmation text must name
   the missing protection (existing `ui_invalid_action` / `ui_warning` cues — Wave 1 17C).
4. **Recommendation, not automation**: offer "who to send / what to patch" as a read-only hint
   derived from the same `Estimate` call, so the player can disagree. No auto-assignment (that
   would delete the decision).
5. **Trade screen parity**: a trader with spare canisters must be able to sell them, and the price
   must reflect the same scarcity signal the player feels (22's consumption authority makes filters
   genuinely spent).
6. **Geiger/dosimeter legibility**: calibration state changes the *reliability* of the reading the
   player trusts (`geiger_calibration` and `triangulation` routes exist; `Device.Calibration` is
   already saved at `Inventory.cs:978`-region) — a badly calibrated meter misreports zone rates
   from 20A, turning gear decisions into gambling. Keep the mechanic honest: the meter lies, the
   dose does not.
7. **Failure pre-announcement**: if a trip's projected exposure implies gear failure mid-route, say
   so before departure and let the player shorten the route (route editing already exists via
   map/waystation surfaces).
8. **Post-mortem honesty**: after an expedition, the memorial/journal entry states whether gear
   failure contributed — reuse `SurvivorFateSystem`/`MemorialSystem` text slots and the
   `ashfall-write` tone rules (cold, restrained, no lecturing).
9. **Accessibility**: condition must be readable without colour-only cues (percent + word, per
   `ashfall-ui-access`).
10. **Snapshots** for the changed dispatch/inventory surfaces per
    `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`; regenerate approvals explicitly, never silently.
11. **Tests**: one UI-logic test per screen asserting it reads the unified value (not a re-derived
    one), plus a headless journey test that dispatch → wear → warning → repair → re-dispatch works
    end to end.
12. **Run the checklist.**

**DoD:** gear condition is an input to at least three decisions and one of them can save your
life.

---

## Cross-Task Dependencies

```
20A (environmental dose) ──► 21A (wear from dose) ──► 21B (one ledger) ──► 21C (decisions read it)
                                  │                        │
   22 (consumables: filters, patch kits, meds) ◄──────────┘  shared bills/recipes
   24 (labour: repair takes a shift)          ◄─────────────┘
```

**Execution order:** 20A → 21A → 21B → 21C. Do not start 21B before 21A lands: unifying a ledger
whose main field never changes is refactoring a no-op.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --survivors-selftest             # gear probes (H2 path)
7. ashfall-equipment-balance (protective-gear lifespan report)
8. bash scripts/ci/triad-drift-gate.sh
9. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 21A | 2 | 1 | 1 | 0 | 8–11 | Medium | MEDIUM (dose curves shift → rebalance 20) |
| 21B | 4–5 | 2 | 1 (recipes) | 0 | 12–16 | **High** (save migration) | **HIGH — save compatibility** |
| 21C | 1 (`Estimate`) | 1 | 0 | 5 | 6–9 | Medium | LOW |

**Guardrails:** no new condition system, no new repair station, no new items beyond authored
consumables already in `items.json`, no auto-assignment. The item descriptions already promise
degradation ("every percent shows") — this plan makes the prose true.
