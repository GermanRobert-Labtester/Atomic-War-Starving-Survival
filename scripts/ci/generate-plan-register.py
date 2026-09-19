#!/usr/bin/env python3
"""E1A baseline enumerator for the plan-governance pipeline.

The later E1B phases extend this entry point into the generated plan register.
The baseline mode deliberately does only repository enumeration and evidence
capture: it never edits plan bodies or runtime files.
"""

from __future__ import annotations

import argparse
import fnmatch
import hashlib
import json
import os
import subprocess
import sys
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path
from typing import Any


SCRIPT_PATH = Path(__file__).resolve()
REPO_ROOT = SCRIPT_PATH.parents[2]
DEFAULT_CONFIG = SCRIPT_PATH.with_name("plan_governance_config.json")
BASELINE_DIR = REPO_ROOT / "docs" / "roadmap" / "e1"
BASELINE_JSON = BASELINE_DIR / "e1_baseline.json"
BASELINE_MD = BASELINE_DIR / "E1_BASELINE.md"


def fail(message: str) -> "NoReturn":
    print(f"generate-plan-register.py: {message}", file=sys.stderr)
    raise SystemExit(2)


def read_json(path: Path) -> dict[str, Any]:
    try:
        return json.loads(path.read_text(encoding="utf-8"))
    except FileNotFoundError:
        fail(f"required JSON file not found: {path.relative_to(REPO_ROOT)}")
    except json.JSONDecodeError as exc:
        fail(f"invalid JSON in {path.relative_to(REPO_ROOT)}: {exc}")
    raise AssertionError("unreachable")


def rel(path: Path) -> str:
    return path.relative_to(REPO_ROOT).as_posix()


def glob_matches(value: str, pattern: str) -> bool:
    """Match both pathlib-style ** patterns and the short relative suffix."""

    value = value.replace(os.sep, "/")
    pattern = pattern.replace(os.sep, "/")
    if fnmatch.fnmatchcase(value, pattern):
        return True
    if pattern.startswith("**/") and fnmatch.fnmatchcase(value, pattern[3:]):
        return True
    return False


def excluded(relative_path: str, patterns: list[str]) -> bool:
    return any(glob_matches(relative_path, pattern) for pattern in patterns)


