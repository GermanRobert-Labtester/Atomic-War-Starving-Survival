# ASHFALL — Expansion Wave 10 Index (Expansions 57–61)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-24
**Purpose:** Index and evidence summary for the five Wave 10 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: the small works

Waves 1–9 built a shelter that survives, works, reaches, keeps, remembers, and
keeps house. Wave 10 is about the patient hand trades that convert raw
material into the small things a shelter cannot buy: **time, wood, bone,
light, and salt.** Each plan takes a live or half-wired seam — a clock
contract that nobody maintains, a crafting layer that consumes wood nobody
converts, two bone items with no chain behind them, a live apiary whose wax
goes nowhere, and a 454-line salt mine with no data file — and gives it the
trade around the simulation.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 57 · The Hour | `ISimClock`/`IWallClock`, `DayRecord`, `ShelterScheduleSystem` + host (`SetCurfew`, `AssignBed`, `TickHour`), `EquipmentConditionSystem`, workshop | 4 authored horology logs (5.8 KB, 5.9 KB, 5.0 KB, 5.2 KB); `TimekeepingHorologyCatalog` orphaned; **0 timepiece items** | clocks, bells, water clocks, drift, standard hour, chronometers, records |
| 58 · The Joinery | `CraftingSystem`, `ShelterWorkshopSystem`, `SubterraneanSystem`, `ShelterDecorSystem`, `EquipmentConditionSystem` | 4 authored carpentry logs (5.2 KB, 5.8 KB, 5.3 KB, 6.0 KB); `TimberCarpentryCatalog` orphaned; **0 timber items**; crafting catalogs list wooden tools with no source trade | timber species, seasoning, sawpit, joints, shoring, dry rot, creosote, plans |
| 59 · The Bone Shop | `CraftingSystem`, `ShelterWorkshopSystem`, `ShelterDecorSystem`; sources from 32/15 | 4 authored bonework logs (3.7 KB, 3.6 KB, 3.3 KB, 3.2 KB); `BoneHornCarvingCatalog` orphaned (scanner/journal refs only); **2 bone items with no chain** | bone/horn/antler grades, degreasing, blanks, point angles, glue, small goods, mounts |
| 60 · The Wick | `ShelterFireHazardSystem`, live `ApicultureSystem` (`item_beeswax_block`), chemistry boundary 39, quiet owner 41, grid 21 | 5 authored chandlery logs (3.5 KB, 3.8 KB, 3.2 KB, 3.1 KB, 5.0 KB); `CandleMakingWaxCatalog` orphaned; **0 candle/wick/lamp items**; 39's table already routes wax to candles | rendering, clarity, wicks, dips, burn tests, lamps, lanterns, light plan, ration |
| 61 · The Salt Pan | `SaltMineExtractionSystem` (454 lines, veins/workers/drill/pump/treaty deliveries), `SilentFoundryHostSession` (`RegisterVein` `:582`), `BrineExtractionPanel`, `ExpansionHubSave.saltMine` v6 | **No salt catalog file at all**; veins registered from code; 1 gallery log (5.0 KB) unread; 5 salt items already in `items.json` | data veins, pans, boiling, grades, issues, deliveries, road salt, gallery records |

The strongest authority discipline in the wave: every plan extends a live owner
rather than replacing it, and every plan carries a hard boundary table. The
Hour reads the tick contract and never modifies it; the Joinery buys standing
timber and never owns forests; the Bone Shop works animal material only and
never human remains; the Wick registers every flame with the fire owner and
obeys the quiet owners; the Salt Pan adds no price to a live salt system.

---

## The five plans

1. `expansion_57_the_hour_plan.md` — ~70,000 chars.
   Clock registry, drift book, clock bench, bells, clepsydras, hourglasses,
   noon mark, chronometer issue, and the standard hour offered to outposts.
2. `expansion_58_the_joinery_plan.md` — ~70,000 chars.
   Timber species and grades, sawpit, seasoning stacks, joints and pegs,
   shoring sets and audits, dry rot, creosote retort, structural repairs, and
   the drawn set of plans.
3. `expansion_59_the_bone_shop_plan.md` — ~70,000 chars.
   Sourcing slips, the yard, degreasing vats, blanks, point angles, needles,
   awls, buttons, combs, glue, mounts, and an absolute animal-only rule.
4. `expansion_60_the_wick_plan.md` — ~70,000 chars.
   Tallow and beeswax lots, rendering, clarity grades, braided wicks, blended
   candles, burn tests, oil lamps, horn lanterns, the light plan, rations, and
   the snuffer.
