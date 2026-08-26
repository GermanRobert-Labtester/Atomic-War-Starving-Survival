#!/usr/bin/env python3
"""Phase 14B — Family classification + manifest + priority for the missing queue.

Reads Phase-13 wiring matrix and produces:
  docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json  (the canonical plan)

The visual_asset_manifest.json is wrapped (active_assets list); tolerate both
list and dict schema forms so the script does not break if the schema changes.
"""
import json
import re
from pathlib import Path
from collections import Counter

REPO = Path(__file__).resolve().parent.parent
DATA = REPO / "Assets/StreamingAssets/Data"

WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))

# Manifest schema: tolerate both list and wrapped dict
_manifest_obj = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
if isinstance(_manifest_obj, dict) and "active_assets" in _manifest_obj:
    MANIFEST = _manifest_obj["active_assets"]
elif isinstance(_manifest_obj, list):
    MANIFEST = _manifest_obj
else:
    MANIFEST = []

# Build asset stem index
art_stems = {}
for r in MANIFEST:
    if not isinstance(r, dict):
        continue
    fp = (r.get("full_path") or r.get("file_path") or r.get("path")) or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    if not fp or "/" not in fp:
        continue
    parts = fp.split("/")
    if len(parts) < 2:
        continue
    stem = r.get("stem") or r.get("asset_id") or ""
    art_stems.setdefault(stem.lower(), (fp, parts[1], r))

# Catalog → family mapping
CATALOG_TO_FAMILY = {
    "items": ("Inventory-Item", "item"),
    "year_of_ash_items": ("Inventory-Item", "item"),
    "holdfast_items": ("Inventory-Item", "item"),
    "crossing_items": ("Inventory-Item", "item"),
    "verdict_items": ("Inventory-Item", "item"),
    "black_flotilla_items": ("Inventory-Item", "item"),
    "foundry_items": ("Inventory-Item", "item"),
    "greenhouse_items": ("Inventory-Item", "item"),
    "dose_items": ("Inventory-Item", "item"),
    "economy_goods": ("Inventory-Item", "item"),
    "survivors": ("Survivor-Portrait", "portrait"),
    "expansion_survivor_fields": ("Survivor-Portrait", "portrait"),
    "year_of_ash_survivors": ("Survivor-Portrait", "portrait"),
    "characters": ("NPC-Portrait", "portrait"),
    "locations": ("Location-Art", "location"),
    "year_of_ash_locations": ("Location-Art", "location"),
    "holdfast_locations": ("Location-Art", "location"),
    "crossing_locations": ("Location-Art", "location"),
    "verdict_locations": ("Location-Art", "location"),
    "deep_lore_locations": ("Location-Art", "location"),
    "dose_locations": ("Location-Art", "location"),
    "locations_expansion3": ("Location-Art", "location"),
    "factions": ("Faction-Art", "faction"),
    "holdfast_factions": ("Faction-Art", "faction"),
    "crossing_factions": ("Faction-Art", "faction"),
    "standing_record_factions": ("Faction-Art", "faction"),
    "recipes": ("Reference-Skip", "item"),
    "relic_recipes": ("Reference-Skip", "item"),
    "faction_war_radio": ("Reference-Skip", "faction"),
    "faction_war_journal": ("Reference-Skip", "faction"),
    "faction_war_dialogue": ("Reference-Skip", "faction"),
    "faction_war_communiques": ("Reference-Skip", "faction"),
    "faction_war_location_overrides": ("Reference-Skip", "location"),
    "faction_war_events": ("Reference-Skip", "faction"),
}

