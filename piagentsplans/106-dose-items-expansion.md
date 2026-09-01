# Plan 106 — Dose Items Expansion (5 → 15 dose-ledger items)

## Goal (2 lines)
Expand `dose_items.json` from 5 verified items to 15. The dose item system
(`ItemCatalogLoader.cs` confirmed live) defines items specific to the
dose-ledger system — dosimeters, ledgers, calibration tools, tags, and
palliative supplies. 5 items is too few for a radiation-bureaucracy system
with 12 bands (Plan 90) and 12 quests (Plan 101).

## Why (P2)
- Verified: `dose_items.json` has 5 entries (id, name, weightKg, tradeValue,
  category, description). The dose item system is confirmed live via
  `ItemCatalogLoader.cs` and `WarlordDoctrineCatalog.cs`.
- Creates the dose-equipment pillar: the dose-ledger system needs physical
  tools — dosimeters, calibration keys, tags, ledgers, shielding, and
  palliative supplies. 5 items covers the basics; 15 covers the full range
  of radiation-management equipment.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dose_items.json` (expand 5 → 15 items)
- Read-only: `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` (confirm
  schema and how dose items are loaded)

## Content grammar (per item)
- snake_case `id` with prefix `item_` (confirmed prefix).
- `name`: 1–3 words (The Dose Ledger, Dosimeter Calibration Key).
- `weightKg`: 0.01–2.0 (tools are light, ledgers are heavy).
- `tradeValue`: 0–100 (story items are 0, tools are valuable, medical items
  are very valuable).
- `category`: story / tool / medical / protective / consumable.
- `description`: 1–2 sentences describing the item. Match the existing
  quality — grounded, specific, slightly bureaucratic. These are the tools
  of a radiation-tracking bureaucracy.
- Item categories: measurement (dosimeters, calibration tools), documentation
  (ledgers, tags, boards), protection (shielding, suits), palliative
  (morphine, iodine, chelation), and cohort (children's baseline items).

## Steps
1. Read `ItemCatalogLoader.cs` to confirm the schema and how dose items are
   loaded (are they a separate catalog or merged into the main item
   catalog?).
2. Read the existing 5 items to confirm the quality bar and naming
   convention.
3. Author 10 new items across 5 categories:
   - Measurement (2): pocket dosimeter (personal exposure tracker),
     geiger counter (area survey tool).
   - Documentation (2): dose register book (the formal ledger), cohort
     baseline card (per-child tracking card).
   - Protection (2): lead-lined apron (partial shielding for the
     examiner), dosimeter holster (worn on the belt, keeps the dosimeter
     accessible and protected).
   - Palliative (2): chelation kit (reduces body burden, single-use),
     iodine prophylaxis pack (thyroid protection, 10 doses).
   - Cohort (2): children's chalk (for the baseline board, erasable),
     growth chart (tracks children's development against radiation
     exposure).
4. Each item: distinct id, name, weightKg, tradeValue, category, and
   description. Match the existing grounded, bureaucratic tone.
5. Cross-reference: every item id unique; every id follows the `item_*`
   convention; check if ids need to resolve in items.json (if the dose
   catalog is merged with the main catalog).
6. Wire 3 items into Plan 101 (dose quests — quests grant dose items).
7. Wire 2 items into Plan 90 (dose registers — care plans consume dose
   items).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: dose item catalog loads 15 items, all ids unique, weightKg and
   tradeValue within valid ranges, all categories valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is catalog merging (step 1): confirm whether
dose items are loaded as a separate catalog or merged into the main item
catalog — if merged, ids must not collide with existing item ids.

## Definition of Done
- `dose_items.json` has 15 items, all ids unique, 3 wired to dose quests,
  2 wired to dose register care plans, integrity + tests green.

## Follow-on
- Plan 101 (dose quests) — quests grant dose items.
- Plan 90 (dose registers) — care plans consume dose items.
- Plan 81 (dose locations) — dose items are used at dose locations.
- Plan 55 (crafting recipes) — some dose items are craftable.
- Plan 46 (scavenging) — dose items are scavenged from medical/scientific
  sites.
