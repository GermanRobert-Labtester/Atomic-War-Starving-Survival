# Plan 78 — Archive Inks Expansion (3 → 12 ink types)

## Goal (2 lines)
Expand `archive_inks.json` from 3 verified ink types to 12. The archive ink system
defines ink types used for document preservation — each ink has legibility,
archival longevity, fade rate, and a required crafting ingredient. The
`ArchiveInkCatalogLoader` and `ArchiveDeskHostSession` are confirmed live, but 3
inks is too few for a document-preservation system.

## Why (P2)
- Verified: `archive_inks.json` has 3 entries (ink_id, display_name,
  legibility_score, archival_longevity_days, fade_rate_per_day, required_item_id,
  required_amount). `ArchiveInkCatalogLoader.cs` and `ArchiveDeskHostSession.cs`
  are confirmed live.
- Creates the archival-preservation pillar: inks determine how long documents
  remain legible. Better inks last longer but cost more to craft. This makes
  document preservation a resource decision, not a free action.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/archive_inks.json` (expand 3 → 12 inks)
- Read-only: `Assets/Ashfall.Core/ArchiveInkCatalogLoader.cs` (confirm schema)
- `Assets/StreamingAssets/Data/items.json` (required_item_id must resolve)

## Content grammar (per ink)
- snake_case `id` with prefix `ink_` (confirmed prefix).
- Tier system: improvised (plant dye, soot, charcoal paste) / standard (iron
  gall, lamp black, indigo) / archival (carbon pigment, polymer-based salvage,
  vacuum-sealed pre-war stock).
- Each ink: distinct legibility_score (0.3–0.95), archival_longevity_days
  (100–1000), fade_rate_per_day (0.0005–0.003), required_item_id (must resolve in
  items.json), required_amount (1–4).
- Trade-off: high-legibility inks fade faster; long-archival inks are less
  legible. No ink is best at everything.

## Steps
1. Read `ArchiveInkCatalogLoader.cs` to confirm the ink schema and how
   legibility/fade are applied to documents.
2. Read `items.json` to confirm which crafting ingredient items exist
   (charcoal, cloth, and others); note gaps for step 5.
3. Author 9 new inks across 3 tiers:
   - Improvised (4): charcoal paste, berry stain, ash slurry, bone-black ink.
   - Standard (3): lamp black, indigo extract, ferrous tannate.
   - Archival (2): carbon pigment compound, pre-war vacuum ink (rare salvage).
4. Each ink: unique legibility/longevity/fade profile, distinct required_item_id.
5. Add any missing ingredient items to `items.json` (e.g. `berries`, `ash`,
   `bone_black`, `indigo_powder`) — only if an ink's required_item_id does not
   exist.
6. Cross-reference: every ink_id unique; every required_item_id resolves in
   items.json; required_amount is a positive integer.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: archive ink catalog loads 12 inks, all ids unique, all required_item_id
   resolve, legibility and fade rates within valid ranges, no two inks have
   identical profiles.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is required_item_id resolution (step 6): confirm
all ingredient items exist before authoring.

## Definition of Done
- `archive_inks.json` has 12 inks, all ids resolving, all required_item_id
  resolving in items.json, integrity + tests green.

## Follow-on
- Plan 51 (environmental storytelling documents) — inks determine document
  preservation quality.
- Plan 47 (collectibles) — pre-war vacuum ink is a rare collectible.
- Plan 55 (crafting recipes) — ink crafting recipes use these ingredients.
- Existing 17 (environmental storytelling) — preserved documents are
  environmental story entries.
