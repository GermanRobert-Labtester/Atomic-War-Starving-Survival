#!/usr/bin/env python3
"""
ASHFALL Phase B — cheap per-image statistics, intended for the 2335 assets.
Uses ImageStat and getextrema — no per-pixel lists.
"""
import json
from pathlib import Path
from PIL import Image, ImageStat
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
M = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))

NEAR_WHITE = 0.95
NEAR_BLACK = 0.95


def pixel_signature(file_path: Path):
    """Average-hash + approximate near-white/near-black detection using extrema.
    Avoids per-pixel Python lists. mu/gray = mean grey level, w_min/max for white
    count, b_min/max for black count. Returns (mean_lum, max_lum, min_lum).
    """
    try:
        with Image.open(file_path) as im:
            if im.mode == "P" or im.mode == "1":
                # palette/bitmap — skip
                return None
            if im.mode != "L":
                im_g = im.convert("L")
            else:
                im_g = im
            stat = ImageStat.Stat(im_g)
            mean_lum = stat.mean[0]
            extrema = im_g.getextrema()  # (min, max) tuple
            return (round(mean_lum, 1), extrema[0], extrema[1])
    except Exception:
        return None


def main():
    sigs = Counter()
    ext_count = Counter()
    flagged = []
    for r in M:
        if r.get("file_type") not in (".png", ".jpg", ".jpeg"):
            continue
        if not r.get("md5") or r["md5"] == "UNREADABLE":
            continue
        p = REPO / r["file_path"]
        sig = pixel_signature(p)
        if sig is None:
            continue
        mean_lum, lo, hi = sig
        ext_count[(lo, hi)] += 1
        # Heuristic: if max < 15 → pure black; if min > 240 → pure white
        if hi < 15 and lo < 15:
            kind = "near-black"
        elif lo > 240 and mean_lum > 240:
            kind = "near-white"
        else:
            kind = "image"
        sigs[kind] += 1
        if kind in ("near-black", "near-white"):
            flagged.append((r["file_path"], kind, round(mean_lum, 1)))

    print(f"distribution: {dict(sigs)}")
    print(f"flagged: {len(flagged)}")
    print()
    print("Sample near-white / near-black assets:")
    for fp, kind, mu in flagged[:40]:
        print(f"  [{kind:11s}] {fp} (mean_lum={mu})")
    print(f"  … {len(flagged)} total")

    out = {
        "distribution": dict(sigs),
        "flagged_count": len(flagged),
        "flagged_first_50": flagged[:50],
        "extrema_distribution": {f"{lo}-{hi}": n for (lo, hi), n in ext_count.most_common(20)},
    }
    (REPO / "docs/visual/pixel_signature_stats.json").write_text(json.dumps(out, indent=1))
    print()
    print(f"extrema histogram (top 20): {out['extrema_distribution']}")


if __name__ == "__main__":
    main()
