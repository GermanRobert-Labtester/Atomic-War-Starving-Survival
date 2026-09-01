# Dive Loot Provenance (Plan 23 / Task 23B)

Every maritime loot source, its provenance, and its determinism contract. Authoritative
data: `dive_sites.json` (`loot_table` = `VariableLootNode` grammar, `safes[].loot`),
`MaritimeHostSession` (host node seed retained for non-site surfaces).

## Loot categories and provenance

| Category | Sources | Example ids | Notes |
|---|---|---|---|
| Flotilla material culture | strongroom/cistern/picket loot; Flotilla trade | item_claim_tag_stamped, item_marine_sealant_kit, item_rebreather_canister | identity objects zero-value by design |
| Technical salvage | all wrecks | scrap_mechanical/electronic, brass_fittings, item_process_barrel | buys-at-premium at Flotilla |
| Repair materials | depot/siphon/cistern | item_ro_resin, item_process_barrel, fuel (bounded) | no infinite fuel node |
| Relics | picket craft (bell), sovereign | item_ships_bell_picket (unique node, one-time) | war-grave memorial hook |
| Dossiers/documents | sovereign purser safe, metro | item_fleet_log_cylinder, paper_scrap | single authoritative source |
| Medical goods | quarantine barge, cistern | medical_kit, iodine_pills | contaminated-flag handled by scavenge |
| Trade cargo | convoy, ferry, barge | canned_food (degrades → spoiled), cloth | bounded |
| Marine gear | cistern, deep sites | descent line, lamp, canister, sealant | gear loop |
| Ammunition/weapon salvage | convoy, armory locker | ammo_9x19 (small weights, degraded chance) | bounded, never farms |
| Code/quest objects | purser safe (log cylinder), claim tags | item_fleet_log_cylinder, item_claim_tag_stamped | quest/radio consumers |

## Determinism contract (pinned by tests)

- Selection: `ProceduralScavengeSystem` + `ISeededRng` (xorshift64*), stable candidate
  order (list order from the catalog), no filesystem/dictionary order dependence.
- Quantities: Poisson-skewed within [MinQty, MaxQty], skewed by world phase and
  per-location visit count (picked-over effect) — deterministic for equal state+seed.
- Degradation: `DegradationChance` → half quantity + `DegradedItemId` swap.
- One-time rewards: safe loot transfers once (`lootTransferred` persisted); resolved
  node results that must never reroll are the site safes + quest objects — enforced by
  `SafeCracking_SiteSafes_ResolveThroughLiveRuntime_AndPersist`.
- Unique quest objects: `item_fleet_log_cylinder` has exactly one authored source (the
  purser safe); `item_ships_bell_picket` appears only in the picket grave node.
- Never regenerates on load: resolved safes/nodes ride the checksummed maritime save
  envelope; old bare-state saves restore through the legacy fallback untouched.
