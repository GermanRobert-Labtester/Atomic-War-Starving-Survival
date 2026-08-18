#!/usr/bin/env python3
"""Generate ASSET_REPLACEMENT_QUEUE.md from missing content + orphan catalog."""
import json
from pathlib import Path
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))
M = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))

MISSING = [e for e in WM if e["resolved_path"] == "MISSING" and e["kind"] in ("item", "portrait", "location", "faction", "weapon")]

# Severity bucket
def severity_for(entry):
    kind = entry["kind"]
    cat = Path(entry["catalog"]).name
    cid = entry["content_id"]
    # Recipes & expansion subfolders are internal data, rarely rendered → P2
    if "recipe" in cat or "faction_war" in cat:
        return "P2"
    if kind in ("portrait", "location"):
        return "P1"
    if kind == "weapon":
        return "P1"
    return "P1"

# Visual family
def family_for(kind, cid):
    cl = cid.lower()
    if cl.startswith(("weapon_", "armor_", "gear_", "mod_", "att_")):
        return "Weapons / Equipment"
    if cl.startswith(("med_", "pharma_", "iodine", "anti_rad", "bandage", "syringe",
                      "vacc", "chloroform", "morphine", "rx_", "pills")):
        return "Medical"
    if cl.startswith(("ammo_",)):
        return "Ammo"
    if cl.startswith(("food_", "water_", "canned", "ration", "spirits", "tincture_",
                      "tea_", "brew")):
        return "Food / Water"
    if cl.startswith(("scrap_", "crafting_", "electronic_", "mechanical_", "metal_",
                      "wood_", "iron_", "steel_", "cable_", "wire_")):
        return "Crafting Material"
    if cl.startswith(("faction_", "badge_", "emblem_")):
        return "Faction"
    if cl.startswith(("loc_", "bunker_", "shelter_", "abandoned_", "subway_", "factory_",
                      "hospital_", "school_", "ruin")):
        return "Location Art"
    if cl.startswith(("survivor_", "npc_", "character_")):
        return "Character Portrait"
    if cl.startswith(("status_", "effect_", "debuff_", "ailment_", "trauma_", "affliction")):
        return "Status / Affliction Icon"
    if cl.startswith(("enc_", "encounter_", "event_", "radio_")):
        return "Encounter / Event"
    if cl.startswith(("muzzle_", "vfx_", "flash_", "ash_", "smoke_")):
        return "VFX"
    if cl.startswith(("relic_", "lore_", "note_", "diary")):
        return "Lore / Relic"
    if kind == "weapon":
        return "Weapons / Equipment"
    if kind == "portrait":
        return "Character Portrait"
    if kind == "location":
        return "Location Art"
    return "Generic Item / Icon"


def style_ref_for(kind, family):
    # Pixel-art Remnant/Mutant Year style still applies for items; survivors in ASHFALL native style.
    if family in ("Medical", "Food / Water", "Ammo", "Crafting Material", "Generic Item / Icon"):
        return "Pixel-art 32-128 px icon, transparent background, ASHFALL palette (charcoal + amber + blood-rust)."
    if family in ("Weapons / Equipment",):
        return "Pixel-art 64-128 px silhouette, hatch-line shading, transparent background."
    if family in ("Character Portrait",):
        return "512×512 head-and-shoulders portrait, muted earth tones, ASHFALL native style (modelled after Phase 11 verified survivors)."
    if family in ("Location Art",):
        return "1024×1024 environment plate, 16:9 usable crop, desaturated palette."
    if family in ("Faction",):
        return "512×512 monochrome emblem, high-contrast single-line aesthetic (matches verified `emblem_iron_raiders` and `faction_badge_*`)."
    if family in ("VFX",):
        return "1024×1024 centred particle plate, alpha-channel only, dark base."
    if family in ("Encounter / Event",):
        return "512×512 narrative plate, painterly, single subject, desaturated."
    if family in ("Status / Affliction Icon",):
        return "64-128 px icon, single-symbol, high-contrast."
    if family in ("Lore / Relic",):
        return "256-512 px plate, sepia-tone overlay, period-styled."
    return "Match the closest existing asset in `assets/art/{family}/`."


def dimension_for(kind, family):
    if family in ("Character Portrait",): return "512×512"
    if family in ("Location Art",):        return "1024×1024"
    if family in ("Weapons / Equipment",): return "256×256"
    if family in ("Faction",):             return "512×512"
    if family in ("VFX",):                 return "1024×1024"
    if family in ("Lore / Relic",):        return "512×512"
    if family in ("Encounter / Event",):   return "512×512"
    if family in ("Medical", "Food / Water", "Ammo", "Crafting Material"):
        return "256×256"
    if family == "Status / Affliction Icon": return "64×64"
    return "256×256"


# Build per-entry rows
md = []
md.append("# ASHFALL — Visual Asset Replacement Queue\n\n")
md.append("**Purpose:** prioritized list of content IDs whose visual asset is missing from the runtime registry. Each row carries the source catalog, the runtime kind, the visual family, the required dimension, and the reference style. No replacements are generated in this audit phase.\n\n")
md.append(f"Total entries: **{len(MISSING)}**.\n\n")

