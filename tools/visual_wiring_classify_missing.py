#!/usr/bin/env python3
"""Phase 13.5 — Classify every still-missing row by root cause.

Categories (from the Phase 13 brief):
  A. ACTUALLY_MISSING_ART
  B. NAMING_MISMATCH   (this is what prefix-add addresses — kept here for completeness)
  C. BAD_CATALOG_ID    (catalog entry looks wrong)
  D. LEGACY_CONTENT    (referenced only by deprecated gameplay systems)
  E. DEPRECATED_CONTENT
  F. REFERENCE_ONLY    (faction_war_*, etc. — filtered out above but we re-check)
  G. INTENTIONAL_FALLBACK
  H. WRONG_CATEGORY_SEARCH_PATH  (e.g. characters.json id sits in portrait roots, but the row is in item roots?)
  I. AMBIGUOUS
"""
import json
from pathlib import Path
from collections import Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")

WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))

# Reference data sets
missing = [e for e in WM if e["resolved_path"] == "MISSING"]

# Pre-known reference-only catalogs (no visual surface "by design")
NON_VISUAL_KINDS = (
    "faction_war_radio", "faction_war_journal", "faction_war_dialogue",
    "faction_war_communiques", "faction_war_location_overrides",
    "holding_record_survivors",  # not in this project
)
TRULY_NON_VISUAL = {"faction_war_radio.json", "faction_war_journal.json",
                    "faction_war_dialogue.json", "faction_war_communiques.json",
                    "faction_war_location_overrides.json", "recipes.json",
                    "relic_recipes.json"}

# Asset stem index
art_stems = {fp.stem.lower() for fp in (REPO / "assets/art").iterdir()
             if fp.is_file() and fp.suffix.lower() in ('.jpg', '.png')}
for sub in ("Items", "Portraits", "Locations", "Factions"):
    d = REPO / "assets/sprites" / sub
    if d.exists():
        for fp in d.rglob("*"):
            if fp.is_file() and fp.suffix.lower() in ('.jpg', '.png'):
                art_stems.add(fp.stem.lower())


def heuristic_classify(entry):
    cid = entry["content_id"].lower()
    catfile = Path(entry["catalog"]).stem
    
    # Rule F: reference-only catalog
    if catfile in TRULY_NON_VISUAL:
        return "F.REFERENCE_ONLY"
    if catfile in {"recipes", "recipe_dsl"}:
        return "F.REFERENCE_ONLY"
    
    # Rule E: deprecated ammo family (catalog entry starts with ammo_deprecated_)
    if cid.startswith("ammo_deprecated_"):
        return "E.DEPRECATED_CONTENT"
    
    # Rule H: characters.json → portrait; if kind==portrait and not in portrait roots,
    # probably a wrong-category alias issue.
    # (Already handled by the resolver for canonical prefix variations.)
    
    # Rule B: naming mismatch — could be one of these:
    #   - bare stem not in art, prefixed stem not in art (file genuinely missing)
    #   - bare stem not in art, prefixed stem IS in art (this is what prefix-add fixes)
    #   - bare stem IS in art but the canonical kind roots don't carry the prefix
    #     (this catches e.g. "npc_X" in characters.json, kind=portrait).
    # After the postfix, only "unique" remaining missings matter.
    if cid in art_stems:
        return "H.WRONG_CATEGORY_SEARCH_PATH"  # the bare file exists but our roots didn't include a non-prefixed slot
    # Otherwise genuinely missing.
    return "A.ACTUALLY_MISSING_ART"


classes = Counter()
buckets = {"A.ACTUALLY_MISSING_ART": [], "B.NAMING_MISMATCH": [],
           "C.BAD_CATALOG_ID": [], "D.LEGACY_CONTENT": [],
           "E.DEPRECATED_CONTENT": [], "F.REFERENCE_ONLY": [],
           "G.INTENTIONAL_FALLBACK": [], "H.WRONG_CATEGORY_SEARCH_PATH": [],
           "I.AMBIGUOUS": []}
for e in missing:
    cls = heuristic_classify(e)
    classes[cls] += 1
    buckets[cls].append(e)


print(f"Total still-missing: {len(missing)}")
print(f"\nClassification:")
for c, n in classes.most_common():
    print(f"  {c}: {n}")

# Detailed report
print(f"\n── Subset details ──")
for cls in ("A.ACTUALLY_MISSING_ART", "B.NAMING_MISMATCH",
            "H.WRONG_CATEGORY_SEARCH_PATH", "E.DEPRECATED_CONTENT",
            "F.REFERENCE_ONLY"):
    rows = buckets[cls]
    print(f"\n[{cls}] {len(rows)} rows (first 12):")
    for r in rows[:12]:
        print(f"  {r['content_id']}  (kind={r['kind']}, from {Path(r['catalog']).stem})")

# Write classification
(REPO / "docs/visual/_phase13_missing_classification.json").write_text(json.dumps({
    "counts": dict(classes),
    "samples": {cls: [{"id": e["content_id"], "catalog": e["catalog"], "kind": e["kind"]}
                      for e in buckets[cls][:30]]
               for cls in classes}
}, indent=2))
print(f"\n→ wrote _phase13_missing_classification.json")
