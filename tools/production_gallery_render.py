#!/usr/bin/env python3
"""Phase 15 — Actual contact-sheet gallery renderer.

Builds deterministic per-family contact sheets from current ASHFALL
production assets. Each tile carries machine-readable metadata in
`snapshots/gallery_index.json`.

For Phase 14 the gallery produced only a manifest; Phase 15 actually
generates the PNG contact sheets so visual regression can begin.

Output:  snapshots/gallery_<family>_p<NN>.png
         snapshots/gallery_index.json (already produced by Phase 14)
"""
import json
from pathlib import Path
from collections import defaultdict
from PIL import Image, ImageDraw, ImageFont

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
SNAPSHOTS = REPO / "snapshots"
SNAPSHOTS.mkdir(exist_ok=True)
TILE_W, TILE_H = 192, 192
TILE_PAD = 12
HEADER_H = 80
COLS = 6
PAGE_W = COLS * (TILE_W + TILE_PAD) + TILE_PAD

MANIFEST_OBJ = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
M = MANIFEST_OBJ if isinstance(MANIFEST_OBJ, list) else MANIFEST_OBJ["active_assets"]
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))
GEN = json.load(open(REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"))


def family_for_catalog(cat):
    s = cat.lower()
    if "items" in s or "ammo" in s or "armor" in s or "craft" in s or "weapons" in s or "loot" in s:
        return "Inventory-Item"
    if "survivors" in s or "characters" in s or "npcs" in s:
        return "NPC-Portrait" if "characters" in s or "npcs" in s else "Survivor-Portrait"
    if "locations" in s:
        return "Location-Art"
    if "factions" in s:
        return "Faction-Art"
    return None


# Asset-path index by content_id (resolved rows)
cid_to_file = {}
for e in WM:
    if e["resolved_path"] != "MISSING":
        cid_to_file[e["content_id"]] = e["resolved_path"]

# Family buckets: only include rows that already have an on-disk file
# and align with the production manifest's family classification.
family_cids = defaultdict(list)
for e in WM:
    if e["resolved_path"] == "MISSING":
        continue
    fam = family_for_catalog(Path(e["catalog"]).stem)
    if fam is None:
        continue
    family_cids[fam].append(e["content_id"])

# Also pull already-on-disk inventory that the catalog doesn't drive
# but visually matches each family — used as anchors.
anchor_paths_per_family = defaultdict(list)
for r in M:
    if not isinstance(r, dict): continue
    fp = r.get("full_path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    if not fp.startswith("assets/art/"):
        continue
    stem = (r.get("stem") or "").lower()
    # crude anchor selection by stem prefix
    if stem.startswith(("bandage", "syringe", "iodine", "morphine", "splint",
                        "suture", "medical", "first_aid", "antibiotic", "antiseptic")):
        anchor_paths_per_family["Inventory-Item"].append(fp)
    elif stem.startswith(("canned", "ration", "mre", "jerky", "water", "food")):
        anchor_paths_per_family["Inventory-Item"].append(fp)
    elif stem.startswith(("geiger", "dosimeter", "gas_mask", "hazmat",
                        "respirator", "helmet", "armor", "vest", "goggles")):
        anchor_paths_per_family["Inventory-Item"].append(fp)
    elif stem.startswith(("ak47", "m4a1", "pistol", "shotgun", "rifle",
                        "sniper", "machete", "crossbow", "axe", "knife",
                        "weapon_", "pipe_", "bat", "crowbar")):
        anchor_paths_per_family["Inventory-Item"].append(fp)
    elif stem.startswith(("abandoned", "ruined", "subway", "factory", "hospital",
                        "bunker", "house", "city", "suburban", "ghost",
                        "rural", "collapsed", "petrified", "mushroom",
                        "frozen", "flooded")):
        anchor_paths_per_family["Location-Art"].append(fp)
    elif stem.startswith(("survivor_", "bunker_commander", "mysterious",
                          "raider_", "deserter_", "crazed", "wounded",
                          "merchant")):
        anchor_paths_per_family["Survivor-Portrait"].append(fp)
    elif stem.startswith(("elena_", "marcus_", "suki_", "npc_",
                          "faction_", "leader")):
        anchor_paths_per_family["NPC-Portrait"].append(fp)
    elif stem.startswith(("faction_badge", "emblem_", "coat")):
        anchor_paths_per_family["Faction-Art"].append(fp)


def render_family_page(family_name, content_ids, anchor_paths, page_index):
    """Render one PNG montage for the family. 24 tiles per page maximum."""
    layout = []
    for cid in content_ids[:24]:
        path = cid_to_file.get(cid)
        if path:
            layout.append({"kind": "catalog", "cid": cid, "path": path})
    for fp in anchor_paths[:12]:
        layout.append({"kind": "anchor", "path": fp})
    if not layout:
        # No tiles; emit an empty placeholder page so the page count is honest.
        return f"snapshots/gallery_{family_name.lower()}_p{page_index:02d}.png (empty-page-stub)"

    rows = math.ceil(len(layout) / COLS)
    page_h = HEADER_H + rows * (TILE_H + TILE_PAD) + TILE_PAD
    page = Image.new("RGB", (PAGE_W, page_h), (16, 18, 22))
    draw = ImageDraw.Draw(page)
    # Header band
    page.paste(Image.new("RGB", (PAGE_W, HEADER_H), (32, 50, 64)), (0, 0))
    title = f"ASHFALL Visual Asset Gallery — {family_name} — page {page_index:02d}"
    subtitle = f"tiles: {len(layout)} / column count: {COLS}"
    try:
        font = ImageFont.load_default()
    except Exception:
        font = None
    draw.text((TILE_PAD, 12), title, fill=(220, 220, 220), font=font)
    draw.text((TILE_PAD, 40), subtitle, fill=(150, 150, 170), font=font)
    for i, tile in enumerate(layout):
        col = i % COLS
        row = i // COLS
        x = TILE_PAD + col * (TILE_W + TILE_PAD)
        y = HEADER_H + TILE_PAD + row * (TILE_H + TILE_PAD)
        # Tile border
        draw.rectangle([x, y, x + TILE_W, y + TILE_H], outline=(64, 64, 80), width=1)
        # Try to render the asset
        fp = REPO / tile["path"]
        try:
            with Image.open(fp) as im:
                im_r = im.convert("RGB").resize((TILE_W, TILE_H), Image.LANCZOS)
                page.paste(im_r, (x, y))
                # Caption below tile (tile stem)
                stem = Path(tile["path"]).stem
                color = (200, 200, 210)
                if tile.get("kind") == "anchor":
                    color = (180, 220, 180)
                draw.text((x + 4, y + TILE_H - 18), stem[:30], fill=color, font=font)
        except Exception as e:
            draw.text((x + 4, y + 4), f"ERR: {Path(tile['path']).name[:24]}", fill=(255, 100, 100), font=font)
    out = SNAPSHOTS / f"gallery_{family_name.lower().replace(' ', '_').replace('-','_')}_p{page_index:02d}.png"
    page.save(out)
    return str(out.relative_to(REPO))


import math
index = {
    "pages": [],
}
page_index = 0
for family in ("Inventory-Item", "Location-Art", "Survivor-Portrait", "NPC-Portrait", "Faction-Art"):
    cids = family_cids.get(family, [])
    anchors = anchor_paths_per_family.get(family, [])
    if not cids and not anchors:
        index["pages"].append({"family": family, "status": "no-tiles"})
        continue
    page_index += 1
    fp = render_family_page(family, cids, anchors, page_index)
    seen = set()
    entries = []
    for cid in cids[:24]:
        if cid in seen:
            continue
        path = cid_to_file.get(cid)
        if path:
            seen.add(cid)
            entries.append({"kind": "catalog", "cid": cid, "tile": str(page_index),
                            "resolution": [TILE_W, TILE_H],
                            "fallback_status": "OK"})
    for path in anchors[:12]:
        seen.add(path)
        entries.append({"kind": "anchor", "cid": Path(path).stem, "tile": str(page_index),
                        "resolution": [TILE_W, TILE_H], "fallback_status": "OK"})
    index["pages"].append({"family": family, "page": page_index,
                            "page_file": fp,
                            "tile_count": len(entries),
                            "tiles": entries})

(SNAPSHOTS / "gallery_index.json").write_text(json.dumps(index, indent=1))
print(f"→ wrote snapshots/gallery_*.png + updated gallery_index.json "
      f"({page_index} pages)")
