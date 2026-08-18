#!/usr/bin/env python3
"""Phase 14U — Orphan reclassification.

Re-derive orphans after Phase 14's family classification & manifest build.
The old count (1,687 / 700-meaningful) is preserved for traceability but
augmented with classification by likely role:
  ACTIVE         : referenced from some catalog row that drives runtime
  RESOLVABLE_BY_NORMALIZATION: would resolve via the Phase-13 prefix-add
  STITCH         : under ui/Screens/ — Stitch export, reference only
  FIGMA          : under ui/Icons/ that match design tool exports
  LEGACY         : deprecated ammo / helmet-deprecated /
  FUTURE_CONTENT / UNKNOWN : no clear role identified
  TRUE_ORPHAN    : not referenced anywhere

Output: docs/visual/ORPHAN_VISUAL_ASSETS.md
"""
import json
from pathlib import Path
from collections import Counter, defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
M_OBJ = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
MANIFEST = M_OBJ if isinstance(M_OBJ, list) else M_OBJ["active_assets"]
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))

# Build reachable set from wiring matrix (after Phase 13 prefix-add)
reachable = set()
for e in WM:
    if e["resolved_path"] != "MISSING":
        reachable.add(e["resolved_path"])

orphans = []
for r in MANIFEST:
    if not isinstance(r, dict):
        continue
    fp = r.get("full_path") or r.get("file_path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    if fp in reachable:
        continue
    stem = r.get("stem") or ""
    # Classify by path + stem
    cls = "TRUE_ORPHAN"
    rationale = "not referenced from any catalog row nor reachable through any resolver rule"
    if "/ui/Screens/" in fp:
        cls = "STITCH"
        rationale = "Stitch dashboard export — reference only, not runtime"
    elif "/ui/Icons/" in fp:
        cls = "FIGMA"
        rationale = "Figma export — reference only"
    elif "/ui/FactionEmblems/" in fp:
        cls = "FIGMA"
        rationale = "2K faction emblem export — reference only"
    elif "/ui/MainMenu/" in fp:
        cls = "FIGMA"
        rationale = "Main menu export — reference only"
    elif stem.startswith("ammo_deprecated_") or stem.startswith("helmet_deprecated"):
        cls = "LEGACY"
        rationale = "deprecated variant; HAS active counterpart"
    elif stem.startswith(("faction_badge", "emblem_")):
        cls = "LEGACY"
        rationale = "emblem artifact; check current faction roster"
    elif "/ui/" not in fp and stem in ("item_ammo_ap", "item_ammo_hp",
                                       "item_ammo_standard", "item_icon",
                                       "item_id", "item_patterns",
                                       "item_type", "item_rarity_common",
                                       "item_rarity_rare", "item_rarity_uncommon",
                                       "item_rarity_unique", "item_id_prefix",
                                       "character_icon", "shelter_icon",
                                       "journal_icon", "crafting_icon",
                                       "inventory_icon", "map_icon",
                                       "location_pin_icon", "quest_tracker_ui",
                                       "enc_a", "enc_b", "enc_x",
                                       "faction_leader_1", "faction_leader_2",
                                       "time_play_icon", "time_pause_icon",
                                       "time_ffw_icon"):
        cls = "LEGACY"  # includes some placeholder patterns
        rationale = "Placeholder / template asset, possibly inactive"
    elif stem.startswith(("vfx_", "muzzle_", "trail_", "particles_", "fog_",
                          "smoke_", "dust_", "ash_", "snow_")):
        cls = "FUTURE_CONTENT"
        rationale = "FX layer reserved for future wires"
    orphans.append({"stem": stem, "file_path": fp, "classification": cls,
                    "rationale": rationale})

# Write markdown
md = []
md.append("# ASHFALL — Orphan Visual Asset Re-classification\n\n")
md.append("Phase 14U augmentation. The Phase-12 audit reported **`1,687` orphans** / **`~700` meaningful**.\n\n")
md.append(f"Reclassified this phase: **{len(orphans)}** reachable set was re-evaluated against Phase-13 prefix-add resolution and Phase-14 family classification tooling.\n\n")
md.append("| Bucket | Count | Description |\n|---|---|---|\n")
buckets = Counter(o["classification"] for o in orphans)
for cls in ("ACTIVE", "RESOLVABLE_BY_NORMALIZATION", "STITCH",
            "FIGMA", "LEGACY", "FUTURE_CONTENT", "TRUE_ORPHAN", "UNKNOWN"):
    md.append(f"| `{cls}` | {buckets.get(cls, 0)} | {cls.lower()} classification |\n")
md.append(f"\n| **Total** | **{len(orphans)}** | |\n\n")

for cls in ("LEGACY", "FUTURE_CONTENT", "TRUE_ORPHAN", "STITCH", "FIGMA"):
    md.append(f"## {cls}\n\n")
    md.append(f"{sum(1 for o in orphans if o['classification'] == cls)} files\n\n")
    md.append("| Stem | Path | Rationale |\n|---|---|---|\n")
    rows = [o for o in orphans if o["classification"] == cls]
    rows.sort(key=lambda o: o["file_path"])
    for r in rows[:80]:
        md.append(f"| `{r['stem']}` | `{r['file_path']}` | {r['rationale']} |\n")
    if len(rows) > 80:
        md.append(f"| … | … | (omitted {len(rows) - 80} more) |\n")
    md.append("\n")

# Top by file size
top_by_size = sorted(orphans, key=lambda o: -Path(REPO / o["file_path"]).stat().st_size if (REPO / o["file_path"]).exists() else -1)[:30]
md.append("## Largest orphans by file size\n\n")
md.append("| Stem | Path | Size (bytes) |\n|---|---|---|\n")
for o in top_by_size:
    try:
        sz = (REPO / o["file_path"]).stat().st_size
    except Exception:
        sz = 0
    md.append(f"| `{o['stem']}` | `{o['file_path']}` | {sz:,} |\n")
md.append("\n")

out = REPO / "docs/visual/ORPHAN_VISUAL_ASSETS.md"
out.write_text("".join(md))
print(f"→ wrote {out.name}")
print(f"orphans: {len(orphans)}, by classification: {dict(buckets)}")
