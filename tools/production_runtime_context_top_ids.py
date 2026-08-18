#!/usr/bin/env python3
"""Phase 17 — Per-content_id runtime-context trace.

The AssetRegistrySelfTest probes the top N item/survivor/location IDs from
the master catalog files — these are the IDs that actually reach the runtime
at first paint. This script reconciles the production manifest against that
top-N set so the next batch can prioritize IDs that are *visible at runtime
today*, not just IDs that are missing.

Outputs:
  - docs/visual/RUNTIME_CONTEXT_TRACE.md (updated in-place)
  - docs/visual/runtime_context_top_ids.json (machine-readable)
"""
import json
import re
from pathlib import Path
from collections import defaultdict, Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
DATA = REPO / "Assets/StreamingAssets/Data"
VISUAL = REPO / "docs/visual"

TOP_N_PER_CATEGORY = 50


def extract_ids_from_json(path: Path, key: str = "id"):
    """Walk a JSON file and collect every value at `key` position."""
    if not path.exists():
        return []
    try:
        text = path.read_text()
        data = json.loads(text)
    except Exception:
        return []
    ids = []
    def walk(node):
        if isinstance(node, dict):
            for k, v in node.items():
                if k == key and isinstance(v, str):
                    ids.append(v)
                else:
                    walk(v)
        elif isinstance(node, list):
            for item in node:
                walk(item)
    walk(data)
    return ids


def load_manifest_ids():
    """Return {(content_id, family): priority_band} for every actionable row."""
    p = VISUAL / "PRODUCTION_ART_GENERATION_MANIFEST.json"
    if not p.exists():
        return {}
    m = json.loads(p.read_text())
    out = {}
    for r in m:
        if r.get("generation_status") != "PENDING":
            continue
        out[r["content_id"]] = {
            "family": r.get("visual_family", ""),
            "subfamily": r.get("subfamily", ""),
            "band": r.get("runtime_priority", ""),
            "importance": r.get("gameplay_importance", 0.0),
        }
    return out


