#!/usr/bin/env python3
"""Re-trace catalog→asset wiring using a faithful mirror of
src/Host/AssetRegistry.cs's actual resolve logic (no prefix stripping)."""
import json
from pathlib import Path
from collections import Counter, defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ASSETS = REPO / "assets"
DATA = REPO / "Assets/StreamingAssets/Data"

# ───── literal mirror of AssetRegistry.cs search-path constants ─────
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
# ───── literal mirror of ItemIdAliases ─────
ITEM_ALIASES = {
    "mechanical_components": "scrap_mechanical",
    "mechanical_parts":      "scrap_mechanical",
    "scrap_mechanical":      "scrap_mechanical",
}

VISUAL_KINDS = ("item", "portrait", "location", "faction", "weapon")
ID_KEYS = ("id", "Id", "key", "name", "name_en", "code")


def file_to_kind(fp: Path):
    s = fp.stem.lower()
    if s.startswith(("items", "item_", "item", "ammo", "armor", "scrap", "components",
                     "materials", "consumables", "crafting", "drugs", "trade_goods",
                     "recipe", "relics", "craftables", "equipment", "mods", "trait",
                     "verdict_items", "year_of_ash_items", "dose_items", "foundry_items",
                     "greenhouse_items", "black_flotilla_items", "deep_lore_items",
                     "holdfast_items", "crossing_items", "crafting_material",
                     "medical_item", "chemical_dependency", "expansion_item",
                     "economy_goods")):
        return "item"
    if s.startswith(("survivor", "npcs", "characters", "character")):
        return "portrait"
    if s.startswith(("loc", "location", "locations", "verdict_locations",
                     "year_of_ash_locations", "deep_lore_locations", "dose_locations",
                     "holdfast_locations", "crossing_locations")):
        return "location"
    if s.startswith(("faction", "factions", "holdfast_factions",
                     "standing_record_factions", "foundry_faction",
                     "crossing_factions")):
        return "faction"
    if s.startswith(("weapon", "weapons")):
        return "weapon"
    return None  # non-visual or untracked


def resolve_paths(content_id, paths):
    for pat in paths:
        rel = pat.format(content_id)
        if (REPO / rel).exists():
            return rel
    return None


def resolve_id(content_id, kind):
    if kind not in VISUAL_KINDS:
        return None, False, None
    paths = {
        "item": ITEM_PATHS,
        "portrait": PORTRAIT_PATHS,
        "location": LOCATION_PATHS,
        "faction": FACTION_PATHS,
    }[kind]
    direct = resolve_paths(content_id, paths)
    if direct:
        return direct, False, None
    # alias fallback for items (literal AssetRegistry.cs behaviour)
    if kind == "item" and content_id in ITEM_ALIASES:
        target = ITEM_ALIASES[content_id]
        aliased = resolve_paths(target, paths)
        if aliased:
            return aliased, True, target
    return None, False, None


def collect_content_ids():
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
                if isinstance(r, dict):
                    cid = next((r[k] for k in ID_KEYS if isinstance(r.get(k), (str, int))), None)
                    if cid is not None and isinstance(cid, (str, int)):
                        out.append((str(fp.relative_to(REPO)), "(record)", kind, str(cid), r))
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
    # dedup
    seen = set()
    deduped = []
    for e in out:
        k = (e[2], e[3])
        if k in seen:
            continue
        seen.add(k)
        deduped.append(e)
    return deduped


# ──── Detect genuine "bare-stem-X exists but catalog is item_X" or vice-versa ────
# These are the true alias candidates — adds only where strictly needed.

ALIAS_PREFIXES = {
    "item":      ("item_", "item_X"),   # we will examine whether catalog-prefixed or bare-stem
    "weapon":    ("weapon_",),
    "ammo":      ("ammo_",),
    "portrait":  ("survivor_", "npc_"),
    "location":  ("loc_", "location_"),
    "faction":   ("faction_",),
}


