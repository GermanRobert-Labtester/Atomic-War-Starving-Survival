#!/usr/bin/env python3
"""perf-baseline-gate.py — ASHFALL runtime performance regression gate.

Compares a measured `PerformanceBaselineReport`
(`artifacts/performance/perf-baseline.json`, produced by the host
`--runtime-scale-selftest`) against the committed references in
`docs/ci/PERFORMANCE_BASELINE.json`.

Rules (same posture as `scripts/ci/timing-budget.py`):
  * a benchmark whose measured median exceeds its reference by more than the
    tolerance ratio is `SUSPICIOUS`;
  * a benchmark with no committed reference is `NO-REFERENCE` and never fails
    (it is how new references get discovered);
  * a missing measured artifact is reported and passes, because this is a
    non-critical performance gate that must not block a build it did not run in.

Usage:
  python3 scripts/ci/perf-baseline-gate.py check --measured artifacts/performance/perf-baseline.json
  python3 scripts/ci/perf-baseline-gate.py check --measured ... --warn-only
  python3 scripts/ci/perf-baseline-gate.py --check-only   # validate the baseline file

Exit codes: 0 = within budget / warn-only / no artifact; 1 = regression.
"""

from __future__ import annotations

import argparse
import datetime
import json
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
DEFAULT_BASELINE = REPO_ROOT / "docs" / "ci" / "PERFORMANCE_BASELINE.json"
DEFAULT_MEASURED = REPO_ROOT / "artifacts" / "performance" / "perf-baseline.json"
DEFAULT_TOLERANCE = 1.30
GATE = "PERF_BASELINE_GATE"


def load_json(path: Path):
    with path.open("r", encoding="utf-8") as handle:
        return json.load(handle)


def _get(mapping, *keys, default=None):
    if not isinstance(mapping, dict):
        return default
    for key in keys:
        if key in mapping:
            return mapping[key]
    return default


def collect_measured(path: Path) -> dict:
    """Return {benchmark_id: {median_ms, p95_ms, median_allocated_bytes}}."""
    data = load_json(path)
    if isinstance(data, dict):
        results = _get(data, "Results", "results", default=None)
        if results is None:
            results = [data]
    elif isinstance(data, list):
        results = data
    else:
        raise ValueError("measured artifact must be an object or list")

    out = {}
    for item in results:
        benchmark_id = _get(item, "BenchmarkId", "benchmarkId")
        if not benchmark_id:
            continue
        stats = _get(item, "Statistics", "statistics", default={}) or {}
        out[benchmark_id] = {
            "median_ms": float(_get(stats, "Median", "median", default=0.0) or 0.0),
            "p95_ms": float(_get(stats, "P95", "p95", default=0.0) or 0.0),
            "median_allocated_bytes": int(
                _get(stats, "MedianAllocatedBytes", "medianAllocatedBytes", default=0) or 0
            ),
        }
    return out


def validate_baseline(path: Path) -> list:
    problems = []
    if not path.exists():
        return [f"baseline file missing: {path}"]
    try:
        data = load_json(path)
    except Exception as exc:  # noqa: BLE001 - report parse failure verbatim
        return [f"baseline file is not valid JSON: {exc}"]
    if not isinstance(data, dict):
        return ["baseline root must be an object"]
    if not isinstance(data.get("schema_version"), str):
        problems.append("schema_version must be a string")
    benchmarks = data.get("benchmarks")
    if not isinstance(benchmarks, dict):
        problems.append("benchmarks must be an object")
        return problems
    for benchmark_id, entry in benchmarks.items():
        if not isinstance(entry, dict):
            problems.append(f"{benchmark_id}: entry must be an object")
            continue
        if not isinstance(entry.get("median_ms"), (int, float)):
            problems.append(f"{benchmark_id}: median_ms must be a number")
    return problems


