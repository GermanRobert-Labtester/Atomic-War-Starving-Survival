# PLAN 147 — BASELINE (Task A recon, 2026-09-06)

## Mission recap

Turn the 20-entry `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json`
corpus into a safe, player-facing black-market/hidden-goods content layer without
letting its declarative `mechanics` object become a second implementation of
morale, trade, water, radio, dependency, door, agriculture or health systems.

## 1. What existed before this plan

| Artifact | State before Plan 147 |
|---|---|
| `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json` | 20 authored entries, `schema_version: 1`, no runtime consumer |
| `Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs` | Typed loader + tier/category/tag queries; `ContrabandMechanics` binds only 14 fields |
| `Ashfall.Core.Tests/BunkerContrabandCatalogTests.cs` | 3 tests: count=20, field sanity, query functions. Proves shape, **not** runtime authority |
| Host (`src/`) | **Zero** references — catalog never loaded, no panel, no save section, no producer |
| Content-utilization | Catalog is loaded only by its own test class; no `GAMEPLAY_CONSUMED` runtime proof |

### The latent debt, precisely quantified

The authored corpus carries **45 distinct mechanics keys** across the 20 entries.
The typed `ContrabandMechanics` class binds **14**. The other **31** keys
(`faction_influence_tempest`, `diesel_theft_liters_per_use`,
`rad_detection_precision_multiplier`, `ration_chit_forgery_success_rate`,
`genetic_integrity_score`, …) are silently dropped by `System.Text.Json`
deserialization — the exact "JSON says it works" hazard the plan names. Even the
14 bound fields had **no executing consumer anywhere**: no code reads
`ContrabandMechanics` outside the loader and its shape tests.

Per-entry mechanics payloads: see `CONTRABAND_ENTRY_MATRIX.md`.
Field-by-field ownership: see `CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md`.

## 2. Existing systems surveyed as candidate owners

| Domain | Live authority found | Contract relevant to contraband |
|---|---|---|
| Inventory | `Ashfall.Core.Inventory.Inventory` + `ItemCatalogLoader` (items.json + 10 secondary files) | Canonical ids (`sugar`, `item_playing_cards`, `item_seed_wheat`, `spirits`, …) with `moraleEffect`, `tradeValue`, weight/stack; atomic `InventoryBill` transactions |
| Morale | Canonical item `moraleEffect` applied by the host item-use pipeline (`Needs.Modify(s, NeedKind.Morale, …)` in `InventoryHostSession`) | Morale may only move on a consumption/use event through a canonical item — never on catalog load/view |
| Economy/barter | `ShelterBarterSystem` (Plan 54), `MarketSystem`, `TradeScreenScenarios` (Plan 61 family), `HoldfastTradeSession` | Item-for-item barter with base values, bp multipliers, counterfeit-risk rolls; **no scrip currency runtime** |
| Chemical dependency | `ChemicalDependencySystem` (`Medical/`), catalog `chemical_dependency_items.json` (`morphine`, `opium`, `alcohol`, …) | `OnSubstanceConsumed(survivorId, itemId, kind)`; probability contract owned by the system, not by item data |
| Agriculture | `GreenhouseExpansionCatalog.CropCatalog` (13 crops; `item_seed_wheat` → `crop_wheat`) | Seed → yield route already canonical; no yield multiplier field exists or is needed |
| Water | `clean_water` item + `WaterTreatmentSystem`/`BrineWaterSystem` | Litres are inventory units of `clean_water`, not a parallel pool |
| Doors/security | `AirlockSecuritySystem` | Blast-door override has no success-rate extension point |
| Radio | Radio systems (`src/Host` radio sessions) | No radio-range-boost mechanic exists |
| Locations | `loc_`-prefixed world locations; bunker interior rooms (`room_*` in `ShelterThermalSystem`) | `hidden_stash_location` strings are neither `loc_` ids nor room ids — prose-ish stash niches |
| Justice/tribunal | `JusticeSystem` (Plans 190–193), `StandingRecord` | No suspicion meter exists |

## 3. Baseline decisions taken in this session

1. **Schema v1 retained; code-side allowlist chosen** (Plan §8). The 45 authored
   mechanics keys are frozen in `ContrabandCatalogValidator.KnownMechanicsKeys`;
   any NEW key is a validation error until a disposition is recorded in the
   authority matrix. No `effect_refs` rewrite — the corpus is data-stable.
2. **New Core runtime**: `ContrabandStashSystem`
   (`Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs`) — deterministic,
   once-only stash discovery granting **canonical** inventory items through the
   atomic `InventoryBill` pipeline. No mechanics field is executed by it.
3. **New Core validator**: `ContrabandCatalogValidator`
   (`Assets/Ashfall.Core/Narrative/ContrabandCatalogValidator.cs`) — raw-JSON
   validation closing the silent-drop hole (Plan Task A.15).
4. **Vertical slices (Plan §13)**: three activations
   (`ContrabandStashSystem.DefaultActivations`), one per tier:
   - tier 1 `contraband_card_deck_pinned_kings` → `item_playing_cards`
   - tier 2 `contraband_unrationed_sugar_brick` → `sugar` ×8 (800 g brick = 8 × 100 g packets)
   - tier 3 `contraband_century_seed_grain_vial` → `item_seed_wheat`
5. **Tests**: `Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs` (28 tests).

## 4. Explicitly out of scope for this session (documented, not silently skipped)

- Host/Godot wiring: no `Setup/Save/Tick` triad, no panel, no save-store
  section registration yet. The Core system is host-ready but unwired
  (`CONTRABAND_SAVE_COMPATIBILITY.md` §3).
- Barter-route acquisition (caravan stock listings), barter-side price
  semantics and arbitrage closure beyond the audit in
  `CONTRABAND_TRADE_AND_ARBITRAGE_AUDIT.md`.
- Remaining 17 records: deferred pending per-record owner review
  (`CONTRABAND_ITEM_IDENTITY_MATRIX.md`).
- Chemical-dependency mapping for the morphine entry (deferred; requires the
  canonical `morphine` item, which does not exist in items.json today).
