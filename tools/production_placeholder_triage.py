#!/usr/bin/env python3
"""Phase 14R — Placeholder audit.

The Phase-12 audit identified ~252 byte-identical catalog placeholder files.
This script performs a heuristic triage:

  A. ACTIVE_PRODUCTION_PLACEHOLDER — reachable from a real catalog id that
     drives a runtime render path. Should be considered for replacement.
  B. GENERIC_RUNTIME_UTILITY — load  by hero/chrome code paths for default
     or template rendering. Don't replace, keep as "utility".
  C. CATALOG_TEMPLATE / REFERENCE — referenced only by tools or content
     authors internally. Don't replace.
  D. DEBUG — named or paths that suggest debug/test (e.g. _test_, debug_).
  E. DEPRECATED — matches `ammo_deprecated_*` or similar.
  F. UNKNOWN — pending review.

Output:
  docs/visual/PLACEHOLDER_TRIAGE.md
"""
import json
from pathlib import Path
from collections import Counter, defaultdict

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
MANIFEST = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
active = (MANIFEST if isinstance(MANIFEST, list) else MANIFEST["active_assets"])

# Placeholder naming patterns
PLACEHOLDER_PATTERNS = [
    ("item_ammo_ap.jpg", "A:ACTIVE_PRODUCTION_PLACEHOLDER"),
    ("item_ammo_hp.jpg", "A:ACTIVE_PRODUCTION_PLACEHOLDER"),
    ("item_ammo_standard.jpg", "A:ACTIVE_PRODUCTION_PLACEHOLDER"),
    ("item_id.jpg", "B:RUNTIME_UTILITY"),
    ("item_icon.jpg", "B:RUNTIME_UTILITY"),
    ("item_patterns.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_type.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_id_prefix.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_rarity_common.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_rarity_rare.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_rarity_uncommon.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_rarity_unique.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_ammo_types.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_keycards.jpg", "B:RUNTIME_UTILITY"),
    ("item_worldcatalog_loot.jpg", "C:TEMPLATE_REFERENCE"),
    ("item_ammotypes_combatloot.jpg", "C:TEMPLATE_REFERENCE"),
    ("enc_a.jpg", "F:UNKNOWN"),
    ("enc_b.jpg", "F:UNKNOWN"),
    ("enc_x.jpg", "F:UNKNOWN"),
    ("enc_a.png", "F:UNKNOWN"),
    ("enc_x.png", "F:UNKNOWN"),
    ("encounter_category.jpg", "C:TEMPLATE_REFERENCE"),
    ("quest_tracker_ui.jpg", "B:RUNTIME_UTILITY"),
    ("crafting_icon.jpg", "B:RUNTIME_UTILITY"),
    ("character_icon.jpg", "B:RUNTIME_UTILITY"),
    ("inventory_icon.jpg", "B:RUNTIME_UTILITY"),
    ("location_pin_icon.jpg", "B:RUNTIME_UTILITY"),
    ("map_icon.jpg", "B:RUNTIME_UTILITY"),
    ("journal_icon.jpg", "B:RUNTIME_UTILITY"),
    ("shelter_icon.jpg", "B:RUNTIME_UTILITY"),
    ("faction_leader_1.jpg", "F:UNKNOWN"),
    ("faction_leader_2.jpg", "F:UNKNOWN"),
    ("time_play_icon.jpg", "B:RUNTIME_UTILITY"),
    ("time_pause_icon.jpg", "B:RUNTIME_UTILITY"),
    ("time_ffw_icon.jpg", "B:RUNTIME_UTILITY"),
]

