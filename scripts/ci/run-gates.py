#!/usr/bin/env python3
"""
run-gates.py — Canonical ASHFALL Gate Runner

Executes verification gates defined in docs/ci/CI_GATE_MANIFEST.json.
Used identically by local developers (verify-fast.sh) and GitHub Actions CI.

Features:
  - Stable IDs, commands, timeouts, expected summaries, and classifications.
  - Per-gate execution tracking, duration reporting, and timeout enforcement.
  - Machine-readable JSON summary generation (--report-json).
  - Concise failed-gate artifact generation (--fail-artifact).
  - Fast/full/single-gate selection.

Usage:
  python3 scripts/ci/run-gates.py                      # Runs all fast-tier gates
  python3 scripts/ci/run-gates.py --tier full          # Runs all full-tier gates
  python3 scripts/ci/run-gates.py --gate data_integrity # Runs single gate
  python3 scripts/ci/run-gates.py --list               # Lists all registered gates
"""

import os
import sys
import json
import time
import argparse
import pathlib
import subprocess
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
DEFAULT_MANIFEST = REPO_ROOT / "docs" / "ci" / "CI_GATE_MANIFEST.json"


def load_manifest(manifest_path):
    if not manifest_path.exists():
        print(f"❌ Error: Gate manifest not found at {manifest_path}", file=sys.stderr)
        sys.exit(1)
    try:
        with open(manifest_path, "r", encoding="utf-8") as f:
            return json.load(f)
    except Exception as ex:
        print(f"❌ Error reading gate manifest {manifest_path}: {ex}", file=sys.stderr)
        sys.exit(1)


def list_gates(manifest):
    gates = manifest.get("gates", [])
    print(f"\n=================================================================================")
    print(f"  ASHFALL CI GATE MANIFEST ({len(gates)} Registered Gates)")
    print(f"=================================================================================")
    print(f"{'#':<3} {'Gate ID':<28} {'Tier':<6} {'Timeout':<8} {'Name'}")
    print(f"---------------------------------------------------------------------------------")
    for i, g in enumerate(gates, 1):
        gid = g.get("gate_id", "unknown")
        tier = g.get("classification", "fast")
        tout = f"{g.get('timeout_seconds', 30)}s"
        name = g.get("name", "")
        print(f"{i:<3} {gid:<28} {tier:<6} {tout:<8} {name}")
    print(f"=================================================================================\n")


def write_failure_artifact(artifact_path, failed_gates, total_gates, start_time, end_time):
    artifact_path.parent.mkdir(parents=True, exist_ok=True)
    duration = end_time - start_time
    ts = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%SZ")

    lines = [
        "# ❌ ASHFALL CI Gate Failure Report",
        "",
        f"**Generated:** {ts}  ",
        f"**Status:** FAILED ({len(failed_gates)} of {total_gates} gates failed)  ",
        f"**Duration:** {duration:.2f}s  ",
        "",
        "## Failed Gates Summary",
        "",
        "| Gate ID | Name | Exit Code | Duration | Error Summary |",
        "|---|---|---|---|---|",
    ]

    for g in failed_gates:
        gid = g["gate_id"]
        name = g["name"]
        code = g["exit_code"]
        dur = f"{g['duration']:.2f}s"
        err = g["error_reason"].replace("|", "\\|")
        lines.append(f"| `{gid}` | {name} | `{code}` | {dur} | {err} |")

    lines.append("")
    lines.append("## Failure Diagnostics & Output Logs")
    lines.append("")

    for g in failed_gates:
        gid = g["gate_id"]
        name = g["name"]
        cmd = g["command"]
        output = g.get("output", "").strip()
        lines.append(f"### `{gid}` — {name}")
        lines.append(f"**Command:** `{cmd}`  ")
        lines.append(f"**Reason:** {g['error_reason']}  ")
        lines.append("")
        lines.append("```text")
        # Tail last 60 lines
        out_lines = output.splitlines()
        tail = "\n".join(out_lines[-60:]) if len(out_lines) > 60 else output
        lines.append(tail if tail else "(no output captured)")
        lines.append("```")
        lines.append("")

    lines.append("## Remediation Steps")
    lines.append("")
    lines.append("To reproduce and fix failed gates locally, run:")
    lines.append("```bash")
    for g in failed_gates:
        lines.append(f"# Run gate '{g['gate_id']}':")
        lines.append(f"{g['command']}")
    lines.append("```")
    lines.append("")

    with open(artifact_path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))