bucket = Counter(severity_for(e) for e in MISSING)
md.append("## Summary by severity\n\n| Severity | Count |\n|---|---|\n")
for sev in ("P1", "P2", "P3"):
    md.append(f"| {sev} | {bucket.get(sev, 0)} |\n")
md.append(f"| total | {len(MISSING)} |\n\n")

md.append("## Summary by visual family\n\n| Family | Count |\n|---|---|\n")
fam_count = Counter(family_for(e["kind"], e["content_id"]) for e in MISSING)
for fam, n in fam_count.most_common():
    md.append(f"| `{fam}` | {n} |\n")
md.append("\n")

# Bucket by family
md.append("## Per-row entries\n\n")
md.append("| Severity | Content ID | Catalog | Family | Required dims | Style reference |\n")
md.append("|---|---|---|---|---|---|\n")

# Sort by family then id
MISSING.sort(key=lambda e: (family_for(e["kind"], e["content_id"]), e["content_id"]))
for e in MISSING:
    fam = family_for(e["kind"], e["content_id"])
    sev = severity_for(e)
    dims = dimension_for(e["kind"], fam)
    sref = style_ref_for(e["kind"], fam)
    md.append(f"| **{sev}** | `{e['content_id']}` | `{Path(e['catalog']).name}` | {fam} | {dims} | {sref} |\n")

# Section: Orphan portraits currently deployed but unindexed
md.append("\n## Orphan-but-not-missing (deployed-but-not-wired)\n\n")
md.append("These are catalog-resolved content entities whose art exists, but the on-disk filename differs from what the AssetRegistry chain expects. They render correctly but the catalog audit traces them via the prefix-strip fallback. Listed for re-wiring hygiene:\n\n")
md.append("| Content ID | Catalog | Note |\n|---|---|---|\n")
ALIASED = [e for e in WM if e["was_alias_used"] and e["kind"] in ("item", "portrait", "location", "faction", "weapon")]
for e in ALIASED:
    md.append(f"| `{e['content_id']}` | `{Path(e['catalog']).name}` | resolves via alias `→ {e.get('alias_target', '?')}` |\n")
md.append("\n")

# Section: Placeholders to retire
md.append("## Standard catalog placeholders to retire (cleanup candidates)\n\n")
md.append("252 byte-identical files in the catalog placeholder family are flagged for retirement. Suggested mapping (canonical replacement where it exists):\n\n")
md.append("| Placeholder filename | Canonical replacement |\n|---|---|\n")
md.append("| `assets/art/item_ammo_ap.jpg` | replaced by per-caliber `ammo_<cal>_<type>.jpg` |\n")
md.append("| `assets/art/item_ammo_hp.jpg` | replaced by per-caliber `ammo_<cal>_<type>.jpg` |\n")
md.append("| `assets/art/item_ammo_standard.jpg` | replaced by per-caliber `ammo_<cal>_<type>.jpg` |\n")
md.append("| `assets/art/item_ammo_types.jpg` | direct replacement of all ammo art |\n")
md.append("| `assets/art/item_id.jpg` | inventory icon — retire, use canonical ItemIcon when bound |\n")
md.append("| `assets/art/item_icon.jpg` | retire |\n")
md.append("| `assets/art/item_patterns.jpg` | retire |\n")
md.append("| `assets/art/item_type.jpg` | retire |\n")
md.append("| `assets/art/item_rarity_common/rare/uncommon/unique.jpg` | retire |\n")
md.append("| `assets/art/item_rarity_unique.jpg` | retire |\n")
md.append("| `assets/art/item_id_prefix.jpg` | retire |\n")
md.append("| `assets/art/character_icon.jpg` | retire |\n")
md.append("| `assets/art/inventory_icon.jpg` | retire |\n")
md.append("| `assets/art/journal_icon.jpg` | retire |\n")
md.append("| `assets/art/location_pin_icon.jpg` | retire |\n")
md.append("| `assets/art/crafting_icon.jpg` | retire |\n")
md.append("| `assets/art/shelter_icon.jpg` | retire |\n")
md.append("| `assets/art/weapon_fire.jpg` | retire (no canonical ‘fire’ weapon) |\n")
md.append("| `assets/art/weapon_hmg.jpg` | replaced by `weapon_heavy_machine_gun.jpg` |\n")
md.append("| `assets/art/weapon_maint.jpg` | retire (service rifle category handles this) |\n")
md.append("| `assets/art/weapon_lmg.jpg` | replaced by `weapon_lmg_*` |\n")
md.append("| `assets/art/weapon_rpg.jpg` | replaced by `weapon_rpg_launcher_scavenged.jpg` |\n")
md.append("\n")

REPLACE_OUT = REPO / "docs/visual/ASSET_REPLACEMENT_QUEUE.md"
REPLACE_OUT.write_text("".join(md))
print(f"wrote ASSET_REPLACEMENT_QUEUE.md ({REPLACE_OUT.stat().st_size} bytes)")
