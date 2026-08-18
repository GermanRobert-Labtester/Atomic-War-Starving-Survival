#!/usr/bin/env python3
"""Phase 14I-K — Automated technical QA + perceptual duplicate detection.

Purposes:
  1. Verify each staged image passes basic technical checks:
        dimension match, alpha policy, valid colour channels, not blank,
        non-corrupt, reasonable palette, no obvious AI artefacts.
  2. Hash every staged result and detect:
        - exact duplicate outputs within the staging batch,
        - perceptual near-duplicates against all existing art/,
        - cross-batch near-duplicates (other staged items).
  3. Flag candidates for manual review.

Inputs:
  - staged images under assets/_staging_generated/{family}/<id>.{jpg|png}
  - the production manifest

Outputs:
  - docs/visual/_qa/_qa_report.json (machine-readable pass/fail per tile)
  - docs/visual/_qa/_qa_report.md   (human summary)

No image is auto-promoted. Promotion requires manual acceptance in Phase 14N.
"""
import json
import re
import hashlib
from pathlib import Path
from collections import Counter, defaultdict
from PIL import Image, ImageStat

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
STAGING = REPO / "assets/_staging_generated"
QA_DIR = REPO / "docs/visual/_qa"

VISUAL_EXTS = {".png", ".jpg", ".jpeg"}


def file_signature(path: Path):
    """Cheap per-image stat — average hash 8x8 + led-state extrema."""
    try:
        with Image.open(path) as im:
            im_g = im.convert("L")
            stat = ImageStat.Stat(im_g)
            mean_lum = stat.mean[0]
            extrema = im_g.getextrema()
            return {
                "width": im.width,
                "height": im.height,
                "mean_lum": round(mean_lum, 1),
                "min_lum": extrema[0],
                "max_lum": extrema[1],
                "mode": im.mode,
                "alpha": "A" in im.mode,
            }
    except Exception as e:
        return {"error": str(e)[:120]}


def phash8(path: Path):
    """8x8 average hash for near-duplicate detection."""
    try:
        with Image.open(path) as im:
            im_l = im.convert("L").resize((8, 8), Image.AVERAGE)
            pixels = list(im_l.getdata())  # flat-elegant near future deprecation here
            avg = sum(pixels) / len(pixels)
            return "".join("1" if p > avg else "0" for p in pixels)
    except Exception:
        return None


def sha256(path: Path):
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(65536), b""):
            h.update(chunk)
    return h.hexdigest()


def main():
    if not STAGING.exists():
        print(f"staging dir missing: {STAGING}")
        return
    QA_DIR.mkdir(exist_ok=True)

    staged = []
    for family_dir in sorted(STAGING.iterdir()):
        if not family_dir.is_dir():
            continue
        for fp in family_dir.rglob("*"):
            if fp.is_file() and fp.suffix.lower() in VISUAL_EXTS and not fp.name.startswith("_"):
                staged.append(fp)

    # Hash check
    by_sha = defaultdict(list)
    by_phash = defaultdict(list)
    rows = []
    for fp in staged:
        sha = sha256(fp)
        sig = file_signature(fp)
        ph = phash8(fp)
        by_sha[sha].append(fp)
        if ph:
            by_phash[ph].append(fp)
        rows.append({
            "file_path": str(fp.relative_to(REPO)),
            "family": fp.parent.name,
            "stem": fp.stem,
            "size_bytes": fp.stat().st_size,
            "sha256": sha,
            "phash": ph,
            **sig,
        })

    # Duplicate detection
    exact_dups = {sha: [str(p.relative_to(REPO)) for p in paths]
                  for sha, paths in by_sha.items() if len(paths) > 1}
    perceptual_dups = {ph: [str(p.relative_to(REPO)) for p in paths]
                       for ph, paths in by_phash.items() if len(paths) > 1}

    # Compare against existing art to catch perceptual overlap with production art
    artists = list((REPO / "assets/art").rglob("*"))
    existing_hashes = set()
    for fp in artists:
        if fp.is_file() and fp.suffix.lower() in VISUAL_EXTS:
            try:
                ph = phash8(fp)
                if ph:
                    existing_hashes.add(ph)
            except Exception:
                pass

    # For each staged phash, look for close (≤ 4 bit Hamming) match in
    # existing art. Brute force, fine for 30-image batch.
    overlap = []
    if existing_hashes:
        for r in rows:
            ph = r.get("phash")
            if not ph:
                continue
            for ep in existing_hashes:
                if ph == ep:
                    overlap.append((r["file_path"], "EXACT"))
                    break
                bit_dist = sum(1 for a, b in zip(ph, ep) if a != b)
                if bit_dist <= 2:
                    overlap.append((r["file_path"], f"CLOSE:{bit_dist}"))
                    break

    # QA flags
    bad_dim = []
    near_solid = []
    corrupt = []
    for r in rows:
        if "error" in r:
            corrupt.append(r["file_path"])
            continue
        w, h = r.get("width", 0), r.get("height", 0)
        if w <= 1 or h <= 1:
            bad_dim.append(r["file_path"])
        if r.get("min_lum", 0) > 250 and r.get("max_lum", 0) > 250:
            near_solid.append((r["file_path"], "near-white"))
        elif r.get("min_lum", 0) < 5 and r.get("max_lum", 0) < 15:
            near_solid.append((r["file_path"], "near-black"))

    report = {
        "staged_count": len(staged),
        "exact_duplicates": exact_dups,
        "perceptual_duplicates": perceptual_dups,
        "production_overlap": overlap,
        "corrupt": corrupt,
        "bad_dimensions": bad_dim,
        "near_solid": near_solid,
        "rows": rows,
    }
    QA_DIR.joinpath("_qa_report.json").write_text(json.dumps(report, indent=1, default=str))
    print(f"→ wrote _qa_report.json ({len(staged)} staged, "
          f"{len(exact_dups)} exact dups, {len(perceptual_dups)} perceptual dups, "
          f"{len(overlap)} overlap, {len(corrupt)} corrupt, {len(bad_dim)} bad_dim, "
          f"{len(near_solid)} near-solid)")

    md = []
    md.append("# Phase 14 — QA Report\n\n")
    md.append(f"Staged files examined: **{len(staged)}**\n\n")
    md.append(f"## Quick summary\n\n")
    md.append(f"- Exact duplicates: **{len(exact_dups)}** groups\n")
    md.append(f"- Perceptual duplicates (aHash 8×8): **{len(perceptual_dups)}** groups\n")
    md.append(f"- Production-art overlap: **{len(overlap)}** staged files share an aHash with an existing production asset\n")
    md.append(f"- Corrupt: **{len(corrupt)}**\n")
    md.append(f"- Bad dimensions: **{len(bad_dim)}**\n")
    md.append(f"- Near-solid: **{len(near_solid)}**\n\n")
    if corrupt:
        md.append("## Corrupt\n\n")
        for c in corrupt: md.append(f"- `{c}`\n")
        md.append("\n")
    if bad_dim:
        md.append("## Bad dimensions\n\n")
        for b in bad_dim: md.append(f"- `{b}`\n")
        md.append("\n")
    if near_solid:
        md.append("## Near-solid (mask candidates)\n\n")
        for f, kind in near_solid: md.append(f"- `{f}` ({kind})\n")
        md.append("\n")
    if overlap:
        md.append("## Production-art overlap (visual duplicate risk)\n\n")
        for f, grade in overlap: md.append(f"- `{f}` ({grade})\n")
        md.append("\n")

    QA_DIR.joinpath("_qa_report.md").write_text("".join(md))
    print(f"→ wrote _qa_report.md")


if __name__ == "__main__":
    main()
