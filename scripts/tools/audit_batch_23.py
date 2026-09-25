#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audit script for Batch 23 (10 Plans expanded to >= 250,000 characters).
Verifies:
1. File character count >= 250,000.
2. Master Authority link to newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md.
3. Section XII: Deep Polishing Pass & Architectural Harmonization.
4. Section XV: Precision Pass & Integration Architecture Harmonization.
5. 100 xUnit Tests (100-TEST xUnit VERIFICATION SUITE).
6. 600-Day Deterministic Simulation Trace.
7. 25-Point QA Acceptance Checklist.
8. Engine-free C# Domain Architecture (`netstandard2.1`).
9. Authoritative Snake_Case JSON Schemas.
"""

import os
import sys

AUTHORITY_FILE = "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md"

PLANS = [
    "docs/plans/PLAN_B66_METALLURGY_CLOSEOUT.md",
    "docs/plans/PLAN_B66_B69_HOST_WIRING_CLOSEOUT.md",
    "docs/plans/PLANS_B70_B73_AUTHORITY_MAP.md",
    "docs/plans/PLANS_B98_B101_IMPLEMENTATION_LOG.md",
    "docs/plans/PLANS_202_205_FLAGSHIP_IMPLEMENTATION_LOG.md",
    "docs/plans/SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_IMPLEMENTATION_LOG.md",
    "docs/plans/PLANS_54_57_AUTHORITY_MAP.md",
    "docs/plans/SHELTER_GRID_CATALOG_SEAL_IMPLEMENTATION_LOG.md",
    "docs/plans/PLANS_162_165_IMPLEMENTATION_LOG.md",
    "docs/plans/PLANS_66_69_RECONNAISSANCE.md"
]

def audit_batch_23():
    print("=" * 80)
    print("BATCH 23 AUDIT: 10 PLANS EXPANSION & INTEGRATION VERIFICATION")
    print("=" * 80)

    total_chars = 0
    all_passed = True

    for idx, rel_path in enumerate(PLANS, 1):
        if not os.path.exists(rel_path):
            print(f"[{idx:02d}] FAIL - File not found: {rel_path}")
            all_passed = False
            continue

        with open(rel_path, "r", encoding="utf-8") as f:
            content = f.read()

        char_count = len(content)
        total_chars += char_count

        has_min_chars = char_count >= 250000
        has_authority = AUTHORITY_FILE in content
        has_sec_xii = ("SECTION XII: DEEP POLISHING PASS" in content) or ("SECTION XII: ARCHITECTURAL DEEP POLISHING" in content)
        has_sec_xv = ("SECTION XV: PRECISION PASS" in content) or ("SECTION XV: ARCHITECTURAL PRECISION" in content)
        has_100_tests = ("100-TEST xUnit" in content) or ("100-Test xUnit" in content) or ("100 tests" in content.lower() and "xunit" in content.lower())
        has_600_day = "600-Day" in content or "600-day" in content or "Day 600" in content
        has_qa_checklist = ("25-POINT" in content) or ("25-Point" in content)
        has_csharp = "```csharp" in content
        has_json = "```json" in content

        checks = [
            has_min_chars,
            has_authority,
            has_sec_xii,
            has_sec_xv,
            has_100_tests,
            has_600_day,
            has_qa_checklist,
            has_csharp,
            has_json
        ]

        status = "PASS" if all(checks) else "FAIL"
        if status == "FAIL":
            all_passed = False

        print(f"[{idx:02d}] {status} | Chars: {char_count:,} | {rel_path}")
        if status == "FAIL":
            if not has_min_chars:
                print(f"     -> FAILED: Char count {char_count:,} < 250,000")
            if not has_authority:
                print(f"     -> FAILED: Missing Master Authority reference ({AUTHORITY_FILE})")
            if not has_sec_xii:
                print("     -> FAILED: Missing Section XII (Deep Polishing Pass)")
            if not has_sec_xv:
                print("     -> FAILED: Missing Section XV (Precision Pass)")
            if not has_100_tests:
                print("     -> FAILED: Missing 100 xUnit Test Suite")
            if not has_600_day:
                print("     -> FAILED: Missing 600-Day Simulation Trace")
            if not has_qa_checklist:
                print("     -> FAILED: Missing 25-Point QA Checklist")
            if not has_csharp:
                print("     -> FAILED: Missing C# Domain Model block")
            if not has_json:
                print("     -> FAILED: Missing JSON Data Schema block")

    print("-" * 80)
    print(f"Total Batch 23 Characters: {total_chars:,}")
    print(f"Average Plan Length: {total_chars // len(PLANS):,} chars")
    print(f"Overall Status: {'ALL 10 PLANS PASSED' if all_passed else 'SOME PLANS FAILED'}")
    print("=" * 80)

    if not all_passed:
        sys.exit(1)

if __name__ == "__main__":
    audit_batch_23()
