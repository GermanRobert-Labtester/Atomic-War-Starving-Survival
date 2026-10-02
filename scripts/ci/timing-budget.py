#!/usr/bin/env python3
"""timing-budget.py — reference-time budget checker for ASHFALL builds/gates.

Agents should aim for a known duration range, not let a build or gate silently
run 2-3x longer than usual (which usually means an undetected error, a stale
build, or a deadlocked subprocess). This compares measured durations against the
committed reference durations in `docs/ci/TIMING_BASELINE.json` and flags any
run that is MORE than the tolerance ratio over reference (default 1.30 = 30%).
Rule: a stage running **30% or less** over its reference is tolerable; **more
than 30%** means stop, investigate, and optimize the test — do not simply raise
its budget.

Usage:
  # Compare a gate report produced by run-gates.py --report-json
  python3 scripts/ci/timing-budget.py check --measured build/reports/gates.json

  # Compare a single ad-hoc stage (e.g. a build)
  python3 scripts/ci/timing-budget.py check --key build_core_tests --seconds 84.2

  # Compare a single stage's peak RSS (MB) against its memory budget
  python3 scripts/ci/timing-budget.py check --key build_godot_host --rss 2500

  # Refresh references from a real (quiet-system) run
  python3 scripts/ci/timing-budget.py update --measured build/reports/gates.json
  python3 scripts/ci/timing-budget.py update --key build_core_tests --seconds 56.0

Exit codes: 0 = every measured stage within budget; 1 = at least one SUSPICIOUS.
Stages with no baseline entry are reported NO-REFERENCE and never fail the check
(they are how you discover which references still need recording).
"""

from __future__ import annotations

import argparse
import datetime
import json
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
DEFAULT_BASELINE = REPO_ROOT / "docs" / "ci" / "TIMING_BASELINE.json"
DEFAULT_TOLERANCE = 1.30


def load_json(path: Path):
    with path.open("r", encoding="utf-8") as handle:
        return json.load(handle)


def collect_measured(measured_path: Path) -> dict:
    """Return {key: seconds} from a run-gates report, a result list, or a map."""
    data = load_json(measured_path)
    out: dict = {}

    def take(item):
        if not isinstance(item, dict):
            return
        key = item.get("gate_id") or item.get("id") or item.get("name")
        secs = item.get("duration", item.get("duration_seconds"))
        if key and isinstance(secs, (int, float)):
            out[key] = float(secs)

    if isinstance(data, dict) and isinstance(data.get("results"), list):
        for item in data["results"]:
            take(item)
        return out
    if isinstance(data, list):
        for item in data:
            take(item)
        return out
    if isinstance(data, dict):
        for key, value in data.items():
            if isinstance(value, (int, float)):
                out[key] = float(value)
            elif isinstance(value, dict) and isinstance(value.get("seconds"), (int, float)):
                out[key] = float(value["seconds"])
    return out


def compare(budgets: dict, measured: dict, default_tol: float, override_tol):
    rows = []
    any_suspicious = False
    for key in sorted(measured):
        secs = measured[key]
        entry = budgets.get(key) or {}
        ref = entry.get("reference_seconds")
        tol = override_tol if override_tol is not None else entry.get("tolerance_ratio") or default_tol
        if not isinstance(ref, (int, float)) or ref <= 0:
            rows.append((key, None, secs, None, "NO-REFERENCE"))
            continue
        ratio = secs / float(ref)
        # "30% or less over" is tolerable; only strictly more than the tolerance
        # is a suspicion, so the boundary itself stays OK.
        verdict = "SUSPICIOUS" if ratio > float(tol) else "OK"
        if verdict == "SUSPICIOUS":
            any_suspicious = True
        rows.append((key, float(ref), secs, ratio, verdict))
    return rows, any_suspicious


def compare_rss(budgets: dict, measured_rss: dict, default_tol: float, override_tol):
    """Compare measured peak RSS (MB) to each entry's peak_rss_mb budget.

    Same non-fatal posture as the wall-clock check: a stage over its RSS budget
    beyond the tolerance is RSS_SUSPICIOUS (investigate, do not raise the budget).
    An entry with no peak_rss_mb is reported RSS_NO-BUDGET and never fails.
    """
    rows = []
    any_suspicious = False
    for key in sorted(measured_rss):
        mb = measured_rss[key]
        entry = budgets.get(key) or {}
        budget = entry.get("peak_rss_mb")
        tol = override_tol if override_tol is not None else entry.get("tolerance_ratio") or default_tol
        if not isinstance(budget, (int, float)) or budget <= 0:
            rows.append((key, None, mb, None, "RSS_NO-BUDGET"))
            continue
        ratio = mb / float(budget)
        verdict = "RSS_SUSPICIOUS" if ratio > float(tol) else "RSS_OK"
        if verdict == "RSS_SUSPICIOUS":
            any_suspicious = True
        rows.append((key, float(budget), mb, ratio, verdict))
    return rows, any_suspicious