def sha256(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for block in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def enumerate_files(config: dict[str, Any]) -> dict[str, list[dict[str, Any]]]:
    result: dict[str, list[dict[str, Any]]] = {}
    global_exclude = list(config.get("global_exclude", []))

    for namespace in config.get("plan_namespaces", []):
        namespace_id = namespace["id"]
        root = REPO_ROOT / namespace["root"]
        if not root.is_dir():
            fail(f"configured namespace root does not exist: {namespace['root']}")
        found: set[Path] = set()
        for pattern in namespace.get("include", []):
            found.update(path for path in root.glob(pattern) if path.is_file())
        rows = []
        namespace_exclude = global_exclude + list(namespace.get("exclude", []))
        for path in sorted(found, key=rel):
            relative_to_namespace = path.relative_to(root).as_posix()
            if excluded(rel(path), global_exclude) or excluded(relative_to_namespace, namespace_exclude):
                continue
            rows.append(
                {
                    "path": rel(path),
                    "bytes": path.stat().st_size,
                    "sha256": sha256(path),
                }
            )
        result[namespace_id] = rows
    return result


def classify_doc(path: str) -> str:
    lower = path.lower()
    parts = lower.split("/")
    name = parts[-1]
    if "/roadmap/" in lower:
        return "roadmap"
    if "/design/" in lower:
        return "design"
    if "/debug/" in lower:
        return "debug"
    if "audit" in name or "/audit" in lower:
        return "audit"
    if "index" in name or name in {"readme.md", "current_authority.md"}:
        return "index"
    if "generated" in lower or "/ci/" in lower:
        return "generated"
    return "other"


def enumerate_docs(config: dict[str, Any]) -> list[dict[str, Any]]:
    documentation = config.get("documentation", {})
    root = REPO_ROOT / documentation["root"]
    if not root.is_dir():
        fail(f"configured documentation root does not exist: {documentation['root']}")
    rows = []
    for path in sorted(root.glob("**/*.md"), key=rel):
        relative_to_docs = path.relative_to(root).as_posix()
        if excluded(rel(path), config.get("global_exclude", [])):
            continue
        if excluded(relative_to_docs, list(documentation.get("exclude", []))):
            continue
        rows.append({"path": rel(path), "category": classify_doc(rel(path))})
    return rows


def command_output(command: list[str], timeout: int = 30) -> dict[str, Any]:
    started = datetime.now(timezone.utc)
    environment = os.environ.copy()
    if any("run-godot-bounded.sh" in part for part in command):
        # The bounded runtime witness writes user:// logs. Keep this baseline
        # outside the repository while remaining reproducible in the sandbox.
        environment["XDG_DATA_HOME"] = "/tmp/ashfall_e1a_xdg"
    try:
        completed = subprocess.run(
            command,
            cwd=REPO_ROOT,
            env=environment,
            text=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            timeout=timeout,
            check=False,
        )
        output = completed.stdout or ""
        return {
            "command": " ".join(command),
            "exit_code": completed.returncode,
            "duration_seconds": round((datetime.now(timezone.utc) - started).total_seconds(), 3),
            "output_tail": "\n".join(output.splitlines()[-20:]),
            "inherited": True,
        }
    except subprocess.TimeoutExpired as exc:
        return {
            "command": " ".join(command),
            "exit_code": 124,
            "duration_seconds": round((datetime.now(timezone.utc) - started).total_seconds(), 3),
            "output_tail": "\n".join((exc.output or "").splitlines()[-20:]),
            "inherited": True,
            "timeout": True,
        }


def git_value(*args: str) -> str:
    try:
        return subprocess.run(
            ["git", *args],
            cwd=REPO_ROOT,
            text=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            check=True,
            timeout=10,
        ).stdout.strip()
    except (OSError, subprocess.SubprocessError):
        return "unavailable"


def tool_version(command: list[str]) -> str:
    try:
        completed = subprocess.run(
            command,
            cwd=REPO_ROOT,
            text=True,
            stdout=subprocess.PIPE,
            stderr=subprocess.STDOUT,
            check=False,
            timeout=10,
        )
        return (completed.stdout or "").strip().splitlines()[0] if completed.stdout else "unavailable"
    except (OSError, subprocess.SubprocessError):
        return "unavailable"


def fast_gate_summary(path: Path | None) -> dict[str, Any] | None:
    if path is None or not path.is_file():
        return None
    data = read_json(path)
    results = data.get("results", [])
    return {
        "source": rel(path),
        "tier": data.get("tier"),
        "total_gates": data.get("total_gates", len(results)),
        "passed_count": data.get("passed_count"),
        "failed_count": data.get("failed_count"),
        "all_passed": data.get("all_passed"),
        "failures": [
            {
                "gate_id": item.get("gate_id"),
                "exit_code": item.get("exit_code"),
                "error_reason": item.get("error_reason"),
            }
            for item in results
            if item.get("exit_code", 0) != 0
        ],
        "inherited": True,
    }


def build_baseline(config_path: Path, gate_results: Path | None = None, run_witnesses: bool = False) -> dict[str, Any]:
    config = read_json(config_path)
    namespaces = enumerate_files(config)
    documents = enumerate_docs(config)
    counts = {key: len(value) for key, value in namespaces.items()}
    counts["total_plans"] = sum(counts.values())
    doc_counts = dict(sorted(Counter(row["category"] for row in documents).items()))

    evidence: list[dict[str, Any]] = []
    if run_witnesses:
        commands = [
            (["python3", "scripts/ci/generate-docs-index.py", "--check"], 60),
            (["python3", "scripts/ci/sync-agent-rulebooks.py", "--check"], 60),
            (["python3", "scripts/ci/verify-capability-claims.py", "--check"], 60),
            (["bash", "scripts/ci/doc-link-gate.sh"], 60),
            (["dotnet", "build", "Ashfall.csproj", "--no-restore"], 180),
            (["bash", "scripts/ci/run-godot-bounded.sh", "--path", ".", "--", "--data-integrity-selftest"], 180),
            (["bash", "scripts/ci/run-godot-bounded.sh", "--path", ".", "--", "--bridge-selftest"], 180),
        ]
        for command, timeout in commands:
            record = command_output(command, timeout)
            if record["exit_code"] == 0:
                record["classification"] = "baseline_witness_pass"
            elif "generate-docs-index.py" in record["command"] and (BASELINE_JSON.exists() or BASELINE_MD.exists()):
                record["classification"] = "e1a_caused_artifact_drift"
                record["inherited"] = False
            else:
                record["classification"] = "inherited_failure"
                record["inherited"] = True
            evidence.append(record)

    generated_at = datetime.now(timezone.utc).replace(microsecond=0).isoformat().replace("+00:00", "Z")
    if BASELINE_JSON.is_file():
        try:
            previous = read_json(BASELINE_JSON)
            same_head = previous.get("repository", {}).get("head") == git_value("rev-parse", "HEAD")
            same_config = previous.get("config", {}).get("sha256") == sha256(config_path)
            same_paths = previous.get("namespaces") and stable_file_lists(previous["namespaces"], namespaces)
            if same_head and same_config and same_paths:
                generated_at = previous.get("generated_at_utc", generated_at)
        except (OSError, TypeError, KeyError):
            pass

    return {
        "schema_version": 1,
        "generated_by": "scripts/ci/generate-plan-register.py --baseline",
        "generated_at_utc": generated_at,
        "repository": {
            "head": git_value("rev-parse", "HEAD"),
            "branch": git_value("branch", "--show-current"),
            "dirty": bool(git_value("status", "--porcelain=v1")),
            "status_porcelain": git_value("status", "--porcelain=v1").splitlines(),
        },
        "toolchain": {
            "python": tool_version(["python3", "--version"]),
            "dotnet": tool_version(["dotnet", "--version"]),
            "godot": tool_version(["godot", "--version"]),
        },
        "config": {"path": rel(config_path), "sha256": sha256(config_path)},
        "counts": {"plans": counts, "documentation": {"total": len(documents), "categories": doc_counts}},
        "namespaces": namespaces,
        "documentation": documents,
        "verification": {
            "fast_gate_report": fast_gate_summary(gate_results),
            "witnesses": evidence,
        },
        "exclusions": {
            "runtime_paths_touched_by_e1": [],
            "historical_plan_bodies_rewritten": False,
        },
    }


def render_markdown(data: dict[str, Any]) -> str:
    counts = data["counts"]
    plan_counts = counts["plans"]
    doc_counts = counts["documentation"]["categories"]
    lines = [
        "# E1A Baseline — Ambition Governance & Expansion Intake",
        "",
        "<!-- GENERATED BY scripts/ci/generate-plan-register.py --baseline — DO NOT EDIT -->",
        "",
        f"- Generated (UTC): `{data['generated_at_utc']}`",
        f"- Repository HEAD: `{data['repository']['head']}`",
        f"- Branch: `{data['repository']['branch']}`",
        f"- Dirty at capture: `{data['repository']['dirty']}`",
        f"- Schema version: `{data['schema_version']}`",
        "",
        "## Plan namespace counts",
        "",
        "| Namespace | Files |",
        "|---|---:|",
    ]
    for namespace, count in plan_counts.items():
        lines.append(f"| `{namespace}` | {count} |")
    lines += [
        "",
        "## Documentation classification",
        "",
        "| Category | Files |",
        "|---|---:|",
    ]
    for category, count in doc_counts.items():
        lines.append(f"| `{category}` | {count} |")
    lines += [
        f"| **total** | **{counts['documentation']['total']}** |",
        "",
        "## Toolchain",
        "",
    ]
    for name, version in data["toolchain"].items():
        lines.append(f"- `{name}`: `{version}`")
    lines += [
        "",
        "## Gate baseline",
        "",
    ]
    fast = data["verification"].get("fast_gate_report")
    if fast:
        lines.append(
            f"Fast-tier report `{fast['source']}`: {fast.get('passed_count')} passed, "
            f"{fast.get('failed_count')} failed, all_passed=`{fast.get('all_passed')}`. "
            "Failures are inherited evidence captured before E1 implementation."
        )
        for failure in fast.get("failures", []):
            lines.append(
                f"- `{failure.get('gate_id')}` exit `{failure.get('exit_code')}`: "
                f"{failure.get('error_reason') or 'no reason reported'}"
            )
    else:
        lines.append("No fast-tier report was supplied; run `bash scripts/ci/verify-fast.sh` before E1A closeout.")
    lines += [
        "",
        "## Regression witnesses",
        "",
        "| Command | Exit | Classification |",
        "|---|---:|---|",
    ]
    for witness in data["verification"].get("witnesses", []):
        lines.append(
            f"| `{witness['command']}` | {witness['exit_code']} | `{witness.get('classification', 'unclassified')}` |"
        )
    lines += [
        "",
        "## Reproduction and raw evidence",
        "",
        f"- Machine-readable baseline: [`e1_baseline.json`](e1_baseline.json)",
        f"- Configuration: `{data['config']['path']}` (`{data['config']['sha256']}`)",
        "- Plan files include path, byte count, and SHA-256 in the JSON artifact.",
        "- The enumerator's `--self-test` performs two sorted scans and requires identical path lists.",
        "- E1 claims no runtime path changes; this baseline records the explicit empty runtime-change list.",
        "",
    ]
    return "\n".join(lines)


def stable_file_lists(first: dict[str, list[dict[str, Any]]], second: dict[str, list[dict[str, Any]]]) -> bool:
    def paths(value: dict[str, list[dict[str, Any]]]) -> dict[str, list[str]]:
        return {key: [row["path"] for row in rows] for key, rows in value.items()}

    return paths(first) == paths(second)


def main() -> int:
    parser = argparse.ArgumentParser(description="E1A baseline and future plan-register entry point")
    parser.add_argument("--baseline", action="store_true", help="capture or validate the E1A baseline")
    parser.add_argument("--write", action="store_true", help="write E1A baseline artifacts")
    parser.add_argument("--check", action="store_true", help="verify the existing baseline artifacts")
    parser.add_argument("--self-test", action="store_true", help="prove stable sorted enumeration")
    parser.add_argument("--config", type=Path, default=DEFAULT_CONFIG)
    parser.add_argument("--gate-results", type=Path, default=None)
    parser.add_argument("--run-witnesses", action="store_true", help="run the bounded E1A regression witnesses")
    args = parser.parse_args()

    if not (args.baseline or args.self_test):
        parser.error("E1A requires --baseline or --self-test")
    config_path = (REPO_ROOT / args.config).resolve() if not args.config.is_absolute() else args.config.resolve()
    config = read_json(config_path)

    if args.self_test:
        first = enumerate_files(config)
        second = enumerate_files(config)
        if not stable_file_lists(first, second):
            print("SELFTEST FAIL: consecutive plan enumerations differ", file=sys.stderr)
            return 1
        print("SELFTEST PASS: plan namespace enumeration is sorted and byte-stable")
        return 0

    gate_results = None
    if args.gate_results is not None:
        gate_results = args.gate_results if args.gate_results.is_absolute() else REPO_ROOT / args.gate_results
        gate_results = gate_results.resolve()
    first = build_baseline(config_path, gate_results, args.run_witnesses)
    second_namespaces = enumerate_files(config)
    if not stable_file_lists(first["namespaces"], second_namespaces):
        print("E1A FAIL: consecutive plan enumerations differ", file=sys.stderr)
        return 1

    if args.check:
        if not BASELINE_JSON.is_file() or not BASELINE_MD.is_file():
            print("E1A FAIL: baseline artifacts are missing; run --baseline --write", file=sys.stderr)
            return 1
        existing = read_json(BASELINE_JSON)
        if existing.get("counts") != first.get("counts"):
            print("E1A FAIL: current counts differ from the baseline artifact", file=sys.stderr)
            return 1
        for namespace in first["namespaces"]:
            if [row["path"] for row in existing["namespaces"].get(namespace, [])] != [row["path"] for row in first["namespaces"][namespace]]:
                print(f"E1A FAIL: namespace path list drifted: {namespace}", file=sys.stderr)
                return 1
        print("E1A PASS: baseline counts and sorted path lists are stable")
        return 0

    if args.write:
        BASELINE_DIR.mkdir(parents=True, exist_ok=True)
        BASELINE_JSON.write_text(json.dumps(first, indent=2, sort_keys=True) + "\n", encoding="utf-8")
        BASELINE_MD.write_text(render_markdown(first), encoding="utf-8")
        print(f"WROTE {rel(BASELINE_JSON)}")
        print(f"WROTE {rel(BASELINE_MD)}")
        print(json.dumps(first["counts"], sort_keys=True))
        return 0

    print(json.dumps(first, indent=2, sort_keys=True))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
