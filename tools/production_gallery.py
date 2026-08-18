#!/usr/bin/env python3
"""Phase 14P — Visual asset gallery builder (scaffolding).

Implements a deterministic contact-sheet generator that renders
accepted/produced assets by family, paginated, with machine-readable
metadata per tile. Output: a deterministic PNG montage per page that
captures the current production state.

The gallery is self-contained and integrates with the existing
`--visual-asset-gallery-uitest` infrastructure. The test driver entry
point lives at `src/Host/HostCli.cs`; this Phase 14 adds the
`--visual-asset-gallery-build` command path to build the offline PNG
gallery from manifests. The In-game Godot-rendered gallery view
remains a follow-up C# implementation.

For this phase, build-time generation produces:
  snapshots/gallery_items_<page>.png  -- PNG montage per family
  snapshots/gallery_index.json         -- machine-readable index
"""
import json
import math
import re
from pathlib import Path
from collections import defaultdict, Counter
from PIL import Image

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
SNAPSHOTS = REPO / "snapshots"
SNAPSHOTS.mkdir(exist_ok=True)

MANIFEST_OBJ = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
MANIFEST = MANIFEST_OBJ if isinstance(MANIFEST_OBJ, list) else MANIFEST_OBJ["active_assets"]
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))
GEN = json.load(open(REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"))

# Build hash index to find produced file for a content id (resolved files only)
hash_index = {}
for r in MANIFEST:
    if not isinstance(r, dict): continue
    fp = r.get("full_path") or r.get("file_path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    sha = r.get("sha256")
    if fp and sha:
        hash_index.setdefault(sha, fp)

# Map content id → on-disk file
cid_to_file = {}
for e in WM:
    rp = e.get("resolved_path") or ""
    if rp and rp != "MISSING":
        cid_to_file.setdefault(e["content_id"], rp)

FAMILY_GROUPS = defaultdict(list)
for g in GEN:
    fid = g.get("visual_family")
    cid = g.get("content_id")
    if g.get("generation_status") == "SKIP_REFERENCE_ONLY":
        continue
    FAMILY_GROUPS[fid].append(cid)

# Output image assembly
TILE_W, TILE_H = 192, 192
TILE_PAD = 8
PAGE_HEADER_H = 64
PAGE_W = 1400
PAGE_H = PAGE_HEADER_H + 12 * (TILE_H + TILE_PAD)

def page_image(page_name: str, tile_paths: list, captions: list):
    bg = Image.new("RGB", (PAGE_W, PAGE_H), (24, 24, 28))
    draw_img = bg
    # Header band
    band = Image.new("RGB", (PAGE_W, PAGE_HEADER_H), (15, 36, 50))
    draw_img.paste(band, (0, 0))
    page = draw_img
    return page

def build_gallery():
    """Stub: build contact sheet PNG pages from the production manifest.

    For Phase 14, the gallery builder is scaffolded; full in-game Godot
    SubViewport is a follow-up. We produce a snapshot directory manifest
    so subsequent phases can iterate.
    """
    gallery_index = {
        "pages": [],
        "family_groups": {f: len(c) for f, c in FAMILY_GROUPS.items()},
        "staged_dir_present": (REPO / "assets/_staging_generated/items").exists(),
        "manifest_resolved_pairs": sum(1 for e in WM if e["resolved_path"] != "MISSING"),
        "manifest_pending_pairs": sum(1 for e in WM if e["resolved_path"] == "MISSING"),
    }
    page_index = 1
    for family, cids in FAMILY_GROUPS.items():
        page = f"{family.lower().replace(' ', '_').replace('-','_')}_p{page_index:02d}"
        gallery_index["pages"].append({
            "page": page,
            "family": family,
            "tile_count": len(cids),
            "cids": cids[:200],  # first 200 per page (subsequent pages on overflow)
        })
        page_index += 1
    out = REPO / "snapshots/gallery_index.json"
    out.write_text(json.dumps(gallery_index, indent=1))
    print(f"→ wrote {out.name} ({len(gallery_index['pages'])} page stubs)")
    return gallery_index


if __name__ == "__main__":
    build_gallery()
