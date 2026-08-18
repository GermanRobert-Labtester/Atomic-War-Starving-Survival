#!/usr/bin/env python3
"""Phase 14S — Deprecated ammo audit.

For every deprecated ammo file in assets/art/:
  - find its active counterpart (likely `ammo_<cal>.{jpg,png}` -> byte-duplicate)
  - check whether it is referenced from any catalog
  - check whether the file is byte-identical to its active counterpart
  - classify

Output: docs/visual/DEPRECATED_AMMO_AUDIT.md
"""
import json
import hashlib
from pathlib import Path
from collections import defaultdict
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
ART = REPO / "assets/art"
DATA = REPO / "Assets/StreamingAssets/Data"

MANIFEST = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))

# Build active catalog id set (any non-deprecated ammo id)
active_ammo_ids = set()
asset_to_id = {}  # absolute_path → sha256
artifact_index = {}  # canonical filename → md5 (from manifest)

active = MANIFEST if isinstance(MANIFEST, list) else MANIFEST["active_assets"]
for r in active:
    if not isinstance(r, dict): continue
    fp = r.get("full_path") or r.get("file_path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    sha = r.get("sha256")
    stem = r.get("stem") or ""
    if fp:
        artifact_index.setdefault(stem, fp)
    if sha:
        asset_to_id[fp] = sha

# Filesystem walk for deprecated ammo
depr_files = list(ART.rglob("ammo_deprecated_*"))
catalog_ammo_ids = set()
for fp_wm in WM:
    if fp_wm.get("catalog", "").endswith("items.json") or fp_wm.get("catalog", "").endswith("ammo.json"):
        cid = fp_wm.get("content_id", "").lower()
        if cid.startswith("ammo_") and not cid.startswith("ammo_deprecated_"):
            catalog_ammo_ids.add(cid)

# Quick code reference search
deprecated_id_pattern = "ammo_deprecated_"
referenced_in_code = set()
for fp in (REPO / "src").rglob("*"):
    if fp.is_file() and (fp.suffix in (".cs", ".gd", ".csproj", ".md") or fp.name.startswith("HostCli")):
        try:
            text = fp.read_text()
        except Exception:
            continue
        # Search whole-word matches
        for token in ("ammo_deprecated_cal_12ga", "ammo_deprecated_cal_9x19",
                      "ammo_deprecated_cal_762x39", "ammo_deprecated_cal_556x45"):
            if token in text:
                referenced_in_code.add(token)

rows = []
def sha256(p):
    h = hashlib.sha256()
    h.update(p.read_bytes())
    return h.hexdigest()

# For each deprecated file find candidate active counterpart
identify_pairs = {}
for fp in depr_files:
    stem = fp.stem
    # active counterpart: strip "deprecated_" / "deprecated_cal_"
    if stem.startswith("ammo_deprecated_cal_"):
        suffix = stem[len("ammo_deprecated_cal_"):]
        candidate = f"ammo_{suffix}"
    elif stem.startswith("ammo_deprecated_"):
        suffix = stem[len("ammo_deprecated_"):]
        candidate = f"ammo_{suffix}"
    else:
        candidate = None
    counterpart = ART / f"{candidate}{fp.suffix}" if candidate else None
    has_pair = counterpart.exists() if counterpart else False
    is_dup = False
    if has_pair and counterpart:
        try:
            is_dup = sha256(fp) == sha256(counterpart)
        except Exception:
            pass
    # Check catalog references for the deprecated id
    in_catalog = any(
        e["content_id"].lower() == stem.lower()
        for e in WM
        if e["resolved_path"] == "MISSING" or e["resolved_path"] != "MISSING"
    )
    # Code reference
    in_code = stem in referenced_in_code or any(stem.startswith(prefix) for prefix in referenced_in_code)

    # Classify
    if is_dup and has_pair:
        classification = "MERGE_CANDIDATE"
        rationale = "Byte-identical to active counterpart. Merge candidate."
    elif not in_catalog and not in_code and not has_pair:
        classification = "SAFE_TO_RETIRE_LATER"
        rationale = "Not in catalog, not in code, no active counterpart."
    elif in_catalog:
        classification = "KEEP_COMPAT"
        rationale = "Referenced from a catalog (rarely)."
    elif in_code:
        classification = "KEEP_COMPAT"
        rationale = "Referenced from a source file."
    elif has_pair:
        classification = "SAFE_TO_RETIRE_LATER"
        rationale = "Has active counterpart but not identical; possibly different export."
    else:
        classification = "UNKNOWN"
        rationale = "Pending manual review."

    rows.append({
        "stem": stem,
        "file_path": str(fp.relative_to(REPO)),
        "size_bytes": fp.stat().st_size,
        "candidate_active": (candidate + fp.suffix) if has_pair else None,
        "is_dup_pair": is_dup,
        "in_catalog": in_catalog,
        "in_code": in_code,
        "classification": classification,
        "rationale": rationale,
    })

# Markdown output
md = []
md.append("# ASHFALL — Deprecated Ammo Audit\n\n")
md.append(f"Total deprecated ammo files found in `assets/art/`: **{len(depr_files)}**.\n\n")
md.append(f"Classifications:\n\n")
md.append("| Bucket | Count | Description |\n|---|---|---|\n")
md.append(f"| `KEEP_COMPAT` | {sum(1 for r in rows if r['classification']=='KEEP_COMPAT')} | referenced by catalog or code; keep until cleanup replaces actively |\n")
md.append(f"| `MERGE_CANDIDATE` | {sum(1 for r in rows if r['classification']=='MERGE_CANDIDATE')} | byte-identical to active variant; safe to alias away |\n")
md.append(f"| `SAFE_TO_RETIRE_LATER` | {sum(1 for r in rows if r['classification']=='SAFE_TO_RETIRE_LATER')} | no consumer; candidate for cleanup phase |\n")
md.append(f"| `UNKNOWN` | {sum(1 for r in rows if r['classification']=='UNKNOWN')} | manual review |\n")

md.append("\n## Per-file detail\n\n")
md.append("| Stem | Path | Pair exists | Byte-dup | Catalog ref | Code ref | Classification |\n")
md.append("|---|---|---|---|---|---|---|\n")
for r in rows:
    md.append(
        f"| `{r['stem']}` | `{r['file_path']}` | "
        f"{'yes' if r['candidate_active'] else 'no'} | "
        f"{'yes' if r['is_dup_pair'] else 'no'} | "
        f"{'yes' if r['in_catalog'] else 'no'} | "
        f"{'yes' if r['in_code'] else 'no'} | "
        f"`{r['classification']}` |\n"
    )

out = REPO / "docs/visual/DEPRECATED_AMMO_AUDIT.md"
out.write_text("".join(md))
print(f"→ wrote {out.name}")
print(f"Deprecated ammo: {len(rows)}, classification breakdown: {dict(Counter(r['classification'] for r in rows))}")