def cmd_check(args) -> int:
    baseline = load_json(Path(args.baseline))
    budgets = baseline.get("budgets", {})
    default_tol = float(baseline.get("default_tolerance_ratio", DEFAULT_TOLERANCE))

    measured: dict = {}
    measured_rss: dict = {}
    if args.measured:
        measured.update(collect_measured(Path(args.measured)))
    if args.key and args.seconds is not None:
        measured[args.key] = float(args.seconds)
    if args.key and args.rss is not None:
        measured_rss[args.key] = float(args.rss)

    if not measured and not measured_rss:
        print("timing-budget: nothing measured (pass --measured and/or --key/--seconds/--rss).", file=sys.stderr)
        return 2

    rows, suspicious = compare(budgets, measured, default_tol, args.tolerance_ratio)
    rss_rows, rss_suspicious = compare_rss(budgets, measured_rss, default_tol, args.tolerance_ratio)
    suspicious = suspicious or rss_suspicious

    if args.json:
        payload = [
            {"kind": "wall", "key": k, "reference_seconds": ref, "measured_seconds": secs,
             "ratio": ratio, "verdict": verdict}
            for k, ref, secs, ratio, verdict in rows
        ] + [
            {"kind": "rss", "key": k, "budget_mb": budget, "measured_mb": mb,
             "ratio": ratio, "verdict": verdict}
            for k, budget, mb, ratio, verdict in rss_rows
        ]
        print(json.dumps(payload, indent=2))
    else:
        print(f"TIMING BUDGET (tolerance {default_tol:.2f}x)")
        for key, ref, secs, ratio, verdict in rows:
            if ref is None:
                print(f"  {verdict:<12} {key:<28} {secs:8.2f}s  (no reference recorded)")
            else:
                print(f"  {verdict:<12} {key:<28} {secs:8.2f}s / ref {ref:7.2f}s = {ratio:4.2f}x")
        for key, budget, mb, ratio, verdict in rss_rows:
            if budget is None:
                print(f"  {verdict:<16} {key:<28} {mb:8.0f}MB  (no RSS budget recorded)")
            else:
                print(f"  {verdict:<16} {key:<28} {mb:8.0f}MB / budget {budget:7.0f}MB = {ratio:4.2f}x")
        if suspicious:
            print("\nSUSPICIOUS: a stage ran MORE THAN the tolerance over its reference or RSS "
                  "budget. ≤30% over is tolerable; >30% means investigate (stale build, "
                  "deadlocked subprocess, undetected error, memory pressure) and optimize the "
                  "test — do not just raise the budget.")

    return 1 if suspicious else 0


def cmd_update(args) -> int:
    baseline_path = Path(args.baseline)
    baseline = load_json(baseline_path)
    budgets = baseline.setdefault("budgets", {})

    measured: dict = {}
    if args.measured:
        measured.update(collect_measured(Path(args.measured)))
    if args.key and args.seconds is not None:
        measured[args.key] = float(args.seconds)

    if not measured:
        print("timing-budget: nothing to update.", file=sys.stderr)
        return 2

    today = datetime.date.today().isoformat()
    for key, secs in sorted(measured.items()):
        budgets[key] = {
            "reference_seconds": round(secs, 3),
            "note": "recorded from a measured run; re-record on a quiet system if load made it unrepresentative",
            "recorded": today,
        }
        print(f"updated {key} -> reference {secs:.2f}s")

    baseline_path.write_text(json.dumps(baseline, indent=2) + "\n", encoding="utf-8")
    print(f"wrote {baseline_path}")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description="Reference-time budget checker.")
    parser.add_argument("--baseline", default=str(DEFAULT_BASELINE),
                        help="Path to TIMING_BASELINE.json")
    sub = parser.add_subparsers(dest="command", required=True)

    check = sub.add_parser("check", help="Compare measured durations to references")
    check.add_argument("--measured", default=None, help="run-gates --report-json output")
    check.add_argument("--key", default=None, help="Ad-hoc stage name")
    check.add_argument("--seconds", type=float, default=None, help="Ad-hoc measured seconds")
    check.add_argument("--rss", type=float, default=None,
                       help="Ad-hoc measured peak RSS in MB; compared to the entry's peak_rss_mb budget")
    check.add_argument("--tolerance-ratio", type=float, default=None,
                       help="Override the baseline tolerance (e.g. 1.30)")
    check.add_argument("--json", action="store_true", help="Machine-readable output")
    check.set_defaults(func=cmd_check)

    update = sub.add_parser("update", help="Refresh references from a measured run")
    update.add_argument("--measured", default=None, help="run-gates --report-json output")
    update.add_argument("--key", default=None, help="Ad-hoc stage name")
    update.add_argument("--seconds", type=float, default=None, help="Ad-hoc measured seconds")
    update.set_defaults(func=cmd_update)

    args = parser.parse_args()
    return args.func(args)


if __name__ == "__main__":
    sys.exit(main())