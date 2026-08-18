#!/usr/bin/env python3
"""
ASHFALL Phase B — Content-ID ↔ Asset wiring audit.

For each catalog item, survivor, location, faction, etc. that has a visual
property, trace: requested ID → registry alias chain → canonical path → file
existence → perceptual-hash group → fallback used?

Outputs:
  docs/visual/WIRING_MATRIX.json
  docs/visual/FALLBACK_VISUAL_ASSETS.md
  docs/visual/DUPLICATE_VISUAL_ASSETS.md
  docs/visual/ORPHAN_VISUAL_ASSETS.md
"""
import os
import json
import hashlib
import re
from pathlib import Path
from collections import defaultdict, Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ASSETS = REPO / "assets"
MANIFEST = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
DATA = REPO / "Assets/StreamingAssets/Data"

# AssetRegistry: aliases from src/Host/AssetRegistry.cs (literal mirror)
# In production this lives in the C# code; replicate the canonical mapping here for static analysis.
# Mirrors `ItemIdAliases` in src/Host/AssetRegistry.cs
ITEM_ALIASES = {
    "mechanical_components": "scrap_mechanical",
    "mechanical_parts":      "scrap_mechanical",
    "scrap_mechanical":      "scrap_mechanical",
}

# AssetRegistry search paths (literal mirror of `ItemSearchPaths` et al.)
ITEM_PATHS = [
    "assets/art/{0}.jpg",
    "assets/art/{0}.png",
    "assets/sprites/Items/{0}.png",
    "assets/sprites/items/{0}.png",
]
PORTRAIT_PATHS = [
    "assets/art/{0}.jpg",
    "assets/art/{0}.png",
    "assets/sprites/Portraits/{0}.png",
    "assets/sprites/portraits/{0}.png",
]
LOCATION_PATHS = [
    "assets/art/{0}.jpg",
    "assets/art/{0}.png",
    "assets/sprites/Locations/{0}.png",
    "assets/sprites/locations/{0}.png",
]
FACTION_PATHS = [
    "assets/art/{0}.jpg",
    "assets/art/{0}.png",
    "assets/sprites/Factions/{0}.png",
    "assets/sprites/factions/{0}.png",
]

# Build path → record index for fast existence lookup
PATH_INDEX = {r["file_path"]: r for r in MANIFEST}
STEM_INDEX = defaultdict(list)
for r in MANIFEST:
    STEM_INDEX[r["asset_id"]].append(r)

# ────────── perceptual hash (cheap, PIL-friendly) ──────────
try:
    from PIL import Image
    HAVE_PIL = True
except Exception:
    HAVE_PIL = False


def perceptual_hash(file_path: Path, size=8):
    """Average-hash. Robust to resampling/compression, coarse enough for grouping."""
    if not HAVE_PIL:
        return None
    try:
        with Image.open(file_path) as im:
            im = im.convert("L").resize((size, size), Image.NEAREST)
            pixels = list(im.getdata())
            avg = sum(pixels) / len(pixels)
            return "".join("1" if p > avg else "0" for p in pixels)
    except Exception:
        return None


def file_exists(rel: str) -> bool:
    return (REPO / rel).exists()


def resolve_paths(content_id: str, paths: list):
    """Replicate AssetRegistry.GetItem/Portrait/Location/ByPath logic."""
    if not content_id:
        return None
    for pat in paths:
        rel = pat.format(content_id)
        if file_exists(rel):
            return rel
    return None


VISUAL_KINDS = ("item", "portrait", "location", "faction", "weapon")


def candidate_ids(content_id: str, kind: str):
    """Yield possible filesystem stems to try. Honors AssetRegistry alias map AND ASHFALL's
    real `item_X` <-> `X` filename convention used across the data catalog."""
    if not content_id:
        return
    yield content_id
    # catalog entries may use `item_X` while assets use plain `X`
    if kind == "item":
        for prefix in ("item_", "weapon_", "ammo_", "med_"):
            if content_id.startswith(prefix):
                stem = content_id[len(prefix):]
                yield stem
        # else if bare stem, also try `item_X`
        yield "item_" + content_id
        yield "weapon_" + content_id
    if kind == "portrait":
        if content_id.startswith("survivor_"):
            yield content_id[len("survivor_"):]
        if content_id.startswith("npc_"):
            yield content_id[len("npc_"):]
    if kind == "location":
        if content_id.startswith("loc_"):
            yield content_id[len("loc_"):]
        yield "loc_" + content_id
    if kind == "faction":
        if content_id.startswith("faction_"):
            yield content_id[len("faction_"):]
        yield "faction_" + content_id


