#!/usr/bin/env python3
"""Dependency-free parsing and reference helpers for plan governance."""

from __future__ import annotations

import hashlib
import json
import re
import subprocess
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path
from typing import Any, Iterable

REGISTER_SCHEMA_VERSION = 1
METADATA_SCHEMA_VERSION = 1
STATUSES = ("PROPOSED", "PREMISE_STALE", "READY", "IN_PROGRESS", "BLOCKED", "DONE", "MERGED", "NOT_NOW", "DROPPED", "SUPERSEDED", "ARCHIVED")
CATEGORIES = ("SYSTEM", "LINK", "CONTENT", "PRESENTATION", "PROCESS")
GENERATED_STATUSES = ("METADATA_MISSING",)
STATUS_ALIASES = {
    "READY_FOR_EXECUTION": "READY", "READY-UNCLAIMED": "READY", "OPEN": "PROPOSED",
    "ACTIVE": "IN_PROGRESS", "COMPLETE": "DONE", "COMPLETED": "DONE", "SEALED": "DONE",
    "SEALED-ELSEWHERE": "MERGED", "RETIRED": "ARCHIVED", "DEFERRED": "NOT_NOW",
}
CATEGORY_ALIASES = {"SYSTEMS": "SYSTEM", "UI": "PRESENTATION", "PRESENTATION-AUTHORITY": "PRESENTATION", "GOVERNANCE": "PROCESS"}


@dataclass(frozen=True)
class ReferenceVerdict:
    reference: str
    status: str
    severity: str
    detail: str

    def as_dict(self) -> dict[str, str]:
        return {"reference": self.reference, "status": self.status, "severity": self.severity, "detail": self.detail}


def sha256_bytes(value: bytes) -> str:
    return hashlib.sha256(value).hexdigest()


def sha256_file(path: Path) -> str:
    return sha256_bytes(path.read_bytes())


def _unquote(value: str) -> str:
    value = value.strip()
    if len(value) >= 2 and value[0] == value[-1] and value[0] in "\"'":
        if value[0] == '"':
            try:
                return str(json.loads(value))
            except json.JSONDecodeError:
                pass
        return value[1:-1]
    return value


def _parse_value(value: str) -> Any:
    value = value.strip()
    if not value:
        return ""
    if value.startswith("[") and value.endswith("]"):
        inner = value[1:-1].strip()
        return [] if not inner else [_parse_value(part) for part in re.split(r"\s*,\s*", inner)]
    if value.lower() in {"true", "false"}:
        return value.lower() == "true"
    if value.lower() in {"null", "~"}:
        return None
    return _unquote(value)


def parse_front_matter(text: str) -> tuple[dict[str, Any] | None, list[str], list[str]]:
    lines = text.splitlines()
    if not lines or lines[0].strip() != "---":
        return None, [], []
    end = next((index for index in range(1, len(lines)) if lines[index].strip() == "---"), None)
    if end is None:
        return {}, ["front matter starts with '---' but has no closing '---'"], []
    fields: dict[str, Any] = {}
    errors: list[str] = []
    warnings: list[str] = []
    list_key: str | None = None
    for line_number, line in enumerate(lines[1:end], start=2):
        stripped = line.strip()
        if not stripped or stripped.startswith("#"):
            continue
        list_match = re.match(r"^[-*]\s+(.*)$", stripped)
        if list_match and list_key:
            current = fields.setdefault(list_key, [])
            if not isinstance(current, list):
                errors.append(f"line {line_number}: list item follows scalar field {list_key}")
            else:
                current.append(_parse_value(list_match.group(1)))
            continue
        match = re.match(r"^([A-Za-z][A-Za-z0-9_]*)\s*:\s*(.*)$", stripped)
        if not match:
            errors.append(f"line {line_number}: expected KEY: VALUE")
            list_key = None
            continue
        key, value = match.groups()
        if key in fields:
            errors.append(f"line {line_number}: duplicate field {key}")
        if not value:
            fields[key] = []
            list_key = key
        else:
            fields[key] = _parse_value(value)
            list_key = None
    return fields, errors, warnings


def _as_list(value: Any) -> list[str]:
    if value is None or value == "":
        return []
    if isinstance(value, list):
        return [str(item) for item in value if item is not None and str(item).strip()]
    return [str(value)]


