#!/usr/bin/env python3
"""Phase 14V — Production-art generation ledger.

This script produces a ledger entry for every candidate row in the production
manifest. Each entry records:
  content_id, family, prompt_id, model hint, staged file, QA result,
  final canonical file, wiring result, runtime verification.

Phase 14 deliberately does NOT call any image-generation API in this turn
(arkcli +gen requires auth not present in the sandbox). The ledger is
populated from existing on-disk state plus a status column that an
executor can fill in subsequent phases.

Output: docs/visual/PRODUCTION_ART_GENERATION_LEDGER.md
        docs/visual/PRODUCTION_ART_GENERATION_LEDGER.json
"""
import json
from pathlib import Path
from collections import defaultdict, Counter

REPO = Path("/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War")
STAGING = REPO / "assets/_staging_generated"
PROMPTS = REPO / "docs/visual/generated_prompts"
WM = json.load(open(REPO / "docs/visual/WIRING_MATRIX.json"))
GEN = json.load(open(REPO / "docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json"))

# Existing canonical files (for ledger entry)
existing_files = set()
M_OBJ = json.load(open(REPO / "docs/visual/visual_asset_manifest.json"))
for r in (M_OBJ if isinstance(M_OBJ, list) else M_OBJ["active_assets"]):
    if not isinstance(r, dict): continue
    fp = r.get("full_path") or r.get("file_path") or ""
    if fp.startswith("/"):
        try:
            fp = str(Path(fp).relative_to(REPO))
        except ValueError:
            pass
    if fp:
        existing_files.add(fp)

# Process each manifest row
ledger = []
counts = Counter()
for row in GEN:
    cid = row["content_id"]
    fam = row["visual_family"]
    target = (row.get("target_directory") or "assets/art/") + (row.get("target_filename") or f"{cid}.jpg")
    prompt_path = PROMPTS / f"{cid}.json"
    count_model = (row.get("model_hint") or "auto")  # placeholder
    # Status
    if row.get("generation_status") == "SKIP_REFERENCE_ONLY":
        status = "SKIP_REFERENCE_ONLY"
        prompt_status = "N/A"
        staging_file = None
        canonical_file = None
    elif target in existing_files:
        status = "SKIP_EXISTING"
        prompt_status = "ready"
        staging_file = None
        canonical_file = target
    else:
        status = "PENDING"
        prompt_status = "ready"
        staging_file = None
        canonical_file = target  # planned

    ledger.append({
        "content_id": cid,
        "family": fam,
        "subfamily": row.get("subfamily", "Other"),
        "prompt_id": cid,
        "model_hint": count_model,
        "prompt_file": str(prompt_path.relative_to(REPO)) if prompt_path.exists() else None,
        "prompt_status": prompt_status,
        "staged_file": staging_file,
        "qa_result": "NOT_RUN",
        "regeneration_count": 0,
        "final_canonical_file": canonical_file,
        "wiring_result": (row.get("wiring_status") if row.get("wiring_status") else "DOCUMENTED_MISSING"),
        "runtime_verification": row.get("runtime_status", "NEVER_BOUND"),
    })
    counts[status] += 1

# JSON ledger
out_json = REPO / "docs/visual/PRODUCTION_ART_GENERATION_LEDGER.json"
out_json.write_text(json.dumps(ledger, indent=1))
print(f"→ wrote {out_json.name} ({len(ledger)} entries)")

# Markdown summary
md = []
md.append("# ASHFALL — Phase 14 Production-Art Generation Ledger\n\n")
md.append(f"Batched ledger for the {len(ledger)} candidate rows.\n\n")
md.append("## Status counts at ledger-start\n\n")
md.append("| Status | Count |\n|---|---|\n")
for s, n in sorted(counts.items(), key=lambda x: -x[1]):
    md.append(f"| `{s}` | {n} |\n")
md.append("\n")
md.append("## Generation status not implemented in this run\n\n")
md.append("Image generation API was not invoked. The structural pipeline is in place:\n\n")
md.append("- Each candidate has a generated prompt template in `docs/visual/generated_prompts/`.\n")
md.append("- Each candidate has a staging path `assets/_staging_generated/<family>/<id>.{jpg|png}` ready to accept output.\n")
md.append("- QA harness is ready (`tools/production_qa.py`).\n")
md.append("- Wiring re-trace is ready (`tools/visual_wiring_postfix.py`).\n\n")
md.append("A subsequent exec with the +gen API authenticated will:\n1. Take the priority-ordered manifest as input.\n")
md.append("2. For each row in Batches 1..N (recommended 30 / 50 / 100 / …):\n")
md.append("   - read `docs/visual/generated_prompts/<content_id>.json`\n")
md.append("   - run QA before promotion\n")
md.append("   - if PASS, write to `assets/art/<family>/<canonical-filename>`\n")
md.append("   - mark in this ledger\n")
md.append("3. Re-run the wiring trace to confirm zero MISSING rows for that batch.\n\n")
md.append("## Re-run instructions (Phase 15 prep)\n\n```bash\n")
md.append("# Dry-run with arkcli:\n")
md.append("arkcli +gen \"<prompt-text>\" --model seedream-3 --output-format jpeg --ratio 1:1\n")
md.append("# Then route through QA → promotion:\n")
md.append("python3 tools/production_qa.py\n")
md.append("python3 tools/visual_wiring_postfix.py\n")
md.append("```\n")

out_md = REPO / "docs/visual/PRODUCTION_ART_GENERATION_LEDGER.md"
out_md.write_text("".join(md))
print(f"→ wrote {out_md.name}")
