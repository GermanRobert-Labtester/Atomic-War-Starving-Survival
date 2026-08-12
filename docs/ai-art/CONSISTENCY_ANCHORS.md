# CONSISTENCY_ANCHORS.md

Tracks what must stay identical across every future generation of a recurring subject. This file is seeded by the pilot batch, not invented ahead of art existing — entries get locked in only once a piece is actually generated and approved.

## Location anchor: The Shelter (Tessarat sub-pen)
- **Seed reference:** `UI_StyleReference_01.jpg` — the one finished piece in the repo (also the Figma main-menu background). Check every future shelter-room illustration against it for linework weight, palette, and light logic before accepting it.
- **Fixed:** 2D graphic-novel ink illustration; single warm practical bulb/fixture as the dominant light source; cold blue-grey light from any window/exterior opening; bare concrete/brick with visible damage; exposed conduit and wiring; clutter mixing scavenged domestic and military objects.
- **Free to vary per room:** furniture set, room-specific equipment (Medical Bay ≠ Power Room ≠ Hydroponics), camera framing.
- **Rooms still needing a first piece:** Medical Bay (this batch — `env_shelter_medical_bay`), Hydroponics, Power Room, Decontamination, Workshop, Radio Room. Once Medical Bay is approved, add it here as a second reference alongside the menu background.

## Character anchor: Elena Vasquez (`elena_vasquez`)
- **Status:** no art yet — this batch's portrait becomes the seed reference once generated and approved.
- **Fixed once seeded:** age range (30s–40s), thin build (rationing-worn), medical smock over layered scavenged clothing, restrained/composed expression, steady hands as a deliberate visual detail (per her authored bio: "her hands never shake").
- **Why she's first:** she's the only pilot character with a named quest already in the codebase (`quest_elena_triage`), so she's the most likely to need a second, consistent appearance soon.

## UI icon family anchor (`icon_eye`, `icon_shield`, `icon_heart`, `icon_pill`, `icon_hourglass`, `icon_checkmark`)
- **Fixed:** single stroke weight, no fill gradients, symmetrical construction, flat vector (Recraft), semantic color from `design-tokens.json` — not `INTEGRATION_PLAN_FOR_CURSOR.md`'s inline hex where the two disagree (see `EXISTING_PROMPT_AUDIT.md`; e.g. that doc's heart-icon `#42A5F5` vs. the token file's `moral_compassion #6ea3a8` — token file wins, it's what actually feeds the USS).
- **Free to vary:** the symbol itself and its assigned color.
- `icon_eye` is generated first in this batch and becomes the literal stroke-weight/size reference for the other five.

## Faction anchors (drafted in `prompts/FULL_CATALOG_EXPANSION.md` Part D — not yet generated)
Five factions exist (GDD Ch2.3), no art yet. Prompts are written as establishing lineups (3-4 figures, 16:9). When first generated, lock: **Central Garrison Remnants** — disciplined military-surplus gear, faded insignia. **Upland Provincial Militia** — hunting/agrarian gear, local-made patches. **Cultists of the Glow** — robes of scavenged fabric, symbols painted in blood/rust (already specified in the old prompt file — worth keeping). **Scavenger Warlords** — mismatched scavenged armor, improvised weapons. **Safe Haven Communities** — deliberately under-armed civilian silhouette, to read as non-threatening against the other four. Once each lineup is approved, the named NPCs that belong to it (`NPC_AshWidows`, `NPC_TheTollman`, `NPC_BurnedPatrol`, `NPC_TheCollector`, `NPC_FeralChildren`, `NPC_SurgeonsCaravan`, `NPC_Bandits`) should reuse its visual DNA rather than being generated independently.

## Palette anchor
See `GAME_VISUAL_DNA.md` for full detail. Illustrative tier: ash blue / charcoal grey / rust orange / mud brown, warm amber practical-light accent. UI semantic tier: exact hex from `Assets/_Game/UI/Phase11/design-tokens.json` — don't re-derive by eye.