ID_HINTS = [
    ("Medical", ("med_", "bandage", "iodine_pills", "iodine_", "morphine", "syrette",
                 "splint", "suture", "first_aid_kit", "medical_kit", "antibiotic",
                 "vacc", "inhaler", "painkiller", "tourniquet", "tweezers",
                 "anti_rad", "prussian_blue", "alcohol_wipes", "thermometer",
                 "stethoscope", "bleach", "burn_salve", "epinephrine",
                 "epipen", "rad_away", "poppy_latex", "scopolamine",
                 "amphetamines", "opium_raw", "sedative_vial",
                 "amnestic_syrup")),
    ("Ammunition", ("item_ammo", "ammo_", "magazine", "shell_casing",
                    "armor_piercing", "hollow_point", "tracer", "incendiary",
                    "tranquilizer", "depleted_uranium")),
    ("Weapons", ("weapon_", "pistol_", "rifle_", "shotgun_", "sniper_",
                 "smg", "carbine", "lmg", "rpg", "ak47", "m4a1",
                 "crossbow", "pipe_", "machete", "knife_", "axe_",
                 "fireaxe", "baseball_bat", "crowbar",
                 "sledgehammer", "chainsaw", "throwing_axe", "rope_",
                 "stun_baton", "bow", "dart")),
    ("Equipment", ("armor_", "helmet_", "kevlar_", "coveralls_", "ghillie",
                   "snowshoes", "snow_goggles", "cog_", "gear_",
                   "gas_mask", "gasmask", "hazmat", "mask_", "goggles_",
                   "headlamp", "binoculars", "night_vision", "compass",
                   "wrist_", "dosimeter", "geiger_", "holster",
                   "sheath", "vest", "boots", "gloves_", "mittens")),
    ("Food-Water", ("food_", "water_", "mre_", "canned_", "ration_",
                    "ration", "spirits", "tea", "coffee", "chocolate",
                    "honey", "jerky", "bottle", "tuna", "beans", "meat",
                    "wheat", "grain", "dried_", "preserved_", "cereal",
                    "lard", "tallow", "clean_water", "dirty_water",
                    "bottled_water", "bacon", "caviar", "spices",
                    "salt", "sugar", "stew", "porridge", "kibble")),
    ("Crafting-Material", ("item_scrap", "scrap_", "crafting_", "mechanical_",
                           "electronic_", "aluminum_", "steel_", "iron_",
                           "copper_", "wire_", "cable_", "cloth",
                           "wood", "metal_", "glass_", "plastic_",
                           "rubber_", "charcoal_", "lead_", "gunpowder",
                           "sulfur", "saltpeter", "fertilizer",
                           "asbestos", "paper_", "fabric_",
                           "leather_", "industrial")),
    ("Special-Resource", ("rel", "lore_", "keycard_", "document",
                          "encrypted", "music_box", "keepsake",
                          "heirloom", "codex", "log_", "ledger",
                          "chit", "locket")),
]

FAMILY_DIMS = {
    ("Inventory-Item", "Medical"):      (256, 256, "transparent", "1:1 icon"),
    ("Inventory-Item", "Ammunition"):   (256, 256, "transparent", "1:1 icon"),
    ("Inventory-Item", "Weapons"):      (384, 384, "transparent", "1:1 weapon silhouette"),
    ("Inventory-Item", "Equipment"):    (256, 256, "transparent", "1:1 gear icon"),
    ("Inventory-Item", "Food-Water"):   (256, 256, "transparent", "1:1 consumable icon"),
    ("Inventory-Item", "Crafting-Material"): (256, 256, "transparent", "1:1 material icon"),
    ("Inventory-Item", "Special-Resource"): (256, 256, "transparent", "1:1 quest item"),
    ("Inventory-Item", "Other"):       (256, 256, "transparent", "1:1 generic"),
    ("Survivor-Portrait", "Other"):     (512, 512, "opaque", "head/shoulder portrait"),
    ("NPC-Portrait", "Other"):          (512, 512, "opaque", "head/shoulder portrait"),
    ("Location-Art", "Other"):          (1024, 1024, "opaque", "16:9 location plate"),
    ("Faction-Art", "Other"):           (512, 512, "opaque", "monochrome emblem"),
    ("Reference-Skip", "Other"):        None,
}

