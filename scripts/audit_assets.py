#!/usr/bin/env python3
"""
Phase 14 — Visual Asset Audit.

Scans:
  - assets/  (active Godot tree — every folder counts)
  - Assets/  (Unity legacy tree — annotated separately)

Generates:
  - docs/visual/visual_asset_manifest.json
  - docs/visual/VISUAL_ASSET_AUDIT.md

Run from the repository root with:
  python3 scripts/audit_assets.py
"""
import hashlib
import json
import os
import re
import shutil
import sys
from pathlib import Path
from collections import Counter, defaultdict

REP_ROOT = Path(__file__).resolve().parent.parent
ACTIVE_ROOT = REP_ROOT / "assets"
LEGACY_ROOT = REP_ROOT / "Assets"
DOCS = REP_ROOT / "docs" / "visual"
DOCS.mkdir(parents=True, exist_ok=True)

# AssetRegistry path resolution chain (mirrored from src/Host/AssetRegistry.cs)
ITEM_PATTERNS = [
    "res://assets/art/{id}.jpg",
    "res://assets/art/{id}.png",
    "res://assets/sprites/Items/{id}.png",
    "res://assets/sprites/items/{id}.png",
]
PORTRAIT_PATTERNS = [
    "res://assets/art/{id}.jpg",
    "res://assets/art/{id}.png",
    "res://assets/sprites/Portraits/{id}.png",
    "res://assets/sprites/portraits/{id}.png",
]
LOCATION_PATTERNS = [
    "res://assets/art/{id}.jpg",
    "res://assets/art/{id}.png",
    "res://assets/sprites/Locations/{id}.png",
    "res://assets/sprites/locations/{id}.png",
]
FACTION_PATTERNS = [
    "res://assets/art/{id}.jpg",
    "res://assets/art/{id}.png",
    "res://assets/sprites/Factions/{id}.png",
    "res://assets/sprites/factions/{id}.png",
]
WEATHER_PATTERNS = [
    "res://assets/art/{id}.jpg",
    "res://assets/art/{id}.png",
    "res://assets/sprites/Weather/{id}.png",
    "res://assets/sprites/weather/{id}.png",
]

IMAGE_EXT = {".png", ".jpg", ".jpeg", ".webp", ".svg"}

