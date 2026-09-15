#!/usr/bin/env python3
"""Generate deterministic 512x512 Pillow placeholder assets for missing catalog art.

Every output is explicitly a placeholder:
  assets/art/placeholders-512/placeholder_<category>_<id>.png  (512x512 RGBA)
  assets/art/placeholders-512/PLACEHOLDER_512_MANIFEST.json
  docs/visual/PLACEHOLDER_512_INVENTORY.md (written by --write-inventory)

Coverage source of truth mirrors the runtime resolvers without a Godot run:
  - AssetRegistry search roots: assets/art/{id}.jpg|.png,
    assets/sprites/Items|Portraits|Locations|Factions/{id}.png
  - ItemIdAliases semantic fallbacks + category prefix-add
    (item_: item_, portrait: survivor_/npc_, location: loc_, faction: faction_)
  - FactionIconCatalog explicit UI-emblem mapping
    (assets/ui/Icons/faction_icon_*.png, verified on disk)

Style follows the ASHFALL visual target: charcoal/concrete ground, restrained
amber + teal accents, programmatic line pictograms, PLACEHOLDER stamped in
pixels AND in the manifest. No AI, no network, deterministic seed per id.

Usage:
    python3 scripts/tools/generate-placeholder-512-pack.py [--check] [--write-inventory]
    python3 scripts/tools/generate-placeholder-512-pack.py --categories item,location --limit 5
    python3 scripts/tools/generate-placeholder-512-pack.py --check
"""

from __future__ import annotations

import argparse
import glob
import json
import os
import random
import re
import zlib
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont

REPO_ROOT = Path(__file__).resolve().parents[2]
DATA_DIR = REPO_ROOT / "Assets" / "StreamingAssets" / "Data"
OUT_DIR = REPO_ROOT / "assets" / "art" / "placeholders-512"
MANIFEST_PATH = OUT_DIR / "PLACEHOLDER_512_MANIFEST.json"
INVENTORY_PATH = REPO_ROOT / "docs" / "visual" / "PLACEHOLDER_512_INVENTORY.md"
FONT_DIR = REPO_ROOT / "assets" / "fonts"

SIZE = 512
SEED_BASE = 20260913

# Coverage manifest mirrors AssetCoverageScanner.CoverageCatalogFiles.
COVERAGE_CATALOG_FILES: list[tuple[str, str, str]] = [
    ("item", "items.json", "id"),
    ("item", "black_flotilla_items.json", "id"),
    ("item", "chemical_dependency_items.json", "id"),
    ("item", "crossing_items.json", "id"),
    ("item", "dose_items.json", "id"),
    ("item", "foundry_items.json", "id"),
    ("item", "greenhouse_items.json", "id"),
    ("item", "holdfast_items.json", "id"),
    ("item", "verdict_items.json", "id"),
    ("item", "year_of_ash_items.json", "id"),
    ("portrait", "survivors.json", "id"),
    ("portrait", "year_of_ash_survivors.json", "id"),
    ("portrait", "characters.json", "id"),
    ("portrait", "verdict_npcs.json", "id"),
    ("location", "locations.json", "id"),
    ("location", "crossing_locations.json", "id"),
    ("location", "deep_lore_locations.json", "id"),
    ("location", "dose_locations.json", "id"),
    ("location", "duty_roster_locations.json", "id"),
    ("location", "holdfast_locations.json", "id"),
    ("location", "locations_expansion3.json", "id"),
    ("location", "verdict_locations.json", "id"),
    ("location", "year_of_ash_locations.json", "id"),
    ("faction", "currents.json", "id"),
    ("faction", "crossing_factions.json", "id"),
    ("faction", "holdfast_factions.json", "id"),
    ("faction", "standing_record_factions.json", "id"),
    ("faction", "foundry_faction.json", "faction_id"),
    ("faction", "faction_lore.json", "faction_id"),
]

