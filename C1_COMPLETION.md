# C1 COMPLETION — Economy Core (Plans 14A + 14B + wiring)

> **Status:** COMPLETE and verified — checkpoints C1.1, C1.2, C1.3 (Core scope).
> **Plan:** `docs/plans/C1_planintegration.md` (see its CHECKPOINT REPORTS section
> for full evidence). This file is the short handoff summary and the tag target
> for the next agent.

## What is done

- **14A Commodity Embargoes:** `TradeEmbargoSystem` (Core, deterministic, zero
  RNG) + `trade_embargoes.json` (10 rules, all 8 source-required weather cases).
  Caravans block/slow from authoritative weather (`OnCaravanEmbargoed` fires
  once per transition; slowdown = additional travel-day cost). Embargo price
  shocks decay to exactly neutral after the weather clears.
- **14B Regional Price Atlas:** `RegionalPriceAtlas` + `regional_prices.json`
  (18 entries, all 5 canonical regions). Item-level overrides category-level;
  `GetBestRegion` is deterministic with ordinal tie-break.
- **Market composition (one path):** canonical quote order is
  `base × demand → category index → Plan 212 shocks → REGIONAL → EMBARGO →
  existing clamps`. Each factor applies at most once. `MarketState` bumped
  additively v2→v3 (nested embargo decay state; old saves restore neutral).
- **Integrity:** both new JSON files are validated by the permanent
  `--data-integrity-selftest` gate (327 catalogs, 0 errors).

## Verification (all green)

| Gate | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/` | 161/161 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | 1154/1154 |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |
| `godot --headless --path . -- --bridge-selftest` / `--caravan-selftest` | PASS / PASS |
| `dotnet build Ashfall.csproj` | 0 errors, no new warnings |
| architecture map / save-store matrix / catalog registry `--check` | in sync |

---

## ACTIVE HANDOFF — AGY (Antigravity): GENERATE THE UI PANELS

**Task:** implement the presentation wave for the completed economy core. The
Core authorities, read models, and typed price factors already exist — panels
must RENDER them, never recompute them (plan §9.6).

**Read first, in order:**

1. `DESIGN.md` — tactical UI style (BarlowCondensed caps headers,
   ShareTechMono body, 1px hard borders, no round corners, 1920x1080 fixed,
   segmented `[|||| ]` bars, no color-only meaning).
2. `docs/plans/C1_planintegration.md` — §14A.8/§14A.9 (economy read model +
   economy_detail behavior), §14B.7/§14B.8 (regional heat map + read-model
   refresh discipline), §9.1/§9.2 (panel surface requirements).
3. `AGENTS.md` — non-negotiable rules (esp. UI owns presentation only).

**Panels to generate/extend:**

- `economy_detail` — embargo banner (active weather, affected regions,
  multipliers, decay days remaining), regional price heat map (5 regions ×
  goods, cheap/neutral/expensive TEXT labels + optional color reinforcement,
  keyboard navigable), blocked-route indicator, neutral state when clear.
- `traveling_caravan` — origin region + specialty, blocked/slowed state with
  the weather reason string, paused/slowed route progress, stale blocked state
  must clear immediately when the embargo clears.

**Data sources to bind (read models only):**

- `MarketSystem.ExplainPrice(itemId, side, region)` — typed
  `PriceFactorRecord` rows (`PriceFactorKind.Regional/Embargo`) are the ONLY
  approved source for price decomposition copy.
- `TradeEmbargoSystem.GetEmbargoSummary(weather)` — `EmbargoSummary` struct.
- `TravelingCaravanSystem` — `CaravanEntry.embargoBlocked`,
  `OnCaravanEmbargoed`/`OnCaravanResumed`, `GetRouteProgressMultiplier`.

**Constraints:** extend the existing panels registered in
`PanelRegistryBootstrap` (`economy_detail`, `traveling_caravan`); no new panel
authorities; no gameplay logic in UI; accessibility: never color-only, keep
keyboard close/back behavior; refresh on economy/weather transition events,
not per frame.

**Verify:** `godot --headless --path . -- --panel-bind-lifecycle-selftest`,
`--ui-a11y-selftest` (if registered), `bash scripts/run_test.sh
Ashfall.Core.Tests/UI/PanelRouteGateTests.cs`, `dotnet build Ashfall.csproj`.

---

## What is NOT done yet (remaining C1 waves)

- **The economy UI wave (above) is DONE as of Wave 8 B1 (2026-09-17):** both
  panels render the Core read models (embargo banner, regional heat map,
  caravan route states with the event-clearing edge); the combined Phase 1
  economy gate is complete end-to-end. Evidence: the plan's checkpoint report
  plus panel lifecycle/a11y/caravan/bridge gates.
- C1.4 crisis prediction → C1.5 cloud seeding → C1.6 trophies → C1.7/C1.8
  kennel → C1.9 hardening → C1.10 content expansion tranche (+30%) are ALL
  COMPLETED as of Wave 9 Part 1 (2026-09-17).
- C1 Chain Status: `C1 CORE/MECHANISMS CLOSED — SURFACE DEFERRED`.
  Daily briefing surface integration is deferred per §5.17 and documented in
  `A1_BRIEFING_DEFERRED.md` due to active claim `claim-c1-plan24-survivor-ledger-2026-09-16`.
  Core crisis prediction models, weather intelligence coordinator integration,
  weather forecast panel presentation, cloud seeding runtime consumer, trophy
  system and shelter decor integration, companion animal kennel verification,
  and the +30% content expansion tranche are 100% verified green.

## Content expansion note (user-directed, 2026-09-15)

The plan's **Phase 22 — Content Expansion Tranche (+30%)** is FULLY IMPLEMENTED:
- embargo rules: 10 → 14 rules (+Ashfall, BloodRain, ThermalInversion, ParticulateFog)
- regional price atlas: 18 → 24 entries (coastal region added with trap_fish, clean_water, tools)
- cloud-seeding bulk recipe: +1 (`craft_silver_iodide_cartridge_bulk` at distiller)
- trophies: 8 → 11 rows (Ash Hound, Dust Lynx, Iron Crow added with recipes and items)
- kennel companion animals: +2 species profiles (`species_rad_dog`, `species_iron_crow`)
- companion events: +2 acquisition events (`event_drowning_pup_rescue`, `event_expedition_stray_follows_home`)
- radio broadcasts: +4 flavor broadcasts (2 embargo market alerts, 2 kennel milestones)
- All rows pass `--data-integrity-selftest` (333 catalogs, 0 errors).