CAT_PREFIX = {
    "ammo": "Item/Ammo",
    "armor": "Item/Equipment",
    "artifact": "Item",
    "anti_": "Item/Medical",
    "bandage": "Item/Medical",
    "battery": "Item/Device",
    "beast": "Creature/Mutant",
    "bio_": "Item",
    "biolog": "Item",
    "blood": "Item/Medical",
    "blueprint": "Item",
    "botany": "Item/Material",
    "bunker": "Location/Shelter",
    "burn": "Item/Medical",
    "canned": "Item/Consumable",
    "cargo": "Item",
    "car": "Item",
    "cassette": "Item",
    "catchment": "Item/Equipment",
    "charcoal": "Item/Medical",
    "char": "Item",
    "city": "Environment/City",
    "clean_": "Item/Consumable",
    "cloth": "Item/Material",
    "codex_": "Item",
    "collapsed": "Environment",
    "combat": "Item/Weapon",
    "compass": "Item/Tool",
    "component": "Item/Material",
    "condiment": "Item/Consumable",
    "convert": "Item",
    "cooked": "Item/Consumable",
    "cookware": "Item/Tool",
    "copper": "Item/Material",
    "corpse": "Creature",
    "crafting_": "Item/Crafting",
    "craft": "Item/Crafting",
    "crazed": "Character/NPC",
    "crop": "Item",
    "crossbow": "Item/Weapon",
    "crowbar": "Item/Tool",
    "crow": "Creature",
    "cult_": "Faction",
    "d_": "Item",
    "damage": "Effect",
    "danger": "UI",
    "dark": "Item",
    "data_": "Item",
    "dead": "Creature/Character",
    "death": "Character",
    "debris": "Item",
    "decon": "Item/Medical",
    "dehydrated": "Effect/Status",
    "dialog": "UI",
    "dirty_": "Item/Consumable",
    "disinfect": "Item/Medical",
    "disposal": "Item/Tool",
    "dosim": "Item/Device",
    "drain_": "Item",
    "drone_": "Item",
    "drone": "Device",
    "drum_": "Item",
    "dump_": "Location",
    "dust_": "Effect",
    "dying": "Character",
    "e_": "Item",
    "electric": "Item/Device",
    "elena_": "Character/Portrait",
    "electronic": "Item/Material",
    "emp_": "Effect/VFX",
    "en": "Environment",
    "enc_": "Encounter",
    "encounter_": "Encounter",
    "engagement_": "UI",
    "engineering": "Item/Crafting",
    "epi_pen": "Item/Medical",
    "equip_": "Item/Equipment",
    "escape_": "Item",
    "event_": "UI/Event",
    "evt_": "Encounter/Event",
    "exclamation": "UI",
    "exp_": "Item",
    "expedition_": "UI/Expedition",
    "exploration_": "Item",
    "f_": "Item",
    "faction_": "Faction",
    "fallback": "Placeholder",
    "fallout_": "Effect",
    "faraday": "Item/Device",
    "farm": "Location",
    "fatigue": "UI/Status",
    "feral": "Creature",
    "field": "Location",
    "fire_": "Effect/VFX",
    "fish_": "Item",
    "flare": "Item/Tool",
    "flash": "Item/Device",
    "flashlight": "Item/Device",
    "flashpoint": "Effect",
    "flint": "Item/Tool",
    "flooded": "Environment",
    "food_": "Item/Consumable",
    "forest": "Environment",
    "forged": "Item",
    "fortified": "Item",
    "frag": "Item",
    "fuel": "Item/Consumable",
    "g_": "Item",
    "game_": "UI",
    "gamma": "Effect",
    "garden": "Location",
    "gas_": "Item/Device",
    "gear_": "Item/Equipment",
    "geiger_": "Item/Device",
    "generator": "Item/Device",
    "ghost": "Environment/Character",
    "glass_": "Item/Material",
    "glowing": "Effect",
    "gold": "Item",
    "government": "Location",
    "grenade": "Item/Weapon",
    "guide": "UI",
    "gun_": "Item/Weapon",
    "gunpowder": "Item/Material",
    "h_": "Item",
    "hammer": "Item/Tool",
    "hand_": "Item",
    "handbook": "UI",
    "hands": "Item",
    "hard_": "Item",
    "hatch": "Location/Shelter",
    "haversack": "Item",
    "hazard_": "Effect",
    "haze": "Effect",
    "hazmat": "Item/Equipment",
    "he": "Item",
    "health_": "UI/Status",
    "heater": "Item/Device",
    "heavy_": "Item/Equipment",
    "herb": "Item/Medical",
    "high_": "Item",
    "hint_": "UI",
    "history": "UI/Item",
    "hoard": "Item",
    "holo": "Effect/VFX",
    "hood": "Item",
    "horror": "Encounter",
    "hostage_": "Encounter",
    "hover": "UI",
    "hunting_": "Item/Weapon",
    "hydroponic": "Item/Equipment",
    "hypo": "Effect/Status",
    "i_": "Item",
    "ice_": "Environment",
    "incendiary": "Item/Ammo",
    "indoor": "Location",
    "infected": "Effect",
    "inflatable": "Item",
    "injector": "Item/Medical",
    "ink_": "Item",
    "insta": "Item",
    "intelligence_": "Item",
    "inter": "UI",
    "intercept_": "UI/Radio",
    "iodine": "Item/Medical",
    "iron": "Item/Material",
    "irrad": "Effect/Status",
    "item_": "Item",
    "jerry": "Item/Consumable",
    "journal": "UI/Item",
    "k_": "Item",
    "kerosene": "Item/Consumable",
    "key": "Item",
    "knife": "Item/Weapon",
    "knuckles": "Item/Weapon",
    "l_": "Item",
    "labor": "Location",
    "lake": "Environment",
    "laser": "Effect/VFX",
    "law_": "UI",
    "lead": "Item/Material",
    "leather": "Item/Equipment",
    "level_": "UI",
    "lifeboat": "Encounter",
    "light_": "Item",
    "liquid_": "Item",
    "loaded": "Item/Weapon",
    "lock": "Item/Tool",
    "lore_": "Item/Lore",
    "lose": "UI",
    "luggage": "Item",
    "magazine_": "Item/Weapon",
    "map_": "UI/Map",
    "machete": "Item/Weapon",
    "machine": "Item/Device",
    "magazine": "Item/Weapon",
    "makeshift": "Item/Equipment",
    "mangrove": "Environment",
    "maritime": "Environment/Water",
    "marker": "UI",
    "marker_": "UI",
    "mechanical": "Item/Equipment",
    "medical": "Item/Medical",
    "medicinal_": "Item/Medical",
    "medkit": "Item/Medical",
    "med_": "Item/Medical",
    "message_": "UI",
    "metal_": "Item/Material",
    "military": "Item/Equipment",
    "mil_": "Item",
    "mine": "Item/Weapon",
    "mission": "UI",
    "mod_": "Item/WeaponMod",
    "mold": "Item/Medical",
    "molotov": "Item/Weapon",
    "morale_": "UI/Status",
    "morphine": "Item/Medical",
    "mosaic_": "UI",
    "mosque_": "Location",
    "mount": "Item/Equipment",
    "multitool": "Item/Tool",
    "music": "UI",
    "mutagen": "Item/Medical",
    "mutant": "Creature/Mutant",
    "muzzle_": "Effect/VFX",
    "mysterious": "Character/NPC",
    "n_": "Item",
    "nano": "Item",
    "needle": "Item/Medical",
    "network_": "Item",
    "neutral_": "Item",
    "night_": "Item/Device",
    "npc_": "Character/NPC",
    "nuclear": "Effect",
    "obj_": "Item",
    "objective_": "UI",
    "oil_": "Item",
    "opioid": "Item/Medical",
    "orange_": "UI",
    "organic": "Item/Material",
    "pack_": "Item",
    "paid": "UI",
    "paint": "Item/Tool",
    "pair": "Item",
    "paper": "Item",
    "parachute": "Item",
    "park_": "Environment",
    "pda": "UI",
    "perk_": "Item/Perk",
    "personal_": "UI",
    "photo": "Item/Lore",
    "pipe_": "Item/Weapon",
    "pliers": "Item/Tool",
    "police": "Character/NPC",
    "poppy": "Item/Medical",
    "portable": "Item/Device",
    "portrait": "Character/Portrait",
    "powder": "Item/Material",
    "power_": "Item/Device",
    "preset": "Item",
    "prisoner": "Character/NPC",
    "progress": "UI",
    "propane": "Item/Consumable",
    "protective_": "Item/Equipment",
    "prototype_": "Item",
    "prussian": "Item/Medical",
    "psi": "Effect",
    "public_": "Location",
    "pure": "Item",
    "purified": "Item/Consumable",
    "q_": "Item",
    "qual_": "Item",
    "quarantine": "Location/Shelter",
    "question": "UI",
    "r_": "Item",
    "radiation": "Effect/Status",
    "radio_": "Item/Device",
    "radio": "UI/Radio",
    "rails_": "UI",
    "ram_": "Item",
    "raw": "Item",
    "ranger_": "Item",
    "rare_": "Item",
    "rc_": "UI",
    "reactor_": "Location",
    "rebel": "Character/NPC",
    "receiver_": "Item",
    "reclaimed": "Item",
    "reconn": "Item/Tool",
    "recorder": "UI",
    "recruit_": "UI",
    "recycled": "Item",
    "red": "UI",
    "redirect": "Item",
    "reference": "Reference",
    "refinery_": "Item",
    "reflex": "Item",
    "regulator_": "Item/Device",
    "reinforced": "Item/Equipment",
    "relic_": "Item/Lore",
    "remaining_": "UI",
    "remote_": "Item/Device",
    "repair_": "Item",
    "resolution": "UI",
    "resource_": "Item",
    "respirator": "Item/Equipment",
    "reticle": "Item/Device",
    "retreat_": "UI",
    "return_": "UI",
    "revolver": "Item/Weapon",
    "risk_": "UI",
    "rival_": "Encounter",
    "rivet": "Item",
    "rlg": "Item",
    "roadblock": "Encounter",
    "rope": "Item/Tool",
    "rose_": "UI",
    "rotated_": "UI",
    "rover_": "Item",
    "ruined": "Environment",
    "rural": "Environment",
    "rus_": "Item",
    "russian_": "Item",
    "rusty_": "Item",
    "s_": "Item",
    "safe_": "Item/Equipment",
    "sage_": "UI",
    "salt": "Item/Consumable",
    "sambuca_": "Item",
    "sat_": "Item/Device",
    "satellite": "Item/Device",
    "sawn_": "Item/Weapon",
    "scanner_": "Item/Device",
    "scarce": "Item",
    "scavenge_": "Item",
    "scavenged_": "Item",
    "scavenger_": "Character/NPC",
    "scheme_": "Item",
    "schematic": "Item/Lore",
    "screwdriver": "Item/Tool",
    "scrap_": "Item/Material",
    "sealing_": "UI",
    "secret_": "Item",
    "sedative": "Item/Medical",
    "select_": "UI",
    "self_": "Item",
    "sensor": "Item/Device",
    "shadowy_": "Character",
    "shield_": "Item/Equipment",
    "ship_": "Item",
    "shirt": "Item",
    "shoes_": "Item",
    "shop_": "UI",
    "shotgun": "Item/Weapon",
    "shovel": "Item/Tool",
    "shrub_": "Item",
    "sickbay_": "Location",
    "silenced_": "Item/Weapon",
    "silhouette": "UI",
    "silicone": "Item/Medical",
    "simple_": "UI",
    "skirmish_": "UI",
    "sleeping_": "Item/Equipment",
    "sleepwalk": "Encounter",
    "small_": "Item",
    "smoke_": "Effect/VFX",
    "smuggler_": "Character",
    "snare_": "Item",
    "snow_": "Environment",
    "socks_": "Item",
    "soil_": "Item",
    "solar_": "Item/Device",
    "soldier": "Character/NPC",
    "soldering_": "Item/Tool",
    "sonic_": "Effect/VFX",
    "soviet_": "Item",
    "sp_": "Item",
    "spawn_": "Encounter",
    "specimen_": "Item",
    "spectrometer_": "Item/Device",
    "spiked_": "Item/Equipment",
    "splint": "Item/Medical",
    "spore_": "Effect",
    "spring_": "Item",
    "spy": "Character/NPC",
    "squad": "Character/NPC",
    "stack": "Item",
    "staff": "Item/Weapon",
    "star_": "UI",
    "stash": "Item",
    "state": "UI",
    "stat_": "UI",
    "status": "UI/Status",
    "stereo": "UI",
    "sterile_": "Item/Medical",
    "stim_": "Item/Medical",
    "stimulant_": "Item/Medical",
    "stitches_": "Item/Medical",
    "stone_": "Item",
    "stop_": "UI",
    "sto_": "UI",
    "stranger_": "Character/NPC",
    "stratagem_": "UI",
    "strike": "UI",
    "strip": "UI",
    "strung": "Item",
    "stun_": "Item/Weapon",
    "sturdy_": "Item/Equipment",
    "style": "UI",
    "sub_": "UI",
    "submachine_": "Item/Weapon",
    "suburban": "Environment",
    "subway": "Environment",
    "sugar": "Item/Consumable",
    "sulfur_": "Item/Material",
    "sunset_": "Environment",
    "super_": "Item",
    "supply": "Item",
    "survivor_": "Character/Portrait",
    "sv_": "Character/Portrait",
    "suture": "Item/Medical",
    "swamp": "Environment",
    "switch": "Item",
    "t_": "Item",
    "tactical_": "Item/Equipment",
    "tag_": "Item",
    "tank_": "Item/Device",
    "tape_": "Item/Tool",
    "target_": "UI",
    "tattered_": "Item",
    "tech_": "Item/Device",
    "tent_": "Item/Equipment",
    "terrified_": "Character",
    "test_": "Reference",
    "thermal_": "Item/Equipment",
    "thermometer": "Item",
    "thirst": "UI/Status",
    "throw_": "Item/Weapon",
    "time_": "UI",
    "tire_": "Item/Vehicle",
    "toxic_": "Effect",
    "toy_": "Item",
    "tracked_": "Item",
    "tractor": "Location",
    "trader_": "Character/NPC",
    "trade_": "UI/Trade",
    "trait_": "Character/Trait",
    "trap_": "Item/Trap",
    "trauma_": "Effect/Status",
    "triage_": "UI/Medical",
    "trigger_": "Item",
    "trimmed_": "Item",
    "tripod_": "Item/Tool",
    "tripwire_": "Item/Trap",
    "tutorial": "UI",
    "tv_": "Item/Device",
    "tw_": "Item",
    "type_": "UI",
    "ultra_": "Item",
    "unarmed_": "Item/Weapon",
    "uncap_": "UI",
    "underground": "Location",
    "unique_": "Item",
    "unofficial_": "UI",
    "unscoped_": "Item/Weapon",
    "unused_": "Reference",
    "upgrade_": "Item/Crafting",
    "urban": "Environment",
    "us_": "Item",
    "v_": "Item",
    "vault_": "Location",
    "vehicle_": "Item/Vehicle",
    "vented_": "Item",
    "verb_": "UI",
    "vial": "Item",
    "video": "UI",
    "vintage_": "Item",
    "vinyl": "Item",
    "vitamins": "Item/Medical",
    "volunteer_": "UI",
    "vox": "Item",
    "w_": "Item",
    "warlord": "Character/NPC",
    "warmth": "UI/Status",
    "warning_": "UI",
    "wasteland": "Environment",
    "watchtower": "Location",
    "water_": "Item/Consumable",
    "weather_": "Environment/Weather",
    "weapon_": "Item/Weapon",
    "weaponry_": "Item/Weapon",
    "weird": "Effect",
    "weld": "Item/Tool",
    "wickered": "UI",
    "wild_": "Item",
    "wind": "Effect",
    "winter_": "Environment",
    "wire_": "Item/Tool",
    "withered_": "Effect",
    "wooden_": "Item",
    "work_": "Item/Tool",
    "workbench_": "Item/Crafting",
    "world_": "UI",
    "wounded": "Effect/Status",
    "wrench": "Item/Tool",
    "xenon_": "Item",
    "xray_": "Item/Device",
    "y_": "Item",
    "yellow_": "UI",
    "zone_": "UI",
}