def main():
    parser = argparse.ArgumentParser(description="Canonical ASHFALL Gate Runner")
    parser.add_argument("--tier", choices=["fast", "full", "all"], default="fast",
                        help="Filter gates by classification tier (default: fast)")
    parser.add_argument("--gate", type=str, default=None,
                        help="Run a specific gate ID or comma-separated list of gate IDs")
    parser.add_argument("--list", action="store_true",
                        help="List all registered gates in manifest and exit")
    parser.add_argument("--manifest", type=str, default=str(DEFAULT_MANIFEST),
                        help="Path to CI gate manifest JSON")
    parser.add_argument("--report-json", type=str, default=None,
                        help="Output path for structured gate results JSON")
    parser.add_argument("--fail-artifact", type=str, default=None,
                        help="Output path for concise failure markdown artifact")
    parser.add_argument("--no-fail-fast", action="store_true",
                        help="Do not stop on first failure; run remaining gates")
    parser.add_argument("--check-only", action="store_true",
                        help="Validate gate manifest consistency and exit")

    args = parser.parse_args()

    manifest_path = pathlib.Path(args.manifest)
    manifest = load_manifest(manifest_path)

    if args.list:
        list_gates(manifest)
        return 0

    all_gates = manifest.get("gates", [])

    # Filter gates
    if args.gate:
        requested_ids = {gid.strip() for gid in args.gate.split(",") if gid.strip()}
        gates_to_run = [g for g in all_gates if g.get("gate_id") in requested_ids]
        missing = requested_ids - {g.get("gate_id") for g in gates_to_run}
        if missing:
            print(f"❌ Error: Unknown gate ID(s): {', '.join(sorted(missing))}", file=sys.stderr)
            return 1
    elif args.tier == "all":
        gates_to_run = all_gates
    else:
        gates_to_run = [g for g in all_gates if g.get("classification") == args.tier]

    if not gates_to_run:
        print(f"❌ Error: No gates matched tier '{args.tier}'.", file=sys.stderr)
        return 1

    if args.check_only:
        print(f"✅ Gate manifest valid: {len(all_gates)} total gates, {len(gates_to_run)} in tier '{args.tier}'.")
        return 0

    fail_fast = not args.no_fail_fast

    print("=============================================================================")
    print("  ASHFALL CANONICAL VERIFICATION GATE RUNNER")
    print("=============================================================================")
    print(f"Started at:  {datetime.now(timezone.utc).strftime('%Y-%m-%dT%H:%M:%SZ')}")
    print(f"Manifest:    {manifest_path.relative_to(REPO_ROOT) if manifest_path.is_relative_to(REPO_ROOT) else manifest_path}")
    print(f"Target Tier: {args.tier.upper()} ({len(gates_to_run)} gates selected)")
    print(f"Fail-Fast:   {'Enabled' if fail_fast else 'Disabled'}")
    print("-----------------------------------------------------------------------------")

    results = []
    failed_gates = []
    start_all = time.time()

    for idx, gate in enumerate(gates_to_run, 1):
        gid = gate.get("gate_id", "unknown")
        name = gate.get("name", gid)
        cmd = gate.get("command", "")
        timeout = gate.get("timeout_seconds", 30)
        expected_summary = gate.get("expected_summary", "")
        category = gate.get("category", "General")

        print(f"\n[{idx}/{len(gates_to_run)}] Running [{category}] {name} ({gid})...")
        sys.stdout.flush()

        gate_start = time.time()
        exit_code = 0
        output = ""
        error_reason = ""
        passed = False

        try:
            proc = subprocess.run(
                cmd,
                shell=True,
                cwd=str(REPO_ROOT),
                capture_output=True,
                text=True,
                timeout=timeout
            )
            exit_code = proc.returncode
            output = proc.stdout + ("\n" + proc.stderr if proc.stderr else "")

            if exit_code != 0:
                error_reason = f"Command exited with non-zero code {exit_code}"
                passed = False
            elif expected_summary and expected_summary not in output:
                error_reason = f"Missing expected summary token '{expected_summary}'"
                passed = False
            else:
                passed = True

        except subprocess.TimeoutExpired as tex:
            exit_code = 124
            out_str = tex.stdout.decode("utf-8", errors="replace") if isinstance(tex.stdout, bytes) else (tex.stdout or "")
            err_str = tex.stderr.decode("utf-8", errors="replace") if isinstance(tex.stderr, bytes) else (tex.stderr or "")
            output = out_str + ("\n" + err_str if err_str else "")
            error_reason = f"Gate timed out after {timeout} seconds"
            passed = False
        except Exception as ex:
            exit_code = 1
            error_reason = f"Execution error: {ex}"
            passed = False

        gate_elapsed = time.time() - gate_start

        res_record = {
            "gate_id": gid,
            "name": name,
            "category": category,
            "command": cmd,
            "timeout_seconds": timeout,
            "expected_summary": expected_summary,
            "classification": gate.get("classification", "fast"),
            "passed": passed,
            "exit_code": exit_code,
            "duration": gate_elapsed,
            "error_reason": error_reason,
            "output": output
        }
        results.append(res_record)

        if passed:
            print(f"  -> PASS ({gate_elapsed:.2f}s)")
        else:
            print(f"  -> ❌ FAIL ({gate_elapsed:.2f}s): {error_reason}")
            failed_gates.append(res_record)
            if output.strip():
                print("  --- Output Snippet (last 15 lines) ---")
                for line in output.strip().splitlines()[-15:]:
                    print(f"  {line}")
                print("  ---------------------------------------")

            if fail_fast:
                print(f"\n❌ [ABORT] Fail-fast active: stopping on gate '{gid}'.")
                break

    end_all = time.time()
    total_elapsed = end_all - start_all

    # Write report JSON if requested
    if args.report_json:
        rep_path = pathlib.Path(args.report_json)
        rep_path.parent.mkdir(parents=True, exist_ok=True)
        report_data = {
            "timestamp": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
            "total_duration_seconds": total_elapsed,
            "total_gates": len(gates_to_run),
            "passed_count": len(gates_to_run) - len(failed_gates),
            "failed_count": len(failed_gates),
            "all_passed": len(failed_gates) == 0,
            "results": results
        }
        with open(rep_path, "w", encoding="utf-8") as f:
            json.dump(report_data, f, indent=2)
        print(f"\n[Artifact] Wrote JSON gate report to {rep_path}")

    # Write failure artifact if requested and failures occurred
    if args.fail_artifact:
        art_path = pathlib.Path(args.fail_artifact)
        if failed_gates:
            write_failure_artifact(art_path, failed_gates, len(gates_to_run), start_all, end_all)
            print(f"[Artifact] Wrote failure markdown report to {art_path}")
        elif art_path.exists():
            art_path.unlink()

    # Final summary banner
    print("\n=============================================================================")
    if not failed_gates:
        print(f"  ✅ ALL {len(gates_to_run)} GATES PASSED CLEANLY ({total_elapsed:.2f}s)")
        print("=============================================================================")
        return 0
    else:
        print(f"  ❌ {len(failed_gates)} OF {len(gates_to_run)} GATES FAILED ({total_elapsed:.2f}s)")
        print("=============================================================================")
        return 1


if __name__ == "__main__":
    sys.exit(main())