# Mirrors AssetRegistry.ItemIdAliases (semantic fallbacks, item-only).
ITEM_ALIASES: dict[str, str] = {
    "mechanical_components": "scrap_mechanical",
    "mechanical_parts": "scrap_mechanical",
    "scrap_mechanical": "scrap_mechanical",
    "item_decon_chelator_concentrate": "item_prussian_blue_chelating_pellets",
    "item_lead_lined_effluent_filter": "filter_cartridge",
    "item_heavy_neoprene_scrub_brush": "toothbrush",
    "item_sealed_waste_bin": "bunker_grid_container",
    "item_theodolite_brass_precision": "relic_antique_brass_telescope",
    "item_surveyor_stadia_rod": "sensor_laser_rangefinder",
    "item_datum_plate_bronze": "item_lead_plate",
    "item_concrete_mix": "concrete_mix",
    "item_forged_rotor_shaft": "bearing_set_industrial",
    "item_magnetic_bearing_coil": "antenna_coil",
    "item_high_vacuum_pump": "pump",
    "item_containment_ring_steel": "shelter_wall_steel_plate",
    "item_reinforced_concrete_vault": "shelter_storage_crate_large",
    "item_seismic_damper_pad": "shelter_wall_steel_plate",
    "item_vacuum_pump_oil": "machine_oil",
    "item_bearing_grease": "item_uv_grease",
}

PREFIX_ADD: dict[str, list[str]] = {
    "item": ["item_"],
    "portrait": ["survivor_", "npc_"],
    "location": ["loc_"],
    "faction": ["faction_"],
}

# ASHFALL restrained palette (matches shelter/visual-pack generators).
CONCRETE = (28, 32, 38, 255)
CONCRETE_DARK = (20, 23, 27, 255)
CONCRETE_LIGHT = (52, 58, 66, 255)
OFFWHITE = (206, 210, 214, 255)
STEEL = (118, 128, 138, 255)
AMBER = (229, 183, 84, 255)
TEAL = (96, 158, 156, 255)
RUST = (150, 92, 58, 255)
BONE = (176, 172, 158, 255)
BRASS = (195, 157, 91, 255)

CATEGORY_STYLE: dict[str, dict] = {
    "item": {"accent": STEEL, "glyph": "crate", "label": "ITEM"},
    "portrait": {"accent": TEAL, "glyph": "bust", "label": "PORTRAIT"},
    "location": {"accent": AMBER, "glyph": "shelter", "label": "LOCATION"},
    "faction": {"accent": BRASS, "glyph": "shield", "label": "FACTION"},
}


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont | ImageFont.ImageFont:
    candidates = (
        ["BarlowCondensed-Bold.ttf", "BarlowCondensed-SemiBold.ttf"] if bold
        else ["BarlowCondensed-Regular.ttf", "ShareTechMono-Regular.ttf"]
    )
    for name in candidates:
        path = FONT_DIR / name
        try:
            return ImageFont.truetype(str(path), size)
        except OSError:
            continue
    return ImageFont.load_default()


def extract_ids(path: Path, field: str) -> list[str]:
    try:
        text = path.read_text(encoding="utf-8")
    except OSError:
        return []
    pattern = f'"{field}":'
    ids: list[str] = []
    pos = 0
    guard = 0
    while True:
        guard += 1
        if guard > len(text) * 2 + 100:
            break
        idx = text.find(pattern, pos)
        if idx < 0:
            break
        pos = idx + len(pattern)
        while pos < len(text) and text[pos] in " \t\n\r":
            pos += 1
        if pos >= len(text) or text[pos] != '"':
            continue
        pos += 1
        start = pos
        while pos < len(text) and text[pos] != '"':
            if text[pos] == "\\":
                pos += 1
            pos += 1
        iid = text[start:pos]
        if iid and iid not in ids:
            ids.append(iid)
        pos += 1
    return ids


def load_faction_mapping() -> dict[str, str]:
    """Parse FactionIconCatalog.cs for id -> relative-path pairs."""
    src = REPO_ROOT / "Assets" / "Ashfall.Core" / "UI" / "FactionIconCatalog.cs"
    try:
        text = src.read_text(encoding="utf-8")
    except OSError:
        return {}
    pairs = re.findall(r'\{\s*"([^"]+)"\s*,\s*"([^"]+)"\s*\}', text)
    return {k: v for k, v in pairs}