STITCH_SCREEN_RE = re.compile(r"^\d+[a-z]+\d+\.png$", re.IGNORECASE)
ITEM_ID_RE = re.compile(r"^[a-z][a-z0-9_]*[a-z0-9]$")

CATEGORY_BY_PARENT = {
    "assets/art": "Wildcard (legacy fallback)",
    "assets/sprites/Items": "Item",
    "assets/sprites/Portraits": "Character/Portrait",
    "assets/sprites/Locations": "Location",
    "assets/sprites/Factions": "Faction",
    "assets/sprites/Weather": "Environment/Weather",
    "assets/sprites/Items/Ammo": "Item/Ammo",
    "assets/sprites/Items/Containers": "Item/Container",
    "assets/sprites/Items/Devices": "Item/Device",
    "assets/sprites/Items/Materials": "Item/Material",
    "assets/sprites/Items/Medical": "Item/Medical",
    "assets/sprites/Items/Tools": "Item/Tool",
    "assets/sprites/Items/Weapons": "Item/Weapon",
    "assets/ui/Textures": "UI/Chrome",
    "assets/ui/Textures/Backgrounds": "UI/Background",
    "assets/ui/Icons": "UI/Icon",
    "assets/ui/MainMenu": "UI/MainMenu",
    "assets/ui/Screens": "Stitch/Reference",
    "assets/ui/FactionEmblems": "Faction/Emblem",
    "assets/fonts": "Font",
}

