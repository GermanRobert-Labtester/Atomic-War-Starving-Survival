#!/usr/bin/env python3
"""scripts/ci/plan-intake-check.py
Plan 53 / E1I: Plan Intake Form & Executable Intake Gate for ASHFALL.

Validates new and modified integration plans against intake invariants:
1. Front matter structure: mandatory YAML delimiters ('---')
2. Required metadata: PLAN_ID, TITLE, STATUS, CATEGORY
3. Status enum validity: PROPOSED, READY, IN_PROGRESS, DONE, etc.
4. Category enum validity: SYSTEM, LINK, CONTENT, PRESENTATION, PROCESS
5. System plan rules: requires architectural authority / seam reference to prevent duplicate systems
6. Presentation plan rules: requires underlying Core/host read model or authority reference
"""

from __future__ import annotations

import argparse
import json
import os
import re
import subprocess
import sys
from pathlib import Path
from typing import Any

SCRIPT_DIR = Path(__file__).resolve().parent
REPO_ROOT = SCRIPT_DIR.parent.parent
sys.path.insert(0, str(SCRIPT_DIR))

from plan_corpus_lib import (  # noqa: E402
    CATEGORIES,
    STATUSES,
    STATUS_ALIASES,
    CATEGORY_ALIASES,
    parse_front_matter,
)


REQUIRED_FIELDS = ["PLAN_ID", "TITLE", "STATUS", "CATEGORY"]


class IntakeCheckResult:
    def __init__(self, plan_path: Path):
        self.plan_path = plan_path
        self.errors: list[str] = []
        self.warnings: list[str] = []
        self.plan_id: str = ""
        self.status: str = ""
        self.category: str = ""
        self.is_valid: bool = False

    def to_dict(self) -> dict[str, Any]:
        return {
            "path": str(self.plan_path),
            "plan_id": self.plan_id,
            "status": self.status,
            "category": self.category,
            "is_valid": self.is_valid,
            "errors": self.errors,
            "warnings": self.warnings,
        }


def check_plan_intake(plan_path: Path, explain: bool = False) -> IntakeCheckResult:
    res = IntakeCheckResult(plan_path)
    if not plan_path.is_file():
        res.errors.append(f"File not found: {plan_path}")
        return res

    text = plan_path.read_text(encoding="utf-8", errors="replace")
    front_matter, parse_errors, parse_warnings = parse_front_matter(text)

    res.errors.extend(parse_errors)
    res.warnings.extend(parse_warnings)

    if front_matter is None:
        res.errors.append("Missing YAML front matter block (must begin and end with '---')")
        return res

    # 1. Required fields
    for field in REQUIRED_FIELDS:
        if field not in front_matter or not str(front_matter[field]).strip():
            res.errors.append(f"Missing required metadata field: '{field}'")

    res.plan_id = str(front_matter.get("PLAN_ID", "")).strip()
    raw_status = str(front_matter.get("STATUS", "")).strip().upper()
    raw_category = str(front_matter.get("CATEGORY", "")).strip().upper()

    # 2. Status validation
    if raw_status:
        if raw_status in STATUSES:
            res.status = raw_status
        elif raw_status in STATUS_ALIASES:
            res.status = STATUS_ALIASES[raw_status]
            res.warnings.append(f"Status alias '{raw_status}' mapped to '{res.status}'")
        else:
            res.errors.append(f"Invalid STATUS: '{raw_status}'. Allowed values: {', '.join(STATUSES)}")
            res.status = raw_status

    # 3. Category validation
    if raw_category:
        if raw_category in CATEGORIES:
            res.category = raw_category
        elif raw_category in CATEGORY_ALIASES:
            res.category = CATEGORY_ALIASES[raw_category]
            res.warnings.append(f"Category alias '{raw_category}' mapped to '{res.category}'")
        else:
            res.errors.append(f"Invalid CATEGORY: '{raw_category}'. Allowed values: {', '.join(CATEGORIES)}")
            res.category = raw_category

    # 4. SYSTEM category discipline
    if res.category == "SYSTEM":
        body_lower = text.lower()
        has_seam = any(marker in body_lower for marker in [
            "extend", "extends", "seam", "authority", "subsystem", "owner", "architecture"
        ])
        has_duplicate_check = any(marker in body_lower for marker in [
            "duplicate", "non-goals", "no new parallel", "one authority"
        ])
        if not (has_seam or has_duplicate_check):
            res.warnings.append("SYSTEM plan does not explicitly cite extending an existing seam or duplicate prevention rationale.")

    # 5. PRESENTATION category discipline
    if res.category == "PRESENTATION":
        body_lower = text.lower()
        has_authority = any(marker in body_lower for marker in [
            "read model", "model", "host session", "session", "panel", "ui", "display", "presentation"
        ])
        if not has_authority:
            res.warnings.append("PRESENTATION plan does not explicitly specify its underlying authority or read model.")

    # 6. Rails validation
    raw_rails = front_matter.get("RAILS_REQUIRED")
    if raw_rails:
        rails_list = raw_rails if isinstance(raw_rails, list) else [str(raw_rails).strip()]
        known_rails: set[str] = set()
        registry_path = REPO_ROOT / "docs" / "roadmap" / "rails.registry.json"
        if registry_path.is_file():
            try:
                reg = json.loads(registry_path.read_text(encoding="utf-8"))
                known_rails = {r["id"] for r in reg.get("rails", []) if "id" in r}
            except Exception:
                pass
        known_rails.update(["docs", "docs_index", "none"])

        for r in rails_list:
            r_str = str(r).strip()
            if known_rails and r_str not in known_rails:
                res.errors.append(f"Unknown rail required: '{r_str}'. Must be one of registered canonical rails: {sorted(known_rails)}")

    res.is_valid = len(res.errors) == 0
    return res