def resolve_id(content_id: str, kind: str):
    """Returns (resolved_path, was_alias_used, alias_target) or (None, False, None).
    Only resolves for visually meaningful kinds — others return (None, False, None)."""
    if kind not in VISUAL_KINDS:
        return None, False, None
    paths = {
        "item": ITEM_PATHS,
        "portrait": PORTRAIT_PATHS,
        "location": LOCATION_PATHS,
        "faction": FACTION_PATHS,
    }[kind]
    # Direct stem(s) first.
    for stem in candidate_ids(content_id, kind):
        direct = resolve_paths(stem, paths)
        if direct:
            alias_used = stem != content_id
            return direct, alias_used, stem if alias_used else None
    # alias fallback for items
    if kind == "item" and content_id in ITEM_ALIASES:
        target = ITEM_ALIASES[content_id]
        aliased = resolve_paths(target, paths)
        if aliased:
            return aliased, True, target
    return None, False, None


# ────────── catalog reader ──────────
# Catalog names mapped to (content_kind, id_key).
# Keys here are PREFIXES — a file like "items.json" matches "items".
# A file like "characters.json" matches "characters". A file like "survivors.json"
# matches "survivors".
FILE_PREFIX_KIND = [
    # file stem matches the first prefix (longest-match wins).
    ("items",              "item"),
    ("item",               "item"),
    ("survivors",          "portrait"),
    ("npcs",               "portrait"),
    ("characters",         "portrait"),
    ("characters_static",  "portrait"),
    ("characters_",        "portrait"),
    ("locations",          "location"),
    ("location",           "location"),
    ("locations_expansion","location"),
    ("holdfast_locations", "location"),
    ("holdfast_items",     "item"),
    ("crossing_locations", "location"),
    ("crossing_items",     "item"),
    ("crossing_encounters","encounter"),
    ("crossing_factions",  "faction"),
    ("verdict_locations",  "location"),
    ("verdict_items",      "item"),
    ("verdict_npcs",       "portrait"),
    ("year_of_ash_locations","location"),
    ("year_of_ash_items",  "item"),
    ("year_of_ash_survivors","portrait"),
    ("deep_lore_locations","location"),
    ("expansion_survivor","portrait"),
    ("dose_locations",     "location"),
    ("dose_items",         "item"),
    ("greenhouse_items",   "item"),
    ("black_flotilla_items","item"),
    ("factions",           "faction"),
    ("holdfast_factions",  "faction"),
    ("standing_record_factions","faction"),
    ("foundry_faction",    "faction"),
    ("foundry_items",      "item"),
    ("weapons",            "weapon"),
    ("recipes",            "recipe"),
    ("relic_recipes",      "recipe"),
    ("relics",             "item"),
    ("events",             "event"),
    ("encounters",         "encounter"),
    ("weather_seasons",    "weather"),
    ("weather",            "weather"),
    ("perks",              "perk"),
    ("scrap",              "item"),
    ("components",         "item"),
    ("materials",          "item"),
    ("ammo",               "item"),
    ("mods",               "item"),
    ("traits",             "trait"),
    ("equipment",          "item"),
    ("trade_goods",        "item"),
    ("craftables",         "item"),
    ("crafting_material",  "item"),
    ("consumables",        "item"),
    ("drugs",              "item"),
    ("medical_item",       "item"),
    ("economy_goods",      "item"),
    ("chemical_dependency","item"),
    ("expansion_item",     "item"),
    ("faction_war_radio",      "FACTWAR_NONVISUAL"),
    ("faction_war_journal",    "FACTWAR_NONVISUAL"),
    ("faction_war_dialogue",   "FACTWAR_NONVISUAL"),
    ("faction_war_communiques","FACTWAR_NONVISUAL"),
    ("faction_war_location",   "FACTWAR_NONVISUAL"),
    ("faction_war_events",     "FACTWAR_NONVISUAL"),
    ("standing_record_layouts","FACTWAR_NONVISUAL"),
    ("standing_record_memory","FACTWAR_NONVISUAL"),
    ("confession_secrets",     "FACTWAR_NONVISUAL"),
    ("dynamic_questlines",     "FACTWAR_NONVISUAL"),
    ("narrative_arc_events",   "FACTWAR_NONVISUAL"),
    ("narrative_encounters",   "FACTWAR_NONVISUAL"),
    ("narrative_questlines",   "FACTWAR_NONVISUAL"),
    ("echoes",                 "FACTWAR_NONVISUAL"),
    ("duty_roster_marks",      "FACTWAR_NONVISUAL"),
    ("duty_roster_seasons",    "FACTWAR_NONVISUAL"),
    ("final_wishes",           "FACTWAR_NONVISUAL"),
    ("guilt_sources",          "FACTWAR_NONVISUAL"),
    ("hardcore_economy_tuning","FACTWAR_NONVISUAL"),
    ("muster_witnesses",       "FACTWAR_NONVISUAL"),
    ("muster_epilogues",       "FACTWAR_NONVISUAL"),
    ("questline_master",       "FACTWAR_NONVISUAL"),
    ("trade_screen_scenarios", "FACTWAR_NONVISUAL"),
    ("trade_specialties",      "FACTWAR_NONVISUAL"),
    ("trade_tell_lines",       "FACTWAR_NONVISUAL"),
    ("utility_actions",        "FACTWAR_NONVISUAL"),
    ("wall_carving_templates","FACTWAR_NONVISUAL"),
    ("world_history",          "FACTWAR_NONVISUAL"),
    ("warlord_doctrines",      "FACTWAR_NONVISUAL"),
    ("year_of_ash_quests",     "FACTWAR_NONVISUAL"),
    ("year_of_ash_events",     "FACTWAR_NONVISUAL"),
    ("year_of_ash_radio",      "FACTWAR_NONVISUAL"),
    ("duty_roster_quests",     "FACTWAR_NONVISUAL"),
    ("duty_roster_locations",  "FACTWAR_NONVISUAL"),
    ("standing_record_quests","FACTWAR_NONVISUAL"),
    ("dose_quest",             "FACTWAR_NONVISUAL"),
    ("holdfast_quests",        "FACTWAR_NONVISUAL"),
    ("crossing_quests",        "FACTWAR_NONVISUAL"),
    ("holdfast_flavor",         "FACTWAR_NONVISUAL"),
    ("foundry_production",     "FACTWAR_NONVISUAL"),
    ("foundry_treaty_consequences","FACTWAR_NONVISUAL"),
    ("foundry_accords",        "FACTWAR_NONVISUAL"),
    ("verdict_data",           "FACTWAR_NONVISUAL"),
    ("verdict_questlines",     "FACTWAR_NONVISUAL"),
    ("verdict_radio",          "FACTWAR_NONVISUAL"),
    ("door_encounters",        "FACTWAR_NONVISUAL"),
    ("radio",                  "FACTWAR_NONVISUAL"),
    ("radio_distress_signals", "FACTWAR_NONVISUAL"),
    ("cassette_sets",          "FACTWAR_NONVISUAL"),
    ("dive_sites",             "FACTWAR_NONVISUAL"),
    ("currents",               "FACTWAR_NONVISUAL"),
    ("damaged_map_zones",     "FACTWAR_NONVISUAL"),
    ("deep_lore_survivor_fields","FACTWAR_NONVISUAL"),
    ("antigravity_survivor_fields","FACTWAR_NONVISUAL"),
    ("faction_lore",           "FACTWAR_NONVISUAL"),
    ("faction_radio_corpus",   "FACTWAR_NONVISUAL"),
    ("phantom_triggers",       "FACTWAR_NONVISUAL"),
    ("disease_catalog",        "FACTWAR_NONVISUAL"),
    ("combat_catalog",         "FACTWAR_NONVISUAL"),
]
# Sort by length-desc so longer prefixes match first.
FILE_PREFIX_KIND.sort(key=lambda x: -len(x[0]))