5. `expansion_61_the_salt_pan_plan.md` — ~70,000 chars.
   Data veins and gallery names, brine wells and tests, evaporation pans,
   boiling house costs, five grades, issue lists, reserve floors, treaty
   deliveries, road salt, and the inscription ledger.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second schedule, crafting, masonry, forest,
  hunting, leather, medical, chemistry, fire, power, sleep, foundry, economy,
  water, or record system is introduced.
- **Deterministic.** Live seeded paths only: the tick, the mine's `TickDaily`,
  the hunt and herd yields, the live weather. Paired replay hashes must match.
- **Persistence.** New state is additive inside existing owners: `crafting`
  (58, 59, 60), `shelter_schedule` (57), `shelter_fire` registrations (60),
  and `ExpansionHubSave.saltMine` (61). Legacy saves load neutral; the Triad
  drift gate must pass.
- **Tone.** Restrained, human, fictional. No real sites, brands, trades, or
  persons copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 10

| Plan | Hard contract |
|---|---|
| 57 Hour | The tick contract is read-only; no time police, no penalties, no sleep shaming; bells never imitate alarms; the standard hour is offered, never enforced |
| 58 Joinery | No sacred or protected tree felling; no collapse spectacle; rot is treated by moisture and airflow, never a miracle; no poison framing; drawings belong to the shelter, not a guild |
| 59 Bone Shop | **Human remains are never materials** and the validator rejects them; no kill or butchery scenes; sourcing always names a provider and a permission; no ritual or magical bone; no trophy-of-war framing; pre-war work is honored or stored, never sold |
| 60 Wick | No candle currency or wager; the ration is posted before it is enforced and never shames a room; the ward is always first; every flame has a stand and a snuffer; no mystical flame |
| 61 Salt Pan | No prices or market mechanics; deliveries are made or missed with reasons and never fudged; no mineshaft spectacle; no treasure framing in galleries; the ward owns all medicine; road salt is a supply, not a traffic system |

---

## Cross-wave hooks (summary)

- **Within Wave 10:** clock cases (57→58), horn panes and wicks (59↔60), bell
  frames (57←58), candle marks for powerless hours (60→57), glue (59→58).
- **With earlier waves:** live apiculture wax (60), the grid's failures (57,
  60, 61), the fire owner's rules (58, 60, 61), the quiet owners (57, 60), the
  mine shoring (58→18), the ward's instruments (59→38), the thread trade's
  needles (59→27), the herd and hunt (59, 60, 61←15, 32), the foundry accords
  (61→10), and the vault's records (61→50).

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 10 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible.

**Wiring notes to carry into any promotion package:**

- **The Salt Pan (61) — Phase 1.** This is the only Wave 10 plan that touches
  an already-live host path. Vein registration moves from code
  (`SilentFoundryHostSession.RegisterVein`, `:582`) to authored
  `salt_veins.json`, while every legacy code-registered vein stays loadable:
  authored veins load first, legacy veins load second, dedupe by `vein_id`
  with authored data winning, and a save with no salt catalog bound keeps the
  exact legacy behavior. A focused test must prove both registration paths and
  that paired replay hashes match. The package must claim
  `SilentFoundryHostSession` and `ExpansionHubSave.saltMine` explicitly, since
  both are shared seams.
- **The Bone Shop (59) — Phase 1.** The animal-only rule must be enforced by
  the integrity gate, not only by prose. The validators for
  `bone_materials.json`, `bone_sources.json`, `glue_batches.json`, and
  `mounts.json` reject any source that resolves to human remains and fail
  closed on ambiguity; `RemainsRuleSystem` stops the batch rather than
  queueing it. A deliberately invalid fixture must fail the gate, so the rule
  cannot be edited out of the data.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 280,000–290,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. All
  five plans recommend additive sub-objects.
- Whether any Wave 10 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending panel lifecycle, data integrity,
  and content utilization verbs, with a domain verb only if none exists.
- Priority order. Recommended: **59 (The Bone Shop) first** because two bone
  items are already live with no chain and the ward's suture supply is the
  smallest high-value gap; **60 (The Wick) second** because the apiary already
  produces wax and the fire owner already lists candles as ignition risk;
  **61 (The Salt Pan) third** because the live system exists and only the data
  layer is missing; **57 (The Hour) fourth** because the tick contract makes it
  technically clean but its need is invisible until a schedule fails; **58 (The
  Joinery) fifth** because it is the largest content volume and depends least
  on the others.

---

## Evidence anchors (file references used across the plans)

