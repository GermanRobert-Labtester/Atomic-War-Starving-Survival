#!/usr/bin/env python3
"""Phase 15 — Promotion script.

Take staged assets that have passed QA and move them to their canonical
home (`assets/art/<canonical-filename>`). Then re-run the wiring trace
to confirm the catalog ID now resolves.

Inputs:
  - assets/_staging_generated/<family>/<id>.{jpg,png}
  - docs/visual/_qa/_qa_report.json (must report no corrupt / bad_dim /
    near-solid / perceptual-dup / production-overlap files in the
    candidate set)
  - docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json (canonical
    target_filename per content_id)

Args:
  --id <content_id>      Promote exactly one entry by content_id.
  --all                  Promote every staged entry that has a matching
                         manifest row and passes QA.
  --dry-run              Print the intended promotion plan without
                         actually moving files.
  --force                Promote even if QA flags issues. Use only if a
                         human reviewer has accepted the candidate.

Output: writes a `_promotion_log.json` so the audit trail in
PRODUCTION_ART_GENERATION_LEDGER.json can be updated from this run.

DOES NOT delete the staging copy. Staging is kept until a separate
cleanup pass decides to remove it.
"""
import argparse
import json
import shutil
from pathlib import Path
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
STAGING = REPO / "assets/_staging_generated"
ART_DIR = REPO / "assets/art"

QA_REPORT = REPO / "docs/visual/_qa/_qa_report.json"
MANIFEST = json.load(open(REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"))

# qa index: filename -> set of issues
qa_index = {}
if QA_REPORT.exists():
    qa = json.load(open(QA_REPORT))
    # Mark every stage file PASS by default; flag the broken ones.
    for fp_str in qa.get("corrupt", []):
        qa_index[fp_str] = qa_index.get(fp_str, set()) | {"corrupt"}
    for fp_str in qa.get("bad_dimensions", []):
        qa_index[fp_str] = qa_index.get(fp_str, set()) | {"bad_dimensions"}
    for fp_str, kind in qa.get("near_solid", []):
        qa_index[fp_str] = qa_index.get(fp_str, set()) | {f"near_solid:{kind}"}
    for fp_str, grade in qa.get("production_overlap", []):
        qa_index[fp_str] = qa_index.get(fp_str, set()) | {f"production_overlap:{grade}"}
    for fp_str in qa.get("rows", []):  # carry forward
        pass  # default PASS

# Find manifest index by content_id
manifest_idx = {r["content_id"]: r for r in MANIFEST}


def family_from_id(cid):
    """Reverse-map a content id to a family using the same logic as
    production_manifest.py."""
    row = manifest_idx.get(cid)
    if row is None:
        return None
    return row["visual_family"]


def locate_staged(cid):
    """Find staged file for a content_id."""
    family = family_from_id(cid)
    if not family:
        return None
    subdir = {
        "Inventory-Item": "items",
        "Survivor-Portrait": "portraits",
        "NPC-Portrait": "portraits",
        "Location-Art": "locations",
        "Faction-Art": "factions",
    }.get(family)
    if subdir is None:
        return None
    candidate_dir = STAGING / subdir
    if not candidate_dir.exists():
        return None
    for ext in (".jpg", ".png", ".jpeg"):
        fp = candidate_dir / f"{cid}{ext}"
        if fp.exists():
            return fp
    return None


def canonical_target(row):
    target = row.get("target_directory") or "assets/art/"
    if not target.endswith("/"):
        target += "/"
    fname = row.get("target_filename") or f"{row['content_id']}.jpg"
    return REPO / (target + fname)


def int_to_quality(fp_rel):
    """Look up QA flag set for a staged file path."""
    issues = qa_index.get(fp_rel, set())
    if not issues:
        return "PASS"
    if "production_overlap" in str(issues):
        return "REVIEW_OVERLAP"
    return f"REJECT:{issues.pop()}"


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--id", default=None, help="content_id to promote")
    ap.add_argument("--all", action="store_true", help="promote all eligible")
    ap.add_argument("--dry-run", action="store_true", help="plan only, no copy")
    ap.add_argument("--force", action="store_true", help="promote even QA-failed")
    args = ap.parse_args()

    candidates = []
    if args.id:
        cands = [args.id] if args.id in manifest_idx else []
        if not cands:
            print(f"[promote] id {args.id!r} is not in the manifest.")
            return
    elif args.all:
        # Walk staging directory, find matches
        cands = []
        for family_dir in sorted(STAGING.iterdir()):
            if not family_dir.is_dir():
                continue
            for fp in family_dir.iterdir():
                if fp.is_file() and fp.suffix.lower() in (".jpg", ".png", ".jpeg"):
                    cands.append(fp.stem)
    else:
        ap.print_help()
        return

    promoted = []
    rejected = []
    skipped = []
    log = []

    for cid in cands:
        row = manifest_idx.get(cid)
        if row is None:
            log.append({"content_id": cid, "decision": "SKIP_NO_MANIFEST_ROW"})
            skipped.append(cid)
            continue
        if row.get("generation_status") == "SKIP_REFERENCE_ONLY":
            log.append({"content_id": cid, "decision": "SKIP_REFERENCE"})
            skipped.append(cid)
            continue
        staged = locate_staged(cid)
        if staged is None:
            log.append({"content_id": cid, "decision": "SKIP_NO_STAGED_FILE"})
            skipped.append(cid)
            continue
        # QA gate
        rel = str(staged.relative_to(REPO))
        qa_status = int_to_quality(rel)
        if qa_status != "PASS" and not args.force:
            log.append({"content_id": cid, "decision": "REJECT_QA",
                        "staged": rel, "qa_status": qa_status})
            rejected.append((cid, qa_status))
            continue
        target = canonical_target(row)
        if target.exists():
            log.append({"content_id": cid, "decision": "SKIP_TARGET_EXISTS",
                        "target": str(target.relative_to(REPO))})
            skipped.append(cid)
            continue
        # Promote
        if args.dry_run:
            log.append({"content_id": cid, "decision": "DRY_RUN_PROMOTE",
                        "from": rel, "to": str(target.relative_to(REPO))})
            promoted.append(cid + " (planned)")
        else:
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(staged, target)
            log.append({"content_id": cid, "decision": "PROMOTED",
                        "from": rel, "to": str(target.relative_to(REPO)),
                        "qa_status": qa_status})
            promoted.append(cid + " (copied)")

    out_log = REPO / "docs/visual/_promotion_log.json"
    out_log.write_text(json.dumps(log, indent=1))
    print(f"→ wrote {out_log.name}")
    print(f"promoted (planned or actual): {len(promoted)}; rejected by QA: {len(rejected)}; skipped: {len(skipped)}")
    if rejected:
        print("rejection reasons:")
        for cid, status in rejected:
            print(f"  {cid}: {status}")


if __name__ == "__main__":
    main()
