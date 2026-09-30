#!/usr/bin/env python3
"""One-time, body-preserving E1C migration of plan front matter.

Dry-run is the default. Review the proposal and its digest before applying it:
    python3 scripts/ci/migrate-plan-metadata.py --report-out /tmp/e1c-review.json
    python3 scripts/ci/migrate-plan-metadata.py --write --reviewed-digest <digest>

The script intentionally leaves uncertain identities/classifications in a
review queue. It never treats prose as evidence that work is DONE.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
import stat
import subprocess
import sys
import tempfile
from datetime import datetime, timezone
from pathlib import Path
from typing import Any

SCRIPT_DIR = Path(__file__).resolve().parent
REPO_ROOT = SCRIPT_DIR.parent.parent
sys.path.insert(0, str(SCRIPT_DIR))

from plan_corpus_lib import (  # noqa: E402
    CATEGORIES,
    STATUSES,
    enumerate_paths,
    parse_front_matter,
    sha256_bytes,
    stable_legacy_id,
)

CONFIG_PATH = SCRIPT_DIR / "plan_governance_config.json"
BASELINE_PATH = REPO_ROOT / "docs/roadmap/e1/e1_baseline.json"
EXECUTION_BASELINE_PATH = REPO_ROOT / "docs/roadmap/e1/E1C_EXECUTION_BASELINE.json"
REPORT_SCHEMA_VERSION = 1
STATUS_ALIASES = {
    "ACTIVE": "IN_PROGRESS",
    "COMPLETE": "BLOCKED",
    "COMPLETED": "BLOCKED",
    "SEALED": "BLOCKED",
    "SEALED-ELSEWHERE": "MERGED",
    "RETIRED": "ARCHIVED",
    "DEFERRED": "NOT_NOW",
    "OPEN": "PROPOSED",
    "READY_FOR_EXECUTION": "READY",
    "READY-UNCLAIMED": "READY",
}
CATEGORY_ALIASES = {
    "SYSTEMS": "SYSTEM",
    "UI": "PRESENTATION",
    "PRESENTATION-AUTHORITY": "PRESENTATION",
    "GOVERNANCE": "PROCESS",
}


def _line_ending(line: bytes) -> bytes:
    if line.endswith(b"\r\n"):
        return b"\r\n"
    if line.endswith(b"\n"):
        return b"\n"
    if line.endswith(b"\r"):
        return b"\r"
    return b""


def _head_block(data: bytes) -> tuple[int, int] | None:
    """Return byte offsets for the first front matter block only."""
    bom = b"\xef\xbb\xbf" if data.startswith(b"\xef\xbb\xbf") else b""
    start = len(bom)
    lines = data[start:].splitlines(keepends=True)
    if not lines or lines[0].rstrip(b"\r\n") != b"---":
        return None
    offset = start + len(lines[0])
    for line in lines[1:]:
        next_offset = offset + len(line)
        if line.rstrip(b"\r\n") == b"---":
            return start, next_offset
        offset = next_offset
    raise ValueError("front matter opens at byte 0 but has no closing delimiter")


def _front_fields(block: bytes) -> tuple[dict[str, Any], list[str]]:
    text = block.decode("utf-8-sig")
    fields, errors, _warnings = parse_front_matter(text)
    if fields is None:
        return {}, ["front matter parser did not recognize the head block"]
    return fields, errors


def _first_title(body: bytes) -> str | None:
    text = body.decode("utf-8-sig")
    for line in text.splitlines():
        match = re.match(r"^#\s+(.+?)\s*#*\s*$", line)
        if match:
            return match.group(1).strip()
    return None


def _candidate_id(relative: str, namespace: str) -> str | None:
    stem = Path(relative).stem
    match = re.match(r"^(C[12]|E[12]|D[12])_planintegration(?:\[(\d+)\])?$", stem, re.I)
    if match:
        family = match.group(1).upper()
        number = match.group(2)
        return family if number is None else f"{family}-{int(number)}"
    match = re.match(r"^Plan_(\d+)(?:_|$)", stem, re.I)
    if match:
        return f"NEXT-PLAN-{int(match.group(1))}"
    match = re.match(r"^(\d+)-", stem)
    if match and namespace == "piagents":
        return f"PIA-{int(match.group(1))}"
    match = re.search(r"(?:QUALITY_ROADMAP_)?BATCH_?(\d+)$", stem, re.I)
    if match and namespace == "integration":
        prefix = "INT-ROADMAP-BATCH" if "ROADMAP" in stem.upper() else "INT-BATCH"
        return f"{prefix}-{int(match.group(1))}"
    return None


def _namespace(relative: str, config: dict[str, Any]) -> str:
    for entry in config.get("plan_namespaces", []):
        root = str(entry["root"]).rstrip("/") + "/"
        if relative.startswith(root):
            return str(entry.get("name", entry["root"]))
    return "unknown"


def _title_and_id(path: Path, relative: str, namespace: str, fields: dict[str, Any], body: bytes) -> tuple[str, str, list[str]]:
    review: list[str] = []
    title = str(fields.get("TITLE") or "").strip()
    if not title:
        title = _first_title(body) or ""
    if not title:
        title = Path(relative).stem.replace("_", " ").replace("-", " ").strip()
        review.append("TITLE needs human review: no unambiguous top-level heading")
    plan_id = str(fields.get("PLAN_ID") or "").strip()
    if not plan_id:
        candidate = _candidate_id(relative, namespace)
        if candidate:
            plan_id = candidate
        else:
            plan_id = stable_legacy_id(relative)
            review.append("PLAN_ID uses stable path identity; filename candidate is ambiguous")
    return title, plan_id, review


def _category(raw: Any) -> tuple[str, str | None]:
    value = str(raw or "").strip().upper()
    if value in CATEGORIES:
        return value, None
    if value in CATEGORY_ALIASES:
        return CATEGORY_ALIASES[value], None
    # Preserve the source spelling in the report, but do not reduce a compound
    # or domain-only value to a gameplay category without a human review.
    return "PROCESS", "CATEGORY needs human review: source is missing, compound, or noncanonical"


def _status(raw: Any, fields: dict[str, Any]) -> tuple[str, str | None]:
    value = str(raw or "").strip().upper()
    if value in STATUSES:
        if value == "DONE" and (not fields.get("COMPLETED_AT") or not fields.get("COMPLETION_EVIDENCE")):
            return "BLOCKED", "explicit DONE lacks completion date/evidence; review before changing status"
        return value, None
    if value in STATUS_ALIASES:
        mapped = STATUS_ALIASES[value]
        if mapped == "BLOCKED" and value in {"COMPLETE", "COMPLETED", "SEALED"}:
            return mapped, "completion-like alias lacks canonical completion evidence; review required"
        return mapped, f"STATUS alias canonicalized from {value} to {mapped}"
    if value.startswith(("READY_WHEN_", "READY_FOR_EXECUTION_WHEN_", "READY_FOR_EXECUTION_AFTER_")):
        return "BLOCKED", "conditional readiness retained as BLOCKED; verify named prerequisite"
    if not value:
        return "PROPOSED", "STATUS absent; conservative PROPOSED default requires review"
    return "PROPOSED", f"unrecognized STATUS {value!r}; preserved in report for review"


def _yaml_scalar(value: str) -> str:
    return json.dumps(value, ensure_ascii=False)


def _namespace_root(relative: str) -> str:
    return relative.split("/", 1)[0]


def _apply_front_matter(data: bytes, relative: str, fields: dict[str, Any], values: dict[str, Any]) -> bytes:
    block_span = _head_block(data)
    bom = b"\xef\xbb\xbf" if data.startswith(b"\xef\xbb\xbf") else b""
    if block_span:
        start, end = block_span
        block = data[start:end]
        ending = b"\r\n" if b"\r\n" in block else b"\n"
        block_lines = block.splitlines(keepends=True)
        close_index = next(index for index in range(len(block_lines) - 1, 0, -1) if block_lines[index].rstrip(b"\r\n") == b"---")
        present: dict[str, int] = {}
        for index, line in enumerate(block_lines[1:close_index], start=1):
            match = re.match(rb"^([A-Za-z][A-Za-z0-9_]*)\s*:", line)
            if match:
                present[match.group(1).decode("ascii")] = index
        for key, value in values.items():
            if key not in present:
                continue
            if key not in {"PLAN_SCHEMA_VERSION", "PLAN_ID", "TITLE", "STATUS", "CATEGORY", "NAMESPACE", "INFERRED"}:
                continue
            idx = present[key]
            line_ending = _line_ending(block_lines[idx]) or ending
            scalar = "true" if value is True else "false" if value is False else _yaml_scalar(str(value))
            block_lines[idx] = f"{key}: {scalar}".encode("utf-8") + line_ending
        additions: list[bytes] = []
        for key, value in values.items():
            if key in present:
                continue
            if value is None:
                rendered = "null"
            elif isinstance(value, bool):
                rendered = "true" if value else "false"
            elif isinstance(value, list):
                rendered = "[]" if not value else "[" + ", ".join(_yaml_scalar(str(item)) for item in value) + "]"
            else:
                rendered = _yaml_scalar(str(value))
            additions.append(f"{key}: {rendered}".encode("utf-8") + ending)
        block_lines[close_index:close_index] = additions
        new_block = b"".join(block_lines)
        return bom + data[len(bom):start] + new_block + data[end:]

    ending = b"\r\n" if b"\r\n" in data else b"\n"
    rows = [b"---" + ending]
    for key, value in values.items():
        if value is None:
            rendered = "null"
        elif isinstance(value, bool):
            rendered = "true" if value else "false"
        elif isinstance(value, list):
            rendered = "[]" if not value else "[" + ", ".join(_yaml_scalar(str(item)) for item in value) + "]"
        else:
            rendered = _yaml_scalar(str(value))
        rows.append(f"{key}: {rendered}".encode("utf-8") + ending)
    rows.append(b"---" + ending)
    return bom + b"".join(rows) + data[len(bom):]


def _proposal(path: Path, repo_root: Path, config: dict[str, Any]) -> dict[str, Any]:
    relative = path.relative_to(repo_root).as_posix()
    before = path.read_bytes()
    span = _head_block(before)
    if span:
        start, end = span
        block = before[start:end]
        fields, parse_errors = _front_fields(block)
        body = before[end:]
    else:
        fields, parse_errors = {}, []
        body = before[len(b"\xef\xbb\xbf"):] if before.startswith(b"\xef\xbb\xbf") else before
    namespace = _namespace(relative, config)
    title, plan_id, review = _title_and_id(path, relative, namespace, fields, body)
    raw_status = fields.get("STATUS")
    status, status_review = _status(raw_status, fields)
    if status_review:
        review.append(status_review)
    raw_category = fields.get("CATEGORY")
    category, category_review = _category(raw_category)
    if category_review:
        review.append(category_review)
    if parse_errors:
        review.extend(f"front matter parse issue: {error}" for error in parse_errors)
    inferred = bool(review) or not bool(fields.get("INFERRED", False)) and span is None
    values: dict[str, Any] = {
        "PLAN_SCHEMA_VERSION": 1,
        "PLAN_ID": plan_id,
        "TITLE": title,
        "STATUS": status,
        "CATEGORY": category,
        "NAMESPACE": namespace,
        "PREMISE_VERIFIED_AT": None,
        "PREMISE_VERIFIED_DATE": None,
        "OWNER": None,
        "PILLARS": [],
        "RAILS_REQUIRED": [],
        "METRIC_MOVED": [],
        "ACCEPTANCE_TIER": None,
        "SOURCE_AUTHORITY": [],
        "SUPERSEDES": [],
        "SUPERSEDED_BY": [],
        "MERGED_INTO": None,
        "COMPLETED_AT": fields.get("COMPLETED_AT"),
        "COMPLETION_EVIDENCE": fields.get("COMPLETION_EVIDENCE", []),
        "LEGACY_SOURCE_PATH": relative,
        "INFERRED": bool(fields.get("INFERRED", False)) or inferred,
    }
    # Keep explicit source metadata, including noncanonical values, in the
    # migration report; replace only the five canonicalized head scalars.
    after = _apply_front_matter(before, relative, fields, values)
    after_span = _head_block(after)
    assert after_span is not None
    after_body = after[after_span[1]:]
    return {
        "path": relative,
        "before_sha256": sha256_bytes(before),
        "after_sha256": sha256_bytes(after),
        "body_sha256_before": sha256_bytes(body),
        "body_sha256_after": sha256_bytes(after_body),
        "changed": before != after,
        "original_front_matter": fields,
        "proposed": {key: values[key] for key in ("PLAN_SCHEMA_VERSION", "PLAN_ID", "TITLE", "STATUS", "CATEGORY", "NAMESPACE", "INFERRED")},
        "review": sorted(set(review)),
        "_bytes": after,
    }


def build_report(repo_root: Path, config: dict[str, Any], baseline_path: Path) -> dict[str, Any]:
    baseline = json.loads(baseline_path.read_text(encoding="utf-8"))
    paths = enumerate_paths(repo_root, config)
    current = [path.relative_to(repo_root).as_posix() for path in paths]
    frozen = list(baseline.get("plan_file_list", []))
    missing = sorted(set(frozen) - set(current))
    added = sorted(set(current) - set(frozen))
    rows = [_proposal(path, repo_root, config) for path in paths]
    # A basename can legitimately occur in more than one namespace (for
    # example, an original plan and its shipped copy). Never mint the same
    # PLAN_ID for both files: keep the readable candidate only when unique,
    # otherwise fall back to the stable path identity and queue human review.
    owners: dict[str, list[str]] = {}
    for row in rows:
        owners.setdefault(row["proposed"]["PLAN_ID"], []).append(row["path"])
    collisions = {plan_id: sorted(items) for plan_id, items in owners.items() if len(items) > 1}
    for items in collisions.values():
        for relative in items:
            row = next(candidate for candidate in rows if candidate["path"] == relative)
            row["proposed"]["PLAN_ID"] = stable_legacy_id(relative)
            row["review"] = sorted(set(row["review"] + [
                "PLAN_ID filename candidate collides across corpus paths; stable path identity proposed"
            ]))
            row["proposed"]["INFERRED"] = True
            span = _head_block(row["_bytes"])
            if span is None:
                raise ValueError(f"collision fallback lost front matter: {relative}")
            fields, errors = _front_fields(row["_bytes"][span[0]:span[1]])
            if errors:
                raise ValueError(f"collision fallback encountered malformed front matter in {relative}: {errors}")
            row["_bytes"] = _apply_front_matter(
                row["_bytes"], relative, fields, {"PLAN_ID": row["proposed"]["PLAN_ID"], "INFERRED": True}
            )
            after_span = _head_block(row["_bytes"])
            assert after_span is not None
            row["after_sha256"] = sha256_bytes(row["_bytes"])
            row["body_sha256_after"] = sha256_bytes(row["_bytes"][after_span[1]:])
            row["changed"] = row["before_sha256"] != row["after_sha256"]
    final_owners: dict[str, list[str]] = {}
    for row in rows:
        final_owners.setdefault(row["proposed"]["PLAN_ID"], []).append(row["path"])
    duplicates = {plan_id: sorted(items) for plan_id, items in final_owners.items() if len(items) > 1}
    digest_rows = [{key: value for key, value in row.items() if key != "_bytes"} for row in rows]
    canonical = json.dumps(digest_rows, ensure_ascii=False, sort_keys=True, separators=(",", ":")).encode("utf-8")
    digest = hashlib.sha256(canonical).hexdigest()
    body_mismatches = [row["path"] for row in rows if row["body_sha256_before"] != row["body_sha256_after"]]
    inferred_done = [row["path"] for row in rows if row["proposed"]["STATUS"] == "DONE" and row["proposed"]["INFERRED"]]
    expected_by_path = {
        str(item["path"]): str(item.get("expected_sha256", item.get("source_sha256", "")))
        for item in baseline.get("execution_files", [])
    }
    proposal_by_path = {row["path"]: row for row in rows}
    source_mismatches = sorted(
        relative for relative, expected in expected_by_path.items()
        if relative in proposal_by_path and proposal_by_path[relative]["before_sha256"] != expected
    )
    return {
        "schema_version": REPORT_SCHEMA_VERSION,
        "generated_by": "scripts/ci/migrate-plan-metadata.py",
        "migration_digest": digest,
        "baseline_head": baseline.get("repository", {}).get("head", baseline.get("repository_head")),
        "baseline_plan_count": len(frozen),
        "current_plan_count": len(current),
        "baseline_paths_missing": missing,
        "current_paths_not_in_baseline": added,
        "path_parity": not missing and not added,
        "body_hash_mismatches": body_mismatches,
        "execution_baseline_source_mismatches": source_mismatches,
        "e1a_reconciliation": baseline.get("e1a_reconciliation", {}),
        "inferred_done": inferred_done,
        "duplicate_plan_ids": duplicates,
        "changed_count": sum(1 for row in rows if row["changed"]),
        "review_count": sum(1 for row in rows if row["review"]),
        "rows": digest_rows,
        "_proposals": rows,
    }


def _atomic_write(path: Path, data: bytes) -> None:
    mode = stat.S_IMODE(path.stat().st_mode) if path.exists() else 0o644
    path.parent.mkdir(parents=True, exist_ok=True)
    fd, temp_name = tempfile.mkstemp(prefix=f".{path.name}.", dir=path.parent)
    try:
        with os.fdopen(fd, "wb") as handle:
            handle.write(data)
            handle.flush()
            os.fsync(handle.fileno())
        os.chmod(temp_name, mode)
        os.replace(temp_name, path)
    finally:
        if os.path.exists(temp_name):
            os.unlink(temp_name)


def _write_execution_expected_hashes(baseline_path: Path, report: dict[str, Any]) -> None:
    baseline = json.loads(baseline_path.read_text(encoding="utf-8"))
    after_by_path = {row["path"]: row["after_sha256"] for row in report["rows"]}
    for entry in baseline.get("execution_files", []):
        if entry["path"] in after_by_path:
            entry["expected_sha256"] = after_by_path[entry["path"]]
    _atomic_write(
        baseline_path,
        (json.dumps(baseline, ensure_ascii=False, indent=2, sort_keys=True) + "\n").encode("utf-8"),
    )


def render_markdown_report(report: dict[str, Any]) -> str:
    e1a = report.get("e1a_reconciliation", {})
    e1a_resolution_count = len(e1a.get("missing_path_resolutions", []))
    lines = [
        "# E1C Plan Metadata Migration Report",
        "",
        f"- Migration digest: `{report['migration_digest']}`",
        f"- Execution baseline head: `{report.get('baseline_head') or 'unavailable'}`",
        f"- Current paths: {report['current_plan_count']} (baseline {report['baseline_plan_count']}; path parity `{str(report['path_parity']).lower()}`)",
        f"- E1A reconciliation: {e1a.get('paths_still_present', 'n/a')} retained, {len(e1a.get('historical_paths_missing', []))} historical paths missing, {len(e1a.get('current_paths_added_since_e1a', []))} current paths added, {e1a_resolution_count} historical candidates traced.",
        f"- Proposed changes: {report['changed_count']}; review queue: {report['review_count']}; duplicate IDs: {len(report['duplicate_plan_ids'])}; inferred `DONE`: {len(report['inferred_done'])}.",
        f"- Body hash mismatches: {len(report['body_hash_mismatches'])}; execution source drift: {len(report['execution_baseline_source_mismatches'])}.",
        "",
        "The JSON sibling contains the complete per-path record: original metadata, proposed fields, review findings, pre/post file hashes, and body hashes. `INFERRED: true` marks proposals that still require human review. Historical missing-path candidates are reported with both historical and current hashes; non-identical copies are not presented as byte-preserving renames.",
        "",
        "| Plan path | Proposed ID | Status | Category | Review findings | Body SHA-256 |",
        "|---|---|---|---|---:|---|",
    ]
    for row in report["rows"]:
        proposed = row["proposed"]
        review_count = len(row["review"])
        path = row["path"].replace("|", "\\|")
        plan_id = str(proposed["PLAN_ID"]).replace("|", "\\|")
        body_hash = row["body_sha256_before"]
        lines.append(f"| `{path}` | `{plan_id}` | {proposed['STATUS']} | {proposed['CATEGORY']} | {review_count} | `{body_hash}` |")
    return "\n".join(lines) + "\n"


def finalize_report(
    proposal_report: dict[str, Any],
    post_write_report: dict[str, Any],
    prior_report_path: Path | None = None,
) -> dict[str, Any]:
    source_report = json.loads(prior_report_path.read_text(encoding="utf-8")) if prior_report_path else proposal_report
    final_by_path = {row["path"]: row for row in post_write_report["rows"]}
    rows = [dict(row) for row in source_report["rows"]]
    for row in rows:
        final = final_by_path[row["path"]]
        row["after_sha256"] = final["before_sha256"]
        row["body_sha256_after"] = final["body_sha256_before"]
        row["proposed"] = final["proposed"]
        row["changed"] = row["before_sha256"] != row["after_sha256"]
    canonical = json.dumps(rows, ensure_ascii=False, sort_keys=True, separators=(",", ":")).encode("utf-8")
    result = dict(source_report)
    result.update({
        "migration_digest": hashlib.sha256(canonical).hexdigest(),
        "reviewed_proposal_digest": proposal_report["migration_digest"],
        "rows": rows,
        "path_parity": post_write_report["path_parity"],
        "body_hash_mismatches": [row["path"] for row in rows if row["body_sha256_before"] != row["body_sha256_after"]],
        "execution_baseline_source_mismatches": post_write_report["execution_baseline_source_mismatches"],
        "duplicate_plan_ids": post_write_report["duplicate_plan_ids"],
        "inferred_done": [row["path"] for row in rows if row["proposed"]["STATUS"] == "DONE" and row["proposed"]["INFERRED"]],
        "changed_count": sum(1 for row in rows if row["changed"]),
    })
    result["review_count"] = sum(1 for row in rows if row["review"])
    return result


def _historical_blob(repo_root: Path, revision: str, relative: str) -> tuple[str | None, str | None]:
    spec = f"{revision}:{relative}"
    blob = subprocess.run(
        ["git", "rev-parse", "--verify", spec], cwd=repo_root, capture_output=True, text=True
    )
    if blob.returncode != 0:
        return None, None
    content = subprocess.run(["git", "show", spec], cwd=repo_root, capture_output=True)
    if content.returncode != 0:
        return blob.stdout.strip(), None
    return blob.stdout.strip(), sha256_bytes(content.stdout)


def capture_execution_baseline(repo_root: Path, config: dict[str, Any], e1a_path: Path) -> dict[str, Any]:
    e1a = json.loads(e1a_path.read_text(encoding="utf-8"))
    e1a_paths = sorted(set(e1a.get("plan_file_list", [])))
    live_paths = enumerate_paths(repo_root, config)
    by_relative = {path.relative_to(repo_root).as_posix(): path for path in live_paths}
    current_paths = sorted(by_relative)
    e1a_set = set(e1a_paths)
    current_set = set(current_paths)
    missing = sorted(e1a_set - current_set)
    added = sorted(current_set - e1a_set)
    historical: list[dict[str, Any]] = []
    e1a_head = e1a.get("repository", {}).get("head")
    for relative in missing:
        replacement = relative.replace("Next-steps-plans/", "Next-steps-plans/shipped_to_chat/", 1)
        live_replacement = by_relative.get(replacement)
        historical_blob, historical_sha = _historical_blob(repo_root, str(e1a_head), relative) if e1a_head else (None, None)
        live_sha = sha256_bytes(live_replacement.read_bytes()) if live_replacement else None
        historical.append({
            "historical_path": relative,
            "historical_git_blob": historical_blob,
            "historical_sha256": historical_sha,
            "current_candidate_path": replacement if live_replacement else None,
            "current_candidate_sha256": live_sha,
            "content_matches_historical": bool(historical_sha and live_sha and historical_sha == live_sha),
            "resolution": "REPLACED_COPY_CONTENT_DIFFERS" if live_replacement and historical_sha != live_sha else (
                "MOVED_COPY_IDENTICAL" if live_replacement else "UNRESOLVED_MISSING_PATH"
            ),
        })
    head = subprocess.run(["git", "rev-parse", "HEAD"], cwd=repo_root, capture_output=True, text=True)
    now = datetime.now(timezone.utc).replace(microsecond=0).isoformat().replace("+00:00", "Z")
    return {
        "schema_version": 1,
        "generated_by": "scripts/ci/migrate-plan-metadata.py --capture-current-baseline",
        "captured_at_utc": now,
        "repository_head": head.stdout.strip() if head.returncode == 0 else None,
        "e1a_baseline_path": e1a_path.relative_to(repo_root).as_posix(),
        "e1a_baseline_head": e1a_head,
        "e1a_baseline_plan_count": len(e1a_paths),
        "plan_count": len(current_paths),
        "plan_file_list": current_paths,
        "execution_files": [
            {"path": relative, "source_sha256": sha256_bytes(by_relative[relative].read_bytes()),
             "expected_sha256": sha256_bytes(by_relative[relative].read_bytes())}
            for relative in current_paths
        ],
        "e1a_reconciliation": {
            "paths_still_present": len(e1a_set & current_set),
            "historical_paths_missing": missing,
            "current_paths_added_since_e1a": added,
            "missing_path_resolutions": historical,
            "unresolved_missing_paths": [row["historical_path"] for row in historical if row["resolution"] == "UNRESOLVED_MISSING_PATH"],
        },
    }


def self_test() -> None:
    import tempfile

    with tempfile.TemporaryDirectory(prefix="e1c-migration-") as temp:
        root = Path(temp)
        for folder in ("Next-steps-plans", "Next-steps-plans/shipped_to_chat", "piagentsplans", "C-integration-plans"):
            (root / folder).mkdir()
        crlf_body = b"# Exact Heading\r\n\r\nBody --- stays\r\n```yaml\r\nSTATUS: DONE\r\n```\r\n"
        (root / "Next-steps-plans/Plan_7_example.md").write_bytes(crlf_body)
        (root / "Next-steps-plans/shipped_to_chat/Plan_7_example.md").write_bytes(crlf_body)
        existing_body = b"# Child\n\nExample follows:\n---\nSTATUS: READY\n---\n"
        (root / "C-integration-plans/E1_planintegration[2].md").write_bytes(
            b"---\r\nPLAN_ID: E1-2\r\nSTATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS\r\nCATEGORY: LINK+VOICE\r\n"
            b"OWNER: foreman\r\nPREMISE_VERIFIED_AT: ccac926e\r\nPILLARS: [scarcity-information]\r\n"
            b"UNKNOWN_KEY: keep-me\r\n---\r\n" + existing_body
        )
        (root / "piagentsplans/ambiguous.md").write_bytes(b"no heading\ntext\n")
        config = {"plan_namespaces": [
            {"name": "next_steps", "root": "Next-steps-plans", "include": ["**/*.md"]},
            {"name": "piagents", "root": "piagentsplans", "include": ["**/*.md"]},
            {"name": "integration", "root": "C-integration-plans", "include": ["**/*.md"]},
        ]}
        baseline = {"repository": {"head": "fixture"}, "plan_file_list": sorted([
            "Next-steps-plans/Plan_7_example.md", "Next-steps-plans/shipped_to_chat/Plan_7_example.md",
            "C-integration-plans/E1_planintegration[2].md", "piagentsplans/ambiguous.md"
        ]), "execution_files": [
            {"path": "Next-steps-plans/Plan_7_example.md", "source_sha256": sha256_bytes(crlf_body), "expected_sha256": sha256_bytes(crlf_body)},
            {"path": "Next-steps-plans/shipped_to_chat/Plan_7_example.md", "source_sha256": sha256_bytes(crlf_body), "expected_sha256": sha256_bytes(crlf_body)},
        ]}
        baseline_path = root / "baseline.json"
        baseline_path.write_text(json.dumps(baseline), encoding="utf-8")
        report = build_report(root, config, baseline_path)
        by_path = {row["path"]: row for row in report["_proposals"]}
        assert report["path_parity"] and report["changed_count"] == 4
        assert not report["duplicate_plan_ids"]
        assert not report["execution_baseline_source_mismatches"]
        assert not report["body_hash_mismatches"] and not report["inferred_done"]
        assert by_path["Next-steps-plans/Plan_7_example.md"]["_bytes"].endswith(crlf_body)
        assert by_path["Next-steps-plans/Plan_7_example.md"]["proposed"]["PLAN_ID"] != by_path["Next-steps-plans/shipped_to_chat/Plan_7_example.md"]["proposed"]["PLAN_ID"]
        assert any("collides across corpus paths" in issue for issue in by_path["Next-steps-plans/Plan_7_example.md"]["review"])
        root_span = _head_block(by_path["Next-steps-plans/Plan_7_example.md"]["_bytes"])
        assert root_span is not None
        written_fields, _ = _front_fields(by_path["Next-steps-plans/Plan_7_example.md"]["_bytes"][root_span[0]:root_span[1]])
        assert written_fields["PLAN_ID"] == by_path["Next-steps-plans/Plan_7_example.md"]["proposed"]["PLAN_ID"]
        assert b"UNKNOWN_KEY: keep-me" in by_path["C-integration-plans/E1_planintegration[2].md"]["_bytes"]
        assert b"OWNER: foreman\r\nPREMISE_VERIFIED_AT: ccac926e\r\nPILLARS: [scarcity-information]\r\n" in by_path["C-integration-plans/E1_planintegration[2].md"]["_bytes"]
        assert by_path["C-integration-plans/E1_planintegration[2].md"]["proposed"]["STATUS"] == "BLOCKED"
        assert by_path["C-integration-plans/E1_planintegration[2].md"]["proposed"]["PLAN_ID"] == "E1-2"
        assert by_path["C-integration-plans/E1_planintegration[2].md"]["_bytes"].endswith(existing_body)
        assert by_path["piagentsplans/ambiguous.md"]["review"]
        assert _candidate_id("C-integration-plans/C1_planintegration[2].md", "integration") == "C1-2"
        assert _candidate_id("C-integration-plans/D1_planintegration[2].md", "integration") == "D1-2"
        second = _proposal_from_bytes_for_test(by_path["Next-steps-plans/Plan_7_example.md"]["_bytes"], root / "Next-steps-plans/Plan_7_example.md", root, config)
        assert not second["changed"]


def _proposal_from_bytes_for_test(data: bytes, path: Path, repo_root: Path, config: dict[str, Any]) -> dict[str, Any]:
    previous = path.read_bytes()
    try:
        path.write_bytes(data)
        return _proposal(path, repo_root, config)
    finally:
        path.write_bytes(previous)


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--write", action="store_true", help="apply a reviewed migration proposal")
    parser.add_argument("--reviewed-digest", help="exact migration_digest printed by the reviewed dry run")
    parser.add_argument("--report-out", type=Path, help="write the machine-readable proposal/report")
    parser.add_argument("--report-markdown-out", type=Path, help="write the human-readable migration report")
    parser.add_argument("--prior-report", type=Path, help="preserve an earlier proposal's source hashes and review queue while finalizing a corrective write")
    parser.add_argument("--config", type=Path, default=CONFIG_PATH)
    parser.add_argument("--baseline", type=Path, default=EXECUTION_BASELINE_PATH)
    parser.add_argument("--capture-current-baseline", action="store_true", help="capture current paths and source hashes against historical E1A")
    parser.add_argument("--repo-root", type=Path, default=REPO_ROOT, help=argparse.SUPPRESS)
    parser.add_argument("--self-test", action="store_true")
    args = parser.parse_args()
    if args.self_test:
        self_test()
        print("E1C migration self-test PASS")
        return 0
    config_path = args.config if args.config.is_absolute() else args.repo_root / args.config
    baseline_path = args.baseline if args.baseline.is_absolute() else args.repo_root / args.baseline
    config = json.loads(config_path.read_text(encoding="utf-8"))
    if args.capture_current_baseline:
        if baseline_path.exists():
            print(f"refusing to overwrite execution baseline: {baseline_path}", file=sys.stderr)
            return 2
        e1a_path = args.repo_root / BASELINE_PATH.relative_to(REPO_ROOT)
        execution_baseline = capture_execution_baseline(args.repo_root, config, e1a_path)
        _atomic_write(
            baseline_path,
            (json.dumps(execution_baseline, ensure_ascii=False, indent=2, sort_keys=True) + "\n").encode("utf-8"),
        )
        reconciliation = execution_baseline["e1a_reconciliation"]
        print(
            f"captured={execution_baseline['plan_count']} e1a={execution_baseline['e1a_baseline_plan_count']} "
            f"missing={len(reconciliation['historical_paths_missing'])} added={len(reconciliation['current_paths_added_since_e1a'])} "
            f"unresolved={len(reconciliation['unresolved_missing_paths'])} path={baseline_path}"
        )
        return 0 if not reconciliation["unresolved_missing_paths"] else 3
    report = build_report(args.repo_root, config, baseline_path)
    digest = report["migration_digest"]
    print(f"plans={report['current_plan_count']} changed={report['changed_count']} review={report['review_count']} digest={digest}")
    if not report["path_parity"]:
        print(f"baseline path mismatch: missing={len(report['baseline_paths_missing'])} added={len(report['current_paths_not_in_baseline'])}", file=sys.stderr)
        return 2
    if report["execution_baseline_source_mismatches"]:
        print(f"execution baseline source drift: {len(report['execution_baseline_source_mismatches'])} paths changed", file=sys.stderr)
        return 2
    if report["body_hash_mismatches"] or report["inferred_done"] or report["duplicate_plan_ids"]:
        print("proposal violates a migration invariant; see report", file=sys.stderr)
        return 2
    if args.write:
        if not args.reviewed_digest or args.reviewed_digest != digest:
            print("--write requires --reviewed-digest matching the reviewed dry-run proposal", file=sys.stderr)
            return 2
        for row in report["_proposals"]:
            if row["changed"]:
                _atomic_write(args.repo_root / row["path"], row["_bytes"])
        _write_execution_expected_hashes(baseline_path, report)
        # Re-read every file and prove both no-op and exact body parity.
        after_report = build_report(args.repo_root, config, baseline_path)
        if after_report["changed_count"] != 0 or after_report["execution_baseline_source_mismatches"]:
            print("post-write migration is not idempotent or execution baseline is out of sync", file=sys.stderr)
            return 3
        prior_report_path = args.prior_report
        if prior_report_path and not prior_report_path.is_absolute():
            prior_report_path = args.repo_root / prior_report_path
        report = finalize_report(report, after_report, prior_report_path)
        print("write PASS; second-run proposal is a no-op")
    if args.report_out:
        output = {key: value for key, value in report.items() if key != "_proposals"}
        output_path = args.report_out if args.report_out.is_absolute() else args.repo_root / args.report_out
        output_path.write_text(json.dumps(output, ensure_ascii=False, indent=2, sort_keys=True) + "\n", encoding="utf-8")
    if args.report_markdown_out:
        markdown_path = args.report_markdown_out if args.report_markdown_out.is_absolute() else args.repo_root / args.report_markdown_out
        markdown_path.write_text(render_markdown_report(report), encoding="utf-8", newline="")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