def _normalize_status(raw: Any, warnings: list[str], errors: list[str], path: str) -> str:
    value = str(raw).strip().upper() if raw not in (None, "") else ""
    if value in STATUSES:
        return value
    if value in STATUS_ALIASES:
        normalized = STATUS_ALIASES[value]
        warnings.append(f"{path}: STATUS alias {value!r} normalized to {normalized!r}")
        return normalized
    errors.append(f"{path}: STATUS invalid value {value!r}; allowed: {', '.join(STATUSES)}")
    # An invalid authored status is not silently promoted to an executable
    # state. Keep the raw value beside the generated quarantine status.
    return "METADATA_MISSING"


def _normalize_category(raw: Any, warnings: list[str], errors: list[str], path: str) -> str:
    value = str(raw).strip().upper() if raw not in (None, "") else ""
    if value in CATEGORIES:
        return value
    if value in CATEGORY_ALIASES:
        normalized = CATEGORY_ALIASES[value]
        warnings.append(f"{path}: CATEGORY alias {value!r} normalized to {normalized!r}")
        return normalized
    if "+" in value:
        pieces = [CATEGORY_ALIASES.get(piece.strip(), piece.strip()) for piece in value.split("+")]
        recognized = [piece for piece in pieces if piece in CATEGORIES]
        if recognized:
            warnings.append(f"{path}: compound CATEGORY {value!r} reduced to {recognized[0]!r}")
            return recognized[0]
    errors.append(f"{path}: CATEGORY invalid value {value!r}; allowed: {', '.join(CATEGORIES)}")
    return value or "UNKNOWN"


def stable_legacy_id(relative_path: str) -> str:
    return "LEGACY-" + hashlib.sha256(relative_path.encode("utf-8")).hexdigest()[:12].upper()


def normalize_metadata(raw: dict[str, Any] | None, relative_path: str) -> dict[str, Any]:
    warnings: list[str] = []
    errors: list[str] = []
    complete = raw is not None
    raw = raw or {}
    plan_id = str(raw.get("PLAN_ID") or stable_legacy_id(relative_path)).strip()
    raw_status = raw.get("STATUS")
    if not complete:
        warnings.append(f"{relative_path}: front matter missing; generated identity is {plan_id}")
        status, category = "METADATA_MISSING", "UNKNOWN"
    else:
        status = _normalize_status(raw.get("STATUS"), warnings, errors, relative_path)
        category = _normalize_category(raw.get("CATEGORY"), warnings, errors, relative_path)
    return {
        "plan_id": plan_id, "metadata_state": "COMPLETE" if complete else "METADATA_MISSING",
        "status": status, "status_raw": raw_status, "category": category, "category_raw": raw.get("CATEGORY"), "wave": raw.get("WAVE"),
        "premise_verified_at": raw.get("PREMISE_VERIFIED_AT"), "premise_sha": raw.get("PREMISE_SHA") or raw.get("VERIFIED_AT_SHA"),
        "supersedes": _as_list(raw.get("SUPERSEDES")), "superseded_by": _as_list(raw.get("SUPERSEDED_BY")),
        "owner": raw.get("OWNER"), "rails_required": _as_list(raw.get("RAILS_REQUIRED")), "metric_moved": raw.get("METRIC_MOVED"),
        "acceptance_tier": raw.get("ACCEPTANCE_TIER"), "source_authority": raw.get("SOURCE_AUTHORITY"),
        "raw": raw, "warnings": warnings, "errors": errors,
    }


def parse_plan(path: Path, repo_root: Path) -> dict[str, Any]:
    text = path.read_text(encoding="utf-8")
    relative = path.relative_to(repo_root).as_posix()
    raw, parse_errors, parse_warnings = parse_front_matter(text)
    record = normalize_metadata(raw, relative)
    record["errors"] = parse_errors + record["errors"]
    record["warnings"] = parse_warnings + record["warnings"]
    record.update({"body_sha256": sha256_bytes(text.encode("utf-8")), "file_sha256": sha256_file(path), "path": relative, "namespace": relative.split("/", 1)[0], "filename": path.name})
    return record


def _line_count(path: Path) -> int:
    try:
        return len(path.read_text(encoding="utf-8", errors="replace").splitlines())
    except OSError:
        return 0