def collect_art_stems() -> set[str]:
    stems: set[str] = set()
    out_resolved = OUT_DIR.resolve()
    for pattern in ("assets/art/*",):
        for p in glob.glob(str(REPO_ROOT / pattern)):
            # Placeholders must never count as coverage: they are explicitly
            # not final art, so the output dir and placeholder_* stems are
            # excluded even if a future glob turns recursive.
            if p.endswith(".import") or os.path.isdir(p):
                continue
            try:
                if os.path.commonpath([os.path.abspath(p), str(out_resolved)]) == str(out_resolved):
                    continue
            except ValueError:
                pass
            stem = os.path.splitext(os.path.basename(p))[0]
            if stem.startswith("placeholder_"):
                continue
            stems.add(stem)
    for sub in ("Items", "Portraits", "Locations", "Factions",
                "items", "portraits", "locations", "factions"):
        d = REPO_ROOT / "assets" / "sprites" / sub
        if not d.is_dir():
            continue
        for p in glob.glob(str(d / "*.png")) + glob.glob(str(d / "**/*.png"), recursive=True):
            if p.endswith(".import"):
                continue
            stems.add(os.path.splitext(os.path.basename(p))[0])
    return stems


def is_covered(category: str, cid: str, stems: set[str],
               faction_map: dict[str, str]) -> bool:
    if cid in stems:
        return True
    if category == "item" and cid in ITEM_ALIASES and ITEM_ALIASES[cid] in stems:
        return True
    for prefix in PREFIX_ADD.get(category, []):
        if not cid.startswith(prefix) and (prefix + cid) in stems:
            return True
    if category == "faction":
        rel = faction_map.get(cid)
        if rel and (REPO_ROOT / rel).is_file():
            return True
    return False


def inventory_missing() -> dict[str, list[str]]:
    stems = collect_art_stems()
    faction_map = load_faction_mapping()
    missing: dict[str, list[str]] = {"item": [], "portrait": [], "location": [], "faction": []}
    seen: dict[str, set[str]] = {k: set() for k in missing}
    for category, filename, field in COVERAGE_CATALOG_FILES:
        ids = extract_ids(DATA_DIR / filename, field)
        for cid in ids:
            if not cid or cid in seen[category]:
                continue
            seen[category].add(cid)
            if not is_covered(category, cid, stems, faction_map):
                missing[category].append(cid)
    for key in missing:
        missing[key].sort()
    return missing


def placeholder_filename(category: str, cid: str) -> str:
    safe = re.sub(r"[^a-z0-9_]+", "_", cid.strip().lower()).strip("_") or "unknown"
    return f"placeholder_{category}_{safe}.png"