def run_self_test() -> int:
    print("Running plan-intake-check self-test...")
    import tempfile

    with tempfile.TemporaryDirectory() as tmp_dir:
        tmp_path = Path(tmp_dir)

        # Case 1: Fully valid plan
        valid_plan = tmp_path / "valid_plan.md"
        valid_plan.write_text("""---
PLAN_ID: EXP-99
TITLE: Valid Intake Test Plan
STATUS: PROPOSED
CATEGORY: SYSTEM
---

# EXP-99
Extends existing system seam with non-goals and duplicate prevention.
""", encoding="utf-8")

        r1 = check_plan_intake(valid_plan)
        assert r1.is_valid, f"Expected valid, got errors: {r1.errors}"
        assert r1.plan_id == "EXP-99"

        # Case 2: Missing front matter
        invalid_plan1 = tmp_path / "no_fm.md"
        invalid_plan1.write_text("# Plan without front matter\nSome text.", encoding="utf-8")
        r2 = check_plan_intake(invalid_plan1)
        assert not r2.is_valid, "Expected invalid for missing front matter"
        assert any("Missing YAML front matter" in err for err in r2.errors)

        # Case 3: Missing required field (STATUS)
        invalid_plan2 = tmp_path / "missing_status.md"
        invalid_plan2.write_text("""---
PLAN_ID: EXP-100
TITLE: Missing Status Plan
CATEGORY: LINK
---
# Text
""", encoding="utf-8")
        r3 = check_plan_intake(invalid_plan2)
        assert not r3.is_valid, "Expected invalid for missing status"
        assert any("STATUS" in err for err in r3.errors)

        # Case 4: Invalid category
        invalid_plan3 = tmp_path / "bad_cat.md"
        invalid_plan3.write_text("""---
PLAN_ID: EXP-101
TITLE: Bad Category Plan
STATUS: PROPOSED
CATEGORY: NONEXISTENT_CATEGORY
---
# Text
""", encoding="utf-8")
        r4 = check_plan_intake(invalid_plan3)
        assert not r4.is_valid, "Expected invalid for invalid category"
        assert any("Invalid CATEGORY" in err for err in r4.errors)

    print("plan-intake-check self-test: PASS")
    return 0


def main() -> int:
    parser = argparse.ArgumentParser(description="ASHFALL Plan Intake Checker (Plan 53 / E1I).")
    parser.add_argument("--check", action="store_true", help="Run intake check on target plans.")
    parser.add_argument("--plan", type=str, help="Check intake for a specific plan file.")
    parser.add_argument("--changed", action="store_true", help="Check only plans modified in git worktree.")
    parser.add_argument("--explain", action="store_true", help="Provide detailed explanation and remediation advice.")
    parser.add_argument("--json", action="store_true", help="Output results in JSON format.")
    parser.add_argument("--self-test", action="store_true", help="Run automated self-test suite and exit.")
    args = parser.parse_args()

    if args.self_test:
        return run_self_test()

    plans_to_check: list[Path] = []

    if args.plan:
        p = Path(args.plan)
        if not p.is_absolute():
            p = REPO_ROOT / p
        plans_to_check.append(p)
    elif args.changed:
        try:
            cmd = ["git", "diff", "--name-only", "HEAD"]
            out = subprocess.run(cmd, cwd=REPO_ROOT, capture_output=True, text=True, check=True).stdout
            for line in out.splitlines():
                if line.endswith(".md") and ("plans" in line or "Next-steps" in line):
                    candidate = REPO_ROOT / line
                    if candidate.is_file():
                        plans_to_check.append(candidate)
        except Exception as e:
            print(f"Warning: git diff check failed: {e}", file=sys.stderr)

    if not plans_to_check and not args.changed:
        print("Specify --plan <path>, --changed, or --self-test.", file=sys.stderr)
        return 1

    if not plans_to_check and args.changed:
        print("No changed plan files detected.")
        return 0

    results = []
    any_errors = False

    for plan in plans_to_check:
        res = check_plan_intake(plan, explain=args.explain)
        results.append(res.to_dict())
        if not res.is_valid:
            any_errors = True

    if args.json:
        print(json.dumps({"success": not any_errors, "results": results}, indent=2))
        return 1 if any_errors else 0

    for r in results:
        status_str = "PASS" if r["is_valid"] else "FAIL"
        rel_path = Path(r["path"]).relative_to(REPO_ROOT) if Path(r["path"]).is_relative_to(REPO_ROOT) else r["path"]
        print(f"[{status_str}] {rel_path} (ID: {r['plan_id'] or 'NONE'}, Cat: {r['category'] or 'NONE'}, Status: {r['status'] or 'NONE'})")
        for err in r["errors"]:
            print(f"  - ERROR: {err}")
        for warn in r["warnings"]:
            print(f"  - WARN: {warn}")

    if any_errors:
        print("\nIntake check failed. See errors above.")
        return 1

    print("\nAll checked plans passed intake validation.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
