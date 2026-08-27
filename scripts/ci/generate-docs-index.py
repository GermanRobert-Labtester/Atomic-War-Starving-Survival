#!/usr/bin/env python3
"""
generate-docs-index.py — Documentation Index Generator & Drift Checker

Indexes all markdown documentation across root, docs/, deprecated_audits/, and
scripts/maintenance/, classifying each document by status:
  - CURRENT:    Authoritative, living documentation matching Godot architecture.
  - HISTORICAL: Forensic reports, phase execution logs, migration milestones.
  - GENERATED:  Programmatically generated matrices, CLI references, or AI logs.
  - DEPRECATED: Archived or superseded pre-migration / duplicate audits.

Usage:
  python3 scripts/ci/generate-docs-index.py           # Regenerates docs/INDEX.md
  python3 scripts/ci/generate-docs-index.py --check   # Verifies docs/INDEX.md is in sync
"""

import argparse
import collections
import datetime
import os
import pathlib
import re
import sys

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
INDEX_FILE = REPO_ROOT / "docs" / "INDEX.md"

def get_doc_files():
    docs = []
    for p in sorted(REPO_ROOT.rglob("*.md")):
        rel = p.relative_to(REPO_ROOT).as_posix()
        # Exclude hidden directories, subagent configs, and node_modules
        if any(part.startswith(".") for part in p.relative_to(REPO_ROOT).parts):
            continue
        if "node_modules/" in rel or "obj/" in rel or "bin/" in rel or "build/" in rel or "artifacts/" in rel:
            continue
        if rel == "docs/INDEX.md":
            continue
        docs.append(p)
    return docs

def classify_doc(rel_path: str, content: str, title: str):
    """Classifies a document into (Status, Category, Summary)."""
    # 1. Status classification
    status = "CURRENT"
    c_upper = content.upper()
    r_lower = rel_path.lower()

    if "deprecated_audits" in r_lower or "junk_" in r_lower or "historical" in r_lower or "repo_review_report.md" in r_lower:
        status = "HISTORICAL"
    elif "SAVE_STORE_CONTRACT_MATRIX.md" in rel_path or "CLI_COMMANDS_REFERENCE" in rel_path or "STITCH_GENERATED_UI_INVENTORY" in rel_path:
        status = "GENERATED"
    elif "nano banana pro" in r_lower or "prompt" in r_lower or "asset_manifest" in r_lower:
        status = "GENERATED"
    elif "forensic" in r_lower or "phase_log" in r_lower or "audit_report" in r_lower or "deep_code_audit" in r_lower or "execution_log" in r_lower:
        status = "HISTORICAL"
    elif "STATUS: HISTORICAL" in content or "STATUS:  HISTORICAL" in content:
        status = "HISTORICAL"

    # Specific overrides
    if rel_path in ["REPO_REVIEW_REPORT.md", "AUDIT_REPORT.md", "COMPREHENSIVE_GAME_AUDIT.md", "DEEP_CODE_AUDIT_2026-08-23.md"]:
        status = "HISTORICAL"
    if "scripts/maintenance/README.md" == rel_path:
        status = "CURRENT"

    # 2. Category classification
    if rel_path in ["AGENTS.md", "README.md"] or rel_path.startswith("docs/architecture/"):
        category = "1. Living System Architecture & Governance"
    elif rel_path in ["docs/CI.md"] or rel_path.startswith("docs/ci/"):
        category = "2. CI, Fast-Tier Gates & Verification"
    elif rel_path.startswith("docs/saves/") or "SAVE" in rel_path:
        category = "3. Save Systems & State Architecture"
    elif rel_path.startswith("docs/expansions/") or "EXPANSION" in rel_path:
        category = "4. Expansions (01–10 Master Plans & Context)"
    elif rel_path.startswith("docs/ui/") or rel_path.startswith("docs/visual/") or "UI" in rel_path:
        category = "5. UI, UX & Visual Systems"
    elif rel_path.startswith("docs/lore/") or rel_path.startswith("docs/narrative/"):
        category = "6. Lore, Gazetteer & World Design"
    elif rel_path.startswith("docs/forensics/") or rel_path.startswith("docs/audit/") or rel_path.startswith("docs/debug/") or rel_path.startswith("docs/qa/") or rel_path.startswith("docs/superpowers/"):
        category = "7. Forensics, Phase Logs & System Audits"
    elif rel_path.startswith("docs/ai-art/") or rel_path.startswith("prompt_assets/"):
        category = "8. AI Art & Prompt Generation Catalogs"
    elif rel_path.startswith("scripts/maintenance/") or rel_path.startswith("docs/skills/"):
        category = "9. Maintenance & Developer Tooling"
    elif rel_path.startswith("deprecated_audits/") or rel_path.startswith("docs/deprecated_audits/"):
        category = "10. Quarantined & Historical Audits"
    else:
        category = "11. General Project Guides & Summaries"

    # 3. Summary extraction
    summary = ""
    for line in content.splitlines():
        line = line.strip()
        if not line or line.startswith("#") or line.startswith("---") or line.startswith(">") or line.startswith("|") or line.startswith("<!--"):
            continue
        if len(line) > 15:
            summary = line
            break
    if not summary:
        summary = title
    if len(summary) > 120:
        summary = summary[:117] + "..."

    return status, category, summary

def find_duplicate_generations(docs):
    by_filename = collections.defaultdict(list)
    for doc in docs:
        rel_path = doc.relative_to(REPO_ROOT).as_posix()
        by_filename[doc.name].append(rel_path)

    duplicates = {k: v for k, v in by_filename.items() if len(v) > 1 and k != "README.md"}
    return duplicates