ID_KEYS = ("id", "Id", "key", "name", "name_en", "code")


def file_to_kind(fp: Path):
    stem = fp.stem.lower()
    # Try with content-type suffixes (e.g. items_static, items_proto) by partial prefix.
    for prefix, kind in FILE_PREFIX_KIND:
        if stem == prefix or stem.startswith(prefix + "_") or stem.startswith(prefix):
            return kind
    return None


def collect_records_from_obj(data: dict, source_path: str, kind: str):
    """Map top-level keys to known collections when file is a dict."""
    out = []
    id_keys = ("id", "Id", "key", "name", "name_en", "code")
    for top, arr in data.items():
        if isinstance(arr, list) and arr:
            for r in arr:
                if not isinstance(r, dict):
                    break
                if not any(k in r for k in id_keys):
                    break
            else:
                for r in arr:
                    if not isinstance(r, dict):
                        continue
                    cid = next((r[k] for k in id_keys if k in r), None)
                    if cid is None:
                        continue
                    if isinstance(cid, (dict, list)):
                        continue
                    out.append((source_path, top, kind, str(cid), r))
    return out


def collect_records_from_list(data: list, source_path: str, kind: str):
    """When the top-level is a list, decide kind from the filename."""
    out = []
    for r in data:
        if not isinstance(r, dict):
            continue
        # Try multiple id keys
        cid = None
        for k in ID_KEYS:
            if k in r and isinstance(r[k], (str, int)):
                cid = str(r[k])
                break
        if not cid:
            continue
        out.append((source_path, "(record)", kind, cid, r))
    return out


