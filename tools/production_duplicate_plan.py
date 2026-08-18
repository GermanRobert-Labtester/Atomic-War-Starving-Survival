#!/usr/bin/env python3
"""Phase 14T — Duplicate consolidation plan.

For every exact and perceptual duplicate group in the visual library:
  - rank by potential consolidation value:
    * exact duplicates that are pairwise byte-identical between deprecated
      and active variants → highest value
    * exact duplicates within the same visual family → medium
    * cross-extension duplicates (.jpg + .png of same image) → medium
    * perceptual near-duplicates → lowest confidence (manual review)

DO NOT actually delete or move files. Output is a plan only.

Output: docs/visual/DUPLICATE_CONSOLIDATION_PLAN.md
"""
import json
import hashlib
from pathlib import Path
from collections import Counter, defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ART = REPO / "assets/art"
MANIFEST_OBJ = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
MANIFEST = MANIFEST_OBJ if isinstance(MANIFEST_OBJ, list) else MANIFEST_OBJ["active_assets"]

# MD5 grouping (existing duplicate report also covers this)
md5_groups = defaultdict(list)
for r in MANIFEST:
    if not isinstance(r, dict): continue
    sha = r.get("sha256")
    if sha:
        md5_groups[sha].append(r)

PER_GROUP_WASTE_BUDGET = []
plan = []
total_potential_waste = 0
for sha, entries in md5_groups.items():
    if len(entries) < 2:
        continue
    paths = []
    stems = []
    for e in entries:
        fp = e.get("full_path") or e.get("file_path") or ""
        if fp.startswith("/"):
            try:
                fp = str(Path(fp).relative_to(REPO))
            except ValueError:
                pass
        if fp:
            paths.append(fp)
            stems.append(e.get("stem") or "")
    # Categorise by relationship
    is_deprecated_active = any(s.startswith("ammo_deprecated_") for s in stems) and any(not s.startswith("ammo_deprecated_") for s in stems)
    is_cross_ext = (
        len({Path(p).suffix for p in paths}) > 1
        and sorted(stems) == [Path(p).stem for p in paths]
    )
    same_stem = len(set(stems)) == 1
    if is_deprecated_active:
        rank = "HIGH"
        rationale = "Deprecated ammo mirroring active variant. Cross-verified by Phase 14S."
        potential_save_bytes = (len(entries) - 1) * (entries[0].get("size_bytes") or 0)
    elif same_stem:
        rank = "MEDIUM"
        rationale = "Same stem across multiple files (likely cross-extension pair)."
        potential_save_bytes = (len(entries) - 1) * (entries[0].get("size_bytes") or 0)
    elif is_cross_ext:
        rank = "MEDIUM"
        rationale = "Cross-extension pair."
        potential_save_bytes = (len(entries) - 1) * (entries[0].get("size_bytes") or 0)
    else:
        rank = "LOW"
        rationale = "Multiple distinct stems sharing MD5 (rare). Manual review."
        potential_save_bytes = (len(entries) - 1) * (entries[0].get("size_bytes") or 0)
    plan.append({
        "rank": rank,
        "md5_or_sha": sha[:10] + "...",
        "file_count": len(entries),
        "potential_save_bytes": potential_save_bytes,
        "stems": stems,
        "paths": paths,
        "rationale": rationale,
    })
    total_potential_waste += potential_save_bytes

# Sort by potential save descending
plan.sort(key=lambda r: -r["potential_save_bytes"])

# Markdown
md = []
md.append("# ASHFALL — Duplicate Consolidation Plan\n\n")
md.append(f"Phase 14T. **DOES NOT delete or move files.** Output is a documented plan only.\n\n")
md.append(f"Total exact-duplicate groups: **{len(plan)}**\n\n")
md.append(f"Grouped by rank:\n\n")
md.append("| Rank | Groups | Estimated disk waste |\n|---|---|---|\n")
for rank in ("HIGH", "MEDIUM", "LOW"):
    n = sum(1 for r in plan if r["rank"] == rank)
    waste = sum(r["potential_save_bytes"] for r in plan if r["rank"] == rank)
    md.append(f"| `{rank}` | {n} groups | ~{waste:,} bytes ({waste/1024:.1f} KiB) |\n")
md.append(f"| **Total** | **{len(plan)} groups** | **~{total_potential_waste:,} bytes ({total_potential_waste/(1024*1024):.2f} MiB)** |\n\n")

md.append("## HIGH-rank groups (deprecated / active pairs)\n\n")
md.append("These have the strongest single deletion signal: byte-identical; relationship is\n")
md.append("explicit by naming (e.g. `ammo_9x19.jpg` ↔ `ammo_deprecated_9x19.jpg`).\n\n")
md.append("| Group sha | Stems | Save |\n|---|---|---|\n")
for r in plan:
    if r["rank"] != "HIGH": continue
    md.append(f"| `{r['md5_or_sha']}` | `{'`, `'.join(r['stems'])}` | {r['potential_save_bytes']:,} bytes |\n")
md.append("\n")

md.append("## MEDIUM-rank groups (cross-extension, same stem)\n\n")
md.append("Sample (first 30 by potential save):\n\n")
md.append("| Group sha | Files | Save |\n|---|---|---|\n")
n_med = 0
for r in plan:
    if r["rank"] != "MEDIUM": continue
    md.append(f"| `{r['md5_or_sha'][:10]}…` | `{len(r['paths'])} files` | {r['potential_save_bytes']:,} bytes |\n")
    n_med += 1
md.append(f"\n({n_med} medium-rank groups)\n\n")

md.append("## LOW-rank groups (multi-stem, manual review)\n\n")
md.append("Sample (first 20):\n\n")
n_low = 0
for r in plan:
    if r["rank"] != "LOW": continue
    md.append(f"| `{r['md5_or_sha'][:10]}…` | `{'`, `'.join(r['stems'][:4])}` | {r['potential_save_bytes']:,} bytes |\n")
    n_low += 1
    if n_low >= 20:
        break
md.append(f"\n({n_low} shown; total {sum(1 for r in plan if r['rank']=='LOW')} low-rank groups)\n\n")

md.append("## Recommended deletion policy (NOT executed this phase)\n\n")
md.append("Before any deletion:\n")
md.append("1. Confirm canonical survivor (The `ammo_<cal>_<type>.jpg` is canonical; the `ammo_deprecated_*` is mirror).\n")
md.append("2. Trace all runtime references — `assets/art/*` is consumed by `src/Host/AssetRegistry.cs` via `ItemSearchPaths`. Identical content means duplicate lookup will return the first match deterministically.\n")
md.append("3. Save compatibility is irrelevant: deprecated ammo entries are not stored in save state.\n")
md.append("4. Tests pass — the existing `--asset-registry-selftest` continues to find at least one valid file for any deprecated id even after deletion.\n")
md.append("5. Move phase — move deprecated to `assets/_legacy_compat/` (gitignore or quarantine dir) before destructive deletion.\n\n")
md.append("Phase 14T only documents. No destructive cleanup is performed.\n")

out = REPO / "docs/visual/DUPLICATE_CONSOLIDATION_PLAN.md"
out.write_text("".join(md))
print(f"→ wrote {out.name}")
print(f"total groups: {len(plan)}, total potential waste: {total_potential_waste:,} bytes")