- **The Hour:** `Assets/Ashfall.Core/Clock/ISimClock.cs`,
  `Assets/Ashfall.Core/IWallClock.cs`, `Assets/Ashfall.Core/Campaign/DayRecord.cs`,
  `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs`,
  `src/Host/ShelterScheduleHostSession.cs` (`SetCurfew`,
  `SetEmergencyOverride`, `AssignBed`, `LoadCatalog`, `TickDay`, `TickHour`,
  `Save`), `Assets/Ashfall.Core/EquipmentConditionSystem.cs`,
  `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
  `Assets/Ashfall.Core/Narrative/TimekeepingHorologyCatalog.cs` (orphaned;
  `DeadbeatEscapementWearEntry`, `InvarPendulumThermalEntry`,
  `MainspringFatigueRuptureEntry`, `ClepsydraWaterClockEntry`), narrative logs
  `deadbeat_escapement_wear_logs.json` (5,832 B),
  `invar_pendulum_thermal_expansion.json` (5,909 B),
  `mainspring_fatigue_rupture_audits.json` (5,046 B),
  `water_clock_orifice_silt_records.json` (5,226 B).
- **The Joinery:** `Assets/Ashfall.Core/Narrative/TimberCarpentryCatalog.cs`
  (orphaned; `MortiseTenonFailureEntry`, `TimberCreosoteTreatmentEntry`,
  `TimberDryRotFruitingEntry`, `SquareSetShoringEntry`),
  `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`,
  `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
  `Assets/Ashfall.Core/SubterraneanSystem.cs`,
  `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`, narrative logs
  `mortise_tenon_failure_reports.json` (5,246 B),
  `timber_creosote_treatment_logs.json` (5,781 B),
  `timber_dry_rot_fruiting_records.json` (5,294 B),
  `square_set_shoring_audits.json` (5,950 B); crafting catalogs list
  Wheelbarrow, Screen, Pottery wheel, and Mold as wooden tools with no source
  trade.
- **The Bone Shop:** `Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs`
  (orphaned from runtime; referenced only by `ContentUtilizationScanner`,
  `ContentUtilizationRuntimeCollector`, `src/Journal/JournalCatalogData.cs`;
  `BoneDegreasingPrepLog`, `AntlerHornSawingRecord`, `ScrapingPolishingReport`,
  `NeedleAwlHookAssay`), `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`,
  `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
  `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`, narrative logs
  `antler_horn_sawing_records.json` (3,650 B),
  `bone_degreasing_prep_logs.json` (3,623 B),
  `scraping_polishing_reports.json` (3,290 B),
  `needle_awl_hook_assays.json` (3,228 B); items `item_surgical_bone_chisel`
  and `item_decor_trophy_deer_antlers` with no production chain.
- **The Wick:** `Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs`
  (orphaned; `TallowRenderingVatLog`, `BeeswaxClarificationRecord`,
  `WickBraidingPrimingReport`, `CandleDipMouldAssay`),
  `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`,
  `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` (live; wax),
  `item_beeswax_block` in `items.json`, narrative logs
  `tallow_rendering_vat_logs.json` (3,516 B),
  `beeswax_clarification_records.json` (3,756 B),
  `wick_braiding_priming_reports.json` (3,195 B),
  `candle_dip_mould_assays.json` (3,099 B),
  `beeswax_rendering_dipping_assays.json` (5,011 B); expansion 39's product
  table already routes wax to candles with the note "nights darker".
- **The Salt Pan:** `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs`
  (454 lines; `SaltMineVeinState`, `TreatyDeliveryRecord`, `SaltMineState`;
  `RegisterVein`, `UnlockVein`, `AssignWorkers`, `TickDaily`, `ReplaceDrill`,
  `RepairPump`, `SetPower`, `IsTreatyFulfilled`, `GetDeliveryCount`,
  `CaptureState`, `RestoreState`),
  `src/Foundry/SilentFoundryHostSession.cs` (`RegisterVein` `:582`,
  `OnTreatyDeliveryAccepted`/`OnTreatyDeliveryMissed` `:208–209`),
  `src/UI/BrineExtractionPanel.cs` (vein/workers/drill/pump/contamination/
  storage labels), `Assets/Ashfall.Core/ExpansionHubSave.cs` (`saltMine` v6,
  lines 54–55, 350–433), `Assets/Ashfall.Core/Foundry/BrineWaterSystem.cs`,
  items `item_preservation_salt`, `item_rock_salt_sack`,
  `item_trade_salt_sack`, `item_medical_saline_salt`,
  `item_chem_clarity_salts`, narrative log `salt_mine_inscriptions.json`
  (5,036 B); no salt catalog file exists.
- Data and governance: `items.json`, architecture map and save-store matrix,
  `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`.
- Wave indexes: `docs/expansions/wave1/WAVE1_INDEX.md` through
  `docs/expansions/wave9/WAVE9_INDEX.md`.