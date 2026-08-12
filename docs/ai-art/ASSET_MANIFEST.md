# ASSET_MANIFEST.md (AI-art pipeline tracking)

> Not to be confused with the deprecated code-audit manifest at `audit/ASSET_MANIFEST.md` (item-icon file paths only, closed 2026-08-08, pre-dates the catalog's growth to 419 items). This one tracks the AI-art pipeline state — drafted → generated → painted-over → shipped — per `docs/HUMAN_AUTHORSHIP.md`'s no-raw-AI-output rule.

## Pilot batch (this run)

| id | name | category | primary model | status | prompt location |
|---|---|---|---|---|---|
| `stone_wall_bg` | Memorial Wall stone background | UI texture | FLUX.2 Pro | DRAFTED | `prompts/pilot_batch.md` |
| `cracked_glass_overlay` | Keepsake cracked-glass overlay | VFX/UI overlay | FLUX.2 Pro | DRAFTED | `prompts/pilot_batch.md` |
| `vignette_sepia` + `vignette_blue` | Phantom memory vignette pair | VFX/UI overlay | Nano Banana Pro | DRAFTED | `prompts/pilot_batch.md` |
| `icon_eye` | Hypervigilance eye badge | UI icon | Recraft | DRAFTED | `prompts/pilot_batch.md` |
| `icon_shield` + `icon_heart` | Moral branch icon pair | UI icon | Recraft | DRAFTED | `prompts/pilot_batch.md` |
| `icon_pill` | Addiction detox pill badge | UI icon | Recraft | DRAFTED | `prompts/pilot_batch.md` |
| `memorial_name_plate` | Memorial name plate template | UI prop | FLUX.2 Pro | DRAFTED | `prompts/pilot_batch.md` |
| `geiger_counter` | Geiger Counter | Inventory icon | Nano Banana 2 | DRAFTED | `prompts/pilot_batch.md` |
| `elena_vasquez` | Elena Vasquez portrait | Character | Nano Banana Pro | DRAFTED | `prompts/pilot_batch.md` |
| `env_shelter_medical_bay` | Shelter Medical Bay | Environment / key art | FLUX.2 Max | DRAFTED | `prompts/pilot_batch.md` |

**DRAFTED** = prompt written and QC-passed against `PROMPT_RULES.md`, not yet run through a generator. Next states: **GENERATED** (raw model output exists) → **PAINTED-OVER** (human pass applied per `docs/HUMAN_AUTHORSHIP.md`'s checklist) → **SHIPPED**.

## Remaining Phase 11 Canva list (`INTEGRATION_PLAN_FOR_CURSOR.md` §III) not individually drafted
- `icon_hourglass`, `icon_checkmark` — same recipe as `icon_pill` (Recraft, same stroke weight), symbol/color swapped only.
- `badge_background` — same recipe as `stone_wall_bg`'s flat-UI-texture register, small badge-circle variant.

## Full-catalog coverage (as of 2026-08-12)

Two external, pre-existing Desktop libraries were discovered this pass and are now the production baseline (see `EXISTING_PROMPT_AUDIT.md` for the earlier, narrower audit — this supersedes its coverage counts):
- `ASHFALL_Firefly_Item_Icon_Prompts.md` — 321 items individually prompted.
- `ASHFALL_Firefly_Flux_200_Asset_Prompts.pdf` — 200 entries: key art, shelter rooms, 12 generic survivor/visitor archetypes, events, UI/map/VFX.

`docs/ai-art/prompts/FULL_CATALOG_EXPANSION.md` (synced copies: game root `ASHFALL_PROMPT_CATALOG_EXPANSION.md`, Desktop `New Folder (1)/ASHFALL_PROMPT_CATALOG_EXPANSION.md`) fills the gap between those two files and the live catalog data:

| Family | Total in data | Covered before this pass | Covered by this file | Now at |
|---|---|---|---|---|
| **Items** | 419 (`items.json`) | 249 (321-item file, actual per-item match) | +170 | 419/419 |
| **Locations** | 47 (`locations.json`) | 5 (only ids that actually match the PDF's pre-expansion location names) | +42 | 47/47 |
| **Survivors** | 96 (`survivors.json`) | 0 individually (PDF only has 12 generic profession archetypes) | +96 | 96/96 |
| **Factions** | 5 (GDD Ch2.3) | 0 | +5 (lineup-level, new territory) | 5/5 |
| **Weather** | 22 (`WeatherKind` enum) | 7 (generic icon sheet + key art panoramas) | +15 | 22/22 |

Prompt depth differs by family on purpose: items/locations/survivors are compact production-table rows (id + one dense visual-delta sentence); the 10 pilot-batch assets in `prompts/pilot_batch.md` remain the only entries with full multi-model Master Asset Briefs. Scale up any row to that depth on demand before it actually goes through a generator.

**Status:** all of the above are DRAFTED (prompt written, not yet run through a generator). None are GENERATED yet.

## Backlog size (for scale planning — not drafted yet)
- Everything in the table above is now drafted at table-row depth. What's *not* yet done: running any of it through a generator, the human-authorship paint-over pass, and expanding any individual row to a full Master Asset Brief if a generator needs more than the compact prompt.

Scaling from pilot to full batch: follow `PROMPT_RULES.md`'s compilation formula per catalog entry, run each against the QC checklist, and update this table's status column as pieces move through the human-authorship pipeline. `FULL_CATALOG_EXPANSION.md`'s own footer documents how to add new rows as expansion work adds more items/locations/survivors.