_BASENAME_CACHE: dict[Path, dict[str, list[Path]]] = {}


def _basename_index(repo_root: Path) -> dict[str, list[Path]]:
    cached = _BASENAME_CACHE.get(repo_root)
    if cached is not None:
        return cached
    index: dict[str, list[Path]] = {}
    for candidate in repo_root.rglob("*"):
        if candidate.is_file():
            index.setdefault(candidate.name, []).append(candidate)
    for matches in index.values():
        matches.sort()
    _BASENAME_CACHE[repo_root] = index
    return index


def classify_reference(reference: str, repo_root: Path) -> ReferenceVerdict:
    original = reference.strip()
    cleaned = original.strip("`\"'()[]{}.,;: ")
    if not cleaned:
        return ReferenceVerdict(original, "EXTERNAL", "info", "empty or punctuation-only reference")
    if len(cleaned) > 240 or any(character.isspace() for character in cleaned):
        return ReferenceVerdict(original, "EXTERNAL", "info", "inline prose is not a repository path")
    if re.match(r"^(?:https?|mailto|ssh|git)://", cleaned, re.IGNORECASE):
        return ReferenceVerdict(original, "EXTERNAL", "info", "external URL")
    if cleaned.startswith("PLAN_ID:") or ("/" not in cleaned and "." not in cleaned):
        return ReferenceVerdict(original, "EXTERNAL", "info", "named plan or prose token")
    path_part = cleaned
    line_number = None
    line_match = re.match(r"^(.*?):(\d+)(?:-(\d+))?$", cleaned)
    end_line = None
    if line_match:
        path_part = line_match.group(1)
        line_number = int(line_match.group(2))
        end_line = int(line_match.group(3) or line_match.group(2))
    candidate = repo_root / path_part
    if candidate.is_file() or candidate.is_dir():
        if candidate.is_file() and line_number is not None and end_line is not None and end_line > _line_count(candidate):
            return ReferenceVerdict(original, "MISSING_LINE_RANGE", "warning", f"line {end_line} exceeds file length")
        return ReferenceVerdict(original, "OK", "none", "repository path exists")
    if any(char in path_part for char in "*?["):
        try:
            matches = list(repo_root.glob(path_part))
        except (ValueError, NotImplementedError):
            matches = []
        if matches:
            return ReferenceVerdict(original, "OK", "none", f"glob matched {len(matches)} path(s)")
        return ReferenceVerdict(original, "MISSING_PATH", "warning", "glob matched no repository path")
    name = Path(path_part).name
    matches = _basename_index(repo_root).get(name, []) if name else []
    if len(matches) == 1:
        suggestion = matches[0].relative_to(repo_root).as_posix()
        return ReferenceVerdict(original, "RENAMED_CANDIDATE", "warning", f"unique basename candidate: {suggestion}")
    if len(matches) > 1:
        return ReferenceVerdict(original, "AMBIGUOUS", "warning", f"{len(matches)} basename candidates")
    return ReferenceVerdict(original, "MISSING_PATH", "warning", "repository path does not exist")


_BACKTICK_RE = re.compile(r"`([^`]+)`")
_LINK_RE = re.compile(r"\[[^]]+\]\(([^)]+)\)")
_PATH_RE = re.compile(r"(?<![\w./-])(?:[A-Za-z0-9_.-]+/)+[A-Za-z0-9_.-]+(?:\.(?:csproj|json|md|py|sh|gd|tscn|toml|yaml|yml|cs))(?:[:-]\d+(?:-\d+)?)?")


def extract_references(text: str) -> list[str]:
    candidates = _BACKTICK_RE.findall(text) + _LINK_RE.findall(text) + _PATH_RE.findall(text)
    result: list[str] = []
    seen: set[str] = set()
    for candidate in candidates:
        candidate = candidate.strip().split("#", 1)[0]
        if candidate and candidate not in seen:
            seen.add(candidate)
            result.append(candidate)
    return sorted(result)


