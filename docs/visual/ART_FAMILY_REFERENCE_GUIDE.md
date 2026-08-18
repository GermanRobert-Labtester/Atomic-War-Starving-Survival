# ASHFALL — Art Family Reference Guide

**Date:** Phase 16.
**Purpose:** Establish approved reference assets per visual family so generation prompts can lock palette, lighting, composition, and material language consistently.

## Global ASHFALL Art Language

The project's visual identity is fixed:

```
ORIGINAL 2D HAND-PAINTED SURVIVAL-MANAGEMENT ARTWORK

charcoal pencil underdrawing
dry gouache / worn painted texture
grounded grim realism

charcoal          #2A2A2C
concrete grey     #5C5F62
faded blue-grey   #708085
rust brown        #6E4A2F
dirty bone        #B5A88A
rare muted amber  #A26E2C  (1-2% of pixels)
subtle cyan-green #4F7461  (only for contamination)
```

Atmosphere is `ash, nuclear winter, condensation, repair marks, worn functional materials, frost, dirt, scarcity, industrial decay`.

### Hard visual negatives

- text, letters, numbers, logos, brands, watermarks, flags, AI signatures, nonsense glyphs
- neon cyberpunk, glossy sci-fi, fantasy ornament, cartoon, anime, oversaturation, stock-photo photorealism
- gratuitous gore, weapon glamour
- duplicated objects, malformed anatomy, bad hands, broken perspective, AI geometry artifacts

---

## Family 1 — Inventory-Item (Equipment)

### Reference assets (3-8)

| Asset | Stems | Notes |
|---|---|---|
| `assets/art/hazmat_suit.jpg` | `hazmat_suit` | Anchors "industrial safety gear, worn" treatment |
| `assets/art/geiger_counter.jpg` | `geiger_counter` | Anchors "vintage Soviet-era measurement device, scratched glass" |
| `assets/art/iodine_pills.jpg` | `iodine_pills` | Anchors "small survival bottle, faded label" |
| `assets/art/anti_rad.jpg` | `anti_rad` | Anchors "pharmaceutical container, dusty" |
| `assets/art/flashlight.jpg` | `flashlight` | Anchors "metal tool, dings, paint loss" |
| `assets/art/duct_tape.jpg` | `duct_tape` | Anchors "soft salvage supply, half-unspooled" |
| `assets/art/crowbar.jpg` | `crowbar` | Anchors "heavy metal tool, dirty handle" |
| `assets/art/first_aid_kit.jpg` | `first_aid_kit` | Anchors "metal tin box, scratched red cross" |

### Derived style

- **Camera:** eye-level, slight 3/4 turn, never overhead, never pointing at camera.
- **Composition:** single subject, ~60% of canvas, isolated.
- **Lighting:** diffuse overcast with soft key from upper-left ~30°.
- **Palette:** charcoal / concrete grey / rust brown / dirty bone.
- **Materials:** brushed metal, raw wood, dented tin, dirty plastic, oxidised copper, worn leather — never polish, never varnish, never chrome.
- **Background:** transparent-to-charcoal gradient, no setting, no horizon.
- **Runtime readability:** must remain recognisable in silhouette at 32×32.

### P1 sample targets

`iodine_tablets`, `clean_water_jug`, `dried_rations`, `military_rations`, `water_purification_tablets`, `water_sample_contaminated`, `spirits`, `grain_exchange_scale_weight`, `mira_chalk_ration_token`, `ration_plaza_paint_stick`.

---

## Family 2 — Location-Art

### Reference assets (3-8)

| Asset | Stem | Notes |
|---|---|---|
| `assets/art/abandoned_hospital.jpg` | `abandoned_hospital` | Anchors "pre-war concrete, broken windows, ash" |
| `assets/art/suburban_house.jpg` | `suburban_house` | Anchors "post-blast residential ruin" |
| `assets/art/government_bunker.jpg` | `government_bunker` | Anchors "deep concrete bunker, blast door" |
| `assets/art/rural_gas_station.jpg` | `rural_gas_station` | Anchors "industrial ruin, faded signage" |
| `assets/art/stranger_cache.jpg` | `stranger_cache` | Anchors "salvage cache, hidden" |
| `assets/art/bunker_fractured.jpg` | `bunker_fractured` | Anchors "structural damage, exposed rebar" |
| `assets/art/bunker_generator.jpg` | `bunker_generator` | Anchors "industrial machinery, diesel staining" |
| `assets/art/bunker_greenhouse_uv_lamp.jpg` | `bunker_greenhouse_uv_lamp` | Anchors "functional survival space, lit by sodium lamp" |

### Derived style

- **Camera:** 3/4 isometric or wide-flat (forced-perspective board-game style).
- **Composition:** full environmental scene with silhouette landmarks readable from 256×256.
- **Lighting:** overcast daylight + single warm sodium lamp / fire pit where appropriate.
- **Palette:** heavy charcoal/concrete grey, 30% rust brown decay, 1-2% muted amber.
- **Materials:** weathered concrete, peeling paint, exposed rebar, broken glass, dead vegetation.
- **Background:** never pure — atmospheric ash haze or simplified silhouette horizon.
- **Runtime readability:** silhouette of dominant landmark must read at 64×64 panel preview.

### P1 sample targets

`loc_grain_silo`, `loc_ration_queue_plaza`, `loc_checkpoint_kilo_armory`, `loc_collapsed_building`, `loc_concert_hall_ruins`, `loc_convoy_echo7_cache`, `loc_electrical_substation`, `loc_family_bunker_backyard_shed`, `loc_hospital_pharmacy`.

---

