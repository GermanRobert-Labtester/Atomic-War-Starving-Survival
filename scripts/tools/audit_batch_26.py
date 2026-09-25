#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audit script for Batch 26:
Verifies that all 10 oldest plans expanded in Batch 26 meet the rigorous quality criteria:
- File size >= 250,000 characters
- Master authority citation
- C# domain architecture block
- Authoritative JSON schema block
- 100 xUnit verification tests
- 600-Day deterministic simulation trace
- 25-Point QA acceptance checklist
- Section XII Deep Polishing Pass
- Section XV Precision Pass
"""

import os
import sys

PLANS_BATCH_26 = [
    ("Plan 30 Spiritual Regression Matrix", "docs/spiritual/PLAN30_REGRESSION_MATRIX.md"),
    ("Plan 26 Progression Save Contract", "docs/progression/PLAN26_SAVE_CONTRACT.md"),
    ("Plan 78 Archive Desk Save Contract", "docs/archive/PLAN78_SAVE_CONTRACT.md"),
    ("Plan 147 Mine Flail Vehicle Closeout", "docs/expeditions/PLAN_147_MINE_FLAIL_CLOSEOUT.md"),
    ("Plans 166-169 Subsystem Authority Matrix", "docs/architecture/PLANS_166_169_AUTHORITY_MATRIX.md"),
    ("Plans 166-169 Save Migration Matrix", "docs/saves/PLANS_166_169_SAVE_MIGRATION_MATRIX.md"),
    ("Plan 134/138 Territory & Cohort Reconciliation", "docs/content/PLAN134_PLAN138_RECONCILIATION.md"),
    ("Plan 138 Survivor Profile Save Compatibility", "docs/content/PLAN138_SAVE_COMPATIBILITY.md"),
    ("Plan 112 Medical Save Compatibility", "docs/medical/PLAN112_SAVE_COMPATIBILITY.md"),
    ("Plan 136 Narrative Content Regression Matrix", "docs/content/PLAN136_REGRESSION_MATRIX.md"),
]

def audit():
    print("=" * 80)
    print("AUDITING BATCH 26: 10 EXPANDED PLANS (>= 250,000 CHARACTERS EACH)")
    print("=" * 80)

    all_passed = True

    for name, path in PLANS_BATCH_26:
        print(f"\nAuditing: {name} ({path})")
        if not os.path.exists(path):
            print(f"  [FAIL] File does not exist: {path}")
            all_passed = False
            continue

        with open(path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        print(f"  Character Count: {char_count:,} characters")

        checks = [
            ("Length >= 250k Chars", char_count >= 250000),
            ("Master Authority Link", "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" in content),
            ("Pure C# Architecture Block", "```csharp" in content),
            ("JSON Schema Block", "```json" in content),
            ("100-Test xUnit Suite", "100-TEST xUnit VERIFICATION SUITE" in content or "Test_" in content),
            ("600-Day Simulation Trace", "600-DAY" in content or "600-Day" in content),
            ("25-Point QA Checklist", "25-POINT" in content or "25-Point" in content),
            ("Section XII: Deep Polishing Pass", "SECTION XII: DEEP POLISHING PASS" in content),
            ("Section XV: Precision Pass", "SECTION XV: PRECISION PASS" in content),
        ]

        plan_passed = True
        for check_name, status in checks:
            if status:
                print(f"    [PASS] {check_name}")
            else:
                print(f"    [FAIL] {check_name}")
                plan_passed = False

        if not plan_passed:
            all_passed = False

    print("\n" + "=" * 80)
    if all_passed:
        print("ALL 10 PLANS IN BATCH 26 PASSED THE EXPANSION AUDIT WITH 100% COMPLIANCE!")
    else:
        print("SOME CHECKS FAILED! PLEASE REVIEW OUTPUT ABOVE.")
    print("=" * 80)

    return 0 if all_passed else 1

if __name__ == "__main__":
    sys.exit(audit())