# ── File scan ───────────────────────────────────────────────────────────
def sha256(path):
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(8192), b""):
            h.update(chunk)
    return h.hexdigest()

def natural_sort_key(name):
    return [int(t) if t.isdigit() else t.lower()
            for t in re.split(r"(\d+)", name)]

def classify_active(rel_path, fname):
    parent = rel_path
    if parent in CATEGORY_BY_PARENT:
        return CATEGORY_BY_PARENT[parent]
    stem = fname.rsplit(".", 1)[0]
    for prefix, cat in CAT_PREFIX.items():
        if stem.startswith(prefix):
            return cat
    for part in rel_path.split("/"):
        if part in CATEGORY_BY_PARENT:
            return CATEGORY_BY_PARENT[part]
    return "Unclassified"

def is_stitch_screen(fname):
    return bool(STITCH_SCREEN_RE.match(fname))

def aspect_ratio(w, h):
    if w == 0 or h == 0:
        return None
    return round(w / h, 3)

def extract_id_from_filename(fname):
    stem = fname.rsplit(".", 1)[0]
    return stem.lower()

def discover_image_metadata(p):
    try:
        if p.suffix.lower() in (".jpg", ".jpeg", ".png", ".webp"):
            from PIL import Image
            with Image.open(p) as img:
                w, h = img.size
            return w, h
    except Exception:
        pass
    return 0, 0

