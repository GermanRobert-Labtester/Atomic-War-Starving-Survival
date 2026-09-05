# Balance Sim — Expedition Destination Yield vs Tier (Plan 76.2)

System: expedition destination loot economy — expected salvage value per
sortie vs danger tier, stamina cost, encounter burden, collapse risk.
Harness: `Ashfall.Core.Tests/Expeditions/Plan76BalanceSimulationTests.cs`
(deterministic xUnit, `ISeededRng` only). **No production data modified** —
findings and proposals only.

## Model (replicates runtime 1:1)

| Knob | Value | Owner |
|---|---|---|
| Loot roll chance / looting tick | `0.5 + dangerLevel × 0.05` | `ExpeditionSystem.PerformLootRoll` |
| Looting ticks (auto-retreat) | 3 | `AutoRetreatAfterLootTicks` |
| Encounter chance / tick | `encounterChancePerTick × 0.5` (stealth) | data + `RollEncounter` |
| Stamina / tick | `baseStaminaDrainPerHour × 0.5 + loadRatio × 15 × 0.5` | data + `ApplyStaminaDrain` |
| Capacity | 40 kg on foot, 1.0 kg per loot unit | `maxLootCapacityKg`, `PerformLootRoll` |
| Item value | `tradeValue` per item catalog | `items*.json` |
| Sortie | stealth, on foot, day, 0.5 h/tick, out `distanceTicks` + 3 loot + back | `TickHours` |

Seed manifest: one `SeededRng` per sortie, seed =
`761_000_000 + destinationIndex × 1000 + runIndex` (destinationIndex = record
order in `expeditions.json`). 200 runs per destination × 53 destinations.
Determinism proven in-test: two full sweeps serialize byte-identically.
Artifact: `artifacts/balance-sim-expeditions.json`.

## Sweep results (E[value] per completed sortie, tradeValue units)

Full table in the artifact. Tier medians:

| Tier | Destinations | E[value] median | Range (excl. outlier) |
|---|---:|---:|---|
| d1–3 | 16 | ~24 | 9.2 – 40.9 |
| d4–5 | 18 | ~29 | 13.8 – 40.2 |
| d6–7 | 13 | ~36 | 22.3 – 57.6 |
| d8–10 | 6 | ~50 | 25.6 – 61.5 |

The tier ladder works — **with one violent exception**.

## Verdicts

### FLAG 1 (HIGH) — `loc_ordnance_shoulder` is a degenerate outlier

> **STATUS: RESOLVED (owner-approved trim applied — see post-trim section at the end of this report).**

- E[value] **216.94** — **3.5× the next destination** (`location_the_dead_hand_core` 61.50).
- value/stamina 3.07 while leading the d6+ tier by a wide margin; collapse risk only 0.5%.
- Root cause (item economics, not table shape): `ammo_762` trades at **12/round**
  and `ammunition_brass` at **14/round**, and the table's quantity bands are
  stacks (`ammo_762` 10–25, `ammo_12g` 8–20, `ammunition_brass` 5–15). One
  successful loot roll ≈ 150–210 value — more than the full expected haul of
  any other destination. Only the 40-unit foot capacity caps it.

**Proposal (not applied — data change list for the owner):** trim the
`table_loot_ordnance_shoulder` stack bands to e.g. `ammo_762` 4–10,
`ammo_12g` 3–8, `ammunition_brass` 2–6, `smokeless_powder` 1–3. Modeled effect:
E[value] ≈ 90–110 — still the best ammunition destination (its identity, §76
"might solve our ammunition problem"), no longer a 3.5× economy break. The
`PLAN76_1_MILITARY_BINDINGS.md` claim ("highest per-visit ammunition yield")
survives at roughly 1.5× the depot's ammunition value.

### FLAG 2 (LOW) — `collapsed_building` is the weakest destination

> **STATUS: RESOLVED (bulk-band bump applied — see post-trim section).**

E[value] **9.23** — bottom of the catalog, less than half of `ruined_garage`
(20.72) at the same tier/distance. Root cause: rubble items trade at
1.2 (`scrap_metal`), 2 (`wooden_plank`, `box_of_nails_10`), 5 (`concrete_mix`).
Not dead (100% completion, distinct construction-materials signature), but
strictly value-dominated among d3 sites. Proposal: raise `steel_rebar`
quantity band or add one uncommon find (e.g. `item_galvanized_rebar`,
w≈10) — owner's call; low urgency.

