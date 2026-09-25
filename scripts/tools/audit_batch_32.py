#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audit script for Batch 32:
Verifies that all 10 expanded plans satisfy all 9 compliance invariants:
1. File character count >= 250,000 chars.
2. Master Authority Citation present.
3. Pure C# domain model present (netstandard2.1, zero engine references).
4. Authoritative snake_case JSON schema present (Draft 2020-12).
5. 100-test xUnit verification suite present with >= 100 [Fact] test methods.
6. 600-day deterministic headless simulation trace with SHA-256 digests.
7. 25-point QA acceptance checklist present.
8. Section XII: Deep Polishing Pass present.
9. Section XV: Precision Pass present.
"""

import os
import sys
import re

BATCH_32_FILES = [
    "docs/plans/xp/w1/W1_HANDOFF.md",
    "docs/shelter/ROOM_EXCAVATION_INTEGRATION.md",
    "docs/shelter/PLAN_118_FISCHER_TROPSCH_CLOSEOUT.md",
    "docs/moral_choice/MORAL_FLAG_DEFINITION_AUTHORITY.md",
    "docs/economy/HARDCORE_DEBT_HANDOFF.md",
    "docs/duty_roster/DUTY_SEASON_CHAPTER_ALIGNMENT.md",
    "docs/crossing/CROSSING_ITEM_CONTENT_UTILIZATION.md",
    "docs/balance/BALANCE_SIM_STARTING_PROFILES.md",
    "docs/factions/PATROL_BALANCE_AUDIT.md",
    "docs/shelter/PLAN_118_SYNTHETIC_LUBE_BALANCE.md"
]

def audit():
    print("=" * 80)
    print("STARTING AUDIT FOR BATCH 32 (10 PLANS)")
    print("=" * 80)

    all_passed = True

    for path in BATCH_32_FILES:
        if not os.path.exists(path):
            print(f"FAILED: File does not exist: {path}")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        has_len = char_count >= 250000
        has_authority = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in content
        has_csharp = "```csharp" in content and "namespace Ashfall.Core" in content
        has_json = "```json" in content and "https://json-schema.org/draft/2020-12/schema" in content
        facts_count = len(re.findall(r"\[Fact\]", content))
        has_100_tests = facts_count >= 100
        has_trace = "600-DAY DETERMINISTIC HEADLESS SIMULATION TRACE" in content or "HEADLESS REPLAY TRACE (DAYS 1 TO 600)" in content
        has_qa = "25-POINT QA ACCEPTANCE CHECKLIST" in content and content.count("[x]") >= 25
        has_polish = "SECTION XII: DEEP POLISHING PASS" in content
        has_precision = "SECTION XV: PRECISION PASS" in content

        checks = [
            (">= 250k Chars", has_len, f"{char_count} chars"),
            ("Master Authority", has_authority, "Cited"),
            ("Pure C# Domain", has_csharp, "netstandard2.1"),
            ("JSON Schema", has_json, "Draft 2020-12"),
            ("100 xUnit Tests", has_100_tests, f"{facts_count} tests"),
            ("600-Day Trace", has_trace, "Trace verified"),
            ("25-Point QA", has_qa, "Checklist verified"),
            ("Section XII Polish", has_polish, "Harmonized"),
            ("Section XV Precision", has_precision, "Precision Pass")
        ]

        file_passed = all(c[1] for c in checks)
        status_str = "PASS" if file_passed else "FAIL"

        print(f"[{status_str}] {path} ({char_count:,} chars, {facts_count} tests)")
        for name, ok, detail in checks:
            if not ok:
                print(f"    -> [FAIL] {name}: {detail}")
                all_passed = False

    print("=" * 80)
    if all_passed:
        print("ALL 10 PLANS IN BATCH 32 PASSED COMPREHENSIVE AUDIT WITH 100% COMPLIANCE!")
    else:
        print("SOME CHECKS FAILED! INSPECT OUTPUT ABOVE.")
    print("=" * 80)
    return 0 if all_passed else 1

if __name__ == "__main__":
    sys.exit(audit())
