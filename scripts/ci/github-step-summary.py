#!/usr/bin/env python3
"""
github-step-summary.py — GitHub Actions Step Summary & PR Reporter

Ingests 'gate-results.json' (or run-gates output) and writes a clean, formatted
GitHub Flavored Markdown report to $GITHUB_STEP_SUMMARY or stdout.

Features:
  1. Header metrics card with overall pass/fail status and execution time.
  2. Category-organized gate status table with emoji badges and timing.
  3. Collapsible <details><summary> diagnostics for any failed gates.
  4. --preview mode for local CLI inspection.
  5. Zero trailing whitespace hygiene.

Usage:
  python3 scripts/ci/github-step-summary.py --input build/reports/gate-results.json
  python3 scripts/ci/github-step-summary.py --preview
"""

import sys
import json
import pathlib
import argparse
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent


def generate_markdown(data: dict) -> str:
    total_gates = data.get("total_gates", 0)
    passed_count = data.get("passed_count", 0)
    failed_count = data.get("failed_count", 0)
    duration = data.get("duration_seconds", 0.0)
    tier = data.get("tier", "FAST").upper()
    ts = data.get("timestamp", datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%SZ"))
    gates = data.get("gates", [])

    is_success = failed_count == 0

    lines = []
    lines.append(f"# {'✅' if is_success else '❌'} ASHFALL Canonical CI Verification Summary")
    lines.append("")
    lines.append(f"**Target Tier:** `{tier}` &nbsp;|&nbsp; **Status:** **{'ALL GATES PASSED' if is_success else f'{failed_count} GATE(S) FAILED'}** &nbsp;|&nbsp; **Duration:** `{duration:.2f}s` &nbsp;|&nbsp; **Run Date:** `{ts}`")
    lines.append("")

    # Summary table
    lines.append("## Verification Gates Status")
    lines.append("")
    lines.append("| # | Status | Gate ID | Name | Category | Duration |")
    lines.append("|---|---|---|---|---|---|")

    categories = {}
    for i, g in enumerate(gates, 1):
        gid = g.get("gate_id", "unknown")
        name = g.get("name", "")
        cat = g.get("category", "General")
        status = g.get("status", "PASS")
        dur = f"{g.get('duration_seconds', 0.0):.2f}s"
        badge = "✅ PASS" if status == "PASS" else "❌ FAIL"
        lines.append(f"| {i} | {badge} | `{gid}` | {name} | {cat} | {dur} |")

        if cat not in categories:
            categories[cat] = {"total": 0, "passed": 0}
        categories[cat]["total"] += 1
        if status == "PASS":
            categories[cat]["passed"] += 1

    lines.append("")

    # Category breakdown
    lines.append("## Category Breakdown")
    lines.append("")
    lines.append("| Category | Passed / Total | Health |")
    lines.append("|---|---|---|")
    for cat, counts in sorted(categories.items()):
        p = counts["passed"]
        t = counts["total"]
        health = "✅ 100%" if p == t else f"❌ {int(p/t*100)}%"
        lines.append(f"| **{cat}** | `{p}/{t}` | {health} |")
    lines.append("")

    # Failure Diagnostics
    failed_gates = [g for g in gates if g.get("status") != "PASS"]
    if failed_gates:
        lines.append("## ❌ Failure Diagnostics")
        lines.append("")
        for fg in failed_gates:
            gid = fg.get("gate_id", "unknown")
            name = fg.get("name", "")
            cmd = fg.get("command", "")
            err = fg.get("error_reason", "Command exited with error.")
            output = fg.get("output", "").strip()

            lines.append(f"<details><summary><b>❌ {gid}</b> — {name} ({err})</summary>")
            lines.append("")
            lines.append(f"**Command:** `{cmd}`")
            lines.append("")
            lines.append("```text")
            lines.append(output if output else "No output captured.")
            lines.append("```")
            lines.append("</details>")
            lines.append("")

    return "\n".join(lines) + "\n"


def create_mock_report() -> dict:
    return {
        "timestamp": datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%SZ"),
        "tier": "FAST",
        "total_gates": 38,
        "passed_count": 38,
        "failed_count": 0,
        "duration_seconds": 50.53,
        "gates": [
            {"gate_id": "whitespace_hygiene", "name": "Trailing Whitespace & Hygiene Gate", "category": "Code Hygiene", "status": "PASS", "duration_seconds": 0.09},
            {"gate_id": "json_schema_policy", "name": "StreamingAssets JSON Syntax & Schema Policy", "category": "Code Hygiene", "status": "PASS", "duration_seconds": 0.25},
            {"gate_id": "build_core_tests", "name": "Build Ashfall.Core.Tests (net9.0)", "category": "Build & Tests", "status": "PASS", "duration_seconds": 1.70},
            {"gate_id": "test_core_suite", "name": "Execute Core Unit & Determinism Test Suite (xUnit)", "category": "Build & Tests", "status": "PASS", "duration_seconds": 18.57},
            {"gate_id": "build_godot_host", "name": "Build Godot Host Application (Ashfall.csproj net8.0)", "category": "Build & Tests", "status": "PASS", "duration_seconds": 1.69},
            {"gate_id": "godot_import", "name": "Godot Resource Cache Import", "category": "Host Selftests", "status": "PASS", "duration_seconds": 5.76},
            {"gate_id": "data_integrity", "name": "Data Authority Integrity Gate", "category": "Host Selftests", "status": "PASS", "duration_seconds": 1.31},
            {"gate_id": "bridge_removal", "name": "Bridge Shim Removal Confirmation Gate", "category": "Host Selftests", "status": "PASS", "duration_seconds": 0.61},
            {"gate_id": "asset_registry", "name": "Asset Registry Resolution Gate", "category": "Host Selftests", "status": "PASS", "duration_seconds": 0.74},
            {"gate_id": "player_panels_uitest", "name": "Player UI Panels Construction & Binding", "category": "Host Selftests", "status": "PASS", "duration_seconds": 2.91},
            {"gate_id": "ui_panel_contracts_test", "name": "UI Scene Unique Node Contract Test Gate (xUnit)", "category": "Build & Tests", "status": "PASS", "duration_seconds": 1.93},
        ]
    }


def main():
    parser = argparse.ArgumentParser(description="Generate GitHub Step Summary markdown from gate results.")
    parser.add_argument("--input", "-i", help="Path to gate-results.json file")
    parser.add_argument("--output", "-o", help="Path to output markdown file (default: stdout)")
    parser.add_argument("--preview", action="store_true", help="Print a preview summary using mock/live data")

    args = parser.parse_args()

    if args.preview or not args.input:
        if args.input and pathlib.Path(args.input).is_file():
            data = json.loads(pathlib.Path(args.input).read_text(encoding="utf-8"))
        else:
            data = create_mock_report()
    else:
        input_path = pathlib.Path(args.input)
        if not input_path.is_file():
            print(f"Error: input file {input_path} not found.", file=sys.stderr)
            sys.exit(1)
        data = json.loads(input_path.read_text(encoding="utf-8"))

    md = generate_markdown(data)

    if args.output:
        out_path = pathlib.Path(args.output)
        out_path.parent.mkdir(parents=True, exist_ok=True)
        out_path.write_text(md, encoding="utf-8")
        print(f"Wrote summary to {out_path}")
    else:
        print(md)


if __name__ == "__main__":
    main()