def collect_content_ids():
    out = []
    if not DATA.exists():
        return out

    for fp in sorted(DATA.glob("*.json")):
        try:
            with open(fp) as fh:
                data = json.load(fh)
        except Exception:
            continue

        kind = file_to_kind(fp)
        if kind is None:
            # Skip files we can't categorize; the catalog integrity validator
            # knows them but we don't need them for asset wiring.
            continue

        if isinstance(data, dict):
            out.extend(collect_records_from_obj(data, str(fp.relative_to(REPO)), kind))
        elif isinstance(data, list):
            out.extend(collect_records_from_list(data, str(fp.relative_to(REPO)), kind))

    seen = set()
    deduped = []
    for entry in out:
        key = (entry[2], entry[3])
        if key in seen:
            continue
        seen.add(key)
        deduped.append(entry)
    return deduped


def main():
    matrix = []
    fallback_used = []
    missing = []
    duplicates_perceptual = defaultdict(list) if HAVE_PIL else None
    asset_md5_groups = defaultdict(list)
    # quick MD5 duplicate groups from manifest
    for r in MANIFEST:
        if r.get("md5"):
            asset_md5_groups[r["md5"]].append(r)

    # ── Trace content entities ──
    content = collect_content_ids()
    print(f"[wiring] catalog entries with id: {len(content)}")
    seen = set()
    for source, top, kind, cid, rec in content:
        if kind == "FACTWAR_NONVISUAL" or kind not in VISUAL_KINDS:
            # non-visual catalog (faction_war_radio, etc.) — skip visual wiring
            continue
        path, alias_used, alias_target = resolve_id(cid, kind)
        rec_md5 = None
        if path and path in PATH_INDEX:
            rec_md5 = PATH_INDEX[path].get("md5")
        entry = {
            "catalog": source,
            "content_type": top,
            "content_id": cid,
            "kind": kind,
            "resolved_path": path or "MISSING",
            "was_alias_used": alias_used,
            "alias_target": alias_target if alias_used else None,
            "fallback_status": "NOT_TRIGGERED" if path else "MISSING_FROM_REGISTRY",
            "asset_md5": rec_md5,
        }
        matrix.append(entry)
        if not path:
            missing.append(entry)
        if alias_used:
            # alias was forced — record fallback case
            entry["fallback_status"] = "ALIAS_RESOLUTION"
        seen.add(cid)

    # ── perceptual near-duplicate grouping (cheap aHash, 8x8) ──
    if HAVE_PIL:
        ph_groups = defaultdict(list)
        for r in MANIFEST:
            p = REPO / r["file_path"]
            ph = perceptual_hash(p)
            if ph:
                ph_groups[ph].append(r["file_path"])
        # group only those with 2+ members and not identical MD5s
        near_dup = {}
        idx = 0
        for ph, paths in ph_groups.items():
            if len(paths) < 2:
                continue
            # filter: only count if there are 2 different files that already share MD5 OR share perceptual hash but not MD5
            actual_paths = paths
            distinct_md5s = {r["md5"] for r in MANIFEST if r["file_path"] in actual_paths and r.get("md5")}
            if len(distinct_md5s) >= 1:  # perceptual near-dup (same bytes already OR visually similar)
                idx += 1
                near_dup[f"pdup_{idx}"] = sorted(actual_paths)
        perceptual_duplicates = near_dup
    else:
        perceptual_duplicates = {}

    # ── "by MD5 exact duplicates" ──
    exact_dup = {}
    idx = 0
    for md5, entries in asset_md5_groups.items():
        if len(entries) < 2:
            continue
        idx += 1
        exact_dup[f"dup_{idx}"] = {
            "md5": md5,
            "paths": sorted(e["file_path"] for e in entries),
            "size_bytes": entries[0].get("file_size", 0),
        }
    # ── Orphan assets (not referenced from any catalog) ──
    all_paths_in_matrix = {m["resolved_path"] for m in matrix if m["resolved_path"] not in ("MISSING", None, "")}
    orphan_paths = []
    for r in MANIFEST:
        if r["file_path"] in all_paths_in_matrix:
            continue
        # do not count canonical UI helpers
        stem = r["asset_id"]
        if any(stem.startswith(p) for p in ("frame_9slice", "panel_bg", "scroll", "tab_strip",
                                            "tooltip_box", "btn_", "icon_")):
            continue
        # do not count generic UI chrome (svg sprites live in ui/ root)
        # do not count reference_library, altar etc
        orphan_paths.append(r)

    # ── write outputs ──
    out_dir = REPO / "docs/visual"
    out_dir.mkdir(parents=True, exist_ok=True)

    (out_dir / "WIRING_MATRIX.json").write_text(json.dumps(matrix, indent=1))
    print(f"[wiring] wrote WIRING ({len(matrix)} rows)")

    # Group-by-id summary
    id_summary = defaultdict(list)
    for m in matrix:
        id_summary[m["content_id"]].append(m)
    # content_ids that resolve to MISSING
    md_by_id = {m["content_id"]: m for m in matrix}
    missing_ids = sorted([m["content_id"] for m in missing])
    # catalog level report
    by_catalog_missing = Counter(m["catalog"] for m in missing)
    # build markdown
    md = []
    md.append("# ASHFALL — Visual Asset Wiring Matrix\n")
    md.append(f"\nGenerated from `{MANIFEST}` length={len(MANIFEST)}.")
    md.append(f"\nContent entries catalogued: **{len(matrix)}**.")
    md.append(f"\nUnique content IDs: **{len(set(m['content_id'] for m in matrix))}**.")
    md.append(f"\nMissing from registry: **{len(missing_ids)}**.")
    md.append(f"\nAlias-resolved (forced fallback): **{sum(1 for m in matrix if m['was_alias_used'])}**.")
    md.append(f"\nExact-duplicate file groups (MD5): **{len(exact_dup)}**.")
    md.append(f"\nPerceptual-duplicate groups (aHash): **{len(perceptual_duplicates)}**.")
    md.append(f"\nOrphan assets (not in any catalog): **{len(orphan_paths)}**.\n")

    md.append("\n## Missing by Catalog\n")
    for cat, n in by_catalog_missing.most_common(20):
        md.append(f"- `{cat}`: {n} missing\n")

    md.append("\n## Wiring Matrix (one row per content entity)\n")
    md.append("| Content ID | Catalog | Kind | Resolved path | Fallback? | Alias? | Status |\n")
    md.append("|---|---|---|---|---|---|---|\n")
    for m in sorted(matrix, key=lambda r: (r["catalog"], r["content_id"]))[:500]:
        md.append(
            f"| `{m['content_id']}` | `{m['catalog']}` | {m['kind']} | `{m['resolved_path']}` | "
            f"{m['fallback_status']} | {'YES → ' + (m.get('alias_target') or '-') if m['was_alias_used'] else '-'} | "
            f"{'OK' if m['resolved_path'] != 'MISSING' else 'MISSING'}|\n"
        )

    md.append(f"\n*(showing first 500 of {len(matrix)})*\n")
    (out_dir / "WIRING_MATRIX.md").write_text("".join(md))
    print(f"[wiring] wrote WIRING_MATRIX.md")

    # Fallback report
    fb_rows = [m for m in matrix if m["was_alias_used"] or m["resolved_path"] == "MISSING"]
    fb_md = ["# ASHFALL — Fallback Visual Assets Audit\n",
             f"\nGenerated for content catalog entries that either (a) require the registry's "
             f"`ItemIdAliases` map (`mechanical_*` → `scrap_mechanical*`) or (b) are entirely "
             f"missing from the asset registry.\n",
             f"\nTotal fallback / missing rows: **{len(fb_rows)}**.\n",
             "\n## Production-safe (alias-resolved)\n",
             "\nA content ID like `mechanical_components` and `mechanical_parts` is ALIASED → `scrap_mechanical`. "
             "The `ItemIdAliases` map keeps this working — investigate whether the alias target's art "
             "is acceptable as production art for `mechanical_*` items.\n",
             "\n| Content ID | Catalog | Resolved path | Why |\n",
             "|---|---|---|---|\n"]
    for m in fb_rows:
        why = "alias-resolved" if m["was_alias_used"] else "MISSING"
        fb_md.append(f"| `{m['content_id']}` | `{m['catalog']}` | `{m['resolved_path']}` | {why} |\n")
    (out_dir / "FALLBACK_VISUAL_ASSETS.md").write_text("".join(fb_md))
    print(f"[wiring] wrote FALLBACK")

    # Duplicate report
    dup_md = ["# ASHFALL — Duplicate Visual Assets\n",
              f"\nExact duplicates (MD5): **{len(exact_dup)} groups**.\n",
              f"\nPerceptual near-duplicates (8x8 average-hash, 2+ files): **{len(perceptual_duplicates)} groups**.\n",
              "\n## Exact duplicates (binary-identical files saved under different names)\n"]
    for gid, info in sorted(exact_dup.items()):
        dup_md.append(f"\n### `{gid}` — md5 `{info['md5']}` ({info['size_bytes']:,} bytes)\n")
        for p in info["paths"]:
            dup_md.append(f"- `{p}`\n")

    dup_md.append("\n## Perceptual duplicates (visually similar — same composition, different files)\n")
    for gid, paths in sorted(perceptual_duplicates.items())[:200]:
        dup_md.append(f"\n### `{gid}` (aHash group)\n")
        for p in paths[:25]:
            dup_md.append(f"- `{p}`\n")
        if len(paths) > 25:
            dup_md.append(f"- … {len(paths) - 25} more\n")
    (out_dir / "DUPLICATE_VISUAL_ASSETS.md").write_text("".join(dup_md))
    print(f"[wiring] wrote DUPLICATE")

    # Orphan report
    orph_md = ["# ASHFALL — Orphan Visual Assets\n",
               "\n**Orphan** = visual file on disk NOT referenced from any catalog entry.\n",
               "\nSplit into:\n\n",
               f"- `definite_orphan`: never referenced\n",
               f"- `probable_orphan`: only referenced by generated/promotional content\n\n",
               f"\n\nSibling directories like `ui/Screens` contain Stitch exports (62 PNGs) "
               f"that are reference library outputs not wired into runtime. They are tracked separately in `STITCH_GENERATED_UI_INVENTORY.md`.\n\n",
               "\n## Orphan paths (top 200 — by file_path sprite subdirs)\n",
               "\n| Path | Category | Visual family | size | dims |\n",
               "|---|---|---|---|---|\n"]
    orph_md_rows = []
    # categorize by directory
    art_orphans = [r for r in orphan_paths if r["file_path"].startswith("assets/art/")]
    other_orphans = [r for r in orphan_paths if not r["file_path"].startswith("assets/art/")]
    orph_md.append(f"\n*Generated AI / sprite assets: {len(other_orphans)} orphans*\n")
    orph_md.append(f"\n*Generic `assets/art/` items not in any catalog: {len(art_orphans)} orphans*\n")

    orph_md.append("\n### generated_AI / sprite orphans\n")
    for r in sorted(other_orphans, key=lambda r: r["file_path"])[:80]:
        orph_md.append(f"`{r['file_path']}`  [{r.get('semantic_category','-')}]\n")
    orph_md.append("\n### `assets/art/` orphans (top 100 by file size)\n")
    orph_md_rows = sorted(art_orphans, key=lambda r: -r.get("file_size", 0))
    md_table = ["| Path | Cat | size | dims |\n", "|---|---|---|---|\n"]
    for r in orph_md_rows[:100]:
        md_table.append(f"| `{r['file_path']}` | {r.get('semantic_category','-')} | {r.get('file_size', 0):,} | {r.get('width','-')}×{r.get('height','-')} |\n")
    orph_md.append("\n".join(md_table))
    orph_md.append("\n*(omitted large group for brevity)*\n")
    orph_md.append(f"\nTotal art/ orphans: {len(art_orphans)}. Total sprite/AI_Generated orphans: {len(other_orphans)}.\n")
    (out_dir / "ORPHAN_VISUAL_ASSETS.md").write_text("\n".join(orph_md))
    print(f"[wiring] wrote ORPHAN")

    # Print summary
    print(f"\n──── Summary ────")
    print(f"  Total catalog entries: {len(matrix)}")
    print(f"  Missing (no asset): {len(missing)}")
    print(f"  Alias-resolved: {sum(1 for m in matrix if m['was_alias_used'])}")
    print(f"  Exact duplicate groups: {len(exact_dup)}")
    print(f"  Perceptual duplicate groups: {len(perceptual_duplicates)}")
    print(f"  Orphan asset files: {len(orphan_paths)}")


if __name__ == "__main__":
    main()