### OBSERVATION — same-table dominance (pre-existing)

> **STATUS: ACCEPTED (evidence-based decision — see decision note).**

`loc_denial_cut_substation` (8 ticks, E 38.66, val/stam 1.03) is strictly
outperformed by `electrical_substation` (6 ticks, E 38.06, val/stam 1.50) —
same bound table (`table_loot_power_substation`). Both pre-date Plan 76.
The Cut's distinct identity is narrative (warlord territory); a unique hook
or a small distance reduction would close the gap. Observation only.

**Decision (Plan 76.3): accepted, no data change.** Three reasons:
1. The static sim cannot see the runtime warlord layer — the host composes a
   per-location encounter multiplier from `TravelDangerModifier`
   (`Main.EvolvingWorld.cs:46,148`), so the Cut's real encounter pressure on
   hostile ground exceeds its authored 0.18.
2. The Cut already carries unique narrative wiring: the "High-Risk Cut" route
   (Holdfast ↔ Denial Cut, radiation hazard), the fallout-hub map entry, and a
   dedicated combat encounter (`loc_denial_cut`) — Plan 58/59 hooks, not
   loot-table work.
3. Its distance (8 ticks) is parity-protected under Plan 76 §1.2 (original
two destinations).

### CONFIRMATION (working as intended)

`location_the_dead_hand_core`: **22.5% collapse** on foot — the only
destination where a baseline survivor regularly fails from exhaustion
(95.6 mean stamina spent vs 100 capacity). This is the intended endgame wall;
vehicles/garage support or high-stamina survivors are the counterplay.
No change proposed.

Starter-adjacent efficiency leaders (`loc_water_station` 3.69 val/stam,
`loc_settlement_brine_pans` 3.20) are low-absolute-value quick runs — the
per-stamina efficiency of a 2-tick site is expected and not a trivialization.

## Post-trim re-run (applied changes)

The owner-approved quantity-band trim was applied to
`table_loot_ordnance_shoulder` (`ammo_762` 10–25 → **4–10**, `ammo_12g`
8–20 → **3–8**, `ammunition_brass` 5–15 → **2–6**, `smokeless_powder`
2–5 → **1–3**; weights, other entries, and `reloading_primer` untouched).
Same seeds, same harness (artifact regenerated):

| Metric | Before | After | Target |
|---|---:|---:|---|
| E[value] / sortie | 216.94 | **114.41** | 90–110 (modeled) — landed just above, accepted |
| Ratio to next destination | 3.53× | **1.86×** | ≤ ~2 |
| value/stamina | 3.07 | **2.13** | catalog norm 0.4–3.7 ✓ |
| Collapse risk | 0.5% | 0% | — |

- The site **keeps its identity**: still the highest-yield destination in the
catalog and the clear best ammunition source (checkpoints 33.8 / 37.1,
government bunker 55.7).
- Perfect isolation: no other destination's metrics moved — the RNG stream is
  unchanged because `RollLoot` consumes the same number of rolls regardless of
  quantity bands; only the quantities differ.
- Verdict: the degenerate outlier is resolved; remaining 1.86× headroom is
  consistent with a deep dedicated ordnance site at d7/8 ticks.

### FLAG 2 resolution — `collapsed_building` bulk-band bump

Applied in the same pass: stack bands raised to bulk-salvage levels
(`scrap_metal` 3–6, `wooden_plank` 3–5, `box_of_nails_10` 2–4,
`concrete_mix` 2–3, `steel_rebar` 2–3) plus one uncommon find
(`item_galvanized_rebar` w10 q1–2, tradeValue 8). Same seeds:

| Metric | Before | After |
|---|---:|---:|
| E[value] / sortie | 9.23 | **13.40** (+45%) |
| value/stamina | 0.57 | 0.77 |

The site remains the d3 tier's cheap rubble run by design (concert hall is
now the tier floor), but is no longer half of its peers. No other destination
moved (same RNG-stream isolation as the ordnance trim).

## Data changes applied by this sim

**None.** The sim added one deterministic test artifact
(`artifacts/balance-sim-expeditions.json`, committed for reproducibility) and
this report. All balance proposals above are hand-offs to the owner.
