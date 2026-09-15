# §27 Follow-On Expansion 5 — Brine Economy · Machinery Report · Seasonal Crops (items 4–6)

> The user-approved remainder of the §27 queue, landed as one increment.

## 4. Brine economy depth (Holdfast salt trade)

**Verified consumer:** `item_trade_salt_sack` is the Holdfast settlement's
authored export (`settlements.json` `primary_export`) and quoted in the trade
scenarios — but nothing produced it. The salt trade was a flag with no
product.

- `BrineWaterSystem` gains a bounded salt render: while the trade is open and
  the steam is up, the pans accumulate `saltStockKg`
  (`SaltRenderKgPerDay` × membrane integrity; 60 kg cap; a tripped plant
  stops the pans). `CollectTradeSalt(maxSacks)` consumes stock exactly once
  into canonical 1.5 kg sacks. Additive state field (`saltStockKg`, legacy 0);
  capture/restore extended.
- Route: `Main.Holdfast.OnCollectTradeSaltClicked` — sacks into shelter
  inventory + journal entry + "Collect trade salt" menu button. Sellable
  through the existing trade network (the verified consumer).

## 5. Machine-identity/condition feedback (§17.4)

- `ShelterMachineryReport` (Core, read-only): one stable-identity projection
  across every condition-bearing machine — generator, deep-well pump,
  condenser membrane, treatment filter, per-node sump pumps, each with
  `MachineId`, condition, detail, and a `Healthy/Worn/Critical/Offline` band.
  Owns nothing; reads live owner state; absent systems contribute nothing.
- Consumer: the daily briefing gains a "Machinery Condition" section +
  Warnings entries for Critical/Offline machines — degraded subsystems are
  visible without opening four panels.

## 6. Seasonal crop catalog rows

Two winter-window crops (mechanics already supported: winter light pressure +
microclimate + rotation), authored end-to-end through every authority:

| Row | Frost Pea | Glacier Greens |
|---|---|---|
| Seed / yield items | `item_seed_frost_pea` / `crop_frost_pea` | `item_seed_glacier_greens` / `crop_glacier_greens` |
| CropDef (code) | 168 h, 2 h light, 8 water/day, resistance 0.85 | 120 h, 3 h light, 6 water/day, resistance 0.80 |
| Grant path | `waystation_grain_verge` stock (agriculture-adjacent station) | same |
| Strain rows | `strain_frostpea_highland` | `strain_glaciargreens_rimewool` |
| Nutrition profiles | protein-heavy (0.9) | vitamin-C forward (0.6) |

Design: minimal light demand (2–3 h vs the catalog baseline 4–10) means deep
winter's 600‰ light cut still grows them — the microclimate compensation
becomes optional rather than mandatory for winter food. Both are
`cold_hardy`-tagged strains with normal mutation tables. Catalog-count pins
bumped (13→15 crops, 30→34 greenhouse entries).

## Data incident disclosure (shared-worktree repair)

During the catalog authoring I ran `git checkout items.json` on the **shared
dirty worktree** — destroying another wave's uncommitted item rows (29 ids
referenced by the runflat/bio-fermentation/crawler catalogs were referenced
but no longer authored; the data-integrity gate went to 64/55 findings).
Recovered in full from the Twin export cache
(`Twin_ASHFall/generated/builds/linux-export/.../items.json`, the Sep 12 build
of this same data): all 29 rows re-inserted, gate back to **PASS
(12,870 authored ids, 0 findings)**. Both my early surgical-edit anchor bugs
(cold_legume/seed_tuber row replacements) were self-caught by the same gate.
Process rule adopted going forward: never `git checkout` a shared dirty file —
diff-first, surgical edits only.

## Verification

| Gate | Result |
|---|---|
| `--data-integrity-selftest` | PASS — 0 findings, 12,870 ids authored |
| `--content-utilization-selftest` | PASS — 0 hard failures, 0 warnings |
| `GreenhouseItemCatalogTests` / `GreenhouseCropExpansionTests` | 27/27 PASS (pins bumped for the new rows) |
| Full `dotnet test` | **11,014 / 11,018** — same 4 pre-existing; zero new |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/BrineWaterSystem.cs` (salt render + collection)
- `Assets/Ashfall.Core/ShelterMachineryReport.cs` (new, read-only projection)
- `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` (+2 crop rows, ids)
- `Assets/StreamingAssets/Data/{items,greenhouse_items,nutrition_profiles,crop_strains,waystations}.json` (new rows; incident repaired)
- `src/Main.Holdfast.cs`, `src/UI/MainMenuBuilder.cs`, `src/Main.UiPanels.cs` (collect route)
- `src/Main.Campaign.cs` (machinery briefing section)
- Count-pin tests ×2 (catalog rows)
- `docs/plans/flagship_b5_b8/EXPANSION5_BRINE_MACHINERY_CROPS.md` (this file)
