# Production-island wiring log

**Date:** 2026-09-19
**Branch:** `Zcode_Branch`
**Authorization:** user — execute the open (LIVE) ranked plans; do not start Plan 30 / 32B / 32C / 34; do not merge closed PRs #53/#55.

## Phase 0 — Port contract (36C slice)

Status: PASS (partial 36C; ratchet still open)

- Classified Plan 28C `SubsystemManifest.RegisterSetupAction` as `HOST_REQUIRED`.
- Activated `StealthSystem.RegisterWeaponNoise` (`DEFERRED` → `HOST_REQUIRED`).
- Reclassified `ShelterThermalSystem.RegisterExternalBurst` (`DEFERRED` → `LIVE_VIA_CORE`; WeatherHardening freeze bursts).
- Reclassified `EquipmentConditionSystem.RegisterProfile` (`DEFERRED` → `LIVE_VIA_CORE`; `LoadProfiles` is host-called).
- Classified newly landed Core seams as `TEST_ONLY` (no `src/` callers): ChildDevelopment, DiscoveryConsequence, DynamicQuestGenerator, MemoryDecay, PersonalBelongings, RelationshipDecay, ShelterNoise, ShelterSecurity, Cartography, TunnelNetwork, Rumor, ItemLore.
- Regenerated `PORT_CONTRACT.md` / `port-contract.json`.
- Did **not** merge closed PRs #53/#55.

Verify: `python3 scripts/ci/generate-port-contract.py --check` PASS — **262 seams, 178 HOST_REQUIRED bound, 14 DEFERRED**. `PortContractGateTests` 8/8.

## Phase 1 — Equipment degradation catalog

Status: PASS

- `EquipmentConditionHostSession.LoadCatalog` reads `item_degradation.json`.
- `SetupEquipmentCondition` loads it after built-ins. Authored profile id/family keys override matching built-ins.
- Tests: `EquipmentConditionCatalogTests` 4/4.

## Phase 2 — Weather hardening → thermal

Status: PASS

- Freeze threshold now `RegisterExternalBurst` on pipes in the frozen room and fires `OnPipeBurst`.
- Installed thermal-retention upgrades `SetExternalInsulationModifier` on the matching thermal room each tick.
- Tests: `WeatherHardeningSystemTests` 7/7.

## Phase 3 — Weapon noise consumer

Status: PASS (Core + host registration + combat fire)

- `StealthSystem.ApplyWeaponNoise` adds registered handling/melee/fired noise into `PartyStealthState.accumulatedNoise`.
- Host registers assault / pipe / suppressed rifle profiles at stealth setup.
- Combat session carries `Stealth` + `StealthExpeditionId`; `ActionFire` on Success applies Fired noise.
- Tests: `StealthSystemTests` 8/8 including suppressed vs unsuppressed.

## Phase 4 — Pneumatic blackout toggle

Status: PASS (small)

- Debug `blackout` action no longer flips a private flag; it re-derives served state from the foundry/workshop bus.

## Phase 5 — Campaign RNG (World + Expedition + remaining DemoSeed hosts)

Status: PASS (production Create paths; DemoSeed remains headless fallback)

- `CampaignDayCoordinator.RestoreState` rebuilds `CampaignRngManager` from the saved master seed before restoring positions.
- `WorldHostSession.Create(dataDir, campaignRng)` derives weather bind + intelligence from `CampaignStreamIds.Weather`.
- `ExpeditionHostSession.Create(..., campaignRng)` per-day `Fork(Expedition, CurrentDay)`; vehicles `Fork(Expedition, 0, 1)`.
- Remaining host Create paths now take optional `ICampaignRngManager` (DemoSeed if null):
  - Radio → `CampaignStreamIds.Radio`
  - Combat encounter + roll seeds → `CampaignStreamIds.Combat`
  - Narrative `SelectDemo` → `CampaignStreamIds.Narrative`
  - Maritime scavenge/safe-crack → `CampaignStreamIds.Maritime`
  - Deep-coast dock salvage → new `CampaignStreamIds.DeepCoast` (`deep_coast`)
  - Dose ledger constructor stream → `CampaignStreamIds.Medical`
- Economy `TickDay` was already campaign-forked; unused `DemoSeed` constant left in place.
- Tests: `CampaignDayCoordinatorTests` 19/19; `CampaignRngSourceGateTests` 2/2.

Limitation: mid-day `TickHours` suffix after reload is still day-granularity. `_core.DeepCoast` itself still uses ice-road `seedSalt`, not the campaign stream.

## Phase 6 — Mechanical regional treaties

Status: PASS (prior slice)

- Host loads `regional_treaties.json` via `RegionalTreatyCatalogLoader` (DTO properties).
- No longer maps narrative `regional_treaty_protocols.json` or foundry accords as the mechanical catalog.
- Tests: `RegionalTreatyCatalogLoaderTests` 2/2; `RegionalTreatySystemTests` 8/8.

## Phase 7 — Fixture-fallback purge

Status: PASS (unbound grids)