def generate_index_markdown(docs, verified_date):
    categorized = {}
    status_counts = {"CURRENT": 0, "HISTORICAL": 0, "GENERATED": 0}

    for doc in docs:
        rel_path = doc.relative_to(REPO_ROOT).as_posix()
        content = doc.read_text(encoding="utf-8", errors="ignore")

        # Extract title
        title = ""
        for line in content.splitlines():
            line = line.strip()
            if line.startswith("# "):
                title = line.lstrip("# ").strip()
                break
        if not title:
            title = doc.stem.replace("_", " ").title()

        status, category, summary = classify_doc(rel_path, content, title)
        status_counts[status] = status_counts.get(status, 0) + 1

        if category not in categorized:
            categorized[category] = []
        categorized[category].append({
            "path": rel_path,
            "title": title,
            "status": status,
            "summary": summary
        })

    total_docs = len(docs)
    duplicates = find_duplicate_generations(docs)

    lines = [
        "# ASHFALL — Master Documentation Index",
        "",
        f"**Authoritative Engine:** Godot 4.7+ (.NET / C#) | **Status:** Migration Complete (Unity host removed)",
        f"**Total Indexed Documents:** {total_docs} | **Last Verified:** {verified_date}",
        "",
        "| Status Badge | Meaning | Corpus Count |",
        "|---|---|---|",
        f"| 🟢 `CURRENT` | Authoritative, active living documentation matching Godot architecture | {status_counts.get('CURRENT', 0)} |",
        f"| 🟡 `HISTORICAL` | Forensic reports, phase logs, and historical postmortems (retained for record) | {status_counts.get('HISTORICAL', 0)} |",
        f"| 🔵 `GENERATED` | Programmatically generated or updated catalogs (contracts, CLI reference, AI logs) | {status_counts.get('GENERATED', 0)} |",
        "",
        "---",
        "",
        "## Duplicate & Near-Duplicate Audit Generations",
        "",
        "The following documents share identical or near-identical filenames across root, `docs/`, and `deprecated_audits/`. Use the canonical location listed below:",
        "",
        "| Filename | Copies / Locations | Canonical Location | Notes |",
        "|---|---|---|---|"
    ]

    for name, paths in sorted(duplicates.items(), key=lambda x: len(x[1]), reverse=True):
        paths_str = "<br>".join(f"`{p}`" for p in paths)
        # Select canonical location
        canonical = [p for p in paths if not p.startswith("deprecated_audits") and not p.startswith("docs/deprecated_audits")]
        canonical_loc = f"`{canonical[0]}`" if canonical else f"`{paths[0]}`"
        notes = "Historical audit duplicate" if any("audit" in p.lower() or "junk" in p.lower() for p in paths) else "Root vs docs mirror"
        lines.append(f"| `{name}` | {paths_str} | {canonical_loc} | {notes} |")

    lines.extend([
        "",
        "---",
        ""
    ])

    for cat_name in sorted(categorized.keys()):
        items = categorized[cat_name]
        lines.append(f"## {cat_name} ({len(items)} documents)")
        lines.append("")
        lines.append("| Status | Document | Title / Summary |")
        lines.append("|---|---|---|")

        for item in sorted(items, key=lambda x: (x['status'] != 'CURRENT', x['status'] != 'GENERATED', x['path'])):
            badge = "🟢 `CURRENT`" if item["status"] == "CURRENT" else ("🔵 `GENERATED`" if item["status"] == "GENERATED" else "🟡 `HISTORICAL`")
            rel_to_index = os.path.relpath(REPO_ROOT / item['path'], INDEX_FILE.parent).replace('\\', '/')
            doc_link = f"[`{item['path']}`]({rel_to_index})"
            title_summary = f"**{item['title']}** — {item['summary']}" if item['title'] != item['summary'] else f"**{item['title']}**"
            title_summary = title_summary.replace("|", "\\|")
            lines.append(f"| {badge} | {doc_link} | {title_summary} |")

        lines.append("")

    return "\n".join(lines)

def main():
    check_mode = "--check" in sys.argv
    docs = get_doc_files()

    verified_date = datetime.date.today().isoformat()
    if check_mode and INDEX_FILE.exists():
        current_text = INDEX_FILE.read_text(encoding="utf-8")
        m = re.search(r"\*\*Last Verified:\*\*\s+(\d{4}-\d{2}-\d{2})", current_text)
        if m:
            verified_date = m.group(1)

    rendered = generate_index_markdown(docs, verified_date)

    if check_mode:
        if not INDEX_FILE.exists():
            print(f"FAIL: {INDEX_FILE} does not exist. Run: python3 scripts/ci/generate-docs-index.py", file=sys.stderr)
            sys.exit(1)
        current = INDEX_FILE.read_text(encoding="utf-8")
        if current.strip() != rendered.strip():
            print(f"FAIL: {INDEX_FILE} is out of sync with repository markdown files.", file=sys.stderr)
            print("Run: python3 scripts/ci/generate-docs-index.py && git commit", file=sys.stderr)
            sys.exit(1)
        else:
            print(f"OK: Master docs index is up to date ({len(docs)} documents verified).")
            sys.exit(0)
    else:
        INDEX_FILE.parent.mkdir(parents=True, exist_ok=True)
        INDEX_FILE.write_text(rendered, encoding="utf-8")
        print(f"Wrote {INDEX_FILE} ({len(docs)} documents indexed).")

if __name__ == "__main__":
    main()