# Deprecated patterns
def classify_file(stem: str, dir_kind: str):
    name = stem + ".jpg"  # try both
    # Deprecated ammo
    if stem.startswith("ammo_deprecated_"):
        return "E:DEPRECATED", "Deprecated ammo, mirror of active variant"
    if stem.startswith("helmet_deprecated"):
        return "E:DEPRECATED", "Deprecated helmet variant"
    # Match known patterns
    for fn, cls in PLACEHOLDER_PATTERNS:
        if fn.startswith(stem + "."):
            return cls, "Matched known placeholder pattern"
    # Heuristic: cargo-style numbered placeholder
    if stem in ("enc_a", "enc_b", "enc_x", "enc_a.png", "enc_b.png", "enc_x.png"):
        return "F:UNKNOWN", "Unclassified encounter placeholder"
    return None


# Build triage
triage = defaultdict(list)
for r in active:
    if not isinstance(r, dict):
        continue
    stem = r.get("stem") or r.get("asset_id", "")
    fp = r.get("full_path") or r.get("file_path") or r.get("path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    if not stem or not fp or "/" not in fp:
        continue
    head, _, tail = fp.partition("/")
    cls = classify_file(stem, head)
    if cls:
        triage[cls[0]].append({"stem": stem, "file_path": fp, "head": head,
                                 "rationale": cls[1]})

# Build markdown
out = REPO / "docs/visual/PLACEHOLDER_TRIAGE.md"
md = []
md.append("# ASHFALL — Placeholder Triage\n\n")
md.append("Phase 14R — per-placeholder classification.\n\n")
md.append(f"Categorisation key:\n\n")
md.append("- `A.ACTIVE_PRODUCTION_PLACEHOLDER` — drives a runtime render path; candidate for replacement.\n")
md.append("- `B.GENERIC_RUNTIME_UTILITY` — referenced by chrome code paths (`icon = ...pattern`). Keep, do not replace.\n")
md.append("- `C.TEMPLATE_REFERENCE` — used by tools (catalog generator, Figma, exporter). Keep.\n")
md.append("- `D.DEBUG` — debug/test artefacts. Move out-of-runtime in a later cleanup phase.\n")
md.append("- `E.DEPRECATED` — historical/legacy. Delete after a separately verified cleanup pass.\n")
md.append("- `F.UNKNOWN` — review needed.\n\n")
md.append(f"## Counts\n\n| Bucket | Count |\n|---|---|\n")
for cat in ("A.ACTIVE_PRODUCTION_PLACEHOLDER", "B.GENERIC_RUNTIME_UTILITY",
            "C.TEMPLATE_REFERENCE", "D.DEBUG", "E.DEPRECATED", "F.UNKNOWN"):
    md.append(f"| `{cat}` | {len(triage.get(cat, []))} |\n")
md.append("\n")

# Active production placeholders are the only ones that become art-replacement targets.
active_count = len(triage.get("A.ACTIVE_PRODUCTION_PLACEHOLDER", []))
md.append(f"## Active production placeholders → eligible for replacement queue\n\n")
md.append(f"There are {active_count} placeholder files in the active class. They are NOT auto-replaced; they are documented as candidates. A future art-replacement batch may target them on a per-ID basis.\n\n")
md.append("| Stem | Path | Rationale |\n|---|---|---|\n")
for r in triage.get("A.ACTIVE_PRODUCTION_PLACEHOLDER", []):
    md.append(f"| `{r['stem']}` | `{r['file_path']}` | active placeholder |\n")
md.append("\n")

for cat in ("B.GENERIC_RUNTIME_UTILITY", "C.TEMPLATE_REFERENCE",
            "D.DEBUG", "E.DEPRECATED", "F.UNKNOWN"):
    md.append(f"## {cat}\n\n")
    md.append("| Stem | Path | Rationale |\n|---|---|---|\n")
    for r in triage.get(cat, []):
        md.append(f"| `{r['stem']}` | `{r['file_path']}` | {r['rationale']} |\n")
    md.append("\n")

out.write_text("".join(md))
print(f"→ wrote {out.name}")
total = sum(len(v) for v in triage.values())
print(f"triaged: {total}, by category: { {k: len(v) for k, v in triage.items()} }")
