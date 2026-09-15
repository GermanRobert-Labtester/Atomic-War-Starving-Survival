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
  python3 scripts/ci/generate-selftest-manifest.py --smoke-all # Run headless self-tests with timing & budgets
  Options for --smoke-all (all optional, `--opt=value` or `--opt value`):
    --shard I/N     deterministic slice I of N (e.g. 1/4) for low-latency runs
    --include SUB   substring filter over test id / flag / aliases
    --fail-fast     stop at the first functional failure
    --dry-run       list the selection + budget estimate, boot no per-test Godot
    --max-seconds S global cap, clamped to (1, 180]
  Exit codes for --smoke-all: 0 all selected PASS; 1 functional FAIL;
  2 truncated (cap hit) or empty selection — SKIPPED_NOT_STARTED, never FAIL.
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
MAX_GODOT_SECONDS = 180
GODOT_RUNNER = REPO_ROOT / "scripts" / "ci" / "run-godot-bounded.sh"

DEFAULT_PER_TEST_BUDGET_SEC = 5.0
SPECIAL_BUDGETS_SEC = {
    "7_day_smoke_selftest": 30.0,
    "expansions_selftest": 10.0,
    "data_integrity_selftest": 6.0,
    "deep_coast_host_selftest": 8.0,
    "warlord_host_selftest": 8.0
}


def budget_for(test_id):
    return SPECIAL_BUDGETS_SEC.get(test_id, DEFAULT_PER_TEST_BUDGET_SEC)


def parse_smoke_options(args):
    """Parse --smoke-all slice options without disturbing legacy invocations."""
    opts = {"shard": None, "include": None, "fail_fast": "--fail-fast" in args,
            "dry_run": "--dry-run" in args, "max_seconds": MAX_GODOT_SECONDS}

    def take_value(flag):
        for i, a in enumerate(args):
            if a == flag and i + 1 < len(args) and not args[i + 1].startswith("--"):
                return args[i + 1]
            if a.startswith(flag + "="):
                return a[len(flag) + 1:]
        return None

    shard_raw = take_value("--shard")
    if shard_raw is not None:
        try:
            num, den = shard_raw.split("/")
            idx, total = int(num), int(den)
            if idx < 1 or total < 1 or idx > total:
                raise ValueError
            opts["shard"] = (idx, total)
        except ValueError:
            print(f"ERROR: --shard must be I/N with 1 <= I <= N (got '{shard_raw}')", file=sys.stderr)
            sys.exit(2)
    inc = take_value("--include")
    if inc is not None and inc.strip():
        opts["include"] = inc.strip().lower()
    max_raw = take_value("--max-seconds")
    if max_raw is not None:
        try:
            opts["max_seconds"] = min(max(int(max_raw), 1), MAX_GODOT_SECONDS)
        except ValueError:
            print(f"ERROR: --max-seconds must be an integer (got '{max_raw}')", file=sys.stderr)
            sys.exit(2)
    return opts


def select_smoke_tests(tests, opts):
    selected = list(tests)
    if opts.get("include"):
        needle = opts["include"]
        selected = [t for t in selected
                    if needle in t.get("test_id", "").lower()
                    or needle in t.get("primary_flag", "").lower()
                    or any(needle in a.lower() for a in (t.get("aliases") or []))]
    shard = opts.get("shard")
    if shard is not None:
        idx, total = shard
        selected = [t for i, t in enumerate(selected) if (i % total) == (idx - 1)]
    return selected

def fetch_live_manifest():
    cmd = ["bash", str(GODOT_RUNNER), "--path", str(REPO_ROOT), "--", "--selftest-manifest"]
    res = subprocess.run(
        cmd,
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True,
        cwd=str(REPO_ROOT),
        timeout=MAX_GODOT_SECONDS,
    )
    if res.returncode != 0:
        print(f"ERROR: failed to query --selftest-manifest (exit code {res.returncode}):", file=sys.stderr)
        print(res.stderr, file=sys.stderr)
        sys.exit(1)

    output = res.stdout
    start_idx = output.find("{")
    if start_idx == -1:
        print("ERROR: no JSON payload found in --selftest-manifest output", file=sys.stderr)
        print(output, file=sys.stderr)
        sys.exit(1)

    # The verb may print the manifest JSON ahead of [HOST_SELFTEST_*] summary
    # lines; decode exactly the first JSON value instead of trusting brace counts.
    try:
        data, _ = json.JSONDecoder().raw_decode(output[start_idx:])
        return data
    except Exception as ex:
        print(f"ERROR: failed to parse JSON from --selftest-manifest: {ex}", file=sys.stderr)
        print(output[start_idx:start_idx+400], file=sys.stderr)
        sys.exit(1)

