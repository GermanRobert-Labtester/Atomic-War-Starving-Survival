#!/usr/bin/env python3
"""scripts/ci/verify-plan-freshness.py
Plan 53 / E1D: Premise freshness, dead-reference, and capability-claim verifier.

Evaluates references in ASHFALL integration plans:
- Identifies missing repository paths (MISSING_PATH)
- Detects line number range overruns (MISSING_LINE_RANGE)
- Suggests renamed candidates when a unique file with the same basename exists (RENAMED_CANDIDATE)
- Flags ambiguous references when multiple candidate files match (AMBIGUOUS)
- Distinguishes external URLs and token references (EXTERNAL)
"""

from __future__ import annotations

import argparse
import json
import os
import sys
from pathlib import Path
from typing import Any

SCRIPT_DIR = Path(__file__).resolve().parent
REPO_ROOT = SCRIPT_DIR.parent.parent
sys.path.insert(0, str(SCRIPT_DIR))

from plan_corpus_lib import (  # noqa: E402
    classify_reference,
    extract_references,
    analyze_references,
    enumerate_paths,
    parse_plan,
    parse_front_matter,
    ReferenceVerdict,
)


CONFIG_PATH = SCRIPT_DIR / "plan_governance_config.json"


def load_config(path: Path) -> dict[str, Any]:
    with path.open(encoding="utf-8") as handle:
        return json.load(handle)


def verify_plan_references(plan_path: Path, repo_root: Path) -> tuple[list[dict[str, str]], str, dict[str, int]]:
    text = plan_path.read_text(encoding="utf-8", errors="replace")
    front_matter, _, _ = parse_front_matter(text)
    is_legacy = front_matter is None or not front_matter.get("PLAN_ID")
    verdicts, health = analyze_references(text, repo_root, is_legacy)

    summary = {"OK": 0, "MISSING_PATH": 0, "MISSING_LINE_RANGE": 0, "RENAMED_CANDIDATE": 0, "AMBIGUOUS": 0, "EXTERNAL": 0}
    for v in verdicts:
        status = v.get("status", "EXTERNAL")
        if status in summary:
            summary[status] += 1

    return verdicts, health, summary


def run_self_test(repo_root: Path) -> int:
    print("Running verify-plan-freshness self-test...")
    # Test 1: Real existing path
    v1 = classify_reference("Assets/Ashfall.Core/ActionResult.cs", repo_root)
    assert v1.status == "OK", f"Expected OK, got {v1.status}"

    # Test 2: Nonexistent path
    v2 = classify_reference("Assets/Ashfall.Core/CompletelyFictionalSystem123.cs", repo_root)
    assert v2.status == "MISSING_PATH", f"Expected MISSING_PATH, got {v2.status}"

    # Test 3: External reference
    v3 = classify_reference("https://example.com/spec", repo_root)
    assert v3.status == "EXTERNAL", f"Expected EXTERNAL, got {v3.status}"

    # Test 4: Line count bounds
    v4 = classify_reference("Assets/Ashfall.Core/ActionResult.cs:999999", repo_root)
    assert v4.status == "MISSING_LINE_RANGE", f"Expected MISSING_LINE_RANGE, got {v4.status}"

    print("verify-plan-freshness self-test: PASS")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description="Verify plan premise freshness and reference health.")
    parser.add_argument("--check", action="store_true", help="Run reference freshness check across plans.")
    parser.add_argument("--plan", type=str, help="Check references for a single plan file or path.")
    parser.add_argument("--summary", action="store_true", help="Display aggregate reference status counts.")
    parser.add_argument("--json", action="store_true", help="Output JSON results.")
    parser.add_argument("--self-test", action="store_true", help="Run built-in self test and exit.")
    args = parser.parse_args()

    if args.self_test:
        return run_self_test(REPO_ROOT)

    config = load_config(CONFIG_PATH) if CONFIG_PATH.exists() else {}

    if args.plan:
        target = Path(args.plan)
        if not target.is_absolute():
            target = REPO_ROOT / target
        if not target.is_file():
            print(f"Error: Plan file not found: {target}", file=sys.stderr)
            return 1

        verdicts, health, counts = verify_plan_references(target, REPO_ROOT)
        if args.json:
            print(json.dumps({"plan": str(target), "health": health, "counts": counts, "verdicts": verdicts}, indent=2))
        else:
            print(f"Plan: {target.relative_to(REPO_ROOT)}")
            print(f"Health: {health}")
            print(f"Counts: OK={counts['OK']}, Missing={counts['MISSING_PATH']}, LineRangeOverrun={counts['MISSING_LINE_RANGE']}, RenamedCandidates={counts['RENAMED_CANDIDATE']}, Ambiguous={counts['AMBIGUOUS']}")
            if counts["MISSING_PATH"] > 0 or counts["MISSING_LINE_RANGE"] > 0:
                print("\nStale or Missing References:")
                for v in verdicts:
                    if v["status"] in {"MISSING_PATH", "MISSING_LINE_RANGE"}:
                        print(f"  - [{v['status']}] {v['reference']} -> {v['detail']}")
        return 0

    paths = enumerate_paths(REPO_ROOT, config)
    total_plans = len(paths)
    aggregate_counts = {"OK": 0, "MISSING_PATH": 0, "MISSING_LINE_RANGE": 0, "RENAMED_CANDIDATE": 0, "AMBIGUOUS": 0}
    health_counts = {"OK": 0, "WARN": 0, "MISSING": 0}

    plan_reports = []
    for path in paths:
        verdicts, health, counts = verify_plan_references(path, REPO_ROOT)
        health_counts[health] = health_counts.get(health, 0) + 1
        for k in aggregate_counts:
            aggregate_counts[k] += counts.get(k, 0)
        plan_reports.append({
            "path": str(path.relative_to(REPO_ROOT)),
            "health": health,
            "counts": counts,
            "verdicts_count": len(verdicts)
        })

    if args.json:
        print(json.dumps({
            "total_plans": total_plans,
            "health_summary": health_counts,
            "reference_counts": aggregate_counts,
            "plans": plan_reports
        }, indent=2))
        return 0

    print("==================================================")
    print("ASHFALL PLAN REFERENCE FRESHNESS REPORT (Plan 53 / E1D)")
    print("==================================================")
    print(f"Total plans audited: {total_plans}")
    print(f"Plan health breakdown:")
    print(f"  OK (all references valid)   : {health_counts['OK']}")
    print(f"  WARN (legacy/minor alerts)  : {health_counts['WARN']}")
    print(f"  MISSING (active broken refs): {health_counts['MISSING']}")
    print(f"Aggregate reference status counts:")
    print(f"  OK                          : {aggregate_counts['OK']}")
    print(f"  MISSING_PATH                : {aggregate_counts['MISSING_PATH']}")
    print(f"  MISSING_LINE_RANGE          : {aggregate_counts['MISSING_LINE_RANGE']}")
    print(f"  RENAMED_CANDIDATE           : {aggregate_counts['RENAMED_CANDIDATE']}")
    print(f"  AMBIGUOUS                   : {aggregate_counts['AMBIGUOUS']}")
    print("--------------------------------------------------")
    print("Freshness verification complete.")
    print("==================================================")
    return 0


if __name__ == "__main__":
    sys.exit(main())
