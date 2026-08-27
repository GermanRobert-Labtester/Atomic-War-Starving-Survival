#!/usr/bin/env python3
"""
generate-selftest-manifest.py — Machine-Readable Self-Test Manifest, Timing & CI Runner

Maintains docs/ci/SELFTEST_MANIFEST.json as the authoritative machine-readable
manifest for all host self-tests, UI tests, and headless diagnostic gates.
Measures execution time against test budgets and reports performance regressions
without failing CI when functional assertions pass.

Usage:
  python3 scripts/ci/generate-selftest-manifest.py           # Regenerate docs/ci/SELFTEST_MANIFEST.json
  python3 scripts/ci/generate-selftest-manifest.py --check   # Verify manifest is in sync with host registry
  python3 scripts/ci/generate-selftest-manifest.py --run <id># Run a specific test with timing and validate summary
  python3 scripts/ci/generate-selftest-manifest.py --smoke-all # Run all headless self-tests with timing & budgets
"""

import json
import os
import pathlib
import re
import subprocess
import sys
import time

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
MANIFEST_PATH = REPO_ROOT / "docs" / "ci" / "SELFTEST_MANIFEST.json"

DEFAULT_PER_TEST_BUDGET_SEC = 5.0
SPECIAL_BUDGETS_SEC = {
    "7_day_smoke_selftest": 30.0,
    "expansions_selftest": 10.0,
    "data_integrity_selftest": 6.0,
    "deep_coast_host_selftest": 8.0,
    "warlord_host_selftest": 8.0
}

def fetch_live_manifest():
    cmd = ["godot", "--headless", "--path", str(REPO_ROOT), "--", "--selftest-manifest"]
    res = subprocess.run(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True, cwd=str(REPO_ROOT))
    if res.returncode != 0:
        print(f"ERROR: failed to query --selftest-manifest (exit code {res.returncode}):", file=sys.stderr)
        print(res.stderr, file=sys.stderr)
        sys.exit(1)

    output = res.stdout
    start_idx = output.find("{")
    end_idx = output.rfind("}")
    if start_idx == -1 or end_idx == -1:
        print("ERROR: no JSON payload found in --selftest-manifest output", file=sys.stderr)
        print(output, file=sys.stderr)
        sys.exit(1)

    json_str = output[start_idx:end_idx+1]
    try:
        data = json.loads(json_str)
        return data
    except Exception as ex:
        print(f"ERROR: failed to parse JSON from --selftest-manifest: {ex}", file=sys.stderr)
        print(json_str, file=sys.stderr)
        sys.exit(1)

def run_and_validate_test(entry):
    primary_flag = entry["primary_flag"]
    test_id = entry["test_id"]
    budget_sec = SPECIAL_BUDGETS_SEC.get(test_id, DEFAULT_PER_TEST_BUDGET_SEC)

    print(f"── Running {test_id} ({primary_flag}, budget: {budget_sec:.1f}s) ──")
    cmd = ["godot", "--headless", "--path", str(REPO_ROOT), "--", primary_flag]

    start_time = time.perf_counter()
    try:
        res = subprocess.run(
            cmd,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            cwd=str(REPO_ROOT),
            timeout=entry.get("timeout_seconds", 30)
        )
    except subprocess.TimeoutExpired:
        elapsed = time.perf_counter() - start_time
        print(f"FAIL: {test_id} timed out after {elapsed:.2f}s (timeout: {entry.get('timeout_seconds', 30)}s)", file=sys.stderr)
        return False, elapsed, budget_sec, False

    elapsed = time.perf_counter() - start_time
    output = res.stdout + "\n" + res.stderr

    # Check for standard summary line
    summary_match = re.search(r"\[HOST_SELFTEST_SUMMARY\]\s+test=(\S+)\s+status=(PASS|FAIL)", output)
    json_match = re.search(r"\[HOST_SELFTEST_JSON\]\s+(\{.*?\})", output)
    banner_match = re.search(r"\[HOST_SELFTEST\]\s+(\S+)\s+(PASS|FAIL)", output)
    legacy_match = re.search(r"SELFTEST\s+(PASS|FAIL):\s*(\S+)", output)

    has_summary = summary_match or json_match or banner_match or legacy_match

    if res.returncode != 0:
        print(f"FAIL: {test_id} exited with non-zero exit code: {res.returncode} ({elapsed:.2f}s)", file=sys.stderr)
        if not has_summary:
            print("      (and emitted no standard summary line)", file=sys.stderr)
        return False, elapsed, budget_sec, False

    if not has_summary:
        print(f"FAIL: {test_id} exited 0 but emitted NO machine-readable summary line ({elapsed:.2f}s)", file=sys.stderr)
        return False, elapsed, budget_sec, False

    status = "UNKNOWN"
    if summary_match:
        status = summary_match.group(2)
    elif json_match:
        try:
            j = json.loads(json_match.group(1))
            status = j.get("status", "UNKNOWN")
        except:
            pass
    elif banner_match:
        status = banner_match.group(2)
    elif legacy_match:
        status = legacy_match.group(1)

    if status.upper() != "PASS":
        print(f"FAIL: {test_id} summary status reported {status} ({elapsed:.2f}s)", file=sys.stderr)
        return False, elapsed, budget_sec, False

    is_over_budget = elapsed > budget_sec
    if is_over_budget:
        diff = elapsed - budget_sec
        print(f"⚠️  PASS with PERF REGRESSION: {test_id} took {elapsed:.2f}s (budget: {budget_sec:.2f}s, +{diff:.2f}s)")
    else:
        print(f"PASS: {test_id} (exit 0, status={status}, took {elapsed:.2f}s / budget {budget_sec:.2f}s)")

    return True, elapsed, budget_sec, is_over_budget