def main():
    items = extract_ids_from_json(DATA / "items.json")
    survivors = extract_ids_from_json(DATA / "survivors.json")
    locations = extract_ids_from_json(DATA / "locations.json")
    characters = extract_ids_from_json(DATA / "characters.json")

    top_items = items[:TOP_N_PER_CATEGORY]
    top_survivors = survivors[:TOP_N_PER_CATEGORY]
    top_locations = locations[:TOP_N_PER_CATEGORY]
    top_characters = characters[:TOP_N_PER_CATEGORY]

    manifest = load_manifest_ids()

    # Per-category: which top-N IDs are in the manifest?
    in_manifest_items = [i for i in top_items if i in manifest]
    in_manifest_survivors = [s for s in top_survivors if s in manifest]
    in_manifest_locations = [l for l in top_locations if l in manifest]
    in_manifest_characters = [c for c in top_characters if c in manifest]

    # Per-category: which manifest rows are surfaced by the top-N?
    surfaced_in_manifest = set(top_items) | set(top_survivors) | set(top_locations) | set(top_characters)
    surfaced = {cid: meta for cid, meta in manifest.items() if cid in surfaced_in_manifest}
    not_surfaced = {cid: meta for cid, meta in manifest.items() if cid not in surfaced_in_manifest}

    # Surface by family: visualise which P1/etc. are runtime-checked today.
    surfaced_by_band = Counter()
    not_surfaced_by_band = Counter()
    for cid, meta in surfaced.items():
        surfaced_by_band[meta["band"]] += 1
    for cid, meta in not_surfaced.items():
        not_surfaced_by_band[meta["band"]] += 1

    out = {
        "top_n_per_category": TOP_N_PER_CATEGORY,
        "totals": {
            "items_in_catalog": len(items),
            "survivors_in_catalog": len(survivors),
            "locations_in_catalog": len(locations),
            "characters_in_catalog": len(characters),
            "manifest_actionable": len(manifest),
        },
        "top_in_manifest": {
            "items": in_manifest_items,
            "survivors": in_manifest_survivors,
            "locations": in_manifest_locations,
            "characters": in_manifest_characters,
        },
        "surfaced_count": len(surfaced),
        "not_surfaced_count": len(not_surfaced),
        "surfaced_by_band": dict(surfaced_by_band),
        "not_surfaced_by_band": dict(not_surfaced_by_band),
    }
    (VISUAL / "runtime_context_top_ids.json").write_text(json.dumps(out, indent=1))

    # Append to the existing runtime-context trace
    trace_path = VISUAL / "RUNTIME_CONTEXT_TRACE.md"
    if not trace_path.exists():
        trace_lines = ["# Runtime-context wiring trace\n\n"]
    else:
        trace_text = trace_path.read_text()
        # Strip any prior Phase 17 section
        trace_text = re.sub(r"\n## Phase 17 — Per-content_id coverage.*\Z", "", trace_text, flags=re.S)
        trace_lines = trace_text.splitlines(keepends=True)

    trace_lines.append("\n## Phase 17 — Per-content_id coverage\n\n")
    trace_lines.append(f"Taken from the top {TOP_N_PER_CATEGORY} ids of each master catalog — the slice the AssetRegistrySelfTest probes at runtime. Rows already in the manifest are *runtime-surfaced*; rows not in the manifest are *already resolved* (art exists).\n\n")
    trace_lines.append("| Category | Catalog total | Manifest actionable | Top-N in manifest | Top-N NOT in manifest |\n")
    trace_lines.append("|---|---|---|---|---|\n")
    family_by_cat = {
        "items": "Inventory-Item",
        "survivors": "Survivor-Portrait",
        "locations": "Location-Art",
        "characters": "NPC-Portrait",
    }
    for cat, total, top_n, in_m in [
        ("items", len(items), TOP_N_PER_CATEGORY, in_manifest_items),
        ("survivors", len(survivors), TOP_N_PER_CATEGORY, in_manifest_survivors),
        ("locations", len(locations), TOP_N_PER_CATEGORY, in_manifest_locations),
        ("characters", len(characters), TOP_N_PER_CATEGORY, in_manifest_characters),
    ]:
        family = family_by_cat[cat]
        actionable_in_family = sum(1 for v in manifest.values() if v["family"] == family)
        trace_lines.append(f"| {cat} | {total} | {actionable_in_family} | {len(in_m)} | {top_n - len(in_m)} |\n")
    trace_lines.append("\n")
    trace_lines.append(f"Manifest actionable rows total: **{len(manifest)}**\n")
    trace_lines.append(f"Surfaced by top-N: **{len(surfaced)}** ({len(surfaced)*100/len(manifest):.1f}%)\n")
    trace_lines.append(f"Not surfaced: **{len(not_surfaced)}** ({len(not_surfaced)*100/len(manifest):.1f}%)\n\n")
    trace_lines.append("### Surfaced by priority band\n\n")
    trace_lines.append("| Band | Surfaced | Not surfaced |\n|---|---|---|\n")
    for band in ("P0", "P1", "P2", "P3", "P4"):
        trace_lines.append(f"| {band} | {surfaced_by_band.get(band, 0)} | {not_surfaced_by_band.get(band, 0)} |\n")
    trace_lines.append("\n")
    trace_lines.append("### Not-surfaced IDs by priority band (drives Batch 1 strategy)\n\n")
    by_band_then_id = defaultdict(list)
    for cid, meta in not_surfaced.items():
        by_band_then_id[meta["band"]].append((meta["importance"], cid, meta))
    for band in ("P1", "P2", "P3", "P4"):
        ids = by_band_then_id.get(band, [])
        if not ids:
            continue
        ids.sort(key=lambda x: -x[0])
        trace_lines.append(f"**{band}** ({len(ids)} rows):\n\n")
        for imp, cid, meta in ids[:20]:
            trace_lines.append(f"- `{cid}` ({meta['family']}/{meta['subfamily']}, importance {imp})\n")
        if len(ids) > 20:
            trace_lines.append(f"- ... and {len(ids) - 20} more\n")
        trace_lines.append("\n")

    trace_path.write_text("".join(trace_lines))
    print(f"→ wrote runtime_context_top_ids.json")
    print(f"→ updated RUNTIME_CONTEXT_TRACE.md (Phase 17 section)")
    print(f"surfaced: {len(surfaced)} / {len(manifest)}")


if __name__ == "__main__":
    main()