def draw_glyph(draw: ImageDraw.ImageDraw, glyph: str, cx: int, cy: int,
               s: int, col: tuple) -> None:
    lw = max(6, s // 40)
    if glyph == "crate":
        draw.rectangle([cx - s // 2, cy - s // 3, cx + s // 2, cy + s // 3],
                       outline=col, width=lw)
        draw.line([cx - s // 2, cy - s // 3, cx + s // 2, cy + s // 3], fill=col, width=lw)
        draw.line([cx + s // 2, cy - s // 3, cx - s // 2, cy + s // 3], fill=col, width=lw)
        draw.rectangle([cx - s // 4, cy + s // 3 + 20, cx + s // 4, cy + s // 3 + 60],
                       outline=col, width=lw)
    elif glyph == "bust":
        draw.ellipse([cx - s // 4, cy - s // 2, cx + s // 4, cy - s // 8], outline=col, width=lw)
        draw.arc([cx - s // 2, cy - s // 8, cx + s // 2, cy + s // 2], 200, 340, fill=col, width=lw)
        draw.line([cx - s // 2, cy + s // 2, cx + s // 2, cy + s // 2], fill=col, width=lw)
        draw.rectangle([cx - s // 6, cy - s // 4, cx + s // 6, cy - s // 6], fill=col)
    elif glyph == "shelter":
        draw.polygon([(cx, cy - s // 2), (cx + s // 2, cy - s // 8),
                      (cx + s // 2, cy + s // 3), (cx - s // 2, cy + s // 3),
                      (cx - s // 2, cy - s // 8)], outline=col, width=lw)
        draw.rectangle([cx - s // 10, cy, cx + s // 10, cy + s // 3], outline=col, width=lw)
        draw.line([cx - s // 2, cy + s // 3 + 30, cx + s // 2, cy + s // 3 + 30],
                  fill=col, width=lw)
    elif glyph == "shield":
        draw.polygon([(cx, cy - s // 2), (cx + s // 3, cy - s // 4),
                      (cx + s // 4, cy + s // 4), (cx, cy + s // 2),
                      (cx - s // 4, cy + s // 4), (cx - s // 3, cy - s // 4)],
                     outline=col, width=lw)
        draw.ellipse([cx - s // 12, cy - s // 12, cx + s // 12, cy + s // 12],
                     outline=col, width=lw)


def render_placeholder(category: str, cid: str) -> Image.Image:
    style = CATEGORY_STYLE[category]
    accent = style["accent"]
    seed = SEED_BASE + (zlib.crc32(f"{category}:{cid}".encode("utf-8")) & 0xFFFFFFFF)
    rnd = random.Random(seed)

    img = Image.new("RGBA", (SIZE, SIZE), CONCRETE)
    draw = ImageDraw.Draw(img, "RGBA")

    # Subtle backdrop variation: deterministic soft blotches.
    for _ in range(18):
        r = rnd.randrange(30, 110)
        x = rnd.randrange(r, SIZE - r)
        y = rnd.randrange(r, SIZE - r)
        alpha = rnd.randrange(8, 26)
        tone = rnd.choice([CONCRETE_LIGHT, CONCRETE_DARK])
        draw.ellipse([x - r, y - r, x + r, y + r], fill=(*tone[:3], alpha))

    # Horizon line + faint grid so large flat areas read as concrete at 512.
    draw.line([0, SIZE * 3 // 4, SIZE, SIZE * 3 // 4], fill=(*CONCRETE_LIGHT[:3], 90), width=2)
    for gx in range(0, SIZE + 1, 64):
        draw.line([gx, 0, gx, SIZE], fill=(0, 0, 0, 28), width=1)
    for gy in range(0, SIZE + 1, 64):
        draw.line([0, gy, SIZE, gy], fill=(0, 0, 0, 28), width=1)

    # Outer frame + corner ticks.
    draw.rounded_rectangle([7, 7, SIZE - 8, SIZE - 8], radius=14,
                           outline=CONCRETE_LIGHT, width=3)
    draw.rounded_rectangle([15, 15, SIZE - 16, SIZE - 16], radius=9,
                           outline=accent, width=3)
    tick = 32
    for x0, y0, dx, dy in ((15, 15, 1, 1), (SIZE - 16, 15, -1, 1),
                           (15, SIZE - 16, 1, -1), (SIZE - 16, SIZE - 16, -1, -1)):
        draw.line([x0, y0, x0 + dx * tick, y0], fill=AMBER, width=5)
        draw.line([x0, y0, x0, y0 + dy * tick], fill=AMBER, width=5)

    # Top banner.
    draw.rectangle([15, 15, SIZE - 16, 78], fill=(0, 0, 0, 110))
    draw.text((SIZE // 2, 40), "PLACEHOLDER", font=font(36, bold=True),
              fill=AMBER, anchor="mm")
    draw.text((SIZE // 2, 64),
              f"{style['label']}  •  512x512  •  NOT FINAL ART",
              font=font(15, bold=True), fill=OFFWHITE, anchor="mm")

    # Center pictogram in an accent ring.
    cx, cy, ring = SIZE // 2, SIZE // 2 - 10, 125
    draw.ellipse([cx - ring, cy - ring, cx + ring, cy + ring],
                 outline=accent, width=5)
    draw.ellipse([cx - ring + 11, cy - ring + 11, cx + ring - 11, cy + ring - 11],
                 outline=(*accent[:3], 90), width=2)
    draw_glyph(draw, style["glyph"], cx, cy, 150, accent)

    # Catalog id block (bottom).
    draw.rectangle([15, SIZE - 152, SIZE - 16, SIZE - 16], fill=(0, 0, 0, 130))
    draw.line([15, SIZE - 152, SIZE - 16, SIZE - 152], fill=accent, width=3)
    label = cid.upper()
    if len(label) > 34:
        label = label[:33] + "…"
    draw.text((SIZE // 2, SIZE - 116), label, font=font(26, bold=True),
              fill=OFFWHITE, anchor="mm")
    draw.text((SIZE // 2, SIZE - 86), f"placeholder_{category}_{cid}.png",
              font=font(13), fill=STEEL, anchor="mm")
    draw.text((SIZE // 2, SIZE - 60),
              "Pillow procedural placeholder — replace with final art",
              font=font(13), fill=BONE, anchor="mm")
    draw.text((SIZE // 2, SIZE - 38),
              "scripts/tools/generate-placeholder-512-pack.py",
              font=font(11), fill=(*STEEL[:3], 220), anchor="mm")

    # Deterministic grain so flat 512 areas do not band.
    grain_layer = Image.new("RGBA", (SIZE, SIZE), (0, 0, 0, 0))
    gd = ImageDraw.Draw(grain_layer)
    for _ in range(1600):
        x = rnd.randrange(17, SIZE - 17)
        y = rnd.randrange(17, SIZE - 17)
        tone = 255 if rnd.random() > 0.5 else 0
        gd.point((x, y), fill=(tone, tone, tone, rnd.randrange(6, 26)))
    img.alpha_composite(grain_layer)
    return img


def assert_no_filename_collisions(selected: dict[str, list[str]]) -> None:
    """Fail loud if two catalog ids normalize to one placeholder filename.

    Without this, build() would silently let the second render overwrite the
    first while the manifest claims both files exist."""
    owners: dict[str, tuple[str, str]] = {}
    for category in ("item", "portrait", "location", "faction"):
        for cid in selected.get(category, []):
            fname = placeholder_filename(category, cid)
            if fname in owners and owners[fname] != (category, cid):
                other = owners[fname]
                raise SystemExit(
                    f"FILENAME COLLISION: {other} and {(category, cid)} "
                    f"both normalize to {fname}; rename one catalog id")
            owners[fname] = (category, cid)


def build(selected: dict[str, list[str]], *, prune_stale: bool = False,
          manifest_path: Path = MANIFEST_PATH) -> list[dict]:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    entries: list[dict] = []
    total = sum(len(v) for v in selected.values())
    done = 0
    for category in ("item", "portrait", "location", "faction"):
        for cid in selected.get(category, []):
            fname = placeholder_filename(category, cid)
            render_placeholder(category, cid).save(OUT_DIR / fname, format="PNG", optimize=True)
            entries.append({
                "catalog_id": cid,
                "category": category,
                "file": fname,
                "dimensions": [SIZE, SIZE],
                "placeholder": True,
                "label_in_image": "PLACEHOLDER",
                "runtime_path": f"res://assets/art/placeholders-512/{fname}",
                "source": "scripts/tools/generate-placeholder-512-pack.py",
                "replace_with_final_art": True,
            })
            done += 1
            if done % 100 == 0 or done == total:
                print(f"  [{done}/{total}] {fname}", flush=True)
    manifest = {
        "schema_version": 1,
        "collection": "ashfall_placeholder_512_pack",
        "status": "PLACEHOLDER — NOT FINAL ART",
        "scope": "512x512 Pillow placeholders for catalog ids with no authored art",
        "generator": "scripts/tools/generate-placeholder-512-pack.py",
        "generated_by": "Pillow, deterministic programmatic line art",
        "seed_base": SEED_BASE,
        "dimensions": [SIZE, SIZE],
        "license": "MIT",
        "replace_with_final_art": True,
        "godot_import": "Run Godot import to refresh .import sidecars; do not hand-edit them",
        "coverage_roots": [
            "assets/art/{id}.jpg",
            "assets/art/{id}.png",
            "assets/sprites/Items/{id}.png",
            "assets/sprites/Portraits/{id}.png",
            "assets/sprites/Locations/{id}.png",
            "assets/sprites/Factions/{id}.png",
            "FactionIconCatalog explicit UI emblems (faction)",
        ],
        "counts": {k: len(v) for k, v in selected.items()},
        "files": entries,
    }
    manifest_path.write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    if prune_stale:
        # Full runs own the directory: remove stale placeholder_*.png files
        # (e.g. catalog ids retired since the last run) so cruft cannot linger.
        expected = {e["file"] for e in entries}
        pruned = 0
        for stale in sorted(OUT_DIR.glob("placeholder_*.png")):
            if stale.name not in expected:
                stale.unlink()
                pruned += 1
        if pruned:
            print(f"  pruned {pruned} stale placeholder file(s)")
    return entries


def write_inventory(missing: dict[str, list[str]], generated: int) -> None:
    INVENTORY_PATH.parent.mkdir(parents=True, exist_ok=True)
    lines = [
        "# ASHFALL Placeholder 512 Inventory",
        "",
        "All files in `assets/art/placeholders-512/` are **PLACEHOLDERS — NOT FINAL ART**.",
        "Every PNG is 512x512, Pillow-generated, deterministic, and stamped `PLACEHOLDER`",
        "in pixels and in `PLACEHOLDER_512_MANIFEST.json`. Replace with final art; never",
        "ship placeholders as finished visuals.",
        "",
        f"Generator: `scripts/tools/generate-placeholder-512-pack.py` (seed base {SEED_BASE}).",
        f"Generated files this run: {generated}.",
        "",
        "## Coverage roots (mirrors runtime resolvers)",
        "",
        "- `assets/art/{{id}}.jpg` / `{{id}}.png`",
        "- `assets/sprites/Items|Portraits|Locations|Factions/{{id}}.png`",
        "- `ItemIdAliases` semantic fallbacks + category `prefix-add` normalization",
        "  (`item_` / `survivor_`+`npc_` / `loc_` / `faction_`)",
        "- `FactionIconCatalog` explicit `assets/ui/Icons/faction_icon_*.png` emblems",
        "",
        "## Counts",
        "",
        "| Category | Missing ids | Placeholder files |",
        "|---|---|---|",
    ]
    for cat in ("item", "portrait", "location", "faction"):
        lines.append(f"| {cat} | {len(missing.get(cat, []))} | {len(missing.get(cat, []))} |")
    lines += ["", "## Missing catalog ids", ""]
    for cat in ("item", "portrait", "location", "faction"):
        ids = missing.get(cat, [])
        lines.append(f"### {cat} ({len(ids)})")
        lines.append("")
        if not ids:
            lines.append("_none — full coverage_")
            lines.append("")
            continue
        # Keep the doc reviewable: full id list, wrapped as code spans.
        for i in range(0, len(ids), 8):
            lines.append(", ".join(f"`{c}`" for c in ids[i:i + 8]))
        lines.append("")
    lines += [
        "## Naming",
        "",
        "- Every generated file is named `placeholder_<category>_<catalog_id>.png`.",
        "- No generated file impersonates a final-art stem (`{id}.jpg/png`).",
        "- Manifest maps each `catalog_id` to its placeholder `runtime_path`.",
        "",
        "## Regenerate",
        "",
        "```bash",
        "python3 scripts/tools/generate-placeholder-512-pack.py --write-inventory",
        "python3 scripts/tools/generate-placeholder-512-pack.py --check",
        "```",
        "",
    ]
    INVENTORY_PATH.write_text("\n".join(lines), encoding="utf-8")


def check() -> int:
    failures: list[str] = []
    if not MANIFEST_PATH.is_file():
        print(f"MISSING: {MANIFEST_PATH}")
        return 1
    try:
        manifest = json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
    except Exception as exc:
        print(f"MANIFEST READ FAIL: {exc}")
        return 1
    files = manifest.get("files", [])
    if manifest.get("dimensions") != [SIZE, SIZE]:
        failures.append(f"manifest dimensions != [{SIZE}, {SIZE}]")
    if manifest.get("status") != "PLACEHOLDER — NOT FINAL ART":
        failures.append("manifest status is not the placeholder sentinel")
    for entry in files:
        fname = entry.get("file", "")
        if not fname.startswith("placeholder_"):
            failures.append(f"{fname}: name does not carry placeholder_ prefix")
        path = OUT_DIR / fname
        if not path.is_file():
            failures.append(f"missing {fname}")
            continue
        try:
            with Image.open(path) as im:
                if list(im.size) != [SIZE, SIZE]:
                    failures.append(f"{fname}: expected [{SIZE}, {SIZE}], found {list(im.size)}")
                im.verify()
        except Exception as exc:
            failures.append(f"{fname}: {exc}")
        if entry.get("placeholder") is not True or entry.get("label_in_image") != "PLACEHOLDER":
            failures.append(f"{fname}: manifest entry not marked placeholder")
    # Manifest must match current missing-id inventory (no drift, no extras).
    missing = inventory_missing()
    expected = {placeholder_filename(c, i) for c, ids in missing.items() for i in ids}
    actual = {e.get("file", "") for e in files}
    if expected != actual:
        only_manifest = sorted(actual - expected)[:10]
        only_missing = sorted(expected - actual)[:10]
        failures.append(
            f"manifest/inventory drift: {len(actual)} files vs {len(expected)} missing ids "
            f"(only-manifest={only_manifest}, only-missing={only_missing})"
        )
    # Stale placeholder_*.png files with no manifest entry are leftover state
    # (retired ids, interrupted runs). build(prune_stale=True) removes them.
    on_disk = {p.name for p in OUT_DIR.glob("placeholder_*.png")}
    orphans = sorted(on_disk - actual)[:10]
    if orphans:
        failures.append(
            f"{len(on_disk - actual)} orphan placeholder file(s) on disk "
            f"(e.g. {orphans}); re-run the generator without --limit/--categories"
        )
    if failures:
        print("PLACEHOLDER_512_CHECK FAIL")
        for failure in failures[:40]:
            print(f"- {failure}")
        if len(failures) > 40:
            print(f"... and {len(failures) - 40} more")
        return 1
    print(f"PLACEHOLDER_512_CHECK PASS ({len(files)} files, {SIZE}x{SIZE}, seed {SEED_BASE})")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true",
                        help="validate the existing pack without writing files")
    parser.add_argument("--write-inventory", action="store_true",
                        help="write docs/visual/PLACEHOLDER_512_INVENTORY.md after building")
    parser.add_argument("--categories", default="",
                        help="comma-separated subset: item,portrait,location,faction")
    parser.add_argument("--limit", type=int, default=0,
                        help="max files per category (smoke test only)")
    args = parser.parse_args()

    if args.check:
        return check()

    missing = inventory_missing()
    subset = bool(args.categories) or (args.limit and args.limit > 0)
    if subset:
        # Subset runs are smoke tests: they must not clobber the real manifest
        # or inventory with a partial set. Guard the shared outputs.
        if args.categories:
            wanted = {c.strip() for c in args.categories.split(",") if c.strip()}
            missing = {k: v for k, v in missing.items() if k in wanted}
        if args.limit and args.limit > 0:
            missing = {k: v[:args.limit] for k, v in missing.items()}
    assert_no_filename_collisions(missing)

    total = sum(len(v) for v in missing.values())
    print(f"missing ids: {total} "
          f"(item={len(missing.get('item', []))}, "
          f"portrait={len(missing.get('portrait', []))}, "
          f"location={len(missing.get('location', []))}, "
          f"faction={len(missing.get('faction', []))})")
    entries = build(missing, prune_stale=not subset,
                    manifest_path=(OUT_DIR / "PLACEHOLDER_512_MANIFEST.partial.json"
                                     if subset else MANIFEST_PATH))
    print(f"wrote {len(entries)} placeholder files ({SIZE}x{SIZE}) to "
          f"{OUT_DIR.relative_to(REPO_ROOT)}")
    if subset:
        print("  subset run: real manifest + inventory left untouched "
              "(partial manifest at PLACEHOLDER_512_MANIFEST.partial.json; "
              "re-run without --limit/--categories to refresh the real one)")
        if args.write_inventory:
            print("  --write-inventory ignored on subset runs")
        return 0
    print("  PLACEHOLDER_512_MANIFEST.json")
    if args.write_inventory:
        write_inventory(missing, len(entries))
        print(f"  {INVENTORY_PATH.relative_to(REPO_ROOT)}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
