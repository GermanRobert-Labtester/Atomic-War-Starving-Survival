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

## Backlog size (for scale planning — not drafted yet)
- **Inventory:** 419 items across 17 categories, 0 with art (`items.json`; supersedes the deprecated 316-count manifest). 1 template drafted this batch (`geiger_counter`).
- **Survivors:** 96 archetypes, 0 with art. 1 drafted this batch (`elena_vasquez`).
- **Locations:** 47 scavenging sites + 7 shelter rooms, 0 with art. 1 drafted this batch (`env_shelter_medical_bay`).
- **Factions:** 5, 0 with art. Anchors pre-seeded in `CONSISTENCY_ANCHORS.md` for the next batch.

Scaling from pilot to full batch: follow `PROMPT_RULES.md`'s compilation formula per remaining catalog entry, run each against the QC checklist, and update this table's status column as pieces move through the human-authorship pipeline. No new process is needed — the pilot validated the system end to end.