# Gameplay importance weighting
ITEM_TYPE_BOOST = {
    "Medical": 1.5,
    "Ammunition": 1.3,
    "Weapons": 1.4,
    "Crafting-Material": 1.2,
    "Food-Water": 1.4,
    "Equipment": 1.2,
    "Special-Resource": 0.7,
    "Other": 1.0,
}
SOURCE_BOOST = {
    "items": 1.5,
    "survivors": 1.8,
    "characters": 1.4,
    "locations": 1.5,
    "year_of_ash_items": 0.7,
    "year_of_ash_locations": 0.7,
    "holdfast_items": 0.8,
    "holdfast_locations": 0.8,
    "crossing_items": 0.7,
    "crossing_locations": 0.7,
    "verdict_items": 0.6,
    "verdict_locations": 0.6,
    "black_flotilla_items": 0.6,
    "foundry_items": 0.6,
    "greenhouse_items": 0.6,
    "dose_items": 0.6,
    "economy_goods": 0.6,
    "deep_lore_locations": 0.5,
    "locations_expansion3": 0.7,
}


def subfamily_for(kind, cid):
    cid = cid.lower()
    for bucket, keys in ID_HINTS:
        for k in keys:
            if k in cid:
                return bucket
    return "Other"


def priority_score(family, sub, source, kind):
    sub_boost = ITEM_TYPE_BOOST.get(sub, 1.0)
    src_boost = SOURCE_BOOST.get(source, 1.0)
    base = {"Survivor-Portrait": 3.0, "NPC-Portrait": 2.5,
            "Location-Art": 2.5, "Faction-Art": 2.0,
            "Inventory-Item": 2.0}.get(family, 1.0)
    return round(base * sub_boost * src_boost, 3)


def priority_band(score):
    if score >= 2.5: return "P1"
    elif score >= 1.5: return "P2"
    elif score >= 1.0: return "P3"
    else: return "P4"


def dims_for(family, sub):
    key = (family, sub)
    if key in FAMILY_DIMS:
        return FAMILY_DIMS[key]
    if (family, "Other") in FAMILY_DIMS:
        return FAMILY_DIMS[(family, "Other")]
    return (256, 256, "transparent", "1:1 generic")


def reference_assets_for(family, content_id):
    """Pick 3-8 existing assets in art/ whose stem shares a keyword."""
    cid_keys = [k for k in content_id.lower().split("_") if len(k) >= 3]
    if not cid_keys:
        return []
    cands = []
    for stem, (path, dir_kind, meta) in art_stems.items():
        if dir_kind != "art":
            continue
        score = sum(1 for k in cid_keys if k in stem)
        if score > 0:
            cands.append((score, stem, path, meta.get("width"), meta.get("height")))
    cands.sort(key=lambda x: -x[0])
    return [{"stem": s, "file_path": p, "width": w, "height": h}
            for _, s, p, w, h in cands[:8]]