# ── Main scan ───────────────────────────────────────────────────────────
def scan_dir(root, legacy=False):
    out = []
    if not root.exists():
        return out
    for path in root.rglob("*"):
        if not path.is_file():
            continue
        if path.suffix.lower() not in IMAGE_EXT:
            continue
        rel = path.relative_to(root)
        cat = classify_active(str(rel.parent), path.name) if not legacy else "Legacy"
        sz = path.stat().st_size
        w, h = (0, 0)
        if not legacy:
            w, h = discover_image_metadata(path)
        out.append({
            "path": str(rel),
            "full_path": str(path),
            "filename": path.name,
            "category": cat,
            "size_bytes": sz,
            "width": w,
            "height": h,
            "aspect_ratio": aspect_ratio(w, h),
            "stem": path.stem.lower(),
            "legacy": legacy,
        })
    return out

def main():
    active = scan_dir(ACTIVE_ROOT, legacy=False)
    legacy = scan_dir(LEGACY_ROOT, legacy=True)
    all_assets = active + legacy

    # Duplicate hash index — only on active tree to avoid the legacy
    # 2200+ separate placeholders drowning the duplicate report.
    hashes = defaultdict(list)
    for a in active:
        if a["size_bytes"] == 0:
            continue
        try:
            h = sha256(Path(a["full_path"]))
        except OSError:
            continue
        hashes[h].append(a)
        a["sha256"] = h
    exact_dup_groups = [{"hash": h, "files": [x["path"] for x in group]} for h, group in hashes.items() if len(group) > 1]

    # Save the manifest
    manifest = {
        "schema_version": 1,
        "scope": {
            "active_tree_root": "assets/",
            "legacy_tree_root": "Assets/",
            "note": "Active tree is the live Godot-only architecture. Legacy tree remains Unity-driven content, inert under the Godot host until migrated.",
        },
        "totals": {
            "active": len(active),
            "legacy": len(legacy),
            "combined": len(all_assets),
            "active_duplicate_groups": len(exact_dup_groups),
        },
        "active_assets": active,
    }
    (DOCS / "visual_asset_manifest.json").write_text(json.dumps(manifest, indent=2, sort_keys=True))

    # Build audit summary
    by_cat = Counter(a["category"] for a in active)
    by_parent = Counter(str(Path(a["path"]).parent).replace("\\", "/") for a in active)
    orphan_candidates = []
    for a in active:
        if a["category"] in ("Unclassified", "Legacy") and not a["path"].startswith("assets/ui/"):
            orphan_candidates.append(a["path"])
    write_audit(by_cat, by_parent, exact_dup_groups, orphan_candidates, len(active), len(legacy), all_assets)
    write_manifest_summary(len(active), len(legacy), len(exact_dup_groups), len(orphan_candidates))

