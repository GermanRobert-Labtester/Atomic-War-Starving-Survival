#!/usr/bin/env python3
"""Phase 13 wiring trace — mirrors the *new* AssetRegistry resolver exactly.

Implements:
  - Literal stem first
  - Semantic alias (item-only) second — same ItemIdAliases map
  - Prefix-add candidate per category third (skip if already prefixed)
  - Forward through the four category-path-roots per candidate stem
"""
import json
from pathlib import Path
from collections import Counter, defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
DATA = REPO / "Assets/StreamingAssets/Data"

# Mirror AssetRegistry.cs PrefixAddRules exactly.
PREFIX_ADD = {
    "item":     ["item_"],
    "portrait": ["survivor_", "npc_"],
    "location": ["loc_"],
    "faction":  ["faction_"],
}

ALIASES = {
    "mechanical_components": "scrap_mechanical",
    "mechanical_parts":      "scrap_mechanical",
    "scrap_mechanical":      "scrap_mechanical",
}

PATH_ROOTS = {
    "item":     ["assets/art/{0}.jpg", "assets/art/{0}.png",
                 "assets/sprites/Items/{0}.png", "assets/sprites/items/{0}.png"],
    "portrait": ["assets/art/{0}.jpg", "assets/art/{0}.png",
                 "assets/sprites/Portraits/{0}.png", "assets/sprites/portraits/{0}.png"],
    "location": ["assets/art/{0}.jpg", "assets/art/{0}.png",
                 "assets/sprites/Locations/{0}.png", "assets/sprites/locations/{0}.png"],
    "faction":  ["assets/art/{0}.jpg", "assets/art/{0}.png",
                 "assets/sprites/Factions/{0}.png", "assets/sprites/factions/{0}.png"],
}
VISUAL_KINDS = ("item", "portrait", "location", "faction")
ID_KEYS = ("id", "Id", "key", "name", "name_en", "code")


def candidates_for(id_, kind):
    """Mirror AssetRegistry.ResolveStemCandidates (POST-FIX)."""
    out = [(id_, "literal")]
    if not id_:
        return []
    out = [(id_, "literal")]
    if kind == "item" and id_ in ALIASES:
        out.append((ALIASES[id_], "semantic-alias"))
    for p in PREFIX_ADD.get(kind, ()):
        if id_.startswith(p):
            continue
        out.append((p + id_, "prefix-add"))
    return out


def resolve_for(id_, kind):
    if kind not in VISUAL_KINDS:
        return None, None, None
    for stem, origin in candidates_for(id_, kind):
        for pat in PATH_ROOTS[kind]:
            rel = pat.format(stem)
            if (REPO / rel).exists():
                return rel, origin, stem
    return None, None, None


def file_to_kind(fp: Path):
    s = fp.stem.lower()
    if s.startswith(("items", "item", "ammo", "armor", "scrap", "components",
                     "materials", "consumables", "crafting", "drugs", "trade_goods",
                     "recipe_", "recipes", "relics", "craftables", "equipment", "mods",
                     "trait", "verdict_items", "year_of_ash_items", "dose_items",
                     "foundry_items", "greenhouse_items", "black_flotilla_items",
                     "deep_lore_items", "holdfast_items", "crossing_items",
                     "crafting_material", "medical_item", "chemical_dependency",
                     "expansion_item", "economy_goods", "crossing_factions")):
        return "item"
    if s.startswith(("survivor", "npcs", "character")):
        return "portrait"
    if s.startswith(("loc", "locations", "location", "verdict_locations",
                     "year_of_ash_locations", "deep_lore_locations", "dose_locations",
                     "holdfast_locations", "crossing_locations")):
        return "location"
    if s.startswith(("faction", "factions", "holdfast_factions",
                     "standing_record_factions", "foundry_faction")):
        return "faction"
    return None


def collect():
    out = []
    for fp in sorted(DATA.glob("*.json")):
        kind = file_to_kind(fp)
        if kind is None:
            continue
        try:
            d = json.load(open(fp))
        except Exception:
            continue
        if isinstance(d, list):
            for r in d:
                if not isinstance(r, dict):
                    continue
                cid = next((r[k] for k in ID_KEYS if isinstance(r.get(k), (str, int))), None)
                if cid is not None:
                    out.append((str(fp.relative_to(REPO)), "(rec)", kind, str(cid), r))
        elif isinstance(d, dict):
            for top, arr in d.items():
                if isinstance(arr, list) and arr and isinstance(arr[0], dict):
                    if any(k in arr[0] for k in ID_KEYS):
                        for r in arr:
                            if not isinstance(r, dict):
                                continue
                            cid = next((r[k] for k in ID_KEYS if isinstance(r.get(k), (str, int))), None)
                            if cid is not None:
                                out.append((str(fp.relative_to(REPO)), top, kind, str(cid), r))
    # weapon rows use item roots
    for i, e in enumerate(out):
        if e[2] == "weapon":
            out[i] = (e[0], e[1], "item", e[3], e[4])
    seen = set()
    deduped = []
    for e in out:
        k = (e[2], e[3])
        if k in seen:
            continue
        seen.add(k)
        deduped.append(e)
    return deduped


def main():
    content = collect()
    matrix = []
    for src, top, kind, cid, rec in content:
        path, origin, stem_used = resolve_for(cid, kind)
        matrix.append({
            "catalog": src,
            "content_type": top,
            "content_id": cid,
            "kind": kind,
            "resolved_path": path or "MISSING",
            "resolution_origin": origin,
            "stem_used": stem_used,
        })

    # Aggregate
    total = len(matrix)
    missing = [m for m in matrix if m["resolved_path"] == "MISSING"]
    by_origin = Counter(m["resolution_origin"] for m in matrix if m["resolved_path"] != "MISSING")
    by_kind_total = Counter(m["kind"] for m in matrix)
    by_kind_missing = Counter(m["kind"] for m in missing)

    print(f"Total visual catalog rows (deduped): {total}")
    print(f"Resolved: {total - len(missing)}")
    print(f"Missing:  {len(missing)}")
    print(f"\nResolution origin breakdown:")
    for k, v in sorted(by_origin.items(), key=lambda x: -x[1]):
        print(f"  {k}: {v}")
    print(f"\nBy kind:")
    for k in ("item", "portrait", "location", "faction"):
        tt = by_kind_total.get(k, 0)
        mm = by_kind_missing.get(k, 0)
        print(f"  {k}: total={tt}, missing={mm} ({100*mm/tt if tt else 0:.1f}%)")
    print(f"\nMissing by source catalog:")
    by_cat = Counter(Path(m["catalog"]).stem for m in missing)
    for c, n in by_cat.most_common():
        print(f"  {c}: {n}")

    # Persistent output
    out_dir = REPO / "docs/visual"
    out_dir.mkdir(exist_ok=True)
    (out_dir / "WIRING_MATRIX.json").write_text(json.dumps(matrix, indent=1))
    print(f"\n→ wrote WIRING_MATRIX.json ({len(matrix)} rows)")

    # Write practice stats
    stats = {
        "total": total,
        "resolved": total - len(missing),
        "missing": len(missing),
        "by_origin": dict(by_origin),
        "by_kind_total": dict(by_kind_total),
        "by_kind_missing": dict(by_kind_missing),
        "missing_by_catalog": dict(by_cat),
    }
    (out_dir / "_phase13_wiring_stats.json").write_text(json.dumps(stats, indent=1))
    print(f"→ wrote _phase13_wiring_stats.json")


if __name__ == "__main__":
    main()
