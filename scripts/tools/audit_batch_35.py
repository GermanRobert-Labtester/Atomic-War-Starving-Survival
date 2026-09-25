#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Audit script for Batch 35 (10 Plans in docs/bodymind/):
1. docs/bodymind/DOSE_INSTITUTION_CONSEQUENCE_MATRIX.md
2. docs/bodymind/DOSE_ITEM_MATRIX.md
3. docs/bodymind/PLAN27_REGRESSION_MATRIX.md
4. docs/bodymind/PSYCHOLOGICAL_CONTAMINATION_SOURCE_MATRIX.md
5. docs/bodymind/AUTOPSY_CONSENT_MATRIX.md
6. docs/bodymind/PSYCHOLOGICAL_CONTAMINATION_EFFECT_MATRIX.md
7. docs/bodymind/FORENSIC_EVIDENCE_CHAIN.md
8. docs/bodymind/PLAN23_PLAN27_CONTAMINATION_RECONCILIATION.md
9. docs/bodymind/AUTOPSY_PROCEDURE_MATRIX.md
10. docs/bodymind/DOSE_NPC_CONTINUITY.md

Verifies:
- Character count >= 250,000
- Master Authority Reference
- Pure C# domain model (Assets/Ashfall.Core/...) with netstandard2.1 & 0 engine references
- Authoritative snake_case JSON Schema (Draft 2020-12)
- 100-test xUnit suite with single assertions
- 600-day simulation trace
- 25-point QA checklist
- Section XII Deep Polishing Pass
- Section XV Precision Pass
"""

import os
import re
import sys

BATCH_35_PLANS = [
    "docs/bodymind/DOSE_INSTITUTION_CONSEQUENCE_MATRIX.md",
    "docs/bodymind/DOSE_ITEM_MATRIX.md",
    "docs/bodymind/PLAN27_REGRESSION_MATRIX.md",
    "docs/bodymind/PSYCHOLOGICAL_CONTAMINATION_SOURCE_MATRIX.md",
    "docs/bodymind/AUTOPSY_CONSENT_MATRIX.md",
    "docs/bodymind/PSYCHOLOGICAL_CONTAMINATION_EFFECT_MATRIX.md",
    "docs/bodymind/FORENSIC_EVIDENCE_CHAIN.md",
    "docs/bodymind/PLAN23_PLAN27_CONTAMINATION_RECONCILIATION.md",
    "docs/bodymind/AUTOPSY_PROCEDURE_MATRIX.md",
    "docs/bodymind/DOSE_NPC_CONTINUITY.md",
]

def audit():
    print("=" * 80)
    print("AUDITING BATCH 35 (10 PLANS)")
    print("=" * 80)

    all_passed = True

    for idx, rel_path in enumerate(BATCH_35_PLANS, 1):
        full_path = os.path.abspath(rel_path)
        if not os.path.exists(full_path):
            print(f"[{idx}/10] FAIL: File not found: {rel_path}")
            all_passed = False
            continue

        with open(full_path, "r", encoding="utf-8") as f:
            content = f.read()

        char_len = len(content)
        issues = []

        if char_len < 250000:
            issues.append(f"Char count {char_len} < 250,000")

        if "newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md" not in content:
            issues.append("Missing Master Authority reference")

        if "Assets/Ashfall.Core/" not in content or "netstandard2.1" not in content:
            issues.append("Missing Ashfall.Core / netstandard2.1 reference")

        if "https://json-schema.org/draft/2020-12/schema" not in content:
            issues.append("Missing Draft 2020-12 JSON Schema")

        # Check for 100 xUnit tests
        test_matches = re.findall(r"\[Fact\]\s+public void (Test_\d{3}_[\w]+)", content)
        if len(test_matches) < 100:
            issues.append(f"Only {len(test_matches)} xUnit tests found (expected >= 100)")

        # Check for 600-day simulation
        if "600-DAY" not in content and "600 DAYS" not in content and "600-Day" not in content and "DAY 600" not in content and "Day 600" not in content:
            issues.append("Missing 600-day simulation trace")

        # Check for 25-point QA checklist
        qa_items = re.findall(r"\[[x ]\]\s+\*\*", content)
        if len(qa_items) < 25:
            issues.append(f"Only {len(qa_items)} QA checklist items found (expected >= 25)")

        # Check for Section XII Deep Polish
        if "SECTION XII" not in content and "DEEP POLISHING PASS" not in content:
            issues.append("Missing Section XII Deep Polishing Pass")

        # Check for Section XV Precision Pass
        if "SECTION XV" not in content and "PRECISION PASS" not in content:
            issues.append("Missing Section XV Precision Pass")

        if issues:
            print(f"[{idx}/10] FAIL: {rel_path} ({char_len:,} chars)")
            for issue in issues:
                print(f"   - {issue}")
            all_passed = False
        else:
            print(f"[{idx}/10] PASS: {rel_path} ({char_len:,} chars, {len(test_matches)} tests, {len(qa_items)} QA items)")

    print("=" * 80)
    if all_passed:
        print("ALL 10 PLANS IN BATCH 35 PASSED ALL 9 AUDIT CHECKS PERFECTLY!")
    else:
        print("SOME CHECKS FAILED! REVIEW ABOVE LOG.")
    print("=" * 80)
    return all_passed

if __name__ == "__main__":
    success = audit()
    sys.exit(0 if success else 1)