def main():
    args = sys.argv[1:]

    if "--check" in args:
        live_data = fetch_live_manifest()
        formatted_live = json.dumps(live_data, indent=2) + "\n"

        if not MANIFEST_PATH.exists():
            print(f"FAIL: {MANIFEST_PATH} does not exist. Run python3 scripts/ci/generate-selftest-manifest.py", file=sys.stderr)
            sys.exit(1)

        current = MANIFEST_PATH.read_text(encoding="utf-8")
        if current != formatted_live:
            print("FAIL: docs/ci/SELFTEST_MANIFEST.json is out of date with HostCliRegistry.", file=sys.stderr)
            print("Fix:  python3 scripts/ci/generate-selftest-manifest.py && git add docs/ci/SELFTEST_MANIFEST.json", file=sys.stderr)
            sys.exit(1)

        print(f"OK: docs/ci/SELFTEST_MANIFEST.json is valid and in sync ({live_data['total_tests']} tests cataloged).")
        sys.exit(0)

    elif "--run" in args:
        idx = args.index("--run")
        if idx + 1 >= len(args):
            print("Usage: --run <test_id_or_flag>", file=sys.stderr)
            sys.exit(2)
        target = args[idx + 1].strip().lstrip("-")

        data = fetch_live_manifest()
        found = None
        for t in data["tests"]:
            if t["test_id"] == target or t["primary_flag"].lstrip("-") == target or target in [a.lstrip("-") for a in t["aliases"]]:
                found = t
                break

        if not found:
            print(f"ERROR: test '{target}' not found in manifest", file=sys.stderr)
            sys.exit(1)

        ok, elapsed, budget, is_over = run_and_validate_test(found)
        sys.exit(0 if ok else 1)

    elif "--smoke-all" in args:
        data = fetch_live_manifest()
        tests = [t for t in data["tests"] if t.get("headless_compatible", True)]
        print(f"Running smoke test over {len(tests)} headless self-tests with timing & budgets...\n")

        passed = 0
        failed = 0
        failures = []
        regressions = []
        total_time = 0.0

        for t in tests:
            ok, elapsed, budget, is_over = run_and_validate_test(t)
            total_time += elapsed
            if ok:
                passed += 1
                if is_over:
                    regressions.append((t["test_id"], elapsed, budget, elapsed - budget))
            else:
                failed += 1
                failures.append(t["test_id"])

        print(f"\n=================================================================================")
        print(f"  SELF-TEST SMOKE SUMMARY ({total_time:.2f}s total)")
        print(f"=================================================================================")
        print(f"Functional Status: {passed}/{len(tests)} PASS, {failed} FAIL")

        if len(regressions) > 0:
            print(f"\n⚠️  PERF REGRESSION ADVISORIES ({len(regressions)} tests exceeded budget — non-blocking):")
            for r_id, r_el, r_bud, r_diff in regressions:
                print(f"  - {r_id}: {r_el:.2f}s vs budget {r_bud:.2f}s (+{r_diff:.2f}s)")

        if failed > 0:
            print(f"\n❌ Functional Failures: {', '.join(failures)}", file=sys.stderr)
            sys.exit(1)

        print("\n✅ All headless self-tests functional gates passed.")
        sys.exit(0)

    else:
        live_data = fetch_live_manifest()
        formatted_live = json.dumps(live_data, indent=2) + "\n"
        MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
        MANIFEST_PATH.write_text(formatted_live, encoding="utf-8")
        print(f"Wrote {MANIFEST_PATH} ({live_data['total_tests']} tests cataloged, {live_data['headless_test_count']} headless compatible).")

if __name__ == "__main__":
    main()
