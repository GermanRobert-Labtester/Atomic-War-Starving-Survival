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
  python3 scripts/ci/run-gates.py --tier performance   # Runs all performance-tier gates
  python3 scripts/ci/run-gates.py --gate data_integrity # Runs single gate
  python3 scripts/ci/run-gates.py --list               # Lists all registered gates
"""

import os
import sys
import json
import time
import argparse
import fnmatch
import pathlib
import subprocess
import tempfile
from datetime import datetime, timezone

REPO_ROOT = pathlib.Path(__file__).resolve().parent.parent.parent
DEFAULT_MANIFEST = REPO_ROOT / "docs" / "ci" / "CI_GATE_MANIFEST.json"
DEFAULT_QUARANTINE = REPO_ROOT / "scripts" / "ci" / "quarantine.json"
QUARANTINE_MAX_DAYS = 14
MAX_GATE_TIMEOUT_SECONDS = 420
# Upper bound for a single gate. compiler_warning_baseline rebuilds three
# projects (-t:Rebuild each: Core.Tests ~125s, Core ~58s, host ~140s under load),
# so it is structurally >180s. Other gates declare <=180s and are unaffected.
FAST_TIER_DEFAULT_TIMEOUT_SECONDS = 180


def load_quarantine(path=DEFAULT_QUARANTINE):
    """Plan VIII Task 24.7 — flake quarantine registry.

    Returns (by_gate, errors): by_gate maps gate_id → entry; errors is a list of
    policy violations (protected gate targeted, expired entry, bad fields).
    Expired entries are reported and then ignored — the gate's failure then
    fails the run like any other gate.
    """
    by_gate, errors = {}, []
    if not path.exists():
        return by_gate, errors
    try:
        data = json.loads(path.read_text(encoding="utf-8"))
    except Exception as ex:
        return by_gate, [f"quarantine registry unreadable: {ex}"]

    protected = set(data.get("protected_gates", []))
    today = datetime.now(timezone.utc).date()
    for entry in data.get("quarantines", []):
        gid = entry.get("gate", "")
        if not gid or not entry.get("owner") or not entry.get("reason") or not entry.get("added"):
            errors.append(f"quarantine entry for '{gid}' missing owner/reason/added fields")
            continue
        if gid in protected:
            errors.append(f"quarantine entry targets protected Core-invariant gate '{gid}' — rejected")
            continue
        try:
            expiry = datetime.strptime(entry["expiry"], "%Y-%m-%d").date()
        except Exception:
            errors.append(f"quarantine entry for '{gid}' has bad expiry (need YYYY-MM-DD)")
            continue
        added = datetime.strptime(entry["added"], "%Y-%m-%d").date()
        if (expiry - added).days > QUARANTINE_MAX_DAYS:
            errors.append(
                f"quarantine entry for '{gid}' exceeds max duration "
                f"({QUARANTINE_MAX_DAYS} days): {added} → {expiry}")
            continue
        if expiry < today:
            errors.append(
                f"quarantine entry for '{gid}' EXPIRED on {expiry} — remove it or re-justify with the owner")
            continue
        by_gate[gid] = entry
    return by_gate, errors


def classify_failure(record):
    """Plan VIII Task 24.9 — failure taxonomy. Returns one of
    blocked / timeout / build-break / assert-fail / selftest-fail /
    infrastructure / quarantined / fail."""
    if record.get("blocked"):
        return "blocked"
    if record.get("exit_code") == 124:
        return "timeout"
    output = record.get("output", "")
    cmd = record.get("command", "")
    if record.get("exit_code") == 127 or "command not found" in output:
        return "infrastructure"
    if record.get("exit_code") == 125:
        return "infrastructure"
    if "error CS" in output or "MSBUILD" in output[:200] or cmd.startswith("dotnet build"):
        return "build-break"
    if cmd.startswith("dotnet test") and ("Failed!" in output or "error" in output.lower()):
        return "assert-fail"
    if "SELFTEST FAIL" in output or "FAIL" in output[-2000:]:
        return "selftest-fail"
    if "Execution error" in record.get("error_reason", ""):
        return "infrastructure"
    return "fail"


# ---------------------------------------------------------------------------
# Optional parallel execution (--jobs N)
#
# Independent static gates are delegated to the approved Go task runner.
# This Python orchestrator never creates parallel subprocesses itself.
# ---------------------------------------------------------------------------
HEAVY_COMMAND_MARKERS = (
    "dotnet ", "dotnet\t",
    "godot", "run-godot-bounded",
    "ui-layout-check.sh", "warning-baseline-gate.sh",
    "coverage-gate.sh", "content-acceptance-gate.sh",
)


def is_heavy_gate(gate):
    """True for gates that must execute on the serial spine."""
    cmd = gate.get("command", "") or ""
    return any(marker in cmd for marker in HEAVY_COMMAND_MARKERS)


def run_parallel_gates(gates, jobs, quarantine_by_gate):
    """Run independent gates through the Go-only subprocess runner."""
    tasks = []
    for gate in gates:
        timeout = min(max(int(gate.get("timeout_seconds", 30)), 1), MAX_GATE_TIMEOUT_SECONDS)
        tasks.append({
            "id": gate.get("gate_id", "unknown"),
            "command": "bash",
            "args": ["-c", gate.get("command", "")],
            "working_dir": str(REPO_ROOT),
            "timeout": timeout * 1_000_000_000,
        })
    try:
        proc = subprocess.run(
            [str(REPO_ROOT / "bin" / "ashfall-dev"), "run-tasks", "-j", str(jobs), "-json"],
            input=json.dumps(tasks), capture_output=True, text=True, cwd=str(REPO_ROOT),
            # Safety net only: the Go runner enforces each task's own timeout, so
            # bound the batch by the worst case (all waves serial) rather than the
            # single longest gate, which would kill a large pool early.
            timeout=MAX_GATE_TIMEOUT_SECONDS * max(1, len(tasks)) + 60,
        )
        by_id = {result.get("id"): result for result in json.loads(proc.stdout)}
    except Exception as ex:
        by_id = {gate.get("gate_id", "unknown"): {
            "exit_code": 1, "duration_seconds": 0, "output": "",
            "error": f"Go task runner failed: {ex}", "timed_out": False,
        } for gate in gates}

    records = {}
    for gate in gates:
        gid = gate.get("gate_id", "unknown")
        cmd = gate.get("command", "")
        timeout = min(max(int(gate.get("timeout_seconds", 30)), 1), MAX_GATE_TIMEOUT_SECONDS)
        task = by_id.get(gid, {})
        exit_code = int(task.get("exit_code", 1))
        output = task.get("output", "")
        expected = gate.get("expected_summary", "")
        error = task.get("error", "")
        if task.get("timed_out"):
            exit_code = 124
            error = f"Gate timed out after {timeout} seconds"
        passed = exit_code == 0 and (not expected or expected in output)
        if exit_code != 0 and not error:
            error = f"Command exited with non-zero code {exit_code}"
        elif exit_code == 0 and expected and expected not in output:
            error = f"Missing expected summary token '{expected}'"
        record = {
            "gate_id": gid, "name": gate.get("name", gid),
            "category": gate.get("category", "General"), "command": cmd,
            "timeout_seconds": timeout, "expected_summary": expected,
            "classification": gate.get("classification", "fast"),
            "remediation": gate.get("remediation"), "passed": passed,
            "blocked": False, "quarantined": False, "exit_code": exit_code,
            "duration": float(task.get("duration_seconds", 0)),
            "error_reason": error, "output": output,
        }
        if not passed:
            record["failure_type"] = classify_failure(record)
            if gid in quarantine_by_gate:
                record["quarantined"] = True
                record["failure_type"] = "quarantined"
        records[gid] = record
    return records


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


# Plan VIII · Task 24.1 — the generated inventory is a deterministic projection of
# the manifest so `gate_inventory_drift` can fail closed when prose drifts.
INVENTORY_TIER_CONTRACT = (
    "## Tier contract\n"
    "\n"
    "- **fast** — pre-merge standard: build + unit suite + data integrity + bridge + asset registry + drift guards + case alias guard + save/failure UX smoke. Target < 10 min on a clean machine.\n"
    "- **full** — shippable standard: everything in fast plus runtime-scale performance and `export_parity` (exported-build packaged-data parity; requires a fresh `scripts/ci/export-build.sh` artifact — on runners without export templates, run the export on a capable machine and verify the artifact, per docs/RELEASE_EXPORT.md).\n"
    "- **performance** — long runtime-scale runs; never hides release requirements.\n"
)


def render_inventory(manifest):
    """Render docs/ci/GATE_INVENTORY.md deterministically from the manifest."""
    gates = manifest.get("gates", [])
    fast = sum(1 for g in gates if g.get("classification") == "fast")
    out = []
    out.append("# CI Gate Inventory (Plan VIII · Task 24.1)")
    out.append("")
    out.append(
        "Generated from `docs/ci/CI_GATE_MANIFEST.json` — "
        f"{len(gates)} gates, {fast} fast. Regenerate with "
        "`python3 scripts/ci/run-gates.py --write-inventory docs/ci/GATE_INVENTORY.md`. "
        "The manifest is the single authority: add or change gates THERE, never in prose only."
    )
    out.append("")
    out.append(
        "Runtimes below are budgeted timeouts (enforced ceiling), not measured durations; "
        "measured durations land in every `--report-json` run (Task 24.10 budgets)."
    )
    out.append("")
    out.append("| Tier | Gate | Category | Timeout ceiling | Critical | Depends on |")
    out.append("|---|---|---|---|---|---|")
    for g in gates:
        deps = ", ".join(g.get("depends_on", []) or []) or "—"
        critical = "yes" if g.get("critical") else "no"
        out.append(
            f"| {g.get('classification', 'fast')} | `{g.get('gate_id', 'unknown')}` | "
            f"{g.get('category', 'General')} | {g.get('timeout_seconds', 30)}s | {critical} | {deps} |"
        )
    out.append("")
    out.append(INVENTORY_TIER_CONTRACT.rstrip("\n"))
    out.append("")
    return "\n".join(out)


def check_inventory(manifest, inventory_path):
    expected = render_inventory(manifest)

    def matches(path):
        p = pathlib.Path(path)
        actual = p.read_text(encoding="utf-8") if p.exists() else ""
        return actual == expected

    if matches(inventory_path):
        # F10 — negative self-check: a mutated inventory must fail, so the
        # drift gate can never become vacuously green.
        mutated = expected.replace("| fast |", "| full |", 1)
        if mutated == expected:
            mutated = expected + "\n<!-- mutation -->\n"
        with tempfile.NamedTemporaryFile("w", suffix=".md", delete=False, encoding="utf-8") as tmp:
            tmp.write(mutated)
            tmp_path = tmp.name
        try:
            if matches(tmp_path):
                print("GATE_INVENTORY FAIL: self-check — a mutated inventory unexpectedly matched", file=sys.stderr)
                return 1
        finally:
            os.unlink(tmp_path)
        print("GATE_INVENTORY PASS")
        return 0
    print(
        f"GATE_INVENTORY FAIL: {inventory_path} is out of sync with docs/ci/CI_GATE_MANIFEST.json",
        file=sys.stderr,
    )
    print("Run: python3 scripts/ci/run-gates.py --write-inventory docs/ci/GATE_INVENTORY.md", file=sys.stderr)
    return 1


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
        hint = g.get("remediation")
        if hint:
            lines.append(f"**Remediation:** `{hint}`  ")
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


def check_timing_budget(results):
    """Non-fatal: flag gates running >30% over their recorded reference.

    A stage <=30% over is tolerable; >30% means investigate and optimize the
    test, not raise its budget (docs/ci/TIMING_BUDGET.md). Never changes the
    exit code — this is a warning only.
    """
    baseline_path = REPO_ROOT / "docs" / "ci" / "TIMING_BASELINE.json"
    if not baseline_path.exists():
        return
    try:
        baseline = json.loads(baseline_path.read_text(encoding="utf-8"))
    except (OSError, ValueError):
        return
    budgets = baseline.get("budgets", {})
    default_tol = float(baseline.get("default_tolerance_ratio", 1.30))
    suspicious = []
    for row in results:
        gid = row.get("gate_id")
        secs = row.get("duration")
        entry = budgets.get(gid) or {}
        ref = entry.get("reference_seconds")
        if not gid or not isinstance(secs, (int, float)):
            continue
        if not isinstance(ref, (int, float)) or ref <= 0:
            continue
        tol = float(entry.get("tolerance_ratio") or default_tol)
        ratio = float(secs) / float(ref)
        if ratio > tol:
            suspicious.append((gid, float(secs), float(ref), ratio))
    if suspicious:
        print("\n[TIMING BUDGET] SUSPICIOUS: gate(s) more than 30% over reference "
              "— investigate and optimize, do not raise the budget:")
        for gid, secs, ref, ratio in suspicious:
            print(f"  ! {gid}: {secs:.2f}s / ref {ref:.2f}s = {ratio:.2f}x")
        print("  Detail: docs/ci/TIMING_BUDGET.md")


def resolve_dependencies(all_gates, selected):
    """
    Expand `selected` to include every transitive `depends_on` prerequisite,
    returned in manifest order.

    Without this, `--gate <id>` on a host-dependent gate runs Godot without
    building first. Godot then loads the last successfully compiled
    .godot/mono/temp/bin/Debug/Ashfall.dll, so the gate exercises stale bytes
    and can report PASS while the current source does not even compile.
    """
    by_id = {g.get("gate_id"): g for g in all_gates}
    wanted = {g.get("gate_id") for g in selected}

    pending = list(wanted)
    while pending:
        gid = pending.pop()
        for dep in by_id.get(gid, {}).get("depends_on", []) or []:
            if dep not in wanted:
                wanted.add(dep)
                pending.append(dep)

    # Manifest order is the execution order, so prerequisites declared earlier
    # naturally run first.
    return [g for g in all_gates if g.get("gate_id") in wanted]


def validate_dependencies(all_gates):
    """Return a list of manifest problems: unknown or cyclic dependencies."""
    by_id = {g.get("gate_id"): g for g in all_gates}
    order = {g.get("gate_id"): i for i, g in enumerate(all_gates)}
    problems = []

    for g in all_gates:
        gid = g.get("gate_id")
        for dep in g.get("depends_on", []) or []:
            if dep not in by_id:
                problems.append(f"gate '{gid}' depends on unknown gate '{dep}'")
            elif order[dep] > order[gid]:
                problems.append(
                    f"gate '{gid}' depends on '{dep}', which is declared later in the manifest; "
                    f"manifest order is execution order")

    # Cycle detection.
    state = {}

    def visit(gid, stack):
        if state.get(gid) == "done":
            return
        if state.get(gid) == "active":
            problems.append("dependency cycle: " + " -> ".join(stack + [gid]))
            return
        state[gid] = "active"
        for dep in by_id.get(gid, {}).get("depends_on", []) or []:
            if dep in by_id:
                visit(dep, stack + [gid])
        state[gid] = "done"

    for g in all_gates:
        visit(g.get("gate_id"), [])

    return problems


# G07 — critical gates whose tooling has no stable output token (exit code is the
# contract). Shrink-only: a NEW critical gate must declare an expected_summary.
KNOWN_EMPTY_SUMMARY_CRITICAL = {
    "build_core_tests",
    "build_godot_host",
    "godot_import",
    "catalog_registry_drift",
}


def validate_critical_summaries(manifest):
    problems = []
    for g in manifest.get("gates", []):
        if not g.get("critical"):
            continue
        if (g.get("expected_summary") or "").strip():
            continue
        if g.get("gate_id") not in KNOWN_EMPTY_SUMMARY_CRITICAL:
            problems.append(
                f"critical gate '{g.get('gate_id')}' declares no expected_summary token "
                f"(add one, or add it to KNOWN_EMPTY_SUMMARY_CRITICAL with a reason)"
            )
    return problems


# H15 — category ratchet: new gates must reuse an existing category, not invent one.
KNOWN_CATEGORIES = {
    "Architecture & Catalog Gates",
    "Build & Tests",
    "Campaign Smoke",
    "Code & Repo Hygiene",
    "Drift & Architecture Gates",
    "Drift guard",
    "Host Selftests & Lifecycle",
    "Performance",
    "Quality & Verification",
    "Release",
    "Repository hygiene",
    "Save Stores & Persistence",
    "Source Policy & Lint Gates",
    "UI & Accessibility",
}


def validate_gate_fields(manifest):
    """H07 — fast pre-check mirroring the compiled manifest drift test."""
    problems = []
    allowed = {"fast", "full", "performance", "release"}
    for g in manifest.get("gates", []):
        gid = g.get("gate_id")
        for field in ("gate_id", "name", "command"):
            if not (g.get(field) or "").strip():
                problems.append(f"gate {gid!r} has an empty/missing '{field}'")
        if not isinstance(g.get("timeout_seconds"), int) or g.get("timeout_seconds", 0) <= 0:
            problems.append(f"gate {gid!r} has a non-positive 'timeout_seconds'")
        if g.get("classification") not in allowed:
            problems.append(f"gate {gid!r} has invalid classification {g.get('classification')!r}")
    return problems


def validate_categories(manifest):
    problems = []
    for g in manifest.get("gates", []):
        cat = g.get("category")
        if cat not in KNOWN_CATEGORIES:
            problems.append(
                f"gate '{g.get('gate_id')}' uses unknown category {cat!r}; reuse an existing category "
                f"or add it to KNOWN_CATEGORIES with a reason"
            )
    return problems


def validate_remediation(manifest):
    """I09 — a per-gate remediation hint must be a non-empty string when present."""
    problems = []
    for g in manifest.get("gates", []):
        rem = g.get("remediation")
        if rem is not None and not (isinstance(rem, str) and rem.strip()):
            problems.append(f"gate '{g.get('gate_id')}' has an empty/non-string 'remediation'")
    return problems


def validate_manifest_counts(manifest):
    """F08/I15 — the header count fields must be ints and match the gates array."""
    problems = []
    gates = manifest.get("gates", [])
    actual_total = len(gates)
    actual_fast = sum(1 for g in gates if g.get("classification") == "fast")
    total = manifest.get("total_gates")
    fast = manifest.get("fast_tier_count")
    if not isinstance(total, int) or isinstance(total, bool):
        problems.append(f"total_gates must be an integer, got {type(total).__name__}")
    elif total != actual_total:
        problems.append(f"total_gates is {total} but the manifest declares {actual_total} gates")
    if not isinstance(fast, int) or isinstance(fast, bool):
        problems.append(f"fast_tier_count must be an integer, got {type(fast).__name__}")
    elif fast != actual_fast:
        problems.append(f"fast_tier_count is {fast} but {actual_fast} gates are classified 'fast'")
    return problems


def validate_manifest_header(manifest):
    """I06 — schema_version and _schema_version_note must be present strings."""
    problems = []
    sv = manifest.get("schema_version")
    if not (isinstance(sv, str) and sv.strip()):
        problems.append("manifest 'schema_version' must be a non-empty string")
    note = manifest.get("_schema_version_note")
    if not (isinstance(note, str) and note.strip()):
        problems.append("manifest '_schema_version_note' must be a non-empty string")
    return problems


def git_changed_files(base_ref):
    """Files changed vs base_ref (committed range + working tree).

    Used only by opt-in path filters (--changed-base). On any git failure the
    list is empty, so a filtered gate is skipped only if the caller explicitly
    opted in and the diff genuinely shows no matching file.
    """
    names = set()
    for diff_args in ([f"{base_ref}...HEAD"], []):
        try:
            out = subprocess.run(
                ["git", "diff", "--name-only", *diff_args],
                cwd=str(REPO_ROOT), capture_output=True, text=True, timeout=60)
        except Exception:
            continue
        if out.returncode == 0:
            names.update(line.strip() for line in out.stdout.splitlines() if line.strip())
    return sorted(names)


def path_filter_matches(path, patterns):
    """True if `path` matches any glob (tested on both full path and basename).

    fnmatch's `*` also matches `/`, so basename matching alone safely covers
    extension globs like `*.cs` for files in subdirectories.
    """
    base = path.rsplit("/", 1)[-1]
    return any(fnmatch.fnmatch(path, p) or fnmatch.fnmatch(base, p) for p in patterns)


def main():
    parser = argparse.ArgumentParser(description="Canonical ASHFALL Gate Runner")
    parser.add_argument("--tier", choices=["fast", "full", "performance", "release", "all"], default="fast",
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
    parser.add_argument("--write-inventory", type=str, default=None,
                        help="Render the manifest into a GATE_INVENTORY.md path and exit")
    parser.add_argument("--check-inventory", type=str, default=None,
                        help="Fail if the GATE_INVENTORY.md path does not match the manifest")
    parser.add_argument("--explain", type=str, default=None,
                        help="Print one gate's command/timeout/deps/classification and exit")
    parser.add_argument("--jobs", type=int, default=1,
                        help="Run independent static gates concurrently (default 1 = serial). "
                             "Gates that build or run Godot always stay serial.")
    parser.add_argument("--changed-base", type=str, default=None,
                        help="Opt-in path filter: skip gates that declare a `paths` list "
                             "when no file changed vs <ref> matches it (e.g. skip "
                             "compiler_warning_baseline on a doc-only change). Default: "
                             "run every selected gate.")

    args = parser.parse_args()

    if args.jobs < 1:
        print("❌ --jobs must be at least 1.", file=sys.stderr)
        return 2

    manifest_path = pathlib.Path(args.manifest)
    manifest = load_manifest(manifest_path)

    if args.list:
        list_gates(manifest)
        return 0

    if args.write_inventory:
        pathlib.Path(args.write_inventory).write_text(render_inventory(manifest), encoding="utf-8")
        print(f"GATE_INVENTORY written: {args.write_inventory}")
        return 0

    if args.check_inventory:
        return check_inventory(manifest, args.check_inventory)

    if args.explain:
        gate = next((g for g in manifest.get("gates", []) if g.get("gate_id") == args.explain), None)
        if gate is None:
            print(f"❌ Unknown gate ID: {args.explain}", file=sys.stderr)
            return 1
        print(f"gate_id:          {gate.get('gate_id')}")
        print(f"name:             {gate.get('name')}")
        print(f"category:         {gate.get('category')}")
        print(f"classification:   {gate.get('classification')}")
        print(f"timeout_seconds:  {gate.get('timeout_seconds')}")
        print(f"critical:         {gate.get('critical')}")
        print(f"expected_summary: {gate.get('expected_summary')!r}")
        print(f"depends_on:       {', '.join(gate.get('depends_on', []) or []) or '(none)'}")
        hint = gate.get("remediation")
        print(f"remediation:      {hint or '(none)'}")
        print(f"command:          {gate.get('command')}")
        return 0

    # Task 24.7 — quarantine policy. Policy violations are fatal: an expired
    # or illegal quarantine must never silently soften the gate set.
    quarantine_by_gate, quarantine_errors = load_quarantine()
    if quarantine_errors:
        print("❌ Quarantine policy violations:", file=sys.stderr)
        for qe in quarantine_errors:
            print(f"   - {qe}", file=sys.stderr)
        return 1

    all_gates = manifest.get("gates", [])

    dep_problems = validate_dependencies(all_gates)
    if dep_problems:
        print("❌ Error: gate manifest dependency problems:", file=sys.stderr)
        for p in dep_problems:
            print(f"   - {p}", file=sys.stderr)
        return 1

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

    # Pull in prerequisites. A host self-test must never run without a
    # successful current-source host build in the same invocation.
    explicit_ids = {g.get("gate_id") for g in gates_to_run}
    gates_to_run = resolve_dependencies(all_gates, gates_to_run)
    added = [g.get("gate_id") for g in gates_to_run if g.get("gate_id") not in explicit_ids]
    if added:
        print(f"[Prerequisites] Adding {len(added)} required gate(s): {', '.join(added)}")

    if args.check_only:
        count_problems = validate_manifest_counts(manifest)
        if count_problems:
            print("❌ Error: gate manifest count problems:", file=sys.stderr)
            for p in count_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        summary_problems = validate_critical_summaries(manifest)
        if summary_problems:
            print("❌ Error: critical gate summary problems:", file=sys.stderr)
            for p in summary_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        header_problems = validate_manifest_header(manifest)
        if header_problems:
            print("❌ Error: manifest header problems:", file=sys.stderr)
            for p in header_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        field_problems = validate_gate_fields(manifest)
        if field_problems:
            print("❌ Error: gate field problems:", file=sys.stderr)
            for p in field_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        category_problems = validate_categories(manifest)
        if category_problems:
            print("❌ Error: gate category problems:", file=sys.stderr)
            for p in category_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        remediation_problems = validate_remediation(manifest)
        if remediation_problems:
            print("❌ Error: gate remediation problems:", file=sys.stderr)
            for p in remediation_problems:
                print(f"   - {p}", file=sys.stderr)
            return 1
        print(f"✅ Gate manifest valid: {len(all_gates)} total gates, "
              f"{len(gates_to_run)} in tier '{args.tier}', dependencies resolve cleanly.")
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

    changed_files = set(git_changed_files(args.changed_base)) if args.changed_base else None
    if changed_files is not None:
        print(f"Path filter: --changed-base {args.changed_base} — "
              f"{len(changed_files)} changed file(s)")
    results = []
    failed_gates = []
    outcome_by_id = {}
    parallel_records = {}
    start_all = time.time()

    # Optional: run dependency-free, non-building gates concurrently. Their
    # records are consumed in manifest order by the serial loop below, so
    # reporting and exit codes are identical to the serial path.
    if args.jobs and args.jobs > 1:
        parallel_gates = [
            g for g in gates_to_run
            if not is_heavy_gate(g) and not (g.get("depends_on") or [])
        ]
        if parallel_gates:
            print(f"Parallel:    {len(parallel_gates)} independent static gate(s) on "
                  f"{args.jobs} worker(s) via bin/ashfall-dev")
            parallel_records = run_parallel_gates(parallel_gates, args.jobs, quarantine_by_gate)

    for idx, gate in enumerate(gates_to_run, 1):
        gid = gate.get("gate_id", "unknown")
        name = gate.get("name", gid)
        cmd = gate.get("command", "")
        configured_timeout = int(gate.get("timeout_seconds", 30))
        timeout = min(max(configured_timeout, 1), MAX_GATE_TIMEOUT_SECONDS)
        expected_summary = gate.get("expected_summary", "")
        category = gate.get("category", "General")

        pre = parallel_records.get(gid)
        if pre is not None:
            results.append(pre)
            outcome_by_id[gid] = pre["passed"]
            if pre.get("quarantined"):
                state = "🛡 QUARANTINED"
            elif pre["passed"]:
                state = "PASS"
            else:
                state = "❌ FAIL"
            print(f"\n[{idx}/{len(gates_to_run)}] [parallel] {name} ({gid}) -> {state} ({pre['duration']:.2f}s)")
            if not pre["passed"]:
                if pre.get("error_reason"):
                    print(f"  -> {pre['error_reason']}")
                snippet = pre.get("output", "").strip().splitlines()[-5:]
                if snippet:
                    print("  --- Output Snippet (last 5 lines) ---")
                    for line in snippet:
                        print(f"  {line}")
                    print("  ------------------------------------")
            if not pre["passed"] and not pre.get("quarantined"):
                failed_gates.append(pre)
                if fail_fast:
                    print(f"\n❌ [ABORT] Fail-fast active: stopping on gate '{gid}'.")
                    break
            continue

        # Opt-in path filter: when --changed-base is given, a gate that declares
        # `paths` is skipped if no changed file matches it (e.g. do not rebuild
        # all C# for compiler_warning_baseline on a doc-only change). Default
        # (no --changed-base) runs every selected gate.
        gate_paths = gate.get("paths") or []
        if changed_files is not None and gate_paths:
            if not any(path_filter_matches(f, gate_paths) for f in changed_files):
                print(f"\n[{idx}/{len(gates_to_run)}] SKIP [path filter] {name} ({gid}) "
                      f"— no changed file matches {gate_paths}")
                results.append({
                    "gate_id": gid, "name": name, "category": category, "command": cmd,
                    "timeout_seconds": timeout, "expected_summary": expected_summary,
                    "classification": gate.get("classification", "fast"),
                    "remediation": gate.get("remediation"),
                    "passed": True, "blocked": False, "quarantined": False,
                    "skipped": True, "skip_reason": "path-filter",
                    "failure_type": None, "exit_code": 0, "duration": 0.0,
                    "error_reason": None, "output": ""
                })
                outcome_by_id[gid] = True
                continue

        print(f"\n[{idx}/{len(gates_to_run)}] Running [{category}] {name} ({gid})...")
        if configured_timeout != timeout:
            print(f"  -> timeout policy: configured {configured_timeout}s, capped at {timeout}s")
        sys.stdout.flush()

        # A gate whose prerequisite did not pass must not execute. Running it
        # anyway is how a host self-test ends up exercising a stale assembly and
        # reporting PASS against source that does not compile.
        unmet = [d for d in (gate.get("depends_on") or []) if outcome_by_id.get(d) is not True]
        if unmet:
            reason = (f"Host build prerequisite failed; self-test not executed. "
                      f"Unmet prerequisite(s): {', '.join(unmet)}")
            print(f"  -> ⛔ BLOCKED: {reason}")
            res_record = {
                "gate_id": gid,
                "name": name,
                "category": category,
                "command": cmd,
                "timeout_seconds": timeout,
                "expected_summary": expected_summary,
                "classification": gate.get("classification", "fast"),
                "remediation": gate.get("remediation"),
                "passed": False,
                "blocked": True,
                "quarantined": False,
                "failure_type": "blocked",
                "exit_code": 1,
                "duration": 0.0,
                "error_reason": reason,
                "output": ""
            }
            results.append(res_record)
            failed_gates.append(res_record)
            outcome_by_id[gid] = False
            if fail_fast:
                print(f"\n❌ [ABORT] Fail-fast active: stopping on gate '{gid}'.")
                break
            continue

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
            "remediation": gate.get("remediation"),
            "passed": passed,
            "blocked": False,
            "quarantined": False,
            "exit_code": exit_code,
            "duration": gate_elapsed,
            "error_reason": error_reason,
            "output": output
        }

        if not passed:
            # Task 24.7/24.9 — quarantine flag + failure taxonomy. A quarantined
            # failure is displayed, recorded, and does not by itself fail the run;
            # downstream gates that depend on it still run (it did execute).
            res_record["failure_type"] = classify_failure(res_record)
            q = quarantine_by_gate.get(gid)
            if q is not None:
                res_record["quarantined"] = True
                res_record["failure_type"] = "quarantined"
                results.append(res_record)
                outcome_by_id[gid] = True
                print(f"  -> 🛡 QUARANTINED ({gate_elapsed:.2f}s): {error_reason}")
                print(f"     owner={q.get('owner')} reason={q.get('reason')} expiry={q.get('expiry')}")
                continue

        results.append(res_record)
        outcome_by_id[gid] = passed

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
        taxonomy = {}
        for r in results:
            if not r["passed"]:
                taxonomy[r["failure_type"]] = taxonomy.get(r["failure_type"], 0) + 1
        report_data = {
            "timestamp": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
            "tier": args.tier,
            "total_duration_seconds": total_elapsed,
            "total_gates": len(gates_to_run),
            "passed_count": len(gates_to_run) - len(failed_gates),
            "failed_count": len(failed_gates),
            "failure_taxonomy": taxonomy,
            "quarantined_failures": [r["gate_id"] for r in results if r.get("quarantined")],
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

    # Non-fatal reference-time budget check (never changes the exit code)
    check_timing_budget(results)

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
