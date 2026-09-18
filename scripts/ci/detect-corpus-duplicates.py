#!/usr/bin/env python3
"""Report exact C1/C2 integration-plan identity collisions.

The report is intentionally advisory: a matching historical Plan ID or title is
only a duplicate candidate. A human must compare scope and record the final
classification in docs/plans/UNCLAIMED_CORPUS_CENSUS.md before either plan is
superseded.

Usage:
  python3 scripts/ci/detect-corpus-duplicates.py
  python3 scripts/ci/detect-corpus-duplicates.py --check
"""

from __future__ import annotations

import argparse
import re
import sys
from collections import defaultdict
from dataclasses import dataclass
from pathlib import Path


REPO_ROOT = Path(__file__).resolve().parents[2]
PLAN_ROOT = REPO_ROOT / "C-integration-plans"
CENSUS = REPO_ROOT / "docs" / "plans" / "UNCLAIMED_CORPUS_CENSUS.md"
SOURCE_PLAN_RE = re.compile(r"\bPlan\s+(\d+[A-Za-z]?)\b", re.IGNORECASE)
TITLE_RE = re.compile(r"^#\s+(?:C[12][^—]*—\s*)?(.+?)\s*$")


@dataclass(frozen=True)
class PlanHeader:
    corpus_key: str
    path: Path
    title: str
    source_plan: str | None


def normalized(value: str) -> str:
    return re.sub(r"[^a-z0-9]+", " ", value.lower()).strip()


def read_header(path: Path) -> PlanHeader:
    text = path.read_text(encoding="utf-8")
    header = text[:6000]
    title = path.stem
    for line in header.splitlines():
        match = TITLE_RE.match(line)
        if match:
            title = match.group(1)
            break

    source_match = re.search(r"^>\s*\*\*Source(?: (?:baseline|scope))?:\*\*\s*(.+)$", header, re.MULTILINE)
    source_plan = None
    if source_match:
        plan_match = SOURCE_PLAN_RE.search(source_match.group(1))
        if plan_match:
            source_plan = plan_match.group(1).lower()

    corpus_match = re.match(r"C([12])_planintegration\[(\d+)\]", path.stem, re.IGNORECASE)
    if not corpus_match:
        raise ValueError(f"Unexpected corpus filename: {path.name}")
    return PlanHeader(f"C{corpus_match.group(1)}[{corpus_match.group(2)}]", path, title, source_plan)


def candidates(headers: list[PlanHeader]) -> list[tuple[str, list[PlanHeader]]]:
    by_source: dict[str, list[PlanHeader]] = defaultdict(list)
    by_title: dict[str, list[PlanHeader]] = defaultdict(list)
    for header in headers:
        if header.source_plan:
            by_source[header.source_plan].append(header)
        by_title[normalized(header.title)].append(header)

    found: dict[tuple[str, tuple[str, ...]], list[PlanHeader]] = {}
    for key, rows in by_source.items():
        if {row.corpus_key[:2] for row in rows} == {"C1", "C2"}:
            found[(f"source Plan {key}", tuple(sorted(row.corpus_key for row in rows)))] = rows
    for key, rows in by_title.items():
        if {row.corpus_key[:2] for row in rows} == {"C1", "C2"}:
            found[(f"normalized title {key!r}", tuple(sorted(row.corpus_key for row in rows)))] = rows
    return sorted(found.items(), key=lambda item: item[0])


def is_reconciled(rows: list[PlanHeader]) -> bool:
    census = CENSUS.read_text(encoding="utf-8")
    return all(row.corpus_key in census for row in rows) and "Duplicate reconciliation registry" in census


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--check", action="store_true", help="fail when a reported candidate lacks a census reconciliation registry")
    args = parser.parse_args()

    headers = [read_header(path) for path in sorted(PLAN_ROOT.glob("C[12]_planintegration[[]*.md"))]
    found = candidates(headers)
    print("# C1/C2 corpus duplicate candidates")
    print()
    if not found:
        print("No exact cross-corpus source-Plan-ID or normalized-title candidates.")
        return 0

    print("| Candidate basis | Corpus rows | Files | Manual disposition required |")
    print("|---|---|---|---|")
    missing_reconciliation = False
    for (basis, _), rows in found:
        corpus_rows = ", ".join(f"`{row.corpus_key}`" for row in rows)
        files = ", ".join(f"`{row.path.relative_to(REPO_ROOT)}`" for row in rows)
        reconciled = is_reconciled(rows)
        state = "registry present; inspect semantic disposition" if reconciled else "MISSING census reconciliation"
        print(f"| {basis} | {corpus_rows} | {files} | {state} |")
        missing_reconciliation |= not reconciled

    if args.check and missing_reconciliation:
        print("FAIL: duplicate candidate has no census reconciliation registry.", file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