def analyze_references(text: str, repo_root: Path, legacy: bool) -> tuple[list[dict[str, str]], str]:
    verdicts: list[dict[str, str]] = []
    for reference in extract_references(text):
        verdict = classify_reference(reference, repo_root)
        if verdict.status != "EXTERNAL":
            verdicts.append(verdict.as_dict())
    bad = [item for item in verdicts if item["status"] not in {"OK", "EXTERNAL"}]
    if not bad:
        health = "OK"
    elif legacy:
        health = "WARN"
    elif any(item["status"] in {"MISSING_PATH", "MISSING_LINE_RANGE"} for item in bad):
        health = "MISSING"
    else:
        health = "WARN"
    return verdicts, health


def enumerate_paths(repo_root: Path, config: dict[str, Any]) -> list[Path]:
    excluded_global = set(config.get("global_excludes", []))
    output: list[Path] = []
    for namespace in config.get("plan_namespaces", []):
        root = repo_root / str(namespace["root"])
        patterns = namespace.get("include", ["**/*.md"])
        excluded = excluded_global | set(namespace.get("exclude", []))
        for pattern in patterns:
            for path in root.glob(pattern):
                if not path.is_file() or path.suffix.lower() != ".md":
                    continue
                relative_root = path.relative_to(root).as_posix()
                relative_parts = path.relative_to(repo_root).parts
                if any(part in excluded for part in relative_parts):
                    continue
                if any(relative_root == item or relative_root.startswith(item.rstrip("/") + "/") for item in namespace.get("exclude", [])):
                    continue
                output.append(path)
    return sorted(set(output), key=lambda item: item.relative_to(repo_root).as_posix())


def enumerate_docs(repo_root: Path, config: dict[str, Any]) -> list[Path]:
    docs = config.get("docs", {})
    root = repo_root / str(docs.get("root", "docs"))
    excluded = set(docs.get("exclude", []))
    result = []
    for path in root.glob(docs.get("include", "**/*.md")):
        if not path.is_file():
            continue
        relative = path.relative_to(root).as_posix()
        if any(relative == item or relative.startswith(item.rstrip("/") + "/") for item in excluded):
            continue
        result.append(path)
    return sorted(result, key=lambda item: item.relative_to(repo_root).as_posix())


def git_value(repo_root: Path, args: list[str], fallback: str = "") -> str:
    try:
        return subprocess.run(["git", *args], cwd=repo_root, check=True, capture_output=True, text=True).stdout.strip()
    except (OSError, subprocess.CalledProcessError):
        return fallback


def utc_now() -> str:
    return datetime.now(timezone.utc).replace(microsecond=0).isoformat().replace("+00:00", "Z")


def stable_file_lists(repo_root: Path, config: dict[str, Any]) -> bool:
    first = [path.relative_to(repo_root).as_posix() for path in enumerate_paths(repo_root, config)]
    second = [path.relative_to(repo_root).as_posix() for path in enumerate_paths(repo_root, config)]
    return first == second


def detect_supersedence_cycles(rows: Iterable[dict[str, Any]]) -> list[list[str]]:
    links = {str(row["plan_id"]): [str(value) for value in row.get("supersedes", [])] for row in rows}
    cycles: list[list[str]] = []
    seen: set[tuple[str, ...]] = set()
    for start in sorted(links):
        stack: list[str] = []
        visiting: set[str] = set()

        def visit(node: str) -> None:
            if node in visiting:
                cycle = stack[stack.index(node):] if node in stack else [node]
                canonical = tuple(sorted(cycle))
                if canonical not in seen:
                    seen.add(canonical)
                    cycles.append(list(canonical))
                return
            if node not in links:
                return
            visiting.add(node)
            stack.append(node)
            for parent in links[node]:
                visit(parent)
            stack.pop()
            visiting.remove(node)

        visit(start)
    return sorted(cycles)


def classify_doc(relative_path: str) -> str:
    lower = relative_path.lower()
    name = Path(relative_path).name.lower()
    if "/roadmap/" in lower or lower.startswith("roadmap/"):
        return "roadmap"
    if "/design/" in lower or lower.startswith("design/"):
        return "design"
    if "/debug/" in lower or "debug" in name:
        return "debug"
    if "/audit/" in lower or "audit" in name:
        return "audit"
    if name in {"index.md", "readme.md"} or lower.endswith("/index.md"):
        return "index"
    if "generated" in lower or lower.endswith("_generated.md"):
        return "generated"
    return "other"