## Family 3 — Survivor-Portrait (anchored)

### Reference assets (3-8)

| Asset | Stem | Notes |
|---|---|---|
| `assets/art/crazed_survivor.jpg` | `crazed_survivor` | Anchors "drawn face, hollow eyes, stubble" |
| `assets/art/survivor_child.jpg` | `survivor_child` | Anchors "child portrait, oversized coat" |
| `assets/art/survivor_female_1.jpg` | `survivor_female_1` | Anchors "female survivor, exhausted" |
| `assets/art/dying_survivor.jpg` | `dying_survivor` | Anchors "gaunt, hollow, last-stretch" |
| `assets/art/enc_frozen_family.jpg` | `enc_frozen_family` | Anchors "family grouping, weary" |
| `assets/art/lore_faded_family_photograph.jpg` | `lore_faded_family_photograph` | Anchors "old photograph, faded colour" |

### Derived style

- **Camera:** head-and-shoulders, eye-level, slight 3/4 turn.
- **Composition:** subject fills frame, 80% of canvas, chest up.
- **Lighting:** window-light from upper-left, deep shadow on one side of face.
- **Palette:** desaturated skin tones, cool shadow, no warm lighting.
- **Materials:** skin, hair, fabric, eye sclera. No makeup, no perfect skin.
- **Background:** solid murky mid-grey or blurred silhouette (no scene).
- **Critical:** no anime-style eyes, no fantasy hair, no glamour makeup.

### P1 sample targets

`survivor_family_adult`, `survivor_family_child`.

---

## Family 4 — NPC-Portrait

### Reference assets (3-8)

Same as Survivor-Portrait (the survivor roster is the canonical NPC base). 35 P1 NPCs are drawn from `characters.json`:

`npc_cluster_teacher`, `npc_ansel_duth`, `npc_benno_kade`, `npc_bram_ostrowski`, `npc_cael_ormund`, `npc_dara_mewn`, `npc_dessa_vane`, `npc_doctor_ianov`, `npc_dr_irina_vel`, `npc_edor_vale`, `npc_hadi_morrow`, `npc_halden_mire`, `npc_ira_vell`, `npc_ivo_fenn`, `npc_ivor_lasko`, `npc_kess_adler`, `npc_len_quill`, `npc_leva_quist`, `npc_maren_holt`, `npc_mattis_cray`, `npc_nila_brant`, `npc_nomi_fisk`, `npc_osran_kell`, `npc_osric_tann`, `npc_perrin_ashby`, `npc_piet_abar`, `npc_quil_esser`, `npc_saria_voss`, `npc_sergeant_pell`, `npc_tamsin_rook`, `npc_the_cartwright_sisters`, `npc_wren`, `npc_wyn_omah`, `npc_wyn_sabler`, `npc_yara_holm`.

### Derived style

Same as Survivor-Portrait, with the addition of *occupation cue* through clothing detail (doctor's collar, soldier's fatigue, scout's hood). NO occupation badge, NO label, NO text on chest.

---

## Family 5 — Faction-Art

### Reference assets (3-8)

| Asset | Stem | Notes |
|---|---|---|
| `assets/art/emblem_iron_raiders.jpg` | `emblem_iron_raiders` | Anchors "scratched metal plate, hammered edge" |
| `assets/art/emblem_cold_count.jpg` | `emblem_cold_count` | Anchors "frosted sigil, narrow stamp" |
| `assets/art/emblem_hydro_barons.jpg` | `emblem_hydro_barons` | Anchors "water-marked plate, rivets" |
| `assets/art/emblem_long_walk.jpg` | `emblem_long_walk` | Anchors "worn cloth patch, faded dye" |

### Derived style

- **Camera:** flat, frontal, square crop.
- **Composition:** centre single icon, strong silhouette, 65% of canvas.
- **Lighting:** diffuse, no harsh shadow.
- **Palette:** faction-tinted (rust, charcoal, dirty bone, muted amber). Rarely the faction's name in text.
- **Materials:** metal/wood/cloth — never plastic, never neon.
- **Background:** solid dirty bone, faint texture, no setting.

### P2 sample targets

`faction_the_compact`, `faction_the_cutters`, `faction_the_fleet`, `faction_the_office`, `faction_the_overlay`, `faction_the_scale`, `faction_the_underwrite`.

---

## Family 6 — Ammunition (placeholder protected)

The following three assets are **active generic placeholders** for caliber-agnostic ammo rendering and are NOT to be replaced or quarantined until per-caliber resolution is verified:

| Asset | Path | Status |
|---|---|---|
| `item_ammo_ap.jpg` | `assets/art/item_ammo_ap.jpg` | KEEP_ACTIVE_PLACEHOLDER |
| `item_ammo_hp.jpg` | `assets/art/item_ammo_hp.jpg` | KEEP_ACTIVE_PLACEHOLDER |
| `item_ammo_standard.jpg` | `assets/art/item_ammo_standard.jpg` | KEEP_ACTIVE_PLACEHOLDER |

Per-caliber specific art (e.g. `ammo_12ga_ap.jpg`, `ammo_9mm.jpg`) is canonical and resolves correctly through the AssetRegistry.

---

## Notes for downstream

- This guide is the **canonical reference** for any model used to generate Phase 16 batch art.
- The `production_prompt_composer.py` script reads the manifest + manifest-derived reference assets and emits a per-content_id prompt JSON in `docs/visual/generated_prompts/`. Each prompt encodes the family style above.
- The `production_manifest.py` script can update reference assets per content_id if better anchors become available.
- The `production_ledger.py` script records each generated attempt with its model and prompt ID.