# Build manifest
missing_only = [e for e in WM if e["resolved_path"] == "MISSING"]
manifest = []
for e in missing_only:
    catfile = Path(e["catalog"]).stem
    fam_default, kind_default = CATALOG_TO_FAMILY.get(catfile, ("Inventory-Item", "item"))
    if fam_default == "Reference-Skip":
        manifest.append({
            "content_id": e["content_id"],
            "source_catalog": catfile,
            "visual_family": "Reference-Skip",
            "subfamily": "Reference",
            "kind": kind_default,
            "generation_status": "SKIP_REFERENCE_ONLY",
            "qa_status": "N/A",
            "wiring_status": "DOCUMENTED_MISSING",
            "runtime_status": "NOT_RUNTIME_VISIBLE",
        })
        continue
    sub = subfamily_for(kind_default, e["content_id"])
    w, h, alpha, aspect = dims_for(fam_default, sub)
    refs = reference_assets_for(fam_default, e["content_id"])
    score = priority_score(fam_default, sub, catfile, kind_default)
    band = priority_band(score)
    target_subdir = {
        "Inventory-Item": "items",
        "Survivor-Portrait": "portraits",
        "NPC-Portrait": "portraits",
        "Location-Art": "locations",
        "Faction-Art": "factions",
    }.get(fam_default, "items")
    cid_clean = re.sub(r"[^a-z0-9_]+", "_", e["content_id"].lower()).strip("_")
    if fam_default == "Inventory-Item" and not cid_clean.startswith(("item_",)):
        target_filename = f"item_{cid_clean}.jpg"
    elif fam_default in ("Survivor-Portrait", "NPC-Portrait") and not cid_clean.startswith(("survivor_", "npc_")):
        target_filename = f"survivor_{cid_clean}.jpg"
    elif fam_default == "Location-Art" and not cid_clean.startswith(("loc_", "location_")):
        target_filename = f"loc_{cid_clean}.jpg"
    elif fam_default == "Faction-Art" and not cid_clean.startswith("faction_"):
        target_filename = f"faction_{cid_clean}.jpg"
    else:
        target_filename = f"{cid_clean}.jpg"

    # If the target file already exists, mark it as skip
    if (REPO / "assets/art" / target_filename).exists():
        continue

    manifest.append({
        "content_id": e["content_id"],
        "canonical_name": e["content_id"].replace("_", " ").title(),
        "content_type": kind_default,
        "visual_family": fam_default,
        "subfamily": sub,
        "source_catalog": catfile,
        "runtime_priority": band,
        "gameplay_importance": score,
        "target_filename": target_filename,
        "target_directory": "assets/art/",
        "target_extension": ".jpg",
        "target_width": w,
        "target_height": h,
        "aspect_ratio": aspect,
        "alpha_requirement": alpha,
        "art_direction_profile": "ashfall_2d_survival_gouache",
        "reference_assets": refs,
        "semantic_description": f"A 2D painted icon for {fam_default} '{e['content_id']}', readable at icon scale (~64×64).",
        "required_objects": [],
        "forbidden_objects": [
            "real-country flag", "real brand mark", "logo", "watermark",
            "rendered-text label", "neon cyberpunk",
            "anime-style eyes", "fantasy ornament", "glossy sci-fi glow",
            "stock-photo backdrop"
        ],
        "generation_status": "PENDING",
        "qa_status": "NOT_STARTED",
        "wiring_status": "DOCUMENTED_MISSING",
        "runtime_status": "NEVER_BOUND",
    })

band_order = {"P0": 0, "P1": 1, "P2": 2, "P3": 3, "P4": 4}
manifest.sort(key=lambda r: (band_order.get(r.get("runtime_priority", "P4"), 9),
                              -r.get("gameplay_importance", 0.0),
                              r["content_id"]))

out_dir = REPO / "docs/visual"
out_dir.mkdir(exist_ok=True)
out_dir.joinpath("PRODUCTION_ART_GENERATION_MANIFEST.json").write_text(json.dumps(manifest, indent=1))
print(f"→ wrote PRODUCTION_ART_GENERATION_MANIFEST.json ({len(manifest)} rows)")

gentarget = [r for r in manifest if r["generation_status"] != "SKIP_REFERENCE_ONLY"]
_skip = [r for r in manifest if r["generation_status"] == "SKIP_REFERENCE_ONLY"]
band_count = Counter(r["runtime_priority"] for r in gentarget)
fam_count = Counter(r["visual_family"] for r in gentarget)
print(f"\nGeneration target: {len(gentarget)} (Reference-Skip: {len(_skip)})")
print("Priority bands:")
for b in ("P1", "P2", "P3", "P4"):
    print(f"  {b}: {band_count.get(b, 0)}")
print("Family distribution:")
for f, n in fam_count.most_common():
    print(f"  {f:24s}: {n}")
print()
print("Subfamily distribution (Inventory-Item only):")
sub_count = Counter(r["subfamily"] for r in gentarget if r["visual_family"] == "Inventory-Item")
for s, n in sub_count.most_common():
    print(f"  {s:24s}: {n}")