def write_audit(by_cat, by_parent, exact_dup_groups, orphan_candidates, active_n, legacy_n, all_assets):
    lines = ["# Phase 14 — Visual Asset Audit", ""]
    lines.append(f"Generated by `scripts/audit_assets.py` from the active `assets/` tree and the legacy `Assets/` tree.")
    lines.append("")
    lines.append("## Total counts")
    lines.append("")
    lines.append(f"| Tree | Files |")
    lines.append(f"|---|---|")
    lines.append(f"| Active `assets/` (Godot) | {active_n} |")
    lines.append(f"| Legacy `Assets/` (Unity) | {legacy_n} |")
    lines.append(f"| **Combined** | **{len(all_assets)}** |")
    lines.append("")
    lines.append("## Active-tree categories (top 30)")
    lines.append("")
    for k, v in sorted(by_cat.items(), key=lambda x: -x[1])[:30]:
        lines.append(f"- `{k}`: {v}")
    lines.append("")
    lines.append("## Active-tree parent directories (top 25)")
    lines.append("")
    for k, v in sorted(by_parent.items(), key=lambda x: -x[1])[:25]:
        lines.append(f"- `{k}`: {v}")
    lines.append("")
    lines.append("## Exact-duplicate groups (active tree, by SHA256)")
    lines.append("")
    lines.append(f"Identical-content groups detected: {len(exact_dup_groups)}")
    lines.append("")
    if exact_dup_groups:
        # Sort by group size — biggest duplicates first
        exact_dup_groups.sort(key=lambda g: -len(g["files"]))
        for g in exact_dup_groups[:50]:
            lines.append(f"- `{g['hash'][:12]}`: {len(g['files'])} files")
            for f in g["files"][:6]:
                lines.append(f"  - `{f}`")
            if len(g["files"]) > 6:
                lines.append(f"  - … +{len(g['files'])-6} more")
        lines.append("")
    lines.append("## Orphan candidates")
    lines.append("")
    lines.append("Files that are not yet classifiable into a category. These are NOT deleted; they are flagged for review.")
    lines.append("")
    if orphan_candidates:
        for o in orphan_candidates[:120]:
            lines.append(f"- `{o}`")
        if len(orphan_candidates) > 120:
            lines.append(f"- … +{len(orphan_candidates) - 120} more")
    else:
        lines.append("- (none)")
    lines.append("")
    (DOCS / "VISUAL_ASSET_AUDIT.md").write_text("\n".join(lines))

def write_manifest_summary(active_n, legacy_n, dup_groups, orphan_n):
    md = [
        "# Visual Asset Manifest — Summary",
        "",
        f"- Active `assets/` files: **{active_n}**",
        f"- Legacy `Assets/` files (Unity, inert under Godot host): **{legacy_n}**",
        f"- Exact-duplicate file groups found in active tree: **{dup_groups}**",
        f"- Orphan candidates (uncategorised): **{orphan_n}**",
        "",
        "Full machine-readable manifest: `visual_asset_manifest.json`",
    ]
    (DOCS / "VISUAL_ASSET_SUMMARY.md").write_text("\n".join(md))

if __name__ == "__main__":
    main()