def run_check(args) -> int:
    baseline_path = Path(args.baseline)
    problems = validate_baseline(baseline_path)
    if problems:
        for problem in problems:
            print(f"{GATE} BASELINE-INVALID: {problem}", file=sys.stderr)
        print(f"{GATE} FAIL (invalid baseline)")
        return 1

    measured_path = Path(args.measured)
    if not measured_path.exists():
        print(f"{GATE} NO-ARTIFACT: {measured_path} not found — nothing to compare")
        print(f"{GATE} PASS (no measured artifact)")
        return 0

    baseline = load_json(baseline_path)
    references = baseline.get("benchmarks", {})
    tolerance = float(args.tolerance) if args.tolerance else float(
        baseline.get("default_tolerance_ratio", DEFAULT_TOLERANCE)
    )
    min_delta_ms = float(args.min_delta_ms) if args.min_delta_ms is not None else float(
        baseline.get("min_delta_ms", 0.0)
    )
    measured = collect_measured(measured_path)

    suspicious = []
    checked = 0
    no_reference = 0
    for benchmark_id in sorted(measured):
        sample = measured[benchmark_id]
        reference = references.get(benchmark_id)
        if not reference or not reference.get("median_ms"):
            print(f"{GATE} NO-REFERENCE {benchmark_id} (measured median {sample['median_ms']:.3f}ms)")
            no_reference += 1
            continue
        checked += 1
        ref_median = float(reference["median_ms"])
        ratio = sample["median_ms"] / ref_median if ref_median > 0 else 0.0
        delta = sample["median_ms"] - ref_median
        status = "OK"
        # Require BOTH a relative breach and a meaningful absolute delta so a
        # sub-millisecond benchmark (e.g. save_30d ~0.06ms) cannot flag on timer
        # noise alone. The floor is committed in the baseline as min_delta_ms.
        if ratio > tolerance and delta > min_delta_ms:
            status = "SUSPICIOUS"
            suspicious.append((benchmark_id, sample["median_ms"], ref_median, ratio))
        print(
            f"{GATE} CHECK {benchmark_id}: median={sample['median_ms']:.3f}ms "
            f"ref={ref_median:.3f}ms ratio={ratio:.2f}x delta={delta:+.3f}ms "
            f"tol={tolerance:.2f}x floor={min_delta_ms:.3f}ms [{status}]"
        )

    summary = {
        "schema_version": "1.0.0",
        "generated_at": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "checked": checked,
        "no_reference": no_reference,
        "suspicious": [
            {"benchmark_id": b, "measured_ms": m, "reference_ms": r, "ratio": ratio}
            for (b, m, r, ratio) in suspicious
        ],
    }
    if args.json:
        print(json.dumps(summary, indent=2))

    if suspicious:
        if args.warn_only:
            print(f"{GATE} SUSPICIOUS ({len(suspicious)} over tolerance; warn-only)")
            return 0
        print(f"{GATE} FAIL ({len(suspicious)} benchmark(s) over tolerance)")
        return 1
    print(f"{GATE} PASS ({checked} checked, {no_reference} no-reference)")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description="ASHFALL runtime performance regression gate")
    parser.add_argument("mode", nargs="?", default="check", choices=["check"],
                        help="check a measured artifact against the committed baseline")
    parser.add_argument("--measured", default=str(DEFAULT_MEASURED),
                        help="measured PerformanceBaselineReport JSON")
    parser.add_argument("--baseline", default=str(DEFAULT_BASELINE),
                        help="committed baseline JSON")
    parser.add_argument("--tolerance", type=float, default=None,
                        help="override the baseline tolerance ratio (e.g. 1.30)")
    parser.add_argument("--min-delta-ms", type=float, default=None,
                        help="only flag when the absolute median delta also exceeds this many ms")
    parser.add_argument("--warn-only", action="store_true",
                        help="report SUSPICIOUS but exit 0")
    parser.add_argument("--check-only", action="store_true",
                        help="validate the baseline file and exit")
    parser.add_argument("--json", action="store_true",
                        help="emit a machine-readable summary")
    args = parser.parse_args()

    if args.check_only:
        problems = validate_baseline(Path(args.baseline))
        if problems:
            for problem in problems:
                print(f"{GATE} BASELINE-INVALID: {problem}", file=sys.stderr)
            print(f"{GATE} FAIL (invalid baseline)")
            return 1
        print(f"{GATE} PASS (baseline valid)")
        return 0

    return run_check(args)


if __name__ == "__main__":
    sys.exit(main())