def main():
    content = collect_content_ids()
    matrix = []
    missing = []
    resolved = []
    alias_resolved = 0
    # For missings analyse: which way does the asset filename lean?
    bare_to_assets = defaultdict(set)
    asset_to_bare = {}
    for fp in (REPO / "assets/art").iterdir():
        if fp.is_file() and fp.suffix.lower() in ('.jpg', '.png'):
            asset_to_bare[fp.stem] = fp.stem
    for fd in [ASSETS / "sprites/Items", ASSETS / "sprites/Portraits",
               ASSETS / "sprites/Locations", ASSETS / "sprites/Factions"]:
        if fd.exists():
            for fp in fd.rglob("*"):
                if fp.is_file() and fp.suffix.lower() in ('.jpg', '.png'):
                    asset_to_bare[fp.stem] = fp.stem

    for source, top, kind, cid, rec in content:
        path, alias, alias_target = resolve_id(cid, kind)
        rec_md5 = None
        if path:
            resolved.append(cid)
        if alias:
            alias_resolved += 1
        if not path:
            missing.append((source, cid, kind))
        matrix.append({
            "catalog": source,
            "content_type": top,
            "content_id": cid,
            "kind": kind,
            "resolved_path": path or "MISSING",
            "was_alias_used": alias,
            "alias_target": alias_target if alias else None,
        })

    print(f"Catalog-driven visual rows (deduped): {len(matrix)}")
    print(f"Resolved (direct + alias): {len(resolved)}")
    print(f"  - via alias: {alias_resolved}")
    print(f"Missing (no asset at any resolved path): {len(missing)}")
    print()
    print(f"By kind:")
    by_kind = Counter(e["kind"] for e in matrix)
    for k, n in by_kind.most_common():
        miss = sum(1 for s, c, kn in missing if kn == k)
        print(f"  {k}: total={n}, missing={miss}")
    print()
    # Missing-by-catalog
    by_cat = Counter(Path(s).stem for s, _, _ in missing)
    print("Missing by source catalog:")
    for c, n in by_cat.most_common():
        print(f"  {c}: {n}")
    # Sample of 60 missing
    print()
    print("Sample of missing catalog entries (first 60):")
    for s, cid, kn in missing[:60]:
        print(f"  [{kn}] {cid}  (from {Path(s).stem})")

    # ── Detect the FAILURE MODE of each missing row ──
    print()
    print("Failure-mode classification of MISSING rows:")
    # For each missing, decide:
    #   (a) `cid` already exists in art under bare stem — but the asset filename doesn't have the `item_`
    #       prefix, so it's not normally found. Direction: catalog says X (bare), asset is X.jpg — already
    #       resolvable; we re-check; if the bare cid is non-PREFIXED and the asset has the prefix, we
    #       need an alias.
    #   (b) Catalog says item_X, asset says X.jpg → resolver doesn't see item_X format → needs prefix-strip.
    #   (c) Both sides missing.
    # ── Strategy: try the OPPOSITE of the current recipe and see if it resolves by direct path.
    directions = Counter()
    overdiagnosed = []
    for source, cid, kind in missing:
        cid_l = cid.lower()
        # Try opposite direction alternative paths
        if kind == "item":
            for prefix in ("item_", "weapon_", "ammo_", "med_", "scrap_", "gear_", "armor_"):
                if cid.lower().startswith(prefix):
                    bare = cid[len(prefix):]
                    if bare in {s.lower() for s in asset_to_bare}:
                        # catalog is prefixed, asset is bare → this needs prefix-strip
                        directions["CATALOG_ITEM_→_BARE_ASSET"] += 1
                        break
            else:
                # bare alias check
                bare_variants = []
                for prefix in ("item_", "weapon_", "ammo_", "med_", "scrap_", "gear_", "armor_"):
                    if (prefix + cid.lower()) in {s.lower() for s in asset_to_bare}:
                        bare_variants.append(prefix + cid)
                if bare_variants:
                    directions["CATALOG_BARE_→_PREFIXED_ASSET"] += 1
                else:
                    directions["NEITHER"] += 1
        elif kind in ("portrait", "location", "faction"):
            for prefix in ALIAS_PREFIXES.get(kind, ()):
                if cid.lower().startswith(prefix):
                    bare = cid[len(prefix):]
                    if bare.lower() in {s.lower() for s in asset_to_bare}:
                        directions[f"{kind.upper()}:CATALOG_PREFIXED_→_BARE_ASSET"] += 1
                        break
            else:
                for prefix in ALIAS_PREFIXES.get(kind, ()):
                    if (prefix + cid.lower()) in {s.lower() for s in asset_to_bare}:
                        directions[f"{kind.upper()}:CATALOG_BARE_→_PREFIXED_ASSET"] += 1
                        break
                else:
                    directions["NEITHER"] += 1
        else:
            directions["OTHER"] += 1
    for k, n in directions.most_common():
        print(f"  {k}: {n}")

    # show 5 examples of each directional bucket
    print()
    bucket_samples = defaultdict(list)
    for source, cid, kind in missing:
        cid_l = cid.lower()
        if kind == "item":
            bucket = None
            for prefix in ("item_", "weapon_", "ammo_", "med_", "scrap_", "gear_", "armor_"):
                if cid.lower().startswith(prefix):
                    bare = cid[len(prefix):]
                    if bare in {s.lower() for s in asset_to_bare}:
                        bucket = "catalog-prefixed→bare-asset"
                        break
            if bucket is None:
                for prefix in ("item_", "weapon_", "ammo_", "med_", "scrap_", "gear_", "armor_"):
                    if (prefix + cid.lower()) in {s.lower() for s in asset_to_bare}:
                        bucket = "catalog-bare→prefixed-asset"
                        break
            if bucket is None:
                bucket = "neither"
            bucket_samples[bucket].append((cid, Path(source).stem))
    for bucket, samples in bucket_samples.items():
        print(f"\n[{bucket}] ({(len(samples))} total)")
        for cid, cat in samples[:8]:
            print(f"  {cid}  (from {cat})")

    # Write the trace so we can also generate wiring matrix after fix
    out_dir = REPO / "docs/visual"
    out_dir.mkdir(exist_ok=True)
    (out_dir / "_trace_phase13_baseline.json").write_text(json.dumps({
        "matrix": matrix,
        "missing": [{"catalog": s, "content_id": c, "kind": k} for s, c, k in missing],
    }, indent=2))
    return matrix, missing


if __name__ == "__main__":
    main()