Unbound panels now call `AshfallDataGrid.UnavailableRows` instead of inventing fixture gameplay:

- FactionsNarrative, SkillMatrix, Greenhouse, ExpeditionRadar (prior)
- DutyRoster, DoseLedger, MapAtlas (all three quadrants), SurvivalWorkstation, SilentFoundry, FactionMatrix (this slice)

`BuildFixtureRows` methods remain in those files but are unused on the unbound path. Tests: `ProductionUiNoFabricatedFallbackGateTests` 4/4.

## Phase 8 — Craft specialty / Plan 32A

Status: STALE (no change)

- Workshop→specialty already live via `Main.Phase0.OnCraftCompletedForSpecialty`. Do not restore `CraftContext`.
- `DEBT-PLAN32-MAP-ORPHANS` is RETIRED.

## Phase 9 — 36C deferred ratchet shrink

Status: PASS (2 DEFERRED remain)

PRs #53/#55 are merged on `main`. Equivalent evidence applied on `Zcode_Branch` (not GitHub-merged onto this branch):

- LIVE_VIA_CORE: naval `RegisterVessel`, thermal `RegisterThermalGear`, downtime `RegisterHobby`, tell `RegisterBand`/`RegisterTellPool`, ice-road `RegisterHoldfastNode` (defaults now call it), narrative `RegisterAdapter` (defaults now call it), map `RegisterTrapSiteLocation` (ctor now calls it).
- OPTIONAL_HOST: loot `RegisterCategory`, stance `RegisterFactions`, espionage `BindFactionResolver` (already defaults to canonical IDs).
- TEST_ONLY: `SkillProgressionSystem.RegisterDefaultSkills` (zero-op; skills.json is authority).
- **Not deleted:** `CraftingSystem.BindCraftResultGate` — `_isCraftResultAllowed` is read by CanCraft/preview. PR 55's "never read" premise is stale.
- Left DEFERRED: `BindCraftResultGate` (no host binder) and `SpiritualMeaningCoordinator.RegisterDeath` (Plan 30).
- Expedition `StartDive` now forks campaign Expedition RNG (DemoSeed only when unbound).

Verify: `--check` PASS — **262 seams, 178 HOST_REQUIRED, 2 DEFERRED**. `PortContractGateTests` 8/8; `IceRoadSystemTests` 10/10; `WastelandMapCatalogLoaderTests` 5/5.

## Phase 10 — leftover full integration (authorized)

Status: PASS (0 DEFERRED)

- **Craft result gate:** `CraftingHostSession.Create` binds `BindCraftResultGate` to `ItemCatalog.Contains` when the catalog loaded. Unknown result ids cannot be crafted. `CraftingSystemTests` 10/10.
- **Spiritual coordinator (Plan 30 meaning axis, not war-clock):** `SetupSpiritual` loads `SpiritualCatalogLoader`; `SurvivorFate.OnSurvivorFate` calls `RegisterDeath`; daily `TickMourning`; memorial `OnMourned` performs `ShelterVigilRiteId` (`memorial_rite_roll_call_naming`); Iron Cenotaph shows mourning-arc and open-rite counts; `spiritual_meaning` save section (194 sections). DX-02 closed; loader allowlist pruned.
- Port contract `--check` PASS: **262 seams, 180 HOST_REQUIRED, 0 DEFERRED**.

## Phase 11 — remaining blocked items (user: finish everything)

Status: PASS (bounded)

- **War clock (D5 map, not JSON rewrite):** `ToAuthoredDay(180)=480`, `307=607`. Year of Ash `TickDay` feeds the chain runner the authored clock. Clash/decree already radio+journal+sound-ranging; stage/chain events now do the same. Returning sorties call `RecordWarLocationVisited` + map `DiscoverVisited`.
- **32B/32C expedition:** graph distance overlays catalog ticks when a discovered path exists; Unknown fog map nodes refuse dispatch. Caravan not migrated.
- **Difficulty live consumer:** `SetupDifficulty` loads the XP-01 catalog; hostile-encounter scalar multiplies the existing expedition danger composer. Completion-history schema left at v1 (no checksum break).

## Phase 12 — authorized follow-ups (close remaining later items)

Status: PASS

- **Caravan graph:** traveling caravans expand authored hops through `WastelandMapSystem.PlanRoute` and refuse Unknown fog; trade-network transit days overlay graph ticks when region IDs resolve to map nodes. Daily caravan tick now passes campaign day + Economy RNG so route encounters can fire. Trade network `TickDay` runs from the expeditions/caravans day owner.
- **30C wildlife overlay:** `WildlifeMigrationSystem.MergeSectorAdjacency` keeps seed neighbors and adds sectors connected by wasteland-map location seeds. World host applies the overlay after map restore.
- **Completion history v2:** new records stamp `difficultyPresetId`; v1 checksums unchanged (`[NonSerialized]` + versioned hash). `CampaignCompletionHistoryTests` 11/11; `WildlifeMapOverlayTests` 1/1.

Crop-roster dirty files on `Zcode_Branch` intentionally untouched.