def run_and_validate_test(entry, timeout_override=None):
    primary_flag = entry["primary_flag"]
    test_id = entry["test_id"]
    budget_sec = SPECIAL_BUDGETS_SEC.get(test_id, DEFAULT_PER_TEST_BUDGET_SEC)

    print(f"── Running {test_id} ({primary_flag}, budget: {budget_sec:.1f}s) ──")
    cmd = ["bash", str(GODOT_RUNNER), "--path", str(REPO_ROOT), "--", primary_flag]
    configured_timeout = int(entry.get("timeout_seconds", 30))
    timeout_sec = min(configured_timeout, MAX_GODOT_SECONDS)
    if timeout_override is not None:
        timeout_sec = min(timeout_sec, max(int(timeout_override), 1))

    start_time = time.perf_counter()
    try:
        res = subprocess.run(
            cmd,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            cwd=str(REPO_ROOT),
            timeout=timeout_sec
        )
    except subprocess.TimeoutExpired:
        elapsed = time.perf_counter() - start_time
        print(f"FAIL: {test_id} timed out after {elapsed:.2f}s (timeout: {timeout_sec}s)", file=sys.stderr)
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
        opts = parse_smoke_options(args)
        cap = opts["max_seconds"]
        if opts["dry_run"]:
            try:
                data = json.loads(MANIFEST_PATH.read_text(encoding="utf-8"))
            except Exception as ex:
                print(f"ERROR: --dry-run reads {MANIFEST_PATH}: {ex}", file=sys.stderr)
                sys.exit(1)
        else:
            data = fetch_live_manifest()
        headless = [t for t in data["tests"] if t.get("headless_compatible", True)]
        tests = select_smoke_tests(headless, opts)
        if not tests:
            print("Smoke selection is empty after --include/--shard filters; nothing to run.")
            sys.exit(2 if opts["include"] or opts["shard"] else 0)
        estimate = sum(budget_for(t["test_id"]) for t in tests)
        scope = f"{len(tests)}/{len(headless)} headless"
        if opts["shard"] is not None:
            idx, total = opts["shard"]
            scope += f" (shard {idx}/{total})"
        if opts["include"]:
            scope += f" (include '{opts['include']}')"
        print(f"Running smoke test over {scope} with timing & budgets...\n")
        print(f"Estimate: budgets alone sum to ~{estimate:.0f}s (excludes per-boot engine "
              f"startup); global cap {cap}s. Prefer --shard I/N or --include SUB for "
              f"focused low-latency agent runs.\n")
        if opts["dry_run"]:
            for t in tests:
                print(f"  - {t['test_id']} ({t['primary_flag']}, budget {budget_for(t['test_id']):.1f}s)")
            print(f"\nDRY-RUN: {len(tests)} selected, ~{estimate:.0f}s budget estimate, cap {cap}s.")
            sys.exit(0)

        passed = 0
        failed = 0
        failures = []
        skipped = []
        regressions = []
        total_time = 0.0

        smoke_deadline = time.monotonic() + cap
        for index, t in enumerate(tests):
            remaining = int(smoke_deadline - time.monotonic())
            if remaining <= 0:
                skipped.extend(test["test_id"] for test in tests[index:])
                print(
                    f"\n⏭ Smoke cap reached at {cap}s; "
                    f"{len(tests) - index} selected self-test(s) NOT STARTED "
                    f"(reported as SKIPPED, not FAIL). Re-run the remainder with "
                    f"--shard I/N or a narrower --include.",
                    file=sys.stderr,
                )
                break

            ok, elapsed, budget, is_over = run_and_validate_test(t, remaining)
            total_time += elapsed
            if ok:
                passed += 1
                if is_over:
                    regressions.append((t["test_id"], elapsed, budget, elapsed - budget))
            else:
                failed += 1
                failures.append(t["test_id"])
                if opts["fail_fast"]:
                    skipped.extend(test["test_id"] for test in tests[index + 1:])
                    print(f"\n⏹ Fail-fast: stopping after '{t['test_id']}'. "
                          f"{len(skipped)} remaining marked SKIPPED.", file=sys.stderr)
                    break

        print(f"\n=================================================================================")
        print(f"  SELF-TEST SMOKE SUMMARY ({total_time:.2f}s total)")
        print(f"=================================================================================")
        print(f"Functional Status: {passed}/{len(tests)} PASS, {failed} FAIL, {len(skipped)} SKIPPED_NOT_STARTED")

        if len(regressions) > 0:
            print(f"\n⚠️  PERF REGRESSION ADVISORIES ({len(regressions)} tests exceeded budget — non-blocking):")
            for r_id, r_el, r_bud, r_diff in regressions:
                print(f"  - {r_id}: {r_el:.2f}s vs budget {r_bud:.2f}s (+{r_diff:.2f}s)")

        if skipped:
            print(f"\n⏭ Skipped (not started): {', '.join(skipped)}", file=sys.stderr)
        if failed > 0:
            print(f"\n❌ Functional Failures: {', '.join(failures)}", file=sys.stderr)
            sys.exit(1)
        if skipped:
            print(f"\n⚠️ TRUNCATED: all executed gates passed but {len(skipped)} selected "
                  f"test(s) did not start within {cap}s. Narrow with --shard/--include.",
                  file=sys.stderr)
            sys.exit(2)

        print("\n✅ All selected headless self-tests functional gates passed.")
        sys.exit(0)

    else:
        live_data = fetch_live_manifest()
        formatted_live = json.dumps(live_data, indent=2) + "\n"
        MANIFEST_PATH.parent.mkdir(parents=True, exist_ok=True)
        MANIFEST_PATH.write_text(formatted_live, encoding="utf-8")
        print(f"Wrote {MANIFEST_PATH} ({live_data['total_tests']} tests cataloged, {live_data['headless_test_count']} headless compatible).")

if __name__ == "__main__":
    main()
